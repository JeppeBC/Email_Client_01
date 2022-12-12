﻿using MailKit;
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
using MailKit.Net.Imap;
using MailKit.Search;
using Org.BouncyCastle.Asn1.X509;

namespace Email_Client_01
{
    public partial class NewMail : Form
    {
        List<string> attachmentPaths;

        // draft specific
        bool MailIsDraft = false;
        string? DraftID;
        ImapClient client; 
        

        public NewMail(ImapClient client)
        {
            InitializeComponent();
            
            this.client = client;
            attachmentPaths = new();

        }

        public NewMail(MimeMessage msg, ImapClient client)
        {
            InitializeComponent();
            attachmentPaths = new();

            this.client = client;
            RecipientTextBox.Text = msg.To.ToString();
            SubjectTextBox.Text = msg.Subject;
            MessageBodyTextBox.Text = msg.TextBody;
            CCTextBox.Text = msg.Cc.ToString();


            foreach(var attach in msg.Attachments)
            {
                MessageBox.Show(attach.ToString());
            }
        }

        public NewMail(MimeMessage msg, bool isDraft, ImapClient client)
        {
            InitializeComponent();
            attachmentPaths = new();

            this.client = client;
            MailIsDraft = isDraft;
            RecipientTextBox.Text = msg.To.ToString();
            SubjectTextBox.Text = msg.Subject;
            MessageBodyTextBox.Text = msg.TextBody;
            CCTextBox.Text = msg.Cc.ToString();

            if(isDraft)
            {
                DraftID = msg.MessageId;
            }
        }

        private void RecipientsMouseOver(object sender, EventArgs e)
        {

            ToolTip tooltip = new ToolTip();
            tooltip.InitialDelay = 150;

            tooltip.SetToolTip(RecipientTextBox, "Separate recipients with ','");
        }

        private void To_Click(object sender, EventArgs e)
        {
            RecipientTextBox.ForeColor = System.Drawing.Color.Black;
        }

        private void Subject_Click(object sender, EventArgs e)
        {
            SubjectTextBox.ForeColor = System.Drawing.Color.Black;
        }

