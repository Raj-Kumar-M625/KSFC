using Application.Interface.Persistence.Document;
using Application.UserStories.Document.Request.Queries;
using Domain.Document;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Document.Handlers.Queries
{
    public class GetDocumentByRefIdRequestHandler : IRequestHandler<GetDocumentByRefIdRequest, Documents>
    {
        private readonly IDocumentRepository _documentRepository;
        public GetDocumentByRefIdRequestHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public Task<Documents> Handle(GetDocumentByRefIdRequest request, CancellationToken cancellationToken)
        {
            return _documentRepository.GetTDSDocument(request.DocumentRefId);
        }
    }
}
