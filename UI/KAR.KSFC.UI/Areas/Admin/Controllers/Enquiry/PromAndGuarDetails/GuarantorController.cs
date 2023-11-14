using System;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.PromAndGuarDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class GuarantorController : Controller
    {
        private const string resultViewPath = "~/Areas/Admin/Views/Enquiry/PromAndGuarDetails/Guarantor/";
        private const string viewPath = "../../Areas/Admin/Views/Enquiry/PromAndGuarDetails/Guarantor/";
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;

        public GuarantorController(SessionManager sessionManager, ILogger logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }

        /// <summary>
        /// Get Method to View a guarantor details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecord with id " + id);

                ViewBag.ListDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                //ViewBag.ListTypeOfAccount = _sessionManager.GetDDListTypeOfAccount();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new GuarantorDetailsDTO());
                }
                else
                {
                    var guaDetailsList = _sessionManager.GetGuarantorDetailsList();
                    GuarantorDetailsDTO gua = guaDetailsList.Where(x => x.EnqGuarId == id).FirstOrDefault();
                    _logger.Information("Completed - ViewRecord with id " + id);

                    return View(resultViewPath + "ViewRecord.cshtml", gua);
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
        /// Get method to create or edit guarantor details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEdit(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEdit HttptGet with id " + id);

                ViewBag.ListDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEdit HttptGet with id " + id);

                    ViewBag.IsEdit = false;
                    return View(resultViewPath + "CreateOrEdit.cshtml", new GuarantorDetailsDTO());

                }
                else
                {
                    ViewBag.IsEdit = true;
                    var guaDetailsList = _sessionManager.GetGuarantorDetailsList();
                    GuarantorDetailsDTO gua = guaDetailsList.Where(x => x.EnqGuarId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEdit HttptGet with id " + id);

                    return View(resultViewPath + "CreateOrEdit.cshtml", gua);
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
        /// post method to create or edit guarantor details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gua"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, GuarantorDetailsDTO gua)
        {

            try
            {

                _logger.Information(string.Format("Started - CreateOrEdit HttpPost with id {0} GuarName :{1} EnqGuarcibil :{2} Pan :{3} pin code :{4} UniqueId :{5}"
                       , id, gua.GuarName, gua.EnqGuarcibil, gua.Pan, gua.pinCode, gua.UniqueId));

                List<GuarantorDetailsDTO> guaDetailsList = new();
                if (ModelState.IsValid)
                {
                    var domList = _sessionManager.GetDDListDomicileStatus();
                    if (_sessionManager.GetGuarantorDetailsList() != null)
                        guaDetailsList = _sessionManager.GetGuarantorDetailsList();
                    gua.DomicileMasterDTO = new DomicileMasterDTO
                    {
                        DomDets = domList.FirstOrDefault(x => x.Value == gua.DomCd.ToString()).Text
                    };
                    if (id == 0)
                    {
                        ViewBag.IsEdit = false;
                        gua.EnqGuarId = guaDetailsList.Max(x => x.EnqGuarId) + 1 ?? 1;
                        guaDetailsList.Add(gua);
                    }
                    else
                    {
                        ViewBag.IsEdit = true;
                        guaDetailsList.Remove(guaDetailsList.Find(m => m.EnqGuarId == id));
                        gua.EnqGuarId = id;
                        guaDetailsList.Add(gua);

                    }
                    _sessionManager.SetGuarantorDetailsList(guaDetailsList);
                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();

                    _logger.Information(string.Format("Completed - CreateOrEdit HttpPost with id {0} GuarName :{1} EnqGuarcibil :{2} Pan :{3} pin code :{4} UniqueId :{5}"
                       , id, gua.GuarName, gua.EnqGuarcibil, gua.Pan, gua.pinCode, gua.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }

                _logger.Information(string.Format("Completed - CreateOrEdit HttpPost with id {0} GuarName :{1} EnqGuarcibil :{2} Pan :{3} pin code :{4} UniqueId :{5}"
                      , id, gua.GuarName, gua.EnqGuarcibil, gua.Pan, gua.pinCode, gua.UniqueId));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", gua) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! CreateOrEdit HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        

        /// <summary>
        /// Post method to delete guarantor details
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
                    var guarAssetDetailsList = _sessionManager.GetGuarantorAssetList();
                    var guarLiaDetailsList = _sessionManager.GetGuarantorLiabilityList();

                    if (guarAssetDetailsList != null)
                    {
                        var ifGuaAssetExist = guarAssetDetailsList.Find(x => x.GuarantorDetailsDTO.EnqGuarId == id);
                        if (ifGuaAssetExist != null)
                        {
                            ViewBag.ErrorGuaAssLiaExist = "Please delete the guarantor assets and liabilities first in order to delete a guarantor.";
                            GuarantorAllDetailsDTO guarantorAllDetailsDTOExist = GuarantorAssetLiabilityNetWorth();
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
                            _logger.Information("Completed - Delete with id " + id);

                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTOExist) });
                        }
                    }

                    var guaDetailList = _sessionManager.GetGuarantorDetailsList();

                    var itemToRemove = guaDetailList.Find(r => r.EnqGuarId == id);
                    guaDetailList.Remove(itemToRemove);

                    _sessionManager.SetGuarantorDetailsList(guaDetailList);
                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();

                    _logger.Information("Completed - Delete with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Delete HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
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
                _logger.Information("Started - ViewRecordAsset with id " + id);

                ViewBag.GuarantorDetailsList = _sessionManager.GetGuarantorDetailsList();
                ViewBag.ListPromAndGuarAssetType = _sessionManager.GetDDListPromAndGuarAssetType();
                ViewBag.ListPromAndGuarAssetCategory = _sessionManager.GetDDListPromAndGuarAssetCategory();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecordAsset with id " + id);
                    return View(resultViewPath + "ViewRecordAsset.cshtml", new GuarantorAssetsNetWorthDTO());
                }
                else
                {
                    var guarAssetList = _sessionManager.GetGuarantorAssetList();
                    GuarantorAssetsNetWorthDTO guar = guarAssetList.Where(x => x.EnqGuarassetId == id).FirstOrDefault();

                    _logger.Information("Completed - ViewRecordAsset with id " + id);

                    return View(resultViewPath + "ViewRecordAsset.cshtml", guar);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ViewRecordAsset page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpGet]
        public IActionResult CheckGuarantorExit()
        {
            try
            {
                _logger.Information("Started - CheckGuarantorExit");

                var Gurantors = _sessionManager.GetGuarantorDetailsList();
                if (Gurantors != null)
                {
                    _logger.Information("Completed - CheckGuarantorExit");
                    return Json(new { IsValid = true, Message = "please add guarantor first" });
                }
                else
                {
                    _logger.Information("Completed - CheckGuarantorExit");
                    return Json(new { IsValid = false, Message = "please add guarantor first" });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! CheckGuarantorExit page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
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
                _logger.Information("Started - CreateOrEditAsset HttptGet with id " + id);

                ViewBag.GuarantorDetailsList = _sessionManager.GetGuarantorDetailsList();
                ViewBag.ListPromAndGuarAssetType = _sessionManager.GetDDListPromAndGuarAssetType();
                ViewBag.ListPromAndGuarAssetCategory = _sessionManager.GetDDListPromAndGuarAssetCategory();
                if (id == 0)
                {
                    ViewBag.IsEdit = false;
                    _logger.Information("Completed - CreateOrEditAsset HttptGet with id " + id);
                    return View(resultViewPath + "CreateOrEditAsset.cshtml", new GuarantorAssetsNetWorthDTO());
                }

                else
                {
                    ViewBag.IsEdit = true;
                    var guarAssetList = _sessionManager.GetGuarantorAssetList();
                    GuarantorAssetsNetWorthDTO guar = guarAssetList.Where(x => x.EnqGuarassetId == id).FirstOrDefault();
                    _logger.Information("Completed - CreateOrEditAsset HttptGet with id " + id);

                    return View(resultViewPath + "CreateOrEditAsset.cshtml", guar);
                }
            }
            catch (Exception ex) 
            {
                _logger.Error("Error occured! CreateOrEditAsset HttpGet page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
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
                _logger.Information(string.Format("Started - CreateOrEditAsset HttpPost with id {0} AssetcatCd :{1} AssettypeCd :{2} PromoterCode :{3} UniqueId :{4}"
                              , id, guarAsset.AssetcatCd, guarAsset.AssettypeCd, guarAsset.PromoterCode, guarAsset.UniqueId));

                List<GuarantorAssetsNetWorthDTO> guarAssetList = new();
                if (ModelState.IsValid)
                {
                    var guarantorList = _sessionManager.GetGuarantorDetailsList();
                    if (guarantorList != null)
                        guarAsset.GuarantorDetailsDTO.GuarName = guarantorList.Find(x => x.EnqGuarId == guarAsset.GuarantorDetailsDTO.EnqGuarId).GuarName;

                    guarAsset.AssetCategoryMasterDTO = new AssetCategoryMasterDTO
                    {
                        AssetcatDets = _sessionManager.GetDDListPromAndGuarAssetCategory().FirstOrDefault(x => x.Value == guarAsset.AssetcatCd.ToString()).Text

                    };
                    guarAsset.AssetTypeMasterDTO = new AssetTypeMasterDTO
                    {
                        AssettypeDets = _sessionManager.GetDDListPromAndGuarAssetType().FirstOrDefault(x => x.Value == guarAsset.AssetcatCd.ToString()).Text

                    };
                    if (id == 0)
                    {
                        ViewBag.IsEdit = false;
                        if (_sessionManager.GetGuarantorAssetList() != null)
                            guarAssetList = _sessionManager.GetGuarantorAssetList();
                        guarAsset.EnqGuarassetId = guarAssetList.Max(x => x.EnqGuarassetId) + 1 ?? 1; //Increment ID
                        guarAssetList.Add(guarAsset);
                    }
                    else
                    {
                        ViewBag.IsEdit = true;
                        guarAssetList = _sessionManager.GetGuarantorAssetList();
                        guarAssetList.Remove(guarAssetList.Find(m => m.EnqGuarassetId == id));
                        guarAsset.EnqGuarassetId = id;
                        guarAssetList.Add(guarAsset);
                    }

                    _sessionManager.SetGuarantorAssetList(guarAssetList);

                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();

                    _logger.Information(string.Format("Started - CreateOrEditAsset HttpPost with id {0} AssetcatCd :{1} AssettypeCd :{2} PromoterCode :{3} UniqueId :{4}"
                       , id, guarAsset.AssetcatCd, guarAsset.AssettypeCd, guarAsset.PromoterCode, guarAsset.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }
                _logger.Information(string.Format("Started - CreateOrEditAsset HttpPost with id {0} AssetcatCd :{1} AssettypeCd :{2} PromoterCode :{3} UniqueId :{4}"
                       , id, guarAsset.AssetcatCd, guarAsset.AssettypeCd, guarAsset.PromoterCode, guarAsset.UniqueId));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", guarAsset) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! CreateOrEditAsset HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
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
                _logger.Information("Started - DeleteAsset with id " + id);

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

                    _logger.Information("Completed - DeleteAsset with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! DeleteAsset HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
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
                _logger.Information("Started - ViewRecordLiability with id " + id);

                ViewBag.GuarantorDetailsList = _sessionManager.GetGuarantorDetailsList();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecordLiability with id " + id);
                    return View(resultViewPath + "ViewRecordLiability.cshtml", new GuarantorDetailsDTO());
                }
                else
                {
                    var guarLiaDetailList = _sessionManager.GetGuarantorLiabilityList();
                    GuarantorLiabilityDetailsDTO liability = guarLiaDetailList.Where(x => x.EnqGuarliabId == id).FirstOrDefault();

                    _logger.Information("Completed - ViewRecordLiability with id " + id);

                    return View(resultViewPath + "ViewRecordLiability.cshtml", liability);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ViewRecordLiability page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
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
                _logger.Information("Started - CreateOrEditLiability HttptGet with id " + id);

                ViewBag.GuarantorDetailsList = _sessionManager.GetGuarantorDetailsList();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEditLiability HttptGet with id " + id);

                    ViewBag.IsEdit = false;
                    return View(resultViewPath + "CreateOrEditLiability.cshtml", new GuarantorLiabilityDetailsDTO());
                }
                else
                {
                    ViewBag.IsEdit = true;
                    var guarLiaDetailList = _sessionManager.GetGuarantorLiabilityList();
                    GuarantorLiabilityDetailsDTO liability = guarLiaDetailList.Where(x => x.EnqGuarliabId == id).FirstOrDefault();

                    _logger.Information("Completed - CreateOrEditLiability HttptGet with id " + id);

                    return View(resultViewPath + "CreateOrEditLiability.cshtml", liability);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! CreateOrEditLiability HttpGet page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
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
                _logger.Information(string.Format("Started - CreateOrEditLiability HttpPost with id {0} EnqGuarId :{1} GuarLiabDesc :{2} GuarLiabValue :{3} PromoterCode :{4} UniqueId :{5}"
                             , id, liability.EnqGuarId, liability.GuarLiabDesc, liability.GuarLiabValue, liability.PromoterCode, liability.UniqueId));

                List<GuarantorLiabilityDetailsDTO> guarLiaDetailsList = new();

                if (ModelState.IsValid)
                {
                    var guarantorList = _sessionManager.GetGuarantorDetailsList();
                    if (guarantorList != null)
                    {
                        liability.GuarantorDetailsDTO.GuarName = guarantorList.Find(x => x.EnqGuarId == liability.GuarantorDetailsDTO.EnqGuarId).GuarName;
                    }
                    if (id == 0)
                    {
                        ViewBag.IsEdit = false;
                        if (_sessionManager.GetGuarantorLiabilityList() != null)
                            guarLiaDetailsList = _sessionManager.GetGuarantorLiabilityList();
                        liability.EnqGuarliabId = guarLiaDetailsList.Max(x => x.EnqGuarliabId) + 1 ?? 1;
                        guarLiaDetailsList.Add(liability);
                    }
                    else
                    {
                        ViewBag.IsEdit = true;
                        guarLiaDetailsList = _sessionManager.GetGuarantorLiabilityList();
                        guarLiaDetailsList.Remove(guarLiaDetailsList.Find(m => m.EnqGuarliabId == id));
                        liability.EnqGuarliabId = id;
                        guarLiaDetailsList.Add(liability);
                    }

                    _sessionManager.SetGuarantorLiabilityList(guarLiaDetailsList);

                    GuarantorAllDetailsDTO guarantorAllDetailsDTO = GuarantorAssetLiabilityNetWorth();

                    _logger.Information(string.Format("Completed - CreateOrEditLiability HttpPost with id {0} EnqGuarId :{1} GuarLiabDesc :{2} GuarLiabValue :{3} PromoterCode :{4} UniqueId :{5}"
                         , id, liability.EnqGuarId, liability.GuarLiabDesc, liability.GuarLiabValue, liability.PromoterCode, liability.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }
                _logger.Information(string.Format("Completed - CreateOrEditLiability HttpPost with id {0} EnqGuarId :{1} GuarLiabDesc :{2} GuarLiabValue :{3} PromoterCode :{4} UniqueId :{5}"
                         , id, liability.EnqGuarId, liability.GuarLiabDesc, liability.GuarLiabValue, liability.PromoterCode, liability.UniqueId));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", liability) });
            }
            catch (Exception ex)
            {

                _logger.Error("Error occured! CreateOrEditLiability HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
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
                _logger.Information("Started - DeleteLiability with id " + id);

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

                    _logger.Information("Completed - DeleteLiability with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", guarantorAllDetailsDTO) });
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! DeleteLiability HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Calculates real time Asset, Liability and NetWorth from Session values
        /// </summary>
        /// <returns></returns>
        public GuarantorAllDetailsDTO GuarantorAssetLiabilityNetWorth()
        {
           
            try
            {
                _logger.Information("Started - Calculating real time Asset, Liability and NetWorth from Session values");

                //ModelState.Clear();
                decimal totalAssets = 0;
                decimal totalLiabilities = 0;
                //Getting Guarantor list
                var guarantorList = _sessionManager.GetGuarantorDetailsList();

                //Calculating Asset
                List<GuarantorAssetsNetWorthDTO> guarantorAssetList = new List<GuarantorAssetsNetWorthDTO>();
                List<GuarantorLiabilityDetailsDTO> guarantorLiabilityList = new List<GuarantorLiabilityDetailsDTO>();
                var guaAssetDetailsList = _sessionManager.GetGuarantorAssetList();

                if (guaAssetDetailsList != null)
                {
                    foreach (var asset in guaAssetDetailsList)
                    {
                        //Update GuarantorName in Assets if Guarantor name has been changed in Guarantor List by user.
                        asset.GuarantorDetailsDTO.GuarName = guarantorList.Find(d => d.GuarName == asset.GuarantorDetailsDTO.GuarName).GuarName;

                        //Find if One Guarantor has multiple assets
                        GuarantorAssetsNetWorthDTO netWorth = guarantorAssetList.Find(x => x.GuarantorDetailsDTO.GuarName == asset.GuarantorDetailsDTO.GuarName);
                        if (netWorth != null)
                        {
                            netWorth.GuarAssetValue = (Convert.ToDecimal(netWorth.GuarAssetValue) + Convert.ToDecimal(asset.GuarAssetValue));
                            //netWorth.Networth = (Convert.ToDecimal(netWorth.Networth) + Convert.ToDecimal(asset.Value)).ToString();
                        }
                        else
                        {
                            GuarantorDetailsDTO guarantor = guarantorList.Find(d => d.GuarName == asset.GuarantorDetailsDTO.GuarName);
                            guarantorAssetList.Add(new GuarantorAssetsNetWorthDTO
                            {

                                EnqGuarassetId = guarantorAssetList.Max(x => x.EnqGuarassetId) + 1 ?? 1,
                                GuarantorDetailsDTO = new GuarantorDetailsDTO()
                                {
                                    PromoterCode = guarantor.PromoterCode,
                                    GuarName = guarantor.GuarName,
                                },
                                GuarAssetValue = asset.GuarAssetValue
                            });
                        }
                        totalAssets += Convert.ToDecimal(asset.GuarAssetValue);
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
                            guaLiability.GuarantorDetailsDTO.GuarName = guarantorList.Find(d => d.GuarName == guaLiability.GuarantorDetailsDTO.GuarName).GuarName;

                        //Find if One Guarantor has multiple Liability
                        GuarantorLiabilityDetailsDTO guarentorLiability = guarantorLiabilityList.Find(x => x.PromoterCode == guaLiability.PromoterCode);
                        if (guarentorLiability != null)
                        {
                            guarentorLiability.GuarLiabValue = (Convert.ToDecimal(guarentorLiability.GuarLiabValue) - Convert.ToDecimal(guaLiability.GuarLiabValue));

                        }
                        else
                        {
                            GuarantorDetailsDTO guarantorDetails = guarantorList.Find(d => d.PromoterCode == guaLiability.PromoterCode);
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
                    }
                    _sessionManager.SetGuarantorLiabilityList(guaLiaDetailsList);
                }
                // ViewBag.NetWorthList = netWorths; // need to send to list need to merge

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
                    }
                };

                _logger.Information(String.Format("Completed - Calculating real time Asset, Liability and NetWorth from Session values with" +
                    " Guarantors :{0} TotalAssets: {1} TotalNetworth: {2} TotalLiabilities :{3}",
                    guarantorAllDetailsDTO.ListGuarantor.Count, guarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.TotalAssets, guarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.TotalNetworth
                    , guarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.TotalLiabilities));

                return guarantorAllDetailsDTO;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Calculating real time Asset . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return default;
            }
        }
    }
}
