using AutoMapper;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class HealthService:IHealthService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public HealthService(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<HealthDto>> GetHealthDetailsList()
        {
            var result = await _repository.HealthRepository.FindAll();
            var resultData = _mapper.Map<IEnumerable<HealthDto>>(result);

            return resultData;
        }

    }
}
