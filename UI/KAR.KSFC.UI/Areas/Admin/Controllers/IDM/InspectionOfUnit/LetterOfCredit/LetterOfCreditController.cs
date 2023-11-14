using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.InspectionOfUnit.LetterOfCredit
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class LetterOfCreditController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        public LetterOfCreditController(ILogger logger, SessionManager sessionManager)
        {

            _logger = logger;
            _sessionManager = sessionManager;
        }
        [HttpGet]
        public IActionResult Create(long accountNumber, byte OffcCd, string LoanSub)
        {
            try
            {
                var letterOfCreditList = _sessionManager.GetAllLetterOfCreditDetail();
                var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();
                if (letterOfCreditList != null)
                {
                    ViewBag.ItemNumber = letterOfCreditList.Select(x => x.DlCrdtItmNo).ToList();
                }
                ViewBag.IfscBankDetails = bankIsfcDetails;
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                return View(Constants.letterOfCreditresultViewPath + Constants.createCS);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }
        [HttpPost]
        public IActionResult Create(IdmDsbLetterOfCreditDTO model)
        {
            try
            {
                Random rand = new Random(); // <-- Make this static somewhere
                const int maxValue = 9999;
                var number = Convert.ToInt64(rand.Next(maxValue + 1).ToString("D4"));
                if (ModelState.IsValid)
                {
                    
                    List<IdmDsbLetterOfCreditDTO> letterOfCreditDetails = new();
                    if (_sessionManager.GetAllLetterOfCreditDetail() != null)
                        letterOfCreditDetails = _sessionManager.GetAllLetterOfCreditDetail();
                    IdmDsbLetterOfCreditDTO list = new IdmDsbLetterOfCreditDTO();
                    list.LoanAcc = model.LoanAcc;
                    list.OffcCd = model.OffcCd;
                    list.LoanSub = model.LoanSub;
                    list.DlCrdtItmNo = (int)number;
                    list.Action = (int)Constant.Create;
                    list.DlCrdtCrltNo = model.DlCrdtCrltNo;
                    list.DlCrdtItmDets = model.DlCrdtItmDets;
                    list.DlCrdtDt = model.DlCrdtDt;
                    list.DlCrdtSup = model.DlCrdtSup;
                    list.DlCrdtBankIfsc = model.DlCrdtBankIfsc;
                    list.DlCrdtSupAddr = model.DlCrdtSupAddr;
                    list.DlCrdtBank = model.DlCrdtBank;
                    list.DlCrdtAmt = model.DlCrdtAmt;
                    list.DlCrdtRqdt = model.DlCrdtRqdt;
                    list.DlCrdtCif = model.DlCrdtCif;
                    list.DlCrdtOpenDt = model.DlCrdtOpenDt;
                    list.DlCrdtTotalAmt = model.DlCrdtTotalAmt;
                    list.DlCrdtVdty = model.DlCrdtVdty;
                    list.DlCrdtAqrdStat = model.DlCrdtAqrdStat;
                    list.DlCrdtHondt = model.DlCrdtHondt;
                    list.DlCrdtMrgMny = model.DlCrdtMrgMny;
                    list.DlCrdtBnkadr1 = model.DlCrdtBnkadr1;
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    letterOfCreditDetails.Add(list);
                    _sessionManager.SetLetterOfCreditList(letterOfCreditDetails);
                    List<IdmDsbLetterOfCreditDTO> activeList = letterOfCreditDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.letterOfCreditviewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = model.LoanAcc;
                ViewBag.LoanSub = model.LoanSub;
                ViewBag.OffcCd = model.OffcCd;
                return Json(new { isValid = false, data = model.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.letterOfCreditviewPath + Constants.Create, model) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var allletterOfCreditList = _sessionManager.GetAllLetterOfCreditDetail();
                IdmDsbLetterOfCreditDTO letterOfCreditList = allletterOfCreditList.FirstOrDefault(x => x.UniqueId == id);

                var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();
                ViewBag.IfscBankDetails = bankIsfcDetails;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.letterOfCreditresultViewPath + Constants.ViewRecord, letterOfCreditList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult Edit(string id = "")
        {
            try
            {
                var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();
                var allletterOfCreditList = _sessionManager.GetAllLetterOfCreditDetail();
                IdmDsbLetterOfCreditDTO letterOfCreditList = allletterOfCreditList.FirstOrDefault(x => x.UniqueId == id);
                if(letterOfCreditList != null)
                {
                    ViewBag.LoanAcc = letterOfCreditList.LoanAcc;
                    ViewBag.LoanSub = letterOfCreditList.LoanSub;
                    ViewBag.OffcCd = letterOfCreditList.OffcCd;
                }

                ViewBag.IfscBankDetails = bankIsfcDetails;
                return View(Constants.letterOfCreditresultViewPath + Constants.editCs, letterOfCreditList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDsbLetterOfCreditDTO model)
        {
            try
            {
                List<IdmDsbLetterOfCreditDTO> letterOfCreditDetails = _sessionManager.GetAllLetterOfCreditDetail();
                
                IdmDsbLetterOfCreditDTO letterOfCreditExist = letterOfCreditDetails.Find(x => x.UniqueId == id);
                if (letterOfCreditExist != null)
                {
                    letterOfCreditDetails.Remove(letterOfCreditExist);
                    var list = letterOfCreditExist;
                    list.LoanAcc = model.LoanAcc;
                    list.LoanSub = model.LoanSub;
                    list.OffcCd = model.OffcCd;
                    list.DlCrdtItmNo = model.DlCrdtItmNo;
                    list.DlCrdtCrltNo = model.DlCrdtCrltNo;
                    list.DlCrdtItmDets = model.DlCrdtItmDets;
                    list.DlCrdtDt = model.DlCrdtDt;
                    list.DlCrdtSup = model.DlCrdtSup;
                    list.DlCrdtBankIfsc = model.DlCrdtBankIfsc;
                    list.DlCrdtSupAddr = model.DlCrdtSupAddr;
                    list.DlCrdtBank = model.DlCrdtBank;
                    list.DlCrdtAmt = model.DlCrdtAmt;
                    list.DlCrdtRqdt = model.DlCrdtRqdt;
                    list.DlCrdtCif = model.DlCrdtCif;
                    list.DlCrdtOpenDt = model.DlCrdtOpenDt;
                    list.DlCrdtTotalAmt = model.DlCrdtTotalAmt;
                    list.DlCrdtVdty = model.DlCrdtVdty;
                    list.DlCrdtAqrdStat = model.DlCrdtAqrdStat;
                    list.DlCrdtHondt = model.DlCrdtHondt;
                    list.DlCrdtMrgMny = model.DlCrdtMrgMny;
                    list.DlCrdtBnkadr1 = model.DlCrdtBnkadr1;
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    if (letterOfCreditExist.DcLocRowId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }
                    letterOfCreditDetails.Add(list);

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    _sessionManager.SetLetterOfCreditList(letterOfCreditDetails);
                    List<IdmDsbLetterOfCreditDTO> activeList = letterOfCreditDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                   return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.letterOfCreditviewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.LoanSub = model.LoanSub;
                ViewBag.OffcCd = model.OffcCd;
                ViewBag.AccountNumber = model.LoanAcc;

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.letterOfCreditviewPath + Constants.Edit, model) });
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
                IEnumerable<IdmDsbLetterOfCreditDTO> activeLetterOfCredit = new List<IdmDsbLetterOfCreditDTO>();

                var letterOfCreditList = JsonConvert.DeserializeObject<List<IdmDsbLetterOfCreditDTO>>(HttpContext.Session.GetString(Constants.SessionAllLetterOfCreditList));
                var itemToRemove = letterOfCreditList.Find(r => r.UniqueId == id);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                letterOfCreditList.Add(itemToRemove);

                _sessionManager.SetLetterOfCreditList(letterOfCreditList);
                if (letterOfCreditList.Count != 0)
                {
                    activeLetterOfCredit = letterOfCreditList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.letterOfCreditviewPath + Constants.ViewAll, activeLetterOfCredit) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
