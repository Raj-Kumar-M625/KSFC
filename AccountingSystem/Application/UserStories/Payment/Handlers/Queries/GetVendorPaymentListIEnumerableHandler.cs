using Application.DTOs.Payment;
using Application.Interface.Persistence.Payment;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using Domain.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Queries
{
    public class GetVendorPaymentListIEnumerableHandler : IRequestHandler<GetVendorPaymentListIEnumerable, List<VendorPaymentListDto>>
    {
        private readonly IPaymentRepository _vendorPaymentRepository;
        private readonly IMapper _mapper;
        public GetVendorPaymentListIEnumerableHandler(IPaymentRepository vendorPaymentRepository, IMapper mapper)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
            _mapper = mapper;
        }



        public async Task<List<VendorPaymentListDto>> Handle(GetVendorPaymentListIEnumerable request, CancellationToken cancellationToken)
        {
            var vendorPaymentLists = await _vendorPaymentRepository.GetAll();
            return vendorPaymentLists;
        }

    }
}
