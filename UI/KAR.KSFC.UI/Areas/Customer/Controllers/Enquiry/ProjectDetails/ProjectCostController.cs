using System;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.ProjectDetails
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class ProjectCostController : Controller
    {
        private const string resultViewPath = "~/Areas/Customer/Views/Enquiry/ProjectDetails/ProjectCost/";
        private const string viewPath = "../../Areas/Customer/Views/Enquiry/ProjectDetails/ProjectCost/";
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
                _logger.Information("Started - ViewRecord method for " + id);
                ViewBag.ListProjectCostComponent = _sessionManager.GetDDListProjectCostComponent();// JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("ListProjectCostComponent"));
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord method for " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new ProjectCostDetailsDTO());
                }
                else
                {
                    var secDetailList = _sessionManager.GetProjectCostList();// JsonConvert.DeserializeObject<List<ProjectCost>>(HttpContext.Session.GetString("ProjectCostList"));
                    ProjectCostDetailsDTO sec = secDetailList.Where(x => x.EnqPjcostId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecord method for " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", sec);
                }


            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecord page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
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
                _logger.Information("Started - CreateOrEdit method for " + id);
                ViewBag.ListProjectCostComponent = _sessionManager.GetDDListProjectCostComponent();// JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("ListProjectCostComponent"));

                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEdit method for " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new ProjectCostDetailsDTO());
                }
                else
                {
                    var secDetailList = _sessionManager.GetProjectCostList();// JsonConvert.DeserializeObject<List<ProjectCost>>(HttpContext.Session.GetString("ProjectCostList"));
                    ProjectCostDetailsDTO sec = secDetailList.Where(x => x.EnqPjcostId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEdit method for " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", sec);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
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
                _logger.Information(string.Format("Started - CreateOrEdit method for Id: {0} EnqPjcostId :{1} EnqtempId :{2} PjcostCd :{3} EnqPjcostAmt :{4} EnqPjcostRem :{5} UniqueId :{6} Operation",
                    id, proj.EnqPjcostId, proj.EnqtempId, proj.PjcostCd, proj.EnqPjcostAmt, proj.EnqPjcostRem, proj.UniqueId, proj.Operation));
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
                    _logger.Information(string.Format("Completed - CreateOrEdit method for Id: {0} EnqPjcostId :{1} EnqtempId :{2} PjcostCd :{3} EnqPjcostAmt :{4} EnqPjcostRem :{5} UniqueId :{6} Operation",
                    id, proj.EnqPjcostId, proj.EnqtempId, proj.PjcostCd, proj.EnqPjcostAmt, proj.EnqPjcostRem, proj.UniqueId, proj.Operation));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", projDetailList) });
                }
                _logger.Information(string.Format("Completed - CreateOrEdit method for Id: {0} EnqPjcostId :{1} EnqtempId :{2} PjcostCd :{3} EnqPjcostAmt :{4} EnqPjcostRem :{5} UniqueId :{6} Operation",
                    id, proj.EnqPjcostId, proj.EnqtempId, proj.PjcostCd, proj.EnqPjcostAmt, proj.EnqPjcostRem, proj.UniqueId, proj.Operation));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", proj) });


            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
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
                _logger.Information("Started - Delete HttpPost method for " + id);
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
                    _logger.Information("Completed - Delete HttpPost method for " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", projDetailList) });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading Delete HttpPost page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
