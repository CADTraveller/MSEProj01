﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSVProject
{
    class Program
    {
        static void Main(string[] args)
        {
            DataService.AccessService dataService = new DataService.AccessService();
            var file = new StreamReader(File.OpenRead(@"c:\Test.csv"));
            while (!file.EndOfStream)
            {
                var EachLine = file.ReadLine();
                var valuesOfLine = EachLine.Split(',');
                string userEmail = valuesOfLine[0];
                int role = int.Parse(valuesOfLine[1]);
                string Action = valuesOfLine[2];
                string NewEmail = valuesOfLine[3];
                if (Action == "add")
                {
                    dataService.AddUser(userEmail, role);
                }
                else
                if (Action == "delete")
                {
                    dataService.DeleteUser(userEmail);
                }
                else
                if (Action == "updateRole")
                {
                    dataService.UpdateUserRole(userEmail, role);
                }
                else 
                if(Action == "updateEmail")
                {
                    dataService.UpdateUserEmail(userEmail , NewEmail);
                }
            }
        }
    }
}
