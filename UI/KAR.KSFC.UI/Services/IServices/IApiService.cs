using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices
{
    public interface IApiService
    {
        Task<HttpResponseMessage> PostAdminAsync(string url, object postData);
        Task<HttpResponseMessage> PostAsync(string url, StringContent postData);
        Task<HttpResponseMessage> GetAdminAsync(string url);
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
