using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Payment.Requests.Commands;
using Application.UserStories.Transactions.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Bill;
using Domain.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Commands
{
    public class ApprovePaymentCommandHandler : IRequestHandler<ApprovePaymentCommand, int>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;
        private readonly IMediator _mediator;
        private readonly IBillRepository _vendorBillRepository;


        public ApprovePaymentCommandHandler(IPaymentRepository vendorPaymentRepository, ICommonMasterRepository commonMaster, IMapper mapper, IVendorRepository vendorRepository,
            IMediator mediator, IBillRepository billRepository)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _commonMaster = commonMaster;
            _vendorRepository = vendorRepository;
            _mapper = mapper;
            _mediator = mediator;
            _vendorBillRepository = billRepository;
        }

        public async Task<int> Handle(ApprovePaymentCommand request, CancellationToken cancellationToken)
        {

            var paymentStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.pStatus);
            decimal openingBalance = 0;
            var list = await _vendorPaymentRepository.GetPaymentsById(request.paymentId);
            if (list != null)
            {
                list.ApprovedBy = request.CurrentUser;
                list.ModifiedBy = request.CurrentUser;
                list.Remarks = request.Remarks;
                list.ModifiedOn = DateTime.Now;
                if (request.Status == ValueMapping.rejected.ToLower())
                {
                    openingBalance = list.PaymentAmountAgainstOB;
                    list.PaymentAmountAgainstOB = 0;
                }
                await _vendorPaymentRepository.ApprovePayment(list);

                var paymentStatus = await _vendorPaymentRepository.GetPaymentStatuses(request.paymentId);
                foreach (var item in paymentStatus)
                {
                    item.ModifiedOn = DateTime.Now;
                    item.ModifedBy = request.CurrentUser;
                    if (request.Status == ValueMapping.approved.ToLower() && item.Payments.PaidAmount > 0)
                    {
                        item.StatusCMID = paymentStatusList.First(p => p.CodeValue == ValueMapping.approved).Id;
                    }
                    else if (request.Status == ValueMapping.rejected.ToLower())
                    {
                        item.StatusCMID = paymentStatusList.First(p => p.CodeValue == ValueMapping.rejected).Id;

                    }
                    else if (request.Status == ValueMapping.approved.ToLower())
                    {
                        item.StatusCMID = paymentStatusList.First(p => p.CodeValue == ValueMapping.paid).Id;
                        var vendorPaymentLists = await _vendorPaymentRepository.GetBillPayment(item.PaymentID);
                        List<BillStatus> billstatuses = new List<BillStatus>();
                        var billtatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.bStatus);

                        foreach (var vendorPayment in vendorPaymentLists)
                        {
                            var ID = vendorPayment.Bill.BillReferenceNo;
                            Bills bills = await _vendorBillRepository.GetBillsByReferenceNo(ID);
                            BillStatus billStatus = bills.BillStatus;
                            if (bills.BalanceAmount == 0)
                            {

                                billStatus.StatusCMID = billtatusList.First(p => p.CodeValue == ValueMapping.paid).Id;
                            }
                            else
                            {
                                billStatus.StatusCMID = billtatusList.First(p => p.CodeValue == ValueMapping.PartiallyPaid).Id;
                            }

                            billStatus.ModifedBy = request.CurrentUser;
                            billStatus.ModifiedOn = DateTime.Now;
                            billstatuses.Add(billStatus);
                        }

                        await _vendorBillRepository.UpdateBillStatus(billstatuses);

                    }
                }
                await _vendorPaymentRepository.UpdatePaymentStatus(paymentStatus);
                if (list.Type == "Actual")
                {
                    //Update Vendor Balance
                    var vendorBalace = await _vendorRepository.GetVendorBalanceByID(list.VendorID);
                    if (request.Status != ValueMapping.rejected.ToLower())
                    {
                        vendorBalace.Paid_NetPayable += list.PaidAmount == 0 ? list.AdvanceAmountUsed : list.PaidAmount + list.AdvanceAmountUsed;
                        vendorBalace.Pending_NetPayable =  list.PaidAmount == 0 ? vendorBalace.Pending_NetPayable - list.AdvanceAmountUsed : vendorBalace.Pending_NetPayable -  (list.PaidAmount + list.AdvanceAmountUsed);

                    }
                    else if (request.Status == ValueMapping.rejected.ToLower())
                    {
                       // vendorBalace.Pending_NetPayable =  list.PaidAmount == 0 ? vendorBalace.Pending_NetPayable - list.AdvanceAmountUsed : vendorBalace.Pending_NetPayable - (list.PaidAmount + list.AdvanceAmountUsed);
                        //vendorBalace.OpeningBalance = vendorBalace.OpeningBalance + openingBalance;
                        var res = await _vendorPaymentRepository.GetBillPaymentByPaymentId(request.paymentId);
                        foreach (var item in res)
                        {
                            item.Bill.BalanceAmount = (decimal)(item.Bill.BalanceAmount + item.PaymentAmount);
                            item.IsActive = false;
                        }
                        if(list.AdvanceAmountUsed > 0)
                        {
                            var mappingAdvancePayments = await _vendorPaymentRepository.GetMappingAdvancePayments(list.ID);
                            foreach(var item in mappingAdvancePayments)
                            {
                                item.IsActive = false;
                                item.ModifiedOn = DateTime.Now;
                                item.ModifiedBy = request.CurrentUser;
                                item.Payments.BalanceAdvanceAmount = 0;

                                var advancePayment = await _vendorPaymentRepository.GetPaymentsById(item.AdvancePaymentId);
                                advancePayment.BalanceAdvanceAmount = (decimal)advancePayment.BalanceAdvanceAmount + (decimal)item.AdvanceAmount;
                                advancePayment.ModifiedBy=request.CurrentUser;
                                advancePayment.ModifiedOn = DateTime.Now;
                            }
                            await _vendorPaymentRepository.UpdateMappingAdvancePayment(mappingAdvancePayments);
                        }
                        await _vendorPaymentRepository.UpdateBillPayment(res);
                    }

                   await _vendorRepository.UpdateVendorBalance(vendorBalace);
                }
                if (list.PaidAmount == 0 && request.Status == ValueMapping.approved.ToLower())
                {
                    var mappingAdvancePayments = await _vendorPaymentRepository.GetMappingAdvancePayments(list.ID);
                    var addTransaction = await _mediator.Send(new AddPaidTransactionCommand { MappingAdvancePayment = mappingAdvancePayments, CurrentUser = request.CurrentUser });

                }

            }
            return request.paymentId;
        }
    }
}
