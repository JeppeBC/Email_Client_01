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
    public partial class NewMail : Form
    {
        public NewMail()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // read from username
        }
            
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
               
        private void From_Click(object sender, EventArgs e)
        {
            richTextBox1.ResetText();
            richTextBox1.ForeColor = System.Drawing.Color.Black;
        }

        private void To_Click(object sender, EventArgs e)
        {
            richTextBox2.ResetText();
            richTextBox2.ForeColor = System.Drawing.Color.Black;
        }

        private void Subject_Click(object sender, EventArgs e)
        {
            richTextBox3.ResetText();
            richTextBox3.ForeColor = System.Drawing.Color.Black;
        }

        private void Mail_click(object sender, EventArgs e)
        {
            richTextBox4.ResetText();
            richTextBox4.ForeColor = System.Drawing.Color.Black;
        }

        private void Attach_file(object sender, EventArgs e)
        {
            this.TopMost = false;
            Process.Start("explorer.exe", @"C:\User");
        }

        private void Send_mail(object sender, EventArgs e)
        {
            // send email from "richTextBox_1" to "richTextBox_2" with subject "richTextBox_3", and message "richTextBox_4".
            MessageBox.Show("Email sent");
        }

        private void Exit_button(object sender, EventArgs e)
        {
            this.Close();            
        }
    }
}
