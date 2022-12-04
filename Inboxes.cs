using Email_Client_01;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.DirectoryServices;
using System.Drawing.Design;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Linq;
using static Email_Client_01.Utility;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


// for distinguishing between types of click: https://learn.microsoft.com/en-us/dotnet/desktop/winforms/input-mouse/how-to-distinguish-between-clicks-and-double-clicks?view=netdesktop-6.0

namespace Email_Client_01
{
    public partial class Inboxes : Form
    {
        IList<IMessageSummary> messageSummaries = null!;
/*        IList<IMailFolder> folders = null!;*/

        List<string> folderNames = new();
        List<string> filterNames = new();
        private static Inboxes instance = null!;
        private bool clicked = false; // for double clicks handling

        // private construtor as we employ singleton pattern
        private Inboxes()
        {
            InitializeComponent();

            // todo change this to the idle, active and ui threads. Add checks that this connection does not break / expire
            RetrieveFolders();

        }

        // singleton pattern
        public static Inboxes GetInstance
        {
            // coalescing operator, return first non-null value; 
            get { return instance ??= new Inboxes(); }
        }

        // retrives all the folder names and add to the listbox
        private async void RetrieveFolders()
        {
            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {

                    // Load in the folders from imap into a list
                    var folders = await client.GetFoldersAsync(new FolderNamespace('.', ""));


                    // storing folders in a dictionary.
                    Dictionary<string, string> foldersMap = new Dictionary<string, string>();


                    var filepath = Path.Combine(Path.GetTempPath(), "filters.json");
                    // if file does not exist
                    if (!File.Exists(filepath))
                    {
                        // create the file
                        File.Create(filepath).Close();
                    }

                    // Read the entire file and De-serialize to list of filters
                    var FilterList = Utility.JsonFileReader.Read<List<Filter>>(filepath) ?? new List<Filter>();


                    foreach (var filter in FilterList)
                    {
                        foreach (var folder in folders)
                        {

                            if (!folder.Exists) continue;
                            foreach (var f in folders)
                            {
                                if (!f.Exists) continue;
                                if (f == folder)
                                    continue;

                                // Folder.fullnames are of the form "[Gmail]/Spam", we only want the part after '/'
                                var folderName = f.FullName.Substring(f.FullName.LastIndexOf("/") + 1);
                                if (filter.DestinationFolder == folderName)
                                {
                                    var query = GetSearchQueryFromFilter(filter);
                                    await folder.OpenAsync(FolderAccess.ReadWrite);
                                    foreach (var uid in folder.Search(query))
                                    {
                                        await folder.MoveToAsync(uid, f);
                                    }
                                    break;
                                }

                            }
                        }
                    }


                    foreach (var folder in folders)
                    {
                        if (folder.Exists)
                        {
                            int unreadCount = 0;

                            // counting number of unread messages, the time this takes is noticeable #TODO
                            folder.Open(FolderAccess.ReadOnly);


                            foreach(var uid in folder.Search(MailKit.Search.SearchQuery.NotSeen))
                            {
                                unreadCount++;
                            }
                        
                            var folderName = folder.FullName.Substring(folder.FullName.LastIndexOf("/") + 1);
                            folderNames.Add(folderName); // for bookkeeping without spinning up a client 
                            folderName += " (" + unreadCount + ")";
                            foldersMap.Add(key: folder.FullName, value: folderName); // add to dictionary. 
                            folder.Close();
                        }
                    }

                    // Designating a data source for the listbox. 
                    Folders.DataSource = new BindingSource(foldersMap, null);

                    // The value and keys
                    Folders.DisplayMember = "Value";
                    Folders.ValueMember = "Key";


                    RetrieveMessagesFromFolder(client.Inbox);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                    this.Cursor = Cursors.Default;
                }

            }
        }



/*
         // apply filter to the open folder
                            foreach(var filter in FilterList)
                            {
                                if(filter.DestinationFolder == folder.FullName)
                                {
                                    // do filtering. 
                                    var query = GetSearchQueryFromFilter(filter);
                                    foreach(var uid in folder.Search(MailKit.Search.SearchQuery.GMailRawSearch("qwerty")))
                                    {
                                        MessageBox.Show(uid.ToString());
                                        // find the destination folder of the filter.
                                        foreach (var f in folders)
                                        {
                                            if (f.FullName == filter.DestinationFolder)
                                            {
                                                MessageBox.Show("test");
                                                await f.OpenAsync(FolderAccess.ReadWrite);
        await f.MoveToAsync(uid, f);
        await f.CloseAsync();
                                                break;
                                            }
}
                                    }
                                }*/
                           

