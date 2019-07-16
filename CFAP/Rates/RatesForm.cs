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
    public partial class RatesForm : Telerik.WinControls.UI.RadForm
    {
        BindingList<Rate> Rates { get; set; }
        CFAPBusinessLogic businessLogic;
        public RatesForm()
        {
            InitializeComponent();          

            this.Rates = new BindingList<Rate>(CFAPBusinessLogic.Rates);
            businessLogic = new CFAPBusinessLogic(new ExceptionsHandlerUI());

            InitializeDataGrid();
        }

        void InitializeDataGrid()
        {
            this.radGridView.DataSource = this.Rates;

            InitializeColumns();
        }

        void InitializeColumns()
        {
            this.radGridView.Columns["RowVersion"].IsVisible = false;

            this.radGridView.Columns["Id"].IsVisible = false;

            this.radGridView.Columns["ReadOnly"].HeaderText = "Только чтения";
            this.radGridView.Columns["ReadOnly"].ReadOnly = true;

            this.radGridView.Columns["RateUSD"].HeaderText = "Курс долара";
            this.radGridView.Columns["EuroToDollarRate"].HeaderText = "Курс евро к долару";

            this.radGridView.Columns["DateRate"].HeaderText = "Месяц";
            this.radGridView.Columns["DateRate"].FormatString = "{0:MM/yyyy}";
        }


        private void radButton_AddItem_Click(object sender, EventArgs e)
        {
            new ChangeRateForm(new Rate(), ChangeDataOptions.AddNew).ShowDialog();
            this.Rates.ResetBindings();
        }

        private void radGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem is Rate)
            {
                Rate rateToChange = (Rate)e.Row.DataBoundItem;
                new ChangeRateForm(rateToChange, ChangeDataOptions.ChangeData).ShowDialog();
                this.Rates.ResetBindings();
            }
        }
    }
}
