using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.ReviewAndSubmit
{
   [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class ReviewAndSubmitController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
