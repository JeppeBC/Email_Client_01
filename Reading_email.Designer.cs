namespace Email_Client_01
{
    partial class Reading_email
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reading_email));
            this.FromTextBox = new System.Windows.Forms.TextBox();
            this.SubjectTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MessageTextBox = new System.Windows.Forms.RichTextBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ReplyButton = new System.Windows.Forms.Button();
            this.ReplyAllButton = new System.Windows.Forms.Button();
            this.ForwardButton = new System.Windows.Forms.Button();
            this.DateTextBox = new System.Windows.Forms.TextBox();
            this.DateLabel = new System.Windows.Forms.Label();
            this.CCTextBox = new System.Windows.Forms.TextBox();
            this.ToTextBox = new System.Windows.Forms.TextBox();
            this.CCLabel = new System.Windows.Forms.Label();
            this.ToLabel = new System.Windows.Forms.Label();
            this.AttachmentListBox = new System.Windows.Forms.ListBox();
            this.AttachmentsLabel = new System.Windows.Forms.Label();
            this.DownloadAttachmentButton = new System.Windows.Forms.Button();
            this.DownloadAllAttachmentsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FromTextBox
            // 
            this.FromTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.FromTextBox.Location = new System.Drawing.Point(18, 18);
            this.FromTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromTextBox.Name = "FromTextBox";
            this.FromTextBox.Size = new System.Drawing.Size(592, 23);
            this.FromTextBox.TabIndex = 0;
            // 
            // SubjectTextBox
            // 
            this.SubjectTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.SubjectTextBox.Location = new System.Drawing.Point(18, 86);
            this.SubjectTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.SubjectTextBox.Name = "SubjectTextBox";
            this.SubjectTextBox.Size = new System.Drawing.Size(592, 23);
            this.SubjectTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Subject:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 104);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Message:";
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.MessageTextBox.Location = new System.Drawing.Point(18, 120);
            this.MessageTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(592, 229);
            this.MessageTextBox.TabIndex = 5;
            this.MessageTextBox.Text = "";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(2094, 1139);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(6);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(243, 112);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ReplyButton
            // 
            this.ReplyButton.Location = new System.Drawing.Point(31, 1139);
            this.ReplyButton.Margin = new System.Windows.Forms.Padding(6);
            this.ReplyButton.Name = "ReplyButton";
            this.ReplyButton.Size = new System.Drawing.Size(243, 112);
            this.ReplyButton.TabIndex = 7;
            this.ReplyButton.Text = "Reply";
            this.ReplyButton.UseVisualStyleBackColor = true;
            this.ReplyButton.Click += new System.EventHandler(this.Reply_Click);
            // 
            // ReplyAllButton
            // 
            this.ReplyAllButton.Location = new System.Drawing.Point(286, 1139);
            this.ReplyAllButton.Margin = new System.Windows.Forms.Padding(6);
            this.ReplyAllButton.Name = "ReplyAllButton";
            this.ReplyAllButton.Size = new System.Drawing.Size(243, 112);
            this.ReplyAllButton.TabIndex = 8;
            this.ReplyAllButton.Text = "Reply all";
            this.ReplyAllButton.UseVisualStyleBackColor = true;
            this.ReplyAllButton.Click += new System.EventHandler(this.ReplyAll_Click);
            // 
            // ForwardButton
            // 
            this.ForwardButton.Location = new System.Drawing.Point(189, 356);
            this.ForwardButton.Margin = new System.Windows.Forms.Padding(2);
            this.ForwardButton.Name = "ForwardButton";
            this.ForwardButton.Size = new System.Drawing.Size(85, 35);
            this.ForwardButton.TabIndex = 9;
            this.ForwardButton.Text = "Forward";
            this.ForwardButton.UseVisualStyleBackColor = true;
            this.ForwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            // 
            // DateTextBox
            // 
            this.DateTextBox.Location = new System.Drawing.Point(630, 18);
            this.DateTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.DateTextBox.Name = "DateTextBox";
            this.DateTextBox.Size = new System.Drawing.Size(179, 23);
            this.DateTextBox.TabIndex = 12;
            // 
            // DateLabel
            // 
            this.DateLabel.AutoSize = true;
            this.DateLabel.Location = new System.Drawing.Point(630, 2);
            this.DateLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(34, 15);
            this.DateLabel.TabIndex = 13;
            this.DateLabel.Text = "Date:";
            // 
            // CCTextBox
            // 
            this.CCTextBox.Location = new System.Drawing.Point(630, 51);
            this.CCTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.CCTextBox.Name = "CCTextBox";
            this.CCTextBox.Size = new System.Drawing.Size(180, 23);
            this.CCTextBox.TabIndex = 14;
            this.CCTextBox.Visible = false;
            // 
            // ToTextBox
            // 
            this.ToTextBox.Location = new System.Drawing.Point(18, 51);
            this.ToTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.ToTextBox.Name = "ToTextBox";
            this.ToTextBox.Size = new System.Drawing.Size(592, 23);
            this.ToTextBox.TabIndex = 15;
            // 
            // CCLabel
            // 
            this.CCLabel.AutoSize = true;
            this.CCLabel.Location = new System.Drawing.Point(630, 36);
            this.CCLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.CCLabel.Name = "CCLabel";
            this.CCLabel.Size = new System.Drawing.Size(31, 15);
            this.CCLabel.TabIndex = 16;
            this.CCLabel.Text = "CCs:";
            this.CCLabel.Visible = false;
            // 
            // ToLabel
            // 
            this.ToLabel.AutoSize = true;
            this.ToLabel.Location = new System.Drawing.Point(18, 34);
            this.ToLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(22, 15);
            this.ToLabel.TabIndex = 17;
            this.ToLabel.Text = "To:";
            // 
            // AttachmentListBox
            // 
            this.AttachmentListBox.FormattingEnabled = true;
            this.AttachmentListBox.ItemHeight = 15;
            this.AttachmentListBox.Location = new System.Drawing.Point(630, 120);
            this.AttachmentListBox.Margin = new System.Windows.Forms.Padding(1);
            this.AttachmentListBox.Name = "AttachmentListBox";
            this.AttachmentListBox.Size = new System.Drawing.Size(180, 169);
            this.AttachmentListBox.TabIndex = 18;
            this.AttachmentListBox.Visible = false;
            // 
            // AttachmentsLabel
            // 
            this.AttachmentsLabel.AutoSize = true;
            this.AttachmentsLabel.Location = new System.Drawing.Point(630, 104);
            this.AttachmentsLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.AttachmentsLabel.Name = "AttachmentsLabel";
            this.AttachmentsLabel.Size = new System.Drawing.Size(78, 15);
            this.AttachmentsLabel.TabIndex = 19;
            this.AttachmentsLabel.Text = "Attachments:";
            this.AttachmentsLabel.Visible = false;
            // 
            // DownloadAttachmentButton
            // 
            this.DownloadAttachmentButton.Location = new System.Drawing.Point(630, 292);
            this.DownloadAttachmentButton.Margin = new System.Windows.Forms.Padding(1);
            this.DownloadAttachmentButton.Name = "DownloadAttachmentButton";
            this.DownloadAttachmentButton.Size = new System.Drawing.Size(178, 22);
            this.DownloadAttachmentButton.TabIndex = 20;
            this.DownloadAttachmentButton.Text = "Download Attachment";
            this.DownloadAttachmentButton.UseVisualStyleBackColor = true;
            this.DownloadAttachmentButton.Visible = false;
            this.DownloadAttachmentButton.Click += new System.EventHandler(this.DownloadSelectedAttachment_click);
            // 
            // DownloadAllAttachmentsButton
            // 
            this.DownloadAllAttachmentsButton.Location = new System.Drawing.Point(630, 319);
            this.DownloadAllAttachmentsButton.Margin = new System.Windows.Forms.Padding(1);
            this.DownloadAllAttachmentsButton.Name = "DownloadAllAttachmentsButton";
            this.DownloadAllAttachmentsButton.Size = new System.Drawing.Size(177, 22);
            this.DownloadAllAttachmentsButton.TabIndex = 21;
            this.DownloadAllAttachmentsButton.Text = "Download All Attachments";
            this.DownloadAllAttachmentsButton.UseVisualStyleBackColor = true;
            this.DownloadAllAttachmentsButton.Visible = false;
            this.DownloadAllAttachmentsButton.Click += new System.EventHandler(this.DownloadAllAttachmentsButton_Click);
            // 
            // Reading_email
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(836, 398);
            this.Controls.Add(this.DownloadAllAttachmentsButton);
            this.Controls.Add(this.DownloadAttachmentButton);
            this.Controls.Add(this.AttachmentsLabel);
            this.Controls.Add(this.AttachmentListBox);
            this.Controls.Add(this.ToLabel);
            this.Controls.Add(this.CCLabel);
            this.Controls.Add(this.ToTextBox);
            this.Controls.Add(this.CCTextBox);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.DateTextBox);
            this.Controls.Add(this.ForwardButton);
            this.Controls.Add(this.ReplyAllButton);
            this.Controls.Add(this.ReplyButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SubjectTextBox);
            this.Controls.Add(this.FromTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Reading_email";
            this.Text = "Read Email";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox FromTextBox;
        private TextBox SubjectTextBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private RichTextBox MessageTextBox;
        private Button CloseButton;
        private Button ReplyButton;
        private Button ReplyAllButton;
        private Button ForwardButton;
        private TextBox DateTextBox;
        private Label DateLabel;
        private TextBox CCTextBox;
        private TextBox ToTextBox;
        private Label CCLabel;
        private Label ToLabel;
        private ListBox AttachmentListBox;
        private Label AttachmentsLabel;
        private Button DownloadAttachmentButton;
        private Button DownloadAllAttachmentsButton;
    }
}