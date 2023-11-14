using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices
{
    public interface IOtpService
    {
        bool GetOtpAttempts();
        Task<JsonResult> Generate(string process, string mobileNo, string panNo, string empId);
        Task<JsonResult> Resend(string process, string mobileNo, string panNo, string empId);
        Task<JsonResult> Validate(string process, string mobileNo, string otp);
        bool GetOtpAttemptsCustomer();
        bool GetOtpAttemptsFP();
    }
}
