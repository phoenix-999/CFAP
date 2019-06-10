using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace CFAPDataModel.Models
{
    [DataContract]
    public class Balance
    {
        [DataMember]
        public double BalanceUAH { get; set; }

        [DataMember]
        public double BalanceUSD { get; set; }
    }
}
