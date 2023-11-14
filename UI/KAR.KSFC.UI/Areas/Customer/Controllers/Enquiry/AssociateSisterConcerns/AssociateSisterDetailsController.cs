using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.AssociateSisterConcerns
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class AssociateSisterDetailsController : Controller
    {
        private const string resultViewPath = "~/Areas/Customer/Views/Enquiry/AssociateSisterConcerns/AssociateSisterDetails/";
        private const string viewPath = "../../Areas/Customer/Views/Enquiry/AssociateSisterConcerns/AssociateSisterDetails/";
        private readonly SessionManager _sessionManager;
        private readonly IEnquirySubmissionService _enquirySubmissionService;
        private readonly ILogger _logger;
        public AssociateSisterDetailsController(SessionManager sessionManager, IEnquirySubmissionService enquirySubmissionService, ILogger logger)
        {
            _sessionManager = sessionManager;
            _enquirySubmissionService = enquirySubmissionService;
            _logger = logger;
        }
        #region sisterconcern
        /// <summary>
        /// Method to view a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecord with id " + id);
                ViewBag.ListFacility = _sessionManager.GetDDLBankFacilityType();

                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new SisterConcernDetailsDTO());
                }
                else
                {
                    var assoSisDetailList = _sessionManager.GetAssociateDetailsDTOList();
                    SisterConcernDetailsDTO asso = assoSisDetailList.Where(x => x.EnqSisId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", asso);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecord page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
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
                _logger.Information("Started - CreateOrEdit method for " + id);
                ViewBag.ListFacility = _sessionManager.GetDDLBankFacilityType();
                ViewBag.Edit = false;
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEdit method for " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new SisterConcernDetailsDTO());
                }
                else
                {
                    ViewBag.Edit = true;
                    var assoSisDetailList = _sessionManager.GetAssociateDetailsDTOList();
                    SisterConcernDetailsDTO asso = assoSisDetailList.Where(x => x.EnqSisId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEdit method for " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", asso);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
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
        public async Task<IActionResult> CreateOrEdit(int id, SisterConcernDetailsDTO asso)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEdit HttpPost method for Id:{0} EnqSisId:{1} EnqtempId:{2} EnqSisName:{3} EnqSisIfsc:{4} BfacilityCode:{5} EnqOutamt:{6} EnqDeftamt:{7} EnqOts:{8} EnqRelief:{9} bankName:{10} EnqSiscibil:{11} UniqueId:{12}",
                    id, asso.EnqSisId, asso.EnqtempId, asso.EnqSisName, asso.EnqSisIfsc, asso.BfacilityCode, asso.EnqOutamt, asso.EnqDeftamt, asso.EnqOts, asso.EnqRelief, asso.bankName, asso.EnqSiscibil, asso.UniqueId));
                AssociateSisterConcernDetailsDTO sis = new();
                sis.ListAssociates = new List<SisterConcernDetailsDTO>();
                sis.ListFYDetails = new List<SisterConcernFinancialDetailsDTO>();
                if (ModelState.IsValid)
                {
                    var ListFacility = _sessionManager.GetDDLBankFacilityType();
                    asso.BfacilityCodeNavigation = new BankFacilityMasterDTO
                    {
                        BfacilityDesc = ListFacility.FirstOrDefault(x => x.Value == asso.BfacilityCode.ToString()).Text
                    };
                    if (id == 0)
                    {
                        sis.ListAssociates.Add(asso);
                        await _enquirySubmissionService.SaveAssociateSisterDetails(sis.ListAssociates);
                    }
                    else
                    {
                        var itemtoUpdate = new List<SisterConcernDetailsDTO>();
                        sis.ListAssociates.Add(asso);
                        await _enquirySubmissionService.UpdateAssociateSisterDetails(sis.ListAssociates);
                    }
                    if (_sessionManager.GetAssociatePrevFYDetailsList() != null)
                    {
                        sis.ListFYDetails.AddRange(_sessionManager.GetAssociatePrevFYDetailsList());
                    }
                    _logger.Information(string.Format("Completed - CreateOrEdit HttpPost method for Id:{0} EnqSisId:{1} EnqtempId:{2} EnqSisName:{3} EnqSisIfsc:{4} BfacilityCode:{5} EnqOutamt:{6} EnqDeftamt:{7} EnqOts:{8} EnqRelief:{9} bankName:{10} EnqSiscibil:{11} UniqueId:{12}",
                    id, asso.EnqSisId, asso.EnqtempId, asso.EnqSisName, asso.EnqSisIfsc, asso.BfacilityCode, asso.EnqOutamt, asso.EnqDeftamt, asso.EnqOts, asso.EnqRelief, asso.bankName, asso.EnqSiscibil, asso.UniqueId));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", sis) });
                }
                _logger.Information(string.Format("Completed - CreateOrEdit HttpPost method for Id:{0} EnqSisId:{1} EnqtempId:{2} EnqSisName:{3} EnqSisIfsc:{4} BfacilityCode:{5} EnqOutamt:{6} EnqDeftamt:{7} EnqOts:{8} EnqRelief:{9} bankName:{10} EnqSiscibil:{11} UniqueId:{12}",
                    id, asso.EnqSisId, asso.EnqtempId, asso.EnqSisName, asso.EnqSisIfsc, asso.BfacilityCode, asso.EnqOutamt, asso.EnqDeftamt, asso.EnqOts, asso.EnqRelief, asso.bankName, asso.EnqSiscibil, asso.UniqueId));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", sis.ListAssociates) });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit HttpPost page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Post method to delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                _logger.Information("Started - Delete method for " + id);
                AssociateSisterConcernDetailsDTO sis = new();
                sis.ListAssociates = new List<SisterConcernDetailsDTO>();
                sis.ListFYDetails = new List<SisterConcernFinancialDetailsDTO>();
                if (_sessionManager.GetAssociatePrevFYDetailsList() != null)
                {
                    sis.ListFYDetails = _sessionManager.GetAssociatePrevFYDetailsList();
                }
                if (_sessionManager.GetAssociateDetailsDTOList() != null)
                {
                    sis.ListAssociates = _sessionManager.GetAssociateDetailsDTOList();
                }
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
                        ViewBag.ErrorSisterConcernExist = "Please delete the Assosiate Sister first in order to delete a FY Details.";
                        _logger.Information("Completed - Delete method for " + id);
                        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", sis) });
                    }
                    assoDetailList.Remove(itemToRemove);
                    _sessionManager.SetAssociateDetailsDTOList(assoDetailList);
                    sis.ListAssociates = assoDetailList;
                    await _enquirySubmissionService.DeleteSisterConcernDetails(id);
                    _logger.Information("Completed - Delete method for " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", sis) });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading Delete page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        #endregion

        #region FyDetail
        /// <summary>
        /// Get method to view a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecordFy(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecordFy method for " + id);
                var assoDetailList = _sessionManager.GetAssociateDetailsDTOList();
                IList<SelectListItem> associateDetailsList = new List<SelectListItem>();
                if (assoDetailList != null)
                {
                    foreach (var associate in assoDetailList)
                        associateDetailsList.Add(new SelectListItem() { Text = associate.EnqSisName, Value = associate.EnqSisId.ToString() });
                }
                ViewBag.AssociateConcernList = associateDetailsList;
                ViewBag.FinancialYearsList = _sessionManager.GetDDListFinancialYear();
                ViewBag.ListFinancialComponent = _sessionManager.GetDDListFinancialComponent();

                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecordFy method for " + id);
                    return View(resultViewPath + "ViewRecordFy.cshtml", new SisterConcernFinancialDetailsDTO());
                }
                else
                {
                    var assoFinanceDetailList = _sessionManager.GetAssociatePrevFYDetailsList();
                    SisterConcernFinancialDetailsDTO assoFinance = assoFinanceDetailList.Where(x => x.EnqSisfinId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecordFy method for " + id);
                    return View(resultViewPath + "ViewRecordFy.cshtml", assoFinance);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecordFy page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Get Method to create or edit a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEditFy(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEditFy method for " + id);
                var assoDetailList = _sessionManager.GetAssociateDetailsDTOList();
                if (assoDetailList == null || assoDetailList.Count == 0)
                {
                    assoDetailList = new List<SisterConcernDetailsDTO>();
                }
                ViewBag.AssociateConcernList = assoDetailList;
                ViewBag.FinancialYearsList = _sessionManager.GetDDListFinancialYear();
                ViewBag.ListFinancialComponent = _sessionManager.GetDDListFinancialComponent();
                ViewBag.Edit = false;
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEditFy method for " + id);
                    return View(resultViewPath + "CreateOrEditFy.cshtml", new SisterConcernFinancialDetailsDTO());
                }
                else
                {
                    ViewBag.Edit = true;
                    var assoFinanceDetailList = _sessionManager.GetAssociatePrevFYDetailsList();
                    SisterConcernFinancialDetailsDTO assoFinance = assoFinanceDetailList.Where(x => x.EnqSisfinId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEditFy method for " + id);
                    return View(resultViewPath + "CreateOrEditFy.cshtml", assoFinance);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditFy page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
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
        public IActionResult CreateOrEditFy(int id, SisterConcernFinancialDetailsDTO assoFinance)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEditFy method for Id:{0} EnqSisfinId:{1} EnqSisId:{2} FinyearCode:{3} FincompCd:{4} EnqFinamt:{5} WhProv:{6} UniqueId:{7} EnqtempId:{8}",
                    id, assoFinance.EnqSisfinId, assoFinance.EnqSisId, assoFinance.FinyearCode, assoFinance.FincompCd, assoFinance.EnqFinamt, assoFinance.WhProv, assoFinance.UniqueId, assoFinance.EnqtempId));
                List<SisterConcernFinancialDetailsDTO> assoFinanceDetailList = new();
                AssociateSisterConcernDetailsDTO sis = new();
                sis.ListAssociates = new List<SisterConcernDetailsDTO>();
                sis.ListFYDetails = new List<SisterConcernFinancialDetailsDTO>();
                if (ModelState.IsValid)
                {

                    var FinancialYearsList = _sessionManager.GetDDListFinancialYear();
                    var ListFinancialComponent = _sessionManager.GetDDListFinancialComponent();
                    assoFinance.FinyearCodeNavigation = new FinancialYearMasterDTO
                    {
                        FinyearDesc = FinancialYearsList.
                        FirstOrDefault(x => x.Value == assoFinance.FinyearCode.ToString()).Text,
                        FinyearCode = assoFinance.FinyearCode.Value
                    };
                    assoFinance.FincompCdNavigation = new FinancialComponentMasterDTO
                    {
                        FincompDets = ListFinancialComponent.FirstOrDefault(x => x.Value == assoFinance.FincompCd.ToString()).Text
                        ,
                        FincompCd = assoFinance.FincompCd.Value
                    };
                    var assoDetailList = _sessionManager.GetAssociateDetailsDTOList();
                    assoFinance.EnqSis = new SisterConcernDetailsDTO();
                    assoFinance.EnqSis = assoDetailList.FirstOrDefault(x => x.EnqSisId == assoFinance.EnqSisId);
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
                    if (_sessionManager.GetAssociateDetailsDTOList() != null)
                    {
                        sis.ListAssociates.AddRange(_sessionManager.GetAssociateDetailsDTOList());
                    }
                    if (_sessionManager.GetAssociatePrevFYDetailsList() != null)
                    {
                        sis.ListFYDetails.AddRange(_sessionManager.GetAssociatePrevFYDetailsList());
                    }

                    _logger.Information(string.Format("Completed - CreateOrEditFy method for Id:{0} EnqSisfinId:{1} EnqSisId:{2} FinyearCode:{3} FincompCd:{4} EnqFinamt:{5} WhProv:{6} UniqueId:{7} EnqtempId:{8}",
                    id, assoFinance.EnqSisfinId, assoFinance.EnqSisId, assoFinance.FinyearCode, assoFinance.FincompCd, assoFinance.EnqFinamt, assoFinance.WhProv, assoFinance.UniqueId, assoFinance.EnqtempId));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", sis) });
                }
                _logger.Information(string.Format("Completed - CreateOrEditFy method for Id:{0} EnqSisfinId:{1} EnqSisId:{2} FinyearCode:{3} FincompCd:{4} EnqFinamt:{5} WhProv:{6} UniqueId:{7} EnqtempId:{8}",
                    id, assoFinance.EnqSisfinId, assoFinance.EnqSisId, assoFinance.FinyearCode, assoFinance.FincompCd, assoFinance.EnqFinamt, assoFinance.WhProv, assoFinance.UniqueId, assoFinance.EnqtempId));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEditFy", assoFinance) });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditFy page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to delete a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteFy(int id)
        {
            try
            {
                _logger.Information("Started - DeleteFy method for Id = " + id);
                AssociateSisterConcernDetailsDTO sis = new();
                sis.ListAssociates = new List<SisterConcernDetailsDTO>();
                sis.ListFYDetails = new List<SisterConcernFinancialDetailsDTO>();

                if (_sessionManager.GetAssociateDetailsDTOList() != null)
                {
                    sis.ListAssociates = _sessionManager.GetAssociateDetailsDTOList();
                }
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
                    sis.ListFYDetails = assoFinanceDetailList;
                    _logger.Information("Completed - DeleteFy method for Id = " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", sis) });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteFy page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        #endregion
    }
}
