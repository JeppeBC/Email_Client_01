using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using System.Globalization;
using System.Runtime.InteropServices; // for the known folder stuff.
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices; // for the known folder stuff.
using System.Windows.Forms.VisualStyles;
using MailKit.Security;

namespace Email_Client_01
{
    // Class that holds some nice variables (such as the filepath to filter.json or the gmail IMAP port) and also some nice to have functionality

    internal class Utility
    {
        public static string? username;
        public static string? password;

        public static readonly string SmtpServer = "smtp.gmail.com";
        public static readonly int SmtpPort = 465;

        public static readonly string ImapServer = "imap.gmail.com";
        public static readonly int ImapPort = 993;



        // Method to get an ImapClient. This uses the credentials stored in utility and immediately tries to connect and authorize.
        public static ImapClient? GetImapClient()
        {
            var client = new ImapClient();

            client.Connect(ImapServer, ImapPort, MailKit.Security.SecureSocketOptions.Auto);
            try
            {
                client.Authenticate(username, password);

            }
            catch(Exception ex)
            {
                if(ex is AuthenticationException)
                {
                    return null;
                }
            }
            return client;
        }


        // Reconnect / Reauthorize the given ImapClient if needed.
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

        public static void Reconnect(ImapClient client)
        {
            if (!client.IsConnected)
            {
                client.Connect(ImapServer, ImapPort, MailKit.Security.SecureSocketOptions.Auto);
            }
            if (!client.IsAuthenticated)
            {
                client.Authenticate(username, password);
            }
        }

        // Get an SMTP client. Like with the IMAP client method, this also immediately tries to connect and authorize using the credentials in utility.
        public static SmtpClient GetSmtpClient()
        {
            var client = new SmtpClient();
            client.Connect(SmtpServer, SmtpPort, MailKit.Security.SecureSocketOptions.Auto);
            client.Authenticate(username, password);

            return client;
        }


        // Class that allows for us read Json content. 
        // This method uses NewtonSoft.Json library.
        public static class JsonFileReader
        {

