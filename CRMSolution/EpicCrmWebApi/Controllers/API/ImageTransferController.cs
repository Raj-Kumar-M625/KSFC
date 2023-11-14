using BusinessLayer;
using CRMUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace EpicCrmWebApi.Controllers.API
{
    /// <summary>
    /// Controller is used to Transfer images to S3
    /// </summary>
    public class ImageTransferController : ApiController
    {
        [HttpGet]
        public string Process()
        {
            int returnStatus = 0;

            // take tenantId from UrlResolve
            long tenantId = Utils.SiteConfigData.TenantId;

            // Url Resolve - setting is made to not archive images
            // August 14 2020
            if (Utils.SiteConfigData.ArchiveImage == false)
            {
                return returnStatus.ToString();
            }

            try
            {
                int imagesToUploadPerCycle = Utils.SiteConfigData.UploadImageCountPerCycle;
                Business.ProcessImageTransfer(tenantId, 
                                    Utils.SiteConfigData.DeleteLocalImageAfterUpload,
                                    imagesToUploadPerCycle);

                return returnStatus.ToString();
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(ImageTransferController), ex.ToString(), " ");
                returnStatus = -1;
                return ex.ToString();
            }
        }
    }
}
