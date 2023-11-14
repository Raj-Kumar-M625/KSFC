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
    public class AddAdvancePaymentCommandHandler : IRequestHandler<AddAdvancePaymentCommand, string>

    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;
        public AddAdvancePaymentCommandHandler(IPaymentRepository vendorPaymentRepository, ICommonMasterRepository commonMaster, IMapper mapper, IVendorRepository vendorRepository)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _commonMaster = commonMaster;
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        public async Task<string> Handle(AddAdvancePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var paymentStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.pStatus);
                var paymentType = await _commonMaster.GetCommoMasterValues("PaymentType");

                var payment = _mapper.Map<Payments>(request.payment);
                payment.CreatedBy = request.currentUser;
                payment.CreatedOn = DateTime.Now;
                payment.ModifiedBy = request.currentUser;
                payment.ModifiedOn = DateTime.Now;
                payment.PaymentAmount = payment.PaidAmount;
                payment.BalanceAdvanceAmount = payment.PaidAmount;
                payment.Type = paymentType.Where(x => x.CodeName == "Advance").FirstOrDefault().CodeValue;
                var addedPayment = await _vendorPaymentRepository.AddAdvancePayment(payment);
                PaymentStatus advancePaymentStatus = new PaymentStatus
                {
                    PaymentID = addedPayment.ID,
                    StatusCMID = paymentStatusList.Where(x => x.CodeName == ValueMapping.pending).FirstOrDefault().Id,
                    VendorId=request.payment.VendorID,
                    CreatedBy = request.currentUser,
                    CreatedOn = DateTime.Now,
                    ModifedBy=request.currentUser,
                    ModifiedOn=DateTime.Now
                };
                var addedPaymentStatus=await _vendorPaymentRepository.AddPaymentStatus(advancePaymentStatus);

                return addedPayment.PaymentReferenceNo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

