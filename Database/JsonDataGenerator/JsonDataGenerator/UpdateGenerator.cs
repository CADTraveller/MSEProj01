using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonDataGenerator
{
    public class UpdateGenerator
    {

        public static List<StatusUpdate> GenerateUpdates(int numberOfProjects)
        {
            List<StatusUpdate> updates = new List<StatusUpdate>();
            Random rnd = new Random();
            for (int i = 0; i < numberOfProjects; i++)
            {
                string project = projectNames[rnd.Next(4)] + " " + projectTypes[rnd.Next(3)];
                int verticalID = rnd.Next(7);
                int numberOfUpdates = rnd.Next(11) + 1;

                for (int k = 0; k < numberOfUpdates; k++)
                {
                    StatusUpdate update = new StatusUpdate();
                    update.ProjectID = project;
                    update.PhaseID = rnd.Next(6);
                    update.VerticalID = verticalID;

                    int dataPointCount = rnd.Next(4) + 2;
                    bool TaskNotRecorded = true;

                    for (int m = 0; m < dataPointCount; m++)
                    {

                        int doTagSwitch = rnd.Next(6);
                        if (doTagSwitch < 3 && TaskNotRecorded)
                        {
                            update.Info.Add("Environment", "System #" + rnd.Next(12));

                            update.Info.Add("Task", "Task #" + rnd.Next(7));

                            //__this ensures we get only one Environment/Task pair in a given update
                            TaskNotRecorded = false;
                            continue;
                        }

                        update.Info.Add("Key." + i + "." + k + "." + m, "Value." + i + "." + k + "." + m);
                    }

                    updates.Add(update);
                }
            }

            return updates;
        }

        private static List<string> projectNames = new List<string>()
        {
            "Website",
            "Enabler",
            "Manager",
            "Portal",
            "Monitor",
            "Thingamajig"
        };

        private static List<string> projectTypes = new List<string>()
        {
            "Jimmy",
            "Martha",
            "Stewart",
            "Schenectady",
            "Bocephus"
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
