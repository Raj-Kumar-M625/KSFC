using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class DBTEKYCRequestRepository : Repository<DBTEKYCRequest>,IDBTEKYCRequestRepository
    {
        public DBTEKYCRequestRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }
    }
}
