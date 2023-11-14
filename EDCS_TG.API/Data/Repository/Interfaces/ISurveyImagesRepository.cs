using EDCS_TG.API.Data.Models;

namespace EDCS_TG.API.Data.Repository.Interfaces
{
    public interface ISurveyImagesRepository:IRepository<SurveyImages>
    {
     Task<SurveyImages> CreateImage(SurveyImages entity);
        Task<IEnumerable<SurveyImages>> GetImagesBySurveyIdrepo(string surveyId);

    }



}
