using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IEmploymentService
    {
        Task<IEnumerable<EmploymentDto>> GetEmploymentDetails();
    }
}
