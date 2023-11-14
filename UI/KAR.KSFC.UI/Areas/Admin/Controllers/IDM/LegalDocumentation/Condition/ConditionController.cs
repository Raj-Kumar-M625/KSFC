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

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.LegalDocumentation.Condition
{
    /// <summary>
    ///  Author: Sandeep/Gagana; Module: Condition; Date:03/08/2022
    /// </summary>
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class ConditionController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
     
        public ConditionController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult ViewRecord(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + unqid);
                var AllConditionList = _sessionManager.GetAllConditionList();
              

                var allconditiontypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionConditonType));
                var allconditionstages = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionCondtionStage));
                var allconditiondescriptions = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionConditionDes));
                LDConditionDetailsDTO ConditionList = AllConditionList.Find(x => x.UniqueId == unqid);
                var condlist = allconditiondescriptions.Where(x => x.Text == ConditionList.CondDetails);
                if (condlist.Any())
                {
                    ConditionList.CondDetails = condlist.First().Value;
                }
               
                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == ConditionList.CondDetId && x.SubModuleType == Constants.Condition && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.MainModule = Constants.LegalDocumentation;
                ViewBag.Documentlist = doc;

                ViewBag.ConditionTypes = allconditiontypes;
                ViewBag.ConditionStages = allconditionstages;
                ViewBag.ConditionDescriptions = allconditiondescriptions;
                ViewBag.SubModuleId = ConditionList.CondDetId;
                ViewBag.SubModuleType = Constants.Condition;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + unqid);
                return View(Constants.conditionresultViewPath + Constants.ViewRecord, ConditionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult Edit(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + unqid);
                var AllConditionList = _sessionManager.GetAllConditionList();
                
                var allconditiontypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionConditonType));
                var allconditionstages = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionCondtionStage));
                var allconditiondescriptions = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionConditionDes));

                LDConditionDetailsDTO ConditionList = AllConditionList.Find(x => x.UniqueId == unqid);
                var condlist = allconditiondescriptions.Where(x => x.Text == ConditionList.CondDetails);
                if (condlist.Any())
                {
                    ConditionList.CondDetails = condlist.First().Value;
                }
              
                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == ConditionList.CondDetId && x.SubModuleType == Constants.Condition && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.Documentlist = doc;
                ViewBag.SubModuleId = ConditionList.CondDetId;
                ViewBag.SubModuleType = Constants.Condition;
                ViewBag.MainModule = Constants.LegalDocumentation;
                ViewBag.MainModule =Constants.LegalDocumentation;
                ViewBag.AccountNumber = ConditionList.LoanAcc;
                ViewBag.ConditionTypes = allconditiontypes;
                ViewBag.ConditionStages = allconditionstages;
                ViewBag.ConditionDescriptions = allconditiondescriptions;
                _logger.Information(CommonLogHelpers.UpdateCompleted + unqid);
                return View(Constants.conditionresultViewPath + Constants.editCs, ConditionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LDConditionDetailsDTO condtion)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.CondtionDto,
                 condtion.CondDetId, condtion.ConditionType, condtion.CondDetails, condtion.ConditionStage));

                List<LDConditionDetailsDTO> conditionDetails = new();
                List<LDConditionDetailsDTO> activeconditionDetails = new();
                if (_sessionManager.GetAllConditionList() != null)
                    conditionDetails = _sessionManager.GetAllConditionList();

                LDConditionDetailsDTO condtionExist = conditionDetails.Find(x => x.UniqueId == condtion.UniqueId);
                if (condtionExist != null)
                {

                    conditionDetails.Remove(condtionExist);
                    var list = condtionExist;
                    list.LoanAcc = condtion.LoanAcc;
                    list.LoanSub = condtion.LoanSub;
                    list.OffcCd = condtion.OffcCd;
                    list.UniqueId = condtion.UniqueId;
                    list.CondType = condtion.CondType;
                    list.CondStg = condtion.CondStg;
                    list.Compliance = condtion.Compliance;

                    var condList = _sessionManager.GetAllConditionDescription();
                    var conddetails = condList.Where(x => x.Value == condtion.CondDetails);
                    if (conddetails.Any())
                    {
                        list.CondDetails = conddetails.First().Text;
                    }
                    else
                    {
                        list.CondDetails = condtion.CondDetails;
                    }

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

                    var allconditionstages = _sessionManager.GetAllCondtionStage();
                    var conddrp = allconditionstages.Find(x => x.Value == list.CondStg.ToString());
                    list.ConditionStage = conddrp.Text;

                    conditionDetails.Add(list);
                    _sessionManager.SetConditionList(conditionDetails);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;

                    if (conditionDetails.Count != 0)
                    {
                        activeconditionDetails = (conditionDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }

                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.CondtionDto,
                    condtion.CondDetId, condtion.ConditionType, condtion.CondDetails, condtion.ConditionStage));
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.conditionviewPath + Constants.ViewAll, activeconditionDetails) });
                }
                ViewBag.AccountNumber = condtion.LoanAcc;
                ViewBag.OffCd = condtion.OffcCd;
                ViewBag.LoanSub = condtion.LoanSub;

                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.CondtionDto,
                condtion.CondDetId, condtion.ConditionType, condtion.CondDetails, condtion.ConditionStage));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.conditionviewPath + Constants.Edit, condtion) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        public IActionResult Create(long AccountNumber, int LoanSub, byte OffCd)
        {
            try
            {
               
                var allconditiontypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionConditonType));
                var allconditionstages = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionCondtionStage));
                var allconditiondescriptions = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionConditionDes));
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                ViewBag.LoanAcc = AccountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffCd;
                ViewBag.ConditionTypes = allconditiontypes;
                ViewBag.ConditionStages = allconditionstages;
                ViewBag.ConditionDescriptions = allconditiondescriptions;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.conditionresultViewPath +Constants.createCS);

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
                _logger.Information(string.Format(CommonLogHelpers.CreateStartedPost + LogAttribute.CondtionDto,
                  condtion.CondDetId, condtion.ConditionType, condtion.CondDetails, condtion.ConditionStage));

                if (ModelState.IsValid)
                {
                    List<LDConditionDetailsDTO> conditionDetails = new();
                    List<LDConditionDetailsDTO> activeconditionDetails = new();
                    if (_sessionManager.GetAllConditionList() != null)
                        conditionDetails = _sessionManager.GetAllConditionList();

                    LDConditionDetailsDTO list = new LDConditionDetailsDTO();

                    list.LoanAcc = condtion.LoanAcc;
                    list.LoanSub = condtion.LoanSub;
                    list.OffcCd = condtion.OffcCd;
                    list.CondType = condtion.CondType;
                    list.Compliance = condtion.Compliance;
                    list.CondStg = condtion.CondStg;
                    list.UniqueId = Guid.NewGuid().ToString();
                    
                   
                    var condList = _sessionManager.GetAllConditionDescription();
                    var conddetails = condList.Where(x => x.Value == condtion.CondDetails);
                    if(conddetails.Any())
                    {
                        list.CondDetails = conddetails.First().Text;
                    }
                    else
                    {
                        list.CondDetails = condtion.CondDetails;
                    }
                   
                    var allconditionstages = _sessionManager.GetAllCondtionStage();
                    var conddrp = allconditionstages.Find(x => x.Value == list.CondStg.ToString());
                    list.ConditionStage = conddrp.Text;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    conditionDetails.Add(list);
                    _sessionManager.SetConditionList(conditionDetails);
                    if (conditionDetails.Count != 0)
                    {
                        activeconditionDetails = (conditionDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }

                    _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + LogAttribute.CondtionDto,
                          condtion.CondDetId, condtion.ConditionType, condtion.CondDetails, condtion.ConditionStage));

                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.AccountNumber = list.LoanAcc;
                    return Json(new { isValid = true, data = condtion.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.conditionviewPath +  Constants.ViewAll, activeconditionDetails) });
                }
                ViewBag.AccountNumber = condtion.LoanAcc;
                ViewBag.OffCd = condtion.OffcCd;
                ViewBag.LoanSub = condtion.LoanSub;

                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.CondtionDto,
                  condtion.CondDetId, condtion.ConditionType, condtion.CondDetails, condtion.ConditionStage));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.conditionviewPath + Constants.Create, condtion) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

      
        [HttpPost]
        public IActionResult Delete(string Id)
        {
            try
            {
                IEnumerable<LDConditionDetailsDTO> activeConditionList = new List<LDConditionDetailsDTO>();

                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, Id));

                var conditionList = JsonConvert.DeserializeObject<List<LDConditionDetailsDTO>>(HttpContext.Session.GetString(Constants.sessionConditonList));
                var itemToRemove = conditionList.Find(r => r.UniqueId == Id);
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;
                itemToRemove.Action = (int)Constant.Delete;
                conditionList.Add(itemToRemove);
                _sessionManager.SetConditionList(conditionList);
                if (conditionList.Count != 0)
                {
                    activeConditionList = conditionList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, Id));
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.OffCd = itemToRemove.OffcCd;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.conditionviewPath +  Constants.ViewAll, activeConditionList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}
