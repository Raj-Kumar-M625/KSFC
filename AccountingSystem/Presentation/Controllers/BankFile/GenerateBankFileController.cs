using Application.DTOs.GenerateBankFile;
using Application.UserStories.GenerateBankFiles.Requests.Commands;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Payment.Requests.Commands;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Common.Helper;
using Domain.GenarteBankfile;
using Domain.Payment;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Omu.Awem.Export;
using Omu.AwesomeMvc;
using Persistence;
using Persistence.Repositories.Generic;
using Presentation.AwesomeToolUtils;
using Presentation.Extensions.Payment;
using Presentation.Helpers;
using Presentation.Models.GenerateBankFiles;
using Presentation.Models.Master;
using Presentation.Services.Infra.Cache;
using Serilog;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{

    [Authorize]
    [SessionTimeoutHandlerAttribute]

    public class GenerateBankFileController:Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private GenericRepository<GenerateBankFile> repository;

        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;

        //private readonly AccountingDbContext _dbContext;
        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        public GenerateBankFileController(IMediator mediator,IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            repository = new GenericRepository<GenerateBankFile>(unitOfWork);

        }
        /// <summary>
        /// Author:Swetha M Date:06/06/2022
        /// Purpose: Get the list of payments and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Breadcrumb(ValueMapping.approvedPayments)]
        public IActionResult Index()
        {
            ViewBag.UserName = CurrentUser;

            return View();
        }
        /// <summary>
        /// Author:Swetha M Date:06/05/2022
        /// Purpose: Get the list of payments which are in approved status and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetVendorPaymentsToGenBankFile(GridParams gridParams,string[] forder,PaymentFilters paymentFilters)
        {
            try
            {


                var paymentList = await _mediator.Send(new GetPaymentsToGenBankfileRequest());
                var paymentQuery = paymentList.AsQueryable<Payments>();

                var frow = new PaymentFilterRow();
                var searchFilter = new PaymentSearchFilter();

                var filterBuilder = searchFilter.GetPaymentSearchCriteria(paymentList,paymentFilters);
                paymentQuery = await filterBuilder.ApplyAsync(paymentQuery,forder);


                var gmb = new GridModelBuilder<Payments>(paymentQuery.AsQueryable(),gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
           paymentQuery.Count();
               paymentQuery.Sum(x => x.PaidAmount );
                ViewBag.UserName = CurrentUser;
                return Json(await gmb.BuildAsync());
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getGenBankFile);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }
        private object MapToGridModel(Payments o)
        {
            return new
            {
                o.ID,
                o.VendorID,
                Name = o.Vendor == null ? "" : o.Vendor.Name,
                PaymentDate = o.PaymentDate.ToString("dd-MM-yyyy"),
                PaymentStatus = o.PaymentStatus.StatusMaster.CodeName,
                o.PaymentReferenceNo,
                ApprovedBy = o.ApprovedBy == null ? "Not Approved" : o.ApprovedBy,
                o.CreatedBy,
                PaymentAmount = o.PaidAmount .ToString("0.00"),
                Type=o.Type,
                AdvanceAmountUsed = o.AdvanceAmountUsed.ToString("0.00"),
                TotalPaymentAmount = o.PaymentAmount.ToString("0.00"),

            };
        }

        /// <summary>
        /// Author:Swetha M Date:07/05/2022
        /// Purpose: Get the list of Created By and bind it Awesome grid dropdown filter 
        /// </summary>
        /// <returns>Created By List</returns>
        public async Task<IActionResult> GetCreatedByList()
        {

            var list = await _mediator.Send(new GetPaymentsToGenBankfileRequest());
            var query = new List<KeyContent>();
            query.AddRange(list.Select(o => new KeyContent(o.CreatedBy,o.CreatedBy)).Distinct());
            return Json(query);

        }

        /// <summary>
        /// Author:Swetha M Date:07/05/2022
        /// Purpose: Get the list of Approved By and bind it Awesome grid dropdown filter
        /// </summary>
        /// <returns>Approved By List</returns>
        public async Task<IActionResult> GetApprovedByList()
        {
            var list = await _mediator.Send(new GetPaymentsToGenBankfileRequest());
            var query = new List<KeyContent>();
            query.AddRange(list.Select(o => new KeyContent(o.ApprovedBy == null ? "N/A" : o.ApprovedBy,o.ApprovedBy == null ? "N/A" : o.ApprovedBy)).Distinct());
            return Json(query);

        }

        /// <summary>
        /// Author:Swetha M Date:09/05/2022
        /// Purpose: Get the list of Payment statuses and bind it Awesome grid along with
        /// </summary>
        /// <returns>Payment Status List</returns>
        public async Task<IActionResult> GetPaymentStatus()
        {
            var list = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.pStatus });
            var query = new List<KeyContent>();
            query.AddRange(list.Select(o => new KeyContent(o.CodeValue,o.CodeValue)).Distinct());
            return Json(query);

        }
        /// <summary>
        /// Author:Swetha M Date:26/05/2022
        /// Purpose:Returns View for GenerateView
        /// </summary> 
        /// <returns></returns>
        [HttpGet, Breadcrumb(ValueMapping.approvedPayments)]
        public async Task<IActionResult> GenerateBankFile(List<int> id)
        {
            try
            {
                var PaymentList = await _mediator.Send(new GetPaymentListByIdRequest { ID = id });
                ViewBag.PaymentList = PaymentList;

                var paymentList = await _mediator.Send(new GetPaymentsToGenBankfileRequest());
               // IQueryable<Payments> paymentQuery = paymentList.AsQueryable<Payments>();
                List<int> vendorId = paymentList.Where(x => id.Any(y => y == x.ID)).Select(x => x.VendorID).Distinct().ToList();

                var Count = new GenerateBankFileModel
                {
                    NoOfTransactions = id.Count,
                    NoOfVendors = vendorId.Count,
                };
                var banks = await _mediator.Send(new GetBankMasterListRequest { });
                ViewBag.Banks = _mapper.Map<List<BankMasterModel>>(banks);
                ViewBag.UserName = CurrentUser;
                ViewBag.PaymentId = id.ToList();
                return PartialView(Count);
            }

            catch (Exception e)
            {
                Log.Information(ValueMapping.getGenBankFile);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }
        /// <summary>
        /// Author:Swetha M Date:04/05/2022
        /// Purpose:Saves Data of Generate Bank File
        /// </summary>
        /// <returns></returns>

        [HttpPost]

        public async Task<IActionResult> GenerateBankFile(GenerateBankFileModel model)
        {

            try
            {
                _unitOfWork.CreateTransaction();

                var banks = await _mediator.Send(new GetBankMasterListRequest { });
                var selectedBank = _mapper.Map<BankMasterModel>(banks.Where(x => x.Id == model.BankMasterId).FirstOrDefault());
                //if(selectedBank.IsBulkPayment!=model.IsBulkPayment)
                //{
                //    model.IsBulkPaymentModified = true;
                //}
                string[] Ids = model.BranchName.Split(',');
                model.BankMasterId = Convert.ToInt32(Ids.ElementAt(1));
                model.CreatedBy = CurrentUser;
                model.CreatedOn = DateTime.Now;
                GenerateBankFileDto genarteBankFile = _mapper.Map<GenerateBankFileDto>(model);
                var command = new CreateGenerateBankFileCommand { GenerateBankFileDto = genarteBankFile };
                var generatedId = await _mediator.Send(command);


                var command3 = new UpdatePaymentStatusCommand { PaymentID = model.PaymentId };
              await _mediator.Send(command3);


                var command2 = new AddMappingFromPaymentsToGenerateBankFileCommand { Id = model.PaymentId,GenerateBankFileId = generatedId,CurrentUser = CurrentUser };
                await _mediator.Send(command2);


                var command4 = new AddGeneratteBankFileStatusCommand { GeneratedBankID = generatedId,CurrentUser = CurrentUser };
                 await _mediator.Send(command4);

                _unitOfWork.Commit();
                _unitOfWork.Save();
                TempData["Message"] = PopUpServices.Notify(ValueMapping.generatedBankFile,ValueMapping.BankFile,notificationType: Alerts.success);
                return RedirectToAction("Index","BankFiles");
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getGenBankFile);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }

        }
        /// <summary>
        /// Author:Swetha M Date:04/05/2022
        /// Purpose:Saves Data of Generate Bank File
        /// </summary>
        /// <returns></returns>

        [HttpGet]

        public async Task<IActionResult> GetDefaultReocrds()
        {

            try
            {
                var paymentList = await _mediator.Send(new GetPaymentsToGenBankfileRequest());
                IQueryable<Payments> paymentQuery = paymentList.AsQueryable<Payments>();
                var mdoelCount = paymentQuery.Count();
                var aprovedAmount = paymentQuery.Sum(x => x.PaidAmount );

                return Json(new { ModelCount = mdoelCount,TotalAmount = aprovedAmount.ToString("0.00") });


            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getGenBankFile);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }

        }
        public async Task<GridModel<Payments>> GetVendorPaymentsToDownload(GridParams gridParams,string[] forder,PaymentFilters paymentFilters)
        {
            try
            {


                var paymentList = await _mediator.Send(new GetPaymentsToGenBankfileRequest());
                IQueryable<Payments> paymentQuery = paymentList.AsQueryable<Payments>();

                var frow = new PaymentFilterRow();
                PaymentSearchFilter searchFilter = new PaymentSearchFilter();

                var filterBuilder = searchFilter.GetPaymentSearchCriteria(paymentList,paymentFilters);
                paymentQuery = await filterBuilder.ApplyAsync(paymentQuery,forder);


                var gmb = new GridModelBuilder<Payments>(paymentQuery.AsQueryable(),gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                paymentQuery.Count();
              paymentQuery.Sum(x => x.PaidAmount );
                ViewBag.UserName = CurrentUser;
                return (await gmb.BuildModelAsync());
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getGenBankFile);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
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
        public async Task<IActionResult> ExportToGenBankBileList(PaymentFilters paymentFilters,string[] forder,GridParams gridParams)
        {
            try
            {

                var gridModel = await GetVendorPaymentsToDownload(gridParams,forder,paymentFilters);
                var expColumns = new[]
                {
                    new ExpColumn { Name = "PaymentReferenceNo", Width = 2.5f, Header = "Payment Ref No" },
                    new ExpColumn { Name = "PaymentDate", Width = 2.5f, Header = "Pay Date (dd-mm-yyy)" },
                    new ExpColumn { Name = "Type", Width = 2.5f, Header = "Type" },
                    new ExpColumn { Name = "Name", Width = 2.5f, Header = "Vendor Name" },
                    new ExpColumn { Name = "TotalPayment", Width = 3f, Header = "Total Payable Amount ₹" },
                    new ExpColumn { Name = "PaymentAmount", Width = 3f, Header = "Payable Amount ₹" },
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
                    return File(stream.ToArray(),"application/vnd.ms-excel","GenerateBankFileLsit.xls");
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
