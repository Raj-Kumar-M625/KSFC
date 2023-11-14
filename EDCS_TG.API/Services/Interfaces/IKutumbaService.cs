using EDCS_TG.API.Dto;
using EDCS_TG.API.Helpers.Kutumba;
namespace EDCS_TG.API.Services.Interfaces
{
    public interface IKutumbaService
    {
        Task<IEnumerable<ResultDataList>> GetKutumbaData(KutumbaRequestDto requestDto);
        
    }
}
