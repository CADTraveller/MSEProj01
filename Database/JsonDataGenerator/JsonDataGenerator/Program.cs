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
            
            int numberProjects = 6;

            List<ProjectUpdate> updates = UpdateGenerator.GenerateUpdates(numberProjects);

            Console.WriteLine("Generated " + updates.Count + " updates");



            string json = JsonConvert.SerializeObject(updates, Formatting.Indented);
            string path = Directory.GetCurrentDirectory() + "\\JsonSample.json";

            File.WriteAllText(path, json);

            Console.WriteLine("Created file: " + path);
            Console.WriteLine();

            try
            {
                AccessService dataPortal = new AccessService();
                foreach (ProjectUpdate update in updates)
                {
                    dataPortal.RecordStatusUpdate(update);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            Console.WriteLine("Number of singles to generate?");
            Console.WriteLine();

            string reply = Console.ReadLine();

            try
            {
                int iNumSingles = Convert.ToInt16(reply);
                int entries = numberProjects;
                List<int> usedIndices = new List<int>();
                Random rnd = new Random();

                for (int i = 0; i < iNumSingles; i++)
                {
                    int index = rnd.Next(entries);
                    while (usedIndices.Contains(index))//__prevent duplicates
                    {
                        index = rnd.Next(entries);
                    }

                    ProjectUpdate single = updates[index];
                    string singleJson = JsonConvert.SerializeObject(single, Formatting.Indented);
                    string singlePath = Directory.GetCurrentDirectory() + "\\Single" + (i + 1) + ".json";
                    File.WriteAllText(singlePath, singleJson);
                }

                Console.WriteLine("Created " + iNumSingles + " single entry Json files");
            }
            catch (Exception e)
            {

                Console.WriteLine("***");
                Console.WriteLine(e.Message);
                Console.WriteLine("***");
            }
            Console.ReadLine();
        }
    }
}
