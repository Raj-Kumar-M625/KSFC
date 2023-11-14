using BusinessLayer;
using DomainEntities;
using EpicCrmWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace EpicCrmWebApi
{
    public class DistanceController : ApiController
    {
        [HttpGet]
        //public JsonResult<UserDataResponse> GetUserData(UserDataRequest userRequest)
        public JsonResult<string> CalculateDistance()
        {
            int returnStatus = 0;
            try
            {
                BusinessLayer.Business.CalculateTrackingDistance();
                return Json(returnStatus.ToString());
            }
            catch(Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(DistanceController), ex.ToString(), " ");
                returnStatus = -1;
                return Json(ex.ToString());
            }
        }
    }
}