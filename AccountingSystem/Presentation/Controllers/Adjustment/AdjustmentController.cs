using Application.UserStories.Bill.Requests.Queries;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Vendor.Requests.Queries;
using AutoMapper;
using Common.ConstantVariables;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Presentation.Models.Master;
using Presentation.Models.Vendor;
using Presentation.Services.Infra.Cache;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Serilog;
using Application.UserStories.Payment.Requests.Queries;
using Domain.Payment;
using Omu.AwesomeMvc;
using Presentation.Extensions.Payment;
using Domain.Adjustment;
using Presentation.GridFilters.Adjustment;
using DocumentFormat.OpenXml.Office2010.Excel;
using SmartBreadcrumbs.Attributes;
using Microsoft.EntityFrameworkCore;
using Presentation.Models.Adjustment;
using Application.UserStories.Bill.Requests.Commands;
using DocumentFormat.OpenXml.Presentation;
using Application.DTOs.Adjustment;
using Application.UserStories.Adjustment.Requests.Commands;
using Application.UserStories.Adjustment.Requests.Queries;
using Application.UserStories.Payment.Requests.Commands;
using Microsoft.AspNetCore.Http;
using Presentation.Models.Bill;
using Presentation.Models;
using SmartBreadcrumbs.Nodes;
using Domain.Vendor;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Common.Helper;
using Presentation.Models.Payment;

