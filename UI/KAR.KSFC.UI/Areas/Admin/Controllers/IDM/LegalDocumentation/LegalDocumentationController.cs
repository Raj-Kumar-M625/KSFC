using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Security;
using KAR.KSFC.UI.Services.IServices.Admin;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ILegalDocumentationService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Constant = KAR.KSFC.UI.Utility.Constant;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.LegalDocumentation
{
    //ModifieBy:Swetha M  Added Authorization 
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class LegalDocumentationController : Controller
    {
        private readonly ILegalDocumentationService _legalDocumentationService;
        private readonly IIdmService _idmService;
        private readonly ICommonService _commonService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly IDataProtector protector;

        public LegalDocumentationController(ILegalDocumentationService legalDocumentationService, ILogger logger, SessionManager sessionManager,
            IIdmService idmService, ICommonService commonService, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _legalDocumentationService = legalDocumentationService;
            _logger = logger;
            _sessionManager = sessionManager;
            _idmService = idmService;
            _commonService = commonService;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.AdminRouteValue);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewAccount(string OffCd, string AccountNumber, string LoanSub, string UnitName, string MainModule)
        {
            long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
           byte offCd = Convert.ToByte(protector.Unprotect(OffCd));
            int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
            string unitname = protector.Unprotect(UnitName);    
            try
            {
                _logger.Information(Constants.LegalDocumentationviewAccount);
                IdmDDLListDTO idmDTO = await _commonService.GetAllIdmDropDownList();
                _sessionManager.SetConditionDescriptionDDL(idmDTO.AllConditionDescriptions);
                _sessionManager.SetConditionTypeDDL(idmDTO.AllConditionTypes);
                _sessionManager.SetAssetTypeDDL(idmDTO.AllAssetTypes);
                _sessionManager.SetAssetCategaryMaster(idmDTO.AllAssetCategoryMaster);
                _sessionManager.SetSecurityTypeDDL(idmDTO.AllSecurityTypes);
                _sessionManager.SetSecurityCategoryDDL(idmDTO.AllSecurityCategory);
                _sessionManager.SetChargeTypeDDL(idmDTO.AllChargeType);
                _sessionManager.SetSubRegisterOfficeDDL(idmDTO.AllSubRegistrarOffice);
                _sessionManager.SetBankIFSCCodeDDL(idmDTO.AllIfscCode);
                _sessionManager.SetCondtionStage(idmDTO.AllConditionStages);
                _sessionManager.SetPromoterNamesList(idmDTO.AllPromoterNames);

                _logger.Information(Constants.GetAllPrimaryCollateralList);
                var allSecurityHolderList = await _legalDocumentationService.GetAllPrimaryCollateralList(accountNumber);
                foreach(var i in allSecurityHolderList)
                {
                    i.SecurityValue *= 100000;
                }
                _sessionManager.SetPrimaryCollateralList(allSecurityHolderList);

                _logger.Information(Constants.GetAllHypothecationList);
                var paramater = "FilteredRecord";


                var allHypothecationList = await _legalDocumentationService.GetAllHypothecationList(accountNumber, paramater);
                foreach (var i in allHypothecationList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                     
                    }
                    i.AssetValue *= 100000;
                    i.HypothValue = i.HypothValue;
                    //i.HypothMapId = i.HypothMapId;
                }
                ViewBag.deedNo = allHypothecationList.Last().HypothNo;
                _sessionManager.SetHypothecationList(allHypothecationList);
                var IdmHypothDetId = _sessionManager.GetAllHypothecationList();
                 var IdmHypothDetIds =  IdmHypothDetId.Select(x => x.IdmHypothDetId).ToList();
                {
                    ViewBag.IdmHypothDetId = IdmHypothDetIds;
                }

                var allHypodetailsWithoutDeedNumber = allHypothecationList.Where(r => r.HypothNo != null).ToList();
                _logger.Information(Constants.GetAllSecurityChargeList);
                var allSecurityChargeList = await _legalDocumentationService.GetAllSecurityChargeList(accountNumber);
                foreach (var i in allSecurityChargeList)
                {
                    i.SecurityValue *= 100000;
                }
                _sessionManager.SetSecurityChargeList(allSecurityChargeList);

                _logger.Information(Constants.GetAllCERSAIList);
                var parameter = "filterRecord";
                var allCERSAIList = await _legalDocumentationService.GetAllCERSAIList(accountNumber , parameter);
                _sessionManager.SetCERSAIList(allCERSAIList);

               
                var allAssetList = await _legalDocumentationService.GetAllAssetRefList(accountNumber);
                _sessionManager.SetAllAssetRefList(allAssetList);

                foreach (var i in allCERSAIList)
                {
                    var assetdes = allAssetList.Where(x => x.AssetCd == i.AssetCd).Select(x => x.AssetOthDetails).FirstOrDefault();
                    i.AssetDescription = assetdes;
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    i.AssetVal *= 100000;
                }
                ViewBag.cersai = allCERSAIList.Last().CersaiRegNo;
                _sessionManager.SetCERSAIList(allCERSAIList);
                var allCersai = _sessionManager.GetAllCERSAIList();
                var IdmDsbChargeIds = allCersai.Select(x => x.IdmDsbChargeId).ToList();
                {
                    ViewBag.IdmDsbChargeId = IdmDsbChargeIds;
                }



                List<IdmCersaiRegDetailsDTO> cersiAssetlist = new();
                var newCresai = new IdmCersaiRegDetailsDTO();
                foreach (var i in allAssetList)
                {
                    var assetdes = allCERSAIList.Where(x => x.AssetCd == i.AssetCd).Any();
                    if (assetdes)
                    {
                        newCresai = i.TblIdmCersaiRegistration.Where(x => x.AssetCd == i.AssetCd).LastOrDefault();
                        newCresai.AssetDescription = i.AssetOthDetails;
                        cersiAssetlist.Add(newCresai);
                    }
                    i.TblIdmCersaiRegistration.Select(x => x.AssetVal *= 100000);
                }
                ViewBag.cersai = cersiAssetlist.Last().CersaiRegNo;
                var AssetRefDetails = await _legalDocumentationService.GetAllCERSAIList(accountNumber, parameter);
                _sessionManager.SetCERSAIList(AssetRefDetails);

                _logger.Information(Constants.GetAllPromotorNames);
                var allPromoternames = await _idmService.GetAllPromotorNames();
                var allPrmoterphNo = await _idmService.GetAllPromoterPhNo();
                var allGuarantorDeedList = await _legalDocumentationService.GetAllGuarantorDeedList(accountNumber);

                foreach(var i in allGuarantorDeedList)
                {
                    var promcode = allPromoternames.Where(x => x.Value == i.PromoterCode);
                    i.GuarName = promcode.First().Text;

                    var promphNo = allPrmoterphNo.Where(x => x.Value == i.PromoterCode);
                    i.GuarMobileNo = promphNo.First().Text;
                    i.ValueAsset *= 100000;
                    i.ValueLiab *= 100000;
                    i.ValueNetworth *= 100000;
                }

                _sessionManager.SetGuarantorDeedList(allGuarantorDeedList);

                _logger.Information(Constants.GetAllConditionStages);
               

                _logger.Information(Constants.GetAllConditionList);
                var allConditionList = await _legalDocumentationService.GetAllConditionList(accountNumber);
                var allConditionStages = await _idmService.GetAllConditionStages();
                foreach (var i in allConditionList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var condstage = allConditionStages.Where(x => x.Value == i.CondStg);
                    i.ConditionStage = condstage.First().Text;
                }
                _sessionManager.SetConditionList(allConditionList);

                _logger.Information(Constants.FileList);
                var ldFileList = await _commonService.FileList(MainModule);
           
                _sessionManager.SetIDMDocument(ldFileList);


                LDCheckListDTO ldCheckList = new();
                ldCheckList.PrimarySecurityCount = allSecurityHolderList.Count();
                var PrimarySecurityEdit = allSecurityHolderList.Count(r => r.DeedNo != null );
                ldCheckList.PrimarySecurityEdit = PrimarySecurityEdit;
                ldCheckList.GuarantorDeedCount = allGuarantorDeedList.Count();
                var GuarantorDeedEdit = allGuarantorDeedList.Count(r => r.DeedNo != null);
                ldCheckList.GuarantorDeedEdit = GuarantorDeedEdit;
                ldCheckList.HypothecationCount = allHypothecationList.Count();
                var HypothecationEdit = allHypothecationList.Count(r => r.HypothNo != null);
                ldCheckList.HypothecationEdit = HypothecationEdit;
                ldCheckList.SecurityChargeCount = allSecurityChargeList.Count();
                var SecurityChargeEdit = allSecurityChargeList.Count(r => r.RequestLtrNo != null );
                ldCheckList.SecurityChargeEdit = SecurityChargeEdit;
                ldCheckList.CersaiCount = allCERSAIList.Count();
                var CersaiEdit = allCERSAIList.Count(r => r.CersaiRegNo != null);
                ldCheckList.CersaiEdit = CersaiEdit;
                ldCheckList.ConditionCount = allConditionList.Count();

                LegalDocumentationDTO legalDocumentsDTO = new();
                legalDocumentsDTO.AssetDetails = allHypodetailsWithoutDeedNumber;
                legalDocumentsDTO.SecurityChargeDetails= allSecurityChargeList.ToList();
                legalDocumentsDTO.SecurityDetails= allSecurityHolderList.ToList();
                legalDocumentsDTO.IdmGuarantorDeedDetailsDTO = allGuarantorDeedList.ToList();
                legalDocumentsDTO.CersaiRegDetails = allCERSAIList.ToList();
                legalDocumentsDTO.ConditionDetails= allConditionList.ToList();
                legalDocumentsDTO.LDCheckList = ldCheckList;

                var LoanAccountDetails= _sessionManager.GetAllLoanNumber();
                ViewBag.showApproval = false;
                 foreach(var obj in allConditionList)
                {
                    if(obj.Compliance != "YES")
                    {
                        ViewBag.showApproval = true;
                        break;
                    }
                }

                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                
                ViewBag.OffCd = offCd; 
                ViewBag.LoanDetails = LoanAccountDetails.Find( x => x.LoanAcc == accountNumber );
                ViewBag.Count = allSecurityChargeList.Where(x => x.RequestLtrNo != null).ToList().Count();

                return View(legalDocumentsDTO);
            }
            catch(Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePrimaryCollateralDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StratedSavePrimaryCollateralDetails);

                if (_sessionManager.GetPrimaryCollateralList().Count != 0)
                {
                    var SecurityHolderList = _sessionManager.GetPrimaryCollateralList();

                    foreach (var item in SecurityHolderList)
                    {
                        switch (item.Action)
                        {
                            
                            case (int)Constant.Update:
                                await _legalDocumentationService.UpdatePrimaryCollateralDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedPrimaryUpdate);

                                break;
                            default:
                                break;
                        }
                    }

                    _logger.Information(CommonLogHelpers.CompletedSavePrimaryCollateralDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SavePrimaryCollateralError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveCollateralDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StratedSaveCollateralDetails);

                if (_sessionManager.GetPrimaryCollateralList().Count != 0)
                {
                    var SecurityHolderList = _sessionManager.GetCollateralList();

                    foreach (var item in SecurityHolderList)
                    {
                        switch (item.Action)
                        {

                            case (int)Constant.Update:
                                await _legalDocumentationService.UpdateCollateralDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedPrimaryUpdate);

                                break;
                            default:
                                break;
                        }
                    }

                    _logger.Information(CommonLogHelpers.CompletedSavePrimaryCollateralDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SavePrimaryCollateralError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }



        [HttpPost]
        public async Task<IActionResult> SaveCersaiRegistrationForLD()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveCersaiDetails);
                if (_sessionManager.GetAllCERSAIList().Count != 0)
                {
                    var CersairegistrationList = _sessionManager.GetAllCERSAIList();
                    foreach (var item in CersairegistrationList)
                    {
                        switch (item.Action)
                        {
                            
                            case (int)Constant.Create:
                                await _legalDocumentationService.CreateLDCersaiRegDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedCersaiCreate);
                                break;
                            case (int)Constant.Delete:
                                await _legalDocumentationService.DeleteLDCersaiRegDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedHypothicationDelete);

                                break;
                            default:
                                break;
                        }
                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveCersaiDetails);
                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveCersaiError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
        public async Task<IActionResult> SaveGuarrantorDeedDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveGuarantorDeedDetails);
                if (_sessionManager.GetAllGuarantorDeedList().Count != 0)
                {
                    var GuarantorHolderList = _sessionManager.GetAllGuarantorDeedList();
                    foreach (var item in GuarantorHolderList)
                    {
                        switch (item.Action)
                        {
                            
                            case (int)Constant.Update:
                                await _legalDocumentationService.UpdateLDGuarantorDeedDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedGuarantorUpdate);

                                break;
                            default:
                                break;
                        }
                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveGuarantorDeedDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveGuarantorDeedError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveHypothecationForLD()
        {
            try
            {

                _logger.Information(CommonLogHelpers.StartedSaveHyopthecationDetails);
                if (_sessionManager.GetAllHypothecationList().Count != 0)
                {
                    var HypothecationList = _sessionManager.GetAllHypothecationList();

                    //Migrating HypoAssetRefDTO to IdmHypothecationDetailsDTO
                    List<IdmHypotheDetailsDTO> Hypo = HypothecationList.Select(x => new IdmHypotheDetailsDTO()
                    {
                        IdmHypothDetId = x.IdmHypothDetId,
                        LoanAcc = x.LoanAcc,
                        LoanSub = x.LoanSub,
                        OffcCd = x.OffcCd,
                        AssetCd = x.AssetCd,
                        HypothNo = x.HypothNo,
                        HypothDesc = x.HypothDesc,
                        AssetValue = x.AssetValue,
                        ExecutionDate = x.ExecutionDate,
                        HypothUpload = x.HypothUpload,
                        ApprovedEmpId = x.ApprovedEmpId,
                        AssetDet = x.AssetDet,
                        AssetName= x.AssetName,
                        HypothValue= x.HypothValue,
                        //HypothMapId= x.HypothMapId,
                        IsActive = x.IsActive,
                        IsDeleted = x.IsDeleted,
                        CreateBy = x.CreateBy,
                        ModifiedBy = x.ModifiedBy,
                        CreatedDate = x.CreatedDate,
                        ModifiedDate = x.ModifiedDate,
                        UniqueId = x.UniqueId,
                        Action = x.Action,
                    }).ToList();

                    //var CreateHypothe = _sessionManager.GetAllHypothecationList();
                    //var Create = CreateHypothe.Where(x => x.IdmHypothDetId == 0);

                    //List<IdmHypotheDetailsDTO> createHypothe = Create.Select(x => new IdmHypotheDetailsDTO()
                    //{
                    //    IdmHypothDetId = x.IdmHypothDetId,
                    //    LoanAcc = x.LoanAcc,
                    //    LoanSub = x.LoanSub,
                    //    OffcCd = x.OffcCd,
                    //    AssetCd = x.AssetCd,
                    //    HypothNo = x.HypothNo,
                    //    HypothDesc = x.HypothDesc,
                    //    AssetValue = x.AssetValue,
                    //    ExecutionDate = x.ExecutionDate,
                    //    HypothUpload = x.HypothUpload,
                    //    ApprovedEmpId = x.ApprovedEmpId,
                    //    AssetName= x.AssetName,
                    //    AssetDet= x.AssetDet,
                    //    IsActive = x.IsActive,
                    //    IsDeleted = x.IsDeleted,
                    //    CreateBy = x.CreateBy,
                    //    ModifiedBy = x.ModifiedBy,
                    //    CreatedDate = x.CreatedDate,
                    //    ModifiedDate = x.ModifiedDate,
                    //    UniqueId = x.UniqueId,
                    //    Action = x.Action,
                    //}).ToList();

                    await _legalDocumentationService.CreateHypothecationDetails(Hypo);
                    _logger.Information(CommonLogHelpers.CompletedHypothicationCreate);

                    foreach (var item in Hypo)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _legalDocumentationService.DeleteHypothecationDetail(item);
                                _logger.Information(CommonLogHelpers.CompletedHypothicationDelete);

                                break;
                            case (int)Constant.Update:
                                await _legalDocumentationService.UpdateHypothecationDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedHypothicationUpdate);
                                break;
                            //case (int)Constant.Create:
                            //    await _legalDocumentationService.CreateHypothecationDetails(item);
                            //    _logger.Information(CommonLogHelpers.CompletedHypothicationCreate);
                            //    break;
                            default:
                                break;
                        }
                    }

                  

                    //foreach (var item in createHypothe)
                    //{
                    //    await _legalDocumentationService.CreateHypothecationDetails(item);
                    //    _logger.Information(CommonLogHelpers.CompletedHypothicationCreate);

                    //}

                    _logger.Information((CommonLogHelpers.CompletedSaveHyopthecationDetails));

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveHyopthecationError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveConditionsForLD()
        {
            try
            {

                _logger.Information(CommonLogHelpers.StartedSaveConditionDetails);

                if (_sessionManager.GetAllConditionList().Count != 0)
                {
                 
                 var ConditionalList = _sessionManager.GetAllConditionList();
                    foreach (var item in ConditionalList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _legalDocumentationService.DeleteLDConditionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedCondtionDelete);

                                break;

                            case (int)Constant.Update:
                                await _legalDocumentationService.UpdateLDConditionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedCondtionUpdate);

                                break;
                            case (int)Constant.Create:
                                await _legalDocumentationService.CreateLDConditionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedCondtionCreate);

                                break;
                            default:
                                break;
                        }

                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveConditionDetails);

                    return Json(new { isValid = true });
                }
                return Json(new { isValid = true });

            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveConditionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveSecurityChargeForLD()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveSecurityChargeDetails);

                if (_sessionManager.GetAllSecurityChargeList().Count != 0)
                {
                    var SecurityHolderList = _sessionManager.GetAllSecurityChargeList();

                    foreach (var item in SecurityHolderList)
                    {
                        switch (item.Action)
                        {
                            
                            case (int)Constant.Update:
                                await _legalDocumentationService.UpdateSecurityCharge(item);
                                _logger.Information(CommonLogHelpers.CompletedSecurityUpdate);

                                break;
                            default:
                                break;
                        }

                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveSecurityChargeDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveSecurityChargeError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
    }
}
