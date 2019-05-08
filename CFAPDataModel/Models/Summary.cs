using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace CFAPDataModel.Models
{
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
            this.SummaDolar = this.SummaGrn * GetRate();
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
        public int Id { get; set; }

        public double SummaGrn { get; set; }

        public double SummaDolar{ get; set; }

        public bool CashFlowType { get; set; }

        public DateTime ActionDate { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }

        public virtual ICollection<DescriptionItem> Descriptions { get; set; }

        public virtual ICollection<Accountable> Accountables { get; set; }

        public virtual ICollection<User> Users { get; set; }

        //public virtual Rate Rate { get; set; }
        #endregion
    }
}
