using Application.DTOs.Adjustment;
using Application.DTOs.Bill;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Transactions;
using Application.UserStories.Adjustment.Requests.Commands;
using Application.UserStories.Bill.Requests.Commands;
using Application.UserStories.Master.Request.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Adjustment;
using Domain.Bill;
using Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Adjustment.Handlers.Commands
{
    
    public class UpdateAdjustmentCommandHandler : IRequestHandler<UpdateAdjustmentCommand, AdjustmentDto>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMediator _mediator;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly ICommonMasterRepository _commonMasterRepository;
        public UpdateAdjustmentCommandHandler(IBillRepository vendorRepository, ITransactionRepository transactionRepository, IMapper mapper, IMediator mediator,ICommonMasterRepository commonMasterRepository)
        {
            _billRepository = vendorRepository;
            _mapper = mapper;
            _mediator = mediator;
            _commonMasterRepository = commonMasterRepository;
            _transactionRepository = transactionRepository;
        }


        public async Task<AdjustmentDto> Handle(UpdateAdjustmentCommand request, CancellationToken cancellationToken)
        {
            var commonMasters = await _commonMasterRepository.GetCommoMasterValues();
            if (request.action != "approve" && request.action != "reject")
            {              

                var bills = _mapper.Map<Adjustments>(request.AdjustmentDto);
                var res = await _billRepository.GetAdjustmentDetails(bills.ID);
                if (bills.AdjustmentType == "Bill")
                {
                    bills.Amount = bills.Amount > 0 ? bills.Amount : bills.Amount * -1;
                }
                else if (bills.AdjustmentType == "Payment")
                {
                    bills.Amount = bills.Amount > 0 ? bills.Amount * -1 : bills.Amount;
                }
                else if (bills.AdjustmentType == "Adjustment")
                {
                    bills.Amount = bills.Amount;
                }
                res.UTR_No = bills.UTR_No;
                res.AdjustmentType = bills.AdjustmentType;
                res.BillPaymentRefNo = bills.BillPaymentRefNo;
                res.Date = bills.Date;
                res.ModifiedBy = request.user;
                res.ModifiedOn = DateTime.UtcNow;
                res.Amount = bills.Amount;
                res.Description = bills.Description;
                var data = await _billRepository.UpdateAdjustmentDetails(res);
                return _mapper.Map<AdjustmentDto>(data);
            }          
            else if (request.action == "approve")
            {
                var bills = _mapper.Map<Adjustments>(request.AdjustmentDto);
                var res = await _billRepository.GetAdjustmentDetails(bills.ID);
                if (bills.AdjustmentType == "Bill")
                {
                    res.Amount = bills.Amount > 0 ? bills.Amount : bills.Amount * -1;
                }
                else if (bills.AdjustmentType == "Payment")
                {
                    res.Amount = bills.Amount > 0 ? bills.Amount * -1 : bills.Amount;
                }
                else if (bills.AdjustmentType == "Adjustment")
                {
                    res.Amount = bills.Amount;
                }
                res.AdjustmentStatus.StatusCMID = commonMasters.Where(x => x.CodeValue == "Approved" && x.CodeType == "PaymentStatus").Select(x => x.Id).First();
                res.AdjustmentStatus.ModifedBy = request.user;
                res.AdjustmentStatus.ModifiedOn = DateTime.UtcNow;
                res.ModifiedBy = request.user;
                res.ModifiedOn = DateTime.UtcNow;
               res.Remarks = bills.Remarks;
                res.ApprovedDate = DateTime.UtcNow;
                res.ApprovedBy = request.user;
                var data = await _billRepository.UpdateAdjustmentDetails(res);
                int currentYear = DateTime.Now.Year;
                var transaction = new Transaction();
                transaction.ReferenceId = res.ID; 
                transaction.VendorID = res.VendorID;
                transaction.TransactionType = "A";
                transaction.VendorName = res.Vendor.Name;
                transaction.PhoneNumber = res.Vendor.VendorPerson.Contacts.Phone;
                transaction.ReferenceNumber = res.ID;
                transaction.ReferenceType = "Adjustment";
                transaction.TransactionDetailedType = res.AdjustmentType == "Payment" ? "AdjustmentPayment" : res.AdjustmentType == "Bill" ? "AdjustmentBill" : res.AdjustmentType;
                transaction.GSTIN_Number = res.Vendor.GSTIN_Number;
                transaction.PAN_Number = res.Vendor.PAN_Number;
                transaction.TAN_Number = res.Vendor.TAN_Number;
                transaction.AccountNumber = res.Vendor.VendorBankAccounts.AccountNumber;
                transaction.BankName = res.Vendor.VendorBankAccounts.BranchMaster.BankDetails.BankName;
                transaction.BranchName = res.Vendor.VendorBankAccounts.BranchMaster.branch_name;
                transaction.IFSCCode = res.Vendor.VendorBankAccounts.BranchMaster.branch_ifsc;
                transaction.Amount = res.AdjustmentType == "Payment" ? res.Amount * -1 : res.Amount;
                transaction.TransactionDate = DateTime.UtcNow;
                transaction.TransactionGeneratedDate = DateTime.UtcNow;
                transaction.Status = "Approved";
                transaction.CreatedBy = request.user;
                transaction.CreatedDate = DateTime.UtcNow;
                transaction.TransactionRefNo = res.AdjustmentReferenceNo;
                transaction.FinancialYear = (DateTime.Now.Month >= 4) ? $"{currentYear}-{currentYear + 1}" : $"{currentYear - 1}-{currentYear}";
                await _transactionRepository.AddTransaction(transaction);
                return _mapper.Map<AdjustmentDto>(data);


            }
            else if(request.action == "reject")
            {
                var bills = _mapper.Map<Adjustments>(request.AdjustmentDto);
                var res = await _billRepository.GetAdjustmentDetails(bills.ID);
                res.AdjustmentStatus.StatusCMID = commonMasters.Where(x => x.CodeValue == "Rejected" && x.CodeType == "PaymentStatus").Select(x => x.Id).First();
                res.AdjustmentStatus.ModifedBy = request.user;
                res.AdjustmentStatus.ModifiedOn = DateTime.UtcNow;
                res.ModifiedBy = request.user;
                res.ModifiedOn = DateTime.UtcNow;
                res.Remarks = bills.Remarks;
                res.ApprovedDate = DateTime.UtcNow;
                res.ApprovedBy = request.user;
                var data = await _billRepository.UpdateAdjustmentDetails(res);
                return _mapper.Map<AdjustmentDto>(data);
            }

            return request.AdjustmentDto;
        }

        
    }
}
