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

            //__check for existence of this project by ID, Name
            Guid projectID = updates.FirstOrDefault(u => u.ProjectID != null).ProjectID;
            string projectName = updates.FirstOrDefault(u => !string.IsNullOrEmpty(u.ProjectName)).ProjectName;
            int? verticalID = updates.FirstOrDefault(u => u.VerticalID != null).VerticalID;
            if (verticalID == null || verticalID < 0 || verticalID > 7) verticalID = 0;
            bool hasID = projectID != Guid.Empty;
            bool hasName = !string.IsNullOrEmpty(projectName);
            if (!hasID && !hasName) return null;//__cannot record anonymous updates

            if (hasID)
            {
                Project recordedProject = context.Projects.FirstOrDefault(p => p.ProjectID == projectID);
                //__if no name is provided, use a placeholder

                if (recordedProject == null) //__must  be a new project
                {
                    if (!hasName) projectName = "**Name Not Set**";
                    context.Projects.Add(new Project()
                    {
                        ProjectID = projectID,
                        ProjectName = projectName,
                        VerticalID = verticalID
                    });
                }
                else //__project already exists, so simply check if we need to update the name
                {
                    if (recordedProject.ProjectName != projectName)
                        recordedProject.ProjectName = projectName;
                }
                context.SaveChanges();
            }
            else
            {
                //__reaching this code means we have a name but no ID
                //__see if any ID Is already recorded
                Project recordedProject = context.Projects.FirstOrDefault(p => p.ProjectName == projectName);
                if (recordedProject != null) projectID = recordedProject.ProjectID;
                else //__this must be new project, generate ID and record
                {
                    projectID = Guid.NewGuid();
                    context.Projects.Add(new Project()
                    {
                        ProjectID = projectID,
                        ProjectName = projectName,
                        VerticalID = verticalID
                    });
                }
                context.SaveChanges();
            }
            try
            {

                DateTime currentDT = DateTime.Now;

                foreach (StatusUpdate u in updates)
                {
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
                    if (u.ProjectID == Guid.Empty) u.ProjectID = projectID;
                    if (string.IsNullOrEmpty(u.ProjectName)) u.ProjectName = projectName;
                    if (u.VerticalID == null || u.VerticalID < 0 || u.VerticalID > 7) u.VerticalID = verticalID;
                    Console.WriteLine("\n--Added Update| updateKey=" + u.UpdateKey + ", updateValue=" + u.UpdateValue);
                    context.StatusUpdates.Add(u);

                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
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
        public List<Vertical> GetAllVerticals()
        {
            return context.Verticals.AsEnumerable().ToList();
        }
        

        public List<Project> GetProjectIDs(string projectName = "", int verticalID = -1)
        {
            List<Project> projects = new List<Project>();
            bool bHaveName = !string.IsNullOrEmpty(projectName);
            bool bHaveVertical = verticalID >= 0;
            if (bHaveName && bHaveVertical)
            {
                projects = context.Projects.Where(p =>
                p.ProjectName == projectName &&
                p.VerticalID == verticalID).ToList();
            }
            else if (bHaveName && !bHaveVertical)
            {
                projects = context.Projects.Where(p => p.ProjectName == projectName).ToList();
            }
            else if (!bHaveName && bHaveVertical)
            {
                projects = context.Projects.Where(p => p.VerticalID == verticalID).ToList();
            }
            return projects;
        }

        #endregion
    }
}
