using Application.DTOs.Bank;
using Application.DTOs.Bill;
using Application.DTOs.Document;
using Application.DTOs.Payment;
using Application.UserStories.Bank.Requests.Commands;
using Application.UserStories.Bank.Requests.Queries;
using Application.UserStories.Bill.Requests.Commands;
using Application.UserStories.Document.Request.Command;
using Application.UserStories.Document.Request.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Common.Downloads;
using Common.Helper;
using Common.InputSearchCriteria;
using DocumentFormat.OpenXml.Presentation;
using Domain.Bank;
using Domain.Uploads;
using Domain.Vendor;
using EllipticCurve.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Omu.AwesomeMvc;
using Persistence;
using Persistence.Repositories.Generic;
using Presentation.GridFilters.Reconcile;
using Presentation.Helpers;
using Presentation.Models.Bank;
using Presentation.Services.Infra.Cache;
using Serilog;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers.Bank
{
    [Authorize]
    [SessionTimeoutHandlerAttribute]

    public class BankController : Controller
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private GenericRepository<BankStatementInputTransaction> repository;
        protected string CurrentUser => this.HttpContext.User.Identity.Name;

        public BankController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            repository = new GenericRepository<BankStatementInputTransaction>(unitOfWork);
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.UserName = CurrentUser;
                return View();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.indexM);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }

       
        public async Task<IActionResult> BankList(BankStatementsSearchCriteriaModel searchCriteria)
        {
            var bankStatementsSearchCriteria = _mapper.Map<GenericInputSearchCriteria>(searchCriteria);

            var request = new GetBankStatementsFilterListRequest { searchCriteria = bankStatementsSearchCriteria };
            var response = await _mediator.Send(request);


            ViewBag.data = _mapper.Map<List<BankStatementsModel>>(response);
            ViewBag.UserName = CurrentUser;
            return View("Index", searchCriteria);
        }
        public async Task<IActionResult> GetBankStatements( GridParams gridParams)
        {
            try
            {
                var list = await _mediator.Send(new GetBankStatementInputListRequest());
                //var bankStatementList = list.ToList();
                //var result = _mapper.Map<List<BankStatementInputDto>>(bankStatementList);
                var gmb = new GridModelBuilder<BankStatementInput>(list, gridParams)
                {
                    KeyProp = o => o.Id,
                    Map = MapToGridModel,
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return Json(await gmb.BuildAsync());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Purpose: Bind the values to the awesome grid columns
        /// Author: Manoj; Date: 18/04/2023
        /// ModifiedBy:
        /// ModifiedPurpose:
        /// </summary>
        /// <param name="o"></param>
        private object MapToGridModel(BankStatementInput o)
        {
            try
            {
                return new
                {
                    o.Id,
                    StartDate = o.StartDate.ToString("dd-MM-yyyy"),
                    EndDate = o.EndDate.ToString("dd-MM-yyyy"),
                    NoOfTransaction = o.NoOfTransaction,
                    Remarks = o.Remarks??"",
                    FileName = o.FileName??"",
                    UploadedDate = o.CreatedDate.ToString("dd-MM-yyyy"),
                    TotalCreditAmount = o.TotalCreditAmount == null ? "" : o.TotalCreditAmount?.ToString("0.00"),
                    TotalDebitAmount = o.TotalDebitAmount == null ? "" : o.TotalDebitAmount?.ToString("0.00"),
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task<ActionResult> DownloadBankStatement(int id)
        {
            try
            {
                var document = await _mediator.Send(new GetBankStatementDocRequest() { DocumentRefId = id });
                if (document != null)
                {

                    if (System.IO.File.Exists(document.FilePath))
                    {
                        using (var fs = System.IO.File.OpenRead(document.FilePath))
                        {
                            using (var ms = new MemoryStream())
                            {
                                await fs.CopyToAsync(ms);
                                return File(ms.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, document.Name + "." + document.Extension);
                            }
                        }
                    }
                    else
                    {
                        TempData["Message"] = PopUpServices.Notify(ValueMapping.DNE, "Uploaded Bank File", notificationType: Alerts.warning);
                        return RedirectToAction("UploadBankStatement");
                    }

                    
                }
                return RedirectToAction("UploadBankStatement"); ;
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.downloadPaidChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }
        public async Task<IActionResult> UploadBankStatement()
        {
            try
            {
                ViewBag.UserName = CurrentUser;
                return View();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.indexM);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UploadBankStatementInput(UploadBankStatementsModel data)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ViewBag.UserName = CurrentUser;
            try
            {
                _unitOfWork.CreateTransaction();
                using (var package = new ExcelPackage(data.File.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    decimal totalDebit = 0;
                    decimal totalCredit = 0;
                    int NoOfDebitCount = 0;
                    int NoOCreditCount = 0;
                    List<BankStatementsModel> bankStatements = new List<BankStatementsModel>();
                    int lastRowWithData = 0;
                    for (int row = worksheet.Dimension.End.Row; row >= 1; row--)
                    {
                        bool rowHasData = false;

                        for (int col = worksheet.Dimension.Start.Column; col <= worksheet.Dimension.End.Column; col++)
                        {
                            var cellValue = worksheet.Cells[row, col].Value;
                            if (cellValue != null)
                            {
                                rowHasData = true;
                                break;
                            }
                        }

                        if (rowHasData)
                        {
                            lastRowWithData = row;
                            break;
                        }
                    }
                    for (int row = 2; row <= lastRowWithData; row++)
                    {
                        //Validation For Transaction Date
                        ExcelRange transactionCell = worksheet.Cells[row, 1];
                        if (!DateTime.TryParse(transactionCell.Value.ToString(), out DateTime transactionDateValue))
                        {
                            TempData["Message"] = PopUpServices.Notify("Transaction Date Column DataType MisMatch Please Check", "Error", notificationType: Alerts.error);
                            return View("UploadBankStatement");
                        }
                        //Validation For Value Date
                        ExcelRange valueCell = worksheet.Cells[row, 2];
                        if (!DateTime.TryParse(valueCell.Value.ToString(), out DateTime valueDateValue))
                        {
                            TempData["Message"] = PopUpServices.Notify("Value Date Column DataType MisMatch Please Check", "Error", notificationType: Alerts.error);
                            return View("UploadBankStatement");
                        }
                        if (worksheet.Cells[row, 8].Value != null)
                        {
                            // Read debit amount
                            if (worksheet.Cells[row, 8].Value is double debitAmount)
                            {
                                totalDebit += (decimal)debitAmount;
                                NoOfDebitCount++;
                            }
                            else
                            {
                                TempData["Message"] = PopUpServices.Notify("Debit Column DataType MisMatch Please Check", "Error", notificationType: Alerts.error);
                                return View("UploadBankStatement");
                            }
                        }
                        if (worksheet.Cells[row, 9].Value != null)
                        {
                            // Read credit amount
                            if (worksheet.Cells[row, 9].Value is double creditAmount)
                            {
                                totalCredit += (decimal)creditAmount;
                                NoOCreditCount++;
                            }
                            else
                            {
                                TempData["Message"] = PopUpServices.Notify("Credit Column DataType MisMatch Please Check", "Error", notificationType: Alerts.error);
                                return View("UploadBankStatement");
                            }
                        }
                        BankStatementsModel bankStatement = new BankStatementsModel()
                        {
                            Transaction_Date = transactionDateValue,
                            Value_Date = valueDateValue,
                            RefNo_ChequeNo = worksheet.Cells[row, 3].Value !=null ? worksheet.Cells[row, 3].Value?.ToString() : "",
                            Description = worksheet.Cells[row, 4].Value!=null ? worksheet.Cells[row, 4].Value?.ToString() :"",
                            Branch_Code = worksheet.Cells[row, 5].Value !=null ? long.Parse(worksheet.Cells[row, 5].Value?.ToString()) : 0,
                            Transaction_Mnemonic = worksheet.Cells[row, 6].Value != null ? worksheet.Cells[row, 6].Value?.ToString() : "",
                            Transaction_Literal = worksheet.Cells[row, 7].Value != null ? worksheet.Cells[row, 7].Value?.ToString() : "",
                            Debit = worksheet.Cells[row, 8].Value == null ? 0 : Convert.ToDecimal(worksheet.Cells[row, 8].Value),
                            Credit = worksheet.Cells[row, 9].Value == null ? 0 : Convert.ToDecimal(worksheet.Cells[row, 9].Value),
                            Balance = worksheet.Cells[row, 10].Value == null ? 0 : Convert.ToDecimal(worksheet.Cells[row, 10].Value),
                            AccountNo = long.Parse(worksheet.Cells[row, 11].Value?.ToString()),
                            BankName = worksheet.Cells[row, 12].Value?.ToString(),
                            FileName = worksheet.Cells[row, 13].Value?.ToString(),
                            IsDuplicate = false,
                            IsJunk = false,
                            IsProcessed = false,
                            IsSuccess = false
                        };
                        bankStatements.Add(bankStatement);
                    }
                    string roundedTotalDebit = Math.Round(totalDebit, 2).ToString("F0");
                    string roundedTotalCredit = Math.Round(totalCredit, 2).ToString("F0");
                    if (Math.Round(decimal.Parse(roundedTotalDebit), 2) == Math.Round(data.TotalDebitAmount, 2))
                    {
                        if (Math.Round(decimal.Parse(roundedTotalCredit), 2) == Math.Round(data.TotalCreditAmount, 2))
                        {
                            //Save in bankStatementInput
                            BankStatementInputDto bankStatementInput = new BankStatementInputDto()
                            {
                                FileName = data.File.FileName,
                                NoOfTransaction = bankStatements.Count(),
                                TotalCreditAmount = (decimal.Parse(roundedTotalCredit)),
                                TotalDebitAmount = (decimal.Parse(roundedTotalDebit)),
                                TotalCreditTransaction = NoOCreditCount,
                                TotalDebitTransaction = NoOfDebitCount,
                                NoOfProcessedTransaction = 0,
                                NoOfDuplicateTransaction = 0,
                                NoOfJunkTransaction = 0,
                                NoOfSuccessTransaction = 0,
                                StartDate = data.StartDate,
                                EndDate = data.EndDate,
                                Remarks = data.Remarks,
                                //UniqueFileName = new Guid(),
                                CreatedDate = DateTime.Now,
                                CreatedBy = CurrentUser
                            };
                            var bankInpuCommand = new CreateBankStatementInputCommand { bankStatementInput = bankStatementInput, user = CurrentUser };
                            var response = await _mediator.Send(bankInpuCommand);

                            //Save in BankStatementInputTransaction
                            var banktransaction = _mapper.Map<List<BankStatementsListDto>>(bankStatements);
                            banktransaction.ForEach(b => b.BankStatementInputId = response.Id);
                            var command = new CreateBankStatementCommand { bankStatements = banktransaction, user = CurrentUser };
                            var bills = await _mediator.Send(command);


                            //Save in Documents
                            List<IFormFile> files = new List<IFormFile>();
                            // Get an IFormFile object from somewhere
                            IFormFile dataFile = data.File;
                            // Add the IFormFile object to the list
                            files.Add(dataFile);
                            DocumentsDto document = new DocumentsDto()
                            {
                                DocumentRefID = response.Id,
                                Name = data.File.Name,
                                FileType = data.File.ContentType,
                                Extension = data.File.ContentType,
                                Description = "Upload Bank File",
                                files = files,
                                UploadedBy = CurrentUser,
                            };
                            var doc = new AddDocumentCommand { documents = document, id = response.Id, entityPath = ValueMapping.BankStatementEntityPath, entityType = ValueMapping.UploadBankStatement, user = CurrentUser, };
                            await _mediator.Send(doc);
                        }
                        else
                        {
                            TempData["Message"] = PopUpServices.Notify("Credit value is not Matching Please Check", "Error", notificationType: Alerts.error);
                            return View("UploadBankStatement");
                        }
                    }
                    else
                    {
                        TempData["Message"] = PopUpServices.Notify("Debit value is not Matching Please Check", "Error", notificationType: Alerts.error);
                        return View("UploadBankStatement");
                    }
                }
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return View("UploadBankStatement");
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.indexM);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }

    }
}
