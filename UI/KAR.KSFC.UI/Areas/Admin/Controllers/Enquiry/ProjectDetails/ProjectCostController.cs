using System;
using System.Collections.Generic;
using System.Linq;

using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Utility;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.ProjectDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class ProjectCostController : Controller
    {
        private const string resultViewPath = "~/Areas/Admin/Views/Enquiry/ProjectDetails/ProjectCost/";
        private const string viewPath = "../../Areas/Admin/Views/Enquiry/ProjectDetails/ProjectCost/";
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;

        public ProjectCostController(SessionManager sessionManager, ILogger logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }

        /// <summary>
        /// Get method to View a Project Cost Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ProjectCost/ViewRecord with id " + id);

                ViewBag.ListProjectCostComponent = _sessionManager.GetDDListProjectCostComponent();// JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("ListProjectCostComponent"));
                if (id == 0)
                {
                    _logger.Information("Completed - ProjectCost/ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new ProjectCostDetailsDTO());
                }
                else
                {
                    var secDetailList = _sessionManager.GetProjectCostList();// JsonConvert.DeserializeObject<List<ProjectCost>>(HttpContext.Session.GetString("ProjectCostList"));
                    ProjectCostDetailsDTO sec = secDetailList.Where(x => x.EnqPjcostId == id).FirstOrDefault();

                    _logger.Information("Completed - ProjectCost/ViewRecord with id " + id);

                    return View(resultViewPath + "ViewRecord.cshtml", sec);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ProjectCost/ViewRecord page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Get method to Create or Edit a Project Cost 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEdit(int id = 0)
        {
            try
            {
                _logger.Information("Started - ProjectCost/CreateOrEdit HttpGet with id " + id);

                ViewBag.ListProjectCostComponent = _sessionManager.GetDDListProjectCostComponent();// JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("ListProjectCostComponent"));

                if (id == 0)
                {
                    _logger.Information("Completed - ProjectCost/CreateOrEdit HttpGet with id " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new ProjectCostDetailsDTO());
                }
                else
                {
                    var secDetailList = _sessionManager.GetProjectCostList();// JsonConvert.DeserializeObject<List<ProjectCost>>(HttpContext.Session.GetString("ProjectCostList"));
                    ProjectCostDetailsDTO sec = secDetailList.Where(x => x.EnqPjcostId == id).FirstOrDefault();

                    _logger.Information("Completed - ProjectCost/CreateOrEdit HttpGet with id " + id);

                    return View(resultViewPath + "CreateOrEdit.cshtml", sec);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ProjectCost/CreateOrEdit page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to create or edit a Project Cost
        /// </summary>
        /// <param name="id"></param>
        /// <param name="proj"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, ProjectCostDetailsDTO proj)
        {

            try
            {
                _logger.Information(string.Format("Started - ProjectCost/CreateOrEdit HttpPost with id {0} EnqPjcostAmt :{1} EnqPjcostId :{2} Operation :{3} UniqueId :{4}"
                  , id, proj.EnqPjcostAmt, proj.EnqPjcostId, proj.Operation, proj.UniqueId));

                List<ProjectCostDetailsDTO> projDetailList = new();
                var dropdownList = _sessionManager.GetDDListProjectCostComponent();
                proj.PjcostCdNavigation = new() { PjcostDets = dropdownList.Where(X => X.Value == proj.PjcostCd.ToString()).FirstOrDefault().Text, PjcostCd = proj.PjcostCd };
                if (ModelState.IsValid)
                {
                    if (id == 0)
                    {
                        if (_sessionManager.GetProjectCostList() != null)
                            projDetailList = _sessionManager.GetProjectCostList();

                        proj.EnqPjcostId = projDetailList.Count() == 0 ? 1 : projDetailList.Max(x => x.EnqPjcostId) + 1;

                        projDetailList.Add(proj);

                    }
                    else
                    {
                        projDetailList = _sessionManager.GetProjectCostList();
                        projDetailList.Remove(projDetailList.Find(m => m.EnqPjcostId == id));
                        proj.EnqPjcostId = id;
                        projDetailList.Add(proj);

                    }

                    ViewBag.TotalCost = projDetailList.Sum(x => x.EnqPjcostAmt);

                    _sessionManager.SetProjectCostList(projDetailList);

                    _logger.Information(string.Format("Completed - ProjectCost/CreateOrEdit HttpPost with id {0} EnqPjcostAmt :{1} EnqPjcostId :{2} Operation :{3} UniqueId :{4}"
                 , id, proj.EnqPjcostAmt, proj.EnqPjcostId, proj.Operation, proj.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", projDetailList) });
                }

                _logger.Information(string.Format("Completed - ProjectCost/CreateOrEdit HttpPost with id {0} EnqPjcostAmt :{1} EnqPjcostId :{2} Operation :{3} UniqueId :{4}"
                 , id, proj.EnqPjcostAmt, proj.EnqPjcostId, proj.Operation, proj.UniqueId));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", proj) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ProjectCost/CreateOrEdit HttpPost . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to delete a Project Cost
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.Information("Started - ProjectCost/Delete HttpPost with id " + id);

                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var projDetailList = _sessionManager.GetProjectCostList();//JsonConvert.DeserializeObject<List<ProjectCost>>(HttpContext.Session.GetString("ProjectCostList"));

                    var itemToRemove = projDetailList.Find(r => r.EnqPjcostId == id);
                    projDetailList.Remove(itemToRemove);
                    _sessionManager.SetProjectCostList(projDetailList);

                    ViewBag.TotalCost = projDetailList.Sum(x => x.EnqPjcostAmt); ;

                    _logger.Information("Completed - ProjectCost/Delete HttpPost with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", projDetailList) });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ProjectCost/Delete HttpPost . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
