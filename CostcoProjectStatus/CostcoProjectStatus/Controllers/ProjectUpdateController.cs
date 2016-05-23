﻿using System;
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
        /// <summary>
        /// This controller is include post actions. It basically receives the information in format of Json from email adapter or excel adapter
        /// and write all the information into the database by the help of Data Access layer which is a wrapper around the SQL data base.
        /// 
        /// </summary>
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
        //public void Update(UpdateObject jsonPacket)
        //{
        //     Insert into database
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
        public HttpResponseMessage UpdatePhase(string projectUpdate)
        {

            try
            {
                ProjectUpdate update = JsonConvert.DeserializeObject<ProjectUpdate>(projectUpdate);
                DataAccess.ChangeProjectUpdatePhase(update);
            }
            catch (Exception e)
            {
                return new HttpResponseMessage (HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        /// <summary>
        /// This method receiving a json object, deserialze the object and pass the inforamtion to the the RecordUpdatePackage
        /// from Data Access layer. And then all the information will be recorded in SQL db live in Azure.
        /// </summary>
        /// <param name="jsonPacket"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Update(string jsonPacket)
        {
            try
            {
                UpdatePackage update = JsonConvert.DeserializeObject<UpdatePackage>(jsonPacket);
                DataAccess.RecordUpdatePackage(update);
            }
            catch (Exception)
            {

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }


        [Serializable]
        public class UpdateObject
        {
            public string ProjectName { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public List<KVPPairs> Updates { get; set; }


        }

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

