using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService
{
    [Serializable]
    public class UpdatePackage
    {
        public string ProjectName;
        public string Subject;
        public string Body;

        public Dictionary<string, string> Updates = new Dictionary<string, string>();
    }
}
