namespace Email_Client_01
{
    partial class LogIn
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
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.LogInButton = new System.Windows.Forms.Button();
            this.PrimeMailTitle = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.RememberMeCheckBox = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Location = new System.Drawing.Point(87, 78);
            this.UsernameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(78, 20);
            this.UsernameLabel.TabIndex = 0;
            this.UsernameLabel.Text = "Username:";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(87, 146);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(73, 20);
            this.PasswordLabel.TabIndex = 1;
            this.PasswordLabel.Text = "Password:";
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.EmailTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.EmailTextBox.Location = new System.Drawing.Point(87, 98);
            this.EmailTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(427, 27);
            this.EmailTextBox.TabIndex = 2;
            this.EmailTextBox.Text = "example@uni.au.dk";
            this.EmailTextBox.Click += new System.EventHandler(this.EmailAddress_Click);
            this.EmailTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EmailTextBox_KeyDown);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.PasswordTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PasswordTextBox.Location = new System.Drawing.Point(87, 170);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(427, 27);
            this.PasswordTextBox.TabIndex = 3;
            this.PasswordTextBox.Click += new System.EventHandler(this.Password_Click);
            this.PasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTextBox_KeyDown);
            // 
            // LogInButton
            // 
            this.LogInButton.Location = new System.Drawing.Point(124, 258);
            this.LogInButton.Margin = new System.Windows.Forms.Padding(2);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(114, 46);
            this.LogInButton.TabIndex = 4;
            this.LogInButton.Text = "Log in";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogIn_button);
            // 
            // PrimeMailTitle
            // 
            this.PrimeMailTitle.AutoSize = true;
            this.PrimeMailTitle.Font = new System.Drawing.Font("Segoe UI", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.PrimeMailTitle.Location = new System.Drawing.Point(218, 25);
            this.PrimeMailTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PrimeMailTitle.Name = "PrimeMailTitle";
            this.PrimeMailTitle.Size = new System.Drawing.Size(174, 41);
            this.PrimeMailTitle.TabIndex = 5;
            this.PrimeMailTitle.Text = "Prime Mail";
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(370, 258);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(2);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(114, 46);
            this.ExitButton.TabIndex = 6;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.Exit_button);
            // 
            // RememberMeCheckBox
            // 
            this.RememberMeCheckBox.AutoSize = true;
            this.RememberMeCheckBox.Checked = true;
            this.RememberMeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RememberMeCheckBox.Location = new System.Drawing.Point(87, 213);
            this.RememberMeCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.RememberMeCheckBox.Name = "RememberMeCheckBox";
            this.RememberMeCheckBox.Size = new System.Drawing.Size(129, 24);
            this.RememberMeCheckBox.TabIndex = 7;
            this.RememberMeCheckBox.Text = "Remember me";
            this.RememberMeCheckBox.UseVisualStyleBackColor = true;
            // 
            // LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(640, 360);
            this.Controls.Add(this.RememberMeCheckBox);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.PrimeMailTitle);
            this.Controls.Add(this.LogInButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.EmailTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LogIn";
            this.Text = "Log in";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label UsernameLabel;
        private Label PasswordLabel;
        private TextBox EmailTextBox;
        private TextBox PasswordTextBox;
        private Button LogInButton;
        private Label PrimeMailTitle;
        private Button ExitButton;
        private CheckBox RememberMeCheckBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}