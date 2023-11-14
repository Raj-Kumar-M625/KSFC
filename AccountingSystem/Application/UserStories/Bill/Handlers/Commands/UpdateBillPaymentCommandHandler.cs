using Application.DTOs.Payment;
using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Payment;
using Application.UserStories.Bill.Requests.Commands;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Drawing;
using Domain.Payment;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{
    public class UpdateBillPaymentCommandHandler : IRequestHandler<UpdateBillPaymentCommandRequest, string>
    {
        List<PaymentDto> payemntsDto = new List<PaymentDto>();
        private readonly IBillRepository _billRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public UpdateBillPaymentCommandHandler(IBillRepository billRepository, IMapper mapper, IMediator mediator, IPaymentRepository paymentRepository)
        {
            _billRepository = billRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<string> Handle(UpdateBillPaymentCommandRequest request, CancellationToken cancellationToken)
        {
            List<BillPayment> billsDetails = request.bills;
            List<MappingAdvancePayment> mappingAdvancePayments = new();
            List<MappingAdvancePayment> UpdatemappingAdvancePayments = new();
            decimal advanceAmountUsed = 0;
            if (request.bills.FirstOrDefault().AdvancePayments != null)
            {
                List<Payments> advancePayments = JsonConvert.DeserializeObject<List<Payments>>(request.bills.FirstOrDefault().AdvancePayments);
                if (advancePayments.Any())
                {
                    var mappingPayments = await _paymentRepository.GetMappingAdvancePayments(billsDetails.FirstOrDefault().PaymentID);

                    foreach (var item in advancePayments)
                    {
                        if (item.PaymentAmount == 0 && item.BalanceAmount > 0)
                        {
                            var payments = await _paymentRepository.GetPaymentsById(item.ID);
                            payments.BalanceAdvanceAmount = item.BalanceAmount;
                            payments.ModifiedOn = DateTime.UtcNow;
                            var updatePayments = await _paymentRepository.UpdateAdvancePayment(payments);
                            var mappedAdvancePayment = mappingPayments.Where(x => x.AdvancePaymentId == item.ID).FirstOrDefault();

                            if (payments.PaidAmount == item.BalanceAmount)
                            {
                                mappedAdvancePayment.IsActive = false;
                                mappedAdvancePayment.ModifiedOn = DateTime.UtcNow;
                                mappedAdvancePayment.Payments.BalanceAdvanceAmount = 0;
                                UpdatemappingAdvancePayments.Add(mappedAdvancePayment);
                            }
                            else
                            {
                                mappedAdvancePayment.AdvanceAmount = payments.PaidAmount - item.BalanceAmount;
                                mappedAdvancePayment.ModifiedOn = DateTime.UtcNow;
                                mappedAdvancePayment.Payments.BalanceAdvanceAmount = 0;
                                UpdatemappingAdvancePayments.Add(mappedAdvancePayment);

                            }

                        }
                        else
                        {
                            var payments = await _paymentRepository.GetPaymentsById(item.ID);

                            var alreadyMapped = mappingPayments.Where(x => x.AdvancePaymentId == item.ID).FirstOrDefault();
                            if (alreadyMapped != null)
                            {
                                alreadyMapped.AdvanceAmount = payments.PaidAmount - item.BalanceAmount;
                                alreadyMapped.ModifiedOn = DateTime.UtcNow;
                                alreadyMapped.Payments.BalanceAdvanceAmount = 0;
                                UpdatemappingAdvancePayments.Add(alreadyMapped);
                            }
                            else
                            {
                                var mappingAdvancePayment = new MappingAdvancePayment()
                                {
                                    AdvancePaymentId = item.ID,
                                    PaymentsID = billsDetails.FirstOrDefault().PaymentID,
                                    AdvanceAmount = item.PaymentAmount - item.BalanceAmount,
                                    CreatedBy = billsDetails.FirstOrDefault().CreatedBy,
                                    ModifiedBy = billsDetails.FirstOrDefault().CreatedBy,
                                    CreatedOn = DateTime.UtcNow,
                                    ModifiedOn = DateTime.UtcNow,
                                    VendorId = billsDetails.FirstOrDefault().VendorId,
                                    IsActive = true
                                };
                                mappingAdvancePayments.Add(mappingAdvancePayment);
                            }
                            payments.BalanceAdvanceAmount = item.BalanceAmount;
                            payments.ModifiedOn = DateTime.UtcNow;
                            var updatePayments = await _paymentRepository.UpdateAdvancePayment(payments);

                        }
                    }

                }
                else
                {
                    var mappingPayments = await _paymentRepository.GetMappingAdvancePayments(billsDetails.FirstOrDefault().PaymentID);
                    if (mappingPayments.Any())
                    {
                        foreach (var mappingAdvancePayment in mappingPayments)
                        {
                            mappingAdvancePayment.IsActive = false;
                            mappingAdvancePayment.ModifiedOn = DateTime.UtcNow;
                            mappingAdvancePayment.Payments.BalanceAdvanceAmount = 0;
                            UpdatemappingAdvancePayments.Add(mappingAdvancePayment);
                        }

                    }

                }
            }

            if (mappingAdvancePayments.Any())
            {
                var mappedAdvancePaymenstAdd = await _paymentRepository.AddMappingAdvancePayment(mappingAdvancePayments);
            }
            if (UpdatemappingAdvancePayments.Any())
            {
                var updatedMappingAdvancePayments = await _paymentRepository.UpdateMappingAdvancePayment(UpdatemappingAdvancePayments);
            }

            advanceAmountUsed = (decimal)mappingAdvancePayments.Where(x => x.IsActive == true).Sum(x => x.AdvanceAmount)
                + (decimal)UpdatemappingAdvancePayments.Where(x => x.IsActive == true).Sum(x => x.AdvanceAmount);


            BillPayment obbj = new BillPayment();

            var paidAmount = billsDetails.Sum(x => x.PaymentAmount) - advanceAmountUsed;

            foreach (var res in billsDetails)
            {
                var billpay = await _paymentRepository.GetBillPaymentById(res.Id);
                obbj = billpay;
                obbj.PaymentAmount = res.PaymentAmount;
                obbj.Payments.PaidAmount = (decimal)paidAmount < 0 ? 0 : (decimal)paidAmount;
                obbj.Payments.PaymentAmount = (decimal)billsDetails.Sum(x => x.PaymentAmount);
                obbj.Payments.AdvanceAmountUsed = advanceAmountUsed;
                //obbj.Payments.PaymentAmountAgainstOB = res.Payments.PaymentAmountAgainstOB;
                obbj.Bill.BalanceAmount = (decimal)((obbj.Bill.NetPayable -res.PaidAmount) - res.PaymentAmount);
                obbj.Vendor.VendorBalance.OpeningBalance = res.Payments.Vendor.VendorBalance.OpeningBalance;

                await _paymentRepository.UpdateBillPayment(obbj);
            }

            return obbj.Payments.PaymentReferenceNo;

        }
    }
}
