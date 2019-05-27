using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using NLog;
using CFAPDataModel.Models;

namespace CFAPService.Faults
{
    [DataContract]
    public class NoRightsToChangeDataException
    {

        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public NoRightsToChangeDataException(User user, string entityName)
        {
            this.UserId = user.Id;
            this.UserName = user.UserName;
            this.EntityName = entityName;
            Log.Error(string.Format("Попытка добавления или изменения данных не админом. Для пользователя {0}, сущность {1}", user.UserName, entityName));
        }

        [DataMember]
        public string EntityName { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public virtual string Message
        {
            get { return string.Format("Пользователь {0} не обладает необходимыми правами для добавления новых или изменение данных существующих записей.", UserName); }
            set { } //Добавлено для возможности восстановления данных после при демарашлинге. Равносильно сеттеру свойства по уммолчанию
        }
    }
}
