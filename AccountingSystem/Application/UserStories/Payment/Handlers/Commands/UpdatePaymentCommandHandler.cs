using Application.DTOs.Payment;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Payment.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Payment;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Commands
{
    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentsCommand, PaymentDto>
    {


        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;
        public UpdatePaymentCommandHandler(IPaymentRepository vendorPaymentRepository, ICommonMasterRepository commonMaster, IMapper mapper, IVendorRepository vendorRepository)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _commonMaster = commonMaster;
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }


        public async Task<PaymentDto> Handle(UpdatePaymentsCommand request, CancellationToken cancellationToken)
        {
            List<Payments> advancePaymentsUsed = new();
            List<MappingAdvancePayment> mappingAdvancPayments = new();
            List<Payments> payments = _mapper.Map<List<Payments>>(request.payments);

            var paymentType = await _commonMaster.GetCommoMasterValues("PaymentType");

            var paymentStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.pStatus);
            var pendingStatusId = paymentStatusList.FirstOrDefault(p => p.CodeValue == ValueMapping.pending)?.Id;

            var paidAmount = payments.Sum(x => x.PaymentAmount) - request.payments.FirstOrDefault().AdvancePaymentUsed;
            var balanceAmount = request.payments.FirstOrDefault().AdvancePaymentUsed - payments.Sum(x => x.PaymentAmount);
            var paymentDetails = new Payments()
            {
                VendorID = payments.FirstOrDefault().VendorID,
                PaymentBillReference = payments.FirstOrDefault().PaymentBillReference,
                //PaymentAmount = payments.FirstOrDefault().PaymentAmount,
                PaymentAmount = payments.Sum(x => x.PaymentAmount),
                PaidAmount = paidAmount < 0 ? 0 : paidAmount,
                AdvanceAmountUsed = paidAmount >= 0 ? request.payments.FirstOrDefault().AdvancePaymentUsed : payments.Sum(x => x.PaymentAmount),
                // PaymentAmountAgainstOB = payments.FirstOrDefault().PaymentAmountAgainstOB,
                //PaidAmount = payments.FirstOrDefault().PaidAmount,
                PaymentStatus = new PaymentStatus()
                {
                    VendorId = payments.FirstOrDefault().VendorID,
                    CreatedBy = payments.FirstOrDefault().CreatedBy,
                    CreatedOn = DateTime.UtcNow,
                    ModifedBy = payments.FirstOrDefault().ModifiedBy,
                    StatusCMID = pendingStatusId.HasValue ? pendingStatusId.Value : 0
                },
                CreatedOn = DateTime.UtcNow,
                CreatedBy = payments.FirstOrDefault().CreatedBy,
                ModifiedBy = payments.FirstOrDefault().ModifiedBy,
                Vendor = payments.FirstOrDefault().Vendor,
                PaymentDate = DateTime.UtcNow,
                Type = paymentType.Where(x => x.CodeName == "Actual").FirstOrDefault().CodeValue
            };
            paymentDetails = await _vendorPaymentRepository.UpdateAsync(paymentDetails);

            if (request.payments.FirstOrDefault().AdvancePaymentUsed > 0)
            {
                List<Payments> adavncePayments = JsonConvert.DeserializeObject<List<Payments>>(request.payments.FirstOrDefault().AdvancePayments);
                foreach (var item in adavncePayments)
                {
                    var advancePayment = await _vendorPaymentRepository.GetPaymentsById(item.ID);
                    advancePayment.BalanceAdvanceAmount = item.BalanceAmount;
                    advancePayment.ModifiedOn = DateTime.UtcNow;
                    var updatePayment = await _vendorPaymentRepository.UpdateAdvancePayment(advancePayment);

                    var mappingAdvancePayment = new MappingAdvancePayment()
                    {
                        AdvancePaymentId = item.ID,
                        PaymentsID = paymentDetails.ID,
                        AdvanceAmount = item.PaymentAmount - item.BalanceAmount,
                        CreatedBy = payments.FirstOrDefault().CreatedBy,
                        ModifiedBy = payments.FirstOrDefault().CreatedBy,
                        CreatedOn = DateTime.UtcNow,
                        ModifiedOn = DateTime.UtcNow,
                        VendorId = advancePayment.VendorID,
                        IsActive = true
                    };
                    mappingAdvancPayments.Add(mappingAdvancePayment);
                }


                var mappingAvancePayments = await _vendorPaymentRepository.AddMappingAdvancePayment(mappingAdvancPayments);
            }

            return _mapper.Map<PaymentDto>(paymentDetails);
        }
    }
}

