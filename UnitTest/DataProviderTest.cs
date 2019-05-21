using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using UnitTest.DataProviderService;
using System.Collections.Generic;
using System.Linq;
using CFAPDataModel;

namespace UnitTest
{
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
        const string OFFICE1 = "Office1";

        const int OFFICE2_ID = 45;
        const string OFFICE2 = "Office2";

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
                    new UserGroup(){ Id = MAIN_OFFICE_ID}
                    , new UserGroup() { Id = OFFICE1_ID}
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

        //TODO реализовать проверку удаленных групп
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
            UserGroup[] oldGroups = (from g in userForUpdate.UserGroups where g.CanUserAllData == false select g).ToArray();
            userForUpdate.UserGroups = new UserGroup[] { new UserGroup() { Id = OFFICE2_ID } };

            DataProviderProxy.UpdateUser(userForUpdate, owner);

            using (CFAPContext ctx = new CFAPContext())
            {

                var updatedUser = (from user in ctx.Users where user.UserName == userForUpdate.UserName select user).Single();
                ctx.Users.Include("UserGroups");

                Assert.AreEqual(updatedUser.IsAdmin, userForUpdate.IsAdmin);
                Assert.AreEqual(updatedUser.UserName, userForUpdate.UserName);
                Assert.AreEqual(updatedUser.Password, oldPassword);
                Assert.AreEqual(updatedUser.CanAddNewUsers, userForUpdate.CanAddNewUsers);

                foreach (var newGroup in userForUpdate.UserGroups)
                {
                    var correctGroup = (from g in updatedUser.UserGroups where g.Id == newGroup.Id select g).FirstOrDefault();
                    Assert.AreNotEqual(correctGroup, null);
                }

                var canUseAllDataGroups = (from g in ctx.UserGroups where g.CanUserAllData == true select g).ToArray();
                
                foreach (var groupCanUseAllData in canUseAllDataGroups)
                {
                    var correctGroup = (from g in updatedUser.UserGroups where g.Id == groupCanUseAllData.Id select g).FirstOrDefault();
                    Assert.AreNotEqual(correctGroup, null);
                }

                //Изменение Id обновленного пользователя, так как при изменении данных пользователя Id меняется
                userForUpdate.Id = updatedUser.Id;
            }

            userForUpdate.UserName = oldUserName;
            userForUpdate.Password = oldPassword;
            userForUpdate.IsAdmin = oldIsAdmin;
            userForUpdate.UserGroups = oldGroups;

            DataProviderProxy.UpdateUser(userForUpdate, owner);
        }

        #endregion
    }
}
