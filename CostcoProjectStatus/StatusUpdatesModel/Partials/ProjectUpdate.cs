using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusUpdatesModel
{
    public partial class ProjectUpdate
    {
        public string Date { get; set; }

        public string  Environment { get; set; }

        public string Description { get; set; }

        private string phase;
        public string Phase
        {
            get
            {
                phase = Phases.Not_Assigned.ToString();
                if (StatusUpdates.Count > 0)
                {
                    try
                    {

                    phase = Enum.GetName(typeof(Phases), StatusUpdates.First().PhaseID);
                    }
                    catch (Exception)
                    {
                        phase = Phases.Not_Assigned.ToString();
                    }
                }
                return phase;
            }
             
            set { phase = value; }
        }

        public int PhaseID { get; set; }
    }
}
