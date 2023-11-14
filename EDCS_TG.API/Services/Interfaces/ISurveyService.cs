using EDCS_TG.API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface ISurveyService
    {
        Task<IEnumerable<SurveyDto>> GetSurveyDetailsList();

        Task<List<SurveyDto>> SaveSurveyDetail(IEnumerable<SurveyDto> surveyDto,string SurveyId);

        Task<IEnumerable<SurveyDto>> UpdateSurveyDetail(IEnumerable<SurveyDto> surveyDto,string SurveyId);
    }
}
