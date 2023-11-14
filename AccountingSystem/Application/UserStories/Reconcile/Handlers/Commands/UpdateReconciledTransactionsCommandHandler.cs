using Application.Interface.Persistence.Bank;
using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Reconcile;
using Application.Interface.Persistence.Transactions;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Reconcile.Requests.Commands;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Domain.Bank;
using Domain.CessTransactions;
using Domain.Payment;
using Domain.Transactions;
using Domain.TransactionsBenefits;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Reconcile.Handlers.Commands
{
    public class UpdateReconciledTransactionsCommandHandler : IRequestHandler<UpdateReconciledTransactionsCommand, int>
    {
        private readonly ITransactionSummaryRepository _transactionSummaryRepository;
        private readonly ITransactionsCessRepository _transactionCessRepository;
        private readonly ITransactionsBenefitsRepository _transactionBenefitsRepository;
        private readonly IBankTransactionRepository _banktransactionsRepository;
        private readonly IReconciliationRepository _reconcileRepository;
        private readonly IBankFileRepository _bankFileRepository;
        private readonly IPaymentRepository _vendorPaymentRepository;

        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public UpdateReconciledTransactionsCommandHandler(ITransactionSummaryRepository transactionSummaryRepository, IBankTransactionRepository banktransactionsRepository,
            IReconciliationRepository reconcileRepository, IMapper mapper, IMediator mediator, ITransactionsCessRepository transactionsCess,
            ITransactionsBenefitsRepository benefitsRepository, IBankFileRepository bankFileRepository, IPaymentRepository paymentRepository)
        {
            _transactionSummaryRepository = transactionSummaryRepository;
            _banktransactionsRepository = banktransactionsRepository;
            _reconcileRepository = reconcileRepository;
            _mapper = mapper;
            _mediator = mediator;
            _transactionBenefitsRepository = benefitsRepository;
            _transactionCessRepository = transactionsCess;
            _bankFileRepository = bankFileRepository;
            _vendorPaymentRepository = paymentRepository;
        }
        public async Task<int> Handle(UpdateReconciledTransactionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<TransactionsSummary> trnasactionSummaryList = new();
                List<TransactionsCess> transactionsCessList = new();
                List<TransactionsBenefits> transactionsBenefits = new();
                List<Reconciliation> reconciliations = new();
                List<int> otherBankTransactionsId = new();
                List<Reconciliation> otherbankTransactionMatchedinRecon = new();

                #region Reconciled Transactions Update
                var reconcileStatus = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "ReconcileStatus" });
                var selectedTransactions = _mapper.Map<List<Reconciliation>>(request.selectedTransactions);


                foreach (var transaction in selectedTransactions)
                {
                    transaction.ReconcileStatus = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault();
                    transaction.ReconciledDate = DateTime.Now;
                    transaction.UpdatedDate = DateTime.Now;
                    transaction.UpdatedBy = request.currentUser;
                    switch (transaction.SystemName)
                    {
                        case "Accounts":
                            var transactionSummary = transaction.TransactionsSummary;
                            transactionSummary.Status = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault();
                            trnasactionSummaryList.Add(transactionSummary);
                            break;
                        case "CESS":
                            var transactionCess = _mapper.Map<TransactionsCess>(transaction.TransactionsSummary);
                            transactionCess.Status = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault();
                            transactionsCessList.Add(transactionCess);

                            break;
                        case "Benefits":
                            var transactionBenefit = _mapper.Map<TransactionsBenefits>(transaction.TransactionsSummary);
                            transactionBenefit.Status = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault();
                            transactionsBenefits.Add(transactionBenefit);

                            break;

                    }
                }
                reconciliations.AddRange(selectedTransactions);
                // await _reconcileRepository.UpdateReconciledTransactions(selectedTransactions);
                #endregion

                #region Updating status for Not Selected Transactions
                if (request.unSelectedTransactions.Any())
                {
                    //Update Un Matched Transactions in Reconciliations
                    var unSelectedTransactions = _mapper.Map<List<Reconciliation>>(request.unSelectedTransactions);
                    foreach (var transaction in unSelectedTransactions)
                    {
                        transaction.ReconcileStatus = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                        transaction.IsActive = false;
                        transaction.UpdatedDate = DateTime.Now;
                        transaction.UpdatedBy = request.currentUser;
                        transaction.IsActive = false;

                    }
                    reconciliations.AddRange(unSelectedTransactions);

                    await _reconcileRepository.UpdateReconciledTransactions(reconciliations);

                }


                //Update Other system transactions

                #endregion 

                #region Updating the other transaction which are matche with other bank transactions for Reconciled Transactions
                var otherbankTransactionMatched = await _reconcileRepository.GetReconciliationsByTransactionIds(selectedTransactions, request.bankTransactionId);
                if (otherbankTransactionMatched.Any())
                {
                    otherBankTransactionsId = otherbankTransactionMatched.Select(x => x.BankTransactionsId).Distinct().ToList();
                    foreach (var transaction in otherbankTransactionMatched)
                    {
                        transaction.ReconcileStatus = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                        transaction.UpdatedDate = DateTime.Now;
                        transaction.UpdatedBy = request.currentUser;
                        transaction.IsActive = false;

                    }
                    await _reconcileRepository.UpdateReconciledTransactions(otherbankTransactionMatched);

                }
                #endregion

                #region Updating the other transaction which are matche with other bank transactions for UnSelected  Transactions

                if (request.unSelectedTransactions.Any())
                {
                    var unSelectedTransactions = _mapper.Map<List<Reconciliation>>(request.unSelectedTransactions);

                    var unselectedOtherTransactions = await _reconcileRepository.GetReconciliationsByTransactionIds(unSelectedTransactions, request.bankTransactionId);
                    var unselectedIds = unselectedOtherTransactions.Select(x => x.TransactionsId).ToList();
                    var notMatchedTransactionsinRecon = unSelectedTransactions.Where(x => !unselectedIds.Contains(x.TransactionsId)).ToList();
                    if (unselectedOtherTransactions.Count() == 0)
                    {
                        foreach (var transaction in unSelectedTransactions)
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

                }
                #endregion

                if (otherBankTransactionsId.Any())
                {
                    var ohterBankTransactions = await _reconcileRepository.GetReconciliationsByBankTransactionIds(otherBankTransactionsId);
                    if (ohterBankTransactions.Any())
                    {
                        foreach (var item in ohterBankTransactions)
                        {
                            var otherbankTransactions = await _banktransactionsRepository.GetBankTransactionById(item);
                            otherbankTransactions.Status = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                            await _banktransactionsRepository.UpdateBankTransactionsStatus(otherbankTransactions);
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
                bankTransactions.Status = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault();
                await _banktransactionsRepository.UpdateBankTransactionsStatus(bankTransactions);

                return request.bankTransactionId;


            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
