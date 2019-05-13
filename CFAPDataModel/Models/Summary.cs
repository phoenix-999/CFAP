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
    public class Summary
    {
        
        public Summary()
        {
            this.UserGroups = new HashSet<UserGroup>();
            this.SetDefaultValues();   
        }


        private void SetDefaultValues()
        {
            this.ActionDate = DateTime.Now;
        }

        public void SetSummaDollar()
        {
            try
            {
                double rate = GetRate();
                if (rate > 0)
                    this.SummaDolar = this.SummaGrn / rate;
            }
            catch (DivideByZeroException) 
            {
                //Возникновения исключения маловероятно по причине неточности обработки чисел с плавающей точкой.
                //Более вероятно ошибка переполнения из-за слишком маленького значения.
                this.SummaDolar = double.Epsilon;
            }
            catch (OverflowException)
            {
                this.SummaDolar = double.Epsilon;
            }
        }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        private double GetRate()
        {
            double result = 0;

            using (CFAPContext ctx = new CFAPContext())
            {
                double? query = (from rate in ctx.Rates
                            where rate.DateRate.Month == this.ActionDate.Month && rate.DateRate.Year == this.ActionDate.Year
                            select rate.Dolar).FirstOrDefault();
                if (query != null)
                {
                    result = (double)query;
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            bool result = false;

            Summary otherProject = obj as Summary;

            if (otherProject == null)
                return false;

            result = this.Id == otherProject.Id;

            return result;
        }

        #region Properies
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public double SummaGrn { get; set; }

        [DataMember]
        public double SummaDolar{ get; set; }

        [DataMember]
        public bool CashFlowType { get; set; }

        [DataMember]
        public DateTime ActionDate { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }

        [DataMember]
        public virtual Project Project { get; set; }

        [DataMember]
        public virtual BudgetItem BudgetItem { get; set; }

        [DataMember]
        public virtual DescriptionItem Description { get; set; }

        [DataMember]
        public virtual Accountable Accountable { get; set; }

        [DataMember]
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        #endregion
    }
}
