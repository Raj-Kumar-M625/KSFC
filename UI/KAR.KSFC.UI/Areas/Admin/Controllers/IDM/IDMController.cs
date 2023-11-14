using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Security;
using KAR.KSFC.UI.Services.IServices.Admin;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.ILegalDocumentationService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class IdmController : Controller
    {
        private readonly SessionManager _sessionManager;
        private readonly IDataProtector protector;
        public IdmController(SessionManager sessionManager, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _sessionManager = sessionManager;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.AdminRouteValue);
        }
        public IActionResult Index()
        {
            var loans = _sessionManager.GetAllLoanNumber()
                .Select(e =>
                {
                    e.EncryptedLoanAcc = protector.Protect(e.LoanAcc.ToString());
                    e.EncryptedLoanUnit = protector.Protect(e.LoanUnit);
                    e.EncryptedLoanSub = protector.Protect(e.LoanSub.ToString());
                    e.EncryptedInOffc = protector.Protect(e.InOffc.ToString());
                    return e;
                });
            return View(loans);
        }

    }
}
