using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    internal class SpecialFolders
    {
        public static bool isFolderUnreadBlacklisted(IMailFolder? f)
        {
            if (f == null) return false;
            if (f.Attributes.HasFlag(FolderAttributes.Trash) ||
                f.Attributes.HasFlag(FolderAttributes.Drafts) ||
                f.Attributes.HasFlag(FolderAttributes.Sent) ||
                f.Attributes.HasFlag(FolderAttributes.All) ||
                f.Attributes.HasFlag(FolderAttributes.Flagged) ||
                f.Attributes.HasFlag(FolderAttributes.Important) ||
                f.Attributes.HasFlag(FolderAttributes.Archive)
                ) return true;
            return false;
        }

        public static bool isFolderWithoutUnreads(IMailFolder? f)
        {
            if (f == null) return false;
            if (f.Attributes.HasFlag(FolderAttributes.Sent) ||
               f.Attributes.HasFlag(FolderAttributes.Drafts)
               ) return true;
            return false;
        }

        // At the moment one-to-one identical with the function above, however we can imagine these could end up doing two different things
        // so we make them into separate functions. 
        public static bool isFilterBlacklisted(IMailFolder? f)
        {
            if (f == null) return false;
            if (f.Attributes.HasFlag(FolderAttributes.Trash) ||
                f.Attributes.HasFlag(FolderAttributes.Drafts) ||
                f.Attributes.HasFlag(FolderAttributes.Sent) ||
                f.Attributes.HasFlag(FolderAttributes.All) ||
                f.Attributes.HasFlag(FolderAttributes.Flagged) ||
                f.Attributes.HasFlag(FolderAttributes.Important) ||
                f.Attributes.HasFlag(FolderAttributes.Archive)
                ) return true;
            return false;
        }

        public static bool isFolderMoveMailToThisBlacklisted(IMailFolder? f)
        {
            if (f == null) return false;
            if (f.Attributes.HasFlag(FolderAttributes.Drafts) ||
                f.Attributes.HasFlag(FolderAttributes.Sent) ||
                f.Attributes.HasFlag(FolderAttributes.All)
                ) return true;
            return false;
        }

        public static bool isFolderDisplayAllCount(IMailFolder? f)
        {
            if (f == null) return false;

            // could add f.Attributes.HasFlag(FolderAttributes.Drafts) here if we wanted this for drafts.
            if (f.Attributes.HasFlag(FolderAttributes.Flagged) ||
                f.Attributes.HasFlag(FolderAttributes.Important)
                ) return true;
            return false;
        }
    }
}
