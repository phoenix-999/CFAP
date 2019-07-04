using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using CFAPDataModel.Models.Exceptions;
using System.Data.Entity.Validation;
using System.ServiceModel;

namespace CFAPDataModel.Models
{
    [DataContract]
    public class Period:ICrudOperations
    {

        [DataMember]
        [Key]
        [Column(Order = 1)]
        public int Month { get; set; }

        [DataMember]
        [Key]
        [Column(Order = 2)]
        public int Year { get; set; }

        [DataMember]
        public bool IsLocked { get; set; }

        public ICrudOperations Add(DbConcurencyUpdateOptions concurencyUpdateOptions, User user)
        {
            concurencyUpdateOptions = DbConcurencyUpdateOptions.ClientPriority;

            if (user.IsAdmin == false)
                throw new FaultException<NoRightsToChangeDataException>(new NoRightsToChangeDataException(user, this.GetType().Name));

            bool isExists = false;

            try
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var existsPeriod = (from p in ctx.Periods where p.Month == this.Month && p.Year == this.Year select p).FirstOrDefault();
                    if (existsPeriod != null)
                        isExists = true;
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DbException>(new DbException(ex));
            }
            

            if (isExists)
                return Update(concurencyUpdateOptions, user);

            Period result = null;
            try
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;

                    ctx.Periods.Add(this);
                    ctx.SaveChanges(concurencyUpdateOptions);
                    result = (from p in ctx.Periods where p.Month == this.Month && p.Year == this.Year select p).Single();
                }
            }
            catch (DbEntityValidationException ex)
            {
                throw new FaultException<DataNotValidException>(new DataNotValidException(ex.EntityValidationErrors));
            }
            catch (Exception ex)
            {
                throw new FaultException<DbException>(new DbException(ex));
            }
            return result;

        }

        public ICrudOperations Delete(DbConcurencyUpdateOptions concurencyUpdateOptions, User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICrudOperations> Select(Filter filter, User user)
        {
            List<Period> result = new List<Period>();
            try
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    result = (from p in ctx.Periods select p).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                throw new FaultException<DbException>(new DbException(ex));
            }
            return result;
        }

        public ICrudOperations Update(DbConcurencyUpdateOptions concurencyUpdateOptions, User user)
        {
            if (user.IsAdmin == false)
                throw new FaultException<NoRightsToChangeDataException>(new NoRightsToChangeDataException(user, this.GetType().Name));

            concurencyUpdateOptions = DbConcurencyUpdateOptions.ClientPriority;

            Period result = null;
            try
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    ctx.Periods.Attach(this);
                    ctx.Entry(this).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges(concurencyUpdateOptions);
                    result = (from p in ctx.Periods where p.Month == this.Month && p.Year == this.Year select p).Single();
                }
            }
            catch (DbEntityValidationException ex)
            {
                throw new FaultException<DataNotValidException>(new DataNotValidException(ex.EntityValidationErrors));
            }
            catch (Exception ex)
            {
                throw new FaultException<DbException>(new DbException(ex));
            }
            return result;
        }
    }
}
