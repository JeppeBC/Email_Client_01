namespace Email_Client_01
{
    partial class NewMail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewMail));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RecipientTextBox = new System.Windows.Forms.RichTextBox();
            this.SubjectTextBox = new System.Windows.Forms.RichTextBox();
            this.MessageBodyTextBox = new System.Windows.Forms.RichTextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.CCTextBox = new System.Windows.Forms.RichTextBox();
            this.AttachmentsListBox = new System.Windows.Forms.ListBox();
            this.AttachmentLabel = new System.Windows.Forms.Label();
            this.RemoveAttachmentButton = new System.Windows.Forms.Button();
            this.SaveDraftButton = new System.Windows.Forms.Button();
            this.RemoveAttachment1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Subject:";
            // 
            // RecipientTextBox
            // 
            this.RecipientTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.RecipientTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.RecipientTextBox.Location = new System.Drawing.Point(12, 21);
            this.RecipientTextBox.Name = "RecipientTextBox";
            this.RecipientTextBox.Size = new System.Drawing.Size(658, 20);
            this.RecipientTextBox.TabIndex = 2;
            this.RecipientTextBox.Text = "";
            this.RecipientTextBox.Click += new System.EventHandler(this.To_Click);
            this.RecipientTextBox.MouseHover += new System.EventHandler(this.RecipientsMouseOver);
            this.RecipientTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.RecipientsValidating);
            // 
            // SubjectTextBox
            // 
            this.SubjectTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.SubjectTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SubjectTextBox.Location = new System.Drawing.Point(12, 100);
            this.SubjectTextBox.Name = "SubjectTextBox";
            this.SubjectTextBox.Size = new System.Drawing.Size(658, 24);
            this.SubjectTextBox.TabIndex = 3;
            this.SubjectTextBox.Text = "";
            this.SubjectTextBox.Click += new System.EventHandler(this.Subject_Click);
            // 
            // MessageBodyTextBox
            // 
            this.MessageBodyTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.MessageBodyTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MessageBodyTextBox.Location = new System.Drawing.Point(13, 130);
            this.MessageBodyTextBox.Name = "MessageBodyTextBox";
            this.MessageBodyTextBox.Size = new System.Drawing.Size(656, 290);
            this.MessageBodyTextBox.TabIndex = 4;
            this.MessageBodyTextBox.Text = "Write your message here";
            this.MessageBodyTextBox.Click += new System.EventHandler(this.Mail_click);
            // 
            // SendButton
            // 
            this.SendButton.Enabled = false;
            this.SendButton.Location = new System.Drawing.Point(713, 424);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(75, 22);
            this.SendButton.TabIndex = 6;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.Send_mail);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 424);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 22);
            this.button2.TabIndex = 5;
            this.button2.Text = "Attach file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Attach_file);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(633, 424);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 22);
            this.button3.TabIndex = 9;
            this.button3.Text = "Exit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Exit_button);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Cc:";
            // 
            // CCTextBox
            // 
            this.CCTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.CCTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.CCTextBox.Location = new System.Drawing.Point(12, 58);
            this.CCTextBox.Name = "CCTextBox";
            this.CCTextBox.Size = new System.Drawing.Size(658, 22);
            this.CCTextBox.TabIndex = 11;
            this.CCTextBox.Text = "";
            this.CCTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.CCValidating);
            // 
            // AttachmentsListBox
            // 
            this.AttachmentsListBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.AttachmentsListBox.FormattingEnabled = true;
            this.AttachmentsListBox.ItemHeight = 15;
            this.AttachmentsListBox.Location = new System.Drawing.Point(682, 130);
            this.AttachmentsListBox.Margin = new System.Windows.Forms.Padding(1);
            this.AttachmentsListBox.Name = "AttachmentsListBox";
            this.AttachmentsListBox.Size = new System.Drawing.Size(140, 154);
            this.AttachmentsListBox.TabIndex = 12;
            this.AttachmentsListBox.Visible = false;
            // 
            // AttachmentLabel
            // 
            this.AttachmentLabel.AutoSize = true;
            this.AttachmentLabel.Location = new System.Drawing.Point(682, 107);
            this.AttachmentLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.AttachmentLabel.Name = "AttachmentLabel";
            this.AttachmentLabel.Size = new System.Drawing.Size(78, 15);
            this.AttachmentLabel.TabIndex = 13;
            this.AttachmentLabel.Text = "Attachments:";
            this.AttachmentLabel.Visible = false;
            // 
            // RemoveAttachmentButton
            // 
            this.RemoveAttachmentButton.Location = new System.Drawing.Point(1730, 796);
            this.RemoveAttachmentButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RemoveAttachmentButton.Name = "RemoveAttachmentButton";
            this.RemoveAttachmentButton.Size = new System.Drawing.Size(74, 48);
            this.RemoveAttachmentButton.TabIndex = 14;
            this.RemoveAttachmentButton.Text = "Remove Attachment";
            this.RemoveAttachmentButton.UseVisualStyleBackColor = true;
            this.RemoveAttachmentButton.Visible = false;
            this.RemoveAttachmentButton.Click += new System.EventHandler(this.RemoveAttachmentButton_Click);
            // 
            // SaveDraftButton
            // 
            this.SaveDraftButton.Location = new System.Drawing.Point(529, 424);
            this.SaveDraftButton.Margin = new System.Windows.Forms.Padding(1);
            this.SaveDraftButton.Name = "SaveDraftButton";
            this.SaveDraftButton.Size = new System.Drawing.Size(93, 22);
            this.SaveDraftButton.TabIndex = 15;
            this.SaveDraftButton.Text = "Save as Draft";
            this.SaveDraftButton.UseVisualStyleBackColor = true;
            this.SaveDraftButton.Click += new System.EventHandler(this.SaveDraftButton_Click);
            // 
            // RemoveAttachment1
            // 
            this.RemoveAttachment1.Location = new System.Drawing.Point(682, 295);
            this.RemoveAttachment1.Margin = new System.Windows.Forms.Padding(1);
            this.RemoveAttachment1.Name = "RemoveAttachment1";
            this.RemoveAttachment1.Size = new System.Drawing.Size(140, 22);
            this.RemoveAttachment1.TabIndex = 16;
            this.RemoveAttachment1.Text = "Remove attachment";
            this.RemoveAttachment1.UseVisualStyleBackColor = true;
            this.RemoveAttachment1.Visible = false;
            this.RemoveAttachment1.Click += new System.EventHandler(this.RemoveAttachmentButton_Click);
            // 
            // NewMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(832, 455);
            this.Controls.Add(this.RemoveAttachment1);
            this.Controls.Add(this.SaveDraftButton);
            this.Controls.Add(this.RemoveAttachmentButton);
            this.Controls.Add(this.AttachmentLabel);
            this.Controls.Add(this.AttachmentsListBox);
            this.Controls.Add(this.CCTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.MessageBodyTextBox);
            this.Controls.Add(this.SubjectTextBox);
            this.Controls.Add(this.RecipientTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewMail";
            this.Text = "New Email";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label label2;
        private Label label3;
        private RichTextBox RecipientTextBox;
        private RichTextBox SubjectTextBox;
        private RichTextBox MessageBodyTextBox;
        private Button SendButton;
        private Button button2;
        private Button button3;
        private Label label4;
        private RichTextBox CCTextBox;
        private ListBox AttachmentsListBox;
        private Label AttachmentLabel;
        private Button RemoveAttachmentButton;
        private Button SaveDraftButton;
        private Button RemoveAttachment1;
    }
}