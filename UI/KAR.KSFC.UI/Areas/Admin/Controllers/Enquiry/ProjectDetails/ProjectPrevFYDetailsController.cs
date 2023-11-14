using System.Collections.Generic;
using System.Linq;

using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.UI.Utility;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.ProjectDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class ProjectPrevFYDetailsController : Controller
    {
        private const string resultViewPath = "~/Areas/Admin/Views/Enquiry/ProjectDetails/ProjectPrevFYDetails/";
        private const string viewPath = "../../Areas/Admin/Views/Enquiry/ProjectDetails/ProjectPrevFYDetails/";
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;

        public ProjectPrevFYDetailsController(SessionManager sessionManager, ILogger logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }

        /// <summary>
        /// Get method to view a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecord with id " + id);

                ViewBag.FinancialYearsList = _sessionManager.GetDDListFinancialYear();
                ViewBag.ListFinancialComponent = _sessionManager.GetDDListFinancialComponent();

                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new ProjectFinancialYearDetailsDTO());
                }
                else
                {
                    var projFYDetailList = _sessionManager.GetProjectPrevFYDetailsList();
                    ProjectFinancialYearDetailsDTO pro = projFYDetailList.Where(x => x.EnqPjfinId == id).FirstOrDefault();

                    _logger.Information("Completed - ViewRecord with id " + id);

                    return View(resultViewPath + "ViewRecord.cshtml", pro);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ViewRecord page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        

        /// <summary>
        /// Get method to Create or Edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEdit(int id = 0)
        {
            try
            {

                _logger.Information("Started - CreateOrEdit HttptGet with id " + id);

                ViewBag.FinancialYearsList = _sessionManager.GetDDListFinancialYear();
                ViewBag.ListFinancialComponent = _sessionManager.GetDDListFinancialComponent();

                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEdit HttptGet with id " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new ProjectFinancialYearDetailsDTO());
                }
                else
                {
                    var projFYDetailList = _sessionManager.GetProjectPrevFYDetailsList();
                    ProjectFinancialYearDetailsDTO pro = projFYDetailList.Where(x => x.EnqPjfinId == id).FirstOrDefault();

                    _logger.Information("Completed - CreateOrEdit HttptGet with id " + id);

                    return View(resultViewPath + "CreateOrEdit.cshtml", pro);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! CreateOrEdit HttpGet page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to create or edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pro"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, ProjectFinancialYearDetailsDTO pro)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEdit HttpPost with id {0} EnqPjfinamt :{1} EnqPjfinId :{2} Operation :{3} UniqueId :{4}"
                       , id, pro.EnqPjfinamt, pro.EnqPjfinId, pro.Operation, pro.UniqueId));

                List<ProjectFinancialYearDetailsDTO> projFYDetailList = new();
                var ddlFY = _sessionManager.GetDDListFinancialYear();
                var ddlFC = _sessionManager.GetDDListFinancialComponent();
                pro.FinyearCodeNavigation = new() { FinyearDesc = ddlFY.Where(X => X.Value == pro.FinyearCode.ToString()).FirstOrDefault().Text, FinyearCode = (int)pro.FinyearCode };
                pro.FincompCdNavigation = new() { FincompDets = ddlFC.Where(X => X.Value == pro.FincompCd.ToString()).FirstOrDefault().Text, FincompCd = (int)pro.FincompCd };
                if (ModelState.IsValid)
                {
                    if (id == 0)
                    {
                        if (_sessionManager.GetProjectPrevFYDetailsList() != null)
                            projFYDetailList = _sessionManager.GetProjectPrevFYDetailsList();

                        pro.EnqPjfinId = projFYDetailList.Max(x => x.EnqPjfinId) + 1 ?? 1; //Increment ID
                        projFYDetailList.Add(pro);

                    }
                    else
                    {
                        projFYDetailList = _sessionManager.GetProjectPrevFYDetailsList();
                        projFYDetailList.Remove(projFYDetailList.Find(m => m.EnqPjfinId == id));
                        pro.EnqPjfinId = id;
                        projFYDetailList.Add(pro);

                    }
                    _sessionManager.SetProjectPrevFYDetailsList(projFYDetailList);
                    _logger.Information(string.Format("Started - CreateOrEdit HttpPost with id {0} EnqPjfinamt :{1} EnqPjfinId :{2} Operation :{3} UniqueId :{4}"
                       , id, pro.EnqPjfinamt, pro.EnqPjfinId, pro.Operation, pro.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", projFYDetailList) });
                }
                _logger.Information(string.Format("Started - CreateOrEdit HttpPost with id {0} EnqPjfinamt :{1} EnqPjfinId :{2} Operation :{3} UniqueId :{4}"
                       , id, pro.EnqPjfinamt, pro.EnqPjfinId, pro.Operation, pro.UniqueId));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", pro) });
            }
            catch (Exception ex)
            {

                _logger.Error("Error occured! CreateOrEdit HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to Delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _logger.Information("Started - Delete with id " + id);

                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var projFYDetailList = _sessionManager.GetProjectPrevFYDetailsList();

                    var itemToRemove = projFYDetailList.Find(r => r.EnqPjfinId == id);
                    projFYDetailList.Remove(itemToRemove);
                    _sessionManager.SetProjectPrevFYDetailsList(projFYDetailList);
                    _logger.Information("Completed - Delete with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", projFYDetailList) });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Delete HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
