using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Transactions;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Bill.Requests.Commands;
using Application.UserStories.Master.Request.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Bill;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, int>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        //Adding dependency injection for IBillRepository,IMapper and IMediator
        public CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IMapper mapper, IMediator mediator)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _mediator = mediator;
        }
        
        public async Task<int> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            async Task<Transaction> AddTransaction(string referenceType,string transactionType, decimal amount)
            {
                int currentYear = DateTime.Now.Year;
                return await _transactionRepository.AddTransaction(new Transaction
                {
                    ReferenceId = request.bills.ID,
                    VendorID = request.bills.VendorId,
                    TransactionType = "C",
                    VendorName = request.vendor.Name,
                    PhoneNumber = request.vendor.VendorPerson.Contacts.Phone,
                    ReferenceNumber = request.bills.ID,
                    ReferenceType = referenceType,
                    TransactionDetailedType = transactionType,
                    GSTIN_Number = request.vendor.GSTIN_Number??null,
                    PAN_Number = request.vendor.PAN_Number,
                    TAN_Number = request.vendor.TAN_Number,
                    AccountNumber = request.vendor.VendorBankAccounts.AccountNumber,
                    BankName = request.vendor.VendorBankAccounts.BranchMaster.BankDetails.BankName,
                    BranchName = request.vendor.VendorBankAccounts.BranchMaster.branch_name,
                    IFSCCode = request.vendor.VendorBankAccounts.BranchMaster.branch_ifsc,
                    BillReferenceNo = request.bills.BillReferenceNo,
                    BillNo = request.bills.BillNo,
                    Amount = amount,
                    FinancialYear = (DateTime.Now.Month >= 4) ?
                                           $"{currentYear}-{currentYear + 1}" :
                                           $"{currentYear - 1}-{currentYear}",
                    TransactionDate = DateTime.Now,
                    CreatedBy = request.CurrentUser,
                    CreatedDate = DateTime.Now,
                    TransactionRefNo = request.bills.BillReferenceNo,
                    Status = "Approved"
                });
            }
            var transactionType = await _mediator.Send(new GetCommonMasterListRequest { CodeType = "TransactionDetailedType" });
            var transactionTypeBillId = transactionType.Where(x => x.CodeName == "BillNetPayable").Select(x => x.CodeValue).First();
            await AddTransaction("Bill", transactionTypeBillId, request.bills.NetPayable);
            

            if (request.bills.GSTTDS != 0 && request.bills.GSTTDS != null)
            {
                var transactionTypeId = transactionType.Where(x => x.CodeName == "GSTTDS").Select(x => x.CodeValue).First();
                await AddTransaction("Bill", transactionTypeId, request.bills.GSTTDS);
            }

            if (request.bills.TDS != 0 && request.bills.TDS != null)
            {
                var transactionTypeId = transactionType.Where(x => x.CodeName == "TDS").Select(x => x.CodeValue).First();
                await AddTransaction("Bill", transactionTypeId, request.bills.TDS);
            }

            if(request.bills.Royalty != 0 && request.bills.Royalty != null)
            {
                var transactionTypeId = transactionType.Where(x => x.CodeName == "BillRoyalty").Select(x => x.CodeValue).First();
                await AddTransaction("Bill", transactionTypeId, request.bills.Royalty);
            }
            if (request.bills.Penalty != 0 && request.bills.Penalty != null)
            {
                var transactionTypeId = transactionType.Where(x => x.CodeName == "BillPenalty").Select(x => x.CodeValue).First();
                await AddTransaction("Bill", transactionTypeId, (decimal)request.bills.Penalty);
            }
            if (request.bills.CBF != 0 && request.bills.CBF != null)
            {
                var transactionTypeId = transactionType.Where(x => x.CodeName == "BillCBF").Select(x => x.CodeValue).First();
                await AddTransaction("Bill", transactionTypeId, request.bills.CBF);
            }
            if (request.bills.LabourWelfareCess != 0 && request.bills.LabourWelfareCess != null)
            {
                var transactionTypeId = transactionType.Where(x => x.CodeName == "BillLabourWelfareCess").Select(x => x.CodeValue).First();
                await AddTransaction("Bill", transactionTypeId, request.bills.LabourWelfareCess);
            }
            if (request.bills.Other1Value != 0 && request.bills.Other1Value != null)
            {
                var transactionTypeId = transactionType.Where(x => x.CodeName == "BillOthers1").Select(x => x.CodeValue).First();
                if((decimal)request.bills.Other1Value <0)
                {
                    await AddTransaction("Bill", transactionTypeId, Math.Abs((decimal)request.bills.Other1Value));
                }
                else
                {
                    await AddTransaction("Bill", transactionTypeId, (decimal)request.bills.Other1Value * -1);

                }
            }
            if (request.bills.Other2Value != 0 && request.bills.Other2Value != null)
            {
                var transactionTypeId = transactionType.Where(x => x.CodeName == "BillOthers2").Select(x => x.CodeValue).First();
                if ((decimal)request.bills.Other2Value < 0)
                {
                    await AddTransaction("Bill", transactionTypeId, Math.Abs((decimal)request.bills.Other2Value));
                }
                else
                {
                    await AddTransaction("Bill", transactionTypeId, (decimal)request.bills.Other2Value * -1);

                }
            }
            if (request.bills.Other3Value != 0 && request.bills.Other3Value != null)
            {
                var transactionTypeId = transactionType.Where(x => x.CodeName == "BillOthers3").Select(x => x.CodeValue).First();
                if ((decimal)request.bills.Other3Value < 0)
                {
                    await AddTransaction("Bill", transactionTypeId, Math.Abs((decimal)request.bills.Other3Value ));
                }
                else
                {
                    await AddTransaction("Bill", transactionTypeId, (decimal)request.bills.Other3Value * -1);

                }
            }
            return request.bills.ID;
        }

    }
}
