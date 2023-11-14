using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace EpicCrmWebApi
{
    /// <summary>
    /// Class to support Executive App - TStanes - Oct 31 2019
    /// </summary>
    public class ExecutiveStatsController : ApiController
    {
        [HttpGet]
        public CompressDownloadDataResponse EncryptedData(string imei, string version)
        {
            ExecutiveStatsResponse response = UnEncryptedData(imei, version);
            try
            {
                // convert it to json
                string responseAsString = JsonConvert.SerializeObject(response);

                // compress it
                // 1. make a byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(responseAsString);

                // 2. Create GZip
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var gzip = new GZipStream(ms, CompressionMode.Compress))
                    {
                        gzip.Write(inputBytes, 0, inputBytes.Length);
                    }
                    string compressedBase64Response = Convert.ToBase64String(ms.ToArray());

                    return new CompressDownloadDataResponse()
                    {
                        EncryptedData = compressedBase64Response,
                        Status = true,
                        UtcTimestamp = DateTime.UtcNow,
                        ErrorMessage = ""
                    };
                }
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(ExecutiveStatsController)}", ex);
                return new CompressDownloadDataResponse()
                {
                    EncryptedData = "",
                    Status = false,
                    ErrorMessage = ex.Message,
                    UtcTimestamp = DateTime.UtcNow
                };
            }
        }

        [HttpGet]
        public ExecutiveStatsResponse UnEncryptedData(string imei, string version)
        {
            ExecutiveStatsResponse response = new ExecutiveStatsResponse();

            // don't take the tenant id sent in request - as it is hardcoded 1 in exec Crm app.
            // - Aug 16 2019
            long tenantId = Utils.SiteConfigData.TenantId;

            try
            {
                DateTime processingDate = Helper.GetCurrentIstDateTime();
                response.TenantId = tenantId;
                response.ReportDate = processingDate.ToString("yyyy-MM-dd");
                response.Imei = imei;

                // check that IMEI is supported for this app
                if (!Business.IsImeiSupportedForExecApp(imei, processingDate))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "App is not supported on this device.";
                    response.EraseData = true;
                    return response;
                }

                // try to get record from ExecAppImei table - that defines entries for Global Executive
                // (global executives can see data for all areas)
                ExecAppImei execAppImeiRec = Business.GetImeiRecForExecAppGlobal(imei, processingDate);
                bool isRegisteredForExecCrmAtTopLevel = false;
                string staffCode = "";
                if (execAppImeiRec != null)
                {
                    isRegisteredForExecCrmAtTopLevel = true;
                    staffCode = "SuperAdmin";
                    response.EnableLogging = execAppImeiRec.EnableLog;
                }
                else
                {
                    isRegisteredForExecCrmAtTopLevel = false;
                    // get staff code
                    TenantEmployee te = Business.GetEmployeeRecord(imei);
                    if (te != null)
                    {
                        staffCode = te.EmployeeCode;
                        response.EnableLogging = te.SendLogFromPhone;
                    }
                }

                // Need to get the list of Sales Person that this user can see the data for
                DomainEntities.StaffFilter searchCriteria = new DomainEntities.StaffFilter()
                {
                    TenantId = tenantId,
                    IsSuperAdmin = isRegisteredForExecCrmAtTopLevel,
                    CurrentUserStaffCode = staffCode
                };

                response.Divisions = Business.GetCodeTable("Division");
                response.Segments = Business.GetCodeTable("Segment");
                response.DivisionSegments = Business.GetDivisionSegment(tenantId);

                List<string> applicableDivisions = FillStaffDivisions(response, isRegisteredForExecCrmAtTopLevel, staffCode);

                // March 20 2020
                // send only those divisions that current requesting person has access to
                
                if (isRegisteredForExecCrmAtTopLevel == false && applicableDivisions != null)
                {
                    response.Divisions = response.Divisions
                                        .Where(x => applicableDivisions.Any(y => y.Equals(x.Code, StringComparison.OrdinalIgnoreCase)))
                                        .ToList();

                    response.DivisionSegments = response.DivisionSegments
                                        .Where(x => applicableDivisions.Any(y => y.Equals(x.DivisionCode, StringComparison.OrdinalIgnoreCase)))
                                        .ToList();

                    // send segments that belong to the divisions that we are sending to phone.
                    response.Segments = response.Segments.Where(x => response.DivisionSegments.Any(y => y.SegmentCode.Equals(x.Code, StringComparison.OrdinalIgnoreCase)))
                                        .ToList();
                }

                IEnumerable<SalesPersonModel> salesPersons = Business.GetSalesPersonData(searchCriteria);

                // here salesPersons is the list of sales persons that are as per assignment.
                // response.StaffDivisions has list of sales person by division.
                // We need to send only those sales persons that are as per assignments (that is already
                // done in salesPersons) and also belong to the applicableDivisions (that is in response.StaffDivisions)
                // [ So it is join of sales person in selected areas and selected divisions ]
                
                response.SalesPersons = salesPersons
                    .Where(x=> response.StaffDivisions.Any(y=> y.StaffCode.Equals(x.StaffCode, StringComparison.OrdinalIgnoreCase)))
                    .Select(x => new SalesPersonMiniModel()
                                {
                                    Name = x.Name,
                                    StaffCode = x.StaffCode,
                                }).ToList();

                // list of HQs as per assignment for each sales person
                List<string> filteredStaffCodes = response.SalesPersons.Select(x => x.StaffCode).ToList();
                response.AssignedHQs = Business.GetHQCodeAsPerAssignment(filteredStaffCodes);

                FillOfficeHierarchy(response, imei, processingDate, isRegisteredForExecCrmAtTopLevel);

                SetStaffDailyData(response, isRegisteredForExecCrmAtTopLevel, applicableDivisions);

                FillCustomers(response);
                FillCustomerSalesData(response, isRegisteredForExecCrmAtTopLevel, applicableDivisions);
                FillPPAData(response, filteredStaffCodes);

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(ExecutiveStatsController), ex.ToString(), " ");
                response.Content = ex.ToString();
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        private static void FillOfficeHierarchy(ExecutiveStatsResponse response, string imei, 
                            DateTime processingDate, bool isRegisteredForExecCrmAtTopLevel)
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = Business.GetAssociations();

            if (isRegisteredForExecCrmAtTopLevel)
            {
                response.OfficeHierarchy = officeHierarchy;
            }
            else
            {
                // IMEI passed does not belongs to a Global Exec - 
                // send data only for the area codes that this user is authorized
                IEnumerable<string> managingAreasCodes = Business.GetManagingAreaCodes(imei);
                response.OfficeHierarchy = officeHierarchy
                        .Where(x => managingAreasCodes.Any(y => y.Equals(x.AreaCode, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
            }
        }

        private static void SetStaffDailyData(ExecutiveStatsResponse response, bool isRegisteredForExecCrmAtTopLevel, 
                                            IEnumerable<string> applicableDivisions)
        {
            ICollection<StaffDailyData> dailyData = Business.GetStaffDailyData();
            // return data only for those sales persons who are visible to current user;
            // return data only for those areas that are visible to current user;

            var staffDailyData = dailyData
                .Where(x => response.SalesPersons.Any(y => y.StaffCode.Equals(x.StaffCode, StringComparison.OrdinalIgnoreCase))
                        && response.OfficeHierarchy.Any(y => y.AreaCode.Equals(x.AreaCode, StringComparison.OrdinalIgnoreCase))
                       )
                .Select(c => new DownloadStaffDailyData()
                {
                    Date = c.Date,
                    StaffCode = c.StaffCode,
                    DivisionCode = c.DivisionCode,
                    SegmentCode = c.SegmentCode,
                    AreaCode = c.AreaCode,
                    TargetOutstandingYTD = c.TargetOutstandingYTD,
                    TotalCostYTD = c.TotalCostYTD,
                    CGAYTD = c.CGAYTD,
                    GT180YTD = c.GT180YTD,
                    CollectionTargetYTD = c.CollectionTargetYTD,
                    CollectionActualYTD = c.CollectionActualYTD,
                    SalesTargetYTD = c.SalesTargetYTD,
                    SalesActualYTD = c.SalesActualYTD
                });

            // March 20 2020
            // send data only for those divisions that current requesting user has access to
            if (isRegisteredForExecCrmAtTopLevel == false)
            {
                // get division codes that are already set in response structure
                staffDailyData = staffDailyData.Where(x => applicableDivisions.Any(y => y.Equals(x.DivisionCode, StringComparison.OrdinalIgnoreCase)));
            }

            response.StaffDailyData = staffDailyData.ToList();
        }

        // fill customers who belong to HQCodes that are visible to this person
        private static void FillCustomers(ExecutiveStatsResponse response)
        {
            List<string> visibleHQCodes = response.OfficeHierarchy.Select(x => x.HQCode)
                .GroupBy(x => x)
                .Select(x => x.Key)
                .ToList();

            response.Customers = Business.GetCustomers(visibleHQCodes);
        }

        private static void FillCustomerSalesData(ExecutiveStatsResponse response, bool isRegisteredForExecCrmAtTopLevel, 
                                                IEnumerable<string> applicableDivisions)
        {
            // make the list of customer codes 
            List<string> customerCodes = response.Customers.Select(x => x.Code).ToList();
            response.CustomerDivisionBalances = Business.GetCustomerDivisionBalance(customerCodes);

            // March 20 2020
            // send data only for divisions that current requesting person has access to
            if (isRegisteredForExecCrmAtTopLevel == false && response.CustomerDivisionBalances != null)
            {
                response.CustomerDivisionBalances = response.CustomerDivisionBalances
                        .Where(x => applicableDivisions.Any(y => y.Equals(x.DivisionCode, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
            }
        }

        private static void FillPPAData(ExecutiveStatsResponse response, IEnumerable<string> staffCodes)
        {
            //response.SalesPersons has the list of staff that this user can see.
            // we should send the PPA data only for these staff codes
            response.PPAData = Business.GetPPAData(staffCodes);
        }

        private static List<string> FillStaffDivisions(ExecutiveStatsResponse response, 
            bool isRegisteredForExecCrmAtTopLevel, 
            string staffCode)
        {
            ICollection<StaffDivision> staffDivisions = Business.GetStaffDivisions();
            if (isRegisteredForExecCrmAtTopLevel)
            {
                response.StaffDivisions = staffDivisions;
                return null;
            }
            else
            {
                // March 20 2020
                // send only those divisions that the current phone user has access to
                var applicableDivisions = staffDivisions
                                    .Where(x => x.StaffCode.Equals(staffCode, StringComparison.OrdinalIgnoreCase))
                                    .Select(x => x.DivisionCode)
                                    .Distinct()
                                    .ToList();

                response.StaffDivisions = staffDivisions
                                    .Where(x => applicableDivisions.Any(y => y.Equals(x.DivisionCode, StringComparison.OrdinalIgnoreCase)))
                                    .ToList();

                return applicableDivisions;
            }
        }
    }
}
