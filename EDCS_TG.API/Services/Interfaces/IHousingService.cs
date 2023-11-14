using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IHousingService
    {
        Task<IEnumerable<HousingDto>> GetHousingDetailsList();
    }
}
