using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using UnitTest.DataProviderService;
using System.Collections.Generic;
using System.Linq;
using CFAPDataModel;

namespace UnitTest
{
    /// <summary>
    /// ТЕСТИРОВАНИЕ ПРОВОДИТЬ ТОЛЬКО НА ТЕСТОВОЙ БАЗУ ДАННЫХ ДЛЯ ПРЕДОТВРАЩЕНИЯ ПОВРЕЖДЕНИЯ ДАННЫХ ПРИ ТЕСТОВЫХ СБОЯХ
    /// </summary>

    [TestClass]
    public class DataProviderTest
    {
        User MainUser;

        DataProviderClient DataProviderProxy = new DataProviderClient();
        #region CONSTANTS

        const string ADMIN_USER_NAME = "yurii";
        const string ADMIN_USER_PASSWORD = "1";

        const string USER_NOT_ADMIN_NAME = "Liubov";
        const string USER_NOT_ADMIN_PASSWORD = "2";

        const string TEST_USER_NAME = "TestUserName";
        const string TEST_USER_PASSWORD = "TestUserPassword";

        const int MAIN_OFFICE_ID = 1;
        const string MAIN_OFFICE = "MainOffice";

        const int OFFICE1_ID = 2;
        const string OFFICE1 = "Office1"; //Удаляемая группа

        const int OFFICE2_ID = 45;
        const string OFFICE2 = "Office2"; //Добавляемая группа

        const int PROJECT1_ID = 1;
        const int PROJECT2_ID = 36;

        const int ACCOUNTABLE1_ID = 1;
        const string ACCOUNTABLE1 = "Accountable1";

        const int ACCOUNTABLE2_ID = 40;
        const string ACCOUNTABLE2 = "Accountable2";

        const int BUDGET_ITEM1_ID = 1;

        const int DESCRIPTION1_ID = 2;

        #endregion

        #region Authenticate
        [TestMethod]
        public void Authenticate()
        {
            User user = new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD };
            MainUser = DataProviderProxy.Authenticate(user);

            User correctUser = new User()
            {
                UserName = user.UserName
                ,CanAddNewUsers = true
                ,IsAdmin = true
            };

            Assert.IsNotNull(MainUser);
            Assert.AreEqual(MainUser.UserName, correctUser.UserName);
            Assert.AreEqual(MainUser.CanAddNewUsers, correctUser.CanAddNewUsers);
            Assert.AreEqual(MainUser.IsAdmin, correctUser.IsAdmin);
        }

        [TestMethod]
        public void Authenticate_UserGroups()
        {
            User user = new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD };
            MainUser = DataProviderProxy.Authenticate(user);

            List<UserGroup> correctedGroups = new List<UserGroup>();

            UserGroup mainOffice = new UserGroup{ Id = 1, GroupName = MAIN_OFFICE, CanUserAllData = true };
            correctedGroups.Add(mainOffice);

            UserGroup office1 = new UserGroup { Id = 2, GroupName = OFFICE1, CanUserAllData = false };
            correctedGroups.Add(office1);

            Assert.IsNotNull(MainUser);
            Assert.IsNotNull(MainUser.UserGroups);
            Assert.AreNotEqual(MainUser.UserGroups.Length, 0);
            
