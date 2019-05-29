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
    public class Rate
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [Column(TypeName = "datetime2")]
        public DateTime DateRate { get; set; }

        [DataMember]
        public double RateUSD { get; set; }

        [Timestamp]
        [DataMember]
        public byte[] RowVersion { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }
    }
}
