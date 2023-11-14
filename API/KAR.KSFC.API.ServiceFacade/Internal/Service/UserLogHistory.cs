using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using KAR.KSFC.Components.Data.Repository.UoW;

namespace KAR.KSFC.API.ServiceFacade.Internal.Service
{
    public class UserLogHistory : IUserLogHistory
    {
        private readonly IEntityRepository<RegduserTab> _userRepository;
        private readonly IEntityRepository<PromsessionTab> _promotersesionRepository;
        private readonly IEntityRepository<EmpsessionTab> _empsesionRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _work;
        public UserLogHistory(IEntityRepository<RegduserTab> userRepository, IEntityRepository<PromsessionTab> promotersesionRepository, IConfiguration configuration,
            IEntityRepository<EmpsessionTab> empsesionRepository, IUnitOfWork work)//, IEntityRepository<KsfcUserTab> empRepository)
        {
            _userRepository = userRepository;
            _promotersesionRepository = promotersesionRepository;
            _empsesionRepository = empsesionRepository;
            _configuration = configuration;
            _work = work;
        }

        //public bool UserLoggedInDifferentIP(string IP, Usermaster userModel)
        //{
        //    try
        //    {
        //        bool IsUserLogin = false;
        //        var UserId = _appContext.Usermasters.Where(x => x.Username == userModel.Username).Select(y => y.Id).FirstOrDefault();
        //        Loginhistory loginHistory = _appContext.Loginhistories.Where(a => a.Userid == UserId).OrderByDescending(a => a.Accesstokenexpirytime).FirstOrDefault();
        //        var AccessTokenExpireTime = Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]);
        //        if (loginHistory != null)
        //        {
        //            if (loginHistory.AccesstokenRevoke == false)
        //            {
        //                TimeSpan timeDiff = DateTime.Now.Subtract(loginHistory.Accesstokenexpirytime.Value);

        //                if (timeDiff.TotalMinutes < AccessTokenExpireTime)
        //                {
        //                    IsUserLogin = true;
        //                }
        //            }
        //        }

