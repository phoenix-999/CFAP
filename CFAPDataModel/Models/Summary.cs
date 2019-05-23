﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel;

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

        public ICollection<int> GetUserGroupsId()
        {
            var groupsId = (from g in this.UserGroups
                            select g.Id).ToList();

            return groupsId;
        }

        private void LoadUserGroups(CFAPContext ctx)
        {
            //Для корректной загрузки данных нужен экземпляр контекста вызывающей строны
            ctx.Configuration.ProxyCreationEnabled = false;

            var goupsId = this.GetUserGroupsId();

            //LINQ to Entities не умеет вызывать методы
            var groups = (from g in ctx.UserGroups
                          where goupsId.Contains(g.Id)
                          select g).ToList();
            this.UserGroups = groups;

            var groupsCanUseAllData = (from g in ctx.UserGroups
                                       where g.CanUserAllData == true
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
                //this.Project = (from p in ctx.Projects where p.Id == this.Project.Id select p).First();
                ctx.Projects.Attach(this.Project);
                this.Project_Id = this.Project.Id;
            }

            if (this.Accountable != null && this.Accountable.Id != default(int))
            {
                //this.Accountable = (from a in ctx.Accountables where a.Id == this.Accountable.Id select a).First();
                ctx.Accountables.Attach(this.Accountable);
                this.Accountable_Id = this.Accountable.Id;
            }

            if (this.BudgetItem != null && this.BudgetItem.Id != default(int))
            {
                //this.BudgetItem = (from i in ctx.BudgetItems where i.Id == this.BudgetItem.Id select i).First();
                ctx.BudgetItems.Attach(this.BudgetItem);
                this.BudgetItem_Id = this.BudgetItem.Id;
            }

            if (this.Description != null && this.Description.Id != default(int))
            {
                //this.Description = (from d in ctx.Descriptions where d.Id == this.Description.Id select d).First();
                ctx.Descriptions.Attach(this.Description);
                this.DescriptionItem_Id = this.Description.Id;
            }

            //В данном случае Attach не сработает по причине наличия связи с группами пользователей
            //Если добавлять через Attach надо загрузить все группы пользователя и до каждой грппы догрузить всех пользователей
            this.UserLastChanged = (from u in ctx.Users where u.Id == this.UserLastChanged.Id select u).First();
            this.UserLastChangedId = UserLastChanged.Id;

            this.LoadUserGroups(ctx);
        }

        public static void LoadRelationships(CFAPContext ctx)
        {
            ctx.Summaries
                .Include("Project")
                .Include("BudgetItem")
                .Include("Accountable")
                .Include("Description")
                .Include("UserLastChanges")
                .Include("UserGroups");

            foreach (var s in ctx.Summaries.Local)
            {
                s.Accountable = (from a in ctx.Accountables where a.Id == s.Accountable_Id select a).Single();
                s.Project = (from p in ctx.Projects where p.Id == s.Project_Id select p).Single();
                s.BudgetItem = (from b in ctx.BudgetItems where b.Id == s.BudgetItem_Id select b).Single();
                s.Description = (from d in ctx.Descriptions where d.Id == s.DescriptionItem_Id select d).Single();
                s.UserLastChanged = (from u in ctx.Users where u.Id == s.UserLastChangedId select u).Single();
                ctx.Entry(s).Collection("UserGroups").Load();
            }
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
        public double SummaGrn { get; set; }

        [DataMember]
        public double SummaDolar{ get; set; }

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

        public int DescriptionItem_Id { get; set; }

        [DataMember]
        [ForeignKey("DescriptionItem_Id")]
        [Required]
        public virtual DescriptionItem Description { get; set; }

        public int Accountable_Id { get; set; }

        [DataMember]
        [ForeignKey("Accountable_Id")]
        [Required]
        public virtual Accountable Accountable { get; set; }

        [DataMember]
        public virtual ICollection<UserGroup> UserGroups { get; set; }

        [DataMember]
        [NotMapped]
        public bool IsModified { get; set; }

        #endregion
    }
}
