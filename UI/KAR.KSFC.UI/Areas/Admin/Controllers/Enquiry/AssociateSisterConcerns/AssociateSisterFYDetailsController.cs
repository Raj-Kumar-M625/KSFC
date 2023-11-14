using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.AssociateSisterConcerns
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class AssociateSisterFYDetailsController : Controller
    {
        private const string resultViewPath = "~/Areas/Admin/Views/Enquiry/AssociateSisterConcerns/AssociateSisterFYDetails/";
        private const string viewPath = "../../Areas/Admin/Views/Enquiry/AssociateSisterConcerns/AssociateSisterFYDetails/";
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;

        public AssociateSisterFYDetailsController(SessionManager sessionManager, ILogger logger)
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
                _logger.Information("Started - AssociateSisterFYDetails/ViewRecord with id " + id);

                var assoDetailList = _sessionManager.GetAssociateDetailsDTOList();
                IList<SelectListItem> associateDetailsList = new List<SelectListItem>();
                if (assoDetailList != null)
                {
                    foreach (var associate in assoDetailList)
                        associateDetailsList.Add(new SelectListItem() { Text = associate.EnqSisName, Value = associate.EnqSisName });
                }
                ViewBag.AssociateConcernList = associateDetailsList;
                ViewBag.FinancialYearsList = _sessionManager.GetDDListFinancialYear();
                ViewBag.ListFinancialComponent = _sessionManager.GetDDListFinancialComponent();

                if (id == 0)
                {
                    _logger.Information("Started - AssociateSisterFYDetails/ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new SisterConcernFinancialDetailsDTO());
                }
                else
                {
                    var assoFinanceDetailList = _sessionManager.GetAssociatePrevFYDetailsList();
                    SisterConcernFinancialDetailsDTO assoFinance = assoFinanceDetailList.Where(x => x.EnqSisfinId == id).FirstOrDefault();
                    _logger.Information("Started - Associate sister details/ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", assoFinance);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! AssociateSisterFYDetails/ViewRecord page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Get Method to create or edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEdit(int id = 0)
        {
            try
            {
                _logger.Information("Started - AssociateSisterFYDetails/CreateOrEdit HttpGet with id " + id);

                var assoDetailList = _sessionManager.GetAssociateDetailsDTOList();
                IList<SelectListItem> associateDetailsList = new List<SelectListItem>();
                if (assoDetailList != null)
                {
                    foreach (var associate in assoDetailList)
                        associateDetailsList.Add(new SelectListItem() { Text = associate.EnqSisName, Value = associate.EnqSisName });
                }
                ViewBag.AssociateConcernList = associateDetailsList;
                ViewBag.FinancialYearsList = _sessionManager.GetDDListFinancialYear();
                ViewBag.ListFinancialComponent = _sessionManager.GetDDListFinancialComponent();

                if (id == 0)
                {
                    _logger.Information("Completed - AssociateSisterFYDetails/CreateOrEdit HttpGet with id " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new SisterConcernFinancialDetailsDTO());
                }
                else
                {
                    var assoFinanceDetailList = _sessionManager.GetAssociatePrevFYDetailsList();
                    SisterConcernFinancialDetailsDTO assoFinance = assoFinanceDetailList.Where(x => x.EnqSisfinId == id).FirstOrDefault();
                    _logger.Information("Completed - AssociateSisterFYDetails/ViewRecord with id " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", assoFinance);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! AssociateSisterFYDetails/CreateOrEdit page HttpGet. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to create or edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assoFinance"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, SisterConcernFinancialDetailsDTO assoFinance)
        {
            try
            {
                _logger.Information(string.Format("Started - AssociateSisterFYDetails/CreateOrEdit HttpPost with employee id {0} EnqFinamt :{1} EnqSis :{2} FincompCd :{3} UniqueId :{4}"
                   , id, assoFinance.EnqFinamt, assoFinance.EnqSis, assoFinance.FincompCd, assoFinance.UniqueId));

                List<SisterConcernFinancialDetailsDTO> assoFinanceDetailList = new();
                if (ModelState.IsValid)
                {

                    var FinancialYearsList = _sessionManager.GetDDListFinancialYear();
                    var ListFinancialComponent = _sessionManager.GetDDListFinancialComponent();
                    assoFinance.FinyearCodeNavigation = new FinancialYearMasterDTO
                    {
                        FinyearDesc = FinancialYearsList.
                        FirstOrDefault(x => x.Value == assoFinance.FinyearCode.ToString()).Text
                    };
                    assoFinance.FincompCdNavigation = new FinancialComponentMasterDTO
                    {
                        FincompDets = ListFinancialComponent.FirstOrDefault(x => x.Value == assoFinance.FincompCd.ToString()).Text
                    };
                    if (id == 0)
                    {
                        if (_sessionManager.GetAssociatePrevFYDetailsList() != null)
                            assoFinanceDetailList = _sessionManager.GetAssociatePrevFYDetailsList();

                        assoFinance.EnqSisfinId = assoFinanceDetailList.Max(x => x.EnqSisfinId) + 1 ?? 1;
                        assoFinanceDetailList.Add(assoFinance);

                    }
                    else
                    {
                        assoFinanceDetailList = _sessionManager.GetAssociatePrevFYDetailsList();
                        assoFinanceDetailList.Remove(assoFinanceDetailList.Find(m => m.EnqSisfinId == id));
                        assoFinance.EnqSisfinId = id;
                        assoFinanceDetailList.Add(assoFinance);

                    }
                    _sessionManager.SetAssociatePrevFYDetailsList(assoFinanceDetailList);

                    _logger.Information(string.Format("Completed - AssociateSisterFYDetails/CreateOrEdit HttpPost with employee id {0} EnqFinamt :{1} EnqSis :{2} FincompCd :{3} UniqueId :{4}"
                          , id, assoFinance.EnqFinamt, assoFinance.EnqSis, assoFinance.FincompCd, assoFinance.UniqueId));


                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", assoFinanceDetailList) });
                }
                _logger.Information(string.Format("Completed - AssociateSisterFYDetails/CreateOrEdit HttpPost with employee id {0} EnqFinamt :{1} EnqSis :{2} FincompCd :{3} UniqueId :{4}"
                          , id, assoFinance.EnqFinamt, assoFinance.EnqSis, assoFinance.FincompCd, assoFinance.UniqueId));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", assoFinance) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! AssociateSisterFYDetails/CreateOrEdit page HttpPost. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
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
                _logger.Information("Started - AssociateSisterFYDetails/Delete HttpPost with id " + id);

                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var assoFinanceDetailList = _sessionManager.GetAssociatePrevFYDetailsList();

                    var itemToRemove = assoFinanceDetailList.Find(r => r.EnqSisfinId == id);
                    assoFinanceDetailList.Remove(itemToRemove);

                    _sessionManager.SetAssociatePrevFYDetailsList(assoFinanceDetailList);

                    _logger.Information("Started - AssociateSisterFYDetails/Delete HttpPost with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", assoFinanceDetailList) });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! AssociateSisterFYDetails/Delete page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
    }
}
