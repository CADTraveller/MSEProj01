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
        [System.Web.Mvc.HttpPost]
        public void Update(List<EmailObject> jsonList)
        //        public void Update(String jsonList)
        {
            List<StatusUpdate> listOfUpdates = new List<StatusUpdate>();
            foreach (EmailObject eo in jsonList)
            {
                StatusUpdate temp = new StatusUpdate();
                temp.PhaseID = Convert.ToInt32(eo.PhaseID);
                temp.ProjectName = eo.ProjectName;
                temp.ProjectID = Guid.Parse(eo.ProjectID);
                temp.VerticalID = Convert.ToInt32(eo.VerticalID);
                temp.UpdateKey = eo.UpdateKey;
                temp.UpdateValue = eo.UpdateValue;

                listOfUpdates.Add(temp);
            }

            //            List<StatusUpdate> listOfUpdates = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StatusUpdate>>(jsonList);

            DataAccess.RecordStatusUpdate(listOfUpdates);
        }

        public HttpResponseMessage Post(string value)
        {
            Console.WriteLine(value);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        List<EmailObject> EmailObjectList = new List<EmailObject>();
    }

    [Serializable]
    public class EmailObject
    {
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string PhaseID { get; set; }
        public string VerticalID { get; set; }
        public string UpdateKey { get; set; }
        public string UpdateValue { get; set; }
        public DateTime RecordedDate { get; set; }
    }

    [Serializable]
    public class EmailObjectList
    {
        List<EmailObject> emailObjectList = new List<EmailObject>();
    }
}

