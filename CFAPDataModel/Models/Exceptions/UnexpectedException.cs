using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CFAPDataModel.Models.Exceptions
{
    [DataContract]
    class UnexpectedException
    {
        public UnexpectedException(Exception ex)
        {
            this.Message = ex.Message;
        }

        public virtual string Message { get; set; }
    }
}
