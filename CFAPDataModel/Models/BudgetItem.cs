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
    public class BudgetItem
    {

        public BudgetItem()
        {
            this.Projects = new HashSet<Project>();
            this.Users = new HashSet<User>();
            this.Accountables = new HashSet<Accountable>();
            this.Descriptions = new HashSet<DescriptionItem>();
            this.Summaries = new HashSet<Summary>();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string ItemName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }

        [DataMember]
        public virtual ICollection<Project> Projects { get; set; }

        [DataMember]
        public virtual ICollection<User> Users { get; set; }

        [DataMember]
        public virtual ICollection<Accountable> Accountables { get; set; }

        [DataMember]
        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        [DataMember]
        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
