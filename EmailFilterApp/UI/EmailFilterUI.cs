using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EmailFilterApp.BLL;
using EmailFilterApp.Models;
using Google.Apis.Gmail.v1;
using Message = Google.Apis.Gmail.v1.Data.Message;

namespace EmailFilterApp.UI
{
    public partial class EmailFilterUi : Form
    {
        public EmailFilterUi()
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

        private string _userName;
        private GmailService _service;
        private EmailLoadManager _emailLoadManager;
        private List<Message> _allMessageInformation; 
        private void loadbutton_Click(object sender, EventArgs e)
        {            
            headerComboBox.Items.Clear();
            string userName = emailTextBox.Text.ToLower();
            this._userName = "";
            if (userName.Equals(""))
            {
                MessageBox.Show(@"Please give email address or username");
                return;
            }

            dropdownLoadProgressBar.Visible = true;
            pleaseWaitLabel.Visible = true;
            dropdownLoadProgressBar.Value = 0;          

            string[] tokens = userName.Split('@');
            userName = tokens[0] + "@gmail.com";

            _service = _emailLoadManager.GetService(userName);
            List<Message> allMessageIds = _emailLoadManager.ListMessages(_service, userName, "");
            if (allMessageIds.Count==0)
            {
                MessageBox.Show(@"No message found for this account");
                return;
            }

            dropdownLoadProgressBar.Maximum = allMessageIds.Count;
            List<Message> fullMessages = GetFullMessages(_service, userName, allMessageIds);
            _allMessageInformation = fullMessages;
            this._userName = userName;

            HashSet<string> uniqueHeadersinHashSet = _emailLoadManager.GetUniqueHeaders(fullMessages);
            List<string> uniqueHeader = _emailLoadManager.GetHeaderInList(uniqueHeadersinHashSet);
            
            //headerComboBox.Items.Add("-------Select Subject of email------");
            foreach (string header in uniqueHeader)
            {
                headerComboBox.Items.Add(header);
            }
            
            dropdownLoadProgressBar.Visible = false;
            pleaseWaitLabel.Visible = false;
        }

        private void retrivebutton_Click(object sender, EventArgs e)
        {
            string userName = emailTextBox.Text.ToLower();
            if (userName.Equals(""))
            {
                MessageBox.Show(@"Please give email address or username");
                return;
            }
            if (_allMessageInformation==null)
            {
                MessageBox.Show(@"Please load the messages and dropdown first");
                return;
            }
            string[] tokens = userName.Split('@');
            userName = tokens[0] + "@gmail.com";
            if (!userName.Equals(_userName))
            {
                MessageBox.Show(@"User name or Email address does not match with dropdown loaded user name or email address");
                return;
            }

            string notificationMessage = "";
            string selectedHeader = headerComboBox.SelectedItem.ToString();
            List<Message> selectedMessages = _emailLoadManager.GetSelectedMessages(_allMessageInformation, selectedHeader);           

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
                MessageBox.Show(@"All condition did not fulfill any message");
                return;
            }


            MessageBox.Show(@"Give Location for saving Excel File.");
            List<Email> messages = _emailLoadManager.GetMessagesInFormatedWay(selectedMessages);
            folderBrowserDialog1.ShowDialog();
            string dictonary = folderBrowserDialog1.SelectedPath;           
            notificationMessage+=_emailLoadManager.ProduceExcelFile(messages, status,dictonary);

            if (getAttachmentCheckBox.Checked)
            {
                MessageBox.Show(@"Give Location for saving Attactments.");
                folderBrowserDialog1.ShowDialog();
                string attachmentDictonay = folderBrowserDialog1.SelectedPath;
                notificationMessage += _emailLoadManager.GetMessageAttacthment(_service, userName, selectedMessages, attachmentDictonay);
            }

            MessageBox.Show(notificationMessage);
        }
        public List<Message> GetFullMessages(GmailService service, string userName, List<Message> allMessages)
        {
            var newMessages = new List<Message>();

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
                MessageBox.Show(@"Please give email address or username");
                return;
            }

            string[] tokens = userName.Split('@');
            userName = tokens[0] + "@gmail.com";
            if (_emailLoadManager.DeleteFile(userName))
            {
                MessageBox.Show(@"Authentication related to " + userName + @" has been removed");
            }
            else
            {
                MessageBox.Show(@"No authentication related to " + userName + @" is present");
            }
        }
        
    }
}
