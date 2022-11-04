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
            textBox1.Text = message.From.ToString();

            //TODO: NEED TO CHECK IF THIS IS NULL
            textBox2.Text = message.Subject;


            // TODO: ADD HTML BODY HERE TO SHOW IMAGES AND SUCH?? LOW PRIO
            richTextBox1.Text = message.TextBody;

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
    }
}
