using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.AssociateSisterConcerns
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class AssociateSisterDetailsController : Controller
    {
        private const string resultViewPath = "~/Areas/Admin/Views/Enquiry/AssociateSisterConcerns/AssociateSisterDetails/";
        private const string viewPath = "../../Areas/Admin/Views/Enquiry/AssociateSisterConcerns/AssociateSisterDetails/";
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;

        public AssociateSisterDetailsController(SessionManager sessionManager, ILogger logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }

        /// <summary>
        /// Method to view a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - Associate sister details/ViewRecord with id " + id);

                _logger.Information("Invoking sessionManager for GetDDLBankFacilityType ");

                ViewBag.ListFacility = _sessionManager.GetDDLBankFacilityType();

                _logger.Information("Completed sessionManager for GetDDLBankFacilityType ");
                if (id == 0)
                {
                    _logger.Information("Completed - Associate sister details/ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new SisterConcernDetailsDTO());
                }
                else
                {
                    var assoSisDetailList = _sessionManager.GetAssociateDetailsDTOList();
                    SisterConcernDetailsDTO asso = assoSisDetailList.Where(x => x.EnqSisId == id).FirstOrDefault();
                    _logger.Information("Completed - Associate sister details/ViewRecord with id " + id);

                    return View(resultViewPath + "ViewRecord.cshtml", asso);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Associate sister details/ViewRecord page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
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

                _logger.Information("Started - Associate sister details/CreateOrEdit HttpGet with employee id " + id);

                _logger.Information("Invoking sessionManager for GetDDLBankFacilityType ");

                ViewBag.ListFacility = _sessionManager.GetDDLBankFacilityType();

                _logger.Information("Completed sessionManager for GetDDLBankFacilityType ");

                if (id == 0)
                {
                    _logger.Information("Completed - Associate sister details/CreateOrEdit with employee id " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new SisterConcernDetailsDTO());
                }
                else
                {
                    _logger.Information("Invoking sessionManager for GetAssociateDetailsDTOList ");
                    var assoSisDetailList = _sessionManager.GetAssociateDetailsDTOList();
                    _logger.Information("Completed sessionManager for GetAssociateDetailsDTOList with result count " + assoSisDetailList.Count);
                    SisterConcernDetailsDTO asso = assoSisDetailList.Where(x => x.EnqSisId == id).FirstOrDefault();

                    _logger.Information("Completed - Associate sister details/CreateOrEdit with employee id " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", asso);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Associate sister details/CreateOrEdit HttpGet page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to Create or Edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asso"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, SisterConcernDetailsDTO asso)
        {
            try
            {
                _logger.Information(string.Format("Started - Associate sister details/CreateOrEdit with employee id {0} bankName :{1} EnqSisIfsc :{2} EnqSisName :{3} EnqSiscibil :{4} UniqueId :{5}"
                       , id, asso.bankName, asso.EnqSisIfsc, asso.EnqSisName, asso.EnqSiscibil, asso.UniqueId));

                List<SisterConcernDetailsDTO> sisterDetailList = new();


                if (ModelState.IsValid)
                {
                    var ListFacility = _sessionManager.GetDDLBankFacilityType();
                    asso.BfacilityCodeNavigation = new BankFacilityMasterDTO
                    {
                        BfacilityDesc = ListFacility.FirstOrDefault(x => x.Value == asso.BfacilityCode.ToString()).Text
                    };
                    if (id == 0)
                    {
                        if (_sessionManager.GetAssociateDetailsDTOList() != null)
                            sisterDetailList = _sessionManager.GetAssociateDetailsDTOList();
                        asso.EnqSisId = sisterDetailList.Max(x => x.EnqSisId) + 1 ?? 1;
                        sisterDetailList.Add(asso);

                    }
                    else
                    {
                        sisterDetailList = _sessionManager.GetAssociateDetailsDTOList();
                        sisterDetailList.Remove(sisterDetailList.Find(m => m.EnqSisId == id));
                        asso.EnqSisId = id;
                        sisterDetailList.Add(asso);

                    }
                    _sessionManager.SetAssociateDetailsDTOList(sisterDetailList);

                    _logger.Information(string.Format("Completed - Associate sister details/CreateOrEdit with employee id {0} bankName :{1} EnqSisIfsc :{2} EnqSisName :{3} EnqSiscibil :{4} UniqueId :{5}"
                        , id, asso.bankName, asso.EnqSisIfsc, asso.EnqSisName, asso.EnqSiscibil, asso.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", sisterDetailList) });
                }

                _logger.Information(string.Format("Completed - Associate sister details/CreateOrEdit with employee id {0} bankName :{1} EnqSisIfsc :{2} EnqSisName :{3} EnqSiscibil :{4} UniqueId :{5}"
                       , id, asso.bankName, asso.EnqSisIfsc, asso.EnqSisName, asso.EnqSiscibil, asso.UniqueId));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", asso) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Associate sister details/CreateOrEdit HttpGet page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
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
                _logger.Information("Started - Associate sister details/Delete HttpPost with id " + id);
                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var assoDetailList = _sessionManager.GetAssociateDetailsDTOList();
                    var itemToRemove = assoDetailList.Find(r => r.EnqSisId == id);

                    var assoFinanceDetailList = _sessionManager.GetAssociatePrevFYDetailsList();
                    var item = assoFinanceDetailList.FirstOrDefault(x => x.EnqSis.EnqSisName == itemToRemove.EnqSisName);
                    if (item != null)
                    {
                        _logger.Information("Completed - Associate sister details/Delete HttpPost with id " + id);
                        ViewBag.ErrorSisterConcernExist = "Please delete the Assosiate Sister first in order to delete a FY Details.";
                        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", assoDetailList) });
                    }
                    assoDetailList.Remove(itemToRemove);
                    _sessionManager.SetAssociateDetailsDTOList(assoDetailList);

                    _logger.Information("Completed - Associate sister details/Delete HttpPost with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", assoDetailList) });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Associate sister details/Delete page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
