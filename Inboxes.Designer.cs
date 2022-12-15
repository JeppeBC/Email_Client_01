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
            this.label2 = new System.Windows.Forms.Label();
            this.Compose = new System.Windows.Forms.Button();
            this.Folders = new System.Windows.Forms.ListBox();
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
            this.FilterUnreadCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Inbox
            // 
            this.Inbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Inbox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Inbox.FormattingEnabled = true;
            this.Inbox.ItemHeight = 48;
            this.Inbox.Location = new System.Drawing.Point(411, 178);
            this.Inbox.Margin = new System.Windows.Forms.Padding(6);
            this.Inbox.Name = "Inbox";
            this.Inbox.Size = new System.Drawing.Size(1835, 1060);
            this.Inbox.TabIndex = 6;
            this.Inbox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Inbox_DrawItem);
            this.Inbox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Inbox_MouseDoubleClick);
            this.Inbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Inbox_MouseDown);
            this.Inbox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Inbox_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(20, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(9, 0, 9, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(406, 96);
            this.label2.TabIndex = 8;
            this.label2.Text = "Prime Mail";
            // 
            // Compose
            // 
            this.Compose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Compose.Location = new System.Drawing.Point(33, 178);
            this.Compose.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.Compose.Name = "Compose";
            this.Compose.Size = new System.Drawing.Size(363, 106);
            this.Compose.TabIndex = 9;
            this.Compose.Text = "Compose";
            this.Compose.UseVisualStyleBackColor = false;
            this.Compose.Click += new System.EventHandler(this.Compose_Click);
            // 
            // Folders
            // 
            this.Folders.AllowDrop = true;
            this.Folders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(125)))), ((int)(((byte)(125)))));
            this.Folders.FormattingEnabled = true;
            this.Folders.ItemHeight = 48;
            this.Folders.Location = new System.Drawing.Point(33, 320);
            this.Folders.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.Folders.Name = "Folders";
            this.Folders.Size = new System.Drawing.Size(355, 916);
            this.Folders.TabIndex = 10;
            this.Folders.SelectedIndexChanged += new System.EventHandler(this.Folders_SelectedIndexChanged);
            this.Folders.DragDrop += new System.Windows.Forms.DragEventHandler(this.Folders_DragDrop);
            this.Folders.DragEnter += new System.Windows.Forms.DragEventHandler(this.Folders_DragEnter);
            this.Folders.DragOver += new System.Windows.Forms.DragEventHandler(this.Folders_DragOver);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(1517, 70);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(528, 55);
            this.SearchTextBox.TabIndex = 11;
            this.SearchTextBox.Text = "Search in the current folder...";
            this.SearchTextBox.Click += new System.EventHandler(this.SearchTextBox_Click);
            this.SearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(2082, 39);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(164, 87);
            this.SearchButton.TabIndex = 12;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchSenderCheck
            // 
            this.SearchSenderCheck.AutoSize = true;
            this.SearchSenderCheck.Location = new System.Drawing.Point(1322, 14);
            this.SearchSenderCheck.Name = "SearchSenderCheck";
            this.SearchSenderCheck.Size = new System.Drawing.Size(177, 52);
            this.SearchSenderCheck.TabIndex = 13;
            this.SearchSenderCheck.Text = "Sender";
            this.SearchSenderCheck.UseVisualStyleBackColor = true;
            // 
            // SearchSubjectCheck
            // 
            this.SearchSubjectCheck.AutoSize = true;
            this.SearchSubjectCheck.Location = new System.Drawing.Point(1322, 66);
            this.SearchSubjectCheck.Name = "SearchSubjectCheck";
            this.SearchSubjectCheck.Size = new System.Drawing.Size(183, 52);
            this.SearchSubjectCheck.TabIndex = 14;
            this.SearchSubjectCheck.Text = "Subject";
            this.SearchSubjectCheck.UseVisualStyleBackColor = true;
            // 
            // SearchContentCheck
            // 
            this.SearchContentCheck.AutoSize = true;
            this.SearchContentCheck.Location = new System.Drawing.Point(1322, 116);
            this.SearchContentCheck.Name = "SearchContentCheck";
            this.SearchContentCheck.Size = new System.Drawing.Size(192, 52);
            this.SearchContentCheck.TabIndex = 15;
            this.SearchContentCheck.Text = "Content";
            this.SearchContentCheck.UseVisualStyleBackColor = true;
            // 
            // FilterCheckbox
            // 
            this.FilterCheckbox.AutoSize = true;
            this.FilterCheckbox.Location = new System.Drawing.Point(1517, 12);
            this.FilterCheckbox.Name = "FilterCheckbox";
            this.FilterCheckbox.Size = new System.Drawing.Size(279, 52);
            this.FilterCheckbox.TabIndex = 16;
            this.FilterCheckbox.Text = "Add to Filters";
            this.FilterCheckbox.UseVisualStyleBackColor = true;
            this.FilterCheckbox.CheckedChanged += new System.EventHandler(this.FilterCheckbox_CheckedChanged);
            // 
            // FilterListbox
            // 
            this.FilterListbox.FormattingEnabled = true;
            this.FilterListbox.ItemHeight = 48;
            this.FilterListbox.Location = new System.Drawing.Point(862, 16);
            this.FilterListbox.Name = "FilterListbox";
            this.FilterListbox.Size = new System.Drawing.Size(428, 148);
            this.FilterListbox.TabIndex = 17;
            this.FilterListbox.Visible = false;
            // 
            // FilterLabel
            // 
            this.FilterLabel.AutoSize = true;
            this.FilterLabel.Location = new System.Drawing.Point(733, 9);
            this.FilterLabel.Name = "FilterLabel";
            this.FilterLabel.Size = new System.Drawing.Size(123, 48);
            this.FilterLabel.TabIndex = 18;
            this.FilterLabel.Text = "Filters:";
            this.FilterLabel.Visible = false;
            // 
            // RemoveFilterButton
            // 
            this.RemoveFilterButton.Location = new System.Drawing.Point(696, 53);
            this.RemoveFilterButton.Name = "RemoveFilterButton";
            this.RemoveFilterButton.Size = new System.Drawing.Size(160, 57);
            this.RemoveFilterButton.TabIndex = 19;
            this.RemoveFilterButton.Text = "Remove";
            this.RemoveFilterButton.UseVisualStyleBackColor = true;
            this.RemoveFilterButton.Visible = false;
            this.RemoveFilterButton.Click += new System.EventHandler(this.RemoveFilterButton_Click);
            // 
            // ShowFiltersCheckbox
            // 
            this.ShowFiltersCheckbox.AutoSize = true;
            this.ShowFiltersCheckbox.Location = new System.Drawing.Point(1806, 12);
            this.ShowFiltersCheckbox.Name = "ShowFiltersCheckbox";
            this.ShowFiltersCheckbox.Size = new System.Drawing.Size(257, 52);
            this.ShowFiltersCheckbox.TabIndex = 20;
            this.ShowFiltersCheckbox.Text = "Show Filters";
            this.ShowFiltersCheckbox.UseVisualStyleBackColor = true;
            this.ShowFiltersCheckbox.CheckStateChanged += new System.EventHandler(this.ShowFiltersCheckbox_CheckStateChanged);
            // 
            // RefreshFoldersButton
            // 
            this.RefreshFoldersButton.Location = new System.Drawing.Point(451, 16);
            this.RefreshFoldersButton.Name = "RefreshFoldersButton";
            this.RefreshFoldersButton.Size = new System.Drawing.Size(225, 69);
            this.RefreshFoldersButton.TabIndex = 21;
            this.RefreshFoldersButton.Text = "Refresh";
            this.RefreshFoldersButton.UseVisualStyleBackColor = true;
            this.RefreshFoldersButton.Click += new System.EventHandler(this.RefreshFoldersButton_Click);
            // 
            // CreateFolderButton
            // 
            this.CreateFolderButton.Location = new System.Drawing.Point(264, 107);
            this.CreateFolderButton.Name = "CreateFolderButton";
            this.CreateFolderButton.Size = new System.Drawing.Size(225, 69);
            this.CreateFolderButton.TabIndex = 22;
            this.CreateFolderButton.Text = "Create Folder";
            this.CreateFolderButton.UseVisualStyleBackColor = true;
            this.CreateFolderButton.Click += new System.EventHandler(this.CreateFolderButton_Click);
            // 
            // DeleteFolderButton
            // 
            this.DeleteFolderButton.Location = new System.Drawing.Point(33, 100);
            this.DeleteFolderButton.Name = "DeleteFolderButton";
            this.DeleteFolderButton.Size = new System.Drawing.Size(225, 69);
            this.DeleteFolderButton.TabIndex = 23;
            this.DeleteFolderButton.Text = "Delete Folder";
            this.DeleteFolderButton.UseVisualStyleBackColor = true;
            this.DeleteFolderButton.Click += new System.EventHandler(this.DeleteFolderButton_Click);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(20F, 48F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(2286, 1274);
            this.Controls.Add(this.FilterUnreadCheckbox);
            this.Controls.Add(this.DeleteFolderButton);
            this.Controls.Add(this.CreateFolderButton);
            this.Controls.Add(this.RefreshFoldersButton);
            this.Controls.Add(this.ShowFiltersCheckbox);
            this.Controls.Add(this.RemoveFilterButton);
            this.Controls.Add(this.FilterLabel);
            this.Controls.Add(this.FilterListbox);
            this.Controls.Add(this.FilterCheckbox);
            this.Controls.Add(this.SearchContentCheck);
            this.Controls.Add(this.SearchSubjectCheck);
            this.Controls.Add(this.SearchSenderCheck);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.Folders);
            this.Controls.Add(this.Compose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Inbox);
            this.Margin = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this.Name = "Inboxes";
            this.Text = "Prime Email";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ListBox Inbox;
        private Label label2;
        private Button Compose;
        private ListBox Folders;
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
        private CheckBox FilterUnreadCheckbox;
    }
}