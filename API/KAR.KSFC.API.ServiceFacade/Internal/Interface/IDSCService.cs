using KAR.KSFC.Components.Data.Models.DbModels;

using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface
{
    public interface IDscService
    {
        public Task<TblEmpdscTab> AuthenticateDSC(string pubKey, string empId, CancellationToken token);
    }
}
