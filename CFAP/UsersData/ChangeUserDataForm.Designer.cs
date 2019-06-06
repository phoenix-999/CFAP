namespace CFAP
{
    partial class ChangeUserDataForm
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
            this.radPanel = new Telerik.WinControls.UI.RadPanel();
            this.radButton_CahngeUser = new Telerik.WinControls.UI.RadButton();
            this.radButton_Cancel = new Telerik.WinControls.UI.RadButton();
            this.radButton_AddUser = new Telerik.WinControls.UI.RadButton();
            this.radCheckedDropDownList_UserGroups = new Telerik.WinControls.UI.RadCheckedDropDownList();
            this.radTextBox_Password = new Telerik.WinControls.UI.RadTextBox();
            this.radTextBox_UserName = new Telerik.WinControls.UI.RadTextBox();
            this.radCheckBox_CanChangeUsersData = new Telerik.WinControls.UI.RadCheckBox();
            this.radCheckBox_IsAdmin = new Telerik.WinControls.UI.RadCheckBox();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel)).BeginInit();
            this.radPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_CahngeUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_AddUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckedDropDownList_UserGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox_Password)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox_UserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox_CanChangeUsersData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox_IsAdmin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel
            // 
            this.radPanel.Controls.Add(this.radButton_CahngeUser);
            this.radPanel.Controls.Add(this.radButton_Cancel);
            this.radPanel.Controls.Add(this.radButton_AddUser);
            this.radPanel.Controls.Add(this.radCheckedDropDownList_UserGroups);
            this.radPanel.Controls.Add(this.radTextBox_Password);
            this.radPanel.Controls.Add(this.radTextBox_UserName);
            this.radPanel.Controls.Add(this.radCheckBox_CanChangeUsersData);
            this.radPanel.Controls.Add(this.radCheckBox_IsAdmin);
            this.radPanel.Controls.Add(this.radLabel5);
            this.radPanel.Controls.Add(this.radLabel2);
            this.radPanel.Controls.Add(this.radLabel1);
            this.radPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel.Location = new System.Drawing.Point(0, 0);
            this.radPanel.Name = "radPanel";
            this.radPanel.Size = new System.Drawing.Size(532, 294);
            this.radPanel.TabIndex = 0;
            // 
            // radButton_CahngeUser
            // 
            this.radButton_CahngeUser.Enabled = false;
            this.radButton_CahngeUser.Location = new System.Drawing.Point(138, 258);
            this.radButton_CahngeUser.Name = "radButton_CahngeUser";
            this.radButton_CahngeUser.Size = new System.Drawing.Size(110, 24);
            this.radButton_CahngeUser.TabIndex = 12;
            this.radButton_CahngeUser.Text = "Изменить";
            this.radButton_CahngeUser.Click += new System.EventHandler(this.radButton_CahngeUser_Click);
            // 
            // radButton_Cancel
            // 
            this.radButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButton_Cancel.Location = new System.Drawing.Point(410, 258);
            this.radButton_Cancel.Name = "radButton_Cancel";
            this.radButton_Cancel.Size = new System.Drawing.Size(110, 24);
            this.radButton_Cancel.TabIndex = 11;
            this.radButton_Cancel.Text = "Отмена";
            // 
            // radButton_AddUser
            // 
            this.radButton_AddUser.Enabled = false;
            this.radButton_AddUser.Location = new System.Drawing.Point(13, 258);
            this.radButton_AddUser.Name = "radButton_AddUser";
            this.radButton_AddUser.Size = new System.Drawing.Size(110, 24);
            this.radButton_AddUser.TabIndex = 10;
            this.radButton_AddUser.Text = "Добавить";
            this.radButton_AddUser.Click += new System.EventHandler(this.radButton_AddUser_Click);
            // 
            // radCheckedDropDownList_UserGroups
            // 
            this.radCheckedDropDownList_UserGroups.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radCheckedDropDownList_UserGroups.Location = new System.Drawing.Point(242, 195);
            this.radCheckedDropDownList_UserGroups.Name = "radCheckedDropDownList_UserGroups";
            // 
            // 
            // 
            this.radCheckedDropDownList_UserGroups.RootElement.AccessibleRole = System.Windows.Forms.AccessibleRole.ListItem;
            this.radCheckedDropDownList_UserGroups.Size = new System.Drawing.Size(278, 20);
            this.radCheckedDropDownList_UserGroups.TabIndex = 9;
            this.radCheckedDropDownList_UserGroups.Click += new System.EventHandler(this.radCheckedDropDownList_UserGroups_Click);
            // 
            // radTextBox_Password
            // 
            this.radTextBox_Password.Location = new System.Drawing.Point(242, 80);
            this.radTextBox_Password.Name = "radTextBox_Password";
            this.radTextBox_Password.Size = new System.Drawing.Size(278, 20);
            this.radTextBox_Password.TabIndex = 8;
            this.radTextBox_Password.Click += new System.EventHandler(this.radTextBox_Click);
            // 
            // radTextBox_UserName
            // 
            this.radTextBox_UserName.Location = new System.Drawing.Point(242, 43);
            this.radTextBox_UserName.MaxLength = 70;
            this.radTextBox_UserName.Name = "radTextBox_UserName";
            this.radTextBox_UserName.Size = new System.Drawing.Size(278, 20);
            this.radTextBox_UserName.TabIndex = 7;
            this.radTextBox_UserName.Click += new System.EventHandler(this.radTextBox_Click);
            // 
            // radCheckBox_CanChangeUsersData
            // 
            this.radCheckBox_CanChangeUsersData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radCheckBox_CanChangeUsersData.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radCheckBox_CanChangeUsersData.Location = new System.Drawing.Point(13, 128);
            this.radCheckBox_CanChangeUsersData.Name = "radCheckBox_CanChangeUsersData";
            this.radCheckBox_CanChangeUsersData.Size = new System.Drawing.Size(318, 18);
            this.radCheckBox_CanChangeUsersData.TabIndex = 6;
            this.radCheckBox_CanChangeUsersData.Text = "Может редактировать данные других пользователей";
            // 
            // radCheckBox_IsAdmin
            // 
            this.radCheckBox_IsAdmin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radCheckBox_IsAdmin.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radCheckBox_IsAdmin.Location = new System.Drawing.Point(13, 164);
            this.radCheckBox_IsAdmin.Name = "radCheckBox_IsAdmin";
            this.radCheckBox_IsAdmin.Size = new System.Drawing.Size(108, 18);
            this.radCheckBox_IsAdmin.TabIndex = 5;
            this.radCheckBox_IsAdmin.Text = "Администратор";
            // 
            // radLabel5
            // 
            this.radLabel5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radLabel5.Location = new System.Drawing.Point(13, 198);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(128, 18);
            this.radLabel5.TabIndex = 4;
            this.radLabel5.Text = "Группы пользователя";
            // 
            // radLabel2
            // 
            this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radLabel2.Location = new System.Drawing.Point(13, 82);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(48, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "Пароль";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radLabel1.Location = new System.Drawing.Point(13, 46);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(110, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Имя пользователя";
            // 
            // ChangeUserDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.radButton_Cancel;
            this.ClientSize = new System.Drawing.Size(532, 294);
            this.Controls.Add(this.radPanel);
            this.Name = "ChangeUserDataForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Данные пользователя";
            ((System.ComponentModel.ISupportInitialize)(this.radPanel)).EndInit();
            this.radPanel.ResumeLayout(false);
            this.radPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_CahngeUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_AddUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckedDropDownList_UserGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox_Password)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox_UserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox_CanChangeUsersData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox_IsAdmin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox_CanChangeUsersData;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox_IsAdmin;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox radTextBox_Password;
        private Telerik.WinControls.UI.RadTextBox radTextBox_UserName;
        private Telerik.WinControls.UI.RadButton radButton_Cancel;
        private Telerik.WinControls.UI.RadButton radButton_AddUser;
        private Telerik.WinControls.UI.RadCheckedDropDownList radCheckedDropDownList_UserGroups;
        private Telerik.WinControls.UI.RadButton radButton_CahngeUser;
    }
}
