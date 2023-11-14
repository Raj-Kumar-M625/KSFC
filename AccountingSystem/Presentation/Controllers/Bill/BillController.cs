using Application.DTOs.Bill;
using Application.UserStories.Bill.Requests.Commands;
using Application.UserStories.Bill.Requests.Queries;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Payment.Requests.Queries;
using Application.UserStories.Vendor.Requests.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Common.Downloads;
using Domain.Bill;
using Domain.Payment;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.EntityFrameworkCore;
using Omu.Awem.Export;
using Omu.AwesomeMvc;
using Persistence;
using Persistence.Repositories.Generic;
using Presentation.AwesomeToolUtils;
using Presentation.Extensions.Bill;
using Presentation.Helpers;
using Presentation.Models;
using Presentation.Models.Bill;
using Presentation.Models.Master;
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

namespace Presentation.Controllers.Bill
{
    /// <summary>
    /// Bill Controller
    /// </summary>
    /// 
    // [Authorize]
    [SessionTimeoutHandlerAttribute]

    public class BillController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private GenericRepository<Bills> repository;
        /// <summary>
        /// Current User
        /// </summary>
        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        protected bool Role => this.HttpContext.User.IsInRole("ADMINISTRATOR");

        /// <summary>
        /// Custom Constructor with DI
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        public BillController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            repository = new GenericRepository<Bills>(unitOfWork);
        }
        /// <summary>
        /// Purpose = Display The Bill List Page
        /// Author = Sandeep 
        /// Date = May 06 2022  
        /// </summary>
        [Breadcrumb(ValueMapping.bill)]
        public IActionResult Index()
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


        /// <summary>
        /// Purpose: Get the list of Bill and bind it Awesome grid along with filter search enabled
        /// Author: Sandeep; Date: 07/05/2022
        /// ModifiedBy:
        /// ModifiedPurpose:
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="billfilter"></param>
        /// <param name="forder"></param>
        /// <returns>List of Bill based of filters</returns>
        /// 



        public async Task<IActionResult> GetBillList(BillFilter billfilter, GridParams gridParams, string[] forder)
        {
            try
            {

                _unitOfWork.CreateTransaction();
                var list = await _mediator.Send(new GetBillListRequest());
                var frow = new BillFilterRow();
                var searchFilter = new BillSearchFilters();
                var filterBuilder = searchFilter.GetBillSearchCriteria(list, billfilter);
                var query = await filterBuilder.ApplyAsync(list, forder);
                var gmb = new GridModelBuilder<Bills>(query, gridParams)
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
                Log.Information(ValueMapping.getBillList);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Purpose: Bind the values to the awesome grid columns
        /// Author: Sandeep; Date: 05/05/20220
        /// ModifiedBy:
        /// ModifiedPurpose:
        /// </summary>
        /// <param name="o"></param>
        private object MapToGridModel(Bills o)
        {
            try
            {
                return new
                {
                    o.ID,
                    o.BillReferenceNo,
                    name = o.Vendor.Name,
                    BillDate = o.BillDate.ToString("dd-MM-yyyy"),
                    Category = o.Vendor.VendorDefaults == null ? "" : o.Vendor.VendorDefaults.Category,
                    //TotalGST = o.Vendor.VendorBalance == null ? 0 : o.Vendor.VendorBalance.TotalGST,
                    BillDueDate = o.BillDueDate.ToString("dd-MM-yyyy"),
                    BillTotal = o.TotalBillAmount.ToString("0.00"),
                    TotalGST = o.GSTAmount.ToString("0.00"),
                    TDSAmount = o.TDS.ToString("0.00"),
                    GstTdsAmount = o.GSTTDS.ToString("0.00"),
                    TotalPayable = o.NetPayable.ToString("0.00"),
                    CreatedBy = o.CreatedBy == null ? "N/A" : o.CreatedBy,
                    Status = o.BillStatus.StatusMaster.CodeValue,
                    vendorID = o.VendorId
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
        /// Purpose: Bind the values to the created grid columns
        /// Author: Sandeep; Date: 05/05/2022
        /// ModifiedBy:
        /// ModifiedPurpose:
        /// </summary>
        public async Task<IActionResult> GetCreatedByList()
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var list = await _mediator.Send(new GetBillListRequest());
                var query = new List<KeyContent>();
                query.AddRange(list.Select(o => new KeyContent(o.CreatedBy, o.CreatedBy)).Distinct());
                return Json(query);
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
        /// Purpose: File Download
        /// Author: Sandeep; Date: 05/05/2022
        /// Modified By:Pankaj K; Date: 28/05/2022
        /// Modified Purpose: Refactor the code
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
                DownloadService<BillListDto> dl = new();
                List<BillListDto> listOfBills = new();
                listOfBills = await _mediator.Send(new GetBillListRequestIEnumerable());

                //Parse list data into byte array and generate the file            
                dl.ListItems = listOfBills;
                dl.FileName = fileName;
                var fc = new FileContentResult(dl.GetFile(), dl.ContentType());
                fc.FileDownloadName = fileName.AddTimeStamp();

                _unitOfWork.Save();
                _unitOfWork.Commit();
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
        /// Author:Swetha M Date:13/05/2022
        /// Purpose: Get the list of Categories and bind it Awesome grid along with
        /// </summary>
        /// <returns>Category Status List</returns>
        public async Task<ActionResult> GetCategoryList()
        {
            try
            {

                _unitOfWork.CreateTransaction();

                var list = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.categoryType });
                var query = new List<KeyContent>();
                query.AddRange(list.Select(o => new KeyContent(o.CodeValue, o.CodeValue)).Distinct());
                return Json(query);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getCategoryList);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                _unitOfWork.Rollback();

                throw;
            }
        }

        /// <summary>
        /// Author:Swetha M Date:13/05/2022
        /// Purpose: Get the list of Categories and bind it Awesome grid along with
        /// </summary>
        /// <returns>Bill Status List</returns>
        public async Task<ActionResult> GetBillStatus()
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var list = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.bStatus });
                var query = new List<KeyContent>();
                query.AddRange(list.Select(o => new KeyContent(o.CodeValue, o.CodeValue)).Distinct());
                return Json(query);
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.getBillStatus);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                _unitOfWork.Rollback();

                throw;
            }
        }

        /// <summary>
        /// Author:Sandeep M Date:25/05/2022
        /// Purpose: Add the Bill Details of the Vendor
        /// </summary>
        [Breadcrumb(ValueMapping.addBill)]
        [HttpGet]
        public async Task<ActionResult> AddBill(int id)
        {
            try
            {

                var list = await _mediator.Send(new GetVendorListByIDRequest { ID = id });
                var bills = await _mediator.Send(new GetBillListRequest());
                var billNo = bills.ToList().Where(x => x.VendorId == id).Select(o => o.BillNo);
                var vendorDetails = _mapper.Map<VendorViewModel>(list);
                if (vendorDetails.GSTIN_Number != null)
                {
                    ViewBag.GSTIN = vendorDetails.GSTIN_Number.Substring(0, 2);

                }
                else
                {
                    return RedirectToAction("AddBillForUnregistered", new { Id = id });
                }
                var vendorCategoryType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.categoryType });
                ViewBag.VendorCategoryType = _mapper.Map<List<CommonMasterModel>>(vendorCategoryType);
                var dropDownList = await _mediator.Send(new GetDropDownListRequest());
                ViewBag.billNo = billNo;
                ViewBag.DropDownList = dropDownList;
                ViewBag.VendorDetails = vendorDetails;
                ViewBag.UserName = CurrentUser;
                ViewBag.Id = id;
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


        /// <summary>
        /// Created On:18-11-2022
        /// Created By:SK
        /// Purpose:Add a bill for Unregistered Vendor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [Breadcrumb(ValueMapping.addBill)]
        [HttpGet]
        public async Task<ActionResult> AddBillForUnregistered(int id)
        {
            try
            {
                var list = await _mediator.Send(new GetVendorListByIDRequest { ID = id });
                var vendorDetails = _mapper.Map<VendorViewModel>(list);
                var bills = await _mediator.Send(new GetBillListRequest());
                var billNo = bills.ToList().Where(x => x.VendorId == id).Select(o => o.BillNo);
                var dropDownList = await _mediator.Send(new GetDropDownListRequest());
                var vendorCategoryType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.categoryType });
                ViewBag.VendorCategoryType = _mapper.Map<List<CommonMasterModel>>(vendorCategoryType);
                ViewBag.billNo = billNo;
                ViewBag.DropDownList = dropDownList;
                ViewBag.VendorDetails = vendorDetails;
                ViewBag.UserName = CurrentUser;
                ViewBag.Id = id;
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


        /// <summary>
        /// Purpose = Add The Bill  Details
        /// Author = Sandeep
        /// Date = 13 June 2022  
        /// </summary>

        [HttpPost]
        public async Task<ActionResult> AddBillRecords(List<BillItemsModel> billsData, List<Bills> billpaymentdata)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                 await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.tStatus });
               // var pendingStatusId = tdsStatusList.OrderBy(t => t.DisplaySequence).FirstOrDefault()?.Id;

                var billlist = _mapper.Map<List<BillsDto>>(billpaymentdata);
                var command = new CreateBillCommand { BillDto = billlist, user = CurrentUser };
                var bills = await _mediator.Send(command);
                var referenceno = bills.BillReferenceNo;
                var billid = bills.Id;
                var billpaydet = _mapper.Map<List<BillItemsDto>>(billsData);
                var createBillPaymentDetails = new CreateBillPaymentDetails { BillDto = billpaydet, billPayment = bills, user = CurrentUser };
                 await _mediator.Send(createBillPaymentDetails);
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return Ok(new { data = billid, referenceno });
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.addBillRecords);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }



        /// <summary>
        /// Purpose = Get Particular Bill Details
        /// Author = Sandeep
        /// Date = 09 June 2022  
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> BillDetails(string id, string? ModuleType, int PaymentId)
        {
            try
            {
                var payment = await _mediator.Send(new GetPaymentsByIdRequest { Id = PaymentId });
                if (ModuleType == null)
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "Bill", "Bill List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;
                }

                else if (ModuleType == "ListOfTransaction")
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "ListOfTransaction", "List of Transaction", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;

                }
                else if (ModuleType == "TDS")
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "TDS", "TDS Payable List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;

                }
                else if (ModuleType == "GST-TDS")
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "GSTTDS", "GST-TDS List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;
                }

                else if (ModuleType == "PaymentList")
                {
                    if (payment.PaymentStatus.StatusMaster.CodeValue != "Rejected")
                    {
                        var childNode1 = new MvcBreadcrumbNode("Index", "Payment", "Payment List", false)
                        {
                            //this comes in as a param into the action
                        };
                        var childNode2 = new MvcBreadcrumbNode("ApprovePayment", "Payment", "Approve Payments")
                        {
                            RouteValues = new { id = PaymentId },
                            OverwriteTitleOnExactMatch = true,
                            Parent = childNode1
                        };
                        var childNode3 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                        {
                            OverwriteTitleOnExactMatch = true,
                            Parent = childNode2
                        };

                        ViewData["BreadcrumbNode"] = childNode3;


                    }
                    else
                    {
                        var childNode1 = new MvcBreadcrumbNode("Index", "Payment", "Payment List", false)
                        {
                            //this comes in as a param into the action
                        };
                        var childNode2 = new MvcBreadcrumbNode("ApprovePayment", "Payment", "Rejected Payments")
                        {
                            RouteValues = new { id = PaymentId },
                            OverwriteTitleOnExactMatch = true,
                            Parent = childNode1
                        };
                        var childNode3 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                        {
                            OverwriteTitleOnExactMatch = true,
                            Parent = childNode2
                        };

                        ViewData["BreadcrumbNode"] = childNode3;

                    }

                }
                else if (ModuleType == "EditPaymentList")
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "Payment", "Payment List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("ApprovePayment", "Payment", "Edit Payments")
                    {
                        RouteValues = new { id = PaymentId },
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    var childNode3 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode2
                    };

                    ViewData["BreadcrumbNode"] = childNode3;

                }

                var list = await _mediator.Send(new GetBillPaymentDetails { ID = id.ToString() });
                var billPaymentDetails = _mapper.Map<List<BillItemsModel>>(list);
                var bills = billPaymentDetails.ToList();
                if (bills[0].Bills.Vendor.GSTIN_Number != null)
                {
                    ViewBag.GSTINNumber = (bills[0].Bills.Vendor.GSTIN_Number.Substring(0, 2));
                }
                else
                {

                    return RedirectToAction("BillDetailsForUnregistered", new { Id = id, PaymentId = PaymentId, ModuleType = ModuleType });
                }
                int billID = new();
                if (id.StartsWith("B"))
                {
                    billID = bills.Where(x => x.BillReferenceNo == id).Select(x => x.Bills.Id).First();

                }
                else
                {
                    billID = bills.Where(x => x.BillsID.ToString() == id).Select(x => x.Bills.Id).First();

                }

                var documentDetails = await _mediator.Send(new GetBillDocumentDetailsRequest { ID = billID });
                ViewBag.DocumentDetails = _mapper.Map<List<DocumentsModel>>(documentDetails);
                var vendorCategoryType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.categoryType });
                ViewBag.VendorCategoryType = _mapper.Map<List<CommonMasterModel>>(vendorCategoryType);

                var categoryGSTPerc = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.CategoryGSTPercentage });
                ViewBag.categoryGSTPerc = _mapper.Map<List<CommonMasterModel>>(categoryGSTPerc);

                var dropDownList = await _mediator.Send(new GetDropDownListRequest());
                ViewBag.DropDownList = dropDownList;

                ViewBag.BillPaymentDetails = billPaymentDetails;
                ViewBag.UserName = CurrentUser;
                ViewBag.ReferenceNo = list[0].Bills.BillReferenceNo;
                ViewBag.BillNo = list[0].Bills.BillNo;
                ViewBag.Id = list[0].VendorID;
                ViewBag.BillID = list[0].BillsID;
                ViewBag.Role = HttpContext.Session.GetString("_role");
                return View();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.billDetails);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }





        [HttpGet]
        [Breadcrumb(ValueMapping.details)]
        public async Task<ActionResult> BillDetailsForUnregistered(string id, int PaymentId, string? ModuleType)
        {
            try
            {
                var payment = await _mediator.Send(new GetPaymentsByIdRequest { Id = PaymentId });
                if (ModuleType == null)
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "Bill", "Bill List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;
                }
                else if (ModuleType == "ListOfTransaction")
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "ListOfTransaction", "List of Transaction", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;

                }
                else if (ModuleType == "TDS")
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "TDS", "TDS Payable List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;

                }
                else if (ModuleType == "GST-TDS")
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "GSTTDS", "GST-TDS List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    ViewData["BreadcrumbNode"] = childNode2;

                }
                else if (ModuleType == "PaymentList")
                {
                    if (payment.PaymentStatus.StatusMaster.CodeValue != "Rejected")
                    {
                        var childNode1 = new MvcBreadcrumbNode("Index", "Payment", "Payment List", false)
                        {
                            //this comes in as a param into the action
                        };
                        var childNode2 = new MvcBreadcrumbNode("ApprovePayment", "Payment", "Approve Payments")
                        {
                            RouteValues = new { id = PaymentId },
                            OverwriteTitleOnExactMatch = true,
                            Parent = childNode1
                        };
                        var childNode3 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                        {
                            OverwriteTitleOnExactMatch = true,
                            Parent = childNode2
                        };

                        ViewData["BreadcrumbNode"] = childNode3;


                    }
                    else
                    {
                        var childNode1 = new MvcBreadcrumbNode("Index", "Payment", "Payment List", false)
                        {
                            //this comes in as a param into the action
                        };
                        var childNode2 = new MvcBreadcrumbNode("ApprovePayment", "Payment", "Rejected Payments")
                        {
                            RouteValues = new { id = PaymentId },
                            OverwriteTitleOnExactMatch = true,
                            Parent = childNode1
                        };
                        var childNode3 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                        {
                            OverwriteTitleOnExactMatch = true,
                            Parent = childNode2
                        };

                        ViewData["BreadcrumbNode"] = childNode3;
                    }

                }
                else if (ModuleType == "EditPaymentList")
                {
                    var childNode1 = new MvcBreadcrumbNode("Index", "Payment", "Payment List", false)
                    {
                        //this comes in as a param into the action
                    };
                    var childNode2 = new MvcBreadcrumbNode("ApprovePayment", "Payment", "Edit Payments")
                    {
                        RouteValues = new { id = PaymentId },
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode1
                    };
                    var childNode3 = new MvcBreadcrumbNode("Index", "Bill", "Bill Details")
                    {
                        OverwriteTitleOnExactMatch = true,
                        Parent = childNode2
                    };

                    ViewData["BreadcrumbNode"] = childNode3;

                }
                var list = await _mediator.Send(new GetBillPaymentDetails { ID = id.ToString() });
                var billPaymentDetails = _mapper.Map<List<BillItemsModel>>(list);
                var bills = billPaymentDetails.ToList();
                int billID = new();
                if (id.StartsWith("B"))
                {
                    billID = bills.Where(x => x.BillReferenceNo == id).Select(x => x.Bills.Id).First();

                }
                else
                {
                    billID = bills.Where(x => x.BillsID.ToString() == id).Select(x => x.Bills.Id).First();
                }
                var documentDetails = await _mediator.Send(new GetBillDocumentDetailsRequest { ID = billID });
                ViewBag.DocumentDetails = _mapper.Map<List<DocumentsModel>>(documentDetails);

                var vendorCategoryType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.categoryType });
                ViewBag.VendorCategoryType = _mapper.Map<List<CommonMasterModel>>(vendorCategoryType);

                var categoryGSTPerc = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.CategoryGSTPercentage });
                ViewBag.categoryGSTPerc = _mapper.Map<List<CommonMasterModel>>(categoryGSTPerc);

                var dropDownList = await _mediator.Send(new GetDropDownListRequest());
                ViewBag.DropDownList = dropDownList;

                ViewBag.BillPaymentDetails = billPaymentDetails;
                ViewBag.UserName = CurrentUser;
                ViewBag.ReferenceNo = list[0].Bills.BillNo;
                ViewBag.SubTotal = list[0].Bills.TotalBillAmount;
                ViewBag.Id = list[0].VendorID;
                ViewBag.BillID = list[0].BillsID;
                ViewBag.Role = HttpContext.Session.GetString("_role"); 
                return View();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.billDetails);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                throw;
            }
        }





        /// <summary>
        /// Purpose = Update The Bill Records
        /// Author = Sandeep
        /// Date = 21 June 2022  
        /// </summary>

        [HttpPost]
        public async Task<ActionResult> UpdateBillRecords(List<BillItemsModel> billsData, string[] deltedval, int[] docdeltedval, string[] docpath, List<Bills> billpaymentdata)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var billlist = _mapper.Map<List<BillsDto>>(billpaymentdata);
                var command = new UpdateBillCommand { BillDto = billlist, user = CurrentUser };
                var bills = await _mediator.Send(command);
                var ReferenceNo = bills.BillReferenceNo;
                var billsid = bills.Id;
                var billpaydet = _mapper.Map<List<BillItemsDto>>(billsData);
                var createBillPaymentDetails = new UpdateBillPaymentDetails { BillDto = billpaydet, billPayment = bills, user = CurrentUser };
                 await _mediator.Send(createBillPaymentDetails);
                if (deltedval != null && deltedval.Length != 0)
                {
                    var deletebill = new DeleteBillPaymentDetails { BillpaymentDetails = deltedval };
                     await _mediator.Send(deletebill);
                }
                if (docdeltedval != null && docdeltedval.Length != 0)
                {
                    var deletebill = new DeleteDocumentRequest { document = docdeltedval, filepath = docpath };
                    await _mediator.Send(deletebill);
                }
                _unitOfWork.Save();
                _unitOfWork.Commit();
                return Ok(new { data = billsid, ReferenceNo });

            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.addBillRecords);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);

                _unitOfWork.Rollback();

                throw;
            }
        }



        /// <summary>
        /// Author:Sandeep M Date:31/10/2022
        /// Purpose: Download the Bill Document       
        /// </summary> 

        public ActionResult DownloadFile(string filePath, string name, string extension, int VendorId)
        {
            try
            {

                byte[] bytes = null;

                if (System.IO.File.Exists(filePath))
                {
                    bytes = System.IO.File.ReadAllBytes(filePath);
                }
                else
                {
                    //TempData["Message"] = PopUpServices.Notify(ValueMapping.DNE, name, notificationType: Alerts.warning);
                    return RedirectToAction("EditVendor", new { Id = VendorId }); 
                }

                //TempData["Message"] = PopUpServices.Notify(ValueMapping.downloadSuccess, name, notificationType: Alerts.success);
                return File(bytes, "application/force-download", string.Concat(name, extension));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "File Not Found";
                throw new Exception();
            }
        }
        [HttpPost]
        public async Task<ActionResult> ApproveBillPayments(string BillRefNo, string Remarks, string Status)
        {
            try
            {
                if (BillRefNo != null)
                {


                    var approveBill = new ApproveBillPaymentsCommand { BillReferenceNo = BillRefNo, CurrentUser = CurrentUser, Remarks = Remarks, Status = Status };
                   await _mediator.Send(approveBill);


                    return Json(new { IsValid = true });
                }
                else
                {
                    return Json(new { IsValid = false });
                }



            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "File Not Found";
                throw new Exception();
            }
        }
        /// <summary>
        /// Purpose: Get the list of Bill and bind it Awesome grid along with filter search enabled
        /// Author: Sandeep; Date: 07/05/2022
        /// ModifiedBy:
        /// ModifiedPurpose:
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="billfilter"></param>
        /// <param name="forder"></param>
        /// <returns>List of Bill based of filters</returns>
        /// 



        public async Task<GridModel<Bills>> GetBillListToDownload(BillFilter billfilter, GridParams gridParams, string[] forder)
        {
            try
            {

                _unitOfWork.CreateTransaction();

                var list = await _mediator.Send(new GetBillListRequest());

                var frow = new BillFilterRow();
                var searchFilter = new BillSearchFilters();
                var filterBuilder = searchFilter.GetBillSearchCriteria(list, billfilter);
                var query = await filterBuilder.ApplyAsync(list, forder);

                var gmb = new GridModelBuilder<Bills>(query, gridParams)
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
                Log.Information(ValueMapping.getBillList);
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
        /// <param name="gridParams"></param>
        /// <param name="billfilter"></param>
        /// <param name="forder"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<IActionResult> ExportToBillList(BillFilter billfilter, GridParams gridParams, string[] forder)
        {
            try
            {

                var gridModel = await GetBillListToDownload(billfilter, gridParams, forder);
                var expColumns = new[]
                {
                    new ExpColumn { Name = "BillReferenceNo", Width = 2.5f, Header = "Bill Reference No" },
                    new ExpColumn { Name = "name", Width = 2.5f, Header = "Vendor Name" },
                    new ExpColumn { Name = "BillDate", Width = 2.5f, Header = "Bill Date (dd-mm-yyy)" },
                    new ExpColumn { Name = "BillDueDate", Width = 2.5f, Header = "Due Date (dd-mm-yyy)" },
                    new ExpColumn { Name = "BillTotal", Width = 3f, Header = "₹ Bill Total" },
                    new ExpColumn { Name = "TotalGST", Width = 3f, Header = "₹ Total GST " },
                    new ExpColumn { Name = "TDSAmount", Width = 3f, Header ="₹ TDS" },
                    new ExpColumn { Name = "GstTdsAmount", Width = 3f, Header ="₹ GST TDS" },
                    new ExpColumn { Name = "TotalPayable", Width = 2f, Header="₹ TotalPayable" },
                    new ExpColumn { Name = "CreatedBy", Width = 2f, Header="Created By " },
                    new ExpColumn { Name = "Status", Width = 2f, Header="Status" }

                };

                var workbook = (new GridExcelBuilder(expColumns)).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(), "application/vnd.ms-excel", "BillsList.xls");
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



