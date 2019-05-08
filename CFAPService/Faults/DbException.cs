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
    class DbException : Exception
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public DbException(Exception ex)
        {
            Log.Error(ex.ToString());
        }
        [DataMember]
        public override string Message
        {
            get { return "Data base connection error";  }
        }
    }
}
