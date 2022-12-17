using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    internal class Searcher
    {
        private IMailFolder currentFolder;

        public Searcher(IMailFolder current)
        {
            currentFolder = current;
        }


        public SearchQuery? GetSearchQueryFromFilter(Filter filter)
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


        // Searches the current folder for a given target string but it only looks at locations specified in "locations" variable
        // Namely the list of "locations" should contain the strings "Subject", "Sender" and "Body" if we are to search these. 
        // Method returns a list of unique ids in the current folder that matches the search criterion. 
        public IList<UniqueId>? Search(List<string> locations, string target)
        {
            if (!currentFolder.IsOpen) currentFolder.Open(FolderAccess.ReadWrite);
            SearchQuery query;

            if (locations.Count <= 0) return null;
            if (locations.Contains("Sender") && locations.Contains("Subject") && locations.Contains("Body"))
            {
                query = SearchQuery.FromContains(target).Or(SearchQuery.SubjectContains(target)).Or(SearchQuery.BodyContains(target));
            }
            else if (!locations.Contains("Sender") && locations.Contains("Subject") && locations.Contains("Body"))
            {
                query = SearchQuery.SubjectContains(target).Or(SearchQuery.BodyContains(target));
            }
            else if(locations.Contains("Sender") && !locations.Contains("Subject") && locations.Contains("Body"))
            {
                query = SearchQuery.FromContains(target).Or(SearchQuery.BodyContains(target));
            }
            else if (locations.Contains("Sender") && locations.Contains("Subject") && !locations.Contains("Body"))
            {
                query = SearchQuery.FromContains(target).Or(SearchQuery.SubjectContains(target));
            }
            else if (locations.Contains("Sender") && !locations.Contains("Subject") && !locations.Contains("Body"))
            {
                query = SearchQuery.FromContains(target);
            }
            else if (!locations.Contains("Sender") && locations.Contains("Subject") && !locations.Contains("Body"))
            {
                query = SearchQuery.SubjectContains(target);
            }
            else if (!locations.Contains("Sender") && !locations.Contains("Subject") && locations.Contains("Body"))
            {
                query = SearchQuery.BodyContains(target);
            }
            else
            {
                return null;
            }
            return Search(query);
        }



        // Wrapper for async search method of mailkit
        public Task<IList<UniqueId>> SearchAsync(SearchQuery query)
        {
            return currentFolder.SearchAsync(query);
        }


        // Wrapper for synchronous search method of mailkit. 

        public IList<UniqueId> Search(SearchQuery query)
        {
            return currentFolder.Search(query);
        }





    }
}
