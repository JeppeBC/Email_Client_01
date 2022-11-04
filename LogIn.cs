
using MailKit.Net.Smtp;

namespace Email_Client_01
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();

            if (Properties.Credentials.Default.username != null)
            {
                textBox1.Text = Properties.Credentials.Default.username;
            }
            if (Properties.Credentials.Default.password != null)
            {
                textBox2.Text = Properties.Credentials.Default.password;
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
                
        private void EmailAddress_Click(object sender, EventArgs e)
        {
            textBox1.ResetText();
            textBox1.ForeColor = System.Drawing.Color.Black;
        }

        private void Password_Click(object sender, EventArgs e)
        {
            textBox2.ResetText();
            textBox2.ForeColor = System.Drawing.Color.Black;
        }

        private void LogIn_button(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                Properties.Credentials.Default.username = textBox1.Text;
                Properties.Credentials.Default.password = textBox2.Text;
                Properties.Credentials.Default.Save();
            }

            if (!checkBox1.Checked)
            {
                Properties.Credentials.Default.username = "";
                Properties.Credentials.Default.password = "";
                Properties.Credentials.Default.Save();
            }

            string username = textBox1.Text;
            string password = textBox2.Text;
            Utility.username = username;
            Utility.password = password;

            using (var client = new SmtpClient())
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    string mail = username.Substring(username.LastIndexOf("@") + 1);

                    client.Connect("smtp." + mail, 465, true);

                    client.Authenticate(username, password);

                    new Inboxes().Show();
                    /*Inboxes.show();*/
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                    this.Cursor=Cursors.Default;
                }
            }
        }

        private void Exit_button(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
