using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using  Newtonsoft.Json;
using DataService;

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

        // GET: ProjectUpdate/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectUpdate/Edit/5
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

        // GET: ProjectUpdate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectUpdate/Delete/5
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
        //public void Update(string json)
        //{
        //    //ProjectUpdate projectupdates =new ProjectUpdate();
        //    var projectupdates = JsonConvert.DeserializeObject<ProjectUpdate>(json);
        //    DataAccess.RecordStatusUpdate(projectupdates);

        //}
    }
}
