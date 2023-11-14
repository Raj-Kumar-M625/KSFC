using AutoMapper;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class AdditionalInformationService:IAdditionalInformationService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public AdditionalInformationService(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AdditionalInformationDto>> GetAdditionalInformationList()
        {
            try
            {
                var result = await _repository.AdditionalInformationRepository.FindAll();
                var resultData = _mapper.Map<IEnumerable<AdditionalInformationDto>>(result);

                return resultData;
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
    }
}
