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
    /// Purpose = AddVendorDefaultsCommand Handler 
    /// Author =Swetha M 
    /// Date = 13 06 2022 
    /// </summary>
    public class AddVendorDefaultsCommandHandler : IRequestHandler<AddVendorDefaultsCommand, int?>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        /// <summary> 
        /// Purpose = AddVendorDefaultsCommand Handler Constructor
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public AddVendorDefaultsCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = AddVendorDefaultsCommand Handler to add vendor defaults detials
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public async Task<int?> Handle(AddVendorDefaultsCommand request, CancellationToken cancellationToken)
        {
            var vendordefaults = _mapper.Map<VendorDefaults>(request.vendorDefaultsDto);
            
            vendordefaults = await _vendorRepository.AddVendorDefaults(vendordefaults);
            return vendordefaults.Id;
        }
    }
}
