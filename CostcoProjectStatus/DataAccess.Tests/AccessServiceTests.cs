using System;
using System.Collections.Generic;
using DataService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StatusUpdatesModel;
using System.Data.SqlClient;
using System.IO;

namespace DataService.Tests
{
    [TestClass()]
    public class AccessServiceTests
    {
        int VERTICALENUM = 8;
        public static string ConnectionString = "Server=tcp:costcosu.database.windows.net,1433;Database=CostcoDevStatus;User ID=SUAdmin@costcosu;Password=39ffbJeo;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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

                    // Got to make sure that the data is the same
                    using (SqlConnection sqlConnection = new SqlConnection())
                    {
                        sqlConnection.ConnectionString = ConnectionString;


                        SqlCommand sqlCommand = new SqlCommand("select * from Project where VerticalID=\'" + verticalIter + "\'", sqlConnection);
                        sqlCommand.CommandTimeout = 30;


                        sqlConnection.Open();
                        SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                        int sqlCount = 0;

                        while (sqlReader.Read())
                        {
                            // could make this more efficient if i knew hot to cast a sql object into a type
                            bool isProjectThere = false;
                            Project currProject = new Project();
                            currProject.ProjectID = new Guid(sqlReader["ProjectID"].ToString());
                            currProject.ProjectName = sqlReader["ProjectName"].ToString();
                            foreach (Project testProject in allProjectsList)
                            {
                                if (testProject.ProjectID.Equals(currProject.ProjectID))
                                {
                                    isProjectThere = true;
                                }
                            }

                            Assert.IsTrue(isProjectThere, "Project " + currProject.ProjectName + " does not exist in vertical " + verticalIter);
                            sqlCount++;
                        }
                        sqlConnection.Close();
                        Assert.AreEqual(sqlCount, allProjectsList.Count, "The number of projects are not equal. The database has " + sqlCount + " and the Access Service layer is returning " + allProjectsList.Count + " for vertical " + verticalIter);
                    }

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
            try
            {
                var dataAccess = new AccessService();
                List<Project> illegalVertical = dataAccess.GetAllProjectsForVertical(illVertNum);
                if ((illegalVertical != null && illegalVertical.Count > 0) && (illVertNum > 7 || illVertNum < 0))
                {
                    Assert.Fail("The vertical ID " + illVertNum + "exists and it should not!");
                }
            }
            catch (Exception e)
            {
                Assert.Fail("checkForGetAllProjectsForVerticalFailure in GetAppProjectsForVerticalAsync failed with this exception: " + e.Message);
            }

        }
        /// <summary>
        /// Tests the adding of new users. Note that the new user function does not check for illegal user roles.
        /// </summary>
        /// <param name="illVertNum"> Fake vertical ID which the access layer should return null</param>
        [TestMethod()]
        public void AddDeleteUserTest()
        {
            var dataAccess = new AccessService();
            // Make sure that these domains are not there
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            // Make sure that you can add users
            Assert.IsTrue(dataAccess.AddUser("faketestuser@fakedomain.com", 0));
            Assert.IsTrue(dataAccess.AddUser("faketestuser1@fakedomain.com", 1));
            Assert.IsTrue(dataAccess.AddUser("faketestuser2@fakedomain.com", 2));

            // Check that the users are actually in the DB
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));


            // Make sure that you canNOT add users already in the DB
            Assert.IsFalse(dataAccess.AddUser("faketestuser@fakedomain.com", 0));
            Assert.IsFalse(dataAccess.AddUser("faketestuser1@fakedomain.com", 1));
            Assert.IsFalse(dataAccess.AddUser("faketestuser2@fakedomain.com", 2));

            // Make sure that you can delete users
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser2@fakedomain.com"));

            // Make sure that these domains are not there
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));
        }

        [TestMethod()]
        public void IsUserAuthorizedTest()
        {
            var dataAccess = new AccessService();
            Assert.IsTrue(dataAccess.IsUserAuthorized("costcosu@gmail.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("nobodyhere@somedomain.com"));
        }

        [TestMethod()]
        public void GetProjectNameForIDTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetProjectIDbyNameTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetProjectIDsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllUpdatesFromEmailTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllVerticalsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetUpdatesForKeyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllUpdatesForProjectTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllProjectNamesTest()
        {
            var dataAccess = new AccessService();
            List<Project> allProjectsList = dataAccess.GetAllProjectNames();

            // Got to make sure that the data is the same
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                  sqlConnection.ConnectionString = ConnectionString;

                  SqlCommand sqlCommand = new SqlCommand("select * from Project", sqlConnection);
                  sqlCommand.CommandTimeout = 30;


                   sqlConnection.Open();
                   SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                   int sqlCount = 0;

                   while (sqlReader.Read())
                        {
                            // could make this more efficient if i knew hot to cast a sql object into a type
                            bool isProjectThere = false;
                            Project currProject = new Project();
                            currProject.ProjectID = new Guid(sqlReader["ProjectID"].ToString());
                            currProject.ProjectName = sqlReader["ProjectName"].ToString();
                            foreach (Project testProject in allProjectsList)
                            {
                                if (testProject.ProjectID.Equals(currProject.ProjectID))
                                {
                                    isProjectThere = true;
                                }
                            }

                            Assert.IsTrue(isProjectThere, "Project " + currProject.ProjectName + " does not exist in the list of projects");
                            sqlCount++;
                        }
                        sqlConnection.Close();
                        Assert.AreEqual(sqlCount, allProjectsList.Count, "The number of projects are not equal. The database has " + sqlCount + " and the Access Service layer is returning " + allProjectsList.Count + " for this list of projects");
             }
        }

        [TestMethod()]
        public void RecordStatusUpdateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateUserEmailTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateUserRoleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsAppAuthorizedTest()
        {
            var dataAccess = new AccessService();

                // Make sure the authorized apps are authorized
            Assert.IsTrue(dataAccess.IsAppAuthorized("excelCostco"));
            Assert.IsTrue(dataAccess.IsAppAuthorized("emailCostco"));

            // Throw in some fake ones to be sure it's working
            Assert.IsFalse(dataAccess.IsAppAuthorized("junkApp"));
            Assert.IsFalse(dataAccess.IsAppAuthorized("1"));
            Assert.IsFalse(dataAccess.IsAppAuthorized("password"));
        }
    }
}
