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
        /// 

        private void InitializeComponent()
        {
            this.Inbox = new System.Windows.Forms.ListBox();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchSenderCheck = new System.Windows.Forms.CheckBox();
            this.SearchSubjectCheck = new System.Windows.Forms.CheckBox();
            this.SearchContentCheck = new System.Windows.Forms.CheckBox();
            this.FilterCheckbox = new System.Windows.Forms.CheckBox();
            this.FilterListbox = new System.Windows.Forms.ListBox();
            this.FilterLabel = new System.Windows.Forms.Label();
            this.RemoveFilterButton = new System.Windows.Forms.Button();
            this.ShowFiltersCheckbox = new System.Windows.Forms.CheckBox();
            this.RefreshFoldersButton = new System.Windows.Forms.Button();
            this.CreateFolderButton = new System.Windows.Forms.Button();
            this.DeleteFolderButton = new System.Windows.Forms.Button();
            this.InboxGrid = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.Compose = new System.Windows.Forms.Button();
            this.PrioritySelecter = new System.Windows.Forms.ComboBox();
            this.PriorityGrid = new System.Windows.Forms.DataGridView();
            this.remove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Folders = new System.Windows.Forms.ListBox();
            this.Flags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.InboxGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityGrid)).BeginInit();
            this.FilterUnreadCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Inbox
            // 
            this.Inbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Inbox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Inbox.FormattingEnabled = true;
            this.Inbox.ItemHeight = 48;
            this.Inbox.Location = new System.Drawing.Point(228, 132);
            this.Inbox.Margin = new System.Windows.Forms.Padding(2);
            this.Inbox.Name = "Inbox";
            this.Inbox.Size = new System.Drawing.Size(796, 436);
            this.Inbox.TabIndex = 6;
            this.Inbox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Inbox_DrawItem);
            this.Inbox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Inbox_MouseDoubleClick);
            this.Inbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Inbox_MouseDown);
            this.Inbox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Inbox_MouseUp);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(746, 40);
            this.SearchTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(214, 27);
            this.SearchTextBox.TabIndex = 11;
            this.SearchTextBox.Text = "Search in the current folder...";
            this.SearchTextBox.Click += new System.EventHandler(this.SearchTextBox_Click);
            this.SearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(975, 31);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(1);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(66, 36);
            this.SearchButton.TabIndex = 12;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchSenderCheck
            // 
            this.SearchSenderCheck.AutoSize = true;
            this.SearchSenderCheck.Location = new System.Drawing.Point(625, 10);
            this.SearchSenderCheck.Margin = new System.Windows.Forms.Padding(1);
            this.SearchSenderCheck.Name = "SearchSenderCheck";
            this.SearchSenderCheck.Size = new System.Drawing.Size(77, 24);
            this.SearchSenderCheck.TabIndex = 13;
            this.SearchSenderCheck.Text = "Sender";
            this.SearchSenderCheck.UseVisualStyleBackColor = true;
            // 
            // SearchSubjectCheck
            // 
            this.SearchSubjectCheck.AutoSize = true;
            this.SearchSubjectCheck.Location = new System.Drawing.Point(625, 36);
            this.SearchSubjectCheck.Margin = new System.Windows.Forms.Padding(1);
            this.SearchSubjectCheck.Name = "SearchSubjectCheck";
            this.SearchSubjectCheck.Size = new System.Drawing.Size(80, 24);
            this.SearchSubjectCheck.TabIndex = 14;
            this.SearchSubjectCheck.Text = "Subject";
            this.SearchSubjectCheck.UseVisualStyleBackColor = true;
            // 
            // SearchContentCheck
            // 
            this.SearchContentCheck.AutoSize = true;
            this.SearchContentCheck.Location = new System.Drawing.Point(625, 62);
            this.SearchContentCheck.Margin = new System.Windows.Forms.Padding(1);
            this.SearchContentCheck.Name = "SearchContentCheck";
            this.SearchContentCheck.Size = new System.Drawing.Size(83, 24);
            this.SearchContentCheck.TabIndex = 15;
            this.SearchContentCheck.Text = "Content";
            this.SearchContentCheck.UseVisualStyleBackColor = true;
            // 
            // FilterCheckbox
            // 
            this.FilterCheckbox.AutoSize = true;
            this.FilterCheckbox.Location = new System.Drawing.Point(739, 7);
            this.FilterCheckbox.Margin = new System.Windows.Forms.Padding(1);
            this.FilterCheckbox.Name = "FilterCheckbox";
            this.FilterCheckbox.Size = new System.Drawing.Size(120, 24);
            this.FilterCheckbox.TabIndex = 16;
            this.FilterCheckbox.Text = "Add to Filters";
            this.FilterCheckbox.UseVisualStyleBackColor = true;
            this.FilterCheckbox.CheckedChanged += new System.EventHandler(this.FilterCheckbox_CheckedChanged);
            // 
            // FilterListbox
            // 
            this.FilterListbox.FormattingEnabled = true;
            this.FilterListbox.ItemHeight = 20;
            this.FilterListbox.Location = new System.Drawing.Point(431, 10);
            this.FilterListbox.Margin = new System.Windows.Forms.Padding(1);
            this.FilterListbox.Name = "FilterListbox";
            this.FilterListbox.Size = new System.Drawing.Size(174, 64);
            this.FilterListbox.TabIndex = 17;
            this.FilterListbox.Visible = false;
            // 
            // FilterLabel
            // 
            this.FilterLabel.AutoSize = true;
            this.FilterLabel.Location = new System.Drawing.Point(353, 6);
            this.FilterLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.FilterLabel.Name = "FilterLabel";
            this.FilterLabel.Size = new System.Drawing.Size(51, 20);
            this.FilterLabel.TabIndex = 18;
            this.FilterLabel.Text = "Filters:";
            this.FilterLabel.Visible = false;
            // 
            // RemoveFilterButton
            // 
            this.RemoveFilterButton.Location = new System.Drawing.Point(353, 31);
            this.RemoveFilterButton.Margin = new System.Windows.Forms.Padding(1);
            this.RemoveFilterButton.Name = "RemoveFilterButton";
            this.RemoveFilterButton.Size = new System.Drawing.Size(76, 29);
            this.RemoveFilterButton.TabIndex = 19;
            this.RemoveFilterButton.Text = "Remove";
            this.RemoveFilterButton.UseVisualStyleBackColor = true;
            this.RemoveFilterButton.Visible = false;
            this.RemoveFilterButton.Click += new System.EventHandler(this.RemoveFilterButton_Click);
            // 
            // ShowFiltersCheckbox
            // 
            this.ShowFiltersCheckbox.AutoSize = true;
            this.ShowFiltersCheckbox.Location = new System.Drawing.Point(861, 6);
            this.ShowFiltersCheckbox.Margin = new System.Windows.Forms.Padding(1);
            this.ShowFiltersCheckbox.Name = "ShowFiltersCheckbox";
            this.ShowFiltersCheckbox.Size = new System.Drawing.Size(110, 24);
            this.ShowFiltersCheckbox.TabIndex = 20;
            this.ShowFiltersCheckbox.Text = "Show Filters";
            this.ShowFiltersCheckbox.UseVisualStyleBackColor = true;
            this.ShowFiltersCheckbox.CheckStateChanged += new System.EventHandler(this.ShowFiltersCheckbox_CheckStateChanged);
            // 
            // RefreshFoldersButton
            // 
            this.RefreshFoldersButton.Location = new System.Drawing.Point(228, 7);
            this.RefreshFoldersButton.Margin = new System.Windows.Forms.Padding(1);
            this.RefreshFoldersButton.Name = "RefreshFoldersButton";
            this.RefreshFoldersButton.Size = new System.Drawing.Size(90, 29);
            this.RefreshFoldersButton.TabIndex = 21;
            this.RefreshFoldersButton.Text = "Refresh";
            this.RefreshFoldersButton.UseVisualStyleBackColor = true;
            this.RefreshFoldersButton.Click += new System.EventHandler(this.RefreshFoldersButton_Click);
            // 
            // CreateFolderButton
            // 
            this.CreateFolderButton.Location = new System.Drawing.Point(228, 40);
            this.CreateFolderButton.Margin = new System.Windows.Forms.Padding(1);
            this.CreateFolderButton.Name = "CreateFolderButton";
            this.CreateFolderButton.Size = new System.Drawing.Size(90, 29);
            this.CreateFolderButton.TabIndex = 22;
            this.CreateFolderButton.Text = "Create Folder";
            this.CreateFolderButton.UseVisualStyleBackColor = true;
            this.CreateFolderButton.Click += new System.EventHandler(this.CreateFolderButton_Click);
            // 
            // DeleteFolderButton
            // 
            this.DeleteFolderButton.Location = new System.Drawing.Point(228, 71);
            this.DeleteFolderButton.Margin = new System.Windows.Forms.Padding(1);
            this.DeleteFolderButton.Name = "DeleteFolderButton";
            this.DeleteFolderButton.Size = new System.Drawing.Size(90, 29);
            this.DeleteFolderButton.TabIndex = 23;
            this.DeleteFolderButton.Text = "Delete Folder";
            this.DeleteFolderButton.UseVisualStyleBackColor = true;
            this.DeleteFolderButton.Click += new System.EventHandler(this.DeleteFolderButton_Click);
            // 
            // InboxGrid
            // 
            this.InboxGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.InboxGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InboxGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Flags,
            this.Sender,
            this.Subject,
            this.Date});
            this.InboxGrid.Location = new System.Drawing.Point(416, 154);
            this.InboxGrid.Name = "InboxGrid";
            this.InboxGrid.RowHeadersVisible = false;
            this.InboxGrid.RowHeadersWidth = 51;
            this.InboxGrid.RowTemplate.Height = 29;
            this.InboxGrid.Size = new System.Drawing.Size(569, 371);
            this.InboxGrid.TabIndex = 24;
            this.InboxGrid.Click += new System.EventHandler(this.InboxGrid_Click);
            this.InboxGrid.DoubleClick += new System.EventHandler(this.InboxGrid_DoubleClick);
            this.InboxGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.InboxGrid_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(13, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 41);
            this.label2.TabIndex = 8;
            this.label2.Text = "Prime Mail";
            // 
            // Compose
            // 
            this.Compose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Compose.Location = new System.Drawing.Point(13, 62);
            this.Compose.Margin = new System.Windows.Forms.Padding(4);
            this.Compose.Name = "Compose";
            this.Compose.Size = new System.Drawing.Size(199, 56);
            this.Compose.TabIndex = 9;
            this.Compose.Text = "Compose";
            this.Compose.UseVisualStyleBackColor = false;
            this.Compose.Click += new System.EventHandler(this.Compose_Click);
            // 
            // PrioritySelecter
            // 
            this.PrioritySelecter.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.PrioritySelecter.FormattingEnabled = true;
            this.PrioritySelecter.Items.AddRange(new object[] {
            "1st",
            "2nd",
            "3rd",
            "4th",
            "5th"});
            this.PrioritySelecter.Location = new System.Drawing.Point(12, 356);
            this.PrioritySelecter.Name = "PrioritySelecter";
            this.PrioritySelecter.Size = new System.Drawing.Size(199, 28);
            this.PrioritySelecter.TabIndex = 0;
            this.PrioritySelecter.Text = "Priorities";
            this.PrioritySelecter.SelectedIndexChanged += new System.EventHandler(this.Priority_Clicked);
            this.PrioritySelecter.Click += new System.EventHandler(this.PriorityClicked);
            // 
            // PriorityGrid
            // 
            this.PriorityGrid.BackgroundColor = System.Drawing.SystemColors.AppWorkspace;
            this.PriorityGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PriorityGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.remove,
            this.Priority,
            this.dataGridViewTextBoxColumn1});
            this.PriorityGrid.Location = new System.Drawing.Point(11, 390);
            this.PriorityGrid.Name = "PriorityGrid";
            this.PriorityGrid.RowHeadersVisible = false;
            this.PriorityGrid.RowHeadersWidth = 51;
            this.PriorityGrid.RowTemplate.Height = 29;
            this.PriorityGrid.Size = new System.Drawing.Size(201, 188);
            this.PriorityGrid.TabIndex = 10;
            this.PriorityGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PriorityGrid_Click);
            this.PriorityGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PriorityGrid_DoubleClick);
            // 
            // remove
            // 
            this.remove.HeaderText = "";
            this.remove.MinimumWidth = 6;
            this.remove.Name = "remove";
            this.remove.Width = 30;
            // 
            // Priority
            // 
            this.Priority.HeaderText = "Priority";
            this.Priority.MinimumWidth = 6;
            this.Priority.Name = "Priority";
            this.Priority.Width = 60;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Subject";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 125;
            // 
            // Folders
            // 
            this.Folders.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Folders.FormattingEnabled = true;
            this.Folders.ItemHeight = 20;
            this.Folders.Location = new System.Drawing.Point(13, 125);
            this.Folders.Name = "Folders";
            this.Folders.Size = new System.Drawing.Size(199, 204);
            this.Folders.TabIndex = 25;
            // 
            // Flags
            // 
            this.Flags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Flags.FillWeight = 7.699075F;
            this.Flags.HeaderText = "Flags";
            this.Flags.MinimumWidth = 6;
            this.Flags.Name = "Flags";
            this.Flags.Width = 75;
            // 
            // Sender
            // 
            this.Sender.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Sender.FillWeight = 190F;
            this.Sender.HeaderText = "Sender";
            this.Sender.MinimumWidth = 6;
            this.Sender.Name = "Sender";
            // 
            // Subject
            // 
            this.Subject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Subject.FillWeight = 190F;
            this.Subject.HeaderText = "Subject";
            this.Subject.MinimumWidth = 6;
            this.Subject.Name = "Subject";
            // 
            // Date
            // 
            this.Date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Date.FillWeight = 5.59213F;
            this.Date.HeaderText = "Date";
            this.Date.MinimumWidth = 6;
            this.Date.Name = "Date";
            this.Date.Width = 125;
            // 
            // FilterUnreadCheckbox
            // 
            this.FilterUnreadCheckbox.AutoSize = true;
            this.FilterUnreadCheckbox.Location = new System.Drawing.Point(517, 109);
            this.FilterUnreadCheckbox.Name = "FilterUnreadCheckbox";
            this.FilterUnreadCheckbox.Size = new System.Drawing.Size(278, 52);
            this.FilterUnreadCheckbox.TabIndex = 24;
            this.FilterUnreadCheckbox.Text = "Show Unread";
            this.FilterUnreadCheckbox.UseVisualStyleBackColor = true;
            this.FilterUnreadCheckbox.CheckedChanged += new System.EventHandler(this.FilterUnreadCheckbox_CheckedChanged);
            // 
            // Inboxes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(1051, 599);
            this.Controls.Add(this.Folders);
            this.Controls.Add(this.PriorityGrid);
            this.Controls.Add(this.InboxGrid);
            this.Controls.Add(this.PrioritySelecter);
            this.Controls.Add(this.FilterLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Compose);
            this.Controls.Add(this.RemoveFilterButton);
            this.Controls.Add(this.FilterListbox);
            this.Controls.Add(this.DeleteFolderButton);
            this.Controls.Add(this.CreateFolderButton);
            this.Controls.Add(this.RefreshFoldersButton);
            this.Controls.Add(this.ShowFiltersCheckbox);
            this.Controls.Add(this.FilterCheckbox);
            this.Controls.Add(this.SearchContentCheck);
            this.Controls.Add(this.SearchSubjectCheck);
            this.Controls.Add(this.SearchSenderCheck);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.Inbox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Inboxes";
            this.Text = "Prime Email";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.InboxGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PriorityGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ListBox Inbox;
        private TextBox SearchTextBox;
        private Button SearchButton;
        private CheckBox SearchSenderCheck;
        private CheckBox SearchSubjectCheck;
        private CheckBox SearchContentCheck;
        private CheckBox FilterCheckbox;
        private ListBox FilterListbox;
        private Label FilterLabel;
        private Button RemoveFilterButton;
        private CheckBox ShowFiltersCheckbox;
        private Button RefreshFoldersButton;
        private Button CreateFolderButton;
        private Button DeleteFolderButton;
        private DataGridView InboxGrid;
        private Label label2;
        private Button Compose;
        private ComboBox PrioritySelecter;
        private DataGridView PriorityGrid;
        private DataGridViewButtonColumn remove;
        private DataGridViewTextBoxColumn Priority;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private ListBox Folders;
        private DataGridViewTextBoxColumn Flags;
        private DataGridViewTextBoxColumn Sender;
        private DataGridViewTextBoxColumn Subject;
        private DataGridViewTextBoxColumn Date;
        private CheckBox FilterUnreadCheckbox;
    }
}