namespace Presentation.Controllers.Adjustment
{
    public class AdjustmentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        protected bool Role => this.HttpContext.User.IsInRole("ADMINISTRATOR");
        public AdjustmentController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
       // [Breadcrumb(FromController = typeof(AdjustmentController))]
        public async Task<IActionResult> Index(int ID)
        {
            try
            {                 

                var childNode1 = new MvcBreadcrumbNode("Index", "Vendor", "Vendor List", false)
                {
                    //this comes in as a param into the action
                };

                var childNode2 = new MvcBreadcrumbNode("EditVendor", "Vendor", "Edit Vendor")
                {
                    RouteValues = new { ID },
                    OverwriteTitleOnExactMatch = true,
                    Parent = childNode1
                };

                var childNode3 = new MvcBreadcrumbNode("Index", "Adjustment", "Adjustment List")
                {
                    OverwriteTitleOnExactMatch = true,
                    Parent = childNode2
                };
                var paymentList = await _mediator.Send(new GetVenorQuerableValue());
                IQueryable<Vendors> paymentQuery = paymentList.Where(x => x.Id == ID).Distinct().AsQueryable();
                ViewBag.Name = paymentQuery.First().Name;
                ViewData["BreadcrumbNode"] = childNode3;
                ViewBag.UserName = CurrentUser;
                ViewBag.Id = ID;
                return View();
            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.indexM);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                throw;
            }
        }
        public async Task<ActionResult> GetAdjustmentList(int VendorId, GridParams gridParams, string[] forder, AdjustmentFilters adjustmentFilters)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Payment List
                var adjustmentList = await _mediator.Send(new GetAdjustmentListRequest { VendorId = VendorId });
                IQueryable<Adjustments> adjustmentQuery = adjustmentList.AsQueryable();


                var frow = new AdjustmentFilterRow();
                AdjustmentSearchFilter searchFilter = new AdjustmentSearchFilter();

                //Get Payment Searchfilters
                var filterBuilder = searchFilter.GetAdjustmentSearchCriteria(adjustmentQuery, adjustmentFilters);
                adjustmentQuery = await filterBuilder.ApplyAsync(adjustmentQuery, forder);

                var gmb = new GridModelBuilder<Adjustments>(adjustmentQuery.AsQueryable(), gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = MapToGridModel,
                    Tag = new { frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };              
                ViewBag.UserName = CurrentUser;
                return Json(await gmb.BuildAsync());
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getVendorPayments);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        private object MapToGridModel(Adjustments o)
        {
            try
            {

                return new
                {
                    o.ID,
                    AdjustmentReferenceNo = o.AdjustmentReferenceNo,
                    Date = o.Date.ToString("dd-MM-yyyy"),
                    VendorId = o.VendorID,                  
                    Amount = o.Amount.ToString("0.00"),
                    AdjustmentType = o.AdjustmentType,
                    BillPaymentRefNo = o.BillPaymentRefNo,
                    CreatedBy = o.CreatedBy,
                    Status = o.AdjustmentStatus.StatusMaster.CodeValue,
                    ApprovedBy = o.ApprovedBy == null ? "Not Approved" : o.ApprovedBy

                };
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.mapToGridModel);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }
        [HttpGet]
        //[Breadcrumb(ValueMapping.addAdjustment)]
  
        public async Task<ActionResult> AddAdjustment(int VendorId)
        {
            try
            {
                var childNode1 = new MvcBreadcrumbNode("Index", "Vendor", "Vendor List", false)
                {
                    //this comes in as a param into the action
                };

                var childNode2 = new MvcBreadcrumbNode("EditVendor", "Vendor", "Edit Vendor")
                {
                    RouteValues = new { id =VendorId },
                    OverwriteTitleOnExactMatch = true,
                    Parent = childNode1
                };

                var childNode3 = new MvcBreadcrumbNode("Index", "Adjustment", "Adjustment List")
                {
                    RouteValues = new { id = VendorId },
                    OverwriteTitleOnExactMatch = true,
                    Parent = childNode2
                };
                var childNode4 = new MvcBreadcrumbNode("Index", "Adjustment", "Adjustment Details")
                {

                    OverwriteTitleOnExactMatch = true,
                    Parent = childNode3
                };
                ViewData["BreadcrumbNode"] = childNode4;

                var vendorList = await _mediator.Send(new GetVendorListByIDRequest { ID = VendorId });
                var vendorDetails = _mapper.Map<VendorViewModel>(vendorList);
                ViewBag.VendorBalanceDetails = vendorDetails.VendorBalance;
                ViewBag.UserName = CurrentUser;
                ViewBag.VendorDetails = vendorDetails;
                ViewBag.VendorId = VendorId;
                return View();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.addBillM);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                _unitOfWork.Rollback();

                throw;
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddAdjustment(AdjustmentDto data)
        {
            try
            {
                //AdjustmentDto adjustments = _mapper.Map<AdjustmentDto>(data);
                var command = new AddAdjustmentCommadRequest { adjustmentDto = data, user = CurrentUser };
                var adjustmentId = await _mediator.Send(command);
                var command2 = new AddAdjustmentStatusCommandRequest { AdjustmentId = adjustmentId.ID};
                await _mediator.Send(command2);
                TempData["Message"] = PopUpServices.Notify(ValueMapping.adjustmentAdd, "Adjustment Ref No. " + adjustmentId.AdjustmentReferenceNo + "", notificationType: Alerts.success);
                return RedirectToAction("Index", new { Id = data.VendorID });
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.addBillM);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                _unitOfWork.Rollback();

                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult> AdjustmentDetails(int id, string? ModuleType)
        {
            try
            {
                
                
                var list = await _mediator.Send(new GetAdjustmentDetailsById { ID = id });
                var adjustmentDetails = _mapper.Map<AdjustmentViewModel>(list);
                if (ModuleType == null)
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "Vendor", "Vendor List", false)
                    {
                        //this comes in as a param into the action
                    };

                    var childNode2 = new MvcBreadcrumbNode("EditVendor", "Vendor", "Edit Vendor")
                    {
                        RouteValues = new { id = adjustmentDetails.VendorID },
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };

                    var childNode3 = new MvcBreadcrumbNode("Index", "Adjustment", "Adjustment List")
                    {
                        RouteValues = new { id = adjustmentDetails.VendorID },
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode2
                    };
                    var childNode4 = new MvcBreadcrumbNode("Index", "Adjustment", "Adjustment Details")
                    {

                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode3
                    };
                    ViewData["BreadcrumbNode"] = childNode4;

                }

                ViewBag.VendorBalanceDetails = adjustmentDetails.Vendor.VendorBalance;
                ViewBag.UserName = CurrentUser;
                ViewBag.VendorDetails = adjustmentDetails.Vendor;
                ViewBag.VendorId = adjustmentDetails.VendorID;
                ViewBag.UserName = CurrentUser;
                ViewBag.Role = HttpContext.Session.GetString("_role");
                return View(adjustmentDetails);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.billDetails);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }


        [HttpPost]       
        public async Task<ActionResult> AdjustmentDetails(AdjustmentViewModel data,string action)
        {
            try
            {
                //AdjustmentDto adjustments = _mapper.Map<AdjustmentDto>(data);
                var adjustmentDetails = _mapper.Map<AdjustmentDto>(data);
                var command = new UpdateAdjustmentCommand { AdjustmentDto = adjustmentDetails, user = CurrentUser, action = action };
                var adjustmentId = await _mediator.Send(command);
                if(action == "approve")
                {
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.adjustmentApprove, "Adjustment Ref No. " + adjustmentId.AdjustmentReferenceNo + "", notificationType: Alerts.success);
                }
                else if(action == "reject")
                {
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.adjustmentReject, "Adjustment Ref No. " + adjustmentId.AdjustmentReferenceNo + "", notificationType: Alerts.warning);
                }
                else
                {
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.adjustmentUpdate, "Adjustment Ref No. " + adjustmentId.AdjustmentReferenceNo + "", notificationType: Alerts.success);
                }
                
                return RedirectToAction("Index", new { Id = data.VendorID });
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.addBillM);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                _unitOfWork.Rollback();

                throw;
            }
        }
    }
}
