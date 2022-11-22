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
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
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
            this.TrashButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FromTextBox
            // 
            this.FromTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.FromTextBox.Location = new System.Drawing.Point(24, 30);
            this.FromTextBox.Name = "FromTextBox";
            this.FromTextBox.Size = new System.Drawing.Size(844, 31);
            this.FromTextBox.TabIndex = 0;
            // 
            // SubjectTextBox
            // 
            this.SubjectTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.SubjectTextBox.Location = new System.Drawing.Point(24, 143);
            this.SubjectTextBox.Name = "SubjectTextBox";
            this.SubjectTextBox.Size = new System.Drawing.Size(844, 31);
            this.SubjectTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Subject:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Message:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.MessageTextBox.Location = new System.Drawing.Point(24, 201);
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(844, 379);
            this.MessageTextBox.TabIndex = 5;
            this.MessageTextBox.Text = "";
            this.MessageTextBox.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1047, 593);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 58);
            this.button1.TabIndex = 6;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 593);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(122, 58);
            this.button2.TabIndex = 7;
            this.button2.Text = "Reply";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(143, 593);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(122, 58);
            this.button3.TabIndex = 8;
            this.button3.Text = "Reply all";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(270, 593);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(122, 58);
            this.button4.TabIndex = 9;
            this.button4.Text = "Forward";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(920, 593);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(122, 58);
            this.button5.TabIndex = 10;
            this.button5.Text = "Delete";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(664, 593);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(158, 58);
            this.button6.TabIndex = 11;
            this.button6.Text = "Mark as unread";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // DateTextBox
            // 
            this.DateTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.DateTextBox.Location = new System.Drawing.Point(900, 30);
            this.DateTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DateTextBox.Name = "DateTextBox";
            this.DateTextBox.Size = new System.Drawing.Size(254, 31);
            this.DateTextBox.TabIndex = 12;
            // 
            // DateLabel
            // 
            this.DateLabel.AutoSize = true;
            this.DateLabel.Location = new System.Drawing.Point(900, 4);
            this.DateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(53, 25);
            this.DateLabel.TabIndex = 13;
            this.DateLabel.Text = "Date:";
            // 
            // CCTextBox
            // 
            this.CCTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.CCTextBox.Location = new System.Drawing.Point(900, 84);
            this.CCTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CCTextBox.Name = "CCTextBox";
            this.CCTextBox.Size = new System.Drawing.Size(256, 31);
            this.CCTextBox.TabIndex = 14;
            this.CCTextBox.Visible = false;
            // 
            // ToTextBox
            // 
            this.ToTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ToTextBox.Location = new System.Drawing.Point(24, 84);
            this.ToTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ToTextBox.Name = "ToTextBox";
            this.ToTextBox.Size = new System.Drawing.Size(844, 31);
            this.ToTextBox.TabIndex = 15;
            // 
            // CCLabel
            // 
            this.CCLabel.AutoSize = true;
            this.CCLabel.Location = new System.Drawing.Point(900, 60);
            this.CCLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CCLabel.Name = "CCLabel";
            this.CCLabel.Size = new System.Drawing.Size(46, 25);
            this.CCLabel.TabIndex = 16;
            this.CCLabel.Text = "CCs:";
            this.CCLabel.Visible = false;
            // 
            // ToLabel
            // 
            this.ToLabel.AutoSize = true;
            this.ToLabel.Location = new System.Drawing.Point(24, 58);
            this.ToLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(34, 25);
            this.ToLabel.TabIndex = 17;
            this.ToLabel.Text = "To:";
            // 
            // AttachmentListBox
            // 
            this.AttachmentListBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.AttachmentListBox.FormattingEnabled = true;
            this.AttachmentListBox.ItemHeight = 25;
            this.AttachmentListBox.Location = new System.Drawing.Point(900, 201);
            this.AttachmentListBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.AttachmentListBox.Name = "AttachmentListBox";
            this.AttachmentListBox.Size = new System.Drawing.Size(256, 279);
            this.AttachmentListBox.TabIndex = 18;
            this.AttachmentListBox.Visible = false;
            // 
            // AttachmentsLabel
            // 
            this.AttachmentsLabel.AutoSize = true;
            this.AttachmentsLabel.Location = new System.Drawing.Point(900, 174);
            this.AttachmentsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AttachmentsLabel.Name = "AttachmentsLabel";
            this.AttachmentsLabel.Size = new System.Drawing.Size(116, 25);
            this.AttachmentsLabel.TabIndex = 19;
            this.AttachmentsLabel.Text = "Attachments:";
            this.AttachmentsLabel.Visible = false;
            // 
            // DownloadAttachmentButton
            // 
            this.DownloadAttachmentButton.Location = new System.Drawing.Point(900, 487);
            this.DownloadAttachmentButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DownloadAttachmentButton.Name = "DownloadAttachmentButton";
            this.DownloadAttachmentButton.Size = new System.Drawing.Size(254, 36);
            this.DownloadAttachmentButton.TabIndex = 20;
            this.DownloadAttachmentButton.Text = "Download Attachment";
            this.DownloadAttachmentButton.UseVisualStyleBackColor = true;
            this.DownloadAttachmentButton.Visible = false;
            this.DownloadAttachmentButton.Click += new System.EventHandler(this.DownloadAttachmentButton_Click);
            // 
            // DownloadAllAttachmentsButton
            // 
            this.DownloadAllAttachmentsButton.Location = new System.Drawing.Point(900, 532);
            this.DownloadAllAttachmentsButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.DownloadAllAttachmentsButton.Name = "DownloadAllAttachmentsButton";
            this.DownloadAllAttachmentsButton.Size = new System.Drawing.Size(252, 36);
            this.DownloadAllAttachmentsButton.TabIndex = 21;
            this.DownloadAllAttachmentsButton.Text = "Download All Attachments";
            this.DownloadAllAttachmentsButton.UseVisualStyleBackColor = true;
            this.DownloadAllAttachmentsButton.Click += new System.EventHandler(this.DownloadAllAttachmentsButton_Click);
            // 
            // TrashButton
            // 
            this.TrashButton.Location = new System.Drawing.Point(826, 594);
            this.TrashButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TrashButton.Name = "TrashButton";
            this.TrashButton.Size = new System.Drawing.Size(89, 58);
            this.TrashButton.TabIndex = 22;
            this.TrashButton.Text = "Trash";
            this.TrashButton.UseVisualStyleBackColor = true;
            this.TrashButton.Click += new System.EventHandler(this.TrashButton_Click);
            // 
            // Reading_email
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(1194, 666);
            this.Controls.Add(this.TrashButton);
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
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SubjectTextBox);
            this.Controls.Add(this.FromTextBox);
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
        private Button button4;
        private Button button5;
        private Button button6;
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
        private Button TrashButton;
    }
}