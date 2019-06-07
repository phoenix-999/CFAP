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
using System.Linq;

namespace CFAP
{
    public partial class ChangeSummaryForm : Telerik.WinControls.UI.RadForm
    {
        Summary summary;
        ChangeDataOptions changeDataOption;
        CFAPBusinessLogic businessLogic;

        public ChangeSummaryForm(Summary summary, ChangeDataOptions changeDataOption)
        {
            InitializeComponent();

            this.summary = summary;
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
            this.radCheckBox_CashFlowType.Checked = summary.CashFlowType;
            this.radDateTimePicker_SummaryDate.Value = summary.SummaryDate;
            this.radMaskedEditBox_SummaUAH.Value = summary.SummaUAH;
            if (summary.Description != null)
                this.radTextBox_Description.Text = summary.Description;

            InitializeListBoxes();    
        }

        void InitializeListBoxes()
        {
            bool isLoadGeneralData = true;

            if (CFAPBusinessLogic.BudgetItems != null)
                this.radDropDownList_BudgetItems.DataSource = CFAPBusinessLogic.BudgetItems;
            else
                isLoadGeneralData = false;

            if (CFAPBusinessLogic.Projects != null)
                this.radDropDownList_Projects.DataSource = CFAPBusinessLogic.Projects;
            else
                isLoadGeneralData = false;

            if (CFAPBusinessLogic.Accountables != null)
                this.radDropDownList_Accountables.DataSource = CFAPBusinessLogic.Accountables;
            else
                isLoadGeneralData = false;

            if (!isLoadGeneralData)
            {
                MessageBox.Show("Некоторые данные не были получены", "Ошибка загрузки данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (summary.BudgetItem != null)
            {
                var checkedItem = (from i in CFAPBusinessLogic.BudgetItems where i.Id == summary.BudgetItem.Id select i).FirstOrDefault();
                if (checkedItem != null)
                {
                    var itemToSelect = (from i in this.radDropDownList_BudgetItems.Items where checkedItem.Id == ((BudgetItem)i.DataBoundItem).Id select i).First();
                    itemToSelect.Selected = true;
                }
            }

            if (summary.Project != null)
            {
                var checkedItem = (from i in CFAPBusinessLogic.Projects where i.Id == summary.Project.Id select i).FirstOrDefault();
                if (checkedItem != null)
                {
                    var itemToSelect = (from i in this.radDropDownList_Projects.Items where checkedItem.Id == ((Project)i.DataBoundItem).Id select i).First();
                    itemToSelect.Selected = true;
                }
            }

            if (summary.Accountable != null)
            {
                var checkedItem = (from i in CFAPBusinessLogic.Accountables where i.Id == summary.Accountable.Id select i).FirstOrDefault();
                if (checkedItem != null)
                {
                    var itemToSelect = (from i in this.radDropDownList_Accountables.Items where checkedItem.Id == ((Accountable)i.DataBoundItem).Id select i).First();
                    itemToSelect.Selected = true;
                }
            }


        }

        bool ValidateFormData()
        {
            bool result = true;

            try
            {
                double.Parse(this.radMaskedEditBox_SummaUAH.Value.ToString());
            }
            catch(Exception)
            { 
                result = false;
                this.radMaskedEditBox_SummaUAH.BackColor = Color.Red;
            }

            if (this.radDateTimePicker_SummaryDate.Value == null || this.radDateTimePicker_SummaryDate.Value == default(DateTime))
            {
                result = false;
                this.radDateTimePicker_SummaryDate.BackColor = Color.Red;
            }

            if (this.radDropDownList_Accountables.SelectedItems == null || this.radDropDownList_Accountables.SelectedItems.Count == 0)
            {
                result = false;
                this.radDropDownList_Accountables.BackColor = Color.Red;
            }

            if (this.radDropDownList_BudgetItems.SelectedItems == null || this.radDropDownList_BudgetItems.SelectedItems.Count == 0)
            {
                result = false;
                this.radDropDownList_BudgetItems.BackColor = Color.Red;
            }

            if (this.radDropDownList_Projects.SelectedItems == null || this.radDropDownList_Projects.SelectedItems.Count == 0)
            {
                result = false;
                this.radDropDownList_Projects.BackColor = Color.Red;
            }

            return result;
        }
        private void radButton_Add_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.AddSummary(summary);

            this.Close();
        }

        private void radButton_Update_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.UpdateSummary(summary);

            this.Close();
        }

        void SetData()
        {
            summary.CashFlowType = this.radCheckBox_CashFlowType.Checked;
            summary.SummaryDate = this.radDateTimePicker_SummaryDate.Value;
            summary.Accountable = (Accountable)this.radDropDownList_Accountables.SelectedItem.DataBoundItem;
            summary.Project = (Project)this.radDropDownList_Projects.SelectedItem.DataBoundItem;
            summary.BudgetItem = (BudgetItem)this.radDropDownList_BudgetItems.SelectedItem.DataBoundItem;
            summary.SummaUAH = double.Parse(this.radMaskedEditBox_SummaUAH.Value.ToString());
            if (this.radTextBox_Description.Text != null)
            {
                summary.Description = this.radTextBox_Description.Text;
            }
        }

        private void radTextBox_Click(object sender, EventArgs e)
        {
            ((RadTextBox)sender).BackColor = Color.White;
        }

        private void radCheckBox_ReadOnly_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для установки значения \"Только чтения\" обратитесь к администратору.");
        }

        private void radDropDownList_Click(object sender, EventArgs e)
        {
            ((RadDropDownList)sender).BackColor = Color.White;
        }

        private void radMaskedEditBox_SummaUAH_Click(object sender, EventArgs e)
        {
            ((RadMaskedEditBox)sender).BackColor = Color.White;
        }

        private void radButton_Remove_Click(object sender, EventArgs e)
        {
           var ansver =  MessageBox.Show("После удаления востановление невозможно. Вы действительно хотите удалить операцию?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (ansver == DialogResult.No)
                return;

            this.businessLogic.RemoveSummary(summary);

            this.Close();
        }
    }
}
