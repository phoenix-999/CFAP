using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAPDataModel.Models;
using CFAPDataModel;
using System.ServiceModel;
using CFAPService.Faults;
using NLog;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Data.Entity.Migrations;


namespace CFAPService
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class DataProvider : IDataProvider
    {
        #region IDataProvider
        public User Authenticate(User user)
        {
            User result = (User)AuthenticateUser(user, false);
            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void AddNewUser(User newUser, User owner)
        {
            try
            {
                AddUser(newUser, owner);
            }
            catch (DbEntityValidationException ex)
            {
                throw new FaultException<DataNotValidException>(new DataNotValidException(ex.EntityValidationErrors));
            }
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void UpdateUser(User userForUpdate, User owner)
        {
            
            //TODO Реализовать метод изменения данных пользователя

        }

        public HashSet<Summary> GetSummary(User user, Filter filter)
        {
            HashSet<Summary> result = new HashSet<Summary>();

            try
            {
                result = GetFilteredSummary(user, filter);
            }
            catch (Exception ex)
            {
                throw new FaultException<DbException>(new DbException(ex));
            }
            return result;            
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void AddOrUpdateSummary(HashSet<Summary> summary, User user)
        {
            UpdateSummary(summary, user);
        }
        #endregion

        private void UpdateSummary(HashSet<Summary> summary, User user)
        {
            AuthenticateUser(user);

            

            using (CFAPContext ctx = new CFAPContext())
            {
                foreach (var s in summary)
                {
                    ctx.Entry(s.Project).State = EntityState.Unchanged;
                    ctx.Entry(s.Accountable).State = EntityState.Unchanged;
                    ctx.Entry(s.BudgetItem).State = EntityState.Unchanged;
                    ctx.Entry(s.Description).State = EntityState.Unchanged;

                    s.LoadUserGroups(ctx);
                    s.ModifyForeignKey();
                }

                try
                {
                    ctx.Summaries.AddOrUpdate<Summary>(summary.ToArray());
                    ctx.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new FaultException<DataNotValidException>(new DataNotValidException(ex.EntityValidationErrors));
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }
        }


        private User AuthenticateUser(User user, bool hasEncriptedPassword = true)
        {
            User result = null;

            if (!hasEncriptedPassword)
                user.EncriptPassword();

            using (CFAPContext ctx = new CFAPContext())
            {
                //Отключение создания прокси-классов наследников для сущностей. Позволяет использовать DataContractAttribute для класса сущности.
                ctx.Configuration.ProxyCreationEnabled = false;
                var query = from u in ctx.Users
                            where u.Password == user.Password && u.UserName == user.UserName
                            select new { User = u, UserGoup = u.UserGroups};

                try
                {
                    result = query.FirstOrDefault().User;
                    //var groups = ctx.UserGroups.Include("Users").ToList(); //В этом случае ProxyCreationEnabled = true
                    var groups = query.FirstOrDefault().UserGoup;
                    result.UserGroups = groups.ToList();
                }
                catch(Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            if (result == null)
            {
                FaultException<AutenticateFaultException> fault = new FaultException<AutenticateFaultException>(new AutenticateFaultException(user));
                throw fault;
            }

            return result;
        }

        private void AddUser(User newUser, User owner)
        {
            AuthenticateUser(owner);

            if (!owner.CanAddNewUsers)
            {
                throw new FaultException<AddUserNotAdminException>(new AddUserNotAdminException(owner));
            }

            newUser.EncriptPassword(); 

            using (CFAPContext ctx = new CFAPContext())
            {
                try
                {
                    newUser.LoadUserGroups(ctx);
                    ctx.Users.Add(newUser);
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }
        }

       private HashSet<Summary> GetFilteredSummary(User user, Filter filter)
       {
            HashSet<Summary> result = new HashSet<Summary>();

            DateTime? dateStart = filter.DateStart != null ? filter.DateStart : DateTime.MinValue;
            DateTime? dateEnd = filter.DateEnd != null ? filter.DateEnd : DateTime.MaxValue;


            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                var userGroupsId = user.GetUserGroupsId();

                if (filter == null)
                {
                    var query = (from s in ctx.Summaries
                              from g in s.UserGroups
                              where userGroupsId.Contains(g.Id)
                              select  s).ToArray(); //Distinct() не аожходит для удаления дубликатов по переопределенным GetHashCode и Equals

                    result = new HashSet<Summary>(query);

                    return result;
                }

                var summaries = from s in ctx.Summaries
                                from g in s.UserGroups
                                where
                                    s.ActionDate >= dateStart && s.ActionDate <= dateEnd
                                    && userGroupsId.Contains(g.Id)
                                select s;

                if (filter.Projects != null && filter.Projects.Count > 0)
                {
                    var projectsId = filter.GetProjectsId();

                    summaries = from s in summaries
                                where projectsId.Contains(s.Project.Id) //В выражении LINQ To Entyties для сравнения экзмпляров можно использовать только примитивные типы или перечисления
                                select s;
                }

                if (filter.Accountables != null && filter.Accountables.Count > 0)
                {
                    var AccountableId = filter.GetAccountableId();

                    summaries = from s in summaries
                                where AccountableId.Contains(s.Accountable.Id)
                                select s;
                }

                if (filter.BudgetItems != null && filter.BudgetItems.Count > 0)
                {
                    var BudgetItemId = filter.GetBudgetItemsId();

                    summaries = from s in summaries
                                where BudgetItemId.Contains(s.BudgetItem.Id)
                                select s;
                }

                ctx.Summaries
                     .Include(s => s.Project)
                     .Include(s=> s.Accountable)
                     .Include(s => s.Description)
                     .Include(s => s.BudgetItem).Load();
               

                result = new HashSet<Summary>(summaries.ToArray());
            }

            return result;
       }

        

    }
}
