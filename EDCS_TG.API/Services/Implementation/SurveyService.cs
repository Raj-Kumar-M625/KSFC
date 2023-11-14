using AutoMapper;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;
using EDCS_TG.API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using EDCS_TG.API.Data;

namespace EDCS_TG.API.Services.Implementation
{
    public class SurveyService:ISurveyService
    {
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private readonly KarmaniDbContext _karmaniDbContext;

        public SurveyService(IUnitOfWork repository, IMapper mapper, KarmaniDbContext karmaniDbContext)
        {
            _repository = repository;
            _mapper = mapper;
            _karmaniDbContext = karmaniDbContext;
        }
        public async Task<IEnumerable<SurveyDto>> GetSurveyDetailsList()
        {
            var result = await _repository.surveyRepository.FindAll();
            var resultData = _mapper.Map<IEnumerable<SurveyDto>>(result);

            return resultData;
        }

        public async Task<List<SurveyDto>> SaveSurveyDetail(IEnumerable<SurveyDto> surveyDto,string surveyId)
        {
            List<SurveyDto> surveyList = new List<SurveyDto>();
            IEnumerable<Survey> survey;
            var surveyEntity = _mapper.Map<IEnumerable<Survey>>(surveyDto);

            foreach(var i in surveyEntity)
            {
                
                 survey = await _repository.surveyRepository.FindByCondition(t => t.SurveyId == surveyId);
                if (survey != null)
                {
                   var  question = survey.FirstOrDefault(t => t.QuestionId == i.QuestionId);

                    if (question == null)
                    {
                        var result = await _repository.surveyRepository.Create(i);
                        var surveydto = _mapper.Map<SurveyDto>(result);
                        surveyList.ToList().Add(surveydto);
                    }
                    else
                    {
                        question.Answer = i.Answer;
                    }
                }

                _repository.save();

            }
            return surveyList;

            
        }


        public async Task<IEnumerable<SurveyDto>> UpdateSurveyDetail(IEnumerable<SurveyDto> surveyDto, string SurveyId)
        {
            Expression<Func<Survey, bool>> expression = t => t.SurveyId == SurveyId;
            var survey = await _repository.surveyRepository.FindByCondition(expression);
            var updateList = _mapper.Map<IEnumerable<Survey>>(surveyDto);

            foreach (var s in updateList)
            {
                var result = survey.FirstOrDefault(e => e.QuestionId == s.QuestionId);
                if (result != null)
                {
                    result.Answer = s.Answer;

                }
                else
                {
                    var entity = await _repository.surveyRepository.Create(s);
                }

            }

            _repository.save();

            return surveyDto;
        }

    }
}
