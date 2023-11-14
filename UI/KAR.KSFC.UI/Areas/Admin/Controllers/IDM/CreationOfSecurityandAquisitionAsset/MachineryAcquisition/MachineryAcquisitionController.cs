using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.CreationOfSecurityandAquisitionAsset.MachineryAcquisition
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class MachineryAcquisitionController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;        

        public MachineryAcquisitionController(ILogger logger, SessionManager sessionManager)
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
                var AllMachineryAcquisitionList = _sessionManager.GetAllMachineryAcquisitionList();
                IdmIrPlmcDTO MachineryAcquisitionList = AllMachineryAcquisitionList.FirstOrDefault (x => x.UniqueId == id);

                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.MachineAcqresultViewPath + Constants.ViewRecord, MachineryAcquisitionList);

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
                var AllMachineryAcquisitionList = _sessionManager.GetAllMachineryAcquisitionList();

                var AllIndigenousMachineryInspectionList = _sessionManager.GetAllIndigenousMachineryInspectionList();   
                var lastdataAllIndigenousMachineryInspectionList = AllIndigenousMachineryInspectionList.Last();
                {
                    ViewBag.Machinary = lastdataAllIndigenousMachineryInspectionList.SecurityCreated;
                }
                var AccountNumber = AllMachineryAcquisitionList.First().LoanAcc;
                IdmIrPlmcDTO MachineryAcquisitionList = AllMachineryAcquisitionList.FirstOrDefault(x => x.UniqueId == id);                

                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                ViewBag.LoanAcc = AccountNumber;
                return View(Constants.MachineAcqresultViewPath + Constants.editCs, MachineryAcquisitionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmIrPlmcDTO machineryAcq)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStartedPost);
                List<IdmIrPlmcDTO> editmachineryacq = new();
                if (_sessionManager.GetAllMachineryAcquisitionList() != null)
                    editmachineryacq = _sessionManager.GetAllMachineryAcquisitionList();

                IdmIrPlmcDTO machineryExist = editmachineryacq.Find(x => x.UniqueId == id);
                if (machineryExist != null)
                {

                    editmachineryacq.Remove(machineryExist);
                    var list = machineryExist;
                    list.LoanAcc = machineryAcq.LoanAcc;
                    list.LoanSub = machineryAcq.LoanSub;
                    list.OffcCd = machineryAcq.OffcCd;
                    list.IrPlmcAmt = machineryAcq.IrPlmcAmt;
                    list.IrPlmcTotalRelease = machineryAcq.IrPlmcTotalRelease;
                    list.IrPlmcSecAmt = machineryAcq.IrPlmcSecAmt;
                    list.IrPlmcAcqrdStatus = machineryAcq.IrPlmcAcqrdStatus;
                    list.IrPlmcRelseStat = machineryAcq.IrPlmcRelseStat;
                    list.IrPlmcAamt = machineryAcq.IrPlmcAamt;
                    list.UniqueId = machineryAcq.UniqueId;                   
                    list.IsActive = true;
                    list.IsDeleted = false;
                    
                    if (machineryExist.IrPlmcId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    editmachineryacq.Add(machineryExist);
                    _sessionManager.SetMachineryAcquisitionList(editmachineryacq);

                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    List<IdmIrPlmcDTO> activemachineryAcquisitionList = editmachineryacq.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    _logger.Information(CommonLogHelpers.UpdateCompletedPost);
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.MachineAcqviewPath + Constants.ViewAll, activemachineryAcquisitionList) });
                }
                ViewBag.AccountNumber = machineryAcq.LoanAcc;
                ViewBag.LoanSub = machineryAcq.LoanSub;
                ViewBag.OffcCd = machineryAcq.OffcCd;
                _logger.Information(CommonLogHelpers.Failed);
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.MachineAcqviewPath + Constants.Edit, machineryAcq) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}