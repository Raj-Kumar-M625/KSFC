using EDCS_TG.API.Data.Models;
using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IBasicSurveyDetailService
    {
        Task<IEnumerable<BasicSurveyDetail>> GetBasicSurveyDetailListByUser(Guid UserId);
       
        Task<IEnumerable<BasicSurveyDetail>> GetAllBasicSurveyDetailList();
        Task<IEnumerable<BasicSurveyDetailDto>> GetSurveyDetailByDistrict(string district);

        Task<IEnumerable<BasicSurveyDetailDto>> GetSurveyDetailByTaluk(string taluk);

        Task<IEnumerable<BasicSurveyDetailDto>> GetSurveyDetailByHobli(string hobli);

        Task<IEnumerable<BasicSurveyDetailDto>> GetSurveyDetailByVillage(string village);
    }
}