            // This method deserializes the contents of a json file given a "filepath"
            public static T? Read<T>(string filePath)
            {

                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        // A class to write (json) to a file
        public static class JsonFileWriter
        {

            // This method Serializes a deserialized json object and writes it to a file at "filepath".
            public static void Write<T>(string filePath, T jsonDeserializedObject)
            {
                var newJson = JsonConvert.SerializeObject(jsonDeserializedObject);
                System.IO.File.WriteAllText(filePath, newJson);
            }
        }



        // Below is a handy and pretty standard way of accessing the special directories of windows systems.
        // This uses the WinAPI method SHGetKnownFolderPath
        // See also: https://stackoverflow.com/questions/10667012/getting-downloads-folder-in-c
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



        // Windowforms does not really have any built in option to quickly prompt the user
        // This function basically does that. It prompts the user about something. The user can write something in a text field
        // and submit that. This does not return the user input (user input mutates the "value" parameter). Instead this returns
        // if the user presses "OK" or "Cancel"
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



        // Similarly to the Prompt functionality above, window forms does not really have a built-in way of prompting a user
        // to select a given item in a listbox. Below class implements this.

        // The class is heavily inspired by the top answer of
        // https://stackoverflow.com/questions/1015411/winforms-radiobuttonlist-doesnt-exist
        // however it is not 1-1 and we extended it quite a bit, making it more versatile. It now works with custom datasources
        // from classes and shows a chosen displaymember.
        public class RadioButtonList : ListBox
        {
            Size s;
            public RadioButtonList()
            {
                this.DrawMode = DrawMode.OwnerDrawFixed;
                using (var g = Graphics.FromHwnd(IntPtr.Zero))
                    s = RadioButtonRenderer.GetGlyphSize(
                        Graphics.FromHwnd(IntPtr.Zero), RadioButtonState.CheckedNormal);
            }

            protected override void OnDrawItem(DrawItemEventArgs e)
            {

                var text = (Items.Count > 0) ? GetItemText(Items[e.Index]) : Name;
                Rectangle r = e.Bounds; Point p;
                var flags = TextFormatFlags.Default | TextFormatFlags.NoPrefix;
                var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                var state = selected ?
                    (Enabled ? RadioButtonState.CheckedNormal :
                               RadioButtonState.CheckedDisabled) :
                    (Enabled ? RadioButtonState.UncheckedNormal :
                               RadioButtonState.UncheckedDisabled);
                if (RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                {
                    p = new Point(r.Right - r.Height + (ItemHeight - s.Width) / 2,
                        r.Top + (ItemHeight - s.Height) / 2);
                    r = new Rectangle(r.Left, r.Top, r.Width - r.Height, r.Height);
                    flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                }
                else
                {
                    p = new Point(r.Left + (ItemHeight - s.Width) / 2,
                    r.Top + (ItemHeight - s.Height) / 2);
                    r = new Rectangle(r.Left + r.Height, r.Top, r.Width - r.Height, r.Height);
                }
                var bc = selected ? (Enabled ? SystemColors.Highlight :
                    SystemColors.InactiveBorder) : BackColor;
                var fc = selected ? (Enabled ? SystemColors.HighlightText :
                    SystemColors.GrayText) : ForeColor;
                using (var b = new SolidBrush(bc))
                    e.Graphics.FillRectangle(b, e.Bounds);
                RadioButtonRenderer.DrawRadioButton(e.Graphics, p, state);
                TextRenderer.DrawText(e.Graphics, text, Font, r, fc, bc, flags);
                e.DrawFocusRectangle();
                base.OnDrawItem(e);
            }
            [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public override SelectionMode SelectionMode
            {
                get { return System.Windows.Forms.SelectionMode.One; }
                set { }
            }
            [Browsable(false), EditorBrowsable(EditorBrowsableState.Never),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public override int ItemHeight
            {
                get { return (this.Font.Height + 2); }
                set { }
            }
            [EditorBrowsable(EditorBrowsableState.Never), Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public override DrawMode DrawMode
            {
                get { return base.DrawMode; }
                set { base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed; }
            }


            // takes listbox, translates it into a radiolistbox and prompts the user to select one item.
            // SelectedFolder is mutated to match the user's choice. DialogResult returned. 
            // DisplayMember is for class objects if they are stored in the list box, for specifying which attribute to show
            public static DialogResult Input(ListBox lb, string displayMember, string valueMember, ref string? selectedObject)
            {
                Form form = new Form();
                Label label = new Label();
                RadioButtonList RBL = new RadioButtonList();
                Button buttonOk = new Button();
                Button buttonCancel = new Button();

                RBL.Height = lb.ItemHeight * (lb.Items.Count + 1);
                RBL.Width = lb.Width;
                // todo set maximum limit to protect form?

                RBL.DataSource = lb.DataSource;

                // Currently just hardcoded
                RBL.DisplayMember = displayMember;
                RBL.ValueMember = valueMember;

                form.Text = "Folders";
                label.Text = "Please select a destination folder: ";

                buttonOk.Text = "OK";
                buttonCancel.Text = "Cancel";
                buttonOk.DialogResult = DialogResult.OK;
                buttonCancel.DialogResult = DialogResult.Cancel;


                RBL.SetBounds(25, label.Height * 2, RBL.Width, RBL.Height);
                buttonOk.SetBounds(400, 160, 160, 60);
                buttonCancel.SetBounds(400, 240, 160, 60);


                label.AutoSize = true;
                form.ClientSize = new Size((int)(Math.Max(RBL.Width, label.Width) * 2), (int)((RBL.Height + label.Height) * 1.25));
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.MinimizeBox = false;
                form.MaximizeBox = false;

                form.Controls.AddRange(new Control[] { label, RBL, buttonOk, buttonCancel });
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                DialogResult dialogResult = form.ShowDialog();


                selectedObject = (string?)RBL.SelectedValue;
                return dialogResult;
            }

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

            if (startdate == DateTime.MinValue)
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
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff + 6).Date;
        }
    }

}
