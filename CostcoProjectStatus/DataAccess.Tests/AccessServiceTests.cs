using System;
using System.Collections.Generic;
using DataService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataService.Tests
{
    [TestClass()]
    public class AccessServiceTests
    {
        [TestMethod()]
        public void IsUserAuthorizedTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetUserRoleTest()
        {//__this covers AddUser, GetUserRole, DeleteUser, UpdateUserRole
            string emailAddy = "Test@mail.com";
           /// string emailAddy2 = "Test2@mail.com";
            int userRole = 1;
            AccessService dataAccess = new AccessService();
            try//__make sure we can back out in case of errors
            {

                bool userAdded = dataAccess.AddUser(emailAddy, userRole);

                //__test adding new user
                Assert.IsTrue(userAdded, "AddUser returned false for email " + emailAddy + ", UserRole=" + userRole);
                int recordedRole = dataAccess.GetUserRole(emailAddy);
                Assert.AreEqual(userRole, recordedRole);

                int newRole = 0;
                dataAccess.UpdateUserRole(emailAddy, newRole);
                recordedRole = dataAccess.GetUserRole(emailAddy);
                Assert.AreEqual(newRole, recordedRole, "User Role not properly Modified");

                bool userRemoved = dataAccess.DeleteUser(emailAddy);
                Assert.IsTrue(userRemoved, "userRemoved returned false");
            }
            finally
            {//__make sure to clean out our test data
                if (dataAccess.IsUserAuthorized(emailAddy))
                {
                    dataAccess.DeleteUser(emailAddy);
                }
            }
        }

        //[TestMethod]
        //public void AddSUemails()
        //{//__this covers AddUser, GetUserRole, DeleteUser, UpdateUserRole
        //    List<string> emailAddys = new List<string>();
        //    emailAddys.Add("seattleuni12@gmail.com");

        //    int userRole = 0;
        //    AccessService dataAccess = new AccessService();
        //    try//__make sure we can back out in case of errors
        //    {

        //        foreach (var email in emailAddys)
        //        {
        //            dataAccess.AddUser(email, userRole);
        //        }

        //    }
        //    catch (Exception e)
        //    {//__make sure to clean out our test data

        //    }
        //}

        [TestMethod]
        public void AccessServiceConstructorWorks()
        {
            try
            {
                var dataAccess = new AccessService();
                if (dataAccess == null) throw new Exception("AccessService constructor returned null");
            }
            catch (Exception e)
            {
                Assert.Fail("AccessService constructor threw exception: " + e.Message);

            }
        }
    }
}
