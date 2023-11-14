using Application.DTOs;
using Application.DTOs.Vendor;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Vendor.Requests.Commands;
using AutoMapper;
using Common.FileUpload;
using Domain;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Handlers.Commands
{
    /// <summary> 
    /// Purpose = EditVendorCommand Handler 
    /// Author =Swetha M 
    /// Date = 13 06 2022 
    /// </summary>
    public class EditVendorCommandHandler : IRequestHandler<EditVendorCommand, Vendors>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IMapper _mapper;

        /// <summary> 
        /// Purpose = EditVendorCommand Handler constructor
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public EditVendorCommandHandler(IVendorRepository vendorRepository, IMapper mapper)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
        }
        /// <summary> 
        /// Purpose = EditVendorCommand Handler to edit vendor details
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public async Task<Vendors> Handle(EditVendorCommand request, CancellationToken cancellationToken)
        {
            var vendors = _mapper.Map<Vendors>(request.vendorDetailsDto);
            vendors =await _vendorRepository.EditVendor(vendors);
            return vendors;
        }
    }
}
