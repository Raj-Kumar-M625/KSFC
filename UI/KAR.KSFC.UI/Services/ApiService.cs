using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.UI.Services.IServices;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly SessionManager _sessionManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        const string SessionAdminToken = "JWTToken";
        const string SessionAdminUser = "AdminUsername";
        public ApiResultResponse ApiResultResponse { get; set; }
        public ApiService(IHttpClientFactory clientFactory, SessionManager sessionManager, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _sessionManager = sessionManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<HttpResponseMessage> PostAdminAsync(string url, object postData)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var accToken = _sessionManager.GetLoginCustToken();
                if (accToken != null)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accToken);
                }
                var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
                client.DefaultRequestHeaders.Add("IpAddress", remoteIpAddress.ToString());
                var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
                var responseHttp = await client.PostAsync($"" + url, content);
                return responseHttp;
            }
            catch (Exception ex)
            {

            }
            return default;
        }
        public async Task<HttpResponseMessage> GetAdminAsync(string url)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var accToken = _sessionManager.GetLoginCustToken();
                if (accToken != null)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accToken);
                }
                if (_httpContextAccessor.HttpContext.Session.GetString(SessionAdminUser) != null)
                {
                    var token = _httpContextAccessor.HttpContext.Session.GetString(SessionAdminToken);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
                client.DefaultRequestHeaders.Add("IpAddress", remoteIpAddress.ToString());
                var responseHttp = await client.GetAsync($"" + url);
                return responseHttp;
            }
            catch (Exception ex)
            {

            }
            return default;
        }
        public async Task<HttpResponseMessage> PostAsync(string url, StringContent postData)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var accToken = _sessionManager.GetLoginCustToken();
                if (accToken != null)
                {
                    var refreshToken = _sessionManager.GetLoginCustRefToken();
                    TokenDTO info = new() { Access_Token = accToken, Refresh_Token = refreshToken };
                    var credentials = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("Account/PromoterRefresh", credentials);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        accToken = successObj.Result["access_Token"]?.Value;
                        var refToken = successObj.Result["refresh_Token"]?.Value;
                        _sessionManager.SetLoginCustToken(accToken);
                        _sessionManager.SetLoginCustRefToken(refToken);
                    }
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accToken);
                }
                if (accToken == null)
                {
                    var refreshToken = _sessionManager.GetLoginCustRefToken();
                    TokenDTO info = new() { Access_Token = accToken, Refresh_Token = refreshToken };
                    var credentials = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("Account/PromoterRefresh", credentials);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        accToken = successObj.Result["access_Token"]?.Value;
                        var refToken = successObj.Result["refresh_Token"]?.Value;
                        _sessionManager.SetLoginCustToken(accToken);
                        _sessionManager.SetLoginCustRefToken(refToken);
                    }
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accToken);

                }
                var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
                client.DefaultRequestHeaders.Add("IpAddress", remoteIpAddress.ToString());
                
                var responseHttp = await client.PostAsync($"" + url, postData);
                return responseHttp;
            }
            catch (Exception ex)
            {

            }
            return default;
        }
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            try
            {
                var client = _clientFactory.CreateClient("ksfcApi");
                var accToken = _sessionManager.GetLoginCustToken();
                if (accToken != null)
                {
                    var refreshToken = _sessionManager.GetLoginCustRefToken();
                    TokenDTO info = new() { Access_Token = accToken, Refresh_Token = refreshToken };
                    var credentials = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("Account/PromoterRefresh", credentials);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        accToken = successObj.Result["access_Token"]?.Value;
                        var refToken = successObj.Result["refresh_Token"]?.Value;
                        _sessionManager.SetLoginCustToken(accToken);
                        _sessionManager.SetLoginCustRefToken(refToken);
                    }
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accToken);

                }
                if (_httpContextAccessor.HttpContext.Session.GetString(SessionAdminUser) != null)
                {
                    var token = _httpContextAccessor.HttpContext.Session.GetString(SessionAdminToken);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
                if (accToken == null)
                {
                    var refreshToken = _sessionManager.GetLoginCustRefToken();
                    TokenDTO info = new() { Access_Token = accToken, Refresh_Token = refreshToken };
                    var credentials = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("Account/PromoterRefresh", credentials);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var successObj = JsonConvert.DeserializeObject<ApiResultResponse>(responseString);
                        accToken = successObj.Result["access_Token"]?.Value;
                        var refToken = successObj.Result["refresh_Token"]?.Value;
                        _sessionManager.SetLoginCustToken(accToken);
                        _sessionManager.SetLoginCustRefToken(refToken);
                    }
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accToken);

                }
                var remoteIpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
                client.DefaultRequestHeaders.Add("IpAddress", remoteIpAddress.ToString());
                var responseHttp = await client.GetAsync($"" + url);
                return responseHttp;
            }
            catch (Exception ex)
            {

            }
            return default;
        }
    }
}
