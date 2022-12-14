using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using System.Globalization;
using System.Runtime.InteropServices; // for the known folder stuff.
using System.Text;
using System.Xml.Linq;
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

        public enum KnownFolder
        {
            Contacts,
            Downloads,
            Favorites,
            Links,
            SavedGames,
            SavedSearches
        }

        public static class KnownFolders
        {
            private static readonly Dictionary<KnownFolder, Guid> _guids = new()
            {
                [KnownFolder.Contacts] = new("56784854-C6CB-462B-8169-88E350ACB882"),
                [KnownFolder.Downloads] = new("374DE290-123F-4565-9164-39C4925E467B"),
                [KnownFolder.Favorites] = new("1777F761-68AD-4D8A-87BD-30B759FA33DD"),
                [KnownFolder.Links] = new("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968"),
                [KnownFolder.SavedGames] = new("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"),
                [KnownFolder.SavedSearches] = new("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA")
            };

            public static string GetPath(KnownFolder knownFolder)
            {
                return SHGetKnownFolderPath(_guids[knownFolder], 0);
            }

            [DllImport("shell32",
                CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
            private static extern string SHGetKnownFolderPath(
                [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
                nint hToken = 0);
        }


        public string MakeBold(string text, string[] splitwords)
        {
            string returnValue = text;
            foreach (var word in splitwords)
            {
                returnValue = returnValue.Replace(word, @"\b" + word + @"\b0");
            }
            var finalString = new StringBuilder();
            finalString.Append(@"{\rtf1\ansi");
            finalString.Append(returnValue);
            finalString.Append(@"}");
            return finalString.ToString();
        }

        // Creates XML directory of dates, recieved, sent mails
        public static void CreateXML(IList<IMessageSummary> summaries)
        {
            var days = new List<DateTime>();
            var recieved_amount = new List<int>();
            var sent_amount = new List<int>();

            CultureInfo ci = CultureInfo.InvariantCulture;

            // Hardcoded temp value
            DateTime startdate = Properties.Settings.Default.dateLastLoaded;

            if (startdate==DateTime.MinValue)
            {
                startdate = DateTime.ParseExact("01-01-2010", "dd-MM-yyyy", ci);
            }

            // List of days, excluding current
            for (var dt = startdate; dt < DateTime.Today; dt = dt.AddDays(1))
            {
                days.Add(dt);
                recieved_amount.Add(0);
                sent_amount.Add(0);
            }

            foreach (var summary in summaries)
            {

                // Inside interval to be updated?
                if (summary.Date.UtcDateTime.Date >= days[0] && summary.Date.UtcDateTime.Date <= days.Last())
                {
                    for (int i = 0; i < days.Count; i++)
                    {
                        if (summary.Date.UtcDateTime.Date == days[i])
                        {
                            if (summary.Folder.Attributes.HasFlag(FolderAttributes.Sent))
                            {
                                sent_amount[i]++;
                            }
                            else
                            {
                                recieved_amount[i]++;
                            }
                        }
                    }
                }
            }

            string myTempFile = Path.Combine(Path.GetTempPath(), "root.xml");

            // Check if file exists, if not create start template file
            if (!File.Exists(myTempFile))
            {
                XDocument xmlFile = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"));
                xmlFile.Add(new XElement("Days"));
                xmlFile.Save(myTempFile);
            }


            XElement root = XElement.Load(myTempFile);

            for (int i = 0; i < days.Count; i++)
            {
                root.Add(new XElement("Day",
                    new XElement("Date", days[i].ToShortDateString()),
                    new XElement("Recieved", recieved_amount[i]),
                    new XElement("Sent", sent_amount[i])));
            }

            root.Save(myTempFile);

            // Set setting, prohibiting multiple checks of same day later
            Properties.Settings.Default.dateLastLoaded = DateTime.Today;
            Properties.Settings.Default.Save();
        }

    }
}
