using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService;
using StatusUpdatesModel;
using Newtonsoft;
using Newtonsoft.Json;

namespace CostcoProjectStatus.Controllers
{
    public class TreeViewDataController : Controller
    {
        // GET: TreeViewData
        public ActionResult Index()
        {
            return View();
        }

        // GET: TreeViewData/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TreeViewData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TreeViewData/Create
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

        // GET: TreeViewData/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TreeViewData/Edit/5
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

        // GET: TreeViewData/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TreeViewData/Delete/5
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

        public JsonResult ListVerticals()
        {
            AccessService data = new AccessService();
            List<string> verticals = Enum.GetNames(typeof(Verticals)).ToList();
            string json = JsonConvert.SerializeObject(verticals);
            JsonResult result = new JsonResult();
            result.Data = json;
            return result;
        }
    }
}
