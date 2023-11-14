using Application.DTOs.Document;
using Application.DTOs.Vendor;
using Application.Interface.Persistence.Document;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Vendor.Requests.Commands;
using AutoMapper;
using Common.FileUpload;
using Domain.Document;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Handlers.Commands
{
    /// <summary> 
    /// Purpose = UpdateVendorDetailsCommand Handler 
    /// Author =Swetha M 
    /// Date = 13 06 2022 
    /// </summary>
    public class UpdateVendorDetailsCommandHandler : IRequestHandler<UpdateVendorDetailsCommand, VendorDetailsDto>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private IHostingEnvironment Environment;
        /// <summary> 
        /// Purpose = UpdateVendorDetailsCommand Handler Constructor
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        public UpdateVendorDetailsCommandHandler(IVendorRepository vendorRepository, IMapper mapper, 
                                                  IDocumentRepository documentRepository, IHostingEnvironment _environment)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
            _documentRepository = documentRepository;
            Environment = _environment;
        }

        /// <summary> 
        /// Purpose = UpdateVendorDetailsCommand Handler to Update Vendor details
        /// Author =Swetha M 
        /// Date = 13 06 2022 
        /// </summary>
        async Task<VendorDetailsDto> IRequestHandler<UpdateVendorDetailsCommand, VendorDetailsDto>.Handle(UpdateVendorDetailsCommand request, CancellationToken cancellationToken)
        {

            var vendor = _mapper.Map<Vendors>(request.VendorDetailsDto);
            vendor = await _vendorRepository.UpdateVendor(vendor);
            FileUpload fileUpload = new FileUpload();
            string wwwPath = this.Environment.WebRootPath;
            var filemodel = await fileUpload.UploadFiles(request.VendorDetailsDto.files, request.entityPath, wwwPath, request.VendorDetailsDto.DocumentName);
                var document = _mapper.Map<List<DocumentsDto>>(filemodel);
                if (document != null)
                {
                    foreach (var docs in document)
                    {
                        // For Document Upload
                        docs.DocumentRefID = vendor.Id;
                        docs.EntityType = request.entityType;
                        docs.CreatedOn = DateTime.UtcNow;
                        docs.UploadedBy = request.user;
                        docs.Status = true;
                        var vendorDocument = _mapper.Map<Documents>(docs);
                        vendorDocument = await _documentRepository.AddVendorDocument(vendorDocument);

                    }
                }
                
                       
            return _mapper.Map<VendorDetailsDto>(vendor);
        }
    }
}
