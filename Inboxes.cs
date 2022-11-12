using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.DirectoryServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Email_Client_01
{
    public partial class Inboxes : Form
    {
        IList<IMessageSummary> messageSummaries = null!;
        private static Inboxes instance = null!; 

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
            RetrieveInboxMessages();

            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {

                    // Load in the folders from imap into a list
                    var folders = await client.GetFoldersAsync(new FolderNamespace('.', ""));


                    // storing folders in a dictionary.
                    Dictionary<string, string> foldersMap = new Dictionary<string, string>();

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
                            var folderName = folder.FullName.Substring(folder.FullName.LastIndexOf("/") + 1) + "  (" + unreadCount + ")"; // folder name
                            foldersMap.Add(key: folder.FullName, value: folderName); // add to dictionary. 
                        }
                    }

                    // Designating a data source for the listbox. 
                    Folders.DataSource = new BindingSource(foldersMap, null);

                    // The value and keys
                    Folders.DisplayMember = "Value";
                    Folders.ValueMember = "Key";
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
            return item.Envelope.Sender[0].Name;
        }

        private string FormatInboxMessageText(IMessageSummary item)
        {
            string subject = getSubject(item);
            string flags = GetFlags(item);
            string sender = GetSender(item);
            string result = flags + sender + ": " + subject;

            return result;
        }
        // Load all messages in default folder
        private async void RetrieveInboxMessages()
        {
            // remove all messages
            Inbox.Items.Clear();

            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    // Inbox folder is default folder, always exists on server
                    var folder = client.Inbox;

                    await folder.OpenAsync(FolderAccess.ReadOnly);

                    var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                    if (messages.Count <= 0)
                    {
                        toolStrip1.Visible = false;
                        Inbox.Items.Add("No messages in this folder!");
                        Inbox.Enabled = false;
                    }
                    else
                    {
                        toolStrip1.Visible = true;
                        messageSummaries = messages;
                        foreach (var item in messages.Reverse())
                        {
                            Inbox.Items.Add(FormatInboxMessageText(item));
                      
                        }
                        Inbox.Enabled = true;
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

        public static void RefreshCurrentFolder()
        {
            instance.RetrieveMessagesFromFolder();
        }

        private async Task<IMailFolder> GetCurrentFolder(ImapClient client)
        {
            var folder = await client.GetFolderAsync(Folders.SelectedValue.ToString());
            return folder;
        }

        // TODO implement this 
        private void openDraftFolder(IMailFolder folder)
        {
            return;
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

                    var folder = await GetCurrentFolder(client);

                    await folder.OpenAsync(FolderAccess.ReadOnly);

                    var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                    if (messages.Count <= 0)
                    {
                        Inbox.Items.Add("This folder is empty!");
                        Inbox.Enabled = false;
                    }
                    else if(folder.Attributes.HasFlag(FolderAttributes.Drafts))
                    {
                        openDraftFolder(folder);
                    }
                    else
                    {
                        /*                        addFlagsBt.visible = true;*/
                        messageSummaries = messages;
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
            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {



                    // Get the specific message
                    var messageId = (((ListBox)sender).SelectedIndex); // 2 parenthesis warns about missing ';' for some reason.
                    var messageItem = messageSummaries[messageSummaries.Count - messageId - 1];

                    // Add "Seen" flag to the message
                    var folder = await client.GetFolderAsync(messageItem.Folder.ToString());
                    await folder.OpenAsync(FolderAccess.ReadWrite);
                    await folder.AddFlagsAsync(messageItem.UniqueId, MessageFlags.Seen, true);

                    
                    // Get the MimeMessage from id:
                    MimeMessage msg = folder.GetMessage(messageItem.UniqueId);

                    new Reading_email(msg).Show();
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

        private void Prime_Mail_Homepage(object sender, EventArgs e)
        {
            // return to home page
        }


            // delete element from inbox (does not put in trashfolder, deletes entirely)

        
        private async void DeleteButton_click(object sender, EventArgs e)
        {
            var msgIndex = Inbox.SelectedIndex;
            var msg = messageSummaries[messageSummaries.Count - 1 - msgIndex];


            // quick check so we do not waste unnecessary time to establish an imap connection in case of errors.
            if(msgIndex < 0 || msgIndex > Inbox.Items.Count)
            {
                MessageBox.Show("Please select an email for deletion.");
                return;
            }

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
            this.Cursor = Cursors.WaitCursor;
            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    var messageIndex = Inbox.SelectedIndex;
                    var message = messageSummaries[messageSummaries.Count - 1 - messageIndex];

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

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchButton.Text))
            {
                string searchQuery = SearchTextBox.Text;
                search(searchQuery);
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

         IMailFolder GetTrashFolder(ImapClient client, CancellationToken cancellationToken)
        {
            string[] TrashFolderNames = { "Deleted", "Papirkurv", "Trash", "Bin", "Trashbin" };

            // this capability is generally dperecated
            if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
            {
                var trashFolder = client.GetFolder(SpecialFolder.Trash);
                return trashFolder;
            }

            else
            {
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
        }

        private async void MoveToTrashButton_Click(object sender, EventArgs e)
        {
            var msgIndex = Inbox.SelectedIndex;
            var msg = messageSummaries[messageSummaries.Count - 1 - msgIndex];

            // just a quick comparison to not use unnecessary time to establish an IMAP connection if nothing is selected.
            if (msgIndex < 0 || msgIndex > Inbox.Items.Count)
            {
                MessageBox.Show("Please select an email to move to trash");
                return;
            }

            


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
    }
}