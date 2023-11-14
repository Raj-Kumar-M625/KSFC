using Application.DTOs.BankFile;
using Application.DTOs.GenerateBankFile;
using Application.UserStories.BankFile.Requests.Commands;
using Application.UserStories.BankFile.Requests.Queries;
using Application.UserStories.GenerateBankFiles.Requests.Commands;
using Application.UserStories.GenerateBankFiles.Requests.Queries;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Transactions.Handlers;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Common.Downloads;
using Common.Helper;
using Domain.GenarteBankfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Omu.Awem.Export;
using Omu.AwesomeMvc;
using Persistence;
using Presentation.AwesomeToolUtils;
using Presentation.Extensions.BankFile;
using Presentation.Extensions.Payment;
using Presentation.Helpers;
using Presentation.Models.BankFile;
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

namespace Presentation.Controllers.BankFile
{
    //Author:Swetha M Date:06/06/2022
    //Purpose: Bankfile Controller to get Bank File Detail
    [Authorize]
    [SessionTimeoutHandler]

    public class BankFilesController:Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private const string resultViewPathBE = "~/Views/BankFiles/";
        private string Status = string.Empty;
        protected string CurrentUser => HttpContext.User.Identity.Name;
        public BankFilesController(IMediator mediator,IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        /// <summary>
        /// Author:Swetha M Date:06/05/2022
        /// Purpose: Get the list of BankFile 
        /// </summary>
        /// <returns></returns>
        /// 
        [Breadcrumb(ValueMapping.bankFileList)]
        public IActionResult Index()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }
        /// <summary>
        /// Author:Swetha M Date:05/11/2022
        /// Purpose: Get the list of bankfiles which are in genearted 
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetBankFileList(GridParams gridParams,string[] forder,BankFileFilters bankFileFilters,string Status,string BankName)
        {
            try
            {               
                if (Status!= null)
                {
                    bankFileFilters.paymentStatus = Status;
                    bankFileFilters.bankName = BankName;
                }                            
                var bankFileList = await _mediator.Send(new GetGeneratedBqankFileListRequest());
                IQueryable<GenerateBankFile> bankFile = bankFileList.AsQueryable();
                forder = forder ?? new string[] { };
                var frow = new BankFileFilterRow();
                BankFileSerachFilter searchFilter = new BankFileSerachFilter();
                var filterBuilder = searchFilter.GetBankFileSearchCriteria(bankFile,bankFileFilters);
                bool status = Array.Exists(forder, element => element == "bankFileFilters.paymentStatus");
                if (status)
                {  
                    forder = new List<string>(forder) {"bankFileFilters.paymentStatus"}.ToArray();
                }
                bankFile = await filterBuilder.ApplyAsync(bankFile,forder);
                var gmb = new GridModelBuilder<GenerateBankFile>(bankFile.AsQueryable(),gridParams)
                {
                   KeyProp = o => o.Id,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
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
        private object MapToGridModel(GenerateBankFile o)
        {
            try
            {   
                return new
                {
                    o.Id,
                    BankFileReferenceNO = o.BankFileReferenceNo,
                    CreatedOn = o.CreatedOn.ToString("dd-MM-yyyy"),
                    o.NoOfVendors,
                    o.NoOfTransactions,
                    o.CreatedBy,
                    sameUTRno = o.SameBankUTRNumber !=null ? o.SameBankUTRNumber :"-" ,
                   // differentUTRNo = o.DifferentBankUTRNumber !=null? o.DifferentBankUTRNumber: "-" ,
                    SourceBankName = o.Bank.BankName,
                    AccountNumber = o.Bank.Accountnumber,
                    TotalAmount = o.TotalAmount.ToString("0.00"),
                    Status = o.CodeValue != null ? o.CodeValue: string.Empty
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

        public async Task<GridModel<GenerateBankFile>> GetBankFileListToDonwload(GridParams gridParams,string[] forder,BankFileFilters bankFileFilters)
        {
            try
            {
                var bankFileList = await _mediator.Send(new GetGeneratedBqankFileListRequest());
                IQueryable<GenerateBankFile> bankFile = bankFileList.AsQueryable();


                forder = forder ?? new string[] { };
                var frow = new BankFileFilterRow();
                BankFileSerachFilter searchFilter = new BankFileSerachFilter();
                var filterBuilder = searchFilter.GetBankFileSearchCriteria(bankFile,bankFileFilters);
                bankFile = await filterBuilder.ApplyAsync(bankFile,forder);
                var gmb = new GridModelBuilder<GenerateBankFile>(bankFile.AsQueryable(),gridParams)
                {
                    KeyProp = o => o.Id,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
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
        /// Author:Swetha M Date:26/05/2022
        /// Purpose:Returns View for EnterUTR 
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        public async Task<IActionResult> EnterUTR(List<int> id)
        {
            try
            {

                var bankFileList = await _mediator.Send(new GetGeneratedBqankFileListRequest());
                IQueryable<GenerateBankFile> bankFile = bankFileList.AsQueryable();
                BankFileUtrDetailsModel bankFileUTRDetails = new BankFileUtrDetailsModel();
                var banks = await _mediator.Send(new GetBankMasterListRequest { });
                var selectedBank = _mapper.Map<BankMasterModel>(banks.Where(x => x.Id == bankFile.FirstOrDefault().BankMasterId).FirstOrDefault());

                bankFileUTRDetails.NoOfTransactions = bankFile.Where(x => id.Any(y => y == x.Id)).Sum(x => x.NoOfTransactions);
                bankFileUTRDetails.TotalAmount = bankFile.Where(x => id.Any(y => y == x.Id)).Sum(x => x.TotalAmount);
                bankFileUTRDetails.IsBulkPayment = selectedBank.IsBulkPayment;
                ViewBag.GenerateBankFile = id;
                return PartialView(bankFileUTRDetails);


            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getGenBankFile);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }

        }
        /// Author:Swetha M Date:26/05/2022
        /// Purpose:Returns View for EnterUTR 
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        public async Task<IActionResult> EnterUTR(BankFileUtrDetailsModel bankFileUTRDetails)
        {
            try
            {
                if (bankFileUTRDetails.SameBankUTRNumber != null || bankFileUTRDetails.DifferentBankUTRNumber != null)
                {
                    bankFileUTRDetails.CreatedBy = CurrentUser;
                    bankFileUTRDetails.ModifedBy = CurrentUser;
                    bankFileUTRDetails.CreatedOn = DateTime.Now;
                    bankFileUTRDetails.ModifiedOn = DateTime.Now;
                    var list = _mapper.Map<BankFileUtrDetailsDto>(bankFileUTRDetails);
                    var transactionSummary = new AddPaymentTransactionSummaryCommand { bankFileUTRDetails =list, currentUser =CurrentUser};
                    await _mediator.Send(transactionSummary);

                    var transaction = new AddPaidTransactionCommand {Id = bankFileUTRDetails.Id, GenerateBankFileID = bankFileUTRDetails.GenerateBankFileID,UTR = bankFileUTRDetails.SameBankUTRNumber ?? bankFileUTRDetails.DifferentBankUTRNumber,  CurrentUser = CurrentUser };
                    await _mediator.Send(transaction);

                    var command = new AddBankUtrDetailsCommand { bankFileDetails = list };
                    var generatedId = await _mediator.Send(command);

                    var command3 = new UpdateBankFilePaymentsStatusCommand { GenerateBankFileID = bankFileUTRDetails.GenerateBankFileID, CurrentUser = CurrentUser };
                    await _mediator.Send(command3);

                    var command2 = new AddMappingForBankFileCommand { BankFileId = generatedId, GenerateBankFileID = bankFileUTRDetails.GenerateBankFileID, CurrentUser = CurrentUser };
                     await _mediator.Send(command2);

                    var command4 = new UpdateGenerateBankFileStatusCommand { GeneratedBankFileID = bankFileUTRDetails.GenerateBankFileID, CurrentUser = CurrentUser };
                    await _mediator.Send(command4);

                    TempData["Message"] = PopUpServices.Notify(ValueMapping.updatedsuc,ValueMapping.utr,notificationType: Alerts.success);

                    return RedirectToAction("Index");
                }
                return RedirectToAction("EnterUTR",bankFileUTRDetails.GenerateBankFileID);


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

        [HttpGet]
        public async Task<IActionResult> GetDefaultReocrds(string paymentStatus,string bankName)
        {
            try
            {
                var bankFileList = await _mediator.Send(new GetGeneratedBqankFileListRequest());
                IQueryable<GenerateBankFile> bankFile = bankFileList.AsQueryable();
                var res = bankFile.Where(x => x.CodeValue == paymentStatus);
                var aprovedAmount = res.Sum(x => x.TotalAmount);
                return Json(new { ApprovedAmount = aprovedAmount.ToString("0.00") });
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
        /// <param name="tdsPaidFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="allPages"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<IActionResult> ExportBankFileList(BankFileFilters bankFileFilters,string[] forder,GridParams gridParams)
        {
            try
            {

                var gridModel = await GetBankFileListToDonwload(gridParams,forder,bankFileFilters);
                var expColumns = new[]
                {
                   new ExpColumn { Name = "BankFileReferenceNO", Width = 1.5f, Header = "Bank FileReference No" },
                    new ExpColumn { Name = "CreatedOn", Width = 1.5f, Header = "Bank File Gen.Date" },
                    new ExpColumn { Name = "NoOfVendors", Width = 1.5f, Header = "No Of Vendors" },
                    new ExpColumn { Name = "NoOfTransactions", Width = 2.5f, Header = "No Of Transactions" },
                    new ExpColumn { Name = "TotalAmount", Width = 3, Header = "Total Amount" },
                    new ExpColumn { Name = "SourceBankName", Width = 2.2f, Header ="Bank Name" },
                    new ExpColumn { Name = "AccountNumber", Width = 2f, Header="Account Number" }

                };

                var workbook = (new GridExcelBuilder(expColumns)).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(),"application/vnd.ms-excel","BankFileList.xls");
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


        /// <summary>
        /// Author:Swetha M Date:15/11/2022
        /// Purpose:Get the Generated Bank File details
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> ViewBankFile(int id)
        {
            try
            {
                if (id != 0)
                {
                    var generatedBankFiles = await _mediator.Send(new GetGeneratedBankFilesByIDRequest { Id = id });
                    var generatedBankFile = _mapper.Map<List<MappingPaymentGenerateBankFileModel>>(generatedBankFiles);
                    var banks = await _mediator.Send(new GetBankMasterListRequest { });
                    var banksList = _mapper.Map<List<BankMasterModel>>(banks);
                    foreach (var item in generatedBankFile)
                    {
                        var bankName = banksList.Where(x => x.Id == item.GenerateBankFile.BankMasterId).Select(x => x.BankName).First();
                        var accountNo = banksList.Where(x => x.Id == item.GenerateBankFile.BankMasterId).Select(x => x.AccountNumber).First();
                        item.BankName = bankName;
                        item.AccountNumber = accountNo;
                    }
                    ViewBag.ID = id;
                    return View(resultViewPathBE + "Edit.cshtml",generatedBankFile);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getGenBankFile);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }

        }

        //US#002: Add the GetBankName()  method to bind the Dropdown in search filter.
        /// <summary>
        /// Author:Swetha M Date:27/01/2023
        /// Purpose: Get the list of Bank Name dropdown for  Filters
        /// </summary>
        /// <returns>Bank NameList</returns>
        public async Task<IActionResult> GetBankNameList()
        {
            try
            {
                //Get Created By List
                var banks = await _mediator.Send(new GetBankMasterListRequest { });
                var bankList = banks.DistinctBy(x => x.BankName).ToList();
                var createdbyQuery = new List<KeyContent>();
                createdbyQuery.AddRange(bankList.Select(o => new KeyContent(o.BankName,o.BankName)));
                return Json(createdbyQuery);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getCreatedByList);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }



        [HttpPost]
        public async Task<IActionResult> FileDownload(string fileName,int ID)
        {
            try
            {
                //Get Data from data store
                DownloadService<BankFileDto> dl = new();
                List<BankFileDto> listOfBills = new();
                listOfBills = await _mediator.Send(new GetGenerateBankFileRequestList { Id = ID });
                //Parse list data into byte array and generate the file            
                dl.ListItems = listOfBills;
                dl.FileName = fileName;
                FileContentResult fc = new FileContentResult(dl.GetFile(),dl.ContentType());
                fc.FileDownloadName = fileName.AddTimeStamp();
                return fc;
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.fileDownload);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }
        /// <summary>
        /// Author:Swetha M Date:09/05/2022
        /// Purpose: Get the list of Payment statuses and bind it Awesome grid along with
        /// </summary>
        /// <returns>Payment Status List</returns>
        public async Task<IActionResult> GetBankFileStatus(bool isIdKey = false)
        {
            try
            {
                //Get Payment Status
                var paymentStatusList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.pStatus });
                var paymentStatus = paymentStatusList.OrderBy(x => x.DisplaySequence).Where(x => x.CodeValue =="Bank File Generated" || x.CodeValue =="Paid");
                var paymentStatusQuery = new List<KeyContent>();
                paymentStatusQuery.AddRange(paymentStatus.Select(o => new KeyContent(isIdKey ? o.Id : o.CodeValue,o.CodeValue)).Distinct());
                return Json(paymentStatusQuery);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getPaymentStatus);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                throw;
            }

        }
    }
}
