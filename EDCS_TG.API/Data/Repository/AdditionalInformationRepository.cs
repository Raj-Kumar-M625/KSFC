using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;

namespace EDCS_TG.API.Data.Repository
{
    public class AdditionalInformationRepository: Repository<AdditionalInformation>, IAdditionalInformationRepository
    {
        public AdditionalInformationRepository(KarmaniDbContext karmaniDbContext) : base(karmaniDbContext)
        {

        }
    }
}
