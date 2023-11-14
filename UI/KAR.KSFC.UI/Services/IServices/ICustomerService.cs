using KAR.KSFC.UI.Models;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices
{
    public interface ICustomerService
    {
        Task<CustClaimsModel> Login(CustomerViewModel customerViewModel);

        Task<bool> Logout(string panNo, string accToken);
    }
}
