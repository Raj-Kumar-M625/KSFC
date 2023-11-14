using BusinessLayer;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EpicCrmWebApi
{
    /// <summary>
    /// This is used to download just the most basic data (except customer and products)
    /// </summary>
    public class DownloadMiniController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId">Tenant Employee Id</param>
        /// <returns></returns>
        [HttpGet]
        public DownloadMiniDataResponse GetData(long employeeId, string IMEI = "", string appVersion="")
        {
            DownloadMiniDataResponse response = new DownloadMiniDataResponse();

            try
            {
                if (Business.IsRequestSourceValid(employeeId, IMEI) == false)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "Invalid Source";
                    response.EraseData = true;
                    return response;
                }

                // app version check
                if (!String.IsNullOrEmpty(appVersion))
                {
                    if (!Business.IsAppVersionSupported(appVersion, Helper.GetCurrentIstDateTime()))
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Content = $"App version {appVersion} is not supported.";
                        response.EraseData = true;
                        return response;
                    }
                }

                // user must be active
                if (!Business.IsUserAllowed(employeeId))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "User does not exist or not active.";
                    response.EraseData = true;
                    return response;
                }

                TenantEmployee te = Business.GetTenantEmployee(employeeId);
                string staffCode = te?.EmployeeCode ?? "";

                Helper.FillResponseCodeTableData(response, staffCode);

                response.StatusCode = HttpStatusCode.OK;
                response.Content = "";
            }
            catch(Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(DownloadController), ex.ToString(), " ");
                response.Content = ex.ToString();
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}
