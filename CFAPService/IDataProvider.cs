using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CFAPDataModel.Models;
using CFAPService.Faults;


namespace CFAPService
{
    [ServiceContract]
    interface IDataProvider
    {
        [OperationContract]
        [FaultContract(typeof(AutenticateFaultException))]
        [FaultContract(typeof(DbException))]
        User AuthentificateUser(User user);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(AutenticateFaultException))]
        [FaultContract(typeof(DbException))]
        [FaultContract(typeof(AddUserNotAdminException))]
        void AddNewUser(User newUser, User owner);
    }
}
