using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.CreationOfSecurityandAquisitionAsset.FurnitureAcquisition
{
    /*
     *  Author:Kiran Vasishta TS
     *  Date: 29-Sep-2022
     *  Module: Furniture Acquisition
     */
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class FurnitureAcquisitionController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;        
       
        public FurnitureAcquisitionController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }

        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var FurnitureAcquisitionList = _sessionManager.GetFurnitureAcquisitionList();
                TblIdmIrFurnDTO FurnitureAcqList = FurnitureAcquisitionList.FirstOrDefault(x => x.IrfId == id);
                var AllFurnitureInspectionList = _sessionManager.GetAllFurnitureInspectionList();
                var lastdataallFurnitureInspectionList = AllFurnitureInspectionList.Last();
                {
                    ViewBag.Furniture = lastdataallFurnitureInspectionList.FurnSec;
                }
                ViewBag.AllFurnitureList = FurnitureAcqList;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id); 
                return View(Constants.FurnitureAcqresultViewPath + Constants.ViewRecord, FurnitureAcqList);
            }
            catch (System.Exception ex)
            {   
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult Edit(int id = 0)
        {
            try
            {
                _logger.Information(CommonLogHelpers.UpdateStarted + id);
                var FurnAcqList = _sessionManager.GetFurnitureAcquisitionList();
                TblIdmIrFurnDTO FurnitureAcquisitionList = FurnAcqList.FirstOrDefault(x => x.IrfId == id);

                var AllFurnitureInspectionList = _sessionManager.GetAllFurnitureInspectionList();
                var lastdataallFurnitureInspectionList = AllFurnitureInspectionList.Last();
                {
                    ViewBag.Furniture = lastdataallFurnitureInspectionList.FurnSec;
                }

                _logger.Information(CommonLogHelpers.UpdateCompleted + id);    
                return View(Constants.FurnitureAcqresultViewPath + Constants.editCs, FurnitureAcquisitionList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TblIdmIrFurnDTO updateData, IFormCollection form)
        {
            try
            {
                 _logger.Information(string.Format(CommonLogHelpers.RegisterStartedPost + LogAttribute.FurnitureDto,
                    id, updateData.IrfItem));

                if (ModelState.IsValid)
                {
                    List<TblIdmIrFurnDTO> FurnitureUpdate = new();
                    List<TblIdmIrFurnDTO> activeFurnitureUpdate = new();
                    if (_sessionManager.GetFurnitureAcquisitionList() != null)
                        FurnitureUpdate = _sessionManager.GetFurnitureAcquisitionList();

                    TblIdmIrFurnDTO FurnitureUpdateExist = FurnitureUpdate.Find(x => x.IrfId == id);
                    long? acc = 0;
                    if (FurnitureUpdateExist != null)
                    {
                        FurnitureUpdate.Remove(FurnitureUpdateExist);
                        var newFurniture = FurnitureUpdateExist;
                        acc = FurnitureUpdateExist.LoanAcc;   
                        newFurniture.IrfItemDets = updateData.IrfItemDets;
                        newFurniture.IrfSupplier = updateData.IrfSupplier;
                        newFurniture.IrfSecAmt = updateData.IrfSecAmt;
                        newFurniture.IrfAamt = updateData.IrfAamt;
                        newFurniture.IrfAqrdStat = updateData.IrfAqrdStat;
                        newFurniture.IrfRelStat = updateData.IrfRelStat;
                        newFurniture.IrfTotalRelease = updateData.IrfTotalRelease;
                        newFurniture.Action = (int)Constant.Update;
                        FurnitureUpdate.Add(newFurniture);

                        _sessionManager.SetFurnitureAcquisitionList(FurnitureUpdate);
                        if (FurnitureUpdate.Count != 0)
                        {
                            activeFurnitureUpdate = (FurnitureUpdate.Where(x => x.IsDeleted == false && x.IsActive == true).ToList());
                        }
                        ViewBag.AccountNumber = newFurniture.LoanAcc;
                        ViewBag.LoanSub = newFurniture.LoanSub;
                        ViewBag.OffcCd = newFurniture.OffcCd;
                    }
                    _logger.Information(string.Format(CommonLogHelpers.RegisterCompletedPost + LogAttribute.FurnitureDto,id, updateData.IrfItem));
                    return Json(new { isValid = true, data = acc, html = Helper.RenderRazorViewToString(this, Constants.FurnitureAcqviewPath + Constants.ViewAll, activeFurnitureUpdate) });
                }
                ViewBag.AccountNumber = updateData.LoanAcc;
                ViewBag.LoanSub = updateData.LoanSub;
                ViewBag.OffcCd = updateData.OffcCd;
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.FurnitureDto,id, updateData.IrfItem));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.FurnitureAcqviewPath + Constants.Edit, updateData) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.RegisterErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    
        
    }
}