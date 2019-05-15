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
    public class DescriptionItem
    {

        public DescriptionItem()
        {
            this.UserGroups = new HashSet<UserGroup>();
            this.Summaries = new List<Summary>();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }


        [DataMember]
        public virtual ICollection<UserGroup> UserGroups { get; set; }


        //Не сериализуеться для предотвращения возникновения цыклической сериализации
        public virtual ICollection<Summary> Summaries { get; set; }
    }
}
