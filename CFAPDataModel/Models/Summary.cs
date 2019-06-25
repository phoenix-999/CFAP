using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Collections;

namespace CFAPDataModel.Models
{
    [DataContract]
    public class Summary : ICloneable
    {
        
        public Summary()
        {
            this.UserGroups = new HashSet<UserGroup>();  
        }


        public void SetAutoValues()
        {
             this.ActionDate = DateTime.Now; 
        }


        private double GetRateUSD()
        {
            double result = 0;

            using (CFAPContext ctx = new CFAPContext())
            {
                double? query = (from rate in ctx.Rates
                            where rate.DateRate.Month == this.SummaryDate.Month && rate.DateRate.Year == this.SummaryDate.Year
                            select rate.RateUSD).FirstOrDefault();
                if (query != null)
                {
                    result = (double)query;
                }
            }

            return result;
        }

        private double GetEuroToDollarRate()
        {
            double result = 0;

            using (CFAPContext ctx = new CFAPContext())
            {
                double? query = (from rate in ctx.Rates
                                 where rate.DateRate.Month == this.SummaryDate.Month && rate.DateRate.Year == this.SummaryDate.Year
                                 select rate.EuroToDollarRate).FirstOrDefault();
                if (query != null)
                {
                    result = (double)query;
                }
            }

            return result;
        }

        public ICollection<int> GetUserGroupsId()
        {
            if (this.UserGroups == null || this.UserGroups.Count == 0)
            {
                throw new NullReferenceException("Группы пользователя не определены.");
            }

            var groupsId = (from g in this.UserGroups
                            select g.Id).ToList();

            return groupsId;
        }

        private void LoadUserGroups(CFAPContext ctx)
        {
            //Для корректной загрузки данных нужен экземпляр контекста вызывающей строны
            ctx.Configuration.ProxyCreationEnabled = false;

            ICollection<int> goupsId = null;
            try
            {
                goupsId = this.GetUserGroupsId();
            }
            catch(NullReferenceException)
            {
                DbValidationError validationError = new DbValidationError("Группы пользователя", "Значениие не определено");
                DbEntityValidationResult dbEntityValidationResult = new DbEntityValidationResult(ctx.Entry(this), new DbValidationError[] { validationError });

                throw new DbEntityValidationException("Ошибка при проверке данных групп пользователей. Группы не добавлены", new DbEntityValidationResult[] { dbEntityValidationResult });
            }

            //LINQ to Entities не умеет вызывать методы
            var groups = (from g in ctx.UserGroups
                          where goupsId.Contains(g.Id)
                          select g).ToList();
            this.UserGroups = groups;

            var groupsCanUseAllData = (from g in ctx.UserGroups
                                       where g.CanReadAllData == true
                                       select g).ToList();

            foreach (var currentGroup in this.UserGroups)
            {
                for (int groupIndex = 0; groupIndex < groupsCanUseAllData.Count; groupIndex++)
                {
                    if (groupsCanUseAllData[groupIndex].Id == currentGroup.Id)
                    {
                        groupsCanUseAllData.RemoveAt(groupIndex);
                    }
                }
            }

            if (groupsCanUseAllData.Count > 0)
            {
                foreach (var group in groupsCanUseAllData)
                {
                    this.UserGroups.Add(group);
                }
            }

        }

        public void SetRelationships(CFAPContext ctx)
        {
            ctx.Configuration.ProxyCreationEnabled = false;


            if (this.Project != null && this.Project.Id != default(int))
            {
                // В случае наличия связей с другимим сущностями, это наиболле простой способ добавления сущностей в контекст, иначе, видимо, придется прикрепить каждую связанную сущность и ее связи
                //this.Project = (from p in ctx.Projects where p.Id == this.Project.Id select p).First(); 
                ctx.Projects.Attach(this.Project);
                this.Project_Id = this.Project.Id;
            }

            if (this.Accountable != null && this.Accountable.Id != default(int))
            {
                // В случае наличия связей с другимим сущностями, это наиболле простой способ добавления сущностей в контекст, иначе, видимо, придется прикрепить каждую связанную сущность и ее связи
                //this.Accountable = (from a in ctx.Accountables where a.Id == this.Accountable.Id select a).First();
                ctx.Accountables.Attach(this.Accountable);
                this.Accountable_Id = this.Accountable.Id;
            }

            if (this.BudgetItem != null && this.BudgetItem.Id != default(int))
            {
                // В случае наличия связей с другимим сущностями, это наиболле простой способ добавления сущностей в контекст, иначе, видимо, придется прикрепить каждую связанную сущность и ее связи
                //this.BudgetItem = (from i in ctx.BudgetItems where i.Id == this.BudgetItem.Id select i).First();
                ctx.BudgetItems.Attach(this.BudgetItem);
                this.BudgetItem_Id = this.BudgetItem.Id;
            }

            //В данном случае Attach не сработает по причине наличия связи с группами пользователей
            //Если добавлять через Attach надо загрузить все группы пользователя и до каждой грппы догрузить всех пользователей
            this.UserLastChanged = (from u in ctx.Users where u.Id == this.UserLastChanged.Id select u).First();
            this.UserLastChangedId = UserLastChanged.Id;

            this.LoadUserGroups(ctx);
        }

