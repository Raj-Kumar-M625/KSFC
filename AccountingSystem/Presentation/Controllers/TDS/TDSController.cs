using Application.DTOs.Document;
using Application.DTOs.Payment;
using Application.DTOs.TDS;
using Application.Interface.Persistence.Master;
using Application.UserStories.Bill.Requests.Queries;
using Application.UserStories.Document.Request.Command;
using Application.UserStories.Document.Request.Queries;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Payment.Requests.Queries;
using Application.UserStories.TDS.Requests.Commands;
using Application.UserStories.TDS.Requests.Queries;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Common.Helper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Bill;
using Domain.Payment;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NuGet.Packaging.Signing;
using Omu.Awem.Export;
using Omu.AwesomeMvc;
using Persistence;
using Presentation.AwesomeToolUtils;
using Presentation.GridFilters.TDS;
using Presentation.Helpers;
using Presentation.Models;
using Presentation.Models.Master;
using Presentation.Models.Payment;
using Presentation.Models.TDS;
using Presentation.Services.Infra.Cache;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Log = Serilog.Log;

namespace Presentation.Controllers.TDS
{


    /// <summary>
    /// Author:Monika K Y Date:06/07/2022
    /// Purpose:Controller for TDS
    /// </summary>
    /// <returns></returns>
    /// 
    [SessionTimeoutHandlerAttribute]

    public class TdsController:Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private IHostingEnvironment Environment;
        private readonly ICommonMasterRepository _commonMaster;
        protected string CurrentUser => HttpContext.User.Identity.Name;
        public TdsController(IMediator mediator,IMapper mapper,IUnitOfWork<AccountingDbContext> unitOfWork,IHostingEnvironment _environment, ICommonMasterRepository commonMaster)
        {
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _commonMaster = commonMaster;
            Environment = _environment;
        }

