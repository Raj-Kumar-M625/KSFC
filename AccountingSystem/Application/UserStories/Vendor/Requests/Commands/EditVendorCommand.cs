using Application.DTOs.Vendor;
using Domain.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.UserStories.Vendor.Requests.Commands
{
    public class EditVendorCommand:IRequest<Vendors>
    {
        public VendorDetailsDto vendorDetailsDto { get;set;}
    }
}
