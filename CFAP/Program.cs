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
        static User secondUser;
        static void Main(string[] args)
        {
            mainUser = Authenticate("yurii", "1");
            secondUser = Authenticate("Liubov", "2"); //2kuSN7rMzfGcB2DKt67EqDWQELA=
            //AddUser();
            List<Summary> summaries = GetSummary();
            //AddSummaries(summaries.FirstOrDefault(), mainUser);
            summaries = GetSummary();
            //UpdateSummaries(summaries.ToArray(), mainUser);

            AddSummary(summaries[0], mainUser);
            //UpdateSummary(summaries[1], mainUser);
            //DeleteSummary(summaries[1], mainUser);



            //UpdateUser(secondUser, mainUser);
        }

        static void UpdateUser(User user, User owner)
        {
            user.IsAdmin = true;
            var userGroup1 = new UserGroup() { Id = 2, GroupName = "Office1" };
            user.UserGroups = new UserGroup[] { userGroup1 };
            user.Password = "2";

            try
            {
                DataProviderProxy.UpdateUser(user, mainUser);
                Console.WriteLine("Пользователь {0} обновлен.", user.UserName);
            }
            catch (FaultException<AuthenticateFaultException> ex)
            {
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

        static void UpdateSummary(Summary oldSummary, User user)
        {
            oldSummary.SummaUAH = 5000;
            oldSummary.Project = new Project() { Id = 36, ProjectName = "Project3" };
            oldSummary.SummaryDate = DateTime.Now.AddDays(1);
            oldSummary.IsModified = true;

            try
            {
                var result = DataProviderProxy.UpdateSummary(oldSummary, mainUser, DbConcurencyUpdateOptions.None);

                oldSummary.SummaUAH = 10000;
                //oldSummary.RowVersion = result.RowVersion;
                DataProviderProxy.UpdateSummary(oldSummary, mainUser, DbConcurencyUpdateOptions.None);

                Console.WriteLine("Summary обновлена");
            }
            catch (FaultException<AuthenticateFaultException> ex)
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
            catch (FaultException<ConcurrencyExceptionOfSummarydxjYbbDT> ex)
            {
                Console.WriteLine("Ошибка оптимистичного парралелизма:");
                Console.WriteLine("Значение в БД: {0}", ex.Detail.DatabaseValue.SummaUAH);
                Console.WriteLine("Значение в сейчас: {0}", ex.Detail.CurrentValue.SummaUAH);
            }
            catch (FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
        }

        static void DeleteSummary(Summary oldSummary, User user)
        {
            try
            {
                var result = DataProviderProxy.UpdateSummary(oldSummary, mainUser, DbConcurencyUpdateOptions.None);

                DataProviderProxy.RemoveSummary(oldSummary, mainUser, DbConcurencyUpdateOptions.None);

                Console.WriteLine("Summary удалена");
            }
            catch (FaultException<AuthenticateFaultException> ex)
            {
                Console.WriteLine("Ошибка аутентификации");
                Console.WriteLine(ex.Detail.Message);
            }
            catch (FaultException<ConcurrencyExceptionOfSummarydxjYbbDT> ex)
            {
                Console.WriteLine("Ошибка оптимистичного парралелизма:");
                Console.WriteLine("Значение в БД: {0}", ex.Detail.DatabaseValue.SummaUAH);

                if (ex.Detail.CurrentValue != null)
                    Console.WriteLine("Значение в сейчас: {0}", ex.Detail.CurrentValue.SummaUAH);
            }
            catch (FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
        }

        static void AddSummary(Summary anySummary, User user)
        {
            Console.WriteLine("Добавление одной summary");
            Summary s1 = new Summary()
            {
                Accountable = anySummary.Accountable,
                Project = anySummary.Project,
                BudgetItem = anySummary.BudgetItem,
                Description = anySummary.Description,
                SummaUAH = 400,
                UserGroups = new UserGroup[] { user.UserGroups[1] },
                IsModified = true,
                SummaryDate = DateTime.Now
            };

            try
            {
                var receivedSummary = DataProviderProxy.AddSummary(s1, mainUser);
                Console.WriteLine("Summary {0} добавлена", receivedSummary.Id);
            }
            catch (FaultException<AuthenticateFaultException> ex)
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

        static User Authenticate(string userName, string password)
        {
            Console.WriteLine("Аутентификация пользователя {0}", userName);

            User user = new User() { UserName = userName, Password = password };
            User receivedUser = null;
            try
            {
                receivedUser = DataProviderProxy.Authenticate(user);
            }
            catch (FaultException<ArgumentNullException> ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(FaultException<AuthenticateFaultException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
            catch (FaultException<DataNotValidException> ex)
            {
                Console.WriteLine("Ошибка валидации:");
                foreach (var err in ex.Detail.ValidationErrors)
                {
                    Console.WriteLine("{0} => {1}", err.Key, err.Value);
                }
            }
            catch (FaultException<DbException> ex)
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
            catch (FaultException<AuthenticateFaultException> ex)
            {
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
                SummaUAH = 100,
                UserGroups = new UserGroup[] { user.UserGroups[1] },
                IsModified = true
                ,SummaryDate = DateTime.Now                
            };
            Summary s2 = new Summary()
            {
                Accountable = summary.Accountable,
                Project = summary.Project,
                BudgetItem = summary.BudgetItem,
                Description = summary.Description,
                SummaUAH = 300,
                IsModified = true
                ,SummaryDate = DateTime.Now
            };

            try
            {
                DataProviderProxy.AlterSummaries(new Summary[] { s1, s2}, mainUser);
                Console.WriteLine("Summaries добавлены");
            }
            catch (FaultException<AuthenticateFaultException> ex)
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
        

        static void UpdateSummaries(Summary[] summaries, User user)
        {
            Console.WriteLine("Обновление summary");

            summaries[0].SummaUAH = 1000;
            summaries[0].Project = new Project() { Id=36, ProjectName="Project3" };
            summaries[0].IsModified = true;
            summaries[0].SummaryDate = DateTime.Now.AddDays(1);
            summaries[0].UserLastChanged = new User() { Id = 2, UserName = "Liubov", Password = "2kuSN7rMzfGcB2DKt67EqDWQELA=" };

            summaries[1].SummaUAH = 2000;
            summaries[1].Project = new Project() { Id = 37, ProjectName = "Project2" };
            summaries[1].IsModified = true;

            try
            {
                DataProviderProxy.AlterSummaries(summaries, mainUser);
                Console.WriteLine("Summaries обновлены");
            }
            catch (FaultException<AuthenticateFaultException> ex)
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
            catch (FaultException<TryChangeReadOnlyFiledException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }

        }
    }
}
