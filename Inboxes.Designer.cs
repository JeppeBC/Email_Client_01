﻿namespace Email_Client_01
{
    partial class Inboxes
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inboxes));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.croup7MailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inboxToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.draftsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sentMailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trashcanToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.Inbox = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.croup7MailToolStripMenuItem,
            this.sendToolStripMenuItem,
            this.inboxToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(167, 663);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // croup7MailToolStripMenuItem
            // 
            this.croup7MailToolStripMenuItem.Name = "croup7MailToolStripMenuItem";
            this.croup7MailToolStripMenuItem.Size = new System.Drawing.Size(154, 36);
            this.croup7MailToolStripMenuItem.Text = "Prime - Mail";
            this.croup7MailToolStripMenuItem.Click += new System.EventHandler(this.Prime_Mail_Homepage);
            // 
            // sendToolStripMenuItem
            // 
            this.sendToolStripMenuItem.Name = "sendToolStripMenuItem";
            this.sendToolStripMenuItem.Size = new System.Drawing.Size(154, 36);
            this.sendToolStripMenuItem.Text = "Send";
            this.sendToolStripMenuItem.Click += new System.EventHandler(this.SendNewMail);
            // 
            // inboxToolStripMenuItem
            // 
            this.inboxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inboxToolStripMenuItem1,
            this.draftsToolStripMenuItem,
            this.sentMailsToolStripMenuItem,
            this.spamToolStripMenuItem,
            this.trashcanToolStripMenuItem1});
            this.inboxToolStripMenuItem.Name = "inboxToolStripMenuItem";
            this.inboxToolStripMenuItem.Size = new System.Drawing.Size(154, 36);
            this.inboxToolStripMenuItem.Text = "E-Mails";
            this.inboxToolStripMenuItem.Click += new System.EventHandler(this.Emails_Click);
            // 
            // inboxToolStripMenuItem1
            // 
            this.inboxToolStripMenuItem1.Name = "inboxToolStripMenuItem1";
            this.inboxToolStripMenuItem1.Size = new System.Drawing.Size(228, 40);
            this.inboxToolStripMenuItem1.Text = "Inbox";
            this.inboxToolStripMenuItem1.Click += new System.EventHandler(this.Inbox_Click);
            // 
            // draftsToolStripMenuItem
            // 
            this.draftsToolStripMenuItem.Name = "draftsToolStripMenuItem";
            this.draftsToolStripMenuItem.Size = new System.Drawing.Size(228, 40);
            this.draftsToolStripMenuItem.Text = "Drafts";
            // 
            // sentMailsToolStripMenuItem
            // 
            this.sentMailsToolStripMenuItem.Name = "sentMailsToolStripMenuItem";
            this.sentMailsToolStripMenuItem.Size = new System.Drawing.Size(228, 40);
            this.sentMailsToolStripMenuItem.Text = "Sent mails";
            // 
            // spamToolStripMenuItem
            // 
            this.spamToolStripMenuItem.Name = "spamToolStripMenuItem";
            this.spamToolStripMenuItem.Size = new System.Drawing.Size(228, 40);
            this.spamToolStripMenuItem.Text = "Spam";
            // 
            // trashcanToolStripMenuItem1
            // 
            this.trashcanToolStripMenuItem1.Name = "trashcanToolStripMenuItem1";
            this.trashcanToolStripMenuItem1.Size = new System.Drawing.Size(228, 40);
            this.trashcanToolStripMenuItem1.Text = "Trash bin";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(167, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(976, 33);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(34, 28);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.trashicon_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(34, 28);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(34, 28);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.RefreshPage_Click);
            // 
            // Inbox
            // 
            this.Inbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Inbox.FormattingEnabled = true;
            this.Inbox.ItemHeight = 25;
            this.Inbox.Location = new System.Drawing.Point(205, 79);
            this.Inbox.Name = "Inbox";
            this.Inbox.Size = new System.Drawing.Size(905, 529);
            this.Inbox.TabIndex = 6;
            this.Inbox.SelectedIndexChanged += new System.EventHandler(this.Inbox_SelectedIndexChanged);
            // 
            // Inboxes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(1143, 663);
            this.Controls.Add(this.Inbox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Inboxes";
            this.Text = "Prime Email";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem croup7MailToolStripMenuItem;
        private ToolStripMenuItem sendToolStripMenuItem;
        private ToolStripMenuItem inboxToolStripMenuItem;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton toolStripButton2;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButton3;
        private ToolStripMenuItem inboxToolStripMenuItem1;
        private ToolStripMenuItem draftsToolStripMenuItem;
        private ToolStripMenuItem sentMailsToolStripMenuItem;
        private ToolStripMenuItem spamToolStripMenuItem;
        private ToolStripMenuItem trashcanToolStripMenuItem1;
        private ListBox Inbox;
    }
}