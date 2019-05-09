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
            this.Projects = new HashSet<Project>();
            this.Users = new HashSet<User>();
            this.Accountables = new HashSet<Accountable>();
            this.Descriptions = new HashSet<DescriptionItem>();
            this.BudgetItems = new HashSet<BudgetItem>();

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
        public virtual ICollection<Project> Projects { get; set; }

        [DataMember]
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        [DataMember]
        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        [DataMember]
        public virtual ICollection<Accountable> Accountables { get; set; }

        [DataMember]
        public virtual ICollection<User> Users { get; set; }

        //public virtual Rate Rate { get; set; }
        #endregion
    }
}
