using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IDisbursementService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.UI.Services.IServices.Admin;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using KAR.KSFC.UI.Security;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.Disbursement
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class DisbursementController : Controller
    {
        private readonly IDisbursementService _disbursementService;
        private readonly IIdmService _idmService;
        private readonly ICommonService _commonService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly IDataProtector protector;

        public DisbursementController(IDisbursementService disbursementService, ILogger logger, SessionManager sessionManager, 
            IIdmService idmService, ICommonService commonService, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _disbursementService = disbursementService;
            _logger = logger;
            _sessionManager = sessionManager;
            _idmService = idmService;
            _commonService = commonService;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.AdminRouteValue);

        }
        //Author: Manoj Date:17/08/2022
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
        //Author: Manoj Date:17/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:20/10/2022
        public async Task<IActionResult> ViewAccount(string AccountNumber, string OffcCd, string LoanSub, string UnitName, string loans, string MainModule)
        {
            long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
            byte offCd = Convert.ToByte(protector.Unprotect(OffcCd));
            int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
            string unitname = protector.Unprotect(UnitName);
            try
            {
                _logger.Information(Constants.DisbursmentviewAccount);
               
                IdmDDLListDTO idmDDLListDTO = await _commonService.GetAllIdmDropDownList();

                _sessionManager.SetConditionTypeDDL(idmDDLListDTO.AllConditionTypes);
                _sessionManager.SetCondtionStage(idmDDLListDTO.AllConditionStages);
                _sessionManager.SetConditionDescriptionDDL(idmDDLListDTO.AllConditionDescriptions);
                _sessionManager.SetCondtionStageMaster(idmDDLListDTO.AllConditionStagesMaster);
                _sessionManager.SetAllConstutionlist(idmDDLListDTO.AllConstutionDetails);
                _logger.Information(Constants.GetAllConditionTypes);
                var allConditionTypes = await _idmService.GetAllConditionTypes();
                var allconditionstages = await _idmService.GetAllConditionStages();
               
               

                var allForm8andForm13Master = await _idmService.GetAllForm8AndForm13Master(); //gowtham
                _sessionManager.SetForm8andForm13Master(idmDDLListDTO.AllForm8andForm13Master); //gowtham
                _logger.Information(Constants.GetAllDisbursementConditionList);
                var allDisbursementList = await _disbursementService.GetAllDisbursementConditionList(accountNumber);
                _logger.Information(Constants.GetAllAdditionalConditonlist);
                var allAdditionalCondition = await _disbursementService.GetAllAdditionalConditonlist(accountNumber);
                _logger.Information(Constants.GetAllForm8AndForm13List);
                var allForm8AndForm13List = await _disbursementService.GetAllForm8AndForm13List(accountNumber);  //Gowtahm

                _logger.Information(Constants.GetAllFirstInvestmentClauseDetails);
                var allFirstInvestmentClause = await _disbursementService.GetAllFirstInvestmentClauseDetails(accountNumber);
                allFirstInvestmentClause.DCFICLoanACC = accountNumber;
                allFirstInvestmentClause.DCFICAmount = allFirstInvestmentClause.DCFICAmount * 100000;
                allFirstInvestmentClause.DCFICAmountOriginal = allFirstInvestmentClause.DCFICAmountOriginal * 100000;


                var allSidbiApproval = await _disbursementService.GetAllSidbiApprovalDetails(accountNumber);
               

                var constutionlist = _sessionManager.GetAllConstutionlist();
                var condDDl = constutionlist.Where(x => x.Value == allSidbiApproval.PromTypeCd.ToString());
                allSidbiApproval.CnstDets = condDDl.First().Text;

                foreach (var i in allDisbursementList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var condStgDDL = allconditionstages.Where(x => x.Value == i.CondStg);
                    var condTypeDDL = allConditionTypes.Where(x => x.Value == i.CondType);
                    i.ConditionType = condTypeDDL.First().Text;
                    i.ConditionStage = condStgDDL.First().Text;
                }


                foreach (var i in allAdditionalCondition)
                {
                    i.UniqueId = Guid.NewGuid().ToString();
                    var condStgDDL = allconditionstages.Where(x => x.Value == i.AddCondStage);
                    i.ConditionStage = condStgDDL.First().Text;
                }

                foreach (var i in allForm8AndForm13List)  //Gowtham
                {
                    i.UniqueId = Guid.NewGuid().ToString();
                    var formTypeDDl = allForm8andForm13Master.Where(x => x.Value == i.DF813t1);
                    i.FormType = formTypeDDl.First().Text;
                    
                }

             

                _sessionManager.SetConditionList(allDisbursementList);
                _sessionManager.SetForm8AndForm13List(allForm8AndForm13List);
                _sessionManager.SetAdditionalConditionList(allAdditionalCondition);
              


                var ldFileList = await _commonService.FileList(MainModule);
                _sessionManager.SetIDMDocument(ldFileList);

                var othRelx = await _disbursementService.GetAllOtherRelaxation(accountNumber);
                _sessionManager.SetAllOtherRelaxation(othRelx);

                ViewBag.MainModule = "DCdoc";
                ViewBag.MainModule = "DisbursementCondition";

                VerificationOfDisbursementDTO verificationOfDisbursementDTO = new();

                verificationOfDisbursementDTO.CondType = idmDDLListDTO.AllConditionTypes;
                verificationOfDisbursementDTO.DisbursementDetails = allDisbursementList.ToList();
                verificationOfDisbursementDTO.AdditionalCondition = allAdditionalCondition.ToList();
                verificationOfDisbursementDTO.Form8AndForm13List = allForm8AndForm13List.ToList();
                verificationOfDisbursementDTO.SidbiApproval = allSidbiApproval;
                verificationOfDisbursementDTO.FirstInvestmentClause = allFirstInvestmentClause;
                verificationOfDisbursementDTO.OtherRelaxation = othRelx.ToList();
                ViewBag.Documentlist = ldFileList;
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.loans = loans;
                ViewBag.OffcCd = offCd;
                _logger.Information(Constants.CompletedDisbursmentviewAccount);
                return View(verificationOfDisbursementDTO);
            }
            catch (Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }

        }
        [HttpPost]
        public async Task<IActionResult> SaveDisbursementConditionDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveDisbursementDetails);
                if (_sessionManager.GetAllConditionList().Count != 0)
                {

                    var disbursementConditionalList = _sessionManager.GetAllConditionList();
                  

                    foreach (var item in disbursementConditionalList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _disbursementService.DeleteDisbursementConditionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedDisbursementConditionalDelete);
                                break;
                            case (int)Constant.Update:
                                await _disbursementService.UpdateDisbursementConditionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedDisbursementConditionalUpdate);
                                break;
                            case (int)Constant.Create:
                                await _disbursementService.CreateDisbursementConditionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedDisbursementConditionalCreate);
                                break;
                            default:
                                break;
                        }

                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveDisbursementDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveDisbursement + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveForm8AndForm13Details()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveForm8AndForm13Details);
                if (_sessionManager.GetForm8AndForm13List().Count != 0)
                {
                    var Form8AndForm13ConditionalList = _sessionManager.GetForm8AndForm13List();

                    foreach (var item in Form8AndForm13ConditionalList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _disbursementService.DeleteForm8AndForm13Details(item);
                                break;
                            case (int)Constant.Update:
                                await _disbursementService.UpdateForm8AndForm13Details(item);
                                break;
                            case (int)Constant.Create:
                                await _disbursementService.CreateForm8AndForm13Details(item);
                                break;
                            default:
                                break;
                        }
                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveForm8AndForm13Details);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveForm8AndForm13 + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveAddtionalConditionDetails(string accountNumber)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveDisbursementDetails);
                if (_sessionManager.GetAllAdditionalConditionList().Count != 0)
                {
                    var disbursementConditionalList = _sessionManager.GetAllAdditionalConditionList();
                   
                    foreach (var item in disbursementConditionalList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _disbursementService.DeleteAdditionalConditionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedAdditionalConditionalDelete);
                                break;
                            case (int)Constant.Update:
                                await _disbursementService.UpdateAdditionalConditionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedAdditionalConditionalUpdate);
                                break;
                            case (int)Constant.Create:
                                await _disbursementService.CreateAdditionalConditionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedAdditionalConditionalCreate);
                                break;
                            default:
                                break;
                        }

                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveDisbursementDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveAdditionalCondtional + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(IdmFirstInvestmentClauseDTO idmFICDetail)
        {
            try
            {
                _logger.Information(Constants.UpdateFIC);

                if (idmFICDetail.DCFICRequestDate == null && idmFICDetail.DCFICCommunicationDate == null && idmFICDetail.DCFICAmountOriginal == null)
                {
                    return Ok();
                }
                else
                {

                    IdmFirstInvestmentClauseDTO idmFICDetails = new IdmFirstInvestmentClauseDTO();
                    idmFICDetails.DCFICAmountOriginal = idmFICDetail.DCFICAmount/100000;
                    idmFICDetails.DCFICId = idmFICDetail.DCFICId;
                    idmFICDetails.DCFICAmount= idmFICDetail.DCFICAmount/100000;
                    idmFICDetails.DCFICRequestDate = idmFICDetail.DCFICRequestDate;
                    idmFICDetails.DCFICCommunicationDate = idmFICDetail.DCFICCommunicationDate;
                    idmFICDetails.DCFICLoanACC = idmFICDetail.DCFICLoanACC;
                    idmFICDetails.DCFICSno = idmFICDetail.DCFICSno;
                    idmFICDetails.DCFICOffc = idmFICDetail.DCFICOffc;
                    idmFICDetails.IsActive = true;
                    idmFICDetails.IsDeleted = false;
                    idmFICDetails.ModifiedDate = DateTime.Now;

                    await _disbursementService.UpdateFirstInvestmentClauseDetails(idmFICDetails);
                    _logger.Information(Constants.CompletedUpdateFIC);
                    return Json(new { isValid = true, data = idmFICDetail.DCFICLoanACC });

                }
            }
            catch (Exception ex)
            {
                _logger.Error(Error.updateFirstinvestment + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }

        }
        // Dev
        [HttpPost]
        public async Task<IActionResult> SaveSidbiApprovalDetails(IdmSidbiApprovalDTO sidbi)
        {
            try
            {
                _logger.Information(Constants.SaveSidbiApprovalDetails);
                    IdmSidbiApprovalDTO sidbia = new IdmSidbiApprovalDTO();

                    var sa = await _disbursementService.GetAllSidbiApprovalDetails(sidbi.LoanAcc);
                    if (sa != null)
                    {
                        sidbia = sa;
                        sidbia.WhAppr = sidbi.WhAppr;
                    }
                    var res = await _disbursementService.UpdateSidbiApprovalDetails(sidbia);
                   _logger.Information(Constants.CompletedSaveSidbiApprovalDetails);
                    ViewBag.SidbiApproval = res;
                    return Ok(new { data = res, sidbia.WhAppr });
                
            }
            catch (Exception ex)
            {
                _logger.Error(Error.Savesidbiapproval + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveOtherRelaxation(List<RelaxationItem> OthRelax)
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedOtherRelaxation + OthRelax);
                List<RelaxationDTO> allsavedrelaxlist = _sessionManager.GetAllOtherRelaxation();

                foreach(var item in OthRelax)
                {
                    var relaxexistlist = allsavedrelaxlist.Where(x => x.ModelName == item.Value && x.RelaxCondId == Convert.ToInt64 (item.Id)).ToList();
                    relaxexistlist[0].WhRelAllowed = item.WhRelAllowed;
                    await _disbursementService.UpdateOtherRelaxation(relaxexistlist);
                }  
                _logger.Information(CommonLogHelpers.CompletedOtherRelaxation + OthRelax);
                return Json(new { isValid = true });                    
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveOtherRelaxation + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
        public class RelaxationItem
        {
            public string Id { get; set; }
            public string Value { get; set; }
            public bool WhRelAllowed { get; set; }
        }
    }
}