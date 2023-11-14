using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class KutumbaDLRepository : Repository<KutumbaDL>, IKutumbaDLRepository
    {
        public KutumbaDLRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {
        }
    }
}