            foreach (var groupUser in MainUser.UserGroups)
            {
                UserGroup currentCorrectedGroup = (from g in correctedGroups where g.Id == groupUser.Id select g).FirstOrDefault();

                Assert.IsNotNull(currentCorrectedGroup);

                Assert.AreEqual(currentCorrectedGroup.GroupName, groupUser.GroupName);
                Assert.AreEqual(currentCorrectedGroup.CanUserAllData, groupUser.CanUserAllData);
            }

        }

        [TestMethod]
        public void Authenticate_ArgumentNullExceptions()
        {
            User user = new User();

            Assert.ThrowsException<FaultException<ArgumentNullException>>(()=> { DataProviderProxy.Authenticate(user); });
        }

        [TestMethod]
        public void Authenticate_AuthenticateFaultException()
        {
            User user = new User() { UserName = "not name", Password = "not password" };

            Assert.ThrowsException<FaultException<AuthenticateFaultException>>(()=> { DataProviderProxy.Authenticate(user); });
        }
        #endregion

        #region AddNewUser
        [TestMethod]
        public void AddNewUser()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            User newUser = new User()
            {
                UserName = TEST_USER_NAME
                ,Password = TEST_USER_PASSWORD
                ,UserGroups = new UserGroup[] { new UserGroup() { Id = OFFICE1_ID } }
            };

            DataProviderProxy.AddNewUser(newUser, owner);

            using (CFAPContext ctx = new CFAPContext())
            {
                var addedUser = (from u in ctx.Users
                                 where u.UserName == newUser.UserName
                                 select u
                                 ).FirstOrDefault();
                Assert.AreNotEqual(addedUser, null);

                UserGroup[] correctedUserGroups = new UserGroup[]
                {
                    new UserGroup() { Id = OFFICE1_ID}
                };

                foreach (var addedGroup in addedUser.UserGroups)
                {
                    var correctedGroup = (from g in correctedUserGroups where addedGroup.Id == g.Id select g).FirstOrDefault();
                    Assert.AreNotEqual(correctedGroup, null);
                }

                ctx.Users.Remove(addedUser);
                ctx.SaveChanges();

                var removedUser = (from u in ctx.Users
                                   where u.UserName == newUser.UserName
                                   select u
                                 ).FirstOrDefault();
                Assert.AreEqual(removedUser, null);
            }

        }

        [TestMethod]
        public void AddNewUser_AuthentacateFaultException()
        {
            User owner  = new User() { UserName = "otherUser", Password = "otherPassword" };

            User newUser = new User()
            {
                UserName = TEST_USER_NAME
                ,
                Password = TEST_USER_PASSWORD
                ,
                UserGroups = new UserGroup[] { new UserGroup() { Id = OFFICE1_ID } }
            };

            Assert.ThrowsException<FaultException<AuthenticateFaultException>>(()=> { DataProviderProxy.AddNewUser(newUser, owner); });

            using (CFAPContext ctx = new CFAPContext())
            {
                var user = (from u in ctx.Users where u.UserName == newUser.UserName select u).FirstOrDefault();

                Assert.AreEqual(user, null);
            }
        }

        [TestMethod]
        public void AddNewUser_UserHasNotGroupsException()
        {
            User owner = new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD };
            owner = DataProviderProxy.Authenticate(owner);

            User newUser = new User()
            {
                UserName = TEST_USER_NAME,
                Password = TEST_USER_PASSWORD
            };

            Assert.ThrowsException<FaultException<UserHasNotGroupsException>>(() => { DataProviderProxy.AddNewUser(newUser, owner); });

            using (CFAPContext ctx = new CFAPContext())
            {
                var user = (from u in ctx.Users where u.UserName == newUser.UserName select u).FirstOrDefault();

                Assert.AreEqual(user, null);
            }
        }

        [TestMethod]
        public void AddNewUser_Duplicate()
        {
            User owner = new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD };
            owner = DataProviderProxy.Authenticate(owner);

            Assert.ThrowsException<FaultException<DbException>>(() => { DataProviderProxy.AddNewUser(owner, owner); });

            using (CFAPContext ctx = new CFAPContext())
            {
                var users = (from u in ctx.Users where u.UserName == owner.UserName select u).ToArray();

                Assert.AreEqual(users.Length, 1);
            }
        }

        [TestMethod]
        public void AddNewUser_UserNotAdminException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Assert.AreEqual(owner.CanAddNewUsers, false);

            User testUser = new User() { UserName = TEST_USER_NAME, Password = TEST_USER_PASSWORD, UserGroups = new UserGroup[] { new UserGroup { Id = OFFICE1_ID } } };

            Assert.ThrowsException<FaultException<AddUserNotAdminException>>(()=> { DataProviderProxy.AddNewUser(testUser, owner); });

            using (CFAPContext ctx = new CFAPContext())
            {
                var user = (from u in ctx.Users where u.UserName == testUser.UserName select u).FirstOrDefault();

                Assert.AreEqual(user, null);
            }
        }

        [TestMethod]
        public void AddNewUser_DataNotValidException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            Assert.AreEqual(owner.CanAddNewUsers, true);

            User testUser = new User() { Password = TEST_USER_PASSWORD,  UserGroups = new UserGroup[] { new UserGroup { Id = OFFICE1_ID } } };

            var errors = Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.AddNewUser(testUser, owner); });
            

            using (CFAPContext ctx = new CFAPContext())
            {
                var user = (from u in ctx.Users where u.UserName == testUser.UserName select u).FirstOrDefault();

                Assert.AreEqual(user, null);
            }
        }

        #endregion

        #region UpdateUser
        [TestMethod]
        public void UpdateUser()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD } );
            User userForUpdate = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD});

            var oldUserName = userForUpdate.UserName;
            userForUpdate.UserName = "updateUser";
            var oldPassword = userForUpdate.Password;
            userForUpdate.Password = null;
            var oldIsAdmin = userForUpdate.IsAdmin;
            userForUpdate.IsAdmin = true;
            UserGroup[] oldGroups = (from g in userForUpdate.UserGroups select g).ToArray();
            userForUpdate.UserGroups = new UserGroup[] { new UserGroup() { Id = OFFICE2_ID, GroupName = OFFICE2 } };

            DataProviderProxy.UpdateUser(userForUpdate, owner);

            using (CFAPContext ctx = new CFAPContext())
            {

                var updatedUser = (from user in ctx.Users where user.UserName == userForUpdate.UserName select user).Single();
                ctx.Users.Include("UserGroups");

                Assert.AreEqual(updatedUser.IsAdmin, userForUpdate.IsAdmin);
                Assert.AreEqual(updatedUser.UserName, userForUpdate.UserName);
                Assert.AreEqual(updatedUser.Password, oldPassword);
                Assert.AreEqual(updatedUser.CanAddNewUsers, userForUpdate.CanAddNewUsers);

                //Проверка добавления новых групп
                foreach (var newGroup in userForUpdate.UserGroups)
                {
                    var correctGroup = (from g in updatedUser.UserGroups where g.Id == newGroup.Id select g).FirstOrDefault();
                    Assert.AreNotEqual(correctGroup, null);
                    //Проверка удаления старой группы
                    Assert.AreNotEqual(correctGroup.Id, userForUpdate.UserGroups[0]);
                }

                var canUseAllDataGroups = (from g in ctx.UserGroups where g.CanUserAllData == true select g).ToArray();
                
                //Изменение Id обновленного пользователя, так как при изменении данных пользователя Id меняется
                userForUpdate.Id = updatedUser.Id;
            }

            userForUpdate.UserName = oldUserName;
            userForUpdate.Password = oldPassword;
            userForUpdate.IsAdmin = oldIsAdmin;
            userForUpdate.UserGroups = oldGroups;

            DataProviderProxy.UpdateUser(userForUpdate, owner);
        }

        [TestMethod]
        public void UpdateUser_AddUserNotAdminException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });
            User userForUpdate = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Assert.ThrowsException<FaultException<AddUserNotAdminException>>(()=> { DataProviderProxy.UpdateUser(userForUpdate, owner); });
        }

        [TestMethod]
        public void UpdateUser_UserHasNotGroupException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            User userForUpdate = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });
            userForUpdate.UserGroups = null;

            Assert.ThrowsException<FaultException<UserHasNotGroupsException>>(() => { DataProviderProxy.UpdateUser(userForUpdate, owner); });

            userForUpdate.UserGroups = new UserGroup[] { };

            Assert.ThrowsException<FaultException<UserHasNotGroupsException>>(() => { DataProviderProxy.UpdateUser(userForUpdate, owner); });
        }

        [TestMethod]
        public void UpdateUser_DatNotValidException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            User userForUpdate = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });
            userForUpdate.UserName = "";

            Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.UpdateUser(userForUpdate, owner); });

            userForUpdate.UserName = null;

            Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.UpdateUser(userForUpdate, owner); });
        }

        #endregion

        #region AddSummary
        [TestMethod]
        public void AddSummary()
        {
            Summary newSummary = new Summary()
            {
                SummaUAH = 200,
                SummaryDate = DateTime.Now,
                Accountable = new Accountable() { Id = ACCOUNTABLE1_ID },
                Project = new Project() { Id = PROJECT1_ID },
                BudgetItem = new BudgetItem() { Id = BUDGET_ITEM1_ID },
                Description = new DescriptionItem() { Id = DESCRIPTION1_ID }
            };

            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD});

            Summary addedSummary = DataProviderProxy.AddSummary(newSummary, user);

            Assert.AreNotEqual(addedSummary.Id, 0);
            Assert.AreNotEqual(addedSummary.ActionDate, null);
            Assert.AreEqual(addedSummary.Accountable.Id, newSummary.Accountable.Id);
            Assert.AreEqual(addedSummary.Project.Id, newSummary.Project.Id);
            Assert.AreEqual(addedSummary.BudgetItem.Id, newSummary.BudgetItem.Id);
            Assert.AreEqual(addedSummary.Description.Id, newSummary.Description.Id);

            foreach (var summaryGroup in addedSummary.UserGroups)
            {
                var correctedGroup = (from g in user.UserGroups where g.Id == summaryGroup.Id || summaryGroup.CanUserAllData select summaryGroup).FirstOrDefault();
                Assert.AreNotEqual(correctedGroup, null);
            }

            using (CFAPContext ctx = new CFAPContext())
            {
                var summaryToRemove = (from s in ctx.Summaries where s.Id == addedSummary.Id select s).Single();
                ctx.Entry(summaryToRemove).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
        }

        [TestMethod]
        public void AddSummary_DataNotValidException()
        {
            Summary newSummary = new Summary()
            {
                SummaUAH = 200,
                SummaryDate = DateTime.Now,
                Accountable = new Accountable() { Id = ACCOUNTABLE1_ID },
                Project = new Project() { Id = PROJECT1_ID },
                BudgetItem = new BudgetItem() { Id = BUDGET_ITEM1_ID },
                Description = null
            };

            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.AddSummary(newSummary, user); });

            newSummary.Accountable = new Accountable();
            newSummary.Project = new Project();
            newSummary.BudgetItem = new BudgetItem();
            newSummary.Description = new DescriptionItem();

            Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.AddSummary(newSummary, user); });
        }
        #endregion

        #region GetSummaries

        [TestMethod]
        public void GetSummaries()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD});
            Filter filter = new Filter()
            {
                Projects = new Project[] { new Project() { Id = PROJECT2_ID } },
                DateStart = DateTime.Parse("19.05.2019")

            };

            Summary[] summaries = DataProviderProxy.GetSummary(user, filter);
            Assert.AreNotEqual(summaries.Length, 0);

            
            var correctedProjectsId = (from p in filter.Projects select p.Id).ToList();
            var correctedUserGroupsId = (from u in user.UserGroups select u.Id).ToList();
            foreach (var summary in summaries)
            {
                var hasCorrectedProject = correctedProjectsId.Contains(summary.Project.Id);
                Assert.AreEqual(hasCorrectedProject, true);

                var hasCorrectedPeriod = summary.SummaryDate >= filter.DateStart;
                Assert.AreEqual(hasCorrectedPeriod, true);

                var numbersOfCorrectedGroups = 0;
                foreach (var userGroup in summary.UserGroups)
                {
                    var hasCorrectedUserGroup = correctedUserGroupsId.Contains(userGroup.Id);
                    if (hasCorrectedUserGroup || user.UserGroups.Where(g => g.CanUserAllData).FirstOrDefault() != null)
                        numbersOfCorrectedGroups++;  
                }
                Assert.AreEqual(numbersOfCorrectedGroups, user.UserGroups.Length);
            }

            

        }

        #endregion

        #region UpdateSummary

        [TestMethod]
        public void UpdateSummary()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            Summary[] summaries = DataProviderProxy.GetSummary(user, null);
            var summariesCanWrite = (from s in summaries where s.ReadOnly == false select s).ToList();

            Summary summaryToUpdate = summariesCanWrite[0];

            var oldUser = summaryToUpdate.UserLastChanged;
            var oldSumma = summaryToUpdate.SummaUAH;
            summaryToUpdate.SummaUAH = -1;
            Accountable oldAccountable = summaryToUpdate.Accountable;
            summaryToUpdate.Accountable = new Accountable() { Id = ACCOUNTABLE2_ID, AccountableName = ACCOUNTABLE2 };

            Summary updatedSummary = DataProviderProxy.UpdateSummary(summaryToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

            Assert.AreNotEqual(updatedSummary, null);
            Assert.AreEqual(updatedSummary.Id, summaryToUpdate.Id);
            Assert.AreEqual(updatedSummary.Accountable.Id, summaryToUpdate.Accountable.Id);
            Assert.AreEqual(updatedSummary.Project.Id, summaryToUpdate.Project.Id);
            Assert.AreEqual(updatedSummary.BudgetItem.Id, summaryToUpdate.BudgetItem.Id);
            Assert.AreEqual(updatedSummary.Description.Id, summaryToUpdate.Description.Id);
            Assert.AreEqual(updatedSummary.SummaUAH, summaryToUpdate.SummaUAH);
            Assert.AreEqual(updatedSummary.UserLastChanged.Id, user.Id);

            var correctedUserGroupsId = (from g in user.UserGroups select g.Id).ToList();
            foreach (var userGroup in updatedSummary.UserGroups)
            {
                var hasCorrectedGroups = correctedUserGroupsId.Contains(userGroup.Id)
                    || userGroup.CanUserAllData
                    || user.UserGroups.Where(g=>g.CanUserAllData).FirstOrDefault() != null;
                Assert.AreEqual(hasCorrectedGroups, true);
            }

            updatedSummary.SummaUAH = oldSumma;
            updatedSummary.Accountable = oldAccountable;
            updatedSummary.UserLastChanged = oldUser;

            DataProviderProxy.UpdateSummary(updatedSummary, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

        }

        [TestMethod]
        public void UpdateSummary_TryChangeReadOnlyFileds()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            Summary[] summaries = DataProviderProxy.GetSummary(user, null);

            var summariesCanReadOnly = (from s in summaries where s.ReadOnly == true select s).ToList();

            Assert.AreNotEqual(summariesCanReadOnly.Count, 0);

            Summary summaryToUpdate = summariesCanReadOnly[0];

            Assert.ThrowsException<FaultException<TryChangeReadOnlyFiledException>>(()=> { DataProviderProxy.UpdateSummary(summaryToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.None); });
        }

        [TestMethod]
        public void UpdateSummary_DataNotValidException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Summary[] summaries = DataProviderProxy.GetSummary(user, null);
            var summariesCanWrite = (from s in summaries where s.ReadOnly == false select s).ToList();

            Summary summaryToUpdate = summariesCanWrite[0];
            summaryToUpdate.Accountable = null;
            summaryToUpdate.Project = null;
            summaryToUpdate.BudgetItem = null;
            summaryToUpdate.Description = null;
            summaryToUpdate.UserGroups = null;

            Assert.ThrowsException<FaultException<DataNotValidException>>(()=> { DataProviderProxy.UpdateSummary(summaryToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.None); });
        }

        [TestMethod]
        public void UpdateSummary_ConcurencyException()
        {
            User user = DataProviderProxy.Authenticate(new User() {UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Summary[] summaries = DataProviderProxy.GetSummary(user, null);
            var summariesCanWrite = (from s in summaries where s.ReadOnly == false select s).ToList();

            Summary summaryToUpdate = summariesCanWrite[0];

            var oldUser = summaryToUpdate.UserLastChanged;
            var oldSumma = summaryToUpdate.SummaUAH;
            summaryToUpdate.SummaUAH = -1;
            Accountable oldAccountable = summaryToUpdate.Accountable;
            summaryToUpdate.Accountable = new Accountable() { Id = ACCOUNTABLE2_ID, AccountableName = ACCOUNTABLE2 };

            //Имитация изменения сущности в БД после ее получения другим пользователем
            Summary updatedSummary = DataProviderProxy.UpdateSummary(summaryToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);
            summaryToUpdate.SummaUAH = -2;

            Assert.ThrowsException<FaultException<ConcurrencyExceptionOfSummarydxjYbbDT>>(()=> { DataProviderProxy.UpdateSummary(summaryToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.None); });

            var databaseSummary = DataProviderProxy.UpdateSummary(summaryToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.DatabasePriority);

            Assert.AreNotEqual(databaseSummary, null);
            Assert.AreEqual(databaseSummary.Id, summaryToUpdate.Id);
            Assert.AreEqual(databaseSummary.SummaUAH, -1);

            updatedSummary.SummaUAH = oldSumma;
            updatedSummary.Accountable = oldAccountable;
            updatedSummary.UserLastChanged = oldUser;

            DataProviderProxy.UpdateSummary(updatedSummary, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);
        }

        #endregion

        #region RemoveSummary

        [TestMethod]
        public void RemoveSummary()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Summary[] summaries = DataProviderProxy.GetSummary(user, null);

            Summary testSummary = summaries[0];
            testSummary.SummaUAH = 0;
            testSummary.SummaryDate = DateTime.Now;

            Summary addedSummary = DataProviderProxy.AddSummary(testSummary, user);

            using (CFAPContext ctx = new CFAPContext())
            {
                var checkAddedSummary = (from s in ctx.Summaries where s.Id == addedSummary.Id select s).Distinct().ToArray();
                Assert.AreEqual(1, checkAddedSummary.Length);
            }

            int numberRemovedSummary = DataProviderProxy.RemoveSummary(addedSummary, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

            Assert.AreNotEqual(0, numberRemovedSummary);

            using (CFAPContext ctx = new CFAPContext())
            {
                var checkRemovedSummary = (from s in ctx.Summaries where s.Id == addedSummary.Id select s).ToArray();

                Assert.AreEqual(0, checkRemovedSummary.Length);
            }
        }

        [TestMethod]
        public void RemoveSummary_TryChangeReadOnlyFileds()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            Summary[] summaries = DataProviderProxy.GetSummary(user, null);

            Summary testSummary = summaries[0];
            testSummary.SummaUAH = 0;
            testSummary.SummaryDate = DateTime.Now;
            testSummary.ReadOnly = true;

            Summary addedSummary = DataProviderProxy.AddSummary(testSummary, user);

            using (CFAPContext ctx = new CFAPContext())
            {
                var checkAddedSummary = (from s in ctx.Summaries where s.Id == addedSummary.Id select s).Distinct().ToArray();
                Assert.AreEqual(1, checkAddedSummary.Length);
            }

            Assert.ThrowsException<FaultException<TryChangeReadOnlyFiledException>>(()=> { DataProviderProxy.RemoveSummary(addedSummary, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority); }) ;

            using (CFAPContext ctx = new CFAPContext())
            {
                var testSummryToRemove = (from s in ctx.Summaries where s.Id == addedSummary.Id select s).First();

                ctx.Summaries.Remove(testSummryToRemove);
                ctx.SaveChanges();
            }
        }


        [TestMethod]
        public void RemoveSummary_ConcurencyException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Summary[] summaries = DataProviderProxy.GetSummary(user, null);

            Summary testSummary = summaries[0];
            testSummary.SummaUAH = 0;
            testSummary.SummaryDate = DateTime.Now;

            Summary addedSummary = DataProviderProxy.AddSummary(testSummary, user);

            using (CFAPContext ctx = new CFAPContext())
            {
                var checkAddedSummary = (from s in ctx.Summaries where s.Id == addedSummary.Id select s).Distinct().ToArray();
                Assert.AreEqual(1, checkAddedSummary.Length);
            }

            //Имитация изменения сущности в БД после ее получения другим пользователем
            Summary updatedSummary = DataProviderProxy.UpdateSummary(addedSummary, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);
            addedSummary.SummaUAH = -2;

            Assert.ThrowsException<FaultException<ConcurrencyExceptionOfSummarydxjYbbDT>>(() => { DataProviderProxy.RemoveSummary(addedSummary, user, DataProviderService.DbConcurencyUpdateOptions.None); });

            //При выбраном режиме работы удаление сущностей не должно произойти
            Assert.ThrowsException<FaultException<InvalidOperationException>>(()=> { DataProviderProxy.RemoveSummary(addedSummary, user, DataProviderService.DbConcurencyUpdateOptions.DatabasePriority); }) ;

            using (CFAPContext ctx = new CFAPContext())
            {
                var testSummryToRemove = (from s in ctx.Summaries where s.Id == addedSummary.Id select s).First();

                ctx.Summaries.Remove(testSummryToRemove);
                ctx.SaveChanges();
            }
        }

        #endregion

    }
}
