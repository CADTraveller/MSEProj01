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
            // Select a mailbox. Case-insensitive
            imapClient.SelectMailbox("INBOX");
            string emailJson="";
            
            Console.WriteLine(imapClient.GetMessageCount());

            imapClient.NewMessage += (sender, e) =>
            {
                var msg = imapClient.GetMessage(e.MessageCount - 1);
                List<string> emailSub = new List<string>();
                Dictionary<string, string> emailBody = new Dictionary<string, string>();
                List<string> emailBodyKeys = new List<string>();
                List<string> emailBodyValues = new List<string>();
                List<EmailJsonObject> emailObjectlist = new List<EmailJsonObject>();
                emailSub = ParseSubject(msg.Subject);
                emailBody = ParseBody(msg.Body);
                foreach (string keys in emailBody.Keys)
                {
                    emailBodyKeys.Add(keys);
                }

                foreach (string values in emailBody.Values)
                {
                    emailBodyValues.Add(values);
                } 
               
                for (int i = 0; i < emailBody.Count(); i++)
                {
                    EmailJsonObject emailJsonObject = new EmailJsonObject();
                    emailJsonObject.ProjectID = emailSub[0];
                    emailJsonObject.PhaseID = emailSub[1];
                    emailJsonObject.VerticalID = emailSub[2];
                    emailJsonObject.UpdateKey = emailBodyKeys[i];
                    emailJsonObject.UpdateValue = emailBodyValues[i];
                    emailJsonObject.RecordedDate = msg.Date;
                    emailObjectlist.Add(emailJsonObject);
                }

               emailJson = JsonConvert.SerializeObject(emailObjectlist);
               Console.WriteLine(emailJson);
              // byte[] data = Encoding.UTF8.GetBytes(emailJson.ToString());
               string result = "";


                // Rest API call works with this URL

               //using (var client = new HttpClient())
               //{
               //    client.BaseAddress = new Uri("http://localhost:61804");
               //    var content = new FormUrlEncodedContent(new[] 
               //          {
               //              new KeyValuePair<string, string>("", "emailJson")
               //          });
               //    var res = client.PostAsync("/api/contact", content).Result;
               //    string resultContent = res.Content.ReadAsStringAsync().Result;
               //    Console.WriteLine(resultContent);
               //}


               //using (var client = new HttpClient())
               //{
               //    client.BaseAddress = new Uri("https://localhost:44300");
               //    var content = new FormUrlEncodedContent(new[] 
               //          {
               //              new KeyValuePair<string, string>("", "emailJson")
               //          });
               //    var res = client.PostAsync("/ProjectUpdate/Update", content).Result;
               //    string resultContent = res.Content.ReadAsStringAsync().Result;
               //    Console.WriteLine(resultContent);
               //}


               //using (var client = new HttpClient())
               //{
               //    client.BaseAddress = new Uri("https://localhost:44300");
               //    client.DefaultRequestHeaders.Accept.Clear();
               //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

               //    //New code
               //    HttpRequestMessage response = await client.GetAsync("api/product/1");               

                   
               //}

               using (var client = new WebClient())
               {
                   client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                   var data = "=Karl";
                   result = client.UploadString("https://localhost:44300/ProjectUpdate/Update", "Post", data);
                   Console.WriteLine(result);
               }


               
            };
           return emailJson;           
        }

        

        public List<string> ParseSubject(string Subject)
        {
            List<string> li = new List<string>();
            string[] sub = Subject.Split('|');
            li.Add(sub[0]);
            li.Add(sub[1]);
            li.Add(sub[2]);
            return li;

        }

        public Dictionary<string, string> ParseBody(string Body)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] body = Body.Split('|');
            foreach (string s in body)
            {
                string[] temp = s.Split(':');
                dict.Add(temp[0], temp[1]);
                //Console.WriteLine(temp[0]);
                //Console.WriteLine(temp[1]);
            }

            return dict;
        }


    }
}
