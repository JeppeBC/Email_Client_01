using ScottPlot;
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
            displayPeriodDropdown.SelectedIndex = 0;
            mailTypeDropdown.SelectedIndex = 0;
            SetMyCustomFormat();



            Render_Metrics();

        }

        // Changes date selector depending on selected representation in combobox
        private void SetMyCustomFormat()
        {

            // Year
            if (displayPeriodDropdown.SelectedIndex == 0)
            {
                displayPeriodDateSelector.Format = DateTimePickerFormat.Custom;
                displayPeriodDateSelector.CustomFormat = "yyyy";
                displayPeriodDateSelector.ShowUpDown = true;
            }

            // Month
            else if (displayPeriodDropdown.SelectedIndex == 1)
            {
                displayPeriodDateSelector.Format = DateTimePickerFormat.Custom;
                displayPeriodDateSelector.CustomFormat = "MM-yyyy";
                displayPeriodDateSelector.ShowUpDown = true;
            }

            // Week
            else
            {
                displayPeriodDateSelector.Format = DateTimePickerFormat.Short;
                displayPeriodDateSelector.ShowUpDown = false;
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

        // Day labels
        private void Render_Plot(double[] positions, double[] y, string[] labels)
        {
            formsPlot1.Plot.Clear();
            BarPlot = formsPlot1.Plot.AddBar(y, positions);
            Color_Bar();
            formsPlot1.Plot.XAxis.DateTimeFormat(false);
            formsPlot1.Plot.XTicks(positions, labels);
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Plot.SetAxisLimits(yMin: 0);
            formsPlot1.Refresh();
        }

        // Date format
        private void Render_Plot(double[] positions, double[] y)
        {

            formsPlot1.Plot.Clear();
            formsPlot1.Plot.XAxis.AutomaticTickPositions();
            formsPlot1.Plot.XAxis.DateTimeFormat(true);
            BarPlot = formsPlot1.Plot.AddBar(y, positions);
            Color_Bar();
            formsPlot1.Plot.AxisAuto();
            formsPlot1.Plot.SetAxisLimits(yMin: 0);
            formsPlot1.Refresh();
        }

        private void Color_Bar()
        {
            switch (mailTypeDropdown.SelectedIndex)
            {
                case 0:
                    BarPlot.Color = ColorTranslator.FromHtml("#1F77B4");
                    break;

                case 1:
                    BarPlot.Color = ColorTranslator.FromHtml("#FF7F0E");
                    break;

                default:
                    break;
            }
        }

        private void Render_Metrics()
        {
            DateTime selected_date = displayPeriodDateSelector.Value.Date;

            // Show for year
            if (displayPeriodDropdown.SelectedIndex == 0)
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

                    if (mailTypeDropdown.SelectedIndex == 0)
                    {
                        Render_Plot(positions, recieved_month_array, month_labels);
                    }

                    else
                    {
                        Render_Plot(positions, sent_month_array, month_labels);
                    }


                    
                }
            }

            else if (displayPeriodDropdown.SelectedIndex == 1)
            {
                // Do month stuff
                int year = selected_date.Year;
                int month = selected_date.Month;
                int days_in_month = DateTime.DaysInMonth(year, month);
                DateTime start = DateTime.Parse("01-"+month+"-"+year);
                DateTime end = DateTime.Parse(days_in_month + "-" + month + "-" + year);

                var days = Get_Days(start, end.AddDays(1));


                // Get recieved and sent as list
                var recieved = from e in days.Elements("Recieved")
                                          select (double.Parse(e.Value));

                var sent = from e in days.Elements("Sent")
                                 select (double.Parse(e.Value));


                double[] recieved_array = new double[days_in_month];
                double[] sent_array = new double[days_in_month];

                if (recieved.Count() != 0 && sent.Count() != 0)
                {
                    // Convert to array
                    recieved_array = recieved.Select(x => (double)x).ToArray();
                    sent_array = sent.Select(x => (double)x).ToArray();
                }

                


                // space every time point by 1 day from a starting point
                double[] positions = new double[recieved_array.Length];
                for (int i = 0; i < recieved_array.Length; i++)
                    positions[i] = start.AddDays(i).ToOADate();

                if (mailTypeDropdown.SelectedIndex == 0)
                {
                    Render_Plot(positions, recieved_array);
                }

                else
                {
                    Render_Plot(positions, sent_array);
                }

            }
            else
            {
                // Do week stuff
            }

            return;
        }

        /* Event handlers */

        private void displayPeriodDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMyCustomFormat();
            Render_Metrics();
        }

        private void displayPeriodDateSelector_ValueChanged_1(object sender, EventArgs e)
        {
            Render_Metrics();
        }

        private void mailTypeDropdown_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Render_Metrics();
        }
    }
}