        private MailKit.Search.SearchQuery? GetSearchQueryFromFilter(Filter filter)
        {
            if (filter.SearchLocations == null)
            {
                return null;
            }
            if (filter.SearchLocations.Contains("Subject") && filter.SearchLocations.Contains("Sender") && filter.SearchLocations.Contains("Body"))
            {
                return SearchQuery.FromContains(filter.TargetString).Or(SearchQuery.SubjectContains(filter.TargetString)).Or(SearchQuery.BodyContains(filter.TargetString));
            }
            if (filter.SearchLocations.Contains("Subject") && filter.SearchLocations.Contains("Body"))
            {
                return SearchQuery.SubjectContains(filter.TargetString).Or(SearchQuery.BodyContains(filter.TargetString));
            }
            if (filter.SearchLocations.Contains("Sender") && filter.SearchLocations.Contains("Body"))
            {
                return SearchQuery.FromContains(filter.TargetString).Or(SearchQuery.BodyContains(filter.TargetString));
            }
            if (filter.SearchLocations.Contains("Sender") && filter.SearchLocations.Contains("Subject"))
            {
                return SearchQuery.FromContains(filter.TargetString).Or(SearchQuery.SubjectContains(filter.TargetString));
            }
            if (filter.SearchLocations.Contains("Sender"))
            {
                return SearchQuery.FromContains(filter.TargetString);
            }
            if (filter.SearchLocations.Contains("Subject"))
            {
                return SearchQuery.SubjectContains(filter.TargetString);
            }
            if (filter.SearchLocations.Contains("Body"))
            {
                return SearchQuery.BodyContains(filter.TargetString);
            }
            return null;
        }

        private string GetFlags(IMessageSummary item)
        {
            string FlagString = "";

            if (item.Flags.Value.HasFlag(MessageFlags.Flagged))
            {
                FlagString += "(FLAGGED) ";
            }

            if (item.Flags.Value.HasFlag(MessageFlags.Draft))
            {
                FlagString += "(DRAFT) ";
            }

            if (!(item.Flags.Value.HasFlag(MessageFlags.Seen)))
            {
                FlagString += "(UNREAD) ";
            }

            return FlagString;
        }

        private string getSubject(IMessageSummary item)
        {
            string subject = "";
            if(item.Envelope.Subject != null)
            {
                subject += item.Envelope.Subject;
            }

            // mutate the item in case of null
            else
            {
                item.Envelope.Subject = "<no subject>";
                subject += item.Envelope.Subject;
            }
            return subject;
        }

        private string GetSender(IMessageSummary item)
        {
            // if an alias is present, i.e. the name and not the actual mailaddress, then return that
            if (!string.IsNullOrEmpty(item.Envelope.Sender[0].Name))
                return item.Envelope.Sender[0].Name;
            // else check for the first actual mail address of the sender(s), if non found return empty string;
            return item.Envelope.From.Mailboxes.FirstOrDefault()?.Address ?? "";
        }

        private string FormatInboxMessageText(IMessageSummary item)
        {
            string subject = getSubject(item);
            string flags = GetFlags(item);
            string sender = GetSender(item);
            string result = flags + sender + ": " + subject;

            return result;
        }

        private string FormatDraftInboxText(IMessageSummary item)
        {
            return "(DRAFT) " + getSubject(item);
        }

       
        public static void RefreshCurrentFolder()
        {
            instance.RetrieveMessagesFromFolder();
        }

        private async Task<IMailFolder> GetCurrentFolder(ImapClient client)
        {
            var folder = await client.GetFolderAsync(Folders.SelectedValue.ToString());
            return folder;
        }


