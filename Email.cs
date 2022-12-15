using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    public class Email
    {
        public bool isDraft = false;
        public List<MailboxAddress> To { get; set; }

        public List<MailboxAddress> CC { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }

        public List<string?> LocalAttachments { get; set; }

        public IEnumerable<MimeEntity>? nonlocalAttachments { get; set; }

        public Email(string[] to, string[]? cc, string subject, string content, List<string?> localAttachments, IEnumerable<MimeEntity>? nonLocal = null)
        {
            To = new List<MailboxAddress>();
            // username and address, #TODO currently we do not have aliases but extend this once we do

            foreach (var recipient in to)
            {
                To.Add(MailboxAddress.Parse(recipient));
            }


            CC = new List<MailboxAddress>();
            if (!(cc == null))
            {
                foreach (var c in cc)
                {
                    CC.Add(MailboxAddress.Parse(c));
                }
            }

            Subject = subject;
            Content = content;
            LocalAttachments = localAttachments;
            nonlocalAttachments = nonLocal;
        }
    }
}
