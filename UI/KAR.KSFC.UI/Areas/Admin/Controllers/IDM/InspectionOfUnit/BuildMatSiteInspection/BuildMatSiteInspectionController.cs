using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.InspectionOfUnit.BuildMatSiteInspection
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class BuildMatSiteInspectionController : Controller
    {
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;


        public BuildMatSiteInspectionController(ILogger logger, SessionManager sessionManager)
        {

            _logger = logger;
            _sessionManager = sessionManager;
        }
        //Author Manoj on 29/08/2022
        [HttpGet]
        public IActionResult Create(long accountNumber, long InspectionId, byte OffcCd, string LoanSub, bool firstinspection)
        {
            try
            {
                var buildingMaterialDetails = _sessionManager.GetAllBuildMatSiteInspectionList();
                var UmoMasterList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDLMasterList"));
                if (buildingMaterialDetails != null)
                {
                    ViewBag.ItemNumber = buildingMaterialDetails.Select(x => x.IrbmItem).ToList();
                }
                _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                ViewBag.LoanAcc = accountNumber;
                ViewBag.InspectionId = InspectionId;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.UmoList = UmoMasterList;
                ViewBag.firstbuildingmaterialinspection = firstinspection;
                var materialInspectionDetails  = _sessionManager.GetAllBuildMatSiteInspectionList();
                var inspdetails = _sessionManager.GetAllInspectionDetail();
                var forfirstinspection  = inspdetails.FirstOrDefault().DinNo;

                //if (materialInspectionDetails.Count > 0 || isfirstinspection == InspectionId)
                //{
                //    ViewBag.firstbuildingmaterialinspection = false;
                //}
                if (materialInspectionDetails.Where(x => x.IrbmIno == InspectionId).Count() > 0 || forfirstinspection == InspectionId)
                {
                    ViewBag.firstbuildingmaterialinspection = false;
                }

                var previousinspection = inspdetails
                         .OrderByDescending(x => x.DinNo)
                         .Skip(1)
                          .FirstOrDefault();
                if (previousinspection != null && previousinspection.DinNo == InspectionId)
                {
                    ViewBag.firstbuildingmaterialinspection = false;
                }
                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.BuildingMatresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        //Author Manoj on 25/08/2022
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmBuildingMaterialSiteInspectionDTO buildMatSiteInspection)
        {
            try
            {
                Random rand = new Random(); // <-- Make this static somewhere
                const int maxValue = 9999;
                var number = Convert.ToInt64(rand.Next(maxValue + 1).ToString("D4"));

                if (ModelState.IsValid)
                {
                    List<IdmBuildingMaterialSiteInspectionDTO> buildMatSiteInspectionDetails = new();
                    if (_sessionManager.GetAllBuildMatSiteInspectionList() != null)
                        buildMatSiteInspectionDetails = _sessionManager.GetAllBuildMatSiteInspectionList();
                    IdmBuildingMaterialSiteInspectionDTO list = new IdmBuildingMaterialSiteInspectionDTO();
                    list.LoanAcc = buildMatSiteInspection.LoanAcc;
                    list.LoanSub = buildMatSiteInspection.LoanSub;
                    list.OffcCd = buildMatSiteInspection.OffcCd;
                    list.IrbmIdt = buildMatSiteInspection.IrbmIdt;
                    list.IrbmRdt = buildMatSiteInspection.IrbmRdt;
                    list.IrbmItem = (int)number;
                    list.IrbmMat = buildMatSiteInspection.IrbmMat;
                    list.IrbmQty = buildMatSiteInspection.IrbmQty;
                    list.IrbmRate = buildMatSiteInspection.IrbmRate;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IrbmNo = buildMatSiteInspection.IrbmNo;
                    list.IrbmQtyIn = buildMatSiteInspection.IrbmQtyIn;
                    list.IrbmAmt = buildMatSiteInspection.IrbmAmt;
                    list.UomId = buildMatSiteInspection.UomId;
                    list.IrbmTotalAmt = buildMatSiteInspection.IrbmTotalAmt;
                    var UmoMasterCode = _sessionManager.GetAllUmoMasterlist();
                    var umodesc = UmoMasterCode.Where(x => x.Value == list.UomId.ToString());
                    list.UmoDesc = umodesc.First().Text;
                    list.CreatedBy = buildMatSiteInspection.CreatedBy;
                    list.CreatedDate = buildMatSiteInspection.CreatedDate;
                    list.ModifiedBy = buildMatSiteInspection.ModifiedBy;
                    list.ModifiedDate = buildMatSiteInspection.ModifiedDate;
                    list.IrbmIno = buildMatSiteInspection.IrbmIno;
                    list.Action = (int)Constant.Create;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    buildMatSiteInspectionDetails.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.InspectionId = buildMatSiteInspection.IrbmIno;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.firstbuildingmaterialinspection = true;
                    var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                    ViewBag.inspectiondetails = inspectiondetailslist;
                    _sessionManager.SetBuildMatSiteInspectionList(buildMatSiteInspectionDetails);
                    List<IdmBuildingMaterialSiteInspectionDTO> activeList = buildMatSiteInspectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.BuildingMatviewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = buildMatSiteInspection.LoanAcc;
                ViewBag.InspectionId = buildMatSiteInspection.IrbmIno;
                ViewBag.LoanSub = buildMatSiteInspection.LoanSub;
                ViewBag.OffcCd = buildMatSiteInspection.OffcCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.BuildingMatviewPath + Constants.Create, buildMatSiteInspection) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        //Author Manoj on 30/08/2022
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllbuildMatSiteInspectionList = _sessionManager.GetAllBuildMatSiteInspectionList();
                IdmBuildingMaterialSiteInspectionDTO buildMatSiteInspectionList = AllbuildMatSiteInspectionList.FirstOrDefault(x => x.UniqueId == id);
                var UmoMasterList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDLMasterList"));
                ViewBag.UmoList = UmoMasterList;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.BuildingMatresultViewPath + Constants.ViewRecord, buildMatSiteInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        //Author Manoj on 25/08/2022
        public IActionResult Edit(long InspectionId ,string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllbuildMatSiteInspectionList = _sessionManager.GetAllBuildMatSiteInspectionList();
                if (AllbuildMatSiteInspectionList != null)
                {
                    ViewBag.ItemNumber = AllbuildMatSiteInspectionList.Select(x => x.IrbmItem).ToList();
                }
                IdmBuildingMaterialSiteInspectionDTO buildMatSiteInspectionList = AllbuildMatSiteInspectionList.FirstOrDefault(x => x.UniqueId == id);
                if (buildMatSiteInspectionList != null)
                {
                    //ViewBag.InspectionId = buildMatSiteInspectionList.IrbmIno;
                    ViewBag.LoanAcc = buildMatSiteInspectionList.LoanAcc;
                    ViewBag.LoanSub = buildMatSiteInspectionList.LoanSub;
                    ViewBag.OffcCd = buildMatSiteInspectionList.OffcCd;
                    ViewBag.InspectionId = InspectionId;
                }
                var UmoMasterList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDLMasterList"));
                ViewBag.UmoList = UmoMasterList;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.BuildingMatresultViewPath + Constants.editCs, buildMatSiteInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        //Author Manoj on 25/08/2022
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmBuildingMaterialSiteInspectionDTO buildMatSiteInspection)
        {
            try
            {
                List<IdmBuildingMaterialSiteInspectionDTO> buildMatSiteInspectionDetails = new();
                if (_sessionManager.GetAllBuildMatSiteInspectionList() != null)
                    buildMatSiteInspectionDetails = _sessionManager.GetAllBuildMatSiteInspectionList();
                IdmBuildingMaterialSiteInspectionDTO buildmaterialInspectionExist = buildMatSiteInspectionDetails.Find(x => x.UniqueId == id);
                if (buildmaterialInspectionExist != null)
                {

                    buildMatSiteInspectionDetails.Remove(buildmaterialInspectionExist);
                    var list = buildmaterialInspectionExist;
                    list.LoanAcc = buildMatSiteInspection.LoanAcc;
                    list.LoanSub = buildMatSiteInspection.LoanSub;
                    list.OffcCd = buildMatSiteInspection.OffcCd;
                    list.IrbmIdt = buildMatSiteInspection.IrbmIdt;
                    list.IrbmRdt = buildMatSiteInspection.IrbmRdt;
                    list.IrbmItem = buildMatSiteInspection.IrbmItem;
                    list.IrbmMat = buildMatSiteInspection.IrbmMat;
                    list.IrbmQty = buildMatSiteInspection.IrbmQty;
                    list.IrbmRate = buildMatSiteInspection.IrbmRate;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IrbmNo = buildMatSiteInspection.IrbmNo;
                    list.IrbmQtyIn = buildMatSiteInspection.IrbmQtyIn;
                    list.UomId = buildMatSiteInspection.UomId;
                    list.IrbmTotalAmt = buildMatSiteInspection.IrbmTotalAmt;
                    var UmoMasterCode = _sessionManager.GetAllUmoMasterlist();
                    var umodesc = UmoMasterCode.Where(x => x.Value == list.UomId.ToString());
                    list.UmoDesc = umodesc.First().Text;
                    list.CreatedBy = buildMatSiteInspection.CreatedBy;
                    list.CreatedDate = buildMatSiteInspection.CreatedDate;
                    list.ModifiedBy = buildMatSiteInspection.ModifiedBy;
                    list.ModifiedDate = buildMatSiteInspection.ModifiedDate;
                    list.IrbmIno = buildMatSiteInspection.IrbmIno;
                    list.IrbmAmt = buildMatSiteInspection.IrbmAmt;
                    if (buildmaterialInspectionExist.IrbmRowId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    buildMatSiteInspectionDetails.Add(list);

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.InspectionId = list.IrbmIno;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.firstbuildingmaterialinspection = true;
                    var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                    ViewBag.inspectiondetails = inspectiondetailslist;
                    _sessionManager.SetBuildMatSiteInspectionList(buildMatSiteInspectionDetails);
                    List<IdmBuildingMaterialSiteInspectionDTO> activeList = buildMatSiteInspectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.BuildingMatviewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.InspectionId = buildMatSiteInspection.IrbmIno;
                ViewBag.LoanSub = buildMatSiteInspection.LoanSub;
                ViewBag.OffcCd = buildMatSiteInspection.OffcCd;
                ViewBag.AccountNumber = buildMatSiteInspection.LoanAcc;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.BuildingMatviewPath + Constants.Edit, buildMatSiteInspection) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        //Author Manoj on 25/08/2022
        [HttpPost]
        public IActionResult Delete(long InspectionId, string id)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, id));
                IEnumerable<IdmBuildingMaterialSiteInspectionDTO> activeBuilMat = new List<IdmBuildingMaterialSiteInspectionDTO>();
                var buildMatSiteInspectionList = JsonConvert.DeserializeObject<List<IdmBuildingMaterialSiteInspectionDTO>>(HttpContext.Session.GetString(Constants.SessionAllBuildMatSiteInspectionList));
                var itemToRemove = buildMatSiteInspectionList.Find(r => r.UniqueId == id);
                itemToRemove.Action = (int)Constant.Delete;
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;
                buildMatSiteInspectionList.Add(itemToRemove);
                _sessionManager.SetBuildMatSiteInspectionList(buildMatSiteInspectionList);
                if (buildMatSiteInspectionList.Count != 0)
                {
                    activeBuilMat = buildMatSiteInspectionList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.InspectionId = itemToRemove.IrbmIno;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InspectionId = InspectionId;
                ViewBag.firstbuildingmaterialinspection = true;
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.BuildingMatviewPath + Constants.ViewAll, activeBuilMat),delete = "Deleted" });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
