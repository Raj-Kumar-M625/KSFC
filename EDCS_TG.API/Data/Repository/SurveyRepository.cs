using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class SurveyRepository: Repository<Survey>, ISurveyRepository
    {
        public SurveyRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }


    }
}
