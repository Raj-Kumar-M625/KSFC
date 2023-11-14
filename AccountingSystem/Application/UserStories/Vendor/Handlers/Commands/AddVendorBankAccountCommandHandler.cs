using Application.DTOs.Vendor;
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
    /// Purpose = AddVendorBankAccountCommandHandler
    /// Author = Karthik 
    /// Date = May 05 2022 
    /// </summary>
    public class AddVendorBankAccountCommandHandler : IRequestHandler<AddVendorBankAccountCommand, int?>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Purpose = AddVendorBankAccountCommandHandler Constructor
        /// Author = Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public AddVendorBankAccountCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = AddVendorBankAccountCommand Handler to add vendorbank details
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public async Task<int?> Handle(AddVendorBankAccountCommand request, CancellationToken cancellationToken)
        {
            var vendorbankaccount = _mapper.Map<VendorBankAccount>(request.vendorBankAccountDto);
            vendorbankaccount = await _vendorRepository.AddVendorBankAccount(vendorbankaccount);
            return vendorbankaccount.Id;
        }
    }
}
