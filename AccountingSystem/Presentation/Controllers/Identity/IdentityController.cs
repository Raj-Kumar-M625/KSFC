using Application.Contracts.Identity;
using Application.Models.Identity;
using Application.UserStories.HeaderProfile.Request.Queries;
using Application.UserStories.Vendor.Requests.Queries;
using Common.ConstantVariables;
using Domain.User;
using Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Omu.AwesomeMvc;
using Persistence;
using Presentation.GridFilters.User;
using Presentation.Helpers;
using Presentation.Models.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Presentation.Controllers.Identity
{
    public class IdentityController:Controller
    {
        private readonly IAuthService _auth;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private readonly IMediator _mediator;
        public const string SessionKeyName = "_role";
        public const string SessionKeyName1 = "HeaderImage1";
        public const string SessionKeyName2 = "HeaderImage2";
        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        public IdentityController(IAuthService authService,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IMediator mediator, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _auth = authService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
            _unitOfWork = unitOfWork;

        }

        /// <summary>
        /// Purpose = Display login form
        /// Author = Sandeep 
        /// Date = May 05 2022
        /// </summary>

        [AllowAnonymous]
        public async Task<ActionResult> Index(string returnUrl = null)
        {
            _unitOfWork.CreateTransaction();

            var ImageList = await _mediator.Send(new GetImage());

            string Imagepath = ImageList.Where(o=>o.EntityType== "Chief Minister").FirstOrDefault().ImagePath;
            string secondImage = ImageList.Where(o => o.EntityType == "Labour Officer").FirstOrDefault().ImagePath;





            string wwwrootPath = string.Empty;
            int wwwrootIndex = Imagepath.IndexOf("images");
            int wwwrootIndex1 = secondImage.IndexOf("images");
            if (wwwrootIndex >= 0)
            {
                wwwrootPath = Imagepath.Substring(wwwrootIndex);
                string fullPath = "/" + wwwrootPath;
                HttpContext.Session.SetString(SessionKeyName1, fullPath);
            }
            if (wwwrootIndex1 >= 0)
            {
                wwwrootPath = secondImage.Substring(wwwrootIndex1);
                string fullPath = "/" + wwwrootPath;
                HttpContext.Session.SetString(SessionKeyName2, fullPath);
            }


            var model = new OtpRequestViewModel()
            {
                Email = null,
                Otp = null
            };
            ViewBag.OTP = null;
            
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }



        /// <summary>
        /// Purpose = Send OTP to the User by Email 
        /// Author = Sandeep 
        /// Date = May 05 2022
        /// </summary>

        [HttpPost]
      [AllowAnonymous]
        public async Task<IActionResult> SendOtp(OtpRequestViewModel input)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                var nulldata = new OtpRequestViewModel()
                {

                    Email = null,
                    Otp = null
                };
                ViewBag.Error = "User does not exist";
                return View("Index",nulldata);
            }
            var data = new OtpRequest()
            {
                Email = input.Email,
            };
            var result = await _auth.SendOtp(data);
            var model = new OtpRequestViewModel()
            {
                Email = data.Email,
                Otp = result.ToString()

            };
            ViewBag.result = result.OTP;
            return View("Index",model);
        }



        /// <summary>
        /// Purpose = Return to Login Page
        /// Author = Sandeep 
        /// Date = May 05 2022
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View("Index");
        }

        /// <summary>
        /// Purpose = Validate OTP and Login to the application
        /// Author = Sandeep 
        /// Date = May 05 2022
        /// </summary>



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthRequestViewModel input,string returnUrl = null)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(input.Email);
            var claims = new List<Claim>();
            var data = new AuthRequest()
            {
                Email = input.Email,
                OTP = input.OTP,
            };
            if (!await _auth.Login(data))
            {
                var model = new OtpRequestViewModel()
                {
                    Otp = input.OTP,
                    Email = input.Email
                };
                ViewBag.Error = "Incorrect Otp";
                return View("Index",model);
            }
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name,user.UserName));
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,new ClaimsPrincipal(identity));
            new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            var role = await _signInManager.UserManager.GetRolesAsync(user);
            var Role = role.FirstOrDefault();
            HttpContext.Session.SetString(SessionKeyName,Role);
            return RedirectToAction("Index","Home");
        }



        /// <summary>
        /// Purpose = Logout the User out of application
        /// Author = Sandeep 
        /// Date = May 05 2022 
        /// </summary>
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index","Identity");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// Purpose = ReSend OTP to the User by Email
        /// Author = Sandeep 
        /// Date = May 05 2022  
        /// </summary>
        public async Task<IActionResult> ResendOTP(string email)
        {
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var nulldata = new OtpRequestViewModel()
                    {

                        Email = null,
                        Otp = null
                    };
                    ViewBag.Error = "User does not exist";
                    return View("Index",nulldata);
                }
                var data = new OtpRequest()
                {
                    Email = email
                };
                var result = await _auth.SendOtp(data);
                var model = new OtpRequestViewModel()
                {
                    Email = data.Email,
                    Otp = result.ToString()
                };
                ViewBag.result = result.OTP;
                return View("Index",model);

            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }


        [SessionTimeoutHandlerAttribute]
        //  [Breadcrumb(ValueMapping.User)]
        public IActionResult UserList()
        {
            try
            {
                ViewBag.UserName = CurrentUser;
                return View();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.Inside,ValueMapping.IndexM);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }
        [SessionTimeoutHandlerAttribute]
        [HttpPost]
        public async Task<IActionResult> GetUserList(UserFilter Userfilters,GridParams gridParams,string[] forder)
        {
            try
            {

                forder = forder ?? new string[] { };

                var usersList = _auth.GetUsersLists();
                var vendorQuery = usersList.AsQueryable();
                // filter row search
                var frow = new UserFilterRow();

                var searchFilter = new UserSearchFilter();
                var filterBuilder = searchFilter.GetUserCriteria(usersList,Userfilters);

                vendorQuery = await filterBuilder.ApplyAsync(vendorQuery,forder);

                var gmb = new GridModelBuilder<UsersList>(vendorQuery,gridParams)
                {
                    KeyProp = o => o.Id,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };

                return Json(await gmb.BuildAsync());
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.Inside);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }

        private object MapToGridModel(UsersList o)
        {
            try
            {
                return new
                {
                    o.Id,
                    o.UserName,
                    o.EmailId,
                    o.MobileNumber,
                    o.Role
                };
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.mapToGridModel);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }

        [SessionTimeoutHandlerAttribute]
        [HttpGet]
        public IActionResult Edit(string id)
        {

            //return Ok();
            //var input = await _signInManager.UserManager.FindByIdAsync(id.ToString());

            UsersList objectUserlist;
            objectUserlist = new UsersList
            {
                EmailId = "Sandeep",
                MobileNumber = "7795979878",
                UserName = "Sandeep"
            };
            return PartialView("Edit",objectUserlist);
        }
    }
}
