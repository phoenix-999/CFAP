using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CFAPDataModel.Models;

namespace CFAPService.Faults
{
    [DataContract]
    public class AccountableUserHasNotAccountableRefferenceException 
    {
        User user;

        public AccountableUserHasNotAccountableRefferenceException(User user)
        {
            this.user = user;
            this.Message = string.Format("Пользователь {0}, помечен как подотчетное лицо не имеет сопоставленного подотчетного лица. Изменения отменены.", user.UserName);
        }

        [DataMember]
        public virtual string Message { get; set; }
    }
}