        private void Mail_click(object sender, EventArgs e)
        {
            MessageBodyTextBox.ForeColor = System.Drawing.Color.Black;
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
                    AttachmentSizeLabel.Visible = true;

                    // Shorten the file name to not include directories
                    string fileSelectedShort = fileSelected.Substring(fileSelected.LastIndexOf('\\') + 1) + " ";
                    AttachmentsListBox.Items.Add(fileSelectedShort);
                }
            }
        }




        private string[]? GetRecipients()
        {
            if (string.IsNullOrEmpty(RecipientTextBox.Text))
            {
                MessageBox.Show("No recipient!");
                return null;
            }

            else
            {
                return RecipientTextBox.Text.Split(",");
            }
        }

        private string[]? GetCCs()
        {
            if (string.IsNullOrEmpty(CCTextBox.Text))
            {
                return null;
            }
            else
            {
                return CCTextBox.Text.Split(",");
            }
        }
        private string? GetSubject()
        {
            if (string.IsNullOrEmpty(SubjectTextBox.Text))
            {
                DialogResult result = MessageBox.Show("No subject. Do you want to send the email anyway?", "Fault", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    return "<no subject>";
                }
                return null;
            }
            else
            {
                return SubjectTextBox.Text;
            }
        }


        private string FilterString(string s)
        {
            // we expect strings of the form "Alias" <MailAddress>
            // and want to return only MailAddress
            string result = s;
            // remove the text between quotations and " chars. 
            int startQuotation = s.IndexOf('"');
            int endQuoatation = s.IndexOf('"', startQuotation + 1);
            if (startQuotation != -1 && endQuoatation != -1)
                result = s.Remove(startQuotation, endQuoatation - startQuotation + 1);

            // Remove the '<' chars
            result = result.Replace('<', ' ');
            result = result.Replace('>', ' ');

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

                if (!EmailValidator.Validate(r))
                {
                    return false;
                }
            }
            SendButton.Enabled = true;
            return true;
        }



        private async void Send_mail(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;

            var To = GetRecipients();

            // To should never be null, as you would not be able to click the button
            // but this is here just in case. IDK data corruption or something could still happen. 
            if(To == null)
            {
                MessageBox.Show("Recipients not found");
                return;
            }
            
            var CC = GetCCs();
            var Subject = GetSubject();


            if(Subject == null) // another gaurd 
            {
                MessageBox.Show("Invalid subject found");
                return;
            }

            var Content = MessageBodyTextBox.Text;


            Email email = new Email(To, CC, Subject, Content, attachmentPaths);

            var s = new EmailSender();
            s.sendEmail(email);

            // expensive as we establish smtp connection to send just prior and then imap connection to delete..
            if(MailIsDraft)
            {
                // delete the mail from drafts folder
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    var draftsFolder = await getDraftFolder(CancellationToken.None);
                    if(draftsFolder == null)
                    {
                        MessageBox.Show("\"Draft(s)\" folder not found");
                        return;
                    }
                    await draftsFolder.OpenAsync(FolderAccess.ReadWrite);
                    var uid = draftsFolder.Search(SearchQuery.HeaderContains("Message-Id", DraftID));

                    await draftsFolder.AddFlagsAsync(uid, MessageFlags.Deleted, true);
                    await draftsFolder.ExpungeAsync();
/*                        RefreshCurrentFolder();*/
                }
                catch (Exception ex)
                {
                    // Protocol exceptions often result in client getting disconnected. IO exception always result in client disconnects. 
                    if (ex is ImapProtocolException || ex is IOException)
                    {
                        await Utility.ReconnectAsync(client);
                    }
                    else
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }

            this.Close();
        }

        private void Exit_button(object sender, EventArgs e)
        {
            this.Close();
        }


        private void RecipientsValidating(object sender, CancelEventArgs e)
        {
            string[] recipients = RecipientTextBox.Text.Split(",");
            bool valid = validateRecipientArray(recipients);
            SendButton.Enabled = valid;

            if (!valid)
            {
                RecipientTextBox.ForeColor = Color.Red;

            }

            else
            {
                RecipientTextBox.ForeColor = Color.Green;
            }
        }


        // CC validation does not work for some reason, #TODO
        private void CCValidating(object sender, CancelEventArgs e)
        {
            string[] cc = CCTextBox.Text.Split(",");
            bool valid = validateRecipientArray(cc);
            SendButton.Enabled = valid;

            if (!valid)
            {
                CCTextBox.ForeColor = Color.Red;
            }
            else
            {
                CCTextBox.ForeColor = Color.Green;
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

                // failsafe
                if (idx < 0) return;
                AttachmentsListBox.Items.RemoveAt(idx);

                // Remove the attachment from the list to be constructed as mime message
                attachmentPaths.RemoveAt(idx);
            }
            catch
            {
                MessageBox.Show("No Attachment selected!");
            }

            // if no more attachments hide the listbox, label and button
            if (attachmentPaths.Count <= 0)
            {
                RemoveAttachmentButton.Visible = false;
                AttachmentLabel.Visible = false;
                AttachmentsListBox.Visible = false;
                AttachmentSizeLabel.Visible = false;
            }
        }


        public class Email
        {
            public bool isDraft = false;
            public List<MailboxAddress> To { get; set; }

            public List<MailboxAddress> CC { get; set; }
            public string Subject { get; set; }

            public string Content { get; set; }

            public List<string> Attachments { get; set; }

            public Email(string[] to, string[]? cc, string subject, string content, List<string> attachments)

            {
                To = new List<MailboxAddress>();
                // username and address, #TODO currently we do not have aliases but extend this once we do

                foreach (var recipient in to)
                {
                    To.Add(MailboxAddress.Parse(recipient));
                }


                CC = new List<MailboxAddress>();
                if (!(cc == null))
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
                emailMessage.Cc.AddRange(email.CC);
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

        private async Task<IMailFolder?> getDraftFolder(CancellationToken cancellationToken)
        {
            try
            {
                string[] DraftFolderNames = { "Drafts", "Kladder", "Draft" };
                if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
                {
                    var trashFolder = client.GetFolder(SpecialFolder.Drafts);
                    return trashFolder;
                }

                else
                {
                    var personal = client.GetFolder(client.PersonalNamespaces[0]);

                    foreach (var folder in personal.GetSubfolders(false, cancellationToken))
                    {
                        foreach (var name in DraftFolderNames)
                        {
                            if (folder.Name == name)
                                return folder;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                // Protocol exceptions often result in client getting disconnected. IO exception always result in client disconnects.
                if (ex is ImapProtocolException || ex is IOException)
                {
                    await Utility.ReconnectAsync(client);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return null;
            
        }

        // saves the current message state as a draft
        private MimeMessage BuildDraftMessage()
        {
            BodyBuilder builder = new BodyBuilder();
            MimeMessage message = new MimeMessage();
            
            message.Subject = SubjectTextBox.Text;

            if (string.IsNullOrEmpty(MessageBodyTextBox.Text))
            {
                builder.TextBody = @"";
            }
            else
            {
                builder.TextBody = MessageBodyTextBox.Text;
            }

            if (!string.IsNullOrEmpty(RecipientTextBox.Text))
            {
                string[] recipients = RecipientTextBox.Text.Split(",");

                foreach (var r in recipients)
                {
                    message.To.Add(MailboxAddress.Parse(r));
                }
            }

            if (!string.IsNullOrEmpty(CCTextBox.Text))
            {
                string[] CCs = CCTextBox.Text.Split(",");

                foreach (var cc in CCs)
                {
                    message.Cc.Add(MailboxAddress.Parse(cc));
                }
            }

            if (AttachmentsListBox.Items.Count > 0)
            {
                foreach (var item in AttachmentsListBox.Items)
                {
                    // todo don't know if this works
                    builder.Attachments.Add(item.ToString());
                }
            }

            message.Body = builder.ToMessageBody();
            return message;
        }


        private async void SaveDraftButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                MimeMessage message = BuildDraftMessage();
                IMailFolder? draftsFolder = await getDraftFolder(CancellationToken.None);
                if(draftsFolder == null)
                {
                    MessageBox.Show("\"Draft(s)\" folder not found");
                    return;

                }
                await draftsFolder.OpenAsync(FolderAccess.ReadWrite);
                await draftsFolder.AppendAsync(message, MessageFlags.Draft);
            }
            catch (Exception ex)
            {
                // IMAP protocol exception often causes client disconnects, and io exceptions always do.
                if(ex is ImapProtocolException || ex is IOException)
                {
                    await Utility.ReconnectAsync(client);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.Close();
            }
        }
    }
}
