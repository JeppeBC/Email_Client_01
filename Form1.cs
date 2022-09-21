namespace Email_Client_01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
              
        private void croup7MailToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 send_mail = new Form2();
            send_mail.Show();
        }

        private void inboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void trashcanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Deleted mails");
        }

        private void inboxToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}