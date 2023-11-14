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
    public class GetBankStatementDocHandler : IRequestHandler<GetBankStatementDocRequest, Documents>
    {
        private readonly IDocumentRepository _documentRepository;
        public GetBankStatementDocHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public Task<Documents> Handle(GetBankStatementDocRequest request, CancellationToken cancellationToken)
        {
            return _documentRepository.GetBankStatementDocument(request.DocumentRefId);
        }
    }
}
