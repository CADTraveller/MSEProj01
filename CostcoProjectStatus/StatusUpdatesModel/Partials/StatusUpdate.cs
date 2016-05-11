using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusUpdatesModel
{
    public partial class StatusUpdate
    {
        public string ProjectName;

        public StatusUpdate Clone()
        {
            StatusUpdate newStatusUpdate = new StatusUpdate();
            newStatusUpdate.ProjectID = ProjectID;
            newStatusUpdate.PhaseID = PhaseID;
            newStatusUpdate.VerticalID = VerticalID;
            newStatusUpdate.ProjectUpdateID = ProjectUpdateID;
            return newStatusUpdate;
        }
    }

}
