using CrmAlert;
using CRMUtilities;
using DatabaseLayer;
using DomainEntities;
using DomainEntities.Questionnaire;
using GSMTracker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Collections.ObjectModel;

namespace BusinessLayer
{
    public static class Business
    {
        public static string TwoWheeler = "Two Wheeler";
        public static string FourWheeler = "Four Wheeler";

        public static BusinessResponse RecordStartDay(StartDay sd)
        {
            BusinessResponse br = new BusinessResponse();

            // 1. check that employee exist
            TenantEmployee te = DBLayer.GetEmployeeRecord(sd.EmployeeId);
            if (te == null)
            {
                br.EmployeeDayId = -1;
                br.Content = $"Invalid User Id {sd.EmployeeId}";
                br.TimeIntervalInMillisecondsForTracking = -1;
                return br;
            }

            br.TimeIntervalInMillisecondsForTracking = te.TimeIntervalInMillisecondsForTrcking;

            // 2. Create start of the day request
            br.EmployeeDayId = DBLayer.ProcessStartDayRequest(sd);
            br.Content = (br.EmployeeDayId == 0) ? "Failed Sproc Call" : "Success";

            if (br.EmployeeDayId > 0)
            {
                // create a tracking request as well
                Tracking tracking = new Tracking()
                {
                    At = sd.At,
                    IsMileStone = true,
                    IsStartOfDay = true,
                    IsEndOfDay = false,
                    EmployeeDayId = br.EmployeeDayId,
                    ActivityId = 0,
                    Latitude = sd.Latitude,
                    Longitude = sd.Longitude
                };
                RecordTracking(tracking);
            }

            return br;
        }

        public static void DeletePendingUpload(long uploadId)
        {
            DBLayer.DeletePendingUpload(uploadId);
        }

        /// <summary>
        /// Delete feature access for this user
        /// </summary>
        /// <param name="u"></param>
        public static void DeleteUserFeatureAccess(string u)
        {
            DBLayer.DeleteUserFeatureAccess(u);
        }

        public static ICollection<SmsDataEx> GetSmsDetailData(long tenantId, DateTime currentIstDateTime)
        {
            // Get Sms Data
            ICollection<SmsDataEx> smsData = DBLayer.GetDataForSMS(tenantId, currentIstDateTime);
            return smsData;
        }

        public static bool IsAppVersionSupported(string appVersion, DateTime at)
        {
            // if configured to bypass version number check - return true;
            if (Utils.SiteConfigData.EnforceAppVersionCheckOnStartDay == false)
            {
                return true;
            }

            return DBLayer.IsAppVersionSupported(appVersion, at);
        }

        public static bool IsAppVersionSupportedOnUpload(string appVersion, DateTime at)
        {
            // if configured to bypass version number check - return true;
            if (Utils.SiteConfigData.EnforceAppVersionCheckOnEndDay == false)
            {
                return true;
            }

            return DBLayer.IsAppVersionSupported(appVersion, at);
        }

        internal static void SaveParseErrors(long id, List<ExcelUploadError> errorList)
        {
            if (errorList == null)
            {
                return;
            }

            // truncate string values before save
            foreach (var e in errorList)
            {
                e.MessageType = Utils.TruncateString(e.MessageType, 50);
                e.CellReference = Utils.TruncateString(e.CellReference, 50);
                e.ExpectedValue = Utils.TruncateString(e.ExpectedValue, 50);

                e.ActualValue = Utils.TruncateString(e.ActualValue, 512);
                e.Description = Utils.TruncateString(e.Description, 512);
            }
            DBLayer.SaveParseErrors(id, errorList);
        }

        public static ICollection<ExcelUploadError> GetParseErrors(long refId)
        {
            return DBLayer.GetParseErrors(refId);
        }

        public static IEnumerable<CodeTable> GetActiveCrops()
        {
            ICollection<WorkflowSeason> seasons = DBLayer.GetOpenWorkflowSeasons();
            return seasons.Select(x => new CodeTable()
            {
                Code = x.TypeName,
                DisplaySequence = 10
            }).ToList();
        }

        public static ExecAppImei GetImeiRecForExecAppGlobal(string Imei, DateTime dt)
        {
            // imeiRec here can be null;
            ExecAppImei imeiRec = DBLayer.GetImeiRecForExecAppGlobal(Imei);

            if (imeiRec != null && imeiRec.EffectiveDate <= dt && imeiRec.ExpiryDate >= dt)
            {
                return imeiRec;
            }

            return null;
        }

        public static bool IsImeiRegisteredForExecAppGlobal(string Imei, DateTime dt)
        {
            // app can be supported in two ways - by adding IMEI in ExecAppImei table
            // or by allowing for registered phone users (TenantEmployee.ExecAppAccess)
            // this routine checks if IMEI is registered for Exec CRM App as Global Exec
            ExecAppImei rec = GetImeiRecForExecAppGlobal(Imei, dt);

            return rec != null;
        }

        public static bool IsImeiSupportedForExecApp(string Imei, DateTime dt)
        {
            // app can be supported in two ways - by adding IMEI in ExecAppImei table
            // or by allowing for registered phone users (TenantEmployee.ExecAppAccess)
            bool status = IsImeiRegisteredForExecAppGlobal(Imei, dt);
            if (status)
            {
                return status;
            }

            // if user is not supported through ExecAppImei table - check if allowed in
            // TenantEmployee table - but for this we have to go via dbo.IMEI table
            // as we have only IMEI number here.
            TenantEmployee te = DBLayer.GetEmployeeRecord(Imei);
            if (te == null || te.IsActive == false || te.ExecAppAccess == false)
            {
                return false;
            }

            // we also need to make sure that user is active in SalesPerson table
            return IsUserAllowed(te.EmployeeCode);
        }

        public static ICollection<AssignedHQ> GetHQCodeAsPerAssignment(IEnumerable<string> salesPersonStaffCodes)
        {
            List<AssignedHQ> assignedHQs = new List<AssignedHQ>();
            if ((salesPersonStaffCodes?.Count() ?? 0) == 0)
            {
                return assignedHQs;
            }

            foreach (var sp in salesPersonStaffCodes)
            {
                IEnumerable<OfficeHierarchy> officeHierarchy = Business.GetAssociations(sp);
                assignedHQs.AddRange(
                    officeHierarchy.Select(x => x.HQCode).Distinct().Select(x => new AssignedHQ()
                    {
                        HQCode = x,
                        StaffCode = sp
                    }).ToList()
                );
            }

            return assignedHQs;
        }

        public static IEnumerable<AppVersion> GetAllReleasedAppVersions()
        {
            return DBLayer.GetAllReleasedAppVersions();
        }

        public static IEnumerable<ExecAppImei> GetAllExecAppImei()
        {
            return DBLayer.GetAllExecAppImei();
        }

        public static ExecAppImei GetSingleExecAppImei(long imeiRecId)
        {
            return DBLayer.GetSingleExecAppImei(imeiRecId);
        }

        public static ICollection<SmsSummary> GetSmsSummaryData(ICollection<SmsDataEx> smsData,
            IEnumerable<CodeTableEx> offices,
            Func<SmsDataEx, string> groupingCriteria)
        {
            // Formulate Sms Items to be sent.
            ICollection<SmsSummary> smsSummaryItems = smsData.GroupBy(x => groupingCriteria(x))
            .Select(x => new SmsSummary()
            {
                Code = x.Key,
                Name = offices.Where(o => o.Code == x.Key).FirstOrDefault()?.CodeName ?? "",
                // number of people in field
                InFieldCount = x.Where(y => y.IsInFieldToday).Count(),
                // number of people who are registered on phone
                RegisteredCount = x.Where(y => y.IsRegisteredOnPhone).Count(),
                // total people in the area
                HeadCount = x.Where(y => !String.IsNullOrWhiteSpace(y.StaffCode)).Count(),
                TotalOrderAmount = x.Sum(y => y.TotalOrderAmount),
                TotalReturnAmount = x.Sum(y => y.TotalReturnAmount),
                TotalPaymentAmount = x.Sum(y => y.TotalPaymentAmount),
                TotalExpenseAmount = x.Sum(y => y.TotalExpenseAmount),
                TotalActivityCount = x.Sum(y => y.TotalActivityCount)
            })
            .OrderBy(y => y.Name)
            .ToList();

            return smsSummaryItems;
        }

        public static void ProcessSms(long tenantId, DateTime currentIstDateTime,
            string smsRequestLogFile = "", string smsResponseLogFile = "")
        {
            long logId = 0;
            Business.LogError(nameof(ProcessSms), $"SMS Request received for TenantId {tenantId}", " ");

            // SMS has to be enabled
            if (!Business.IsSMSEnabled(tenantId))
            {
                Business.LogError(nameof(ProcessSms), $"SMS Disabled for TenantId: {tenantId}; check configuration");
                return;
            }

            if (Business.IsDayForSMS(tenantId, currentIstDateTime) == false)
            {
                Business.LogError(nameof(ProcessSms), $"No SMS Today due to company Holiday/non-woring day. TenantId: {tenantId}");
                return;
            }

            bool lockStatus = DBLayer.LockTenantForSMSProcessing(tenantId, Utils.AutoReleaseLockTimeInMinutes);

            if (!lockStatus)
            {
                Task.Run(async () =>
                {
                    await Business.LogAlert("WSMLKNA", "Lock could not be acquired to Process SMS");
                });

                LogError($"{nameof(ProcessSms)}", "A similar process may already be active. Terminating!", ".");
                return;
            }

            Task mainTask = Task.Factory.StartNew(() =>
            {
                logId = DBLayer.CreateSMSProcessLogEntry(tenantId, lockStatus);

                // retrieve the list of Tenant SMS Types
                ICollection<TenantSmsType> tenantSmsTypes = Business.GetSmsTypes(tenantId);

                foreach (var smsType in tenantSmsTypes)
                {
                    Business.LogError($"{nameof(ProcessSms)}", $"Processing TenantSmsType with id {smsType.Id} ; SmsProcessClass {smsType.SmsProcessClass}");
                    DBLayer.RenewLeaseForSMSProcessing(tenantId);

                    ISMSType smsProcessClass = null;
                    if (smsType.SmsProcessClass.Equals("StartEndDaySms", StringComparison.OrdinalIgnoreCase))
                    {
                        smsProcessClass = new StartEndDaySms();
                    }
                    else if (smsType.SmsProcessClass.Equals("OrderCustomerSms", StringComparison.OrdinalIgnoreCase))
                    {
                        smsProcessClass = new OrderCustomerSms();
                    }
                    else if (smsType.SmsProcessClass.Equals("OrderAgentSms", StringComparison.OrdinalIgnoreCase))
                    {
                        smsProcessClass = new OrderAgentSms();
                    }
                    else if (smsType.SmsProcessClass.Equals("EndDayAgentSms", StringComparison.OrdinalIgnoreCase))
                    {
                        smsProcessClass = new EndDayAgentSmsAsBulk();
                    }
                    else if (smsType.SmsProcessClass.Equals("ReitzelSms", StringComparison.OrdinalIgnoreCase))
                    {
                        smsProcessClass = new ReitzelSms();
                    }

                    if (smsProcessClass == null)
                    {
                        Business.LogError(nameof(ProcessSms), $"Invalid SmsProcessClass {smsType.SmsProcessClass} in SmsType {smsType.Id}");
                        continue;
                    }

                    smsProcessClass.Process(tenantId, smsType, currentIstDateTime, smsRequestLogFile, smsResponseLogFile);
                }
            }, TaskCreationOptions.LongRunning);

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromSMSProcessing(tenantId);
                DBLayer.UpdateSMSLogRecord(logId, false, true);
                long errorLogId = 0;
                p.Exception?.Handle(ex =>
                {
                    errorLogId = Business.LogError(nameof(ProcessSms), ex);
                    return true;
                });

                Task.Run(async () =>
                {
                    await Business.LogAlert("ESMFAULT", $"Error occured in SMS Processing; Log Id {errorLogId}");
                });

            }, TaskContinuationOptions.OnlyOnFaulted);

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromSMSProcessing(tenantId);
                DBLayer.UpdateSMSLogRecord(logId, true, false);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static ICollection<DomainEntities.EntityWorkFlow> GetEntityWorkFlow(string agreement)
        {
            return DBLayer.GetEntityWorkFlow(agreement);
        }

        public static ICollection<DomainEntities.WorkFlowSchedule> GetWorkFlowSchedule()
        {
            return DBLayer.GetWorkFlowSchedule();
        }

        public static ICollection<DomainEntities.WorkFlowFollowUp> GetWorkFlowFollowUp()
        {
            return DBLayer.GetWorkFlowFollowUp();
        }

        public static bool IsDataComingFromLegacyApp(string appVersion)
        {
            if (String.IsNullOrEmpty(appVersion))
            {
                return true;
            }

            if (appVersion.Equals("1.3", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Author:Pankaj K; Date:25/04/2021; Purpose:Upload Dealer Questionnaire;
        /// Save Question Paper
        /// </summary>
        /// <param name="inputObject"></param>
        /// <returns></returns>
        public static bool SaveQuestionnaire(QuestionPaper inputObject)
        {
            return DBLayer.SaveQuestionnaire(inputObject);
        }
        public static void SaveSmsData(string smsTemplate, SmsData sd)
        {
            long tenantId = Utils.SiteConfigData.TenantId;
            string jsonString = JsonConvert.SerializeObject(sd);
            DBLayer.SaveSmsData(tenantId, smsTemplate, "JSON", jsonString);
        }

        public static void SaveBankDetail(EntityBankDetail ea, string currentUser)
        {
            // retrieve old value of Agreement
            //DomainEntities.EntityAgreement oldData = DBLayer.GetSingleAgreement(ea.Id);
            //ICollection<DomainEntities.EntityContact> contacts = DBLayer.GetEntityContacts(oldData.EntityId);
            //DomainEntities.Entity entity = DBLayer.GetSingleEntity(oldData.EntityId);
            //DomainEntities.SalesPerson sp = DBLayer.GetSingleSalesPerson(entity.EmployeeCode);

            //string smsTemplate = "";
            //SmsData sd = new SmsData()
            //{
            //    AgreementNumber = oldData.AgreementNumber,
            //    TypeName = oldData.TypeName,
            //    EntityName = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.Name ?? "",
            //    FieldPersonName = entity.EmployeeName,
            //    CompanyName = Utils.SiteConfigData.Caption
            //};

            DBLayer.UpdateBankDetail(ea, currentUser);

            // goes to Farmer
            //if (ea.IsPassbookReceived && oldData.IsPassbookReceived == false)
            //{
            //    // goes to Farmer
            //    sd.PhoneNumber = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.PhoneNumber ?? "";
            //    smsTemplate = "PassbookReceived";
            //    Business.SaveSmsData(smsTemplate, sd);
            //}

            //if (ea.Status.Equals(oldData.Status, StringComparison.OrdinalIgnoreCase) == false)
            //{
            //    if (ea.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // goes to Farmer
            //        sd.PhoneNumber = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.PhoneNumber ?? "";
            //        smsTemplate = "AgreementApproved";
            //        Business.SaveSmsData(smsTemplate, sd);
            //        return;
            //    }

            //    if (ea.Status.Equals("Terminated", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // goes to Field Person
            //        sd.PhoneNumber = sp.Phone;
            //        smsTemplate = "AgreementTerminated";
            //        Business.SaveSmsData(smsTemplate, sd);

            //        // goes to Farmer as well;
            //        sd.PhoneNumber = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.PhoneNumber ?? "";
            //        smsTemplate = "AgreementSettled";
            //        Business.SaveSmsData(smsTemplate, sd);
            //        return;
            //    }

            //    if (ea.Status.Equals("Closed", StringComparison.OrdinalIgnoreCase))
            //    {
            //        // goes to field Person
            //        sd.PhoneNumber = sp.Phone;
            //        smsTemplate = "AgreementClosed";
            //        Business.SaveSmsData(smsTemplate, sd);

            //        // goes to Farmer as well;
            //        sd.PhoneNumber = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.PhoneNumber ?? "";
            //        smsTemplate = "AgreementSettled";
            //        Business.SaveSmsData(smsTemplate, sd);
            //        return;
            //    }
            //}
        }

        public static void SaveAgreement(EntityAgreement ea, string currentUser)
        {
            // retrieve old value of Agreement
            DomainEntities.EntityAgreement oldData = DBLayer.GetSingleAgreement(ea.Id);
            ICollection<DomainEntities.EntityContact> contacts = DBLayer.GetEntityContacts(oldData.EntityId);
            DomainEntities.Entity entity = DBLayer.GetSingleEntity(oldData.EntityId);
            DomainEntities.SalesPerson sp = DBLayer.GetSingleSalesPerson(entity.EmployeeCode);

            string smsTemplate = "";
            SmsData sd = new SmsData()
            {
                AgreementNumber = oldData.AgreementNumber,
                TypeName = oldData.TypeName,
                EntityName = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.Name ?? "",
                FieldPersonName = entity.EmployeeName,
                CompanyName = Utils.SiteConfigData.Caption
            };

            DBLayer.UpdateAgreement(ea, currentUser);

            // goes to Farmer
            if (ea.IsPassbookReceived && oldData.IsPassbookReceived == false)
            {
                // goes to Farmer
                sd.PhoneNumber = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.PhoneNumber ?? "";
                smsTemplate = "PassbookReceived";
                Business.SaveSmsData(smsTemplate, sd);
            }

            if (ea.Status.Equals(oldData.Status, StringComparison.OrdinalIgnoreCase) == false)
            {
                if (ea.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                {
                    // goes to Farmer
                    sd.PhoneNumber = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.PhoneNumber ?? "";
                    smsTemplate = "AgreementApproved";
                    Business.SaveSmsData(smsTemplate, sd);
                    return;
                }

                if (ea.Status.Equals("Terminated", StringComparison.OrdinalIgnoreCase))
                {
                    // goes to Field Person
                    sd.PhoneNumber = sp.Phone;
                    smsTemplate = "AgreementTerminated";
                    Business.SaveSmsData(smsTemplate, sd);

                    // goes to Farmer as well;
                    sd.PhoneNumber = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.PhoneNumber ?? "";
                    smsTemplate = "AgreementSettled";
                    Business.SaveSmsData(smsTemplate, sd);
                    return;
                }

                if (ea.Status.Equals("Closed", StringComparison.OrdinalIgnoreCase))
                {
                    // goes to field Person
                    sd.PhoneNumber = sp.Phone;
                    smsTemplate = "AgreementClosed";
                    Business.SaveSmsData(smsTemplate, sd);

                    // goes to Farmer as well;
                    sd.PhoneNumber = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.PhoneNumber ?? "";
                    smsTemplate = "AgreementSettled";
                    Business.SaveSmsData(smsTemplate, sd);
                    return;
                }
            }
        }

        public static void SaveSurvey(EntitySurvey ea, string currentUser)
        {
            DBLayer.UpdateSurvey(ea, currentUser);
        }

        public static bool SendSMS(long tenantId, IEnumerable<string> phoneNumbers,
                                    string messageText, DateTime currentIstDateTime,
                                    string smsTypeName, string sender,
                                    string template,
                                    out long smsLogId)
        {
            IAlert alertApi = new SmsAlert();
            string formattedMessage = alertApi.PutMessageInTemplate(messageText, template);

            string alertApiResponse = alertApi.Send(formattedMessage, phoneNumbers);

            // Log SMS in db
            smsLogId = DBLayer.LogSms(tenantId, formattedMessage, currentIstDateTime, alertApiResponse, smsTypeName, sender);

            // parse api response and determine send status
            return alertApi.ParseSendResponse(alertApiResponse);
        }

        public static bool SendSMSAsBulk(long tenantId, DateTime currentIstDateTime,
                            string logText, string smsTypeName,
                            SMSMessages smsMessages, string sender,
                            string smsRequestLogFile, string smsResponseLogFile,
                            out long smsLogId)
        {
            IAlert alertApi = new SmsAlert();
            SmsResponseFormat smsResponseObject = alertApi.SendBulk(smsMessages, smsRequestLogFile, smsResponseLogFile);

            SmsResponseLog forLog = new SmsResponseLog()
            {
                test = smsResponseObject.test,
                balance_pre_send = smsResponseObject.balance_pre_send,
                total_cost = smsResponseObject.total_cost,
                balance_post_send = smsResponseObject.balance_post_send,
                status = smsResponseObject.status,
                MessagesSent = smsResponseObject.messages?.Count ?? 0,
                MessagesNotSent = smsResponseObject.messages_not_sent?.Count ?? 0,
                CurrentDateTime = currentIstDateTime.ToString("dd-MM-yyyy HH:mm:ss "),
                LogFileName = smsResponseLogFile
            };

            // convert it to json for logging
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SmsResponseLog));
            string jsonResponseString = "";
            using (MemoryStream ms = new MemoryStream())
            {
                js.WriteObject(ms, forLog);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    jsonResponseString = sr.ReadToEnd();
                    sr.Close();
                    ms.Close();
                }
            }

            // Log SMS in db
            if (String.IsNullOrEmpty(logText))
            {
                logText = $"Bulk message to {smsMessages.messages.Count} recipients on {forLog.CurrentDateTime}; Log file {smsRequestLogFile}";
            }
            smsLogId = DBLayer.LogSms(tenantId, logText, currentIstDateTime, jsonResponseString, smsTypeName, sender);

            // parse api response and determine send status
            return smsResponseObject.status.Equals("Success", StringComparison.OrdinalIgnoreCase);
        }

        public static IEnumerable<CustomerData> GetCustomerTotal(long tenantId, Func<CustomerData, string> selector2)
        {
            IEnumerable<CustomerData> customerDataByHQ = DBLayer.GetCustomerTotalByHQ(tenantId);

            // now rollup data as desired
            return customerDataByHQ.GroupBy(t => selector2(t))
                    .Select(t => new CustomerData()
                    {
                        Code = t.Key,
                        CustomerCount = t.Count(),
                        TotalOutstanding = t.Sum(u => u.TotalOutstanding),
                        TotalLongOutstanding = t.Sum(u => u.TotalLongOutstanding),
                        TotalTarget = t.Sum(u => u.TotalTarget)
                    }).ToList();
        }

        /// <summary>
        /// Super admin can create users on the fly which have limited time span;
        /// delete these users after their life span is over
        /// </summary>
        public static ICollection<string> GetDisabledPortalLogins()
        {
            return DBLayer.GetDisabledPortalLogins();
        }

        public static IEnumerable<DownloadCustomerExtend> GetCustomers(CustomersFilter searchCriteria)
        {
            searchCriteria.TenantId = Utils.SiteConfigData.TenantId;
            return DBLayer.GetCustomers(searchCriteria);
        }

        public static IEnumerable<DownloadCustomerExtend> GetCustomersWithLocation(CustomersFilter searchCriteria)
        {
            searchCriteria.TenantId = Utils.SiteConfigData.TenantId;
            return DBLayer.GetCustomersWithLocation(searchCriteria);
        }

        public static IEnumerable<DownloadCustomer> GetCustomers(IEnumerable<string> hqcodes)
        {
            return DBLayer.GetCustomers(hqcodes);
        }

        /// <summary>
        /// we need to auto disable the login for Super Admin user
        /// that has been created by SuperAdmin user on the fly.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsUserLoginDisableTimeReached(string userName)
        {
            DateTime? dt = DBLayer.GetDisableUserAfterUtcTime(userName);
            return (dt.HasValue == false || dt.Value >= DateTime.UtcNow) ? false : true;
        }

        public static TenantEmployee GetTenantEmployee(long employeeId)
        {
            return DBLayer.GetEmployeeRecord(employeeId);
        }

        public static IEnumerable<CodeTableEx> GetAreaCodes(string staffCode)
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = GetAssociations(staffCode);
            return officeHierarchy.Select(x => new
            {
                Code = x.AreaCode,
                CodeName = x.AreaName
            }).GroupBy(y => y.Code)
            .Select(y => new CodeTableEx()
            {
                Code = y.Key,
                CodeName = y.First().CodeName
            }).ToList();
        }

        public static ConsolidatedCustomerData GetConsolidatedCustomerDownloadInfo(string staffCode)
        {
            return DBLayer.GetConsolidatedCustomerDownloadInfo(Utils.SiteConfigData.TenantId, staffCode);
        }

        /// <summary>
        /// Get the list of area codes that are applicable to employeeId
        /// based on associations defined.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public static IEnumerable<CodeTableEx> GetAreaCodes(long employeeId)
        {
            TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            if (te == null || te.IsActive == false)
            {
                return new List<CodeTableEx>();
            }

            return GetAreaCodes(te.EmployeeCode);
        }

        public static SalesPersonModel GetSingleSalesPersonData(string staffCode)
        {
            IEnumerable<SalesPersonModel> data = GetSelectedSalesPersonData(new List<string>() { staffCode });
            return data.FirstOrDefault();
        }

        public static DomainEntities.STRWeight GetSingleSTRWeight(long strWeightId)
        {
            return DBLayer.GetSingleSTRWeight(strWeightId);
        }

        public static DomainEntities.STRTag GetSingleSTRTag(long strTagId)
        {
            return DBLayer.GetSingleSTRTag(strTagId);
        }

        public static DomainEntities.STR GetSingleSTR(long strId)
        {
            return DBLayer.GetSingleSTR(strId);
        }

        public static DomainEntities.DWS GetSingleDWS(long dwsId)
        {
            return DBLayer.GetSingleDWS(dwsId);
        }

        public static DomainEntities.Entity GetSingleEntity(long entityId)
        {
            return DBLayer.GetSingleEntity(entityId);
        }

        // ensure that request is coming from a phone where user is registered
        public static bool IsRequestSourceValid(long employeeId, string iMEI)
        {
            // if ignore requested (that is not to check IMEI for employeeId) - respect it;
            if (Utils.SiteConfigData.IgnoreIMEICheckOnIncomingRequest)
            {
                return true;
            }

            if (String.IsNullOrEmpty(iMEI) || employeeId == 0)
            {
                return false;
            }

            TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            if (te == null || te.IsActive == false || String.IsNullOrEmpty(te.IMEI))
            {
                return false;
            }

            return te.IMEI.Equals(iMEI, StringComparison.OrdinalIgnoreCase);
        }

        public static void CreateSalesPersonData(SalesPersonModel sp)
        {
            DBLayer.CreateSalesPersonData(sp);
        }

        public static void SaveSalesPersonData(SalesPersonModel es)
        {
            DBLayer.SaveSalesPersonData(es);
        }

        public static void SaveEntityData(Entity entityRec)
        {
            DBLayer.SaveEntityData(entityRec);
        }

        public static DBSaveStatus SaveDWSData(DWS dwsRec)
        {
            if (dwsRec.ChangeDWSData)
            {
                DBSaveStatus status = DBLayer.SaveDWSData(dwsRec);
                if (status != DBSaveStatus.Success)
                {
                    return status;
                }
                DBLayer.RecalculateSTRTotals(dwsRec.STRId, dwsRec.CurrentUser);
            }

            // now handle change in STR Number
            if (dwsRec.ChangeSTRNumber)
            {
                DBLayer.ReAssignDWS(dwsRec.Id, dwsRec.STRTagId, dwsRec.MoveToStrTagId, dwsRec.CurrentUser);
            }

            return DBSaveStatus.Success;
        }

        public static DBSaveStatus SaveDWSApprovedWeightData(DWS dwsRec)
        {
            dwsRec.NetPayableWt = dwsRec.GoodsWeight - dwsRec.SiloDeductWtOverride;
            dwsRec.GoodsPrice = dwsRec.NetPayableWt * dwsRec.RatePerKg;
            dwsRec.NetPayable = dwsRec.GoodsPrice - dwsRec.DeductAmount;

            dwsRec.Status = STRDWSStatus.WeightApproved.ToString();

            return DBLayer.SaveDWSApprovedWeightData(dwsRec);
        }

        public static DBSaveStatus SaveDWSApprovedAmountData(DWS dwsRec)
        {
            dwsRec.NetPayable = dwsRec.GoodsPrice - dwsRec.DeductAmount;

            dwsRec.Status = STRDWSStatus.AmountApproved.ToString();

            return DBLayer.SaveDWSApprovedAmountData(dwsRec);
        }

        public static DBSaveStatus SaveSTRData(STR strRec)
        {
            // first update any other details;
            //if (strRec.ChangeSTRData)
            {
                DBSaveStatus status = DBLayer.SaveSTRData(strRec);
                if (status != DBSaveStatus.Success)
                {
                    return status;
                }
            }

            // now handle change in STR Number
            if (strRec.ChangeSTRNumber)
            {
                DBLayer.ReAssignSTR(strRec.Id, strRec.STRTagId, strRec.MoveToStrTagId, strRec.CurrentUser);
            }

            return DBSaveStatus.Success;
        }

        public static DBSaveStatus SaveSTRWeightData(STRWeight strWeightRec)
        {
            DBSaveStatus status;
            if (strWeightRec.Id > 0)
            {
                status = DBLayer.SaveSTRWeightData(strWeightRec);
            }
            else
            {
                DBLayer.CreateSTRWeightData(strWeightRec);
                status = DBSaveStatus.Success;
            }

            return status;
        }

        public static DBSaveStatus SaveSTRTagData(STRTag strTagRec)
        {
            if (strTagRec.IsFinal)
            {
                strTagRec.Status = STRDWSStatus.SiloChecked.ToString();
            }
            else if (strTagRec.IsCancel)
            {
                strTagRec.Status = STRDWSStatus.Cancelled.ToString();
            }

            DBSaveStatus status = DBLayer.SaveSTRTagData(strTagRec);

            if (status == DBSaveStatus.Success && strTagRec.IsFinal)
            {
                // cyclic count check has already happened in the action
                DBLayer.UpdateSTRWeightStatus(strTagRec.STRWeightRecId, strTagRec.STRWeightCyclicCount, strTagRec.Status, strTagRec.CurrentUser);

                // mark all DWS as Ready for approval
                if (strTagRec.STRWeightRecId > 0)
                {
                    DBLayer.UpdateDWSStatus(strTagRec.Id, STRDWSStatus.SiloChecked, strTagRec.CurrentUser);
                    DBLayer.CalculateDWSOnSiloCheck(strTagRec.STRWeightRecId, strTagRec.Id, strTagRec.CurrentUser);
                }
            }

            return status;
        }

        public static void CreateSTRTagData(STRTag strTagRec)
        {
            DBLayer.CreateSTRTagData(strTagRec);
        }

        public static DBSaveStatus CreateSTR(STR strRec)
        {
            return DBLayer.CreateSTR(strRec);
        }

        public static DBSaveStatus CreateDWS(DWS dwsRec)
        {
            DBSaveStatus status = DBLayer.CreateDWS(dwsRec);
            if (status != DBSaveStatus.Success)
            {
                return status;
            }
            DBLayer.RecalculateSTRTotals(dwsRec.STRId, dwsRec.CurrentUser);

            return DBSaveStatus.Success;
        }

        /// <summary>
        /// Get Data for Dashboard chart;
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static EORP GetEORPData(SearchCriteria searchCriteria)
        {
            // get data from database now.
            ICollection<EORPData> expenseData = Business.GetExpenseData(searchCriteria);
            ICollection<EORPData> orderData = DBLayer.GetOrderData(searchCriteria);
            ICollection<EORPData> returnOrderData = DBLayer.GetReturnOrderData(searchCriteria);
            ICollection<EORPData> paymentData = DBLayer.GetPaymentData(searchCriteria);

            return AccumulateEORPData(searchCriteria.DateFrom, searchCriteria.DateTo, expenseData, orderData, returnOrderData, paymentData);
        }

