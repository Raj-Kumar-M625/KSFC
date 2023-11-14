using Application.Interface.Persistence.Vendor;
using Application.UserStories.Vendor.Requests.Queries;
using AutoMapper;
using Domain.Vendor;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Handlers.Queries
{

    public class GetVendorQuerableValueHandler:IRequestHandler<GetVenorQuerableValue,IQueryable<Vendors>>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public GetVendorQuerableValueHandler(IVendorRepository vendorRepository,IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }

        public async Task<IQueryable<Vendors>> Handle(GetVenorQuerableValue request,CancellationToken cancellationToken)
        {
            var vendors = _vendorRepository.GetVendor();
            return vendors;
        }
    }
}
