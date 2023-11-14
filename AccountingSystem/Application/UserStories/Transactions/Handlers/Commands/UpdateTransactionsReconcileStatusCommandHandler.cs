using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Transactions;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using Domain.Payment;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Handlers.Commands
{
    public class UpdateTransactionsReconcileStatusCommandHandler : IRequestHandler<UpdateTransactionReconcileStatusCommand, int>
    {
        private readonly ITransactionSummaryRepository _transactionSummaryRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IBankFileRepository _bankFileRepository;
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMediator _mediator;

        public UpdateTransactionsReconcileStatusCommandHandler(ITransactionSummaryRepository transactionSummaryRepository, ITransactionRepository transactionRepository,
            IMapper mapper, IBankFileRepository bankFileRepository, IPaymentRepository paymentRepository,
           IMediator mediator)
        {
            _transactionSummaryRepository = transactionSummaryRepository;
            _mapper = mapper;
            _bankFileRepository = bankFileRepository;
            _vendorPaymentRepository = paymentRepository;
            _mediator = mediator;
            _transactionRepository = transactionRepository;
        }
        public async Task<int> Handle(UpdateTransactionReconcileStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<BillPayment> reConciledbillPaymentsList = new();
                List<BillPayment> UnMatchedBillPayment = new();
                List<Transaction> reConciledtransactionList = new();
                List<Transaction> unMatchedtransactionList = new();
                List<Transaction> transactionList = new();

                foreach (var item in request.TransactionsSummary)
                {
                    if (item.Status == "Reconciled")
                    {
                        switch (item.ReferenceType)
                        {
                            case "Payment":
                                List<int> paymentids = new() { item.ReferenceNumber };
                                var billPaymentsByBankFile = await _vendorPaymentRepository.GetBillPaymentById(paymentids);
                                reConciledbillPaymentsList.AddRange(billPaymentsByBankFile);
                                break;
                            case "GenerateBankFile":
                                List<int> bankFileId = new() { item.ReferenceNumber };
                                var payment = _bankFileRepository.GetPaymnetIDByGenBankFileID(bankFileId);
                                List<int> paymentidList = new() { item.ReferenceNumber };
                                var billPayments = await _vendorPaymentRepository.GetBillPaymentById(paymentidList);
                                reConciledbillPaymentsList.AddRange(billPayments);
                                break;
                            case "TDSPaymentChallan":
                                var tdsTransactions = await _transactionRepository.GetTransactionsBasedRefIdForTDS(item.ReferenceNumber);
                                reConciledtransactionList.AddRange(tdsTransactions);
                                break;
                            case "GSTTDSPaymentChallan":
                                var gsttdsTransactions = await _transactionRepository.GetTransactionsBasedRefIdForGSTTDS(item.ReferenceNumber);
                                reConciledtransactionList.AddRange(gsttdsTransactions);
                                break;
                        }
                    }
                    else if (item.Status == "UnMatched")
                    {
                        switch (item.ReferenceType)
                        {
                            case "Payment":
                                List<int> paymentids = new() { item.ReferenceNumber };
                                var billPaymentsByBankFile = await _vendorPaymentRepository.GetBillPaymentById(paymentids);
                                UnMatchedBillPayment.AddRange(billPaymentsByBankFile);
                                break;
                            case "GenerateBankFile":
                                List<int> bankFileId = new() { item.ReferenceNumber };
                                var payment = _bankFileRepository.GetPaymnetIDByGenBankFileID(bankFileId);
                                List<int> paymentidList = new() { item.ReferenceNumber };
                                var billPayments = await _vendorPaymentRepository.GetBillPaymentById(paymentidList);
                                UnMatchedBillPayment.AddRange(billPayments);
                                break;
                            case "TDSPaymentChallan":
                                var tdsTransactions = await _transactionRepository.GetTransactionsBasedRefIdForTDS(item.ReferenceNumber);
                                unMatchedtransactionList.AddRange(tdsTransactions);
                                break;
                            case "GSTTDSPaymentChallan":
                                var gsttdsTransactions = await _transactionRepository.GetTransactionsBasedRefIdForGSTTDS(item.ReferenceNumber);
                                unMatchedtransactionList.AddRange(gsttdsTransactions);
                                break;
                        }
                    }

                }
                if (reConciledbillPaymentsList.Any())
                {
                    var billId = reConciledbillPaymentsList.Select(x => x.BillID).Distinct().ToList();
                    var paymentTransactions = await _transactionRepository.GetTransactionsBasedRefId(billId);
                    reConciledtransactionList.AddRange(paymentTransactions);
                }

                if (UnMatchedBillPayment.Any())
                {
                    var billId = UnMatchedBillPayment.Select(x => x.BillID).Distinct().ToList();
                    var paymentTransactions = await _transactionRepository.GetTransactionsBasedRefId(billId);
                    unMatchedtransactionList.AddRange(paymentTransactions);
                }

                foreach (var item in reConciledtransactionList)
                {
                    item.ReconcileStatus = "Reconciled";
                    transactionList.Add(item);
                }
                foreach (var item in unMatchedtransactionList)
                {
                    item.ReconcileStatus = "UnMatched";
                    transactionList.Add(item);


                }
                await _transactionRepository.UpdateTransactionReconcileStatus(transactionList);
                return request.TransactionsSummary.FirstOrDefault().ID;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
