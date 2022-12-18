namespace Email_Client_01
{
    partial class metrics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(metrics));
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.displayPeriodDropdown = new System.Windows.Forms.ComboBox();
            this.displayPeriodDateSelector = new System.Windows.Forms.DateTimePicker();
            this.mailTypeDropdown = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // formsPlot1
            // 
            this.formsPlot1.Location = new System.Drawing.Point(13, 12);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(1378, 545);
            this.formsPlot1.TabIndex = 0;
            // 
            // displayPeriodDropdown
            // 
            this.displayPeriodDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.displayPeriodDropdown.FormattingEnabled = true;
            this.displayPeriodDropdown.Items.AddRange(new object[] {
            "Year",
            "Month",
            "Week"});
            this.displayPeriodDropdown.Location = new System.Drawing.Point(1416, 33);
            this.displayPeriodDropdown.Name = "displayPeriodDropdown";
            this.displayPeriodDropdown.Size = new System.Drawing.Size(121, 23);
            this.displayPeriodDropdown.TabIndex = 1;
            this.displayPeriodDropdown.SelectedIndexChanged += new System.EventHandler(this.displayPeriodDropdown_SelectedIndexChanged);
            // 
            // displayPeriodDateSelector
            // 
            this.displayPeriodDateSelector.Location = new System.Drawing.Point(1416, 62);
            this.displayPeriodDateSelector.Name = "displayPeriodDateSelector";
            this.displayPeriodDateSelector.Size = new System.Drawing.Size(121, 23);
            this.displayPeriodDateSelector.TabIndex = 2;
            this.displayPeriodDateSelector.ValueChanged += new System.EventHandler(this.displayPeriodDateSelector_ValueChanged_1);
            // 
            // mailTypeDropdown
            // 
            this.mailTypeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mailTypeDropdown.FormattingEnabled = true;
            this.mailTypeDropdown.Items.AddRange(new object[] {
            "Recieved",
            "Sent"});
            this.mailTypeDropdown.Location = new System.Drawing.Point(1416, 121);
            this.mailTypeDropdown.Name = "mailTypeDropdown";
            this.mailTypeDropdown.Size = new System.Drawing.Size(121, 23);
            this.mailTypeDropdown.TabIndex = 3;
            this.mailTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.mailTypeDropdown_SelectedIndexChanged_1);
            // 
            // metrics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1549, 573);
            this.Controls.Add(this.mailTypeDropdown);
            this.Controls.Add(this.displayPeriodDateSelector);
            this.Controls.Add(this.displayPeriodDropdown);
            this.Controls.Add(this.formsPlot1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "metrics";
            this.Text = "Metrics";
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private ComboBox displayPeriodDropdown;
        private DateTimePicker displayPeriodDateSelector;
        private ComboBox mailTypeDropdown;
    }
}