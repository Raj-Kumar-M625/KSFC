using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Data.Models.DbModels;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface
{
    public interface IMobileService
    {
        public Task<RegdUserDTO> IsMobileNumberAlreadyExist(string mobileNum, CancellationToken token);

        public Task<OtpTab> IsGeneratedOtpExpired(string mobileNum,CancellationToken token);

        public Task SaveOtp(string otp, string mobileNum, bool verStatus, string process, CancellationToken token);

        public Task<bool> IsOtpVerSuccessfull(string otp, string mobileNum,string process, CancellationToken token);

        public Task<OtpTab> PostSuccessDeleteRec(string mobile, string process, CancellationToken token);
        public Task<string> FetchMobileByPan(string panNum, CancellationToken token);
    }
}
