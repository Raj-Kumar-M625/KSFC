using Application.Interface.Persistence.BankFile;
using Application.Interface.Persistence.GenerateBankFiles;
using Application.UserStories.BankFile.Requests.Commands;
using AutoMapper;
using Domain.BankFile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.BankFile.Handlers.Commands
{
    public class AddBankUtrDetailsCommandHandler : IRequestHandler<AddBankUtrDetailsCommand, int>
    {
        private readonly IBankFileRepository _bankFileRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public AddBankUtrDetailsCommandHandler(IBankFileRepository bankFileRepository, IMapper mapper, IMediator mediator)
        {
            _bankFileRepository = bankFileRepository;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<int> Handle(AddBankUtrDetailsCommand request, CancellationToken cancellationToken)
        {
            var list = _mapper.Map<BankFileUtrDetails>(request.bankFileDetails);
            var result = await _bankFileRepository.AddBankUTRDetails(list);
            return result.Id;
        }
    }
}
