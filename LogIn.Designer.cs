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
            this.UsernameLabel.Location = new System.Drawing.Point(218, 186);
            this.UsernameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(188, 48);
            this.UsernameLabel.TabIndex = 0;
            this.UsernameLabel.Text = "Username:";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(218, 351);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(176, 48);
            this.PasswordLabel.TabIndex = 1;
            this.PasswordLabel.Text = "Password:";
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.EmailTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.EmailTextBox.Location = new System.Drawing.Point(218, 236);
            this.EmailTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(1062, 55);
            this.EmailTextBox.TabIndex = 2;
            this.EmailTextBox.Text = "example@uni.au.dk";
            this.EmailTextBox.Click += new System.EventHandler(this.EmailAddress_Click);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.PasswordTextBox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PasswordTextBox.Location = new System.Drawing.Point(218, 407);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(1062, 55);
            this.PasswordTextBox.TabIndex = 3;
            this.PasswordTextBox.Click += new System.EventHandler(this.Password_Click);
            // 
            // LogInButton
            // 
            this.LogInButton.Location = new System.Drawing.Point(310, 618);
            this.LogInButton.Margin = new System.Windows.Forms.Padding(6);
            this.LogInButton.Name = "LogInButton";
            this.LogInButton.Size = new System.Drawing.Size(286, 111);
            this.LogInButton.TabIndex = 4;
            this.LogInButton.Text = "Log in";
            this.LogInButton.UseVisualStyleBackColor = true;
            this.LogInButton.Click += new System.EventHandler(this.LogIn_button);
            // 
            // PrimeMailTitle
            // 
            this.PrimeMailTitle.AutoSize = true;
            this.PrimeMailTitle.Font = new System.Drawing.Font("Segoe UI", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.PrimeMailTitle.Location = new System.Drawing.Point(544, 61);
            this.PrimeMailTitle.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PrimeMailTitle.Name = "PrimeMailTitle";
            this.PrimeMailTitle.Size = new System.Drawing.Size(412, 96);
            this.PrimeMailTitle.TabIndex = 5;
            this.PrimeMailTitle.Text = "Prime Mail";
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(924, 618);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(6);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(286, 111);
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
            this.RememberMeCheckBox.Location = new System.Drawing.Point(218, 511);
            this.RememberMeCheckBox.Margin = new System.Windows.Forms.Padding(6);
            this.RememberMeCheckBox.Name = "RememberMeCheckBox";
            this.RememberMeCheckBox.Size = new System.Drawing.Size(300, 52);
            this.RememberMeCheckBox.TabIndex = 7;
            this.RememberMeCheckBox.Text = "Remember me";
            this.RememberMeCheckBox.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(20F, 48F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.ClientSize = new System.Drawing.Size(1600, 864);
            this.Controls.Add(this.RememberMeCheckBox);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.PrimeMailTitle);
            this.Controls.Add(this.LogInButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.EmailTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UsernameLabel);
            this.Margin = new System.Windows.Forms.Padding(6);
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