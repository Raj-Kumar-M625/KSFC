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
    public class AddDocumentCommandHandler : IRequestHandler<AddDocumentCommand, int>
    {

        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private IHostingEnvironment Environment;

        public AddDocumentCommandHandler(IDocumentRepository documentRepository, IMapper mapper, IHostingEnvironment environment)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            Environment = environment;
        }
        public async Task<int> Handle(AddDocumentCommand request, CancellationToken cancellationToken)
        {
            FileUpload fileUpload = new FileUpload();
            string wwwPath = this.Environment.WebRootPath;
            var filemodel = await fileUpload.UploadFiles(request.documents.files, request.entityPath, wwwPath,request.documents.DocumentName);

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
