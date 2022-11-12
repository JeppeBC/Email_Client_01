using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Email_Client_01
{
    public partial class metrics : Form
    {
        public metrics()
        {
            InitializeComponent();

            double[] values = { 26, 20, 23, 7, 16, 2, 3 };
            double[] positions = { 0, 1, 2, 3, 4, 5, 6 };
            string[] labels = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            formsPlot1.Plot.AddBar(values, positions);
            formsPlot1.Plot.XTicks(positions, labels);
            formsPlot1.Plot.SetAxisLimits(yMin: 0);
            formsPlot1.Refresh();
        }
    }
}
