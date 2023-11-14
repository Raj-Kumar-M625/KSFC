using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Payment.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Master;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Commands
{
    public class UpdatePaymentStatusCommandHandler : IRequestHandler<UpdatePaymentStatusCommand, int>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;
        public UpdatePaymentStatusCommandHandler(IPaymentRepository vendorPaymentRepository, ICommonMasterRepository commonMaster, IMapper mapper, IVendorRepository vendorRepository)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _commonMaster = commonMaster;
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            List<PaymentStatus> paymentStatuses = new List<PaymentStatus>();
            var paymentStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.pStatus);
            foreach (var id in request.PaymentID)
            {
                Payments payments = await _vendorPaymentRepository.GetPaymentsById(id);

                PaymentStatus paymentStatus = payments.PaymentStatus;
                paymentStatus.StatusCMID = paymentStatusList.First(p => p.CodeValue == ValueMapping.pbankFile).Id;
                paymentStatus.ModifedBy = payments.CreatedBy;
                paymentStatus.ModifiedOn = DateTime.Now;
                paymentStatuses.Add(paymentStatus);
            }
            await _vendorPaymentRepository.UpdatePaymentStatus(paymentStatuses);
            return request.PaymentID.FirstOrDefault();
        }
    }
}
