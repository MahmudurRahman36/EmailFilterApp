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

        public GmailService GetService(string userName, string password)
        {
            UserCredential credential;
            string[] tokens = userName.Split('@');
            string[] Scopes = { GmailService.Scope.GmailReadonly };
            string ApplicationName = "Gmail API .NET Quickstart";
            using (var stream = new FileStream("client_secret_for_"+tokens[0]+".json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart-for-" + tokens[0] + ".json");
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes,
                    "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
            }


            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");
            return service;
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

        public void ProduceExcelFile(List<Email> messages)
        {
            Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                //MessageBox.Show("Excel is not properly installed!!");
                return;
            }


            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "Applicant Name";
            xlWorkSheet.Cells[1, 2] = "Contact No";
            xlWorkSheet.Cells[1, 3] = "Email";

            int i = 2;
            foreach (Email message in messages)
            {
                xlWorkSheet.Cells[i, 1] = message.ApplicantName;
                xlWorkSheet.Cells[i, 2] = message.ContactNo;
                xlWorkSheet.Cells[i, 3] = message.From;
                i++;
            }

            xlWorkBook.SaveAs("d:\\ApplicateInformation.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);

            MessageBox.Show("Excel file created , you can find the file d:\\ApplicateInformation.xls");
        }
    }
}
