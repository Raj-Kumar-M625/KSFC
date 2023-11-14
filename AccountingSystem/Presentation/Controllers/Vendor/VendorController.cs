using Application.DTOs.Master;
using Application.DTOs.Vendor;
using Application.UserStories.Document.Request.Command;
using Application.UserStories.Document.Request.Queries;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Payment.Requests.Queries;
using Application.UserStories.Vendor.Requests.Commands;
using Application.UserStories.Vendor.Requests.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Common.Downloads;
using Common.Helper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using Domain.Payment;
using Domain.Transactions;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Omu.Awem.Export;
using Omu.AwesomeMvc;
using Persistence;
using Persistence.Repositories.Generic;
using Presentation.AwesomeToolUtils;
using Presentation.Extensions.Payment;
using Presentation.Extensions.Vendor;
using Presentation.GridFilters.ListOfTransaction;
using Presentation.Helpers;
using Presentation.Models;
using Presentation.Models.Master;
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

namespace Presentation.Controllers.Vendor
{
    /// <summary>
    /// Purpose = Controller to perform get and post methods for the Vendor
    /// Author = Kartik 
    /// Date = May 05 2022 
    /// Modified By :Swetha M
    /// Modified Date:10/05/2022
    /// Purpose:Modified the controller
    /// </summary>
    /* [Authorize]*/

    [SessionTimeoutHandlerAttribute]

    public class VendorController:Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        private GenericRepository<Vendors> repository;
        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        protected bool Role => this.HttpContext.User.IsInRole("Administrator");

        /// <summary>
        /// Purpose = Constructor for the vendor controller
        /// Date = May 05 2022 
        /// Modified By :Swetha M
        /// Modified Date:10/05/2022
        /// </summary>
        public VendorController(IMediator mediator,IMapper mapper,AccountingDbContext dbContext,IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            repository = new GenericRepository<Vendors>(unitOfWork);
        }

