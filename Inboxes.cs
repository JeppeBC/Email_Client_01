using MailKit;
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

        public Inboxes()
        {
            InitializeComponent();


            // todo change this to the idle, active and ui threads. Add checks that this connection does not break / expire

            RetrieveFolders();
        }

        private async void RetrieveFolders()
        {
            RetrieveInboxMessages();

            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;


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

        private async void RetrieveInboxMessages()
        {
            Inbox.Items.Clear();

            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    var folder = client.Inbox;

                    await folder.OpenAsync(FolderAccess.ReadOnly);

                    var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                    if (messages.Count <= 0)
                    {
                        Inbox.Items.Add("No messages in this folder!");
                        Inbox.Enabled = false;
                    }
                    else
                    {
                        messageSummaries = messages;
                        foreach (var item in messages.Reverse())
                        {
                            if (item.Flags.Value.HasFlag(MessageFlags.Flagged))
                            {
                                var sub = "(FLAGGED) " + item.Envelope.Subject;
                                Inbox.Items.Add(sub);
                            }

                            if (item.Flags != null && !item.Flags.Value.HasFlag(MessageFlags.Seen))
                            {
                                var sub = "(UNREAD) " + item.Envelope.Subject;
                                Inbox.Items.Add(sub);
                            }
                            else if (item.Envelope.Subject != null)
                            {
                                Inbox.Items.Add(item.Envelope.Subject);
                            }
                            else
                            {
                                item.Envelope.Subject = "<no subject>";
                                Inbox.Items.Add(item.Envelope.Subject);
                            }
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
                    await client.DisconnectAsync(true);
                    client.Dispose();
                    this.Cursor = Cursors.Default;
                }

            }

        }

        private async void RetrieveMessagesFromFolder(object sender, EventArgs e)
        {
            Inbox.Items.Clear();

            using (var client = await Utility.GetImapClient())
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    var folder = await client.GetFolderAsync(((ListBox)sender).SelectedValue.ToString());

                    await folder.OpenAsync(FolderAccess.ReadOnly);

                    var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                    if (messages.Count <= 0)
                    {
                        Inbox.Items.Add("This folder is empty!");
                        Inbox.Enabled = false;
                    }
                    else
                    {
                        /*                        addFlagsBt.visible = true;*/
                        messageSummaries = messages;
                        foreach (var item in messages.Reverse())
                        {
                            if (item.Flags.Value.HasFlag(MessageFlags.Flagged))
                            {
                                var sub = "(FLAGGED) " + item.Envelope.Subject;
                                Inbox.Items.Add(sub);
                            }

                            if (!(item.Flags.Value.HasFlag(MessageFlags.Seen)))
                            {
                                var sub = "(UNREAD) " + item.Envelope.Subject;
                                Inbox.Items.Add(sub);
                            }


                            else if (item.Envelope.Subject != null)
                                Inbox.Items.Add(item.Envelope.Subject);

                            else
                            {
                                item.Envelope.Subject = "<no subject>";
                                Inbox.Items.Add(item.Envelope.Subject);
                            }

                        }
                    }

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

        // Read a specific method when doubleClicked
        private async void ReadMessage(object sender, EventArgs e)
        {
            using (var client = await Utility.GetImapClient())
            {
                try
                {

                    this.Cursor = Cursors.WaitCursor;


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
                    await client.DisconnectAsync(true);
                    client.Dispose();
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


        // TODO rename this window forms specific method?
        private void Inbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadMessage(sender, e);
        }

        // once an email is chosen from list of recieved emails use
        // Reading_email Read_mail = new Reading_email();
        // Read_mail.Show();
        // To open form to read mails

        private void trashicon_Click(object sender, EventArgs e)
        {
            // delete element from inbox
        }

        private void RefreshPage_Click(object sender, EventArgs e)
        {
            // refresh elements in inbox
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
            // folders should be read in here (flags), det var sådan andreas havde det
        }
    }
}