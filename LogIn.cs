
using EmailValidation;
using MailKit.Net.Smtp;
using System.ComponentModel;

namespace Email_Client_01
{
    public partial class LogIn : Form
    {
        private static LogIn instance = null!;
        private LogIn()
        {
            InitializeComponent();

            if (Properties.Credentials.Default.username != null)
            {
                EmailTextBox.Text = Properties.Credentials.Default.username;
            }
            if (Properties.Credentials.Default.password != null)
            {
                PasswordTextBox.Text = Properties.Credentials.Default.password;
            }
        }


        public static LogIn GetInstance
        {
            // coalescing operator, return first non-null value; 
            get { return instance ??= new LogIn(); }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }


        private void EmailAddress_Click(object sender, EventArgs e)
        {
            EmailTextBox.ResetText();
            EmailTextBox.ForeColor = System.Drawing.Color.Black;
        }

        private void Password_Click(object sender, EventArgs e)
        {
            PasswordTextBox.ResetText();
            PasswordTextBox.ForeColor = System.Drawing.Color.Black;
        }

        private async void LogIn_button(object sender, EventArgs e)
        {
            // check username and password validity (length, and username looks like email?)
            if (PasswordTextBox.Text.Length > 99 || PasswordTextBox.Text.Length < 8)
            {
                MessageBox.Show("Please enter a password between 8 and 99 characters (inclusive)");
                return;
            }
            if (!EmailValidator.Validate(EmailTextBox.Text))
            {
                MessageBox.Show("Please enter a valid email address");
                return;
            }


            if (RememberMeCheckBox.Checked)
            {
                Properties.Credentials.Default.username = EmailTextBox.Text;
                Properties.Credentials.Default.password = PasswordTextBox.Text;
                Properties.Credentials.Default.Save();
            }

            if (!RememberMeCheckBox.Checked)
            {
                Properties.Credentials.Default.username = "";
                Properties.Credentials.Default.password = "";
                Properties.Credentials.Default.Save();
            }

            string username = EmailTextBox.Text;
            string password = PasswordTextBox.Text;
            Utility.username = username;
            Utility.password = password;

            this.Cursor = Cursors.WaitCursor;
            var client = await Utility.GetImapClient();
            if (client.IsConnected && client.IsAuthenticated)
            {
                this.Hide();
                var Inbox = Inboxes.GetInstance(client);
                Inbox.FormClosed += (s, args) =>
                {
                    Properties.Time.Default.Date = DateTime.Now;
                    Properties.Time.Default.Save();
                    this.Close();
                };
                Inbox.Show();
            }
            this.Cursor = Cursors.Default;
        }

        private void Exit_button(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
