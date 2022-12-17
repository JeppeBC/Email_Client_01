using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Email_Client_01.NewMail;

namespace Email_Client_01
{
    // Concrete Class for sending emails
    public class EmailSender : IEmailSender
    {

        // Method that sends an email instance, implements the method of IEmailSender
        public void sendEmail(Email email)
        {
            var emailMessage = CreateEmailMessage(email); // Construct a mime message from the given email
            Send(emailMessage);                           // Call private send method that sends the MimeMessage. 
        }


        // Creates a MimeMessage from a given Email instance. 
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

        // Method for sending a MimeMessage via SMTP. 
        private void Send(MimeMessage emailMessage)
        {

            using (var client = Utility.GetSmtpClient())
            {
                try
                {
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally // Ensure that no matter what, we dispose and disconnect the client. 
                {
                    client?.Disconnect(true);
                    client?.Dispose();
                }
            }
        }
    }

}
