using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class UserAssignmentRepository: Repository<UserAssignment>, IUserAssignmentREpository
    {
        public UserAssignmentRepository(KarmaniDbContext karmanidbcontex) : base(karmanidbcontex)
        {


        }
    }
}
