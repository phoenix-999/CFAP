﻿namespace CFAPDataModel
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
            var s = Summaries.Local;

            foreach (var summary in Summaries.Local)
            {
              
                if (this.Entry<Summary>(summary).State == EntityState.Added
                    || this.Entry<Summary>(summary).State == EntityState.Modified)
                {
                    summary.SetSummaDollar();
                }

            }
        }

        private int SaveChangesFullData(DbConcurencyUpdateOptions concurrencyOption)
        {
            CalculateSummary();

            int result = 0;
            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    //Добавления логикик обнаружения исключений оптимистического парралелизма
                    //Без ручного обнаружения исключения не возникает. Выяснить причину
                    try
                    {
                        var changed = this.ChangeTracker.Entries()
                            .Any(x => !x.CurrentValues.GetValue<byte[]>("RowVersion")
                            .SequenceEqual(x.OriginalValues.GetValue<byte[]>("RowVersion")));
                        var ex = new DbUpdateConcurrencyException();

                        if (changed) throw ex;
                    }
                    //Контсрукиця скрывает исключения если в сущности отсутствует поле RowVersion
                    catch (ArgumentException) {  }

                    result = base.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    //TODO: Добавить логику формирования Entries
                    var entry = ex.Entries.Single();//Перезагрузка исходного значения свойства сущности с БД для решения проблемы оптимистичного параллелизма.

                    switch (concurrencyOption)
                    {
                        case DbConcurencyUpdateOptions.None:
                            throw ex;
                            //break;
                        case DbConcurencyUpdateOptions.ClientPriority:
                            entry.OriginalValues.SetValues(entry.GetDatabaseValues());//Приоритет клиента
                            break;
                        case DbConcurencyUpdateOptions.DatabasePriority:
                            entry.CurrentValues.SetValues(entry.GetDatabaseValues());//Приоритет БД
                            break;
                    }
                }
            } while (saveFailed);

            return result;
        }

        //Метод переопределен для действия по умоланию с учетом дополнений внесенных в классе
        public override int SaveChanges()
        {
            return this.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
        }

        public int SaveChanges(DbConcurencyUpdateOptions concurrencyOption)
        {
            int result = 0;

            result = SaveChangesFullData(concurrencyOption);

            return result;
        }

        public virtual int SaveChangesInTransaction(DbTransaction transaction)
        {
            this.Database.UseTransaction(transaction);
            return this.SaveChanges();
        }
    }

}