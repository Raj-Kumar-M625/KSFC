using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class DBTLocalEKYCDataRepository : Repository<DBTLocalEKYCData>,IDBTLocalEKYCDataRepository
    {
        public DBTLocalEKYCDataRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }
    }
}
