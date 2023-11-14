using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.Enquiry.ReviewAndSubmit
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    [SwitchModuleFilter(SwitchedModule = SwitchedModuleEnum.Admin)]
    public class ReviewAndSubmitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
