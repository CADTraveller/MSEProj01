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
                string newUpdateID = updates[0].ProjectID;

                //__check to see if Project exists, add it if not
                Project existingProjectEntry = context.Projects.FirstOrDefault(p => p.ProjectID == newUpdateID);
                if (existingProjectEntry == null) //_if there is no existing entry then add one
                {
                    context.Projects.Add(new Project()
                    {
                        ProjectID = newUpdateID,
                        VerticalID = updates[0].VerticalID
                    });
                }

                int iUpdatePhaseID = updates[0].PhaseID;
                int iNewSequenceNumber = 0;
               
                ProjectPhase projectPhaseEntry = context.ProjectPhases.FirstOrDefault(p => p.ProjectID == newUpdateID && p.PhaseID == iUpdatePhaseID);

                // check for existing entries for this Project & Phase
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
                        ProjectID = newUpdateID,
                        PhaseID = iUpdatePhaseID,
                        UpdateCount = 0,
                        LatestUpdate = currentDT
                    });
                }

                foreach (StatusUpdate u in updates)
                {
                    u.StatusSequence = iNewSequenceNumber;
                    context.StatusUpdates.Add(u);
                }
                context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        private List<StatusUpdate> GetAllUpdatesForProjectPhase(string projectID, int phaseID)
        {
            List<StatusUpdate> updates = new List<StatusUpdate>();
            updates = context.StatusUpdates.Where(p => p.ProjectID == projectID && p.PhaseID == phaseID).ToList();
            return updates;
        }

        public List<Project> GetAllProjectNames()
        {
            return context.Projects.AsEnumerable().ToList();


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

        #endregion
    }
}
