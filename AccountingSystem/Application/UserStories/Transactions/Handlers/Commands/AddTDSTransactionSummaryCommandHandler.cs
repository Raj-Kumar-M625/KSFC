using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.GSTTDS;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Transactions;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Transactions.Handlers.Commands
{
    public class AddTDSTransactionSummaryCommandHandler : IRequestHandler<AddTDSTransactionSummaryCommand, int>
    {
        private readonly ITransactionSummaryRepository _transactionSummaryRepository;
        private readonly IMapper _mapper;
        private readonly IBankFileRepository _bankFileRepository;
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly ITdsPaymentChallanRepository _billTDSPaymentChallanRepository;
        private readonly IBillTdsPaymentRepository _billTDSPaymentRepository;
        private readonly IMediator _mediator;

        public AddTDSTransactionSummaryCommandHandler(ITransactionSummaryRepository transactionSummaryRepository,
            IMapper mapper, IBankFileRepository bankFileRepository, IPaymentRepository paymentRepository,
            ITdsPaymentChallanRepository billTDSPaymentChallanRepository, IBillTdsPaymentRepository billTDSPaymentRepository, IMediator mediator)
        {
            _transactionSummaryRepository = transactionSummaryRepository;
            _mapper = mapper;
            _bankFileRepository = bankFileRepository;
            _vendorPaymentRepository = paymentRepository;
            _billTDSPaymentChallanRepository = billTDSPaymentChallanRepository;
            _billTDSPaymentRepository = billTDSPaymentRepository;
            _mediator = mediator;
        }

        public async Task<int> Handle(AddTDSTransactionSummaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var transactionSubType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "TransactionDetailedType" });

                //int currentYear = DateTime.Now.Year;
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });

                if (request.tdsPaymentChallan.Id != 0)
                {
                    var tdsPaymentchallan = await _billTDSPaymentChallanRepository.GetTDSPaymentChallanById(request.tdsPaymentChallan.Id);
                    if (request.tdsPaymentChallan.IsBulkTDS)
                    {
                        var transactionSummary = new TransactionsSummary
                        {
                            TransactionDate = (DateTime)request.tdsPaymentChallan.PaymentDate,
                            TransactionGeneratedDate = (DateTime)request.tdsPaymentChallan.PaymentDate,
                            UTRNumber = request.tdsPaymentChallan.UTRNo,
                            BankName = tdsPaymentchallan.Bank.BankName,
                            BranchName = tdsPaymentchallan.Bank.BranchName,
                            IFSCCode = tdsPaymentchallan.Bank.IfscCode,
                            AccountNumber = tdsPaymentchallan.Bank.Accountnumber,
                            TransactionRefNo = request.tdsPaymentChallan.ChallanNo,
                            Amount = request.tdsPaymentChallan.TotalTDSPayment,
                            ChargeOrPayment = "P",
                            ReferenceId = request.tdsPaymentChallan.Id,
                            ReferenceNumber = request.tdsPaymentChallan.Id,
                            ReferenceType = "TDSPaymentChallan",
                            SystemName = "Accounts",
                            TransactionDetailedType = transactionSubType.Where(x => x.CodeValue == "TDS").Select(x => x.CodeValue).FirstOrDefault(),
                            AssesmentYear = assessmentYearList.Where(x => x.Id == request.tdsPaymentChallan.AssementYear).Select(x => x.CodeValue).FirstOrDefault(),
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
                        List<TransactionsSummary> newTDSTransactionSummary = new();
                        var billTDSPaymentChalan = await _billTDSPaymentRepository.GetBillTDSPaymentChallanById(request.tdsPaymentChallan.Id);
                        //BillTDSPaymentChalan -- Will get BILL Id
                        //Get Bill Data
                        //Insert Based on Bill
                        foreach (var item in billTDSPaymentChalan)
                        {
                            //int currentYear = DateTime.Now.Year;
                            var netTDSTransaction = new TransactionsSummary
                            {
                                ReferenceId = item.BillID,
                                VendorID = item.Bill.VendorId,
                                ChargeOrPayment = "P",
                                VendorName = item.Bill.Vendor.VendorPerson.Contacts.Name,
                                PhoneNumber = item.Bill.Vendor.VendorPerson.Contacts.Phone,
                                ReferenceNumber = (int)item.TDSPaymentChallanID,
                                ReferenceType = "TDSPaymentChallan",
                                SystemName = "Accounts",
                                TransactionDetailedType = transactionSubType.Where(x => x.CodeValue == "TDS").Select(x => x.CodeValue).FirstOrDefault(),
                                UTRNumber = request.tdsPaymentChallan.UTRNo,
                                ChallanNo = request.tdsPaymentChallan.ChallanNo,
                                GSTIN_Number = item.Bill.Vendor.GSTIN_Number,
                                PAN_Number = item.Bill.Vendor.PAN_Number,
                                TAN_Number = item.Bill.Vendor.TAN_Number,
                                BankName = tdsPaymentchallan.Bank.BankName,
                                BranchName = tdsPaymentchallan.Bank.BranchName,
                                IFSCCode = tdsPaymentchallan.Bank.IfscCode,
                                AccountNumber = tdsPaymentchallan.Bank.Accountnumber,
                                BillReferenceNo = item.Bill.BillReferenceNo,
                                BillNo = item.Bill.BillNo,
                                Amount = (decimal)request.tdsPaymentChallan.TDSAmount,
                                AssesmentYear = assessmentYearList.Where(x => x.Id == request.tdsPaymentChallan.AssementYear).Select(x => x.CodeValue).First(),
                                TransactionGeneratedDate = DateTime.Now,
                                CreatedBy = request.currentUser,
                                CreatedDate = DateTime.Now,
                                TransactionRefNo = request.tdsPaymentChallan.ChallanNo,
                                Status = "Paid",
                                IsPicked = false,
                                IsMatched = false,
                            };
                            newTDSTransactionSummary.Add(netTDSTransaction);
                        }
                        await _transactionSummaryRepository.AddTransactionSummaryDetails(newTDSTransactionSummary);

                    }
                }

                return request.tdsPaymentChallan.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
