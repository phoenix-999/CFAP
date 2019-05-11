using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using NLog;

namespace CFAPService.Faults
{
    [DataContract]
    class DbException
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private Exception exception;
        public DbException() { }
        public DbException(Exception ex)
        {
            exception = ex;
            while(exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            Log.Error(ex.ToString());
        }

        [DataMember]
        public virtual string Message
        {
            get { return "Ошибка в работе с базой данных. Обратитесь к администратору. \n" + exception.Message; }
            set { } //Добавлено для возможности восстановления данных после при демарашлинге. Равносильно сеттеру свойства по уммолчанию
        }
    }
}
