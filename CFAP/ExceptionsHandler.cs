﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CFAP.DataProviderClient;
using System.Windows.Forms;
using System.Transactions;

namespace CFAP
{
    abstract class ExceptionsHandler
    {

        public abstract void AuthenticateFaultExceptionHandler(FaultException<AuthenticateFaultException> fault);

        public abstract void DbExceptionHandler(FaultException<DbException> fault);

        public abstract void ArgumentNullExceptionHandler(FaultException<ArgumentNullException> fault);

        public abstract void FaultExceptionHandler(FaultException fault);

        public abstract void CommunicationExceptionHandler(CommunicationException ex);

        public abstract void TimeOutExceptionExceptionHandler(TimeoutException ex);

        public abstract void NoRightsToChangeDataExceptionHandler(FaultException<NoRightsToChangeDataException> ex);

        public abstract void DataNotValidExceptionHandler(FaultException<DataNotValidException> fault);

        public abstract void UserHasNotGroupsExceptionHandler(FaultException<UserHasNotGroupsException> fault);

        public abstract void TransactionAbortedExceptionHandler(TransactionAbortedException ex);
    }
}
