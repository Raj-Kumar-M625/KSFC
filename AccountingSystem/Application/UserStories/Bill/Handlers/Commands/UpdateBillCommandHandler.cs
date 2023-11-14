using Application.DTOs.Bill;
using Application.DTOs.Payment;
using Application.Interface.Persistence.Bill;
using Application.UserStories.Bill.Requests.Commands;
using Application.UserStories.Payment.Requests.Commands;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using Domain.Bill;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{
    /// <summary>
    /// Author:Swetha M Date:07/06/2022
    /// Purpose: Update Bills Command Request handler
    /// </summary>
    /// <returns></returns>

    public class UpdateBillCommandHandler : IRequestHandler<UpdateBillsCommandRequest, BillsDto>
    {
        List<PaymentDto> payemntsDto = new List<PaymentDto>();
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        //Adding dependency injection for IBillRepository,IMapper and IMediator
        public UpdateBillCommandHandler(IBillRepository vendorRepository, IMapper mapper, IMediator mediator)
        {
            _billRepository = vendorRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        //Handling the Request to Update the bill Details
        public async Task<BillsDto> Handle(UpdateBillsCommandRequest request, CancellationToken cancellationToken)
        {

            List<PaymentDto> paymentModel = new List<PaymentDto>();

            List<BillsDto> billsDetails = request.bills;
            //var openingBalance = billsDetails.First().OpeningBalancePayableAmount;
            List<BillpaymentDto> BillItemIds = new List<BillpaymentDto>();
            Bills billDetail = new Bills();
            decimal paidAmount = 0;
            foreach (var bill in billsDetails)
            {
                //Get all bills from databas
                var bills = await _mediator.Send(new GetBillsListByIdRequest { ID = bill.VendorId });
                var billListById = bills.First(o => o.Id == bill.Id);
                if (billListById != null)
                {
                    bill.BillReferenceNo = billListById.BillReferenceNo;
                    bill.Vendor = billListById.Vendor;
                    bill.Vendor.VendorBalance = billListById.Vendor.VendorBalance;
                    bill.Royalty = billListById.Royalty;
                    bill.Penalty = billListById.Penalty;
                    bill.CBF = billListById.CBF;
                    bill.LabourWelfareCess = billListById.LabourWelfareCess;
                    bill.Other1 = billListById.Other1;
                    bill.Other1Value = billListById.Other1Value;
                    bill.Other2Value = billListById.Other2Value;
                    bill.Other3Value = billListById.Other3Value;
                    bill.Other2 = billListById.Other2;
                    bill.Other3 = billListById.Other3;
                    //if(bill.Vendor.VendorBalance.OpeningBalance > 0)
                    //{
                    //    bill.Vendor.VendorBalance.OpeningBalance = bill.Vendor.VendorBalance.OpeningBalance - openingBalance;
                    //}
                    //else
                    //{
                    //    bill.Vendor.VendorBalance.OpeningBalance = bill.Vendor.VendorBalance.OpeningBalance + openingBalance;
                    //}
                    bill.GSTTDS = billListById.GSTTDS;
                    bill.TDS = billListById.TDS;
                }
                if (bill.NetPayableAmount != 0)
                {
                    var billItem = new BillpaymentDto()
                    {

                        PaymentAmount = bill.NetPayableAmount,
                        BillID = bill.Id,
                        BillAmount = billListById.NetPayable,
                    };
                    BillItemIds.Add(billItem);
                }

                if (billListById != null)
                {
                    if (billListById.BalanceAmount > 0)
                    {
                        bill.BalanceAmount = billListById.BalanceAmount - bill.NetPayableAmount;
                        paidAmount = bill.NetPayableAmount;
                        bill.NetPayableAmount = 0;
                    }
                    else if(billListById.BalanceAmount == 0 && bill.PaidAmount > 0)
                    {
                        bill.BalanceAmount=(billListById.NetPayable-bill.PaidAmount) - bill.NetPayableAmount;
                    }

                }
                if (bill.BalanceAmount == 0)
                {

                    //bill.NetPayable = 0;
                    bill.BalanceAmount = 0;

                }
                //else
                //{

                //}

                //Update BillPaymentDetails
                billDetail = _mapper.Map<Bills>(bill);
                billDetail = await _billRepository.UpdateBills(billDetail);

                //Map the bill details payment
                if (billListById != null)
                {
                    if (billListById.BalanceAmount > 0)
                    {
                        paymentModel = new List<PaymentDto>()
                    {
                        new PaymentDto()
                        {
                            PaymentBillReference = bill.BillReferenceNo,
                            VendorID = bill.VendorId,
                            Vendor = billListById.Vendor,
                            PaymentAmount = paidAmount,
                            PaidAmount = paidAmount,
                            CreatedBy = bill.CreatedBy,
                            ModifiedBy = bill.ModifedBy,
                            AdvancePayments=bill.AdvancePayments,
                            AdvancePaymentUsed=bill.AdvancePaymentUsed,

                           // PaymentAmountAgainstOB = openingBalance,

                        }

                    };
                    }
                    else
                    {
                        paymentModel = new List<PaymentDto>()
                    {
                        new PaymentDto()
                        {
                            PaymentBillReference = bill.BillReferenceNo,
                            VendorID = bill.VendorId,
                            Vendor = billListById.Vendor,
                            PaymentAmount = bill.NetPayableAmount,
                            PaidAmount =bill.NetPayableAmount,
                            CreatedBy = bill.CreatedBy,
                            ModifiedBy = bill.ModifedBy,
                            AdvancePayments=bill.AdvancePayments,
                            AdvancePaymentUsed=bill.AdvancePaymentUsed,
                           // PaymentAmountAgainstOB = openingBalance,

                        }
                    };
                    }
                }

                payemntsDto.AddRange(paymentModel);
            }


            //Command to Update/Insert data into Payment table
            var command = new UpdatePaymentsCommand { payments = payemntsDto };
            var paymentId = await _mediator.Send(command);

            billDetail.PaymentRefNo = paymentId.PaymentReferenceNo;
            var createBillPayment = new CreateBillPaymentCommand { paymentDetails = paymentId, ID = billDetail.ID, BillItemId = BillItemIds };
            await _mediator.Send(createBillPayment);

            return _mapper.Map<BillsDto>(billDetail);


        }
    }
}

