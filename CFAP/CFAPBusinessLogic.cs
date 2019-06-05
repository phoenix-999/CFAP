using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAP.DataProviderClient;
using System.ServiceModel;
using System.Transactions;

namespace CFAP
{
    class CFAPBusinessLogic
    {
        public ExceptionsHandler ExceptionsHandler { get; set; }

        public static User User { get; private set; }
        DataProviderClient.DataProviderClient DataProviderProxy;
       
        public static List<User> UsersData { get; private set; }

        public static List<UserGroup> UserGroups { get; private set; }


        public CFAPBusinessLogic(ExceptionsHandler exceptionsHandler)
        {
            this.ExceptionsHandler = exceptionsHandler;
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

            LoadTotalData();
        }

        private void LoadTotalData()
        {
            if (CFAPBusinessLogic.User.CanChangeUsersData)
                LoadUsers();

            if (CFAPBusinessLogic.User.IsAdmin == false)
            {
                CFAPBusinessLogic.UserGroups = CFAPBusinessLogic.User.UserGroups.ToList();
            }
            else
            {
                LoadUserGroups();
            }
        }

        void LoadUserGroups()
        {
            try
            {
                UserGroups = DataProviderProxy.GetUserGroups(CFAPBusinessLogic.User).ToList();
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

        public void AddUser(User newUser)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        User user = DataProviderProxy.AddNewUser(newUser, CFAPBusinessLogic.User);
                        CFAPBusinessLogic.UsersData.Add(user);
                        transaction.Complete();
                    }
                    catch(FaultException<AuthenticateFaultException> fault)
                    {
                        ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
                    }
                    catch (FaultException<DbException> fault)
                    {
                        ExceptionsHandler.DbExceptionHandler(fault);
                    }
                    catch(FaultException<NoRightsToChangeDataException> fault)
                    {
                        ExceptionsHandler.NoRightsToChangeDataExceptionHandler(fault);
                    }
                    catch(FaultException<DataNotValidException> fault)
                    {
                        ExceptionsHandler.DataNotValidExceptionHandler(fault);
                    }
                    catch (FaultException<UserHasNotGroupsException> fault)
                    {
                        ExceptionsHandler.UserHasNotGroupsExceptionHandler(fault);
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
            catch (TransactionAbortedException ex)
            {
                ExceptionsHandler.TransactionAbortedExceptionHandler(ex);
            }
        }

        public void UpdateUser(User userForUpdate)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        DataProviderProxy.UpdateUser(userForUpdate, CFAPBusinessLogic.User);
                        transaction.Complete();
                    }
                    catch (FaultException<AuthenticateFaultException> fault)
                    {
                        ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
                    }
                    catch (FaultException<DbException> fault)
                    {
                        ExceptionsHandler.DbExceptionHandler(fault);
                    }
                    catch (FaultException<NoRightsToChangeDataException> fault)
                    {
                        ExceptionsHandler.NoRightsToChangeDataExceptionHandler(fault);
                    }
                    catch (FaultException<DataNotValidException> fault)
                    {
                        ExceptionsHandler.DataNotValidExceptionHandler(fault);
                    }
                    catch (FaultException<UserHasNotGroupsException> fault)
                    {
                        ExceptionsHandler.UserHasNotGroupsExceptionHandler(fault);
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
            catch (TransactionAbortedException ex)
            {
                ExceptionsHandler.TransactionAbortedExceptionHandler(ex);
            }
        }

    }
}
