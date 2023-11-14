using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfSecurityandAquisitionAssetService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.UI.Services.Admin.IDM.CreationOfSecurityandAquisitionAssetService;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfDisbursmentProposalService;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.Admin.IDM.CreationOfDisbursmentProposal;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using KAR.KSFC.UI.Security;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IDisbursementService;
using AutoMapper.Configuration.Annotations;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.CreationOfDisbursmentProposal
{
    //ModifieBy:Swetha M  Added Authorization 
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class CreationOfDisbursmentProposalController : Controller
    {
        private readonly ICreationOfDisbursmentProposalService _createOfDisbursementProposal;
        private readonly IUnitDetailsService _unitDetailsService;
        private readonly IInspectionOfUnitService _inspectionOfUnitService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly ICommonService _commonService;
        private readonly IDataProtector protector;
        private readonly IDisbursementService _disbursementService;


        public CreationOfDisbursmentProposalController(ILogger logger, SessionManager sessionManager, ICommonService commonService,
            ICreationOfDisbursmentProposalService createOfDisbursementProposal, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings,
             IUnitDetailsService unitDetailsService, IDisbursementService disbursementService ,IInspectionOfUnitService inspectionOfUnitService)
        {
           
            _logger = logger;
            _sessionManager = sessionManager;
            _commonService = commonService;
            _inspectionOfUnitService= inspectionOfUnitService;
            _createOfDisbursementProposal = createOfDisbursementProposal;
                _unitDetailsService = unitDetailsService;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.AdminRouteValue);
            _disbursementService = disbursementService;

        }
        public async Task<IActionResult> Index()
        {
            //var loans = _sessionManager.GetAllLoanNumber()
            //   .Select(e =>
            //   {
            //       e.EncryptedLoanAcc = protector.Protect(e.LoanAcc.ToString());
            //       e.EncryptedLoanUnit = protector.Protect(e.LoanUnit);
            //       e.EncryptedLoanSub = protector.Protect(e.LoanSub.ToString());
            //       e.EncryptedInOffc = protector.Protect(e.InOffc.ToString());
            //       e.ShowView = false;
            //       return e;
            //   });

            var accNum = _sessionManager.GetAllLoanNumber();
            foreach (var obj in accNum)
            {
            var allDisbursementList = await  _disbursementService.GetAllDisbursementConditionList((long)obj.LoanAcc);
            var allAdditionalCondition = await _disbursementService.GetAllAdditionalConditonlist((long)obj.LoanAcc);

            int disbursementCount = allDisbursementList.Where(e => e.IsActive==true).Count();
            int additionalConditionCount = allAdditionalCondition.Where(e => e.IsActive==true).Count();
            int totalConditions = disbursementCount + additionalConditionCount;

            int disbursementCountRlx = allDisbursementList.Where(e => e.IsActive == true && (e.WhRelAllowed==true || e.Compliance == "YES")).Count();
            int additionalConditionCountRlx = allAdditionalCondition.Where(e => e.IsActive == true && (e.WhRelAllowed == true || e.Compliance == "YES")).Count();
                int totalConditionsRlx = disbursementCountRlx + additionalConditionCountRlx;
            
            if(totalConditions == totalConditionsRlx)
                {
                    obj.ShowView = false;
                }
                else
                {
                    obj.ShowView = true;
                }
            }

            var loans = accNum.Select(e =>
               {
                   e.EncryptedLoanAcc = protector.Protect(e.LoanAcc.ToString());
                   e.EncryptedLoanUnit = protector.Protect(e.LoanUnit);
                   e.EncryptedLoanSub = protector.Protect(e.LoanSub.ToString());
                   e.EncryptedInOffc = protector.Protect(e.InOffc.ToString());
                   e.ShowView = e.ShowView;
                   return e;
               });
            return View(loans);
        }
        public async Task<IActionResult> ViewAccount(string AccountNumber, string OffCd, string LoanSub, string UnitName, string loans)
        {
            try
            {
                _logger.Information(Constants.ViewAccountCreationofSecurityandAcquisition);
                ViewBag.encryptedloanacc = AccountNumber;
                ViewBag.encryptedoffcd = OffCd;
                ViewBag.encryptedlnsb = LoanSub;
                ViewBag.encryptedutname = UnitName;
                long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
                byte offCd = Convert.ToByte(protector.Unprotect(OffCd));
                int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
                string unitname = protector.Unprotect(UnitName);
                ViewBag.ProposalDetails = new List<TblIdmReleDetlsDTO>();

                IdmDDLListDTO idmDTO = await _commonService.GetAllIdmDropDownList();
                _sessionManager.SetDDListLandType(idmDTO.AllLandType);
                _sessionManager.SetDDListDeptMaster(idmDTO.AllDeptMaster);
                _sessionManager.SetAllOtherDebitCodeDDL(idmDTO.AllOtherDebitsDetails);
                _sessionManager.SetAllDsbChargeMapDDL(idmDTO.AllDsbChargeMap);

                //_logger.Information(Constants.GetAllocationCodeDetails);
                //var alllocationDetails = await _createOfDisbursementProposal.GetAllocationCodeDetails();
                //_logger.Information(Constants.GetRecomDisbursementReleaseDetails);
                //var allRecomDisbursementReleaseDetails = await _createOfDisbursementProposal.GetRecomDisbursementReleaseDetails();
                //_logger.Information(Constants.GetAllRecomDisbursementDetails);
                //var allRecomDisbursementList = await _createOfDisbursementProposal.GetAllRecomDisbursementDetails(accountNumber);
                //foreach (var i in allRecomDisbursementList)
                //{
                //    if (i.UniqueId == null)
                //    {
                //        i.UniqueId = Guid.NewGuid().ToString();
                //    }
                //    var allcCode = alllocationDetails.Where(x => x.AllcId == i.DsbAcd);
                //    i.AllocationDetails = allcCode.First().AllcDets;
                //    var eligibleAmt = allRecomDisbursementReleaseDetails.Where(x => x.PropNumber == i.DsbNo);
                //    // i.EligibleAmt = eligibleAmt.FirstOrDefault().ReleAmount;
                //    //i.ReleAmt = eligibleAmt.FirstOrDefault().ReleAmount;
                //}
                //_sessionManager.SetRecommDisbursementList(allRecomDisbursementList);
                _logger.Information(Constants.GetAllProposalDetails);
                var ProposalDetailsList = await _createOfDisbursementProposal.GetAllProposalDetails(accountNumber);
                _sessionManager.SetProposalDetailsList(ProposalDetailsList);

                _logger.Information(Constants.GetRecomDisbursementReleaseDetails);
                var allRecomDisbursementList = await _inspectionOfUnitService.GetAllRecomDisbursementDetails(accountNumber);
                _sessionManager.SetRecommDisbursementList(allRecomDisbursementList);

               
               
                //ViewBag.AllRecomDisbursementList = allRecomDisbursementList;
                ViewBag.ProposalDetailsList = ProposalDetailsList;
                var AllProposalDetailsList = _sessionManager.GetAllProposalDetailsList();
                ViewBag.ProposalDetails = AllProposalDetailsList;
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.loans = loans;
                ViewBag.OffCd = offCd;
                _logger.Information(Constants.ViewAccountCreationofSecurityandAcquisitionCompleted);
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }


        public async Task<IActionResult> ViewBeneficiaryDetails(string AccountNumber, string OffCd, string LoanSub, string UnitName,long PropNumber)
        {
            try
            {
                _logger.Information(Constants.GetAllBeneficiaryDetails);
                long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
                byte offCd = Convert.ToByte(protector.Unprotect(OffCd));
                int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
                string unitname = protector.Unprotect(UnitName);
                ViewBag.encryptedloanacc = AccountNumber;
                ViewBag.encryptedoffcd = OffCd;
                ViewBag.encryptedlnsb = LoanSub;
                ViewBag.encryptedutname = UnitName;
                var allChangeBankDetails = await _unitDetailsService.GetAllChangebankDetails(accountNumber);
                var primarybank = allChangeBankDetails.Where(x => x.UtBankPrimary == true).ToList();
                ViewBag.primarybank = primarybank;
                var allbeneficiaryDetails = await _createOfDisbursementProposal.GetAllBeneficiaryDetails(accountNumber);

                var beneficiarydetails = allbeneficiaryDetails.Where(r => r.BenfNumber == PropNumber).ToList();
                var proposalDetails = _sessionManager.GetAllProposalDetailsList();
                var proposalDetail = proposalDetails.Where(x => x.TblIdmDisbProp.PropNumber == PropNumber && x.LoanAcc == accountNumber).ToList();
                ViewBag.ProposalDetails = proposalDetail;
                ViewBag.Uniqid = proposalDetail.First().UniqueId;
                var otherdebitmast = _sessionManager.GetAllOtherDebitCode();
                ViewBag.otherdebitMast = otherdebitmast;


                if (beneficiarydetails.Count > 0 )
                {
                    //viewbag.beneficiarydetails = beneficiarydetails;
                    ViewBag.cheqno = beneficiarydetails.First().BenfRtgsChqNo;

                    //var chqdt = beneficiarydetails.First().BenfRtgsChqDt;
                    //var chqdt1 = chqdt.Value.ToString("");
                    //ViewBag.chqdt = chqdt1;
                }

                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.PropNumber = PropNumber;
                ViewBag.BenfAmt = beneficiarydetails[0].BenfAmt;
                ViewBag.OffCd = offCd;
                _logger.Information(Constants.ViewAccountGetAllBeneficiaryDetailsCompleted);
                return View();
            }
            catch(Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> SaveRecommendedDisbursementDetails()
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StartedSaveRecommendedDisbursementDetails);

        //        if (_sessionManager.GetAllRecommDisbursementDetail().Count != 0)
        //        {
        //            var UpdatedList = _sessionManager.GetAllRecommDisbursementDetail();
        //            foreach (var item in UpdatedList)
        //            {
        //                switch (item.Action)
        //                {
                            
        //                    case 2:
        //                        await _createOfDisbursementProposal.UpdateRecomDisbursementDetail(item);
        //                        break;
        //                    default:
        //                        break;
        //                }

        //            }

        //            _logger.Information(CommonLogHelpers.CompletedSaveRecommendedDisbursementDetails);

        //            return Json(new { isValid = true });
        //        }
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.SaveLandAcquisitionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        ViewBag.error = (Error.ViewBagError);
        //        return View(Error.ErrorPath);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> SaveBeneficiaryDetails(TblIdmBenfDetDTO beneficiaryDet)
        {
            if (beneficiaryDet.BenfRtgsChqDt == null && beneficiaryDet.BenfRtgsChqNo == null && beneficiaryDet.LoanAcc == null) // Address field to be added
            {
                return Ok();
            }
            else
            {

                TblIdmBenfDetDTO beneficiaryDets = new();
                beneficiaryDets.BenfId = beneficiaryDet.BenfId;
                beneficiaryDets.BenfNumber = beneficiaryDet.BenfNumber;
                beneficiaryDets.BenfRtgsBnkBranch = beneficiaryDet.BenfRtgsBnkBranch;
               // beneficiaryDets.BenfRtgsBank = beneficiaryDet.BenfRtgsBank;    var(30) is not sufficent for bank name
                beneficiaryDets.BenfRtgsAcNo = beneficiaryDet.BenfRtgsAcNo;
                beneficiaryDets.BenfRtgsIfsc = beneficiaryDet.BenfRtgsIfsc;
                beneficiaryDets.BenfRtgsBnkCity = beneficiaryDet.BenfRtgsBnkCity;
                beneficiaryDets.BenfCode = beneficiaryDet.BenfCode;

                beneficiaryDets.LoanAcc = beneficiaryDet.LoanAcc;
                beneficiaryDets.LoanSub = beneficiaryDet.LoanSub;
                beneficiaryDets.OffcCd = beneficiaryDet.OffcCd;
                beneficiaryDets.BenfDept = beneficiaryDet.BenfDept;
                beneficiaryDets.BenfName = beneficiaryDet.BenfName;
                beneficiaryDets.BenfAmt = beneficiaryDet.BenfAmt;
                beneficiaryDets.BenfRtgsChqDt = beneficiaryDet.BenfRtgsChqDt;
                beneficiaryDets.BenfRtgsChqNo = beneficiaryDet.BenfRtgsChqNo;
                //beneficiaryDets.DCFICRequestDate = beneficiaryDet.DCFICRequestDate; // address field to be added               
                beneficiaryDets.IsActive = true;
                beneficiaryDets.IsDeleted = false;
                beneficiaryDets.ModifiedDate = DateTime.Now;

                var res = await _createOfDisbursementProposal.UpdateBeneficiaryDetails(beneficiaryDets);
                ViewBag.BeneficiaryDetails = res;
                return Json(new { isValid = true });

            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveProposalDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveAuditDetails);
                if(_sessionManager.GetAllProposalDetailsList().Count != 0)
                {
                    var ProposalDetailsList = _sessionManager.GetAllProposalDetailsList();
                    foreach (var item in ProposalDetailsList)
                    {
                        switch (item.Action)
                        {
                            case 0:
                                await _createOfDisbursementProposal.CreateProposalDetail(item);
                                break;
                            case 2:
                                await _createOfDisbursementProposal.UpdateProposalDetail(item);
                                break;
                            case 3:
                                await _createOfDisbursementProposal.DeleteProposalDetail(item);
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
            catch(Exception ex)
            {
                _logger.Error(Error.SaveConditionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
    }
}
