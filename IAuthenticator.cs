using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    public interface IAuthenticator
    {
        ImapClient? Authenticate(string email, string password);

        string GetErrorMessage();

    }
}
