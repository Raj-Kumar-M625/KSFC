using System.Web.Mvc;

namespace EpicCrmWebApi
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (HttpContext.User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }

                if (HttpContext.User.IsInRole("Developer"))
                {
                    return RedirectToAction("Index", "Developer");
                }

                if (HttpContext.User.IsInRole("Manager"))
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }

            return RedirectToAction("Login", "Account");
        }
    }
}