        /// <summary>
        /// This method is marked as public for unit test
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="expenseData"></param>
        /// <param name="orderData"></param>
        /// <param name="returnOrderData"></param>
        /// <param name="paymentData"></param>
        /// <returns></returns>
        public static EORP AccumulateEORPData(
                            DateTime startDate,
                            DateTime endDate,
                            ICollection<EORPData> expenseData,
                            ICollection<EORPData> orderData,
                            ICollection<EORPData> returnOrderData,
                            ICollection<EORPData> paymentData)
        {
            EORP eorp = new EORP()
            {
                StartDate = startDate,
                EndDate = endDate,
                DayCount = endDate.Subtract(startDate).Days + 1,
                EORPSummary = new EORPSummary()
                {
                    ExpenseTotal = 0,
                    OrderTotal = 0,
                    ReturnOrderTotal = 0,
                    PaymentTotal = 0,
                    ExpenseAverage = 0,
                    OrderAverage = 0,
                    ReturnOrderAverage = 0,
                    PaymentAverage = 0
                },
                EORPMonthlySummary = null,
                EORPDays = null
            };

            // get distinct dates for which any data is available
            List<DateTime> allDates = new List<DateTime>();
            allDates.AddRange(expenseData.Select(x => x.Date));
            allDates.AddRange(orderData.Select(x => x.Date));
            allDates.AddRange(returnOrderData.Select(x => x.Date));
            allDates.AddRange(paymentData.Select(x => x.Date));

            IEnumerable<DateTime> distinctDates = allDates.Distinct().OrderBy(x => x).ToList();

            // create response items;
            eorp.EORPDays = distinctDates.Select(x => new EORPDay() { Date = x }).ToList();

            // fill expense Data in output structure
            foreach (EORPData data in expenseData)
            {
                EORPDay dayRecord = eorp.EORPDays.Where(x => x.Date == data.Date).FirstOrDefault();
                if (dayRecord != null)
                {
                    dayRecord.ExpenseAmount = data.TotalAmountForDay;
                    dayRecord.ExpenseCount = data.TotalItemCountForDay;
                }
            }

            // fill order Data in output structure
            foreach (EORPData data in orderData)
            {
                EORPDay dayRecord = eorp.EORPDays.Where(x => x.Date == data.Date).FirstOrDefault();
                if (dayRecord != null)
                {
                    dayRecord.OrderAmount = data.TotalAmountForDay;
                    dayRecord.OrderCount = data.TotalItemCountForDay;
                }
            }

            // fill ReturnOrder in output structure
            foreach (EORPData data in returnOrderData)
            {
                EORPDay dayRecord = eorp.EORPDays.Where(x => x.Date == data.Date).FirstOrDefault();
                if (dayRecord != null)
                {
                    dayRecord.ReturnOrderAmount = data.TotalAmountForDay;
                    dayRecord.ReturnOrderCount = data.TotalItemCountForDay;
                }
            }

            // fill Payment Data in output structure
            foreach (EORPData data in paymentData)
            {
                EORPDay dayRecord = eorp.EORPDays.Where(x => x.Date == data.Date).FirstOrDefault();
                if (dayRecord != null)
                {
                    dayRecord.PaymentAmount = data.TotalAmountForDay;
                    dayRecord.PaymentCount = data.TotalItemCountForDay;
                }
            }

            // Sum up the totals for summary
            eorp.EORPSummary.ExpenseTotal = eorp.EORPDays.Sum(x => x.ExpenseAmount); // expenseData.Sum(x => x.TotalAmountForDay);
            eorp.EORPSummary.ExpenseAverage = eorp.EORPSummary.ExpenseTotal / eorp.DayCount;

            eorp.EORPSummary.OrderTotal = eorp.EORPDays.Sum(x => x.OrderAmount); // orderData.Sum(x => x.TotalAmountForDay);
            eorp.EORPSummary.OrderAverage = eorp.EORPSummary.OrderTotal / eorp.DayCount;

            eorp.EORPSummary.ReturnOrderTotal = eorp.EORPDays.Sum(x => x.ReturnOrderAmount); // returnOrderData.Sum(x => x.TotalAmountForDay);
            eorp.EORPSummary.ReturnOrderAverage = eorp.EORPSummary.ReturnOrderTotal / eorp.DayCount;

            eorp.EORPSummary.PaymentTotal = eorp.EORPDays.Sum(x => x.PaymentAmount); // paymentData.Sum(x => x.TotalAmountForDay);
            eorp.EORPSummary.PaymentAverage = eorp.EORPSummary.PaymentTotal / eorp.DayCount;

            // calculate numbers by month
            eorp.EORPMonthlySummary = allDates.Select(x => new DateTime(x.Year, x.Month, 1))
                                        .Distinct()
                                        .OrderBy(x => x)
                                        .Select(x => new EORPMonth() { Date = x })
                                        .ToList();
            foreach (EORPMonth eorpMonthData in eorp.EORPMonthlySummary)
            {
                List<EORPDay> dataForMonth = eorp.EORPDays
                    .Where(x => x.Date.Month == eorpMonthData.Date.Month && x.Date.Year == eorpMonthData.Date.Year)
                    .ToList();

                eorpMonthData.OrderAmount = dataForMonth.Sum(x => x.OrderAmount);
                eorpMonthData.ExpenseAmount = dataForMonth.Sum(x => x.ExpenseAmount);
                eorpMonthData.PaymentAmount = dataForMonth.Sum(x => x.PaymentAmount);
                eorpMonthData.ReturnOrderAmount = dataForMonth.Sum(x => x.ReturnOrderAmount);
            }

            return eorp;
        }

        public static void ToggleExecAppAccess(long empId)
        {
            DBLayer.ToggleExecAppAccess(empId);
        }

        public static bool SaveEmployeeConfigData(ConfigEmployeeModel saveModel)
        {
            return DBLayer.SaveEmployeeConfigData(saveModel);
        }

        public static void CreateFeatureControl(string userName)
        {
            DBLayer.CreateFeatureControl(userName);
        }

        public static void TogglePhoneLog(long employeeId)
        {
            DBLayer.TogglePhoneLog(employeeId);
        }

        /// <summary>
        /// Get order items for multiple orders - used in downloading detail order report
        /// from search result page;
        /// </summary>
        public static IEnumerable<OrderItem> GetOrderItems(ICollection<Order> orders)
        {
            return DBLayer.GetOrderItems(orders.Select(x => x.Id).ToList());
        }

        public static IEnumerable<EntityAgreement> GetEntityAgreements(ICollection<Entity> entities)
        {
            return DBLayer.GetEntityAgreements(entities.Select(x => x.Id).ToList());
        }

        /// <summary>
        /// check to see if user is allowed to access dashboard
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsUserAllowed(string staffCode)
        {
            // first check that user must be active in table coming from SAP
            SalesPerson sp = DBLayer.GetSingleSalesPerson(staffCode);
            EmployeeRecord er = DBLayer.GetSingleTenantEmployee(staffCode);

            return (sp != null && sp.IsActive && er != null && er.IsActive);
        }

        public static bool IsUserAllowed(long employeeId)
        {
            TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            if (te == null)
            {
                return false;
            }

            return IsUserAllowed(te.EmployeeCode);
        }

        public static string GetTenantEmployeeName(string staffCode)
        {
            EmployeeRecord er = DBLayer.GetSingleTenantEmployee(staffCode);
            return er?.Name ?? "";
        }

        public static EmployeeRecord GetTenantEmployee(string staffCode)
        {
            return DBLayer.GetSingleTenantEmployee(staffCode);
        }

        public static IEnumerable<ConfigCodeTable> GetCodeTableData(ConfigCodeTable searchCriteria)
        {
            return DBLayer.GetCodeTableData(searchCriteria);
        }

        public static ConfigCodeTable GetSingleCodeTableData(long codeTableId)
        {
            return DBLayer.GetSingleCodeTableData(codeTableId);
        }

        public static void SaveTerminateAgreementRequestStatus(TerminateAgreementRequest terminateAgreement, string currentUser)
        {
            DBLayer.SaveTerminateAgreementRequestStatus(terminateAgreement, currentUser);
            if ("approved".Equals(terminateAgreement.Status, StringComparison.OrdinalIgnoreCase))
            {
                EntityAgreement ea = DBLayer.GetSingleAgreement(terminateAgreement.EntityAgreementId);
                if (ea != null)
                {
                    ea.Status = "Terminated";
                    Business.SaveAgreement(ea, "TerminateAgreement");
                }
                else
                {
                    Business.LogError($"{nameof(SaveTerminateAgreementRequestStatus)}", $"Could not termiate agreement id {terminateAgreement.EntityAgreementId} for associated Red Farmer Request Approval {terminateAgreement.Id}");
                }
            }
        }

        public static void SaveCodeTableData(ConfigCodeTable codeTableRec)
        {
            DBLayer.SaveCodeTableData(codeTableRec);
        }

        public static void CreateCodeTableData(ConfigCodeTable codeTableRec)
        {
            DBLayer.CreateCodeTableData(codeTableRec);
        }

        public static List<string> GetUniqueCodeTypes()
        {
            return DBLayer.GetUniqueCodeTypes();
        }

        public static IEnumerable<Tenant> GetTenants()
        {
            return DBLayer.GetTenants();
        }

        public static IEnumerable<DashboardLeave> GetDownloadLeaves(long employeeId)
        {
            //TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            //if (te == null)
            //{
            //    return new List<DashboardLeave>();
            //}

            return DBLayer.GetLeaves(employeeId);
        }
        //Added by Swetha -Mar 15
        public static ICollection<LeaveTypes> GetDownloadLeaveTypes(string staffCode)
        {
            return DBLayer.GetLeaveTypes(staffCode);
        }
        //Added by Swetha -Mar 16
        public static IEnumerable<HolidayList> GetDownloadHolidayList(string staffCode)
        {
            return DBLayer.GetHolidayList(staffCode);

        }
        public static IEnumerable<DownloadAvailableLeaves> GetDownloadAvailableLeave(string staffcode)
        {
            return DBLayer.GetAvailableLeaves(staffcode);
        }
        public static IEnumerable<DashboardBankAccount> GetDownloadBankAccounts(long employeeId, string inputAreaCode)
        {
            TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            if (te == null)
            {
                return new List<DashboardBankAccount>();
            }

            string areaCode = inputAreaCode ?? "";

            // find out area codes for employee
            IEnumerable<CodeTableEx> tenantAreaCodes = Business.GetAreaCodes(te.EmployeeCode);
            if (tenantAreaCodes == null || tenantAreaCodes.Count() == 0)
            {
                return new List<DashboardBankAccount>();
            }

            // input area code has to exist in tenantAreaCodes
            if (tenantAreaCodes.Any(x => x.Code == areaCode) == false)
            {
                areaCode = tenantAreaCodes.First().Code;
            }

            // get bank accounts for area code now.
            BankAccountFilter filter = new BankAccountFilter()
            {
                ApplyAreaCodeFilter = true,
                ApplyBankNameFilter = false,
                AreaCode = areaCode
            };

            return DBLayer.GetBankAccounts(filter);
        }

        // use to get download product data, when downloaded data is not compressed.
        public static IEnumerable<DownloadProductEx> GetDownloadProducts(long employeeId, string inputAreaCode, DateTime gstRateDate)
        {
            TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            if (te == null)
            {
                return new List<DownloadProductEx>();
            }

            string areaCode = inputAreaCode ?? "";

            // find out area codes for employee
            IEnumerable<CodeTableEx> tenantAreaCodes = Business.GetAreaCodes(te.EmployeeCode);
            if (tenantAreaCodes == null || tenantAreaCodes.Count() == 0)
            {
                return new List<DownloadProductEx>();
            }

            // input area code has to exist in tenantAreaCodes
            if (tenantAreaCodes.Any(x => x.Code == areaCode) == false)
            {
                areaCode = tenantAreaCodes.First().Code;
            }

            return GetDownloadProductsForArea(areaCode, gstRateDate);
        }

        public static IEnumerable<DownloadProductEx> GetDownloadProductsForArea(string areaCode, DateTime gstRateDate)
        {
            ICollection<DownloadProductEx> downloadProductData = DBLayer.GetDownloadProductsForArea(areaCode);

            GstRateFilter filter = new GstRateFilter()
            {
                ApplyGstCodeFilter = false,
                ApplyDateFilter = true,
                SearchDate = gstRateDate.Date
            };

            // fill GST percent
            IEnumerable<DashboardGstRate> currentGstRates = DBLayer.GetGstRate(filter);
            Parallel.ForEach(downloadProductData, dpd =>
            {
                dpd.GstPercent = currentGstRates.FirstOrDefault(x => x.GstCode.Equals(dpd.GstCode, StringComparison.OrdinalIgnoreCase))?.GstRate ?? 0;
            });

            return downloadProductData;
        }

        public static ICollection<DownloadProductEx> GetProductsWithPrice2(long employeeId, string inputAreaCode, DateTime gstRateDate)
        {
            TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            if (te == null)
            {
                return new List<DownloadProductEx>();
            }

            return GetProductsWithPrice2(te.EmployeeCode, inputAreaCode, gstRateDate);
        }

        // use to get download product data, when downloaded data is compressed.
        public static ICollection<DownloadProductEx> GetProductsWithPrice2(string employeeCode, string inputAreaCode, DateTime gstRateDate)
        {
            Stopwatch sw = new Stopwatch();

            sw.Reset();
            sw.Start();
            // create product data
            var VolumeProducts = Business.GetProducts2(gstRateDate);
            sw.Stop();
            Business.LogError($"{nameof(GetProductsWithPrice2)}", $"Employee {employeeCode}, Getting {VolumeProducts.Count()} products from DB took {sw.ElapsedMilliseconds / 1000} seconds [{sw.ElapsedMilliseconds} ms]");

            // create product price data
            sw.Reset();
            sw.Start();
            var VolumeProductPrices = Business.GetProductPrice2(employeeCode, inputAreaCode);
            sw.Stop();
            Business.LogError($"{nameof(GetProductsWithPrice2)}", $"Employee {employeeCode}, Getting {VolumeProductPrices.Count()} product price from DB took {sw.ElapsedMilliseconds / 1000} seconds [{sw.ElapsedMilliseconds} ms]");

            sw.Reset();
            sw.Start();
            var returnList = (from p in VolumeProducts
                              join productPrice in VolumeProductPrices on p.Id equals productPrice.ProductId
                              //let productPrice = VolumeProductPrices.Where(x => x.ProductId == p.Id).FirstOrDefault()
                              //where productPrice != null
                              select new DownloadProductEx()
                              {
                                  Name = p.Name,
                                  Code = p.Code,
                                  UOM = p.UOM,
                                  MRP = productPrice.MRP,
                                  GroupName = p.GroupName,
                                  IsActive = true,
                                  Stock = productPrice.Stock,
                                  GstCode = p.GstCode,
                                  GstPercent = p.GstPercent,
                                  PriceList = new List<DownloadProductPrice>()
                            {
                                new DownloadProductPrice() { CustomerType = "DEALER", BillingPrice = productPrice.DEALERPrice },
                                new DownloadProductPrice() { CustomerType = "DISTRIBUTOR", BillingPrice = productPrice.DISTPrice },
                                new DownloadProductPrice() { CustomerType = "P.DISTRIBUTOR", BillingPrice = productPrice.PDISTPrice }
                            },
                                  AreaCode = inputAreaCode
                              }).OrderBy(x => x.Name).ToList();
            sw.Stop();
            Business.LogError($"{nameof(GetProductsWithPrice2)}", $"Employee {employeeCode}, Formatting download product data took {sw.ElapsedMilliseconds / 1000} seconds [{sw.ElapsedMilliseconds} ms]");
            return returnList;
        }

        /// <summary>
        /// Only Products are returned - used when compression is applied as well, during download.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="inputAreaCode"></param>
        /// <param name="gstRateDate"></param>
        /// <returns></returns>
        public static IEnumerable<Product2> GetProducts2(DateTime gstRateDate)
        {
            ICollection<Product2> downloadProductData = DBLayer.GetProducts2();

            GstRateFilter filter = new GstRateFilter()
            {
                ApplyGstCodeFilter = false,
                ApplyDateFilter = true,
                SearchDate = gstRateDate.Date
            };

            // fill GST percent
            IEnumerable<DashboardGstRate> currentGstRates = DBLayer.GetGstRate(filter);
            Parallel.ForEach(downloadProductData, dpd =>
            {
                dpd.GstPercent = currentGstRates.FirstOrDefault(x => x.GstCode.Equals(dpd.GstCode, StringComparison.OrdinalIgnoreCase))?.GstRate ?? 0;
            });

            return downloadProductData;
        }

        /// <summary>
        /// Returns only Product Price - used when downloaded data is also compressed;
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="inputAreaCode"></param>
        /// <returns></returns>
        public static IEnumerable<ProductPrice2> GetProductPrice2(string employeeCode, string inputAreaCode)
        {
            //TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            //if (te == null)
            //{
            //    return new List<ProductPrice2>();
            //}

            string areaCode = inputAreaCode ?? "";

            // find out area codes for employee
            IEnumerable<CodeTableEx> tenantAreaCodes = Business.GetAreaCodes(employeeCode);
            if (tenantAreaCodes == null || tenantAreaCodes.Count() == 0)
            {
                return new List<ProductPrice2>();
            }

            // input area code has to exist in tenantAreaCodes
            if (tenantAreaCodes.Any(x => x.Code == areaCode) == false)
            {
                areaCode = tenantAreaCodes.First().Code;
            }

            return DBLayer.GetProductPrice2(areaCode);
        }

        public static Payment GetPayment(long paymentId)
        {
            return DBLayer.GetPayment(paymentId);
        }

        public static ICollection<Payment> GetPayments(DomainEntities.SearchCriteria searchCriteria)
        {
            return DBLayer.GetPayments(searchCriteria);
        }

        private static WorkFlowStatus GetWorkFlowStatus(DomainEntities.EntityWorkFlowDetail entityWorkFlowDetail, DateTime timeInIST)
        {
            if (entityWorkFlowDetail.IsComplete && entityWorkFlowDetail.PlannedEndDate.Date < entityWorkFlowDetail.CompletedOn.Value.Date)
            {
                return WorkFlowStatus.OverdueCompleted;
            }
            else if (entityWorkFlowDetail.IsComplete && entityWorkFlowDetail.PlannedEndDate.Date.Date >= entityWorkFlowDetail.CompletedOn.Value.Date && entityWorkFlowDetail.PlannedFromDate.Date <= entityWorkFlowDetail.CompletedOn.Value.Date)
            {
                return WorkFlowStatus.Completed;
            }
            else if (entityWorkFlowDetail.IsComplete && entityWorkFlowDetail.PlannedFromDate.Date > entityWorkFlowDetail.CompletedOn.Value.Date)
            {
                return WorkFlowStatus.Completed;
                //return WorkFlowStatus.CompletedAhead.ToString();
            }
            else if (entityWorkFlowDetail.IsComplete == false && entityWorkFlowDetail.PlannedEndDate.Date.Date < timeInIST.Date)
            {
                return WorkFlowStatus.Overdue;
            }
            else if (entityWorkFlowDetail.IsComplete == false && entityWorkFlowDetail.PlannedFromDate.Date > timeInIST.Date)
            {
                return WorkFlowStatus.Waiting;
            }
            else
            {
                return WorkFlowStatus.Due;
            }
        }

        public static Dictionary<long, string> GetS3Buckets()
        {
            Dictionary<long, string> s3Buckets = new Dictionary<long, string>();
            //s3BucketItems.Add(1, Utils.SiteConfigData.S3Bucket);
            //s3BucketItems.Add(2, Utils.S3DebugUploadBucket);

            s3Buckets.Add(1, "Image Bucket");
            s3Buckets.Add(2, "Global Debug Bucket");

            return s3Buckets;
        }

        public static string GetActualS3BucketName(long id)
        {
            string bucketName = "";
            if (id == 1)
            {
                bucketName = Utils.SiteConfigData.S3Bucket;
            }
            else if (id == 2)
            {
                bucketName = Utils.S3DebugUploadBucket;
            }

            return bucketName;
        }

        public static ICollection<EntityWorkFlowDetail> GetEntityWorkFlowDetails(DomainEntities.SearchCriteria searchCriteria)
        {
            ICollection<EntityWorkFlowDetail> entityWorkFlowDetails = DBLayer.GetEntityWorkFlowDetails(searchCriteria);
            foreach (var item in entityWorkFlowDetails)
            {
                WorkFlowStatus wfs = GetWorkFlowStatus(item, searchCriteria.CurrentISTTime);
                item.Status = wfs.ToString();

                // Apply work flow status filter
                if (searchCriteria.ApplyWorkFlowStatusFilter && wfs != searchCriteria.WorkFlowStatus)
                {
                    item.IsVisibleOnSearch = false;
                }
            }
            return entityWorkFlowDetails;
        }

        public static ICollection<EntityProgressDetail> GetEntityProgressDetails(SearchCriteria searchCriteria)
        {
            return DBLayer.GetEntityProgressDetails(searchCriteria);
        }

        public static ICollection<STRTag> GetSTRTag(STRFilter searchCriteria)
        {
            return DBLayer.GetSTRTag(searchCriteria);
        }

        public static ICollection<STRWeight> GetSTRWeight(STRFilter searchCriteria)
        {
            return DBLayer.GetSTRWeight(searchCriteria);
        }

        public static ICollection<STR> GetSTR(long strTagId)
        {
            return DBLayer.GetSTR(strTagId);
        }

        public static ICollection<DWS> GetDWS(long strId)
        {
            return DBLayer.GetDWS(strId);
        }

        public static ICollection<DWSAudit> GetDWSAudit(long strId)
        {
            return DBLayer.GetDWSAudit(strId);
        }

        public static ICollection<DWS> GetDWS(DomainEntities.DWSFilter searchCriteria)
        {
            return DBLayer.GetDWS(searchCriteria);
        }

        public static ICollection<Entity> GetEntities(DomainEntities.EntitiesFilter searchCriteria)
        {
            return DBLayer.GetEntities(searchCriteria);
        }

        public static ICollection<UnSownData> GetUnSownData(DomainEntities.EntitiesFilter searchCriteria)
        {
            return DBLayer.GetUnSownData(searchCriteria);
        }

