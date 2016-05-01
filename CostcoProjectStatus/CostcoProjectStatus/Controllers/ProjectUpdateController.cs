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
        public void Update(AppPacket jsonPacket)
        //        public void Update(String jsonList)
        {
            // need to read this dynamically through csv after Hasnath checks in her code
            DataService.AccessService dataService = new DataService.AccessService();
            if (dataService.IsAppAuthorized(jsonPacket.AppId))
            {
                List<StatusUpdate> listOfUpdates = new List<StatusUpdate>();
                foreach (AppObject eo in jsonPacket.StatusUpdateList)
                {
                    StatusUpdate temp = new StatusUpdate();
                    temp.PhaseID = Convert.ToInt32(eo.PhaseID);
                    temp.ProjectName = eo.ProjectName;
                    temp.ProjectID = eo.ProjectID;
                    temp.VerticalID = Convert.ToInt32(eo.VerticalID);
                    temp.UpdateKey = eo.UpdateKey;
                    temp.UpdateValue = eo.UpdateValue;

                    listOfUpdates.Add(temp);
                }
                DataAccess.RecordStatusUpdate(listOfUpdates);
            }
        }

        public HttpResponseMessage Post(string value)
        {
            Console.WriteLine(value);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

       // List<EmailObject> EmailObjectList = new List<EmailObject>();
    }
    [Serializable]
    public class AppPacket
    {
        public string AppId { get; set; }
        public List<AppObject> StatusUpdateList { get; set; }
    }
    [Serializable]
    public class AppObject
    {
        public Guid ProjectID { get; set; }
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
      //  List<EmailObject> emailObjectList = new List<EmailObject>();
    }
}

