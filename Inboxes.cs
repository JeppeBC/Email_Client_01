using Email_Client_01;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using MimeKit.Cryptography;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


// for distinguishing between types of click: https://learn.microsoft.com/en-us/dotnet/desktop/winforms/input-mouse/how-to-distinguish-between-clicks-and-double-clicks?view=netdesktop-6.0

namespace Email_Client_01
{
    public partial class Inboxes : Form
    {
        IList<IMailFolder> folders = null!;                 // local storage of all the IMAP folders
        IMailFolder? folder;                                // current folder (mostly bookkeeping for loading)
        IList<IMessageSummary> messageSummaries = null!;    // local storage of the messages in currently open folder


        private static Inboxes instance = null!; // for singleton pattern on the inbox form. 
        private bool clicked = false; // for double clicks handling
        private ImapClient client;                      // store the client connection so we can reuse this for noticable speed ups. 

        // Below are variables to check if the client is currently in use in different ways. 
        bool ClientInUse = false;
        bool loadingFolders = false;
        bool loadingMessages = false;




        // List of filters, changing this variable automatically updates the associated filterListbox
        // as we link the datasources
        BindingList<Filter> FilterList;

        // Similarly this is a list of folders that when changed automatically updates the listbox they reside in. 
        BindingList<Folder> FolderList;

        // We have an extra client connection running that simply observes if we have any incoming mails
        // if so we load it in -- observer pattern is used for this. 
        IdleClient idle;


        // private construtor as we employ singleton pattern
        private Inboxes(ImapClient client)
        {
            InitializeComponent();

            this.client = client;

            var FilterJsonPath = Path.Combine(Path.GetTempPath(), "filters.json");

            // filepath of filter.json file
            // if file does not exist
            if (!File.Exists(FilterJsonPath))
            {
                // create the file
                File.Create(FilterJsonPath).Close();
            }
            // Read the entire file and De-serialize to list of filters
            var Filters = new BindingList<Filter>(Utility.JsonFileReader.Read<List<Filter>>(FilterJsonPath) ?? new List<Filter>());



            // Update the jsonfile when form closes
            this.FormClosed += (s, args) =>
            {
                if (FilterList == null) return;
                Utility.JsonFileWriter.Write<List<Filter>>(FilterJsonPath, FilterList.ToList());
            };


            // Below links the data sources of local lists with the listboxes. 
            BindingList<Filter> BindingListFilters = new BindingList<Filter>(Filters);
            BindingListFilters.ListChanged += new ListChangedEventHandler(filters_changed);
            FilterList = BindingListFilters;
            FilterListbox.DataSource = FilterList;
            FilterListbox.DisplayMember = "Name";

            BindingList<Folder> BindingListFolders = new BindingList<Folder>();
            FolderList = BindingListFolders;
            Folders.DataSource = FolderList;
            Folders.DisplayMember = "ListBoxName";

            // Add a timer so we can ping the server at given intervals (if we are not currently using the IMAP client already)
            // This enforces that the client does not naturally time out, which normally can happen
            // anywhere between 10, 15, 20, 25 or even 30 min. It is not very consistent. 
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(PingServerOnTimer);
            aTimer.Interval = 120000; // every 2 min, unit is milliseconds. 
            aTimer.Enabled = true;


            folder = null;


            // Setup the idle client, attach the inbox as subscriber. 
            idle = new IdleClient(Utility.ImapServer, Utility.ImapPort, MailKit.Security.SecureSocketOptions.Auto, Utility.username!, Utility.password!);
            idle.InboxMessages.Attach(this);

            // Load in all the folders
            RetrieveFolders(); 


        }


        // Method that runs on a timer and pings the server to ensure no natural time outs. 
        private void PingServerOnTimer(object? sender, ElapsedEventArgs e)
        {
            if (ClientInUse || loadingFolders || loadingMessages) return;

            // Dummy ping the server to prevent timing out
            ClientInUse = true;
            client.NoOp(); // this effectively takes no time but just in case
            ClientInUse = false;
        }



        // singleton pattern
        public static Inboxes GetInstance(ImapClient client)
        {
            // coalescing operator, return first non-null value; 
            return instance ??= new Inboxes(client);
        }


