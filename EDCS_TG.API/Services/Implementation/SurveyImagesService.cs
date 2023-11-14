using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.Services.Interfaces;

namespace EDCS_TG.API.Services.Implementation
{
    public class SurveyImagesService : ISurveyImagesService
    {
        private readonly IUnitOfWork _repository;

        public SurveyImagesService(IUnitOfWork repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SurveyImages>> addSurveyImage(IEnumerable<SurveyImages> surveyImages)
        {
           
     
            foreach (SurveyImages image in surveyImages)
            {

                var  result = _repository.SurveyImagesRepository.CreateImage(image);
            }
                return surveyImages; 
        }

        public Task<IEnumerable<SurveyImages>> GetAllImages()
        {
            var result = _repository.SurveyImagesRepository.FindAll();
            return result;
        }

        public Task<IEnumerable<SurveyImages>> GetImagesBySurveyId(string surveyId)
        {
            var result = _repository.SurveyImagesRepository.GetImagesBySurveyIdrepo(surveyId);
            
            return result;
        }
    }
}
