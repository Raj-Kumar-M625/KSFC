using Application.Interface.Persistence.Adjustment;
using Application.Interface.Persistence.Master;
using Application.UserStories.Adjustment.Requests.Commands;
using Application.UserStories.Payment.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Adjustment;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Adjustment.Handlers.Commands
{
    public class AddAdjustmentStatusCommandHandler : IRequestHandler<AddAdjustmentStatusCommandRequest, int>
    {
        private readonly IAdjustmentRepository _adjustmentRepository;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMapper _mapper;
        public AddAdjustmentStatusCommandHandler(IAdjustmentRepository adjustmentRepository, ICommonMasterRepository commonMaster, IMapper mapper)
        {
            _adjustmentRepository = adjustmentRepository;
            _commonMaster = commonMaster;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddAdjustmentStatusCommandRequest request, CancellationToken cancellationToken)
        {
            var adjustment = await _adjustmentRepository.GetAdjustmentsById(request.AdjustmentId);
            var adjustmentStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.pStatus);
            AdjustmentStatus adjustmentStatus = new()
            {
                AdjustmentID = request.AdjustmentId,
                StatusCMID = adjustmentStatusList.First(p => p.CodeValue == ValueMapping.pending).Id,
                VendorID = adjustment.VendorID,
                CreatedBy = adjustment.CreatedBy,
                CreatedOn = DateTime.Now,
            };
            await _adjustmentRepository.AddAdjustmentStatus(adjustmentStatus);
            return request.AdjustmentId;
        }
    }
}
