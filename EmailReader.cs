using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    internal class EmailReader : IEmailReader
    {
        private readonly ImapClient client;
        private readonly IMailFolder folder;

        public EmailReader(ImapClient client, IMailFolder folder)
        {
            this.client = client;
            this.folder = folder;
        }

        public async Task ReadMessage(IMessageSummary messageItem)
        {
            try
            {
                await Utility.ReconnectAsync(client);
                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                // if unread mail (also cannot be draft)
                if (messageItem.Flags != null && !messageItem.Flags.Value.HasFlag(MessageFlags.Seen) && !messageItem.Flags.Value.HasFlag(MessageFlags.Draft))
                {
                    // Add read flag
                    await folder.AddFlagsAsync(messageItem.UniqueId, MessageFlags.Seen, true);
                }

                // Get the MimeMessage from id:
                MimeMessage msg = folder.GetMessage(messageItem.UniqueId);

                //if the message is draft, open as draft!
                if (folder.Attributes.HasFlag(FolderAttributes.Drafts))
                {
                    new NewMail(msg, isDraft: true, client).Show();
                }
                else
                {
                    new Reading_email(msg, client).Show();
                }
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
        }
    }
}
