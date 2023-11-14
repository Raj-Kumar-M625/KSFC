using Application.Interface.Persistence.Bill;
using Application.UserStories.Bill.Requests.Queries;
using AutoMapper;
using Domain.Bill;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Queries
{
    public class GetBillsForGsdttdsRequestHandler : IRequestHandler<GetBillsForGsttdsRequest, IQueryable<Bills>>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public GetBillsForGsdttdsRequestHandler(IBillRepository BillRepository, IMapper mapper)
        {
            _billRepository = BillRepository;
            _mapper = mapper;
        }
        public async Task<IQueryable<Bills>> Handle(GetBillsForGsttdsRequest request, CancellationToken cancellationToken)
        {
            var res = _billRepository.GetBillForGSTTDS();
            return res;
        }
    }
}
