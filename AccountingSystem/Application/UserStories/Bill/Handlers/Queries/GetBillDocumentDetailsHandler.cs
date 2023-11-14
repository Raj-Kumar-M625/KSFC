using Application.DTOs.Document;
using Application.Interface.Persistence.Document;
using Application.UserStories.Bill.Requests.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Queries
{

    public class GetBillDocumentDetailsHandler : IRequestHandler<GetBillDocumentDetailsRequest, List<DocumentsDto>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        public GetBillDocumentDetailsHandler(IDocumentRepository documentRepository, IMapper mapper)
        {
            _documentRepository = documentRepository;
            _mapper = mapper;
        }
        public async Task<List<DocumentsDto>> Handle(GetBillDocumentDetailsRequest request, CancellationToken cancellationToken)
        {
            var Documents = await _documentRepository.GetDocumentDetails(request.ID);
            return _mapper.Map<List<DocumentsDto>>(Documents);
        }
    }
}
