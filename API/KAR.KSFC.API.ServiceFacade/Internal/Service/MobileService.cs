using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Data.Repository.UoW;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service
{
    public class MobileService : IMobileService
    {

        private readonly IEntityRepository<RegduserTab> _userRepository;
        private readonly IEntityRepository<OtpTab> _smsTrackRepository;
        private readonly IEntityRepository<Promoter> _promotorRepository;
        private readonly IConfiguration _configure;
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        private readonly IUnitOfWork _work;
        public MobileService(IEntityRepository<RegduserTab> userRepository,
            IEntityRepository<OtpTab> smsTrackRepository,
            IEntityRepository<Promoter> promotorRepository,
            IConfiguration configure,
            UserInfo userInfo,
            IMapper mapper = null, IUnitOfWork work = null)
        {
            _userRepository = userRepository;
            _promotorRepository = promotorRepository;
            _smsTrackRepository = smsTrackRepository;
            _userInfo = userInfo;
            _configure = configure;
            _mapper = mapper;
            _work = work;
        }

        public async Task<RegdUserDTO> IsMobileNumberAlreadyExist(string mobileNum, CancellationToken token)
        {
            var user = await _userRepository.FirstOrDefaultByExpressionAsync(x => x.UserMobileno == mobileNum && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            var promotor = await _promotorRepository.FirstOrDefaultByExpressionAsync(x => x.PromMobile == mobileNum && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);

            if (user == null && promotor == null)
            {
                return null;
            }

            if (user != null)
            {
                var user_details = await _userRepository.FirstOrDefaultByExpressionAsync(x => x.UserPan == user.UserPan && x.UserMobileno == user.UserMobileno && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
                return _mapper.Map<RegdUserDTO>(user_details);
            }

            var Prom_Details = _mapper.Map<RegdUserDTO>(promotor);
            var pm_details = _promotorRepository.FirstOrDefaultByExpressionAsync(x => x.PanNo == Prom_Details.Pan && x.PromMobile == Prom_Details.mobile && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return _mapper.Map<RegdUserDTO>(pm_details);
        }

        public async Task<OtpTab> IsGeneratedOtpExpired(string mobileNum, CancellationToken token)
        {
            var item = await _smsTrackRepository.FirstOrDefaultByExpressionAsync(x => x.MobileNo == mobileNum && x.Otpexpirationdatetime >= DateTime.Now && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return item;
        }

        public async Task SaveOtp(string otp, string mobileNum, bool verStatus, string process, CancellationToken token)
        {
            int expTime = Convert.ToInt32(_configure["SMS:OtpExpireTime"]);
            OtpTab otpDet = new()
            {
                MobileNo = mobileNum,
                Otp = otp,
                VerStatus = verStatus,
                Process = process,
                UserId = _userInfo?.UserId,
                IsActive = true,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                Otpexpirationdatetime = DateTime.Now.AddSeconds(expTime)
            };
            await _smsTrackRepository.AddAsync(otpDet, token).ConfigureAwait(false);
           await _work.CommitAsync(token).ConfigureAwait(false);
        }

        public async Task<bool> IsOtpVerSuccessfull(string otp, string mobileNum, string process, CancellationToken token)
        {
            var resp = await _smsTrackRepository.FirstOrDefaultByExpressionAsync(x => x.MobileNo == mobileNum && x.Otp == otp && x.Process == process && x.Otpexpirationdatetime >= DateTime.Now && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            if (resp != null)
            {
                return true;
            }
            return false;
        }

        public async Task<OtpTab> PostSuccessDeleteRec(string mobile, string process, CancellationToken token)
        {
            var item = await _smsTrackRepository.FirstOrDefaultByExpressionAsync(x => x.MobileNo == mobile && x.Process == process && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            item.IsDeleted = true;
            item.IsActive = false;
            item.ModifiedDate = DateTime.UtcNow;
            await _smsTrackRepository.UpdateAsync(item, token);
            await _work.CommitAsync(token).ConfigureAwait(false);
            return item;
        }

        public async Task<string> FetchMobileByPan(string panNum, CancellationToken token)
        {
            var data = await _userRepository.FirstOrDefaultByExpressionAsync(x => x.UserPan == panNum && x.IsActive == true && x.IsDeleted == false, token).ConfigureAwait(false);
            return data.UserMobileno;
        }
    }
}