        //        return IsUserLogin;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<bool> UpdateCustomerLoginHistory(string IP, string panNum, string AccessToken, string RefreshToken, CancellationToken token)
        {
            //disable other IPs and tokens for this user
            var userPan = await _userRepository.FirstOrDefaultByExpressionAsync(x => x.UserPan == panNum, token).ConfigureAwait(false);
            List<PromsessionTab> lstloginHistory = await _promotersesionRepository.FindByExpressionAsync(a => a.PromPan == userPan.UserPan, token).ConfigureAwait(false);
            lstloginHistory.ForEach(a =>
            {
                a.SessionStatus = false;
                a.Accesstokenrevoke = true;
                a.Refreshtokenrevoke = true;
            });

            PromsessionTab loginHistory = await _promotersesionRepository.FirstOrDefaultByExpressionAsync(a => a.PromPan == userPan.UserPan, token).ConfigureAwait(false);
            if (loginHistory != null)
            {
                loginHistory.Ipadress = IP;
                loginHistory.PromPan = panNum;
                loginHistory.Accesstoken = AccessToken;
                loginHistory.Refreshtoken = RefreshToken;
                loginHistory.SessionStatus = true;
                loginHistory.Refreshtokenrevoke = false;
                loginHistory.Accesstokenrevoke = false;
                loginHistory.LoginDateTime = DateTime.Now;
                loginHistory.Accesstokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"]));
                loginHistory.Refreshtokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]));
                await _promotersesionRepository.UpdateAsync(loginHistory, token).ConfigureAwait(false);
            }
            else
            {
                loginHistory = new PromsessionTab()
                {
                    Ipadress = IP,
                    PromPan = panNum,
                    Accesstoken = AccessToken,
                    Refreshtoken = RefreshToken,
                    SessionStatus = true,
                    Refreshtokenrevoke = false,
                    Accesstokenrevoke = false,
                    LoginDateTime = DateTime.Now,
                    // Modifylogin = DateTime.Now,
                    Accesstokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"])),
                    Refreshtokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]))
                };
                await _promotersesionRepository.AddAsync(loginHistory, token).ConfigureAwait(false);
            }
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> UpdateAdminLoginHistory(string empId, string AccessToken, string RefreshToken, string ip, CancellationToken token)
        {

            // disable other IPs and tokens for this user
            List<EmpsessionTab> lstloginHistory = await _empsesionRepository.FindByExpressionAsync(a => a.EmpId == empId, token).ConfigureAwait(false);
            lstloginHistory.ForEach(a =>
            {
                a.SessionStatus = false;
                a.Accesstokenrevoke = true;
                a.Refreshtokenrevoke = true;
            });

            EmpsessionTab loginHistory = await _empsesionRepository.FirstOrDefaultByExpressionAsync(a => a.EmpId == empId, token).ConfigureAwait(false); ;// && a.Macadress == IP);
            if (loginHistory != null)
            {
                loginHistory.Ipadress = ip;
                loginHistory.EmpId = empId;
                loginHistory.Accesstoken = AccessToken;
                loginHistory.Refreshtoken = RefreshToken;
                loginHistory.SessionStatus = true;
                loginHistory.Refreshtokenrevoke = false;
                loginHistory.Accesstokenrevoke = false;
                loginHistory.LoginDateTime = DateTime.Now;
                loginHistory.Accesstokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"]));
                loginHistory.Refreshtokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]));
                await _empsesionRepository.UpdateAsync(loginHistory, token).ConfigureAwait(false);
            }
            else
            {
                loginHistory = new EmpsessionTab()
                {
                    Ipadress = ip,
                    EmpId = empId,
                    Accesstoken = AccessToken,
                    Refreshtoken = RefreshToken,
                    SessionStatus = true,
                    Refreshtokenrevoke = false,
                    Accesstokenrevoke = false,
                    LoginDateTime = DateTime.Now,
                    // Modifylogin = DateTime.Now,
                    Accesstokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"])),
                    Refreshtokenexpirydatetime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]))
                };
                await _empsesionRepository.AddAsync(loginHistory, token).ConfigureAwait(false);
            }
            await _work.CommitAsync(token).ConfigureAwait(false);
            return true;
        }

        //public bool DifferentIPLoggedIn(string IP, string username, string AccessToken, string RefreshToken)
        //{
        //    try
        //    {
        //        bool IsAnotherUserLogin = false;
        //        var UserId = _appContext.Usermasters.Where(x => x.Username == username).Select(y => y.Id).FirstOrDefault();
        //        Loginhistory loginHistory = _appContext.Loginhistories.
        //            Where(a => a.Userid == UserId && a.AccessToken != AccessToken && a.Refreshtoken != RefreshToken).
        //            OrderByDescending(a => a.Accesstokenexpirytime).FirstOrDefault();

        //        if (loginHistory != null)
        //        {
        //            if (loginHistory.AccesstokenRevoke == false)
        //            {
        //                TimeSpan timeDiff = DateTime.Now.Subtract(loginHistory.Accesstokenexpirytime.Value);
        //                if (timeDiff.TotalMinutes < 20)
        //                {
        //                    IsAnotherUserLogin = true;
        //                }
        //            }
        //        }
        //        return IsAnotherUserLogin;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public long UpdateRefreshToken(string IP, string username, string AccessToken, string RefreshToken)
        //{
        //    try
        //    {
        //        var UserId = _appContext.Usermasters.Where(x => x.Username == username).Select(y => y.Id).FirstOrDefault();
        //        Loginhistory loginHistory = _appContext.Loginhistories.Where(a => a.Userid == UserId && a.Macadress == IP)
        //            .OrderByDescending(a => a.Accesstokenexpirytime).FirstOrDefault();

        //        if (loginHistory == null)
        //            return 0;
        //        else
        //        {
        //            loginHistory.AccessToken = AccessToken;
        //            loginHistory.Refreshtoken = RefreshToken;
        //            loginHistory.Isactive = true;
        //            loginHistory.Modifylogin = DateTime.Now;
        //            loginHistory.RefreshtokenRevoke = false;
        //            loginHistory.AccesstokenRevoke = false;
        //            loginHistory.Accesstokenexpirytime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:AccessTokenExpire"]));
        //            loginHistory.Refreshtokenexpirytime = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:RefreshTokenExpire"]));

        //            _appContext.SaveChanges();
        //            return loginHistory.Id;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        throw;
        //    }
        //}

    }
}
