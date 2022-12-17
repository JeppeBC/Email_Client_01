using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{

    // This file is an amalgamation of handy methods to check for the characteristics of a given folder based on the attributes of SpecialFolders
    internal class SpecialFolders
    {
        // Method returns true if folder f should not display a count of unread mails. 
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


        // Method returns true if folder does not have any unread mails.
        public static bool isFolderWithoutUnreads(IMailFolder? f)
        {
            if (f == null) return false;
            if (f.Attributes.HasFlag(FolderAttributes.Sent) ||
               f.Attributes.HasFlag(FolderAttributes.Drafts)
               ) return true;
            return false;
        }


        // At the moment this is identical with  isFolderUnreadBlacklisted() however we can easily imagine the behaviour of these
        // to eventually differ. Thus, we have two methods.
        // Method returns true if filters should not work on (move mails from) folder f. 
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


        // If this method returns true, we should not allow mails to be moved to to folder f.
        public static bool isFolderMoveMailToThisBlacklisted(IMailFolder? f)
        {
            if (f == null) return false;
            if (f.Attributes.HasFlag(FolderAttributes.Drafts) ||
                f.Attributes.HasFlag(FolderAttributes.Sent) ||
                f.Attributes.HasFlag(FolderAttributes.All)
                ) return true;
            return false;
        }


        // If this method returns true, the folders should have a displayed count of the number of mails in it.
        public static bool isFolderDisplayAllCount(IMailFolder? f)
        {
            if (f == null) return false;

            // could add f.Attributes.HasFlag(FolderAttributes.Drafts) here if we wanted this for drafts.
            if (f.Attributes.HasFlag(FolderAttributes.Flagged)   ||
                f.Attributes.HasFlag(FolderAttributes.Important) ||
                f.Attributes.HasFlag(FolderAttributes.Drafts)
                ) return true;
            return false;
        }
    }
}
