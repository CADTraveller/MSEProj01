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
        
        public string Display()
        {
            var ProjectNames = DataAccsess.GetAllProjectNames();
            string result = JsonConvert.SerializeObject(ProjectNames);
            return result;
        }
        public string GetStatusUpdates(String id)
        {
            var ProjectUpdates = DataAccsess.GetAllUpdatesForProject(id);
            string result = JsonConvert.SerializeObject(ProjectUpdates);
            return result;
        }
        public string GetStatusData(String projectId, String phaseId, String statusSequence)
        {
            var statusData = DataAccsess.GetAllUpdatesFromEmail(projectId, Convert.ToInt32(phaseId), Convert.ToInt32(statusSequence));
            string result = JsonConvert.SerializeObject(statusData);
            return result;
        }
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return null;
        }


    }
}
