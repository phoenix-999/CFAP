using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CFAP.DataProviderClient;
using System.Collections.Specialized;
using Telerik.WinControls.UI;

namespace CFAP
{
    public partial class BudgetItemsForm : Telerik.WinControls.UI.RadForm
    {
        BindingList<BudgetItem> BudgetItems { get; set; }
        public BudgetItemsForm()
        {
            InitializeComponent();          

            this.BudgetItems = new BindingList<BudgetItem>(CFAPBusinessLogic.BudgetItems);

            InitializeDataGrid();
        }

        void InitializeDataGrid()
        {
            this.radGridView.DataSource = this.BudgetItems;

            InitializeColumns();
        }

        void InitializeColumns()
        {
            this.radGridView.Columns["UserGroups"].IsVisible = false;
            this.radGridView.Columns["RowVersion"].IsVisible = false;

            this.radGridView.Columns["Id"].ReadOnly = true;

            this.radGridView.Columns["ReadOnly"].HeaderText = "Только чтения";
            this.radGridView.Columns["ReadOnly"].ReadOnly = true;

            this.radGridView.Columns["ItemName"].HeaderText = "Бюджетная статья";
        }


        private void radButton_AddItem_Click(object sender, EventArgs e)
        {
            new ChangeBudgetItemForm(new BudgetItem(), ChangeDataOptions.AddNew).ShowDialog();
            this.BudgetItems.ResetBindings();
        }

        private void radGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem is BudgetItem)
            {
                BudgetItem budgetItemToUpdate = (BudgetItem)e.Row.DataBoundItem;
                new ChangeBudgetItemForm(budgetItemToUpdate, ChangeDataOptions.ChangeData).ShowDialog();
                this.BudgetItems.ResetBindings();
            }
        }
    }
}
