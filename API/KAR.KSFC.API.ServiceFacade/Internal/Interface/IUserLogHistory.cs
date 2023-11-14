using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface
{
    public interface IUserLogHistory
    {
        public Task<bool> UpdateCustomerLoginHistory(string IP, string panNum, string AccessToken, string RefreshToken, CancellationToken token);
        public Task<bool> UpdateAdminLoginHistory(string empId, string AccessToken, string RefreshToken, string ip, CancellationToken token);
        //bool UserLoggedInDifferentIP(string IP, Usermaster userModel);
        //bool DifferentIPLoggedIn(string IP, string userModel, string AccessToken, string RefreshToken);
        //long UpdateRefreshToken(string IP, string username, string AccessToken, string RefreshToken);

    }
}
