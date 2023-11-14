using Application.DTOs.Document;
using Application.Interface.Persistence.Document;
using Application.UserStories.Document.Request.Command;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Document.Handlers.Commands
{
    public class DeleteVendorDocumnetCommandHandler : IRequestHandler<DeleteVendorDocumnetCommand, DocumentsDto>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private IHostingEnvironment Environment;
        public DeleteVendorDocumnetCommandHandler(IDocumentRepository documentRepository, IMapper mapper, IHostingEnvironment environment)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            Environment = environment;
        }
        public async Task<DocumentsDto> Handle(DeleteVendorDocumnetCommand request, CancellationToken cancellationToken)
        {
            try
            {

               
                var document = await _documentRepository.GetDocument(request.ID);
                
                //var document = await _documentRepository.DeleteVendorDocumentDetails(request.ID);

                string wwwPath = this.Environment.WebRootPath;

                string[] res = document.FilePath.Split('/');

                var paths = Path.Combine(wwwPath + "/" + "Upload/ArchiveFiles/Vendor/VendorDetails" + "/" + res[4] + "/");
                var path = Path.Combine(wwwPath + "/" + "Upload/ArchiveFiles/Vendor/VendorDetails" + "/" + res[4]+ "/"+ document.Name +""+document.Extension+"");

                if (!Directory.Exists(paths))
                {
                    Directory.CreateDirectory(paths);
                    System.IO.File.Move(document.FilePath, path);
                }
                else
                {
                    System.IO.File.Move(document.FilePath, path);
                }

                System.IO.File.Delete(document.FilePath);

                document.Status = false;
                document.FilePath = path;

                document = await _documentRepository.DeleteDocumentDetails(document);



                return _mapper.Map<DocumentsDto>(document);
            }catch(Exception e)
            {
                throw e;
            }
        }
    }
}
