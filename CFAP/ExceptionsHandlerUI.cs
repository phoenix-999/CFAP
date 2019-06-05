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
    }
}
