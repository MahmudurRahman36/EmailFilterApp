using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmailFilterApp.BLL;
using EmailFilterApp.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Message = Google.Apis.Gmail.v1.Data.Message;

namespace EmailFilterApp
{
    public partial class EmailFilterUI : Form
    {
        public EmailFilterUI()
        {
            InitializeComponent();
            dropdownLoadProgressBar.Visible = false;
            pleaseWaitLabel.Visible = false;
        }
        private void EmailFilterUI_Load(object sender, EventArgs e)
        {
            _emailLoadManager = new EmailLoadManager();
            allRadioButton.Checked = true;
        }

        private string userName;
        private GmailService service;
        private EmailLoadManager _emailLoadManager;
        private List<Message> allMessageInformation; 
        private void loadbutton_Click(object sender, EventArgs e)
        {
            headerComboBox.Items.Clear();
            string userName = emailTextBox.Text.ToLower();
            this.userName = "";
            if (userName.Equals(""))
            {
                MessageBox.Show("Please give email address or username");
                goto Finish;
            }

            dropdownLoadProgressBar.Visible = true;
            pleaseWaitLabel.Visible = true;
            dropdownLoadProgressBar.Value = 0;          

            string[] tokens = userName.Split('@');
            userName = tokens[0] + "@gmail.com";

            service = _emailLoadManager.GetService(userName);
            List<Message> allMessageIds = _emailLoadManager.ListMessages(service, userName, "");
            if (allMessageIds.Count==0)
            {
                MessageBox.Show("No message found for this account");
                goto Finish;
            }

            dropdownLoadProgressBar.Maximum = allMessageIds.Count;
            List<Message> fullMessages = GetFullMessages(service, userName, allMessageIds);
            allMessageInformation = fullMessages;
            this.userName = userName;

            HashSet<string> uniqueHeadersinHashSet = _emailLoadManager.GetUniqueHeaders(fullMessages);
            List<string> uniqueHeader = _emailLoadManager.GetHeaderInList(uniqueHeadersinHashSet);
            
            //headerComboBox.Items.Add("-------Select Subject of email------");
            foreach (string header in uniqueHeader)
            {
                headerComboBox.Items.Add(header);
            }
            
            Finish:
            dropdownLoadProgressBar.Visible = false;
            pleaseWaitLabel.Visible = false;
            headerComboBox.Refresh();
        }

        private void retrivebutton_Click(object sender, EventArgs e)
        {
            string userName = emailTextBox.Text.ToLower();
            if (userName.Equals(""))
            {
                MessageBox.Show("Please give email address or username");
                goto Finish;
            }
            
            string[] tokens = userName.Split('@');
            userName = tokens[0] + "@gmail.com";
            if (!userName.Equals(this.userName))
            {
                MessageBox.Show("User name or Email address does not match with dropdown loaded user name or email address");
                goto Finish;
            }

            string notificationMessage = "";
            string selectedHeader = headerComboBox.SelectedItem.ToString();
            List<Message> selectedMessages = _emailLoadManager.GetSelectedMessages(allMessageInformation, selectedHeader);           

            string status = "AllMessages";
            if (readRadioButton.Checked)
            {
                selectedMessages = _emailLoadManager.GetOnlyReadMessages(selectedMessages);
                status = "OnlyReadMessages";
            }
            else if (unreadRadioButton.Checked)
            {
                selectedMessages = _emailLoadManager.GetOnlyUnreadMessages(selectedMessages);
                status = "OnlyUnreadMessages";
            }
            if (selectedMessages.Count == 0)
            {
                MessageBox.Show("All condition did not fulfill any message");
                goto Finish;
            }

            notificationMessage+=_emailLoadManager.GetMessageAttacthment(service, userName, selectedMessages);
            List<Email> messages = _emailLoadManager.GetMessagesInFormatedWay(selectedMessages);
            notificationMessage+=_emailLoadManager.ProduceExcelFile(messages, status);
            MessageBox.Show(notificationMessage);

            Finish:
            headerComboBox.Refresh();
        }
        public List<Message> GetFullMessages(GmailService service, string userName, List<Message> allMessages)
        {
            List<Message> newMessages = new List<Message>();

            foreach (var message1 in allMessages)
            {
                Message message2 = _emailLoadManager.GetMessage(service, userName, message1.Id);
                newMessages.Add(message2);
                dropdownLoadProgressBar.Increment(1);
            }
            return newMessages;
        }

        private void deleteAuthenticationButton_Click(object sender, EventArgs e)
        {
            string userName = emailTextBox.Text.ToLower();
            if (userName.Equals(""))
            {
                MessageBox.Show("Please give email address or username");
                goto Finish;
            }

            string[] tokens = userName.Split('@');
            userName = tokens[0] + "@gmail.com";
            if (_emailLoadManager.DeleteFile(userName))
            {
                MessageBox.Show("Authentication related to " + userName + " has been removed");
            }
            else
            {
                MessageBox.Show("No authentication related to " + userName + " is present");
            }
            Finish:
            headerComboBox.Refresh();
        }
        
    }
}
