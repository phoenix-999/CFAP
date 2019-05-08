using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CFAPDataModel.Models;
using NLog;



namespace CFAPService.Faults
{
    [DataContract]
    class AutenticateFaultException : Exception
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public AutenticateFaultException(User user)
        {
            Log.Error(string.Format("Authentication error for User {0}", user.UserName) );
        }
        [DataMember]
        public override string Message
        {
            get { return "Authentication error"; }
        }
    }
}
