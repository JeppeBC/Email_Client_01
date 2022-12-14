using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using MimeKit.Utils;
using static System.Net.Mime.MediaTypeNames;


namespace Email_Client_01
{
    public partial class Reading_email : Form
    {
        MimeMessage message = null!;
        ImapClient client;
        public Reading_email(MimeMessage msg, ImapClient client)
        {
            InitializeComponent();
            message = msg;
            InitializeMessage();
            this.client = client;
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


            DateTextBox.Text = message.Date.LocalDateTime.ToString();


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

                quoted.WriteLine("On {0}, {1} wrote:", message.Date.ToString("f"), !string.IsNullOrEmpty(sender?.Name) ? sender.Name : sender?.Address);
                if(!string.IsNullOrEmpty(message.TextBody))
                {
                    using (var reader = new StringReader(message.TextBody))
                    {
                        string? line;

                        while ((line = reader.ReadLine()) != null)
                        {
                            quoted.Write("> ");
                            quoted.WriteLine(line);
                        }
                    }

                }

                reply.Body = new TextPart("plain")
                {
                    Text = quoted.ToString()
                };
            }

            new NewMail(reply, client).Show();
        }

        private void Forward(MimeMessage message)
        {
            var ForwardedMessage = new MimeMessage();

            /*            ForwardedMessage.ReplyTo.AddRange(message.ReplyTo);*/

            var builder = new BodyBuilder();
            // Add attachments 
            foreach(var attachment in message.Attachments)
            {
                builder.Attachments.Add(attachment);
            }



            // quote the original message text
            using (var quoted = new StringWriter())
            {
                var sender = message.Sender ?? message.From.Mailboxes.FirstOrDefault();

                using (var text = new StringWriter())
                {
                    text.WriteLine();
                    text.WriteLine("-----Original Message-----");
                    text.WriteLine("From: {0}", message.From);
                    text.WriteLine("Sent: {0}", DateUtils.FormatDate(message.Date));
                    text.WriteLine("To: {0}", message.To);
                    text.WriteLine("Subject: {0}", message.Subject);
                    text.WriteLine();

                    text.Write(message.TextBody);

                    


                    builder.TextBody = text.ToString();
/*                    ForwardedMessage.Body = new TextPart("plain")
                    {
                        Text = text.ToString()
                    };*/
                }
            }

            ForwardedMessage.Body = builder.ToMessageBody();

            // set the reply subject
            if (!message.Subject.StartsWith("FWD:", StringComparison.OrdinalIgnoreCase))
                ForwardedMessage.Subject = "FWD:" + message.Subject;
            else
                ForwardedMessage.Subject = message.Subject;

            new NewMail(ForwardedMessage, client).Show();
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

        private async void DownloadAttachmentButton_Click(object sender, EventArgs e)
        {
            try
            {
                await Utility.ReconnectAsync(client);
                // Get the attachment index
                var idx = AttachmentListBox.SelectedIndex;
                var filename = AttachmentListBox.SelectedItem.ToString();

                if (string.IsNullOrEmpty(filename)) return; 

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
            catch(Exception ex)
            {
                // ImapProtocolExceptions often cause client disconnects, and IOExceptions always do.
                if(ex is ImapProtocolException || ex is IOException)
                {
                    await Utility.ReconnectAsync(client);
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void DownloadAllAttachmentsButton_Click(object sender, EventArgs e)
        {
            try
            {
                await Utility.ReconnectAsync(client);
                var downloadFolderPath = Utility.KnownFolders.GetPath(Utility.KnownFolder.Downloads);
                for (int i = 0; i < AttachmentListBox.Items.Count; i++)
                {
                    var attachment = message.Attachments.ElementAt(i);
                    var filename = AttachmentListBox.Items[i].ToString(); // listbox items are filenames

                    if (string.IsNullOrEmpty(filename)) return; // guard
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

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            Forward(message);
        }
    }
}
