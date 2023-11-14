using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Transactions;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Transactions.Requests.Commands;
using Common.ConstantVariables;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Handlers.Commands
{
    public class AddPaymentTransactionSummaryCommandHandler : IRequestHandler<AddPaymentTransactionSummaryCommand, int>
    {
        private readonly ITransactionSummaryRepository _transactionSummaryRepository;
        private readonly IBankFileRepository _bankFileRepository;
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMediator _mediator;

        public AddPaymentTransactionSummaryCommandHandler(ITransactionSummaryRepository transactionSummaryRepository, IBankFileRepository bankFileRepository, IPaymentRepository paymentRepository, IMediator mediator)
        {
            _transactionSummaryRepository = transactionSummaryRepository;
            _bankFileRepository = bankFileRepository;
            _vendorPaymentRepository = paymentRepository;
            _mediator = mediator;
        }

        public async Task<int> Handle(AddPaymentTransactionSummaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var transactionSubType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "TransactionDetailedType" });

                int currentYear = DateTime.Now.Year;
                if (request.bankFileUTRDetails != null)
                {
                    var generatedBankFileDetails = await _bankFileRepository.GetGeneratedBankFielById(request.bankFileUTRDetails.GenerateBankFileID.First());
                    if (request.bankFileUTRDetails.IsBulkPayment)
                    {
                        var transactionSummary = new TransactionsSummary
                        {
                            TransactionDate = generatedBankFileDetails.CreatedOn,
                            TransactionGeneratedDate = DateTime.Now,
                            UTRNumber = request.bankFileUTRDetails.SameBankUTRNumber ?? request.bankFileUTRDetails.DifferentBankUTRNumber,
                            BankName = generatedBankFileDetails.Bank.BankName,
                            BranchName = generatedBankFileDetails.Bank.BranchName,
                            IFSCCode = generatedBankFileDetails.Bank.IfscCode,
                            AccountNumber = generatedBankFileDetails.Bank.Accountnumber,
                            TransactionRefNo = generatedBankFileDetails.BankFileReferenceNo,
                            Amount = request.bankFileUTRDetails.TotalAmount,
                            ChargeOrPayment = "P",
                            ReferenceId = generatedBankFileDetails.Id,
                            ReferenceNumber = generatedBankFileDetails.Id,
                            ReferenceType = "GenerateBankFile",
                            SystemName = "Accounts",
                            TransactionDetailedType = transactionSubType.Where(x => x.CodeValue == "Payment").Select(x => x.CodeValue).FirstOrDefault(),
                            AssesmentYear = (DateTime.Now.Month >= 4) ?
                                           $"{currentYear}-{currentYear + 1}" :
                                           $"{currentYear - 1}-{currentYear}",
                            CreatedBy = request.currentUser,
                            CreatedDate = DateTime.Now,
                            Status = "Paid",
                            IsPicked = false,
                            IsMatched = false,
                        };
                        await _transactionSummaryRepository.AddTransactionSummary(transactionSummary);

                    }
                    else
                    {
                        List<TransactionsSummary> listtransactionSummary = new();

                        var paymentid = _bankFileRepository.GetPaymnetIDByGenBankFileID(request.bankFileUTRDetails.GenerateBankFileID);
                        var payments = await _vendorPaymentRepository.GetPaymentsById(paymentid);
                        var actualPaymentIds = payments.Where(x => x.Type == "Actual").ToList();
                        var advancePayments = payments.Where(x => x.Type == "Advance").ToList();

                        if (actualPaymentIds.Any())
                        {
                            //var billPayments = await _vendorPaymentRepository.GetBillPaymentById(actualPaymentIds);
                            foreach (var item in actualPaymentIds)
                            {
                                var newTransactionDetails = new TransactionsSummary
                                {
                                    ReferenceId = item.ID,
                                    VendorID = item.VendorID,
                                    ChargeOrPayment = "P",
                                    VendorName = item.Vendor.VendorPerson.Contacts.Name,
                                    PhoneNumber = item.Vendor.VendorPerson.Contacts.Phone,
                                    ReferenceNumber = item.ID,
                                    ReferenceType = "Payment",
                                    SystemName = "Accounts",
                                    TransactionDetailedType = transactionSubType.Where(x => x.CodeValue == "Payment").Select(x => x.CodeValue).FirstOrDefault(),
                                    UTRNumber = request.bankFileUTRDetails.SameBankUTRNumber ?? request.bankFileUTRDetails.DifferentBankUTRNumber,
                                    GSTIN_Number = item.Vendor.GSTIN_Number,
                                    PAN_Number = item.Vendor.PAN_Number,
                                    TAN_Number = item.Vendor.TAN_Number,
                                    BankName = generatedBankFileDetails.Bank.BankName,
                                    BranchName = generatedBankFileDetails.Bank.BranchName,
                                    IFSCCode = generatedBankFileDetails.Bank.IfscCode,
                                    AccountNumber = generatedBankFileDetails.Bank.Accountnumber,
                                    BillReferenceNo = item.PaymentBillReference,
                                    Amount = (decimal)item.PaidAmount,
                                    AssesmentYear = (DateTime.Now.Month >= 4) ?
                                                       $"{currentYear}-{currentYear + 1}" :
                                                       $"{currentYear - 1}-{currentYear}",
                                    TransactionGeneratedDate = DateTime.Now,
                                    CreatedBy = request.currentUser,
                                    CreatedDate = DateTime.Now,
                                    TransactionRefNo = item.PaymentReferenceNo,
                                    Status = "Paid",
                                    IsPicked = false,
                                    IsMatched = false,
                                };
                                listtransactionSummary.Add(newTransactionDetails);
                            }
                        }

                        if (advancePayments.Any())
                        {
                            foreach (var item in advancePayments)
                            {
                                var newTransactionDetails = new TransactionsSummary
                                {

                                    ReferenceId = item.ID,
                                    VendorID = item.VendorID,
                                    ChargeOrPayment = "A",
                                    VendorName = item.Vendor.Name,
                                    PhoneNumber = item.Vendor.VendorPerson.Contacts.Phone,
                                    ReferenceNumber = item.ID,
                                    ReferenceType = "Payment",
                                    SystemName = "Accounts",
                                    TransactionDetailedType = transactionSubType.Where(x => x.CodeValue == "Payment").Select(x => x.CodeValue).FirstOrDefault(),
                                    UTRNumber = request.bankFileUTRDetails.SameBankUTRNumber ?? request.bankFileUTRDetails.DifferentBankUTRNumber,
                                    GSTIN_Number = item.Vendor.GSTIN_Number,
                                    PAN_Number = item.Vendor.PAN_Number,
                                    TAN_Number = item.Vendor.TAN_Number,
                                    BankName = generatedBankFileDetails.Bank.BankName,
                                    BranchName = generatedBankFileDetails.Bank.BranchName,
                                    IFSCCode = generatedBankFileDetails.Bank.IfscCode,
                                    AccountNumber = generatedBankFileDetails.Bank.Accountnumber,
                                    Amount = (decimal)item.PaidAmount,
                                    AssesmentYear = (DateTime.Now.Month >= 4) ?
                                                      $"{currentYear}-{currentYear + 1}" :
                                                      $"{currentYear - 1}-{currentYear}",
                                    TransactionGeneratedDate = DateTime.Now,
                                    CreatedBy = request.currentUser,
                                    CreatedDate = DateTime.Now,
                                    TransactionRefNo = item.PaymentReferenceNo,
                                    PaymentReferenceNo = item.PaymentReferenceNo,
                                    Status = "Paid",
                                    IsPicked = false,
                                    IsMatched = false,
                                };
                                listtransactionSummary.Add(newTransactionDetails);
                            }
                        }
                        await _transactionSummaryRepository.AddTransactionSummaryDetails(listtransactionSummary);
                    }
                }
                return request.bankFileUTRDetails.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
