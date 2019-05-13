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
            IDictionary<string, string> validationErrors = ValidateData(newUser);
            if (validationErrors.Count > 0)
            {
                throw new FaultException<DataNotValidException>(new DataNotValidException(validationErrors));
            }
            AddUser(newUser, owner);
        }

        public IDictionary<string, string> Validate(User user)
        {
            return ValidateData(user);
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
        #endregion


        private IDictionary<string, string> ValidateData(User user)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            DbEntityValidationResult validateResult;

            using (CFAPContext ctx = new CFAPContext())
            {
                try
                {
                    validateResult = ctx.Entry<User>(user).GetValidationResult();
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            if (!validateResult.IsValid)
            {
                foreach (var error in validateResult.ValidationErrors)
                {
                    result[error.PropertyName] = error.ErrorMessage;
                }
            }

            return result;
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
