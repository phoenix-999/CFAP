using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Data.Entity.Infrastructure;
using CFAPDataModel.Models;
using CFAPDataModel;
using System.ServiceModel;

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

        public ConcurrencyException(DbUpdateConcurrencyException ex, T currentValue)
        {
            var entry =  ex.Entries.Single();

            //if (entry.State != System.Data.Entity.EntityState.Deleted)
            //    this.CurrentValue = (T)entry.CurrentValues.ToObject();

            this.CurrentValue = currentValue;

            this.DatabaseValue = (T)entry.GetDatabaseValues().ToObject();

            //Метод вызываеться для установки связей ассоциаций. Возможно, надо определить интерфйс, который будет указывать на необходимость обновления связей и применять его к разным сущностям без дублирования кода.
            CorrectSummaries();

            this.Message = ex.Message;
        }

        private void CorrectSummaries()
        {
            if (this.CurrentValue.GetType() != typeof(Summary))
            {
                return;
            }

            Summary dbSummary = (this.DatabaseValue as Summary);
            Summary currentSummary = (this.CurrentValue as Summary);

            dbSummary.UserGroups = currentSummary.UserGroups;
            

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                var findedSummary = (from s in ctx.Summaries where s.Id == dbSummary.Id select s).First();

                try
                {
                    Summary.LoadRelationships(ctx);

                    this.DatabaseValue = (findedSummary as T);
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }
        }
    }
}
