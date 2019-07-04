using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CFAPDataModel.Models.Exceptions
{
    [DataContract]
    public class FiledDeletedException
    {
        public FiledDeletedException(NullReferenceException ex)
        {
            this.Message = "Поле не было найдено. Возможно оно было удалено другим пользователем после получения Вами данных.";
        }

        [DataMember]
        public virtual string Message { get; set; }
    }
}
