using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.InspectionOfUnit.InspectionDetail
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class InspectionDetailController : Controller
    {

        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
       

        public InspectionDetailController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create(long accountNumber,int LoanSub,byte OffcCd)
        {
            try
            {
                var allInpsectionDetail = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionAllInspectionDetail));
                _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                ViewBag.LoanAcc = accountNumber;
                ViewBag.LoanSub = LoanSub;           
                ViewBag.OffcCd =OffcCd;
                ViewBag.inpsectionDetail = allInpsectionDetail;
                ViewBag.AccountNumber=accountNumber;    
                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.inspectionDetailresultViewPath + Constants.createCS);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmDspInspDTO inspection)
        {
            try
            {
                
                List<IdmDspInspDTO> InspectionDetailList = new();
                if (_sessionManager.GetAllInspectionDetail() != null)
                    InspectionDetailList = _sessionManager.GetAllInspectionDetail();
                IdmDspInspDTO list = new IdmDspInspDTO();
                list.LoanAcc = inspection.LoanAcc;
                list.LoanSub=inspection.LoanSub;
                list.OffcCd = inspection.OffcCd;
                list.DinNo = inspection.DinNo;
                list.DinDt = inspection.DinDt;
                list.DinRdt = inspection.DinRdt;
                list.DinTeam = inspection.DinTeam;
                list.UniqueId = Guid.NewGuid().ToString();
                list.IsActive = true;
                list.IsDeleted = false;
                list.Action = (int)Constant.Create;
                InspectionDetailList.Add(list);
                ViewBag.AccountNumber = list.LoanAcc;
                ViewBag.LoanSub = list.LoanSub;
                ViewBag.OffcCd = list.OffcCd;
                _sessionManager.SetInspectionDetialList(InspectionDetailList);
                List<IdmDspInspDTO> activeList = InspectionDetailList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
               
                return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.inspectionDetailviewPath + Constants.ViewAll, activeList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult ViewRecord(string id = "")
        {
            List<IdmDspInspDTO> InspectionList = new();
            try
            {
               
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
               if (_sessionManager.GetAllInspectionDetail()!=null)
                {
                    InspectionList = _sessionManager.GetAllInspectionDetail();
                }
               else
                {
                    InspectionList = _sessionManager.GetAllInspectionList();
                }

                IdmDspInspDTO inspectionDetailList = InspectionList.FirstOrDefault(x => x.UniqueId == id);


                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == inspectionDetailList.DinRowID && x.SubModuleType == Constants.InspectionDetail && x.MainModule == Constants.InspectionOfUnit).ToList();
                ViewBag.Documentlist = doc;
               
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.inspectionDetailresultViewPath + Constants.ViewRecord, inspectionDetailList);

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
                var AllInspectionDetailList = _sessionManager.GetAllInspectionDetail();
                IdmDspInspDTO InspectionDetail = AllInspectionDetailList.FirstOrDefault(x => x.UniqueId == id);
                if(InspectionDetail!= null)
                {
                    var doclist = _sessionManager.GetIDMDocument();
                    var doc = doclist.Where(x => x.SubModuleId == InspectionDetail.DinRowID && x.SubModuleType == Constants.InspectionDetail && x.MainModule == Constants.InspectionOfUnit).ToList();
                    ViewBag.Documentlist = doc;
                    _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                    ViewBag.LoanAcc = InspectionDetail.LoanAcc;
                    ViewBag.LoanSub = InspectionDetail.LoanSub;
                    ViewBag.OffcCd = InspectionDetail.OffcCd;
                    ViewBag.SubModuleId = InspectionDetail.DinRowID;
                    ViewBag.SubModuleType = Constants.InspectionDetail;
                    ViewBag.MainModule = Constants.InspectionOfUnit;
                }
               
                return View(Constants.inspectionDetailresultViewPath + Constants.editCs, InspectionDetail);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDspInspDTO inspection)
        {
            try
            {
                List<IdmDspInspDTO> InpsectionDetails = new();
                if (_sessionManager.GetAllInspectionDetail() != null)
                    InpsectionDetails = _sessionManager.GetAllInspectionDetail();
                IdmDspInspDTO condtionExist = InpsectionDetails.Find(x => x.UniqueId == id);
                if (condtionExist != null)
                {

                    InpsectionDetails.Remove(condtionExist);
                    var list = condtionExist;
                    list.LoanAcc = inspection.LoanAcc;
                    list.DinNo = inspection.DinNo;
                    list.LoanSub = inspection.LoanSub;
                    list.OffcCd = inspection.OffcCd;
                    list.DinDt = inspection.DinDt;
                    list.DinRdt = inspection.DinRdt;
                    list.DinTeam = inspection.DinTeam;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.IsDeleted = false;
                    if (condtionExist.DinRowID > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;
                    }
                    InpsectionDetails.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    _sessionManager.SetInspectionDetialList(InpsectionDetails);

                    List<IdmDspInspDTO> activeList  = InpsectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    return Json(new { isValid = true, data = list.LoanAcc,html = Helper.RenderRazorViewToString(this, Constants.inspectionDetailviewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = inspection.LoanAcc;
                ViewBag.LoanSub = inspection.LoanSub;
                ViewBag.OffcCd = inspection.OffcCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.inspectionDetailviewPath + Constants.ViewAll, inspection) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                IEnumerable<IdmDspInspDTO> activeInsp = new List<IdmDspInspDTO>();
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, id));
                var InspectionDetail = JsonConvert.DeserializeObject<List<IdmDspInspDTO>>(HttpContext.Session.GetString(Constants.SessionAllInspectionDetail));
                var itemToRemove = InspectionDetail.Find(r => r.UniqueId == id);
                itemToRemove.Action = (int)Constant.Delete;
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;
                InspectionDetail.Add(itemToRemove);

                _sessionManager.SetInspectionDetialList(InspectionDetail);
                if (InspectionDetail.Count != 0)
                {
                    activeInsp = InspectionDetail.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.inspectionDetailviewPath + Constants.ViewAll, activeInsp) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public Task<bool> InpsectionNoValidation(string Inpsection)
        {
            try
            {

                var InpsectionDetails = _sessionManager.GetAllInspectionDetail();
                var isValid = InpsectionDetails.Find(x => x.DinNo.ToString() == Inpsection);


                if (isValid != null)
                {

                    return Task.FromResult(true);
                }
                else
                {
                    return Task.FromResult(false);

                }
            }
            catch (Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                throw;
            }

        }


    }
}
