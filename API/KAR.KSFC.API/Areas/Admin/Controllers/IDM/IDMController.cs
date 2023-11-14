using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using KAR.KSFC.API.Helpers;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM
{
    public class IdmController : BaseApiController
    {
        private readonly IIdmService _idmService;
     
        public IdmController(IIdmService idmService)
        {
            _idmService = idmService;
     
        }

        [HttpGet, Route(RouteName.GetAccountNumber)]
        public async Task<IActionResult> GetAccountNumber(string EmpId , CancellationToken token)
        {
            var list = await _idmService.GetAccountNumber(EmpId,token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(list, CommonLogHelpers.Success));
        }

    }
}
