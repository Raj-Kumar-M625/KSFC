using AutoMapper;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class Questionservice:IQuestionService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;

        public Questionservice(IUnitOfWork repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<QuestionsDto>> GetQuestions()
        {
            var result = await _repository.questionRepository.FindAll();
            var resultData = _mapper.Map<IEnumerable<QuestionsDto>>(result);

            return resultData;
        }

    }
}
