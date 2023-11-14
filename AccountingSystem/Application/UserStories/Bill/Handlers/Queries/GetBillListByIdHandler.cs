using Application.Interface.Persistence.Bill;
using Application.UserStories.Bill.Requests.Queries;
using AutoMapper;
using Domain.Bill;
using Domain.Vendor;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Bill.Handlers.Queries
{
    public class GetBillListByIdHandler : IRequestHandler<GetBillListByIdRequest, IQueryable<Bills>>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public GetBillListByIdHandler(IBillRepository BillRepository, IMapper mapper)
        {
            _billRepository = BillRepository;
            _mapper = mapper;
        }
        public async Task<IQueryable<Bills>> Handle(GetBillListByIdRequest request, CancellationToken cancellationToken)
        {
            var bills = _billRepository.GetBillsById(request.BillIds);
            return bills;
        }
    }

}



