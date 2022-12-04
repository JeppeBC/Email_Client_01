using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using Newtonsoft.Json;
using System.Runtime.InteropServices; // for the known folder stuff.


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

        public static async Task ReconnectAsync(ImapClient client)
        {
            // if somehow not connected or authenticated, try again
            if (!client.IsConnected)
            {
                await client.ConnectAsync(ImapServer, ImapPort, MailKit.Security.SecureSocketOptions.Auto);
            }
            if (!client.IsAuthenticated)
            {
                await client.AuthenticateAsync(username, password);
            }
        }

        public static async Task<SmtpClient> GetSmtpClient()
        {
            var client = new SmtpClient();
            await client.ConnectAsync(SmtpServer, SmtpPort, MailKit.Security.SecureSocketOptions.Auto);
            await client.AuthenticateAsync(username, password);

            return client;
        }



        public static class JsonFileReader
        {
            public static T? Read<T>(string filePath)
            {

                // TODO make this async/await??? Newtonsoft json does not support this?
                // so use the .Net version : System.Text.Json....
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
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


        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(36, 36, 372, 13);
            textBox.SetBounds(36, 86, 700, 20);
            buttonOk.SetBounds(228, 160, 160, 60);
            buttonCancel.SetBounds(400, 160, 160, 60);
            label.AutoSize = true;
            form.ClientSize = new Size(796, 307);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;

            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();

            value = textBox.Text;
            return dialogResult;

        }

        /*    // see https://stackoverflow.com/questions/5427020/prompt-dialog-in-windows-forms
            public static class Prompt
            {
                public static string ShowDialog(string text, string caption)
                {
                    Form prompt = new Form()
                    {
                        Width = 750,
                        Height = 400,
                        FormBorderStyle = FormBorderStyle.FixedDialog,
                        Text = caption,
                        StartPosition = FormStartPosition.CenterScreen
                    };
                    Label textLabel = new Label() { Left = 100, Top = 0, Text = text };
                    TextBox textBox = new TextBox() { Left = 100, Top = 100, Width = 800 };
                    Button confirmation = new Button() { Text = "Ok", Left = 700, Width = 200, Top = 140, DialogResult = DialogResult.OK };
                    confirmation.Click += (sender, e) => { prompt.Close(); };
                    prompt.Controls.Add(textBox);
                    prompt.Controls.Add(confirmation);
                    prompt.Controls.Add(textLabel);
                    prompt.AcceptButton = confirmation;

                    return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
                }
            }
    */







    }
}
