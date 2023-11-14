using KAR.KSFC.Components.Common.Dto.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Customer.Controllers.Enquiry.UnitDetails
{
   [Area("Customer")]
    [Authorize(Roles = RolesEnum.Customer)]
    public class BankDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
