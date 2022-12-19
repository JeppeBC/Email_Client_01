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
            this.Close = new System.Windows.Forms.Button();
            this.Reply1 = new System.Windows.Forms.Button();
            this.ReplyAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FromTextBox
            // 
            this.FromTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.FromTextBox.Location = new System.Drawing.Point(21, 24);
            this.FromTextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.FromTextBox.Name = "FromTextBox";
            this.FromTextBox.Size = new System.Drawing.Size(676, 27);
            this.FromTextBox.TabIndex = 0;
            // 
            // SubjectTextBox
            // 
            this.SubjectTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.SubjectTextBox.Location = new System.Drawing.Point(21, 130);
            this.SubjectTextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SubjectTextBox.Name = "SubjectTextBox";
            this.SubjectTextBox.Size = new System.Drawing.Size(676, 27);
            this.SubjectTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 107);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Subject:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 160);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Message:";
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.MessageTextBox.Location = new System.Drawing.Point(21, 183);
            this.MessageTextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(676, 304);
            this.MessageTextBox.TabIndex = 5;
            this.MessageTextBox.Text = "";
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(2393, 1519);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(278, 149);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ReplyButton
            // 
            this.ReplyButton.Location = new System.Drawing.Point(35, 1519);
            this.ReplyButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.ReplyButton.Name = "ReplyButton";
            this.ReplyButton.Size = new System.Drawing.Size(278, 149);
            this.ReplyButton.TabIndex = 7;
            this.ReplyButton.Text = "Reply";
            this.ReplyButton.UseVisualStyleBackColor = true;
            this.ReplyButton.Click += new System.EventHandler(this.Reply_Click);
            // 
            // ReplyAllButton
            // 
            this.ReplyAllButton.Location = new System.Drawing.Point(327, 1519);
            this.ReplyAllButton.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.ReplyAllButton.Name = "ReplyAllButton";
            this.ReplyAllButton.Size = new System.Drawing.Size(278, 149);
            this.ReplyAllButton.TabIndex = 8;
            this.ReplyAllButton.Text = "Reply all";
            this.ReplyAllButton.UseVisualStyleBackColor = true;
            this.ReplyAllButton.Click += new System.EventHandler(this.ReplyAll_Click);
            // 
            // ForwardButton
            // 
            this.ForwardButton.Location = new System.Drawing.Point(223, 510);
            this.ForwardButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ForwardButton.Name = "ForwardButton";
            this.ForwardButton.Size = new System.Drawing.Size(97, 47);
            this.ForwardButton.TabIndex = 9;
            this.ForwardButton.Text = "Forward";
            this.ForwardButton.UseVisualStyleBackColor = true;
            this.ForwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            // 
            // DateTextBox
            // 
            this.DateTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.DateTextBox.Location = new System.Drawing.Point(756, 24);
            this.DateTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.DateTextBox.Name = "DateTextBox";
            this.DateTextBox.Size = new System.Drawing.Size(204, 27);
            this.DateTextBox.TabIndex = 12;
            // 
            // DateLabel
            // 
            this.DateLabel.AutoSize = true;
            this.DateLabel.Location = new System.Drawing.Point(756, -1);
            this.DateLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(44, 20);
            this.DateLabel.TabIndex = 13;
            this.DateLabel.Text = "Date:";
            // 
            // CCTextBox
            // 
            this.CCTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.CCTextBox.Location = new System.Drawing.Point(756, 77);
            this.CCTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.CCTextBox.Name = "CCTextBox";
            this.CCTextBox.Size = new System.Drawing.Size(205, 27);
            this.CCTextBox.TabIndex = 14;
            this.CCTextBox.Visible = false;
            // 
            // ToTextBox
            // 
            this.ToTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ToTextBox.Location = new System.Drawing.Point(21, 79);
            this.ToTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.ToTextBox.Name = "ToTextBox";
            this.ToTextBox.Size = new System.Drawing.Size(676, 27);
            this.ToTextBox.TabIndex = 15;
            // 
            // CCLabel
            // 
            this.CCLabel.AutoSize = true;
            this.CCLabel.Location = new System.Drawing.Point(756, 56);
            this.CCLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.CCLabel.Name = "CCLabel";
            this.CCLabel.Size = new System.Drawing.Size(36, 20);
            this.CCLabel.TabIndex = 16;
            this.CCLabel.Text = "CCs:";
            this.CCLabel.Visible = false;
            // 
            // ToLabel
            // 
            this.ToLabel.AutoSize = true;
            this.ToLabel.Location = new System.Drawing.Point(21, 54);
            this.ToLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(28, 20);
            this.ToLabel.TabIndex = 17;
            this.ToLabel.Text = "To:";
            // 
            // AttachmentListBox
            // 
            this.AttachmentListBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.AttachmentListBox.FormattingEnabled = true;
            this.AttachmentListBox.ItemHeight = 20;
            this.AttachmentListBox.Location = new System.Drawing.Point(756, 184);
            this.AttachmentListBox.Margin = new System.Windows.Forms.Padding(1);
            this.AttachmentListBox.Name = "AttachmentListBox";
            this.AttachmentListBox.Size = new System.Drawing.Size(205, 224);
            this.AttachmentListBox.TabIndex = 18;
            this.AttachmentListBox.Visible = false;
            // 
            // AttachmentsLabel
            // 
            this.AttachmentsLabel.AutoSize = true;
            this.AttachmentsLabel.Location = new System.Drawing.Point(756, 163);
            this.AttachmentsLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.AttachmentsLabel.Name = "AttachmentsLabel";
            this.AttachmentsLabel.Size = new System.Drawing.Size(95, 20);
            this.AttachmentsLabel.TabIndex = 19;
            this.AttachmentsLabel.Text = "Attachments:";
            this.AttachmentsLabel.Visible = false;
            // 
            // DownloadAttachmentButton
            // 
            this.DownloadAttachmentButton.Location = new System.Drawing.Point(756, 413);
            this.DownloadAttachmentButton.Margin = new System.Windows.Forms.Padding(1);
            this.DownloadAttachmentButton.Name = "DownloadAttachmentButton";
            this.DownloadAttachmentButton.Size = new System.Drawing.Size(203, 29);
            this.DownloadAttachmentButton.TabIndex = 20;
            this.DownloadAttachmentButton.Text = "Download Attachment";
            this.DownloadAttachmentButton.UseVisualStyleBackColor = true;
            this.DownloadAttachmentButton.Visible = false;
            this.DownloadAttachmentButton.Click += new System.EventHandler(this.DownloadSelectedAttachment_click);
            // 
            // DownloadAllAttachmentsButton
            // 
            this.DownloadAllAttachmentsButton.Location = new System.Drawing.Point(756, 449);
            this.DownloadAllAttachmentsButton.Margin = new System.Windows.Forms.Padding(1);
            this.DownloadAllAttachmentsButton.Name = "DownloadAllAttachmentsButton";
            this.DownloadAllAttachmentsButton.Size = new System.Drawing.Size(202, 29);
            this.DownloadAllAttachmentsButton.TabIndex = 21;
            this.DownloadAllAttachmentsButton.Text = "Download All Attachments";
            this.DownloadAllAttachmentsButton.UseVisualStyleBackColor = true;
            this.DownloadAllAttachmentsButton.Visible = false;
            this.DownloadAllAttachmentsButton.Click += new System.EventHandler(this.DownloadAllAttachmentsButton_Click);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(863, 510);
            this.Close.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(97, 47);
            this.Close.TabIndex = 22;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // Reply1
            // 
            this.Reply1.Location = new System.Drawing.Point(21, 510);
            this.Reply1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Reply1.Name = "Reply1";
            this.Reply1.Size = new System.Drawing.Size(97, 47);
            this.Reply1.TabIndex = 23;
            this.Reply1.Text = "Reply";
            this.Reply1.UseVisualStyleBackColor = true;
            this.Reply1.Click += new System.EventHandler(this.Reply_Click);
            // 
            // ReplyAll
            // 
            this.ReplyAll.Location = new System.Drawing.Point(122, 510);
            this.ReplyAll.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ReplyAll.Name = "ReplyAll";
            this.ReplyAll.Size = new System.Drawing.Size(97, 47);
            this.ReplyAll.TabIndex = 24;
            this.ReplyAll.Text = "Reply all";
            this.ReplyAll.UseVisualStyleBackColor = true;
            this.ReplyAll.Click += new System.EventHandler(this.ReplyAll_Click);
            // 
            // Reading_email
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(971, 569);
            this.Controls.Add(this.ReplyAll);
            this.Controls.Add(this.Reply1);
            this.Controls.Add(this.Close);
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
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
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
        private Button Close;
        private Button Reply1;
        private Button ReplyAll;
    }
}