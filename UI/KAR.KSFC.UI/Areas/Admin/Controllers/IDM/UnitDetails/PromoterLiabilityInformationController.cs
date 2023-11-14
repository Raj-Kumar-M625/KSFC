using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
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
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails
{
    //ModifieBy:Swetha M  Added Authorization 
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class PromoterLiabilityInformationController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        
        public PromoterLiabilityInformationController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }
        // Author Sandeep M on 08/09/2022
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var PromoterList = _sessionManager.GetAllPromoterLiabilityInfo();
                TblPromoterLiabDetDTO PromoterLiability = PromoterList.FirstOrDefault(x => x.UniqueID == id);

                //var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("DDLSessionPromoterNames"));
                //ViewBag.promoterName = allPromoterNames;
                var allPromoterNames = _sessionManager.GetAllPromoterProfileList();
                {
                    ViewBag.AllPromoterNames = allPromoterNames.Select(x => new { x.PromName, x.PromoterCode }).ToList();
                }

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.promLiabresultViewPath + Constants.ViewRecord, PromoterLiability);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        //Author Sandeep M on 09/09/2022
        [HttpGet]
        public IActionResult Create(long accountNumber, byte OffcCd, string LoanSub, int UtCd)
        {
            try
            {
                var promoterName = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));
               _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                ViewBag.AccountNumber = accountNumber;
                ViewBag.promoterName = promoterName;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.UtCd = UtCd;
                var allPromoterNames = _sessionManager.GetAllPromoterProfileList();
                {
                    ViewBag.AllPromoterNames = allPromoterNames.Select(x => new { x.PromName, x.PromoterCode }).ToList();
                }
                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.promLiabresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TblPromoterLiabDetDTO liabilityInfo)
        {
            try
            {
                List<TblPromoterLiabDetDTO> liabilityInformation = _sessionManager.GetAllPromoterLiabilityInfo();

                    TblPromoterLiabDetDTO list = new TblPromoterLiabDetDTO();

                    list.LoanAcc = liabilityInfo.LoanAcc;
                    list.LoanSub = liabilityInfo.LoanSub;
                    list.OffcCd = liabilityInfo.OffcCd;
                    list.UTCD = liabilityInfo.UTCD;
                    list.PromoterCode = liabilityInfo.PromoterCode;
                    list.LiabDesc = liabilityInfo.LiabDesc;
                    list.LiabVal = liabilityInfo.LiabVal;                    
                    list.UniqueID = Guid.NewGuid().ToString();                  
                    var allpromoterNames = _sessionManager.GetAllPromoterNames();
                    var promname = allpromoterNames.Where(x => x.Value == list.PromoterCode.ToString());
                    list.PromoterName = promname.First().Text;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    liabilityInformation.Add(list);
                    _sessionManager.SetPromoterLiabilityInfo(liabilityInformation);
                List<TblPromoterLiabDetDTO> activeinfo = liabilityInformation.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                ViewBag.AccountNumber = list.LoanAcc;
                ViewBag.LoanSub = list.LoanSub;
                ViewBag.OffcCd = list.OffcCd;
                ViewBag.UtCd = list.UTCD;
                ViewBag.InUnit = list.UTCD;
                return Json(new { isValid = true, data = liabilityInfo.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.promLiabviewPath + Constants.ViewAll, activeinfo) });
               
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
                var pomoterList = _sessionManager.GetAllPromoterLiabilityInfo();
                TblPromoterLiabDetDTO PromoterLiability = pomoterList.FirstOrDefault(x => x.UniqueID == id);
                if(PromoterLiability != null)
                {
                    //var allPromoterNames = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionPromoterNames));

                    ViewBag.LoanSub = PromoterLiability.LoanSub;
                    ViewBag.OffcCd = PromoterLiability.OffcCd;
                    ViewBag.UtCd = PromoterLiability.UTCD;
                    ViewBag.AccountNumber = PromoterLiability.LoanAcc;
                    //ViewBag.promoterName = allPromoterNames;
                }
                var allPromoterNames = _sessionManager.GetAllPromoterProfileList();
                {
                    ViewBag.AllPromoterNames = allPromoterNames.Select(x => new { x.PromName, x.PromoterCode }).ToList();
                }
                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.promLiabresultViewPath + Constants.editCs, PromoterLiability);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TblPromoterLiabDetDTO condtion)
        {
            try
            {
                List<TblPromoterLiabDetDTO> LiabilityDetails = _sessionManager.GetAllPromoterLiabilityInfo();

                TblPromoterLiabDetDTO promoterLiabilityExist = LiabilityDetails.Find(x => x.UniqueID == condtion.UniqueID);
                if (promoterLiabilityExist != null)
                {
                    LiabilityDetails.Remove(promoterLiabilityExist);                    
                    var list = promoterLiabilityExist;
                    list.LoanAcc = condtion.LoanAcc;
                    list.LoanSub = condtion.LoanSub;
                    list.OffcCd = condtion.OffcCd;
                    list.UTCD = condtion.UTCD;
                    list.LiabDesc = condtion.LiabDesc;
                    list.LiabVal = condtion.LiabVal;                    
                    list.UniqueID = Guid.NewGuid().ToString();
                    list.PromoterCode = promoterLiabilityExist.PromoterCode;
                    list.PromoterName = promoterLiabilityExist.PromoterName;
                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (promoterLiabilityExist.PromLiabId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    LiabilityDetails.Add(list);
                    _sessionManager.SetPromoterLiabilityInfo(LiabilityDetails);
                    List<TblPromoterLiabDetDTO> activeinfo = LiabilityDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.UtCd = list.UTCD;
                    ViewBag.InUnit = list.UTCD;
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.promLiabviewPath + Constants.ViewAll, activeinfo) });
                }


                ViewBag.AccountNumber = condtion.LoanAcc;
                ViewBag.LoanSub = condtion.LoanSub;
                ViewBag.OffcCd = condtion.OffcCd;
                ViewBag.UtCd = condtion.UTCD;
                ViewBag.InUnit = condtion.UTCD;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.promLiabviewPath + Constants.Edit, condtion) });
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

                IEnumerable<TblPromoterLiabDetDTO> activePromoterLiabList = new List<TblPromoterLiabDetDTO>();

                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, Id));

                var promoterLiabilityList = JsonConvert.DeserializeObject<List<TblPromoterLiabDetDTO>>(HttpContext.Session.GetString(Constants.sessionPromLiability));
                var itemToRemove = promoterLiabilityList.Find(r => r.UniqueID == Id);
                promoterLiabilityList.Remove(itemToRemove);
                var list = itemToRemove;
                list.IsDeleted = true;
                list.IsActive = false;
                list.Action = (int)Constant.Delete;
                if (list.PromLiabId == 0)
                {
                    list.LiabVal = 0;
                }
               
                promoterLiabilityList.Add(list);
                _sessionManager.SetPromoterLiabilityInfo(promoterLiabilityList);
                if (promoterLiabilityList.Count != 0)
                {
                    activePromoterLiabList = promoterLiabilityList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.UtCd = itemToRemove.UTCD;
                ViewBag.InUnit = itemToRemove.UTCD;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, Id));

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.promLiabviewPath + Constants.ViewAll, activePromoterLiabList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
