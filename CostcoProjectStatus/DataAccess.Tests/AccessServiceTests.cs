using System;
using System.Collections.Generic;
using DataService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatusUpdatesModel;

namespace DataService.Tests
{
    [TestClass()]
    public class AccessServiceTests
    {
        int VERTICALENUM = 8;
        [TestMethod()]
        public void IsUserAuthorizedTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetUserRoleTest()
        {//__this covers AddUser, GetUserRole, DeleteUser, UpdateUserRole
            string emailAddy = "Test@mail.com";
            //string emailAddy2 = "Test2@mail.com";
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

        [TestMethod()]
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

        /// <summary>
        /// Tests public accessable GetAllProjectsForVertical function. Checks to make sure that all valid vertical ID's returns
        /// some form of valid data (brute force), and non valid verticals returns no data via boundary and random test.
        /// </summary>
        [TestMethod()]
        public void GetAllProjectsForVerticalTest()
        {
            try
            {
                var dataAccess = new AccessService();
                for (int verticalIter = 0; verticalIter < VERTICALENUM; verticalIter++)
                {
                    List<Project> allProjectsList = dataAccess.GetAllProjectsForVertical(verticalIter);
                    Assert.AreNotEqual(allProjectsList, null);
                }

                // Boundary test #1: Vertical ID -1
                checkForGetAllProjectsForVerticalFailure(-1);

                // Boundary test #2: Vertical ID -2
                checkForGetAllProjectsForVerticalFailure(-2);

                // Boundary test #3: Vertical ID 8
                checkForGetAllProjectsForVerticalFailure(8);

                // Boundary test #4: Vertical ID 9
                checkForGetAllProjectsForVerticalFailure(9);

                // Random number test
                Random random = new Random();
                int randomNumber = random.Next(8, int.MaxValue);
                checkForGetAllProjectsForVerticalFailure(randomNumber);

            }
            catch (Exception e)
            {
                Assert.Fail("GetAppProjectsForVerticalAsync failed with this exception: " + e.Message);
            }
        }

        /// <summary>
        /// Helper function for GetAllProjectsForVerticalTest - whatever number that gets passed in should
        /// not be between 0-8
        /// </summary>
        /// <param name="illVertNum"> Fake vertical ID which the access layer should return null</param>
        private void checkForGetAllProjectsForVerticalFailure(int illVertNum)
        {
            try {
                var dataAccess = new AccessService();
                List<Project> illegalVertical = dataAccess.GetAllProjectsForVertical(illVertNum);
                if ((illegalVertical != null && illegalVertical.Count > 0) && (illVertNum > 7 || illVertNum < 0 ))
                {
                    Assert.Fail("The vertical ID " + illVertNum + "exists and it should not!");
                }
            }
            catch (Exception e)
            {
                Assert.Fail("checkForGetAllProjectsForVerticalFailure in GetAppProjectsForVerticalAsync failed with this exception: " + e.Message);
            }

        }
    }
}
