using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Security;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ILoanAllocationService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.LoanAllocation
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    /// <summary>
    ///  Author: Gagana; Module: LoanAllocation; Date:29/09/2022
    /// </summary>

    public class LoanAllocationController : Controller
    {
        private readonly ILoanAllocationService _loanAllocationService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly ICommonService _commonService;
        private readonly IIdmService _idmService;
        private readonly IDataProtector protector;


        public LoanAllocationController(ILoanAllocationService loanAllocationService, ILogger logger, SessionManager sessionManager, 
            IIdmService idmService, ICommonService commonService, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _loanAllocationService = loanAllocationService;
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

        public async Task<IActionResult> ViewAccount(string AccountNumber, string OffCd, string LoanSub, string UnitName)
        {
            long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
            byte offCd = Convert.ToByte(protector.Unprotect(OffCd));
            int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
            string unitname = protector.Unprotect(UnitName);
            try
            {
                _logger.Information(CommonLogHelpers.LoanAllocationviewAccount);

               
                IdmDDLListDTO idmDTO = await _commonService.GetAllIdmDropDownList();
                foreach (var i in idmDTO.GetAllAllocationCode)
                {
                    i.Text = i.Value.ToString() + " - " + i.Text;
                }
                _sessionManager.SetAllAllocationCodeDDL(idmDTO.GetAllAllocationCode);

                _logger.Information(CommonLogHelpers.GetAllocationCodes);
                var allAllocationCodes = await _idmService.GetAllocationCodes();

                // _logger.Information(CommonLogHelpers.GetAllLoanAllocationList);
                //var allLoanAllocationList = await _loanAllocationService.GetAllLoanAllocationList(accountNumber);
                //foreach (var i in allLoanAllocationList)
                //{
                //    if (i.UniqueId == null)
                //    {
                //        i.UniqueId = Guid.NewGuid().ToString();
                //    }
                //    var allcCode = allAllocationCodes.Where(x => x.Value == i.DcalcCd);
                //    i.DcalcCode = i.DcalcCd + " - " + allcCode.First().Text;
                //}
                //_sessionManager.SetAllLoanAllocationList(allLoanAllocationList);

                LoanAllocationDTO loanAllocationDTO = new();
                //loanAllocationDTO.LoanAllocationDetails = allLoanAllocationList.ToList();

                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.OffCd = offCd;
                return View(loanAllocationDTO);
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
                var AllLoanAllocationList = _sessionManager.GetAllLoanAllocationList();
                TblIdmDhcgAllcDTO LoanAllocationList = AllLoanAllocationList.FirstOrDefault(x => x.UniqueId == unqid);
                var allAllocationCode = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDListAllocationCode"));
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + unqid);
                ViewBag.AllocationCodes = allAllocationCode;

                return View(Constants.loanAllocationresultViewPath + Constants.ViewRecord, LoanAllocationList);

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
                var AllAllocationList = _sessionManager.GetAllLoanAllocationList();
                var AccountNumber = AllAllocationList.First().LoanAcc;
                var allAllocationCode = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDListAllocationCode"));

                TblIdmDhcgAllcDTO AllocationList = AllAllocationList.FirstOrDefault(x => x.UniqueId == unqid);

                ViewBag.AccountNumber = AccountNumber;
                ViewBag.AllocationCodes = allAllocationCode;
                _logger.Information(CommonLogHelpers.UpdateCompleted + unqid);
                return View(Constants.loanAllocationresultViewPath + Constants.editCs, AllocationList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TblIdmDhcgAllcDTO allocation)
        {
            try
            {
                 _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.LoanAllocataionDto,
                allocation.DcalcCd, allocation.DcalcDetails, allocation.DcalcRqdt, allocation.DcalcComdt));

                List<TblIdmDhcgAllcDTO> allocationDetails = new();
                List<TblIdmDhcgAllcDTO> activeallocationDetails = new();
                if (_sessionManager.GetAllLoanAllocationList() != null)
                    allocationDetails = _sessionManager.GetAllLoanAllocationList();

                TblIdmDhcgAllcDTO allocationExist = allocationDetails.Find(x => x.UniqueId == allocation.UniqueId);
              
                if (allocationExist != null)
                {
                   
                    allocationDetails.Remove(allocationExist);
                  
                    var list = allocationExist;
                    list.DcalcAmtRevised = allocation.DcalcAmtRevised;
                    list.DcalcComdt = allocation.DcalcComdt;
                    var allAllocationCodes = _sessionManager.GetAllAllocationCode();
                    var allcCode = allAllocationCodes.Where(x => x.Value == list.DcalcCd.ToString());
                    list.DcalcCode = allcCode.First().Text;

                    if (allocationExist.DcalcId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    allocationDetails.Add(list);
                    _sessionManager.SetAllLoanAllocationList(allocationDetails);
                    ViewBag.AccountNumber = allocation.LoanAcc;
                    ViewBag.OffCd = allocation.OffcCd;
                    ViewBag.LoanSub = allocation.LoanSub;

                    if (allocationDetails.Count != 0)
                    {
                        activeallocationDetails = (allocationDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }

                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.LoanAllocataionDto,
                allocation.DcalcCd, allocation.DcalcDetails, allocation.DcalcRqdt, allocation.DcalcComdt));
                    return Json(new { isValid = true, data = allocation.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.loanAllocationviewPath + Constants.ViewAll, activeallocationDetails) });
                }
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.LoanAllocataionDto,
                allocation.DcalcCd, allocation.DcalcDetails, allocation.DcalcRqdt, allocation.DcalcComdt));

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.loanAllocationviewPath + Constants.Edit, allocation) });
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

                var allAllocationCode = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDListAllocationCode"));

                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                ViewBag.LoanAcc = AccountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffCd;
                ViewBag.AllocationCodes = allAllocationCode;
                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.loanAllocationresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TblIdmDhcgAllcDTO allocation)
        {
            try
            {
               _logger.Information(string.Format(CommonLogHelpers.CreateStartedPost + LogAttribute.LoanAllocataionDto,
                  allocation.DcalcCd, allocation.DcalcDetails, allocation.DcalcRqdt, allocation.DcalcComdt));

                if (ModelState.IsValid)
                {
                    List<TblIdmDhcgAllcDTO> allocationDetails = new();
                    List<TblIdmDhcgAllcDTO> activeallocationDetails = new();
                    if (_sessionManager.GetAllLoanAllocationList() != null)
                        allocationDetails = _sessionManager.GetAllLoanAllocationList();

                    TblIdmDhcgAllcDTO list = new TblIdmDhcgAllcDTO();

                    list.LoanAcc = allocation.LoanAcc;
                    list.LoanSub = allocation.LoanSub;
                    list.OffcCd = allocation.OffcCd;
                    list.DcalcCd = allocation.DcalcCd;
                    list.DcalcDetails = allocation.DcalcDetails;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.DcalcAmt = allocation.DcalcAmt;
                    list.DcalcRqdt = allocation.DcalcRqdt;
                    list.DcalcComdt = allocation.DcalcComdt;
                    var allAllocationCodes = _sessionManager.GetAllAllocationCode();
                    var allcCode = allAllocationCodes.Where(x => x.Value == list.DcalcCd.ToString());
                    list.DcalcCode = allcCode.First().Text;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    allocationDetails.Add(list);
                    _sessionManager.SetAllLoanAllocationList(allocationDetails);
                    if (allocationDetails.Count != 0)
                    {
                        activeallocationDetails = (allocationDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }

                     _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + LogAttribute.LoanAllocataionDto,
                    allocation.DcalcCd, allocation.DcalcDetails, allocation.DcalcRqdt, allocation.DcalcComdt));

                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.AccountNumber = list.LoanAcc;
                    return Json(new { isValid = true, data = allocation.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.loanAllocationviewPath + Constants.ViewAll, activeallocationDetails) });
                }
                 _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.LoanAllocataionDto,
                allocation.DcalcCd, allocation.DcalcDetails, allocation.DcalcRqdt, allocation.DcalcComdt));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.loanAllocationviewPath + Constants.Create, allocation) });
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
                List<TblIdmDhcgAllcDTO> activeallocationDetails = new();

                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, Id));

                var allocationList = JsonConvert.DeserializeObject<List<TblIdmDhcgAllcDTO>>(HttpContext.Session.GetString("SessionAllLoanAllocationList"));
                var itemToRemove = allocationList.Find(r => r.UniqueId == Id);
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;
                itemToRemove.Action = (int)Constant.Delete;
                allocationList.Add(itemToRemove);
                _sessionManager.SetAllLoanAllocationList(allocationList);
                if (allocationList.Count != 0)
                {
                    activeallocationDetails = allocationList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, Id));
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.OffCd = itemToRemove.OffcCd;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.loanAllocationviewPath + Constants.ViewAll, activeallocationDetails) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> SaveAllocationDetails()
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedSaveAllocationDetails);
        //        if (_sessionManager.GetAllLoanAllocationList().Count != 0)
        //        {
        //            var LoanAllocationList = _sessionManager.GetAllLoanAllocationList();

        //            foreach (var item in LoanAllocationList)
        //            {
        //                switch (item.Action)
        //                {
        //                    case (int)Constant.Delete:
        //                        await _loanAllocationService.DeleteLoanAllocationDetails(item);
        //                        _logger.Information(CommonLogHelpers.CompletedLoanAllocationDelete);
        //                        break;
        //                    case (int)Constant.Update:
        //                        await _loanAllocationService.UpdateLoanAllocationDetails(item);
        //                        _logger.Information(CommonLogHelpers.CompletedLoanAllocationUpdate);
        //                        break;
        //                    case (int)Constant.Create:
        //                        await _loanAllocationService.CreateLoanAllocationDetails(item);
        //                        _logger.Information(CommonLogHelpers.CompletedLoanAllocationCreate);
        //                        break;
        //                    default:
        //                        break;
        //                }

        //            }
        //            _logger.Information(CommonLogHelpers.CompletedSaveAllocationDetails);

        //            return Json(new { isValid = true });
        //        }
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.SaveLoanAllocation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        ViewBag.error = (Error.ViewBagError);
        //        return View(Error.ErrorPath);
        //    }
        //}
    }
}
