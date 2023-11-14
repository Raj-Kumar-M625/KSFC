using Application.Models.Identity;
using Domain.User;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<OtpResponse> SendOtp(OtpRequest optRequest);
        Task<bool> Login(AuthRequest authRequest);
        Task<RegistrationResponse> Register(RegistrationRequest registrationRequest);
        IQueryable<UsersList> GetUsersLists();

    }
}
