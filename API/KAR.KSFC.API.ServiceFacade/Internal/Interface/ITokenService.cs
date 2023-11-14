using KAR.KSFC.Components.Common.Dto;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public Task<TokenDTO> Refresh(TokenDTO tokenModel, CancellationToken token);
        public Task<TokenDTO> PromoterRefresh(TokenDTO tokenModel, CancellationToken token);
    }
}
