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
    public partial class ChangeBudgetItemForm : Telerik.WinControls.UI.RadForm
    {
        BudgetItem budgetItem;
        ChangeDataOptions changeDataOption;
        CFAPBusinessLogic businessLogic;
        public ChangeBudgetItemForm(BudgetItem budgetItem, ChangeDataOptions changeDataOption)
        {
            InitializeComponent();

            this.budgetItem = budgetItem;
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
            if (budgetItem.ItemName != null)
                this.radTextBox_ItemName.Text = budgetItem.ItemName;

            this.radCheckBox_ReadOnly.Checked = budgetItem.ReadOnly;
        }

        bool ValidateFormData()
        {
            bool result = true;

            if (this.radTextBox_ItemName.Text == null || this.radTextBox_ItemName.Text.Length == 0)
            {
                this.radTextBox_ItemName.BackColor = Color.Red;
                result = false;
            }

            return result;
        }
        private void radButton_Add_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.AddBudgetItem(budgetItem);

            this.Close();
        }

        private void radButton_Update_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.UpdateBudgetItem(budgetItem);

            this.Close();
        }

        void SetData()
        {
            budgetItem.ItemName = this.radTextBox_ItemName.Text;
            budgetItem.ReadOnly = this.radCheckBox_ReadOnly.Checked;
        }

        private void radTextBox_Click(object sender, EventArgs e)
        {
            ((RadTextBox)sender).BackColor = Color.White;
        }

        private void radCheckBox_ReadOnly_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для установки значения \"Только чтения\" обратитесь к администратору.");
        }


    }
}
