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

    // Form shown when writing a new mail.
    public partial class NewMail : Form
    {
        BindingList<Attachment> Attachments = new();

        // draft specific
        bool MailIsDraft = false;
        string? DraftID;
        ImapClient client;

        // Forward and reply to get the attachments.
        IEnumerable<MimeEntity>? nonlocalAttachments; 
        

        // When writing a mail from scratch, this constructor is used (e.g. by pressing the "compose" button)
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

        // if we already have a skeleton for a mail, we use this constructor.
        // E.g. when forwarding a mail, the content should stay. 
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
        // This constructor is used for when we have drafts, opening drafts is slightly different. 
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


        // Dynamically checks whenever the attachments change if we need to display the associated GUI elements.
        private void attachments_changed(object? sender, ListChangedEventArgs e)
        {
            bool show = Attachments.Count > 0; 
            AttachmentLabel.Visible = show;
            AttachmentsListBox.Visible = show;
            RemoveAttachmentButton.Visible = show;
        }

        // When hovering over the recipients field, a helpful message is displayed.
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


        // For attaching files to a mail. Opens the windows directory and allows the user to pick one. 
        // That is, the file must be stored locally on the user's computer!
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



        // Get the recipients from the textbox field. 
        // Filtered into a list of strings. Instead of one long, concatenated string. 
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

        // Get list of CCs instead of having one concatenated blob. 
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

        // Method for getting the subject. If there is no subject, it prompts the user if this is fine. 
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


            // we expect strings of the form:        "Alias <MailAddress>"          <-- is a mailbox address
            // and want to return only MailAddress
            // Below method does that
        private string FilterMailboxAddress(string s)
        {
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

        // Check if the recipients are seemingly valid. 
        // We use a separate library by jeffrey stedfast, who created MailKit to do this. 
        private bool validateRecipientArray(string[] recipients)
        {

            // if we reply to a mail an internet address is inserted i.e. of the form 
            // "Alias" <MailAddress>, ....


            foreach (string recipient in recipients)
            {
                string r = recipient.Replace(",", "");

                r = FilterMailboxAddress(r);

                if (!EmailValidator.Validate(r)) // use jstedfast 
                {
                    return false;
                }
            }
            SendButton.Enabled = true;
            return true;
        }



        // WHen we click the send_mail button this runs. 
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
            
            

            // Construct an email and send that email.
            Email email = new Email(To, CC, Subject, Content, attachmentFilepaths, nonlocalAttachments);
            IEmailSender s = new EmailSender();
            s.sendEmail(email);

            // If the mail was a mail from the drafts folder, we must delete it.
            // To do this, we must use IMAP connection. 
            if(MailIsDraft)
            {
                // delete the mail from drafts folder
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    await Utility.ReconnectAsync(client);
                    var draftsFolder = client.GetFolder(SpecialFolder.Drafts);
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


        // When the Recipients textbox registers changes, this method runs
        // Checks if the recpients appear to be valid, if so highlight in green and enable send button
        // else highlight in red and disable send button.
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


        // CC validation works in the exact same way as Recipient validation above, only for the CC field instead. 
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


        // Method for removing attachments in the new mail that is currently being composed. 
        private async void RemoveAttachmentButton_Click(object sender, EventArgs e)
        {
            try
            {
                await Utility.ReconnectAsync(client);
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

        // Translates the current message state (from the form) into a MimeMessage and returns this. 
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



        // Save the current MessageState as a draft and put in drafts folder.
        private async void SaveDraftButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                await Utility.ReconnectAsync(client);

                MimeMessage message = BuildDraftMessage();
                IMailFolder? draftsFolder = client.GetFolder(SpecialFolder.Drafts);
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
