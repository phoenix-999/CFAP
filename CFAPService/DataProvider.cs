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
            //Аутентификация пользователя-владельца
            AuthenticateUser(owner);

            //Проверка - иммеет ли право владелец добавлять или изменять данные пользователей (User.CanAddNewUser)
            if (owner.CanAddNewUsers == false)
            {
                //Если ложь - сбой
                throw new FaultException<AddUserNotAdminException>(new AddUserNotAdminException(owner));
            }

            //Создание экземпляра контекста
            using (CFAPContext ctx = new CFAPContext())
            {
                //Загрузка в контекст данных о группах пользователя
                userForUpdate.LoadUserGroups(ctx);

                //Отмена изменения пароля

                userForUpdate.Password = (from u in ctx.Users where u.Id == userForUpdate.Id select u.Password).Single();

                //Обновление данных о пользователе путем удаление старого пользователя и добавления нового.
                //При обчном изменении данных не меняються ссылочные коллеции
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        //Удаление старых данных пользователя
                        var oldUser = (from u in ctx.Users where u.Id == userForUpdate.Id select u).Single();

                        //Сохранение старого идентификатора
                        int id = oldUser.Id;
                        userForUpdate.Id = id;

                        ctx.Users.Remove(oldUser);
                        ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);

                        //Добавление новых данных о пользователе
                        //Меняеться идентификатор
                        ctx.Users.Add(userForUpdate);
                        ctx.SaveChanges(DbConcurencyUpdateOptions.ClientPriority);


                        //Подтверждение завершения транзакции
                        transaction.Commit();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        transaction.Rollback();
                        throw new FaultException<DataNotValidException>(new DataNotValidException(ex.EntityValidationErrors));
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new FaultException<DbException>(new DbException(ex));
                    }
                }
            }




        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void AlterSummaries(List<Summary> summaries, User user)
        {
            AddOrUpdateSummaries(summaries, user, DbConcurencyUpdateOptions.ClientPriority);
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

                summary.SetStateProperties(ctx);

                if (summary.ReadOnly)
                {
                    throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(summary.GetType(), summary.Id, null, user));
                }

                try
                {
                    ctx.Summaries.Add(summary);

                    ctx.SaveChanges(DbConcurencyUpdateOptions.None);

                    summary.IsModified = false;

                    result = summary;
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
        public Summary UpdateSummary(Summary summary, User user, DbConcurencyUpdateOptions concurencyUpdateOption)
        {
            AuthenticateUser(user);


            //Группы пользователей уже существуют в сущности
            if (summary.UserGroups == null || summary.UserGroups.Count == 0)
            {
                summary.UserGroups = user.UserGroups;
            }


            //Пользователь который изменил данные устанавливается по факту
            if (summary.UserLastChanged == null || summary.UserLastChanged.Id != user.Id)
            {
                summary.UserLastChanged = user;
            }

            Summary result = null;

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Configuration.ProxyCreationEnabled = false;

                summary.SetStateProperties(ctx);

                if (summary.ReadOnly)
                {
                    throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(summary.GetType(), summary.Id, null, user));
                }

                try
                {
                    //В данном случае AddOrUpdate не сработает в выдаче исключения оптимистичного паралелизма. Он перезагружет сущности в контекс и формирует уже актуальное поле RowVersion
                    ctx.Summaries.Attach(summary);

                    ctx.Entry(summary).State = EntityState.Modified;

                    ctx.SaveChanges(concurencyUpdateOption);

                    result = (from s in ctx.Summaries where s.Id == summary.Id select s).Single();

                    result.IsModified = false;
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
        public int RemoveSummary(Summary summary, User user, DbConcurencyUpdateOptions concurencyUpdateOption)
        {
            AuthenticateUser(user);

            int result = 0;

            
            using (CFAPContext ctx = new CFAPContext())
            {
                if (summary.UserLastChanged == null || summary.UserLastChanged.Id != user.Id)
                {
                    summary.UserLastChanged = user;
                }

                summary.SetStateProperties(ctx);

                if (summary.ReadOnly)
                {
                    throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(summary.GetType(), summary.Id, null, user));
                }

                try
                {
                    ctx.Summaries.Attach(summary);

                    ctx.Entry(summary).State = EntityState.Deleted;

                    result = ctx.SaveChanges(concurencyUpdateOption);
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
        #endregion


        private void AddOrUpdateSummaries(List<Summary> summaries, User user, DbConcurencyUpdateOptions concurencyUpdateOption)
        {

            ///<summary>
            ///Обрабаиывает обьекты помечены IsModified
            /// </summary>
            //Провести атунтификацию пользователя с шифрованным паролем
            //В случае отсутсвия пользователя - сбой аутентификации
            AuthenticateUser(user);

            using (CFAPContext ctx = new CFAPContext())
            {
                var modifiedSummary = (from s in summaries
                                      where s.IsModified == true
                                      select s).ToList();
                //Перебор измененных или добавленных все summary и установка состояния свойств
                foreach (var s in modifiedSummary)
                {
                    if (s.UserGroups == null || s.UserGroups.Count == 0)
                    {
                        s.UserGroups = user.UserGroups;
                    }

                    if (s.UserLastChanged == null || s.UserLastChanged.Id != user.Id)
                    {
                        s.UserLastChanged = user;
                    }

                    s.SetStateProperties(ctx);

                    if (s.ReadOnly)
                    {
                        throw new FaultException<TryChangeReadOnlyFiledException>(new TryChangeReadOnlyFiledException(s.GetType(), s.Id, null, user));
                    }
                }

                //Добавление полученого списка в БД
                try
                {
                    ctx.Summaries.AddOrUpdate<Summary>(modifiedSummary.ToArray());
                    ctx.SaveChanges(concurencyUpdateOption);
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
                    //При возникновении ошибки связаной с обнулением ссылочных свйоств следует обратить внимание на тип коллекции в которой хранится ссылка на свойство в котором данные обнуляються.
                    //В данном случае классы ссылочных свойств сущности summary, такие как (Project, Accountable...) содержали коллекцию HashSet<Summary>, что стало причиной обнуления ссылочных свойств в сущности summary.
                    //Причиной такого поведения являеться тип коллекции, в которую при обработке первой summary происходит добавления этой сущности в коллекции ссылочных свойств
                    //В этом случае, экземляр summary косвенно (через ссылочное свойство) экземпляр HashSet
                    //Так как методы GetHashCode и Equals переопределы на сравнения summary.Id - каждый последующий добавленный (Id=0) экземпляр summary будет равен первому
                    //Более того, каждые обькет summary в данном случае содержит ссылку на один и тот же экземляр ссылочного свойства (Project, Accountable...)
                    //И при обработке последущих summary ссылки на их экземпляры не записываються в Project.Summary, Accountable.Summary...
                    //Соответсвенно каждый последующий summary больше не владеет ссылочными свойствами.
                    //Получаеться ситуация взаимного исключения.
                    //С коллекцией summary.UserGroups в данном случае проблем не возникает, так как в этой коллекции GetHashCode и Equals самой коллекции определны по уммолчанию
                }

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
                              select  s).ToArray(); //Distinct() не подходит для удаления дубликатов по переопределенным GetHashCode и Equals

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
