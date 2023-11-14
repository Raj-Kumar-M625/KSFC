using BusinessLayer;
using CRMUtilities;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// To Show header on each page
// https://stackoverflow.com/questions/10846752/how-to-repeat-tables-header-rows-on-each-page-in-reportviewer11-with-visual-stu
// 
// Add totals
// https://docs.microsoft.com/en-us/sql/reporting-services/lesson-6-adding-grouping-and-totals-reporting-services?view=sql-server-2017
//
// Report will not show in Chrome
// https://stackoverflow.com/questions/18731997/rdlc-data-not-showing-in-google-chrome
//
// Table header not repeating on each page
// https://stackoverflow.com/questions/20699635/rdlc-tablix-column-header-not-repeating-on-every-page-repeat-column-header-on-e
//
// how to control export options
// https://www.aspsnippets.com/Articles/ASPNet-RDLC-Local-SSRS-Report-Viewer-Hide-Disable-specific-export-option-Word-Excel-PDF-from-Export-button.aspx


namespace EpicCrmWebApi.Reports
{
    public partial class ExpenseReport : System.Web.UI.Page
    {
        private DomainEntities.SearchCriteria dsc = null;
        private string rdlcReportName = "";
        private bool isTestMode = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Create Object from data in Query String
            SearchCriteria sc = new SearchCriteria()
            {
                Zone = Request.QueryString["Zone"],
                Area = Request.QueryString["Area"],
                Territory = Request.QueryString["Territory"],
                HQ = Request.QueryString["HQ"],
                EmployeeCode = Request.QueryString["EmployeeCode"],
                EmployeeName = Request.QueryString["EmployeeName"],
                DateFrom = Request.QueryString["DateFrom"],
                DateTo = Request.QueryString["DateTo"],
                ReportType = Request.QueryString["ReportType"],
            };

            rdlcReportName = Request.QueryString["rdlcReportName"];

            int empStatus = 0;
            Int32.TryParse(Request.QueryString["EmployeeStatus"], out empStatus);
            sc.EmployeeStatus = empStatus;

            isTestMode = !String.IsNullOrEmpty(Request.QueryString["TestMode"]);

            dsc = Helper.DashboardParseSearchCriteria(sc);
            if (false == dsc.ApplyDateFilter)
            {
                Business.LogError(nameof(ExpenseReport), "Valid report options (dates) not specified");
                HttpContext.Current.Response.Redirect("/");
                ReportViewer1.Visible = false;
                return;
            }

            // ensure that user has rights for the report
            if (string.IsNullOrEmpty(sc.ReportType))
            {
                Business.LogError(nameof(ExpenseReport), "Report type parameter not available in the request.");
                HttpContext.Current.Response.Redirect("/");
                ReportViewer1.Visible = false;
                return;
            }

            FeatureData availableFeatures = Helper.GetAvailableFeatures(HttpContext.Current.User.Identity.Name, 
                                            Helper.IsSuperAdmin(HttpContext.Current.User));
            if (availableFeatures == null)
            {
                Business.LogError(nameof(ExpenseReport), "Feature data is not available.");
                HttpContext.Current.Response.Redirect("/");
                ReportViewer1.Visible = false;
                return;
            }

