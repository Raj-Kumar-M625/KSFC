using EDCS_TG.API.Data.Models;
using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IPersonalDetailsServices
    {
        Task<IEnumerable<PersonalDetailsDto>> GetPersonalDetailsList();
    }
}
