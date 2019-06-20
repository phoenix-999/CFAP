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

        DataProviderClient.DataProviderClient DataProviderProxy;

        public static User User { get; private set; }
        public static List<User> UsersData { get; private set; }
        public static List<UserGroup> UserGroups { get; private set; }
        public static List<Accountable> Accountables { get; private set; }
        public static List<BudgetItem> BudgetItems { get; private set; }
        public static List<Project> Projects { get; private set; }
        public static List<Rate> Rates { get; private set; }
        public static List<Summary> Summaries { get; set; }
        public static Filter CurrentFilter { get; set; }

        public static Balance BalanceBeginningPeriod { get; set; }

        public static Balance Incomming
        {
            get
            {
                Balance result = new Balance();

                if (Summaries == null)
                    return result;

                result.BalanceUAH = Summaries.Where(s => s.CashFlowType == true).Sum(s => s.SummaUAH);
                result.BalanceUSD = Summaries.Where(s => s.CashFlowType == true).Sum(s => s.SummaUSD);

                return result;
            }
        }

        public static Balance Expense
        {
            get
            {
                Balance result = new Balance();

                if (Summaries == null)
                    return result;

                result.BalanceUAH = Summaries.Where(s => s.CashFlowType == false).Sum(s => s.SummaUAH);
                result.BalanceUSD = Summaries.Where(s => s.CashFlowType == false).Sum(s => s.SummaUSD);

                return result;
            }
        }

        static CFAPBusinessLogic()
        {
            Summaries = new List<Summary>();
            BalanceBeginningPeriod = new Balance() { BalanceUAH = 0, BalanceUSD = 0};
        }

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

            if (result != null)
            {
                CFAPBusinessLogic.User = result;
                LoadTotalData();
            }
            
        }

        public void LoadTotalData()
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

            LoadAccountables();
            LoadBudgetItems();
            LoadProjects();
            LoadRates();
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

        public void AddUserGroup(UserGroup newGroup)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        UserGroup addedUserGroup = DataProviderProxy.AddNewUserGroup(newGroup, CFAPBusinessLogic.User);
                        CFAPBusinessLogic.UserGroups.Add(addedUserGroup);
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

        public void UpdateUserGroup(UserGroup userGroupForUpdate)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        DataProviderProxy.UpdateUserGroup(userGroupForUpdate, CFAPBusinessLogic.User);
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
                        User updatedUser = DataProviderProxy.UpdateUser(userForUpdate, CFAPBusinessLogic.User);

                        if (userForUpdate.Id == CFAPBusinessLogic.User.Id)
                        {
                            CFAPBusinessLogic.User = updatedUser;
                        }

                        for (int userIndex = 0; userIndex < CFAPBusinessLogic.UsersData.Count; userIndex++)
                        {
                            if (CFAPBusinessLogic.UsersData[userIndex].Id == userForUpdate.Id)
                            {
                                CFAPBusinessLogic.UsersData[userIndex] = updatedUser;
                            }
                        }

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

        public void LoadAccountables()
        {
            if (CFAPBusinessLogic.User.IsAccountable)
            {
                CFAPBusinessLogic.Accountables = new List<Accountable>() { CFAPBusinessLogic.User.Accountable};
                return;
            }

            try
            {
                CFAPBusinessLogic.Accountables = DataProviderProxy.GetAccountables(CFAPBusinessLogic.User).ToList();
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
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

        public void AddAccountable(Accountable newAccountable)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Accountable addedAccountable = DataProviderProxy.AddAccountable(newAccountable, CFAPBusinessLogic.User);
                        CFAPBusinessLogic.Accountables.Add(addedAccountable);
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

        public void UpdateAccountable(Accountable accountableToUpdate, DbConcurencyUpdateOptions updateOption = DbConcurencyUpdateOptions.None)
        {
            //Изменена структура выполнения транзакции по причине запуска новой транзакции с обработчика исключения FaultException<ConcurrencyExceptionOf...>
            try
            {
                try
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        Accountable updatedAccountable = DataProviderProxy.UpdateAccountable(accountableToUpdate, CFAPBusinessLogic.User, updateOption);

                        for (int accountableIndex = 0; accountableIndex < CFAPBusinessLogic.Accountables.Count; accountableIndex++)
                        {
                            if (CFAPBusinessLogic.Accountables[accountableIndex].Id == updatedAccountable.Id)
                            {
                                CFAPBusinessLogic.Accountables[accountableIndex] = updatedAccountable;
                            }
                        }


                        transaction.Complete();
                    }
                }
                catch (TransactionAbortedException ex)
                {
                    ExceptionsHandler.TransactionAbortedExceptionHandler(ex);
                }
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
            catch (FaultException<TryChangeReadOnlyFiledException> fault)
            {
                ExceptionsHandler.TryChangeReadOnlyFieldExceptionHandler(fault);
            }
            catch (FaultException<ConcurrencyExceptionOfAccountabledxjYbbDT> fault)
            {
                ExceptionsHandler.ConcurrencyExceptionAccountablesHandler(fault);
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

        public void LoadBudgetItems()
        {
            try
            {
                CFAPBusinessLogic.BudgetItems = DataProviderProxy.GetBudgetItems(CFAPBusinessLogic.User).ToList();
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
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

        public void AddBudgetItem(BudgetItem newBudgetItem)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        BudgetItem addedBudgetItem = DataProviderProxy.AddBudgetItem(newBudgetItem, CFAPBusinessLogic.User);
                        CFAPBusinessLogic.BudgetItems.Add(addedBudgetItem);
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

        public void UpdateBudgetItem(BudgetItem budgetItemToUpdate, DbConcurencyUpdateOptions updateOption = DbConcurencyUpdateOptions.None)
        {
            //Изменена структура выполнения транзакции по причине запуска новой транзакции с обработчика исключения FaultException<ConcurrencyExceptionOf...>
            try
            {
                try
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        BudgetItem updatedBudgetItem = DataProviderProxy.UpdateBudgetItem(budgetItemToUpdate, CFAPBusinessLogic.User, updateOption);

                        for (int budgetItemIndex = 0; budgetItemIndex < CFAPBusinessLogic.BudgetItems.Count; budgetItemIndex++)
                        {
                            if (CFAPBusinessLogic.BudgetItems[budgetItemIndex].Id == updatedBudgetItem.Id)
                            {
                                CFAPBusinessLogic.BudgetItems[budgetItemIndex] = updatedBudgetItem;
                            }
                        }


                        transaction.Complete();
                    }
                }
                catch (TransactionAbortedException ex)
                {
                    ExceptionsHandler.TransactionAbortedExceptionHandler(ex);
                }
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
            catch (FaultException<TryChangeReadOnlyFiledException> fault)
            {
                ExceptionsHandler.TryChangeReadOnlyFieldExceptionHandler(fault);
            }
            catch (FaultException<ConcurrencyExceptionOfBudgetItemdxjYbbDT> fault)
            {
                ExceptionsHandler.ConcurrencyExceptionBudgetItemsHandler(fault);
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

        public void LoadProjects()
        {
            try
            {
                CFAPBusinessLogic.Projects = DataProviderProxy.GetProjects(CFAPBusinessLogic.User).ToList();
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
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

        public void AddProject(Project newProject)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Project addedProject = DataProviderProxy.AddProject(newProject, CFAPBusinessLogic.User);
                        CFAPBusinessLogic.Projects.Add(addedProject);
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

        public void UpdateProject(Project projectToUpdate, DbConcurencyUpdateOptions updateOption = DbConcurencyUpdateOptions.None)
        {
            //Изменена структура выполнения транзакции по причине запуска новой транзакции с обработчика исключения FaultException<ConcurrencyExceptionOf...>
            try
            {
                try
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        Project updatedProject = DataProviderProxy.UpdateProject(projectToUpdate, CFAPBusinessLogic.User, updateOption);

                        for (int projectIndex = 0; projectIndex < CFAPBusinessLogic.Projects.Count; projectIndex++)
                        {
                            if (CFAPBusinessLogic.Projects[projectIndex].Id == updatedProject.Id)
                            {
                                CFAPBusinessLogic.Projects[projectIndex] = updatedProject;
                            }
                        }


                        transaction.Complete();
                    }
                }
                catch (TransactionAbortedException ex)
                {
                    ExceptionsHandler.TransactionAbortedExceptionHandler(ex);
                }
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
            catch (FaultException<TryChangeReadOnlyFiledException> fault)
            {
                ExceptionsHandler.TryChangeReadOnlyFieldExceptionHandler(fault);
            }
            catch (FaultException<ConcurrencyExceptionOfProjectdxjYbbDT> fault)
            {
                ExceptionsHandler.ConcurrencyExceptionProjectsHandler(fault);
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

        public void LoadRates()
        {
            try
            {
                CFAPBusinessLogic.Rates = DataProviderProxy.GetRates(CFAPBusinessLogic.User).ToList();
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
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

        public void AddRate(Rate newRate)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Rate addedRate = DataProviderProxy.AddRate(newRate, CFAPBusinessLogic.User);
                        CFAPBusinessLogic.Rates.Add(addedRate);
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

        public void UpdateRate(Rate rateToUpdate, DbConcurencyUpdateOptions updateOption = DbConcurencyUpdateOptions.None)
        {
            //Изменена структура выполнения транзакции по причине запуска новой транзакции с обработчика исключения FaultException<ConcurrencyExceptionOf...>
            try
            {
                try
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        Rate updatedRate = DataProviderProxy.UpdateRate(rateToUpdate, CFAPBusinessLogic.User, updateOption);

                        for (int rateIndex = 0; rateIndex < CFAPBusinessLogic.Rates.Count; rateIndex++)
                        {
                            if (CFAPBusinessLogic.Rates[rateIndex].Id == updatedRate.Id)
                            {
                                CFAPBusinessLogic.Rates[rateIndex] = updatedRate;
                            }
                        }


                        transaction.Complete();
                    }
                }
                catch (TransactionAbortedException ex)
                {
                    ExceptionsHandler.TransactionAbortedExceptionHandler(ex);
                }
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
            catch (FaultException<TryChangeReadOnlyFiledException> fault)
            {
                ExceptionsHandler.TryChangeReadOnlyFieldExceptionHandler(fault);
            }
            catch (FaultException<ConcurrencyExceptionOfRatedxjYbbDT> fault)
            {
                ExceptionsHandler.ConcurrencyExceptionRatesHandler(fault);
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

        public void LoadSummaries(Filter filter)
        {
            if (CFAPBusinessLogic.User.IsAccountable)
            {
                if (filter == null)
                    filter = new Filter();

                filter.Accountables = new Accountable[] { CFAPBusinessLogic.User.Accountable };
            }

            try
            {
                if (DataProviderProxy.State == CommunicationState.Faulted)
                {
                    DataProviderProxy = new DataProviderClient.DataProviderClient();
                }

                CFAPBusinessLogic.CurrentFilter = filter;
                CFAPBusinessLogic.Summaries = DataProviderProxy.GetSummary(CFAPBusinessLogic.User, filter).ToList();
                LoadBalanceBeginningPeriod(filter);
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
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

        public void ChangeReadOnlySummary(bool onOff, Filter filter)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        DataProviderProxy.ChangeSummaryReadOnlyStatus(onOff, filter, CFAPBusinessLogic.User);
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

        public void AddSummary(Summary newSummary)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        Summary addedSummary = DataProviderProxy.AddSummary(newSummary, CFAPBusinessLogic.User);
                        CFAPBusinessLogic.Summaries.Add(addedSummary);
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
                    catch (FaultException<DataNotValidException> fault)
                    {
                        ExceptionsHandler.DataNotValidExceptionHandler(fault);
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

        public void UpdateSummary(Summary summaryToUpdate, DbConcurencyUpdateOptions updateOption = DbConcurencyUpdateOptions.None)
        {
            //Изменена структура выполнения транзакции по причине запуска новой транзакции с обработчика исключения FaultException<ConcurrencyExceptionOf...>
            try
            {
                try
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        Summary updatedSummary = DataProviderProxy.UpdateSummary(summaryToUpdate, CFAPBusinessLogic.User, updateOption);

                        for (int summaryIndex = 0; summaryIndex < CFAPBusinessLogic.Summaries.Count; summaryIndex++)
                        {
                            if (CFAPBusinessLogic.Summaries[summaryIndex].Id == updatedSummary.Id)
                            {
                                CFAPBusinessLogic.Summaries[summaryIndex] = updatedSummary;
                            }
                        }


                        transaction.Complete();
                    }
                }
                catch (TransactionAbortedException ex)
                {
                    ExceptionsHandler.TransactionAbortedExceptionHandler(ex);
                }
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
            }
            catch (FaultException<DbException> fault)
            {
                ExceptionsHandler.DbExceptionHandler(fault);
            }
            catch (FaultException<DataNotValidException> fault)
            {
                ExceptionsHandler.DataNotValidExceptionHandler(fault);
            }
            catch (FaultException<TryChangeReadOnlyFiledException> fault)
            {
                ExceptionsHandler.TryChangeReadOnlyFieldExceptionHandler(fault);
            }
            catch (FaultException<ConcurrencyExceptionOfSummarydxjYbbDT> fault)
            {
                ExceptionsHandler.ConcurrencyExceptionSummariesHandler(fault, UpdateDeleteOptions.Update);
            }
            catch (FaultException<FiledDeletedException> fault)
            {
                ExceptionsHandler.FiledDeletedExceptionHandler(fault);
                CFAPBusinessLogic.Summaries.Remove(summaryToUpdate);
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

        public void RemoveSummary(Summary summaryToRemove, DbConcurencyUpdateOptions updateOption = DbConcurencyUpdateOptions.None)
        {
            //Изменена структура выполнения транзакции по причине запуска новой транзакции с обработчика исключения FaultException<ConcurrencyExceptionOf...>
            try
            {
                try
                {
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        Summary removedSummary = DataProviderProxy.RemoveSummary(summaryToRemove, CFAPBusinessLogic.User, updateOption);

                        for (int summaryIndex = 0; summaryIndex < CFAPBusinessLogic.Summaries.Count; summaryIndex++)
                        {
                            if (CFAPBusinessLogic.Summaries[summaryIndex].Id == removedSummary.Id)
                            {
                                CFAPBusinessLogic.Summaries.RemoveAt(summaryIndex);
                            }
                        }


                        transaction.Complete();
                    }
                }
                catch (TransactionAbortedException ex)
                {
                    ExceptionsHandler.TransactionAbortedExceptionHandler(ex);
                }
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
            }
            catch (FaultException<DbException> fault)
            {
                ExceptionsHandler.DbExceptionHandler(fault);
            }
            catch (FaultException<DataNotValidException> fault)
            {
                ExceptionsHandler.DataNotValidExceptionHandler(fault);
            }
            catch (FaultException<TryChangeReadOnlyFiledException> fault)
            {
                ExceptionsHandler.TryChangeReadOnlyFieldExceptionHandler(fault);
            }
            catch (FaultException<ConcurrencyExceptionOfSummarydxjYbbDT> fault)
            {
                ExceptionsHandler.ConcurrencyExceptionSummariesHandler(fault, UpdateDeleteOptions.Delete);
            }
            catch (FaultException<FiledDeletedException> fault)
            {
                ExceptionsHandler.FiledDeletedExceptionHandler(fault);
                CFAPBusinessLogic.Summaries.Remove(summaryToRemove);
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

        public void LoadBalanceBeginningPeriod(Filter filter)
        {
            try
            {
                CFAPBusinessLogic.CurrentFilter = filter;
                CFAPBusinessLogic.BalanceBeginningPeriod = DataProviderProxy.GetBalanceBeginningPeriod(CFAPBusinessLogic.User, filter);
            }
            catch (FaultException<AuthenticateFaultException> fault)
            {
                ExceptionsHandler.AuthenticateFaultExceptionHandler(fault);
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
