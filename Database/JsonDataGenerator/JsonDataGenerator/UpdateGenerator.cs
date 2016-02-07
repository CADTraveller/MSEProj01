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
            List<ProjectUpdate> updates = new List<ProjectUpdate>();
            Random rnd = new Random();
            for (int i = 0; i < numberOfProjects; i++)
            {
                string project = projectNames[rnd.Next(4)] + " " + projectTypes[rnd.Next(3)];
                int verticalID = rnd.Next(7);
                int numberOfUpdates = rnd.Next(11) + 1;

                for (int k = 0; k < numberOfUpdates; k++)
                {
                    ProjectUpdate update = new ProjectUpdate();
                    update.ProjectID = project;
                    //update.PhaseID = rnd.Next(6);
                    update.VerticalID = verticalID;

                    int dataPointCount = rnd.Next(4) + 2;
                    int iStatusSequence = i + k; //__need a unique value for each ProjectUpdate
                    bool TaskNotRecorded = true;

                    for (int m = 0; m < dataPointCount; m++)
                    {

                        int doTagSwitch = rnd.Next(6);
                        if (doTagSwitch < 3 && TaskNotRecorded)
                        {
                            StatusUpdate enviroStatus = new StatusUpdate()
                            {
                                ProjectID = update.ProjectID,
                                PhaseID = rnd.Next(6),//= update.PhaseID,
                                StatusSequence = iStatusSequence,
                                RecordDate = DateTime.Now,
                                UpdateKey = "Environment",
                                UpdateValue = "System #" + rnd.Next(12)
                            };
                            update.Updates.Add(enviroStatus);
                            StatusUpdate taskStatus = new StatusUpdate()
                            {
                                ProjectID = update.ProjectID,
                                PhaseID = rnd.Next(6), //update.PhaseID,
                                StatusSequence = iStatusSequence,
                                RecordDate = DateTime.Now,
                                UpdateKey = "Task",
                                UpdateValue = "Task #" + rnd.Next(7)
                            };
                            update.Updates.Add(taskStatus);

                            //__this ensures we get only one Environment/Task pair in a given update
                            TaskNotRecorded = false;
                            continue;
                        }
                        StatusUpdate genStatus = new StatusUpdate()
                        {
                            ProjectID = update.ProjectID,
                            PhaseID = rnd.Next(6),//= update.PhaseID,
                            StatusSequence = iStatusSequence,
                            RecordDate = DateTime.Now,
                            UpdateKey = "Key." + i + "." + k + "." + m,
                        UpdateValue = "Value." + i + "." + k + "." + m
                        };
                        update.Updates.Add(genStatus );
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
