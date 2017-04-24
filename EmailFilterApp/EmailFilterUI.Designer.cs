namespace EmailFilterApp
{
    partial class EmailFilterUI
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
            this.retrivebutton = new System.Windows.Forms.Button();
            this.unreadRadioButton = new System.Windows.Forms.RadioButton();
            this.allRadioButton = new System.Windows.Forms.RadioButton();
            this.readRadioButton = new System.Windows.Forms.RadioButton();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.loadbutton = new System.Windows.Forms.Button();
            this.headerComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dropdownLoadProgressBar = new System.Windows.Forms.ProgressBar();
            this.pleaseWaitLabel = new System.Windows.Forms.Label();
            this.deleteAuthenticationButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // retrivebutton
            // 
            this.retrivebutton.Location = new System.Drawing.Point(135, 257);
            this.retrivebutton.Name = "retrivebutton";
            this.retrivebutton.Size = new System.Drawing.Size(75, 23);
            this.retrivebutton.TabIndex = 7;
            this.retrivebutton.Text = "Retrive";
            this.retrivebutton.UseVisualStyleBackColor = true;
            this.retrivebutton.Click += new System.EventHandler(this.retrivebutton_Click);
            // 
            // unreadRadioButton
            // 
            this.unreadRadioButton.AutoSize = true;
            this.unreadRadioButton.Location = new System.Drawing.Point(135, 210);
            this.unreadRadioButton.Name = "unreadRadioButton";
            this.unreadRadioButton.Size = new System.Drawing.Size(60, 17);
            this.unreadRadioButton.TabIndex = 5;
            this.unreadRadioButton.TabStop = true;
            this.unreadRadioButton.Text = "Unread";
            this.unreadRadioButton.UseVisualStyleBackColor = true;
            // 
            // allRadioButton
            // 
            this.allRadioButton.AutoSize = true;
            this.allRadioButton.Location = new System.Drawing.Point(135, 233);
            this.allRadioButton.Name = "allRadioButton";
            this.allRadioButton.Size = new System.Drawing.Size(36, 17);
            this.allRadioButton.TabIndex = 6;
            this.allRadioButton.TabStop = true;
            this.allRadioButton.Text = "All";
            this.allRadioButton.UseVisualStyleBackColor = true;
            // 
            // readRadioButton
            // 
            this.readRadioButton.AutoSize = true;
            this.readRadioButton.Location = new System.Drawing.Point(135, 187);
            this.readRadioButton.Name = "readRadioButton";
            this.readRadioButton.Size = new System.Drawing.Size(51, 17);
            this.readRadioButton.TabIndex = 4;
            this.readRadioButton.TabStop = true;
            this.readRadioButton.Text = "Read";
            this.readRadioButton.UseVisualStyleBackColor = true;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(135, 20);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(199, 20);
            this.emailTextBox.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Email or User Name";
            // 
            // loadbutton
            // 
            this.loadbutton.Location = new System.Drawing.Point(135, 57);
            this.loadbutton.Name = "loadbutton";
            this.loadbutton.Size = new System.Drawing.Size(102, 23);
            this.loadbutton.TabIndex = 2;
            this.loadbutton.Text = "Load Dropdown";
            this.loadbutton.UseVisualStyleBackColor = true;
            this.loadbutton.Click += new System.EventHandler(this.loadbutton_Click);
            // 
            // headerComboBox
            // 
            this.headerComboBox.FormattingEnabled = true;
            this.headerComboBox.Location = new System.Drawing.Point(135, 160);
            this.headerComboBox.Name = "headerComboBox";
            this.headerComboBox.Size = new System.Drawing.Size(314, 21);
            this.headerComboBox.TabIndex = 3;
            this.headerComboBox.Text = "--------------------------Select Subject Of Email--------------------------------" +
    "--";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Unique Subject Of Email";
            // 
            // dropdownLoadProgressBar
            // 
            this.dropdownLoadProgressBar.Location = new System.Drawing.Point(35, 117);
            this.dropdownLoadProgressBar.Name = "dropdownLoadProgressBar";
            this.dropdownLoadProgressBar.Size = new System.Drawing.Size(439, 23);
            this.dropdownLoadProgressBar.TabIndex = 11;
            // 
            // pleaseWaitLabel
            // 
            this.pleaseWaitLabel.AutoSize = true;
            this.pleaseWaitLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pleaseWaitLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.pleaseWaitLabel.Location = new System.Drawing.Point(142, 94);
            this.pleaseWaitLabel.Name = "pleaseWaitLabel";
            this.pleaseWaitLabel.Size = new System.Drawing.Size(208, 20);
            this.pleaseWaitLabel.TabIndex = 10;
            this.pleaseWaitLabel.Text = "-------Please Wait-------";
            // 
            // deleteAuthenticationButton
            // 
            this.deleteAuthenticationButton.Location = new System.Drawing.Point(374, 18);
            this.deleteAuthenticationButton.Name = "deleteAuthenticationButton";
            this.deleteAuthenticationButton.Size = new System.Drawing.Size(117, 23);
            this.deleteAuthenticationButton.TabIndex = 13;
            this.deleteAuthenticationButton.Text = "Delete Authentication";
            this.deleteAuthenticationButton.UseVisualStyleBackColor = true;
            this.deleteAuthenticationButton.Click += new System.EventHandler(this.deleteAuthenticationButton_Click);
            // 
            // EmailFilterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 360);
            this.Controls.Add(this.deleteAuthenticationButton);
            this.Controls.Add(this.pleaseWaitLabel);
            this.Controls.Add(this.dropdownLoadProgressBar);
            this.Controls.Add(this.retrivebutton);
            this.Controls.Add(this.unreadRadioButton);
            this.Controls.Add(this.allRadioButton);
            this.Controls.Add(this.readRadioButton);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.loadbutton);
            this.Controls.Add(this.headerComboBox);
            this.Controls.Add(this.label1);
            this.Name = "EmailFilterUI";
            this.Text = "Email Filter UI";
            this.Load += new System.EventHandler(this.EmailFilterUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button retrivebutton;
        private System.Windows.Forms.RadioButton unreadRadioButton;
        private System.Windows.Forms.RadioButton allRadioButton;
        private System.Windows.Forms.RadioButton readRadioButton;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button loadbutton;
        private System.Windows.Forms.ComboBox headerComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar dropdownLoadProgressBar;
        private System.Windows.Forms.Label pleaseWaitLabel;
        private System.Windows.Forms.Button deleteAuthenticationButton;
    }
}

