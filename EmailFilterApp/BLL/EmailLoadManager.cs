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
        public GmailService GetService(string userName)
        {
            UserCredential credential;
            string[] tokens = userName.Split('@');
            string[] Scopes = { GmailService.Scope.GmailReadonly };
            const string applicationName = "Gmail API .NET Quickstart";
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

        public Message GetMessage(GmailService service, String userId, String messageId)
        {
            try
            {
                return service.Users.Messages.Get(userId, messageId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine(@"An error occurred: " + e.Message);
            }

            return null;
        }
        public List<Message> ListMessages(GmailService service, String userId, String query)
        {
            var result = new List<Message>();
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
                    string[] tokens = e.Message.Split('.');
                    if (tokens[0].Equals("An error occurred while sending the request"))
                    {
                        MessageBox.Show(@"Please Check your internet Connection. Could not seed request");
                    }
                    else if (tokens[0].Equals("Google"))
                    {
                        DeleteFile(userId);
                        MessageBox.Show(@"The provided email address and sign in email address in browser are not same. Please sign in with provided email address. and Try again");
                    }
                    else
                    {
                        DeleteFile(userId);
                        MessageBox.Show(@"Some problem occured while connecting with requested account please try again");
                    }
                }
            } while (!String.IsNullOrEmpty(request.PageToken));

            return result;
        }

        public bool DeleteFile(string userName)
        {
            string[] tokens = userName.Split('@');
            string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            credPath = Path.Combine(credPath, ".credentials\\gmail-dotnet-quickstart-for-" + tokens[0] + ".json\\Google.Apis.Auth.OAuth2.Responses.TokenResponse-user");
            if (File.Exists(credPath))
            {
                File.Delete(credPath);
                return true;
            }
            return false;
        }
        public HashSet<string> GetUniqueHeaders(List<Message> allMessages)
        {


            var headerList = new List<string>();

            foreach (var body in allMessages)
            {
                IList<MessagePartHeader> headerPart = body.Payload.Headers;
                headerList.AddRange(from messagePartHeader in headerPart where messagePartHeader.Name.Equals("Subject") select messagePartHeader.Value);
            }
            var uniqueHeaders = new HashSet<string>(headerList);
            return uniqueHeaders;
        }

        public List<string> GetHeaderInList(HashSet<string> uniqueHeaders)
        {
            return uniqueHeaders.ToList();
        }

        public List<Message> GetSelectedMessages(List<Message> allMessages, string header)
        {
            return (from message in allMessages let headerPart = message.Payload.Headers from messagePartHeader in headerPart where messagePartHeader.Name.Equals("Subject") && messagePartHeader.Value.Equals(header) select message).ToList();
        }

        public List<Email> GetMessagesInFormatedWay(List<Message> allMessages)
        {
            var messages = new List<Email>();
            foreach (Message message in allMessages)
            {
                var email = new Email();
                email.Body = message.Snippet;
                string[] tokens = email.Body.Split(':');
                if (!tokens[0].Equals("Applicant Name")||tokens.Count()<4)
                {
                    goto Finish;
                }
                email.ApplicantName = tokens[1].Substring(1, tokens[1].Length - 14);
                email.From = tokens[2].Split(' ')[1];
                email.ContactNo = tokens[3].Split(' ')[1];

                IList<MessagePartHeader> headerPart = message.Payload.Headers;
                foreach (MessagePartHeader messagePartHeader in headerPart)
                {
                    if (messagePartHeader.Name.Equals("Date"))
                    {
                        email.DateTime = Convert.ToDateTime(messagePartHeader.Value);
                    }
                }
                messages.Add(email);
                Finish:
                string forTest = "forTest";
            }
            messages = messages.OrderByDescending(e => e.DateTime).ToList();
            return messages;
        }

        public List<Message> GetOnlyUnreadMessages(List<Message> messages)
        {
            return (from message in messages let labels = message.LabelIds from label in labels where label.ToLower().Equals("unread".ToLower()) select message).ToList();
        }

        public List<Message> GetOnlyReadMessages(List<Message> messages)
        {
            var readMessages = new List<Message>();
            foreach (Message message in messages)
            {
                IList<string> labels = message.LabelIds;
                bool isRead = true;
                foreach (string label in labels.Where(label => label.ToLower().Equals("unread".ToLower())))
                {
                    isRead = false;
                }
                if (isRead)
                {
                    readMessages.Add(message);
                }
            }
            return readMessages;
        }

        public string GetMessageAttacthment(GmailService service, String userId, List<Message> messages, string dictonary)
        {
            return messages.Aggregate("", (current, message) => current + GetAttachments(service, userId, message.Id, dictonary + "\\", message));
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

                        int i = 0;
                        string outputDirectory=outputDir+ fileName[0]+ "." + fileType[fileType.Length - 1];
                        while (File.Exists(outputDirectory))
                        {
                            i = i + 1;
                            outputDirectory = outputDir + fileName[0] + "(" + i + ")" + "." + fileType[fileType.Length - 1];
                            
                        }

                        File.WriteAllBytes(Path.Combine(outputDirectory,""), data);
                       
                        
                        attachmentMessage += userId + "'s " + "file has been saved as " + outputDirectory+ "\n";
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(@"An error occurred: " + e.Message);
            }
            return attachmentMessage;
        }
        public string ProduceExcelFile(List<Email> messages,string status,string dictonary)
        {
            var xlApp = new Microsoft.Office.Interop.Excel.Application();
            object misValue = System.Reflection.Missing.Value;

            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
            var xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.Item[1];

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

            int count = 0;
            string outputDirectory = dictonary+"\\ApplicantInformationOf"+status+".xls";
            while (File.Exists(outputDirectory))
            {
                count = count + 1;
                outputDirectory = dictonary + "\\ApplicantInformationOf" + status + "(" + count + ")" + ".xls";
            }

            xlWorkBook.SaveAs(outputDirectory, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            return @"Excel file created , you can find the excel file at " + outputDirectory+"\n";
        }
    }
}
