using MailKit.Net.Imap;
using MailKit.Net.Smtp;
/*using Newtonsoft.Json;*/


namespace Email_Client_01
{
    internal class Utility
    {
        public static string? username;
        public static string? password;

        public static readonly string SmtpServer = "smtp.gmail.com";
        public static readonly int SmtpPort = 465;

        public static readonly string ImapServer = "imap.gmail.com";
        public static readonly int ImapPort = 993;


        public static async Task<ImapClient> GetImapClient()
        {
            var client = new ImapClient();

            await client.ConnectAsync(ImapServer, ImapPort, MailKit.Security.SecureSocketOptions.Auto);
            await client.AuthenticateAsync(username, password);

            return client;
        }

        public static async Task<SmtpClient> GetSmtpClient()
        {
            var client = new SmtpClient();
            await client.ConnectAsync(SmtpServer, SmtpPort, MailKit.Security.SecureSocketOptions.Auto);
            await client.AuthenticateAsync(username, password);

            return client;
        }

/*        public static class JsonFileReader
        {
            public static T? Read<T>(string filePath)
            {

                // TODO make this async/await??? Newtonsoft json does not support this?
                // so use the .Net version : System.Text.Json....
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }*/
    }
}
