using Application.Interface.Persistence.GenerateBankFiles;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.UserStories.GenerateBankFiles.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Handlers.Commands
{
    public class UpdateGenerateBankFileStatusCommandHandler : IRequestHandler<UpdateGenerateBankFileStatusCommand, int>
    {
        private readonly IGenerateBankFileRepository _generateBankFile;
        private readonly IMapper _mapper;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMediator _mediator;

        public UpdateGenerateBankFileStatusCommandHandler(IGenerateBankFileRepository generateBankFile, ICommonMasterRepository commonMaster, IMapper mapper, IMediator mediator)
        {
            _generateBankFile = generateBankFile;
            _mapper = mapper;
            _mediator = mediator;
            _commonMaster = commonMaster;
        }

        public async Task<int> Handle(UpdateGenerateBankFileStatusCommand request, CancellationToken cancellationToken)
        {
            var paymentStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.pStatus);

            var list = _generateBankFile.GetGenerateBankFileStatus(request.GeneratedBankFileID);
            foreach (var item in list)
            {
                item.StatusCMID = paymentStatusList.First(p => p.CodeValue == ValueMapping.paid).Id;
                item.ModifedBy = request.CurrentUser;
                item.ModifiedOn = DateTime.Now;
            }
            var result = await _generateBankFile.UpdateGeneratedBankFileStatus(list.ToList());
            return result.GenerateBankFileId;
        }
    }
}
