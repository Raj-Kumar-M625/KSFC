using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.SMS;

using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.External.Interface
{
    public interface ISmsService
    {       
        public Task<SmsInfoDTO> SendSMS(string mobileNo, string process);
        public Task<string> SendSms(SmsDataModel sdm);
    }
}
