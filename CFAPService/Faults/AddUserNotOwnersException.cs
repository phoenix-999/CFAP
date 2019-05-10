//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CFAPDataModel.Models;
//using System.Runtime.Serialization;

//namespace CFAPService.Faults
//{
//    [DataContract]
//    public class AddUserNotOwnersException
//    {
//        private User user;

//        public AddUserNotOwnersException()
//        {

//        }

//        public AddUserNotOwnersException(User user)
//        {
//            this.user = user;
//        }

//        [DataMember]
//        public virtual string Message
//        {
//            get { return string.Format("Пользователь {0} не имеет управляющих пользователей", user.UserName);  }
//            set { }
//        }
//    }
//}
