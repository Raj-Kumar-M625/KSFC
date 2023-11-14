using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
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

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails.PromoterDetails
{
    /// <summary>
    /// Author: Dev
    /// Date: 29/08/2022
    /// Module: Promoter Profile
    /// </summary>
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class PromoterDetailsController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;


        public PromoterDetailsController( ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllPromoterProfileList = _sessionManager.GetAllPromoterProfileList();
                IdmPromoterDTO PromoterProfileList = AllPromoterProfileList.FirstOrDefault(x => x.UniqueId == id);

                var AllPositionDesignation = _sessionManager.GetDDListPromoterDesignation();
                var AllDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                var AllPromotorClass = _sessionManager.GetDDListPromoterClass();
                var AllPromotorSubClass = _sessionManager.GetDDListPromoterSubClass();
                var AllPromotorQual = _sessionManager.GetDDListPromoterQualification();
                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == PromoterProfileList.IdmPromId && x.SubModuleType == Constants.PromoterDetails && x.MainModule == Constants.ChangeOfUnit).ToList();
                ViewBag.Documentlist = doc;

                ViewBag.PromoterDesignation = AllPositionDesignation;
                ViewBag.PromoterDomicile = AllDomicileStatus;
                ViewBag.PromoterClass = AllPromotorClass;
                ViewBag.PromoterSubClass = AllPromotorSubClass;
                ViewBag.PromoterQual = AllPromotorQual;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.promDetresultViewPath + Constants.ViewRecord, PromoterProfileList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult Create(long accountNumber, byte OffcCd, string LoanSub, int InUnit)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                ViewBag.LoanAcc = accountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                var AllPositionDesignation = _sessionManager.GetDDListPromoterDesignation();
                var AllDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                var AllPromotorClass = _sessionManager.GetDDListPromoterClass();
                var AllPromotorSubClass = _sessionManager.GetDDListPromoterSubClass();
                var AllPromotorQual = _sessionManager.GetDDListPromoterQualification();
                List<TblPromcdtabDTO> masterpromoter = new();
                if (_sessionManager.GetAllMasterPromoterProfileList() != null)
                    masterpromoter = _sessionManager.GetAllMasterPromoterProfileList();

                if (_sessionManager.GetAllPromoterProfileList() != null)
                {
                    ViewBag.Promoters = _sessionManager.GetAllPromoterProfileList();
                }

                ViewBag.PromoterDesignation = AllPositionDesignation;
                ViewBag.PromoterDomicile = AllDomicileStatus;
                ViewBag.PromoterClass = AllPromotorClass;
                ViewBag.PromoterSubClass = AllPromotorSubClass;
                ViewBag.PromoterQual = AllPromotorQual;
                ViewBag.MasterPromoter = masterpromoter;
                ViewBag.InUnit = InUnit;

                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.promDetresultViewPath + Constants.createCS);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmPromoterDTO promoterprofile)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.CreateStartedPost + "" + LogAttribute.IdmPromoterDTO,
                          promoterprofile.IdmPromId, promoterprofile.PromName, promoterprofile.PromPan));

                if (ModelState.IsValid)
                {

                    List<IdmPromoterDTO> cpromprofile = new();
                    List<IdmPromoterDTO> activecpromprofile = new();
                    if (_sessionManager.GetAllPromoterProfileList() != null)
                        cpromprofile = _sessionManager.GetAllPromoterProfileList();

                    var pan = cpromprofile.Find(x => x.PromPan == promoterprofile.PromPan);
                    if(pan != null)
                    {
                        
                        return View();
                    }

                    IdmPromoterDTO list = new IdmPromoterDTO();
                    list.LoanAcc = promoterprofile.LoanAcc;
                    list.LoanSub = promoterprofile.LoanSub;
                    list.OffcCd = promoterprofile.OffcCd;

                    var AllPositionDesignation = _sessionManager.GetDDListPromoterDesignation();
                    var promoterdesig = AllPositionDesignation.Where(x => x.Value == promoterprofile.PdesigCd.ToString());
                    list.PdesigDet = promoterdesig.First().Text;

                    var AllPromotorClass = _sessionManager.GetDDListPromoterClass();
                    var promoterclass = AllPromotorClass.Where(x => x.Value == promoterprofile.PclasCd.ToString());
                    list.PclassDet = promoterclass.First().Text;

                    var AllDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                    var promoterdom = AllDomicileStatus.Where(x => x.Value == promoterprofile.DomCd.ToString());
                    list.PdomDet = promoterdom.First().Text;

                    list.NameFatherSpouse = promoterprofile.NameFatherSpouse;
                    list.IdmPromId = promoterprofile.IdmPromId;
                    list.PclasCd = promoterprofile.PclasCd;
                    list.PdesigCd = promoterprofile.PdesigCd;
                    list.PqualCd = promoterprofile.PdesigCd;
                    list.PromAddlQual = promoterprofile.PromAddlQual;
                    list.PromAge = promoterprofile.PromAge;
                    list.PromChief = promoterprofile.PromChief;
                    list.PromDob = promoterprofile.PromDob;
                    list.PromEmail = promoterprofile.PromEmail;
                    list.PromExDt = promoterprofile.PromExDt;
                    list.PromExpDet = promoterprofile.PromExpDet;
                    list.PromExpYrs = promoterprofile.PromExpYrs;
                    list.PromGender = promoterprofile.PromGender;
                    list.PromJnDt = promoterprofile.PromJnDt;
                    list.PromMajor = promoterprofile.PromMajor;
                    list.PromMobileNo = promoterprofile.PromMobileNo;
                    list.PromName = promoterprofile.PromName;
                    list.PromoterCode = promoterprofile.PromoterCode;
                    list.PromPan = promoterprofile.PromPan;
                    list.PromPassport = promoterprofile.PromPassport;
                    list.PromPhoto = promoterprofile.PromPhoto;
                    list.PromPhyHandicap = promoterprofile.PromPhyHandicap;
                    list.PromTelNo = promoterprofile.PromTelNo;
                    list.PsubclasCd = promoterprofile.PsubclasCd;
                    list.DomCd = promoterprofile.DomCd;
                    list.UtCd = promoterprofile.UtCd;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.Action = (int)Constant.Create;
                    cpromprofile.Add(list);
                    _sessionManager.SetPromoterProfileList(cpromprofile);
                    if (cpromprofile.Count != 0)
                    {
                        activecpromprofile = (cpromprofile.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }

                    _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + "" + LogAttribute.IdmPromoterDTO,
                          promoterprofile.IdmPromId, promoterprofile.PromName, promoterprofile.PromPan));
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InUnit = list.UtCd;
                    return Json(new { isValid = true, data = promoterprofile.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.promDetviewPath + Constants.ViewAll, activecpromprofile) });
                }
                ViewBag.AccountNumber = promoterprofile.LoanAcc;
                ViewBag.LoanSub = promoterprofile.LoanSub;
                ViewBag.OffcCd = promoterprofile.OffcCd;
                ViewBag.InUnit = promoterprofile.UtCd;
                _logger.Information(string.Format(CommonLogHelpers.Failed + "" + LogAttribute.IdmPromoterDTO,
                          promoterprofile.IdmPromId, promoterprofile.PromoterCode, promoterprofile.PromName));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.promDetviewPath + Constants.Create, promoterprofile) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        public IActionResult Edit(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);

                var AllpromprofileList = _sessionManager.GetAllPromoterProfileList();
                IdmPromoterDTO pprofileList = AllpromprofileList.FirstOrDefault(x => x.UniqueId == id);
                if (pprofileList != null)
                {
                    var doclist = _sessionManager.GetIDMDocument();
                    var doc = doclist.Where(x => x.SubModuleId == pprofileList.IdmPromId && x.SubModuleType == Constants.PromoterDetails && x.MainModule == Constants.ChangeOfUnit).ToList();
                    ViewBag.SubModuleId = pprofileList.IdmPromId;
                    ViewBag.SubModuleType = Constants.PromoterDetails;
                    ViewBag.MainModule = Constants.ChangeOfUnit;

                    ViewBag.Documentlist = doc;

                    var AllPositionDesignation = _sessionManager.GetDDListPromoterDesignation();
                    var AllDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                    var AllPromotorClass = _sessionManager.GetDDListPromoterClass();
                    var AllPromotorSubClass = _sessionManager.GetDDListPromoterSubClass();
                    var AllPromotorQual = _sessionManager.GetDDListPromoterQualification();
                    ViewBag.LoanSub = pprofileList.LoanSub;
                    ViewBag.OffcCd = pprofileList.OffcCd;
                    ViewBag.PromoterDesignation = AllPositionDesignation;
                    ViewBag.PromoterDomicile = AllDomicileStatus;
                    ViewBag.PromoterClass = AllPromotorClass;
                    ViewBag.PromoterSubClass = AllPromotorSubClass;
                    ViewBag.PromoterQual = AllPromotorQual;
                    ViewBag.LoanAcc = pprofileList.LoanAcc;
                    ViewBag.InUnit = pprofileList.UtCd;
                }

                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.promDetresultViewPath + Constants.editCs, pprofileList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmPromoterDTO promoterprofile)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + "" +LogAttribute.IdmPromoterDTO,
                          promoterprofile.IdmPromId, promoterprofile.PromName, promoterprofile.PromPan));

                if (_sessionManager.GetAllPromoterProfileList() != null)
                {
                    List<IdmPromoterDTO> epromprofile = _sessionManager.GetAllPromoterProfileList();

                    IdmPromoterDTO promoterExist = epromprofile.Find(x => x.UniqueId == id);
                    if (promoterExist != null)
                    {
                        epromprofile.Remove(promoterExist);
                        var list = promoterExist;
                        list.LoanAcc = promoterprofile.LoanAcc;
                        list.LoanSub = promoterprofile.LoanSub;
                        list.OffcCd = promoterprofile.OffcCd;

                        var AllPositionDesignation = _sessionManager.GetDDListPromoterDesignation();
                        var promoterdesig = AllPositionDesignation.Where(x => x.Value == promoterprofile.PdesigCd.ToString());
                        list.PdesigDet = promoterdesig.First().Text;

                        var AllPromotorClass = _sessionManager.GetDDListPromoterClass();
                        var promoterclass = AllPromotorClass.Where(x => x.Value == promoterprofile.PclasCd.ToString());
                        list.PclassDet = promoterclass.First().Text;

                        var AllDomicileStatus = _sessionManager.GetDDListDomicileStatus();
                        var promoterdom = AllDomicileStatus.Where(x => x.Value == promoterprofile.DomCd.ToString());
                        list.PdomDet = promoterdom.First().Text;

                        list.NameFatherSpouse = promoterprofile.NameFatherSpouse;
                        list.IdmPromId = promoterprofile.IdmPromId;
                        list.PclasCd = promoterprofile.PclasCd;
                        list.PdesigCd = promoterprofile.PdesigCd;
                        list.PqualCd = promoterprofile.PdesigCd;
                        list.PromAddlQual = promoterprofile.PromAddlQual;
                        list.PromAge = promoterprofile.PromAge;
                        list.PromChief = promoterprofile.PromChief;
                        list.PromDob = promoterprofile.PromDob;
                        list.PromEmail = promoterprofile.PromEmail;
                        list.PromExDt = promoterprofile.PromExDt;
                        list.PromExpDet = promoterprofile.PromExpDet;
                        list.PromExpYrs = promoterprofile.PromExpYrs;
                        list.PromGender = promoterprofile.PromGender;
                        list.PromJnDt = promoterprofile.PromJnDt;
                        list.PromMajor = promoterprofile.PromMajor;
                        list.PromMobileNo = promoterprofile.PromMobileNo;
                        list.PromName = promoterprofile.PromName;
                        list.PromoterCode = promoterprofile.PromoterCode;
                        list.PromPan = promoterprofile.PromPan;
                        list.PromPassport = promoterprofile.PromPassport;
                        list.PromPhoto = promoterprofile.PromPhoto;
                        list.PromPhyHandicap = promoterprofile.PromPhyHandicap;
                        list.PromTelNo = promoterprofile.PromTelNo;
                        list.PsubclasCd = promoterprofile.PsubclasCd;
                        list.DomCd = promoterprofile.DomCd;
                        list.UniqueId = promoterprofile.UniqueId;
                        list.UtCd = promoterprofile.UtCd;
                        list.IsActive = true;
                        list.IsDeleted = false;

                        if (promoterExist.IdmPromId > 0)
                        {
                            list.Action = (int)Constant.Update;
                        }
                        else
                        {
                            list.Action = (int)Constant.Create;

                        }

                        epromprofile.Add(list);
                        _sessionManager.SetPromoterProfileList(epromprofile);
                        List<IdmPromoterDTO> activeList = (epromprofile.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                        ViewBag.AccountNumber = promoterprofile.LoanAcc;
                        ViewBag.LoanSub = promoterprofile.LoanSub;
                        ViewBag.OffcCd = promoterprofile.OffcCd;
                        ViewBag.InUnit = list.UtCd;


                        _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + "" + LogAttribute.IdmPromoterDTO,
                              promoterprofile.IdmPromId, promoterprofile.PromName, promoterprofile.PromPan));
                        return Json(new { isValid = true, data = promoterprofile.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.promDetviewPath + Constants.ViewAll, activeList) });
                    }
                   
                }
                ViewBag.AccountNumber = promoterprofile.LoanAcc;
                ViewBag.LoanSub = promoterprofile.LoanSub;
                ViewBag.OffcCd = promoterprofile.OffcCd;
                ViewBag.InUnit = promoterprofile.UtCd;
                _logger.Information(string.Format(CommonLogHelpers.Failed + "" + LogAttribute.IdmPromoterDTO,
                promoterprofile.IdmPromId, promoterprofile.PromoterCode, promoterprofile.PromName));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.promDetviewPath + Constants.Edit, promoterprofile) });
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
                List<IdmPromoterDTO> activepromprofileList = new();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, Id));
                var promoterList = JsonConvert.DeserializeObject<List<IdmPromoterDTO>>(HttpContext.Session.GetString(Constants.sessionPromProfile));
                var itemToRemove = promoterList.Find(r => r.UniqueId == Id);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                promoterList.Add(itemToRemove);
                _sessionManager.SetPromoterProfileList(promoterList);

                if (promoterList.Count != 0)
                {
                    activepromprofileList = promoterList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }

                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InUnit = itemToRemove.UtCd;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, Id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.promDetviewPath + Constants.ViewAll, activepromprofileList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
