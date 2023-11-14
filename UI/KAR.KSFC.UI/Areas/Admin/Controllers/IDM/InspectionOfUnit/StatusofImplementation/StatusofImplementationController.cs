using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
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

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.InspectionOfUnit.StatusofImplementation
{


    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class StatusofImplementationController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        public StatusofImplementationController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var allStatusofImplementationList  = _sessionManager.GetAllStatusOfImplementation();
                IdmDsbStatImpDTO  statusImpList  = allStatusofImplementationList.FirstOrDefault(x => x.UniqueId == id);
                if (statusImpList != null)
                {
                   

                }
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.StatusofImplementationViewPath + Constants.ViewRecord, statusImpList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult Create(long accountNumber, byte OffcCd, long InspectionId, string LoanSub, bool firstinspection)
        {
            try
            {
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.InspectionId = InspectionId;
                ViewBag.firststatImp = firstinspection;

                var StatusImplementaioin = _sessionManager.GetAllStatusOfImplementation();
                var inspdetails = _sessionManager.GetAllInspectionDetail();
                var forfirstinspection = inspdetails.FirstOrDefault().DinNo;

                if (StatusImplementaioin.Where(x => x.DsbIno == InspectionId).Count() > 0 || forfirstinspection == InspectionId)
                {
                    ViewBag.firststatImp = false;
                }
                var previousinspection = inspdetails
                       .OrderByDescending(x => x.DinNo)
                       .Skip(1)
                        .FirstOrDefault();
                if (previousinspection != null && previousinspection.DinNo == InspectionId)
                {
                    ViewBag.firststatImp = false;
                }

                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.StatusofImplementationViewPath + Constants.createCS);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        public IActionResult Create(IdmDsbStatImpDTO model)
        {
            Random rand = new Random(); // <-- Make this static somewhere
            const int maxValue = 9999;
            var number = Convert.ToInt64(rand.Next(maxValue + 1).ToString("D4"));
            try
            {
                //if (ModelState.IsValid)
                //{
                List<IdmDsbStatImpDTO> StatusofImplementationList = new();
                if (_sessionManager.GetAllStatusOfImplementation() != null)
                    StatusofImplementationList = _sessionManager.GetAllStatusOfImplementation();
                IdmDsbStatImpDTO list = new IdmDsbStatImpDTO();
                list.LoanAcc = model.LoanAcc;
                list.OffcCd = model.OffcCd;
                list.LoanSub = model.LoanSub;
                //list.DsbOffc = model.DsbOffc;
                list.Action = (int)Constant.Create;
                //list.DsbUnit = model.DsbUnit;
                //list.DsbSno = model.DsbSno;
                list.DsbIno = model.DsbIno;
                list.DsbImpStat = model.DsbImpStat;
                //list.DsbNamePl = model.DsbNamePl;
                list.DsbProgimpBldg = model.DsbProgimpBldg;
                list.DsbProgimpMc = model.DsbProgimpMc;
                list.DsbBldgVal = model.DsbBldgVal;
                list.DsbMcVal = model.DsbMcVal;
                //list.DsbPhyPrg = model.DsbPhyPrg;
                list.DsbValPrg = model.DsbValPrg;
                list.DsbTmcstOvr = model.DsbTmcstOvr;
                list.DsbRec = model.DsbRec;
                list.DsbComplDt = model.DsbComplDt;
                list.DsbBalBldg = model.DsbBalBldg;
                list.IsActive = true;
                list.CreatedDate = DateTime.Now;
                list.IsDeleted = false;
                list.UniqueId = Guid.NewGuid().ToString();
                StatusofImplementationList.Add(list);
                _sessionManager.SetStatusofImplementaionList(StatusofImplementationList);
                List<IdmDsbStatImpDTO> activeList = StatusofImplementationList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                ViewBag.AccountNumber = list.LoanAcc;
                ViewBag.LoanSub = list.LoanSub;
                ViewBag.OffcCd = list.OffcCd;
                ViewBag.firststatImp = true;
                ViewBag.InspectionId = model.DsbIno;
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.StatusImpPath + Constants.ViewAll, StatusofImplementationList) });
                //}
                //ViewBag.AccountNumber = model.LoanAcc;
                //ViewBag.InspectionId = model.DimcIno;
                //ViewBag.LoanSub = model.LoanSub;
                //ViewBag.OffcCd = model.OffcCd;
                //return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.ImportviewPath + Constants.Create, model) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult Edit(long InspectionId , string id = "")
        {
            try
            {
               
                var allStatusofImplementationList = _sessionManager.GetAllStatusOfImplementation();

                IdmDsbStatImpDTO statusImp  = allStatusofImplementationList.FirstOrDefault(x => x.UniqueId == id);
                if (statusImp != null)
                {
                  

                    var AccountNumber = statusImp.LoanAcc;
                    ViewBag.InspectionId = InspectionId;
                    ViewBag.AccountNumber = AccountNumber ;
                    ViewBag.LoanSub = statusImp.LoanSub;
                    ViewBag.OffcCd = statusImp.OffcCd;
                    
                }

                return View(Constants.StatusofImplementationViewPath + Constants.editCs, statusImp);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDsbStatImpDTO model)
        {
            try
            {

                List<IdmDsbStatImpDTO> statusImp = _sessionManager.GetAllStatusOfImplementation();
                IdmDsbStatImpDTO statusImpExist = statusImp.Find(x => x.UniqueId == id);
                //if (importMachineryExist != null)
                //{
                statusImp.Remove(statusImpExist);
                var list = statusImpExist;
                list.LoanAcc = model.LoanAcc;
                list.OffcCd = model.OffcCd;
                list.LoanSub = model.LoanSub;
                //list.DsbOffc = model.DsbOffc;
                list.Action = (int)Constant.Create;
                //list.DsbUnit = model.DsbUnit;
                //list.DsbSno = model.DsbSno;
                list.DsbIno = model.DsbIno;
                list.DsbImpStat = model.DsbImpStat;
                //list.DsbNamePl = model.DsbNamePl;
                list.DsbProgimpBldg = model.DsbProgimpBldg;
                list.DsbProgimpMc = model.DsbProgimpMc;
                list.DsbBldgVal = model.DsbBldgVal;
                list.DsbMcVal = model.DsbMcVal;
                //list.DsbPhyPrg = model.DsbPhyPrg;
                list.DsbValPrg = model.DsbValPrg;
                list.DsbTmcstOvr = model.DsbTmcstOvr;
                list.DsbRec = model.DsbRec;
                list.DsbComplDt = model.DsbComplDt;
                list.DsbBalBldg = model.DsbBalBldg;
                list.IsActive = true;
                list.CreatedDate = DateTime.Now;
                list.IsDeleted = false;
                list.UniqueId = Guid.NewGuid().ToString();
                if (statusImpExist.DsbId > 0)
                {
                    list.Action = (int)Constant.Update;
                }
                else
                {
                    list.Action = (int)Constant.Create;

                }

                statusImp.Add(list);
                ViewBag.AccountNumber = model.LoanAcc;
                ViewBag.LoanSub = list.LoanSub;
                ViewBag.OffcCd = list.OffcCd;
                ViewBag.InspectionId = model.DsbIno;
                ViewBag.firststatImp = true;

                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                _sessionManager.SetStatusofImplementaionList(statusImp);

                List<IdmDsbStatImpDTO> activeList = statusImp.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

                return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.StatusImpPath + Constants.ViewAll, statusImp) });
                //}
                //ViewBag.AccountNumber = model.LoanAcc;
                //ViewBag.InspectionId = model.DimcIno;
                //ViewBag.LoanSub = model.LoanSub;
                //ViewBag.OffcCd = model.OffcCd;
                //return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.ImportviewPath + Constants.Edit, model) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        public IActionResult Delete(long InspectionId, string id)
        {
            try
            {
                IEnumerable<IdmDsbStatImpDTO> activeStatusofImplementation  = new List<IdmDsbStatImpDTO>();
                var statusofImplementationList = _sessionManager.GetAllStatusOfImplementation();
                var itemToRemove = statusofImplementationList.Find(r => r.UniqueId == id);
                itemToRemove.IsActive = false;
                itemToRemove.IsDeleted = true;
                itemToRemove.Action = (int)Constant.Delete;
                statusofImplementationList.Add(itemToRemove);
                _sessionManager.SetStatusofImplementaionList(statusofImplementationList);
                if (statusofImplementationList.Count != 0)
                {
                    activeStatusofImplementation = statusofImplementationList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }

                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.InspectionId = itemToRemove.DsbIno;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InspectionId = InspectionId;
                ViewBag.firststatImp = true;
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.StatusImpPath + Constants.ViewAll, statusofImplementationList.Where(x=>x.IsDeleted==false).ToList()),delete = "Deleted" });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }





    }
}
