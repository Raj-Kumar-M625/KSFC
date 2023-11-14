using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Models;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Common.Logging.Client;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry
{
    [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class EnquiryController : Controller
    {
        private readonly SessionManager _sessionManager;
        private readonly IEnquirySubmissionService _enquirySubmissionService;
        private readonly IOtpService _otpService;
        private readonly IRegisterService _registerService;
        private readonly IPanService _panService;
        private const string viewPath = "../../Areas/Customer/Views/Enquiry/UnitDetails/";
        private readonly ILogger _logger;
        public EnquiryController(IEnquirySubmissionService enquirySubmissionService, SessionManager sessionManager,
            IDataProtectionProvider provider, IOtpService otpService, IRegisterService registerService, IPanService panService, ILogger logger)
        {
            _enquirySubmissionService = enquirySubmissionService;
            _sessionManager = sessionManager;
            _otpService = otpService;
            _registerService = registerService;
            _panService = panService;
            _logger = logger;
        }

        /// <summary>
        /// To get all enquiries
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.Information("Started - Index method");
                _sessionManager.SetCustLoginTime(DateTime.Now.ToString("HH:mm"));//To be added from Claims From API
                _sessionManager.SetCustLoginDateTime(DateTime.Now.ToString());
                List<EnquirySummary> enquirySummaryList = await _enquirySubmissionService.GetAllEnquiries();
                _logger.Information("Completed - Index method");
                return View(enquirySummaryList);
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading Index  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }



        //New Enquiry -->redirect to new enquiry form. EnquiryController.cs
        public async Task<IActionResult> New()
        {
            try
            {
                _logger.Information("Started - New method ");
                EnquiryDTO enquiryDTO = new();
                enquiryDTO.DDLDTO = await _enquirySubmissionService.GetAllEnquiryDropDownList();
                enquiryDTO.DDLDTO.ListMeansOfFinanceType = new List<SelectListItem>();
                enquiryDTO.UnitDetails = new UnitDetailsDTO();
                //Set all entities to empty string
                _sessionManager.SetAllEntitiesToEmptyString();
                _sessionManager.SetNewEnqTempId("0");
                _sessionManager.SetOperationType("New");
                List<SelectListItem> listAddressTypes = await _enquirySubmissionService.getAllAddressTypesFromDB();
                _sessionManager.SetAddressTypesFromDB(listAddressTypes); //Saving AddressTypes to Session to use on Address Details create view and edit for New Enquiry only

                enquiryDTO.UnitDetails.ListAddressDetail = new List<AddressDetailsDTO>();
                enquiryDTO.UnitDetails.ListRegDetails = new List<RegistrationNoDetailsDTO>();

            //Populating Dropdown List of BasicDetails Presonal Details page
            ViewBag.ListBranch = enquiryDTO.DDLDTO.ListBranch;
            ViewBag.ListLoanPurpose = enquiryDTO.DDLDTO.ListLoanPurpose;
            ViewBag.ListFirmSize = enquiryDTO.DDLDTO.ListFirmSize;
            ViewBag.ListProduct = enquiryDTO.DDLDTO.ListProduct;
            ViewBag.ListDistrict = enquiryDTO.DDLDTO.ListDistrict;
            ViewBag.ListPremises = enquiryDTO.DDLDTO.ListPremises;
            ViewBag.ListIndustryType = enquiryDTO.DDLDTO.ListIndustryType;
            ViewBag.ListPromotorClass = enquiryDTO.DDLDTO.ListPromotorClass;
            var allConstitutions = await _registerService.GetAllConstitutionTypes();
            _sessionManager.SetAllConstitutionTypes(allConstitutions);
            ViewBag.ConstitutionTypes = allConstitutions;
            if (_sessionManager.GetLoginCustUserName() != null)
            {
                var constitution = _panService.GetConstitutionByPanNumber();
                enquiryDTO.UnitDetails.BasicDetails = new BasicDetailsDto();
                enquiryDTO.UnitDetails.BasicDetails.ConstCd = constitution.CnstCd;
            }


                ViewBag.ListTaluka = new List<SelectListItem>();
                ViewBag.ListHobli = new List<SelectListItem>();
                ViewBag.ListVillage = new List<SelectListItem>();
                ViewBag.ListProjectFinanceType = new List<SelectListItem>();

                // Populating Dropdown List of BasicDetails Bank Details page
                _sessionManager.SetDDListDomicileStatus(enquiryDTO.DDLDTO.ListDomicileStatus);
                _sessionManager.SetDDLBankFacilityType(enquiryDTO.DDLDTO.ListFacility);

                //Adding dropdownlist data to Session to be used by Promoter and Guar page ddl
                _sessionManager.SetDDListPromoterDesignation(enquiryDTO.DDLDTO.ListPromDesgnType);

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

                _sessionManager.SetDDListPromAndGuarAssetCategory(enquiryDTO.DDLDTO.ListAssetCategory);
                _sessionManager.SetDDListPromAndGuarAssetType(enquiryDTO.DDLDTO.ListAssetType);

                _sessionManager.SetDDListModeOfAcquire(enquiryDTO.DDLDTO.ListAcquireMode);

                enquiryDTO.AssociateConcernDetails = new AssociateSisterConcernDetailsDTO
                {
                    ListAssociates = new List<SisterConcernDetailsDTO>(),
                    ListFYDetails = new List<SisterConcernFinancialDetailsDTO>()
                };

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


                _logger.Information("Completed - New method ");
                return View(enquiryDTO);

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading New  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
        public async Task<IActionResult> getCascadeDDL()
        {
            try
            {
                _logger.Information("Started - getCascadeDDL method");
                var details = _sessionManager.GetUDPersonalDetails();
                if (details != null)
                {
                    var enquiryDTODetails = await _enquirySubmissionService.getEnquiryDetails(Convert.ToInt32(details.EnqtempId));
                    _logger.Information("Completed - getCascadeDDL method ");
                    return Json(new { isValid = true, basicDetails = enquiryDTODetails.BasicDetails });
                }
                EnquiryDTO enquiryDTO = new();
                enquiryDTO.DDLDTO = await _enquirySubmissionService.GetAllEnquiryDropDownList();
                _logger.Information("Completed - getCascadeDDL method ");
                return Json(new { isValid = false, basicDetails = enquiryDTO.DDLDTO.ListDistrict });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getCascadeDDL  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        public async Task<List<SelectListItem>> getDistricForCascadeDDl()
        {
            try
            {
                _logger.Information("Started - getDistricForCascadeDDl method ");
                EnquiryDTO enquiryDTO = new();
                enquiryDTO.DDLDTO = await _enquirySubmissionService.GetAllEnquiryDropDownList();
                _logger.Information("Completed - getDistricForCascadeDDl method");
                return enquiryDTO.DDLDTO.ListDistrict;

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading getDistricForCascadeDDl  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        //Edit Enquiry (EnqId) -->redirect to enquiry form. Bind values from API. Editable (Few fields are not editable) 
        [HttpPost]
        public async Task<IActionResult> Edit(EnquirySummary model)
        {
           
            try
            {
                _logger.Information(string.Format("Started - Edit method For EnquiryId:{0} EnqStatus:{1} PromotorPan:{2} EnqInitiateDate:{3} EnqSubmitDate:{4}",
                    model.EnquiryId, model.EnqStatus, model.PromotorPan, model.EnqInitiateDate, model.EnqSubmitDate));
                if (model.EnquiryId != null && model.EnquiryId > 0)
                {
                    _sessionManager.SetAllEntitiesToEmptyString();
                    _sessionManager.SetNewEnqTempId(model.EnquiryId.Value.ToString());
                    _sessionManager.SetOperationType("Edit");
                    EnquiryDTO enquiryDTO = new();
                    #region UnitTab
                    ViewBag.UnitTab = "bg-warning";
                    List<SelectListItem> listAddressTypes = await _enquirySubmissionService.getAllAddressTypesFromDB();
                    _sessionManager.SetAddressTypesFromDB(listAddressTypes);
                    enquiryDTO.DDLDTO = await _enquirySubmissionService.GetAllEnquiryDropDownList();
                    //Populating Dropdown List of BasicDetails Presonal Details page
                    ViewBag.ListBranch = enquiryDTO.DDLDTO.ListBranch;
                    ViewBag.ListLoanPurpose = enquiryDTO.DDLDTO.ListLoanPurpose;
                    ViewBag.ListFirmSize = enquiryDTO.DDLDTO.ListFirmSize;
                    ViewBag.ListProduct = enquiryDTO.DDLDTO.ListProduct;
                    ViewBag.ListDistrict = enquiryDTO.DDLDTO.ListDistrict;
                    ViewBag.ListPremises = enquiryDTO.DDLDTO.ListPremises;
                    ViewBag.ListIndustryType = enquiryDTO.DDLDTO.ListIndustryType;
                    ViewBag.ListPromotorClass = enquiryDTO.DDLDTO.ListPromotorClass;
                    var allConstitutions = await _registerService.GetAllConstitutionTypes();
                    _sessionManager.SetAllConstitutionTypes(allConstitutions);
                    ViewBag.ConstitutionTypes = allConstitutions;
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
                    enquiryDTO.DDLDTO.ListMeansOfFinanceType = new List<SelectListItem>();

                    _sessionManager.SetDDListProjectCostComponent(enquiryDTO.DDLDTO.ListProjectCost);
                    _sessionManager.SetDDListProjectMeansOfFinance(enquiryDTO.DDLDTO.ListMeansOfFinanceCategory);
                    _sessionManager.SetDDListProjectFinanceType(enquiryDTO.DDLDTO.ListMeansOfFinanceType);
                    _sessionManager.SetDDListTypeOfSecurity(enquiryDTO.DDLDTO.ListSecurityType);
                    _sessionManager.SetDDListSecurityDetailsType(enquiryDTO.DDLDTO.ListSecurityDet);
                    _sessionManager.SetDDListRelationType(enquiryDTO.DDLDTO.ListSecurityRelation);
                    var enq = await _enquirySubmissionService.getEnquiryDetails(model.EnquiryId.Value);



                    var ListTaluka = new List<SelectListItem>();
                    var ListHobli = new List<SelectListItem>();
                    var ListVillage = new List<SelectListItem>();
                    if (enq.BasicDetails != null)
                    {
                        var getcascadePrefillDDLList = await _enquirySubmissionService.getCascadeDDLForEditPrefill(enq.BasicDetails.DistrictCd, enq.BasicDetails.TalukaCd,
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
                        if (enq.AddressDetails != null && enq.AddressDetails.Any())
                        {
                            if (listAddressTypes.Count() == enq.AddressDetails.Count())
                            {
                                ViewBag.AllAddressTypeExist = true;
                            }
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

                    if (_sessionManager.GetLoginCustUserName() != null)
                    {
                        var constitution = _panService.GetConstitutionByPanNumber();
                        if (enquiryDTO.UnitDetails.BasicDetails != null)
                        {
                            enquiryDTO.UnitDetails.BasicDetails.ConstCd = constitution.CnstCd;
                        }
                        else
                        {
                            enquiryDTO.UnitDetails.BasicDetails = new BasicDetailsDto();
                            enquiryDTO.UnitDetails.BasicDetails.ConstCd = constitution.CnstCd;
                        }
                    }

                    ViewBag.ListTaluka = ListTaluka;
                    ViewBag.ListHobli = ListHobli;
                    ViewBag.ListVillage = ListVillage;
                    ViewBag.ListProjectFinanceType = new List<SelectListItem>();
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
                        PromoterNetWorthList = new List<PromoterNetWorthDetailsDTO>(),
                        PromotersAssetLiabilityDetails = new PromAssetLiabilityDetailsDTO { ListAssetDetails = new List<PromoterAssetsNetWorthDTO>(), ListLiabilityDetails = new List<PromoterLiabilityDetailsDTO>() }
                    };
                    enquiryDTO.GuarantorAllDetailsDTO = new GuarantorAllDetailsDTO
                    {
                        ListGuarantor = new List<GuarantorDetailsDTO>(),
                        GuarantorNetWorthList = new List<GuarantorNetWorthDetailsDTO>(),
                        GuarantorAssetLiabilityDetails = new GuarAssetLiabilityDetailsDTO { ListAssetDetails = new List<GuarantorAssetsNetWorthDTO>(), ListLiabilityDetails = new List<GuarantorLiabilityDetailsDTO>() }
                    };
                    if (enq.PromoterDetails != null && enq.PromoterDetails.Any())
                    {
                        ViewBag.ShareHoldingIsAutomatically = true;
                        enquiryDTO.PromoterAllDetailsDTO.ListPromoters = enq.PromoterDetails.ToList();
                        var empShare = enq.PromoterDetails.Sum(x => x.EnqPromShare);
                        if (empShare < 100)
                        {
                            ViewBag.ShareHoldingIsAutomatically = false;
                        }

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
                    if (enq.PromoterNetWorth != null && enq.PromoterNetWorth.Any())
                    {
                        foreach (var item in enq.PromoterNetWorth)
                        {
                            item.PromoterDetailsDTO = new PromoterDetailsDTO();
                            item.PromoterDetailsDTO.PromoterMaster = new PromoterMasterDTO();
                            item.PromoterDetailsDTO.PromoterMaster = enq.PromoterDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster;
                        }
                        enquiryDTO.PromoterAllDetailsDTO.PromoterNetWorthList = enq.PromoterNetWorth.ToList();
                        _sessionManager.SetPromoterNetWorthList(enq.PromoterNetWorth.ToList());
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
                            item.GuarantorDetailsDTO.PromoterMaster = enq.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster;
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
                            item.GuarantorDetailsDTO.PromoterMaster = enq.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster;
                        }
                        enquiryDTO.GuarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.ListLiabilityDetails = enq.GuarantorLiability.ToList();
                        _sessionManager.SetGuarantorLiabilityList(enq.GuarantorLiability.ToList());
                    }
                    if (enq.GuarantorNetWorth != null && enq.GuarantorNetWorth.Any())
                    {
                        foreach (var item in enq.GuarantorNetWorth)
                        {
                            item.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                            item.GuarantorDetailsDTO.PromoterMaster = new PromoterMasterDTO();
                            item.GuarantorDetailsDTO.PromoterMaster = enq.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster;
                        }
                        enquiryDTO.GuarantorAllDetailsDTO.GuarantorNetWorthList = enq.GuarantorNetWorth.ToList();
                        _sessionManager.SetGuarantorNetWorthList(enq.GuarantorNetWorth.ToList());
                    }

                    if ((enq.PromoterDetails != null && enq.PromoterDetails.Any()) && (enq.PromoterAssetsDetails != null && enq.PromoterAssetsDetails.Any()) && (enq.PromoterLiability != null && enq.PromoterLiability.Any())
                        && (enq.GuarantorDetails != null && enq.GuarantorDetails.Any()) && (enq.GuarantorAssetsDetails != null && enq.GuarantorAssetsDetails.Any()) && (enq.GuarantorLiability != null && enq.GuarantorLiability.Any()))
                    {
                        ViewBag.PromoterTab = "bg-success";
                    }
                    #endregion

                    #region Assosiate Concern
                    ViewBag.SisterConcern = "bg-warning";
                    ViewBag.SisterConcernNotApplicable = false;
                    if (enq.HasAssociateSisterConcern != null && enq.HasAssociateSisterConcern.Value)
                    {
                        enquiryDTO.HasAssociateSisterConcern = enq.HasAssociateSisterConcern;
                        ViewBag.SisterConcernNotApplicable = true;
                        ViewBag.SisterConcern = "bg-success";
                    }
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

                    }
                    if ((enquiryDTO.HasAssociateSisterConcern == null || !enquiryDTO.HasAssociateSisterConcern.Value) &&
                        (enq.SisterConcernDetails != null && enq.SisterConcernDetails.Any()) && (enq.SisterConcernFinancialDetails != null && enq.SisterConcernFinancialDetails.Any()))
                    {
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

                    if (enq.SecurityDetails != null && enq.SecurityDetails.Any() && enq.DocumentList != null && enq.DocumentList.Count() == 8)
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

                    var totalEquity = enquiryDTO.ProjectDetails.ListMeansOfFinance.Where(x => x.MfcatCdNavigation.PjmfDets.ToLower() == "equity").ToList();
                    var totalDedt = enquiryDTO.ProjectDetails.ListMeansOfFinance.Where(x => x.MfcatCdNavigation.PjmfDets.ToLower() == "debt").ToList();

                    ViewBag.TotalEquity = totalEquity.Sum(x => x.EnqPjmfValue);
                    ViewBag.TotalDebt = totalDedt.Sum(x => x.EnqPjmfValue);

                    if (enquiryDTO.Status == (int)EnqStatus.Submitted)
                    {
                        ViewBag.ReviewTab = "bg-success";
                    }
                    else
                    {
                        ViewBag.ReviewTab = "bg-warning";

                    }
                    enquiryDTO.SummaryNote = enq.SummaryNote;
                    enquiryDTO.EnquiryRefNo = enq.EnquiryRefNo;
                    #endregion
                    _logger.Information(string.Format("Completed - Edit method For EnquiryId:{0} EnqStatus:{1} PromotorPan:{2} EnqInitiateDate:{3} EnqSubmitDate:{4}",
                    model.EnquiryId, model.EnqStatus, model.PromotorPan, model.EnqInitiateDate, model.EnqSubmitDate));
                    return View(enquiryDTO);
                }
                _logger.Information(string.Format("Completed - Edit method For EnquiryId:{0} EnqStatus:{1} PromotorPan:{2} EnqInitiateDate:{3} EnqSubmitDate:{4}",
                    model.EnquiryId, model.EnqStatus, model.PromotorPan, model.EnqInitiateDate, model.EnqSubmitDate));
                return RedirectToAction("Index");

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading Edit page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        [HttpPost]
        public async Task<IActionResult> View(EnquirySummary model)
        {
            try
            {
                _logger.Information(string.Format("Started - View method For EnquiryId:{0} EnqStatus:{1} PromotorPan:{2} EnqInitiateDate:{3} EnqSubmitDate:{4}",
                    model.EnquiryId, model.EnqStatus, model.PromotorPan, model.EnqInitiateDate, model.EnqSubmitDate));
                if (model.EnquiryId != null && model.EnquiryId > 0)
                {
                    _sessionManager.SetAllEntitiesToEmptyString();
                    _sessionManager.SetNewEnqTempId(model.EnquiryId.Value.ToString());
                    _sessionManager.SetOperationType("View");
                    EnquiryDTO enquiryDTO = new();
                    #region UnitTab
                    ViewBag.UnitTab = "bg-warning";
                    List<SelectListItem> listAddressTypes = await _enquirySubmissionService.getAllAddressTypesFromDB();
                    _sessionManager.SetAddressTypesFromDB(listAddressTypes);
                    enquiryDTO.DDLDTO = await _enquirySubmissionService.GetAllEnquiryDropDownList();
                    //Populating Dropdown List of BasicDetails Presonal Details page
                    ViewBag.ListBranch = enquiryDTO.DDLDTO.ListBranch;
                    ViewBag.ListLoanPurpose = enquiryDTO.DDLDTO.ListLoanPurpose;
                    ViewBag.ListFirmSize = enquiryDTO.DDLDTO.ListFirmSize;
                    ViewBag.ListProduct = enquiryDTO.DDLDTO.ListProduct;
                    ViewBag.ListDistrict = enquiryDTO.DDLDTO.ListDistrict;
                    ViewBag.ListPremises = enquiryDTO.DDLDTO.ListPremises;
                    ViewBag.ListIndustryType = enquiryDTO.DDLDTO.ListIndustryType;
                    ViewBag.ListPromotorClass = enquiryDTO.DDLDTO.ListPromotorClass;
                    var allConstitutions = await _registerService.GetAllConstitutionTypes();
                    _sessionManager.SetAllConstitutionTypes(allConstitutions);
                    ViewBag.ConstitutionTypes = allConstitutions;
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
                    var enq = await _enquirySubmissionService.getEnquiryDetails(model.EnquiryId.Value);
                    var ListTaluka = new List<SelectListItem>();
                    var ListHobli = new List<SelectListItem>();
                    var ListVillage = new List<SelectListItem>();
                    if (enq.BasicDetails != null)
                    {
                        var getcascadePrefillDDLList = await _enquirySubmissionService.getCascadeDDLForEditPrefill(enq.BasicDetails.DistrictCd, enq.BasicDetails.TalukaCd,
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

                    if (_sessionManager.GetLoginCustUserName() != null)
                    {
                        var constitution = _panService.GetConstitutionByPanNumber();
                        if (enquiryDTO.UnitDetails.BasicDetails != null)
                        {
                            enquiryDTO.UnitDetails.BasicDetails.ConstCd = constitution.CnstCd;
                        }
                        else
                        {
                            enquiryDTO.UnitDetails.BasicDetails = new BasicDetailsDto();
                            enquiryDTO.UnitDetails.BasicDetails.ConstCd = constitution.CnstCd;
                        }
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
                    if (enq.PromoterNetWorth != null && enq.PromoterNetWorth.Any())
                    {
                        foreach (var item in enq.PromoterNetWorth)
                        {
                            item.PromoterDetailsDTO = new PromoterDetailsDTO();
                            item.PromoterDetailsDTO.PromoterMaster = new PromoterMasterDTO();
                            item.PromoterDetailsDTO.PromoterMaster = enq.PromoterDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster;
                        }
                        enquiryDTO.PromoterAllDetailsDTO.PromoterNetWorthList = enq.PromoterNetWorth.ToList();
                        _sessionManager.SetPromoterNetWorthList(enq.PromoterNetWorth.ToList());
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
                    if (enq.GuarantorNetWorth != null && enq.GuarantorNetWorth.Any())
                    {
                        foreach (var item in enq.GuarantorNetWorth)
                        {
                            item.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                            item.GuarantorDetailsDTO.PromoterMaster = new PromoterMasterDTO();
                            item.GuarantorDetailsDTO.PromoterMaster = enq.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster;
                        }
                        enquiryDTO.GuarantorAllDetailsDTO.GuarantorNetWorthList = enq.GuarantorNetWorth.ToList();
                        _sessionManager.SetGuarantorNetWorthList(enq.GuarantorNetWorth.ToList());
                    }

                    if (enq.PromoterDetails != null && enq.PromoterDetails.Any())
                    {
                        ViewBag.PromoterTab = "bg-success";
                    }
                    #endregion

                    #region Assosiate Concern
                    ViewBag.SisterConcern = "bg-warning";
                    ViewBag.SisterConcernNotApplicable = false;
                    if (enq.HasAssociateSisterConcern != null && enq.HasAssociateSisterConcern.Value)
                    {
                        enquiryDTO.HasAssociateSisterConcern = enq.HasAssociateSisterConcern;
                        ViewBag.SisterConcernNotApplicable = true;
                        ViewBag.SisterConcern = "bg-success";
                    }
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

                    }
                    if ((enquiryDTO.HasAssociateSisterConcern == null || !enquiryDTO.HasAssociateSisterConcern.Value) &&
                        (enq.SisterConcernDetails != null && enq.SisterConcernDetails.Any()) && (enq.SisterConcernFinancialDetails != null && enq.SisterConcernFinancialDetails.Any()))
                    {
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

                    if (enq.SecurityDetails != null && enq.SecurityDetails.Any() && enq.DocumentList != null && enq.DocumentList.Count() == 8)
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
                    var totalEquity = enquiryDTO.ProjectDetails.ListMeansOfFinance.Where(x => x.MfcatCdNavigation.PjmfDets.ToLower() == "equity").ToList();
                    var totalDedt = enquiryDTO.ProjectDetails.ListMeansOfFinance.Where(x => x.MfcatCdNavigation.PjmfDets.ToLower() == "debt").ToList();

                    ViewBag.TotalEquity = totalEquity.Sum(x => x.EnqPjmfValue);
                    ViewBag.TotalDebt = totalDedt.Sum(x => x.EnqPjmfValue);
                    #endregion
                    _logger.Information(string.Format("Completed - View method For EnquiryId:{0} EnqStatus:{1} PromotorPan:{2} EnqInitiateDate:{3} EnqSubmitDate:{4}",
                        model.EnquiryId, model.EnqStatus, model.PromotorPan, model.EnqInitiateDate, model.EnqSubmitDate));
                    return View(enquiryDTO);
                }
                _logger.Information(string.Format("Completed - View method For EnquiryId:{0} EnqStatus:{1} PromotorPan:{2} EnqInitiateDate:{3} EnqSubmitDate:{4}",
                        model.EnquiryId, model.EnqStatus, model.PromotorPan, model.EnqInitiateDate, model.EnqSubmitDate));
                return RedirectToAction("Index");

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading SaveOrEditPromAndGuarDetailsForEnquiry  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrEditUnitDetailsForEnquiry(BasicDetailsDto basicDetailsDTO, BankDetailsDTO bankDetailsDTO)
        {
            try
            {
                _logger.Information(string.Format("Started - SaveOrEditUnitDetailsForEnquiry method for PromoterPan:{1} EnqBdetId:{2} EnqtempId:{3} EnqApplName:{4} EnqAddress:{5} EnqPlace:{6} EnqPincode:{7} EnqEmail:{8} AddlLoan:{9} UnitName:{10} EnqRepayPeriod:{11} EnqLoanamt:{12} ConstCd:{13} IndCd:{14} ConstType:{15} PurpCd:{16} PurposeOfLoan:{17} SizeCd:{18} SizeOfFirm:{19} ProdCd:{20} ProdCode:{21} VilCd:{22} VillageName:{23} PremCd:{24} PremCode:{25} OffcCd:{26} OffcCode:{27} UniqueId:{28} TypeOfIndustry:{29} DistrictCd:{30} TalukaCd:{31} HobliCd:{32} District:{33} Taluk:{34} Hobli:{35} EnqBankId:{36} EnqtempId:{37} EnqAcctype:{38} EnqBankaccno:{39} EnqIfsc:{40} EnqAccName:{41} EnqBankname:{42} EnqBankbr:{43} UniqueId:{44} BankPinCode:{45}",
                    basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.ProdCode, basicDetailsDTO.VilCd, basicDetailsDTO.VillageName, basicDetailsDTO.PremCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Hobli, basicDetailsDTO.Taluk, bankDetailsDTO.EnqBankId, bankDetailsDTO.EnqtempId, bankDetailsDTO.EnqAcctype, bankDetailsDTO.EnqBankaccno, bankDetailsDTO.EnqIfsc, bankDetailsDTO.EnqAccName, bankDetailsDTO.EnqBankname, bankDetailsDTO.EnqBankbr, bankDetailsDTO.UniqueId, bankDetailsDTO.BankPinCode));
                var ListAddressDetail = _sessionManager.GetAddressList();
                var ListRegDetails = _sessionManager.GetRegistrationDetList();
                var AddressTypes = _sessionManager.GetAddressTypesFromDB();
                if (!ModelState.IsValid)
                {
                    _logger.Information(string.Format("Completed - SaveOrEditUnitDetailsForEnquiry method for PromoterPan:{1} EnqBdetId:{2} EnqtempId:{3} EnqApplName:{4} EnqAddress:{5} EnqPlace:{6} EnqPincode:{7} EnqEmail:{8} AddlLoan:{9} UnitName:{10} EnqRepayPeriod:{11} EnqLoanamt:{12} ConstCd:{13} IndCd:{14} ConstType:{15} PurpCd:{16} PurposeOfLoan:{17} SizeCd:{18} SizeOfFirm:{19} ProdCd:{20} ProdCode:{21} VilCd:{22} VillageName:{23} PremCd:{24} PremCode:{25} OffcCd:{26} OffcCode:{27} UniqueId:{28} TypeOfIndustry:{29} DistrictCd:{30} TalukaCd:{31} HobliCd:{32} District:{33} Taluk:{34} Hobli:{35} EnqBankId:{36} EnqtempId:{37} EnqAcctype:{38} EnqBankaccno:{39} EnqIfsc:{40} EnqAccName:{41} EnqBankname:{42} EnqBankbr:{43} UniqueId:{44} BankPinCode:{45}",
                    basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.ProdCode, basicDetailsDTO.VilCd, basicDetailsDTO.VillageName, basicDetailsDTO.PremCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Hobli, basicDetailsDTO.Taluk, bankDetailsDTO.EnqBankId, bankDetailsDTO.EnqtempId, bankDetailsDTO.EnqAcctype, bankDetailsDTO.EnqBankaccno, bankDetailsDTO.EnqIfsc, bankDetailsDTO.EnqAccName, bankDetailsDTO.EnqBankname, bankDetailsDTO.EnqBankbr, bankDetailsDTO.UniqueId, bankDetailsDTO.BankPinCode));
                    return Json(new { isValid = false, invalidAccordion = "BasicDetails", Message = "Please fill all sections before saving the details." });
                }
                else if (ListAddressDetail == null || ListAddressDetail.Count == 0 || ListAddressDetail.Count < AddressTypes.Count)
                {
                    _logger.Information(string.Format("Completed - SaveOrEditUnitDetailsForEnquiry method for PromoterPan:{1} EnqBdetId:{2} EnqtempId:{3} EnqApplName:{4} EnqAddress:{5} EnqPlace:{6} EnqPincode:{7} EnqEmail:{8} AddlLoan:{9} UnitName:{10} EnqRepayPeriod:{11} EnqLoanamt:{12} ConstCd:{13} IndCd:{14} ConstType:{15} PurpCd:{16} PurposeOfLoan:{17} SizeCd:{18} SizeOfFirm:{19} ProdCd:{20} ProdCode:{21} VilCd:{22} VillageName:{23} PremCd:{24} PremCode:{25} OffcCd:{26} OffcCode:{27} UniqueId:{28} TypeOfIndustry:{29} DistrictCd:{30} TalukaCd:{31} HobliCd:{32} District:{33} Taluk:{34} Hobli:{35} EnqBankId:{36} EnqtempId:{37} EnqAcctype:{38} EnqBankaccno:{39} EnqIfsc:{40} EnqAccName:{41} EnqBankname:{42} EnqBankbr:{43} UniqueId:{44} BankPinCode:{45}",
                    basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.ProdCode, basicDetailsDTO.VilCd, basicDetailsDTO.VillageName, basicDetailsDTO.PremCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Hobli, basicDetailsDTO.Taluk, bankDetailsDTO.EnqBankId, bankDetailsDTO.EnqtempId, bankDetailsDTO.EnqAcctype, bankDetailsDTO.EnqBankaccno, bankDetailsDTO.EnqIfsc, bankDetailsDTO.EnqAccName, bankDetailsDTO.EnqBankname, bankDetailsDTO.EnqBankbr, bankDetailsDTO.UniqueId, bankDetailsDTO.BankPinCode));
                    return new JsonResult(new { isValid = false, invalidAccordion = "Address", Message = "Please add all address types before submitting the form." });
                }
                else if (ListRegDetails == null || ListRegDetails.Count == 0)
                {
                    _logger.Information(string.Format("Completed - SaveOrEditUnitDetailsForEnquiry method for PromoterPan:{1} EnqBdetId:{2} EnqtempId:{3} EnqApplName:{4} EnqAddress:{5} EnqPlace:{6} EnqPincode:{7} EnqEmail:{8} AddlLoan:{9} UnitName:{10} EnqRepayPeriod:{11} EnqLoanamt:{12} ConstCd:{13} IndCd:{14} ConstType:{15} PurpCd:{16} PurposeOfLoan:{17} SizeCd:{18} SizeOfFirm:{19} ProdCd:{20} ProdCode:{21} VilCd:{22} VillageName:{23} PremCd:{24} PremCode:{25} OffcCd:{26} OffcCode:{27} UniqueId:{28} TypeOfIndustry:{29} DistrictCd:{30} TalukaCd:{31} HobliCd:{32} District:{33} Taluk:{34} Hobli:{35} EnqBankId:{36} EnqtempId:{37} EnqAcctype:{38} EnqBankaccno:{39} EnqIfsc:{40} EnqAccName:{41} EnqBankname:{42} EnqBankbr:{43} UniqueId:{44} BankPinCode:{45}",
                    basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.ProdCode, basicDetailsDTO.VilCd, basicDetailsDTO.VillageName, basicDetailsDTO.PremCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Hobli, basicDetailsDTO.Taluk, bankDetailsDTO.EnqBankId, bankDetailsDTO.EnqtempId, bankDetailsDTO.EnqAcctype, bankDetailsDTO.EnqBankaccno, bankDetailsDTO.EnqIfsc, bankDetailsDTO.EnqAccName, bankDetailsDTO.EnqBankname, bankDetailsDTO.EnqBankbr, bankDetailsDTO.UniqueId, bankDetailsDTO.BankPinCode));
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

                unitDetailsDTO.ListAddressDetail.ForEach(a => a.EnqAddresssId = null); //Making Id=null for saving new record in DB otherwise it will return View("~/Views/Shared/Error.cshtml"); exception id id is already present in DB
                unitDetailsDTO.ListRegDetails.ForEach(a => a.EnqRegnoId = null);//Making Id=null for saving new record in DB otherwise it will return View("~/Views/Shared/Error.cshtml"); exception id id is already present in DB


                //Calling Api to Save Data

                var task1 = await _enquirySubmissionService.SaveUnitDetailsBasicDetails(unitDetailsDTO.BasicDetails, true);
                if (task1 == false)
                {
                    _logger.Information(string.Format("Completed - SaveOrEditUnitDetailsForEnquiry method for PromoterPan:{1} EnqBdetId:{2} EnqtempId:{3} EnqApplName:{4} EnqAddress:{5} EnqPlace:{6} EnqPincode:{7} EnqEmail:{8} AddlLoan:{9} UnitName:{10} EnqRepayPeriod:{11} EnqLoanamt:{12} ConstCd:{13} IndCd:{14} ConstType:{15} PurpCd:{16} PurposeOfLoan:{17} SizeCd:{18} SizeOfFirm:{19} ProdCd:{20} ProdCode:{21} VilCd:{22} VillageName:{23} PremCd:{24} PremCode:{25} OffcCd:{26} OffcCode:{27} UniqueId:{28} TypeOfIndustry:{29} DistrictCd:{30} TalukaCd:{31} HobliCd:{32} District:{33} Taluk:{34} Hobli:{35} EnqBankId:{36} EnqtempId:{37} EnqAcctype:{38} EnqBankaccno:{39} EnqIfsc:{40} EnqAccName:{41} EnqBankname:{42} EnqBankbr:{43} UniqueId:{44} BankPinCode:{45}",
                    basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.ProdCode, basicDetailsDTO.VilCd, basicDetailsDTO.VillageName, basicDetailsDTO.PremCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Hobli, basicDetailsDTO.Taluk, bankDetailsDTO.EnqBankId, bankDetailsDTO.EnqtempId, bankDetailsDTO.EnqAcctype, bankDetailsDTO.EnqBankaccno, bankDetailsDTO.EnqIfsc, bankDetailsDTO.EnqAccName, bankDetailsDTO.EnqBankname, bankDetailsDTO.EnqBankbr, bankDetailsDTO.UniqueId, bankDetailsDTO.BankPinCode));
                    return new JsonResult(new { isValid = false, invalidAccordion = "BasicDetails", message = "Please check your basic Details." });
                }
                var task2 = await _enquirySubmissionService.SaveUnitDetailsAddressDetails(unitDetailsDTO.ListAddressDetail, true);
                if (task2 == false)
                {
                    _logger.Information(string.Format("Completed - SaveOrEditUnitDetailsForEnquiry method for PromoterPan:{1} EnqBdetId:{2} EnqtempId:{3} EnqApplName:{4} EnqAddress:{5} EnqPlace:{6} EnqPincode:{7} EnqEmail:{8} AddlLoan:{9} UnitName:{10} EnqRepayPeriod:{11} EnqLoanamt:{12} ConstCd:{13} IndCd:{14} ConstType:{15} PurpCd:{16} PurposeOfLoan:{17} SizeCd:{18} SizeOfFirm:{19} ProdCd:{20} ProdCode:{21} VilCd:{22} VillageName:{23} PremCd:{24} PremCode:{25} OffcCd:{26} OffcCode:{27} UniqueId:{28} TypeOfIndustry:{29} DistrictCd:{30} TalukaCd:{31} HobliCd:{32} District:{33} Taluk:{34} Hobli:{35} EnqBankId:{36} EnqtempId:{37} EnqAcctype:{38} EnqBankaccno:{39} EnqIfsc:{40} EnqAccName:{41} EnqBankname:{42} EnqBankbr:{43} UniqueId:{44} BankPinCode:{45}",
                    basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.ProdCode, basicDetailsDTO.VilCd, basicDetailsDTO.VillageName, basicDetailsDTO.PremCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Hobli, basicDetailsDTO.Taluk, bankDetailsDTO.EnqBankId, bankDetailsDTO.EnqtempId, bankDetailsDTO.EnqAcctype, bankDetailsDTO.EnqBankaccno, bankDetailsDTO.EnqIfsc, bankDetailsDTO.EnqAccName, bankDetailsDTO.EnqBankname, bankDetailsDTO.EnqBankbr, bankDetailsDTO.UniqueId, bankDetailsDTO.BankPinCode));
                    return new JsonResult(new { isValid = false, invalidAccordion = "Address", message = "Please check your Address Details List." });
                }
                var task3 = await _enquirySubmissionService.SaveUnitDetailsBankDetails(unitDetailsDTO.BankDetails, true);
                if (task3 == false)
                {
                    _logger.Information(string.Format("Completed - SaveOrEditUnitDetailsForEnquiry method for PromoterPan:{1} EnqBdetId:{2} EnqtempId:{3} EnqApplName:{4} EnqAddress:{5} EnqPlace:{6} EnqPincode:{7} EnqEmail:{8} AddlLoan:{9} UnitName:{10} EnqRepayPeriod:{11} EnqLoanamt:{12} ConstCd:{13} IndCd:{14} ConstType:{15} PurpCd:{16} PurposeOfLoan:{17} SizeCd:{18} SizeOfFirm:{19} ProdCd:{20} ProdCode:{21} VilCd:{22} VillageName:{23} PremCd:{24} PremCode:{25} OffcCd:{26} OffcCode:{27} UniqueId:{28} TypeOfIndustry:{29} DistrictCd:{30} TalukaCd:{31} HobliCd:{32} District:{33} Taluk:{34} Hobli:{35} EnqBankId:{36} EnqtempId:{37} EnqAcctype:{38} EnqBankaccno:{39} EnqIfsc:{40} EnqAccName:{41} EnqBankname:{42} EnqBankbr:{43} UniqueId:{44} BankPinCode:{45}",
                    basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.ProdCode, basicDetailsDTO.VilCd, basicDetailsDTO.VillageName, basicDetailsDTO.PremCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Hobli, basicDetailsDTO.Taluk, bankDetailsDTO.EnqBankId, bankDetailsDTO.EnqtempId, bankDetailsDTO.EnqAcctype, bankDetailsDTO.EnqBankaccno, bankDetailsDTO.EnqIfsc, bankDetailsDTO.EnqAccName, bankDetailsDTO.EnqBankname, bankDetailsDTO.EnqBankbr, bankDetailsDTO.UniqueId, bankDetailsDTO.BankPinCode));
                    return new JsonResult(new { isValid = false, invalidAccordion = "Bank", message = "Please check your Bank Details." });
                }
                var task4 = await _enquirySubmissionService.SaveUnitDetailsRegistrationDetails(unitDetailsDTO.ListRegDetails, true);
                if (task4 == false)
                {
                    _logger.Information(string.Format("Completed - SaveOrEditUnitDetailsForEnquiry method for PromoterPan:{1} EnqBdetId:{2} EnqtempId:{3} EnqApplName:{4} EnqAddress:{5} EnqPlace:{6} EnqPincode:{7} EnqEmail:{8} AddlLoan:{9} UnitName:{10} EnqRepayPeriod:{11} EnqLoanamt:{12} ConstCd:{13} IndCd:{14} ConstType:{15} PurpCd:{16} PurposeOfLoan:{17} SizeCd:{18} SizeOfFirm:{19} ProdCd:{20} ProdCode:{21} VilCd:{22} VillageName:{23} PremCd:{24} PremCode:{25} OffcCd:{26} OffcCode:{27} UniqueId:{28} TypeOfIndustry:{29} DistrictCd:{30} TalukaCd:{31} HobliCd:{32} District:{33} Taluk:{34} Hobli:{35} EnqBankId:{36} EnqtempId:{37} EnqAcctype:{38} EnqBankaccno:{39} EnqIfsc:{40} EnqAccName:{41} EnqBankname:{42} EnqBankbr:{43} UniqueId:{44} BankPinCode:{45}",
                    basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.ProdCode, basicDetailsDTO.VilCd, basicDetailsDTO.VillageName, basicDetailsDTO.PremCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Hobli, basicDetailsDTO.Taluk, bankDetailsDTO.EnqBankId, bankDetailsDTO.EnqtempId, bankDetailsDTO.EnqAcctype, bankDetailsDTO.EnqBankaccno, bankDetailsDTO.EnqIfsc, bankDetailsDTO.EnqAccName, bankDetailsDTO.EnqBankname, bankDetailsDTO.EnqBankbr, bankDetailsDTO.UniqueId, bankDetailsDTO.BankPinCode));
                    return new JsonResult(new { isValid = false, invalidAccordion = "Registration", message = "Please check your Registration Details List." });
                }
                _logger.Information(string.Format("Completed - SaveOrEditUnitDetailsForEnquiry method for PromoterPan:{1} EnqBdetId:{2} EnqtempId:{3} EnqApplName:{4} EnqAddress:{5} EnqPlace:{6} EnqPincode:{7} EnqEmail:{8} AddlLoan:{9} UnitName:{10} EnqRepayPeriod:{11} EnqLoanamt:{12} ConstCd:{13} IndCd:{14} ConstType:{15} PurpCd:{16} PurposeOfLoan:{17} SizeCd:{18} SizeOfFirm:{19} ProdCd:{20} ProdCode:{21} VilCd:{22} VillageName:{23} PremCd:{24} PremCode:{25} OffcCd:{26} OffcCode:{27} UniqueId:{28} TypeOfIndustry:{29} DistrictCd:{30} TalukaCd:{31} HobliCd:{32} District:{33} Taluk:{34} Hobli:{35} EnqBankId:{36} EnqtempId:{37} EnqAcctype:{38} EnqBankaccno:{39} EnqIfsc:{40} EnqAccName:{41} EnqBankname:{42} EnqBankbr:{43} UniqueId:{44} BankPinCode:{45}",
                    basicDetailsDTO.PromoterPan, basicDetailsDTO.EnqBdetId, basicDetailsDTO.EnqtempId, basicDetailsDTO.EnqApplName, basicDetailsDTO.EnqAddress, basicDetailsDTO.EnqPlace, basicDetailsDTO.EnqPincode, basicDetailsDTO.EnqEmail, basicDetailsDTO.AddlLoan, basicDetailsDTO.UnitName, basicDetailsDTO.EnqRepayPeriod, basicDetailsDTO.EnqLoanamt, basicDetailsDTO.ConstCd, basicDetailsDTO.IndCd, basicDetailsDTO.ConstType, basicDetailsDTO.PurpCd, basicDetailsDTO.PurposeOfLoan, basicDetailsDTO.SizeCd, basicDetailsDTO.SizeOfFirm, basicDetailsDTO.ProdCd, basicDetailsDTO.ProdCode, basicDetailsDTO.VilCd, basicDetailsDTO.VillageName, basicDetailsDTO.PremCd, basicDetailsDTO.OffcCd, basicDetailsDTO.OffcCode, basicDetailsDTO.UniqueId, basicDetailsDTO.TypeOfIndustry, basicDetailsDTO.DistrictCd, basicDetailsDTO.TalukaCd, basicDetailsDTO.HobliCd, basicDetailsDTO.District, basicDetailsDTO.Hobli, basicDetailsDTO.Taluk, bankDetailsDTO.EnqBankId, bankDetailsDTO.EnqtempId, bankDetailsDTO.EnqAcctype, bankDetailsDTO.EnqBankaccno, bankDetailsDTO.EnqIfsc, bankDetailsDTO.EnqAccName, bankDetailsDTO.EnqBankname, bankDetailsDTO.EnqBankbr, bankDetailsDTO.UniqueId, bankDetailsDTO.BankPinCode));
                return Json(new { isValid = true });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading SaveOrEditUnitDetailsForEnquiry  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        //public  CheckEnquirySessionForTabName(string TabName)
        //{

        //    switch (TabName)
        //    {
        //        case "UnitDetails":
        //            if (_sessionManager.GetAddressList() == null)
        //            {
        //                return new JsonResult(new { isValid = false, invalidAccordion = "Address", Message = "Please add all three address types before submitting the form." });
        //            }
        //            else if (_sessionManager.GetRegistrationDetList() == null)
        //            {
        //                return new JsonResult(new { isValid = false, invalidAccordion = "Registration", Message = "Please enter atleast one registration details before submitting the form." });
        //            }
        //            break;
        //        case "PromAndGuar":
        //            return new JsonResult(new { isValid = false, invalidAccordion = "Promoter", Message = "Please enter atleast one Promoter details before submitting the form." });
        //        case "Project":
        //            return new JsonResult(new { isValid = false, invalidAccordion = "Project", Message = "Please enter Project details before submitting the form." });

        //    }
        //    return Json(new { isValid = true });
        //}

        public void ClearEnquirySession()
        {
            _sessionManager.ClearSessionData();
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrEditPromAndGuarDetailsForEnquiry()
        {
            try
            {
                _logger.Information("Started - SaveOrEditPromAndGuarDetailsForEnquiry method ");
                //Async Call API to save prom details
                //Async Call API to save guar details
                //return result in such a way that UI should be able to identify the error and expand accordion and highlight the field
                var promotorDetail = _sessionManager.GetPromoterDetailsList();
                var PromoterAssetList = _sessionManager.GetPromoterAssetList();
                var PromoterLiabilityList = _sessionManager.GetPromoterLiabilityList();
                var GuarantorDetailsList = _sessionManager.GetGuarantorDetailsList();
                var GuarantorAssetList = _sessionManager.GetGuarantorAssetList();
                var GuarantorLiabilityList = _sessionManager.GetGuarantorLiabilityList();
                var ProterNetWorthList = _sessionManager.GetPromoterNetWorthList();
                var GuarantorNetWorthList = _sessionManager.GetGuarantorNetWorthList();

                if ((promotorDetail == null || promotorDetail.Count == 0) && (PromoterAssetList == null || PromoterAssetList.Count == 0)
                    && (PromoterLiabilityList == null || PromoterLiabilityList.Count == 0) && (GuarantorDetailsList == null || GuarantorDetailsList.Count == 0)
                    && (GuarantorAssetList == null || GuarantorAssetList.Count == 0) && (GuarantorLiabilityList == null || GuarantorLiabilityList.Count == 0))
                {
                    _logger.Information("Completed - SaveOrEditPromAndGuarDetailsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "PromoterBasicAll", Message = "Please fill all sections before saving the details." });
                }


                if (promotorDetail == null || promotorDetail.Count == 0)
                {
                    _logger.Information("Completed - SaveOrEditPromAndGuarDetailsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "PromoterBasic", Message = "Please enter promoter details before submitting the form." });
                }
                else if (PromoterAssetList == null || PromoterAssetList.Count == 0)
                {
                    _logger.Information("Completed - SaveOrEditPromAndGuarDetailsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "PromoterAsset", Message = "Please enter promoter asset details before submitting the form." });
                }
                else if (PromoterLiabilityList == null || PromoterLiabilityList.Count == 0)
                {
                    _logger.Information("Completed - SaveOrEditPromAndGuarDetailsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "PromoterLiability", Message = "Please enter promoter liability details before submitting the form." });
                }
                else if (GuarantorDetailsList == null || GuarantorDetailsList.Count == 0)
                {
                    _logger.Information("Completed - SaveOrEditPromAndGuarDetailsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "GuarantorDetails", Message = "Please enter guarantor details before submitting the form." });
                }
                else if (GuarantorAssetList == null || GuarantorAssetList.Count == 0)
                {
                    _logger.Information("Completed - SaveOrEditPromAndGuarDetailsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "GuarantorAsset", Message = "Please enter guarantor asset details before submitting the form." });
                }
                else if (GuarantorLiabilityList == null || GuarantorLiabilityList.Count == 0)
                {
                    _logger.Information("Completed - SaveOrEditPromAndGuarDetailsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "GuarantorLiability", Message = "Please enter guarantor liability details before submitting the form." });
                }

                List<PromoterDetailsDTO> promoterDetails = _sessionManager.GetPromoterDetailsList();
                List<PromoterAssetsNetWorthDTO> promoterAssetDetails = _sessionManager.GetPromoterAssetList();
                List<PromoterLiabilityDetailsDTO> promoterLiabilityDetails = _sessionManager.GetPromoterLiabilityList();
                var task1 = await _enquirySubmissionService.SaveorEditPromoterAssetsNetWorthDetails(promoterAssetDetails);
                var task2 = await _enquirySubmissionService.SaveorEditPromoterLiabilityDetails(promoterLiabilityDetails);
                var task3 = await _enquirySubmissionService.SaveorEditPromoterLiabilityNetWorth(ProterNetWorthList);

                List<GuarantorDetailsDTO> GuarantorDetailsDTO = _sessionManager.GetGuarantorDetailsList();
                List<GuarantorAssetsNetWorthDTO> GuarantorAssetsNetWorthDTO = _sessionManager.GetGuarantorAssetList();
                List<GuarantorLiabilityDetailsDTO> GuarantorLiabilityDetailsDTO = _sessionManager.GetGuarantorLiabilityList();
                var task4 = await _enquirySubmissionService.SaveorEditGuarantorAssetsNetWorthDetails(GuarantorAssetsNetWorthDTO);
                var task5 = await _enquirySubmissionService.SaveorEditGuarantorLiabilityDetails(GuarantorLiabilityDetailsDTO);
                var task6 = await _enquirySubmissionService.SaveorEditGuarantorLiabilityNetWorth(GuarantorNetWorthList);

                _logger.Information("Completed - SaveOrEditPromAndGuarDetailsForEnquiry method ");
                return Json(new { isValid = true });


            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading SaveOrEditPromAndGuarDetailsForEnquiry  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAssociateSisterconcernChecked()
        {
            try
            {
                _logger.Information("Started - UpdateAssociateSisterconcernChecked method ");
                if (await _enquirySubmissionService.UpdateAssociateSisterConcernDetail())
                {
                    _logger.Information("Completed - UpdateAssociateSisterconcernChecked method ");
                    return Json(new { isValid = true });
                }
                _logger.Information("Completed - UpdateAssociateSisterconcernChecked method ");
                return Json(new { isValid = false, Message = "Please sumbit at-least one other form." });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateAssociateSisterconcernChecked  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrEditAssociateDetailsForEnquiry()
        {
            try
            {
                _logger.Information("Started - SaveOrEditAssociateDetailsForEnquiry method ");
                //Async Call API to save Associate / Sister Concern Detail
                //Async Call API to save Previous years Financial details
                //return result in such a way that UI should be able to identify the error and expand accordion and highlight the field
                if (_sessionManager.GetAssociateDetailsDTOList() == null)
                {
                    _logger.Information("Completed - SaveOrEditAssociateDetailsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "Associate", Message = "Please enter Associate details before submitting the form." });
                }
                else if (_sessionManager.GetAssociatePrevFYDetailsList() == null)
                {
                    _logger.Information("Completed - SaveOrEditAssociateDetailsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "PrevFYDetails", Message = "Please enter Associate previous financial year details before submitting the form." });
                }

                AssociateSisterConcernDetailsDTO associateSisterConcernDetailsDTO = new()
                {
                    ListAssociates = _sessionManager.GetAssociateDetailsDTOList(),
                };

                //Calling Api to Save Data
                //  var task1 = await _enquirySubmissionService.SaveAssociateSisterDetails(associateSisterConcernDetailsDTO.ListAssociates);
                associateSisterConcernDetailsDTO.ListFYDetails = _sessionManager.GetAssociatePrevFYDetailsList();
                var task2 = await _enquirySubmissionService.SaveAssociateSisterFYDetails(associateSisterConcernDetailsDTO.ListFYDetails);
                _logger.Information("Completed - SaveOrEditAssociateDetailsForEnquiry method ");
                return Json(new { isValid = true });

            }
            catch (Exception ex)
            {
                _logger.Error("Error occured while loading SaveOrEditAssociateDetailsForEnquiry  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
        [HttpPost]
        public async Task<IActionResult> SaveProjectDetails(ProjectWorkingCapitalDeatailsDTO workingCapArrDetails)
        {
            try
            {
                _logger.Information(string.Format("Started - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                EnquiryDTO enquiryDTO = new();
                if (!ModelState.IsValid)
                {
                    _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                    return Json(new { isValid = false, invalidAccordion = "ProjectDetails", Message = "Please enter the Working Capital details before submitting the form." });
                }
                _sessionManager.SetProjectWorkingCapitalArrDetails(workingCapArrDetails);

                var workingCapital = _sessionManager.GetProjectWorkingCapitalArrDetails();
                var projectCost = _sessionManager.GetProjectCostList();
                var meansOfFinance = _sessionManager.GetProjectMeansOfFinanceList();
                var prevFYDetails = _sessionManager.GetProjectPrevFYDetailsList();

                if (workingCapital == null)
                {
                    _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                    return Json(new { isValid = false, invalidAccordion = "WorkingCapital", Message = "Please enter the Working Capital details before submitting the form." });
                }
                else if (projectCost == null)
                {
                    _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                    return Json(new { isValid = false, invalidAccordion = "projectCost", Message = "Please add Project Cost before submitting the form." });
                }
                else if (meansOfFinance == null)
                {
                    _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                    return Json(new { isValid = false, invalidAccordion = "meansOfFinance", Message = "Please enter Means of Finance details before submitting the form." });
                }
                else if (prevFYDetails == null)
                {
                    _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                    return Json(new { isValid = false, invalidAccordion = "prevFYDetails", Message = "Please enter Previous Financial Details details before submitting the form." });
                }

                var task1 = await _enquirySubmissionService.saveCapitalDetails(workingCapital);
                if (!task1)
                {
                    _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                    return new JsonResult(new { isValid = false, message = "Please check your working Capital details." });
                }
                var task2 = await _enquirySubmissionService.saveProjectCostDetails(projectCost);
                if (!task2)
                {
                    _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                    return new JsonResult(new { isValid = false, message = "Please check your Project cost details." });
                }
                var task3 = await _enquirySubmissionService.saveProjectMeansOfFinanceDetails(meansOfFinance);
                if (!task3)
                {
                    _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                    return new JsonResult(new { isValid = false, message = "Please check your Means of finance details." });
                }
                var task4 = await _enquirySubmissionService.saveProjectPrevYearFinDetailsDetails(prevFYDetails);
                if (!task4)
                {
                    _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                    return new JsonResult(new { isValid = false, message = "Please check your previous year financial details." });
                }
                _logger.Information(string.Format("Completed - SaveProjectDetails method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}",
                   workingCapArrDetails.EnqWcId, workingCapArrDetails.EnqtempId, workingCapArrDetails.EnqWcIfsc, workingCapArrDetails.EnqWcBank, workingCapArrDetails.EnqWcBranch, workingCapArrDetails.EnqWcAmt, workingCapArrDetails.UniqueId, workingCapArrDetails.Operation));
                return Json(new { isValid = true });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading SaveProjectDetails page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveSecurityAndDocsForEnquiry()
        {
            try
            {
                _logger.Information("Started - SaveSecurityAndDocsForEnquiry method ");
                //get all details from session for s&D
                var SecurityDetails = _sessionManager.GetSecurityDetailsList();
                if (SecurityDetails == null || SecurityDetails.Count() == 0)
                {
                    _logger.Information("Completed - SaveSecurityAndDocsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "SecurityDetails", Message = "Please enter the Security details before submitting the form." });
                }
                var documentDetails = _sessionManager.GetDocuments();
                if (documentDetails == null || documentDetails.Count() != 8)
                {
                    _logger.Information("Completed - SaveSecurityAndDocsForEnquiry method ");
                    return Json(new { isValid = false, invalidAccordion = "SecurityDetails", Message = "Please upload all documents before submitting the form." });
                }
                var result = await _enquirySubmissionService.SaveSecurityDetails(SecurityDetails).ConfigureAwait(false);
                if (result.Any())
                {
                    _logger.Information("Completed - SaveSecurityAndDocsForEnquiry method ");
                    return Json(new { isValid = true, Message = "Successful" });
                }
                _logger.Information("Completed - SaveSecurityAndDocsForEnquiry method ");
                return Json(new { isValid = false, Message = "Please enter the Security details before submitting the form." });

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading SaveSecurityAndDocsForEnquiry  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        [HttpGet]
        public async Task<IActionResult> SaveSecurityAndDocsForEnquiry(int enquiryId)
        {
            try
            {
                _logger.Information("Started - SaveSecurityAndDocsForEnquiry method for Id = " + enquiryId);
                var result = await _enquirySubmissionService.getEnquiryDetails(enquiryId);
                _logger.Information("Completed - SaveSecurityAndDocsForEnquiry method for Id = " + enquiryId);
                return Json(result);

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading SaveSecurityAndDocsForEnquiry  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEnquiry(int id)
        {
            try
            {
                _logger.Information("Started - DeleteEnquiry method for Id = " + id);
                string viewPath = "../../Areas/Customer/Views/Enquiry/Index";
                var response = await _enquirySubmissionService.DeleteEnquiry(id);
                List<EnquirySummary> enquirySummaryList = await _enquirySubmissionService.GetAllEnquiries();
                _logger.Information("Completed - DeleteEnquiry method for Id = " + id);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, viewPath, enquirySummaryList) });
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteEnquiry  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }


        [HttpGet]
        public async Task<IActionResult> GenerateOtpForReview()
        {
            try
            {
                _logger.Information("Started - GenerateOtpForReview Method");
                 string mobileNo = _sessionManager.GetCustMobile();
                _otpService.GetOtpAttempts();
                var response = await _otpService.Generate("ReviewE", mobileNo, null, null);
                _logger.Information("Completed - GenerateOtpForReview Method");
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! GenerateOtpForReview page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ResendOtpForReview()
        {
            try
            {
                _logger.Information("Started - ResendOtpForReview Method");
                string mobileNo = _sessionManager.GetCustMobile();

                var response = await _otpService.Resend("ReviewE", mobileNo, null, null);
                _logger.Information("Completed - ResendOtpForReview Method");
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ResendOtpForReview page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOtpForReview(string entOtp)
        {
            try
            {
                _logger.Information("Started - ValidateOtpForReview method ");
                string mobileNum = _sessionManager.GetCustMobile();
                var response = await _otpService.Validate("ReviewE", mobileNum, entOtp);
                _logger.Information("Completed - ValidateOtpForReview method ");
                return response;
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! ValidateOtpForReview page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        [HttpPost]
        public async Task<IActionResult> SubmitEnquiry(string sNote)
        {
            try
            {
                _logger.Information("Started - SubmitEnquiry method ");
                int enqId = Convert.ToInt32(_sessionManager.GetNewEnqTempId());
                if (enqId == 0)
                {
                    _logger.Information("Completed - SubmitEnquiry method ");
                    return new JsonResult(new { isValid = false, tab = "", invalidAccordion = "", Message = $"Please fill all tab to submit this enquiry" });
                }
                var enq = await _enquirySubmissionService.getEnquiryDetails(enqId);
                if (enq == null)
                {
                    _logger.Information("Completed - SubmitEnquiry method ");
                    return new JsonResult(new { isValid = false, tab = "", invalidAccordion = "", Message = $"Please fill all tab to submit this enquiry" });
                }
                else
                {
                    if (enq.BasicDetails == null)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Unit", invalidAccordion = "BasicDetails", Message = "Please enter the basic and bank details before submitting the form." });
                    }
                    else if (enq?.AddressDetails == null || enq?.AddressDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return new JsonResult(new { isValid = false, tab = "Unit", invalidAccordion = "Address", Message = "Please add all address types before submitting the form." });
                    }
                    else if (enq?.BankDetails == null)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Unit", invalidAccordion = "Bank", Message = "Please enter Bank details before submitting the form." });
                    }
                    else if (enq?.RegistrationDetails == null || enq?.RegistrationDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return new JsonResult(new { isValid = false, tab = "Unit", invalidAccordion = "Registration", Message = "Please enter atleast one registration details before submitting the form." });
                    }
                    if (enq?.PromoterDetails == null || enq?.PromoterDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Promoter", invalidAccordion = "PromoterBasic", Message = "Please enter promoter details before submitting the form." });
                    }
                    else if (enq?.PromoterAssetsDetails == null || enq?.PromoterAssetsDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Promoter", invalidAccordion = "PromoterAsset", Message = "Please enter promoter asset details before submitting the form." });
                    }
                    else if (enq?.PromoterLiability == null || enq?.PromoterLiability.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Promoter", invalidAccordion = "PromoterLiability", Message = "Please enter promoter liability details before submitting the form." });
                    }
                    else if (enq?.GuarantorDetails == null || enq?.GuarantorDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Promoter", invalidAccordion = "GuarantorDetails", Message = "Please enter guarantor details before submitting the form." });
                    }
                    else if (enq?.GuarantorAssetsDetails == null || enq?.GuarantorAssetsDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Promoter", invalidAccordion = "GuarantorAsset", Message = "Please enter guarantor asset details before submitting the form." });
                    }
                    else if (enq?.GuarantorLiability == null || enq?.GuarantorLiability.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Promoter", invalidAccordion = "GuarantorLiability", Message = "Please enter guarantor liability details before submitting the form." });
                    }
                    else if (enq?.WorkingCapitalDetails == null)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Project", invalidAccordion = "ProjectDetails", Message = "Please enter the basic details before submitting the form." });
                    }
                    else if (enq?.ProjectCostDetails == null || enq?.ProjectCostDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Project", invalidAccordion = "projectCost", Message = "Please add Project Cost before submitting the form." });
                    }
                    else if (enq?.ProjectMeansOfFinanceDetails == null || enq?.ProjectMeansOfFinanceDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Project", invalidAccordion = "meansOfFinance", Message = "Please enter Means of Finance details before submitting the form." });
                    }
                    else if (enq?.ProjectFinancialYearDetails == null || enq?.ProjectFinancialYearDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Project", invalidAccordion = "prevFYDetails", Message = "Please enter Previous Financial Details details before submitting the form." });
                    }
                    if (enq?.SecurityDetails == null || enq?.SecurityDetails.Count() == 0)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Security", invalidAccordion = "SecurityDetails", Message = "Please enter the Security details before submitting the form." });
                    }
                    if (enq?.DocumentList == null || enq?.DocumentList.Count() != 8)
                    {
                        _logger.Information("Completed - SubmitEnquiry method ");
                        return Json(new { isValid = false, tab = "Security", invalidAccordion = "SecurityDetails", Message = "Please upload all documents before submitting the form." });

                    }
                }
                var result = await _enquirySubmissionService.SubmitEnquiry(sNote);
                if (result > 0)
                {
                    _logger.Information("Completed - SubmitEnquiry method ");
                    return new JsonResult(new { isValid = true, tab = "", invalidAccordion = "", Message = $"Enquiry Ref. No. {result} <br/> Enquiry Ref.No.sent to E - Mail and Mobile No." });
                }
                _logger.Information("Completed - SubmitEnquiry method ");
                return new JsonResult(new { isValid = false, tab = "", invalidAccordion = "", Message = $"Some Error occured. Please try again" });
            }
            catch (Exception ex)
            {
                _logger.Error("Error occured! SubmitEnquiry page . Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                ViewBag.error = "Unknown error occurred! Please try again after sometime.";
                return View("~/Views/Shared/Error.cshtml");
            }

        }

        public async Task<IActionResult> ReviewEnquiry()
        {
            try
            {
                _logger.Information("Started - ReviewEnquiry method ");
                var tempId = _sessionManager.GetNewEnqTempId();
                if (!string.IsNullOrEmpty(tempId))
                {
                    int id = Convert.ToInt32(tempId);
                    if (id != 0)
                    {
                        var ddllist = await _enquirySubmissionService.GetAllEnquiryDropDownList();
                        List<SelectListItem> listAddressTypes = await _enquirySubmissionService.getAllAddressTypesFromDB();

                        var data = await _enquirySubmissionService.getEnquiryDetails(id).ConfigureAwait(false);
                        foreach (var item in data.AddressDetails)
                        {
                            item.AddressTypeMasterDTO = new AddressTypeMasterDTO
                            {
                                AddtypeCd = item.AddtypeCd,
                                AddtypeDets = listAddressTypes.Find(a => a.Value == item.AddtypeCd.ToString()).Text
                            };
                        }
                        foreach (var item in data.RegistrationDetails)
                        {
                            item.RegTypeText = ddllist.ListRegnType.FindAll(x => x.Value == item.RegrefCd.ToString()).FirstOrDefault().Text;
                        }
                        if (data.PromoterAssetsDetails != null && data.PromoterAssetsDetails.Any())
                        {
                            foreach (var item in data.PromoterAssetsDetails)
                            {
                                var AssetCategorymasterData = ddllist.ListAssetCategory.FirstOrDefault(x => x.Value == item.AssetcatCd.ToString());
                                item.AssetCategoryMasterDTO = new AssetCategoryMasterDTO();
                                item.AssetCategoryMasterDTO.AssetcatCd = Convert.ToInt32(AssetCategorymasterData.Value);
                                item.AssetCategoryMasterDTO.AssetcatDets = AssetCategorymasterData.Text;

                                var AssetTypemasterData = ddllist.ListAssetType.FirstOrDefault(x => x.Value == item.AssettypeCd.ToString());
                                item.AssetTypeMasterDTO = new AssetTypeMasterDTO();
                                item.AssetTypeMasterDTO.AssettypeCd = Convert.ToInt32(AssetTypemasterData.Value);
                                item.AssetTypeMasterDTO.AssettypeDets = AssetTypemasterData.Text;

                                item.PromoterMasterDTO = new PromoterMasterDTO();
                                item.PromoterMasterDTO.PromoterCode = item.PromoterCode.Value;
                                item.PromoterMasterDTO.PromoterName = data.PromoterDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                            }
                        }
                        if (data.PromoterLiability != null && data.PromoterLiability.Any())
                        {
                            foreach (var item in data.PromoterLiability)
                            {
                                item.PromoterMasterDTO = new PromoterMasterDTO();
                                item.PromoterMasterDTO.PromoterCode = item.PromoterCode;
                                item.PromoterMasterDTO.PromoterName = data.PromoterDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                            }
                        }
                        if (data.PromoterNetWorth != null && data.PromoterNetWorth.Any())
                        {
                            foreach (var item in data.PromoterNetWorth)
                            {
                                item.PromoterDetailsDTO = new PromoterDetailsDTO();
                                item.PromoterDetailsDTO.PromoterMaster = new PromoterMasterDTO();
                                item.PromoterDetailsDTO.PromoterMaster = data.PromoterDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster;
                            }
                        }
                        if (data.GuarantorDetails != null && data.GuarantorDetails.Any())
                        {
                            foreach (var item in data.GuarantorDetails)
                            {
                                var domMasterData = ddllist.ListDomicileStatus.
                                     FirstOrDefault(x => x.Value == item.DomCd.ToString());
                                item.DomicileMasterDTO = new DomicileMasterDTO();
                                item.DomicileMasterDTO.DomDets = domMasterData.Text;
                            }
                            //enquiryDTO.GuarantorAllDetailsDTO.ListGuarantor = enq.GuarantorDetails.ToList();
                            //_sessionManager.SetGuarantorDetailsList(enq.GuarantorDetails.ToList());
                        }
                        if (data.GuarantorAssetsDetails != null && data.GuarantorAssetsDetails.Any())
                        {
                            foreach (var item in data.GuarantorAssetsDetails)
                            {
                                var AssetCategorymasterData = ddllist.ListAssetCategory.
                                    FirstOrDefault(x => x.Value == item.AssetcatCd.ToString());
                                item.AssetCategoryMasterDTO = new AssetCategoryMasterDTO();
                                item.AssetCategoryMasterDTO.AssetcatCd = Convert.ToInt32(AssetCategorymasterData.Value);
                                item.AssetCategoryMasterDTO.AssetcatDets = AssetCategorymasterData.Text;

                                var AssetTypemasterData = ddllist.ListAssetType.
                                   FirstOrDefault(x => x.Value == item.AssettypeCd.ToString());
                                item.AssetTypeMasterDTO = new AssetTypeMasterDTO();
                                item.AssetTypeMasterDTO.AssettypeCd = Convert.ToInt32(AssetTypemasterData.Value);
                                item.AssetTypeMasterDTO.AssettypeDets = AssetTypemasterData.Text;

                                item.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                                item.GuarantorDetailsDTO.PromoterCode = item.PromoterCode.Value;
                                item.GuarantorDetailsDTO.GuarName = data.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                            }
                            //enquiryDTO.GuarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.ListAssetDetails = data.GuarantorAssetsDetails.ToList();
                            //_sessionManager.SetGuarantorAssetList(enq.GuarantorAssetsDetails.ToList());
                        }
                        if (data.GuarantorLiability != null && data.GuarantorLiability.Any())
                        {
                            foreach (var item in data.GuarantorLiability)
                            {
                                item.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                                item.GuarantorDetailsDTO.PromoterCode = item.PromoterCode;
                                item.GuarantorDetailsDTO.GuarName = data.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster.PromoterName;
                            }
                            //enquiryDTO.GuarantorAllDetailsDTO.GuarantorAssetLiabilityDetails.ListLiabilityDetails = data.GuarantorLiability.ToList();
                            //_sessionManager.SetGuarantorLiabilityList(enq.GuarantorLiability.ToList());
                        }
                        if (data.GuarantorNetWorth != null && data.GuarantorNetWorth.Any())
                        {
                            foreach (var item in data.GuarantorNetWorth)
                            {
                                item.GuarantorDetailsDTO = new GuarantorDetailsDTO();
                                item.GuarantorDetailsDTO.PromoterMaster = new PromoterMasterDTO();
                                item.GuarantorDetailsDTO.PromoterMaster = data.GuarantorDetails.FirstOrDefault(x => x.PromoterCode == item.PromoterCode).PromoterMaster;
                            }
                            //enquiryDTO.GuarantorAllDetailsDTO.GuarantorNetWorthList = enq.GuarantorNetWorth.ToList();
                            //_sessionManager.SetGuarantorNetWorthList(enq.GuarantorNetWorth.ToList());
                        }
                        ViewBag.SisterConcernNotApplicable = false;
                        if (data.HasAssociateSisterConcern != null && data.HasAssociateSisterConcern.Value)
                        {
                            ViewBag.SisterConcernNotApplicable = true;
                        }
                        _logger.Information("Completed - ReviewEnquiry method ");
                        return View(data);
                    }
                }
                _logger.Information("Completed - ReviewEnquiry method ");
                return RedirectToAction(actionName: "New", controllerName: "Enquiry");

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading ReviewEnquiry  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                return View("~/Views/Shared/Error.cshtml");;
            }
        }
    }

}
