using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.CreationOfSecurityandAquisitionAsset.LandAcquisition
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class LandAcquisitionController : Controller
    {
        private readonly IInspectionOfUnitService _inspectionOfUnitService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;        

        public LandAcquisitionController(IInspectionOfUnitService inspectionOfUnitService, ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _inspectionOfUnitService = inspectionOfUnitService;
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
                var AllLandAcquisitionList = _sessionManager.GetAllLandAcquisitionList();
                TblIdmIrLandDTO LandAcquisitionList = AllLandAcquisitionList.FirstOrDefault(x => x.UniqueId == id);

                var AllLandType = _sessionManager.GetDDListLandType();
                ViewBag.LandType = AllLandType;

                var AllLandInspectionList = _sessionManager.GetAllLandInspectionList();
                var lastdataallLandInspectionList = AllLandInspectionList.Last();
                {
                    ViewBag.Inspection = lastdataallLandInspectionList.DcLndSecCreated;
                }
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.LandAcqresultViewPath + Constants.ViewRecord, LandAcquisitionList);
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
                var AllLandAcquisitionList = _sessionManager.GetAllLandAcquisitionList();
                var AccountNumber = AllLandAcquisitionList.First().LoanAcc;
               
                TblIdmIrLandDTO LandAcquisitionList = AllLandAcquisitionList.FirstOrDefault(x => x.UniqueId == id);

                var AllLandInspectionList = _sessionManager.GetAllLandInspectionList();
                var lastdataallLandInspectionList = AllLandInspectionList.Last();
                {
                    ViewBag.Inspection = lastdataallLandInspectionList.DcLndSecCreated;
                }
               

                var AllLandType = _sessionManager.GetDDListLandType();
                ViewBag.LandType = AllLandType;

                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                ViewBag.LoanAcc = AccountNumber;
                return View(Constants.LandAcqresultViewPath + Constants.editCs, LandAcquisitionList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, TblIdmIrLandDTO landAcq)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStartedPost);
                List<TblIdmIrLandDTO> editlandacq = new();
                if (_sessionManager.GetAllLandAcquisitionList() != null)
                    editlandacq = _sessionManager.GetAllLandAcquisitionList();

                TblIdmIrLandDTO landExist = editlandacq.Find(x => x.UniqueId == id);
                if (landExist != null)
                {
                    editlandacq.Remove(landExist);
                    var list = landExist;
                    list.LoanAcc = landAcq.LoanAcc;
                    list.LoanSub = landAcq.LoanSub;
                    list.OffcCd = landAcq.OffcCd;
                    list.IrlArea= landAcq.IrlArea;  
                    list.IrlAreaIn = landAcq.IrlAreaIn;
                    list.IrlLandTy = landAcq.IrlLandTy;
                    list.IrlLandCost = landAcq.IrlLandCost;
                    list.IrlDevCost = landAcq.IrlDevCost;
                    list.IrlSecValue = landAcq.IrlSecValue;
                    list.IrlRelStat = landAcq.IrlRelStat;                    
                    list.UniqueId = landAcq.UniqueId;
                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (landExist.IrlId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    editlandacq.Add(landExist);
                    _sessionManager.SetLandAcquisitionList(editlandacq);

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    List<TblIdmIrLandDTO> activelandAcquisitionList = editlandacq.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    _logger.Information(CommonLogHelpers.UpdateCompletedPost);
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.LandAcqviewPath + Constants.ViewAll, activelandAcquisitionList) });
                }
                ViewBag.AccountNumber = landAcq.LoanAcc;
                ViewBag.LoanSub = landAcq.LoanSub;
                ViewBag.OffcCd = landAcq.OffcCd;
                _logger.Information(CommonLogHelpers.Failed);
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.LandAcqviewPath + Constants.Edit, landAcq) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}