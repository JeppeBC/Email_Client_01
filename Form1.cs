namespace Email_Client_01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
                     
        private void SendNewMail(object sender, EventArgs e)
        {
            Form2 send_mail = new Form2();
            send_mail.Show();
        }

        private void LogIn(object sender, EventArgs e)
        {
            Form3 log_in = new Form3();
            log_in.Show();
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
    }
}