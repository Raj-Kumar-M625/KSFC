using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class PersonalDetailsRepository:Repository<PersonalDetails>, IPersonalDetailsRepository
    {
       public PersonalDetailsRepository(KarmaniDbContext karmanidbcontex):base(karmanidbcontex)
        {
            

        }

        
    }
}
