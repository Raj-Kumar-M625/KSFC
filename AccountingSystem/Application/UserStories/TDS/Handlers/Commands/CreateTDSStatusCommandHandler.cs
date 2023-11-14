using Application.Interface.Persistence.Bill;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.TDS;
using Application.UserStories.TDS.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.Bill;
using Domain.TDS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.TDS.Handlers.Commands
{
  
    public class CreateTdsStatusCommandHandler : IRequestHandler<CreateTdsStatusCommand, long>
    {
        private readonly ITdsStatusRepository _tdsStatusRepository;
        private readonly ICommonMasterRepository _commonMasterRepository;
        private readonly IMapper _mapper;
        public CreateTdsStatusCommandHandler(ITdsStatusRepository tdsStatusRepository,ICommonMasterRepository commonMasterRepository, IMapper mapper)
        {
            _tdsStatusRepository = tdsStatusRepository;
            _commonMasterRepository = commonMasterRepository;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreateTdsStatusCommand request, CancellationToken cancellationToken)
        {
            var tdsStatus = new TdsStatus();
            tdsStatus.ID =request.billDetails.Id;
            tdsStatus.VendorId = request.billDetails.VendorId;
            tdsStatus.StatusCMID = request.statusId.Value;
            tdsStatus.CreatedBy = request.user;
            tdsStatus.CreatedOn = DateTime.UtcNow;
            tdsStatus.ModifedBy = request.user;
            tdsStatus.ModifiedOn = DateTime.UtcNow;

            var bill = await _tdsStatusRepository.Add(tdsStatus);
            return bill.ID;
        }
    }
}
