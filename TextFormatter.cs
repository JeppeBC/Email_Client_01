using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    internal class TextFormatter
    {
        public static string? GetFlags(IMessageSummary item)
        {
            string FlagString = "";

            if (item.Flags == null)
                return null;

            if (item.Flags.Value.HasFlag(MessageFlags.Flagged))
            {
                FlagString += "(FLAGGED) ";
            }

            if (item.Flags.Value.HasFlag(MessageFlags.Draft))
            {
                FlagString += "(DRAFT) ";
            }

            if (!(item.Flags.Value.HasFlag(MessageFlags.Seen)))
            {
                FlagString += "(UNREAD) ";
            }

            return FlagString;
        }

        public static string GetSubject(IMessageSummary item)
        {
            string subject = "";
            if (item.Envelope.Subject != null)
            {
                subject += item.Envelope.Subject;
            }

            // mutate the item in case of null
            else
            {
                item.Envelope.Subject = "<no subject>";
                subject += item.Envelope.Subject;
            }
            return subject;
        }

        public static string GetSender(IMessageSummary item)
        {
            // if an alias is present, i.e. the name and not the actual mailaddress, then return that
            if (item.Envelope.Sender.Count > 0 && !string.IsNullOrEmpty(item.Envelope.Sender[0].Name))
                return item.Envelope.Sender[0].Name;
            // else check for the first actual mail address of the sender(s), if non found return empty string;
            return item.Envelope.From.Mailboxes.FirstOrDefault()?.Address ?? "";
        }

        public static string FormatInboxText(IMessageSummary item)
        {
            string subject = GetSubject(item);
            string? flags = GetFlags(item);
            string sender = GetSender(item);

            string result;
            if (flags == null)
            {
                result = sender + ": " + subject;
            }
            else
            {
                result = flags + sender + ": " + subject;
            }
            return result;
        }


        public static string FormatDraftInboxText(IMessageSummary item)
        {
            return "(DRAFT) " + GetSubject(item);
        }

    }
}
