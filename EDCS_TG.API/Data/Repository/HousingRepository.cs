using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class HousingRepository : Repository<Housing>, IHousingRepository
    {
        public HousingRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }
    }
}
