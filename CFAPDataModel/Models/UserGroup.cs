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
            this.Users = new List<User>();
            this.Summaries = new List<Summary>();
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string GroupName { get; set; }

        //Не сериализуеться для предотвращения возникновения цыклической сериализации
        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Accountable> Accountables { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        [DataMember]
        public bool CanReadAllData { get; set; }

        //Не сериализуеться для предотвращения возникновения цыклической сериализации
        public virtual ICollection<Summary> Summaries { get; set; }

        public override string ToString()
        {
            return this.GroupName;
        }
    }
}
