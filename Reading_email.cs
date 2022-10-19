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
    public partial class Reading_email : Form
    {
        public Reading_email()
        {
            InitializeComponent();
        }

        private void Exit_button(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
