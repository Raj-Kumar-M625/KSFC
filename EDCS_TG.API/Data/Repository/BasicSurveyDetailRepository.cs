using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class BasicSurveyDetailRepository:Repository<BasicSurveyDetail>,IBasicSurveylDetailRepository
    {
        public BasicSurveyDetailRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }
    }
}
