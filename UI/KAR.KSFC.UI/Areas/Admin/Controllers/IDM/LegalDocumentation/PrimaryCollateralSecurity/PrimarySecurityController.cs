using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.LegalDocumentation.PrimaryCollateralSecurity
{
    //ModifieBy:Swetha M  Added Authorization 
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class PrimarySecurityController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
      

        public PrimarySecurityController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var allSecurityCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionsecurityCat));
                var allSecurityType = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionsecurityType));
                var allSubRegistrarOffice = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionSubRegister));

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == id && x.SubModuleType == Constants.PrimarySecurity && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.SecurityCategory = allSecurityCategory;
                ViewBag.SecurityType = allSecurityType;
                ViewBag.SubRegistrarOffice = allSubRegistrarOffice;

                var securityHolderList = _sessionManager.GetPrimaryCollateralList();
                IdmSecurityDetailsDTO holderList = securityHolderList.FirstOrDefault(x => x.IdmDeedDetId == id);

                ViewBag.Documentlist = doc;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.primaryresultViewPath + Constants.ViewRecord, holderList);

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
                var allSecurityCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionsecurityCat));
                var allSecurityType = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionsecurityType));
                var allSubRegistrarOffice = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.sessionSubRegister));

                ViewBag.SecurityCategory = allSecurityCategory;
                ViewBag.SecurityType = allSecurityType;
                ViewBag.SubRegistrarOffice = allSubRegistrarOffice;

                var securityHolderList = _sessionManager.GetPrimaryCollateralList();
                if (securityHolderList.Count != 0)
                {
                    ViewBag.ItemNumber = securityHolderList.Select(x => x.DeedNo).ToList();
                }

                IdmSecurityDetailsDTO holderList = securityHolderList.Find(x => x.IdmDeedDetId == id);

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == id && x.SubModuleType == Constants.PrimarySecurity && x.MainModule == Constants.LegalDocumentation).ToList();
              
                ViewBag.Documentlist = doc;
                ViewBag.SubModuleId = holderList.IdmDeedDetId;
                ViewBag.SubModuleType = Constants.PrimarySecurity;
                ViewBag.MainModule = Constants.LegalDocumentation;
                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.primaryresultViewPath + Constants.editCs, holderList);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IdmSecurityDetailsDTO holder, IFormCollection form)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.PrimarySecurityDto,
                    id, holder.TblSecurityRefnoMast.SecNameHolder, holder.TblSecurityRefnoMast.SecCd, holder.TblSecurityRefnoMast.PjsecCd, holder.TblSecurityRefnoMast.SecurityDetails, holder.DeedNo, holder.DeedDesc, holder.SubregistrarCd, holder.ExecutionDate, holder.TblSecurityRefnoMast.SecurityValue));

                if (ModelState.IsValid)
                {
                    IEnumerable<IdmSecurityDetailsDTO> activeHoldersList = new List<IdmSecurityDetailsDTO>();
                    List<IdmSecurityDetailsDTO> securityHolder = new();
                    if (_sessionManager.GetPrimaryCollateralList() != null)
                        securityHolder = _sessionManager.GetPrimaryCollateralList();

                    IdmSecurityDetailsDTO holderExist = securityHolder.Find(x => x.IdmDeedDetId == id);
                    long? acc = 0 ;
                    if (holderExist != null)
                    {
                        acc = holderExist.LoanAcc;
                        securityHolder.Remove(holderExist);
                        holderExist.DeedNo = holder.DeedNo;
                        holderExist.DeedDesc = holder.DeedDesc;
                        holderExist.SubregistrarCd = holder.SubregistrarCd;
                        holderExist.ExecutionDate = holder.ExecutionDate;
                        holderExist.SecCd = holder.SecCd;
                        holderExist.SecurityCd = holder.SecurityCd; 
                        holderExist.Action = (int)Constant.Update;
                        securityHolder.Add(holderExist);
                    }
                    _sessionManager.SetPrimaryCollateralList(securityHolder);
                    if (holderExist != null)
                    {
                        activeHoldersList = securityHolder.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    }
                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.PrimarySecurityDto,
                   id, holder.TblSecurityRefnoMast.SecNameHolder, holder.TblSecurityRefnoMast.SecCd, holder.TblSecurityRefnoMast.PjsecCd, holder.TblSecurityRefnoMast.SecurityDetails, holder.DeedNo, holder.DeedDesc, holder.SubregistrarCd, holder.ExecutionDate, holder.TblSecurityRefnoMast.SecurityValue,holder.SecCd,holder.SecurityCd));
                     return Json(new { isValid = true,data= acc, html = Helper.RenderRazorViewToString(this, Constants.primaryviewPath +  Constants.ViewAll, activeHoldersList) });
                }
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.PrimarySecurityDto,
                   id, holder.TblSecurityRefnoMast.SecNameHolder, holder.TblSecurityRefnoMast.SecCd, holder.TblSecurityRefnoMast.PjsecCd, holder.TblSecurityRefnoMast.SecurityDetails, holder.DeedNo, holder.DeedDesc, holder.SubregistrarCd, holder.ExecutionDate, holder.TblSecurityRefnoMast.SecurityValue,holder.SecCd, holder.SecurityCd));
                  return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.primaryviewPath + Constants.Edit, holder) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


    }
}
