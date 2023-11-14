using Application.DTOs.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Requests.Queries
{
    public class GetVendorListByIDRequest : IRequest<VendorDetailsDto>
    {
        public int ID { get; set; }
    }
    
}
