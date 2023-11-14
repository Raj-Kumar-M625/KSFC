using Application.Interface.Persistence.BankFile;
using Application.UserStories.BankFile.Requests.Commands;
using Application.UserStories.GenerateBankFiles.Requests.Queries;
using AutoMapper;
using Domain.BankFile;
using Domain.GenarteBankfile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.BankFile.Handlers.Commands
{
    public class AddMappingForBankFileCommandHandler : IRequestHandler<AddMappingForBankFileCommand, int>
    {
        private readonly IBankFileRepository _bankFileRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public AddMappingForBankFileCommandHandler(IBankFileRepository bankFileRepository, IMapper mapper, IMediator mediator)
        {
            _bankFileRepository = bankFileRepository;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<int> Handle(AddMappingForBankFileCommand request, CancellationToken cancellationToken)
        {
            MappingGenBankFileUtrDetails mappingGenBankFileUTRDetails = new MappingGenBankFileUtrDetails();
            List<MappingGenBankFileUtrDetails > mappingGenBankFileUTR = new List<MappingGenBankFileUtrDetails>();
          
            foreach(var id in request.GenerateBankFileID)
            {
                mappingGenBankFileUTRDetails.GenerateBankFileId = id;
                mappingGenBankFileUTRDetails.BankFileUTRId = request.BankFileId;
                mappingGenBankFileUTRDetails.ModifiedOn = DateTime.Now;
                mappingGenBankFileUTRDetails.CreatedOn = DateTime.Now;
                mappingGenBankFileUTRDetails.CreatedBy = request.CurrentUser;
                mappingGenBankFileUTRDetails.ModifedBy = request.CurrentUser;
                mappingGenBankFileUTR.Add(mappingGenBankFileUTRDetails);    
            }
             await _bankFileRepository.AddMappingForBankFile(mappingGenBankFileUTR);
            return request.BankFileId;
        }
    }
}
