﻿using System;
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
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
                
        private void EmailAddress_Click(object sender, EventArgs e)
        {
            textBox1.ResetText();
            textBox1.ForeColor = System.Drawing.Color.Black;
        }

        private void Password_Click(object sender, EventArgs e)
        {
            textBox2.ResetText();
            textBox2.ForeColor = System.Drawing.Color.Black;
        }

        private void LogIn_button(object sender, EventArgs e)
        {
            Inboxes Inbox = new Inboxes();
            Inbox.Show();
        }

        private void Exit_button(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