        #region TDS List
        ///<summary>
        /// Author:Monika K Y Date:06/07/2022
        /// Purpose: Get the list of TDS
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Breadcrumb(ValueMapping.TdsList)]
        public IActionResult Index()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }

        public async Task<FileResult> DownloadPaidChallan(int id)
        {
            try
            {
                var document = await _mediator.Send(new GetDocumentByRefIdRequest() { DocumentRefId = id });
                if (document != null)
                {
                    using (var fs = System.IO.File.OpenRead(document.FilePath))
                    {
                        using (var ms = new MemoryStream())
                        {
                            await fs.CopyToAsync(ms);
                            return File(ms.ToArray(),System.Net.Mime.MediaTypeNames.Application.Octet,document.Name + "." + document.Extension);
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.downloadPaidChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }
        public async Task<FileResult> DownloadPayableChallan(int id)
        {
            try
            {
                var paymentChallanList = await _mediator.Send(new GetTdsPaymentChallanByIdRequest() { Ids = new List<int>() { id } });
                var paymentChallan = paymentChallanList.FirstOrDefault();
                var fileName = "Payable_Challan.xlsx";
                var wwwPath = this.Environment.WebRootPath;
                var filePath = "" + wwwPath + "\\Files\\Payable Challan.xlsx " + "";
                var typeCode = paymentChallan.TDSSection;
                var tdsAmount = paymentChallan.TDSAmount;
                var interest = paymentChallan.Interest;
                var penalty = paymentChallan.Penalty;
                var totalTdsAmount = paymentChallan.TotalTDSPayment!;
                IWorkbook workbook = new XSSFWorkbook(filePath);
                var sheet = workbook.GetSheet("Income Tax Challan 281");
                if (sheet != null)
                {
                    var coreTypeRow = sheet.GetRow(17);
                    var codeArray = typeCode.ToCharArray();
                    var typeCodeColumnPosition = 21;
                    for (var i = 0;i < 4;i++)
                    {
                        var cell = coreTypeRow.GetCell(typeCodeColumnPosition);
                        cell.SetCellValue(Convert.ToString(codeArray[i]));
                        typeCodeColumnPosition++;
                    }
                    var tdsAmountRow = sheet.GetRow(24);
                    var tdsAmountCell = tdsAmountRow.Cells[8];
                    tdsAmountCell.SetCellValue(Convert.ToString(tdsAmount));

                    var interestRow = sheet.GetRow(27);
                    var interestCell = interestRow.Cells[8];
                    interestCell.SetCellValue(Convert.ToString(interest));

                    var penaltyRow = sheet.GetRow(28);
                    var penaltyCell = penaltyRow.Cells[8];
                    penaltyCell.SetCellValue(Convert.ToString(penalty));

                    var totalAmountRow = sheet.GetRow(29);
                    var totalAmountCell = totalAmountRow.Cells[8];
                    totalAmountCell.SetCellValue(Convert.ToString(totalTdsAmount));

                    var totalTdsAmountInWords = CurrencyInWordsHelper.ToVerbalCurrency(totalTdsAmount);
                    var totalAmountInWordsRow = sheet.GetRow(30);
                    var totalAmountInWordsCell = totalAmountInWordsRow.Cells[8];
                    totalAmountInWordsCell.SetCellValue(totalTdsAmountInWords);

                    var ones = totalTdsAmount % 10;
                    var tens = totalTdsAmount % 100 - ones;
                    var hundreds = totalTdsAmount % 1000 - tens - ones;
                    var thousands = totalTdsAmount % 1000000 - hundreds - tens - ones;
                    var lakhs = totalTdsAmount % 10000000 - thousands - hundreds - tens - ones;
                    var Crores = totalTdsAmount % 100000000 - lakhs - thousands - hundreds - tens - ones;
                    thousands = thousands / 1000;
                    lakhs = lakhs / 100000;
                    Crores = Crores / 10000000;
                    hundreds = hundreds / 100;
                    tens = tens / 10;


                    var totalAmountInwordsEachRow = sheet.GetRow(32);
                    var totalthousandAmountInwordsCell = totalAmountInwordsEachRow.Cells[8];
                    var totalAmountInwordsCell2 = totalAmountInwordsEachRow.Cells[13];
                    var totalAmountInwordsCell3 = totalAmountInwordsEachRow.Cells[1];
                    var totalAmountInwordsCell4 = totalAmountInwordsEachRow.Cells[5];
                    var totalAmountInwordsCell5 = totalAmountInwordsEachRow.Cells[18];
                    var totalAmountInwordsCell6 = totalAmountInwordsEachRow.Cells[21];

                    totalthousandAmountInwordsCell.SetCellValue(thousands.ToString());
                    totalAmountInwordsCell2.SetCellValue(hundreds.ToString());
                    totalAmountInwordsCell3.SetCellValue(Crores.ToString());
                    totalAmountInwordsCell4.SetCellValue(lakhs.ToString());
                    totalAmountInwordsCell5.SetCellValue(tens.ToString());
                    totalAmountInwordsCell6.SetCellValue(ones.ToString());
                    var notLocked = workbook.CreateCellStyle();
                    notLocked.IsLocked = false;
                    sheet.SetDefaultColumnStyle(0,notLocked);
                    sheet.ProtectSheet("");
                }
                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);

                    return File(stream.ToArray(),System.Net.Mime.MediaTypeNames.Application.Octet,fileName);
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.downloadPayableChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Author:Karthick J Date:18/08/2022
        /// Purpose: Get the TDS Status list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetTDSSectionList()
        {
            try
            {
                var tdsSectionList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.tdsSectionType });
                var tdsSectionQuery = new List<KeyContent>();
                tdsSectionQuery.AddRange(tdsSectionList.Select(o => new KeyContent(o.CodeName,o.CodeName)).Distinct());
                return Json(tdsSectionQuery);
            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.getTDSSectionList);
                Log.Information(ex.InnerException?.Message ?? string.Empty);
                Log.Information(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Author:Karthick J Date:18/08/2022
        /// Purpose: Get the TDS Status list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetTDSStatus(bool isIdKey = false)
        {
            try
            {
                var tdsStatusList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.tStatus });
                var tdsStatusQuery = new List<KeyContent>();
                tdsStatusQuery.AddRange(tdsStatusList.Select(o => new KeyContent(isIdKey ? o.Id : o.CodeValue,o.CodeValue)).Distinct());
                return Json(tdsStatusQuery);
            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.getTDSStatus);
                Log.Information(ex.InnerException?.Message ?? string.Empty);
                Log.Information(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Author:Karthick J Date:13/10/2022
        /// Purpose: Get the TDS Assessment Year list
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetTDSAssessmentYear()
        {
            try
            {
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });
                var assessmentYearQuery = new List<KeyContent>();
                assessmentYearQuery.AddRange(assessmentYearList.Select(o => new KeyContent(o.Id,o.CodeValue)).Distinct());
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

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Get the total payable amount along with filter search enabled
        /// </summary>
        /// <param name="tdsFilters"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetTotalTDSPayment1(TdsFilter tdsFilters,string[] forder)
        {
            var tdsQuery = await GetFilteredBills(tdsFilters,forder);
            var totalTDSPayment = tdsQuery.Sum(t => t.TDS);
            return Json(totalTDSPayment.ToString("0.00"));
        }

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Get the list of tds and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="tdsFilters"> </param>
        /// <returns>List of TDS based of filters</returns>
        public async Task<IActionResult> GetTDSList(TdsFilter tdsFilters,GridParams gridParams,string[] forder)
        {
            var result = await GetTDSListGridModel(tdsFilters,gridParams,forder);
            return Json(result.ToDto());
        }

        private async Task<IQueryable<Bills>> GetFilteredBills(TdsFilter tdsFilters,string[] forder)
        {
            try
            {
                forder = forder ?? new string[] { };

                var billList = await _mediator.Send(new GetBillsForGsttdsRequest());
                //var billsDtoList = _mapper.Map<List<Bills>>(billList.ToList());
                //var billsQuery = billList.Where(c => c.TDSStatus.StatusMaster.CodeValue == ValueMapping.pending).AsQueryable();
                var searchFilter = new TdsSearchFilters();
                var filterBuilder = searchFilter.GetTDSSearchCriteria(billList, tdsFilters);

                return await filterBuilder.ApplyAsync(billList, forder);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetFilteredBills);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        private async Task<GridModel<Bills>> GetTDSListGridModel(TdsFilter tdsFilters,GridParams gridParams,string[] forder)
        {
            var tdsQuery = await GetFilteredBills(tdsFilters,forder);
            try
            {
                // filter row search
                var frow = new TdsFilterRow();
                var gmb = new GridModelBuilder<Bills>(tdsQuery,gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = o => MapToGridModel(o),
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return await gmb.BuildModelAsync();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTDSListGridModel);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Bind the values to the awesome grid columns
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private object MapToGridModel(Bills o)
        {
            try
            {
                return new
                {
                    o.ID,
                    o.VendorId,
                    BillReferenceNo = o.BillReferenceNo,
                    BillDate = o.BillDate.ToString("dd-MM-yyyy"),
                    VendorName = o.Vendor == null ? string.Empty : o.Vendor.Name,
                    Pan_Number = o.Vendor == null ? string.Empty : o.Vendor.PAN_Number,                   
                    InvoiceAmount = o.BillAmount <0 ? "0" : o.BillAmount.ToString("0.00"),
                    TdsSection = o.Vendor?.VendorDefaults?.TDSSection == null ? string.Empty : o.Vendor.VendorDefaults.TDSSection,
                    TdsPayableAmount = o.TDS.ToString("0.00"),
                    TdsStatus = o.TDSStatus?.StatusMaster?.CodeName ?? string.Empty
                };
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.mapToGridModel);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Author:Monika K Y Date:07/07/2022
        /// Purpose:To Generate TDS Challan
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Breadcrumb(ValueMapping.genTDSChallan)]
        public async Task<IActionResult> GenerateTDSChallan(List<int> id,string assementYear)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var billList = await _mediator.Send(new GetBillListByIdRequest() { BillIds = id });
                //var tdsQuery = billList.AsQueryable();

                var Count = new TdsPaymentChallanViewModel
                {
                    NoOfVendors = billList.Select(b => b.VendorId).Distinct().Count(),
                    NoOfTrans = id.Count,
                    TDSAmount = billList.Sum(c => c.TDS),
                    TotalTDSPayment = billList.Sum(c => c.TDS),
                    TDSSection = billList.Select(o => o.Vendor.VendorDefaults.TDSSection).FirstOrDefault(),
                    BillsId = billList.Select(b => b.ID).ToList()
                };
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });
                ViewBag.assessmentYear = assessmentYearList;
                var assementCode = assessmentYearList.Where(x => x.CodeName == assementYear).Select(x => x.Id).FirstOrDefault();
                ViewBag.AssementCode = assementCode;
                var banks = await _mediator.Send(new GetBankMasterListRequest { });
                ViewBag.Banks = _mapper.Map<List<BankMasterModel>>(banks);
                ViewBag.UserName = CurrentUser;
                ViewBag.BillId = id.ToList();
                return PartialView(Count);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GenerateTDSChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Author:Karthick J Date:31/08/2022
        /// Purpose:To Generate TDS Challan
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Breadcrumb(ValueMapping.genTDSChallan)]
        public async Task<IActionResult> GenerateTDSChallan(TdsPaymentChallanViewModel tdsPaymentChallanViewModel,string billsId)
        {
            try
            {
                // ModelState.Remove(nameof(TDSPaymentChallanViewModel.BillsId));
                //if (ModelState.IsValid)
                //{
               
                var Ids = tdsPaymentChallanViewModel.Bank.BranchName.Split(',');
                tdsPaymentChallanViewModel.BankMasterID = Convert.ToInt32(Ids.ElementAt(1));

                //tdsPaymentChallanViewModel.BillsId = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(billsId);
                _unitOfWork.CreateTransaction();
                var tdsStatusList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.tStatus });
                tdsPaymentChallanViewModel.TDSStatus.Status = tdsStatusList.FirstOrDefault(t => t.CodeValue == ValueMapping.ChallanCreated)?.Id ?? 0;
                var quarterList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.quarter });
                tdsPaymentChallanViewModel.Quarter = quarterList.FirstOrDefault(t => t.CodeValue == DateTime.Today.ToString("MMMM"))?.Id ?? 0;

                var tdsPaymentChallanDto = _mapper.Map<Application.DTOs.Payment.TdsPaymentChallanDto>(tdsPaymentChallanViewModel);
                var tdsPaymentChallanCommand = new GenerateTdsPaymentChallanCommand { tdsPaymentChallan = tdsPaymentChallanDto,user = CurrentUser };
                 await _mediator.Send(tdsPaymentChallanCommand);
                TempData["Message"] = PopUpServices.Notify(ValueMapping.TdsChallan,"TDS section : " + tdsPaymentChallanViewModel.TDSSection + "",notificationType: Alerts.success);
                return RedirectToAction("TDSChallanList");
                //}             
                //else
                //{
                //    return View(tdsPaymentChallanViewModel);
                //}
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GenerateTDSChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>        
        /// Purpose: Export Tds List
        /// Author: Karthick; Date: 27/08/2022        
        /// </summary>
        /// <param name="tdsFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="allPages"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<IActionResult> ExportTdsList(TdsFilter tdsFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                //if (allPages.HasValue && allPages.Value)
                //{
                //    gridParams.Paging = false;
                //}

                var gridModel = await GetTDSListGridModel(tdsFilters,gridParams,forder);
                var expColumns = new[]
                {
                    //new ExpColumn { Name = "Id", Width = 1.5f, Header = "Id" },
                    new ExpColumn { Name = "BillReferenceNo", Width = 1.5f, Header = "Bill Ref No" },
                    new ExpColumn { Name = "BillDate", Width = 2.5f, Header = "Bill Date (dd-MM-yyyy)" },
                    new ExpColumn { Name = "VendorName", Width = 3, Header = "Vendor Name" },
                    new ExpColumn { Name = "Pan_Number", Width = 2.2f, Header ="PAN No" },
                    new ExpColumn { Name = "InvoiceAmount", Width = 2f, Header="Invoice Amount" },
                    new ExpColumn { Name = "TdsSection", Width = 3, Header = "TDS Section" },
                    new ExpColumn { Name = "TdsPayableAmount", Width = 3.2f, Header = "TDS Payable Amount" },
                    new ExpColumn { Name = "TdsStatus", Width = 2.7f, Header = "TDS Status" }
                };

                var workbook = (new GridExcelBuilder(expColumns) { }).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(),"application/vnd.ms-excel","TdsPayableList.xls");
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.exportTdsList);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        #endregion
        #region ChallanList

        ///<summary>
        /// Author:Monika K Y Date:07/07/2022
        /// Purpose: Get the TDS Challan list 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Breadcrumb(ValueMapping.TdsChallanList)]
        public IActionResult TDSChallanList()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }

        public async Task<IActionResult> GetBanks()
        {
            var banksDto = await _mediator.Send(new GetBankMasterListRequest { });
            var banks = _mapper.Map<List<BankMasterModel>>(banksDto);

            var banksQuery = new List<KeyContent>();
            banksQuery.AddRange(banks.DistinctBy(b => b.BankName).Select(b => new KeyContent(b.BankName, b.BankName)));
            return Json(banksQuery);
        }

        public async Task<IActionResult> GetAccountNumber(string[] bankNames)
        {
            bankNames = bankNames ?? new string[] { };
            var banksDto = await _mediator.Send(new GetBankMasterListRequest { });
            var banks = _mapper.Map<List<BankMasterModel>>(banksDto);

            var accountNumbersQuery = new List<KeyContent>();
            accountNumbersQuery.AddRange(banks.Where(b => bankNames.Contains(b.BankName)).Select(b => new KeyContent(b.AccountNumber, b.AccountNumber)));
            return Json(accountNumbersQuery);
        }

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Get the total TDS Challan payable amount along with filter search enabled
        /// </summary>
        /// <param name="challanFilters"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetTotalTDSChallanPayableAmount(TdsChallanFilter challanFilters,string[] forder)
        {
            try
            {
                var challanQuery = await GetFilteredTdsPaymentChallan(challanFilters,forder);
                var totalTDSPayment = challanQuery.Sum(t => t.TDSPaymentChallan.TotalTDSPayment);
                return Json(totalTDSPayment.ToString("0.00"));
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTotalTDSChallanPayableAmount);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Get the list of tds challans and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="challanFilters"> </param>
        /// <returns>List of TDS based of filters</returns>
        public async Task<IActionResult> GetTDSChallanList(TdsChallanFilter challanFilters,GridParams gridParams,string[] forder)
        {
            var challanList = await GetTdsPaymentChallanGridModel(challanFilters,gridParams,forder);
            return Json(challanList.ToDto());
        }

        private async Task<GridModel<Application.DTOs.TDS.TdssPaymentChallanDto>> GetTdsPaymentChallanGridModel(TdsChallanFilter challanFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                var frow = new TdsChallanFilterRow();
                var challanQuery = await GetFilteredTdsPaymentChallan(challanFilters,forder);
                var challnlist = challanQuery.Distinct().AsEnumerable();

                var gmb = new GridModelBuilder<Application.DTOs.TDS.TdssPaymentChallanDto>(challnlist.AsQueryable(), gridParams)
                {
                    KeyProp = o => o.Id,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return await gmb.BuildModelAsync();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTdsPaymentChallanGridModel);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        private async Task<IQueryable<Application.DTOs.TDS.TdssPaymentChallanDto>> GetFilteredTdsPaymentChallan(TdsChallanFilter challanFilters,string[] forder)
        {
            try
            {
                forder = forder ?? new string[] { };

                var challanList = await _mediator.Send(new GetTdsPaymentChallanListRequest());
                var challanQuery = challanList.Where(c => c.CodeValue == ValueMapping.ChallanCreated);

                var searchFilter = new TdsChallanSearchFilters();
                var filterBuilder = searchFilter.GetTDSChallanSearchCriteria(challanList,challanFilters);

                return await filterBuilder.ApplyAsync(challanQuery,forder);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetFilteredTdsPaymentChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Bind the values to the awesome grid columns
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private object MapToGridModel(Application.DTOs.TDS.TdssPaymentChallanDto o)
        {
            try
            {
                return new
                {
                    o.Id,
                    Quarter = o.TDSPaymentChallan.QuarterMaster == null ? string.Empty : o.TDSPaymentChallan.QuarterMaster.CodeName,
                    TotalTdsPayment = o.TDSPaymentChallan.TotalTDSPayment.ToString("0.00"),
                    TdsStatus = o.CodeName,
                    AssessmentYear = o.TDSPaymentChallan.AssementYear,
                    CreatedOn = o.TDSPaymentChallan.CreatedOn.ToString("dd-MM-yyyy"),
                    TdsSection = string.IsNullOrEmpty(o.TDSPaymentChallan.TDSSection) ? "-" : o.TDSPaymentChallan.TDSSection,
                    NoOfVendors = o.TDSPaymentChallan.NoOfVendors,
                    NoOfTransactions = o.TDSPaymentChallan.NoOfTrans,
                    ChallanDate = o.TDSPaymentChallan.TDSChallanDate,
                    BankMasterId = o.TDSPaymentChallan.BankMasterID,
                    BankName = o.Bank == null ? string.Empty : o.Bank.BankName,
                    AccountNo = o.Bank == null ? string.Empty : o.Bank.Accountnumber,
                    ChallanNo = string.IsNullOrEmpty(o.TDSPaymentChallan.ChallanNo) ? "-" : o.TDSPaymentChallan.ChallanNo,
                    UtrNo = string.IsNullOrEmpty(o.TDSPaymentChallan.UTRNo) ? "-" : o.TDSPaymentChallan.UTRNo
                };
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.mapToGridModel);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        [HttpGet]
        [Breadcrumb(ValueMapping.enterTdsUTR)]
        public async Task<IActionResult> UpdateTDSPaymentChallan(string id)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var paymentChallanQuery = await _mediator.Send(new GetTdsPaymentChallanByIdRequest() { Ids = new List<int> { Convert.ToInt32(id) } });
                var paymentChallan = paymentChallanQuery.FirstOrDefault(p => p.Id == Convert.ToInt32(id));
                var viewModel = new UpdateChallanViewModel
                {
                    TotalTDSAmount = paymentChallan.TotalTDSPayment,
                    Id = paymentChallan.Id
                };
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });
                var challanList = await _mediator.Send(new GetTdsPaymentChallanListRequest());
                var banks = await _mediator.Send(new GetBankMasterListRequest { });
                var selectedBank = _mapper.Map<BankMasterModel>(banks.Where(x => x.Id == paymentChallan.BankMasterID).FirstOrDefault());
                viewModel.IsBulkTDS = selectedBank.IsBulkTDS;
                ViewBag.assessmentYear = assessmentYearList;
                ViewBag.ID = id;
                ViewBag.UserName = CurrentUser;
                ViewBag.UTRNo = challanList.Select(x => x.TDSPaymentChallan.UTRNo).ToList();
                ViewBag.ChallanNo = challanList.Select(x => x.TDSPaymentChallan.ChallanNo).ToList();
                ViewBag.BSRCode = challanList.Select(x => x.TDSPaymentChallan.BSRCode).ToList();

                return PartialView(viewModel);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.UpdateTDSPaymentChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        private async Task<int> AddTDSDocuments(DocumentsModel documents)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var document = _mapper.Map<DocumentsDto>(documents);
                var documentid = document.DocumentRefID;
                document.EntityType = ValueMapping.TDS;
                var command = new AddDocumentCommand { documents = document,id = documentid,entityPath = ValueMapping.tdsChallanEntityPath,entityType = ValueMapping.TDS };

                var documentcount = await _mediator.Send(command);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return documentcount;
            }
            catch (Exception)
            {
                Log.Error("Add TDS Documents failed");
                _unitOfWork.Rollback();
                return 0;
            }

        }

        [HttpPost]
        [Breadcrumb(ValueMapping.enterTdsUTR)]
        public async Task<IActionResult> UpdateTDSPaymentChallan(UpdateChallanViewModel updateChallanViewModel,IFormFile File)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var documentCount = await AddTDSDocuments(new DocumentsModel() { DocumentRefID = updateChallanViewModel.Id, files = new List<IFormFile>() { updateChallanViewModel.File } });
                    if (documentCount == 0)
                    {
                        ModelState.AddModelError("File", "Could not upload tds document. Please try again");
                        var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });
                        ViewBag.assessmentYear = assessmentYearList;
                        return PartialView(updateChallanViewModel);
                    }
                    
                    _unitOfWork.CreateTransaction();
                    var paymentChallanList = await _mediator.Send(new GetTdsPaymentChallanByIdRequest() { Ids = new List<int>() { updateChallanViewModel.Id } });
                    var paymentChallan = paymentChallanList.FirstOrDefault();
                    await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.tStatus });
                    var tdsStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.tStatus);
                    if (paymentChallan != null)
                    {
                        paymentChallan.BSRCode = updateChallanViewModel.BSRCode;
                        paymentChallan.ChallanNo = updateChallanViewModel.ChallanNo;
                        paymentChallan.PaymentDate = updateChallanViewModel.PaymentDate;
                        paymentChallan.TDSChallanDate = updateChallanViewModel.TenderDate;
                        paymentChallan.TenderDate = updateChallanViewModel.TenderDate;
                        paymentChallan.UTRNo = updateChallanViewModel.UTRNo;
                        paymentChallan.TDSStatus.StatusCMID = tdsStatusList.Where(x => x.CodeValue == ValueMapping.paid).First().Id;
                        paymentChallan.AssementYear = updateChallanViewModel.AssessmentYear;
                        paymentChallan.ModifiedOn = DateTime.UtcNow;
                        paymentChallan.ModifiedBy = CurrentUser;
                    }
                    paymentChallan.IsBulkTDS = updateChallanViewModel.IsBulkTDS;
					var tdsPaymentChallanCommand = new UpdateTdsPaymentChallanCommand { tdsPaymentChallan = new List<TdsPaymentChallan>() { paymentChallan }, user = CurrentUser };
					await _mediator.Send(tdsPaymentChallanCommand);

					var tdsTransactionSummary = new AddTDSTransactionSummaryCommand { currentUser = CurrentUser, tdsPaymentChallan = paymentChallan };
                    await _mediator.Send(tdsTransactionSummary);

                    var tdsTransaction = new AddPaidTransactionCommand { Id = paymentChallan.Id, tdsPaymentChallan = new List<TdsPaymentChallan>() { paymentChallan }, CurrentUser = CurrentUser, UTR = updateChallanViewModel.UTRNo };
                    await _mediator.Send(tdsTransaction);

                  
                    _unitOfWork.Save();
                    _unitOfWork.Commit();
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.UTRDetails,"UTR No: " + updateChallanViewModel.UTRNo + "",notificationType: Alerts.success);
                    return RedirectToAction("TDSQuarterlyPaidList");
                }
                else
                {
                    return View(updateChallanViewModel);
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.UpdateTDSPaymentChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>        
        /// Purpose: Export Tds Challan List
        /// Author: Karthick; Date: 27/08/2022        
        /// </summary>
        /// <param name="challanFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="allPages"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<IActionResult> ExportTdsChallanList(TdsChallanFilter challanFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                var gridModel = await GetTdsPaymentChallanGridModel(challanFilters,gridParams,forder);
                var expColumns = new[]
                {
                    new ExpColumn { Name = "CreatedOn", Width = 1.5f, Header = "Challan Date" },
                    new ExpColumn { Name = "TdsSection", Width = 1.5f, Header = "TDS Section" },
                    new ExpColumn { Name = "NoOfVendors", Width = 2.5f, Header = "No. of Vendors" },
                    new ExpColumn { Name = "NoOfTransactions", Width = 3, Header = "No. of Transaction" },
                    new ExpColumn { Name = "BankName", Width = 2.2f, Header ="Bank Name" },
                    new ExpColumn { Name = "AccountNo", Width = 2f, Header="Account No." },
                    new ExpColumn { Name = "ChallanNo", Width = 3, Header = "Challan No." },
                    new ExpColumn { Name = "UtrNo", Width = 3.2f, Header = "UTR No." },
                    new ExpColumn { Name = "TotalTdsPayment", Width = 2.7f, Header = "TDS Payable Amount" },
                    new ExpColumn { Name = "TdsStatus", Width = 3f, Header = "Status"}
                };
                var workbook = (new GridExcelBuilder(expColumns)).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(),"application/vnd.ms-excel","TdsPayableList.xls");
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.exportTdsChallanList);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        #endregion

        #region TDS Quarterly Paid List

        /// <summary>
        /// Author:Karthick J Date:10/07/2022
        /// Purpose: Get the TDS Quarterly Paid list 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Breadcrumb(ValueMapping.TdsQuarterlyPaidList)]
        public IActionResult TDSQuarterlyPaidList()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QuarterlyFiling(string ids)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var Id = JsonConvert.DeserializeObject<List<int>>(ids);
                var statusList = await _mediator.Send(new GetCommonMasterListRequest() { CodeType = ValueMapping.tStatus });

                var paymentChallanQuery = await _mediator.Send(new GetTdsPaymentChallanByIdRequest() { Ids = Id });
                if (paymentChallanQuery != null)
                {
                    var quarterlyTdsPaymentChallan = new QuarterlyTdsPaymentChallanDto
                    {
                        Quarter = paymentChallanQuery.FirstOrDefault().Quarter,
                        NoOfChallan = paymentChallanQuery.Count(),
                        TotalAmount = paymentChallanQuery.Sum(p => p.TotalTDSPayment),
                        AssementYear = paymentChallanQuery.FirstOrDefault().AssementYear,
                        QuarterStatus = statusList.FirstOrDefault(s => s.CodeValue == ValueMapping.CSVPending).Id,
                    };

                    await _mediator.Send(new CreateQuarterlyTdsPaymentChallanCommand() { Ids = Id,quarterlyTdsPaymentChallan = quarterlyTdsPaymentChallan,user = CurrentUser });
                    await paymentChallanQuery.ForEachAsync(q =>
                    {
                        q.TDSStatus.StatusCMID = statusList.First(s => s.CodeValue == "CSVPending").Id;
                        q.ModifiedBy = CurrentUser;
                        q.ModifiedOn = DateTime.UtcNow;
                    });
                    await _mediator.Send(new UpdateTdsPaymentChallanCommand() { tdsPaymentChallan = paymentChallanQuery.ToList(),user = CurrentUser });
                    ViewBag.UserName = CurrentUser;
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.QuarterlyFiling);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Author: Karthick J; Date: 13/10/2022
        /// Purpose: Get the total TDS Quarterly paid amount along with filter search enabled
        /// </summary>
        /// <param name="tdsQuarterlyPaidListFilters"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetTotalTDSQuarterlyPaidAmount(TdsQuarterlyPaidListFilter tdsQuarterlyPaidListFilters,string[] forder)
        {
            try
            {
                var tdsQuarterlyPaidQuery = await GetFilteredTdsPaymentChallan(tdsQuarterlyPaidListFilters,forder);
                var totalTDSPayment = tdsQuarterlyPaidQuery.Sum(t => t.TDSPaymentChallan.TotalTDSPayment);
                return Json(totalTDSPayment.ToString("0.00"));
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTotalTDSQuarterlyPaidAmount);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }


        /// <summary>
        /// Author: Karthick J; Date: 13/10/2022
        /// Purpose: Get the list of tds quarterly filing challans and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="tdsQuarterlyPaidListFilters"> </param>
        /// <returns>List of TDS Quarterly Filing Challans based of filters</returns>
        public async Task<IActionResult> GetTDSQuarterlyPaidList(TdsQuarterlyPaidListFilter tdsQuarterlyPaidListFilters,GridParams gridParams,string[] forder)
        {
            var tdsQuarterlyPaidQuery = await GetTdsQuarterlyPaidGridModel(tdsQuarterlyPaidListFilters,gridParams,forder);
            return Json(tdsQuarterlyPaidQuery.ToDto());
        }

        private async Task<GridModel<Application.DTOs.TDS.TdssPaymentChallanDto>> GetTdsQuarterlyPaidGridModel(TdsQuarterlyPaidListFilter tdsQuarterlyPaidListFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                var tdsQuarterlyFilingQuery = await GetFilteredTdsPaymentChallan(tdsQuarterlyPaidListFilters,forder);
                var gmb = new GridModelBuilder<Application.DTOs.TDS.TdssPaymentChallanDto>(tdsQuarterlyFilingQuery.AsQueryable(), gridParams)
                {
                    KeyProp = o => o.Id,
                    Map = MapToGridModel,
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return await gmb.BuildModelAsync();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTdsQuarterlyPaidGridModel);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        private async Task<IQueryable<Application.DTOs.TDS.TdssPaymentChallanDto>> GetFilteredTdsPaymentChallan(TdsQuarterlyPaidListFilter tdsQuarterlyPaidListFilters,string[] forder)
        {
            try
            {
                forder = forder ?? new string[] { };

                var challanList = await _mediator.Send(new GetTdsPaymentChallanListRequest());
                var challanQuery = challanList.Where(c => c.CodeValue == ValueMapping.Paid);
                var searchFilter = new TdsQuarterlyPaidListSearchFilters();
                var filterBuilder = searchFilter.GetTDSQuarterlyPaidSearchCriteria(challanList,tdsQuarterlyPaidListFilters);
                return await filterBuilder.ApplyAsync(challanQuery,forder);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetFilteredTdsPaymentChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        /// <summary>        
        /// Purpose: Export Tds Quarterly Paid List
        /// Author: Karthick; Date: 19/10/2022        
        /// </summary>
        /// <param name="tdsQuarterlyPaidListFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="allPages"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<IActionResult> ExportTdsQuarterlyPaidList(TdsQuarterlyPaidListFilter tdsQuarterlyPaidListFilters,GridParams gridParams,string[] forder)
        {
            try
            {

                //if (allPages.HasValue && allPages.Value)
                //{
                //    gridParams.Paging = false;
                //}

                var gridModel = await GetTdsQuarterlyPaidGridModel(tdsQuarterlyPaidListFilters,gridParams,forder);
                var expColumns = new[]
                {
                    new ExpColumn { Name = "CreatedOn", Width = 1.5f, Header = "Challan Date" },
                    new ExpColumn { Name = "TdsSection", Width = 1.5f, Header = "TDS Section" },
                    new ExpColumn { Name = "NoOfVendors", Width = 2.5f, Header = "No. of Vendors" },
                    new ExpColumn { Name = "NoOfTransactions", Width = 3, Header = "No. of Transaction" },
                    new ExpColumn { Name = "BankName", Width = 2.2f, Header ="Bank Name" },
                    new ExpColumn { Name = "AccountNo", Width = 2f, Header="Account No." },
                    new ExpColumn { Name = "ChallanNo", Width = 3, Header = "Challan No." },
                    new ExpColumn { Name = "UtrNo", Width = 3.2f, Header = "UTR No." },
                    new ExpColumn { Name = "TotalTdsPayment", Width = 2.7f, Header = "TDS Payable Amount" },
                    new ExpColumn { Name = "TdsStatus", Width = 3f, Header = "Status"}
                };

                var workbook = (new GridExcelBuilder(expColumns)).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(),"application/vnd.ms-excel","TdsQuarterlyPaidList.xls");
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.exportTdsQuarterlyPaidList);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }
        #endregion

        #region TDS Quarterly List

        /// <summary>
        /// Author:Karthick J Date:19/09/2022
        /// Purpose: Get the TDS Quarterly list 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Breadcrumb(ValueMapping.TdsQuarterlyList)]
        public IActionResult TDSQuarterlyList()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }

        public async Task<IActionResult> GetNumberOfChallans()
        {
            var quarterlyQuery = new List<KeyContent>();
            quarterlyQuery.AddRange(Enumerable.Range(1,10).Select(o => new KeyContent(o,o.ToString())));
            return Json(quarterlyQuery);
        }

        /// <summary>
        /// Author: Karthick J; Date: 14/10/2022
        /// Purpose: Get the total TDS Quarterly list payable amount along with filter search enabled
        /// </summary>
        /// <param name="tdsQuarterlyListFilters"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetTotalTDSQuarterlyListPayableAmount(TdsQuarterlyListFilter tdsQuarterlyListFilters,string[] forder)
        {
            try
            {
                var tdsQuarterlyListQuery = await GetFilteredQuarterlyTdsPaymentChallan(tdsQuarterlyListFilters,forder);
                var totalTDSPayment = tdsQuarterlyListQuery.Sum(t => t.TotalAmount);
                return Json(totalTDSPayment.ToString("0.00"));
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTotalTDSQuarterlyListPayableAmount);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }


        /// <summary>
        /// Author: Karthick J; Date: 14/10/2022
        /// Purpose: Get the list of tds quarterly list and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="tdsQuarterlyListFilters"> </param>
        /// <returns>List of TDS Quarterly List based of filters</returns>
        public async Task<IActionResult> GetTDSQuarterlyList(TdsQuarterlyListFilter tdsQuarterlyListFilters,GridParams gridParams,string[] forder)
        {
            var tdsQuarterlyListQuery = await GetTdsQuarterlyListGridModel(tdsQuarterlyListFilters,gridParams,forder);
            return Json(tdsQuarterlyListQuery.ToDto());
        }

        private async Task<GridModel<QuarterlyTdsPaymentChallan>> GetTdsQuarterlyListGridModel(TdsQuarterlyListFilter tdsQuarterlyListFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                var tdsQuarterlyListQuery = await GetFilteredQuarterlyTdsPaymentChallan(tdsQuarterlyListFilters,forder);
                var gmb = new GridModelBuilder<QuarterlyTdsPaymentChallan>(tdsQuarterlyListQuery.AsQueryable(),gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = o => MapToGridModel(o,null),
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return await gmb.BuildModelAsync();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTdsQuarterlyListGridModel);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        private async Task<IQueryable<QuarterlyTdsPaymentChallan>> GetFilteredQuarterlyTdsPaymentChallan(TdsQuarterlyListFilter tdsQuarterlyListFilters,string[] forder)
        {
            try
            {
                forder = forder ?? new string[] { };

                var challanQuery = await _mediator.Send(new GetQuarterlyTdsPaymentChallanRequest());
                challanQuery = challanQuery.Where(c => c.QuarterStatusMaster.CodeValue == ValueMapping.CSVPending || c.QuarterStatusMaster.CodeValue == ValueMapping.CSVCreated);
                var searchFilter = new TdsQuarterlyListSearchFilters();
                var filterBuilder = searchFilter.GetTDSQuarterlyListSearchCriteria(challanQuery,tdsQuarterlyListFilters);

                return await filterBuilder.ApplyAsync(challanQuery,forder);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetFilteredQuarterlyTdsPaymentChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Author: Karthick J; Date: 14/10/2022
        /// Purpose: Bind the values to the awesome grid columns
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private object MapToGridModel(QuarterlyTdsPaymentChallan o,List<MappingTdsQuarterChallan> mappingTDSQuarterChallans)
        {
            try
            {
                return new
                {
                    Id = o.ID,
                    EditorAttr = o.QuarterStatusMaster.CodeValue == ValueMapping.CSVPending ? "disabled" : string.Empty,
                    Quarter = o.QuarterMaster == null ? string.Empty : o.QuarterMaster.CodeName,
                    AssementYear = o.AssementYearMaster.CodeName,
                    NoOfChallan = o.NoOfChallan,
                    NoOfVendors = mappingTDSQuarterChallans == null ? 0 : mappingTDSQuarterChallans.Where(m => m.QuarterlyTDSPaymentChallanID == o.ID).Sum(m => m.TDSPaymentChallan.NoOfVendors),
                    TotalAmount = o.TotalAmount.ToString("0.00"),
                    QuarterlyStatus = o.QuarterStatusMaster.CodeName
                };
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.mapToGridModel);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        public async Task<IActionResult> UpdateQuarterlyTDSChallanStatus(int id)
             
        {
           
            var statusList = await _mediator.Send(new GetCommonMasterListRequest() { CodeType = ValueMapping.tStatus });
            //var paymentChallanQuery = await _mediator.Send(new GetTDSPaymentChallanByIdRequest() { Ids = new List<int>() { id } });
            var quarterlyPaymentChallanQuery = await _mediator.Send(new GetQuarterlyTdsPaymentChallanByIdRequest() { ids = new List<int>() { id } });
            var quarterlyPaymentChallanList = quarterlyPaymentChallanQuery.ToList();
            foreach (var challan in quarterlyPaymentChallanList)
            {
                challan.QuarterStatus = statusList.First(s => s.CodeValue == ValueMapping.CSVCreated).Id;
            }
          
            var quarterlyTdsPaymentChallanDtoList = _mapper.Map<List<QuarterlyTdsPaymentChallanDto>>(quarterlyPaymentChallanList);
            var updateQuarterTdsPaymentChallanCommand = new UpdateQuarterlyTdsPaymentChallanCommand { QuarterlyTDSPaymentChallanList = quarterlyTdsPaymentChallanDtoList,user = CurrentUser };
           
            await _mediator.Send(updateQuarterTdsPaymentChallanCommand);
            return Json(true);
        }

        public async Task<FileResult> DownloadCSVForChallan(int id)
        {
            var quarterPaymentChallan = await _mediator.Send(new GetMappingQuarterlyTdsPaymentChallanByTdsChallanIdRequest() { Id = id });
            return await DownloadCSV(quarterPaymentChallan.QuarterlyTDSPaymentChallanID);
        }

        public async Task<FileResult> DownloadCSV(int id)
        {
            try
            {
                var quarterlyTdsPaymentChallanList = await _mediator.Send(new GetQuarterlyTdsPaymentChallanByIdRequest() { ids = new List<int>() { id } });
                var quarterlyPaymentChallan = quarterlyTdsPaymentChallanList.FirstOrDefault();
                var fileName = "Quarterly_CSV.xlsx";
                var wwwPath = this.Environment.WebRootPath;
                var filePath = "" + wwwPath + "\\Files\\TDS_CSV.xlsx " + "";

                var quarterList = await _mediator.Send(new GetCommonMasterListRequest() { CodeType = ValueMapping.quarter });
                await _mediator.Send(new GetCommonMasterListRequest() { CodeType = ValueMapping.assementYear });
                var quarterLastMonth = quarterList.Where(q => q.CodeName == quarterlyPaymentChallan.QuarterMaster.CodeName).OrderBy(q => q.DisplaySequence).LastOrDefault().CodeValue;
                var assessmentYearValues = quarterlyPaymentChallan.AssementYearMaster.CodeValue.Split('-');
                var assessmentYearStart = assessmentYearValues[0];
                var assessmentYearEnd = assessmentYearValues[1];
                var financialYearStart = Convert.ToInt32(assessmentYearStart) - 1;
                var financialYearEnd = Convert.ToInt32(assessmentYearEnd) - 1;

                var mappingQuarterlyTdsPaymentChallanList = await _mediator.Send(new GetMappingQuarterlyTdsPaymentChallanRequest() { Ids = new List<int>() { id } });
                var tdsPaymentChallanList = mappingQuarterlyTdsPaymentChallanList.Select(m => m.TDSPaymentChallan);
                var billTdsPaymentList = await _mediator.Send(new GetBillTdsPaymentListByChallanIdRequest() { ChallanIds = tdsPaymentChallanList.Select(t => t.Id).ToList() });

                IWorkbook workbook = new XSSFWorkbook(filePath);
                var formSheet = workbook.GetSheet("Form");
                if (formSheet != null)
                {
                    var quarterEndMonthRow = formSheet.GetRow(5);
                    var quarterEndMonthCell = quarterEndMonthRow.GetCell(22);
                    quarterEndMonthCell.SetCellValue(quarterLastMonth);

                    var quarterEndYearRow = formSheet.GetRow(5);
                    var quarterEndYearCell = quarterEndYearRow.GetCell(28);
                    quarterEndYearCell.SetCellValue(financialYearStart);

                    var financialYearRow = formSheet.GetRow(7);
                    var financialYearCell = financialYearRow.GetCell(32);
                    financialYearCell.SetCellValue(string.Join('-',financialYearStart,financialYearEnd));

                    var assessmentYearRow = formSheet.GetRow(9);
                    var assessmentYearCell = assessmentYearRow.GetCell(32);
                    assessmentYearCell.SetCellValue(string.Join('-',assessmentYearStart,assessmentYearEnd));
                }

                var challanSheet = workbook.GetSheet("Challan");
                var annexureSheet = workbook.GetSheet("Annexure-I");

                if (challanSheet != null)
                {
                    var rowPosition = 6;
                    var totalRowPosition = 65;
                    var serialNo = 1;
                    var annexureRowPosition = 10;
                    var annexureSerialNo = 1;

                    foreach (var tdsPaymentChallan in tdsPaymentChallanList)
                    {
                        var challanRow = challanSheet.GetRow(rowPosition);

                        var serialNoCell = challanRow.GetCell(0);
                        serialNoCell.SetCellValue(serialNo);

                        var sectionCodeCell = challanRow.GetCell(1);
                        sectionCodeCell.SetCellValue(tdsPaymentChallan.TDSSection);

                        var tdsAmountCell = challanRow.GetCell(2);
                        tdsAmountCell.SetCellValue(Convert.ToDouble(tdsPaymentChallan.TDSAmount));

                        var surchargeCell = challanRow.GetCell(3);
                        surchargeCell.SetCellValue(Convert.ToDouble(tdsPaymentChallan.Penalty));

                        var interestCell = challanRow.GetCell(5);
                        interestCell.SetCellValue(Convert.ToDouble(tdsPaymentChallan.Interest));

                        var totalTdsPaymentCell = challanRow.GetCell(8);
                        totalTdsPaymentCell.SetCellValue(Convert.ToDouble(tdsPaymentChallan.TotalTDSPayment));

                        var bsrCodeCell = challanRow.GetCell(11);
                        bsrCodeCell.SetCellValue(tdsPaymentChallan.BSRCode);

                        var dateOnWhichTaxDeposited = challanRow.GetCell(13);
                        dateOnWhichTaxDeposited.SetCellValue(tdsPaymentChallan.PaymentDate.Value);


                        //if (tdsPaymentChallan.TenderDate.HasValue)
                        //{
                        //    ICell tenderDateCell = challanRow.GetCell(13);
                        //    tenderDateCell.SetCellValue(tdsPaymentChallan.TenderDate.Value);
                        //}

                        var challanNoCell = challanRow.GetCell(15);
                        challanNoCell.SetCellValue(tdsPaymentChallan.ChallanNo);

                        var interest2Cell = challanRow.GetCell(17);
                        interest2Cell.SetCellValue(Convert.ToDouble(tdsPaymentChallan.Interest));

                        var totalTaxDepositCell = challanRow.GetCell(20);
                        totalTaxDepositCell.SetCellValue(Convert.ToDouble(tdsPaymentChallan.TotalTDSPayment));

                        rowPosition++;
                        serialNo++;

                        var bills = billTdsPaymentList.Where(b => b.TDSPaymentChallanID == tdsPaymentChallan.Id).Select(b => b.Bill);
                        foreach (var bill in bills)
                        {
                            var billRow = annexureSheet.GetRow(annexureRowPosition);

                            var annexureChallanNoCell = billRow.GetCell(0);
                            annexureChallanNoCell.SetCellValue(tdsPaymentChallan.ChallanNo);

                            var annexureBSRCodeCell = billRow.GetCell(1);
                            annexureBSRCodeCell.SetCellValue(tdsPaymentChallan.BSRCode);

                            if (tdsPaymentChallan.PaymentDate.HasValue)
                            {
                                var annexureTenderDateCell = billRow.GetCell(2);
                                annexureTenderDateCell.SetCellValue(tdsPaymentChallan.PaymentDate.Value);

                                var annexureTenderDate2Cell = billRow.GetCell(14);
                                annexureTenderDate2Cell.SetCellValue(tdsPaymentChallan.PaymentDate.Value);

                                var annexureTenderDate3Cell = billRow.GetCell(24);
                                annexureTenderDate3Cell.SetCellValue(tdsPaymentChallan.PaymentDate.Value);
                            }

                            var annexureChallanNo2Cell = billRow.GetCell(3);
                            annexureChallanNo2Cell.SetCellValue(tdsPaymentChallan.ChallanNo);

                            var annexureTdsSectionCell = billRow.GetCell(4);
                            annexureTdsSectionCell.SetCellValue(tdsPaymentChallan.TDSSection);

                            var totTDS = billRow.GetCell(5);
                            totTDS.SetCellValue(Convert.ToDouble(bill.TDS));

                            var totpen = billRow.GetCell(6);
                            totpen.SetCellValue(Convert.ToDouble(tdsPaymentChallan.Interest));
                            var totint = billRow.GetCell(7);
                            totint.SetCellValue(Convert.ToDouble(tdsPaymentChallan.Penalty));

                            var annexureTotalTdsPaymentCell = billRow.GetCell(8);
                            annexureTotalTdsPaymentCell.SetCellValue(Convert.ToDouble(tdsPaymentChallan.TotalTDSPayment));

                            var annexureSerialNoCell = billRow.GetCell(9);
                            annexureSerialNoCell.SetCellValue(annexureSerialNo);

                            var annexureDeducteeCodeCell = billRow.GetCell(10);
                            var vendorName = bill.Vendor.Name.ToLower();
                            annexureDeducteeCodeCell.SetCellValue((vendorName.Contains("private limited") || vendorName.Contains("limited") || vendorName.Contains("pvt ltd")) ? "01" : "02");

                            var annexureVendorPanCell = billRow.GetCell(12);
                            annexureVendorPanCell.SetCellValue(bill.Vendor.PAN_Number);

                            var annexureVendorNameCell = billRow.GetCell(13);
                            annexureVendorNameCell.SetCellValue(bill.Vendor.Name);

                            var annexureBillTotalCell = billRow.GetCell(15);
                            annexureBillTotalCell.SetCellValue(Convert.ToDouble(bill.TotalBillAmount));

                            var annexureTdsAmountCell = billRow.GetCell(17);
                            annexureTdsAmountCell.SetCellValue(Convert.ToDouble(bill.TDS));

                            var annexureTdsAmount2Cell = billRow.GetCell(20);
                            annexureTdsAmount2Cell.SetCellValue(Convert.ToDouble(bill.TDS));

                            var annexureTdsAmount3Cell = billRow.GetCell(22);
                            annexureTdsAmount3Cell.SetCellValue(Convert.ToDouble(bill.TDS));

                            var annexureTdsPercentageCell = billRow.GetCell(25);
                            annexureTdsPercentageCell.SetCellValue(Convert.ToDouble(bill.Vendor.VendorDefaults.TDSPercentage));

                            annexureSerialNo++;
                            annexureRowPosition++;
                        }
                    }
                    var challanTotalRow = challanSheet.GetRow(totalRowPosition);

                    var sumTdsAmountCell = challanTotalRow.GetCell(2);
                    sumTdsAmountCell.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.TDSAmount)));

                    var sumSurchargeCell = challanTotalRow.GetCell(3);
                    sumSurchargeCell.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.Penalty)));

                    var sumInterestCell = challanTotalRow.GetCell(5);
                    sumInterestCell.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.Interest)));

                    var sumTotalTdsPaymentCell = challanTotalRow.GetCell(8);
                    sumTotalTdsPaymentCell.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.TotalTDSPayment)));

                    var sumInterest2Cell = challanTotalRow.GetCell(17);
                    sumInterest2Cell.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.Interest)));

                    var sumTotalTaxDepositCell = challanTotalRow.GetCell(20);
                    sumTotalTaxDepositCell.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.TotalTDSPayment)));

                    var annexureTotalRowPosition = 115;
                    var annexureTotalRow = annexureSheet.GetRow(annexureTotalRowPosition);

                    var sumAnnexureTotalTds = annexureTotalRow.GetCell(5);
                    sumAnnexureTotalTds.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.TDSAmount)));

                    var sumAnnexureTotalInterest = annexureTotalRow.GetCell(6);
                    sumAnnexureTotalInterest.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.Interest)));

                    var sumAnnexureTotalPenality = annexureTotalRow.GetCell(7);
                    sumAnnexureTotalPenality.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.Penalty)));

                    var sumAnnexureTotalTdsPaymentCell = annexureTotalRow.GetCell(8);
                    sumAnnexureTotalTdsPaymentCell.SetCellValue(Convert.ToDouble(tdsPaymentChallanList.Sum(t => t.TotalTDSPayment)));

                    var sumAnnexureBillTotalCell = annexureTotalRow.GetCell(15);
                    sumAnnexureBillTotalCell.SetCellValue(Convert.ToDouble(billTdsPaymentList.Sum(b => b.Bill.TotalBillAmount)));

                    var sumAnnexureTdsAmountCell = annexureTotalRow.GetCell(17);
                    sumAnnexureTdsAmountCell.SetCellValue(Convert.ToDouble(billTdsPaymentList.Sum(b => b.Bill.TDS)));

                    var sumAnnexureTdsAmount2Cell = annexureTotalRow.GetCell(20);
                    sumAnnexureTdsAmount2Cell.SetCellValue(Convert.ToDouble(billTdsPaymentList.Sum(b => b.Bill.TDS)));

                    var sumAnnexureTdsAmount3Cell = annexureTotalRow.GetCell(22);
                    sumAnnexureTdsAmount3Cell.SetCellValue(Convert.ToDouble(billTdsPaymentList.Sum(b => b.Bill.TDS)));

                    var notLocked = workbook.CreateCellStyle();
                    notLocked.IsLocked = false;
                    challanSheet.SetDefaultColumnStyle(0,notLocked);
                    challanSheet.ProtectSheet("");
                    annexureSheet.SetDefaultColumnStyle(0,notLocked);
                    annexureSheet.ProtectSheet("");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);

                    return File(stream.ToArray(),System.Net.Mime.MediaTypeNames.Application.Octet,fileName);
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.downloadPayableChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }


        #endregion

        #region TDS Quarterly Filing

        /// <summary>
        /// Author:Karthick J Date:19/09/2022
        /// Purpose: Get the TDS Quarterly Filing list 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Breadcrumb(ValueMapping.TdsQuarterlyFiledList)]
        public IActionResult TDSQuarterlyFiledList()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }

        public async Task<IActionResult> GetQuarterly()
        {
            var tdsStatusList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.quarter });
            var quarterlyQuery = new List<KeyContent>();
            quarterlyQuery.AddRange(tdsStatusList.GroupBy(o => o.CodeName).Select(o => new KeyContent(o.Key,o.Key)).Distinct());
            return Json(quarterlyQuery);
        }

        /// <summary>
        /// Author: Karthick J; Date: 19/09/2022
        /// Purpose: Get the total TDS Quarterly filed payable amount along with filter search enabled
        /// </summary>
        /// <param name="quarterlyFiledFilters"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetTotalTDSQuarterlyFiledPayableAmount(TdsQuarterlyFiledFilter quarterlyFiledFilters,string[] forder)
        {
            try
            {
                var tdsQuarterlyFiledQuery = await GetFilteredQuarterlyTdsPaymentChallan(quarterlyFiledFilters,forder);
                var totalTDSPayment = tdsQuarterlyFiledQuery.Sum(t => t.TotalAmount);
                return Json(totalTDSPayment.ToString("0.00"));
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTotalTDSQuarterlyFiledPayableAmount);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }


        /// <summary>
        /// Author: Karthick J; Date: 19/09/2022
        /// Purpose: Get the list of tds quarterly filing challans and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="tdsQuarterlyFiledFilters"> </param>
        /// <returns>List of TDS Quarterly Filing Challans based of filters</returns>
        public async Task<IActionResult> GetTDSQuarterlyFiledList(TdsQuarterlyFiledFilter tdsQuarterlyFiledFilters,GridParams gridParams,string[] forder)
        {
            var tdsQuarterlyFiledQuery = await GetTdsQuarterlyFiledGridModel(tdsQuarterlyFiledFilters,gridParams,forder);
            return Json(tdsQuarterlyFiledQuery.ToDto());
        }

        private async Task<GridModel<QuarterlyTdsPaymentChallan>> GetTdsQuarterlyFiledGridModel(TdsQuarterlyFiledFilter tdsQuarterlyFilingFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                var tdsQuarterlyFiledQuery = await GetFilteredQuarterlyTdsPaymentChallan(tdsQuarterlyFilingFilters,forder);
                var mappingQuarterlyTdsPaymentChallan = await _mediator.Send(new GetMappingQuarterlyTdsPaymentChallanRequest() { Ids = tdsQuarterlyFiledQuery.Select(t => t.ID).ToList() });
                var gmb = new GridModelBuilder<QuarterlyTdsPaymentChallan>(tdsQuarterlyFiledQuery.AsQueryable(),gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = o => MapToGridModel(o,mappingQuarterlyTdsPaymentChallan.ToList()),
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return await gmb.BuildModelAsync();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTdsQuarterlyFiledGridModel);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        private async Task<IQueryable<QuarterlyTdsPaymentChallan>> GetFilteredQuarterlyTdsPaymentChallan(TdsQuarterlyFiledFilter tdsQuarterlyFilingFilters,string[] forder)
        {
            try
            {
                forder = forder ?? new string[] { };

                var challanList = await _mediator.Send(new GetQuarterlyTdsPaymentChallanRequest());
                var challanQuery = challanList.Where(c => c.QuarterStatusMaster.CodeValue == ValueMapping.QtyFiled).AsQueryable();

                var searchFilter = new TdsQuarterlyFiledSearchFilters();
                var filterBuilder = searchFilter.GetTDSQuarterlyFiledSearchCriteria(challanList,tdsQuarterlyFilingFilters);

                return await filterBuilder.ApplyAsync(challanQuery,forder);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetFilteredQuarterlyTdsPaymentChallan);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }


        [HttpGet]
        [Breadcrumb(ValueMapping.updateTdsTRN)]
        public async Task<IActionResult> UpdateTraceReferenceNo(List<int> id)
        {
            try
            {
                var quarterlyPaymentChallanQuery = await _mediator.Send(new GetQuarterlyTdsPaymentChallanByIdRequest() { ids = id });

                var viewModel = new UpdateTdsQuarterlyFilingViewModel
                {
                    Ids = id,
                    NoOfTrans = id.Count,
                    TotalPaidAmount = quarterlyPaymentChallanQuery.Sum(x => x.TotalAmount)
                };
                ViewBag.UserName = CurrentUser;
                return PartialView(viewModel);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.UpdateTDSTraceReferenceNumber);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        [HttpPost]
        [Breadcrumb(ValueMapping.updateTdsTRN)]
        public async Task<IActionResult> UpdateTraceReferenceNo(UpdateTdsQuarterlyFilingViewModel updateTdsQuarterlyFilingViewModel,string ids)
        {
            try
            {
                ModelState.Remove(nameof(UpdateTdsQuarterlyFilingViewModel.Ids));
                if (ModelState.IsValid)
                {
                    _unitOfWork.CreateTransaction();
                    updateTdsQuarterlyFilingViewModel.Ids = JsonConvert.DeserializeObject<List<int>>(ids);
                    var quarterlyPaymentChallanQuery = await _mediator.Send(new GetQuarterlyTdsPaymentChallanByIdRequest() { ids = updateTdsQuarterlyFilingViewModel.Ids });
                    var statusList = await _mediator.Send(new GetCommonMasterListRequest() { CodeType = ValueMapping.tStatus });
                    var quarterlyPaymentChallanList = quarterlyPaymentChallanQuery.ToList();
                    foreach (var challan in quarterlyPaymentChallanList)
                    {
                        challan.DateOfFiling = updateTdsQuarterlyFilingViewModel.DateOfFiling;
                        challan.TracesReferenceNo = updateTdsQuarterlyFilingViewModel.TracesReferenceNumber;
                        challan.TotalPaidAmount = updateTdsQuarterlyFilingViewModel.TotalPaidAmount;
                        challan.NoOfTrans = updateTdsQuarterlyFilingViewModel.NoOfTrans;
                        challan.QuarterStatus = statusList.FirstOrDefault(s => s.CodeValue == ValueMapping.QtyFiled).Id;

                    }
                    var quarterlyTdsPaymentChallanDtoList = _mapper.Map<List<QuarterlyTdsPaymentChallanDto>>(quarterlyPaymentChallanList);
                    var updateQuarterTdsPaymentChallanCommand = new UpdateQuarterlyTdsPaymentChallanCommand { QuarterlyTDSPaymentChallanList = quarterlyTdsPaymentChallanDtoList,user = CurrentUser };
                    await _mediator.Send(updateQuarterTdsPaymentChallanCommand);

                    TempData["Message"] = PopUpServices.Notify(ValueMapping.TDSQuarterFilling,"Traces Reference No: " + updateTdsQuarterlyFilingViewModel.TracesReferenceNumber + "",notificationType: Alerts.success);

                    return RedirectToAction("TDSQuarterlyFiledList");
                }
                else
                {
                    return RedirectToAction("UpdateTraceReferenceNo",new { id = ids });
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.UpdateTDSTraceReferenceNumber);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        #endregion

        #region TDS Paid List

        ///<summary>
        /// Author:Monika K Y Date:06/07/2022
        /// Purpose: Get the TDS Paid list 
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Breadcrumb(ValueMapping.TdsPaidList)]
        public IActionResult TDSPaidList()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Get the total TDS Challan payable amount along with filter search enabled
        /// </summary>
        /// <param name="tdsPaidfilters"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetTotalTDSPaidAmount(TdsPaidFilter tdsPaidfilters,string[] forder)
        {
            try
            {
                var tdsBillPaymentQuery = await GetFilteredTdsPaidList(tdsPaidfilters,forder);
                var totalTDSPaidAmount = tdsBillPaymentQuery.Sum(t => t.TDSPaymentChallan.TotalTDSPayment);
                return Json(totalTDSPaidAmount.ToString("0.00"));
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTotalTDSPaidAmount);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }


        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Get the list of tds challans and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="tdsPaidfilters"> </param>
        /// <returns>List of TDS based of filters</returns>
        public async Task<IActionResult> GetTDSPaidList(TdsPaidFilter tdsPaidfilters,GridParams gridParams,string[] forder)
        {
            var tdsPaidListQuery = await GetTdsPaidListGridModel(tdsPaidfilters,gridParams,forder);
            return Json(tdsPaidListQuery.ToDto());
        }

        private async Task<GridModel<BillTdsPayment>> GetTdsPaidListGridModel(TdsPaidFilter tdsPaidFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                var frow = new TdsPaidFilterRow();
                var tdsBillPaymentQuery = await GetFilteredTdsPaidList(tdsPaidFilters,forder);
                var gmb = new GridModelBuilder<BillTdsPayment>(tdsBillPaymentQuery,gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = o => MapToGridModel(o),
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return await gmb.BuildModelAsync();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTDSPaidList);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        private async Task<IQueryable<BillTdsPayment>> GetFilteredTdsPaidList(TdsPaidFilter tdsPaidFilters,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                forder = forder ?? new string[] { };

                var tdsBillPaymentQuery = await _mediator.Send(new GetBillTdsPaymentListRequest());
                tdsBillPaymentQuery = tdsBillPaymentQuery.Where(t => t.Bill.TDSStatus.StatusMaster.CodeValue == ValueMapping.QtyFiled).AsQueryable();
                TdsPaidSearchFilters searchFilter = new TdsPaidSearchFilters();
                var filterBuilder = searchFilter.GetTDSPaidSearchCriteria(tdsBillPaymentQuery,tdsPaidFilters);

                return await filterBuilder.ApplyAsync(tdsBillPaymentQuery,forder);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTDSPaidList);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Bind the values to the awesome grid columns
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private object MapToGridModel(BillTdsPayment o)
        {
            try
            {
                return new
                {
                    o.ID,
                    BillNumber = o.Bill?.BillNo ?? String.Empty,
                    BillReferenceNo = o.Bill?.BillReferenceNo ?? string.Empty,
                    PaymentDate = o.TDSPaymentChallan.PaymentDate.HasValue ? o.TDSPaymentChallan.PaymentDate.Value.ToString("dd-MM-yyyy") : "-",
                    BsrCode = o.TDSPaymentChallan?.BSRCode != null ? o.TDSPaymentChallan.BSRCode : "-",
                    UtrNo = o.TDSPaymentChallan?.UTRNo != null ? o.TDSPaymentChallan.UTRNo : "-",
                    VendorName = o.Bill?.Vendor?.Name,
                    PanNumber = o.Bill?.Vendor?.PAN_Number,
                    BillAmount = o.Bill?.BillAmount.ToString("0.00"),
                    TdsSection = string.IsNullOrEmpty(o.TDSPaymentChallan.TDSSection) ? "-" : o.TDSPaymentChallan.TDSSection,
                    PaidAmount = o.TDSPaymentChallan?.TotalTDSPayment != null ? o.TDSPaymentChallan.TotalTDSPayment : 0,
                    ChallanNo = o.TDSPaymentChallan?.ChallanNo != null ? o.TDSPaymentChallan.ChallanNo : "-",
                    TdsStatus = o.TDSPaymentChallan?.TDSStatus.StatusMaster?.CodeName ?? string.Empty,
                    ChallanId = o.TDSPaymentChallan.Id
                };
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.mapToGridModel);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                throw;
            }
        }

        /// <summary>        
        /// Purpose: Export Tds Paid List
        /// Author: Karthick; Date: 27/08/2022        
        /// </summary>
        /// <param name="tdsPaidFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="allPages"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<IActionResult> ExportTdsPaidList(TdsPaidFilter tdsPaidFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                //if (allPages.HasValue && allPages.Value)
                //{
                //    gridParams.Paging = false;
                //}

                var gridModel = await GetTdsPaidListGridModel(tdsPaidFilters,gridParams,forder);
                var expColumns = new[]
                {
                    new ExpColumn { Name = "BillReferenceNo", Width = 1.5f, Header = "Bill Ref No." },
                    new ExpColumn { Name = "PaymentDate", Width = 1.5f, Header = "Pay Date (dd-MM-yyyy)" },
                    new ExpColumn { Name = "VendorName", Width = 2.5f, Header = "Vendor Name" },
                    new ExpColumn { Name = "PanNumber", Width = 3, Header = "PAN No." },
                    new ExpColumn { Name = "BillAmount", Width = 2.2f, Header ="Bill Amount" },
                    new ExpColumn { Name = "TdsSection", Width = 2f, Header="TDS Section" },
                    new ExpColumn { Name = "PaidAmount", Width = 3, Header = "TDS Paid Amount" },
                    new ExpColumn { Name = "ChallanNo", Width = 3.2f, Header = "Challan No." },
                    new ExpColumn { Name = "BsrCode", Width = 2.7f, Header = "BSR Code" },
                    new ExpColumn { Name = "UtrNo", Width = 2.7f, Header = "UTR No." },
                    new ExpColumn { Name = "TdsStatus", Width = 3f, Header = "TDS Status"}
                };

                var workbook = (new GridExcelBuilder(expColumns)).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(),"application/vnd.ms-excel","TdsPaidList.xls");
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.exportTdsPaidList);
                Log.Information(e.InnerException?.Message ?? string.Empty);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        #endregion
    }
}
