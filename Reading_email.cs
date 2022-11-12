using MailKit;
using MailKit.Net.Imap;
using MimeKit;


namespace Email_Client_01
{
    public partial class Reading_email : Form
    {
        MimeMessage message = null!;
        public Reading_email(MimeMessage msg)
        {
            InitializeComponent();
            message = msg;
            InitializeMessage();
        }

        private void InitializeMessage()
        {
            FromTextBox.Text = message.From.ToString();

            SubjectTextBox.Text = message.Subject;


            // TODO: ADD HTML BODY HERE TO SHOW IMAGES AND SUCH?? LOW PRIO?
            MessageTextBox.Text = message.TextBody;

            ToTextBox.Text = message.To.ToString(); 

            if(message.Cc.Any())
            {
                CCLabel.Visible = true;
                CCTextBox.Visible = true;
                CCTextBox.Text = message.Cc.ToString();
            }

            DateTextBox.Text = message.Date.ToString();


            if(message.Attachments.Any())
            {
                AttachmentsLabel.Visible = true;
                AttachmentListBox.Visible = true;
                DownloadAttachmentButton.Visible = true;

                foreach(var attachment in message.Attachments)
                {
                    var filename = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                    AttachmentListBox.Items.Add(filename);
                }
            }



            // TODO ADD CC:
/*            ccRecipientsTb.Text = message.Envelope.Cc.ToString();*/
        }

        // TODO assumes text body is not null, fix.
        private void Reply(MimeMessage message, bool replyToAll)
        {
            var reply = new MimeMessage();
            // reply to the sender of the message
            if (message.ReplyTo.Count > 0)
            {
                reply.To.AddRange(message.ReplyTo);
            }
            else if (message.From.Count > 0)
            {
                reply.To.AddRange(message.From);
            }
            else if (message.Sender != null)
            {
                reply.To.Add(message.Sender);
            }

            if (replyToAll)
            {
                // include all of the other original recipients - TODO: remove ourselves from these lists
                reply.To.AddRange(message.To);
                reply.Cc.AddRange(message.Cc);
            }

            // set the reply subject
            if (!message.Subject.StartsWith("Re:", StringComparison.OrdinalIgnoreCase))
                reply.Subject = "Re:" + message.Subject;
            else
                reply.Subject = message.Subject;

            // construct the In-Reply-To and References headers
            if (!string.IsNullOrEmpty(message.MessageId))
            {
                reply.InReplyTo = message.MessageId;
                foreach (var id in message.References)
                    reply.References.Add(id);
                reply.References.Add(message.MessageId);
            }

            // quote the original message text
            using (var quoted = new StringWriter())
            {
                var sender = message.Sender ?? message.From.Mailboxes.FirstOrDefault();

                quoted.WriteLine("On {0}, {1} wrote:", message.Date.ToString("f"), !string.IsNullOrEmpty(sender.Name) ? sender.Name : sender.Address);
                using (var reader = new StringReader(message.TextBody))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        quoted.Write("> ");
                        quoted.WriteLine(line);
                    }
                }

                reply.Body = new TextPart("plain")
                {
                    Text = quoted.ToString()
                };
            }

            new NewMail(reply).Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Reply
        private void button2_Click(object sender, EventArgs e)
        {
            Reply(message, false);
        }

        //ReplyAll
        private void button3_Click(object sender, EventArgs e)
        {
            Reply(message, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DownloadAttachmentButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the attachment index
                var idx = AttachmentListBox.SelectedIndex;
                var filename = AttachmentListBox.SelectedItem.ToString();

                // Utility.GetDownloadsPath is windows specific? see the implementation
                var downloadFolderPath = Utility.KnownFolders.GetPath(Utility.KnownFolder.Downloads);
                var path = Path.Combine(downloadFolderPath, filename);

                using (var stream = File.Create(path))
                {
                    var attachment = message.Attachments.ElementAt(idx);
                    if (attachment is MessagePart)
                    {
                        var part = (MessagePart)attachment;
                        part.Message.WriteTo(stream);
                    }
                    else
                    {
                        var part = (MimePart)attachment;
                        part.Content.DecodeTo(stream);
                    }
                }
                MessageBox.Show(filename + " was saved successfully to the downloads folder!");
            }
            catch
            {
                MessageBox.Show("No Attachment selected!");
            }
        }

        private void DownloadAllAttachmentsButton_Click(object sender, EventArgs e)
        {
            try
            {
                var downloadFolderPath = Utility.KnownFolders.GetPath(Utility.KnownFolder.Downloads);
                for (int i = 0; i < AttachmentListBox.Items.Count; i++)
                {
                    var attachment = message.Attachments.ElementAt(i);
                    var filename = AttachmentListBox.Items[i].ToString(); // listbox items are filenames
                    var path = Path.Combine(downloadFolderPath, filename);

                    using (var stream = File.Create(path))
                    {
                        if (attachment is MessagePart)
                        {
                            var part = (MessagePart)attachment;
                            part.Message.WriteTo(stream);
                        }
                        else
                        {
                            var part = (MimePart)attachment;
                            part.Content.DecodeTo(stream);
                        }
                    }
                }
                MessageBox.Show("Files were successfully downloaded and placed in downloads folder!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TrashButton_Click(object sender, EventArgs e)
        {

        }
    }
}
