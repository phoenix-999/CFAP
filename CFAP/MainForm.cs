﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CFAP.DataProviderClient;
using System.Globalization;

namespace CFAP
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        CFAPBusinessLogic businessLogic;

        public CFAP.DataProviderClient.Filter Filter { get; set; }

        BindingList<Summary> Summaries;
        public MainForm()
        {
            InitializeComponent();

            if (CFAPBusinessLogic.User == null)
            {
                Application.Exit();
            }

            businessLogic = new CFAPBusinessLogic(new ExceptionsHandlerUI());

            InitializeRadMenu();

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            if (this.Summaries == null)
                return;

            this.radGridView.DataSource = this.Summaries;

            this.radGridView.Columns["Accountable"].HeaderText = "Подотчетник";
            //Явное указание свойства привязки. Необходимо для корректной работы сортировки и группировки ассоциированных обьктов.
            this.radGridView.Columns["Accountable"].FieldName = "Accountable.AccountableName";

            this.radGridView.Columns["ActionDate"].IsVisible = false;

            this.radGridView.Columns["BudgetItem"].HeaderText = "Статья";
            this.radGridView.Columns["BudgetItem"].FieldName = "BudgetItem.ItemName";

            this.radGridView.Columns["CashFlowType"].HeaderText = "Приход/Расход";

            this.radGridView.Columns["CurrentRateUSD"].IsVisible = false;

            this.radGridView.Columns["Description"].HeaderText = "Расшифровка";

            this.radGridView.Columns["Id"].IsVisible = false;

            this.radGridView.Columns["Project"].HeaderText = "Проект";            
            this.radGridView.Columns["Project"].FieldName = "Project.ProjectName";


            this.radGridView.Columns["ReadOnly"].HeaderText = "Только чтение";

            this.radGridView.Columns["RowVersion"].IsVisible = false;

            this.radGridView.Columns["SummaUAH"].HeaderText = "Сумма, грн.";
            this.radGridView.Columns["SummaUAH"].FormatString = "{0:C}";
            this.radGridView.Columns["SummaUSD"].FormatInfo = CultureInfo.CreateSpecificCulture("uk-UA");

            this.radGridView.Columns["SummaUSD"].HeaderText = "Сумма, $";
            this.radGridView.Columns["SummaUSD"].FormatString = "{0:C}";
            this.radGridView.Columns["SummaUSD"].FormatInfo = CultureInfo.CreateSpecificCulture("en-US");

            this.radGridView.Columns["SummaryDate"].HeaderText = "Дата";
            this.radGridView.Columns["SummaryDate"].FormatString = "{0:d}";

            this.radGridView.Columns["UserGroups"].IsVisible = false;

            this.radGridView.Columns["UserLastChanged"].IsVisible = false;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        void InitializeRadMenu()
        {
            if (CFAPBusinessLogic.User.CanChangeUsersData == false)
            {
                this.radMenuItem_Users.Visibility = ElementVisibility.Hidden;
            }

            if (CFAPBusinessLogic.User.IsAdmin == false)
            {
                this.radMenuItem_Accountables.Visibility = ElementVisibility.Hidden;
                this.radMenuItem_BudgetItems.Visibility = ElementVisibility.Hidden;
                this.radMenuItem_Projects.Visibility = ElementVisibility.Hidden;
                this.radMenuItem_Rates.Visibility = ElementVisibility.Hidden;
                this.radMenuItem_UsersGroups.Visibility = ElementVisibility.Hidden;
            }
        }

        void InitializeMenuItemUsers()
        {

        }

        private void radMenuItem_Users_Click(object sender, EventArgs e)
        {
            if (CFAPBusinessLogic.UsersData == null)
            {
                MessageBox.Show("Данные о пользователях не были загружены.", "Ошибка получения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new UsersForm().Show();
        }

        private void radMenuItem_Accountables_Click(object sender, EventArgs e)
        {
            if (CFAPBusinessLogic.Accountables == null)
            {
                MessageBox.Show("Данные о подотчетных лицах не были загружены.", "Ошибка получения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new AccountableForm().Show();
        }

        private void radMenuItem_BudgetItems_Click(object sender, EventArgs e)
        {
            if (CFAPBusinessLogic.BudgetItems == null)
            {
                MessageBox.Show("Данные о бюджетных статьях не были загружены.", "Ошибка получения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new BudgetItemsForm().Show();
        }

        private void radMenuItem_Projects_Click(object sender, EventArgs e)
        {
            if (CFAPBusinessLogic.Projects == null)
            {
                MessageBox.Show("Данные о проектах не были загружены.", "Ошибка получения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new ProjectsForm().Show();
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            if (CFAPBusinessLogic.Rates == null)
            {
                MessageBox.Show("Данные о курсах валют не были загружены.", "Ошибка получения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new RatesForm().Show();
        }

        private void radMenuItem_UsersGroups_Click(object sender, EventArgs e)
        {
            if (CFAPBusinessLogic.UserGroups == null)
            {
                MessageBox.Show("Данные о группах пользователей не были загружены.", "Ошибка получения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new UserGroupsForm().Show();
        }

        private void radButton_GetData_Click(object sender, EventArgs e)
        {
            this.businessLogic.LoadSummaries(this.Filter);
            this.Summaries = new BindingList<Summary>(CFAPBusinessLogic.Summaries);
            InitializeGrid();
        }

        private void radMenuItem_Settings_Click(object sender, EventArgs e)
        {
            if (this.Filter == null)
                this.Filter = new DataProviderClient.Filter();

            new SettingsForm(this.Filter).ShowDialog();            
        }

        private void radButton_Add_Click(object sender, EventArgs e)
        {
            if (CFAPBusinessLogic.Summaries == null)
            {
                CFAPBusinessLogic.Summaries = new List<Summary>();
            }

            if (this.Summaries == null)
            {
                this.Summaries = new BindingList<Summary>(CFAPBusinessLogic.Summaries);
            }

            new ChangeSummaryForm(new Summary(), ChangeDataOptions.AddNew).ShowDialog();
            this.Summaries.ResetBindings();
        }

        private void radGridView_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (this.Summaries == null)
                return;

            if (e.Row.DataBoundItem is Summary)
            {
                Summary summaryToChange = (Summary)e.Row.DataBoundItem;
                new ChangeSummaryForm(summaryToChange, ChangeDataOptions.ChangeData).ShowDialog();
                this.Summaries.ResetBindings();
            }
        }
    }
}
