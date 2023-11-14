using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace EpicCrmWebApi
{
    /// <summary>
    /// Class to support Executive App
    /// </summary>
    public class EmployeeStatsController : ApiController
    {
        [HttpGet]
        public EmployeeStatsSummaryWithDetailResponse GetSummaryWithDetail(string IMEI, string Version, DateTime? reportDate)
        {
            return GetSummaryWithDetail2(IMEI, Version, 0, "", 0, reportDate);
        }
        [HttpGet]
        public EmployeeStatsSummaryWithDetailResponse GetSummaryWithDetail2(string IMEI, 
            string Version,
            int currentRequestLevel,
            string currentCode,
            int targetLevel,
            DateTime? reportDate)
        {
            EmployeeStatsSummaryWithDetailResponse response = new EmployeeStatsSummaryWithDetailResponse();

            // don't take the tenant id sent in request - as it is hardcoded 1 in exec Crm app.
            // - Aug 16 2019
            long tenantId = Utils.SiteConfigData.TenantId;

            try
            {
                DateTime processingDate = Helper.GetCurrentIstDateTime();
                if (reportDate.HasValue)
                {
                    processingDate = reportDate.Value;
                }

                response.TenantId = tenantId;
                response.ReportDate = processingDate.ToString("yyyy-MM-dd");

                // check that IMEI is supported for this app
                if (!Business.IsImeiSupportedForExecApp(IMEI, processingDate))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = "App is not supported on this phone.";
                    response.EraseData = true;
                    response.SummaryWithDetail = null;
                    response.ItemCount = 0;
                    return response;
                }

                // try to get record from ExecAppImei table - that defines entries for Global Executive
                // (global executives can see data for all areas)
                ExecAppImei execAppImeiRec = Business.GetImeiRecForExecAppGlobal(IMEI, processingDate);

                IEnumerable<string> managingCodes = null;

                if (execAppImeiRec != null)
                {
                    ExecAppRollupEnum rollup;
                    
                    if (currentRequestLevel == 0)
                    {
                        rollup = Business.ConvertToRollUpEnum(execAppImeiRec.ExecAppDetailLevel);
                        response.CurrentLevel = execAppImeiRec.ExecAppDetailLevel;
                    }
                    else
                    {
                        rollup = Business.ConvertToRollUpEnum(targetLevel);
                        response.CurrentLevel = targetLevel;
                        managingCodes = GetNextLevelCodes(currentRequestLevel, currentCode);
                    }

                    response.DerivedRollUp = rollup.ToString();
                    response.IsGlobal = true;
                    response.SummaryWithDetail = Helper.GetModelForPeopleInField(tenantId, processingDate, rollup, managingCodes);

                    // if support person, clear all phone numbers so that there is no accidental phone call from support person
                    // to any area manager or sales person
                    if (execAppImeiRec.IsSupportPerson)
                    {
                        Parallel.ForEach(response.SummaryWithDetail, x =>
                        {
                            Parallel.ForEach(x.Details, d =>
                            {
                                d.Phone = "9999999999";
                            });

                            Parallel.ForEach(x.Managers, am =>
                            {
                                am.Phone = "9999999999";
                            });
                        });
                    }
                }
                else
                {
                    TenantEmployee te = Business.GetEmployeeRecord(IMEI);

                    ExecAppRollupEnum rollup;
                    if (currentRequestLevel == 0)
                    {
                        response.CurrentLevel = te?.ExecAppDetailLevel ?? 4;
                        rollup = Business.ConvertToRollUpEnum(response.CurrentLevel);

                        // IMEI passed does not belongs to a Global Exec - 
                        // send data only for the codes that this user is authorized
                        managingCodes = Business.GetManagingCodes(te, rollup);
                    }
                    else
                    {
                        response.CurrentLevel = targetLevel;
                        rollup = Business.ConvertToRollUpEnum(response.CurrentLevel);

                        // get codes of requested level;
                        managingCodes = Business.GetManagingCodes(te, rollup);

                        // now further filter it - as user has asked to drill down for one code only.
                        var requestedCodes = GetNextLevelCodes(currentRequestLevel, currentCode);

                        // security:
                        // user should have access to the nextLevelCodes - hence following code
                        managingCodes = requestedCodes.Where(x => managingCodes.Any(y => y.Equals(x, StringComparison.OrdinalIgnoreCase)))
                            .ToList();
                    }

                    response.DerivedRollUp = rollup.ToString();
                    response.IsGlobal = false;

                    response.SummaryWithDetail = Helper.GetModelForPeopleInField(tenantId, processingDate, rollup, managingCodes);

                    //response.SummaryWithDetail = response.SummaryWithDetail
                    //    .Where(x => managingCodes.Any(y => y.Equals(x.Code, StringComparison.OrdinalIgnoreCase))).ToList();
                }

                response.ItemCount = response.SummaryWithDetail?.Count() ?? 0;

                response.NextLevel = response.CurrentLevel + 1;
                response.IsDrillDownSupported = (response.NextLevel > 1 && response.NextLevel <= 4);

                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(EmployeeStatsController), ex.ToString(), " ");
                response.Content = ex.ToString();
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpGet]
        public SupportedStatus IsSupported(string IMEI, DateTime? reportDate, long tenantId = 1)
        {
            SupportedStatus response = new SupportedStatus()
            {
                IsSupported = false,
                Content = ""
            };

            try
            {
                DateTime processingDate = Helper.GetCurrentIstDateTime();
                if (reportDate.HasValue)
                {
                    processingDate = reportDate.Value;
                }

                response.IsSupported = Business.IsImeiSupportedForExecApp(IMEI, processingDate);
            }
            catch (Exception ex)
            {
                BusinessLayer.Business.LogError(nameof(EmployeeStatsController), ex.ToString(), " ");
                response.IsSupported = false;
                response.Content = ex.ToString();
            }

            return response;
        }

        private ICollection<string> GetNextLevelCodes(int currentLevel, string currentCode)
        {
            ICollection<OfficeHierarchy> oh = Business.GetAssociations();

            if (currentLevel == 1)
            {
                return oh.Where(x => x.ZoneCode.Equals(currentCode, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.AreaCode).ToList();
            }
            else if (currentLevel == 2)
            {
                return oh.Where(x => x.AreaCode.Equals(currentCode, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.TerritoryCode).ToList();
            }
            else if (currentLevel == 3)
            {
                return oh.Where(x => x.TerritoryCode.Equals(currentCode, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.HQCode).ToList();
            }
            return new List<string>();
        }
    }
}
