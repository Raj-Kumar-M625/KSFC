using AutoMapper;

using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Repository.UoW;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountService : IAccountService
    {

        private readonly IEntityRepository<NuserhistoryTab> _smsTrackRepository;
        private readonly IEntityRepository<RegduserTab> _regdUserRepository;
        private readonly IEntityRepository<TblEmpdscTab> _empRepository;
        private readonly IEntityRepository<PromsessionTab> _custSessionRepository;
        private readonly IEntityRepository<EmpsessionTab> _empSessionRepository;
        private readonly IEntityRepository<IpCdtab> _ipRepository;
        private readonly IEntityRepository<TblUnitMast> _TblUnitMast;
        private readonly IEntityRepository<TblAppLoanMast> _TblAppLoanMast;
        private readonly UserInfo _userInfo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _work;
        /// <summary>
        /// custome parameters
        /// </summary>
        /// <param name="constituencyRepo"></param>
        /// <param name="empRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="custSessionRepository"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="smsTrackRepository"></param>
        /// <param name="regdUserRepository"></param>
        /// <param name="empSessionRepository"></param>
        /// <param name="ipRepository"></param>
        public AccountService(IEntityRepository<TblEmpdscTab> empRepository, IMapper mapper, IEntityRepository<PromsessionTab> custSessionRepository,
           IEntityRepository<NuserhistoryTab> smsTrackRepository, IEntityRepository<RegduserTab> regdUserRepository, IEntityRepository<EmpsessionTab> empSessionRepository,
            IEntityRepository<IpCdtab> ipRepository, UserInfo userInfo, IUnitOfWork work, IEntityRepository<TblUnitMast> tblUnitMast, IEntityRepository<TblAppLoanMast> tblAppLoanMast)
        {

            _smsTrackRepository = smsTrackRepository;
            _regdUserRepository = regdUserRepository;
            _empRepository = empRepository;
            _custSessionRepository = custSessionRepository;
            _empSessionRepository = empSessionRepository;
            _ipRepository = ipRepository;
            _userInfo = userInfo;
            _mapper = mapper;
            _work = work;
            _TblUnitMast = tblUnitMast;
            _TblAppLoanMast = tblAppLoanMast;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verDetails"></param>
        /// <param name="verType"></param>
        /// <param name="status"></param>
        /// <param name="regId"></param>
        /// <param name="Process"></param>
        public async Task<bool> SaveUserTryAction(string verDetails, string verType, string status, int regId, string Process, CancellationToken token)
        {
            NuserhistoryTab nUserTrack = new()
            {
                VerType = verType,
                VerDetails = verDetails,
                VerStatus = status,
                VerDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = _userInfo.UserId,
                CreatedDate = DateTime.UtcNow,
                Process = Process
            };
            await _smsTrackRepository.AddAsync(nUserTrack, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<int> SaveRegistration(RegduserTab registrationDetails, CancellationToken token)
        {
            registrationDetails.UserRegnDate = DateTime.Now;
            registrationDetails.IsActive = true;
            registrationDetails.IsDeleted = false;
            registrationDetails.CreatedBy = _userInfo.UserId;
            registrationDetails.CreatedDate = DateTime.UtcNow;

            var data = await _regdUserRepository.AddAsync(registrationDetails, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return (int)data.UserRowId;
        }

        public async Task<bool> IsIpUnderKSWAN(string ip, CancellationToken token)
        {

            var ipDet = await _ipRepository.FirstOrDefaultByExpressionAsync(x => x.Ip == ip && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            if (ipDet != null)
            {
                return true;
            }

            return false;
        }

        public async Task<TblEmpdscTab> AdminLogin(string empId, string password, CancellationToken token)
        {

            return await _empRepository.FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EmpId == empId && x.EmpPswd == password && x.IsActive == true && x.IsDeleted == false, include => include.Emp).ConfigureAwait(false);

        }

        public async Task<List<string>> GetAssignedRoles(string empId, CancellationToken token)
        {

            return new List<string> { "Admin", "Appraisal", "EG", "Legal Officer", "Accounting Officer" };

        }

        public async Task<bool> IsCustAlreadyActive(string panNum, CancellationToken token)
        {

            var activeUser = await _custSessionRepository.FirstOrDefaultByExpressionAsync(x => x.PromPan == panNum && x.SessionStatus == true && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            if (activeUser != null)
            {
                if (activeUser.Accesstokenexpirydatetime > DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;

        }

        public async Task<bool> IsAdmAlreadyActive(string EmployeeId, CancellationToken token)
        {

            var user = await _empSessionRepository.FirstOrDefaultByExpressionAsync(x => x.EmpId == EmployeeId && x.SessionStatus == true && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            if (user != null)
            {
                if (user.Accesstokenexpirydatetime > DateTime.Now)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;

        }

        public async Task<TblEmpdscTab> KsfcUserForgotPassword(string empId, CancellationToken token)
        {

            var user = await _empRepository.FindByFirstOrDefalutMatchingPropertiesAsync(token, x => x.EmpId == empId && x.IsActive == true && x.IsDeleted == false, include => include.Emp).ConfigureAwait(false);

            return user;

        }

        public async Task<bool> UpdateNewpassword(string empId, string newPassword, CancellationToken token)
        {

            var user = await _empRepository.FirstOrDefaultByExpressionAsync(x => x.EmpId == empId && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            if (user != null)
            {
                user.EmpPswd = newPassword;
                user.IsPswdChng = true;
                user.ModifiedBy = _userInfo.UserId;
                user.ModifiedDate = DateTime.UtcNow;
            }
            await _empRepository.UpdateAsync(user, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> CustomerLogout(string panNum, CancellationToken token)
        {

            var item = await _custSessionRepository.FirstOrDefaultByExpressionAsync(x => x.PromPan == panNum, token).ConfigureAwait(false);
            if (item != null)
            {
                item.LogoutDateTime = DateTime.Now;
                item.SessionStatus = false;
            }
            await _custSessionRepository.UpdateAsync(item, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> AdminLogout(string empId, CancellationToken token)
        {

            var user = await _empSessionRepository.FirstOrDefaultByExpressionAsync(x => x.EmpId == empId, token).ConfigureAwait(false);

            if (user != null)
            {
                user.SessionStatus = false;
                user.LogoutDateTime = DateTime.Now;
            }
            await _empSessionRepository.UpdateAsync(user, token).ConfigureAwait(false);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<TblEmpdscTab> EmpPasswordChange(PasswordChangeDTO info, CancellationToken token)
        {

            var item = await _empRepository.FirstOrDefaultByExpressionAsync(x => x.EmpId == info.EmpId && x.EmpPswd == info.OldPassword, token).ConfigureAwait(false);

            if (item != null)
            {
                item.EmpPswd = info.NewPassword;
                item.IsPswdChng = false;
                item.ModifiedBy = _userInfo.UserId;
                item.ModifiedDate = DateTime.UtcNow;
                await _empRepository.UpdateAsync(item, token).ConfigureAwait(false);
                await _work.CommitAsync(token).ConfigureAwait(false);

            }
            return item;

        }

        /// <summary>
        /// Get Mobile Number
        /// </summary>
        /// <param name="panNum"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string> GetUserMobile(string panNum, CancellationToken token)
        {
            var data = await _regdUserRepository.FirstOrDefaultByExpressionAsync(x => x.UserPan == panNum && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (data != null)
            {
                return data.UserMobileno;
            }
            return String.Empty;
        }


        public async Task<IEnumerable<TblUnitDet>> GetAccountDetails(CancellationToken token)
        {
            var tblunit = await _TblUnitMast.FindByExpressionAsync(x => x.UtName != null, token);
            var tblloan = await _TblAppLoanMast.FindByExpressionAsync(x => x.InMastId != null, token);

            var result = tblunit.Join(tblloan,
                unitmast => unitmast.UtCd,
                loanmast => loanmast.InUnit,
                (unitmast, loanmast) => new TblUnitDet
                {
                    ut_cd = unitmast.UtCd,
                    ln_no = loanmast.InNo,
                    ut_name = unitmast.UtName
                }).ToList();

            return result;

        }
    }
}
