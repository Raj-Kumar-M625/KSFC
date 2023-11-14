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
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ILegalDocumentationService;
using KAR.KSFC.UI.Services.Admin.IDM.LegalDocumentationService;
using System.Xml;
using System.Globalization;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.LegalDocumentation.Cersai
{
    /// <summary>
    ///  Author: Gagana K; Module: CERSAI; Date:21/07/2022
    /// </summary>
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class CersaiController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly ILegalDocumentationService _legalDocumentationService;
        public CersaiController(ILegalDocumentationService legalDocumentationService, ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _legalDocumentationService = legalDocumentationService;
        }
        public IActionResult ViewRecord(int assetcd, string cersai)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + cersai);
                var AllCERSAIList = _sessionManager.GetAllCERSAIList();
                var AssetRefDetails = _sessionManager.GetAllAssetRefList();
                var AssetTypeDDL = _sessionManager.GetAllAssetType();
                var allAssetCategary = _sessionManager.GetallAssetCategaryMasterMaster();

                var CERSAIList = AllCERSAIList.Where(x => x.CersaiRegNo == cersai && x.AssetCd == assetcd).FirstOrDefault();
                var AssetList = AssetRefDetails.Where(x => x.AssetCd == assetcd).FirstOrDefault();

                AssetList.TblIdmCersaiRegistration.FirstOrDefault().CersaiRegNo = cersai;
                AssetList.TblIdmCersaiRegistration.FirstOrDefault().CersaiRegDate = CERSAIList.CersaiRegDate;
                AssetList.TblIdmCersaiRegistration.FirstOrDefault().CersaiRemarks = CERSAIList.CersaiRemarks;
                AssetList.TblIdmCersaiRegistration.FirstOrDefault().AssetVal = CERSAIList.AssetVal * 100000;
                AssetList.TblIdmCersaiRegistration.FirstOrDefault().AssetDet = CERSAIList.AssetDet;
                ViewBag.AssetType = AssetTypeDDL;
                ViewBag.AssetCategary = allAssetCategary;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + cersai);

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == CERSAIList.IdmDsbChargeId && x.SubModuleType == Constants.CERSAI && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.Documentlist = doc;
                return View(Constants.cersairesultViewPath + Constants.ViewRecord, AssetList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        public IActionResult ViewRegister(int assetcd, string cersai)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + cersai);
                var AllCERSAIList = _sessionManager.GetAllCERSAIList();
                var AssetRefDetails = _sessionManager.GetAllAssetRefList();
                if (cersai != null)
                {
                    var CERSAIList = AllCERSAIList.Where(x => x.CersaiRegNo == cersai && x.AssetCd == assetcd).FirstOrDefault();
                    var AssetList = AssetRefDetails.Where(x => x.AssetCd == assetcd).FirstOrDefault();
                    var AssetTypeDDL = _sessionManager.GetAllAssetType();
                    var allAssetCategary = _sessionManager.GetallAssetCategaryMasterMaster();

                    AssetList.TblIdmCersaiRegistration.FirstOrDefault().CersaiRegNo = cersai;
                    AssetList.TblIdmCersaiRegistration.FirstOrDefault().CersaiRegDate = CERSAIList.CersaiRegDate;
                    AssetList.TblIdmCersaiRegistration.FirstOrDefault().CersaiRemarks = CERSAIList.CersaiRemarks;
                    AssetList.TblIdmCersaiRegistration.FirstOrDefault().AssetVal = CERSAIList.AssetVal * 100000;
                    AssetList.TblIdmCersaiRegistration.FirstOrDefault().AssetDet = CERSAIList.AssetDet;

                    ViewBag.AssetType = AssetTypeDDL;
                    ViewBag.AssetCategary = allAssetCategary;
                    _logger.Information(CommonLogHelpers.ViewRecordCompleted + cersai);
                    return View(Constants.cersairesultViewPath + Constants.ViewRegister, AssetList);
                }
                else
                {
                    var AssetTypeDDL = _sessionManager.GetAllAssetType();
                    ViewBag.AssetType = AssetTypeDDL;
                    var allAssetCategary = _sessionManager.GetallAssetCategaryMasterMaster();
                    ViewBag.AssetCategary = allAssetCategary;
                    var AssetList = AssetRefDetails.Where(x => x.AssetCd == assetcd).FirstOrDefault();
                    return View(Constants.cersairesultViewPath + Constants.ViewRegister, AssetList);
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Register(long AccountNumber, byte OffCd, int LoanSub, int IdmDsbChargeId)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                //var AssetRefDetails = _sessionManager.GetAllAssetRefList();
                var AllCersaiList = _sessionManager.GetAllCERSAIList();
                var allSecurityHolderList = await _legalDocumentationService.GetAllPrimaryCollateralList(AccountNumber);
                //if (AllCersaiList != null)
                //{
                //    ViewBag.CersaiRegNumber = AllCersaiList.Select(x => x.CersaiRegNo).ToList();
                //}

                //ViewBag.AssetRefDetails = AllCersaiList;

                foreach (var obj in AllCersaiList)
                {
                    obj.AssetVal = obj.AssetVal * 100000;
                }
                DateTime date = (DateTime)allSecurityHolderList.ToList().Last().ExecutionDate;
                ViewBag.AccountNumber = AccountNumber;
                ViewBag.CersaiList = AllCersaiList;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffCd = OffCd;
                ViewBag.ExeDate = date.AddDays(30).ToString("yyyy-MM-dd");
                ViewBag.IdmDsbChargeId = IdmDsbChargeId;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.cersaiviewPath + Constants.Register);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(List<int> id, IdmCersaiRegDetailsDTO registration, IFormCollection form)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.RegisterStartedPost + LogAttribute.CersaiDto,
                    id, registration.CersaiRegNo, registration.CersaiRegDate, registration.CersaiRemarks));

                if (ModelState.IsValid)
                {

                    List<IdmCersaiRegDetailsDTO> cersairegistration = new();
                    if (_sessionManager.GetAllCERSAIList() != null)
                        cersairegistration = _sessionManager.GetAllCERSAIList();

                    //List<AssetRefnoDetailsDTO> AssetReflist = new();
                    //if (_sessionManager.GetAllAssetRefList() != null)
                    //AssetReflist = _sessionManager.GetAllAssetRefList();
                    //CultureInfo provider = CultureInfo.InvariantCulture;
                    foreach (var item in id)
                    {
                        IdmCersaiRegDetailsDTO assetExist = cersairegistration.Find(x => x.IdmDsbChargeId == item);
                        if (assetExist != null)
                        {
                            cersairegistration.Remove(assetExist);

                            var newCresai = new IdmCersaiRegDetailsDTO()
                            {
                                IdmDsbChargeId = item,
                                LoanAcc = assetExist.LoanAcc,
                                LoanSub = assetExist.LoanSub,
                                OffcCd = assetExist.OffcCd,
                                AssetCd = assetExist.AssetCd,
                                AssetDet = assetExist.AssetDet,
                                AssetVal = assetExist.AssetVal,
                                CersaiRegNo = registration.CersaiRegNo,
                                CersaiRegDate = registration.CersaiRegDate,
                                CersaiRemarks = registration.CersaiRemarks,
                                Action = (int)Constant.Create,
                                UniqueId = Guid.NewGuid().ToString(),
                                IsActive = true,
                                IsDeleted = false,

                            };
                            cersairegistration.Add(newCresai);
                        }
                    }


                    ViewBag.AccountNumber = registration.LoanAcc;
                    ViewBag.LoanAcc = registration.LoanSub;
                    ViewBag.OffcCd = registration.OffcCd;
                    _sessionManager.SetCERSAIList(cersairegistration);
                    List<IdmCersaiRegDetailsDTO> activeHoldersList = cersairegistration.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();

                    _logger.Information(string.Format(CommonLogHelpers.RegisterCompletedPost + LogAttribute.CersaiDto,
                    id, registration.CersaiRegNo, registration.CersaiRegDate, registration.CersaiRemarks));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.cersaiviewPath + Constants.ViewAll, activeHoldersList) });
                }
                ViewBag.AccountNumber = registration.LoanAcc;
                ViewBag.LoanAcc = registration.LoanSub;
                ViewBag.OffcCd = registration.OffcCd;

                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.CersaiDto,
                 id, registration.CersaiRegNo, registration.CersaiRegDate, registration.CersaiRemarks));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.cersaiviewPath + Constants.Register, registration) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.RegisterErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult Edit(long AccountNumber, byte OffCd, int LoanSub, int IdmDsbChargeId)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                //var AssetRefDetails = _sessionManager.GetAllAssetRefList();
                var AllCersaiList = _sessionManager.GetAllCERSAIList();
                var dateofregister = _sessionManager.GetPrimaryCollateralList();
                DateTime date = (DateTime)dateofregister.Last().ExecutionDate;
                DateTime dateTime = date.AddDays(30);
                ViewBag.Date = dateTime.ToString("dd/M/yyyy");
                //if (AllCersaiList != null)
                //{
                //    ViewBag.CersaiRegNumber = AllCersaiList.Select(x => x.CersaiRegNo).ToList();
                //}

                //ViewBag.AssetRefDetails = AllCersaiList;

                foreach (var obj in AllCersaiList)
                {
                    obj.AssetVal = obj.AssetVal * 100000;
                }
                ViewBag.AccountNumber = AccountNumber;
                ViewBag.CersaiList = AllCersaiList;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffCd = OffCd;
                ViewBag.IdmDsbChargeId = IdmDsbChargeId;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.cersaiviewPath + Constants.EditAll);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(List<int> id, IdmCersaiRegDetailsDTO registration, IFormCollection form)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.RegisterStartedPost + LogAttribute.CersaiDto,
                    id, registration.CersaiRegNo, registration.CersaiRegDate, registration.CersaiRemarks));

                if (ModelState.IsValid)
                {

                    List<IdmCersaiRegDetailsDTO> cersairegistration = new();
                    if (_sessionManager.GetAllCERSAIList() != null)
                        cersairegistration = _sessionManager.GetAllCERSAIList();

                    //List<AssetRefnoDetailsDTO> AssetReflist = new();
                    //if (_sessionManager.GetAllAssetRefList() != null)
                    //AssetReflist = _sessionManager.GetAllAssetRefList();
                    //CultureInfo provider = CultureInfo.InvariantCulture;
                    foreach (var item in id)
                    {
                        IdmCersaiRegDetailsDTO assetExist = cersairegistration.Find(x => x.IdmDsbChargeId == item);
                        if (assetExist != null)
                        {
                            cersairegistration.Remove(assetExist);

                            var newCresai = new IdmCersaiRegDetailsDTO()
                            {
                                IdmDsbChargeId = item,
                                LoanAcc = assetExist.LoanAcc,
                                LoanSub = assetExist.LoanSub,
                                OffcCd = assetExist.OffcCd,
                                AssetCd = assetExist.AssetCd,
                                AssetDet = assetExist.AssetDet,
                                AssetVal = assetExist.AssetVal,
                                CersaiRegNo = registration.CersaiRegNo,
                                CersaiRegDate = registration.CersaiRegDate,
                                CersaiRemarks = registration.CersaiRemarks,
                                Action = (int)Constant.Create,
                                UniqueId = Guid.NewGuid().ToString(),
                                IsActive = true,
                                IsDeleted = false,

                            };
                            cersairegistration.Add(newCresai);
                        }
                    }


                    ViewBag.AccountNumber = registration.LoanAcc;
                    ViewBag.LoanAcc = registration.LoanSub;
                    ViewBag.OffcCd = registration.OffcCd;
                    _sessionManager.SetCERSAIList(cersairegistration);
                    List<IdmCersaiRegDetailsDTO> activeHoldersList = cersairegistration.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();

                    _logger.Information(string.Format(CommonLogHelpers.RegisterCompletedPost + LogAttribute.CersaiDto,
                    id, registration.CersaiRegNo, registration.CersaiRegDate, registration.CersaiRemarks));

                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.cersaiviewPath + Constants.ViewAll, activeHoldersList) });
                }
                ViewBag.AccountNumber = registration.LoanAcc;
                ViewBag.LoanAcc = registration.LoanSub;
                ViewBag.OffcCd = registration.OffcCd;

                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.CersaiDto,
                 id, registration.CersaiRegNo, registration.CersaiRegDate, registration.CersaiRemarks));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.cersaiviewPath + Constants.Register, registration) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.RegisterErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public async Task<IActionResult> ViewRegistration(long AccountNumber)
        {
            try
            {
                //var AllCersaiList = _sessionManager.GetAllCERSAIList();
                var AllAssetRefDetails = _sessionManager.GetAllAssetRefList();
                var parameter = "AllRecords";
                var AllCersaiList = await _legalDocumentationService.GetAllCERSAIList(AccountNumber, parameter);
                //var AllAssetRefDetails = await _legalDocumentationService.GetAllAssetRefList(AllCersaiList.FirstOrDefault().LoanAcc);
                var result = (from a in AllCersaiList
                              join b in AllAssetRefDetails
                              on a.AssetCd equals b.AssetCd
                              select new IdmCersaiRegDetailsDTO()
                              {
                                  CersaiRegNo = a.CersaiRegNo,
                                  TotalValue = a.AssetVal,
                                  CreatedDate = a.CreatedDate,
                                  ModifiedDate = a.ModifiedDate
                              }).GroupBy(x => x.CersaiRegNo).Select(x => new IdmCersaiRegDetailsDTO
                              {
                                  CersaiRegNo = x.First().CersaiRegNo,
                                  TotalValue = x.Sum(y => y.TotalValue),
                                  CreatedDate = x.First().CreatedDate,
                                  ModifiedDate = x.First().ModifiedDate
                              });
                List<IdmCersaiRegDetailsDTO> distinctCersaiNo = AllCersaiList
               .Where(x => x.CersaiRegNo != null)
              .OrderBy(x => x.ModifiedDate)
                .GroupBy(p => p.CersaiRegNo)
                .Select(g => g.First())
                .ToList();
                var newList = distinctCersaiNo.OrderByDescending(x => x.ModifiedDate).ToList();

                //var newlist = result.orderby(x => x.createddate).tolist();
                //if (newlist.count > 1)
                //{
                //    for (int i = 0; i < newlist.count; i++)
                //    {
                //        if (i + 1 < newlist.count)
                //        {
                //            newlist[i].modifieddate = newlist[i + 1].createddate;

                //            if (newlist[i].modifieddate?.dayofyear > newlist[i].createddate?.dayofyear)
                //            {
                //                newlist[i].modifieddate = convert.todatetime(newlist[i].modifieddate).subtract(timespan.fromdays(1));
                //            }
                //        }

                //    }

                //}



                return View(Constants.cersairesultViewPath + Constants.ViewRegistrationCs, newList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public async Task<IActionResult> ViewRegistrationDetails(string CersaiNo, long AccountNumber)
        {
            try
            {
                //var AllCersaiList = _sessionManager.GetAllCERSAIList();
                var parameter = "AllRecords";
                var AllCersaiList = await _legalDocumentationService.GetAllCERSAIList(AccountNumber, parameter);
                var CersaiList = AllCersaiList.Where(r => r.CersaiRegNo == CersaiNo).ToList();
                foreach (var obj in CersaiList)
                {
                    obj.AssetVal = obj.AssetVal * 100000;
                }
                //var CersaiList = AllCersaiList.Where(r => r.CersaiRegNo == CersaiNo).ToList();

                //var AllAssetRefDetails = await _legalDocumentationService.GetAllAssetRefList(AllCersaiList.FirstOrDefault().LoanAcc);

                //List<AssetRefnoDetailsDTO> AssetReflist = AllAssetRefDetails.Where(x => CersaiList.Any(y => y.AssetCd == x.AssetCd)).ToList();
                ViewBag.CersaiNo = CersaiNo;
                ViewBag.AccountNumber = AccountNumber;
                return View(Constants.cersairesultViewPath + Constants.ViewRegistrationDetailsCs, CersaiList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult AssetViewRecord(int assetcd, string CersaiNo)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + assetcd);
                var AllCERSAIList = _sessionManager.GetAllCERSAIList();
                IdmCersaiRegDetailsDTO CERSAIList = AllCERSAIList.FirstOrDefault(x => x.AssetCd == assetcd);
                var AssetTypeDDL = _sessionManager.GetAllAssetType();
                CERSAIList.CersaiRegNo = CersaiNo;
                ViewBag.AssetType = AssetTypeDDL;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + assetcd);

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == CERSAIList.IdmDsbChargeId && x.SubModuleType == Constants.CERSAI && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.Documentlist = doc;
                return View(Constants.cersairesultViewPath + Constants.ViewRecord, CERSAIList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult Delete(int IdmDsbChargeId)
        {
            try
            {
                IEnumerable<IdmCersaiRegDetailsDTO> activeCersaiList = new List<IdmCersaiRegDetailsDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, IdmDsbChargeId));
                var cersaiList = _sessionManager.GetAllCERSAIList();
                var itemToRemove = cersaiList.Find(r => r.IdmDsbChargeId == IdmDsbChargeId);
                cersaiList.Remove(itemToRemove);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                cersaiList.Remove(itemToRemove);
                _sessionManager.SetCERSAIList(cersaiList);
                if (cersaiList.Count > 0)
                {
                    activeCersaiList = cersaiList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, IdmDsbChargeId));
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.OffCd = itemToRemove.OffcCd;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.cersaiviewPath + Constants.ViewAll, activeCersaiList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
