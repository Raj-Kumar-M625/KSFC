using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.Disbursement.Form8AndForm13
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class Form8AndForm13Controller : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;


        public Form8AndForm13Controller(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var allForm8AndForm13List = _sessionManager.GetForm8AndForm13List();
                Form8AndForm13DTO form8AndForm13list = allForm8AndForm13List.First(x => x.UniqueId == id);
                var allFormtypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionForm8andForm13Master));
                ViewBag.Form8and13list = allFormtypes;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);


                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == form8AndForm13list.DF813Id && x.SubModuleType == Constants.Form8and13Condition && x.MainModule == Constants.DisbursementCondition).ToList();

                ViewBag.Documentlist = doc;

                return View(Constants. Form8AndForm13resultViewPath +  Constants.ViewRecord, form8AndForm13list);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
      
        [HttpGet]
        public IActionResult Create(long accountNumber, int loansub, byte offcCd, int InUnit)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                
                var allFormtypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionForm8andForm13Master));
                ViewBag.Form8and13list = allFormtypes;
                ViewBag.LoanAcc = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.OffcCd = offcCd;
                ViewBag.InUnit = InUnit;
                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.Form8AndForm13resultViewPath +  Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Form8AndForm13DTO form8AndForm13)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    List<Form8AndForm13DTO> form8AndForm13Details = new();
                    if (_sessionManager.GetForm8AndForm13List() != null)
                        form8AndForm13Details = _sessionManager.GetForm8AndForm13List();
                    Form8AndForm13DTO list = new Form8AndForm13DTO();
                    list.DF813LoanAcc = form8AndForm13.DF813LoanAcc;
                    list.DF813Sno = form8AndForm13.DF813Sno;
                    list.DF813Offc = form8AndForm13.DF813Offc;
                    list.DF813Unit = form8AndForm13.DF813Unit;
                    list.DF813t1 = form8AndForm13.DF813t1;
                    list.DF813RqDt = form8AndForm13.DF813RqDt;
                    list.DF813Dt = form8AndForm13.DF813Dt;
                    list.DF813Ref = form8AndForm13.DF813Ref;                    
                    list.DF813cc = form8AndForm13.DF813cc;
                    list.DF813a1 = form8AndForm13.DF813a1;
                    list.UniqueId = Guid.NewGuid().ToString();                    
                    list.CreatedDate = form8AndForm13.CreatedDate;
                    var allformTypes = _sessionManager.GetAllForm8AndForm13Master();
                    var formType = allformTypes.Where(x => x.Value == list.DF813t1.ToString());
                    list.FormType = formType.First().Text;

                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    form8AndForm13Details.Add(list);
                   
                    _sessionManager.SetForm8AndForm13List(form8AndForm13Details);
                    ViewBag.AccountNumber = list.DF813LoanAcc;
                    ViewBag.LoanSub = list.DF813Sno;
                    ViewBag.OffcCd = list.DF813Offc;
                    ViewBag.InUnit = list.DF813Unit;
                    List<Form8AndForm13DTO> activeList = form8AndForm13Details.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    return Json(new { isValid = true,data=list.DF813LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.Form8AndForm13viewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = form8AndForm13.DF813LoanAcc;
                ViewBag.LoanSub = form8AndForm13.DF813Sno;
                ViewBag.OffcCd = form8AndForm13.DF813Offc;
                ViewBag.InUnit = form8AndForm13.DF813Unit;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.Form8AndForm13viewPath + Constants.Create, form8AndForm13) });
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
                var allForm8AndForm13List = _sessionManager.GetForm8AndForm13List();

                Form8AndForm13DTO form8AndForm13list = allForm8AndForm13List.First(x => x.UniqueId == id);
                if (form8AndForm13list != null)
                {
                    var allFormtypes = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionForm8andForm13Master));
                    ViewBag.Form8and13list = allFormtypes;
                    _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                    ViewBag.LoanAcc = form8AndForm13list.DF813LoanAcc;
                    ViewBag.LoanSub = form8AndForm13list.DF813Sno;
                    ViewBag.OffcCd = form8AndForm13list.DF813Offc;
                    ViewBag.InUnit = form8AndForm13list.DF813Unit;
                    var doclist = _sessionManager.GetIDMDocument();
                    var doc = doclist.Where(x => x.SubModuleId == form8AndForm13list.DF813Id && x.SubModuleType == Constants.Form8and13Condition && x.MainModule == Constants.DisbursementCondition).ToList();

                    ViewBag.Documentlist = doc;
                    ViewBag.SubModuleId = Convert.ToInt32(form8AndForm13list.DF813Id);
                    ViewBag.SubModuleType = Constants.Form8and13Condition;
                    ViewBag.MainModule = Constants.DisbursementCondition;

                }
                return View(Constants.Form8AndForm13resultViewPath + Constants.editCs, form8AndForm13list);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Form8AndForm13DTO form8AndForm13)
        {
            try
            {
             
                List<Form8AndForm13DTO> form8AndForm13Details = new();
                if (_sessionManager.GetForm8AndForm13List() != null)
                    form8AndForm13Details = _sessionManager.GetForm8AndForm13List();
                Form8AndForm13DTO form8AndForm13Exist = form8AndForm13Details.Find(x => x.UniqueId == id);
                if (form8AndForm13Exist != null)
                {

                    form8AndForm13Details.Remove(form8AndForm13Exist);
                    var list = form8AndForm13Exist;
                    list.DF813LoanAcc = form8AndForm13.DF813LoanAcc;
                    list.DF813Sno = form8AndForm13.DF813Sno;
                    list.DF813Offc = form8AndForm13.DF813Offc;
                    list.DF813Unit = form8AndForm13.DF813Unit;
                    list.DF813t1 = form8AndForm13.DF813t1;
                    list.DF813Dt = form8AndForm13.DF813Dt;
                    list.DF813RqDt = form8AndForm13.DF813RqDt;
                    list.DF813Ref = form8AndForm13.DF813Ref;                    
                    list.DF813cc = form8AndForm13.DF813cc;
                    list.DF813a1 = form8AndForm13.DF813a1;
                    list.UniqueId = form8AndForm13.UniqueId;
                    var allformTypes = _sessionManager.GetAllForm8AndForm13Master();
                    var formType = allformTypes.Where(x => x.Value == list.DF813t1.ToString());
                    list.FormType = formType.First().Text;

                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.DF813Sno = form8AndForm13.DF813Sno;
                    list.CreatedDate = form8AndForm13.CreatedDate;
                    if (form8AndForm13Exist.DF813Id > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    form8AndForm13Details.Add(list);
                    _sessionManager.SetForm8AndForm13List(form8AndForm13Details);
                    List<Form8AndForm13DTO> activeList = form8AndForm13Details.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    ViewBag.AccountNumber = form8AndForm13.DF813LoanAcc;
                    ViewBag.LoanSub = form8AndForm13.DF813Sno;
                    ViewBag.OffcCd = form8AndForm13.DF813Offc;
                    ViewBag.InUnit = form8AndForm13.DF813Unit;
                    return Json(new { isValid = true, data = list.DF813LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.Form8AndForm13viewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = form8AndForm13.DF813LoanAcc;
                ViewBag.LoanSub = form8AndForm13.DF813Sno;
                ViewBag.OffcCd = form8AndForm13.DF813Offc;
                ViewBag.InUnit = form8AndForm13.DF813Unit;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.Form8AndForm13viewPath + Constants.Edit, form8AndForm13) });
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
                IEnumerable<Form8AndForm13DTO> activeForm8List = new List<Form8AndForm13DTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, id));
                var form8AndForm13List = JsonConvert.DeserializeObject<List<Form8AndForm13DTO>>(HttpContext.Session.GetString(Constants.SessionSetForm8AndForm13));
                var itemToRemove = form8AndForm13List.Find(r => r.UniqueId == id);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                form8AndForm13List.Add(itemToRemove);
                _sessionManager.SetForm8AndForm13List(form8AndForm13List);
                if (form8AndForm13List.Count != 0)
                {
                    activeForm8List = form8AndForm13List.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }

                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                ViewBag.AccountNumber = itemToRemove.DF813LoanAcc;
                ViewBag.LoanSub = itemToRemove.DF813Sno;
                ViewBag.OffcCd = itemToRemove.DF813Offc;
                ViewBag.InUnit = itemToRemove.DF813Unit;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.Form8AndForm13viewPath + Constants.ViewAll, activeForm8List) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}