using Application.DTOs.Bill;
using Application.DTOs.Payment;
using Application.UserStories.Bill.Requests.Commands;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Payment.Requests.Commands;
using Application.UserStories.Payment.Requests.Queries;
using Application.UserStories.Vendor.Requests.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Common.Downloads;
using Common.Helper;
using Domain.Payment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Omu.Awem.Export;
using Omu.AwesomeMvc;
using Persistence;
using Persistence.Repositories.Generic;
using Presentation.AwesomeToolUtils;
using Presentation.Extensions.Payment;
using Presentation.Helpers;
using Presentation.Models.Bill;
using Presentation.Models.Payment;
using Presentation.Models.Vendor;
using Presentation.Services.Infra.Cache;
using Serilog;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers.Payment
{
    /// <summary>
    /// Author:Swetha M Date:05/05/2022
    /// Purpose: Payment Controller
    /// </summary>
    /// <returns></returns>
    /// 
    [Authorize]
    [SessionTimeoutHandlerAttribute]

    public class PaymentController : Controller
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private GenericRepository<Payments> repository;

        protected string CurrentUser => HttpContext.User.Identity.Name;
        protected bool Role => this.HttpContext.User.IsInRole("Administrator");

        //// <summary>
        /// Author:Swetha M Date:05/05/2022
        /// Purpose: Constructor
        /// </summary>
        /// <returns></returns>
        public PaymentController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            repository = new GenericRepository<Payments>(unitOfWork);
        }

        /// <summary>
        /// Author:Swetha M Date:05/05/2022
        /// Purpose: Get the list of payments and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Breadcrumb(ValueMapping.payList)]
        public IActionResult Index()
        {
            try
            {
                ViewBag.UserName = CurrentUser;
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

        /// <summary>
        /// Author:Swetha M Date:06/05/2022
        /// Purpose: Get the list of payments and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetVendorPayments(GridParams gridParams, string[] forder, PaymentFilters paymentFilters)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Payment List
                var paymentList = await _mediator.Send(new GetPaymentsRequest());
                IQueryable<Payments> paymentQuery = paymentList.AsQueryable();


                var frow = new PaymentFilterRow();
                PaymentSearchFilter searchFilter = new PaymentSearchFilter();

                //Get Payment Searchfilters
                var filterBuilder = searchFilter.GetPaymentSearchCriteria(paymentQuery, paymentFilters);
                paymentQuery = await filterBuilder.ApplyAsync(paymentQuery, forder);

                var gmb = new GridModelBuilder<Payments>(paymentQuery.AsQueryable(), gridParams)
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

        private object MapToGridModel(Payments o)
        {
            try
            {
                return new
                {
                    o.ID,
                    PaymentReferenceNo = o.PaymentReferenceNo,
                    PaymentDate = o.PaymentDate.ToString("dd-MM-yyyy"),
                    VendorId = o.VendorID,
                    Name = o.Vendor == null ? "" : o.Vendor.Name,
                    PaymentAmount = o.PaidAmount.ToString("0.00"),
                    CreatedBy = o.CreatedBy,
                    PaymentStatus = o.PaymentStatus.StatusMaster.CodeValue,
                    ApprovedBy = o.ApprovedBy == null ? "Not Approved" : o.ApprovedBy,
                    Type = o.Type,
                    AdvanceAmountUsed = o.AdvanceAmountUsed.ToString("0.00"),
                    TotalPaymentAmount = o.PaymentAmount.ToString("0.00"),

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

        /// <summary>
        /// Author:Swetha M Date:07/05/2022
        /// Purpose: Get the list of Created By and bind it Awesome grid dropdown filter 
        /// </summary>
        /// <returns>Created By List</returns>
        public async Task<IActionResult> GetCreatedByList()
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Created By List
                var paymentList = await _mediator.Send(new GetPaymentsRequest());
                var createdbyQuery = new List<KeyContent>();
                createdbyQuery.AddRange(paymentList.Select(o => new KeyContent(o.CreatedBy, o.CreatedBy)).Distinct());
                return Json(createdbyQuery);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getCreatedByList);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }

        }

        /// <summary>
        /// Author:Swetha M Date:07/05/2022
        /// Purpose: Get the list of Approved By and bind it Awesome grid dropdown filter
        /// </summary>
        /// <returns>Approved By List</returns>
        public async Task<IActionResult> GetApprovedByList()
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Approved By List
                var paymentList = await _mediator.Send(new GetPaymentsRequest());
                var approvedbyQuery = new List<KeyContent>();
                approvedbyQuery.AddRange(paymentList.Select(o => new KeyContent(o.ApprovedBy == null ? "Not Approved" : o.ApprovedBy, o.ApprovedBy == null ? "Not Approved" : o.ApprovedBy)).Distinct());
                return Json(approvedbyQuery);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getApprovedByList);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }

        }

        /// <summary>
        /// Author:Swetha M Date:09/05/2022
        /// Purpose: Get the list of Payment statuses and bind it Awesome grid along with
        /// </summary>
        /// <returns>Payment Status List</returns>
        public async Task<IActionResult> GetPaymentStatus(bool isIdKey = false)
        {
            try
            {

                //Get Payment Status


                var tdsStatusList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.pStatus });
                var tdsStatusQuery = new List<KeyContent>();
                tdsStatusQuery.AddRange(tdsStatusList.Select(o => new KeyContent(isIdKey ? o.Id : o.CodeValue, o.CodeValue)).Distinct());
                return Json(tdsStatusQuery);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getPaymentStatus);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                throw;
            }

        }

        /// <summary>
        /// Author:Swetha M Date:09/05/2022
        /// Purpose: Get the list of Payment statuses and bind it Awesome grid along with
        /// </summary>
        /// <returns>Payment Type List</returns>
        public async Task<IActionResult> GetPaymentType(bool isIdKey = false)
        {
            try
            {

                //Get Payment Status


                var paymentType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "PaymentType" });
                var paymentTypeQuery = new List<KeyContent>();
                paymentTypeQuery.AddRange(paymentType.Select(o => new KeyContent(isIdKey ? o.Id : o.CodeValue, o.CodeValue)).Distinct());
                return Json(paymentTypeQuery);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getPaymentStatus);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                throw;
            }

        }

        /// <summary>
        /// Author:Swetha M Date:19/05/2022
        /// Purpose:Returns View with Data  for Add Payment Page
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Breadcrumb(ValueMapping.addPay)]
        public async Task<ActionResult> AddPayment(int id)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Vendors List
                var vendorList = await _mediator.Send(new GetVendorListByIDRequest { ID = id });
                var vendorDetails = _mapper.Map<VendorViewModel>(vendorList);

                if (vendorList.VendorBankAccounts == null)
                {
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.addBankAccount, ValueMapping.bankntf, notificationType: Alerts.warning);
                    return RedirectToAction("EditVendor", "Vendor", new { Id = id });
                }

                //Get Bills List  
                var billsList = await _mediator.Send(new GetBillsListByIdRequest { ID = id, Type = "Add" });
                var blist = _mapper.Map<List<BillModel>>(billsList);
                var billPaymentList = blist.Where(o => o.BillStatus.StatusMaster.CodeValue == ValueMapping.approved || o.BillStatus.StatusMaster.CodeValue == ValueMapping.paid || o.BillStatus.StatusMaster.CodeValue == ValueMapping.PartiallyPaid).ToList();
                var billDetails = billPaymentList;
                var advancePayments = await _mediator.Send(new GetAdvancePaymentByVendorRequest { vendorID = id });
                ViewBag.AdvancePayments = _mapper.Map<List<PaymentViewModel>>(advancePayments.Where(x => x.BalanceAdvanceAmount > 0));

                if (blist.Count == 0)
                {
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.upBill, ValueMapping.billntf, notificationType: Alerts.warning);
                    return RedirectToAction("EditVendor", "Vendor", new { Id = id });
                }
                else if (billPaymentList.Count == 0)
                {
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.addbillMessage, ValueMapping.billsNotApproved, notificationType: Alerts.warning);

                    return RedirectToAction("EditVendor", "Vendor", new { Id = id });
                }
                else
                {
                    ViewBag.BillDetails = billDetails;
                    ViewBag.VendorBalanceDetails = vendorDetails.VendorBalance;
                    ViewBag.VendorDetails = vendorDetails;
                    ViewBag.UserName = CurrentUser;
                    ViewBag.Id = id;
                    return View(billDetails.ToList());
                }

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
        /// <summary>
        /// Author:Swetha M Date:04/06/2022
        /// Purpose:Add Payment Approve Functionality
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<ActionResult> AddPayment(List<BillModel> Bills)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    List<BillsDto> billsDetails = _mapper.Map<List<BillsDto>>(Bills);
                    var res = billsDetails.Sum(x => x.NetPayableAmount);
                    if (res <= 0)
                    {
                        TempData["Message"] = PopUpServices.Notify(ValueMapping.AddAmount, ValueMapping.paymentamtnotgre, notificationType: Alerts.warning);
                        return RedirectToAction("EditPayment", "Payment", new { Id = Bills.First().VendorId });
                    }
                    _unitOfWork.CreateTransaction();
                    foreach (var bill in Bills)
                    {
                        bill.CreatedBy = CurrentUser;
                        bill.ModifedBy = CurrentUser;
                    }

                    //Get the Bills where the netpayable greater than 0
                    var billslists = Bills.Where(o => o.BalanceAmount > 0).ToList();

                    //Map the Model data to Dto
                    var billlist = _mapper.Map<List<BillsDto>>(billslists);

                    //Request to update the Bills
                    var command = new UpdateBillsCommandRequest { bills = billlist };
                    var billId = await _mediator.Send(command);
                    var billsId = "Payment Ref No. " + billId.PaymentRefNo;
                    _unitOfWork.Save();
                    _unitOfWork.Commit();
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.payAdd, billsId, notificationType: Alerts.success);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.addPayment);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Author:Swetha M Date:08/07/2022
        /// Purpose:Edit Payment  Functionality
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Breadcrumb(ValueMapping.edPay)]
        public async Task<ActionResult> EditPayment(int id)
        {
            try
            {

                var paymentDetails = await _mediator.Send(new GetPaymentsByIdRequest { Id = id });
                var payments = _mapper.Map<VendorPaymentViewModel>(paymentDetails);
                var mappedPaymentIds = payments.MappingAdvancePayment.Select(x => x.AdvancePaymentId).ToList();
                ViewBag.paymets = payments;
                ViewBag.VendorDetails = payments.Vendor;
                ViewBag.VendorBalanceDetails = payments.Vendor.VendorBalance;
                ViewBag.UserName = CurrentUser;
                ViewBag.Role = HttpContext.Session.GetString("_role");

                var advancePayments = await _mediator.Send(new GetAdvancePaymentByVendorRequest { vendorID = (int)payments.VendorID });
                var mappedAdvancePayments = advancePayments.Where(x => mappedPaymentIds.Contains(x.ID)).ToList();
                var unmappedAdvancePayments = advancePayments.Where(x => !mappedPaymentIds.Contains(x.ID) && x.BalanceAdvanceAmount > 0).ToList();

                foreach (var item in mappedAdvancePayments)
                {
                    var amount = item.PaidAmount - item.BalanceAdvanceAmount;
                    item.AdvanceAmountUsed =(decimal) amount<= 0 ? 0 : (decimal)amount;  
                }
                mappedAdvancePayments.AddRange(unmappedAdvancePayments);
                ViewBag.AdvancePayments = _mapper.Map<List<PaymentViewModel>>(mappedAdvancePayments);

                if (payments.Type == "Actual")
                {
                    var billpaymentDetails = await _mediator.Send(new GetBillPaymentRequest { PaymentID = id });                 
                    var res = billpaymentDetails.ToList();
                    return View(res);
                }
                else
                {
                    var paymentViewModel = _mapper.Map<PaymentViewModel>(paymentDetails);
                    return View("~/Views/AdvancePayment/EditAdvancePayment.cshtml", paymentViewModel);
                }

            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();

                Log.Information(ValueMapping.addPayment);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditPayment(List<BillPayment> bills)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var paymentDetails = await _mediator.Send(new UpdateBillPaymentCommandRequest { bills = bills });
                var paymentId = "Payment Ref No. " + paymentDetails;
                _unitOfWork.Save();
                _unitOfWork.Commit();
                TempData["Message"] = PopUpServices.Notify(ValueMapping.payUpdate, paymentId, notificationType: Alerts.success);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();

                Log.Information(ValueMapping.editPayment);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }

        }




        /// <summary>
        /// Author:Swetha M Date:08/07/2022
        /// Purpose:Returns Apporve Payments View against each vendor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        // [Breadcrumb(ValueMapping.bankFile)]
        public async Task<IActionResult> ApprovePayment(int id)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Payment Details
                ViewBag.Role = HttpContext.Session.GetString("_role");
                var paymentDetails = await _mediator.Send(new GetPaymentsByIdRequest { Id = id });
                if (paymentDetails.PaymentStatus.StatusMaster.CodeValue != "Rejected")
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "Payment", "Payment List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("ApprovePayment", "Payment", "Approve Payments")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;

                }
                else
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "Payment", "Payment List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("ApprovePayment", "Payment", "Rejected Payments")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;
                }
                if (ViewBag.Role == "User")
                {

                    if (paymentDetails.PaymentStatus.StatusMaster.CodeValue == "Pending" || (paymentDetails.PaymentStatus.StatusMaster.CodeValue == "Rejected" && paymentDetails.Type == "Advance"))
                    {
                        return RedirectToAction("EditPayment", new { id = id });
                    }
                }
                


                var payments = _mapper.Map<VendorPaymentViewModel>(paymentDetails);

                ViewBag.VendorBalanceDetails = payments.Vendor.VendorBalance;
                var advancePayments = await _mediator.Send(new GetAdvancePaymentByVendorRequest { vendorID = (int)payments.VendorID });
                ViewBag.AdvancePayments = _mapper.Map<List<PaymentViewModel>>(advancePayments);
                var billpaymentDetails = await _mediator.Send(new GetBillPaymentRequest { PaymentID = id });
                ViewBag.UserName = CurrentUser;
                ViewBag.PaymentDetails = billpaymentDetails.ToList();
                ViewBag.Role = HttpContext.Session.GetString("_role");
                return View(payments);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.edit);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }
        /// <summary>
        /// Author:Swetha M Date:08/07/2022
        /// Purpose:Returns Apporve Payments View against each vendor
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Breadcrumb(ValueMapping.bankFile)]
        public async Task<IActionResult> ApprovePayment(int paymentId, string paymentRefNo, string remarks, string status)
        {
            try
            {
                if (paymentId != null)
                {
                    _unitOfWork.CreateTransaction();
                    //Get Payment Details
                    await _mediator.Send(new ApprovePaymentCommand { paymentId = paymentId, CurrentUser = CurrentUser, Remarks = remarks, Status = status });
                    if (status == ValueMapping.approved.ToLower())
                    {
                        TempData["Message"] = PopUpServices.Notify(ValueMapping.approvesuc, ValueMapping.approvePayment + paymentRefNo + ".", notificationType: Alerts.success);
                    }
                    else if (status == ValueMapping.rejected.ToLower())
                    {
                        TempData["Message"] = PopUpServices.Notify(ValueMapping.approvefail, "", notificationType: Alerts.error);
                    }
                    return RedirectToAction("Index");
                }

                return View();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.edit);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Author:Pankaj K Date:28/05/2022
        /// Purpose: Download the File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FileDownload(string fileName)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Data from data store
                DownloadService<VendorPaymentListDto> dl = new();
                List<VendorPaymentListDto> listOfPayments = new();
                listOfPayments = await _mediator.Send(new GetVendorPaymentListIEnumerable());
                //Parse list data into byte array and generate the file            
                dl.ListItems = listOfPayments;
                dl.FileName = fileName;
                FileContentResult fc = new FileContentResult(dl.GetFile(), dl.ContentType());
                fc.FileDownloadName = fileName.AddTimeStamp();
                return fc;
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.fileDownload);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }
        /// <summary>
        /// Author:Swetha M Date:21/11/2022
        /// Purpose: Get the list of payments and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <returns></returns>
        public async Task<GridModel<Payments>> GetVendorPaymentsToDownload(GridParams gridParams, string[] forder, PaymentFilters paymentFilters)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Payment List
                var paymentList = await _mediator.Send(new GetPaymentsRequest());
                IQueryable<Payments> paymentQuery = paymentList.AsQueryable();


                var frow = new PaymentFilterRow();
                PaymentSearchFilter searchFilter = new PaymentSearchFilter();

                //Get Payment Searchfilters
                var filterBuilder = searchFilter.GetPaymentSearchCriteria(paymentQuery, paymentFilters);
                paymentQuery = await filterBuilder.ApplyAsync(paymentQuery, forder);

                var gmb = new GridModelBuilder<Payments>(paymentQuery.AsQueryable(), gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = MapToGridModel,
                    Tag = new { frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return await gmb.BuildModelAsync();
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
        /// <summary>        
        /// Purpose: Export Bank File List
        /// Author: Swetha M; Date: 09/11/2022        
        /// </summary>
        /// <param name="paymentFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<IActionResult> ExportToPaymentList(PaymentFilters paymentFilters, string[] forder, GridParams gridParams)
        {
            try
            {

                var gridModel = await GetVendorPaymentsToDownload(gridParams, forder, paymentFilters);
                var expColumns = new[]
                {
                    new ExpColumn { Name = "PaymentBillReference", Width = 2.5f, Header = "Payment Ref No" },
                    new ExpColumn { Name = "Type", Width = 2.5f, Header = "Type" },
                    new ExpColumn { Name = "PaymentDate", Width = 2.5f, Header = "Pay Date (dd-mm-yyy)" },
                    new ExpColumn { Name = "Name", Width = 2.5f, Header = "Vendor Name" },
                    new ExpColumn { Name = "TotalPaymentAmount", Width = 3f, Header = "Total Payable Amount ₹" },
                    new ExpColumn { Name = "PaidAmount", Width = 3f, Header = "Payable Amount ₹" },
                    new ExpColumn { Name = "AdvanceAmountUsed", Width = 3f, Header = "Advance Amount Used ₹" },
                    new ExpColumn { Name = "CreatedBy", Width = 3f, Header ="Created By" },
                    new ExpColumn { Name = "ApprovedBy", Width = 2f, Header="Approved By" },
                    new ExpColumn { Name = "PaymentStatus", Width = 2f, Header="Payment Status" }
                };
                var workbook = (new GridExcelBuilder(expColumns)).Build(gridModel);
                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(), "application/vnd.ms-excel", "PaymentsList.xls");
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.exportTdsPaidList);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

    }
}

