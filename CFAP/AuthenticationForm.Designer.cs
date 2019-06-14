namespace CFAP
{
    partial class AuthenticationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuthenticationForm));
            this.radLabel_Login = new Telerik.WinControls.UI.RadLabel();
            this.radLabel_AuthenticateText = new Telerik.WinControls.UI.RadLabel();
            this.radLabel_Password = new Telerik.WinControls.UI.RadLabel();
            this.radButton_Ok = new Telerik.WinControls.UI.RadButton();
            this.radButton_Cancel = new Telerik.WinControls.UI.RadButton();
            this.radGroupBox_Main = new Telerik.WinControls.UI.RadGroupBox();
            this.radDropDownList_Logins = new Telerik.WinControls.UI.RadDropDownList();
            this.radTextBox_Password = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel_Login)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel_AuthenticateText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel_Password)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Ok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox_Main)).BeginInit();
            this.radGroupBox_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList_Logins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox_Password)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel_Login
            // 
            this.radLabel_Login.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.radLabel_Login.Location = new System.Drawing.Point(5, 52);
            this.radLabel_Login.Name = "radLabel_Login";
            this.radLabel_Login.Size = new System.Drawing.Size(57, 25);
            this.radLabel_Login.TabIndex = 0;
            this.radLabel_Login.Text = "Логин";
            // 
            // radLabel_AuthenticateText
            // 
            this.radLabel_AuthenticateText.AutoSize = false;
            this.radLabel_AuthenticateText.BorderVisible = true;
            this.radLabel_AuthenticateText.Dock = System.Windows.Forms.DockStyle.Top;
            this.radLabel_AuthenticateText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.radLabel_AuthenticateText.Location = new System.Drawing.Point(0, 0);
            this.radLabel_AuthenticateText.Name = "radLabel_AuthenticateText";
            // 
            // 
            // 
            this.radLabel_AuthenticateText.RootElement.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.radLabel_AuthenticateText.RootElement.ShouldPaint = false;
            this.radLabel_AuthenticateText.Size = new System.Drawing.Size(292, 25);
            this.radLabel_AuthenticateText.TabIndex = 1;
            this.radLabel_AuthenticateText.Text = "Введите логин и пароль";
            this.radLabel_AuthenticateText.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radLabel_Password
            // 
            this.radLabel_Password.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.radLabel_Password.Location = new System.Drawing.Point(5, 100);
            this.radLabel_Password.Name = "radLabel_Password";
            this.radLabel_Password.Size = new System.Drawing.Size(69, 25);
            this.radLabel_Password.TabIndex = 2;
            this.radLabel_Password.Text = "Пароль";
            // 
            // radButton_Ok
            // 
            this.radButton_Ok.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.radButton_Ok.Location = new System.Drawing.Point(12, 189);
            this.radButton_Ok.Name = "radButton_Ok";
            this.radButton_Ok.Size = new System.Drawing.Size(110, 24);
            this.radButton_Ok.TabIndex = 3;
            this.radButton_Ok.Text = "OK";
            this.radButton_Ok.Click += new System.EventHandler(this.radButton_Ok_Click);
            // 
            // radButton_Cancel
            // 
            this.radButton_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.radButton_Cancel.Location = new System.Drawing.Point(170, 189);
            this.radButton_Cancel.Name = "radButton_Cancel";
            this.radButton_Cancel.Size = new System.Drawing.Size(110, 24);
            this.radButton_Cancel.TabIndex = 4;
            this.radButton_Cancel.Text = "Отмена";
            this.radButton_Cancel.Click += new System.EventHandler(this.radButton_Cancel_Click);
            // 
            // radGroupBox_Main
            // 
            this.radGroupBox_Main.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox_Main.Controls.Add(this.radDropDownList_Logins);
            this.radGroupBox_Main.Controls.Add(this.radTextBox_Password);
            this.radGroupBox_Main.Controls.Add(this.radLabel_Login);
            this.radGroupBox_Main.Controls.Add(this.radLabel_Password);
            this.radGroupBox_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGroupBox_Main.HeaderPosition = Telerik.WinControls.UI.HeaderPosition.Left;
            this.radGroupBox_Main.HeaderText = "";
            this.radGroupBox_Main.Location = new System.Drawing.Point(0, 0);
            this.radGroupBox_Main.Name = "radGroupBox_Main";
            // 
            // 
            // 
            this.radGroupBox_Main.RootElement.BorderHighlightColor = System.Drawing.Color.Gray;
            this.radGroupBox_Main.RootElement.BorderHighlightThickness = 3;
            this.radGroupBox_Main.Size = new System.Drawing.Size(292, 221);
            this.radGroupBox_Main.TabIndex = 5;
            // 
            // radDropDownList_Logins
            // 
            this.radDropDownList_Logins.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radDropDownList_Logins.DropDownAnimationEnabled = false;
            this.radDropDownList_Logins.Location = new System.Drawing.Point(94, 56);
            this.radDropDownList_Logins.Name = "radDropDownList_Logins";
            this.radDropDownList_Logins.Size = new System.Drawing.Size(193, 20);
            this.radDropDownList_Logins.TabIndex = 4;
            this.radDropDownList_Logins.Text = "Выбрать логин из списка";
            // 
            // radTextBox_Password
            // 
            this.radTextBox_Password.AcceptsTab = true;
            this.radTextBox_Password.Location = new System.Drawing.Point(94, 103);
            this.radTextBox_Password.Name = "radTextBox_Password";
            this.radTextBox_Password.PasswordChar = '*';
            this.radTextBox_Password.Size = new System.Drawing.Size(193, 20);
            this.radTextBox_Password.TabIndex = 3;
            // 
            // AuthenticationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 221);
            this.Controls.Add(this.radButton_Cancel);
            this.Controls.Add(this.radButton_Ok);
            this.Controls.Add(this.radLabel_AuthenticateText);
            this.Controls.Add(this.radGroupBox_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AuthenticationForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Аутентификация";
            this.Load += new System.EventHandler(this.AuthenticationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel_Login)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel_AuthenticateText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel_Password)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Ok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox_Main)).EndInit();
            this.radGroupBox_Main.ResumeLayout(false);
            this.radGroupBox_Main.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList_Logins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox_Password)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel_Login;
        private Telerik.WinControls.UI.RadLabel radLabel_AuthenticateText;
        private Telerik.WinControls.UI.RadLabel radLabel_Password;
        private Telerik.WinControls.UI.RadButton radButton_Ok;
        private Telerik.WinControls.UI.RadButton radButton_Cancel;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox_Main;
        private Telerik.WinControls.UI.RadTextBox radTextBox_Password;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList_Logins;
    }
}