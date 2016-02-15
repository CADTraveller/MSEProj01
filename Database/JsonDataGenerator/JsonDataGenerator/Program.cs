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
        static void Main(string[] args)
        {

            //AccessService dataAccess = new AccessService();
            //List<Project> projects = dataAccess.GetAllProjectNames();
            //List<StatusUpdate> updates = dataAccess.GetAllUpdatesForProject(projects[0].ProjectID);

            //string projectWritePath = Directory.GetCurrentDirectory() + "\\Project.json";
            //string updatesJson = JsonConvert.SerializeObject(updates, Formatting.Indented);
            //File.WriteAllText(projectWritePath, updatesJson);

            //int numberProjects = 6;
            //Console.WriteLine(projectWritePath);
            //Console.ReadLine();

            List<ProjectUpdate> projects = new List<ProjectUpdate>();
            projects = UpdateGenerator.GenerateUpdates(4);


            foreach (var project in projects)
            {
                List<StatusUpdate> updates = project.Updates;
                AccessService dataPortal = new AccessService();

                try
                {
                    dataPortal.RecordStatusUpdate(updates);
                }
                catch (Exception e)
                {

                    throw e;
                }

            }            
        }
    }
}
