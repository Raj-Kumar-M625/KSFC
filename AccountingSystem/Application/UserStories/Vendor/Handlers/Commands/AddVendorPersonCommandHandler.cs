using Application.Interface.Persistence.Vendor;
using Application.UserStories.Vendor.Requests.Commands;
using AutoMapper;
using Domain.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Handlers.Commands
{
    /// <summary> 
    /// Purpose = AddVendorPersonCommand Handler 
    /// Author =Swetha M 
    /// Date = 13 06 2022 
    /// </summary>
    public class AddVendorPersonCommandHandler : IRequestHandler<AddVendorPersonCommand, long?>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        /// <summary> 
        /// Purpose = AddVendorPersonCommand Contsructor 
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public AddVendorPersonCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = AddVendorPersonCommand Handler  to add vendor person details
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public async Task<long?> Handle(AddVendorPersonCommand request, CancellationToken cancellationToken)
        {
            var vendorperson = _mapper.Map<VendorPerson>(request.vendorPerson);
            vendorperson =await _vendorRepository.AddVendorPerson(vendorperson);
            return vendorperson.Id;

        }
    }
}
