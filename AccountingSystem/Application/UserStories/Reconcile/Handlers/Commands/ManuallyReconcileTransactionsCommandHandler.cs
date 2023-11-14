using Application.Interface.Persistence.Bank;
using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.Payment;
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
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Reconcile.Handlers.Commands
{
    public class ManuallyReconcileTransactionsCommandHandler : IRequestHandler<ManuallyReconcileTransactionsCommand, int>
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
        public ManuallyReconcileTransactionsCommandHandler(ITransactionSummaryRepository transactionSummaryRepository, IBankTransactionRepository banktransactionsRepository,
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
        public async Task<int> Handle(ManuallyReconcileTransactionsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var reconcileStatus = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "ReconcileStatus" });
                List<Reconciliation> reconciliationList = new();
                List<TransactionsSummary> trnasactionSummaryList = new();
                List<TransactionsCess> transactionsCessList = new();
                List<TransactionsBenefits> transactionsBenefits = new();

                List<int> otherbankTransactionsId = new();
                foreach (var item in request.TransactionsSummary)
                {
                    var reconciliation = new Reconciliation
                    {
                        BankTransactionsId = request.BankTransaction.Id,
                        TransactionsId = item.ID,
                        Amount = item.Amount,
                        TransactionDetailedType = item.TransactionDetailedType,
                        SystemName = item.SystemName,
                        TransactionDate = item.TransactionDate,
                        MatchedBy = request.currentUser,
                        MatchedDate = DateTime.UtcNow,
                        ReconciledDate = DateTime.UtcNow,
                        ReconcileStatus = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault(),
                        ChargeOrPayment = request.BankTransaction.Credit != null ? "C" : "P",
                        CreatedBy = request.currentUser,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedBy = request.currentUser,
                        UpdatedDate = DateTime.UtcNow,
                        IsActive = true,
                    };
                    switch (item.SystemName)
                    {
                        case "Accounts":
                            var transactionSummary = item;
                            transactionSummary.Status = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault();
                            trnasactionSummaryList.Add(transactionSummary);
                            break;
                        case "CESS":
                            var transactionCess = _mapper.Map<TransactionsCess>(item);
                            transactionCess.Status = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault();
                            transactionsCessList.Add(transactionCess);

                            break;
                        case "Benefits":
                            var transactionBenefit = _mapper.Map<TransactionsBenefits>(item);
                            transactionBenefit.Status = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault();
                            transactionsBenefits.Add(transactionBenefit);
                            break;

                    }
                    reconciliationList.Add(reconciliation);
                }
                var reconciledTransactions = await _reconcileRepository.AddReconciliationList(reconciliationList);

                var matchedTransactions = request.TransactionsSummary.Where(x => x.Status == "Matched").ToList();
                if(matchedTransactions.Any())
                {
                    List<Reconciliation> matchedStatusReconc = new();
                    foreach (var item in matchedTransactions)
                    {
                        var reconciliation = new Reconciliation
                        {
                            TransactionsId = item.ID,
                            SystemName = item.SystemName,
                        };
                        matchedStatusReconc.Add(reconciliation);
                    }
                    var transactionMatchedWithOtherbank = await _reconcileRepository.GetReconciliationsByTransactionIds(matchedStatusReconc, request.BankTransaction.Id);
                    if (transactionMatchedWithOtherbank.Any())
                    {
                        otherbankTransactionsId = transactionMatchedWithOtherbank.Select(x => x.BankTransactionsId).Distinct().ToList();
                        foreach (var transaction in transactionMatchedWithOtherbank)
                        {
                            transaction.ReconcileStatus = reconcileStatus.Where(x => x.CodeName == "UnMatched").Select(x => x.CodeValue).FirstOrDefault();
                            transaction.UpdatedDate = DateTime.Now;
                            transaction.UpdatedBy = request.currentUser;
                            transaction.IsActive = false;

                        }
                        await _reconcileRepository.UpdateReconciledTransactions(transactionMatchedWithOtherbank);
                        if (otherbankTransactionsId.Any())
                        {
                            var ohterBankTransactions = await _reconcileRepository.GetReconciliationsByBankTransactionIds(otherbankTransactionsId);
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
                var bankTransactions = await _banktransactionsRepository.GetBankTransactionById(request.BankTransaction.Id);
                bankTransactions.Status = reconcileStatus.Where(x => x.CodeName == "Reconciled").Select(x => x.CodeValue).FirstOrDefault();
                await _banktransactionsRepository.UpdateBankTransactionsStatus(bankTransactions);
                return request.BankTransaction.Id;
            }
            catch
            {
                throw;
            }
        }
    }
}
