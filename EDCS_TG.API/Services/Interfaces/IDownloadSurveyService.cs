using EDCS_TG.API.Data.Models;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IDownloadSurveyService
    {
        Task<IEnumerable<DownloadSurveyModel>> GetAllDownload();
        Task<IEnumerable<DownloadSurveyModel>> GetIndividualSurveyDownload(string SurveyId);
        Task<IEnumerable<DownloadSurveyModel>> FilterSurveyDownload(SearchFilter filter);
    }
}
