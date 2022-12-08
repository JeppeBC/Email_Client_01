using Email_Client_01;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Drawing.Design;
using System.Reflection;
using System.Security.Cryptography;
using System.Timers;
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
        IList<IMailFolder> folders = null!;
        List<string> filterNames = new();
        private static Inboxes instance = null!;
        private bool clicked = false; // for double clicks handling
        ImapClient client;
        bool loadingFolders = false;
        bool loadingMessages = false;
        IMailFolder? folder; // current folder (mostly bookkeeping for loading)
        int filterCount = 0;

        // storing folders in a dictionary.
        Dictionary<string, string>? foldersMap;


        // private construtor as we employ singleton pattern
        private Inboxes(ImapClient client)
        {
            this.client = client;

            // Add timer as a way of enforcing that this client connection does not time out naturally.
            // Normally this can happen anywhere between 10, 15, 20, 25 or even 30 min. It is not very consistent. 
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 300000; // every 5 min, unit is milliseconds.
            aTimer.Enabled = true;

            folder = null;

            InitializeComponent();
            // todo change this to the idle, active and ui threads. Add checks that this connection does not break / expire
            RetrieveFolders();
        }

        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            // Dummy ping the server to prevent timing out
            client.NoOp();
        }


        // singleton pattern
        public static Inboxes GetInstance(ImapClient client)
        {
            // coalescing operator, return first non-null value; 
            return instance ??= new Inboxes(client);
        }

        // retrives all the folder names and add to the listbox
        // retrives all the folder names and add to the listbox
        private async void RetrieveFolders()
        {
            this.Cursor = Cursors.WaitCursor;
            loadingFolders = true;
            try
            {
                // Load in the folders from imap into a list
                folders = await client.GetFoldersAsync(new FolderNamespace('.', ""));

                // storing folders in a dictionary.
                foldersMap = new Dictionary<string, string>();


                foreach (var folder in folders)
                {
                    if (folder.Exists)
                    {
                        int unreadCount = 0;

                        // counting number of unread messages, the time this takes is noticeable #TODO
                        folder.Open(FolderAccess.ReadOnly);

                        foreach (var uid in folder.Search(SearchQuery.NotSeen))
                        {
                            unreadCount++;
                        }

                        var folderName = folder.FullName.Substring(folder.FullName.LastIndexOf("/") + 1);
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

                RetrieveMessagesFromFolder();
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
            finally
            {
                this.Cursor = Cursors.Default;
                loadingFolders = false;
            }

        }


        private SearchQuery? GetSearchQueryFromFilter(Filter filter)
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


        private string? GetFlags(IMessageSummary item)
        {
            string FlagString = "";

            if (item.Flags == null)
                return null;

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
            if (item.Envelope.Subject != null)
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
            if (item.Envelope.Sender.Count > 0 && !string.IsNullOrEmpty(item.Envelope.Sender[0].Name))
                return item.Envelope.Sender[0].Name;
            // else check for the first actual mail address of the sender(s), if non found return empty string;
            return item.Envelope.From.Mailboxes.FirstOrDefault()?.Address ?? "";
        }

        private string FormatInboxMessageText(IMessageSummary item)
        {
            string subject = getSubject(item);
            string? flags = GetFlags(item);
            string sender = GetSender(item);

            string result;
            if (flags == null)
            {
                result = sender + ": " + subject;
            }
            else
            {
                result = flags + sender + ": " + subject;
            }
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

        private IMailFolder GetCurrentFolder()
        {
            return client.GetFolder(Folders.SelectedValue.ToString());
        }




        // default parameters here because we send it another method that does not care.
        // retrives messages in a folder when it is double clicked. 
        // default parameters here because we send it another method that does not care.
        // retrives messages in a folder when it is double clicked. 
        private async void RetrieveMessagesFromFolder(object sender = null!, EventArgs e = null!)
        {
            Inbox.Items.Clear();

            if (loadingMessages) return; // ensure we don't start loading another batch of messages before we are done loading the current batch. Even prevents queueing of many folders that need to be loaded in

            this.Cursor = Cursors.WaitCursor;
            loadingMessages = true;
            try
            {
                Inbox.Items.Clear();

                folder = GetCurrentFolder();
                if (!folder.Exists) return;

                await folder.OpenAsync(FolderAccess.ReadWrite);

                var filepath = Path.Combine(Path.GetTempPath(), "filters.json");
                // if file does not exist
                if (!File.Exists(filepath))
                {
                    // create the file
                    File.Create(filepath).Close();
                }
                // Read the entire file and De-serialize to list of filters
                var FilterList = Utility.JsonFileReader.Read<List<Filter>>(filepath) ?? new List<Filter>();


                // optional TODO optimize the search queries so we can do these in batches.
                // Each call to search takes about 200ms per filter (of course depending on the number of mails it is filtering)
                foreach (var filter in FilterList)
                {
                    foreach (var f in folders) // find the IMailFolder from string DestinationFolder
                    {
                        if (f == folder) continue; // no point in moving to the same folder as we are currently in
                        if (f.FullName != filter.DestinationFolder || !f.Exists) continue;
                        var query = GetSearchQueryFromFilter(filter);
                        if (query == null) continue;

                        // find all the mails to be moved

                        // takes like 150-200 ms per filter, slightly (almost negligible) extra ammount from moving the mail.
                        var listUIDs = await folder.SearchAsync(query.And(SearchQuery.DeliveredAfter(Properties.Time.Default.Date)));

                        //TODO FIx this

                        // this takes slightly less than 200 ms, for loop included for a few mails. Mostly just the call to fecth. 
                        // fetch summaries before moving to different folder

                        foreach (var summary in folder.Fetch(listUIDs, MessageSummaryItems.Flags))
                        {
                            MessageBox.Show("SUMMARY");
                            updateFoldersUnreadCount(summary, TargetFolder: f);
                        }


                        await folder.MoveToAsync(listUIDs, f);

                        break;

                    }
                }

                // load in the mails after filtering
                messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);


                if (messageSummaries.Count <= 0)
                {
                    Inbox.Items.Add("This folder is empty!");
                }
                else if (folder.Attributes.HasFlag(FolderAttributes.Drafts))
                {
                    if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);
                    foreach (var item in messageSummaries.Reverse())
                    {
                        // Remove messages not flagged as draft.
                        if (item.Flags != null && !item.Flags.Value.HasFlag(MessageFlags.Draft))
                        {
                            folder.AddFlags(item.UniqueId, MessageFlags.Draft, true);
                            folder.Expunge();
                        }
                        Inbox.Items.Add(FormatDraftInboxText(item));
                    }
                }
                else
                {
                    foreach (var item in messageSummaries.Reverse())
                    {
                        Inbox.Items.Add(FormatInboxMessageText(item));
                    }
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
            finally
            {
                this.Cursor = Cursors.Default;
                loadingMessages = false;
                if (folder != GetCurrentFolder()) // we have since we started loading selected a new folder, so we load that in instead
                {
                    RetrieveMessagesFromFolder(sender, e);
                }
            }
        }


        // Read a specific method when doubleClicked
        // Read a specific method when doubleClicked
        private async void ReadMessage(object sender, EventArgs e)
        {
            // Get the specific message
            var messageId = (((ListBox)sender).SelectedIndex); // 2 parenthesis warns about missing ';' for some reason.
            if (messageId < 0) return; // failsafe
            var messageItem = messageSummaries[messageSummaries.Count - messageId - 1];

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (folder == null) folder = GetCurrentFolder();

                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                // if unread mail
                if (messageItem.Flags != null && !messageItem.Flags.Value.HasFlag(MessageFlags.Seen))
                {
                    // Mutate the message in the listbox, so it no longer says unread
                    string currentString = (string)Inbox.Items[messageId];
                    // Remove (UNREAD) if present in the inbox view
                    Inbox.Items[messageId] = currentString.Replace("(UNREAD)", "").Trim();

                    // Update the unread count
                    updateFoldersUnreadCount(messageItem, TargetFolder: folder);

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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }


        // delete element from inbox (does not put in trashfolder, deletes entirely)

        private void RefreshPage_Click(object sender, EventArgs e)
        {
            // #TODO make this only refresh the current folder to save time?
            RetrieveFolders();
        }


        private void Compose_Click(object sender, EventArgs e)
        {
            NewMail send_mail = new NewMail(client);
            send_mail.Show();
        }


        // TODO CAN WE RENAME THIS WINDOW FORMS SPECIFIC METHOD?
        private void Folders_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!loadingFolders && sender != null)
            {
                RetrieveMessagesFromFolder(sender, e);
            }
        }










        private async void search(string searchQuery)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (folder == null) folder = GetCurrentFolder();
                if (!folder.IsOpen) folder.Open(FolderAccess.ReadWrite);
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
                if (uids != null)
                {
                    ShowSearchResult(folder, uids);
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }



        private void ShowSearchResult(IMailFolder folder, IList<UniqueId> uids)
        {
            Inbox.Items.Clear();
            messageSummaries = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

            if (messageSummaries.Count <= 0)
            {
                Inbox.Items.Add("No results!");
            }
            foreach (var item in messageSummaries.Reverse())
            {
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
            // When LOADING MESSAGES when choosing filters
            var InitialFolder = Folders.SelectedItem;


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

            // the 5 commented lines here about the SelecteDIndexChanged and selectedItem is if we do not want to load in the messages as we select them
            /*          var selectedFolder = Folders.SelectedItem;
                        Folders.SelectedIndexChanged -= Folders_SelectedIndexChanged;*/
            string? DestinationFolder = "";
            if (!(Utility.RadioListBoxInput(Folders, ref DestinationFolder) == DialogResult.OK))
            {
                /*                Folders.SelectedIndexChanged += Folders_SelectedIndexChanged;*/
                return;
            }
            /*            Folders.SelectedIndexChanged += Folders_SelectedIndexChanged;
                        Folders.SelectedItem = selectedFolder;*/

            // this should mutate DestinationFolder to the stringified selected item of the data source
            // In our case the folder name is something like: "[Folder.Fullname, Displayed name]";
            if (string.IsNullOrEmpty(DestinationFolder)) return; // guard

            // Mutate the string to the "Folder.Fullname" part only
            DestinationFolder = DestinationFolder.Substring(1, DestinationFolder.IndexOf(",") - 1);


            string FilterName = "";
            if (!(Utility.InputBox("Add Filter", "Filter Name: ", ref FilterName) == DialogResult.OK))
            {
                return;
            }

            // Check if filter name is already in use
            if (filterNames.Contains(FilterName))
            {
                MessageBox.Show("A filter with the name " + FilterName + " already exists");
                return;
            }

            if (string.IsNullOrEmpty(FilterName))
            {
                MessageBox.Show("Please enter a valid filter name");
                return;
            }


            // add the search query locations
            List<string>? searchLocations = new List<string>();
            if (!SearchSubjectCheck.Checked && !SearchSenderCheck.Checked && !SearchContentCheck.Checked)
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


            // sort by destination folder, so we could filter all mails to the same folder in one swoop. 
            // not implemented currently but if we decide to go with batches. This is very efficient as the number of filters is small
            FilterList.Sort((x, y) =>
            {
                if (!string.IsNullOrEmpty(x.DestinationFolder))
                    return x.DestinationFolder.CompareTo(y.DestinationFolder);
                return 0;
            });


            // Update json file with new deserialized object
            Utility.JsonFileWriter.Write<List<Filter>>(filepath, FilterList);

            // Update the listbox immediately, so we don't have to rely on the guard and untick and tick the show checkbox
            FilterListbox.Items.Add(FilterName);
            filterCount += 1;

            // show the item if checkbox is checked
            if (ShowFiltersCheckbox.Checked && filterCount > 0)
            {
                if (FilterListbox.Items.Count != filterCount)
                {
                    // remove the entries of listbox and add all. Few so performance cost is negligible. 
                    FilterListbox.Items.Clear();
                    foreach (var filter in FilterList)
                    {
                        if (!string.IsNullOrEmpty(filter.Name))
                            FilterListbox.Items.Add(filter.Name);
                    }
                }
                FilterListbox.Visible = true;
                FilterLabel.Visible = true;
                RemoveFilterButton.Visible = true;
            }


            // LOADING MESSAGES: Load the initial folder back
            if (InitialFolder != Folders.SelectedValue)
            {
                Folders.SelectedItem = InitialFolder;
                RetrieveMessagesFromFolder();
            }
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

        private void Inbox_DrawItem(object sender, DrawItemEventArgs e)
        {
            //base.OnDrawItem(e);
            e.DrawBackground();
            e.DrawFocusRectangle();

            // failsafe
            if (e.Index < 0) return;
            String? item = Inbox.Items[e.Index].ToString();

            if (string.IsNullOrEmpty(item)) return; // another guard
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

            var idx = Inbox.IndexFromPoint(e.X, e.Y);
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
            if (e.Data == null) return; // guard
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void updateFoldersUnreadCount(IMessageSummary? movedItem, object? FoldersLBItem = null, IMailFolder? TargetFolder = null)
        {
            // Modify the folder unread counts
            if (foldersMap == null) return; // cannot modify something that is not there
            if (movedItem?.Flags != null && movedItem.Flags.Value.HasFlag(MessageFlags.Seen)) return; // does not need to update count as mail is already seen
            if (folder == null || folder.FullName == null) return; // this should never happen;


            // find the listbox folder which has the unread count from the IMailFolder


            // [Gmail]/Spam <--- ActualFolder.FullName

            string fullFolderName = "";

            // if we did not pass the folder listbox item, but we pass the targeted folder, we need to find the corresponding listbox item.
            if (FoldersLBItem == null && TargetFolder != null)
            {
                foreach (var item in Folders.Items)
                {
                    string? itemString = item.ToString();             //[[Gmail]/Spam, Spam (68)] <-- ListBox string entries 
                    if (string.IsNullOrEmpty(itemString)) continue;
                    var firstPart = itemString.Substring(1, itemString.IndexOf(',') - 1); // Get first part [Gmail]/Spam as these are the ActualFolder.FullName
                    if (firstPart == TargetFolder?.Name)   // If match with ActualFolder
                    {
                        fullFolderName = itemString; // The entire "[[Gmail]/Spam, Spam (68)]"
                        break;
                    }
                }

            }
            else if (FoldersLBItem != null)
            {
                string? folderItem = FoldersLBItem.ToString();
                if (!string.IsNullOrEmpty(folderItem)) fullFolderName = folderItem;
            }


            // Update currently open folder
            // Here displayedName will be of the form "[[Gmail]/Spam, Spam (68)]", and folder.FullName is the "[Gmail]/Spam" part.
            string displayedName = foldersMap[folder.FullName].Substring(folder.FullName.LastIndexOf(',') + 1); // get part after comma
            displayedName = displayedName.Remove(displayedName.Length - 1);  // remove the last bracket ]
            int number = int.Parse(displayedName.Split('(', ')')[1]); // fetch the number part
            number -= 1; // update
            foldersMap[folder.FullName] = (displayedName.Split('(', ')')[0] + "(" + number.ToString() + ")").Trim(); //remove extra spaces and form new string.


            // If these are not null, we are moving a mail to this folder. We update this too then.
            if (FoldersLBItem != null && TargetFolder != null)
            {
                displayedName = fullFolderName.Substring(fullFolderName.LastIndexOf(',') + 1); // We get "Spam (25)]", so we need to remove last character
                displayedName = displayedName.Remove(displayedName.Length - 1);
                number = int.Parse(displayedName.Split('(', ')')[1]); // get number between parentheses
                number += 1;
                foldersMap[TargetFolder.FullName] = (displayedName.Split('(', ')')[0] + "(" + number.ToString() + ")").Trim();
            }


            // Modifying listboxes with assigned datasources is very restricted. So we just reassign it. 
            Folders.DataSource = new BindingSource(foldersMap, null);


        }




        // what to do on successful drag and drop
        // what to do on successful drag and drop
        private async void Folders_DragDrop(object sender, DragEventArgs e)
        {

            // get mouse position relative to the folders listbox
            Point relativeCursorPoint = Folders.PointToClient(Control.MousePosition);
            var folderIdx = Folders.IndexFromPoint(relativeCursorPoint);
            if (folderIdx < 0 || folderIdx > Folders.Items.Count) return;

            // Currently we get the folder name like this as: "[Folder.Fullname, Displayed name]";
            string? fullFolderName = Folders.Items[folderIdx].ToString();
            if (string.IsNullOrEmpty(fullFolderName)) return;



            // we want the Folder.Fullname part
            // tokenize the string at "," and get the first part minus first character
            string folderName = fullFolderName.Substring(1, fullFolderName.IndexOf(",") - 1);

            // find index of message to be moved:
            if (e.Data == null) return;
            // get the passed index in mail of mail to move from inbox.
            var InboxIdx = (int)e.Data.GetData(e.Data.GetFormats()[0]);




            // the message selected in the inbox folder (message to be moved)
            var selectedItem = messageSummaries[messageSummaries.Count - 1 - InboxIdx];


            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (folder == null) folder = GetCurrentFolder();



                // find the folder that has "folderName", list of folders is stored as an attribute from when we loaded in the folders.
                foreach (var f in folders)
                {
                    // we find the correct folder and ensure that it is not either the special drafts- or sent folder.
                    if (f.FullName == folderName)
                    {
                        if(f.Attributes.HasFlag(FolderAttributes.Drafts) ||f.Attributes.HasFlag(FolderAttributes.Sent)) break;
                        
                        // Move mail
                        await folder.OpenAsync(FolderAccess.ReadWrite);
                        await folder.MoveToAsync(selectedItem.UniqueId, f);

                        // Ensure that if we move an unread mail that the displayed unread counts are updated in the respective folders;
                        updateFoldersUnreadCount(selectedItem, FoldersLBItem: Folders.Items[folderIdx], TargetFolder: f);
                        break;
                    }
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void Folders_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }


        // changes the text of the button to .... if and else...
        private void FilterCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (FilterCheckbox.Checked)
            {
                SearchButton.Text = "Add";
            }
            else
            {
                SearchButton.Text = "Search";
            }
        }

        private void ShowFiltersCheckbox_CheckStateChanged(object sender, EventArgs e)
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

            filterCount = FilterList.Count();


            if (ShowFiltersCheckbox.Checked && filterCount > 0)
            {
                if (FilterListbox.Items.Count != filterCount)
                {
                    // remove the entries of listbox and add all. Few so performance cost is negligible. 
                    FilterListbox.Items.Clear();
                    foreach (var filter in FilterList)
                    {
                        if (!string.IsNullOrEmpty(filter.Name))
                            FilterListbox.Items.Add(filter.Name);
                    }
                }
                FilterListbox.Visible = true;
                FilterLabel.Visible = true;
                RemoveFilterButton.Visible = true;
            }
            else
            {
                FilterListbox.Visible = false;
                FilterLabel.Visible = false;
                RemoveFilterButton.Visible = false;
            }
        }



        private void RemoveFilterButton_Click(object sender, EventArgs e)
        {
            // get the filter name
            if (FilterListbox.SelectedItem == null) return;
            string? filterName = FilterListbox.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(filterName))
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

                var filter = FilterList.Find(x => x.Name == filterName);
                if (filter != null && !string.IsNullOrEmpty(filter.Name))
                {
                    FilterList.Remove(filter);
                    filterCount -= 1;

                    // We need to serialize and write the updated version to file
                    Utility.JsonFileWriter.Write<List<Filter>>(filepath, FilterList);

                    // remove from listbox too
                    FilterListbox.Items.Remove(filter.Name);

                    // no more filters so we don't show these ui elements. 
                    if (FilterListbox.Items.Count < 1)
                    {
                        FilterListbox.Visible = false;
                        FilterLabel.Visible = false;
                        RemoveFilterButton.Visible = false;
                    }

                }
            }
        }


        private void Inbox_MouseUp(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                var itemIndex = Inbox.IndexFromPoint(e.Location);
                if (itemIndex < 0 || itemIndex > Inbox.Items.Count) return;

                Inbox.SelectedIndex = itemIndex;

                var ContextMenu = new ContextMenuStrip();
                ContextMenu.Items.Clear();


                var DeleteItem = new ToolStripMenuItem("Delete");
                DeleteItem.Click += new EventHandler(DeleteMail);
                ContextMenu.Items.Add(DeleteItem);

                var FlagItem = new ToolStripMenuItem("Flag");
                FlagItem.Click += new EventHandler(ToggleFlagMail);
                ContextMenu.Items.Add(FlagItem);

                var UnreadItem = new ToolStripMenuItem("Mark as Unread");
                UnreadItem.Click += new EventHandler(MarkMailAsUnread);
                ContextMenu.Items.Add(UnreadItem);

                Inbox.ContextMenuStrip = ContextMenu;
                Inbox.ContextMenuStrip.Show(Inbox, e.Location);

            }
        }

        private async void MarkMailAsUnread(object? sender, EventArgs e)
        {
            var msgIndex = Inbox.SelectedIndex;
            if(msgIndex < 0 || msgIndex > Inbox.Items.Count) // this should not happen
            {
                MessageBox.Show("No email to mark as unread");
                return;
            }
            var msg = messageSummaries[messageSummaries.Count - 1 - msgIndex];

            // Message is already unread 
            if (msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen)) return;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (folder == null) folder = GetCurrentFolder();
                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                // Mark the message as unread
                await folder.RemoveFlagsAsync(msg.UniqueId, MessageFlags.Seen, false);

                // instead of reloading the entire folder to capture this (UNREAD) change,
                // we just manually forcefully update that one element in the listbox (this will automatically happen on refresh)
                Inbox.Items[msgIndex] = "(UNREAD) " + Inbox.Items[msgIndex];

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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void DeleteMail(object? sender, EventArgs e)
        {
            var msgIndex = Inbox.SelectedIndex;
            // quick check so we do not waste unnecessary time to establish an imap connection in case of errors.
            if (msgIndex < 0 || msgIndex > Inbox.Items.Count) // dont know how this would appear, but just in case
            {
                MessageBox.Show("No email is selected for deletion.");
                return;
            }
            var msg = messageSummaries[messageSummaries.Count - 1 - msgIndex];
            DialogResult result = MessageBox.Show("Are you sure you want to delete this message? The action cannot be undone.", "Delete Message?", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (folder == null) folder = GetCurrentFolder();
                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                // Delete the message
                await folder.AddFlagsAsync(msg.UniqueId, MessageFlags.Deleted, true);
                await folder.ExpungeAsync();

                // Instead of calling RefreshCurrentFolder(), we just remove it from the current listbox as this is faster
                // and the next time we load the messages in, then it wont be there anyway as it is gone on the IMAP server side. 
                Inbox.Items.Remove(Inbox.SelectedItem);

                // if the message is unread, we update the unread count of the folder
                updateFoldersUnreadCount(msg, TargetFolder: folder);

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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private async void ToggleFlagMail(object? sender, EventArgs e)
        {
            var messageIndex = Inbox.SelectedIndex;
            if (messageIndex < 0) return; // failsafe
            var message = messageSummaries[messageSummaries.Count - 1 - messageIndex];

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (folder == null) folder = GetCurrentFolder();
                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                // toggle the flag
                if (message.Flags != null && message.Flags.Value.HasFlag(MessageFlags.Flagged))
                {
                    await folder.RemoveFlagsAsync(message.UniqueId, MessageFlags.Flagged, false);
                }
                else
                {
                    await folder.AddFlagsAsync(message.UniqueId, MessageFlags.Flagged, false);
                }

                RefreshCurrentFolder();
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void RefreshFoldersButton_Click(object sender, EventArgs e)
        {
            RetrieveFolders();
        }

        private async void CreateFolderButton_Click(object sender, EventArgs e)
        {
            try
            {
                string FolderName = "";
                if (!(Utility.InputBox("Create New Folder", "Name of Folder: ", ref FolderName) == DialogResult.OK))
                {
                    return;
                }

                //TODO easily extendable to subfolders

                // Could add waitcursors here but the call to create a toplevel folder is really fast...
                var toplevel = client.GetFolder(client.PersonalNamespaces[0]);
                var test = await toplevel.CreateAsync(FolderName, true);
                RetrieveFolders(); // Load in all folders again, this function is quite slow but how often do you create new folders...
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

        private async void DeleteFolderButton_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO easily extendable to subfolders

                // Guard
                if (foldersMap == null) return;

                DialogResult result = MessageBox.Show("Are you sure you want to delete the current folder? The action cannot be undone.", "Delete Folder?", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;

                if (folder == null) folder = GetCurrentFolder();

                // Could add waitcursors here but the call to delete a toplevel folder is really fast...
                var folderName = folder.FullName;
                await folder.DeleteAsync();
                // Select another folder, here we just select the first one.
                Folders.SelectedIndex = 0;
                


                // Remove the folder from the listbox instead of reloading all folders, which is computationally expensive. 
                // Folders have a datasource though, so we need to assign a new one to do this.
                foldersMap.Remove(folderName);
                Folders.DataSource = new BindingSource(foldersMap, null);
                

                // Check if we have a filter to the deleted folder, in which case we should delete this.
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

                // FilterList.ToList() here because we are modifying the list as we increment. If we don't do this we get the 
                // "Collection was modified, enumeration operation may not execute" exception.
                foreach(var filter in FilterList.ToList())
                {
                    if(filter.DestinationFolder == null || filter.DestinationFolder == folderName)
                    {
                        // delete the filter 
                        FilterList.Remove(filter);
                        filterCount -= 1;


                        if (string.IsNullOrEmpty(filter.Name)) return;
                        // remove from listbox too
                        FilterListbox.Items.Remove(filter.Name);

                        // no more filters so we don't show these ui elements. 
                        if (FilterListbox.Items.Count < 1)
                        {
                            FilterListbox.Visible = false;
                            FilterLabel.Visible = false;
                            RemoveFilterButton.Visible = false;
                        }
                    }
                }
                // We need to serialize and write the updated version to file
                Utility.JsonFileWriter.Write<List<Filter>>(filepath, FilterList);

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

        // need somewhere to display active filters and option to remove any active filters (does not work backwards, only works for future filtering)


        // TODO: Option to create new folders
        // TODO: option to delete folders

        // TODO: Change folder clicking from index changed to double click (so we can allow for selecting a folder without opening it)

    }
}