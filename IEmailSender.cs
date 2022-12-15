using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Email_Client_01.NewMail;

namespace Email_Client_01
{
    public interface IEmailSender
    {
        void sendEmail(Email email);
    }
}
