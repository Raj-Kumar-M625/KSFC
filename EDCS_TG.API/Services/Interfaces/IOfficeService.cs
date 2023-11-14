using EDCS_TG.API.Data.Models;
using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IOfficeService
    {
        Task<IList<Office>> getAllOffficeList(string StateCode);

        Task<IList<Office>> GetTalukList(string DistrictCode);

        Task<IList<Office>> GetHobliList(string TalukCode);

        Task<IList<Office>> GetVillageList(string HobliCode);

    }
}
