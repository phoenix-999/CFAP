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
            //Validate();
            List<Summary> summaries = GetSummary();

            AddSummary(summaries.FirstOrDefault());

            UpdateSummary(summaries.FirstOrDefault(), new Accountable() { Id = 1, AccountableName = "Accountable1"});
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

        static void Validate()
        {
            Summary summary = new Summary() {SummaGrn = 27, UserGroups = mainUser.UserGroups };
            IDictionary<string, string> validationErrors = DataProviderProxy.Validate(mainUser);
            Console.WriteLine("Ошибки валидации:");
            if (validationErrors.Count == 0)
                Console.WriteLine("Валидация прошла успешно.");
            foreach(var error in validationErrors)
            {
                Console.WriteLine("Property = {0} --- value={1}", error.Key, error.Value);
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
                summaries = DataProviderProxy.GetSummary(mainUser, filter).ToList();

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

        static void UpdateSummary(Summary summary, Accountable accountable)
        {
            summary.SummaGrn = 1000;
            summary.Accountable = accountable;

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
