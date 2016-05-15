using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    public class UpdatePackage
    {
        public string ProjectName;
        public string Subject;
        public string Body;
        public List<KeyValuePair<string, string>> Updates;
    }
}
