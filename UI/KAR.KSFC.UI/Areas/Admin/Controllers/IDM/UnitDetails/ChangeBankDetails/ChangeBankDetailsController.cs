using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails.ChangeBankDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class ChangeBankDetailsController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        public ChangeBankDetailsController(ILogger logger, SessionManager sessionManager /*,IUnitDetailsService unitDetailsService*/)
        {

            _logger = logger;
            _sessionManager = sessionManager;
          
        }
        [HttpGet]
        public IActionResult Create(long accountNumber, byte OffcCd, string LoanSub,int InUnit)
        {
            try
            {
                var changeBankDetails = _sessionManager.GetChangeBankDetailsList();
                if (changeBankDetails != null)
                {
                    ViewBag.ItemNumber = changeBankDetails.Select(x => x.IdmUtBankRowId).ToList();
                }
                var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();
                //var IfscCodeDDL = _sessionManager.GetAllBankIFSCCodeDDL();

                var AllAccountType = _sessionManager.GetDDListAccountType();
                ViewBag.PromoterAccount = AllAccountType;

                //ViewBag.IfscCodeDDL = IfscCodeDDL;
                ViewBag.IfscBankDetails = bankIsfcDetails;
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.InUnit = InUnit;
                return View(Constants.chngbankresultViewPath + Constants.createCS);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        public IActionResult Create(IdmChangeBankDetailsDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<IdmChangeBankDetailsDTO> changeBankDetails = new();
                    if (_sessionManager.GetChangeBankDetailsList() != null)
                        changeBankDetails = _sessionManager.GetChangeBankDetailsList();
                    IdmChangeBankDetailsDTO list = new();
                    list.LoanAcc = model.LoanAcc;
                    list.OffcCd = model.OffcCd;
                    list.LoanSub = model.LoanSub;
                    list.UtCd = model.UtCd;
                    list.UtBankPhone = model.UtBankPhone;
                    list.Action = (int)Constant.Create;
                    list.UtIfsc = model.UtIfsc;
                    //var allIfscCode = _sessionManager.GetAllBankIFSCCodeDDL();
                    var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();
                    var IfscCode = bankIsfcDetails.Where(x => x.IFSCCode == model.UtIfsc.ToString());
                    list.BankIfscId = IfscCode.FirstOrDefault().IFSCRowID;
                    list.UtAccType = model.UtAccType;
                    var AllAccountType = _sessionManager.GetDDListAccountType();
                    var promoteracc = AllAccountType.Where(x => x.Value == model.UtAccType.ToString());
                    list.AccType = promoteracc.First().Text;
                    list.UtBank = model.UtBank;
                    list.UtBankBranch = model.UtBankBranch;
                    list.UtBankAddress = model.UtBankAddress;
                    list.UtBankState = model.UtBankState;
                    list.UtBankDistrict = model.UtBankDistrict;
                    list.UtBankTaluka = model.UtBankTaluka;
                    list.UtBankPincode = model.UtBankPincode;
                    list.UtBankAccno = model.UtBankAccno;
                    list.UtBankHolderName = model.UtBankHolderName;
                    list.UtBankPrimary = model.UtBankPrimary;
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    changeBankDetails.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub; 
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InUnit = list.UtCd;
                    _sessionManager.SetChangeBankDetailsList(changeBankDetails);
                    List<IdmChangeBankDetailsDTO> activeList = changeBankDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.chngbankViewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = model.LoanAcc;
                ViewBag.LoanSub = model.LoanSub;
                ViewBag.OffcCd = model.OffcCd;
                ViewBag.InUnit = model.UtCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.chngbankViewPath + Constants.Create, model) });
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
                var allChangeBankDetailsList = _sessionManager.GetChangeBankDetailsList();
                IdmChangeBankDetailsDTO changeBankDetailsList = allChangeBankDetailsList.FirstOrDefault(x => x.UniqueId == id);

                var AllAccountType = _sessionManager.GetDDListAccountType();
                ViewBag.PromoterAccount = AllAccountType;

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.chngbankresultViewPath + Constants.ViewRecord, changeBankDetailsList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        public IActionResult Edit(string id = "")
        {
            try
            {

                var allChangeBankDetailsList = _sessionManager.GetChangeBankDetailsList();
                IdmChangeBankDetailsDTO changeBankDetailsList = allChangeBankDetailsList.FirstOrDefault(x => x.UniqueId == id);
                if(changeBankDetailsList != null)
                {
                    var AccountNumber = changeBankDetailsList.LoanAcc;
                    var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();
                    //var IfscCodeDDL = _sessionManager.GetAllBankIFSCCodeDDL();
                    var AllAccountType = _sessionManager.GetDDListAccountType();
                    ViewBag.PromoterAccount = AllAccountType;
                    //ViewBag.IfscCodeDDL = IfscCodeDDL;
                    ViewBag.IfscBankDetails = bankIsfcDetails;
                    ViewBag.AccountNumber = AccountNumber;
                    ViewBag.LoanSub = changeBankDetailsList.LoanSub;
                    ViewBag.OffcCd = changeBankDetailsList.OffcCd;
                    ViewBag.InUnit = changeBankDetailsList.UtCd;
                }
                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.chngbankresultViewPath + Constants.editCs, changeBankDetailsList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmChangeBankDetailsDTO model)
        {
            try
            {

                List<IdmChangeBankDetailsDTO> changeBankDetails = _sessionManager.GetChangeBankDetailsList();

                IdmChangeBankDetailsDTO bankDetailExist = changeBankDetails.Find(x => x.UniqueId == id);
                if (bankDetailExist != null)
                {
                    changeBankDetails.Remove(bankDetailExist);
                    var list = bankDetailExist;
                    list.LoanAcc = model.LoanAcc;
                    list.OffcCd = model.OffcCd;
                    list.LoanSub = model.LoanSub;
                    list.UtBankPhone = model.UtBankPhone;
                    list.UtIfsc = model.UtIfsc;
                    //var allIfscCode = _sessionManager.GetAllBankIFSCCodeDDL();
                    var bankIsfcDetails = _sessionManager.GetIfscBankDetailsList();
                    var IfscCode = bankIsfcDetails.Where(x => x.IFSCCode == model.UtIfsc.ToString());
                    list.BankIfscId = IfscCode.FirstOrDefault().IFSCRowID;
                    list.UtCd = model.UtCd;
                    var AllAccountType = _sessionManager.GetDDListAccountType();
                    var promoteracc = AllAccountType.Where(x => x.Value == model.UtAccType.ToString());
                    list.AccType = promoteracc.First().Text;
                    list.UtAccType = model.UtAccType;
                    list.UtBank = model.UtBank;
                    list.UtBankBranch = model.UtBankBranch;
                    list.UtBankAddress = model.UtBankAddress;
                    list.UtBankState = model.UtBankState;
                    list.UtBankDistrict = model.UtBankDistrict;
                    list.UtBankTaluka = model.UtBankTaluka;
                    list.UtBankPincode = model.UtBankPincode;
                    list.UtBankAccno = model.UtBankAccno;
                    list.UtBankHolderName = model.UtBankHolderName;
                    list.UtBankPrimary = model.UtBankPrimary;
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    if (bankDetailExist.IdmUtBankRowId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    changeBankDetails.Add(list);
                    _sessionManager.SetChangeBankDetailsList(changeBankDetails);
                    List<IdmChangeBankDetailsDTO> activeList = changeBankDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InUnit = model.UtCd;
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.chngbankViewPath + Constants.ViewAll, activeList) });
                }

                ViewBag.AccountNumber = model.LoanAcc;
               
                ViewBag.LoanSub = model.LoanSub;
                ViewBag.OffcCd = model.OffcCd;
                ViewBag.InUnit = model.UtCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.chngbankViewPath + Constants.Edit, model) });
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
                IEnumerable<IdmChangeBankDetailsDTO> activeBankDetails = new List<IdmChangeBankDetailsDTO>();

                var bankDetailsList = JsonConvert.DeserializeObject<List<IdmChangeBankDetailsDTO>>(HttpContext.Session.GetString(Constants.sessionchngBankDetails));
                var itemToRemove = bankDetailsList.Find(r => r.UniqueId == Id);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                bankDetailsList.Add(itemToRemove);

                _sessionManager.SetChangeBankDetailsList(bankDetailsList);
                if (bankDetailsList.Count != 0)
                {
                    activeBankDetails = bankDetailsList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InUnit = itemToRemove.UtCd;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.chngbankViewPath + Constants.ViewAll, activeBankDetails) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
