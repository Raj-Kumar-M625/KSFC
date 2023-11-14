using AutoMapper;
using Microsoft.AspNetCore.Identity;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;

namespace SRS.API.Services.Implementations
{
    public class DownloadService : IDownloadService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _repository;
        private readonly IOfficeService _officeService;
 


        public DownloadService(IOfficeService officeService, IUnitOfWork repository, IMapper mapper
            )
        {
            _officeService = officeService;
            _repository = repository;
            _mapper = mapper;
           
        }

     

        public async Task<IEnumerable<QuestionPaperDto>> GetSurveys()
        {
            //var qps = await _repository.QuestionPaper.FindByCondition(x => x.IsActive == true, y => y.QuestionPaperQuestions);

            var qps = await _repository.QuestionPaper.GetAllQuestionPapers();
            var qpsDto = _mapper.Map<IEnumerable<QuestionPaperDto>>(qps);
            return qpsDto;


            //var qpqs = await _repository.QuestionPaperQuestion.FindByCondition(x => x.QuestionPaperId.Equals(qps.Select(y => y.Id).Distinct()))
            //throw new NotImplementedException();
        }
    }
}
