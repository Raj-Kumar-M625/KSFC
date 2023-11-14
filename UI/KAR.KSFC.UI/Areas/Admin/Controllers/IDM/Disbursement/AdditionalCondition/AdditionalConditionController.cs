using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
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

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.Disbursement.AdditionalCondition
{

    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class AdditionalConditionController : Controller
    {

        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
       

        public AdditionalConditionController(ILogger logger, SessionManager sessionManager)
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
                var AllAdditionalConditionList = _sessionManager.GetAllAdditionalConditionList();
                AdditionConditionDetailsDTO AdditionalConditionList = AllAdditionalConditionList.First(x => x.UniqueId == id);

                var allconditionstage = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionSetCondtionStage));

                ViewBag.condtionStageMaster = allconditionstage;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.AdditionalConditionresultViewPath + Constants.ViewRecord, AdditionalConditionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }



        [HttpGet]
        public IActionResult Create(long accountNumber, int LoanSub, byte OffcCd)
        {
            try
            {

                var allconditionstage = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionSetCondtionStage));

                _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                ViewBag.LoanAcc = accountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.condtionStages = allconditionstage;
                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.AdditionalConditionresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdditionConditionDetailsDTO condtion)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  
                    List<AdditionConditionDetailsDTO> disbursementConditionDetails = new();
                    if (_sessionManager.GetAllAdditionalConditionList() != null)
                        disbursementConditionDetails = _sessionManager.GetAllAdditionalConditionList();
                    AdditionConditionDetailsDTO list = new AdditionConditionDetailsDTO();
                    list.LoanAcc = condtion.LoanAcc;
                    list.LoanSub = condtion.LoanSub;
                    list.OffcCd = condtion.OffcCd;
                    list.AddCondCode = condtion.AddCondCode;
                    list.AddCondStage = condtion.AddCondStage;
                    list.AddCondDetails = condtion.AddCondDetails;
                    list.Relaxation = condtion.Compliance == "0" ? true : false;
                    list.Compliance = condtion.Compliance;
                    list.UniqueId = Guid.NewGuid().ToString();
                    var allconditionstages = _sessionManager.GetAllCondtionStage();
                    var conddrp = allconditionstages.Where(x => x.Value == list.AddCondStage.ToString());
                    list.ConditionStage = conddrp.First().Text;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    disbursementConditionDetails.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    _sessionManager.SetAdditionalConditionList(disbursementConditionDetails);

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;

                    List<AdditionConditionDetailsDTO> activeAddConditionList = disbursementConditionDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.AdditionalConditionviewPath + Constants.ViewAll, activeAddConditionList) });
                }
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.AdditionalConditionviewPath + Constants.Create, condtion) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult Edit(int loansub, byte OffcCd, string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);
                var AllDisbursementConditionList = _sessionManager.GetAllAdditionalConditionList();
                var AccountNumber = AllDisbursementConditionList.First().LoanAcc;
             

                AdditionConditionDetailsDTO AdditionalConditionList = AllDisbursementConditionList.First(x => x.UniqueId == id);
                var allconditionstages = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionSetCondtionStage));
                AdditionalConditionList.WhRelAllowed = AdditionalConditionList.WhRelAllowed == null ? false : true;
                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                ViewBag.LoanAcc = AccountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.ConditionStages = allconditionstages;
                return View(Constants.AdditionalConditionresultViewPath + Constants.editCs, AdditionalConditionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, AdditionConditionDetailsDTO condtion)
        {
            try
            {
              
                List<AdditionConditionDetailsDTO> conditionDetails = new();
                if (_sessionManager.GetAllConditionList() != null)
                    conditionDetails = _sessionManager.GetAllAdditionalConditionList();
                AdditionConditionDetailsDTO condtionExist = conditionDetails.Find(x => x.UniqueId == id);
                if (condtionExist != null)
                {

                    conditionDetails.Remove(condtionExist);
                    var list = condtionExist;
                    list.LoanAcc = condtion.LoanAcc;
                    list.LoanSub = condtion.LoanSub;
                    list.OffcCd = condtion.OffcCd;
                    list.AddCondCode = condtion.AddCondCode;
                    list.AddCondStage = condtion.AddCondStage;
                    list.AddCondDetails = condtion.AddCondDetails;
                    list.Relaxation = condtion.Compliance == "0" ? true : false;
                    list.Compliance = condtion.Compliance;
                    list.UniqueId = condtion.UniqueId;
                    var allconditionstages = _sessionManager.GetAllCondtionStage();
                    var conddrp = allconditionstages.Where(x => x.Value == list.AddCondStage.ToString());
                    list.ConditionStage = conddrp.First().Text;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    if (condtionExist.AddCondId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    conditionDetails.Add(list);
                    _sessionManager.SetAdditionalConditionList(conditionDetails);

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    List<AdditionConditionDetailsDTO> activeAddConditionList = conditionDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();

                    return Json(new { isValid = true,data= list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.AdditionalConditionviewPath + Constants.ViewAll, activeAddConditionList) });
                }

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.AdditionalConditionviewPath + Constants.Edit, condtion) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {              
                    IEnumerable<AdditionConditionDetailsDTO> activeAddConditionList = new List<AdditionConditionDetailsDTO>();
                    _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, id));
                     var conditionList = JsonConvert.DeserializeObject<List<AdditionConditionDetailsDTO>>(HttpContext.Session.GetString(Constants.sessionAllAdditionalCondition));
                    var itemToRemove = conditionList.Find(r => r.UniqueId == id);
                    itemToRemove.IsActive = false;
                    itemToRemove.IsDeleted = true;
                    itemToRemove.Action = (int)Constant.Delete;
                    conditionList.Add(itemToRemove);
                    _sessionManager.SetAdditionalConditionList(conditionList);
                    if (conditionList.Count != 0)
                    {
                    activeAddConditionList = conditionList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    }
                    _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                    ViewBag.AccountNumber = itemToRemove.LoanAcc;
                    ViewBag.LoanSub = itemToRemove.LoanSub;
                    ViewBag.OffcCd = itemToRemove.OffcCd;
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.AdditionalConditionviewPath + Constants.ViewAll, activeAddConditionList) });

            }

            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
