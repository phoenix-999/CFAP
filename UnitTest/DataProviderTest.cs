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

        const string ADMIN_USER_NAME = "yurii";
        const string ADMIN_USER_PASSWORD = "1";

        const string TEST_USER_NAME = "TestUserName";
        const string TEST_USER_PASSWORD = "TestUserPassword";

        const int MAIN_OFFICE_ID = 1;
        const string MAIN_OFFICE = "MainOffice";

        const int OFFICE1_ID = 2;
        const string OFFICE1 = "Office1";

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
        }

        #endregion
    }
}
