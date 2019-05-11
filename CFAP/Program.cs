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
            Validate();
            GetData();
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

        static void GetData()
        {
            Filter filter = new Filter()
            {
                Projects = new Project[] { new Project() { Id = 1 } }
            };

            try
            {
                List<Summary> summaries = DataProviderProxy.GetSummary(mainUser, new Filter()).ToList();

                Console.WriteLine("Полученные данные:");
                foreach (var s in summaries)
                {
                    Console.WriteLine(s.Id);
                }
            }
            catch(FaultException<DbException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
        }
    }
}
