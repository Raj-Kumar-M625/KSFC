using EDCS_TG.API.DTO;
using System.Threading.Tasks;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IAdditionalInformationService
    {
        Task<IEnumerable<AdditionalInformationDto>> GetAdditionalInformationList();
    }
}
