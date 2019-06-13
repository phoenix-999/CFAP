using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CFAP.DataProviderClient;
using System.Globalization;
using System.Linq;
using System.Reflection;

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

            InitializeData();

        }

        private void InitializeData()
        {
            ///<summary>
            ///Инициализирует сетку представления и данные по остаткам
            /// </summary>
            InitializeGrid();
            InitializeBalance();
        }

        private void InitializeBalance()
        {
            ///<summary>
            ///Устанавливает текстовое представление данных остатков на начало периода, которые были предаврительно загружены или созданы экземпляры по уммолчанию.
            /// </summary>
            if (CFAPBusinessLogic.BalanceBeginningPeriod == null || CFAPBusinessLogic.Summaries == null)
                return;


            this.radLabel_BeginPeriodBalanceUAH.Text = string.Format(CultureInfo.CreateSpecificCulture("uk-UU"), "{0:C}", CFAPBusinessLogic.BalanceBeginningPeriod.BalanceUAH);
            this.radLabel_BeginPeriodBalanceUSD.Text = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C}", CFAPBusinessLogic.BalanceBeginningPeriod.BalanceUSD);

            this.AddOrUpdateCurrentBalance();
        }

        private void AddOrUpdateCurrentBalance()
        {
            ///<summary>
            ///Устанавливает текстовое представление данных о остатках по загруженным операциям.
            ///Если данные о отстаткам на начло периода не были загружены с сервера - создает экземляр остатков на начало периода с данными по уммолчанию.
            ///Требует предварительно созданного экземпляра коллекции операций.
            /// </summary>
            if (CFAPBusinessLogic.Summaries == null)
                return;

            if (CFAPBusinessLogic.BalanceBeginningPeriod == null)
            {
                CFAPBusinessLogic.BalanceBeginningPeriod = new Balance() { BalanceUAH = 0, BalanceUSD = 0};
            }

            double endPeriodBalanceUAH = CFAPBusinessLogic.BalanceBeginningPeriod.BalanceUAH + CFAPBusinessLogic.Incomming.BalanceUAH - CFAPBusinessLogic.Expense.BalanceUAH;
            this.radLabel_EndPeriodBalanceUAH.Text = string.Format(CultureInfo.CreateSpecificCulture("uk-UA"), "{0:C}", endPeriodBalanceUAH);

            double endPeriodBalanceUSD = CFAPBusinessLogic.BalanceBeginningPeriod.BalanceUSD + CFAPBusinessLogic.Incomming.BalanceUSD - CFAPBusinessLogic.Expense.BalanceUSD;
            this.radLabel_EndPeriodBalanceUSD.Text = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C}", endPeriodBalanceUSD);

            this.radLabel_CurrentExpenseUAH.Text = string.Format(CultureInfo.CreateSpecificCulture("uk-UA"), "{0:C}", CFAPBusinessLogic.Expense.BalanceUAH);
            this.radLabel_CurrentExpenseUSD.Text = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C}", CFAPBusinessLogic.Expense.BalanceUSD);

            this.radLabel_CurrentIncommingUAH.Text = string.Format(CultureInfo.CreateSpecificCulture("uk-UA"), "{0:C}", CFAPBusinessLogic.Incomming.BalanceUAH);
            this.radLabel_CurrentIncommingUSD.Text = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:C}", CFAPBusinessLogic.Incomming.BalanceUSD);
        }

        private void InitializeGrid()
        {
            ///<summary>
            ///Инициализирует поля сетки.
            ///Если коллекция операций в экземпляре формы не инициализированна - действия не происходят.
            /// </summary>
            if (this.Summaries == null)
                return;

            this.radGridView.DataSource = this.Summaries;

            this.radGridView.Columns["Id"].IsVisible = false;
            this.radGridView.Columns["RowVersion"].IsVisible = false;
            this.radGridView.Columns["ActionDate"].IsVisible = false;
            this.radGridView.Columns["CurrentRateUSD"].IsVisible = false;
            this.radGridView.Columns["UserGroups"].IsVisible = false;
            this.radGridView.Columns["UserLastChanged"].IsVisible = false;

            //Упорядчивание столбцов
            this.radGridView.Columns.Move(this.radGridView.Columns["SummaryDate"].Index, 0);
            this.radGridView.Columns.Move(this.radGridView.Columns["CashFlowType"].Index, 1);
            this.radGridView.Columns.Move(this.radGridView.Columns["Project"].Index, 2);
            this.radGridView.Columns.Move(this.radGridView.Columns["Accountable"].Index, 3);
            this.radGridView.Columns.Move(this.radGridView.Columns["BudgetItem"].Index, 4);
            this.radGridView.Columns.Move(this.radGridView.Columns["SummaUAH"].Index, 5);
            this.radGridView.Columns.Move(this.radGridView.Columns["SummaUSD"].Index, 6);
            this.radGridView.Columns.Move(this.radGridView.Columns["Description"].Index, 7);
            this.radGridView.Columns.Move(this.radGridView.Columns["ReadOnly"].Index, 8);

            //Настройка данных столбцов
            //Явное указание свойства привязки. Необходимо для корректной работы сортировки и группировки ассоциированных обьктов.
            this.radGridView.Columns["Accountable"].HeaderText = "Подотчетник";
            this.radGridView.Columns["Accountable"].FieldName = "Accountable.AccountableName";

            this.radGridView.Columns["BudgetItem"].HeaderText = "Статья";
            this.radGridView.Columns["BudgetItem"].FieldName = "BudgetItem.ItemName";

            this.radGridView.Columns["CashFlowType"].HeaderText = "Приход/Расход";

            this.radGridView.Columns["Description"].HeaderText = "Расшифровка";

            this.radGridView.Columns["Project"].HeaderText = "Проект";
            this.radGridView.Columns["Project"].FieldName = "Project.ProjectName";

            this.radGridView.Columns["ReadOnly"].HeaderText = "Только чтение";

            this.radGridView.Columns["SummaUAH"].HeaderText = "Сумма, грн.";
            this.radGridView.Columns["SummaUAH"].FormatString = "{0:C}";
            this.radGridView.Columns["SummaUSD"].FormatInfo = CultureInfo.CreateSpecificCulture("uk-UA");

            this.radGridView.Columns["SummaUSD"].HeaderText = "Сумма, $";
            this.radGridView.Columns["SummaUSD"].FormatString = "{0:C}";
            this.radGridView.Columns["SummaUSD"].FormatInfo = CultureInfo.CreateSpecificCulture("en-US");

            this.radGridView.Columns["SummaryDate"].HeaderText = "Дата";
            this.radGridView.Columns["SummaryDate"].FormatString = "{0:d}";
            this.radGridView.Columns["SummaryDate"].TextAlignment = ContentAlignment.MiddleRight;

            
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
            ///<summary>
            ///При возниковении события происходит загрузка колекции операций из сервера.
            ///Инициализируеться экземпляр коллекции операций формы.
            /// </summary>
            this.businessLogic.LoadSummaries(this.Filter);
            this.Summaries = new BindingList<Summary>(CFAPBusinessLogic.Summaries);
            InitializeData();
        }

        private void radMenuItem_Settings_Click(object sender, EventArgs e)
        {
            if (this.Filter == null)
                this.Filter = new DataProviderClient.Filter();

            new SettingsForm(this.Filter).ShowDialog();            
        }

        private void radButton_Add_Click(object sender, EventArgs e)
        {
            ///<summary>
            ///Вызывает форму добавления новой записи.
            //////После закрытия вызванной формы обновляет данные.
            ///Если экземпляр коллекции операций не создан - создает экземпляр коллекции и инициализирует сетку представления.
            /// </summary>
            if (CFAPBusinessLogic.Summaries == null)
            {
                CFAPBusinessLogic.Summaries = new List<Summary>();
            }

            if (this.Summaries == null)
            {
                this.Summaries = new BindingList<Summary>(CFAPBusinessLogic.Summaries);
                InitializeData();
            }

            new ChangeSummaryForm(new Summary(), ChangeDataOptions.AddNew).ShowDialog();
            RefreshData();
        }

        private void radGridView_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            ///<summary>
            ///Вызывает форму изменения данных операции.
            ///После закрытия формы обновляет данные.
            /// </summary>
            if (this.Summaries == null)
                return;

            if (e.Row.DataBoundItem is Summary)
            {
                Summary summaryToChange = (Summary)e.Row.DataBoundItem;
                new ChangeSummaryForm(summaryToChange, ChangeDataOptions.ChangeData).ShowDialog();
                RefreshData();
            }
        }
        
        private void RefreshData()
        {
            ///<summary>
            ///Обновляет данные привязки и данные остатков загруженных операций
            /// </summary>
            this.Summaries.ResetBindings();
            this.AddOrUpdateCurrentBalance();
        }

    }
}
