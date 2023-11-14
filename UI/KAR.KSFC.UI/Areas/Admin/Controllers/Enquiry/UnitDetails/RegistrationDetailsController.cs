using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.UnitDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class RegistrationDetailsController : Controller
    {
        private const string resultViewPath = "~/Areas/Admin/Views/Enquiry/UnitDetails/RegistrationDetails/";
        private const string viewPath = "../../Areas/Admin/Views/Enquiry/UnitDetails/RegistrationDetails/";
        private readonly IEnquirySubmissionService _enquirySubmissionService;
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;
        public RegistrationDetailsController(IEnquirySubmissionService enquirySubmissionService, SessionManager sessionManager, ILogger logger)
        {
            _enquirySubmissionService = enquirySubmissionService;
            _sessionManager = sessionManager;
            _logger = logger;
        }

        /// <summary>
        /// Get method to view a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecord with id " + id);

                ViewBag.ListRegnType = await getRegitrationType();

                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new RegistrationNoDetailsDTO());
                }
                else
                {
                    var regDetailList = _sessionManager.GetRegistrationDetList();
                    RegistrationNoDetailsDTO reg = regDetailList.Where(x => x.EnqRegnoId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecord with id " + id);

                    return View(resultViewPath + "ViewRecord.cshtml", reg);
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
        /// get method to create or edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEdit HttptGet with id " + id);

                ViewBag.ListRegnType = await getRegitrationType();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEdit HttptGet with id " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new RegistrationNoDetailsDTO());
                }
                else
                {
                    var regDetailList = _sessionManager.GetRegistrationDetList();
                    RegistrationNoDetailsDTO reg = regDetailList.Where(x => x.EnqRegnoId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEdit HttptGet with id " + id);

                    return View(resultViewPath + "CreateOrEdit.cshtml", reg);
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
        /// post method to create or edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regD"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(int id, RegistrationNoDetailsDTO regD)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEdit HttpPost with id {0} EnqRegno :{1} EnqRegnoId :{2} RegrefCd :{3} RegTypeText :{4} UniqueId :{5}"
                                                 , id, regD.EnqRegno, regD.EnqRegnoId, regD.RegrefCd, regD.RegTypeText, regD.UniqueId));

                List<RegistrationNoDetailsDTO> regDetailList = new();
                var regType = await getRegitrationType();
                if (ModelState.IsValid)
                {
                    if (id == 0)
                    {
                        if (_sessionManager.GetRegistrationDetList() != null)
                            regDetailList = _sessionManager.GetRegistrationDetList();

                        regD.EnqRegnoId = regDetailList.Max(x => x.EnqRegnoId) + 1 ?? 1;
                        var RegTypeId = regD.RegrefCd.ToString();
                        regD.RegTypeText = regType.FindAll(x => x.Value == RegTypeId).FirstOrDefault().Text;
                        regDetailList.Add(regD);

                    }
                    else
                    {
                        regDetailList = _sessionManager.GetRegistrationDetList();
                        regDetailList.Remove(regDetailList.Find(m => m.EnqRegnoId == id));
                        regD.EnqRegnoId = id;
                        var RegTypeId = regD.RegrefCd.ToString();
                        regD.RegTypeText = regType.FindAll(x => x.Value == RegTypeId).FirstOrDefault().Text;
                        regDetailList.Add(regD);

                    }
                    _sessionManager.SetRegistrationDetList(regDetailList);
                    _logger.Information(string.Format("Completed - CreateOrEdit HttpPost with id {0} EnqRegno :{1} EnqRegnoId :{2} RegrefCd :{3} RegTypeText :{4} UniqueId :{5}"
                                         , id, regD.EnqRegno, regD.EnqRegnoId, regD.RegrefCd, regD.RegTypeText, regD.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", regDetailList) });
                }
                ViewBag.ListRegnType = regType;
                _logger.Information(string.Format("Completed - CreateOrEdit HttpPost with id {0} EnqRegno :{1} EnqRegnoId :{2} RegrefCd :{3} RegTypeText :{4} UniqueId :{5}"
                                         , id, regD.EnqRegno, regD.EnqRegnoId, regD.RegrefCd, regD.RegTypeText, regD.UniqueId));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", regD) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! CreateOrEdit HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// post method to delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(RegistrationNoDetailsDTO model)
        {
            try
            {
                _logger.Information("Started - Delete with RegistrationNoDetailsDTO , UniqueId " + model.UniqueId);

                if (model.EnqRegnoId == null || model.EnqRegnoId == 0)
                {
                    return NotFound();
                }
                else
                {
                    var regDetailList = _sessionManager.GetRegistrationDetList();

                    var itemToRemove = regDetailList.Find(r => r.EnqRegnoId == model.EnqRegnoId);
                    regDetailList.Remove(itemToRemove);

                    _sessionManager.SetRegistrationDetList(regDetailList);
                    _logger.Information("Completed - Delete with RegistrationNoDetailsDTO , UniqueId " + model.UniqueId);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", regDetailList) });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Delete HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private async Task<List<SelectListItem>> getRegitrationType()
        {
            EnquiryDTO enquiryDTO = new();
            enquiryDTO.DDLDTO = await _enquirySubmissionService.GetAllEnquiryDropDownList();
            return enquiryDTO.DDLDTO.ListRegnType;
        }
    }
}
