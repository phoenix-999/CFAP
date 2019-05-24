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
    public class NoRightsToChangeUserDataException
    {

        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public NoRightsToChangeUserDataException(User user)
        {
            this.UserId = user.Id;
            this.UserName = user.UserName;
            Log.Error(string.Format("Попытка добавить нового или изменить данные существующего пользователя не админом. Для пользователя {0}", user.UserName));
        }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public virtual string Message
        {
            get { return string.Format("Пользователь {0} не обладает необходимыми правами для добавления новых или изменение данных существующих пользователей.", UserName); }
            set { } //Добавлено для возможности восстановления данных после при демарашлинге. Равносильно сеттеру свойства по уммолчанию
        }
    }
}
