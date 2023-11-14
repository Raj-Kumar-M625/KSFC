using Application.Interface.Persistence.Adjustment;
using Application.Interface.Persistence.Bill;
using Application.UserStories.Adjustment.Requests.Queries;
using Application.UserStories.Payment.Requests.Queries;
using AutoMapper;
using Domain.Adjustment;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Adjustment.Handlers.Queries
{
    public class GetAdjustmentListRequestHandler : IRequestHandler<GetAdjustmentListRequest, IQueryable<Adjustments>>
    {
        private readonly IAdjustmentRepository _adjustmentRepository;
        private readonly IBillRepository _billRepository;
        private readonly IMapper _mapper;

        public GetAdjustmentListRequestHandler(IAdjustmentRepository adjustmentRepository, IMapper mapper,IBillRepository billRepository)
        {
            _adjustmentRepository = adjustmentRepository;
            _billRepository = billRepository;
            _mapper = mapper;
        }
        public async Task<IQueryable<Adjustments>> Handle(GetAdjustmentListRequest request, CancellationToken cancellationToken)
        {
            //var payments = _adjustmentRepository.GetAdjustmentList(request.VendorId);
            var res = _billRepository.GetAdjustmentList(request.VendorId);
            return res;
        }
    }
}
