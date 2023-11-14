using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class BaseSiteConfigData
    {
        public DateTime UtcRefreshTime { get; set; }
    }

    public class SiteConfigData : BaseSiteConfigData
    {
        public long Id { get; set; }
        public string SiteName { get; set; }
        public string SiteUrl { get; set; }

        public string TitleBarColor { get; set; }
        public string TitleBarTextColor { get; set; }
        public long TenantId { get; set; }

        public bool ActivityOption { get; set; }
        public bool ExpenseOption { get; set; }
        public bool CustomerOption { get; set; }
        public bool SummaryOption { get; set; }
        public bool UploadOption { get; set; }
        public bool DownloadOption { get; set; }
        public bool BankOption { get; set; }

        public bool AddEntityOption { get; set; }
        public bool ShowAlertToUpload { get; set; }
        public bool ShowAlertToEndDay { get; set; }
        public bool IsEntityNameEnabled { get; set; }

        public bool IsEntityButtonVisible { get; set; }
        public bool IsAddressVisible { get; set; }
        public bool IsAddressMandatory { get; set; }
        public bool IsMySummaryVisible { get; set; }
        public bool IsEntityImagesEnabled { get; set; }

        public bool IsLogsButtonVisible { get; set; }
        public bool TakeLastKnownLocation { get; set; }
        public bool AutoUploadFromPhone { get; set; }

        public bool AddContactForSelectedEntityOnActivity { get; set; }
        public bool PullAlertMessagesFromServer { get; set; }

        public string EndDayAlertMessage { get; set; }

        public int MaxImagesPerPage { get; set; }
        public int GPSDesiredAccuracyInMeters { get; set; }
        public int GPSTimeOutInSeconds { get; set; }
        public string ActivityPageName { get; set; }

        //public byte[] PanelLogo { get; set; }
        //public byte[] SplashScreenLogo { get; set; }
        public byte[] HomePageLogo { get; set; }
        public string Caption { get; set; }

        public string DisplayDateFormatString { get; set; }
        public string OnlyDateDisplayFormatString { get; set; }

        public string EndDayAlertTime1 { get; set; }
        public string EndDayAlertTime2 { get; set; }

        // Dec 22 2018
        public bool WorkflowActivityOption { get; set; }
        public bool TrackLocationByCrossGeoLocator { get; set; }
        public bool TrackLocationByGPS { get; set; }
        public bool TrackLocationByNetwork { get; set; }
        public int GPSNetworkTrackTimeInSeconds { get; set; }
        public int GPSNetworkTrackDistInMts { get; set; }

        // April 1 2019
        public bool IssueReturnModule { get; set; }
        public bool AutoDownload { get; set; }
        public bool ActivityModuleAddEnabled { get; set; }
        public string ActivityModuleIcon { get; set; }
        public string ExpenseModuleIcon { get; set; }
        public string CustomerModuleIcon { get; set; }
        public string WorkflowModuleIcon { get; set; }
        public string EntityModuleIcon { get; set; }
        public bool EntityModuleUniqueIdVisible { get; set; }
        public bool EntityModuleTaxIdVisible { get; set; }
        public string EntityModuleImageButtonCaption { get; set; }
        public string IssueReturnModuleIcon { get; set; }
        public string IssueReturnModuleImageButtonCaption { get; set; }
        public string ActivityModuleColor { get; set; }
        public string EntityModuleColor { get; set; }
        public bool IsImageMandatory { get; set; }
        public bool RedFarmerModule { get; set; }
        public bool LeaveModule     {get; set;}

        public bool AdvanceRequestModule { get; set; }

        // May 5 2019
        public bool LogErrorsToDb { get; set; }
        public bool DetailLoggingForExcelFileUpload { get; set; }
        public bool DeleteExcelFileAfterUpload { get; set; }
        public bool IgnoreIMEICheckOnIncomingRequest { get; set; }
        public bool IgnorePhoneCheckOnRegisterRequest { get; set; }
        public bool EnforceAppVersionCheckOnStartDay { get; set; }
        
        public bool DumpRequestContentToFile { get; set; }
        public bool LogGetRequest { get; set; }
        public bool LogIncomingPortalRequest { get; set; }
        public string GoogleMapAPIKey { get; set; }
        public string SuperAdminUserName { get; set; }
        public string SuperAdminUserPassword { get; set; }
        public string SuperAdminUserEmail { get; set; }
        public int MaxDashboardUsers { get; set; }
        public int MaxTopItemCountForDashboard { get; set; }
        public int LifespanInMinutesForOnFlySuperAdminUser { get; set; }
        public bool AllowSalesPersonMaintenanceOnWeb { get; set; }
        public bool SalesPersonGradeEnabled { get; set; }
        public bool ForceEndPreviousBatchOnStartDay { get; set; }
        public string WorkflowActivityEntityType { get; set; }
        public bool ShowTerminateAndDeleteLinksOnCrmUsersPage { get; set; }
        public bool ShowAvailableLeaveDataOnPhone { get; set; }
        public string DateDisplayFormatString { get; set; }
        public decimal RatePerDistanceUnit { get; set; }
        public decimal MaxDiscountPercentage { get; set; }
        public string DiscountType { get; set; }
        public bool IsEntityAgreementEnabled { get; set; }

        public bool PutNewStaffOnAutoUpload { get; set; }
        public long TimeIntervalInMillisecondsForTracking { get; set; }
        public string LocationFromType { get; set; }
        public bool IsSMSEnabled { get; set; }
        public bool SMSTestMode { get; set; }
        public bool SMSOnDemandFeatureEnabled { get; set; }
        public string SMSApiKey { get; set; }
        public string SMSSender { get; set; }
        public string SMSRecipient { get; set; }
        public string SMSTemplate { get; set; }
        public int MaxSMSTextSize { get; set; }
        public string OrderSMSToCustomerTemplate { get; set; }
        public string ImagesFolder { get; set; }
        public string ActivityImageFilePrefix { get; set; }
        public string ExpenseImageFilePrefix { get; set; }
        public string PaymentImageFilePrefix { get; set; }
        public string OrderImageFilePrefix { get; set; }
        public string EntityImageFilePrefix { get; set; }

        public bool UseMCCMNCValuesForCoordinates { get; set; }
        public string DashboardPageTitle { get; set; }

        // Regex to validate Entity Module Unique Id
        public string EMUniqueIdRegEx { get; set; }
        // is unique Id a number only field - to show appropriate keyboard
        public bool IsEMUniqueIdNumber { get; set; }

        // Reports
        public bool ExpenseReport { get; set; }
        public bool FieldActivityReport { get; set; }
        public bool ProfileProgressReport { get; set; }
        public bool AttendanceReport { get; set; }
        public bool AttendanceSummaryReport { get; set; }
        public bool AttendanceRegister { get; set; }
        public bool AbsenteeReport { get; set; }
        public bool AppSignUpReport { get; set; }
        public bool AppSignInReport { get; set; }
        public bool ActivityReport { get; set; }
        public bool ActivityByTypeReport { get; set; }
        public bool KPIReport { get; set; }
        public bool MAPReport { get; set; }
        public bool WrongLocationReport { get; set; }

        public bool DistantActivityReport => WrongLocationReport;

        // June 14 2019
        public string TransmitDateFormat { get; set; }
        public int ActivityButtonSequence { get; set; }
        public int ExpenseButtonSequence { get; set; }
        public int CustomerButtonSequence { get; set; }
        public int SummaryButtonSequence { get; set; }
        public int UploadButtonSequence { get; set; }
        public int DownloadButtonSequence { get; set; }
        public int WorkflowButtonSequence { get; set; }
        public int BankButtonSequence { get; set; }
        public int LeaveButtonSequence { get; set; }
        public int EntityButtonSequence { get; set; }
        public int IssueButtonSequence { get; set; }
        public bool EmployeeExpenseReport { get; set; }
        public bool EmployeeExpenseReport2 { get; set; }  // added for Heranba
        public bool DistanceReport { get; set; }
        public bool UnSownReport { get; set; }  // added for Reitzel
        
        // Author:PK; Purpose: Day Planning; Date: 26/04/2021
        //public bool DayPlanningReport { get; set; }

        public bool AllowActivityEdit { get; set; }
        public string PhoneNoRegEx { get; set; }

        public string SiteGuid { get; set; }
        public bool ArchiveImage { get; set; }
        public string S3Bucket { get; set; }
        public int ArchiveImageAfterDays { get; set; }
        public int UploadImageCountPerCycle { get; set; }
        public bool DeleteLocalImageAfterUpload { get; set; }

        //public bool CreateDownloadDataCache { get; set; }
        //public string DownloadDataCacheFolder { get; set; }

        // used on phone to download data in compressed or uncompressed format.
        public bool PerformCompressedDataDownload { get; set; }

        //public string S3BucketJson { get; set; }

        public long DBServerId { get; set; }
        public string DBName { get; set; }

        // - August 4 2019
        public bool RevisedUploadSupported { get; set; }
        //public string SiteCode { get; set; }

        // To Log Database query, that is generated by LINQ
        public string PostDataFileLocation { get; set; }
        public int DaysToKeepPostDataOnServer { get; set; }
        public bool LogDatabaseQuery { get; set; }

        // Whether buttons are enabled on phone or not.
        public bool OrderButton { get; set; }
        public bool PaymentButton { get; set; }
        public bool ReturnButton { get; set; }

        public string EmployeeExpenseRdlcReportName { get; set; }
        public string EmployeeExpenseSumupClassName { get; set; }

        // Dec 4 2019 - to enforce apk version check on Batch Upload
        public bool EnforceAppVersionCheckOnEndDay { get; set; }

        // Dec 6 2019 - File Upload
        public bool AllowCSVUpload { get; set; }
        public bool AllowExcelUpload { get; set; }
        public int FileUploadErrorStopLimit { get; set; } //
        public string FileUploadCSVSeparator { get; set; } //
        public string FileUploadTrimChars { get; set; } //

        // March 5 2020 - Define folder for customer specific views
        public string CustomWebViewPath { get; set; }

        // March 24 2020
        public string PANRegEx { get; set; }
        public string AgreementDefaultStatus { get; set; }

        // May 12 2020
        public bool STROption { get; set; }
        public bool DWSPaymentReport { get; set; }
        public bool TransporterPaymentReport { get; set; }

        public bool ValueAddedServices { get; set; }

        public bool QuestionnaireOption { get; set; }
        public bool ApplyLeaveOption { get; set; }
    }
}
