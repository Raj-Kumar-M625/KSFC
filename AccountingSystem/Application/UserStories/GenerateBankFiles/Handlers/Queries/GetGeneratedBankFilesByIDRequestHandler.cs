using Application.DTOs.GenerateBankFile;
using Application.Interface.Persistence.GenerateBankFiles;
using Application.Interface.Persistence.Master;
using Application.UserStories.GenerateBankFiles.Requests.Queries;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Handlers.Queries
{
    public class GetGeneratedBankFilesByIDRequestHandler:IRequestHandler<GetGeneratedBankFilesByIDRequest,List<PaymentGenerateBankFileDto>>
    {
        private readonly IGenerateBankFileRepository _generateBankFile;
        private readonly IMapper _mapper;
        private readonly IBankMasterRepository _bankMaster;
        public GetGeneratedBankFilesByIDRequestHandler(IGenerateBankFileRepository generateBankFile,IMapper mapper,IBankMasterRepository bankMaster)
        {
            _generateBankFile = generateBankFile;
            _mapper = mapper;
            _bankMaster = bankMaster;
        }

        public async Task<List<PaymentGenerateBankFileDto>> Handle(GetGeneratedBankFilesByIDRequest request,CancellationToken cancellationToken)
        {
            var res = await _generateBankFile.GetGeneratedBankFileById(request.Id);
            var banklist = await _bankMaster.GetBankMasterList();
          
                return _mapper.Map<List<PaymentGenerateBankFileDto>>(res);
        }
    }
}
