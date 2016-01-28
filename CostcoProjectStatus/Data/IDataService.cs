using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusUpdatesModel;

namespace DataService
{
    interface IDataService
    {
        Task<List<string>> GetAllProjectsAsync();
        Task<List<Project>> GetAllProjectsForVerticalAsync(int verticalID);
        Task<List<string>> GetAllProjectNamesForVerticalAsync(int verticalID);
        Task<List<StatusUpdate>> GetAllUpdatesForProjectAsync(string ProjectID);
        Task<List<StatusUpdate>> GetAllUpdatesForProjectPhaseAsynch(string ProjectID, int PhaseID);

        Task RecordStatusUpdateAsync(ProjectUpdate newUpdate);
    }
}
