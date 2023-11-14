using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Security;
 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers
{
    public class HomeController : BaseApiController
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<HomeController> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountService"></param>
        /// <param name="logger"></param>
        public HomeController(IAccountService accountService,   ILogger<HomeController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// KSFC User logout.
        /// </summary>
       // [Authorize]
        [HttpPost, Route("AdminLogout")]
        public async Task AdminLogout(EmployeeLoginDTO info,CancellationToken token)
        {
           await _accountService.AdminLogout(info.EmpId,token).ConfigureAwait(false);
        }

        /// <summary>
        /// KSFC User password change.
        /// </summary>
        //[Authorize]
        [HttpPost, Route("ChangePassword")]
        public async Task<IActionResult> PasswordChange(PasswordChangeDTO info,CancellationToken token)
        {
            var encryOldPassword = SecurityHandler.Base64Encode(info.OldPassword);
            var encryNewPassword = SecurityHandler.Base64Encode(info.NewPassword);
            info.OldPassword = encryOldPassword;
            info.NewPassword = encryNewPassword;

            var user =await _accountService.EmpPasswordChange(info,token).ConfigureAwait(false);

            if (user == null)
            {
               return NotFound();
            }
            return Ok("Password changed Successfully.please login using new password.");
        }
    }
}
