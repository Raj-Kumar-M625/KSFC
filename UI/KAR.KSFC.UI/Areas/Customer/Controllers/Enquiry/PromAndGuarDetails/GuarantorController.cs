using System;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System.Threading.Tasks;
using KAR.KSFC.UI.Services.IServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using KAR.KSFC.Components.Common.Logging.Client;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.PromAndGuarDetails
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class GuarantorController : Controller
    {
        private const string resultViewPath = "~/Areas/Customer/Views/Enquiry/PromAndGuarDetails/Guarantor/";
        private const string viewPath = "../../Areas/Customer/Views/Enquiry/PromAndGuarDetails/Guarantor/";
        private readonly SessionManager _sessionManager;
        private readonly IEnquirySubmissionService _enquirySubmissionService;
        private readonly ILogger _logger;
        public GuarantorController(SessionManager sessionManager, IEnquirySubmissionService enquirySubmissionService, ILogger logger)
        {
            _sessionManager = sessionManager;
            _enquirySubmissionService = enquirySubmissionService;
            _logger = logger;
        }

        /// <summary>
        /// Get Method to View a guarantor details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecord method for Id = " + id);
                ViewBag.ListDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                ViewBag.Gender = await _enquirySubmissionService.GetGenderTypes();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord method for Id = " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new GuarantorDetailsDTO());
                }
                else
                {
                    var guaDetailsList = _sessionManager.GetGuarantorDetailsList();
                    GuarantorDetailsDTO gua = guaDetailsList.Where(x => x.EnqGuarId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecord method for Id = " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", gua);
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditLiability page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Get method to create or edit guarantor details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateOrEdit(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEdit method for Id = " + id);
                ViewBag.Gender = await _enquirySubmissionService.GetGenderTypes();
                ViewBag.ListDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEdit method for Id = " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", new GuarantorDetailsDTO());
                }
                else
                {
                    var guaDetailsList = _sessionManager.GetGuarantorDetailsList();
                    GuarantorDetailsDTO gua = guaDetailsList.Where(x => x.EnqGuarId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEdit method for Id = " + id);
                    return View(resultViewPath + "CreateOrEdit.cshtml", gua);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// post method to create or edit guarantor details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gua"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(int id, GuarantorDetailsDTO gua)
        {
            
            try
            {
                _logger.Information(string.Format("Started - CreateOrEdit method for Id: {0} EnqGuarId :{1} EnqtempId :{2} PromoterCode :{3} DomCd :{4} GuarName :{5} Pan :{6} pinCode :{7} EnqGuarcibil :{8} UniqueId ",
                    id, gua.EnqGuarId, gua.EnqtempId, gua.PromoterCode, gua.DomCd, gua.GuarName, gua.Pan, gua.pinCode, gua.EnqGuarcibil, gua.UniqueId));
                gua.PromoterMaster.PromoterPassport = "NOPASSPORT";
                // promotor date of birth is required but in gurantor doesn't have any property  dob that's why default value given here
                gua.PromoterMaster.PromoterDob = DateTime.Now;
                List<GuarantorDetailsDTO> guaDetailsList = new();
                var domList = _sessionManager.GetDDListDomicileStatus();
                gua.DomicileMasterDTO = new DomicileMasterDTO
                {
                    DomDets = domList.FirstOrDefault(x => x.Value == gua.DomCd.ToString()).Text
                };

                    gua.DomicileMasterDTO = new DomicileMasterDTO
                    {
                        DomDets = domList.FirstOrDefault(x => x.Value == gua.DomCd.ToString()).Text
                    };

                    if (id == 0)
                    {

                        guaDetailsList.Add(gua);
                        await _enquirySubmissionService.SaveGuarantorDetails(guaDetailsList);
                    }
                    else
                    {
                        var itemtoUpdate = new List<GuarantorDetailsDTO>();
                        itemtoUpdate.Add(gua);
                        await _enquirySubmissionService.UpdateGuarantorDetails(itemtoUpdate);
                    }
                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();
                    _logger.Information(string.Format("Completed - CreateOrEdit method for Id: {0} EnqGuarId :{1} EnqtempId :{2} PromoterCode :{3} DomCd :{4} GuarName :{5} Pan :{6} pinCode :{7} EnqGuarcibil :{8} UniqueId ",
                    id, gua.EnqGuarId, gua.EnqtempId, gua.PromoterCode, gua.DomCd, gua.GuarName, gua.Pan, gua.pinCode, gua.EnqGuarcibil, gua.UniqueId));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                
                  }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEdit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to delete guarantor details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                _logger.Information("Started - Delete method for Id = " + id);
                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var guarAssetDetailsList = _sessionManager.GetGuarantorAssetList();
                    var guarLiaDetailsList = _sessionManager.GetGuarantorLiabilityList();

                    if (guarAssetDetailsList != null)
                    {
                        var ifGuaAssetExist = guarAssetDetailsList.Find(x => x.GuarantorDetailsDTO.EnqGuarId == id);
                        if (ifGuaAssetExist != null)
                        {
                            ViewBag.ErrorGuaAssLiaExist = "Please delete the guarantor assets and liabilities first in order to delete a guarantor.";
                            GuarantorAllDetailsDTO guarantorAllDetailsDTOExist = GuarantorAssetLiabilityNetWorth();
                            _logger.Information("Completed - Delete method for Id = " + id);
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTOExist) });
                        }
                    }
                    if (guarLiaDetailsList != null)
                    {
                        var ifGuaLiabilityExist = guarLiaDetailsList.Find(x => x.GuarantorDetailsDTO.EnqGuarId == id);
                        if (ifGuaLiabilityExist != null)
                        {
                            ViewBag.ErrorGuaAssLiaExist = "Please delete the guarantor assets and liabilities first in order to delete a guarantor.";
                            GuarantorAllDetailsDTO guarantorAllDetailsDTOExist = GuarantorAssetLiabilityNetWorth();
                            _logger.Information("Completed - Delete method for Id = " + id);
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTOExist) });
                        }
                    }

                    var guaDetailList = _sessionManager.GetGuarantorDetailsList();
                    var itemToRemove = guaDetailList.Find(r => r.EnqGuarId == id);
                    guaDetailList.Remove(itemToRemove);
                    _sessionManager.SetGuarantorDetailsList(guaDetailList);
                    await _enquirySubmissionService.DeleteGuarantor(id);
                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();
                    _logger.Information("Completed - Delete method for Id = " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading Delete page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Get method to View guarantor Asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecordAsset(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecordAsset method for Id = " + id);
                ViewBag.GuarantorDetailsList = _sessionManager.GetGuarantorDetailsList();
                ViewBag.ListPromAndGuarAssetType = _sessionManager.GetDDListPromAndGuarAssetType();
                ViewBag.ListPromAndGuarAssetCategory = _sessionManager.GetDDListPromAndGuarAssetCategory();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecordAsset method for Id = " + id);
                    return View(resultViewPath + "ViewRecordAsset.cshtml", new GuarantorAssetsNetWorthDTO());
                }
                else
                {
                    var guarAssetList = _sessionManager.GetGuarantorAssetList();
                    GuarantorAssetsNetWorthDTO guar = guarAssetList.Where(x => x.EnqGuarassetId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecordAsset method for Id = " + id);
                    return View(resultViewPath + "ViewRecordAsset.cshtml", guar);
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecordAsset page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpGet]
        public IActionResult CheckGuarantorExit()
        {
            try
            {
                _logger.Information("Started - CheckGuarantorExit method HtppGet");
                var Gurantors = _sessionManager.GetGuarantorDetailsList();
                if (Gurantors != null)
                {
                    _logger.Information("Completed - CheckGuarantorExit method HtppGet");
                    return Json(new { IsValid = true, Message = "please add guarantor" });
                }
                else
                {
                    _logger.Information("Completed - CheckGuarantorExit method HtppGet");
                    return Json(new { IsValid = false, Message = "please add guarantor" });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CheckGuarantorExit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// create or edit guarantor asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEditAsset(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEditAsset method for Id = " + id);
                ViewBag.GuarantorDetailsList = _sessionManager.GetGuarantorDetailsList();
                ViewBag.ListPromAndGuarAssetType = _sessionManager.GetDDListPromAndGuarAssetType();
                ViewBag.ListPromAndGuarAssetCategory = _sessionManager.GetDDListPromAndGuarAssetCategory();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEditAsset method for Id = " + id);
                    return View(resultViewPath + "CreateOrEditAsset.cshtml", new GuarantorAssetsNetWorthDTO());
                }
                else
                {
                    var guarAssetList = _sessionManager.GetGuarantorAssetList();
                    GuarantorAssetsNetWorthDTO guar = guarAssetList.Where(x => x.EnqGuarassetId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEditAsset method for Id = " + id);
                    return View(resultViewPath + "CreateOrEditAsset.cshtml", guar);
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditAsset page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to Create or Edit Guarantor Asset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="guarAsset"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEditAsset(int id, GuarantorAssetsNetWorthDTO guarAsset)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEditAsset method for Id: {0} EnqGuarassetId :{1} EnqtempId :{2} PromoterCode :{3} AssetcatCd :{4} AssettypeCd :{5} GuarAssetDesc :{6} GuarAssetValue :{7}  GuarAssetSiteno :{8} GuarAssetAddr :{9} GuarAssetDim :{10} GuarAssetArea :{11} UniqueId :{12} EnqGuarId  ",
                    id, guarAsset.EnqGuarassetId, guarAsset.EnqtempId, guarAsset.PromoterCode, guarAsset.AssetcatCd, guarAsset.AssettypeCd, guarAsset.GuarAssetDesc, guarAsset.GuarAssetValue, guarAsset.GuarAssetSiteno, guarAsset.GuarAssetAddr, guarAsset.GuarAssetDim, guarAsset.GuarAssetArea, guarAsset.UniqueId, guarAsset.EnqGuarId));
                List<GuarantorAssetsNetWorthDTO> guarAssetList = new();
                if (ModelState.IsValid)
                {
                    var guarantorList = _sessionManager.GetGuarantorDetailsList();

                    guarAsset.AssetCategoryMasterDTO = new AssetCategoryMasterDTO
                    {
                        AssetcatDets = _sessionManager.GetDDListPromAndGuarAssetCategory().FirstOrDefault(x => x.Value == guarAsset.AssetcatCd.ToString()).Text
                    };
                    guarAsset.AssetTypeMasterDTO = new AssetTypeMasterDTO
                    {
                        AssettypeDets = _sessionManager.GetDDListPromAndGuarAssetType().FirstOrDefault(x => x.Value == guarAsset.AssetcatCd.ToString()).Text
                    };
                    guarAsset.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                    guarAsset.GuarantorDetailsDTO.PromoterMaster = guarantorList.Find(x => x.PromoterCode == guarAsset.PromoterCode).PromoterMaster;
                    if (id == 0)
                    {
                        if (_sessionManager.GetGuarantorAssetList() != null)
                            guarAssetList = _sessionManager.GetGuarantorAssetList();
                        guarAsset.EnqGuarassetId = guarAssetList.Max(x => x.EnqGuarassetId) + 1 ?? 1; //Increment ID

                        guarAssetList.Add(guarAsset);
                    }
                    else
                    {
                        guarAssetList = _sessionManager.GetGuarantorAssetList();
                        guarAssetList.Remove(guarAssetList.Find(m => m.EnqGuarassetId == id));
                        guarAsset.EnqGuarassetId = id;
                        guarAssetList.Add(guarAsset);
                    }

                    _sessionManager.SetGuarantorAssetList(guarAssetList);

                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();
                    _logger.Information(string.Format("Completed - CreateOrEditAsset method for Id: {0} EnqGuarassetId :{1} EnqtempId :{2} PromoterCode :{3} AssetcatCd :{4} AssettypeCd :{5} GuarAssetDesc :{6} GuarAssetValue :{7}  GuarAssetSiteno :{8} GuarAssetAddr :{9} GuarAssetDim :{10} GuarAssetArea :{11} UniqueId :{12} EnqGuarId  ",
                    id, guarAsset.EnqGuarassetId, guarAsset.EnqtempId, guarAsset.PromoterCode, guarAsset.AssetcatCd, guarAsset.AssettypeCd, guarAsset.GuarAssetDesc, guarAsset.GuarAssetValue, guarAsset.GuarAssetSiteno, guarAsset.GuarAssetAddr, guarAsset.GuarAssetDim, guarAsset.GuarAssetArea, guarAsset.UniqueId, guarAsset.EnqGuarId));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }
                _logger.Information(string.Format("Completed - CreateOrEditAsset method for Id: {0} EnqGuarassetId :{1} EnqtempId :{2} PromoterCode :{3} AssetcatCd :{4} AssettypeCd :{5} GuarAssetDesc :{6} GuarAssetValue :{7}  GuarAssetSiteno :{8} GuarAssetAddr :{9} GuarAssetDim :{10} GuarAssetArea :{11} UniqueId :{12} EnqGuarId  ",
                    id, guarAsset.EnqGuarassetId, guarAsset.EnqtempId, guarAsset.PromoterCode, guarAsset.AssetcatCd, guarAsset.AssettypeCd, guarAsset.GuarAssetDesc, guarAsset.GuarAssetValue, guarAsset.GuarAssetSiteno, guarAsset.GuarAssetAddr, guarAsset.GuarAssetDim, guarAsset.GuarAssetArea, guarAsset.UniqueId, guarAsset.EnqGuarId));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", guarAsset) });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditAsset page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Deletes a Guarantor Asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAsset(int id)
        {
            try
            {
                _logger.Information("Started - DeleteAsset method for Id = " + id);
                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var guarAssetList = _sessionManager.GetGuarantorAssetList();
                    var itemToRemove = guarAssetList.Find(r => r.EnqGuarassetId == id);
                    guarAssetList.Remove(itemToRemove);
                    _sessionManager.SetGuarantorAssetList(guarAssetList);
                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();
                    _logger.Information("Completed - DeleteAsset method for Id = " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteAsset page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Get method to view a Guarantor Liability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecordLiability(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecordLiability method for Id = " + id);
                ViewBag.GuarantorDetailsList = _sessionManager.GetGuarantorDetailsList();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecordLiability method for Id = " + id);
                    return View(resultViewPath + "ViewRecordLiability.cshtml", new GuarantorDetailsDTO());
                }
                else
                {
                    var guarLiaDetailList = _sessionManager.GetGuarantorLiabilityList();
                    GuarantorLiabilityDetailsDTO liability = guarLiaDetailList.Where(x => x.EnqGuarliabId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecordLiability method for Id = " + id);
                    return View(resultViewPath + "ViewRecordLiability.cshtml", liability);
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ViewRecordLiability page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Get method to Create or Edit Guarantor liability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEditLiability(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEditLiability method for Id = " + id);
                ViewBag.GuarantorDetailsList = _sessionManager.GetGuarantorDetailsList();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEditLiability method for Id = " + id);
                    return View(resultViewPath + "CreateOrEditLiability.cshtml", new GuarantorLiabilityDetailsDTO());
                }
                else
                {

                    var guarLiaDetailList = _sessionManager.GetGuarantorLiabilityList();
                    GuarantorLiabilityDetailsDTO liability = guarLiaDetailList.Where(x => x.EnqGuarliabId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEditLiability method for Id = " + id);
                    return View(resultViewPath + "CreateOrEditLiability.cshtml", liability);
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditLiability page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to Create or Edit Guarantor liability
        /// </summary>
        /// <param name="id"></param>
        /// <param name="liability"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEditLiability(int id, GuarantorLiabilityDetailsDTO liability)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEditLiability method for Id: {0} EnqGuarliabId :{1} EnqtempId :{2} PromoterCode :{3} GuarLiabDesc :{4} GuarLiabValue :{5} UniqueId :{6} EnqGuarId ",
                    id, liability.EnqGuarliabId, liability.EnqtempId, liability.PromoterCode, liability.GuarLiabDesc, liability.GuarLiabValue, liability.UniqueId, liability.EnqGuarId));
                List<GuarantorLiabilityDetailsDTO> guarLiaDetailsList = new();

                if (ModelState.IsValid)
                {
                    var guarantorList = _sessionManager.GetGuarantorDetailsList();

                    liability.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                    liability.GuarantorDetailsDTO.PromoterMaster = guarantorList.Find(x => x.PromoterCode == liability.PromoterCode).PromoterMaster;
                    if (id == 0)
                    {
                        if (_sessionManager.GetGuarantorLiabilityList() != null)
                            guarLiaDetailsList = _sessionManager.GetGuarantorLiabilityList();
                        liability.EnqGuarliabId = guarLiaDetailsList.Max(x => x.EnqGuarliabId) + 1 ?? 1;
                        guarLiaDetailsList.Add(liability);
                    }
                    else
                    {
                        guarLiaDetailsList = _sessionManager.GetGuarantorLiabilityList();
                        guarLiaDetailsList.Remove(guarLiaDetailsList.Find(m => m.EnqGuarliabId == id));
                        liability.EnqGuarliabId = id;
                        guarLiaDetailsList.Add(liability);
                    }

                    _sessionManager.SetGuarantorLiabilityList(guarLiaDetailsList);

                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();
                    _logger.Information(string.Format("Completed - CreateOrEditLiability method for Id: {0} EnqGuarliabId :{1} EnqtempId :{2} PromoterCode :{3} GuarLiabDesc :{4} GuarLiabValue :{5} UniqueId :{6} EnqGuarId ",
                    id, liability.EnqGuarliabId, liability.EnqtempId, liability.PromoterCode, liability.GuarLiabDesc, liability.GuarLiabValue, liability.UniqueId, liability.EnqGuarId));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }
                _logger.Information(string.Format("Completed - CreateOrEditLiability method for Id: {0} EnqGuarliabId :{1} EnqtempId :{2} PromoterCode :{3} GuarLiabDesc :{4} GuarLiabValue :{5} UniqueId :{6} EnqGuarId ",
                    id, liability.EnqGuarliabId, liability.EnqtempId, liability.PromoterCode, liability.GuarLiabDesc, liability.GuarLiabValue, liability.UniqueId, liability.EnqGuarId));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", liability) });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading CreateOrEditLiability page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Deletes a Guarantor Liability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLiability(int id)
        {
            try
            {
                _logger.Information("Started - DeleteLiability method for Id = " + id);
                if (id == 0)
                {
                    return NotFound();
                }
                else
                {
                    var guarLiaDetailsList = _sessionManager.GetGuarantorLiabilityList();

                    var itemToRemove = guarLiaDetailsList.Find(r => r.EnqGuarliabId == id);
                    guarLiaDetailsList.Remove(itemToRemove);

                    _sessionManager.SetGuarantorLiabilityList(guarLiaDetailsList);
                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();
                    _logger.Information("Completed - DeleteLiability method for Id = " + id);
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteLiability page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Calculates real time Asset, Liability and NetWorth from Session values
        /// </summary>
        /// <returns></returns>
        public GuarantorAllDetailsDTO GuarantorAssetLiabilityNetWorth()
        {
            //ModelState.Clear();
            decimal totalAssets = 0;
            decimal totalLiabilities = 0;
            //Getting Guarantor list
            var guarantorList = _sessionManager.GetGuarantorDetailsList();

            //Calculating Asset
            List<GuarantorAssetsNetWorthDTO> guarantorAssetList = new List<GuarantorAssetsNetWorthDTO>();
            List<GuarantorLiabilityDetailsDTO> guarantorLiabilityList = new List<GuarantorLiabilityDetailsDTO>();
            List<GuarantorNetWorthDetailsDTO> networthList = new List<GuarantorNetWorthDetailsDTO>();
            var guaAssetDetailsList = _sessionManager.GetGuarantorAssetList();

            if (guaAssetDetailsList != null)
            {
                foreach (var asset in guaAssetDetailsList)
                {
                    //Update GuarantorName in Assets if Guarantor name has been changed in Guarantor List by user.
                    asset.GuarantorDetailsDTO.PromoterMaster = guarantorList.FirstOrDefault(d => d.PromoterCode == asset.PromoterCode).PromoterMaster;

                    //Find if One Guarantor has multiple assets
                    GuarantorAssetsNetWorthDTO netWorth = guarantorAssetList.FirstOrDefault(x => x.PromoterCode == asset.PromoterCode);
                    if (netWorth != null)
                    {
                        netWorth.GuarAssetValue = (Convert.ToDecimal(netWorth.GuarAssetValue) + Convert.ToDecimal(asset.GuarAssetValue));
                        //netWorth.Networth = (Convert.ToDecimal(netWorth.Networth) + Convert.ToDecimal(asset.Value)).ToString();
                    }
                    else
                    {
                        GuarantorDetailsDTO guarantor = guarantorList.FirstOrDefault(d => d.PromoterCode == asset.PromoterCode);
                        guarantorAssetList.Add(new GuarantorAssetsNetWorthDTO
                        {

                            EnqGuarassetId = guarantorAssetList.Max(x => x.EnqGuarassetId) + 1 ?? 1,
                            GuarantorDetailsDTO = new GuarantorDetailsDTO()
                            {
                                PromoterCode = guarantor.PromoterCode,
                                GuarName = guarantor.GuarName,
                                PromoterMaster = guarantor.PromoterMaster
                            },
                            GuarAssetValue = asset.GuarAssetValue
                        });
                    }
                    totalAssets += Convert.ToDecimal(asset.GuarAssetValue);
                    GuarantorNetWorthDetailsDTO networtitem = new GuarantorNetWorthDetailsDTO()
                    {
                        GuarantorDetailsDTO = new GuarantorDetailsDTO()
                        {
                            PromoterMaster = guarantorList.FirstOrDefault(d => d.PromoterCode == asset.PromoterCode).PromoterMaster
                        }
                    };

                    if (networthList.Count == 0)
                    {
                        networtitem.GuarMov = asset.GuarAssetValue;
                        networtitem.PromoterCode = asset.PromoterCode.Value;
                        networthList.Add(networtitem);
                    }
                    else
                    {
                        var item = networthList.FirstOrDefault(x => x.PromoterCode == asset.PromoterCode);
                        if (item != null)
                        {
                            item.GuarMov = item.GuarMov + asset.GuarAssetValue;
                        }
                        else
                        {
                            networtitem.GuarMov = asset.GuarAssetValue;
                            networtitem.PromoterCode = asset.PromoterCode.Value;
                            networthList.Add(networtitem);
                        }
                    }
                }
                _sessionManager.SetGuarantorAssetList(guaAssetDetailsList);
            }
            //Calculating Liability
            var guaLiaDetailsList = _sessionManager.GetGuarantorLiabilityList();
            if (guaLiaDetailsList != null)
            {
                foreach (var guaLiability in guaLiaDetailsList)
                {
                    //Update GuarantorName in Liability if Guarantor name has been changed in Guarantor List by user.
                    if (guarantorList != null)
                        guaLiability.GuarantorDetailsDTO.PromoterMaster = guarantorList.FirstOrDefault(d => d.PromoterCode == guaLiability.PromoterCode).PromoterMaster;

                    //Find if One Guarantor has multiple Liability
                    GuarantorLiabilityDetailsDTO guarentorLiability = guarantorLiabilityList.FirstOrDefault(x => x.PromoterCode == guaLiability.PromoterCode);
                    if (guarentorLiability != null)
                    {
                        guarentorLiability.GuarLiabValue = (Convert.ToDecimal(guarentorLiability.GuarLiabValue) - Convert.ToDecimal(guaLiability.GuarLiabValue));

                    }
                    else
                    {
                        GuarantorDetailsDTO guarantorDetails = guarantorList.FirstOrDefault(d => d.PromoterCode == guaLiability.PromoterCode);
                        guarantorLiabilityList.Add(new GuarantorLiabilityDetailsDTO
                        {
                            EnqGuarliabId = guarantorAssetList.Max(x => x.EnqGuarassetId) + 1 ?? 1,
                            GuarantorDetailsDTO = new GuarantorDetailsDTO()
                            {
                                PromoterCode = guarantorDetails.PromoterCode,
                                GuarName = guarantorDetails.GuarName,
                            },
                            GuarLiabValue = guaLiability.GuarLiabValue,
                        });
                    }
                    totalLiabilities += Convert.ToDecimal(guaLiability.GuarLiabValue);
                    GuarantorNetWorthDetailsDTO networtitem = new GuarantorNetWorthDetailsDTO();
                    if (guaAssetDetailsList != null)
                    {
                        var assestDetail = guaAssetDetailsList.FirstOrDefault(x => x.PromoterCode == guaLiability.PromoterCode);
                        if (assestDetail != null)
                        {
                            var item = networthList.FirstOrDefault(x => x.PromoterCode == guaLiability.PromoterCode);
                            if (item != null)
                            {
                                if (item.GuarLiab != null)
                                    item.GuarLiab = item.GuarLiab + guaLiability.GuarLiabValue;
                                else
                                    item.GuarLiab = guaLiability.GuarLiabValue;
                                item.GuarNw = item.GuarMov - item.GuarLiab;
                            }
                            else
                            {
                                networtitem.GuarMov = assestDetail.GuarAssetValue;
                                networtitem.GuarNw = networtitem.GuarMov - guaLiability.GuarLiabValue;
                                networtitem.PromoterCode = guaLiability.PromoterCode;
                                networtitem.GuarLiab = guaLiability.GuarLiabValue;
                                networthList.Add(networtitem);
                            }
                        }


                    }
                }
                _sessionManager.SetGuarantorLiabilityList(guaLiaDetailsList);
            }

            _sessionManager.SetGuarantorNetWorthList(networthList);

            GuarantorAllDetailsDTO guarantorAllDetailsDTO = new()
            {
                ListGuarantor = guarantorList,
                GuarantorAssetLiabilityDetails = new GuarAssetLiabilityDetailsDTO()
                {
                    TotalAssets = totalAssets.ToString(),
                    ListLiabilityDetails = guaLiaDetailsList,
                    TotalNetworth = (totalAssets - totalLiabilities).ToString(),
                    TotalLiabilities = totalLiabilities.ToString(),
                    ListAssetDetails = guaAssetDetailsList,
                },
                GuarantorNetWorthList = networthList
            };

            return guarantorAllDetailsDTO;
        }
    }
}
