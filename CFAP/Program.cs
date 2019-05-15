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
            //AddSummaries(summaries.FirstOrDefault(), mainUser);
            UpdateSummary(summaries.ToArray(), mainUser);
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

        static void AddSummaries(Summary summary, User user)
        {
            Console.WriteLine("Добавление summaries");
            Summary s1 = new Summary()
            {
                Accountable = summary.Accountable,
                Project = summary.Project,
                BudgetItem = summary.BudgetItem,
                Description = summary.Description,
                SummaGrn = 100                
            };
            Summary s2 = new Summary()
            {
                Accountable = summary.Accountable,
                Project = summary.Project,
                BudgetItem = summary.BudgetItem,
                Description = summary.Description,
                SummaGrn = 300
            };

            try
            {
                DataProviderProxy.ChangeSummaries(new Summary[] { s1, s2}, mainUser);
                Console.WriteLine("Summaries добавлены");
            }
            catch (FaultException<AutenticateFaultException> ex)
            {
                Console.WriteLine("Ошибка аутентификации");
                Console.WriteLine(ex.Detail.Message);
            }
            catch (FaultException<DataNotValidException> ex)
            {
                Console.WriteLine("Ошибки валидации:");
                foreach (var e in ex.Detail.ValidationErrors)
                {
                    Console.WriteLine(e.Key + " => " + e.Value);
                }
            }
            catch (FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
        }
        

        static void UpdateSummary(Summary[] summaries, User user)
        {
            Console.WriteLine("Обновление summary");

            summaries[0].SummaGrn = 1000;
            summaries[0].Project = new Project() { Id=36, ProjectName="Project3" };

            summaries[1].SummaGrn = 2000;
            summaries[1].Project = new Project() { Id = 37, ProjectName = "Project2" };

            try
            {
                DataProviderProxy.ChangeSummaries(summaries, mainUser);
                Console.WriteLine("Summaries обновлены");
            }
            catch (FaultException<AutenticateFaultException> ex)
            {
                Console.WriteLine("Ошибка аутентификации");
                Console.WriteLine(ex.Detail.Message);
            }
            catch (FaultException<DataNotValidException> ex)
            {
                Console.WriteLine("Ошибки валидации:");
                foreach (var e in ex.Detail.ValidationErrors)
                {
                    Console.WriteLine(e.Key + " => " + e.Value);
                }
            }
            catch (FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
        }
    }
}
