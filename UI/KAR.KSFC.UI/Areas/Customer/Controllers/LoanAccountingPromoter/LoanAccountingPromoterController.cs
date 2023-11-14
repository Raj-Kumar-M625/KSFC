using KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.LoanAccounting
{
    [Area("Customer")]
    public class LoanAccountingPromoterController : Controller
    {
        private readonly SessionManager _sessionManager;
        public LoanAccountingPromoterController(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            var loans = _sessionManager.GetAllAccountingLoanNumber();
            return View(loans);
        }
       
    }
}
