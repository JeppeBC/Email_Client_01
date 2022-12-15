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

        public Task<IList<UniqueId>> SearchAsync(SearchQuery query)
        {
            return currentFolder.SearchAsync(query);
        }

        public IList<UniqueId> Search(SearchQuery query)
        {
            return currentFolder.Search(query);
        }
    }
}
