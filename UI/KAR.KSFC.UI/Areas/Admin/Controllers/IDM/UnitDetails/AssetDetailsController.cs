using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class AssetDetailsController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        public AssetDetailsController(ILogger logger, SessionManager sessionManager, IUnitDetailsService unitDetailsService)
        {

            _logger = logger;
            _sessionManager = sessionManager;
        }

        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllAssetDetailsList = _sessionManager.GetAssetDetailsList();
                IdmPromAssetDetDTO AssetDetails = AllAssetDetailsList.FirstOrDefault(x => x.UniqueId == id);

                //var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));

                //ViewBag.AllPromoterNames = allPromoterNames;
                var allPromoterNames = _sessionManager.GetAllPromoterProfileList();
                {
                    ViewBag.AllPromoterNames = allPromoterNames.Select(x => new { x.PromName, x.PromoterCode }).ToList();
                }

                var allAssetType = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionAssetTypeMaster));
                ViewBag.AssetType = allAssetType;


                var allAssetCategary = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionAssetCategaryMaster));
                ViewBag.AssetCategary = allAssetCategary;

                var AllLandType = _sessionManager.GetDDListLandType();
                ViewBag.LandType = AllLandType;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.chngassetresultViewPath + Constants.ViewRecord, AssetDetails);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult Create(long AccountNumber, byte OffcCd, string LoanSub, int InUnit)
        {
            try 
            {
                
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                var allAssetDetailsList = _sessionManager.GetAllAssetTypeDetails();
                ViewBag.AssetTypes = allAssetDetailsList;
                var allAssetCategaryList = _sessionManager.GetAllAssetCategaryDetails();
                ViewBag.AssetCategary = allAssetCategaryList;


                    
                //var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));

                //ViewBag.AllPromoterNames = allPromoterNames;

                //var allAssetDetailsList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionAssetTypeMaster));
                //ViewBag.AssetTypes = allAssetDetailsList;

                //var allAssetCategary = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionAssetCategaryMaster));
                //ViewBag.AssetCategary = allAssetCategary;

                var AllLandType = _sessionManager.GetDDListLandType();
                ViewBag.LandType = AllLandType;
                var allPromoterNames = _sessionManager.GetAllPromoterProfileList();
                {
                    ViewBag.AllPromoterNames = allPromoterNames.Select(x => new { x.PromName, x.PromoterCode }).ToList();
                }
                ViewBag.AccountNumber = AccountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.InUnit = InUnit;
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                return View(Constants.chngassetresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmPromAssetDetDTO AssetDrtails)
        
        {
            try
            {

                IEnumerable<IdmPromAssetDetDTO> activeAssetinfo = new List<IdmPromAssetDetDTO>();
                _logger.Information(string.Format(CommonLogHelpers.CreateStartedPost + LogAttribute.productDto,
                  AssetDrtails.LoanAcc, AssetDrtails.AssetCatCD, AssetDrtails.AssetTypeCD));

                if (ModelState.IsValid)
                {
                    List<IdmPromAssetDetDTO> Assetinfo = new();
                    if (_sessionManager.GetAssetDetailsList() != null)
                        Assetinfo = _sessionManager.GetAssetDetailsList();

                    IdmPromAssetDetDTO list = new IdmPromAssetDetDTO();
                    list.LoanAcc = AssetDrtails.LoanAcc;
                    var accountNumber = AssetDrtails.LoanAcc;
                    list.UtCd = AssetDrtails.UtCd;
                    list.LoanAcc = AssetDrtails.LoanAcc;                   
                    list.LoanSub = AssetDrtails.LoanSub;
                    list.OffcCd = AssetDrtails.OffcCd;
                    list.UtCd = AssetDrtails.UtCd;
                    list.PromoterCode = AssetDrtails.PromoterCode;
                    list.AssetTypeCD = AssetDrtails.AssetTypeCD;
                    list.AssetCatCD = AssetDrtails.AssetCatCD;
                    list.IdmAssetSiteno = AssetDrtails.IdmAssetSiteno;
                    list.IdmAssetaddr = AssetDrtails.IdmAssetaddr;
                    list.IdmAssetDim = AssetDrtails.IdmAssetDim;
                    list.IdmAssetArea = AssetDrtails.IdmAssetArea;
                    list.IdmAssetDesc = AssetDrtails.IdmAssetDesc;
                    list.IdmAssetValue = AssetDrtails.IdmAssetValue;
                    list.LandType = AssetDrtails.LandType;

                    var allPromoterNames = _sessionManager.GetAllPromoterNames();
                    var PromoterNames = allPromoterNames.Where(x => x.Value == list.PromoterCode.ToString());
                    list.PromoterName = PromoterNames.First().Text;


                    var allAssettypes = _sessionManager.GetallAssetTypeMasterMaster();
                    var assettype = allAssettypes.Where(x => x.Value == list.AssetTypeCD.ToString());
                    list.AssettypeDets = assettype.First().Text;

                    var allAssetCategary = _sessionManager.GetallAssetCategaryMasterMaster();
                    var AssetCategary = allAssetCategary.Where(x => x.Value == list.AssetCatCD.ToString());
                    list.AssetcatDets = AssetCategary.First().Text;

                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    Assetinfo.Add(list);

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InUnit = list.UtCd;
                    _sessionManager.SetAssetDetailsList(Assetinfo);
                    if (Assetinfo.Count != 0)
                    {
                        activeAssetinfo = Assetinfo.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    }

                    _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + LogAttribute.productDto,
                           AssetDrtails.IdmPromassetId, AssetDrtails.AssetCatCD, AssetDrtails.AssetTypeCD));
                    return Json(new { isValid = true, data = accountNumber, html = Helper.RenderRazorViewToString(this, Constants.chngassetViewPath + Constants.ViewAll, activeAssetinfo) });
                }
                ViewBag.AccountNumber = AssetDrtails.LoanAcc;
                ViewBag.LoanSub = AssetDrtails.LoanSub;
                ViewBag.OffcCd = AssetDrtails.OffcCd;
                ViewBag.InUnit = AssetDrtails.UtCd;
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.productDto,
                   AssetDrtails.IdmPromassetId, AssetDrtails.AssetCatCD, AssetDrtails.AssetTypeCD));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.chngassetViewPath + Constants.Create, AssetDrtails) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult Edit(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);
                 var AllAssetDetailsList = _sessionManager.GetAssetDetailsList();
                IdmPromAssetDetDTO AssetDetails = AllAssetDetailsList.FirstOrDefault(x => x.UniqueId == id);
                if (AssetDetails != null)
                {
                    //var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));

                    //ViewBag.AllPromoterNames = allPromoterNames;
                    var allPromoterNames = _sessionManager.GetAllPromoterProfileList();
                    {
                        ViewBag.AllPromoterNames = allPromoterNames.Select(x => new { x.PromName, x.PromoterCode }).ToList();
                    }


                    var allAssetDetailsList = _sessionManager.GetAllAssetTypeDetails();
                    var filteredasset = allAssetDetailsList.Where(x => x.AssetcatCd == AssetDetails.AssetCatCD);
                    ViewBag.filteredasset = filteredasset.Select(x => new { x.AssettypeCd, x.AssettypeDets}).ToList();

                    var allAssetCategaryList = _sessionManager.GetAllAssetCategaryDetails();
                    ViewBag.AssetCategary = allAssetCategaryList;

                    //var allAssetType = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionAssetTypeMaster));
                    //ViewBag.AssetType = allAssetType;

                    //var allAssetCategary = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionAssetCategaryMaster));
                    //ViewBag.AssetCategary = allAssetCategary;

                    var AllLandType = _sessionManager.GetDDListLandType();
                    ViewBag.LandType = AllLandType;

                    ViewBag.AccountNumber = AssetDetails.LoanAcc;
                    ViewBag.LoanSub = AssetDetails.LoanSub;
                    ViewBag.OffcCd = AssetDetails.OffcCd;
                    ViewBag.InUnit = AssetDetails.UtCd;

                }
                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.chngassetresultViewPath + Constants.editCs, AssetDetails);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmPromAssetDetDTO assetinfo)
        {
            try
            {
                List<IdmPromAssetDetDTO> assetDetails = new();
                if (_sessionManager.GetAssetDetailsList() != null)
                    assetDetails = _sessionManager.GetAssetDetailsList();
                IdmPromAssetDetDTO assetExist = assetDetails.Find(x => x.UniqueId == id);
                if (assetExist != null)
                {
                    assetDetails.Remove(assetExist);
                    var list = assetExist;

                    list.LoanAcc = assetinfo.LoanAcc;
                    list.LoanSub = assetinfo.LoanSub;
                    list.OffcCd = assetinfo.OffcCd;
                    list.UtCd = assetinfo.UtCd;
                    list.AssetTypeCD = assetinfo.AssetTypeCD;
                    list.AssetCatCD = assetinfo.AssetCatCD;
                    list.IdmAssetSiteno = assetinfo.IdmAssetSiteno;
                    list.IdmAssetaddr = assetinfo.IdmAssetaddr;
                    list.IdmAssetDim = assetinfo.IdmAssetDim;
                    list.IdmAssetArea = assetinfo.IdmAssetArea;
                    list.IdmAssetDesc = assetinfo.IdmAssetDesc;
                    list.IdmAssetValue = assetinfo.IdmAssetValue;
                    list.LandType = assetinfo.LandType;
                    list.PromoterCode = assetExist.PromoterCode;
                    list.PromoterName = assetExist.PromoterName;

                    var allAssettypes = _sessionManager.GetallAssetTypeMasterMaster();
                    var assetype = allAssettypes.Where(x => x.Value == list.AssetTypeCD.ToString());
                    list.AssettypeDets = assetype.First().Text;

                    var allAssetCategary = _sessionManager.GetallAssetCategaryMasterMaster();
                    var AssetCategary = allAssetCategary.Where(x => x.Value == list.AssetCatCD.ToString());
                    list.AssetcatDets = AssetCategary.First().Text;

                    list.IsActive = true;
                    list.IsDeleted = false;

                    list.CreatedDate = assetinfo.CreatedDate;
                    if (assetExist.IdmPromassetId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    assetDetails.Add(list);

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InUnit = list.UtCd;
                    _sessionManager.SetAssetDetailsList(assetDetails);
                    List<IdmPromAssetDetDTO> activeAssetinfo = assetDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.chngassetViewPath + Constants.ViewAll, activeAssetinfo) });
                }
                ViewBag.AccountNumber = assetinfo.LoanAcc;
                ViewBag.LoanSub = assetinfo.LoanSub;
                ViewBag.OffcCd = assetinfo.OffcCd;
                ViewBag.InUnit = assetinfo.UtCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.chngassetViewPath + Constants.Edit, assetinfo) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
                

        [HttpPost]
        public IActionResult Delete(string Id)
        {
            try
            {
                IEnumerable<IdmPromAssetDetDTO> activeAssetList = new List<IdmPromAssetDetDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, Id));
                var assetDetailsList = JsonConvert.DeserializeObject<List<IdmPromAssetDetDTO>>(HttpContext.Session.GetString(Constants.sessionAllAssets));
                var itemToRemove = assetDetailsList.Find(r => r.UniqueId == Id);
                assetDetailsList.Remove(itemToRemove);
                var list = itemToRemove;
                list.IsActive = false;
                list.IsDeleted = true;
                list.Action = (int)Constant.Delete;
                if (list.IdmPromassetId == 0)
                {
                    list.IdmAssetValue = 0;
                }
                assetDetailsList.Add(list);
                _sessionManager.SetAssetDetailsList(assetDetailsList);
                if (assetDetailsList.Count != 0)
                {
                    activeAssetList = assetDetailsList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InUnit = itemToRemove.UtCd;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, Id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.chngassetViewPath + Constants.ViewAll, activeAssetList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult AssetTypeDropDown(int Id)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedGetAllAssetType + Id);
                var AllAssetTypeDetails = _sessionManager.GetAllAssetTypeDetails();
                var AllAssetCategaryDetails = _sessionManager.GetAllAssetCategaryDetails();
                TblAssetCatCDTabDTO AssetCategaryDetails = AllAssetCategaryDetails.Find(x => x.AssetcatCd == Id);
                var output = AllAssetTypeDetails.Where(x => x.AssetcatCd == AssetCategaryDetails.AssetcatCd).Select(x => new { x.AssettypeCd, x.AssettypeDets }).ToList();
                _logger.Information(CommonLogHelpers.CompletedGetAllAssetType + Id);
                return Json(new SelectList(output, Constants.AssettypeCd, Constants.AssettypeDets));
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.GetAllAssetType + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
