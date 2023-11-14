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
    public class GetVendorListRequestHandler : IRequestHandler<GetVendorListRequest, List<VendorDetailsDto>>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public GetVendorListRequestHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        public async Task<List<VendorDetailsDto>> Handle(GetVendorListRequest request, CancellationToken cancellationToken)
        {
            var vendors = await _vendorRepository.GetAll();
            return _mapper.Map<List<VendorDetailsDto>>(vendors);
        }
    }
}
