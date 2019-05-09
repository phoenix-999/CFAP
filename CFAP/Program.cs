using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAPDataModel;
using CFAPDataModel.Models;
using CFAP.DataProviderService;
using System.ServiceModel;

namespace CFAP
{
    class Program
    {
        static DataProviderClient DataProviderProxy = new DataProviderClient();
        static void Main(string[] args)
        {
            Authenticate();
            //AddUser();
            //Validate();
        }

        static void AddStartData()
        {
            Rate rate1 = new Rate() { DateRate = DateTime.Now, Dolar = 27.0 };
            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Rates.Add(rate1);
                ctx.SaveChanges();
            }


            User user1 = new User() { UserName = "yurii", Password = "1", IsAdmin = true };
            Summary summary1 = new Summary() { SummaGrn = 54 };
            summary1.SetSummaDollar();

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Users.Add(user1);
                ctx.Summaries.Add(summary1);
                ctx.SaveChanges();

                User user = (from u in ctx.Users
                             where u.Id == 1
                             select u).FirstOrDefault();

                user.UserName = "Admin";
            }
        }

        static void Authenticate()
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
                Console.WriteLine("!!!Аутентификация прошла успешно");
            }
        }

        static void AddUser()
        {
            User ovner = new User() { UserName = "yurii", Password = "1", IsAdmin = true };
            User user = new User() { UserName = "Liubov", Password = "2", IsAdmin = false };
            DataProviderProxy.AddNewUser(user, ovner);
        }

        static void Validate()
        {
            User user = new User() { UserName = "yurii", Password = "1", IsAdmin = true };
            Summary summary = new Summary() {SummaGrn = 27, Users = new List<User>() {user} };
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
