using Email_Client_01;
using MailKit;
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
    internal class FilterUser
    {
        private List<Filter> FilterList;            // container for filters
        private HashSet<string> FilterSet = new(); // For bookkeeping which mails have been moved.

        private IMailFolder folder;             // current folder
        private IList<IMailFolder> folders;     // all folders

        public FilterUser(IMailFolder folder, IList<IMailFolder> folders)
        {
            this.folder = folder;
            this.folders = folders;


            // filepath of filter.json file
            // if file does not exist
            if (!File.Exists(Utility.JsonFilePath))
            {
                // create the file
                File.Create(Utility.JsonFilePath).Close();
            }
            // Read the entire file and De-serialize to list of filters
            FilterList = Utility.JsonFileReader.Read<List<Filter>>(Utility.JsonFilePath) ?? new List<Filter>();


        }

        ~FilterUser()
        {
            if (FilterList == null) return;
            Utility.JsonFileWriter.Write<List<Filter>>(Utility.JsonFilePath, FilterList.ToList());
        }


        // returns number of filtered mails
        /*public int FilterFolder(IMailFolder FolderToFilter)
        {
            if()
        }

        if (!isFilterBlacklisted(folder))
        {

            foreach (var filter in FilterList)
            {
                // get the destination folder;
                IMailFolder? f = folders.Where(folder => folder.FullName == filter.DestinationFolder).FirstOrDefault();

                if (f == folder) continue; // No need to move anything to the same folder it is currently in
                if (f == null || !f.Exists) continue; //guards that should never happen but just in case
                if (isFolderMoveMailToThisBlacklisted(f)) continue; // again a guard that should not happen but we make sure.


                var query = GetSearchQueryFromFilter(filter);
                if (query == null) continue;

                // find all the mails to be moved
                // takes like 150-200 ms per filter, slightly (almost negligible) extra ammount from moving the mail.
                var listUIDs = await folder.SearchAsync(query.And(SearchQuery.DeliveredAfter(Properties.Time.Default.Date)));



                // This similarly takes 100-200ms per filter, this following segment prevens the filter collisions
                // where you can have one filter move one mail to say spam and another one that moves the same mail to inbox or whatever.
                // In this scenario the user never sees the mail, but only the count go up and down when it is being moved. 
                // This block ensures that we only filter a mail at most once. However it is a somewhat expensive call as we access the imap client.
                // We do this as the Unique IDs are only folder specific, whereas the emailid is a globally unique identifier. 
                if (listUIDs.ToList().Count <= 0) continue; // if nothing to move we should not do expensive calls to folder.fetch or folder.movetoasync

                var summaries = folder.Fetch(listUIDs, MessageSummaryItems.EmailId | MessageSummaryItems.UniqueId);
                foreach (var sum in summaries)
                {
                    // if the email is already in the filterset
                    if (!FilterSet.Add(sum.EmailId))
                    {
                        // remove the corresponding uid from the list of uids to be moved.
                        listUIDs.Remove(listUIDs.Where(uid => uid == sum.UniqueId).FirstOrDefault());
                    }
                }

                await folder.MoveToAsync(listUIDs, f);
                // update the unread count on folder f. We are moving (unread) mails to f
                IncrementFolderCount(f, value: listUIDs.Count());
                // We are moving a message before it is shown in the current listbox, so we do not need to update the count of current folder. 
            }
        }*/



    }
}
