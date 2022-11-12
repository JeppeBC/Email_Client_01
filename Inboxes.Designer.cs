namespace Email_Client_01
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.DeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToggleFlagButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.MoveToTrashButton = new System.Windows.Forms.ToolStripButton();
            this.Inbox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Compose = new System.Windows.Forms.Button();
            this.Folders = new System.Windows.Forms.ListBox();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchSenderCheck = new System.Windows.Forms.CheckBox();
            this.SearchSubjectCheck = new System.Windows.Forms.CheckBox();
            this.SearchContentCheck = new System.Windows.Forms.CheckBox();
            this.metrics_button = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteButton,
            this.toolStripSeparator2,
            this.ToggleFlagButton,
            this.toolStripSeparator1,
            this.RefreshButton,
            this.MoveToTrashButton});
            this.toolStrip1.Location = new System.Drawing.Point(242, 9);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(137, 31);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // DeleteButton
            // 
            this.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(28, 28);
            this.DeleteButton.Text = "toolStripButton1";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // ToggleFlagButton
            // 
            this.ToggleFlagButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToggleFlagButton.Image = ((System.Drawing.Image)(resources.GetObject("ToggleFlagButton.Image")));
            this.ToggleFlagButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.ToggleFlagButton.Name = "ToggleFlagButton";
            this.ToggleFlagButton.Size = new System.Drawing.Size(28, 28);
            this.ToggleFlagButton.Text = "toolStripButton2";
            this.ToggleFlagButton.Click += new System.EventHandler(this.ToggleFlagButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
            this.RefreshButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(28, 28);
            this.RefreshButton.Text = "RefreshButton";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshPage_Click);
            // 
            // MoveToTrashButton
            // 
            this.MoveToTrashButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveToTrashButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveToTrashButton.Image")));
            this.MoveToTrashButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveToTrashButton.Name = "MoveToTrashButton";
            this.MoveToTrashButton.Size = new System.Drawing.Size(28, 28);
            this.MoveToTrashButton.Text = "Moves selected item to trash folder";
            this.MoveToTrashButton.Click += new System.EventHandler(this.MoveToTrashButton_Click);
            // 
            // Inbox
            // 
            this.Inbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Inbox.FormattingEnabled = true;
            this.Inbox.ItemHeight = 15;
            this.Inbox.Location = new System.Drawing.Point(144, 54);
            this.Inbox.Margin = new System.Windows.Forms.Padding(2);
            this.Inbox.Name = "Inbox";
            this.Inbox.Size = new System.Drawing.Size(645, 334);
            this.Inbox.TabIndex = 6;
            this.Inbox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Inbox_MouseDoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(7, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 32);
            this.label2.TabIndex = 8;
            this.label2.Text = "Prime Mail";
            // 
            // Compose
            // 
            this.Compose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Compose.Location = new System.Drawing.Point(12, 54);
            this.Compose.Name = "Compose";
            this.Compose.Size = new System.Drawing.Size(127, 33);
            this.Compose.TabIndex = 9;
            this.Compose.Text = "Compose";
            this.Compose.UseVisualStyleBackColor = false;
            this.Compose.Click += new System.EventHandler(this.Compose_Click);
            // 
            // Folders
            // 
            this.Folders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Folders.FormattingEnabled = true;
            this.Folders.ItemHeight = 15;
            this.Folders.Location = new System.Drawing.Point(12, 98);
            this.Folders.Name = "Folders";
            this.Folders.Size = new System.Drawing.Size(127, 289);
            this.Folders.TabIndex = 10;
            this.Folders.SelectedIndexChanged += new System.EventHandler(this.Folders_SelectedIndexChanged);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(531, 22);
            this.SearchTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(187, 23);
            this.SearchTextBox.TabIndex = 11;
            this.SearchTextBox.Text = "Search in the current folder...";
            this.SearchTextBox.Click += new System.EventHandler(this.SearchTextBox_Click);
            this.SearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(729, 12);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(1);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(57, 27);
            this.SearchButton.TabIndex = 12;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchSenderCheck
            // 
            this.SearchSenderCheck.AutoSize = true;
            this.SearchSenderCheck.Location = new System.Drawing.Point(407, 4);
            this.SearchSenderCheck.Margin = new System.Windows.Forms.Padding(1);
            this.SearchSenderCheck.Name = "SearchSenderCheck";
            this.SearchSenderCheck.Size = new System.Drawing.Size(62, 19);
            this.SearchSenderCheck.TabIndex = 13;
            this.SearchSenderCheck.Text = "Sender";
            this.SearchSenderCheck.UseVisualStyleBackColor = true;
            // 
            // SearchSubjectCheck
            // 
            this.SearchSubjectCheck.AutoSize = true;
            this.SearchSubjectCheck.Location = new System.Drawing.Point(407, 21);
            this.SearchSubjectCheck.Margin = new System.Windows.Forms.Padding(1);
            this.SearchSubjectCheck.Name = "SearchSubjectCheck";
            this.SearchSubjectCheck.Size = new System.Drawing.Size(65, 19);
            this.SearchSubjectCheck.TabIndex = 14;
            this.SearchSubjectCheck.Text = "Subject";
            this.SearchSubjectCheck.UseVisualStyleBackColor = true;
            // 
            // SearchContentCheck
            // 
            this.SearchContentCheck.AutoSize = true;
            this.SearchContentCheck.Location = new System.Drawing.Point(407, 36);
            this.SearchContentCheck.Margin = new System.Windows.Forms.Padding(1);
            this.SearchContentCheck.Name = "SearchContentCheck";
            this.SearchContentCheck.Size = new System.Drawing.Size(69, 19);
            this.SearchContentCheck.TabIndex = 15;
            this.SearchContentCheck.Text = "Content";
            this.SearchContentCheck.UseVisualStyleBackColor = true;
            // 
            // metrics_button
            // 
            this.metrics_button.Location = new System.Drawing.Point(151, 14);
            this.metrics_button.Name = "metrics_button";
            this.metrics_button.Size = new System.Drawing.Size(75, 23);
            this.metrics_button.TabIndex = 16;
            this.metrics_button.Text = "Metrics";
            this.metrics_button.UseVisualStyleBackColor = true;
            this.metrics_button.Click += new System.EventHandler(this.metrics_button_Click);
            // 
            // Inboxes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(745, 541);
            this.Controls.Add(this.metrics_button);
            this.Controls.Add(this.SearchContentCheck);
            this.Controls.Add(this.SearchSubjectCheck);
            this.Controls.Add(this.SearchSenderCheck);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.Folders);
            this.Controls.Add(this.Compose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Inbox);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Inboxes";
            this.Text = "Prime Email";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ToolStrip toolStrip1;
        private ToolStripButton DeleteButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton ToggleFlagButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton RefreshButton;
        private ListBox Inbox;
        private Label label2;
        private Button Compose;
        private ListBox Folders;
        private TextBox SearchTextBox;
        private Button SearchButton;
        private CheckBox SearchSenderCheck;
        private CheckBox SearchSubjectCheck;
        private CheckBox SearchContentCheck;
        private ToolStripButton MoveToTrashButton;
        private Button metrics_button;
    }
}