using Application.DTOs;
using Application.Interface.Persistence.Document;
using Application.UserStories.Document.Request.Command;
using AutoMapper;
using Common.ConstantVariables;
using Common.FileUpload;
using Domain.Document;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Document.Handlers.Commands
{
    public class CreateBillDocumentCommandHandler : IRequestHandler<CreateBillDocumentCommand, long>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private IHostingEnvironment Environment;
        public CreateBillDocumentCommandHandler(IDocumentRepository documentRepository, IMapper mapper, IHostingEnvironment _environment)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            Environment = _environment;
        }

        public async Task<long> Handle(CreateBillDocumentCommand request, CancellationToken cancellationToken)
        {

            if (request.BillPaymentDetailsDto.File.Count > 0 || request.BillPaymentDetailsDto.Attachment.Count > 0)
            {
                FileUpload fileUpload = new FileUpload();
                var ID = request.BillPaymentDetailsDto.BillsID;
                string wwwPath = this.Environment.WebRootPath;
                var filemodel = await fileUpload.UploadFiles(request.BillPaymentDetailsDto.File, request.entityPath, wwwPath,request.BillPaymentDetailsDto.DocumentName);
                //var filemodelattachment = await fileUpload.UploadFiles(request.BillPaymentDetailsDto.Attachment, request.entityPath, wwwPath,null);
                var billpayattachment = filemodel.ToList();
                foreach (var billattachment in billpayattachment)
                {
                    string entity = ValueMapping.Bill;
                    billattachment.DocumentRefID = ID;
                    billattachment.EntityType = entity;
                    billattachment.UploadedBy = request.user;
                    billattachment.Status = true;
                }
                var documentattachment = _mapper.Map<List<Documents>>(billpayattachment);
                documentattachment = await _documentRepository.AddDocumentDetails(documentattachment);
                return documentattachment[0].Id;

            }
            return 0;

        }
    }
}




