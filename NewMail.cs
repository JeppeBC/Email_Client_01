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
using MailKit.Net.Imap;
using MailKit.Search;
using Org.BouncyCastle.Asn1.X509;
using System.Net.Mail;
using Microsoft.VisualBasic.ApplicationServices;

namespace Email_Client_01
{
    public partial class NewMail : Form
    {
        BindingList<Attachment> Attachments = new();

        // draft specific
        bool MailIsDraft = false;
        string? DraftID;
        ImapClient client;

        // Forward and reply to get the attachments.
        IEnumerable<MimeEntity>? nonlocalAttachments; 
        

        public NewMail(ImapClient client)
        {
            InitializeComponent();
            
            this.client = client;

            BindingList<Attachment> BL = new BindingList<Attachment>();
            BL.ListChanged += new ListChangedEventHandler(attachments_changed);
            Attachments = BL;
            AttachmentsListBox.DataSource = Attachments;
            AttachmentsListBox.DisplayMember = "name";

        }

        public NewMail(MimeMessage msg, ImapClient client)
        {
            InitializeComponent();

            this.client = client;
            RecipientTextBox.Text = msg.To.ToString();
            SubjectTextBox.Text = msg.Subject;
            MessageBodyTextBox.Text = msg.TextBody;
            CCTextBox.Text = msg.Cc.ToString();


            BindingList<Attachment> BL = new BindingList<Attachment>();
            BL.ListChanged += new ListChangedEventHandler(attachments_changed);
            Attachments = BL;
            AttachmentsListBox.DataSource = Attachments;
            AttachmentsListBox.DisplayMember = "name";

            nonlocalAttachments = msg.Attachments;

            foreach (var attachment in nonlocalAttachments)
            {
                Attachments.Add(new Attachment()
                {
                    name = attachment.ContentDisposition?.FileName,
                    filepath = "",
                });
            }



        }

        // could be combined with the constructor above. But having one more is no issue. 
        public NewMail(MimeMessage msg, bool isDraft, ImapClient client)
        {
            InitializeComponent();

            this.client = client;
            MailIsDraft = isDraft;
            RecipientTextBox.Text = msg.To.ToString();
            SubjectTextBox.Text = msg.Subject;
            MessageBodyTextBox.Text = msg.TextBody;
            CCTextBox.Text = msg.Cc.ToString();



            BindingList<Attachment> BL = new BindingList<Attachment>();
            BL.ListChanged += new ListChangedEventHandler(attachments_changed);
            Attachments = BL;
            AttachmentsListBox.DataSource = Attachments;
            AttachmentsListBox.DisplayMember = "name";
            nonlocalAttachments = msg.Attachments;
            if (isDraft)
            {
                DraftID = msg.MessageId;
            }

            foreach (var attachment in nonlocalAttachments)
            {
                Attachments.Add(new Attachment()
                {
                    name = attachment.ContentDisposition?.FileName,
                    filepath = "",
                });
            }

        }


        private void attachments_changed(object? sender, ListChangedEventArgs e)
        {
            bool show = Attachments.Count > 0; 
            AttachmentLabel.Visible = show;
            AttachmentsListBox.Visible = show;
            RemoveAttachmentButton.Visible = show;
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

                    // Shorten the file name to not include directories
                    string fileSelectedShort = fileSelected.Substring(fileSelected.LastIndexOf('\\') + 1) + " ";


                    Attachments.Add(new Attachment()
                    {
                        filepath = fileSelected,
                        name = fileSelectedShort,
                    });
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
                MessageBox.Show("Invalid subject");
                return;
            }

            var Content = MessageBodyTextBox.Text;


            List<string?> attachmentFilepaths = Attachments.Select(a => a.filepath).ToList();
            attachmentFilepaths.RemoveAll(fp => string.IsNullOrEmpty(fp)); // remove all the null cases, and empty cases that corresponds to the nonlocal attachments.
            Email email = new Email(To, CC, Subject, Content, attachmentFilepaths, nonlocalAttachments);

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
           
                if (AttachmentsListBox.SelectedItem == null) return;
                var attachment = Attachments[AttachmentsListBox.SelectedIndex];
                if (attachment == null) return;
                Attachments.Remove(attachment);

                // currently we are using the same Attachments list for both nonlocal and local attachments, only difference being the filepath is empty for nonlocal.
                // we should remove the mimeentity part if we remove a file that has empty filepath
                if (string.IsNullOrEmpty(attachment.name)) return; // guard
                if(attachment.filepath == "")
                {
                    string FileName = attachment.name;
                    // Cannot delete from an IEnumerable collection, so we use linq to instead create a new list with all but the 
                    // removed element
                    nonlocalAttachments = nonlocalAttachments?.Where(a => a.ContentDisposition.FileName != FileName).ToList();

                }
            }
            catch
            {
                MessageBox.Show("No Attachment selected!");
            }
        }


        public class Email
        {
            public bool isDraft = false;
            public List<MailboxAddress> To { get; set; }

            public List<MailboxAddress> CC { get; set; }
            public string Subject { get; set; }

            public string Content { get; set; }

            public List<string?> LocalAttachments { get; set; }

            public IEnumerable<MimeEntity>? nonlocalAttachments { get; set; }

            public Email(string[] to, string[]? cc, string subject, string content, List<string?> localAttachments, IEnumerable<MimeEntity>? nonLocal = null)
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
                LocalAttachments = localAttachments;
                nonlocalAttachments = nonLocal;
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
                foreach (var attachment in email.LocalAttachments)
                {
                    builder.Attachments.Add(attachment);
                }

                if(email.nonlocalAttachments != null)
                {
                    foreach(var attachment in email.nonlocalAttachments)
                    {
                        builder.Attachments.Add(attachment);
                    }
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
