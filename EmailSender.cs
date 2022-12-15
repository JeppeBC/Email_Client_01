using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Email_Client_01.NewMail;

namespace Email_Client_01
{
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

            if (email.nonlocalAttachments != null)
            {
                foreach (var attachment in email.nonlocalAttachments)
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

}
