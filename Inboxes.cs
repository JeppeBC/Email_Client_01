using Email_Client_01;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.VisualBasic;
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
using System.Text;
using Email_Client_01.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json.Linq;
using ScottPlot.Statistics.Interpolation;

using System;
using System.Linq;



// for distinguishing between types of click: https://learn.microsoft.com/en-us/dotnet/desktop/winforms/input-mouse/how-to-distinguish-between-clicks-and-double-clicks?view=netdesktop-6.0

namespace Email_Client_01
{
    public partial class Inboxes : Form, ISubscriber
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

            folder = client.Inbox;
            folder.Open(FolderAccess.ReadWrite);

            // Make columns in datagridview (inbox) not sortable.
            foreach (DataGridViewColumn column in InboxGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            // Make columns in not sortable.
            foreach (DataGridViewColumn column in InboxGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            // Setup the idle client, attach the inbox as subscriber. 
            idle = new IdleClient(Utility.ImapServer, Utility.ImapPort, MailKit.Security.SecureSocketOptions.Auto, Utility.username!, Utility.password!);
            idle.reciever.Attach(this);
            idle.RunAsync(); // idle client starts empty initially, so it immediately detects changes and loads everything in for us by invoking reload()


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
        private async Task RetrieveFolders()
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
                        if (SpecialFolders.isFolderDisplayAllCount(folder))
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
            InboxGrid.Rows.Clear();

            if (loadingMessages) return; // ensure we don't start loading another batch of messages before we are done loading the current batch. Even prevents queueing of many folders that need to be loaded in
            this.Cursor = Cursors.WaitCursor;
            loadingMessages = true;



            try
            {

                await Utility.ReconnectAsync(client); // if timed out, reconnect

                InboxGrid.Rows.Clear();


                folder = GetCurrentFolder();
                if (!folder.Exists) return;

                await folder.OpenAsync(FolderAccess.ReadWrite);



                // Takes less than 200 ms per filter that can move mails from that given folder.
                if(folder == client.Inbox)
                {
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
                }





                // load in the mails after filtering
                this.messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
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
                    InboxGrid.Rows.Add("This folder is empty!");

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
                        InboxGrid.Rows.Add(TextFormatter.GetSubject(item));
                    }
                }
                else
                {

                    foreach (var item in messageSummaries.Reverse())
                    {

                        InboxGrid.Rows.Add(TextFormatter.GetFlags(item), TextFormatter.GetSender(item), TextFormatter.GetSubject(item), item.Envelope.Date);


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
                if (locs.Count <= 0)
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
            InboxGrid.Rows.Clear();

            if (!folder.IsOpen) return;
            if (loadingMessages) return;

            // Get the actual messages from list of uids. 
            messageSummaries = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);


            if (messageSummaries.Count <= 0)
            {
                InboxGrid.Rows.Add("No results!");
            }
            foreach (var item in messageSummaries.Reverse())
            {
                InboxGrid.Rows.Add(TextFormatter.GetFlags(item), TextFormatter.GetSender(item), TextFormatter.GetSubject(item), item.Envelope.Date);
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



        private Filter? GetFilter()
        {
            string searchQuery = SearchTextBox.Text;
            // Prompt the user on where to filter mails to.
            string? DestinationFolder = "";
            if (!(Utility.RadioButtonList.Input(Folders, "ListBoxName", "FullName", ref DestinationFolder) == DialogResult.OK))
            {
                return null;
            }

            // this should mutate DestinationFolder to the stringified selected item of the data source
            // In our case the folder name is something like: "[Folder.Fullname, Displayed name]";
            if (string.IsNullOrEmpty(DestinationFolder)) return null; // guard

            // Find the folder selected in the folderlist
            var f = folders.First(item => item.FullName == DestinationFolder);
            // Check if it is a valid target destination
            if (SpecialFolders.isFolderMoveMailToThisBlacklisted(f))
            {
                MessageBox.Show($"Not allowed to filter mails to special folder {f.FullName}.\nFilter not saved.");
                return null;
            }


            // Prompt the user a name for the filter
            string FilterName = "";
            if (!(Utility.InputBox("Add Filter", "Filter Name: ", ref FilterName) == DialogResult.OK))
            {
                return null;
            }

            // Check if filter name is already in use
            var ExistingFilterNames = FilterList.Select(filter => filter.Name).ToList();
            if (ExistingFilterNames.Contains(FilterName))
            {
                MessageBox.Show("A filter with the name " + FilterName + " already exists");
                return null;
            }

            if (string.IsNullOrEmpty(FilterName))
            {
                MessageBox.Show("Please enter a valid filter name");
                return null;
            }


            List<string>? searchLocations = GetSearchLocations();
            if (searchLocations.Count <= 0)
            {
                MessageBox.Show("Please specify a location to query");
                return null;
            }

            Filter filter = new Filter()
            {
                Name = FilterName,
                DestinationFolder = DestinationFolder,
                TargetString = searchQuery,
                SearchLocations = searchLocations,
            };
            return filter;

        }

        // Method for adding a new filter (locally, we write it to json file at program closure).
        private void AddFilter(Filter filter)
        {
            // Add the new filter, to local list of filters.
            FilterList.Add(filter);
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
                    Filter? filter = GetFilter();
                    if (filter != null) AddFilter(filter);
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

            // Below commented code is if we want to allow multiselection drag and drop. 
            /*DataGridViewSelectedRowCollection rows = (DataGridViewSelectedRowCollection)e.Data.GetData(typeof(DataGridViewSelectedRowCollection));
            List<int> indices = new List<int>();
            foreach (DataGridViewRow row in rows)
            {
                indices.Add(row.Index);
            }

            List<IMessageSummary> MessagesToMove = new();

            foreach (int idx in indices)
            {
                MessagesToMove.Add(messageSummaries[messageSummaries.Count - 1 - idx]);
            }*/


            List<IMessageSummary> MessagesToMove = new();
            DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
            int idx = row.Index;
            MessagesToMove.Add(messageSummaries[messageSummaries.Count - 1 - idx]);

            /*            var InboxIdx = (int)e.Data.GetData(e.Data.GetFormats)
                            var InboxIdx = (int)e.Data.GetData(e.Data.GetFormats()[0]);*/


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
                        if (SpecialFolders.isFolderMoveMailToThisBlacklisted(f))
                        {
                            MessageBox.Show($"Not allowed to drag and drop mails to special folder \"{f.FullName}\"\nYour Email will be loaded into the current folder again on refresh");
                            break;
                        }

                        // Move mail
                        var uids = MessagesToMove.Select(m => m.UniqueId).ToList();
                        await folder.MoveToAsync(uids, f);
                        /*                        await folder.OpenAsync(FolderAccess.ReadWrite);
                                                await folder.MoveToAsync(selectedItem.UniqueId, f);*/

                        // Instead of calling RefreshCurrentFolder() or RetrieveMessagesFromFolder(), we just remove it from the current listbox as this is faster (this is done by drag and drop automatically)
                        // and the next time we load the messages in, then it wont be there anyway as it is gone on the IMAP server side. 
                        InboxGrid.Rows.Remove(row);


                        // Comented code here is if we want to move many rows. 
/*                        List<DataGridViewRow> toBeDeleted = new List<DataGridViewRow>();
                        foreach (DataGridViewRow row in InboxGrid.SelectedRows)
                        {
                            toBeDeleted.Add(row);
                        }
                        foreach (DataGridViewRow row in toBeDeleted)
                        {
                            InboxGrid.Rows.Remove(row);
                        }*/

                        // update messageSummaries so futuure operations without loading all messages work as intended
                        messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);


                        // Update current folder count
                        if (SpecialFolders.isFolderDisplayAllCount(folder))
                        {
                            IncrementFolderCount(folder, decrement: true, value: MessagesToMove.Count);
                        }
                        else if (!SpecialFolders.isFolderUnreadBlacklisted(folder))
                        {
                            List<MessageFlags?> flags = MessagesToMove.Select(msg => msg.Flags).ToList();
                            var unseenMessages = flags.Where(flag => flag != null && !flag.Value.HasFlag(MessageFlags.Seen)).ToList();
                            IncrementFolderCount(folder, decrement: true, value: unseenMessages.Count);
                        }

                        // Update target folder if applicable
                        if (SpecialFolders.isFolderDisplayAllCount(f))
                        {
                            IncrementFolderCount(f, value: MessagesToMove.Count);
                        }
                        else if (!SpecialFolders.isFolderUnreadBlacklisted(f))
                        {
                            List<MessageFlags?> flags = MessagesToMove.Select(msg => msg.Flags).ToList();
                            var unseenMessages = flags.Where(flag => flag != null && !flag.Value.HasFlag(MessageFlags.Seen)).ToList();
                            IncrementFolderCount(f, value: unseenMessages.Count);
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


        private void RemoveFilter(Filter filter)
        {
            FilterList.Remove(filter);
        }


        // Method that is run whenever the remove filter button is clicked
        // The method removes that filter from the list of local filters (and ultimately the json file at program closure).
        private void RemoveFilterButton_Click(object sender, EventArgs e)
        {
            // get the filter name
            if (FilterListbox.SelectedItem == null) return;
            var filter = FilterList[FilterListbox.SelectedIndex];
            if (filter == null) return;
            RemoveFilter(filter);
        }


      
        // handler for how we move mails to trash when we click on "Move to Trash" on context menu from right clicks
        private async void MoveMailToTrash_handler(object? sender, EventArgs e)
        {
            var msgIndex = InboxGrid.SelectedRows[0].Index;
            if (msgIndex < 0 || msgIndex > InboxGrid.Rows.Count) // this should not happen
            {
                MessageBox.Show("No email to mark as unread");
                return;
            }
            var msg = messageSummaries[messageSummaries.Count - 1 - msgIndex];


            this.Cursor = Cursors.WaitCursor;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);
                if (folder == null) folder = GetCurrentFolder();
                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                await MoveMail(msg.UniqueId, client.GetFolder(SpecialFolder.Trash));

                // Instead of calling RefreshCurrentFolder() or RetrieveMessagesFromFolder(), we just remove the one mail from the view as this is faster
                // and the next time we load the messages in, then it wont be there anyway as it is gone on the IMAP server side. 
                InboxGrid.Rows.Remove(InboxGrid.SelectedRows[0]);
                messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                // if the message is unread, we update the unread count of the folder
                if (msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen) && !SpecialFolders.isFolderUnreadBlacklisted(folder))
                {
                    IncrementFolderCount(folder, decrement: true);
                }
                else if (SpecialFolders.isFolderDisplayAllCount(folder))
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

        private async Task MarkAsUnread(IMessageSummary? msg)
        {
            if (msg == null) return; // guard

            // Message is already unread. With the current methods we never get here, but this might be relevant later. 
            if (msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen)) return;

            // Make sure folder is not null and it is open.
            if (folder == null) folder = GetCurrentFolder();
            if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

            // Remove the seen flag from the message
            await folder.RemoveFlagsAsync(msg.UniqueId, MessageFlags.Seen, false);

        }

        // This method marks a given mail as unread if it is not already unread.
        private async void MarkAsUnread_handler(object? sender, EventArgs e)
        {

            var msgIndex = InboxGrid.SelectedRows[0].Index;
            if (msgIndex < 0 || msgIndex > InboxGrid.RowCount) // this should not happen
            {
                MessageBox.Show("No email to mark as unread");
                return;
            }
            var msg = messageSummaries[messageSummaries.Count - 1 - msgIndex];

            if (ClientInUse) return; // guard 
            if (msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen)) return;

            this.Cursor = Cursors.WaitCursor;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);


                // Mark the message as unread
                await MarkAsUnread(msg);

                // instead of reloading the entire folder using RefreshCurrenFolder() or RetrieveMessagesFromFolder() to capture this (UNREAD) change,
                // we just manually forcefully update that one element in the listbox (this will automatically happen on refresh)

                // To prevent us from prefixing (UNREAD) multiple times we need to update the messagesummaries however
                messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                InboxGrid.Rows[InboxGrid.SelectedRows[0].Index].Cells[0].Value = "(UNREAD) " + InboxGrid.Rows[InboxGrid.SelectedRows[0].Index].Cells[0].Value;


                if (!SpecialFolders.isFolderUnreadBlacklisted(folder)) IncrementFolderCount(folder);
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


        private async Task DeleteMail(UniqueId id)
        {
            if (folder == null) folder = GetCurrentFolder();
            if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

            // Delete the message
            await folder.AddFlagsAsync(id, MessageFlags.Deleted, true);
            await folder.ExpungeAsync();

        }
        private async void DeleteMail_handler(object? sender, EventArgs e)
        {

            if (ClientInUse) return;

            // var msgIndex = Inbox.SelectedIndex;

            var msgIndex = InboxGrid.SelectedRows[0].Index;

            // quick check so we do not waste unnecessary time to establish an imap connection in case of errors.
            if (msgIndex < 0 || msgIndex > InboxGrid.Rows.Count) // dont know how this would appear, but just in case
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

                await DeleteMail(msg.UniqueId);

                // If Email to be deleted is in priority list, remove it from the list
                if (PriorityGrid.Rows.Count > 0)
                {
                    for (int i = 0; i < PriorityGrid.Rows.Count; i++)
                    {
                        if (PriorityGrid.Rows[i].Cells[2].Value == InboxGrid.Rows[InboxGrid.SelectedRows[0].Index].Cells[2].Value)
                        {
                            PriorityGrid.Rows[i].Selected = true;
                            PriorityGrid.Rows.Remove(PriorityGrid.SelectedRows[0]);
                        }
                    }
                }

                // Instead of calling RefreshCurrentFolder() or RetrieveMessagesFromFolder(), we just remove it from the current listbox as this is faster
                // and the next time we load the messages in, then it wont be there anyway as it is gone on the IMAP server side. 

                InboxGrid.Rows.Remove(InboxGrid.SelectedRows[0]);
                messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                // if the message is unread, we update the unread count of the folder
                if (msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen) && !SpecialFolders.isFolderUnreadBlacklisted(folder))
                {
                    IncrementFolderCount(folder, decrement: true);
                }
                else if (SpecialFolders.isFolderDisplayAllCount(folder))
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


        private async Task ToggleFlag(IMessageSummary message)
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

        }

