using Application.Interface.Persistence.Document;
using Application.UserStories.Bill.Requests.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Commands
{


    public class DeleteDocumentRequestHandler : IRequestHandler<DeleteDocumentRequest, string[]>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment Environment;

        public DeleteDocumentRequestHandler(IDocumentRepository documentRepository, IMapper mapper, IHostingEnvironment _environment)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
            Environment = _environment;
        }

        public async Task<string[]> Handle(DeleteDocumentRequest request, CancellationToken cancellationToken)
        {

            int id = 0;
            string path = string.Empty;
            string wwwPath = this.Environment.WebRootPath;
            var document = request.document.ToList();
            for(int i = 0; i< document.Count; i++)
            {
                id = document[i];

                var billdetail = await _documentRepository.GetDocument(id);
              

                if (System.IO.File.Exists(billdetail.FilePath))
                {
                    string[] res =  billdetail.FilePath.Split('/');
                    var paths = Path.Combine(wwwPath + "/" + "Upload/ArchiveFiles/Vendor/VendorBills" + "/" + res[4] + "/");
                    path = Path.Combine(wwwPath + "/" + "Upload/ArchiveFiles/Vendor/VendorBills" + "/" + res[4] + "/" + billdetail.Name + "" + billdetail.Extension + "");
                    if (!Directory.Exists(paths))
                    {
                        Directory.CreateDirectory(paths);
                        System.IO.File.Move(billdetail.FilePath, path);                      
                    }
                    else
                    {
                        System.IO.File.Move(billdetail.FilePath, path);
                    }

                    System.IO.File.Delete(billdetail.FilePath);


                    billdetail.Status = false;
                    billdetail.FilePath = path;

                    billdetail = await _documentRepository.DeleteDocumentDetails(billdetail);
                    return new string[] { path };
                }

            }

           

           return new string[] { path };


        }
    }
}