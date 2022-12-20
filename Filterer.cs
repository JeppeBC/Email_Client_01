using Email_Client_01;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    // Class that does the filtering of emails.
    
    internal class Filterer
    {
        private List<Filter> filters;               // container for filters
        static private HashSet<string> filteredMails = new();  // For bookkeeping which mails have already been moved.
                                                    // Can imagine scenarios where filters are conflicting (moving mails back and forth)
                                                    // We need to deal with this as the user would never be able to read such a mail. 
                        
        private readonly ImapClient client;         // IMAP-client connection to actually move the mails.

        private IList<IMailFolder> folders;         // List of all folders.

        public Filterer(List<Filter> filters, IList<IMailFolder> folders, ImapClient client)
        {
            this.folders = folders;
            this.client = client;
            this.filters = filters;
        }


        // This method returns a (MailKit) search query corresponding to the mails that the given filter should move.
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

        // returns number of filtered mails to each folder;
        public async Task<Dictionary<IMailFolder, int>?> FilterFolder(IMailFolder FolderToFilter)
        {
            if (SpecialFolders.isFilterBlacklisted(FolderToFilter)) return null; // this folder cannot be filtered so nothing is done

            try
            {
                Dictionary<IMailFolder, int> MovedMailCount = new(); 

                foreach (var filter in filters) // Loop through each filter
                {
                    // Find the folder we want to filter mails to. 
                    IMailFolder? TargetFolder = folders.Where(folder => folder.FullName == filter.DestinationFolder).FirstOrDefault();

                    //guards that should never happen but just in case
                    if (TargetFolder == null || !TargetFolder.Exists) continue;

                    // No need to move anything to the same folder it is currently in
                    if (TargetFolder == FolderToFilter) continue;

                    // again a guard that should not happen but we make sure.
                    if (SpecialFolders.isFolderMoveMailToThisBlacklisted(TargetFolder)) continue;

                    var query = GetSearchQueryFromFilter(filter);
                    if (query == null) continue;


                    // find all the mails to be moved
                    // takes like 150-200 ms per filter, slightly (almost negligible) extra ammount from moving the mail.
                    var listUIDs = await FolderToFilter.SearchAsync(query.And(SearchQuery.DeliveredAfter(Properties.Time.Default.Date)));
                    if (listUIDs.ToList().Count <= 0) continue; // if nothing to move we should not do expensive calls to folder.fetch or folder.movetoasync
                    // Get message descriptions.  The call to do this takes 100-200ms per filter,
                    var summaries = await FolderToFilter.FetchAsync(listUIDs, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                    // IMAP query delivered after only compares by date, so we need to filter by time also.
                    IList<IMessageSummary> sums = summaries.Where(sum => DateTime.Compare(sum.Date.DateTime, Properties.Time.Default.Date) > 0).ToList();
                    listUIDs = sums.Select(sum => sum.UniqueId).ToList();

                    foreach (var sum in sums)
                    {
                        // if the email is already in the filterset
                        if (!filteredMails.Add(TextFormatter.FormatInboxText(sum)))
                        {
                            // remove the corresponding uid from the list of uids to be moved.
                            listUIDs.Remove(listUIDs.Where(uid => uid == sum.UniqueId).FirstOrDefault());
                        }
                    }

                    await FolderToFilter.MoveToAsync(listUIDs, TargetFolder);
                    // update the unread count on folder f. We are moving (unread) mails to f
                    MovedMailCount.Add(key: TargetFolder, value: listUIDs.Count());
                }

                return MovedMailCount;
            }
            catch (Exception ex)
            {
                if (ex is ImapProtocolException || ex is IOException)
                {
                    await Utility.ReconnectAsync(client);
                }
                return null;
            }
        }
    }
}
