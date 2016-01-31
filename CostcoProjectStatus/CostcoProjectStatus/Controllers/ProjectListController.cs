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
            for (int i = 0; i < ProjectNames.Count; i++)
            {
                ProjectId.Add(ProjectNames[i].ProjectID);
            }
            // This is what it used to be, had to edit it because LTC was getting a 500 error
            //return Json(new { ProjectNames }, JsonRequestBehavior.AllowGet);
            return Json(new { ProjectId }, JsonRequestBehavior.AllowGet);
        }
    }
}
