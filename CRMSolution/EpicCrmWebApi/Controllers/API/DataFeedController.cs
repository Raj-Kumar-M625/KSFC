using BusinessLayer;
using CRMUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace EpicCrmWebApi.Controllers.API
{
    /// <summary>
    /// Controller is used to process the Data Feed
    /// Transform SAP data to project tables
    /// </summary>
    public class DataFeedController : ApiController
    {
        [HttpGet]
        public string Process(long tenantId)
        {
            int returnStatus = 0;
            try
            {
                // don't use tenant id passed as input - instead take it from urlResolver Config
                Business.ParseUploadFile(CRMUtilities.Utils.SiteConfigData.TenantId);

                // don't use tenant id passed as input - instead take it from urlResolver Config
                Business.ProcessDataFeed(CRMUtilities.Utils.SiteConfigData.TenantId);

                // since this is nightly job - also delete the users 
                // which have been created as super admin user on the fly
                // and have completed their life span
                ICollection<string> disabledUserList = Business.GetDisabledPortalLogins();
                foreach(string u in disabledUserList)
                {
                    BusinessLayer.Business.LogError(nameof(DataFeedController), $"Lapsed user {u} being deleted from Portal Table", "");
                    BusinessLayer.Business.DeleteUserFeatureAccess(u);
                    Helper.DeletePortalLogin(u);
                }

                // Delete old post data files
                string folder = Helper.GetServerPathForFileStorage();
                DateTime cutoffDate = DateTime.UtcNow.AddDays(Utils.SiteConfigData.DaysToKeepPostDataOnServer * -1);

                IEnumerable<FileSystemInfo> filesInfo = new DirectoryInfo(folder).EnumerateFileSystemInfos($"{Utils.SiteConfigData.SiteName.Replace(' ', '_')}*.txt");
                foreach(FileSystemInfo fi in filesInfo)
                {
                    if (fi.CreationTime < cutoffDate)
                    {
                        Helper.DeleteFile(fi.FullName);
                    }
                }

                return returnStatus.ToString();
            }
            catch (Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(DataFeedController), ex.ToString(), " ");
                returnStatus = -1;
                return ex.ToString();
            }
        }

        [HttpGet]
        public string RefreshOfficeHierarchy(long tenantId)
        {
            int returnStatus = 0;
            try
            {
                // don't use tenant id passed as input - instead take it from urlResolver Config
                //BusinessLayer.Business.RefreshOfficeHierarchy(CRMUtilities.Utils.SiteConfigData.TenantId);

                // office hierarchy is now transformed as part of Transform Data Feed only - 
                // August 18 2019
                return returnStatus.ToString();
            }
            catch (Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(DataFeedController), ex.ToString(), " ");
                returnStatus = -1;
                return ex.ToString();
            }
        }
    }
}
