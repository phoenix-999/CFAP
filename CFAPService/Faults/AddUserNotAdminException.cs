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
    public class AddUserNotAdminException : Exception
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public AddUserNotAdminException(User user)
        {
            Log.Error(string.Format("Попытка добавить нового пользователя не админом для пользователя {0}", user.UserName));
        }
    }
}
