using EDCS_TG.API.DTO;

namespace EDCS_TG.API.Services.Interfaces
{
    public interface ISocialSecurityService
    {
        Task<IEnumerable<SocialSecurityDto>> GetSocialSecurityDetailsList();
    }
}