        // retrives all the folder names and add to the folder listbox
        // Finally this loads in the messages of the default client.Inbox
        private async void RetrieveFolders()
        {
            if (ClientInUse) return;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);
                // Load in the folders from imap into a list
                folders = await client.GetFoldersAsync(new FolderNamespace('.', ""));

                
                // Clearing the listbox causes the selected index to change, we dont want the event handler to run while loading in folders.
                Folders.SelectedIndexChanged -= Folders_SelectedIndexChanged; 
                FolderList.Clear();

                foreach (var folder in folders)
                {
                    if (folder.Exists)
                    {
                        // counting number of unread messages, the time this takes is noticeable #TODO
                        folder.Open(FolderAccess.ReadOnly);

                        // Names are of the form "[Gmail]/Spam" -- get the "Spam" part only
                        var folderName = folder.FullName.Substring(folder.FullName.LastIndexOf("/") + 1);

                        if (!SpecialFolders.isFolderUnreadBlacklisted(folder))
                        {
                            int unreadCount = 0;
                            foreach (var uid in folder.Search(SearchQuery.NotSeen))
                            {
                                unreadCount++;
                            }
                            folderName += " (" + unreadCount + ")";
                        }
                        // show the number of items in the following folders:
                        if(SpecialFolders.isFolderDisplayAllCount(folder))
                        {
                            if (folder.Count > 0) folderName += " (" + folder.Count + ")";
                        }


                        // Add the folder to local list (linked with datasource of listbox)
                        FolderList.Add(new Folder()
                        {
                            FullName = folder.FullName,
                            ListBoxName = folderName
                        }); ;
                        folder.Close();
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
            finally // ensure these are run no matter what afterwards
            {
                this.Cursor = Cursors.Default;
                Folders.SelectedIndexChanged += Folders_SelectedIndexChanged; 
                ClientInUse = false;
                idle.RunAsync(); // idle client starts empty initially, so it immediately detects changes and loads everything in for us by invoking reload()

            }

        }


        // A method that returns the currently selected folder. 
        private IMailFolder GetCurrentFolder()
        {

            if (Folders.SelectedIndex < 0)
            {
                return client.Inbox; // guard for when refresh, just 

            }
            return client.GetFolder(FolderList[Folders.SelectedIndex].FullName);
        }



        // This method triggers whenever the idle client detects a new incoming mail.
        // Currently, we just refresh the current folder, which applies the filters and check for unread counts.
        // Atm. we are not really running into bad performance issues doing it this way, but this could definitely be optimized
        // if performance was an issue. 
        public void Reload()
        {
            RetrieveMessagesFromFolder();
        }


        // Loads in the messages of the current folder
        private async void RetrieveMessagesFromFolder()
        {
            Inbox.Items.Clear();
            if (loadingMessages) return; // ensure we don't start loading another batch of messages before we are done loading the current batch. Even prevents queueing of many folders that need to be loaded in
            this.Cursor = Cursors.WaitCursor;
            loadingMessages = true;



            try
            {
                await Utility.ReconnectAsync(client); // if timed out, reconnect

                folder = GetCurrentFolder();
                if (!folder.Exists) return;

                await folder.OpenAsync(FolderAccess.ReadWrite);



                // Takes less than 200 ms per filter that can move mails from that given folder.
                Filterer filterer = new Filterer(FilterList.ToList(), folders, client);
                Dictionary<IMailFolder, int>? MovedMessages = await filterer.FilterFolder(folder);
                if (MovedMessages != null) // null if something went wrong or current folder is blacklisted from being filtered 
                {
                    // entry is of the form (key: FolderMailWasMovedTo, value: NumberOfMailsMoved)
                    foreach (var entry in MovedMessages)
                    {
                        IncrementFolderCount(entry.Key, decrement: false, entry.Value);
                    }
                }





                // load in the mails after filtering
                this.messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.EmailId | MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
                int unreadCount = 0;


                // Add the mails to listboxes (show them). We could similarly use a binding list here for the inbox messages,
                // which would make the code more readable. However we would need to add a lot of attributes to the "InboxMessage" class in that case
                // To keep track of the flags, different type of mails etc, we decided to not do it that way. 
            
                if (FilterUnreadCheckbox.Checked && !SpecialFolders.isFolderWithoutUnreads(folder))
                {
                    unreadCount = ShowUnreadMails();
                }
                else if (messageSummaries.Count <= 0)
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
                        Inbox.Items.Add(TextFormatter.FormatDraftInboxText(item));
                    }
                }
                else
                {

                    foreach (var item in messageSummaries.Reverse())
                    {
                        Inbox.Items.Add(TextFormatter.FormatInboxMessageText(item));

                        // make sure the folder count is correct
                        if (SpecialFolders.isFolderUnreadBlacklisted(folder)) continue;
                        if (item.Flags != null && !item.Flags.Value.HasFlag(MessageFlags.Seen)) unreadCount += 1;
                    }
                }
                // display the unread count if not blacklisted
                if (SpecialFolders.isFolderDisplayAllCount(folder)) UpdateFolderCount(folder, folder.Count);
                else if (!SpecialFolders.isFolderUnreadBlacklisted(folder) && unreadCount > -1) UpdateFolderCount(folder, unreadCount);

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
            }
        }

