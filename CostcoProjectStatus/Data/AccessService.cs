﻿using System;
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

        private void RecordStatusUpdate(ProjectAndUpdates projectUpdate)
        {
            List<StatusUpdate> NewUpdates = projectUpdate.Updates;
            DateTime currentDT = DateTime.Now;
            foreach (StatusUpdate u in NewUpdates)
            {
                u.RecordDate = currentDT;


                string newUpdateID = projectUpdate.ID;
                int iUpdatePhaseID = projectUpdate.PhaseID;
                List<ProjectPhase> projectPhaseEntries = context.ProjectPhases.Where(p => p.ProjectID == newUpdateID && p.PhaseID == iUpdatePhaseID).ToList();

                if (projectPhaseEntries.Count > 0))
            {
                    //__update existing update count and use this for sequence number
                    ProjectPhase existingProjectPhase = projectPhaseEntries[0];
                    int iOldSequenceNumber = Convert.ToInt32(existingProjectPhase.UpdateCount);
                    int iNewSequenceNumber = iOldSequenceNumber + 1;
                    existingProjectPhase.UpdateCount = iNewSequenceNumber;

                    update.StatusSequence = iNewSequenceNumber;
                    Dictionary<string, string> updateInfo = update.

            }

            }

        }
    }
}