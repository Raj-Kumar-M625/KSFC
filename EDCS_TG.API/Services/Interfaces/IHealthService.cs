using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IHealthService
    {
        Task<IEnumerable<HealthDto>> GetHealthDetailsList();
    }
}
