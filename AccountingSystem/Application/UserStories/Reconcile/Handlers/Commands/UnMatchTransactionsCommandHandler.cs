using Application.Interface.Persistence.Bank;
using Application.Interface.Persistence.Reconcile;
using Application.Interface.Persistence.Transactions;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Reconcile.Requests.Commands;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using Domain.Bank;
using Domain.CessTransactions;
using Domain.Transactions;
using Domain.TransactionsBenefits;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Reconcile.Handlers.Commands
{
    public class UnMatchTransactionsCommandHandler : IRequestHandler<UnMatchTransactionsCommand, int>
    {
        private readonly ITransactionSummaryRepository _transactionSummaryRepository;
        private readonly ITransactionsCessRepository _transactionCessRepository;
        private readonly ITransactionsBenefitsRepository _transactionBenefitsRepository;
        private readonly IBankTransactionRepository _banktransactionsRepository;
        private readonly IReconciliationRepository _reconcileRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public UnMatchTransactionsCommandHandler(ITransactionSummaryRepository transactionSummaryRepository, IBankTransactionRepository banktransactionsRepository,
            IReconciliationRepository reconcileRepository, IMapper mapper, IMediator mediator, ITransactionsCessRepository transactionsCess, ITransactionsBenefitsRepository benefitsRepository)
        {
            _transactionSummaryRepository = transactionSummaryRepository;
            _banktransactionsRepository = banktransactionsRepository;
            _reconcileRepository = reconcileRepository;
            _mapper = mapper;
            _mediator = mediator;
            _transactionBenefitsRepository = benefitsRepository;
            _transactionCessRepository = transactionsCess;

        }
        public async Task<int> Handle(UnMatchTransactionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<TransactionsSummary> trnasactionSummaryList = new();
                List<TransactionsCess> transactionsCessList = new();
                List<TransactionsBenefits> transactionsBenefits = new();

                var reconcileStatus = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "ReconcileStatus" });
                if (request.unSelectedtransactions == null)
                {
                    #region Umatching if all the transactions are selected

                    var unMatchedTransactions = _mapper.Map<List<Reconciliation>>(request.selectedTransactions);
                    foreach (var transaction in unMatchedTransactions)
                    {
                        transaction.ReconcileStatus = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                        transaction.UpdatedDate = DateTime.Now;
                        transaction.UpdatedBy = request.currentUser;
                        transaction.IsActive = false;

                    }
                    await _reconcileRepository.UpdateReconciledTransactions(unMatchedTransactions);

                    var unselectedOtherTransactions = await _reconcileRepository.GetReconciliationsByTransactionIds(unMatchedTransactions, request.bankTransactionId);
                    var unselectedIds = unselectedOtherTransactions.Select(x => x.TransactionsId).ToList();

                    var notMatchedTransactionsinRecon = unMatchedTransactions.Where(x => !unselectedIds.Contains(x.TransactionsId)).ToList();
                    if (unselectedOtherTransactions.Count() == 0)
                    {
                        foreach (var transaction in unMatchedTransactions)
                        {
                            switch (transaction.SystemName)
                            {
                                case "Accounts":
                                    var transactionSummary = transaction.TransactionsSummary;
                                    transactionSummary.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionSummary.IsMatched = false;
                                    trnasactionSummaryList.Add(transactionSummary);
                                    break;
                                case "CESS":
                                    var transactionCess = _mapper.Map<TransactionsCess>(transaction.TransactionsSummary);
                                    transactionCess.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionCess.IsMatched = false;

                                    transactionsCessList.Add(transactionCess);

                                    break;
                                case "Benefits":
                                    var transactionBenefit = _mapper.Map<TransactionsBenefits>(transaction.TransactionsSummary);
                                    transactionBenefit.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionBenefit.IsMatched = false;
                                    transactionsBenefits.Add(transactionBenefit);

                                    break;

                            }
                        }
                    }
                    else if (notMatchedTransactionsinRecon.Any())
                    {

                        foreach (var transaction in notMatchedTransactionsinRecon)
                        {
                            switch (transaction.SystemName)
                            {
                                case "Accounts":
                                    var transactionSummary = transaction.TransactionsSummary;
                                    transactionSummary.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionSummary.IsMatched = false;
                                    trnasactionSummaryList.Add(transactionSummary);
                                    break;
                                case "CESS":
                                    var transactionCess = _mapper.Map<TransactionsCess>(transaction.TransactionsSummary);
                                    transactionCess.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionCess.IsMatched = false;

                                    transactionsCessList.Add(transactionCess);

                                    break;
                                case "Benefits":
                                    var transactionBenefit = _mapper.Map<TransactionsBenefits>(transaction.TransactionsSummary);
                                    transactionBenefit.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionBenefit.IsMatched = false;
                                    transactionsBenefits.Add(transactionBenefit);

                                    break;

                            }
                        }
                    }


                    if (trnasactionSummaryList.Count() > 0)
                    {
                        var updateTransactionStatus = await _mediator.Send(new UpdateTransactionReconcileStatusCommand { TransactionsSummary = trnasactionSummaryList });

                        await _transactionSummaryRepository.UpdateListOfTransactions(trnasactionSummaryList);

                    }
                    if (transactionsCessList.Count() > 0)
                    {
                        await _transactionCessRepository.UpdateListCessTransaction(transactionsCessList);
                    }
                    if (transactionsBenefits.Count() > 0)
                    {
                        await _transactionBenefitsRepository.UpdateListtOfBenefitsTransaction(transactionsBenefits);

                    }
                    //Update bankTransactions

                    var bankTransactions = await _banktransactionsRepository.GetBankTransactionById(request.bankTransactionId);
                    bankTransactions.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                    await _banktransactionsRepository.UpdateBankTransactionsStatus(bankTransactions);

                    #endregion
                }
                else
                {
                    #region Unmatching if any of the transactions are selected
                    var unMatchedTransactions = _mapper.Map<List<Reconciliation>>(request.selectedTransactions);
                    foreach (var transaction in unMatchedTransactions)
                    {
                        transaction.ReconcileStatus = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                        transaction.UpdatedDate = DateTime.Now;
                        transaction.UpdatedBy = request.currentUser;
                        transaction.IsActive = false;
                    }
                    await _reconcileRepository.UpdateReconciledTransactions(unMatchedTransactions);


                    var unselectedOtherTransactions = await _reconcileRepository.GetReconciliationsByTransactionIds(unMatchedTransactions, request.bankTransactionId);
                    var unselectedIds = unselectedOtherTransactions.Select(x => x.TransactionsId).ToList();

                    var notMatchedTransactionsinRecon = unMatchedTransactions.Where(x => !unselectedIds.Contains(x.TransactionsId)).ToList();
                    if (unselectedOtherTransactions.Count() == 0)
                    {
                        foreach (var transaction in unMatchedTransactions)
                        {
                            switch (transaction.SystemName)
                            {
                                case "Accounts":
                                    var transactionSummary = transaction.TransactionsSummary;
                                    transactionSummary.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionSummary.IsMatched = false;
                                    trnasactionSummaryList.Add(transactionSummary);
                                    break;
                                case "CESS":
                                    var transactionCess = _mapper.Map<TransactionsCess>(transaction.TransactionsSummary);
                                    transactionCess.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionCess.IsMatched = false;

                                    transactionsCessList.Add(transactionCess);

                                    break;
                                case "Benefits":
                                    var transactionBenefit = _mapper.Map<TransactionsBenefits>(transaction.TransactionsSummary);
                                    transactionBenefit.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionBenefit.IsMatched = false;
                                    transactionsBenefits.Add(transactionBenefit);

                                    break;

                            }
                        }
                    }
                    else if (notMatchedTransactionsinRecon.Any())
                    {

                        foreach (var transaction in notMatchedTransactionsinRecon)
                        {
                            switch (transaction.SystemName)
                            {
                                case "Accounts":
                                    var transactionSummary = transaction.TransactionsSummary;
                                    transactionSummary.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionSummary.IsMatched = false;
                                    trnasactionSummaryList.Add(transactionSummary);
                                    break;
                                case "CESS":
                                    var transactionCess = _mapper.Map<TransactionsCess>(transaction.TransactionsSummary);
                                    transactionCess.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionCess.IsMatched = false;

                                    transactionsCessList.Add(transactionCess);

                                    break;
                                case "Benefits":
                                    var transactionBenefit = _mapper.Map<TransactionsBenefits>(transaction.TransactionsSummary);
                                    transactionBenefit.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                                    transactionBenefit.IsMatched = false;
                                    transactionsBenefits.Add(transactionBenefit);

                                    break;

                            }
                        }
                    }
                    if (trnasactionSummaryList.Count() > 0)
                    {
                        var updateTransactionStatus = await _mediator.Send(new UpdateTransactionReconcileStatusCommand { TransactionsSummary = trnasactionSummaryList });

                        await _transactionSummaryRepository.UpdateListOfTransactions(trnasactionSummaryList);
                    }
                    if (transactionsCessList.Count() > 0)
                    {
                        await _transactionCessRepository.UpdateListCessTransaction(transactionsCessList);
                    }
                    if (transactionsBenefits.Count() > 0)
                    {
                        await _transactionBenefitsRepository.UpdateListtOfBenefitsTransaction(transactionsBenefits);
                    }
                    #endregion
                }



                return request.bankTransactionId;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
