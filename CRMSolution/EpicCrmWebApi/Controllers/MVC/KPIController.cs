using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EpicCrmWebApi
{
    [Authorize(Roles = "Manager")]
    public class KPIController : BaseDashboardController
    {
        // GET: Main Dashboard Page
        [CheckRightsAuthorize(Feature = FeatureEnum.KPIFeature)]
        public ActionResult Index(int topItemCount = -1)
        {
            ViewBag.TopItemsCount = GetTopItemCount(topItemCount);
            ViewBag.IsSuperAdmin = IsSuperAdmin;

            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
         
            ViewBag.OfficeHierarchy = officeHierarchy;

            return View();
        }

        /// <summary>
        /// User can specify top item count in web.config and can
        /// also override it in url as query string parameter
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        private int GetTopItemCount(int topCount)
        {
            int topItemsCount = Utils.GetTopItemCount();

            // now also look if TopItemCount is sent as query string
            if (topCount > 0)
            {
                topItemsCount = topCount;
            }

            return topItemsCount;
        }
    }
}