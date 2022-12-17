using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    // Class representation of an attachment
    internal class Attachment
    {
        public string? filepath { get; set; } // The filepath to the attachment if the attachment is stored locally on the computer

        public string? name { get; set; }     // The file name of the attachment

    }
}
