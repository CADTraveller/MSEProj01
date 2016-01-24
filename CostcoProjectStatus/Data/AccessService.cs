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

        public AccessService()
        {
            context = new CostcoDevStatusEntities();
        }
        public Task<List<Project>> GetAllProjectsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetAllProjectsForVerticalAsync(Vertical vertical)
        {
            throw new NotImplementedException();
        }

        public Task<List<StatusUpdate>> GetAllUpdatesForProjectAsync(string ProjectID)
        {
            throw new NotImplementedException();
        }

        public Task<List<StatusUpdate>> GetAllUpdatesForProjectPhaseAsynch(string ProjectID, Phase phase)
        {
            throw new NotImplementedException();
        }

        public Task RecordStatusUpdateAsync(StatusUpdate newUpdate)
        {
            throw new NotImplementedException();
        }

        public void RecordStatusUpdate(ProjectUpdate projectUpdate)
        {
            List<StatusUpdate> NewUpdates = projectUpdate.Updates;
            DateTime currentDT = DateTime.Now;
            foreach (StatusUpdate u in NewUpdates)
            {
                u.RecordDate = currentDT;


                string newUpdateID = projectUpdate.ID;
                int iUpdatePhaseID = u.PhaseID;
                List<ProjectPhase> projectPhaseEntries = context.ProjectPhases.Where(p => p.ProjectID == newUpdateID && p.PhaseID == iUpdatePhaseID).ToList();
                int iNewSequenceNumber = 0;
                if (projectPhaseEntries.Count > 0)
                {
                    //__update existing update count and use this for sequence number
                    ProjectPhase existingProjectPhase = projectPhaseEntries[0];
                    int iOldSequenceNumber = Convert.ToInt32(existingProjectPhase.UpdateCount);
                    iNewSequenceNumber = iOldSequenceNumber + 1;
                    existingProjectPhase.UpdateCount = iNewSequenceNumber;
                }

                u.StatusSequence = iNewSequenceNumber;
                context.StatusUpdates.Add(u);
            }
            context.SaveChanges();
        }

        public bool IsUserAuthorized(string email)
        {
            return true;
        }

        public List<string> GetAllProjectNamess()
        {
            List<string> projectNames = new List<string>();


            projectNames = context.Projects.Select(p => p.Name).ToList();

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
    }
}
