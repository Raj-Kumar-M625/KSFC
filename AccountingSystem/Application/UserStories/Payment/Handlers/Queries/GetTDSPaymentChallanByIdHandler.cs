using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Payment;
using Application.UserStories.Bill.Requests.Queries;
using AutoMapper;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Payment.Handlers.Queries
{
    public class GetTdsPaymentChallanByIdHandler : IRequestHandler<GetTdsPaymentChallanByIdRequest, IQueryable<TdsPaymentChallan>>
    {
        private readonly ITdsPaymentChallanRepository _tdsPaymentChallanRepository;
        private readonly IMapper _mapper;

        public GetTdsPaymentChallanByIdHandler(ITdsPaymentChallanRepository tdsPaymentChallanRepository, IMapper mapper)
        {
            _tdsPaymentChallanRepository = tdsPaymentChallanRepository;
            _mapper = mapper;
        }
        public async Task<IQueryable<TdsPaymentChallan>> Handle(GetTdsPaymentChallanByIdRequest request, CancellationToken cancellationToken)
        {
            var bill = await _tdsPaymentChallanRepository.GetTDSPaymentChallanById(request.Ids);
            return bill;
        }
    }

}



