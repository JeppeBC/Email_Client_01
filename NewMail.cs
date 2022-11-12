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
using EmailValidation; // package

namespace Email_Client_01
{
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

        private void richTextBox2_MouseHover(object sender, EventArgs e)
        {
            
            ToolTip tooltip = new ToolTip();
            tooltip.InitialDelay = 150;

            tooltip.SetToolTip(richTextBox2, "Separate recipients with ','");
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

                    // add it to the mime message
                    attachmentPaths.Add(fileSelected);

                    // add to listbox and show the listbox if it is currently hidden.
                    AttachmentLabel.Visible = true;
                    AttachmentsListBox.Visible = true;
                    RemoveAttachmentButton.Visible = true;

                    // Shorten the file name to not include directories
                    string fileSelectedShort = fileSelected.Substring(fileSelected.LastIndexOf('\\') + 1) + " ";
                    AttachmentsListBox.Items.Add(fileSelectedShort);
                }
            }
        }




        private string[]? GetRecipients()
        {
            if (string.IsNullOrEmpty(richTextBox2.Text))
            {
                MessageBox.Show("No recipient!");
                return null;
            }

            else
            {
                return richTextBox2.Text.Split(",");
            }
        }

        private string[]? GetCCs()
        {
            if (string.IsNullOrEmpty(richTextBox5.Text))
            {
                return null; 
            }
            else
            {
                return richTextBox5.Text.Split(",");
            }
        }
        private string? GetSubject()
        {
            if(string.IsNullOrEmpty(richTextBox3.Text))
            {
                DialogResult result = MessageBox.Show("No subject. Do you want to send the email anyway?", "Fault", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return null;
                }
                else if(result == DialogResult.Yes)
                {
                    return "<no subject>";
                }
                return null;
            }
            else
            {
                return richTextBox3.Text;
            }
        }


        private string FilterString(string s)
        {
            // we expect strings of the form "Alias" <MailAddress>
            // and want to return only MailAddress

            // remove the text between quotations and " chars. 
            int startQuotation = s.IndexOf('"');
            int endQuoatation = s.IndexOf('"', startQuotation + 1);
            string result = s.Remove(startQuotation, endQuoatation - startQuotation + 1);
            
            // Remove the '<' chars
            result = result.Replace('<', ' ');
            result = result.Replace('>', ' ');


            // remove the whitespaces;
            MessageBox.Show(result.Trim());
            return result.Trim();
        }

        private bool validateRecipientArray(string[] recipients)
        {

            // if we reply to a mail an internet address is inserted i.e. of the form 
            // "Alias" <MailAddress>, ....


            foreach (string recipient in recipients)
            {
                string r = recipient.Replace(",", "");

                r = FilterString(r);

                if(!EmailValidator.Validate(r))
                {
                    return false;
                }
            }
            return true;
        }



        private void Send_mail(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;

            var To = GetRecipients();
            var CC = GetCCs();
            
            var Subject = GetSubject();
/*                richTextBox3.Text;*/
            var Content = richTextBox4.Text;


            Email email = new Email(To, CC, Subject, Content, attachmentPaths);


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

        private void NewMail_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_Validating(object sender, CancelEventArgs e)
        {
            string[] recipients = richTextBox2.Text.Split(",");
            bool valid = validateRecipientArray(recipients);
            button1.Enabled = valid;

            if (!valid)
            {
                richTextBox2.ForeColor = Color.Red;

            }

            else
            {
                richTextBox2.ForeColor = Color.Green;
            }
        }


        // CC validation does not work for some reason, #TODO
        private void richTextBox5_Validating(object sender, CancelEventArgs e)
        {
            string[] cc = richTextBox5.Text.Split(",");
            bool valid = validateRecipientArray(cc);
            if (!valid)
            {
                richTextBox5.ForeColor = Color.Red;
            }
            else
            {
                richTextBox2.ForeColor = Color.Green;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void RemoveAttachmentButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Remove the attachment from the listbox
                var idx = AttachmentsListBox.SelectedIndex;
                AttachmentsListBox.Items.RemoveAt(idx);

                // Remove the attachment from the list to be constructed as mime message
                attachmentPaths.RemoveAt(idx);
            }
            catch
            {
                MessageBox.Show("No Attachment selected!");
            }

            // if no more attachments hide the listbox, label and button
            if(attachmentPaths.Count <= 0)
            {
                RemoveAttachmentButton.Visible = false;
                AttachmentLabel.Visible = false;
                AttachmentsListBox.Visible = false;
            }
        }
    }

    public class Email
    {
        public List<MailboxAddress> To { get; set; }

        public List<MailboxAddress> CC { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }

        public List<string> Attachments { get; set; }

        public Email(string[] to, string[]? cc, string subject, string content, List<string> attachments)

        {
            To = new List<MailboxAddress>();
            // username and address, #TODO currently we do not have aliases but extend this once we do

            foreach(var recipient in to)
            {
                To.Add(MailboxAddress.Parse(recipient));
            }

            
            CC = new List<MailboxAddress>();
            if(!(cc == null))
            {
                foreach (var c in cc)
                {
                    CC.Add(MailboxAddress.Parse(c));
                }
            }



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

}
