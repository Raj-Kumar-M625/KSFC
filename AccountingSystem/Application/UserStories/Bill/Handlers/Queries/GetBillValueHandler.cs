using Application.DTOs.Bill;
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
    public class GetBillValueHandler : IRequestHandler<GetBillListRequest, IQueryable<Bills>>
    {
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public GetBillValueHandler(IBillRepository BillRepository, IMapper mapper)
        {
            _billRepository = BillRepository;
            _mapper = mapper;
        }
        public async Task<IQueryable<Bills>> Handle(GetBillListRequest request, CancellationToken cancellationToken)
        {
            var bills = _billRepository.GetBill();
            //var billsDto = _mapper.Map<List<BillsDto>>(bills.ToList());
            return bills;
        }
    }

}



