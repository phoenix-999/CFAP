using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CFAPDataModel.Models
{
    [DataContract]
    public class UserGroup
    {
        public UserGroup()
        {
            this.Accountables = new HashSet<Accountable>();
            this.Users = new HashSet<User>();
            this.Projects = new HashSet<Project>();
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Descriptions = new HashSet<DescriptionItem>();
            this.Summaries = new HashSet<Summary>();
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string GroupName { get; set; }

        [DataMember]
        public virtual ICollection<User> Users { get; set; }

        [DataMember]
        public virtual ICollection<Project> Projects { get; set; }

        [DataMember]
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        [DataMember]
        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        [DataMember]
        public virtual ICollection<Accountable> Accountables { get; set; }

        //Не сериализуеться для предотвращения возникновения цыклической сериализации
        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
