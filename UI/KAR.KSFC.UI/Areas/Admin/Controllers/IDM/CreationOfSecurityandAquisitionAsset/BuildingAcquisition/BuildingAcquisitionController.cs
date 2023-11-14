using Newtonsoft.Json;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Logging.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ICreationOfSecurityandAquisitionAssetService;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.AspNetCore.Http;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.CreationOfSecurityandAquisitionAsset.BuildingAcquisition
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class BuildingAcquisitionController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;     

        public BuildingAcquisitionController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var ALLBuildingAcquisitionList = _sessionManager.GetAllBuildingAcquisitionDetail();
                TblIdmBuildingAcquisitionDetailsDTO BuildingAcquisitionList = ALLBuildingAcquisitionList.FirstOrDefault(x => x.UniqueId == id);
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.BuildingAcqresultViewPath + Constants.ViewRecord, BuildingAcquisitionList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult Edit(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);
                var AllBuildingAcquisitionDetailsDTO = _sessionManager.GetAllBuildingAcquisitionDetail();
                var AccountNumber = AllBuildingAcquisitionDetailsDTO.First().LoanAcc;
                TblIdmBuildingAcquisitionDetailsDTO BuildingAcquisitionList = AllBuildingAcquisitionDetailsDTO.FirstOrDefault(x => x.UniqueId == id);

                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                ViewBag.LoanAcc = AccountNumber;
                return View(Constants.BuildingAcqresultViewPath + Constants.editCs, BuildingAcquisitionList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, TblIdmBuildingAcquisitionDetailsDTO BuildingList)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStartedPost);
            
                List<TblIdmBuildingAcquisitionDetailsDTO> editbuildingacq = new();

                if (_sessionManager.GetAllBuildingAcquisitionDetail() != null)
                    editbuildingacq = _sessionManager.GetAllBuildingAcquisitionDetail();

                TblIdmBuildingAcquisitionDetailsDTO buildingAcqExist = editbuildingacq.Find(x => x.UniqueId == id);
                if (buildingAcqExist != null)
                {
                    editbuildingacq.Remove(buildingAcqExist);
                    var list = buildingAcqExist;
                    list.LoanAcc = BuildingList.LoanAcc;
                    list.UniqueId = BuildingList.UniqueId;
                    list.OffcCd = BuildingList.OffcCd;
                    list.LoanSub = BuildingList.LoanSub;
                    list.IrbIdt = BuildingList.IrbIdt;
                    list.IrbItem = BuildingList.IrbItem;
                    list.IrbArea = BuildingList.IrbArea;
                    list.IrbValue = BuildingList.IrbValue;
                    list.IrbNo = BuildingList.IrbNo;
                    list.IrbStatus = BuildingList.IrbStatus;
                    list.IrbSecValue = BuildingList.IrbSecValue;
                    list.IrbRelStat = BuildingList.IrbRelStat;
                    list.IrbAPArea = BuildingList.IrbAPArea;
                    list.IrbATCost = BuildingList.IrbATCost;
                    list.IrbPercentage = BuildingList.IrbPercentage;
                    list.IrbBldgConstStatus = BuildingList.IrbBldgConstStatus;
                    list.IrbBldgDetails = BuildingList.IrbBldgDetails;
                    list.IrbUnitCost = BuildingList.IrbUnitCost;
                    list.RoofType = BuildingList.RoofType;
                    list.IrbCost = BuildingList.IrbCost;
                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (buildingAcqExist.Irbid > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;
                    }
                    editbuildingacq.Add(list);
                    _sessionManager.SetBuildingAcquisitionList(editbuildingacq);
                    
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    List<TblIdmBuildingAcquisitionDetailsDTO> activebuildingAcquisitionList = editbuildingacq.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    _logger.Information(CommonLogHelpers.UpdateCompletedPost);
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.BuildingAcqviewPath + Constants.ViewAll, activebuildingAcquisitionList) });
                }
                ViewBag.AccountNumber = BuildingList.LoanAcc;
                ViewBag.LoanSub = BuildingList.LoanSub;
                ViewBag.OffcCd = BuildingList.OffcCd;
                _logger.Information(CommonLogHelpers.Failed);
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.BuildingAcqviewPath + Constants.Edit, BuildingList) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}