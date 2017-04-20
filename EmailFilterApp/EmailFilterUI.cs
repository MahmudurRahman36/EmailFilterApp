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
        EmailLoadManager _emailLoadManager=new EmailLoadManager();
        List<Message> allMessageInformation; 
        private void loadbutton_Click(object sender, EventArgs e)
        {
            allMessageInformation = new List<Message>();
            string userName = emailTextBox.Text;
            string password = passwordTextBox.Text;
            GmailService service = _emailLoadManager.GetService(userName, password);
            List<Message> allMessageIds = _emailLoadManager.ListMessages(service, userName, "");;
            List<Message> fullMessages = _emailLoadManager.GetFullMessages(service, userName, allMessageIds);
            allMessageInformation = fullMessages;
            HashSet<string> uniqueHeadersinHashSet = _emailLoadManager.GetUniqueHeaders(fullMessages);
            List<string> uniqueHeader = _emailLoadManager.GetHeaderInList(uniqueHeadersinHashSet);
            headerComboBox.Items.Clear();
            foreach (string header in uniqueHeader)
            {
                headerComboBox.Items.Add(header);
            }
        }

        private void retrivebutton_Click(object sender, EventArgs e)
        {
            string selectedHeader = headerComboBox.SelectedItem.ToString();
            List<Message> selectedMessages = _emailLoadManager.GetSelectedMessages(allMessageInformation, selectedHeader);
            List<Email> messages = _emailLoadManager.GetMessagesInFormatedWay(selectedMessages);
            _emailLoadManager.ProduceExcelFile(messages);
        }
    }
}
