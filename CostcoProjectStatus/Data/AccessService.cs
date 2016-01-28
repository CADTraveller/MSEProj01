using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusUpdatesModel;

namespace DataService
{
    public class AccessService : IDataService
    {
        private CostcoDevStatusEntities context;
        private const string ConnectionString = "Server=tcp:costcosu.database.windows.net,1433;Database=CostcoDevStatus;User ID=SUAdmin@costcosu;Password=39ffbJeo;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public AccessService()
        {
            context = CostcoDevStatusEntities.Create(ConnectionString);

        }

        #region Async methods
        public async Task<List<string>> GetAllProjectsAsync()
        {
            return GetAllProjectNames();
        }

        public async Task<List<Project>> GetAllProjectsForVerticalAsync(int verticalID)
        {
            return GetAllProjectsForVertical(verticalID);
        }

        public async Task<List<string>> GetAllProjectNamesForVerticalAsync(int verticalID)
        {
            return GetAllProjectNamesForVertical(verticalID);
        }

        public async Task<List<StatusUpdate>> GetAllUpdatesForProjectAsync(string ProjectID)
        {
            return GetAllUpdatesForProject(ProjectID);
        }

        public async Task<List<StatusUpdate>> GetAllUpdatesForProjectPhaseAsynch(string projectID, int phaseID)
        {
            return GetAllUpdatesForProjectPhase(projectID, phaseID);
        }


        public async Task RecordStatusUpdateAsync(ProjectUpdate newUpdate)
        {
            RecordStatusUpdate(newUpdate);
        }
        #endregion

        private List<StatusUpdate> GetAllUpdatesForProjectPhase(string projectID, int phaseID)
        {
            List<StatusUpdate> updates = new List<StatusUpdate>();
            updates = context.StatusUpdates.Where(p => p.ProjectID == projectID && p.PhaseID == phaseID).ToList();
            return updates;
        }

        public void RecordStatusUpdate(ProjectUpdate projectUpdate)
        {
            List<StatusUpdate> NewUpdates = projectUpdate.Updates;

            //__this information from the projectUpdate will also be written to each status update entry
            DateTime currentDT = DateTime.Now;
            string newUpdateID = projectUpdate.ProjectID;
            int iUpdatePhaseID = NewUpdates[0].PhaseID;// projectUpdate.PhaseID;
            int iVerticalID = projectUpdate.VerticalID;
            int iNewSequenceNumber = 0;

            //__check to see if Project exists, add it if not

            //Project existingProjectEntry = context.Projects.First(p => p.ProjectID == newUpdateID);
            Project existingProjectEntry = context.Projects.FirstOrDefault(p => p.ProjectID == newUpdateID);
            if (existingProjectEntry == null) //_if there is no existing entry then add one
            {
                context.Projects.Add(new Project()
                {
                    ProjectID = newUpdateID,
                    VerticalID = iVerticalID
                });
            }

            ProjectPhase projectPhaseEntry = context.ProjectPhases.FirstOrDefault(p => p.ProjectID == newUpdateID && p.PhaseID == iUpdatePhaseID);

            if (projectPhaseEntry != null)//__there are existing entries for this Project & Phase
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
                    ProjectID = newUpdateID,
                    PhaseID = iUpdatePhaseID,
                    UpdateCount = 0,
                    LatestUpdate = currentDT
                });
            }

            foreach (StatusUpdate u in NewUpdates)
            {
                u.RecordDate = currentDT;
                u.ProjectID = newUpdateID;
                u.PhaseID = iUpdatePhaseID;
                u.VerticalID = iVerticalID;

                u.StatusSequence = iNewSequenceNumber;
                context.StatusUpdates.Add(u);
            }
            context.SaveChanges();
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
        /// <returns></returns>
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

        public List<string> GetAllProjectNames()
        {
            List<string> projectNames = new List<string>();

            try {
                projectNames = context.Projects.Select(p => p.ProjectID).ToList();
            } catch (Exception e) {
                Console.WriteLine("Unsuccessful: {0}",e.ToString());
            }

            //This section is for sample development data and should be removed
            if (projectNames.Count == 0)
            {
                projectNames.Add("Sample Project Names");
                projectNames.Add("Moonbase Alpha");
                projectNames.Add("Costco Member Tracker");
                projectNames.Add("Dog Walker Portal");
                projectNames.Add("Travel Packages Website");
            }
            return projectNames;
        }

        public List<StatusUpdate> GetAllUpdatesForProject(string projectID)
        {
            return context.StatusUpdates.Where(s => s.ProjectID == projectID).ToList();
        }

        public List<Project> GetAllProjectsForVertical(int verticalID)
        {
            //return context.Projects.Where(p => p.VerticalID == verticalID).Select(p => p.ProjectID).ToList();
            return context.Projects.Where(p => p.VerticalID == verticalID).ToList();

        }

        public List<string> GetAllProjectNamesForVertical(int verticalID)
        {
            return context.Projects.Where(p => p.VerticalID == verticalID).Select(p => p.ProjectID).ToList();
        }
    }
}
