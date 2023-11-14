using KAR.KSFC.Components.Common.Dto;

using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.External.Interface
{
    public interface IPanService
    {
        public Task<RegdUserDTO> IsPanAlreadyExist(string panNo,CancellationToken token);
    }
}
