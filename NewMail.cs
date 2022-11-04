using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Email_Client_01
{
    public class Email
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }

        public List<string> Attachments { get; set; }

        public Email(IEnumerable<string> to, string subject, string content, List<string> attachments)

        {

            To = new List<MailboxAddress>();
            // username and address, #TODO currently we do not have aliases but extend this once we do
            To.AddRange(to.Select(x => new MailboxAddress(x, x)));

            Subject = subject;
            Content = content;
            Attachments = attachments;
        }
    }

    public interface IEmailSender
    {
        void sendEmail(Email email);
    }


    public class EmailSender : IEmailSender
    {

        public void sendEmail(Email email)
        {
            var emailMessage = CreateEmailMessage(email);

            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Email email)
        {
            MimeMessage emailMessage = new();

            var builder = new BodyBuilder
            {
                TextBody = email.Content // TODO formatting needed here???
            };
            foreach (var attachment in email.Attachments)
            {
                builder.Attachments.Add(attachment);
            }

            emailMessage.Body = builder.ToMessageBody();                        // TODO add alias/username
            emailMessage.From.Add(new MailboxAddress(Properties.Credentials.Default.username, Properties.Credentials.Default.username)); // username    &&     //email of user
            emailMessage.To.AddRange(email.To);
            emailMessage.Subject = email.Subject;

            return emailMessage;
        }

        private async void Send(MimeMessage emailMessage)
        {

            using (var client = await Utility.GetSmtpClient())
            {
                try
                {
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    // always disconnect no matter the scenario
                    client?.Disconnect(true);
                    // and dispose/free/delete the smtp client object
                    client?.Dispose();
                }
            }
        }
    }


    public partial class NewMail : Form
    {
        List<string> attachmentPaths;
        public NewMail()
        {
            InitializeComponent();
            attachmentPaths = new();

        }

        public NewMail(MimeMessage msg)
        {

            InitializeComponent();
            attachmentPaths = new();
            

            richTextBox2.Text = msg.To.ToString();
            richTextBox3.Text = msg.Subject;
            richTextBox4.Text = msg.TextBody;
            richTextBox5.Text = msg.Cc.ToString();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // read from username
        }
            
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
               
        private void From_Click(object sender, EventArgs e)
        {
            richTextBox1.ResetText();
            richTextBox1.ForeColor = System.Drawing.Color.Black;
        }

        private void To_Click(object sender, EventArgs e)
        {
            richTextBox2.ForeColor = System.Drawing.Color.Black;
        }

        private void Subject_Click(object sender, EventArgs e)
        {
            richTextBox3.ForeColor = System.Drawing.Color.Black;
        }

        private void Mail_click(object sender, EventArgs e)
        {
            richTextBox4.ForeColor = System.Drawing.Color.Black;
        }

        private void Attach_file(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.InitialDirectory = "c:\\";
                fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                fileDialog.FilterIndex = 2;
                fileDialog.RestoreDirectory = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileSelected = fileDialog.FileName;
                    attachmentPaths.Add(fileSelected);
                    MessageBox.Show("File added succesfuly");
                }
            }
        }


        private static String RemoveQuatations(string s)
        {
            var rs = s.Split(new[] { '"' }).ToList();
            return String.Join("\"\"", rs.Where(_ => rs.IndexOf(_) % 2 == 0));
        }


        private List<string> GetRecipients()
        {
            string recipientsTextBox = richTextBox2.Text;
            // "John Doe" <John.Doe@gmail.com>, ...

            recipientsTextBox = RemoveQuatations(recipientsTextBox);
            // "" <John.Doe@gmail.com>, ...


            // remove specific chars and whitespace
            recipientsTextBox = recipientsTextBox.Trim(new char[] { '"', ' ', '<', '>' });
            // John.Doe@gmail.com, ...

            string[] recipients = recipientsTextBox.Split(',');


            return new List<string>(recipients);
        }



        private void Send_mail(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;

            var To = GetRecipients();
            var Subject = richTextBox3.Text;
            var Content = richTextBox4.Text;

            Email email = new Email(To, Subject, Content, attachmentPaths);


            var s = new EmailSender();
            s.sendEmail(email);

            this.Close();
        }

        private void Exit_button(object sender, EventArgs e)
        {
            this.Close();            
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
