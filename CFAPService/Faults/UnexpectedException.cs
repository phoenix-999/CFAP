using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CFAPService.Faults
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
