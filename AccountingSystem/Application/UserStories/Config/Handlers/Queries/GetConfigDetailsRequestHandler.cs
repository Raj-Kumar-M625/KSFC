using Application.DTOs.Configuration;
using Application.Interface.Persistence.Configuration;
using Application.UserStories.Config.Request.Queries;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UserStories.Config.Handlers.Queries
{
    internal class GetConfigDetailsRequestHandler : IRequestHandler<GetConfigDetailsRequest, ConfigDto>
    {
        private readonly IConfigRepository _configRepository;
        private readonly IMapper _mapper;

        public GetConfigDetailsRequestHandler(IConfigRepository configRepository, IMapper mapper)
        {
            _configRepository = configRepository;
            _mapper = mapper;
        }
        public async Task<ConfigDto> Handle(GetConfigDetailsRequest request, CancellationToken cancellationToken)
        {
            var config = await _configRepository.Get(request.Id);
            var configDetails = _mapper.Map<ConfigDto>(config);
            return configDetails;
        }
    }
}
