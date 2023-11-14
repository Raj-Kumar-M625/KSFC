using System;
using System.Collections.Generic;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.UI.Utility;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
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
    public class PromoterController : Controller
    {
        private const string resultViewPath = "~/Areas/Admin/Views/Enquiry/PromAndGuarDetails/Promoter/";
        private const string viewPath = "../../Areas/Admin/Views/Enquiry/PromAndGuarDetails/Promoter/";
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;

        public PromoterController(SessionManager sessionManager, ILogger logger)
        {
            _sessionManager = sessionManager;
            _logger = logger;
        }

        /// <summary>
        /// Get method to View a Promoter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecord with id " + id);

                ViewBag.ListPromoterDesignation = _sessionManager.GetDDListPromoterDesignation();
                ViewBag.ListDomicileStatus = _sessionManager.GetDDListDomicileStatus();

                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecord with id " + id);
                    return View(resultViewPath + "ViewRecord.cshtml", new PromoterDetailsDTO());
                }
                else
                {
                    var proDetailList = _sessionManager.GetPromoterDetailsList();
                    PromoterDetailsDTO pro = proDetailList.Where(x => x.EnqPromId == id).FirstOrDefault();
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
        /// Get method to Create or Edit a Promoter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEdit(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEdit HttptGet with id " + id);

                ViewBag.ListPromoterDesignation = _sessionManager.GetDDListPromoterDesignation();
                ViewBag.ListDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEdit HttptGet with id " + id);
                    ViewBag.IsEdit = false;
                    return View(resultViewPath + "CreateOrEdit.cshtml", new PromoterDetailsDTO());
                }

                else
                {
                    ViewBag.IsEdit = true;
                    var proDetailList = _sessionManager.GetPromoterDetailsList();
                    PromoterDetailsDTO pro = proDetailList.Where(x => x.EnqPromId == id).FirstOrDefault();
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
        /// Post method to Create or Edit Promoter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pro"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEdit(int id, PromoterDetailsDTO pro)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEdit HttpPost with id {0} PromName :{1} PanNo :{2} PromEmail :{3} PromMobile :{4} UniqueId :{5}"
                              , id, pro.PromName, pro.PanNo, pro.PromEmail, pro.PromMobile, pro.UniqueId));

                List<PromoterDetailsDTO> proDetailList = new();
                if (ModelState.IsValid)
                {

                    if (pro.PdesigCd != 0)
                    {
                        var desgList = _sessionManager.GetDDListPromoterDesignation();
                        pro.PromDesg = desgList.FirstOrDefault(x => x.Value == pro.PdesigCd.ToString()).Text;
                        pro.PdesigCdNavigation = new PromDesignationMasterDTO();
                        pro.PdesigCdNavigation.PdesigDets = desgList.FirstOrDefault(x => x.Value == pro.PdesigCd.ToString()).Text;
                    }
                    if (pro.DomCd != 0)
                    {
                        var domList = _sessionManager.GetDDListDomicileStatus();
                        pro.PromoDomText = domList.FirstOrDefault(x => x.Value == pro.DomCd.ToString()).Text;
                    }
                    if (id == 0)
                    {
                        ViewBag.IsEdit = false;
                        if (_sessionManager.GetPromoterDetailsList() != null)
                            proDetailList = _sessionManager.GetPromoterDetailsList();

                        pro.EnqPromId = proDetailList.Max(x => x.EnqPromId) + 1 ?? 1;
                        pro.PromoterCode = proDetailList.Max(x => x.EnqPromId) + 1 ?? 1;
                        pro.PromoterMaster.PromoterCode = proDetailList.Max(x => x.EnqPromId) + 1 ?? 1;


                        if (pro.PromoterMaster.PromoterDob != null)
                            pro.PromoterMaster.Age = (DateTime.Now.Year - (pro.PromoterMaster.PromoterDob?.Year)).ToString();
                        proDetailList.Add(pro);

                    }
                    else
                    {
                        ViewBag.IsEdit = true;
                        proDetailList = _sessionManager.GetPromoterDetailsList();
                        proDetailList.Remove(proDetailList.Find(m => m.EnqPromId == id));
                        pro.EnqPromId = id;
                        pro.PromoterMaster.Age = (DateTime.Now.Year - (pro.PromoterMaster.PromoterDob?.Year)).ToString();
                        proDetailList.Add(pro);

                    }
                    _sessionManager.SetPromoterDetailsList(proDetailList);
                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();

                    _logger.Information(string.Format("Completed - CreateOrEdit HttpPost with id {0} PromName :{1} PanNo :{2} PromEmail :{3} PromMobile :{4} UniqueId :{5}"
                      , id, pro.PromName, pro.PanNo, pro.PromEmail, pro.PromMobile, pro.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
                }

                _logger.Information(string.Format("Completed - CreateOrEdit HttpPost with id {0} PromName :{1} PanNo :{2} PromEmail :{3} PromMobile :{4} UniqueId :{5}"
                     , id, pro.PromName, pro.PanNo, pro.PromEmail, pro.PromMobile, pro.UniqueId));

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
        /// Post method to delete a Promoter
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
                    var proAssetDetailsList = _sessionManager.GetPromoterAssetList();
                    var proLiaDetailsList = _sessionManager.GetPromoterLiabilityList();


                    if (proAssetDetailsList != null)
                    {
                        var ifProAssetExist = proAssetDetailsList.Find(x => x.EnqPromassetId == id);
                        if (ifProAssetExist != null)
                        {
                            ViewBag.ErrorProAssLiaExist = "Please delete the promoter assets and liabilities first in order to delete a promoter.";
                            PromoterAllDetailsDTO promoterAllDetailsDTOExist = PromoterAssetLiabilityNetWorth();

                            _logger.Information("Completed - Delete with id " + id);

                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTOExist) });
                        }
                    }
                    if (proLiaDetailsList != null)
                    {
                        var ifProLiabilityExist = proLiaDetailsList.Find(x => x.EnqPromliabId == id);
                        if (ifProLiabilityExist != null)
                        {
                            ViewBag.ErrorProAssLiaExist = "Please delete the promoter assets and liabilities first in order to delete a promoter.";
                            PromoterAllDetailsDTO promoterAllDetailsDTOExist = PromoterAssetLiabilityNetWorth();

                            _logger.Information("Completed - Delete with id " + id);

                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTOExist) });
                        }
                    }

                    var proDetailList = _sessionManager.GetPromoterDetailsList();

                    var itemToRemove = proDetailList.Find(r => r.EnqPromId == id);
                    proDetailList.Remove(itemToRemove);

                    _sessionManager.SetPromoterDetailsList(proDetailList);
                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();

                    _logger.Information("Completed - Delete with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
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
        /// Get method to view Promoter asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecordAsset(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecordAsset with id " + id);

                ViewBag.PromoterDetailsList = _sessionManager.GetPromoterDetailsList();
                ViewBag.ListPromAndGuarAssetType = _sessionManager.GetDDListPromAndGuarAssetType();
                ViewBag.ListPromAndGuarAssetCategory = _sessionManager.GetDDListPromAndGuarAssetCategory();
                ViewBag.ListModeOfAcquire = _sessionManager.GetDDListModeOfAcquire();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecordAsset with id " + id);
                    return View(resultViewPath + "ViewRecordAsset.cshtml", new PromoterAssetsNetWorthDTO());
                }
                else
                {
                    var proAssetList = _sessionManager.GetPromoterAssetList();// JsonConvert.DeserializeObject<List<AssetDetails>>(HttpContext.Session.GetString("PromoterAssetList"));
                    PromoterAssetsNetWorthDTO pro = proAssetList.Where(x => x.EnqPromassetId == id).FirstOrDefault();

                    _logger.Information("Completed - ViewRecordAsset with id " + id);

                    return View(resultViewPath + "ViewRecordAsset.cshtml", pro);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ViewRecordAsset page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        /// <summary>
        /// Get method to create or edit Promoter asset
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEditAsset(int id = 0)
        {
            try
            {
                _logger.Information("Started - CreateOrEditAsset HttptGet with id " + id);

                ViewBag.PromoterDetailsList = _sessionManager.GetPromoterDetailsList();
                ViewBag.ListPromAndGuarAssetType = _sessionManager.GetDDListPromAndGuarAssetType();
                ViewBag.ListPromAndGuarAssetCategory = _sessionManager.GetDDListPromAndGuarAssetCategory();
                ViewBag.ListModeOfAcquire = _sessionManager.GetDDListModeOfAcquire();

                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEditAsset HttptGet with id " + id);
                    ViewBag.IsEdit = false;
                    return View(resultViewPath + "CreateOrEditAsset.cshtml", new PromoterAssetsNetWorthDTO());
                }
                else
                {
                    ViewBag.IsEdit = true;
                    var proAssetList = _sessionManager.GetPromoterAssetList();
                    PromoterAssetsNetWorthDTO pro = proAssetList.Where(x => x.EnqPromassetId == id).FirstOrDefault();

                    _logger.Information("Completed - CreateOrEditAsset HttptGet with id " + id);

                    return View(resultViewPath + "CreateOrEditAsset.cshtml", pro);
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
        /// post method to create or edit Promoter asset
        /// </summary>
        /// <param name="id"></param>
        /// <param name="proAsset"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEditAsset(int id, PromoterAssetsNetWorthDTO proAsset)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEditAsset HttpPost with id {0} AssetcatCd :{1} AssettypeCd :{2} PromoterCode :{3} UniqueId :{4}"
                                    , id, proAsset.AssetcatCd, proAsset.AssettypeCd, proAsset.PromoterCode, proAsset.UniqueId));

                List<PromoterAssetsNetWorthDTO> proAssetList = new();
                if (ModelState.IsValid)
                {
                    var promoterList = _sessionManager.GetPromoterDetailsList();
                    var catList = _sessionManager.GetDDListPromAndGuarAssetCategory();
                    var typelist = _sessionManager.GetDDListPromAndGuarAssetType();
                    proAsset.AssetCategoryMasterDTO = new AssetCategoryMasterDTO()
                    {
                        AssetcatCd = proAsset.AssetcatCd,
                        AssetcatDets = catList.FirstOrDefault(x => x.Value == proAsset.AssetcatCd.ToString()).Text
                    };
                    proAsset.AssetTypeMasterDTO = new AssetTypeMasterDTO()
                    {
                        AssettypeCd = proAsset.AssettypeCd,
                        AssettypeDets = typelist.FirstOrDefault(x => x.Value == proAsset.AssettypeCd.ToString()).Text
                    };
                    if (id == 0)
                    {
                        if (_sessionManager.GetPromoterAssetList() != null)
                            proAssetList = _sessionManager.GetPromoterAssetList();
                        ViewBag.IsEdit = false;
                        proAsset.EnqPromassetId = proAssetList.Max(x => x.EnqPromassetId) + 1 ?? 1; //Increment ID
                        proAsset.PromoterMasterDTO = promoterList.Find(x => x.PromoterCode == proAsset.PromoterCode).PromoterMaster;
                        proAssetList.Add(proAsset);
                    }
                    else
                    {
                        ViewBag.IsEdit = true;
                        proAssetList = _sessionManager.GetPromoterAssetList();
                        proAssetList.Remove(proAssetList.Find(m => m.PromoterCode == id));
                        proAsset.PromoterMasterDTO = promoterList.Find(x => x.PromoterCode == proAsset.PromoterCode).PromoterMaster;
                        proAssetList.Add(proAsset);
                    }
                    _sessionManager.SetPromoterAssetList(proAssetList);

                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();

                    _logger.Information(string.Format("Completed - CreateOrEditAsset HttpPost with id {0} AssetcatCd :{1} AssettypeCd :{2} PromoterCode :{3} UniqueId :{4}"
                                        , id, proAsset.AssetcatCd, proAsset.AssettypeCd, proAsset.PromoterCode, proAsset.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
                }
                _logger.Information(string.Format("Completed - CreateOrEditAsset HttpPost with id {0} AssetcatCd :{1} AssettypeCd :{2} PromoterCode :{3} UniqueId :{4}"
                                        , id, proAsset.AssetcatCd, proAsset.AssettypeCd, proAsset.PromoterCode, proAsset.UniqueId));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "CreateOrEdit", proAsset) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! CreateOrEditAsset HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        /// <summary>
        /// Post method to create or edit Promoter asset
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
                    var proAssetList = _sessionManager.GetPromoterAssetList();// JsonConvert.DeserializeObject<List<AssetDetails>>(HttpContext.Session.GetString("PromoterAssetList"));

                    var itemToRemove = proAssetList.Find(r => r.EnqPromassetId == id);
                    proAssetList.Remove(itemToRemove);
                    _sessionManager.SetPromoterAssetList(proAssetList);

                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();

                    _logger.Information("Completed - DeleteAsset with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
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
        /// Get method to View Promoter liability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult ViewRecordLiability(int id = 0)
        {
            try
            {
                _logger.Information("Started - ViewRecordLiability with id " + id);

                ViewBag.PromoterDetailsList = _sessionManager.GetPromoterDetailsList();
                if (id == 0)
                {
                    _logger.Information("Completed - ViewRecordLiability with id " + id);
                    return View(resultViewPath + "ViewRecordLiability.cshtml", new PromoterLiabilityDetailsDTO());
                }
                else
                {
                    var proLiaDetailList = _sessionManager.GetPromoterLiabilityList();
                    PromoterLiabilityDetailsDTO liability = proLiaDetailList.Where(x => x.EnqPromliabId == id).FirstOrDefault();
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
        /// Get method to create or edit Promoter liability
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CreateOrEditLiability(int id = 0)
        {
            try
            {

                _logger.Information("Started - CreateOrEditLiability HttptGet with id " + id);

                ViewBag.PromoterDetailsList = _sessionManager.GetPromoterDetailsList();
                if (id == 0)
                {
                    _logger.Information("Completed - CreateOrEditLiability HttptGet with id " + id);

                    ViewBag.IsEdit = false;
                    return View(resultViewPath + "CreateOrEditLiability.cshtml", new PromoterLiabilityDetailsDTO());
                }

                else
                {
                    ViewBag.IsEdit = true;
                    var proLiaDetailList = _sessionManager.GetPromoterLiabilityList();
                    PromoterLiabilityDetailsDTO liability = proLiaDetailList.Where(x => x.EnqPromliabId == id).FirstOrDefault();
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
        /// Post method to create or edit Promoter liability
        /// </summary>
        /// <param name="id"></param>
        /// <param name="liability"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOrEditLiability(int id, PromoterLiabilityDetailsDTO liability)
        {
            try
            {
                _logger.Information(string.Format("Started - CreateOrEditLiability HttpPost with id {0} EnqPromId :{1} EnqLiabDesc :{2} EnqLiabValue :{3} PromoterCode :{4} UniqueId :{5}"
                                     , id, liability.EnqPromId, liability.EnqLiabDesc, liability.EnqLiabValue, liability.PromoterCode, liability.UniqueId));

                List<PromoterLiabilityDetailsDTO> proLiaDetailsList = new();

                if (ModelState.IsValid)
                {
                    var promoterList = _sessionManager.GetPromoterDetailsList();
                    if (id == 0)
                    {
                        ViewBag.IsEdit = false;
                        if (_sessionManager.GetPromoterLiabilityList() != null)
                            proLiaDetailsList = _sessionManager.GetPromoterLiabilityList();

                        liability.EnqPromliabId = proLiaDetailsList.Max(x => x.EnqPromliabId) + 1 ?? 1;
                        liability.PromoterMasterDTO = promoterList.Find(x => x.PromoterCode == liability.PromoterCode).PromoterMaster;
                        proLiaDetailsList.Add(liability);

                    }
                    else
                    {
                        ViewBag.IsEdit = true;
                        proLiaDetailsList = _sessionManager.GetPromoterLiabilityList();
                        proLiaDetailsList.Remove(proLiaDetailsList.Find(m => m.EnqPromliabId == id));
                        liability.EnqPromliabId = id;
                        liability.PromoterMasterDTO = promoterList.Find(x => x.PromoterCode == liability.PromoterCode).PromoterMaster;
                        proLiaDetailsList.Add(liability);
                    }
                    _sessionManager.SetPromoterLiabilityList(proLiaDetailsList);

                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();
                    _logger.Information(string.Format("Completed - CreateOrEditLiability HttpPost with id {0} EnqPromId :{1} EnqLiabDesc :{2} EnqLiabValue :{3} PromoterCode :{4} UniqueId :{5}"
                             , id, liability.EnqPromId, liability.EnqLiabDesc, liability.EnqLiabValue, liability.PromoterCode, liability.UniqueId));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
                }
                _logger.Information(string.Format("Completed - CreateOrEditLiability HttpPost with id {0} EnqPromId :{1} EnqLiabDesc :{2} EnqLiabValue :{3} PromoterCode :{4} UniqueId :{5}"
                             , id, liability.EnqPromId, liability.EnqLiabDesc, liability.EnqLiabValue, liability.PromoterCode, liability.UniqueId));

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
        /// Post method to delete a Promoter liability
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
                    var proLiaDetailsList = _sessionManager.GetPromoterLiabilityList();

                    var itemToRemove = proLiaDetailsList.Find(r => r.EnqPromliabId == id);
                    proLiaDetailsList.Remove(itemToRemove);
                    _sessionManager.SetPromoterLiabilityList(proLiaDetailsList);
                    PromoterAllDetailsDTO promoterAllDetailsDTO = PromoterAssetLiabilityNetWorth();
                    _logger.Information("Completed - DeleteLiability with id " + id);

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", promoterAllDetailsDTO) });
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
        public PromoterAllDetailsDTO PromoterAssetLiabilityNetWorth()
        {
            try
            {
                _logger.Information("Started - Calculating real time Asset, Liability and NetWorth from Session values");

                //ModelState.Clear();
                decimal totalAssets = 0;
                decimal totalLiabilities = 0;
                //Getting promoter list
                var promoterList = _sessionManager.GetPromoterDetailsList();

                //Calculating Asset
                List<PromoterAssetsNetWorthDTO> promoterAssetList = new List<PromoterAssetsNetWorthDTO>();
                List<PromoterLiabilityDetailsDTO> PromoterLiabilityList = new List<PromoterLiabilityDetailsDTO>();

                var proAssetDetailsList = _sessionManager.GetPromoterAssetList();
                if (proAssetDetailsList != null)
                {
                    foreach (var asset in proAssetDetailsList)
                    {
                        //Update PromoterName in Assets if promoter name has been changed in Promoter List by user.
                        asset.PromoterMasterDTO = promoterList.Find(d => d.PromoterCode == asset.PromoterCode).PromoterMaster;

                        //Find if One Promoter has multiple assets
                        PromoterAssetsNetWorthDTO assetDto = promoterAssetList.Find(x => x.PromoterCode == asset.PromoterCode);
                        if (assetDto != null)
                        {
                            assetDto.EnqAssetValue = (Convert.ToDecimal(assetDto.EnqAssetValue) + Convert.ToDecimal(assetDto.EnqAssetValue));
                            //netWorth. = (Convert.ToDecimal(netWorth.Networth) + Convert.ToDecimal(asset.Value)).ToString();
                        }
                        else
                        {
                            PromoterDetailsDTO promoter = promoterList.Find(d => d.PromoterCode == asset.PromoterCode);
                            promoterAssetList.Add(new PromoterAssetsNetWorthDTO
                            {

                                EnqPromassetId = promoterAssetList.Max(x => x.EnqPromassetId) + 1 ?? 1,
                                PromoterMasterDTO = promoter.PromoterMaster,
                                EnqAssetValue = asset.EnqAssetValue,
                            });
                        }
                        totalAssets += Convert.ToDecimal(asset.EnqAssetValue);
                    }
                    _sessionManager.SetPromoterAssetList(proAssetDetailsList);
                }
                //Calculating Liability
                var proLiaDetailsList = _sessionManager.GetPromoterLiabilityList();
                if (proLiaDetailsList != null)
                {
                    foreach (var proLiability in proLiaDetailsList)
                    {
                        //Update PromoterName in Liability if promoter name has been changed in Promoter List by user.
                        proLiability.PromoterMasterDTO = promoterList.Find(d => d.PromoterCode == proLiability.PromoterCode).PromoterMaster;

                        //Find if One Promoter has multiple Liability
                        PromoterLiabilityDetailsDTO promoterLiability = PromoterLiabilityList.Find(x => x.PromCode == proLiability.PromCode);
                        if (promoterLiability != null)
                        {
                            promoterLiability.EnqLiabValue = (Convert.ToDecimal(promoterLiability.EnqLiabValue) - Convert.ToDecimal(proLiability.EnqLiabValue));
                            //promoterLiability.Networth = (Convert.ToDecimal(promoterLiability.Networth) - Convert.ToDecimal(proLiability.Value)).ToString();
                        }
                        else
                        {
                            PromoterDetailsDTO promoter = promoterList.Find(d => d.PromoterCode == proLiability.PromoterCode);
                            PromoterLiabilityList.Add(new PromoterLiabilityDetailsDTO
                            {
                                EnqPromliabId = PromoterLiabilityList.Max(x => x.EnqPromliabId) + 1 ?? 1,
                                PromoterMasterDTO = promoter.PromoterMaster,
                                EnqLiabValue = proLiability.EnqLiabValue,
                            });
                        }
                        totalLiabilities += Convert.ToDecimal(proLiability.EnqLiabValue);
                    }
                    _sessionManager.SetPromoterLiabilityList(proLiaDetailsList);
                }
                //ViewBag.NetWorthList = promoterAssetList;  need to send to list need to merge
                PromoterAllDetailsDTO promoterAllDetailsDTO = new()
                {
                    ListPromoters = promoterList,
                    PromotersAssetLiabilityDetails = new PromAssetLiabilityDetailsDTO()
                    {
                        TotalAssets = totalAssets.ToString(),
                        ListLiabilityDetails = proLiaDetailsList,
                        TotalNetworth = (totalAssets - totalLiabilities).ToString(),
                        TotalLiabilities = totalLiabilities.ToString(),
                        ListAssetDetails = proAssetDetailsList,

                    }
                };
                //if(promoterAllDetailsDTO.PromotersAssetLiabilityDetails.ListLiabilityDetails!=null)
                //{
                //    foreach (var item in promoterAllDetailsDTO.PromotersAssetLiabilityDetails.ListLiabilityDetails)
                //    {
                //        item.pro
                //    }
                //}

                _logger.Information(String.Format("Completed - Calculating real time Asset, Liability and NetWorth from Session values with" +
                   " Guarantors :{0} TotalAssets: {1} TotalNetworth: {2} TotalLiabilities :{3}",
                   promoterAllDetailsDTO.ListPromoters.Count, promoterAllDetailsDTO.PromotersAssetLiabilityDetails.TotalAssets, promoterAllDetailsDTO.PromotersAssetLiabilityDetails.TotalNetworth
                   , promoterAllDetailsDTO.PromotersAssetLiabilityDetails.TotalLiabilities));


                return promoterAllDetailsDTO;
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

