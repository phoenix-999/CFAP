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

        public IEnumerable<int> GetProjectsId()
        {
            return (from p in this.Projects
                    select p.Id).ToArray();
        }

        public IEnumerable<int> GetAccountableId()
        {
            return (from a in this.Accountables
                    select a.Id).ToArray();
        }

        public IEnumerable<int> GetBudgetItemsId()
        {
            return (from i in this.BudgetItems
                    select i.Id).ToArray();
        }

    }
}
