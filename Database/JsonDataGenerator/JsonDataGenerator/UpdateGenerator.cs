using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusUpdatesModel;
using DataService;

namespace JsonDataGenerator
{
    public class UpdateGenerator
    {

        public static List<ProjectUpdate> GenerateUpdates(int numberOfProjects)
        {
            List<ProjectUpdate> projectUpdates = new List<ProjectUpdate>();
            List<String> usedNames = new List<string>() { "Start" };
            Random rnd = new Random();

            for (int n = 0; n < 7; n++)//__loop through the number of Verticals
            {

                for (int i = 0; i < numberOfProjects; i++)//__create this many projects
                {
                    string project = "Start";
                    while (usedNames.Contains(project))
                    {
                        int nameIndex = rnd.Next(8);
                        int typeIndex = rnd.Next(10);
                        project = projectNames[nameIndex] + " " + projectTypes[typeIndex] + "-V" + n;
                    }
                    int verticalID = n;// rnd.Next(7);

                   int iNumPhases = rnd.Next(7);
                    for (int p = 0; p < iNumPhases; p++)//__loop through this number of phases
                    {


                        int iNumberEmails = rnd.Next(12) + 1;
                        for (int m = 0; m < iNumberEmails; m++)//__generate m emails for this Project, Phase
                        {
                            //__we want only 1 Environemnt and 1 Task per email
                            bool TaskNotRecorded = true;
                            bool EnvironmentNotRecorded = true;
                            List<string> usedIdentifiers = new List<string>();

                            ProjectUpdate newProjectUpdate = new ProjectUpdate();

                            int numberOfDataPoints = rnd.Next(11) + 1;
                            for (int k = 0; k < numberOfDataPoints; k++)//__generate k data points for this email
                            {

                                StatusUpdate update = new StatusUpdate();
                                update.ProjectID = project;
                                update.PhaseID = p;// rnd.Next(6),//= update.PhaseID,
                                update.VerticalID = n;
                                update.RecordDate = DateTime.Now;
                                int doTagSwitch = rnd.Next(6);

                                if (doTagSwitch < 3 && EnvironmentNotRecorded)
                                {

                                    update.UpdateKey = "Environment";
                                    update.UpdateValue = "System #" + rnd.Next(12);
                                    EnvironmentNotRecorded = false;
                                    
                                }
                                 else if (doTagSwitch < 3 && TaskNotRecorded)
                                {

                                    update.UpdateKey = "Task";
                                    update.UpdateValue = "Task #" + rnd.Next(7);
                                    TaskNotRecorded = false;
                                }
                                else
                                {
                                    string identifier = rnd.Next(8) + "." + rnd.Next(8);
                                    //__ensure there are no duplicate keys in a given email
                                    while(usedIdentifiers.Contains(identifier)) identifier = rnd.Next(8) + "." + rnd.Next(8);
                                        update.UpdateKey = "Key." + identifier;
                                    update.UpdateValue = "Value." + identifier;
                                }
                                newProjectUpdate.Updates.Add(update);
                            }//__end loop through data points
                            projectUpdates.Add(newProjectUpdate);//__Add this email to the list to return
                        }//__end loop through emails
                    }//__end loop through Phases
                }//__end loop through Projects
            }//__end loop through Verticals
            return projectUpdates;
        }

        private static List<string> projectNames = new List<string>()
        {
            "Website",
            "Enabler",
            "Manager",
            "Portal",
            "Monitor",
            "Thingamajig",
            "Driver",
            "Checker"
        };

        private static List<string> projectTypes = new List<string>()
        {
            "Jimmy",
            "Martha",
            "Stewart",
            "Schenectady",
            "Bocephus",
            "Aardvark",
            "Samantha",
            "Trippy",
            "Vermicious Knid",
            "Particulate"
        };

        private static List<string> verticals = new List<string>()
        { "eBusiness Solutions",
            "Membership Solutions",
            "Warehouse Solutions",
            "Distribution Solutions",
            "Corporate Solutions",
            "Ancillary Solutions",
        "Merchandising Solutions",
        "International Solutions"
        };

        private static List<string> phases = new List<string>()
        {
            "Start Up",
            "Solution Outline",
            "Macro Design",
            "Micro Design",
            "Build & Test",
            "Deploy",
            "Transition & Close"
        };


    }




}
