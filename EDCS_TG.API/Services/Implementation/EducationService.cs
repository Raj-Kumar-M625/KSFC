using AutoMapper;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class EducationService:IEducationService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public EducationService(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;   
        }
        public async Task<IEnumerable<EducationDto>> GetEducationDetailsList()
        {
            var result = await _repository.EducationRepository.FindAll();
            var resultData = _mapper.Map<IEnumerable<EducationDto>>(result);

            return resultData;
        }

    }
}
