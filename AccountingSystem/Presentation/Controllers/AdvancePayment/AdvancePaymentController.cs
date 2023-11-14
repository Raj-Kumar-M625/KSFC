using Application.UserStories.Vendor.Requests.Queries;
using AutoMapper;
using Common.ConstantVariables;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Domain.Payment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories.Generic;
using Persistence;
using Presentation.Models.Vendor;
using Presentation.Services.Infra.Cache;
using System;
using System.Threading.Tasks;
using Serilog;
using SmartBreadcrumbs.Attributes;
using Common.Helper;
using Application.DTOs.Payment;
using Domain.Vendor;
using Application.UserStories.Payment.Requests.Commands;
using Presentation.Models.Payment;
using SmartBreadcrumbs.Nodes;
using Application.UserStories.Payment.Requests.Queries;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Presentation.Controllers.AdvancePayment
{
    /// <summary>
    /// Author:Swetha M Date:12/05/2023
    /// Purpose:Advance PAyment Controller
    /// </summary>
    /// <returns></returns>
    public class AdvancePaymentController : Controller
    {
        /// <summary>
        /// Author:Swetha M Date:12/05/2023
        /// Purpose:Returns View with Data  for Add Advance Payment Page
        /// </summary>
        /// <returns></returns>
        ///  

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private GenericRepository<Payments> repository;

        protected string CurrentUser => HttpContext.User.Identity.Name;

        //// <summary>
        /// Author:Swetha M Date:05/05/2022
        /// Purpose: Constructor
        /// </summary>
        /// <returns></returns>
        public AdvancePaymentController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            repository = new GenericRepository<Payments>(unitOfWork);
        }
        [Breadcrumb("PaymentList")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Payment");

        }

        [HttpGet]
        [Breadcrumb("Add Advance Payment")]
        public async Task<ActionResult> AddAdvancePayment(int vendorId)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Vendors List
                var vendorList = await _mediator.Send(new GetVendorListByIDRequest { ID = vendorId });
                var vendorDetails = _mapper.Map<VendorViewModel>(vendorList);
                if (vendorList.VendorBankAccounts == null)
                {
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.addBankAccount, ValueMapping.bankntf, notificationType: Alerts.warning);
                    return RedirectToAction("EditVendor", "Vendor", new { Id = vendorId });
                }
                var advancePayments = await _mediator.Send(new GetAdvancePaymentByVendorRequest { vendorID = vendorId });

                ViewBag.AdvancePayments = _mapper.Map<List<PaymentViewModel>>(advancePayments);


                ViewBag.VendorBalanceDetails = vendorDetails.VendorBalance;
                ViewBag.VendorDetails = vendorDetails;
                ViewBag.UserName = CurrentUser;
                ViewBag.Id = vendorId;
                _unitOfWork.Commit();
                _unitOfWork.Save();
                return View();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.addPayment);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }

        }
        [HttpPost]

        public async Task<ActionResult> AddAdvancePayment(PaymentViewModel advancePayment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CreateTransaction();
                    var payment = _mapper.Map<PaymentDto>(advancePayment);
                    var addadvancePaymentId = await _mediator.Send(new AddAdvancePaymentCommand { payment = payment, currentUser = CurrentUser });
                    if (addadvancePaymentId != "")
                    {
                        _unitOfWork.Commit();
                        _unitOfWork.Save();
                        TempData["Message"] = PopUpServices.Notify("Advance Payment Created Successfully", addadvancePaymentId, notificationType: Alerts.success);

                        return RedirectToAction("Index", "Payment");
                    }
                    TempData["Message"] = PopUpServices.Notify("An Error Occured While Adding Advance Payment", addadvancePaymentId, notificationType: Alerts.success);
                    return RedirectToAction("AddAdvancePayment");

                }

                return RedirectToAction("AddAdvancePayment");
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.addPayment);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }

        }

        [HttpPost]
        public async Task<ActionResult> EditAdvancePayment(PaymentViewModel advancePayment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CreateTransaction();
                    var payment = _mapper.Map<PaymentDto>(advancePayment);
                    var addadvancePaymentId = await _mediator.Send(new EditAdvancePaymentCommand { payment = payment, currentUser = CurrentUser });
                    if (addadvancePaymentId != "")
                    {
                        _unitOfWork.Commit();
                        _unitOfWork.Save();
                        TempData["Message"] = PopUpServices.Notify(ValueMapping.payUpdate, addadvancePaymentId, notificationType: Alerts.success);
                        return RedirectToAction("Index", "Payment");
                    }
                    TempData["Message"] = PopUpServices.Notify("Unable to find the Payment Details", addadvancePaymentId, notificationType: Alerts.success);
                    return RedirectToAction("EditPayment", "Payment", new { id = advancePayment.ID });

                }
                return RedirectToAction("EditPayment", "Payment", new { id = advancePayment.ID });
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.addPayment);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }

        }
    }
}
