using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IAuditService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using KAR.KSFC.UI.Security;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.AuditClearance
{
    //ModifieBy:Swetha M  Added Authorization 
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class AuditController : Controller
    {
        private readonly IAuditService _auditService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly ICommonService _commonService;
        private readonly IDataProtector protector;


        public AuditController(IAuditService auditService, ILogger logger, SessionManager sessionManager, ICommonService commonService
            ,IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _auditService = auditService;
            _logger = logger;
            _sessionManager = sessionManager;
            _commonService = commonService;
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
        public async Task<IActionResult> ViewAccount(string AccountNumber, string OffCd, string LoanSub, string UnitName, string loans, string MainModule)
        {
            long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
            byte offCd = Convert.ToByte(protector.Unprotect(OffCd));
            int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
            string unitname = protector.Unprotect(UnitName);

            try
            {
              
                _logger.Information(Constants.GetAllAuditClearanceList);
                var allAuditClearanceList = await _auditService.GetAllAuditClearanceList(accountNumber);
                foreach (var i in allAuditClearanceList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                }
                _sessionManager.SetAuditClearanceList(allAuditClearanceList);
                _logger.Information(Constants.FileList);
                var ldFileList = await _commonService.FileList(MainModule);
                _sessionManager.SetIDMDocument(ldFileList);


                AuditClearenceDTO auditClearence = new();
                auditClearence.AuditClearence = allAuditClearanceList.ToList();

                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.loans = loans;
                ViewBag.OffCd = offCd;
                return View(auditClearence);
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
                var AllAuditClearanceList = _sessionManager.GetAllAuditClearanceList();
                IdmAuditDetailsDTO AuditClearanceList = AllAuditClearanceList.FirstOrDefault(x => x.UniqueId == unqid);
              

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == AuditClearanceList.IdmAuditId && x.SubModuleType == Constants.AuditClearance && x.MainModule == Constants.AuditClearance).ToList();
                ViewBag.Documentlist = doc;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + unqid);
                return View(Constants.auditResultViewPath + Constants.ViewRecord, AuditClearanceList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult Edit(string unqid = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + unqid);
                var AllAuditClearanceList = _sessionManager.GetAllAuditClearanceList();
                IdmAuditDetailsDTO AuditClearanceList = AllAuditClearanceList.FirstOrDefault(x => x.UniqueId == unqid);
                if (AuditClearanceList != null)
                {


                    var doclist = _sessionManager.GetIDMDocument();
                    var doc = doclist.Where(x => x.SubModuleId == AuditClearanceList.IdmAuditId && x.SubModuleType == Constants.AuditClearance && x.MainModule == Constants.AuditClearance).ToList();
                    ViewBag.SubModuleId = AuditClearanceList.IdmAuditId;
                    ViewBag.SubModuleType = Constants.AuditClearance;
                    ViewBag.MainModule = Constants.AuditClearance;

                    ViewBag.Documentlist = doc;

                    _logger.Information(CommonLogHelpers.UpdateCompleted + unqid);
                }

                return View(Constants.auditResultViewPath + Constants.editCs, AuditClearanceList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IdmAuditDetailsDTO audit)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.AuditClearanceDto,
                  audit.IdmAuditId, audit.AuditObservation, audit.AuditCompliance));

                List<IdmAuditDetailsDTO> auditDetails = new();
                List<IdmAuditDetailsDTO> activeauditDetails = new();
                if (_sessionManager.GetAllAuditClearanceList() != null)
                    auditDetails = _sessionManager.GetAllAuditClearanceList();

                IdmAuditDetailsDTO auditExist = auditDetails.Find(x => x.UniqueId == audit.UniqueId);
                long? accountNumber = 0;
                if (auditExist != null)
                {
                    accountNumber = auditExist.LoanAcc;
                    auditDetails.Remove(auditExist);
                    var list = auditExist;
                    list.LoanAcc = audit.LoanAcc;
                    list.UniqueId = audit.UniqueId;
                    list.OffcCd = audit.OffcCd;
                    list.LoanSub = audit.LoanSub;
                    list.AuditObservation = audit.AuditObservation;
                    list.AuditCompliance = audit.AuditCompliance;
                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (auditExist.IdmAuditId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;
                    }

                    auditDetails.Add(list);
                    _sessionManager.SetAuditClearanceList(auditDetails);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;

                    if (auditDetails.Count != 0)
                    {
                        activeauditDetails = (auditDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }

                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.AuditClearanceDto,
                 audit.IdmAuditId, audit.AuditObservation, audit.AuditCompliance));
                    return Json(new { isValid = true, data = accountNumber, html = Helper.RenderRazorViewToString(this, Constants.auditViewPath + Constants.ViewAll, activeauditDetails) });
                }
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.AuditClearanceDto,
               audit.IdmAuditId, audit.AuditObservation, audit.AuditCompliance));
                ViewBag.AccountNumber = audit.LoanAcc;
                ViewBag.OffCd = audit.OffcCd;
                ViewBag.LoanSub = audit.LoanSub;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.auditViewPath + Constants.Edit, audit) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult Create(long AccountNumber, int LoanSub, byte OffCd)
        {
            try
            {

                _logger.Information(CommonLogHelpers.CreateStarted + AccountNumber);
                ViewBag.LoanAcc = AccountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffCd;

                _logger.Information(CommonLogHelpers.CreateCompleted + AccountNumber);
                return View(Constants.auditResultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmAuditDetailsDTO audit)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.CreateStartedPost + LogAttribute.AuditClearanceDto,
                     audit.IdmAuditId, audit.AuditObservation, audit.AuditCompliance));
                if (ModelState.IsValid)
                {

                    List<IdmAuditDetailsDTO> auditDetails = new();
                    List<IdmAuditDetailsDTO> activeauditDetails = new();
                    if (_sessionManager.GetAllAuditClearanceList() != null)
                        auditDetails = _sessionManager.GetAllAuditClearanceList();

                    IdmAuditDetailsDTO list = new IdmAuditDetailsDTO();
                    list.LoanAcc = audit.LoanAcc;
                    list.AuditObservation = audit.AuditObservation;
                    list.AuditCompliance = audit.AuditCompliance;
                    list.OffcCd = audit.OffcCd;
                    list.LoanSub = audit.LoanSub;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    auditDetails.Add(list);
                    _sessionManager.SetAuditClearanceList(auditDetails);
                    if (auditDetails.Count != 0)
                    {
                        activeauditDetails = (auditDetails.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                    }
                    _logger.Information(string.Format(CommonLogHelpers.CreateCompletedPost + LogAttribute.AuditClearanceDto,
                      audit.IdmAuditId, audit.AuditObservation, audit.AuditCompliance));
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.OffCd = list.OffcCd;
                    ViewBag.LoanSub = list.LoanSub;
                    return Json(new { isValid = true, data = audit.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.auditViewPath + Constants.ViewAll, activeauditDetails) });
                }

                ViewBag.AccountNumber = audit.LoanAcc;
                ViewBag.OffCd = audit.OffcCd;
                ViewBag.LoanSub = audit.LoanSub;
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.AuditClearanceDto,
                        audit.IdmAuditId, audit.AuditObservation, audit.AuditCompliance));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.auditViewPath + Constants.Create, audit) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult Delete(string unqid = "")
        {
            try
            {
                IEnumerable<IdmAuditDetailsDTO> activeAuditList = new List<IdmAuditDetailsDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, unqid));
                var auditList = JsonConvert.DeserializeObject<List<IdmAuditDetailsDTO>>(HttpContext.Session.GetString(Constants.sessionAuditClearance));
                var itemToRemove = auditList.Find(r => r.UniqueId == unqid);
                auditList.Remove(itemToRemove);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                auditList.Add(itemToRemove);
                _sessionManager.SetAuditClearanceList(auditList);
                if (auditList.Count > 0)
                {
                    activeAuditList = auditList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                }
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, unqid));
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.OffCd = itemToRemove.OffcCd;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.auditViewPath + Constants.ViewAll, activeAuditList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveAuditClearanceDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveAuditDetails);
                if (_sessionManager.GetAllAuditClearanceList().Count != 0)
                {
                    var AuditDetailsList = _sessionManager.GetAllAuditClearanceList();

                    foreach (var item in AuditDetailsList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                if (item.IdmAuditId != 0)
                                {
                                    await _auditService.DeleteAuditClearanceDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedAuditClearenceDelete);
                                }
                                break;
                            case (int)Constant.Update:
                                await _auditService.UpdateAuditClearanceDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedAuditClearenceUpdate);
                                break;
                            case (int)Constant.Create:
                                await _auditService.CreateAuditClearanceDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedAuditClearenceCreate);
                                break;
                            default:
                                break;
                        }

                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveAuditDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveAuditClearanceError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
    }
}
