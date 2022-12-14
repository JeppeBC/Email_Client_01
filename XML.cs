using MailKit;
using MailKit.Net.Imap;
using Org.BouncyCastle.Utilities.Date;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Email_Client_01
{
    internal class XML_Test
    {
        // Create an XML file
        public static void CreateXML(IList<IMessageSummary> summaries)
        {
            var days = new List<DateTime>();
            var recieved_amount = new List<int>();
            var sent_amount = new List<int>();

            CultureInfo ci = CultureInfo.InvariantCulture;

            // Hardcoded temp value
            DateTime startdate = DateTime.ParseExact("01-01-2018", "dd-MM-yyyy", ci);

            // List of days, excluding current
            for (var dt = startdate; dt < DateTime.Today; dt = dt.AddDays(1))
            {
                days.Add(dt);
                recieved_amount.Add(0);
                sent_amount.Add(0);
            }

            foreach(var summary in summaries)
            {
                
                // Inside interval to be updated?
                if (summary.Date.UtcDateTime.Date >= days[0] && summary.Date.UtcDateTime.Date <= days.Last())
                {
                    for (int i = 0; i < days.Count; i++)
                    {
                        if (summary.Date.UtcDateTime.Date == days[i])
                        {
                            if (summary.Folder.Attributes.HasFlag(FolderAttributes.Sent))
                            {
                                sent_amount[i]++;
                            }
                            else
                            {
                                recieved_amount[i]++;
                            }
                        }
                    }
                }
            }

            string myTempFile = Path.Combine(Path.GetTempPath(), "root.xml");

            // Check if file exists, if not create start template file
            if (!File.Exists(myTempFile))
            {
                XDocument xmlFile = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"));
                xmlFile.Add(new XElement("Days"));
                xmlFile.Save(myTempFile);
            }
            

            XElement root = XElement.Load(myTempFile);

            for (int i = 0; i < days.Count; i++)
            {
                root.Add(new XElement("Day",
                    new XElement("Date", days[i].ToShortDateString()),
                    new XElement("Recieved", recieved_amount[i]),
                    new XElement("Sent", sent_amount[i])));
            }

            root.Save(myTempFile);
        }
    }
}