        // This method handles how we toggles the flagged state of a given mail, when clicking "Toggle Flag" from the context menu
        // on right clicks on a given email in inbox.
        private async void ToggleFlag_handler(object? sender, EventArgs e)
        {
            if (ClientInUse) return;
            // var messageIndex = Inbox.SelectedIndex; // index of the message to modify

            var messageIndex = InboxGrid.CurrentRow.Index; ;

            if (messageIndex < 0) return; // failsafe
            var message = messageSummaries[messageSummaries.Count - 1 - messageIndex]; // find the message to modify

            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);

                await ToggleFlag(message);

                
                string? FlagCell = InboxGrid.SelectedRows[0].Cells[0].Value.ToString();
                if (FlagCell == null) return;
                if(FlagCell.Contains("(FLAGGED"))
                {
                    InboxGrid.SelectedRows[0].Cells[0].Value = FlagCell.Replace("(FLAGGED)", "");
                }
                else
                {
                    InboxGrid.SelectedRows[0].Cells[0].Value = "(FLAGGED) " + FlagCell;
                }

                messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
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


        // Re-retrieve all the folders and load in the inbox, when refresh button is clicked.
        // This is the function that runs on program startup (quite slow).
        // Refreshing the current folder is done by simply clicking on the currently open folder again (this is much faster)
        private void RefreshFoldersButton_Click(object sender, EventArgs e)
        {
            RetrieveFolders();
        }


