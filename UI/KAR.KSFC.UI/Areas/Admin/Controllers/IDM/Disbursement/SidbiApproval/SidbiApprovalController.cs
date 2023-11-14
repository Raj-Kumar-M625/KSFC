using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.Disbursement.SidbiApproval
{     /// <summary>
      /// Author: Dev
      /// Date: 23/08/2022
      /// Module: Sidbi Approval
      /// </summary>
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class SidbiApprovalController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;


        public SidbiApprovalController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;
        }

        public IActionResult Edit(int id = 0)

        {
            try
            {
                IdmSidbiApprovalDTO Sidbi = new IdmSidbiApprovalDTO();

                _logger.Information(CommonLogHelpers.UpdateStarted + id);
                var allSidbiApproval = _sessionManager.GetAllSidbiApproval();
                var AccountNumber = allSidbiApproval.LoanAcc;
                if (allSidbiApproval.SidbiApprId == id)
                {
                    Sidbi = allSidbiApproval;
                }

                var doclist = _sessionManager.GetIDMDocument();
                var doc = doclist.Where(x => x.SubModuleId == Sidbi.SidbiApprId && x.SubModuleType == Constants.SidbiApproval && x.MainModule == Constants.DisbursementCondition).ToList();

                ViewBag.Documentlist = doc;
                ViewBag.SubModuleId = Convert.ToInt32(Sidbi.SidbiApprId);
                ViewBag.SubModuleType = Constants.SidbiApproval;
                ViewBag.MainModule = Constants.DisbursementCondition;
                var SidbiApproval = JsonConvert.DeserializeObject(HttpContext.Session.GetString("SessionSetSidbiApproval"));

                ViewBag.AccountNumber = AccountNumber;
                ViewBag.SidbiApproval = SidbiApproval;

                _logger.Information(CommonLogHelpers.UpdateCompleted + id);

                return View(Constants.SidbiresultViewPath + Constants.editCs, Sidbi);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}

