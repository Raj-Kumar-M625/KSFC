using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Security;
using KAR.KSFC.UI.Services.Admin.IDM;
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

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.InspectionOfUnit
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class InspectionOfUnitController : Controller
    {
        private readonly IInspectionOfUnitService _inspectionOfUnitService;
        private readonly SessionManager _sessionManager;
        private readonly ICommonService _commonService;
        private readonly ILogger _logger;
        private readonly IIdmService _idmService;
        private readonly IDataProtector protector;
        private readonly IUnitDetailsService _unitDetailsService;
        private readonly ICreationOfSecurityandAquisitionAssetService _creationOfSecurityandAquisitionAssetService;

        public InspectionOfUnitController(IInspectionOfUnitService inspectionOfUnitService, ILogger logger, SessionManager sessionManager,
            ICommonService commonService, IIdmService idmService, IDataProtectionProvider dataProtectionProvider, ICreationOfSecurityandAquisitionAssetService creationOfSecurityandAquisitionAssetService,
            DataProtectionPurposeStrings dataProtectionPurposeStrings, IUnitDetailsService unitDetailsService)
        {
            _inspectionOfUnitService = inspectionOfUnitService;
            _logger = logger;
            _sessionManager = sessionManager;
            _commonService = commonService;
            _idmService = idmService;
            _unitDetailsService = unitDetailsService;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.AdminRouteValue);
            _creationOfSecurityandAquisitionAssetService = creationOfSecurityandAquisitionAssetService;

        }
        //Author: Manoj Date:25/08/2022
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
        //Author: Manoj Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings Date:19/10/2022
        public async Task<IActionResult> ViewAccount(string AccountNumber, string LoanSub, string UnitName , string OffcCd, long ProjectCostID, string MainModule)
        {
           
            try
            {
                long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
                byte offCd = Convert.ToByte(protector.Unprotect(OffcCd));
                int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
                string unitname = protector.Unprotect(UnitName);
                _logger.Information(CommonLogHelpers.InspectionDetailsviewAccount);

                IdmDDLListDTO idmDDLListDTO = await _commonService.GetAllIdmDropDownList();

                _sessionManager.SetConditionTypeDDL(idmDDLListDTO.AllStateZone);
              //  _sessionManager.SetDDListProjectCostComponent(idmDDLListDTO.AllProjectCostComponents);
              //  _sessionManager.SetFinanceCategoryList(idmDDLListDTO.AllFinanceCategory);


                _logger.Information(Constants.FileList);
                var ldFileList = await _commonService.FileList(MainModule);
                _sessionManager.SetIDMDocument(ldFileList);

                _logger.Information(CommonLogHelpers.GetAllInspectionDetailsList);
                var InspectionDetailList = await _inspectionOfUnitService.GetAllInspectionDetailsList(accountNumber);
                foreach (var i in InspectionDetailList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                        i.EncryptedLoanAcc = protector.Protect(i.LoanAcc.ToString());
                        i.EncryptedOffcCd = protector.Protect(i.OffcCd.ToString());
                        i.EncryptedLoanSub = protector.Protect(i.LoanSub.ToString());
                    }
                }
                _sessionManager.SetInspectionDetialList(InspectionDetailList);

                _logger.Information(CommonLogHelpers.GetAllLetterOfCreditDetailsList);
                var LetterOfCreditList = await _inspectionOfUnitService.GetAllLetterOfCreditDetailsList(accountNumber);
                foreach (var i in LetterOfCreditList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                }
                _sessionManager.SetLetterOfCreditList(LetterOfCreditList);

                //_logger.Information(CommonLogHelpers.GetAllMeansOfFinanceList);
                //var meansOfFinanceDetails = await _inspectionOfUnitService.GetAllMeansOfFinanceList(accountNumber);


                //var financeCategory = _sessionManager.GetAllFinanceCategoryList();
                //foreach (var i in meansOfFinanceDetails)
                //{
                //    if (i.UniqueId == null)
                //    {
                //        i.UniqueId = Guid.NewGuid().ToString();
                //    }
                //    var categoryCode = financeCategory.Where(x => x.Value == i.DcmfCd.ToString());
                //    i.Category = categoryCode.First().Text;
                //  var id = i.DcmfCd;
                //    if (id != null)
                //    {
                //        var AllFinancetype = await _inspectionOfUnitService.GetFinanceTypeAsync();

                //        var AllFinanceCode = AllFinancetype.Where(x => x.Value == i.DcmfMfType.ToString());
                //        i.FinanceType = AllFinanceCode.First().Text;
                //    }

                //}
                //_sessionManager.SetMeansOfFinanceList(meansOfFinanceDetails);

                //_logger.Information(CommonLogHelpers.GetAllProjectCostComponentsDetails);
                //var allprojectCostComponentDetail = await _inspectionOfUnitService.GetAllProjectCostComponentsDetails();
                //List<SelectListItem> projectCostComponentDetail = allprojectCostComponentDetail.Select(x => new SelectListItem
                //{
                //    Value = x.Value.ToString(),
                //    Text = x.Text
                //}).ToList();

                //var ProjectCostDetailsList = await _inspectionOfUnitService.GetAllProjectCostDetailsList(accountNumber, ProjectCostID);
                //foreach (var i in ProjectCostDetailsList)
                //{
                //    if (i.UniqueId == null)
                //    {
                //        i.UniqueId = Guid.NewGuid().ToString();
                //    }
                //    var projectCostComponent = allprojectCostComponentDetail.Where(x => x.Value == i.DcpcstCode);
                //    i.ProjectCost = projectCostComponent.First().Text;
                //}
                //_sessionManager.SetProjectCostList(ProjectCostDetailsList);


                var allIfscBankCode = await _unitDetailsService.GetAllIfscbankDetails();
                _sessionManager.SetIfscBankDetailsList(allIfscBankCode);

               // _sessionManager.SetProjectCostDetailsList(projectCostComponentDetail);

                InspectionOfUnitDTO inspectionOfUnitDTO = new();
                inspectionOfUnitDTO.ProjectCostComponents = idmDDLListDTO.AllProjectCostComponents;
                inspectionOfUnitDTO.InspectionDetail = InspectionDetailList.ToList();
                //inspectionOfUnitDTO.MeansOfFinanceDetails = meansOfFinanceDetails.ToList();
                inspectionOfUnitDTO.LetterOfCreditList = LetterOfCreditList.ToList();
                //inspectionOfUnitDTO.ProjectCostDetail = ProjectCostDetailsList.ToList();
                ViewBag.Id = ProjectCostID;
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.OffcCd = offCd;
                ViewBag.EncryptedUnitName = UnitName;
                return View(inspectionOfUnitDTO);
            }
            catch (Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }

        }
        //Author: Manoj Date:25/08/2022
        //Modified: Gagana K Added try catch with Loggings and Added LandType dropdown Date:19/10/2022- 
        public async Task<IActionResult> ViewInspectionAccount(string AccountNumber, string LoanSub, long InspectionId,string NameOfUnit, string OffcCd)
        {
            try
            { 
                long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
                byte offCd = Convert.ToByte(protector.Unprotect(OffcCd));
                int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
                string unitname = protector.Unprotect(NameOfUnit);

                IdmDDLListDTO idmDDLListDTO = await _commonService.GetAllIdmDropDownList();

                _sessionManager.SetRegisteredStateDDL(idmDDLListDTO.AllStateZone);

                _sessionManager.SetDDListLandType(idmDDLListDTO.AllLandType);
                _sessionManager.SetAllUmoMasterlist(idmDDLListDTO.AllUmoMasterDetails);

                _logger.Information(CommonLogHelpers.GetAllLandType);
                var allListLandType = await _idmService.GetAllLandType();

                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                var allLandInspectionList = await _inspectionOfUnitService.GetAllLandInspectionList(accountNumber, InspectionId);
               
                _sessionManager.SetLandInspectionList(allLandInspectionList);

                _logger.Information(CommonLogHelpers.GetAllLandInspectionList);
              
                var lastCreatedDate = inspectiondetailslist.Max(x => x.CreatedDate);
                var inspnumber = inspectiondetailslist.Where(x => x.CreatedDate == lastCreatedDate).Select(x => x.DinNo).First();
                
                //if (!allLandInspectionList.Any())
                //{
                //    ViewBag.firstbuildinginspection = true; // for  land inspection
                //}
                //else
                //{
                //    ViewBag.firstbuildinginspection = false; // for  land inspection
                //}
                var latestInspection = inspectiondetailslist.OrderByDescending(insp => insp.CreatedDate).FirstOrDefault();

                if (latestInspection == null || allLandInspectionList.Any(land => land.DcLndIno == latestInspection.DinNo))
                {
                    ViewBag.firstbuildinginspection = false;
                }
                else 
                {
                    ViewBag.firstbuildinginspection = true;
                }

                foreach (var i in allLandInspectionList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var LandType = allListLandType.Where(x => x.Value == i.DcLndType);
                    i.LandType = LandType.First().Text;
                }
                var landAcquisition = await _creationOfSecurityandAquisitionAssetService.GetAllCreationOfSecurityandAquisitionAssetList(accountNumber, InspectionId);

                    //var landAcquisitionDetails = landAcquisition.Where(x => x.IrlIno == InspectionId).ToList();

                    foreach (var item in landAcquisition)
                    {
                        foreach (var item1 in allLandInspectionList)
                        {
                            if (item1.UniqueId == item.UniqueId)
                            {
                                item1.IrlAreaIn = item.IrlAreaIn;
                                item1.IrlSecValue = item.IrlSecValue;
                                item1.IrlId = item.IrlId;
                                item1.IrlRelStat = item.IrlRelStat;
                            }
                        }
                    }

                    _sessionManager.SetLandInspectionList(allLandInspectionList);
                //}
                //else
                //{
                //    //TODO: add code to handle the case where the inspectionlist is empty or the InspectionId doesn't match
                //}


               

                //_logger.Information(CommonLogHelpers.GetAllBuildingMaterialInspectionList);

                var allbuildMatSiteInspectionList = await _inspectionOfUnitService.GetAllBuildingMaterialInspectionList(accountNumber, InspectionId);
                _sessionManager.SetBuildMatSiteInspectionList(allbuildMatSiteInspectionList);

                //if (!allbuildMatSiteInspectionList.Any())
                //{
                //    ViewBag.firstbuildingmaterialinspection = true;
                //}
                //else
                //{
                //    ViewBag.firstbuildingmaterialinspection = false;
                //}
                _logger.Information(CommonLogHelpers.GetAllBuildingMaterialInspectionList);

                var lastCreatedDates = inspectiondetailslist.Max(x => x.CreatedDate);
                var inspnumbers = inspectiondetailslist.Where(x => x.CreatedDate == lastCreatedDates).Select(x => x.DinNo).First();

                var LatestInspection = inspectiondetailslist.OrderByDescending(insp => insp.CreatedDate).FirstOrDefault();
                if (LatestInspection == null || allbuildMatSiteInspectionList.Any(material => material.IrbmIno== LatestInspection.DinNo))
                {
                    ViewBag.firstbuildingmaterialinspection = false;
                }
                else
                {
                    ViewBag.firstbuildingmaterialinspection = true;
                }

                var UmoMaster = await _idmService.GetAllUomMaster();
                foreach (var i in allbuildMatSiteInspectionList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var UmoMasterType = UmoMaster.Where(x => x.Value == i.UomId);
                    i.UmoDesc = UmoMasterType.First().Text;
                }

                _sessionManager.SetBuildMatSiteInspectionList(allbuildMatSiteInspectionList);

               
                var allFurnitureInspectionList = await _inspectionOfUnitService.GetAllFurnitureInspectionList(accountNumber, InspectionId);
                _sessionManager.SetFurnitureInspectionList(allFurnitureInspectionList);

                //if (!allFurnitureInspectionList.Any())
                //{
                //    ViewBag.firstfurnitureinspection = true;
                //}
                //else
                //{
                //    ViewBag.firstfurnitureinspection = false;
                //}

                _logger.Information(CommonLogHelpers.GetAllFurnitureInspectionList);
                var lastCreateDate = inspectiondetailslist.Max(x => x.CreatedDate);
                var inspnumberss  = inspectiondetailslist.Where(x => x.CreatedDate == lastCreateDate).Select(x => x.DinNo).First();

                var LatestInspections = inspectiondetailslist.OrderByDescending(insp => insp.CreatedDate).FirstOrDefault();
                if (LatestInspections == null || allFurnitureInspectionList.Any(furniure => furniure.FurnIno == LatestInspections.DinNo))
                {
                    ViewBag.firstfurnitureinspection = false;
                }
                else
                {
                    ViewBag.firstfurnitureinspection = true;
                }


                foreach (var i in allFurnitureInspectionList)
                {
                    if (i.UniqueID == null)
                    {
                        i.UniqueID = Guid.NewGuid().ToString();
                    }
                }
                var furnitureAcquisition = await _creationOfSecurityandAquisitionAssetService.GetFurnitureAcquisitionList(accountNumber,InspectionId);

                //var furnitureAcquisitionDetails  = furnitureAcquisition.Where(x => x.UniqueId == allFurnitureInspectionList.).ToList();

                foreach (var item in furnitureAcquisition)
                {
                    foreach (var item1 in allFurnitureInspectionList)
                    {
                        if (item1.UniqueID == item.UniqueId)
                        {
                            item1.IrfSecAmt = item.IrfSecAmt;
                            item1.IrfTotalRelease = item.IrfTotalRelease;
                            item1.IrfId = item.IrfId;
                            item1.IrfRelStat = item.IrfRelStat;
                        }
                    }
                }

                _sessionManager.SetFurnitureInspectionList(allFurnitureInspectionList);



                var InspectionDetailList = await _inspectionOfUnitService.GetAllInspectionDetailsList(accountNumber);
                foreach (var i in InspectionDetailList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                }

                _sessionManager.SetInspectionDetialList(InspectionDetailList);
                //UC 08
                _logger.Information(Constants.GetAllocationCodeDetails);
                var alllocationDetails = await _inspectionOfUnitService.GetAllocationCodeDetails();
                _logger.Information(Constants.GetRecomDisbursementReleaseDetails);
                var allRecomDisbursementReleaseDetails = await _inspectionOfUnitService.GetRecomDisbursementReleaseDetails();
                _logger.Information(Constants.GetAllRecomDisbursementDetails);
                var allRecomDisbursementList = await _inspectionOfUnitService.GetAllRecomDisbursementDetails(accountNumber);
                foreach (var i in allRecomDisbursementList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                    var allcCode = alllocationDetails.Where(x => x.AllcId == i.DsbAcd);
                    i.AllocationDetails = allcCode.First().AllcDets;
                    var eligibleAmt = allRecomDisbursementReleaseDetails.Where(x => x.PropNumber == i.DsbNo);
                    // i.EligibleAmt = eligibleAmt.FirstOrDefault().ReleAmount;
                    //i.ReleAmt = eligibleAmt.FirstOrDefault().ReleAmount;
                }
                 ViewBag.AllRecomDisbursementList = allRecomDisbursementList;
                _sessionManager.SetRecommDisbursementList(allRecomDisbursementList);

                _logger.Information(CommonLogHelpers.GetAllStatusofImplementationList);
                var allStatusofImplementationList = await _inspectionOfUnitService.GetTblDsbStatImps(accountNumber, InspectionId);
                _sessionManager.SetStatusofImplementaionList(allStatusofImplementationList);

                var latestInspectionsss = inspectiondetailslist.OrderByDescending(insp => insp.CreatedDate).FirstOrDefault();
                if (latestInspectionsss == null || allStatusofImplementationList.Any(statusImp => statusImp.DsbIno == latestInspection.DinNo))
                {
                    ViewBag.firststatImp = false;
                }
                else
                {
                    ViewBag.firststatImp = true;
                }

                foreach (var i in allStatusofImplementationList)
                {

                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                }
                _sessionManager.SetStatusofImplementaionList(allStatusofImplementationList);
                var allBuildingInspectionList = await _inspectionOfUnitService.GetAllBuildingnspectionList(accountNumber, InspectionId);
                _sessionManager.SetBuildingInspectionList(allBuildingInspectionList);

                //if (!allBuildingInspectionList.Any())
                //{
                //    ViewBag.firstbuildinspection = true;
                //}
                //else
                //{
                //    ViewBag.firstbuildinspection = false;
                //}
                _logger.Information(CommonLogHelpers.GetAllBuildingnspectionList);
                var LastCreateDate = inspectiondetailslist.Max(x => x.CreatedDate);
                var Inspnumber  = inspectiondetailslist.Where(x => x.CreatedDate == LastCreateDate).Select(x => x.DinNo).First();

                var latestInspections = inspectiondetailslist.OrderByDescending(insp => insp.CreatedDate).FirstOrDefault();

                if (latestInspections == null || allBuildingInspectionList.Any(building => building.DcBdgIno == latestInspections.DinNo))
                {
                    ViewBag.firstbuildinspection = false;
                }
                else
                {
                    ViewBag.firstbuildinspection = true;
                }

                foreach (var i in allBuildingInspectionList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                    }
                }
               
                var buildingAcquisition = await _creationOfSecurityandAquisitionAssetService.GetAllBuildingAcquisitionDetails(accountNumber,InspectionId);



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
                            item1.IrbSecValue = item.IrbSecValue;
                            item1.Irbid = item.Irbid;
                        }
                    }
                }
                    _sessionManager.SetBuildingInspectionList(allBuildingInspectionList);

                

             
                    _logger.Information(CommonLogHelpers.GetAllImportMachineryList);
                    var allImportMachineryInspection = await _inspectionOfUnitService.GetAllImportMachineryList(accountNumber, InspectionId);

               var importMachinaryAqusition  = await _creationOfSecurityandAquisitionAssetService.GetAllMachineryAcquisitionDetails(accountNumber, InspectionId);

                foreach (var items in importMachinaryAqusition)
                {
                    foreach (var item1 in allImportMachineryInspection)
                    {
                        if (item1.UniqueId == items.UniqueId)
                        {
                            item1.IrPlmcSecAmt = items.IrPlmcSecAmt;
                            item1.IrPlmcAamt = items.IrPlmcAamt;
                            item1.IrPlmcTotalRelease = items.IrPlmcTotalRelease;
                            item1.IrPlmcRelseStat = items.IrPlmcRelseStat;
                            item1.IrPlmcId = items.IrPlmcId;
                        }
                    }

                }
                _sessionManager.SetImportMachineryList(allImportMachineryInspection);

                   _logger.Information(CommonLogHelpers.GetAllImportMachineryList);

                  //var lastCreatedDated = allImportMachineryInspection.Max(x => x.CreatedDate);
                  //var inspnumbers = inspectiondetailslist.Where(x => x.CreatedDate == lastCreatedDate).Select(x => x.DinNo).First();
                    //if (!allImportMachineryInspection.Any())
                  //{
                  //    ViewBag.firstimportMachine = true;
                  //}
                  //else
                  //{
                 //    ViewBag.firstimportMachine = false;
                //}
                var latestInspectionss = inspectiondetailslist.OrderByDescending(insp => insp.CreatedDate).FirstOrDefault();
                if (latestInspectionss == null || allImportMachineryInspection.Any(machinery => machinery.DimcIno == latestInspection.DinNo))
                {
                    ViewBag.firstimportMachine = false;
                }
                else
                {
                    ViewBag.firstimportMachine = true;
                }


                var allIndigenousMachineryInspectionList = await _inspectionOfUnitService.GetAllIndigenousMachineryInspectionList(accountNumber, InspectionId);
                _sessionManager.SetIndigenousMachineryInspectionList(allIndigenousMachineryInspectionList);

                _logger.Information(CommonLogHelpers.GetAllIndigenousMachineryInspectionList);

                //var lastCreatDate = allIndigenousMachineryInspectionList.Max(x => x.CreatedDate);
                //var inspnumbes = inspectiondetailslist.Where(x => x.CreatedDate == lastCreatedDate).Select(x => x.DinNo).First();

                //if (!allIndigenousMachineryInspectionList.Any())
                //{
                //    ViewBag.firstindigenousMachine = true;
                //}
                //else
                //{
                //    ViewBag.firstindigenousMachine = false;
                //}
                var latestInspectiones = inspectiondetailslist.OrderByDescending(insp => insp.CreatedDate).FirstOrDefault();
                if (latestInspections == null || allIndigenousMachineryInspectionList.Any(machinery => machinery.Ino == latestInspection.DinNo))
                {
                    ViewBag.firstindigenousMachine = false;
                }
                else
                {
                    ViewBag.firstindigenousMachine = true;
                }

                var MachinaryAqusition = await _creationOfSecurityandAquisitionAssetService.GetAllMachineryAcquisitionDetails(accountNumber ,InspectionId);

                foreach (var items in MachinaryAqusition)
                {
                    foreach (var item1 in allIndigenousMachineryInspectionList)
                    {
                        if (item1.UniqueId == items.UniqueId)
                        {
                            item1.IrPlmcSecAmt = items.IrPlmcSecAmt;
                            item1.IrPlmcAamt = items.IrPlmcAamt;
                            item1.IrPlmcTotalRelease = items.IrPlmcTotalRelease;
                            item1.IrPlmcRelseStat = items.IrPlmcRelseStat;
                            item1.IrPlmcId = items.IrPlmcId;
                        }
                    }

                }
                _sessionManager.SetIndigenousMachineryInspectionList(allIndigenousMachineryInspectionList);




                var machineryStatusList = await _inspectionOfUnitService.GetAllMachineryStatusList();
                var procureList = await _inspectionOfUnitService.GetAllProcureList();
                var currencyList = await _inspectionOfUnitService.GetAllCurrencyList(); 
                //foreach (var i in allIndigenousMachineryInspectionList)
                //{
                //    if (i.MachineryStatus != null)
                //    {
                //        var machinarydesc = machineryStatusList.Where(x => x.MacStatus == i.MachineryStatus);
                //        i.MacDets = machinarydesc.First().MacDets;
                //    }
                    _logger.Information(CommonLogHelpers.GetAllIndigenousMachineryInspectionList);
                    var allIndigenousMachineryInspectionLists = await _inspectionOfUnitService.GetAllIndigenousMachineryInspectionList(accountNumber, InspectionId);
                    var machineryStatusLists = await _inspectionOfUnitService.GetAllMachineryStatusList();
                    var procureLists = await _inspectionOfUnitService.GetAllProcureList();
                    var currencyLists = await _inspectionOfUnitService.GetAllCurrencyList();
                    //foreach (var i in allIndigenousMachineryInspectionList)
                    //{
                    //    if (i.MachineryStatus != null)
                    //    {
                    //        var machinarydesc = machineryStatusList.Where(x => x.MacStatus == i.MachineryStatus);
                    //        i.MacDets = machinarydesc.First().MacDets;
                    //    }

                    //}
                    _sessionManager.SetProcureList(procureList);
                    _sessionManager.SetCurrencyList(currencyList);
                    _sessionManager.SetMachinaryStatusList(machineryStatusList);
                    _sessionManager.SetIndigenousMachineryInspectionList(allIndigenousMachineryInspectionList);



                    InspectionAccountDTO inspectionAccountDTO = new();

                    inspectionAccountDTO.LandInspectionDetails = allLandInspectionList.Where(x => x.IsDeleted==false).OrderBy(e => e.CreatedDate).ToList();
                    inspectionAccountDTO.BuildingInspectionDetails = allBuildingInspectionList.Where(x => x.IsDeleted == false).ToList();
                    inspectionAccountDTO.BuildMatSiteInspectionDetails = allbuildMatSiteInspectionList.Where(x => x.IsDeleted == false).ToList();
                    inspectionAccountDTO.IndigenousMachineryInspectionDetails = allIndigenousMachineryInspectionList.Where(x => x.IsDeleted == false).ToList();
                    inspectionAccountDTO.ImportMachineryInspection = allImportMachineryInspection.Where(x => x.IsDeleted == false).ToList();
                    inspectionAccountDTO.FurnitureInspectionDetails = allFurnitureInspectionList.Where(x => x.IsDeleted == false).ToList();
                    inspectionAccountDTO.InspectionDetail = inspectiondetailslist.Where(x => x.IsDeleted == false).ToList();
                    inspectionAccountDTO.StatusofImplementationDetails = allStatusofImplementationList.Where(x => x.IsDeleted == false).ToList();
                    ViewBag.AccountNumber = accountNumber;
                    ViewBag.LoanSub = loansub;
                    ViewBag.UnitName = unitname;
                    ViewBag.OffcCd = offCd;
                    ViewBag.InspectionId = InspectionId;
                    ViewBag.PAccountNumber = AccountNumber;
                    ViewBag.PLoanSub = LoanSub;
                    ViewBag.PUnitName = NameOfUnit;
                    ViewBag.POffcCd = OffcCd;
                    return View(inspectionAccountDTO);
                
            }
            catch (Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
            
        }

        //Author:Swetha M Date:29/08/2022
        [HttpPost]
        public async Task<IActionResult> SaveBuildingInspectionDetails()
        {

            try
            {
                if (_sessionManager.GetAllBuildingInspectionList().Count != 0)
                {

                    var allBuildingInspection = _sessionManager.GetAllBuildingInspectionList();

                    foreach (var item in allBuildingInspection)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _inspectionOfUnitService.DeleteBuildingInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedBuildingInspectionDelete);
                                break;
                            case (int)Constant.Update:
                                await _inspectionOfUnitService.UpdateBuildingInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedBuildingInspectionUpdate);
                                break;
                            case (int)Constant.Create:
                                await _inspectionOfUnitService.CreateBuildingInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedBuildingInspectionCreate);
                                break;
                            default:
                                break;
                        }

                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveBuildingInspectionDetails);
                    ViewBag.AccountNumber = allBuildingInspection.First().LoanAcc;
                    return Json(new { isValid = true });
                }
                return Ok();
            }

            catch (Exception ex)
            {
                _logger.Error(Error.SaveBuildingInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }

        }

        //Author: Manoj Date:25/08/2022
        [HttpPost]
        public async Task<IActionResult> SaveLandInspectionDetails()
        {
            try
            {
                if (_sessionManager.GetAllLandInspectionList().Count != 0)
                {
                    _logger.Information(CommonLogHelpers.StratedSaveLandInspectionDetails);

                    var LandInspectionList = _sessionManager.GetAllLandInspectionList();

                    foreach (var item in LandInspectionList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _inspectionOfUnitService.DeleteLandInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLandInspectionDelete);
                                break;
                            case (int)Constant.Update:
                                await _inspectionOfUnitService.UpdateLandInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLandInspectionUpdate);
                                break;
                            case (int)Constant.Create:
                                await _inspectionOfUnitService.CreateLandInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLandInspectionCreate);
                                break;
                            default:
                                break;
                        }
                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveLandInspectionDetails);

                    return Json(new { isValid = true });
                }
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveLandInspectionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        //Author: Manoj Date:25/08/2022
        [HttpPost]
        public async Task<IActionResult> SaveBuildMatSiteInspectionDetails()
        {
            try
            {
                if (_sessionManager.GetAllBuildMatSiteInspectionList().Count != 0)
                {
                    _logger.Information(CommonLogHelpers.StratedSaveLandInspectionDetails);
                    var buildMatSiteInspectionList = _sessionManager.GetAllBuildMatSiteInspectionList();

                    foreach (var item in buildMatSiteInspectionList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _inspectionOfUnitService.DeleteBuildMatSiteInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedBuildMatSiteDelete);
                                break;
                            case (int)Constant.Update:
                                await _inspectionOfUnitService.UpdateBuildMatSiteInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedBuildMatSiteUpdate);
                                break;
                            case (int)Constant.Create:
                                await _inspectionOfUnitService.CreateBuildMatSiteInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedBuildMatSiteCreate);
                                break;
                            default:
                                break;
                        }
                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveLandInspectionDetails);
                    return Json(new { isValid = true });
                }
                return Ok();


            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveBuildMatSiteError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        //Author: Sandeep M Date:25/08/2022
        [HttpPost]
        public async Task<IActionResult> SaveInpsectionDetail()
        {
            try
            {
                var InspectionList = _sessionManager.GetAllInspectionDetail();
                if (_sessionManager.GetAllInspectionDetail().Count != 0)
                {
                    _logger.Information(CommonLogHelpers.StratedSaveInspectionDetails);
                    foreach (var item in InspectionList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _inspectionOfUnitService.DeleteInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedInspectionDetailsDelete);
                                break;
                            case (int)Constant.Update:
                                await _inspectionOfUnitService.UpdateInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedInspectionDetailsUpdate);
                                break;
                            case (int)Constant.Create:
                                await _inspectionOfUnitService.CreateInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedInspectionDetailsCreate);
                                break;
                            default:
                                break;
                        }
                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveInspectionDetails);
                    return Json(new { isValid = true });
                }
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        //Author:Swetha M Date:29/08/2022
        //[HttpPost]
        //public async Task<IActionResult> SaveWorkingCapitalDetails(IdmDchgWorkingCapitalDTO model)
        //{

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            model.IsActive = true;
        //            model.IsDeleted = false;
        //            model.CreatedDate = DateTime.UtcNow;
        //            model.UniqueId = Guid.NewGuid().ToString();
        //            _sessionManager.SetWorkingCapitalDetails(model);

        //            await _inspectionOfUnitService.CreateWorkingCapitalDetails(model);
        //            ViewBag.AccountNumber = model.LoanAcc;
        //            _logger.Information(CommonLogHelpers.CompletedSaveWorkingCapitalInspectionDetails);
        //            return Json(new { isValid = true });
        //        }
        //        return Json(new { isValid = false });
        //    }

        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.SaveWorkingCapitalInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        ViewBag.error = (Error.ViewBagError);
        //        return View(Error.ErrorPath);
        //    }

        //}
        //Author:Swetha M Date:02/09/2022
        [HttpPost]
        public async Task<IActionResult> SaveImportMachineryDetails()
        {

            try
            {
                if (_sessionManager.GetAllImportMachineryList().Count != 0)
                {
                    var allimportMachineryList = _sessionManager.GetAllImportMachineryList();

                    foreach (var item in allimportMachineryList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _inspectionOfUnitService.DeleteImportMachineryDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedImportMachineryDelete);
                                break;
                            case (int)Constant.Update:
                                await _inspectionOfUnitService.UpdateImportMachineryDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedImportMachineryUpdate);
                                break;
                            case (int)Constant.Create:
                                await _inspectionOfUnitService.CreateImportMachineryDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedImportMachineryCreate);
                                break;
                            default:
                                break;
                        }

                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveBuildingInspectionDetails);
                    ViewBag.AccountNumber = allimportMachineryList.First().LoanAcc;
                    return Json(new { isValid = true });
                }
                return Ok();
            }

            catch (Exception ex)
            {
                _logger.Error(Error.SaveImportMachineryInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
        //Author: Manoj Date:25/08/2022
        [HttpPost]
        public async Task<IActionResult> SaveIndigenousMachineryInspectionDetails()
        {
            try
            {
                if (_sessionManager.GetAllIndigenousMachineryInspectionList().Count != 0)
                {
                    _logger.Information(CommonLogHelpers.StratedSaveIndigenousMachineryInspectionDetails);

                    var indigenousMachineryInspectionList = _sessionManager.GetAllIndigenousMachineryInspectionList();

                    foreach (var item in indigenousMachineryInspectionList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _inspectionOfUnitService.DeleteIndigenousMachineryInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedIndigenousMachineryInspectionDelete);
                                break;
                            case (int)Constant.Update:
                                await _inspectionOfUnitService.UpdateIndigenousMachineryInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedIndigenousMachineryInspectionUpdate);
                                break;
                            case (int)Constant.Create:
                                await _inspectionOfUnitService.CreateIndigenousMachineryInspectionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedIndigenousMachineryInspectionCreate);
                                break;
                            default:
                                break;
                        }
                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveLandInspectionDetails);

                    return Json(new { isValid = true });
                }
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveIndigenousMachineryInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
        //Author:Swetha M Date:02/09/2022
        //[HttpPost]
        //public async Task<IActionResult> SaveMeansOfFinanceDetails()
        //{

        //    try
        //    {
        //        if (_sessionManager.GetAllMeansOfFinanceList().Count != 0)
        //        {
        //            var allMeansOfFinanceList = _sessionManager.GetAllMeansOfFinanceList();

        //            foreach (var item in allMeansOfFinanceList)
        //            {
        //                switch (item.Action)
        //                {
        //                    case (int)Constant.Delete:
        //                        await _inspectionOfUnitService.DeleteMeansOfFinanceDetails(item);
        //                        _logger.Information(CommonLogHelpers.CompletedMeansOfFinanceDelete);
        //                        break;
        //                    case (int)Constant.Update:
        //                        await _inspectionOfUnitService.UpdateMeansOfFinanceDetails(item);
        //                        _logger.Information(CommonLogHelpers.CompletedMeansOfFinanceUpdate);
        //                        break;
        //                    case (int)Constant.Create:
        //                        await _inspectionOfUnitService.CreateMeansOfFinanceDetails(item);
        //                        _logger.Information(CommonLogHelpers.CompletedMeansOfFinanceCreate);
        //                        break;
        //                    default:
        //                        break;
        //                }

        //            }
        //            _logger.Information(CommonLogHelpers.CompletedSaveBuildingInspectionDetails);
        //            ViewBag.AccountNumber = allMeansOfFinanceList.First().LoanAcc;
        //            return Json(new { isValid = true });
        //        }
        //        return Ok();
        //    }

        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.SaveMeansOfFinance + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        ViewBag.error = (Error.ViewBagError);
        //        return View(Error.ErrorPath);
        //    }
        //}

        //Author:Manoj Date:05/09/2022
        [HttpPost]
        public async Task<IActionResult> SaveLetterOfCreditDetail()
        {

            try
            {
                if (_sessionManager.GetAllLetterOfCreditDetail().Count != 0)
                {
                    var allLetterOfCreditList = _sessionManager.GetAllLetterOfCreditDetail();

                    foreach (var item in allLetterOfCreditList)
                    {
                        switch (item.Action)
                        {
                            case (int)Constant.Delete:
                                await _inspectionOfUnitService.DeleteLetterOfCreditDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLetterOfCreditDelete);
                                break;
                            case (int)Constant.Update:
                                await _inspectionOfUnitService.UpdateLetterOfCreditDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLetterOfCreditUpdate);
                                break;
                            case (int)Constant.Create:
                                await _inspectionOfUnitService.CreateLetterOfCreditDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLetterOfCreditCreate);
                                break;
                            default:
                                break;
                        }

                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveBuildingInspectionDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }

            catch (Exception ex)
            {
                _logger.Error(Error.SaveLetterOfCredit + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveRecommendedDisbursementDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveRecommendedDisbursementDetails);

                if (_sessionManager.GetAllRecommDisbursementDetail().Count != 0)
                {
                    var UpdatedList = _sessionManager.GetAllRecommDisbursementDetail();
                    foreach (var item in UpdatedList)
                    {
                        switch (item.Action)
                        {

                            case 2:
                                await _inspectionOfUnitService.UpdateRecomDisbursementDetail(item);
                                break;
                            default:
                                break;
                        }

                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveRecommendedDisbursementDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveLandAcquisitionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveStatusOfImplementaionDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveStatusofImplementationDetails);

                if (_sessionManager.GetAllStatusOfImplementation().Count != 0)
                {
                    var UpdatedList = _sessionManager.GetAllStatusOfImplementation();
                    foreach (var item in UpdatedList)
                    {
                        switch (item.Action)
                        {

                            case (int)Constant.Create:
                                await _inspectionOfUnitService.CreateStatusofImplementation(item);
                                _logger.Information(CommonLogHelpers.StartedStatusofImplementationCreate);
                                break;
                            case (int)Constant.Update:
                                await _inspectionOfUnitService.UpdateStatusofImplementation(item);
                                _logger.Information(CommonLogHelpers.CompletedStatusofImplementationUpdate);
                                break;
                            case (int)Constant.Delete:
                                await _inspectionOfUnitService.DeleteStatusofImplementation(item);
                                _logger.Information(CommonLogHelpers.CompletedStatusofImplementationDelete);
                                break;
                            default:
                                break;
                            
                        }

                    }

                    _logger.Information(CommonLogHelpers.CompletedSaveRecommendedDisbursementDetails);

                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveLandAcquisitionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }


        ////Author: Akhila Date:06/09/2022
        //[HttpPost]
        //public async Task<IActionResult> SaveProjectCostDetails()
        //{
        //    try
        //    {
        //        _logger.Information(CommonLogHelpers.StratedSaveProjectCostDetails);

        //        var projectCostList = _sessionManager.GetAllProjectCostList();

        //        foreach (var item in projectCostList)
        //        {

        //            switch (item.Action)
        //            {
        //                case (int)Constant.Delete:
        //                    await _inspectionOfUnitService.DeleteProjectCostDetails(item);
        //                    _logger.Information(CommonLogHelpers.CompletedProjectCostDelete);
        //                    break;
        //                case (int)Constant.Update:
        //                    await _inspectionOfUnitService.UpdateProjectCostDetails(item);
        //                    _logger.Information(CommonLogHelpers.CompletedProjectCostUpdate);
        //                    break;
        //                case (int)Constant.Create:
        //                    await _inspectionOfUnitService.CreateProjectCostDetails(item);
        //                    _logger.Information(CommonLogHelpers.CompletedProjectCostCreate);
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //        _logger.Information(CommonLogHelpers.CompletedSaveProjectCostDetails);

        //        return Json(new { isValid = true });

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(Error.SaveProjectCostError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
        //        ViewBag.error = (Error.ViewBagError);
        //        return View(Error.ErrorPath);
        //    }
        //}

    }
}
