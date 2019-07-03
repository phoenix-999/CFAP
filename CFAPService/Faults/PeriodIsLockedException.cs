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
    public class PeriodIsLockedException
    {
        public PeriodIsLockedException(DateTime period)
        {
            Message = string.Format("Период {0} заблокирован", period.ToShortDateString());
        }

        [DataMember]
        public virtual string Message { get; set; }
    }
}
