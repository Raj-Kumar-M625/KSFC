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
    /// Purpose = EditVendorPersonCommand Handler 
    /// Author =Swetha M 
    /// Date = 13 06 2022 
    /// </summary>
    public class EditVendorPersonCommandHandler : IRequestHandler<EditVendorPersonCommand, VendorPerson>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        /// <summary> 
        /// Purpose = EditVendorPersonCommand Handler constructor
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public EditVendorPersonCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = EditVendorPersonCommand Handler to edit vendor person details
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public async Task<VendorPerson> Handle(EditVendorPersonCommand request, CancellationToken cancellationToken)
        {
            var vendorperson = _mapper.Map<VendorPerson>(request.vendorPerson);
            vendorperson =await _vendorRepository.EditVendorPerson(vendorperson);
            return vendorperson;

        }
    }
}
