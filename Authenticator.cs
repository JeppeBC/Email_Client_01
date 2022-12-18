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
    // Concrete Class for authenticating login credentials
    public class Authenticator : IAuthenticator
    {

        private string ErrorMessage; // Attribute to store an error message if anything goes wrong.

        public Authenticator()
        {
            ErrorMessage = "";
        }
        public string GetErrorMessage()
        {
            return ErrorMessage;
        }

        // Function to validate the username/email 
        private bool isUsernameInvalid(string username)
        {
            if (!EmailValidator.Validate(username))
            {
                ErrorMessage = "Please enter a valid email address";
                return true;
            }
            return false;

        }


        // Function to validate the password
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
            // Preliminary testing if the password and username seem valid. If not return early. 
            if(isUsernameInvalid(username) || isPasswordInvalid(password)) return null;

            // Check if the combination of username + password is valid. If we can establish and authorize an IMAP
            // client connection, we accept the user credentials.
            var client = Utility.GetImapClient();
            if (client.IsConnected && client.IsAuthenticated) return client;

            // Failed to connect/authorize to the IMAP client, timed out. 
            ErrorMessage = "Could not authorize the given credentials";
            return null;
        }
    }
}
