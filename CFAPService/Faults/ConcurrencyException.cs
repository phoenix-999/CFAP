using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.Entity.Infrastructure;

namespace CFAPService.Faults
{
    [DataContract]
    public class ConcurrencyException<T> where T: class
    {
        [DataMember]
        public T DatabaseValue { get; set; }

        [DataMember]
        public T CurrentValue { get; set; }

        [DataMember]
        public virtual string Message { get; set; }

        public ConcurrencyException(DbUpdateConcurrencyException ex)
        {
            var entry =  ex.Entries.Single();

            this.CurrentValue = (T)entry.CurrentValues.ToObject();

            this.DatabaseValue = (T)entry.GetDatabaseValues().ToObject();

            this.Message = ex.Message;
        }
    }
}
