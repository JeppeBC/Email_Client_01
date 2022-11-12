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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.AttachmentsListBox = new System.Windows.Forms.ListBox();
            this.AttachmentLabel = new System.Windows.Forms.Label();
            this.RemoveAttachmentButton = new System.Windows.Forms.Button();
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
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox2.Location = new System.Drawing.Point(12, 21);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(657, 20);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "";
            this.richTextBox2.Click += new System.EventHandler(this.To_Click);
            this.richTextBox2.TextChanged += new System.EventHandler(this.richTextBox2_TextChanged);
            this.richTextBox2.MouseHover += new System.EventHandler(this.richTextBox2_MouseHover);
            this.richTextBox2.Validating += new System.ComponentModel.CancelEventHandler(this.richTextBox2_Validating);
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox3.Location = new System.Drawing.Point(12, 100);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(657, 24);
            this.richTextBox3.TabIndex = 3;
            this.richTextBox3.Text = "";
            this.richTextBox3.Click += new System.EventHandler(this.Subject_Click);
            this.richTextBox3.TextChanged += new System.EventHandler(this.richTextBox3_TextChanged);
            // 
            // richTextBox4
            // 
            this.richTextBox4.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox4.Location = new System.Drawing.Point(13, 131);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(656, 290);
            this.richTextBox4.TabIndex = 4;
            this.richTextBox4.Text = "Write your message here";
            this.richTextBox4.Click += new System.EventHandler(this.Mail_click);
            this.richTextBox4.TextChanged += new System.EventHandler(this.richTextBox4_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(713, 424);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Send_mail);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 424);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Attach file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Attach_file);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(633, 424);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "Exit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Exit_button);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Cc:";
            // 
            // richTextBox5
            // 
            this.richTextBox5.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox5.Location = new System.Drawing.Point(12, 58);
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.Size = new System.Drawing.Size(657, 22);
            this.richTextBox5.TabIndex = 11;
            this.richTextBox5.Text = "";
            this.richTextBox5.TextChanged += new System.EventHandler(this.richTextBox5_TextChanged);
            this.richTextBox5.Validating += new System.ComponentModel.CancelEventHandler(this.richTextBox5_Validating);
            // 
            // AttachmentsListBox
            // 
            this.AttachmentsListBox.FormattingEnabled = true;
            this.AttachmentsListBox.ItemHeight = 15;
            this.AttachmentsListBox.Location = new System.Drawing.Point(684, 172);
            this.AttachmentsListBox.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.AttachmentsListBox.Name = "AttachmentsListBox";
            this.AttachmentsListBox.Size = new System.Drawing.Size(96, 154);
            this.AttachmentsListBox.TabIndex = 12;
            this.AttachmentsListBox.Visible = false;
            // 
            // AttachmentLabel
            // 
            this.AttachmentLabel.AutoSize = true;
            this.AttachmentLabel.Location = new System.Drawing.Point(684, 148);
            this.AttachmentLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.AttachmentLabel.Name = "AttachmentLabel";
            this.AttachmentLabel.Size = new System.Drawing.Size(78, 15);
            this.AttachmentLabel.TabIndex = 13;
            this.AttachmentLabel.Text = "Attachments:";
            this.AttachmentLabel.Visible = false;
            this.AttachmentLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // RemoveAttachmentButton
            // 
            this.RemoveAttachmentButton.Location = new System.Drawing.Point(692, 336);
            this.RemoveAttachmentButton.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.RemoveAttachmentButton.Name = "RemoveAttachmentButton";
            this.RemoveAttachmentButton.Size = new System.Drawing.Size(75, 48);
            this.RemoveAttachmentButton.TabIndex = 14;
            this.RemoveAttachmentButton.Text = "Remove Attachment";
            this.RemoveAttachmentButton.UseVisualStyleBackColor = true;
            this.RemoveAttachmentButton.Visible = false;
            this.RemoveAttachmentButton.Click += new System.EventHandler(this.RemoveAttachmentButton_Click);
            // 
            // NewMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(985, 526);
            this.Controls.Add(this.RemoveAttachmentButton);
            this.Controls.Add(this.AttachmentLabel);
            this.Controls.Add(this.AttachmentsListBox);
            this.Controls.Add(this.richTextBox5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox4);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "NewMail";
            this.Text = "New Email";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label label2;
        private Label label3;
        private RichTextBox richTextBox2;
        private RichTextBox richTextBox3;
        private RichTextBox richTextBox4;
        private Button button1;
        private Button button2;
        private Button button3;
        private Label label4;
        private RichTextBox richTextBox5;
        private ListBox AttachmentsListBox;
        private Label AttachmentLabel;
        private Button RemoveAttachmentButton;
    }
}