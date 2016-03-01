using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService;
using Newtonsoft.Json;

namespace CostcoProjectStatus.Controllers
{
    public class VerticalController : Controller
    {
        // GET: Vertical

        public string GetAllVertical()
        {
            AccessService DataAccess = new AccessService();
            //var verticals = DataAccess.GetAllVerticals();
            var verticals = true;
            string result = JsonConvert.SerializeObject(verticals);
            return result;
            
        }
        public string GetVerticalProjects(int VerticalId)
        {
            AccessService DataAccess = new AccessService();
            var VerticalProjets = DataAccess.GetAllProjectsForVertical(VerticalId);
            string result = JsonConvert.SerializeObject(VerticalProjets);
            return result;

        }







        public ActionResult Index()
        {
            return View();
        }

        // GET: Vertical/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Vertical/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vertical/Create
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

        // GET: Vertical/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Vertical/Edit/5
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

        // GET: Vertical/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Vertical/Delete/5
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

    }
}
