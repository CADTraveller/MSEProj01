using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService;
using Newtonsoft.Json;
using CostcoProjectStatus.CustomAttributes;

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
            string result = "<script>window.location.replace(\"/dashboard/index.html\");</script>";
            return result;
        }
        //[AuthAttribute]
    //    [BasicAuthentication]
        public string GetStatusUpdates(String id)
        {

            try
            {
               
                if (this.Session["username"].ToString() != null && DataAccsess.IsUserAuthorized(this.Session["username"].ToString()))
                {
                    var ProjectUpdates = DataAccsess.GetAllUpdatesForProject(id);
                    //var passedStatusUpdateList = new List<StatusUpdatesModel.StatusUpdate>();
                    //foreach (StatusUpdatesModel.StatusUpdate passedStatusUpdate in ProjectUpdates)
                    //{
                    //    StatusUpdatesModel.StatusUpdate tempStatusUpdate = new StatusUpdatesModel.StatusUpdate();
                    //    //tempStatusUpdate.Phase = passedStatusUpdate.Phase;
                    //    tempStatusUpdate.PhaseID = passedStatusUpdate.PhaseID;
                    //    //tempStatusUpdate.Project = passedStatusUpdate.Project;
                    //    tempStatusUpdate.ProjectID = passedStatusUpdate.ProjectID;
                    //    tempStatusUpdate.ProjectName = passedStatusUpdate.ProjectName;
                    //    tempStatusUpdate.RecordDate = passedStatusUpdate.RecordDate;
                    //    tempStatusUpdate.ProjectUpdateID = passedStatusUpdate.ProjectUpdateID;
                    //    tempStatusUpdate.UpdateKey = passedStatusUpdate.UpdateKey;
                    //    tempStatusUpdate.UpdateValue = passedStatusUpdate.UpdateValue;
                    //    //tempStatusUpdate.Vertical = passedStatusUpdate.Vertical;
                    //    tempStatusUpdate.VerticalID = passedStatusUpdate.VerticalID;
                    //    passedStatusUpdateList.Add(tempStatusUpdate);

                    //}
                    //string result = JsonConvert.SerializeObject(passedStatusUpdateList);
                    string result = JsonConvert.SerializeObject(ProjectUpdates);
                    return result;
                }
            } catch (Exception)
            {
                string emptyException = JsonConvert.SerializeObject("");
                return emptyException;
            }
            string empty = JsonConvert.SerializeObject("");
            return empty;

        }
        
        public string GetStatusData(String projectId, String phaseId, String ProjectUpdateId)
        {

            try
            {
                if (this.Session["username"].ToString() != null && DataAccsess.IsUserAuthorized(this.Session["username"].ToString()))
                {

                    var statusData = DataAccsess.GetAllUpdatesFromEmail(projectId, Convert.ToInt32(phaseId), Guid.Parse(ProjectUpdateId));
                    var passedStatusUpdateList = new List<StatusUpdatesModel.StatusUpdate>();
                    foreach (StatusUpdatesModel.StatusUpdate passedStatusUpdate in statusData)
                    {
                        StatusUpdatesModel.StatusUpdate tempStatusUpdate = new StatusUpdatesModel.StatusUpdate();
                        //tempStatusUpdate.Phase = passedStatusUpdate.Phase;
                        tempStatusUpdate.PhaseID = passedStatusUpdate.PhaseID;
                        //tempStatusUpdate.Project = passedStatusUpdate.Project;
                        tempStatusUpdate.ProjectID = passedStatusUpdate.ProjectID;
                        tempStatusUpdate.ProjectName = passedStatusUpdate.ProjectName;
                        tempStatusUpdate.RecordDate = passedStatusUpdate.RecordDate;
                        tempStatusUpdate.ProjectUpdateID = passedStatusUpdate.ProjectUpdateID;
                        tempStatusUpdate.UpdateKey = passedStatusUpdate.UpdateKey;
                        tempStatusUpdate.UpdateValue = passedStatusUpdate.UpdateValue;
                        //tempStatusUpdate.Vertical = passedStatusUpdate.Vertical;
                        tempStatusUpdate.VerticalID = passedStatusUpdate.VerticalID;
                        passedStatusUpdateList.Add(tempStatusUpdate);

                    }
                    string result = JsonConvert.SerializeObject(passedStatusUpdateList);
                    return result;
                }
            }
            catch (Exception)
            {
                string emptyException = JsonConvert.SerializeObject("");
                return emptyException;
            }
            string empty = JsonConvert.SerializeObject("");
            return empty;

           
        }
        //public string GetprojectUpdates( Guid projectID )
        //{
        //    var ProjectUpdateKeys = DataAccsess.GetUpdatesForKey(projectID);
        //    string result = JsonConvert.SerializeObject(ProjectUpdateKeys);
        //    return result;

        //}
        //
        // POST: /Account/ExternalLogin
        //[HttpPost]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    Request a redirect to the external login provider
        //    return null;
        //}


    }
}
