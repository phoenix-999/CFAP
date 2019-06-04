using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CFAP.DataProviderClient;
using System.Windows.Forms;

namespace CFAP
{
    class ExceptionsHandler
    {
        
        public static void AuthenticateFaultExceptionHandler(FaultException<AuthenticateFaultException> fault)
        {
            MessageBox.Show(fault.Detail.Message, "Ошибка аутентификации", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void DbExceptionHandler(FaultException<DbException> fault)
        {
            MessageBox.Show(fault.Detail.Message, "Ошибка в работе с базой данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ArgumentNullExceptionHandler(FaultException<ArgumentNullException> fault)
        {
            MessageBox.Show(fault.Detail.Message, "Ошибка аутентификации", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void FaultExceptionHandler(FaultException fault)
        {
            MessageBox.Show(fault.Message, "Неопознання ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void CommunicationExceptionHandler(CommunicationException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка связи с сервером", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void TimeOutExceptionExceptionHandler(TimeoutException ex)
        {
            MessageBox.Show(ex.Message, "Ошибка связи с сервером", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void NoRightsToChangeDataExceptionHandler(FaultException<NoRightsToChangeDataException> ex)
        {
            MessageBox.Show(ex.Message, "Ошибка в процессе изменения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
