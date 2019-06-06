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
    public partial class AccountableForm : Telerik.WinControls.UI.RadForm
    {
        BindingList<Accountable> Accountables { get; set; }
        public AccountableForm()
        {
            InitializeComponent();          

            this.Accountables = new BindingList<Accountable>(CFAPBusinessLogic.Accountables);

            InitializeDataGrid();
        }

        void InitializeDataGrid()
        {
            this.radGridView.DataSource = this.Accountables;

            InitializeColumns();
        }

        void InitializeColumns()
        {
            this.radGridView.Columns["UserGroups"].IsVisible = false;
            this.radGridView.Columns["RowVersion"].IsVisible = false;

            this.radGridView.Columns["Id"].ReadOnly = true;

            this.radGridView.Columns["ReadOnly"].HeaderText = "Только чтения";
            this.radGridView.Columns["ReadOnly"].ReadOnly = true;

            this.radGridView.Columns["AccountableName"].HeaderText = "Подотчетное лицо";
        }


        private void radButton_AddItem_Click(object sender, EventArgs e)
        {
            new ChangeAccountableForm(new Accountable(), ChangeDataOptions.AddNew).ShowDialog();
            this.Accountables.ResetBindings();
        }

        private void radGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem is Accountable)
            {
                Accountable accountableToChange = (Accountable)e.Row.DataBoundItem;
                new ChangeAccountableForm(accountableToChange, ChangeDataOptions.ChangeData).ShowDialog();
                this.Accountables.ResetBindings();
            }
        }
    }
}
