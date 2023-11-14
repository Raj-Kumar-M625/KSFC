using Newtonsoft.Json;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Logging.Client;
using System.Collections.Generic;
using System;
using System.Linq;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using Microsoft.AspNetCore.Http;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using System.Threading.Tasks;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IDisbursementService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfSecurityandAquisitionAssetService;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;
using Microsoft.CodeAnalysis;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.CreationOfDisbursmentProposal.RecommendedDisbursement
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class RecommDisbursementController : Controller
    {
        private readonly IInspectionOfUnitService _inspectionOfUnitService;
        
        private readonly IDisbursementService _disbursementService;
        private readonly IUnitDetailsService _unitDetailsService;
        private readonly ICreationOfSecurityandAquisitionAssetService _creationOfSecurityandAquisitionAssetService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private const string resultViewPath = "~/Areas/Admin/Views/CreationOfDisbursmentProposal/RecommendedDisbursement/";
        private const string viewPath = "../../Areas/Admin/Views/CreationOfDisbursmentProposal/RecommendedDisbursement/";

        public RecommDisbursementController(IInspectionOfUnitService inspectionOfUnitService, ILogger logger, SessionManager sessionManager, IDisbursementService disbursementService , ICreationOfSecurityandAquisitionAssetService creationOfSecurityandAquisitionAssetService,IUnitDetailsService unitDetailsService)
        {
            _inspectionOfUnitService = inspectionOfUnitService;
            _logger = logger;
            _sessionManager = sessionManager;
            _disbursementService = disbursementService;
            _unitDetailsService = unitDetailsService;
            _creationOfSecurityandAquisitionAssetService = creationOfSecurityandAquisitionAssetService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
               
                var AllRecommDisbursementlist = _sessionManager.GetAllRecommDisbursementDetail();
                IdmDsbdetsDTO RecommDisbursementList = AllRecommDisbursementlist.FirstOrDefault(x => x.UniqueId == id);
                var AccountNumber = AllRecommDisbursementlist.First().LoanAcc;
                var InspectionId = 0;
                var allBuildingInspectionList = await _inspectionOfUnitService.GetAllBuildingnspectionList((long)AccountNumber, InspectionId);
                var allBuildingInspectionList1 = (!allBuildingInspectionList.Any()) ? 0 : allBuildingInspectionList.Last().DcBdgSecCreatd;

                var allLandInspectionList = await _inspectionOfUnitService.GetAllLandInspectionList((long)AccountNumber, InspectionId);
                var landInspection = (!allLandInspectionList.Any()) ? 0 : allLandInspectionList.Last().DcLndSecCreated;

                var allFurnitureInspectionList = await _inspectionOfUnitService.GetAllFurnitureInspectionList((long)AccountNumber, InspectionId);
                var furnitureinspection = (!allFurnitureInspectionList.Any()) ? 0 : allFurnitureInspectionList.Last().FurnSec;


                var allImportMachineryInspection = await _inspectionOfUnitService.GetAllImportMachineryList((long)AccountNumber, InspectionId);
                var allImportMachineryInspectionsec = (!allImportMachineryInspection.Any()) ? 0 : allImportMachineryInspection.Last().Dimcsec;


                var allIndigenousMachineryInspectionList = await _inspectionOfUnitService.GetAllIndigenousMachineryInspectionList((long)AccountNumber, InspectionId);
                var allIndigenousMachineryInspectionListsec = (!allIndigenousMachineryInspectionList.Any()) ? 0 : allIndigenousMachineryInspectionList.Last().SecurityCreated;
                ViewBag.total = allBuildingInspectionList1 + landInspection + furnitureinspection + allImportMachineryInspectionsec + allIndigenousMachineryInspectionListsec;

                var allSidbiApproval = await _disbursementService.GetAllSidbiApprovalDetails(AccountNumber);
                ViewBag.sancamount = (allSidbiApproval.Equals(0)) ? 0 : allSidbiApproval.LnSancAmt;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(resultViewPath + "ViewRecord.cshtml", RecommDisbursementList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public async Task<IActionResult>  Edit(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);
                var AllRecommDisbursementlist = _sessionManager.GetAllRecommDisbursementDetail();
                var AccountNumber = AllRecommDisbursementlist.First().LoanAcc;
                IdmDsbdetsDTO RecommDisbursementList = AllRecommDisbursementlist.FirstOrDefault(x => x.UniqueId == id);

                var InspectionId = 0;
                var allBuildingInspectionList = await _inspectionOfUnitService.GetAllBuildingnspectionList((long)AccountNumber, InspectionId);
                var allBuildingInspectionList1 = (!allBuildingInspectionList.Any()) ? 0 : allBuildingInspectionList.Last().DcBdgSecCreatd;
                ViewBag.Building = allBuildingInspectionList.Sum(x => x.DcBdgAtCost);
                var allBuildingAquisition = await _creationOfSecurityandAquisitionAssetService.GetAllBuildingAcquisitionDetails((long)AccountNumber, InspectionId);
                ViewBag.Build = allBuildingAquisition.Sum(x => x.IrbSecValue);

                var allLandInspectionList = await _inspectionOfUnitService.GetAllLandInspectionList((long)AccountNumber, InspectionId);
                var landInspection = (!allLandInspectionList.Any()) ? 0 : allLandInspectionList.Last().DcLndSecCreated;
                ViewBag.LandInspection = allLandInspectionList.Sum(x => x.DcLndAmt);
                var allLandAquisition = await _creationOfSecurityandAquisitionAssetService.GetAllCreationOfSecurityandAquisitionAssetList((long)AccountNumber, InspectionId);
                ViewBag.LandInsp = allLandAquisition.Sum(x => x.IrlSecValue);


                var allFurnitureInspectionList = await _inspectionOfUnitService.GetAllFurnitureInspectionList((long)AccountNumber, InspectionId);
                var furnitureinspection = (!allFurnitureInspectionList.Any()) ? 0 : allFurnitureInspectionList.Last().FurnSec;
                ViewBag.furniture = allFurnitureInspectionList.Sum(x => x.FurnActualCost);
                var furnitureaquistion = await _creationOfSecurityandAquisitionAssetService.GetFurnitureAcquisitionList((long)AccountNumber, InspectionId);
                ViewBag.furn = furnitureaquistion.Sum(x => x.IrfSecAmt);

                var allImportMachineryInspection = await _inspectionOfUnitService.GetAllImportMachineryList((long)AccountNumber, InspectionId);
                var allImportMachineryInspectionsec = (!allImportMachineryInspection.Any()) ? 0 : allImportMachineryInspection.Last().Dimcsec;
                ViewBag.import = allImportMachineryInspection.Sum(x => x.DimcActualCost);
                var importAquisiton = await _creationOfSecurityandAquisitionAssetService.GetAllMachineryAcquisitionDetails((long)AccountNumber, InspectionId);
                ViewBag.importInsp = importAquisiton.Sum(x => x.IrPlmcSecAmt);

                var allIndigenousMachineryInspectionList = await _inspectionOfUnitService.GetAllIndigenousMachineryInspectionList((long)AccountNumber, InspectionId);
                var allIndigenousMachineryInspectionListsec = (!allIndigenousMachineryInspectionList.Any()) ? 0 : allIndigenousMachineryInspectionList.Last().SecurityCreated;
                ViewBag.total = allBuildingInspectionList1+ landInspection + furnitureinspection+ allImportMachineryInspectionsec+ allIndigenousMachineryInspectionListsec;
                ViewBag.Indigenous = allIndigenousMachineryInspectionList.Sum(x => x.ActualCost);
                var Indigenous = await _creationOfSecurityandAquisitionAssetService.GetAllMachineryAcquisitionDetails((long)AccountNumber, InspectionId);
                ViewBag.Indinsp = Indigenous.Sum(x => x.IrPlmcSecAmt);

                var allSidbiApproval = await _disbursementService.GetAllSidbiApprovalDetails(AccountNumber);
                ViewBag.sancamount = (allSidbiApproval.Equals(0)) ? 0 : allSidbiApproval.LnSancAmt;

                var project = await _unitDetailsService.GetAllProjectCostDetailsList((long)AccountNumber);
                decimal buildAmount = 0;
                decimal landAmount = 0;
                decimal importAmount = 0;
                decimal indeAmount = 0;
                decimal furnAmount = 0;
                foreach (var obj in project)
                {
                    if(obj.DcpcstCode == 1 || obj.DcpcstCode == 2)
                    {
                        landAmount += (decimal)obj.DcpcAmount * 100000;
                    }
                    else if(obj.DcpcstCode == 3 || obj.DcpcstCode == 4 )
                    {
                        buildAmount += (decimal)obj.DcpcAmount * 100000;
                    }
                    else if (obj.DcpcstCode == 5 || obj.DcpcstCode == 6 || obj.DcpcstCode == 7 || obj.DcpcstCode == 8)
                    {
                        indeAmount += (decimal)obj.DcpcAmount * 100000;
                    }
                    else if (obj.DcpcstCode == 9 || obj.DcpcstCode == 10 || obj.DcpcstCode == 11 || obj.DcpcstCode == 12)
                    {
                        importAmount += (decimal)obj.DcpcAmount * 100000;
                    }
                    else if (obj.DcpcstCode == 13 || obj.DcpcstCode == 14)
                    {
                        furnAmount += (decimal)obj.DcpcAmount * 100000;
                    }
                }
                ViewBag.landcost = landAmount;
                ViewBag.Buildcost = buildAmount;
                ViewBag.importcost = importAmount;
                ViewBag.indegcost = indeAmount;
                ViewBag.furncost = furnAmount;


                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                ViewBag.LoanAcc = AccountNumber;
                return View(resultViewPath + "Edit.cshtml", RecommDisbursementList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDsbdetsDTO RecomdisburList)
        {
            try
            {
            
                List<IdmDsbdetsDTO> editRecomdisburse = new();

                if (_sessionManager.GetAllRecommDisbursementDetail() != null)
                    editRecomdisburse = _sessionManager.GetAllRecommDisbursementDetail();

                IdmDsbdetsDTO ExistList = editRecomdisburse.Find(x => x.UniqueId == id);
                if (ExistList != null)
                {
                    editRecomdisburse.Remove(ExistList);
                    var list = ExistList;
                    list.LoanAcc = RecomdisburList.LoanAcc;
                    list.UniqueId = RecomdisburList.UniqueId;
                    list.OffcCd = RecomdisburList.OffcCd;
                    list.LoanSub = RecomdisburList.LoanSub;
                   list.PropAmt = RecomdisburList.PropAmt;
                    list.DsbAmt = RecomdisburList.DsbAmt;
                    list.DsbAcd = RecomdisburList.DsbAcd;
                    list.DsbEstAmt = RecomdisburList.DsbEstAmt;
                    list.SecConsideredFRelease = RecomdisburList.SecConsideredFRelease;
                    list.SecInspection = RecomdisburList.SecInspection;
                    list.MarginRetained = RecomdisburList.MarginRetained;
                 
                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (ExistList.DsbdetsID > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;
                    }
                    editRecomdisburse.Add(list);
                    _sessionManager.SetRecommDisbursementList(editRecomdisburse);

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.offccd = list.OffcCd;
                    List<IdmDsbdetsDTO> activeList = editRecomdisburse.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();

                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, viewPath + "_ViewAll", activeList) });
                }

                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, viewPath + "Edit", RecomdisburList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}
