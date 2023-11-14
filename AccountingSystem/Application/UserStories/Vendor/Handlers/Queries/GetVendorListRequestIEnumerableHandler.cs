using Application.DTOs.Vendor;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Vendor.Requests.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Handlers.Queries
{

    public class GetVendorListRequestIEnumerableHandler : IRequestHandler<GetVendorListRequestIEnumerable, List<VendorListDto>>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;
        public GetVendorListRequestIEnumerableHandler(IVendorRepository billRepository, IMapper mapper)
        {
            _vendorRepository = billRepository;
            _mapper = mapper;
        }
        public async Task<List<VendorListDto>> Handle(GetVendorListRequestIEnumerable request, CancellationToken cancellationToken)
        {
            var bill = await _vendorRepository.GetVendorList();         
            return bill;
        }


    }
}
