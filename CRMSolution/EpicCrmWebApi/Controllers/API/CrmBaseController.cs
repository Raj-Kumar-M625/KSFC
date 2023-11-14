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

namespace EpicCrmWebApi
{
    /// <summary>
    /// Base controller for common API functionality
    /// </summary>
    public class CrmBaseController : ApiController
    {
        protected void LogModelErrors(string errorSource)
        {
            string errorText = GetModelErrors();
            if (String.IsNullOrEmpty(errorText) == false)
            {
                Business.LogError(errorSource, errorText, " ");
            }
        }

        protected string GetModelErrors()
        {
            if (!ModelState.IsValid)
            {
                // https://www.exceptionnotfound.net/asp-net-mvc-demystified-modelstate/
                string allModelErrors = String.Join("; ",
                            ModelState.Values
                            .SelectMany(ms => ms.Errors)
                            .Select(x =>
                            {
                                return String.IsNullOrEmpty(x.ErrorMessage) ? x.Exception.Message : x.ErrorMessage;
                            }));
                return allModelErrors;
            }
            return String.Empty;
        }

        protected void FillUserConfiguration(long employeeId, StartDayResponse sdr)
        {
            TenantEmployee te = Business.GetTenantEmployee(employeeId);
            sdr.TimeIntervalInMillisecondsForTracking = te != null ? te.TimeIntervalInMillisecondsForTrcking : 0;
            sdr.SendLogs = te?.SendLogFromPhone ?? false;
            sdr.AutoUpload = te?.AutoUploadFromPhone ?? false;

            // these fields can later be maintained at TenantEmployee level - if so needed;
            sdr.MaxDiscountPercentage = (double)Utils.SiteConfigData.MaxDiscountPercentage;
            sdr.DiscountType = Utils.SiteConfigData.DiscountType;

            sdr.ActivityPageName = te?.ActivityPageName ?? "";

            sdr.AvailablePaidLeaves = 5;
            sdr.AvailableCompOffs = 7;
            sdr.ShowAvailableLeaveData = Utils.SiteConfigData.ShowAvailableLeaveDataOnPhone;
            sdr.WorkflowActivityEntityType = Utils.SiteConfigData.WorkflowActivityEntityType;
            sdr.EnhancedDebugEnabled = te?.EnhancedDebugEnabled ?? false;
            sdr.TestFeatureEnabled = te?.TestFeatureEnabled ?? false;
            sdr.VoiceFeatureEnabled = te?.VoiceFeatureEnabled ?? false;

            SalesPersonModel salesPersonRec = Business.GetSingleSalesPersonData(te?.EmployeeCode ?? "");
            sdr.BusinessRole = salesPersonRec?.BusinessRole ?? "";
        }

        protected string GetBatchDataFileName()
        {
            string keyName = Utils.PostDataFileNameKey;
            if (HttpContext.Current.Items.Contains(keyName))
            {
                string fileName = HttpContext.Current.Items[keyName] as string;
                return Utils.TruncateString(fileName, 128);
            }

            return "";
        }

        protected long GetBatchDataFileSize()
        {
            string keyName = Utils.PostDataFileSizeKey;
            if (HttpContext.Current.Items.Contains(keyName))
            {
                var o = HttpContext.Current.Items[keyName];

                long batchSize = 0;
                long.TryParse(o as string, out batchSize);
                return batchSize;
            }

            return -1;
        }
    }
}
