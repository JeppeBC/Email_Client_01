using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    // In this file we implement the observer pattern that allows us to detect incoming emails and automatically update/fetch them.

    // 1) Below is the generic abstract Subject class 

    // Only allows subscribers that implement a Reload() method. 
    public abstract class ReceiverSubject<Subscriber> where Subscriber : ISubscriber
    {
        private List<Subscriber> Subscribers = new List<Subscriber>();
        private int _MessageCountChanged = 0;

        public void Attach(Subscriber sub)
        {
            Subscribers.Add(sub);
        }

        public void Detach(Subscriber sub)
        {
            Subscribers.Remove(sub);
        }

        public void Notify()
        {
            foreach (var subscriber in Subscribers)
            {
                // Update reserved keyword
                subscriber.Reload();
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

    // 2) Below is the concrete subject. 
    // Whenever the MessageCountChanged variable is mutated, Notify() is called.
    // Trigerring Reload() of the subscribers.
    public class DynamicReciever : ReceiverSubject<Inboxes>
    {
    }
}
