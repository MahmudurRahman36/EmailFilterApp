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
        }
        private void EmailFilterUI_Load(object sender, EventArgs e)
        {
            _emailLoadManager = new EmailLoadManager();
            allRadioButton.Checked = true;
        }

        private GmailService service;
        private EmailLoadManager _emailLoadManager;
        private List<Message> allMessageInformation; 
        private void loadbutton_Click(object sender, EventArgs e)
        {
            allMessageInformation = new List<Message>();
            string userName = emailTextBox.Text.ToLower();
            string password = passwordTextBox.Text;

            string[] tokens = userName.Split('@');
            userName = tokens[0] + "@gmail.com";

            service = _emailLoadManager.GetService(userName, password);
            List<Message> allMessageIds = _emailLoadManager.ListMessages(service, userName, "");;
            List<Message> fullMessages = _emailLoadManager.GetFullMessages(service, userName, allMessageIds);
            allMessageInformation = fullMessages;
            HashSet<string> uniqueHeadersinHashSet = _emailLoadManager.GetUniqueHeaders(fullMessages);
            List<string> uniqueHeader = _emailLoadManager.GetHeaderInList(uniqueHeadersinHashSet);

            headerComboBox.Items.Clear();
            headerComboBox.Items.Add("-------Select Subject of email------");
            foreach (string header in uniqueHeader)
            {
                headerComboBox.Items.Add(header);
            }

            MessageBox.Show("Dropdown Loaded for Messages Subject");
        }

        private void retrivebutton_Click(object sender, EventArgs e)
        {
            string userName = emailTextBox.Text.ToLower();
            string password = passwordTextBox.Text;

            string[] tokens = userName.Split('@');
            userName = tokens[0] + "@gmail.com";

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
            notificationMessage+=_emailLoadManager.GetMessageAttacthment(service, userName, selectedMessages);
            List<Email> messages = _emailLoadManager.GetMessagesInFormatedWay(selectedMessages);
            notificationMessage+=_emailLoadManager.ProduceExcelFile(messages, status);
            MessageBox.Show(notificationMessage);
        }

        
    }
}
