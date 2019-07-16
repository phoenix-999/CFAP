using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CFAP.DataProviderClient;
using Telerik.WinControls.UI;
using System.Reflection;

namespace CFAP
{
    public partial class ChangeUserGroupForm : Telerik.WinControls.UI.RadForm
    {
        UserGroup userGroup;
        ChangeDataOptions changeDataOption;
        CFAPBusinessLogic businessLogic;
        public ChangeUserGroupForm(UserGroup userGroup, ChangeDataOptions changeDataOption)
        {
            InitializeComponent();

            this.userGroup = userGroup;
            this.changeDataOption = changeDataOption;

            InitializeFileds();
            InitializeButtons();

            businessLogic = new CFAPBusinessLogic(new ExceptionsHandlerUI());
        }

        void InitializeButtons()
        {
            switch(this.changeDataOption)
            {
                case ChangeDataOptions.AddNew:
                    this.radButton_AddAccountable.Enabled = true;
                    break;
                case ChangeDataOptions.ChangeData:
                    this.radButton_UpdateAccountable.Enabled = true;
                    break;
            }
        }

        void InitializeFileds()
        {
            if (userGroup.GroupName != null)
                this.radTextBox_GroupName.Text = userGroup.GroupName;

            this.radCheckBox_CanReadAccountablesSummary.Checked = userGroup.CanReadAccountablesSummary;
        }

        bool ValidateFormData()
        {
            bool result = true;

            if (this.radTextBox_GroupName.Text == null || this.radTextBox_GroupName.Text.Length == 0)
            {
                this.radTextBox_GroupName.BackColor = Color.Red;
                result = false;
            }

            return result;
        }
        private void radButton_Add_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.AddUserGroup(userGroup);

            MessageBox.Show("Изменения вступят в силу после следующего входа в систему", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void radButton_Update_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.UpdateUserGroup(userGroup);

            MessageBox.Show("Изменения вступят в силу после следующего входа в систему", "Инфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        void SetData()
        {
            userGroup.GroupName = this.radTextBox_GroupName.Text;
            userGroup.CanReadAccountablesSummary = this.radCheckBox_CanReadAccountablesSummary.Checked;
        }

        private void radTextBox_Click(object sender, EventArgs e)
        {
            ((RadTextBox)sender).BackColor = Color.White;
        }

    }
}
