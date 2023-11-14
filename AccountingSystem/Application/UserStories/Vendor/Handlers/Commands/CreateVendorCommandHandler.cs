using Application.DTOs.Document;
using Application.Interface.Persistence.Document;
using Application.Interface.Persistence.Vendor;
using Application.UserStories.Vendor.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Common.FileUpload;
using Domain.Document;
using Domain.Vendor;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Vendor.Handlers.Commands
{
    /// <summary>
    /// Purpose = CreateVendorCommand Handler 
    /// Author = Kartik
    /// Date = 13 04 2022 
    /// Modiified By=Swetha M
    /// Modified on= 11 05 2022
    /// </summary>
    internal class CreateVendorCommandHandler:IRequestHandler<CreateVendorCommand,long>
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private IHostingEnvironment Environment;

        /// <summary>
        /// Purpose = CreateVendorCommand Handler Consturctor
        /// Author = Kartik
        /// Date = 13 04 2022 
        /// Modiified By=Swetha M
        /// Modified on= 11 05 2022
        /// </summary>
        public CreateVendorCommandHandler(IVendorRepository vendorRepository,IMapper mapper,IDocumentRepository documentRepository,IHostingEnvironment _environment)
        {
            _vendorRepository = vendorRepository;
            _mapper = mapper;
            _documentRepository = documentRepository;
            Environment = _environment;
        }

        /// <summary>
        /// Purpose = CreateVendorCommand Handler to create the vednor
        /// Author = Kartik
        /// Date = 13 04 2022 
        /// Modiified By=Swetha M
        /// Modified on= 11 05 2022
        /// </summary>
        public async Task<long> Handle(CreateVendorCommand request,CancellationToken cancellationToken)
        {
            FileUpload fileUpload = new FileUpload();
            string wwwPath = this.Environment.WebRootPath;
            var documentFilemodel = await fileUpload.UploadFiles(request.vendorFiles,request.entityPath,wwwPath,request.VendorDetailsDto.DocumentName);

            List<DocumentsDto> documents = _mapper.Map<List<DocumentsDto>>(documentFilemodel);

            //For Add Vendor
            var vendor = _mapper.Map<Vendors>(request.VendorDetailsDto);
            var vendorbaalnce = new VendorBalance()
            {
                OpeningBalance = request.VendorDetailsDto.VendorBalance.OpeningBalance,
                TotalBillAmount = 0,
                CreatedOn = DateTime.Now,
                CreatedBy = request.user,
                ModifiedBy = request.user,
               
                OpeningBalanceDate = request.VendorDetailsDto.VendorBalance.OpeningBalanceDate,

            };
            vendor.CreatedBy = request.user;
            vendor.ModifiedBy = request.user;
            vendor.ModifiedOn = DateTime.UtcNow;
            vendor.VendorPerson.Addresses.CreatedBy = request.user;
            vendor.VendorPerson.Addresses.ModifiedBy = request.user;
            vendor.VendorPerson.Contacts.CreatedBy = request.user;
            vendor.VendorPerson.Contacts.ModifiedBy = request.user;
            vendor.VendorPerson.CreatedBy = request.user;
            vendor.VendorPerson.ModifiedBy = request.user;
            vendor.VendorPerson.CreatedOn = DateTime.UtcNow;
            vendor.VendorPerson.ModifiedOn = DateTime.UtcNow;
            vendor.VendorBankAccounts.CreatedBy = request.user;
            vendor.VendorBankAccounts.ModifiedOn = DateTime.UtcNow;
            vendor.VendorBankAccounts.ModifedBy = request.user;
            vendor.VendorDefaults.CreatedBy = request.user;
            vendor.VendorDefaults.CreatedOn = DateTime.UtcNow;
            vendor.VendorDefaults.ModifiedOn = DateTime.UtcNow;
            vendor.VendorDefaults.ModifiedBy = request.user;

            vendor.VendorBalance = vendorbaalnce;
            vendor = await _vendorRepository.AddVendor(vendor);
            foreach (var doc in documents)
            {
                // For Document Upload
                doc.DocumentRefID = vendor.Id;
                doc.EntityType = ValueMapping.Vendor;
                doc.UploadedBy = request.user;
                doc.Description = request.DocumentType;
                doc.CreatedOn = DateTime.UtcNow;
                doc.Status = true;
                var vendorDocument = _mapper.Map<Documents>(doc);
                vendorDocument = await _documentRepository.AddVendorDocument(vendorDocument);

            }
            return vendor.Id;

        }

    }
}
