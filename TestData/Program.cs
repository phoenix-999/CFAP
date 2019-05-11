using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAPDataModel.Models;
using CFAPDataModel;

namespace TestData
{
    class Program
    {
        static void Main(string[] args)
        {
            AddStartData();
        }

        static void AddStartData()
        {
            Rate rate1 = new Rate() { DateRate = DateTime.Now, Dolar = 27.0 };
            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Rates.Add(rate1);
                ctx.SaveChanges();
            }

            UserGroup userGroup = new UserGroup() { GroupName = "MainOffice" };
            UserGroup userGroup1 = new UserGroup() { GroupName = "Office1" };

            User user1 = new User() { UserName = "yurii", Password = "1", CanAddNewUsers = true, UserGroups = new UserGroup[] { userGroup, userGroup1} }; // NWoZK3kTsExUV00Ywo1G5jlUKKs=
            user1.EncriptPassword();
            Summary summary1 = new Summary() { SummaGrn = 54, UserGroups = new UserGroup[] { userGroup}, Project = new Project() { ProjectName = "Project1"} };
            Summary summary2 = new Summary() { SummaGrn = 0, UserGroups = new UserGroup[] { userGroup, userGroup1 } };
            summary1.SetSummaDollar();

            using (CFAPContext ctx = new CFAPContext())
            {
                ctx.Users.Add(user1);
                ctx.Summaries.Add(summary1);
                ctx.Summaries.Add(summary2);
                ctx.SaveChanges();

                User user = (from u in ctx.Users
                             where u.Id == 1
                             select u).FirstOrDefault();

                user.UserName = "Admin";
            }
        }
    }
}
