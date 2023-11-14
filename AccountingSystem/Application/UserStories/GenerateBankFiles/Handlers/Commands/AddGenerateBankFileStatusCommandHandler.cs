using Application.Interface.Persistence.GenerateBankFiles;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.UserStories.GenerateBankFiles.Requests.Commands;
using AutoMapper;
using Common.ConstantVariables;
using Domain.GenerateBankfile;
using Domain.Master;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Handlers.Commands
{
    public class AddGenerateBankFileStatusCommandHandler : IRequestHandler<AddGeneratteBankFileStatusCommand, int>
    {
        private readonly IGenerateBankFileRepository _generateBankFile;
        private readonly IMapper _mapper;
        private readonly ICommonMasterRepository _commonMaster;
        private readonly IMediator _mediator;
        private readonly IPaymentRepository _vendorPaymentRepository;
        public AddGenerateBankFileStatusCommandHandler(IGenerateBankFileRepository generateBankFile, ICommonMasterRepository commonMaster,IPaymentRepository vendorPaymentRepository, IMapper mapper, IMediator mediator)
        {
            _generateBankFile = generateBankFile;
            _mapper = mapper;
            _mediator = mediator;
            _vendorPaymentRepository = vendorPaymentRepository;
            _commonMaster = commonMaster;
        }

        public async Task<int> Handle(AddGeneratteBankFileStatusCommand request, CancellationToken cancellationToken)
        {
            var paymentStatusList = await _commonMaster.GetCommoMasterValues(ValueMapping.pStatus);

            var generateBankFileStatus = new GenerateBankFileStatus();
            generateBankFileStatus.StatusCMID= paymentStatusList.First(p => p.CodeValue == ValueMapping.pbankFile).Id;
            generateBankFileStatus.CreatedOn = DateTime.Now;
            generateBankFileStatus.CreatedBy = request.CurrentUser;
            generateBankFileStatus.GenerateBankFileId = request.GeneratedBankID;
            generateBankFileStatus.ModifiedOn = DateTime.Now;
            generateBankFileStatus.ModifedBy = request.CurrentUser;

            var list = await _generateBankFile.AddGeneratedBankFileStatus(generateBankFileStatus);
            return list.GenerateBankFileId;
        }
    }
}
