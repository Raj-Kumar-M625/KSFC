using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.GSTTDS;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Transactions;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Domain.Payment;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Handlers.Commands
{
    public class AddPaidTransactionCommandHandler : IRequestHandler<AddPaidTransactionCommand, int>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IBankFileRepository _bankFileRepository;
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IBillTdsPaymentRepository _billTDSPaymentChallanRepository;
        private readonly IBillGsttdsPaymentRepository _billGSTTDSPaymentRepository;
        private readonly IMediator _mediator;

        public AddPaidTransactionCommandHandler(ITransactionRepository transactionRepository, IMapper mapper, IBankFileRepository bankFileRepository, IPaymentRepository paymentRepository, IBillTdsPaymentRepository billTDSPaymentChallanRepository, IBillGsttdsPaymentRepository billGSTTDSPaymentRepository, IMediator mediator)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _bankFileRepository = bankFileRepository;
            _vendorPaymentRepository = paymentRepository;
            _billTDSPaymentChallanRepository = billTDSPaymentChallanRepository;
            _billGSTTDSPaymentRepository = billGSTTDSPaymentRepository;
            _mediator = mediator;
        }

        public async Task<int> Handle(AddPaidTransactionCommand request, CancellationToken cancellationToken)
        {
            if (request.GenerateBankFileID != null)
            {
                int currentYear = DateTime.Now.Year;
                var paymentid = _bankFileRepository.GetPaymnetIDByGenBankFileID(request.GenerateBankFileID);
                var payments = await _vendorPaymentRepository.GetPaymentsById(paymentid);
                var advancePayments = payments.Where(x => x.Type == "Advance").ToList();
                var actualPaymentIds = payments.Where(x => x.Type == "Actual" && x.AdvanceAmountUsed <= 0).Select(x => x.ID).ToList();
                var transactionSubType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "TransactionDetailedType" });
                if (actualPaymentIds.Any())
                {
                    await AddActualPaymentsTransaction(actualPaymentIds);
                }
                if (advancePayments.Any())
                {
                    await AddAdvancePaymentsTransactions(advancePayments);
                }
                var fullAdvancePaymentsUsedPaymentsId = payments.Where(x => x.PaidAmount == 0 && x.AdvanceAmountUsed > 0).Select(x => x.ID).ToList();
                var partiallyAdvancePaymentUsedPayments = payments.Where(x => x.PaidAmount > 0 && x.AdvanceAmountUsed > 0).ToList();

                if (fullAdvancePaymentsUsedPaymentsId.Any())
                {
                    await AddActualPaymentsTransaction(fullAdvancePaymentsUsedPaymentsId);

                    foreach (var item in fullAdvancePaymentsUsedPaymentsId)
                    {
                        var mappingPayments = await _vendorPaymentRepository.GetMappingAdvancePayments(item);
                        await AddMappingPaymentsTransaction(mappingPayments);
                    }
                }

                if (partiallyAdvancePaymentUsedPayments.Any())
                {
                    foreach (var item in partiallyAdvancePaymentUsedPayments)
                    {
                        await AddActualPaymentsTransaction(partiallyAdvancePaymentUsedPayments.Select(x => x.ID).ToList());

                        var mappingPayments = await _vendorPaymentRepository.GetMappingAdvancePayments(item.ID);

                        await AddMappingPaymentsTransaction(mappingPayments);
                        //if(item.PaidAmount > 0)
                        //{
                        //    var netGSTTDSTransaction = new Transaction
                        //    {
                        //        ReferenceId = item.ID,
                        //        VendorID = item.VendorID,
                        //        TransactionType = "P",
                        //        VendorName = item.Vendor.Name,
                        //        PhoneNumber = item.Vendor.VendorPerson.Contacts.Phone,
                        //        ReferenceNumber = item.ID,
                        //        ReferenceType = "Payment",
                        //        TransactionDetailedType = transactionSubType.Where(x => x.CodeName == "BillNetPayable").Select(x => x.CodeValue).First(),
                        //        UTRNumber = request.UTR,
                        //        GSTIN_Number = item.Vendor.GSTIN_Number,
                        //        PAN_Number = item.Vendor.PAN_Number,
                        //        TAN_Number = item.Vendor.TAN_Number,
                        //        AccountNumber = item.Vendor.VendorBankAccounts.AccountNumber,
                        //        BankName = item.Vendor.VendorBankAccounts.BranchMaster.BankDetails.BankName,
                        //        BranchName = item.Vendor.VendorBankAccounts.BranchMaster.branch_name,
                        //        IFSCCode = item.Vendor.VendorBankAccounts.BranchMaster.branch_ifsc,
                        //        Amount = (decimal)item.PaidAmount * -1,
                        //        FinancialYear = (DateTime.Now.Month >= 4) ?
                        //                   $"{currentYear}-{currentYear + 1}" :
                        //                   $"{currentYear - 1}-{currentYear}",
                        //        TransactionGeneratedDate = DateTime.Now,
                        //        TransactionDate = item.CreatedOn,
                        //        CreatedBy = request.CurrentUser,
                        //        CreatedDate = DateTime.Now,
                        //        TransactionRefNo = item.PaymentReferenceNo,
                        //        PaymentReferenceNo = item.PaymentReferenceNo,
                        //        Status = "Paid",
                        //        ReconcileStatus = "Pending",
                        //    };
                        //    await _transactionRepository.AddTransaction(netGSTTDSTransaction);
                        //}
                    }
                }
            }
            else if (request.tdsPaymentChallan != null)
            {
                var tdsId = request.tdsPaymentChallan.First().Id;
                var tdsPaymentChallan = request.tdsPaymentChallan.First();
                var billTDSPaymentChalan = await _billTDSPaymentChallanRepository.GetBillTDSPaymentChallanById(tdsId);
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });
                var transactionSubType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "TransactionDetailedType" });

                foreach (var item in billTDSPaymentChalan)
                {
                    var AssessmentYear = assessmentYearList.Where(x => x.Id == tdsPaymentChallan.AssementYear).Select(x => x.CodeValue).First();
                    int startYear = int.Parse(AssessmentYear.Substring(0, 4));
                    int endYear = int.Parse(AssessmentYear.Substring(5));
                    startYear--;
                    endYear--;
                    string FinancialYear = $"{startYear}-{endYear}";
                    int currentYear = DateTime.Now.Year;
                    var netGSTTDSTransaction = new Transaction
                    {
                        ReferenceId = item.BillID,
                        VendorID = item.Bill.VendorId,
                        TransactionType = "P",
                        VendorName = item.Bill.Vendor.Name,
                        PhoneNumber = item.Bill.Vendor.VendorPerson.Contacts.Phone,
                        ReferenceNumber = item.TDSPaymentChallanID,
                        ReferenceType = "TDSPaymentChallan",
                        TransactionDetailedType = transactionSubType.Where(x => x.CodeName == "TDS").Select(x => x.CodeValue).First(),
                        UTRNumber = request.UTR,
                        ChallanNo = tdsPaymentChallan.ChallanNo,
                        GSTIN_Number = item.Bill.Vendor.GSTIN_Number,
                        PAN_Number = item.Bill.Vendor.PAN_Number,
                        TAN_Number = item.Bill.Vendor.TAN_Number,
                        AccountNumber = item.Bill.Vendor.VendorBankAccounts.AccountNumber,
                        BankName = item.Bill.Vendor.VendorBankAccounts.BranchMaster.BankDetails.BankName,
                        BranchName = item.Bill.Vendor.VendorBankAccounts.BranchMaster.branch_name,
                        IFSCCode = item.Bill.Vendor.VendorBankAccounts.BranchMaster.branch_ifsc,
                        BillReferenceNo = item.Bill.BillReferenceNo,
                        BillNo = item.Bill.BillNo,
                        Amount = (decimal)tdsPaymentChallan.TDSAmount * -1,
                        FinancialYear = FinancialYear,
                        AssesmentYear = assessmentYearList.Where(x => x.Id == tdsPaymentChallan.AssementYear).Select(x => x.CodeValue).First(),
                        TransactionGeneratedDate = request.tdsPaymentChallan.FirstOrDefault().PaymentDate,
                        TransactionDate = (DateTime)request.tdsPaymentChallan.FirstOrDefault().TDSChallanDate,
                        CreatedBy = request.CurrentUser,
                        CreatedDate = DateTime.Now,
                        TransactionRefNo = tdsPaymentChallan.ChallanNo,
                        Status = "Paid",
                        ReconcileStatus = "Pending",

                    };
                    await _transactionRepository.AddTransaction(netGSTTDSTransaction);
                }
            }
            else if (request.gstTdsPaymentChallanList != null)
            {
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });
                var billGSTTDSPaymentChalan = await _billGSTTDSPaymentRepository.GetAllBillGSTTDSPaymentByID(request.gstTdsPaymentChallanList.Id);
                var transactionSubType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "TransactionDetailedType" });
                foreach (var item in billGSTTDSPaymentChalan)
                {
                    var AssesmentYear = assessmentYearList.Where(x => x.Id == item.GSTTDSPaymentChallan.AssementYearCMID).Select(x => x.CodeValue).First();
                    int startYear = int.Parse(AssesmentYear.Substring(0, 4));
                    int endYear = int.Parse(AssesmentYear.Substring(5));
                    startYear--;
                    endYear--;
                    string FinancialYear = $"{startYear}-{endYear}";
                    int currentYear = DateTime.Now.Year;
                    var netGSTTDSTransaction = new Transaction
                    {
                        ReferenceId = item.BillID,
                        VendorID = item.Bill.VendorId,
                        TransactionType = "P",
                        VendorName = item.Bill.Vendor.Name,
                        PhoneNumber = item.Bill.Vendor.VendorPerson.Contacts.Phone,
                        ReferenceNumber = item.GSTTDSPaymentID,
                        ReferenceType = "GSTTDSPaymentChallan",
                        TransactionDetailedType = transactionSubType.Where(x => x.CodeName == "GSTTDS").Select(x => x.CodeValue).First(),
                        UTRNumber = request.UTR,
                        GSTIN_Number = item.Bill.Vendor.GSTIN_Number,
                        PAN_Number = item.Bill.Vendor.PAN_Number,
                        TAN_Number = item.Bill.Vendor.TAN_Number,
                        AccountNumber = item.Bill.Vendor.VendorBankAccounts.AccountNumber,
                        BankName = item.Bill.Vendor.VendorBankAccounts.BranchMaster.BankDetails.BankName,
                        BranchName = item.Bill.Vendor.VendorBankAccounts.BranchMaster.branch_name,
                        IFSCCode = item.Bill.Vendor.VendorBankAccounts.BranchMaster.branch_ifsc,
                        BillReferenceNo = item.Bill.BillReferenceNo,
                        BillNo = item.Bill.BillNo,
                        Amount = request.gstTdsPaymentChallanList.PaidAmount * -1,
                        FinancialYear = FinancialYear,
                        AssesmentYear = assessmentYearList.Where(x => x.Id == item.GSTTDSPaymentChallan.AssementYearCMID).Select(x => x.CodeValue).First(),
                        TransactionGeneratedDate = request.gstTdsPaymentChallanList.PaidDate,
                        TransactionDate = (DateTime)request.gstTdsPaymentChallanList.PaidDate,
                        CreatedBy = request.CurrentUser,
                        CreatedDate = DateTime.Now,
                        TransactionRefNo = request.gstTdsPaymentChallanList.AcknowledgementRefNo,
                        Status = "Paid",
                        ReconcileStatus = "Pending",

                    };
                    await _transactionRepository.AddTransaction(netGSTTDSTransaction);
                }
            }
            else if (request.MappingAdvancePayment != null)
            {
                await AddMappingPaymentsTransaction(request.MappingAdvancePayment);
            }
            async Task AddAdvancePaymentsTransactions(List<Payments> advancePayments)
            {
                int currentYear = DateTime.Now.Year;
                foreach (var item in advancePayments)
                {
                    var newTransactionDetails = new Transaction
                    {

                        ReferenceId = item.ID,
                        VendorID = item.VendorID,
                        TransactionType = "P",
                        VendorName = item.Vendor.Name,
                        PhoneNumber = item.Vendor.VendorPerson.Contacts.Phone,
                        ReferenceNumber = item.ID,
                        ReferenceType = "Payment",
                        TransactionDetailedType = "AdvancePayment",
                        UTRNumber = request.UTR,
                        GSTIN_Number = item.Vendor.GSTIN_Number,
                        PAN_Number = item.Vendor.PAN_Number,
                        TAN_Number = item.Vendor.TAN_Number,
                        AccountNumber = item.Vendor.VendorBankAccounts.AccountNumber,
                        BankName = item.Vendor.VendorBankAccounts.BranchMaster.BankDetails.BankName,
                        BranchName = item.Vendor.VendorBankAccounts.BranchMaster.branch_name,
                        IFSCCode = item.Vendor.VendorBankAccounts.BranchMaster.branch_ifsc,
                        Amount = (decimal)item.PaidAmount * -1,
                        FinancialYear = (DateTime.Now.Month >= 4) ?
                                           $"{currentYear}-{currentYear + 1}" :
                                           $"{currentYear - 1}-{currentYear}",
                        TransactionGeneratedDate = DateTime.Now,
                        CreatedBy = request.CurrentUser,
                        CreatedDate = DateTime.Now,
                        TransactionRefNo = item.PaymentReferenceNo,
                        PaymentReferenceNo = item.PaymentReferenceNo,
                        Status = "Paid",
                        ReconcileStatus = "Pending",

                    };
                    await _transactionRepository.AddTransaction(newTransactionDetails);
                }
            }

            async Task AddMappingPaymentsTransaction(List<MappingAdvancePayment> mappingAdvancePayments)
            {
                int currentYear = DateTime.Now.Year;
                var transactionSubType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "TransactionDetailedType" });

                foreach (var item in mappingAdvancePayments)
                {
                    var netGSTTDSTransaction = new Transaction()
                    {
                        ReferenceId = item.Id,
                        VendorID = item.VendorId,
                        TransactionType = "C",
                        VendorName = item.Vendor.Name,
                        PhoneNumber = item.Vendor.VendorPerson.Contacts.Phone,
                        ReferenceNumber = item.Id,
                        ReferenceType = "MappingAdvancePayment",
                        TransactionDetailedType = "AdvancePayment",
                        UTRNumber = request.UTR,
                        GSTIN_Number = item.Vendor.GSTIN_Number,
                        PAN_Number = item.Vendor.PAN_Number,
                        TAN_Number = item.Vendor.TAN_Number,
                        AccountNumber = item.Vendor.VendorBankAccounts.AccountNumber,
                        BankName = item.Vendor.VendorBankAccounts.BranchMaster.BankDetails.BankName,
                        BranchName = item.Vendor.VendorBankAccounts.BranchMaster.branch_name,
                        IFSCCode = item.Vendor.VendorBankAccounts.BranchMaster.branch_ifsc,
                        Amount = (decimal)item.AdvanceAmount,
                        FinancialYear = (DateTime.Now.Month >= 4) ?
                                               $"{currentYear}-{currentYear + 1}" :
                                               $"{currentYear - 1}-{currentYear}",
                        TransactionGeneratedDate = DateTime.Now,
                        CreatedBy = request.CurrentUser,
                        CreatedDate = DateTime.Now,
                        TransactionRefNo = item.Payments.PaymentReferenceNo,
                        PaymentReferenceNo = item.Payments.PaymentReferenceNo,
                        Status = "Paid",
                        ReconcileStatus = "Pending",
                    };
                    await _transactionRepository.AddTransaction(netGSTTDSTransaction);

                }
            }
            async Task AddActualPaymentsTransaction(List<int> paymentid)
            {
                var transactionSubType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "TransactionDetailedType" });
                int currentYear = DateTime.Now.Year;

                var billPayments = await _vendorPaymentRepository.GetBillPaymentById(paymentid);
                var billIds = billPayments.Where(x => x.Bill.BalanceAmount == 0).Select(x => x.Bill.ID).ToList();
                if (billIds.Any())
                {
                    var transactions = await _transactionRepository.GetTransactionsBasedBillId(billIds);
                    if (transactions.Any())
                    {
                        await AddReverseTransactionForOtherPayments(transactions);
                    }
                }

                foreach (var item in billPayments)
                {
                    var netGSTTDSTransaction = new Transaction
                    {
                        ReferenceId = item.BillID,
                        VendorID = item.VendorId,
                        TransactionType = "P",
                        VendorName = item.Vendor.Name,
                        PhoneNumber = item.Vendor.VendorPerson.Contacts.Phone,
                        ReferenceNumber = item.BillID,
                        ReferenceType = "Bill",
                        TransactionDetailedType = transactionSubType.Where(x => x.CodeName == "BillNetPayable").Select(x => x.CodeValue).First(),
                        UTRNumber = request.UTR,
                        GSTIN_Number = item.Vendor.GSTIN_Number,
                        PAN_Number = item.Vendor.PAN_Number,
                        TAN_Number = item.Vendor.TAN_Number,
                        AccountNumber = item.Vendor.VendorBankAccounts.AccountNumber,
                        BankName = item.Bill.Vendor.VendorBankAccounts.BranchMaster.BankDetails.BankName,
                        BranchName = item.Bill.Vendor.VendorBankAccounts.BranchMaster.branch_name,
                        IFSCCode = item.Bill.Vendor.VendorBankAccounts.BranchMaster.branch_ifsc,
                        BillReferenceNo = item.Bill.BillReferenceNo,
                        BillNo = item.Bill.BillNo,
                        Amount = (decimal)item.PaymentAmount * -1,
                        FinancialYear = (DateTime.Now.Month >= 4) ?
                                           $"{currentYear}-{currentYear + 1}" :
                                           $"{currentYear - 1}-{currentYear}",
                        TransactionGeneratedDate = DateTime.Now,
                        TransactionDate = item.CreatedOn,
                        CreatedBy = request.CurrentUser,
                        CreatedDate = DateTime.Now,
                        TransactionRefNo = item.Payments.PaymentReferenceNo,
                        PaymentReferenceNo = item.Payments.PaymentReferenceNo,
                        Status = "Paid",
                        ReconcileStatus = "Pending",
                    };
                    await _transactionRepository.AddTransaction(netGSTTDSTransaction);
                }
            }

            async Task AddReverseTransactionForOtherPayments(List<Transaction> transactions)
            {
                foreach (var item in transactions)
                {
                    var netGSTTDSTransaction = new Transaction
                    {
                        ReferenceId = item.ReferenceId,
                        VendorID = item.VendorID,
                        TransactionType = "P",
                        VendorName = item.VendorName,
                        PhoneNumber = item.PhoneNumber,
                        ReferenceNumber = item.ReferenceNumber,
                        ReferenceType = item.ReferenceType,
                        TransactionDetailedType = item.TransactionDetailedType,
                        UTRNumber = item.UTRNumber,
                        GSTIN_Number = item.GSTIN_Number,
                        PAN_Number = item.PAN_Number,
                        TAN_Number = item.TAN_Number,
                        AccountNumber = item.AccountNumber,
                        BankName = item.BankName,
                        BranchName = item.BranchName,
                        IFSCCode = item.IFSCCode,
                        BillReferenceNo = item.BillReferenceNo,
                        BillNo = item.BillNo,
                        Amount = Math.Abs((decimal)item.Amount),
                        FinancialYear = item.FinancialYear,
                        TransactionGeneratedDate = DateTime.Now,
                        TransactionDate = item.TransactionDate,
                        CreatedBy = request.CurrentUser,
                        CreatedDate = DateTime.Now,
                        TransactionRefNo = item.PaymentReferenceNo,
                        PaymentReferenceNo = item.PaymentReferenceNo,
                        Status = "Paid",
                        ReconcileStatus = "Pending",
                    };
                    await _transactionRepository.AddTransaction(netGSTTDSTransaction);
                }
            }

            return request.Id;
        }
    }
}
