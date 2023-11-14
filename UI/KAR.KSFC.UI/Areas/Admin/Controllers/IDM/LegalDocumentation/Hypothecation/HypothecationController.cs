using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ILegalDocumentationService;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.LegalDocumentation.Hypothecation
{
    /// <summary>
    ///  Author: Gagana K; Module: Hypothecation; Date:21/07/2022
    /// </summary>
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class HypothecationController : Controller
    {
        private readonly ILegalDocumentationService _legalDocumentationService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
       

        public HypothecationController(ILegalDocumentationService legalDocumentationService, ILogger logger, SessionManager sessionManager)
        {
            _legalDocumentationService = legalDocumentationService;
            _logger = logger;
            _sessionManager = sessionManager;
        }

        public IActionResult ViewRecord(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + unqid);
                var AllHypothecationList = _sessionManager.GetAllHypothecationList();
                HypoAssetDetailDTO HypothecationList = AllHypothecationList.FirstOrDefault(x => x.UniqueId == unqid);
                var AssetTypeDDL = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionAssetType));
                ViewBag.AssetType = AssetTypeDDL;

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == HypothecationList.IdmHypothDetId && x.SubModuleType == Constants.Hypothecation && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.Documentlist = doc;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + unqid);
                return View(Constants.hyporesultViewPath + Constants.ViewRecord, HypothecationList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult ViewCreate(int unqid)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + unqid);
             
                var AllHypothecationList = _sessionManager.GetAllAssetRefList();

                AssetRefnoDetailsDTO HypothecationList = AllHypothecationList.Find(x => x.AssetCd == unqid);
                var AssetTypeDDL = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionAssetType));
                ViewBag.AssetType = AssetTypeDDL;
             
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + unqid);
                return View(Constants.hyporesultViewPath + Constants.ViewCreate, HypothecationList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        public IActionResult Delete(string Id)
        {
            try
            {
                IEnumerable<HypoAssetDetailDTO> activeHypoList = new List<HypoAssetDetailDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, Id));
                if (Id == null)
                {
                    return NotFound();
                }
                else
                {
                    var hypotheList = JsonConvert.DeserializeObject<List<HypoAssetDetailDTO>>(HttpContext.Session.GetString(Constants.sessionHypothecation));
                    var itemToRemove = hypotheList.Find(r => r.UniqueId == Id);
                    itemToRemove.IsActive = false;
                    itemToRemove.IsDeleted = true;
                    itemToRemove.Action = (int)Constant.Delete;
                    hypotheList.Remove(itemToRemove);
                   
                    _sessionManager.SetHypothecationList(hypotheList);
                    if (hypotheList.Count !=0)
                    {
                        activeHypoList = hypotheList.Where(x => x.IsActive == true && x.IsDeleted==false).ToList();
                    }
                   
                    ViewBag.AccountNumber = itemToRemove.LoanAcc;
                    ViewBag.LoanSub = itemToRemove.LoanSub;
                    ViewBag.OffCd = itemToRemove.OffcCd;
                    _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, Id));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.hypoviewPath +  Constants.ViewAll, activeHypoList) });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(long AccountNumber, byte OffCd, int LoanSub ,int IdmHypothDetId)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
               
                var AllHypothecationList = _sessionManager.GetAllHypothecationList();
                ViewBag.AssetRefList = AllHypothecationList;

                ViewBag.AccountNumber = AccountNumber;
                //ViewBag.IdmHypothDetId = IdmHypothDetId;

                ViewBag.LoanSub = LoanSub;
                ViewBag.OffCd = OffCd;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.hyporesultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(List<int>id, HypoAssetDetailDTO hypotheDeedHolder, IFormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  
                    List<HypoAssetDetailDTO> hypotheDeedlist = new();
                    if (_sessionManager.GetAllHypothecationList() != null)
                        hypotheDeedlist = _sessionManager.GetAllHypothecationList();

                    foreach (var item in id)
                    {
                       
                        HypoAssetDetailDTO assetexist  = hypotheDeedlist.Find(x => x.IdmHypothDetId == item);
                       
                        if (assetexist != null)
                        {
                            hypotheDeedlist.Remove(assetexist);

                            var newHold = new HypoAssetDetailDTO()
                            {
                                IdmHypothDetId = item,
                                AssetCd = assetexist.AssetCd,
                                LoanSub = assetexist.LoanSub,
                                OffcCd = assetexist.OffcCd,                               
                                AssetValue = assetexist.AssetValue,
                                HypothValue = hypotheDeedHolder.HypothValue,
                                LoanAcc = assetexist.LoanAcc,
                                HypothNo = hypotheDeedHolder.HypothNo,
                                HypothDesc = hypotheDeedHolder.HypothDesc,
                                ExecutionDate = hypotheDeedHolder.ExecutionDate,
                                AssetDet = assetexist.AssetDet,
                                AssetName = assetexist.AssetName,
                                Action = (int)Constant.Create,
                                UniqueId = Guid.NewGuid().ToString(),
                                IsActive = true,
                                IsDeleted = false,

                            };
                            hypotheDeedlist.Add(newHold);
                        
                        }   
                           
                    }
                    ViewBag.AccountNumber = hypotheDeedHolder.LoanAcc;
                    ViewBag.LoanSub = hypotheDeedHolder.LoanSub;
                    ViewBag.OffCd = hypotheDeedHolder.OffcCd;
                    _sessionManager.SetHypothecationList(hypotheDeedlist);
                    List<HypoAssetDetailDTO> activeHoldersList = hypotheDeedlist.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();                   

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.hypoviewPath +  Constants.ViewAll, activeHoldersList) });
                }
                ViewBag.AccountNumber = hypotheDeedHolder.LoanAcc;
                ViewBag.LoanSub = hypotheDeedHolder.LoanSub;
                ViewBag.OffCd = hypotheDeedHolder.OffcCd;

                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.HypothecationDto,
                   id, hypotheDeedHolder.HypothNo, hypotheDeedHolder.HypothDesc, hypotheDeedHolder.ExecutionDate));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.hypoviewPath +  Constants.ViewAll, hypotheDeedHolder) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long AccountNumber, byte OffCd, int LoanSub, int IdmHypothDetId)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);

                var AllHypothecationList = _sessionManager.GetAllHypothecationList();
                ViewBag.AssetRefList = AllHypothecationList;

                ViewBag.AccountNumber = AccountNumber;
                //ViewBag.IdmHypothDetId = IdmHypothDetId;

                ViewBag.LoanSub = LoanSub;
                ViewBag.OffCd = OffCd;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.hyporesultViewPath + Constants.editCs);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(List<int> id, HypoAssetDetailDTO hypotheDeedHolder, IFormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    List<HypoAssetDetailDTO> hypotheDeedlist = new();
                    if (_sessionManager.GetAllHypothecationList() != null)
                        hypotheDeedlist = _sessionManager.GetAllHypothecationList();

                    foreach (var item in id)
                    {

                        HypoAssetDetailDTO assetexist = hypotheDeedlist.Find(x => x.IdmHypothDetId == item);

                        if (assetexist != null)
                        {
                            hypotheDeedlist.Remove(assetexist);

                            var newHold = new HypoAssetDetailDTO()
                            {
                                IdmHypothDetId = item,
                                AssetCd = assetexist.AssetCd,
                                LoanSub = assetexist.LoanSub,
                                OffcCd = assetexist.OffcCd,
                                AssetValue = assetexist.AssetValue,
                                HypothValue = hypotheDeedHolder.HypothValue,
                                LoanAcc = assetexist.LoanAcc,
                                HypothNo = hypotheDeedHolder.HypothNo,
                                HypothDesc = hypotheDeedHolder.HypothDesc,
                                ExecutionDate = hypotheDeedHolder.ExecutionDate,
                                AssetDet = assetexist.AssetDet,
                                AssetName = assetexist.AssetName,
                                Action = (int)Constant.Create,
                                UniqueId = Guid.NewGuid().ToString(),
                                IsActive = true,
                                IsDeleted = false,

                            };
                            hypotheDeedlist.Add(newHold);

                        }

                    }
                    ViewBag.AccountNumber = hypotheDeedHolder.LoanAcc;
                    ViewBag.LoanSub = hypotheDeedHolder.LoanSub;
                    ViewBag.OffCd = hypotheDeedHolder.OffcCd;
                    _sessionManager.SetHypothecationList(hypotheDeedlist);
                    List<HypoAssetDetailDTO> activeHoldersList = hypotheDeedlist.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.hypoviewPath + Constants.ViewAll, activeHoldersList) });
                }
                ViewBag.AccountNumber = hypotheDeedHolder.LoanAcc;
                ViewBag.LoanSub = hypotheDeedHolder.LoanSub;
                ViewBag.OffCd = hypotheDeedHolder.OffcCd;

                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.HypothecationDto,
                   id, hypotheDeedHolder.HypothNo, hypotheDeedHolder.HypothDesc, hypotheDeedHolder.ExecutionDate));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.hypoviewPath + Constants.ViewAll, hypotheDeedHolder) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        public async Task<IActionResult> ViewDeed(long AccountNumber)
        {
            try
            {
               // var AllHypothecationList = _sessionManager.GetAllHypothecationList();
                var paramater = "AllRecords";
                var AllHypothecationList = await _legalDocumentationService.GetAllHypothecationList(AccountNumber, paramater);
                List<HypoAssetDetailDTO> distinctDeedNo = AllHypothecationList
                .Where(x => x.HypothNo != null)
                .OrderBy(x => x.ModifiedDate)                
                .GroupBy(p => p.HypothNo)                
                .Select(g => g.First())
                .ToList();
                var newList = distinctDeedNo.OrderByDescending(x => x.ModifiedDate).ToList();
                return View(Constants.hyporesultViewPath + Constants.ViewDeedCs, newList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        public async Task<IActionResult> ViewDeedDetails(string Deedno,long AccountNumber)
        {
            try
            {
               // var AllHypothecationList = _sessionManager.GetAllHypothecationList();
                var paramater = "AllRecords";
                var AllHypothecationList = await _legalDocumentationService.GetAllHypothecationList(AccountNumber, paramater);
                var HypothecationList = AllHypothecationList.Where(r => r.HypothNo==Deedno).ToList();
                foreach(var item in HypothecationList)
                {
                    item.AssetValue *= 100000;
                }
                var AllAssetRefDetails = await _legalDocumentationService.GetAllAssetRefList(AccountNumber);
                ViewBag.AccountNumber = AccountNumber;
                ViewBag.deednumber = Deedno;
                //   List<AssetRefnoDetailsDTO> AssetReflist = AllAssetRefDetails.Where(x => HypothecationList.Any(y => y.AssetCd == x.AssetCd)).ToList();

                return View(Constants.hyporesultViewPath + Constants.ViewDeedDetailsCs, HypothecationList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult DeedViewRecord(int assetcd, string Deedno)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + assetcd);

                 var AllHypothecationList = _sessionManager.GetAllHypothecationList();
             
                HypoAssetDetailDTO HypothecationList = AllHypothecationList.FirstOrDefault(x => x.AssetCd == assetcd);
                HypothecationList.HypothNo = Deedno;
                var AssetTypeDDL = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionAssetType));
                ViewBag.AssetType = AssetTypeDDL;

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == HypothecationList.IdmHypothDetId && x.SubModuleType == Constants.Hypothecation && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.Documentlist = doc;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + assetcd);
                return View(Constants.hyporesultViewPath + Constants.ViewRecord, HypothecationList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
