using AutoMapper;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class SocialSecurityService:ISocialSecurityService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public SocialSecurityService(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SocialSecurityDto>> GetSocialSecurityDetailsList()
        {
            var result = await _repository.SocialSecurityRepository.FindAll();
            var resultData = _mapper.Map<IEnumerable<SocialSecurityDto>>(result);

            return resultData;
        }
    }
}