        // If we double click on a mail this method runs, opens the mail clicked
        private void OpenMail_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ClientInUse) return;
            if (folder == null) folder = GetCurrentFolder();


            // Index of mail in inbox. 
            var MailIdx = (((ListBox)sender).SelectedIndex);
            // Get the specific message
            if (MailIdx < 0) return; // failsafe
            var Message = messageSummaries[messageSummaries.Count - 1 - MailIdx];



            ClientInUse = true;
            this.Cursor = Cursors.WaitCursor;
            IEmailReader reader = new EmailReader(client, folder);
            reader.ReadMessage(Message);
            ClientInUse = false;
            this.Cursor=Cursors.Default;

            // If message is unread
            if(Message.Flags != null && !Message.Flags.Value.HasFlag(MessageFlags.Seen))
            {
                // Mutate the message in the listbox, so it no longer says unread
                string currentString = (string)Inbox.Items[MailIdx];
                // Remove (UNREAD) if present in the inbox view
                Inbox.Items[MailIdx] = currentString.Replace("(UNREAD)", "").Trim();

                // Update the unread count
                IncrementFolderCount(folder, decrement: true);
            }

        }

        // When clicking compose button to write a new email, this method runs.
        private void Compose_Click(object sender, EventArgs e)
        {
            NewMail send_mail = new NewMail(client);
            send_mail.Show();
        }


        // Whenever a different folder is selected this method runs.
        // If not currently busy loading in all the folders, it fetches all the messages
        // Of that folder and displays them. (applies filters too)
        private void Folders_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!loadingFolders && sender != null)
            {
                RetrieveMessagesFromFolder();
            }
        }

        private List<string> GetSearchLocations()
        {
            List<string> locations = new();
            if (SearchSenderCheck.Checked) locations.Add("Sender");
            if (SearchSubjectCheck.Checked) locations.Add("Subject");
            if (SearchContentCheck.Checked) locations.Add("Body");
            return locations;
        }

        // Method that searches the currently open folder's messages in locations
        // based on which checkboxes are checked. It searches for the string specified in the search text box. 
        // We could and probably should refactor this method quite a bit, and should probably separate the GUI logic (which is embedded in this method)
        // from the search logic. 


        private async void search(string searchQuery)
        {
            this.Cursor = Cursors.WaitCursor;
            if (ClientInUse) return;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);
                if (folder == null) folder = GetCurrentFolder();
                if (!folder.IsOpen) folder.Open(FolderAccess.ReadWrite);


                
                Searcher s = new Searcher(folder);
                List<string> locs = GetSearchLocations();
                if(locs.Count <= 0)
                {
                    MessageBox.Show("Please specify where to search");
                    return;
                }
                var uids = s.Search(GetSearchLocations(), searchQuery);
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
                ClientInUse = false;
            }
        }


        // Method that displays a number of mails corresponding to a list of UIDs of a folder.
        // This list of uids is typically generated from the search method.
        private void ShowSearchResult(IMailFolder folder, IList<UniqueId> uids)
        {
            Inbox.Items.Clear();
            if (!folder.IsOpen) return;
            if (loadingMessages) return;

            // Get the actual messages from list of uids. 
            messageSummaries = folder.Fetch(uids, MessageSummaryItems.EmailId | MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);


            if (messageSummaries.Count <= 0)
            {
                Inbox.Items.Add("No results!");
            }
            foreach (var item in messageSummaries.Reverse())
            {
                Inbox.Items.Add(TextFormatter.FormatInboxMessageText(item));
            }
        }



        // On pressing a key down, i.e. enter here (must be in searchfield)
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchButton.PerformClick();
                e.SuppressKeyPress = true;
            }
        }


        // Method for adding a new filter (locally, we write it to json file at program closure).
        private void AddFilter(string searchQuery)
        {
            var InitialFolder = Folders.SelectedItem;


            // Prompt the user on where to filter mails to.
            string? DestinationFolder = "";
            if (!(Utility.RadioButtonList.Input(Folders, "ListBoxName", "FullName", ref DestinationFolder) == DialogResult.OK))
            {
                return;
            }

            // this should mutate DestinationFolder to the stringified selected item of the data source
            // In our case the folder name is something like: "[Folder.Fullname, Displayed name]";
            if (string.IsNullOrEmpty(DestinationFolder)) return; // guard

            // Find the folder selected in the folderlist
            var f = folders.First(item => item.FullName == DestinationFolder);
            // Check if it is a valid target destination
            if(SpecialFolders.isFolderMoveMailToThisBlacklisted(f))
            {
                MessageBox.Show($"Not allowed to filter mails to special folder {f.FullName}.\nFilter not saved.");
                return;
            }


            // Prompt the user a name for the filter
            string FilterName = "";
            if (!(Utility.InputBox("Add Filter", "Filter Name: ", ref FilterName) == DialogResult.OK))
            {
                return;
            }

            // Check if filter name is already in use
            var ExistingFilterNames = FilterList.Select(filter => filter.Name).ToList();
            if (ExistingFilterNames.Contains(FilterName))
            {
                MessageBox.Show("A filter with the name " + FilterName + " already exists");
                return;
            }

            if (string.IsNullOrEmpty(FilterName))
            {
                MessageBox.Show("Please enter a valid filter name");
                return;
            }


            List<string>? searchLocations = GetSearchLocations();
            if(searchLocations.Count <= 0)
            {
                MessageBox.Show("Please specify a location to query");
                return;
            }


            // Add the new filter, to local list of filters.
            FilterList.Add(new Filter()
            {
                Name = FilterName,
                DestinationFolder = DestinationFolder,
                TargetString = searchQuery,
                SearchLocations = searchLocations,
            });

        }

        // When we click the search/add filter button this runs. Performs the search operation or add filter operation.
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


        // Method resets the text in the search textbox when clicked on.
        private void SearchTextBox_Click(object sender, EventArgs e)
        {
            SearchTextBox.ResetText();
        }



        // This method simply determines how the listbox items (mails) are drawn into the inbox. 
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


        // This method starts a drag and drop event.
        private async void Inbox_MouseDown(object sender, MouseEventArgs e)
        {
            // ensure that we are able to distinguish double clicks from this type of mouse down. 
            if (clicked) return;
            clicked = true;
            await Task.Delay(SystemInformation.DoubleClickTime); // get the user system default double click time. 
            if (!clicked) return;
            clicked = false;


            // Get the index of the mail clicked on
            var idx = Inbox.IndexFromPoint(e.X, e.Y);
            if (idx < 0 || idx > Inbox.Items.Count) return;

            DragDropEffects dde1 = DoDragDrop(idx,
                    DragDropEffects.All);

            if (dde1 == DragDropEffects.All)
            {
                Inbox.Items.RemoveAt(Inbox.IndexFromPoint(e.X, e.Y));
            }

            // Add this index to the data transfered during this drag-drop sequence
            Folders.DoDragDrop(idx, DragDropEffects.Copy);
        }

        // When we enter the folders listbox during a drag and drop sequence this runs, mostly just modifies the visuals
        // but also a guard if we have no data (index of a mail, it halts).
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


        // Moves from the current "folder".
        private async Task MoveMail(UniqueId id, IMailFolder TargetFolder)
        {
            // Move mail
            if (folder == null) folder = GetCurrentFolder();
            await folder.OpenAsync(FolderAccess.ReadWrite);
            await folder.MoveToAsync(id, TargetFolder);
        }

        // Method for modifying the count displayed on a given folder to the variable "count";
        // 
        private void UpdateFolderCount(IMailFolder? folder, int count)
        {
            if (folder == null) return;


            Folder? folderInList;
            string DisplayedName = "";
            int idx;

            folderInList = FolderList.First(item => item.FullName == folder?.FullName);
            if (folderInList == null || string.IsNullOrEmpty(folderInList.ListBoxName)) return; // guard
            DisplayedName = folderInList.ListBoxName;
            idx = FolderList.IndexOf(folderInList);

            // turning off the index selected here, or we get an error because when modifying a property of the data source bindinglist it automatically selects a new index briefly
            // (enough to cause error/exception as this index is out of bounds)
            Folders.SelectedIndexChanged -= Folders_SelectedIndexChanged;
            FolderList[idx].ListBoxName = (DisplayedName.Split('(', ')')[0] + "(" + count.ToString() + ")");
            Folders.SelectedIndexChanged += Folders_SelectedIndexChanged;

        }


        // Method for adding/subtracting to the counter next to a folder. The default number incremented is 1, but can specify this.
        private void IncrementFolderCount(IMailFolder? folder, bool decrement = false, int value = 1)
        {
            if (folder == null) return;

            Folder? folderInList;
            string DisplayedName = "";
            int number;
            int idx;

            folderInList = FolderList.First(item => item.FullName == folder?.FullName);
            if (folderInList == null || string.IsNullOrEmpty(folderInList.ListBoxName)) return; // guard
            DisplayedName = folderInList.ListBoxName;
            // we have moved one mail to this target folder, so we should update the display / view
            number = int.Parse(DisplayedName.Split('(', ')')[1]); // get number between parentheses
            number = decrement ? number - value : number + value;
            idx = FolderList.IndexOf(folderInList);

            // turning off the index selected here, or we get an error because when modifying a property of the data source bindinglist it automatically selects a new index briefly
            // (enough to cause error/exception as this index is out of bounds)
            Folders.SelectedIndexChanged -= Folders_SelectedIndexChanged;
            FolderList[idx].ListBoxName = (DisplayedName.Split('(', ')')[0] + "(" + number.ToString() + ")");
            Folders.SelectedIndexChanged += Folders_SelectedIndexChanged;
        }

        // what to do on successful drag and DROP (this is the drop part)
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
            string? folderName = FolderList[folderIdx].FullName;

            // find index of message to be moved:
            if (e.Data == null) return;
            if (string.IsNullOrEmpty(folderName)) return;
            // get the passed index in mail of mail to move from inbox.
            var InboxIdx = (int)e.Data.GetData(e.Data.GetFormats()[0]);




            // the message selected in the inbox folder (message to be moved)
            var selectedItem = messageSummaries[messageSummaries.Count - 1 - InboxIdx];

            if (ClientInUse) return;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);
                if (folder == null) folder = GetCurrentFolder();



                // find the folder that has "folderName", list of folders is stored as an attribute from when we loaded in the folders.
                foreach (var f in folders)
                {
                    // we find the correct folder and ensure that it is not either the special drafts- or sent folder.
                    if (f.FullName == folderName)
                    {
                        if(SpecialFolders.isFolderMoveMailToThisBlacklisted(f))
                        {
                            MessageBox.Show($"Not allowed to drag and drop mails to special folder \"{f.FullName}\"\nYour Email will be loaded into the current folder again on refresh");
                            break;
                        }

                        // Move mail
                        await MoveMail(selectedItem.UniqueId, f);
/*                        await folder.OpenAsync(FolderAccess.ReadWrite);
                        await folder.MoveToAsync(selectedItem.UniqueId, f);*/

                        // Instead of calling RefreshCurrentFolder() or RetrieveMessagesFromFolder(), we just remove it from the current listbox as this is faster (this is done by drag and drop automatically)
                        // and the next time we load the messages in, then it wont be there anyway as it is gone on the IMAP server side. 

                        // update messageSummaries so futuure operations without loading all messages work as intended
                        messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.EmailId | MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags); 


                        // Update current folder count
                        if (SpecialFolders.isFolderDisplayAllCount(folder))
                        {
                            IncrementFolderCount(folder, decrement: true);
                        }
                        else if (!SpecialFolders.isFolderUnreadBlacklisted(folder))
                        {
                            if (selectedItem.Flags != null && !selectedItem.Flags.Value.HasFlag(MessageFlags.Seen))
                                IncrementFolderCount(folder, decrement: true);
                        }

                        // Update target folder if applicable
                        if (SpecialFolders.isFolderDisplayAllCount(f))
                        {
                            IncrementFolderCount(f);
                        }
                        else if(!SpecialFolders.isFolderUnreadBlacklisted(f))
                        {
                            if (selectedItem.Flags != null && !selectedItem.Flags.Value.HasFlag(MessageFlags.Seen))
                                IncrementFolderCount(f);
                        }

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
                ClientInUse = false;
            }
        }

        // Adds visiual effects to the drag and drop chain
        private void Folders_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }


        // changes the text of the button based on the FilterCheckbox state.
        // This method runs every time the state of that checkbox changes.
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

        // Method for toggling visibility of the user's filters.
        // This method runs automatically every time filtercheckbox state is changed
        private void ShowFiltersCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            if (ShowFiltersCheckbox.Checked && FilterList.Count > 0)
            {
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



        // Method that is run whenever the remove filter button is clicked
        // The method removes that filter from the list of local filters (and ultimately the json file at program closure).
        private void RemoveFilterButton_Click(object sender, EventArgs e)
        {
            // get the filter name
            if (FilterListbox.SelectedItem == null) return;
            var filter = FilterList[FilterListbox.SelectedIndex];
            if (filter == null) return;
            FilterList.Remove(filter);
        }


        // This method triggers when we right click (on lift of right click) on a specific mail in inbox
        // This method just displays a context menu with functionality (delete, toggle flag, mark unread)
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

        // This method marks a given mail as unread if it is not already unread.
        private async void MarkMailAsUnread(object? sender, EventArgs e)
        {
            var msgIndex = Inbox.SelectedIndex;
            if (msgIndex < 0 || msgIndex > Inbox.Items.Count) // this should not happen
            {
                MessageBox.Show("No email to mark as unread");
                return;
            }
            var msg = messageSummaries[messageSummaries.Count - 1 - msgIndex];

            // Message is already unread 
            if (msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen)) return;

            if (ClientInUse) return; // guard 

        
            this.Cursor = Cursors.WaitCursor;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);
                if (folder == null) folder = GetCurrentFolder();
                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                // Mark the message as unread
                await folder.RemoveFlagsAsync(msg.UniqueId, MessageFlags.Seen, false);

                // instead of reloading the entire folder using RefreshCurrenFolder() or RetrieveMessagesFromFolder() to capture this (UNREAD) change,
                // we just manually forcefully update that one element in the listbox (this will automatically happen on refresh)
                Inbox.Items[msgIndex] = "(UNREAD) " + Inbox.Items[msgIndex];
                // To prevent us from prefixing (UNREAD) multiple times we need to update the messagesummaries however
                messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.EmailId | MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                IncrementFolderCount(folder);
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
                ClientInUse = false;
                
            }
        }

        // This method deletes a given mail entirely from the user's mailbox. No do-overs, does not move to trash. 
        private async void DeleteMail(object? sender, EventArgs e)
        {
            if (ClientInUse) return;

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
                ClientInUse = true;
                await Utility.ReconnectAsync(client);
                if (folder == null) folder = GetCurrentFolder();
                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                // Delete the message
                await folder.AddFlagsAsync(msg.UniqueId, MessageFlags.Deleted, true);
                await folder.ExpungeAsync();


                // Instead of calling RefreshCurrentFolder() or RetrieveMessagesFromFolder(), we just remove it from the current listbox as this is faster
                // and the next time we load the messages in, then it wont be there anyway as it is gone on the IMAP server side. 
                Inbox.Items.Remove(Inbox.SelectedItem);
                messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.EmailId | MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                // if the message is unread, we update the unread count of the folder
                if (msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen) && !SpecialFolders.isFolderUnreadBlacklisted(folder)) 
                {
                    IncrementFolderCount(folder, decrement: true);
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
                ClientInUse = false;
            }
        }

        // This method toggles the flagged state of a given mail.
        private async void ToggleFlagMail(object? sender, EventArgs e)
        {
            if (ClientInUse) return;
            var messageIndex = Inbox.SelectedIndex; // index of the message to modify
            if (messageIndex < 0) return; // failsafe
            var message = messageSummaries[messageSummaries.Count - 1 - messageIndex]; // find the message to modify

            this.Cursor = Cursors.WaitCursor;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);
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
                    Inbox.Items[messageIndex] = "(FLAGGED) " + Inbox.Items[messageIndex];
                    messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.EmailId | MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
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
                ClientInUse = false;
            }
        }


        // Re-retrieve all the folders and load in the inbox, when refresh button is clicked.
        // This is the function that runs on program startup (quite slow).
        // Refreshing the current folder is done by simply clicking on the currently open folder again (this is much faster)
        private void RefreshFoldersButton_Click(object sender, EventArgs e)
        {
            RetrieveFolders();
        }

        // method for creating a new folder. Currently we do not allow for sub-folders.
        private async void CreateFolderButton_Click(object sender, EventArgs e)
        {
            if (ClientInUse) return;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);

                // Let the user specify the name of the folder.
                string FolderName = "";
                if (!(Utility.InputBox("Create New Folder", "Name of Folder: ", ref FolderName) == DialogResult.OK))
                {
                    return;
                }

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
            finally
            {
                ClientInUse = false;
            }
        }


        // Method for deleting the currently open folder and all the mails in it.
        // Afterwards the client opens the default inbox. 
        private async void DeleteFolderButton_Click(object sender, EventArgs e)
        {
            if (ClientInUse) return;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);

                // Guard
                if (FolderList == null) return;

                DialogResult result = MessageBox.Show("Are you sure you want to delete the current folder? The action cannot be undone.", "Delete Folder?", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                    return;

                // folder is the imap folder.
                if (folder == null) folder = GetCurrentFolder();

  
                // Could add waitcursors here but the call to delete a toplevel folder is really fast...
                await folder.DeleteAsync();

                // if we get here, we should also delete it locally and not just on the server.
                // get the folder
                if (Folders.SelectedItem == null) return;
                Folder FolderInList = FolderList[Folders.SelectedIndex];
                if (FolderInList == null) return;
                FolderList.Remove(FolderInList);

                // Select another folder, here we just select the first one.
                Folders.SelectedIndex = 0;

                // FilterList.ToList() here because we are modifying the list as we increment. If we don't do this we get the 
                // "Collection was modified, enumeration operation may not execute" exception.
                foreach (var filter in FilterList.ToList())
                {
                    if (filter.DestinationFolder == null || filter.DestinationFolder == FolderInList.FullName)
                    {
                        // delete the filter 
                        FilterList.Remove(filter);
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
                ClientInUse = false;
            }

        }


        // Method that returns the list of UIDs of the currently open folder
        private List<UniqueId> GetUnreadMailsCurrentFolder()
        {
            // If folder is null, get current folder
            if (folder == null) folder = GetCurrentFolder(); 
            // If no messageSummaries locally, load in the messageSummaries of the current folder
            if (messageSummaries == null) messageSummaries = folder.Fetch(0, -1, MessageSummaryItems.EmailId | MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
            
            // find all the unread mails.
            var unreadMails = messageSummaries.Where(msg => msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen));
            var listUIDs = unreadMails.Select(msg => msg.UniqueId);
            return listUIDs.ToList();
        }


        // Displays the unread mails of the currently open folder in the inbox.
        private int ShowUnreadMails()
        {
            var uids = GetUnreadMailsCurrentFolder(); // Find the UIDs of the unread mails.
            if (folder == null) return -1; // if no folder, returns -1 (guard)
            ShowSearchResult(folder, uids); // display the unread mails
            return uids.Count; 

        }


        // Whenever we tick the checkbox to show unread mails only
        // this method runs
        // If checked it display only unread mails, if unchecked we load in the current folder's messages again. 
        private void FilterUnreadCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (folder == null) return;
            if (folder.Attributes.HasFlag(FolderAttributes.Sent) || folder.Attributes.HasFlag(FolderAttributes.Drafts)) return; // Never any unread mails in these folders, so dont do anything

            if (FilterUnreadCheckbox.Checked)
            {
                ShowUnreadMails();
            }
            else
            {
                RetrieveMessagesFromFolder();
            }
        }

        // Whenver the list of filters change, we check if we need to hide the associated GUI elements
        // in case there is nothing to show. 
        void filters_changed(object? sender, ListChangedEventArgs e)
        {
            // show the item if checkbox is checked
            if (ShowFiltersCheckbox.Checked && FilterList.Count > 0)
            {
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

    }
}



