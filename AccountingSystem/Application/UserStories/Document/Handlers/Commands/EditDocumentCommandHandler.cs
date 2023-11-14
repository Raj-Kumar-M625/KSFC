using Application.DTOs.Document;
using Application.Interface.Persistence.Document;
using Application.UserStories.Document.Request.Command;
using AutoMapper;
using Common.FileUpload;
using Domain.Document;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Application.UserStories.Document.Handlers.Commands
{
    public class EditDocumentCommandHandler : IRequestHandler<EditDocumentCommand, int>
    {

        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private IHostingEnvironment Environment;

        public EditDocumentCommandHandler(IDocumentRepository documentRepository, IMapper mapper, IHostingEnvironment _environment)
        {
            
            _documentRepository = documentRepository;
            _mapper = mapper;
            Environment = _environment;
        }
        /// <summary>
        /// Author:Swetha M Date:18/06/2022
        /// Purpose:Get file and Upload it to DB
        /// </summary>
        /// <returns>Edit vendor details</returns>
        public async Task<int> Handle(EditDocumentCommand request, CancellationToken cancellationToken)
        {
            FileUpload fileUpload = new FileUpload();
            string wwwPath = this.Environment.WebRootPath;
            var filemodel = await fileUpload.UploadFiles(request.vendordocuments.files,request.entityPath, wwwPath, request.vendordocuments.DocumentName);

            var document = _mapper.Map<List<DocumentsDto>>(filemodel);
            var id = request.id;
            foreach (var item in document)
            {
                item.DocumentRefID = id;
                item.EntityType = request.entityType;
                item.UploadedBy = request.user;                
                item.Status = true;
            }

            var vendorDocument = _mapper.Map<List<Documents>>(document);
            vendorDocument = await _documentRepository.AddDocuments(vendorDocument);
            return vendorDocument.Count;

        }
    }
}
