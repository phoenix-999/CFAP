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
    public partial class ChangeAccountableForm : Telerik.WinControls.UI.RadForm
    {
        Accountable accountable;
        ChangeDataOptions changeDataOption;
        CFAPBusinessLogic businessLogic;
        public ChangeAccountableForm(Accountable accountable, ChangeDataOptions changeDataOption)
        {
            InitializeComponent();

            this.accountable = accountable;
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
            if (accountable.AccountableName != null)
                this.radTextBox_AccountableName.Text = accountable.AccountableName;

            this.radCheckBox_ReadOnly.Checked = accountable.ReadOnly;
        }

        bool ValidateFormData()
        {
            bool result = true;

            if (this.radTextBox_AccountableName.Text == null || this.radTextBox_AccountableName.Text.Length == 0)
            {
                this.radTextBox_AccountableName.BackColor = Color.Red;
                result = false;
            }

            return result;
        }
        private void radButton_Add_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.AddAccountable(accountable);

            this.Close();
        }

        private void radButton_Update_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.UpdateAccountable(accountable);

            this.Close();
        }

        void SetData()
        {
            accountable.AccountableName = this.radTextBox_AccountableName.Text;
            accountable.ReadOnly = this.radCheckBox_ReadOnly.Checked;
        }

        private void radTextBox_Click(object sender, EventArgs e)
        {
            ((RadTextBox)sender).BackColor = Color.White;
        }


        
    }
}
