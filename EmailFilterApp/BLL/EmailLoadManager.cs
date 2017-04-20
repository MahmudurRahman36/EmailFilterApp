using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmailFilterApp.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Excel = Microsoft.Office.Interop.Excel;
using Message = Google.Apis.Gmail.v1.Data.Message;

namespace EmailFilterApp.BLL
{
    class EmailLoadManager
    {
        public GmailService GetService(string userName, string password)
        {
            UserCredential credential;
            string[] tokens = userName.Split('@');
            string[] Scopes = { GmailService.Scope.GmailReadonly };
            const string applicationName = "Gmail API .NET Quickstart";
            //using (var stream = new FileStream("client_secret_for_" + tokens[0] + ".json", FileMode.Open, FileAccess.Read))
            using (var stream = new FileStream("client_secret_for_mrkolince.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart-for-" + tokens[0] + ".json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes,
                    "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
            }
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName,
            });
            return service;
        }

        public static Message GetMessage(GmailService service, String userId, String messageId)
        {
            try
            {
                return service.Users.Messages.Get(userId, messageId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            return null;
        }
        public List<Message> ListMessages(GmailService service, String userId, String query)
        {
            List<Message> result = new List<Message>();
            UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
            request.Q = query;
            do
            {
                try
                {
                    ListMessagesResponse response = request.Execute();
                    result.AddRange(response.Messages);
                    request.PageToken = response.NextPageToken;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
            } while (!String.IsNullOrEmpty(request.PageToken));

            return result;
        }

        

        public List<Message> GetFullMessages(GmailService service, string userName, List<Message> allMessages)
        {
            List<Message> newMessages = new List<Message>();

            foreach (var message1 in allMessages)
            {
                Message message2 = GetMessage(service, userName, message1.Id);
                newMessages.Add(message2);
            }
            return newMessages;
        }
        public HashSet<string> GetUniqueHeaders(List<Message> allMessages)
        {


            List<string> headerList = new List<string>();

            foreach (var body in allMessages)
            {
                IList<MessagePartHeader> headerPart = body.Payload.Headers;
                foreach (MessagePartHeader messagePartHeader in headerPart)
                {
                    if (messagePartHeader.Name.Equals("Subject"))
                    {
                        headerList.Add(messagePartHeader.Value);
                    }
                }
            }
            var unique_headers = new HashSet<string>(headerList);
            return unique_headers;
        }

        public List<string> GetHeaderInList(HashSet<string> uniqueHeaders)
        {
            List<string> list = new List<string>();
            foreach (string header in uniqueHeaders)
            {
                list.Add(header);
            }
            return list;
        }

        public List<Message> GetSelectedMessages(List<Message> allMessages, string header)
        {
            List<Message> selecetdMessages = new List<Message>();
            foreach (Message message in allMessages)
            {
                IList<MessagePartHeader> headerPart = message.Payload.Headers;
                foreach (MessagePartHeader messagePartHeader in headerPart)
                {
                    if (messagePartHeader.Name.Equals("Subject") && messagePartHeader.Value.Equals(header))
                    {
                        selecetdMessages.Add(message);
                    }
                }
            }
            return selecetdMessages;
        }

        public List<Email> GetMessagesInFormatedWay(List<Message> allMessages)
        {
            List<Email> messages = new List<Email>();
            foreach (Message message in allMessages)
            {
                Email email = new Email();
                email.Body = message.Snippet;
                string[] tokens = email.Body.Split(':');
                email.ApplicantName = tokens[1].Substring(1, tokens[1].Length - 14);
                email.From = tokens[2].Substring(1, tokens[2].Length - 16);
                email.ContactNo = tokens[3].Substring(1, tokens[3].Length - 9);

                IList<MessagePartHeader> headerPart = message.Payload.Headers;
                foreach (MessagePartHeader messagePartHeader in headerPart)
                {
                    if (messagePartHeader.Name.Equals("Date"))
                    {
                        email.DateTime = Convert.ToDateTime(messagePartHeader.Value);
                    }
                }
                messages.Add(email);
            }
            messages = messages.OrderByDescending(e => e.DateTime).ToList();
            return messages;
        }

        public List<Message> GetOnlyUnreadMessages(List<Message> messages)
        {
            List<Message> unreadMessages=new List<Message>();
            foreach (Message message in messages)
            {
                IList<string> labels = message.LabelIds;
                foreach (string label in labels)
                {
                    if (label.ToLower().Equals("unread".ToLower()))
                    {
                        unreadMessages.Add(message);
                    }
                }
            }
            return unreadMessages;
        }
        public List<Message> GetOnlyReadMessages(List<Message> messages)
        {
            List<Message> readMessages = new List<Message>();
            bool isRead = true;
            foreach (Message message in messages)
            {
                IList<string> labels = message.LabelIds;
                isRead = true;
                foreach (string label in labels)
                {
                    if (label.ToLower().Equals("unread".ToLower()))
                    {
                        isRead = false;
                    }
                }
                if (isRead)
                {
                    readMessages.Add(message);
                }
            }
            return readMessages;
        }

        public string GetMessageAttacthment(GmailService service, String userId,List<Message> messages )
        {
            string attachmentMessage = "";
            foreach (Message message in messages)
            {
                attachmentMessage+=GetAttachments(service, userId, message.Id, "d:\\",message);
            }
            return attachmentMessage;
        }
        public string GetAttachments(GmailService service, String userId, String messageId, String outputDir, Message message)
        {
            string attachmentMessage = "";
            try
            {
                IList<MessagePart> parts = message.Payload.Parts;
                foreach (MessagePart part in parts)
                {
                    if (!String.IsNullOrEmpty(part.Filename))
                    {
                        String attId = part.Body.AttachmentId;
                        MessagePartBody attachPart = service.Users.Messages.Attachments.Get(userId, messageId, attId).Execute();

                        String attachData = attachPart.Data.Replace('-', '+');
                        attachData = attachData.Replace('_', '/');

                        byte[] data = Convert.FromBase64String(attachData);

                        string[] fileType = part.Filename.Split('.');
                        string[] fileName = userId.Split('@');
                        File.WriteAllBytes(Path.Combine(outputDir, fileName[0] + "." + fileType[fileType.Length - 1]), data);
                        attachmentMessage += userId + "'s " + "file has been saved as " + outputDir + fileName[0] + "." +
                                             fileType[fileType.Length - 1] + "\n";
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return attachmentMessage;
        }
        public string ProduceExcelFile(List<Email> messages,string status)
        {
            string excelMessage = "";
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                //MessageBox.Show("Excel is not properly installed!!");
                return excelMessage;
            }
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "SL No.";
            xlWorkSheet.Cells[1, 2] = "Applicant Name";
            xlWorkSheet.Cells[1, 3] = "Contact No";
            xlWorkSheet.Cells[1, 4] = "Email";

            int i = 2;
            int slNo = 1;
            foreach (Email message in messages)
            {
                xlWorkSheet.Cells[i, 1] = slNo.ToString();
                xlWorkSheet.Cells[i, 2] = message.ApplicantName;
                xlWorkSheet.Cells[i, 3] = message.ContactNo;
                xlWorkSheet.Cells[i, 4] = message.From;
                i++;
                slNo++;
            }

            xlWorkBook.SaveAs("d:\\ApplicantInformationOf"+status+".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

           return "Excel file created , you can find the file d:\\ApplicantInformationOf" + status + ".xls";
        }
    }
}
