using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    internal class Filter
    {
        public string? Name { get; set; }

        public string? DestinationFolder { get; set; }

        public List<string>? SearchLocations { get; set; }

        public string? TargetString { get; set; }

    }
}
