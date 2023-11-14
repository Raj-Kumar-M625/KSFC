using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class EmploymentRepository:Repository<Employment>, IEmploymentRepository
    {
        public EmploymentRepository(KarmaniDbContext karmaniDbContext):base(karmaniDbContext)
        {

        }
    }
}
