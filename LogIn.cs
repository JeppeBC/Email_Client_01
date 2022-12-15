
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

/*        private void Form3_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }*/


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

        private void LogIn_button(object sender, EventArgs e)
        {
            Utility.username = EmailTextBox.Text;
            Utility.password = PasswordTextBox.Text;

            Authenticator auth = new();


            this.Cursor = Cursors.WaitCursor;
            var client = auth.Authenticate(Utility.username, Utility.password); // attempt to authorize the given credentials

            this.Cursor = Cursors.Default;
            if(client == null)
            {
                MessageBox.Show(auth.ErrorMessage);
                return;
            }


            // If we get here the client authorized successfully

            // Store the credentials if checkbox is checked.
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


            this.Hide();
            var Inbox = Inboxes.GetInstance(client);
            Inbox.FormClosed += (s, args) =>
            {
                Properties.Time.Default.Date = DateTime.Now; // Store the exit time so we have a reference to how long since we last used the client.
                Properties.Time.Default.Save();
                this.Close();
            };
            Inbox.Show();

        }

        private void Exit_button(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
