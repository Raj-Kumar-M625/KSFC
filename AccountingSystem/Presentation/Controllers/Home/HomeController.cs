using Application.DTOs.Document;
using Application.DTOs.Profile;
using Application.UserStories.Document.Request.Command;
using Application.UserStories.HeaderProfile.Request.Commands;
using Application.UserStories.HeaderProfile.Request.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Common.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Omu.AwesomeMvc;
using Persistence;
using Presentation.Helpers;
using Presentation.Models;
using Presentation.Models.Error;
using Presentation.Models.Profile;
using Presentation.Services.Infra.Cache;
using Serilog;
using SmartBreadcrumbs.Attributes;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers.Home
{
    [Authorize]
    [SessionTimeoutHandlerAttribute]
    public class HomeController : Controller
    {
        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        public const string SessionKeyName1 = "HeaderImage1";
        public const string SessionKeyName2 = "HeaderImage2";


        public HomeController(ILogger<HomeController> logger, IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _logger = logger;
            _mediator = mediator;
                _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;


        }
        //public HomeController(ILogger<HomeController> logger,)
        //{
        //    _logger = logger;
           
        //}

        [DefaultBreadcrumb("Home")]
        public  IActionResult Index()
        {
            
            ViewBag.UserName = CurrentUser;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult UnderConstruction()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
         {
            ViewBag.UserName = CurrentUser;
            var exceptionHandler = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionMessage = exceptionHandler.Error.Message;
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
         }
        [HttpPost]
        public async Task<IActionResult> AddImages(HeaderProfileDetailsModel Images)
        {
            try
            {

                _unitOfWork.CreateTransaction();



                var Image = _mapper.Map<HeaderProfileDetailsDto>(Images);
                //var documentid = document.DocumentRefID;
             
                if (Image.Type== "Chief Minister")
                {
                    Image.EntityType = ValueMapping.CM;
                    
                    var command = new AddHeaderProfileCommand { HeaderProfileDetails = Image, entityPath = ValueMapping.DocEntityPath, entityType = ValueMapping.CM, user = CurrentUser, };
                    await _mediator.Send(command);
                }
                else
                {
                    Image.EntityType = ValueMapping.LabourO;
                    var command = new AddHeaderProfileCommand { HeaderProfileDetails = Image, entityPath = ValueMapping.DocEntityPath, entityType = ValueMapping.LabourO, user = CurrentUser, };
                    await _mediator.Send(command);
                }

                _unitOfWork.Save();
                _unitOfWork.Commit();
                var ImageList = await _mediator.Send(new GetImage());

                string Imagepath = ImageList.Where(o => o.EntityType == "Chief Minister").FirstOrDefault().ImagePath;
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

                ModelState.Clear();
                

                return this.Ok("Form Data received!");

            }
            catch (Exception)
            {
                Log.Error("Add  Image Details failed");
                _unitOfWork.Rollback();
            }

            return View("Index", "Home");

        }


    }
}
