using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.GSTTDS;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Transactions;
using Application.UserStories.Master.Request.Queries;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Payment;
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
    public class AddGSTDSTransactionSummaryCommandHandler : IRequestHandler<AddGSTTDSTransactionSummaryCommand, int>
    {
        private readonly ITransactionSummaryRepository _transactionSummaryRepository;
        private readonly IMapper _mapper;
        private readonly IBankMasterRepository _bankFileRepository;
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IGstdsPaymentChallanRepository _gsttdschallanRepository;
        private readonly IBillGsttdsPaymentRepository _billGSTTDSPaymentRepository;
        private readonly IMediator _mediator;

        public AddGSTDSTransactionSummaryCommandHandler(ITransactionSummaryRepository transactionSummaryRepository,
            IMapper mapper, IBankMasterRepository bankFileRepository, IPaymentRepository paymentRepository,
            IGstdsPaymentChallanRepository gsttdschallanRepository, IBillGsttdsPaymentRepository billGSTTDSPaymentRepository, IMediator mediator)
        {
            _transactionSummaryRepository = transactionSummaryRepository;
            _mapper = mapper;
            _bankFileRepository = bankFileRepository;
            _vendorPaymentRepository = paymentRepository;
            _gsttdschallanRepository = gsttdschallanRepository;
            _billGSTTDSPaymentRepository = billGSTTDSPaymentRepository;
            _mediator = mediator;
        }

        public async Task<int> Handle(AddGSTTDSTransactionSummaryCommand request, CancellationToken cancellationToken)
        {
            try
            {
               
                var transactionSubType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "TransactionDetailedType" });
                var assessmentYearList = await _mediator.Send(new GetCommonMasterListRequest { CodeType = ValueMapping.assementYear });

                if (request.gstTdsPaymentChallanList.Id != 0)
                {
                    var gsttdsPaymentchllan = await _gsttdschallanRepository.GetGSTTDSPaymentChallanAsync(request.gstTdsPaymentChallanList.Id);
                    if (request.gstTdsPaymentChallanList.Bank.IsBulkGSTTDS)
                    {
                        var transactionSummary = new TransactionsSummary
                        {
                            TransactionDate = (DateTime)request.gstTdsPaymentChallanList.PaidDate,
                            TransactionGeneratedDate = (DateTime)request.gstTdsPaymentChallanList.PaidDate,
                            UTRNumber = request.gstTdsPaymentChallanList.UTRNo,
                            BankName = request.gstTdsPaymentChallanList.Bank.BankName,
                            BranchName = request.gstTdsPaymentChallanList.Bank.BranchName,
                            IFSCCode = request.gstTdsPaymentChallanList.Bank.IfscCode,
                            AccountNumber = request.gstTdsPaymentChallanList.Bank.Accountnumber,
                            TransactionRefNo = request.gstTdsPaymentChallanList.AcknowledgementRefNo,
                            Amount = request.gstTdsPaymentChallanList.PaidAmount,
                            ChargeOrPayment = "P",
                            ReferenceId = request.gstTdsPaymentChallanList.Id,
                            ReferenceNumber = request.gstTdsPaymentChallanList.Id,
                            ReferenceType = "GSTTDSPaymentChallan",
                            SystemName = "Accounts",
                            TransactionDetailedType = transactionSubType.Where(x => x.CodeValue == "GSTTDS").Select(x => x.CodeValue).First(),
                            AssesmentYear = assessmentYearList.Where(x => x.Id == gsttdsPaymentchllan.AssementYearCMID).Select(x => x.CodeValue).First(),
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

                        var billGSTTDSPaymentChalan = await _billGSTTDSPaymentRepository.GetAllBillGSTTDSPaymentByID(request.gstTdsPaymentChallanList.Id);
                        foreach (var item in billGSTTDSPaymentChalan)
                        {
                            var netGSTTDSTransaction = new TransactionsSummary
                            {
                                ReferenceId = item.BillID,
                                VendorID = item.Bill.VendorId,
                                ChargeOrPayment = "P",
                                VendorName = item.Bill.Vendor.VendorPerson.Contacts.Name,
                                PhoneNumber = item.Bill.Vendor.VendorPerson.Contacts.Phone,
                                ReferenceNumber = (int)item.GSTTDSPaymentID,
                                ReferenceType = "GSTTDSPaymentChallan",
                                SystemName = "Accounts",
                                TransactionDetailedType = transactionSubType.Where(x => x.CodeValue == "GSTTDS").Select(x => x.CodeValue).First(),
                                UTRNumber = request.gstTdsPaymentChallanList.UTRNo,
                                GSTIN_Number = item.Bill.Vendor.GSTIN_Number,
                                PAN_Number = item.Bill.Vendor.PAN_Number,
                                TAN_Number = item.Bill.Vendor.TAN_Number,
                                BankName = request.gstTdsPaymentChallanList.Bank.BankName,
                                BranchName = request.gstTdsPaymentChallanList.Bank.BranchName,
                                IFSCCode = request.gstTdsPaymentChallanList.Bank.IfscCode,
                                AccountNumber = request.gstTdsPaymentChallanList.Bank.Accountnumber,
                                BillReferenceNo = item.Bill.BillReferenceNo,
                                BillNo = item.Bill.BillNo,
                                Amount = request.gstTdsPaymentChallanList.PaidAmount,
                                AssesmentYear = assessmentYearList.Where(x => x.Id == gsttdsPaymentchllan.AssementYearCMID).Select(x => x.CodeValue).First(),
                                TransactionGeneratedDate = DateTime.Now,
                                CreatedBy = request.currentUser,
                                CreatedDate = DateTime.Now,
                                TransactionRefNo = request.gstTdsPaymentChallanList.AcknowledgementRefNo,
                                Status = "Paid",
                                IsPicked = false,
                                IsMatched = false,
                            };
                            listtransactionSummary.Add(netGSTTDSTransaction);
                        }
                    }
                }
                return request.gstTdsPaymentChallanList.Id;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
