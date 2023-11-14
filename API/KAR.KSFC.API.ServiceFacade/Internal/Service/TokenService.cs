using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KAR.KSFC.Components.Data.Repository.UoW;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service
{
    class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IEntityRepository<EmpsessionTab> _empSession;
        private readonly IEntityRepository<PromsessionTab> _promSession;
        private readonly IUnitOfWork _work;
        public TokenService(IConfiguration configuration, IEntityRepository<EmpsessionTab> empSession, IEntityRepository<PromsessionTab> promSession,IUnitOfWork work)
        {
            _configuration = configuration;
            _empSession = empSession;
            _promSession = promSession;
            _work = work;
        }
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"])),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public async Task<TokenDTO> Refresh(TokenDTO tokenModel, CancellationToken token)
        {
            //  string accessToken = tokenModel.Access_Token;
            string refreshToken = tokenModel.Refresh_Token;

            var principal = GetPrincipalFromExpiredToken(tokenModel.Access_Token);

            // var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = await _empSession.FirstOrDefaultByExpressionAsync(x => x.Accesstoken == tokenModel.Access_Token && x.Refreshtoken == refreshToken, token).ConfigureAwait(false);
            if (user == null || user.Refreshtokenexpirydatetime <= DateTime.Now)
            {
                return null;
            }

            var newAccessToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();
            await UpdateRefreshToken(user.EmpId, newAccessToken, newRefreshToken,token).ConfigureAwait(false); ;

            return new TokenDTO
            {
                Access_Token = newAccessToken,
                Refresh_Token = newRefreshToken
            };
        }

        public async Task<TokenDTO> PromoterRefresh(TokenDTO tokenModel, CancellationToken token)
        {
            string refreshToken = tokenModel.Refresh_Token;

            var user = await _promSession.FirstOrDefaultByExpressionAsync(x => x.Refreshtoken == refreshToken, token).ConfigureAwait(false);

            var principal = GetPrincipalFromExpiredToken(user.Accesstoken);

            if (user == null || user.Refreshtokenexpirydatetime <= DateTime.Now)
            {
                return null;
            }

            if(user.Accesstokenexpirydatetime >= DateTime.Now)
            {
                return new TokenDTO
                {
                    Access_Token = user.Accesstoken,
                    Refresh_Token = user.Refreshtoken
                };
            }

            var newAccessToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();
            await UpdatePromRefreshToken(user.PromPan, newAccessToken, newRefreshToken, token).ConfigureAwait(false); ;
            await _work.CommitAsync(token).ConfigureAwait(false);
            return new TokenDTO
            {
                Access_Token = newAccessToken,
                Refresh_Token = newRefreshToken
            };
        }
        private async Task<bool> UpdateRefreshToken(string EmpId, string AccessToken, string RefreshToken, CancellationToken token)
        {
             
                var loginHistory =await _empSession.FirstOrDefaultByExpressionAsync(x => x.EmpId == EmpId, token).ConfigureAwait(false);

                if (loginHistory == null)
                    return false;
                else
                {
                    loginHistory.Accesstoken = AccessToken;
                    loginHistory.Refreshtoken = RefreshToken;
                    loginHistory.SessionStatus = true;
                    loginHistory.LoginDateTime = DateTime.Now;
                    loginHistory.Refreshtokenrevoke = false;
                    loginHistory.Accesstokenrevoke = false;
                    loginHistory.Accesstokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"]));
                    loginHistory.Refreshtokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]));

                    await _empSession.UpdateAsync(loginHistory, token).ConfigureAwait(false);
                    return true;
                }
            
        }
        private async Task<bool> UpdatePromRefreshToken(string promPan, string AccessToken, string RefreshToken, CancellationToken token)
        {

            var loginHistory = await _promSession.FirstOrDefaultByExpressionAsync(x => x.PromPan == promPan, token).ConfigureAwait(false);

            if (loginHistory == null)
                return false;
            else
            {
                loginHistory.Accesstoken = AccessToken;
                loginHistory.Refreshtoken = RefreshToken;
                loginHistory.SessionStatus = true;
                loginHistory.LoginDateTime = DateTime.Now;
                loginHistory.Refreshtokenrevoke = false;
                loginHistory.Accesstokenrevoke = false;
                loginHistory.Accesstokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"]));
                loginHistory.Refreshtokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]));

                await _promSession.UpdateAsync(loginHistory, token).ConfigureAwait(false);
                return true;
            }

        }
    }
}
