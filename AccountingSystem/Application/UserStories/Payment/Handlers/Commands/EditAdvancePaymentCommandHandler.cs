using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Payment.Requests.Commands;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Commands
{
    public class EditAdvancePaymentCommandHandler : IRequestHandler<EditAdvancePaymentCommand, string>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMapper _mapper;
        private readonly IVendorRepository _vendorRepository;
        public EditAdvancePaymentCommandHandler(IPaymentRepository vendorPaymentRepository, ICommonMasterRepository commonMaster, IMapper mapper, IVendorRepository vendorRepository)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _commonMaster = commonMaster;
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(EditAdvancePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await _vendorPaymentRepository.GetPaymentsById(request.payment.ID);
                if (payment != null)
                {
                    payment.Description = request.payment.Description;
                    payment.PaidAmount = request.payment.PaidAmount;
                    payment.PaymentAmount = request.payment.PaymentAmount;
                    payment.PaymentDate = request.payment.PaymentDate;
                    payment.PaymentBillReference = request.payment.PaymentBillReference;
                    payment.ModifiedBy = request.currentUser;
                    payment.ModifiedOn = DateTime.Now;
                    var editedPayment=await _vendorPaymentRepository.UpdateAdvancePayment(payment);
                    return payment.PaymentReferenceNo;

                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
