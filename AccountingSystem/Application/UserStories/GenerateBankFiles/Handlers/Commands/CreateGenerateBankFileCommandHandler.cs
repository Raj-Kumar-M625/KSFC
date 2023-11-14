using Application.Interface.Persistence.GenerateBankFiles;
using Application.Interface.Persistence.Master;
using Application.Interface.Persistence.Payment;
using Application.UserStories.GenerateBankFiles.Requests.Commands;
using Application.UserStories.Payment.Requests.Commands;
using AutoMapper;
using Domain.GenarteBankfile;
using Domain.Payment;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Handlers.Commands
{
    public class CreateGenerateBankFileCommandHandler:IRequestHandler<CreateGenerateBankFileCommand , int>
    {
        private readonly IGenerateBankFileRepository _generateBankFile;
        private readonly IBankMasterRepository _bankMasterrepo;
        private readonly IMapper _mapper;
        public CreateGenerateBankFileCommandHandler(IGenerateBankFileRepository generateBankFile, IMapper mapper, IBankMasterRepository bankmasterRepo)
        {
            _generateBankFile = generateBankFile;
            _mapper = mapper;
            _bankMasterrepo = bankmasterRepo;
            
        }

        public async Task<int> Handle(CreateGenerateBankFileCommand request, CancellationToken cancellationToken)
        {
          
            var list = _mapper.Map<GenerateBankFile>(request.GenerateBankFileDto);
            var result= await _generateBankFile.AddGeneratedBankFile(list);
            return result.Id;
            
        }
    }
}
