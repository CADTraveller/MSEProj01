using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDataGenerator
{
    public class StatusUpdate
    {
        public StatusUpdate()
        {
            info = new Dictionary<string, string>();
        }

        private string projectID;
        public string ProjectID
        {
            get { return projectID; }
            set { projectID = value; }
        }


        private int phaseID;
        public int PhaseID
        {
            get { return phaseID; }
            set { phaseID = value; }
        }


        private int verticalID;
        public int VerticalID
        {
            get { return verticalID; }
            set { verticalID = value; }
        }


        private Dictionary<string, string> info;
        public Dictionary<string, string> Info
        {
            get { return info; }
            set { info = value; }
        }




        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

    }
}
