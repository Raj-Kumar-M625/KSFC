using Application.DTOs.Document;
using Application.Interface.Persistence.Document;
using Application.UserStories.Document.Request.Queries;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Document.Handlers.Queries
{
    public class GetVendorDocumentDetailsHandler : IRequestHandler<GetVendorDocumentDetailsRequest, List<DocumentsDto>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        public GetVendorDocumentDetailsHandler(IDocumentRepository documentRepository, IMapper mapper)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
        }
        public async Task<List<DocumentsDto>> Handle(GetVendorDocumentDetailsRequest request, CancellationToken cancellationToken)
        {
            var Documents = await _documentRepository.GetDocumentDetails(request.ID);
            return _mapper.Map<List<DocumentsDto>>(Documents);
        }
    }
}
