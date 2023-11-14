using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class OfficeRepository : Repository<Office>,IOfficeRepository
    {
        public OfficeRepository(KarmaniDbContext karmaniDbContext):base(karmaniDbContext)
        {

        }
    }
}