        /// <summary>
        /// Purpose = Display The Vendor List Page
        /// Author = Sandeep 
        /// Date = May 05 2022  
        /// </summary>
        [Breadcrumb(ValueMapping.vendorList)]
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
                Log.Information(ValueMapping.Inside,ValueMapping.IndexM);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                throw;
            }
        }

        





        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
        //    {
        //        filterContext.Result = RedirectToAction("Login", "Identity");
        //    }
        //    base.OnActionExecuting(filterContext);
        //}
        /// <summary>
        /// Purpose = Save the Vendor Details to DataBase
        /// Author = Kartik 
        /// Date = May 05 2022 
        /// Modified By :Swetha M
        /// Modified Date:10/05/2022
        /// Purpose:Gets all the dropdown values from common master table
        /// </summary>

        [HttpPost]
        [Breadcrumb(ValueMapping.addVendor,FromAction = "Index")]
        public async Task<ActionResult> Create(VendorViewModel vendorModel,List<IFormFile> files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CreateTransaction();
                    var list = await _mediator.Send(new GetvendorListBypanIdRequest { PANID = vendorModel.PAN_Number });
                    //var vendorQuery = vendorList.AsQueryable();
                    if(list != null)
                    {    
                        var vendorDetails = _mapper.Map<VendorDetailsDto>(vendorModel);
                        var Id = list.Id;
                        var vendorListByID = await _mediator.Send(new GetVendorListByIDRequest { ID = Id });
                        var vendorName = vendorDetails.Name;
                        var vendorList = await _mediator.Send(new AddvenderDetailcommand {ID=Id ,VendorDetailsDto = vendorDetails, vendorFiles = files, vendorListByID= vendorListByID, entityPath = ValueMapping.vendorDetailsEntityPath, entityType = ValueMapping.Vendor, user = CurrentUser });

                        _unitOfWork.Save();
                        _unitOfWork.Commit();

                        
                        //Get Dropdown List 
                        var dropDownList = await _mediator.Send(new GetDropDownListRequest());
                        ViewBag.DropDownList = dropDownList;


                        var banks = await _mediator.Send(new GetBankMasterListRequest { });
                        ViewBag.Banks = _mapper.Map<List<BankMasterModel>>(banks);
                        if (vendorList != null)
                        {
                            TempData["Message"] = PopUpServices.Notify(ValueMapping.addSuccess, vendorName, notificationType: Alerts.success);

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = PopUpServices.Notify(ValueMapping.error, vendorName, notificationType: Alerts.error);
                            return View();
                        }
                    }
                    else
                    {
                        
                        var vendorDetails = _mapper.Map<VendorDetailsDto>(vendorModel);
                        var vendorName = vendorDetails.Name;
                        var command = new CreateVendorCommand { VendorDetailsDto = vendorDetails, DocumentType = vendorModel.FileType, vendorFiles = files, entityPath = ValueMapping.vendorDetailsEntityPath, user = CurrentUser };

                        var vendorId = await _mediator.Send(command);
                        _unitOfWork.Save();
                        _unitOfWork.Commit();

                        //Get Dropdown List 
                        var dropDownList = await _mediator.Send(new GetDropDownListRequest());
                        ViewBag.DropDownList = dropDownList;


                        var banks = await _mediator.Send(new GetBankMasterListRequest { });
                        ViewBag.Banks = _mapper.Map<List<BankMasterModel>>(banks);
                        if (vendorId != 0)
                        {
                            TempData["Message"] = PopUpServices.Notify(ValueMapping.addSuccess, vendorName, notificationType: Alerts.success);

                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Message"] = PopUpServices.Notify(ValueMapping.error, vendorName, notificationType: Alerts.error);
                            return View();
                        }

                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                Log.Information(ValueMapping.createMethod);
                Log.Information(e.InnerException.Message);
                Log.Information(e.Message);
                _unitOfWork.Rollback();
                throw;
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Author: Rajesh V; Date: 05/05/2022
        /// Purpose: Get the list of vendors and bind it Awesome grid along with filter search enabled
        /// ModifiedBy:Swetha M
        /// ModifiedPurpose:Moved Search filter to sepearte class  file
        /// </summary>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        ///<param name="vendorfilters"> </param>
        /// <returns>List of Vendors based of filters</returns>
        public async Task<IActionResult> GetVendorList(VendorFilter vendorfilters,GridParams gridParams,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                forder = forder ?? new string[] { };

                var vendorList = await _mediator.Send(new GetVenorQuerableValue());
                var vendorQuery = vendorList.AsQueryable();

                // filter row search
                var frow = new VendorFilterRow();

                var searchFilter = new VendorSearchFilters();
                var filterBuilder = searchFilter.GetVendorSearchCriteria(vendorList,vendorfilters);

                vendorQuery = await filterBuilder.ApplyAsync(vendorQuery,forder);

                var gmb = new GridModelBuilder<Vendors>(vendorQuery.AsQueryable(),gridParams)
                {
                    KeyProp = o => o.Id,
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

        /// <summary>
        /// Author: Rajesh V; Date: 05/05/2022
        /// Purpose: Bind the values to the awesome grid columns
        /// ModifiedBy:
        /// ModifiedPurpose:
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private object MapToGridModel(Vendors o)
        {
            try
            {
                return new
                {
                    o.Id,
                    o.Name,
                    GstIn_Number = o.GSTIN_Number == null ? "UnRegistered" : o.GSTIN_Number,
                    Pan_Number = o.PAN_Number == null ? "N/A" : o.PAN_Number,
                    Tan_Number = o.TAN_Number == null ? "N/A" : o.TAN_Number,
                    Status = o.Status ? "Active" : "Inactive",
                    Category = o.VendorDefaults == null ? "N/A" : o.VendorDefaults.Category,
                    GstTds = o.VendorDefaults?.GST_TDSPercentage?.ToString("0.00") ?? "0.00",
                    //GstTds = o.VendorDefaults == null ? "0" : o.VendorDefaults.GST_TDSPercentage.ToString("0.00"),
                    //Tds = o.VendorDefaults == null ? 0 : o.VendorDefaults.TDSPercentage.ToString("0.00"),
                    Tds = o.VendorDefaults?.TDSPercentage?.ToString("0.00") ?? "0.00",
                BalanceAmount = o.VendorBalance == null ? "0" : o.VendorBalance.Pending_NetPayable.ToString("0.00"),
                    OpeningBalance = o.VendorBalance == null ? "0" : o.VendorBalance.OpeningBalance.ToString("0.00"),
                    InvoiceAmount = o.VendorBalance == null ? "0" : o.VendorBalance.TotalBillAmount.ToString("0.00"),
                    NetPayable = o.VendorBalance == null ? "0" : o.VendorBalance.TotalNetPayable.ToString("0.00"),
                    TotalPayment = o.VendorBalance == null ? "0" : o.VendorBalance.Paid_NetPayable.ToString("0.00"),
                    Paid_TDS = o.VendorBalance == null ? "0" : o.VendorBalance.Paid_TDS.ToString("0.00"),
                    Paid_GST_TDS = o.VendorBalance == null ? "0" : o.VendorBalance.Paid_GST_TDS.ToString("0.00")
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
        /// Author:Swetha M Date:09/05/2022
        /// Purpose: Get the list of Categories and bind it Awesome grid along with
        /// </summary>
        /// <returns>Payment Status List</returns>
        public async Task<ActionResult> GetCategoryList()
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var categorylist = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.categoryType });
                var categoryQuery = new List<KeyContent>();
                categoryQuery.AddRange(categorylist.Select(o => new KeyContent(o.CodeValue,o.CodeValue)).Distinct());
                return Json(categoryQuery);
            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.getCategoryList);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw;
            }

        }

        /// <summary>
        /// Author:Swetha M Date:13/05/2022
        /// Purpose: Get the list of Categories and bind it Awesome grid along with
        /// </summary>
        /// <returns>category Status List</returns>
        public async Task<ActionResult> GetVendorStatus()
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var vendorList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.vstatus });
                var vendorStatusQuery = new List<KeyContent>();
                vendorStatusQuery.AddRange(vendorList.Select(o => new KeyContent(o.CodeValue,o.CodeValue)).Distinct());
                return Json(vendorStatusQuery);
            }
            catch (Exception ex)
            {
                Log.Information(ValueMapping.getVendorStatus);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }


        /// <summary>
        /// Author:Sandeep M Date:19/05/2022
        /// Purpose: Update the details of vendors
        /// </summary>      
        /// 
        [HttpPost]
        public async Task<ActionResult> Update(VendorViewModel vendorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CreateTransaction();
                    vendorModel.ModifiedBy = CurrentUser;
                    var vendorDetails = _mapper.Map<VendorDetailsDto>(vendorModel);
                    var Id = vendorModel.Id;
              await _mediator.Send(new UpdateVendorDetailsCommand { VendorDetailsDto = vendorDetails,entityPath = ValueMapping.vendorDetailsEntityPath,entityType = ValueMapping.Vendor,user = CurrentUser });

                    _unitOfWork.Save();
                    _unitOfWork.Commit();

                    TempData["Message"] = PopUpServices.Notify(ValueMapping.Update,vendorModel.Name,notificationType: Alerts.success);

                    return RedirectToAction("EditVendor",new { id = Id });
                }


            }
            catch (Exception ex)
            {

                Log.Information(ValueMapping.update);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw;
            }
            return View();
        }

        /// <summary>
        /// Author:Sandeep M Date:20/05/2022
        /// Purpose: Download the Vendor Details Report
        /// ModifiedBy:Swetha M
        /// Date:24/06.2022
        /// Purpose:Catched File Not found Exeption
        /// <returns>Popup is thrown when the file is not found</returns>
        /// </summary> 
        public ActionResult Download(string filePath,string name,string extension,int VendorId)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                byte[] bytes = null;

                if (System.IO.File.Exists(filePath))
                {
                    bytes = System.IO.File.ReadAllBytes(filePath);
                }
                else
                {
                    //TempData["Message"] = PopUpServices.Notify(ValueMapping.DNE, name, notificationType: Alerts.warning);
                    return RedirectToAction("EditVendor",new { Id = VendorId }); 
                }
                _unitOfWork.Save();
                _unitOfWork.Commit();
                //TempData["Message"] = PopUpServices.Notify(ValueMapping.downloadSuccess, name, notificationType: Alerts.success);
                return File(bytes,"application/force-download",string.Concat(name,extension));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "File Not Found";
                throw new Exception();
            }
        }

        /// <summary>
        /// Author:Sandeep M Date:20/05/2022
        /// Purpose: Delete the Vendor Details Report from Database
        /// </summary> 
        public async Task<ActionResult> Delete(int id,int VendorId,string filepath,string name)
        {
            try
            {
                _unitOfWork.CreateTransaction();


                if (System.IO.File.Exists(filepath))
                {
                     await _mediator.Send(new DeleteVendorDocumnetCommand { ID = id });
                    //System.IO.File.Delete(filepath);
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.delSuccess,name,notificationType: Alerts.success);
                }
                else
                {
                    TempData["Message"] = PopUpServices.Notify(ValueMapping.notFound,name,notificationType: Alerts.warning);
                }

                _unitOfWork.Save();
                _unitOfWork.Commit();


                return RedirectToAction("EditVendor",new { Id = VendorId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ValueMapping.notFound;
                Log.Information(ValueMapping.delMethod);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw new Exception();
            }

        }
        /// <summary>
        /// Author:Swetha M Date:18/06/2022
        /// Purpose:dropdown and view binding for Add Vendor Page 
        /// </summary>
        /// <returns>Dropdowns and Add Vendor View</returns>
        [Breadcrumb(ValueMapping.addVendor)]
        [HttpGet]
        public async Task<ActionResult> AddVendor()
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get VendorList            
                var vendorList = await _mediator.Send(new GetVenorQuerableValue());
                var gstNumber = vendorList.ToList().Select(o => o.GSTIN_Number);
                var AccountNumber = vendorList.Select(o => o.VendorBankAccounts.AccountNumber).ToList();
                var panNumber = vendorList.ToList().Select(o => o.PAN_Number);
                ViewBag.GstNumber = gstNumber;
                ViewBag.PanNumber = panNumber;
                ViewBag.AccountNumber = AccountNumber;

                //Get DropDown List
                var dropDownList = await _mediator.Send(new GetDropDownListRequest());
                ViewBag.DropDownList = dropDownList;

                //Get Bank Details
                //var banks = await _mediator.Send(new GetBankMasterListRequest { });
                var banks = await _mediator.Send(new GetBankDetailsRequest { });
                //ViewBag.Banks = _mapper.Map<List<BankMasterModel>>(banks);
                ViewBag.Banks = _mapper.Map<List<BranchMasterModel>>(banks);
                ViewBag.UserName = CurrentUser;
                ModelState.Clear();

            }
            catch (Exception ex)
            {

                Log.Information(ValueMapping.createMethod);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw;
            }

            ModelState.Clear();
            return View();
        }

        /// <summary>
        /// Author:Swetha M Date:18/06/2022
        /// Purpose: Save the vendor details to database
        /// </summary>
        /// <returns>added vendor id</returns>
        [HttpPost]
        public async Task<ActionResult> AddVendor(VendorViewModel vendorinfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    vendorinfo.CreatedBy = CurrentUser;

                    


                    _unitOfWork.CreateTransaction();
                    var vendorDetails = _mapper.Map<VendorDetailsDto>(vendorinfo);
                    //var Balancedetail = _mapper.Map<VendorBalanceDto>(Balancedetails);
                    var addCommand = new AddVendorCommand { vendor = vendorDetails  };

                    var vendorId = await _mediator.Send(addCommand);
                    _unitOfWork.Save();
                    _unitOfWork.Commit();
                    vendorinfo.Id = vendorId;
                    ModelState.Clear();
                    return Ok(new { data = vendorId });
                }

            }
            catch (Exception)
            {
                Log.Error(ValueMapping.failed);
                _unitOfWork.Rollback();
            }
            ModelState.Clear();
            return View();
        }
        /// <summary>
        /// Author:Swetha M Date:13/05/2022
        /// Purpose: Get the Existing details of vendor and Edit
        /// </summary>
        /// <returns>Gets the existing data of particular vendor</returns>

        [Breadcrumb(ValueMapping.editVendor)]
        [HttpGet]

        public async Task<IActionResult> EditVendor(int id)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Vendor List
                var vendorlist = await _mediator.Send(new GetVenorQuerableValue());
                var gstNumber = vendorlist.ToList().Select(o => o.GSTIN_Number);
                var panNumber = vendorlist.ToList().Select(o => o.PAN_Number);
                var AccountNumber = vendorlist.Select(o => o.VendorBankAccounts.AccountNumber).ToList();
                ViewBag.GstNumber = gstNumber;
                ViewBag.PanNumber = panNumber;
                ViewBag.AccountNumber = AccountNumber;

                //Get the VendorList by ID
                var vendorListByID = await _mediator.Send(new GetVendorListByIDRequest { ID = id });
                var vendorDetails = _mapper.Map<VendorViewModel>(vendorListByID);

                //Get the Document Details
                var documentDetails = await _mediator.Send(new GetVendorDocumentDetailsRequest { ID = id });
                ViewBag.DocumentDetails = _mapper.Map<List<DocumentsModel>>(documentDetails);

                //Get the DropDown list
                var dropDownList = await _mediator.Send(new GetDropDownListRequest());
                ViewBag.DropDownList = dropDownList;


                //  var citues = dropDownList.Cities.Where (x => x.CodeValue == "Bangalore");
                //Get the bank Details
                // var banks = await _mediator.Send(new GetBankMasterListRequest { });

             
                
                var banks = await _mediator.Send(new GetBankDetailsRequest { });
                ViewBag.Banks = _mapper.Map<List<BranchMasterModel>>(banks);
                //ViewBag.Banks = _mapper.Map<List<BankMasterModel>>(banks);
                var advancePayments = await _mediator.Send(new GetAdvancePaymentByVendorRequest { vendorID = id });
                
                ViewBag.AdvancePayments = _mapper.Map<List<PaymentViewModel>>(advancePayments);
                ViewBag.UserName = CurrentUser;
                ViewBag.Id = id;
                ViewBag.VendorBalanceDetails = vendorDetails.VendorBalance;

                
                return View(vendorDetails);
            }
            catch (Exception ex)
            {

                Log.Information(ValueMapping.vendorD);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw;
            }
        }
        /// <summary>
        /// Author:Swetha M Date:18/06/2022
        /// Purpose:Save Edited vendor details to database
        /// </summary>
        /// <returns>Edit vendor details</returns>

        [HttpPost]

        [Breadcrumb(ValueMapping.editVendor)]
        public async Task<IActionResult> EditVendor(VendorViewModel vendorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CreateTransaction();
                    vendorModel.ModifiedBy = CurrentUser;
                    vendorModel.CreatedBy = CurrentUser;
                    vendorModel.ModifiedOn = DateTime.UtcNow;
                    var vendorDetails = _mapper.Map<VendorDetailsDto>(vendorModel);
                    var Id = vendorModel.Id;
                    //Edit the vendor details
                    var editCommand = new EditVendorCommand { vendorDetailsDto = vendorDetails };
                   await _mediator.Send(editCommand);
                    return RedirectToAction("EditVendor",new { id = Id });
                }
            }
            catch (Exception ex)
            {

                Log.Information(ValueMapping.update);
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw;
            }

            return RedirectToAction("EditVendor",new { id = vendorModel.Id });
        }

        // [Breadcrumb(ValueMapping.listOfTransaction, FromAction = "Vendor.Index")]
        //[Breadcrumb("ViewData.Title", CacheFile = true, FromAction = "Vendor.Index")]
        //[Breadcrumb("ViewData.Title", FromAction = "Index")]
        //[Breadcrumb("ViewData.Title",  FromAction = "Vendor.Index")]
        public async Task<IActionResult> TransactionList(int id)
        {

            var childNode1 = new MvcBreadcrumbNode("Index","Vendor","Vendor List",false)
            {
                //this comes in as a param into the action
            };

            var childNode2 = new MvcBreadcrumbNode("EditVendor","Vendor","Edit Vendor")
            {
                RouteValues = new { id },
                OverwriteTitleOnExactMatch = true,
                Parent = childNode1
            };

            var childNode3 = new MvcBreadcrumbNode("Index","Vendor","List of Bill & Payment")
            {
                OverwriteTitleOnExactMatch = true,
                Parent = childNode2
            };

            ViewData["BreadcrumbNode"] = childNode3;

            // All you have to do now is tell SmartBreadcrumbs about this
            var paymentList = await _mediator.Send(new GetVenorQuerableValue());
            IQueryable<Vendors> paymentQuery = paymentList.Where(x => x.Id == id).Distinct().AsQueryable();
            ViewBag.Name = paymentQuery.First().Name;
            ViewBag.UserName = CurrentUser;
            ViewBag.Id = id;
            return View();
        }



        public async Task<ActionResult> GetListOfTransactions(GridParams gridParams,string[] forder, ListOfTransactionFilter transactionFilter, int Id)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Payment List
                var transactionList = await _mediator.Send(new GetBillPaymentsByVendorRequest { VendorId = Id});

                var frow = new ListOfTransactionFilterRow();
                ListOfTransactionSearchFilters searchFilter = new ListOfTransactionSearchFilters();

                //Get Payment Searchfilters
                var filterBuilder = searchFilter.GetListOfTransactionSearchCriteria(transactionList, transactionFilter);
                transactionList = await filterBuilder.ApplyAsync(transactionList, forder);

                ViewBag.Name = transactionFilter.name;

                var gmb = new GridModelBuilder<Transaction>(transactionList.AsQueryable(),gridParams)
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

        private object MapToGridModel(Transaction o)
        {
            try
            {

                return new
                {
                   o.ID,
                   o.Status,
                   o.TransactionType,
                   o.BillReferenceNo,
                   Amount = o.TransactionType=="P"? "-"+o.Amount.ToString("0.00"): o.Amount.ToString("0.00"),
                   TransactionDate=o.TransactionDate.ToString("dd-MM-yyyy"),
                   o.CreatedBy,
                   o.BillNo,
                   o.ApprovedBy,
                   o.Type,
                    AdvanceAmountUsed=o.AdvanceAmountUsed.ToString("0.00")

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
        public async Task<ActionResult> GetBranchDetails(int id)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Payment Details
                    var BranchDetails = await _mediator.Send(new GetBranchByBankIdRequest { Id = id });
                var output = BranchDetails.Select(x => new {Isvalid=true, x.branch_name, x.branch_ifsc,x.branch_id }).OrderBy(x => x.branch_name).Distinct().ToList();
                return Ok(output);
            }
            catch(Exception ex) {
                Log.Information(ex.InnerException.Message);
                Log.Information(ex.Message);
                _unitOfWork.Rollback();
                throw;
            }

           // ModelState.Clear();
           

        }





        [HttpGet]
        public async Task<ActionResult> ViewTransactionHistory(int id,int vendor,string RefNo)
        {
            try
            {
                var childNode1 = new MvcBreadcrumbNode("Index","Vendor","Vendor List",false)
                {
                    //this comes in as a param into the action
                };

                var childNode2 = new MvcBreadcrumbNode("EditVendor","Vendor","Edit Vendor")
                {
                    RouteValues = new { id = vendor },
                    OverwriteTitleOnExactMatch = true,
                    Parent = childNode1
                };

                var childNode3 = new MvcBreadcrumbNode("TransactionList","Vendor","List of Bill & Payment")
                {
                    RouteValues = new { id = vendor },
                    OverwriteTitleOnExactMatch = true,
                    Parent = childNode2


                };
                var childNode4 = new MvcBreadcrumbNode("Index","Vendor","Transaction Details")
                {
                    OverwriteTitleOnExactMatch = true,
                    Parent = childNode3
                };

                ViewData["BreadcrumbNode"] = childNode4;

                _unitOfWork.CreateTransaction();
                //Get Payment Details
                if(RefNo.StartsWith("P"))
                {
                    var paymentDetails = await _mediator.Send(new GetPaymentsByIdRequest { Id = id });
                    var payments = _mapper.Map<VendorPaymentViewModel>(paymentDetails);
                    ViewBag.VendorBalanceDetails = payments.Vendor.VendorBalance;
                    var billpaymentDetails = await _mediator.Send(new GetBillPaymentRequest { PaymentID = id });
                    ViewBag.UserName = CurrentUser;
                    ViewBag.PaymentDetails = billpaymentDetails.ToList();
                    return View(payments);
                }else if (RefNo.StartsWith("A"))
                {
                    return RedirectToAction("ApprovePayment", "Payment", new { id = id });
                }
                else
                {
                    return RedirectToAction("BillDetails", "Bill" , new { id = id  });
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
        /// Purpose: File Download
        /// Author: Swetha M; Date: 21/11/2022        
        /// </summary>
        /// <param name="vendorfilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        /// <returns></returns>   
        public async Task<GridModel<Vendors>> GetVendorListToDownload(VendorFilter vendorfilters,GridParams gridParams,string[] forder)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                forder = forder ?? new string[] { };

                var vendorList = await _mediator.Send(new GetVenorQuerableValue());
                var vendorQuery = vendorList.AsQueryable();

                // filter row search
                var frow = new VendorFilterRow();

                var searchFilter = new VendorSearchFilters();
                var filterBuilder = searchFilter.GetVendorSearchCriteria(vendorList,vendorfilters);

                vendorQuery = await filterBuilder.ApplyAsync(vendorQuery,forder);

                var gmb = new GridModelBuilder<Vendors>(vendorQuery.AsQueryable(),gridParams)
                {
                    KeyProp = o => o.Id,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };

                return (await gmb.BuildModelAsync());
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
        /// <summary>        
        /// Purpose: Export Bank File List
        /// Author: Swetha M; Date: 09/11/2022        
        /// </summary>
        /// <param name="vendorfilters"></param>
        /// <param name="gridParams"></param>
        /// <param name="forder"></param>
        /// <returns></returns>        
        [HttpPost]
        public async Task<IActionResult> ExportToGridVendorList(VendorFilter vendorfilters,GridParams gridParams,string[] forder)
        {
            try
            {

                var gridModel = await GetVendorListToDownload(vendorfilters,gridParams,forder);
                var expColumns = new[]
                {

                    new ExpColumn { Name = "Name", Width = 2.5f, Header = "Vendor Name" },
                    new ExpColumn { Name = "GstIn_Number", Width = 2.5f, Header = "GST IN Number " },
                    new ExpColumn { Name = "Pan_Number", Width = 2.5f, Header = "PAN Number " },
                    new ExpColumn { Name = "Tan_Number", Width = 2.5f, Header = "TAN Number " },
                    new ExpColumn { Name = "Category", Width = 3f, Header = "Category" },
                    new ExpColumn { Name = "OpeningBalance", Width = 3f, Header ="₹ Opening Balance" },
                    new ExpColumn { Name = "NetPayable", Width = 2f, Header="₹ Net Payable " },
                    new ExpColumn { Name = "TotalPayment", Width = 2f, Header="₹ TotalPayment " },
                    new ExpColumn { Name = "BalanceAmount", Width = 2f, Header="₹ Balance " },
                    new ExpColumn { Name = "Tds", Width = 2f, Header="TDS % " },
                    new ExpColumn { Name = "BalanceAmount", Width = 2f, Header="₹ TDS Paid" },
                    new ExpColumn { Name = "GstTds", Width = 2f, Header="GST-TDS % " },
                    new ExpColumn { Name = "BalanceAmount", Width = 2f, Header="₹ GST-TDS Paid" },
                    new ExpColumn { Name = "Status", Width = 2f, Header="Vendor Status" },

                };

                var workbook = (new GridExcelBuilder(expColumns)).Build(gridModel);

                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(),"application/vnd.ms-excel","VendorList.xls");
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
        /// Purpose: File Download
        /// Author: Sandeep; Date: 23/06/2022        
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
                DownloadService<VendorListDto> dl = new();
                List<VendorListDto> listOfVendor = new();
                listOfVendor = await _mediator.Send(new GetVendorListRequestIEnumerable());
                dl.ListItems = listOfVendor;
                dl.FileName = fileName;
                var fc = new FileContentResult(dl.GetFile(),dl.ContentType());
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


        public async Task<GridModel<Transaction>> GetListOfTransactionsToDownload(GridParams gridParams, string[] forder, ListOfTransactionFilter transactionFilter, int Id)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                //Get Payment List
                var transactionList = await _mediator.Send(new GetBillPaymentsByVendorRequest { VendorId = Id });

                var frow = new ListOfTransactionFilterRow();
                ListOfTransactionSearchFilters searchFilter = new ListOfTransactionSearchFilters();

                //Get Payment Searchfilters
                var filterBuilder = searchFilter.GetListOfTransactionSearchCriteria(transactionList, transactionFilter);
                transactionList = await filterBuilder.ApplyAsync(transactionList, forder);

                ViewBag.Name = transactionFilter.name;

                var gmb = new GridModelBuilder<Transaction>(transactionList.AsQueryable(), gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = MapToGridModel,
                    Tag = new { frow },
                    ToArrayAsync = qitems => qitems.ToArrayAsync(),
                };

                ViewBag.UserName = CurrentUser;
                return await  gmb.BuildModelAsync();
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
        public async Task<IActionResult> ExportToTransactions(ListOfTransactionFilter transactionFilter, int Id ,string[] forder, GridParams gridParams)
        {
            try
            {

                var gridModel = await GetListOfTransactionsToDownload(gridParams, forder, transactionFilter,Id);
                var expColumns = new[]
                {
                    new ExpColumn { Name = "BillReferenceNo", Width = 2.5f, Header = "Transaction Ref No" },
                    new ExpColumn { Name = "BillNo", Width = 2.5f, Header = "Bill No" },
                    new ExpColumn { Name = "Type", Width = 2.5f, Header = "Type" },
                    new ExpColumn { Name = "TransactionDate", Width = 2.5f, Header = "Transaction Date (dd-mm-yyy)" },
                    new ExpColumn { Name = "Amount", Width = 3f, Header = "Amount ₹" },
                    new ExpColumn { Name = "AdvanceAmountUsed", Width = 3f, Header = "Advance Amount Used ₹" },
                    new ExpColumn { Name = "TransactionType", Width = 3f, Header ="Transaction Type" },
                    new ExpColumn { Name = "CreatedBy", Width = 2f, Header="Created By" },
                    new ExpColumn { Name = "ApprovedBy", Width = 2f, Header="Approved By" },
                    new ExpColumn { Name = "Status", Width = 2f, Header="Status" }
                };
                var workbook = (new GridExcelBuilder(expColumns)).Build(gridModel);
                using (var stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    stream.Close();
                    return File(stream.ToArray(), "application/vnd.ms-excel", "TransactionList.xls");
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
