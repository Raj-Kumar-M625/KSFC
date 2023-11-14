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
    /// Purpose = EditVendorDefaultsCommand Handler 
    /// Author =Swetha M 
    /// Date = 13 06 2022 
    /// </summary>
    public class EditVendorDefaultsCommandHandler : IRequestHandler<EditVendorDefaultsCommand, VendorDefaults>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        /// <summary> 
        /// Purpose = EditVendorDefaultsCommand Handler constructor
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public EditVendorDefaultsCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = EditVendorDefaultsCommand Handler to edit vendor defaults detials
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public async Task<VendorDefaults> Handle(EditVendorDefaultsCommand request, CancellationToken cancellationToken)
        {
            var vendordefaults = _mapper.Map<VendorDefaults>(request.vendorDefaultsDto);
            
            vendordefaults = await _vendorRepository.EditVendorDefaults(vendordefaults);
            return vendordefaults;
        }
    }
}
