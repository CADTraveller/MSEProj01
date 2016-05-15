using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;
using DataService;
using StatusUpdatesModel;
using System.Net.Http;
using System.Net;
using System.Collections.Specialized;

using HttpPost = System.Web.Mvc.HttpPostAttribute;



namespace CostcoProjectStatus.Controllers
{
    public class ProjectUpdateController : Controller
    {
        private AccessService DataAccess = new AccessService();
        // GET: ProjectUpdate
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProjectUpdate/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectUpdate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectUpdate/Create
        //[HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectUpdate/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectUpdate/Edit/5
        // [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectUpdate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectUpdate/Delete/5
        // [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // [System.Web.Mvc.HttpPostAttribute]
        //[System.Web.Mvc.HttpPost]
        //public void Update(UpdatePackage jsonPacket)        
        //{
        //   // Insert into database
        //    List<KeyValuePair<string, string>> temp = new List<KeyValuePair<string, string>>();
        //    DataService.AccessService dataService = new DataService.AccessService();
        //    DataService.UpdatePackage updatePackage = new DataService.UpdatePackage();
        //    updatePackage.ProjectName = jsonPacket.ProjectName;
        //    updatePackage.Subject = jsonPacket.Subject;
        //    updatePackage.Body = jsonPacket.Body;
        //    foreach (KVPPairs kvp in jsonPacket.Updates)
        //    {
        //        temp.Add(new KeyValuePair<string, string>(kvp.Key, kvp.Value));
        //    }
        //    updatePackage.Updates = temp;
        //    dataService.RecordUpdatePackage(updatePackage);

        //}

        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Update(UpdatePackage jsonPacket)
        {
            try
            {
                DataAccess.RecordUpdatePackage(jsonPacket);
            }
            catch (Exception)
            {

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }


        //[Serializable]
        //public class UpdateObject
        //{
        //    public string ProjectName{get;set;}
        //    public string Subject{get;set;}
        //    public string Body{get;set;}
        //    public List<KVPPairs> Updates { get; set; }


        //}

        public class KVPPairs
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public HttpResponseMessage Post(string value)
        {
            Console.WriteLine(value);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // List<EmailObject> EmailObjectList = new List<EmailObject>();
    }

}

