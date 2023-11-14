using Application.DTOs.Document;
using Application.DTOs.Vendor;
using Application.Interface.Persistence.Document;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Vendor.Requests.Commands;
using Application.UserStories.Vendor.Requests.Queries;
using AutoMapper;
using Common.ConstantVariables;
using Common.FileUpload;
using DocumentFormat.OpenXml.Office2010.Excel;
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
    public class Addvendordetailscommandhandler : IRequestHandler<AddvenderDetailcommand, VendorDetailsDto>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private IHostingEnvironment Environment;

        public Addvendordetailscommandhandler(IVendorRepository vendorRepository, IMapper mapper, IDocumentRepository documentRepository, IHostingEnvironment _environment)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
            _documentRepository = documentRepository;
            Environment = _environment;

        }


        public async Task<VendorDetailsDto> Handle(AddvenderDetailcommand request, CancellationToken cancellationToken)
        {
            var vendor = _mapper.Map<Vendors>(request.VendorDetailsDto);
            vendor.VendorDefaults.VendorId = request.ID;
            vendor.VendorPerson.VendorId= request.ID;
            //vendor.VendorBalance.VendorId= request.ID;
            vendor.VendorBankAccounts.VendorId= request.ID;


            if (request.vendorListByID.VendorDefaults != null)
            {
                vendor.VendorDefaults.Id = request.vendorListByID.VendorDefaults.Id;
            }
            if (request.vendorListByID.VendorBankAccounts != null)
            {
                vendor.VendorBankAccounts.Id = request.vendorListByID.VendorBankAccounts.Id;
            }
            if (request.vendorListByID.VendorPerson != null)
            {
                vendor.VendorPerson.Id = request.vendorListByID.VendorPerson.Id;
                vendor.VendorPerson.Addresses.VendorPersonID = vendor.VendorPerson.Id;
                vendor.VendorPerson.Contacts.VendorPersonID= vendor.VendorPerson.Id;
            }



            vendor = await _vendorRepository.AddVendorDetail(vendor);
            
            FileUpload fileUpload = new FileUpload();
            string wwwPath = this.Environment.WebRootPath;
            var filemodel = await fileUpload.UploadFiles(request.vendorFiles, request.entityPath, wwwPath, request.VendorDetailsDto.DocumentName);
            var document = _mapper.Map<List<DocumentsDto>>(filemodel);
            if (document != null)
            {
                foreach (var docs in document)
                {
                    // For Document Upload
                    docs.DocumentRefID = request.ID;
                    docs.EntityType = request.entityType;
                    docs.CreatedOn = DateTime.UtcNow;
                    docs.UploadedBy = request.user;
                    docs.Status = true;
                    var vendorDocument = _mapper.Map<Documents>(docs);
                    vendorDocument = await _documentRepository.AddVendorDocument(vendorDocument);

                }
            }

            vendor.Id = request.ID;

            return _mapper.Map<VendorDetailsDto>(vendor);
        }
    }
}
