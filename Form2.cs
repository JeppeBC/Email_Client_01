using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Email_Client_01
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // send email from "richTextBox_1" to "richTextBox_2" with subject "richTextBox_3", and message "richTextBox_4".
            MessageBox.Show("Email sent");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.TopMost= false;
            Process.Start("explorer.exe", @"C:\User");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            richTextBox1.ResetText();
            richTextBox1.ForeColor = System.Drawing.Color.Black;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void richTextBox2_Click(object sender, EventArgs e)
        {
            richTextBox2.ResetText();
            richTextBox2.ForeColor = System.Drawing.Color.Black;
        }

        private void richTextBox3_Click(object sender, EventArgs e)
        {
            richTextBox3.ResetText();
            richTextBox3.ForeColor = System.Drawing.Color.Black;
        }

        private void richTextBox4_click(object sender, EventArgs e)
        {
            richTextBox4.ResetText();
            richTextBox4.ForeColor = System.Drawing.Color.Black;
        }
    }
}
