using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using KAR.KSFC.UI.Security;
using Microsoft.AspNetCore.DataProtection;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.LoanAccounting
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class LoanAccountingController : Controller
    {
        private readonly SessionManager _sessionManager;
        private readonly IDataProtector protector;

        public LoanAccountingController(SessionManager sessionManager,IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
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
