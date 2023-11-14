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
    public class AreaCodeController : ApiController
    {
        /// <summary>
        /// Get the list of Area Codes
        /// </summary>
        /// <param name="employeeId">Tenant Employee Id</param>
        /// <returns></returns>
        [HttpGet]
        public AreaCodeResponse GetData(long employeeId, string IMEI = "", string appVersion = "")
        {
            AreaCodeResponse response = new AreaCodeResponse();

            if (Business.IsRequestSourceValid(employeeId, IMEI) == false)
            {
                response.Content = "Invalid Source";
                response.EraseData = true;
                return response;
            }

            // user must be active
            if (!Business.IsUserAllowed(employeeId))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = "User does not exist or not active.";
                response.EraseData = true;
                return response;
            }

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

            response.AreaCodes = Business.GetAreaCodes(employeeId);
            response.StatusCode = HttpStatusCode.OK;
            response.Content = "";

            return response;
        }
    }
}
