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
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.loadbutton = new System.Windows.Forms.Button();
            this.headerComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // retrivebutton
            // 
            this.retrivebutton.Location = new System.Drawing.Point(135, 230);
            this.retrivebutton.Name = "retrivebutton";
            this.retrivebutton.Size = new System.Drawing.Size(75, 23);
            this.retrivebutton.TabIndex = 17;
            this.retrivebutton.Text = "Retrive";
            this.retrivebutton.UseVisualStyleBackColor = true;
            this.retrivebutton.Click += new System.EventHandler(this.retrivebutton_Click);
            // 
            // unreadRadioButton
            // 
            this.unreadRadioButton.AutoSize = true;
            this.unreadRadioButton.Location = new System.Drawing.Point(135, 183);
            this.unreadRadioButton.Name = "unreadRadioButton";
            this.unreadRadioButton.Size = new System.Drawing.Size(60, 17);
            this.unreadRadioButton.TabIndex = 14;
            this.unreadRadioButton.TabStop = true;
            this.unreadRadioButton.Text = "Unread";
            this.unreadRadioButton.UseVisualStyleBackColor = true;
            // 
            // allRadioButton
            // 
            this.allRadioButton.AutoSize = true;
            this.allRadioButton.Location = new System.Drawing.Point(135, 206);
            this.allRadioButton.Name = "allRadioButton";
            this.allRadioButton.Size = new System.Drawing.Size(36, 17);
            this.allRadioButton.TabIndex = 15;
            this.allRadioButton.TabStop = true;
            this.allRadioButton.Text = "All";
            this.allRadioButton.UseVisualStyleBackColor = true;
            // 
            // readRadioButton
            // 
            this.readRadioButton.AutoSize = true;
            this.readRadioButton.Location = new System.Drawing.Point(135, 160);
            this.readRadioButton.Name = "readRadioButton";
            this.readRadioButton.Size = new System.Drawing.Size(51, 17);
            this.readRadioButton.TabIndex = 16;
            this.readRadioButton.TabStop = true;
            this.readRadioButton.Text = "Read";
            this.readRadioButton.UseVisualStyleBackColor = true;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(135, 46);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(199, 20);
            this.passwordTextBox.TabIndex = 12;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(79, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Password";
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(135, 20);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(199, 20);
            this.emailTextBox.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Email or User Name";
            // 
            // loadbutton
            // 
            this.loadbutton.Location = new System.Drawing.Point(135, 87);
            this.loadbutton.Name = "loadbutton";
            this.loadbutton.Size = new System.Drawing.Size(102, 23);
            this.loadbutton.TabIndex = 9;
            this.loadbutton.Text = "Load Dropdown";
            this.loadbutton.UseVisualStyleBackColor = true;
            this.loadbutton.Click += new System.EventHandler(this.loadbutton_Click);
            // 
            // headerComboBox
            // 
            this.headerComboBox.FormattingEnabled = true;
            this.headerComboBox.Location = new System.Drawing.Point(135, 133);
            this.headerComboBox.Name = "headerComboBox";
            this.headerComboBox.Size = new System.Drawing.Size(314, 21);
            this.headerComboBox.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Unique Subject Of Email";
            // 
            // EmailFilterUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 360);
            this.Controls.Add(this.retrivebutton);
            this.Controls.Add(this.unreadRadioButton);
            this.Controls.Add(this.allRadioButton);
            this.Controls.Add(this.readRadioButton);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.label3);
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
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button loadbutton;
        private System.Windows.Forms.ComboBox headerComboBox;
        private System.Windows.Forms.Label label1;
    }
}

