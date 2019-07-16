using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CFAPDataModel.Models;
using NLog;



namespace CFAPDataModel.Models.Exceptions
{
    [DataContract]
    public class AuthenticateFaultException
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public AuthenticateFaultException() { }
        public AuthenticateFaultException(User user)
        {
            Log.Error(string.Format("Ошибка аутентификации для пользователя {0}", user.UserName) );
        }

        [DataMember]
        public virtual string Message
        {
            get { return "Ошибка аутентификации. Пользователь не найден или не обладает необходимыми правами."; }
            set { } //Добавлено для возможности восстановления данных после при демарашлинге. Равносильно сеттеру свойства по уммолчанию
        }

    }
}
