using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccounting;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.LoanAccounting
{
    public class LoanAccountingController : BaseApiController
    {
        private readonly ILoanAccountingService _loanAccountingService;
        private readonly ILogger _logger;
        public LoanAccountingController(ILoanAccountingService loanAccountingService, ILogger logger)
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


        [HttpGet, Route(RouteName.GetCodetable)]
        public async Task<IActionResult> GetCodetable(CancellationToken token)
        {
            var list = await _loanAccountingService.GetCodetable(token).ConfigureAwait(false);
           // var res = list.Select(x => x.CodeType == "wewe");
            return Ok(new ApiResultResponse(list, CommonLogHelpers.Success));
        }
    }
}
