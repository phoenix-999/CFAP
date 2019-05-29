using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;

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

        public void CustomValidate(CFAPContext ctx)
        {
            List<DbEntityValidationResult> validationResults = new List<DbEntityValidationResult>();

            var rateResult = ctx.Entry(this).GetValidationResult();
            if (!rateResult.IsValid) { validationResults.Add(rateResult); }

            if (this.DateRate == default(DateTime))
            {
                DbValidationError validationError = new DbValidationError(typeof(DateTime).ToString(), "Значениие не определено");
                DbEntityValidationResult dbEntityValidationResult = new DbEntityValidationResult(ctx.Entry(this), new DbValidationError[] { validationError });
                validationResults.Add(dbEntityValidationResult);
            }

            if (validationResults.Count > 0)
            {
                throw new DbEntityValidationException(
                    "Ошибка при проверке данных. Данные могут остутствовать или указаны не верно. Проверте внесенные данные и повторите попытку"
                    , validationResults);
            }
        }

    }
}
