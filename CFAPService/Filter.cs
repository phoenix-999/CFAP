using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CFAPDataModel.Models;

namespace CFAPService
{
    [DataContract]
    public class Filter
    {
        [DataMember]
        public DateTime? DateStart { get; set; }

        [DataMember]
        public DateTime? DateEnd { get; set; }

        [DataMember]
        public ICollection<Project> Projects { get; set; }

        [DataMember]
        public ICollection<Accountable> Accountables { get; set; }

        [DataMember]
        public ICollection<BudgetItem> BudgetItems { get; set; }

    }
}
