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
    public class AddVendorPersonCommand:IRequest<long?>
    {
        public VendorPersonDto vendorPerson { get; set; }
    }
}