        // default parameters here because we send it another method that does not care.
        // retrives messages in a folder when it is double clicked. 
        private async void RetrieveMessagesFromFolder(object sender = null!, EventArgs e = null!)
        {
            Inbox.Items.Clear();

            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    Inbox.Items.Clear();

                    var folder = await GetCurrentFolder(client);
                    await folder.OpenAsync(FolderAccess.ReadOnly);


                    var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
                    messageSummaries = messages;

                    if (messages.Count <= 0)
                    {
                        Inbox.Items.Add("This folder is empty!");
                    }
                    else if(folder.Attributes.HasFlag(FolderAttributes.Drafts))
                    {
                        foreach (var item in messages.Reverse())
                        {
                            // Remove messages not flagged as draft.
                            if (!item.Flags.Value.HasFlag(MessageFlags.Draft))
                            {
                                folder.AddFlags(item.UniqueId, MessageFlags.Draft, true);
                                folder.Expunge();
                            }
                            Inbox.Items.Add(FormatDraftInboxText(item));
                        }
                    }
                    else
                    {
                        foreach (var item in messages.Reverse())
                        {
                            Inbox.Items.Add(FormatInboxMessageText(item));
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    client?.DisconnectAsync(true);
                    client?.Dispose();
                    this.Cursor = Cursors.Default;
                }
            }
        }


        // Read a specific method when doubleClicked
        private async void ReadMessage(object sender, EventArgs e)
        {
            // Get the specific message
            var messageId = (((ListBox)sender).SelectedIndex); // 2 parenthesis warns about missing ';' for some reason.
            if (messageId < 0) return; // failsafe
            var messageItem = messageSummaries[messageSummaries.Count - messageId - 1];

            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    // Add "Seen" flag to the message
                    var folder = await client.GetFolderAsync(messageItem.Folder.ToString());
                    await folder.OpenAsync(FolderAccess.ReadWrite);
                    await folder.AddFlagsAsync(messageItem.UniqueId, MessageFlags.Seen, true);

                    // Mutate the message in the listbox, so it no longer says unread
                    // TODO

                    // Get the MimeMessage from id:
                    MimeMessage msg = folder.GetMessage(messageItem.UniqueId);

                    //if the message is draft, open as draft!
                    if (folder.Attributes.HasFlag(FolderAttributes.Drafts))
                    {
                        new NewMail(msg, isDraft: true).Show();
                    }
                    else
                    {
                        new Reading_email(msg).Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    client?.DisconnectAsync(true);
                    client?.Dispose();
                    this.Cursor = Cursors.Default;
                }


            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }


        // delete element from inbox (does not put in trashfolder, deletes entirely)
        
