using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Logging.Client;

namespace KAR.KSFC.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger; 
        public HomeController(ILogger logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.Information("Home controller Index action invoked.");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.Information("Home controller Privacy action invoked.");
            return View();
        }

        public IActionResult Contactus()
        {
            _logger.Information("Home controller Contactus action invoked.");
            return View();
        }
        public IActionResult Error()
        {
            _logger.Information("Home controller Error action invoked.");
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
