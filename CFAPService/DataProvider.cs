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

        public User GetData(User user, Filter filter)
        {
            User result;
            try
            {
                result = GetFilteredData(user, filter);
            }
            catch (Exception ex)
            {
                throw new FaultException<DbException>(new DbException(ex));
            }
            return GetFilteredData(user, filter);
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

        private User GetFilteredData(User user, Filter filter)
        {
            User result = user;

            using (CFAPContext ctx = new CFAPContext())
            {
                var userGroups = new List<UserGroup>(user.UserGroups);
                for (int groupIndex = 0; groupIndex < userGroups.Count; groupIndex++)
                {
                    userGroups[groupIndex] = GetSummaryByGroup(
                                               userGroups[groupIndex],
                                               filter,
                                               ctx
                                             );
                }
            }

            return result;
        }

        private UserGroup GetSummaryByGroup(UserGroup userGroup, Filter filter, CFAPContext ctx)
        {
            UserGroup result = userGroup;

            if (ctx.Configuration.ProxyCreationEnabled)
                ctx.Configuration.ProxyCreationEnabled = false;


            DateTime? dateStart = filter.DateStart != null ? filter.DateStart : DateTime.MinValue;
            DateTime? dateEnd = filter.DateEnd != null ? filter.DateEnd : DateTime.MaxValue;

            var summaries = from s in ctx.Summaries
                            from g in s.UserGroups
                            where s.ActionDate >= dateStart && s.ActionDate <= dateEnd
                            && userGroup.Id == g.Id
                            select s;

            result.Summaries = summaries.ToList();

            return result;
        }

    }
}
