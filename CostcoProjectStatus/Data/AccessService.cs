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
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Project>> GetAllProjectsForVerticalAsync(Vertical vertical)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StatusUpdate>> GetAllUpdatesForProjectAsync(string ProjectID)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StatusUpdate>> GetAllUpdatesForProjectPhaseAsynch(string ProjectID, Phase phase)
        {
            throw new NotImplementedException();
        }

        public async Task RecordStatusUpdateAsync(StatusUpdate newUpdate)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void RecordStatusUpdate(ProjectUpdate projectUpdate)
        {
            List<StatusUpdate> NewUpdates = projectUpdate.Updates;

            //__this information from the projectUpdate will also be written to each status update entry
            DateTime currentDT = DateTime.Now;
            string newUpdateID = projectUpdate.ProjectID;
            int iUpdatePhaseID = projectUpdate.PhaseID;
            int iVerticalID = projectUpdate.VerticalID;
            int iNewSequenceNumber = 0;

            //__check to see if Project exists, add it if not

            //Project existingProjectEntry = context.Projects.First(p => p.ProjectID == newUpdateID);
            Project existingProjectEntry =context.Projects.FirstOrDefault(p => p.ProjectID == newUpdateID);
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
            return true;
        }

        public List<string> GetAllProjectNames()
        {
            List<string> projectNames = new List<string>();


            projectNames = context.Projects.Select(p => p.ProjectID).ToList();

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

        public List<string> GetAllProjectsForVertical(int verticalID)
        {
            return context.Projects.Where(p => p.VerticalID == verticalID).Select(p => p.ProjectID).ToList();
        }
    }
}
