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
            this.UserGroups = new HashSet<UserGroup>();
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
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        //Не сериализуеться для предотвращения возникновения цыклической сериализации
        public virtual ICollection<Summary> Summaries { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            bool result = false;

            Accountable otherProject = obj as Accountable;

            if (otherProject == null)
                return false;

            result = this.Id == otherProject.Id;

            return result;
        }

    }
}
