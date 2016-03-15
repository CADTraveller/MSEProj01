using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DataService;
using StatusUpdatesModel;

namespace JsonDataGenerator
{
    class Program
    {
        private static int numberOfProjectsToGenerate = 12;
        private static string errorMessage = "";

        static void Main(string[] args)
        {

            //AccessService dataAccess = new AccessService();
            int actionOption = 1;

            while (actionOption > 0)
            {
                Console.WriteLine("Select action to take:");
                Console.WriteLine("--> 1) Clear all data");
                Console.WriteLine("--> 2) Create sample data");
                Console.WriteLine("??\n");

                try
                {
                    string input = Console.ReadLine();
                    actionOption = Convert.ToInt16(input);
                }
                catch (Exception)
                {

                    Console.WriteLine("Integers only please");
                    Console.WriteLine("-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-\n");
                    continue;
                }
                bool success = false;
                switch (actionOption)
                {
                    case 1:
                        success = deleteAllData();
                        if (!success)
                        {
                            Console.WriteLine("Problem deleting data: " + errorMessage);
                            errorMessage = "";
                        }
                        break;
                    case 2:
                        success = writeSampleData();
                        if (!success)
                        {
                            Console.WriteLine("Problem writing sample data: " + errorMessage);
                            errorMessage = "";
                        }
                        break;
                    default:
                        return;
                }
                success = false;
            }
        }

        private static bool deleteAllData()
        {
            try
            {

                const string ConnectionString = "Server=tcp:costcosu.database.windows.net,1433;Database=CostcoDevStatus;User ID=SUAdmin@costcosu;Password=39ffbJeo;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                CostcoDevStatusEntities context = CostcoDevStatusEntities.Create(ConnectionString);
                IEnumerable<StatusUpdate> updates = context.StatusUpdates;
                foreach (StatusUpdate update in updates)
                {
                    Console.WriteLine("Removing Status Update " + update.ProjectName + "," + update.UpdateKey );
                    context.StatusUpdates.Remove(update);
                }
                context.SaveChanges();

                IEnumerable<ProjectPhase> statusSequences = context.ProjectPhases;
                foreach (ProjectPhase statusSequence in statusSequences)
                {
                    Console.WriteLine("Removing ProjectPhase record " + statusSequence.Project.ProjectName + "_" 
                        + statusSequence.UpdateKey);
                    context.ProjectPhases.Remove(statusSequence);
                }
                context.SaveChanges();

                IEnumerable<Project> projects = context.Projects;
                foreach (Project project in projects)
                {
                    Console.WriteLine("Removing Project " + project.ProjectName);
                    context.Projects.Remove(project);
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
            return true;
        }

        private static bool writeSampleData()
        {
            try
            {
                int totalRecords = 0;
                DateTime start = DateTime.Now;
                AccessService dataAccess = new AccessService();
                List<ProjectUpdate> projects = UpdateGenerator.GenerateUpdates(numberOfProjectsToGenerate);
                int numberOfProjects = projects.Count;
                foreach (ProjectUpdate project in projects)
                {
                    List<StatusUpdate> updates = project.Updates;
                    totalRecords += updates.Count;
                    dataAccess.RecordStatusUpdate(updates);
                    Console.WriteLine("Recorded " + updates.Count + " updates for Project " + project.ProjectName);
                }
                int durationInMinutes = (DateTime.Now - start).Minutes;
                Console.WriteLine("Recorded " + totalRecords + " for " + numberOfProjects + " projects in " + durationInMinutes + "m");
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
            return true;

        }
    }
}
