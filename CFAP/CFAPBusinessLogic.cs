using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAP.DataProviderClient;
using System.ServiceModel;

namespace CFAP
{
    class CFAPBusinessLogic
    {
        public static User User { get; private set; }
        DataProviderClient.DataProviderClient DataProviderProxy;
       
        public static List<User> UsersData { get; private set; }


        public CFAPBusinessLogic()
        {
            DataProviderProxy = new DataProviderClient.DataProviderClient();
        }

        public List<string> GetLogins()
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

        public void Authenticate(string login, string password)
        {
            User userForAuthenticate = new User() { UserName = login, Password = password };
            User result = null;
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

            CFAPBusinessLogic.User = result;
        }

        public void LoadUsers()
        {
            try
            {
                UsersData = DataProviderProxy.GetUsers(CFAPBusinessLogic.User).ToList();
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
            }
            catch (FaultException<NoRightsToChangeDataException> fault)
            {
                ExceptionsHandler.NoRightsToChangeDataExceptionHandler(fault);
            }
            catch (FaultException<DbException> fault)
            {
                ExceptionsHandler.DbExceptionHandler(fault);
            }
            catch (FaultException fault)
            {
                ExceptionsHandler.FaultExceptionHandler(fault);
            }
            catch (CommunicationException ex)
            {
                ExceptionsHandler.CommunicationExceptionHandler(ex);
            }
            catch (TimeoutException ex)
            {
                ExceptionsHandler.TimeOutExceptionExceptionHandler(ex);
            }
        }
       
    }
}
