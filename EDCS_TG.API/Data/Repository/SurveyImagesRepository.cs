using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EDCS_TG.API.Data.Repository
{
    public class SurveyImagesRepository: Repository<SurveyImages>, ISurveyImagesRepository
    {
        KarmaniDbContext _karmaniDbContext;
        public SurveyImagesRepository(KarmaniDbContext karmaniDbContext):base(karmaniDbContext)
        {
            _karmaniDbContext = karmaniDbContext;
        }


        public async Task<SurveyImages> CreateImage(SurveyImages entity)
        {
            var obj = _karmaniDbContext.SurveyImages.FirstOrDefault(T => T.SurveyId == entity.SurveyId);
                var result = _karmaniDbContext.SurveyImages.Add(entity);
           _karmaniDbContext.SaveChanges();
          
            return entity;
        }

        public async Task<IEnumerable<SurveyImages>> GetImagesBySurveyIdrepo(string surveyId)
        {
            var res = _karmaniDbContext.SurveyImages.Where(T => T.SurveyId == surveyId);
            return res;
        }




    }
}
