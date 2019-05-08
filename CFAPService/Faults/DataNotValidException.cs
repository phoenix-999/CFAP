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
    }
}
