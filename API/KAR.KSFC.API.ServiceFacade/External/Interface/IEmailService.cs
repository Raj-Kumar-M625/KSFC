using KAR.KSFC.API.ServiceFacade.External.Service;
using KAR.KSFC.Components.Common.Dto.Email;

using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.External.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailServiceRequest mailRequest);
    }

}
