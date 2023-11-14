using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace EpicCrmWebApi
{
    /// <summary>
    /// Controller is used to send SMS
    /// </summary>
    public class SmsController : ApiController
    {
        [HttpGet]
        public string Process(long tenantId)
        {
            int returnStatus = 0;

            string requestFileName = $"{Utils.SiteConfigData.SiteName.Replace(' ', '_')}_SMSReq_{ Guid.NewGuid().ToString()}.txt";
            string smsRequestLogFile = Helper.GetStorageFileNameWithPath(requestFileName);

            string responseFileName = $"{Utils.SiteConfigData.SiteName.Replace(' ', '_')}_SMSRes_{ Guid.NewGuid().ToString()}.txt";
            string smsResponseLogFile = Helper.GetStorageFileNameWithPath(responseFileName);

            //string smsRequestLogFile = HttpContext.Current.Server.MapPath("~/App_Data/" + "SmsReq_" + Guid.NewGuid().ToString() + ".txt");
            //string smsResponseLogFile = HttpContext.Current.Server.MapPath("~/App_Data/" + "SmsRes_" + Guid.NewGuid().ToString() + ".txt");

            Business.ProcessSms(tenantId, Helper.GetCurrentIstDateTime(), smsRequestLogFile, smsResponseLogFile);
            return returnStatus.ToString();
        }
    }
}
