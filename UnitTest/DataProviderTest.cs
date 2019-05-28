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

        const string PROJECT1 = "Project1";
        const int PROJECT1_ID = 39;

        const string PROJECT2 = "Project2";
        const int PROJECT2_ID = 40;

        const int ACCOUNTABLE1_ID = 71;
        const string ACCOUNTABLE1 = "Accountable1";

        const int ACCOUNTABLE2_ID = 79;
        const string ACCOUNTABLE2 = "Accountable2";

        const int BUDGET_ITEM1_ID = 39;
        const string BUDGET_ITEM1 = "BudgetItem1";

        #endregion

        #region GetLogins

        [TestMethod]
        public void GetLogins()
        {
            string[] logins = DataProviderProxy.GetLogins();

            Assert.AreNotEqual(0, logins.Length);

            using (CFAPContext ctx = new CFAPContext())
            {
                var correctedLogins = (from u in ctx.Users select u.UserName).ToArray();
                HashSet<string> correctedLoginsSet = new HashSet<string>(correctedLogins);

                Assert.AreEqual(correctedLoginsSet.Count, logins.Length);

                foreach (var login in correctedLoginsSet)
                {
                    var hasCorrectedLogin = correctedLogins.Contains(login);
                    Assert.AreEqual(true, hasCorrectedLogin);
                }
            }
        }

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
                ,CanChangeUsersData = true
                ,IsAdmin = true
            };

            Assert.IsNotNull(MainUser);
            Assert.AreEqual(MainUser.UserName, correctUser.UserName);
            Assert.AreEqual(MainUser.CanChangeUsersData, correctUser.CanChangeUsersData);
            Assert.AreEqual(MainUser.IsAdmin, correctUser.IsAdmin);
        }

        [TestMethod]
        public void Authenticate_UserGroups()
        {
            User user = new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD };
            MainUser = DataProviderProxy.Authenticate(user);

            List<UserGroup> correctedGroups = new List<UserGroup>();

            UserGroup mainOffice = new UserGroup{ Id = MAIN_OFFICE_ID, GroupName = MAIN_OFFICE, CanReadAllData = true };
            correctedGroups.Add(mainOffice);
            UserGroup office2 = new UserGroup { Id = OFFICE2_ID, GroupName = "Office2", CanReadAllData = false };
            correctedGroups.Add(office2);

            UserGroup office1 = new UserGroup { Id = 2, GroupName = OFFICE1, CanReadAllData = false };
            correctedGroups.Add(office1);

            Assert.IsNotNull(MainUser);
            Assert.IsNotNull(MainUser.UserGroups);
            Assert.AreNotEqual(MainUser.UserGroups.Length, 0);
            
            foreach (var groupUser in MainUser.UserGroups)
            {
                UserGroup currentCorrectedGroup = (from g in correctedGroups where g.Id == groupUser.Id select g).FirstOrDefault();

                Assert.IsNotNull(currentCorrectedGroup);

                Assert.AreEqual(currentCorrectedGroup.GroupName, groupUser.GroupName);
                Assert.AreEqual(currentCorrectedGroup.CanReadAllData, groupUser.CanReadAllData);
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

            var addedUser = DataProviderProxy.AddNewUser(newUser, owner);

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

            using (CFAPContext ctx = new CFAPContext())
            {
                var userToRemove = (from u in ctx.Users
                                 where u.UserName == newUser.UserName
                                 select u
                                 ).FirstOrDefault();

                ctx.Users.Remove(userToRemove);
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
        public void AddNewUser_NoRightsToChangeDataException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Assert.AreEqual(owner.CanChangeUsersData, false);

            User testUser = new User() { UserName = TEST_USER_NAME, Password = TEST_USER_PASSWORD, UserGroups = new UserGroup[] { new UserGroup { Id = OFFICE1_ID } } };

            Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(()=> { DataProviderProxy.AddNewUser(testUser, owner); });

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

            Assert.AreEqual(owner.CanChangeUsersData, true);

            User testUser = new User() { Password = TEST_USER_PASSWORD,  UserGroups = new UserGroup[] { new UserGroup { Id = OFFICE1_ID } } };

            var errors = Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.AddNewUser(testUser, owner); });
            

            using (CFAPContext ctx = new CFAPContext())
            {
                var user = (from u in ctx.Users where u.UserName == testUser.UserName select u).FirstOrDefault();

                Assert.AreEqual(user, null);
            }
        }

        #endregion

        #region GetUsers
        [TestMethod]
        public void GetUsers()
        {
            //Проверка для админа
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            User[] users = DataProviderProxy.GetUsers(owner);

            Assert.AreNotEqual(0, users.Length);

            using (CFAPContext ctx = new CFAPContext())
            {
                var correctedUsers = (from u in ctx.Users select u).Distinct().ToList();

                foreach (var user in users)
                {
                    var hasCorrectedUser = (from u in correctedUsers where u.Id == user.Id select u).FirstOrDefault();
                    Assert.AreNotEqual(null, hasCorrectedUser);
                    Assert.AreEqual(correctedUsers.Count, users.Length);
                }
            }

            //Проверка для НЕ админа
            User testUser = new User()
            {
                UserName = TEST_USER_NAME,
                Password = TEST_USER_PASSWORD,
                UserGroups = new UserGroup[] { new UserGroup() { Id = OFFICE2_ID, GroupName = OFFICE2 } },
                CanChangeUsersData = true
            };

            testUser = DataProviderProxy.AddNewUser(testUser, owner);

            users = DataProviderProxy.GetUsers(testUser);

            Assert.AreNotEqual(0, users.Length);

            using (CFAPContext ctx = new CFAPContext())
            {
                var correctedUsers = (from u in ctx.Users
                                      from g in u.UserGroups
                                      where g.Id == OFFICE2_ID
                                      select u).Distinct().ToList();

                foreach (var user in users)
                {
                    var hasCorrectedUser = (from u in correctedUsers where u.Id == user.Id select u).FirstOrDefault();
                    Assert.AreNotEqual(null, hasCorrectedUser);
                    Assert.AreEqual(correctedUsers.Count, users.Length);
                }

                //Удаление тестового пользователя

                ctx.Users.Remove(ctx.Users.Find(testUser.Id));
                ctx.SaveChanges();
            }

        }
        [TestMethod]
        public void GetUsers_NoRightsToChangeDataException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            User testUser = new User()
            {
                UserName = TEST_USER_NAME,
                Password = TEST_USER_PASSWORD,
                UserGroups = new UserGroup[] { new UserGroup() { Id = OFFICE2_ID, GroupName = OFFICE2 } },
                CanChangeUsersData = false
            };

            testUser = DataProviderProxy.AddNewUser(testUser, owner);

            try
            {
                Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(() => { DataProviderProxy.GetUsers(testUser); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    //Удаление тестового пользователя
                    ctx.Users.Remove(ctx.Users.Find(testUser.Id));
                    ctx.SaveChanges();
                }
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
            userForUpdate.UserName = "test user";
            var oldPassword = userForUpdate.Password;
            userForUpdate.Password = null;
            var oldIsAdmin = userForUpdate.IsAdmin;
            userForUpdate.IsAdmin = true;
            UserGroup[] oldGroups = (from g in userForUpdate.UserGroups select g).ToArray();
            userForUpdate.UserGroups = new UserGroup[] { new UserGroup() { Id = OFFICE2_ID, GroupName = OFFICE2 } };

            var updatedUser = DataProviderProxy.UpdateUser(userForUpdate, owner);

            Assert.AreEqual(updatedUser.IsAdmin, userForUpdate.IsAdmin);
            Assert.AreEqual(updatedUser.UserName, userForUpdate.UserName);
            Assert.AreEqual(updatedUser.Password, oldPassword);
            Assert.AreEqual(updatedUser.CanChangeUsersData, userForUpdate.CanChangeUsersData);

            //Проверка добавления новых групп
            foreach (var newGroup in userForUpdate.UserGroups)
            {
                var correctGroup = (from g in updatedUser.UserGroups where g.Id == newGroup.Id select g).FirstOrDefault();
                Assert.AreNotEqual(correctGroup, null);
                //Проверка удаления старой группы
                Assert.AreNotEqual(correctGroup.Id, userForUpdate.UserGroups[0]);
            }

            userForUpdate.UserName = oldUserName;
            userForUpdate.Password = oldPassword;
            userForUpdate.IsAdmin = oldIsAdmin;
            userForUpdate.UserGroups = oldGroups;

            DataProviderProxy.UpdateUser(userForUpdate, owner);
        }

        [TestMethod]
        public void UpdateUser_NoRightsToChangeDataException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });
            User userForUpdate = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(()=> { DataProviderProxy.UpdateUser(userForUpdate, owner); });
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

        #region GetUserGroups
        [TestMethod]
        public void GetUserGroups()
        {
            //Проверка для админа
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            UserGroup[] userGroups = DataProviderProxy.GetUserGroups(owner);

            Assert.AreNotEqual(0, userGroups.Length);

            using (CFAPContext ctx = new CFAPContext())
            {
                var correctedUserGroups = (from g in ctx.UserGroups select g).Distinct().ToList();

                foreach (var userGroup in userGroups)
                {
                    var hasCorrectedUserGroup = (from g in correctedUserGroups where g.Id == userGroup.Id select g).FirstOrDefault();
                    Assert.AreNotEqual(null, hasCorrectedUserGroup);
                    Assert.AreEqual(correctedUserGroups.Count, userGroups.Length);
                }
            }
        }
        [TestMethod]
        public void GetUserGroups_NoRightsToChangeDataException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            User testUser = new User()
            {
                UserName = TEST_USER_NAME,
                Password = TEST_USER_PASSWORD,
                UserGroups = new UserGroup[] { new UserGroup() { Id = OFFICE2_ID, GroupName = OFFICE2 } },
                IsAdmin = false
            };

            testUser = DataProviderProxy.AddNewUser(testUser, owner);

            try
            {
                Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(() => { DataProviderProxy.GetUserGroups(testUser); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    //Удаление тестового пользователя
                    ctx.Users.Remove(ctx.Users.Find(testUser.Id));
                    ctx.SaveChanges();
                }
            }
        }

        #endregion

        #region AddNewUserGroup

        [TestMethod]
        public void AddNewUserGroup()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            UserGroup newUserGroup = new UserGroup()
            {
                GroupName = "Test group",
                CanReadAllData = true
            };

            var addedUserGroup = DataProviderProxy.AddNewUserGroup(newUserGroup, owner);

            Assert.AreNotEqual(addedUserGroup, null);


            using (CFAPContext ctx = new CFAPContext())
            {
                var userGroupToRemove = (from g in ctx.UserGroups
                                    where g.GroupName == newUserGroup.GroupName
                                    select g
                                 ).FirstOrDefault();

                ctx.UserGroups.Remove(userGroupToRemove);
                ctx.SaveChanges();

                var removedUserGroup = (from g in ctx.UserGroups
                                   where g.GroupName == newUserGroup.GroupName
                                   select g
                                 ).FirstOrDefault();
                Assert.AreEqual(removedUserGroup, null);
            }

        }

        [TestMethod]
        public void AddNewUserGroup_AuthentacateFaultException()
        {
            User owner = new User() { UserName = "otherUser", Password = "otherPassword" };

            UserGroup newUserGroup = new UserGroup()
            {
                GroupName = "Test group",
                CanReadAllData = true
            };

            Assert.ThrowsException<FaultException<AuthenticateFaultException>>(() => { DataProviderProxy.AddNewUserGroup(newUserGroup, owner); });

            using (CFAPContext ctx = new CFAPContext())
            {
                var userGroup = (from g in ctx.UserGroups where g.GroupName == newUserGroup.GroupName select g).FirstOrDefault();

                Assert.AreEqual(userGroup, null);
            }
        }


        [TestMethod]
        public void AddNewUserGroup_NoRightsToChangeDataException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Assert.AreEqual(owner.IsAdmin, false);

            UserGroup testUserGroup = new UserGroup()
            {
                GroupName = "Test group",
                CanReadAllData = true
            };

            Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(() => { DataProviderProxy.AddNewUserGroup(testUserGroup, owner); });

            using (CFAPContext ctx = new CFAPContext())
            {
                var userGroup = (from g in ctx.UserGroups where g.GroupName == testUserGroup.GroupName select g).FirstOrDefault();

                Assert.AreEqual(userGroup, null);
            }
        }

        [TestMethod]
        public void AddNewUserGroup_DataNotValidException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            Assert.AreEqual(owner.IsAdmin, true);

            UserGroup testUserGroup = new UserGroup()
            {
                GroupName = "",
                CanReadAllData = true
            };

            var errors = Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.AddNewUserGroup(testUserGroup, owner); });


            using (CFAPContext ctx = new CFAPContext())
            {
                var userGroup = (from g in ctx.UserGroups where g.GroupName == testUserGroup.GroupName select g).FirstOrDefault();

                Assert.AreEqual(userGroup, null);
            }
        }

        #endregion

        #region UpdateUserGoup
        [TestMethod]
        public void UpdateUserGroup()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            UserGroup[] userGroups = DataProviderProxy.GetUserGroups(owner);

            UserGroup groupForUpdate = userGroups[0];

            Summary[] summaries = DataProviderProxy.GetSummary(owner, null);
            List<Summary> groupForUpdateSummaries = (from s in summaries
                                               from g in s.UserGroups
                                               where g.Id == groupForUpdate.Id
                                               select s).ToList();
            Assert.AreNotEqual(0, groupForUpdateSummaries.Count);

            User[] users = DataProviderProxy.GetUsers(owner);
            List<User> groupForUpdateUsers = (from u in users
                                                     from g in u.UserGroups
                                                     where g.Id == groupForUpdate.Id
                                                     select u).ToList();
            Assert.AreNotEqual(0, groupForUpdateUsers.Count);

            var oldGrupName = groupForUpdate.GroupName;
            groupForUpdate.GroupName = "UpdateTest Group";

            UserGroup updatedGroup = DataProviderProxy.UpdateUserGroup(groupForUpdate, owner);
            try
            {
                Assert.AreNotEqual(null, updatedGroup);
                Assert.AreEqual(groupForUpdate.Id, updatedGroup.Id);

                using (CFAPContext ctx = new CFAPContext())
                {
                    var isUpdate = (from g in ctx.UserGroups where g.Id == groupForUpdate.Id select g).Single().GroupName == groupForUpdate.GroupName;
                    Assert.AreEqual(true, isUpdate);

                    var updatedGroupSummaries = (from s in ctx.Summaries
                                                 from g in s.UserGroups
                                                 where g.Id == updatedGroup.Id
                                                 select s).ToList();

                    foreach (var correctedSummary in groupForUpdateSummaries)
                    {
                        var hasCorrectedSummary = (from s in updatedGroupSummaries where s.Id == correctedSummary.Id select s).FirstOrDefault() != null;
                        Assert.AreEqual(true, hasCorrectedSummary);
                    }

                    var updatedGroupUsers = (from u in users
                                             from g in u.UserGroups
                                             where g.Id == updatedGroup.Id
                                             select u).ToList();
                    foreach (var correctedUser in groupForUpdateUsers)
                    {
                        var hasCorrectedUser = (from u in updatedGroupUsers where u.Id == correctedUser.Id select u).FirstOrDefault() != null;
                        Assert.AreEqual(true, hasCorrectedUser);
                    }

                }
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var groupForBackData = (from g in ctx.UserGroups where g.Id == groupForUpdate.Id select g).Single();
                    groupForBackData.GroupName = oldGrupName;
                    ctx.Entry(groupForBackData).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
            }




        }

        [TestMethod]
        public void UpdateUserGroup_NoRightsToChangeDataException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            UserGroup groupForUpdate = DataProviderProxy.AddNewUserGroup(new UserGroup() { GroupName = "TestGroup" }, owner);

            try
            {
                owner = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });
                Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(() => { DataProviderProxy.UpdateUserGroup(groupForUpdate, owner); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var groupForRemove = (from g in ctx.UserGroups where g.GroupName == groupForUpdate.GroupName select g).Single();
                    ctx.UserGroups.Remove(groupForRemove);
                    ctx.SaveChanges();
                }
            }
        }


        [TestMethod]
        public void UpdateUserGroup_DatNotValidException()
        {
            User owner = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            UserGroup groupForUpdate = DataProviderProxy.AddNewUserGroup(new UserGroup() { GroupName = "TestGroup" }, owner);
            string oldGroupName = groupForUpdate.GroupName;
            groupForUpdate.GroupName = "";

            try
            {
                Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.UpdateUserGroup(groupForUpdate, owner); });

                groupForUpdate.GroupName = null;

                Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.UpdateUserGroup(groupForUpdate, owner); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var groupForRemove = (from g in ctx.UserGroups where g.GroupName == oldGroupName select g).Single();
                    ctx.UserGroups.Remove(groupForRemove);
                    ctx.SaveChanges();
                }
            }
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
                Description = "test description"
            };

            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD});

            Summary addedSummary = DataProviderProxy.AddSummary(newSummary, user);

            Assert.AreNotEqual(addedSummary.Id, 0);
            Assert.AreNotEqual(addedSummary.ActionDate, null);
            Assert.AreEqual(addedSummary.Accountable.Id, newSummary.Accountable.Id);
            Assert.AreEqual(addedSummary.Project.Id, newSummary.Project.Id);
            Assert.AreEqual(addedSummary.BudgetItem.Id, newSummary.BudgetItem.Id);

            foreach (var summaryGroup in addedSummary.UserGroups)
            {
                var correctedGroup = (from g in user.UserGroups where g.Id == summaryGroup.Id || summaryGroup.CanReadAllData select summaryGroup).FirstOrDefault();
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
                BudgetItem = null,
                Description = "test description"
            };

            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.AddSummary(newSummary, user); });

            newSummary.Accountable = new Accountable();
            newSummary.Project = new Project();
            newSummary.BudgetItem = new BudgetItem();
            newSummary.Description = "test description";

            Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.AddSummary(newSummary, user); });
        }
        #endregion

        #region GetSummaries

        [TestMethod]
        public void GetSummaries()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD});
            Filter filter = new Filter()
            {
                Projects = new Project[] { new Project() { Id = PROJECT2_ID } },
                DateStart = DateTime.Parse("19.05.2019")

            };

            Summary[] summaries = DataProviderProxy.GetSummary(user, filter);
            Assert.AreNotEqual(summaries.Length, 0);

            var correctedProjectsId = (from p in filter.Projects select p.Id).ToList();
            var userGroupsId = (from u in user.UserGroups select u.Id).ToList();
            foreach (var summary in summaries)
            {
                var hasCorrectedProject = correctedProjectsId.Contains(summary.Project.Id);
                Assert.AreEqual(hasCorrectedProject, true);

                var hasCorrectedPeriod = summary.SummaryDate >= filter.DateStart;
                Assert.AreEqual(hasCorrectedPeriod, true);
            }

            using (CFAPContext ctx = new CFAPContext())
            {
                var correctedSummaries = (from s in ctx.Summaries
                                          from g in s.UserGroups
                                          where s.SummaryDate >= filter.DateStart
                                          && s.Project.Id == PROJECT2_ID
                                          && userGroupsId.Contains(g.Id)
                                          select s).Distinct().ToList();

                var numbersCorrectedSummaries = (from s in summaries
                                          from correctedSummary in correctedSummaries
                                          where s.Id == correctedSummary.Id select s).ToArray();

                Assert.AreEqual(numbersCorrectedSummaries.Length, summaries.Length);
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
            Assert.AreEqual(updatedSummary.SummaUAH, summaryToUpdate.SummaUAH);
            Assert.AreEqual(updatedSummary.UserLastChanged.Id, user.Id);

            var correctedUserGroupsId = (from g in user.UserGroups select g.Id).ToList();
            foreach (var userGroup in updatedSummary.UserGroups)
            {
                var hasCorrectedGroups = correctedUserGroupsId.Contains(userGroup.Id)
                    || userGroup.CanReadAllData
                    || user.UserGroups.Where(g=>g.CanReadAllData).FirstOrDefault() != null;
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

            Summary testSummary = new Summary()
            {
                SummaUAH = 200,
                SummaryDate = DateTime.Now,
                Accountable = new Accountable() { Id = ACCOUNTABLE1_ID },
                Project = new Project() { Id = PROJECT1_ID },
                BudgetItem = new BudgetItem() { Id = BUDGET_ITEM1_ID },
                Description = "TEST"
            };
            DataProviderProxy.AddSummary(testSummary, user);

            try
            {
                Summary[] summaries = DataProviderProxy.GetSummary(user, null);

                Summary summaryToUpdate = (from s in summaries where s.Description == testSummary.Description select s).First();

                summaryToUpdate.ReadOnly = true;
                var readOnlySummary = DataProviderProxy.UpdateSummary(summaryToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

                summaryToUpdate.ReadOnly = false;
                Assert.ThrowsException<FaultException<TryChangeReadOnlyFiledException>>(() => { DataProviderProxy.UpdateSummary(summaryToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority); });

            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var summaryToRemove = (from s in ctx.Summaries where s.Description == testSummary.Description select s).Single();
                    ctx.Summaries.Remove(summaryToRemove);
                    ctx.SaveChanges();
                }
            }
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
            testSummary.ReadOnly = false;

            Summary addedSummary = DataProviderProxy.AddSummary(testSummary, user);

            using (CFAPContext ctx = new CFAPContext())
            {
                var checkAddedSummary = (from s in ctx.Summaries where s.Id == addedSummary.Id select s).Distinct().ToArray();
                Assert.AreEqual(1, checkAddedSummary.Length);
            }

            addedSummary.ReadOnly = true;
            DataProviderProxy.UpdateSummary(addedSummary, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

            addedSummary.ReadOnly = false;
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

        #region GetAccountables
        [TestMethod]
        public void GetAccountables()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD});

            Accountable[] accountables = DataProviderProxy.GetAccountables(user);

            using (CFAPContext ctx = new CFAPContext())
            {
                var correctAccountables = (from a in ctx.Accountables select a).Distinct().ToList();

                Assert.AreEqual(correctAccountables.Count, accountables.Length);

                foreach (var accountable in correctAccountables)
                {
                    var correctAccountable = (from a in accountables where a.Id == accountable.Id select a).Single(); //В случае если не найдет или найдет больше одного - исключение
                }
            }
        }
        #endregion

        #region AddAccountable

        [TestMethod]
        public void AddAccountable()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            Accountable newAccountable = new Accountable() { AccountableName = "test accountable" };

            try
            {
                Accountable addedAccountbale = DataProviderProxy.AddAccountable(newAccountable, user);

                Assert.AreNotEqual(null, addedAccountbale);
                Assert.AreNotEqual(0, addedAccountbale.Id);
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var accountableToRemove = (from a in ctx.Accountables where a.AccountableName == newAccountable.AccountableName select a).Single();
                    ctx.Accountables.Remove(accountableToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void AddAccountable_DataNotValidException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            Accountable newAccountable = new Accountable() {  };

            Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.AddAccountable(newAccountable, user); });

                
        }

        [TestMethod]
        public void AddAccountable_NoRightsToChangeDataException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Accountable newAccountable = new Accountable() { };

            Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(() => { DataProviderProxy.AddAccountable(newAccountable, user); });


        }

        #endregion

        #region UpdateAccountable

        [TestMethod]
        public void UpdateAccountable()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD});
            Accountable testAccountable = new Accountable() { AccountableName = "Test accountable" };
            testAccountable = DataProviderProxy.AddAccountable(testAccountable, user);

            Accountable[] accountables = DataProviderProxy.GetAccountables(user);

            Accountable accountableToUpdate = (from a in accountables where a.AccountableName == testAccountable.AccountableName select a).Single();

            accountableToUpdate.AccountableName = "Test UPDATE accountable name";

            try
            {

                Accountable updatedAccountable = DataProviderProxy.UpdateAccountable(accountableToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

                Assert.AreEqual(accountableToUpdate.Id, updatedAccountable.Id);
                Assert.AreEqual(accountableToUpdate.AccountableName, updatedAccountable.AccountableName);

            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var accountableToRemove = (from a in ctx.Accountables where a.AccountableName == accountableToUpdate.AccountableName select a).Single();
                    ctx.Accountables.Remove(accountableToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void UpdateAccountable_ConcurencyException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            Accountable testAccountable = new Accountable() { AccountableName = "Test accountable" };
            testAccountable = DataProviderProxy.AddAccountable(testAccountable, user);

            Accountable[] accountables = DataProviderProxy.GetAccountables(user);

            Accountable accountableToUpdate = (from a in accountables where a.AccountableName == testAccountable.AccountableName select a).Single();

            accountableToUpdate.AccountableName = "Test UPDATE accountable name";

            try
            {
                Accountable updatedAccountable = DataProviderProxy.UpdateAccountable(accountableToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

                Assert.ThrowsException<FaultException<ConcurrencyExceptionOfAccountabledxjYbbDT>>(() => { DataProviderProxy.UpdateAccountable(accountableToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.None); });

            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var accountableToRemove = (from a in ctx.Accountables where a.AccountableName == accountableToUpdate.AccountableName select a).Single();
                    ctx.Accountables.Remove(accountableToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void UpdateAccountable_DataNotValidException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            Accountable testAccountable = new Accountable() { AccountableName = "Test accountable" };
            testAccountable = DataProviderProxy.AddAccountable(testAccountable, user);

            Accountable[] accountables = DataProviderProxy.GetAccountables(user);

            Accountable accountableToUpdate = (from a in accountables where a.AccountableName == testAccountable.AccountableName select a).Single();

            accountableToUpdate.AccountableName = "";

            try
            {
                Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.UpdateAccountable(accountableToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var accountableToRemove = (from a in ctx.Accountables where a.AccountableName == testAccountable.AccountableName select a).Single();
                    ctx.Accountables.Remove(accountableToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void UpdateAccountable_TryChangeReadOnlyFiledsException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            Accountable testAccountable = new Accountable() { AccountableName = "Test accountable" };
            testAccountable = DataProviderProxy.AddAccountable(testAccountable, user);

            Accountable[] accountables = DataProviderProxy.GetAccountables(user);

            Accountable accountableToUpdate = (from a in accountables where a.AccountableName == testAccountable.AccountableName select a).Single();

            accountableToUpdate.AccountableName = "TEst UPDATE Accountable";

            try
            {
                accountableToUpdate.ReadOnly = true;
                var updatedAccountable = DataProviderProxy.UpdateAccountable(accountableToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);
                Assert.ThrowsException<FaultException<TryChangeReadOnlyFiledException>>(() => { DataProviderProxy.UpdateAccountable(accountableToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var accountableToRemove = (from a in ctx.Accountables where a.AccountableName == accountableToUpdate.AccountableName select a).Single();
                    ctx.Accountables.Remove(accountableToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void UpdateAccountable_NoRigthToChangeDataException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            Accountable testAccountable = new Accountable() { AccountableName = "Test accountable" };
            testAccountable = DataProviderProxy.AddAccountable(testAccountable, user);

            Accountable[] accountables = DataProviderProxy.GetAccountables(user);

            Accountable accountableToUpdate = (from a in accountables where a.AccountableName == testAccountable.AccountableName select a).Single();

            accountableToUpdate.AccountableName = "TEst UPDATE Accountable";

            try
            {
                User userNotAdmin = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });
                Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(() => { DataProviderProxy.UpdateAccountable(accountableToUpdate, userNotAdmin, DataProviderService.DbConcurencyUpdateOptions.ClientPriority); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var accountableToRemove = (from a in ctx.Accountables where a.AccountableName == testAccountable.AccountableName select a).Single();
                    ctx.Accountables.Remove(accountableToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        #endregion

        #region GetProjects
        [TestMethod]
        public void GetProjects()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Project[] projects = DataProviderProxy.GetProjects(user);

            using (CFAPContext ctx = new CFAPContext())
            {
                var correctProjects = (from a in ctx.Projects select a).Distinct().ToList();

                Assert.AreEqual(correctProjects.Count, projects.Length);

                foreach (var project in correctProjects)
                {
                    var correctProject = (from a in projects where a.Id == project.Id select a).Single(); //В случае если не найдет или найдет больше одного - исключение
                }
            }
        }
        #endregion

        #region AddProject

        [TestMethod]
        public void AddProject()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            Project newProject = new Project() { ProjectName = "test project" };

            try
            {
                Project addedProject = DataProviderProxy.AddProject(newProject, user);

                Assert.AreNotEqual(null, addedProject);
                Assert.AreNotEqual(0, addedProject.Id);
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var projectToRemove = (from p in ctx.Projects where p.ProjectName == newProject.ProjectName select p).Single();
                    ctx.Projects.Remove(projectToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void AddProject_DataNotValidException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });

            Project newProject = new Project() { };

            Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.AddProject(newProject, user); });


        }

        [TestMethod]
        public void AddProject_NoRightsToChangeDataException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            Project newProject = new Project() { };

            Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(() => { DataProviderProxy.AddProject(newProject, user); });


        }

        #endregion

        #region UpdateProject

        [TestMethod]
        public void UpdateProject()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            Project testProject = new Project() { ProjectName = "Test project" };
            testProject = DataProviderProxy.AddProject(testProject, user);

            Project[] projects = DataProviderProxy.GetProjects(user);

            Project projectToUpdate = (from p in projects where p.ProjectName == testProject.ProjectName select p).Single();

            projectToUpdate.ProjectName = "Test UPDATE project name";

            try
            {

                Project updatedProject = DataProviderProxy.UpdateProject(projectToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

                Assert.AreEqual(projectToUpdate.Id, updatedProject.Id);
                Assert.AreEqual(projectToUpdate.ProjectName, updatedProject.ProjectName);

            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var projectToRemove = (from p in ctx.Projects where p.ProjectName == projectToUpdate.ProjectName select p).Single();
                    ctx.Projects.Remove(projectToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void UpdateProject_ConcurencyException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            Project testProject = new Project() { ProjectName = "Test project" };
            testProject = DataProviderProxy.AddProject(testProject, user);

            Project[] projects = DataProviderProxy.GetProjects(user);

            Project projectToUpdate = (from p in projects where p.ProjectName == testProject.ProjectName select p).Single();

            projectToUpdate.ProjectName = "Test UPDATE project name";

            try
            {
                Project updatedProject = DataProviderProxy.UpdateProject(projectToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

                Assert.ThrowsException<FaultException<ConcurrencyExceptionOfProjectdxjYbbDT>>(() => { DataProviderProxy.UpdateProject(projectToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.None); });

            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var projectToRemove = (from p in ctx.Projects where p.ProjectName == projectToUpdate.ProjectName select p).Single();
                    ctx.Projects.Remove(projectToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void UpdateProject_DataNotValidException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            Project testProject = new Project() { ProjectName = "Test project" };
            testProject = DataProviderProxy.AddProject(testProject, user);

            Project[] projects = DataProviderProxy.GetProjects(user);

            Project projectToUpdate = (from p in projects where p.ProjectName == testProject.ProjectName select p).Single();

            projectToUpdate.ProjectName = "";

            try
            {
                Assert.ThrowsException<FaultException<DataNotValidException>>(() => { DataProviderProxy.UpdateProject(projectToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var projectToRemove = (from p in ctx.Projects where p.ProjectName == testProject.ProjectName select p).Single();
                    ctx.Projects.Remove(projectToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void UpdateProject_TryChangeReadOnlyFiledsException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            Project testProject = new Project() { ProjectName = "Test project" };
            testProject = DataProviderProxy.AddProject(testProject, user);

            Project[] projects = DataProviderProxy.GetProjects(user);

            Project projectToUpdate = (from p in projects where p.ProjectName == testProject.ProjectName select p).Single();

            projectToUpdate.ProjectName = "TEst UPDATE Project";

            try
            {
                projectToUpdate.ReadOnly = true;
                var updatedProject = DataProviderProxy.UpdateProject(projectToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);
                Assert.ThrowsException<FaultException<TryChangeReadOnlyFiledException>>(() => { DataProviderProxy.UpdateProject(projectToUpdate, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var projectToRemove = (from p in ctx.Projects where p.ProjectName == projectToUpdate.ProjectName select p).Single();
                    ctx.Projects.Remove(projectToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void UpdateProject_NoRigthToChangeDataException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD });
            Project testProject = new Project() { ProjectName = "Test project" };
            testProject = DataProviderProxy.AddProject(testProject, user);

            Project[] projects = DataProviderProxy.GetProjects(user);

            Project projectToUpdate = (from p in projects where p.ProjectName == testProject.ProjectName select p).Single();

            projectToUpdate.ProjectName = "TEst UPDATE Project";

            try
            {
                User userNotAdmin = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });
                Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(() => { DataProviderProxy.UpdateProject(projectToUpdate, userNotAdmin, DataProviderService.DbConcurencyUpdateOptions.ClientPriority); });
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var projectToRemove = (from p in ctx.Projects where p.ProjectName == testProject.ProjectName select p).Single();
                    ctx.Projects.Remove(projectToRemove);
                    ctx.SaveChanges();
                }
            }
        }

        #endregion

        #region ChangeSummaryReadOnlyStatus

        [TestMethod]
        public void ChangeSummaryReadOnlyStatus()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = ADMIN_USER_NAME, Password = ADMIN_USER_PASSWORD});

            List<Summary> testSummaries = new List<Summary>();
            Summary newSummary = new Summary()
            {
                SummaUAH = 200,
                SummaryDate = DateTime.MinValue,
                Accountable = new Accountable() { Id = ACCOUNTABLE1_ID, AccountableName = ACCOUNTABLE1 },
                Project = new Project() { Id = PROJECT1_ID, ProjectName = PROJECT1 },
                BudgetItem = new BudgetItem() { Id = BUDGET_ITEM1_ID, ItemName = BUDGET_ITEM1 },
                Description = "test description"
            };

            
            for (int i = 0; i < 10; i++)
            {
                testSummaries.Add(newSummary);
            }

            Summary testSummary = null;
            foreach (var s in testSummaries)
            {
                testSummary = DataProviderProxy.AddSummary(s, user);
            }
            testSummary.SummaUAH = 99999999;
            try
            {
                DataProviderProxy.UpdateSummary(testSummary, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

                Filter filter = new Filter() { DateEnd = DateTime.MinValue };

                DataProviderProxy.ChangeSummaryReadOnlyStatus(true, filter, user);

                using (CFAPContext ctx = new CFAPContext())
                {
                    var correctedSummaries = (from s in ctx.Summaries where s.Description == newSummary.Description select s).ToList();
                    foreach (var summary in correctedSummaries)
                    {
                        Assert.AreEqual(true, summary.ReadOnly);
                    }
                }

                DataProviderProxy.ChangeSummaryReadOnlyStatus(false, filter, user);

                using (CFAPContext ctx = new CFAPContext())
                {
                    var correctedSummaries = (from s in ctx.Summaries where s.Description == newSummary.Description select s).ToList();
                    foreach (var summary in correctedSummaries)
                    {
                        Assert.AreEqual(false, summary.ReadOnly);
                    }
                }
            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var summariesToRemove = (from s in ctx.Summaries where s.Description == newSummary.Description select s).ToList();
                    ctx.Summaries.RemoveRange(summariesToRemove);
                    ctx.SaveChanges();
                }
            }



        }

        [TestMethod]
        public void ChangeSummaryReadOnlyStatus_NoRightsToChangeDataException()
        {
            User user = DataProviderProxy.Authenticate(new User() { UserName = USER_NOT_ADMIN_NAME, Password = USER_NOT_ADMIN_PASSWORD });

            List<Summary> testSummaries = new List<Summary>();
            Summary newSummary = new Summary()
            {
                SummaUAH = 200,
                SummaryDate = DateTime.MinValue,
                Accountable = new Accountable() { Id = ACCOUNTABLE1_ID, AccountableName = ACCOUNTABLE1 },
                Project = new Project() { Id = PROJECT1_ID, ProjectName = PROJECT1 },
                BudgetItem = new BudgetItem() { Id = BUDGET_ITEM1_ID, ItemName = BUDGET_ITEM1 },
                Description = "test description"
            };


            for (int i = 0; i < 10; i++)
            {
                testSummaries.Add(newSummary);
            }

            Summary testSummary = null;
            foreach (var s in testSummaries)
            {
                testSummary = DataProviderProxy.AddSummary(s, user);
            }
            testSummary.SummaUAH = 99999999;
            try
            {
                DataProviderProxy.UpdateSummary(testSummary, user, DataProviderService.DbConcurencyUpdateOptions.ClientPriority);

                Filter filter = new Filter() { DateEnd = DateTime.MinValue };

                Assert.ThrowsException<FaultException<NoRightsToChangeDataException>>(()=>{DataProviderProxy.ChangeSummaryReadOnlyStatus(true, filter, user);});

            }
            finally
            {
                using (CFAPContext ctx = new CFAPContext())
                {
                    var summariesToRemove = (from s in ctx.Summaries where s.Description == newSummary.Description select s).ToList();
                    ctx.Summaries.RemoveRange(summariesToRemove);
                    ctx.SaveChanges();
                }
            }



        }
        #endregion
    }
}
