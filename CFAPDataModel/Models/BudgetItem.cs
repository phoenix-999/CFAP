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
            this.UserGroups = new HashSet<UserGroup>();
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
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        [DataMember]
        public virtual ICollection<Accountable> Accountables { get; set; }

        [DataMember]
        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        //Не сериализуеться для предотвращения возникновения цыклической сериализации
        public virtual ICollection<Summary> Summaries { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            bool result = false;

            BudgetItem otherProject = obj as BudgetItem;

            if (otherProject == null)
                return false;

            result = this.Id == otherProject.Id;

            return result;
        }
    }
}
