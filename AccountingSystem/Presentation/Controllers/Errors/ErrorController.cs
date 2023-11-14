using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Presentation.Controllers.Error
{
    public class ErrorController:Controller
    {
        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Author: Rajesh V; Date: 05/05/2022
        /// Purpose: Handle Http Error Status Code and redirect to appropraite Error Page
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IActionResult HttpStatusCode(int code)
        {
            ViewBag.UserName = CurrentUser;
            if (code == 404)
            {
                return View("404Error");
            }
            else if (code == 500)
            {
                ViewBag.ErrorMessage = "My custom 500 error message.";
            }
            else
            {
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
            }

            ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ViewBag.ShowRequestId = !string.IsNullOrEmpty(ViewBag.RequestId);
            ViewBag.ErrorStatusCode = code;

            return View();
        }
    }
}
