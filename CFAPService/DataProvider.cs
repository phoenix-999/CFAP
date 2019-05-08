﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAPDataModel.Models;
using CFAPDataModel;
using System.ServiceModel;
using CFAPService.Faults;
using NLog;
using System.Transactions;


namespace CFAPService
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class DataProvider : IDataProvider
    {
        public User AuthentificateUser(User user)
        {
            return CheckUser(user);
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void AddNewUser(User newUser, User owner)
        {
            AddUser(newUser, owner);
        }

        private User CheckUser(User user)
        {
            User result = null;
            user.EnriptPassword();

            using (CFAPContext ctx = new CFAPContext())
            {
                var query = from u in ctx.Users
                            where u.Password == user.Password && u.UserName == user.UserName
                            select u;

                try
                {
                    result = query.FirstOrDefault();
                }
                catch(Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }

            if (result == null)
            {
                throw new FaultException<AutenticateFaultException>(new AutenticateFaultException(user));
            }

            return result;
        }

        private void AddUser(User newUser, User owner)
        {
            bool isAdmin = CheckUser(owner).IsAdmin;

            if (!isAdmin)
            {
                new FaultException<AddUserNotAdminException>(new AddUserNotAdminException(owner));
            }

            newUser.EnriptPassword(); 

            using (CFAPContext ctx = new CFAPContext())
            {
                try
                {
                    ctx.Users.Attach(newUser);
                    ctx.Entry<User>(newUser).State = System.Data.Entity.EntityState.Added;
                    ctx.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new FaultException<DbException>(new DbException(ex));
                }
            }
        }
    }
}
