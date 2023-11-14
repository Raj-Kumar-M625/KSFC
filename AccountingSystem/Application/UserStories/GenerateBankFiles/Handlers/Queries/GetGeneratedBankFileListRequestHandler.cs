using Application.DTOs.GenerateBankFile;
using Application.Interface.Persistence.GenerateBankFiles;
using Application.UserStories.GenerateBankFiles.Requests.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.GenarteBankfile;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.GenerateBankFiles.Handlers.Queries
{
    public class GetGeneratedBankFileListRequestHandler :IRequestHandler<GetGeneratedBqankFileListRequest, IQueryable<GenerateBankFile>>
    {
        private readonly IGenerateBankFileRepository _generateBankFile;
        private readonly IMapper _mapper;
        public GetGeneratedBankFileListRequestHandler(IGenerateBankFileRepository generateBankFile, IMapper mapper)
        {
            _generateBankFile = generateBankFile;
            _mapper = mapper;
        }

        public async Task<IQueryable<GenerateBankFile>> Handle(GetGeneratedBqankFileListRequest request, CancellationToken cancellationToken)
        {
            IQueryable<GenerateBankFile> list = _generateBankFile.GetAllGenerateBankFile();


            return   list;

        }
    }
}
