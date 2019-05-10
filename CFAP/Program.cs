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
        static void Main(string[] args)
        {
            AddUser();
            Authenticate();
            Validate();
        }

        

        static void Authenticate()
        {
            User user = new User() { UserName = "Liubov", Password = "2"};
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

            Console.WriteLine("User {0} owners:", user.UserName);

            if (receivedUser.Owners != null)
            {
                foreach (var userOwner in receivedUser.Owners)
                {
                    Console.WriteLine(userOwner.UserName);
                }
            }
        }

        static void AddUser()
        {
            User owner = new User() { UserName = "yurii", Password = "1", IsAdmin = true };
            User user = new User() { UserName = "Liubov", Password = "2", IsAdmin = false };
            try
            {
                owner = DataProviderProxy.Authenticate(owner);
                if (owner != null)
                    DataProviderProxy.AddNewUser(user, owner);
            }
            catch (FaultException<AutenticateFaultException> ex)
            {
                Console.WriteLine(ex.Detail.Message);
            }
        }

        static void Validate()
        {
            User user = new User() { UserName = "yurii", Password = "1", IsAdmin = true };
            Summary summary = new Summary() {SummaGrn = 27, Users = new User[]{user} };
            IDictionary<string, string> validationErrors = DataProviderProxy.Validate(user);
            Console.WriteLine("Ошибки валидации:");
            if (validationErrors.Count == 0)
                Console.WriteLine("Валидация прошла успешно.");
            foreach(var error in validationErrors)
            {
                Console.WriteLine("Property = {0} --- value={1}", error.Key, error.Value);
            }
        }
    }
}
