using Application.DTOs.Filters;
using Application.UserStories.Bank.Requests.Queries;
using Application.UserStories.Master.Handler.Queries;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Reconcile.Requests.Commands;
using Application.UserStories.Transactions.Requests.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Bank;
using Domain.Transactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Omu.AwesomeMvc;
using Persistence;
using Presentation.GridFilters.Reconcile;
using Presentation.Models.Bank;
using Presentation.Services.Infra.Cache;
using Serilog;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers.Reconsile
{
    public class ReconcileController : Controller
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ICacheManager cache;
        private readonly AccountingDbContext _dbContext;
        private readonly IUnitOfWork<AccountingDbContext> _unitOfWork;
        // private GenericRepository<Re>
        protected string CurrentUser => this.HttpContext.User.Identity.Name;
        protected bool Role => this.HttpContext.User.IsInRole("Administrator");

        public ReconcileController(IMediator mediator, IMapper mapper, AccountingDbContext dbContext, IUnitOfWork<AccountingDbContext> unitOfWork)
        {
            _mediator = mediator;
            _mapper = mapper;
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;


        }


        [Breadcrumb(ValueMapping.Reconcile)]
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
                throw e;
            }
        }
        public async Task<IActionResult> BankTransactionList(ReconcileFilter Reconcilefilter, GridParams gridParams, string[] forder)
        {
            try
            {
                var list = await _mediator.Send(new GetBankTransactionRequest());
                var frow = new ReconcileFilterRow();
                var searchFilter = new ReconcileSearchFilters();
                var filterBuilder = searchFilter.GetReconcileSearchCriteria(list, Reconcilefilter);
                var query = await filterBuilder.ApplyAsync(list, forder);
                var gmb = new GridModelBuilder<BankTransactions>(query, gridParams)
                {
                    KeyProp = o => o.Id,
                    Map = MapToGridModel,
                    Tag = new { frow = frow },
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
        private object MapToGridModel(BankTransactions o)
        {
            try
            {
                return new
                {
                    o.Id,
                    Transaction_Date = o.Transaction_Date.ToString("dd-MM-yyyy"),
                    Value_Date = o.Value_Date.ToString("dd-MM-yyyy"),
                    Credit = o.Credit == null ? 0 : o.Credit,
                    Debit = o.Debit == null ? 0 : o.Debit,
                    Balance = o.Balance.ToString("0.00"),
                    AccountNo = o.AccountNo,
                    BankName = o.BankName == null ? "" : o.BankName,
                    Status = o.Status,
                    Description=o.Description
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// Author:Swetha M Date:18/04/2023
        /// Purpose: Get the list of Status and bind it Awesome grid along with
        /// </summary>
        /// <returns>Reconcile Status List</returns>
        public async Task<ActionResult> GetReconcileStatus()
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var reconcileStatuslist = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "ReconcileStatus" });
                var sttasusQuery = new List<KeyContent>();
                sttasusQuery.AddRange(reconcileStatuslist.Select(o => new KeyContent(o.CodeValue, o.CodeValue)).Distinct());
                return Json(sttasusQuery);
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
        /// Author:Swetha M Date:18/04/2023
        /// Purpose: Get the list of Status and bind it Awesome grid along with
        /// </summary>
        /// <returns>Reconcile Status List</returns>
        public async Task<ActionResult> GetBankList()
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var banks = await _mediator.Send(new GetAllBanksListRequest  { });
                var sttasusQuery = new List<KeyContent>();
                sttasusQuery.AddRange(banks.Select(o => new KeyContent(o.BankName, o.BankName)).Distinct());
                return Json(sttasusQuery);
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
        /// Author:Swetha M Date:18/04/2023
        /// Purpose: Get the list of Status and bind it Awesome grid along with
        /// </summary>
        /// <returns>Reconcile Status List</returns>
        public async Task<ActionResult> GetTransactionType()
        {
            try
            {
                _unitOfWork.CreateTransaction();


                var reconcileStatuslist = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "SystemName" });
                var sttasusQuery = new List<KeyContent>();
                sttasusQuery.AddRange(reconcileStatuslist.Select(o => new KeyContent(o.CodeValue, o.CodeValue)).Distinct());
                return Json(sttasusQuery);
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
        /// Author:Swetha M Date:18/04/2023
        /// Purpose: Get the list of Status and bind it Awesome grid along with
        /// </summary>
        /// <returns>Reconcile Status List</returns>
        public async Task<ActionResult> GetBankTransactionType()
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var bankTransctionType = new List<string>
                {
                    "Debit",
                    "Credit"
                };
                var sttasusQuery = new List<KeyContent>();
                sttasusQuery.AddRange(bankTransctionType.Select(o => new KeyContent(o, o)).Distinct());
                return Json(sttasusQuery);
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
        /// Author:Swetha M Date:18/04/2023
        /// Purpose: Get the list of Status and bind it Awesome grid along with
        /// </summary>
        /// <returns>Reconcile Status List</returns>
        /// 
        [HttpGet]
        public async Task<ActionResult> Reconcile(int bankTransactionId)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var reconcile = await _mediator.Send(new GetBankTransactionForReconcileRequest { bankTransactionId = bankTransactionId });
                var reconcileViewModel = _mapper.Map<List<ReconciliationModel>>(reconcile);

                _unitOfWork.Commit();
                _unitOfWork.Save();
                return View(reconcileViewModel);
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
        /// Author:Swetha M Date:18/04/2023
        /// Purpose: Get the list of Status and bind it Awesome grid along with
        /// </summary>
        /// <returns>Reconcile Status List</returns>
        /// 
        [HttpGet]
        [Breadcrumb("Manually Reconcile")]

        public async Task<ActionResult> ManuallyReconcile(int bankTransactionId)
        {
            try
            {

                _unitOfWork.CreateTransaction();
                var bankTransaction = await _mediator.Send(new GetBankTransactionByIdRequest { bankTransactionId = bankTransactionId });
                var bankTransactionViewModel = _mapper.Map<BankTransactionModel>(bankTransaction);
                bankTransactionViewModel.Credit= bankTransactionViewModel.Credit==null ? 0 : bankTransactionViewModel.Credit;
                bankTransactionViewModel.Debit=bankTransactionViewModel.Debit==null?0 : bankTransactionViewModel.Debit; 
                _unitOfWork.Commit();
                _unitOfWork.Save();
                ViewBag.UserName = CurrentUser;
                ViewBag.Role = Role;
                return View(bankTransactionViewModel);
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
        public async Task<IActionResult> GetListOfTransactions(ReconcileFilter manuallyReoncileFilter, GridParams gridParams, string[] forder)
        {
            try
            {
                ReconcileDto reconcileDto = new ReconcileDto
                {
                    Date = manuallyReoncileFilter.Date,
                    Status = manuallyReoncileFilter.Status,
                    TransactionType = manuallyReoncileFilter.TransactionType,
                    PayableAmount = manuallyReoncileFilter.PayableAmount,
                    PayableMaxAmount = manuallyReoncileFilter.PayableMaxAmount,
                    Bankname = manuallyReoncileFilter.Bankname,
                    AccountNumber = manuallyReoncileFilter.AccountNumber,
                };
                var frow = new ReconcileFilterRow();
                _unitOfWork.CreateTransaction();
                                var listOfTransactions = await _mediator.Send(new GetListOfTransactionsByFiltersRequest { reconcileDto = reconcileDto });
                _unitOfWork.Commit();
                _unitOfWork.Save();
                ViewBag.UserName = CurrentUser;
                ViewBag.Role = Role;
                var gmb = new GridModelBuilder<TransactionsSummary>(listOfTransactions, gridParams)
                {
                    KeyProp = o => o.ID,
                    Map = MapToGridModel1,
                    Tag = new { frow = frow },
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
        /// Author: Swetha M; Date: 25/04/2023
        /// ModifiedBy:
        /// ModifiedPurpose:
        /// </summary>
        /// <param name="o"></param>
        private object MapToGridModel1(TransactionsSummary o)
        {
            try
            {
                return new
                {
                    o.ID,
                    TransactionDate = o.TransactionGeneratedDate.ToString("dd-MM-yyyy"),
                    o.SystemName,
                    o.TransactionDetailedType,
                    o.PaymentMode,
                    UtrNumberforBank= o.UTRNumber,
                    o.Status,
                    o.Cheque_No,
                    o.Amount,
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Reconcile(List<int> transactionsid, int bankTransactionId)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var reconcile = await _mediator.Send(new GetBankTransactionForReconcileRequest { bankTransactionId = bankTransactionId });
                var unselectedTransactions = reconcile.Where(x => !transactionsid.Contains(x.Id)).ToList();
                var selectedTransactions = reconcile.Where(x => transactionsid.Contains(x.Id)).ToList();

                var reconciledTransactions = await _mediator.Send(new UpdateReconciledTransactionsCommand
                {
                    bankTransactionId = bankTransactionId,
                    selectedTransactions = selectedTransactions,
                    unSelectedTransactions = unselectedTransactions,
                    currentUser = CurrentUser,
                    selectedTransactionsId= transactionsid
                });

                _unitOfWork.Commit();
                _unitOfWork.Save();
                return RedirectToAction("Index");
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
        [HttpPost]
        public async Task<ActionResult> UnMatch(List<int> transactionsid, int bankTransactionId)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var reconcile = await _mediator.Send(new GetBankTransactionForReconcileRequest { bankTransactionId = bankTransactionId });
                var totalMatchedTransactions = reconcile.Where(x => x.ReconcileStatus == "Matched").Select(x => x.TransactionsId).ToList();
                var unselectedTransactions = reconcile.Where(x => x.ReconcileStatus == "Matched" && !transactionsid.Contains(x.Id))
                   .Select(x => x.Id).ToList();
                var selectedTransactions = reconcile.Where(x => x.ReconcileStatus == "Matched" && transactionsid.Contains(x.Id))
                   .ToList();

                if (transactionsid.Count() == totalMatchedTransactions.Count())
                {
                    var unmatchTransactions = await _mediator.Send(new UnMatchTransactionsCommand
                    {
                        bankTransactionId = bankTransactionId,
                        selectedTransactions = reconcile,
                        currentUser = CurrentUser,
                    });
                }
                else
                {
                    var unmatchTransactions = await _mediator.Send(new UnMatchTransactionsCommand
                    {
                        bankTransactionId = bankTransactionId,
                        selectedTransactions = selectedTransactions,
                        currentUser = CurrentUser,
                        unSelectedtransactions = unselectedTransactions
                    });
                }

                _unitOfWork.Commit();
                _unitOfWork.Save();
                return RedirectToAction("Index");
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
        [HttpPost]
        public async Task<ActionResult> ManuallyReconcile(List<TransactionsSummary> transactions, int bankTransactionId)
        {
            try
            {
                _unitOfWork.CreateTransaction();
                var bankTransaction = await _mediator.Send(new GetBankTransactionByIdRequest { bankTransactionId = bankTransactionId });

                var unmatchTransactions = await _mediator.Send(new ManuallyReconcileTransactionsCommand
                {
                    BankTransaction= bankTransaction,
                    TransactionsSummary = transactions,
                    currentUser = CurrentUser,
                });

                _unitOfWork.Commit();
                _unitOfWork.Save();
                return RedirectToAction("Index");
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

    }
}
