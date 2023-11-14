
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Security;
using KAR.KSFC.UI.Services.Admin.IDM.CreationOfSecurityandAquisitionAssetService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfSecurityandAquisitionAssetService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class UnitDetailsController : Controller
    {

        private readonly IUnitDetailsService _unitDetailsService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly ICommonService _commonService;
        private readonly IIdmService _idmService;
        private readonly IDataProtector protector;
        private readonly IInspectionOfUnitService _inspectionOfUnitService;
        private readonly ICreationOfSecurityandAquisitionAssetService _creationOfSecurityandAquisitionAssetService;

        public UnitDetailsController(ICreationOfSecurityandAquisitionAssetService creationOfSecurityandAquisitionAssetService,IInspectionOfUnitService inspectionOfUnitService,ILogger logger, SessionManager sessionManager, IUnitDetailsService unitDetailsService, 
            ICommonService commonService, IIdmService idmService, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _unitDetailsService = unitDetailsService;
            _commonService = commonService;
            _idmService = idmService;
            _creationOfSecurityandAquisitionAssetService = creationOfSecurityandAquisitionAssetService;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.AdminRouteValue);
            _inspectionOfUnitService = inspectionOfUnitService;
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
                     e.EncryptedInUnit = protector.Protect(e.InUnit.ToString());
                     return e;
                 });
            return View(loans);
        }
        public async Task<IActionResult> ViewAccount(string AccountNumber, string LoanSub, string UnitName, string OffcCd, string InUnit, string MainModule ,long InspectionId)
        {
            long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
            byte offCd = Convert.ToByte(protector.Unprotect(OffcCd));
            int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
            string unitname = protector.Unprotect(UnitName);
            string inUnit = protector.Unprotect(InUnit);
            try
            {
                _logger.Information(CommonLogHelpers.UnitDetailsviewAccount);
                IdmDDLListDTO idmDTO = await _commonService.GetAllIdmDropDownList();

                _sessionManager.SetPromoterNamesList(idmDTO.AllPromoterNames);
                _sessionManager.SetPromoterStateList(idmDTO.AllPromoterState);
                _sessionManager.SetPromoterDistrictList(idmDTO.AllPromoterDistricts);
                _sessionManager.SetDDListPromoterDesignation(idmDTO.AllPositionDesignation);
                _sessionManager.SetDDListDomicileStatus(idmDTO.AllDomicileStatus);
                _sessionManager.SetDDListPromoterClass(idmDTO.AllPromotorClass);
                _sessionManager.SetDDListPromoterSubClass(idmDTO.AllPromotorSubClass);
                _sessionManager.SetDDListPromoterQualification(idmDTO.AllPromotorQual);
                _sessionManager.SetDDLDistrictList(idmDTO.AllDistrictDetails);
                _sessionManager.SetDDLTalukList(idmDTO.AllTalukDetails);
                _sessionManager.SetDDLHobliList(idmDTO.AllHobliDetails);
                _sessionManager.SetDDLVillageList(idmDTO.AllVillageDetails);
                _sessionManager.SetBankIFSCCodeDDL(idmDTO.AllIfscCode);
                _sessionManager.SetDDListAccountType(idmDTO.AllAccountType);
                _sessionManager.SetDDListLandType(idmDTO.AllLandType);
                _sessionManager.SetDDLPincodeList(idmDTO.AllPincodeDetails);
                _sessionManager.SetAllAllocationCodeDDL(idmDTO.GetAllAllocationCode);
                _sessionManager.SetFinanceCategoryList(idmDTO.AllFinanceCategory); //US#05 
                _sessionManager.SetDDListProjectCostComponent(idmDTO.AllProjectCostComponents);


                IdmDDLListDTO idmDTo = await _commonService.GetAllIdmDropDownList();
                foreach (var i in idmDTo.GetAllAllocationCode)
                {
                    i.Text = i.Value.ToString() + " - " + i.Text;
                }
                _sessionManager.SetAllAllocationCodeDDL(idmDTo.GetAllAllocationCode);


                _logger.Information(CommonLogHelpers.GetAllUnitDetails);
                var NameofUnitDetails = await _unitDetailsService.GetUnitDetails(accountNumber);

                _logger.Information(CommonLogHelpers.GetAllDistrictNames);
                var allDistrictNames = await _idmService.GetAllDistrictNames();

                _logger.Information(CommonLogHelpers.GetAllTalukNames);
                var allTalukDetails = await _unitDetailsService.GetAllTalukDetails();
                var allTalukNames = await _idmService.GetAllTalukNames();

                _logger.Information(CommonLogHelpers.GetAllHobliNames);
                var allHobliDetails = await _unitDetailsService.GetAllHobliDetails();
                var allHobliNames = await _idmService.GetAllHobliNames();

                _logger.Information(CommonLogHelpers.GetAllVillageNames);
                var allVillageDetails = await _unitDetailsService.GetAllVillageDetails();
                var allVillageNames = await _idmService.GetAllVillageNames();

                _logger.Information(CommonLogHelpers.GetAllAddressDetails);
                var allAddressDetails = await _unitDetailsService.GetAllAddressDetails(accountNumber);

                _logger.Information(CommonLogHelpers.GetAllMasterPinCodeDetails);
                var allMasterPincodeDetails = await _unitDetailsService.GetAllMasterPinCodeDetails();

                _logger.Information(CommonLogHelpers.GetAllPinCodeDistrictDetails);
                var allPincodeDistrictDetails = await _unitDetailsService.GetAllPinCodeDistrictDetails();

                _logger.Information(CommonLogHelpers.GetAllAssetTypeDetails);
                var allAssetTypeDetails = await _unitDetailsService.GetAllAssetTypeDetails();
                

                _logger.Information(CommonLogHelpers.GetAllAssetCategaryDetails);
                var allAssetCategaryDetails = await _unitDetailsService.GetAllAssetCategaryDetails();

              

                foreach (var i in allAddressDetails)
                {
                    var District = allDistrictNames.Where(x => x.Value == i.UtDistCd);
                    i.DistrictName = District.First().Text;

                    var Taluk = allTalukNames.Where(x => x.Value == i.UtTlqCd);
                    i.TalukName = Taluk.First().Text;

                    var Hobli = allHobliNames.Where(x => x.Value == i.UtHobCd);
                    i.HobliName = Hobli.First().Text;

                    var Village = allVillageNames.Where(x => x.Value == i.UtVilCd);
                    i.VillageName = Village.First().Text;

                }
                _sessionManager.SetAllAddressDetailsList(allAddressDetails);
                _sessionManager.SetAllMasterPincodeDetails(allMasterPincodeDetails);
                _sessionManager.SetAllPincodeDistrictDetails(allPincodeDistrictDetails);
                _sessionManager.SetAllTalukDetails(allTalukDetails);
                _sessionManager.SetAllHobliDetails(allHobliDetails);
                _sessionManager.SetAllVillageDetails(allVillageDetails);

                _sessionManager.SetAllAssetTypeDetails(allAssetTypeDetails);
                _sessionManager.SetAllAssetCategaryDetails(allAssetCategaryDetails);


                _logger.Information(CommonLogHelpers.GetAllMasterPromoterProfileDetails);
                var GetAllMasterPromoterProfile = await _unitDetailsService.GetAllMasterPromoterProfileDetails();
                _sessionManager.SetMasterPromoterProfileList(GetAllMasterPromoterProfile);

                _logger.Information(CommonLogHelpers.GetAllPromoterProfileDetails);
                var ChangePromoterProfile = await _unitDetailsService.GetAllPromoterProfileDetails(accountNumber);
                _sessionManager.SetPromoterProfileList(ChangePromoterProfile);

                _logger.Information(CommonLogHelpers.GetPositionDesignationAsync);
                var AllPositionDesignation = await _idmService.GetPositionDesignationAsync();

                _logger.Information(CommonLogHelpers.GetAllPromotorClass);
                var AllPromotorClass = await _idmService.GetAllPromotorClass();

                _logger.Information(CommonLogHelpers.GetDomicileStatusAsync);
                var AllDomicileStatus = await _idmService.GetDomicileStatusAsync();


                _logger.Information(CommonLogHelpers.GetAllPromotorNames);
                var allPromoterNames = await _idmService.GetAllPromotorNames();


                foreach (var i in ChangePromoterProfile)
                {
                    var PromClass = AllPromotorClass.Where(x => x.Value == i.PclasCd);
                    i.PclassDet = PromClass.First().Text;
                    var PromDom = AllDomicileStatus.Where(x => x.Value == i.DomCd);
                    i.PdomDet = PromDom.First().Text;
                    var PromDesig = AllPositionDesignation.Where(x => x.Value == i.PdesigCd);
                    i.PdesigDet = PromDesig.First().Text;
                   
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                }
                _sessionManager.SetPromoterProfileList(ChangePromoterProfile);

                _logger.Information(CommonLogHelpers.GetAllPromoAddressDetails);
                var allPromoAddressDetails = await _unitDetailsService.GetAllPromoAddressDetails(accountNumber);
                foreach (var i in allPromoAddressDetails)
                {
                    var Promoname = allPromoterNames.Where(x => x.Value == i.PromoterCode);
                    i.PromoterName = Promoname.First().Text;
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                }
                _sessionManager.SetPromoterAddressList(allPromoAddressDetails);

                _logger.Information(CommonLogHelpers.GetAllLandAssetDetails);
                var allLandAssetDetails = await _unitDetailsService.GetAllProjLandDetailsList(accountNumber);
                foreach(var i in allLandAssetDetails)
                {
                   if(i.UniqueId == null)
                   {
                        i.UniqueId = Guid.NewGuid().ToString();
                   }
                }
                _sessionManager.SetAllLandAssetDetails(allLandAssetDetails);

                _logger.Information(CommonLogHelpers.GetallAssetTypeMaster);
                var AllAssetTypeMaster = await _unitDetailsService.GetallAssetTypeMaster();
                _sessionManager.SetAssetTypeMaster(idmDTO.AllAssetTypeMaster);

                _logger.Information(CommonLogHelpers.GetallAssetCategoryMaster);
                var AllAssetCategaryMaster = await _unitDetailsService.GetallAssetCategoryMaster(); //gowtham
                _sessionManager.SetAssetCategaryMaster(idmDTO.AllAssetCategoryMaster);

                _logger.Information(CommonLogHelpers.GetAllPromoterAssetDetails);
                var allAssetDetailsList = await _unitDetailsService.GetAllPromoterAssetDetails(accountNumber);
                foreach (var i in allAssetDetailsList)
                {
                    var Promoname = allPromoterNames.Where(x => x.Value == i.PromoterCode);
                    i.PromoterName = Promoname.First().Text;
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var AssetType = AllAssetTypeMaster.Where(x => x.Value == i.AssetTypeCD);
                    i.AssettypeDets = AssetType.First().Text;
                    var Assetcat = AllAssetCategaryMaster.Where(x => x.Value == i.AssetCatCD);
                    i.AssetcatDets = Assetcat.First().Text;
                    i.IdmAssetValue *= 100000;
                }
                _sessionManager.SetAssetDetailsList(allAssetDetailsList);

                _logger.Information(CommonLogHelpers.GetAllIfscbankDetails);
                var allIfscBankCode = await _unitDetailsService.GetAllIfscbankDetails();
                _sessionManager.SetIfscBankDetailsList(allIfscBankCode);

                _logger.Information(CommonLogHelpers.GetAccountTypeAsync);
                var AllAccountType = await _idmService.GetAccountTypeAsync();

                _logger.Information(CommonLogHelpers.GetAllPromoterBankInfo);
                var allPromoterBankList = await _unitDetailsService.GetAllPromoterBankInfo(accountNumber);
                foreach (var i in allPromoterBankList)
                {
                    var res = allIfscBankCode.Where(x => x.IFSCRowID == i.PrmIfscId);
                    i.PrmIFSCValue = res.Select(x => x.IFSCCode).First();
                    var PromAcc = AllAccountType.Where(x => x.Value == i.PrmAcType);
                    i.PrmAcTypeDet = PromAcc.First().Text;
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var Promoname = allPromoterNames.Where(x => x.Value == i.PromoterCode);
                    i.PromoterName = Promoname.FirstOrDefault().Text;
                }
                _sessionManager.SetPromoterBankList(allPromoterBankList);


                _logger.Information(CommonLogHelpers.GetAllProductList);
                var allProducdetailsMaster = await _unitDetailsService.GetAllProductList(); //gowtham
                _sessionManager.SetProductList(allProducdetailsMaster);
                _sessionManager.SetProducdetailsMaster(idmDTO.allProducdetailsMaster);  //gowtham

                _logger.Information(CommonLogHelpers.GetallIndustrydetailsMaster);
                var allIndustrydetailsMaster = await _unitDetailsService.GetallIndustrydetailsMaster(); //gowtham
                _sessionManager.SetIndustrydetailsMaster(idmDTO.allindustrydetailsMaster); //gowtham

                _logger.Information(CommonLogHelpers.GetAllProductDetails);
                var allProductDetailsList = await _unitDetailsService.GetAllProductDetails(accountNumber);
                foreach (var i in allProductDetailsList)
                {
                    var IndustryName = allIndustrydetailsMaster.Where(x => x.Value == i.IndId);
                    i.IndDets = IndustryName.First().Text;
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var productTypeDDl = allProducdetailsMaster.Where(x => x.Id == i.ProdId);
                    i.ProdDets = productTypeDDl.First().ProdDets;
                }
                _sessionManager.SetProductDetailsList(allProductDetailsList);

                _logger.Information(CommonLogHelpers.GetAllChangebankDetails);
                var allChangeBankDetails = await _unitDetailsService.GetAllChangebankDetails(accountNumber);

                foreach (var i in allChangeBankDetails)  //MJ
                {
                    var allbankType = AllAccountType.Where(x => x.Value == i.UtAccType);
                    i.AccType = allbankType.First().Text;
                    i.UniqueId = Guid.NewGuid().ToString();
                }
                _sessionManager.SetChangeBankDetailsList(allChangeBankDetails);

                _logger.Information(CommonLogHelpers.GetAllPromoLiabilityInfo);
                var allPromoLiabilityInfo = await _unitDetailsService.GetAllPromoLiabilityInfo(accountNumber);

                foreach (var i in allPromoLiabilityInfo)
                {
                    var Promoname = allPromoterNames.Where(x => x.Value == i.PromoterCode);
                    i.PromoterName = Promoname.First().Text;
                    if (i.UniqueID == null)
                    {
                        i.UniqueID = Guid.NewGuid().ToString();
                    }
                    i.LiabVal *= 100000;
                }

                _sessionManager.SetPromoterLiabilityInfo(allPromoLiabilityInfo);
                _logger.Information(CommonLogHelpers.GetAllPromoterNetWorth);
                var allPromoNetWorth = await _unitDetailsService.GetAllPromoterNetWorth(accountNumber);

                foreach (var i in allPromoNetWorth)
                {
                    var Promoname = allPromoterNames.Where(x => x.Value == i.PromoterCode);
                    i.PromoterName = Promoname.First().Text;
                    if (i.UniqueID == null)
                    {
                        i.UniqueID = Guid.NewGuid().ToString();
                    }
                    i.IdmNetworth *= 100000;
                    i.IdmMov *= 100000;
                    i.IdmLiab *= 100000;
                }
                _sessionManager.SetPromoteNetWorth(allPromoNetWorth);


                _logger.Information(Constants.FileList);
                var ldFileList = await _commonService.FileList(MainModule);
                _sessionManager.SetIDMDocument(ldFileList);

                //US#05
                _logger.Information(CommonLogHelpers.GetAllMeansOfFinanceList);
                var meansOfFinanceDetails = await _unitDetailsService.GetAllMeansOfFinanceList(accountNumber);

                var financeCategory = _sessionManager.GetAllFinanceCategoryList();
                decimal meansOfFinance = 0;
                foreach (var i in meansOfFinanceDetails)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    meansOfFinance = meansOfFinance + (decimal)i.DcmfAmt;
                    var categoryCode = financeCategory.Where(x => x.Value == i.DcmfCd.ToString());
                    i.Category = categoryCode.First().Text;
                    var id = i.DcmfCd;
                    if (id != null)
                    {
                        var AllFinancetype = await _unitDetailsService.GetFinanceTypeAsync();

                        var AllFinanceCode = AllFinancetype.Where(x => x.Value == i.DcmfMfType.ToString());
                        i.FinanceType = AllFinanceCode.First().Text;
                    }

                }
                _sessionManager.SetMeansOfFinanceList(meansOfFinanceDetails);

                _logger.Information(CommonLogHelpers.GetAllProjectCostComponentsDetails);
                var allprojectCostComponentDetail = await _unitDetailsService.GetAllProjectCostComponentsDetails();
                List<SelectListItem> projectCostComponentDetail = allprojectCostComponentDetail.Select(x => new SelectListItem
                {
                    Value = x.Value.ToString(),
                    Text = x.Text
                }).ToList();

                var ProjectCostDetailsList = await _unitDetailsService.GetAllProjectCostDetailsList(accountNumber);
                decimal projectCost = 0;
                foreach (var i in ProjectCostDetailsList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var projectCostComponent = allprojectCostComponentDetail.Where(x => x.Value == i.DcpcstCode);
                    i.ProjectCost = projectCostComponent.First().Text;
                    i.DcpcAmount *= 100000;
                    projectCost += (decimal)i.DcpcAmount;
                }
                _sessionManager.SetProjectCostList(ProjectCostDetailsList);
                _sessionManager.SetProjectCostDetailsList(projectCostComponentDetail);

                _logger.Information(CommonLogHelpers.LoanAllocationviewAccount);


                _logger.Information(CommonLogHelpers.GetAllocationCodes);
                var allAllocationCodes = await _idmService.GetAllocationCodes();

                _logger.Information(CommonLogHelpers.GetAllLoanAllocationList);
                var allLoanAllocationList = await _unitDetailsService.GetAllLoanAllocationList(accountNumber);
                decimal allocation = 0;
                foreach (var i in allLoanAllocationList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var allcCode = allAllocationCodes.Where(x => x.Value == i.DcalcCd);
                    i.DcalcCode = i.DcalcCd + " - " + allcCode.First().Text;
                    if (i.DcalcAmtRevised == null)
                    {
                        allocation += (decimal)i.DcalcAmt;
                    }
                    else
                    {
                        allocation += (decimal)i.DcalcAmtRevised;
                    }
                }
                
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.OffCd = offCd;
                ViewBag.meansOfFinance = meansOfFinance;
                ViewBag.projectCost = projectCost;
                ViewBag.allocation = allocation;
                _sessionManager.SetAllLoanAllocationList(allLoanAllocationList);
                //_logger.Information(CommonLogHelpers.GetAllLetterOfCreditDetailsList);
                //var LetterOfCreditList = await _unitDetailsService.GetAllLetterOfCreditDetailsList(accountNumber);
                //foreach (var i in LetterOfCreditList)
                //{
                //    if (i.UniqueId == null)
                //    {
                //        i.UniqueId = Guid.NewGuid().ToString();
                //    }
                //}
                //_sessionManager.SetLetterOfCreditList(LetterOfCreditList);

                // Asset Details 
                var inspectiondetailslist = await _inspectionOfUnitService.GetAllInspectionDetailsList(accountNumber);
                var buildingAcquisition = await _creationOfSecurityandAquisitionAssetService.GetAllBuildingAcquisitionDetails(accountNumber , InspectionId);

                foreach (var i in inspectiondetailslist)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                        i.EncryptedLoanAcc = protector.Protect(i.LoanAcc.ToString());
                        i.EncryptedOffcCd = protector.Protect(i.OffcCd.ToString());
                        i.EncryptedLoanSub = protector.Protect(i.LoanSub.ToString());
                    }
                }



                IdmDDLListDTO idmDDLListDTO = await _commonService.GetAllIdmDropDownList();
                ViewBag.inspectiondetails = inspectiondetailslist;
                ViewBag.InspectionId = inspectiondetailslist.LastOrDefault().DinRowID;

                var latestInspections = inspectiondetailslist.OrderByDescending(insp => insp.CreatedDate).FirstOrDefault();
                var allBuildingInspectionList = await _inspectionOfUnitService.GetAllBuildingnspectionList(accountNumber, 0);
                //var allLandInspectionList = await _inspectionOfUnitService.GetAllLandInspectionList(accountNumber, 0);
                var allFurnitureInspectionList = await _inspectionOfUnitService.GetAllFurnitureInspectionList(accountNumber, 0);
                var allIndigenousMachineryInspectionList = await _inspectionOfUnitService.GetAllIndigenousMachineryInspectionList(accountNumber, 0);
                var machineryStatusLists = await _inspectionOfUnitService.GetAllMachineryStatusList();
                var procureLists = await _inspectionOfUnitService.GetAllProcureList();
                var allImportMachineryInspection = await _inspectionOfUnitService.GetAllImportMachineryList(accountNumber, 0);
                var currencyList = await _inspectionOfUnitService.GetAllCurrencyList();
                var allListLandType = await _idmService.GetAllLandType();
                foreach (var item in buildingAcquisition)
                {
                    foreach (var item1 in allBuildingInspectionList)
                    {
                        if (item1.UniqueId == item.UniqueId)
                        {
                            item1.IrbBldgConstStatus = item.IrbBldgConstStatus;
                            item1.IrbPercentage = item.IrbPercentage;
                            item1.IrbUnitCost = item.IrbUnitCost;
                            item1.RoofType = item.RoofType;
                            item1.Irbid = item.Irbid;
                            item1.IrbSecValue = item.IrbSecValue;
                        }
                    }
                }
                //foreach (var i in allLandInspectionList)
                //{
                //    if (i.UniqueId == null)
                //    {
                //        i.UniqueId = Guid.NewGuid().ToString();
                //    }
                //    var LandType = allListLandType.Where(x => x.Value == i.DcLndType);
                //    i.LandType = LandType.First().Text;
                //}

                _sessionManager.SetImportMachineryList(allImportMachineryInspection);
                _sessionManager.SetIndigenousMachineryInspectionList(allIndigenousMachineryInspectionList);
                _sessionManager.SetBuildingInspectionList(allBuildingInspectionList);
                _sessionManager.SetFurnitureInspectionList(allFurnitureInspectionList);
                //_sessionManager.SetLandInspectionList(allLandInspectionList);
                _sessionManager.SetDDListLandType(idmDDLListDTO.AllLandType);
                _sessionManager.SetInspectionDetialList(inspectiondetailslist);
                _sessionManager.SetRegisteredStateDDL(idmDDLListDTO.AllStateZone);
                _sessionManager.SetMachinaryStatusList(machineryStatusLists);
                _sessionManager.SetProcureList(procureLists);
                _sessionManager.SetCurrencyList(currencyList);

                if (latestInspections == null || allBuildingInspectionList.Any(building => building.DcBdgIno == latestInspections.DinNo))
                {
                    ViewBag.firstinspection = false;
                }
                else
                {
                    ViewBag.firstinspection = true;
                }

                ChangeOfUnitInfoDTO ChangeUnitDet = new();
                ChangeUnitDet.UnitDetails = NameofUnitDetails;
                ChangeUnitDet.PromoterLiability = allPromoLiabilityInfo.ToList();
                ChangeUnitDet.AddressList = allAddressDetails.ToList();
                ChangeUnitDet.PromoterAddress = allPromoAddressDetails.ToList();
                ChangeUnitDet.PromoterDetails = ChangePromoterProfile.ToList();
                ChangeUnitDet.UnitProducts = allProductDetailsList.ToList();
                ChangeUnitDet.ChangeBankDetails = allChangeBankDetails.ToList();
                ChangeUnitDet.PromoterAsset = allAssetDetailsList.ToList();
                ChangeUnitDet.PromoterNetWorth = allPromoNetWorth.ToList();
                ChangeUnitDet.PromoterBankDetails = allPromoterBankList.ToList();
                ChangeUnitDet.AllPositionDesignation = idmDTO.AllPositionDesignation;
                ChangeUnitDet.AllDomicileStatus = idmDTO.AllDomicileStatus;
                ChangeUnitDet.AllPromotorClass = idmDTO.AllPromotorClass;
                ChangeUnitDet.AllPromotorSubClass = idmDTO.AllPromotorSubClass;
                ChangeUnitDet.AllPromotorQual = idmDTO.AllPromotorQual;
                ChangeUnitDet.allProducdetailsMaster = idmDTO.allProducdetailsMaster;
                ChangeUnitDet.allindustrydetailsMaster = idmDTO.allindustrydetailsMaster;
                ChangeUnitDet.AllAssetTypeMaster = idmDTO.AllAssetTypeMaster;
                ChangeUnitDet.AllAssetCategoryMaster = idmDTO.AllAssetCategoryMaster;
                ChangeUnitDet.AllAccountType = idmDTO.AllAccountType;
                ChangeUnitDet.AllAccountType = idmDTO.AllAccountType;
                ChangeUnitDet.MeansOfFinanceDetails = meansOfFinanceDetails.ToList();
                ChangeUnitDet.ProjectCostDetail = ProjectCostDetailsList.ToList();
                //ChangeUnitDet.LetterOfCreditList = LetterOfCreditList.ToList();
                ChangeUnitDet.LoanAllocationDetails = allLoanAllocationList.ToList();
                //ChangeUnitDet.LandInspectionDetails = allLandInspectionList.ToList();
                ChangeUnitDet.BuildingInspectionDetails = allBuildingInspectionList.ToList();
                ChangeUnitDet.FurnitureInspectionDetails = allFurnitureInspectionList.ToList();
                ChangeUnitDet.IndigenousMachineryDetails = allIndigenousMachineryInspectionList.ToList();
                ChangeUnitDet.ImportedMachineryDetails = allImportMachineryInspection.ToList();
                ChangeUnitDet.ProjectLandDetails = allLandAssetDetails.ToList();

                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.OffcCd = offCd;
                ViewBag.InUnit = inUnit;

                return View(ChangeUnitDet);
            }
            catch (Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(IdmUnitDetailDTO idmUnitDetail)
        {
            if (idmUnitDetail.Name == null)
            {
                return Ok();
            }
            else
            {
                _logger.Information(CommonLogHelpers.StartedSaveNameOfUnitDetails);
                IdmUnitDetailDTO idmUnitDetails = new IdmUnitDetailDTO();

                var NameofUnitDetails = await _unitDetailsService.GetUnitDetails(idmUnitDetail.LoanAcc);
                if (NameofUnitDetails != null)
                {
                    idmUnitDetails = NameofUnitDetails;
                    idmUnitDetails.TblUnitMast.UtName = idmUnitDetail.Name;
                }
                var res = await _unitDetailsService.updateUnitName(idmUnitDetails);
                _logger.Information(CommonLogHelpers.CompletedSaveNameOfUnitDetails);
                ViewBag.NameOfUnit = res.TblUnitMast.UtName;
                return Ok(new { data = res, idmUnitDetails.TblUnitMast.UtName });
            }

        }

        //Author: Gagana Date:29/11/2022
        [HttpPost]
        public async Task<IActionResult> SaveAllUnitDetails()
        {
            try
            {
                // Address Details
                try
                {
                    _logger.Information(CommonLogHelpers.StartedSaveChangeLocationDetails);

                    if (_sessionManager.GetAllAddressDetailsList().Count != 0)
                    {
                         List<IdmUnitAddressDTO> AllAddressList = _sessionManager.GetAllAddressDetailsList();

                        foreach (var item in AllAddressList)
                        {
                            switch (item.Action)
                            {
                                case (int)Constant.Update:
                                    await _unitDetailsService.UpdateAddressDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedUpdateAddressDetails);
                                    break;
                                default:
                                    break;

                            }

                        }
                        _logger.Information(CommonLogHelpers.CompletedSaveChangeLocationDetails);

                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(Error.SaveChangeLocationError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                    ViewBag.error = (Error.ViewBagError);
                    return View(Error.ErrorPath);
                }

                // ChangeBank Details
                try
                {
                    _logger.Information(CommonLogHelpers.StartedSaveChangeBankDetails);
                    if (_sessionManager.GetChangeBankDetailsList().Count != 0)
                    {
                        var allChangeBankDetailsList = _sessionManager.GetChangeBankDetailsList();

                        foreach (var item in allChangeBankDetailsList)
                        {
                            switch (item.Action)
                            {
                                case (int)Constant.Delete:
                                    await _unitDetailsService.DeleteChangeBankDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedDeleteChangeBankDetails);
                                    break;
                                case (int)Constant.Update:
                                    await _unitDetailsService.UpdateChangeBankDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedUpdateChangeBankDetails);
                                    break;
                                case (int)Constant.Create:
                                    await _unitDetailsService.CreateChangeBankDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedCreateChangeBankDetails);
                                    break;
                                default:
                                    break;
                            }
                        }
                        _logger.Information(CommonLogHelpers.CompletedSaveChangeBankDetails);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(Error.SaveChangeBankDetailsError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                    ViewBag.error = (Error.ViewBagError);
                    return View(Error.ErrorPath);
                }

                // Product Details
                try
                {
                    _logger.Information(CommonLogHelpers.StartedSaveProductDetails);

                    if (_sessionManager.GetProductDetailsList().Count != 0)
                    {
                        List<IdmUnitProductsDTO> AllProductList = _sessionManager.GetProductDetailsList();

                        foreach (var item in AllProductList)
                        {
                            switch (item.Action)
                            {
                                case (int)Constant.Delete:
                                    await _unitDetailsService.DeleteProductDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedDeleteProductDetails);
                                    break;
                                case (int)Constant.Update:
                                    await _unitDetailsService.UpdateProductDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedUpdateProductDetails);
                                    break;
                                case (int)Constant.Create:
                                    await _unitDetailsService.CreateProductDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedCreateProductDetails);
                                    break;
                                default:
                                    break;
                            }
                        }
                        _logger.Information(CommonLogHelpers.CompletedSaveProductDetails);                       
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(Error.SaveProductDetailsError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                    ViewBag.error = (Error.ViewBagError);
                    return View(Error.ErrorPath);
                }
                return Json(new { isValid = true });
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveAllUnitDetailsError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
            
        }

        //Author: Manoj Date:25/08/2022
        [HttpPost]
        public async Task<IActionResult> SaveAllPromoterDetails()
        {
            try
            {
                //Promoter Profile Save
                try
                { 
                _logger.Information(CommonLogHelpers.StartedSavePromoterProfile);

                    if (_sessionManager.GetAllPromoterProfileList().Count != 0)
                    {
                        List<IdmPromoterDTO> AllPromoterProfileList = _sessionManager.GetAllPromoterProfileList();

                        foreach (var item in AllPromoterProfileList)
                        {
                            switch (item.Action)
                            {
                                case (int)Constant.Delete:
                                    await _unitDetailsService.DeletePromoterProfileDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedDeletePromoterProfile);
                                    break;
                                case (int)Constant.Update:
                                    await _unitDetailsService.UpdatePromoterProfileDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedUpdatePromoterProfile);
                                    break;
                                case (int)Constant.Create:
                                    await _unitDetailsService.CreatePromoterProfileDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedCreatePromoterProfile);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    
                    _logger.Information(CommonLogHelpers.CompletedSavePromoterProfile);
                }
                catch(Exception ex)
                {
                    ViewBag.error = (Error.ViewBagError + ex.Message);
                    return View(Error.ErrorPath);
                }
                // Promoter Address Save
                try
                {
                    _logger.Information(CommonLogHelpers.StartedSavePromoterAddressDetails);

                    if (_sessionManager.GetAllPromoAddressDetails().Count != 0)
                    {
                        List<IdmPromAddressDTO> AllPromoterAddressList = _sessionManager.GetAllPromoAddressDetails();

                        foreach (var item in AllPromoterAddressList)
                        {
                            switch (item.Action)
                            {
                                case (int)Constant.Delete:
                                    await _unitDetailsService.DeletePromAddressDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedDeletePromoterAddressDetails);
                                    break;
                                case (int)Constant.Update:
                                    await _unitDetailsService.UpdatePromAddressDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedUpdatePromoterAddressDetails);
                                    break;
                                case (int)Constant.Create:
                                    await _unitDetailsService.CreatePromAddressDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedCreatePromoterAddressDetails);
                                    break;
                                default:
                                    break;
                            }

                        }
                        _logger.Information(CommonLogHelpers.CompletedSavePromoterAddressDetails);
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.error = (Error.ViewBagError + ex.Message);
                    return View(Error.ErrorPath);
                }


                // Promoter Bank Details
                try
                {
                    _logger.Information(CommonLogHelpers.StartedSavePromoterBank);

                    if (_sessionManager.GetAllPromoterBankList().Count != 0)
                    {
                        List<IdmPromoterBankDetailsDTO> AllPromoterBankList = _sessionManager.GetAllPromoterBankList();

                        foreach (var item in AllPromoterBankList)
                        {
                            switch (item.Action)
                            {
                                case (int)Constant.Delete:
                                    await _unitDetailsService.DeletePromoterBankInfo(item);
                                    _logger.Information(CommonLogHelpers.CompletedDeletePromoterBank);
                                    break;
                                case (int)Constant.Update:
                                    await _unitDetailsService.UpdatePromoterBankInfo(item);
                                    _logger.Information(CommonLogHelpers.CompletedUpdatePromoterBank);
                                    break;
                                case (int)Constant.Create:
                                    await _unitDetailsService.CreatePromoterBankInfo(item);
                                    _logger.Information(CommonLogHelpers.CompletedCreatePromoterBank);
                                    break;
                                default:
                                    break;
                            }
                        }
                        _logger.Information(CommonLogHelpers.CompletedSavePromoterBank);
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.error = (Error.ViewBagError + ex.Message);
                    return View(Error.ErrorPath);
                }

                //Promoter Asset Information
                try
                {
                    _logger.Information(CommonLogHelpers.StartedSaveAssetDetails);

                    if (_sessionManager.GetAssetDetailsList().Count != 0)
                    {
                        List<IdmPromAssetDetDTO> AllAssettList = _sessionManager.GetAssetDetailsList();
               
                        var output = AllAssettList.Select(x => x.PromoterCode).Distinct().ToList();

                        for (int j = 0; j < output.Count; j++)
                        {
                            List<IdmPromAssetDetDTO>  AllAssettList1 = AllAssettList.Where(x => x.PromoterCode == output[j]).ToList();

                            TblPromoterNetWortDTO AssetList = new();
                            AssetList.IdmMov = 0;
                            foreach (var item in AllAssettList1)
                            {
                                AssetList.PromoterCode = item.PromoterCode;

                                if (item.IsActive == false)
                                {
                                    AssetList.IdmMov = AssetList.IdmMov - item.IdmAssetValue;
                                }
                                else
                                {
                                    AssetList.IdmMov = AssetList.IdmMov + item.IdmAssetValue;
                                }
                                AssetList.LoanAcc = item.LoanAcc;
                                AssetList.OffcCd = item.OffcCd;
                                AssetList.LoanSub = item.LoanSub;
                                AssetList.UTCD = item.UtCd;
                            }
                            await _unitDetailsService.SaveAssetNetworthDetails(AssetList);
                            _logger.Information(CommonLogHelpers.CompletedSaveAssetNetworthDetails);
                        }

                        foreach (var item in AllAssettList)
                        {
                            switch (item.Action)
                            {
                                case (int)Constant.Delete:
                                    await _unitDetailsService.DeleteAssetDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedDeleteAssetDetails);
                                    break;
                                case (int)Constant.Update:
                                    await _unitDetailsService.UpdateAssetDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedUpdateAssetDetails);
                                    break;
                                case (int)Constant.Create:
                                    await _unitDetailsService.CreateAssetDetails(item);
                                    _logger.Information(CommonLogHelpers.CompletedCreateAssetDetails);
                                    break;
                                default:
                                    break;
                            }
                        }
                        _logger.Information(CommonLogHelpers.CompletedSaveAssetDetails);
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.error = (Error.ViewBagError + ex.Message);
                    return View(Error.ErrorPath);
                }


                //Promoter Liability Information
                try
                {
                    if (_sessionManager.GetAllPromoterLiabilityInfo().Count != 0)
                    {
                        _logger.Information(CommonLogHelpers.StartedSavePromoterLiabilityInformation);

                        List<TblPromoterLiabDetDTO> AllLiabilityList = _sessionManager.GetAllPromoterLiabilityInfo();

                        var output = AllLiabilityList.Select(x => x.PromoterCode).Distinct().ToList();

                        for (int j = 0; j < output.Count; j++)
                        {
                            List<TblPromoterLiabDetDTO> AllLiabilityList1 = AllLiabilityList.Where(x => x.PromoterCode == output[j]).ToList();
                            TblPromoterNetWortDTO LiabilityList = new();
                            LiabilityList.IdmLiab = 0;
                            foreach (var item in AllLiabilityList1)
                            {
                                LiabilityList.PromoterCode = item.PromoterCode;
                                if(item.IsActive == false)
                                {
                                    LiabilityList.IdmLiab = LiabilityList.IdmLiab - item.LiabVal;
                                    
                                }
                                else
                                {
                                    LiabilityList.IdmLiab = LiabilityList.IdmLiab + item.LiabVal;
                                }
                                LiabilityList.LoanAcc = item.LoanAcc;
                                LiabilityList.LoanSub = item.LoanSub;
                                LiabilityList.OffcCd = item.OffcCd;
                                LiabilityList.UTCD = item.UTCD;
                            }
                            await _unitDetailsService.SaveLiabilityNetworthDetails(LiabilityList);
                            _logger.Information(CommonLogHelpers.CompletedSaveLiabilityNetworthDetails);
                        }

                        foreach (var item in AllLiabilityList)
                        {
                            switch (item.Action)
                            {
                                case (int)Constant.Delete:
                                    await _unitDetailsService.DeletePromoLiabilityInfo(item);
                                    _logger.Information(CommonLogHelpers.CompletedDeletePromoterLiability);
                                    break;
                                case (int)Constant.Update:
                                    await _unitDetailsService.UpdatePromoLiabilityInfo(item);
                                    _logger.Information(CommonLogHelpers.CompletedUpdatePromoterLiability);
                                    break;
                                case (int)Constant.Create:
                                    await _unitDetailsService.CreatePromoLiabilityInfo(item);
                                    _logger.Information(CommonLogHelpers.CompletedCreatePromoterLiability);
                                    break;
                                default:
                                    break;
                            }
                        }

                        _logger.Information(CommonLogHelpers.CompletedSavePromoterLiabilityInformation);
                    }
                }
                catch(Exception ex)
                {
                    ViewBag.error = (Error.ViewBagError + ex.Message);
                    return View(Error.ErrorPath);
                }
                return Ok(new { isValid = true });
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SavePromoterLiabilityInformatioError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }


        
        [HttpPost]
        public async Task<IActionResult> SaveWorkingCapitalDetails(IdmDchgWorkingCapitalDTO model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    model.IsActive = true;
                    model.IsDeleted = false;
                    model.CreatedDate = DateTime.UtcNow;
                    model.UniqueId = Guid.NewGuid().ToString();
                    _sessionManager.SetWorkingCapitalDetails(model);

                    await _unitDetailsService.CreateWorkingCapitalDetails(model);
                    ViewBag.AccountNumber = model.LoanAcc;
                    _logger.Information(CommonLogHelpers.CompletedSaveWorkingCapitalInspectionDetails);
                    return Json(new { isValid = true });
                }
                return Json(new { isValid = false });
            }

            catch (Exception ex)
            {
                _logger.Error(Error.SaveWorkingCapitalInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }

        }



        //Author:Swetha M Date:02/09/2022
        [HttpPost]
        public async Task<IActionResult> SaveMeansOfFinanceDetails()
        {

            try
            {
                if (_sessionManager.GetAllMeansOfFinanceList().Count != 0)
                {
                    var allMeansOfFinanceList = _sessionManager.GetAllMeansOfFinanceList();

                    foreach (var item in allMeansOfFinanceList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _unitDetailsService.DeleteMeansOfFinanceDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedMeansOfFinanceDelete);
                                break;
                            case (int)Constant.Update:
                                await _unitDetailsService.UpdateMeansOfFinanceDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedMeansOfFinanceUpdate);
                                break;
                            case (int)Constant.Create:
                                await _unitDetailsService.CreateMeansOfFinanceDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedMeansOfFinanceCreate);
                                break;
                            default:
                                break;
                        }

                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveBuildingInspectionDetails);
                    ViewBag.AccountNumber = allMeansOfFinanceList.First().LoanAcc;
                    return Json(new { isValid = true });
                }
                return Ok();
            }

            catch (Exception ex)
            {
                _logger.Error(Error.SaveMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        //Author: Akhila Date:06/09/2022
        [HttpPost]
        public async Task<IActionResult> SaveProjectCostDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StratedSaveProjectCostDetails);

                var projectCostList = _sessionManager.GetAllProjectCostList();

                foreach (var item in projectCostList)
                {

                    switch (item.Action)
                    {
                        case (int)Constant.Delete:
                            await _unitDetailsService.DeleteProjectCostDetails(item);
                            _logger.Information(CommonLogHelpers.CompletedProjectCostDelete);
                            break;
                        case (int)Constant.Update:
                            await _unitDetailsService.UpdateProjectCostDetails(item);
                            _logger.Information(CommonLogHelpers.CompletedProjectCostUpdate);
                            break;
                        case (int)Constant.Create:
                            await _unitDetailsService.CreateProjectCostDetails(item);
                            _logger.Information(CommonLogHelpers.CompletedProjectCostCreate);
                            break;
                        default:
                            break;
                    }
                }
                _logger.Information(CommonLogHelpers.CompletedSaveProjectCostDetails);

                return Json(new { isValid = true });

            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveProjectCostError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }


        //Author:Manoj Date:05/09/2022
        //[HttpPost]
        //public async Task<IActionResult> SaveLetterOfCreditDetail()
        //{

        //    try
        //    {
        //        if (_sessionManager.GetAllLetterOfCreditDetail().Count != 0)
        //        {
        //            var allLetterOfCreditList = _sessionManager.GetAllLetterOfCreditDetail();

        //            foreach (var item in allLetterOfCreditList)
        //            {
        //                switch (item.Action)
        //                {
        //                    case (int)Constant.Delete:
        //                        await _unitDetailsService.DeleteLetterOfCreditDetails(item);
        //                        _logger.Information(CommonLogHelpers.CompletedLetterOfCreditDelete);
        //                        break;
        //                    case (int)Constant.Update:
        //                        await _unitDetailsService.UpdateLetterOfCreditDetails(item);
        //                        _logger.Information(CommonLogHelpers.CompletedLetterOfCreditUpdate);
        //                        break;
        //                    case (int)Constant.Create:
        //                        await _unitDetailsService.CreateLetterOfCreditDetails(item);
        //                        _logger.Information(CommonLogHelpers.CompletedLetterOfCreditCreate);
        //                        break;
        //                    default:
        //                        break;
        //                }

        //            }

        //            _logger.Information(CommonLogHelpers.CompletedSaveBuildingInspectionDetails);

        //            return Json(new { isValid = true });
        //        }
        //        return Ok();
        //    }

        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.SaveLetterOfCredit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        ViewBag.error = (Error.ViewBagError);
        //        return View(Error.ErrorPath);
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> SaveAllocationDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveAllocationDetails);
                if (_sessionManager.GetAllLoanAllocationList().Count != 0)
                {
                    var LoanAllocationList = _sessionManager.GetAllLoanAllocationList();

                    foreach (var item in LoanAllocationList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _unitDetailsService.DeleteLoanAllocationDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLoanAllocationDelete);
                                break;
                            case (int)Constant.Update:
                                await _unitDetailsService.UpdateLoanAllocationDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLoanAllocationUpdate);
                                break;
                            case (int)Constant.Create:
                                await _unitDetailsService.CreateLoanAllocationDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLoanAllocationCreate);
                                break;
                            default:
                                break;
                        }

                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveAllocationDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
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
