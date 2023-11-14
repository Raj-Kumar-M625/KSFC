using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices
{
    public interface IAdminService
    {
        Task<EmployeeClaimsModel> Login(EmployeeLoginDTO employeeLoginDTO);
        Task<JsonResult> ValidateUserSendPassword(string empId);
        Task<string> ChangePassword(string empId, string token, string currPassword, string newPassword);
        Task<bool> UserLogout(string empId, string adminAccToken);
       
    }
}
