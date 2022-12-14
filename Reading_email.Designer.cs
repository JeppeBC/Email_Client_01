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
            this.FromTextBox = new System.Windows.Forms.TextBox();
            this.SubjectTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MessageTextBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
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
            this.FromTextBox.Location = new System.Drawing.Point(20, 24);
            this.FromTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FromTextBox.Name = "FromTextBox";
            this.FromTextBox.Size = new System.Drawing.Size(676, 27);
            this.FromTextBox.TabIndex = 0;
            // 
            // SubjectTextBox
            // 
            this.SubjectTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.SubjectTextBox.Location = new System.Drawing.Point(20, 114);
            this.SubjectTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.SubjectTextBox.Name = "SubjectTextBox";
            this.SubjectTextBox.Size = new System.Drawing.Size(676, 27);
            this.SubjectTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Subject:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 138);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Message:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.MessageTextBox.Location = new System.Drawing.Point(20, 160);
            this.MessageTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(676, 304);
            this.MessageTextBox.TabIndex = 5;
            this.MessageTextBox.Text = "";
            this.MessageTextBox.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(838, 475);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 47);
            this.button1.TabIndex = 6;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 475);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 47);
            this.button2.TabIndex = 7;
            this.button2.Text = "Reply";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(114, 475);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(97, 47);
            this.button3.TabIndex = 8;
            this.button3.Text = "Reply all";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ForwardButton
            // 
            this.ForwardButton.Location = new System.Drawing.Point(216, 475);
            this.ForwardButton.Margin = new System.Windows.Forms.Padding(2);
            this.ForwardButton.Name = "ForwardButton";
            this.ForwardButton.Size = new System.Drawing.Size(97, 47);
            this.ForwardButton.TabIndex = 9;
            this.ForwardButton.Text = "Forward";
            this.ForwardButton.UseVisualStyleBackColor = true;
            this.ForwardButton.Click += new System.EventHandler(this.ForwardButton_Click);
            // 
            // DateTextBox
            // 
            this.DateTextBox.Location = new System.Drawing.Point(720, 24);
            this.DateTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.DateTextBox.Name = "DateTextBox";
            this.DateTextBox.Size = new System.Drawing.Size(204, 27);
            this.DateTextBox.TabIndex = 12;
            // 
            // DateLabel
            // 
            this.DateLabel.AutoSize = true;
            this.DateLabel.Location = new System.Drawing.Point(720, 3);
            this.DateLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(44, 20);
            this.DateLabel.TabIndex = 13;
            this.DateLabel.Text = "Date:";
            // 
            // CCTextBox
            // 
            this.CCTextBox.Location = new System.Drawing.Point(720, 68);
            this.CCTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.CCTextBox.Name = "CCTextBox";
            this.CCTextBox.Size = new System.Drawing.Size(205, 27);
            this.CCTextBox.TabIndex = 14;
            this.CCTextBox.Visible = false;
            // 
            // ToTextBox
            // 
            this.ToTextBox.Location = new System.Drawing.Point(20, 68);
            this.ToTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.ToTextBox.Name = "ToTextBox";
            this.ToTextBox.Size = new System.Drawing.Size(676, 27);
            this.ToTextBox.TabIndex = 15;
            // 
            // CCLabel
            // 
            this.CCLabel.AutoSize = true;
            this.CCLabel.Location = new System.Drawing.Point(720, 48);
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
            this.ToLabel.Location = new System.Drawing.Point(20, 46);
            this.ToLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(28, 20);
            this.ToLabel.TabIndex = 17;
            this.ToLabel.Text = "To:";
            // 
            // AttachmentListBox
            // 
            this.AttachmentListBox.FormattingEnabled = true;
            this.AttachmentListBox.ItemHeight = 20;
            this.AttachmentListBox.Location = new System.Drawing.Point(720, 160);
            this.AttachmentListBox.Margin = new System.Windows.Forms.Padding(1);
            this.AttachmentListBox.Name = "AttachmentListBox";
            this.AttachmentListBox.Size = new System.Drawing.Size(205, 224);
            this.AttachmentListBox.TabIndex = 18;
            this.AttachmentListBox.Visible = false;
            // 
            // AttachmentsLabel
            // 
            this.AttachmentsLabel.AutoSize = true;
            this.AttachmentsLabel.Location = new System.Drawing.Point(720, 139);
            this.AttachmentsLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.AttachmentsLabel.Name = "AttachmentsLabel";
            this.AttachmentsLabel.Size = new System.Drawing.Size(95, 20);
            this.AttachmentsLabel.TabIndex = 19;
            this.AttachmentsLabel.Text = "Attachments:";
            this.AttachmentsLabel.Visible = false;
            // 
            // DownloadAttachmentButton
            // 
            this.DownloadAttachmentButton.Location = new System.Drawing.Point(720, 390);
            this.DownloadAttachmentButton.Margin = new System.Windows.Forms.Padding(1);
            this.DownloadAttachmentButton.Name = "DownloadAttachmentButton";
            this.DownloadAttachmentButton.Size = new System.Drawing.Size(203, 29);
            this.DownloadAttachmentButton.TabIndex = 20;
            this.DownloadAttachmentButton.Text = "Download Attachment";
            this.DownloadAttachmentButton.UseVisualStyleBackColor = true;
            this.DownloadAttachmentButton.Visible = false;
            this.DownloadAttachmentButton.Click += new System.EventHandler(this.DownloadAttachmentButton_Click);
            // 
            // DownloadAllAttachmentsButton
            // 
            this.DownloadAllAttachmentsButton.Location = new System.Drawing.Point(720, 425);
            this.DownloadAllAttachmentsButton.Margin = new System.Windows.Forms.Padding(1);
            this.DownloadAllAttachmentsButton.Name = "DownloadAllAttachmentsButton";
            this.DownloadAllAttachmentsButton.Size = new System.Drawing.Size(202, 29);
            this.DownloadAllAttachmentsButton.TabIndex = 21;
            this.DownloadAllAttachmentsButton.Text = "Download All Attachments";
            this.DownloadAllAttachmentsButton.UseVisualStyleBackColor = true;
            this.DownloadAllAttachmentsButton.Visible = false;
            this.DownloadAllAttachmentsButton.Click += new System.EventHandler(this.DownloadAllAttachmentsButton_Click);
            // 
            // Reading_email
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(955, 530);
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
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SubjectTextBox);
            this.Controls.Add(this.FromTextBox);
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
        private Button button1;
        private Button button2;
        private Button button3;
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