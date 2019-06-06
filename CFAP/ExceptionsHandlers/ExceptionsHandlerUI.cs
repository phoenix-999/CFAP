using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CFAP.DataProviderClient;
using System.Transactions;


namespace CFAP
{
    class ExceptionsHandlerUI : ExceptionsHandler
    {
        public override void AuthenticateFaultExceptionHandler(FaultException<AuthenticateFaultException> fault)
        {
            MessageBox.Show(fault.Detail.Message, "Ошибка аутентификации", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void DbExceptionHandler(FaultException<DbException> fault)
        {
            MessageBox.Show(fault.Detail.Message, "Ошибка в работе с базой данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void ArgumentNullExceptionHandler(FaultException<ArgumentNullException> fault)
        {
            MessageBox.Show(fault.Detail.Message, "Ошибка аутентификации", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void FaultExceptionHandler(FaultException fault)
        {
            MessageBox.Show(fault.Message, "Неопознання ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void CommunicationExceptionHandler(CommunicationException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка связи с сервером", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void TimeOutExceptionExceptionHandler(TimeoutException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка связи с сервером", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void NoRightsToChangeDataExceptionHandler(FaultException<NoRightsToChangeDataException> ex)
        {
            MessageBox.Show(ex.Message, "Ошибка в процессе изменения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void UserHasNotGroupsExceptionHandler(FaultException<UserHasNotGroupsException> fault)
        {
            MessageBox.Show(fault.Detail.Message, "Ошибка в процессе изменения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void DataNotValidExceptionHandler(FaultException<DataNotValidException> fault)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var error in fault.Detail.ValidationErrors)
            {
                stringBuilder.Append(string.Format("Поле: {0}, значение: {1};\n", error.Key, error.Value)); 
            }

            MessageBox.Show(stringBuilder.ToString(), "Ошибка в процессе изменения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void TransactionAbortedExceptionHandler(TransactionAbortedException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка транзакции", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void TryChangeReadOnlyFieldExceptionHandler(FaultException<TryChangeReadOnlyFiledException> fault)
        {
            MessageBox.Show(fault.Message, "Ошибка изменения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public override void ConcurrencyExceptionAccountablesHandler(FaultException<ConcurrencyExceptionOfAccountabledxjYbbDT> fault)
        {
            var errorString = new StringBuilder();
            errorString.Append("Данные были изменены другим пользователем с момента загрузки\n");
            errorString.Append(string.Format("Поле: {0}, значение в БД: {1}, текущее значение: {2}\n\n", "Подотчетник", fault.Detail.DatabaseValue.AccountableName, fault.Detail.CurrentValue.AccountableName));
            errorString.Append("Изменить значение в базе данных?");
            var dialogResult = MessageBox.Show(errorString.ToString(), "Ошибка изменения данных", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            CFAPBusinessLogic businessLogic = new CFAPBusinessLogic(this);
            if (dialogResult == DialogResult.Yes)
            {
                businessLogic.UpdateAccountable(fault.Detail.CurrentValue, DbConcurencyUpdateOptions.ClientPriority);
            }
            else
            {
                businessLogic.UpdateAccountable(fault.Detail.DatabaseValue, DbConcurencyUpdateOptions.DatabasePriority);

            }
        }

        public override void ConcurrencyExceptionBudgetItemsHandler(FaultException<ConcurrencyExceptionOfBudgetItemdxjYbbDT> fault)
        {
            var errorString = new StringBuilder();
            errorString.Append("Данные были изменены другим пользователем с момента загрузки\n");
            errorString.Append(string.Format("Поле: {0}, значение в БД: {1}, текущее значение: {2}\n\n", "Бюджетная статья", fault.Detail.DatabaseValue.ItemName, fault.Detail.CurrentValue.ItemName));
            errorString.Append("Изменить значение в базе данных?");
            var dialogResult = MessageBox.Show(errorString.ToString(), "Ошибка изменения данных", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            CFAPBusinessLogic businessLogic = new CFAPBusinessLogic(this);
            if (dialogResult == DialogResult.Yes)
            {
                businessLogic.UpdateBudgetItem(fault.Detail.CurrentValue, DbConcurencyUpdateOptions.ClientPriority);
            }
            else
            {
                businessLogic.UpdateBudgetItem(fault.Detail.DatabaseValue, DbConcurencyUpdateOptions.DatabasePriority);

            }
        }

        public override void ConcurrencyExceptionProjectsHandler(FaultException<ConcurrencyExceptionOfProjectdxjYbbDT> fault)
        {
            var errorString = new StringBuilder();
            errorString.Append("Данные были изменены другим пользователем с момента загрузки\n");
            errorString.Append(string.Format("Поле: {0}, значение в БД: {1}, текущее значение: {2}\n\n", "Проект", fault.Detail.DatabaseValue.ProjectName, fault.Detail.CurrentValue.ProjectName));
            errorString.Append("Изменить значение в базе данных?");
            var dialogResult = MessageBox.Show(errorString.ToString(), "Ошибка изменения данных", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            CFAPBusinessLogic businessLogic = new CFAPBusinessLogic(this);
            if (dialogResult == DialogResult.Yes)
            {
                businessLogic.UpdateProject(fault.Detail.CurrentValue, DbConcurencyUpdateOptions.ClientPriority);
            }
            else
            {
                businessLogic.UpdateProject(fault.Detail.DatabaseValue, DbConcurencyUpdateOptions.DatabasePriority);

            }
        }

        public override void ConcurrencyExceptionRatesHandler(FaultException<ConcurrencyExceptionOfRatedxjYbbDT> fault)
        {
            var errorString = new StringBuilder();
            errorString.Append("Данные были изменены другим пользователем с момента загрузки\n");
            errorString.Append(string.Format("Поле: {0}, значение в БД: {1}, текущее значение: {2}\n\n", "Курс", fault.Detail.DatabaseValue.RateUSD, fault.Detail.CurrentValue.RateUSD));
            errorString.Append(string.Format("Поле: {0}, значение в БД: {1}, текущее значение: {2}\n\n", "Дата", fault.Detail.DatabaseValue.DateRate, fault.Detail.CurrentValue.DateRate));
            errorString.Append("Изменить значение в базе данных?");
            var dialogResult = MessageBox.Show(errorString.ToString(), "Ошибка изменения данных", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            CFAPBusinessLogic businessLogic = new CFAPBusinessLogic(this);
            if (dialogResult == DialogResult.Yes)
            {
                businessLogic.UpdateRate(fault.Detail.CurrentValue, DbConcurencyUpdateOptions.ClientPriority);
            }
            else
            {
                businessLogic.UpdateRate(fault.Detail.DatabaseValue, DbConcurencyUpdateOptions.DatabasePriority);

            }
        }

        public override void ConcurrencyExceptionSummariesHandler(FaultException<ConcurrencyExceptionOfSummarydxjYbbDT> fault)
        {
            throw new NotImplementedException();
        }
    }
}
