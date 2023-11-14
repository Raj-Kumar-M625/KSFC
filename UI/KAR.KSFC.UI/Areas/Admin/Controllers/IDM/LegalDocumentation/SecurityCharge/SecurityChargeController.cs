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
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.LegalDocumentation.SecurityCharge
{
    /// <summary>
    ///  Author: Gagana K; Module: SecurityCharge; Date:21/07/2022
    ///  //ModifieBy:Swetha M  Added Authorization 
    /// </summary>
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
   
    public class SecurityChargeController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;
       

        public SecurityChargeController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;

        }

        /// <summary>
        /// Author:Sandeep M:03/08/2022
        /// Purpose:To view the records of security charge details
        /// </summary>    
        public IActionResult ViewRecord(int id = 0)
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var allSecurityChargeList = _sessionManager.GetAllSecurityChargeList();
                IdmSecurityChargeDTO securityChargeList = allSecurityChargeList.FirstOrDefault(x => x.IdmDsbChargeId == id);

                var allSecurityType = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionDDListSecurityType));
                var bankIfscCodeList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionDDListBankIFSCCode));
                var allSecurityCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionDDListSecurityCategory));

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == id && x.SubModuleType == Constants.SecurityCharge && x.MainModule == Constants.LegalDocumentation).ToList();

                ViewBag.Documentlist = doc;
                ViewBag.MainModule = Constants.LegalDocumentation;
              
                ViewBag.bankifsc = bankIfscCodeList;
                ViewBag.SecurityCategory = allSecurityCategory;
                ViewBag.SecurityType = allSecurityType;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.securityresultViewPath + Constants.ViewRecord, securityChargeList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        /// <summary>
        /// Author:Sandeep M:10/08/2022
        /// Purpose:Edit the particular security charge details
        /// </summary>    
        public IActionResult Update(int id = 0)
        {
            try
            {

                var allSecurityChargeList = _sessionManager.GetAllSecurityChargeList();
                IdmSecurityChargeDTO securityChargeList = allSecurityChargeList.Find(x => x.IdmDsbChargeId == id);

                var allSecurityType = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionDDListSecurityType));
                var bankIfscCodeList = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionDDListBankIFSCCode));
                var allSecurityCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionDDListSecurityCategory));

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == id && x.SubModuleType == Constants.SecurityCharge && x.MainModule == Constants.LegalDocumentation).ToList();
               
                ViewBag.Documentlist = doc;
                _logger.Information(CommonLogHelpers.UpdateStarted + id);

                ViewBag.bank = bankIfscCodeList;
                ViewBag.SecurityCategory = allSecurityCategory;
                ViewBag.SecurityType = allSecurityType;

                ViewBag.SubModuleId = securityChargeList.IdmDsbChargeId;
                ViewBag.SubModuleType = Constants.SecurityCharge;
                ViewBag.MainModule = Constants.LegalDocumentation;
                _logger.Information(CommonLogHelpers.UpdateCompleted + id);
                return View(Constants.securityresultViewPath + Constants.editCs, securityChargeList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        /// <summary>
        /// Author:Sandeep M:11/08/2022
        /// Purpose:Update the particular security charge details in session
        /// </summary>    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, IdmSecurityChargeDTO holder)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.UpdateStartedPost + LogAttribute.SecurityChargeDto,
                 id, holder.ChargeTypeCd, holder.RequestLtrNo, holder.RequestLtrDate, holder.BankIfscCd, holder.BankRequestLtrNo,
                  holder.BankRequestLtrDate, holder.NocIssueBy, holder.NocIssueTo, holder.NocDate, holder.ChargeDetails,
                  holder.ChargeConditions, holder.AuthLetterBy, holder.BoardResolutionDate, holder.MoeInsuredDate, holder.ChargePurpose));

                if (ModelState.IsValid)
                {
                    List<IdmSecurityChargeDTO> securityHolder = new();
                    if (_sessionManager.GetAllSecurityChargeList() != null)
                        securityHolder = _sessionManager.GetAllSecurityChargeList();

                    IdmSecurityChargeDTO holderExist = securityHolder.Find(x => x.IdmDsbChargeId == id);
                    long? acc = 0;
                    if (holderExist != null)
                    {

                        securityHolder.Remove(holderExist);
                        var newHolder = holderExist;
                        acc = holderExist.LoanAcc;
                        newHolder.ChargeTypeCd = holder.ChargeTypeCd;
                        newHolder.RequestLtrNo = holder.RequestLtrNo;
                        newHolder.RequestLtrDate = holder.RequestLtrDate;
                        newHolder.BankIfscCd = holder.BankIfscCd;
                        newHolder.BankRequestLtrNo = holder.BankRequestLtrNo;
                        newHolder.BankRequestLtrDate = holder.BankRequestLtrDate;
                        newHolder.NocIssueBy = holder.NocIssueBy;
                        newHolder.NocIssueTo = holder.NocIssueTo;
                        newHolder.NocDate = holder.NocDate;
                        newHolder.BankIfscId = holder.BankIfscId;
                        newHolder.ChargeDetails = holder.ChargeDetails;
                        newHolder.ChargeConditions = holder.ChargeConditions;
                        newHolder.AuthLetterBy = holder.AuthLetterBy;
                        newHolder.AuthLetterDate = holder.AuthLetterDate;
                        newHolder.BoardResolutionDate = holder.BoardResolutionDate;
                        newHolder.MoeInsuredDate = holder.MoeInsuredDate;
                        newHolder.ChargePurpose = holder.ChargePurpose;
                        newHolder.Action = (int)Constant.Update;
                        securityHolder.Add(newHolder);
                        _sessionManager.SetSecurityChargeList(securityHolder);
                    }

                    _logger.Information(string.Format(CommonLogHelpers.UpdateCompletedPost + LogAttribute.SecurityChargeDto,
                    id, holder.ChargeTypeCd, holder.RequestLtrNo, holder.RequestLtrDate, holder.BankIfscCd, holder.BankRequestLtrNo,
                    holder.BankRequestLtrDate, holder.NocIssueBy, holder.NocIssueTo, holder.NocDate, holder.ChargeDetails,
                    holder.ChargeConditions, holder.AuthLetterBy, holder.BoardResolutionDate, holder.MoeInsuredDate, holder.ChargePurpose));
                    return Json(new { isValid = true, data = acc, html = Helper.RenderRazorViewToString(this, Constants.securityviewPath +  Constants.ViewAll, securityHolder) });
                }
                _logger.Information(string.Format(CommonLogHelpers.Failed + LogAttribute.SecurityChargeDto,
                  id, holder.ChargeTypeCd, holder.RequestLtrNo, holder.RequestLtrDate, holder.BankIfscCd, holder.BankRequestLtrNo,
                  holder.BankRequestLtrDate, holder.NocIssueBy, holder.NocIssueTo, holder.NocDate, holder.ChargeDetails,
                  holder.ChargeConditions, holder.AuthLetterBy, holder.BoardResolutionDate, holder.MoeInsuredDate, holder.ChargePurpose));
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.securityviewPath + Constants.Edit, holder) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        
    }
}
