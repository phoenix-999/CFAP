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
        [OperationContract]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(ArgumentNullException))]
        User Authenticate(User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(AddUserNotAdminException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(UserHasNotGroupsException))]
        void AddNewUser(User newUser, User owner);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(AddUserNotAdminException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(UserHasNotGroupsException))]
        void UpdateUser(User userForUpdate, User owner);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        Summary AddSummary(Summary summary, User user);

        [OperationContract]
        [FaultContract(typeof(DbException))]
        HashSet<Summary> GetSummary(User user, Filter filter);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        [FaultContract(typeof(ConcurrencyException<Summary>))]
        Summary UpdateSummary(Summary summary, User user, DbConcurencyUpdateOptions concurencyUpdateOptions);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        [FaultContract(typeof(ConcurrencyException<Summary>))]
        int RemoveSummary(Summary summary, User user, DbConcurencyUpdateOptions concurencyUpdateOptions);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AuthenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        void AlterSummaries(List<Summary> summaries, User user);
    }
}
