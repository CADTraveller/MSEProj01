using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataService
{
    public class ProjectUpdate
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int VerticalID { get; set; }

        public List<StatusUpdatesModel.StatusUpdate> Updates { get; set; }
    }

}
