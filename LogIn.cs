
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

        private void LogIn_button(object sender, EventArgs e)
        {

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

            using (var client = new SmtpClient())
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    string mail = username.Substring(username.LastIndexOf("@") + 1);

                    client.Connect("smtp." + mail, 465, true);

                    client.Authenticate(username, password);

                    
                    Inboxes.GetInstance.Show();
                    this.Hide(); // #TODO CLOSE THIS FORM INSTEAD
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


        private async Task DoWork(BackgroundWorker w, DoWorkEventArgs e)
        {
            
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Assign the result of the computation to the Result property of the DoWorkEventArgs
            // object. This is will be available to the RunWorkerCompleted eventhandler.
            e.Result = DoWork(worker, e);

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // not sure if we need this 
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("Error");
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Cancelled");
            }
            else
            {
                // start a different thread?
                MessageBox.Show("Completed");
            }

            
        }
    }
}
