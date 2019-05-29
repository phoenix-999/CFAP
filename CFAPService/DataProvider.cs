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
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace CFAPService
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class DataProvider : IDataProvider
    {
        #region Котракт службы
        public List<string> GetLogins()
        {
            List<string> result = new List<string>();

            using (CFAPContext ctx = new CFAPContext())
            {
                try
                {
                    result = (from u in ctx.Users select u.UserName).Distinct().ToList();
                }
                catch(Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

           return result;
        }

        public User Authenticate(User user)
        {
            if (
                    user == null
                    || user.UserName == null
                    || user.Password == null
                    || user.UserName.Length == 0
                    || user.Password.Length == 0
               )
            {
                throw new FaultException<ArgumentNullException>(new ArgumentNullException("Не введен логин или пароль."));
            }

            User result = AuthenticateUser(user, false);
            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public User AddNewUser(User newUser, User owner)
        {
            AuthenticateUser(owner);

            this.CheckCanChangeUsersData(owner, typeof(User));

            if (newUser.UserGroups == null || newUser.UserGroups.Count == 0)
            {
                throw new FaultException<UserHasNotGroupsException>(new UserHasNotGroupsException(newUser));
            }

            if (newUser.Password == null || newUser.Password.Length == 0)
            {
                Dictionary<string, string> errors = new Dictionary<string, string>();
                errors.Add("Password", "Не указан пароль.");
                throw new FaultException<DataNotValidException>(new DataNotValidException(errors));
            }

            newUser.EncriptPassword();

            using (CFAPContext ctx = new CFAPContext())
            {
                try
                {
                    newUser.LoadUserGroupsFromObject(ctx);
                    ctx.Users.Add(newUser);
                    ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
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

            return newUser;
        }

        public List<User> GetUsers(User owner)
        {
            AuthenticateUser(owner);

            this.CheckCanChangeUsersData(owner, typeof(User));

            List<User> users = new List<User>();

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    if (owner.IsAdmin)
                    {
                        users = (from u in ctx.Users
                                 select u).Distinct().ToList(); //Админы могут видеть все пользователей 
                    }
                    else
                    {
                        owner.LoadUserGroupsFromDatabase(ctx);
                        var ownerUserGroupsId = (from g in owner.UserGroups select g.Id).ToList();
                        users = (from g in ctx.UserGroups
                                 from u in g.Users
                                 where ownerUserGroupsId.Contains(g.Id)
                                 select u).Distinct().ToList(); //Должны быть толкьо пользователи с групп владельца
                    }

                    foreach (var u in users)
                    {
                        u.LoadUserGroupsFromDatabase(ctx);
                    }
                }
                catch(Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            return users;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public User UpdateUser(User userForUpdate, User owner)
        {
            //Аутентификация пользователя-владельца
            AuthenticateUser(owner);

            //Проверка - иммеет ли право владелец добавлять или изменять данные пользователей (User.CanAddNewUser)
            this.CheckCanChangeUsersData(owner, typeof(User));

            if (userForUpdate.UserGroups == null || userForUpdate.UserGroups.Count == 0)
            {
                throw new FaultException<UserHasNotGroupsException>(new UserHasNotGroupsException(userForUpdate));
            }

            //Создание экземпляра контекста
            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                //Отмена изменения пароля

                userForUpdate.Password = (from u in ctx.Users where u.Id == userForUpdate.Id select u.Password).Single();

                try
                {
                    //Загрузка в контекст данных о группах пользователя
                    userForUpdate.LoadUserGroupsFromObject(ctx);
                    //Изменение связей с группами если они изменились
                    userForUpdate.ChangeUserGroups(ctx);

                    ctx.Entry(userForUpdate).State = EntityState.Modified;
                    ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
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


            return userForUpdate;

        }

        public List<UserGroup> GetUserGroups(User owner)
        {
            AuthenticateUser(owner);

            this.ChechIsAdmin(owner, typeof(UserGroup));

            List<UserGroup> userGroups = new List<UserGroup>();

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    userGroups = (from g in ctx.UserGroups
                                select g).Distinct().ToList(); 
                    
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            return userGroups;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public UserGroup AddNewUserGroup(UserGroup newUserGroup, User owner)
        {
            AuthenticateUser(owner);

            this.ChechIsAdmin(owner, typeof(UserGroup));


            using (CFAPContext ctx = new CFAPContext())
            {
                try
                {
                    ctx.UserGroups.Add(newUserGroup);
                    ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
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

            return newUserGroup;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public UserGroup UpdateUserGroup(UserGroup userGroupForUpdate, User owner)
        {
            //Аутентификация пользователя-владельца
            AuthenticateUser(owner);

            //Проверка - иммеет ли право владелец добавлять или изменять данные
            this.ChechIsAdmin(owner, typeof(UserGroup));


            //Создание экземпляра контекста
            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                try
                {
                    ctx.Entry(userGroupForUpdate).State = EntityState.Modified;
                    ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
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


            return userGroupForUpdate;

        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Summary AddSummary(Summary summary, User user)
        {
            AuthenticateUser(user);

            if (summary.UserGroups == null || summary.UserGroups.Count == 0)
            {
                summary.UserGroups = user.UserGroups;
            }

            if (summary.UserLastChanged == null || summary.UserLastChanged.Id != user.Id)
            {
                summary.UserLastChanged = user;
            }

            Summary result = null;

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                summary.SetRelationships(ctx);

                try
                {
                    ctx.Summaries.Add(summary);

                    ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);

                    result = summary;
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

            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Summary UpdateSummary(Summary summary, User user, DbConcurencyUpdateOptions concurencyUpdateOption)
        {
            //Изменение значения поля ReadOnly осуществляеться другой операцией службы


            AuthenticateUser(user);

            //Пользователь который изменил данные устанавливается по факту
            if (summary.UserLastChanged == null || summary.UserLastChanged.Id != user.Id)
            {
                summary.UserLastChanged = user;
            }

            Summary result = null;

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                try
                {
                    //Группы пользователей уже существуют в сущности
                    //Если, по какой то причине их нет - будет исключение валидации

                    summary.SetRelationships(ctx);

                    //В данном случае AddOrUpdate не сработает в выдаче исключения оптимистичного паралелизма. Он перезагружет сущности в контекс и формирует уже актуальное поле RowVersion
                    ctx.Summaries.Attach(summary);

                    var summaryDbVersion = (Summary)ctx.Entry(summary).GetDatabaseValues().ToObject();
                    if (summaryDbVersion.ReadOnly)
                    {
                        throw new ReadOnlyException();
                    }

                    ctx.Entry(summary).State = EntityState.Modified;

                    //Ручной запуск валидации необходм так как при модификации данных связи с сущностями помечены как EntityState.Unchanged
                    //В итоге, без ручной валидации, при неверных данных или значениях null будет исключения при внесении данных в бд, а валидация EF ничего не заметит
                    summary.CustomValidate(ctx);

                    ctx.SaveChanges(concurencyUpdateOption);

                    result = (from s in ctx.Summaries where s.Id == summary.Id select s).Single();
                }
                catch(ReadOnlyException)
                {
                    throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(typeof(Summary), summary.Id, null, user));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ConcurrencyException<Summary> concurrencyException = new ConcurrencyException<Summary>(ex);
                    throw new FaultException<ConcurrencyException<Summary>>(concurrencyException);
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


            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void ChangeSummaryReadOnlyStatus(bool onOff, Filter filter, User user)
        {
            AuthenticateUser(user);

            this.ChechIsAdmin(user, typeof(Summary));

            List<Summary> summaries = this.GetSummary(user, filter).ToList();

            using (CFAPContext ctx = new CFAPContext())
            {
                try
                {
                    foreach (var summary in summaries)
                    {
                        ctx.Summaries.Attach(summary);
                        summary.ReadOnly = onOff;
                        ctx.Entry(summary).State = EntityState.Modified;
                        ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
                    }
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }
        }


        [OperationBehavior(TransactionScopeRequired = true)]
        public int RemoveSummary(Summary summary, User user, DbConcurencyUpdateOptions concurencyUpdateOption)
        {
            AuthenticateUser(user);

            if (concurencyUpdateOption == DbConcurencyUpdateOptions.DatabasePriority)
            {
                throw new FaultException<InvalidOperationException>(new InvalidOperationException("Работа опрации в режиме DbConcurencyUpdateOptions.DatabasePriority не имеет смсла."));
            }

            int result = 0;

            
            using (CFAPContext ctx = new CFAPContext())
            {
                if (summary.UserLastChanged == null || summary.UserLastChanged.Id != user.Id)
                {
                    summary.UserLastChanged = user;
                }

                summary.SetRelationships(ctx);

                try
                {
                    ctx.Summaries.Attach(summary);

                    var summaryDbVersion = (Summary)ctx.Entry(summary).GetDatabaseValues().ToObject();
                    if (summaryDbVersion.ReadOnly)
                    {
                        throw new ReadOnlyException();
                    }

                    ctx.Entry(summary).State = EntityState.Deleted;

                    result = ctx.SaveChanges(concurencyUpdateOption);
                }
                catch (ReadOnlyException)
                {
                    throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(typeof(Summary), summary.Id, null, user));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ConcurrencyException<Summary> concurrencyException = new ConcurrencyException<Summary>(ex);
                    throw new FaultException<ConcurrencyException<Summary>>(concurrencyException);
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }

            }

            return result;
        }
        
        public List<Accountable> GetAccountables(User user)
        {
            AuthenticateUser(user);

            List<Accountable> result = null;

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    result = (from a in ctx.Accountables select a).Distinct().ToList();
                }
                catch(Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

                return result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Accountable AddAccountable(Accountable newAccountable, User user)
        {
            AuthenticateUser(user);

            this.ChechIsAdmin(user, typeof(Accountable));

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    ctx.Accountables.Add(newAccountable);
                    ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
                }
                catch(DbEntityValidationException ex)
                {
                    throw new FaultException<DataNotValidException>(new DataNotValidException(ex.EntityValidationErrors));
                }
                catch(Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            return newAccountable;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Accountable UpdateAccountable(Accountable accountableToUpdate, User user, DbConcurencyUpdateOptions concurencyUpdateOption)
        {
            AuthenticateUser(user);

            this.ChechIsAdmin(user, typeof(Accountable));

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                try
                {
                    ctx.Accountables.Attach(accountableToUpdate);

                    var accountableToUpdateDbVersion = (Accountable)ctx.Entry(accountableToUpdate).GetDatabaseValues().ToObject();
                    if (accountableToUpdateDbVersion.ReadOnly)
                    {
                        throw new ReadOnlyException();
                    }

                    ctx.Entry(accountableToUpdate).State = EntityState.Modified;
                    ctx.SaveChanges(concurencyUpdateOption);
                }
                catch(ReadOnlyException)
                {
                    throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(typeof(Accountable), accountableToUpdate.Id, accountableToUpdate.AccountableName, user));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ConcurrencyException<Accountable> concurrencyException = new ConcurrencyException<Accountable>(ex);
                    throw new FaultException<ConcurrencyException<Accountable>>(concurrencyException);
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

                return accountableToUpdate;
        }


        public List<Project> GetProjects(User user)
        {
            AuthenticateUser(user);

            List<Project> result = null;

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    result = (from a in ctx.Projects select a).Distinct().ToList();
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Project AddProject(Project newProject, User user)
        {
            AuthenticateUser(user);

            this.ChechIsAdmin(user, typeof(Project));

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    ctx.Projects.Add(newProject);
                    ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
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

            return newProject;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Project UpdateProject(Project projectToUpdate, User user, DbConcurencyUpdateOptions concurencyUpdateOption)
        {
            AuthenticateUser(user);

            this.ChechIsAdmin(user, typeof(Project));

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                try
                {
                    ctx.Projects.Attach(projectToUpdate);

                    var projectToUpdateDbVersion = (Project)ctx.Entry(projectToUpdate).GetDatabaseValues().ToObject();
                    if (projectToUpdateDbVersion.ReadOnly)
                    {
                        throw new ReadOnlyException();
                    }

                    ctx.Entry(projectToUpdate).State = EntityState.Modified;
                    ctx.SaveChanges(concurencyUpdateOption);
                }
                catch (ReadOnlyException)
                {
                    throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(typeof(Project), projectToUpdate.Id, projectToUpdate.ProjectName, user));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ConcurrencyException<Project> concurrencyException = new ConcurrencyException<Project>(ex);
                    throw new FaultException<ConcurrencyException<Project>>(concurrencyException);
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

            return projectToUpdate;
        }


        public List<BudgetItem> GetBudgetItems(User user)
        {
            AuthenticateUser(user);

            List<BudgetItem> result = null;

            using (CFAPContext ctx = new CFAPContext())

            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    result = (from i in ctx.BudgetItems select i).Distinct().ToList();
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public BudgetItem AddBudgetItem(BudgetItem newBudgetItem, User user)
        {
            AuthenticateUser(user);

            this.ChechIsAdmin(user, typeof(BudgetItem));

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    ctx.BudgetItems.Add(newBudgetItem);
                    ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
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

            return newBudgetItem;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public BudgetItem UpdateBudgetItem(BudgetItem budgetItemToUpdate, User user, DbConcurencyUpdateOptions concurencyUpdateOption)
        {
            AuthenticateUser(user);

            this.ChechIsAdmin(user, typeof(BudgetItem));

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                try
                {
                    ctx.BudgetItems.Attach(budgetItemToUpdate);

                    var budgetItemToUpdateDbVersion = (BudgetItem)ctx.Entry(budgetItemToUpdate).GetDatabaseValues().ToObject();
                    if (budgetItemToUpdateDbVersion.ReadOnly)
                    {
                        throw new ReadOnlyException();
                    }

                    ctx.Entry(budgetItemToUpdate).State = EntityState.Modified;
                    ctx.SaveChanges(concurencyUpdateOption);
                }
                catch (ReadOnlyException)
                {
                    throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(typeof(BudgetItem), budgetItemToUpdate.Id, budgetItemToUpdate.ItemName, user));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ConcurrencyException<BudgetItem> concurrencyException = new ConcurrencyException<BudgetItem>(ex);
                    throw new FaultException<ConcurrencyException<BudgetItem>>(concurrencyException);
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

            return budgetItemToUpdate;
        }

        public List<Rate> GetRates(User user)
        {
            AuthenticateUser(user);

            List<Rate> result = null;

            using (CFAPContext ctx = new CFAPContext())

            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    result = (from r in ctx.Rates select r).Distinct().ToList();
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Rate AddRate(Rate newRate, User user)
        {
            AuthenticateUser(user);

            this.ChechIsAdmin(user, typeof(Rate));

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;
                try
                {
                    ctx.Rates.Add(newRate);
                    ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);
                }
                catch (DbEntityValidationException ex) //Возникнуть может только при повреждении данных
                {
                    throw new FaultException<DataNotValidException>(new DataNotValidException(ex.EntityValidationErrors));
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            return newRate;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public Rate UpdateRate(Rate rateToUpdate, User user, DbConcurencyUpdateOptions concurencyUpdateOption)
        {
            AuthenticateUser(user);

            this.ChechIsAdmin(user, typeof(Rate));

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                try
                {
                    ctx.Rates.Attach(rateToUpdate);

                    var ratesToUpdateDbVersion = (Rate)ctx.Entry(rateToUpdate).GetDatabaseValues().ToObject();
                    if (ratesToUpdateDbVersion.ReadOnly)
                    {
                        throw new ReadOnlyException();
                    }

                    ctx.Entry(rateToUpdate).State = EntityState.Modified;
                    ctx.SaveChanges(concurencyUpdateOption);
                }
                catch (ReadOnlyException)
                {
                    throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(typeof(Rate), rateToUpdate.Id, null, user));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ConcurrencyException<Rate> concurrencyException = new ConcurrencyException<Rate>(ex);
                    throw new FaultException<ConcurrencyException<Rate>>(concurrencyException);
                }
                catch (DbEntityValidationException ex)//Возникнуть может только при повреждении данных
                {
                    throw new FaultException<DataNotValidException>(new DataNotValidException(ex.EntityValidationErrors));
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            return rateToUpdate;
        }

        #endregion

        private void CheckCanChangeUsersData(User user, Type entityType)
        {
            if (!user.CanChangeUsersData)
            {
                throw new FaultException<NoRightsToChangeDataException>(new NoRightsToChangeDataException(user, entityType.Name));
            }
        }

        private void ChechIsAdmin(User user, Type entityType)
        {
            if (!user.IsAdmin)
            {
                throw new FaultException<NoRightsToChangeDataException>(new NoRightsToChangeDataException(user, entityType.Name));
            }
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
                            select new { User = u, UserGroup = u.UserGroups};

                try
                {
                    var resultQuery = query.Single();
                    result = resultQuery.User;
                    //var groups = ctx.UserGroups.Include("Users").ToList(); //В этом случае ProxyCreationEnabled = true
                    var groups = resultQuery.UserGroup;
                    result.UserGroups = groups.ToList();
                }
                catch(InvalidOperationException ex) //На случай если query.Single(); ничего не вернет или врнет больше одного результата
                {
                    throw new FaultException<AuthenticateFaultException>(new AuthenticateFaultException(user));
                }
                catch(Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            if (result == null)
            {
                FaultException<AuthenticateFaultException> fault = new FaultException<AuthenticateFaultException>(new AuthenticateFaultException(user));
                throw fault;
            }

            return result;
        }

        private HashSet<Summary> GetFilteredSummary(User user, Filter filter)
       {
            HashSet<Summary> result = new HashSet<Summary>();

            DateTime? dateStart = null;
            DateTime? dateEnd = null;
            if (filter != null)
            {
                dateStart = filter.DateStart != null ? filter.DateStart : DateTime.MinValue;
                dateEnd = filter.DateEnd != null ? filter.DateEnd : DateTime.MaxValue;
            }


            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                var userGroupsId = user.GetUserGroupsIdFromObject();

                if (filter == null)
                {
                    var query = (from s in ctx.Summaries
                              from g in s.UserGroups
                              where userGroupsId.Contains(g.Id)
                              select  s).ToArray(); //Distinct() не подходит для удаления дубликатов по переопределенным GetHashCode и Equals

                    result = new HashSet<Summary>(query);

                    try
                    {
                        Summary.LoadRelationships(ctx);
                    }
                    catch(Exception ex)
                    {
                        throw new FaultException<DbException>(new DbException(ex));
                    }

                    return result;
                }

                var summaries = from s in ctx.Summaries
                                from g in s.UserGroups
                                where
                                    s.SummaryDate >= dateStart && s.SummaryDate <= dateEnd
                                    && userGroupsId.Contains(g.Id)
                                select s;
                result = new HashSet<Summary>(summaries.ToArray());

                if (filter.Projects != null && filter.Projects.Count > 0)
                {
                    var projectsId = filter.GetProjectsId();

                    summaries = from s in summaries
                                where projectsId.Contains(s.Project.Id) //В выражении LINQ To Entity для сравнения экзмпляров можно использовать только примитивные типы или перечисления
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

                result = new HashSet<Summary>(summaries.ToArray());

                try
                {
                    Summary.LoadRelationships(ctx);
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }

            }

            return result;
       }

        

    }
}
