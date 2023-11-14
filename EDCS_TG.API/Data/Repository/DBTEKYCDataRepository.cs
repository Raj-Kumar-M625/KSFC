using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
namespace EDCS_TG.API.Data.Repository
{
    public class DBTEKYCDataRepository : Repository<DBTEKYCData>, IDBTEKYCDataRepository
    {
        public DBTEKYCDataRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }
    }
}
