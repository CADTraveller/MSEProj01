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

        public string  Phase { get; set; }

        public int PhaseID { get; set; }
    }
}
