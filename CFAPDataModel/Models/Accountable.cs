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
    public class Accountable
    {

        public Accountable()
        {
            this.Projects = new HashSet<Project>();
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Descriptions = new HashSet<DescriptionItem>();
            this.Users = new HashSet<User>();
            this.Summaries = new HashSet<Summary>();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string AccountableName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }

        [DataMember]
        public virtual ICollection<Project> Projects { get; set; }

        [DataMember]
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        [DataMember]
        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        [DataMember]
        public virtual ICollection<User> Users { get; set; }

        [DataMember]
        public virtual ICollection<Summary> Summaries { get; set; }

    }
}
