using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFAPDataModel.Models
{
    [DataContract]
    public class Project
    {
        public Project()
        {
            this.UserGroups = new HashSet<UserGroup>();
            this.Summaries = new List<Summary>();
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Required, MaxLength(70)]
        [Index(IsUnique = true)]
        public string ProjectName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }

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

            Project otherProject = obj as Project;

            if (otherProject == null)
                return false;

            result = this.Id == otherProject.Id;

            return result;
        }
    }
}
