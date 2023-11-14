using EDCS_TG.API.Data.Models;
using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface ISurveyImagesService
    {
        Task<IEnumerable<SurveyImages>> GetAllImages();
        Task<IEnumerable<SurveyImages>> GetImagesBySurveyId(string surveyId);

        Task<IEnumerable<SurveyImages>> addSurveyImage(IEnumerable<SurveyImages> surveyImages);

    }
}
