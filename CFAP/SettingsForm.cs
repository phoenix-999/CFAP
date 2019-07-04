using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CFAP.DataProviderClient;
using System.Linq;
using System.Reflection;

namespace CFAP
{
    public partial class SettingsForm : Telerik.WinControls.UI.RadForm
    {
        DataProviderClient.Filter filter;
        public SettingsForm(DataProviderClient.Filter filter)
        {
            InitializeComponent();

            this.filter = filter;

            InitializeForm();
            InitializeData();
        }

        void InitializeForm()
        {
            if (CFAPBusinessLogic.User.IsAdmin == false)
            {
                this.radPanel_ChangeReadOnly.Visible = false;
            }
        }

        void InitializeData()
        {
            InitializeLists();

            if (filter.DateStart != default(DateTime) && filter.DateStart != null)
                this.radDateTimePicker_DateStart.Value = (DateTime)filter.DateStart;

            if (filter.DateEnd != default(DateTime) && filter.DateEnd != null)
                this.radDateTimePicker_DateEnd.Value = (DateTime)filter.DateEnd;
        }

        void InitializeLists()
        {
            if (CFAPBusinessLogic.Accountables != null)
            {
                this.radCheckedDropDownList_Accountables.DataSource = CFAPBusinessLogic.Accountables;
                AttachChekedAccountable();
            }


            if (CFAPBusinessLogic.BudgetItems != null)
            {
                this.radCheckedDropDownList_BudgetItems.DataSource = CFAPBusinessLogic.BudgetItems;
                AttachChekedBudgetItems();
            }

            if (CFAPBusinessLogic.Projects != null)
            {
                this.radCheckedDropDownList_Projects.DataSource = CFAPBusinessLogic.Projects;
                AttachChekedProjects();
            }
                
        }

        void AttachChekedAccountable()
        {
            if (this.filter.Accountables == null || this.filter.Accountables.Length == 0)
                return;
                
            foreach (var item in this.radCheckedDropDownList_Accountables.Items)
            {
                Accountable accountable = (Accountable)item.DataBoundItem;
                var isCheckedItem = this.filter.Accountables.Where(a => a.Id == accountable.Id).FirstOrDefault() != null;
                if (!isCheckedItem)
                    continue;

                Type itemType = item.GetType();
                PropertyInfo checkedProperty = itemType.GetRuntimeProperty("Checked");
                checkedProperty.SetValue(item, true);
            } 
        }

        void AttachChekedProjects()
        {
            if (this.filter.Projects == null || this.filter.Projects.Length == 0)
                return;

            foreach (var item in this.radCheckedDropDownList_Projects.Items)
            {
                Project project = (Project)item.DataBoundItem;
                var isCheckedItem = this.filter.Projects.Where(p => p.Id == project.Id).FirstOrDefault() != null;
                if (!isCheckedItem)
                    continue;

                Type itemType = item.GetType();
                PropertyInfo checkedProperty = itemType.GetRuntimeProperty("Checked");
                checkedProperty.SetValue(item, true);
            }
        }

        void AttachChekedBudgetItems()
        {
            if (this.filter.BudgetItems == null || this.filter.BudgetItems.Length == 0)
                return;

            foreach (var item in this.radCheckedDropDownList_BudgetItems.Items)
            {
                BudgetItem budgetItem = (BudgetItem)item.DataBoundItem;
                var isCheckedItem = this.filter.BudgetItems.Where(i => i.Id == budgetItem.Id).FirstOrDefault() != null;
                if (!isCheckedItem)
                    continue;

                Type itemType = item.GetType();
                PropertyInfo checkedProperty = itemType.GetRuntimeProperty("Checked");
                checkedProperty.SetValue(item, true);
            }
        }

        private void radButton_Accept_Click(object sender, EventArgs e)
        {
            AcceptChanges();

            this.Close();
        }

        private void AcceptChanges()
        {
            filter.DateStart = this.radDateTimePicker_DateStart.Value;
            filter.DateEnd = this.radDateTimePicker_DateEnd.Value;

            if (radCheckedDropDownList_Accountables.CheckedItems != null && radCheckedDropDownList_Accountables.CheckedItems.Count > 0)
                filter.Accountables = (from a in radCheckedDropDownList_Accountables.CheckedItems select (Accountable)a.DataBoundItem).ToArray();
            else
                filter.Accountables = null;

            if (radCheckedDropDownList_BudgetItems.CheckedItems != null && radCheckedDropDownList_BudgetItems.CheckedItems.Count > 0)
                filter.BudgetItems = (from b in radCheckedDropDownList_BudgetItems.CheckedItems select (BudgetItem)b.DataBoundItem).ToArray();
            else
                filter.BudgetItems = null;

            if (radCheckedDropDownList_Projects.CheckedItems != null && radCheckedDropDownList_Projects.CheckedItems.Count > 0)
                filter.Projects = (from p in radCheckedDropDownList_Projects.CheckedItems select (Project)p.DataBoundItem).ToArray();
            else
                filter.Projects = null;
        }

        private void radButton_AcceptReadOnly_Click(object sender, EventArgs e)
        {
            AcceptChanges();

            bool onOff = this.radCheckBox_ReadOnly.Checked;

            CFAPBusinessLogic businessLogic = new CFAPBusinessLogic(new ExceptionsHandlerUI());
            businessLogic.ChangeReadOnlySummary(onOff, this.filter);

            MessageBox.Show("Изменения будут видны при следующей загрузке данных");
        }

        private void radButton_OpenLockedPeriodForm_Click(object sender, EventArgs e)
        {
            new LockedPeriodForm().ShowDialog();
        }
    }
}
