using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Mapper;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class PersonalDetailsService : IPersonalDetailsServices
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public PersonalDetailsService(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PersonalDetailsDto>> GetPersonalDetailsList()
        {
            var result = await _repository.PersonalDetailsRepository.FindAll();
            var resultData = _mapper.Map<IEnumerable<PersonalDetailsDto>>(result);

            return resultData;
        }
    }
}
