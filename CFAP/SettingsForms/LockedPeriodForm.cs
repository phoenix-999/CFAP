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

namespace CFAP
{
    public partial class LockedPeriodForm : Telerik.WinControls.UI.RadForm
    {
        CFAPBusinessLogic businessLogic;
        BindingList<Period> Periods;
        public LockedPeriodForm()
        {
            InitializeComponent();
            businessLogic = new CFAPBusinessLogic(new ExceptionsHandlerUI());
        }

        private void LockedPeriodForm_Load(object sender, EventArgs e)
        {
            businessLogic.LoadPeriods();

            if (CFAPBusinessLogic.Periods == null)
            {
                MessageBox.Show("Данные периодов не были получены", "Ошибка загрузки данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            this.Periods = new BindingList<Period>(CFAPBusinessLogic.Periods);
            InitializeDataGrid();
        }

        void InitializeDataGrid()
        {
            this.radGridView.DataSource = this.Periods;

            InitializeColumns();
        }

        void InitializeColumns()
        {
            this.radGridView.Columns["Year"].HeaderText = "Год";
            this.radGridView.Columns["Month"].HeaderText = "Месяц";
            this.radGridView.Columns["IsLocked"].HeaderText = "Блокировка";
        }


        private void radButton_AddItem_Click(object sender, EventArgs e)
        {
            //new ChangeProjectForm(new Project(), ChangeDataOptions.AddNew).ShowDialog();
            this.Periods.ResetBindings();
        }

        private void radGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem is Project)
            {
                Period periodToChange = (Period)e.Row.DataBoundItem;
                //new ChangeProjectForm(periodToChange, ChangeDataOptions.ChangeData).ShowDialog();
                this.Periods.ResetBindings();
            }
        }
    }
}