        public static void LoadRelationships(CFAPContext ctx)
        {
            var oldCofigurationProxy = ctx.Configuration.ProxyCreationEnabled;
            ctx.Configuration.ProxyCreationEnabled = false;
            
            foreach (var s in ctx.Summaries.Local)
            {
                //Исключения могут выпадать только при нарушении целостности данных в БД
                //s.Accountable = (from a in ctx.Accountables where a.Id == s.Accountable_Id select a).Single();
                //s.Project = (from p in ctx.Projects where p.Id == s.Project_Id select p).Single();
                //s.BudgetItem = (from b in ctx.BudgetItems where b.Id == s.BudgetItem_Id select b).Single();
                //s.UserLastChanged = (from u in ctx.Users where u.Id == s.UserLastChangedId select u).Single();
                //ctx.Entry(s).Collection("UserGroups").Load();

                //Все связанные сущности существуют в одном экземпляре
                //В случае отсутствия данных метод Find вернет null
                s.Accountable = ctx.Accountables.Find(s.Accountable_Id);
                s.Project = ctx.Projects.Find(s.Project_Id);
                s.BudgetItem = ctx.BudgetItems.Find(s.BudgetItem_Id);
                s.UserLastChanged = ctx.Users.Find(s.UserLastChangedId);
                ctx.Entry(s).Collection("UserGroups").Load(); //Не сработает, если коллекция уже содержит элементы
            }

            ctx.Configuration.ProxyCreationEnabled = oldCofigurationProxy;
        }

        public void CustomValidate(CFAPContext ctx)
        {
            List<DbEntityValidationResult> validationResults = new List<DbEntityValidationResult>();

            var summaryResult = ctx.Entry(this).GetValidationResult();
            if (!summaryResult.IsValid) { validationResults.Add(summaryResult); }

            CustomProperiesValidate<Accountable>(this.Accountable, ctx, validationResults);
            CustomProperiesValidate<BudgetItem>(this.BudgetItem, ctx, validationResults);
            CustomProperiesValidate<Project>(this.Project, ctx, validationResults);

            if (validationResults.Count > 0)
            {
                throw new DbEntityValidationException(
                    "Ошибка при проверке данных. Данные могут остутствовать или указаны не верно. Проверте внесенные данные и повторите попытку"
                    , validationResults);
            }
        }

        private void CustomProperiesValidate<TProperty>(TProperty property, CFAPContext ctx, List<DbEntityValidationResult> validationResults) where TProperty : class
        {
            if (validationResults == null) throw new ArgumentNullException("List<DbEntityValidationResult> validationResults педреан с неопределенным значением.");

            if (property == null)
            {
                DbValidationError validationError = new DbValidationError(typeof(TProperty).ToString(), "Значениие не определено");
                DbEntityValidationResult dbEntityValidationResult = new DbEntityValidationResult(ctx.Entry(this), new DbValidationError[] { validationError });
                validationResults.Add(dbEntityValidationResult);
                return;
            }

            var propertyValidationResult = ctx.Entry(property).GetValidationResult();

            if (!propertyValidationResult.IsValid) { validationResults.Add(propertyValidationResult); }
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

        public object Clone()
        {
            Summary clonedSummary = new Summary();

            Type thisType = this.GetType();
            Type cloneType = clonedSummary.GetType();

            var thisFileds = thisType.GetFields();

            foreach (var field in thisFileds)
            {
                cloneType.GetField(field.Name).SetValue(clonedSummary, field.GetValue(this));
            }

            var thisProperties = thisType.GetProperties();

            foreach (var property in thisProperties)
            {
                cloneType.GetProperty(property.Name).SetValue(clonedSummary, property.GetValue(this));
            }


            clonedSummary.UserGroups = this.UserGroups.ToList();

            return clonedSummary;
        }

        #region Properies
        [DataMember]
        public int Id { get; set; }

        [Timestamp]
        [DataMember]
        public byte[] RowVersion { get; set; }

        [DataMember]
        [Column(TypeName = "datetime2")]
        public DateTime SummaryDate { get; set; }

        public int? UserLastChangedId { get; set; }

        [DataMember]
        [ForeignKey("UserLastChangedId")]

        public User UserLastChanged { get; set; }

        [DataMember]
        public double SummaUAH { get; set; }

        [DataMember]
        [NotMapped]
        public double CurrentRateUSD { get { return GetRateUSD(); } set { } }

        [DataMember]
        [NotMapped]
        public double CurrentEuroToDollarRate { get { return GetEuroToDollarRate(); } set { } }

        [DataMember]
        [NotMapped]
        public double SummaUSD{ get { return this.SummaUAH / this.CurrentRateUSD; } set { } } //При маршалинге выражение вычисляеться.

        [DataMember]
        [NotMapped]
        public double SummaEuro { get { return this.SummaUAH / (this.CurrentRateUSD * this.CurrentEuroToDollarRate); } set { } } //При маршалинге выражение вычисляеться.

        [DataMember]
        public bool CashFlowType { get; set; }

        [DataMember]
        [Column(TypeName = "datetime2")]
        public DateTime ActionDate { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }

        public int Project_Id { get; set; }

        [DataMember]
        [ForeignKey("Project_Id")]
        [Required]
        public virtual Project Project { get; set; }

        public int BudgetItem_Id { get; set; }

        [DataMember]
        [ForeignKey("BudgetItem_Id")]
        [Required]
        public virtual BudgetItem BudgetItem { get; set; }

        [DataMember]
        public string Description { get; set; }

        public int Accountable_Id { get; set; }

        [DataMember]
        [ForeignKey("Accountable_Id")]
        [Required]
        public virtual Accountable Accountable { get; set; }

        [DataMember]
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        #endregion
    }
}
