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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.richTextBox4 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.richTextBox5 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 48);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 140);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 48);
            this.label2.TabIndex = 8;
            this.label2.Text = "To:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 399);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 48);
            this.label3.TabIndex = 7;
            this.label3.Text = "Subject:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox1.Location = new System.Drawing.Point(34, 75);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(2210, 65);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.Click += new System.EventHandler(this.From_Click);
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox2.Location = new System.Drawing.Point(34, 198);
            this.richTextBox2.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(2210, 56);
            this.richTextBox2.TabIndex = 2;
            this.richTextBox2.Text = "";
            this.richTextBox2.Click += new System.EventHandler(this.To_Click);
            this.richTextBox2.TextChanged += new System.EventHandler(this.richTextBox2_TextChanged);
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox3.Location = new System.Drawing.Point(34, 457);
            this.richTextBox3.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(2210, 67);
            this.richTextBox3.TabIndex = 3;
            this.richTextBox3.Text = "";
            this.richTextBox3.Click += new System.EventHandler(this.Subject_Click);
            this.richTextBox3.TextChanged += new System.EventHandler(this.richTextBox3_TextChanged);
            // 
            // richTextBox4
            // 
            this.richTextBox4.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox4.Location = new System.Drawing.Point(38, 547);
            this.richTextBox4.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.richTextBox4.Name = "richTextBox4";
            this.richTextBox4.Size = new System.Drawing.Size(2210, 787);
            this.richTextBox4.TabIndex = 4;
            this.richTextBox4.Text = "Write your message here";
            this.richTextBox4.Click += new System.EventHandler(this.Mail_click);
            this.richTextBox4.TextChanged += new System.EventHandler(this.richTextBox4_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(2038, 1357);
            this.button1.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(214, 73);
            this.button1.TabIndex = 6;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Send_mail);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(34, 1357);
            this.button2.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(214, 73);
            this.button2.TabIndex = 5;
            this.button2.Text = "Attach file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Attach_file);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1808, 1357);
            this.button3.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(214, 73);
            this.button3.TabIndex = 9;
            this.button3.Text = "Exit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Exit_button);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 267);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 48);
            this.label4.TabIndex = 10;
            this.label4.Text = "Cc:";
            // 
            // richTextBox5
            // 
            this.richTextBox5.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.richTextBox5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox5.Location = new System.Drawing.Point(34, 324);
            this.richTextBox5.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.richTextBox5.Name = "richTextBox5";
            this.richTextBox5.Size = new System.Drawing.Size(2210, 62);
            this.richTextBox5.TabIndex = 11;
            this.richTextBox5.Text = "";
            // 
            // NewMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(20F, 48F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(2286, 1440);
            this.Controls.Add(this.richTextBox5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox4);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.Name = "NewMail";
            this.Text = "New Email";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private RichTextBox richTextBox3;
        private RichTextBox richTextBox4;
        private Button button1;
        private Button button2;
        private Button button3;
        private Label label4;
        private RichTextBox richTextBox5;
    }
}