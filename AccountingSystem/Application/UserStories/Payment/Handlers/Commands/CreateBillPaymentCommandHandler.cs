using Application.Interface.Persistence.Payment;
using Application.UserStories.Payment.Requests.Commands;
using AutoMapper;
using Domain.Payment;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Commands
{


    public class CreateBillPaymentCommandHandler:IRequestHandler<CreateBillPaymentCommand,long>
    {

        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMapper _mapper;
        public CreateBillPaymentCommandHandler(IPaymentRepository vendorPaymentRepository,IMapper mapper)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateBillPaymentCommand request,CancellationToken cancellationToken)
        {
            var paymentlist = request.paymentDetails;
            // BillPayment billPayment = new BillPayment();
            List<BillPayment> BillpaymentModel = new List<BillPayment>();

            foreach (var item in request.BillItemId)
            {
                //BillItems Billitem = await _vendorPaymentRepository.GetBillItemById(id);
                BillPayment billPayment = new BillPayment();

                billPayment.BillID = item.BillID;
                billPayment.PaymentID = paymentlist.ID;
                billPayment.VendorId = paymentlist.VendorID;
                billPayment.BillAmount = item.BillAmount;
                billPayment.PaymentAmount = item.PaymentAmount;
                billPayment.CreatedOn = paymentlist.CreatedOn;
                billPayment.CreatedBy = paymentlist.CreatedBy;
                billPayment.ModifedBy = paymentlist.CreatedBy;
                billPayment.ModifiedOn = paymentlist.CreatedOn;
                billPayment.IsActive = true;
                BillpaymentModel.Add(billPayment);
            }


            var payment = await _vendorPaymentRepository.AddBillPayment(BillpaymentModel);

            return payment.Id;
        }
    }
}