        // Method that creates a new folder with the given name.We do not allow sub-folders
        private async Task CreateFolder(string name)
        {
            var toplevel = client.GetFolder(client.PersonalNamespaces[0]);
            await toplevel.CreateAsync(name, true);
        }

        // Method prompts the user for a name and then attempts to create a folder with that name. 
        // Finally the folder is loaded in immediately. 
        private async void CreateFolderButton_Click(object sender, EventArgs e)
        {
            string FolderName = "";
            if (ClientInUse) return;
            try
            {
                ClientInUse = true;
                await Utility.ReconnectAsync(client);

                // Let the user specify the name of the folder.
                if (!(Utility.InputBox("Create New Folder", "Name of Folder: ", ref FolderName) == DialogResult.OK))
                {
                    return;
                }

                // Could add waitcursors here but the call to create a toplevel folder is really fast...
                await CreateFolder(FolderName);

                FolderList.Add(new Folder()
                {
                    FullName = FolderName,
                    ListBoxName = FolderName,
                });

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
                await RetrieveFolders(); // Load in all folders again, this function is quite slow but how often do you create new folders...
                Folders.SelectedItem = FolderList.Where(folder => folder.FullName == FolderName).FirstOrDefault();
            }
        }


        // Method deletes the current folder.
        private async Task DeleteCurrentFolder()
        {
            // folder is the imap folder.
            if (folder == null) folder = GetCurrentFolder();
            await folder.DeleteAsync();
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


                // Delete the current folder
                await DeleteCurrentFolder();

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
        // MERGE : FIX THIS getuneradmailscurrentfolder
        // Method that returns the list of UIDs of the currently open folder
        // private List<UniqueId> GetUnreadMailsCurrentFolder()
        private List<UniqueId> GetUnreadMailsCurrentFolder()
        {
            // If folder is null, get current folder
            if (folder == null) folder = GetCurrentFolder();
            // If no messageSummaries locally, load in the messageSummaries of the current folder
            if (messageSummaries == null) messageSummaries = folder.Fetch(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

            // find all the unread mails.
            var unreadMails = messageSummaries.Where(msg => msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen));
            var listUIDs = unreadMails.Select(msg => msg.UniqueId);
            return listUIDs.ToList();
        }

        private void Priority_Clicked(object sender, EventArgs e)
        {
            // Take selected email and selected priority
            object Selecteditem = PrioritySelecter.SelectedItem;

            object PriorityMsg = InboxGrid.SelectedRows[0].Cells[2].Value;

            // Display the selected mail in listbox "Priority" as "priority + subject of email"
            PriorityGrid.Rows.Add("X", Selecteditem, PriorityMsg);

            PriorityGrid.Sort(PriorityGrid.Columns[1], ListSortDirection.Ascending);
        }


        private void OpenMessage(IMessageSummary? message)
        {
            if (message == null || ClientInUse) return;
            if (folder == null) folder = GetCurrentFolder();

            ClientInUse = true;
            this.Cursor = Cursors.WaitCursor;
            IEmailReader reader = new EmailReader(client, folder);
            reader.ReadMessage(message);
            ClientInUse = false;
            this.Cursor = Cursors.Default;
        }


        private void InboxGrid_DoubleClick(object sender, EventArgs e)
        {


            // Get the specific message
            if (((DataGridView)sender).SelectedRows.Count <= 0) return;
            int MailIdx = (((DataGridView)sender).SelectedRows[0].Index); // 2 parenthesis warns about missing ';' for some reason.
            if (MailIdx < 0) return; // failsafe
            var Mail = messageSummaries[messageSummaries.Count - InboxGrid.SelectedRows[0].Index - 1];


            OpenMessage(Mail);

            // if unread mail
            if (Mail.Flags != null && !Mail.Flags.Value.HasFlag(MessageFlags.Seen))
            {
                // Mutate the message in the listbox, so it no longer says unread
                string? currentString = InboxGrid.SelectedRows[0].Cells[0].Value.ToString();
                if (string.IsNullOrEmpty(currentString)) return;
                // Remove (UNREAD) if present in the inbox view
                InboxGrid.SelectedRows[0].Cells[0].Value = currentString.Replace("(UNREAD)", "").Trim();

                // Update the unread count
                IncrementFolderCount(folder, decrement: true);
            }


        }

        private void PriorityGrid_Click(object sender, DataGridViewCellEventArgs e)
        {
            // Clicking on any row, selects whole row.
            PriorityGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // If we click on the "x"
            if (e.ColumnIndex == 0)
                PriorityGrid.Rows.RemoveAt(e.RowIndex);
        }

        private void PriorityGrid_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // opens the specified email when doubleclicked in Prioritygrid
            InboxGrid.ClearSelection();
            var subject = PriorityGrid.SelectedRows[0].Cells[2].Value;



            IMessageSummary? mail = null;
            foreach (DataGridViewRow row in InboxGrid.Rows)
            {
                if (subject == InboxGrid.Rows[row.Index].Cells[2].Value)
                {
                    row.Selected = true;
                    int index = InboxGrid.SelectedRows[0].Index;
                    mail = messageSummaries[messageSummaries.Count - index - 1];
                }
            }

            if (mail == null) return;
            OpenMessage(mail);


            // if unread mail
            if (mail.Flags != null && !mail.Flags.Value.HasFlag(MessageFlags.Seen))
            {
                // Mutate the message in the listbox, so it no longer says unread
                string? currentString = InboxGrid.SelectedRows[0].Cells[0].Value.ToString();
                if (string.IsNullOrEmpty(currentString)) return;
                // Remove (UNREAD) if present in the inbox view
                InboxGrid.SelectedRows[0].Cells[0].Value = currentString.Replace("(UNREAD)", "").Trim();

                // Update the unread count
                IncrementFolderCount(folder, decrement: true);
            }
        }


