namespace CFAP
{
    partial class ChangeUserGroupForm
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
            this.radButton_UpdateAccountable = new Telerik.WinControls.UI.RadButton();
            this.radButton_Cancel = new Telerik.WinControls.UI.RadButton();
            this.radButton_AddAccountable = new Telerik.WinControls.UI.RadButton();
            this.radTextBox_GroupName = new Telerik.WinControls.UI.RadTextBox();
            this.radCheckBox_CanReadAllData = new Telerik.WinControls.UI.RadCheckBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel)).BeginInit();
            this.radPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_UpdateAccountable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_AddAccountable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox_GroupName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox_CanReadAllData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel
            // 
            this.radPanel.Controls.Add(this.radButton_UpdateAccountable);
            this.radPanel.Controls.Add(this.radButton_Cancel);
            this.radPanel.Controls.Add(this.radButton_AddAccountable);
            this.radPanel.Controls.Add(this.radTextBox_GroupName);
            this.radPanel.Controls.Add(this.radCheckBox_CanReadAllData);
            this.radPanel.Controls.Add(this.radLabel1);
            this.radPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel.Location = new System.Drawing.Point(0, 0);
            this.radPanel.Name = "radPanel";
            this.radPanel.Size = new System.Drawing.Size(532, 294);
            this.radPanel.TabIndex = 0;
            // 
            // radButton_UpdateAccountable
            // 
            this.radButton_UpdateAccountable.Enabled = false;
            this.radButton_UpdateAccountable.Location = new System.Drawing.Point(138, 258);
            this.radButton_UpdateAccountable.Name = "radButton_UpdateAccountable";
            this.radButton_UpdateAccountable.Size = new System.Drawing.Size(110, 24);
            this.radButton_UpdateAccountable.TabIndex = 12;
            this.radButton_UpdateAccountable.Text = "Изменить";
            this.radButton_UpdateAccountable.Click += new System.EventHandler(this.radButton_Update_Click);
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
            // radButton_AddAccountable
            // 
            this.radButton_AddAccountable.Enabled = false;
            this.radButton_AddAccountable.Location = new System.Drawing.Point(13, 258);
            this.radButton_AddAccountable.Name = "radButton_AddAccountable";
            this.radButton_AddAccountable.Size = new System.Drawing.Size(110, 24);
            this.radButton_AddAccountable.TabIndex = 10;
            this.radButton_AddAccountable.Text = "Добавить";
            this.radButton_AddAccountable.Click += new System.EventHandler(this.radButton_Add_Click);
            // 
            // radTextBox_GroupName
            // 
            this.radTextBox_GroupName.Location = new System.Drawing.Point(170, 43);
            this.radTextBox_GroupName.MaxLength = 70;
            this.radTextBox_GroupName.Name = "radTextBox_GroupName";
            this.radTextBox_GroupName.Size = new System.Drawing.Size(350, 20);
            this.radTextBox_GroupName.TabIndex = 7;
            this.radTextBox_GroupName.Click += new System.EventHandler(this.radTextBox_Click);
            // 
            // radCheckBox_CanReadAllData
            // 
            this.radCheckBox_CanReadAllData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radCheckBox_CanReadAllData.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radCheckBox_CanReadAllData.Location = new System.Drawing.Point(15, 80);
            this.radCheckBox_CanReadAllData.Name = "radCheckBox_CanReadAllData";
            this.radCheckBox_CanReadAllData.Size = new System.Drawing.Size(322, 18);
            this.radCheckBox_CanReadAllData.TabIndex = 5;
            this.radCheckBox_CanReadAllData.Text = "Доступ ко всем данным (с момента создания группы)";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.radLabel1.Location = new System.Drawing.Point(13, 46);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(132, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Группа пользователей";
            // 
            // ChangeUserGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.radButton_Cancel;
            this.ClientSize = new System.Drawing.Size(532, 294);
            this.Controls.Add(this.radPanel);
            this.Name = "ChangeUserGroupForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Группа пользователей";
            ((System.ComponentModel.ISupportInitialize)(this.radPanel)).EndInit();
            this.radPanel.ResumeLayout(false);
            this.radPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_UpdateAccountable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_AddAccountable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox_GroupName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox_CanReadAllData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadTextBox radTextBox_GroupName;
        private Telerik.WinControls.UI.RadButton radButton_Cancel;
        private Telerik.WinControls.UI.RadButton radButton_AddAccountable;
        private Telerik.WinControls.UI.RadButton radButton_UpdateAccountable;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox_CanReadAllData;
    }
}
