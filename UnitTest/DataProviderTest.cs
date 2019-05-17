using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ServiceModel;
using UnitTest.DataProviderService;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class DataProviderTest
    {
        User MainUser;

        DataProviderClient DataProviderProxy = new DataProviderClient();

        #region Authenticate
        [TestMethod]
        public void Authenticate()
        {
            User user = new User() { UserName = "yurii", Password = "1" };
            MainUser = DataProviderProxy.Authenticate(user);

            User correctUser = new User()
            {
                UserName = "yurii"
                ,Password = "NWoZK3kTsExUV00Ywo1G5jlUKKs="
                ,CanAddNewUsers = true
                ,IsAdmin = true
            };

            Assert.IsNotNull(MainUser);
            Assert.AreEqual(MainUser.UserName, correctUser.UserName);
            Assert.AreEqual(MainUser.Password, correctUser.Password);
            Assert.AreEqual(MainUser.CanAddNewUsers, correctUser.CanAddNewUsers);
            Assert.AreEqual(MainUser.IsAdmin, correctUser.IsAdmin);
        }

        [TestMethod]
        public void AuthenticateUserGroups()
        {
            User user = new User() { UserName = "yurii", Password = "1" };
            MainUser = DataProviderProxy.Authenticate(user);

            List<UserGroup> correctedGroups = new List<UserGroup>();

            UserGroup mainOffice = new UserGroup{ Id = 1, GroupName = "MainOffice", CanUserAllData = true };
            correctedGroups.Add(mainOffice);

            UserGroup office1 = new UserGroup { Id = 2, GroupName = "Office1", CanUserAllData = false };
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
        public void AuthenticateArgumentNullExceptions()
        {
            User user = new User();

            Assert.ThrowsException<FaultException<ArgumentNullException>>(()=> { DataProviderProxy.Authenticate(user); });
        }
        #endregion
    }
}
