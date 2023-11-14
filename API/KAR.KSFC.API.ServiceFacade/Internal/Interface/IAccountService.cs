using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Data.Models.DbModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface
{
    public interface IAccountService
    {
        public Task<bool> SaveUserTryAction(string verDetails, string verType, string verStatus, int regId,string Process, CancellationToken token);
        public Task<int> SaveRegistration(RegduserTab registrationDetails, CancellationToken token);
        public Task<bool> IsIpUnderKSWAN(string ip, CancellationToken token);
        public Task<TblEmpdscTab> AdminLogin(string empId, string password, CancellationToken token);
        public Task<bool> IsCustAlreadyActive(string panNum, CancellationToken token);
        public Task<string> GetUserMobile(string panNum, CancellationToken token);
        public Task<bool> IsAdmAlreadyActive(string EmployeeId, CancellationToken token);
        public Task<TblEmpdscTab> KsfcUserForgotPassword(string empId, CancellationToken token);
        public Task<bool> UpdateNewpassword(string empId, string newPassword, CancellationToken token);
        public Task<bool> AdminLogout(string empId, CancellationToken token);
        public Task<TblEmpdscTab> EmpPasswordChange(PasswordChangeDTO info, CancellationToken token);
        public Task<bool> CustomerLogout(string panNum, CancellationToken token);
        public Task<List<string>> GetAssignedRoles(string empId, CancellationToken token);
        public Task<IEnumerable<TblUnitDet>> GetAccountDetails(CancellationToken token);
    }
}