        // Displays the unread mails of the currently open folder in the inbox.
        private int ShowUnreadMails()
        {
            var uids = GetUnreadMailsCurrentFolder(); // Find the UIDs of the unread mails.
            if (folder == null) return -1; // if no folder, returns -1 (guard)
            ShowSearchResult(folder, uids); // display the unread mails
            return uids.Count;
        }


        // need somewhere to display active filters and option to remove any active filters (does not work backwards, only works for future filtering)








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


        // Whenever we tick the checkbox to show unread mails only
        // this method runs
        // If checked it display only unread mails, if unchecked we load in the current folder's messages again. 
        private void FilterUnreadCheckbox_CheckChanged(object sender, EventArgs e)
        {
            if (folder == null) return;
            if (folder.Attributes.HasFlag(FolderAttributes.Sent) || folder.Attributes.HasFlag(FolderAttributes.Drafts)) return; // Never any unread mails in these folders, so dont do anything

            if (FilterUnreadCheckbox1.Checked)
            {
                ShowUnreadMails();
            }
            else
            {
                RetrieveMessagesFromFolder();
            }
        }

        private async void MetricsButton_Click(object sender, EventArgs e)
        {

            string myTempFile = Path.Combine(Path.GetTempPath(), "root.xml");

            // If XML needs to be updated
            if (Settings.Default.dateLastLoaded != DateTime.Today || !File.Exists(myTempFile))
            {

                if (!File.Exists(myTempFile))
                {
                    Settings.Default.dateLastLoaded = DateTime.Parse("01-01-2010");
                    Settings.Default.Save();
                }

                using (var client = Utility.GetImapClient())
                {
                    try
                    {
                        var folders = await client.GetFoldersAsync(new FolderNamespace('.', ""));
                        var messages = new List<IMessageSummary>();
                        var messages_sorted = new List<IMessageSummary>();
                        MailboxAddress MyAddress = MailboxAddress.Parse(Utility.username);

                        // Sent mails folder
                        foreach (var folder in folders)
                        {
                            // Perhaps rewrite to accress from ID or something instead of name
                            // All (including sent)
                            if (folder.Exists && folder.Attributes.HasFlag(FolderAttributes.All))
                            {
                                folder.Open(FolderAccess.ReadOnly);
                                var test = folder.Fetch(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
                                messages.AddRange(test.ToList());
                            }

                            // Sent
                            if (folder.Exists && folder.Attributes.HasFlag(FolderAttributes.Sent))
                            {
                                folder.Open(FolderAccess.ReadOnly);
                                var test = folder.Fetch(0, -1,  MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
                                messages_sorted.AddRange(test.ToList());
                            }
                        }

                        // Sort out sent mails from "All" folder
                        foreach (var message in messages)
                        {
                            foreach (var from in message.Envelope.From)
                            {
                                // If not sent by myself
                                if (!(((MailboxAddress)from).Address == MyAddress.Address))
                                {
                                    messages_sorted.Add(message);
                                }
                            }
                        }

                        Utility.CreateXML(messages_sorted);
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

            // Create metrics form
            metrics Metrics_Form = new metrics();
            Metrics_Form.Show();
        }




        // Right click functionality for main inbox.
        private async void InboxGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            
            if (e.Button == MouseButtons.Right)
            {
                InboxGrid.ClearSelection(); // deselect all rows
                InboxGrid.Rows[e.RowIndex].Selected = true;

                var itemIndex = InboxGrid.SelectedRows[0].Index;
                if (itemIndex < 0 || itemIndex > InboxGrid.RowCount) return;

                var ContextMenu = new ContextMenuStrip();
                ContextMenu.Items.Clear();


                var DeleteItem = new ToolStripMenuItem("Delete");
                DeleteItem.Click += new EventHandler(DeleteMail_handler);
                ContextMenu.Items.Add(DeleteItem);

                var FlagItem = new ToolStripMenuItem("Toggle Flag");
                FlagItem.Click += new EventHandler(ToggleFlag_handler);
                ContextMenu.Items.Add(FlagItem);

                var UnreadItem = new ToolStripMenuItem("Mark as Unread");
                UnreadItem.Click += new EventHandler(MarkAsUnread_handler);
                ContextMenu.Items.Add(UnreadItem);

                var MoveToTrash = new ToolStripMenuItem("Move to trash");
                MoveToTrash.Click += new EventHandler(MoveMailToTrash_handler);
                ContextMenu.Items.Add(MoveToTrash);

                InboxGrid.ContextMenuStrip = ContextMenu;
                InboxGrid.ContextMenuStrip.Show(Cursor.Position);
            }
        }


        // When we enter the folders listbox during a drag and drop sequence this runs, mostly just modifies the visuals
        // but also a guard if we have no data (index of a mail, it halts).
        private void Folders_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data == null) return; // guard
            if (e.Data.GetDataPresent(typeof(DataGridViewSelectedRowCollection)))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private async void InboxGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // ensure that we are able to distinguish double clicks from this type of mouse down. 
                if (clicked) return;
                clicked = true;
                await Task.Delay(SystemInformation.DoubleClickTime); // get the user system default double click time. 
                if (!clicked) return;
                clicked = false;

                // Add this to the data transfered during this drag-drop sequence
                InboxGrid.DoDragDrop(InboxGrid.SelectedRows[0], DragDropEffects.Move);
            }
        }
    }
}



