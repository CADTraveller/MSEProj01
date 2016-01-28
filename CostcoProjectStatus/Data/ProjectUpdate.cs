using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataService
{
    public class ProjectUpdate
    {

       public ProjectUpdate()
        {
            Updates = new List<StatusUpdatesModel.StatusUpdate>();
        }
            public string ProjectID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int VerticalID { get; set; }


        public List<StatusUpdatesModel.StatusUpdate> Updates { get; set; }
    }

}
