using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Logging.Client;
using System.Threading.Tasks;
using System;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfSecurityandAquisitionAssetService;
using KAR.KSFC.UI.Helpers;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.UI.Security;
using Microsoft.AspNetCore.DataProtection;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.CreationOfSecurityandAquisitionAsset
{
    //ModifieBy:Swetha M  Added Authorization 
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class CreationOfSecurityandAquisitionAssetController : Controller
    {
        private readonly IIdmService _idmService;
        private readonly ICreationOfSecurityandAquisitionAssetService _creationOfSecurityandAquisitionAssetService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
        private readonly IInspectionOfUnitService _inspectionOfUnitService;
        private readonly ICommonService _commonService;
        private readonly IDataProtector protector;

        public CreationOfSecurityandAquisitionAssetController( ILogger logger, SessionManager sessionManager, ICommonService commonService, IInspectionOfUnitService inspectionOfUnitService, ICreationOfSecurityandAquisitionAssetService creationOfSecurityandAquisitionAssetService,
            IIdmService idmService, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _idmService = idmService;
            _logger = logger;
            _sessionManager = sessionManager;
            _inspectionOfUnitService = inspectionOfUnitService;
            _commonService = commonService;
            _creationOfSecurityandAquisitionAssetService = creationOfSecurityandAquisitionAssetService;
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

        public async Task<IActionResult> ViewInspectionList(string AccountNumber, string OffCd, string LoanSub, string UnitName, string loans, string MainModule)
        {
            
            try
            {
                long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
                byte offCd = Convert.ToByte(protector.Unprotect(OffCd));
                int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
                string unitname = protector.Unprotect(UnitName);
                var inspectionList = await _inspectionOfUnitService.GetAllInspectionDetailsList(accountNumber);
                foreach (var i in inspectionList)
                {
                    if (i.UniqueId == null)
                    {
                        i.UniqueId = Guid.NewGuid().ToString();
                        i.EncryptedLoanAcc = protector.Protect(i.LoanAcc.ToString());
                        i.EncryptedOffcCd = protector.Protect(i.OffcCd.ToString());
                        i.EncryptedLoanSub = protector.Protect(i.LoanSub.ToString());
                    }
                }
                var ldFileList = await _commonService.FileList(MainModule);
                _sessionManager.SetIDMDocument(ldFileList);
                
                _sessionManager.SetInspectionDetialList(inspectionList);

                    
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.loans = loans;
                ViewBag.OffCd = offCd;
                ViewBag.EncryptedUnitName = UnitName;
                return View(inspectionList);
            }
            catch(Exception ex)
            {
                _logger.Error(Error.SaveLandAcquisitionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
          
        }

        public async Task<IActionResult> ViewAccount(string AccountNumber, string OffCd, string LoanSub, string UnitName,long InspectionId, int loans , long inspectionID, string officialName)
        {
            try
            {
                long accountNumber = Convert.ToInt64(protector.Unprotect(AccountNumber));
                byte offCd = Convert.ToByte(protector.Unprotect(OffCd));
                int loansub = Convert.ToInt32(protector.Unprotect(LoanSub));
                string unitname = protector.Unprotect(UnitName);
                _logger.Information(Constants.GetAllIdmDropDownList);
                
                IdmDDLListDTO idmDTO = await _commonService.GetAllIdmDropDownList();
                _sessionManager.SetDDListLandType(idmDTO.AllLandType);
                _logger.Information(Constants.GetAllCreationOfSecurityandAquisitionAssetList);
                var landAcquisition = await _creationOfSecurityandAquisitionAssetService.GetAllCreationOfSecurityandAquisitionAssetList(accountNumber,InspectionId);
                foreach (var i in landAcquisition)
                {
                    i.UniqueId = Guid.NewGuid().ToString();
                }

                var AllLandType = await _idmService.GetAllLandType();
               
                foreach (var i in landAcquisition)
                {
                    var LandType = AllLandType.Where(x => x.Value == i.IrlLandTy);
                    i.LandTyDet = LandType.First().Text;
                }
                var landAcquisitionDetails = landAcquisition.Where(x => x.IrlIno == inspectionID).ToList();
                _sessionManager.SetLandAcquisitionList(landAcquisitionDetails);

                _logger.Information(Constants.GetAllMachineryAcquisitionDetails);
                var machineryAcquisition = await _creationOfSecurityandAquisitionAssetService.GetAllMachineryAcquisitionDetails(accountNumber, InspectionId);
                foreach (var i in machineryAcquisition)
                {
                    i.UniqueId = Guid.NewGuid().ToString();
                }
                var machineryAcquisitionDetails = machineryAcquisition.Where(x => x.IrPlmcIno == inspectionID).ToList();
                _sessionManager.SetMachineryAcquisitionList(machineryAcquisitionDetails);
                _logger.Information(Constants.GetAllBuildingAcquisitionDetails);
                var buildingAcquisition = await _creationOfSecurityandAquisitionAssetService.GetAllBuildingAcquisitionDetails(accountNumber,InspectionId);
                foreach (var i in buildingAcquisition)
                {
                    i.UniqueId = Guid.NewGuid().ToString();
                }

                _logger.Information(CommonLogHelpers.GetAllIndigenousMachineryInspectionList);
                var allIndigenousMachineryInspectionList = await _creationOfSecurityandAquisitionAssetService.GetAllIndigenousMachineryInspectionList(accountNumber, InspectionId);
                _sessionManager.SetIndigenousMachineryInspectionList(allIndigenousMachineryInspectionList);

                var buildingAcquisitionDetails = buildingAcquisition.Where(x => x.IrbIno == inspectionID).ToList();
                _sessionManager.SetBuildingAcquisitionList(buildingAcquisitionDetails);
                _logger.Information(Constants.GetFurnitureAcquisitionList);
                var AllFurnitureAcquisitionList = await _creationOfSecurityandAquisitionAssetService.GetFurnitureAcquisitionList(accountNumber,InspectionId);
                foreach (var i in AllFurnitureAcquisitionList)
                {
                    i.UniqueId = Guid.NewGuid().ToString();
                }
                var AllFurnitureAcquisitionListDetails = AllFurnitureAcquisitionList.Where(x => x.IrfIno == inspectionID).ToList();
                _sessionManager.SetFurnitureAcquisitionList(AllFurnitureAcquisitionListDetails);

                CreationSecurityAcqAssetDTO CSAA = new();
                CSAA.FurnitureAcqDetails = AllFurnitureAcquisitionListDetails;
                CSAA.MachineryAcqDetails = machineryAcquisitionDetails;
                CSAA.BuildingAcqDetails = buildingAcquisitionDetails;
                CSAA.LandAcqDetails = landAcquisitionDetails;
                CSAA.AllLandType = idmDTO.AllLandType;

                ViewBag.EncryptedAccountNumber = AccountNumber;
                ViewBag.EncryptedLoanSub = LoanSub;
                ViewBag.EncryptedUnitName = UnitName;
                ViewBag.EncryptedOffCd = OffCd;
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = loansub;
                ViewBag.UnitName = unitname;
                ViewBag.loans = loans;
                ViewBag.OffCd = offCd;
                ViewBag.inspectonID = inspectionID;
                ViewBag.officialName = officialName;
                _logger.Information(Constants.CompletedViewAccount);
                return View(CSAA);
            }
            catch (Exception ex)
            {
                _logger.Error(Error.ViewAccount + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);

                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> SaveLandAcquisitionDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveLandAcquisitionDetails);
                if (_sessionManager.GetAllLandAcquisitionList().Count != 0)
                {
                    var landAcquisitionList = _sessionManager.GetAllLandAcquisitionList();
                    foreach (var item in landAcquisitionList)
                    {
                        switch (item.Action)
                        {
                           
                            case (int)Constant.Update:
                                await _creationOfSecurityandAquisitionAssetService.UpdateLandAcquisitionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedLandAcqUpdate);
                                break;
                            default:
                                break;
                        }
                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveLandAcquisitionDetails);
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
        public async Task<IActionResult> SaveMachineryAcquisitionDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveMachineryAcquisitionDetails);
                if (_sessionManager.GetAllMachineryAcquisitionList().Count != 0)
                {
                    var machineryAcquisitionList = _sessionManager.GetAllMachineryAcquisitionList();
                    foreach (var item in machineryAcquisitionList)
                    {
                        switch (item.Action)
                        {
                            
                            case (int)Constant.Update:
                                await _creationOfSecurityandAquisitionAssetService.UpdateMachineryAcquisitionDetails(item);
                                _logger.Information(CommonLogHelpers.CompletedMachineryAcqUpdate);
                                break;
                            default:
                                break;
                        }
                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveMachineryAcquisitionDetails);
                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveMachineryAcquisitionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveBuildingAcquisitionDetails()
        {
            try
            {
                _logger.Information(CommonLogHelpers.StartedSaveBuildingAcquisitionDetails);
                if (_sessionManager.GetAllBuildingAcquisitionDetail().Count != 0)
                {
                    var buildingAcquisitionList = _sessionManager.GetAllBuildingAcquisitionDetail();
                    foreach (var item in buildingAcquisitionList)
                    {
                        switch (item.Action)
                        {
                            
                            case (int)Constant.Update:
                                await _creationOfSecurityandAquisitionAssetService.UpdateBuildingAcquisitionDetail(item);
                                _logger.Information(CommonLogHelpers.CompletedBuildingAcqUpdate);
                                break;
                            default:
                                break;
                        }
                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveBuildingAcquisitionDetails);
                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveBuildingAcquisitionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }

        public async Task<IActionResult> SaveFurnitureAcquisition()
        {
            try
            {
                 _logger.Information(CommonLogHelpers.StartedSaveFurnitureAcquisition);
                if (_sessionManager.GetFurnitureAcquisitionList().Count != 0)
                {
                    var FurnitureAcqList = _sessionManager.GetFurnitureAcquisitionList();
                    foreach (var item in FurnitureAcqList)
                    {
                        switch (item.Action)
                        {
                            
                            case (int)Constant.Update:
                                await _creationOfSecurityandAquisitionAssetService.UpdateFurnitureAcquisition(item);
                                _logger.Information(CommonLogHelpers.CompletedFurnitureAcqUpdate);
                                break;
                            default:
                                break;
                        }
                    }
                    _logger.Information(CommonLogHelpers.CompletedSaveFurnitureAcquisition);
                    return Json(new { isValid = true });
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(Error.SaveFurnitureAcquisitionError + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                ViewBag.error = (Error.ViewBagError);
                return View(Error.ErrorPath);
            }
        }
    }
}