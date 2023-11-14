using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace EpicCrmWebApi
{
    /// <summary>
    /// Controller is used to process Mobile Action Data
    /// </summary>
    public class MobileActionController : ApiController
    {
        [HttpGet]
        public string Process(long tenantId, long employeeId = -1)
        {
            int returnStatus = 0;
            try
            {
                BusinessLayer.Business.ProcessMobileData(tenantId, employeeId);
                return returnStatus.ToString();
            }
            catch (Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(MobileActionController), ex.ToString(), " ");
                returnStatus = -1;
                return ex.ToString();
            }
        }
    }
}
