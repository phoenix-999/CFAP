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
    class DataNotValidException : Exception
    {

        public DataNotValidException(IDictionary<string, string> validationErrors)
        {
            this.ValidationErrors = validationErrors;
        }

        [DataMember]
        public IDictionary<string, string> ValidationErrors { get; private set; }
    }
}
