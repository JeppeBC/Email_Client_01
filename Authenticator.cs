using EmailValidation;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client_01
{
    public class Authenticator : IAuthenticator
    {


        public string? ErrorMessage { get; private set; }

        private bool isUsernameInvalid(string username)
        {
            if (!EmailValidator.Validate(username))
            {
                ErrorMessage = "Please enter a valid email address";
                return true;
            }
            return false;

        }

        private bool isPasswordInvalid(string password)
        {
            if (password.Length > 99 || password.Length < 8)
            {
                ErrorMessage = "Please enter a password between 8 and 99 characters (inclusive)";
                return true;
            }
            return false;
        }

        public ImapClient? Authenticate(string username, string password)
        {
            // Preliminary testing 
            if(isUsernameInvalid(username) || isPasswordInvalid(password)) return null;


            // Attempt to connect to the IMAP server, if we can connect and authorize to an IMAP client we accept
            var client = Utility.GetImapClient();
            if (client.IsConnected && client.IsAuthenticated) return client;

            // Failed to connect/authorize to the IMAP client
            ErrorMessage = "Could not authorize the given credentials";
            return null;
        }
    }
}
