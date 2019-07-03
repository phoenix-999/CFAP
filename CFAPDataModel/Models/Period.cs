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
    public class Period
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int Month { get; set; }

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }
    }
}
