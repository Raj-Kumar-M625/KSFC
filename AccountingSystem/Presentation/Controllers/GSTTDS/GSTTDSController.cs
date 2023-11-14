using Application.DTOs.GSTTDS;
using Application.UserStories.Bill.Requests.Queries;
using Application.UserStories.GSTTDS.Requests.Commands;
using Application.UserStories.GSTTDS.Requests.Queries;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Common.Helper;
using Domain.Bill;
using Domain.GSTTDS;
using Domain.Master;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Omu.Awem.Export;
using Omu.AwesomeMvc;
using Persistence;
using Presentation.AwesomeToolUtils;
using Presentation.GridFilters.GSTTDS;
using Presentation.GridFilters.TDS;
using Presentation.Helpers;
using Presentation.Models.GSTTDS;
using Presentation.Models.Master;
using Presentation.Models.Payment;
using Presentation.Services.Infra.Cache;
using Serilog;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers.GSTTDS
{
    /// <summary>
    /// Author:Swetha M Date:07/07/2022
    /// Purpose: GSTTDS Controller
    /// </summary>
    /// <returns></returns>
    [SessionTimeoutHandlerAttribute]

    public class GsttdsController:Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        protected string CurrentUser => HttpContext.User.Identity.Name;
        /// <summary>
        /// Author:Swetha M Date:07/07/2022
        /// Purpose: Constructor for the GST-TDS Controller
        /// </summary>
        /// <returns></returns>
        public GsttdsController(IMediator mediator,IMapper mapper,IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #region GST-TDS List
        /// <summary>
        /// Author:Swetha M Date:07/07/2022
        /// Purpose: Returns View to display list GST-TDS File List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Breadcrumb(ValueMapping.GstTdsList)]
        public IActionResult Index()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }

        /// <summary>   
        /// Author:Karthick J Date:12/09/2022
        /// Purpose: Get the TDS Status list
        /// Modified BY:SK
        /// Modified On:05-11-2022
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetTDSStatus(bool isIdKey = false)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var tdsStatusList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.gstTdsStatus });
                var tdsStatusQuery = new List<KeyContent>();
                tdsStatusQuery.AddRange(tdsStatusList.Select(o => new KeyContent(isIdKey ? o.Id : o.CodeValue,o.CodeValue)).Distinct());
                return Json(tdsStatusQuery);
            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.getTDSStatus);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Author:Karthick J Date:26/09/2022
        /// Purpose: Get the GST TDS Status list
        /// Modified BY:SK
        /// Modified On:05-11-2022
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetGSTTDSStatus(bool isIdKey = false)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var tdsStatusList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.gstTdsStatus });
                //var tdsStatusQuery = new List<KeyContent> { new KeyContent(string.Empty, ValueMapping.gstTdsStatus) };

                var tdsStatusQuery = new List<KeyContent>();
                tdsStatusQuery.AddRange(tdsStatusList.Select(o => new KeyContent(isIdKey ? o.Id : o.CodeValue,o.CodeValue)).Distinct());
                return Json(tdsStatusQuery);
            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.getGSTTDSStatus);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Get the total payable amount along with filter search enabled
        /// </summary>
        /// <param name="gsttdsFilters"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetTotalGSTTDSPayment(GstTdsFilter gsttdsFilters,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                forder = forder ?? new string[] { };
                var billList = await _mediator.Send(new GetBillsForGsttdsRequest());
                var gsttdsQuery = billList.Where(c => c.GSTTDSStatus.StatusMaster.CodeValue == ValueMapping.pending).AsQueryable();
                GstTdsSearchFilters searchFilter = new GstTdsSearchFilters();
                var filterBuilder = searchFilter.GetGSTTDSSearchCriteria(gsttdsQuery,gsttdsFilters);

                gsttdsQuery = await filterBuilder.ApplyAsync(gsttdsQuery,forder);
                var totalTDSPayment = gsttdsQuery.Sum(t => t.GSTTDS);
                return Json(totalTDSPayment.ToString("0.00"));
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTotalGSTTDSPayment);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }





        /// <summary>
        /// Author:SK Date 07-11-2022
        /// Purpose:Get the GST TDS List
        /// </summary>
        /// <param name="gsttdsFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetGSTTDSList(GstTdsFilter gsttdsFilters,GridParams gridParams,string[] forder)
        {
            var result = await GetGSTTDSListGridModel(gsttdsFilters,gridParams,forder);
            return Json(result.ToDto());
        }


        /// <summary>
        /// Author :SK Date 05-11-2022
        /// Purpose:Bind the data to the list in awesome grid
        /// </summary>
        /// <param name="gsttdsFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        private async Task<GridModel<Bills>> GetGSTTDSListGridModel(GstTdsFilter gsttdsFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                forder = forder ?? new string[] { };

                var billList = await _mediator.Send(new GetBillsForGsttdsRequest());

                // var gsttdsQuery = billList.Where(c => c.GSTTDSStatus.StatusMaster.CodeValue == ValueMapping.pending).AsQueryable();

                // filter row search
                var frow = new GstTdsFilterRow();

                GstTdsSearchFilters searchFilter = new GstTdsSearchFilters();
                var filterBuilder = searchFilter.GetGSTTDSSearchCriteria(billList,gsttdsFilters);

                billList = await filterBuilder.ApplyAsync(billList,forder);
                var gmb = new GridModelBuilder<Bills>(billList,gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = o => MapToGridModel(o),
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return await gmb.BuildModelAsync();

                //return Json(await gmb.BuildAsync());
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetGSTTDS);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }



        /// <summary>
        /// Author: Karthick J; Date: 12/09/2022
        /// Purpose: Bind the values to the awesome grid columns
        /// Modified By: SK
        /// Modified On: 05-11-2022
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
                    GstIn_Number = o.Vendor.GSTIN_Number == null ? "UnRegistered" : o.Vendor.GSTIN_Number,
                    BillAmount = o.BillAmount.ToString("0.00"),
                    GstTdsPayableAmount = o.GSTTDS.ToString("0.00"),
                    GstTdsStatus = o.GSTTDSStatus?.StatusMaster?.CodeName ?? string.Empty
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

        /// <summary>
        /// Author:Karthick J Date:13/09/2022
        /// Purpose:To Generate JSON File
        /// Modified By:SK Date:07-11-2022         
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Breadcrumb(ValueMapping.genGSTTDSJsonFile)]
        public async Task<IActionResult> GenerateJsonFile(List<int> id)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var billList = await _mediator.Send(new GetBillListByIdRequest() { BillIds = id });
               // IQueryable<Bills> tdsQuery = billList.AsQueryable();

                var Count = new GsttdsPaymentChallanViewModel
                {
                    NoOfVendors = billList.Select(b => b.VendorId).Distinct().Count(),
                    NoOfTrans = id.Count,
                    GSTTDSAmount = billList.Sum(c => c.GSTTDS),
                    TotalGSTTDSPayment = billList.Sum(c => c.GSTTDS),
                    BillsId = billList.Select(b => b.ID).ToList()
                };
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });
                ViewBag.assessmentYear = assessmentYearList;
                ViewBag.BillId = id.ToList();
                ViewBag.UserName = CurrentUser;
                return PartialView(Count);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GenerateJsonFile);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Author:Karthick J Date:13/09/2022
        /// Purpose:To Generate JSON File
        /// Modified By:SK Date:07-11-2022
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Breadcrumb(ValueMapping.genGSTTDSJsonFile)]
        public async Task<IActionResult> GenerateJsonFile(GsttdsPaymentChallanViewModel gstTdsPaymentChallanViewModel,string billsId)
        {
            try
            {
                ModelState.Remove(nameof(GsttdsPaymentChallanViewModel.BillsId));
                if (ModelState.IsValid)
                {
                    //gstTdsPaymentChallanViewModel.BillsId = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(billsId);
                    _unitOfWork.CreateTransaction();
                    var tdsStatusList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.gstTdsStatus });
                    gstTdsPaymentChallanViewModel.GSTTDSStatus.Status = tdsStatusList.FirstOrDefault(t => t.CodeType == ValueMapping.gstTdsStatus && t.CodeValue == ValueMapping.ChallanCreated)?.Id ?? 0;
                    var gstTdsPaymentChallanDto = _mapper.Map<GsttdsPaymentChallanDto>(gstTdsPaymentChallanViewModel);
                    var gstTdsPaymentChallanCommand = new GenerateGsttdsPaymentChallanCommand { gstTdsPaymentChallan = gstTdsPaymentChallanDto,user = CurrentUser };
                  await _mediator.Send(gstTdsPaymentChallanCommand);
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.generatedBankFile,ValueMapping.JsonFile,notificationType: Alerts.success);

                    return RedirectToAction("GSTTDSJSONFileList");
                }
                else
                {
                    return View(gstTdsPaymentChallanViewModel);
                }
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GenerateJsonFile);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Created By:SK Date:10-11-2022
        /// Prupose:Export the xls report for GSTTdsList
        /// </summary>
        /// <param name="gsttdsFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> ExportGstTdsList(GstTdsFilter gsttdsFilters,GridParams gridParams,string[] forder)
        {

            try
            {
                //if (allPages.HasValue && allPages.Value)
                //{
                //    gridParams.Paging = false;
                //}

                var gridModel = await GetGSTTDSListGridModel(gsttdsFilters,gridParams,forder);
                var expColumns = new[]
                {
                    new ExpColumn { Name = "BillReferenceNo", Width = 1.5f, Header = "Bill Ref No" },
                    new ExpColumn { Name = "BillDate", Width = 2.5f, Header = "Bill Date (dd-MM-yyyy)" },
                    new ExpColumn { Name = "VendorName", Width = 3, Header = "Vendor Name" },
                    new ExpColumn { Name = "GstIn_Number", Width = 2.2f, Header ="GSTIN" },
                    new ExpColumn { Name = "BillAmount", Width = 2f, Header="Invoice Amount" },
                    new ExpColumn { Name = "GstTdsPayableAmount", Width = 3.2f, Header = "TDS Payable Amount" },
                    new ExpColumn { Name = "GstTdsStatus", Width = 2.7f, Header = "GST-TDS Status" }
                };
                var workbook = (new GridExcelBuilder(expColumns) { }).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(),"application/vnd.ms-excel","GSTTdsPayableList.xls");
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

        #region JsonFileList

        /// <summary>
        /// Author:Sandeep  Date:07/07/2022
        /// Purpose: Returns View to display list GST-TDS  List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Breadcrumb(ValueMapping.GstTdsJsonFileList)]
        public IActionResult GSTTDSJSONFileList()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }

        /// <summary>
        /// Author: Karthick J; Date: 13/09/2022
        /// Purpose: Get the total GST-TDS Challan payable amount along with filter search enabled
        /// </summary>
        /// <param name="jsonFileFilters"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetTotalGSTTDSChallanPayableAmount(JsonFileFilter jsonFileFilters,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                forder = forder ?? new string[] { };
                var jsonFileList = await _mediator.Send(new GetGsttdsPaymentChallanListRequest());

                //var frow = new TdsChallanFilterRow();
                JsonFileSearchFilters searchFilter = new JsonFileSearchFilters();
                var filterBuilder = searchFilter.GetJsonFileSearchCriteria(jsonFileList,jsonFileFilters);

                jsonFileList = await filterBuilder.ApplyAsync(jsonFileList,forder);
                var totalGSTTDSPayment = jsonFileList.Sum(c => c.GSTTDSPaymentChallan.TotalGSTTDSPayment);
                return Json(totalGSTTDSPayment.ToString("0.00"));
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetTotalTDSChallanPayableAmount);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Author: Karthick J; Date: 14/09/2022
        /// Purpose: Get the list of gst tds challans and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="jsonFileFilters"> </param>
        /// <returns>List of TDS based of filters</returns>
        public async Task<IActionResult> GetGSTTDSChallanList(JsonFileFilter jsonFileFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                forder = forder ?? new string[] { };

                var jsonFileList = await _mediator.Send(new GetGsttdsPaymentChallanListRequest());

                var frow = new GstTdsFilterRow();
                JsonFileSearchFilters searchFilter = new JsonFileSearchFilters();
                var filterBuilder = searchFilter.GetJsonFileSearchCriteria(jsonFileList,jsonFileFilters);

                jsonFileList = await filterBuilder.ApplyAsync(jsonFileList,forder);
                var gmb = new GridModelBuilder<GsstPaymentChallanDto>(jsonFileList,gridParams)
                {
                    KeyProp = o => o.GSTTDSPaymentChallan.Id,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return Json(await gmb.BuildAsync());
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.GetGSTTDSChallanList);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Created By:SK 10-11-2022
        /// Purpose : Download the xls report for GSTTDS json file list
        /// </summary>
        /// <param name="jsonFileFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        public async Task<GridModel<GsstPaymentChallanDto>> GetGSTTDSChallanListDownload(JsonFileFilter jsonFileFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                forder = forder ?? new string[] { };

                var jsonFileList = await _mediator.Send(new GetGsttdsPaymentChallanListRequest());
                IQueryable<GsstPaymentChallanDto> jsonQuery = jsonFileList.AsQueryable<GsstPaymentChallanDto>();

                var frow = new GstTdsFilterRow();
                JsonFileSearchFilters searchFilter = new JsonFileSearchFilters();

                var filterBuilder = searchFilter.GetJsonFileSearchCriteria(jsonQuery,jsonFileFilters);
                jsonQuery = await filterBuilder.ApplyAsync(jsonFileList,forder);


                var gmb = new GridModelBuilder<GsstPaymentChallanDto>(jsonQuery,gridParams)
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
                Log.Information(ValueMapping.GetGSTTDSChallanList);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }
        /// <summary>
        /// Author: Karthick J; Date: 14/09/2022
        /// Purpose: Bind the values to the awesome grid columns
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private object MapToGridModel(BillGsttdsPayment o)
        {
            try
            {
                return new
                {
                    Id = o.ID,
                    ReferenceNo = o.Bill.BillReferenceNo,
                    PaidDate = o.GSTTDSPaymentChallan.PaidDate,
                    VendorName = o.Bill.Vendor.Name,
                    GstinNumber = o.Bill.Vendor.GSTIN_Number,
                    BillAmount = o.Bill.BillAmount.ToString("0.00"),
                    GstTdsPayableAmount = o.GSTTDSPaymentChallan.PaidAmount.ToString("0.00"),
                    GstCertificate = o.GSTTDSPaymentChallan.GSTR7ACertificate,
                    UtrNo = o.GSTTDSPaymentChallan.UTRNo,
                    GstTdsStatus = o.GSTTDSPaymentChallan.GSTTDSStatus.StatusMaster.CodeName
                    //GstTdsStatus = o.GSTTDSPaymentChallan.GSTTDSStatus?.StatusMaster?.CodeName ?? string.Empty
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

        /// <summary>
        /// Author: Karthick J; Date: 14/09/2022
        /// Purpose: Bind the values to the awesome grid columns
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private object MapToGridModel(GsstPaymentChallanDto o)
        {
            try
            {
                return new
                {
                    Id = o.GSTTDSPaymentChallan.Id,
                    CreateDate = o.GSTTDSPaymentChallan.CreatedOn.ToString("dd/MM/yyyy"),
                    NoOfVendors = o.GSTTDSPaymentChallan.NoOfVendors,
                    NoOfTrans = o.GSTTDSPaymentChallan.NoOfTrans,
                    GstTdsAmount = o.GSTTDSPaymentChallan.TotalGSTTDSPayment.ToString("0.00"),
                    AcknowledgementRefNo = o.GSTTDSPaymentChallan.AcknowledgementRefNo == null ? string.Empty : o.GSTTDSPaymentChallan.AcknowledgementRefNo,
                    UtrNo = o.GSTTDSPaymentChallan.UTRNo == null ? string.Empty : o.GSTTDSPaymentChallan.UTRNo,
                    GstTdsStatus = o.CodeName != null ? o.CodeName : string.Empty
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

        [HttpGet]
        [Breadcrumb(ValueMapping.updateAcknowledgementRefNo)]
        public async Task<IActionResult> UpdateAcknowledgementRefNo(int id)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var billGstTdsPayment = await _mediator.Send(new GetGsttdsPaymentChallanByIDRequest() { ID = id });
                var res = _mapper.Map<GsttdsPaymentChallanViewModel>(billGstTdsPayment);
                var gstTdsBillPaymentQuery = await _mediator.Send(new GetBillGsttdsPaymentListRequest());
                var banks = await _mediator.Send(new GetBankMasterListRequest { });
                ViewBag.Banks = _mapper.Map<List<BankMasterModel>>(banks);

                ViewBag.UserName = CurrentUser;
                ViewBag.AcknowledgementRefNo = gstTdsBillPaymentQuery.Select(x => x.GSTTDSPaymentChallan.AcknowledgementRefNo).ToList();

                return PartialView(res);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.UpdateGSTTDSPaymentChallan);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        [HttpPost]
        [Breadcrumb(ValueMapping.updateAcknowledgementRefNoPost)]
        public async Task<IActionResult> UpdateAcknowledgementRefNo(GsttdsPaymentChallanViewModel entity)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var banks = await _mediator.Send(new GetBankMasterListRequest { });

                var selectedBank = _mapper.Map<BankMasterModel>(banks.Where(x => x.Id == entity.BankMasterID).FirstOrDefault());
                if (selectedBank.IsBulkGSTTDS != entity.IsBulkGSTTDS)
                {
                    entity.IsBulkSTTDSModified = true;
                }
                var gstTdsPaymentChallanDto = _mapper.Map<GsttdsPaymentChallanDto>(entity);
                gstTdsPaymentChallanDto.Bank= _mapper.Map <BankMaster>(banks.Where(x=>x.Id==entity.BankMasterID).FirstOrDefault());
                var gsttdsTrnsactionsummary = new AddGSTTDSTransactionSummaryCommand { currentUser = CurrentUser, gstTdsPaymentChallanList = gstTdsPaymentChallanDto };
                await _mediator.Send(gsttdsTrnsactionsummary);

                var gstTdsTransaction = new AddPaidTransactionCommand {Id = gstTdsPaymentChallanDto.Id, gstTdsPaymentChallanList = gstTdsPaymentChallanDto, CurrentUser = CurrentUser, UTR = gstTdsPaymentChallanDto.UTRNo};
               await _mediator.Send(gstTdsTransaction);
                var gstTdsPaymentChallanCommand = new UpdateGsttdsPaymentChallanCommand { gstTdsPaymentChallanList = gstTdsPaymentChallanDto,user = CurrentUser };
                await _mediator.Send(gstTdsPaymentChallanCommand);

                // var billGstTdsPayment = await _mediator.Send(new GetGSTTDSPaymentChallanByIDRequest() { ID = Id });
                // ViewBag.UserName = CurrentUser;
                TempData["Message"] = PopUpServices.Notify(ValueMapping.updatedsuc,ValueMapping.updateAcknowledgementRefNo,notificationType: Alerts.success);


                return RedirectToAction("GSTTDSPaidList");
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.UpdateGSTTDSPaymentChallan);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }



        public async Task<FileResult> DownloadJsonFile(int id)
        {

            var gstTdsBillPaymentQuery = await _mediator.Send(new GetGstJsonFileList());
            var res = gstTdsBillPaymentQuery.Where(x => x.GSTTDSPaymentID == id).ToList();


            var obj = new Root();
            obj.gstin = "29AAALK0820C1DU";
            obj.fp = String.Concat(DateTime.UtcNow.Month,DateTime.UtcNow.Year);
            foreach (var res1 in res.Select(res=>res.Bill))
            {

                Td file = new Td();
                file.flag = "N";
                file.gstin_ded = res1.Vendor.GSTIN_Number;
                file.amt_ded = res1.BillAmount;
                var vendor = res1.Vendor.GSTIN_Number.Substring(0,2);
                if (vendor != "29")
                {
                    file.iamt = res1.GSTTDS;
                    file.camt = (decimal?)0.00;
                    file.samt = (decimal?)0.00;

                }
                else
                {
                    file.camt = res1.GSTTDS / 2;
                    file.samt = res1.GSTTDS / 2;
                    file.iamt = (decimal?)0.00;
                }

                obj.tds.Add(file);
            }

            var jsonstr = System.Text.Json.JsonSerializer.Serialize(obj);
            byte[] byteArray = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonstr);
            return File(byteArray,"application/octet-stream","file1.json");
        }


        /// <summary>
        /// Created By:SK Date:10-11-2022
        /// Prupose:Export the xls report for GSTTdsJsonList
        /// </summary>
        /// <param name="gsttdsFilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> ExportGstTdsJsonList(JsonFileFilter jsonFileFilters,GridParams gridParams,string[] forder)
        {

            try
            {

                var gridModel = await GetGSTTDSChallanListDownload(jsonFileFilters,gridParams,forder);
                var expColumns = new[]
                {
                    new ExpColumn { Name = "CreateDate", Width = 1.5f, Header = "Created Date" },
                    new ExpColumn { Name = "NoOfVendors", Width = 2.5f, Header = "No Of Vendors" },
                    new ExpColumn { Name = "NoOfTrans", Width = 3, Header = "No Of Trans" },
                    new ExpColumn { Name = "GstTdsAmount", Width = 2.2f, Header ="GST TDS Amount" },
                    new ExpColumn { Name = "AcknowledgementRefNo", Width = 2f, Header="Acknowledgement Ref.No" },
                    new ExpColumn { Name = "UtrNo", Width = 3.2f, Header = "UTR No" },
                    new ExpColumn { Name = "GstTdsStatus", Width = 2.7f, Header = "GST-TDS Status" }
                };
                var workbook = (new GridExcelBuilder(expColumns) { }).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(),"application/vnd.ms-excel","GSTTdsPList.xls");
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

        #region GST-TDS Paid List
        /// <summary>
        /// Author: Karthick J Date: 13/9/2022
        /// Purpose: Returns View to display GST-TDS Paid List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Breadcrumb(ValueMapping.GstTdsPaidList)]
        public IActionResult GSTTDSPaidList()
        {
            ViewBag.UserName = CurrentUser;
            return View();
        }

        /// <summary>
        /// Author: Karthick J; Date: 14/09/2022
        /// Purpose: Get the total GST-TDS paid amount along with filter search enabled
        /// </summary>
        /// <param name="gstTdsPaidFilters"></param>
        /// <param name="forder"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetTotalGSTTDSPaidAmount(GstTdsPaidFilter gstTdsPaidFilters,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                forder = forder ?? new string[] { };
                var gstTdsBillPaymentQuery = await _mediator.Send(new GetBillGsttdsPaymentListRequest());
                GstTdsPaidSearchFilters searchFilter = new GstTdsPaidSearchFilters();
                var filterBuilder = searchFilter.GetGSTTDSPaidListSearchCriteria(gstTdsBillPaymentQuery,gstTdsPaidFilters);

                gstTdsBillPaymentQuery = await filterBuilder.ApplyAsync(gstTdsBillPaymentQuery,forder);
                var totalGSTTDSPaidAmount = gstTdsBillPaymentQuery.Select(b => b.GSTTDSPaymentChallan).Distinct().Sum(b => b.PaidAmount);
                return Json(totalGSTTDSPaidAmount.ToString("0.00"));
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getTotalGSTTDSPaidAmount);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Author: Karthick J; Date: 14/09/2022
        /// Purpose: Get the list of gst-tds paid List and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="gstTdsPaidFilters"> </param>
        /// <returns>List of TDS based of filters</returns>
        public async Task<IActionResult> GetGSTTDSPaidList(GstTdsPaidFilter gstTdsPaidFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                forder = forder ?? new string[] { };

                var gstTdsBillPaymentQuery = await _mediator.Send(new GetBillGsttdsPaymentListRequest());

                var frow = new GstTdsPaidFilterRow();
                GstTdsPaidSearchFilters searchFilter = new GstTdsPaidSearchFilters();
                var filterBuilder = searchFilter.GetGSTTDSPaidListSearchCriteria(gstTdsBillPaymentQuery,gstTdsPaidFilters);
                await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.tStatus });

                gstTdsBillPaymentQuery = await filterBuilder.ApplyAsync(gstTdsBillPaymentQuery,forder);

                var gmb = new GridModelBuilder<BillGsttdsPayment>(gstTdsBillPaymentQuery,gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = o => MapToGridModelGst(o),
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return Json(await gmb.BuildAsync());
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getGSTTDSPaidList);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Author: Swetha M J; Date: 11/11/2022
        /// Purpose: Get the list of gst-tds paid List and bind it Awesome grid along with filter search enabled
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="gstTdsPaidFilters"> </param>
        /// <returns>List of TDS based of filters</returns>
        public async Task<GridModel<BillGsttdsPayment>> GetGSTTDSPaidListDownload(GstTdsPaidFilter gstTdsPaidFilters,GridParams gridParams,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                forder = forder ?? new string[] { };

                var gstTdsBillPaymentQuery = await _mediator.Send(new GetBillGsttdsPaymentListRequest());

                var frow = new GstTdsPaidFilterRow();
                GstTdsPaidSearchFilters searchFilter = new GstTdsPaidSearchFilters();
                var filterBuilder = searchFilter.GetGSTTDSPaidListSearchCriteria(gstTdsBillPaymentQuery,gstTdsPaidFilters);
                await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.tStatus });

                gstTdsBillPaymentQuery = await filterBuilder.ApplyAsync(gstTdsBillPaymentQuery,forder);

                var gmb = new GridModelBuilder<BillGsttdsPayment>(gstTdsBillPaymentQuery,gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = o => MapToGridModelGst(o),
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };
                return (await gmb.BuildModelAsync());
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getGSTTDSPaidList);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Author: Karthick J; Date: 22/08/2022
        /// Purpose: Bind the values to the awesome grid columns
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private object MapToGridModelGst(BillGsttdsPayment o)
        {
            try
            {
                return new
                {
                    Id = o.ID,
                    ReferenceNo = o.Bill.BillReferenceNo,
                    PaidDate = o.GSTTDSPaymentChallan.PaidDate.Value.ToString("dd-MM-yyyy"),
                    VendorName = o.Bill.Vendor.Name,
                    GstinNumber = o.Bill.Vendor.GSTIN_Number,
                    BillAmount = o.Bill.BillAmount.ToString("0.00"),
                    GstTdsPayableAmount = o.GSTTDSPaymentChallan.PaidAmount.ToString("0.00"),
                    AcknowledgementRefNo = o.GSTTDSPaymentChallan.AcknowledgementRefNo,
                    UtrNo = o.GSTTDSPaymentChallan.UTRNo,
                    GstTdsStatus = o.GSTTDSPaymentChallan.GSTTDSStatus.StatusMaster.CodeName
                    //GstTdsStatus = o.GSTTDSPaymentChallan.StatusMaster?.CodeValue ?? string.Empty
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



        /// <summary>
        /// Created By:SK Date:10-11-2022
        /// Prupose:Export the xls report for GSTTdsList
        /// </summary>
        /// <param name="gstTdsPaidfilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> ExportToGSTTDSPaidList(GstTdsPaidFilter gstTdsPaidfilters,GridParams gridParams,string[] forder)
        {

            try
            {

                var gridModel = await GetGSTTDSPaidListDownload(gstTdsPaidfilters,gridParams,forder);
                var expColumns = new[]
                {
                    new ExpColumn  { Name = "ReferenceNo", Width = 1.5f, Header = "Bill Ref No"},
                    new ExpColumn { Name = "PaidDate",Width = 1.5f, Header = "Pay Date (dd-MM-yyyy)" },
                    new ExpColumn { Name = "VendorName",Width = 1.5f, Header = "Vendor Name" },
                    new ExpColumn { Name = "GstinNumber", Width = 1.5f, Header = "GSTIN",  },
                    new ExpColumn { Name = "BillAmount",  Width = 1.5f,Header = "Total Bill Amount"},
                    new ExpColumn { Name = "GstTdsPayableAmount",Width = 1.5f, Header = "GST-TDS Paid"},
                    new ExpColumn { Name = "AcknowledgementRefNo",Width = 1.5f, Header = "Acknowledgment Ref.No"},
                    new ExpColumn { Name = "UtrNo", Width = 1.5f, Header = "UTR No." },
                    new ExpColumn { Name = "GstTdsStatus",Width = 1.5f, Header = "GST-TDS Status" }
                };
                var workbook = (new GridExcelBuilder(expColumns) { }).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(),"application/vnd.ms-excel","GSTTdsPaidList.xls");
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
    }
}
