using System;
using System.DirectoryServices;
using System.Xml.Linq;

namespace Email_Client_01
{
    public partial class Inboxes : Form
    {
        public Inboxes()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void SendNewMail(object sender, EventArgs e)
        {
            NewMail send_mail = new NewMail();
            send_mail.Show();
        }

        private void Prime_Mail_Homepage(object sender, EventArgs e)
        {
            // return to home page
        }

        private void Inbox_Click(object sender, EventArgs e)
        {

        }

        private void Emails_Click(object sender, EventArgs e)
        {

        }

        private void Inbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // retrieve inbox here
        }

        // once an email is chosen from list of recieved emails use
        // Reading_email Read_mail = new Reading_email();
        // Read_mail.Show();
        // To open form to read mails

        private void trashicon_Click(object sender, EventArgs e)
        {
            // delete element from inbox
        }

        private void RefreshPage_Click(object sender, EventArgs e)
        {
            // refresh elements in inbox
        }
    }
}