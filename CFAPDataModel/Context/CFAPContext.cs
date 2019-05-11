namespace CFAPDataModel
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.IO;
    using System.Data.Entity.Infrastructure.Interception;
    using System.Transactions;
    using System.Data.Entity.Infrastructure;
    using CFAPDataModel.Models;
    using System.Data.Common;

    public class CFAPContext : DbContext
    {
        static CFAPContext()
        {
            //DbInterception.Add(new NLogCommandInterceptor());
            //Database.SetInitializer<CFAPContext>(new DropCreateDatabaseAlways<CFAPContext>());
        }
        public CFAPContext()
            : base("name=CFAPContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public virtual DbSet<Accountable> Accountables { get; set; }
        public virtual DbSet<BudgetItem> BudgetItems { get; set; }
        public virtual DbSet<DescriptionItem> Descriptions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Summary> Summaries { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }


        private void CalculateSummary()
        {
            foreach (var summary in Summaries.Local)
            {
                if (this.Entry<Summary>(summary).State == EntityState.Added
                    || this.Entry<Summary>(summary).State == EntityState.Modified)
                {
                    summary.SetSummaDollar();
                }
            }
        }

        private int SaveChangesFullData()
        {
            CalculateSummary();

            int result = 0;
            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    result = base.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    var entry = ex.Entries.Single(); //Перезагрузка исходного значения свойства сущности с БД для решения проблемы оптимистичного параллелизма.
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    //entry.CurrentValues.SetValues(entry.GetDatabaseValues());//Приоритет БД
                }
            } while (saveFailed);

            return result;
        }

        public override int SaveChanges()
        {
            int result = 0;

            result = SaveChangesFullData();

            return result;
        }

        public virtual int SaveChangesInTransaction(DbTransaction transaction)
        {
            this.Database.UseTransaction(transaction);
            return this.SaveChanges();
        }
    }

}