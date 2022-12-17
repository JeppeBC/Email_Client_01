using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    // In this file we implement the observer pattern that allows us to detect incoming emails and automatically update/fetch them.

    // 1) Below is the generic abstract Subject class 
    public abstract class Subject
    {
        private List<Inboxes> Inboxes = new List<Inboxes>();
        private int _MessageCountChanged;

        public Subject(int messages)
        {
            this._MessageCountChanged = messages;
        }

        public void Attach(Inboxes inbox)
        {
            Inboxes.Add(inbox);
        }

        public void Detach(Inboxes inbox)
        {
            Inboxes.Remove(inbox);
        }

        public void Notify()
        {
            foreach (var inbox in Inboxes)
            {
                // Update reserved keyword
                inbox.Reload();
            }
        }

        public int MessageCountChanged
        {
            get { return _MessageCountChanged; }
            set
            {
                if (_MessageCountChanged != value)
                {
                    _MessageCountChanged = value;
                    Notify();
                }
            }
        }
    }

    // 2) Below is the concrete subject that we are observing. 
    // Whenever the MessageCountChanged variable is mutated, Notify() is called.
    public class InboxMessagesSubject : Subject
    {
        public InboxMessagesSubject(int MessageCountChanged) : base(MessageCountChanged)
        {
        }

    }
}
