using EDCS_TG.API.DTO.DBT;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface IDBTService
    {
        Task<string> EKyc(string name, int SurveyeeId);
        Task<KYCResponse> KYCResponse(KYCResponse kYCResponse);
        Task<int> GetKYCResponse(string vaultrefno);
    }
}
