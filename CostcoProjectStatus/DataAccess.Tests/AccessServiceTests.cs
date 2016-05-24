﻿using System;
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

        /// <summary>
        /// Tests AccessService Constructor - pretty straight forward.
        /// </summary>
        [TestMethod()]
        public void AccessServiceConstructorTest()
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

        #region Authentication Tests
        /// <summary>
        /// Tests adding a user - 
        /// adds 3 users of different levels then checks for the existance
        /// Also checks if user already exists, etc
        /// </summary>
        [TestMethod()]
        public void AddUserTest()
        {
            var dataAccess = new AccessService();
            // Make sure that these domains are not there
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            Guid userId = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsTrue(userId == Guid.Empty);

            // Make sure that you can add users
            Assert.IsTrue(dataAccess.AddUser("faketestuser@fakedomain.com", 0));
            Assert.IsTrue(dataAccess.AddUser("faketestuser1@fakedomain.com", 1));
            Assert.IsTrue(dataAccess.AddUser("faketestuser2@fakedomain.com", 2));

            // Check that the users are actually in the DB
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            Guid userIdCheck2 = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsFalse(userIdCheck2 == Guid.Empty);

            // Make sure that you canNOT add users already in the DB
            Assert.IsFalse(dataAccess.AddUser("faketestuser@fakedomain.com", 0));
            Assert.IsFalse(dataAccess.AddUser("faketestuser1@fakedomain.com", 1));
            Assert.IsFalse(dataAccess.AddUser("faketestuser2@fakedomain.com", 2));

            // Delete users
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser2@fakedomain.com"));

        }

        /// <summary>
        /// Check that IsUserAuthorized test works
        /// </summary>
        [TestMethod()]
        public void IsUserAuthorizedTest()
        {
            var dataAccess = new AccessService();
            // Make sure that these domains are not there
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            Guid userId = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsTrue(userId == Guid.Empty);

            // Make sure that you can add users
            Assert.IsTrue(dataAccess.AddUser("faketestuser@fakedomain.com", 0));
            Assert.IsTrue(dataAccess.AddUser("faketestuser1@fakedomain.com", 1));
            Assert.IsTrue(dataAccess.AddUser("faketestuser2@fakedomain.com", 2));

            // Check that the users are actually in the DB
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            Guid userIdCheck2 = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsFalse(userIdCheck2 == Guid.Empty);

            // Delete users
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser2@fakedomain.com"));
        }

        /// <summary>
        /// Checks if IsAppAuthorized works
        /// These Apps are hardcoded into the database
        /// by SeattleU. They may change after 6/2/2016
        /// </summary>
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
        /// <summary>
        /// Retrieves User Roles
        /// </summary>
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

        /// <summary>
        ///  Test for updating the user role
        /// </summary>
        [TestMethod()]
        public void UpdateUserRoleTest()
        {
            var dataAccess = new AccessService();

            // Make sure that you can add users
            Assert.IsTrue(dataAccess.AddUser("faketestuser@fakedomain.com", 0));
            Assert.IsTrue(dataAccess.AddUser("faketestuser1@fakedomain.com", 1));
            Assert.IsTrue(dataAccess.AddUser("faketestuser2@fakedomain.com", 2));

            Guid userIdCheck2 = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsFalse(userIdCheck2 == Guid.Empty);

            // Check the user role
            Assert.IsTrue(dataAccess.GetUserRole("faketestuser@fakedomain.com") == 0);
            Assert.IsTrue(dataAccess.GetUserRole("faketestuser1@fakedomain.com") == 1);
            Assert.IsTrue(dataAccess.GetUserRole("faketestuser2@fakedomain.com") == 2);

            // Here's the actual test - update the user role:
            Assert.IsTrue(dataAccess.UpdateUserRole("faketestuser@fakedomain.com", 1));
            Assert.IsTrue(dataAccess.UpdateUserRole("faketestuser1@fakedomain.com", 2));
            Assert.IsTrue(dataAccess.UpdateUserRole("faketestuser2@fakedomain.com", 0));

            // Check that the user role has changed correctly
            Assert.IsFalse(dataAccess.GetUserRole("faketestuser@fakedomain.com") == 0);
            Assert.IsFalse(dataAccess.GetUserRole("faketestuser1@fakedomain.com") == 1);
            Assert.IsFalse(dataAccess.GetUserRole("faketestuser2@fakedomain.com") == 2);
            Assert.IsTrue(dataAccess.GetUserRole("faketestuser@fakedomain.com") == 1);
            Assert.IsTrue(dataAccess.GetUserRole("faketestuser1@fakedomain.com") == 2);
            Assert.IsTrue(dataAccess.GetUserRole("faketestuser2@fakedomain.com") == 0);

            // Delete users
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser2@fakedomain.com"));
        }
        /// <summary>
        /// Tests the adding, deleting, and looking up of new users. Note that the new user function does not check for illegal user roles.
        /// </summary>
        /// <param name="illVertNum"> Fake vertical ID which the access layer should return null</param>
        [TestMethod()]
        public void DeleteUserTest()
        {
            var dataAccess = new AccessService();

            // Make sure that you can add users
            Assert.IsTrue(dataAccess.AddUser("faketestuser@fakedomain.com", 0));
            Assert.IsTrue(dataAccess.AddUser("faketestuser1@fakedomain.com", 1));
            Assert.IsTrue(dataAccess.AddUser("faketestuser2@fakedomain.com", 2));

            // Check that the users are actually in the DB
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));


            // Make sure that you can delete users
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser2@fakedomain.com"));

            // Make sure that these domains are not there
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

        }
        /// <summary>
        /// Tests UpdateUserEmail with the arguments:
        /// string oldEmail, new Email
        /// </summary>
        [TestMethod()]
        public void UpdateUserEmailTest()
        {
            var dataAccess = new AccessService();
            // Make sure that these domains are not there
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            // Make sure these updated email addresses do not exist
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuserupdated@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1updated@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2updated@fakedomain.com"));

            Guid userId = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsTrue(userId == Guid.Empty);

            // Make sure that you can add users
            Assert.IsTrue(dataAccess.AddUser("faketestuser@fakedomain.com", 0));
            Assert.IsTrue(dataAccess.AddUser("faketestuser1@fakedomain.com", 1));
            Assert.IsTrue(dataAccess.AddUser("faketestuser2@fakedomain.com", 2));

            // Check that the users are actually in the DB
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            Guid userIdCheck2 = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsFalse(userIdCheck2 == Guid.Empty);

            // Make sure these updated email addresses do not exist
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuserupdated@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1updated@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2updated@fakedomain.com"));

            // Now we are going to update these users - this is the main part
            Assert.IsTrue(dataAccess.UpdateUserEmail("faketestuser@fakedomain.com", "faketestuserupdated@fakedomain.com"));
            Assert.IsTrue(dataAccess.UpdateUserEmail("faketestuser1@fakedomain.com", "faketestuser1updated@fakedomain.com"));
            // Update it in the overloaded way
            Guid userId3 = dataAccess.GetUserID("faketestuser2@fakedomain.com");
            Assert.IsTrue(dataAccess.UpdateUserEmail(userId3, "faketestuser2updated@fakedomain.com"));

            // Make sure that these domains are not there anymore
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            // Make sure these updated email addresses exist
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuserupdated@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser1updated@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser2updated@fakedomain.com"));

            // Clean up users
            Assert.IsTrue(dataAccess.DeleteUser("faketestuserupdated@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser1updated@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser2updated@fakedomain.com"));

        }

        /// <summary>
        /// Tests overloaded UpdateUser Email with the arguments:
        /// GUID UserId, New Email
        /// </summary>
        [TestMethod()]
        public void UpdateUserEmailTest2()
        {
            var dataAccess = new AccessService();
            // Make sure that these domains are not there
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            // Make sure these updated email addresses do not exist
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuserupdated@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1updated@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2updated@fakedomain.com"));

            Guid userId = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsTrue(userId == Guid.Empty);

            // Make sure that you can add users
            Assert.IsTrue(dataAccess.AddUser("faketestuser@fakedomain.com", 0));
            Assert.IsTrue(dataAccess.AddUser("faketestuser1@fakedomain.com", 1));
            Assert.IsTrue(dataAccess.AddUser("faketestuser2@fakedomain.com", 2));

            // Check that the users are actually in the DB
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            Guid userIdCheck2 = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsFalse(userIdCheck2 == Guid.Empty);

            // Make sure these updated email addresses do not exist
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuserupdated@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1updated@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2updated@fakedomain.com"));

            // Now we are going to update these users - this is the main part
            Assert.IsTrue(dataAccess.UpdateUserEmail("faketestuser@fakedomain.com", "faketestuserupdated@fakedomain.com"));
            Assert.IsTrue(dataAccess.UpdateUserEmail("faketestuser1@fakedomain.com", "faketestuser1updated@fakedomain.com"));
            // Update it in the overloaded way
            Guid userId3 = dataAccess.GetUserID("faketestuser2@fakedomain.com");
            Assert.IsTrue(dataAccess.UpdateUserEmail(userId3, "faketestuser2updated@fakedomain.com"));

            // Make sure that these domains are not there anymore
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser1@fakedomain.com"));
            Assert.IsFalse(dataAccess.IsUserAuthorized("faketestuser2@fakedomain.com"));

            // Make sure these updated email addresses exist
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuserupdated@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser1updated@fakedomain.com"));
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser2updated@fakedomain.com"));

            // Clean up users
            Assert.IsTrue(dataAccess.DeleteUser("faketestuserupdated@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser1updated@fakedomain.com"));
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser2updated@fakedomain.com"));

        }

        /// <summary>
        /// Checks if user ID is correct
        /// </summary>
        [TestMethod()]
        public void GetUserIDTest()
        {
            var dataAccess = new AccessService();
            // Add a user
            Assert.IsTrue(dataAccess.AddUser("faketestuser@fakedomain.com", 0));

            // Check that the users are actually in the DB
            Assert.IsTrue(dataAccess.IsUserAuthorized("faketestuser@fakedomain.com"));

            // Here's the actual test - the GUID should not be empty
            Guid userIdCheck = dataAccess.GetUserID("faketestuser@fakedomain.com");
            Assert.IsFalse(userIdCheck == Guid.Empty);

            // Now delete and check again - if that was really the user's id, then it should return false after deleting
            Assert.IsTrue(dataAccess.DeleteUser("faketestuser@fakedomain.com"));
            Assert.IsTrue(userIdCheck == Guid.Empty);

        }
        #endregion

        #region StatusUpdate and Project Methods Tests

        /// <summary>
        /// Tests Updating of a package
        /// </summary>
        [TestMethod]
        public void RecordPackageUpdateTest()
        {
            AccessService dbService = new AccessService();
            //var projectUpdates = dbService.GetProjectUpdates("6a8a7e56-e9ac-4385-a8be-5be702c1f2e6");
            UpdatePackage package = new UpdatePackage();
            package.ProjectName = "Test Project";
            package.Subject = "Deployment";
            package.Body = "Environment:br549|Jimmy, toloose";

            package.Updates.Add("verticalID", "3");
            package.Updates.Add("Environment", "br549");
            package.Updates.Add("Author", "Samantha");
            package.Updates.Add("Manager", "Bocephus");

            //__Adding this package should create a new Project and return the ProjectID as string
            string stId = dbService.RecordUpdatePackage(package);
            Assert.IsFalse(string.IsNullOrEmpty(stId));

            Guid projectID = Guid.Parse(stId);
            Assert.IsFalse(projectID == Guid.Empty);

            Guid recordedId = dbService.GetProjectIDbyName(package.ProjectName);
            Assert.AreEqual(projectID, recordedId);



            dbService.DeleteProject(projectID);
        }
        /// <summary>
        /// Tests the deletion of a project
        /// </summary>
        [TestMethod()]
        public void DeleteProjectTest()
        {
            AccessService dbService = new AccessService();
            //var projectUpdates = dbService.GetProjectUpdates("6a8a7e56-e9ac-4385-a8be-5be702c1f2e6");
            UpdatePackage package = new UpdatePackage();
            package.ProjectName = "Test Project";
            package.Subject = "Deployment";
            package.Body = "Environment:br549|Jimmy, toloose";

            package.Updates.Add("verticalID", "3");
            package.Updates.Add("Environment", "br549");
            package.Updates.Add("Author", "Samantha");
            package.Updates.Add("Manager", "Bocephus");

            //__Adding this package should create a new Project and return the ProjectID as string
            string stId = dbService.RecordUpdatePackage(package);
            Assert.IsFalse(string.IsNullOrEmpty(stId));

            Guid projectID = Guid.Parse(stId);
            Assert.IsFalse(projectID == Guid.Empty);

            Guid recordedId = dbService.GetProjectIDbyName(package.ProjectName);
            Assert.AreEqual(projectID, recordedId);
            // This is the actual test - let's see if it actually deletes the project
            dbService.DeleteProject(projectID);
            // When we look for updates, it should be null or empty
            Assert.IsTrue(dbService.GetAllUpdatesForProject(projectID.ToString()) == null);
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
                for (int verticalIter = -1; verticalIter < VERTICALENUM; verticalIter++)
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
                Assert.Fail("GetAllProjectsForVerticalTest failed with this exception: " + e.Message);
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

        [TestMethod()]
        public void GetProjectNameForIDTest()
        {
            var dataAccess = new AccessService();
            // Grab a list of all the projects in the database
            List<Project> allProjects = dataAccess.GetAllProjectNames();

            // In case the database has just been wiped out...
            if (allProjects.Count != 0)
            {
                // Pick a random project and get the ID and the (correct) project name
                Random random = new Random();
                int randomNumber = random.Next(0, allProjects.Count);
                Guid randomProjectID = allProjects[randomNumber].ProjectID;
                string randomProjectName = allProjects[randomNumber].ProjectName;

                // Using the ID, use the function that we are testing to retrieve the projectname
                string checkThisName = dataAccess.GetProjectNameForID(randomProjectID);

                // Check if it is correct. boom.
                Assert.AreEqual(checkThisName, randomProjectName);
            }
            else
            {
                Assert.Inconclusive("Database might be empty, try rerunning with data");
            }
        }

        /// <summary>
        /// Do the reverse of the previous test - grab the project ID using 
        /// the project name.
        /// </summary>
        [TestMethod()]
        public void GetProjectIDbyNameTest()
        {
            var dataAccess = new AccessService();
            // Grab a list of all the projects in the database
            List<Project> allProjects = dataAccess.GetAllProjectNames();
            if (allProjects.Count != 0)
            {
                // Pick a random project and get the ID and the (correct) project name
                Random random = new Random();
                int randomNumber = random.Next(0, allProjects.Count);
                Guid randomProjectID = allProjects[randomNumber].ProjectID;
                string randomProjectName = allProjects[randomNumber].ProjectName;

                // Using the ID, use the function that we are testing to retrieve the projectname
                Guid checkThisID = dataAccess.GetProjectIDbyName(randomProjectName);

                // Check if it is correct. boom.
                Assert.AreEqual(checkThisID, randomProjectID);
            }
            else
            {
                Assert.Inconclusive("Database might be empty, try rerunning with data");
            }
        }

        [TestMethod()]
        public void GetProjectIDsTest()
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
        public void GetAllUpdatesFromEmailTest()
        {
            var dataAccess = new AccessService();
            // First, let's create a StatusUpdate
            Random random = new Random();
            int randomNumber = random.Next(0, int.MaxValue);
            string projectName = "Access Service Unit Test Project" + randomNumber;
            StatusUpdate newProject = new StatusUpdate();
            newProject.PhaseID = 0;
            newProject.ProjectName = projectName;
            newProject.VerticalID = 3;
            List<StatusUpdate> newProjectsList = new List<StatusUpdate>();
            newProjectsList.Add(newProject);

            dataAccess.RecordStatusUpdate(newProjectsList);

            // Now, let's make sure that the project got added
            Guid projectGuid = dataAccess.GetProjectIDbyName(projectName);

            // Test the updating part of the project. The function only updates one project at a time
            Assert.IsTrue(projectGuid != Guid.Empty);
            StatusUpdate updateProject = newProject;
            updateProject.PhaseID = 1;
            updateProject.UpdateKey = "Environment";
            updateProject.UpdateValue = "QA1";
            StatusUpdate updateProject2 = newProject;
            updateProject2.PhaseID = 2;
            updateProject.UpdateKey = "Random Key";
            updateProject.UpdateValue = "BT1";
            StatusUpdate updateProject3 = newProject;
            updateProject2.PhaseID = 3;
            updateProject.UpdateKey = "Environment";
            updateProject.UpdateValue = "BT3";
            List<StatusUpdate> updateProjectsList = new List<StatusUpdate>();
            updateProjectsList.Add(updateProject);
            updateProjectsList.Add(updateProject2);
            updateProjectsList.Add(updateProject3);
            dataAccess.RecordStatusUpdate(updateProjectsList);

            // Test that the updates got in
            List<StatusUpdate> retrievedProjectUpdates = dataAccess.GetUpdatesForKey("Environment", projectGuid);
            Guid projectUpdateID = Guid.Empty;
            // Look for the Project Update ID
            for (int i = 0; i < retrievedProjectUpdates.Count; i++)
            {
                if (retrievedProjectUpdates[i].PhaseID == 1)
                {
                    projectUpdateID = retrievedProjectUpdates[i].ProjectUpdateID;
                }
            }

            Assert.AreEqual(dataAccess.GetAllUpdatesFromEmail(projectUpdateID).Count, 1);
            dataAccess.DeleteProject(projectGuid);
        }

        [TestMethod()]
        public void GetAllVerticalsTest()
        {
            var dataAccess = new AccessService();
            List<KeyValuePair<int, string>> verticalsList = dataAccess.GetAllVerticals();
            // We can hardcode this because the verticals should never change. If it does,
            // it shouldn't be very frequent and is reasonable to change this relatively unimportant unit test.
            Assert.IsTrue(verticalsList[0].Key == 0);
            Assert.IsTrue(verticalsList[0].Value == "Warehouse_Solutions");
            Assert.IsTrue(verticalsList[1].Key == 1);
            Assert.IsTrue(verticalsList[1].Value == "Merchandising_Solutions");
            Assert.IsTrue(verticalsList[2].Key == 2);
            Assert.IsTrue(verticalsList[2].Value == "Membership_Solutions");
            Assert.IsTrue(verticalsList[3].Key == 3);
            Assert.IsTrue(verticalsList[3].Value == "Distribution_Solutions");
            Assert.IsTrue(verticalsList[4].Key == 4);
            Assert.IsTrue(verticalsList[4].Value == "International_Solutions");
            Assert.IsTrue(verticalsList[5].Key == 5);
            Assert.IsTrue(verticalsList[5].Value == "Ancillary_Solutions");
            Assert.IsTrue(verticalsList[6].Key == 6);
            Assert.IsTrue(verticalsList[6].Value == "eBusiness_Solutions");
            Assert.IsTrue(verticalsList[7].Key == 7);
            Assert.IsTrue(verticalsList[7].Value == "Corporate_Solutions");
            Assert.IsTrue(verticalsList[8].Key == -1);
            Assert.IsTrue(verticalsList[8].Value == "Not_Assigned");

        }

        [TestMethod()]
        public void GetUpdatesForKeyTest()
        {
            var dataAccess = new AccessService();
            // This function records a status update as a new project if one does not already exist.
            // We want to test two things - does it record a new project, and if a new project already
            // exists, does it store it properly

            // First, let's create a StatusUpdate
            Random random = new Random();
            int randomNumber = random.Next(0, int.MaxValue);
            string projectName = "Access Service Unit Test Project" + randomNumber;
            StatusUpdate newProject = new StatusUpdate();
            newProject.PhaseID = 0;
            newProject.ProjectName = projectName;
            newProject.VerticalID = 3;
            List<StatusUpdate> newProjectsList = new List<StatusUpdate>();
            newProjectsList.Add(newProject);

            dataAccess.RecordStatusUpdate(newProjectsList);

            // Now, let's make sure that the project got added
            Guid projectGuid = dataAccess.GetProjectIDbyName(projectName);

            // Test the updating part of the project. The function only updates one project at a time
            Assert.IsTrue(projectGuid != Guid.Empty);
            StatusUpdate updateProject = newProject;
            updateProject.PhaseID = 1;
            updateProject.UpdateKey = "Environment";
            updateProject.UpdateValue = "QA1";
            StatusUpdate updateProject2 = newProject;
            updateProject2.PhaseID = 2;
            updateProject.UpdateKey = "Random Key";
            updateProject.UpdateValue = "BT1";
            StatusUpdate updateProject3 = newProject;
            updateProject2.PhaseID = 3;
            updateProject.UpdateKey = "Environment";
            updateProject.UpdateValue = "BT3";
            List<StatusUpdate> updateProjectsList = new List<StatusUpdate>();
            updateProjectsList.Add(updateProject);
            updateProjectsList.Add(updateProject2);
            updateProjectsList.Add(updateProject3);
            dataAccess.RecordStatusUpdate(updateProjectsList);

            // Test that the updates got in
            List<StatusUpdate> retrievedProjectUpdates = dataAccess.GetUpdatesForKey("Environment", projectGuid);

            Assert.AreEqual(retrievedProjectUpdates.Count, 2);
            dataAccess.DeleteProject(projectGuid);

        }

        [TestMethod()]
        public void GetAllUpdatesForProjectTest()
        {
            var dataAccess = new AccessService();
            // This function records a status update as a new project if one does not already exist.
            // We want to test two things - does it record a new project, and if a new project already
            // exists, does it store it properly

            // First, let's create a StatusUpdate
            Random random = new Random();
            int randomNumber = random.Next(0, int.MaxValue);
            string projectName = "Access Service Unit Test Project" + randomNumber;
            StatusUpdate newProject = new StatusUpdate();
            newProject.PhaseID = 0;
            newProject.ProjectName = projectName;
            newProject.VerticalID = 3;
            newProject.UpdateKey = "Unit Test Key";
            newProject.UpdateValue = "Unit Test Value";
            List<StatusUpdate> newProjectsList = new List<StatusUpdate>();
            newProjectsList.Add(newProject);

            dataAccess.RecordStatusUpdate(newProjectsList);

            // Now, let's make sure that the project got added
            Guid projectGuid = dataAccess.GetProjectIDbyName(projectName);

            // Test the updating part of the project. The function only updates one project at a time
            StatusUpdate updateProject = newProject;
            updateProject.PhaseID = 1;
            StatusUpdate updateProject2 = newProject;
            updateProject2.PhaseID = 2;
            StatusUpdate updateProject3 = newProject;
            updateProject2.PhaseID = 3;
            List<StatusUpdate> updateProjectsList = new List<StatusUpdate>();
            updateProjectsList.Add(updateProject);
            updateProjectsList.Add(updateProject2);
            updateProjectsList.Add(updateProject3);
            dataAccess.RecordStatusUpdate(updateProjectsList);

            // Test that the updates got in
            List<StatusUpdate> retrievedProjectUpdates = dataAccess.GetAllUpdatesForProject(projectGuid.ToString());
            Assert.AreEqual(retrievedProjectUpdates.Count, 4);
            dataAccess.DeleteProject(projectGuid);
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
            var dataAccess = new AccessService();
            // This function records a status update as a new project if one does not already exist.
            // We want to test two things - does it record a new project, and if a new project already
            // exists, does it store it properly

            // First, let's create a StatusUpdate
            Random random = new Random();
            int randomNumber = random.Next(0, int.MaxValue);
            string projectName = "Access Service Unit Test Project" + randomNumber;
            StatusUpdate newProject = new StatusUpdate();
            newProject.PhaseID = 0;
            newProject.ProjectName = projectName;
            newProject.VerticalID = 3;
            List<StatusUpdate> newProjectsList = new List<StatusUpdate>();
            newProjectsList.Add(newProject);

            dataAccess.RecordStatusUpdate(newProjectsList);

            // Now, let's make sure that the project got added
            Guid projectGuid = dataAccess.GetProjectIDbyName(projectName);

            // Test the updating part of the project. The function only updates one project at a time
            Assert.IsTrue(projectGuid != Guid.Empty);
            StatusUpdate updateProject = newProject;
            updateProject.PhaseID = 1;
            StatusUpdate updateProject2 = newProject;
            updateProject2.PhaseID = 2;
            StatusUpdate updateProject3 = newProject;
            updateProject2.PhaseID = 3;
            List<StatusUpdate> updateProjectsList = new List<StatusUpdate>();
            updateProjectsList.Add(updateProject);
            updateProjectsList.Add(updateProject2);
            updateProjectsList.Add(updateProject3);
            dataAccess.RecordStatusUpdate(updateProjectsList);

            // Test that the updates got in
            List<StatusUpdate> retrievedProjectUpdates = dataAccess.GetAllUpdatesForProject(projectGuid.ToString());
            Assert.AreEqual(retrievedProjectUpdates.Count, 4);

        }

    }
    #endregion
}
