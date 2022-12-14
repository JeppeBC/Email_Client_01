using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static ScottPlot.Plottable.PopulationPlot;

namespace Email_Client_01
{
    public partial class metrics : Form
    {
        private readonly XElement root;
        private readonly CultureInfo ci;
        private ScottPlot.Plottable.BarPlot BarPlot;

        public metrics()
        {
            

            InitializeComponent();

            // Load XML file
            string myTempFile = Path.Combine(Path.GetTempPath(), "root.xml");
            root = XElement.Load(myTempFile);
            // Set cultureinfo variable
            ci = CultureInfo.InvariantCulture;

            // Initialize mode dropdown selector
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            SetMyCustomFormat();



            Render_Metrics();


            /*DateTime start = DateTime.ParseExact("01-01-2022", "dd-MM-yyyy", ci);
            DateTime end = DateTime.ParseExact("01-11-2022", "dd-MM-yyyy", ci);

            IEnumerable<XElement> days = Get_Days(start, end);



            // Array of Recieved values

            IEnumerable<string> recieved =
                from e in days.Elements("Recieved")
                select e.Value;

            double[] recieved_array = recieved.Select(double.Parse).ToArray();

            // Sent

            IEnumerable<string> sent =
                from e in days.Elements("Sent")
                select e.Value;

            double[] sent_array = sent.Select(double.Parse).ToArray();

            // Date

            IEnumerable<string> date =
               from e in days.Elements("Date")
               select e.Value;

            string[] date_array = date.ToArray();


            // Amount of total days
            int size = date.Count();
            double[] positions;

            DateTime[] month_starts = {
                DateTime.ParseExact("01-01-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-02-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-03-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-04-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-05-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-06-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-07-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-08-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-09-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-10-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-11-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-12-2022", "dd-MM-yyyy", ci),
                DateTime.ParseExact("01-01-2023", "dd-MM-yyyy", ci)
            };

            // Group into months
            double[] recieved_month_array = new double[12];
            double[] sent_month_array = new double[12];
            positions = new double[12];
            for (int i = 0; i < 12; i++)
            {
                positions[i] = i;
            }

            for (int i = 0; i < 12; i++)
            {
                // Get data from 1 month
                IEnumerable<XElement> test =
                from el in root.Descendants("Day")
                where DateTime.ParseExact((string)(el.Element("Date")), "dd-MM-yyyy", ci) >= month_starts[i]
                && DateTime.ParseExact((string)(el.Element("Date")), "dd-MM-yyyy", ci) < month_starts[i + 1]
                select el;

                // Count amount of emails recieved
                IEnumerable<string> recieved_test =
                from e in test.Elements("Recieved")
                select e.Value;

                foreach (var e in recieved_test)
                {
                    recieved_month_array[i] += double.Parse(e);
                }


                // Count amount of emails sent
                IEnumerable<string> sent_test =
                from e in test.Elements("Sent")
                select e.Value;

                foreach (var e in sent_test)
                {
                    sent_month_array[i] += double.Parse(e);
                }
            }


            *//*double[] positions = new double[size];
            for (int i = 0; i < size; i++)
            {
                positions[i] = i;
            }*//*

            string[] month_labels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string[] labels_weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            formsPlot1.Plot.AddBar(recieved_month_array, positions);
            formsPlot1.Plot.XTicks(positions, month_labels);
            formsPlot1.Plot.SetAxisLimits(yMin: 0);
            formsPlot1.Refresh();*/
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMyCustomFormat();
            // Render_Metrics here
        }

        // Changes date selector depending on selected representation in combobox
        private void SetMyCustomFormat()
        {

            // Year
            if (comboBox1.SelectedIndex == 0)
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "yyyy";
                dateTimePicker1.ShowUpDown = true;
            }

            // Month
            else if (comboBox1.SelectedIndex == 1)
            {
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "MM-yyyy";
                dateTimePicker1.ShowUpDown = true;
            }

            // Week
            else
            {
                dateTimePicker1.Format = DateTimePickerFormat.Short;
                dateTimePicker1.ShowUpDown = false;
            }
        }

        private IEnumerable<XElement> Get_Days(DateTime start, DateTime end)
        {
            IEnumerable<XElement> days =
                from el in root.Descendants("Day")
                where DateTime.ParseExact((string)el.Element("Date"), "dd-MM-yyyy", ci) >= start
                && DateTime.ParseExact((string)el.Element("Date"), "dd-MM-yyyy", ci) < end
                select el;

            return days;
        }

        private void Render_Plot(double[] positions, double[] y, string[] labels)
        {
            formsPlot1.Plot.Clear();
            BarPlot = formsPlot1.Plot.AddBar(y, positions);
            formsPlot1.Plot.XTicks(positions, labels);
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Plot.SetAxisLimits(yMin: 0);
            formsPlot1.Refresh();
        }

        private void Render_Metrics()
        {
            DateTime selected_date = dateTimePicker1.Value.Date;

            // Show for year
            if (comboBox1.SelectedIndex == 0)
            {
                string[] month_labels = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                int year = selected_date.Year;
                /*DateTime start = DateTime.ParseExact("01-01-" + year, "dd-MM-yyyy", ci);
                DateTime end = DateTime.ParseExact("31-12-" + year, "dd-MM-yyyy", ci);

                var days = Get_Days(start, end);*/

                DateTime[] month_starts =
                {
                    DateTime.ParseExact("01-01-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-02-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-03-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-04-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-05-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-06-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-07-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-08-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-09-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-10-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-11-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-12-"+year, "dd-MM-yyyy", ci),
                    DateTime.ParseExact("01-01-"+(year+1), "dd-MM-yyyy", ci)
                };

                // Group into months
                double[] recieved_month_array = new double[12];
                double[] sent_month_array = new double[12];
                double[] positions = new double[12];
                for (int i = 0; i < 12; i++)
                {
                    positions[i] = i;
                }

                for (int i = 0; i < 12; i++)
                {
                    // Get data from 1 month
                    IEnumerable<XElement> month = Get_Days(month_starts[i], month_starts[i + 1]);
                    

                    // Count amount of emails recieved
                    IEnumerable<string> recieved_month =
                        from e in month.Elements("Recieved")
                        select e.Value;

                    foreach (var e in recieved_month)
                    {
                        recieved_month_array[i] += double.Parse(e);
                    }


                    // Count amount of emails sent
                    IEnumerable<string> sent_month =
                        from e in month.Elements("Sent")
                        select e.Value;

                    foreach (var e in sent_month)
                    {
                        sent_month_array[i] += double.Parse(e);
                    }

                    if (comboBox2.SelectedIndex == 0)
                    {
                        Render_Plot(positions, recieved_month_array, month_labels);
                    }

                    else
                    {
                        Render_Plot(positions, sent_month_array, month_labels);
                    }


                    
                }
            }

            else if (comboBox1.SelectedIndex == 1)
            {
                // Do month stuff
            }
            else
            {
                // Do week stuff
            }

            return;
        }

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
            Render_Metrics();
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Render_Metrics();
        }
    }
}