using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class DBTEKYCResponseRepository : Repository<DBTEKYCResponse>,IDBTEKYCResponseRepository
    {
        public DBTEKYCResponseRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }
    }
}
