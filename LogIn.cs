
using EmailValidation;
using MailKit.Net.Smtp;
using System.ComponentModel;

namespace Email_Client_01
{
    // LogInForm.
    public partial class LogIn : Form
    {

        // We emply a singleton pattern on this form. No advantage to having multiple login pages open at once.
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

            // Get the user's input.
            Utility.username = EmailTextBox.Text;
            Utility.password = PasswordTextBox.Text;


            // Attempt to authorize the inptus
            IAuthenticator auth = new Authenticator();
            this.Cursor = Cursors.WaitCursor;
            var client = auth.Authenticate(Utility.username, Utility.password); // This does the authorization.
            this.Cursor = Cursors.Default;


            // If client is null, an error happened and we display the custom error message.
            if(client == null)
            {
                MessageBox.Show(auth.GetErrorMessage());
                return;
            }
            // If we get here the client we got is already authenticated and connected. 


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
            var Inbox = Inboxes.GetInstance(client); // Open the main inbox.
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

        private void PressEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LogInButton.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void EmailTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            PressEnter(sender, e);
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            PressEnter(sender, e);
        }
    }
}
