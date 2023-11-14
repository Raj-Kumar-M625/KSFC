using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices
{
    public interface IDscService
    {
        bool GetDscAttempts();

        Task<bool> IsDscRequired(string ip);
        Task<JsonResult> VerifyDsc(string empId, string publicKey);
    }
}
