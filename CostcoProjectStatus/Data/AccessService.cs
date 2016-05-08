using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusUpdatesModel;
using Newtonsoft.Json;


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

     


        #region Authentication Methods
        public bool AddUser(string email, int userRole)
        {
           
            if (context.AllowedUsers.Any(a => a.Email == email)) return false;
            try
            {
         
                AllowedUser newUser = new AllowedUser()
                {
                    Email = email,
                    UserID = Guid.NewGuid(),
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
        public bool IsAppAuthorized(string app)
        {
            return context.AllowedApps.Any(u => u.Name == app);
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
            if (!context.AllowedUsers.Any(a => a.Email == email)) return false;
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

        public bool UpdateUserEmail(Guid userID, string newEmail)
        {
            AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.UserID == userID);
            if (user == null) return false;
            user.Email = newEmail;
            context.SaveChanges();
            return true;
        }

        public Guid GetUserID(string email)
        {
            AllowedUser user = context.AllowedUsers.FirstOrDefault(u => u.Email == email);
            if (user == null) return Guid.Empty;
            return user.UserID;
        }
        #endregion


        #region StatusUpdate and Project Methods

        public bool RecordProjectUpdate(ProjectUpdate projectUpdate)
        {

            try
            {
                List<StatusUpdate> updates = projectUpdate.StatusUpdates.ToList();

                //__first make sure each StatusUpdate has necessary info'
                Guid projectUpdateId = Guid.NewGuid();
                foreach (StatusUpdate statusUpdate in updates)
                {
                    statusUpdate.ProjectUpdateID = projectUpdateId;
                }

                //__create new entry in ProjectUpdate table
                projectUpdate.ProjectUpdateID = projectUpdateId;
                context.ProjectUpdates.Add(projectUpdate);
                context.SaveChanges();

                //__use existing method to record StatusUpdates
                RecordStatusUpdate(updates);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool? RecordStatusUpdate(List<StatusUpdate> updates)
        {
            //__safety check, cannot record an empty list
            if (updates.Count == 0) return null;
            StatusUpdate refUpdate = updates[0];           

            //__check for existence of this project by ID, Name
            Guid projectID = refUpdate.ProjectID;
            string projectName = refUpdate.ProjectName;
            int? verticalID = refUpdate.VerticalID;
            if (verticalID == null ||  verticalID > 7) verticalID = -1;
            int? phaseID = refUpdate.PhaseID;
            if (phaseID == null || phaseID > 6) phaseID = -1;
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
                //__make sure all updates have proper ProjectID, PhaseID, and VerticalID as these might have 
                //___changed or been generated here
                foreach (StatusUpdate u in updates)
                {
                    u.ProjectID = projectID;
                    u.PhaseID = phaseID;
                    u.VerticalID = verticalID;
                    u.ProjectName = projectName;
                }


                DateTime currentDT = DateTime.Now;

                ////__first record the raw update data as ProjectUpdate
                ////__generate an ID for this update and save the raw data
                //JsonSerializerSettings settings = new JsonSerializerSettings();
                //settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                
                //string updateJson = JsonConvert.SerializeObject(updates, settings);
                //StatusUpdatesModel.ProjectUpdate projectUpdate = new StatusUpdatesModel.ProjectUpdate();
                //Guid projectUpdateID = Guid.NewGuid();
                //projectUpdate.ProjectUpdateID = projectUpdateID;
                //projectUpdate.Body = updateJson;
                //projectUpdate.ProjectID = projectID;
                //context.ProjectUpdates.Add(projectUpdate);

                foreach (StatusUpdate u in updates)
                {
                    

                    // check for existing entries for this Project & Phase & UpdateKey
                    ProjectPhase projectPhaseEntry = context.ProjectPhases.FirstOrDefault(
                        p => p.ProjectID == u.ProjectID &&
                        p.PhaseID == u.PhaseID &&
                        p.UpdateKey == u.UpdateKey);

                    if (projectPhaseEntry != null)//__an entry exists
                    {
                        //__update existing update count and use this for sequence number
                        int iOldCount = Convert.ToInt32(projectPhaseEntry.UpdateCount);
                       int  iNewCount = iOldCount + 1;
                        projectPhaseEntry.UpdateCount = iNewCount;
                        projectPhaseEntry.LatestUpdate = currentDT;
                    }
                    else //__since none was found we need a new entry
                    {
                        context.ProjectPhases.Add(new ProjectPhase()
                        {
                            ProjectID = u.ProjectID,
                            PhaseID = Convert.ToInt16(u.PhaseID),
                            UpdateKey = u.UpdateKey,
                            UpdateCount = 0,
                            LatestUpdate = currentDT
                        });
                        context.SaveChanges();
                    }

                    
                    if (u.ProjectID == Guid.Empty) u.ProjectID = projectID;
                    if (string.IsNullOrEmpty(u.ProjectName)) u.ProjectName = projectName;
                    if (u.VerticalID == null || u.VerticalID < 0 || u.VerticalID > 7) u.VerticalID = verticalID;
                    u.RecordDate = DateTime.Now;
                    //u.ProjectUpdateID = projectUpdateID; This needs to be recorded in RecordProjectUpdateMethod
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

            //__now also write ProjectName to each update
            foreach (StatusUpdate update in updates)
            {
                update.ProjectName = context.Projects.FirstOrDefault(p => p.ProjectID == update.ProjectID).ProjectName;
            }
            return updates;
        }

        public List<Project> GetAllProjectNames()
        {
            List<Project> projects = context.Projects.AsEnumerable().ToList();
            DateTime now = DateTime.Now;
            foreach (var project in projects)
            {                
                List<ProjectUpdate> recordedProjectUpdates = context.ProjectUpdates.Where(pu => pu.ProjectID == project.ProjectID).ToList();
                if (recordedProjectUpdates.Count == 0) continue;

                IEnumerable <DateTime> dates = recordedProjectUpdates.Select(pu => Convert.ToDateTime(pu.Date));
                DateTime lastUpdateDate = dates.Max();
                project.LatestUpdate = lastUpdateDate;
            }
            return projects;
        }

        public List<StatusUpdate> GetAllUpdatesForProject(string projectID)
        {
            Guid projectGuid = new Guid(projectID);
            string projectName = context.Projects.FirstOrDefault(p => p.ProjectID == projectGuid).ProjectName;
            if (string.IsNullOrEmpty(projectName)) return new List<StatusUpdate>();//__return empty list when project not found
            var updates = context.StatusUpdates.Where(s => s.ProjectID == projectGuid).ToList();
            foreach (var update in updates) update.ProjectName = projectName;
            
            return updates;
        }

        public List<ProjectUpdate> GetProjectUpdates(string projectID)
        {
            Guid projectGuid = Guid.Parse(projectID);
            List < ProjectUpdate > projectUpdates = context.ProjectUpdates.Where(pu => pu.ProjectID == projectGuid).ToList();
            
            //_now need to populate required column headings for more efficient client-side processing
            foreach (ProjectUpdate projectUpdate in projectUpdates)
            {
                //__some Properties should be available on all StatusUpdates
                List<StatusUpdate> statusUpdates = projectUpdate.StatusUpdates.ToList();
                StatusUpdate referenceUpdate = statusUpdates.FirstOrDefault();
                if (referenceUpdate == null) continue;//__should not happen
                projectUpdate.Date = referenceUpdate.RecordDate.ToString();
                int phaseIndex = referenceUpdate.PhaseID.Value;
                //__correct for "Not Assigned" value which is -1
                phaseIndex = phaseIndex < 0 ? 7 : phaseIndex;
                projectUpdate.Phase = Enum.GetNames(typeof(Phases))[phaseIndex]; 

                //__will need to look for Environment and Description
                StatusUpdate environmentUpdate = statusUpdates.FirstOrDefault(su => su.UpdateKey == "Environment");
                projectUpdate.Environment = environmentUpdate == null ? "--" : environmentUpdate.UpdateValue;

                StatusUpdate descriptionUpdate = statusUpdates.FirstOrDefault(su => su.UpdateKey == "Description");
                projectUpdate.Description = descriptionUpdate == null ? "--" : descriptionUpdate.UpdateValue;

            }
            return projectUpdates;
        } 

        public List<StatusUpdate> GetUpdatesForKey(string updateKey, Guid? projectID = null, int phaseID = -2,
            bool getOnlyLatest = false)
        {

            bool getUpdatesForSpecificProject = projectID != null;
            bool getUpdatesForSpecificPhase = phaseID >= -1;

            //__first get just the updates with the key of interest
            List<StatusUpdate> updates = context.StatusUpdates.Where(su => su.UpdateKey == updateKey).ToList();

                
            if (updates.Count == 0) return updates;//__nothing found, return empty list

            if (getUpdatesForSpecificProject)
                updates = updates.Where(su => su.ProjectID == projectID).ToList();
            if (updates.Count == 0) return updates; //__still nothing found

            if (getUpdatesForSpecificPhase) updates = updates.Where(su => su.PhaseID == phaseID).ToList();
            if (updates.Count == 0) return updates;

            if (getOnlyLatest)
            {
                updates.OrderBy(su => su.RecordDate);
                StatusUpdate lastUpdate = updates.Last();
                updates.Clear();
                updates.Add(lastUpdate);
            }

            //__now also write ProjectName to each update
            foreach (StatusUpdate update in updates)
            {
                update.ProjectName = context.Projects.FirstOrDefault(p => p.ProjectID == update.ProjectID).ProjectName;
            }
            return updates;
        }

        public List<KeyValuePair<int, string>> GetAllVerticals()
        {
            string[] names = Enum.GetNames(typeof (Verticals));
            int[] values = (int[])Enum.GetValues(typeof (Verticals));
            List<KeyValuePair<int, string>> verticals = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < names.Length; i++)
            {
                verticals.Add(new KeyValuePair<int, string>(values[i], names[i]));
            }
            return verticals;
        }  

        public List<Project> GetAllProjectsForVertical(int verticalID)
        {
            //return context.Projects.Where(p => p.VerticalID == verticalID).Select(p => p.ProjectID).ToList();
            var projects = context.Projects.Where(p => p.VerticalID == verticalID).ToList();
            foreach (Project project in projects)
            {
                Guid projectID = project.ProjectID;
                var lastUpdateDate = context.ProjectPhases.Where(p => p.ProjectID == projectID && p.LatestUpdate != null).Max(p => p.LatestUpdate);
                project.LatestUpdate = (DateTime)lastUpdateDate;
            }
            return projects;
        }

        public List<StatusUpdate> GetAllUpdatesFromEmail(string projectID,  Guid projectUpdateID)
        {
            Guid projectGuid = new Guid(projectID);
            string projectName = context.Projects.FirstOrDefault(p => p.ProjectID == projectGuid)?.ProjectName;
            if (string.IsNullOrEmpty(projectName )) return new List<StatusUpdate>();//___return empty list when project name not found
            ;
            var updates = context.StatusUpdates.Where(su => su.ProjectUpdateID == projectUpdateID).ToList();
            //var updates = context.StatusUpdates.Where(su =>
            //su.ProjectID == projectGuid &&
            //su.PhaseID == phaseID &&
            //su.StatusSequence == statusSequence).ToList();

            //__now also write ProjectName to each update
            foreach (StatusUpdate update in updates)
            {
                update.ProjectName = projectName;
            }
            return updates;
        }

        public bool UpdateProjectUpdatePhase(StatusUpdate update, int newPhase)
        {
            if (update == null || newPhase < 0 || newPhase > Enum.GetNames(typeof(Phases)).Length) return false;

            int oldPhase = update.PhaseID.Value;
            Guid projectID = update.ProjectID;
            string updateKey = update.UpdateKey;
            Guid projectUpdateID = update.ProjectUpdateID;
            var updates = context.StatusUpdates.Where(u => u.ProjectUpdateID == projectUpdateID);
            foreach (StatusUpdate statusUpdate in updates)
            {
                statusUpdate.PhaseID = newPhase;

                //__also update any related entries in ProjectPhase table
                ProjectPhase pp = context.ProjectPhases.FirstOrDefault(p => p.PhaseID == oldPhase
                                                                            && p.ProjectID == projectID
                                                                            && p.UpdateKey == updateKey);
                if (pp != null) pp.PhaseID = newPhase;
            }
            return true;
        }

        public bool UpdateProjectVertical(int newVerticalID, Guid? projectID = null, string projectName = "")
        {
            //__validate arguments
            if (newVerticalID < -1 || newVerticalID > 7) return false;
            bool haveNoId = projectID == null || projectID == Guid.Empty;
            bool haveNoName = string.IsNullOrEmpty(projectName);
            if ( haveNoId && haveNoName) return false;

            projectName = projectName.Trim();

            Project recordedProject;
            if (!haveNoId) recordedProject = context.Projects.FirstOrDefault(p => p.ProjectID == projectID);
            else recordedProject = context.Projects.FirstOrDefault(p => p.ProjectName == projectName);

            //__exit if no project found
            if (recordedProject == null) return false;

            recordedProject.VerticalID = newVerticalID;
            return true;
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

        public Guid GetProjectIDbyName(string projectName)
        {
            Project project = context.Projects.FirstOrDefault(p => p.ProjectName == projectName);
            Guid? projectID = project?.ProjectID ;
            if (projectID == null) projectID = Guid.Empty;
            return (Guid)projectID;
        }

        public string GetProjectNameForID(Guid projectID)
        {
            return context.Projects.FirstOrDefault(p => p.ProjectID == projectID).ProjectName;
        }

        public bool DeleteProject(Guid projectID)//__this must be done in a specific order so as to not violate SQL integrity
        {
            try
            {
                //__first remove all related StatusUpdates
                var statusUpdatesToRemove = context.StatusUpdates.Where(su => su.ProjectID == projectID);
                context.StatusUpdates.RemoveRange(statusUpdatesToRemove);
                context.SaveChanges();

                //__Remove projectupdates
                var projectUpdatesToRemove = context.ProjectUpdates.Where(pu => pu.ProjectID == projectID);
                context.ProjectUpdates.RemoveRange(projectUpdatesToRemove);
                context.SaveChanges();

                //__Remove all projectPhase entries
                var projectPhaseToRemove = context.ProjectPhases.Where(pp => pp.ProjectID == projectID);
                context.ProjectPhases.RemoveRange(projectPhaseToRemove);
                context.SaveChanges();

                //__Finally remove the Project entry itself
                Project projectToRemove = context.Projects.FirstOrDefault(p => p.ProjectID == projectID);
                context.Projects.Remove(projectToRemove);
                context.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
