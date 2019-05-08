using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CFAPDataModel.Models
{
    public class Rate
    {
        public int Id { get; set; }

        public DateTime DateRate { get; set; }

        public double Dolar { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
