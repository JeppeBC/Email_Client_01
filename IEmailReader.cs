using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    internal interface IEmailReader
    {
        public Task ReadMessage(IMessageSummary messageItem);
    }
}
