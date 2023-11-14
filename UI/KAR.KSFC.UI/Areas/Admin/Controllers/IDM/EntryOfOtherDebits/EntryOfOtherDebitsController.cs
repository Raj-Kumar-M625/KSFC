using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IEntryOfOtherDebits;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using KAR.KSFC.UI.Security;
using Microsoft.AspNetCore.DataProtection;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.EntryOfOtherDebits
{
    //CreatedBy Gagana on 27/10/2022

    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class EntryOfOtherDebitsController : Controller
    {

        private readonly IEntryOfOtherDebitsService _otherDebitsService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly ICommonService _commonService;
        private readonly IIdmService _idmService;
        private readonly IDataProtector protector;

        public EntryOfOtherDebitsController(IEntryOfOtherDebitsService otherDebitsService, ILogger logger, IIdmService idmService, 
            SessionManager sessionManager, ICommonService commonService, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _otherDebitsService = otherDebitsService;
            _logger = logger;
            _sessionManager = sessionManager;
            _commonService = commonService;
            _idmService = idmService;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.AdminRouteValue);

        }
        public IActionResult Index()
        {
            var loans = _sessionManager.GetAllLoanNumber()
             .Select(e =>
             {
                 e.EncryptedLoanAcc = protector.Protect(e.LoanAcc.ToString());
                 e.EncryptedLoanUnit = protector.Protect(e.LoanUnit);
                 e.EncryptedLoanSub = protector.Protect(e.LoanSub.ToString());
                 e.EncryptedInOffc = protector.Protect(e.InOffc.ToString());
                 return e;
             });
            return View(loans);
        }

        public async Task<IActionResult> ViewAccount(string AccountNumber, string OffCd, string LoanSub, string UnitName, string loans)
        {
            long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
            byte offCd = Convert.ToByte(protector.Unprotect(OffCd));
            int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
            string unitname = protector.Unprotect(UnitName);
            try
            {
                _logger.Information(Constants.StarteddViewotherdebits);
                IdmDDLListDTO idmDTO = await _commonService.GetAllIdmDropDownList();
                _sessionManager.SetAllOtherDebitCodeDDL(idmDTO.AllOtherDebitsDetails);
              
                var allOtherDebitsCode = await _idmService.GetAllOtherDebitsDetails();
                _logger.Information(Constants.GetAllOtherDebitsList);
                var allOtherDebitsList = await _otherDebitsService.GetAllOtherDebitsList(accountNumber);

                foreach (var i in allOtherDebitsList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var otherdebitcode = allOtherDebitsCode.Where(x => x.Value == i.DsbOthdebitId);
                    i.DsbOthdebitDetails = otherdebitcode.First().Text;
                }
                _sessionManager.SetOtherDebitsList(allOtherDebitsList);

                OtherDebitsDTO otherdebits = new();
                otherdebits.OtherDebits = allOtherDebitsList.ToList();
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.loans = loans;
                ViewBag.OffCd = offCd;
                _logger.Information(Constants.CompletedViewotherdebits);
                return View(otherdebits);
            }
            catch (Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }

        }

        public IActionResult ViewRecord(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + unqid);
                var AllOtherDebitList = _sessionManager.GetAllOtherDebitsList();
                IdmOthdebitsDetailsDTO OtherDebitList = AllOtherDebitList.First(x => x.UniqueId == unqid);
                var allOtherDebitsCode = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionDDListOtherDebitCode));
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + unqid);
                ViewBag.OtherDebitCode = allOtherDebitsCode;
                return View(Constants.OtherDebitsResultViewPath + Constants.ViewRecord, OtherDebitList);

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
                var AllOtherDebitList = _sessionManager.GetAllOtherDebitsList();
                
                var allOtherDebitsCode = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionDDListOtherDebitCode));
                IdmOthdebitsDetailsDTO OtherDebitList = AllOtherDebitList.First(x => x.UniqueId == unqid);

                ViewBag.AccountNumber = OtherDebitList.LoanAcc;
                ViewBag.OtherDebitCode = allOtherDebitsCode;
                _logger.Information(CommonLogHelpers.UpdateCompleted + unqid);
                return View(Constants.OtherDebitsResultViewPath + Constants.editCs, OtherDebitList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IdmOthdebitsDetailsDTO otherdebit)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.IdmOthdebitsDetailsDTO,
               otherdebit.LoanAcc, otherdebit.LoanSub));

                List<IdmOthdebitsDetailsDTO> otherDebitDetails = new();
                List<IdmOthdebitsDetailsDTO> activeotherDebitDetails = new();
                if (_sessionManager.GetAllOtherDebitsList() != null)
                    otherDebitDetails = _sessionManager.GetAllOtherDebitsList();

                IdmOthdebitsDetailsDTO otherDebitExist = otherDebitDetails.Find(x => x.UniqueId == otherdebit.UniqueId);
           
                if (otherDebitExist != null)
                {
                    otherDebitDetails.Remove(otherDebitExist);
                   
                    var list = otherDebitExist;
                    list.DsbOthdebitId = otherdebit.DsbOthdebitId;
                    list.OthdebitAmt = otherdebit.OthdebitAmt;
                    list.OthdebitRemarks = otherdebit.OthdebitRemarks;
                    list.OthdebitTaxes = otherdebit.OthdebitTaxes;
                    var AllOtherDebitList = _sessionManager.GetAllOtherDebitCode();
                    var othreDebitlist = AllOtherDebitList.Where(x => x.Value == list.DsbOthdebitId.ToString());
                    list.DsbOthdebitDetails = othreDebitlist.First().Text;
                    list.OthdebitGst = otherdebit.OthdebitGst;
                    list.OthdebitDuedate = otherdebit.OthdebitDuedate;
                    list.OthdebitTotal = otherdebit.OthdebitTotal;

                    if (otherDebitExist.OthdebitDetId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    otherDebitDetails.Add(list);
                    _sessionManager.SetOtherDebitsList(otherDebitDetails);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;

                    if (otherDebitDetails.Count != 0)
                    {
                        activeotherDebitDetails = (otherDebitDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }

                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.IdmOthdebitsDetailsDTO,
                otherdebit.LoanAcc, otherdebit.LoanSub));
                    return Json(new { isValid = true, data = otherdebit.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.OtherDebitsviewPath + Constants.ViewAll, activeotherDebitDetails) });
                }
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.IdmOthdebitsDetailsDTO,
                 otherdebit.LoanAcc, otherdebit.LoanSub));
                ViewBag.AccountNumber = otherdebit.LoanAcc;
                ViewBag.OffCd = otherdebit.OffcCd;
                ViewBag.LoanSub = otherdebit.LoanSub;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.OtherDebitsviewPath + Constants.Edit, otherdebit) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult Delete(string unqid = "")
        {
            try
            {
                IEnumerable<IdmOthdebitsDetailsDTO> activeotherList = new List<IdmOthdebitsDetailsDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, unqid));

                if (unqid == "")
                {
                    return NotFound();
                }
                else
                {
                    var otherdebitList = _sessionManager.GetAllOtherDebitsList();
                    var itemToRemove = otherdebitList.Find(r => r.UniqueId == unqid);
                    otherdebitList.Remove(itemToRemove);
                    itemToRemove.IsActive = false;
                    itemToRemove.IsDeleted = true;
                    itemToRemove.Action = (int)Constant.Delete;
                    otherdebitList.Add(itemToRemove);
                    _sessionManager.SetOtherDebitsList(otherdebitList);
                    if (otherdebitList.Count != 0)
                    {
                        activeotherList = otherdebitList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    }
                    ViewBag.AccountNumber = itemToRemove.LoanAcc;
                    ViewBag.OffCd = itemToRemove.OffcCd;
                    ViewBag.LoanSub = itemToRemove.LoanSub;

                    _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, unqid));
                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.OtherDebitsviewPath + Constants.ViewAll, activeotherList) });
                }
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult Create(long AccountNumber, int LoanSub, byte OffCd)
        {
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);

                var allOtherDebitsCode = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionDDListOtherDebitCode));
                ViewBag.LoanAcc = AccountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffCd;
                ViewBag.OtherDebitCode = allOtherDebitsCode;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.OtherDebitsResultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmOthdebitsDetailsDTO otherdebit)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.CreateStartedPost + LogAttribute.IdmOthdebitsDetailsDTO,
               otherdebit.LoanAcc, otherdebit.LoanSub));

                if (ModelState.IsValid)
                {
                    List<IdmOthdebitsDetailsDTO> otherDebitDetails = new();
                    List<IdmOthdebitsDetailsDTO> activeotherDebitDetails = new();
                    if (_sessionManager.GetAllOtherDebitsList() != null)
                        otherDebitDetails = _sessionManager.GetAllOtherDebitsList();

                    IdmOthdebitsDetailsDTO list = new IdmOthdebitsDetailsDTO();

                    list.LoanAcc = otherdebit.LoanAcc;
                    list.LoanSub = otherdebit.LoanSub;
                    list.OffcCd = otherdebit.OffcCd;
                    list.DsbOthdebitId = otherdebit.DsbOthdebitId;
                    list.OthdebitAmt = otherdebit.OthdebitAmt;
                    list.OthdebitRemarks = otherdebit.OthdebitRemarks;
                    list.OthdebitTaxes = otherdebit.OthdebitTaxes;
                    var AllOtherDebitList = _sessionManager.GetAllOtherDebitCode();
                    var othreDebitlist = AllOtherDebitList.Where(x => x.Value == list.DsbOthdebitId.ToString());
                    list.DsbOthdebitDetails = othreDebitlist.First().Text;
                    list.OthdebitGst = otherdebit.OthdebitGst;
                    list.OthdebitDuedate = otherdebit.OthdebitDuedate;
                    list.OthdebitTotal = otherdebit.OthdebitTotal;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    otherDebitDetails.Add(list);
                    _sessionManager.SetOtherDebitsList(otherDebitDetails);
                    if (otherDebitDetails.Count != 0)
                    {
                        activeotherDebitDetails = (otherDebitDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }

                    _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + LogAttribute.IdmOthdebitsDetailsDTO,
               otherdebit.LoanAcc, otherdebit.LoanSub));

                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.AccountNumber = list.LoanAcc;
                    return Json(new { isValid = true, data = otherdebit.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.OtherDebitsviewPath + Constants.ViewAll, activeotherDebitDetails) });
                }
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.IdmOthdebitsDetailsDTO,
               otherdebit.LoanAcc, otherdebit.LoanSub));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.loanAllocationviewPath + Constants.Create, otherdebit) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult SubmitOtherDebitsDetails()
        {
            try
            {
                List<IdmOthdebitsDetailsDTO> otherDebitDetails = new();
                 otherDebitDetails = _sessionManager.GetAllOtherDebitsList();
              
                foreach (var item in otherDebitDetails)
                {
                    item.IsSubmitted = true;
                }
                _sessionManager.SetOtherDebitsList(otherDebitDetails);
                return RedirectToAction("SaveOtherDebitsDetails");
            }
           
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        

        [HttpPost]
        public async Task<IActionResult> SaveOtherDebitsDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveOtherDebitDetails);
                if (_sessionManager.GetAllOtherDebitsList().Count != 0)
                {
                    var OtherDebitDetailsList = _sessionManager.GetAllOtherDebitsList();

                    foreach (var item in OtherDebitDetailsList)
                    {
                        switch (item.Action)
                            {
                                case (int)Constant.Delete:
                                    await _otherDebitsService.DeleteOtherDebitDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedOtherDebitDetailsDelete);
                                    break;
                                case (int)Constant.Update:
                                    await _otherDebitsService.UpdateOtherDebitDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedOtherDebitDetailsUpdate);
                                    break;
                                case (int)Constant.Create:
                                    await _otherDebitsService.CreateOtherDebitDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedOtherDebitDetailsCreate);
                                    break;
                                default:
                                    await _otherDebitsService.SubmitOtherDebitDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedOtherDebitDetailsSubmit);
                                    break;
                            }
                     
                       
                        

                    }
                    
                    _logger.Information(CommonLogHelpers.CompletedSaveOtherDebitDetails);

                    return Json(new { isValid = true });
                }
                return Json(new { isValid = true });
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
    }
}
