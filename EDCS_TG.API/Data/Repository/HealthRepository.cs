using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class HealthRepository: Repository<Health>,IHealthRepository
    {
        public HealthRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }
    }
}
