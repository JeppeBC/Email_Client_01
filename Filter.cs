using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{

    // Class representation of our custom filters
    internal class Filter
    {
        public string? Name { get; set; }                   // Name of the filter

        public string? DestinationFolder { get; set; }      // Which folder the filter is filtering to

        public List<string>? SearchLocations { get; set; }  // Where the filter is looking. Each string is a part of the mail, the filter searches.
                                                            // Can be an arbitrary combination of: Subject, Sender, Body.
        public string? TargetString { get; set; }           // The string the filter is searching for. Mails containing this string in a SearchLocation
                                                            // Will be moved to the destination folder. 

    }
}
