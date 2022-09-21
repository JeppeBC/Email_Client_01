namespace Email_Client_01
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
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
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.croup7MailToolStripMenuItem,
            this.sendToolStripMenuItem,
            this.inboxToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(191, 750);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // croup7MailToolStripMenuItem
            // 
            this.croup7MailToolStripMenuItem.Name = "croup7MailToolStripMenuItem";
            this.croup7MailToolStripMenuItem.Size = new System.Drawing.Size(178, 36);
            this.croup7MailToolStripMenuItem.Text = "Croup 7 - Mail";
            this.croup7MailToolStripMenuItem.Click += new System.EventHandler(this.croup7MailToolStripMenuItem_Click);
            // 
            // sendToolStripMenuItem
            // 
            this.sendToolStripMenuItem.Name = "sendToolStripMenuItem";
            this.sendToolStripMenuItem.Size = new System.Drawing.Size(178, 36);
            this.sendToolStripMenuItem.Text = "Send";
            this.sendToolStripMenuItem.Click += new System.EventHandler(this.sendToolStripMenuItem_Click);
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
            this.inboxToolStripMenuItem.Size = new System.Drawing.Size(178, 36);
            this.inboxToolStripMenuItem.Text = "E-Mail";
            this.inboxToolStripMenuItem.Click += new System.EventHandler(this.inboxToolStripMenuItem_Click);
            // 
            // inboxToolStripMenuItem1
            // 
            this.inboxToolStripMenuItem1.Name = "inboxToolStripMenuItem1";
            this.inboxToolStripMenuItem1.Size = new System.Drawing.Size(270, 40);
            this.inboxToolStripMenuItem1.Text = "Inbox";
            this.inboxToolStripMenuItem1.Click += new System.EventHandler(this.inboxToolStripMenuItem1_Click);
            // 
            // draftsToolStripMenuItem
            // 
            this.draftsToolStripMenuItem.Name = "draftsToolStripMenuItem";
            this.draftsToolStripMenuItem.Size = new System.Drawing.Size(270, 40);
            this.draftsToolStripMenuItem.Text = "Drafts";
            // 
            // sentMailsToolStripMenuItem
            // 
            this.sentMailsToolStripMenuItem.Name = "sentMailsToolStripMenuItem";
            this.sentMailsToolStripMenuItem.Size = new System.Drawing.Size(270, 40);
            this.sentMailsToolStripMenuItem.Text = "Sent mails";
            // 
            // spamToolStripMenuItem
            // 
            this.spamToolStripMenuItem.Name = "spamToolStripMenuItem";
            this.spamToolStripMenuItem.Size = new System.Drawing.Size(270, 40);
            this.spamToolStripMenuItem.Text = "Spam";
            // 
            // trashcanToolStripMenuItem1
            // 
            this.trashcanToolStripMenuItem1.Name = "trashcanToolStripMenuItem1";
            this.trashcanToolStripMenuItem1.Size = new System.Drawing.Size(270, 40);
            this.trashcanToolStripMenuItem1.Text = "Trashcan";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(191, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(952, 33);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(34, 28);
            this.toolStripButton1.Text = "toolStripButton1";
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
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
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
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(34, 28);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1143, 750);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
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
    }
}