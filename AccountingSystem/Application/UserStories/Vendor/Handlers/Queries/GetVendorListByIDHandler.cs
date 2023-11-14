using Application.DTOs.Vendor;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Vendor.Requests.Queries;
using AutoMapper;
using Domain.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Handlers.Queries
{
    public class GetVendorListByIDHandler : IRequestHandler<GetVendorListByIDRequest, VendorDetailsDto>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;
        public GetVendorListByIDHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        public async Task<VendorDetailsDto>Handle(GetVendorListByIDRequest request, CancellationToken cancellationToken)
        {
            var vendors = await _vendorRepository.GetVendorsDetailsByID(request.ID);

            return _mapper.Map<VendorDetailsDto>(vendors);

          }
       
    }
}
