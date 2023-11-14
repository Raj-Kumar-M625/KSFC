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
    
    public class EditVendorBankAccountCommandHandler : IRequestHandler<EditVendorBankAccountCommand, VendorBankAccount>
    {
        
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        public EditVendorBankAccountCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
    

        public async Task<VendorBankAccount> Handle(EditVendorBankAccountCommand request, CancellationToken cancellationToken)
        {
            var vendorbankaccount = _mapper.Map<VendorBankAccount>(request.vendorBankAccountDto);
            if (vendorbankaccount.Id == 0)
            {
                vendorbankaccount.CreatedBy = request.user;

            }
            else
            {
                vendorbankaccount.ModifedBy = request.user;
                vendorbankaccount.ModifiedOn = DateTime.UtcNow;
            }          
       
            vendorbankaccount = await _vendorRepository.EditVendorBankAccount(vendorbankaccount);
            return vendorbankaccount;
        }
    }
}
