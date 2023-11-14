using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails.PromoterBankInfo
{
    /// <summary>
    /// Author: Dev
    /// Date: 01/09/2022
    /// Module: Promoter Bank Info
    /// Updated By Gagana - Added Promoter name details
    /// </summary>

    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class PromoterBankInfoController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        

        public PromoterBankInfoController( ILogger logger, SessionManager sessionManager)
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
                var AllPromoterBankList = _sessionManager.GetAllPromoterBankList();
                IdmPromoterBankDetailsDTO PromoterBankList = AllPromoterBankList.FirstOrDefault(x => x.UniqueId == id);
                //var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));
                var AllAccountType = _sessionManager.GetDDListAccountType();                
                ViewBag.PromoterAccount = AllAccountType;
                //ViewBag.AllPromoterNames = allPromoterNames;
                var allPromoterNames = _sessionManager.GetAllPromoterProfileList();
                {
                    ViewBag.AllPromoterNames = allPromoterNames.Select(x => new { x.PromName, x.PromoterCode }).ToList();
                }
                var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();
                ViewBag.IfscBankDetails = bankIsfcDetails;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.promBankresultViewPath + Constants.ViewRecord, PromoterBankList);
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

                var AllprombankList = _sessionManager.GetAllPromoterBankList();
                if (AllprombankList != null)
                {
                    ViewBag.ItemNumber = AllprombankList.Select(x => x.IdmPromBankId).ToList();
                }
                var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();
                //var IfscCodeDDL = _sessionManager.GetAllBankIFSCCodeDDL();
                var AllAccountType = _sessionManager.GetDDListAccountType();
                //var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));
                var allPromoterNames  = _sessionManager.GetAllPromoterProfileList();
                {
                    ViewBag.AllPromoterNames = allPromoterNames.Select(x => new { x.PromName,x.PromoterCode }).ToList();
                } 
                //ViewBag.IfscCodeDDL = IfscCodeDDL;
                ViewBag.IfscBankDetails = bankIsfcDetails;
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.InUnit = InUnit;
                ViewBag.PromoterAccount = AllAccountType;

                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.promBankresultViewPath + Constants.createCS);          
                
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmPromoterBankDetailsDTO promoterBank)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStartedPost);
                if (ModelState.IsValid)
                {
                    List<IdmPromoterBankDetailsDTO> cprombank = _sessionManager.GetAllPromoterBankList();
                    IdmPromoterBankDetailsDTO list = new IdmPromoterBankDetailsDTO();
                    list.LoanAcc = promoterBank.LoanAcc;
                    list.LoanSub = promoterBank.LoanSub;
                    list.OffcCd = promoterBank.OffcCd;
                    list.UtCd = promoterBank.UtCd;
                    //var allIfscCode = _sessionManager.GetAllBankIFSCCodeDDL();
                    //var IfscCode = allIfscCode.Where(x => x.Value == promoterBank.PrmIfscId.ToString());
                    //list.PrmIFSCValue = IfscCode.First().Text;
                    //list.PrmIfsc = IfscCode.First().Text;
                    var allpromoternames = _sessionManager.GetAllPromoterNames();
                    var promotername = allpromoternames.Where(x => x.Value == promoterBank.PromoterCode.ToString());
                    list.PromoterName = promotername.First().Text;
                    var AllAccountType = _sessionManager.GetDDListAccountType();
                    var promoteracc = AllAccountType.Where(x => x.Value == promoterBank.PrmAcType.ToString());
                    list.PrmAcTypeDet = promoteracc.First().Text;
                    list.PromoterCode = promoterBank.PromoterCode;
                    list.IdmPromBankId = promoterBank.IdmPromBankId;
                    list.PrmAcType = promoterBank.PrmAcType;
                    list.PrmBankName = promoterBank.PrmBankName;
                    list.PrmBankBranch = promoterBank.PrmBankBranch;
                    list.PrmAcNo = promoterBank.PrmAcNo;
                    list.PrmIfscId = promoterBank.PrmIfscId;
                    list.PrmBankAddress = promoterBank.PrmBankAddress;
                    list.PrmBankAcName = promoterBank.PrmBankAcName;
                    list.PrmBankState = promoterBank.PrmBankState;
                    list.PrmBankDistrict = promoterBank.PrmBankDistrict;
                    list.PrmBankTaluk = promoterBank.PrmBankTaluk;
                    list.PrmBankPincode = promoterBank.PrmBankPincode;
                    list.PrmPrimaryBank = promoterBank.PrmPrimaryBank;
                    list.PrmCibilScore = promoterBank.PrmCibilScore;
                    var ifsc = _sessionManager.GetIfscBankDetailsList();
                    var res = ifsc.Where(x => x.IFSCRowID == list.PrmIfscId);
                    list.PrmIFSCValue = res.Select(x => x.IFSCCode).First();
                    list.IsDeleted = false;
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.Action = (int)Constant.Create;
                    cprombank.Add(list);
                    _sessionManager.SetPromoterBankList(cprombank);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InUnit = list.UtCd;
                    List<IdmPromoterBankDetailsDTO> activeDetails = cprombank.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    _logger.Information(CommonLogHelpers.CreateCompletedPost);
                    return Json(new { isValid = true, data = promoterBank.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.promBankviewPath + Constants.ViewAll, activeDetails) });

                }
                ViewBag.AccountNumber = promoterBank.LoanAcc;
                ViewBag.LoanSub = promoterBank.LoanSub;
                ViewBag.OffcCd = promoterBank.OffcCd;
                ViewBag.InUnit = promoterBank.UtCd;
                _logger.Information(CommonLogHelpers.Failed);
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.promBankviewPath + Constants.Create, promoterBank) });
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

                var AllprombankList = _sessionManager.GetAllPromoterBankList();
                IdmPromoterBankDetailsDTO pbankList = AllprombankList.FirstOrDefault(x => x.UniqueId == id);
                if(pbankList != null)
                {
                    var AccountNumber = pbankList.LoanAcc;
                    var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();

                    //var IfscCodeDDL = _sessionManager.GetAllBankIFSCCodeDDL();
                    var AllAccountType = _sessionManager.GetDDListAccountType();
                    //var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));

                    //ViewBag.AllPromoterNames = allPromoterNames;
                    //ViewBag.IfscCodeDDL = IfscCodeDDL;
                    var allPromoterNames = _sessionManager.GetAllPromoterProfileList();
                    {
                        ViewBag.AllPromoterNames = allPromoterNames.Select(x => new { x.PromName, x.PromoterCode }).ToList();
                    }
                    ViewBag.IfscBankDetails = bankIsfcDetails;
                    ViewBag.PromoterAccount = AllAccountType;
                    ViewBag.LoanAcc = AccountNumber;
                    ViewBag.LoanSub = pbankList.LoanSub;
                    ViewBag.OffcCd = pbankList.OffcCd;
                    ViewBag.InUnit = pbankList.UtCd;
                    ViewBag.idmprombankid = pbankList.IdmPromBankId;
                }
                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.promBankresultViewPath + Constants.editCs, pbankList);                              
                
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmPromoterBankDetailsDTO promoterBank)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);

                List<IdmPromoterBankDetailsDTO> eprombank = _sessionManager.GetAllPromoterBankList();
                IdmPromoterBankDetailsDTO promoterExist = eprombank.Find(x => x.UniqueId == id);
                if (promoterExist != null)
                {

                    eprombank.Remove(promoterExist);
                    var list = promoterExist;
                    list.LoanAcc = promoterBank.LoanAcc;
                    list.LoanSub = promoterBank.LoanSub;
                    list.OffcCd = promoterBank.OffcCd;
                    list.UtCd = promoterBank.UtCd;
                    var allIfscCode = _sessionManager.GetAllBankIFSCCodeDDL();
                    var IfscCode = allIfscCode.Where(x => x.Value == promoterBank.PrmIfscId.ToString());
                    list.PrmIfsc = IfscCode.First().Text;
                    list.PrmIFSCValue= IfscCode.First().Text;
                    var AllAccountType = _sessionManager.GetDDListAccountType();
                    var promoteracc = AllAccountType.Where(x => x.Value == promoterBank.PrmAcType.ToString());
                    list.PrmAcTypeDet = promoteracc.First().Text;
                    list.IdmPromBankId = promoterBank.IdmPromBankId;                                        
                    list.PrmAcType = promoterBank.PrmAcType;
                    list.PrmBankName = promoterBank.PrmBankName;
                    list.PrmBankAcName = promoterBank.PrmBankAcName;
                    list.PrmBankBranch = promoterBank.PrmBankBranch;                    
                    list.PrmAcNo = promoterBank.PrmAcNo;
                    list.PrmIfscId = promoterBank.PrmIfscId;
                    list.PrmBankAddress = promoterBank.PrmBankAddress;
                    list.PrmBankState = promoterBank.PrmBankState;
                    list.PrmBankDistrict = promoterBank.PrmBankDistrict;
                    list.PrmBankTaluk = promoterBank.PrmBankTaluk;
                    list.PrmBankPincode = promoterBank.PrmBankPincode;
                    list.PrmPrimaryBank = promoterBank.PrmPrimaryBank;
                    list.PrmCibilScore = promoterBank.PrmCibilScore;
                    var ifsc = _sessionManager.GetIfscBankDetailsList();
                    var res = ifsc.Where(x => x.IFSCRowID == list.PrmIfscId);
                    list.PrmIFSCValue = res.Select(x => x.IFSCCode).First();
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.PromoterCode= promoterExist.PromoterCode;
                    list.PromoterName = promoterExist.PromoterName;
                    if (promoterExist.IdmPromBankId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    eprombank.Add(list);
                    _sessionManager.SetPromoterBankList(eprombank);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InUnit = list.UtCd;
                    List<IdmPromoterBankDetailsDTO> activeDetails = eprombank.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    _logger.Information(CommonLogHelpers.UpdateCompletedPost + id);
                    return Json(new { isValid = true, data = promoterBank.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.promBankviewPath + Constants.ViewAll, activeDetails) });

                }
                ViewBag.AccountNumber = promoterBank.LoanAcc;
                ViewBag.LoanSub = promoterBank.LoanSub;
                ViewBag.OffcCd = promoterBank.OffcCd;
                ViewBag.InUnit = promoterBank.UtCd;
                _logger.Information(CommonLogHelpers.Failed + id);
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.promBankviewPath + Constants.Edit, promoterBank) });
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
                IEnumerable<IdmPromoterBankDetailsDTO> activePBankDetails = new List<IdmPromoterBankDetailsDTO>();

                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, Id));                
                var promoterBankList = JsonConvert.DeserializeObject<List<IdmPromoterBankDetailsDTO>>(HttpContext.Session.GetString(Constants.sessionPromBank));
                var itemToRemove = promoterBankList.Find(r => r.UniqueId == Id);
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;
                itemToRemove.Action = (int)Constant.Delete;
                promoterBankList.Add(itemToRemove);                

                _sessionManager.SetPromoterBankList(promoterBankList);
                if (promoterBankList.Count != 0)
                {
                    activePBankDetails = promoterBankList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, Id));
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InUnit = itemToRemove.UtCd;
               return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.promBankviewPath + Constants.ViewAll, activePBankDetails) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}