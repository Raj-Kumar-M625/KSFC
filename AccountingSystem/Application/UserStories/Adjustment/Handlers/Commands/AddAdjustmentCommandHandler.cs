using Application.DTOs.Adjustment;
using Application.DTOs.Bill;
using Application.Interface.Persistence.Adjustment;
using Application.Interface.Persistence.Bill;
using Application.UserStories.Adjustment.Requests.Commands;
using Application.UserStories.Bill.Requests.Commands;
using AutoMapper;
using Domain.Adjustment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Adjustment.Handlers.Commands
{
    public class AddAdjustmentCommandHandler : IRequestHandler<AddAdjustmentCommadRequest, AdjustmentDto>
    {
        private readonly IAdjustmentRepository _adjustmentRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public AddAdjustmentCommandHandler(IAdjustmentRepository adjustmentRepository, IMapper mapper, IMediator mediator)
        {
            _adjustmentRepository = adjustmentRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<AdjustmentDto> Handle(AddAdjustmentCommadRequest request, CancellationToken cancellationToken)
        {
            Adjustments adjustments = new Adjustments();
            adjustments = _mapper.Map<Adjustments>(request.adjustmentDto);
            adjustments.CreatedBy = request.user;
            adjustments.CreatedOn = DateTime.Now;
            if(adjustments.AdjustmentType == "Bill")
            {                
                adjustments.Amount = adjustments.Amount >0 ? adjustments.Amount : adjustments .Amount * - 1;
            }
            else if(adjustments.AdjustmentType == "Payment")
            {
                adjustments.Amount = adjustments.Amount > 0 ? adjustments.Amount * -1:adjustments.Amount ;
            }
            else if(adjustments.AdjustmentType == "Adjustment")
            {
                adjustments.Amount = adjustments.Amount;
            }
            adjustments = await _adjustmentRepository.AddAdjustments(adjustments);
            AdjustmentDto dto = _mapper.Map<AdjustmentDto>(adjustments);
            return dto;
        }
    }
}
