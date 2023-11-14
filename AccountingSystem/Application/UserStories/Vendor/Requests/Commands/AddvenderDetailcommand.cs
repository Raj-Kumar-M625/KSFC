using Application.DTOs.Vendor;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Requests.Commands
{
    public class AddvenderDetailcommand : IRequest<VendorDetailsDto>
    {

        public int ID { get; set; }

        public VendorDetailsDto vendorListByID { get; set; }
        public List<IFormFile> vendorFiles { get; set; }
        public VendorDetailsDto VendorDetailsDto { get; set; }
        public string entityPath { get; set; }
        public string entityType { get; set; }
        public string user { get; set; }


    }
}
