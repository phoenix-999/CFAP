using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CFAPDataModel.Models;

namespace CFAPService
{
    [DataContract]
    public class Transport
    {
        [DataMember]
        public ICrudOperations Single { get; set; }

        [DataMember]
        public IEnumerable<ICrudOperations> Collection { get; set; }
    }
}