            bool status = Helper.IsFeatureEnabled(FeatureEnum.EmployeeExpenseReportFeature, availableFeatures);
            if (status == false)
            {
                Business.LogError(nameof(ExpenseReport), $"{sc.ReportType} not available for Current user {HttpContext.Current.User.Identity.Name}");
                HttpContext.Current.Response.Redirect("/");
                ReportViewer1.Visible = false;
                return;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            // if request is not authenticated, go to login page
            if (HttpContext.Current.User.Identity.IsAuthenticated == false)
            {
                HttpContext.Current.Response.Redirect("/");
                ReportViewer1.Visible = false;
            }

            base.OnPreRender(e);

            if (!IsPostBack)
            {
                Business.LogError(nameof(ExpenseReport), nameof(OnPreRender));

                string dateDisplayFormat = Utils.SiteConfigData.OnlyDateDisplayFormatString;
                string todayDate = Helper.GetCurrentIstDateTime().ToString(dateDisplayFormat);

                try
                {
                    IEnumerable<EmployeeExpenseReportDataModel> model = (isTestMode) 
                                ? ReportTestData() : Helper.GetEmployeeExpenseReportModel(dsc);
                    Business.LogError(nameof(ExpenseReport), $"{model.Count()} rows are used for report.");
                  

                    // set report from and To Date in report parameters
                    string reportFromDate = "";
                    string reportToDate = "";
                    if (dsc.ApplyDateFilter)
                    {
                        reportFromDate = dsc.DateFrom.ToString(dateDisplayFormat);
                        reportToDate = dsc.DateTo.ToString(dateDisplayFormat);
                    }


                    ReportViewer1.ProcessingMode = ProcessingMode.Local;
                    //ReportViewer1.LocalReport.ReportPath = Server.MapPath("/Reports/ExpenseReport.rdlc");
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath($"/{rdlcReportName}.rdlc");
                    ReportViewer1.LocalReport.DataSources.Clear();

                    ReportDataSource dataSource = new ReportDataSource("DataSet1", model);
                    ReportViewer1.LocalReport.DataSources.Add(dataSource);

                    // set parameters for report
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("SiteName", Utils.SiteConfigData.Caption));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("TodayDate", todayDate));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("ReportFromDate", reportFromDate));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("ReportToDate", reportToDate));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("DateFormat", dateDisplayFormat));

                    // hide word/excel report formats
                    string[] exportOptionsToKeep = new string[] { "PDF" };
                    var availableRenderOptions = ReportViewer1.LocalReport.ListRenderingExtensions()
                                                    .ToList();

                    foreach(var ro in availableRenderOptions)
                    {
                        if (false == exportOptionsToKeep.Any(x=> x.Equals(ro.Name, StringComparison.OrdinalIgnoreCase)))
                        {
                            FieldInfo fieldInfo = ro.GetType().GetField("m_isVisible", BindingFlags.Instance | BindingFlags.NonPublic);
                            fieldInfo?.SetValue(ro, false);
                        }
                    }

                    ReportViewer1.LocalReport.Refresh();
                }
                catch (Exception ex)
                {
                    Business.LogError(nameof(ExpenseReport), ex.ToString());
                    ReportViewer1.Visible = false;
                }
            }
        }

        #region Test Data
        private IEnumerable<EmployeeExpenseReportDataModel> ReportTestData()
        {
            List<EmployeeExpenseReportDataModel> resultSet = new List<EmployeeExpenseReportDataModel>();

            DateTime fromDate = new DateTime(2019, 06, 01);
            DateTime toDate = new DateTime(2019, 06, 20);
            FillTestData(resultSet, "W0000010", fromDate, toDate);
            FillTestData(resultSet, "E0000010", fromDate, toDate);

            return resultSet;
        }

        private void FillTestData(ICollection<EmployeeExpenseReportDataModel> resultSet, string staffCode, DateTime fromDate, DateTime toDate)
        {
            int recCount = 0;
            while(toDate.Date >= fromDate.Date)
            {
                recCount++;
                var o = new EmployeeExpenseReportDataModel()
                {
                    StaffCode = staffCode,
                    Name = $"{staffCode}_Name",
                    Department = $"{staffCode} Department",
                    Designation = $"{staffCode} Designation",
                    StartPosition = $"{staffCode} {toDate.ToString("yyyy-MM-dd")} StartPosition",
                    EndPosition = $"{staffCode} {toDate.ToString("yyyy-MM-dd")} EndPosition",
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.Now,
                    ExpenseDate = toDate.Date,
                    ModeAndClassOfFare = 1000 + recCount,
                    LodgeRent = 2000 + recCount,
                    LocalConveyance = 500 + recCount,
                    OutstationConveyance = 600 + recCount,
                    IncdlCharges = 50 + recCount,
                    CommunicationExpenses = 70 + recCount,
                    ZoneName = $"{staffCode}_Zone Name",
                    AreaName = $"{staffCode}_Area Name",
                    TerritoryName = $"{staffCode}_Territory Name",
                    HQName = $"{staffCode}_HQ Name",
                };

                resultSet.Add(o);
                //fromDate = fromDate.AddDays(1);
                toDate = toDate.AddDays(-1);
            }
        }
        #endregion
    }
}