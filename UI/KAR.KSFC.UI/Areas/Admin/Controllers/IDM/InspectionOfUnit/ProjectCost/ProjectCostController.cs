using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin;
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

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.InspectionOfUnit.ProjectCost
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class ProjectCostController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
       

        public ProjectCostController( ILogger logger, SessionManager sessionManager)
        {
          
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View();

        }
        [HttpGet]
        public IActionResult Create(long accountNumber, byte OffcCd, string LoanSub)
        {
            try

            {
                var AllProjectCostComponents = _sessionManager.GetDDListProjectCostComponent();
                ViewBag.ProjectCostComponents = AllProjectCostComponents;
                ViewBag.LoanAcc = accountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                return View(Constants.projectCostresultViewPath + Constants.createCS);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmDchgProjectCostDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<IdmDchgProjectCostDTO> projectCostDetails = _sessionManager.GetAllProjectCostList();
                    IdmDchgProjectCostDTO list = new IdmDchgProjectCostDTO();
                    list.LoanAcc = model.LoanAcc;
                    list.OffcCd = model.OffcCd;
                    list.LoanSub = model.LoanSub;
                    
                    list.DcpcstCode = model.DcpcstCode;
                    var allprojectCostComponent = _sessionManager.GetDDListProjectCostComponent();
                    var projectCostComponent = allprojectCostComponent.Where(x => x.Value == list.DcpcstCode.ToString());
                    list.ProjectCost = projectCostComponent.First().Text;
                    list.DcpcAmount = model.DcpcAmount;
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.Action = (int)Constant.Create;
                    projectCostDetails.Add(list);
                    _sessionManager.SetProjectCostList(projectCostDetails);
                    List<IdmDchgProjectCostDTO> activeList = projectCostDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.projectCostviewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = model.LoanAcc;
                ViewBag.LoanSub = model.LoanSub;
                ViewBag.OffcCd = model.OffcCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.projectCostviewPath + Constants.Create, model) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                var AllProjectCostComponents = _sessionManager.GetDDListProjectCostComponent();
                ViewBag.ProjectCostComponents = AllProjectCostComponents;

                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var allProjectCostList = _sessionManager.GetAllProjectCostList();
                IdmDchgProjectCostDTO allProjectCost = allProjectCostList.FirstOrDefault(x => x.UniqueId == id);                
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.projectCostresultViewPath + Constants.ViewRecord, allProjectCost);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult Edit(string id = "")
        {
            try
            {
                
                //for DropDown
                var AllProjectCostComponents = _sessionManager.GetDDListProjectCostComponent();              
                ViewBag.ProjectCostComponents = AllProjectCostComponents;

                var allProjectCostList = _sessionManager.GetAllProjectCostList();

                IdmDchgProjectCostDTO projectCostList = allProjectCostList.FirstOrDefault(x => x.UniqueId == id);
                if(projectCostList != null)
                {
                    ViewBag.LoanAcc = projectCostList.LoanSub;
                    ViewBag.LoanSub = projectCostList.LoanSub;
                    ViewBag.OffcCd = projectCostList.OffcCd;
                    ViewBag.ID = projectCostList.Id;
                }

                return View(Constants.projectCostresultViewPath + Constants.editCs, projectCostList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDchgProjectCostDTO projectDet)
        {
            try
            {
                List<IdmDchgProjectCostDTO> projectCostDetails  = _sessionManager.GetAllProjectCostList();
                IdmDchgProjectCostDTO projectCostExist = projectCostDetails.Find(x => x.UniqueId == id);
                
                if (projectCostExist != null)
                {
                    
                    projectCostDetails.Remove(projectCostExist);
                    var list = projectCostExist;
                    list.LoanAcc = projectDet.LoanAcc;
                    list.LoanSub = projectDet.LoanSub;
                    list.OffcCd = projectDet.OffcCd;
                    list.Action = (int)Constant.Create;
                    list.DcpcstCode = projectDet.DcpcstCode;
                    var allprojectCostComponent = _sessionManager.GetDDListProjectCostComponent();
                    var projectCostComponent = allprojectCostComponent.Where(x => x.Value == list.DcpcstCode.ToString());
                    list.ProjectCost = projectCostComponent.First().Text;
                    list.DcpcAmount = projectDet.DcpcAmount;
                    list.UniqueId = projectDet.UniqueId;
                    list.ModifiedDate = DateTime.Now;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (projectCostExist.Id> 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    projectCostDetails.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;

                    _sessionManager.SetProjectCostList(projectCostDetails);
                    List<IdmDchgProjectCostDTO> activeList = projectCostDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.projectCostviewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = projectDet.LoanAcc;
                ViewBag.LoanSub = projectDet.LoanSub;
                ViewBag.OffcCd = projectDet.OffcCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.projectCostviewPath + Constants.Edit, projectDet) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        public IActionResult DeleteProjectCost(string id)
        {
            try
            {
                IEnumerable<IdmDchgProjectCostDTO> activeProjCostList = new List<IdmDchgProjectCostDTO>();
                //_logger.Information(string.Format(CommonLogHelpers.DeleteStarted, id));
                
                    var projectCostList = JsonConvert.DeserializeObject<List<IdmDchgProjectCostDTO>>(HttpContext.Session.GetString(Constants.SessionAllProjectCostList));
                    var itemToRemove = projectCostList.Find(r => r.UniqueId == id);
                    itemToRemove.IsActive = false;
                    itemToRemove.IsDeleted = true;
                    itemToRemove.Action = (int)Constant.Delete;
                    projectCostList.Add(itemToRemove);
                    _sessionManager.SetProjectCostList(projectCostList);
                    if (projectCostList.Count != 0)
                    {
                        activeProjCostList = projectCostList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    }
                    ViewBag.AccountNumber = itemToRemove.LoanAcc;
                     ViewBag.LoanSub = itemToRemove.LoanSub;
                     ViewBag.OffcCd = itemToRemove.OffcCd;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.projectCostviewPath + Constants.ViewAll, activeProjCostList) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }

}
