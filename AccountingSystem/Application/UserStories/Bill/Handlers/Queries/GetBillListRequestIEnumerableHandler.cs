using Application.DTOs.Bill;
using Application.Interface.Persistence.Bill;
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
    public class GetBillListRequestIEnumerableHandler : IRequestHandler<GetBillListRequestIEnumerable, List<BillListDto>>
    {
        private readonly IBillRepository _billrepository;
        private readonly IMapper _mapper;
        public GetBillListRequestIEnumerableHandler(IBillRepository billRepository, IMapper mapper)
        {
            _billrepository = billRepository;
            _mapper = mapper;
        }
        public async Task<List<BillListDto>> Handle(GetBillListRequestIEnumerable request, CancellationToken cancellationToken)
        {           
           var bill = await _billrepository.GetAll();
          
            return bill;
        }


    }

   
}
