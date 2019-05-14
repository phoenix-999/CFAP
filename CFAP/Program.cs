using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAP.DataProviderService;
using System.ServiceModel;

namespace CFAP
{
    class Program
    {
        static DataProviderClient DataProviderProxy = new DataProviderClient();
        static User mainUser;
        static void Main(string[] args)
        {
            mainUser = Authenticate();
            //AddUser();
            List<Summary> summaries = GetSummary();

            //AddSummary(summaries.FirstOrDefault());

            UpdateSummary(
                    summaries.FirstOrDefault(),
                    new Accountable()
                        {
                            Id = 2,
                            AccountableName = "Accountable2"
                        },
                    new UserGroup[]
                        {
                            new UserGroup() { Id = 1, GroupName = "MainOffice"},
                            new UserGroup() { Id = 2, GroupName = "Office1"}
                        });
        }

        

        static User Authenticate()
        {
            User user = new User() { UserName = "yurii", Password = "1"};
            User receivedUser = null;
            try
            {
                receivedUser = DataProviderProxy.Authenticate(user);
            }
            catch(FaultException<AutenticateFaultException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
            catch(FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
            catch(TimeoutException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            if (receivedUser != null)
            {
                Console.WriteLine("!!!Аутентификация прошла успешно. Пользователь {0}", receivedUser.UserName);
            }

            return receivedUser;

            
        }

        static void AddUser()
        {

            User user = new User() { UserName = "Liubov", Password = "2", CanAddNewUsers = false, UserGroups = mainUser.UserGroups };
            try
            {
                 DataProviderProxy.AddNewUser(user, mainUser);
                 Console.WriteLine("Пользователь {0} добавлен.", user.UserName);
            }
            catch (FaultException<AutenticateFaultException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
            catch(FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
            
        }

        static List<Summary> GetSummary()
        {
            List<Summary> summaries = null;

            Filter filter = new Filter()
            {
                Projects = new Project[] { new Project() { Id = 1 } },
                Accountables = new Accountable[] { new Accountable() { Id = 3} }
            };

            try
            {
                summaries = DataProviderProxy.GetSummary(mainUser, new Filter()).ToList();

                Console.WriteLine("Полученные данные:");
                foreach (var s in summaries)
                {
                    Console.WriteLine("Poject {0}", s.Project.ProjectName);
                    Console.WriteLine("BudgetItem {0}", s.BudgetItem.ItemName);
                    Console.WriteLine("Accountable {0}", s.Accountable.AccountableName);
                    Console.WriteLine("Descriptin {0}", s.Description.Description);
                    Console.WriteLine();
                }
            }
            catch(FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }

            return summaries;
        }

        static void AddSummary(Summary summary)
        {
            Summary newSummary = new Summary()
            {
                Accountable = summary.Accountable,
                Project = summary.Project,
                BudgetItem = summary.BudgetItem,
                Description = summary.Description,
                SummaGrn = 200
            };

            try
            {
                DataProviderProxy.AddOrUpdateSummary(new Summary[] { newSummary }, mainUser);
                Console.WriteLine("Summary добавлена успешно!");
            }
            catch (FaultException<AutenticateFaultException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
            catch (FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
        }

        static void UpdateSummary(Summary summary, Accountable accountable, UserGroup[] userGroups)
        {
            summary.SummaGrn = 1000;
            summary.Accountable = accountable;
            summary.UserGroups = userGroups;

            try
            {
                DataProviderProxy.AddOrUpdateSummary(new Summary[] { summary }, mainUser);
                Console.WriteLine("Summary обновлена успешно!");
            }
            catch (FaultException<AutenticateFaultException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
            catch (FaultException<DataNotValidException> ex)
            {
                Console.WriteLine("Ошибки валидации при добавлении сущностией:");
                foreach (var err in ex.Detail.ValidationErrors)
                {
                    Console.WriteLine("Свойство {0} - {1}", err.Key, err.Value);
                }
            }

            catch (FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }

        }
    }
}
