using AutoMapper;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class HousingService:IHousingService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public HousingService(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<HousingDto>> GetHousingDetailsList()
        {
            var result = await _repository.HousingRepository.FindAll();
            var resultData = _mapper.Map<IEnumerable<HousingDto>>(result);

            return resultData;
        }

    }
}
