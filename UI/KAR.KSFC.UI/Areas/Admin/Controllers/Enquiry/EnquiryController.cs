using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Services.IServices.Admin;
using KAR.KSFC.UI.Utility;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.EG)]
    public class EnquiryController : Controller
    {
        private readonly SessionManager _sessionManager;
        private readonly IEnquiryService _adminenquiryService;
        private readonly IEnquirySubmissionService _enquiryService;
        private readonly IOtpService _otpService;
        private readonly ILogger _logger;
        public EnquiryController(IEnquiryService enquiryService, SessionManager sessionManager, IEnquirySubmissionService enquirySubmissionService, IOtpService otpService = null, ILogger logger = null)
        {
            _adminenquiryService = enquiryService;
            _enquiryService = enquirySubmissionService;
            _sessionManager = sessionManager;
            _otpService = otpService;
            _logger = logger;
        }
        /// <summary>
        /// To get all enquiries for admin
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.Information("Started - Enquiry Index ");

                List<EnquirySummary> enquirySummaryList = await _adminenquiryService.GetAllEnquiriesForAdmin();

                _logger.Information("Completed - Enquiry Index ");

                return View(enquirySummaryList);
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Enquiry page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteEnquiry(int id)
        {
            try
            {
                _logger.Information("Started - DeleteEnquiry with Id " + id);

                string viewPath = "../../Areas/Admin/Views/Enquiry/Index";
                var response = await _enquiryService.DeleteEnquiry(id);
                List<EnquirySummary> enquirySummaryList = await _adminenquiryService.GetAllEnquiriesForAdmin();

                _logger.Information("Completed - DeleteEnquiry with Id " + id);

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath, enquirySummaryList) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Delete HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEnquiryStatus(EnquirySummary model)
        {
            try
            {
                _logger.Information(string.Format("Started - UpdateEnquiryStatus with EnquiryId {0} PromotorPan :{1} EnqSubmitDate :{2} EnqStatus :{3} EnqInitiateDate :{4} "
                                                          , model.EnquiryId, model.PromotorPan, model.EnqSubmitDate, model.EnqStatus, model.EnqInitiateDate));

                string btnValue = string.Empty;
                if (model.EnqStatus == (int)EnqStatus.Submitted)
                {
                    model.EnqStatus = (int)EnqStatus.Acknowledge;
                    btnValue = "Initiate Scrutiny";
                }
                else if (model.EnqStatus == (int)EnqStatus.Acknowledge)
                {
                    model.EnqStatus = (int)EnqStatus.InitiateScrutiny;
                    btnValue = "Approve";
                }
                else if (model.EnqStatus == (int)EnqStatus.InitiateScrutiny)
                {
                    model.EnqStatus = (int)EnqStatus.Approved;
                    btnValue = "Approved";
                }
                else if (model.EnqStatus == (int)EnqStatus.Approved)
                {
                    model.EnqStatus = (int)EnqStatus.Approved;
                    btnValue = "Approved";
                }
                await _enquiryService.UpdateEnquiryStatus(model.EnquiryId.Value, model.EnqStatus.Value);
                string viewPath = "../../Areas/Admin/Views/Enquiry/Index";

                List<EnquirySummary> enquirySummaryList = await _adminenquiryService.GetAllEnquiriesForAdmin();

                _logger.Information(string.Format("Completed - UpdateEnquiryStatus with EnquiryId {0} PromotorPan :{1} EnqSubmitDate :{2} EnqStatus :{3} EnqInitiateDate :{4} "
                                                          , model.EnquiryId, model.PromotorPan, model.EnqSubmitDate, model.EnqStatus, model.EnqInitiateDate));

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath, enquirySummaryList) });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! UpdateEnquiryStatus HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EnquirySummary model)
        {
            try
            {
                _logger.Information(string.Format("Started - Edit with id {0} AddtypeCd :{1} UnitAddress :{2} UnitMobileNo :{3} UnitPincode :{4} "
                                                                 , model.EnquiryId, model.PromotorPan, model.EnqSubmitDate, model.EnqStatus, model.EnqInitiateDate));

                if (model.EnquiryId != null && model.EnquiryId > 0)
                {
                    _sessionManager.SetAllEntitiesToEmptyString();
                    _sessionManager.SetNewEnqTempId(model.EnquiryId.Value.ToString());
                    _sessionManager.SetOperationType("Edit");
                    EnquiryDTO enquiryDTO = new();
                    #region UnitTab
                    ViewBag.UnitTab = "bg-warning";
                    List<SelectListItem> listAddressTypes = await _enquiryService.getAllAddressTypesFromDB();
                    _sessionManager.SetAddressTypesFromDB(listAddressTypes);
                    enquiryDTO.DDLDTO = await _enquiryService.GetAllEnquiryDropDownList();
                    //Populating Dropdown List of BasicDetails Presonal Details page
                    ViewBag.ListBranch = enquiryDTO.DDLDTO.ListBranch;
                    ViewBag.ListLoanPurpose = enquiryDTO.DDLDTO.ListLoanPurpose;
                    ViewBag.ListFirmSize = enquiryDTO.DDLDTO.ListFirmSize;
                    ViewBag.ListProduct = enquiryDTO.DDLDTO.ListProduct;
                    ViewBag.ListDistrict = enquiryDTO.DDLDTO.ListDistrict;
                    ViewBag.ListPremises = enquiryDTO.DDLDTO.ListPremises;

                    // Populating Dropdown List of BasicDetails Bank Details page
                    _sessionManager.SetDDListDomicileStatus(enquiryDTO.DDLDTO.ListDomicileStatus);
                    _sessionManager.SetDDLBankFacilityType(enquiryDTO.DDLDTO.ListFacility);

                    _sessionManager.SetDDListFinancialYear(enquiryDTO.DDLDTO.ListFY);
                    _sessionManager.SetDDListFinancialComponent(enquiryDTO.DDLDTO.ListFinancialComponent);
                    enquiryDTO.ProjectDetails = new ProjectAllDetailsDTO
                    {
                        ListPrjctCost = new List<ProjectCostDetailsDTO>(),
                        ListMeansOfFinance = new List<ProjectMeansOfFinanceDTO>(),
                        ListPrevYearFinDetails = new List<ProjectFinancialYearDetailsDTO>()
                    };
                    _sessionManager.SetDDListProjectCostComponent(enquiryDTO.DDLDTO.ListProjectCost);
                    _sessionManager.SetDDListProjectMeansOfFinance(enquiryDTO.DDLDTO.ListMeansOfFinanceCategory);
                    _sessionManager.SetDDListProjectFinanceType(enquiryDTO.DDLDTO.ListMeansOfFinanceType);
                    _sessionManager.SetDDListTypeOfSecurity(enquiryDTO.DDLDTO.ListSecurityType);
                    _sessionManager.SetDDListSecurityDetailsType(enquiryDTO.DDLDTO.ListSecurityDet);
                    _sessionManager.SetDDListRelationType(enquiryDTO.DDLDTO.ListSecurityRelation);
                    var enq = await _enquiryService.getEnquiryDetails(model.EnquiryId.Value);
                    var ListTaluka = new List<SelectListItem>();
                    var ListHobli = new List<SelectListItem>();
                    var ListVillage = new List<SelectListItem>();
                    if (enq.BasicDetails != null)
                    {
                        var getcascadePrefillDDLList = await _enquiryService.getCascadeDDLForEditPrefill(enq.BasicDetails.DistrictCd, enq.BasicDetails.TalukaCd,
                        enq.BasicDetails.HobliCd);
                        ListTaluka = getcascadePrefillDDLList.ListTaluka;
                        ListHobli = getcascadePrefillDDLList.ListHobli;
                        ListVillage = getcascadePrefillDDLList.ListVillage;

                        enquiryDTO.UnitDetails = new UnitDetailsDTO
                        {
                            BasicDetails = enq.BasicDetails,
                            BankDetails = enq.BankDetails,
                            ListRegDetails = enq.RegistrationDetails.ToList(),
                            ListAddressDetail = enq.AddressDetails.ToList()
                        };

                        foreach (var item in enq.AddressDetails)
                        {
                            item.AddressTypeMasterDTO = new AddressTypeMasterDTO
                            {
                                AddtypeCd = item.AddtypeCd,
                                AddtypeDets = listAddressTypes.Find(a => a.Value == item.AddtypeCd.ToString()).Text
                            };
                        }
                        var regTypes = enquiryDTO.DDLDTO.ListRegnType;
                        foreach (var item in enq.RegistrationDetails)
                        {
                            item.RegTypeText = regTypes.FindAll(x => x.Value == item.RegrefCd.ToString()).FirstOrDefault().Text;
                        }
                        if (enq.AddressDetails.Any())
                        {
                            _sessionManager.SetAddressList(enq.AddressDetails.ToList());
                        }
                        if (enq.RegistrationDetails.Any())
                        {
                            _sessionManager.SetRegistrationDetList(enq.RegistrationDetails.ToList());
                        }
                    }
                    else
                    {
                        enquiryDTO.UnitDetails = new UnitDetailsDTO();
                        enquiryDTO.UnitDetails.ListAddressDetail = new List<AddressDetailsDTO>();
                        enquiryDTO.UnitDetails.ListRegDetails = new List<RegistrationNoDetailsDTO>();
                    }
                    ViewBag.ListTaluka = ListTaluka;
                    ViewBag.ListHobli = ListHobli;
                    ViewBag.ListVillage = ListVillage;

                    if (enq.BasicDetails != null)
                    {
                        ViewBag.UnitTab = "bg-success";
                    }
                    #endregion

                    #region Promotor and Gurantor
                    ViewBag.PromoterTab = "bg-warning";
                    //Adding dropdownlist data to Session to be used by Promoter and Guar page ddl
                    _sessionManager.SetDDListPromoterDesignation(enquiryDTO.DDLDTO.ListPromDesgnType);
                    _sessionManager.SetDDListPromAndGuarAssetCategory(enquiryDTO.DDLDTO.ListAssetCategory);
                    _sessionManager.SetDDListPromAndGuarAssetType(enquiryDTO.DDLDTO.ListAssetType);
                    _sessionManager.SetDDListModeOfAcquire(enquiryDTO.DDLDTO.ListAcquireMode);
                    enquiryDTO.PromoterAllDetailsDTO = new PromoterAllDetailsDTO
                    {
                        ListPromoters = new List<PromoterDetailsDTO>(),
                        PromotersAssetLiabilityDetails = new PromAssetLiabilityDetailsDTO { ListAssetDetails = new List<PromoterAssetsNetWorthDTO>(), ListLiabilityDetails = new List<PromoterLiabilityDetailsDTO>() }
                    };
                    enquiryDTO.GuarantorAllDetailsDTO = new GuarantorAllDetailsDTO
                    {
                        ListGuarantor = new List<GuarantorDetailsDTO>(),
                        GuarantorAssetLiabilityDetails = new GuarAssetLiabilityDetailsDTO { ListAssetDetails = new List<GuarantorAssetsNetWorthDTO>(), ListLiabilityDetails = new List<GuarantorLiabilityDetailsDTO>() }
                    };
                    if (enq.PromoterDetails != null && enq.PromoterDetails.Any())
                    {
                        enquiryDTO.PromoterAllDetailsDTO.ListPromoters = enq.PromoterDetails.ToList();
                        _sessionManager.SetPromoterDetailsList(enq.PromoterDetails.ToList());
                    }
                    if (enq.PromoterAssetsDetails != null && enq.PromoterAssetsDetails.Any())
                    {
                        foreach (var item in enq.PromoterAssetsDetails)
                        {
                            var AssetCategorymasterData = enquiryDTO.DDLDTO.ListAssetCategory.
                                FirstOrDefault(x => x.Value == item.AssetcatCd.ToString());
                            item.AssetCategoryMasterDTO = new AssetCategoryMasterDTO();
                            item.AssetCategoryMasterDTO.AssetcatCd = Convert.ToInt32(AssetCategorymasterData.Value);
                            item.AssetCategoryMasterDTO.AssetcatDets = AssetCategorymasterData.Text;

                            var AssetTypemasterData = enquiryDTO.DDLDTO.ListAssetType.
                               FirstOrDefault(x => x.Value == item.AssettypeCd.ToString());
                            item.AssetTypeMasterDTO = new AssetTypeMasterDTO();
                            item.AssetTypeMasterDTO.AssettypeCd = Convert.ToInt32(AssetTypemasterData.Value);
                            item.AssetTypeMasterDTO.AssettypeDets = AssetTypemasterData.Text;

                            item.PromoterMasterDTO = new PromoterMasterDTO();
                            item.PromoterMasterDTO.PromoterCode = item.PromoterCode.Value;
                            item.PromoterMasterDTO.PromoterName = enq.PromoterDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                        }

                        enquiryDTO.PromoterAllDetailsDTO.PromotersAssetLiabilityDetails.ListAssetDetails = enq.PromoterAssetsDetails.ToList();
                        _sessionManager.SetPromoterAssetList(enq.PromoterAssetsDetails.ToList());
                    }
                    if (enq.PromoterLiability != null && enq.PromoterLiability.Any())
                    {
                        foreach (var item in enq.PromoterLiability)
                        {
                            item.PromoterMasterDTO = new PromoterMasterDTO();
                            item.PromoterMasterDTO.PromoterCode = item.PromoterCode;
                            item.PromoterMasterDTO.PromoterName = enq.PromoterDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                        }
                        enquiryDTO.PromoterAllDetailsDTO.PromotersAssetLiabilityDetails.ListLiabilityDetails = enq.PromoterLiability.ToList();
                        _sessionManager.SetPromoterLiabilityList(enq.PromoterLiability.ToList());
                    }
                    if (enq.GuarantorDetails != null && enq.GuarantorDetails.Any())
                    {
                        foreach (var item in enq.GuarantorDetails)
                        {
                            var domMasterData = enquiryDTO.DDLDTO.ListDomicileStatus.
                                 FirstOrDefault(x => x.Value == item.DomCd.ToString());
                            item.DomicileMasterDTO = new DomicileMasterDTO();
                            item.DomicileMasterDTO.DomDets = domMasterData.Text;
                        }
                        enquiryDTO.GuarantorAllDetailsDTO.ListGuarantor = enq.GuarantorDetails.ToList();
                        _sessionManager.SetGuarantorDetailsList(enq.GuarantorDetails.ToList());
                    }
                    if (enq.GuarantorAssetsDetails != null && enq.GuarantorAssetsDetails.Any())
                    {
                        foreach (var item in enq.GuarantorAssetsDetails)
                        {
                            var AssetCategorymasterData = enquiryDTO.DDLDTO.ListAssetCategory.
                                FirstOrDefault(x => x.Value == item.AssetcatCd.ToString());
                            item.AssetCategoryMasterDTO = new AssetCategoryMasterDTO();
                            item.AssetCategoryMasterDTO.AssetcatCd = Convert.ToInt32(AssetCategorymasterData.Value);
                            item.AssetCategoryMasterDTO.AssetcatDets = AssetCategorymasterData.Text;

                            var AssetTypemasterData = enquiryDTO.DDLDTO.ListAssetType.
                               FirstOrDefault(x => x.Value == item.AssettypeCd.ToString());
                            item.AssetTypeMasterDTO = new AssetTypeMasterDTO();
                            item.AssetTypeMasterDTO.AssettypeCd = Convert.ToInt32(AssetTypemasterData.Value);
                            item.AssetTypeMasterDTO.AssettypeDets = AssetTypemasterData.Text;

                            item.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                            item.GuarantorDetailsDTO.PromoterCode = item.PromoterCode.Value;
                            item.GuarantorDetailsDTO.GuarName = enq.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                        }
                        enquiryDTO.GuarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.ListAssetDetails = enq.GuarantorAssetsDetails.ToList();
                        _sessionManager.SetGuarantorAssetList(enq.GuarantorAssetsDetails.ToList());
                    }
                    if (enq.GuarantorLiability != null && enq.GuarantorLiability.Any())
                    {
                        foreach (var item in enq.GuarantorLiability)
                        {
                            item.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                            item.GuarantorDetailsDTO.PromoterCode = item.PromoterCode;
                            item.GuarantorDetailsDTO.GuarName = enq.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                        }
                        enquiryDTO.GuarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.ListLiabilityDetails = enq.GuarantorLiability.ToList();
                        _sessionManager.SetGuarantorLiabilityList(enq.GuarantorLiability.ToList());
                    }


                    if (enq.PromoterDetails != null && enq.PromoterDetails.Any())
                    {
                        ViewBag.PromoterTab = "bg-success";
                    }
                    #endregion

                    #region Assosiate Concern
                    ViewBag.SisterConcern = "bg-warning";
                    enquiryDTO.AssociateConcernDetails = new AssociateSisterConcernDetailsDTO
                    {
                        ListAssociates = new List<SisterConcernDetailsDTO>(),
                        ListFYDetails = new List<SisterConcernFinancialDetailsDTO>()
                    };
                    if (enq.SisterConcernDetails != null && enq.SisterConcernDetails.Any())
                    {
                        enquiryDTO.AssociateConcernDetails.ListAssociates = enq.SisterConcernDetails.ToList();

                        _sessionManager.SetAssociateDetailsDTOList(enq.SisterConcernDetails.ToList());

                    }
                    if (enq.SisterConcernFinancialDetails != null && enq.SisterConcernFinancialDetails.Any())
                    {
                        enquiryDTO.AssociateConcernDetails.ListFYDetails = enq.SisterConcernFinancialDetails.ToList();
                        _sessionManager.SetAssociatePrevFYDetailsList(enq.SisterConcernFinancialDetails.ToList());
                        ViewBag.SisterConcern = "bg-success";
                    }

                    #endregion

                    #region Project Tab

                    if (enq.ProjectCostDetails != null && enq.ProjectCostDetails.Any())
                    {
                        _sessionManager.SetProjectCostList(enq.ProjectCostDetails.ToList());
                    }
                    if (enq.ProjectMeansOfFinanceDetails != null && enq.ProjectMeansOfFinanceDetails.Any())
                    {
                        _sessionManager.SetProjectMeansOfFinanceList(enq.ProjectMeansOfFinanceDetails.ToList());
                    }
                    if (enq.ProjectFinancialYearDetails != null && enq.ProjectFinancialYearDetails.Any())
                    {
                        _sessionManager.SetProjectPrevFYDetailsList(enq.ProjectFinancialYearDetails.ToList());
                    }

                    if (enq.WorkingCapitalDetails != null && enq.ProjectCostDetails.Count() > 0 && enq.ProjectMeansOfFinanceDetails.Count() > 0 && enq.ProjectFinancialYearDetails.Count() > 0)
                    {
                        ViewBag.ProjectTab = "bg-success";
                    }
                    else
                    {
                        ViewBag.ProjectTab = "bg-warning";
                    }


                    if (enq.SecurityDetails != null && enq.SecurityDetails.Any())
                    {
                        _sessionManager.SetSecurityDetailsList(enq.SecurityDetails.ToList());
                    }
                    if (enq.DocumentList != null && enq.DocumentList.Any())
                    {
                        _sessionManager.SetDocuments(enq.DocumentList.ToList());
                    }

                    if (enq.DocumentList != null && enq.DocumentList.Count() == 8)
                    {
                        ViewBag.SecurityTab = "bg-success";
                    }
                    else
                    {
                        ViewBag.SecurityTab = "bg-warning";
                    }


                    enquiryDTO.ProjectDetails.CapitalDtls = enq.WorkingCapitalDetails;
                    enquiryDTO.ProjectDetails.ListPrjctCost = enq.ProjectCostDetails == null ? new List<ProjectCostDetailsDTO>() : enq.ProjectCostDetails.ToList();
                    enquiryDTO.ProjectDetails.ListMeansOfFinance = enq.ProjectMeansOfFinanceDetails == null ? new List<ProjectMeansOfFinanceDTO>() : enq.ProjectMeansOfFinanceDetails.ToList();
                    enquiryDTO.ProjectDetails.ListPrevYearFinDetails = enq.ProjectFinancialYearDetails == null ? new List<ProjectFinancialYearDetailsDTO>() : enq.ProjectFinancialYearDetails.ToList();
                    enquiryDTO.SecurityDetails = enq.SecurityDetails == null ? new List<SecurityDetailsDTO>() : enq.SecurityDetails;
                    enquiryDTO.DocumentList = enq.DocumentList == null ? new List<EnqDocumentDTO>() : enq.DocumentList;

                    if (enquiryDTO.Status > 0)
                    {
                        ViewBag.ReviewTab = "bg-success";
                    }
                    else
                    {
                        ViewBag.ReviewTab = "bg-warning";

                    }
                    #endregion

                    _logger.Information(string.Format("Completed - Edit with EnquiryId {0} PromotorPan :{1} EnqSubmitDate :{2} EnqStatus :{3} EnqInitiateDate :{4} "
                                                          , model.EnquiryId, model.PromotorPan, model.EnqSubmitDate, model.EnqStatus, model.EnqInitiateDate));

                    return View(enquiryDTO);
                }
                _logger.Information(string.Format("Completed - Edit with EnquiryId {0} PromotorPan :{1} EnqSubmitDate :{2} EnqStatus :{3} EnqInitiateDate :{4} "
                                                          , model.EnquiryId, model.PromotorPan, model.EnqSubmitDate, model.EnqStatus, model.EnqInitiateDate));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! Edit HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> View(EnquirySummary model)
        {

            try
            {
                _logger.Information(string.Format("Started - View with EnquiryId {0} PromotorPan :{1} EnqSubmitDate :{2} EnqStatus :{3} EnqInitiateDate :{4} "
                                                          , model.EnquiryId, model.PromotorPan, model.EnqSubmitDate, model.EnqStatus, model.EnqInitiateDate));

                if (model.EnquiryId != null && model.EnquiryId > 0)
                {
                    _sessionManager.SetAllEntitiesToEmptyString();
                    _sessionManager.SetNewEnqTempId(model.EnquiryId.Value.ToString());
                    _sessionManager.SetOperationType("Edit");
                    EnquiryDTO enquiryDTO = new();
                    #region UnitTab
                    ViewBag.UnitTab = "bg-warning";
                    List<SelectListItem> listAddressTypes = await _enquiryService.getAllAddressTypesFromDB();
                    _sessionManager.SetAddressTypesFromDB(listAddressTypes);
                    enquiryDTO.DDLDTO = await _enquiryService.GetAllEnquiryDropDownList();
                    //Populating Dropdown List of BasicDetails Presonal Details page
                    ViewBag.ListBranch = enquiryDTO.DDLDTO.ListBranch;
                    ViewBag.ListLoanPurpose = enquiryDTO.DDLDTO.ListLoanPurpose;
                    ViewBag.ListFirmSize = enquiryDTO.DDLDTO.ListFirmSize;
                    ViewBag.ListProduct = enquiryDTO.DDLDTO.ListProduct;
                    ViewBag.ListDistrict = enquiryDTO.DDLDTO.ListDistrict;
                    ViewBag.ListPremises = enquiryDTO.DDLDTO.ListPremises;

                    // Populating Dropdown List of BasicDetails Bank Details page
                    _sessionManager.SetDDListDomicileStatus(enquiryDTO.DDLDTO.ListDomicileStatus);
                    _sessionManager.SetDDLBankFacilityType(enquiryDTO.DDLDTO.ListFacility);

                    _sessionManager.SetDDListFinancialYear(enquiryDTO.DDLDTO.ListFY);
                    _sessionManager.SetDDListFinancialComponent(enquiryDTO.DDLDTO.ListFinancialComponent);
                    enquiryDTO.ProjectDetails = new ProjectAllDetailsDTO
                    {
                        ListPrjctCost = new List<ProjectCostDetailsDTO>(),
                        ListMeansOfFinance = new List<ProjectMeansOfFinanceDTO>(),
                        ListPrevYearFinDetails = new List<ProjectFinancialYearDetailsDTO>()
                    };
                    _sessionManager.SetDDListProjectCostComponent(enquiryDTO.DDLDTO.ListProjectCost);
                    _sessionManager.SetDDListProjectMeansOfFinance(enquiryDTO.DDLDTO.ListMeansOfFinanceCategory);
                    _sessionManager.SetDDListProjectFinanceType(enquiryDTO.DDLDTO.ListMeansOfFinanceType);
                    _sessionManager.SetDDListTypeOfSecurity(enquiryDTO.DDLDTO.ListSecurityType);
                    _sessionManager.SetDDListSecurityDetailsType(enquiryDTO.DDLDTO.ListSecurityDet);
                    _sessionManager.SetDDListRelationType(enquiryDTO.DDLDTO.ListSecurityRelation);
                    var enq = await _enquiryService.getEnquiryDetails(model.EnquiryId.Value);
                    var ListTaluka = new List<SelectListItem>();
                    var ListHobli = new List<SelectListItem>();
                    var ListVillage = new List<SelectListItem>();
                    if (enq.BasicDetails != null)
                    {
                        var getcascadePrefillDDLList = await _enquiryService.getCascadeDDLForEditPrefill(enq.BasicDetails.DistrictCd, enq.BasicDetails.TalukaCd,
                        enq.BasicDetails.HobliCd);
                        ListTaluka = getcascadePrefillDDLList.ListTaluka;
                        ListHobli = getcascadePrefillDDLList.ListHobli;
                        ListVillage = getcascadePrefillDDLList.ListVillage;

                        enquiryDTO.UnitDetails = new UnitDetailsDTO
                        {
                            BasicDetails = enq.BasicDetails,
                            BankDetails = enq.BankDetails,
                            ListRegDetails = enq.RegistrationDetails.ToList(),
                            ListAddressDetail = enq.AddressDetails.ToList()
                        };

                        foreach (var item in enq.AddressDetails)
                        {
                            item.AddressTypeMasterDTO = new AddressTypeMasterDTO
                            {
                                AddtypeCd = item.AddtypeCd,
                                AddtypeDets = listAddressTypes.Find(a => a.Value == item.AddtypeCd.ToString()).Text
                            };
                        }
                        var regTypes = enquiryDTO.DDLDTO.ListRegnType;
                        foreach (var item in enq.RegistrationDetails)
                        {
                            item.RegTypeText = regTypes.FindAll(x => x.Value == item.RegrefCd.ToString()).FirstOrDefault().Text;
                        }
                        if (enq.AddressDetails.Any())
                        {
                            _sessionManager.SetAddressList(enq.AddressDetails.ToList());
                        }
                        if (enq.RegistrationDetails.Any())
                        {
                            _sessionManager.SetRegistrationDetList(enq.RegistrationDetails.ToList());
                        }
                    }
                    else
                    {
                        enquiryDTO.UnitDetails = new UnitDetailsDTO();
                        enquiryDTO.UnitDetails.ListAddressDetail = new List<AddressDetailsDTO>();
                        enquiryDTO.UnitDetails.ListRegDetails = new List<RegistrationNoDetailsDTO>();
                    }
                    ViewBag.ListTaluka = ListTaluka;
                    ViewBag.ListHobli = ListHobli;
                    ViewBag.ListVillage = ListVillage;

                    if (enq.BasicDetails != null)
                    {
                        ViewBag.UnitTab = "bg-success";
                    }
                    #endregion

                    #region Promotor and Gurantor
                    ViewBag.PromoterTab = "bg-warning";
                    //Adding dropdownlist data to Session to be used by Promoter and Guar page ddl
                    _sessionManager.SetDDListPromoterDesignation(enquiryDTO.DDLDTO.ListPromDesgnType);
                    _sessionManager.SetDDListPromAndGuarAssetCategory(enquiryDTO.DDLDTO.ListAssetCategory);
                    _sessionManager.SetDDListPromAndGuarAssetType(enquiryDTO.DDLDTO.ListAssetType);
                    _sessionManager.SetDDListModeOfAcquire(enquiryDTO.DDLDTO.ListAcquireMode);
                    enquiryDTO.PromoterAllDetailsDTO = new PromoterAllDetailsDTO
                    {
                        ListPromoters = new List<PromoterDetailsDTO>(),
                        PromotersAssetLiabilityDetails = new PromAssetLiabilityDetailsDTO { ListAssetDetails = new List<PromoterAssetsNetWorthDTO>(), ListLiabilityDetails = new List<PromoterLiabilityDetailsDTO>() }
                    };
                    enquiryDTO.GuarantorAllDetailsDTO = new GuarantorAllDetailsDTO
                    {
                        ListGuarantor = new List<GuarantorDetailsDTO>(),
                        GuarantorAssetLiabilityDetails = new GuarAssetLiabilityDetailsDTO { ListAssetDetails = new List<GuarantorAssetsNetWorthDTO>(), ListLiabilityDetails = new List<GuarantorLiabilityDetailsDTO>() }
                    };
                    if (enq.PromoterDetails != null && enq.PromoterDetails.Any())
                    {
                        enquiryDTO.PromoterAllDetailsDTO.ListPromoters = enq.PromoterDetails.ToList();
                        _sessionManager.SetPromoterDetailsList(enq.PromoterDetails.ToList());
                    }
                    if (enq.PromoterAssetsDetails != null && enq.PromoterAssetsDetails.Any())
                    {
                        foreach (var item in enq.PromoterAssetsDetails)
                        {
                            var AssetCategorymasterData = enquiryDTO.DDLDTO.ListAssetCategory.
                                FirstOrDefault(x => x.Value == item.AssetcatCd.ToString());
                            item.AssetCategoryMasterDTO = new AssetCategoryMasterDTO();
                            item.AssetCategoryMasterDTO.AssetcatCd = Convert.ToInt32(AssetCategorymasterData.Value);
                            item.AssetCategoryMasterDTO.AssetcatDets = AssetCategorymasterData.Text;

                            var AssetTypemasterData = enquiryDTO.DDLDTO.ListAssetType.
                               FirstOrDefault(x => x.Value == item.AssettypeCd.ToString());
                            item.AssetTypeMasterDTO = new AssetTypeMasterDTO();
                            item.AssetTypeMasterDTO.AssettypeCd = Convert.ToInt32(AssetTypemasterData.Value);
                            item.AssetTypeMasterDTO.AssettypeDets = AssetTypemasterData.Text;

                            item.PromoterMasterDTO = new PromoterMasterDTO();
                            item.PromoterMasterDTO.PromoterCode = item.PromoterCode.Value;
                            item.PromoterMasterDTO.PromoterName = enq.PromoterDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                        }

                        enquiryDTO.PromoterAllDetailsDTO.PromotersAssetLiabilityDetails.ListAssetDetails = enq.PromoterAssetsDetails.ToList();
                        _sessionManager.SetPromoterAssetList(enq.PromoterAssetsDetails.ToList());
                    }
                    if (enq.PromoterLiability != null && enq.PromoterLiability.Any())
                    {
                        foreach (var item in enq.PromoterLiability)
                        {
                            item.PromoterMasterDTO = new PromoterMasterDTO();
                            item.PromoterMasterDTO.PromoterCode = item.PromoterCode;
                            item.PromoterMasterDTO.PromoterName = enq.PromoterDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                        }
                        enquiryDTO.PromoterAllDetailsDTO.PromotersAssetLiabilityDetails.ListLiabilityDetails = enq.PromoterLiability.ToList();
                        _sessionManager.SetPromoterLiabilityList(enq.PromoterLiability.ToList());
                    }
                    if (enq.GuarantorDetails != null && enq.GuarantorDetails.Any())
                    {
                        foreach (var item in enq.GuarantorDetails)
                        {
                            var domMasterData = enquiryDTO.DDLDTO.ListDomicileStatus.
                                 FirstOrDefault(x => x.Value == item.DomCd.ToString());
                            item.DomicileMasterDTO = new DomicileMasterDTO();
                            item.DomicileMasterDTO.DomDets = domMasterData.Text;
                        }
                        enquiryDTO.GuarantorAllDetailsDTO.ListGuarantor = enq.GuarantorDetails.ToList();
                        _sessionManager.SetGuarantorDetailsList(enq.GuarantorDetails.ToList());
                    }
                    if (enq.GuarantorAssetsDetails != null && enq.GuarantorAssetsDetails.Any())
                    {
                        foreach (var item in enq.GuarantorAssetsDetails)
                        {
                            var AssetCategorymasterData = enquiryDTO.DDLDTO.ListAssetCategory.
                                FirstOrDefault(x => x.Value == item.AssetcatCd.ToString());
                            item.AssetCategoryMasterDTO = new AssetCategoryMasterDTO();
                            item.AssetCategoryMasterDTO.AssetcatCd = Convert.ToInt32(AssetCategorymasterData.Value);
                            item.AssetCategoryMasterDTO.AssetcatDets = AssetCategorymasterData.Text;

                            var AssetTypemasterData = enquiryDTO.DDLDTO.ListAssetType.
                               FirstOrDefault(x => x.Value == item.AssettypeCd.ToString());
                            item.AssetTypeMasterDTO = new AssetTypeMasterDTO();
                            item.AssetTypeMasterDTO.AssettypeCd = Convert.ToInt32(AssetTypemasterData.Value);
                            item.AssetTypeMasterDTO.AssettypeDets = AssetTypemasterData.Text;

                            item.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                            item.GuarantorDetailsDTO.PromoterCode = item.PromoterCode.Value;
                            item.GuarantorDetailsDTO.GuarName = enq.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                        }
                        enquiryDTO.GuarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.ListAssetDetails = enq.GuarantorAssetsDetails.ToList();
                        _sessionManager.SetGuarantorAssetList(enq.GuarantorAssetsDetails.ToList());
                    }
                    if (enq.GuarantorLiability != null && enq.GuarantorLiability.Any())
                    {
                        foreach (var item in enq.GuarantorLiability)
                        {
                            item.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                            item.GuarantorDetailsDTO.PromoterCode = item.PromoterCode;
                            item.GuarantorDetailsDTO.GuarName = enq.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                        }
                        enquiryDTO.GuarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.ListLiabilityDetails = enq.GuarantorLiability.ToList();
                        _sessionManager.SetGuarantorLiabilityList(enq.GuarantorLiability.ToList());
                    }


                    if (enq.PromoterDetails != null && enq.PromoterDetails.Any())
                    {
                        ViewBag.PromoterTab = "bg-success";
                    }
                    #endregion

                    #region Assosiate Concern
                    ViewBag.SisterConcern = "bg-warning";
                    enquiryDTO.AssociateConcernDetails = new AssociateSisterConcernDetailsDTO
                    {
                        ListAssociates = new List<SisterConcernDetailsDTO>(),
                        ListFYDetails = new List<SisterConcernFinancialDetailsDTO>()
                    };
                    if (enq.SisterConcernDetails != null && enq.SisterConcernDetails.Any())
                    {
                        enquiryDTO.AssociateConcernDetails.ListAssociates = enq.SisterConcernDetails.ToList();

                        _sessionManager.SetAssociateDetailsDTOList(enq.SisterConcernDetails.ToList());

                    }
                    if (enq.SisterConcernFinancialDetails != null && enq.SisterConcernFinancialDetails.Any())
                    {
                        enquiryDTO.AssociateConcernDetails.ListFYDetails = enq.SisterConcernFinancialDetails.ToList();
                        _sessionManager.SetAssociatePrevFYDetailsList(enq.SisterConcernFinancialDetails.ToList());
                        ViewBag.SisterConcern = "bg-success";
                    }

                    #endregion

                    #region Project Tab

                    if (enq.ProjectCostDetails != null && enq.ProjectCostDetails.Any())
                    {
                        _sessionManager.SetProjectCostList(enq.ProjectCostDetails.ToList());
                    }
                    if (enq.ProjectMeansOfFinanceDetails != null && enq.ProjectMeansOfFinanceDetails.Any())
                    {
                        _sessionManager.SetProjectMeansOfFinanceList(enq.ProjectMeansOfFinanceDetails.ToList());
                    }
                    if (enq.ProjectFinancialYearDetails != null && enq.ProjectFinancialYearDetails.Any())
                    {
                        _sessionManager.SetProjectPrevFYDetailsList(enq.ProjectFinancialYearDetails.ToList());
                    }

                    if (enq.WorkingCapitalDetails != null && enq.ProjectCostDetails.Count() > 0 && enq.ProjectMeansOfFinanceDetails.Count() > 0 && enq.ProjectFinancialYearDetails.Count() > 0)
                    {
                        ViewBag.ProjectTab = "bg-success";
                    }
                    else
                    {
                        ViewBag.ProjectTab = "bg-warning";
                    }


                    if (enq.SecurityDetails != null && enq.SecurityDetails.Any())
                    {
                        _sessionManager.SetSecurityDetailsList(enq.SecurityDetails.ToList());
                    }
                    if (enq.DocumentList != null && enq.DocumentList.Any())
                    {
                        _sessionManager.SetDocuments(enq.DocumentList.ToList());
                    }

                    if (enq.DocumentList != null && enq.DocumentList.Count() == 8)
                    {
                        ViewBag.SecurityTab = "bg-success";
                    }
                    else
                    {
                        ViewBag.SecurityTab = "bg-warning";
                    }


                    enquiryDTO.ProjectDetails.CapitalDtls = enq.WorkingCapitalDetails;
                    enquiryDTO.ProjectDetails.ListPrjctCost = enq.ProjectCostDetails == null ? new List<ProjectCostDetailsDTO>() : enq.ProjectCostDetails.ToList();
                    enquiryDTO.ProjectDetails.ListMeansOfFinance = enq.ProjectMeansOfFinanceDetails == null ? new List<ProjectMeansOfFinanceDTO>() : enq.ProjectMeansOfFinanceDetails.ToList();
                    enquiryDTO.ProjectDetails.ListPrevYearFinDetails = enq.ProjectFinancialYearDetails == null ? new List<ProjectFinancialYearDetailsDTO>() : enq.ProjectFinancialYearDetails.ToList();
                    enquiryDTO.SecurityDetails = enq.SecurityDetails == null ? new List<SecurityDetailsDTO>() : enq.SecurityDetails;
                    enquiryDTO.DocumentList = enq.DocumentList == null ? new List<EnqDocumentDTO>() : enq.DocumentList;

                    if (enquiryDTO.Status == (int)EnqStatus.Submitted)
                    {
                        ViewBag.ReviewTab = "bg-success";
                    }
                    else
                    {
                        ViewBag.ReviewTab = "bg-warning";

                    }
                    #endregion

                    if (enquiryDTO.Status > 0)
                    {
                        ViewBag.ReviewTab = "bg-success";
                    }
                    else
                    {
                        ViewBag.ReviewTab = "bg-warning";

                    }
                    _logger.Information(string.Format("Completed - View with EnquiryId {0} PromotorPan :{1} EnqSubmitDate :{2} EnqStatus :{3} EnqInitiateDate :{4} "
                                                          , model.EnquiryId, model.PromotorPan, model.EnqSubmitDate, model.EnqStatus, model.EnqInitiateDate));

                    return View(enquiryDTO);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! View HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveProjectDetails(ProjectWorkingCapitalDeatailsDTO workingCapArrDetails)
        {
            try
            {
                _logger.Information(string.Format("Started - SaveProjectDetails with EnqWcBank {0} EnqWcBranch :{1} EnqWcIfsc :{2} EnqWcAmt :{3} UniqueId :{4} "
                                                                 , workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcIfsc,
                                                                 workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId));

                EnquiryDTO enquiryDTO = new();
                if (!ModelState.IsValid)
                {
                    return Json(new { isValid = false, invalidAccordion = "ProjectDetails", Message = "Please enter the basic details before submitting the form." });
                }
                _sessionManager.SetProjectWorkingCapitalArrDetails(workingCapArrDetails);

                var workingCapital = _sessionManager.GetProjectWorkingCapitalArrDetails();
                var projectCost = _sessionManager.GetProjectCostList();
                var meansOfFinance = _sessionManager.GetProjectMeansOfFinanceList();
                var prevFYDetails = _sessionManager.GetProjectPrevFYDetailsList();

                if (workingCapital == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "WorkingCapital", Message = "Please enter the Working Capital details before submitting the form." });
                }
                else if (projectCost == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "projectCost", Message = "Please add Project Cost before submitting the form." });
                }
                else if (meansOfFinance == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "meansOfFinance", Message = "Please enter Means of Finance details before submitting the form." });
                }
                else if (prevFYDetails == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "prevFYDetails", Message = "Please enter Previous Financial Details details before submitting the form." });
                }

                var task1 = await _enquiryService.saveCapitalDetails(workingCapital);
                if (!task1)
                    return new JsonResult(new { isValid = false, message = "Please check your working Capital details." });

                var task2 = await _enquiryService.saveProjectCostDetails(projectCost);
                if (!task2)
                    return new JsonResult(new { isValid = false, message = "Please check your Project cost details." });

                var task3 = await _enquiryService.saveProjectMeansOfFinanceDetails(meansOfFinance);
                if (!task3)
                    return new JsonResult(new { isValid = false, message = "Please check your Means of finance details." });

                var task4 = await _enquiryService.saveProjectPrevYearFinDetailsDetails(prevFYDetails);
                if (!task4)
                    return new JsonResult(new { isValid = false, message = "Please check your previous year financial details." });

                _logger.Information(string.Format("Completed - SaveProjectDetails with EnqWcBank {0} EnqWcBranch :{1} EnqWcIfsc :{2} EnqWcAmt :{3} UniqueId :{4} "
                                                         , workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcIfsc,
                                                         workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId));

                return Json(new { isValid = true });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! SaveProjectDetails HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        

        [HttpPost]
        public async Task<IActionResult> SaveSecurityAndDocsForEnquiry()
        {
            try
            {
                _logger.Information("Started - SaveSecurityAndDocsForEnquiry");

                //get all details from session for s&D
                var SecurityDetails = _sessionManager.GetSecurityDetailsList();
                if (SecurityDetails == null || SecurityDetails.Count() == 0)
                {
                    return Json(new { isValid = false, invalidAccordion = "SecurityDetails", Message = "Please enter the Security details before submitting the form." });
                }
                var documentDetails = _sessionManager.GetDocuments();
                if (documentDetails == null || documentDetails.Count() != 8)
                {
                    return Json(new { isValid = false, invalidAccordion = "SecurityDetails", Message = "Please upload all documents before submitting the form." });

                }
                var result = await _enquiryService.SaveSecurityDetails(SecurityDetails).ConfigureAwait(false);
                if (result.Any())
                {
                    _logger.Information("Completed - SaveSecurityAndDocsForEnquiry");

                    return Json(new { isValid = true, Message = "Successful" });
                }

                return Json(new { isValid = false, Message = "Please enter the Security details before submitting the form." });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! SaveSecurityAndDocsForEnquiry HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrEditUnitDetailsForEnquiry(BasicDetailsDto basicDetailsDTO, BankDetailsDTO bankDetailsDTO)
        {
            try
            {
                _logger.Information("Started - SaveOrEditUnitDetailsForEnquiry with BasicDetailsDto and bankDetailsDTO");

                var ListAddressDetail = _sessionManager.GetAddressList();
                var ListRegDetails = _sessionManager.GetRegistrationDetList();
                var AddressTypes = _sessionManager.GetAddressTypesFromDB();

                //return result in such a way that UI should be able to identify the error and expand accordion and highlight the field
                if (!TryValidateModel(basicDetailsDTO))
                {
                    return Json(new { isValid = false, invalidAccordion = "BasicDetails", Message = "Please enter the basic and bank details before submitting the form." });
                }
                else if (ListAddressDetail == null || ListAddressDetail.Count == 0 || ListAddressDetail.Count < AddressTypes.Count)
                {
                    return new JsonResult(new { isValid = false, invalidAccordion = "Address", Message = "Please add all address types before submitting the form." });
                }
                else if (!TryValidateModel(bankDetailsDTO))
                {
                    return Json(new { isValid = false, invalidAccordion = "Bank", Message = "Please enter Bank details before submitting the form." });
                }
                else if (ListRegDetails == null || ListRegDetails.Count == 0)
                {
                    return new JsonResult(new { isValid = false, invalidAccordion = "Registration", Message = "Please enter atleast one registration details before submitting the form." });
                }


                if (_sessionManager.GetUDPersonalDetails() != null)
                {
                    var basic = _sessionManager.GetUDPersonalDetails();
                    var bank = _sessionManager.GetUDBankDetails();
                    var EnquiryId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                    basicDetailsDTO.EnqtempId = EnquiryId;
                    bankDetailsDTO.EnqtempId = EnquiryId;
                    bankDetailsDTO.EnqBankId = bank.EnqBankId;
                    basicDetailsDTO.EnqBdetId = basic.EnqBdetId;
                    ListAddressDetail.ForEach(a => a.EnqtempId = EnquiryId);
                    ListRegDetails.ForEach(a => a.EnqtempId = EnquiryId);
                }

                _sessionManager.SetUDPersonalDetails(basicDetailsDTO);
                _sessionManager.SetUDBankDetails(bankDetailsDTO);
                UnitDetailsDTO unitDetailsDTO = new()
                {
                    BasicDetails = basicDetailsDTO,
                    ListAddressDetail = ListAddressDetail,
                    BankDetails = bankDetailsDTO,
                    ListRegDetails = ListRegDetails
                };

                unitDetailsDTO.ListAddressDetail.ForEach(a => a.EnqAddresssId = null); //Making Id=null for saving new record in DB otherwise it will throw exception id id is already present in DB
                unitDetailsDTO.ListRegDetails.ForEach(a => a.EnqRegnoId = null);//Making Id=null for saving new record in DB otherwise it will throw exception id id is already present in DB

                //Calling Api to Save Data
                var task1 = await _enquiryService.SaveUnitDetailsBasicDetails(unitDetailsDTO.BasicDetails, true);
                if (task1 == false)
                    return new JsonResult(new { isValid = false, invalidAccordion = "BasicDetails", message = "Please check your basic Details." });
                var task2 = await _enquiryService.SaveUnitDetailsAddressDetails(unitDetailsDTO.ListAddressDetail, true);
                if (task2 == false)
                    return new JsonResult(new { isValid = false, invalidAccordion = "Address", message = "Please check your Address Details List." });
                var task3 = await _enquiryService.SaveUnitDetailsBankDetails(unitDetailsDTO.BankDetails, true);
                if (task3 == false)
                    return new JsonResult(new { isValid = false, invalidAccordion = "Bank", message = "Please check your Bank Details." });
                var task4 = await _enquiryService.SaveUnitDetailsRegistrationDetails(unitDetailsDTO.ListRegDetails, true);
                if (task4 == false)
                    return new JsonResult(new { isValid = false, invalidAccordion = "Registration", message = "Please check your Registration Details List." });

                _logger.Information("Completed - SaveOrEditUnitDetailsForEnquiry with BasicDetailsDto and bankDetailsDTO");

                return Json(new { isValid = true });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! SaveOrEditUnitDetailsForEnquiry HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveOrEditPromAndGuarDetailsForEnquiry()
        {
            //Async Call API to save prom details
            //Async Call API to save guar details
            //return result in such a way that UI should be able to identify the error and expand accordion and highlight the field

            try
            {
                _logger.Information("Started - SaveOrEditPromAndGuarDetailsForEnquiry");

                if (_sessionManager.GetPromoterDetailsList() == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "PromoterBasic", Message = "Please enter promoter details before submitting the form." });
                }
                else if (_sessionManager.GetPromoterAssetList() == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "PromoterAsset", Message = "Please enter promoter asset details before submitting the form." });
                }
                else if (_sessionManager.GetPromoterLiabilityList() == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "PromoterLiability", Message = "Please enter promoter liability details before submitting the form." });
                }
                else if (_sessionManager.GetGuarantorDetailsList() == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "GuarantorDetails", Message = "Please enter guarantor details before submitting the form." });
                }
                else if (_sessionManager.GetGuarantorAssetList() == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "GuarantorAsset", Message = "Please enter guarantor asset details before submitting the form." });
                }
                else if (_sessionManager.GetGuarantorLiabilityList() == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "GuarantorLiability", Message = "Please enter guarantor liability details before submitting the form." });
                }
                List<PromoterAssetsNetWorthDTO> promoterAssetDetails = _sessionManager.GetPromoterAssetList();
                List<PromoterLiabilityDetailsDTO> promoterLiabilityDetails = _sessionManager.GetPromoterLiabilityList();
                var task1 = await _enquiryService.SaveorEditPromoterAssetsNetWorthDetails(promoterAssetDetails);
                var task2 = await _enquiryService.SaveorEditPromoterLiabilityDetails(promoterLiabilityDetails);
                // var task4 = await _enquirySubmissionService.SaveorEditPromoterLiabilityNetWorth(new List<PromoterNetWorthDetailsDTO>());
                List<GuarantorAssetsNetWorthDTO> GuarantorAssetsNetWorthDTO = _sessionManager.GetGuarantorAssetList();
                List<GuarantorLiabilityDetailsDTO> GuarantorLiabilityDetailsDTO = _sessionManager.GetGuarantorLiabilityList();

                var task3 = await _enquiryService.SaveorEditGuarantorAssetsNetWorthDetails(GuarantorAssetsNetWorthDTO);
                var task4 = await _enquiryService.SaveorEditGuarantorLiabilityDetails(GuarantorLiabilityDetailsDTO);
                // var task8 = await _enquirySubmissionService.SaveorEditGuarantorLiabilityNetWorth(new List<GuarantorNetWorthDetailsDTO>());

                _logger.Information("Completed - SaveOrEditPromAndGuarDetailsForEnquiry");


                return Json(new { isValid = true });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! SaveOrEditPromAndGuarDetailsForEnquiry HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> SaveOrEditAssociateDetailsForEnquiry()
        {
            //Async Call API to save Associate / Sister Concern Detail
            //Async Call API to save Previous years Financial details
            //return result in such a way that UI should be able to identify the error and expand accordion and highlight the field

            try
            {
                _logger.Information("Started - SaveOrEditAssociateDetailsForEnquiry");

                if (_sessionManager.GetAssociateDetailsDTOList() == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "Associate", Message = "Please enter Associate details before submitting the form." });
                }
                else if (_sessionManager.GetAssociatePrevFYDetailsList() == null)
                {
                    return Json(new { isValid = false, invalidAccordion = "PrevFYDetails", Message = "Please enter Associate previous financial year details before submitting the form." });
                }

                AssociateSisterConcernDetailsDTO associateSisterConcernDetailsDTO = new()
                {
                    ListAssociates = _sessionManager.GetAssociateDetailsDTOList(),
                };

                //Calling Api to Save Data
                // var task1 = await _enquiryService.SaveAssociateSisterDetails(associateSisterConcernDetailsDTO.ListAssociates);
                associateSisterConcernDetailsDTO.ListFYDetails = _sessionManager.GetAssociatePrevFYDetailsList();
                var task2 = await _enquiryService.SaveAssociateSisterFYDetails(associateSisterConcernDetailsDTO.ListFYDetails);

                _logger.Information("Completed - SaveOrEditAssociateDetailsForEnquiry");

                return Json(new { isValid = true });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! SaveOrEditPromAndGuarDetailsForEnquiry HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GenerateOtpForReview()
        {
            try
            {

                string mobileNo = _sessionManager.GetCustMobile();

                _logger.Information("Started - GenerateOtpForReview with mobile no." + mobileNo);

                _otpService.GetOtpAttempts();
                var response = await _otpService.Generate("ReviewE", mobileNo, null, null);

                _logger.Information("Completed - GenerateOtpForReview with mobile no." + mobileNo);

                return response;
            }
            catch(Exception ex)
            {
                _logger.Error("Error occured! GenerateOtpForReview HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ResendOtpForReview()
        {
            try
            {
                string mobileNo = _sessionManager.GetCustMobile();

                _logger.Information("Started - ResendOtpForReview with mobile no." + mobileNo);

                var response = await _otpService.Resend("ReviewE", mobileNo, null, null);

                _logger.Information("Completed - ResendOtpForReview with mobile no." + mobileNo);

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ResendOtpForReview HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOtpForReview(string entOtp)
        {
            try
            {
                string mobileNum = _sessionManager.GetCustMobile();

                _logger.Information("Started - ValidateOtpForReview with mobile no." + mobileNum);

                var response = await _otpService.Validate("ReviewE", mobileNum, entOtp);

                _logger.Information("Completed - ValidateOtpForReview with mobile no." + mobileNum);

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ValidateOtpForReview HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        [HttpPost]
        public async Task<IActionResult> SubmitEnquiry(string sNote)
        {
            try
            {
                _logger.Information("Started - SubmitEnquiry with sNote" + sNote);

                var result = await _enquiryService.SubmitEnquiry(sNote);
                if (result > 0)
                {
                    _logger.Information("Completed - SubmitEnquiry with sNote" + sNote);

                    return new JsonResult(new { isValid = true, Message = $"Enquiry Ref. No. {result} <br/> Enquiry Ref.No.sent to E - Mail :   and Mobile No. :{ _sessionManager.GetCustMobile()}" });
                }
                return new JsonResult(new { isValid = false, Message = $"Some Error occured. Please try again" });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! SubmitEnquiry HttpPost page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public async Task<IActionResult> ReviewEnquiry()
        {
            var tempId = _sessionManager.GetNewEnqTempId();
            if (!string.IsNullOrEmpty(tempId))
            {
                int id = Convert.ToInt32(tempId);
                if (id != 0)
                {
                    var data = await _enquiryService.getEnquiryDetails(id).ConfigureAwait(false);
                    return View(data);
                }
            }
            return RedirectToAction(actionName: "Index", controllerName: "Enquiry");
        }
    }
}
