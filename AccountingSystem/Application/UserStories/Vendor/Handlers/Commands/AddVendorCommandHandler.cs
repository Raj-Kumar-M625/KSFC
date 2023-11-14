using Application.Interface.Persistence.Vendor;
using Application.UserStories.Vendor.Requests.Commands;
using AutoMapper;
using Domain.Vendor;
using MediatR;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Handlers.Commands
{
    /// <summary> 
    /// Purpose = AddVendorCommand Handler 
    /// Author =Swetha M 
    /// Date = 13 06 2022 
    /// </summary>
    public class AddVendorCommandHandler:IRequestHandler<AddVendorCommand,int>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        /// <summary> 
        /// Purpose = AddVendorCommand Handler Constructor
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public AddVendorCommandHandler(IVendorRepository vendorRepository,IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = AddVendorCommand Handler to add vendor details
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public async Task<int> Handle(AddVendorCommand request,CancellationToken cancellationToken)
        {
            try
            {
                var vendors = _mapper.Map<Vendors>(request.vendor);

               

                var vendorbaalnce = new VendorBalance()
                {
                    OpeningBalance = request.vendor.VendorBalance.OpeningBalance,
                    TotalBillAmount = 0,
                   
                    OpeningBalanceDate = request.vendor.VendorBalance.OpeningBalanceDate,


                };
                vendors.VendorBalance = vendorbaalnce;
                vendors = await _vendorRepository.Add(vendors);
                return vendors.Id;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }
}
