using AE.Net.Mail;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;



namespace EmailClient
{
    class ImapService
    {
        private string _imapServer;
        private string _userId;
        private string _password;
        public ImapService()
        {
            _imapServer = ConfigurationManager.AppSettings["ImapServer"];
            _userId = ConfigurationManager.AppSettings["UserId"];
            _password = ConfigurationManager.AppSettings["Password"];
        }

        public string ParseNewEmail()
        {
            // Connect to the IMAP server. The 'true' parameter specifies to use SSL, which is important (for Gmail at least)
            ImapClient imapClient = new ImapClient(ConfigurationManager.AppSettings["ImapServer"], ConfigurationManager.AppSettings["UserId"], ConfigurationManager.AppSettings["Password"], AuthMethods.Login, 993, true);
            var userName = ConfigurationManager.AppSettings["UserID"];
          //  ImapClient imapClient = new ImapClient(ConfigurationManager.AppSettings["ImapServer"], "jayasreetestemail@gmail.com", "7Ywy7N[S", AuthMethods.Login, 993, true);
            // Select a mailbox. Case-insensitive
            imapClient.SelectMailbox("INBOX");
            string emailJson="";
                  
           // Console.WriteLine(imapClient.GetMessageCount());

            imapClient.NewMessage += (sender, e) =>
            {
                var msg = imapClient.GetMessage(e.MessageCount - 1);
              appPacket ap = new appPacket();
              appObject ao = new appObject();
              List<string> emailbodylist = new List<string>();
              emailbodylist=ParseBody(msg.Body);
              ao.ProjectID = Guid.NewGuid();
              ao.ProjectName = emailbodylist[0];
              ao.PhaseID = " ";
              ao.VerticalID = " ";
              ao.RecordedDate = DateTime.Now;
              ao.UpdateKey = "Execution Summary";
              ao.UpdateValue = emailbodylist[1];
              ap.AppId = "emailCostco";           
              ap.StatusUpdateList.Add(ao);
              emailJson = JsonConvert.SerializeObject(ap);
              Console.WriteLine(emailJson);
              string result = "";
              using (var client = new WebClient())
              {
                  client.Headers[HttpRequestHeader.ContentType] = "application/json";
                  result = client.UploadString("https://localhost:44300/ProjectUpdate/Update", "Post", emailJson);
                  Console.WriteLine(result);
              }
              
               
            };           

           return emailJson;           
        }
      /*  public Dictionary<string,string> ParseSubject(string emailSubject)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] sub = emailSubject.Split(' ');
            string op = sub[2] + sub[3] + sub[4];
            dict.Add("Phase",sub[0]); // Phase
            dict.Add("Result", sub[1]); // Result
            dict.Add("Operation",op); // Operation 
            Console.WriteLine("");
            Console.WriteLine("Subject");
            foreach (string key in dict.Keys)
            {
                Console.WriteLine(key+":"+dict[key]);
            }
            return dict;
        }*/


        public List<string> ParseBody(string Body)
        {
            int counter = 1;
            string line;
            string summary = "";
            List<string> li = new List<string>();
                      
            System.IO.File.WriteAllText("Test.txt",Body);
            System.IO.StreamReader file =
            new System.IO.StreamReader("Test.txt");
            while ((line = file.ReadLine()) != null)
            {
              /*  if (line.Equals("Application:") || line.Equals("Process:") || line.Equals("Environment:") || line.Equals("Requested By:") || line.Equals("Requested On:")||line.Equals("Description:"))
                {
                // Console.WriteLine(getNextLine(counter)); 
                    dict.Add(line, getNextLine(counter));
                 
                }*/
                if (line.Equals("Application:"))
                {
                   li.Add(getNextLine(counter));
                 
                }
                else
                {
                    summary += line;
                }
                counter++;
            }
            li.Add(summary);       
            file.Close();
            return li;         
     
        }

        
        private string getNextLine(int lineNumber)
        {
            // using will make sure the file is closed
            string nextLine;
            using (System.IO.StreamReader file = new System.IO.StreamReader("Test.txt"))
            {
                // Skip lines
                for (int i = 0; i <= lineNumber; ++i)
                    file.ReadLine();

                // Store your line
               nextLine=file.ReadLine();
               // LastLineNumber++;
            }
            return nextLine;
        }


       

        


    }
}
