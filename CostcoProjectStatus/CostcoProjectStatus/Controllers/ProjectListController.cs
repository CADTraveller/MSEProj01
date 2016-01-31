using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService;
using Newtonsoft.Json;


namespace CostcoProjectStatus.Controllers
{
    public class ProjectListController : Controller
    {

        private AccessService DataAccsess = new AccessService();
            // GET: ProjectList
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProjectList/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectList/Create
        [HttpPost]
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

        // GET: ProjectList/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectList/Edit/5
        [HttpPost]
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

        // GET: ProjectList/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectList/Delete/5
        [HttpPost]
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
        
        public JsonResult Display()
        {
            var ProjectNames = DataAccsess.GetAllProjectNames();
            //LTC: This is a clumsy way to get around the 500 error
            var ProjectId = new List<String>();
            var ProjectVertical = new List<int>();
            var ProjectLastUpdate = new List<String>();
            var ProjectLastPhase = new List<String>();
            for (int i = 0; i < ProjectNames.Count; i++)
            {
                ProjectId.Add(ProjectNames[i].ProjectID);
                // You only need this if the client is expected to filter the verticals
                ProjectVertical.Add((int)ProjectNames[i].VerticalID);
                ProjectLastUpdate.Add(ProjectNames[i].ProjectPhases.Last().LatestUpdate.ToString());
                ProjectLastPhase.Add(ProjectNames[i].ProjectPhases.Last().PhaseID.ToString());
            }
            
            // This is what it used to be, had to edit it because LTC was getting a 500 error
            return Json(new { ProjectId, ProjectVertical, ProjectLastPhase, ProjectLastUpdate }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStatusUpdates(String id)
        {
            // LTC: Wrote this quick function to test data retrieval from the UI    
            var ProjectUpdates = DataAccsess.GetAllUpdatesForProject(id);
            //LTC: This is a clumsy way to get around the 500 error
            // These descriptions are not correct. 
            var ProjectUpdateDescriptions = new List<String>();
            var ProjectUpdatePhases = new List<int>();
            var ProjectDates = new List<String>();
            var ProjectUpdateKey = new List<String>();
            for (int i = 0; i < ProjectUpdates.Count; i++)
            {
                ProjectUpdateDescriptions.Add(ProjectUpdates[i].UpdateValue);
                ProjectUpdatePhases.Add(ProjectUpdates[i].PhaseID);
                ProjectDates.Add(ProjectUpdates[i].RecordDate.ToString());
                ProjectUpdateKey.Add(ProjectUpdates[i].UpdateKey);
            }
            var vId = ProjectUpdates.Last().VerticalID;
            // This is what it used to be, had to edit it because LTC was getting a 500 error
            return Json(new { vId, ProjectUpdateKey, ProjectUpdateDescriptions, ProjectUpdatePhases, ProjectDates }, JsonRequestBehavior.AllowGet);
        }

    }
}
