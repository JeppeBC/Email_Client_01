using Email_Client_01;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.VisualBasic;
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
using System.Diagnostics;
using System.Text;
using Email_Client_01.Properties;
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
        private ImapClient client;
        bool loadingFolders = false;
        bool loadingMessages = false;
        IMailFolder? folder; // current folder (mostly bookkeeping for loading)

        BindingList<Filter> FilterList;
        HashSet<string> FilterSet = new();

        BindingList<Folder> FolderList;



        // private construtor as we employ singleton pattern
        private Inboxes(ImapClient client)
        {
            InitializeComponent();

            this.client = client;

            // filepath of filter.json file
            // if file does not exist
            if (!File.Exists(Utility.JsonFilePath))
            {
                // create the file
                File.Create(Utility.JsonFilePath).Close();
            }
            // Read the entire file and De-serialize to list of filters
            var Filters = Utility.JsonFileReader.Read<List<Filter>>(Utility.JsonFilePath) ?? new List<Filter>();



            // Update the jsonfile when form closes
            this.FormClosed += (s, args) =>
            {
                Utility.JsonFileWriter.Write<List<Filter>>(Utility.JsonFilePath, Filters);
            };

            BindingList<Filter> BindingListFilters = new BindingList<Filter>(Filters);
            BindingListFilters.ListChanged += new ListChangedEventHandler(filters_changed);
            FilterList = BindingListFilters;
            FilterListbox.DataSource = FilterList;
            FilterListbox.DisplayMember = "Name";

            BindingList<Folder> BindingListFolders = new BindingList<Folder>();
            BindingListFolders.ListChanged += new ListChangedEventHandler(folders_changed);
            FolderList = BindingListFolders;
            Folders.DataSource = FolderList;
            Folders.DisplayMember = "ListBoxName";

            // Add timer as a way of enforcing that this client connection does not time out naturally.
            // Normally this can happen anywhere between 10, 15, 20, 25 or even 30 min. It is not very consistent. 
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 300000; // every 5 min, unit is milliseconds.
            aTimer.Enabled = true;

            folder = null;

            // todo change this to the idle, active and ui threads. Add checks that this connection does not break / expire
            RetrieveFolders();
        }

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


            /*            switch (e.ListChangedType)
                        {
            *//*            case ListChangedType.ItemAdded:
                            FilterListbox.Refresh();
                            break;
                        case ListChangedType.ItemChanged:
                            FilterListbox.Refresh();
                            break;
                        case ListChangedType.ItemDeleted:
                            FilterListbox.Refresh();
                            break;
                        case ListChangedType.ItemMoved:
                            FilterListbox.Refresh();
                            break;*//*
                            default:
                                break;
                                // some more minor ones, etc.
                        }*/
        }

        private void folders_changed(object? sender, ListChangedEventArgs e)
        {
/*            MessageBox.Show("folders changed");*/
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


        // get the instance, does not create one
        public static Inboxes? GetInstance()
        {
            return instance;
        }

        // retrives all the folder names and add to the listbox
        private async void RetrieveFolders()
        {
            this.Cursor = Cursors.WaitCursor;
            loadingFolders = true;
            try
            {
                // Load in the folders from imap into a list
                folders = await client.GetFoldersAsync(new FolderNamespace('.', ""));

                FolderList.Clear();


                foreach (var folder in folders)
                {
                    if (folder.Exists)
                    {

                        // counting number of unread messages, the time this takes is noticeable #TODO
                        folder.Open(FolderAccess.ReadOnly);



                        var folderName = folder.FullName.Substring(folder.FullName.LastIndexOf("/") + 1);


                        if (!isFolderUnreadBlacklisted(folder))
                        {
                            int unreadCount = 0;
                            foreach (var uid in folder.Search(SearchQuery.NotSeen))
                            {
                                unreadCount++;
                            }
                            folderName += " (" + unreadCount + ")";
                        }
                        // show the number of items in the following folders:
                        if(isFolderDisplayAllCount(folder))
                        {
                            if (folder.Count > 0) folderName += " (" + folder.Count + ")";
                        }



                        FolderList.Add(new Folder()
                        {
                            FullName = folder.FullName,
                            ListBoxName = folderName
                        }); ;
                        folder.Close();
                    }
                }

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
            return client.GetFolder(FolderList[Folders.SelectedIndex].FullName);
        }




        // default parameters here because we send it another method that does not care.
        // retrives messages in a folder when it is double clicked. 
        // default parameters here because we send it another method that does not care.
        // retrives messages in a folder when it is double clicked. 
        private async void RetrieveMessagesFromFolder(object sender = null!, EventArgs e = null!)
        {
            Inbox.Items.Clear();
            InboxGrid.Rows.Clear();

            if (loadingMessages) return; // ensure we don't start loading another batch of messages before we are done loading the current batch. Even prevents queueing of many folders that need to be loaded in

            this.Cursor = Cursors.WaitCursor;
            loadingMessages = true;
            try
            {
                Inbox.Items.Clear();
                InboxGrid.Rows.Clear();

                folder = GetCurrentFolder();
                if (!folder.Exists) return;

                await folder.OpenAsync(FolderAccess.ReadWrite);

                // optional TODO optimize the search queries so we can do these in batches.
                // Each call to search takes about 200ms per filter (of course depending on the number of mails it is filtering)

                // Notably we do not really care about the unread blacklist here, but it happens to be most of the special folders we also don't want to filter
                // Namely: Sent, drafts, trashcan, all, flagged
                if (!isFilterBlacklisted(folder))
                {
                    foreach (var filter in FilterList)
                    {
                        foreach (var f in folders) // find the IMailFolder from string DestinationFolder
                        {
                            if (f == folder) continue; // no point in moving to the same folder as we are currently in
                            if (f.FullName != filter.DestinationFolder || !f.Exists) continue;
                            // should not allow creating filters to these folders in the first place, but a guard in case json file is manually modified perhaps.
                            if (isFolderMoveMailToThisBlacklisted(f)) continue; 

                            var query = GetSearchQueryFromFilter(filter);
                            if (query == null) continue;

                            // find all the mails to be moved
                            // takes like 150-200 ms per filter, slightly (almost negligible) extra ammount from moving the mail.
                            var listUIDs = await folder.SearchAsync(query.And(SearchQuery.DeliveredAfter(Properties.Time.Default.Date)));

                            if (listUIDs.Count <= 0) continue; // nothing to move so we dont need to do anything 

                            // This similarly takes 100-200ms per filter, this following segment prevens the filter collisions
                            // where you can have one filter move one mail to say spam and another one that moves the same mail to inbox or whatever.
                            // In this scenario the user never sees the mail, but only the count go up and down when it is being moved. 
                            // This block ensures that we only filter a mail at most once. However it is a somewhat expensive call as we access the imap client.
                            // We do this as the Unique IDs are only folder specific, whereas the emailid is a globally unique identifier. 
                            var messages = await folder.FetchAsync(listUIDs, MessageSummaryItems.UniqueId);
                            foreach (var message in messages)
                            {
                                // Add the globally unique emailid to set. If present the method returns false
                                // So if it is already present in the set, we remove the unique id from list of uids to be moved. 
                                if (!FilterSet.Add(message.EmailId))
                                {
                                    listUIDs.Remove(listUIDs.First(id => id == message.UniqueId));
                                }
                            }


                            await folder.MoveToAsync(listUIDs, f);

                            

                            // update the unread count on folder f. We are moving (unread) mails to f
                            IncrementFolderCount(f, value: listUIDs.Count());
                            // We are moving a message before it is shown in the current listbox, so we do not need to update the count of current folder. 
                            
                            break;

                        }
                    }

                }
                // load in the mails after filtering
                messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
                int unreadCount = 0;

                if(FilterUnreadCheckbox.Checked && !isFolderWithoutUnreads(folder))
                {
                    unreadCount = ShowUnreadMails(messageSummaries);
                }
                else if (messageSummaries.Count <= 0)
                {
                    Inbox.Items.Add("This folder is empty!");
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
                        Inbox.Items.Add(FormatDraftInboxText(item));
                    }
                }
                else
                {
                    foreach (var item in messageSummaries.Reverse())
                    {
                        Inbox.Items.Add(FormatInboxMessageText(item));
                        InboxGrid.Rows.Add(GetFlags(item), item.Envelope.Sender, item.Envelope.Subject, item.Envelope.Date);

                        // make sure the folder count is correct
                        if (isFolderUnreadBlacklisted(folder)) continue;
                        if (item.Flags != null && !item.Flags.Value.HasFlag(MessageFlags.Seen)) unreadCount += 1;
                    }
                }
                // display the unread count if not blacklisted
                if (isFolderDisplayAllCount(folder)) UpdateFolderCount(folder, folder.Count);
                else if(!isFolderUnreadBlacklisted(folder) && unreadCount > -1) UpdateFolderCount(folder, unreadCount);

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
            var messageId = (((DataGridView)sender).SelectedRows.ToString); // 2 parenthesis warns about missing ';' for some reason.
            //if (messageId < 0) return; // failsafe
            var messageItem = messageSummaries[messageSummaries.Count - InboxGrid.SelectedRows[0].Index - 1];

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (folder == null) folder = GetCurrentFolder();

                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                // if unread mail
                if (messageItem.Flags != null && !messageItem.Flags.Value.HasFlag(MessageFlags.Seen))
                {
                    // Mutate the message in the listbox, so it no longer says unread
                    string currentString = InboxGrid.SelectedRows[0].Cells[0].Value.ToString();
                    // Remove (UNREAD) if present in the inbox view
                    InboxGrid.SelectedRows[0].Cells[0].Value = currentString.Replace("(UNREAD)", "").Trim();

                    // Update the unread count
                    IncrementFolderCount(folder, decrement: true);

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
            InboxGrid.Rows.Clear();
            messageSummaries = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

            if (messageSummaries.Count <= 0)
            {
                Inbox.Items.Add("No results!");
                InboxGrid.Rows.Add("No results!");
            }
            foreach (var item in messageSummaries.Reverse())
            {
                Inbox.Items.Add(FormatInboxMessageText(item));
                InboxGrid.Rows.Add(GetFlags(item), item.Envelope.Sender, item.Envelope.Subject, item.Envelope.Date);
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


            // the 5 commented lines here about the SelecteDIndexChanged and selectedItem is if we do not want to load in the messages as we select them
            /*          var selectedFolder = Folders.SelectedItem;
                        Folders.SelectedIndexChanged -= Folders_SelectedIndexChanged;*/
            string? DestinationFolder = "";
            if (!(Utility.RadioListBoxInput(Folders, "ListBoxName", "FullName", ref DestinationFolder) == DialogResult.OK))
            {
                /*                Folders.SelectedIndexChanged += Folders_SelectedIndexChanged;*/
                return;
            }
            /*            Folders.SelectedIndexChanged += Folders_SelectedIndexChanged;
                        Folders.SelectedItem = selectedFolder;*/

            // this should mutate DestinationFolder to the stringified selected item of the data source
            // In our case the folder name is something like: "[Folder.Fullname, Displayed name]";
            if (string.IsNullOrEmpty(DestinationFolder)) return; // guard

            foreach(var f in folders)
            {
                // Check if the folder is blacklisted from carelessly being moved mail to
                if(f.FullName == DestinationFolder && isFolderMoveMailToThisBlacklisted(f))
                {
                    MessageBox.Show($"Not allowed to filter mails to special folder {f.FullName}.\nFilter not saved.");
                    return;
                }
            }
/*
            // We check that the destination folder is not a special folder we should not allow mails to be moved to carelessly:
            if (isFolderMoveMailToThisBlacklisted(DestinationFolder)) return;*/

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



            // LOADING MESSAGES: Load the initial folder back, if the data sources have been bugged out for some reason
            // ensure that the correct folder is opened at the end. 
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


        private void UpdateFolderCount(IMailFolder? folder, int unread)
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
            FolderList[idx].ListBoxName = (DisplayedName.Split('(', ')')[0] + "(" + unread.ToString() + ")");
            Folders.SelectedIndexChanged += Folders_SelectedIndexChanged;

        }

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

       private bool isFolderUnreadBlacklisted(IMailFolder? f)
        {
            if (f == null) return false;
            if (f.Attributes.HasFlag(FolderAttributes.Trash)     ||
                f.Attributes.HasFlag(FolderAttributes.Drafts)    ||
                f.Attributes.HasFlag(FolderAttributes.Sent)      || 
                f.Attributes.HasFlag(FolderAttributes.All)       ||
                f.Attributes.HasFlag(FolderAttributes.Flagged)   ||
                f.Attributes.HasFlag(FolderAttributes.Important) ||
                f.Attributes.HasFlag(FolderAttributes.Archive)
                ) return true;
            return false;
        }

        private bool isFolderWithoutUnreads(IMailFolder? f)
        {
            if (f == null) return false;
            if (f.Attributes.HasFlag(FolderAttributes.Sent) ||
               f.Attributes.HasFlag(FolderAttributes.Drafts)
               ) return true;
            return false;
        }

        // At the moment one-to-one identical with the function above, however we can imagine these could end up doing two different things
        // so we make them into separate functions. 
        private bool isFilterBlacklisted(IMailFolder? f)
        {
            if (f == null) return false;
            if(f.Attributes.HasFlag(FolderAttributes.Trash) ||
                f.Attributes.HasFlag(FolderAttributes.Drafts) ||
                f.Attributes.HasFlag(FolderAttributes.Sent) ||
                f.Attributes.HasFlag(FolderAttributes.All) ||
                f.Attributes.HasFlag(FolderAttributes.Flagged) ||
                f.Attributes.HasFlag(FolderAttributes.Important) ||
                f.Attributes.HasFlag(FolderAttributes.Archive)
                ) return true;
            return false;
        }

        private bool isFolderMoveMailToThisBlacklisted(IMailFolder? f)
        {
            if (f == null) return false;
            if (f.Attributes.HasFlag(FolderAttributes.Drafts) ||
                f.Attributes.HasFlag(FolderAttributes.Sent) ||
                f.Attributes.HasFlag(FolderAttributes.All)
                ) return true;
            return false;
        }

        private bool isFolderDisplayAllCount(IMailFolder? f)
        {
            if (f == null) return false;

            // could add f.Attributes.HasFlag(FolderAttributes.Drafts) here if we wanted this for drafts.
            if (f.Attributes.HasFlag(FolderAttributes.Flagged)   ||
                f.Attributes.HasFlag(FolderAttributes.Important)
                ) return true;
            return false;
        }

  


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
            string? folderName = FolderList[folderIdx].FullName;

            // find index of message to be moved:
            if (e.Data == null) return;
            if (string.IsNullOrEmpty(folderName)) return;
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
                        if(isFolderMoveMailToThisBlacklisted(f))
                        {
                            MessageBox.Show($"Not allowed to drag and drop mails to special folder \"{f.FullName}\"\nYour Email will be loaded into the current folder again on refresh");
                            break;
                        }

                        // Move mail
                        await folder.OpenAsync(FolderAccess.ReadWrite);
                        await folder.MoveToAsync(selectedItem.UniqueId, f);

                        // Instead of calling RefreshCurrentFolder() or RetrieveMessagesFromFolder(), we just remove it from the current listbox as this is faster (this is done by drag and drop automatically)
                        // and the next time we load the messages in, then it wont be there anyway as it is gone on the IMAP server side. 

                        // update messageSummaries so futuure operations without loading all messages work as intended
                        messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags); 

/*                        RetrieveMessagesFromFolder(); // updates the current folder unread count too, so we only need to worry about the target folder*/



                        // Update current folder count
                        if (isFolderDisplayAllCount(folder))
                        {
                            IncrementFolderCount(folder, decrement: true);
                        }
                        else if (!isFolderUnreadBlacklisted(folder))
                        {
                            if (selectedItem.Flags != null && !selectedItem.Flags.Value.HasFlag(MessageFlags.Seen))
                                IncrementFolderCount(folder, decrement: true);
                        }

                        // Update target folder if applicable
                        if (isFolderDisplayAllCount(f))
                        {
                            IncrementFolderCount(f);
                        }
                        else if(!isFolderUnreadBlacklisted(f))
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



        private void RemoveFilterButton_Click(object sender, EventArgs e)
        {
            // get the filter name
            if (FilterListbox.SelectedItem == null) return;
            var filter = FilterList[FilterListbox.SelectedIndex];
            if (filter == null) return;
            FilterList.Remove(filter);
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
            var msgIndex = InboxGrid.CurrentRow.Index;
            if (msgIndex < 0 || msgIndex > InboxGrid.Rows.Count) // this should not happen
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

                // instead of reloading the entire folder using RefreshCurrenFolder() or RetrieveMessagesFromFolder() to capture this (UNREAD) change,
                // we just manually forcefully update that one element in the listbox (this will automatically happen on refresh)
                InboxGrid.Rows[InboxGrid.CurrentRow.Index].Cells[0].Value = "(UNREAD) ";

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
            }
        }

        private async void DeleteMail(object? sender, EventArgs e)
        {
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
                if (folder == null) folder = GetCurrentFolder();
                if (!folder.IsOpen) await folder.OpenAsync(FolderAccess.ReadWrite);

                // Delete the message
                await folder.AddFlagsAsync(msg.UniqueId, MessageFlags.Deleted, true);
                await folder.ExpungeAsync();

                // If Email to be deleted is in priority list, remove it from the list
                if (PriorityGrid.Rows.Count > 0)
                {
                    for (int i=0; i<PriorityGrid.Rows.Count;i++)
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
                if (msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen) && !isFolderUnreadBlacklisted(folder)) 
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
            }
        }

        private async void ToggleFlagMail(object? sender, EventArgs e)
        {
            var messageIndex = InboxGrid.CurrentRow.Index; ;
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
                    Inbox.Items[messageIndex] = "(FLAGGED) " + Inbox.Items[messageIndex];
                    messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
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
        }
        private List<UniqueId> GetUnreadMailsCurrentFolder(IList<IMessageSummary> msgSummaries)
        {
            // get all the unread mails.
            var unreadMails = messageSummaries.Where(msg => msg.Flags != null && !msg.Flags.Value.HasFlag(MessageFlags.Seen));
            var listUIDs = unreadMails.Select(msg => msg.UniqueId);
            return listUIDs.ToList();
        }
        private void PriorityClicked(object sender, EventArgs e)
        {

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

        private void InboxGrid_Click(object sender, EventArgs e)
        {
            InboxGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void InboxGrid_DoubleClick(object sender, EventArgs e)
        {
            ReadMessage(sender, e);
        }

        private void PriorityGrid_Click(object sender, DataGridViewCellEventArgs e)
        {
            PriorityGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (e.ColumnIndex == 0)
                PriorityGrid.Rows.RemoveAt(e.RowIndex);
        }

        private void PriorityGrid_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // opens the specified email when doubleclicked in Prioritygrid
            InboxGrid.ClearSelection();
            var subject = PriorityGrid.SelectedRows[0].Cells[2].Value;
            
            foreach (DataGridViewRow row in InboxGrid.Rows)
            {
                if (subject == InboxGrid.Rows[row.Index].Cells[2].Value)
                    row.Selected = true;
            }
            
            ReadMessage(sender, e);

        }

        private void InboxGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var itemIndex = InboxGrid.CurrentRow.Index;
                if (itemIndex < 0 || itemIndex > InboxGrid.SelectedRows.Count) return;

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

                InboxGrid.ContextMenuStrip = ContextMenu;
                InboxGrid.ContextMenuStrip.Show(Inbox, e.Location);

            }
        }



        // need somewhere to display active filters and option to remove any active filters (does not work backwards, only works for future filtering)

        private int ShowUnreadMails(IList<IMessageSummary> msgSummaries)
        {
            var uids = GetUnreadMailsCurrentFolder(msgSummaries);
            if (folder == null) return -1;
            ShowSearchResult(folder, uids);
            return uids.Count;

        }

        private void FilterUnreadCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (folder == null) return;
            if (folder.Attributes.HasFlag(FolderAttributes.Sent) || folder.Attributes.HasFlag(FolderAttributes.Drafts)) return; // Never any unread mails in these folders, so dont do anything

            if (FilterUnreadCheckbox.Checked)
            {
                ShowUnreadMails(messageSummaries);
            }
            else
            {
                RetrieveMessagesFromFolder();
            }
        }

        private void FilterUnreadCheckbox_CheckChanged(object sender, EventArgs e)
        {
            if (folder == null) return;
            if (folder.Attributes.HasFlag(FolderAttributes.Sent) || folder.Attributes.HasFlag(FolderAttributes.Drafts)) return; // Never any unread mails in these folders, so dont do anything

            if (FilterUnreadCheckbox.Checked)
            {
                ShowUnreadMails(messageSummaries);
            }
            else
            {
                RetrieveMessagesFromFolder();
            }
        }

        private async void MetricsButton_Click(object sender, EventArgs e)
        {
            // If XML needs to be updated
            if (Settings.Default.dateLastLoaded != DateTime.Today)
            {
                using (var client = await Utility.GetImapClient())
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
                                var test = folder.Fetch(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);
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
    }
}