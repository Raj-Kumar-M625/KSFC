using AutoMapper;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class EmploymentService:IEmploymentService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public EmploymentService(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<EmploymentDto>> GetEmploymentDetails()
        {
            var result = await _repository.EmploymentRepository.FindAll();
            var resultData = _mapper.Map<IEnumerable<EmploymentDto>>(result);

            return resultData;
        }
    }
}