        public static ICollection<DomainEntities.EntityAgreement> GetEntityAgreementData(DomainEntities.EntitiesFilter criteria, string activityType)
        {
            var agreementList = DBLayer.GetEntityAgreements(criteria.AgreementStatus, activityType);
            if (criteria.ApplyCropFilter)
            {
                return agreementList.Where(x => x.TypeName.Equals(criteria.Crop, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return agreementList;
        }

        public static ICollection<TerminateAgreementRequest> GetTerminateAgreementRequests(TerminateAgreementRequestFilter searchCriteria)
        {
            return DBLayer.GetTerminateAgreementRequests(searchCriteria);
        }

        public static DomainEntities.TerminateAgreementRequest GetSingleTerminateAgreementRequest(long id)
        {
            DomainEntities.TerminateAgreementRequest terminateAgreement = DBLayer.GetSingleTerminateAgreementRequest(id);

            return terminateAgreement;
        }

        public static ICollection<AdvanceRequest> GetAdvanceRequests(AdvanceRequestFilter searchCriteria)
        {
            return DBLayer.GetAdvanceRequests(searchCriteria);
        }

        public static ICollection<EntityContact> GetEntityContacts(long entityId)
        {
            return DBLayer.GetEntityContacts(entityId);
        }

        public static ICollection<EntityAgreement> GetEntityAgreements(long entityId)
        {
            return DBLayer.GetEntityAgreements(entityId);
        }

        public static ICollection<EntitySurvey> GetEntitySurveys(long entityId)
        {
            return DBLayer.GetEntitySurveys(entityId);
        }

        public static ICollection<EntityBankDetail> GetEntityBankDetails(long entityId)
        {
            return DBLayer.GetEntityBankDetails(entityId);
        }

        public static EntityBankDetail GetSingleBankDetail(long entityBankDetailId)
        {
            return DBLayer.GetSingleBankDetail(entityBankDetailId);
        }

        public static EntityAgreement GetSingleAgreement(long entityAgreementId)
        {
            // at the time of agreement create, passbook received date is set to current date;
            // but since passbook is not received, at the time of edit, it gives user a message
            // that passbook received date can't be given; It became a bug, since for PJM, we
            // are not allowing the edit of Passbook received flag/date. Hence clear it out here.

            EntityAgreement ea = DBLayer.GetSingleAgreement(entityAgreementId);
            if (ea != null && ea.IsPassbookReceived == false)
            {
                ea.PassbookReceivedDate = DateTime.MinValue;
            }
            return ea;
        }

        public static EntitySurvey GetSingleSurvey(long entitySurveyId) =>
            DBLayer.GetSingleSurvey(entitySurveyId);

        public static ICollection<WorkflowSeason> GetOpenWorkflowSeasons()
        {
            return DBLayer.GetOpenWorkflowSeasons();
        }

        public static ICollection<EntityCrop> GetEntityCrops(long entityId)
        {
            return DBLayer.GetEntityCrops(entityId);
        }

        public static string ValidateAreaCode(long employeeId, string inputAreaCode)
        {
            TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            if (te == null)
            {
                return "";
            }

            string areaCode = inputAreaCode ?? "";

            // find out area codes for employee
            IEnumerable<CodeTableEx> tenantAreaCodes = Business.GetAreaCodes(te.EmployeeCode);
            if (tenantAreaCodes == null || tenantAreaCodes.Count() == 0)
            {
                return "";
            }

            // input area code has to exist in tenantAreaCodes - if it does not, take first area code
            // that is assigned to tenant; this situation can happen if admin changes the association
            // while user is selecting area on phone;
            if (tenantAreaCodes.Any(x => x.Code == areaCode) == false)
            {
                areaCode = tenantAreaCodes.First().Code;
            }

            return areaCode;
        }

        public static IEnumerable<DownloadCustomerExtend> GetDownloadCustomers(string staffCode, string areaCode)
        {
            return GetCustomersForAreaPlusStaffCode(areaCode, staffCode);
        }

        public static ICollection<CustomerDivisionBalance> GetDownloadCustomerDivisionBalance(
                                string staffCode, string inputAreaCode)
        {
            string areaCode = inputAreaCode ?? "";

            // find out area codes for employee
            IEnumerable<CodeTableEx> tenantAreaCodes = Business.GetAreaCodes(staffCode);
            if (tenantAreaCodes == null || tenantAreaCodes.Count() == 0 || string.IsNullOrEmpty(areaCode))
            {
                return new List<CustomerDivisionBalance>();
            }

            // input area code has to exist in tenantAreaCodes - if it does not, take first area code
            // that is assigned to tenant; this situation can happen if admin changes the association
            // while user is selecting area on phone;
            if (tenantAreaCodes.Any(x => x.Code == areaCode) == false)
            {
                areaCode = tenantAreaCodes.First().Code;
            }

            return GetCustomerDivisionBalance(areaCode, staffCode);
        }

        public static IEnumerable<DownloadEntity> GetDownloadEntities(string staffCode, string inputAreaCode)
        {
            //TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            //if (te == null)
            //{
            //    return new List<DownloadEntity>();
            //}

            //string areaCode = inputAreaCode ?? "";

            //// find out area codes for employee
            //IEnumerable<CodeTableEx> tenantAreaCodes = Business.GetAreaCodes(te.EmployeeCode);
            //if (tenantAreaCodes == null || tenantAreaCodes.Count() == 0)
            //{
            //    return new List<DownloadEntity>();
            //}

            //// input area code has to exist in tenantAreaCodes - if it does not, take first area code
            //// that is assigned to tenant; this situation can happen if admin changes the association
            //// while user is selecting area on phone;
            //if (tenantAreaCodes.Any(x => x.Code == areaCode) == false)
            //{
            //    areaCode = tenantAreaCodes.First().Code;
            //}

            // retrieve open seasons
            //long seasonId = 0;
            //if (Utils.SiteConfigData.IsEntityAgreementEnabled)
            //{
            //    ICollection<DomainEntities.WorkflowSeason> openSeasons = DBLayer.GetOpenWorkflowSeasons();
            //    seasonId = openSeasons?.FirstOrDefault(x => x.SeasonName.Equals(seasonName, StringComparison.OrdinalIgnoreCase))
            //                                ?.Id ?? 0;
            //}

            long tenantId = Utils.SiteConfigData.TenantId;
            return DBLayer.GetEntitiesForAreaPlusStaffCode(tenantId, inputAreaCode,
                staffCode, new List<string>() { "Approved", "Pending" },
                new List<string>() { "Approved", "Pending" },
                new List<string>() { "Approved", "Pending", "Partially Approved" }
                );
        }

        // Added by swetha- To get Entity Contacts for download
        public static IEnumerable<EntityContact> GetEntityContacts(ICollection<Entity> entities)
        {
            return DBLayer.GetEntityContactDetails(entities.Select(x => x.Id).ToList());
        }

        // Added by swetha- To get Entity Crops for download
        public static IEnumerable<EntityCrop> GetEntityCrops(ICollection<Entity> entities)
        {
            return DBLayer.GetEntityCropDetails(entities.Select(x => x.Id).ToList());
        }
        // List all S3 buckets
        public static ICollection<string> S3BucketNames()
        {
            return S3Facade.GetAllBucketNames(Utils.S3Client);
        }

        public static ICollection<string> S3BucketEntries(string bucketName)
        {
            S3Facade.EnsureBucketExist(bucketName, Utils.S3Client);

            return S3Facade.BucketEntries(bucketName, Utils.S3Client)
                ?? new List<string>();
        }

        public static ICollection<string> LocalImageEntries()
        {
            try
            {
                string localFolder = Utils.SiteConfigData.ImagesFolder;
                var files = System.IO.Directory.GetFiles(localFolder);
                return files.ToList();
            }
            catch (Exception ex)
            {
                LogError($"{nameof(LocalImageEntries)}", ex);
                return new List<string>();
            }
        }

        public static Tuple<bool, string, string> GetPreSignedLink(string fileNameWithPath, TimeSpan validity)
        {
            string bucketName = Utils.S3DebugUploadBucket;
            bool status = S3Facade.SaveFileInS3(fileNameWithPath, bucketName, Utils.S3Client, LocalDiskFacade.GetFileStream);

            if (status == false)
            {
                return new Tuple<bool, string, string>(false, "Not able to upload file to S3", "");
            }

            // status, message, link
            Tuple<bool, string, string> response = S3Facade.GetPresignedUrl(fileNameWithPath, bucketName, Utils.S3Client, validity);
            return response;
        }

        private static void UploadImagesToS3(long tenantId, bool deleteFileAfterUpload, int maxImagesToUpload)
        {
            string bucketName = Utils.SiteConfigData.S3Bucket;

            try
            {
                string localFolder = Utils.SiteConfigData.ImagesFolder;
                var files = System.IO.Directory.GetFiles(localFolder);

                int daysToKeepFileOnLocal = Utils.SiteConfigData.ArchiveImageAfterDays;
                DateTime currentUtcTime = DateTime.UtcNow;

                int itemsProcessed = 0;
                foreach (var f in files)
                {
                    // retrieve last write time
                    DateTime fileCreateTime = System.IO.Directory.GetLastWriteTimeUtc(f);
                    long fileSize = new System.IO.FileInfo(f).Length;
                    // How old is this file
                    if (daysToKeepFileOnLocal > 0)
                    {
                        // if days to keep is given as zero - don't keep file on local
                        // (it means upload all)
                        if (currentUtcTime.Subtract(fileCreateTime).Days <= daysToKeepFileOnLocal)
                        {
                            continue;
                        }
                    }

                    itemsProcessed++;
                    if (fileSize > 0)
                    {
                        try
                        {
                            DBLayer.RenewLeaseForImageUploadToS3(tenantId);
                            //bool status = false;

                            bool status = S3Facade.SaveFileInS3(f, bucketName, Utils.S3Client, LocalDiskFacade.GetImageMemoryStream);
                            Business.LogError($"{nameof(UploadImagesToS3)}", $"{f} upload status: {status}");

                            if (status && deleteFileAfterUpload)
                            {
                                System.IO.File.Delete(f);
                                Business.LogError($"{nameof(UploadImagesToS3)}", $"{f} deleted");
                            }
                        }
                        catch (Exception ex)
                        {
                            LogError($"{nameof(UploadImagesToS3)}", ex);
                        }
                    }

                    // respect the max items to process count
                    if (itemsProcessed >= maxImagesToUpload)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"{nameof(UploadImagesToS3)}", ex);
            }
        }

        public static IEnumerable<DomainEntities.EntityWorkFlow> GetDownloadEntitiesWorkFlow(
                            string staffCode, string inputAreaCode)
        {
            //TenantEmployee te = DBLayer.GetEmployeeRecord(employeeId);
            //if (te == null)
            //{
            //    return new List<DomainEntities.EntityWorkFlow>();
            //}

            //string areaCode = inputAreaCode ?? "";

            //// find out area codes for employee
            //IEnumerable<CodeTableEx> tenantAreaCodes = Business.GetAreaCodes(te.EmployeeCode);
            //if (tenantAreaCodes == null || tenantAreaCodes.Count() == 0)
            //{
            //    return new List<DomainEntities.EntityWorkFlow>();
            //}

            //// input area code has to exist in tenantAreaCodes - if it does not, take first area code
            //// that is assigned to tenant; this situation can happen if admin changes the association
            //// while user is selecting area on phone;
            //if (tenantAreaCodes.Any(x => x.Code == areaCode) == false)
            //{
            //    areaCode = tenantAreaCodes.First().Code;
            //}

            return DBLayer.GetEntitiesWorkFlowForAreaPlusStaffCodeV2(Utils.SiteConfigData.TenantId, inputAreaCode, staffCode);
        }

        public static IEnumerable<DownloadCustomerExtend> GetCustomersForAreaPlusStaffCode(string areaCode, string staffCode)
        {
            return DBLayer.GetCustomersForAreaPlusStaffCode(Utils.SiteConfigData.TenantId, areaCode, staffCode);
        }

        public static ICollection<PPA> GetPPAData(string staffCode)
        {
            return DBLayer.GetPPAData(Utils.SiteConfigData.TenantId, staffCode);
        }

        public static ICollection<PPA> GetPPAData(IEnumerable<string> staffCodes)
        {
            return DBLayer.GetPPAData(Utils.SiteConfigData.TenantId, staffCodes);
        }

        public static ICollection<StaffDivision> GetStaffDivisions()
        {
            return DBLayer.GetStaffDivisions();
        }

        public static ICollection<CustomerDivisionBalance> GetCustomerDivisionBalance(string areaCode, string staffCode)
        {
            return DBLayer.GetCustomerDivisionBalance(Utils.SiteConfigData.TenantId, areaCode, staffCode);
        }

        public static ICollection<CustomerDivisionBalance> GetCustomerDivisionBalance(IEnumerable<string> customerCodes)
        {
            return DBLayer.GetCustomerDivisionBalance(Utils.SiteConfigData.TenantId, customerCodes);
        }

        public static Expense GetExpense(long expenseId)
        {
            return DBLayer.GetExpense(expenseId);
        }

        public static ICollection<ExpenseApproval> GetExpenseApprovals(long expenseId)
        {
            return DBLayer.GetExpenseApprovals(expenseId);
        }

        private static int GetRankForCodeType(string codeType)
        {
            int rank = 0;
            switch (codeType)
            {
                case "Zone": rank = 40; break;
                case "AreaOffice": rank = 30; break;
                case "Territory": rank = 20; break;
                case "HeadQuarter": rank = 10; break;
                default: rank = 0; break;
            }
            return rank;
        }

        public static ICollection<TenantSmsSchedule> GetTenantSmsSchedule()
        {
            return DBLayer.GetTenantSmsSchedule();
        }

        public static ICollection<TenantWorkDay> GetTenantWorkDay()
        {
            return DBLayer.GetTenantWorkDay();
        }

        public static ICollection<TenantHoliday> GetTenantHolidays()
        {
            return DBLayer.GetTenantHolidays();
        }

        public static AdvanceRequest GetSingleAdvanceRequest(long id)
        {
            return DBLayer.GetSingleAdvanceRequest(id);
        }

        public static ICollection<Expense> GetExpenses(SearchCriteria searchCriteria = null)
        {
            IEnumerable<Expense> expenseItems = DBLayer.GetExpenses(searchCriteria);

            //==================================
            // apply security filters now;
            //==================================
            // find out unique Staff codes associated for each employee in the expenses list
            IEnumerable<string> uniqueStaffCodes = expenseItems.GroupBy(x => x.StaffCode).Select(x => x.Key).ToList();

            IEnumerable<string> exclusionList = Business.GetExclusionList(searchCriteria, uniqueStaffCodes);

            // now take only those staff codes that don't exist in exclusion list
            expenseItems = expenseItems.Where(x => exclusionList.Any(y => y == x.StaffCode) == false).ToList();

            return expenseItems.OrderByDescending(x => x.ExpenseDate).ThenBy(x => x.EmployeeName).ToList();
        }
         public static ICollection<BulkExpense> GetExpensesById(IEnumerable<long> expenseId)
        {
            ICollection<BulkExpense> expenseItems = DBLayer.GetExpensesById(expenseId);
            ICollection<BulkExpense> notapprovedexpenses = expenseItems.Where(x => x.Approvals.Count() == 0).ToList();
            return notapprovedexpenses;
        }



        public static void SaveAdvanceRequestReview(AdvanceRequest ar, string currentUser)
        {
            DomainEntities.AdvanceRequest origRec = DBLayer.GetSingleAdvanceRequest(ar.Id);
            ICollection<DomainEntities.EntityContact> contacts = DBLayer.GetEntityContacts(origRec.EntityId);
            DomainEntities.Entity entity = DBLayer.GetSingleEntity(origRec.EntityId);
            DomainEntities.SalesPerson sp = DBLayer.GetSingleSalesPerson(entity.EmployeeCode);

            SmsData sd = new SmsData()
            {
                FieldPersonName = entity.EmployeeName,
                AmountRequested = ar.Amount,
                EntityName = contacts.Where(x => x.IsPrimary).FirstOrDefault()?.Name ?? "",
                AmountApproved = ar.AmountApproved,
                AgreementNumber = origRec.AgreementNumber,
                TypeName = origRec.Crop
            };

            var tuple = GetAdvanceRequestStatus(ar.Amount, ar.AmountApproved);
            ar.AdvanceRequestStatus = tuple.Item1;
            DBLayer.SaveAdvanceRequestReview(ar, currentUser);

            // Create Sms Data
            sd.PhoneNumber = sp.Phone;
            Business.SaveSmsData(tuple.Item2, sd);
        }

        private static Tuple<string, string> GetAdvanceRequestStatus(decimal requestAmount, decimal approveAmount)
        {
            string status = string.Empty;
            string smsType = "";

            if (approveAmount < requestAmount)
            {
                status = (approveAmount == 0) ? "Denied" : "Partially Approved";
                smsType = (approveAmount == 0) ? "AdvanceDenied" : "PartAdvanceApproved";
            }
            else if (approveAmount == requestAmount)
            {
                status = "Approved";
                smsType = "FullAdvanceApproved";
            }

            return new Tuple<string, string>(status, smsType);
        }

        public static ICollection<IssueReturn> GetIssueReturns(SearchCriteria searchCriteria)
        {
            IEnumerable<IssueReturn> issueReturnItems = DBLayer.GetIssueReturns(searchCriteria);

            //==================================
            // apply security filters now;
            //==================================
            // find out unique Staff codes associated for each employee in the expenses list
            IEnumerable<string> uniqueStaffCodes = issueReturnItems.GroupBy(x => x.StaffCode).Select(x => x.Key).ToList();

            IEnumerable<string> exclusionList = Business.GetExclusionList(searchCriteria, uniqueStaffCodes);

            // now take only those staff codes that don't exist in exclusion list
            issueReturnItems = issueReturnItems.Where(x => exclusionList.Any(y => y == x.StaffCode) == false).ToList();

            return issueReturnItems.OrderByDescending(x => x.TransactionDate).ThenBy(x => x.EmployeeName).ToList();
        }

        public static void AddExecAppImeiRec(ExecAppImei rec)
        {
            DBLayer.AddExecAppImeiRec(rec);
        }

        public static void SaveExecAppImeiRec(ExecAppImei rec)
        {
            DBLayer.SaveExecAppImeiRec(rec);
        }

        public static ICollection<AspNetUser> GetAspNetUserData()
        {
            return DBLayer.GetAspNetUserData();
        }

        public static ICollection<FeatureControl> GetVirtualSuperAdminWithRights()
        {
            // List of users who are super admins created by SuperAdmin user
            //List<AspNetUser> virtualSuperAdmins = Business.GetAspNetUserData()
            //        .Where(x => x.Roles.Contains("Admin") && x.Roles.Contains("Manager"))
            //        .ToList();

            // Now get the rights
            return DBLayer.GetVirtualSuperAdminFeatureControl();
        }

        public static FeatureControl GetVirtualSuperAdminWithRights(string userName)
        {
            ICollection<FeatureControl> rights = Business.GetVirtualSuperAdminWithRights();
            FeatureControl fc = rights
                .Where(x => x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            return fc;
        }

        public static ICollection<EORPData> GetExpenseData(SearchCriteria searchCriteria)
        {
            IEnumerable<Expense> expenses = DBLayer.GetExpenseDataEORP(searchCriteria);

            IEnumerable<string> uniqueStaffCodes = expenses.GroupBy(x => x.StaffCode).Select(x => x.Key).ToList();

            IEnumerable<string> exclusionList = Business.GetExclusionList(searchCriteria, uniqueStaffCodes);

            // now take only those staff codes that don't exist in exclusion list
            expenses = expenses.Where(x => exclusionList.Any(y => y == x.StaffCode) == false).ToList();

            return expenses
               .GroupBy(x => x.ExpenseDate)
               .OrderBy(x => x.Key)
               .Select(y => new EORPData
               {
                   Date = y.Key,
                   TotalAmountForDay = y.Sum(z => z.TotalAmount),
                   TotalItemCountForDay = y.Count()
               }).ToList();
        }

        public static IEnumerable<DashboardGstRate> GetDashboardGstRate(GstRateFilter searchCriteria)
        {
            return DBLayer.GetGstRate(searchCriteria);
        }

        public static void SaveVirtualSuperAdminRights(FeatureControl fc)
        {
            DBLayer.SaveVirtualSuperAdminRights(fc);
        }

        public static long LogError(string processName, Exception ex)
        {
            if (ex == null)
            {
                return 0;
            }

            StringBuilder sb = new StringBuilder();
            while (ex != null)
            {
                sb.AppendLine(ex.ToString());
                ex = ex.InnerException;
            }

            return LogError(processName, sb.ToString(), ">");
        }

        public static long LogError(string processName, string logText, string logSnip = "")
        {
            if (Utils.SiteConfigData.LogErrorsToDb)
            {
                if (String.IsNullOrEmpty(logSnip)) // if caller has given a snip - respect that;
                {
                    logSnip = Utils.GetSnipForLog(logText);
                }

                return DBLayer.LogError(processName, logText, logSnip);
            }

            return 0;
        }

        public static void DeleteExecAppImei(long execAppImeiId)
        {
            DBLayer.DeleteExecAppImei(execAppImeiId);
        }

        public static IEnumerable<ExpenseItem> GetExpenseItems(long expenseId)
        {
            return DBLayer.GetExpenseItems(expenseId);
        }

        //Author:Kartik V, Purpose:Edit ExpenseItem for deducting amount; Date:08-03-2022
        public static ExpenseItem GetExpenseItem(long expenseId)
        {
            return DBLayer.GetExpenseItem(expenseId);
        }

        public static DBSaveStatus SaveExpenseItemData(ExpenseItem expenseItem)
        {
            DBSaveStatus status;

            status = DBLayer.SaveExpenseItemData(expenseItem);

            return status;
        }

        public static ICollection<DashboardGstRate> GetOverlappingGSTRates(string gstCode, DateTime trnStartDate, DateTime trnEndDate)
        {
            return DBLayer.GetOverlappingGSTRates(gstCode, trnStartDate, trnEndDate);
        }

        public static long CreateGstRate(GstSaveRate gr)
        {
            //var overlaps = Business.GetOverlappingGSTRates(gr.GstCode, gr.EffectiveStartDate, gr.EffectiveEndDate);
            //if (overlaps.Any())
            //{
            //    return 0;
            //}

            return DBLayer.CreateGSTRate(gr);
        }

        public static long UpdateGstRate(GstSaveRate gr)
        {
            // need to check for overlaps again, in case user left the edit screen open overnight
            //
            var overlaps = Business.GetOverlappingGSTRates(gr.GstCode, gr.EffectiveStartDate, gr.EffectiveEndDate);
            if (overlaps.Any(x => x.Id != gr.Id))
            {
                return 0;
            }

            return DBLayer.UpdateGstRate(gr);
        }

        public static ICollection<Order> GetOrders(SearchCriteria searchCriteria)
        {
            return DBLayer.GetOrders(searchCriteria);
        }

        public static IEnumerable<OrderItem> GetOrderItems(long orderId)
        {
            return DBLayer.GetOrderItems(orderId);
        }

        public static ICollection<CodeTableEx> GetCodeTable(string codeTableType, long tenantId = -1)
        {
            tenantId = (tenantId == -1) ? Utils.SiteConfigData.TenantId : tenantId;
            // retrieve code table data for tenant id - that is set in UrlResolve
            return DBLayer.GetCodeTable(tenantId, codeTableType);
        }

        public static DashboardGstRate GetGstRateDetails(long id)
        {
            return DBLayer.GetGstRateData(id);
        }

        public static ICollection<string> GetDivisionCodes(string staffCode)
        {
            return DBLayer.GetDivisionCodes(staffCode);
        }

        public static IEnumerable<CodeTableEx> GetZones(bool isSuperAdmin, IEnumerable<OfficeHierarchy> officeHierarchy)
        {
            if (isSuperAdmin)
            {
                return Business.GetCodeTable("Zone");
            }

            return officeHierarchy
                .Select(x => new { CodeName = x.ZoneName, Code = x.ZoneCode })
                .GroupBy(x => x.Code)
                .Select(x => new CodeTableEx { Code = x.Key, CodeName = x.First().CodeName })
                .ToList();
        }

        public static IEnumerable<CodeTableEx> GetAreas(bool isSuperAdmin, IEnumerable<OfficeHierarchy> officeHierarchy)
        {
            if (isSuperAdmin)
            {
                return Business.GetCodeTable("AreaOffice");
            }

            return officeHierarchy
                .Select(x => new { CodeName = x.AreaName, Code = x.AreaCode })
                .GroupBy(x => x.Code)
                .Select(x => new CodeTableEx { Code = x.Key, CodeName = x.First().CodeName })
                .ToList();
        }

        public static void AddAppVersion(AppVersion rec)
        {
            DBLayer.AddAppVersion(rec);
        }

        public static IEnumerable<CodeTableEx> GetTerritories(bool isSuperAdmin, IEnumerable<OfficeHierarchy> officeHierarchy)
        {
            if (isSuperAdmin)
            {
                return Business.GetCodeTable("Territory");
            }

            return officeHierarchy
                .Select(x => new { CodeName = x.TerritoryName, Code = x.TerritoryCode })
                .GroupBy(x => x.Code)
                .Select(x => new CodeTableEx { Code = x.Key, CodeName = x.First().CodeName })
                .ToList();
        }

        public static IEnumerable<CodeTableEx> GetHeadQuarters(bool isSuperAdmin, IEnumerable<OfficeHierarchy> officeHierarchy)
        {
            if (isSuperAdmin)
            {
                return Business.GetCodeTable("HeadQuarter");
            }

            return officeHierarchy
                .Select(x => new { CodeName = x.HQName, Code = x.HQCode })
                .GroupBy(x => x.Code)
                .Select(x => new CodeTableEx { Code = x.Key, CodeName = x.First().CodeName })
                .ToList();
        }

        /// <summary>
        /// This will give unique activity types as in dbo.Activity table
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CodeTableEx> GetActivityTypes()
        {
            return DBLayer.GetActivityTypes();
        }

        public static void SaveAppVersion(AppVersion rec)
        {
            DBLayer.SaveAppVersion(rec);
        }

        public static IEnumerable<ActivityMapData> GetActivityMapData(long empDayId)
        {
            return DBLayer.GetActivityMapData(empDayId);
        }

        public static IEnumerable<ActivityMapData> GetActivityData(long activityId)
        {
            return DBLayer.GetActivityData(activityId);
        }

        public static IEnumerable<ActivityMapData> GetManyActivityData(IEnumerable<long> activityIds)
        {
            return DBLayer.GetManyActivityData(activityIds);
        }

        public static IEnumerable<ActivityContactData> GetActivityContactData(long activityId)
        {
            return DBLayer.GetActivityContactData(activityId);
        }

        public static ICollection<ActivityMapData> GetAllActivityMapData(SearchCriteria searchCriteria)
        {
            return DBLayer.GetAllActivityMapData(searchCriteria);
        }

        public static IEnumerable<SqliteExpenseData> GetSavedExpenseItems(long batchId)
        {
            return DBLayer.GetSavedExpenseItems(batchId);
        }

        public static IEnumerable<SignedInEmployeeData> GetSignedInEmployeeData(SearchCriteria sc, DateTime inputDate)
        {
            ICollection<SignedInEmployeeData> returnList = DBLayer.GetSignedInEmployeeData(inputDate);

            if (returnList == null)
            {
                return new List<SignedInEmployeeData>();
            }

            //==================================
            // apply security filters now;
            //==================================
            // find out unique Staff codes
            IEnumerable<string> uniqueStaffCodes = returnList.GroupBy(x => x.EmployeeCode).Select(x => x.Key).ToList();
            IEnumerable<string> exclusionList = Business.GetExclusionList(sc, uniqueStaffCodes);

            // now take only those staff codes that don't exist in exclusion list
            return returnList.Where(x => exclusionList.Any(y => y == x.EmployeeCode) == false).ToList();
        }

        public static void DeleteAppVersion(long recId)
        {
            DBLayer.DeleteAppVersion(recId);
        }

        public static string GetStaffCode(long id)
        {
            // first get the staff code
            TenantEmployee te = DBLayer.GetEmployeeRecord(id);
            return te?.EmployeeCode ?? "";
        }

        public static int GetActivityCount(long employeeDayId)
        {
            return DBLayer.GetActivityCount(employeeDayId);
        }

        //public static void RefreshOfficeHierarchy(long tenantId)
        //{
        //    DBLayer.RefreshOfficeHierarchy(tenantId);
        //}

        public static void ParseUploadFile(long tenantId)
        {
            // First check if we can get lock to process this tenant
            // (as data for this tenant may already be process)

            // TODO: always log an entry for request received and lock acquired status for this entry;
            // this has to be followed by its start time and finish time;

            bool lockStatus = DBLayer.LockTenantForUploadParseFile(tenantId, Utils.AutoReleaseLockTimeInMinutes);
            long logId = DBLayer.CreateProcessLogEntry(tenantId, lockStatus, "ParseUpload");

            if (!lockStatus)
            {
                Task.Run(async () =>
                {
                    await Business.LogAlert("WUPLKNA", "Lock could not be acquired to upload Data Feed");
                });

                LogError($"{nameof(ParseUploadFile)}", "A similar process may already be active. Terminating!", ".");
                return;
            }

            // lock has been acquired - now do the processing on a separate thread
            Task mainTask = Task.Factory.StartNew(() =>
            {
                // retrieve rows that are pending to be processed
                ICollection<ExcelUploadStatus> excelUploadStatusRows
                            = Business.GetExcelUploadStatus(tenantId);

                ParseExcel excelParser = new ParseExcel();
                ParseCSV csvParser = new ParseCSV();

                foreach (var r in excelUploadStatusRows)
                {
                    // renew the lease - as other instance of this job should not take over
                    // if processing of all files takes longer than configured 15 minutes.
                    DBLayer.RenewLeaseForUploadParseFile(tenantId);

                    if (r.IsParsed == false && r.IsLocked == false)
                    {
                        // lock the row
                        bool status = DBLayer.AcquireParseLockOnExcelUpload(r.Id);
                        // if lock could not be acuired, don't process the row.
                        if (status == false)
                        {
                            Business.LogError($"{nameof(ParseUploadFile)}", $"Could not lock table row (in ExcelUploadStatus) to parse upload data for {r.UploadTable} table.");
                            continue;
                        }

                        // Process
                        if (r.IsExcel && Utils.SiteConfigData.AllowExcelUpload)
                        {
                            excelParser.Parse(r, Utils.FileUploadErrorStopLimit());
                        }
                        else if (r.IsCSV && Utils.SiteConfigData.AllowCSVUpload)
                        {
                            csvParser.Parse(r, Utils.FileUploadErrorStopLimit());
                        }
                        else
                        {
                            new ParseUnKnown().Parse(r, 0);
                        }

                        // Unlock
                        DBLayer.ReleaseParseLockOnExcelUpload(r.Id);
                    }
                }
            },
            TaskCreationOptions.LongRunning);

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromUploadParseFile(tenantId);
                DBLayer.UpdateLogRecord(logId, false, true);
                long errorLogId = 0;
                p.Exception?.Handle((ex) =>
                {
                    errorLogId = Business.LogError(nameof(ProcessDataFeed), ex.ToString(), " ");
                    return true;
                });

                Task.Run(async () =>
                {
                    await Business.LogAlert("EUPFAULT", $"Error occured in Data Feed Upload; Log Id {errorLogId}");
                });

            }, TaskContinuationOptions.OnlyOnFaulted);

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromUploadParseFile(tenantId);
                DBLayer.UpdateLogRecord(logId, true, false);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static void ProcessDataFeed(long tenantId)
        {
            // First check if we can get lock to process this tenant
            // (as data for this tenant may already be process)

            // TODO: always log an entry for request received and lock acquired status for this entry;
            // this has to be followed by its start time and finish time;

            bool lockStatus = DBLayer.LockTenantForDataFeedProcessing(tenantId, Utils.AutoReleaseLockTimeInMinutes);
            long logId = DBLayer.CreateProcessLogEntry(tenantId, lockStatus, "DataFeed");

            if (!lockStatus)
            {
                Task.Run(async () =>
                {
                    await Business.LogAlert("WDFLKNA", "Lock could not be acquired to Process Data Feed");
                });

                LogError($"{nameof(ProcessDataFeed)}", "A similar process may already be active. Terminating!", ".");
                return;
            }

            // lock has been acquired - now do the processing on a separate thread
            Task mainTask = Task.Factory.StartNew(() =>
            {
                // retrieve rows that are pending to be processed
                ICollection<ExcelUploadStatus> excelUploadStatusRows
                            = Business.GetExcelUploadStatus(tenantId);

                // Product master and price list are updated in a single sproc.
                // if user has uploaded both files, we want to do this transformation only once.
                bool productDataFeedTransformed = false;

                foreach (var r in excelUploadStatusRows)
                {
                    DBLayer.RenewLeaseForDataFeedProcessing(tenantId);

                    if (r.IsPosted == false && r.IsParsed &&
                        r.ErrorCount == 0 && r.RecordCount > 0 && !r.IsLocked)
                    {
                        if (r.UploadTable.Equals("CustomerMaster", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.ProcessCustomerDataFeed(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("EmployeeMaster", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.ProcessSalesPersonDataFeed(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("MaterialMaster", StringComparison.OrdinalIgnoreCase)
                            || r.UploadTable.Equals("PriceList", StringComparison.OrdinalIgnoreCase))
                        {
                            if (productDataFeedTransformed == false)
                            {
                                DBLayer.ProcessProductDataFeed(r.TenantId, r.IsCompleteRefresh);
                                productDataFeedTransformed = true;
                            }
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("ZoneAreaTerritory", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshOfficeHierarchy(r.TenantId);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("DivisionSegment", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshDivisionSegmentData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("StaffMessageInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshStaffMessageData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("CustomerDivisionBalanceInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshCustomerDivisionBalance(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("StaffDailyReportDataInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshStaffDailyReportData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("StaffDivisionInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshStaffDivisionData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("AgreementNumberInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshAgreementNumberData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("SurveyNumberInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshSurveyNumberData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("EntityNumberInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshEntityNumberData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("ItemMasterInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshItemMasterData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("PPAInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshPPAData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("TransporterInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshTransporterData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("VendorInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshVendorData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("GrnNumberInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshGrnNumberData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("RequestNumberInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshRequestNumberData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("EmployeeAchievedInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshEmployeeAchievedData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("EmployeeMonthlyTargetInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshEmployeeMonthlyTargetData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("EmployeeYearlyTargetInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshEmployeeYearlyTargetData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("BonusRateInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshBonusRateData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("StaffHQInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshStaffHQData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("PPAStaffInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.RefreshPPAStaffData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        //Added by Swetha -Mar 15
                        else if (r.UploadTable.Equals("LeaveTypeInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.ProcessLeaveTypeDataFeed(r.TenantId);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("HolidayListInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.ProcessHolidayListDataFeed(r.TenantId);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                        else if (r.UploadTable.Equals("SOParentSOInput", StringComparison.OrdinalIgnoreCase))
                        {
                            DBLayer.ProcessSoParentSOData(r.TenantId, r.IsCompleteRefresh);
                            DBLayer.CreateExcelUploadHistory(r);
                        }
                    }
                }

                // also call this, to update tenant names, that have been changed on web
                // and transform them to TenantEmployee table.
                DBLayer.ProcessDataFeed(tenantId);
            },
            TaskCreationOptions.LongRunning);

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromDataFeedProcessing(tenantId);
                DBLayer.UpdateLogRecord(logId, false, true);
                long errorLogId = 0;
                p.Exception?.Handle((ex) =>
                {
                    errorLogId = Business.LogError(nameof(ProcessDataFeed), ex.ToString(), " ");
                    return true;
                });

                Task.Run(async () =>
                {
                    await Business.LogAlert("EDFFAULT", $"Error occured in Data Feed Processing; Log Id {errorLogId}");
                });

                //AggregateException exception = p.Exception;
                //if (exception != null)
                //{
                //    foreach (Exception ex in exception.InnerExceptions)
                //    {
                //        Business.LogError(nameof(ProcessMobileData), ex.ToString(), " ");
                //    }
                //}
            }, TaskContinuationOptions.OnlyOnFaulted);

            //.ContinueWith((p) =>
            //{
            //    DBLayer.UnLockTenantFromDataFeedProcessing(tenantId);
            //    DBLayer.UpdateDataFeedLogRecord(logId, false, true);

            //    AggregateException exception = p.Exception;
            //    if (exception != null)
            //    {
            //        foreach (Exception ex in exception.InnerExceptions)
            //        {
            //            Business.LogError(nameof(ProcessMobileData), ex.ToString(), " ");
            //        }
            //    }
            //}, TaskContinuationOptions.OnlyOnCanceled)

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromDataFeedProcessing(tenantId);
                DBLayer.UpdateLogRecord(logId, true, false);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static void ProcessImageTransfer(long tenantId, bool deleteImageAfterUpload, int maxImagesToTransfer)
        {
            // First check if we can get lock to process this tenant
            // (as data for this tenant may already be in process)

            // TODO: always log an entry for request received and lock acquired status for this entry;
            // this has to be followed by its start time and finish time;

            // validate bucket Name

            bool lockStatus = DBLayer.LockTenantForImageUploadToS3(tenantId, Utils.AutoReleaseLockTimeInMinutes);
            long logId = DBLayer.CreateProcessLogEntry(tenantId, lockStatus, "ImageTransfer");

            if (!lockStatus)
            {
                Task.Run(async () =>
                {
                    await Business.LogAlert("WITLKNA", "Lock could not be acquired to Transfer Images to S3");
                });

                LogError($"{nameof(ProcessImageTransfer)}", "A similar process may already be active. Terminating!", ".");
                return;
            }

            string bucketName = Utils.SiteConfigData.S3Bucket;
            if (String.IsNullOrEmpty(bucketName) || "none".Equals(bucketName.ToLower()))
            {
                Business.LogError($"{nameof(ProcessImageTransfer)}", $"Invalid Bucket Name {bucketName}");
                DBLayer.UnLockTenantFromImageUploadToS3(tenantId);
                return;
            }

            // lock has been acquired - now do the processing on a separate thread
            Task mainTask = Task.Factory.StartNew(() =>
            {
                Business.UploadImagesToS3(tenantId, deleteImageAfterUpload, maxImagesToTransfer);
            },
            TaskCreationOptions.LongRunning);

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromImageUploadToS3(tenantId);
                DBLayer.UpdateLogRecord(logId, false, true);
                long errorLogId = 0;
                p.Exception?.Handle((ex) =>
                {
                    errorLogId = Business.LogError(nameof(ProcessImageTransfer), ex.ToString(), " ");
                    return true;
                });

                Task.Run(async () =>
                {
                    await Business.LogAlert("EITFAULT", $"Error occured while transfering images to S3; Log Id {errorLogId}");
                });

            }, TaskContinuationOptions.OnlyOnFaulted);

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromImageUploadToS3(tenantId);
                DBLayer.UpdateLogRecord(logId, true, false);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static Order GetOrder(long orderId)
        {
            return DBLayer.GetOrder(orderId);
        }

        public static byte[] ImageData(long expenseId, int imageItem)
        {
            string fileName = DBLayer.ImageData(expenseId, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] PaymentImageData(long id, int imageItem)
        {
            string fileName = DBLayer.PaymentImageData(id, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] ActivityImageData(long id, int imageItem)
        {
            string fileName = DBLayer.ActivityImageData(id, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] STRImageData(long id, int imageItem)
        {
            string fileName = DBLayer.STRImageData(id, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] EntityImageData(long id, int imageItem)
        {
            string fileName = DBLayer.EntityImageData(id, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] EntityBankDetailImageData(long id, int imageItem)
        {
            string fileName = DBLayer.EntityBankDetailImageData(id, imageItem);
            return GetImageBytes(fileName);
        }

        //private static byte[] GetImageBytes(string imageFileName)
        //{
        //    // first form complete image file name
        //    string imagesFolder = Utils.SiteConfigData.ImagesFolder;
        //    string completeFileName = System.IO.Path.Combine(imagesFolder, imageFileName);

        //    if (File.Exists(completeFileName))
        //    {
        //        MemoryStream tmpStream = new MemoryStream();
        //        System.Drawing.Image img = System.Drawing.Image.FromFile(completeFileName);
        //        img.Save(tmpStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        return tmpStream.ToArray();
        //    }

        //    // we can return a default image
        //    return new byte[] { };
        //}

        public static byte[] GetImageBytes(string imageFileName)
        {
            if (String.IsNullOrEmpty(imageFileName))
            {
                return new byte[] { };
            }

            // take it from local
            byte[] fileBytesFromLocal = LocalDiskFacade.GetImageBytes(Utils.SiteConfigData.ImagesFolder, imageFileName);
            if (fileBytesFromLocal != null)
            {
                return fileBytesFromLocal;
            }

            // go to s3 only if archive is enabled in configuration.
            if (Utils.SiteConfigData.ArchiveImage)
            {
                byte[] fileBytesFromS3 = S3Facade.GetS3ImageBytes(imageFileName, Utils.SiteConfigData.S3Bucket, Utils.S3Client);
                if (fileBytesFromS3 != null)
                {
                    return fileBytesFromS3;
                }
            }

            return new byte[] { };
        }

        public static IEnumerable<SqliteOrderData> GetSavedOrders(long batchId)
        {
            return DBLayer.GetSavedOrders(batchId);
        }

        public static IEnumerable<SqliteDeviceLog> GetDeviceLogs(long batchId)
        {
            return DBLayer.GetDeviceLogs(batchId);
        }

        public static EmployeeDayData GetEmployeeDayData(long empDayId)
        {
            return DBLayer.GetEmployeeDayData(empDayId);
        }

        public static EmployeeDayData GetEmployeeDayData(long empId, long dayId)
        {
            return DBLayer.GetEmployeeDayData(empId, dayId);
        }

        public static IEnumerable<TransportType> GetTransportTypes(string staffCode)
        {
            ICollection<TransportType> transportTypes = DBLayer.GetTransportTypes();

            // now see if override is provided for Private vehicle expenses
            // functionality added on 24.12.2020 for Geolife - to provide
            // different transport rates on per grade/person basis.

            // retrieve SalesPerson Record
            DomainEntities.SalesPerson sp = DBLayer.GetSingleSalesPerson(staffCode);
            if (sp.OverridePrivateVehicleRatePerKM)
            {
                var tt2w = transportTypes.FirstOrDefault(x =>
                                x.TransportTypeCode.Equals(TwoWheeler, StringComparison.OrdinalIgnoreCase));
                if (tt2w != null)
                {
                    tt2w.ReimbursementRatePerUnit = sp.TwoWheelerRatePerKM;
                }

                var tt4w = transportTypes.FirstOrDefault(x =>
                                x.TransportTypeCode.Equals(FourWheeler, StringComparison.OrdinalIgnoreCase));
                if (tt4w != null)
                {
                    tt4w.ReimbursementRatePerUnit = sp.FourWheelerRatePerKM;
                }
            }

            return transportTypes;
        }

        public static IEnumerable<SqliteActionProcessLog> GetSqliteBatchProcessLog(int startItem, int itemCount)
        {
            return DBLayer.GetSqliteBatchProcessLog(startItem, itemCount);
        }

        public static IEnumerable<SqliteOrderLineData> GetSavedOrderItems(long sqliteOrderId)
        {
            return DBLayer.GetSavedOrderItems(sqliteOrderId);
        }

        public static IEnumerable<SqliteActionProcessLog> GetDataFeedProcessLog(int startItem, int itemCount)
        {
            return DBLayer.GetDataFeedProcessLog(startItem, itemCount);
        }

        public static IEnumerable<SqliteActionProcessLog> GetSmsJobLog(int startItem, int itemCount)
        {
            return DBLayer.GetSmsJobLog(startItem, itemCount);
        }

        public static IEnumerable<PurgeDataLog> GetPurgeDataLog(int startItem, int itemCount)
        {
            return DBLayer.GetPurgeDataLog(startItem, itemCount);
        }

        public static ICollection<EmployeeRecord> GetActivityReportUsers(SearchCriteria searchCriteria)
        {
            CRMUsersFilter crmUsersFilter = new CRMUsersFilter();

            if (searchCriteria != null)
            {
                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    crmUsersFilter.ApplyEmployeeCodeFilter = searchCriteria.ApplyEmployeeCodeFilter;
                    crmUsersFilter.EmployeeCode = searchCriteria.EmployeeCode;
                }

                if (searchCriteria.ApplyEmployeeNameFilter)
                {
                    crmUsersFilter.ApplyNameFilter = searchCriteria.ApplyEmployeeNameFilter;
                    crmUsersFilter.Name = searchCriteria.EmployeeName;
                }
            }

            ICollection<EmployeeRecord> employees = Business.Users(crmUsersFilter);

            if (searchCriteria?.ApplyEmployeeStatusFilter ?? false)
            {
                employees = employees.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap)).ToList();
            }

            // find out unique Staff codes associated for each employee in the expenses list
            IEnumerable<string> uniqueStaffCodes = employees.Select(x => x.EmployeeCode).ToList();
            ICollection<string> exclusionList = Business.GetExclusionList(searchCriteria, uniqueStaffCodes);

            // now take only those staff codes that don't exist in exclusion list
            employees = employees.Where(x => exclusionList.Any(y => y == x.EmployeeCode) == false).ToList();

            return employees.OrderBy(x => x.Name).ToList();
        }

        public static void ProcessMobileData(long tenantId, long employeeId)
        {
            // First check if we can get lock to process this tenant
            // (as data for this tenant may already be under process)

            // TODO: always log an entry for request received and lock acquired status for this entry;
            // this has to be followed by its start time and finish time;

            bool lockStatus = DBLayer.LockTenantForMobileDataProcessing(tenantId, Utils.AutoReleaseLockTimeInMinutes);
            long logId = DBLayer.CreateSqliteActionProcessLogEntry(tenantId, employeeId, lockStatus);

            if (!lockStatus)
            {
                Task.Run(async () =>
                {
                    await Business.LogAlert("WBPLKNA", "Lock could not be acquired to Process Phone Batches");
                });

                LogError($"{nameof(ProcessMobileData)}", "A similar process may already be active. Terminating!", ".");

                return;
            }

            // lock has been acquired - now process the tenant on a separate thread
            Task mainTask = Task.Factory.StartNew(() =>
                {
                    do
                    {
                        // gets the batches only for active tenants/employees
                        IEnumerable<long> batches = DBLayer.GetSqliteBatchForProcessing(1, tenantId, employeeId);
                        if (batches == null || batches.Count() == 0)
                        {
                            break;
                        }

                        long batchId = batches.First();

                        DBLayer.RenewLeaseForMobileDataProcessing(tenantId);

                        // process all batch actions in a transaction;
                        //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                        {
                            // retrieve batch Record
                            SqliteDomainBatch batchRecord = DBLayer.GetSqliteBatch(batchId);

                            // process actions first
                            List<SqliteActionProcessData> batchItems = DBLayer.GetSqliteActionRecords(batchId);
                            if (batchItems != null && batchItems.Count > 0)
                            {
                                // retrieve all ORP items in this batch and add them to batchItems
                                // This is done to handle End Day issues - 29.07.2018
                                batchItems.AddRange(
                                    DBLayer.GetSavedOrders(batchId).Select(x => new SqliteActionProcessData()
                                    {
                                        Id = 0,
                                        BatchId = x.BatchId,
                                        EmployeeId = x.EmployeeId,
                                        At = x.OrderDate,
                                        ActivityTrackingtype = -1,
                                        Latitude = 0,
                                        Longitude = 0,
                                        PhoneModel = "",
                                        PhoneOS = "",
                                        AppVersion = "",
                                        LocationCount = 0,
                                        Locations = null
                                    }).ToList()
                                );

                                batchItems.AddRange(
                                    DBLayer.GetSavedReturns(batchId).Select(x => new SqliteActionProcessData()
                                    {
                                        Id = 0,
                                        BatchId = x.BatchId,
                                        EmployeeId = x.EmployeeId,
                                        At = x.ReturnOrderDate,
                                        ActivityTrackingtype = -1,
                                        Latitude = 0,
                                        Longitude = 0,
                                        PhoneModel = "",
                                        PhoneOS = "",
                                        AppVersion = "",
                                        LocationCount = 0,
                                        Locations = null
                                    })
                                );

                                batchItems.AddRange(
                                    DBLayer.GetSavedPayments(batchId).Select(x => new SqliteActionProcessData()
                                    {
                                        Id = 0,
                                        BatchId = x.BatchId,
                                        EmployeeId = x.EmployeeId,
                                        At = x.PaymentDate,
                                        ActivityTrackingtype = -1,
                                        Latitude = 0,
                                        Longitude = 0,
                                        PhoneModel = "",
                                        PhoneOS = "",
                                        AppVersion = "",
                                        LocationCount = 0,
                                        Locations = null
                                    })
                                );

                                foreach (SqliteActionProcessData item in batchItems.OrderBy(x => x.At))
                                {
                                    ProcessSqliteAction(item);

                                    if (item.Id > 0)
                                    {
                                        item.IsProcessed = true;
                                        DBLayer.SaveProcessedSqliteAction(item);
                                    }
                                }
                            }

                            // process expense data
                            DBLayer.ProcessSqliteExpenseData(batchId);

                            // Process order data
                            DBLayer.ProcessSqliteOrderData(batchId);

                            // Process Payment Data
                            DBLayer.ProcessSqlitePaymentData(batchId);

                            // Process ReturnOrder Data
                            DBLayer.ProcessSqliteReturnOrderData(batchId);
                            // get expense items
                            //scope.Complete();

                            // Process Entity Data
                            // first update derived location coordinates
                            IEnumerable<SqliteEntityData> entities = DBLayer.GetSavedSqliteEntities(batchId);
                            if (entities.Count() > 0)
                            {
                                foreach (var e in entities)
                                {
                                    ProcessSqliteEntity(e);
                                    DBLayer.SaveSqliteEntityData(e);
                                }
                            }

                            // The order between Entity, Agreement, IssueReturn and
                            // advance requests processing has to be maintained.
                            // As Agreement depends on Entity,
                            // Issue Return / Advance depends on Agreement
                            // Oct 16 2019
                            DBLayer.ProcessSqliteEntityData(batchId);

                            // March 19 2020 - PJM
                            DBLayer.ProcessSqliteBankDetailsData(batchId);

                            DBLayer.ProcessSqliteAgreementData(batchId, Utils.SiteConfigData.AgreementDefaultStatus);

                            // Dec 18 2020 - currently, don't have a separate default status value for Survey
                            // hence using AgreementDefaultStatus only.
                            DBLayer.ProcessSqliteSurveyData(batchId, Utils.SiteConfigData.AgreementDefaultStatus);

                            // April 8 2019 - Reitzel
                            DBLayer.ProcessSqliteIssueReturnData(batchId);

                            DBLayer.ProcessSqliteAdvanceRequestData(batchId);

                            // Process Entity Work Flow Requests
                            DBLayer.ProcessEntityWorkFlowData(batchId);

                            // Process Terminate Agreement Data
                            DBLayer.ProcessTerminateAgreementData(batchId);

                            // Process STR/DWS Data - May 12 2020
                            DBLayer.ProcessSTRData(batchId);

                            // Process Day Plan Target Data - June 03 2021
                            DBLayer.ProcessSqliteDayPlanTargetData(batchId);

                            // Process Dealer Questionnaire - June 24 2021 ; Author : Rajesh/Vasanth
                            DBLayer.ProcessSqliteQuestionnaireData(batchId);

                            // Process FollowUpTask Target Data - Oct 09 2021
                            DBLayer.ProcessSqliteTaskData(batchId);

                            // Process FollowUpTask Action Target Data - Oct 09 2021
                            DBLayer.ProcessSqliteTaskActionData(batchId);

                            // Process Leave Data- Apr 22 2022
                            DBLayer.ProcessSqliteLeaveData(batchId);
                        }

                        // at the end unlock batch Id + mark it as processed as well.
                        DBLayer.MarkSqliteBatchAsProcessed(batchId);
                    } while (true);
                },
            TaskCreationOptions.LongRunning);

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromMobileDataProcessing(tenantId);
                DBLayer.UpdateBatchProcessLogRecord(logId, false, true);
                long errorLogId = 0;
                p.Exception?.Handle(ex =>
                {
                    errorLogId = Business.LogError(nameof(ProcessMobileData), ex.ToString(), " ");
                    return true;
                });

                Task.Run(async () =>
                {
                    await Business.LogAlert("EBPFAULT", $"Error occured in Phone Batch Processing; Log Id {errorLogId}");
                });

            }, TaskContinuationOptions.OnlyOnFaulted);

            mainTask.ContinueWith((p) =>
            {
                DBLayer.UnLockTenantFromMobileDataProcessing(tenantId);
                DBLayer.UpdateBatchProcessLogRecord(logId, true, false);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static void MarkDWSAsPaid(IEnumerable<DWS> dwsRecs)
        {
            DBLayer.MarkDWSAsPaid(dwsRecs);
        }

        public static void CreateDWSPaymentReference(DWSPaymentReference dwsPaymentReference)
        {
            DBLayer.CreateDWSPaymentReference(dwsPaymentReference);
        }

        public static IEnumerable<SqliteReturnOrderItemData> GetSavedReturnOrderItems(long sqliteReturnOrderId)
        {
            return DBLayer.GetSavedReturnOrderItems(sqliteReturnOrderId);
        }

        public static IEnumerable<SqliteReturnOrderData> GetSavedReturns(long batchId)
        {
            return DBLayer.GetSavedReturns(batchId);
        }

        public static IEnumerable<SalesPersonModel> GetSalesPersonData(StaffFilter searchCriteria)
        {
            IEnumerable<SalesPersonModel> salesPersonData = DBLayer.GetSalesPersonData(searchCriteria);

            if (!searchCriteria.IsSuperAdmin)
            {
                IEnumerable<string> visibleStaffCode = Business.GetVisibleStaffCodes(searchCriteria.IsSuperAdmin, searchCriteria.CurrentUserStaffCode);
                salesPersonData = salesPersonData.Where(x => visibleStaffCode.Any(y => y.Equals(x.StaffCode, StringComparison.OrdinalIgnoreCase)));
            }

            // apply Zone/Area/Territory/HQ filter
            IEnumerable<string> uniqueStaffCodes = salesPersonData.Select(x => x.StaffCode).ToList();
            ICollection<string> exclusionList = Business.GetExclusionList(searchCriteria, uniqueStaffCodes);

            salesPersonData = salesPersonData.Where(x => exclusionList.Any(y => y == x.StaffCode) == false).ToList();

            // put the name of head quarter in the objects
            IEnumerable<CodeTableEx> headQuarters = Business.GetCodeTable("HeadQuarter");
            Parallel.ForEach(salesPersonData, spd =>
            {
                var hqs = headQuarters.FirstOrDefault(x => x.Code.Equals(spd.HQCode, StringComparison.OrdinalIgnoreCase));
                spd.HQName = hqs?.CodeName ?? "";
            });

            return salesPersonData.ToList();
        }

        public static IEnumerable<string> GetSalesPersonGrades()
        {
            return DBLayer.GetSalesPersonGrades();
        }

        public static IEnumerable<SalesPersonModel> GetSelectedSalesPersonData(IEnumerable<string> inputStaffCodes)
        {
            return DBLayer.GetSelectedSalesPersonData(inputStaffCodes);
        }

        public static byte[] ExpenseItemImage(long expenseItemId, int imageItem)
        {
            string fileName = DBLayer.ExpenseItemImage(expenseItemId, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] PaymentImage(long paymentId, int imageItem)
        {
            string fileName = DBLayer.PaymentImage(paymentId, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] ActivityImage(long activityId, int imageItem)
        {
            string fileName = DBLayer.ActivityImage(activityId, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] OrderImage(long orderId, int imageItem)
        {
            string fileName = DBLayer.OrderImage(orderId, imageItem);
            return GetImageBytes(fileName);
        }

        private static void EnsurePreviousBatchHasEndDay(long batchId, long employeeId)
        {
            // find out previous batch and if it is processed, retrieve its entries
            ICollection<SqliteActionProcessData> prevBatchActionRecords = DBLayer.GetPreviousBatchEntries(batchId, employeeId);
            if ((prevBatchActionRecords?.Count ?? 0) == 0)
            {
                return;
            }

            // check to see if there is already an end of day entry in there
            if (prevBatchActionRecords.Any(x => x.ActivityTrackingtype == 2))
            {
                return;
            }

            // if not we need to create one
            var item = prevBatchActionRecords.First();

            long employeeDayId = DBLayer.GetOpenEmployeeDayIdForAutoEndDay(item.EmployeeId, item.At);
            if (employeeDayId <= 0)
            {
                return;
            }

            Business.LogError("AutoEndDay", $"EmployeeId: {employeeId} | CurrentBatchId: {batchId} | ImpactedEmployeeDayId: {employeeDayId} | At:{item.At.ToString()}", " ");

            Business.RecordEndDay(new EndDay()
            {
                EmployeeDayId = employeeDayId,
                At = item.At,
                Latitude = item.Latitude,
                Longitude = item.Longitude
            });
        }

        private static void ProcessSqliteEntity(SqliteEntityData item)
        {
            // first calculate latitude and longitude based on 4 numbers
            string derivedLocSource = "Main";
            decimal latitude = item.Latitude;
            decimal longitude = item.Longitude;

            // we will go on to use MNC/MCC/LAC/CellId values
            // only if there are no additional location data available.

            // respect MNC/MCC values only if there is no other location data available.
            // note that the item.Locations has only successful locations

            if ((latitude == 0 || longitude == 0) && item.Id > 0)
            {
                if (item.NumberOfLocations == 0)
                {
                    GSMT.GetLatLng((int)item.MCC, (int)item.MNC, (int)item.LAC, (int)item.CellId, out latitude, out longitude);
                    derivedLocSource = "MCC";
                }
                else
                {
                    // lat/longitude is zero - Now go and look for Locations data
                    // retrieve user preference for location data
                    string configSettingForLocation = Utils.SiteConfigData.LocationFromType;
                    // see if there is an override in tenantEmployee
                    string employeeOverrideForLocation = DBLayer.GetSingleTenantEmployee(item.EmployeeId)?.LocationFromType ?? "";

                    if (String.IsNullOrEmpty(employeeOverrideForLocation) == false)
                    {
                        configSettingForLocation = employeeOverrideForLocation;
                    }

                    // henceforth use configSettingForLocation
                    LocationCoord locCoord = DeriveLocationBasedOnChoice(item.Locations, configSettingForLocation);
                    latitude = locCoord.Latitude;
                    longitude = locCoord.Longitude;
                    derivedLocSource = locCoord.LocSource;
                }
            }

            // save the values back in db for analysis purpose;
            item.DerivedLatitude = latitude;
            item.DerivedLongitude = longitude;
            item.DerivedLocSource = derivedLocSource;
        }

        private static void ProcessSqliteAction(SqliteActionProcessData item)
        {
            // name can be either of 'StartDay', 'Tracking', 'SaveActivity', 'EndDay'

            // first calculate latitude and longitude based on 4 numbers
            string derivedLocSource = "Main";
            decimal latitude = item.Latitude;
            decimal longitude = item.Longitude;

            // use configuration setting to either derive latitude/longitude from MNC/MCC
            // or use the one supplied in the record.
            //string configValue = ConfigurationManager.AppSettings["UseMCCMNCValuesForCoordinates"];
            //if (configValue != null && configValue.Equals("true", StringComparison.OrdinalIgnoreCase))
            //{
            //    GSMT.GetLatLng((int)item.MCC, (int)item.MNC, (int)item.LAC, (int)item.CellId, out latitude, out longitude);
            //}
            //else
            //{
            //    latitude = item.Latitude;
            //    longitude = item.Longitude;
            //}

            // we will go on to use MNC/MCC/LAC/CellId values
            // only if there are no additional location data available.

            // respect MNC/MCC values only if there is no other location data available.
            // note that the item.Locations has only successful locations

            if ((latitude == 0 || longitude == 0) && item.Id > 0)
            {
                if (item.LocationCount == 0)
                {
                    GSMT.GetLatLng((int)item.MCC, (int)item.MNC, (int)item.LAC, (int)item.CellId, out latitude, out longitude);
                    derivedLocSource = "MCC";
                }
                else
                {
                    // this logic is also for backward compatibility - if location data is there in main part of record
                    // use it, else take it from locations collection

                    // lat/longitude is zero - Now go and look for Locations data
                    // retrieve user preference for location data
                    string configSettingForLocation = Utils.SiteConfigData.LocationFromType;
                    // see if there is an override in tenantEmployee
                    string employeeOverrideForLocation = DBLayer.GetSingleTenantEmployee(item.EmployeeId)?.LocationFromType ?? "";

                    if (String.IsNullOrEmpty(employeeOverrideForLocation) == false)
                    {
                        configSettingForLocation = employeeOverrideForLocation;
                    }

                    // henceforth use configSettingForLocation
                    LocationCoord locCoord = DeriveLocationBasedOnChoice(item.Locations, configSettingForLocation);
                    latitude = locCoord.Latitude;
                    longitude = locCoord.Longitude;
                    derivedLocSource = locCoord.LocSource;
                }
            }

            // save the values back in db for analysis purpose;
            item.DerivedLatitude = latitude;
            item.DerivedLongitude = longitude;
            item.DerivedLocSource = derivedLocSource;

            // name can be malformed - fix it here before we start processing
            //item.Name = Regex.Replace(Regex.Replace(item.Name, " Action$", ""), " ", "");

            // End Day Fix
            // If activity entry or end day entry or ORP entry (activity tracking Type == -1)
            //    If Day is not open
            //       Open the day
            //       ensure that prev day if open is closed at current activity time
            //    End If
            // End If

            // 2 end Day; 4 Activity; -1 ORP entry in this batch
            if (item.ActivityTrackingtype == 2 || item.ActivityTrackingtype == 4 || item.ActivityTrackingtype == -1)
            {
                long dayId = DBLayer.GetDayId(item.At);
                EmployeeDayData edd = DBLayer.GetEmployeeDayData(item.EmployeeId, dayId);

                // if employeeDay record does not exist or has been already ended
                // we need to start / open it;
                if (edd == null || edd.EndTime.HasValue)
                {
                    // close / end any previous days that are open
                    long prevOpenEmpDayId = DBLayer.GetPreviousOpenEmployeeDayId(item.EmployeeId);
                    if (prevOpenEmpDayId > 0)
                    {
                        Business.RecordEndDay(new EndDay()
                        {
                            EmployeeDayId = prevOpenEmpDayId,
                            At = item.At,
                            Latitude = latitude,
                            Longitude = longitude
                        });
                    }

                    Business.RecordStartDay(new StartDay()
                    {
                        EmployeeId = item.EmployeeId,
                        At = item.At,
                        Latitude = latitude,
                        Longitude = longitude,
                        PhoneModel = item.PhoneModel,
                        PhoneOS = item.PhoneOS,
                        AppVersion = item.AppVersion
                    });
                }

                if (item.ActivityTrackingtype == -1)
                {
                    return;
                }
            }

            if (item.ActivityTrackingtype == 1)  // Start day
            {
                // one batch can have only one start day - this is because end day
                // has to happen only in online mode. (Note that start day can happen in
                // offline mode and therefore start day may not be the only item in a batch)

                // ensure that previous batch of this user has an end day - if not add end day entry to it.
                if (Business.ForceEndPreviousBatchOnStartDay)
                {
                    Business.EnsurePreviousBatchHasEndDay(item.BatchId, item.EmployeeId);
                }

                Business.RecordStartDay(new StartDay()
                {
                    EmployeeId = item.EmployeeId,
                    At = item.At,
                    Latitude = latitude,
                    Longitude = longitude,
                    PhoneModel = item.PhoneModel,
                    PhoneOS = item.PhoneOS,
                    AppVersion = item.AppVersion
                });
                return;
            }

            // for end day and tracking, we want to use the Employee Day Id
            // when day was started; and not when tracking actually got recorded or end day actually happened
            // (case when user did not end his/her day, next day tracking/end day entries
            //  will go under first day - Nov 30 2017)
            // e.g. if user started his day on Nov 30 at 10am and ended the day on Dec 1st at 4pm
            // all tracking entries and end day entry has to go under Nov. 30
            if (item.ActivityTrackingtype == 2)  // End Day
            {
                Business.RecordEndDay(new EndDay()
                {
                    EmployeeDayId = DBLayer.GetLastOpenEmployeeDayId(item.EmployeeId, item.At),
                    At = item.At,
                    Latitude = latitude,
                    Longitude = longitude
                });
                return;
            }

            if (item.ActivityTrackingtype == 3)  // Tracking
            {
                item.TrackingId = DBLayer.ProcessTrackingRequest(new Tracking()
                {
                    EmployeeDayId = DBLayer.GetLastOpenEmployeeDayId(item.EmployeeId, item.At),
                    At = item.At,
                    Latitude = latitude,
                    Longitude = longitude,
                    ActivityId = 0,
                    IsMileStone = false
                });
                return;
            }

            //public enum ActivityTypeEnumeration
            //    {
            //        StartDay = 1,
            //        EndDay,
            //        Tracking,
            //        SaveActivity,
            //        Order,
            //        Payment,
            //        Return
            //        WorkFlowActivity
            //    }

            if (item.ActivityTrackingtype == 4 || item.ActivityTrackingtype == 5 ||
                item.ActivityTrackingtype == 6 || item.ActivityTrackingtype == 7 ||
                item.ActivityTrackingtype == 8 || item.ActivityTrackingtype >= 9)
            {
                long employeeDayId = DBLayer.TranslateEmployeeIdToEmployeeDayId(item.EmployeeId, item.At);
                Activity act = new Activity()
                {
                    EmployeeDayId = employeeDayId,
                    At = item.At,
                    ClientName = item.ClientName,
                    ClientPhone = item.ClientPhone,
                    ClientType = item.ClientType,
                    ActivityType = item.ActivityType,
                    Comments = item.Comments,
                    Latitude = latitude,
                    Longitude = longitude,
                    ImageCount = item.ImageCount,
                    Images = item.Images,
                    ClientCode = item.ClientCode,
                    AtBusiness = item.AtBusiness,
                    ActivityAmount = item.ActivityAmount,
                    ContactCount = item.ContactCount,
                    ActivityTrackingType = item.ActivityTrackingtype,
                    Contacts = item.Contacts.Select(x => new ActivityContact()
                    {
                        Name = x.Name,
                        PhoneNumber = x.PhoneNumber,
                        IsPrimary = x.IsPrimary
                    }).ToList()
                };

                item.ActivityId = DBLayer.ProcessActivityRequest(act);
                if (item.ActivityId > 0)
                {
                    // create a tracking request as well
                    Tracking tracking = new Tracking()
                    {
                        At = item.At,
                        IsMileStone = true,
                        IsStartOfDay = false,
                        IsEndOfDay = false,
                        EmployeeDayId = employeeDayId,
                        ActivityId = item.ActivityId,
                        Latitude = latitude,
                        Longitude = longitude
                    };
                    RecordTracking(tracking);
                }

                // Add customer/Profile lat long from geoTagging activity type
                // Later - Need to make it Dynamic as different client may want this activity type to be named differently
                if (item.ActivityType == Constant.ActivityTypeGeoTagging)
                {
                    // create a customer request as well
                    GeoLocation geoLocation = new GeoLocation()
                    {
                        At = item.At,
                        EmployeeId = item.EmployeeId,
                        ClientCode = item.ClientCode,
                        Latitude = latitude,
                        Longitude = longitude,
                    };
                    long geoLocationId = DBLayer.ProcessGeoLocationRequest(geoLocation);
                }

                // if it is a workflow activity - create a row which
                // will be processed after new entities are added; this is
                // because a workflow activity can come for a newly added entity as well,
                // and we need entity Id to further process/store workflow activity data.
                // (No longer required - April 18 2019 - as we have full blown work flow module)
                //if (item.ActivityId > 0 && item.ActivityTrackingtype == 8)
                //{
                //    try
                //    {
                //        DBLayer.CreateWorkFlowActivityRequest(item);
                //    }
                //    catch (Exception ex)
                //    {
                //        LogError("WorkFlowActivity", ex);
                //    }
                //}
                return;
            }
        }

        private static LocationCoord DeriveLocationBasedOnChoice(IEnumerable<SqliteLocationData> locations, string configSettingForLocation)
        {
            LocationCoord locCoord = new LocationCoord()
            {
                Latitude = 0,
                Longitude = 0,
                LocSource = ""
            };

            if ((locations?.Count() ?? 0) == 0)
            {
                return locCoord;
            }

            // exact match as configured for user
            var locData = locations.FirstOrDefault(x => x.Source.Equals(configSettingForLocation, StringComparison.OrdinalIgnoreCase));
            if (locData != null)
            {
                locCoord.Latitude = locData.Latitude;
                locCoord.Longitude = locData.Longitude;
                locCoord.LocSource = configSettingForLocation;
                return locCoord;
            }

            // if auto -
            // out of successful entries - choose in the order of GPS/Xamarin/Network
            // Invalid user settng is treated as Auto
            // note that locations data passed in here is good location data
            // refer DBLayer.GetSqliteActionRecords()
            // - so no need to put where clause here.
            var chosenLoc = locations
                //.Where(x =>
                //x.LocationTaskStatus.Equals("Success", StringComparison.OrdinalIgnoreCase)
                //&& x.Latitude != 0 && x.Longitude != 0
                //)
                .Select(x => new
                {
                    latitude = x.Latitude,
                    longitude = x.Longitude,
                    source = x.Source,
                    sequence = x.Source.Equals("GPS", StringComparison.OrdinalIgnoreCase) ? 1 :
                                (x.Source.Equals("Xamarin", StringComparison.OrdinalIgnoreCase) ? 2 : 3)
                })
                .OrderBy(x => x.sequence)
                .FirstOrDefault();

            locCoord.Latitude = chosenLoc?.latitude ?? 0;
            locCoord.Longitude = chosenLoc?.longitude ?? 0;
            locCoord.LocSource = chosenLoc?.source ?? "";

            return locCoord;
        }

        public static void SaveRevisedOrderItems(long orderId, IEnumerable<EditOrderItem> editedItems, string approvedBy)
        {
            DBLayer.SaveRevisedOrderItems(orderId, editedItems, approvedBy);
        }

        public static ICollection<ExpenseReportData> GetExpenseReportData(SearchCriteria searchCriteria)
        {
            ICollection<EmployeeRecord> visibleEmployees = GetActivityReportUsers(searchCriteria);
            ICollection<DayRecord> visibleDays = Business.DaysTable(searchCriteria.DateFrom, searchCriteria.DateTo);

            // this will return dataset
            ICollection<ExpenseData> expenseData = DBLayer.GetExpenseReportData(visibleEmployees.Select(x => x.EmployeeId), visibleDays.Select(x => x.Id));

            var groupedExpenseData = expenseData.GroupBy(x => new { x.EmployeeId, x.DayId });
            List<ExpenseReportData> flattenedExpenseData = groupedExpenseData.Select(ged => new ExpenseReportData()
            {
                EmployeeId = ged.Key.EmployeeId,
                Name = visibleEmployees.First(x => x.EmployeeId == ged.Key.EmployeeId).Name,
                ExpenseHQCode = visibleEmployees.First(x => x.EmployeeId == ged.Key.EmployeeId).ExpenseHQCode,
                DayId = ged.Key.DayId,
                ExpenseDate = visibleDays.First(x => x.Id == ged.Key.DayId).DATE,
                DailyConsolidation = DBLayer.GetConsolidatedData(ged.Key.EmployeeId, ged.Key.DayId),
                //RepairAmount = ged.Where(x => x.ExpenseType == "Repair").Sum(x => x.Amount),
                DailyAllowanceAmount = ged.Where(x => x.ExpenseType == "Daily Allowance").Sum(x => x.Amount),
                DALocalAmount = ged.Where(x => x.ExpenseType == "DA (Local)").Sum(x => x.Amount),
                DAOutstationAmount = ged.Where(x => x.ExpenseType == "DA (Outstation)").Sum(x => x.Amount),
                TelephoneAmount = ged.Where(x => x.ExpenseType == "Telephone").Sum(x => x.Amount),
                InternetAmount = ged.Where(x => x.ExpenseType == "Internet").Sum(x => x.Amount),
                LodgeRent = ged.Where(x => x.ExpenseType == "Lodge/Rent").Sum(x => x.Amount),
                VehicleRepairAmount = ged.Where(x => x.ExpenseType == "Vehicle Repair").Sum(x => x.Amount),
                OwnStayAmount = ged.Where(x => x.ExpenseType == "Own Stay").Sum(x => x.Amount),
                TollTaxAmount = ged.Where(x => x.ExpenseType == "Toll Tax").Sum(x => x.Amount),
                ParkingAmount = ged.Where(x => x.ExpenseType == "Parking").Sum(x => x.Amount),
                DriverSalary = ged.Where(x => x.ExpenseType == "Driver Salary").Sum(x => x.Amount),
                StationaryAmount = ged.Where(x => x.ExpenseType == "Stationary").Sum(x => x.Amount),
                MiscellaneousAmount = ged.Where(x => x.ExpenseType == "Miscellaneous").Sum(x => x.Amount),
                TravelPublic = ged.Where(x => x.ExpenseType == "Travel-Public")
                                        .Select(x => new TravelPublicExpenseData()
                                        {
                                            Amount = x.Amount,
                                            TransportType = x.TransportType
                                        }).ToList(),

                TravelPrivate = ged.Where(x => x.ExpenseType == "Travel-Private")
                                        .Select(x => new TravelPrivateExpenseData()
                                        {
                                            Amount = x.Amount,
                                            TransportType = x.TransportType,
                                            OdometerStart = x.OdometerStart,
                                            OdometerEnd = x.OdometerEnd
                                        }).ToList(),

                TravelCompany = ged.Where(x => x.ExpenseType == "Travel-Company")
                                        .Select(x => new TravelCompanyExpenseData()
                                        {
                                            OdometerEnd = x.OdometerEnd,
                                            OdometerStart = x.OdometerStart
                                        }).ToList(),

                Fuel = ged.Where(x => x.ExpenseType == "Fuel")
                                .Select(x => new FuelExpenseData()
                                {
                                    Amount = x.Amount,
                                    FuelQuantityInLiters = x.FuelQuantityInLiters,
                                    FuelType = x.FuelType
                                }).ToList()
            }).ToList();

            return flattenedExpenseData.OrderBy(x => x.ExpenseDate).ThenBy(x => x.Name).ToList();
        }

        public static ICollection<AttendanceData> GetAttendanceReportData(SearchCriteria searchCriteria)
        {
            return DBLayer.GetAttendanceReportData(searchCriteria);
        }

        /// <summary>
        /// Author: Pankaj Kumar; Date 28-04-2021; Purpose: Day Plan (Day Planning)
        /// Test Case using Employee Code: 15121996 for Gagana
        /// Previous written function invoked GetActivityReportUsers() to apply the filters:
        /// 1) Employee Code, 2) Employee Name, 3) Employee Status
        /// GetExclusionList() takes care of by calling GetFilteringHQCodes(BaseSearchCriteria searchCriteria):
        /// 1) Zone, 2) Area, 3) Territory, 4) HQ
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns>Collection of Day Report Data</returns>
        /// Modified By: Kartik
        public static ICollection<DayPlanReportData> GetDayPlanReportDataSet(SearchCriteria searchCriteria)
        {

            ICollection<EmployeeRecord> visibleEmployees = GetActivityReportUsers(searchCriteria);

            ICollection<DayRecord> visibleDays = Business.DaysTable(searchCriteria.DateFrom, searchCriteria.DateTo);

            // Returns Day Report dataset
            IEnumerable<DayPlanData> DayPlData = DBLayer.GetDayPlanReportData(visibleEmployees.Select(x => x.EmployeeId),
                                                                                            visibleDays.Select(x => x.Id),
                                                                                            visibleDays.Select(x => x.DATE), searchCriteria);

            var groupedDayData = DayPlData.GroupBy(x => new
            {
                x.Date,
                x.EmployeeId,
                x.EmployeeCode,
                x.DayId,
                x.TargetSalesAmount,
                x.TargetCollectionAmount,
                x.TargetDealerAppt,
                x.TargetDemoActivity,
                x.TargetVigoreSales,
                x.ActualDealerAppt,
                x.ActualVigoreSales,
                x.ActualDemoActivity
            });
            List<DayPlanReportData> flattenedDayData = groupedDayData.Select(gcd => new DayPlanReportData()
            {
                EmployeeId = gcd.Key.EmployeeId,
                EmployeeCode = gcd.Key.EmployeeCode,
                Name = visibleEmployees.First(x => x.EmployeeId == gcd.Key.EmployeeId).Name,
                DayId = gcd.Key.DayId,
                Date = gcd.Key.Date,
                TargetDealerAppt = gcd.Key.TargetDealerAppt,
                TargetSalesAmount = gcd.Key.TargetSalesAmount,
                TargetCollectionAmount = gcd.Key.TargetCollectionAmount,
                TargetDemoActivity = gcd.Key.TargetDemoActivity,
                TargetVigoreSales = gcd.Key.TargetVigoreSales,
                ActualSalesAmount = gcd.Sum(x => x.ActualSalesAmount),
                ActualCollectionAmount = gcd.Sum(x => x.ActualCollectionAmount),
                ActualDealerAppt = gcd.Key.ActualDealerAppt,
                ActualDemoActivity = gcd.Key.ActualDemoActivity,
                ActualVigoreSales = gcd.Key.ActualVigoreSales

            }).ToList();
            return flattenedDayData.OrderBy(x => x.Date).ToList();
        }

        public static ICollection<AttendanceReportData> GetAttendanceReportDataSet(SearchCriteria searchCriteria,
                        ICollection<AttendanceData> attendanceData = null)
        {
            ICollection<EmployeeRecord> visibleEmployees = GetActivityReportUsers(searchCriteria);

            ICollection<DayRecord> visibleDays = Business.DaysTable(searchCriteria.DateFrom, searchCriteria.DateTo);

            List<long> employeeIds = visibleEmployees.Select(x => x.EmployeeId).ToList();
            //List<long> dayIds = visibleDays.Select(x => x.Id).ToList();

            // this will return dataset
            //ICollection<AttendanceData> attendanceReport = DBLayer.GetAttendanceReportData(employeeIds, dayIds);
            //ICollection<AttendanceData> attendanceReport = DBLayer.GetAttendanceReportData(dayIds);

            // user may pass attendance data - as in certain scenarios,
            // where user does need attendance data also and thereby to avoid duplication;
            ICollection<AttendanceData> attendanceReport = attendanceData;
            if (attendanceReport == null)
            {
                attendanceReport = Business.GetAttendanceReportData(searchCriteria);
            }

            ICollection<AttendanceReportData> attendanceReportData = new List<AttendanceReportData>();

            // prepare data for report
            AttendanceReportData adrs = new AttendanceReportData() { IsEmpty = true };
            foreach (var adr in attendanceReport)
            {
                // filter for employee Ids - if tenantEmployeeId does not exist in employeeIds list
                // then don't process the record;
                if (employeeIds.Any(x => x == adr.TenantEmployeeId) == false)
                {
                    continue;
                }

                if (adrs.IsEmpty)
                {
                    adrs.IsEmpty = false;
                    adrs.RefStartTrackingId = adr.TrackingId;
                    adrs.RefEndTrackingId = 0;
                    //adrs.Name = adr.Name;
                    adrs.TenantEmployeeId = adr.TenantEmployeeId;
                    //adrs.Date = adr.Date;
                    adrs.DayId = adr.DayId;
                    adrs.EmployeeDayId = adr.EmployeeDayId;
                    adrs.StartTime = (adr.IsStartOfDay) ? adr.At : DateTime.MinValue;
                    adrs.EndTime = (adr.IsEndOfDay) ? adr.At : DateTime.MinValue;
                    //adrs.StaffCode = adr.StaffCode;
                    //adrs.ExpenseHQCode = adr.ExpenseHQCode;
                    adrs.StartLocation = (adr.IsStartOfDay) ? adr.StartLocation : "";
                    adrs.EndLocation = "";
                    adrs.ActivityCount = adr.ActivityCount;
                    adrs.DistanceTravelled = adr.GoogleMapsDistanceInMeters;
                }
                else if (adr.EmployeeDayId == adrs.EmployeeDayId)
                {
                    adrs.RefEndTrackingId = adr.TrackingId;
                    adrs.EndTime = (adr.IsEndOfDay) ? adr.At : DateTime.MinValue;
                    adrs.EndLocation = (adr.IsEndOfDay) ? adr.EndLocation : "";
                    adrs.ActivityCount = adr.ActivityCount;
                    adrs.DistanceTravelled = adr.GoogleMapsDistanceInMeters;
                }
                else
                {
                    attendanceReportData.Add(adrs);
                    adrs = new AttendanceReportData()
                    {
                        IsEmpty = false,
                        RefStartTrackingId = adr.TrackingId,
                        RefEndTrackingId = 0,
                        //Name = adr.Name,
                        TenantEmployeeId = adr.TenantEmployeeId,
                        //Date = adr.Date,
                        DayId = adr.DayId,
                        EmployeeDayId = adr.EmployeeDayId,
                        StartTime = (adr.IsStartOfDay) ? adr.At : DateTime.MinValue,
                        EndTime = (adr.IsEndOfDay) ? adr.At : DateTime.MinValue,
                        //StaffCode = adr.StaffCode,
                        //ExpenseHQCode = adr.ExpenseHQCode,
                        StartLocation = (adr.IsStartOfDay) ? adr.StartLocation : "",
                        EndLocation = "",
                        ActivityCount = adr.ActivityCount,
                        DistanceTravelled = adr.GoogleMapsDistanceInMeters
                    };
                }

                if (adrs.EndTime != DateTime.MinValue)
                {
                    attendanceReportData.Add(adrs);
                    adrs = new AttendanceReportData() { IsEmpty = true };
                }
            }

            if (adrs.IsEmpty == false)
            {
                attendanceReportData.Add(adrs);
            }

            // fill columns
            Parallel.ForEach(attendanceReportData, ard =>
            {
                var empRec = visibleEmployees.Where(x => x.EmployeeId == ard.TenantEmployeeId).FirstOrDefault();

                ard.Name = empRec?.Name ?? "";
                ard.Date = visibleDays.Where(x => x.Id == ard.DayId).FirstOrDefault()?.DATE ?? DateTime.Now;
                ard.StaffCode = empRec?.EmployeeCode ?? "";
                ard.ExpenseHQCode = empRec?.ExpenseHQCode ?? "";
                ard.Hours = Business.CalculateTimeInHours(ard.StartTime, ard.EndTime);
            });

            return attendanceReportData;
        }

        public static ICollection<AttendanceReportData> GetAttendanceSummaryReportDataSet(SearchCriteria searchCriteria,
                                        ICollection<AttendanceData> attendanceData = null)
        {
            ICollection<AttendanceReportData> AttendanceReportData = Business.GetAttendanceReportDataSet(searchCriteria, attendanceData);

            DateTime startTime = DateTime.MinValue;
            long prevEmpDayId = 0;
            string startLocation = "";
            long displayRows = 0;

            // this loop will add the hours in the last row for a group
            // then we will sort in reverse order of end time, and take only first entry for each group
            double totalHours = 0;
            foreach (var item in AttendanceReportData.OrderBy(x => x.Date).ThenBy(x => x.Name).ThenBy(x => x.EndTime))
            {
                if (item.EmployeeDayId != prevEmpDayId)
                {
                    displayRows++;
                    prevEmpDayId = item.EmployeeDayId;
                    totalHours = 0;
                    startTime = item.StartTime;
                    startLocation = item.StartLocation;
                }

                totalHours += item.Hours;
                item.Hours = totalHours;
                item.StartTime = startTime;
                item.StartLocation = startLocation;
            }

            prevEmpDayId = 0;
            return AttendanceReportData.OrderBy(x => x.Date).ThenBy(x => x.Name).ThenByDescending(x => x.EndTime)
                .Where(x =>
                {
                    if (prevEmpDayId != x.EmployeeDayId)
                    {
                        prevEmpDayId = x.EmployeeDayId;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }).ToList();
        }

        public static ICollection<AbsenteeData> GetAbsenteeReportDataSet(SearchCriteria searchCriteria)
        {
            // for absentee report it is important that dbo.Days table has all the dates that are in
            // search criteria;
            // create a list of dates
            DateTime dt = searchCriteria.DateFrom;
            while (dt <= searchCriteria.DateTo)
            {
                DBLayer.GetDayId(dt);
                dt = dt.AddDays(1);
            }

            List<long> employeeIds = GetActivityReportUsers(searchCriteria).Select(x => x.EmployeeId).ToList();
            List<long> dayIds = Business.DaysTable(searchCriteria.DateFrom, searchCriteria.DateTo).Select(x => x.Id).ToList();

            // this will return dataset
            ICollection<AbsenteeData> dataSet = DBLayer.GetAbsenteeReportData(employeeIds, dayIds);

            if (searchCriteria.ApplyEmployeeStatusFilter)
            {
                dataSet = dataSet.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap)).ToList();
            }

            return dataSet;
        }

        public static ICollection<AppSignUpData> GetAppSignUpReportDataSet(SearchCriteria searchCriteria)
        {
            // Get Latest installed app version + Phone Model + OS for all employees in EmployeeDay
            ICollection<DeviceInfo> deviceInfo = DBLayer.GetInstalledAppVersion();

            List<long> employeeIds = GetActivityReportUsers(searchCriteria).Select(x => x.EmployeeId).ToList();

            // this will return dataset
            ICollection<AppSignUpData> dataSet = DBLayer.GetAppSignUpReportData(employeeIds, searchCriteria);

            Parallel.ForEach(dataSet, ds =>
            {
                var di = deviceInfo.FirstOrDefault(x => x.TenantEmployeeId == ds.TenantEmployeeId);
                ds.LastAccessDate = di?.LastAccessDate ?? DateTime.MinValue;
                ds.AppVersion = di?.AppVersion ?? "";
                ds.PhoneModel = di?.PhoneModel ?? "";
                ds.PhoneOS = di?.PhoneOS ?? "";
            });

            return dataSet;
        }

        public static ICollection<AppSignInData> GetAppSignInReportDataSet(SearchCriteria searchCriteria)
        {
            List<long> employeeIds = GetActivityReportUsers(searchCriteria)
                //.Where(x=> x.IsActive) // for this report we need deleted employees as well;
                .Select(x => x.EmployeeId)
                .ToList();
            List<long> dayIds = Business.DaysTable(searchCriteria.DateFrom, searchCriteria.DateTo).Select(x => x.Id).ToList();

            return DBLayer.GetAppSignInReportData(employeeIds, dayIds, searchCriteria);
        }

        public static void TerminateUserAccess(long empId)
        {
            Business.DisassociateUserOnPhone(empId);
            DBLayer.TerminateUserAccess(empId);
        }

        public static void DisassociateUserOnPhone(long empId)
        {
            DBLayer.DisassociateUserOnPhone(empId);
        }

        public static ICollection<DashboardProduct> GetProductData(ProductsFilter searchCriteria)
        {
            // this method is called from Admin page and from Dashboard - on Dashboard we show the products
            // that are visible to a signed in user - and therefore we don't want to show products with no price
            // as Area code is in Product Price table only.

            // set the value in filtering Area Codes

            //IEnumerable<OfficeHierarchy> officeHierarchy = (searchCriteria.IsSuperAdmin)
            //        ? DBLayer.GetDetailedAssociations()
            //        : DBLayer.GetDetailedAssociations(searchCriteria.StaffCode);

            //if (searchCriteria.ApplyAreaFilter)
            //{
            //    // select data for area code only if user has access to the area code
            //    if (officeHierarchy.Any(x => x.AreaCode.Equals(searchCriteria.Area)))
            //    {
            //        searchCriteria.FilteringAreaCodes = new List<string> { searchCriteria.Area };
            //    }
            //    else
            //    {
            //        searchCriteria.FilteringAreaCodes = new List<string>();
            //    }
            //}
            //else if (searchCriteria.ApplyZoneFilter)
            //{
            //    searchCriteria.FilteringAreaCodes = officeHierarchy
            //                            .Where(x => x.ZoneCode.Equals(searchCriteria.Zone, StringComparison.OrdinalIgnoreCase))
            //                            .Select(x => x.AreaCode).ToList();
            //}
            //else
            //{
            //    searchCriteria.FilteringAreaCodes = officeHierarchy.Select(x => x.AreaCode).ToList();
            //}

            return DBLayer.GetProductData(searchCriteria);

            // if user has specified Zone or Area in search criteria, then user is looking for product which
            // are valid in those Area codes only
            // July 12 2019, UI has been changed to always take Area Code - so this code won't execute.
            //if (searchCriteria.IsSuperAdmin && searchCriteria.ApplyAreaFilter == false && searchCriteria.ApplyZoneFilter == false)
            //{
            //    IEnumerable<DashboardProduct> productDataList = DBLayer.GetProductsWithoutPrice(searchCriteria);
            //    Parallel.ForEach(productDataList, x => productData.Add(x));
            //}

            //return productData.ToList();
        }

        //Another method for future reference
        public static IEnumerable<DashboardProduct> GetProductsWithoutPrice()
        {
            return DBLayer.GetProductsWithoutPrice();
        }

        public static IEnumerable<ErrorLog> GetErrorLogData(int startItem, int itemCount, string processName)
        {
            return DBLayer.GetErrorLogData(startItem, itemCount, processName);
        }

        public static IEnumerable<SmsLog> GetSmsLogData(int startItem, int itemCount)
        {
            return DBLayer.GetSmsLogData(startItem, itemCount);
        }

        public static IEnumerable<SqlitePaymentData> GetSavedPayments(long batchId)
        {
            return DBLayer.GetSavedPayments(batchId);
        }

        public static IEnumerable<SqliteEntityData> GetSavedSqliteEntities(long batchId)
        {
            return DBLayer.GetSavedSqliteEntities(batchId);
        }

        public static IEnumerable<SqliteEntityContactData> GetSavedSqliteEntityContacts(long entityId)
        {
            return DBLayer.GetSavedSqliteEntityContacts(entityId);
        }

        public static IEnumerable<SqliteEntityCropData> GetSavedSqliteEntityCrops(long entityId)
        {
            return DBLayer.GetSavedSqliteEntityCrops(entityId);
        }

        public static IEnumerable<SqliteLeaveData> GetSavedSqliteLeaves(long batchId)
        {
            return DBLayer.GetSavedSqliteLeaves(batchId);
        }

        public static IEnumerable<SqliteCancelledLeaveData> GetSavedSqliteCancelledLeaves(long batchId)
        {
            return DBLayer.GetSavedSqliteCancelledLeaves(batchId);
        }

        public static IEnumerable<SqliteAgreementData> GetSavedAgreements(long batchId)
        {
            return DBLayer.GetSavedAgreements(batchId);
        }

        public static IEnumerable<SqliteSurveyData> GetSavedSurveys(long batchId)
        {
            return DBLayer.GetSavedSurveys(batchId);
        }

        public static IEnumerable<SqliteIssueReturnData> GetSavedSqliteIssueReturns(long batchId)
        {
            return DBLayer.GetSavedSqliteIssueReturns(batchId);
        }

        public static IEnumerable<SqliteTerminateAgreementEx> GetSavedTerminateAgreements(long batchId)
        {
            return DBLayer.GetSavedTerminateAgreements(batchId);
        }

        public static IEnumerable<SqliteAdvanceRequestData> GetSavedSqliteAdvanceRequests(long batchId)
        {
            return DBLayer.GetSavedSqliteAdvanceRequests(batchId);
        }

        public static IEnumerable<SqliteEntityWorkFlowData> GetSavedSqliteWorkFlowData(long batchId)
        {
            return DBLayer.GetSavedSqliteWorkFlowData(batchId);
        }

        public static IEnumerable<SqliteSTRData> GetSavedSTRData(long batchId)
        {
            return DBLayer.GetSavedSTRData(batchId);
        }

        public static IEnumerable<SqliteBankDetailData> GetSavedBankDetails(long batchId)
        {
            return DBLayer.GetSavedBankDetails(batchId);
        }

        public static IEnumerable<SqliteDayPlanTargetData> GetSavedDayPlanTargetData(long batchId)
        {
            return DBLayer.GetSavedDayPlanTargetData(batchId);
        }

        public static IEnumerable<SqliteTaskData> GetSavedFollowUpTaskData(long batchId)
        {
            return DBLayer.GetSavedFollowUpTaskData(batchId);
        }

        public static IEnumerable<SqliteTaskActionData> GetSavedFollowUpTaskActionData(long batchId)
        {
            return DBLayer.GetSavedFollowUpTaskActionData(batchId);
        }
        // Author: Rajesh; Date: 13-07-2021; Purpose: Display Questionnaire Details
        public static IEnumerable<SqliteDomainQuestionnaireData> GetSavedQuestionnaireData(long batchId)
        {
            return DBLayer.GetSavedQuestionnaireData(batchId);
        }

        public static IEnumerable<SqliteActionDisplayData> GetDupBatchItems(long batchId)
        {
            return DBLayer.GetDupBatchItems(batchId);
        }

        public static IEnumerable<SqliteActionDisplayData> GetSavedBatchItems(long batchId)
        {
            return DBLayer.GetSavedBatchItems(batchId);
        }

        public static IEnumerable<SqliteActionContactData> GetSavedSqliteActionContacts(long actionId)
        {
            return DBLayer.GetSavedSqliteActionContacts(actionId);
        }

        public static IEnumerable<SqliteActionLocationData> GetSavedSqliteActionLocations(long actionId)
        {
            return DBLayer.GetSavedSqliteActionLocations(actionId);
        }

        public static IEnumerable<SqliteEntityLocationData> GetSavedSqliteEntityLocations(long actionId)
        {
            return DBLayer.GetSavedSqliteEntityLocations(actionId);
        }

        public static IEnumerable<SqliteDWSData> GetSavedSqliteSTRDWS(long sqliteSTRId)
        {
            return DBLayer.GetSavedSqliteSTRDWS(sqliteSTRId);
        }

        public static IEnumerable<SqliteEntityWorkFlowFollowUp> GetSavedSqliteWorkFlowFollowUps(long ewfId)
        {
            return DBLayer.GetSavedSqliteWorkFlowFollowUps(ewfId);
        }

        public static IEnumerable<string> GetSavedSqliteWorkFlowItemsUsed(long ewfId)
        {
            return DBLayer.GetSavedSqliteWorkFlowItemsUsed(ewfId);
        }

        public static IEnumerable<string> GetWorkFlowDetailItemsUsed(long ewfId)
        {
            return DBLayer.GetWorkFlowDetailItemsUsed(ewfId);
        }

        public static IEnumerable<SqliteDomainBatch> GetBatches(long employeeId, bool onlyLockedBatches, bool onlyUnprocessedBatches, long inRecentBatches)
        {
            return DBLayer.GetBatches(employeeId, onlyLockedBatches, onlyUnprocessedBatches, inRecentBatches);
        }

        public static void ClearData(long employeeId)
        {
            DBLayer.ClearData(employeeId);
        }

        //public static IEnumerable<Expense> GetExpenseList(long employeeId, DateTime date)
        //{
        //    DateTime dt = new DateTime(date.Year, date.Month, date.Day);
        //    return DBLayer.GetExpenseList(employeeId, dt);
        //}

        //public static IEnumerable<ExpenseType> GetValidExpenseTypes(string travelMode)
        //{
        //    return DBLayer.GetValidExpenseTypes(travelMode);
        //}

        public static long SaveSqliteData(SqliteDomainData domainData)
        {
            long employeeId = domainData.EmployeeId;

            // TODO: perform basic validation

            // Save Data - after checking if record already exist.
            long batchId = DBLayer.CreateSqliteDataBatch(domainData);

            if (domainData.IsDataBatch)
            {
                if (domainData.DomainActions != null)
                {
                    DBLayer.SaveSqliteActionDataWrapper(employeeId, batchId, domainData.DomainActions);
                }
                Business.SaveSqliteExpenseData(employeeId, batchId, domainData.DomainExpense);
                Business.SaveSqliteOrdersData(employeeId, batchId, domainData.DomainOrders);
                Business.SaveSqlitePaymentsData(employeeId, batchId, domainData.DomainPayments);
                Business.SaveSqliteReturnOrdersData(employeeId, batchId, domainData.DomainReturnOrders);
                Business.SaveSqliteLeavesData(employeeId, batchId, domainData.DomainLeaves);
                Business.SaveSqliteCancelledLeavesData(employeeId, batchId, domainData.DomainCancelledLeaves);
                Business.SaveSqliteEntitiesData(employeeId, batchId, domainData.DomainEntities);

                Business.SaveSqliteAgreementsData(employeeId, batchId, domainData.DomainAgreements);
                Business.SaveSqliteSurveysData(employeeId, batchId, domainData.DomainSurveys);
                Business.SaveSqliteBankDetailsData(employeeId, batchId, domainData.DomainBankDetails);

                Business.SaveSqliteDeviceLogData(employeeId, batchId, domainData.DeviceLogs);
                Business.SaveSqliteIssueReturns(employeeId, batchId, domainData.DomainIssueReturns);
                Business.SaveSqliteAdvanceRequests(employeeId, batchId, domainData.DomainAdvanceRequests);

                Business.SaveSqliteWorkFlowData(employeeId, batchId, domainData.DomainWorkFlowPageData);
                Business.SaveSqliteTerminateAgreementsData(employeeId, batchId, domainData.DomainTerminateAgreementData);

                Business.SaveSqliteSTRData(employeeId, batchId, domainData.DomainSTRData);

                Business.SaveSqliteDayPlanTargetData(employeeId, batchId, domainData.DomainDayPlanTarget);

                // Author: Rajesh V; Date24-06-2021 ; Purpose: dealer Questionnaire
                Business.SaveSqliteQuestionnaireData(employeeId, batchId, domainData.DomainQuestionnaire);

                Business.SaveSqliteTaskData(employeeId, batchId, domainData.DomainTask);
                Business.SaveSqliteTaskActionData(employeeId, batchId, domainData.DomainTaskAction);
            }

            DBLayer.MarkBatchReadyForProcessing(batchId);
            return batchId;
        }

        private static void SaveSqliteTerminateAgreementsData(long employeeId, long batchId, IEnumerable<SqliteDomainTerminateAgreementData> domainTerminateAgreementData)
        {
            if (domainTerminateAgreementData == null || domainTerminateAgreementData.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteDataWrapper(employeeId, batchId, domainTerminateAgreementData);
        }

        private static void SaveSqliteWorkFlowData(long employeeId, long batchId, IEnumerable<SqliteDomainWorkFlowPageData> domainWFPData)
        {
            if (domainWFPData == null || domainWFPData.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteWorkFlowDataWrapper(employeeId, batchId, domainWFPData);
        }

        private static void SaveSqliteAgreementsData(long employeeId, long batchId,
                                IEnumerable<SqliteDomainAgreement> domainAgreements)
        {
            if (domainAgreements == null || domainAgreements.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteAgreementsDataWrapper(employeeId, batchId, domainAgreements);
        }

        private static void SaveSqliteSurveysData(long employeeId, long batchId,
                                IEnumerable<SqliteDomainSurvey> domainSurveys)
        {
            if (domainSurveys == null || domainSurveys.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteSurveysDataWrapper(employeeId, batchId, domainSurveys);
        }

        private static void SaveSqliteDayPlanTargetData(long employeeId, long batchId,
                                IEnumerable<SqliteDomainDayPlanTarget> domainDayPlanTarget)
        {
            if (domainDayPlanTarget == null || domainDayPlanTarget.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteDayPlanTargetDataWrapper(employeeId, batchId, domainDayPlanTarget);
        }

        private static void SaveSqliteTaskData(long employeeId, long batchId,
                        IEnumerable<SqliteDomainTask> domainTask)
        {
            if (domainTask == null || domainTask.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteTaskDataWrapper(employeeId, batchId, domainTask);
        }

        private static void SaveSqliteTaskActionData(long employeeId, long batchId,
                        IEnumerable<SqliteDomainTaskAction> domainTaskAction)
        {
            if (domainTaskAction == null || domainTaskAction.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteTaskActionDataWrapper(employeeId, batchId, domainTaskAction);
        }

        private static void SaveSqliteBankDetailsData(long employeeId, long batchId,
                                IEnumerable<SqliteDomainBankDetail> domainBankDetails)
        {
            if (domainBankDetails == null || domainBankDetails.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteBankDetailsDataWrapper(employeeId, batchId, domainBankDetails);
        }

        private static void SaveSqliteSTRData(long employeeId, long batchId,
                                IEnumerable<SqliteDomainSTRData> domainSTRData)
        {
            if (domainSTRData == null || domainSTRData.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteSTRDataWrapper(employeeId, batchId, domainSTRData);
        }

        private static void SaveSqliteAdvanceRequests(long employeeId, long batchId,
                        IEnumerable<SqliteDomainAdvanceRequest> domainAdvanceRequests)
        {
            if (domainAdvanceRequests == null || domainAdvanceRequests.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteAdvanceRequestsDataWrapper(employeeId, batchId, domainAdvanceRequests);
        }

        private static void SaveSqliteIssueReturns(long employeeId, long batchId, IEnumerable<SqliteDomainIssueReturn> domainIssueReturns)
        {
            if (domainIssueReturns == null || domainIssueReturns.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteIssueReturnDataWrapper(employeeId, batchId, domainIssueReturns);
        }

        private static void SaveSqlitePaymentsData(long employeeId, long batchId, IEnumerable<SqliteDomainPayment> domainPayments)
        {
            if (domainPayments == null || domainPayments.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqlitePaymentDataWrapper(employeeId, batchId, domainPayments);
        }

        private static void SaveSqliteOrdersData(long employeeId, long batchId, IEnumerable<SqliteDomainOrder> domainOrders)
        {
            if (domainOrders == null || domainOrders.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteOrdersDataWrapper(employeeId, batchId, domainOrders);
        }

        private static void SaveSqliteLeavesData(long employeeId, long batchId, IEnumerable<SqliteDomainLeave> domainLeaves)
        {
            if (domainLeaves == null || domainLeaves.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteLeavesDataWrapper(employeeId, batchId, domainLeaves);
        }

        private static void SaveSqliteCancelledLeavesData(long employeeId, long batchId, IEnumerable<long> domainCancelledLeaves)
        {
            if (domainCancelledLeaves == null || domainCancelledLeaves.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteCancelledLeavesDataWrapper(employeeId, batchId, domainCancelledLeaves);
        }

        private static void SaveSqliteReturnOrdersData(long employeeId, long batchId, IEnumerable<SqliteDomainReturnOrder> DomainReturnOrders)
        {
            if (DomainReturnOrders == null || DomainReturnOrders.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteReturnOrdersDataWrapper(employeeId, batchId, DomainReturnOrders);
        }

        private static void SaveSqliteExpenseData(long employeeId, long batchId, SqliteDomainExpense expenseObject)
        {
            if (expenseObject == null)
            {
                return;
            }

            if (expenseObject.ExpenseItemCount > 0 && (expenseObject.ExpenseItems == null || expenseObject.ExpenseItems.Count() != expenseObject.ExpenseItemCount))
            {
                throw new InvalidOperationException("Expense Item count is > 0 and there are not enough expense items in the collection.");
            }

            // for travel-company expense type - expense amount is zero, and if that is the only expense type
            // this check is causing problem - hence removing it - Nov 16 2017
            //if (expenseObject.ExpenseItemCount > 0 && expenseObject.TotalAmount <= 0)
            //{
            //    throw new InvalidOperationException("Expense Item count is > 0 and total expense amount is 0");
            //}

            if (expenseObject.ExpenseItemCount > 0 && !expenseObject.TimeStamp.HasValue)
            {
                throw new InvalidOperationException("Expense Item count is > 0 and expense date is null");
            }

            if (expenseObject.ExpenseItemCount > 0 && expenseObject.ExpenseItems != null)
            {
                DBLayer.SaveSqliteExpenseDataWrapper(employeeId, batchId, expenseObject);
            }
        }

        private static void SaveSqliteEntitiesData(long employeeId, long batchId, IEnumerable<SqliteDomainEntity> entityObjects)
        {
            if (entityObjects == null || entityObjects.Count() == 0)
            {
                return;
            }

            // May 21 2020
            // Bug is reported, where on phone, user is able to tap on Save button multiple times
            // and as a result, multiple entity records are created - put a fix here so that
            // we don't store multiple entities in this case.

            // first select all records where unique Id is null or empty
            List<SqliteDomainEntity> itemsToSave = entityObjects.Where(x => String.IsNullOrEmpty(x.UniqueId)).ToList();

            // remove duplicates based on unique id
            var items = entityObjects.Where(x => !String.IsNullOrEmpty(x.UniqueId))
                .GroupBy(x => x.UniqueId)
                .Select(x => x.First())
                .ToList();

            itemsToSave.AddRange(items);

            int ignoredItemsCount = entityObjects.Count() - itemsToSave.Count;

            DBLayer.SaveSqliteEntitiesDataWrapper(employeeId, batchId, itemsToSave, ignoredItemsCount);
        }

        private static void SaveSqliteDeviceLogData(long employeeId, long batchId, IEnumerable<SqliteDeviceLog> logs)
        {
            if ((logs?.Count() ?? 0) == 0)
            {
                return;
            }

            DBLayer.SaveSqliteDeviceLogsDataWrapper(employeeId, batchId, logs);
        }

        /// <summary>
        /// Returns the list of activities for a given employeeId and Date
        /// </summary>
        /// <param name="activityRequest"></param>
        /// <returns>List of ActivityRecord</returns>
        public static IEnumerable<ActivityRecord> GetActivityList(long employeeId, DateTime date)
        {
            // truncate time
            DateTime dt = new DateTime(date.Year, date.Month, date.Day);
            long employeeDayId = DBLayer.RetrieveEmployeeDayId(employeeId, dt);

            if (employeeDayId == -1)
            {
                // return an empty list;
                return new List<ActivityRecord>();
            }

            return Business.ActivityData(employeeDayId);
        }

        public static BusinessResponse RecordEndDay(EndDay ed)
        {
            BusinessResponse br = new BusinessResponse();

            int status = DBLayer.ProcessEndDayRequest(ed);
            if (status < 0)
            {
                br.EmployeeDayId = -1;
                br.Content = "Invalid Id or day is already closed.";
            }
            else if (status == 0)
            {
                br.EmployeeDayId = -1;
                br.Content = "DB call returned a null value";
            }
            else
            {
                br.EmployeeDayId = ed.EmployeeDayId;
                br.Content = "Success";

                // create a tracking request as well
                Tracking tracking = new Tracking()
                {
                    At = ed.At,
                    IsMileStone = true,
                    IsStartOfDay = false,
                    IsEndOfDay = true,
                    EmployeeDayId = ed.EmployeeDayId,
                    ActivityId = 0,
                    Latitude = ed.Latitude,
                    Longitude = ed.Longitude
                };
                RecordTracking(tracking);
            }

            return br;
        }

        //public static long SaveExpense(Expense expense)
        //{
        //    expense.EmployeeDayId = DBLayer.RetrieveEmployeeDayId(expense.EmployeeId,
        //        new DateTime(expense.ExpenseDate.Year, expense.ExpenseDate.Month, expense.ExpenseDate.Day));

        //    return DBLayer.SaveExpense(expense);
        //}

        public static BusinessResponse RecordTracking(Tracking tracking)
        {
            BusinessResponse br = new BusinessResponse();
            long trackingRecordId = DBLayer.ProcessTrackingRequest(tracking);

            if (trackingRecordId == 0)
            {
                br.EmployeeDayId = -1;
                br.Content = "Invalid Id or day is already closed.";
            }
            else if (trackingRecordId == -1)
            {
                br.EmployeeDayId = -1;
                br.Content = "DB call returned a null value";
            }
            else if (trackingRecordId == -2)
            {
                br.EmployeeDayId = -2;
                br.Content = "No change in coordinates.";
            }
            else
            {
                br.EmployeeDayId = tracking.EmployeeDayId;
                br.Content = $"Success {trackingRecordId}";
            }

            return br;
        }

        public static string TrackingLogData(int trackingLogId)
        {
            return DBLayer.TrackingLogData(trackingLogId);
        }

        public static BusinessResponse RecordActivity(Activity activity)
        {
            BusinessResponse br = new BusinessResponse();

            long activityRecordId = DBLayer.ProcessActivityRequest(activity);
            if (activityRecordId == 0)
            {
                br.EmployeeDayId = -1;
                br.Content = "Invalid Id or day is already closed.";
            }
            else if (activityRecordId == -1)
            {
                br.EmployeeDayId = -1;
                br.Content = "DB call returned a null value";
            }
            else
            {
                br.EmployeeDayId = activity.EmployeeDayId;
                br.Content = $"Success {activityRecordId}";

                // create a tracking request as well
                Tracking tracking = new Tracking()
                {
                    At = activity.At,
                    IsMileStone = true,
                    IsStartOfDay = false,
                    IsEndOfDay = false,
                    EmployeeDayId = activity.EmployeeDayId,
                    ActivityId = activityRecordId,
                    Latitude = activity.Latitude,
                    Longitude = activity.Longitude
                };
                RecordTracking(tracking);
            }

            return br;
        }

        public static ICollection<DashboardDataSet> DashboardData(DateTime startDate, DateTime endDate,
                                            bool includeTrackingData = false)
        {
            return DBLayer.DashboardData(startDate, endDate, includeTrackingData);
        }

        public static ICollection<DayRecord> DaysTable(DateTime startDate, DateTime endDate)
        {
            return DBLayer.DaysTable(startDate.Date, endDate.Date);
        }

        public static ICollection<EmployeeRecord> Users(CRMUsersFilter searchCriteria = null)
        {
            return DBLayer.TenantEmployees(searchCriteria);
        }

        public static ICollection<ActivityRecord> ActivityData(long employeeDayId)
        {
            return DBLayer.ActivityData(employeeDayId);
        }

        public static ICollection<TrackingRecord> TrackingData(long employeeDayId)
        {
            return DBLayer.TrackingData(employeeDayId);
        }

        /// <summary>
        /// Gives the list of active employee Id on an IMEI
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static IEnumerable<long> GetActiveEmployees(string imei)
        {
            if (string.IsNullOrEmpty(imei))
            {
                return new List<long>();
            }

            return DBLayer.GetActiveEmployees(imei.Trim().ToUpper());
        }

        public static UserRecord RegisterUser(RegisterUserData userData)
        {
            //UserRecord userRecord = Business.GetUserRecord(userData.IMEI);
            //if (userRecord.EmployeeId <= 0)

            userData.EmployeeCode = userData.EmployeeCode.Trim().ToUpper();
            userData.IMEI = userData.IMEI.Trim().ToUpper();

            // put staff on auto upload, if so configured.
            userData.AutoUploadStaus = Utils.SiteConfigData.PutNewStaffOnAutoUpload && Utils.SiteConfigData.AutoUploadFromPhone;

            UserRecord userRecord = DBLayer.RegisterUser(userData);

            return userRecord;
        }

        public static bool IsPhoneValidForEmployeeCode(string employeeCode, string phoneNumber)
        {
            var salesPerson = DBLayer.GetSingleSalesPerson(employeeCode);
            if (salesPerson == null)
            {
                return false;
            }

            if (Utils.SiteConfigData.IgnorePhoneCheckOnRegisterRequest)
            {
                return true;
            }

            return (salesPerson.Phone ?? "").Equals(phoneNumber, StringComparison.OrdinalIgnoreCase);
        }

        public static SalesPerson GetSalesPerson(string employeeCode)
        {
            return DBLayer.GetSingleSalesPerson(employeeCode);
        }

        /// <summary>
        /// Method is used to detect if user is supported on current site.
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsValidUser(string employeeCode, string phoneNumber)
        {
            if (String.IsNullOrEmpty(employeeCode) || String.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            var salesPerson = DBLayer.GetSingleSalesPerson(employeeCode);
            if (salesPerson == null || salesPerson.IsActive == false)
            {
                return false;
            }

            if (!(salesPerson?.Phone ?? "").Equals(phoneNumber, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // now check user status in TenantEmployee Table
            EmployeeRecord tenantEmployee = DBLayer.GetSingleTenantEmployee(employeeCode);
            if (tenantEmployee == null)
            {
                // user may not exist in tenant employee - did not register yet, so a valid user.
                return true;
            }

            if (tenantEmployee.IsActive == false)
            {
                // user has been deactivated - so can't register on this site.
                return false;
            }

            return true;
        }

        public static void CalculateTrackingDistance()
        {
            Task mainTask = Task.Factory.StartNew(() =>
            {
                // perform this loop for all records (until there are no records to process)
                do
                {
                    IEnumerable<TrackingRecordForDistance> availableSet = DBLayer.GetTrackingRecordsForDistanceCalculation(10);
                    if (availableSet == null || availableSet.Count() == 0)
                    {
                        return;
                    }

                    //ICalculateDistance bingCalcDist = new BingMaps();
                    ICalculateDistance linearCalcDist = new LinearMaps();
                    ICalculateDistance googleCalcDist = new GoogleMaps();
                    foreach (var rec in availableSet)
                    {
                        //if (rec.IsStartOfDay)
                        //{
                        //    // if it was start of day then we did invoke google API only
                        //    // to get location name; hence set the distance to 0, as
                        //    // start coordinate in this case is zero and we don't want
                        //    // to have distance from ocean.
                        //    rec.GoogleMapsDistanceInMeters = 0;
                        //    rec.LinearDistanceInMeters = 0;
                        //    rec.BingMapsDistanceInMeters = 0;
                        //    rec.GoogleEndAddress = rec.EndCoorindates;
                        //    continue;
                        //}

                        //if (rec.IsMileStone)
                        {
                            // invoke bing and google maps api to calculate distances
                            // only for milestone entries - where activity is performed.

                            // Oct 1 2017 - no more calculation using Bing
                            //try
                            //{

                            //    bingCalcDist.CalculateDistanceInMeters(rec);
                            //}
                            //catch (Exception ex)
                            //{
                            //    rec.BingMapsDistanceInMeters = -1;
                            //    rec.BingMapError = true;
                            //    rec.BingMapErrorDetails += "; " + ex.ToString();
                            //}

                            try
                            {
                                googleCalcDist.CalculateDistanceInMeters(rec);
                            }
                            catch (Exception ex)
                            {
                                rec.GoogleMapsDistanceInMeters = -1;
                                rec.GoogleMapError = true;
                                rec.GoogleMapErrorDetails += "; " + ex.ToString();
                            }
                        }

                        if (rec.IsStartOfDay)
                        {
                            rec.LinearDistanceInMeters = 0;
                        }
                        else
                        {
                            linearCalcDist.CalculateDistanceInMeters(rec);
                        }
                    }

                    DBLayer.UpdateTrackingRecordsForDistance(availableSet);
                } while (true);
            },
            TaskCreationOptions.LongRunning);

            mainTask.ContinueWith((p) =>
            {
                long errorLogId = 0;
                p.Exception?.Handle(ex =>
                {
                    errorLogId = Business.LogError(nameof(CalculateTrackingDistance), ex);
                    return true;
                });

                Task.Run(async () =>
                {
                    await Business.LogAlert("EDCFAULT", $"Error occured in Distance Calculation; Log Id {errorLogId}");
                });

            }, TaskContinuationOptions.OnlyOnFaulted);

            mainTask.ContinueWith((p) =>
            {
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static Return GetReturn(long returnsId)
        {
            return DBLayer.GetReturn(returnsId);
        }

        public static ICollection<Return> GetReturns(DomainEntities.SearchCriteria searchCriteria)
        {
            return DBLayer.GetReturns(searchCriteria);
        }

        public static IEnumerable<ReturnItem> GetReturnItems(long returnsId)
        {
            return DBLayer.GetReturnItems(returnsId);
        }

        public static int SaveApproveAmount(ApprovalData approvalData, bool isSuperAdmin, string currentUserStaffCode)
        {
            string reportType = approvalData.ReportType;
            if (reportType.Equals(Constant.OrderType, StringComparison.OrdinalIgnoreCase))
            {
                return DBLayer.ApproveOrder(approvalData);
            }

            if (reportType.Equals(Constant.PaymentType, StringComparison.OrdinalIgnoreCase))
            {
                return DBLayer.ApprovePayment(approvalData);
            }

            if (reportType.Equals(Constant.ReturnType, StringComparison.OrdinalIgnoreCase))
            {
                return DBLayer.ApproveReturn(approvalData);
            }

            if (reportType.Equals(Constant.ExpenseType, StringComparison.OrdinalIgnoreCase))
            {
                SalesPersonLevel level = GetHighestLevel(isSuperAdmin, currentUserStaffCode);
                return DBLayer.ApproveExpense(approvalData, level);
            }

            return 4;
        }

        public static ApprovalData GetExpenseApprovalDataAtCurrentUserLevel(long expenseId, SalesPersonLevel level)
        {
            ICollection<ExpenseApproval> expenseApprovals = Business.GetExpenseApprovals(expenseId);

            // get current level and find corresponding approval data
            ApprovalData approvalRecord = expenseApprovals
                                .Where(x => x.ApproveLevel.Equals(level.ToString(), StringComparison.OrdinalIgnoreCase))
                                .Select(x => new ApprovalData()
                                {
                                    ApproveComments = x.ApproveNotes,
                                    ApprovedAmt = x.ApproveAmount,
                                    ApprovedBy = x.ApprovedBy,
                                    ApprovedDate = x.ApproveDate,
                                    ApproveRef = x.ApproveRef,
                                    IsApproved = true,
                                })
                                .FirstOrDefault();

            if (approvalRecord == null)
            {
                // check to see if expense is approved by higher level
                // retrieve expense record
                // (when expense is approved at higher level, lower level flags are set as 1)
                Expense expenseRecord = Business.GetExpense(expenseId);
                if ((level == SalesPersonLevel.Territory && expenseRecord.IsTerritoryApproved) ||
                    (level == SalesPersonLevel.Area && expenseRecord.IsAreaApproved) ||
                    (level == SalesPersonLevel.Zone && expenseRecord.IsZoneApproved))
                {
                    approvalRecord = new ApprovalData()
                    {
                        IsApproved = true,
                        ApproveComments = "",
                        ApprovedAmt = 0,
                        ApprovedBy = "",
                        ApprovedDate = DateTime.MinValue,
                        ApproveRef = ""
                    };
                }
            }

            return approvalRecord;
        }

        public static ICollection<DomainEntities.SalesPerson> GetSalesPersons()
        {
            return DBLayer.GetSalesPersons();
        }

        public static IEnumerable<DomainEntities.SalesPersonsAssociation> GetSalesPersonsAssociation(string level, string code)
        {
            return DBLayer.GetSalesPersonsAssociation(level, code);
        }

        public static IEnumerable<DomainEntities.SalesPersonsAssociation> GetSalesPersonAssignmentData(string employeeCode, string level) //get Areas for salesperson
        {
            IEnumerable<DomainEntities.SalesPersonsAssociationData> salesPersonAssignmentData =
                DBLayer.GetStaffAssociations(Utils.SiteConfigData.TenantId, employeeCode);

            return (from s in salesPersonAssignmentData
                    where s.CodeType == level
                    select new DomainEntities.SalesPersonsAssociation()
                    {
                        CodeValue = s.Code,
                        CodeName = s.CodeName
                    }).ToList();
        }

        public static bool SaveSalesPersonAssignmentData(string staffCode, string level, string[] assignedRegioncodes, string currentUser)
        {
            if (assignedRegioncodes == null)
            {
                assignedRegioncodes = new string[] { };
            }

            IEnumerable<DomainEntities.SalesPersonsAssociationData> salesPersonAssignmentData =
               DBLayer.GetStaffAssociations(Utils.SiteConfigData.TenantId, staffCode);

            string[] existingRegionCodes = salesPersonAssignmentData.Where(s => s.CodeType.ToLower().Equals(level.ToLower())).Select(s => s.Code).ToArray();

            return DBLayer.SaveSalesPersonAssignmentData(staffCode, level, existingRegionCodes, assignedRegioncodes, currentUser);
        }

        public static bool SaveAssignedSalesPersons(string[] staffCodes, string level, string code, string approvedBy)
        {
            if (staffCodes == null)
            {
                staffCodes = new string[] { };
            }

            return DBLayer.SaveAssignedSalesPersons(staffCodes, level, code, approvedBy);
        }

        public static ICollection<DomainEntities.SalesPersonsAssociationData> GetStaffAssociations(string staffCode)
        {
            return DBLayer.GetStaffAssociations(Utils.SiteConfigData.TenantId, staffCode);
        }

        /// <summary>
        /// Get the list of area codes that a user is managing
        /// (as per associations defined)
        /// This is used for Exec Crm App - to return data only for areas that a CRM user is managing.
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetManagingAreaCodes(string imei)
        {
            // first get tenant Employee record that is using this imei
            TenantEmployee te = DBLayer.GetEmployeeRecord(imei);
            if (te == null || te.IsActive == false || te.ExecAppAccess == false)
            {
                return new List<string>();
            }

            // now get defined staff associations for this tenant employee
            ICollection<SalesPersonsAssociationData> data = DBLayer.GetStaffAssociations(
                                Utils.SiteConfigData.TenantId, te.EmployeeCode);

            List<string> resultAreaCodes = data
                        .Where(x => x.CodeType.Equals("AreaOffice", StringComparison.OrdinalIgnoreCase))
                        .Select(x => x.Code)
                        .ToList();

            // a user may be assigned to Zones as well - find all areas that come in zones
            List<string> assignedZones = data
                        .Where(x => x.CodeType.Equals("Zone", StringComparison.OrdinalIgnoreCase))
                        .Select(x => x.Code)
                        .ToList();

            if ((assignedZones?.Count ?? 0) > 0)
            {
                IEnumerable<OfficeHierarchy> oh = GetAssociations();
                Parallel.ForEach(assignedZones, az =>
                {
                    resultAreaCodes.AddRange(
                        oh.Where(x => x.ZoneCode.Equals(az, StringComparison.OrdinalIgnoreCase))
                            .Select(x => x.AreaCode)
                            .ToList()
                        );
                });
            }

            // return unique area codes
            return resultAreaCodes.Distinct();
        }

        /// <summary>
        /// Get the list of codes that a user is managing
        /// (as per associations defined)
        /// This is used for Exec Crm App - to return data only for codes that a CRM user is managing.
        /// </summary>
        /// <param name="imei"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetManagingCodes(TenantEmployee te, ExecAppRollupEnum rollUp)
        {
            if (te == null || te.IsActive == false || te.ExecAppAccess == false)
            {
                return new List<string>();
            }

            IEnumerable<OfficeHierarchy> oh2 = GetSelectableAssociations(te.EmployeeCode);
            List<string> selectedCodes = null;
            switch (rollUp)
            {
                case ExecAppRollupEnum.Zone:
                    selectedCodes = oh2.Where(x => x.IsZoneSelectable).Select(x => x.ZoneCode).ToList();
                    break;
                case ExecAppRollupEnum.Area:
                    selectedCodes = oh2.Where(x => x.IsAreaSelectable).Select(x => x.AreaCode).ToList();
                    break;
                case ExecAppRollupEnum.Territory:
                    selectedCodes = oh2.Where(x => x.IsTerritorySelectable).Select(x => x.TerritoryCode).ToList();
                    break;
                case ExecAppRollupEnum.HQ:
                    selectedCodes = oh2.Where(x => x.IsHQSelectable).Select(x => x.HQCode).ToList();
                    break;
                default:
                    selectedCodes = new List<string>();
                    break;
            }

            return selectedCodes.Distinct();

            /*

            // now get defined staff associations for this tenant employee
            ICollection<SalesPersonsAssociationData> data = DBLayer.GetStaffAssociations(
                            Utils.SiteConfigData.TenantId, te.EmployeeCode);

            // Zones
            List<string> resultZones = data
                .Where(x => x.CodeType.Equals("Zone", StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Code)
                .ToList();

            if (rollUp == ExecAppRollupEnum.Zone)
            {
                return resultZones.Distinct();
            }

            //////////////////////////////////////

            IEnumerable<OfficeHierarchy> oh = GetAssociations();

            // Area
            List<string> resultAreaCodes = data
                        .Where(x => x.CodeType.Equals("AreaOffice", StringComparison.OrdinalIgnoreCase))
                        .Select(x => x.Code)
                        .ToList();

            // a user may be assigned to Zones as well - find all areas that come in zones
            if ((resultZones?.Count ?? 0) > 0)
            {
                foreach(string az in resultZones)
                {
                    resultAreaCodes.AddRange(
                        oh.Where(x => x.ZoneCode.Equals(az, StringComparison.OrdinalIgnoreCase))
                            .Select(x => x.AreaCode)
                        );
                }
            }

            if (rollUp == ExecAppRollupEnum.Area)
            {
                return resultAreaCodes.Distinct();
            }
            //////////////////////////////////////

            // Territory
            List<string> resultTerritoryCodes = data
                        .Where(x => x.CodeType.Equals("Territory", StringComparison.OrdinalIgnoreCase))
                        .Select(x => x.Code)
                        .ToList();

            // find all territories that comes under Areas
            if ((resultAreaCodes?.Count ?? 0) > 0)
            {
                foreach(string az in resultAreaCodes)
                {
                    resultTerritoryCodes.AddRange(
                        oh.Where(x => x.AreaCode.Equals(az, StringComparison.OrdinalIgnoreCase))
                        .Select(x => x.TerritoryCode)
                        );
                }
            }

            if (rollUp == ExecAppRollupEnum.Territory)
            {
                return resultTerritoryCodes.Distinct();
            }
            //////////////////////////////////////

            // HQ
            List<string> resultHQCodes = data
                        .Where(x => x.CodeType.Equals("HeadQuarter", StringComparison.OrdinalIgnoreCase))
                        .Select(x => x.Code)
                        .ToList();

            // a user may be assigned to Territories as well - find all HQ that come in zones
            if ((resultTerritoryCodes?.Count ?? 0) > 0)
            {
                foreach(string az in resultTerritoryCodes)
                {
                    resultHQCodes.AddRange(
                        oh.Where(x => x.TerritoryCode.Equals(az, StringComparison.OrdinalIgnoreCase))
                            .Select(x => x.HQCode)
                        );
                }
            }

            if (rollUp == ExecAppRollupEnum.HQ)
            {
                return resultHQCodes.Distinct();
            }

            return new List<String>();
            */
        }

        /// <summary>
        /// Can new users register on Dashboard?
        /// </summary>
        /// <returns></returns>
        public static bool DashboardRegistrationAllowed()
        {
            int allowedCount = Utils.SiteConfigData.MaxDashboardUsers;

            // Now get the number of registered dashboard users
            ICollection<DashboardUser> dashboardUsers = DBLayer.GetRegisteredWebPortalUsers();
            return (dashboardUsers.Count < allowedCount);
        }

        /// <summary>
        /// This will return the users who are not locked out
        /// </summary>
        /// <returns></returns>
        public static ICollection<DashboardUser> GetAllowedWebPortalUsers()
        {
            return DBLayer.GetAllowedWebPortalUsers();
        }

        /// <summary>
        /// Office hierarchy for particular staff code other than Super Admin
        /// </summary>
        /// <param name="staffCode"></param>
        /// <returns></returns>
        public static ICollection<OfficeHierarchy> GetAssociations(string staffCode)
        {
            if (String.IsNullOrEmpty(staffCode))
            {
                return new List<OfficeHierarchy>();
            }

            return DBLayer.GetDetailedAssociations(Utils.SiteConfigData.TenantId, staffCode);
        }

        /// <summary>
        /// Get Office Hierarchy for Super Admin
        /// </summary>
        /// <param name="staffCode"></param>
        /// <returns></returns>
        public static ICollection<OfficeHierarchy> GetAssociations()
        {
            ICollection<OfficeHierarchy> oh = DBLayer.GetDetailedAssociations(Utils.SiteConfigData.TenantId);

            // For super admin, make any zone/area/territory/hq as selectable
            oh.Select(x =>
            {
                x.IsZoneSelectable = x.IsAreaSelectable = x.IsTerritorySelectable = x.IsHQSelectable = true;
                return 1;
            }).ToList();

            return oh;
            //return DBLayer.GetDetailedAssociations(Utils.SiteConfigData.TenantId);
        }

        /// <summary>
        /// Based on search criteria and associations, returns the list of staff codes
        /// that should be removed from returned dataset;
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="inputStaffCodes"></param>
        /// <returns></returns>
        private static ICollection<string> GetExclusionList(BaseSearchCriteria searchCriteria, IEnumerable<string> uniqueStaffCodes)
        {
            // these are the staff codes that the current user has permission to see
            ICollection<string> inclusionList = Business.GetVisibleStaffCodes(searchCriteria.IsSuperAdmin, searchCriteria.CurrentUserStaffCode);

            // start building exclusion list
            List<string> exclusionList = uniqueStaffCodes.Except(inclusionList).ToList();

            // these are the HQ codes that user has asked the data for
            var hqFilterList = DBLayer.GetFilteringHQCodes(searchCriteria);

            if (hqFilterList == null)
            {
                // no filters requested by the user;
                return exclusionList;
            }

            // get the list of visible staff codes also
            List<string> currentInclusionList = uniqueStaffCodes.Intersect(inclusionList).ToList();

            IEnumerable<OfficeHierarchyForAll> detailedAssociationsForAll = DBLayer.GetDetailedAssociationsForAll(Utils.SiteConfigData.TenantId);

            // for each staff code in inclusion list - get the list of HQ codes from Associations table
            foreach (string sc in currentInclusionList)
            {
                //var scHQCodes = DBLayer.GetDetailedAssociations(sc).Select(x => x.HQCode).ToList();
                var scHQCodes = detailedAssociationsForAll.Where(x => x.StaffCode == sc)
                                .Select(x => x.HQCode).ToList();

                // now we will keep the data of this staff code only if its hqCodes meets the criteria;
                // (applying the user specified filter here)
                if (scHQCodes.Intersect(hqFilterList).Count() == 0)
                {
                    exclusionList.Add(sc);
                }
            } // end of foreach

            return exclusionList;
        }

        /// <summary>
        /// Give the list of staff codes that are visible to current logged in user
        /// </summary>
        /// <returns></returns>
        public static SalesPersonLevel GetHighestLevel(bool isSuperAdmin, string currentUserStaffCode)
        {
            if (isSuperAdmin)
            {
                return SalesPersonLevel.Zone;
            }

            // find out the highest rank for currentLoggedInUser
            int currentLoggedInUserRank = DBLayer.GetStaffAssociations(Utils.SiteConfigData.TenantId, currentUserStaffCode)
                                    .Select(x => GetRankForCodeType(x.CodeType))
                                    .OrderByDescending(x => x).FirstOrDefault();

            SalesPersonLevel level = SalesPersonLevel.None;
            switch (currentLoggedInUserRank)
            {
                case 40: level = SalesPersonLevel.Zone; break;
                case 30: level = SalesPersonLevel.Area; break;
                case 20: level = SalesPersonLevel.Territory; break;
                default: level = SalesPersonLevel.None; break;
            }

            return level;
        }

        /// <summary>
        /// Give the list of staff codes that are visible to current logged in user
        /// </summary>
        /// <returns></returns>
        public static ICollection<string> GetVisibleStaffCodes(bool isSuperAdmin, string currentUserStaffCode)
        {
            ICollection<string> uniqueStaffCodes = Business.GetSalesPersons().Select(x => x.StaffCode).ToList();

            // June 15 2020
            if (isSuperAdmin)
            {
                return uniqueStaffCodes;
            }

            List<string> inclusionList = new List<string>();

            IEnumerable<OfficeHierarchyForAll> officeHierarchyForAll = DBLayer.GetDetailedAssociationsForAll(Utils.SiteConfigData.TenantId);
            ICollection<SalesPersonsAssociationDataForAll> allStaffAssociations
                = DBLayer.GetStaffAssociationsForAll(Utils.SiteConfigData.TenantId);

            // find all hq codes that current logged in user has permission to see
            //var currentLoggedInUserHQCodes = DBLayer.GetDetailedAssociations(currentUserStaffCode)
            //                        .Select(x => x.HQCode).ToList();
            var currentLoggedInUserHQCodes = officeHierarchyForAll.Where(x => x.StaffCode == currentUserStaffCode)
                                    .Select(x => x.HQCode).ToList();

            // find out the highest rank for currentLoggedInUser
            //int currentLoggedInUserRank = DBLayer.GetStaffAssociations(currentUserStaffCode)
            //                        .Select(x => GetRankForCodeType(x.CodeType))
            //                        .OrderByDescending(x => x).FirstOrDefault();

            int currentLoggedInUserRank = allStaffAssociations.Where(x => x.StaffCode == currentUserStaffCode)
                                    .Select(x => GetRankForCodeType(x.CodeType))
                                    .OrderByDescending(x => x).FirstOrDefault();

            // don't need this now after a later update that AO1 should be able to see AO2 expenses
            // given AO1 and AO2 are Area Managers for same Area
            //ICollection<DashboardUser> registeredWebPortalUsers = DBLayer.GetRegisteredWebPortalUsers();

            // for each staff code - get the list of HQ codes from Associations table
            foreach (string sc in uniqueStaffCodes)
            {
                //IEnumerable<OfficeHierarchy> expenseUserAssociations = DBLayer.GetDetailedAssociations(sc);
                // var scHQCodes = expenseUserAssociations.Select(x => x.HQCode).ToList();

                bool keepStaffCode = false;
                do
                {
                    // super admin - allow all
                    // June 15 - see top of method - for SuperAdmin we return immediately.
                    //if (isSuperAdmin)
                    //{
                    //    keepStaffCode = true;
                    //    break;
                    //}

                    // do show user's own expenses
                    if (sc == currentUserStaffCode)
                    {
                        // (subsequent discussion: allow user to see his own expense - say HQ incharge can see his own expenses
                        // + all Sales Person in that HQ)
                        keepStaffCode = true;
                        break;
                    }

                    var scHQCodes = officeHierarchyForAll.Where(x => x.StaffCode == sc)
                        .Select(x => x.HQCode)
                        .ToList();

                    // if HQ Codes don't intersect - don't include the sc
                    if (currentLoggedInUserHQCodes.Intersect(scHQCodes).Count() == 0)
                    {
                        keepStaffCode = false;
                        break;
                    }

                    // if the staff code of the tenant employee is higher up, don't show the entry

                    // find out the highest right for sc - staff code from tenant employee
                    // for this get the raw associations data from SalesPersonAssociations table;
                    //int highestRankForStaffCode = DBLayer.GetStaffAssociations(sc)
                    int highestRankForStaffCode = allStaffAssociations.Where(x => x.StaffCode == sc)
                                        .Select(x => GetRankForCodeType(x.CodeType))
                                        .OrderByDescending(x => x)
                                        .FirstOrDefault();

                    // if there is no association found for sc; we don't want this sc - ideally all
                    // sales person should  have an association
                    if (highestRankForStaffCode == 0)
                    {
                        keepStaffCode = false;
                        break;
                    }

                    // if sc's rank is higher than current user's rank - don't want it;
                    if (highestRankForStaffCode > currentLoggedInUserRank)
                    {
                        keepStaffCode = false;
                        break;
                    }

                    // if sc's rank is equal to current user's rank, and sc has access to web portal
                    // that means sc is a sibling of current logged in user
                    // (this covers the case when HQIncharge can view the expenses of sales person in his HQ
                    //  Now if there are two Officers, assigned to same area, then AO1, won't see the personal expenses of AO2
                    //         assumption is that AO2 has signed up for web portal - if AO2 has not signed up for web portal,
                    //         then AO1 will be able to see personal expenses of AO2
                    //
                    // Later Update: irrespective of AO2 signed up for web portal or not, AO1 should be able to see AO2 expense
                    // hence commenting following code
                    //
                    //if (highestRankForStaffCode == currentLoggedInUserRank && registeredWebPortalUsers.Any(x => x.UserName == sc))
                    //{
                    //    keepStaffCode = false;
                    //    break;
                    //}

                    // include the user only if HQ codes intersect

                    keepStaffCode = true;
                } while (false); // this is put to execute the loop once only and to use break statements in between the construct

                // if we are allowed to keep staff code, add it to inclusion list
                if (keepStaffCode)
                {
                    inclusionList.Add(sc);
                }
            } // end of foreach

            return inclusionList;
        }

        public static TOPItemsSummary GetTopItemsData(SearchCriteria sc, int topItemsCount)
        {
            TOPItemsSummary topItemsSummary = new TOPItemsSummary();

            // 1.
            //ICollection<Order> orders = Business.GetOrders(sc);
            //IEnumerable<long> orderIds = orders.Select(x => x.Id).ToList();
            // now we have orders that have filter / security applied
            // now get products data for these orders
            topItemsSummary.Products = DBLayer.GetTopSellingProducts(sc, topItemsCount);

            // 2.
            //ICollection<Return> returns = Business.GetReturns(sc);
            // now we have returns that have filter / security applied
            // now get return data for these filtered items
            topItemsSummary.Returns = DBLayer.GetTopReturnProducts(sc, topItemsCount);

            // 3.
            topItemsSummary.SalesPersonsByOrders = DBLayer.GetTopSalesPersonsByOrders(sc, topItemsCount);

            // 4.
            //ICollection<Payment> payments = Business.GetPayments(sc);
            topItemsSummary.SalesPersonsByPayments = DBLayer.GetTopSalesPersonsByPayments(sc, topItemsCount);

            return topItemsSummary;
        }

        public static bool IsSMSEnabled(long tenantId) => Utils.SiteConfigData.IsSMSEnabled;

        /// <summary>
        /// Determine if SMS is to be sent today or not - e.g. no SMS on company holiday or on Sunday
        ///
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public static bool IsDayForSMS(long tenantId, DateTime currentDateTime)
        {
            DateTime justDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day);

            // get list of upcoming holidays for tenant - if company holiday - no SMS to be sent
            ICollection<TenantHoliday> tenantHolidays = DBLayer.GetUpcomingHoidays(tenantId, justDate);
            if (tenantHolidays.Any(x => x.HolidayDate == justDate))
            {
                return false;
            }

            // check whether SMS is to be sent today or not - e.g. no SMS on Sunday
            ICollection<DomainEntities.TenantWeekDay> weekDays = DBLayer.GetWeekDays(tenantId);
            string dayName = justDate.DayOfWeek.ToString();
            return IsWorkingDay(weekDays, dayName);
        }

        public static bool IsTimeForSMS(long tenantId, TenantSmsType smsType, DateTime currentDateTime)
        {
            DateTime justDate = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day);
            string dayName = justDate.DayOfWeek.ToString();

            // pick up scheduled times at which SMS have to be sent
            ICollection<TimeSpan> scheduledSMSTimes = DBLayer.TenantSMSSchedule(tenantId, smsType.Id, dayName);
            // if no schedules defined for today - return false;
            if ((scheduledSMSTimes?.Count ?? 0) == 0)
            {
                return false;
            }

            // convert schedules set - e.g. 9am 11am, 1 pm, 3pm
            // into today's date
            IEnumerable<DateTime> todayScheduleForSMS = scheduledSMSTimes.Select(x => new DateTime(justDate.Year, justDate.Month, justDate.Day, x.Hours, x.Minutes, x.Seconds))
                                                        .ToList();

            // Get last timestamp when SMS was sent - if there was no SMS sent so far, lastSMSTime will have minValue
            DateTime lastSMSTime = DBLayer.GetRecentSMSSentTime(tenantId, smsType.TypeName);

            DateTime nextSmsTime = GetNextSMSTime(todayScheduleForSMS, lastSMSTime);

            // now check if currenttime >= nextSmsTime - we should send Sms
            return (nextSmsTime != DateTime.MinValue && currentDateTime >= nextSmsTime);
        }

        // this method has unit tests
        public static DateTime GetNextSMSTime(IEnumerable<DateTime> todaySchedules, DateTime lastSmsTime)
        {
            // Now look for next time that SMS should be sent
            // if we have missed a scheduled SMS, (say schedules were 9, 11, 1pm and 3pm)
            // and we are at 9:30am right now, last SMS was sent y'day at 3pm
            // (effectively we have missed 9am schedule for today), but the following
            // query will return 9am, which we can compare with current time and hence send the sms
            // that was to be sent at 9am at 9:30.
            //
            // Likewise, if last sms sent is at 3:05pm and there are no more schedules
            // nextSMSTime will have a MinValue
            return todaySchedules
                .OrderBy(x => x)
                .Where(x => x > lastSmsTime)
                .FirstOrDefault();
        }

        // unit tests - yes
        public static bool IsWorkingDay(ICollection<TenantWeekDay> weekDays, string dayName)
        {
            return weekDays.Any(x => x.IsWorkingDay && x.WeekDayName.Equals(dayName, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<SalesPersonEx> GetAssignedManagers(string managerType)
        {
            return DBLayer.GetAssignedManagers(managerType);
        }

        //public static string SaveImageDataInFile(byte[] binaryData, string fileNamePrefix = "")
        //{
        //    // retrieve path
        //    string imagesFolder = Utils.SiteConfigData.ImagesFolder;
        //    if (string.IsNullOrEmpty(imagesFolder))
        //    {
        //        throw new InvalidOperationException("Images folder not specified in config");
        //    }

        //    string guid = Guid.NewGuid().ToString();
        //    string imageFileName = $"{fileNamePrefix}{Guid.NewGuid().ToString()}.jpg";

        //    string completeOutputFileName = System.IO.Path.Combine(imagesFolder, imageFileName);
        //    System.IO.File.WriteAllBytes(completeOutputFileName, binaryData);

        //    return imageFileName;
        //}

        public static string SaveImageDataInFile(byte[] binaryData, string fileNamePrefix = "")
        {
            return LocalDiskFacade.SaveImageData(binaryData, fileNamePrefix);
        }

        // file name is formed based on id given - which has fileNamePrefix - as formed on phone.
        public static bool SaveImageDataInFile(byte[] binaryData, string id, string saveFileType)
        {
            return LocalDiskFacade.SaveImageData(binaryData, id, saveFileType);
        }

        internal static bool ForceEndPreviousBatchOnStartDay => Utils.SiteConfigData.ForceEndPreviousBatchOnStartDay;

        public static ICollection<ActivityByTypeReportData> GetActivityByTypeReportDataSet(SearchCriteria searchCriteria)
        {
            ICollection<EmployeeRecord> employeeRecords = GetActivityReportUsers(searchCriteria);
            ICollection<DayRecord> dayRecords = Business.DaysTable(searchCriteria.DateFrom, searchCriteria.DateTo);

            List<long> employeeIds = employeeRecords.Select(x => x.EmployeeId).ToList();
            List<long> dayIds = dayRecords.Select(x => x.Id).ToList();

            ICollection<ActivityByTypeReportData> activityByTypeData = DBLayer.GetActivityByTypeReportData(employeeIds, dayIds);

            // now fill in Date/Name/StaffCode values in the returned structure
            Parallel.ForEach(activityByTypeData, ds =>
            {
                var er = employeeRecords.FirstOrDefault(x => x.EmployeeId == ds.TenantEmployeeId);
                var dr = dayRecords.FirstOrDefault(x => x.Id == ds.DayId);
                ds.Date = dr?.DATE ?? DateTime.MinValue;
                ds.Name = er?.Name ?? "";
                ds.StaffCode = er?.EmployeeCode ?? "";
            });

            return activityByTypeData;
        }

        public static ICollection<string> GetStaffPhoneNumbers(IEnumerable<string> staffCodes)
        {
            return Business.GetSelectedSalesPersonData(staffCodes)
                .Where(x => x.IsActive && (x.TenantEmployeeExist == false || (x.TenantEmployeeExist && x.TenantEmployeeIsActive)))
                .Select(x => x.Phone)
                .Distinct().ToList();
        }

        public static ICollection<TenantSmsType> GetSmsTypes(long tenantId = 0)
        {
            return DBLayer.GetSmsTypes(tenantId);
        }

        public static IEnumerable<string> GetStaffCodesForSms(long tenantId, DateTime currentIstDateTime, string sprocName)
        {
            return DBLayer.GetStaffCodesForSms(tenantId, currentIstDateTime, sprocName);
        }

        public static IEnumerable<DashboardBankAccount> GetDashboardBankAccount(BankAccountFilter searchCriteria)
        {
            IEnumerable<DashboardBankAccount> bankAccounts = DBLayer.GetBankAccounts(searchCriteria);

            IEnumerable<CodeTableEx> areaList = Business.GetCodeTable("AreaOffice");

            Parallel.ForEach(bankAccounts, ad =>
            {
                var area = areaList.FirstOrDefault(x => x.Code.Equals(ad.AreaCode, StringComparison.OrdinalIgnoreCase));
                ad.AreaName = area?.CodeName ?? "";
            });

            return bankAccounts;
        }

        public static long CreateBankAccountData(DashboardBankAccount eb)
        {
            return DBLayer.CreateBankAccount(eb);
        }

        public static void SaveBankAccountData(DashboardBankAccount eb)
        {
            DBLayer.SaveBankData(eb);
        }

        // extension method to format amount
        public static string Format(this decimal amount)
        {
            return amount.ToString("#,##,##,##,##0.00", CultureInfo.CreateSpecificCulture("en-IN"));
        }

        // extension method to format Date
        public static string Format(this DateTime dt)
        {
            return dt.ToString("dd-MM-yyyy");
        }

        public static string Format(this TimeSpan ts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{ts.Hours} hrs");

            if (ts.Minutes > 0)
            {
                sb.Append($" {ts.Minutes} mins");
            }

            return sb.ToString().Trim();
        }

        public static DashboardBankAccount GetBankAccountDetails(long bankAccountId)
        {
            IEnumerable<DashboardBankAccount> bankAccountDetails = DBLayer.GetBankAccountDetails(bankAccountId);
            return bankAccountDetails.FirstOrDefault();
        }

        public static double CalculateTimeInHours(DateTime startTime, DateTime endTime)
        {
            if (startTime == DateTime.MinValue || endTime == DateTime.MinValue)
            {
                return -1;
            }

            return endTime.Subtract(startTime).TotalHours;
        }

        public static long CreateCustomer(DownloadCustomerExtend dc, string currentUser)
        {
            return DBLayer.CreateCustomer(dc, currentUser);
        }

        public static long SaveCustomer(DownloadCustomerExtend dc, string currentUserStaffCode)
        {
            return DBLayer.SaveCustomer(dc, currentUserStaffCode);
        }

        public static DownloadCustomerExtend GetCustomerDetails(string Code)
        {
            return DBLayer.GetCustomerDetails(Code);
        }

        public static byte[] OrderImageData(long id, int imageItem)
        {
            string fileName = DBLayer.OrderImageData(id, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] SqliteEntityImageData(long id, int imageItem)
        {
            string fileName = DBLayer.SqliteEntityImageData(id, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] SqliteBankDetailImageData(long id, int imageItem)
        {
            string fileName = DBLayer.SqliteBankDetailImageData(id, imageItem);
            return GetImageBytes(fileName);
        }

        public static byte[] SqliteSTRImageData(long id, int imageItem)
        {
            string fileName = DBLayer.SqliteSTRImageData(id, imageItem);
            return GetImageBytes(fileName);
        }

        public static string GetMessageBarText(string staffCode)
        {
            return DBLayer.GetDailyMessage(staffCode);
        }

        public static ICollection<StaffDailyData> GetStaffDailyData(long tenantId = -1, string staffCode = null)
        {
            if (tenantId == -1)
            {
                tenantId = Utils.SiteConfigData.TenantId;
            }

            return DBLayer.GetStaffDailyData(tenantId, staffCode);
        }

        public static ICollection<ItemMaster> GetAllItemMaster()
        {
            return DBLayer.GetAllItemMaster();
        }

        public static bool IsDuplicateEntityUniqueId(long entityId, string uniqueId)
        {
            ICollection<long> dbRec = DBLayer.GetUniqueIdEntities(uniqueId);
            return dbRec.Any(x => x != entityId);
        }

        public static bool IsDuplicateEntityAgreementNumber(long agreementId, string agreementNumber)
        {
            ICollection<long> dbRec = DBLayer.GetAgreements(agreementNumber);
            return dbRec.Any(x => x != agreementId);
        }
        public static long GetSqliteBatchId(long employeeId, string batchGuid)
        {
            return DBLayer.GetSqliteBatchId(employeeId, batchGuid);
        }

        public static ICollection<TableSchema> GetTableSchema(string tableName)
        {
            return DBLayer.GetTableSchema(tableName);
        }

        public static ICollection<string> GetTableList()
        {
            return DBLayer.GetTableList();
        }

        public static Tenant GetTenantRecord(long tenantId)
        {
            var tenants = DBLayer.GetTenants();
            return tenants.FirstOrDefault(x => x.Id == tenantId);
        }

        public static ICollection<ExcelUploadStatus> GetExcelUploadStatus(long tenantId)
        {
            return DBLayer.GetExcelUploadStatus(tenantId);
        }

        public static ICollection<ExcelUploadHistory> GetExcelUploadHistory(long tenantId, int startItem, int recCount)
        {
            return DBLayer.GetExcelUploadHistory(tenantId, startItem, recCount);
        }

        public static long CreateExcelUploadStatus(ExcelUploadStatus input)
        {
            return DBLayer.CreateExcelUploadStatus(input);
        }

        public static void UpdateExcelUploadStatus(ExcelUploadStatus input)
        {
            DBLayer.UpdateExcelUploadStatus(input);
        }

        public static ICollection<DomainEntities.DivisionSegment> GetDivisionSegment(long tenantId)
        {
            return DBLayer.GetDivisionSegment(tenantId);
        }

        public static TenantEmployee GetEmployeeRecord(string imei)
        {
            return DBLayer.GetEmployeeRecord(imei);
        }

        private static LinearMaps linearMaps = new LinearMaps();
        public static decimal GetLinearDistance(decimal beginLatitude, decimal beginLongitude,
                                            decimal endLatitude, decimal endLongitude)
        {
            if (beginLatitude == 0 || beginLongitude == 0 || endLatitude == 0 || endLongitude == 0)
            {
                return Decimal.MaxValue;
            }

            TrackingRecordForDistance trackingRecord = new TrackingRecordForDistance
            {
                BeginLatitude = beginLatitude,
                BeginLongitude = beginLongitude,
                EndLatitude = endLatitude,
                EndLongitude = endLongitude
            };

            linearMaps.CalculateDistanceInMeters(trackingRecord);
            return Math.Round((trackingRecord.LinearDistanceInMeters / 1000), 2);
        }

        public static Tuple<IEnumerable<DistantActivityData>, IEnumerable<Entity>>
                    GetDistantActivityData(SearchCriteria searchCriteria)
        {
            AdvanceRequestFilter arf = new AdvanceRequestFilter()
            {
                ApplyClientNameFilter = searchCriteria.ApplyClientNameFilter,
                ClientName = searchCriteria.ClientName,

                ApplyEmployeeCodeFilter = searchCriteria.ApplyEmployeeCodeFilter,
                ApplyEmployeeNameFilter = searchCriteria.ApplyEmployeeNameFilter,
                ApplyEmployeeStatusFilter = searchCriteria.ApplyEmployeeStatusFilter,

                EmployeeCode = searchCriteria.EmployeeCode,
                EmployeeName = searchCriteria.EmployeeName,
                EmployeeStatus = searchCriteria.EmployeeStatus,

                ApplyZoneFilter = searchCriteria.ApplyZoneFilter,
                ApplyAreaFilter = searchCriteria.ApplyAreaFilter,
                ApplyTerritoryFilter = searchCriteria.ApplyTerritoryFilter,
                ApplyHQFilter = searchCriteria.ApplyHQFilter,
                Zone = searchCriteria.Zone,
                Area = searchCriteria.Area,
                Territory = searchCriteria.Territory,
                HQ = searchCriteria.HQ,
                IsSuperAdmin = searchCriteria.IsSuperAdmin,
                CurrentUserStaffCode = searchCriteria.CurrentUserStaffCode,
                TenantId = searchCriteria.TenantId,

                ApplyDateFilter = searchCriteria.ApplyDateFilter,
                DateFrom = searchCriteria.DateFrom,
                DateTo = searchCriteria.DateTo
            };

            TerminateAgreementRequestFilter tarf = new TerminateAgreementRequestFilter()
            {
                ApplyClientNameFilter = searchCriteria.ApplyClientNameFilter,
                ClientName = searchCriteria.ClientName,

                ApplyEmployeeCodeFilter = searchCriteria.ApplyEmployeeCodeFilter,
                ApplyEmployeeNameFilter = searchCriteria.ApplyEmployeeNameFilter,
                ApplyEmployeeStatusFilter = searchCriteria.ApplyEmployeeStatusFilter,

                EmployeeCode = searchCriteria.EmployeeCode,
                EmployeeName = searchCriteria.EmployeeName,
                EmployeeStatus = searchCriteria.EmployeeStatus,

                ApplyZoneFilter = searchCriteria.ApplyZoneFilter,
                ApplyAreaFilter = searchCriteria.ApplyAreaFilter,
                ApplyTerritoryFilter = searchCriteria.ApplyTerritoryFilter,
                ApplyHQFilter = searchCriteria.ApplyHQFilter,
                Zone = searchCriteria.Zone,
                Area = searchCriteria.Area,
                Territory = searchCriteria.Territory,
                HQ = searchCriteria.HQ,
                IsSuperAdmin = searchCriteria.IsSuperAdmin,
                CurrentUserStaffCode = searchCriteria.CurrentUserStaffCode,
                TenantId = searchCriteria.TenantId,

                ApplyDateFilter = searchCriteria.ApplyDateFilter,
                DateFrom = searchCriteria.DateFrom,
                DateTo = searchCriteria.DateTo
            };

            IEnumerable<IssueReturn> issueReturns = DBLayer.GetIssueReturns(searchCriteria);
            ICollection<AdvanceRequest> advanceRequests = GetAdvanceRequests(arf);
            ICollection<EntityWorkFlowDetail> workflowData = DBLayer.GetEntityWorkFlowDetails(searchCriteria);
            ICollection<TerminateAgreementRequest> terminateAgreementData = GetTerminateAgreementRequests(tarf);

            List<DistantActivityData> distantActivityData = new List<DistantActivityData>();

            // create a combined list of result set with selected fields only
            distantActivityData.AddRange(
                        issueReturns.Select(x => new DistantActivityData()
                        {
                            ActivityId = x.ActivityId,

                            TenantEmployeeId = x.EmployeeId,
                            StaffCode = x.StaffCode,
                            EmployeeName = x.EmployeeName,

                            EntityId = x.EntityId,
                            Delete = true
                        }).ToList()
            );

            distantActivityData.AddRange(
                advanceRequests.Select(x => new DistantActivityData()
                {
                    ActivityId = x.ActivityId,

                    TenantEmployeeId = x.EmployeeId,
                    StaffCode = x.EmployeeCode,
                    EmployeeName = x.EmployeeName,

                    EntityId = x.EntityId,
                    Delete = true
                }).ToList()
            );

            distantActivityData.AddRange(
                workflowData.Select(x => new DistantActivityData()
                {
                    ActivityId = x.ActivityId,

                    TenantEmployeeId = x.EmployeeId,
                    StaffCode = x.EmployeeCode,
                    EmployeeName = x.EmployeeName,

                    EntityId = x.EntityId,
                    Delete = true
                }).ToList()
            );

            distantActivityData.AddRange(
                terminateAgreementData.Select(x => new DistantActivityData()
                {
                    ActivityId = x.ActivityId,

                    TenantEmployeeId = x.EmployeeId,
                    StaffCode = x.EmployeeCode,
                    EmployeeName = x.EmployeeName,

                    EntityId = x.EntityId,
                    Delete = true
                }).ToList()
            );

            // for the records in reportData, we need to now fill relevant Activity Data and Entity Data

            // fill data for unique entities in a list;
            List<Entity> entities = new List<Entity>();
            distantActivityData.Select(x => x.EntityId).Distinct()
                .Select(x =>
                {
                    entities.Add(DBLayer.GetSingleEntity(x));
                    return 1;
                }).ToList();

            ActivityMapData activityData = null;
            foreach (var item in distantActivityData)
            {
                // set each item a candidate for delete
                // then set flag to false only for selective items.
                item.Delete = true;

                activityData = DBLayer.GetActivityData(item.ActivityId).FirstOrDefault();
                if (activityData != null)
                {
                    item.ActivityDate = activityData.At;
                    item.ActivityType = activityData.ActivityType;
                    item.ActivityAtLocation = activityData.AtLocation;
                    item.ActivityLatitude = activityData.Latitude;
                    item.ActivityLongitude = activityData.Longitude;

                    // apply activity type filter
                    if (searchCriteria.ApplyActivityTypeFilter &&
                            !searchCriteria.ActivityType.Equals(item.ActivityType, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    // calculate distance
                    // retrieve entity record
                    var entityData = entities.Where(x => x.Id == item.EntityId).FirstOrDefault();
                    if (entityData != null)
                    {
                        item.RadiusValue = Business.GetLinearDistance(activityData.Latitude, activityData.Longitude,
                                                    entityData.Latitude, entityData.Longitude);

                        if (item.RadiusValue >= searchCriteria.Distance)
                        {
                            item.Delete = false;
                        }
                    }
                }
            }

            return new Tuple<IEnumerable<DistantActivityData>, IEnumerable<Entity>>(distantActivityData, entities);
        }

        public static async Task<long> LogAlert(string errorCode, string errorDesc)
        {
            string configServerUrl = Utils.GetConfigValue("UrlResolveSite", "http://urlresolve.coolsofttech.com");
            string apiName = Utils.SiteAlertLogApi;

            if (string.IsNullOrEmpty(apiName))
            {
                Business.LogError($"{nameof(LogAlert)}", "API not defined");
                return -1;
            }

            SiteAlert sa = new SiteAlert()
            {
                SiteId = Utils.SiteConfigData.Id,
                SiteName = Utils.SiteConfigData.SiteName,
                ErrorCode = errorCode,
                ErrorDesc = errorDesc,
                UTCAT = DateTime.UtcNow,
            };

            var serializedData = JsonConvert.SerializeObject(sa);
            HttpResponseMessage response = null;

            try
            {
                HttpClient httpClient = new HttpClient();
                response = await httpClient.PostAsync($"{configServerUrl}/{apiName}", new StringContent(serializedData, Encoding.UTF8, "application/json"));

                if ((response?.IsSuccessStatusCode ?? false) == false)
                {
                    Business.LogError($"{nameof(LogAlert)}",
                        $"An error occured while creating alert data: {response.ReasonPhrase}");
                    return -1;
                }

                long alertId = JsonConvert.DeserializeObject<long>(response.Content.ReadAsStringAsync().Result);

                Business.LogError($"{nameof(LogAlert)}", $"Alert Created with Id {alertId}");

                return alertId;
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(LogAlert)}", ex);
                return -1;
            }
        }

        public static ICollection<DomainEntities.Transporter> GetTransporterData()
        {
            return DBLayer.GetTransporterData();
        }

        public static DBSaveStatus SaveIssueReturnData(IssueReturn modelData)
        {
            return DBLayer.SaveIssueReturnData(modelData);
        }

        public static STRWeight GetSTRWeight(string strNumber)
        {
            DomainEntities.STRFilter searchCriteria = new DomainEntities.STRFilter()
            {
                STRNumber = strNumber,
                IsExactSTRNumberMatch = true,
                ApplySTRNumberFilter = true
            };

            ICollection<STRWeight> strWeightRecs = Business.GetSTRWeight(searchCriteria);
            int recCount = (strWeightRecs?.Count ?? 0);

            if (recCount == 0)
            {
                return null;
            }

            return strWeightRecs.First();
        }

        public static STRTag GetSTRTag(string strNumber)
        {
            DomainEntities.STRFilter searchCriteria = new DomainEntities.STRFilter()
            {
                STRNumber = strNumber,
                IsExactSTRNumberMatch = true,
                ApplySTRNumberFilter = true
            };

            ICollection<STRTag> strTagRecs = Business.GetSTRTag(searchCriteria);

            if ((strTagRecs?.Count ?? 0) == 0)
            {
                return null;
            }

            return strTagRecs.First();
        }

        public static bool IsExistingDWS(string dwsNumber)
        {
            ICollection<long> dwsIds = DBLayer.GetDWSIds(dwsNumber);
            return dwsIds.Any();
        }

        public static Tuple<ProcessStatus, long, ICollection<DWS>> GetDWS(IEnumerable<long> dwsIds)
        {
            List<DWS> dwsList = new List<DWS>();
            foreach (long dn in dwsIds)
            {
                //ICollection<long> dwsIds = DBLayer.GetDWSIds(dn);
                //if (dwsIds.Count != 1)
                //{
                //    return new Tuple<ProcessStatus, long, ICollection<DWS>>(ProcessStatus.InvalidDWSNumber, dn, null);
                //}

                DWS dws = DBLayer.GetSingleDWS(dn);
                if (dws == null)
                {
                    return new Tuple<ProcessStatus, long, ICollection<DWS>>(ProcessStatus.InvalidDWS, dn, null);
                }

                dwsList.Add(dws);
            }

            return new Tuple<ProcessStatus, long, ICollection<DWS>>(ProcessStatus.Sucess, 0, dwsList);
        }

        public static ICollection<DWSPaymentReference> GetPaymentReferences(PaymentReferenceFilter searchCriteria)
        {
            return DBLayer.GetPaymentReferences(searchCriteria);
        }

        public static DWSPaymentReference GetPaymentReference(string paymentReference)
        {
            PaymentReferenceFilter prf = new PaymentReferenceFilter()
            {
                ApplyPaymentReferenceFilter = true,
                IsExactReferenceMatch = true,
                PaymentReference = paymentReference
            };

            var itemsList = DBLayer.GetPaymentReferences(prf);
            if ((itemsList?.Count ?? 0) > 0)
            {
                return itemsList.First();
            }

            return null;
        }

        public static IEnumerable<DistanceCalcErrorLog> GetDistanceCalcErrorLog(int startItem, int itemCount)
        {
            return DBLayer.GetDistanceCalcErrorLog(startItem, itemCount);
        }

        public static IEnumerable<EntityAgreement> GetEntityAgreements(int startItem, int itemCount)
        {
            return DBLayer.GetEntityAgreements(startItem, itemCount);
        }

        public static DataTable ExcuteRawSelectQuery(string queryString)
        {
            return DBLayer.ExcuteRawSelectQuery(queryString);
        }

        public static int ExcuteRawScalarQuery(string queryString)
        {
            return DBLayer.ExcuteRawScalarQuery(queryString);
        }

        /// <summary>
        /// A staff code may be assigned to an Area
        /// Though associated zone (which is higher up) is visible, but is actually not selectable
        /// It is only that Area and its related Territory + HQ are selectable.
        ///
        /// This method, does this marking.
        /// </summary>
        /// <param name="staffCode"></param>
        /// <returns></returns>
        public static IEnumerable<OfficeHierarchy> GetSelectableAssociations(string staffCode)
        {
            // get defined staff associations
            IEnumerable<SalesPersonsAssociationData> definedAssociations = Business.GetStaffAssociations(staffCode);

            ICollection<OfficeHierarchy> oh = GetAssociations(staffCode);

            foreach (var asso in definedAssociations)
            {
                if (asso.CodeType.Equals("Zone", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var v in oh.Where(x => x.ZoneCode.Equals(asso.Code, StringComparison.OrdinalIgnoreCase)))
                    {
                        v.IsZoneSelectable = v.IsAreaSelectable = v.IsTerritorySelectable = v.IsHQSelectable = true;
                    }
                    continue;
                }

                if (asso.CodeType.Equals("AreaOffice", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var v in oh.Where(x => x.AreaCode.Equals(asso.Code, StringComparison.OrdinalIgnoreCase)))
                    {
                        v.IsAreaSelectable = v.IsTerritorySelectable = v.IsHQSelectable = true;
                    }
                    continue;
                }

                if (asso.CodeType.Equals("Territory", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var v in oh.Where(x => x.TerritoryCode.Equals(asso.Code, StringComparison.OrdinalIgnoreCase)))
                    {
                        v.IsTerritorySelectable = v.IsHQSelectable = true;
                    }
                    continue;
                }

                if (asso.CodeType.Equals("HeadQuarter", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var v in oh.Where(x => x.HQCode.Equals(asso.Code, StringComparison.OrdinalIgnoreCase)))
                    {
                        v.IsHQSelectable = true;
                    }
                    continue;
                }
            }

            return oh;
        }

        public static ICollection<Vendor> GetVendors()
        {
            return DBLayer.GetVendors();
        }

        // Employee performance for Geolife
        public static ICollection<EmployeeAchieved> GetEmployeeAchieveds(string staffCode)
        {
            return DBLayer.GetEmployeeAchieveds(staffCode);
        }

        public static ICollection<EmployeeMonthlyTarget> GetEmployeeMonthlyTargets(string staffCode)
        {
            return DBLayer.GetEmployeeMonthlyTargets(staffCode);
        }

        public static ICollection<EmployeeYearlyTarget> GetEmployeeYearlyTargets(string staffCode)
        {
            return DBLayer.GetEmployeeYearlyTargets(staffCode);
        }

        public static ICollection<DownloadQuestionnaire> GetQuestionnaire()
        {
            return DBLayer.GetQuestionnaire();
        }

        public static ICollection<DownloadProjects> GetProjectsAssignmentsData(string staffCode)
        {
            return DBLayer.GetProjectsAssignments(staffCode);
        }

        public static IEnumerable<DownloadTasks> GetTasksAssignmentsData(string staffCode)
        {
            ICollection<DownloadTasks> taskAssignment = DBLayer.GetTasksAssignments(staffCode);
            var selfAssigned = taskAssignment.Where(x => x.IsSelfAssigned == true && x.TaskStatus != "Completed");
            var managerAssigned = taskAssignment.Where(x => x.IsSelfAssigned == false && (x.TaskStatus == "Open" || x.TaskStatus == "On-Hold"));

            return selfAssigned.Concat(managerAssigned).ToList();
            //return DBLayer.GetTasksAssignments(staffCode);
        }

        public static IEnumerable<StockInputTag> GetStockInputTags(StockFilter searchCriteria)
        {
            ICollection<StockInputTag> dbResultSet = DBLayer.GetStockInputTags(searchCriteria);

            return FilterStockItems<StockInputTag>(dbResultSet, searchCriteria.IsSuperAdmin, searchCriteria.CurrentUserStaffCode);
        }

        public static string GetNextGRNNumber()
        {
            try
            {
                ICollection<string> grnNumbers = DBLayer.GetGRNNumber(1);
                return grnNumbers.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(GetNextGRNNumber), ex);
                return null;
            }
        }

        public static DBSaveStatus SaveStockInputTagData(StockInputTag tagRecord)
        {
            DBSaveStatus status;
            if (tagRecord.Id > 0)
            {
                status = DBLayer.SaveStockInputTagData(tagRecord);
            }
            else
            {
                DBLayer.CreateStockInputTag(tagRecord);
                status = DBSaveStatus.Success;
            }

            return status;
        }

        public static ICollection<StockInput> GetStockInput(long stockInputTagId)
        {
            return DBLayer.GetStockInput(new List<long>() { stockInputTagId });
        }

        public static ICollection<StockInput> GetStockInput(IEnumerable<StockInputTag> tags)
        {
            return DBLayer.GetStockInput(tags.Select(x => x.Id).ToList());
        }

        public static DBSaveStatus SaveStockInputItemData(StockInput itemRecord)
        {
            DBSaveStatus status;
            if (itemRecord.Id > 0)
            {
                if (itemRecord.DeleteMe == false)
                {
                    status = DBLayer.SaveStockInputItem(itemRecord);
                }
                else
                {
                    status = DBLayer.DeleteStockInputItem(itemRecord);
                }
            }
            else
            {
                status = DBLayer.CreateStockInputItem(itemRecord);
            }

            return status;
        }

        public static IEnumerable<StockRequestTag> GetStockRequestTags(StockRequestFilter searchCriteria)
        {
            ICollection<StockRequestTag> dbResultSet = DBLayer.GetStockRequestTags(searchCriteria);

            return FilterStockItems<StockRequestTag>(dbResultSet, searchCriteria.IsSuperAdmin, searchCriteria.CurrentUserStaffCode);
        }

        public static IEnumerable<StockRequestFull> GetStockRequestItems(StockRequestFilter searchCriteria)
        {
            // get stock request items, that have been confirmed as RequestApproved
            ICollection<StockRequestFull> dbResultSet = DBLayer.GetStockRequestItems(searchCriteria);

            return FilterStockItems<StockRequestFull>(dbResultSet, searchCriteria.IsSuperAdmin, searchCriteria.CurrentUserStaffCode);
        }

        public static IEnumerable<StockLedger> GetStockLedger(StockLedgerFilter searchCriteria)
        {
            ICollection<StockLedger> dbResultSet = DBLayer.GetStockLedger(searchCriteria);

            return FilterStockItems<StockLedger>(dbResultSet, searchCriteria.IsSuperAdmin, searchCriteria.CurrentUserStaffCode);
        }

        public static IEnumerable<StockBalance> GetStockBalance(StockLedgerFilter searchCriteria)
        {
            ICollection<StockBalance> dbResultSet = DBLayer.GetStockBalance(searchCriteria);

            return FilterStockItems<StockBalance>(dbResultSet, searchCriteria.IsSuperAdmin, searchCriteria.CurrentUserStaffCode);
        }

        // called from download -
        public static IEnumerable<StockBalance> GetStockBalance(string staffCode)
        {
            StockLedgerFilter slf = new StockLedgerFilter()
            {
                ApplyEmployeeCodeFilter = true,
                EmployeeCode = staffCode
            };
            ICollection<StockBalance> dbResultSet = DBLayer.GetStockBalance(slf);
            return dbResultSet;
        }

        public static string GetNextRequestNumber()
        {
            try
            {
                ICollection<string> reqNumbers = DBLayer.GetRequestNumber(1);
                return reqNumbers.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(GetNextRequestNumber), ex);
                return null;
            }
        }

        public static DBSaveStatus SaveStockRequestTagData(StockRequestTag tagRecord)
        {
            DBSaveStatus status;
            if (tagRecord.Id > 0)
            {
                status = DBLayer.SaveStockRequestTagData(tagRecord);
            }
            else
            {
                DBLayer.CreateStockRequestTag(tagRecord);
                status = DBSaveStatus.Success;
            }

            return status;
        }

        public static ICollection<StockRequest> GetStockRequest(long stockRequestTagId)
        {
            return DBLayer.GetStockRequest(stockRequestTagId);
        }

        public static DBSaveStatus SaveStockRequestItemData(StockRequest itemRecord)
        {
            DBSaveStatus status;
            if (itemRecord.Id > 0)
            {
                if (itemRecord.DeleteMe == false)
                {
                    status = DBLayer.SaveStockRequestItem(itemRecord);
                }
                else
                {
                    status = DBLayer.DeleteStockRequestItem(itemRecord);
                }
            }
            else
            {
                status = DBLayer.CreateStockRequestItem(itemRecord);
            }

            return status;
        }

        public static DBSaveStatus ReviewStockInputTagData(StockInputTag inputRec)
        {
            DBSaveStatus status = DBLayer.ReviewStockInputTagData(inputRec);

            if (status == DBSaveStatus.Success)
            {
                DBLayer.PostStockLedgerFromInput(inputRec.Id, inputRec.CurrentUser);
            }

            return status;
        }

        public static DBSaveStatus ReviewStockRequestTagData(StockRequestTag inputRec)
        {
            return DBLayer.ReviewStockRequestTagData(inputRec);
        }

        public static SalesPersonMiniModel GetSalesPersonData(string staffCode)
        {
            return DBLayer.GetSalesPersonData(staffCode);
        }

        public static DBSaveStatus PerformFulfillment(StockFulfillmentData fulfillmentData)
        {
            if (fulfillmentData.IsConfirmClicked)
            {
                return DBLayer.PerformFulfillment(fulfillmentData);
            }
            else
            {
                return DBLayer.PerformStockRequestDenied(fulfillmentData);
            }
        }

        public static DBSaveStatus PerformStockClear(StockFulfillmentData fulfillmentData)
        {
            if (fulfillmentData.IsConfirmClicked)
            {
                return DBLayer.PerformStockClear(fulfillmentData);
            }
            else
            {
                return DBLayer.PerformStockRequestDenied(fulfillmentData);
            }
        }

        public static DBSaveStatus PerformStockAdd(StockFulfillmentData fulfillmentData)
        {
            if (fulfillmentData.IsConfirmClicked)
            {
                return DBLayer.PerformStockAdd(fulfillmentData);
            }
            else
            {
                return DBLayer.PerformStockRequestDenied(fulfillmentData);
            }
        }

        public static DBSaveStatus SaveWorkFlowDetail(EntityWorkFlowDetail rec)
        {
            DBSaveStatus status = DBLayer.SaveWorkFlowDetail(rec);
            return status;
        }

        public static DBSaveStatus AddWorkFlowDetail(EntityWorkFlowDetail rec)
        {
            DBSaveStatus status = DBLayer.AddWorkFlowDetail(rec);
            return status;
        }

        public static ExecAppRollupEnum ConvertToRollUpEnum(int n)
        {
            ExecAppRollupEnum rollUp = ExecAppRollupEnum.HQ;
            switch (n)
            {
                case 1:
                    rollUp = ExecAppRollupEnum.Zone;
                    break;
                case 2:
                    rollUp = ExecAppRollupEnum.Area;
                    break;
                case 3:
                    rollUp = ExecAppRollupEnum.Territory;
                    break;
                case 4:
                    rollUp = ExecAppRollupEnum.HQ;
                    break;
                default:
                    rollUp = ExecAppRollupEnum.HQ;
                    break;
            }

            return rollUp;
        }

        private static IEnumerable<T> FilterStockItems<T>(ICollection<T> inputItems, bool isSuperAdmin, string currentUserStaffCode) where T : Stock
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = (isSuperAdmin) ? Business.GetAssociations() :
                                        Business.GetSelectableAssociations(currentUserStaffCode);

            ICollection<string> visibleStaffCodes = GetVisibleStaffCodes(isSuperAdmin, currentUserStaffCode);

            foreach (T sit in inputItems)
            {
                var isZone = !String.IsNullOrEmpty(sit.ZoneCode);
                var isArea = !String.IsNullOrEmpty(sit.AreaCode);
                var isTerritory = !String.IsNullOrEmpty(sit.TerritoryCode);
                var isHQ = !String.IsNullOrEmpty(sit.HQCode);
                var isStaff = !String.IsNullOrEmpty(sit.StaffCode);

                if (isStaff)
                {
                    // validate that current user has access to the staff and if so, fill staff name
                    if (visibleStaffCodes.Any(x => x.Equals(sit.StaffCode, StringComparison.OrdinalIgnoreCase)))
                    {
                        sit.EmployeeName = GetSalesPersonData(sit.StaffCode)?.Name ?? "";
                        yield return sit;
                    }

                    continue;
                }

                if (isHQ)
                {
                    if (officeHierarchy.Any(x => x.HQCode.Equals(sit.HQCode, StringComparison.OrdinalIgnoreCase)
                                            && x.IsHQSelectable))
                    {
                        yield return sit;
                    }
                    continue;
                }

                if (isTerritory)
                {
                    if (officeHierarchy.Any(x => x.TerritoryCode.Equals(sit.TerritoryCode, StringComparison.OrdinalIgnoreCase)
                                            && x.IsTerritorySelectable))
                    {
                        yield return sit;
                    }
                    continue;
                }

                if (isArea)
                {
                    if (officeHierarchy.Any(x => x.AreaCode.Equals(sit.AreaCode, StringComparison.OrdinalIgnoreCase)
                                            && x.IsAreaSelectable))
                    {
                        yield return sit;
                    }
                    continue;
                }

                if (isZone)
                {
                    if (officeHierarchy.Any(x => x.ZoneCode.Equals(sit.ZoneCode, StringComparison.OrdinalIgnoreCase)
                                            && x.IsZoneSelectable))
                    {
                        yield return sit;
                    }
                    continue;
                }
            }
        }

        //SA - 31-05-2021
        public static IEnumerable<string> GetSeasonNames()
        {
            ICollection<WorkflowSeason> seasons = DBLayer.GetWorkflowSeasonsList();

            return seasons.Select(x => x.SeasonName).Distinct().ToList();
        }

        public static ICollection<VendorSTR> GetSearchSTRForApproval(VendorSTRFilter searchCriteria)
        {
            return DBLayer.GetSearchSTRForApproval(searchCriteria);
        }

        public static void CreateSTRPayment(VendorSTRPayment strPayRec)
        {
            DBLayer.CreateSTRPayment(strPayRec);
        }

        public static void MarkSTRAsApprovedAddBankDetails(IEnumerable<VendorSTRPayment> payRecData)
        {
            DBLayer.MarkSTRAsApprovedAddBankDetails(payRecData);
        }

        public static ICollection<VendorSTR> GetSearchSTRForPayment(VendorSTRFilter searchCriteria)
        {
            IEnumerable<VendorSTR> strDetails = DBLayer.GetSTRPaymentData();

            if (searchCriteria.ApplyPaymentReferenceFilter)
            {
                strDetails = strDetails.Where(x => x.PaymentReference == searchCriteria.PaymentReference);
            }

            if (searchCriteria.ApplyVendorNameFilter)
            {
                strDetails = strDetails.Where(x => x.VendorName.ToLower().Contains(searchCriteria.VendorName.ToLower()));
            }
            if (searchCriteria.ApplyVehicleNumberFilter)
            {
                strDetails = strDetails.Where(x => x.VehicleNumber.Contains(searchCriteria.VehicleNumber.ToUpper()));
            }

            if (searchCriteria.ApplySTRNumberFilter)
            {
                strDetails = strDetails.Where(x => x.STRNumber.ToUpper().Contains(searchCriteria.STRNumber.ToUpper()));
            }

            if (searchCriteria.ApplySeasonNameFilter)
            {
                if (searchCriteria.SeasonName != "All")
                {
                    strDetails = strDetails.Where(x => x.SeasonName.Equals(searchCriteria.SeasonName));
                }
            }

            if (searchCriteria.ApplySTRPaymentStatusFilter)
            {
                strDetails = strDetails.Where(x => x.PaymentStatus.Equals(searchCriteria.STRPaymentStatus));
            }

            if (searchCriteria.ApplyDateFilter)
            {
                strDetails = strDetails.Where(x => x.STRDate >= searchCriteria.DateFrom &&
                                                                x.STRDate <= searchCriteria.DateTo);
            }

            return strDetails.OrderBy(x => x.STRNumber).ToList();
        }

        public static Tuple<ProcessStatusSTR, long, ICollection<VendorSTR>> GetSTRForPayment(IEnumerable<long> strTagIds)
        {
            List<VendorSTR> strList = new List<VendorSTR>();
            ICollection<VendorSTR> strPayData = DBLayer.GetSTRPaymentData();
            foreach (long id in strTagIds)
            {
                VendorSTR strData = strPayData.Where(x => x.STRTagId == id).FirstOrDefault();
                if (strData == null)
                {
                    return new Tuple<ProcessStatusSTR, long, ICollection<VendorSTR>>(ProcessStatusSTR.InvalidSTRNumber, id, null);
                }
                strList.Add(strData);
            }
            return new Tuple<ProcessStatusSTR, long, ICollection<VendorSTR>>(ProcessStatusSTR.Success, 0, strList);
        }

        public static VendorSTR GetSingleSTRForPayment(long strTagId)
        {
            return DBLayer.GetSingleSTRForPayment(strTagId);
        }

        public static STRPaymentReference GetSTRPaymentReference(string paymentReference)
        {
            PaymentReferenceFilter prf = new PaymentReferenceFilter()
            {
                ApplyPaymentReferenceFilter = true,
                IsExactReferenceMatch = true,
                PaymentReference = paymentReference
            };

            var itemsList = DBLayer.GetSTRPaymentReferences(prf);
            if ((itemsList?.Count ?? 0) > 0)
            {
                return itemsList.First();
            }

            return null;
        }

        public static ICollection<VendorSTR> GetSTRPaymentDataForApproval(IEnumerable<long> strTagIds)
        {
            return DBLayer.GetSTRPaymentDataForApproval(strTagIds);
        }

        public static void CreateSTRPaymentReference(STRPaymentReference spr)
        {
            DBLayer.CreateSTRPaymentReference(spr);
        }

        public static ICollection<STRPaymentReference> GetSTRPaymentReferences(PaymentReferenceFilter searchCriteria)
        {
            return DBLayer.GetSTRPaymentReferences(searchCriteria);
        }

        public static ICollection<VendorSTR> GetSingleSTRDetails(long strTagId)
        {
            return DBLayer.GetSingleSTRDetails(strTagId);
        }

        public static void MarkSTRAsPaid(IEnumerable<VendorSTR> strRecs)
        {
            DBLayer.MarkSTRAsPaid(strRecs);
        }

        /// <summary>
        /// Author: Rajesh V; Date24-06-2021 ; Purpose: dealer Questionnaire
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="batchId"></param>
        /// <param name="domainQuestionnaire"></param>
        private static void SaveSqliteQuestionnaireData(long employeeId, long batchId,
                               IEnumerable<SqliteDomainQuestionnaire> domainQuestionnaire)
        {
            if (domainQuestionnaire == null || domainQuestionnaire.Count() == 0)
            {
                return;
            }

            DBLayer.SaveSqliteQuestionnaireDataWrapper(employeeId, batchId, domainQuestionnaire);
        }
        /// <summary>
        /// Author: Ajith;  Purpose: Dealer Questionnaire ; Dated:12/06/2021
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<QuestionPaper> GetQuestionpaper()
        {
            return DBLayer.GetQuestionpaper();
        }
        /// <summary>
        /// Author: Ajith;  Purpose: Dealer Questionnaire ; Dated:12/06/2021
        /// </summary>
        /// <returns></returns>
        public static ICollection<CustomerQuestionnaire> GetCustomerQuestionnaire(SearchCriteria searchCriteria)
        {
            return DBLayer.GetCustomerQuestionnaire(searchCriteria);
        }

        /// <summary>
        /// Author: Ajith;  Purpose: Dealer Questionnaire ; Dated:12/06/2021
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CustomerQuestionnairedetails> GetCustomerQuestionnairedetails(long QuestionnaireID)
        {
            return DBLayer.GetCustomerQuestionnairedetails(QuestionnaireID);
        }

        /// <summary>
        /// Author: Ajith;  Purpose: Dealer Questionnaire ; Dated:12/06/2021
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CustomerQuestionnairedetails> GetCustomerQuestionnaireQA(long QuestionnaireID)
        {
            return DBLayer.GetCustomerQuestionnaireQA(QuestionnaireID);
        }

        //Author:Ajith, Purpose:For Customerquestionnaire excel download data ; Dated:29/06/2021
        public static IEnumerable<CustomerQuestionnairedetails> GetCustomerQuestionnaireAlldetails(SearchCriteria searchCriteria)
        {
            return DBLayer.GetCustomerQuestionnaireAlldetails(searchCriteria);
        }

        /// Author:Ajith, Purpose:For Bonus Calculation Search on 23/07/2021
        public static IEnumerable<string> GetSeasonNamesBonus()
        {
            ICollection<BonusCalculation> seasons = DBLayer.GetSeasonNamesBonus();

            return seasons.Select(x => x.SeasonName).Distinct().ToList();
        }

        /// Author:Ajith/Rajesh, Purpose:For Bonus Calculation Pending status Search on 22/07/2021
        public static IEnumerable<BonusCalculation> GetPendingBonusAgreement(BonusCalculationFilter searchCriteria)
        {
            IEnumerable<BonusCalculation> BonusAgreementdetails = DBLayer.GetPendingBonusAgreement(searchCriteria);
            var bonusAgreementList = new List<BonusCalculation>();
            foreach (var item in BonusAgreementdetails)
            {
                var yieldPerAcre = Math.Ceiling(item.NetPayableWt / item.LandSizeInAcres);
                bool eligible = DBLayer.BonusAgreementCheck(item.SeasonName, item.TypeName, yieldPerAcre);
                if (eligible)
                {
                    bonusAgreementList.Add(item);
                }
            }
            return bonusAgreementList;
        }

        /// Author:Rajesh V, Purpose:For Bonus Calculation Awaiting status Search, on 02/08/2021
        public static IEnumerable<BonusCalculation> GetBonusAgreement(BonusCalculationFilter searchCriteria)
        {
            return DBLayer.GetBonusAgreement(searchCriteria);
        }

        /// Author:Rajesh V, Purpose:Mark selected Bonus Agreement status as Approved, on 02/08/2021
        public static void MarkBonusAgreementAsApproved(IEnumerable<long> agreeId, string currentUser)
        {
            DBLayer.MarkBonusAgreementAsApproved(agreeId, currentUser);
        }

        /// Author:Rajesh V, Purpose:Get single agreement Bonus Calculation, on 30/07/2021
        public static IEnumerable<BonusCalculation> GetSingleBonusDetails(long agreeId)
        {
            IEnumerable<BonusCalculation> BonusAgreementdetails = DBLayer.GetSingleBonusDetails(agreeId);

            var bonusAgreementList = new List<BonusCalculation>();
            foreach (var item in BonusAgreementdetails)
            {
                bonusAgreementList.Add(item);
            }
            var yieldPerAcre = Math.Ceiling(bonusAgreementList.FirstOrDefault().NetPayableWt / bonusAgreementList.FirstOrDefault().LandSizeInAcres);
            var SeasonName = bonusAgreementList.FirstOrDefault().SeasonName;
            var TypeName = bonusAgreementList.FirstOrDefault().TypeName;

            var bonusRate = DBLayer.GetBonusRate(SeasonName, TypeName, yieldPerAcre);
            bonusAgreementList.FirstOrDefault().BonusRate = bonusRate;

            return bonusAgreementList;
        }

        public static void CreateBonusDetails(BonusCalculation bonusRecord)
        {
            DBLayer.CreateBonusDetails(bonusRecord);
        }

        /// Author:Rajesh V, Purpose:Get agreements for Bonus payment, on 03/08/2021
        public static IEnumerable<BonusCalculation> GetBonusAgreementPayment(IEnumerable<long> agreeIds)
        {
            List<BonusCalculation> bonusRecords = new List<BonusCalculation>();
            IEnumerable<BonusCalculation> bonusPayData = DBLayer.GetBonusAgreementPayment();
            foreach (long id in agreeIds)
            {
                BonusCalculation bonusData = bonusPayData.Where(x => x.AgreementId == id).FirstOrDefault();
                if (bonusData != null)
                {
                    bonusRecords.Add(bonusData);
                }
            }
            return bonusRecords;
        }

        /// Author:Rajesh V, Purpose:Get bonus Payment Reference details, on 04/08/2021
        public static BonusPaymentReferences GetBonusPaymentReference(string paymentReference)
        {
            PaymentReferenceFilter prf = new PaymentReferenceFilter()
            {
                ApplyPaymentReferenceFilter = true,
                IsExactReferenceMatch = true,
                PaymentReference = paymentReference
            };

            var itemsList = DBLayer.GetBonusPaymentReference(prf);
            if ((itemsList?.Count ?? 0) > 0)
            {
                return itemsList.First();
            }

            return null;
        }

        /// Author:Rajesh V, Purpose:Save bonus Payment  details, on 04/08/2021
        public static void CreateBonusPaymentReference(BonusPaymentReferences bpr)
        {
            DBLayer.CreateBonusPaymentReference(bpr);
        }

        /// Author:Rajesh V, Purpose:Mark bonus agreement as paid  details, on 04/08/2021
        public static void MarkBonusAsPaid(IEnumerable<BonusCalculation> bonusRecord)
        {
            DBLayer.MarkBonusAsPaid(bonusRecord);
        }

        /// Author:Rajesh V, Purpose:Get bonus Payment Reference details based on search, on 04/08/2021
        public static ICollection<BonusPaymentReferences> GetBonusPaymentReference(PaymentReferenceFilter searchCriteria)
        {
            return DBLayer.GetBonusPaymentReference(searchCriteria);
        }

        /// Author:Rajesh V, Purpose:Get bonus download data based on search, on 05/08/2021
        public static ICollection<BonusDownloadData> GetBonusDownloadData(string paymentRef)
        {
            return DBLayer.GetBonusDownloadData(paymentRef);
        }

        /// Author:Rajesh V, Purpose:Get Farmer Summary data based on search, on 07/10/2021
        public static IEnumerable<FarmerSummaryReport> GetFarmerSummary(SearchCriteria searchCriteria)
        {
            return DBLayer.GetFarmerSummary(searchCriteria);
        }
        //Author:Venkatesh, Purpose: Get Dealers Not Met Report on: 2022/11/09
        public static IEnumerable<DealersNotMetReport> GetDealerNotMet(SearchCriteria searchCriteria)
        {
            return DBLayer.GetDealersNotMet(searchCriteria);
        }

        //Author:Gagana, Purpose: Get Geo Tagging Report on: 2022/12/29

        public static IEnumerable<GeoTaggingReport> GetGeoTagging(SearchCriteria searchCriteria)
        {
            return DBLayer.GetGeoTagging(searchCriteria);
        }

        public static IEnumerable<FarmerSummaryReportData> GetFarmerSummarData(long AgreementId)
        {
            return DBLayer.GetFarmerSummaryData(AgreementId);
        }

        public static IEnumerable<FarmerSummaryReportData> GetFarmerIssueData(long AgreementId)
        {
            return DBLayer.GetFarmerIssueData(AgreementId);
        }

        public static IEnumerable<FarmerSummaryReportData> GetFarmerDwsData(long AgreementId)
        {
            return DBLayer.GetFarmerDwsData(AgreementId);
        }

        /// Author:Gagana, Purpose:Get Farmer Summary data based on search, on 18/11/2021

        public static IEnumerable<FarmerSummaryReportData> GetFarmerAdvReqData(long AgreementId)
        {
            return DBLayer.GetFarmerAdvReqData(AgreementId);
        }

        public static IEnumerable<FarmerSummaryReportData> GetFarmerAgrBonusData(long AgreementId)
        {
            return DBLayer.GetFarmerAgrBonusData(AgreementId);
        }

        public static IEnumerable<Projects> GetProjects(ProjectsFilter searchCriteria)
        {
            searchCriteria.TenantId = Utils.SiteConfigData.TenantId;
            return DBLayer.GetProjects(searchCriteria);
        }

        public static DomainEntities.Projects GetSingleProject(long projectId)
        {
            return DBLayer.GetSingleProject(projectId);
        }

        public static ICollection<DomainEntities.ProjectAssignments> GetProjectAssignment(long projectId)
        {
            return DBLayer.GetProjectAssignment(projectId);
        }

        public static DomainEntities.ProjectAssignments GetSingleProjectAssignment(long projectId, long assignmentId)
        {
            ICollection<ProjectAssignments> items = Business.GetProjectAssignment(projectId);
            return items.FirstOrDefault(x => x.Id == assignmentId);
        }

        public static IEnumerable<DomainEntities.ProjectAssignments> GetSingleProjectEmployeeAssignment(long projectId, long employeeId)
        {
            ICollection<ProjectAssignments> items = Business.GetProjectAssignment(projectId);
            return items.Where(x => x.EmployeeId == employeeId).ToList();
        }

        public static DBSaveStatus SaveProjectData(Projects projectRec)
        {
            DBSaveStatus status;
            if (projectRec.Id > 0)
            {
                status = DBLayer.SaveProjectData(projectRec);
            }
            else
            {
                DBLayer.CreateProjectData(projectRec);
                status = DBSaveStatus.Success;
            }

            return status;
        }

        public static DBSaveStatus SaveProjectAssignmentData(ProjectAssignments paRec)
        {
            DBSaveStatus status;
            if (paRec.Id > 0)
            {
                status = DBLayer.SaveProjectAssignmentData(paRec);
            }
            else
            {
                DBLayer.CreateProjectAssignmentData(paRec);
                status = DBSaveStatus.Success;
            }

            return status;
        }

        public static IEnumerable<Projects> GetProjectNames()
        {
            return DBLayer.GetProjectNames();
        }

        public static ICollection<FollowUpTask> GetFollowUpTasks(FollowUpTaskFilter searchCriteria)
        {
            return DBLayer.GetFollowUpTasks(searchCriteria);
        }

        public static DomainEntities.FollowUpTask GetSingleTask(long taskId)
        {
            return DBLayer.GetSingleTask(taskId);
        }

        public static DBSaveStatus SaveFollowUpTaskData(FollowUpTask taskRec)
        {
            DBSaveStatus status;
            if (taskRec.Id > 0)
            {
                status = DBLayer.SaveFollowUpTaskData(taskRec);
            }
            else
            {
                DBLayer.CreateFollowUpTaskData(taskRec);
                status = DBSaveStatus.Success;
            }

            return status;
        }

        public static ICollection<DomainEntities.TaskAssignments> GetTaskAssignment(long taskId)
        {
            return DBLayer.GetTaskAssignment(taskId);
        }

        public static DBSaveStatus SaveTaskAssignmentData(TaskAssignments taskRec)
        {
            DBSaveStatus status;
            if (taskRec.Id > 0)
            {
                status = DBLayer.SaveTaskAssignmentData(taskRec);
            }
            else
            {
                DBLayer.CreateTaskAssignmentData(taskRec);
                status = DBSaveStatus.Success;
            }

            return status;
        }

        public static DomainEntities.TaskAssignments GetSingleTaskAssignment(long taskId, long assignmentId)
        {
            ICollection<TaskAssignments> items = Business.GetTaskAssignment(taskId);
            return items.FirstOrDefault(x => x.Id == assignmentId);
        }

        public static IEnumerable<DomainEntities.TaskAssignments> GetSingleTaskEmployeeAssignment(long taskId, long employeeId)
        {
            ICollection<TaskAssignments> items = Business.GetTaskAssignment(taskId);
            return items.Where(x => x.EmployeeId == employeeId).ToList();
        }

        public static ICollection<DomainEntities.FollowUpTaskAction> GetTaskActions(long taskId)
        {
            return DBLayer.GetTaskActions(taskId);
        }

        public static ICollection<DomainEntities.FollowUpTaskAction> GetTaskAssignmentActions(long taskId, long taskAssignmentId)
        {
            ICollection<DomainEntities.FollowUpTaskAction> rec = DBLayer.GetTaskActions(taskId);
            return rec.Where(x => x.XRefTaskAssignmentId == taskAssignmentId).ToList();
        }

        public static ICollection<Entity> GetEntitiesForTask(string clientType, string hqCode)
        {
            ICollection<DomainEntities.Entity> rec = DBLayer.GetEntitiesForTask(clientType);
            return rec.Where(x => x.HQCode.Equals(hqCode)).OrderBy(y => y.EntityName).ToList();
        }

        public static Entity GetEntityDetailsForTaskAssignment(string clientType, string clientCode)
        {
            ICollection<DomainEntities.Entity> rec = DBLayer.GetEntitiesForTask(clientType);
            return rec.Where(x => x.Id.ToString().Equals(clientCode)).FirstOrDefault();
        }

        //Code to get Visible Employees for and Entity
        public static ICollection<string> GetVisibleStaffCodesForEntity(bool isSuperAdmin, string HQCode, ICollection<string> staffCodes)
        {
            //ICollection<string> uniqueStaffCodes = Business.GetSalesPersons().Select(x => x.StaffCode).ToList();

            // June 15 2020
            //if (isSuperAdmin)
            //{
            //    return staffCodes;
            //}
            List<string> inclusionList = new List<string>();

            // First get Detail associations levels of a HQCode
            IEnumerable<OfficeHierarchyForAll> officeHierarchyForAll = DBLayer.GetDetailedAssociationsForAll(Utils.SiteConfigData.TenantId);
            //IEnumerable<OfficeHierarchyForAll> officeHierarchyForHQCode = officeHierarchyForAll.Where(x => x.HQCode == HQCode);

            // Get all staff associations
            ICollection<SalesPersonsAssociationDataForAll> allStaffAssociations = DBLayer.GetStaffAssociationsForAll(Utils.SiteConfigData.TenantId);

            //ICollection<SalesPersonsAssociationDataForAll> visibleStaffAssociation = allStaffAssociations.Where(x => staffCodes.Any(y => y == x.StaffCode))
            //                                                                        .Select(x => GetRankForCodeType(x.CodeType))
            //                                                                        .OrderByDescending(x => x).FirstOrDefault();

            foreach (string sc in staffCodes)
            {
                //IEnumerable<OfficeHierarchy> expenseUserAssociations = DBLayer.GetDetailedAssociations(sc);
                // var scHQCodes = expenseUserAssociations.Select(x => x.HQCode).ToList();

                bool keepStaffCode = false;
                do
                {

                    var scHQCodes = officeHierarchyForAll.Where(x => x.StaffCode == sc)
                        .Where(y => y.HQCode == HQCode)
                        .Select(x => x.HQCode)
                        .ToList();

                    // if HQ Codes don't intersect - don't include the sc
                    if (scHQCodes.Count() == 0)
                    {
                        keepStaffCode = false;
                        break;
                    }

                    keepStaffCode = true;
                } while (false); // this is put to execute the loop once only and to use break statements in between the construct

                // if we are allowed to keep staff code, add it to inclusion list
                if (keepStaffCode)
                {
                    inclusionList.Add(sc);
                }
            } // end of foreach

            return inclusionList;
        }
        //Added by Swetha ,Purpose:GetLeaveData
        public static ICollection<DomainEntities.DashboardLeave> GetDashboardLeave(LeaveFilter searchCriteria)
        {
            return DBLayer.GetDashboardLeaves(searchCriteria);
        }
        public static DashboardLeave GetSingleLeave(long id)
        {
            return DBLayer.GetSingleLeave(id);
        }

        public static DBSaveStatus SaveLeaveData(DashboardLeave inputRec, string currentUser)
        {
            DBSaveStatus status;

            status = DBLayer.SaveLeaveApproveData(inputRec, currentUser);

            return status;

        }

        public static IEnumerable<DealersSummaryReportData> GetDealersSummaryReports(SearchCriteria searchCriteria)
        {
            ICollection<DealersSummaryReportData> customercount = DBLayer.GetHQCodeForStaff(searchCriteria);

            ICollection<DealersGeoTagCount> geoTaggingcount = DBLayer.GetGeoTaggingCount();

            IQueryable<DealersSummaryReportData> dealerSummary = (from c in customercount
                                                                  join g in geoTaggingcount
                                                                  on c.StaffCode equals g.StaffCode
                                                                  into GeoTagging
                                                                  from gt in GeoTagging.DefaultIfEmpty()
                                                                  select new DealersSummaryReportData
                                                                  {
                                                                      StaffCode = c.StaffCode,
                                                                      EmployeeName = c.EmployeeName,
                                                                      HQCode = c.HQCode,
                                                                      EmployeeStatus = c.EmployeeStatus,
                                                                      TotalDealersCount = c.TotalDealersCount,
                                                                      BranchName = c.BranchName,
                                                                      DivisionName = c.DivisionName,
                                                                      GeoTagCompleted = gt != null ? gt.GeoTagCount : 0,
                                                                      GeoTagsPending = (c.TotalDealersCount) - (gt != null ? gt.GeoTagCount : 0)

                                                                  }).AsQueryable();



            return dealerSummary;

        }

        //Author by : Swetha M  on Date:2023/01/15
        //Get all the agreements 
        public static IQueryable<AgreementReportData> GetAgreementsReportData(SearchCriteria searchCriteria)
        {
            return DBLayer.GetAgreementsReportData(searchCriteria);
        }

        //Author by : Swetha M  on Date:2023/01/31
        //Save the bulk approved expenses
        public static int SaveBulkExpenseApprove(List<ApprovalData> approvalData, bool isSuperAdmin, string currentUserStaffCode)
        {

            SalesPersonLevel level = GetHighestLevel(isSuperAdmin, currentUserStaffCode);
            return DBLayer.ApproveBulkExpense(approvalData, level);
        }


        ////Author by : Gagana on Date:2023/01/15
        public static IQueryable<DuplicateFarmersReport> GetDuplicateFarmersReportData(SearchCriteria searchCriteria)
        {
            return DBLayer.GetDuplicateFarmersReportData(searchCriteria);
        }

        //Author:Gagana Purpose:Save details of new Agreement details; Date:08-02-2023

        public static void AddAgreement(EntityAgreement ea, string currentUser)
        {
            DBLayer.AddAgreement(ea, currentUser);
        }
        public static IQueryable<DomainEntities.WorkflowSeason> GetEntityActiveCrops(long entityId)
        {
            return DBLayer.GetEntityActiveCrops(entityId);
        }

        ////Author by : Gagana on Date:2023/03/02
        public static IQueryable<FarmersBankAccountReport> GetFarmersBankAccountReportData(SearchCriteria searchCriteria)
        {
            return DBLayer.GetFarmersBankAccountReportData(searchCriteria);
        }

        //Author:Gowtham S , Purpose:Add Bank Account details ; Date:27-02-2023
        public static void SaveAddBankDetail(EntityBankDetail ea, string currentUser,string fileName)
        {
            DBLayer.AddBankDetails(ea, currentUser,fileName);
        }

        //Author:Raj Kumar M, Purpose:To add new Profile; Date:21-02-2023
        public static void AddEntityData(Entity entityRec, IEnumerable<string> crop, string number)
        {
            DBLayer.AddEntity(entityRec, crop, number);
        }
      
        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        public static ICollection<EntityAgreementForIR> GetEntityAgreementsForIR(long entityId)
        {
            return DBLayer.GetEntityAgreementsForIR(entityId);
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        public static ICollection<Entity> GetAssociatedEntityName(string hqCode)
        {
            return DBLayer.GetAssociatedEntityName(hqCode);
        }
        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        public static ICollection<IssueReturn> GetTypeName(long aggId)
        {
            return DBLayer.GetTypeName(aggId);
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:17-02-2023
        public static void SaveAddIssueReturnsDetails(IssueReturn sp, string currentUser)
        {
           DBLayer.SaveAddIssueReturnsDetails(sp, currentUser);          
        }
    }
}
