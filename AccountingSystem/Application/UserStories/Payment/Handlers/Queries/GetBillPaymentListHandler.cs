using Application.DTOs.Payment;
using Application.Interface.Persistence.Payment;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using Domain.Payment;
using System;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Bill;

namespace Application.UserStories.Payment.Handlers.Queries
{
    public class GetBillPaymentListHandler : IRequestHandler<GetBillPaymentRequest, IQueryable<BillPayment>>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetBillPaymentListHandler(IPaymentRepository vendorPaymentRepository, IMapper mapper, IMediator mediator)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _mapper = mapper;
        }
        public async Task<IQueryable<BillPayment>> Handle(GetBillPaymentRequest request, CancellationToken cancellationToken)
        {
            var vendorPaymentLists = await _vendorPaymentRepository.GetBillPayment(request.PaymentID);
            var billId = vendorPaymentLists.Select(x => x.BillID).ToList();
            var  billlpayment = await _vendorPaymentRepository.GetBillPaymentList(billId);
            foreach (var item in vendorPaymentLists)
            {
                item.PaidAmount = (decimal)billlpayment.Where(x => x.BillID == item.BillID).Sum(x => x.PaymentAmount);
            }
            return vendorPaymentLists;
        }
    }
 
 }
