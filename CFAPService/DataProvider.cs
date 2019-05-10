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
                            select u;

                try
                {
                    result = query.FirstOrDefault<User>();
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

            if (!owner.IsAdmin)
            {
                throw new FaultException<AddUserNotAdminException>(new AddUserNotAdminException(owner));
            }

            newUser.Owners = GetOwners(owner);
            newUser.Owners = new List<User>();
            newUser.Owners.Add(owner);
            

            newUser.EncriptPassword(); 

            using (CFAPContext ctx = new CFAPContext())
            {
                try
                {
                    ctx.Users.Attach(newUser);
                    ctx.Entry<User>(newUser).State = System.Data.Entity.EntityState.Added;
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }
        }

        private ICollection<User> GetOwners(User user)
        {
            List<User> result = new List<User>();

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                var userDb = ctx.Users.Find(user.Id);
                var owners = user.Owners.ToArray();
                result.AddRange(owners);

                if (user.Owners.Count() > 0)
                {
                    result.AddRange(user.Owners);

                    foreach (var u in user.Owners)
                    {
                        result.AddRange(GetOwners(u));

                    }
                }
            }

                

            return result;
        }

        private User GetFilteredData(User user, Filter filter)
        {
            User result = user;
            user.Summaries = GetSummaries(filter.DateStart, filter.DateEnd, user);
            return result;
        }

        private ICollection<Summary> GetSummaries(DateTime? startDate, DateTime? endDate, User user)
        {
            ICollection<Summary> result = null;

            if (startDate == null) startDate = DateTime.MinValue;
            if (endDate == null) endDate = DateTime.MaxValue;

            using (CFAPContext ctx = new CFAPContext())
            {
                //Отключение создания прокси-классов наследников для сущностей. Позволяет использовать DataContractAttribute для класса сущности.
                ctx.Configuration.ProxyCreationEnabled = false;
                var query = from s in ctx.Summaries
                            where
                                s.ActionDate >= startDate
                                && s.ActionDate <= endDate
                                && s.Users.Where(u=>u.Id == user.Id).Count() > 0
                            select s;
                result = query.ToList();

            }

            return result;
        }
    }
}
