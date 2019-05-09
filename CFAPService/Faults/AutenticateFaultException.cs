using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CFAPDataModel.Models;
using NLog;



namespace CFAPService.Faults
{
    [DataContract]
    class AutenticateFaultException
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public AutenticateFaultException()
        {

        }
        public AutenticateFaultException(User user)
        {
            Log.Error(string.Format("Ошибка аутентификации для пользователя {0}", user.Id) );
        }

        [DataMember]
        public virtual string Message
        {
            get { return "Ошибка аутетификации. Пользователь не найден."; }
        }

    }
}
