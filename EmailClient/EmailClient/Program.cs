using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmailClient
{
    class Program
    {
        static void Main(string[] args)
        {

            // new ImapService().ParseNewEmail();
            // var emailObject= new ImapService().ParseNewEmail();
            //byte[] data = Encoding.UTF8.GetBytes(emailObject.ToString());
            //string result = "";

            //using (var client = new WebClient())
            //{
            //    client.Headers[HttpRequestHeader.ContentType] = "application/json";
            //    byte[] response = client.UploadData("http://localhost:64747/Home/ProjectList", "ProjectList", data);
            //    result = Encoding.UTF8.GetString(response);
            //}
            //Console.WriteLine(result);
            string uriString = "https://localhost:44300/ProjectUpdate/Update";


            // Create a new WebClient instance.
            //WebClient myWebClient = new WebClient();
            ////Console.WriteLine("\nPlease enter the data to be posted to the URI {0}:", uriString);
            //string postData = "{some:\"json data\"}";
            //myWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            //// Display the headers in the request
            //Console.Write("Resulting Request Headers: ");
            //Console.WriteLine(myWebClient.Headers.ToString());
            //myWebClient.Encoding = Encoding.ASCII;
            //// Apply ASCII Encoding to obtain the string as a byte array.

            //byte[] byteArray = Encoding.ASCII.GetBytes(postData);
            //Console.WriteLine("Uploading to {0} ...", uriString);
            //// Upload the input string using the HTTP 1.0 POST method.
            //byte[] responseArray = myWebClient.UploadData(uriString, "POST", byteArray);

            //// Decode and display the response.
            //Console.WriteLine("\nResponse received was {0}",
            //      Encoding.ASCII.GetString(responseArray));


            using (var client = new WebClient())
            {
                //client.Headers.Add("Content-Type", "application/json; charset=ASCII");// "application/x-www-form-urlencoded");
               client.Encoding = Encoding.UTF8;
                //client.UseDefaultCredentials = true;
                var data = "{some:\"json data\"}";

                //client.Headers["Content-Length"] = data.Length.ToString();

                
                string result = client.UploadString("https://localhost:44300/ProjectUpdate/Update/",  data);
                Console.WriteLine(result);
            }

            Console.ReadLine();
        }
    }
}
