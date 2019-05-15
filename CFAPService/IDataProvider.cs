using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CFAPDataModel.Models;
using CFAPService.Faults;
using System.Data.Entity.Validation;


namespace CFAPService
{
    [ServiceContract]
    interface IDataProvider
    {
        [OperationContract]
        [FaultContract(typeof(AutenticateFaultException))]
        [FaultContract(typeof(DbException))]
        User Authenticate(User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AutenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(AddUserNotAdminException))]
        [FaultContract(typeof(DataNotValidException))]
        void AddNewUser(User newUser, User owner);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AutenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(AddUserNotAdminException))]
        [FaultContract(typeof(DataNotValidException))]
        void UpdateUser(User userForUpdate, User owner);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AutenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(DataNotValidException))]
        [FaultContract(typeof(TryChangeReadOnlyFiledException))]
        void ChangeSummaries(List<Summary> summaries, User user);

        [OperationContract]
        [FaultContract(typeof(DbException))]
        HashSet<Summary> GetSummary(User user, Filter filter);
    }
}
