using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAP.DataProviderClient;
using System.ServiceModel;

namespace CFAP
{
    class BusinessLogicEvents
    {
        public static User User { get; private set; }
        DataProviderClient.DataProviderClient DataProviderProxy;

        public BusinessLogicEvents()
        {
            DataProviderProxy = new DataProviderClient.DataProviderClient();
            InitializeEventsHandlers();
        }

        void InitializeEventsHandlers()
        {
            this.GetLoginsEvent += GetLoginsHandler;
            this.AuthenticateEvent += AuthenticateHandler;
        }

        #region Class Inteface

        public List<string> GetLogins()
        {
            return this.GetLoginsEvent();
        }

        public void Authenticate(string login, string password)
        {
            BusinessLogicEvents.User = this.AuthenticateHandler(login, password);
        }

        #endregion

        #region Events

        event Func<List<string>> GetLoginsEvent;
        event Func<string, string, User> AuthenticateEvent;
        #endregion

        #region EventsHandlers

        List<string> GetLoginsHandler()
        {
            List<string> logins = new List<string>();

            try
            {
                logins = DataProviderProxy.GetLogins().ToList();
            }
            catch (FaultException<DbException> fault)
            {
                ExceptionsHandler.DbExceptionHandler(fault);
            }
            catch (FaultException fault)
            {
                ExceptionsHandler.FaultExceptionHandler(fault);
            }
            catch (TimeoutException ex)
            {
                ExceptionsHandler.TimeOutExceptionExceptionHandler(ex);
            }
            catch (CommunicationException ex)
            {
                ExceptionsHandler.CommunicationExceptionHandler(ex);
            }

            return logins;
        }

        User AuthenticateHandler(string login, string password)
        {
            User result = null;

            User userForAuthenticate = new User() { UserName = login, Password = password };

            try
            {
                result = DataProviderProxy.Authenticate(userForAuthenticate);
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
            }
            catch (FaultException<ArgumentNullException> ex)
            {
                ExceptionsHandler.ArgumentNullExceptionHandler(ex);
            }
            catch (FaultException<DbException> fault)
            {
                ExceptionsHandler.DbExceptionHandler(fault);
            }
            catch (FaultException fault)
            {
                ExceptionsHandler.FaultExceptionHandler(fault);
            }
            catch (TimeoutException ex)
            {
                ExceptionsHandler.TimeOutExceptionExceptionHandler(ex);
            }
            catch (CommunicationException ex)
            {
                ExceptionsHandler.CommunicationExceptionHandler(ex);
            }

            return result;
        }

        #endregion

        

        
    }
}
