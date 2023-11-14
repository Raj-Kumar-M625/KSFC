using BusinessLayer;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpicCrmWebApi
{
    [Authorize(Roles = "Manager")]
    public class EmployeeMapController : BaseDashboardController
    {

        //[CheckRightsAuthorize(Feature = FeatureEnum.ActivityFeature)] is changed 
        //to below line as we are giving rights to each and every reports seprately from the super admin.
        [CheckRightsAuthorize(Feature = FeatureEnum.MAPFeature)]
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmployeeMap
        public ActionResult SignedInEmployees(DateTime requestDate)
        {
            //IEnumerable<SignedInEmployeeData> employeeDataList = Business.GetSignedInEmployeeData(requestDate);

            //// Convert data in DomainEntity object to Model class object
            //ICollection<SignedInEmployeeDataModel> dataModel = employeeDataList.Select(edl => new SignedInEmployeeDataModel()
            //{
            //    EmployeeDayId = edl.EmployeeDayId,
            //    EmployeeName = edl.EmployeeName,
            //    Latitude = edl.Latitude,
            //    Longitude = edl.Longitude,
            //    TrackingRecordId = edl.TrackingRecordId,
            //    EmployeeId = edl.EmployeeId,
            //    TrackingTime = edl.TrackingTime
            //}).ToList();

            var dataModel = GetDataModelForActivityMap(requestDate);
            ViewBag.ActivityDate = requestDate;
            return View(dataModel);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult SignedInEmployeesPartial(DateTime requestDate)
        {
            //DateTime requestDate = Helper.GetCurrentIstDateTime();
            var dataModel = GetDataModelForActivityMap(requestDate);
            ViewBag.ActivityDate = requestDate.ToString("ddd dd-MM-yyyy hh:mm:ss tt");
            return PartialView("_SignedInEmployees", dataModel);
        }

        private ICollection<SignedInEmployeeDataModel> GetDataModelForActivityMap(DateTime requestDate)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.User);

            DomainEntities.SearchCriteria sc = new DomainEntities.SearchCriteria()
            {
                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2,
                ApplyZoneFilter = false,
                ApplyAmountFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyDataStatusFilter = false,
                ApplyDateFilter = false
            };

            IEnumerable<SignedInEmployeeData> employeeDataList = Business.GetSignedInEmployeeData(sc, requestDate);

            // Convert data in DomainEntity object to Model class object
            ICollection<SignedInEmployeeDataModel> dataModel = employeeDataList.Select(edl => new SignedInEmployeeDataModel()
            {
                EmployeeDayId = edl.EmployeeDayId,
                EmployeeName = edl.EmployeeName,
                Latitude = edl.Latitude,
                Longitude = edl.Longitude,
                TrackingRecordId = edl.TrackingRecordId,
                EmployeeId = edl.EmployeeId,
                TrackingTime = edl.TrackingTime
            }).ToList();

            return dataModel;
        }
    }
}