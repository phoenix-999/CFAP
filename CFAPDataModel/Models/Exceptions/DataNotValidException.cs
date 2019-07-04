using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using NLog;
using System.Data.Entity.Validation;

namespace CFAPDataModel.Models.Exceptions
{
    [DataContract]
    public class DataNotValidException
    {

        //Логирование не производится умышленно по причине низкой важности информации исключения.
        public DataNotValidException() { }
        public DataNotValidException(IDictionary<string, string> validationErrors)
        {
            this.ValidationErrors = validationErrors;
        }

        public DataNotValidException(IEnumerable<DbEntityValidationResult> validationresults)
        {
            this.ValidationErrors = new Dictionary<string, string>();
            foreach (var res in validationresults)
            {
                foreach (var er in res.ValidationErrors)
                {
                    this.ValidationErrors.Add(er.PropertyName, er.ErrorMessage);
                }
            }
        }

        public DataNotValidException(DbEntityValidationResult validationresults)
        {
            this.ValidationErrors = new Dictionary<string, string>();
            foreach (var er in validationresults.ValidationErrors)
            {
                this.ValidationErrors.Add(er.PropertyName, er.ErrorMessage);
            }
        }


        [DataMember]
        public IDictionary<string, string> ValidationErrors { get; private set; }
    }
}
