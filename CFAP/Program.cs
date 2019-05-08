using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAPDataModel;
using CFAPDataModel.Models;

namespace CFAP
{
    class Program
    {
        static void Main(string[] args)
        {
            User user1 = new User() { UserName = "yurii", Password = "1", IsAdmin = true};
            Rate rate1 = new Rate() { DateRate = DateTime.Now, Dolar = 27.0 };
            Summary summary1 = new Summary() {SummaGrn = 1};
            summary1.SetSummaDollar();
            
            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Users.Add(user1);
                ctx.Rates.Add(rate1);
                ctx.Summaries.Add(summary1);
                ctx.SaveChanges();

                User user = (from u in ctx.Users
                            where u.Id == 1
                            select u).FirstOrDefault();

                user.UserName = "Admin";
                ctx.SaveChanges();

                
            }
        }
    }
}
