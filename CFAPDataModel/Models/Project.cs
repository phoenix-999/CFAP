using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CFAPDataModel.Models
{
    [DataContract]
    public class Project
    {
        public Project()
        {
            this.UserGroups = new HashSet<UserGroup>();
            this.Accountables = new HashSet<Accountable>();
            this.Descriptions = new HashSet<DescriptionItem>();
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Summaries = new HashSet<Summary>();
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string ProjectName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }

        [DataMember]
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        [DataMember]
        public virtual ICollection<Accountable> Accountables { get; set; }

        [DataMember]
        public virtual ICollection<DescriptionItem>Descriptions{ get; set; }

        [DataMember]
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        [DataMember]
        public virtual ICollection<Summary> Summaries { get; set; }

    }
}
