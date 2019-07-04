using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CFAPDataModel.Models;

namespace CFAPDataModel.Models.Exceptions
{
    [DataContract]
    public class UserHasNotGroupsException
    {
        [DataMember]
        private User User { get; set; }
        public UserHasNotGroupsException(User user)
        {
            this.User = user;
        }

        [DataMember]
        public virtual string Message
        {
            get {   return string.Format("Пользователь {0} не прикреплен ни к одной группе.", this.User.UserName); }
            set { }
        }
    }
}
