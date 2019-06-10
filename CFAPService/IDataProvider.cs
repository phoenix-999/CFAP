using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CFAPDataModel.Models;
using CFAPService.Faults;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using CFAPDataModel;

namespace CFAPService
{
    [ServiceContract]
    interface IDataProvider
    {
        /// <summary>
        /// После кодирования всей системы был придуман альтернативный вариант проекта службы.
        /// 
        /// Создать интерфейс ICrudOperations, который будет содеражать перечень методов соответсвующих CRUD операциям работы с БД.
        /// Каждая сущность должна реализовать этот интерфейс.
        /// Все типы исключений, приводящие к сбоям службы, должны быть реализованы в модели данных и инициализироваться реализованным итерфейсом.
        /// 
        /// Создать перечислений CrudOperations  для конкретизации операции вызова службы.
        /// 
        /// Метод службы будет принимать экземпляр User, экземпляр сущности (типа ICrudOperations) и тип перечесления CrudOperations
        /// В методе службы, для работы с сущностями, или классе службы определит аттрибуты KnownType для каждой сущности.
        /// 
        /// Это повысит абстракцию, повторное использования кода, расширяемость и удобство сопровождения.
        /// </summary>

        [OperationContract]
        [FaultContract(typeof(DbException))]
        List<string> GetLogins();

        [OperationContract]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(ArgumentNullException))]
        User Authenticate(User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(UserHasNotGroupsException))]
        User AddNewUser(User newUser, User owner);

        [OperationContract]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        List<User> GetUsers(User owner);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(UserHasNotGroupsException))]
        User UpdateUser(User userForUpdate, User owner);

        [OperationContract]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        List<UserGroup> GetUserGroups(User owner);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        [FaultContract(typeof(DataNotValidException))]
        UserGroup AddNewUserGroup(UserGroup newUserGroup, User owner);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        [FaultContract(typeof(DataNotValidException))]
        UserGroup UpdateUserGroup(UserGroup userGroupForUpdate, User owner);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        Summary AddSummary(Summary summary, User user);

        [OperationContract]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        HashSet<Summary> GetSummary(User user, Filter filter);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        [FaultContract(typeof(FiledDeletedException))]
        [FaultContract(typeof(ConcurrencyException<Summary>))]
        Summary UpdateSummary(Summary summary, User user, DbConcurencyUpdateOptions concurencyUpdateOptions);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        void ChangeSummaryReadOnlyStatus(bool onOff, Filter filter, User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        [FaultContract(typeof(ConcurrencyException<Summary>))]
        [FaultContract(typeof(FiledDeletedException))]
        [FaultContract(typeof(InvalidOperationException))]
        Summary RemoveSummary(Summary summary, User user, DbConcurencyUpdateOptions concurencyUpdateOptions);

        [OperationContract]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        List<Accountable> GetAccountables(User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        Accountable AddAccountable(Accountable newAccountable, User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        [FaultContract(typeof(ConcurrencyException<Accountable>))]
        Accountable UpdateAccountable(Accountable accountableToUpdate, User user, DbConcurencyUpdateOptions concurencyUpdateOption);

        [OperationContract]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        List<Project> GetProjects(User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        Project AddProject(Project newProject, User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        [FaultContract(typeof(ConcurrencyException<Project>))]
        Project UpdateProject(Project projectToUpdate, User user, DbConcurencyUpdateOptions concurencyUpdateOption);


        [OperationContract]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        List<BudgetItem> GetBudgetItems(User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        BudgetItem AddBudgetItem(BudgetItem newBudgetItem, User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        [FaultContract(typeof(ConcurrencyException<BudgetItem>))]
        BudgetItem UpdateBudgetItem(BudgetItem budgetItemToUpdate, User user, DbConcurencyUpdateOptions concurencyUpdateOption);

        [OperationContract]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        List<Rate> GetRates(User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        Rate AddRate(Rate newRate, User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(NoRightsToChangeDataException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        [FaultContract(typeof(ConcurrencyException<Rate>))]
        Rate UpdateRate(Rate rateToUpdate, User user, DbConcurencyUpdateOptions concurencyUpdateOption);
    }
}
