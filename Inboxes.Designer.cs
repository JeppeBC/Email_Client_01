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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inboxes));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.DeleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToggleFlagButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.RefreshButton = new System.Windows.Forms.ToolStripButton();
            this.MoveToTrashButton = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.Compose = new System.Windows.Forms.Button();
            this.Folders = new System.Windows.Forms.ListBox();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchSenderCheck = new System.Windows.Forms.CheckBox();
            this.SearchSubjectCheck = new System.Windows.Forms.CheckBox();
            this.SearchContentCheck = new System.Windows.Forms.CheckBox();
            this.PrioritySelecter = new System.Windows.Forms.ComboBox();
            this.InboxGrid = new System.Windows.Forms.DataGridView();
            this.Flags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.emailSenderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.PriorityGrid = new System.Windows.Forms.DataGridView();
            this.Remove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InboxGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emailBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emailSenderBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
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
            this.toolStrip1.Location = new System.Drawing.Point(215, 15);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip1.Size = new System.Drawing.Size(167, 33);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // DeleteButton
            // 
            this.DeleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(34, 28);
            this.DeleteButton.Text = "toolStripButton1";
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 33);
            // 
            // ToggleFlagButton
            // 
            this.ToggleFlagButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToggleFlagButton.Image = ((System.Drawing.Image)(resources.GetObject("ToggleFlagButton.Image")));
            this.ToggleFlagButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.ToggleFlagButton.Name = "ToggleFlagButton";
            this.ToggleFlagButton.Size = new System.Drawing.Size(34, 28);
            this.ToggleFlagButton.Text = "toolStripButton2";
            this.ToggleFlagButton.Click += new System.EventHandler(this.ToggleFlagButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // RefreshButton
            // 
            this.RefreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RefreshButton.Image = ((System.Drawing.Image)(resources.GetObject("RefreshButton.Image")));
            this.RefreshButton.ImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(34, 28);
            this.RefreshButton.Text = "RefreshButton";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshPage_Click);
            // 
            // MoveToTrashButton
            // 
            this.MoveToTrashButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveToTrashButton.Image = ((System.Drawing.Image)(resources.GetObject("MoveToTrashButton.Image")));
            this.MoveToTrashButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveToTrashButton.Name = "MoveToTrashButton";
            this.MoveToTrashButton.Size = new System.Drawing.Size(34, 28);
            this.MoveToTrashButton.Text = "Moves selected item to trash folder";
            this.MoveToTrashButton.Click += new System.EventHandler(this.MoveToTrashButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(10, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 48);
            this.label2.TabIndex = 8;
            this.label2.Text = "Prime Mail";
            // 
            // Compose
            // 
            this.Compose.AutoSize = true;
            this.Compose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Compose.Location = new System.Drawing.Point(10, 90);
            this.Compose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Compose.Name = "Compose";
            this.Compose.Size = new System.Drawing.Size(277, 55);
            this.Compose.TabIndex = 9;
            this.Compose.Text = "Compose";
            this.Compose.UseVisualStyleBackColor = false;
            this.Compose.Click += new System.EventHandler(this.Compose_Click);
            // 
            // Folders
            // 
            this.Folders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Folders.FormattingEnabled = true;
            this.Folders.ItemHeight = 25;
            this.Folders.Location = new System.Drawing.Point(10, 155);
            this.Folders.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Folders.Name = "Folders";
            this.Folders.Size = new System.Drawing.Size(277, 254);
            this.Folders.TabIndex = 10;
            this.Folders.SelectedIndexChanged += new System.EventHandler(this.Folders_SelectedIndexChanged);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.SearchTextBox.Location = new System.Drawing.Point(758, 36);
            this.SearchTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(266, 31);
            this.SearchTextBox.TabIndex = 11;
            this.SearchTextBox.Text = "Search in the current folder...";
            this.SearchTextBox.Click += new System.EventHandler(this.SearchTextBox_Click);
            this.SearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.SearchButton.Location = new System.Drawing.Point(1041, 20);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(2);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(82, 45);
            this.SearchButton.TabIndex = 12;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = false;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchSenderCheck
            // 
            this.SearchSenderCheck.AutoSize = true;
            this.SearchSenderCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.SearchSenderCheck.Location = new System.Drawing.Point(582, 7);
            this.SearchSenderCheck.Margin = new System.Windows.Forms.Padding(2);
            this.SearchSenderCheck.Name = "SearchSenderCheck";
            this.SearchSenderCheck.Size = new System.Drawing.Size(93, 29);
            this.SearchSenderCheck.TabIndex = 13;
            this.SearchSenderCheck.Text = "Sender";
            this.SearchSenderCheck.UseVisualStyleBackColor = false;
            // 
            // SearchSubjectCheck
            // 
            this.SearchSubjectCheck.AutoSize = true;
            this.SearchSubjectCheck.Location = new System.Drawing.Point(582, 34);
            this.SearchSubjectCheck.Margin = new System.Windows.Forms.Padding(2);
            this.SearchSubjectCheck.Name = "SearchSubjectCheck";
            this.SearchSubjectCheck.Size = new System.Drawing.Size(96, 29);
            this.SearchSubjectCheck.TabIndex = 14;
            this.SearchSubjectCheck.Text = "Subject";
            this.SearchSubjectCheck.UseVisualStyleBackColor = true;
            // 
            // SearchContentCheck
            // 
            this.SearchContentCheck.AutoSize = true;
            this.SearchContentCheck.Location = new System.Drawing.Point(582, 59);
            this.SearchContentCheck.Margin = new System.Windows.Forms.Padding(2);
            this.SearchContentCheck.Name = "SearchContentCheck";
            this.SearchContentCheck.Size = new System.Drawing.Size(101, 29);
            this.SearchContentCheck.TabIndex = 15;
            this.SearchContentCheck.Text = "Content";
            this.SearchContentCheck.UseVisualStyleBackColor = true;
            // 
            // PrioritySelecter
            // 
            this.PrioritySelecter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.PrioritySelecter.FormattingEnabled = true;
            this.PrioritySelecter.Items.AddRange(new object[] {
            "1st",
            "2nd",
            "3rd",
            "4th",
            "5th"});
            this.PrioritySelecter.Location = new System.Drawing.Point(10, 417);
            this.PrioritySelecter.Name = "PrioritySelecter";
            this.PrioritySelecter.Size = new System.Drawing.Size(278, 33);
            this.PrioritySelecter.TabIndex = 17;
            this.PrioritySelecter.Text = "Priorities";
            this.PrioritySelecter.SelectedIndexChanged += new System.EventHandler(this.PriorityClicked);
            // 
            // InboxGrid
            // 
            this.InboxGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.InboxGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InboxGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Flags,
            this.Sender,
            this.Subject,
            this.Date});
            this.InboxGrid.Location = new System.Drawing.Point(294, 90);
            this.InboxGrid.Name = "InboxGrid";
            this.InboxGrid.RowHeadersVisible = false;
            this.InboxGrid.RowHeadersWidth = 62;
            this.InboxGrid.RowTemplate.Height = 33;
            this.InboxGrid.Size = new System.Drawing.Size(847, 568);
            this.InboxGrid.TabIndex = 18;
            this.InboxGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.InboxGrid_Click);
            this.InboxGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.InboxGrid_DoubleClick);
            // 
            // Flags
            // 
            this.Flags.FillWeight = 10F;
            this.Flags.HeaderText = "Flags";
            this.Flags.MinimumWidth = 8;
            this.Flags.Name = "Flags";
            this.Flags.Width = 89;
            // 
            // Sender
            // 
            this.Sender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Sender.FillWeight = 31.57895F;
            this.Sender.HeaderText = "Sender";
            this.Sender.MinimumWidth = 8;
            this.Sender.Name = "Sender";
            // 
            // Subject
            // 
            this.Subject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Subject.FillWeight = 31.57895F;
            this.Subject.HeaderText = "Subject";
            this.Subject.MinimumWidth = 8;
            this.Subject.Name = "Subject";
            // 
            // Date
            // 
            this.Date.FillWeight = 10F;
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 8;
            this.Date.Name = "Date";
            this.Date.Width = 150;
            // 
            // emailBindingSource
            // 
            this.emailBindingSource.DataSource = typeof(Email_Client_01.Email);
            // 
            // emailSenderBindingSource
            // 
            this.emailSenderBindingSource.DataSource = typeof(Email_Client_01.EmailSender);
            // 
            // PriorityGrid
            // 
            this.PriorityGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.PriorityGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PriorityGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Remove,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.PriorityGrid.Location = new System.Drawing.Point(10, 456);
            this.PriorityGrid.Name = "PriorityGrid";
            this.PriorityGrid.RowHeadersVisible = false;
            this.PriorityGrid.RowHeadersWidth = 62;
            this.PriorityGrid.RowTemplate.Height = 33;
            this.PriorityGrid.Size = new System.Drawing.Size(278, 202);
            this.PriorityGrid.TabIndex = 19;
            this.PriorityGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PriorityGrid_Click);
            this.PriorityGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PriorityGrid_DoubleClick);
            // 
            // Remove
            // 
            this.Remove.HeaderText = "";
            this.Remove.MinimumWidth = 8;
            this.Remove.Name = "Remove";
            this.Remove.Width = 30;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Priority";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 70;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Subject";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // Inboxes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(1153, 670);
            this.Controls.Add(this.PriorityGrid);
            this.Controls.Add(this.InboxGrid);
            this.Controls.Add(this.PrioritySelecter);
            this.Controls.Add(this.SearchContentCheck);
            this.Controls.Add(this.SearchSubjectCheck);
            this.Controls.Add(this.SearchSenderCheck);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.Folders);
            this.Controls.Add(this.Compose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Inboxes";
            this.Text = "Prime Email";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InboxGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emailBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emailSenderBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityGrid)).EndInit();
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
        private Label label2;
        private Button Compose;
        private ListBox Folders;
        private TextBox SearchTextBox;
        private Button SearchButton;
        private CheckBox SearchSenderCheck;
        private CheckBox SearchSubjectCheck;
        private CheckBox SearchContentCheck;
        private ToolStripButton MoveToTrashButton;
        private ComboBox PrioritySelecter;
        private DataGridView InboxGrid;
        private BindingSource emailBindingSource;
        private BindingSource emailSenderBindingSource;
        private DataGridView PriorityGrid;
        private DataGridViewTextBoxColumn Flags;
        private DataGridViewTextBoxColumn Sender;
        private DataGridViewTextBoxColumn Subject;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewButtonColumn Remove;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}