        private async void DeleteButton_click(object sender, EventArgs e)
        {
            var msgIndex = Inbox.SelectedIndex;
            // quick check so we do not waste unnecessary time to establish an imap connection in case of errors.
            if(msgIndex < 0 || msgIndex > Inbox.Items.Count)
            {
                MessageBox.Show("No email is selected for deletion.");
                return;
            }

            var msg = messageSummaries[messageSummaries.Count - 1 - msgIndex];


            DialogResult result = MessageBox.Show("Are you sure you want to delete this message? The action cannot be undone.", "Delete Message?", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;

            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    var folder = await client.GetFolderAsync(msg.Folder.ToString());
                    await folder.OpenAsync(FolderAccess.ReadWrite);
                    await folder.AddFlagsAsync(msg.UniqueId, MessageFlags.Deleted, true);
                    await folder.ExpungeAsync();
                    RefreshCurrentFolder();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    client?.DisconnectAsync(true);
                    client?.Dispose();
                    this.Cursor = Cursors.Default;

                }
            }
        }

        private void RefreshPage_Click(object sender, EventArgs e)
        {
            // #TODO make this only refresh the current folder to save time?
            RetrieveFolders();  
        }

        private void Compose_Click(object sender, EventArgs e)
        {
            NewMail send_mail = new NewMail();
            send_mail.Show();
        }


        // TODO CAN WE RENAME THIS WINDOW FORMS SPECIFIC METHOD?
        private void Folders_SelectedIndexChanged(object sender, EventArgs e)
        {
            RetrieveMessagesFromFolder(sender, e);
        }



        private async void ToggleFlagButton_Click(object sender, EventArgs e)
        {
            var messageIndex = Inbox.SelectedIndex;
            if (messageIndex < 0) return; // failsafe
            var message = messageSummaries[messageSummaries.Count - 1 - messageIndex];

            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {

                    var folder = await client.GetFolderAsync(message.Folder.ToString());
                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    // toggle the flag
                    if (message.Flags.Value.HasFlag(MessageFlags.Flagged))
                    {
                        await folder.RemoveFlagsAsync(message.UniqueId, MessageFlags.Flagged, false); 
                    }
                    else
                    {
                        await folder.AddFlagsAsync(message.UniqueId, MessageFlags.Flagged, false);
                    }

                    RefreshCurrentFolder();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    client?.DisconnectAsync(true);
                    client?.Dispose();
                    this.Cursor = Cursors.Default;
                }
            }
                
        }



        private async void search(string searchQuery)
        {
            this.Cursor = Cursors.WaitCursor;
            using(var client = await Utility.GetImapClient())
            {
                try
                {
                    var folder = await GetCurrentFolder(client);

                    folder.Open(FolderAccess.ReadWrite);
                    IList<UniqueId> uids = null!;

                    // TODO make this work on tokens in strings too, i.e. "test" should catch "testing" and not just "RE:test" or "xxx test yyy" 
                    if (SearchSenderCheck.Checked && SearchSubjectCheck.Checked && SearchContentCheck.Checked)
                    {
                        uids = folder.Search(SearchQuery.FromContains(searchQuery).Or(SearchQuery.SubjectContains(searchQuery)).Or(SearchQuery.BodyContains(searchQuery)));
                       
                    }
                    else if (!SearchSenderCheck.Checked && SearchSubjectCheck.Checked && SearchContentCheck.Checked)
                    {
                        uids = folder.Search(SearchQuery.SubjectContains(searchQuery).Or(SearchQuery.BodyContains(searchQuery)));

                    }
                    else if (SearchSenderCheck.Checked && !SearchSubjectCheck.Checked && SearchContentCheck.Checked)
                    {
                        uids = folder.Search(SearchQuery.FromContains(searchQuery).Or(SearchQuery.BodyContains(searchQuery)));
                    }

                    else if (SearchSenderCheck.Checked && SearchSubjectCheck.Checked && !SearchContentCheck.Checked)
                    {
                        uids = folder.Search(SearchQuery.FromContains(searchQuery).Or(SearchQuery.SubjectContains(searchQuery)));
                    }
                    else if (SearchSenderCheck.Checked && !SearchSubjectCheck.Checked && !SearchContentCheck.Checked)
                    {
                        uids = folder.Search(SearchQuery.FromContains(searchQuery));
                    }
                    else if (!SearchSenderCheck.Checked && SearchSubjectCheck.Checked && !SearchContentCheck.Checked)
                    {
                        uids = folder.Search(SearchQuery.SubjectContains(searchQuery));
                    }
                    else if (!SearchSenderCheck.Checked && !SearchSubjectCheck.Checked && SearchContentCheck.Checked)
                    {
                        uids = folder.Search(SearchQuery.BodyContains(searchQuery));
                    }
                    else
                    {
                        MessageBox.Show("Please specify a search query");
                    }
                    if(uids != null)
                    {
                        ShowSearchResult(folder, uids);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    client?.DisconnectAsync(true);
                    client?.Dispose();
                    this.Cursor = Cursors.Default;
                }
            }

        }

        private void ShowSearchResult(IMailFolder folder, IList<UniqueId> uids)
        {
            Inbox.Items.Clear();
            IList<IMessageSummary> messages = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
            messageSummaries = messages;

            if (messages.Count <= 0)
            {
                toolStrip1.Visible = false;
                Inbox.Items.Add("No results!");
            }

            foreach (var item in messages.Reverse())
            {
                toolStrip1.Visible = true;
                Inbox.Items.Add(FormatInboxMessageText(item));
            }
        }



        // On pressing a key down, i.e. enter here
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchButton.PerformClick();
                e.SuppressKeyPress = true;
            }
        }


        private void AddFilter(string searchQuery)
        {
            // read existing json data
            var filepath = Path.Combine(Path.GetTempPath(), "filters.json");


            // if file does not exist
            if (!File.Exists(filepath))
            {
                // create the file
                File.Create(filepath).Close();
            }

            // Read the entire file and De-serialize to list of filters
            var FilterList = Utility.JsonFileReader.Read<List<Filter>>(filepath) ?? new List<Filter>();


            string DestinationFolder = "";
            if (!(Utility.InputBox("Add Filter", "Destination folder", ref DestinationFolder) == DialogResult.OK))
            {
                return;
            }
            string FilterName = "";
            if(!(Utility.InputBox("Add Filter", "Filter Name: ", ref FilterName) == DialogResult.OK))
            {
                return;
            }

            // Check if valid folder name was input?
            if (!folderNames.Contains(DestinationFolder))
            {
                MessageBox.Show(DestinationFolder + " is not an existing folder");
                return;
            }

            if (filterNames.Contains(FilterName))
            {
                MessageBox.Show("A filter with the name " + FilterName + " already exists");
                return;
            }

            // add the search query locations
            List<string>? searchLocations = new List<string>();
            if(!SearchSubjectCheck.Checked && !SearchSenderCheck.Checked && !SearchContentCheck.Checked)
            {
                MessageBox.Show("Please specify a location to query");
                return;
            }
            if (SearchSubjectCheck.Checked)
                searchLocations.Add("Subject");
            if (SearchSenderCheck.Checked)
                searchLocations.Add("Sender");
            if (SearchContentCheck.Checked)
                searchLocations.Add("Body");

            FilterList.Add(new Filter()
            {
                Name = FilterName,
                DestinationFolder = DestinationFolder,
                TargetString = searchQuery,
                SearchLocations = searchLocations,
            });


            // sort by destination folder, so we can filter all mails to the same folder in one swoop. 
            FilterList.Sort((x, y) => x.DestinationFolder.CompareTo(y.DestinationFolder));

            
            // Update json data string
            var newJson = JsonConvert.SerializeObject(FilterList);
            System.IO.File.WriteAllText(filepath, newJson);

        }
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchButton.Text))
            {
                string searchQuery = SearchTextBox.Text;
                if (FilterCheckbox.Checked)
                {
                    // add to list of filters
                    AddFilter(searchQuery);
                }
                else
                {
                    search(searchQuery);
                }
                SearchTextBox.ResetText();
            }

            else
            {
                MessageBox.Show("No search query");
            }
        }


        private void SearchTextBox_Click(object sender, EventArgs e)
        {
            SearchTextBox.ResetText();
        }



        private void Inbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ReadMessage(sender, e);
            
        }

        /* IMailFolder GetTrashFolder(ImapClient client, CancellationToken cancellationToken)
        {

            // this capability is generally dperecated
            if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
            {
                var trashFolder = client.GetFolder(SpecialFolder.Trash);
                return trashFolder;
            }

            else
            {
                string[] TrashFolderNames = { "Deleted", "Papirkurv", "Trash", "Bin", "Trashbin" };
                var personal = client.GetFolder(client.PersonalNamespaces[0]);

                foreach (var folder in personal.GetSubfolders(false, cancellationToken))
                {
                    foreach (var name in TrashFolderNames)
                    {
                        if (folder.Name == name)
                            return folder;
                    }
                }
            }

            return null;
        }*/

       /* private async void MoveToTrashButton_Click(object sender, EventArgs e)
        {
            var msgIndex = Inbox.SelectedIndex;

            // just a quick comparison to not use unnecessary time to establish an IMAP connection if nothing is selected.
            if (msgIndex < 0 || msgIndex > Inbox.Items.Count)
            {
                MessageBox.Show("Please select an email to move to the trash folder");
                return;
            }
            var msg = messageSummaries[messageSummaries.Count - 1 - msgIndex];


            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    var folder = await GetCurrentFolder(client);
                    IMailFolder trashFolder = GetTrashFolder(client, CancellationToken.None);

                    await folder.OpenAsync(FolderAccess.ReadWrite);


                    if (trashFolder == null)
                        return;
                    
                    await folder.MoveToAsync(msg.UniqueId, trashFolder);
                    RefreshCurrentFolder();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    client?.DisconnectAsync(true);
                    client?.Dispose();
                    this.Cursor = Cursors.Default;
                }
            }
        }
*/
        private void Inbox_DrawItem(object sender, DrawItemEventArgs e)
        {
            //base.OnDrawItem(e);
            e.DrawBackground();
            e.DrawFocusRectangle();

            // failsafe
            if (e.Index < 0)
                return;
            String item = Inbox.Items[e.Index].ToString();
            int indexOfChar = item.IndexOf(':');


            // draft folder, as we do not use semi colon on these:
            if (indexOfChar == -1)
            {
                e.Graphics.DrawString(item, new Font(Font, FontStyle.Bold), new SolidBrush(Color.Black), e.Bounds.X, e.Bounds.Y);
            }
            else
            {
                e.Graphics.DrawString(item.Substring(0, indexOfChar), new Font(Font, FontStyle.Bold), new SolidBrush(ForeColor), e.Bounds.X, e.Bounds.Y);
                if (item.Length > indexOfChar)
                {
                    int pos = TextRenderer.MeasureText(item.Substring(0, indexOfChar), new Font(Font, FontStyle.Bold)).Width;
                    e.Graphics.DrawString(item.Substring(indexOfChar), new Font(Font, FontStyle.Italic), new SolidBrush(Color.Black), pos, e.Bounds.Y);
                }
            }
        }



        private async void Inbox_MouseDown(object sender, MouseEventArgs e)
        {
            // ensure that we are able to distinguish double clicks from this type of mouse down. 
            if (clicked) return;
            clicked = true;
            await Task.Delay(SystemInformation.DoubleClickTime);
            if (!clicked) return;
            clicked = false;

            var idx = Inbox.IndexFromPoint(e.X,e.Y);
            if (idx < 0 || idx > Inbox.Items.Count) return;

            DragDropEffects dde1 = DoDragDrop(idx,
                    DragDropEffects.All);

            if (dde1 == DragDropEffects.All)
            {
                Inbox.Items.RemoveAt(Inbox.IndexFromPoint(e.X, e.Y));
            }
            Folders.DoDragDrop(idx, DragDropEffects.Copy);
        }

        private void Folders_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        // what to do on successful drag and drop
        private async void Folders_DragDrop(object sender, DragEventArgs e)
        {

            // get mouse position relative to the folders listbox
            Point relativeCursorPoint = Folders.PointToClient(Control.MousePosition);
            var folderIdx = Folders.IndexFromPoint(relativeCursorPoint);
            if (folderIdx < 0 || folderIdx > Folders.Items.Count) return;

            // Currently we get the folder name like this as: "[Folder.Fullname, Displayed name]";
            var folderName = Folders.Items[folderIdx].ToString();
            if (string.IsNullOrEmpty(folderName)) return;

            // we want the Folder.Fullname part
            // tokenize the string at "," and get the first part minus first character
            folderName = folderName.Substring(1, folderName.IndexOf(",") - 1);

            // find index of message to be moved:
            if (e.Data == null) return;
            // get the passed index in mail of mail to move from inbox.
            var InboxIdx = (int)e.Data.GetData(e.Data.GetFormats()[0]);


            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    
                    // find the unique id of message to be moved
                    var openFolder = GetCurrentFolder(client).Result;

                    var toplevel = client.GetFolder(client.PersonalNamespaces[0]);

                    // get list of all folders
                    var folders = await client.GetFoldersAsync(new FolderNamespace('.', ""));
                    // find the folder that has "folderName"
                    foreach(var folder in folders)
                    {
                        if (folder.FullName == folderName)
                        {
                            await openFolder.OpenAsync(FolderAccess.ReadWrite);
                            await openFolder.MoveToAsync(messageSummaries[messageSummaries.Count - 1 - InboxIdx].UniqueId, folder);
                            RefreshCurrentFolder();
                            break;
                        }
                    }
                    
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    client?.DisconnectAsync(true);
                    client?.Dispose();
                    this.Cursor = Cursors.Default;
                }

            }
        }

        private void Folders_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }


        // changes the text of the button to .... if and else...
        private void FilterCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if(FilterCheckbox.Checked)
            {
                SearchButton.Text = "Add";
            }
            else
            {
                SearchButton.Text = "Search";
            }
        }

        /* filters */




        // json ?? 
        // mailkit only new messages --> apply filters from json file
        // filtering from search query --> filters all mails
        // need somewhere to display active filters and option to remove any active filters (does not work backwards, only works for future filtering)


        // TODO: Option to create new folders
        // TODO: option to delete folders

        // TODO: Change folder clicking from index changed to double click (so we can allow for selecting a folder without opening it)

    }
}