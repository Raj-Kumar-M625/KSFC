using System;
using System.Collections.Generic;
using System.Linq;

using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.UI.Utility;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Services.IServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.ProjectDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class ProjectMeansOfFinanceController : Controller
    {
        private const string resultViewPath = "~/Areas/Admin/Views/Enquiry/ProjectDetails/ProjectMeansOfFinance/";
        private const string viewPath = "../../Areas/Admin/Views/Enquiry/ProjectDetails/ProjectMeansOfFinance/";
        private readonly SessionManager _sessionManager;
        private readonly IEnquirySubmissionService _enquirySubmissionService;
        private readonly ILogger _logger;

        public ProjectMeansOfFinanceController(SessionManager sessionManager, IEnquirySubmissionService enquirySubmissionService, ILogger logger)
        {
            _sessionManager = sessionManager;
            _enquirySubmissionService = enquirySubmissionService;
            _logger = logger;
        }

        /// <summary>
        /// Get method to View a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ViewRecord(int id = 0)
        {

            try
            {
                _logger.Information("Started - ViewRecord with id " + id);

                ViewBag.ListProjectMeansOfFinanceCat = _sessionManager.GetDDListProjectMeansOfFinance();

                if (id == 0)
                {
                    ViewBag.ListProjectFinanceType = _sessionManager.GetDDListProjectFinanceType();
                    _logger.Information("Completed - ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new ProjectMeansOfFinanceDTO());
                }
                else
                {
                    var projMOFList = _sessionManager.GetProjectMeansOfFinanceList();
                    ProjectMeansOfFinanceDTO pro = projMOFList.Where(x => x.EnqPjmfId == id).FirstOrDefault();
                    var ddl = await _enquirySubmissionService.PopulateFinanceTypeList((int)pro.MfcatCd);
                    ViewBag.ListProjectFinanceType = ddl;

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
        /// Get method to Create Or Edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {

            try
            {
                _logger.Information("Started - CreateOrEdit HttptGet with id " + id);

                ViewBag.ListProjectMeansOfFinanceCat = _sessionManager.GetDDListProjectMeansOfFinance();

                if (id == 0)
                {
                    ViewBag.ListProjectFinanceType = new List<SelectListItem>();

                    _logger.Information("Completed - CreateOrEdit HttptGet with id " + id);

                    return View(resultViewPath + "CreateOrEdit.cshtml", new ProjectMeansOfFinanceDTO());
                }
                else
                {
                    var projMOFList = _sessionManager.GetProjectMeansOfFinanceList();
                    ProjectMeansOfFinanceDTO pro = projMOFList.Where(x => x.EnqPjmfId == id).FirstOrDefault();
                    var ddl = await _enquirySubmissionService.PopulateFinanceTypeList((int)pro.MfcatCd);
                    ViewBag.ListProjectFinanceType = ddl;

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
        /// Post method to Create or Edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pro"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(int id, ProjectMeansOfFinanceDTO pro)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEdit HttpPost with id {0} EnqPjmfId :{1} EnqPjmfValue :{2} MfcatCd :{3} UniqueId :{4}"
                 , id, pro.EnqPjmfId, pro.EnqPjmfValue, pro.MfcatCd, pro.UniqueId));

                List<ProjectMeansOfFinanceDTO> projMOFList = new();
                var ddl = new List<SelectListItem>();
                var dropdownList = _sessionManager.GetDDListProjectMeansOfFinance();
                if (id > 0)
                {
                    ddl = await _enquirySubmissionService.PopulateFinanceTypeList((int)pro.MfcatCd);
                }
                else
                {
                    ddl = _sessionManager.GetDDListProjectFinanceType();
                }
                pro.MfcatCdNavigation = new() { PjmfDets = dropdownList.Where(X => X.Value == pro.MfcatCd.ToString()).FirstOrDefault().Text, MfcatCd = (int)pro.MfcatCd };
                pro.PjmfCdNavigation = new() { PjmfDets = ddl.Where(X => X.Value == pro.PjmfCd.ToString()).FirstOrDefault().Text, PjmfCd = pro.PjmfCd };
                if (ModelState.IsValid)
                {
                    if (id == 0)
                    {
                        if (_sessionManager.GetProjectMeansOfFinanceList() != null)
                            projMOFList = _sessionManager.GetProjectMeansOfFinanceList();

                        pro.EnqPjmfId = projMOFList.Any() ? projMOFList.Max(x => x.EnqPjmfId) + 1 : 1; //Increment ID
                        projMOFList.Add(pro);

                    }
                    else
                    {
                        projMOFList = _sessionManager.GetProjectMeansOfFinanceList();
                        projMOFList.Remove(projMOFList.Find(m => m.EnqPjmfId == id));
                        pro.EnqPjmfId = id;
                        projMOFList.Add(pro);

                    }
                    _sessionManager.SetProjectMeansOfFinanceList(projMOFList);
                    decimal totalEquity = 0;
                    decimal totalDedt = 0;
                    foreach (var item in projMOFList)
                    {
                        if (item.MfcatCdNavigation.PjmfDets == "Equity")
                            totalEquity += Convert.ToDecimal(item.EnqPjmfValue);
                        if (item.MfcatCdNavigation.PjmfDets == "Debt")
                            totalDedt += Convert.ToDecimal(item.EnqPjmfValue);
                    }
                    ViewBag.TotalEquity = totalEquity;
                    ViewBag.TotalDebt = totalDedt;

                    _logger.Information(string.Format("Completed - CreateOrEdit HttpPost with id {0} EnqPjmfId :{1} EnqPjmfValue :{2} MfcatCd :{3} UniqueId :{4}"
                , id, pro.EnqPjmfId, pro.EnqPjmfValue, pro.MfcatCd, pro.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", projMOFList) });
                }

                _logger.Information(string.Format("Completed - CreateOrEdit HttpPost with id {0} EnqPjmfId :{1} EnqPjmfValue :{2} MfcatCd :{3} UniqueId :{4}"
                , id, pro.EnqPjmfId, pro.EnqPjmfValue, pro.MfcatCd, pro.UniqueId));

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
        /// Post method to delete a record
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
                    var projMOFList = _sessionManager.GetProjectMeansOfFinanceList();

                    var itemToRemove = projMOFList.Find(r => r.EnqPjmfId == id);
                    projMOFList.Remove(itemToRemove);

                    _sessionManager.SetProjectMeansOfFinanceList(projMOFList);
                    decimal totalEquity = 0;
                    decimal totalDedt = 0;
                    foreach (var item in projMOFList)
                    {
                        if (item.MfcatCdNavigation.PjmfDets == "Equity")
                            totalEquity += Convert.ToDecimal(item.EnqPjmfValue);
                        if (item.MfcatCdNavigation.PjmfDets == "Debt")
                            totalDedt += Convert.ToDecimal(item.EnqPjmfValue);
                    }
                    ViewBag.TotalEquity = totalEquity;
                    ViewBag.TotalDebt = totalDedt;

                    _logger.Information("Completed - Delete with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", projMOFList) });
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