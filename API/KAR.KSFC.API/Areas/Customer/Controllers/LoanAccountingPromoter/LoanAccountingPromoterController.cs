using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccountingPromoter;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.LoanAccountingPromoter
{
    public class LoanAccountingPromoterController : BaseApiController
    {
        private readonly ILoanAccountingPromoterService _loanAccountingService;
        private readonly ILogger _logger;
        public LoanAccountingPromoterController(ILoanAccountingPromoterService loanAccountingService, ILogger logger)
        {
            _loanAccountingService = loanAccountingService;
            _logger = logger;
        }

        [HttpGet, Route(RouteName.GetAccountNumber)]
        public async Task<IActionResult> GetAccountNumber(CancellationToken token, string EmpId)
        {
            var list = await _loanAccountingService.GetAccountNumber(token, EmpId).ConfigureAwait(false);
            return Ok(new ApiResultResponse(list, CommonLogHelpers.Success));
        }
    }
}
