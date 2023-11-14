using AutoMapper;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories.Generic;
using Persistence;
using Presentation.Services.Infra.Cache;
using Common.ConstantVariables;
using SmartBreadcrumbs.Attributes;
using System;
using Serilog;
using Application.UserStories.Vendor.Requests.Queries;
using Omu.AwesomeMvc;
using Presentation.Extensions.Vendor;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Presentation.GridFilters.ListOfTransaction;
using Application.UserStories.Transactions.Requests.Queries;
using Domain.Transactions;
using Application.UserStories.Master.Request.Queries;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Omu.Awem.Export;
using Presentation.AwesomeToolUtils;
using Presentation.Extensions.Bill;
using System.IO;
using Domain.Bill;
using Application.UserStories.Bill.Requests.Queries;

namespace Presentation.Controllers.ListOfTransaction
{
    public class ListOfTransactionController : Controller
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private GenericRepository<Transaction> repository;
        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        protected bool Role => this.HttpContext.User.IsInRole("Administrator");
        public ListOfTransactionController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            repository = new GenericRepository<Transaction>(unitOfWork);
        }

        [Breadcrumb(ValueMapping.listOfTransaction)]
        public IActionResult Index()
        {
            try
            {
                ViewBag.UserName = CurrentUser;
                ViewBag.Role = Role;
                return View();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.Inside, ValueMapping.IndexM);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }

        public async Task<IActionResult> GetListOfTransactionList(ListOfTransactionFilter transactionFilter, GridParams gridParams, string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                forder = forder ?? new string[] { };

                var vendorList = await _mediator.Send(new GetListOfTransactionQuerableValue());
                var vendorQuery = vendorList.AsQueryable();


                // filter row search
                var frow = new ListOfTransactionFilterRow();

                var searchFilter = new ListOfTransactionSearchFilters();
                var filterBuilder = searchFilter.GetListOfTransactionSearchCriteria(vendorList, transactionFilter);

                vendorQuery = await filterBuilder.ApplyAsync(vendorQuery, forder);

                var gmb = new GridModelBuilder<Transaction>(vendorQuery.AsQueryable(), gridParams)
                {
                    KeyProp = o => o.ID,


                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };

                return Json(await gmb.BuildAsync());
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetVendor);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<ActionResult> GetAssessmentYear()
        {
            try
            {
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });
                var assessmentYearQuery = new List<KeyContent>();
                assessmentYearQuery.AddRange(assessmentYearList.Select(o => new KeyContent(o.CodeValue, o.CodeValue)).Distinct());
                return Json(assessmentYearQuery);
            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.getTDSAssessmentYearStatus);
                Log.Information(ex.InnerException?.Message ?? string.Empty);
                Log.Information(ex.Message);
                throw;
            }
        }



        public async Task<ActionResult> GetTransactionType()
        {
            try
            {
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.transactionDetailedType });
                var assessmentYearQuery = new List<KeyContent>();
                assessmentYearQuery.AddRange(assessmentYearList.Select(o => new KeyContent(o.CodeValue, o.CodeValue)).Distinct());
                return Json(assessmentYearQuery);

                //var list = await _mediator.Send(new GetListOfTransactionQuerableValue());
                //var query = new List<KeyContent> { new KeyContent(string.Empty, "All") };
                //query.AddRange(list.Select(o => new KeyContent(o.Status, o.Status)).Distinct());
                //return Json(query);




            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.getTDSAssessmentYearStatus);
                Log.Information(ex.InnerException?.Message ?? string.Empty);
                Log.Information(ex.Message);
                throw;
            }
        }

        public async Task<ActionResult> GetStatusList()
        {
            try
            {
                var list = await _mediator.Send(new GetListOfTransactionQuerableValue());
                var query = new List<KeyContent> { new KeyContent(string.Empty, "--- Select Status ---") };
                query.AddRange(list.Select(o => new KeyContent(o.Status, o.Status)).Distinct());
                return Json(query);
            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.getTDSAssessmentYearStatus);
                Log.Information(ex.InnerException?.Message ?? string.Empty);
                Log.Information(ex.Message);
                throw;
            }
        }



        private object MapToGridModel(Transaction o)
        {
            try
            {
                return new
                {
                    o.ID,
                    o.ReferenceNumber,
                    BillReferenceNo = o.BillReferenceNo == null ? "N/A" : o.BillReferenceNo == "" ? "N/A" : o.BillReferenceNo,
                    TransactionRefNo = o.TransactionRefNo == null ? "N/A" : o.TransactionRefNo == "" ? "N/A" : o.TransactionRefNo,
                    GstIn_Number = o.GSTIN_Number == null ? "UnRegistered" : o.GSTIN_Number,
                    Pan_Number = o.PAN_Number == null ? "N/A" : o.PAN_Number,
                    Tan_Number = o.TAN_Number == null ? "N/A" : o.TAN_Number,
                    TransactionType = o.TransactionType == null ? "N/A" : o.TransactionType,
                    BillNo = o.BillNo == null ? "N/A" : o.BillNo,
                    o.Status,
                    o.ReconcileStatus,
                    AccountNumber = o.AccountNumber,
                    VendorName = o.VendorName,
                    TransactionDate = o.TransactionDate.ToString("dd-MM-yyyy"),
                    TransactionGeneratedDate = o.TransactionGeneratedDate.ToString() == null ? "N/A" : o.TransactionGeneratedDate.ToString(),
                    Amount = o.Amount,
                    CodeValue = o.CodeValue,
                    dfsd = o.UTRNumber == null ? "N/A" : o.UTRNumber,
                    CreatedBy = o.CreatedBy
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

        [HttpPost]
        public async Task<IActionResult> ExportToGridListOfTransaction(ListOfTransactionFilter transactionFilter, GridParams gridParams, string[] forder)
        {
            try
            {

                var gridModel = await GetListOfTransaction(transactionFilter, gridParams, forder);
                var expColumns = new[]
                {

                    new ExpColumn { Name = "BillReferenceNo", Width = 2.5f, Header = "Reference No" },
                    new ExpColumn { Name = "TransactionRefNo", Width = 2.5f, Header = "Transaction Ref.No" },
                    new ExpColumn { Name = "VendorName", Width = 2.5f, Header = "Vendor Name" },
                    new ExpColumn { Name = "GstIn_Number", Width = 2.5f, Header = "GST IN Number" },
                    new ExpColumn { Name = "Pan_Number", Width = 2.5f, Header = "PAN Number" },
                    new ExpColumn { Name = "Tan_Number", Width = 2.5f, Header = "TAN Number" },
                    new ExpColumn { Name = "Amount", Width = 3f, Header ="₹ Amount" },
                    new ExpColumn { Name = "CodeValue", Width = 3f, Header =" Transaction Detailed Type" },
                    new ExpColumn { Name = "TransactionType", Width = 3f, Header ="Transaction Type" },
                   new ExpColumn { Name = "AccountNumber", Width = 3f, Header ="Account Number" },
                    new ExpColumn { Name = "dfsd", Width = 2f, Header="UTR Number" },
                    new ExpColumn { Name = "TransactionDate", Width = 2f, Header="Transaction Date" },
                    new ExpColumn { Name = "TransactionGeneratedDate", Width = 2f, Header="Transaction Generated Date" },
                    new ExpColumn { Name = "Status", Width = 2f, Header="Status" },
                    new ExpColumn { Name = "CreatedBy", Width = 2f, Header="Created By" }

                };

                var workbook = (new GridExcelBuilder(expColumns)).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(), "application/vnd.ms-excel", "ListOfTransaction.xls");
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

        public async Task<GridModel<Transaction>> GetListOfTransaction(ListOfTransactionFilter transactionFilter, GridParams gridParams, string[] forder)
        {
            try
            {

                var list = await _mediator.Send(new GetListOfTransactionQuerableValue());

                var frow = new ListOfTransactionFilterRow();
                var searchFilter = new ListOfTransactionSearchFilters();
                var filterBuilder = searchFilter.GetListOfTransactionSearchCriteria(list, transactionFilter);
                var query = await filterBuilder.ApplyAsync(list, forder);

                var gmb = new GridModelBuilder<Transaction>(query, gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return await gmb.BuildModelAsync();

            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetVendor);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
