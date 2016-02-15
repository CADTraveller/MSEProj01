using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusUpdatesModel;

namespace DataService
{
    public class AccessService
    {
        private CostcoDevStatusEntities context;
        private const string ConnectionString = "Server=tcp:costcosu.database.windows.net,1433;Database=CostcoDevStatus;User ID=SUAdmin@costcosu;Password=39ffbJeo;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public AccessService()
        {
            context = CostcoDevStatusEntities.Create(ConnectionString);

        }

        #region Async methods
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return GetAllProjectNames();
        }

        public async Task<List<Project>> GetAllProjectsForVerticalAsync(int verticalID)
        {
            return GetAllProjectsForVertical(verticalID);
        }

        public async Task<List<StatusUpdate>> GetAllUpdatesForProjectAsync(string ProjectID)
        {
            return GetAllUpdatesForProject(ProjectID);
        }

        public async Task<List<StatusUpdate>> GetAllUpdatesForProjectPhaseAsynch(string projectID, int phaseID)
        {
            return GetAllUpdatesForProjectPhase(projectID, phaseID);
        }


        public async Task RecordStatusUpdateAsync(List<StatusUpdate> newUpdate)
        {
            RecordStatusUpdate(newUpdate);
        }
        #endregion


        #region Authentication Methods
        public bool AddUser(string email, int userRole)
        {
            if (context.AllowedUsers.Any(a => a.Email == email)) return false;
            try
            {

                AllowedUser newUser = new AllowedUser()
                {
                    Email = email,
                    UserID = new Guid(),
                    RoleID = userRole
                };
                context.AllowedUsers.Add(newUser);
                context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool IsUserAuthorized(string email)
        {
            return context.AllowedUsers.Any(u => u.Email == email);
        }

        /// <summary>
        /// Checks for the user in the database. If found RoleID (0,1,2) is returned
        /// If user is not found, -1 is returned
        /// </summary>
        /// <param name="email"></param>
        /// <returns>int indicating user role, or -1</returns>
        public int GetUserRole(string email)
        {
            int result = -1;
            AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                result = user.RoleID;
            }
            return result;
        }


        public bool UpdateUserRole(string email, int newUserRole)
        {
            if (context.AllowedUsers.Any(a => a.Email == email)) return false;
            try
            {

                AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == email);
                user.RoleID = newUserRole;
                context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool DeleteUser(string email)
        {
            try
            {
                AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    context.AllowedUsers.Remove(user);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateUserEmail(string oldEmail, string newEmail)
        {
            AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == oldEmail);
            if (user == null) return false;
            user.Email = newEmail;
            context.SaveChanges();
            return true;
        }
        #endregion


        #region StatusUpdate and Project Methods

        public bool? RecordStatusUpdate(List<StatusUpdate> updates)
        {
            //__safety check, cannot record an empty list
            if (updates.Count == 0) return null;
            try
            {

                DateTime currentDT = DateTime.Now;
                //__check for values that should be constant across updates
                //___ProjectID, ProjectName, VerticalID;
                StatusUpdate updateRef = updates.First();
                Guid projectGuid = updateRef.ProjectID;
                string projectName = updateRef.ProjectName;
                int verticalID = Convert.ToInt32( updateRef.VerticalID);

                foreach (StatusUpdate u in updates)
                {

                    //__check to see if Project exists, add it if not
                    Project existingProjectEntry = context.Projects.FirstOrDefault(p => p.ProjectID == u.ProjectID);
                    if (existingProjectEntry == null) //_if there is no existing entry then add one
                    {
                        context.Projects.Add(new Project()
                        {
                            ProjectID = u.ProjectID,
                            VerticalID = u.VerticalID,
                            ProjectName = u.ProjectName
                        });
                        Console.WriteLine("\nCreated Project:" + u.ProjectName + " With ID:" + u.ProjectID);
                        context.SaveChanges();
                    }
                    
                    int iNewSequenceNumber = 0;

                    // check for existing entries for this Project & Phase & UpdateKey
                    ProjectPhase projectPhaseEntry = context.ProjectPhases.FirstOrDefault(
                        p => p.ProjectID == u.ProjectID &&
                        p.PhaseID == u.PhaseID &&
                        p.UpdateKey == u.UpdateKey);

                    if (projectPhaseEntry != null)//__an entry exists
                    {
                        //__update existing update count and use this for sequence number
                        int iOldSequenceNumber = Convert.ToInt32(projectPhaseEntry.UpdateCount);
                        iNewSequenceNumber = iOldSequenceNumber + 1;
                        projectPhaseEntry.UpdateCount = iNewSequenceNumber;
                        projectPhaseEntry.LatestUpdate = currentDT;
                    }
                    else //__since none was found we need a new entry
                    {
                        context.ProjectPhases.Add(new ProjectPhase()
                        {
                            ProjectID = u.ProjectID,
                            PhaseID = u.PhaseID,
                            UpdateKey = u.UpdateKey,
                            UpdateCount = 0,
                            LatestUpdate = currentDT
                        });
                        context.SaveChanges();
                    }

                    u.StatusSequence = iNewSequenceNumber;
                    Console.WriteLine("\n--Added Update| updateKey=" + u.UpdateKey + ", updateValue=" + u.UpdateValue);
                    context.StatusUpdates.Add(u);                   

                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
            return true;
        }


        private List<StatusUpdate> GetAllUpdatesForProjectPhase(string projectID, int phaseID)
        {
            Guid projectGuid = new Guid(projectID);
            List<StatusUpdate> updates = new List<StatusUpdate>();
            updates = context.StatusUpdates.Where(p => p.ProjectID == projectGuid && p.PhaseID == phaseID).ToList();
            return updates;
        }

        public List<Project> GetAllProjectNames()
        {
            List<Project> projects = context.Projects.AsEnumerable().ToList();
            DateTime now = DateTime.Now;
            foreach (var project in projects)
            {
                project.LatestUpdate = now;
            }
            return projects;

        }

        public List<StatusUpdate> GetAllUpdatesForProject(string projectID)
        {
            Guid projectGuid = new Guid(projectID);
            return context.StatusUpdates.Where(s => s.ProjectID == projectGuid).ToList();
        }

        public List<Project> GetAllProjectsForVertical(int verticalID)
        {
            //return context.Projects.Where(p => p.VerticalID == verticalID).Select(p => p.ProjectID).ToList();
            return context.Projects.Where(p => p.VerticalID == verticalID).ToList();

        }

        public List<StatusUpdate> GetAllUpdatesFromEmail(string projectID, int phaseID, int statusSequence)
        {
            Guid projectGuid = new Guid(projectID);
            return context.StatusUpdates.Where(su =>
            su.ProjectID == projectGuid &&
            su.PhaseID == phaseID &&
            su.StatusSequence == statusSequence).ToList();
        }

        #endregion
    }
}
