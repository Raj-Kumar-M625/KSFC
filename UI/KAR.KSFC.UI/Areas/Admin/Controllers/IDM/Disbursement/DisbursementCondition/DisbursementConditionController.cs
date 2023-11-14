using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
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

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.Disbursement.DisbursementCondition
{
    /// <summary>
    /// Author: Manoj
    /// Date: 18/08/2022    
    /// Module: Disbursement Condition
    /// </summary>
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class DisbursementConditionController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        public DisbursementConditionController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllDisbursementConditionList = _sessionManager.GetAllConditionList();
                LDConditionDetailsDTO disbursementConditionList = AllDisbursementConditionList.FirstOrDefault(x => x.UniqueId == id);

                var allconditiontypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionCodntionType));
                var allconditionstages = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionConditionStage));
                var allconditiondescriptions = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionConditionDesc));
                ViewBag.ConditionTypes = allconditiontypes;
                ViewBag.ConditionStages = allconditionstages;
                ViewBag.ConditionDescriptions = allconditiondescriptions;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == disbursementConditionList.CondDetId && x.SubModuleType == Constants.DisbursementCondition && x.MainModule == Constants.DisbursementCondition).ToList();

                ViewBag.Documentlist = doc;
               
                return View(Constants.DisbursementConditionresultViewPath + Constants.ViewRecord, disbursementConditionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

       
        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                IEnumerable<LDConditionDetailsDTO> activeDisConditionList = new List<LDConditionDetailsDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, id));
                var conditionList = JsonConvert.DeserializeObject<List<LDConditionDetailsDTO>>(HttpContext.Session.GetString(Constants.sessionConditonList));
                var itemToRemove = conditionList.Find(r => r.UniqueId == id);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                conditionList.Add(itemToRemove);
                _sessionManager.SetConditionList(conditionList);
                if (conditionList.Count != 0)
                {
                    activeDisConditionList = conditionList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }             
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.DisbursementConditionviewPath + Constants.ViewAll, activeDisConditionList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        public IActionResult Edit(int loansub, byte offccd, string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);
                var AllDisbursementConditionList = _sessionManager.GetAllConditionList();

                LDConditionDetailsDTO DisbursementConditionList = AllDisbursementConditionList.First(x => x.UniqueId == id);

                var allconditiontypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionCodntionType));
                var allconditionstages = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionConditionStage));
                var allconditiondescriptions = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionConditionDesc));

                _logger.Information(CommonLogHelpers.UpdateCompleted + id);

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == DisbursementConditionList.CondDetId && x.SubModuleType == Constants.DisbursementCondition && x.MainModule == Constants.DisbursementCondition).ToList();

                ViewBag.Documentlist = doc;
                ViewBag.SubModuleId = DisbursementConditionList.CondDetId;
                ViewBag.SubModuleType = Constants.DisbursementCondition;
                ViewBag.MainModule = Constants.DisbursementCondition;
                ViewBag.LoanAcc = DisbursementConditionList.LoanAcc;
                ViewBag.LoanSub = DisbursementConditionList.LoanSub;
                ViewBag.OffcCd = DisbursementConditionList.OffcCd;
                ViewBag.ConditionTypes = allconditiontypes;
                ViewBag.ConditionStages = allconditionstages;
                ViewBag.ConditionDescriptions = allconditiondescriptions;
                DisbursementConditionList.WhRelAllowed = DisbursementConditionList.WhRelAllowed == null ? false : true;

                return View(Constants.DisbursementConditionresultViewPath + Constants.editCs, DisbursementConditionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, LDConditionDetailsDTO condtion)
        {
            try
            {
               
                List<LDConditionDetailsDTO> conditionDetails = new();
                if (_sessionManager.GetAllConditionList() != null)
                    conditionDetails = _sessionManager.GetAllConditionList();
                LDConditionDetailsDTO condtionExist = conditionDetails.Find(x => x.UniqueId == id);
                if (condtionExist != null)
                {

                    conditionDetails.Remove(condtionExist);
                    var list = condtionExist;
                    list.LoanAcc = condtion.LoanAcc;
                    list.LoanSub = condtion.LoanSub;
                    list.OffcCd = condtion.OffcCd;
                    list.Compliance = condtion.Compliance;
                    list.CondType = condtion.CondType;
                    list.CondStg = condtion.CondStg;
                    list.CondDetails = condtion.CondDetails;
                    list.UniqueId = condtion.UniqueId;
                    list.CondRemarks = condtion.CondRemarks;
                    list.WhRelaxation = list.Compliance == "0" ? true : false;
                    var allconditionstages = _sessionManager.GetAllCondtionStage();
                    var allconditionType = _sessionManager.GetAllConditionType();
                    var conType = allconditionType.Where(x => x.Value == list.CondType.ToString());

                    var conddrp = allconditionstages.Where(x => x.Value == list.CondStg.ToString());

                    list.ConditionStage = conddrp.First().Text;
                    list.ConditionType = conType.First().Text;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    if (condtionExist.CondDetId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = condtion.LoanSub;
                    ViewBag.OffcCd = condtion.OffcCd;
                    conditionDetails.Add(list);
                    _sessionManager.SetConditionList(conditionDetails);
                    List<LDConditionDetailsDTO> activeDisConditionList = conditionDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();

                    return Json(new { isValid = true, data=list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.DisbursementConditionviewPath + Constants.ViewAll, activeDisConditionList) });
                }
                ViewBag.AccountNumber = condtion.LoanAcc;

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.DisbursementConditionviewPath + Constants.Edit, condtion) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult Create(long accountNumber, int loansub, byte offccd)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                

                var allconditiontypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDListConditionType"));
                var allconditionstages = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionSetCondtionStage"));
                var allconditiondescriptions = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDListConditionDescriptions"));
                
                ViewBag.LoanAcc = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.OffcCd = offccd;
                ViewBag.ConditionTypes = allconditiontypes;
                ViewBag.ConditionStages = allconditionstages;
                ViewBag.ConditionDescriptions = allconditiondescriptions;
                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.DisbursementConditionresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LDConditionDetailsDTO condtion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    List<LDConditionDetailsDTO> disbursementConditionDetails = new();
                    if (_sessionManager.GetAllConditionList() != null)
                        disbursementConditionDetails = _sessionManager.GetAllConditionList();
                    LDConditionDetailsDTO list = new LDConditionDetailsDTO();
                    list.LoanAcc = condtion.LoanAcc;
                    list.LoanSub = condtion.LoanSub;
                    list.OffcCd = condtion.OffcCd;
                    list.Compliance = condtion.Compliance;
                    list.CondType = condtion.CondType;
                    list.CondStg = condtion.CondStg;
                    list.CondDetails = condtion.CondDetails;
                    list.CondRemarks = condtion.CondRemarks;
                    list.WhRelaxation = condtion.WhRelaxation;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.CondDetails = condtion.CondDetails;
                    var allconditionstages = _sessionManager.GetAllCondtionStage();
                    var allconditionType = _sessionManager.GetAllConditionType();
                    var conType = allconditionType.Where(x => x.Value == list.CondType.ToString());


                    var conddrp = allconditionstages.Where(x => x.Value == list.CondStg.ToString());


                    list.ConditionStage = conddrp.First().Text;
                    list.ConditionType = conType.First().Text;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    disbursementConditionDetails.Add(list);
                    _sessionManager.SetConditionList(disbursementConditionDetails);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = condtion.LoanSub;
                    ViewBag.OffcCd = condtion.OffcCd;
                    List<LDConditionDetailsDTO> activeDisConditionList = disbursementConditionDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.DisbursementConditionviewPath + Constants.ViewAll, activeDisConditionList) });
                }
                ViewBag.AccountNumber = condtion.LoanAcc;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.DisbursementConditionviewPath + Constants.Create, condtion) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}