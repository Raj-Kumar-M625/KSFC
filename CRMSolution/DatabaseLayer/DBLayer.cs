using CRMUtilities;
using DBModel;
using DomainEntities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace DatabaseLayer
{
    public static class DBLayer
    {
        //private static Lazy<EpicCrmEntities> epicCrmEntities = new Lazy<EpicCrmEntities>();
        //private static EpicCrmEntities GetOrm => epicCrmEntities.Value;

        // we don't necessarily have to dispose GetOrm after using it
        // as connection is automatically closed by EF and object GetOrm will
        // be disposed off by GC
        private static void SetOrmForLog(DBModel.EpicCrmEntities orm)
        {
            orm.Database.Log = (s) =>
            {
                // remove noise
                if (string.IsNullOrWhiteSpace(s) ||
                    s.StartsWith("Opened connection") ||
                    s.StartsWith("Closed connection") ||
                    s.StartsWith("-- Executing at")
                )
                {
                    ;
                }
                else
                {
                    int strLen = s.Length;
                    if (strLen > 4000)
                    {
                        s = s.Substring(0, 4000);
                    }
                    LogError("OrmQuery", s, ">");
                }
            };
        }

        private static EpicCrmEntities GetOrm
        {
            get
            {
                var orm = new EpicCrmEntities(Utils.EFConnectionString);
                orm.Database.CommandTimeout = 200;

                if (Utils.SiteConfigData.LogDatabaseQuery)
                {
                    SetOrmForLog(orm);
                }

                return orm;
            }
        }

        private static EpicCrmEntities GetNoLogOrm => new EpicCrmEntities(Utils.EFConnectionString);

        //added by sumegha on 24/04/2019 - Check for the duplicate unique id
        public static ICollection<long> GetUniqueIdEntities(string uniqueId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Entities
                 .Where(x => x.IsActive && x.UniqueId == uniqueId)
                 .Select(x => x.Id).ToList();
        }

        //added by sumegha on 24/04/2019 - Check for the duplicate agreement number
        public static ICollection<long> GetAgreements(string agreementNumber)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.EntityAgreements
                .Where(x => x.AgreementNumber == agreementNumber)
                .Select(x => x.Id).ToList();
        }
        public static DomainEntities.TenantEmployee GetEmployeeRecord(long userId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.TenantEmployees.Include("Tenant")
                .Where(x => x.Id == userId).FirstOrDefault();

            //var rec = orm.TenantEmployees.Find(userId);

            if (rec == null)
            {
                return null;
            }

            var te = new DomainEntities.TenantEmployee()
            {
                Id = rec.Id,
                ManagerId = rec.ManagerId,
                EmployeeCode = rec.EmployeeCode,
                Name = rec.Name,
                TenantId = rec.TenantId,
                IsActive = rec.IsActive,
                TimeIntervalInMillisecondsForTrcking = rec.TimeIntervalInMillisecondsForTracking,
                IMEI = rec.IMEIs.Where(y => y.IsActive).Select(z => z.IMEINumber).FirstOrDefault(),
                SendLogFromPhone = rec.SendLogFromPhone,
                AutoUploadFromPhone = rec.AutoUploadFromPhone,
                ExecAppAccess = rec.ExecAppAccess,
                // this value is now coming from UrlResolve
                MaxDiscountPercentage = (double)rec.Tenant.MaxDiscountPercentage,
                DiscountType = rec.Tenant.DiscountType,
                ActivityPageName = rec.ActivityPageName,
                EnhancedDebugEnabled = rec.EnhancedDebugEnabled,
                TestFeatureEnabled = rec.TestFeatureEnabled,
                VoiceFeatureEnabled = rec.VoiceFeatureEnabled,
                ExecAppDetailLevel = rec.ExecAppDetailLevel
            };
            return te;
        }

        public static void SaveParseErrors(long id, List<DomainEntities.ExcelUploadError> errorList)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            if (errorList != null)
            {
                foreach (var e in errorList)
                {
                    orm.ExcelUploadErrors.Add(new DBModel.ExcelUploadError()
                    {
                        ExcelUploadStatusId = id,
                        MessageType = e.MessageType,
                        CellReference = e.CellReference,
                        ExpectedValue = e.ExpectedValue,
                        ActualValue = e.ActualValue,
                        Description = e.Description,
                    });
                }
            }

            orm.SaveChanges();
        }

        public static ICollection<DomainEntities.ExcelUploadError> GetParseErrors(long refId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.ExcelUploadErrors.Where(x => x.ExcelUploadStatusId == refId)
                .Select(e => new DomainEntities.ExcelUploadError()
                {
                    ExcelUploadStatusId = refId,
                    MessageType = e.MessageType,
                    CellReference = e.CellReference,
                    ExpectedValue = e.ExpectedValue,
                    ActualValue = e.ActualValue,
                    Description = e.Description,
                }).ToList();
        }

        public static void DeletePendingUpload(long uploadId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var rec = orm.ExcelUploadStatus
                    .Where(x => x.Id == uploadId && x.IsPosted == false)
                    .FirstOrDefault();

            if (rec != null)
            {
                // remove excel upload errors first;
                var existingLogs = orm.ExcelUploadErrors.Where(x => x.ExcelUploadStatusId == uploadId)
                                            .ToList();
                orm.ExcelUploadErrors.RemoveRange(existingLogs);

                orm.ExcelUploadStatus.Remove(rec);
                orm.SaveChanges();
            }
        }

        public static DomainEntities.TenantEmployee GetEmployeeRecord(string imei)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.IMEIs.Where(x => x.IMEINumber == imei && x.IsActive)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefault()?.TenantEmployee;

            if (rec == null)
            {
                return null;
            }

            var te = new DomainEntities.TenantEmployee()
            {
                Id = rec.Id,
                ManagerId = rec.ManagerId,
                EmployeeCode = rec.EmployeeCode,
                Name = rec.Name,
                TenantId = rec.TenantId,
                IsActive = rec.IsActive,
                TimeIntervalInMillisecondsForTrcking = rec.TimeIntervalInMillisecondsForTracking,
                IMEI = rec.IMEIs.Where(y => y.IsActive).Select(z => z.IMEINumber).FirstOrDefault(),
                SendLogFromPhone = rec.SendLogFromPhone,
                ExecAppAccess = rec.ExecAppAccess,
                ExecAppDetailLevel = rec.ExecAppDetailLevel
            };
            return te;
        }

        public static IEnumerable<DomainEntities.SqliteActionProcessLog> GetSqliteBatchProcessLog(int startItem, int itemCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteActionProcessLogs
                .OrderByDescending(x => x.Id)
                .Skip(startItem - 1)
                .Take(itemCount)
                .Select(x => new DomainEntities.SqliteActionProcessLog()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    EmployeeId = x.EmployeeId,
                    LockAcquiredStatus = x.LockAcquiredStatus,
                    At = x.At,
                    Timestamp = x.Timestamp,
                    HasCompleted = x.HasCompleted,
                    HasFailed = x.HasFailed,
                }).ToList();
        }

        public static void DeleteUserFeatureAccess(string u)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.FeatureControls.FirstOrDefault(x => x.UserName == u);
            if (rec != null)
            {
                orm.FeatureControls.Remove(rec);
                orm.SaveChanges();
            }
        }

        public static ICollection<DomainEntities.FeatureControl> GetVirtualSuperAdminFeatureControl()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            List<DomainEntities.FeatureControl> featureControlItems = new List<DomainEntities.FeatureControl>();

            foreach (var source in orm.FeatureControls)
            {
                DomainEntities.FeatureControl target = new DomainEntities.FeatureControl();
                Utils.CopyProperties(source, target);
                featureControlItems.Add(target);
            }
            return featureControlItems;
        }

        /// <summary>
        /// Determines if app version is supported on the date
        /// </summary>
        /// <param name="appVersion"></param>
        /// <param name="at"></param>
        /// <returns></returns>
        public static bool IsAppVersionSupported(string appVersion, DateTime at)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var appVersionRecord = orm.AppVersions
                            .Where(x => x.Version == appVersion && x.EffectiveDate <= at.Date && x.ExpiryDate >= at.Date)
                            .FirstOrDefault();

            return appVersionRecord != null;
        }

        public static IEnumerable<DomainEntities.AppVersion> GetAllReleasedAppVersions()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.AppVersions.Select(x => new DomainEntities.AppVersion
            {
                Id = x.Id,
                Version = x.Version,
                Comment = x.Comment,
                EffectiveDate = x.EffectiveDate,
                ExpiryDate = x.ExpiryDate
            }).ToList();
        }

        public static DomainEntities.ExecAppImei GetImeiRecForExecAppGlobal(string Imei)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var rec = orm.ExecAppImeis
                            .Where(x => x.IMEINumber == Imei)
                            .FirstOrDefault();

            if (rec == null)
            {
                return null;
            }

            DomainEntities.ExecAppImei returnRec = new DomainEntities.ExecAppImei()
            {
                Id = rec.Id,
                Comment = rec.Comment,
                EffectiveDate = rec.EffectiveDate,
                ExpiryDate = rec.ExpiryDate,
                IMEINumber = rec.IMEINumber,
                IsSupportPerson = rec.IsSupportPerson,
                EnableLog = rec.EnableLog,
                ExecAppDetailLevel = rec.ExecAppDetailLevel
            };

            return returnRec;
        }

        /// <summary>
        /// Determines if IMEI is registered for Exec App - as Global Exec
        /// </summary>
        //public static bool IsImeiRegisteredForExecAppGlobal(string Imei, DateTime dt)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;

        //    var rec = orm.ExecAppImeis
        //                    .Where(x => x.IMEINumber == Imei && x.EffectiveDate <= dt.Date && x.ExpiryDate >= dt.Date)
        //                    .FirstOrDefault();

        //    return rec != null;
        //}

        public static IEnumerable<DomainEntities.ExecAppImei> GetAllExecAppImei()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.ExecAppImeis.Select(x => new DomainEntities.ExecAppImei
            {
                Id = x.Id,
                IMEINumber = x.IMEINumber,
                EffectiveDate = x.EffectiveDate,
                ExpiryDate = x.ExpiryDate,
                Comment = x.Comment,
                IsSupportPerson = x.IsSupportPerson,
                EnableLog = x.EnableLog,
                ExecAppDetailLevel = x.ExecAppDetailLevel
            }).ToList();
        }

        //public static IEnumerable<string> GetStaffCodesForSms(long tenantId, DateTime currentIstDateTime, string sprocName)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;

        //    //TODO: have to do this dynamically
        //    if ("SMS_GetStartDayStaffCode".Equals(sprocName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        return orm.SMS_GetStartDayStaffCode(tenantId, currentIstDateTime).ToList();
        //    }
        //    else if ("SMS_GetEndDayStaffCode".Equals(sprocName, StringComparison.OrdinalIgnoreCase))
        //    {
        //        return orm.SMS_GetEndDayStaffCode(tenantId, currentIstDateTime).ToList();
        //    }
        //    else
        //    {
        //        return new List<string>();
        //    }
        //}

        public static ICollection<string> GetStaffCodesForSms(long tenantId, DateTime currentIstDateTime, string sprocName)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.GetStaffCodes(tenantId, currentIstDateTime, sprocName).ToList();
        }

        public static DomainEntities.ExecAppImei GetSingleExecAppImei(long execAppImeiId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var imeiRec = orm.ExecAppImeis.Find(execAppImeiId);
            if (imeiRec != null)
            {
                var x = imeiRec;
                return new DomainEntities.ExecAppImei
                {
                    Id = x.Id,
                    IMEINumber = x.IMEINumber,
                    EffectiveDate = x.EffectiveDate,
                    ExpiryDate = x.ExpiryDate,
                    Comment = x.Comment,
                    IsSupportPerson = x.IsSupportPerson,
                    EnableLog = x.EnableLog,
                    ExecAppDetailLevel = x.ExecAppDetailLevel
                };
            }

            return null;
        }

        public static ICollection<DomainEntities.EntityWorkFlow> GetEntityWorkFlow(string agreement)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityWorkFlows.Where(k => k.Agreement == agreement)
                .Select(k => new DomainEntities.EntityWorkFlow()
                {
                    Id = k.Id,
                    EntityId = k.EntityId,
                    EntityName = k.Entity.EntityName,
                    CurrentPhase = k.TagName,
                    CurrentPhaseStartDate = DateTime.MinValue,
                    CurrentPhaseEndDate = DateTime.MaxValue,
                    InitiationDate = k.InitiationDate,
                    AgreementId = k.AgreementId,
                    Agreement = k.Agreement,
                    TypeName = k.TypeName,
                    TagName = k.TagName,
                    HQCode = k.Entity.HQCode
                }).ToList();
        }

        public static void UpdateBankDetail(DomainEntities.EntityBankDetail ea, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.EntityBankDetail rec = orm.EntityBankDetails.Find(ea.Id);

            if (rec == null)
            {
                throw new ArgumentException($"Invalid Bank Detail Id {ea.Id}");
            }

            DateTime ts = DateTime.UtcNow;

            rec.Status = ea.Status;
            rec.DateUpdated = ts;
            rec.UpdatedBy = currentUser;
            rec.IsApproved = ea.IsApproved;
            rec.Comments = ea.Comments;
            rec.IsActive = ea.IsActive;

            rec.IsSelfAccount = ea.IsSelfAccount;
            rec.AccountHolderName = ea.AccountHolderName;
            rec.AccountHolderPAN = ea.AccountHolderPAN;
            rec.BankName = ea.BankName;
            rec.BankAccount = ea.BankAccount;
            rec.BankIFSC = ea.BankIFSC;
            rec.BankBranch = ea.BankBranch;

            var entityName = "EntityAgreement";
            var primaryKey = ea.Id.ToString();

            // Detect changes and Log it here.
            var modifiedEntity = orm.ChangeTracker.Entries<DBModel.EntityBankDetail>()
                .Where(x => x.State == EntityState.Modified)
                .FirstOrDefault();

            if (modifiedEntity != null)
            {
                foreach (var prop in modifiedEntity.OriginalValues.PropertyNames)
                {
                    try
                    {
                        var ov = modifiedEntity.OriginalValues[prop]?.ToString() ?? null;
                        var nv = modifiedEntity.CurrentValues[prop]?.ToString() ?? null;
                        if (ov != nv)
                        {
                            orm.Audits.Add(new DBModel.Audit()
                            {
                                TableName = entityName,
                                PrimaryKey = primaryKey,
                                FieldName = prop,
                                OldValue = ov,
                                NewValue = nv,
                                Timestamp = ts,
                                User = currentUser
                            });
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            orm.SaveChanges();
        }

        public static void UpdateAgreement(DomainEntities.EntityAgreement ea, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.EntityAgreement rec = orm.EntityAgreements.Find(ea.Id);

            if (rec == null)
            {
                throw new ArgumentException($"Invalid Agreement Id {ea.Id}");
            }

            DateTime ts = DateTime.UtcNow;

            rec.Status = ea.Status;
            rec.DateUpdated = ts;
            rec.UpdatedBy = currentUser;
            rec.IsPassbookReceived = ea.IsPassbookReceived;
            rec.PassbookReceivedDate = ea.PassbookReceivedDate;
            rec.LandSizeInAcres = ea.LandSizeInAcres;

            var entityName = "EntityAgreement";
            var primaryKey = ea.Id.ToString();

            // Detect changes and Log it here.
            var modifiedEntity = orm.ChangeTracker.Entries<DBModel.EntityAgreement>()
                .Where(x => x.State == EntityState.Modified)
                .FirstOrDefault();

            if (modifiedEntity != null)
            {
                foreach (var prop in modifiedEntity.OriginalValues.PropertyNames)
                {
                    try
                    {
                        var ov = modifiedEntity.OriginalValues[prop]?.ToString() ?? null;
                        var nv = modifiedEntity.CurrentValues[prop]?.ToString() ?? null;
                        if (ov != nv)
                        {
                            orm.Audits.Add(new DBModel.Audit()
                            {
                                TableName = entityName,
                                PrimaryKey = primaryKey,
                                FieldName = prop,
                                OldValue = ov,
                                NewValue = nv,
                                Timestamp = ts,
                                User = currentUser
                            });
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            orm.SaveChanges();
        }

        public static void UpdateSurvey(DomainEntities.EntitySurvey ea, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.EntitySurvey rec = orm.EntitySurveys.Find(ea.Id);

            if (rec == null)
            {
                throw new ArgumentException($"Invalid Survey Id {ea.Id}");
            }

            DateTime ts = DateTime.UtcNow;
            rec.Status = ea.Status;
            rec.DateUpdated = ts;
            rec.UpdatedBy = currentUser;
            rec.LandSizeInAcres = ea.LandSizeInAcres;

            orm.SaveChanges();
        }

        public static ICollection<DomainEntities.WorkFlowSchedule> GetWorkFlowSchedule()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.WorkFlowSchedules.Where(x => x.IsActive)
                .Join(orm.WorkFlowFollowUps,
                x => new { x.TypeName, x.Phase },
                y => new { y.TypeName, y.Phase },
                (x, y) => new DomainEntities.WorkFlowSchedule()
                {
                    Id = x.Id,
                    TypeName = x.TypeName,
                    TagName = x.TagName,
                    Phase = x.Phase,
                    Sequence = x.Sequence,
                    TargetEndAtDay = x.TargetEndAtDay,
                    TargetStartAtDay = x.TargetStartAtDay,
                    PhoneDataEntryPage = y.PhoneDataEntryPage
                }).ToList();
        }

        public static ICollection<DomainEntities.WorkFlowFollowUp> GetWorkFlowFollowUp()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.WorkFlowFollowUps.Where(x => x.IsActive)
                .Select(x => new DomainEntities.WorkFlowFollowUp()
                {
                    Id = x.Id,
                    TypeName = x.TypeName,
                    Phase = x.Phase,
                    PhoneDataEntryPage = x.PhoneDataEntryPage,
                    FollowUpPhaseTag = x.FollowUpPhaseTag,
                    MinFollowUps = x.MinFollowUps,
                    MaxFollowUps = x.MaxFollowUps,
                    TargetStartAtDay = x.TargetStartAtDay,
                    TargetEndAtDay = x.TargetEndAtDay,
                    MaxDWS = x.MaxDWS
                }).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tenantId">NOT Using right now - as SalesPerson table does not have tenant Id</param>
        /// <param name="currentIstDateTime"></param>
        /// <returns></returns>
        public static ICollection<SmsDataEx> GetDataForSMS(long tenantId, DateTime currentIstDateTime)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IEnumerable<GetInFieldSalesPeople_Result> resultSet = orm.GetInFieldSalesPeople(currentIstDateTime, tenantId).ToList();

            return (from rs in resultSet
                    join sp in orm.SalesPersons on rs.StaffCode equals sp.StaffCode into outer
                    from t in outer.DefaultIfEmpty()
                    select new SmsDataEx()
                    {
                        ZoneCode = rs.ZoneCode,
                        AreaCode = rs.AreaCode,
                        TerritoryCode = rs.TerritoryCode,
                        HQCode = rs.HQCode,
                        StaffCode = rs.StaffCode,
                        IsInFieldToday = rs.IsInFieldToday > 0,
                        IsRegisteredOnPhone = rs.IsRegisteredOnPhone > 0,
                        Name = t?.Name ?? "",
                        Phone = t?.Phone ?? "",
                        StartTime = rs.StartTime,
                        EndTime = rs.EndTime,
                        TotalOrderAmount = rs.TotalOrderAmount,
                        TotalReturnAmount = rs.TotalReturnAmount,
                        TotalPaymentAmount = rs.TotalPaymentAmount,
                        TotalExpenseAmount = rs.TotalExpenseAmount,
                        TotalActivityCount = rs.TotalActivityCount,
                        Latitude = rs.Latitude,
                        Longitude = rs.Longitude,
                        CurrentLocTime = rs.CurrentLocTime,
                        PhoneModel = rs.PhoneModel,
                        PhoneOS = rs.PhoneOS,
                        AppVersion = rs.AppVersion
                    }).ToList();
        }

        public static ICollection<string> GetDisabledPortalLogins()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.AspNetUsers
                .Where(x => x.DisableUserAfterUtc.HasValue && x.DisableUserAfterUtc.Value < DateTime.UtcNow)
                .Select(x => x.UserName)
                .ToList();
        }

        public static DateTime? GetDisableUserAfterUtcTime(string userName)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var aspnetUser = orm.AspNetUsers.Where(x => x.UserName == userName).FirstOrDefault();
            if (aspnetUser != null)
            {
                return aspnetUser.DisableUserAfterUtc;
            }
            else
            {
                return null;
            }
        }

        public static ICollection<DomainEntities.Payment> GetPayments(DomainEntities.SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.Payment> payments = from p in orm.Payments
                                                          join c in orm.Customers on p.CustomerCode equals c.CustomerCode into temp
                                                          from t in temp.DefaultIfEmpty()
                                                          join sp in orm.SalesPersons on p.TenantEmployee.EmployeeCode equals sp.StaffCode into temp1
                                                          from t1 in temp1.DefaultIfEmpty()
                                                          select new DomainEntities.Payment
                                                          {
                                                              Id = p.Id,
                                                              EmployeeId = p.EmployeeId,
                                                              EmployeeCode = p.TenantEmployee.EmployeeCode,
                                                              EmployeeName = p.TenantEmployee.Name,
                                                              DayId = p.DayId,
                                                              TotalAmount = p.TotalAmount,
                                                              CustomerCode = p.CustomerCode,
                                                              CustomerName = (t == null) ? String.Empty : t.Name,
                                                              HQCode = (t == null) ? "" : t.HQCode,
                                                              Comment = p.Comment,
                                                              DateCreated = p.DateCreated,
                                                              DateUpdated = p.DateUpdated,
                                                              ImageCount = p.ImageCount,
                                                              PaymentDate = p.PaymentDate,
                                                              PaymentType = p.PaymentType,
                                                              SqlitePaymentId = p.SqlitePaymentId,
                                                              IsApproved = p.IsApproved,
                                                              ApprovedAmt = p.ApproveAmount,
                                                              ApprovedBy = p.ApprovedBy,
                                                              ApprovedDate = p.ApproveDate,
                                                              ApproveComments = p.ApproveNotes,
                                                              ApproveRef = p.ApproveRef,
                                                              Phone = (t1 == null) ? string.Empty : t1.Phone,
                                                              IsActive = p.TenantEmployee.IsActive,
                                                              IsActiveInSap = (t1 != null) ? t1.IsActive : false
                                                          };

            if (searchCriteria.ApplyAmountFilter)
            {
                payments = payments.Where(x => x.TotalAmount >= searchCriteria.AmountFrom && x.TotalAmount <= searchCriteria.AmountTo);
            }

            if (searchCriteria.ApplyDateFilter)
            {
                payments = payments.Where(x => x.PaymentDate >= searchCriteria.DateFrom && x.PaymentDate <= searchCriteria.DateTo);
            }

            if (searchCriteria.ApplyDataStatusFilter)
            {
                payments = payments.Where(x => x.IsApproved == searchCriteria.DataStatus);
            }
            if (searchCriteria.ApplyEmployeeStatusFilter)
            {
                payments = payments.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                payments = payments.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                payments = payments.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                payments = payments.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            // apply final security
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode)
                                        .Select(x => x.HQCode).ToList();
                payments = payments.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return payments.OrderByDescending(x => x.PaymentDate).ThenBy(x => x.CustomerName).ToList();
        }

        public static ICollection<DomainEntities.EntityWorkFlowDetail> GetEntityWorkFlowDetails(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.EntityWorkFlowDetail> entityWorkFlowDetails = orm.EntityWorkFlowDetails.Join(orm.SalesPersons,
                x => x.EntityWorkFlow.TenantEmployee.EmployeeCode,
                y => y.StaffCode, (x, y) => new DomainEntities.EntityWorkFlowDetail
                {
                    Id = x.Id,
                    EntityId = x.EntityWorkFlow.EntityId,
                    EntityName = x.EntityWorkFlow.Entity.EntityName,
                    EmployeeId = x.EntityWorkFlow.EmployeeId,
                    EmployeeCode = x.EntityWorkFlow.EmployeeCode,
                    EmployeeName = x.EntityWorkFlow.TenantEmployee.Name,
                    Sequence = x.Sequence,
                    TypeName = x.EntityWorkFlow.TypeName,
                    SeasonName = x.EntityWorkFlow.EntityAgreement.WorkflowSeason.SeasonName,
                    Phase = x.Phase,
                    PhaseCompleteStatus = x.PhaseCompleteStatus,
                    PlannedFromDate = x.PlannedStartDate,
                    PlannedEndDate = x.PlannedEndDate,
                    CompletedOn = x.ActualDate,
                    IsComplete = x.IsComplete,
                    HQCode = x.EntityWorkFlow.Entity.HQCode,
                    EmployeeIsActive = x.EntityWorkFlow.TenantEmployee.IsActive,
                    EmployeeIsActiveInSap = y.IsActive,

                    IsStarted = x.IsStarted,
                    WorkFlowDate = x.WorkFlowDate,
                    MaterialType = x.MaterialType,
                    MaterialQuantity = x.MaterialQuantity,
                    GapFillingRequired = x.GapFillingRequired,
                    GapFillingSeedQuantity = x.GapFillingSeedQuantity,
                    LaborCount = x.LaborCount,
                    PercentCompleted = x.PercentCompleted,
                    Agreement = x.EntityWorkFlow.Agreement,
                    AgreementId = x.EntityWorkFlow.AgreementId,
                    UniqueId = x.EntityWorkFlow.Entity.UniqueId,
                    ActivityId = x.ActivityId,

                    // April 11 2020
                    BatchNumber = x.BatchNumber,
                    LandSize = x.LandSize,
                    DWSEntry = x.DWSEntry,
                    ItemCount = x.ItemCount, // Plant Count or Nipping Count
                    ItemsUsedCount = x.ItemsUsedCount,
                    YieldExpected = x.YieldExpected,
                    BagsIssued = x.BagsIssued,
                    HarvestDate = x.HarvestDate,
                    EntityNumber = x.EntityWorkFlow.Entity.EntityNumber,
                    IsFollowUpRow = x.IsFollowUpRow,
                    IsActive = x.IsActive,
                    Notes = x.Notes,
                    AgreementStatus = x.EntityWorkFlow.EntityAgreement.Status
                });

            if (searchCriteria.ApplyIdFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => searchCriteria.FilterId == x.Id);
            }

            //added by sumegha on 21/4/2019 - Add Feature to filter based on the date
            if (searchCriteria.ApplyDateFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.CompletedOn >= searchCriteria.DateFrom && x.CompletedOn <= searchCriteria.DateTo);
            }
            //end

            if (searchCriteria.ApplyWorkFlowFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => searchCriteria.WorkFlow == x.Phase);
            }

            if (searchCriteria.ApplyEmployeeStatusFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => searchCriteria.EmployeeStatus == (x.EmployeeIsActive && x.EmployeeIsActiveInSap));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            if (searchCriteria.ApplyEntityNameFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.EntityName.Contains(searchCriteria.EntityName));
            }

            if (searchCriteria.ApplyAgreementNumberFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.Agreement.Contains(searchCriteria.AgreementNumber));
            }

            if (searchCriteria.ApplyEntityIdFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.EntityId == searchCriteria.EntityId);
            }

            if (searchCriteria.ApplyAgreementIdFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.AgreementId == searchCriteria.AgreementId);
            }

            if (searchCriteria.ApplyCropFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.TypeName.Equals(searchCriteria.Crop, StringComparison.OrdinalIgnoreCase));
            }

            if (searchCriteria.ApplyAgreementStatusFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.AgreementStatus.Equals(searchCriteria.AgreementStatus, StringComparison.OrdinalIgnoreCase));
            }

            if (searchCriteria.ApplyAgreementNumberFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.Agreement.Contains(searchCriteria.AgreementNumber));
            }

            if (searchCriteria.ApplyHarvestDateFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.HarvestDate == searchCriteria.HarvestDate);
            }

            if (searchCriteria.ApplyPlannedDateFilter)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x =>
                    !(x.PlannedFromDate > searchCriteria.PlannedDateTo || x.PlannedEndDate < searchCriteria.PlannedDateFrom)
                    );
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => hqList.Any(y => y == x.HQCode));
            }
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                entityWorkFlowDetails = entityWorkFlowDetails.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            if (searchCriteria.ApplyWorkFlowStatusFilter)
            {
                if (searchCriteria.WorkFlowStatus == WorkFlowStatus.OverdueCompleted)
                {
                    entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.PhaseCompleteStatus == "Late");
                }
                else if (searchCriteria.WorkFlowStatus == WorkFlowStatus.Completed)
                {
                    entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.PhaseCompleteStatus == "OnSchedule");
                }
                //else if (searchCriteria.WorkFlowStatus == WorkFlowStatus.CompletedAhead)
                //{
                //    entityWorkFlowDetails = entityWorkFlowDetails.Where(x => x.PhaseCompleteStatus == "Early");
                //}
                else if (searchCriteria.WorkFlowStatus == WorkFlowStatus.Overdue)
                {
                    entityWorkFlowDetails = entityWorkFlowDetails.Where(x => (!x.IsComplete && x.PlannedEndDate < searchCriteria.CurrentISTTime.Date));
                }
                else if (searchCriteria.WorkFlowStatus == WorkFlowStatus.Waiting)
                {
                    entityWorkFlowDetails = entityWorkFlowDetails.Where(x => (!x.IsComplete && x.PlannedFromDate > searchCriteria.CurrentISTTime.Date));
                }
                else if (searchCriteria.WorkFlowStatus == WorkFlowStatus.Due)
                {
                    entityWorkFlowDetails = entityWorkFlowDetails.Where(x => (!x.IsComplete && x.PlannedEndDate >= searchCriteria.CurrentISTTime.Date && x.PlannedFromDate <= searchCriteria.CurrentISTTime.Date));
                }
            }

            return entityWorkFlowDetails
                .OrderBy(x => new { x.EntityName, x.Sequence })
                .ToList();
        }

        public static void UpdateDWSStatus(long strTagId, STRDWSStatus siloChecked, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.SetDWSStatus(strTagId, siloChecked.ToString(), currentUser);
        }

        public static void CalculateDWSOnSiloCheck(long strWeightId, long strTagId, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.CalculateDWSOnSiloCheck(strWeightId, strTagId, currentUser);
        }

        public static ICollection<EntityProgressDetail> GetEntityProgressDetails(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.EntityProgressDetail> entityProgressDetails = orm.EntityWorkFlows.Join(orm.EntityWorkFlowDetails,
                x => new { JoinPropertyOne = x.Id, JoinPropertyTwo = x.TagName },
                y => new { JoinPropertyOne = y.EntityWorkFlowId, JoinPropertyTwo = y.TagName }, (x, y) => new EntityProgressDetail
                {
                    Id = x.Id,
                    EntityId = x.EntityId,
                    EntityName = x.Entity.EntityName,
                    EmployeeId = x.EmployeeId,
                    EmployeeCode = x.EmployeeCode,
                    EmployeeName = x.TenantEmployee.Name,
                    TypeName = x.TypeName,
                    SeasonName = x.EntityAgreement.WorkflowSeason.SeasonName,
                    LastPhase = (x.IsComplete) ? y.TagName : y.PrevPhase,
                    LastPhaseDate = (x.IsComplete) ? y.ActualDate : y.PrevPhaseActualDate,
                    CurrentPhase = y.TagName,
                    CurrentPlannedFromDate = y.PlannedStartDate,
                    CurrentPlannedEndDate = y.PlannedEndDate,
                    IsComplete = x.IsComplete,
                    HQCode = x.Entity.HQCode,
                    AgreementNumber = x.Agreement,
                    AgreementStatus = x.EntityAgreement.Status
                });

            if (searchCriteria.ApplyEntityNameFilter)
            {
                entityProgressDetails = entityProgressDetails.Where(x => x.EntityName.Contains(searchCriteria.EntityName));
            }

            if (searchCriteria.ApplyCropFilter)
            {
                entityProgressDetails = entityProgressDetails.Where(x => x.TypeName.Equals(searchCriteria.Crop, StringComparison.OrdinalIgnoreCase));
            }

            if (searchCriteria.ApplyAgreementStatusFilter)
            {
                entityProgressDetails = entityProgressDetails.Where(x => x.AgreementStatus.Equals(searchCriteria.AgreementStatus, StringComparison.OrdinalIgnoreCase));
            }

            if (searchCriteria.ApplyAgreementNumberFilter)
            {
                entityProgressDetails = entityProgressDetails.Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                entityProgressDetails = entityProgressDetails.Where(x => hqList.Any(y => y == x.HQCode));
            }
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode)
                            .Select(x => x.HQCode).ToList();
                entityProgressDetails = entityProgressDetails.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return entityProgressDetails.OrderBy(x => x.EntityName).ToList();
        }

        public static ICollection<DomainEntities.TerminateAgreementRequest> GetTerminateAgreementRequests(
                        TerminateAgreementRequestFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.TerminateAgreementRequest> terminateAgreements =
                orm.TerminateAgreementRequests
                    .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                    (tar, sp) => new DomainEntities.TerminateAgreementRequest
                    {
                        Id = tar.Id,
                        EntityAgreementId = tar.EntityAgreementId,
                        EmployeeId = tar.EmployeeId,
                        DayId = tar.DayId,
                        RequestReason = tar.RequestReason,
                        Status = tar.Status,
                        ActivityId = tar.ActivityId,
                        ReviewedBy = tar.ReviewedBy,
                        ReviewDate = tar.ReviewDate,
                        EmployeeCode = tar.TenantEmployee.EmployeeCode,
                        EmployeeName = tar.TenantEmployee.Name,
                        AgreementNumber = tar.EntityAgreement.AgreementNumber,
                        AgreementStatus = tar.EntityAgreement.Status,
                        Crop = tar.EntityAgreement.WorkflowSeason.TypeName,
                        WorkFlowSeasonName = tar.EntityAgreement.WorkflowSeason.SeasonName,
                        EntityId = tar.EntityId,
                        EntityName = tar.Entity.EntityName,
                        RequestDate = tar.RequestDate,
                        HQCode = (sp == null) ? string.Empty : sp.HQCode,

                        IsActive = tar.TenantEmployee.IsActive,
                        IsActiveInSap = (sp != null) ? sp.IsActive : false,
                    });

            if (searchCriteria != null)
            {
                if (searchCriteria.ApplyClientNameFilter)
                {
                    terminateAgreements = terminateAgreements.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
                }

                if (searchCriteria.ApplyEmployeeNameFilter)
                {
                    terminateAgreements = terminateAgreements.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
                }

                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    terminateAgreements = terminateAgreements.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
                }

                if (searchCriteria.ApplyEmployeeStatusFilter)
                {
                    terminateAgreements = terminateAgreements
                                .Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
                }

                if (searchCriteria.ApplyAgreementNumberFilter)
                {
                    terminateAgreements = terminateAgreements
                        .Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
                }

                if (searchCriteria.ApplyAgreementStatusFilter)
                {
                    terminateAgreements = terminateAgreements.Where(x => x.AgreementStatus == searchCriteria.AgreementStatus);
                }

                if (searchCriteria.ApplyRedFarmerReqReasonFilter)
                {
                    terminateAgreements = terminateAgreements.Where(x => x.RequestReason == searchCriteria.RedFarmerReqReason);
                }

                if (searchCriteria.ApplyRedFarmerReqStatusFilter)
                {
                    terminateAgreements = terminateAgreements.Where(x => x.Status == searchCriteria.RedFarmerReqStatus);
                }

                if (searchCriteria.ApplyDateFilter)
                {
                    terminateAgreements = terminateAgreements.Where(x => x.RequestDate >= searchCriteria.DateFrom && x.RequestDate <= searchCriteria.DateTo);
                }

                if (searchCriteria.ApplyCropFilter)
                {
                    terminateAgreements = terminateAgreements.Where(x => x.Crop == searchCriteria.Crop);
                }
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                terminateAgreements = terminateAgreements.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                terminateAgreements = terminateAgreements.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            return terminateAgreements.OrderBy(x => x.EmployeeName).ToList();
        }

        public static DomainEntities.TerminateAgreementRequest GetSingleTerminateAgreementRequest(long id)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.TerminateAgreementRequests.Where(x => x.Id == id)
                    .Select(ta => new DomainEntities.TerminateAgreementRequest
                    {
                        EntityAgreementId = ta.EntityAgreementId,
                        AgreementNumber = ta.EntityAgreement.AgreementNumber,
                        Crop = ta.EntityAgreement.WorkflowSeason.TypeName,
                        RequestReason = ta.RequestReason,
                        EntityName = ta.EntityAgreement.Entity.EntityName,
                        Status = ta.Status,
                        ReviewDate = ta.ReviewDate,
                        ActivityId = ta.ActivityId,
                        Notes = ta.RequestNotes
                    }).FirstOrDefault();
        }

        public static void SaveTerminateAgreementRequestStatus(DomainEntities.TerminateAgreementRequest terminateAgreement, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.TerminateAgreementRequests.Find(terminateAgreement.Id);

            if (sp == null)
            {
                throw new ArgumentException($"Terminate Agreement request id {terminateAgreement.Id} does not exist.");
            }

            DateTime ts = DateTime.UtcNow;
            try
            {
                sp.Status = terminateAgreement.Status;
                sp.UpdatedBy = currentUser;
                sp.DateUpdated = ts;

                sp.ReviewDate = ts;
                sp.ReviewedBy = currentUser;

                // audit
                // have to terminate agreement, if approved

                var entityName = "TerminateAgreementRequest";
                var primaryKey = terminateAgreement.Id.ToString();

                // Detect changes and Log it here.
                var modifiedEntity = orm.ChangeTracker.Entries<DBModel.TerminateAgreementRequest>()
                    .Where(x => x.State == EntityState.Modified)
                    .FirstOrDefault();

                if (modifiedEntity != null)
                {
                    foreach (var prop in modifiedEntity.OriginalValues.PropertyNames)
                    {
                        try
                        {
                            var ov = modifiedEntity.OriginalValues[prop]?.ToString() ?? null;
                            var nv = modifiedEntity.CurrentValues[prop]?.ToString() ?? null;
                            if (ov != nv)
                            {
                                orm.Audits.Add(new DBModel.Audit()
                                {
                                    TableName = entityName,
                                    PrimaryKey = primaryKey,
                                    FieldName = prop,
                                    OldValue = ov,
                                    NewValue = nv,
                                    Timestamp = ts,
                                    User = currentUser
                                });
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }

                orm.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"Terminate Agreement request is not updated for Entity : {terminateAgreement.EntityName}; Request Id: {terminateAgreement.Id};";
                LogError($"{nameof(SaveTerminateAgreementRequestStatus)}", errorText, logSnip);
            }
        }

        public static ICollection<DomainEntities.AdvanceRequest> GetAdvanceRequests(AdvanceRequestFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.AdvanceRequest> advancerequests =
                orm.AdvanceRequests
                    .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                    (tar, sp) => new DomainEntities.AdvanceRequest()
                    {
                        Id = tar.Id,
                        EmployeeCode = tar.TenantEmployee.EmployeeCode,
                        EmployeeName = tar.TenantEmployee.Name,
                        HQCode = (sp == null) ? string.Empty : sp.HQCode,
                        EntityId = tar.EntityId,
                        EntityName = tar.Entity.EntityName,

                        AgreementNumber = tar.EntityAgreement.AgreementNumber,
                        AgreementStatus = tar.EntityAgreement.Status,
                        Crop = tar.EntityAgreement.WorkflowSeason.TypeName,
                        WorkFlowSeasonName = tar.EntityAgreement.WorkflowSeason.SeasonName,
                        Amount = tar.Amount,
                        ReviewedBy = tar.ReviewedBy,
                        ReviewDate = tar.ReviewDate,

                        EmployeeId = tar.EmployeeId,
                        DayId = tar.DayId,
                        AdvanceRequestStatus = tar.Status,
                        ActivityId = tar.ActivityId,
                        AmountApproved = tar.AmountApproved,

                        ApproveNotes = tar.ApproveNotes,
                        RequestNotes = tar.RequestNotes,
                        AdvanceRequestDate = tar.AdvanceRequestDate,

                        IsActive = tar.TenantEmployee.IsActive,
                        IsActiveInSap = (sp != null) ? sp.IsActive : false,
                        EntityAgreementId = tar.EntityAgreementId
                    });

            if (searchCriteria != null)
            {
                if (searchCriteria.ApplyClientNameFilter)
                {
                    advancerequests = advancerequests.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
                }

                if (searchCriteria.ApplyEmployeeNameFilter)
                {
                    advancerequests = advancerequests.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
                }

                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    advancerequests = advancerequests.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
                }

                if (searchCriteria.ApplyEmployeeStatusFilter)
                {
                    advancerequests = advancerequests.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
                }

                if (searchCriteria.ApplyAgreementNumberFilter)
                {
                    advancerequests = advancerequests.Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
                }

                if (searchCriteria.ApplyAgreementStatusFilter)
                {
                    advancerequests = advancerequests.Where(x => x.AgreementStatus == searchCriteria.AgreementStatus);
                }

                if (searchCriteria.ApplyAdvanceRequestStatusFilter)
                {
                    advancerequests = advancerequests.Where(x => x.AdvanceRequestStatus == searchCriteria.AdvanceRequestStatus);
                }

                if (searchCriteria.ApplyDateFilter)
                {
                    advancerequests = advancerequests.Where(x => x.AdvanceRequestDate >= searchCriteria.DateFrom &&
                                                                 x.AdvanceRequestDate <= searchCriteria.DateTo);
                }

                if (searchCriteria.ApplyCropFilter)
                {
                    advancerequests = advancerequests.Where(x => x.Crop == searchCriteria.Crop);
                }

                // May 29 2020 - for DWS Amount approval
                if (searchCriteria.ApplyEntityIdFilter)
                {
                    advancerequests = advancerequests.Where(x => x.EntityId == searchCriteria.EntityId);
                }

                if (searchCriteria.ApplyAgreementIdFilter)
                {
                    advancerequests = advancerequests.Where(x => x.EntityAgreementId == searchCriteria.AgreementId);
                }
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                advancerequests = advancerequests.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                advancerequests = advancerequests.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            return advancerequests.OrderBy(x => x.EntityName).ToList();
        }

        public static ICollection<DomainEntities.Entity> GetEntities(EntitiesFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.Entity> entities = from e in orm.Entities.Include("TenantEmployee")
                                                             //where e.IsActive
                                                         select new DomainEntities.Entity
                                                         {
                                                             Id = e.Id,
                                                             EmployeeId = e.EmployeeId,
                                                             DayId = e.DayId,
                                                             HQCode = e.HQCode,
                                                             EmployeeCode = e.TenantEmployee.EmployeeCode,
                                                             EmployeeName = e.TenantEmployee.Name,
                                                             AtBusiness = e.AtBusiness,
                                                             //Consent = e.Consent, //swetha made thechnages on 24-11
                                                             EntityType = e.EntityType,
                                                             EntityName = e.EntityName,
                                                             EntityDate = e.EntityDate,
                                                             Address = e.Address,
                                                             City = e.City,
                                                             State = e.State,
                                                             Pincode = e.Pincode,
                                                             LandSize = e.LandSize,
                                                             Latitude = e.Latitude,
                                                             Longitude = e.Longitude,
                                                             SqliteEntityId = e.SqliteEntityId,
                                                             ContactCount = e.ContactCount,
                                                             CropCount = e.CropCount,
                                                             ImageCount = e.ImageCount,
                                                             AgreementCount = e.EntityAgreements.Count, // e.AgreementCount,
                                                             UniqueIdType = e.UniqueIdType,
                                                             UniqueId = e.UniqueId,
                                                             TaxId = e.TaxId,
                                                             FatherHusbandName = e.FatherHusbandName,
                                                             HQName = e.HQName,
                                                             TerritoryCode = e.TerritoryCode,
                                                             TerritoryName = e.TerritoryName,
                                                             //MajorCrop = e.MajorCrop,
                                                             //LastCrop = e.LastCrop,
                                                             //WaterSource = e.WaterSource,
                                                             //SoilType = e.SoilType,
                                                             //SowingType = e.SowingType,
                                                             //SowingDate = e.SowingDate,
                                                             CreatedBy =e.CreatedBy,
                                                             EntityNumber = e.EntityNumber,
                                                             IsActive = e.IsActive,
                                                             BankDetailCount = e.EntityBankDetails.Count,
                                                             SurveryDetailCount = e.EntitySurveys.Count,
                                                             DWSCount = e.DWS.Count,
                                                             IssueReturnCount = e.IssueReturns.Count,
                                                             AdvanceRequestCount = e.AdvanceRequests.Count
                                                         };

            if (searchCriteria.ApplyActiveFilter)
            {
                entities = entities.Where(x => x.IsActive == searchCriteria.IsActive);
            }

            if (searchCriteria.ApplyClientNameFilter)
            {
                entities = entities.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyClientTypeFilter)
            {
                entities = entities.Where(x => x.EntityType == searchCriteria.ClientType);
            }

            //added by sumegha on 20/4/2019 - Add search fields on entity page.
            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                entities = entities.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                entities = entities.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyAgreementStatusFilter &&
                searchCriteria.AgreementStatus.Equals(DomainEntities.Constant.NoAgreement,
                                                StringComparison.OrdinalIgnoreCase))
            {
                entities = entities.Where(x => x.AgreementCount == 0);
                // if user has selected No Agreement as filter, then there is no point
                // applying AgreementNumber filter
            }
            else
            {
                if (searchCriteria.ApplyAgreementNumberFilter || searchCriteria.ApplyAgreementStatusFilter)
                {
                    // find those entityIds that satisfy the condition
                    var entityAgreements = orm.EntityAgreements
                        .Select(x => new { x.AgreementNumber, x.Status, x.EntityId });

                    //Infuture if we get any performance issue with search fields, we need to change this part.
                    if (searchCriteria.ApplyAgreementNumberFilter)
                    {
                        entityAgreements = entityAgreements.Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
                    }

                    if (searchCriteria.ApplyAgreementStatusFilter)
                    {
                        entityAgreements = entityAgreements.Where(x => x.Status == searchCriteria.AgreementStatus);
                    }
                    
                    var filteredAgreements = entityAgreements.Select(x => x.EntityId).ToList();
                    entities = entities.Where(x => filteredAgreements.Any(y => y == x.Id));
                }
            }

            // March 21 2020
            if (searchCriteria.ApplyBankDetailStatusFilter)
            {
                // find those entityIds that satisfy the condition
                var entityBankDetails = orm.EntityBankDetails
                    .Where(x => x.Status == searchCriteria.BankDetailStatus)
                    .Select(x => x.EntityId)
                    .ToList();

                entities = entities.Where(x => entityBankDetails.Any(y => y == x.Id));
            }

            if (searchCriteria.ApplyUniqueIdFilter)
            {
                entities = entities.Where(x => x.UniqueId.Contains(searchCriteria.UniqueId));
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                entities = entities.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security for user
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                entities = entities.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }
            if (searchCriteria.ApplyEntityNumberFilter)
            {
                entities = entities.Where(x => x.EntityNumber == searchCriteria.EntityNumber);
            }
            return entities.OrderBy(x => x.EntityName).ToList();
        }

        public static ICollection<DomainEntities.STRTag> GetSTRTag(STRFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.STRTag> items = from e in orm.STRTags.Include("STR")
                                                      select new DomainEntities.STRTag
                                                      {
                                                          Id = e.Id,
                                                          STRNumber = e.STRNumber,
                                                          STRDate = e.STRDate,
                                                          STRCount = e.STRs.Count,
                                                          DWSCount = e.STRs.Sum(y => y.DWSCount),
                                                          BagCount = e.STRs.Sum(y => y.BagCount),
                                                          GrossWeight = e.STRs.Sum(y => y.GrossWeight),
                                                          NetWeight = e.STRs.Sum(y => y.NetWeight),
                                                          Status = e.Status
                                                      };

            if (searchCriteria.ApplyDWSNumberFilter || searchCriteria.ApplyAgreementNumberFilter
                || searchCriteria.ApplyClientNameFilter || searchCriteria.ApplyTypeNameFilter)
            {
                var dbDWS = orm.DWS.Select(x => x); ;
                if (searchCriteria.ApplyDWSNumberFilter)
                {
                    dbDWS = dbDWS.Where(p => p.DWSNumber.ToUpper().Contains(searchCriteria.DWSNumber.ToUpper()));
                }

                if (searchCriteria.ApplyAgreementNumberFilter)
                {
                    dbDWS = dbDWS.Where(p => p.Agreement.Contains(searchCriteria.AgreementNumber));
                }

                if (searchCriteria.ApplyClientNameFilter)
                {
                    dbDWS = dbDWS.Where(p => p.Entity.EntityName.Contains(searchCriteria.ClientName));
                }

                if (searchCriteria.ApplyTypeNameFilter)
                {
                    dbDWS = dbDWS.Where(p => p.TypeName.Contains(searchCriteria.TypeName));
                }

                List<long> strTagIds = dbDWS.Select(x => x.STRTagId).ToList();

                items = items.Where(y => strTagIds.Any(z => z == y.Id));
            }

            if (searchCriteria.ApplySTRNumberFilter)
            {
                if (searchCriteria.IsExactSTRNumberMatch)
                {
                    items = items.Where(x => x.STRNumber == searchCriteria.STRNumber);
                }
                else
                {
                    items = items.Where(x => x.STRNumber.Contains(searchCriteria.STRNumber));
                }
            }

            if (searchCriteria.ApplyDateFilter)
            {
                items = items.Where(x => x.STRDate >= searchCriteria.DateFrom &&
                                                                x.STRDate <= searchCriteria.DateTo);
            }

            return items.OrderBy(x => x.STRNumber).ToList();
        }

        public static ICollection<DomainEntities.STRWeight> GetSTRWeight(STRFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.STRWeight> items = from e in orm.STRWeights
                                                         select new DomainEntities.STRWeight
                                                         {
                                                             Id = e.Id,
                                                             STRNumber = e.STRNumber,
                                                             STRDate = e.STRDate,
                                                             EntryWeight = e.EntryWeight,
                                                             ExitWeight = e.ExitWeight,
                                                             SiloNumber = e.SiloNumber,
                                                             SiloIncharge = e.SiloIncharge,
                                                             UnloadingIncharge = e.UnloadingIncharge,
                                                             ExitOdometer = e.ExitOdometer,
                                                             BagCount = e.BagCount,
                                                             Notes = e.Notes,
                                                             Status = e.Status,
                                                             DeductionPercent = e.DeductionPercent,
                                                             CyclicCount = e.CyclicCount,
                                                             DWSCount = e.DWSCount,
                                                             VehicleNumber = e.VehicleNumber
                                                         };

            if (searchCriteria.ApplySTRNumberFilter)
            {
                if (searchCriteria.IsExactSTRNumberMatch)
                {
                    items = items.Where(x => x.STRNumber == searchCriteria.STRNumber);
                }
                else
                {
                    items = items.Where(x => x.STRNumber.Contains(searchCriteria.STRNumber));
                }
            }

            if (searchCriteria.ApplyDateFilter)
            {
                items = items.Where(x => x.STRDate >= searchCriteria.DateFrom &&
                                                             x.STRDate <= searchCriteria.DateTo);
            }

            return items.OrderBy(x => x.STRNumber).ToList();
        }

        public static DomainEntities.STRWeight GetSingleSTRWeight(long strWeightId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from e in orm.STRWeights
                    where e.Id == strWeightId
                    select new DomainEntities.STRWeight
                    {
                        Id = e.Id,
                        STRNumber = e.STRNumber,
                        STRDate = e.STRDate,
                        EntryWeight = e.EntryWeight,
                        ExitWeight = e.ExitWeight,
                        SiloNumber = e.SiloNumber,
                        SiloIncharge = e.SiloIncharge,
                        UnloadingIncharge = e.UnloadingIncharge,
                        ExitOdometer = e.ExitOdometer,
                        BagCount = e.BagCount,
                        Notes = e.Notes,
                        Status = e.Status,
                        DeductionPercent = e.DeductionPercent,
                        CyclicCount = e.CyclicCount,
                        DWSCount = e.DWSCount,
                        VehicleNumber = e.VehicleNumber
                    }).FirstOrDefault();
        }

        public static DomainEntities.STRTag GetSingleSTRTag(long strTagId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from e in orm.STRTags.Include("STR")
                    where e.Id == strTagId
                    select new DomainEntities.STRTag
                    {
                        Id = e.Id,
                        STRNumber = e.STRNumber,
                        STRDate = e.STRDate,
                        STRCount = e.STRs.Count,
                        DWSCount = e.STRs.Sum(y => y.DWSCount),
                        BagCount = e.STRs.Sum(y => y.BagCount),
                        GrossWeight = e.STRs.Sum(y => y.GrossWeight),
                        NetWeight = e.STRs.Sum(y => y.NetWeight),
                        StartOdo = e.STRs.Min(y => y.StartOdometer),
                        CyclicCount = e.CyclicCount,
                        Status = e.Status
                    }).FirstOrDefault();
        }

        public static ICollection<DomainEntities.STR> GetSTR(long strTagId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.STRs.Where(x => x.STRTagId == strTagId)
                .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                (ex, sp) => new DomainEntities.STR()
                {
                    Id = ex.Id,
                    STRTagId = ex.STRTagId,
                    EmployeeCode = ex.TenantEmployee.EmployeeCode,
                    EmployeeName = ex.TenantEmployee.Name,
                    EmployeePhone = sp.Phone,
                    EmployeeId = ex.EmployeeId,
                    VehicleNumber = ex.VehicleNumber,
                    DriverName = ex.DriverName,
                    DriverPhone = ex.DriverPhone,
                    DWSCount = ex.DWSCount,
                    BagCount = ex.BagCount,
                    GrossWeight = ex.GrossWeight,
                    NetWeight = ex.NetWeight,
                    StartOdometer = ex.StartOdometer,
                    EndOdometer = ex.EndOdometer,
                    IsNew = ex.IsNew,
                    IsTransferred = ex.IsTransferred,
                    TransfereeName = ex.TransfereeName,
                    TransfereePhone = ex.TransfereePhone,
                    ImageCount = ex.ImageCount,
                    ActivityId = ex.ActivityId,
                    ActivityId2 = ex.ActivityId2,
                    STRNumber = ex.STRTag.STRNumber
                }).ToList();
        }

        public static DomainEntities.STR GetSingleSTR(long strId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.STRs.Where(x => x.Id == strId)
                .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                (ex, sp) => new DomainEntities.STR()
                {
                    Id = ex.Id,
                    STRTagId = ex.STRTagId,
                    EmployeeCode = ex.TenantEmployee.EmployeeCode,
                    EmployeeName = ex.TenantEmployee.Name,
                    EmployeePhone = sp.Phone,
                    EmployeeId = ex.EmployeeId,
                    VehicleNumber = ex.VehicleNumber,
                    DriverName = ex.DriverName,
                    DriverPhone = ex.DriverPhone,
                    DWSCount = ex.DWSCount,
                    BagCount = ex.BagCount,
                    GrossWeight = ex.GrossWeight,
                    NetWeight = ex.NetWeight,
                    StartOdometer = ex.StartOdometer,
                    EndOdometer = ex.EndOdometer,
                    IsNew = ex.IsNew,
                    IsTransferred = ex.IsTransferred,
                    TransfereeName = ex.TransfereeName,
                    TransfereePhone = ex.TransfereePhone,
                    ImageCount = ex.ImageCount,
                    ActivityId = ex.ActivityId,
                    ActivityId2 = ex.ActivityId2,
                    STRNumber = ex.STRTag.STRNumber,
                    StrTagCyclicCount = ex.STRTag.CyclicCount
                }).FirstOrDefault();
        }

        public static ICollection<DomainEntities.DWS> GetDWS(long strId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.DWS.Include("Entity")
                .Where(x => x.STRId == strId)
                .Select(x => new DomainEntities.DWS()
                {
                    Id = x.Id,
                    STRId = x.STRId,
                    STRTagId = x.STRTagId,
                    DWSNumber = x.DWSNumber,
                    DWSDate = x.DWSDate,
                    BagCount = x.BagCount,
                    FilledBagsWeightKg = x.FilledBagsWeightKg,
                    EmptyBagsWeightKg = x.EmptyBagsWeightKg,
                    EntityId = x.EntityId,
                    EntityName = x.Entity.EntityName,
                    AgreementId = x.AgreementId,
                    Agreement = x.Agreement,
                    EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                    TypeName = x.TypeName,
                    TagName = x.TagName,
                    ActivityId = x.ActivityId,

                    SiloDeductPercent = x.SiloDeductPercent,
                    GoodsWeight = x.GoodsWeight,
                    SiloDeductWt = x.SiloDeductWt,
                    SiloDeductWtOverride = x.SiloDeductWtOverride,
                    NetPayableWt = x.NetPayableWt,
                    RatePerKg = x.RatePerKg,
                    GoodsPrice = x.GoodsPrice,
                    DeductAmount = x.DeductAmount,
                    NetPayable = x.NetPayable,
                    OrigBagCount = x.OrigBagCount,
                    OrigFilledBagsKg = x.OrigFilledBagsKg,
                    OrigEmptyBagsKg = x.OrigEmptyBagsKg,
                    Status = x.Status,

                    Comments = x.Comments,
                    WtApprovedBy = x.WtApprovedBy,
                    WtApprovedDate = x.WtApprovedDate,
                    AmountApprovedBy = x.AmountApprovedBy,
                    AmountApprovedDate = x.AmountApprovedDate,
                    PaidBy = x.PaidBy,
                    PaidDate = x.PaidDate,
                    PaymentReference = x.PaymentReference,
                    HQCode = x.Entity.HQCode,
                    CyclicCount = x.CyclicCount,
                    STRNumber = x.STRTag.STRNumber,
                    StrTagCyclicCount = x.STRTag.CyclicCount,

                    BankAccountName = x.BankAccountName,
                    BankName = x.BankName,
                    BankAccount = x.BankAccount,
                    BankIFSC = x.BankIFSC,
                    BankBranch = x.BankBranch

                }).ToList();
        }

        public static ICollection<DomainEntities.DWSAudit> GetDWSAudit(long strId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.DWSAudits
                .Where(x => x.STRId == strId)
                .Select(x => new DomainEntities.DWSAudit()
                {
                    Id = x.Id,
                    DWSId = x.DWSId,
                    STRId = x.STRId,
                    STRTagId = x.STRTagId,
                    DWSNumber = x.DWSNumber,
                    DWSDate = x.DWSDate,
                    BagCount = x.BagCount,
                    FilledBagsWeightKg = x.FilledBagsWeightKg,
                    EmptyBagsWeightKg = x.EmptyBagsWeightKg,
                    EntityId = x.EntityId,
                    AgreementId = x.AgreementId,
                    Agreement = x.Agreement,
                    EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                    TypeName = x.TypeName,
                    TagName = x.TagName,
                    CreatedBy = x.CreatedBy,
                    DateCreated = x.DateCreated
                }).ToList();
        }

        public static DomainEntities.DWS GetSingleDWS(long dwsId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.DWS.Include("Entity")
                .Where(x => x.Id == dwsId)
                .Select(x => new DomainEntities.DWS()
                {
                    Id = x.Id,
                    STRId = x.STRId,
                    STRTagId = x.STRTagId,
                    DWSNumber = x.DWSNumber,
                    DWSDate = x.DWSDate,
                    BagCount = x.BagCount,
                    FilledBagsWeightKg = x.FilledBagsWeightKg,
                    EmptyBagsWeightKg = x.EmptyBagsWeightKg,
                    EntityId = x.EntityId,
                    EntityName = x.Entity.EntityName,
                    AgreementId = x.AgreementId,
                    Agreement = x.Agreement,
                    EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                    TypeName = x.TypeName,
                    TagName = x.TagName,
                    ActivityId = x.ActivityId,
                    STRNumber = x.STRTag.STRNumber,
                    EmployeeId = x.STR.EmployeeId,

                    GoodsWeight = x.GoodsWeight,
                    SiloDeductPercent = x.SiloDeductPercent,
                    SiloDeductWt = x.SiloDeductWt,
                    SiloDeductWtOverride = x.SiloDeductWtOverride,
                    NetPayableWt = x.NetPayableWt,
                    RatePerKg = x.RatePerKg,
                    GoodsPrice = x.GoodsPrice,
                    DeductAmount = x.DeductAmount,
                    NetPayable = x.NetPayable,
                    OrigBagCount = x.OrigBagCount,
                    OrigFilledBagsKg = x.OrigFilledBagsKg,
                    OrigEmptyBagsKg = x.OrigEmptyBagsKg,
                    Status = x.Status,
                    Comments = x.Comments,
                    WtApprovedBy = x.WtApprovedBy,
                    WtApprovedDate = x.WtApprovedDate,
                    AmountApprovedBy = x.AmountApprovedBy,
                    AmountApprovedDate = x.AmountApprovedDate,
                    PaidBy = x.PaidBy,
                    PaidDate = x.PaidDate,
                    PaymentReference = x.PaymentReference,
                    StrTagCyclicCount = x.STRTag.CyclicCount,
                    HQCode = x.Entity.HQCode,
                    CyclicCount = x.CyclicCount,

                    BankAccountName = x.BankAccountName,
                    BankName = x.BankName,
                    BankAccount = x.BankAccount,
                    BankIFSC = x.BankIFSC,
                    BankBranch = x.BankBranch
                }).FirstOrDefault();
        }

        public static void UpdateSTRPayment()
        {

        }

        public static int ApprovePayment(ApprovalData approvalData)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var payment = orm.Payments.Find(approvalData.Id);
                if (payment == null)
                {
                    return 2; //Invalid expense Id
                }
                if (payment.IsApproved)
                {
                    return 0; //expense already approved
                }
                payment.IsApproved = true;
                payment.ApproveRef = approvalData.ApproveRef;
                payment.ApproveAmount = approvalData.ApprovedAmt;
                payment.ApproveNotes = approvalData.ApproveComments;
                payment.ApproveDate = approvalData.ApprovedDate;
                payment.ApprovedBy = approvalData.ApprovedBy;
                payment.DateUpdated = DateTime.UtcNow;
                orm.SaveChanges();

                return 1; //Success
            }
        }

        public static ICollection<DomainEntities.UnSownData> GetUnSownData(DomainEntities.EntitiesFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.UnSownData> entities = from e in orm.Entities.Include("TenantEmployee")
                                                             where e.IsActive
                                                             select new DomainEntities.UnSownData
                                                             {
                                                                 Id = e.Id,
                                                                 EmployeeId = e.EmployeeId,
                                                                 DayId = e.DayId,
                                                                 HQCode = e.HQCode,
                                                                 EmployeeCode = e.TenantEmployee.EmployeeCode,
                                                                 EmployeeName = e.TenantEmployee.Name,
                                                                 EntityType = e.EntityType,
                                                                 EntityName = e.EntityName,
                                                                 LandSize = e.LandSize,
                                                                 SqliteEntityId = e.SqliteEntityId,
                                                                 UniqueIdType = e.UniqueIdType,
                                                                 UniqueId = e.UniqueId
                                                             };

            if (searchCriteria.ApplyClientNameFilter)
            {
                entities = entities.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyClientTypeFilter)
            {
                entities = entities.Where(x => x.EntityType == searchCriteria.ClientType);
            }

            //added by sumegha on 20/4/2019 - Add search fields on entity page.
            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                entities = entities.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                entities = entities.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyAgreementStatusFilter &&
                searchCriteria.AgreementStatus.Equals(DomainEntities.Constant.NoAgreement,
                                                StringComparison.OrdinalIgnoreCase))
            {
                ;
                //entities = entities.Where(x => x.AgreementCount == 0);
                // if user has selected No Agreement as filter, then there is no point
                // applying AgreementNumber filter
            }
            else
            {
                if (searchCriteria.ApplyAgreementNumberFilter ||
                    searchCriteria.ApplyAgreementStatusFilter ||
                    searchCriteria.ApplyCropFilter)
                {
                    // find those entityIds that satisfy the condition
                    var entityAgreements = orm.EntityAgreements
                        .Select(x => new { x.AgreementNumber, x.Status, x.EntityId, x.WorkflowSeason.TypeName });

                    //Infuture if we get any performance issue with search fields, we need to change this part.
                    if (searchCriteria.ApplyAgreementNumberFilter)
                    {
                        entityAgreements = entityAgreements.Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
                    }

                    if (searchCriteria.ApplyAgreementStatusFilter)
                    {
                        entityAgreements = entityAgreements.Where(x => x.Status == searchCriteria.AgreementStatus);
                    }

                    if (searchCriteria.ApplyCropFilter)
                    {
                        entityAgreements = entityAgreements.Where(x => x.TypeName == searchCriteria.Crop);
                    }

                    var filteredAgreements = entityAgreements.Select(x => x.EntityId).ToList();
                    entities = entities.Where(x => filteredAgreements.Any(y => y == x.Id));
                }
            }

            if (searchCriteria.ApplyUniqueIdFilter)
            {
                entities = entities.Where(x => x.UniqueId.Contains(searchCriteria.UniqueId));
            }
            //end

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                entities = entities.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security for user
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                entities = entities.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            return entities.OrderBy(x => x.EntityName).ToList();
        }

        public static ICollection<DomainEntities.EntityContact> GetEntityContacts(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityContacts.Where(x => x.EntityId == entityId)
                    .Select(x => new DomainEntities.EntityContact()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        Name = x.Name,
                        PhoneNumber = x.PhoneNumber,
                        IsPrimary = x.IsPrimary
                    }).ToList();
        }
        //Added by swetha -To get Conatct Details for entites
        public static IEnumerable<DomainEntities.EntityContact> GetEntityContactDetails(IEnumerable<long> entityIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from x in orm.EntityContacts
                    join ids in entityIds on x.EntityId equals ids
                    select new DomainEntities.EntityContact()
                    {

                        Id = x.Id,
                        EntityId = x.EntityId,
                        Name = x.Name,
                        PhoneNumber = x.PhoneNumber

                    }).ToList();
        }
        //Added by swetha -To get Crop Details for entites
        public static IEnumerable<DomainEntities.EntityCrop> GetEntityCropDetails(IEnumerable<long> entityIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from x in orm.EntityCrops
                    join ids in entityIds on x.EntityId equals ids
                    select new DomainEntities.EntityCrop()
                    {

                        Id = x.Id,
                        EntityId = x.EntityId,
                        CropName = x.CropName

                    }).ToList();
        }

        public static ICollection<DomainEntities.EntityAgreement> GetEntityAgreements(string agreementStatus, string activityType)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityAgreements.Where(x => x.Status == agreementStatus)
                    .Select(x => new DomainEntities.EntityAgreement()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        TypeName = x.WorkflowSeason.TypeName,
                        AgreementNumber = x.AgreementNumber,
                        Status = x.Status,
                        ActivityCount = x.IssueReturns.Where(y => y.TransactionType == activityType).Count(),
                        TotalAdvanceRequested = x.AdvanceRequests.Sum(y => y.Amount),
                        TotalAdvanceApproved = x.AdvanceRequests.Sum(y => y.AmountApproved),
                    }).ToList();
        }

        public static ICollection<DomainEntities.EntityAgreement> GetEntityAgreements(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityAgreements.Where(x => x.EntityId == entityId)
                    .Select(x => new DomainEntities.EntityAgreement()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        WorkflowSeasonId = x.WorkflowSeasonId,
                        WorkflowSeasonName = x.WorkflowSeason.SeasonName,
                        TypeName = x.WorkflowSeason.TypeName,
                        AgreementNumber = x.AgreementNumber,
                        Status = x.Status,
                        IsPassbookReceived = x.IsPassbookReceived,
                        PassbookReceivedDate = x.PassbookReceivedDate,
                        LandSizeInAcres = x.LandSizeInAcres,
                        RatePerKg = x.RatePerKg,
                        DWSCount = x.DWS.Count,
                        IssueReturnCount = x.IssueReturns.Count,
                        AdvanceRequestCount = x.AdvanceRequests.Count,
                        // workflowCount will be either 0 or 1 - as we are checking on parent table
                        // and not on detail table.
                        HasWorkflow = x.EntityWorkFlows.Count > 0 ? true : false,
                        ActivityId = x.ActivityId,
                        CreatedBy = x.CreatedBy
                    }).ToList();
        }

        public static ICollection<DomainEntities.EntitySurvey> GetEntitySurveys(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntitySurveys.Where(x => x.EntityId == entityId)
                    .Select(x => new DomainEntities.EntitySurvey()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        WorkflowSeasonId = x.WorkflowSeasonId,
                        WorkflowSeasonName = x.WorkflowSeason.SeasonName,
                        TypeName = x.WorkflowSeason.TypeName,
                        SurveyNumber = x.SurveyNumber,
                        MajorCrop = x.MajorCrop,
                        LastCrop = x.LastCrop,
                        WaterSource = x.WaterSource,
                        SoilType = x.SoilType,
                        LandSizeInAcres = x.LandSizeInAcres,
                        SowingDate = x.SowingDate,
                        Status = x.Status,
                        ActivityId = x.ActivityId
                    }).ToList();
        }

        public static ICollection<DomainEntities.EntityBankDetail> GetEntityBankDetails(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityBankDetails.Where(x => x.EntityId == entityId)
                    .Select(x => new DomainEntities.EntityBankDetail()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        IsSelfAccount = x.IsSelfAccount,
                        AccountHolderName = x.AccountHolderName,
                        AccountHolderPAN = x.AccountHolderPAN,
                        BankName = x.BankName,
                        BankAccount = x.BankAccount,
                        BankIFSC = x.BankIFSC,
                        BankBranch = x.BankBranch,
                        ImageCount = x.ImageCount,
                        IsActive = x.IsActive,
                        Status = x.Status,
                        IsApproved = x.IsApproved,
                        Comments = x.Comments
                    }).ToList();
        }

        public static DomainEntities.EntityBankDetail GetSingleBankDetail(long entityBankDetailId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityBankDetails.Where(x => x.Id == entityBankDetailId)
                    .Select(x => new DomainEntities.EntityBankDetail()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        IsSelfAccount = x.IsSelfAccount,
                        AccountHolderName = x.AccountHolderName,
                        AccountHolderPAN = x.AccountHolderPAN,
                        BankName = x.BankName,
                        BankAccount = x.BankAccount,
                        BankIFSC = x.BankIFSC,
                        BankBranch = x.BankBranch,
                        IsActive = x.IsActive,
                        Status = x.Status,
                        IsApproved = x.IsApproved,
                        Comments = x.Comments
                    }).FirstOrDefault();
        }

        public static DomainEntities.EntityAgreement GetSingleAgreement(long entityAgreementId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityAgreements.Where(x => x.Id == entityAgreementId)
                    .Select(x => new DomainEntities.EntityAgreement()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        WorkflowSeasonId = x.WorkflowSeasonId,
                        WorkflowSeasonName = x.WorkflowSeason.SeasonName,
                        AgreementNumber = x.AgreementNumber,
                        TypeName = x.WorkflowSeason.TypeName,
                        Status = x.Status,
                        UniqueId = x.Entity.UniqueId,
                        IsPassbookReceived = x.IsPassbookReceived,
                        PassbookReceivedDate = x.PassbookReceivedDate,
                        LandSizeInAcres = x.LandSizeInAcres
                    }).FirstOrDefault();
        }

        public static DomainEntities.EntitySurvey GetSingleSurvey(long entitySurveyId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntitySurveys.Where(x => x.Id == entitySurveyId)
                    .Select(x => new DomainEntities.EntitySurvey()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        WorkflowSeasonId = x.WorkflowSeasonId,
                        WorkflowSeasonName = x.WorkflowSeason.SeasonName,
                        TypeName = x.WorkflowSeason.TypeName,
                        SurveyNumber = x.SurveyNumber,
                        MajorCrop = x.MajorCrop,
                        LastCrop = x.LastCrop,
                        WaterSource = x.WaterSource,
                        SoilType = x.SoilType,
                        LandSizeInAcres = x.LandSizeInAcres,
                        SowingDate = x.SowingDate,
                        Status = x.Status,
                        ActivityId = x.ActivityId
                    }).FirstOrDefault();
        }

        public static ICollection<DomainEntities.WorkflowSeason> GetOpenWorkflowSeasons()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.WorkflowSeasons.Where(x => x.IsOpen && x.MaxAgreementsPerEntity > 0)
                .Select(x => new DomainEntities.WorkflowSeason()
                {
                    Id = x.Id,
                    SeasonName = x.SeasonName,
                    TypeName = x.TypeName,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsOpen = x.IsOpen,
                    ReferenceId = x.ReferenceId,
                    Description = x.Description,
                    MaxAgreementsPerEntity = x.MaxAgreementsPerEntity
                }).ToList();
        }

        public static ICollection<DomainEntities.EntityCrop> GetEntityCrops(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityCrops.Where(x => x.EntityId == entityId)
                    .Select(x => new DomainEntities.EntityCrop()
                    {
                        Id = x.Id,
                        EntityId = x.EntityId,
                        CropName = x.CropName
                    }).ToList();
        }

        public static DomainEntities.Entity GetSingleEntity(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Entities.Where(x => x.Id == entityId)
                                        .Select(e =>
                                                         new DomainEntities.Entity
                                                         {
                                                             Id = e.Id,
                                                             EmployeeId = e.EmployeeId,
                                                             DayId = e.DayId,
                                                             HQCode = e.HQCode,
                                                             EmployeeCode = e.TenantEmployee.EmployeeCode,
                                                             EmployeeName = e.TenantEmployee.Name,
                                                             AtBusiness = e.AtBusiness,
                                                             //Consent = e.Consent, //swetha made changes on 24-11
                                                             EntityType = e.EntityType,
                                                             EntityName = e.EntityName,
                                                             EntityDate = e.EntityDate,
                                                             Address = e.Address,
                                                             City = e.City,
                                                             State = e.State,
                                                             Pincode = e.Pincode,
                                                             LandSize = e.LandSize,
                                                             Latitude = e.Latitude,
                                                             Longitude = e.Longitude,
                                                             SqliteEntityId = e.SqliteEntityId,
                                                             ContactCount = e.ContactCount,
                                                             CropCount = e.CropCount,
                                                             ImageCount = e.ImageCount,
                                                             AgreementCount = e.EntityAgreements.Count, // e.AgreementCount,
                                                             UniqueIdType = e.UniqueIdType,
                                                             UniqueId = e.UniqueId,
                                                             TaxId = e.TaxId,
                                                             FatherHusbandName = e.FatherHusbandName,
                                                             HQName = e.HQName,
                                                             TerritoryCode = e.TerritoryCode,
                                                             TerritoryName = e.TerritoryName,
                                                             //MajorCrop = e.MajorCrop,
                                                             //LastCrop = e.LastCrop,
                                                             //WaterSource = e.WaterSource,
                                                             //SoilType = e.SoilType,
                                                             //SowingType = e.SowingType,
                                                             //SowingDate = e.SowingDate,
                                                             EntityNumber = e.EntityNumber,
                                                             IsActive = e.IsActive
                                                         }).FirstOrDefault();
        }

        public static IEnumerable<CustomerData> GetCustomerTotalByHQ(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from c in orm.Customers
                    join h in orm.OfficeHierarchies.Where(x => x.TenantId == tenantId && x.IsActive)
                                    on c.HQCode equals h.HQCode
                    select new
                    {
                        ZoneCode = h.ZoneCode,
                        AreaCode = h.AreaCode,
                        TerritoryCode = h.TerritoryCode,
                        HQCode = h.HQCode,
                        Outstanding = c.Outstanding,
                        LongOutstanding = c.LongOutstanding,
                        Target = c.Target
                    }).GroupBy(t => t.HQCode)
                    .Select(t => new CustomerData()
                    {
                        ZoneCode = t.FirstOrDefault().ZoneCode,
                        AreaCode = t.FirstOrDefault().AreaCode,
                        TerritoryCode = t.FirstOrDefault().TerritoryCode,
                        HQCode = t.Key,
                        CustomerCount = t.Count(),
                        TotalOutstanding = t.Sum(u => u.Outstanding),
                        TotalLongOutstanding = t.Sum(u => u.LongOutstanding),
                        TotalTarget = t.Sum(u => u.Target)
                    }).ToList();
        }

        public static DomainEntities.AdvanceRequest GetSingleAdvanceRequest(long id)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return (from a in orm.AdvanceRequests.Where(x => x.Id == id)
                    select new DomainEntities.AdvanceRequest
                    {
                        EntityId = a.EntityId,
                        EntityAgreementId = a.EntityAgreementId,
                        EntityName = a.EntityAgreement.Entity.EntityName,
                        AgreementNumber = a.EntityAgreement.AgreementNumber,
                        Crop = a.EntityAgreement.WorkflowSeason.TypeName,
                        Amount = a.Amount,
                        AdvanceRequestDate = a.AdvanceRequestDate,
                        RequestNotes = a.RequestNotes,
                        AmountApproved = a.AmountApproved,
                        ApproveNotes = a.ApproveNotes,
                        ReviewDate = a.ReviewDate,
                        ReviewedBy = a.ReviewedBy
                    }).FirstOrDefault();
        }

        public static void SaveAdvanceRequestReview(DomainEntities.AdvanceRequest ar, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var rec = orm.AdvanceRequests.Find(ar.Id);

            if (rec == null)
            {
                throw new ArgumentException("Invalid Advance Request");
            }

            rec.AmountApproved = ar.AmountApproved;
            rec.ApproveNotes = ar.ApproveNotes;
            rec.UpdatedBy = currentUser;
            rec.DateUpdated = DateTime.UtcNow;
            rec.ReviewedBy = currentUser;
            rec.ReviewDate = DateTime.UtcNow;
            rec.Status = ar.AdvanceRequestStatus;

            var entityName = "AdvanceRequest";
            var primaryKey = ar.Id.ToString();

            // Detect changes and Log it here.
            var modifiedEntity = orm.ChangeTracker.Entries<DBModel.AdvanceRequest>()
                .Where(x => x.State == EntityState.Modified)
                .FirstOrDefault();

            if (modifiedEntity != null)
            {
                foreach (var prop in modifiedEntity.OriginalValues.PropertyNames)
                {
                    try
                    {
                        var ov = modifiedEntity.OriginalValues[prop]?.ToString() ?? null;
                        var nv = modifiedEntity.CurrentValues[prop]?.ToString() ?? null;
                        if (ov != nv)
                        {
                            orm.Audits.Add(new DBModel.Audit()
                            {
                                TableName = entityName,
                                PrimaryKey = primaryKey,
                                FieldName = prop,
                                OldValue = ov,
                                NewValue = nv,
                                Timestamp = DateTime.UtcNow,
                                User = currentUser
                            });
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            try
            {
                orm.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"Advance Request Data not saved for Entity: {ar.EntityName}; Id: {ar.Id};";
                LogError($"{nameof(SaveAdvanceRequestReview)}", errorText, logSnip);
            }
        }

        public static DomainEntities.Payment GetPayment(long paymentId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from p in orm.Payments
                    join c in orm.Customers on p.CustomerCode equals c.CustomerCode into temp
                    where p.Id == paymentId
                    from t in temp.DefaultIfEmpty()

                    select new DomainEntities.Payment
                    {
                        Id = p.Id,
                        EmployeeId = p.EmployeeId,
                        EmployeeCode = p.TenantEmployee.EmployeeCode,
                        EmployeeName = p.TenantEmployee.Name,
                        DayId = p.DayId,
                        TotalAmount = p.TotalAmount,
                        CustomerCode = p.CustomerCode,
                        CustomerName = (t == null) ? String.Empty : t.Name,
                        HQCode = (t == null) ? "" : t.HQCode,
                        Comment = p.Comment,
                        DateCreated = p.DateCreated,
                        DateUpdated = p.DateUpdated,
                        ImageCount = p.ImageCount,
                        PaymentDate = p.PaymentDate,
                        PaymentType = p.PaymentType,
                        SqlitePaymentId = p.SqlitePaymentId,
                        IsApproved = p.IsApproved,
                        ApprovedAmt = p.ApproveAmount,
                        ApprovedBy = p.ApprovedBy,
                        ApprovedDate = p.ApproveDate,
                        ApproveComments = p.ApproveNotes,
                        ApproveRef = p.ApproveRef
                    }
                    ).FirstOrDefault();
        }

        public static bool SaveEmployeeConfigData(ConfigEmployeeModel saveModel)
        {
            var orm = DBLayer.GetOrm;
            var te = orm.TenantEmployees.Find(saveModel.EmployeeId);
            if (te == null)
            {
                return false;
            }

            te.ActivityPageName = saveModel.ActivityPageName;
            te.AutoUploadFromPhone = saveModel.AutoUploadFromPhone;
            te.LocationFromType = saveModel.LocationFromType;
            te.EnhancedDebugEnabled = saveModel.EnhancedDebugEnabled;
            te.TestFeatureEnabled = saveModel.TestFeatureEnabled;
            te.VoiceFeatureEnabled = saveModel.VoiceFeatureEnabled;
            te.ExecAppDetailLevel = saveModel.ExecAppDetailLevel;
            orm.SaveChanges();
            return true;
        }

        public static ICollection<DomainEntities.Expense> GetExpenseDataEORP(SearchCriteria searchCriteria)
        {
            DateTime justStartDatePart = new DateTime(searchCriteria.DateFrom.Year, searchCriteria.DateFrom.Month, searchCriteria.DateFrom.Day);
            DateTime justEndDatePart = new DateTime(searchCriteria.DateTo.Year, searchCriteria.DateTo.Month, searchCriteria.DateTo.Day);

            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.Expenses
                .Where(x => x.Day.DATE >= justStartDatePart && x.Day.DATE <= justEndDatePart)
                .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                (ex, sp) =>
                 new DomainEntities.Expense()
                 {
                     Id = ex.Id,
                     EmployeeId = ex.EmployeeId,
                     DayId = ex.DayId,
                     TotalAmount = ex.TotalAmount,
                     IsZoneApproved = ex.IsZoneApproved,
                     IsAreaApproved = ex.IsAreaApproved,
                     IsTerritoryApproved = ex.IsTerritoryApproved,
                     EmployeeName = ex.TenantEmployee.Name,
                     ExpenseDate = ex.Day.DATE,
                     StaffCode = sp.StaffCode
                 }).ToList();
        }

        public static void CreateSalesPersonData(SalesPersonModel es)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.SalesPerson sp = orm.SalesPersons.FirstOrDefault(x => x.StaffCode == es.StaffCode);

            if (sp != null)
            {
                throw new ArgumentException("Employee code already exist.");
            }

            sp = new DBModel.SalesPerson()
            {
                Name = es.Name,
                Phone = es.Phone,
                StaffCode = es.StaffCode,
                HQCode = es.HQCode,
                Grade = es.Grade,
                IsActive = es.IsActive,
                Department = es.Department,
                Designation = es.Designation,
                OwnershipType = es.Ownership,
                VehicleType = es.VehicleType,
                FuelType = es.FuelType,
                VehicleNumber = es.VehicleNumber,
                BusinessRole = es.BusinessRole,
                OverridePrivateVehicleRatePerKM = es.OverridePrivateVehicleRatePerKM,
                TwoWheelerRatePerKM = es.TwoWheelerRatePerKM,
                FourWheelerRatePerKM = es.FourWheelerRatePerKM
            };

            orm.SalesPersons.Add(sp);
            orm.SaveChanges();
        }

        public static void SaveSalesPersonData(SalesPersonModel es)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.SalesPerson sp = orm.SalesPersons.FirstOrDefault(x => x.StaffCode == es.StaffCode);

            if (sp == null)
            {
                throw new ArgumentException("Invalid Employee code");
            }

            sp.Name = es.Name;
            sp.Phone = es.Phone;
            sp.StaffCode = es.StaffCode;
            sp.HQCode = es.HQCode;
            sp.Grade = es.Grade;
            sp.IsActive = es.IsActive;
            sp.Department = es.Department;
            sp.Designation = es.Designation;
            sp.OwnershipType = es.Ownership;
            sp.VehicleType = es.VehicleType;
            sp.FuelType = es.FuelType;
            sp.VehicleNumber = es.VehicleNumber;
            sp.BusinessRole = es.BusinessRole;
            sp.OverridePrivateVehicleRatePerKM = es.OverridePrivateVehicleRatePerKM;
            sp.TwoWheelerRatePerKM = es.TwoWheelerRatePerKM;
            sp.FourWheelerRatePerKM = es.FourWheelerRatePerKM;

            orm.SaveChanges();
        }
         
        public static void SaveEntityData(DomainEntities.Entity inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            
            var sp = orm.Entities.FirstOrDefault(x => x.Id == inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid Entity Id");
            }

            sp.EntityName = inputRec.EntityName;
            sp.LandSize = inputRec.LandSize;
            sp.TaxId = inputRec.TaxId;
            sp.UniqueId = inputRec.UniqueId;
            sp.Address = inputRec.Address;
            sp.City = inputRec.City;
            sp.State = inputRec.State;
            sp.Pincode = inputRec.Pincode;
            sp.Latitude = inputRec.Latitude;
            sp.Longitude = inputRec.Longitude;
            sp.HQCode = inputRec.HQCode;
            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;
            sp.AtBusiness = inputRec.AtBusiness;
            //sp.Consent = inputRec.Consent; //Swetha made the changes on 24-11
            sp.FatherHusbandName = inputRec.FatherHusbandName;
            sp.HQName = inputRec.HQName;
            sp.TerritoryCode = inputRec.TerritoryCode;
            sp.TerritoryName = inputRec.TerritoryName;
            sp.IsActive = inputRec.IsActive;

            orm.SaveChanges();
        }

        public static DBSaveStatus SaveDWSData(DomainEntities.DWS inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.DWS.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid DWS Id");
            }

            var strTagRec = sp.STRTag;
            if (strTagRec.CyclicCount != inputRec.StrTagCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            DateTime utcNow = DateTime.UtcNow;

            // Create an Audit Record first
            orm.DWSAudits.Add(new DBModel.DWSAudit()
            {
                STRTagId = sp.STRTagId,
                STRId = sp.STRId,
                DWSId = sp.Id,
                DWSNumber = sp.DWSNumber,
                DWSDate = sp.DWSDate,
                BagCount = sp.BagCount,
                FilledBagsWeightKg = sp.FilledBagsWeightKg,
                EmptyBagsWeightKg = sp.EmptyBagsWeightKg,
                EntityId = sp.EntityId,
                AgreementId = sp.AgreementId,
                Agreement = sp.Agreement,
                EntityWorkFlowDetailId = sp.EntityWorkFlowDetailId,
                TypeName = sp.TypeName,
                TagName = sp.TagName,
                CreatedBy = inputRec.CurrentUser,
                DateCreated = utcNow
            });

            sp.DWSNumber = inputRec.DWSNumber;
            sp.DWSDate = inputRec.DWSDate;
            sp.BagCount = inputRec.BagCount;
            sp.FilledBagsWeightKg = inputRec.FilledBagsWeightKg;
            sp.EmptyBagsWeightKg = inputRec.EmptyBagsWeightKg;
            sp.Comments = inputRec.Comments;
            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = utcNow;

            if (inputRec.ChangeAgreementDetails)
            {
                sp.EntityId = inputRec.EntityId;
                sp.AgreementId = inputRec.AgreementId;
                sp.Agreement = inputRec.Agreement;
                sp.EntityWorkFlowDetailId = inputRec.EntityWorkFlowDetailId;
                sp.TypeName = inputRec.TypeName;
                sp.TagName = inputRec.TagName;
            }

            sp.CyclicCount++;

            // update cyclic count in parent record
            strTagRec.CyclicCount++;

            orm.SaveChanges();
            return DBSaveStatus.Success;
        }

        public static DBSaveStatus SaveDWSApprovedWeightData(DomainEntities.DWS inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.DWS.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid DWS Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.CyclicCount++;
            sp.Comments = inputRec.Comments;
            sp.SiloDeductWtOverride = inputRec.SiloDeductWtOverride;
            sp.WtApprovedBy = inputRec.CurrentUser;
            sp.UpdatedBy = inputRec.CurrentUser;
            sp.WtApprovedDate = DateTime.UtcNow;
            sp.DateUpdated = DateTime.UtcNow;

            sp.NetPayableWt = inputRec.NetPayableWt;
            sp.GoodsPrice = inputRec.GoodsPrice;
            sp.NetPayable = inputRec.NetPayable;

            sp.Status = inputRec.Status;

            orm.SaveChanges();
            return DBSaveStatus.Success;
        }

        public static DBSaveStatus SaveDWSApprovedAmountData(DomainEntities.DWS inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.DWS.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid DWS Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.CyclicCount++;
            sp.Comments = inputRec.Comments;
            sp.DeductAmount = inputRec.DeductAmount;
            sp.NetPayable = inputRec.NetPayable;

            sp.AmountApprovedBy = inputRec.CurrentUser;
            sp.UpdatedBy = inputRec.CurrentUser;
            sp.AmountApprovedDate = DateTime.UtcNow;
            sp.DateUpdated = DateTime.UtcNow;

            sp.BankAccountName = inputRec.BankAccountName;
            sp.BankName = inputRec.BankName;
            sp.BankAccount = inputRec.BankAccount;
            sp.BankIFSC = inputRec.BankIFSC;
            sp.BankBranch = inputRec.BankBranch;

            sp.Status = inputRec.Status;

            orm.SaveChanges();
            return DBSaveStatus.Success;
        }

        public static void RecalculateSTRTotals(long strId, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ReCalculateSTRTotals(strId, currentUser);
        }

        public static DBSaveStatus SaveSTRData(DomainEntities.STR inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.STRs.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid STR Id");
            }

            var strTagRec = orm.STRTags.Find(inputRec.STRTagId);
            if ((strTagRec?.CyclicCount ?? 0) != inputRec.StrTagCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.EmployeeId = inputRec.EmployeeId;
            sp.VehicleNumber = inputRec.VehicleNumber;
            sp.StartOdometer = inputRec.StartOdometer;
            sp.EndOdometer = inputRec.EndOdometer;
            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;

            strTagRec.CyclicCount++;

            orm.SaveChanges();
            return DBSaveStatus.Success;
        }

        public static void ReAssignSTR(long strId, long fromSTRTagId, long toStrTagId, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ReAssignSTRNumber(strId, fromSTRTagId, toStrTagId, currentUser);
        }

        public static void ReAssignDWS(long dwsId, long fromSTRTagId, long toStrTagId, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ReAssignDwsSTRNumber(dwsId, fromSTRTagId, toStrTagId, currentUser);
        }

        public static DBSaveStatus SaveSTRWeightData(DomainEntities.STRWeight inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.STRWeights.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid STR Weight Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.STRNumber = inputRec.STRNumber;
            sp.STRDate = inputRec.STRDate;
            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;

            sp.EntryWeight = inputRec.EntryWeight;
            sp.ExitWeight = inputRec.ExitWeight;
            sp.SiloNumber = inputRec.SiloNumber;
            sp.SiloIncharge = inputRec.SiloIncharge;
            sp.UnloadingIncharge = inputRec.UnloadingIncharge;
            sp.ExitOdometer = inputRec.ExitOdometer;
            sp.BagCount = inputRec.BagCount;
            sp.Notes = inputRec.Notes;
            sp.Status = inputRec.Status;
            sp.DeductionPercent = inputRec.DeductionPercent;

            sp.DWSCount = inputRec.DWSCount;
            sp.VehicleNumber = inputRec.VehicleNumber;

            sp.CyclicCount++;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus UpdateSTRWeightStatus(long id, long cyclicCount, string status, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.STRWeights.Find(id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid STR Weight Id");
            }

            if (sp.CyclicCount != cyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.UpdatedBy = currentUser;
            sp.CyclicCount++;
            sp.Status = status;
            sp.DateUpdated = DateTime.UtcNow;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus SaveIssueReturnData(DomainEntities.IssueReturn inputRec)
        {
            var currentUtcTime = DateTime.UtcNow;
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.IssueReturn sp = orm.IssueReturns.Find(inputRec.Id);
            if (sp == null)
            {
                throw new ArgumentException("Invalid Issue Return Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.AppliedTransactionType = inputRec.AppliedTransactionType;
            sp.TransactionDate = inputRec.TransactionDate;
            sp.AppliedQuantity = inputRec.AppliedQuantity;
            sp.Status = inputRec.Status;
            sp.Comments = inputRec.Comments;
            sp.AppliedItemMasterId = inputRec.AppliedItemMasterId;
            sp.AppliedItemRate = inputRec.AppliedItemRate;

            sp.DateUpdated = currentUtcTime;
            sp.UpdatedBy = inputRec.CurrentUser;
            sp.CyclicCount = inputRec.CyclicCount + 1;

            // retreive balance Record
            if (inputRec.IsApproved)
            {
                string employeeCode = sp.TenantEmployee.EmployeeCode;
                if (inputRec.IsIssueItem)
                {
                    var balanceRec = orm.StockBalances.Find(inputRec.StockBalanceRecId);
                    if (balanceRec == null || balanceRec.StockQuantity < inputRec.AppliedQuantity)
                    {
                        return DBSaveStatus.CyclicCheckFail;
                    }

                    balanceRec.StockQuantity -= inputRec.AppliedQuantity;
                    balanceRec.CyclicCount++;
                    balanceRec.UpdatedBy = inputRec.CurrentUser;
                    balanceRec.DateUpdated = currentUtcTime;
                }
                else
                {
                    // for return the balance record may or may not exist;
                    if (inputRec.StockBalanceRecId > 0)
                    {
                        var balanceRec = orm.StockBalances.Find(inputRec.StockBalanceRecId);
                        if (balanceRec == null)
                        {
                            return DBSaveStatus.CyclicCheckFail;
                        }

                        balanceRec.StockQuantity += inputRec.AppliedQuantity;
                        balanceRec.CyclicCount++;
                        balanceRec.UpdatedBy = inputRec.CurrentUser;
                        balanceRec.DateUpdated = currentUtcTime;
                    }
                    else
                    {
                        orm.StockBalances.Add(new DBModel.StockBalance()
                        {
                            CyclicCount = 1,
                            UpdatedBy = inputRec.CurrentUser,
                            CreatedBy = inputRec.CurrentUser,
                            DateCreated = currentUtcTime,
                            DateUpdated = currentUtcTime,
                            StockQuantity = inputRec.AppliedQuantity,
                            ZoneCode = "",
                            AreaCode = "",
                            TerritoryCode = "",
                            HQCode = "",
                            StaffCode = employeeCode,
                            ItemMasterId = sp.AppliedItemMasterId
                        });
                    }
                }

                // create ledger record
                var stockLedgerRec = new DBModel.StockLedger()
                {
                    TransactionDate = inputRec.CurrentIstTime,
                    ItemMasterId = sp.AppliedItemMasterId,
                    ReferenceNo = $"Slip # {sp.SlipNumber}",
                    LineNumber = 0,
                    IssueQuantity = (inputRec.IsIssueItem) ? inputRec.AppliedQuantity : 0,
                    ReceiveQuantity = (inputRec.IsIssueItem) ? 0 : inputRec.AppliedQuantity,
                    ZoneCode = "",
                    AreaCode = "",
                    TerritoryCode = "",
                    HQCode = "",
                    StaffCode = employeeCode,
                    CyclicCount = 1,
                    UpdatedBy = inputRec.CurrentUser,
                    CreatedBy = inputRec.CurrentUser,
                    DateCreated = currentUtcTime,
                    DateUpdated = currentUtcTime
                };
                orm.StockLedgers.Add(stockLedgerRec);
            }

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static void CreateSTRWeightData(DomainEntities.STRWeight inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.STRWeights.Add(new DBModel.STRWeight()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                STRNumber = inputRec.STRNumber,
                STRDate = inputRec.STRDate,
                Status = inputRec.Status,

                EntryWeight = inputRec.EntryWeight,
                ExitWeight = inputRec.ExitWeight,
                SiloNumber = inputRec.SiloNumber,
                SiloIncharge = inputRec.SiloIncharge,
                UnloadingIncharge = inputRec.UnloadingIncharge,
                ExitOdometer = inputRec.ExitOdometer,
                BagCount = inputRec.BagCount,
                Notes = inputRec.Notes,
                DeductionPercent = inputRec.DeductionPercent,

                DWSCount = inputRec.DWSCount,
                VehicleNumber = inputRec.VehicleNumber,

                CyclicCount = 1
            });

            try
            {
                orm.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus SaveSTRTagData(DomainEntities.STRTag inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.STRTags.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid STR Tag Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.STRNumber = inputRec.STRNumber;
            sp.STRDate = inputRec.STRDate;
            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;
            sp.Status = inputRec.Status;
            sp.CyclicCount++;

            orm.SaveChanges();
            return DBSaveStatus.Success;
        }

        public static void CreateSTRTagData(DomainEntities.STRTag inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.STRTags.Add(new DBModel.STRTag()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                STRNumber = inputRec.STRNumber,
                STRDate = inputRec.STRDate,
                Status = inputRec.Status
            });

            try
            {
                orm.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus CreateSTR(DomainEntities.STR inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var strTagRec = orm.STRTags.Find(inputRec.STRTagId);

            if (strTagRec.CyclicCount != inputRec.StrTagCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            orm.STRs.Add(new DBModel.STR()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,

                STRTagId = inputRec.STRTagId,
                EmployeeId = inputRec.EmployeeId,
                VehicleNumber = inputRec.VehicleNumber,
                DriverName = inputRec.DriverName,
                DriverPhone = inputRec.DriverPhone,
                DWSCount = inputRec.DWSCount,
                BagCount = inputRec.BagCount,
                GrossWeight = inputRec.GrossWeight,
                NetWeight = inputRec.NetWeight,
                StartOdometer = inputRec.StartOdometer,
                EndOdometer = inputRec.EndOdometer,
                IsNew = inputRec.IsNew,
                IsTransferred = inputRec.IsTransferred,
                TransfereeName = inputRec.TransfereeName,
                TransfereePhone = inputRec.TransfereePhone,
                ImageCount = inputRec.ImageCount,
                ActivityId = inputRec.ActivityId,
                ActivityId2 = inputRec.ActivityId2,
                BatchId = 0,
                SqliteSTRId = 0,
                Status = inputRec.Status
            });

            strTagRec.CyclicCount++;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus CreateDWS(DomainEntities.DWS inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var strTagRec = orm.STRTags.Find(inputRec.STRTagId);
            if (strTagRec.CyclicCount != inputRec.StrTagCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            orm.DWS.Add(new DBModel.DW()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,

                STRTagId = inputRec.STRTagId,
                STRId = inputRec.STRId,
                DWSNumber = inputRec.DWSNumber,
                DWSDate = inputRec.DWSDate,
                BagCount = inputRec.BagCount,
                FilledBagsWeightKg = inputRec.FilledBagsWeightKg,
                EmptyBagsWeightKg = inputRec.EmptyBagsWeightKg,
                EntityId = inputRec.EntityId,
                AgreementId = inputRec.AgreementId,
                Agreement = inputRec.Agreement,
                EntityWorkFlowDetailId = inputRec.EntityWorkFlowDetailId,
                TypeName = inputRec.TypeName,
                TagName = inputRec.TagName,
                ActivityId = inputRec.ActivityId,
                SqliteSTRDWSId = 0,
                Comments = inputRec.Comments,
                Status = inputRec.Status,
                CyclicCount = inputRec.CyclicCount
            });

            strTagRec.CyclicCount++;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        /// <summary>
        /// Get Expense Data for a date range grouped by date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static ICollection<EORPData> GetExpenseData(DateTime startDate, DateTime endDate, bool IsSuperAdmin, string CurrentUserStaffCode)
        {
            DateTime justStartDatePart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            DateTime justEndDatePart = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.Expenses.Where(x => x.Day.DATE >= justStartDatePart && x.Day.DATE <= justEndDatePart)
               .GroupBy(x => x.Day.DATE)
               .OrderBy(x => x.Key)
               .Select(y => new EORPData
               {
                   Date = y.Key,
                   TotalAmountForDay = y.Sum(z => z.TotalAmount),
                   TotalItemCountForDay = y.Count()
               }).ToList();
        }

        public static void ToggleExecAppAccess(long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var record = orm.TenantEmployees.Find(employeeId);
            if (record != null)
            {
                record.ExecAppAccess = !record.ExecAppAccess;
                record.DateUpdated = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void CreateFeatureControl(string userName)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var rec = orm.FeatureControls.FirstOrDefault(x => x.UserName == userName);
            if (rec == null)
            {
                DBModel.FeatureControl fc = new DBModel.FeatureControl()
                {
                    UserName = userName,
                    ActivityFeature = false,
                    OrderFeature = false,
                    PaymentFeature = false,
                    OrderReturnFeature = false,
                    ExpenseFeature = false,
                    IssueReturnFeature = false,
                    EntityFeature = false,
                    ExpenseReportFeature = false,
                    AttendanceReportFeature = false,
                    AttendanceSummaryReportFeature = false,
                    KPIFeature = false,
                    SalesPersonFeature = false,
                    CustomerFeature = false,
                    ProductFeature = false,
                    CrmUserFeature = false,
                    AssignmentFeature = false,
                    UploadDataFeature = false,
                    OfficeHierarchyFeature = false,
                    EmployeeExpenseReport = false,
                    DistanceReport = false,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    SecurityContextUser = String.Empty
                };
                orm.FeatureControls.Add(fc);
                orm.SaveChanges();
            }
        }

        public static DBSaveStatus MarkDWSAsPaid(IEnumerable<DWS> dwsRecs)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            foreach (var inputRec in dwsRecs)
            {
                var dbRec = orm.DWS.Find(inputRec.Id);
                if (dbRec == null)
                {
                    return DBSaveStatus.RecordNotFound;
                }

                if (dbRec.CyclicCount != inputRec.CyclicCount)
                {
                    return DBSaveStatus.CyclicCheckFail;
                }

                dbRec.PaymentReference = inputRec.PaymentReference;
                dbRec.DateUpdated = DateTime.UtcNow;
                dbRec.UpdatedBy = inputRec.CurrentUser;
                dbRec.Status = inputRec.Status;
                dbRec.PaidBy = inputRec.CurrentUser;
                dbRec.PaidDate = DateTime.UtcNow;
                dbRec.CyclicCount++;
            }

            orm.SaveChanges();
            return DBSaveStatus.Success;
        }

        public static void CreateDWSPaymentReference(DomainEntities.DWSPaymentReference dwsPaymentReference)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.DWSPaymentReferences.Add(new DBModel.DWSPaymentReference()
            {
                Comments = dwsPaymentReference.Comments,
                PaymentReference = dwsPaymentReference.PaymentReference,
                TotalNetPayable = dwsPaymentReference.TotalNetPayable,
                DWSCount = dwsPaymentReference.DWSCount,
                DWSNumbers = dwsPaymentReference.DWSNumbers,

                CreatedBy = dwsPaymentReference.CurrentUser,
                UpdatedBy = dwsPaymentReference.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,

                AccountNumber = dwsPaymentReference.AccountNumber,
                AccountName = dwsPaymentReference.AccountName,
                AccountAddress = dwsPaymentReference.AccountAddress,
                AccountEmail = dwsPaymentReference.AccountEmail,
                PaymentType = dwsPaymentReference.PaymentType,
                SenderInfo = dwsPaymentReference.SenderInfo,
                LocalTimeStamp = dwsPaymentReference.LocalTimeStamp
            });

            orm.SaveChanges();
        }

        public static ICollection<DomainEntities.DWSPaymentReference> GetPaymentReferences(PaymentReferenceFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.DWSPaymentReference> items = from e in orm.DWSPaymentReferences
                                                                   select new DomainEntities.DWSPaymentReference
                                                                   {
                                                                       Id = e.Id,
                                                                       PaymentReference = e.PaymentReference,
                                                                       Comments = e.Comments,
                                                                       TotalNetPayable = e.TotalNetPayable,
                                                                       DWSNumbers = e.DWSNumbers,
                                                                       DWSCount = e.DWSCount,
                                                                       DateCreated = e.DateCreated,
                                                                       CreatedBy = e.CreatedBy,

                                                                       AccountNumber = e.AccountNumber,
                                                                       AccountName = e.AccountName,
                                                                       AccountAddress = e.AccountAddress,
                                                                       AccountEmail = e.AccountEmail,
                                                                       PaymentType = e.PaymentType,
                                                                       SenderInfo = e.SenderInfo,
                                                                       LocalTimeStamp = e.LocalTimeStamp
                                                                   };
            if (searchCriteria.ApplyPaymentReferenceFilter)
            {
                if (searchCriteria.IsExactReferenceMatch)
                {
                    items = items.Where(x => x.PaymentReference == searchCriteria.PaymentReference);
                }
                else
                {
                    items = items.Where(x => x.PaymentReference.Contains(searchCriteria.PaymentReference));
                }
            }

            if (searchCriteria.ApplyDateFilter)
            {
                searchCriteria.DateTo = searchCriteria.DateTo.AddDays(1);
                // this is because - LocalTimeStamp has time as well, whereas DateTo is only date
                // so when we search for dates upto 2020-05-28, records created at time
                // 2020-05-28 17:00:00 should also get included.
                items = items.Where(x => x.LocalTimeStamp >= searchCriteria.DateFrom &&
                                                             x.LocalTimeStamp < searchCriteria.DateTo);
            }

            return items.OrderBy(x => x.PaymentReference).ToList();
        }

        public static bool AcquireParseLockOnExcelUpload(long id)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var rec = orm.ExcelUploadStatus.Find(id);

                if (rec == null || rec.IsLocked || rec.IsParsed)
                {
                    return false;
                }

                rec.IsLocked = true;
                rec.LockTimestamp = DateTime.UtcNow;
                orm.SaveChanges();
                return true;
            }
        }

        public static bool ReleaseParseLockOnExcelUpload(long id)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var rec = orm.ExcelUploadStatus.Find(id);

                if (rec == null)
                {
                    return false;
                }

                rec.IsLocked = false;
                rec.LockTimestamp = DateTime.UtcNow;
                orm.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Get Order data for a date range grouped by order date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static ICollection<EORPData> GetOrderData(SearchCriteria searchCriteria)
        {
            DateTime justStartDatePart = new DateTime(searchCriteria.DateFrom.Year, searchCriteria.DateFrom.Month, searchCriteria.DateFrom.Day);
            DateTime justEndDatePart = new DateTime(searchCriteria.DateTo.Year, searchCriteria.DateTo.Month, searchCriteria.DateTo.Day);

            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.Order> orders = from o in orm.Orders
                                                      join c in orm.Customers on o.CustomerCode equals c.CustomerCode into temp
                                                      from t in temp.DefaultIfEmpty()
                                                      where o.OrderDate >= justStartDatePart && o.OrderDate <= justEndDatePart
                                                      select new DomainEntities.Order()
                                                      {
                                                          Id = o.Id,
                                                          OrderDate = o.OrderDate,
                                                          TotalAmount = o.TotalAmount,
                                                          HQCode = (t == null) ? string.Empty : t.HQCode,
                                                      };

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                orders = orders.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                orders = orders.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return orders.GroupBy(x => x.OrderDate)
                    .OrderBy(x => x.Key)
                    .Select(y => new EORPData
                    {
                        Date = y.Key,
                        TotalAmountForDay = y.Sum(z => z.TotalAmount),
                        TotalItemCountForDay = y.Count()
                    }).ToList();
        }

        public static void TogglePhoneLog(long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var record = orm.TenantEmployees.Find(employeeId);
            if (record != null)
            {
                record.SendLogFromPhone = !record.SendLogFromPhone;
                record.DateUpdated = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        /// <summary>
        /// Get Return Order Data for a date range grouped by return order date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static ICollection<EORPData> GetReturnOrderData(SearchCriteria searchCriteria)
        {
            DateTime justStartDatePart = new DateTime(searchCriteria.DateFrom.Year, searchCriteria.DateFrom.Month, searchCriteria.DateFrom.Day);
            DateTime justEndDatePart = new DateTime(searchCriteria.DateTo.Year, searchCriteria.DateTo.Month, searchCriteria.DateTo.Day);

            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.Return> returns = from r in orm.ReturnOrders
                                                        join c in orm.Customers on r.CustomerCode equals c.CustomerCode into temp
                                                        from t in temp.DefaultIfEmpty()
                                                        where r.ReturnOrderDate >= justStartDatePart && r.ReturnOrderDate <= justEndDatePart
                                                        select new DomainEntities.Return
                                                        {
                                                            Id = r.Id,
                                                            ReturnDate = r.ReturnOrderDate,
                                                            TotalAmount = r.TotalAmount,
                                                            HQCode = (t == null) ? string.Empty : t.HQCode,
                                                        };

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                returns = returns.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                returns = returns.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return returns.GroupBy(x => x.ReturnDate)
                    .OrderBy(x => x.Key)
                    .Select(y => new EORPData
                    {
                        Date = y.Key,
                        TotalAmountForDay = y.Sum(z => z.TotalAmount),
                        TotalItemCountForDay = y.Count()
                    }).ToList();
        }

        public static void SaveExecAppImeiRec(DomainEntities.ExecAppImei inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.ExecAppImeis.Find(inputRec.Id);
            if (rec != null)
            {
                rec.Comment = inputRec.Comment;
                rec.EffectiveDate = inputRec.EffectiveDate;
                rec.ExpiryDate = inputRec.ExpiryDate;
                rec.IsSupportPerson = inputRec.IsSupportPerson;
                rec.EnableLog = inputRec.EnableLog;
                rec.ExecAppDetailLevel = inputRec.ExecAppDetailLevel;
                orm.SaveChanges();
            }
        }

        public static long AddExecAppImeiRec(DomainEntities.ExecAppImei inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.ExecAppImei dbRec = new DBModel.ExecAppImei()
            {
                IMEINumber = inputRec.IMEINumber,
                Comment = inputRec.Comment,
                EffectiveDate = inputRec.EffectiveDate,
                ExpiryDate = inputRec.ExpiryDate,
                IsSupportPerson = inputRec.IsSupportPerson,
                EnableLog = inputRec.EnableLog,
                ExecAppDetailLevel = inputRec.ExecAppDetailLevel
            };

            orm.ExecAppImeis.Add(dbRec);
            orm.SaveChanges();

            return dbRec.Id;
        }

        /// <summary>
        /// Get Payment data for a date range grouped by payment date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static ICollection<EORPData> GetPaymentData(SearchCriteria searchCriteria)
        {
            DateTime justStartDatePart = new DateTime(searchCriteria.DateFrom.Year, searchCriteria.DateFrom.Month, searchCriteria.DateFrom.Day);
            DateTime justEndDatePart = new DateTime(searchCriteria.DateTo.Year, searchCriteria.DateTo.Month, searchCriteria.DateTo.Day);

            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.Payment> payments = from p in orm.Payments
                                                          join c in orm.Customers on p.CustomerCode equals c.CustomerCode into temp
                                                          from t in temp.DefaultIfEmpty()
                                                          where p.PaymentDate >= justStartDatePart && p.PaymentDate <= justEndDatePart
                                                          select new DomainEntities.Payment
                                                          {
                                                              Id = p.Id,
                                                              PaymentDate = p.PaymentDate,
                                                              TotalAmount = p.TotalAmount,
                                                              HQCode = (t == null) ? string.Empty : t.HQCode,
                                                          };

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                payments = payments.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                payments = payments.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return payments.GroupBy(x => x.PaymentDate)
                    .OrderBy(x => x.Key)
                    .Select(y => new EORPData
                    {
                        Date = y.Key,
                        TotalAmountForDay = y.Sum(z => z.TotalAmount),
                        TotalItemCountForDay = y.Count()
                    }).ToList();
        }

        /// <summary>
        /// Get Associations for staff code
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns></returns>
        public static ICollection<DomainEntities.OfficeHierarchy> GetDetailedAssociations(long tenantId, string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            ObjectResult<GetOfficeHierarchyForStaff_Result> resultSet = orm.GetOfficeHierarchyForStaff(tenantId, staffCode);
            return resultSet.Select(x => new DomainEntities.OfficeHierarchy()
            {
                ZoneCode = x.ZoneCode,
                ZoneName = x.ZoneName,
                AreaCode = x.AreaCode,
                AreaName = x.AreaName,
                TerritoryCode = x.TerritoryCode,
                TerritoryName = x.TerritoryName,
                HQCode = x.HQCode,
                HQName = x.HQName
            }).ToList();
        }

        public static IEnumerable<DomainEntities.OfficeHierarchyForAll> GetDetailedAssociationsForAll(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            ObjectResult<GetOfficeHierarchyForAll_Result> resultSet = orm.GetOfficeHierarchyForAll(tenantId);
            return resultSet.Select(x => new DomainEntities.OfficeHierarchyForAll()
            {
                StaffCode = x.StaffCode,
                ZoneCode = x.ZoneCode,
                ZoneName = x.ZoneName,
                AreaCode = x.AreaCode,
                AreaName = x.AreaName,
                TerritoryCode = x.TerritoryCode,
                TerritoryName = x.TerritoryName,
                HQCode = x.HQCode,
                HQName = x.HQName
            }).ToList();
        }

        public static ICollection<DomainEntities.TableSchema> GetTableSchema(string tableName)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            ObjectResult<TableSchema_Result> resultSet = orm.TableSchema(tableName);
            return resultSet.Select(x => new DomainEntities.TableSchema()
            {
                Position = x.Position,
                ColumnName = x.ColumnName,
                IsNullable = x.IsNullable,
                DataType = x.DataType,
                CharMaxLen = x.CharMaxLen,
                ColumnDefault = x.ColumnDefault
            }).ToList();
        }

        public static void SaveVirtualSuperAdminRights(DomainEntities.FeatureControl fc)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DBModel.FeatureControl rec = orm.FeatureControls.FirstOrDefault(x => x.UserName == fc.UserName);
            if (rec != null)
            {
                long recId = rec.Id;
                Utils.CopyProperties(fc, rec);
                rec.DateUpdated = DateTime.UtcNow;
                rec.Id = recId;  // to ensure it is not changed during copy operation

                orm.SaveChanges();
            }
        }

        public static void DeleteExecAppImei(long execAppImeiId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.ExecAppImeis.Find(execAppImeiId);
            if (rec != null)
            {
                orm.ExecAppImeis.Remove(rec);
                orm.SaveChanges();
            }
        }

        public static ICollection<DomainEntities.TenantSmsSchedule> GetTenantSmsSchedule()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TenantSMSSchedules.AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => new DomainEntities.TenantSmsSchedule()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    TenantName = x.Tenant.Name,
                    WeekDayName = x.WeekDayName,
                    SMSAt = x.SMSAt,
                    TenantSmsTypeId = x.TenantSmsTypeId,
                    SmsTypeName = x.TenantSmsType.TypeName
                })
                .ToList() // this toList is to finish off with db
                .OrderBy(x => x.TenantId)
                .ThenBy(x => x.SmsTypeName)
                .ThenBy(x => GetDayNumber(x.WeekDayName))
                .ThenBy(x => x.SMSAt)
                .ToList();
        }

        public static long AddAppVersion(DomainEntities.AppVersion inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.AppVersion dbRec = new DBModel.AppVersion()
            {
                Version = inputRec.Version,
                Comment = inputRec.Comment,
                EffectiveDate = inputRec.EffectiveDate,
                ExpiryDate = inputRec.ExpiryDate
            };

            orm.AppVersions.Add(dbRec);
            orm.SaveChanges();

            return dbRec.Id;
        }

        public static ICollection<DomainEntities.TenantWorkDay> GetTenantWorkDay()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TenantWorkDays.AsNoTracking()
                .Select(x => new DomainEntities.TenantWorkDay()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    TenantName = x.Tenant.Name,
                    WeekDayName = x.WeekDayName,
                    IsWorkingDay = x.IsWorkingDay
                })
                .ToList() // this toList is to finish off with db
                .OrderBy(x => x.TenantId)
                .ThenBy(x => GetDayNumber(x.WeekDayName))
                .ToList();
        }

        public static IEnumerable<DashboardGstRate> GetGstRate(GstRateFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            // orm.Database.Log = (x) => DBLayer.LogError("GetDashboardBankAccount", x, "");
            //orm.Database.Log = (x) => System.Diagnostics.Debug.WriteLine(x);

            IQueryable<DashboardGstRate> gst = from g in orm.GstRates
                                               where g.IsActive
                                               orderby g.GstCode, g.EffectiveStartDate
                                               select new DashboardGstRate()
                                               {
                                                   Id = g.Id,
                                                   GstCode = g.GstCode,
                                                   GstRate = g.GstRate1,
                                                   EffectiveStartDate = g.EffectiveStartDate,
                                                   EffectiveEndDate = g.EffectiveEndDate
                                               };

            if (searchCriteria?.ApplyGstCodeFilter ?? false)
            {
                gst = gst.Where(s => s.GstCode.Contains(searchCriteria.GstCode));
            }

            if (searchCriteria?.ApplyDateFilter ?? false)
            {
                gst = gst.Where(s => s.EffectiveStartDate <= searchCriteria.SearchDate && s.EffectiveEndDate >= searchCriteria.SearchDate);
            }

            return gst.ToList();
        }

        public static long CreateGSTRate(GstSaveRate gst)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                DBModel.GstRate gstRate = new DBModel.GstRate
                {
                    GstCode = gst.GstCode,
                    GstRate1 = gst.GstRate,
                    EffectiveStartDate = gst.EffectiveStartDate,
                    EffectiveEndDate = gst.EffectiveEndDate,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    CreatedBy = gst.CurrentStaffCode,
                    UpdatedBy = gst.CurrentStaffCode,
                    IsActive = true
                };
                orm.GstRates.Add(gstRate);
                orm.SaveChanges();
                return gstRate.Id;
            }
        }

        public static long UpdateGstRate(GstSaveRate gr)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                DBModel.GstRate gst = orm.GstRates.Find(gr.Id);
                if (gst?.IsActive ?? false)
                {
                    gst.GstRate1 = gr.GstRate;
                    gst.EffectiveStartDate = gr.EffectiveStartDate;
                    gst.EffectiveEndDate = gr.EffectiveEndDate;
                    gst.DateUpdated = DateTime.UtcNow;
                    gst.UpdatedBy = gr.CurrentStaffCode;
                    orm.SaveChanges();
                    return gr.Id;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static DashboardGstRate GetGstRateData(long id)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.GstRates.Where(x => x.Id == id && x.IsActive)
                            .Select(g => new DashboardGstRate()
                            {
                                Id = g.Id,
                                GstCode = g.GstCode,
                                GstRate = g.GstRate1,
                                EffectiveStartDate = g.EffectiveStartDate,
                                EffectiveEndDate = g.EffectiveEndDate
                            }).FirstOrDefault();
        }

        public static List<DashboardGstRate> GetOverlappingGSTRates(string gstCode, DateTime trnStartDate, DateTime trnEndDate)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                return orm.GstRates
                    .Where(db => gstCode == db.GstCode && db.IsActive &&
                    (db.EffectiveStartDate > trnEndDate || db.EffectiveEndDate < trnStartDate) == false)
                    .Select(g => new DashboardGstRate()
                    {
                        Id = g.Id,
                        GstCode = g.GstCode,
                        GstRate = g.GstRate1,
                        EffectiveStartDate = g.EffectiveStartDate,
                        EffectiveEndDate = g.EffectiveEndDate,
                    })
                    .ToList();
            }
        }

        public static void SaveAppVersion(DomainEntities.AppVersion inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.AppVersions.Find(inputRec.Id);
            if (rec != null)
            {
                rec.Comment = inputRec.Comment;
                rec.EffectiveDate = inputRec.EffectiveDate;
                rec.ExpiryDate = inputRec.ExpiryDate;

                orm.SaveChanges();
            }
        }

        private static int GetDayNumber(string weekDayName)
        {
            switch (weekDayName)
            {
                case "Monday": return 1;
                case "Tuesday": return 2;
                case "Wednesday": return 3;
                case "Thursday": return 4;
                case "Friday": return 5;
                case "Saturday": return 6;
                case "Sunday": return 7;
                default: return 9;
            }
        }

        public static ICollection<DomainEntities.TenantHoliday> GetTenantHolidays()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TenantHolidays.AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.TenantId)
                .ThenByDescending(x => x.HolidayDate)
                .Select(x => new DomainEntities.TenantHoliday()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    TenantName = x.Tenant.Name,
                    HolidayDate = x.HolidayDate,
                    Description = x.Description
                })
                .ToList();
        }

        public static ICollection<DomainEntities.AspNetUser> GetAspNetUserData()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.AspNetUsers
                .OrderByDescending(x => x.UserName)
                .Select(x => new DomainEntities.AspNetUser()
                {
                    UserName = x.UserName,
                    LockoutEnabled = x.LockoutEnabled,
                    LockoutEndDateUtc = x.LockoutEndDateUtc,
                    AccessFailedCount = x.AccessFailedCount,
                    DisableUserAfterUtc = x.DisableUserAfterUtc,
                    Roles = x.AspNetRoles.Select(y => y.Name).ToList()
                }).ToList();
        }

        public static void DeleteAppVersion(long recId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.AppVersions.Find(recId);
            if (rec != null)
            {
                orm.AppVersions.Remove(rec);
                orm.SaveChanges();
            }
        }

        /// <summary>
        /// Get Associations for Super Admin
        /// </summary>
        /// <returns></returns>
        public static ICollection<DomainEntities.OfficeHierarchy> GetDetailedAssociations(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            ObjectResult<GetOfficeHierarchyForStaff_Result> resultSet = orm.GetOfficeHierarchyForSuperAdmin(tenantId);
            return resultSet.Select(x => new DomainEntities.OfficeHierarchy()
            {
                ZoneCode = x.ZoneCode,
                ZoneName = x.ZoneName,
                AreaCode = x.AreaCode,
                AreaName = x.AreaName,
                TerritoryCode = x.TerritoryCode,
                TerritoryName = x.TerritoryName,
                HQCode = x.HQCode,
                HQName = x.HQName
            }).ToList();
        }

        /// <summary>
        /// All customers
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DownloadCustomerExtend> GetCustomers(CustomersFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (x) => DBLayer.LogError("GetCustomers", x, "");
            var customers = orm.Customers as IQueryable<Customer>;

            if (searchCriteria != null)
            {
                if (searchCriteria.ApplyCodeFilter)
                {
                    customers = customers.Where(c => c.CustomerCode.Contains(searchCriteria.Code));
                }
                if (searchCriteria.ApplyNameFilter)
                {
                    customers = customers.Where(c => c.Name.Contains(searchCriteria.Name));
                }
                if (searchCriteria.ApplyTypeFilter)
                {
                    customers = customers.Where(c => c.Type.Equals(searchCriteria.Type));
                }
                if (searchCriteria.ApplyHQCodeFilter)
                {
                    customers = customers.Where(c => c.HQCode.Contains(searchCriteria.HQCode));
                }
                if (searchCriteria.ApplyStaffCodeFilter)
                {
                    var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.StaffCode).Select(x => x.HQCode).ToList();
                    customers = customers.Where(x => rsHQCodes.Any(y => y.Equals(x.HQCode, StringComparison.OrdinalIgnoreCase)));
                }
            }

            return customers.Select(c => new DownloadCustomerExtend()
            {
                Code = c.CustomerCode,
                Name = c.Name,
                PhoneNumber = c.ContactNumber,
                Type = c.Type,
                CreditLimit = c.CreditLimit,
                Outstanding = c.Outstanding,
                LongOutstanding = c.LongOutstanding,
                Target = c.Target,
                Sales = c.Sales,
                Payment = c.Payment,
                HQCode = c.HQCode,
                //District = c.District,
                //State = c.State,
                //Branch = c.Branch,
                //Pincode = c.Pincode,
                Address1 = c.Address1,
                Address2 = c.Address2,
                Email = c.Email,
                Active = c.IsActive
            }).ToList();
        }

        public static IEnumerable<DownloadCustomerExtend> GetCustomersWithLocation(CustomersFilter searchCriteria)
        {
            var customers = DBLayer.GetCustomers(searchCriteria);
            var geoLocations = DBLayer.GetGeoLocation();

            EpicCrmEntities orm = DBLayer.GetOrm;
            var customerWithLocation = (from c in customers
                                        join g in geoLocations on c.Code equals g.ClientCode into CustomerLocationGroup
                                        from l in CustomerLocationGroup.DefaultIfEmpty()
                                        select new DownloadCustomerExtend()
                                        {

                                            Code = c.Code,
                                            Name = c.Name,
                                            PhoneNumber = c.PhoneNumber,
                                            Type = c.Type,
                                            CreditLimit = c.CreditLimit,
                                            Outstanding = c.Outstanding,
                                            LongOutstanding = c.LongOutstanding,
                                            Target = c.Target,
                                            Sales = c.Sales,
                                            Payment = c.Payment,
                                            HQCode = c.HQCode,
                                            //District = c.District,
                                            //State = c.State,
                                            //Branch = c.Branch,
                                            //Pincode = c.Pincode,
                                            Address1 = c.Address1,
                                            Address2 = c.Address2,
                                            Email = c.Email,
                                            Active = c.Active,
                                            Latitude = (l == null) ? 0 : l.Latitude,
                                            Longitude = (l == null) ? 0 : l.Longitude
                                        }).ToList();

            return customerWithLocation;
        }

        public static List<DomainEntities.GeoLocation> GetGeoLocation()
        {

            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from l in orm.GeoLocations.Where(x => x.IsActive)
                    select new DomainEntities.GeoLocation
                    {
                        EmployeeId = l.EmployeeId,
                        ClientCode = l.ClientCode,
                        Latitude = l.Latitude,
                        Longitude = l.Longitude,
                        At = l.At,
                    }).ToList();
        }

        public static IEnumerable<DownloadCustomer> GetCustomers(IEnumerable<string> hqcodes)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //SetOrmForLog(orm);

            // getting customers from db and filtering on web server
            // if we try to do join without getting data from db first,
            // then server hangs up due to the nature query is built in LINQ - probably
            //
            var customers = (from c in orm.Customers.Where(x => x.IsActive)
                                .Select(x => new DownloadCustomer()
                                {
                                    Code = x.CustomerCode,
                                    Name = x.Name,
                                    HQCode = x.HQCode,
                                    PhoneNumber = x.ContactNumber,
                                    Type = x.Type,
                                    CreditLimit = x.CreditLimit,
                                    Outstanding = x.Outstanding,
                                    LongOutstanding = x.LongOutstanding,
                                    Target = x.Target,
                                    Sales = x.Sales,
                                    Payment = x.Payment
                                }).ToList()
                             join hqc in hqcodes on c.HQCode equals hqc
                             select c).ToList();

            return customers;
        }

        public static IEnumerable<DownloadCustomerExtend> GetCustomersForAreaPlusStaffCode(long tenantId, string areaCode, string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            if (string.IsNullOrEmpty(areaCode))
            {
                return new List<DownloadCustomerExtend>();
            }

            // find unique hq codes first for area code

            IEnumerable<string> hQCodesForStaff = (from oh in orm.GetOfficeHierarchyForStaff(tenantId, staffCode)
                                                   where oh.AreaCode == areaCode
                                                   select oh.HQCode).ToList();

            var geoLocations = DBLayer.GetGeoLocation();
            var filteredCustomers = orm.Customers.Where(x => hQCodesForStaff.Any(y => y.Equals(x.HQCode, StringComparison.OrdinalIgnoreCase))).ToList();

            // find customers only in selected hq codes;
            return (from c in filteredCustomers
                    join g in geoLocations on c.CustomerCode equals g.ClientCode into CustomerLocationGroup
                    from l in CustomerLocationGroup.DefaultIfEmpty()
                    select new DownloadCustomerExtend()
                    {
                        Id = c.Id,
                        Code = c.CustomerCode,
                        Name = c.Name,
                        PhoneNumber = c.ContactNumber,
                        Type = c.Type,
                        CreditLimit = c.CreditLimit,
                        Outstanding = c.Outstanding,
                        LongOutstanding = c.LongOutstanding,
                        Target = c.Target,
                        Sales = c.Sales,
                        Payment = c.Payment,
                        HQCode = c.HQCode,
                        //District = c.District,
                        //State = c.State,
                        //Branch = c.Branch,
                        //Pincode = c.Pincode,
                        Address1 = c.Address1,
                        Address2 = c.Address2,
                        Email = c.Email,
                        Active = c.IsActive,
                        Latitude = (l == null) ? 0 : l.Latitude,
                        Longitude = (l == null) ? 0 : l.Longitude
                    }).ToList();
        }

        public static ICollection<DomainEntities.PPA> GetPPAData(
                                long tenantId,
                                string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.PPAs.Where(x => x.TenantId == tenantId && x.StaffCode == staffCode)
                            .Select(x => new DomainEntities.PPA()
                            {
                                //Id = x.Id,
                                //TenantId = x.TenantId,
                                AreaCode = x.AreaCode,
                                StaffCode = x.StaffCode,
                                PPACode = x.PPACode,
                                PPAName = x.PPAName,
                                PPAContact = x.PPAContact,
                                Location = x.Location
                            }).ToList();
        }

        public static ICollection<DomainEntities.PPA> GetPPAData(
                                long tenantId,
                                IEnumerable<string> staffCodes)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            // here we are first doing ToList to get data on web server
            // and then doing a join - otherwise there is a server hang up issue.
            return (from cb in orm.PPAs.Where(x => x.TenantId == tenantId).ToList()
                    join cc in staffCodes on cb.StaffCode equals cc
                    select new DomainEntities.PPA()
                    {
                        AreaCode = cb.AreaCode,
                        StaffCode = cb.StaffCode,
                        PPACode = cb.PPACode,
                        PPAName = cb.PPAName,
                        PPAContact = cb.PPAContact,
                        Location = cb.Location
                    }).ToList();
        }

        public static ICollection<DomainEntities.CustomerDivisionBalance> GetCustomerDivisionBalance(
                                long tenantId,
                                string areaCode,
                                string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var resultSet = orm.GetCustomerDivisionBalance(tenantId, staffCode, areaCode);

            return resultSet.Select(x => new DomainEntities.CustomerDivisionBalance()
            {
                Date = x.Date,
                CustomerCode = x.CustomerCode,
                DivisionCode = x.DivisionCode,
                SegmentCode = x.SegmentCode,
                CreditLimit = x.CreditLimit,
                Outstanding = x.Outstanding,
                LongOutstanding = x.LongOutstanding,
                Target = x.Target,
                Sales = x.Sales,
                Payment = x.Payment
            }).ToList();
        }

        public static ICollection<DomainEntities.CustomerDivisionBalance> GetCustomerDivisionBalance(
                                long tenantId,
                                IEnumerable<string> customerCodes)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //SetOrmForLog(orm);

            // here we are first doing ToList to get data on web server
            // and then doing a join - otherwise there is a server hang up issue.
            return (from cb in orm.CustomerDivisionBalances.ToList()
                    join cc in customerCodes on cb.CustomerCode equals cc
                    select new DomainEntities.CustomerDivisionBalance()
                    {
                        Date = cb.DATE,
                        CustomerCode = cb.CustomerCode,
                        DivisionCode = cb.DivisionCode,
                        SegmentCode = cb.SegmentCode,
                        CreditLimit = cb.CreditLimit,
                        Outstanding = cb.Outstanding,
                        LongOutstanding = cb.LongOutstanding,
                        Target = cb.Target,
                        Sales = cb.Sales,
                        Payment = cb.Payment
                    }).ToList();
        }

        public static IEnumerable<DownloadEntity> GetEntitiesForAreaPlusStaffCode(
            long tenantId, string areaCode, string staffCode,
                List<string> agreementStatus,
                List<string> issueReturnStatus,
                List<string> advanceRequestStatus
            )
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            if (string.IsNullOrEmpty(areaCode))
            {
                return new List<DownloadEntity>();
            }

            // find unique hq codes first for area code

            IEnumerable<string> hQCodesForStaff = (from oh in orm.GetOfficeHierarchyForStaff(tenantId, staffCode)
                                                   where oh.AreaCode == areaCode
                                                   select oh.HQCode).ToList();

            // find customers only in selected hq codes;
            return (from c in orm.Entities.Where(x => x.IsActive && hQCodesForStaff.Any(y => y.Equals(x.HQCode, StringComparison.OrdinalIgnoreCase)))
                    select new DownloadEntity()
                    {
                        EntityName = c.EntityName,
                        FatherHusbandName = c.FatherHusbandName,
                        VillageName = c.HQName,
                        EntityType = c.EntityType,
                        Id = c.Id,
                        UniqueIdType = c.UniqueIdType,
                        UniqueId = c.UniqueId,
                        EntityNumber = c.EntityNumber,
                        Latitude = c.Latitude,
                        Longitude = c.Longitude,
                        Agreements = c.EntityAgreements
                                    .Where(b => agreementStatus.Any(s => s.Equals(b.Status, StringComparison.OrdinalIgnoreCase)))
                                    .Select(a => new DownloadEntityAgreement()
                                    {
                                        TypeName = a.WorkflowSeason.TypeName,
                                        SeasonName = a.WorkflowSeason.SeasonName,
                                        AgreementId = a.Id,
                                        AgreementNumber = a.AgreementNumber,
                                        Status = a.Status,
                                        LandSizeInAcres = a.LandSizeInAcres,
                                        IssueReturns = a.IssueReturns
                                                    .Where(n => issueReturnStatus.Any(s => s == n.Status))
                                                    .Select(n => new DownloadIssueReturn()
                                                    {
                                                        Id = n.Id,
                                                        TransactionDate = n.TransactionDate,
                                                        SlipNumber = n.SlipNumber,

                                                        TransactionType = n.TransactionType,
                                                        ItemMasterId = n.ItemMasterId,
                                                        ItemType = n.ItemMaster1.Category,
                                                        ItemCode = n.ItemMaster1.ItemCode,
                                                        ItemUnit = n.ItemMaster1.Unit,
                                                        Quantity = n.Quantity,
                                                        ItemRate = n.ItemRate,

                                                        AppliedTransactionType = n.AppliedTransactionType,
                                                        AppliedItemMasterId = n.AppliedItemMasterId,
                                                        AppliedItemType = n.ItemMaster.Category,
                                                        AppliedItemCode = n.ItemMaster.ItemCode,
                                                        AppliedItemUnit = n.ItemMaster.Unit,
                                                        AppliedQuantity = n.AppliedQuantity,
                                                        AppliedItemRate = n.AppliedItemRate,

                                                        Status = n.Status,

                                                    }).ToList(),
                                        AdvanceRequests = a.AdvanceRequests
                                                        .Where(n => advanceRequestStatus.Any(s => s == n.Status))
                                                        .Select(n => new DownloadAdvanceRequest()
                                                        {
                                                            Id = n.Id,
                                                            AdvanceRequestDate = n.AdvanceRequestDate,
                                                            Status = n.Status,
                                                            AmountRequested = n.Amount,
                                                            AmountApproved = n.AmountApproved
                                                        }).ToList(),
                                        DWS = a.DWS
                                                .Select(n => new DownloadDWS()
                                                {
                                                    Id = n.Id,
                                                    DWSNumber = n.DWSNumber,
                                                    DWSDate = n.DWSDate,
                                                    BagCount = n.BagCount,
                                                    FilledBagsWeightKg = n.FilledBagsWeightKg,
                                                    EmptyBagsWeightKg = n.EmptyBagsWeightKg,
                                                    GoodsWeight = n.GoodsWeight,
                                                    SiloDeductWt = n.SiloDeductWt,
                                                    SiloDeductWtOverride = n.SiloDeductWtOverride,
                                                    NetPayableWt = n.NetPayableWt,
                                                    RatePerKg = n.RatePerKg,
                                                    GoodsPrice = n.GoodsPrice,
                                                    DeductAmount = n.DeductAmount,
                                                    NetPayable = n.NetPayable,
                                                    Status = n.Status,
                                                    BankAccountName = n.BankAccountName,
                                                    BankName = n.BankName,
                                                    PaidDate = n.PaidDate
                                                }).ToList()
                                    }).ToList(),

                        Surveys = c.EntitySurveys
                                    .Where(b => agreementStatus.Any(s => s.Equals(b.Status, StringComparison.OrdinalIgnoreCase)))
                                    .Select(a => new DownloadEntitySurvey()
                                    {
                                        TypeName = a.WorkflowSeason.TypeName,
                                        SeasonName = a.WorkflowSeason.SeasonName,
                                        SurveyId = a.Id,
                                        SurveyNumber = a.SurveyNumber,
                                        Status = a.Status,
                                        LandSizeInAcres = a.LandSizeInAcres,
                                        SowingDate = a.SowingDate
                                    }).ToList(),

                        Contacts = c.EntityContacts.Select(x => new DownloadEntityContact()
                        {
                            Name = x.Name,
                            PhoneNumber = x.PhoneNumber,
                            IsPrimary = x.IsPrimary
                        }).ToList(),

                        BankDetails = c.EntityBankDetails.Where(x => x.IsActive)
                                    .Select(x => new DownloadEntityBankDetail()
                                    {
                                        IsApproved = x.IsApproved,
                                        Status = x.Status,
                                        Comments = x.Comments,
                                        IsSelf = x.IsSelfAccount,
                                        AccountNumber = x.BankAccount,
                                        AccountHolderName = x.AccountHolderName,
                                        BankName = x.BankName,
                                        BankBranch = x.BankBranch
                                    }).ToList()
                    }).ToList();
        }

        public static IEnumerable<DomainEntities.EntityWorkFlow> GetEntitiesWorkFlowForAreaPlusStaffCodeV2(
                            long tenantId, string areaCode, string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            if (string.IsNullOrEmpty(areaCode))
            {
                return new List<DomainEntities.EntityWorkFlow>();
            }

            var resultSet = orm.GetEntityWorkFlow(tenantId, staffCode, areaCode);

            return resultSet.Select(x => new DomainEntities.EntityWorkFlow()
            {
                EntityId = x.EntityId,
                EntityName = x.EntityName,
                Id = x.Id,
                TagName = x.TagName,
                CurrentPhase = x.Phase,
                CurrentPhaseStartDate = x.PlannedStartDate,
                CurrentPhaseEndDate = x.PlannedEndDate,

                InitiationDate = x.InitiationDate,
                IsComplete = x.IsComplete,   // phase complete
                AgreementId = x.AgreementId,
                EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                Notes = x.Notes,
                IsFollowUpRow = x.IsFollowUpRow,
                IsCurrentPhaseRow = (x.IsCurrentPhaseRow == 1),
                IsActive = x.IsActive
            }).ToList();
        }

        public static ConsolidatedCustomerData GetConsolidatedCustomerDownloadInfo(long tenantId, string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IEnumerable<string> uniqueHQCodes = (from oh in orm.GetOfficeHierarchyForStaff(tenantId, staffCode)
                                                 select oh.HQCode)
                                .GroupBy(x => x)
                                .Select(y => y.Key).ToList();

            // find customers only in selected hq codes;
            var custDataList = (from c in orm.Customers
                                join oh in uniqueHQCodes on c.HQCode equals oh
                                select new ConsolidatedCustomerData()
                                {
                                    Outstanding = c.Outstanding,
                                    LongOutstanding = c.LongOutstanding,
                                    Target = c.Target,
                                    Sales = c.Sales,
                                    Payment = c.Payment,
                                }).ToList();

            return new ConsolidatedCustomerData()
            {
                CustomerCount = custDataList.Count,
                LongOutstanding = custDataList.Sum(x => x.LongOutstanding),
                Outstanding = custDataList.Sum(x => x.Outstanding),
                Target = custDataList.Sum(x => x.Target),
                Sales = custDataList.Sum(x => x.Sales),
                Payment = custDataList.Sum(x => x.Payment)
            };
        }

        //public static ICollection<DownloadProductEx> GetDownloadProductsForArea(string areaCode)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;

        //    //orm.Database.Log = (s) => Console.WriteLine(s);

        //    // select only those products for which there is a price
        //    var interimResult = (from p in orm.Products
        //                             //join pg in orm.ProductGroups on p.GroupId equals pg.Id
        //                             //let productPrice = p.ProductPrices.Where(x => x.AreaCode == areaCode).FirstOrDefault()
        //                         where p.IsActive == true
        //                         //&& productPrice != null
        //                         select new DownloadProductEx()
        //                         {
        //                             Name = p.Name,
        //                             Code = p.ProductCode,
        //                             UOM = p.UOM,
        //                             //MRP = 0,
        //                             //GroupName = "" pg.GroupName,
        //                             IsActive = p.IsActive,
        //                             //Stock = 100,
        //                             GstCode = p.GstCode,
        //                             //PriceList = null,
        //                             //AreaCode = areaCode
        //                         }).ToList();

        //    var zeroPriceList = new List<DownloadProductPrice>()
        //                    {
        //                        new DownloadProductPrice() { CustomerType = "DEALER", BillingPrice = 0 },
        //                        new DownloadProductPrice() { CustomerType = "DISTRIBUTOR", BillingPrice = 0 },
        //                        new DownloadProductPrice() { CustomerType = "P.DISTRIBUTOR", BillingPrice = 0 }
        //                    };

        //    Parallel.ForEach(interimResult, ir =>
        //    {
        //        ir.PriceList = zeroPriceList;
        //        ir.GroupName = "";
        //        ir.MRP = 0;
        //        ir.Stock = 100;
        //        ir.AreaCode = areaCode;
        //    });

        //    return interimResult;
        //}

        public static ICollection<DownloadProductEx> GetDownloadProductsForArea(string areaCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            //orm.Database.Log = (s) => Console.WriteLine(s);

            // select only those products for which there is a price
            return (from p in orm.Products
                    join pg in orm.ProductGroups on p.GroupId equals pg.Id
                    let productPrice = p.ProductPrices.Where(x => x.AreaCode == areaCode).FirstOrDefault()
                    where p.IsActive == true
                    && productPrice != null
                    select new DownloadProductEx()
                    {
                        Name = p.Name,
                        Code = p.ProductCode,
                        UOM = p.UOM,
                        MRP = productPrice.MRP,
                        GroupName = pg.GroupName,
                        IsActive = p.IsActive,
                        Stock = productPrice.Stock,
                        GstCode = p.GstCode,
                        PriceList = new List<DownloadProductPrice>()
                            {
                                new DownloadProductPrice() { CustomerType = "DEALER", BillingPrice = productPrice.DEALERPrice },
                                new DownloadProductPrice() { CustomerType = "DISTRIBUTOR", BillingPrice = productPrice.DISTPrice },
                                new DownloadProductPrice() { CustomerType = "P.DISTRIBUTOR", BillingPrice = productPrice.PDISTPrice }
                            },
                        AreaCode = areaCode
                    }).ToList();
        }

        public static ICollection<Product2> GetProducts2()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            //orm.Database.Log = (s) => Console.WriteLine(s);

            return (from p in orm.Products
                    join pg in orm.ProductGroups on p.GroupId equals pg.Id
                    where p.IsActive == true
                    select new Product2()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Code = p.ProductCode,
                        UOM = p.UOM,
                        GroupName = pg.GroupName,
                        GstCode = p.GstCode
                    }).ToList();
        }

        public static ICollection<ProductPrice2> GetProductPrice2(string areaCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            //orm.Database.Log = (s) => Console.WriteLine(s);

            // select only those products for which there is a price
            return (from pp in orm.ProductPrices
                    where pp.AreaCode == areaCode
                    select new ProductPrice2()
                    {
                        Id = pp.Id,
                        ProductId = pp.ProductId,
                        Stock = pp.Stock,
                        MRP = pp.MRP,
                        DEALERPrice = pp.DEALERPrice,
                        DISTPrice = pp.DISTPrice,
                        PDISTPrice = pp.PDISTPrice
                    }).ToList();
        }

        /// <summary>
        /// Gives the count of products that are available to employee
        /// based on association data; To get products, call GetDownloadProducts
        /// </summary>
        /// <param name="staffCode"></param>
        /// <returns></returns>
        //public static int GetAvailableProductsCount(string staffCode)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;

        //    // find out the unique Area codes that employee has association
        //    //
        //    // Due to association error, it may be possible, that a person has
        //    // two HQ codes in different areas.  If the number of areas are more
        //    // than one, we can't return two prices for the same product.
        //    // Hence I am taking the first area code in this case.
        //    string areaCode = orm.GetOfficeHierarchyForStaff(staffCode)
        //                    .Select(x => x.AreaCode).GroupBy(x => x).Select(x => x.Key).FirstOrDefault();

        //    if (String.IsNullOrEmpty(areaCode))
        //    {
        //        areaCode = "";
        //    }

        //    // select only those products for which there is a price
        //    return (from p in orm.Products
        //            join pg in orm.ProductGroups on p.GroupId equals pg.Id
        //            let productPrice = p.ProductPrices.Where(x => x.AreaCode == areaCode).FirstOrDefault()
        //            where p.IsActive == true && productPrice != null
        //            select 1
        //            ).Count();
        //}

        /// <summary>
        /// Gives the count of products that are available to employee
        /// based on association data; To get products, call GetDownloadProducts
        /// To get area for staff code call GetAppliedProductArea
        /// </summary>
        /// <param name="staffCode"></param>
        /// <returns></returns>
        public static int GetAvailableProductsCount(string areaCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            // select only those products for which there is a price
            return (from p in orm.Products
                    join pg in orm.ProductGroups on p.GroupId equals pg.Id
                    let productPrice = p.ProductPrices.Where(x => x.AreaCode == areaCode).FirstOrDefault()
                    where p.IsActive == true && productPrice != null
                    select 1
                    ).Count();
        }

        /// <summary>
        /// Gives the area code that is applied to retrieve products
        /// </summary>
        /// <param name="staffCode"></param>
        /// <returns></returns>
        public static string GetAppliedProductArea(long tenantId, string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            // find out the unique Area codes that employee has association
            //
            // Due to association error, it may be possible, that a person has
            // two HQ codes in different areas.  If the number of areas are more
            // than one, we can't return two prices for the same product.
            // Hence I am taking the first area code in this case.
            string areaCode = orm.GetOfficeHierarchyForStaff(tenantId, staffCode)
                            .Select(x => x.AreaCode).GroupBy(x => x).Select(x => x.Key).FirstOrDefault();

            if (String.IsNullOrEmpty(areaCode))
            {
                areaCode = "";
            }

            return areaCode;
        }

        public static long LogError(string processName, string logText, string logSnip)
        {
            EpicCrmEntities orm = DBLayer.GetNoLogOrm;
            DBModel.ErrorLog errorLog = new DBModel.ErrorLog()
            {
                At = DateTime.UtcNow,
                Process = processName,
                LogText = logText,
                LogSnip = logSnip
            };

            orm.ErrorLogs.Add(errorLog);

            orm.SaveChanges();
            return errorLog.Id;
        }

        public static void ProcessSTRData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteSTRData(batchId);
        }

        public static void ProcessTerminateAgreementData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteTerminateAgreementData(batchId);
        }

        public static void ProcessEntityWorkFlowData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteEntityWorkFlowDataV3(batchId);
        }
        public static void ProcessSqliteLeaveData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteLeaveData(batchId);
        }

        public static long LogSms(long tenantId, string smsText, DateTime istDateTime, string alertApiResponse, string smsType, string sender)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DBModel.TenantSMSLog smsLog = new DBModel.TenantSMSLog()
            {
                TenantId = tenantId,
                SMSDateTime = istDateTime,
                SMSText = smsText,
                SMSApiResponse = alertApiResponse,
                SmsType = smsType,
                SenderName = sender
            };

            orm.TenantSMSLogs.Add(smsLog);

            orm.SaveChanges();
            return smsLog.Id;
        }

        public static IEnumerable<DomainEntities.CodeTableEx> GetActivityTypes()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.ActivityTypes.AsNoTracking()
                .OrderBy(x => x.ActivityName)
                .Select(x => new DomainEntities.CodeTableEx()
                {
                    CodeName = x.ActivityName,
                    Code = x.ActivityName,
                    DisplaySequence = 10
                }).ToList();
        }

        public static ICollection<DomainEntities.CodeTableEx> GetCodeTable(long tenantId, string codeTableType)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.CodeTables.AsNoTracking()
                .Where(x => x.CodeType == codeTableType && x.IsActive && x.TenantId == tenantId)
                .OrderBy(x => x.DisplaySequence)
                .ThenBy(x => x.CodeName)
                .Select(x => new DomainEntities.CodeTableEx()
                {
                    CodeName = x.CodeName,
                    Code = x.CodeValue,
                    DisplaySequence = x.DisplaySequence
                }).ToList();
        }

        public static IEnumerable<DomainEntities.ExpenseItem> GetExpenseItems(long expenseId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.ExpenseItems.Where(x => x.ExpenseId == expenseId)
                .OrderBy(x => x.SequenceNumber)
                .Select(x => new DomainEntities.ExpenseItem()
                {
                    Id = x.Id,
                    ExpenseId = x.ExpenseId,
                    SequenceNumber = x.SequenceNumber,
                    ExpenseType = x.ExpenseType,
                    TransportType = x.TransportType,
                    Amount = x.Amount,
                    DeductedAmount = x.DeductedAmount,
                    RevisedAmount = x.RevisedAmount,
                    OdometerStart = x.OdometerStart,
                    OdometerEnd = x.OdometerEnd,
                    ImageCount = x.ImageCount,
                    FuelType = x.FuelType,
                    FuelQuantityInLiters = x.FuelQuantityInLiters,
                    Comment = x.Comment
                }).ToList();
        }

        //Author:Kartik V, Purpose:Edit ExpenseItem for deducting amount; Date:08-03-2022
        public static DomainEntities.ExpenseItem GetExpenseItem(long Id)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.ExpenseItems.Where(x => x.Id == Id)
                .Select(x => new DomainEntities.ExpenseItem()
                {
                    Id = x.Id,
                    ExpenseId = x.ExpenseId,
                    SequenceNumber = x.SequenceNumber,
                    ExpenseType = x.ExpenseType,
                    TransportType = x.TransportType,
                    Amount = x.Amount,
                    DeductedAmount = x.DeductedAmount,
                    RevisedAmount = x.RevisedAmount,
                    OdometerStart = x.OdometerStart,
                    OdometerEnd = x.OdometerEnd,
                    ImageCount = x.ImageCount,
                    FuelType = x.FuelType,
                    FuelQuantityInLiters = x.FuelQuantityInLiters,
                    Comment = x.Comment
                }).FirstOrDefault();
        }

        public static DBSaveStatus SaveExpenseItemData(DomainEntities.ExpenseItem inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.ExpenseItems.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid ExpenseItem Id");
            }

            sp.DeductedAmount = inputRec.DeductedAmount;
            sp.RevisedAmount = inputRec.RevisedAmount;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DomainEntities.Order GetOrder(long orderId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from o in orm.Orders
                    join c in orm.Customers on o.CustomerCode equals c.CustomerCode into temp
                    from t in temp.DefaultIfEmpty()
                    join sp in orm.SalesPersons on o.TenantEmployee.EmployeeCode equals sp.StaffCode into temp1
                    from t1 in temp1.DefaultIfEmpty()
                    where o.Id == orderId
                    select new DomainEntities.Order()
                    {
                        Id = o.Id,
                        EmployeeId = o.EmployeeId,
                        DayId = o.DayId,
                        CustomerCode = o.CustomerCode,
                        OrderDate = o.OrderDate,
                        OrderType = o.OrderType,
                        ItemCount = o.ItemCount,
                        TotalAmount = o.TotalAmount,
                        TotalGST = o.TotalGST,
                        NetAmount = o.NetAmount,
                        DateUpdated = o.DateUpdated,
                        EmployeeCode = o.TenantEmployee.EmployeeCode,
                        EmployeeName = o.TenantEmployee.Name,
                        CustomerName = (t == null) ? string.Empty : t.Name,
                        IsApproved = o.IsApproved,
                        ApproveRef = o.ApproveRef,
                        ApprovedAmt = o.ApproveAmount,
                        ApproveComments = o.ApproveNotes,
                        ApprovedDate = o.ApproveDate,
                        ApprovedBy = o.ApprovedBy,
                        RevisedTotalAmount = o.RevisedTotalAmount,
                        RevisedTotalGST = o.RevisedTotalGST,
                        RevisedNetAmount = o.RevisedNetAmount,
                        Phone = (t1 == null) ? "" : t1.Phone,
                        CustomerPhone = (t == null) ? "" : t.ContactNumber,
                        DiscountType = o.DiscountType,
                        ImageCount = o.ImageCount
                    }).FirstOrDefault();
        }

        public static int GetActivityCount(long employeeDayId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Activities.AsNoTracking().Where(x => x.EmployeeDayId == employeeDayId).Count();
        }

        public static long GetLastOpenEmployeeDayId(long employeeId, DateTime at)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            EmployeeDay ed = orm.EmployeeDays
                .Where(x => x.TenantEmployeeId == employeeId && x.EndTime.HasValue == false)
                .OrderByDescending(x => x.StartTime)
                .FirstOrDefault();

            if (ed == null)
            {
                return TranslateEmployeeIdToEmployeeDayId(employeeId, at);
            }
            else
            {
                return ed.Id;
            }
        }

        public static long GetPreviousOpenEmployeeDayId(long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            EmployeeDay ed = orm.EmployeeDays
                .Where(x => x.TenantEmployeeId == employeeId && x.EndTime.HasValue == false)
                .OrderByDescending(x => x.StartTime)
                .FirstOrDefault();

            return ed?.Id ?? 0;
        }

        public static ICollection<SqliteExpenseData> GetSavedExpenseItems(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteExpenses
                .Where(x => x.BatchId == batchId)
                .OrderBy(x => x.Id)
                .Select(x => new SqliteExpenseData()
                {
                    Id = x.Id,
                    SqliteTableId = x.SqliteTableId,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    ExpenseType = x.ExpenseType,
                    Amount = (x.Amount.HasValue ? x.Amount.Value : 0M),
                    OdometerStart = x.OdometerStart.HasValue ? x.OdometerStart.Value : 0,
                    OdometerEnd = x.OdometerEnd.HasValue ? x.OdometerEnd.Value : 0,
                    VehicleType = x.VehicleType,
                    ImageCount = x.ImageCount,
                    IsProcessed = x.IsProcessed,
                    ExpenseItemId = x.ExpenseItemId,
                    FuelType = x.FuelType,
                    FuelQuantityInLiters = x.FuelQuantityInLiters,
                    Comment = x.Comment
                }).ToList();
        }

        public static IEnumerable<SqliteOrderData> GetSavedOrders(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteOrders
                .Where(x => x.BatchId == batchId)
                .Select(x => new SqliteOrderData()
                {
                    Id = x.Id,
                    PhoneDbId = x.PhoneDbId,
                    CustomerCode = x.CustomerCode,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    OrderType = x.OrderType,
                    OrderDate = x.OrderDate,
                    TotalAmount = x.TotalAmount,
                    ItemCount = x.ItemCount,
                    OrderId = x.OrderId,
                    PhoneActivityId = x.PhoneActivityId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    TotalGST = x.TotalGST,
                    NetAmount = x.NetAmount,
                    MaxDiscountPercentage = x.MaxDiscountPercentage,
                    DiscountType = x.DiscountType,
                    AppliedDiscountPercentage = x.AppliedDiscountPercentage,
                    ImageCount = x.ImageCount
                }).ToList();
        }

        public static IEnumerable<DomainEntities.SqliteDeviceLog> GetDeviceLogs(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteDeviceLogs
                .Where(x => x.BatchId == batchId)
                .OrderByDescending(x => x.At)
                .Select(x => new DomainEntities.SqliteDeviceLog()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    LogText = x.LogText,
                    TimeStamp = x.At
                }).ToList();
        }

        public static IEnumerable<SqliteReturnOrderData> GetSavedReturns(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteReturnOrders
                .Where(x => x.BatchId == batchId)
                .Select(x => new SqliteReturnOrderData()
                {
                    Id = x.Id,
                    PhoneDbId = x.PhoneDbId,
                    CustomerCode = x.CustomerCode,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    ReturnOrderDate = x.ReturnOrderDate,
                    TotalAmount = x.TotalAmount,
                    ItemCount = x.ItemCount,
                    ReturnOrderId = x.ReturnOrderId,
                    PhoneActivityId = x.PhoneActivityId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    ReferenceNum = x.ReferenceNum,
                    Comment = x.Comment
                }).ToList();
        }

        public static IEnumerable<SqliteOrderLineData> GetSavedOrderItems(long sqliteOrderId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteOrderItems
                .Where(x => x.SqliteOrderId == sqliteOrderId)
                .OrderBy(x => x.SerialNumber)
                .Select(x => new SqliteOrderLineData()
                {
                    Id = x.Id,
                    SqliteOrderId = x.SqliteOrderId,
                    SerialNumber = x.SerialNumber,
                    ProductCode = x.ProductCode,
                    UnitPrice = x.UnitPrice,
                    UnitQuantity = x.UnitQuantity,
                    Amount = x.Amount,
                    DiscountPercent = x.DiscountPercent,
                    DiscountedPrice = x.DiscountedPrice,
                    ItemPrice = x.ItemPrice,
                    GstPercent = x.GstPercent,
                    GstAmount = x.GstAmount,
                    NetPrice = x.NetPrice
                }).ToList();
        }

        public static ICollection<ActivityMapData> GetAllActivityMapData(SearchCriteria searchCriteria)
        {
            DateTime justStartDatePart = DateTime.MinValue;
            DateTime justEndDatePart = DateTime.MinValue;

            if (searchCriteria.ApplyDateFilter)
            {
                justStartDatePart = new DateTime(searchCriteria.DateFrom.Year, searchCriteria.DateFrom.Month, searchCriteria.DateFrom.Day);
                justEndDatePart = new DateTime(searchCriteria.DateTo.Year, searchCriteria.DateTo.Month, searchCriteria.DateTo.Day);
            }

            EpicCrmEntities orm = DBLayer.GetOrm;
            var reportModel = from a in orm.Activities
                              join t in orm.Trackings on a.Id equals t.ActivityId
                              join ed in orm.EmployeeDays on a.EmployeeDayId equals ed.Id
                              join d in orm.Days on ed.DayId equals d.Id
                              where d.DATE >= justStartDatePart && d.DATE <= justEndDatePart
                              select new ActivityMapData()
                              {
                                  ActivityId = a.Id,
                                  TenantEmployeeId = ed.TenantEmployeeId,
                                  EmployeeDayId = a.EmployeeDayId,
                                  ClientName = a.ClientName,
                                  ClientPhone = a.ClientPhone,
                                  ClientType = a.ClientType,
                                  ActivityType = a.ActivityType,
                                  Comments = a.Comments,
                                  At = a.At,
                                  Latitude = t.EndGPSLatitude,
                                  Longitude = t.EndGPSLongitude,
                                  ImageCount = a.ImageCount,
                                  AtLocation = a.AtBusiness,
                                  GoogleMapsDistanceInMeters = t.GoogleMapsDistanceInMeters,
                                  LocationName = t.EndLocationName
                              };
            if (searchCriteria.ApplyActivityTypeFilter)
            {
                reportModel = reportModel.Where(r => r.ActivityType.Equals(searchCriteria.ActivityType));
            }

            if (searchCriteria.ApplyClientTypeFilter)
            {
                reportModel = reportModel.Where(r => r.ClientType.Equals(searchCriteria.ClientType));
            }

            if (searchCriteria.ApplyClientNameFilter)
            {
                reportModel = reportModel.Where(r => r.ClientName.Contains(searchCriteria.ClientName));
            }

            return reportModel.ToList();
        }

        public static IEnumerable<ActivityMapData> GetActivityMapData(long empDayId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from a in orm.Activities
                    join t in orm.Trackings on a.Id equals t.ActivityId
                    where a.EmployeeDayId == empDayId
                    select new ActivityMapData()
                    {
                        ActivityId = a.Id,
                        EmployeeDayId = empDayId,
                        ClientName = a.ClientName,
                        ClientPhone = a.ClientPhone,
                        ClientType = a.ClientType,
                        AtLocation = a.AtBusiness,
                        ActivityType = a.ActivityType,
                        ActivityAmount = a.ActivityAmount,
                        Comments = a.Comments,
                        At = a.At,
                        Latitude = t.EndGPSLatitude,
                        Longitude = t.EndGPSLongitude,
                        ImageCount = a.ImageCount,
                        ContactCount = a.ContactCount,
                        ActivityTrackingType = a.ActivityTrackingType,
                    }).ToList();
        }

        public static IEnumerable<ActivityMapData> GetActivityData(long activityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from a in orm.Activities
                    join t in orm.Trackings on a.Id equals t.ActivityId
                    where a.Id == activityId
                    select new ActivityMapData()
                    {
                        ActivityId = a.Id,
                        EmployeeDayId = a.EmployeeDayId,
                        ClientName = a.ClientName,
                        ClientPhone = a.ClientPhone,
                        ClientType = a.ClientType,
                        AtLocation = a.AtBusiness,
                        ActivityType = a.ActivityType,
                        ActivityAmount = a.ActivityAmount,
                        Comments = a.Comments,
                        At = a.At,
                        Latitude = t.EndGPSLatitude,
                        Longitude = t.EndGPSLongitude,
                        ImageCount = a.ImageCount,
                        ContactCount = a.ContactCount
                    }).ToList();
        }

        public static IEnumerable<ActivityMapData> GetManyActivityData(IEnumerable<long> activityIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from a in orm.Activities
                    join t in orm.Trackings on a.Id equals t.ActivityId
                    where activityIds.Contains(a.Id)
                    select new ActivityMapData()
                    {
                        ActivityId = a.Id,
                        EmployeeDayId = a.EmployeeDayId,
                        ClientName = a.ClientName,
                        ClientPhone = a.ClientPhone,
                        ClientType = a.ClientType,
                        AtLocation = a.AtBusiness,
                        ActivityType = a.ActivityType,
                        ActivityAmount = a.ActivityAmount,
                        Comments = a.Comments,
                        At = a.At,
                        Latitude = t.EndGPSLatitude,
                        Longitude = t.EndGPSLongitude,
                        ImageCount = a.ImageCount,
                        ContactCount = a.ContactCount
                    }).ToList();
        }
        public static IEnumerable<ActivityContactData> GetActivityContactData(long activityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from a in orm.ActivityContacts
                    where a.ActivityId == activityId
                    select new ActivityContactData()
                    {
                        Id = a.Id,
                        ActivityId = a.ActivityId,
                        Name = a.Name,
                        PhoneNumber = a.PhoneNumber,
                        IsPrimary = a.IsPrimary
                    }).ToList();
        }

        public static IEnumerable<SqliteReturnOrderItemData> GetSavedReturnOrderItems(long sqliteReturnOrderId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteReturnOrderItems
                .Where(x => x.SqliteReturnOrderId == sqliteReturnOrderId)
                .OrderBy(x => x.SerialNumber)
                .Select(x => new SqliteReturnOrderItemData()
                {
                    Id = x.Id,
                    SqliteReturnOrderId = x.SqliteReturnOrderId,
                    SerialNumber = x.SerialNumber,
                    ProductCode = x.ProductCode,
                    UnitPrice = x.UnitPrice,
                    UnitQuantity = x.UnitQuantity,
                    Amount = x.Amount,
                    Comment = x.Comment
                }).ToList();
        }

        public static void SaveRevisedOrderItems(long orderId, IEnumerable<EditOrderItem> editedItems, string approvedBy)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            decimal revisedOrderTotalAmount = 0;
            decimal revisedTotalGst = 0;
            decimal revisedNetAmount = 0;

            foreach (var item in editedItems)
            {
                var oi = orm.OrderItems.Find(item.Id);
                if (oi == null)
                {
                    throw new ArgumentException($"Order item with Id {item.Id} does not exist.");
                }

                if (item.RevisedQuantity > oi.UnitQuantity || item.RevisedQuantity < 0)
                {
                    throw new ArgumentOutOfRangeException($"Invalid Revised quantity {item.RevisedQuantity} for Product code {oi.ProductCode}", innerException: null);
                }

                if (oi.RevisedUnitQuantity != item.RevisedQuantity)
                {
                    oi.RevisedUnitQuantity = item.RevisedQuantity;
                    oi.RevisedAmount = oi.RevisedUnitQuantity * oi.UnitPrice;  // legacy
                    oi.DateUpdated = DateTime.UtcNow;
                    oi.UpdatedBy = approvedBy;

                    oi.RevisedItemPrice = oi.RevisedDiscountedPrice * item.RevisedQuantity;
                    oi.RevisedGstAmount = oi.RevisedItemPrice * oi.RevisedGstPercent / 100;
                    oi.RevisedNetPrice = oi.RevisedItemPrice + oi.RevisedGstAmount;
                }

                revisedOrderTotalAmount += oi.RevisedItemPrice;
                revisedTotalGst += oi.RevisedGstAmount;
                revisedNetAmount += oi.RevisedNetPrice;
            }

            //now update the order
            var orderRecord = orm.Orders.Find(orderId);
            if (orderRecord.RevisedTotalAmount != revisedOrderTotalAmount)
            {
                orderRecord.RevisedTotalAmount = revisedOrderTotalAmount;
                orderRecord.RevisedTotalGST = revisedTotalGst;
                orderRecord.RevisedNetAmount = revisedNetAmount;
            }

            orm.SaveChanges();
        }

        // Filtering for employee Ids is not longer done in this LINQ query
        // as major performance bottleneck were detected in Multiplex environment - 20.08.2019
        //
        //public static ICollection<AttendanceData> GetAttendanceReportData(/*IEnumerable<long> employeeIds,*/ IEnumerable<long> dayIds)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;

        //    return (
        //            //from ed in orm.EmployeeDays.Where(x => employeeIds.Any(e => e == x.TenantEmployeeId) && dayIds.Any(d => d == x.DayId))
        //            from ed in orm.EmployeeDays.Where(x => dayIds.Any(d => d == x.DayId))
        //            join t in orm.Trackings.Where(x => x.IsStartOfDay || x.IsEndOfDay) on ed.Id equals t.EmployeeDayId

        //            orderby ed.TenantEmployeeId, t.Id  // order by tracking ids with in each employee
        //            select new AttendanceData()
        //            {
        //                TrackingId = t.Id,
        //                TenantEmployeeId = ed.TenantEmployeeId,
        //                DayId = ed.DayId,
        //                At = t.At,
        //                EmployeeDayId = ed.Id,
        //                IsStartOfDay = t.IsStartOfDay,
        //                IsEndOfDay = t.IsEndOfDay,
        //                StartLocation = (t.IsStartOfDay) ? t.EndLocationName : "",
        //                EndLocation = (t.IsEndOfDay) ? t.EndLocationName : ""
        //            }).ToList();
        //}

        public static ICollection<AttendanceData> GetAttendanceReportData(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var queryData =
                    (
                    from dy in orm.Days
                    join ed in orm.EmployeeDays on dy.Id equals ed.DayId
                    join t in orm.Trackings on ed.Id equals t.EmployeeDayId
                    join act in orm.Activities on t.EmployeeDayId equals act.EmployeeDayId into activity
                    where dy.DATE >= searchCriteria.DateFrom.Date && dy.DATE <= searchCriteria.DateTo.Date
                    && (t.IsStartOfDay || t.IsEndOfDay)
                    orderby ed.TenantEmployeeId, t.Id  // order by tracking ids with in each employee

                    select new AttendanceData()
                    {
                        TrackingId = t.Id,
                        TenantEmployeeId = ed.TenantEmployeeId,
                        DayId = ed.DayId,
                        At = t.At,
                        EmployeeDayId = ed.Id,
                        IsStartOfDay = t.IsStartOfDay,
                        IsEndOfDay = t.IsEndOfDay,
                        StartLocation = (t.IsStartOfDay) ? t.EndLocationName : "",
                        EndLocation = (t.IsEndOfDay) ? t.EndLocationName : "",
                        GoogleMapsDistanceInMeters = t.GoogleMapsDistanceInMeters,
                        StaffCode = ed.TenantEmployee.EmployeeCode,
                        Name = ed.TenantEmployee.Name,
                        Date = dy.DATE,
                        ActivityCount = activity.Count()
                    });

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                queryData = queryData.Where(x => x.StaffCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                queryData = queryData.Where(x => x.Name.Contains(searchCriteria.EmployeeName));
            }
            return queryData.ToList();
        }

        public static ICollection<AbsenteeData> GetAbsenteeReportData(IEnumerable<long> employeeIds, IEnumerable<long> dayIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return (from te in orm.TenantEmployees.Where(x => employeeIds.Any(e => e == x.Id))
                    from ds in orm.Days.Where(x => dayIds.Any(d => d == x.Id))

                    join ed in orm.EmployeeDays.Where(x => x.AppVersion != "***") on new { TenantEmployeeId = te.Id, DayId = ds.Id } equals new { ed.TenantEmployeeId, ed.DayId } into EmployeeDaysOuter
                    from edOuter in EmployeeDaysOuter.DefaultIfEmpty()

                    join sp in orm.SalesPersons on te.EmployeeCode equals sp.StaffCode into SalesPersonOuter
                    from spOuter in SalesPersonOuter.DefaultIfEmpty()

                    where edOuter == null

                    orderby ds.DATE, te.Name
                    select new AbsenteeData()
                    {
                        TenantEmployeeId = te.Id,
                        Name = te.Name,
                        IsActive = te.IsActive,
                        SignupDate = te.DateCreated,
                        DayId = ds.Id,
                        Date = ds.DATE,
                        StaffCode = te.EmployeeCode,
                        ExpenseHQCode = (spOuter != null) ? spOuter.HQCode : "",
                        // HQCode is defined as NOT Null in SalesPerson table;
                        Phone = (spOuter != null) ? spOuter.Phone : "",
                        IsActiveInSap = (spOuter != null) ? spOuter.IsActive : false
                    }).ToList();
        }

        public static ICollection<DeviceInfo> GetInstalledAppVersion()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            // get latest and greatest app version
            return orm.EmployeeDays.AsNoTracking()
                                .Where(x => !x.AppVersion.Equals("***"))
                                .Select(x => new { x.Id, x.TenantEmployeeId, x.Day.DATE })
                                .GroupBy(x => x.TenantEmployeeId)
                                .Select(x => new
                                {
                                    TEID = x.Key,
                                    LastAccessDate = x.OrderByDescending(y => y.DATE).FirstOrDefault().DATE,
                                    Id = x.OrderByDescending(y => y.DATE).FirstOrDefault().Id   // there will always be an element in group result.
                                })
                                .Join(orm.EmployeeDays, s => s.Id, t => t.Id, (s, t) => new DeviceInfo
                                {
                                    TenantEmployeeId = s.TEID,
                                    LastAccessDate = s.LastAccessDate,
                                    PhoneModel = t.PhoneModel,
                                    PhoneOS = t.PhoneOS,
                                    AppVersion = t.AppVersion
                                })
                                .ToList();
        }

        public static ICollection<AppSignUpData> GetAppSignUpReportData(IEnumerable<long> employeeIds, DomainEntities.SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            if (searchCriteria == null || searchCriteria.ApplyDateFilter == false)
            {
                throw new ArgumentException("Invalid search criteria");
            }

            DateTime dateTo = searchCriteria.DateTo.AddDays(1);

            return (from te in orm.TenantEmployees.Where(x => employeeIds.Any(e => e == x.Id) &&
                         x.DateCreated >= searchCriteria.DateFrom && x.DateCreated < dateTo)
                    join sp in orm.SalesPersons on te.EmployeeCode equals sp.StaffCode into SalesPersonOuter
                    from spOuter in SalesPersonOuter.DefaultIfEmpty()

                    orderby te.Name
                    select new AppSignUpData()
                    {
                        TenantEmployeeId = te.Id,
                        Name = te.Name,
                        IsActive = te.IsActive,
                        SignupDate = te.DateCreated,
                        StaffCode = te.EmployeeCode,
                        Phone = (spOuter != null) ? spOuter.Phone : "",
                        ExpenseHQCode = (spOuter != null) ? spOuter.HQCode : "",
                        // HQCode is defined as NOT Null in SalesPerson table;,
                    }).ToList();
        }

        public static ICollection<AppSignInData> GetAppSignInReportData(IEnumerable<long> employeeIds, IEnumerable<long> dayIds, SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var resultSet = orm.EmployeeDays.Where(x => dayIds.Any(d => d == x.DayId) && employeeIds.Any(e => e == x.TenantEmployeeId))
                .Select(x => x.TenantEmployeeId)
                .GroupBy(x => x)
                .Select(x => new { TEId = x.Key, DaysActive = x.Count() })
                .ToList();

            // to result set we also need to add those employees who did not sign in even once.
            resultSet = (from ei in employeeIds
                         join rs in resultSet on ei equals rs.TEId into outer
                         from o in outer.DefaultIfEmpty()
                         select new { TEId = ei, DaysActive = o?.DaysActive ?? 0 })
                         .ToList();

            var distinctEmployeeIds = resultSet.Select(x => x.TEId).ToList();

            IQueryable<AppSignInData> data = from te in orm.TenantEmployees.Where(x => distinctEmployeeIds.Any(e => e == x.Id))
                                             join sp in orm.SalesPersons on te.EmployeeCode equals sp.StaffCode into SalesPersonOuter
                                             from spOuter in SalesPersonOuter.DefaultIfEmpty()
                                             orderby te.Name
                                             select new AppSignInData()
                                             {
                                                 TenantEmployeeId = te.Id,
                                                 Name = te.Name,
                                                 IsActive = te.IsActive,
                                                 SignupDate = te.DateCreated,
                                                 StaffCode = te.EmployeeCode,
                                                 Phone = (spOuter != null) ? spOuter.Phone : "",
                                                 ExpenseHQCode = (spOuter != null) ? spOuter.HQCode : "",
                                                 DaysActive = 0,
                                                 Department = (spOuter != null) ? spOuter.Department : "",
                                                 Designation = (spOuter != null) ? spOuter.Designation : "",
                                                 // HQCode is defined as NOT Null in SalesPerson table
                                                 IsActiveInSap = (spOuter != null) ? spOuter.IsActive : false
                                             };

            if (searchCriteria?.ApplyEmployeeStatusFilter ?? false)
            {
                data = data.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
            }

            if (searchCriteria?.ApplyDepartmentFilter ?? false)
            {
                data = data.Where(x => x.Department == searchCriteria.Department);
            }

            if (searchCriteria?.ApplyDesignationFilter ?? false)
            {
                data = data.Where(x => x.Designation == searchCriteria.Designation);
            }

            var dataAsList = data.ToList();

            // now fill the DaysActive value
            Parallel.ForEach(dataAsList, d =>
            {
                d.DaysActive = resultSet.FirstOrDefault(x => x.TEId == d.TenantEmployeeId)?.DaysActive ?? 0;
            });

            return dataAsList;
        }

        /// <summary>
        /// Author: Pankaj Kumar; Date: 29-04-2021; Purpose: Get Day Plan Data
        /// Build the LINQ queries for Day Data
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <param name="dayIds"></param>
        /// <param name="date"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static IEnumerable<DayPlanData> GetDayPlanReportData(IEnumerable<long> employeeIds,
                                                                        IEnumerable<long> dayIds, IEnumerable<DateTime> date,
                                                                                SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            //Get all the records from DayPlan Target which satisfies the filters applied (empId, dayId & date ids collections)
            var queryDayPlanTargetData = GetDayPlanTargetDataFiltersApplied(employeeIds, dayIds, date);

            var queryOrderData = GetOrderData(employeeIds, dayIds, date);

            var queryPaymentData = GetPaymentData(employeeIds, dayIds, date);

            //var queryConsolidatedData = GetConsolidatedData(queryOrderData, queryPaymentData);
            var queryConsolidatedOrderData = GetConsolidatedDayPlanOrderData(queryDayPlanTargetData, queryOrderData);

            var queryConsolidatedOrderPaymentData = GetConsolidatedDayPlanOrderPaymentData(queryConsolidatedOrderData, queryPaymentData, searchCriteria);

            //var queryTotalsData = GetTotalsData(queryConsolidatedData);

            //var query = AppendActualsZeroItemsDayPlanTarget(queryDayPlanTargetData, queryTotalsData, searchCriteria);
            //var query = ApplyFiltersDayPlanTarget(queryConsolidatedOrderPaymentData, searchCriteria);

            return queryConsolidatedOrderPaymentData;
        }

        /// <summary>
        ///  Author : Kartik; Date 24-06-2021; Purpose : Get DayPlan and Order data
        /// </summary>
        /// <param name="queryDayPlanTargetData"></param>
        /// <param name="queryOrderData"></param>
        /// <returns></returns>
        private static List<DayPlanData> GetConsolidatedDayPlanOrderData(IEnumerable<DayPlanData> queryDayPlanTargetData, List<OrderPaymentGroupData> queryOrderData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            //Get Orders and Collections items for a day and employee in the applied filters
            var dayPlanOrderCollection = (from qdpt in queryDayPlanTargetData
                                          join qod in queryOrderData on
                                              new { EmployeeId = qdpt.EmployeeId, DayId = qdpt.DayId, Date = qdpt.Date }
                                                                          equals new { qod.EmployeeId, qod.DayId, qod.Date } into qdpod
                                          from qod in qdpod.DefaultIfEmpty()
                                          select new DayPlanData()
                                          {
                                              EmployeeId = qdpt.EmployeeId,
                                              EmployeeCode = qdpt.EmployeeCode,
                                              DayId = qdpt.DayId,
                                              Date = qdpt.Date,
                                              TargetSalesAmount = qdpt.TargetSalesAmount,
                                              TargetCollectionAmount = qdpt.TargetCollectionAmount,
                                              TargetDealerAppt = qdpt.TargetDealerAppt,
                                              TargetDemoActivity = qdpt.TargetDemoActivity,
                                              TargetVigoreSales = qdpt.TargetVigoreSales,
                                              ActualSalesAmount = qod == null ? 0 : qod.ActualSalesAmount
                                              //ActualCollectionAmount = qpd.ActualCollectionAmount
                                          }).ToList();

            //Append to orderPaymentCollection with items that had no collection or no order in a day for employees in the applied filters
            //orderPaymentCollection.AddRange(collectionsWithNoOrders);
            //orderPaymentCollection.AddRange(ordersWithNoCollections);

            return dayPlanOrderCollection;
        }

        private static IEnumerable<DayPlanData> GetConsolidatedDayPlanOrderPaymentData(IEnumerable<DayPlanData> queryConsolidatedOrderData, List<OrderPaymentGroupData> queryPaymentData, SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            //Get Orders and Collections items for a day and employee in the applied filters
            var dayPlanOrderPaymentCollection = (from qdpt in queryConsolidatedOrderData
                                                 join qod in queryPaymentData on
                                                    new { EmployeeId = qdpt.EmployeeId, DayId = qdpt.DayId, Date = qdpt.Date }
                                                                          equals new { qod.EmployeeId, qod.DayId, qod.Date } into qdpod
                                                 from qod in qdpod.DefaultIfEmpty()
                                                 select new DayPlanData()
                                                 {
                                                     EmployeeId = qdpt.EmployeeId,
                                                     EmployeeCode = qdpt.EmployeeCode,
                                                     DayId = qdpt.DayId,
                                                     Date = qdpt.Date,
                                                     TargetSalesAmount = qdpt.TargetSalesAmount,
                                                     TargetCollectionAmount = qdpt.TargetCollectionAmount,
                                                     TargetDealerAppt = qdpt.TargetDealerAppt,
                                                     TargetDemoActivity = qdpt.TargetDemoActivity,
                                                     TargetVigoreSales = qdpt.TargetVigoreSales,
                                                     ActualSalesAmount = qdpt.ActualSalesAmount,
                                                     ActualCollectionAmount = qod == null ? 0 : qod.ActualCollectionAmount,
                                                     ActualDealerAppt = orm.Activities.Where(x => x.EmployeeDay.DayId == qdpt.DayId
                                                                                         && x.EmployeeDay.TenantEmployeeId == qdpt.EmployeeId
                                                                                         && x.ActivityType == Constant.NewDealerAppointment).Count(),
                                                     ActualDemoActivity = orm.Activities.Where(x => x.EmployeeDay.DayId == qdpt.DayId
                                                                                         && x.EmployeeDay.TenantEmployeeId == qdpt.EmployeeId
                                                                                         && x.ActivityType == Constant.DemoActivity).Count(),
                                                     ActualVigoreSales = 0
                                                 }).ToList();

            IEnumerable<DayPlanData> rm = dayPlanOrderPaymentCollection.AsEnumerable();

            rm = ApplyTargetStatusFilters(rm, searchCriteria);

            return rm;
        }

        //private static IEnumerable<DayPlanData> AppendActualsZeroItemsDayPlanTarget(IEnumerable<DayPlanData> queryDayPlanTargetData,
        //                                                                        IEnumerable<DayPlanData> queryTotalsData, SearchCriteria searchCriteria)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;

        //    List<DayPlanData> collectDayPlanTargetNotInOrdersPayments = new List<DayPlanData>();

        //    List<DayPlanData> source = queryDayPlanTargetData.ToList();

        //    List<DayPlanData> reportModel = queryTotalsData.ToList();

        //    var dayPlanTargetDataNotInOrdersPayments =
        //                    source.Where(dpt => reportModel.All(qtd => (dpt.DayId != qtd.DayId || dpt.EmployeeId != qtd.EmployeeId))).ToList();

        //    if (dayPlanTargetDataNotInOrdersPayments.Count() > 0)
        //    {
        //        collectDayPlanTargetNotInOrdersPayments = (from dptNOP in dayPlanTargetDataNotInOrdersPayments
        //                                                   select new DayPlanData()
        //                                                   {
        //                                                      EmployeeId = dptNOP.EmployeeId,
        //                                                      EmployeeCode = dptNOP.EmployeeCode,
        //                                                      DayId = dptNOP.DayId,
        //                                                      Date = dptNOP.Date,
        //                                                      TargetSalesAmount = dptNOP.TargetSalesAmount,
        //                                                      TargetCollectionAmount = dptNOP.TargetCollectionAmount,
        //                                                      TargetVigoreSales = dptNOP.TargetVigoreSales,
        //                                                      TargetDealerAppt = dptNOP.TargetDealerAppt,
        //                                                      ActualDealerAppt = orm.Activities.Where(x => x.EmployeeDay.DayId == dptNOP.DayId
        //                                                                          && x.EmployeeDay.TenantEmployeeId == dptNOP.EmployeeId
        //                                                                          && x.ActivityType == Constant.NewDealerAppointment).Count(),
        //                                                      ActualVigoreSales = orm.Orders.Join(orm.OrderItems,
        //                                                                                  o => o.Id,
        //                                                                                  oi => oi.OrderId,
        //                                                                                      (o, oi) => new
        //                                                                                      {
        //                                                                                          o.EmployeeId,
        //                                                                                          o.DayId,
        //                                                                                          oi.ProductCode,
        //                                                                                          oi.UnitQuantity
        //                                                                                      }).Where(x => x.DayId == dptNOP.DayId
        //                                                                                         && x.EmployeeId == dptNOP.EmployeeId
        //                                                                                         && x.ProductCode == Constant.Vigore).Select(s => s.UnitQuantity).DefaultIfEmpty(0).Sum()
        //                                                  }).ToList();
        //        reportModel.AddRange(collectDayPlanTargetNotInOrdersPayments);
        //    }

        //    IEnumerable<DayPlanData> rm = reportModel.AsEnumerable();

        //    rm = ApplyTargetStatusFilters(rm, searchCriteria);

        //    return rm;
        //}

        /// <summary>
        /// Author: Pankaj Kumar; Date: 29-04-2021; Purpose: Get Day Plan Data
        /// </summary>
        /// <param name="queryOrderData"></param>
        /// <param name="queryPaymentData"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        //private static IEnumerable<DayPlanData> GetTotalsData(List<OrderPaymentGroupData> queryConsolidatedData)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;
        //    var reportModel = from qod in queryConsolidatedData
        //                    select new DayPlanData()
        //                    {
        //                        EmployeeId = qod.EmployeeId,
        //                        EmployeeCode = orm.DayPlanTarget.Where(x => x.EmployeeId == qod.EmployeeId
        //                                                                    && x.DayId == qod.DayId)
        //                                                            .Select(l => l.EmployeeCode).FirstOrDefault(),
        //                        DayId = qod.DayId,
        //                        Date = qod.Date,
        //                        TargetSalesAmount = orm.DayPlanTarget.Where(x => x.EmployeeId == qod.EmployeeId
        //                                                                    && x.DayId == qod.DayId)
        //                                                            .Sum(l => l.TargetSales),
        //                        TargetCollectionAmount = orm.DayPlanTarget.Where(x => x.EmployeeId == qod.EmployeeId
        //                                                                    && x.DayId == qod.DayId)
        //                                                            .Sum(l => l.TargetCollection),
        //                        TargetVigoreSales = orm.DayPlanTarget.Where(x => x.EmployeeId == qod.EmployeeId
        //                                                                    && x.DayId == qod.DayId)
        //                                                            .Sum(l => l.TargetVigoreSales),
        //                        TargetDealerAppt = orm.DayPlanTarget.Where(x => x.EmployeeId == qod.EmployeeId
        //                                                                   && x.DayId == qod.DayId)
        //                                                            .Sum(l => l.TargetDealerAppointment),
        //                        ActualSalesAmount = qod.ActualSalesAmount,
        //                        ActualCollectionAmount = qod.ActualCollectionAmount,
        //                        ActualDealerAppt = orm.Activities.Where(x => x.EmployeeDay.DayId == qod.DayId
        //                                            && x.EmployeeDay.TenantEmployeeId == qod.EmployeeId
        //                                            && x.ActivityType == Constant.NewDealerAppointment).Count(),

        //                        ActualVigoreSales = orm.Orders.Join(orm.OrderItems,
        //                                                    o => o.Id,
        //                                                    oi => oi.OrderId,
        //                                                        (o, oi) => new
        //                                                        {
        //                                                            o.EmployeeId,
        //                                                            o.DayId,
        //                                                            oi.ProductCode,
        //                                                            oi.UnitQuantity
        //                                                        }).Where(x => x.DayId == qod.DayId
        //                                                           && x.EmployeeId == qod.EmployeeId
        //                                                           && x.ProductCode == Constant.Vigore).Select(s => s.UnitQuantity).DefaultIfEmpty(0).Sum()
        //                    };

        //    return reportModel;
        //}

        //private static List<OrderPaymentGroupData> GetConsolidatedData(List<OrderPaymentGroupData> queryOrderData,
        //                                              List<OrderPaymentGroupData> queryPaymentData)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;

        //    //Get Orders for a day with no collections for a day and employees in the applied filters
        //    var ordersWithNoCollections = queryOrderData.Where(o => queryPaymentData.All(p => p.DayId != o.DayId && p.EmployeeId != o.EmployeeId && p.Date != o.Date)).ToList();

        //    //Get Payments for a day with no fresh orders for a day and employee in the applied filters
        //    var collectionsWithNoOrders = queryPaymentData.Where(p => queryOrderData.All(o => o.DayId != p.DayId && p.EmployeeId != o.EmployeeId && o.Date != p.Date)).ToList();

        //    //Get Orders and Collections items for a day and employee in the applied filters
        //    var orderPaymentCollection = (from qod in queryOrderData
        //                                  join qpd in queryPaymentData on
        //                                      new { EmployeeId = qod.EmployeeId, DayId = qod.DayId, Date = qod.Date }
        //                                                                  equals new { qpd.EmployeeId, qpd.DayId, qpd.Date }
        //                                  select new OrderPaymentGroupData()
        //                                  {
        //                                      EmployeeId = qod.EmployeeId,
        //                                      DayId = qod.DayId,
        //                                      Date = qod.Date,
        //                                      ActualSalesAmount = qod.ActualSalesAmount,
        //                                      ActualCollectionAmount = qpd.ActualCollectionAmount
        //                                  }).ToList();

        //    //Append to orderPaymentCollection with items that had no collection or no order in a day for employees in the applied filters
        //    orderPaymentCollection.AddRange(collectionsWithNoOrders);
        //    orderPaymentCollection.AddRange(ordersWithNoCollections);

        //    return orderPaymentCollection;
        //}

        /// <summary>
        /// Author: Pankaj Kumar; Date: 29-04-2021; Purpose: Get Day Plan Data
        /// </summary>
        /// <param name="reportModel"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        private static IEnumerable<DayPlanData> ApplyTargetStatusFilters(IEnumerable<DayPlanData> reportModel,
                                                                                SearchCriteria searchCriteria)
        {
            switch (searchCriteria.TargetStatus)
            {
                case Constant.Achieved:
                    reportModel = reportModel.Where(x => x.ActualSalesAmount >= x.TargetSalesAmount
                                                    || x.ActualCollectionAmount >= x.TargetCollectionAmount
                                                    || x.ActualDealerAppt >= x.TargetDealerAppt
                                                    || x.ActualDemoActivity >= x.TargetDemoActivity);
                    //|| x.ActualVigoreSales >= x.TargetVigoreSales
                    reportModel = ApplyPlanTypeFilters(reportModel, searchCriteria);
                    break;

                case Constant.NotAchieved:
                    reportModel = reportModel.Where(x => x.ActualSalesAmount < x.TargetSalesAmount
                                                    || x.ActualCollectionAmount < x.TargetCollectionAmount
                                                    || x.ActualDealerAppt < x.TargetDealerAppt);
                    //|| x.ActualVigoreSales < x.TargetVigoreSales);
                    reportModel = ApplyPlanTypeFilters(reportModel, searchCriteria);
                    break;
                default:
                    reportModel = ApplyPlanTypeFilters(reportModel, searchCriteria);
                    break;
            }
            return reportModel;
        }

        /// <summary>
        /// Author: Pankaj Kumar; Date: 29-04-2021; Purpose: Get Day Plan Data
        /// </summary>
        /// <param name="reportModel"></param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        private static IEnumerable<DayPlanData> ApplyPlanTypeFilters(IEnumerable<DayPlanData> reportModel,
                                                                                SearchCriteria searchCriteria)
        {
            if (!String.IsNullOrEmpty(searchCriteria.TargetStatus))
            {
                switch (searchCriteria.DayPlanType)
                {
                    case Constant.Sales:
                        return searchCriteria.TargetStatus.Equals(Constant.Achieved) ?
                                    reportModel.Where(x => x.ActualSalesAmount >= x.TargetSalesAmount) :
                                        reportModel.Where(x => x.ActualSalesAmount < x.TargetSalesAmount);
                    case Constant.Collection:
                        return searchCriteria.TargetStatus.Equals(Constant.Achieved) ?
                            reportModel.Where(x => x.ActualCollectionAmount >= x.TargetCollectionAmount) :
                                reportModel.Where(x => x.ActualCollectionAmount < x.TargetCollectionAmount);
                    case Constant.DealerAppointment:
                        return searchCriteria.TargetStatus.Equals(Constant.Achieved) ?
                            reportModel.Where(x => x.ActualDealerAppt >= x.TargetDealerAppt) :
                                reportModel.Where(x => x.ActualDealerAppt < x.TargetDealerAppt);
                    case Constant.Demonstration:
                        return searchCriteria.TargetStatus.Equals(Constant.Achieved) ?
                            reportModel.Where(x => x.ActualDemoActivity >= x.TargetDemoActivity) :
                                reportModel.Where(x => x.ActualDemoActivity < x.TargetDemoActivity);
                    //case Constant.VigoreSales:
                    //    return searchCriteria.TargetStatus.Equals(Constant.Achieved) ?
                    //        reportModel.Where(x => x.ActualVigoreSales >= x.TargetVigoreSales) :
                    //            reportModel.Where(x => x.ActualVigoreSales < x.TargetVigoreSales);
                    default:
                        return reportModel;
                }
            }
            else
            {
                switch (searchCriteria.DayPlanType)
                {
                    case Constant.Sales:
                        return reportModel.Where(x => x.ActualSalesAmount > 0 || x.TargetSalesAmount > 0);
                    case Constant.Collection:
                        return reportModel.Where(x => x.ActualCollectionAmount > 0 || x.TargetCollectionAmount > 0);
                    case Constant.DealerAppointment:
                        return reportModel.Where(x => x.ActualDealerAppt > 0 || x.TargetDealerAppt > 0);
                    case Constant.VigoreSales:
                        return reportModel.Where(x => x.ActualVigoreSales > 0 || x.TargetVigoreSales > 0);
                    default:
                        return reportModel;
                }

            }
        }

        /// <summary>
        /// Author: Pankaj Kumar; Date: 29-04-2021; Purpose: Get Day Plan Data
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <param name="dayIds"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private static List<OrderPaymentGroupData> GetOrderData(IEnumerable<long> employeeIds, IEnumerable<long> dayIds,
                                                    IEnumerable<DateTime> date)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var OrdersData = from dat in date
                             from e in employeeIds
                             from d in dayIds
                             join ord in orm.Orders on new { EmployeeId = e, DayId = d, OrderDate = dat }
                                                         equals new { ord.EmployeeId, ord.DayId, ord.OrderDate }
                             group ord by new
                             {
                                 ord.EmployeeId,
                                 ord.DayId,
                                 ord.OrderDate
                             } into grpOrders
                             select new OrderPaymentGroupData()
                             {
                                 EmployeeId = grpOrders.Key.EmployeeId,
                                 DayId = grpOrders.Key.DayId,
                                 Date = grpOrders.Key.OrderDate,
                                 ActualSalesAmount = grpOrders.Sum(t => t.NetAmount)
                             };

            return OrdersData.ToList();
        }

        /// <summary>
        /// Author: Pankaj Kumar; Date: 29-04-2021; Purpose: Get Day Plan Data
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <param name="dayIds"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private static List<OrderPaymentGroupData> GetPaymentData(IEnumerable<long> employeeIds, IEnumerable<long> dayIds,
                                                    IEnumerable<DateTime> date)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from dat in date
                    from e in employeeIds
                    from d in dayIds
                    join pyt in orm.Payments on new { EmployeeId = e, DayId = d, PaymentDate = dat }
                                                equals new { pyt.EmployeeId, pyt.DayId, pyt.PaymentDate }
                    group pyt by new
                    {
                        pyt.EmployeeId,
                        pyt.DayId,
                        pyt.PaymentDate
                    } into grpPayments
                    select new OrderPaymentGroupData()
                    {
                        EmployeeId = grpPayments.Key.EmployeeId,
                        DayId = grpPayments.Key.DayId,
                        Date = grpPayments.Key.PaymentDate,
                        ActualCollectionAmount = grpPayments.Sum(t => t.TotalAmount)
                    }).ToList();
        }

        private static IEnumerable<DayPlanData> GetDayPlanTargetDataFiltersApplied(IEnumerable<long> employeeIds, IEnumerable<long> dayIds,
                                                    IEnumerable<DateTime> date)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var dayPlanTargetList = (from dpt in orm.DayPlanTargets
                                     select dpt).ToList();

            var dayPlanTargetFilteredList = from dat in date
                                            from e in employeeIds
                                            from d in dayIds
                                            join dptfl in dayPlanTargetList
                                                on new { EmployeeId = e, DayId = d, PlanDate = dat }
                                                equals new { dptfl.EmployeeId, dptfl.DayId, dptfl.PlanDate }
                                            group dptfl by new
                                            {
                                                dptfl.TenantId,
                                                dptfl.EmployeeId,
                                                dptfl.DayId,
                                                dptfl.EmployeeCode,
                                                dptfl.PlanDate,
                                                //dptfl.TargetSales,
                                                //dptfl.TargetCollection,
                                                //dptfl.TargetDealerAppointment,
                                                //dptfl.TargetVigoreSales,
                                                //dptfl.SqliteDayPlanTargetId
                                            } into grpDPTFL
                                            select new DayPlanData()
                                            {
                                                EmployeeId = grpDPTFL.Key.EmployeeId,
                                                EmployeeCode = grpDPTFL.Key.EmployeeCode,
                                                DayId = grpDPTFL.Key.DayId,
                                                Date = grpDPTFL.Key.PlanDate,
                                                TargetSalesAmount = grpDPTFL.Sum(x => x.TargetSales),
                                                TargetCollectionAmount = grpDPTFL.Sum(x => x.TargetCollection),
                                                TargetDealerAppt = grpDPTFL.Sum(x => x.TargetDealerAppointment),
                                                TargetDemoActivity = grpDPTFL.Sum(x => x.TargetDemoActivity),
                                                TargetVigoreSales = grpDPTFL.Sum(x => x.TargetVigoreSales)
                                            };

            return dayPlanTargetFilteredList;
        }

        public static ICollection<ExpenseData> GetExpenseReportData(IEnumerable<long> employeeIds, IEnumerable<long> dayIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from e in employeeIds
                    from d in dayIds
                    join ed in orm.Expenses on new { EmployeeId = e, DayId = d } equals new { ed.EmployeeId, ed.DayId }
                    join ei in orm.ExpenseItems on ed.Id equals ei.ExpenseId
                    orderby ed.EmployeeId, ed.DayId, ei.ExpenseType
                    select new ExpenseData()
                    {
                        EmployeeId = ed.EmployeeId,
                        DayId = ed.DayId,
                        ExpenseType = ei.ExpenseType,
                        TransportType = ei.TransportType,
                        Amount = ei.Amount,
                        OdometerStart = ei.OdometerStart,
                        OdometerEnd = ei.OdometerEnd,
                        FuelType = ei.FuelType,
                        FuelQuantityInLiters = ei.FuelQuantityInLiters
                    }).ToList();
        }

        public static EmployeeDailyConsolidation GetConsolidatedData(long employeeId, long dayId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            // get total orders
            var orders = orm.Orders.Where(x => x.EmployeeId == employeeId && x.DayId == dayId).ToList();
            var returnOrders = orm.ReturnOrders.Where(x => x.EmployeeId == employeeId && x.DayId == dayId).ToList();
            var expenses = orm.Expenses.Where(x => x.EmployeeId == employeeId && x.DayId == dayId).ToList();
            var payments = orm.Payments.Where(x => x.EmployeeId == employeeId && x.DayId == dayId).ToList();
            var activityCount = orm.Activities.Where(x => x.EmployeeDay.DayId == dayId && x.EmployeeDay.TenantEmployeeId == employeeId).Count();
            var employeeRecord = orm.TenantEmployees.Find(employeeId);

            var staffCode = employeeRecord?.EmployeeCode ?? "";
            //var salesPersonRecord = orm.SalesPersons.Where(x => x.StaffCode == staffCode).FirstOrDefault();

            EmployeeDailyConsolidation dailyConsolidation = new EmployeeDailyConsolidation()
            {
                EmployeeId = employeeId,
                DayId = dayId,
                TotalOrderAmount = orders.Sum(x => x.TotalAmount),
                TotalReturnOrderAmount = returnOrders.Sum(x => x.TotalAmount),
                TotalExpenseAmount = expenses.Sum(x => x.TotalAmount),
                TotalPaymentAmount = payments.Sum(x => x.TotalAmount),
                ActivityCount = activityCount,

                StartPosition = "",
                EndPosition = "",
                TrackingDistanceInMeters = 0,

                StaffCode = staffCode,
            };

            // retrieve employeeDay record
            var employeeDay = orm.EmployeeDays.Where(x => x.TenantEmployeeId == employeeId && x.DayId == dayId).FirstOrDefault();
            if (employeeDay != null)
            {
                long employeeDayId = employeeDay.Id;
                var trackings = orm.Trackings.Where(x => x.EmployeeDayId == employeeDayId).OrderBy(x => x.Id).ToList();
                if (trackings != null && trackings.Count > 0)
                {
                    // for start day tracking record, start location is in endLocationName
                    dailyConsolidation.StartPosition = trackings.First().EndLocationName;
                    dailyConsolidation.EndPosition = trackings.Last().EndLocationName;

                    dailyConsolidation.StartTime = trackings.First().At;
                    dailyConsolidation.EndTime = trackings.Last().At;

                    dailyConsolidation.TrackingDistanceInMeters = trackings.Where(x => x.IsMilestone)
                                                            .Sum(x => x.GoogleMapsDistanceInMeters);
                }
            }

            return dailyConsolidation;
        }

        public static void TerminateUserAccess(long empId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var record = orm.TenantEmployees.Find(empId);
            if (record != null)
            {
                record.IsActive = false;
                record.DateUpdated = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void MarkBatchReadyForProcessing(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var batchRecord = orm.SqliteActionBatches.Find(batchId);
            if (batchRecord != null)
            {
                batchRecord.UnderConstruction = false;
                orm.SaveChanges();
            }
        }

        public static void DisassociateUserOnPhone(long empId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var record = orm.TenantEmployees.Include("IMEIs").Where(x => x.Id == empId).FirstOrDefault();
            if (record != null && record.IMEIs != null)
            {
                // terminate exec app access as well
                // (Don't want to do this, as when user signs up on another phone, admin
                //  should not have to check Exec App Access for this user)
                //if (record.ExecAppAccess)
                //{
                //    record.ExecAppAccess = false;
                //    record.DateUpdated = DateTime.UtcNow;
                //}

                foreach (var imei in record.IMEIs)
                {
                    if (imei.IsActive)
                    {
                        imei.IsActive = false;
                        imei.DateUpdated = DateTime.UtcNow;
                    }
                }
                orm.SaveChanges();
            }
        }

        public static ICollection<SignedInEmployeeData> GetSignedInEmployeeData(DateTime inputDate)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var resultSet = (IEnumerable<GetSignedInEmployeeData_Result>)orm.GetSignedInEmployeeData(inputDate);
            if (resultSet == null)
            {
                return null;
            }
            return resultSet.Select(item => new SignedInEmployeeData()
            {
                TrackingRecordId = item.TrackingRecordId,
                TrackingTime = item.TrackingTime,
                EmployeeId = item.EmployeeId,
                EmployeeDayId = item.EmployeeDayId,
                EmployeeName = item.EmployeeName,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                EmployeeCode = item.EmployeeCode
            }).ToList();
        }

        public static ICollection<DashboardProduct> GetProductData(ProductsFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (x) => DBLayer.LogError("GetProductData", x, "");

            var allProducts = orm.Products.AsQueryable();

            if (searchCriteria?.ApplyProductCodeFilter ?? false)
            {
                allProducts = allProducts.Where(p => p.ProductCode.Contains(searchCriteria.ProductCode));
            }

            if (searchCriteria?.ApplyNameFilter ?? false)
            {
                allProducts = allProducts.Where(p => p.Name.Contains(searchCriteria.Name));
            }

            if (searchCriteria?.ApplyStatusFilter ?? false)
            {
                allProducts = allProducts.Where(p => p.IsActive == searchCriteria.Status);
            }

            //allProducts = allProducts.Where(x => x.ProductPrices.Any(y => searchCriteria.FilteringAreaCodes.Any(fac => fac == y.AreaCode)));

            Stopwatch sw = new Stopwatch();

            sw.Reset();
            sw.Start();

            List<DashboardProduct> products = allProducts
                           .Select(ap => new DashboardProduct()
                           {
                               Id = ap.Id,
                               ProductCode = ap.ProductCode,
                               Name = ap.Name,
                               GroupName = ap.ProductGroup.GroupName,
                               IsActive = ap.IsActive,

                               ShelfLifeInMonths = ap.ShelfLifeInMonths,
                               UOM = ap.UOM,
                               BrandName = ap.BrandName,
                               GstCode = ap.GstCode,
                               //Prices = new List<DashboardProductPrice>(),
                           })
                           .Take(searchCriteria.MaxResultCount)
                           .OrderBy(x => x.Name)
                           .ToList();
            sw.Stop();
            LogError($"{nameof(GetProductData)}", $"Getting {products.Count()} products from DB took {sw.ElapsedMilliseconds / 1000} seconds [{sw.ElapsedMilliseconds} ms]", "");

            // now fill price
            if (searchCriteria.ApplyAreaFilter)
            {
                sw.Reset();
                sw.Start();
                ICollection<ProductPrice2> productPriceForArea = DBLayer.GetProductPrice2(searchCriteria.Area);
                sw.Stop();
                LogError($"{nameof(GetProductData)}", $"Getting {productPriceForArea.Count} price data from DB took {sw.ElapsedMilliseconds / 1000} seconds [{sw.ElapsedMilliseconds} ms]", "");

                sw.Reset();
                sw.Start();
                products = (from p in products
                            join pp in productPriceForArea on p.Id equals pp.ProductId into outerPP
                            from t in outerPP.DefaultIfEmpty()
                            select new DashboardProduct()
                            {
                                Id = p.Id,
                                ProductCode = p.ProductCode,
                                Name = p.Name,
                                UOM = p.UOM,
                                BrandName = p.BrandName,
                                ShelfLifeInMonths = p.ShelfLifeInMonths,
                                IsActive = p.IsActive,
                                GroupName = p.GroupName,
                                GstCode = p.GstCode,
                                Prices = (t == null) ? null : new List<DashboardProductPrice>()
                              {
                                  new DashboardProductPrice()
                                  {
                                      Id = t?.Id ?? 0,
                                      AreaCode = searchCriteria.Area,
                                      Stock = t?.Stock ?? 0,
                                      MRP = t?.MRP ?? 0,
                                      DEALERPrice = t?.DEALERPrice ?? 0,
                                      DISTPrice = t?.DISTPrice ?? 0,
                                      PDISTPrice = t?.PDISTPrice ?? 0,
                                      ProductId = p.Id
                                  }
                              }
                            }).ToList();

                //Parallel.ForEach(products, p =>
                //{
                //    var pp = productPriceForArea.Where(x => x.ProductId == p.Id).FirstOrDefault();
                //    p.Prices = new List<DashboardProductPrice>();
                //    if (pp != null)
                //    {
                //        p.Prices.Add(new DashboardProductPrice()
                //        {
                //            AreaCode = searchCriteria.Area,
                //            Id = pp.Id,
                //            MRP = pp.MRP,
                //            Stock = pp.Stock,
                //            ProductId = pp.ProductId,
                //            DEALERPrice = pp.DEALERPrice,
                //            DISTPrice = pp.DISTPrice,
                //            PDISTPrice = pp.PDISTPrice
                //        });
                //    }
                //});
                sw.Stop();
                LogError($"{nameof(GetProductData)}", $"compiling Product / price data took {sw.ElapsedMilliseconds / 1000} seconds [{sw.ElapsedMilliseconds} ms]", "");
            }

            return products;
        }

        public static void ProcessSqlitePaymentData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqlitePaymentData(batchId);
        }

        public static void ProcessSqliteOrderData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteOrderData(batchId);
        }

        public static void ProcessSqliteReturnOrderData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteReturnOrderData(batchId);
        }

        public static void ProcessSqliteEntityData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteEntityData(batchId);
        }

        public static void ProcessSqliteBankDetailsData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteBankDetailsData(batchId);
        }

        public static void ProcessSqliteAgreementData(long batchId, string agreementDefaultStatus)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteAgreementData(batchId, agreementDefaultStatus);
        }

        public static void ProcessSqliteSurveyData(long batchId, string surveyDefaultStatus)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteSurveyData(batchId, surveyDefaultStatus);
        }

        public static void ProcessSqliteDayPlanTargetData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteDayPlanTargetData(batchId);
        }

        //Process FollowUpTask
        public static void ProcessSqliteTaskData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteTaskData(batchId);
        }

        // Process FollowUpTask Action

        public static void ProcessSqliteTaskActionData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteTaskActionData(batchId);
        }

        // Process Dealer Questionnaire - June 24 2021 ; Author : Rajesh/Vasanth
        public static void ProcessSqliteQuestionnaireData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteQuestionnaireData(batchId);
        }

        public static void ProcessSqliteIssueReturnData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteIssueReturnData(batchId);
        }

        public static void ProcessSqliteAdvanceRequestData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteAdvanceRequestData(batchId);
        }

        public static string ImageData(long expenseId, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var imageRecord = orm.SqliteExpenseImages.AsNoTracking()
                    .Where(x => x.SqliteExpenseId == expenseId)
                    .OrderBy(x => x.Id)
                    .Skip(imageItem - 1)
                    .FirstOrDefault();
                return (imageRecord != null) ? imageRecord.ImageFileName : null;
            }
        }

        public static string PaymentImageData(long id, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var imageRecord = orm.SqlitePaymentImages.AsNoTracking()
                    .Where(x => x.SqlitePaymentId == id)
                    .OrderBy(x => x.Id)
                    .Skip(imageItem - 1)
                    .FirstOrDefault();
                return (imageRecord != null) ? imageRecord.ImageFileName : null;
            }
        }

        public static string ActivityImageData(long id, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var imageRecord = orm.SqliteActionImages.AsNoTracking()
                    .Where(x => x.SqliteActionId == id)
                    .OrderBy(x => x.Id)
                    .Skip(imageItem - 1)
                    .FirstOrDefault();
                return (imageRecord != null) ? imageRecord.ImageFileName : null;
            }
        }

        public static string EntityImageData(long id, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var entityImage = orm.EntityImages.AsNoTracking()
                    .Where(x => x.EntityId == id && x.SequenceNumber == imageItem)
                    .FirstOrDefault();

                return (entityImage != null) ? entityImage.Image.ImageFileName : null;
            };
        }

        public static string STRImageData(long id, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var entityImage = orm.STRImages.AsNoTracking()
                    .Where(x => x.STRId == id && x.SequenceNumber == imageItem)
                    .FirstOrDefault();

                return (entityImage != null) ? entityImage.ImageFileName : null;
            };
        }

        public static string EntityBankDetailImageData(long id, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                return orm.EntityBankDetailImages.AsNoTracking()
                    .Where(x => x.EntityBankDetailId == id && x.SequenceNumber == imageItem)
                    .Select(x => x.ImageFileName)
                    .FirstOrDefault();
            };
        }

        public static string PaymentImage(long paymentId, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var paymentImage = orm.PaymentImages.AsNoTracking()
                    .Where(x => x.PaymentId == paymentId && x.SequenceNumber == imageItem)
                    .FirstOrDefault();

                return (paymentImage != null) ? paymentImage.Image.ImageFileName : null;
            };
        }

        public static string ActivityImage(long activityId, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var activityImage = orm.ActivityImages.AsNoTracking()
                    .Where(x => x.ActivityId == activityId && x.SequenceNumber == imageItem)
                    .FirstOrDefault();

                return (activityImage != null) ? activityImage.Image.ImageFileName : null;
            };
        }

        public static string ExpenseItemImage(long expenseItemId, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var expenseItemImage = orm.ExpenseItemImages.AsNoTracking()
                    .Where(x => x.ExpenseItemId == expenseItemId && x.SequenceNumber == imageItem)
                    .FirstOrDefault();

                return (expenseItemImage != null) ? expenseItemImage.Image.ImageFileName : null;
            };
        }

        public static string OrderImage(long orderId, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var oi = orm.OrderImages.AsNoTracking()
                    .Where(x => x.OrderId == orderId && x.SequenceNumber == imageItem)
                    .FirstOrDefault();

                return oi?.Image?.ImageFileName;
            };
        }

        public static EmployeeDayData GetEmployeeDayData(long empDayId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EmployeeDays.AsNoTracking().Where(x => x.Id == empDayId).Select(x => new EmployeeDayData()
            {
                Date = x.Day.DATE,
                EmployeeDayId = empDayId,
                EmployeeId = x.TenantEmployeeId,
                EmployeeName = x.TenantEmployee.Name
            }).FirstOrDefault();
        }

        public static EmployeeDayData GetEmployeeDayData(long empId, long dayId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EmployeeDays.AsNoTracking().Where(x => x.TenantEmployeeId == empId && x.DayId == dayId)
                .Select(x => new EmployeeDayData()
                {
                    Date = x.Day.DATE,
                    EmployeeDayId = x.Id,
                    EmployeeId = x.TenantEmployeeId,
                    EmployeeName = x.TenantEmployee.Name,
                    EndTime = x.EndTime
                }).FirstOrDefault();
        }

        public static IEnumerable<DomainEntities.SqliteActionProcessLog> GetDataFeedProcessLog(int startItem, int itemCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.DataFeedProcessLogs
                .OrderByDescending(x => x.At)
                .Skip(startItem - 1)
                .Take(itemCount)
                .Select(x => new DomainEntities.SqliteActionProcessLog()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    LockAcquiredStatus = x.LockAcquiredStatus,
                    At = x.At,
                    Timestamp = x.Timestamp,
                    HasCompleted = x.HasCompleted,
                    HasFailed = x.HasFailed,
                    ProcessName = x.ProcessName
                }).ToList();
        }

        public static IEnumerable<DomainEntities.SqliteActionProcessLog> GetSmsJobLog(int startItem, int itemCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SMSProcessLogs
                .OrderByDescending(x => x.At)
                .Skip(startItem - 1)
                .Take(itemCount)
                .Select(x => new DomainEntities.SqliteActionProcessLog()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    LockAcquiredStatus = x.LockAcquiredStatus,
                    At = x.At,
                    Timestamp = x.Timestamp,
                    HasCompleted = x.HasCompleted,
                    HasFailed = x.HasFailed,
                }).ToList();
        }

        public static IEnumerable<SalesPersonModel> GetSalesPersonData(StaffFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            // orm.Database.Log = (x) => DBLayer.LogError("GetSalesPersonData", x, "");
            IQueryable<SalesPersonModel> staff = (from sp in orm.SalesPersons
                                                  join te in orm.TenantEmployees on sp.StaffCode equals te.EmployeeCode into outer
                                                  from t in outer.DefaultIfEmpty()
                                                  join webUser in orm.AspNetUsers on sp.StaffCode equals webUser.UserName into outer2
                                                  from u in outer2.DefaultIfEmpty()
                                                  orderby sp.Name
                                                  select new SalesPersonModel()
                                                  {
                                                      Id = sp.Id,
                                                      Name = sp.Name,
                                                      Phone = sp.Phone,
                                                      StaffCode = sp.StaffCode,
                                                      IsActive = sp.IsActive,
                                                      EmployeeId = (t == null || t.IsActive == false) ? 0 : t.Id,
                                                      OnWeb = (u != null),
                                                      HQCode = sp.HQCode,
                                                      Grade = sp.Grade ?? "",
                                                      Department = sp.Department,
                                                      Designation = sp.Designation,
                                                      Ownership = sp.OwnershipType,
                                                      VehicleType = sp.VehicleType,
                                                      FuelType = sp.FuelType,
                                                      VehicleNumber = sp.VehicleNumber,
                                                      BusinessRole = sp.BusinessRole,
                                                      OverridePrivateVehicleRatePerKM = sp.OverridePrivateVehicleRatePerKM,
                                                      TwoWheelerRatePerKM = sp.TwoWheelerRatePerKM,
                                                      FourWheelerRatePerKM = sp.FourWheelerRatePerKM
                                                  });

            if (searchCriteria != null)
            {
                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    staff = staff.Where(s => s.StaffCode.Contains(searchCriteria.EmployeeCode));
                }

                if (searchCriteria.ApplyNameFilter)
                {
                    staff = staff.Where(s => s.Name.ToLower().Trim().Contains(searchCriteria.Name));
                }

                if (searchCriteria.ApplyPhoneFilter)
                {
                    staff = staff.Where(s => s.Phone.Contains(searchCriteria.Phone));
                }

                if (searchCriteria.ApplyGradeFilter)
                {
                    staff = staff.Where(s => s.Grade.Equals(searchCriteria.Grade));
                }

                if (searchCriteria.ApplyStatusFilter)
                {
                    staff = staff.Where(s => s.IsActive == searchCriteria.Status);
                }

                if (searchCriteria.ApplyAssociationFilter)
                {

                    var associationActiveStaffs = from spa in orm.SalesPersonAssociations
                                                  where spa.IsDeleted == false
                                                  group spa by spa.StaffCode into g
                                                  select new
                                                  {
                                                      StaffCode = g.Key
                                                  };
                    if (searchCriteria.Association)
                    {
                        staff = staff.Where(x => associationActiveStaffs.Any(s => s.StaffCode.Equals(x.StaffCode)));
                    }
                    else
                    {
                        staff = staff.Where(x => !associationActiveStaffs.Any(s => s.StaffCode.Equals(x.StaffCode)));
                    }
                }

                if (searchCriteria.ApplyDepartmentFilter)
                {
                    staff = staff.Where(s => s.Department.Equals(searchCriteria.Department));
                }

                if (searchCriteria.ApplyDesignationFilter)
                {
                    staff = staff.Where(s => s.Designation.Equals(searchCriteria.Designation));
                }
            }
            return staff.ToList();
        }

        public static IEnumerable<string> GetSalesPersonGrades()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from s in orm.SalesPersons
                    where s.Grade != null
                    select s.Grade).Distinct().ToList();
        }

        /// <summary>
        /// Similar to GetSalesPersonData, but returns data only for given staff codes
        /// </summary>
        /// <param name="inputStaffCodes"></param>
        /// <returns></returns>
        public static IEnumerable<SalesPersonModel> GetSelectedSalesPersonData(IEnumerable<string> inputStaffCodes)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var outputList = (from sp in orm.SalesPersons.Where(x => inputStaffCodes.Any(y => y.Equals(x.StaffCode, StringComparison.OrdinalIgnoreCase)))
                              join te in orm.TenantEmployees on sp.StaffCode equals te.EmployeeCode into outer
                              from t in outer.DefaultIfEmpty()
                              join webUser in orm.AspNetUsers on sp.StaffCode equals webUser.UserName into outer2
                              from u in outer2.DefaultIfEmpty()
                              orderby sp.Name
                              select new SalesPersonModel()
                              {
                                  Id = sp.Id,
                                  Name = sp.Name,
                                  Phone = sp.Phone,
                                  StaffCode = sp.StaffCode,
                                  IsActive = sp.IsActive,
                                  EmployeeId = (t == null) ? 0 : t.Id,
                                  OnWeb = (u != null),
                                  HQCode = sp.HQCode,
                                  Grade = sp.Grade,
                                  TenantEmployeeExist = (t != null),
                                  TenantEmployeeIsActive = (t == null) ? false : t.IsActive,
                                  Department = sp.Department,
                                  Designation = sp.Designation,
                                  Ownership = sp.OwnershipType,
                                  VehicleType = sp.VehicleType,
                                  FuelType = sp.FuelType,
                                  VehicleNumber = sp.VehicleNumber,
                                  BusinessRole = sp.BusinessRole,
                                  OverridePrivateVehicleRatePerKM = sp.OverridePrivateVehicleRatePerKM,
                                  TwoWheelerRatePerKM = sp.TwoWheelerRatePerKM,
                                  FourWheelerRatePerKM = sp.FourWheelerRatePerKM
                              }).ToList();

            return outputList;
        }

        public static SqliteDomainBatch GetSqliteBatch(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteActionBatches.Where(x => x.Id == batchId).Select(b => new SqliteDomainBatch()
            {
                Id = b.Id,
                BatchGuid = b.BatchGuid,
                EmployeeId = b.EmployeeId,
                EmployeeName = "", // here we are not joining with TenantEmployee - hence leaving it empty
                BatchInputCount = b.BatchInputCount,
                BatchSavedCount = b.BatchSavedCount,
                DuplicateItemCount = b.DuplicateItemCount,
                ExpenseLineInputCount = b.ExpenseLineInputCount,
                ExpenseLineSavedCount = b.ExpenseLineSavedCount,
                ExpenseLineRejectCount = b.ExpenseLineRejectCount,
                At = b.At,
                Timestamp = b.Timestamp,
                BatchProcessed = b.BatchProcessed,
                LockTimestamp = b.LockTimestamp,
                TotalExpenseAmount = b.TotalExpenseAmount,
                ExpenseDate = b.ExpenseDate,
                DataFileName = b.DataFileName,
                DataFileSize = b.DataFileSize
            }).FirstOrDefault();
        }

        public static long GetSqliteBatchId(long employeeId, string batchGuid)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.SqliteActionBatches
                    .FirstOrDefault(x => x.EmployeeId == employeeId && x.BatchGuid == batchGuid);

            return rec?.Id ?? 0;
        }

        public static void ProcessSqliteExpenseData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ProcessSqliteExpenseData(batchId);
        }

        public static int ApproveExpense(ApprovalData approvalData, SalesPersonLevel level)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var expense = orm.Expenses.Find(approvalData.Id);
                if (expense == null)
                {
                    return 2; //Invalid expense Id
                }

                if ((level == SalesPersonLevel.Territory && expense.IsTerritoryApproved) ||
                    (level == SalesPersonLevel.Area && expense.IsAreaApproved) ||
                    (level == SalesPersonLevel.Zone && expense.IsZoneApproved))
                {
                    return 0; //expense already approved
                }

                switch (level)
                {
                    case SalesPersonLevel.Territory:
                        expense.IsTerritoryApproved = true;
                        expense.Timestamp = DateTime.UtcNow;
                        break;
                    case SalesPersonLevel.Area:
                        expense.IsTerritoryApproved = true;
                        expense.IsAreaApproved = true;
                        expense.Timestamp = DateTime.UtcNow;
                        break;
                    case SalesPersonLevel.Zone:
                        expense.IsTerritoryApproved = true;
                        expense.IsAreaApproved = true;
                        expense.IsZoneApproved = true;
                        expense.Timestamp = DateTime.UtcNow;
                        break;
                    default:
                        return 5;
                }

                DBModel.ExpenseApproval ea = new DBModel.ExpenseApproval()
                {
                    ApproveLevel = level.ToString(),
                    ApproveDate = approvalData.ApprovedDate,
                    ApproveRef = approvalData.ApproveRef,
                    ApproveNotes = approvalData.ApproveComments,
                    ApproveAmount = approvalData.ApprovedAmt,
                    ApprovedBy = approvalData.ApprovedBy,
                    Timestamp = DateTime.UtcNow
                };
                expense.ExpenseApprovals.Add(ea);
                orm.SaveChanges();

                return 1; //Success
            }
        }

        public static IEnumerable<DomainEntities.ErrorLog> GetErrorLogData(int startItem, int itemCount, string processName)
        {
            EpicCrmEntities orm = DBLayer.GetNoLogOrm;
            if (string.IsNullOrEmpty(processName))
            {
                return orm.ErrorLogs.AsNoTracking().OrderByDescending(x => x.Id).Skip(startItem - 1).Take(itemCount)
                    .Select(x => new DomainEntities.ErrorLog()
                    {
                        Id = x.Id,
                        Process = x.Process,
                        LogText = x.LogText,
                        LogSnip = x.LogSnip,
                        At = x.At
                    }).ToList();
            }
            else
            {
                return orm.ErrorLogs.AsNoTracking()
                    .Where(x => x.Process == processName)
                    .OrderByDescending(x => x.Id).Skip(startItem - 1).Take(itemCount)
                    .Select(x => new DomainEntities.ErrorLog()
                    {
                        Id = x.Id,
                        Process = x.Process,
                        LogText = x.LogText,
                        LogSnip = x.LogSnip,
                        At = x.At
                    }).ToList();
            }
        }

        public static IEnumerable<DomainEntities.SmsLog> GetSmsLogData(int startItem, int itemCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TenantSMSLogs.AsNoTracking().OrderByDescending(x => x.SMSDateTime).Skip(startItem - 1).Take(itemCount)
                .Select(x => new DomainEntities.SmsLog()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    TenantName = x.Tenant.Name,
                    SMSText = x.SMSText,
                    SMSDateTime = x.SMSDateTime,
                    SmsApiResponse = x.SMSApiResponse,
                    SmsType = x.SmsType,
                    SenderName = x.SenderName
                }).ToList();
        }

        public static IEnumerable<DomainEntities.PurgeDataLog> GetPurgeDataLog(int startItem, int itemCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.PurgeLogs
                .OrderByDescending(x => x.DateCreated)
                .Skip(startItem - 1)
                .Take(itemCount)
                .Select(x => new DomainEntities.PurgeDataLog()
                {
                    Id = x.Id,
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    ActionPurged = x.ActionPurged,
                    ActionDupPurged = x.ActionDupPurged,
                    ExpensePurged = x.ExpensePurged,
                    OrderPurged = x.OrderPurged,
                    PaymentPurged = x.PaymentPurged,
                    ReturnPurged = x.ReturnPurged,
                    EntityPurged = x.EntityPurged,
                    DateCreated = x.DateCreated
                }).ToList();
        }

        public static long CreateProcessLogEntry(long tenantId, bool lockStatus, string processName)
        {
            EpicCrmEntities orm = DBLayer.GetNoLogOrm;
            DateTime dt = DateTime.UtcNow;
            var logEntry = new DBModel.DataFeedProcessLog()
            {
                TenantId = tenantId,
                LockAcquiredStatus = lockStatus,
                At = dt,
                Timestamp = dt,
                HasCompleted = false,
                HasFailed = false,
                ProcessName = processName
            };

            orm.DataFeedProcessLogs.Add(logEntry);
            orm.SaveChanges();
            return logEntry.Id;
        }

        public static IEnumerable<SqlitePaymentData> GetSavedPayments(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqlitePayments.Where(x => x.BatchId == batchId)
                .Select(x => new SqlitePaymentData()
                {
                    Id = x.Id,
                    PhoneDbId = x.PhoneDbId,
                    BatchId = batchId,
                    EmployeeId = x.EmployeeId,
                    CustomerCode = x.CustomerCode,
                    PaymentType = x.PaymentType,
                    PaymentDate = x.PaymentDate,
                    TotalAmount = x.TotalAmount,
                    Comment = x.Comment,
                    ImageCount = x.ImageCount,
                    IsProcessed = x.IsProcessed,
                    PhoneActivityId = x.PhoneActivityId,
                    PaymentId = x.PaymentId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                }).ToList();
        }

        public static IEnumerable<SqliteEntityData> GetSavedSqliteEntities(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteEntities.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteEntityData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    PhoneDbId = x.PhoneDbId,
                    AtBusiness = x.AtBusiness,
                    //Consent = x.Consent, //Swetha Made change on 24-11-2021
                    EntityType = x.EntityType,
                    EntityName = x.EntityName,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    Pincode = x.Pincode,
                    LandSize = x.LandSize,
                    TimeStamp = x.TimeStamp,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    LocationTaskStatus = x.LocationTaskStatus,
                    LocationException = x.LocationException,
                    MNC = x.MNC,
                    MCC = x.MCC,
                    LAC = x.LAC,
                    CellId = x.CellId,
                    IsProcessed = x.IsProcessed,
                    EntityId = x.EntityId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    NumberOfContacts = x.ContactCount,
                    NumberOfCrops = x.CropCount,
                    NumberOfImages = x.ImageCount,
                    NumberOfLocations = x.LocationCount,
                    UniqueIdType = x.UniqueIdType,
                    UniqueId = x.UniqueId,
                    TaxId = x.TaxId,
                    DerivedLocSource = x.DerivedLocSource,
                    DerivedLatitude = x.DerivedLatitude,
                    DerivedLongitude = x.DerivedLongitude,

                    Locations = x.SqliteEntityLocations
                    .Where(k => k.IsGood && k.Latitude != 0 && k.Longitude != 0)
                    .Select(k => new SqliteEntityLocationData()
                    {
                        Id = k.Id,
                        SqliteEntityId = k.SqliteEntityId,
                        Source = k.Source,
                        Latitude = k.Latitude,
                        Longitude = k.Longitude,
                        LocationTaskStatus = k.LocationTaskStatus,
                        LocationException = k.LocationException,
                        IsGood = k.IsGood
                    }).ToList(),

                    FatherHusbandName = x.FatherHusbandName,
                    TerritoryCode = x.TerritoryCode,
                    TerritoryName = x.TerritoryName,
                    HQCode = x.HQCode,
                    HQName = x.HQName,
                    //MajorCrop = x.MajorCrop,
                    //LastCrop = x.LastCrop,
                    //WaterSource = x.WaterSource,
                    //SoilType = x.SoilType,
                    //SowingType = x.SowingType,
                    //SowingDate = x.SowingDate
                }).ToList();
        }

        public static IEnumerable<SqliteEntityContactData> GetSavedSqliteEntityContacts(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteEntityContacts.Where(x => x.SqliteEntityId == entityId)
                .Select(x => new SqliteEntityContactData()
                {
                    Id = x.Id,
                    SqliteEntityId = x.SqliteEntityId,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    IsPrimary = x.IsPrimary
                });
        }

        public static IEnumerable<SqliteEntityCropData> GetSavedSqliteEntityCrops(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteEntityCrops.Where(x => x.SqliteEntityId == entityId)
                .Select(x => new SqliteEntityCropData()
                {
                    Id = x.Id,
                    SqliteEntityId = x.SqliteEntityId,
                    Name = x.Name
                });
        }

        public static IEnumerable<SqliteLeaveData> GetSavedSqliteLeaves(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteLeaves.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteLeaveData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    PhoneDbId = x.PhoneDbId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    LeaveType = x.LeaveType,
                    LeaveReason = x.LeaveReason,
                    Comment = x.Comment,
                    IsProcessed = x.IsProcessed,
                    LeaveId = x.LeaveId,
                    DaysCountExcludingHolidays = x.DaysCountExcludingHolidays,
                    DaysCount = x.DaysCount,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                }).ToList();
        }

        public static IEnumerable<SqliteCancelledLeaveData> GetSavedSqliteCancelledLeaves(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteCancelledLeaves.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteCancelledLeaveData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    LeaveId = x.LeaveId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                }).ToList();
        }

        public static IEnumerable<SqliteEntityWorkFlowData> GetSavedSqliteWorkFlowData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteEntityWorkFlowV2.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteEntityWorkFlowData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    EntityId = x.EntityId,
                    EntityType = x.EntityType,
                    EntityName = x.EntityName,
                    AgreementId = x.AgreementId,
                    Agreement = x.Agreement,
                    EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                    TypeName = x.TypeName,
                    Phase = x.Phase,
                    FieldVisitDate = x.FieldVisitDate,
                    IsStarted = x.IsStarted,
                    Date = x.Date,
                    MaterialType = x.MaterialType,
                    MaterialQuantity = x.MaterialQuantity,
                    GapFillingRequired = x.GapFillingRequired,
                    GapFillingSeedQuantity = x.GapFillingSeedQuantity,
                    LaborCount = x.LaborCount,
                    PercentCompleted = x.PercentCompleted,
                    IsProcessed = x.IsProcessed,
                    EntityWorkFlowId = x.EntityWorkFlowId,
                    DateCreated = x.DateCreated,
                    Timestamp = x.Timestamp,
                    FollowUpCount = x.FollowUpCount,

                    BatchNumber = x.BatchNumber,
                    LandSize = x.LandSize,
                    DWSEntry = x.DWSEntry,
                    ItemCount = x.ItemCount, // Plant Count or Nipping Count
                    ItemsUsedCount = x.ItemsUsedCount,
                    YieldExpected = x.YieldExpected,
                    BagsIssued = x.BagsIssued,
                    HarvestDate = x.HarvestDate
                }).ToList();
        }

        public static IEnumerable<SqliteAgreementData> GetSavedAgreements(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteAgreements.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteAgreementData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    EntityAgreementId = x.EntityAgreementId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,

                    IsNewEntity = x.IsNewEntity,

                    EntityId = x.EntityId,
                    EntityName = x.EntityName,

                    SeasonName = x.SeasonName,
                    TypeName = x.TypeName,
                    Acreage = (double)x.Acreage,

                    TimeStamp = x.AgreementDate,
                    ParentReferenceId = x.ParentReferenceId,
                    ActivityId = x.ActivityId,
                    PhoneDbId = x.PhoneDbId,
                    TerritoryCode = x.TerritoryCode,
                    TerritoryName = x.TerritoryName,
                    HQCode = x.HQCode,
                    HQName = x.HQName
                }).ToList();
        }

        public static IEnumerable<SqliteSurveyData> GetSavedSurveys(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteSurveys.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteSurveyData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    EntitySurveyId = x.EntitySurveyId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,

                    IsNewEntity = x.IsNewEntity,

                    EntityId = x.EntityId,
                    EntityName = x.EntityName,

                    SeasonName = x.SeasonName,
                    SowingType = x.SowingType,
                    Acreage = (double)x.Acreage,
                    SowingDate = x.SowingDate,

                    MajorCrop = x.MajorCrop,
                    LastCrop = x.LastCrop,
                    SoilType = x.SoilType,
                    WaterSource = x.WaterSource,

                    TimeStamp = x.SurveyDate,
                    ParentReferenceId = x.ParentReferenceId,
                    ActivityId = x.ActivityId,
                    PhoneDbId = x.PhoneDbId
                }).ToList();
        }

        public static IEnumerable<SqliteTerminateAgreementEx> GetSavedTerminateAgreements(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteTerminateAgreements.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteTerminateAgreementEx()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    IsProcessed = x.IsProcessed,
                    TerminateAgreementId = x.TerminateAgreementId,
                    EntityId = x.EntityId,
                    EntityName = x.EntityName,
                    AgreementId = x.AgreementId,
                    Agreement = x.Agreement,
                    TimeStamp = x.RequestDate,
                    TypeName = x.TypeName,
                    Reason = x.Reason,
                    Notes = x.Notes,
                    ActivityId = x.ActivityId
                }).ToList();
        }

        public static IEnumerable<SqliteIssueReturnData> GetSavedSqliteIssueReturns(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteIssueReturns.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteIssueReturnData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    IssueReturnId = x.IssueReturnId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,

                    IsNewEntity = x.IsNewEntity,
                    IsNewAgreement = x.IsNewAgreement,

                    EntityId = x.EntityId,
                    EntityName = x.EntityName,
                    AgreementId = x.AgreementId,
                    Agreement = x.Agreement,

                    TranType = x.TranType,
                    ItemId = x.ItemId,
                    ItemCode = x.ItemCode,

                    SlipNumber = x.SlipNumber,
                    Acreage = (double)x.Acreage,

                    Quantity = x.Quantity,
                    TimeStamp = x.IssueReturnDate,
                    ParentReferenceId = x.ParentReferenceId,
                    ActivityId = x.ActivityId,
                    ItemRate = x.ItemRate
                }).ToList();
        }

        public static IEnumerable<SqliteAdvanceRequestData> GetSavedSqliteAdvanceRequests(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteAdvanceRequests.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteAdvanceRequestData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    AdvanceRequestId = x.AdvanceRequestId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,

                    IsNewEntity = x.IsNewEntity,
                    IsNewAgreement = x.IsNewAgreement,

                    EntityId = x.EntityId,
                    EntityName = x.EntityName,
                    AgreementId = x.AgreementId,
                    Agreement = x.Agreement,

                    Amount = (double)x.Amount,
                    Notes = x.Notes,
                    TimeStamp = x.AdvanceRequestDate,
                    ParentReferenceId = x.ParentReferenceId,
                    ActivityId = x.ActivityId
                }).ToList();
        }

        public static IEnumerable<SqliteSTRData> GetSavedSTRData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteSTRs.Where(x => x.BatchId == batchId)
                .Select(data => new SqliteSTRData()
                {
                    Id = data.Id,
                    BatchId = data.BatchId,
                    EmployeeId = data.EmployeeId,
                    IsProcessed = data.IsProcessed,
                    STRId = data.STRId,
                    ImageCount = data.ImageCount,
                    DateCreated = data.DateCreated,
                    DateUpdated = data.DateUpdated,

                    PhoneDbId = data.PhoneDbId,
                    STRNumber = data.STRNumber,
                    VehicleNumber = data.VehicleNumber,
                    DriverName = data.DriverName,
                    DriverPhone = data.DriverPhone,
                    DWSCount = data.DWSCount,
                    BagCount = data.BagCount,
                    GrossWeight = data.GrossWeight,
                    NetWeight = data.NetWeight,
                    StartOdometer = data.StartOdometer,
                    EndOdometer = data.EndOdometer,
                    STRDate = data.STRDate,
                    IsNew = data.IsNew,
                    IsTransferred = data.IsTransferred,
                    TransfereeName = data.TransfereeName,
                    TransfereePhone = data.TransfereePhone,

                    TimeStamp = data.TimeStamp,
                    ActivityId = data.ActivityId,
                    TimeStamp2 = data.TimeStamp2,
                    ActivityId2 = data.ActivityId2,

                }).ToList();
        }

        public static IEnumerable<SqliteBankDetailData> GetSavedBankDetails(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteBankDetails.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteBankDetailData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    EntityBankDetailId = x.EntityBankDetailId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,

                    IsNewEntity = x.IsNewEntity,

                    EntityId = x.EntityId,
                    EntityName = x.EntityName,

                    IsSelfAccount = x.IsSelfAccount,
                    AccountHolderName = x.AccountHolderName,
                    AccountHolderPAN = x.AccountHolderPAN,
                    BankName = x.BankName,
                    BankAccount = x.BankAccount,
                    BankIFSC = x.BankIFSC,
                    BankBranch = x.BankBranch,

                    TimeStamp = x.BankDetailDate,
                    ParentReferenceId = x.ParentReferenceId,
                    ActivityId = x.ActivityId,
                    PhoneDbId = x.PhoneDbId,
                    ImageCount = x.ImageCount
                }).ToList();
        }

        public static IEnumerable<SqliteDayPlanTargetData> GetSavedDayPlanTargetData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteDayPlanTargets.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteDayPlanTargetData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    PlanTimeStamp = x.PlanDate,
                    TargetSales = x.TargetSales,
                    TargetCollection = x.TargetCollection,
                    TargetVigoreSales = x.TargetVigoreSales,
                    TargetNewDealerAppointment = x.TargetDealerAppointment,
                    TargetDemoActivity = x.TargetDemoActivity,
                    PhoneDbId = x.PhoneDbId,
                    DayPlanTargetId = x.DayPlanTargetId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                }).ToList();
        }

        public static IEnumerable<SqliteTaskData> GetSavedFollowUpTaskData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteTasks.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteTaskData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    IsNewEntity = x.IsNewEntity,
                    ParentReferenceId = x.ParentReferenceId,
                    ProjectName = x.ProjectName,
                    Description = x.Description,
                    ActivityType = x.ActivityType,
                    ClientType = x.ClientType,
                    ClientName = x.ClientName,
                    ClientCode = x.ClientCode,
                    PlannedStartDate = x.PlannedStartDate,
                    PlannedEndDate = x.PlannedEndDate,
                    Comments = x.Comments,
                    Status = x.Status,
                    NotificationDate = x.NotificationDate,
                    TimeStamp = x.TimeStamp,
                    PhoneDbId = x.PhoneDbId,
                    TaskId = x.TaskId,
                    TaskAssignmentId = x.TaskAssignmentId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                }).ToList();
        }

        public static IEnumerable<SqliteTaskActionData> GetSavedFollowUpTaskActionData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteTaskActions.Where(x => x.BatchId == batchId)
                .Select(x => new SqliteTaskActionData()
                {
                    Id = x.Id,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    IsNewTask = x.IsNewTask,
                    TaskId = x.TaskId,
                    ParentReferenceTaskId = x.ParentReferenceTaskId,
                    TaskAssignmentId = x.TaskAssignmentId,
                    SqliteActionPhoneDbId = x.SqliteActionPhoneDbId,
                    Status = x.Status,
                    NotificationDate = x.NotificationDate,
                    TimeStamp = x.TimeStamp,
                    PhoneDbId = x.PhoneDbId,
                    TaskActionId = x.TaskActionId,
                    IsProcessed = x.IsProcessed,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                }).ToList();
        }

        /// <summary>
        /// Author: Rajesh; Date: 13-07-2021; Purpose: Display Questionnaire Details
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public static IEnumerable<SqliteDomainQuestionnaireData> GetSavedQuestionnaireData(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<SqliteDomainQuestionnaireData> resultSet = (from q in orm.SqliteQuestionnaires
                                                                   join c in orm.CustomerQuestionnaires on q.CustomerQuestionnaireId equals c.Id
                                                                   where q.BatchId == batchId
                                                                   select new SqliteDomainQuestionnaireData()
                                                                   {
                                                                       Id = q.Id,
                                                                       BatchId = (long)q.BatchId,
                                                                       EmployeeId = q.EmployeeId,
                                                                       CustomerCode = c.CustomerCode,
                                                                       EntityName = q.EntityName,
                                                                       SqliteQuestionPaperName = q.SqliteQuestionPaperName,
                                                                       PhoneDbId = q.PhoneDbId,
                                                                       ActivityId = q.ActivityId,
                                                                       IsProcessed = q.IsProcessed,
                                                                       DateCreated = q.DateCreated,
                                                                       DateUpdated = q.DateUpdated
                                                                   });
            return resultSet.ToList();
        }

        public static long GetDayId(DateTime datetime)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DateTime onlyDate = new DateTime(datetime.Year, datetime.Month, datetime.Day);
            Day dayRecord = orm.Days.Where(x => x.DATE == onlyDate).FirstOrDefault();
            if (dayRecord == null)
            {
                // TODO: ideally day record should exist - but make this code thread safe
                dayRecord = new Day() { DATE = onlyDate };
                orm.Days.Add(dayRecord);
                orm.SaveChanges();
            }
            return dayRecord.Id;
        }

        public static void UpdateLogRecord(long logId, bool completed, bool failed)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var entity = orm.DataFeedProcessLogs.Where(x => x.Id == logId).FirstOrDefault();
            if (entity != null)
            {
                entity.Timestamp = DateTime.UtcNow;
                entity.HasCompleted = completed;
                entity.HasFailed = failed;
                orm.SaveChanges();
            }
        }

        public static void ProcessDataFeed(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformDataFeed(tenantId);
        }

        //Author: Rajesh V ; Date: 19-08-21 ; Purpose: BonusRate Excel upload
        public static void RefreshBonusRateData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformBonusRateDataFeed(tenantId, isCompleteRefresh);
        }

        public static void RefreshStaffHQData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformStaffHQData(tenantId, isCompleteRefresh);
        }
        public static void RefreshPPAStaffData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformPPAStaffData(tenantId, isCompleteRefresh);
        }
        public static void RefreshPPAData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformPPAData(tenantId, isCompleteRefresh);
        }

        public static void RefreshTransporterData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformTransporterDataFeed(tenantId, isCompleteRefresh);
        }

        public static void RefreshVendorData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformVendorDataFeed(tenantId, isCompleteRefresh);
        }

        public static void RefreshGrnNumberData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformGRNNumberData(tenantId, isCompleteRefresh);
        }

        public static void RefreshRequestNumberData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformRequestNumberData(tenantId, isCompleteRefresh);
        }

        public static void RefreshItemMasterData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformItemMasterData(tenantId, isCompleteRefresh);
        }

        public static void RefreshAgreementNumberData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformAgreementNumberData(tenantId, isCompleteRefresh);
        }

        public static void RefreshSurveyNumberData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformSurveyNumberData(tenantId, isCompleteRefresh);
        }

        public static void RefreshEntityNumberData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformEntityNumberData(tenantId, isCompleteRefresh);
        }

        public static void RefreshStaffDivisionData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformStaffDivisionData(tenantId, isCompleteRefresh);
        }

        public static void RefreshDivisionSegmentData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformDivisionSegmentData(tenantId, isCompleteRefresh);
        }

        public static void RefreshStaffMessageData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformStaffMessageData(tenantId, isCompleteRefresh);
        }

        public static void RefreshStaffDailyReportData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformStaffDailyReportData(tenantId, isCompleteRefresh);
        }

        public static void RefreshCustomerDivisionBalance(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformCustomerDivisionBalance(tenantId, isCompleteRefresh);
        }

        public static void RefreshEmployeeAchievedData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformEmployeeAchievedData(tenantId, isCompleteRefresh);
        }

        public static void RefreshEmployeeMonthlyTargetData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformEmployeeMonthlyTargetData(tenantId, isCompleteRefresh);
        }

        public static void RefreshEmployeeYearlyTargetData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformEmployeeYearlyTargetData(tenantId, isCompleteRefresh);
        }
        public static void RefreshOfficeHierarchy(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformOfficeHierarchy(tenantId);
        }

        public static void ProcessCustomerDataFeed(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformCustomerDataFeed(tenantId, isCompleteRefresh);
        }

        public static void ProcessProductDataFeed(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformProductDataFeed(tenantId, isCompleteRefresh);
        }

        public static void ProcessSalesPersonDataFeed(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformSalesPersonDataFeed(tenantId, isCompleteRefresh);
        }

        //Added by Swetha -Mar 15
        public static void ProcessLeaveTypeDataFeed(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformLeaveTypeDataFeed(tenantId);
        }
        //Added by Swetha -Mar 15
        public static void ProcessHolidayListDataFeed(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformHolidayListDataFeed(tenantId);
        }

        public static void ProcessSoParentSOData(long tenantId, bool isCompleteRefresh)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.TransformSOParentSOData(tenantId, isCompleteRefresh);
        }
        public static ICollection<DomainEntities.TransportType> GetTransportTypes()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TransportTypes.Where(x => x.IsActive == true).Select(x => new DomainEntities.TransportType()
            {
                //Id = x.Id,
                DisplaySequence = x.DisplaySequence,
                TransportTypeCode = x.TransportTypeCode,
                ReimbursementRatePerUnit = x.ReimbursementRatePerUnit,
                IsPublic = x.IsPublic
            }).ToList();
        }

        public static long ProcessStartDayRequest(StartDay sd)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var result = new System.Data.Entity.Core.Objects.ObjectParameter("employeeDayId", 0);
            orm.StartEmployeeDay(sd.EmployeeId, sd.At, sd.PhoneModel, sd.PhoneOS, sd.AppVersion, result);
            return (long)result.Value;
        }

        public static long CreateSqliteActionProcessLogEntry(long tenantId, long employeeId, bool lockStatus)
        {
            EpicCrmEntities orm = DBLayer.GetNoLogOrm;
            DateTime dt = DateTime.UtcNow;
            var logEntry = new DBModel.SqliteActionProcessLog()
            {
                TenantId = tenantId,
                EmployeeId = employeeId,
                LockAcquiredStatus = lockStatus,
                At = dt,
                Timestamp = dt,
                HasCompleted = false,
                HasFailed = false
            };

            orm.SqliteActionProcessLogs.Add(logEntry);
            orm.SaveChanges();
            return logEntry.Id;
        }

        public static long CreateSMSProcessLogEntry(long tenantId, bool lockStatus)
        {
            EpicCrmEntities orm = DBLayer.GetNoLogOrm;
            DateTime dt = DateTime.UtcNow;
            var logEntry = new DBModel.SMSProcessLog()
            {
                TenantId = tenantId,
                LockAcquiredStatus = lockStatus,
                At = dt,
                Timestamp = dt,
                HasCompleted = false,
                HasFailed = false
            };

            orm.SMSProcessLogs.Add(logEntry);
            orm.SaveChanges();
            return logEntry.Id;
        }

        public static void SaveSqliteDataWrapper(long employeeId, long batchId, IEnumerable<SqliteDomainTerminateAgreementData> domainTerminateAgreementData)
        {
            try
            {
                DBLayer.SaveSqliteData(employeeId, batchId, domainTerminateAgreementData);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"TerminateAgreement not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteDataWrapper)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteData(long employeeId, long batchId, IEnumerable<SqliteDomainTerminateAgreementData> domainTerminateAgreementData)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.TerminateRequests = domainTerminateAgreementData.Count();

                int rowsSaved = 0;
                foreach (SqliteDomainTerminateAgreementData data in domainTerminateAgreementData)
                {
                    orm.SqliteTerminateAgreements.Add(
                            new SqliteTerminateAgreement()
                            {
                                BatchId = batchId,
                                EmployeeId = employeeId,
                                IsProcessed = false,
                                TerminateAgreementId = 0,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                                RequestDate = data.TimeStamp,

                                EntityId = data.EntityId,
                                EntityName = data.EntityName,
                                AgreementId = data.AgreementId,
                                Agreement = data.Agreement,
                                TypeName = data.TypeName,
                                ActivityId = data.ActivityId,
                                Reason = data.Reason,
                                Notes = data.Notes,

                            });

                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.TerminateRequestsSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteWorkFlowDataWrapper(long employeeId, long batchId, IEnumerable<SqliteDomainWorkFlowPageData> domainWFPData)
        {
            try
            {
                DBLayer.SaveSqliteWorkFlowData(employeeId, batchId, domainWFPData);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"WorkFlowData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteWorkFlowDataWrapper)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteWorkFlowData(long employeeId, long batchId, IEnumerable<SqliteDomainWorkFlowPageData> domainWFPData)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.NumberOfWorkFlow = domainWFPData.Count();

                int rowsSaved = 0;
                foreach (SqliteDomainWorkFlowPageData data in domainWFPData)
                {
                    SqliteEntityWorkFlowV2 v2Rec = new SqliteEntityWorkFlowV2()
                    {
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        IsProcessed = false,
                        EntityWorkFlowId = 0,
                        DateCreated = DateTime.UtcNow,
                        Timestamp = DateTime.UtcNow,
                        FieldVisitDate = data.TimeStamp,

                        EntityId = data.EntityId,
                        EntityType = data.EntityType,
                        EntityName = data.EntityName,
                        AgreementId = data.AgreementId,
                        Agreement = data.Agreement,
                        EntityWorkFlowDetailId = data.EntityWorkFlowDetailId,
                        TypeName = data.TypeName,
                        TagName = data.TagName,
                        Phase = data.CurrentPhase,
                        IsStarted = data.PhaseStarted,
                        Date = data.PhaseDate,
                        MaterialType = data.SeedType,
                        MaterialQuantity = data.SeedQuantity,
                        GapFillingRequired = data.GapFillingRequired,
                        GapFillingSeedQuantity = data.GapFillingSeedQuantity,
                        LaborCount = data.HarvestLaborCount,
                        PercentCompleted = data.PercentCompleted,
                        ActivityId = data.ActivityId,
                        FollowUpCount = data.FollowUps?.Count() ?? 0,

                        BatchNumber = data.BatchNumber,
                        LandSize = data.LandSize,
                        DWSEntry = data.DWSEntry,
                        ItemCount = data.ItemCount, // Plant Count or Nipping Count
                        ItemsUsedCount = data.ItemsUsed?.Count() ?? 0,
                        YieldExpected = data.YieldExpected,
                        BagsIssued = data.BagsIssued,
                        HarvestDate = data.HarvestDate
                    };

                    if (v2Rec.FollowUpCount > 0)
                    {
                        foreach (SqliteDomainWorkFlowFollowUp followUpRec in data.FollowUps)
                        {
                            v2Rec.SqliteEntityWorkFlowFollowUps.Add(new DBModel.SqliteEntityWorkFlowFollowUp()
                            {
                                Phase = followUpRec.Phase,
                                StartDate = followUpRec.StartDate,
                                EndDate = followUpRec.EndDate,
                                Notes = followUpRec.Notes
                            });
                        }
                    }

                    int itemsAdded = 0;
                    if (v2Rec.ItemsUsedCount > 0)
                    {
                        foreach (string itemName in data.ItemsUsed)
                        {
                            if (String.IsNullOrEmpty(itemName) == false)
                            {
                                itemsAdded++;
                                v2Rec.SqliteEntityWorkFlowItemUseds.Add(new DBModel.SqliteEntityWorkFlowItemUsed()
                                {
                                    ItemName = itemName
                                });
                            }
                        }
                        v2Rec.ItemsUsedCount = itemsAdded;
                    }

                    orm.SqliteEntityWorkFlowV2.Add(v2Rec);
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.NumberOfWorkFlowSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteSTRDataWrapper(long employeeId, long batchId,
                IEnumerable<SqliteDomainSTRData> domainSTRData)
        {
            try
            {
                DBLayer.SaveSqliteSTRData(employeeId, batchId, domainSTRData);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"STRDetails Data not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteSTRData)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteSTRData(long employeeId, long batchId,
                    IEnumerable<SqliteDomainSTRData> domainSTRData)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();

                int rowsSaved = 0;
                foreach (var data in domainSTRData)
                {
                    var sbd = new SqliteSTR()
                    {
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        STRNumber = data.STRNumber,
                        VehicleNumber = data.VehicleNumber,
                        DriverName = data.DriverName,
                        DriverPhone = data.DriverPhone,
                        DWSCount = data.DWSCount,
                        BagCount = data.BagCount,
                        GrossWeight = data.GrossWeight,
                        NetWeight = data.NetWeight,
                        StartOdometer = data.StartOdometer,
                        EndOdometer = data.EndOdometer,
                        STRDate = data.STRDate,
                        IsNew = data.IsNew,
                        IsTransferred = data.IsTransferred,
                        TransfereeName = data.TransfereeName,
                        TransfereePhone = data.TransfereePhone,

                        ImageCount = data.Images?.Count() ?? 0,
                        TimeStamp = data.TimeStamp,
                        ActivityId = data.ActivityId,
                        TimeStamp2 = data.TimeStamp2,
                        ActivityId2 = data.ActivityId2,

                        PhoneDbId = data.PhoneDbId,
                        IsProcessed = false,
                        STRId = 0,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
                    };

                    if (sbd.ImageCount > 0)
                    {
                        int imgSeqNo = 0;
                        data.Images.Select(img =>
                        {
                            sbd.SqliteSTRImages.Add(new SqliteSTRImage()
                            {
                                ImageFileName = img,
                                SequenceNumber = ++imgSeqNo
                            });
                            return true;
                        }).ToList();
                    }

                    if (sbd.DWSCount > 0)
                    {
                        data.DomainDWSData.Select(x =>
                        {
                            sbd.SqliteSTRDWS.Add(new SqliteSTRDW()
                            {
                                EntityId = x.EntityId,
                                EntityName = x.EntityName,
                                AgreementId = x.AgreementId,
                                Agreement = x.Agreement,
                                EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                                TypeName = x.TypeName,
                                TagName = x.TagName,
                                DWSNumber = x.DWSNumber,
                                BagCount = x.BagCount,
                                FilledBagsWeightKg = x.FilledBagsWeightKg,
                                EmptyBagsWeightKg = x.EmptyBagsWeightKg,
                                DWSDate = x.TimeStamp,
                                ActivityId = x.ActivityId,
                                IsProcessed = false,
                                DWSId = 0
                            });
                            return 1;
                        }).Count();
                    }

                    orm.SqliteSTRs.Add(sbd);
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.STRSavedCount = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteBankDetailsDataWrapper(long employeeId, long batchId,
                IEnumerable<SqliteDomainBankDetail> domainBankDetails)
        {
            try
            {
                DBLayer.SaveSqliteBankDetailsData(employeeId, batchId, domainBankDetails);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"BankDetails Data not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteBankDetailsData)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteBankDetailsData(long employeeId, long batchId,
                    IEnumerable<SqliteDomainBankDetail> domainBankDetails)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();

                int rowsSaved = 0;
                foreach (var data in domainBankDetails)
                {
                    var sbd = new SqliteBankDetail()
                    {
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        IsNewEntity = data.IsNewEntity,
                        EntityId = data.EntityId,
                        EntityName = data.EntityName,

                        IsSelfAccount = data.IsSelfAccount,
                        AccountHolderName = data.AccountHolderName,
                        AccountHolderPAN = data.AccountHolderPAN,
                        BankName = data.BankName,
                        BankAccount = data.BankAccount,
                        BankIFSC = data.BankIFSC,
                        BankBranch = data.BankBranch,

                        BankDetailDate = data.TimeStamp,
                        ImageCount = data.Images?.Count() ?? 0,
                        ActivityId = data.ActivityId,
                        PhoneDbId = data.PhoneDbId,
                        ParentReferenceId = data.ParentReferenceId,
                        IsProcessed = false,
                        EntityBankDetailId = 0,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
                    };

                    if (sbd.ImageCount > 0)
                    {
                        int imgSeqNo = 0;
                        data.Images.Select(img =>
                        {
                            sbd.SqliteBankDetailImages.Add(new SqliteBankDetailImage()
                            {
                                ImageFileName = img,
                                SequenceNumber = ++imgSeqNo
                            });
                            return true;
                        }).ToList();
                    }

                    orm.SqliteBankDetails.Add(sbd);
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.BankDetailsSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteAgreementsDataWrapper(long employeeId, long batchId,
                IEnumerable<SqliteDomainAgreement> domainAgreements)
        {
            try
            {
                DBLayer.SaveSqliteAgreementsData(employeeId, batchId, domainAgreements);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"Agreements Data not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteAgreementsData)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteAgreementsData(long employeeId, long batchId,
                    IEnumerable<SqliteDomainAgreement> domainAgreements)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.Agreements = domainAgreements.Count();

                int rowsSaved = 0;
                foreach (var data in domainAgreements)
                {
                    orm.SqliteAgreements.Add(
                            new SqliteAgreement()
                            {
                                BatchId = batchId,
                                EmployeeId = employeeId,
                                IsNewEntity = data.IsNewEntity,
                                EntityId = data.EntityId,
                                EntityName = data.EntityName,
                                SeasonName = data.SeasonName,
                                TypeName = data.TypeName,
                                Acreage = (decimal)data.Acreage,
                                AgreementDate = data.TimeStamp,
                                ActivityId = data.ActivityId,
                                PhoneDbId = data.PhoneDbId,
                                ParentReferenceId = data.ParentReferenceId,
                                IsProcessed = false,
                                EntityAgreementId = 0,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                                TerritoryCode = data.TerritoryCode,
                                TerritoryName = data.TerritoryName,
                                HQCode = data.HQCode,
                                HQName = data.HQName
                            });
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.AgreementsSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteSurveysDataWrapper(long employeeId, long batchId,
                IEnumerable<SqliteDomainSurvey> domainSurveys)
        {
            try
            {
                DBLayer.SaveSqliteSurveysData(employeeId, batchId, domainSurveys);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"Surveys Data not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteSurveysData)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteSurveysData(long employeeId, long batchId,
                    IEnumerable<SqliteDomainSurvey> domainSurveys)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.Agreements = domainAgreements.Count();

                int rowsSaved = 0;
                foreach (var data in domainSurveys)
                {
                    orm.SqliteSurveys.Add(
                            new SqliteSurvey()
                            {
                                BatchId = batchId,
                                EmployeeId = employeeId,
                                IsNewEntity = data.IsNewEntity,
                                EntityId = data.EntityId,
                                EntityName = data.EntityName,
                                SeasonName = data.SeasonName,
                                SowingType = data.SowingType,
                                Acreage = (decimal)data.Acreage,
                                SowingDate = data.SowingDate,

                                MajorCrop = data.MajorCrop,
                                LastCrop = data.LastCrop,
                                WaterSource = data.WaterSource,
                                SoilType = data.SoilType,

                                SurveyDate = data.TimeStamp,
                                ActivityId = data.ActivityId,
                                PhoneDbId = data.PhoneDbId,
                                ParentReferenceId = data.ParentReferenceId,
                                IsProcessed = false,
                                EntitySurveyId = 0,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                            });
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.SurveysSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteAdvanceRequestsDataWrapper(long employeeId, long batchId,
                IEnumerable<SqliteDomainAdvanceRequest> domainAdvanceRequests)
        {
            try
            {
                DBLayer.SaveSqliteAdvanceRequestData(employeeId, batchId, domainAdvanceRequests);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"Advance Request data not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteAdvanceRequestData)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteAdvanceRequestData(long employeeId, long batchId,
                IEnumerable<SqliteDomainAdvanceRequest> domainAdvanceRequests)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.AdvanceRequests = domainAdvanceRequests.Count();

                int rowsSaved = 0;
                foreach (var data in domainAdvanceRequests)
                {
                    orm.SqliteAdvanceRequests.Add(
                            new SqliteAdvanceRequest()
                            {
                                BatchId = batchId,
                                EmployeeId = employeeId,
                                IsNewEntity = data.IsNewEntity,
                                IsNewAgreement = data.IsNewAgreement,
                                EntityId = data.EntityId,
                                EntityName = data.EntityName,
                                AgreementId = data.AgreementId,
                                Agreement = data.Agreement,
                                Amount = (decimal)data.Amount,
                                AdvanceRequestDate = data.TimeStamp,
                                Notes = data.Notes,
                                ActivityId = data.ActivityId,
                                ParentReferenceId = data.ParentReferenceId,
                                IsProcessed = false,
                                AdvanceRequestId = 0,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                            });
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.AdvanceRequestsSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteIssueReturnDataWrapper(long employeeId, long batchId, IEnumerable<SqliteDomainIssueReturn> domainIssueReturns)
        {
            try
            {
                DBLayer.SaveSqliteIssueReturnData(employeeId, batchId, domainIssueReturns);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"IssueReturnData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteIssueReturnData)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteIssueReturnData(long employeeId, long batchId,
                        IEnumerable<SqliteDomainIssueReturn> domainIssueReturns)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.NumberOfIssueReturns = domainIssueReturns.Count();

                int rowsSaved = 0;
                foreach (SqliteDomainIssueReturn data in domainIssueReturns)
                {
                    orm.SqliteIssueReturns.Add(
                            new SqliteIssueReturn()
                            {
                                BatchId = batchId,
                                EmployeeId = employeeId,
                                IsNewEntity = data.IsNewEntity,
                                IsNewAgreement = data.IsNewAgreement,
                                EntityId = data.EntityId,
                                EntityName = data.EntityName,
                                AgreementId = data.AgreementId,
                                Agreement = data.Agreement,
                                TranType = data.TranType,
                                ItemId = data.ItemId,
                                ItemCode = data.ItemCode,
                                SlipNumber = data.SlipNumber,
                                Acreage = (decimal)data.Acreage,
                                Quantity = data.Quantity,
                                ItemRate = data.ItemRate,
                                IssueReturnDate = data.TimeStamp,
                                ActivityId = data.ActivityId,
                                ParentReferenceId = data.ParentReferenceId,
                                IsProcessed = false,
                                IssueReturnId = 0,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow
                            });
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.NumberOfIssueReturnsSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqlitePaymentDataWrapper(long employeeId, long batchId, IEnumerable<SqliteDomainPayment> domainPayments)
        {
            try
            {
                DBLayer.SaveSqlitePaymentData(employeeId, batchId, domainPayments);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"PaymentData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError("SaveSqlitePaymentData", errorText, logSnip);
            }
        }

        public static ICollection<DashboardUser> GetRegisteredWebPortalUsers()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.AspNetUsers.Select(x => new DashboardUser()
            {
                UserName = x.UserName,
                UserEmail = x.Email
            }).ToList();
        }

        /// <summary>
        /// Filter out locked out user and users for whom disable time has reached
        /// </summary>
        /// <returns></returns>
        public static ICollection<DashboardUser> GetAllowedWebPortalUsers()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.AspNetUsers
                .Where(x => (x.LockoutEndDateUtc.HasValue == false || x.LockoutEndDateUtc.Value < DateTime.UtcNow) &&
                            (x.DisableUserAfterUtc.HasValue == false || x.DisableUserAfterUtc.Value > DateTime.UtcNow)
                    )
                .Select(x => new DashboardUser()
                {
                    UserName = x.UserName,
                    UserEmail = x.Email
                }).ToList();
        }

        private static void SaveSqlitePaymentData(long employeeId, long batchId, IEnumerable<SqliteDomainPayment> domainPayments)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.TotalPaymentAmount = domainPayments.Sum(x => x.TotalAmount);
                //batchRecord.NumberOfPayments = domainPayments.Count();

                int rowsSaved = 0;
                foreach (SqliteDomainPayment domainPayment in domainPayments)
                {
                    SqlitePayment payment = new SqlitePayment()
                    {
                        PhoneDbId = domainPayment.PhoneDbId,
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        CustomerCode = domainPayment.CustomerCode,
                        PaymentType = domainPayment.PaymentMode,
                        TotalAmount = domainPayment.TotalAmount,
                        Comment = domainPayment.Comment,
                        PaymentDate = domainPayment.TimeStamp,
                        PhoneActivityId = domainPayment.ActivityId,
                        IsProcessed = false,
                        PaymentId = 0,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
                        ImageCount = domainPayment.Images == null ? 0 : domainPayment.Images.Count()
                    };

                    if (payment.ImageCount > 0)
                    {
                        int imgSeqNo = 0;
                        domainPayment.Images.Select(img =>
                        {
                            payment.SqlitePaymentImages.Add(new SqlitePaymentImage()
                            {
                                ImageFileName = img,
                                SequenceNumber = ++imgSeqNo
                            });
                            return true;
                        }).ToList();
                    }

                    orm.SqlitePayments.Add(payment);
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.NumberOfPaymentsSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static ICollection<DomainEntities.SalesPersonsAssociationDataForAll> GetStaffAssociationsForAll(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.SalesPersonAssociations
                .Where(x => x.IsDeleted == false)
                .Join(orm.CodeTables.Where(t => t.TenantId == tenantId && t.IsActive),
                        o => new { k1 = o.CodeType, k2 = o.CodeValue },
                        i => new { k1 = i.CodeType, k2 = i.CodeValue },
                        (o, i) => new DomainEntities.SalesPersonsAssociationDataForAll()
                        {
                            StaffCode = o.StaffCode,
                            AssignedDate = o.DateUpdated,
                            CodeType = o.CodeType,
                            CodeName = i.CodeName,
                            Code = o.CodeValue
                        }).ToList();
        }

        public static void SaveSqliteCancelledLeavesDataWrapper(long employeeId, long batchId, IEnumerable<long> domainCancelledLeaves)
        {
            try
            {
                DBLayer.SaveSqliteCancelledLeavesData(employeeId, batchId, domainCancelledLeaves);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"CancelledLeaveData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError("SaveSqliteCancelledLeavesData", errorText, logSnip);
            }
        }

        private static void SaveSqliteCancelledLeavesData(long employeeId, long batchId, IEnumerable<long> domainCancelledLeaves)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                var batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.NumberOfCancelledLeaves = domainCancelledLeaves.Count();

                int savedCancelledLeavesCount = 0;
                foreach (long leaveId in domainCancelledLeaves)
                {
                    savedCancelledLeavesCount++;
                    SqliteCancelledLeave ord = new SqliteCancelledLeave()
                    {
                        LeaveId = leaveId,
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        IsProcessed = false,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    };
                    orm.SqliteCancelledLeaves.Add(ord);
                }

                // now update the rows added to the batch record
                batchRecord.NumberOfCancelledLeavesSaved = savedCancelledLeavesCount;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteLeavesDataWrapper(long employeeId, long batchId, IEnumerable<SqliteDomainLeave> domainLeaves)
        {
            try
            {
                DBLayer.SaveSqliteLeavesData(employeeId, batchId, domainLeaves);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"LeaveData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError("SaveSqliteLeavesData", errorText, logSnip);
            }
        }

        private static void SaveSqliteLeavesData(long employeeId, long batchId, IEnumerable<SqliteDomainLeave> domainLeaves)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                var batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.NumberOfLeaves = domainLeaves.Count();

                int savedLeavesCount = 0;
                foreach (SqliteDomainLeave domainLeave in domainLeaves)
                {
                    savedLeavesCount++;
                    SqliteLeave ord = new SqliteLeave()
                    {
                        PhoneDbId = domainLeave.PhoneDbId,

                        StartDate = domainLeave.StartDate,
                        EndDate = domainLeave.EndDate,
                        LeaveType = domainLeave.LeaveType,
                        LeaveReason = domainLeave.LeaveReason,
                        Comment = domainLeave.Comment,
                        DaysCountExcludingHolidays = domainLeave.DaysCountExcludingHolidays,
                        DaysCount = domainLeave.DaysCount,
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        IsProcessed = false,

                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    };
                    orm.SqliteLeaves.Add(ord);
                }

                // now update the rows added to the batch record
                batchRecord.NumberOfLeavesSaved = savedLeavesCount;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteOrdersDataWrapper(long employeeId, long batchId, IEnumerable<SqliteDomainOrder> domainOrders)
        {
            try
            {
                DBLayer.SaveSqliteOrdersData(employeeId, batchId, domainOrders);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"OrderData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError("SaveSqliteOrdersData", errorText, logSnip);
            }
        }

        private static void SaveSqliteOrdersData(long employeeId, long batchId, IEnumerable<SqliteDomainOrder> domainOrders)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                var batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();

                //batchRecord.TotalOrderAmount = domainOrders.Sum(x => x.NetAmount); // inclusive of GST
                //batchRecord.NumberOfOrders = domainOrders.Count();
                //batchRecord.NumberOfOrderLines = domainOrders.Sum(x => x.ItemCount);

                int savedOrderCount = 0;
                int savedOrderLines = 0;
                foreach (SqliteDomainOrder domainOrder in domainOrders)
                {
                    savedOrderCount++;

                    SqliteOrder ord = new SqliteOrder()
                    {
                        PhoneDbId = domainOrder.PhoneDbId,
                        CustomerCode = domainOrder.CustomerCode,
                        OrderDate = domainOrder.TimeStamp,
                        OrderType = "New",
                        TotalAmount = domainOrder.TotalAmount,
                        TotalGST = domainOrder.TotalGST,
                        NetAmount = domainOrder.NetAmount,
                        MaxDiscountPercentage = domainOrder.MaxDiscountPercentage,
                        DiscountType = domainOrder.DiscountType,
                        AppliedDiscountPercentage = domainOrder.AppliedDiscountPercentage,
                        ImageCount = domainOrder.Images?.Count() ?? 0,
                        ItemCount = domainOrder.ItemCount,
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        IsProcessed = false,
                        PhoneActivityId = domainOrder.ActivityId,
                        OrderId = 0,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    };

                    if (ord.ItemCount > 0)
                    {
                        int seqNo = 0;
                        foreach (var orderItem in domainOrder.Items)
                        {
                            seqNo++;
                            ord.SqliteOrderItems.Add(new SqliteOrderItem()
                            {
                                ProductCode = orderItem.ProductCode,
                                SerialNumber = seqNo,
                                UnitPrice = orderItem.BillingPrice,
                                UnitQuantity = orderItem.Quantity,
                                Amount = orderItem.Amount,  // legacy apk v1.3 and before
                                DiscountPercent = orderItem.DiscountPercent,
                                DiscountedPrice = orderItem.DiscountedPrice,
                                ItemPrice = orderItem.ItemPrice,  // price after discount
                                GstPercent = orderItem.GstPercent,
                                GstAmount = orderItem.GstAmount,
                                NetPrice = orderItem.NetPrice
                            });
                        }
                        savedOrderLines += seqNo;
                    }

                    if (ord.ImageCount > 0)
                    {
                        int imgSeqNo = 0;
                        domainOrder.Images.Select(img =>
                        {
                            ord.SqliteOrderImages.Add(new SqliteOrderImage()
                            {
                                ImageFileName = img,
                                SequenceNumber = ++imgSeqNo
                            });
                            return true;
                        }).ToList();
                    }

                    orm.SqliteOrders.Add(ord);
                }

                // now update the rows added to the batch record
                batchRecord.NumberOfOrdersSaved = savedOrderCount;
                batchRecord.NumberOfOrderLinesSaved = savedOrderLines;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteReturnOrdersDataWrapper(long employeeId, long batchId, IEnumerable<SqliteDomainReturnOrder> DomainReturnOrders)
        {
            try
            {
                DBLayer.SaveSqliteReturnOrdersData(employeeId, batchId, DomainReturnOrders);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"ReturnOrdersData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError("SaveSqliteReturnOrdersData", errorText, logSnip);
            }
        }

        private static void SaveSqliteReturnOrdersData(long employeeId, long batchId, IEnumerable<SqliteDomainReturnOrder> DomainReturnOrders)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                var batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.TotalReturnAmount = DomainReturnOrders.Sum(x => x.TotalAmount);
                //batchRecord.NumberOfReturns = DomainReturnOrders.Count();
                //batchRecord.NumberOfReturnLines = DomainReturnOrders.Sum(x => x.ItemCount);

                int savedReturnOrderCount = 0;
                int savedReturnOrderLines = 0;
                foreach (SqliteDomainReturnOrder domainReturnOrder in DomainReturnOrders)
                {
                    savedReturnOrderCount++;

                    SqliteReturnOrder ord = new SqliteReturnOrder()
                    {
                        PhoneDbId = domainReturnOrder.PhoneDbId,
                        CustomerCode = domainReturnOrder.CustomerCode,
                        ReturnOrderDate = domainReturnOrder.TimeStamp,
                        TotalAmount = domainReturnOrder.TotalAmount,
                        ItemCount = domainReturnOrder.ItemCount,
                        ReferenceNum = domainReturnOrder.ReferenceNum,
                        Comment = domainReturnOrder.Comment,
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        PhoneActivityId = domainReturnOrder.ActivityId,
                        IsProcessed = false,
                        ReturnOrderId = 0,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow
                    };

                    if (ord.ItemCount > 0)
                    {
                        int seqNo = 0;
                        foreach (var orderReturnItem in domainReturnOrder.Items)
                        {
                            seqNo++;
                            ord.SqliteReturnOrderItems.Add(new SqliteReturnOrderItem()
                            {
                                ProductCode = orderReturnItem.ProductCode,
                                SerialNumber = seqNo,
                                UnitPrice = orderReturnItem.BillingPrice,
                                UnitQuantity = orderReturnItem.Quantity,
                                Amount = orderReturnItem.ItemPrice,
                                Comment = orderReturnItem.Comment
                            });
                        }
                        savedReturnOrderLines += seqNo;
                    }
                    orm.SqliteReturnOrders.Add(ord);
                }

                // now update the rows added to the batch record
                batchRecord.NumberOfReturnsSaved = savedReturnOrderCount;
                batchRecord.NumberOfReturnLinesSaved = savedReturnOrderLines;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteEntitiesDataWrapper(long employeeId, long batchId,
                        IEnumerable<SqliteDomainEntity> domainEntities, int ignoredItemsCount)
        {
            try
            {
                DBLayer.SaveSqliteEntitiesData(employeeId, batchId, domainEntities, ignoredItemsCount);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"EntityData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError("SaveSqliteEntitiesData", errorText, logSnip);
            }
        }

        private static void SaveSqliteEntitiesData(long employeeId, long batchId,
            IEnumerable<SqliteDomainEntity> domainEntities, int ignoredItemsCount)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                var batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.NumberOfEntities = domainEntities.Count();

                int savedEntityCount = 0;
                foreach (SqliteDomainEntity domainEntity in domainEntities)
                {
                    savedEntityCount++;

                    SqliteEntity entity = new SqliteEntity()
                    {
                        PhoneDbId = domainEntity.PhoneDbId,
                        AtBusiness = domainEntity.AtBusiness,
                        //Consent = domainEntity.Consent, //Swetha Made change on 24-11-2021
                        EntityType = domainEntity.EntityType,
                        EntityName = domainEntity.EntityName,
                        Address = domainEntity.Address,
                        City = domainEntity.City,
                        State = domainEntity.State,
                        Pincode = domainEntity.Pincode,
                        LandSize = domainEntity.LandSize,
                        TimeStamp = domainEntity.TimeStamp,
                        Latitude = domainEntity.Latitude,
                        Longitude = domainEntity.Longitude,
                        LocationTaskStatus = domainEntity.LocationTaskStatus,
                        LocationException = domainEntity.LocationException,
                        MNC = domainEntity.MNC,
                        MCC = domainEntity.MCC,
                        LAC = domainEntity.LAC,
                        CellId = domainEntity.CellId,
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        IsProcessed = false,
                        EntityId = 0,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
                        ContactCount = domainEntity.Contacts?.Count() ?? 0,
                        CropCount = domainEntity.Crops?.Count() ?? 0,
                        ImageCount = domainEntity.Images?.Count() ?? 0,
                        LocationCount = domainEntity.Locations?.Count() ?? 0,
                        UniqueIdType = domainEntity.UniqueIdType,
                        UniqueId = domainEntity.UniqueId,
                        TaxId = domainEntity.TaxId,
                        DerivedLocSource = "",
                        DerivedLatitude = 0,
                        DerivedLongitude = 0,

                        FatherHusbandName = domainEntity.FatherHusbandName,
                        TerritoryCode = domainEntity.TerritoryCode,
                        TerritoryName = domainEntity.TerritoryName,
                        HQCode = domainEntity.HQCode,
                        HQName = domainEntity.HQName,
                        //MajorCrop = domainEntity.MajorCrop,
                        //LastCrop = domainEntity.LastCrop,
                        //WaterSource = domainEntity.WaterSource,
                        //SoilType = domainEntity.SoilType,
                        //SowingType = domainEntity.SowingType,
                        //SowingDate = domainEntity.SowingDate
                    };

                    if (domainEntity.Contacts != null)
                    {
                        foreach (SqliteDomainEntityContact contact in domainEntity.Contacts)
                        {
                            entity.SqliteEntityContacts.Add(new SqliteEntityContact()
                            {
                                Name = contact.Name,
                                PhoneNumber = contact.PhoneNumber,
                                IsPrimary = contact.IsPrimary
                            });
                        }
                    }

                    if (domainEntity.Crops != null)
                    {
                        foreach (SqliteDomainEntityCrop crop in domainEntity.Crops)
                        {
                            entity.SqliteEntityCrops.Add(new SqliteEntityCrop()
                            {
                                Name = crop.Name
                            });
                        }
                    }

                    if (entity.ImageCount > 0)
                    {
                        int imgSeqNo = 0;
                        domainEntity.Images.Select(img =>
                        {
                            entity.SqliteEntityImages.Add(new SqliteEntityImage()
                            {
                                ImageFileName = img,
                                SequenceNumber = ++imgSeqNo
                            });
                            return true;
                        }).ToList();
                    }

                    // store locations
                    if (entity.LocationCount > 0)
                    {
                        foreach (var item in domainEntity.Locations)
                        {
                            entity.SqliteEntityLocations.Add(new SqliteEntityLocation()
                            {
                                Latitude = item.Latitude,
                                Longitude = item.Longitude,
                                UtcAt = item.UtcAt,
                                LocationTaskStatus = item.LocationTaskStatus,
                                LocationException = item.LocationException,
                                Source = item.Source,
                                IsGood = item.IsGood
                            });
                        }
                    }

                    orm.SqliteEntities.Add(entity);
                }

                // now update the rows added to the batch record
                batchRecord.NumberOfEntitiesSaved = savedEntityCount;
                batchRecord.DuplicateEntityCount = ignoredItemsCount;

                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteDeviceLogsDataWrapper(long employeeId, long batchId, IEnumerable<DomainEntities.SqliteDeviceLog> deviceLogs)
        {
            try
            {
                DBLayer.SaveSqliteDeviceLogsData(employeeId, batchId, deviceLogs);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"DeviceLogs not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError("SaveSqliteDeviceLogsDataWrapper", errorText, logSnip);
            }
        }

        private static void SaveSqliteDeviceLogsData(long employeeId, long batchId, IEnumerable<DomainEntities.SqliteDeviceLog> deviceLogs)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                var batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.DeviceLogCount = deviceLogs.Count();

                foreach (var dl in deviceLogs)
                {
                    DBModel.SqliteDeviceLog logEntry = new DBModel.SqliteDeviceLog()
                    {
                        At = dl.TimeStamp,
                        LogText = dl.LogText,
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        DateCreated = DateTime.UtcNow
                    };
                    orm.SqliteDeviceLogs.Add(logEntry);
                }

                // now update the batch record
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        // DayPlanning 20210603

        public static void SaveSqliteDayPlanTargetDataWrapper(long employeeId, long batchId,
        IEnumerable<SqliteDomainDayPlanTarget> domainDayPlanTarget)
        {
            try
            {
                DBLayer.SaveSqliteDayPlanTargetData(employeeId, batchId, domainDayPlanTarget);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"Day Plan Target Data not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteDayPlanTargetData)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteDayPlanTargetData(long employeeId, long batchId,
                    IEnumerable<SqliteDomainDayPlanTarget> domainDayPlanTarget)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();

                int rowsSaved = 0;
                foreach (var data in domainDayPlanTarget)
                {
                    orm.SqliteDayPlanTargets.Add(
                            new SqliteDayPlanTarget()
                            {
                                BatchId = batchId,
                                EmployeeId = employeeId,
                                PlanDate = data.PlanTimeStamp,
                                TargetSales = data.TargetSales,
                                TargetCollection = data.TargetCollection,
                                TargetVigoreSales = data.TargetVigoreSales,
                                TargetDealerAppointment = data.TargetNewDealerAppointment,
                                TargetDemoActivity = data.TargetDemoActivity,
                                PhoneDbId = data.PhoneDbId,
                                IsProcessed = false,
                                DayPlanTargetId = 0,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                            });
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.DayPlanTargetSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        // FollowUpTasks 20211009

        public static void SaveSqliteTaskDataWrapper(long employeeId, long batchId,
        IEnumerable<SqliteDomainTask> domainTask)
        {
            try
            {
                DBLayer.SaveSqliteTaskData(employeeId, batchId, domainTask);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"FollowUp/Task Data not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteDayPlanTargetData)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteTaskData(long employeeId, long batchId,
            IEnumerable<SqliteDomainTask> domainTask)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();

                int rowsSaved = 0;
                foreach (var data in domainTask)
                {
                    orm.SqliteTasks.Add(
                            new SqliteTask()
                            {
                                BatchId = batchId,
                                EmployeeId = employeeId,
                                IsNewEntity = data.IsNewEntity,
                                ParentReferenceId = data.ParentReferenceId,
                                ProjectName = data.ProjectName,
                                ProjectId = data.ProjectId,
                                Description = data.Description,
                                ActivityType = data.ActivityType,
                                ClientType = data.ClientType,
                                ClientName = data.ClientName,
                                ClientCode = data.ClientCode,
                                PlannedStartDate = data.PlannedStartDate,
                                PlannedEndDate = data.PlannedEndDate,
                                Comments = data.Comments,
                                Status = data.Status,
                                TimeStamp = data.TimeStamp,
                                NotificationDate = data.NotificationDate,
                                PhoneDbId = data.PhoneDbId,
                                TaskId = 0,
                                TaskAssignmentId = 0,
                                IsProcessed = false,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                            });
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.TaskSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        // TaskActions 20211009

        public static void SaveSqliteTaskActionDataWrapper(long employeeId, long batchId,
        IEnumerable<SqliteDomainTaskAction> domainTaskAction)
        {
            try
            {
                DBLayer.SaveSqliteTaskActionData(employeeId, batchId, domainTaskAction);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"FollowUp/Task Action Data not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteDayPlanTargetData)}", errorText, logSnip);
            }
        }

        private static void SaveSqliteTaskActionData(long employeeId, long batchId,
            IEnumerable<SqliteDomainTaskAction> domainTaskAction)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();

                int rowsSaved = 0;
                foreach (var data in domainTaskAction)
                {
                    orm.SqliteTaskActions.Add(
                            new SqliteTaskAction()
                            {
                                BatchId = batchId,
                                EmployeeId = employeeId,
                                IsNewTask = data.IsNewTask,
                                TaskId = data.TaskId,
                                ParentReferenceTaskId = data.ParentReferenceTaskId,
                                SqliteActionPhoneDbId = data.SqliteActionPhoneDbId,
                                Status = data.Status,
                                TimeStamp = data.TimeStamp,
                                NotificationDate = data.NotificationDate,
                                PhoneDbId = data.PhoneDbId,
                                TaskActionId = 0,
                                TaskAssignmentId = data.TaskAssignmentId,
                                IsProcessed = false,
                                DateCreated = DateTime.UtcNow,
                                DateUpdated = DateTime.UtcNow,
                            });
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.TaskActionSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        public static DateTime GetRecentSMSSentTime(long tenantId, string smsTypeName)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DateTime dt = orm.TenantSMSLogs.AsNoTracking()
                .Where(x => x.TenantId == tenantId && x.SmsType == smsTypeName)
                .OrderByDescending(x => x.Id)
                .Select(x => x.SMSDateTime)
                .FirstOrDefault();

            // if no record is found, dt will be set to DateTime.MinValue
            return dt;
        }

        public static ICollection<TimeSpan> TenantSMSSchedule(long tenantId, long smsTypeId, string dayName)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TenantSMSSchedules.Where(x => x.TenantId == tenantId &&
                                            x.Tenant.IsActive && x.IsActive &&
                                            x.WeekDayName == dayName && x.TenantSmsTypeId == smsTypeId)
                .OrderBy(x => x.SMSAt)
                .Select(x => x.SMSAt).ToList();
        }

        public static ICollection<DomainEntities.TenantHoliday> GetUpcomingHoidays(long tenantId, DateTime justDate)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TenantHolidays.AsNoTracking()
                .Where(x => x.TenantId == tenantId && x.HolidayDate >= justDate && x.IsActive && x.Tenant.IsActive)
                .Select(x => new DomainEntities.TenantHoliday()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    HolidayDate = x.HolidayDate,
                    Description = x.Description,
                    TenantName = x.Tenant.Name
                }).ToList();
        }

        public static ICollection<DomainEntities.TenantSmsType> GetSmsTypes(long tenantId = 0)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var resultSet = orm.TenantSmsTypes.AsNoTracking()
                .Where(x => x.IsActive && x.Tenant.IsActive)
                .Select(x => new DomainEntities.TenantSmsType()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    TenantName = x.Tenant.Name,
                    TypeName = x.TypeName,
                    MessageText = x.MessageText,
                    SprocName = x.SprocName,
                    SmsProcessClass = x.SmsProcessClass,
                    PlaceHolders = x.PlaceHolders
                });

            if (tenantId > 0)
            {
                resultSet = resultSet.Where(x => x.TenantId == tenantId);
            }

            return resultSet.ToList();
        }

        public static ICollection<DomainEntities.TenantWeekDay> GetWeekDays(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TenantWorkDays.AsNoTracking()
                .Where(x => x.TenantId == tenantId && x.Tenant.IsActive)
                .Select(x => new DomainEntities.TenantWeekDay()
                {
                    WeekDayName = x.WeekDayName,
                    IsWorkingDay = x.IsWorkingDay
                }).ToList();
        }

        public static IEnumerable<SqliteActionDisplayData> GetDupBatchItems(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteActionDups
                .Where(x => x.BatchId == batchId)
                .OrderByDescending(x => x.At)
                .Select(x => new SqliteActionDisplayData()
                {
                    Id = x.Id,
                    PhoneDbId = x.PhoneDbId,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    At = x.At,
                    Name = x.Name,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    MNC = x.MNC,
                    MCC = x.MCC,
                    LAC = x.LAC,
                    CellId = x.CellId,
                    ClientName = x.ClientName,
                    ClientPhone = x.ClientPhone,
                    ClientType = x.ClientType,
                    ActivityType = x.ActivityType,
                    Comments = x.Comments,
                    DateCreated = x.DateCreated,
                    ClientCode = x.ClientCode,
                    ContactCount = x.ContactCount,
                    AtBusiness = x.AtBusiness,
                    ActivityAmount = x.ActivityAmount
                }).ToList();
        }

        public static List<SqliteActionProcessData> GetSqliteActionRecords(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            // retrieve ids first - which is going to be a small list.
            List<long> actionIds = orm.SqliteActions
                        .Where(x => x.BatchId == batchId)
                        .Select(x => x.Id)
                        .ToList();

            if ((actionIds?.Count ?? 0) <= 0)
            {
                return null;
            }

            // now do a lookup based on ids
            List<SqliteActionProcessData> resultSet = orm.SqliteActions
                //.Where(x => actionIds.Any(y => y == x.Id))
                .Where(x => x.BatchId == batchId)
                .OrderBy(x => x.Id)
                .Select(x => new SqliteActionProcessData()
                {
                    Id = x.Id,
                    SqliteTableId = x.SqliteTableId,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    At = x.At,
                    ActivityTrackingtype = x.ActivityTrackingType,
                    //Name = x.Name,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    MNC = x.MNC,
                    MCC = x.MCC,
                    LAC = x.LAC,
                    CellId = x.CellId,
                    ClientName = x.ClientName,
                    ClientPhone = x.ClientPhone,
                    ClientType = x.ClientType,
                    ActivityType = x.ActivityType,
                    Comments = x.Comments,
                    IsProcessed = x.IsProcessed,
                    IsPostedSuccessfully = x.IsPostedSuccessfully,
                    TrackingId = x.TrackingId,
                    ActivityId = x.ActivityId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    ImageCount = x.ImageCount,
                    Images = x.SqliteActionImages.OrderBy(y => y.SequenceNumber).Select(y => y.ImageFileName).ToList(),
                    PhoneModel = x.PhoneModel,
                    PhoneOS = x.PhoneOS,
                    AppVersion = x.AppVersion,
                    ClientCode = x.ClientCode,
                    IMEI = x.IMEI,
                    ContactCount = x.ContactCount,
                    AtBusiness = x.AtBusiness,
                    InstrumentId = x.InstrumentId,
                    ActivityAmount = x.ActivityAmount,

                    Contacts = x.SqliteActionContacts.OrderBy(z => z.Id).Select(z => new SqliteActionContactData()
                    {
                        Id = z.Id,
                        SqliteActionId = z.SqliteActionId,
                        Name = z.Name,
                        PhoneNumber = z.PhoneNumber,
                        IsPrimary = z.IsPrimary
                    }).ToList(),

                    Locations = x.SqliteActionLocations
                    .Where(k => k.IsGood && k.Latitude != 0 && k.Longitude != 0)
                    .Select(k => new SqliteActionLocationData()
                    {
                        Id = k.Id,
                        SqliteActionId = k.SqliteActionId,
                        Source = k.Source,
                        Latitude = k.Latitude,
                        Longitude = k.Longitude,
                        LocationTaskStatus = k.LocationTaskStatus,
                        LocationException = k.LocationException,
                        IsGood = k.IsGood
                    }).ToList()
                }).ToList();

            // update locationCount in resultSet
            //foreach(var r in resultSet)
            //{
            //    r.LocationCount = r.Locations.Count();
            //}

            var p = resultSet.Select(x => x.LocationCount = x.Locations.Count()).ToList();
            return resultSet;
        }

        public static void UpdateBatchProcessLogRecord(long logId, bool completed, bool failed)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var entity = orm.SqliteActionProcessLogs.Where(x => x.Id == logId).FirstOrDefault();
            if (entity != null)
            {
                entity.Timestamp = DateTime.UtcNow;
                entity.HasCompleted = completed;
                entity.HasFailed = failed;
                orm.SaveChanges();
            }
        }

        public static void UpdateSMSLogRecord(long logId, bool completed, bool failed)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var entity = orm.SMSProcessLogs.Where(x => x.Id == logId).FirstOrDefault();
            if (entity != null)
            {
                entity.Timestamp = DateTime.UtcNow;
                entity.HasCompleted = completed;
                entity.HasFailed = failed;
                orm.SaveChanges();
            }
        }

        public static void SaveProcessedSqliteAction(SqliteActionProcessData item)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.SqliteActions.Where(x => x.Id == item.Id).FirstOrDefault();
            if (rec != null)
            {
                rec.IsProcessed = true;
                rec.TrackingId = item.TrackingId;
                rec.ActivityId = item.ActivityId;
                rec.DateUpdated = DateTime.UtcNow;
                rec.DerivedLatitude = item.DerivedLatitude;
                rec.DerivedLongitude = item.DerivedLongitude;
                rec.DerivedLocSource = item.DerivedLocSource;
                orm.SaveChanges();
            }
        }

        public static void SaveSqliteEntityData(SqliteEntityData item)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.SqliteEntities.Where(x => x.Id == item.Id).FirstOrDefault();
            if (rec != null)
            {
                rec.DateUpdated = DateTime.UtcNow;
                rec.DerivedLatitude = item.DerivedLatitude;
                rec.DerivedLongitude = item.DerivedLongitude;
                rec.DerivedLocSource = item.DerivedLocSource;
                orm.SaveChanges();
            }
        }

        public static void MarkSqliteBatchAsProcessed(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
            if (batchRecord != null)
            {
                batchRecord.LockTimestamp = null;
                batchRecord.BatchProcessed = true;
                orm.SaveChanges();
            }
        }

        public static void MarkCustomerSMSSentInOrder(long orderId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var orderRecord = orm.Orders.Where(x => x.Id == orderId).FirstOrDefault();
            if (orderRecord != null)
            {
                orderRecord.LockTimestamp = null;
                orderRecord.IsCustomerSMSSent = true;
                orm.SaveChanges();
            }
        }

        public static void MarkSmsDataRecord(long id, bool isSent, bool isFailed, string failedText)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.TenantSmsDatas.Find(id);
            if (rec != null)
            {
                rec.LockTimestamp = null;
                rec.Timestamp = DateTime.UtcNow;
                rec.IsSent = isSent;
                rec.IsFailed = isFailed;
                rec.FailedText = failedText;
                orm.SaveChanges();
            }
        }

        public static void MarkAgentSMSSentInOrder(long orderId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var orderRecord = orm.Orders.Where(x => x.Id == orderId).FirstOrDefault();
            if (orderRecord != null)
            {
                orderRecord.LockTimestamp = null;
                orderRecord.IsAgentSMSSent = true;
                orm.SaveChanges();
            }
        }

        public static IEnumerable<long> GetOrderForCustomerSMS(int recordCount, long tenantId, long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var collection = (IEnumerable<GetOrderForCustomerSMS_Result>)orm.GetOrderForCustomerSMS(recordCount, tenantId, employeeId);
            if (collection != null)
            {
                return collection.Select(x => x.OrderId).ToList();
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<DomainEntities.TenantSmsData> GetTenantSMSData(int recordCount, long tenantId, string smsType)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var collection = (IEnumerable<GetTenantSmsData_Result>)orm.GetTenantSmsData(recordCount, tenantId, smsType);
            if (collection != null)
            {
                return collection.Select(x => new DomainEntities.TenantSmsData()
                {
                    Id = x.TenantSmsDataId,
                    DataType = x.DataType,
                    MessageData = x.MessageData
                }).ToList();
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<long> GetOrderForAgentSMS(int recordCount, long tenantId, long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var collection = (IEnumerable<GetOrderForAgentSMS_Result>)orm.GetOrderForAgentSMS(recordCount, tenantId, employeeId);
            if (collection != null)
            {
                return collection.Select(x => x.OrderId).ToList();
            }
            else
            {
                return null;
            }
        }

        public static IEnumerable<long> GetSqliteBatchForProcessing(int recordCount, long tenantId, long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var collection = (IEnumerable<GetSqliteActionBatchForProcessing_Result>)orm.GetSqliteActionBatchForProcessing(recordCount, tenantId, employeeId);
            if (collection != null)
            {
                return collection.Select(x => x.BatchId).ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// try to lock the tenant record for SMS Processing;
        /// Uses transaction with repeatable Read.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static bool LockTenantForSMSProcessing(long tenantId, int AutoReleaseLockTimeInMinutes)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                if (tenantRecord.IsSendingSMS == false ||
                    (tenantRecord.IsSendingSMS == true
                            && DateTime.UtcNow.Subtract(tenantRecord.SMSProcessingAt.Value).TotalMinutes
                                        > AutoReleaseLockTimeInMinutes))
                {
                    tenantRecord.IsSendingSMS = true;
                    tenantRecord.SMSProcessingAt = DateTime.UtcNow;
                    orm.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public static bool RenewLeaseForSMSProcessing(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsSendingSMS = true;
                tenantRecord.SMSProcessingAt = DateTime.UtcNow;
                orm.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// try to lock the tenant record for Mobile Data Processing;
        /// Uses transaction with repeatable Read.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static bool LockTenantForMobileDataProcessing(long tenantId, int minutesToWaitBeforeAutoUnlock = 15)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                if (tenantRecord.IsProcessingMobileData == false ||
                    (tenantRecord.IsProcessingMobileData == true &&
                        DateTime.UtcNow.Subtract(tenantRecord.MobileDataProcessingAt.Value)
                                .TotalMinutes > minutesToWaitBeforeAutoUnlock))
                {
                    tenantRecord.IsProcessingMobileData = true;
                    tenantRecord.MobileDataProcessingAt = DateTime.UtcNow;
                    orm.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public static bool RenewLeaseForMobileDataProcessing(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsProcessingMobileData = true;
                tenantRecord.MobileDataProcessingAt = DateTime.UtcNow;
                orm.SaveChanges();
                return true;
            }
        }

        public static bool LockTenantForImageUploadToS3(long tenantId, int minutesToWaitBeforeAutoUnlock = 15)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                if (tenantRecord.IsUploadingImage == false ||
                    (tenantRecord.IsUploadingImage == true &&
                        DateTime.UtcNow.Subtract(tenantRecord.UploadingImageAt.Value)
                                .TotalMinutes > minutesToWaitBeforeAutoUnlock))
                {
                    tenantRecord.IsUploadingImage = true;
                    tenantRecord.UploadingImageAt = DateTime.UtcNow;
                    orm.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public static bool RenewLeaseForImageUploadToS3(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsUploadingImage = true;
                tenantRecord.UploadingImageAt = DateTime.UtcNow;
                orm.SaveChanges();
                return true;
            }
        }

        public static void SaveSqliteExpenseDataWrapper(long employeeId, long batchId, SqliteDomainExpense expenseObject)
        {
            try
            {
                SaveSqliteExpenseData(employeeId, batchId, expenseObject);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"ExpenseData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError("SaveSqliteExpenseData", errorText, logSnip);
            }
        }

        private static void SaveSqliteExpenseData(long employeeId, long batchId, SqliteDomainExpense expenseObject)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch entry
                var batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();
                //batchRecord.ExpenseLineInputCount = expenseObject.ExpenseItemCount;
                //batchRecord.TotalExpenseAmount = expenseObject.TotalAmount;
                //batchRecord.ExpenseDate = expenseObject.TimeStamp;

                long rowsSaved = 0;
                int seqNo = 0;
                foreach (SqliteDomainExpenseItem domainExpense in expenseObject.ExpenseItems)
                {
                    seqNo++;
                    SqliteExpense exp = new SqliteExpense()
                    {
                        SqliteTableId = expenseObject.Id,
                        BatchId = batchId,
                        EmployeeId = employeeId,
                        ExpenseType = domainExpense.ExpenseType,
                        Amount = domainExpense.Amount,
                        OdometerStart = domainExpense.OdometerStart,
                        OdometerEnd = domainExpense.OdometerEnd,
                        VehicleType = domainExpense.VehicleType,
                        IsProcessed = false,
                        ExpenseItemId = 0,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
                        SequenceNumber = seqNo,
                        ImageCount = (domainExpense.Images != null) ? domainExpense.Images.Count() : 0,
                        FuelType = domainExpense.FuelType,
                        FuelQuantityInLiters = domainExpense.FuelQuantityInLiters,
                        Comment = domainExpense.Comment
                    };

                    if (exp.ImageCount > 0)
                    {
                        int imgSeqNo = 0;
                        foreach (var imgFile in domainExpense.Images)
                        {
                            imgSeqNo++;
                            SqliteExpenseImage imageEntity = new SqliteExpenseImage()
                            {
                                ImageFileName = imgFile,
                                SequenceNumber = imgSeqNo
                            };
                            exp.SqliteExpenseImages.Add(imageEntity);
                        }
                    }

                    orm.SqliteExpenses.Add(exp);
                    rowsSaved += 1;
                }

                // now update the rows added to the batch record
                batchRecord.ExpenseLineSavedCount = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        /// <summary>
        /// try to lock the tenant record for Data Feed Processing;
        /// Uses transaction with repeatable Read.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static bool LockTenantForDataFeedProcessing(long tenantId, int AutoReleaseLockTimeInMinutes)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                if (tenantRecord.IsTransformingDataFeed == false ||
                    (tenantRecord.IsTransformingDataFeed == true &&
                            DateTime.UtcNow.Subtract(tenantRecord.TransformingDataFeedAt.Value).TotalMinutes
                                                > AutoReleaseLockTimeInMinutes))
                {
                    tenantRecord.IsTransformingDataFeed = true;
                    tenantRecord.TransformingDataFeedAt = DateTime.UtcNow;
                    orm.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public static bool RenewLeaseForDataFeedProcessing(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsTransformingDataFeed = true;
                tenantRecord.TransformingDataFeedAt = DateTime.UtcNow;
                orm.SaveChanges();
                return true;
            }
        }

        public static bool LockTenantForUploadParseFile(long tenantId, int AutoReleaseLockTimeInMinutes)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                if (tenantRecord.IsParsingUploadFile == false ||
                    (tenantRecord.IsParsingUploadFile == true &&
                        DateTime.UtcNow.Subtract(tenantRecord.ParsingUploadAt.Value).TotalMinutes
                                    > AutoReleaseLockTimeInMinutes))
                {
                    tenantRecord.IsParsingUploadFile = true;
                    tenantRecord.ParsingUploadAt = DateTime.UtcNow;
                    orm.SaveChanges();
                    return true;
                }

                return false;
            }
        }

        public static bool RenewLeaseForUploadParseFile(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsParsingUploadFile = true;
                tenantRecord.ParsingUploadAt = DateTime.UtcNow;
                orm.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// unlock the tenant record for Mobile Data Processing;
        /// Uses transaction with repeatable Read.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static bool UnLockTenantFromMobileDataProcessing(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsProcessingMobileData = false;
                tenantRecord.MobileDataProcessingAt = null;
                orm.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// unlock the tenant record for SMS Data Processing;
        /// Uses transaction with repeatable Read.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static bool UnLockTenantFromSMSProcessing(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsSendingSMS = false;
                tenantRecord.SMSProcessingAt = null;
                orm.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// unlock the tenant record for Data Feed Processing;
        /// Uses transaction with repeatable Read.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static bool UnLockTenantFromDataFeedProcessing(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId && x.IsTransformingDataFeed == true)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsTransformingDataFeed = false;
                orm.SaveChanges();
                return true;
            }
        }

        public static bool UnLockTenantFromUploadParseFile(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId && x.IsParsingUploadFile == true)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsParsingUploadFile = false;
                orm.SaveChanges();
                return true;
            }
        }

        public static bool UnLockTenantFromImageUploadToS3(long tenantId)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var tenantRecord = orm.Tenants
                            .Where(x => x.IsActive && x.Id == tenantId)
                            .FirstOrDefault();

                if (tenantRecord == null)
                {
                    return false;
                }

                tenantRecord.IsUploadingImage = false;
                tenantRecord.UploadingImageAt = null;
                orm.SaveChanges();
                return true;
            }
        }

        public static IEnumerable<SqliteActionDisplayData> GetSavedBatchItems(long batchId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteActions
                .Where(x => x.BatchId == batchId)
                .OrderByDescending(x => x.At)
                .Select(x => new SqliteActionDisplayData()
                {
                    Id = x.Id,
                    PhoneDbId = x.PhoneDbId,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    At = x.At,
                    Name = x.Name,
                    ActivityTrackingType = x.ActivityTrackingType,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    MNC = x.MNC,
                    MCC = x.MCC,
                    LAC = x.LAC,
                    CellId = x.CellId,
                    ClientName = x.ClientName,
                    ClientPhone = x.ClientPhone,
                    ClientType = x.ClientType,
                    ActivityType = x.ActivityType,
                    Comments = x.Comments,
                    IsProcessed = x.IsProcessed,
                    IsPostedSuccessfully = x.IsPostedSuccessfully,
                    TrackingId = x.TrackingId,
                    ActivityId = x.ActivityId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    ImageCount = x.ImageCount,
                    PhoneModel = x.PhoneModel,
                    PhoneOS = x.PhoneOS,
                    AppVersion = x.AppVersion,
                    ClientCode = x.ClientCode,
                    IMEI = x.IMEI,
                    ContactCount = x.ContactCount,
                    AtBusiness = x.AtBusiness,
                    InstrumentId = x.InstrumentId,
                    ActivityAmount = x.ActivityAmount,
                    LocationException = x.LocationException,
                    LocationTaskStatus = x.LocationTaskStatus,
                    LocationCount = x.LocationCount,
                    DerivedLocSource = x.DerivedLocSource ?? "",
                    DerivedLatitude = x.DerivedLatitude,
                    DerivedLongitude = x.DerivedLongitude
                }).ToList();
        }

        public static IEnumerable<SqliteActionContactData> GetSavedSqliteActionContacts(long actionId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteActionContacts.Where(x => x.SqliteActionId == actionId)
                .Select(x => new SqliteActionContactData()
                {
                    Id = x.Id,
                    SqliteActionId = x.SqliteActionId,
                    Name = x.Name,
                    PhoneNumber = x.PhoneNumber,
                    IsPrimary = x.IsPrimary
                });
        }

        public static IEnumerable<SqliteActionLocationData> GetSavedSqliteActionLocations(long actionId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteActionLocations.Where(x => x.SqliteActionId == actionId)
                .Select(x => new SqliteActionLocationData()
                {
                    Id = x.Id,
                    SqliteActionId = x.SqliteActionId,
                    Source = x.Source,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    UtcAt = x.UtcAt,
                    LocationTaskStatus = x.LocationTaskStatus,
                    LocationException = x.LocationException,
                    IsGood = x.IsGood
                });
        }

        public static IEnumerable<SqliteDWSData> GetSavedSqliteSTRDWS(long sqliteSTRId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteSTRDWS.Where(x => x.SqliteSTRId == sqliteSTRId)
                .Select(x => new SqliteDWSData()
                {
                    Id = x.Id,
                    SqliteSTRId = sqliteSTRId,
                    IsProcessed = x.IsProcessed,
                    DWSId = x.DWSId,
                    EntityId = x.EntityId,
                    EntityName = x.EntityName,
                    AgreementId = x.AgreementId,
                    Agreement = x.Agreement,
                    EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                    TypeName = x.TypeName,
                    TagName = x.TagName,
                    DWSNumber = x.DWSNumber,
                    BagCount = x.BagCount,
                    FilledBagsWeightKg = x.FilledBagsWeightKg,
                    EmptyBagsWeightKg = x.EmptyBagsWeightKg,
                    TimeStamp = x.DWSDate,
                    ActivityId = x.ActivityId,
                });
        }

        public static IEnumerable<SqliteEntityLocationData> GetSavedSqliteEntityLocations(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteEntityLocations.Where(x => x.SqliteEntityId == entityId)
                .Select(x => new SqliteEntityLocationData()
                {
                    Id = x.Id,
                    SqliteEntityId = x.SqliteEntityId,
                    Source = x.Source,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    UtcAt = x.UtcAt,
                    LocationTaskStatus = x.LocationTaskStatus,
                    LocationException = x.LocationException,
                    IsGood = x.IsGood
                });
        }

        public static IEnumerable<DomainEntities.SqliteEntityWorkFlowFollowUp> GetSavedSqliteWorkFlowFollowUps(long ewfId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteEntityWorkFlowFollowUps.Where(x => x.SqliteEntityWorkFlowId == ewfId)
                .Select(x => new DomainEntities.SqliteEntityWorkFlowFollowUp()
                {
                    Phase = x.Phase,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Notes = x.Notes
                });
        }

        public static IEnumerable<string> GetSavedSqliteWorkFlowItemsUsed(long ewfId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SqliteEntityWorkFlowItemUseds.Where(x => x.SqliteEntityWorkFlowId == ewfId)
                .Select(x => x.ItemName)
                .ToList();
        }

        public static IEnumerable<string> GetWorkFlowDetailItemsUsed(long id)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityWorkFlowDetailItemUseds.Where(x => x.EntityWorkFlowDetailId == id)
                .Select(x => x.ItemName)
                .ToList();
        }

        public static IEnumerable<SqliteDomainBatch> GetBatches(long employeeId, bool onlyLockedBatches, bool onlyUnprocessedBatches, long inRecentBatches = 3000)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (s) => LogError(nameof(GetBatches), s, ">");

            // retrieve the max id from sqliteActionBatch and check the data in last 10000 batches only
            // https://stackoverflow.com/questions/2268175/using-linq-to-sql-how-do-i-find-min-and-max-of-a-column-in-a-table

            var max = (from b in orm.SqliteActionBatches
                       group b by true into b1
                       select b1.Max(z => z.Id))
                      .FirstOrDefault();

            var startMax = max - inRecentBatches;

            IQueryable<SqliteDomainBatch> resultSet = (from b in orm.SqliteActionBatches
                                                       join e in orm.TenantEmployees on b.EmployeeId equals e.Id
                                                       where b.Id > startMax
                                                       orderby e.Name ascending, b.At descending
                                                       //orderby b.At descending
                                                       select new SqliteDomainBatch()
                                                       {
                                                           Id = b.Id,
                                                           BatchGuid = b.BatchGuid,
                                                           EmployeeId = e.Id,
                                                           EmployeeName = e.Name,
                                                           EmployeeCode = e.EmployeeCode,
                                                           BatchInputCount = b.BatchInputCount,
                                                           BatchSavedCount = b.BatchSavedCount,
                                                           DuplicateItemCount = b.DuplicateItemCount,
                                                           ExpenseLineInputCount = b.ExpenseLineInputCount,
                                                           ExpenseLineSavedCount = b.ExpenseLineSavedCount,
                                                           ExpenseLineRejectCount = b.ExpenseLineRejectCount,
                                                           DuplicateExpenseCount = b.DuplicateExpenseCount,

                                                           NumberOfOrders = b.NumberOfOrders,
                                                           NumberOfOrderLines = b.NumberOfOrderLines,
                                                           NumberOfOrdersSaved = b.NumberOfOrdersSaved,
                                                           NumberOfOrderLinesSaved = b.NumberOfOrderLinesSaved,
                                                           TotalOrderAmount = b.TotalOrderAmount,
                                                           DuplicateOrderCount = b.DuplicateOrderCount,

                                                           NumberOfReturns = b.NumberOfReturns,
                                                           NumberOfReturnLines = b.NumberOfReturnLines,
                                                           TotalReturnAmount = b.TotalReturnAmount,
                                                           NumberOfReturnsSaved = b.NumberOfReturnsSaved,
                                                           NumberOfReturnLinesSaved = b.NumberOfReturnLinesSaved,
                                                           DuplicateReturnCount = b.DuplicateReturnCount,

                                                           NumberOfPayments = b.NumberOfPayments,
                                                           NumberOfPaymentsSaved = b.NumberOfPaymentsSaved,
                                                           TotalPaymentAmount = b.TotalPaymentAmount,
                                                           DuplicatePaymentCount = b.DuplicatePaymentCount,

                                                           NumberOfEntities = b.NumberOfEntities,
                                                           NumberOfEntitiesSaved = b.NumberOfEntitiesSaved,
                                                           DuplicateEntityCount = b.DuplicateEntityCount,

                                                           NumberOfLeaves = b.NumberOfLeaves,
                                                           NumberOfLeavesSaved = b.NumberOfLeavesSaved,

                                                           NumberOfCancelledLeaves = b.NumberOfCancelledLeaves,
                                                           NumberOfCancelledLeavesSaved = b.NumberOfCancelledLeavesSaved,

                                                           NumberOfIssueReturns = b.NumberOfIssueReturns,
                                                           NumberOfIssueReturnsSaved = b.NumberOfIssueReturnsSaved,

                                                           NumberOfWorkFlow = b.NumberOfWorkFlow,
                                                           NumberOfWorkFlowSaved = b.NumberOfWorkFlowSaved,

                                                           DeviceLogCount = b.DeviceLogCount,
                                                           NumberOfImages = b.NumberOfImages,
                                                           NumberOfImagesSaved = b.NumberOfImagesSaved,

                                                           At = b.At,
                                                           Timestamp = b.Timestamp,
                                                           BatchProcessed = b.BatchProcessed,
                                                           LockTimestamp = b.LockTimestamp,
                                                           TotalExpenseAmount = b.TotalExpenseAmount,
                                                           ExpenseDate = b.ExpenseDate,
                                                           ExpenseId = b.ExpenseId,
                                                           DataFileName = b.DataFileName,
                                                           DataFileSize = b.DataFileSize,
                                                           Agreements = b.Agreements,
                                                           AgreementsSaved = b.AgreementsSaved,

                                                           Surveys = b.Surveys,
                                                           SurveysSaved = b.SurveysSaved,

                                                           DayPlanTarget = b.DayPlanTarget,
                                                           DayPlanTargetSaved = b.DayPlanTargetSaved,

                                                           Task = b.Task,
                                                           TaskSaved = b.TaskSaved,

                                                           TaskAction = b.TaskAction,
                                                           TaskActionSaved = b.TaskActionSaved,

                                                           QuestionnaireTarget = b.QuestionnaireTarget,
                                                           QuestionnaireTargetSaved = b.QuestionnaireTargetSaved,

                                                           AdvanceRequests = b.AdvanceRequests,
                                                           AdvanceRequestsSaved = b.AdvanceRequestsSaved,
                                                           TerminateRequests = b.TerminateRequests,
                                                           TerminateRequestsSaved = b.TerminateRequestsSaved,

                                                           BankDetails = b.BankDetails,
                                                           BankDetailsSaved = b.BankDetailsSaved,
                                                           STRCount = b.STRCount,
                                                           STRSavedCount = b.STRSavedCount,
                                                       });

            if (employeeId > 0)
            {
                resultSet = resultSet.Where(x => x.EmployeeId == employeeId);
            }

            //if (inPastMinutes > 0)
            //{
            //    DateTime dt = DateTime.UtcNow.AddMinutes(inPastMinutes * -1);
            //    resultSet = resultSet.Where(x => x.At >= dt);
            //}

            if (onlyLockedBatches)
            {
                resultSet = resultSet.Where(x => x.LockTimestamp.HasValue);
            }

            if (onlyUnprocessedBatches)
            {
                resultSet = resultSet.Where(x => !x.BatchProcessed && !x.LockTimestamp.HasValue);
            }

            return resultSet.ToList();
        }

        // single expense Id
        public static DomainEntities.Expense GetExpense(long expenseId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (x) => DBLayer.LogError("GetExpense", x, "");
            return (orm.Expenses.Where(x => x.Id == expenseId)
                .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                (ex, sp) =>
                 new DomainEntities.Expense()
                 {
                     Id = ex.Id,
                     EmployeeId = ex.EmployeeId,
                     DayId = ex.DayId,
                     TotalAmount = ex.TotalAmount,
                     IsZoneApproved = ex.IsZoneApproved,
                     IsAreaApproved = ex.IsAreaApproved,
                     IsTerritoryApproved = ex.IsTerritoryApproved,

                     EmployeeCode = ex.TenantEmployee.EmployeeCode,
                     EmployeeName = ex.TenantEmployee.Name,
                     ExpenseDate = ex.Day.DATE,
                     StaffCode = sp.StaffCode
                 })).FirstOrDefault();
        }

        public static ICollection<DomainEntities.ExpenseApproval> GetExpenseApprovals(long expenseId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.ExpenseApprovals.Where(x => x.ExpenseId == expenseId)
                 .Select(ex => new DomainEntities.ExpenseApproval()
                 {
                     Id = ex.Id,
                     ExpenseId = ex.ExpenseId,
                     ApproveLevel = ex.ApproveLevel,
                     ApproveDate = ex.ApproveDate,
                     ApproveRef = ex.ApproveRef,
                     ApproveNotes = ex.ApproveNotes,
                     ApproveAmount = ex.ApproveAmount,
                     ApprovedBy = ex.ApprovedBy
                 }).ToList();
        }

        public static IEnumerable<DomainEntities.Expense> GetExpenses(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (x) => DBLayer.LogError("GetExpense", x, "");
            IQueryable<DomainEntities.Expense> expenses = orm.Expenses
                .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                (ex, sp) =>
                 new DomainEntities.Expense()
                 {
                     Id = ex.Id,
                     EmployeeId = ex.EmployeeId,
                     DayId = ex.DayId,
                     TotalAmount = ex.TotalAmount,

                     EmployeeCode = ex.TenantEmployee.EmployeeCode,
                     EmployeeName = ex.TenantEmployee.Name,
                     ExpenseDate = ex.Day.DATE,
                     StaffCode = sp.StaffCode,
                     IsTerritoryApproved = ex.IsTerritoryApproved,
                     IsAreaApproved = ex.IsAreaApproved,
                     IsZoneApproved = ex.IsZoneApproved,
                     Phone = sp.Phone,
                     IsActive = ex.TenantEmployee.IsActive,
                     IsActiveInSap = (sp != null) ? sp.IsActive : false,
                     Approvals = ex.ExpenseApprovals.Select(x => new DomainEntities.ExpenseApproval()
                     {
                         ApproveAmount = x.ApproveAmount,
                         ApproveDate = x.ApproveDate,
                         ApprovedBy = x.ApprovedBy,
                         ApproveLevel = x.ApproveLevel,
                         ApproveNotes = x.ApproveNotes,
                         ApproveRef = x.ApproveRef,
                         Id = x.Id,
                         ExpenseId = x.ExpenseId
                     }).ToList()
                 });

            if (searchCriteria.ApplyAmountFilter)
            {
                expenses = expenses.Where(x => x.TotalAmount >= searchCriteria.AmountFrom && x.TotalAmount <= searchCriteria.AmountTo);
            }

            if (searchCriteria.ApplyDateFilter)
            {
                expenses = expenses.Where(x => x.ExpenseDate >= searchCriteria.DateFrom && x.ExpenseDate <= searchCriteria.DateTo);
            }

            if (searchCriteria.ApplyDataStatusFilter)
            {
                expenses = expenses.Where(x =>
                 x.IsTerritoryApproved == searchCriteria.TMApprovedExpense &&
                 x.IsAreaApproved == searchCriteria.AMApprovedExpense &&
                 x.IsZoneApproved == searchCriteria.ZMApprovedExpense);
            }
            if (searchCriteria.ApplyEmployeeStatusFilter)
            {
                expenses = expenses.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                expenses = expenses.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                expenses = expenses.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            return expenses.ToList();  // amount, date and status filters so far are applied;
        }

        public static ICollection<DomainEntities.BulkExpense> GetExpensesById(IEnumerable<long> expenseId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            ICollection<DomainEntities.BulkExpense> expenses = orm.Expenses.Where(x => expenseId.Contains(x.Id)).Select(x => 
                 new DomainEntities.BulkExpense()
                 {
                     Id = x.Id,
                     EmployeeId = x.EmployeeId,                    
                     TotalAmount = x.TotalAmount,
                     Approvals = x.ExpenseApprovals.Select(y => new DomainEntities.ExpenseApproval()
                     {                     
                         ExpenseId = y.ExpenseId
                     }).ToList()
                 }).ToList();
            
          
            return expenses;  // amount, date and status filters so far are applied;
        }

        public static IEnumerable<DomainEntities.IssueReturn> GetIssueReturns(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (x) => DBLayer.LogError("GetExpense", x, "");
            IQueryable<DomainEntities.IssueReturn> issueReturns = orm.IssueReturns
                .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                (ex, sp) =>
                 new DomainEntities.IssueReturn()
                 {
                     Id = ex.Id,
                     EmployeeId = ex.EmployeeId,
                     EmployeeCode = ex.TenantEmployee.EmployeeCode,
                     EmployeeName = ex.TenantEmployee.Name,
                     HQCode = (sp == null) ? string.Empty : sp.HQCode,
                     DayId = ex.DayId,
                     TransactionDate = ex.TransactionDate,
                     StaffCode = sp.StaffCode,
                     Phone = sp.Phone,
                     IsActive = ex.TenantEmployee.IsActive,
                     IsActiveInSap = (sp != null) ? sp.IsActive : false,

                     EntityAgreementId = ex.EntityAgreementId == null ? 0 : ex.EntityAgreementId.Value,
                     AgreementNumber = ex.EntityAgreement == null ? "" : ex.EntityAgreement.AgreementNumber,
                     WorkflowSeasonId = ex.EntityAgreement == null ? 0 : ex.EntityAgreement.WorkflowSeasonId,
                     WorkflowSeasonName = ex.EntityAgreement == null ? "" : ex.EntityAgreement.WorkflowSeason.SeasonName,
                     TypeName = ex.EntityAgreement == null ? "" : ex.EntityAgreement.WorkflowSeason.TypeName,
                     EntityId = ex.EntityId,
                     EntityType = ex.Entity.EntityType,
                     EntityName = ex.Entity.EntityName,

                     ItemMasterId = ex.ItemMasterId,
                     ItemType = ex.ItemMaster1.Category,
                     ItemCode = ex.ItemMaster1.ItemCode,
                     ItemUnit = ex.ItemMaster1.Unit,

                     TransactionType = ex.TransactionType,
                     Quantity = ex.Quantity,
                     ReportType = searchCriteria.ReportType,

                     ActivityId = ex.ActivityId,
                     SlipNumber = ex.SlipNumber,
                     LandSizeInAcres = ex.LandSizeInAcres,
                     ItemRate = ex.ItemRate,

                     AppliedTransactionType = ex.AppliedTransactionType,
                     AppliedItemMasterId = ex.AppliedItemMasterId,
                     AppliedItemType = ex.ItemMaster.Category,
                     AppliedItemCode = ex.ItemMaster.ItemCode,
                     AppliedItemUnit = ex.ItemMaster.Unit,
                     AppliedQuantity = ex.AppliedQuantity,
                     AppliedItemRate = ex.AppliedItemRate,
                     Status = ex.Status,
                     DateUpdated = ex.DateUpdated,
                     Comments = ex.Comments,
                     CreatedBy = ex.CreatedBy,
                     CyclicCount = ex.CyclicCount,
                 });

            if (searchCriteria.ApplyIdFilter)
            {
                issueReturns = issueReturns.Where(x => x.Id == searchCriteria.FilterId);
            }
            else
            {
                if (searchCriteria.ApplyDateFilter)
                {
                    issueReturns = issueReturns.Where(x => x.TransactionDate >= searchCriteria.DateFrom && x.TransactionDate <= searchCriteria.DateTo);
                }

                if (searchCriteria.ApplyEmployeeStatusFilter)
                {
                    issueReturns = issueReturns.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
                }

                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    issueReturns = issueReturns.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
                }

                if (searchCriteria.ApplyEmployeeNameFilter)
                {
                    issueReturns = issueReturns.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
                }

                if (searchCriteria.ApplyEntityNameFilter)
                {
                    issueReturns = issueReturns.Where(x => x.EntityName.Contains(searchCriteria.EntityName));
                }

                if (searchCriteria.ApplyAgreementNumberFilter)
                {
                    issueReturns = issueReturns.Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
                }

                // May 29 2020 - for DWS Amount approval
                if (searchCriteria.ApplyEntityIdFilter)
                {
                    issueReturns = issueReturns.Where(x => x.EntityId == searchCriteria.EntityId);
                }

                if (searchCriteria.ApplyAgreementIdFilter)
                {
                    issueReturns = issueReturns.Where(x => x.EntityAgreementId == searchCriteria.AgreementId);
                }

                if (searchCriteria.ApplySlipNumberFilter)
                {
                    issueReturns = issueReturns.Where(x => x.SlipNumber.Contains(searchCriteria.SlipNumber));
                }

                if (searchCriteria.ApplyRowStatusFilter)
                {
                    issueReturns = issueReturns.Where(x => x.Status == searchCriteria.RowStatus);
                }
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                issueReturns = issueReturns.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                issueReturns = issueReturns.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            return issueReturns.ToList();  // amount, date and status filters so far are applied;
        }

        public static ICollection<string> GetFilteringHQCodes(BaseSearchCriteria searchCriteria)
        {
            if (searchCriteria == null || (!searchCriteria.ApplyZoneFilter &&
                                            !searchCriteria.ApplyAreaFilter &&
                                            !searchCriteria.ApplyTerritoryFilter &&
                                            !searchCriteria.ApplyHQFilter))
            {
                return null;
            }

            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (s) =>
            //{
            //    DBLayer.LogError("GetOrders", s, "");
            //};

            IQueryable<DBModel.OfficeHierarchy> officeHierarchy = orm.OfficeHierarchies
                        .Where(x => x.IsActive && x.TenantId == searchCriteria.TenantId);

            if (searchCriteria.ApplyZoneFilter)
            {
                officeHierarchy = officeHierarchy.Where(x => x.ZoneCode == searchCriteria.Zone);
            }

            if (searchCriteria.ApplyAreaFilter)
            {
                officeHierarchy = officeHierarchy.Where(x => x.AreaCode == searchCriteria.Area);
            }

            if (searchCriteria.ApplyTerritoryFilter)
            {
                officeHierarchy = officeHierarchy.Where(x => x.TerritoryCode == searchCriteria.Territory);
            }

            if (searchCriteria.ApplyHQFilter)
            {
                officeHierarchy = officeHierarchy.Where(x => x.HQCode == searchCriteria.HQ);
            }

            return officeHierarchy.Select(x => x.HQCode).ToList();
        }

        public static ICollection<DomainEntities.Order> GetOrders(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (x) => DBLayer.LogError("GetOrders", x, "");
            IQueryable<DomainEntities.Order> orders =
                from o in orm.Orders
                join c in orm.Customers on o.CustomerCode equals c.CustomerCode into temp
                from t in temp.DefaultIfEmpty()
                join sp in orm.SalesPersons on o.TenantEmployee.EmployeeCode equals sp.StaffCode into temp1
                from t1 in temp1.DefaultIfEmpty()
                select new DomainEntities.Order()
                {
                    Id = o.Id,
                    EmployeeId = o.EmployeeId,
                    DayId = o.DayId,
                    CustomerCode = o.CustomerCode,
                    OrderDate = o.OrderDate,
                    OrderType = o.OrderType,
                    ItemCount = o.ItemCount,
                    TotalAmount = o.TotalAmount,
                    TotalGST = o.TotalGST,
                    NetAmount = o.NetAmount,

                    RevisedTotalAmount = o.RevisedTotalAmount,
                    RevisedTotalGST = o.RevisedTotalGST,
                    RevisedNetAmount = o.RevisedNetAmount,

                    DateUpdated = o.DateUpdated,
                    EmployeeName = o.TenantEmployee.Name,
                    CustomerName = (t == null) ? string.Empty : t.Name,
                    HQCode = (t == null) ? string.Empty : t.HQCode,
                    IsApproved = o.IsApproved,
                    ApproveRef = o.ApproveRef,
                    ApproveComments = o.ApproveNotes,
                    ApprovedDate = o.ApproveDate,
                    ApprovedBy = o.ApprovedBy,
                    ApprovedAmt = o.ApproveAmount,
                    EmployeeCode = o.TenantEmployee.EmployeeCode,
                    Phone = (t1 == null) ? string.Empty : t1.Phone,
                    IsActive = o.TenantEmployee.IsActive,
                    IsActiveInSap = (t1 != null) ? t1.IsActive : false
                };

            if (searchCriteria.ApplyOrderIdFilter)
            {
                orders = orders.Where(x => x.Id == searchCriteria.Id);
            }
            else
            {
                if (searchCriteria.ApplyAmountFilter)
                {
                    orders = orders.Where(x => x.TotalAmount >= searchCriteria.AmountFrom && x.TotalAmount <= searchCriteria.AmountTo);
                }

                if (searchCriteria.ApplyDateFilter)
                {
                    orders = orders.Where(x => x.OrderDate >= searchCriteria.DateFrom && x.OrderDate <= searchCriteria.DateTo);
                }

                if (searchCriteria.ApplyDataStatusFilter)
                {
                    orders = orders.Where(x => x.IsApproved == searchCriteria.DataStatus);
                }

                if (searchCriteria.ApplyEmployeeStatusFilter)
                {
                    orders = orders.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
                }

                var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
                if (hqList != null)
                {
                    orders = orders.Where(x => hqList.Any(y => y == x.HQCode));
                }

                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    orders = orders.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
                }

                if (searchCriteria.ApplyEmployeeNameFilter)
                {
                    orders = orders.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
                }
            }

            // apply final security
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                orders = orders.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            return orders.OrderByDescending(x => x.OrderDate).ThenBy(x => x.CustomerName).ToList();
        }

        public static IEnumerable<DomainEntities.OrderItem> GetOrderItems(long orderId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from o in orm.OrderItems
                    join p in orm.Products on o.ProductCode equals p.ProductCode into temp
                    from t in temp.DefaultIfEmpty()
                    where o.OrderId == orderId
                    select new DomainEntities.OrderItem()
                    {
                        Id = o.Id,
                        OrderId = o.OrderId,
                        SequenceNumber = o.SerialNumber,
                        ProductCode = o.ProductCode,
                        ProductName = (t == null) ? string.Empty : t.Name,
                        UOM = (t == null) ? string.Empty : t.UOM,
                        Amount = o.Amount,
                        UnitPrice = o.UnitPrice,
                        Quantity = o.UnitQuantity,

                        DiscountPercent = o.DiscountPercent,
                        DiscountedPrice = o.DiscountedPrice,
                        ItemPrice = o.ItemPrice,  // discounted Price * qty
                        GstPercent = o.GstPercent,
                        GstAmount = o.GstAmount,
                        NetPrice = o.NetPrice,

                        RevisedQuantity = o.RevisedUnitQuantity,
                        RevisedAmount = o.RevisedAmount,

                        RevisedDiscountPercent = o.RevisedDiscountPercent,
                        RevisedDiscountedPrice = o.RevisedDiscountedPrice,
                        RevisedItemPrice = o.RevisedItemPrice,  // discounted Price * qty
                        RevisedGstPercent = o.RevisedGstPercent,
                        RevisedGstAmount = o.RevisedGstAmount,
                        RevisedNetPrice = o.RevisedNetPrice,
                    }).ToList();
        }

        /// <summary>
        /// Get order items for multiple orders - used in downloading detail order report
        /// from search result page;
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public static IEnumerable<DomainEntities.OrderItem> GetOrderItems(IEnumerable<long> orderIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from o in orm.OrderItems
                    join ids in orderIds on o.OrderId equals ids
                    join p in orm.Products on o.ProductCode equals p.ProductCode into temp
                    from t in temp.DefaultIfEmpty()
                    select new DomainEntities.OrderItem()
                    {
                        Id = o.Id,
                        OrderId = o.OrderId,
                        SequenceNumber = o.SerialNumber,
                        ProductCode = o.ProductCode,
                        ProductName = (t == null) ? string.Empty : t.Name,
                        UOM = (t == null) ? string.Empty : t.UOM,
                        Amount = o.Amount,
                        UnitPrice = o.UnitPrice,
                        Quantity = o.UnitQuantity,
                        RevisedQuantity = o.RevisedUnitQuantity,
                        RevisedAmount = o.RevisedAmount
                    }).ToList();
        }

        public static void ClearData(long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.ClearEmployeeData(employeeId);
        }

        public static long CreateSqliteDataBatch(SqliteDomainData domainData)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // Create a batch entry
                SqliteActionBatch sqliteActionBatch = new SqliteActionBatch()
                {
                    BatchGuid = domainData.BatchGuid,
                    EmployeeId = domainData.EmployeeId,
                    NumberOfImages = domainData.ImageCount,
                    NumberOfImagesSaved = domainData.ImageSaveCount,
                    BatchInputCount = domainData.DomainActions?.Count() ?? 0,
                    BatchSavedCount = 0,
                    DuplicateItemCount = 0,

                    ExpenseLineInputCount = domainData.DomainExpense?.ExpenseItemCount ?? 0,
                    TotalExpenseAmount = domainData.DomainExpense?.TotalAmount ?? 0,
                    ExpenseDate = domainData.DomainExpense?.TimeStamp ?? DateTime.MinValue,

                    TotalOrderAmount = domainData.DomainOrders?.Sum(x => x.NetAmount) ?? 0, // inclusive of GST
                    NumberOfOrders = domainData.DomainOrders?.Count() ?? 0,
                    NumberOfOrderLines = domainData.DomainOrders?.Sum(x => x.ItemCount) ?? 0,

                    TotalPaymentAmount = domainData.DomainPayments?.Sum(x => x.TotalAmount) ?? 0,
                    NumberOfPayments = domainData.DomainPayments?.Count() ?? 0,

                    TotalReturnAmount = domainData.DomainReturnOrders?.Sum(x => x.TotalAmount) ?? 0,
                    NumberOfReturns = domainData.DomainReturnOrders?.Count() ?? 0,
                    NumberOfReturnLines = domainData.DomainReturnOrders?.Sum(x => x.ItemCount) ?? 0,

                    NumberOfLeaves = domainData.DomainLeaves?.Count() ?? 0,
                    NumberOfCancelledLeaves = domainData.DomainCancelledLeaves?.Count() ?? 0,

                    NumberOfEntities = domainData.DomainEntities?.Count() ?? 0,
                    Agreements = domainData.DomainAgreements?.Count() ?? 0,
                    Surveys = domainData.DomainSurveys?.Count() ?? 0,
                    BankDetails = domainData.DomainBankDetails?.Count() ?? 0,
                    DeviceLogCount = domainData.DeviceLogs?.Count() ?? 0,

                    NumberOfIssueReturns = domainData.DomainIssueReturns?.Count() ?? 0,
                    AdvanceRequests = domainData.DomainAdvanceRequests?.Count() ?? 0,

                    NumberOfWorkFlow = domainData.DomainWorkFlowPageData?.Count() ?? 0,
                    TerminateRequests = domainData.DomainTerminateAgreementData?.Count() ?? 0,

                    STRCount = domainData.DomainSTRData?.Count() ?? 0,

                    DayPlanTarget = domainData.DomainDayPlanTarget?.Count() ?? 0,

                    //added by VASANTH on 02/07/2021 - Update QuestionnaireTarget IN ACTION BATCH TABLE
                    QuestionnaireTarget = domainData.DomainQuestionnaire?.Count() ?? 0,

                    Task = domainData.DomainTask?.Count() ?? 0,
                    TaskAction = domainData.DomainTaskAction?.Count() ?? 0,

                    ExpenseLineSavedCount = 0,
                    ExpenseLineRejectCount = 0,

                    NumberOfReturnsSaved = 0,
                    NumberOfReturnLinesSaved = 0,
                    ExpenseId = 0,
                    At = DateTime.UtcNow,
                    Timestamp = DateTime.UtcNow,
                    UnderConstruction = true,
                    DataFileName = domainData.DataFileName,
                    DataFileSize = domainData.DataFileSize
                };
                orm.SqliteActionBatches.Add(sqliteActionBatch);
                orm.SaveChanges();

                return sqliteActionBatch.Id; // get the id of the batch just created;
            }
        }

        public static void SaveSqliteActionDataWrapper(long employeeId, long batchId, IEnumerable<SqliteDomainAction> domainActionCollection)
        {
            try
            {
                SaveSqliteActionData(employeeId, batchId, domainActionCollection);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"ActionData not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError("SaveSqliteActionData", errorText, logSnip);
            }
        }

        private static void SaveSqliteActionData(long employeeId, long batchId, IEnumerable<SqliteDomainAction> domainActionCollection)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // retrieve batch entry
                SqliteActionBatch sqliteActionBatch = orm.SqliteActionBatches
                                    .Where(x => x.Id == batchId).FirstOrDefault();

                long rowsSaved = 0;
                long duplicateRows = 0;

                foreach (SqliteDomainAction domainAction in domainActionCollection)
                {
                    SqliteAction sa = orm.SqliteActions
                        .Where(x => x.EmployeeId == employeeId
                                && x.ActivityTrackingType == domainAction.ActivityTrackingType
                                && x.At == domainAction.TimeStamp)
                        .FirstOrDefault();
                    if (sa == null)
                    {
                        sa = new SqliteAction()
                        {
                            SqliteTableId = domainAction.Id,
                            PhoneDbId = domainAction.PhoneDbId,
                            BatchId = batchId,
                            EmployeeId = employeeId,
                            At = domainAction.TimeStamp,
                            Name = String.Empty,
                            ActivityTrackingType = domainAction.ActivityTrackingType,
                            Latitude = domainAction.Latitude,
                            Longitude = domainAction.Longitude,
                            MNC = domainAction.MNC,
                            MCC = domainAction.MCC,
                            LAC = domainAction.LAC,
                            CellId = domainAction.CellId,
                            ClientName = Utils.TruncateString(domainAction.ClientName, 50),
                            ClientPhone = Utils.TruncateString(domainAction.ClientPhone, 20),
                            ClientType = domainAction.ClientType ?? "",
                            ActivityType = domainAction.ActivityType ?? "",
                            Comments = Utils.TruncateString(domainAction.Comments, 2000),
                            IsProcessed = false,
                            IsPostedSuccessfully = false,
                            TrackingId = 0,
                            ActivityId = 0,
                            DateCreated = DateTime.UtcNow,
                            DateUpdated = DateTime.UtcNow,
                            ImageCount = domainAction.Images != null ? domainAction.Images.Count() : 0,
                            PhoneModel = domainAction.PhoneModel,
                            PhoneOS = domainAction.PhoneOS,
                            AppVersion = domainAction.AppVersion,
                            ClientCode = domainAction.ClientCode ?? "",
                            IMEI = domainAction.IMEI ?? "",
                            ContactCount = domainAction.Contacts?.Count() ?? 0,
                            AtBusiness = domainAction.AtBusiness,
                            InstrumentId = domainAction.InstrumentId,
                            ActivityAmount = domainAction.ActivityAmount,
                            LocationException = domainAction.LocationException,
                            LocationTaskStatus = domainAction.LocationTaskStatus,
                            LocationCount = domainAction.Locations?.Count() ?? 0
                        };

                        // add images
                        if (sa.ImageCount > 0)
                        {
                            int seqNo = 0;
                            foreach (var imgFile in domainAction.Images)
                            {
                                sa.SqliteActionImages.Add(new SqliteActionImage()
                                {
                                    SequenceNumber = ++seqNo,
                                    ImageFileName = imgFile
                                });
                            }
                        }

                        if (sa.ContactCount > 0)
                        {
                            foreach (var item in domainAction.Contacts)
                            {
                                sa.SqliteActionContacts.Add(new SqliteActionContact()
                                {
                                    Name = item.Name,
                                    IsPrimary = item.IsPrimary,
                                    PhoneNumber = item.PhoneNumber
                                });
                            }
                        }

                        // store locations
                        if (sa.LocationCount > 0)
                        {
                            foreach (var item in domainAction.Locations)
                            {
                                sa.SqliteActionLocations.Add(new SqliteActionLocation()
                                {
                                    Latitude = item.Latitude,
                                    Longitude = item.Longitude,
                                    UtcAt = item.UtcAt,
                                    LocationTaskStatus = item.LocationTaskStatus,
                                    LocationException = item.LocationException,
                                    Source = item.Source,
                                    IsGood = item.IsGood
                                });
                            }
                        }

                        orm.SqliteActions.Add(sa);
                        rowsSaved += 1;
                    }
                    else
                    {
                        SqliteActionDup saDup = new SqliteActionDup()
                        {
                            SqliteTableId = domainAction.Id,
                            PhoneDbId = domainAction.PhoneDbId,
                            BatchId = batchId,
                            EmployeeId = employeeId,
                            At = domainAction.TimeStamp,
                            Name = String.Empty,
                            ActivityTrackingType = domainAction.ActivityTrackingType,
                            Latitude = domainAction.Latitude,
                            Longitude = domainAction.Longitude,
                            MNC = domainAction.MNC,
                            MCC = domainAction.MCC,
                            LAC = domainAction.LAC,
                            CellId = domainAction.CellId,
                            ClientName = Utils.TruncateString(domainAction.ClientName, 50),
                            ClientPhone = Utils.TruncateString(domainAction.ClientPhone, 20),
                            ClientType = domainAction.ClientType ?? "",
                            ActivityType = domainAction.ActivityType ?? "",
                            Comments = Utils.TruncateString(domainAction.Comments, 2000),
                            DateCreated = DateTime.UtcNow,
                            ImageCount = domainAction.Images != null ? domainAction.Images.Count() : 0,
                            ContactCount = domainAction.Contacts?.Count() ?? 0,
                            ClientCode = domainAction.ClientCode,
                            AtBusiness = domainAction.AtBusiness,
                            ActivityAmount = domainAction.ActivityAmount
                            // not saving actual images for duplicate items.
                        };
                        orm.SqliteActionDups.Add(saDup);
                        duplicateRows += 1;
                    }
                }

                // now update the rows added to the batch record
                //orm.SqliteActionBatches.Attach(sqliteActionBatch);
                sqliteActionBatch.BatchSavedCount = rowsSaved;
                sqliteActionBatch.DuplicateItemCount = duplicateRows;
                sqliteActionBatch.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        private static string GetErrorTextFromValidationException(DbEntityValidationException dbEx)
        {
            if (dbEx == null || dbEx.EntityValidationErrors == null || dbEx.EntityValidationErrors.Count() == 0)
            {
                return $"*** No errors found in {nameof(DbEntityValidationException)} ***";
            }

            ICollection<string> errorList = dbEx.EntityValidationErrors.SelectMany(x => x.ValidationErrors)
                                                    .Select(x => $"{x.PropertyName}:{x.ErrorMessage}")
                                                    .ToList();
            return String.Join("; ", errorList);
        }

        //public static long SaveExpense(DomainEntities.Expense expense)
        //{
        //    return 0;
        //EpicCrmEntities orm = DBLayer.GetOrm;
        //DBModel.Expense dbExpenseRecord = new DBModel.Expense()
        //{
        //    EmployeeDayId = expense.EmployeeDayId,
        //    TotalAmount = expense.TotalAmount,
        //    PlaceFrom = expense.PlaceFrom,
        //    PlaceTo = expense.PlaceTo,
        //    TravelMode = expense.TravelMode,
        //    OdometerStart = expense.OdometerStart,
        //    OdometerEnd = expense.OdometerEnd,
        //    ExpenseActivities = expense.Activities.Select(x => new DBModel.ExpenseActivity()
        //    {
        //        ActivityId = x
        //    }).ToList(),
        //    ExpenseDetails = expense.LineItems.Select(x => new DBModel.ExpenseDetail()
        //    {
        //        ExpenseTypeId = x.ExpenseTypeId,
        //        ExpenseAmount = x.ExpenseAmount
        //    }).ToList()
        //};

        //orm.Expenses.Add(dbExpenseRecord);
        //return dbExpenseRecord.Id;
        //}

        public static int ProcessEndDayRequest(EndDay ed)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var result = new System.Data.Entity.Core.Objects.ObjectParameter("status", 0);
            orm.EndEmployeeDay(ed.EmployeeDayId, ed.At, result);
            return (int)result.Value;
        }

        public static long RetrieveEmployeeDayId(long employeeId, DateTime date)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var rec = (from dy in orm.Days
                       join ed in orm.EmployeeDays on dy.Id equals ed.DayId
                       where dy.DATE == date && ed.TenantEmployeeId == employeeId
                       select ed).FirstOrDefault();

            return (rec == null) ? -1 : rec.Id;
        }

        public static long ProcessTrackingRequest(DomainEntities.Tracking tracking)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var result = new System.Data.Entity.Core.Objects.ObjectParameter("trackingId", 0);
            orm.AddTrackingData(tracking.EmployeeDayId, tracking.At, tracking.Latitude, tracking.Longitude, tracking.ActivityId, tracking.IsMileStone, tracking.IsStartOfDay, tracking.IsEndOfDay, result);
            return (long)result.Value;
        }

        public static long ProcessGeoLocationRequest(DomainEntities.GeoLocation geoLocation)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var result = new System.Data.Entity.Core.Objects.ObjectParameter("geoLocationId", 0);
            orm.AddGeoLocationData(geoLocation.EmployeeId, geoLocation.ClientCode, geoLocation.At, geoLocation.Latitude, geoLocation.Longitude, result);
            return (long)result.Value;
        }

        public static long ProcessActivityRequest(DomainEntities.Activity activity)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var result = new System.Data.Entity.Core.Objects.ObjectParameter("activityId", 0);
            orm.AddActivityData(activity.EmployeeDayId, activity.At, activity.ClientName, activity.ClientPhone, activity.ClientType, activity.ActivityType, activity.Comments, activity.ClientCode, activity.ActivityAmount, activity.AtBusiness, activity.ImageCount, activity.ContactCount, activity.ActivityTrackingType, result);
            long activityId = (long)result.Value;

            // add images
            if (activityId > 0)
            {
                DBModel.Activity activityRecord = orm.Activities.Find(activityId);
                if (activityRecord != null)
                {
                    if (activity.ImageCount > 0)
                    {
                        int seqNo = 0;
                        foreach (string imgFileName in activity.Images)
                        {
                            ActivityImage ai = new ActivityImage() { SequenceNumber = ++seqNo };
                            ai.Image = new Image() { ImageFileName = imgFileName, SourceId = 0 };
                            activityRecord.ActivityImages.Add(ai);
                        }
                    }
                    if (activity.ContactCount > 0)
                    {
                        foreach (DomainEntities.ActivityContact contact in activity.Contacts)
                        {
                            activityRecord.ActivityContacts.Add(new DBModel.ActivityContact()
                            {
                                Name = contact.Name,
                                PhoneNumber = contact.PhoneNumber,
                                IsPrimary = contact.IsPrimary
                            });
                        }
                    }
                    orm.SaveChanges();
                }
            }

            return activityId;
        }

        public static string TrackingLogData(int trackingLogId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rec = orm.DistanceCalcErrorLogs.AsNoTracking().FirstOrDefault(x => x.Id == trackingLogId);
            return (rec == null) ? $"No Log data found for Log Id {trackingLogId}" : rec.ErrorText;
        }

        public static ICollection<DashboardDataSet> DashboardData(DateTime reportStartDate, DateTime reportEndDate,
                                            bool includeTrackingData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var dashboardDataSet = orm.sp_DashboardData(reportStartDate, reportEndDate).Select(x =>
                new DashboardDataSet()
                {
                    Date = x.Date,
                    TenantEmployeeId = x.TenantEmployeeId,
                    EmployeeDayId = x.EmployeeDayId,
                    //TotalDistanceInMeters = x.TotalDistanceInMetersAtMilestones + x.TotalDistanceInMetersAfterLastMilestone,
                    TotalDistanceInMeters = x.TotalDistanceInMetersAtMilestones,
                    ActivityCount = x.ActivityCount
                }).ToList();

            if (includeTrackingData == false)
            {
                return dashboardDataSet;
            }

            var employeeDayIds = dashboardDataSet.Select(x => x.EmployeeDayId).ToList();

            // retrieve relevant tracking entries from database in one shot
            var trackingEntries = orm.Trackings
                            .Where(x => employeeDayIds.Any(y => y == x.EmployeeDayId)
                                        && (x.IsStartOfDay || x.IsEndOfDay || x.IsMilestone))
                            .ToList();

            foreach (var item in dashboardDataSet)
            {
                var itemTrackingEntries = trackingEntries.Where(x => x.EmployeeDayId == item.EmployeeDayId)
                                    .OrderBy(x => x.Id).ToList();
                if (itemTrackingEntries != null && itemTrackingEntries.Count > 0)
                {
                    // for start day tracking record, start location is in endLocationName
                    var firstTrackingRec = itemTrackingEntries.First();
                    var lastTrackingRec = itemTrackingEntries.Last();

                    item.StartPosition = firstTrackingRec.EndLocationName;
                    item.EndPosition = lastTrackingRec.EndLocationName;

                    item.StartTime = firstTrackingRec.At;
                    item.EndTime = lastTrackingRec.At;

                    item.TrackingDistanceInMeters = itemTrackingEntries.Where(x => x.IsMilestone)
                                                            .Sum(x => x.GoogleMapsDistanceInMeters);
                }
            }

            return dashboardDataSet;
        }

        public static ICollection<DayRecord> DaysTable(DateTime startDate, DateTime endDate)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Days.AsNoTracking().Where(x => x.DATE >= startDate && x.DATE <= endDate)
                .OrderByDescending(x => x.DATE)
                .Select(x => new DayRecord()
                {
                    Id = x.Id,
                    DATE = x.DATE
                })
                .ToList();
        }

        public static ICollection<EmployeeRecord> TenantEmployees(CRMUsersFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (x) => DBLayer.LogError("TenantEmployees", x, "");
            IQueryable<EmployeeRecord> employees = (from te in orm.TenantEmployees.AsNoTracking().Include("IMEIs")
                                                    join aspUser in orm.AspNetUsers on te.EmployeeCode equals aspUser.UserName into outer
                                                    from t in outer.DefaultIfEmpty()
                                                    join sp in orm.SalesPersons on te.EmployeeCode equals sp.StaffCode into outer2
                                                    from t2 in outer2.DefaultIfEmpty()
                                                    orderby te.Name
                                                    select new EmployeeRecord()
                                                    {
                                                        EmployeeId = te.Id,
                                                        ManagerId = te.ManagerId,
                                                        Name = te.Name,
                                                        TenantId = te.TenantId,
                                                        EmployeeCode = te.EmployeeCode,
                                                        IsActive = te.IsActive,
                                                        IMEI = te.IMEIs.Where(y => y.IsActive).Select(z => z.IMEINumber).FirstOrDefault(),
                                                        OnWebPortal = (t != null),
                                                        ExpenseHQCode = (t2 != null) ? t2.HQCode : "",
                                                        // HQCode in SalesPerson table is NOT NULL type
                                                        IsActiveInSap = (t2 != null) ? t2.IsActive : false,
                                                        Phone = (t2 != null) ? t2.Phone : "",
                                                        SendLogFromPhone = te.SendLogFromPhone,
                                                        ExecAppAccess = te.ExecAppAccess,
                                                        AutoUploadFromPhone = te.AutoUploadFromPhone,
                                                        ActivityPageName = te.ActivityPageName,
                                                        LocationFromType = te.LocationFromType
                                                    });

            if (searchCriteria != null)
            {
                if (searchCriteria.ApplyNameFilter)
                {
                    employees = employees.Where(e => e.Name.Contains(searchCriteria.Name));
                }
                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    employees = employees.Where(e => e.EmployeeCode.Contains(searchCriteria.EmployeeCode));
                }
                if (searchCriteria.ApplyIMEIFilter)
                {
                    employees = employees.Where(e => e.IMEI != null && e.IMEI.Length > 0);
                    if (employees != null)
                    {
                        employees = employees.Where(e => e.IMEI.Contains(searchCriteria.IMEI));
                    }
                }
                if (searchCriteria.IsActiveInSap == true)
                {
                    employees = employees.Where(e => e.IsActiveInSap == true);
                }
                if (searchCriteria.OnWebPortal == true)
                {
                    employees = employees.Where(e => e.OnWebPortal == true);
                }
                if (searchCriteria.IsEmployeeActive == true)
                {
                    employees = employees.Where(e => e.IsActive == true);
                }
            }

            return employees.ToList();
        }

        public static EmployeeRecord GetSingleTenantEmployee(string employeeCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TenantEmployees.AsNoTracking()
                .Where(x => x.EmployeeCode == employeeCode)
                .Include("IMEIs")
                .Select(x => new EmployeeRecord()
                {
                    EmployeeId = x.Id,
                    ManagerId = x.ManagerId,
                    Name = x.Name,
                    TenantId = x.TenantId,
                    EmployeeCode = x.EmployeeCode,
                    IsActive = x.IsActive,
                    IMEI = x.IMEIs.Where(y => y.IsActive).Select(z => z.IMEINumber).FirstOrDefault(),
                    ActivityPageName = x.ActivityPageName,
                    AutoUploadFromPhone = x.AutoUploadFromPhone,
                    LocationFromType = x.LocationFromType,
                    EnhancedDebugEnabled = x.EnhancedDebugEnabled,
                    TestFeatureEnabled = x.TestFeatureEnabled,
                    VoiceFeatureEnabled = x.VoiceFeatureEnabled,
                    ExecAppDetailLevel = x.ExecAppDetailLevel
                })
                .FirstOrDefault();
        }

        public static IEnumerable<ConfigCodeTable> GetCodeTableData(ConfigCodeTable searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var result = orm.CodeTables
                .Where(x => x.TenantId == searchCriteria.TenantId)
                .Select(x => new ConfigCodeTable()
                {
                    Id = x.Id,
                    CodeType = x.CodeType,
                    CodeValue = x.CodeValue,
                    DisplaySequence = x.DisplaySequence,
                    IsActive = x.IsActive,
                    CodeName = x.CodeName,
                    TenantId = x.TenantId,
                });

            if (searchCriteria.ApplyCodeTypeFilter)
            {
                result = result.Where(x => x.CodeType == searchCriteria.CodeType);
            }

            if (searchCriteria.ApplyCodeNameFilter)
            {
                result = result.Where(x => x.CodeName.Contains(searchCriteria.CodeName));
            }

            if (searchCriteria.ApplyCodeStatusFilter)
            {
                result = result.Where(x => x.IsActive == searchCriteria.CodeStatus);
            }

            return result.OrderBy(x => x.CodeType).ThenBy(x => x.DisplaySequence).ToList();
        }

        public static ConfigCodeTable GetSingleCodeTableData(long codeTableId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.CodeTables.Where(x => x.Id == codeTableId)
                .Select(x => new ConfigCodeTable()
                {
                    Id = x.Id,
                    CodeType = x.CodeType,
                    CodeValue = x.CodeValue,
                    DisplaySequence = x.DisplaySequence,
                    IsActive = x.IsActive,
                    CodeName = x.CodeName,
                    TenantId = x.TenantId,
                }).FirstOrDefault();

        }

        public static void SaveCodeTableData(DomainEntities.ConfigCodeTable codeTableRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.CodeTables.Find(codeTableRec.Id);

            if (sp == null)
            {
                string errorMessage = $"Code Table Id :{codeTableRec.Id} doesn't exist.";
                throw new ArgumentException(errorMessage);
            }

            sp.CodeType = codeTableRec.CodeType;
            sp.CodeName = codeTableRec.CodeName;
            sp.CodeValue = codeTableRec.CodeValue;
            sp.DisplaySequence = codeTableRec.DisplaySequence;
            sp.IsActive = codeTableRec.IsActive;

            orm.SaveChanges();
        }

        public static void CreateCodeTableData(DomainEntities.ConfigCodeTable codeTableRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.CodeTable ct = new DBModel.CodeTable()
            {
                CodeType = codeTableRec.CodeType,
                CodeName = codeTableRec.CodeName,
                CodeValue = codeTableRec.CodeValue,
                DisplaySequence = codeTableRec.DisplaySequence,
                IsActive = codeTableRec.IsActive,
                TenantId = codeTableRec.TenantId,
            };

            orm.CodeTables.Add(ct);
            orm.SaveChanges();
        }

        public static List<string> GetUniqueCodeTypes()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            List<string> resultSet = orm.CodeTables
                 .GroupBy(x => x.CodeType)
                 .Select(x => x.Key)
                 .OrderBy(x => x)
                 .ToList();

            return resultSet;
        }

        public static EmployeeRecord GetSingleTenantEmployee(long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TenantEmployees.AsNoTracking()
                .Where(x => x.Id == employeeId)
                .Include("IMEIs")
                .Select(x => new EmployeeRecord()
                {
                    EmployeeId = x.Id,
                    ManagerId = x.ManagerId,
                    Name = x.Name,
                    TenantId = x.TenantId,
                    EmployeeCode = x.EmployeeCode,
                    IsActive = x.IsActive,
                    IMEI = x.IMEIs.Where(y => y.IsActive).Select(z => z.IMEINumber).FirstOrDefault(),
                    ActivityPageName = x.ActivityPageName,
                    AutoUploadFromPhone = x.AutoUploadFromPhone,
                    LocationFromType = x.LocationFromType,
                })
                .FirstOrDefault();
        }

        public static ICollection<long> GetActiveEmployees(string imeiNumber)
        {
            // with recent change in RegisterUser sproc - there should be only one
            // active record in dbo.IMEI table for a given IMEI.
            // (in registerUser - if another user registers on same phone, we mark
            //  all other users registered on same phone previously, as 0) - Jan 5 2018
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.IMEIs.AsNoTracking()
                            .Where(x => x.IMEINumber == imeiNumber && x.IsActive)
                            .Select(x => x.TenantEmployeeId)
                            .ToList();
        }

        public static ICollection<TrackingRecord> TrackingData(long employeeDayId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Trackings.AsNoTracking().Where(x => x.EmployeeDayId == employeeDayId)
                .OrderByDescending(x => x.Id)
                .Select(x => new TrackingRecord()
                {
                    Id = x.Id,
                    ChainedTrackingId = x.ChainedTrackingId,
                    At = x.At,
                    EmployeeDayId = x.EmployeeDayId,
                    BeginLatitude = x.BeginGPSLatitude,
                    BeginLongitude = x.BeginGPSLongitude,
                    EndLatitude = x.EndGPSLatitude,
                    EndLongitude = x.EndGPSLongitude,
                    BeginLocationName = x.BeginLocationName,
                    EndLocationName = x.EndLocationName,
                    DistanceCalculated = x.DistanceCalculated,
                    BingMapsDistanceInMeters = x.BingMapsDistanceInMeters,
                    GoogleMapsDistanceInMeters = x.GoogleMapsDistanceInMeters,
                    LinearDistanceInMeters = x.LinearDistanceInMeters,
                    LockTimeStamp = x.LockTimestamp,
                    BingApiErrorId = x.DistanceCalcErrorLogs.Any(y => y.APIName == "Bing") ?
                                    (x.DistanceCalcErrorLogs.Where(y => y.APIName == "Bing").FirstOrDefault().Id) : 0,
                    GoogleApiErrorId = x.DistanceCalcErrorLogs.Any(y => y.APIName == "Google") ?
                                    (x.DistanceCalcErrorLogs.Where(y => y.APIName == "Google").FirstOrDefault().Id) : 0,
                    ActivityId = x.ActivityId,
                    IsStartOfDay = x.IsStartOfDay,
                    IsEndOfDay = x.IsEndOfDay,
                    ActivityType = x.ActivityType
                })
                .ToList();
        }

        public static ICollection<ActivityRecord> ActivityData(long employeeDayId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Activities.AsNoTracking().Where(x => x.EmployeeDayId == employeeDayId)
                .OrderByDescending(x => x.At)
                .Select(x => new ActivityRecord()
                {
                    Id = x.Id,
                    At = x.At,
                    EmployeeDayId = x.EmployeeDayId,
                    ActivityType = x.ActivityType,
                    ClientName = x.ClientName,
                    ClientPhone = x.ClientPhone,
                    ClientType = x.ClientType,
                    Comments = x.Comments
                })
                .ToList();
        }

        public static UserRecord RegisterUser(RegisterUserData userData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var outStatus = new System.Data.Entity.Core.Objects.ObjectParameter("outStatus", 0);
            var outEmployeeId = new System.Data.Entity.Core.Objects.ObjectParameter("outEmployeeId", 0);
            orm.RegisterUser(userData.TenantId, userData.TimeIntervalInMillisecondsForTracking, userData.EmployeeCode, userData.IMEI, outStatus, outEmployeeId);

            UserRecord returnObject = new UserRecord()
            {
                Message = "*Unknown*",
                EmployeeId = -1,
                EmployeeName = "",
                TimeIntervalInMillisecondsForTracking = 0,
                IsActive = false
            };

            int status = (int)outStatus.Value;
            long employeeId = (long)outEmployeeId.Value;

            if (status > 0)
            {
                var empRecord = orm.TenantEmployees.Find(employeeId);
                // update TimeIntervalInMillisecondsForTracking from config coming from UrlResolve now
                // empRecord.TimeIntervalInMillisecondsForTracking = Utils.SiteConfigData.TimeIntervalInMillisecondsForTracking;

                returnObject.EmployeeId = employeeId;
                returnObject.EmployeeName = empRecord.Name;
                returnObject.IsActive = empRecord.IsActive;
                returnObject.TimeIntervalInMillisecondsForTracking = empRecord.TimeIntervalInMillisecondsForTracking;
                returnObject.Message = "";

                if (userData.AutoUploadStaus)
                {
                    empRecord.AutoUploadFromPhone = true;
                }
                orm.SaveChanges();

                //empRecord.AutoUploadFromPhone
                return returnObject;
            }

            if (status == -1)
            {
                returnObject.Message = "Not an active DB User";
            }
            else if (status == -2)
            {
                returnObject.Message = "User access blocked in CRM";
            }
            else if (status == -3)
            {
                returnObject.Message = "User registered on another phone";
            }

            return returnObject;
        }

        /// <summary>
        /// find out / create record in EmployeeDay table for employeeId and At
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="at"></param>
        /// <returns></returns>
        public static long TranslateEmployeeIdToEmployeeDayId(long employeeId, DateTime at)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            // first find record in Day table - create if not exist;
            var dayRecord = orm.Days.Where(x => x.DATE == at.Date).FirstOrDefault();
            if (dayRecord == null)
            {
                dayRecord = new Day() { DATE = at.Date };
                orm.Days.Add(dayRecord);
                orm.SaveChanges();
            }

            // now we can use dayRecord.Id - look/create record in EmployeeDay table
            var empDayRecord = orm.EmployeeDays
                .Where(x => x.TenantEmployeeId == employeeId && x.DayId == dayRecord.Id)
                .FirstOrDefault();

            if (empDayRecord == null)
            {
                empDayRecord = new EmployeeDay()
                {
                    DayId = dayRecord.Id,
                    TenantEmployeeId = employeeId,
                    StartTime = at.Date,
                    EndTime = at.Date, // in auto created employee day filling end date also
                    HQCode = "",
                    AreaCode = "",
                    PhoneModel = "",
                    PhoneOS = "",
                    AppVersion = "***"
                };
                orm.EmployeeDays.Add(empDayRecord);
                orm.SaveChanges();
            }

            return empDayRecord.Id;
        }

        public static long GetOpenEmployeeDayIdForAutoEndDay(long employeeId, DateTime at)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var rec = (from ed in orm.EmployeeDays
                       join d in orm.Days on ed.DayId equals d.Id
                       where ed.TenantEmployeeId == employeeId
                       && ed.EndTime.HasValue == false
                       orderby d.DATE descending
                       select ed).FirstOrDefault();

            return rec?.Id ?? 0;
        }

        private static long GetTrackingTimeInterval(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var tenantRecord = orm.Tenants.Where(x => x.Id == tenantId).First();
            return tenantRecord.TimeIntervalInMillisecondsForTracking;
        }

        public static IEnumerable<TrackingRecordForDistance> GetTrackingRecordsForDistanceCalculation(int recordCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IEnumerable<GetTrackingRecordsForDistanceCalculation_Result> resultSet =
                               orm.GetTrackingRecordsForDistanceCalculation(recordCount);

            return resultSet.Select(x => new TrackingRecordForDistance()
            {
                Id = x.Id,
                BeginLatitude = x.BeginGPSLatitude,
                BeginLongitude = x.BeginGPSLongitude,
                EndLatitude = x.EndGPSLatitude,
                EndLongitude = x.EndGPSLongitude,
                IsMileStone = x.IsMilestone,
                IsStartOfDay = x.IsStartOfDay,
                IsEndOfDay = x.IsEndOfDay
            }).ToList();
        }

        public static void UpdateTrackingRecordsForDistance(IEnumerable<TrackingRecordForDistance> inputSet)
        {
            if (inputSet == null || inputSet.Count() == 0)
            {
                return;
            }

            EpicCrmEntities orm = DBLayer.GetOrm;

            // first check if error logging is not disabled
            var configRec = orm.Configs.AsNoTracking().Where(x => x.ConfigName.Equals("LogDistanceCalcError", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            // if record does not exist - we treat it yes for log errors
            bool logDistanceCalcErrors = (configRec == null || configRec.ConfigBooleanValue == true) ? true : false;

            foreach (var rec in inputSet)
            {
                var trackingRecord = orm.Trackings.Where(x => x.Id == rec.Id).FirstOrDefault();
                if (trackingRecord != null)
                {
                    trackingRecord.DistanceCalculated = true;
                    trackingRecord.BingMapsDistanceInMeters = rec.BingMapsDistanceInMeters;
                    trackingRecord.LinearDistanceInMeters = rec.LinearDistanceInMeters;
                    trackingRecord.GoogleMapsDistanceInMeters = rec.GoogleMapsDistanceInMeters;
                    trackingRecord.BeginLocationName = rec.GoogleStartAddress;
                    trackingRecord.EndLocationName = rec.GoogleEndAddress;
                    trackingRecord.LockTimestamp = null;

                    // save error details if any
                    if (logDistanceCalcErrors)
                    {
                        EpicCrmEntities ormNoLog = DBLayer.GetNoLogOrm;
                        if (rec.BingMapError && !String.IsNullOrEmpty(rec.BingMapErrorDetails))
                        {
                            ormNoLog.DistanceCalcErrorLogs.Add(new DBModel.DistanceCalcErrorLog()
                            {
                                APIName = "Bing",
                                TrackingId = rec.Id,
                                ErrorText = (rec.BingMapErrorDetails.Length > 4000) ? rec.BingMapErrorDetails.Substring(0, 4000) : rec.BingMapErrorDetails
                            });
                        }

                        if (rec.GoogleMapError && !String.IsNullOrEmpty(rec.GoogleMapErrorDetails))
                        {
                            ormNoLog.DistanceCalcErrorLogs.Add(new DBModel.DistanceCalcErrorLog()
                            {
                                APIName = "Google",
                                TrackingId = rec.Id,
                                ErrorText = (rec.GoogleMapErrorDetails.Length > 4000) ? rec.GoogleMapErrorDetails.Substring(0, 4000) : rec.GoogleMapErrorDetails
                            });
                        }

                        ormNoLog.SaveChanges();
                    }
                }
            }

            orm.SaveChanges();
        }

        //public static long GetFirstTenantId()
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;
        //    Tenant tenant = orm.Tenants.AsNoTracking().FirstOrDefault(x => x.IsActive);
        //    if (tenant == null)
        //    {
        //        throw new InvalidOperationException("Tenant record does not exist");
        //    }

        //    return tenant.Id;
        //}

        public static ICollection<DomainEntities.Tenant> GetTenants()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Tenants.AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => new DomainEntities.Tenant()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    IsProcessingMobileData = x.IsProcessingMobileData,
                    IsTransformingDataFeed = x.IsTransformingDataFeed,
                    MobileDataProcessingAt = x.MobileDataProcessingAt.HasValue ? x.MobileDataProcessingAt.Value : DateTime.MinValue,
                    IsSMSEnabled = x.IsSMSEnabled,
                    IsSendingSMS = x.IsSendingSMS,
                    SMSProcessingAt = x.SMSProcessingAt.HasValue ? x.SMSProcessingAt.Value : DateTime.MinValue,
                    IsUploadingImage = x.IsUploadingImage,
                    UploadingImageAt = x.UploadingImageAt.HasValue ? x.UploadingImageAt.Value : DateTime.MinValue
                }).ToList();
        }

        public static DomainEntities.Return GetReturn(long returnOrderId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from r in orm.ReturnOrders
                    join c in orm.Customers on r.CustomerCode equals c.CustomerCode into temp
                    from t in temp.DefaultIfEmpty()
                    where r.Id == returnOrderId
                    select new DomainEntities.Return()
                    {
                        Id = r.Id,
                        EmployeeId = r.EmployeeId,
                        DayId = r.DayId,
                        CustomerCode = r.CustomerCode,
                        ReturnDate = r.ReturnOrderDate,
                        ItemCount = r.ItemCount,
                        TotalAmount = r.TotalAmount,
                        EmployeeCode = r.TenantEmployee.EmployeeCode,
                        EmployeeName = r.TenantEmployee.Name,
                        CustomerName = (t == null) ? string.Empty : t.Name,
                        HQCode = (t == null) ? "" : t.HQCode,
                        ReferenceNumber = r.ReferenceNumber,
                        Comments = r.Comment,
                        IsApproved = r.IsApproved,
                        ApproveComments = r.ApproveNotes,
                        ApprovedAmt = r.ApproveAmount,
                        ApprovedBy = r.ApprovedBy,
                        ApprovedDate = r.ApproveDate,
                        ApproveRef = r.ApproveRef
                    }).FirstOrDefault();
        }

        public static ICollection<DomainEntities.Return> GetReturns(DomainEntities.SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.Return> returns = from r in orm.ReturnOrders
                                                        join c in orm.Customers on r.CustomerCode equals c.CustomerCode into temp
                                                        from t in temp.DefaultIfEmpty()
                                                        join sp in orm.SalesPersons on r.TenantEmployee.EmployeeCode equals sp.StaffCode into temp1
                                                        from t1 in temp1.DefaultIfEmpty()
                                                        select new DomainEntities.Return()
                                                        {
                                                            Id = r.Id,
                                                            EmployeeId = r.EmployeeId,
                                                            DayId = r.DayId,
                                                            CustomerCode = r.CustomerCode,
                                                            ReturnDate = r.ReturnOrderDate,
                                                            ItemCount = r.ItemCount,
                                                            TotalAmount = r.TotalAmount,
                                                            EmployeeCode = r.TenantEmployee.EmployeeCode,
                                                            EmployeeName = r.TenantEmployee.Name,
                                                            CustomerName = (t == null) ? string.Empty : t.Name,
                                                            HQCode = (t == null) ? "" : t.HQCode,
                                                            ReferenceNumber = r.ReferenceNumber,
                                                            Comments = r.Comment,
                                                            IsApproved = r.IsApproved,
                                                            ApproveComments = r.ApproveNotes,
                                                            ApprovedAmt = r.ApproveAmount,
                                                            ApprovedBy = r.ApprovedBy,
                                                            ApprovedDate = r.ApproveDate,
                                                            ApproveRef = r.ApproveRef,
                                                            Phone = (t1 == null) ? string.Empty : t1.Phone,
                                                            IsActive = r.TenantEmployee.IsActive,
                                                            IsActiveInSap = (t1 != null) ? t1.IsActive : false
                                                        };

            if (searchCriteria.ApplyAmountFilter)
            {
                returns = returns.Where(x => x.TotalAmount >= searchCriteria.AmountFrom && x.TotalAmount <= searchCriteria.AmountTo);
            }

            if (searchCriteria.ApplyDateFilter)
            {
                returns = returns.Where(x => x.ReturnDate >= searchCriteria.DateFrom && x.ReturnDate <= searchCriteria.DateTo);
            }

            if (searchCriteria.ApplyDataStatusFilter)
            {
                returns = returns.Where(x => x.IsApproved == searchCriteria.DataStatus);
            }

            if (searchCriteria.ApplyEmployeeStatusFilter)
            {
                returns = returns.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                returns = returns.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                returns = returns.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                returns = returns.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            // apply final security
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                returns = returns.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return returns.OrderByDescending(x => x.ReturnDate).ThenBy(x => x.CustomerName).ToList();
        }

        public static IEnumerable<DomainEntities.ReturnItem> GetReturnItems(long returnsId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from r in orm.ReturnOrderItems
                    join p in orm.Products on r.ProductCode equals p.ProductCode into temp
                    from t in temp.DefaultIfEmpty()
                    where r.ReturnOrderId == returnsId
                    orderby r.SerialNumber
                    select new DomainEntities.ReturnItem()
                    {
                        Id = r.Id,
                        ReturnsId = r.ReturnOrderId,
                        SequenceNumber = r.SerialNumber,
                        ProductCode = r.ProductCode,
                        ProductName = (t == null) ? string.Empty : t.Name,
                        Quantity = r.UnitQuantity,
                        BillingPrice = r.UnitPrice,
                        Comments = r.Comment,
                        Amount = r.Amount,
                    }).ToList();
        }

        public static int ApproveOrder(ApprovalData approvalData)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var order = orm.Orders.Find(approvalData.Id);
                if (order == null)
                {
                    return 2; //Invalid OrderId
                }

                if (order.IsApproved)
                {
                    return 0;
                }
                order.IsApproved = true;
                order.ApproveRef = approvalData.ApproveRef;
                order.ApproveAmount = approvalData.ApprovedAmt;
                order.ApproveNotes = approvalData.ApproveComments;
                order.ApproveDate = approvalData.ApprovedDate;
                order.ApprovedBy = approvalData.ApprovedBy;
                order.DateUpdated = DateTime.UtcNow;
                orm.SaveChanges();

                return 1; //Success
            }
        }

        public static int ApproveReturn(ApprovalData approvalData)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var returnOrder = orm.ReturnOrders.Find(approvalData.Id);
                if (returnOrder == null)
                {
                    return 2; //Invalid Return OrderId
                }

                if (returnOrder.IsApproved)
                {
                    return 0;
                }

                returnOrder.IsApproved = true;
                returnOrder.ApproveRef = approvalData.ApproveRef;
                returnOrder.ApproveAmount = approvalData.ApprovedAmt;
                returnOrder.ApproveNotes = approvalData.ApproveComments;
                returnOrder.ApproveDate = approvalData.ApprovedDate;
                returnOrder.ApprovedBy = approvalData.ApprovedBy;
                returnOrder.DateUpdated = DateTime.UtcNow;
                orm.SaveChanges();

                return 1; //Success
            }
        }

        public static ICollection<DomainEntities.SalesPerson> GetSalesPersons()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return (from s in orm.SalesPersons
                    select new DomainEntities.SalesPerson()
                    {
                        SalesPersonName = s.Name,
                        StaffCode = s.StaffCode,
                        IsActive = s.IsActive
                    }).OrderBy(x => x.SalesPersonName).ToList();
        }

        public static DomainEntities.SalesPerson GetSingleSalesPerson(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var salesPerson = (from s in orm.SalesPersons
                               where s.StaffCode == staffCode
                               select new DomainEntities.SalesPerson()
                               {
                                   SalesPersonName = s.Name,
                                   StaffCode = s.StaffCode,
                                   Phone = s.Phone,
                                   IsActive = s.IsActive,
                                   OverridePrivateVehicleRatePerKM = s.OverridePrivateVehicleRatePerKM,
                                   TwoWheelerRatePerKM = s.TwoWheelerRatePerKM,
                                   FourWheelerRatePerKM = s.FourWheelerRatePerKM
                               }).FirstOrDefault();

            return salesPerson;
        }

        public static IEnumerable<DomainEntities.SalesPersonsAssociation> GetSalesPersonsAssociation(string level, string code)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return (from s in orm.SalesPersonAssociations
                    where s.CodeType == level && s.CodeValue == code && s.IsDeleted == false
                    select new DomainEntities.SalesPersonsAssociation()
                    {
                        StaffCode = s.StaffCode,
                        AssignedDate = s.DateCreated,
                        CodeType = s.CodeType,
                        CodeValue = s.CodeValue
                    }).ToList();
        }

        /// <summary>
        /// Return the people data who are assigned at a particular level
        /// e.g. I want all people who are Area Managers
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static IEnumerable<SalesPersonEx> GetAssignedManagers(string level)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.SalesPersonAssociations.Where(x => x.CodeType == level && x.IsDeleted == false)
                .Join(orm.SalesPersons, (ass) => ass.StaffCode, (sp) => sp.StaffCode, (ass, sp) => new SalesPersonEx()
                {
                    AssociationType = level,
                    AssociationCode = ass.CodeValue,
                    StaffCode = ass.StaffCode,
                    SalesPersonName = sp.Name,
                    Phone = sp.Phone,
                    IsActive = sp.IsActive
                }).Where(x => x.IsActive).ToList();
        }

        public static bool SaveAssignedSalesPersons(string[] staffCodes, string level, string code, string approvedBy)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var existingPersons = GetSalesPersonsAssociation(level, code);
            var existingStaffCodes = existingPersons.Select(s => s.StaffCode).ToList();

            //var newPersons = staffCodes.Where(s => !existingAssignments.Any(a => s.Contains(a))).ToList();
            var newPersons = staffCodes.Where(x => !existingStaffCodes.Any(y => y == x)).ToList();

            //var removePersons = existingAssignments.Where(o => !staffCodes.Any(a => o.Contains(a))).ToList();
            var removePersons = existingStaffCodes.Where(x => !staffCodes.Any(y => y == x)).ToList();

            foreach (var p in newPersons)
            {
                DBModel.SalesPersonAssociation assignSPA = new DBModel.SalesPersonAssociation()
                {
                    StaffCode = p,
                    CodeType = level,
                    CodeValue = code,
                    IsDeleted = false,
                    DateUpdated = DateTime.UtcNow,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = approvedBy,
                    UpdatedBy = approvedBy
                };

                orm.SalesPersonAssociations.Add(assignSPA);
            }

            foreach (var r in removePersons)
            {
                var spa = orm.SalesPersonAssociations.Where(s => s.StaffCode == r && s.CodeType == level && s.CodeValue == code && s.IsDeleted == false).FirstOrDefault();
                if (spa != null)
                {
                    spa.IsDeleted = true;
                    spa.DateUpdated = DateTime.UtcNow;
                    spa.UpdatedBy = approvedBy;
                }
            }

            return (orm.SaveChanges() > 0);

        }
        /// <summary>
        public static bool SaveSalesPersonAssignmentData(string staffCode, string level, string[] existingRegionCodes, string[] assignedRegioncodes, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var newRegionsCodes = assignedRegioncodes.Where(x => !existingRegionCodes.Any(y => y.ToLower().Equals(x.ToLower()))).ToList();

            var removeRegionsCodes = existingRegionCodes.Where(x => !assignedRegioncodes.Any(y => y.ToLower().Equals(x.ToLower()))).ToList();

            foreach (var c in newRegionsCodes)
            {
                DBModel.SalesPersonAssociation assignSPA = new DBModel.SalesPersonAssociation()
                {
                    StaffCode = staffCode,
                    CodeType = level,
                    CodeValue = c,
                    IsDeleted = false,
                    DateUpdated = DateTime.UtcNow,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = currentUser,
                    UpdatedBy = currentUser,
                };

                orm.SalesPersonAssociations.Add(assignSPA);
            }

            foreach (var r in removeRegionsCodes)
            {
                var spa = orm.SalesPersonAssociations.Where(s => s.CodeValue == r && s.CodeType == level
                           && s.StaffCode == staffCode && s.IsDeleted == false).FirstOrDefault();
                if (spa != null)
                {
                    spa.IsDeleted = true;
                    spa.DateUpdated = DateTime.UtcNow;
                    spa.UpdatedBy = currentUser;
                }
            }

            return (orm.SaveChanges() > 0);
        }
        /// </summary>

        // https://stackoverflow.com/questions/4736316/joining-tables-using-more-than-one-column-in-linq-to-entities
        public static ICollection<DomainEntities.SalesPersonsAssociationData> GetStaffAssociations(long tenantId, string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.SalesPersonAssociations
                .Where(x => x.StaffCode == staffCode && x.IsDeleted == false)
                .Join(orm.CodeTables.Where(t => t.TenantId == tenantId && t.IsActive),
                        o => new { k1 = o.CodeType, k2 = o.CodeValue },
                        i => new { k1 = i.CodeType, k2 = i.CodeValue },
                        (o, i) => new DomainEntities.SalesPersonsAssociationData()
                        {
                            AssignedDate = o.DateUpdated,
                            CodeType = o.CodeType,
                            CodeName = i.CodeName,
                            Code = o.CodeValue
                        }).ToList();
        }

        /// <summary>
        /// Get top selling products with in given order ids
        /// </summary>
        /// <param name="orderIds"></param>
        /// <param name="itemsCount"></param>
        /// <returns></returns>
        public static IEnumerable<TOPItemsData> GetTopSellingProducts(SearchCriteria sc, int topItemsCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            //orm.Database.Log = (s) => LogError(nameof(GetOrders), s, ">");
            var orders =
                from o in orm.Orders
                join c in orm.Customers on o.CustomerCode equals c.CustomerCode into temp
                from t in temp.DefaultIfEmpty()
                where o.OrderDate >= sc.DateFrom && o.OrderDate <= sc.DateTo
                select new
                {
                    Id = o.Id,
                    HQCode = (t == null) ? string.Empty : t.HQCode,
                    OrderItems = o.OrderItems
                };

            var hqList = DBLayer.GetFilteringHQCodes(sc);
            if (hqList != null)
            {
                orders = orders.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security
            if (!sc.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(sc.TenantId, sc.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                orders = orders.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return orders.Select(x => x.OrderItems)
            .SelectMany(x => x)
            .Select(x =>
                    new
                    {
                        Amount = x.Amount,
                        ProductCode = x.ProductCode
                    }
                )
            .GroupBy(s => s.ProductCode)
            .Select(o => new
            {
                Name = o.Key,
                Amount = o.Sum(t => t.Amount)
            })
            .ToList()
            .OrderByDescending(x => x.Amount)
            .Take(topItemsCount)

            // get product name now
           .Join(orm.Products, tid => tid.Name, p => p.ProductCode, (tid, p) => new TOPItemsData()
           {
               Name = p.Name,
               Code = p.ProductCode,
               Amount = tid.Amount
           })
           .OrderByDescending(x => x.Amount)
           .ToList();
        }

        public static IEnumerable<TOPItemsData> GetTopReturnProducts(SearchCriteria sc, int topItemsCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var returns = from r in orm.ReturnOrders
                          join c in orm.Customers on r.CustomerCode equals c.CustomerCode into temp
                          from t in temp.DefaultIfEmpty()
                          where r.ReturnOrderDate >= sc.DateFrom && r.ReturnOrderDate <= sc.DateTo
                          select new
                          {
                              Id = r.Id,
                              HQCode = (t == null) ? "" : t.HQCode,
                              ReturnItems = r.ReturnOrderItems
                          };

            var hqList = DBLayer.GetFilteringHQCodes(sc);
            if (hqList != null)
            {
                returns = returns.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security
            if (!sc.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(sc.TenantId, sc.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                returns = returns.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return returns.Select(x => x.ReturnItems)
                .SelectMany(x => x)
                .Select(x =>
                    new
                    {
                        Amount = x.Amount,
                        ProductCode = x.ProductCode
                    })
                    .GroupBy(s => s.ProductCode)
                    .Select(o => new
                    {
                        Name = o.Key,
                        Amount = o.Sum(t => t.Amount)
                    })
                    .OrderByDescending(x => x.Amount)
                    .Take(topItemsCount)
                    // get product name now
                    .Join(orm.Products, tid => tid.Name, p => p.ProductCode, (tid, p) => new TOPItemsData()
                    {
                        Name = p.Name,
                        Code = p.ProductCode,
                        Amount = tid.Amount
                    })
                    .OrderByDescending(x => x.Amount)
                    .ToList();
        }

        public static IEnumerable<TOPItemsData> GetTopSalesPersonsByOrders(SearchCriteria sc, int topItemsCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var orders =
                from o in orm.Orders
                join c in orm.Customers on o.CustomerCode equals c.CustomerCode into temp
                from t in temp.DefaultIfEmpty()
                where o.OrderDate >= sc.DateFrom && o.OrderDate <= sc.DateTo
                select new
                {
                    HQCode = (t == null) ? string.Empty : t.HQCode,
                    Amount = o.TotalAmount,
                    EmployeeId = o.EmployeeId
                };

            var hqList = DBLayer.GetFilteringHQCodes(sc);
            if (hqList != null)
            {
                orders = orders.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security
            if (!sc.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(sc.TenantId, sc.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                orders = orders.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return orders
                    .GroupBy(s => s.EmployeeId)
                    .Select(o => new
                    {
                        EmployeeId = o.Key,
                        Amount = o.Sum(x => x.Amount)
                    })
                    .OrderByDescending(x => x.Amount)
                    .Take(topItemsCount)
                    .Join(orm.TenantEmployees, tid => tid.EmployeeId, te => te.Id, (tid, te) => new TOPItemsData()
                    {
                        Name = te.Name,
                        Code = te.EmployeeCode,
                        Amount = tid.Amount
                    })
                    .OrderByDescending(x => x.Amount)
                    .ToList();
        }

        public static IEnumerable<TOPItemsData> GetTopSalesPersonsByPayments(SearchCriteria sc, int topItemsCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var payments = from p in orm.Payments
                           join c in orm.Customers on p.CustomerCode equals c.CustomerCode into temp
                           from t in temp.DefaultIfEmpty()
                           where p.PaymentDate >= sc.DateFrom && p.PaymentDate <= sc.DateTo
                           select new
                           {
                               EmployeeId = p.EmployeeId,
                               TotalAmount = p.TotalAmount,
                               HQCode = (t == null) ? string.Empty : t.HQCode,
                           };

            var hqList = DBLayer.GetFilteringHQCodes(sc);
            if (hqList != null)
            {
                payments = payments.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security
            if (!sc.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(sc.TenantId, sc.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                payments = payments.Where(x => rsHQCodes.Any(rsHqCode => rsHqCode == x.HQCode));
            }

            return payments
                    .GroupBy(s => s.EmployeeId)
                    .Select(o => new
                    {
                        EmployeeId = o.Key,
                        Amount = o.Sum(x => x.TotalAmount)
                    })
                    .OrderByDescending(x => x.Amount)
                    .Take(topItemsCount)
                    .Join(orm.TenantEmployees, tid => tid.EmployeeId, te => te.Id, (tid, te) => new TOPItemsData()
                    {
                        Name = te.Name,
                        Code = te.EmployeeCode,
                        Amount = tid.Amount
                    })
                    .OrderByDescending(x => x.Amount)
                    .ToList();
        }

        public static ICollection<SqliteActionProcessData> GetPreviousBatchEntries(long batchId, long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            // first find previous batch
            var prevBatchRecord = orm.SqliteActionBatches
                .Where(x => x.EmployeeId == employeeId && x.Id < batchId && x.BatchProcessed && x.BatchSavedCount > 0)
                .OrderByDescending(x => x.Id)
                .FirstOrDefault();

            if (prevBatchRecord == null)
            {
                return null;
            }

            return orm.SqliteActions
                .Where(x => x.BatchId == prevBatchRecord.Id)
                .OrderByDescending(x => x.At)
                .Select(x => new SqliteActionProcessData()
                {
                    Id = x.Id,
                    SqliteTableId = x.SqliteTableId,
                    BatchId = x.BatchId,
                    EmployeeId = x.EmployeeId,
                    At = x.At,
                    ActivityTrackingtype = x.ActivityTrackingType,
                    //Name = x.Name,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    MNC = x.MNC,
                    MCC = x.MCC,
                    LAC = x.LAC,
                    CellId = x.CellId,
                    ClientName = x.ClientName,
                    ClientPhone = x.ClientPhone,
                    ClientType = x.ClientType,
                    ActivityType = x.ActivityType,
                    Comments = x.Comments,
                    IsProcessed = x.IsProcessed,
                    IsPostedSuccessfully = x.IsPostedSuccessfully,
                    TrackingId = x.TrackingId,
                    ActivityId = x.ActivityId,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated,
                    ImageCount = x.ImageCount,
                    Images = x.SqliteActionImages.OrderBy(y => y.SequenceNumber).Select(y => y.ImageFileName).ToList(),
                    PhoneModel = x.PhoneModel,
                    PhoneOS = x.PhoneOS,
                    AppVersion = x.AppVersion,
                    ClientCode = x.ClientCode,
                    IMEI = x.IMEI,
                    ContactCount = x.ContactCount,
                    AtBusiness = x.AtBusiness,
                    InstrumentId = x.InstrumentId,
                    ActivityAmount = x.ActivityAmount,
                    Contacts = x.SqliteActionContacts.OrderBy(z => z.Id).Select(z => new SqliteActionContactData()
                    {
                        Id = z.Id,
                        SqliteActionId = z.SqliteActionId,
                        Name = z.Name,
                        PhoneNumber = z.PhoneNumber,
                        IsPrimary = z.IsPrimary
                    }).ToList()
                }).ToList();
        }

        public static ICollection<ActivityByTypeReportData> GetActivityByTypeReportData(IEnumerable<long> employeeIds, IEnumerable<long> dayIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //orm.Database.Log = (x) => DBLayer.LogError("GetActivityByTypeReportData", x, "");
            // orm.Database.Log = x => Console.WriteLine(x);

            return (from ed in orm.EmployeeDays.Where(x => employeeIds.Any(y => y == x.TenantEmployeeId) && dayIds.Any(d => d == x.DayId))
                    join act in orm.Activities on ed.Id equals act.EmployeeDayId
                    select new
                    {
                        DayId = ed.DayId,
                        TenantEmployeeId = ed.TenantEmployeeId,
                        ClientType = act.ClientType,
                        ActivityType = act.ActivityType,
                        HQCode = ed.HQCode,
                    })
                    .GroupBy(x => x)
                    .Select(x => new ActivityByTypeReportData()
                    {
                        DayId = x.Key.DayId,
                        TenantEmployeeId = x.Key.TenantEmployeeId,
                        ClientType = x.Key.ClientType,
                        ActivityType = x.Key.ActivityType,
                        ActivityCount = x.Count(),
                        HQCode = x.Key.HQCode
                    }).ToList();
        }

        public static IEnumerable<DashboardProduct> GetProductsWithoutPrice(ProductsFilter searchCriteria = null)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            // orm.Database.Log = (x) => DBLayer.LogError("GetProductData", x, "");

            var allProducts = orm.Products.AsQueryable();

            if (searchCriteria?.ApplyProductCodeFilter ?? false)
            {
                allProducts = allProducts.Where(p => p.ProductCode.Contains(searchCriteria.ProductCode));
            }

            if (searchCriteria?.ApplyNameFilter ?? false)
            {
                allProducts = allProducts.Where(p => p.Name.Contains(searchCriteria.Name));
            }

            if (searchCriteria?.ApplyStatusFilter ?? false)
            {
                allProducts = allProducts.Where(p => p.IsActive == searchCriteria.Status);
            }

            var products = from ap in allProducts
                           where !ap.ProductPrices.Any()
                           select new DashboardProduct()
                           {
                               Id = ap.Id,
                               ProductCode = ap.ProductCode,
                               Name = ap.Name,
                               GroupName = ap.ProductGroup.GroupName,
                               IsActive = ap.IsActive,

                               ShelfLifeInMonths = ap.ShelfLifeInMonths,
                               UOM = ap.UOM,
                               BrandName = ap.BrandName,
                               GstCode = ap.GstCode,
                               // Prices = null
                           };

            return products.ToList();
        }

        public static IEnumerable<DashboardBankAccount> GetBankAccounts(BankAccountFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            // orm.Database.Log = (x) => DBLayer.LogError("GetDashboardBankAccount", x, "");
            IQueryable<DashboardBankAccount> bankAccounts = from ba in orm.BankAccounts
                                                            orderby ba.BankName
                                                            select new DashboardBankAccount()
                                                            {
                                                                Id = ba.Id,
                                                                AreaCode = ba.AreaCode,
                                                                BankName = ba.BankName,
                                                                BranchName = ba.BranchName,
                                                                AccountNumber = ba.AccountNumber,
                                                                BankPhone = ba.BankPhone,
                                                                IFSC = ba.IFSC,
                                                                IsActive = ba.IsActive,
                                                                AccountName = ba.AccountName,
                                                                AccountAddress = ba.AccountAddress,
                                                                AccountEmail = ba.AccountEmail
                                                            };

            if (searchCriteria?.ApplyBankNameFilter ?? false)
            {
                bankAccounts = bankAccounts.Where(s => s.BankName.Contains(searchCriteria.BankName));
            }

            if (searchCriteria?.ApplyAreaCodeFilter ?? false)
            {
                bankAccounts = bankAccounts.Where(s => s.AreaCode.Equals(searchCriteria.AreaCode));
            }

            return bankAccounts.ToList();
        }

        public static IEnumerable<DashboardLeave> GetLeaves(long employeeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var currentDate = DateTime.Now.Date;
            var minDate = DateTime.Now.AddMonths(-6);

            IQueryable<DashboardLeave> leaveRecs =
                            from ba in orm.Leaves
                            where (ba.DateUpdated >= minDate)
                            select new DashboardLeave()
                            {
                                Id = ba.Id,
                                EmployeeId = ba.EmployeeId,
                                DaysCountExcludingHolidays = ba.DaysCountExcludingHolidays,
                                StartDate = ba.StartDate,
                                EndDate = ba.EndDate,
                                LeaveType = ba.LeaveType,
                                LeaveReason = ba.LeaveReason,
                                Comment = ba.Comment,
                                LeaveStatus = ba.LeaveStatus,
                                ApproveNotes = ba.ApproveNotes
                            };

            if (employeeId > 0)
            {
                leaveRecs = leaveRecs.Where(x => x.EmployeeId == employeeId);
            }

            return leaveRecs.ToList();
        }

        public static long CreateBankAccount(DashboardBankAccount eb)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DBModel.BankAccount bankAccount = new DBModel.BankAccount
            {
                AreaCode = eb.AreaCode,
                BankName = eb.BankName,
                BranchName = eb.BranchName,
                BankPhone = eb.BankPhone,
                AccountNumber = eb.AccountNumber,
                IFSC = eb.IFSC,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CreatedBy = eb.CurrentStaffCode,
                UpdatedBy = eb.CurrentStaffCode,
                IsActive = eb.IsActive,
                AccountName = eb.AccountName,
                AccountAddress = eb.AccountAddress,
                AccountEmail = eb.AccountEmail
            };

            orm.BankAccounts.Add(bankAccount);
            orm.SaveChanges();
            return bankAccount.Id;
        }

        public static void SaveBankData(DashboardBankAccount eb)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.BankAccount bank = orm.BankAccounts.FirstOrDefault(x => x.Id == eb.Id);

            if (bank == null)
            {
                throw new ArgumentException("Invalid Bank Name");
            }

            bank.AreaCode = eb.AreaCode;
            bank.BankName = eb.BankName;
            bank.BranchName = eb.BranchName;
            bank.BankPhone = eb.BankPhone;
            bank.AccountNumber = eb.AccountNumber;
            bank.IFSC = eb.IFSC;
            bank.DateUpdated = DateTime.UtcNow;
            bank.UpdatedBy = eb.CurrentStaffCode;
            bank.IsActive = eb.IsActive;

            bank.AccountName = eb.AccountName;
            bank.AccountAddress = eb.AccountAddress;
            bank.AccountEmail = eb.AccountEmail;

            orm.SaveChanges();
        }

        public static IEnumerable<DashboardBankAccount> GetBankAccountDetails(long bankAccountId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DashboardBankAccount> outputList = from ba in orm.BankAccounts.Where(x => x.Id == bankAccountId)
                                                          select new DashboardBankAccount()
                                                          {
                                                              Id = ba.Id,
                                                              AreaCode = ba.AreaCode,
                                                              BankName = ba.BankName,
                                                              BranchName = ba.BranchName,
                                                              AccountNumber = ba.AccountNumber,
                                                              BankPhone = ba.BankPhone,
                                                              IFSC = ba.IFSC,
                                                              IsActive = ba.IsActive,
                                                              AccountName = ba.AccountName,
                                                              AccountAddress = ba.AccountAddress,
                                                              AccountEmail = ba.AccountEmail
                                                          };

            return outputList.ToList();
        }

        public static long CreateCustomer(DownloadCustomerExtend dc, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DBModel.Customer c = orm.Customers.FirstOrDefault(x => x.CustomerCode == dc.Code);

            if (c != null)
            {
                return -1;  // customer already exist;
            }

            DBModel.Customer customer = new DBModel.Customer
            {
                CustomerCode = dc.Code,
                Name = dc.Name,
                ContactNumber = dc.PhoneNumber,
                Type = dc.Type,
                CreditLimit = dc.CreditLimit,
                Outstanding = dc.Outstanding,
                LongOutstanding = dc.LongOutstanding,
                Target = dc.Target,
                Sales = dc.Sales,
                Payment = dc.Payment,
                HQCode = dc.HQCode,
                //District = dc.District,
                //State = dc.State,
                //Branch = dc.Branch,
                //Pincode = dc.Pincode,
                Address1 = dc.Address1,
                Address2 = dc.Address2,
                Email = dc.Email,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CreatedBy = currentUser,
                UpdatedBy = currentUser,
                IsActive = dc.Active
            };

            orm.Customers.Add(customer);
            orm.SaveChanges();
            return customer.Id;
        }

        public static long SaveCustomer(DownloadCustomerExtend dc, string currentUserStaffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.Customer customer = orm.Customers.Find(dc.Id);

            if (customer == null)
            {
                return -1;
            }

            // cust code can not be changed;
            //customer.CustomerCode = dc.Code;
            customer.Name = dc.Name;
            customer.ContactNumber = dc.PhoneNumber;
            customer.Type = dc.Type;
            customer.CreditLimit = dc.CreditLimit;
            customer.Outstanding = dc.Outstanding;
            customer.LongOutstanding = dc.LongOutstanding;
            customer.Target = dc.Target;
            customer.Sales = dc.Sales;
            customer.Payment = dc.Payment;
            customer.HQCode = dc.HQCode;
            //customer.District = dc.District;
            //customer.State = dc.State;
            //customer.Branch = dc.Branch;
            //customer.Pincode = dc.Pincode;
            customer.Address1 = dc.Address1;
            customer.Address2 = dc.Address2;
            customer.Email = dc.Email;
            customer.DateUpdated = DateTime.UtcNow;
            customer.UpdatedBy = currentUserStaffCode;
            customer.IsActive = dc.Active;
            orm.SaveChanges();

            return dc.Id;
        }

        public static DownloadCustomerExtend GetCustomerDetails(string Code)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Customers.Where(x => x.CustomerCode == Code)
                                        .Select(c => new DownloadCustomerExtend()
                                        {
                                            Id = c.Id,
                                            Code = c.CustomerCode,
                                            Name = c.Name,
                                            PhoneNumber = c.ContactNumber,
                                            Type = c.Type,
                                            CreditLimit = c.CreditLimit,
                                            Outstanding = c.Outstanding,
                                            LongOutstanding = c.LongOutstanding,
                                            Target = c.Target,
                                            Sales = c.Sales,
                                            Payment = c.Payment,
                                            HQCode = c.HQCode,
                                            //District = c.District,
                                            //State = c.State,
                                            //Branch = c.Branch,
                                            //Pincode = c.Pincode,
                                            Address1 = c.Address1,
                                            Address2 = c.Address2,
                                            Email = c.Email,
                                            Active = c.IsActive
                                        }).FirstOrDefault();
        }

        public static string OrderImageData(long id, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var imageRecord = orm.SqliteOrderImages.AsNoTracking()
                    .Where(x => x.SqliteOrderId == id)
                    .OrderBy(x => x.Id)
                    .Skip(imageItem - 1)
                    .FirstOrDefault();
                return imageRecord?.ImageFileName;
            }
        }

        public static string SqliteEntityImageData(long id, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var imageRecord = orm.SqliteEntityImages.AsNoTracking()
                    .Where(x => x.SqliteEntityId == id)
                    .OrderBy(x => x.Id)
                    .Skip(imageItem - 1)
                    .FirstOrDefault();
                return imageRecord?.ImageFileName;
            }
        }

        public static string SqliteBankDetailImageData(long id, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var imageRecord = orm.SqliteBankDetailImages.AsNoTracking()
                    .Where(x => x.SqliteBankDetailId == id)
                    .OrderBy(x => x.Id)
                    .Skip(imageItem - 1)
                    .FirstOrDefault();
                return imageRecord?.ImageFileName;
            }
        }

        public static string SqliteSTRImageData(long id, int imageItem)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                var imageRecord = orm.SqliteSTRImages.AsNoTracking()
                    .Where(x => x.SqliteSTRId == id)
                    .OrderBy(x => x.Id)
                    .Skip(imageItem - 1)
                    .FirstOrDefault();
                return imageRecord?.ImageFileName;
            }
        }

        public static string GetDailyMessage(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.StaffMessages.Where(x => x.StaffCode == staffCode)
                .Select(x => x.Message)
                .FirstOrDefault();
        }

        public static ICollection<DomainEntities.StaffDailyData> GetStaffDailyData(long tenantId, string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var rs = orm.StaffDailyReportDatas.Where(x => x.TenantId == tenantId)
                                        .Select(c => new DomainEntities.StaffDailyData()
                                        {
                                            Id = c.Id,
                                            Date = c.DATE,
                                            TenantId = c.TenantId,
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

            if (String.IsNullOrEmpty(staffCode) == false)
            {
                rs = rs.Where(x => x.StaffCode == staffCode);
            }

            return rs.ToList();
        }

        public static ICollection<DomainEntities.ItemMaster> GetAllItemMaster()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.ItemMasters.Where(x => x.IsActive)
                                        .Select(c => new DomainEntities.ItemMaster()
                                        {
                                            Id = c.Id,
                                            ItemCode = c.ItemCode,
                                            ItemDesc = c.ItemDesc,
                                            Category = c.Category,
                                            Unit = c.Unit,
                                            Classification = c.Classification,
                                            TypeNames = c.ItemMasterTypeNames.Select(y => new DomainEntities.ItemMasterTypeName()
                                            {
                                                TypeName = y.TypeName,
                                                Rate = y.Rate,
                                                ReturnRate = y.ReturnRate
                                            }).ToList()
                                        }).ToList();
        }

        public static ICollection<string> GetDivisionCodes(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.StaffDivisions.Where(x => x.StaffCode == staffCode)
                                        .Select(c => c.DivisionCode).ToList();
        }

        public static ICollection<DomainEntities.StaffDivision> GetStaffDivisions()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.StaffDivisions
                    .Select(c => new DomainEntities.StaffDivision()
                    {
                        DivisionCode = c.DivisionCode,
                        StaffCode = c.StaffCode
                    }).ToList();
        }

        public static ICollection<DomainEntities.ExcelUploadStatus> GetExcelUploadStatus(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.ExcelUploadStatus.Where(x => x.TenantId == tenantId)
                .Select(x => new DomainEntities.ExcelUploadStatus()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    UploadType = x.UploadType,
                    UploadTable = x.UploadTable,
                    UploadFileName = x.UploadFileName,
                    IsCompleteRefresh = x.IsCompleteRefresh,
                    RecordCount = x.RecordCount,
                    RequestedBy = x.RequestedBy,
                    RequestTimestamp = x.RequestTimestamp,
                    PostingTimestamp = x.PostingTimestamp,
                    IsPosted = x.IsPosted,

                    LocalFileName = x.LocalFileName,
                    IsParsed = x.IsParsed,
                    ErrorCount = x.ErrorCount,
                    IsLocked = x.IsLocked,
                    LockTimestamp = x.LockTimestamp

                }).ToList();
        }

        public static long CreateExcelUploadStatus(DomainEntities.ExcelUploadStatus input)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            ExcelUploadStatu dbRec = orm.ExcelUploadStatus
                .FirstOrDefault(x => x.TenantId == input.TenantId && x.UploadTable == input.UploadTable);

            if (dbRec == null)
            {
                dbRec = new ExcelUploadStatu()
                {
                    TenantId = input.TenantId,
                    UploadType = input.UploadType,
                    UploadTable = input.UploadTable,
                    UploadFileName = input.UploadFileName,
                    IsCompleteRefresh = input.IsCompleteRefresh,
                    RecordCount = input.RecordCount,
                    RequestedBy = input.RequestedBy,
                    RequestTimestamp = DateTime.UtcNow,
                    PostingTimestamp = DateTime.MinValue,
                    IsPosted = input.IsPosted,

                    LocalFileName = input.LocalFileName,
                    IsParsed = input.IsParsed,
                    ErrorCount = input.ErrorCount,
                    IsLocked = input.IsLocked,
                    LockTimestamp = input.LockTimestamp
                };

                orm.ExcelUploadStatus.Add(dbRec);
            }
            else
            {
                // remove existing records from excelUploadError
                var existingLogs = orm.ExcelUploadErrors.Where(x => x.ExcelUploadStatusId == dbRec.Id).ToList();
                orm.ExcelUploadErrors.RemoveRange(existingLogs);

                dbRec.UploadType = input.UploadType;
                dbRec.UploadFileName = input.UploadFileName;
                dbRec.IsCompleteRefresh = input.IsCompleteRefresh;
                dbRec.RecordCount = input.RecordCount; //
                dbRec.RequestedBy = input.RequestedBy;
                dbRec.RequestTimestamp = DateTime.UtcNow;
                dbRec.PostingTimestamp = DateTime.MinValue;
                dbRec.IsPosted = false;

                dbRec.LocalFileName = input.LocalFileName;
                dbRec.IsParsed = input.IsParsed; //
                dbRec.ErrorCount = input.ErrorCount; //
                dbRec.IsLocked = input.IsLocked;
                dbRec.LockTimestamp = input.LockTimestamp;
            }

            orm.SaveChanges();

            return dbRec.Id;
        }

        public static void UpdateExcelUploadStatus(DomainEntities.ExcelUploadStatus input)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            ExcelUploadStatu dbRec = orm.ExcelUploadStatus.Find(input.Id);

            if (dbRec != null)
            {
                dbRec.RecordCount = input.RecordCount;
                dbRec.IsParsed = input.IsParsed;
                dbRec.ErrorCount = input.ErrorCount;
            }

            orm.SaveChanges();
        }

        public static void CreateExcelUploadHistory(DomainEntities.ExcelUploadStatus input)
        {
            EpicCrmEntities orm = DBLayer.GetNoLogOrm;

            DateTime updateDateTime = DateTime.UtcNow;

            // first update the time in ExcelUploadStatus
            ExcelUploadStatu dbRec = orm.ExcelUploadStatus.Find(input.Id);
            if (dbRec == null)
            {
                throw new ArgumentException($"Invalid Id {input.Id} for Excel Upload Status Row");
            }
            dbRec.IsPosted = true;
            dbRec.PostingTimestamp = updateDateTime;

            // Create History Record
            DBModel.ExcelUploadHistory historyRec = new DBModel.ExcelUploadHistory()
            {
                TenantId = input.TenantId,
                UploadType = input.UploadType,
                UploadFileName = input.UploadFileName,
                IsCompleteRefresh = input.IsCompleteRefresh,
                RecordCount = input.RecordCount,
                RequestedBy = input.RequestedBy,
                RequestTimestamp = input.RequestTimestamp,
                PostingTimestamp = updateDateTime
            };
            orm.ExcelUploadHistories.Add(historyRec);
            orm.SaveChanges();
        }

        public static ICollection<DomainEntities.ExcelUploadHistory> GetExcelUploadHistory(long tenantId, int startItem, int recCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.ExcelUploadHistories.Where(x => x.TenantId == tenantId)
                .OrderByDescending(x => x.Id)
                .Skip(startItem - 1)
                .Take(recCount)
                .Select(x => new DomainEntities.ExcelUploadHistory()
                {
                    Id = x.Id,
                    TenantId = x.TenantId,
                    UploadType = x.UploadType,
                    UploadFileName = x.UploadFileName,
                    IsCompleteRefresh = x.IsCompleteRefresh,
                    RecordCount = x.RecordCount,
                    RequestedBy = x.RequestedBy,
                    RequestTimestamp = x.RequestTimestamp,
                    PostingTimestamp = x.PostingTimestamp
                }).ToList();
        }

        public static void SaveSmsData(long tenantId, string smsTemplate, string dataType, string messageData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.TenantSmsData dbRec = new DBModel.TenantSmsData()
            {
                TenantId = tenantId,
                TemplateName = smsTemplate,
                DataType = dataType,
                MessageData = messageData,
                IsSent = false,
                IsFailed = false,
                CreatedOn = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow
            };

            orm.TenantSmsDatas.Add(dbRec);
            orm.SaveChanges();
        }

        public static ICollection<DomainEntities.DivisionSegment> GetDivisionSegment(long tenantId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Divisions.Where(x => x.TenantId == tenantId)
                .Select(x => new DomainEntities.DivisionSegment()
                {
                    DivisionCode = x.DivisionCode,
                    SegmentCode = x.SegmentCode
                }).ToList();
        }

        public static ICollection<DomainEntities.Transporter> GetTransporterData()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Transporters
                .Select(x => new DomainEntities.Transporter()
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    VehicleType = x.VehicleType,
                    VehicleNo = x.VehicleNo,
                    TransportationType = x.TransportationType,
                    SiloToReturnKM = x.SiloToReturnKM,
                    VehicleCapacityKgs = x.VehicleCapacityKgs,
                    HamaliRatePerBag = x.HamaliRatePerBag,
                    CostPerKm = x.CostPerKm,
                    ExtraCostPerTon = x.ExtraCostPerTon
                }).ToList();
        }

        public static ICollection<long> GetDWSIds(string dwsNumber)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.DWS.Where(x => x.DWSNumber.Equals(dwsNumber, StringComparison.OrdinalIgnoreCase))
                .Select(x => x.Id)
                .ToList();
        }

        public static ICollection<DomainEntities.DWS> GetDWS(DWSFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.DWS> items = orm.DWS.Include("Entity")
                .Select(x => new DomainEntities.DWS()
                {
                    Id = x.Id,
                    STRId = x.STRId,
                    STRTagId = x.STRTagId,
                    DWSNumber = x.DWSNumber,
                    DWSDate = x.DWSDate,
                    BagCount = x.BagCount,
                    FilledBagsWeightKg = x.FilledBagsWeightKg,
                    EmptyBagsWeightKg = x.EmptyBagsWeightKg,
                    EntityId = x.EntityId,
                    EntityName = x.Entity.EntityName,
                    AgreementId = x.AgreementId,
                    Agreement = x.Agreement,
                    EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                    TypeName = x.TypeName,
                    TagName = x.TagName,
                    ActivityId = x.ActivityId,
                    STRNumber = x.STRTag.STRNumber,
                    EmployeeId = x.STR.EmployeeId,

                    GoodsWeight = x.GoodsWeight,
                    SiloDeductPercent = x.SiloDeductPercent,
                    SiloDeductWt = x.SiloDeductWt,
                    SiloDeductWtOverride = x.SiloDeductWtOverride,
                    NetPayableWt = x.NetPayableWt,
                    RatePerKg = x.RatePerKg,
                    GoodsPrice = x.GoodsPrice,
                    DeductAmount = x.DeductAmount,
                    NetPayable = x.NetPayable,
                    OrigBagCount = x.OrigBagCount,
                    OrigFilledBagsKg = x.OrigFilledBagsKg,
                    OrigEmptyBagsKg = x.OrigEmptyBagsKg,
                    Status = x.Status,
                    Comments = x.Comments,
                    WtApprovedBy = x.WtApprovedBy,
                    WtApprovedDate = x.WtApprovedDate,
                    AmountApprovedBy = x.AmountApprovedBy,
                    AmountApprovedDate = x.AmountApprovedDate,
                    PaidBy = x.PaidBy,
                    PaidDate = x.PaidDate,
                    PaymentReference = x.PaymentReference,
                    StrTagCyclicCount = x.STRTag.CyclicCount,
                    HQCode = x.Entity.HQCode,
                    CyclicCount = x.CyclicCount,

                    BankAccountName = x.BankAccountName,
                    BankName = x.BankName,
                    BankAccount = x.BankAccount,
                    BankIFSC = x.BankIFSC,
                    BankBranch = x.BankBranch
                });

            if (searchCriteria.ApplyClientNameFilter)
            {
                items = items.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyAgreementNumberFilter)
            {
                items = items.Where(x => x.Agreement.Contains(searchCriteria.AgreementNumber));
            }

            if (searchCriteria.ApplySTRNumberFilter)
            {
                if (searchCriteria.IsExactSTRNumberMatch)
                {
                    items = items.Where(x => x.STRNumber == searchCriteria.STRNumber);
                }
                else
                {
                    items = items.Where(x => x.STRNumber.Contains(searchCriteria.STRNumber));
                }
            }

            if (searchCriteria.ApplyDWSNumberFilter)
            {
                items = items.Where(x => x.DWSNumber.ToUpper().Contains(searchCriteria.DWSNumber.ToUpper()));
            }

            if (searchCriteria.ApplyEntityIdFilter)
            {
                items = items.Where(x => x.EntityId == searchCriteria.EntityId);
            }

            if (searchCriteria.ApplyAgreementIdFilter)
            {
                items = items.Where(x => x.AgreementId == searchCriteria.AgreementId);
            }

            if (searchCriteria.ApplyDateFilter)
            {
                items = items.Where(x => x.DWSDate >= searchCriteria.DateFrom &&
                                                             x.DWSDate <= searchCriteria.DateTo);
            }

            if (searchCriteria.ApplyDWSStatusFilter)
            {
                items = items.Where(x => x.Status == searchCriteria.DWSStatus);
            }

            if (searchCriteria.ApplyPaymentReferenceFilter)
            {
                if (searchCriteria.IsExactPaymentReferenceMatch)
                {
                    items = items.Where(x => x.PaymentReference == searchCriteria.PaymentReference);
                }
                else
                {
                    items = items.Where(x => x.PaymentReference.Contains(searchCriteria.PaymentReference));
                }
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                items = items.Where(x => hqList.Any(y => y == x.HQCode));
            }

            // apply final security for user
            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode)
                                        .Select(x => x.HQCode).ToList();
                items = items.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            return items.OrderBy(x => x.DWSNumber).ToList();
        }

        public static IEnumerable<DomainEntities.DistanceCalcErrorLog>
                                GetDistanceCalcErrorLog(int startItem, int itemCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.DistanceCalcErrorLogs.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip(startItem - 1)
                .Take(itemCount)
                .Select(x => new DomainEntities.DistanceCalcErrorLog()
                {
                    Id = x.Id,
                    APIName = x.APIName,
                    ErrorText = x.ErrorText,
                    At = x.Tracking.At,
                    EmployeeCode = x.Tracking.EmployeeDay.TenantEmployee.EmployeeCode,
                    EmployeeName = x.Tracking.EmployeeDay.TenantEmployee.Name,
                    TrackingId = x.TrackingId,
                    ActivityId = x.Tracking.ActivityId,
                    ActivityType = x.Tracking.ActivityType
                }).ToList();
        }

        public static IEnumerable<DomainEntities.EntityAgreement>
                                GetEntityAgreements(int startItem, int itemCount)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityAgreements.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Skip(startItem - 1)
                .Take(itemCount)
                .Select(x => new DomainEntities.EntityAgreement()
                {
                    Id = x.Id,
                    AgreementNumber = x.AgreementNumber,
                    TypeName = x.WorkflowSeason.TypeName,
                    LandSizeInAcres = x.LandSizeInAcres,
                    EntityId = x.EntityId,
                    WorkflowSeasonId = x.WorkflowSeasonId,
                    WorkflowSeasonName = x.WorkflowSeason.SeasonName,
                    Status = x.Status,
                    IsPassbookReceived = x.IsPassbookReceived,
                    PassbookReceivedDate = x.PassbookReceivedDate,
                    RatePerKg = x.RatePerKg,
                    DWSCount = x.DWS.Count,
                    IssueReturnCount = x.IssueReturns.Count,
                    AdvanceRequestCount = x.AdvanceRequests.Count
                }).ToList();
        }

        public static IEnumerable<DomainEntities.EntityAgreement> GetEntityAgreements(IEnumerable<long> entityIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from o in orm.EntityAgreements
                    join ids in entityIds on o.EntityId equals ids
                    select new DomainEntities.EntityAgreement()
                    {
                        Id = o.Id,
                        AgreementNumber = o.AgreementNumber,
                        TypeName = o.WorkflowSeason.TypeName,
                        LandSizeInAcres = o.LandSizeInAcres,
                        EntityId = o.EntityId,
                        WorkflowSeasonId = o.WorkflowSeasonId,
                        WorkflowSeasonName = o.WorkflowSeason.SeasonName,
                        Status = o.Status,
                        IsPassbookReceived = o.IsPassbookReceived,
                        PassbookReceivedDate = o.PassbookReceivedDate,
                        RatePerKg = o.RatePerKg,
                        DWSCount = o.DWS.Count,
                        IssueReturnCount = o.IssueReturns.Count,
                        AdvanceRequestCount = o.AdvanceRequests.Count
                    }).ToList();
        }

        public static ICollection<DomainEntities.Vendor> GetVendors()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.Vendors.Select(x => new DomainEntities.Vendor()
            {
                Id = x.Id,
                VendorId = x.VendorId,
                CompanyName = x.CompanyName,
                ContactPerson = x.ContactPerson,
                Address = x.Address,
                Mobile = x.Mobile
            }).ToList();
        }

        public static ICollection<DomainEntities.EmployeeAchieved> GetEmployeeAchieveds(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.EmployeeAchieveds.Where(x => x.EmployeeCode == staffCode)
                .Select(x => new DomainEntities.EmployeeAchieved()
                {
                    Id = x.Id,
                    EmployeeCode = x.EmployeeCode,
                    Month = x.Month,
                    Year = x.Year,
                    Type = x.Type,
                    AchievedMonthly = x.AchievedMonthly
                }).ToList();
        }
        //Added by Swetha -Mar 16
        public static ICollection<DomainEntities.LeaveTypes> GetLeaveTypes(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var currentDate = DateTime.Now.Date;

            return orm.LeaveTypes.Where(x => x.EmployeeCode == staffCode && x.StartDate <= currentDate && x.EndDate >= currentDate)
                .Select(x => new DomainEntities.LeaveTypes()
                {
                    Id = x.Id,
                    EmployeeCode = x.EmployeeCode,
                    LeaveType = x.LeaveType1,
                    TotalLeaves = x.TotalLeaves,
                }).ToList();
        }
        //Added by Swetha -Mar 16
        public static IEnumerable<DomainEntities.HolidayList> GetHolidayList(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var currentDate = DateTime.Now.Date;
            var areaCode = (from s in orm.SalesPersons
                            join o in orm.OfficeHierarchies on s.HQCode equals o.HQCode
                            where s.StaffCode == staffCode && s.IsActive
                            select o.AreaCode).Single();

            return (from h in orm.HolidayLists
                    .Where(x => x.AreaCode == areaCode && x.StartDate <= currentDate && x.EndDate >= currentDate)
                    select new DomainEntities.HolidayList()

                    {
                        Id = h.Id,
                        AreaCode = h.AreaCode,
                        Date = h.Date,
                        Description = h.Description,
                    }).ToList();

        }
        public static IEnumerable<DomainEntities.DownloadAvailableLeaves> GetAvailableLeaves(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var currentDate = DateTime.Now.Date;

            var getEmployeeTotalLeaves = from a in orm.LeaveTypes
                                         where a.EmployeeCode == staffCode && a.StartDate <= currentDate && a.EndDate >= currentDate
                                         select new
                                         {
                                             employeeCode = a.EmployeeCode,
                                             leaveType = a.LeaveType1,
                                             totalLeave = a.TotalLeaves,
                                             startDate = a.StartDate,
                                             endDate = a.EndDate
                                         };

            var currentYearStartDate = getEmployeeTotalLeaves.Select(x => x.startDate).FirstOrDefault();

            var currentYearEndDate = getEmployeeTotalLeaves.Select(x => x.endDate).FirstOrDefault();

            var getCurrentYearApprovedleaves = from a in orm.Leaves
                                               where a.EmployeeCode == staffCode && a.LeaveStatus == "Approved"
                                               && a.StartDate >= currentYearStartDate && a.EndDate <= currentYearEndDate
                                               group a by new
                                               {
                                                   a.EmployeeCode,
                                                   a.LeaveType
                                               } into ab
                                               select new
                                               {
                                                   employeeCode = ab.Key.EmployeeCode,
                                                   leaveType = ab.Key.LeaveType,
                                                   approvedLeaves = ab.Sum(x => x.DaysCountExcludingHolidays)
                                               };


            var availableLeavesCount = from a in getEmployeeTotalLeaves
                                       select new DownloadAvailableLeaves()
                                       {
                                           EmployeeCode = a.employeeCode,
                                           LeaveType = a.leaveType,
                                           TotalLeaves = a.totalLeave,
                                           AvailableLeaves = a.totalLeave - (getCurrentYearApprovedleaves.Where(x => x.leaveType == a.leaveType).Select(y => y.approvedLeaves).FirstOrDefault())
                                       };

            return availableLeavesCount.ToList();

        }
        public static ICollection<DomainEntities.EmployeeMonthlyTarget> GetEmployeeMonthlyTargets(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.EmployeeMonthlyTargets.Where(x => x.EmployeeCode == staffCode)
                .Select(x => new DomainEntities.EmployeeMonthlyTarget()
                {
                    Id = x.Id,
                    EmployeeCode = x.EmployeeCode,
                    Month = x.Month,
                    Year = x.Year,
                    Type = x.Type,
                    MonthlyTarget = x.MonthlyTarget
                }).ToList();
        }

        public static ICollection<DomainEntities.EmployeeYearlyTarget> GetEmployeeYearlyTargets(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.EmployeeYearlyTargets.Where(x => x.EmployeeCode == staffCode)
                .Select(x => new DomainEntities.EmployeeYearlyTarget()
                {
                    Id = x.Id,
                    EmployeeCode = x.EmployeeCode,
                    Year = x.Year,
                    Type = x.Type,
                    YearlyTarget = x.YearlyTarget
                }).ToList();
        }

        public static ICollection<DomainEntities.DownloadQuestionnaire> GetQuestionnaire()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.QuestionPapers.Where(x => x.IsActive == true).Select(x => new DomainEntities.DownloadQuestionnaire()
            {
                Id = x.Id,
                Name = x.Name,
                EntityType = x.EntityType,
                QuestionCount = x.QuestionCount,
                Questions = x.QuestionPaperQuestions.Select(y => new DownloadQuestionPaperQuestion
                {
                    Id = y.Id,
                    QText = y.QText,
                    AnswerChoices = y.QuestionPaperAnswers.Select(z => new DownloadQuestionPaperAnswer
                    {
                        Id = z.Id,
                        AText = z.AText
                    }).ToList(),
                    QuestionTypeName = y.QuestionTypeName,
                    AdditionalComment = y.AdditionalComment,
                    QuestionPaperId = y.QuestionPaperId,
                    DisplaySequence = y.DisplaySequence,
                }).ToList()
            }).ToList();
        }

        public static ICollection<string> GetTableList()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            ObjectResult<string> resultSet = orm.TableList();
            return resultSet.Select(x => x).ToList();
        }

        public static DataTable ExcuteRawSelectQuery(string queryString)
        {
            using (SqlConnection SqlConn1 = new SqlConnection(CRMUtilities.Utils.DBConnectionString))
            {
                SqlConn1.Open();
                using (var cmd = SqlConn1.CreateCommand())
                {
                    cmd.CommandText = queryString;
                    var reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
            }
        }

        public static int ExcuteRawScalarQuery(string queryString)
        {
            using (SqlConnection SqlConn1 = new SqlConnection(CRMUtilities.Utils.DBConnectionString))
            {
                SqlConn1.Open();
                using (var cmd = SqlConn1.CreateCommand())
                {
                    cmd.CommandText = queryString;
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public static ICollection<string> GetGRNNumber(int n)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            ObjectResult<string> resultSet = orm.GetGRNNumber(n);
            return resultSet.Select(x => x).ToList();
        }

        public static ICollection<string> GetRequestNumber(int n)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            ObjectResult<string> resultSet = orm.GetRequestNumber(n);
            return resultSet.Select(x => x).ToList();
        }

        public static ICollection<DomainEntities.StockInputTag> GetStockInputTags(StockFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.StockInputTag> items = from e in orm.StockInputTags.Include("StockInput")
                                                             select new DomainEntities.StockInputTag
                                                             {
                                                                 Id = e.Id,
                                                                 GRNNumber = e.GRNNumber,
                                                                 ReceiveDate = e.ReceiveDate,
                                                                 VendorName = e.VendorName,
                                                                 VendorBillNo = e.VendorBillNo,
                                                                 VendorBillDate = e.VendorBillDate,
                                                                 TotalItemCount = e.TotalItemCount,
                                                                 TotalAmount = e.TotalAmount,
                                                                 Notes = e.Notes,
                                                                 Status = e.Status,
                                                                 ZoneCode = e.ZoneCode,
                                                                 AreaCode = e.AreaCode,
                                                                 TerritoryCode = e.TerritoryCode,
                                                                 HQCode = e.HQCode,
                                                                 StaffCode = e.StaffCode,
                                                                 CreatedBy = e.CreatedBy,
                                                                 DateCreated = e.DateCreated,
                                                                 ReviewNotes = e.ReviewNotes,
                                                                 ReviewedBy = e.ReviewedBy,
                                                                 ReviewDate = e.ReviewDate,
                                                                 CyclicCount = e.CyclicCount,
                                                                 ItemCountFromLines = e.StockInputs.Sum(y => y.Quantity),
                                                                 AmountTotalFromLines = e.StockInputs.Sum(y => y.Amount)
                                                             };

            if (searchCriteria.ApplyStockInputTagIdFilter)
            {
                items = items.Where(x => x.Id == searchCriteria.StockInputTagId);
            }
            else
            {
                if (searchCriteria.ApplyVendorNameFilter)
                {
                    items = items.Where(x => x.VendorName == searchCriteria.VendorName);
                }

                if (searchCriteria.ApplyGRNNumberFilter)
                {
                    if (searchCriteria.IsExactGrnNumberMatch)
                    {
                        items = items.Where(x => x.GRNNumber == searchCriteria.GRNNumber);
                    }
                    else
                    {
                        items = items.Where(x => x.GRNNumber.Contains(searchCriteria.GRNNumber));
                    }
                }

                if (searchCriteria.ApplyInvoiceNumberFilter)
                {
                    items = items.Where(x => x.VendorBillNo.Contains(searchCriteria.InvoiceNumber));
                }

                if (searchCriteria.ApplyDateFilter)
                {
                    items = items.Where(x => x.ReceiveDate >= searchCriteria.DateFrom &&
                                                                    x.ReceiveDate <= searchCriteria.DateTo);
                }

                if (searchCriteria.ApplyStatusFilter)
                {
                    items = items.Where(x => x.Status == searchCriteria.Status);
                }

                if (searchCriteria.ApplyZoneFilter)
                {
                    items = items.Where(x => x.ZoneCode == searchCriteria.Zone);
                }

                if (searchCriteria.ApplyAreaFilter)
                {
                    items = items.Where(x => x.AreaCode == searchCriteria.Area);
                }

                if (searchCriteria.ApplyTerritoryFilter)
                {
                    items = items.Where(x => x.TerritoryCode == searchCriteria.Territory);
                }

                if (searchCriteria.ApplyHQFilter)
                {
                    items = items.Where(x => x.HQCode == searchCriteria.HQ);
                }
            }

            return items.OrderBy(x => x.GRNNumber).ToList();
        }

        public static void CreateStockInputTag(DomainEntities.StockInputTag inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.StockInputTags.Add(new DBModel.StockInputTag()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CyclicCount = 1,
                Status = inputRec.Status,
                Notes = inputRec.Notes,
                GRNNumber = inputRec.GRNNumber,
                ReceiveDate = inputRec.ReceiveDate,
                VendorName = inputRec.VendorName,
                VendorBillNo = inputRec.VendorBillNo,
                VendorBillDate = inputRec.VendorBillDate,
                TotalItemCount = inputRec.TotalItemCount,
                TotalAmount = inputRec.TotalAmount,
                ZoneCode = inputRec.ZoneCode,
                AreaCode = inputRec.AreaCode,
                TerritoryCode = inputRec.TerritoryCode,
                HQCode = inputRec.HQCode,
                StaffCode = inputRec.StaffCode,
                ReviewDate = DateTime.MinValue,
                ReviewedBy = "",
                ReviewNotes = ""
            });

            try
            {
                orm.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus SaveStockInputTagData(DomainEntities.StockInputTag inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.StockInputTags.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid Stock Input Tag Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;
            sp.CyclicCount++;
            sp.Notes = inputRec.Notes;
            sp.Status = inputRec.Status;

            sp.GRNNumber = inputRec.GRNNumber;
            sp.ReceiveDate = inputRec.ReceiveDate;
            sp.VendorName = inputRec.VendorName;
            sp.VendorBillNo = inputRec.VendorBillNo;
            sp.VendorBillDate = inputRec.VendorBillDate;
            sp.TotalItemCount = inputRec.TotalItemCount;
            sp.TotalAmount = inputRec.TotalAmount;
            sp.ZoneCode = inputRec.ZoneCode;
            sp.AreaCode = inputRec.AreaCode;
            sp.TerritoryCode = inputRec.TerritoryCode;
            sp.HQCode = inputRec.HQCode;
            sp.StaffCode = inputRec.StaffCode;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        //public static ICollection<DomainEntities.StockInput> GetStockInput(long stockInputTagId)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;

        //    return orm.StockInputs.Include("ItemMaster")
        //        .OrderBy(x=> x.LineNumber)
        //        .Where(x => x.StockInputTagId == stockInputTagId)
        //        .Select(ex=> new DomainEntities.StockInput()
        //        {
        //            Id = ex.Id,
        //            StockInputTagId = ex.StockInputTagId,
        //            LineNumber = ex.LineNumber,
        //            ItemMasterId = ex.ItemMasterId,
        //            Quantity = ex.Quantity,
        //            Rate = ex.Rate,
        //            Amount = ex.Amount,
        //            CyclicCount = ex.CyclicCount,
        //            ItemCode = ex.ItemMaster.ItemCode,
        //            ItemDesc = ex.ItemMaster.ItemDesc,
        //            Category = ex.ItemMaster.Category,
        //            Unit = ex.ItemMaster.Unit
        //        }).ToList();
        //}

        public static ICollection<DomainEntities.StockInput> GetStockInput(ICollection<long> stockInputTagIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return (from ex in orm.StockInputs.Include("ItemMaster")
                    join tagId in stockInputTagIds on ex.StockInputTagId equals tagId
                    select new DomainEntities.StockInput()
                    {
                        Id = ex.Id,
                        StockInputTagId = ex.StockInputTagId,
                        LineNumber = ex.LineNumber,
                        ItemMasterId = ex.ItemMasterId,
                        Quantity = ex.Quantity,
                        Rate = ex.Rate,
                        Amount = ex.Amount,
                        CyclicCount = ex.CyclicCount,
                        ItemCode = ex.ItemMaster.ItemCode,
                        ItemDesc = ex.ItemMaster.ItemDesc,
                        Category = ex.ItemMaster.Category,
                        Unit = ex.ItemMaster.Unit
                    }).ToList();
        }

        public static DBSaveStatus CreateStockInputItem(DomainEntities.StockInput inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var parent = orm.StockInputTags.Find(inputRec.StockInputTagId);
            if (parent == null || parent.CyclicCount != inputRec.ParentCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            parent.CyclicCount++;

            orm.StockInputs.Add(new DBModel.StockInput()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CyclicCount = inputRec.CyclicCount,

                LineNumber = inputRec.LineNumber,
                ItemMasterId = inputRec.ItemMasterId,
                Quantity = inputRec.Quantity,
                Rate = inputRec.Rate,
                Amount = inputRec.Amount,

                StockInputTagId = inputRec.StockInputTagId
            });

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus SaveStockInputItem(DomainEntities.StockInput inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var currentRec = orm.StockInputs.Find(inputRec.Id);
            if (currentRec == null)
            {
                throw new ArgumentException("Invalid Stock Input Id");
            }

            var parent = currentRec.StockInputTag;

            if (parent.CyclicCount != inputRec.ParentCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            parent.CyclicCount++;

            currentRec.UpdatedBy = inputRec.CurrentUser;
            currentRec.DateUpdated = DateTime.UtcNow;
            currentRec.CyclicCount++;

            currentRec.LineNumber = inputRec.LineNumber;
            currentRec.ItemMasterId = inputRec.ItemMasterId;
            currentRec.Quantity = inputRec.Quantity;
            currentRec.Rate = inputRec.Rate;
            currentRec.Amount = inputRec.Amount;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus DeleteStockInputItem(DomainEntities.StockInput inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var currentRec = orm.StockInputs.Find(inputRec.Id);
            if (currentRec == null)
            {
                throw new ArgumentException("Invalid Stock Input Id");
            }

            var parent = currentRec.StockInputTag;

            if (parent.CyclicCount != inputRec.ParentCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            try
            {
                parent.CyclicCount++;
                orm.StockInputs.Remove(currentRec);
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static ICollection<DomainEntities.StockRequestTag> GetStockRequestTags(StockRequestFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.StockRequestTag> items = from e in orm.StockRequestTags.Include("StockRequest")
                                                               select new DomainEntities.StockRequestTag
                                                               {
                                                                   Id = e.Id,
                                                                   RequestNumber = e.RequestNumber,
                                                                   RequestDate = e.RequestDate,
                                                                   Notes = e.Notes,
                                                                   Status = e.Status,
                                                                   ZoneCode = e.ZoneCode,
                                                                   AreaCode = e.AreaCode,
                                                                   TerritoryCode = e.TerritoryCode,
                                                                   HQCode = e.HQCode,
                                                                   StaffCode = e.StaffCode,
                                                                   CyclicCount = e.CyclicCount,
                                                                   RequestType = e.RequestType,
                                                                   CreatedBy = e.CreatedBy,
                                                                   DateCreated = e.DateCreated,
                                                                   ItemCountFromLines = e.StockRequests.Sum(y => y.Quantity),
                                                               };

            if (searchCriteria.ApplyStockRequestTagIdFilter)
            {
                items = items.Where(x => x.Id == searchCriteria.StockRequestTagId);
            }
            else
            {
                items = items.Where(x => x.RequestType == searchCriteria.StockRequestType);

                if (searchCriteria.ApplyRequestNumberFilter)
                {
                    if (searchCriteria.IsExactRequestNumberMatch)
                    {
                        items = items.Where(x => x.RequestNumber == searchCriteria.RequestNumber);
                    }
                    else
                    {
                        items = items.Where(x => x.RequestNumber.Contains(searchCriteria.RequestNumber));
                    }
                }

                if (searchCriteria.ApplyDateFilter)
                {
                    items = items.Where(x => x.RequestDate >= searchCriteria.DateFrom &&
                                                                    x.RequestDate <= searchCriteria.DateTo);
                }

                if (searchCriteria.ApplyStatusFilter)
                {
                    items = items.Where(x => x.Status == searchCriteria.Status);
                }

                if (searchCriteria.ApplyZoneFilter)
                {
                    items = items.Where(x => x.ZoneCode == searchCriteria.Zone);
                }

                if (searchCriteria.ApplyAreaFilter)
                {
                    items = items.Where(x => x.AreaCode == searchCriteria.Area);
                }

                if (searchCriteria.ApplyTerritoryFilter)
                {
                    items = items.Where(x => x.TerritoryCode == searchCriteria.Territory);
                }

                if (searchCriteria.ApplyHQFilter)
                {
                    items = items.Where(x => x.HQCode == searchCriteria.HQ);
                }

                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    items = items.Where(x => x.StaffCode.Contains(searchCriteria.EmployeeCode));
                }
            }

            return items.OrderBy(x => x.RequestNumber).ToList();
        }

        public static ICollection<DomainEntities.StockRequestFull> GetStockRequestItems(StockRequestFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.StockRequestFull> items = from e in orm.StockRequests
                                                                where e.StockRequestTag.Status.Equals(searchCriteria.TagRecordStatus, StringComparison.OrdinalIgnoreCase)
                                                                select new DomainEntities.StockRequestFull
                                                                {
                                                                    Id = e.Id,
                                                                    StockRequestTagId = e.StockRequestTagId,
                                                                    RequestNumber = e.StockRequestTag.RequestNumber,
                                                                    RequestDate = e.StockRequestTag.RequestDate,
                                                                    Notes = e.StockRequestTag.Notes,
                                                                    ItemMasterId = e.ItemMasterId,
                                                                    Quantity = e.Quantity,

                                                                    ItemCode = e.ItemMaster.ItemCode,
                                                                    ItemDesc = e.ItemMaster.ItemDesc,
                                                                    Category = e.ItemMaster.Category,
                                                                    Unit = e.ItemMaster.Unit,

                                                                    Status = e.Status,
                                                                    ZoneCode = e.StockRequestTag.ZoneCode,
                                                                    AreaCode = e.StockRequestTag.AreaCode,
                                                                    TerritoryCode = e.StockRequestTag.TerritoryCode,
                                                                    HQCode = e.StockRequestTag.HQCode,
                                                                    StaffCode = e.StockRequestTag.StaffCode,

                                                                    CyclicCount = e.CyclicCount,
                                                                    QuantityIssued = e.QuantityIssued,
                                                                    IssueDate = e.IssueDate,
                                                                    ReviewNotes = e.ReviewNotes,
                                                                    StockLedgerId = e.StockLedgerId,
                                                                    RequestType = e.StockRequestTag.RequestType,
                                                                    CreatedBy = e.StockRequestTag.CreatedBy,
                                                                    DateCreated = e.StockRequestTag.DateCreated,

                                                                    UpdatedBy = e.UpdatedBy,
                                                                    DateUpdated = e.DateUpdated
                                                                };

            if (searchCriteria.ApplyStockRequestIdFilter)
            {
                items = items.Where(x => x.Id == searchCriteria.StockRequestId);
            }
            else
            {
                items = items.Where(x => x.RequestType == searchCriteria.StockRequestType);

                if (searchCriteria.ApplyRequestNumberFilter)
                {
                    if (searchCriteria.IsExactRequestNumberMatch)
                    {
                        items = items.Where(x => x.RequestNumber == searchCriteria.RequestNumber);
                    }
                    else
                    {
                        items = items.Where(x => x.RequestNumber.Contains(searchCriteria.RequestNumber));
                    }
                }

                if (searchCriteria.ApplyItemIdFilter)
                {
                    items = items.Where(x => x.ItemMasterId == searchCriteria.ItemId);
                }
                else if (searchCriteria.ApplyItemTypeFilter)
                {
                    items = items.Where(x => x.Category.Equals(searchCriteria.ItemType, StringComparison.OrdinalIgnoreCase));
                }

                if (searchCriteria.ApplyDateFilter)
                {
                    items = items.Where(x => x.RequestDate >= searchCriteria.DateFrom &&
                                                                    x.RequestDate <= searchCriteria.DateTo);
                }

                if (searchCriteria.ApplyStatusFilter)
                {
                    items = items.Where(x => x.Status == searchCriteria.Status);
                }

                if (searchCriteria.ApplyZoneFilter)
                {
                    items = items.Where(x => x.ZoneCode == searchCriteria.Zone);
                }

                if (searchCriteria.ApplyAreaFilter)
                {
                    items = items.Where(x => x.AreaCode == searchCriteria.Area);
                }

                if (searchCriteria.ApplyTerritoryFilter)
                {
                    items = items.Where(x => x.TerritoryCode == searchCriteria.Territory);
                }

                if (searchCriteria.ApplyHQFilter)
                {
                    items = items.Where(x => x.HQCode == searchCriteria.HQ);
                }

                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    items = items.Where(x => x.StaffCode.Contains(searchCriteria.EmployeeCode));
                }
            }

            return items.OrderBy(x => x.RequestNumber).ToList();
        }

        public static ICollection<DomainEntities.StockLedger> GetStockLedger(StockLedgerFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.StockLedger> items = from e in orm.StockLedgers.Include("ItemMaster")
                                                           select new DomainEntities.StockLedger
                                                           {
                                                               Id = e.Id,
                                                               TransactionDate = e.TransactionDate,
                                                               ItemMasterId = e.ItemMasterId,
                                                               ReferenceNo = e.ReferenceNo,
                                                               LineNumber = e.LineNumber,
                                                               IssueQuantity = e.IssueQuantity,
                                                               ReceiveQuantity = e.ReceiveQuantity,

                                                               ZoneCode = e.ZoneCode,
                                                               AreaCode = e.AreaCode,
                                                               TerritoryCode = e.TerritoryCode,
                                                               HQCode = e.HQCode,
                                                               StaffCode = e.StaffCode,

                                                               ItemCode = e.ItemMaster.ItemCode,
                                                               ItemDesc = e.ItemMaster.ItemDesc,
                                                               Category = e.ItemMaster.Category,
                                                               Unit = e.ItemMaster.Unit,
                                                               CreatedBy = e.CreatedBy,
                                                               DateCreated = e.DateCreated
                                                           };
            if (searchCriteria.ApplyRecordIdFilter)
            {
                items = items.Where(x => x.Id == searchCriteria.RecordId);
            }
            else
            {
                if (searchCriteria.ApplyReferenceNumberFilter)
                {
                    items = items.Where(x => x.ReferenceNo.Contains(searchCriteria.ReferenceNumber));
                }

                if (searchCriteria.ApplyDateFilter)
                {
                    items = items.Where(x => x.TransactionDate >= searchCriteria.DateFrom &&
                                                                    x.TransactionDate <= searchCriteria.DateTo);
                }

                if (searchCriteria.ApplyItemIdFilter)
                {
                    items = items.Where(x => x.ItemMasterId == searchCriteria.ItemId);
                }
                else if (searchCriteria.ApplyItemTypeFilter)
                {
                    items = items.Where(x => x.Category.Equals(searchCriteria.ItemType, StringComparison.OrdinalIgnoreCase));
                }

                if (searchCriteria.ApplyZoneFilter)
                {
                    items = items.Where(x => x.ZoneCode == searchCriteria.Zone);
                }

                if (searchCriteria.ApplyAreaFilter)
                {
                    items = items.Where(x => x.AreaCode == searchCriteria.Area);
                }

                if (searchCriteria.ApplyTerritoryFilter)
                {
                    items = items.Where(x => x.TerritoryCode == searchCriteria.Territory);
                }

                if (searchCriteria.ApplyHQFilter)
                {
                    items = items.Where(x => x.HQCode == searchCriteria.HQ);
                }

                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    items = items.Where(x => x.StaffCode.Contains(searchCriteria.EmployeeCode));
                }
            }

            return items.OrderBy(x => x.TransactionDate).ToList();
        }

        public static ICollection<DomainEntities.StockBalance> GetStockBalance(StockLedgerFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.StockBalance> items = from e in orm.StockBalances.Include("ItemMaster")
                                                            select new DomainEntities.StockBalance
                                                            {
                                                                Id = e.Id,
                                                                ItemMasterId = e.ItemMasterId,
                                                                StockQuantity = e.StockQuantity,

                                                                ZoneCode = e.ZoneCode,
                                                                AreaCode = e.AreaCode,
                                                                TerritoryCode = e.TerritoryCode,
                                                                HQCode = e.HQCode,
                                                                StaffCode = e.StaffCode,

                                                                ItemCode = e.ItemMaster.ItemCode,
                                                                ItemDesc = e.ItemMaster.ItemDesc,
                                                                Category = e.ItemMaster.Category,
                                                                Unit = e.ItemMaster.Unit,
                                                                CyclicCount = e.CyclicCount,
                                                                DateUpdated = e.DateUpdated,
                                                                UpdatedBy = e.UpdatedBy
                                                            };
            if (searchCriteria.ApplyRecordIdFilter)
            {
                items = items.Where(x => x.Id == searchCriteria.RecordId);
            }
            else
            {
                if (searchCriteria.ApplyItemIdFilter)
                {
                    items = items.Where(x => x.ItemMasterId == searchCriteria.ItemId);
                }
                else if (searchCriteria.ApplyItemTypeFilter)
                {
                    items = items.Where(x => x.Category.Equals(searchCriteria.ItemType, StringComparison.OrdinalIgnoreCase));
                }

                if (searchCriteria.ApplyZoneFilter)
                {
                    items = items.Where(x => x.ZoneCode == searchCriteria.Zone);
                }

                if (searchCriteria.ApplyAreaFilter)
                {
                    items = items.Where(x => x.AreaCode == searchCriteria.Area);
                }

                if (searchCriteria.ApplyTerritoryFilter)
                {
                    items = items.Where(x => x.TerritoryCode == searchCriteria.Territory);
                }

                if (searchCriteria.ApplyHQFilter)
                {
                    items = items.Where(x => x.HQCode == searchCriteria.HQ);
                }

                if (searchCriteria.ApplyEmployeeCodeFilter)
                {
                    items = items.Where(x => x.StaffCode.Contains(searchCriteria.EmployeeCode));
                }
            }

            return items.OrderBy(x => x.ItemDesc).ToList();
        }

        public static void CreateStockRequestTag(DomainEntities.StockRequestTag inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.StockRequestTags.Add(new DBModel.StockRequestTag()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CyclicCount = 1,
                Status = inputRec.Status,
                Notes = inputRec.Notes,
                RequestNumber = inputRec.RequestNumber,
                RequestDate = inputRec.RequestDate,
                ZoneCode = inputRec.ZoneCode,
                AreaCode = inputRec.AreaCode,
                TerritoryCode = inputRec.TerritoryCode,
                HQCode = inputRec.HQCode,
                StaffCode = inputRec.StaffCode,
                RequestType = inputRec.RequestType
            });

            try
            {
                orm.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus SaveStockRequestTagData(DomainEntities.StockRequestTag inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.StockRequestTags.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid Stock Input Tag Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;
            sp.CyclicCount++;
            sp.Notes = inputRec.Notes;
            sp.Status = inputRec.Status;

            sp.RequestNumber = inputRec.RequestNumber;
            sp.RequestDate = inputRec.RequestDate;
            sp.ZoneCode = inputRec.ZoneCode;
            sp.AreaCode = inputRec.AreaCode;
            sp.TerritoryCode = inputRec.TerritoryCode;
            sp.HQCode = inputRec.HQCode;
            sp.StaffCode = inputRec.StaffCode;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static ICollection<DomainEntities.StockRequest> GetStockRequest(long stockRequestTagId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return orm.StockRequests.Include("ItemMaster")
                .Where(x => x.StockRequestTagId == stockRequestTagId)
                .Select(ex => new DomainEntities.StockRequest()
                {
                    Id = ex.Id,
                    StockRequestTagId = ex.StockRequestTagId,
                    ItemMasterId = ex.ItemMasterId,
                    Quantity = ex.Quantity,
                    CyclicCount = ex.CyclicCount,
                    ItemCode = ex.ItemMaster.ItemCode,
                    ItemDesc = ex.ItemMaster.ItemDesc,
                    Category = ex.ItemMaster.Category,
                    Unit = ex.ItemMaster.Unit,
                    Status = ex.Status,
                    QuantityIssued = ex.QuantityIssued,
                    IssueDate = ex.IssueDate,
                    StockLedgerId = ex.StockLedgerId,
                    DateUpdated = ex.DateUpdated,
                    UpdatedBy = ex.UpdatedBy,
                    ReviewNotes = ex.ReviewNotes
                }).ToList();
        }

        public static DBSaveStatus CreateStockRequestItem(DomainEntities.StockRequest inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.StockRequestTag parent = orm.StockRequestTags.Find(inputRec.StockRequestTagId);
            if (parent == null || parent.CyclicCount != inputRec.ParentCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            parent.CyclicCount++;

            orm.StockRequests.Add(new DBModel.StockRequest()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CyclicCount = inputRec.CyclicCount,

                ItemMasterId = inputRec.ItemMasterId,
                Quantity = inputRec.Quantity,
                Status = inputRec.Status,

                StockRequestTagId = inputRec.StockRequestTagId,

                IssueDate = DateTime.MinValue,
                QuantityIssued = 0,
                StockLedgerId = 0,
                ReviewNotes = ""
            });

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus SaveStockRequestItem(DomainEntities.StockRequest inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var currentRec = orm.StockRequests.Find(inputRec.Id);
            if (currentRec == null)
            {
                throw new ArgumentException("Invalid Stock Request Id");
            }

            var parent = currentRec.StockRequestTag;

            if (parent.CyclicCount != inputRec.ParentCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            parent.CyclicCount++;

            currentRec.UpdatedBy = inputRec.CurrentUser;
            currentRec.DateUpdated = DateTime.UtcNow;
            currentRec.CyclicCount++;

            currentRec.ItemMasterId = inputRec.ItemMasterId;
            currentRec.Quantity = inputRec.Quantity;
            currentRec.Status = inputRec.Status;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus DeleteStockRequestItem(DomainEntities.StockRequest inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var currentRec = orm.StockRequests.Find(inputRec.Id);
            if (currentRec == null)
            {
                throw new ArgumentException("Invalid Stock Request Id");
            }

            var parent = currentRec.StockRequestTag;

            if (parent.CyclicCount != inputRec.ParentCyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            try
            {
                parent.CyclicCount++;
                orm.StockRequests.Remove(currentRec);
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus ReviewStockInputTagData(DomainEntities.StockInputTag inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.StockInputTags.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid Stock Input Tag Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;
            sp.CyclicCount++;
            sp.Status = inputRec.Status;
            sp.ReviewDate = DateTime.UtcNow;
            sp.ReviewedBy = inputRec.CurrentUser;
            sp.ReviewNotes = inputRec.ReviewNotes;
            //sp.WarehouseType = inputRec.WarehouseType;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static DBSaveStatus ReviewStockRequestTagData(DomainEntities.StockRequestTag inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.StockRequestTags.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid Stock Request Tag Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;
            sp.CyclicCount++;
            sp.Status = inputRec.Status;
            //sp.WarehouseType = inputRec.WarehouseType;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static void PostStockLedgerFromInput(long stockInputTagId, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            orm.PostStockLedgerFromInput(stockInputTagId, currentUser);
        }

        public static SalesPersonMiniModel GetSalesPersonData(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.SalesPersons.Where(x => x.StaffCode == staffCode)
                .Select(x => new SalesPersonMiniModel()
                {
                    StaffCode = staffCode,
                    Name = x.Name
                }).FirstOrDefault();
        }

        public static DBSaveStatus PerformFulfillment(StockFulfillmentData fulfillmentData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DateTime currentTime = DateTime.UtcNow;

            // Now create row in Stock Ledger
            DBModel.StockRequest stockRequestRec = orm.StockRequests.Find(fulfillmentData.StockRequestId);
            DBModel.StockRequestTag stockRequestTagRec = orm.StockRequestTags.Find(fulfillmentData.StockRequestTagId);

            if (stockRequestRec == null || stockRequestTagRec == null)
            {
                throw new ArgumentException("Invalid StockRequest or StockRequestTag Id");
            }

            if (stockRequestRec.CyclicCount != fulfillmentData.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            // update stock balance for each Stock Balance Record
            foreach (var sb in fulfillmentData.StockBalances)
            {
                var stockBalanceRec = orm.StockBalances.Find(sb.Id);
                if (stockBalanceRec == null)
                {
                    throw new ArgumentException("Invalid Stock Balance Id");
                }

                if (stockBalanceRec.CyclicCount != sb.CyclicCount)
                {
                    return DBSaveStatus.CyclicCheckFail;
                }

                stockBalanceRec.CyclicCount++;
                stockBalanceRec.UpdatedBy = fulfillmentData.CurrentUser;
                stockBalanceRec.DateUpdated = currentTime;
                stockBalanceRec.StockQuantity -= sb.IssueQuantity;

                // create record in stock Ledger
                orm.StockLedgers.Add(new DBModel.StockLedger()
                {
                    TransactionDate = fulfillmentData.CurrentIstTime,
                    ItemMasterId = stockBalanceRec.ItemMasterId,
                    ReferenceNo = stockRequestTagRec.RequestNumber,
                    LineNumber = 0,
                    IssueQuantity = sb.IssueQuantity,
                    ReceiveQuantity = 0,
                    ZoneCode = stockBalanceRec.ZoneCode,
                    AreaCode = stockBalanceRec.AreaCode,
                    TerritoryCode = stockBalanceRec.TerritoryCode,
                    HQCode = stockBalanceRec.HQCode,
                    StaffCode = stockBalanceRec.StaffCode,
                    CyclicCount = 1,
                    UpdatedBy = fulfillmentData.CurrentUser,
                    CreatedBy = fulfillmentData.CurrentUser,
                    DateCreated = currentTime,
                    DateUpdated = currentTime
                });
            }

            int totalIssueQty = fulfillmentData.StockBalances.Sum(x => x.IssueQuantity);

            // update stock request Record;
            stockRequestRec.CyclicCount++;
            stockRequestRec.UpdatedBy = fulfillmentData.CurrentUser;
            stockRequestRec.DateUpdated = currentTime;
            stockRequestRec.Status = fulfillmentData.Status;
            stockRequestRec.QuantityIssued = totalIssueQty;
            stockRequestRec.IssueDate = fulfillmentData.CurrentIstTime;
            stockRequestRec.StockLedgerId = 0;
            stockRequestRec.ReviewNotes = fulfillmentData.Notes;

            // Now create ledger entry for receive
            DBModel.StockLedger receiveStockLedgerRec =
                new DBModel.StockLedger()
                {
                    TransactionDate = fulfillmentData.CurrentIstTime,
                    ItemMasterId = stockRequestRec.ItemMasterId,
                    ReferenceNo = stockRequestTagRec.RequestNumber,
                    LineNumber = 0,
                    IssueQuantity = 0,
                    ReceiveQuantity = totalIssueQty,
                    ZoneCode = stockRequestTagRec.ZoneCode,
                    AreaCode = stockRequestTagRec.AreaCode,
                    TerritoryCode = stockRequestTagRec.TerritoryCode,
                    HQCode = stockRequestTagRec.HQCode,
                    StaffCode = stockRequestTagRec.StaffCode,
                    CyclicCount = 1,
                    UpdatedBy = fulfillmentData.CurrentUser,
                    CreatedBy = fulfillmentData.CurrentUser,
                    DateCreated = currentTime,
                    DateUpdated = currentTime
                };

            orm.StockLedgers.Add(receiveStockLedgerRec);

            // Now create or update stock balance entry for receiver;
            var receiverStockBalanceRec = orm.StockBalances.Where(x => x.ZoneCode == stockRequestTagRec.ZoneCode &&
            x.AreaCode == stockRequestTagRec.AreaCode &&
            x.TerritoryCode == stockRequestTagRec.TerritoryCode &&
            x.HQCode == stockRequestTagRec.HQCode &&
            x.StaffCode == stockRequestTagRec.StaffCode &&
            x.ItemMasterId == stockRequestRec.ItemMasterId).FirstOrDefault();

            if (receiverStockBalanceRec != null)
            {
                receiverStockBalanceRec.StockQuantity += totalIssueQty;
                receiverStockBalanceRec.CyclicCount++;
                receiverStockBalanceRec.UpdatedBy = fulfillmentData.CurrentUser;
                receiverStockBalanceRec.DateUpdated = currentTime;
            }
            else
            {
                orm.StockBalances.Add(new DBModel.StockBalance()
                {
                    ItemMasterId = stockRequestRec.ItemMasterId,
                    StockQuantity = totalIssueQty,
                    ZoneCode = stockRequestTagRec.ZoneCode,
                    AreaCode = stockRequestTagRec.AreaCode,
                    TerritoryCode = stockRequestTagRec.TerritoryCode,
                    HQCode = stockRequestTagRec.HQCode,
                    StaffCode = stockRequestTagRec.StaffCode,
                    CyclicCount = 1,
                    DateCreated = currentTime,
                    DateUpdated = currentTime,
                    CreatedBy = fulfillmentData.CurrentUser,
                    UpdatedBy = fulfillmentData.CurrentUser
                });
            }

            orm.SaveChanges();

            stockRequestRec = orm.StockRequests.Find(stockRequestRec.Id);
            stockRequestRec.StockLedgerId = receiveStockLedgerRec.Id;
            orm.SaveChanges();

            return DBSaveStatus.Success;
        }

        public static DBSaveStatus PerformStockClear(StockFulfillmentData fulfillmentData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DateTime currentTime = DateTime.UtcNow;

            // Now create row in Stock Ledger
            DBModel.StockRequest stockRequestRec = orm.StockRequests.Find(fulfillmentData.StockRequestId);
            DBModel.StockRequestTag stockRequestTagRec = orm.StockRequestTags.Find(fulfillmentData.StockRequestTagId);

            if (stockRequestRec == null || stockRequestTagRec == null)
            {
                throw new ArgumentException("Invalid StockRequest or StockRequestTag Id");
            }

            if (stockRequestRec.CyclicCount != fulfillmentData.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            // update stock balance for each Stock Balance Record
            var stockBalanceRec = orm.StockBalances.Find(fulfillmentData.StockBalanceRec.Id);
            if (stockBalanceRec == null)
            {
                throw new ArgumentException("Invalid Stock Balance Id");
            }

            if (stockBalanceRec.CyclicCount != fulfillmentData.StockBalanceRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            stockBalanceRec.CyclicCount++;
            stockBalanceRec.UpdatedBy = fulfillmentData.CurrentUser;
            stockBalanceRec.DateUpdated = currentTime;
            stockBalanceRec.StockQuantity -= stockRequestRec.Quantity;

            // create record in stock Ledger
            var stockLedgerRec = new DBModel.StockLedger()
            {
                TransactionDate = fulfillmentData.CurrentIstTime,
                ItemMasterId = stockBalanceRec.ItemMasterId,
                ReferenceNo = stockRequestTagRec.RequestNumber,
                LineNumber = 0,
                IssueQuantity = stockRequestRec.Quantity,
                ReceiveQuantity = 0,
                ZoneCode = stockBalanceRec.ZoneCode,
                AreaCode = stockBalanceRec.AreaCode,
                TerritoryCode = stockBalanceRec.TerritoryCode,
                HQCode = stockBalanceRec.HQCode,
                StaffCode = stockBalanceRec.StaffCode,
                CyclicCount = 1,
                UpdatedBy = fulfillmentData.CurrentUser,
                CreatedBy = fulfillmentData.CurrentUser,
                DateCreated = currentTime,
                DateUpdated = currentTime
            };
            orm.StockLedgers.Add(stockLedgerRec);

            // update stock request Record;
            stockRequestRec.CyclicCount++;
            stockRequestRec.UpdatedBy = fulfillmentData.CurrentUser;
            stockRequestRec.DateUpdated = currentTime;
            stockRequestRec.Status = fulfillmentData.Status;
            stockRequestRec.QuantityIssued = stockRequestRec.Quantity;
            stockRequestRec.IssueDate = fulfillmentData.CurrentIstTime;
            stockRequestRec.StockLedgerId = 0;
            stockRequestRec.ReviewNotes = fulfillmentData.Notes;

            orm.SaveChanges();

            stockRequestRec = orm.StockRequests.Find(stockRequestRec.Id);
            stockRequestRec.StockLedgerId = stockLedgerRec.Id;
            orm.SaveChanges();

            return DBSaveStatus.Success;
        }

        public static DBSaveStatus PerformStockAdd(StockFulfillmentData fulfillmentData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DateTime currentTime = DateTime.UtcNow;

            // Now create row in Stock Ledger
            DBModel.StockRequest stockRequestRec = orm.StockRequests.Find(fulfillmentData.StockRequestId);
            DBModel.StockRequestTag stockRequestTagRec = orm.StockRequestTags.Find(fulfillmentData.StockRequestTagId);

            if (stockRequestRec == null || stockRequestTagRec == null)
            {
                throw new ArgumentException("Invalid StockRequest or StockRequestTag Id");
            }

            if (stockRequestRec.CyclicCount != fulfillmentData.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            // update stock balance
            if (fulfillmentData.StockBalanceRec != null)
            {
                var stockBalanceRec = orm.StockBalances.Find(fulfillmentData.StockBalanceRec.Id);
                if (stockBalanceRec == null)
                {
                    throw new ArgumentException("Invalid Stock Balance Id");
                }

                if (stockBalanceRec.CyclicCount != fulfillmentData.StockBalanceRec.CyclicCount)
                {
                    return DBSaveStatus.CyclicCheckFail;
                }

                stockBalanceRec.CyclicCount++;
                stockBalanceRec.UpdatedBy = fulfillmentData.CurrentUser;
                stockBalanceRec.DateUpdated = currentTime;
                stockBalanceRec.StockQuantity += stockRequestRec.Quantity;
            }
            else
            {
                orm.StockBalances.Add(new DBModel.StockBalance()
                {
                    CyclicCount = 1,
                    UpdatedBy = fulfillmentData.CurrentUser,
                    CreatedBy = fulfillmentData.CurrentUser,
                    DateCreated = currentTime,
                    DateUpdated = currentTime,
                    StockQuantity = stockRequestRec.Quantity,
                    ZoneCode = stockRequestTagRec.ZoneCode,
                    AreaCode = stockRequestTagRec.AreaCode,
                    TerritoryCode = stockRequestTagRec.TerritoryCode,
                    HQCode = stockRequestTagRec.HQCode,
                    StaffCode = stockRequestTagRec.StaffCode,
                    ItemMasterId = stockRequestRec.ItemMasterId,
                });
            }

            // create record in stock Ledger
            var stockLedgerRec = new DBModel.StockLedger()
            {
                TransactionDate = fulfillmentData.CurrentIstTime,
                ItemMasterId = stockRequestRec.ItemMasterId,
                ReferenceNo = stockRequestTagRec.RequestNumber,
                LineNumber = 0,
                IssueQuantity = 0,
                ReceiveQuantity = stockRequestRec.Quantity,
                ZoneCode = stockRequestTagRec.ZoneCode,
                AreaCode = stockRequestTagRec.AreaCode,
                TerritoryCode = stockRequestTagRec.TerritoryCode,
                HQCode = stockRequestTagRec.HQCode,
                StaffCode = stockRequestTagRec.StaffCode,
                CyclicCount = 1,
                UpdatedBy = fulfillmentData.CurrentUser,
                CreatedBy = fulfillmentData.CurrentUser,
                DateCreated = currentTime,
                DateUpdated = currentTime
            };
            orm.StockLedgers.Add(stockLedgerRec);

            // update stock request Record;
            stockRequestRec.CyclicCount++;
            stockRequestRec.UpdatedBy = fulfillmentData.CurrentUser;
            stockRequestRec.DateUpdated = currentTime;
            stockRequestRec.Status = fulfillmentData.Status;
            stockRequestRec.QuantityIssued = stockRequestRec.Quantity;
            stockRequestRec.IssueDate = fulfillmentData.CurrentIstTime;
            stockRequestRec.StockLedgerId = 0;
            stockRequestRec.ReviewNotes = fulfillmentData.Notes;

            orm.SaveChanges();

            stockRequestRec = orm.StockRequests.Find(stockRequestRec.Id);
            stockRequestRec.StockLedgerId = stockLedgerRec.Id;
            orm.SaveChanges();

            return DBSaveStatus.Success;
        }

        public static DBSaveStatus PerformStockRequestDenied(StockFulfillmentData fulfillmentData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DateTime currentTime = DateTime.UtcNow;

            // Now create row in Stock Ledger
            DBModel.StockRequest stockRequestRec = orm.StockRequests.Find(fulfillmentData.StockRequestId);
            DBModel.StockRequestTag stockRequestTagRec = orm.StockRequestTags.Find(fulfillmentData.StockRequestTagId);

            if (stockRequestRec == null || stockRequestTagRec == null)
            {
                throw new ArgumentException("Invalid StockRequest or StockRequestTag Id");
            }

            if (stockRequestRec.CyclicCount != fulfillmentData.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            // update stock request Record;
            stockRequestRec.CyclicCount++;
            stockRequestRec.UpdatedBy = fulfillmentData.CurrentUser;
            stockRequestRec.DateUpdated = currentTime;
            stockRequestRec.Status = fulfillmentData.Status;
            stockRequestRec.QuantityIssued = 0;
            stockRequestRec.IssueDate = fulfillmentData.CurrentIstTime;
            stockRequestRec.StockLedgerId = 0;
            stockRequestRec.ReviewNotes = fulfillmentData.Notes;

            orm.SaveChanges();

            return DBSaveStatus.Success;
        }

        public static DBSaveStatus SaveWorkFlowDetail(DomainEntities.EntityWorkFlowDetail inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.EntityWorkFlowDetails.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid EntityWorkFlowDetail Id");
            }

            //sp.EntityWorkFlowId

            sp.PlannedStartDate = inputRec.PlannedFromDate;
            sp.PlannedEndDate = inputRec.PlannedEndDate;
            sp.IsActive = inputRec.IsActive;
            sp.UpdatedBy = inputRec.CurrentUserStaffCode;
            sp.Timestamp = DateTime.UtcNow;
            sp.Notes = inputRec.Notes;

            orm.SaveChanges();

            orm.UpdateWorkflowStatus(sp.EntityWorkFlowId, inputRec.CurrentUserStaffCode);

            return DBSaveStatus.Success;
        }

        public static DBSaveStatus AddWorkFlowDetail(DomainEntities.EntityWorkFlowDetail inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.EntityWorkFlowDetails.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid EntityWorkFlowDetail Id");
            }

            DBModel.EntityWorkFlowDetail newRec = new DBModel.EntityWorkFlowDetail()
            {
                Id = 0,
                EntityWorkFlowId = sp.EntityWorkFlowId,
                Sequence = sp.Sequence,
                TagName = sp.TagName,
                Phase = inputRec.Phase,
                PlannedStartDate = inputRec.PlannedFromDate,
                PlannedEndDate = inputRec.PlannedEndDate,
                PrevPhase = sp.Phase,
                IsFollowUpRow = true,
                PrevPhaseActualDate = sp.ActualDate,
                Notes = inputRec.Notes,
                ParentRowId = sp.Id,
                IsActive = inputRec.IsActive,
                CreatedBy = inputRec.CurrentUserStaffCode,
                UpdatedBy = "",
                DateCreated = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                ActivityId = 0,
                IsComplete = false,
                IsStarted = false,
                MaterialQuantity = 0,
                GapFillingRequired = false,
                GapFillingSeedQuantity = 0,
                LaborCount = 0,
                PercentCompleted = 0,
                BatchId = 0,
                BatchNumber = "",
                LandSize = "",
                DWSEntry = "",
                ItemCount = 0,
                ItemsUsedCount = 0,
                YieldExpected = 0,
                BagsIssued = 0,
                HarvestDate = DateTime.MinValue
            };

            orm.EntityWorkFlowDetails.Add(newRec);
            orm.SaveChanges();

            orm.UpdateWorkflowStatus(sp.EntityWorkFlowId, inputRec.CurrentUserStaffCode);

            return DBSaveStatus.Success;
        }

        // SA /Kartik  - 31-05-2021, Modified Date : 20-08-2021

        public static ICollection<DomainEntities.WorkflowSeason> GetWorkflowSeasonsList()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.WorkflowSeasons.Where(x => x.MaxAgreementsPerEntity > 0)
                .Select(x => new DomainEntities.WorkflowSeason()
                {
                    SeasonName = x.SeasonName,
                }).ToList();
        }

        public static List<VendorSTR> GetSTRTagData()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from st in orm.STRTags
                    join sw in orm.STRWeights on st.STRWeightId equals sw.Id
                    where st.Status == "SiloChecked"
                    select new VendorSTR()
                    {
                        STRTagId = st.Id,
                        STRNumber = st.STRNumber,
                        STRDate = st.STRDate,
                        VehicleNumber = sw.VehicleNumber,
                        NumberOfBags = sw.BagCount,
                        EntryWeight = sw.EntryWeight,
                        ExitWeight = sw.ExitWeight,
                        EndOdo = sw.ExitOdometer
                    }).ToList();
        }

        public static ICollection<VendorSTR> GetSTRData(IEnumerable<long> strTagIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            return (from st in strTagIds
                    join s in orm.STRs on new { STRTagId = st } equals new { s.STRTagId }
                    group s by new
                    {
                        s.STRTagId,
                        //s.VehicleNumber,
                        //s.StartOdometer
                    } into grpSTRs
                    select new VendorSTR()
                    {
                        STRTagId = grpSTRs.Key.STRTagId,
                        //VehicleNumber = grpSTRs.Key.VehicleNumber,
                        StartOdo = grpSTRs.Min(t => t.StartOdometer),
                        NumberOfBags = grpSTRs.Sum(x => x.BagCount),
                        //GrossWeight = grpSTRs.Sum(x => x.GrossWeight)
                        NetWeight = grpSTRs.Sum(x => x.NetWeight)
                    }).ToList();
        }

        public static ICollection<VendorSTR> GetTransporterData(IEnumerable<string> vehicleNumber)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from sd in vehicleNumber
                    join t in orm.Transporters on new { VehicleNo = sd } equals new { t.VehicleNo }
                    select new VendorSTR()
                    {
                        VehicleNumber = t.VehicleNo,
                        VendorName = t.CompanyName,
                        SiloToShedKms = t.SiloToReturnKM,
                        CostPerKm = t.CostPerKm,
                        CostPerExtraTon = t.ExtraCostPerTon,
                        VehicleCapacity = t.VehicleCapacityKgs,
                        HamaliRatePerBag = t.HamaliRatePerBag
                    }).ToList();

        }

        public static ICollection<VendorSTR> GetSeasonNames()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from wfs in orm.WorkflowSeasons
                    join ea in orm.EntityAgreements on wfs.Id equals ea.WorkflowSeasonId
                    join d in orm.DWS on ea.Id equals d.AgreementId
                    join st in orm.STRTags on d.STRTagId equals st.Id
                    where st.Status == "SiloChecked"
                    group st by new
                    {
                        st.Id,
                        wfs.SeasonName
                    } into grpSeason
                    select new VendorSTR()
                    {
                        STRTagId = grpSeason.Key.Id,
                        SeasonName = grpSeason.Key.SeasonName
                    }).ToList();

        }
        public static ICollection<VendorSTR> GetSearchSTRForApproval(VendorSTRFilter searchCriteria)
        {
            var strTagData = DBLayer.GetSTRTagData();
            var strTagIds = strTagData.Select(x => x.STRTagId).Distinct();
            var vehicleNumbers = strTagData.Where(x => x.VehicleNumber != null || x.VehicleNumber != String.Empty).Select(x => x.VehicleNumber).Distinct();
            var strData = DBLayer.GetSTRData(strTagIds);
            //var startOdo = strData.Select(x => x.StartOdo).Distinct();
            var transporterData = DBLayer.GetTransporterData(vehicleNumbers);
            var seasonName = DBLayer.GetSeasonNames();

            EpicCrmEntities orm = DBLayer.GetOrm;

            var strDetails = from st in strTagData
                             join s in strData on st.STRTagId equals s.STRTagId
                             join t in transporterData on st.VehicleNumber equals t.VehicleNumber
                             join wfs in seasonName on st.STRTagId equals wfs.STRTagId
                             join sp in orm.STRPayments on st.STRTagId equals sp.STRTagId into strPayData
                             from spd in strPayData.DefaultIfEmpty()
                             select new VendorSTR()
                             {
                                 STRTagId = st.STRTagId,
                                 STRNumber = st.STRNumber,
                                 STRDate = st.STRDate,
                                 VehicleNumber = st.VehicleNumber,
                                 VendorName = t.VendorName,
                                 //EndOdo = s.EndOdo,
                                 EntryWeight = st.EntryWeight,
                                 ExitWeight = st.ExitWeight,
                                 SeasonName = wfs.SeasonName,

                                 ShedOdo = spd == null ? 0 : spd.ShedToFirstLoadingOdo,
                                 StartOdo = spd == null ? s.StartOdo : (long)spd.StartOdometer,
                                 EndOdo = spd == null ? st.EndOdo : (long)spd.EndOdometer,
                                 SiloToShedKms = spd == null ? t.SiloToShedKms : (int)spd.SiloToReturnKm,
                                 TotalRunningKms = spd == null ? 0 : spd.TotalRunningKms,
                                 CostPerKm = spd == null ? t.CostPerKm : spd.CostPerKm,
                                 TransportationCharges = spd == null ? 0 : spd.TransportationCharges,
                                 VehicleCapacity = spd == null ? t.VehicleCapacity : spd.VehicleCapacityKgs,
                                 //GrossWeight = spd == null ? (decimal)s.GrossWeight : (decimal)spd.GrossWeight,
                                 NetWeight = spd == null ? (decimal)s.NetWeight : (decimal)spd.GrossWeight,
                                 ExtraTonnage = spd == null ? 0 : spd.ExtraTonnage,
                                 CostPerExtraTon = spd == null ? t.CostPerExtraTon : spd.ExtraCostPerTon,
                                 ExtraTonCharges = spd == null ? 0 : spd.ExtraTonCharges,
                                 TollCharges = spd == null ? 0 : spd.TollCharges,
                                 WeighmentCharges = spd == null ? 0 : spd.WeighmentCharges,
                                 Others = spd == null ? 0 : spd.Others,
                                 NumberOfBags = spd == null ? s.NumberOfBags : spd.BagCount,
                                 HamaliRatePerBag = spd == null ? t.HamaliRatePerBag : spd.HamaliRatePerBag,
                                 HamaliCharges = spd == null ? 0 : spd.HamaliCharges,
                                 NetPayableAmount = spd == null ? 0 : spd.NetPayableAmount,
                                 Comments = spd == null ? "" : spd.Comments,
                                 PaymentStatus = spd == null ? "Pending" : spd.Status
                             };

            //strDetails = strDetails.Distinct(strDetails.Select(x => x.STRTagId));
            //strDetails = strDetails.Select(x=>x.STRTagId).Contains(strTagIds);

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

        public static void CreateSTRPayment(VendorSTRPayment strPayData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var recExists = orm.STRPayments.Any(x => x.STRTagId == strPayData.STRTagId);
            if (recExists)
            {
                STRPayment uData = orm.STRPayments.Single(x => x.STRTagId == strPayData.STRTagId);

                uData.ShedToFirstLoadingOdo = strPayData.ShedOdo;
                uData.TotalRunningKms = strPayData.TotalRunningKms;
                uData.CostPerKm = strPayData.CostPerKM;
                uData.TransportationCharges = strPayData.TransportationCharges;
                uData.VehicleCapacityKgs = strPayData.VehicleCapacity;
                uData.ExtraTonnage = strPayData.ExtraTonnage;
                uData.ExtraCostPerTon = strPayData.CostPerExtraTon;
                uData.ExtraTonCharges = strPayData.ExtraTonCharges;
                uData.TollCharges = strPayData.TollCharges;
                uData.WeighmentCharges = strPayData.WeighmentCharges;
                uData.Others = strPayData.Others;
                uData.NetPayableAmount = strPayData.NetPayableAmount;
                uData.Comments = strPayData.Comments;
                uData.Status = strPayData.PaymentStatus;
                uData.UpdatedBy = strPayData.UpdatedBy;
                uData.DateUpdated = strPayData.DateUpdated;

                orm.SaveChanges();
            }
            else
            {
                orm.STRPayments.Add(new DBModel.STRPayment()
                {
                    STRTagId = strPayData.STRTagId,
                    STRNumber = strPayData.STRNumber,
                    ShedToFirstLoadingOdo = strPayData.ShedOdo,
                    StartOdometer = strPayData.StartOdo,
                    EndOdometer = strPayData.EndOdo,
                    SiloToReturnKm = strPayData.SiloToShedKms,
                    TotalRunningKms = strPayData.TotalRunningKms,
                    CostPerKm = strPayData.CostPerKM,
                    TransportationCharges = strPayData.TransportationCharges,
                    VehicleCapacityKgs = strPayData.VehicleCapacity,
                    //GrossWeight = strPayData.GrossWeight,
                    GrossWeight = strPayData.NetWeight,
                    SiloWeight = strPayData.LoadedWeight,
                    ExtraTonnage = strPayData.ExtraTonnage,
                    ExtraCostPerTon = strPayData.CostPerExtraTon,
                    ExtraTonCharges = strPayData.ExtraTonCharges,
                    TollCharges = strPayData.TollCharges,
                    WeighmentCharges = strPayData.WeighmentCharges,
                    BagCount = strPayData.NumberOfBags,
                    HamaliRatePerBag = strPayData.HamaliRatePerBag,
                    HamaliCharges = strPayData.HamaliCharges,
                    Others = strPayData.Others,
                    NetPayableAmount = strPayData.NetPayableAmount,
                    Comments = strPayData.Comments,
                    Status = strPayData.PaymentStatus,
                    CreatedBy = strPayData.CreatedBy,
                    UpdatedBy = strPayData.UpdatedBy,
                    DateCreated = strPayData.DateCreated,
                    DateUpdated = strPayData.DateUpdated
                });
            }

            orm.SaveChanges();
        }

        // Kartik STR Payment Data for Approval
        public static ICollection<VendorSTR> GetSTRPaymentDataForApproval(IEnumerable<long> strTagIds)
        {
            var strPaymentData = DBLayer.GetSTRPaymentsData();
            //var strTagIds = strPaymentData.Select(x => x.STRTagId);
            var vehAndDate = DBLayer.GetDateAndVehicleNumber(strTagIds);
            var vehicleNumber = vehAndDate.Select(x => x.VehicleNumber).Distinct();
            var trData = DBLayer.GetTransporterDataForPayment(vehicleNumber);
            //var seasonName = DBLayer.GetSeasonNamesPayment();

            EpicCrmEntities orm = DBLayer.GetOrm;

            var payRec = (from st in strTagIds
                          join sp in strPaymentData on new { STRTagId = st } equals new { sp.STRTagId }
                          join v in vehAndDate on sp.STRTagId equals v.STRTagId
                          join t in trData on v.VehicleNumber equals t.VehicleNumber
                          select new VendorSTR()
                          {
                              STRTagId = v.STRTagId,
                              AccountName = t.AccountName,
                              BankName = t.BankName,
                              BankBranch = t.BankBranch,
                              AccountNumber = t.AccountNumber,
                              IFSC = t.IFSC,
                              IsActive = t.IsActive,
                          });

            return payRec.ToList();

        }

        public static void MarkSTRAsApprovedAddBankDetails(IEnumerable<VendorSTRPayment> payRecData)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            try
            {
                foreach (var inputRec in payRecData)
                {
                    var dbRec = orm.STRPayments.Where(x => x.STRTagId == inputRec.STRTagId).FirstOrDefault();

                    if (dbRec.Status == STRPaymentStatus.AwaitingApproval.ToString())
                    {
                        dbRec.BankName = inputRec.BankName;
                        dbRec.BankBranch = inputRec.BankBranch;
                        dbRec.BankAccountName = inputRec.AccountHolderName;
                        dbRec.BankAccount = inputRec.BankAccount;
                        dbRec.BankIFSC = inputRec.BankIFSC;
                        dbRec.Status = STRPaymentStatus.Approved.ToString();
                    }

                    orm.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static ICollection<VendorSTR> GetSTRPaymentData()
        {
            var strPaymentData = DBLayer.GetSTRPaymentsData();
            var strTagIds = strPaymentData.Select(x => x.STRTagId);
            var vehAndDate = DBLayer.GetDateAndVehicleNumber(strTagIds);
            var vehicleNumber = vehAndDate.Select(x => x.VehicleNumber).Distinct();
            var trData = DBLayer.GetTransporterDataForPayment(vehicleNumber);
            var seasonName = DBLayer.GetSeasonNamesPayment();

            EpicCrmEntities orm = DBLayer.GetOrm;

            var payRec = (from sp in strPaymentData
                          join v in vehAndDate on sp.STRTagId equals v.STRTagId
                          join t in trData on v.VehicleNumber equals t.VehicleNumber
                          join s in seasonName on sp.STRTagId equals s.STRTagId
                          select new VendorSTR()
                          {
                              STRTagId = sp.STRTagId,
                              STRNumber = sp.STRNumber,
                              STRDate = v.STRDate,
                              ShedOdo = sp.ShedOdo,
                              StartOdo = sp.StartOdo,
                              EndOdo = sp.EndOdo,
                              SiloToShedKms = sp.SiloToShedKms,
                              VehicleNumber = v.VehicleNumber,
                              VendorName = t.VendorName,
                              TotalRunningKms = sp.TotalRunningKms,
                              CostPerKm = sp.CostPerKm,
                              TransportationCharges = sp.TransportationCharges,
                              VehicleCapacity = sp.VehicleCapacity,
                              //GrossWeight = sp.GrossWeight,
                              NetWeight = sp.NetWeight,
                              LoadedWeight = sp.LoadedWeight,
                              ExtraTonnage = sp.ExtraTonnage,
                              CostPerExtraTon = sp.CostPerExtraTon,
                              ExtraTonCharges = sp.ExtraTonCharges,
                              TollCharges = sp.TollCharges,
                              WeighmentCharges = sp.WeighmentCharges,
                              NumberOfBags = sp.NumberOfBags,
                              HamaliRatePerBag = sp.HamaliRatePerBag,
                              HamaliCharges = sp.HamaliCharges,
                              Others = sp.Others,
                              NetPayableAmount = sp.NetPayableAmount,
                              PaymentStatus = sp.PaymentStatus,
                              AccountName = sp.AccountName,
                              BankName = sp.BankName,
                              BankBranch = sp.BankBranch,
                              AccountNumber = sp.AccountNumber,
                              IFSC = sp.IFSC,
                              PaymentReference = sp.PaymentReference,
                              Comments = sp.Comments,
                              SeasonName = s.SeasonName
                          });

            return payRec.ToList();

        }

        public static List<VendorSTR> GetSTRPaymentsData()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from sp in orm.STRPayments
                        //where sp.Status == "Approved"
                    select new VendorSTR
                    {
                        STRTagId = (long)sp.STRTagId,
                        STRNumber = sp.STRNumber,
                        ShedOdo = sp.ShedToFirstLoadingOdo,
                        StartOdo = (long)sp.StartOdometer,
                        EndOdo = (long)sp.EndOdometer,
                        SiloToShedKms = (int)sp.SiloToReturnKm,
                        TotalRunningKms = sp.TotalRunningKms,
                        CostPerKm = sp.CostPerKm,
                        TransportationCharges = sp.TransportationCharges,
                        VehicleCapacity = sp.VehicleCapacityKgs,
                        //GrossWeight = (decimal)sp.GrossWeight,
                        NetWeight = (decimal)sp.GrossWeight,
                        LoadedWeight = (decimal)sp.SiloWeight,
                        ExtraTonnage = sp.ExtraTonnage,
                        CostPerExtraTon = sp.ExtraCostPerTon,
                        ExtraTonCharges = sp.ExtraTonCharges,
                        TollCharges = sp.TollCharges,
                        WeighmentCharges = sp.WeighmentCharges,
                        NumberOfBags = sp.BagCount,
                        HamaliRatePerBag = sp.HamaliRatePerBag,
                        HamaliCharges = sp.HamaliCharges,
                        Others = sp.Others,
                        NetPayableAmount = sp.NetPayableAmount,
                        AccountName = sp.BankAccountName,
                        BankName = sp.BankName,
                        AccountNumber = sp.BankAccount,
                        IFSC = sp.BankIFSC,
                        BankBranch = sp.BankBranch,
                        PaymentStatus = sp.Status,
                        PaymentReference = sp.PaymentReference,
                        Comments = sp.Comments,
                    }).ToList();
        }

        public static List<VendorSTR> GetDateAndVehicleNumber(IEnumerable<long> strTagIds)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from ids in strTagIds
                    join st in orm.STRTags on new { Id = ids } equals new { st.Id }
                    join sw in orm.STRWeights on st.STRWeightId equals sw.Id
                    select new VendorSTR
                    {
                        STRTagId = st.Id,
                        STRDate = st.STRDate,
                        VehicleNumber = sw.VehicleNumber
                    }).ToList();
        }

        public static List<VendorSTR> GetTransporterDataForPayment(IEnumerable<string> vehicleNumber)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from vn in vehicleNumber
                    join t in orm.Transporters on new { VehicleNo = vn } equals new { t.VehicleNo }
                    join vb in orm.TransporterBankDetails on t.CompanyCode equals vb.CompanyCode into vbData
                    from vbd in vbData.DefaultIfEmpty()
                    select new VendorSTR()
                    {
                        TransporterId = t.Id,
                        VehicleNumber = t.VehicleNo,
                        VendorName = t.CompanyName,
                        VehicleCapacity = t.VehicleCapacityKgs,
                        AccountName = vbd == null ? "" : vbd.AccountHolderName,
                        BankName = vbd == null ? "" : vbd.BankName,
                        BankBranch = vbd == null ? "" : vbd.BankBranch,
                        AccountNumber = vbd == null ? "" : vbd.BankAccount,
                        IFSC = vbd == null ? "" : vbd.BankIFSC,
                        IsActive = vbd == null ? false : vbd.IsActive

                    }).ToList();
        }

        public static ICollection<VendorSTR> GetSeasonNamesPayment()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from wfs in orm.WorkflowSeasons
                    join ea in orm.EntityAgreements on wfs.Id equals ea.WorkflowSeasonId
                    join d in orm.DWS on ea.Id equals d.AgreementId
                    join sw in orm.STRPayments on d.STRTagId equals sw.STRTagId
                    group sw by new
                    {
                        sw.STRTagId,
                        wfs.SeasonName
                    } into grpSeason
                    select new VendorSTR()
                    {
                        STRTagId = (long)grpSeason.Key.STRTagId,
                        SeasonName = grpSeason.Key.SeasonName
                    }).ToList();

        }

        public static VendorSTR GetSingleSTRForPayment(long strtagId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.STRPayments.Where(x => x.STRTagId == strtagId)
                .Select(x => new DomainEntities.VendorSTR()
                {
                    STRTagId = (long)x.STRTagId,
                    STRNumber = x.STRNumber,
                    NetPayableAmount = x.NetPayableAmount
                }).FirstOrDefault();
        }

        public static ICollection<VendorSTR> GetSingleSTRDataForTransporter(long strTagId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from e in orm.STRTags
                    join s in orm.STRs on e.Id equals s.STRTagId
                    where e.Id == strTagId
                    group s by new
                    {
                        e.Id
                    } into grpSTR
                    select new VendorSTR()
                    {
                        STRTagId = grpSTR.Key.Id,
                        StartOdo = grpSTR.Min(t => t.StartOdometer),
                        NumberOfBags = grpSTR.Sum(x => x.BagCount),
                        //GrossWeight = grpSTR.Sum(x => x.GrossWeight)
                        NetWeight = grpSTR.Sum(x => x.NetWeight)
                    }).ToList();
        }
        public static ICollection<VendorSTR> GetSingleSTRDetails(long strTagId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var strData = DBLayer.GetSingleSTRDataForTransporter(strTagId);

            var strDetails = from s in strData
                             join st in orm.STRTags on s.STRTagId equals st.Id
                             join sw in orm.STRWeights on st.STRWeightId equals sw.Id
                             join t in orm.Transporters on sw.VehicleNumber equals t.VehicleNo
                             join sp in orm.STRPayments on s.STRTagId equals sp.STRTagId into strPayData
                             from spd in strPayData.DefaultIfEmpty()
                             select new VendorSTR()
                             {
                                 STRTagId = st.Id,
                                 STRNumber = st.STRNumber,
                                 NumberOfBags = s.NumberOfBags,
                                 StartOdo = s.StartOdo,
                                 EndOdo = sw.ExitOdometer,
                                 SiloToShedKms = t.SiloToReturnKM,
                                 //CostPerKm = t.CostPerKm,
                                 //VehicleCapacity = t.VehicleCapacityKgs,
                                 //GrossWeight = s.GrossWeight,
                                 NetWeight = s.NetWeight,
                                 //CostPerExtraTon = t.ExtraCostPerTon,
                                 HamaliRatePerBag = t.HamaliRatePerBag,
                                 EntryWeight = sw.EntryWeight,
                                 ExitWeight = sw.ExitWeight,

                                 CostPerKm = spd == null ? t.CostPerKm : spd.CostPerKm,
                                 VehicleCapacity = spd == null ? t.VehicleCapacityKgs : spd.VehicleCapacityKgs,
                                 CostPerExtraTon = spd == null ? t.ExtraCostPerTon : spd.ExtraCostPerTon,
                                 ShedOdo = spd == null ? 0 : spd.ShedToFirstLoadingOdo,
                                 TollCharges = spd == null ? 0 : spd.TollCharges,
                                 WeighmentCharges = spd == null ? 0 : spd.WeighmentCharges,
                                 Others = spd == null ? 0 : spd.Others,
                                 Comments = spd == null ? "" : spd.Comments
                             };
            return strDetails.ToList();

        }

        public static void CreateSTRPaymentReference(STRPaymentReference spr)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.TransporterPaymentReferences.Add(new TransporterPaymentReference()
            {
                PaymentReference = spr.PaymentReference,
                Comments = spr.Comments,
                NetPayableAmount = spr.TotalNetPayable,
                STRCount = spr.STRCount,
                STRNumber = spr.STRNumber,

                CreatedBy = spr.CurrentUser,
                UpdatedBy = spr.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,

                AccountNumber = spr.AccountNumber,
                AccountName = spr.AccountName,
                AccountAddress = spr.AccountAddress,
                AccountEmail = spr.AccountEmail,
                PaymentType = spr.PaymentType,
                SenderInfo = spr.SenderInfo,
                LocalTimeStamp = spr.LocalTimeStamp
            });

            orm.SaveChanges();

        }

        public static DBSaveStatus MarkSTRAsPaid(IEnumerable<VendorSTR> strRecs)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            foreach (var inputRec in strRecs)
            {
                var dbRec = orm.STRPayments.Where(x => x.STRTagId == inputRec.STRTagId).FirstOrDefault();
                if (dbRec == null)
                {
                    return DBSaveStatus.RecordNotFound;
                }

                dbRec.PaymentReference = inputRec.PaymentReference;
                dbRec.DateUpdated = DateTime.UtcNow;
                dbRec.UpdatedBy = inputRec.CurrentUser;
                dbRec.Status = inputRec.PaymentStatus;
            }

            orm.SaveChanges();
            return DBSaveStatus.Success;
        }

        /// <summary>
        /// Author:Pankaj K; Date:25/04/2021; Purpose:Upload Dealer Questionnaire;
        /// Saving Questionnaire in DB
        /// </summary>
        /// <param name="inputObject"></param>
        /// <returns></returns>
        public static bool SaveQuestionnaire(DomainEntities.Questionnaire.QuestionPaper inputObject)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            DBModel.QuestionPaper qp = new DBModel.QuestionPaper()
            {
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Name = inputObject.Name,
                EntityType = inputObject.EntityType,
                QuestionCount = inputObject.QuestionCount,
                IsActive = true
            };

            if (inputObject.QuestionCount > 0)
            {
                // add questions
                foreach (var q in inputObject.Questions)
                {
                    DBModel.QuestionPaperQuestion qpq = new DBModel.QuestionPaperQuestion()
                    {
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.UtcNow,
                        CategoryName = q.CategoryName,
                        CategoryDesc = q.CategoryDesc,
                        SubCategoryName = q.SubCategoryName,
                        SubCategoryDesc = q.SubCategoryDesc,
                        QText = q.QText,
                        AdditionalComment = q.AdditionalComment,
                        DisplaySequence = q.DisplaySequence,
                        QuestionTypeName = q.QuestionTypeName,
                    };

                    // add answer choices to question
                    foreach (var ans in q.AnswerChoices)
                    {
                        qpq.QuestionPaperAnswers.Add(
                                        new DBModel.QuestionPaperAnswer()
                                        {
                                            AText = ans.AText
                                        }
                        );
                    }
                    qp.QuestionPaperQuestions.Add(qpq);
                }
            }

            orm.QuestionPapers.Add(qp);
            orm.SaveChanges();
            return true;
        }

        //Author - SA; Date:20/05/2021; Purpose: Get vendor payment details
        public static ICollection<STRPaymentReference> GetSTRPaymentReferences(PaymentReferenceFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<STRPaymentReference> vprData = from e in orm.TransporterPaymentReferences
                                                      select new STRPaymentReference
                                                      {
                                                          Id = e.Id,
                                                          PaymentReference = e.PaymentReference,
                                                          STRCount = e.STRCount,
                                                          STRNumber = e.STRNumber,
                                                          Comments = e.Comments,
                                                          TotalNetPayable = (decimal)e.NetPayableAmount,
                                                          DateCreated = e.DateCreated,
                                                          CreatedBy = e.CreatedBy,

                                                          AccountNumber = e.AccountNumber,
                                                          AccountName = e.AccountName,
                                                          AccountAddress = e.AccountAddress,
                                                          AccountEmail = e.AccountEmail,
                                                          PaymentType = e.PaymentType,
                                                          SenderInfo = e.SenderInfo,
                                                          LocalTimeStamp = e.LocalTimeStamp
                                                      };

            if (searchCriteria.ApplyPaymentReferenceFilter)
            {
                if (searchCriteria.IsExactReferenceMatch)
                {
                    vprData = vprData.Where(x => x.PaymentReference == searchCriteria.PaymentReference);
                }
                else
                {
                    vprData = vprData.Where(x => x.PaymentReference.Contains(searchCriteria.PaymentReference));
                }
            }

            if (searchCriteria.ApplyDateFilter)
            {
                searchCriteria.DateTo = searchCriteria.DateTo.AddDays(1);
                // this is because - LocalTimeStamp has time as well, whereas DateTo is only date
                // so when we search for dates upto 2020-05-28, records created at time
                // 2020-05-28 17:00:00 should also get included.
                vprData = vprData.Where(x => x.LocalTimeStamp >= searchCriteria.DateFrom &&
                                                             x.LocalTimeStamp < searchCriteria.DateTo);
            }

            return vprData.ToList();
        }

        /// <summary>
        /// Author: Rajesh V; Date24-06-2021 ; Purpose: dealer Questionnaire
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="batchId"></param>
        /// <param name="domainQuestionnaire"></param>
        public static void SaveSqliteQuestionnaireDataWrapper(long employeeId, long batchId,
                                                                    IEnumerable<SqliteDomainQuestionnaire> domainQuestionnaire)
        {
            try
            {

                DBLayer.SaveSqliteQuestionnaireData(employeeId, batchId, domainQuestionnaire);
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorText = GetErrorTextFromValidationException(dbEx);
                string logSnip = $"Questionnaire Target Data not saved for EmployeeId: {employeeId}; BatchId: {batchId};";
                LogError($"{nameof(SaveSqliteQuestionnaireData)}", errorText, logSnip);
            }
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
            int rowsSaved = 0;
            long dupPhoneDbId = 0;
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                foreach (var data in domainQuestionnaire)
                {
                    var rec = orm.SqliteQuestionnaires
                                 .FirstOrDefault(x => x.EmployeeId == employeeId && x.PhoneDbId == data.PhoneDbId);

                    dupPhoneDbId = rec?.Id ?? 0;
                    if (dupPhoneDbId == 0)
                    {
                        orm.SqliteQuestionnaires.Add(
                            new SqliteQuestionnaire()
                            {
                                BatchId = batchId,
                                EmployeeId = employeeId,
                                IsNewEntity = data.IsNewEntity,
                                EntityId = data.EntityId,
                                EntityName = data.EntityName,
                                QuestionnaireDate = data.QuestionnaireDate,
                                SqliteQuestionPaperId = data.SqliteQuestionPaperId,
                                SqliteQuestionPaperName = data.SqliteQuestionPaperName,
                                ActivityId = data.ActivityId,
                                ParentReferenceId = data.ParentReferenceId,
                                //CustomerQuestionnaireId = data.CustomerQuestionnaireId,
                                PhoneDbId = data.PhoneDbId,
                                IsProcessed = false,
                                DateCreated = data.DateCreated,
                                DateUpdated = DateTime.UtcNow,
                            });
                        if (data.Answers != null)
                        {
                            foreach (var item in data.Answers.ToList())
                            {
                                if (item.QuestionTypeName.ToLower() != "descriptive")
                                {
                                    orm.SqliteAnswers.Add(
                                    new SqliteAnswer()
                                    {
                                        Id = item.Id,
                                        QuestionPaperQuestionId = item.QuestionPaperQuestionId,
                                        HasTextComment = item.HasTextComment,
                                        TextComment = item.TextComment
                                    });

                                    if (item.DomainAnswerDetail != null)
                                    {
                                        foreach (var ansDetail in item.DomainAnswerDetail.ToList())
                                        {
                                            if (ansDetail.IsAnswerChecked.ToLower() == "true")
                                            {
                                                orm.SqliteAnswerDetails.Add(
                                                new SqliteAnswerDetail()
                                                {
                                                    Id = ansDetail.Id,
                                                    AnswerId = ansDetail.AnswerId,
                                                    SqliteQuestionPaperQuestionId = ansDetail.SqliteQuestionPaperQuestionId,
                                                    SqliteQuestionPaperAnswerId = ansDetail.SqliteQuestionPaperAnswerId
                                                });
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    orm.SqliteAnswers.Add(
                                    new SqliteAnswer()
                                    {
                                        Id = item.Id,
                                        QuestionPaperQuestionId = item.QuestionPaperQuestionId,
                                        HasTextComment = item.HasTextComment,
                                        TextComment = item.DescriptiveAnswer
                                    });
                                }
                            }
                        }
                        orm.SaveChanges();
                        rowsSaved += 1;
                    }
                    else
                    {
                        string errorText = "PhoneDbId Duplication";
                        string logSnip = $"Questionnaire Data not saved for EmployeeId: {employeeId}; PhoneDbId: {data.PhoneDbId};";
                        LogError($"{nameof(SaveSqliteQuestionnaireData)}", errorText, logSnip);
                    }
                }
            }

            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                // get batch record and update it
                SqliteActionBatch batchRecord = orm.SqliteActionBatches.Where(x => x.Id == batchId).FirstOrDefault();

                // now update the rows added to the batch record
                batchRecord.QuestionnaireTargetSaved = rowsSaved;
                batchRecord.Timestamp = DateTime.UtcNow;
                orm.SaveChanges();
            }
        }

        /// <summary>
        ///  Author:Ajith, Purpose:Dealer Questionnaire search in table,Dated :12/06/2021
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static IEnumerable<DomainEntities.Questionnaire.QuestionPaper> GetQuestionpaper()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.QuestionPapers.AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new DomainEntities.Questionnaire.QuestionPaper()
                {
                    Name = x.Name,
                    Id = x.Id
                }).ToList();
        }

        /// <summary>
        ///  Author:Ajith, Purpose:Dealer Questionnaire search in table
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static ICollection<DomainEntities.Questionnaire.CustomerQuestionnaire> GetCustomerQuestionnaire(SearchCriteria searchCriteria)
        {
            try
            {

                EpicCrmEntities orm = DBLayer.GetOrm;

                IQueryable<DomainEntities.Questionnaire.CustomerQuestionnaire> Questionnaires =
                    from o in orm.CustomerQuestionnaires
                    join c in orm.Activities on o.ActivityId equals c.Id into temp
                    from t in temp.DefaultIfEmpty()
                    join te in orm.TenantEmployees on o.EmployeeId equals te.Id into temp1
                    from t1 in temp1.DefaultIfEmpty()
                    join sp in orm.SalesPersons on t1.EmployeeCode equals sp.StaffCode into temp2
                    from t2 in temp2.DefaultIfEmpty()
                    select new DomainEntities.Questionnaire.CustomerQuestionnaire()
                    {

                        Id = o.Id,
                        EmployeeCode = t1.EmployeeCode,
                        EmployeeId = o.EmployeeId,
                        EmployeeName = t1.Name,
                        CustomerCode = o.CustomerCode,
                        QuestionPaperId = (long)o.QuestionPaperId,
                        QuestionPaperName = o.QuestionPaperName,
                        ActivityId = o.ActivityId,
                        DateCreated = o.DateCreated,
                        DateUpdated = o.DateUpdated,
                        CreatedBy = o.CreatedBy,
                        UpdatedBy = o.UpdatedBy,
                        EntityName = t.ClientName,
                        EntityType = t.ClientType,
                        HQCode = t2.HQCode,
                        IsActive = t1.IsActive,

                        IsActiveInSap = (t1 != null) ? t1.IsActive : false
                    };

                if (searchCriteria.ApplyOrderIdFilter)
                {
                    Questionnaires = Questionnaires.Where(x => x.Id == searchCriteria.Id);
                }
                else
                {
                    if (searchCriteria.ApplyDateFilter)
                    {
                        // DateCreated in CustomerQuestionnaire has time - so we want to include all records for upto dateto
                        searchCriteria.DateTo = searchCriteria.DateTo.AddDays(1);
                        Questionnaires = Questionnaires.Where(x => x.DateCreated >= searchCriteria.DateFrom && x.DateCreated < searchCriteria.DateTo);

                    }

                    if (searchCriteria.ApplyEmployeeCodeFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
                    }
                    if (searchCriteria.ApplyEmployeeNameFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
                    }
                    if (searchCriteria.ApplyEmployeeStatusFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
                    }
                    if (searchCriteria.ApplyClientNameFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
                    }

                    if (searchCriteria.ApplyClientTypeFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EntityType == searchCriteria.ClientType);
                    }

                    if (searchCriteria.ApplyClientTypeFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EntityType == searchCriteria.ClientType);
                    }

                    if (searchCriteria.ApplyQuestionnaireFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.QuestionPaperId == searchCriteria.QuestionPaperId);
                    }

                    var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
                    if (hqList != null)
                    {
                        Questionnaires = Questionnaires.Where(x => hqList.Any(y => y == x.HQCode));
                    }

                    if (!searchCriteria.IsSuperAdmin)
                    {
                        var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                        Questionnaires = Questionnaires.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
                    }

                }
                return Questionnaires.OrderByDescending(x => x.DateCreated).ThenBy(x => x.EmployeeName).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Author:Ajith, Purpose:Dealer Questionnaire Question & Answers on Id
        /// </summary>
        /// <param name="QuestionnaireID"></param>
        /// <returns></returns>
        public static IEnumerable<DomainEntities.Questionnaire.CustomerQuestionnairedetails> GetCustomerQuestionnairedetails(long QuestionnaireID)
        {
            try
            {

                EpicCrmEntities orm = DBLayer.GetOrm;

                IQueryable<DomainEntities.Questionnaire.CustomerQuestionnairedetails> Questionnaires =
                    from o in orm.CustomerQuestionnaires

                    join e in orm.QuestionPapers on o.QuestionPaperId equals e.Id into temp2
                    from t2 in temp2.DefaultIfEmpty()
                    join f in orm.QuestionPaperQuestions on o.QuestionPaperId equals f.QuestionPaperId into temp3
                    from t3 in temp3.DefaultIfEmpty()
                    join g in orm.QuestionPaperAnswers on t3.Id equals g.QuestionPaperQuestionId into temp4
                    from t4 in temp4.DefaultIfEmpty()
                    join te in orm.TenantEmployees on o.EmployeeId equals te.Id into temp5
                    from t5 in temp5.DefaultIfEmpty()
                    join h in orm.Activities on o.ActivityId equals h.Id into temp6
                    from t6 in temp6.DefaultIfEmpty()
                    join c in orm.Answers on new { CQ_Id = o.Id, QPA_ID = t3.Id } equals new { CQ_Id = c.CrossRefId, QPA_ID = c.QuestionPaperQuestionId } into temp
                    from t in temp.DefaultIfEmpty()

                    where o.Id == QuestionnaireID

                    select new DomainEntities.Questionnaire.CustomerQuestionnairedetails()
                    {
                        Id = o.Id,
                        EmployeeCode = t5.EmployeeCode,
                        EmployeeName = t5.Name,
                        QuestionPaperName = o.QuestionPaperName,
                        TextComment = (t.TextComment != null) ? t.TextComment : "",
                        HasTextComment = (t.HasTextComment == true) ? t.HasTextComment : false,
                        DisplaySequence = t3.DisplaySequence,
                        EntityName = t6.ClientName,
                        EntityType = t6.ClientType,
                        QuestionPaperQuestionId = (t3.Id != 0) ? t3.Id : 0,
                        QuestionPaperAnswerId = (t3.QuestionTypeName != "Descriptive") ? t4.Id : 0,
                        QText = t3.QText,
                        AText = (t4.AText != null) ? t4.AText : "",
                        QuestionTypeName = t3.QuestionTypeName,
                        DateCreated = o.DateCreated,
                        ActivityId = (o.ActivityId != 0) ? o.ActivityId : 0,
                        CustomerCode = o.CustomerCode
                    };

                return Questionnaires.OrderBy(x => x.DisplaySequence).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Author:Ajith, Purpose:Dealer Questionnaire Answerdetails on Id
        /// </summary>
        /// <param name="QuestionnaireID"></param>
        /// <returns></returns>

        public static IEnumerable<DomainEntities.Questionnaire.CustomerQuestionnairedetails> GetCustomerQuestionnaireQA(long QuestionnaireID)
        {
            try
            {
                EpicCrmEntities orm = DBLayer.GetOrm;

                IQueryable<DomainEntities.Questionnaire.CustomerQuestionnairedetails> Questionnaires =
                    from o in orm.CustomerQuestionnaires
                    join c in orm.Answers on new { CQ_Id = o.Id } equals new { CQ_Id = c.CrossRefId } into temp
                    from t in temp.DefaultIfEmpty()
                    join d in orm.AnswerDetails on new { QPQ_Id = t.Id } equals new { QPQ_Id = d.AnswerId } into temp1
                    from t1 in temp1.DefaultIfEmpty()
                    where o.Id == QuestionnaireID

                    select new DomainEntities.Questionnaire.CustomerQuestionnairedetails()
                    {
                        QuestionPaperQuestionId = t.QuestionPaperQuestionId,
                        QuestionPaperAnswerId = (t1.QuestionPaperAnswerId != null) ? t1.QuestionPaperAnswerId : 0,
                    };

                return Questionnaires.ToList();
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        /// <summary>
        /// Author:Ajith, Purpose:For Customerquestionnaire excel download data
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static IEnumerable<DomainEntities.Questionnaire.CustomerQuestionnairedetails> GetCustomerQuestionnaireAlldetails(SearchCriteria searchCriteria)
        {
            try
            {
                EpicCrmEntities orm = DBLayer.GetOrm;

                IQueryable<DomainEntities.Questionnaire.CustomerQuestionnairedetails> Questionnaires =
                    from o in orm.CustomerQuestionnaires
                    join f in orm.QuestionPaperQuestions on o.QuestionPaperId equals f.QuestionPaperId into temp3
                    from t3 in temp3.DefaultIfEmpty()
                    join A in orm.Answers on new { CQ_Id = o.Id, QPQ_Id = t3.Id } equals new { CQ_Id = A.CrossRefId, QPQ_Id = A.QuestionPaperQuestionId } into temp6
                    from t6 in temp6.DefaultIfEmpty()
                    join d in orm.AnswerDetails on new { QPQ_Id = t6.Id } equals new { QPQ_Id = d.AnswerId } into temp7
                    from t7 in temp7.DefaultIfEmpty()
                    join e in orm.QuestionPapers on o.QuestionPaperId equals e.Id into temp2
                    from t2 in temp2.DefaultIfEmpty()
                    join g in orm.QuestionPaperAnswers on t7.QuestionPaperAnswerId equals g.Id into temp4
                    from t4 in temp4.DefaultIfEmpty()
                    join c in orm.Activities on o.ActivityId equals c.Id into temp
                    from t in temp.DefaultIfEmpty()
                    join te in orm.TenantEmployees on o.EmployeeId equals te.Id into temp1
                    from t1 in temp1.DefaultIfEmpty()
                    join sp in orm.SalesPersons on t1.EmployeeCode equals sp.StaffCode into temp5
                    from t5 in temp5.DefaultIfEmpty()

                    select new DomainEntities.Questionnaire.CustomerQuestionnairedetails()
                    {
                        Id = o.Id,
                        EmployeeCode = t1.EmployeeCode,
                        EmployeeId = o.EmployeeId,
                        EmployeeName = t1.Name,
                        CustomerCode = o.CustomerCode,
                        QuestionPaperId = (long)o.QuestionPaperId,
                        QuestionPaperName = o.QuestionPaperName,
                        ActivityId = o.ActivityId,
                        DateCreated = o.DateCreated,
                        DateUpdated = o.DateUpdated,
                        CreatedBy = o.CreatedBy,
                        UpdatedBy = o.UpdatedBy,
                        QText = t3.QText,
                        AText = (t4.AText != null) ? t4.AText : "",
                        EntityName = t.ClientName,
                        EntityType = t.ClientType,
                        HQCode = t5.HQCode,
                        IsActive = t1.IsActive,
                        IsActiveInSap = (t1 != null) ? t1.IsActive : false,
                        TextComment = t6.TextComment
                    };

                if (searchCriteria.ApplyOrderIdFilter)
                {
                    Questionnaires = Questionnaires.Where(x => x.Id == searchCriteria.Id);
                }
                else
                {
                    if (searchCriteria.ApplyDateFilter)
                    {
                        // DateCreated in CustomerQuestionnaire has time - so we want to include all records for upto dateto
                        searchCriteria.DateTo = searchCriteria.DateTo.AddDays(1);
                        Questionnaires = Questionnaires.Where(x => x.DateCreated >= searchCriteria.DateFrom && x.DateCreated < searchCriteria.DateTo);
                    }
                    if (searchCriteria.ApplyEmployeeCodeFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
                    }
                    if (searchCriteria.ApplyEmployeeNameFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
                    }
                    if (searchCriteria.ApplyEmployeeStatusFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => searchCriteria.EmployeeStatus == (x.IsActive && x.IsActiveInSap));
                    }
                    if (searchCriteria.ApplyClientNameFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
                    }

                    if (searchCriteria.ApplyClientTypeFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EntityType == searchCriteria.ClientType);
                    }

                    if (searchCriteria.ApplyClientTypeFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.EntityType == searchCriteria.ClientType);
                    }

                    if (searchCriteria.ApplyQuestionnaireFilter)
                    {
                        Questionnaires = Questionnaires.Where(x => x.QuestionPaperId == searchCriteria.QuestionPaperId);
                    }

                    var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
                    if (hqList != null)
                    {
                        Questionnaires = Questionnaires.Where(x => hqList.Any(y => y == x.HQCode));
                    }

                    if (!searchCriteria.IsSuperAdmin)
                    {
                        var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                        Questionnaires = Questionnaires.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
                    }
                }
                return Questionnaires.ToList().GroupBy(x => new
                {
                    Id = x.Id,
                    DateCreated = x.DateCreated,
                    EmployeeCode = x.EmployeeCode,
                    EmployeeName = x.EmployeeName,
                    CustomerCode = x.CustomerCode,
                    EntityType = x.EntityType,
                    EntityName = x.EntityName,
                    QText = x.QText,
                    QuestionPaperName = x.QuestionPaperName,
                    TextComment = x.TextComment
                }).Select(z => new DomainEntities.Questionnaire.CustomerQuestionnairedetails()
                {
                    Id = z.Key.Id,
                    DateCreated = z.Key.DateCreated,
                    EmployeeCode = z.Key.EmployeeCode,
                    EmployeeName = z.Key.EmployeeName,
                    CustomerCode = z.Key.CustomerCode,
                    EntityType = z.Key.EntityType,
                    EntityName = z.Key.EntityName,
                    QText = z.Key.QText,
                    QuestionPaperName = z.Key.QuestionPaperName,
                    TextComment = z.Key.TextComment,
                    AText = String.Join("|", z.Select(v => v.AText).Distinct()),
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ICollection<BonusCalculation> GetSeasonNamesBonus()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from wfs in orm.WorkflowSeasons
                    join ea in orm.EntityAgreements on wfs.Id equals ea.WorkflowSeasonId

                    group ea by new
                    {
                        wfs.Id,
                        wfs.SeasonName
                    } into grpSeason
                    select new BonusCalculation()
                    {
                        EntityId = grpSeason.Key.Id,
                        SeasonName = grpSeason.Key.SeasonName
                    }).ToList();

        }

        /// <summary>
        /// Author:Ajith/Rajesh, Purpose:For Bonus Calculation Pending status Search on 22/07/2021
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public static IEnumerable<DomainEntities.BonusCalculation> GetPendingBonusAgreement(BonusCalculationFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.BonusCalculation> BonusAgreements =
                from ea in orm.EntityAgreements
                join d in orm.DWS on ea.Id equals d.AgreementId
                join e in orm.Entities on ea.EntityId equals e.Id
                join wf in orm.WorkflowSeasons on ea.WorkflowSeasonId equals wf.Id into temp
                from tb in temp.DefaultIfEmpty()
                where ea.BonusProcessed == false

                select new DomainEntities.BonusCalculation()
                {
                    AgreementDate = ea.DateCreated,
                    AgreementNumber = ea.AgreementNumber,
                    TypeName = d.TypeName,
                    EntityName = e.EntityName,
                    LandSizeInAcres = (ea.LandSizeInAcres != 0) ? ea.LandSizeInAcres : 0,
                    RatePerKg = (ea.RatePerKg != 0) ? ea.RatePerKg : 0,
                    NetPayableWt = (d.NetPayableWt != 0) ? d.NetPayableWt : 0,
                    NetPayable = (d.NetPayable != 0) ? d.NetPayable : 0,
                    HQCode = e.HQCode,
                    SeasonName = tb.SeasonName,
                    SeasonId = tb.Id,
                    AgreementStatus = ea.Status,
                    DWSStatus = d.Status,
                    AgreementId = ea.Id,
                    BonusStatus = d.Status
                };

            if (searchCriteria.ApplyDateFilter)
            {
                searchCriteria.DateTo = searchCriteria.DateTo.AddDays(1);
                BonusAgreements = BonusAgreements.Where(x => x.AgreementDate >= searchCriteria.DateFrom && x.AgreementDate < searchCriteria.DateTo);
            }

            if (searchCriteria.ApplyClientNameFilter)
            {
                BonusAgreements = BonusAgreements.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyAgreementNumberFilter)
            {
                BonusAgreements = BonusAgreements.Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
            }

            if (searchCriteria.ApplySeasonNameFilter)
            {
                if (searchCriteria.SeasonName != "All")
                {
                    BonusAgreements = BonusAgreements.Where(x => x.SeasonName.Equals(searchCriteria.SeasonName));
                }
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                BonusAgreements = BonusAgreements.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                BonusAgreements = BonusAgreements.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            BonusAgreements = BonusAgreements.Where(x => x.AgreementStatus == "Approved");

            var BonusAgreementCollection = new List<BonusCalculation>();
            var excludeAgreeIdList = new List<long>();

            foreach (var item in BonusAgreements)
            {
                if (item.DWSStatus == "Paid")
                {
                    BonusAgreementCollection.Add(item);
                }
                else
                {
                    excludeAgreeIdList.Add(item.AgreementId);
                }

            }
            var execludeDistAgreetId = excludeAgreeIdList.Distinct();
            BonusAgreementCollection.RemoveAll(x => execludeDistAgreetId.Any(a => a == x.AgreementId));

            BonusAgreementCollection = BonusAgreementCollection.GroupBy(x => x.AgreementId).Select(y => new BonusCalculation
            {
                AgreementId = y.Key,
                AgreementDate = y.FirstOrDefault().AgreementDate,
                AgreementNumber = y.FirstOrDefault().AgreementNumber,
                TypeName = y.FirstOrDefault().TypeName,
                EntityName = y.FirstOrDefault().EntityName,
                LandSizeInAcres = y.FirstOrDefault().LandSizeInAcres,
                RatePerKg = y.FirstOrDefault().RatePerKg,
                HQCode = y.FirstOrDefault().HQCode,
                SeasonName = y.FirstOrDefault().SeasonName,
                SeasonId = y.FirstOrDefault().SeasonId,
                BonusStatus = searchCriteria.BonusStatus,
                NetPayableWt = y.Sum(z => z.NetPayableWt),
                NetPayable = y.Sum(z => z.NetPayable)
            }).ToList();
            return BonusAgreementCollection;
        }

        /// Author:Rajesh V, Purpose:For Bonus Calculation Awaiting status Search, on 02/08/2021
        public static IEnumerable<DomainEntities.BonusCalculation> GetBonusAgreement(BonusCalculationFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.BonusCalculation> BonusAgreements = orm.BonusAgreementDetails.Where(x => x.BonusStatus == searchCriteria.BonusStatus)
                .Select(x => new BonusCalculation()
                {
                    AgreementDate = x.AgreementDate,
                    AgreementNumber = x.AgreementNumber,
                    TypeName = x.TypeName,
                    EntityName = x.EntityName,
                    EntityId = x.EntityId,
                    LandSizeInAcres = x.LandSizeInAcres,
                    RatePerKg = x.RatePerKg,
                    BonusRate = x.BonusRate,
                    NetPayableWt = x.NetWeight,
                    NetPayable = x.NetPaid,
                    BonusAmountPayable = x.BonusAmountPayable,
                    BonusAmountPaid = x.BonusAmountPaid,
                    Comments = x.Comments,
                    AccountHolderName = x.BankAccountName,
                    BankName = x.BankName,
                    BankAccount = x.BankAccountNumber,
                    BankIFSC = x.BankIFSC,
                    BankBranch = x.BankBranch,
                    HQCode = x.HQCode,
                    SeasonName = x.SeasonName,
                    AgreementId = x.AgreementId,
                    BonusStatus = x.BonusStatus,
                    PaymentReference = x.PaymentReference
                });

            if (searchCriteria.ApplyDateFilter)
            {
                searchCriteria.DateTo = searchCriteria.DateTo.AddDays(1);
                BonusAgreements = BonusAgreements.Where(x => x.AgreementDate >= searchCriteria.DateFrom && x.AgreementDate < searchCriteria.DateTo);
            }

            if (searchCriteria.ApplyClientNameFilter)
            {
                BonusAgreements = BonusAgreements.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyAgreementNumberFilter)
            {
                BonusAgreements = BonusAgreements.Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
            }

            if (searchCriteria.ApplySeasonNameFilter)
            {
                if (searchCriteria.SeasonName != "All")
                {
                    BonusAgreements = BonusAgreements.Where(x => x.SeasonName.Equals(searchCriteria.SeasonName));
                }
            }

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                BonusAgreements = BonusAgreements.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                BonusAgreements = BonusAgreements.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            return BonusAgreements.ToList();
        }

        public static bool BonusAgreementCheck(string SeasonName, string TypeName, decimal YieldPerAcre)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.BonusRates.Any(x => x.SeasonName == SeasonName && x.TypeName == TypeName && x.WeightTonsFrom <= YieldPerAcre && x.WeightTonsTo >= YieldPerAcre);
        }

        /// <summary>
        /// Author:Rajesh, Purpose:Get details of Single Bonus Agreement to display in popup on 30/07/2021
        /// </summary>
        /// <param name="agreeId"></param>
        /// <returns></returns>
        public static IEnumerable<BonusCalculation> GetSingleBonusDetails(long agreeId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            bool agreementProcessed = orm.EntityAgreements.Any(x => x.Id == agreeId && x.BonusProcessed == true);

            if (agreementProcessed)
            {
                IQueryable<DomainEntities.BonusCalculation> BonusAgreements =
                from ea in orm.EntityAgreements
                join d in orm.DWS on ea.Id equals d.AgreementId
                join e in orm.Entities on ea.EntityId equals e.Id
                join wf in orm.WorkflowSeasons on ea.WorkflowSeasonId equals wf.Id
                join bad in orm.BonusAgreementDetails on ea.Id equals bad.AgreementId into temp
                from tb in temp.DefaultIfEmpty()
                where ea.Id == agreeId

                select new DomainEntities.BonusCalculation()
                {
                    AgreementId = ea.Id,
                    EntityId = e.Id,
                    AgreementDate = ea.DateCreated,
                    AgreementNumber = ea.AgreementNumber,
                    TypeName = d.TypeName,
                    RatePerKg = ea.RatePerKg,
                    EntityName = e.EntityName,
                    LandSizeInAcres = (ea.LandSizeInAcres != 0) ? ea.LandSizeInAcres : 0,
                    SeasonName = wf.SeasonName,
                    BonusAmountPayable = tb.BonusAmountPayable,
                    BonusAmountPaid = tb.BonusAmountPaid,
                    Comments = tb.Comments,
                    HQCode = e.HQCode,
                    NetPayableWt = (d.NetPayableWt != 0) ? d.NetPayableWt : 0,
                    NetPayable = (d.NetPayable != 0) ? d.NetPayable : 0
                };

                return BonusAgreements.GroupBy(x => x.AgreementId).Select(y => new BonusCalculation
                {
                    AgreementId = y.Key,
                    EntityId = y.FirstOrDefault().EntityId,
                    AgreementDate = y.FirstOrDefault().AgreementDate,
                    AgreementNumber = y.FirstOrDefault().AgreementNumber,
                    TypeName = y.FirstOrDefault().TypeName,
                    RatePerKg = y.FirstOrDefault().RatePerKg,
                    EntityName = y.FirstOrDefault().EntityName,
                    LandSizeInAcres = y.FirstOrDefault().LandSizeInAcres,
                    SeasonName = y.FirstOrDefault().SeasonName,
                    BonusAmountPayable = y.FirstOrDefault().BonusAmountPayable,
                    BonusAmountPaid = y.FirstOrDefault().BonusAmountPaid,
                    Comments = y.FirstOrDefault().Comments,
                    HQCode = y.FirstOrDefault().HQCode,
                    NetPayableWt = y.Sum(z => z.NetPayableWt),
                    NetPayable = y.Sum(z => z.NetPayable)

                }).ToList();
            }
            else
            {
                IQueryable<DomainEntities.BonusCalculation> BonusAgreements =
                from ea in orm.EntityAgreements
                join d in orm.DWS on ea.Id equals d.AgreementId
                join e in orm.Entities on ea.EntityId equals e.Id
                join wf in orm.WorkflowSeasons on ea.WorkflowSeasonId equals wf.Id into temp
                from tb in temp.DefaultIfEmpty()
                where ea.Id == agreeId

                select new DomainEntities.BonusCalculation()
                {
                    AgreementId = ea.Id,
                    EntityId = e.Id,
                    AgreementDate = ea.DateCreated,
                    AgreementNumber = ea.AgreementNumber,
                    TypeName = d.TypeName,
                    RatePerKg = ea.RatePerKg,
                    EntityName = e.EntityName,
                    LandSizeInAcres = (ea.LandSizeInAcres != 0) ? ea.LandSizeInAcres : 0,
                    SeasonName = tb.SeasonName,
                    BonusAmountPayable = 0,
                    BonusAmountPaid = 0,
                    Comments = null,
                    HQCode = e.HQCode,
                    NetPayableWt = (d.NetPayableWt != 0) ? d.NetPayableWt : 0,
                    NetPayable = (d.NetPayable != 0) ? d.NetPayable : 0
                };

                return BonusAgreements.GroupBy(x => x.AgreementId).Select(y => new BonusCalculation
                {
                    AgreementId = y.Key,
                    EntityId = y.FirstOrDefault().EntityId,
                    AgreementDate = y.FirstOrDefault().AgreementDate,
                    AgreementNumber = y.FirstOrDefault().AgreementNumber,
                    TypeName = y.FirstOrDefault().TypeName,
                    RatePerKg = y.FirstOrDefault().RatePerKg,
                    EntityName = y.FirstOrDefault().EntityName,
                    LandSizeInAcres = y.FirstOrDefault().LandSizeInAcres,
                    SeasonName = y.FirstOrDefault().SeasonName,
                    BonusAmountPayable = y.FirstOrDefault().BonusAmountPayable,
                    BonusAmountPaid = y.FirstOrDefault().BonusAmountPaid,
                    Comments = y.FirstOrDefault().Comments,
                    HQCode = y.FirstOrDefault().HQCode,
                    NetPayableWt = y.Sum(z => z.NetPayableWt),
                    NetPayable = y.Sum(z => z.NetPayable)

                }).ToList();

            }
        }

        public static decimal GetBonusRate(string SeasonName, string TypeName, decimal yieldPerAcre)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var bonusRate = (from br in orm.BonusRates
                             where br.SeasonName == SeasonName && br.TypeName == TypeName && br.WeightTonsFrom <= yieldPerAcre && br.WeightTonsTo >= yieldPerAcre
                             select br.RatePaise).SingleOrDefault();

            return bonusRate;
        }

        public static void CreateBonusDetails(BonusCalculation bonusRecord)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            var bankDetail = orm.EntityBankDetails.Where(x => x.Id == bonusRecord.BankId).FirstOrDefault();
            var bonusData = orm.BonusAgreementDetails.FirstOrDefault(x => x.AgreementId == bonusRecord.AgreementId);
            if (bonusData != null)
            {
                bonusData.BonusAmountPaid = bonusRecord.BonusAmountPaid;
                bonusData.Comments = bonusRecord.Comments;
                bonusData.BankAccountName = (bankDetail != null) ? bankDetail.AccountHolderName : "";
                bonusData.BankName = (bankDetail != null) ? bankDetail.BankName : "";
                bonusData.BankAccountNumber = (bankDetail != null) ? bankDetail.BankAccount : "";
                bonusData.BankIFSC = (bankDetail != null) ? bankDetail.BankIFSC : "";
                bonusData.BankBranch = (bankDetail != null) ? bankDetail.BankBranch : "";
                bonusData.BonusStatus = bonusRecord.BonusStatus;
                bonusData.UpdatedBy = bonusRecord.UpdatedBy;
                bonusData.DateUpdated = bonusRecord.DateUpdated;
                orm.SaveChanges();
            }
            else
            {
                orm.BonusAgreementDetails.Add(new BonusAgreementDetail()
                {
                    AgreementId = bonusRecord.AgreementId,
                    AgreementNumber = bonusRecord.AgreementNumber,
                    AgreementDate = bonusRecord.AgreementDate,
                    EntityId = bonusRecord.EntityId,
                    EntityName = bonusRecord.EntityName,
                    HQCode = bonusRecord.HQCode,
                    TypeName = bonusRecord.TypeName,
                    SeasonName = bonusRecord.SeasonName,
                    LandSizeInAcres = bonusRecord.LandSizeInAcres,
                    RatePerKg = bonusRecord.RatePerKg,
                    NetWeight = bonusRecord.NetPayableWt,
                    NetPaid = bonusRecord.NetPayable,
                    BonusRate = bonusRecord.BonusRate,
                    BonusAmountPayable = bonusRecord.BonusAmountPayable,
                    BonusAmountPaid = bonusRecord.BonusAmountPaid,
                    BonusStatus = bonusRecord.BonusStatus,
                    BankAccountName = (bankDetail != null) ? bankDetail.AccountHolderName : "",
                    BankName = (bankDetail != null) ? bankDetail.BankName : "",
                    BankAccountNumber = (bankDetail != null) ? bankDetail.BankAccount : "",
                    BankIFSC = (bankDetail != null) ? bankDetail.BankIFSC : "",
                    BankBranch = (bankDetail != null) ? bankDetail.BankBranch : "",
                    Comments = bonusRecord.Comments,
                    CreatedBy = bonusRecord.CreatedBy,
                    UpdatedBy = bonusRecord.UpdatedBy,
                    DateCreated = bonusRecord.DateCreated,
                    DateUpdated = bonusRecord.DateUpdated
                });
                orm.SaveChanges();

                // Update Bonusprocessed Flag in EntityAgreement table
                var agreementData = orm.EntityAgreements.FirstOrDefault(e => e.Id == bonusRecord.AgreementId);
                if (agreementData != null)
                {
                    agreementData.BonusProcessed = true;
                    orm.SaveChanges();
                }
            }
        }

        /// Author:Rajesh V, Purpose:Mark selected Bonus Agreement status as Approved, on 02/08/2021
        public static void MarkBonusAgreementAsApproved(IEnumerable<long> agreeId, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            try
            {
                foreach (var id in agreeId)
                {
                    var bonusRecord = orm.BonusAgreementDetails.Where(x => x.AgreementId == id).FirstOrDefault();
                    if (bonusRecord.BonusStatus == BonusStatus.AwaitingApproval.ToString())
                    {
                        bonusRecord.BonusStatus = BonusStatus.Approved.ToString();
                        bonusRecord.DateUpdated = DateTime.Now;
                        bonusRecord.UpdatedBy = currentUser;

                    }
                }
                orm.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        /// Author:Rajesh V, Purpose:Get Approved BonusAgreements for Payment, on 02/08/2021
        public static IEnumerable<BonusCalculation> GetBonusAgreementPayment()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            //var BonusAgreements = new List<BonusCalculation>();
            IEnumerable<BonusCalculation> BonusAgreements = orm.BonusAgreementDetails.Where(x => x.BonusStatus == "Approved")
                .Select(x => new BonusCalculation()
                {
                    AgreementDate = x.AgreementDate,
                    AgreementNumber = x.AgreementNumber,
                    TypeName = x.TypeName,
                    EntityName = x.EntityName,
                    NetPayableWt = x.NetWeight,
                    BonusAmountPaid = x.BonusAmountPaid,
                    HQCode = x.HQCode,
                    SeasonName = x.SeasonName,
                    AgreementId = x.AgreementId,
                    BonusStatus = x.BonusStatus
                }).ToList();
            return BonusAgreements;
        }

        //Author - Rajesh V; Date:04/08/2021; Purpose: Get Bonus payment details
        public static ICollection<BonusPaymentReferences> GetBonusPaymentReference(PaymentReferenceFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<BonusPaymentReferences> bonusPaymentData = from bpr in orm.BonusPaymentReferences
                                                                  select new BonusPaymentReferences
                                                                  {
                                                                      Id = bpr.Id,
                                                                      PaymentReference = bpr.PaymentReference,
                                                                      AgreementNumber = bpr.AgreementNumber,
                                                                      AgreementCount = bpr.AgreementCount,
                                                                      Comments = bpr.Comments,
                                                                      TotalBonusPaid = (decimal)bpr.BonusAmountPaid,
                                                                      DateCreated = bpr.DateCreated,
                                                                      CreatedBy = bpr.CreatedBy,
                                                                      BankName = bpr.BankName,
                                                                      AccountNumber = bpr.AccountNumber,
                                                                      AccountName = bpr.AccountName,
                                                                      AccountAddress = bpr.AccountAddress,
                                                                      AccountEmail = bpr.AccountEmail,
                                                                      PaymentType = bpr.PaymentType,
                                                                      SenderInfo = bpr.SenderInfo,
                                                                  };

            if (searchCriteria.ApplyPaymentReferenceFilter)
            {
                if (searchCriteria.IsExactReferenceMatch)
                {
                    bonusPaymentData = bonusPaymentData.Where(x => x.PaymentReference == searchCriteria.PaymentReference);
                }
                else
                {
                    bonusPaymentData = bonusPaymentData.Where(x => x.PaymentReference.Contains(searchCriteria.PaymentReference));
                }
            }

            if (searchCriteria.ApplyDateFilter)
            {
                searchCriteria.DateTo = searchCriteria.DateTo.AddDays(1);
                // this is because - LocalTimeStamp has time as well, whereas DateTo is only date
                // so when we search for dates upto 2020-05-28, records created at time
                // 2020-05-28 17:00:00 should also get included.
                bonusPaymentData = bonusPaymentData.Where(x => x.DateCreated >= searchCriteria.DateFrom &&
                                                             x.DateCreated < searchCriteria.DateTo);
            }

            return bonusPaymentData.ToList();
        }

        /// Author:Rajesh V, Purpose:Save bonus Payment  details, on 04/08/2021
        public static void CreateBonusPaymentReference(BonusPaymentReferences bpr)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.BonusPaymentReferences.Add(new BonusPaymentReference()
            {
                PaymentReference = bpr.PaymentReference,
                Comments = bpr.Comments,
                BonusAmountPaid = bpr.TotalBonusPaid,
                AgreementNumber = bpr.AgreementNumber,
                AgreementCount = (int)bpr.AgreementCount,
                CreatedBy = bpr.CurrentUser,
                UpdatedBy = bpr.CurrentUser,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                BankName = bpr.BankName,
                AccountNumber = bpr.AccountNumber,
                AccountName = bpr.AccountName,
                AccountAddress = bpr.AccountAddress,
                AccountEmail = bpr.AccountEmail,
                PaymentType = bpr.PaymentType,
                SenderInfo = bpr.SenderInfo,
            });

            orm.SaveChanges();

        }

        /// Author:Rajesh V, Purpose:Mark bonus agreement as paid  details, on 04/08/2021
        public static DBSaveStatus MarkBonusAsPaid(IEnumerable<BonusCalculation> bonusRecord)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            foreach (var inputRec in bonusRecord)
            {
                var dbRec = orm.BonusAgreementDetails.Where(x => x.AgreementId == inputRec.AgreementId).FirstOrDefault();
                if (dbRec == null)
                {
                    return DBSaveStatus.RecordNotFound;
                }
                dbRec.PaymentReference = inputRec.PaymentReference;
                dbRec.DateUpdated = DateTime.Now;
                dbRec.UpdatedBy = inputRec.UpdatedBy;
                dbRec.BonusStatus = inputRec.BonusStatus;
            }

            orm.SaveChanges();
            return DBSaveStatus.Success;
        }

        //Author - Rajesh V; Date:05/08/2021; Purpose: Get Bonus Download data
        public static ICollection<BonusDownloadData> GetBonusDownloadData(string paymentRef)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<BonusDownloadData> bonusdownloadData = from bad in orm.BonusAgreementDetails
                                                              join bpr in orm.BonusPaymentReferences on bad.PaymentReference equals bpr.PaymentReference
                                                              where bad.PaymentReference == paymentRef
                                                              select new BonusDownloadData
                                                              {
                                                                  AgreementDate = bad.AgreementDate,
                                                                  TypeName = bad.TypeName,
                                                                  SeasonName = bad.SeasonName,
                                                                  NetWeight = bad.NetWeight,
                                                                  EntityName = bad.EntityName,
                                                                  BonusPaid = bad.BonusAmountPaid,
                                                                  BonusRate = bad.BonusRate,
                                                                  PaymentReference = bpr.PaymentReference,
                                                                  AgreementNumber = bad.AgreementNumber,
                                                                  Comments = bpr.Comments,
                                                                  DateCreated = bpr.DateCreated,
                                                                  CreatedBy = bpr.CreatedBy,
                                                                  BankName = bpr.BankName,
                                                                  AccountNumber = bpr.AccountNumber,
                                                                  AccountName = bpr.AccountName,
                                                                  AccountAddress = bpr.AccountAddress,
                                                                  AccountEmail = bpr.AccountEmail,
                                                                  PaymentType = bpr.PaymentType,
                                                                  SenderInfo = bpr.SenderInfo,
                                                              };

            return bonusdownloadData.ToList();
        }

        // Author - Kartik; Date:14/09/2021; Purpose: Get Project Data

        public static ICollection<DomainEntities.Projects> GetProjects(ProjectsFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.Projects> items = from e in orm.Projects
                                                        select new DomainEntities.Projects
                                                        {
                                                            Id = e.Id,
                                                            Name = e.Name,
                                                            Description = e.Description,
                                                            Category = e.ProjectCategory,
                                                            PlannedStartDate = e.PlannedStartDate,
                                                            PlannedEndDate = e.PlannedEndDate,
                                                            ActualStartDate = e.ActualStartDate,
                                                            ActualEndDate = e.ActualEndDate,
                                                            Status = e.Status,
                                                            CyclicCount = e.CyclicCount,
                                                            IsActive = e.IsActive
                                                        };

            if (searchCriteria.ApplyNameFilter)
            {
                items = items.Where(x => x.Name.Contains(searchCriteria.Name));
            }

            if (searchCriteria.ApplyDateFilter)
            {
                items = items.Where(x => x.PlannedEndDate >= searchCriteria.DateFrom &&
                                                             x.PlannedStartDate <= searchCriteria.DateTo);
            }

            if (searchCriteria.ApplyCategoryFilter)
            {
                if (searchCriteria.Category != "All")
                {
                    items = items.Where(x => x.Category == searchCriteria.Category);
                }

            }

            if (searchCriteria.ApplyStatusFilter)
            {
                if (searchCriteria.Status != "All")
                {
                    items = items.Where(x => x.Status == searchCriteria.Status);
                }
            }

            if (searchCriteria.ApplyActiveFilter)
            {
                items = items.Where(x => x.IsActive == searchCriteria.IsActive);
            }

            return items.OrderBy(x => x.Name).ToList();
        }

        public static DomainEntities.Projects GetSingleProject(long projectId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from e in orm.Projects
                    where e.Id == projectId
                    select new DomainEntities.Projects
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Description = e.Description,
                        Category = e.ProjectCategory,
                        PlannedStartDate = e.PlannedStartDate,
                        PlannedEndDate = e.PlannedEndDate,
                        ActualStartDate = e.ActualStartDate,
                        ActualEndDate = e.ActualEndDate,
                        Status = e.Status,
                        CyclicCount = e.CyclicCount,
                        IsActive = e.IsActive,
                        CreatedBy = e.CreatedBy,
                        UpdatedBy = e.UpdatedBy,
                    }).FirstOrDefault();
        }

        public static DBSaveStatus SaveProjectData(DomainEntities.Projects inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.Projects.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid Project Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            DateTime utcNow = DateTime.UtcNow;

            orm.ProjectAudits.Add(new DBModel.ProjectAudit()
            {
                XRefProjectId = inputRec.Id,
                Name = inputRec.Name,
                Description = inputRec.Description,
                ProjectCategory = inputRec.Category,
                PlannedStartDate = inputRec.PlannedStartDate,
                PlannedEndDate = inputRec.PlannedEndDate,
                ActualStartDate = inputRec.ActualStartDate,
                ActualEndDate = inputRec.ActualEndDate,
                Status = inputRec.Status,
                IsActive = inputRec.IsActive,
                DateCreated = utcNow,
                CreatedBy = inputRec.CurrentUser
            });

            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;

            sp.Name = inputRec.Name;
            sp.Description = inputRec.Description;
            sp.ProjectCategory = inputRec.Category;
            sp.PlannedStartDate = inputRec.PlannedStartDate;
            sp.PlannedEndDate = inputRec.PlannedEndDate;
            sp.Status = inputRec.Status;
            sp.IsActive = inputRec.IsActive;

            sp.CyclicCount++;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static void CreateProjectData(DomainEntities.Projects inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.Projects.Add(new DBModel.Project()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Name = inputRec.Name,
                Description = inputRec.Description,
                ProjectCategory = inputRec.Category,
                PlannedStartDate = inputRec.PlannedStartDate,
                PlannedEndDate = inputRec.PlannedEndDate,
                ActualStartDate = DateTime.UtcNow,
                ActualEndDate = DateTime.UtcNow,
                Status = inputRec.Status,
                IsActive = inputRec.IsActive,
                CyclicCount = 1
            });

            try
            {
                orm.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static ICollection<DomainEntities.ProjectAssignments> GetProjectAssignment(long projectId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.ProjectAssignments.Where(x => x.XRefProjectId == projectId)
                .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                (ex, sp) => new DomainEntities.ProjectAssignments()
                {
                    Id = ex.Id,
                    XRefProjectId = ex.XRefProjectId,
                    EmployeeId = ex.EmployeeId,
                    EmployeeCode = sp.StaffCode,
                    EmployeeName = sp.Name,
                    StartDate = ex.StartDate,
                    EndDate = ex.EndDate,
                    IsAssigned = ex.IsAssigned,
                    IsSelfAssigned = ex.IsSelfAssigned,
                    Comments = ex.Comments,
                    DateCreated = ex.DateCreated,
                    DateUpdated = ex.DateUpdated,
                    CreatedBy = ex.CreatedBy,
                    UpdatedBy = ex.UpdatedBy
                }).ToList();

        }

        public static ICollection<DomainEntities.DownloadProjects> GetProjectsAssignments(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from pa in orm.ProjectAssignments
                    join p in orm.Projects on pa.XRefProjectId equals p.Id
                    join s in orm.TenantEmployees on pa.EmployeeId equals s.Id
                    where s.EmployeeCode == staffCode && p.IsActive == true && pa.IsAssigned == true && p.Status != "Completed"
                    select new DomainEntities.DownloadProjects()
                    {
                        Id = p.Id,
                        ProjectName = p.Name,
                        ProjectDescription = p.Description,
                        ProjectCategory = p.ProjectCategory,
                        PlannedStartDate = p.PlannedStartDate,
                        PlannedEndDate = p.PlannedEndDate,
                        ActualStartDate = p.ActualStartDate,
                        ActualEndDate = p.ActualEndDate,
                        ProjectStatus = p.Status,
                        AssignedStartDate = pa.StartDate,
                        AssignedEndDate = pa.EndDate,
                        CreatedBy = p.UpdatedBy,
                        AssignedBy = pa.UpdatedBy
                    }).ToList();

        }

        public static ICollection<DomainEntities.DownloadTasks> GetTasksAssignments(string staffCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from pa in orm.TaskAssignments
                    join t in orm.Tasks on pa.XRefTaskId equals t.Id
                    join p in orm.Projects on t.XRefProjectId equals p.Id
                    join s in orm.TenantEmployees on pa.EmployeeId equals s.Id
                    where s.EmployeeCode == staffCode && t.IsActive == true && pa.IsAssigned == true
                    select new DomainEntities.DownloadTasks()
                    {
                        TaskAssignmentId = pa.Id,
                        TaskId = t.Id,
                        XRefProjectId = t.XRefProjectId,
                        ProjectName = p.Name,
                        TaskDescription = t.Description,
                        ActivityType = t.ActivityType,
                        ClientType = t.ClientType,
                        ClientName = t.ClientName,
                        ClientCode = t.ClientCode,
                        PlannedStartDate = t.PlannedStartDate,
                        PlannedEndDate = t.PlannedEndDate,
                        ActualStartDate = t.ActualStartDate,
                        ActualEndDate = t.ActualEndDate,
                        TaskStatus = t.Status,
                        Comments = t.Comments,
                        AssignedStartDate = pa.StartDate,
                        AssignedEndDate = pa.EndDate,
                        IsCreatedOnPhone = t.CreatedOnPhone,
                        IsSelfAssigned = pa.IsSelfAssigned,
                        CreatedBy = t.UpdatedBy,
                        AssignedBy = pa.UpdatedBy
                    }).ToList();

        }

        //public static DomainEntities.ProjectAssignments GetSingleProjectAssignment(long assignmentId)
        //{
        //    EpicCrmEntities orm = DBLayer.GetOrm;
        //    return (from e in orm.ProjectAssignments
        //            where e.Id == assignmentId
        //            select new DomainEntities.ProjectAssignments
        //            {
        //                Id = e.Id,
        //                XRefProjectId = e.XRefProjectId,
        //                EmployeeId = e.EmployeeId,
        //                StartDate = e.StartDate,
        //                EndDate = e.EndDate,
        //                IsDeleted = e.IsDeleted,
        //                DateCreated = e.DateCreated,
        //                DateUpdated = e.DateUpdated,
        //                CreatedBy = e.CreatedBy,
        //                UpdatedBy = e.UpdatedBy
        //            }).FirstOrDefault();
        //}

        public static DBSaveStatus SaveProjectAssignmentData(DomainEntities.ProjectAssignments inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.ProjectAssignments.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid Project Assignment Id");
            }

            DateTime utcNow = DateTime.UtcNow;

            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;

            sp.XRefProjectId = inputRec.XRefProjectId;
            sp.EmployeeId = inputRec.EmployeeId;
            sp.StartDate = inputRec.StartDate;
            sp.EndDate = inputRec.EndDate;
            sp.IsAssigned = inputRec.IsAssigned;
            sp.IsSelfAssigned = inputRec.IsSelfAssigned;
            sp.Comments = inputRec.Comments;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static void CreateProjectAssignmentData(DomainEntities.ProjectAssignments inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.ProjectAssignments.Add(new DBModel.ProjectAssignment()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                XRefProjectId = inputRec.XRefProjectId,
                EmployeeId = inputRec.EmployeeId,
                StartDate = inputRec.StartDate,
                EndDate = inputRec.EndDate,
                IsAssigned = inputRec.IsAssigned,
                IsSelfAssigned = inputRec.IsSelfAssigned,
                Comments = inputRec.Comments
            });

            try
            {
                orm.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }
        // Task and Followup

        public static IEnumerable<DomainEntities.Projects> GetProjectNames()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.Projects.AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new DomainEntities.Projects()
                {
                    Name = x.Name,
                    Id = x.Id
                }).ToList();
        }

        public static ICollection<DomainEntities.FollowUpTask> GetFollowUpTasks(FollowUpTaskFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.FollowUpTask> tasks =
                from t in orm.Tasks
                join p in orm.Projects on t.XRefProjectId equals p.Id
                select new DomainEntities.FollowUpTask()
                {
                    Id = t.Id,
                    XRefProjectId = t.XRefProjectId,
                    ProjectName = p.Name,
                    Description = t.Description,
                    ActivityType = t.ActivityType,
                    ClientType = t.ClientType,
                    ClientName = t.ClientName,
                    ClientCode = t.ClientCode,
                    PlannedStartDate = t.PlannedStartDate,
                    PlannedEndDate = t.PlannedEndDate,
                    ActualStartDate = t.ActualStartDate,
                    ActualEndDate = t.ActualEndDate,
                    Comments = t.Comments,
                    Status = t.Status,
                    CyclicCount = t.CyclicCount,
                    IsActive = t.IsActive,
                    DateCreated = t.DateCreated,
                    DateUpdated = t.DateUpdated,
                    CreatedBy = t.CreatedBy,
                    UpdatedBy = t.UpdatedBy,
                    IsCreatedOnPhone = t.CreatedOnPhone,
                    ActivityCount = orm.TaskActions.Where(x => x.XRefTaskId == t.Id).Count()
                };

            if (searchCriteria.ApplyActivityTypeFilter)
            {
                tasks = tasks.Where(x => x.ActivityType == searchCriteria.ActivityType);
            }

            if (searchCriteria.ApplyDateFilter)
            {
                tasks = tasks.Where(x => x.DateCreated >= searchCriteria.DateFrom && x.DateCreated <= searchCriteria.DateTo);
            }

            if (searchCriteria.ApplyClientTypeFilter)
            {
                tasks = tasks.Where(x => x.ClientType == searchCriteria.ClientType);
            }

            if (searchCriteria.ApplyClientNameFilter)
            {
                tasks = tasks.Where(x => x.ClientName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyProjectFilter)
            {
                tasks = tasks.Where(x => x.ProjectName == searchCriteria.ProjectName);
            }

            if (searchCriteria.ApplyTaskFilter)
            {
                tasks = tasks.Where(x => x.Description.Contains(searchCriteria.TaskDescription));
            }

            if (searchCriteria.ApplyTaskStatusFilter)
            {
                tasks = tasks.Where(x => x.Status == searchCriteria.TaskStatus);
            }

            if (searchCriteria.ApplyActiveFilter)
            {
                tasks = tasks.Where(x => x.IsActive == searchCriteria.IsActive);
            }

            //var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            //if (hqList != null)
            //{
            //    tasks = tasks.Where(x => hqList.Any(y => y == x.HQCode));
            //}

            if (searchCriteria.ApplyCreatedByFilter)
            {
                tasks = tasks.Where(x => x.CreatedBy.Contains(searchCriteria.CreatedBy));
            }

            if (searchCriteria.ApplyUpdatedByFilter)
            {
                tasks = tasks.Where(x => x.UpdatedBy.Contains(searchCriteria.UpdatedBy));
            }

            return tasks.OrderBy(x => x.Id).ToList();
        }

        public static DomainEntities.FollowUpTask GetSingleTask(long taskId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from e in orm.Tasks
                    where e.Id == taskId
                    select new DomainEntities.FollowUpTask
                    {
                        Id = e.Id,
                        XRefProjectId = e.XRefProjectId,
                        Description = e.Description,
                        ClientType = e.ClientType,
                        ClientName = e.ClientName,
                        ClientCode = e.ClientCode,
                        ActivityType = e.ActivityType,
                        PlannedStartDate = e.PlannedStartDate,
                        PlannedEndDate = e.PlannedEndDate,
                        ActualStartDate = e.ActualStartDate,
                        ActualEndDate = e.ActualEndDate,
                        Comments = e.Comments,
                        Status = e.Status,
                        CyclicCount = e.CyclicCount,
                        IsActive = e.IsActive
                    }).FirstOrDefault();
        }

        public static DBSaveStatus SaveFollowUpTaskData(DomainEntities.FollowUpTask inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.Tasks.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid FollowUp/Task Id");
            }

            if (sp.CyclicCount != inputRec.CyclicCount)
            {
                return DBSaveStatus.CyclicCheckFail;
            }

            DateTime utcNow = DateTime.UtcNow;

            orm.TaskAudits.Add(new DBModel.TaskAudit()
            {
                XRefTaskId = inputRec.Id,
                XRefTaskProjectId = inputRec.XRefProjectId,
                Description = inputRec.Description,
                ActivityType = inputRec.ActivityType,
                ClientType = inputRec.ClientType,
                ClientName = inputRec.ClientName,
                ClientCode = inputRec.ClientCode,
                PlannedStartDate = inputRec.PlannedStartDate,
                PlannedEndDate = inputRec.PlannedEndDate,
                ActualStartDate = inputRec.ActualStartDate,
                ActualEndDate = inputRec.ActualEndDate,
                Comments = inputRec.Comments,
                Status = inputRec.Status,
                IsActive = inputRec.IsActive,
                DateCreated = utcNow,
                CreatedBy = inputRec.CurrentUser
            });

            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;

            sp.Description = inputRec.Description;
            sp.ActivityType = inputRec.ActivityType;
            sp.ClientType = inputRec.ClientType;
            sp.ClientName = inputRec.ClientName;
            sp.ClientCode = inputRec.ClientCode;
            sp.PlannedStartDate = inputRec.PlannedStartDate;
            sp.PlannedEndDate = inputRec.PlannedEndDate;
            sp.Comments = inputRec.Comments;
            sp.Status = inputRec.Status;
            sp.IsActive = inputRec.IsActive;

            sp.CyclicCount++;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static void CreateFollowUpTaskData(DomainEntities.FollowUpTask inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.Tasks.Add(new DBModel.Task()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                XRefProjectId = inputRec.XRefProjectId,
                Description = inputRec.Description,
                ActivityType = inputRec.ActivityType,
                ClientType = inputRec.ClientType,
                ClientName = inputRec.ClientName,
                ClientCode = inputRec.ClientCode,
                PlannedStartDate = inputRec.PlannedStartDate,
                PlannedEndDate = inputRec.PlannedEndDate,
                ActualStartDate = DateTime.UtcNow,
                ActualEndDate = DateTime.UtcNow,
                Comments = inputRec.Comments,
                Status = inputRec.Status,
                IsActive = inputRec.IsActive,
                CyclicCount = 1
            });

            try
            {
                orm.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static ICollection<DomainEntities.TaskAssignments> GetTaskAssignment(long taskId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.TaskAssignments.Where(x => x.XRefTaskId == taskId)
                .Join(orm.SalesPersons, (e) => e.TenantEmployee.EmployeeCode, (sp) => sp.StaffCode,
                (ex, sp) => new DomainEntities.TaskAssignments()
                {
                    Id = ex.Id,
                    XRefTaskId = ex.XRefTaskId,
                    EmployeeId = ex.EmployeeId,
                    EmployeeCode = sp.StaffCode,
                    EmployeeName = sp.Name,
                    StartDate = ex.StartDate,
                    EndDate = ex.EndDate,
                    IsAssigned = ex.IsAssigned,
                    IsSelfAssigned = ex.IsSelfAssigned,
                    Comments = ex.Comments,
                    DateCreated = ex.DateCreated,
                    DateUpdated = ex.DateUpdated,
                    CreatedBy = ex.CreatedBy,
                    UpdatedBy = ex.UpdatedBy,
                    ActivityCount = orm.TaskActions.Where(x => x.XRefTaskAssignmentId == ex.Id && x.XRefTaskId == ex.XRefTaskId).Count()
                }).ToList();

        }

        public static DBSaveStatus SaveTaskAssignmentData(DomainEntities.TaskAssignments inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.TaskAssignments.Find(inputRec.Id);

            if (sp == null)
            {
                throw new ArgumentException("Invalid Project Assignment Id");
            }

            DateTime utcNow = DateTime.UtcNow;

            sp.UpdatedBy = inputRec.CurrentUser;
            sp.DateUpdated = DateTime.UtcNow;

            sp.XRefTaskId = inputRec.XRefTaskId;
            sp.EmployeeId = inputRec.EmployeeId;
            sp.StartDate = inputRec.StartDate;
            sp.EndDate = inputRec.EndDate;
            sp.IsAssigned = inputRec.IsAssigned;
            sp.IsSelfAssigned = inputRec.IsSelfAssigned;
            sp.Comments = inputRec.Comments;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static void CreateTaskAssignmentData(DomainEntities.TaskAssignments inputRec)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            orm.TaskAssignments.Add(new DBModel.TaskAssignment()
            {
                CreatedBy = inputRec.CurrentUser,
                UpdatedBy = inputRec.CurrentUser,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                XRefTaskId = inputRec.XRefTaskId,
                EmployeeId = inputRec.EmployeeId,
                StartDate = inputRec.StartDate,
                EndDate = inputRec.EndDate,
                IsAssigned = inputRec.IsAssigned,
                IsSelfAssigned = inputRec.IsSelfAssigned,
                Comments = inputRec.Comments
            });

            try
            {
                orm.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }
        }

        public static ICollection<DomainEntities.FollowUpTaskAction> GetTaskActions(long taskId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from e in orm.TaskActions
                    where e.XRefTaskId == taskId
                    select new DomainEntities.FollowUpTaskAction
                    {
                        Id = e.Id,
                        XRefTaskId = e.XRefTaskId,
                        XRefActivityId = e.XRefActivityId,
                        XRefTaskAssignmentId = e.XRefTaskAssignmentId,
                        EmployeeId = e.EmployeeId,
                        TimeStamp = e.TimeStamp,
                    }).ToList();
        }

        public static ICollection<DomainEntities.Entity> GetEntitiesForTask(string clientType)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.Entity> entities = from e in orm.Entities.Include("TenantEmployee")
                                                         where e.EntityType == clientType
                                                         select new DomainEntities.Entity
                                                         {
                                                             Id = e.Id,
                                                             HQCode = e.HQCode,
                                                             EntityType = e.EntityType,
                                                             EntityName = e.EntityName,
                                                             UniqueIdType = e.UniqueIdType,
                                                             UniqueId = e.UniqueId,
                                                             HQName = e.HQName,
                                                             EntityNumber = e.EntityNumber,
                                                             IsActive = e.IsActive,
                                                         };
            return entities.OrderBy(x => x.Id).ToList();
        }

        /// Author:Rajesh V, Purpose:Get Farmer Summary data based on search, on 07/10/2021
        public static IEnumerable<FarmerSummaryReport> GetFarmerSummary(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<FarmerSummaryReport> farmerSummary =
                from e in orm.Entities
                join ea in orm.EntityAgreements on e.Id equals ea.EntityId
                join wfs in orm.WorkflowSeasons on ea.WorkflowSeasonId equals wfs.Id into temp
                from t in temp.DefaultIfEmpty()
                select new FarmerSummaryReport()
                {
                    AgreementId = ea.Id,
                    EntityName = e.EntityName,
                    UniqueId = e.UniqueId,
                    AgreementNumber = ea.AgreementNumber,
                    Crop = t.TypeName,
                    SeasonName = t.SeasonName,
                    HQCode = e.HQCode
                };
            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                farmerSummary = farmerSummary.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                farmerSummary = farmerSummary.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            if (searchCriteria.ApplyClientNameFilter)
            {
                farmerSummary = farmerSummary.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyAgreementNumberFilter)
            {
                farmerSummary = farmerSummary.Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
            }

            if (searchCriteria.ApplyUniqueIdFilter)
            {
                farmerSummary = farmerSummary.Where(x => x.UniqueId.Contains(searchCriteria.UniqueId));
            }

            if (searchCriteria.ApplyCropFilter)
            {
                farmerSummary = farmerSummary.Where(x => x.Crop.Contains(searchCriteria.Crop));
            }

            if (searchCriteria.ApplySeasonNameFilter)
            {
                farmerSummary = farmerSummary.Where(x => x.SeasonName.Contains(searchCriteria.SeasonName));
            }

            return farmerSummary;

        }

        public static IEnumerable<FarmerSummaryReportData> GetFarmerSummaryData(long AgreementId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<FarmerSummaryReportData> farmerData =
                from e in orm.Entities
                join ea in orm.EntityAgreements on e.Id equals ea.EntityId into temp
                from t in temp.DefaultIfEmpty()
                where t.Id == AgreementId
                select new FarmerSummaryReportData()
                {
                    AgreementId = t.Id,
                    EntityName = e.EntityName,
                    FarmerId = e.EntityNumber,
                    UniqueId = e.UniqueId,
                    AgreementNumber = t.AgreementNumber,
                    HQName = e.HQName,
                    TerritoryName = e.TerritoryName,
                    LandInSize = t.LandSizeInAcres,
                    RatePerKg = t.RatePerKg
                };
            return farmerData.ToList();
        }

        public static IEnumerable<FarmerSummaryReportData> GetFarmerIssueData(long AgreementId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<FarmerSummaryReportData> farmerData =
                from ea in orm.EntityAgreements
                join ir in orm.IssueReturns on ea.Id equals ir.EntityAgreementId
                join im in orm.ItemMasters on ir.AppliedItemMasterId equals im.Id into temp
                from t in temp.DefaultIfEmpty()
                where ea.Id == AgreementId && ir.Status == "Approved"
                select new FarmerSummaryReportData()
                {
                    Issuedate = ir.DateUpdated,
                    IssueSlipNumber = ir.SlipNumber,
                    IssueInput = t.ItemDesc,
                    IssueQuantity = ir.AppliedQuantity,
                    Uom = t.Unit,
                    PricePerUom = ir.AppliedItemRate,
                    IssueType = ir.AppliedTransactionType
                };
            return farmerData.ToList();
        }

        public static IEnumerable<FarmerSummaryReportData> GetFarmerDwsData(long AgreementId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<FarmerSummaryReportData> farmerData =
                from ea in orm.EntityAgreements
                join d in orm.DWS on ea.Id equals d.AgreementId
                join str in orm.STRTags on d.STRTagId equals str.Id
                join dpr in orm.DWSPaymentReferences on d.PaymentReference equals dpr.PaymentReference into temp
                from t in temp.DefaultIfEmpty()
                where ea.Id == AgreementId && d.Status == "Paid"
                select new FarmerSummaryReportData()
                {
                    STRNumber = str.STRNumber,
                    DWSNumber = d.DWSNumber,
                    PurchaseDate = str.STRDate,
                    DWSQuantity = d.NetPayableWt,
                    PurchaseAmount = d.GoodsPrice,
                    PaymentReference = d.PaymentReference,
                    DWSDeduction = d.DeductAmount,
                    PayoutDate = t.DateCreated,
                    PaymentAmount = d.NetPayable
                };
            return farmerData.ToList();
        }

        /// Author:Gagana, Purpose:Get Farmer Summary data based on search, on 18/11/2021
        public static IEnumerable<FarmerSummaryReportData> GetFarmerAdvReqData(long AgreementId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<FarmerSummaryReportData> farmerData =
                from ea in orm.EntityAgreements
                join ar in orm.AdvanceRequests on ea.Id equals ar.EntityAgreementId
                where ea.Id == AgreementId && (ar.Status == "Approved" || ar.Status == "Partially Approved")
                select new FarmerSummaryReportData()
                {
                    AdvanceRequestDate = ar.AdvanceRequestDate,
                    AmountApproved = ar.AmountApproved
                };
            return farmerData.ToList();
        }

        public static IEnumerable<FarmerSummaryReportData> GetFarmerAgrBonusData(long AgreementId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<FarmerSummaryReportData> farmerData =
                from ea in orm.EntityAgreements
                join ab in orm.BonusAgreementDetails on ea.Id equals ab.AgreementId
                join bpr in orm.BonusPaymentReferences on ab.PaymentReference equals bpr.PaymentReference into temp
                from t in temp.DefaultIfEmpty()
                where ea.Id == AgreementId && ab.BonusStatus == "Paid"
                select new FarmerSummaryReportData()
                {
                    TotalNetQuantity = ab.NetWeight,
                    BonusRate = ab.BonusRate,
                    BonusPayableAmount = ab.BonusAmountPayable,
                    BonusAmountPaid = ab.BonusAmountPaid,
                    PaymentDate = t.DateCreated,
                    BonusPaymentReference = ab.PaymentReference
                };
            return farmerData.ToList();
        }
        //Added by Swetha, Purpose:Get Leave Data
        public static ICollection<DomainEntities.DashboardLeave> GetDashboardLeaves(LeaveFilter searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            IQueryable<DomainEntities.DashboardLeave> leaves = from l in orm.Leaves
                                                               join sp in orm.SalesPersons on l.EmployeeCode equals sp.StaffCode
                                                               select new DashboardLeave
                                                               {
                                                                   Id = l.Id,
                                                                   EmployeeId = l.EmployeeId,
                                                                   EmployeeCode = l.EmployeeCode,
                                                                   EmployeeName = sp.Name,
                                                                   StartDate = l.StartDate,
                                                                   EndDate = l.EndDate,
                                                                   LeaveType = l.LeaveType,
                                                                   LeaveReason = l.LeaveReason,
                                                                   Comment = l.Comment,
                                                                   DaysCountExcludingHolidays = l.DaysCountExcludingHolidays,
                                                                   DaysCount = l.DaysCount,
                                                                   LeaveStatus = l.LeaveStatus,
                                                                   ApproveNotes = l.ApproveNotes,
                                                                   DateCreated = l.DateCreated,
                                                                   DateUpdated = l.DateUpdated,
                                                                   CreatedBy = l.CreatedBy,
                                                                   UpdatedBy = l.UpdatedBy,
                                                                   HQCode = (sp == null) ? string.Empty : sp.HQCode,
                                                               };

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                leaves = leaves.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                leaves = leaves.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }
            if (searchCriteria.ApplyLeaveDurationFilter)
            {
                leaves = leaves.Where(x => x.DaysCountExcludingHolidays == searchCriteria.LeaveDuration);
            }
            if (searchCriteria.ApplyLeaveTypeFilter)
            {
                leaves = leaves.Where(x => x.LeaveType == searchCriteria.LeaveType);
            }
            if (searchCriteria.ApplyLeaveStatusFilter)
            {
                leaves = leaves.Where(x => x.LeaveStatus == searchCriteria.LeaveStatus);
            }

            if (searchCriteria.ApplyDateFilter)
            {
                leaves = leaves.Where(x => x.StartDate >= searchCriteria.DateFrom && x.StartDate <= searchCriteria.DateTo);
            }
            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);
            if (hqList != null)
            {
                leaves = leaves.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (!searchCriteria.IsSuperAdmin)
            {
                var rsHQCodes = orm.GetOfficeHierarchyForStaff(searchCriteria.TenantId, searchCriteria.CurrentUserStaffCode).Select(x => x.HQCode).ToList();
                leaves = leaves.Where(x => rsHQCodes.Any(rsHQCode => rsHQCode == x.HQCode));
            }

            return leaves.OrderBy(x => x.Id).ToList();

        }
        public static DomainEntities.DashboardLeave GetSingleLeave(long Id)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return (from l in orm.Leaves
                    join sp in orm.SalesPersons on l.EmployeeCode equals sp.StaffCode
                    where l.Id == Id
                    select new DashboardLeave
                    {
                        Id = l.Id,
                        EmployeeCode = l.EmployeeCode,
                        EmployeeName = sp.Name,
                        StartDate = l.StartDate,
                        EndDate = l.EndDate,
                        LeaveType = l.LeaveType,
                        LeaveReason = l.LeaveReason,
                        LeaveStatus = l.LeaveStatus,
                        Comment = l.Comment,
                        DaysCountExcludingHolidays = l.DaysCountExcludingHolidays,
                        ApproveNotes = l.ApproveNotes,
                        DateUpdated = l.DateUpdated,
                        UpdatedBy = l.UpdatedBy
                    }).FirstOrDefault();

        }
        public static DBSaveStatus SaveLeaveApproveData(DomainEntities.DashboardLeave inputRec, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var sp = orm.Leaves.Find(inputRec.Id);
            
            if (sp == null)
            {
                throw new ArgumentException("Invalid Leave Id");
            }

            DateTime dateUpdated = DateTime.UtcNow;

            sp.LeaveStatus = inputRec.LeaveStatus;
            sp.ApproveNotes = inputRec.ApproveNotes;
            sp.UpdatedBy = currentUser;
            sp.DateUpdated = dateUpdated;

            try
            {
                orm.SaveChanges();
                return DBSaveStatus.Success;
            }
            catch (DbEntityValidationException ex)
            {
                string s = GetErrorTextFromValidationException(ex);
                throw new Exception(s);
            }

        }
        //Author:Venkatesh, Purpose: Get Dealers Not Met Report on: 2022/11/09
        //Modified by : Swetha M  on Date:2022/12/03
        public static IEnumerable<DealersNotMetReport> GetDealersNotMet(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            ICollection<DomainEntities.OfficeHierarchy> oh = DBLayer.GetDetailedAssociations(Utils.SiteConfigData.TenantId);

            var actvityList = (from Activity in orm.Activities
                               where Activity.At >= searchCriteria.DateFrom && Activity.At <= searchCriteria.DateTo
                               group Activity by new
                               {
                                   Activity.ClientCode,
                               } into activities
                               select new
                               {
                                   ClientCode = activities.Key.ClientCode,
                               });

            IQueryable<Customer> dnm = orm.Customers.Where(x => !actvityList.Any(y => y.ClientCode == x.CustomerCode)).AsQueryable();


            IQueryable<DealersNotMetReport> dealersNotMets = (from o in oh
                                                              join d in dnm on o.HQCode equals d.HQCode
                                                              join s in orm.SalesPersons on d.HQCode equals s.HQCode
                                                              select new DealersNotMetReport
                                                              {
                                                                  CustomerCode = d.CustomerCode,
                                                                  CustomerName = d.Name,
                                                                  EmployeeCode = s.StaffCode,
                                                                  EmployeeName = s.Name,
                                                                  HQCode = d.HQCode,
                                                                  HQName = o.HQName,
                                                                  LastActivity = orm.Activities.Where(x => x.ClientCode == d.CustomerCode).FirstOrDefault() != null ?
                                                               orm.Activities.Where(x => x.ClientCode == d.CustomerCode).Max(x => x.DateCreated).ToString() : "N/A",
                                                                  AreaCode = o.AreaCode,
                                                                  AreaName = o.AreaName,
                                                                  TerritoryCode = o.TerritoryCode,
                                                                  TerritoryName = o.TerritoryName,
                                                                  ZoneCode = o.ZoneCode,
                                                                  ZoneName = o.ZoneName,
                                                                  IsActive = d.IsActive,
                                                                  ContactNumber = d.ContactNumber,
                                                              }).AsQueryable();


            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);

            if (hqList != null)
            {
                dealersNotMets = dealersNotMets.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (searchCriteria.ApplyCustomerCodeFilter)
            {
                dealersNotMets = dealersNotMets.Where(x => x.CustomerCode.Contains(searchCriteria.CustomerCode));
            }
            if (searchCriteria.ApplyCustomerNameFilter)
            {
                dealersNotMets = dealersNotMets.Where(x => x.CustomerName.Contains(searchCriteria.CustomerName));
            }
            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                dealersNotMets = dealersNotMets.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }
            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                dealersNotMets = dealersNotMets.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            return dealersNotMets;

        }


        //Author:Gagana, Purpose: Get Geo Tagging Report on: 2022/12/29
        //Modified By: Swetha M on: 2023/01/04
        public static IEnumerable<GeoTaggingReport> GetGeoTagging(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            long TenantId = Utils.SiteConfigData.TenantId;

            ICollection<DomainEntities.OfficeHierarchy> oh = DBLayer.GetDetailedAssociations(TenantId);
            ObjectResult<string> soEmpCode = orm.GetSOStaffCodes();
            IQueryable<string> soStaffCodes = soEmpCode.AsQueryable();

            IQueryable<GeoTaggingReport> geoTagging = (from sp in orm.SalesPersons
                                                       join spa in orm.SalesPersonAssociations.Where(x => x.IsDeleted == false)
                                                       on sp.StaffCode equals spa.StaffCode into Assignments
                                                       from a in Assignments.DefaultIfEmpty()
                                                       join c in orm.Customers on a.CodeValue equals c.HQCode
                                                       join gl in orm.GeoLocations on c.CustomerCode equals gl.ClientCode
                                                       into GeoCustomers
                                                       from gc in GeoCustomers.DefaultIfEmpty()
                                                       group gc by new
                                                       {
                                                           sp.StaffCode,
                                                           sp.HQCode,
                                                           sp.Id,
                                                           sp.Name,
                                                           c.CustomerCode,
                                                           customername = c.Name,
                                                           sp.IsActive,
                                                           c.Type
                                                       } into Geotagging
                                                       select new GeoTaggingReport
                                                       {
                                                           EmployeeCode = Geotagging.Key.StaffCode,
                                                           EmployeeName = Geotagging.Key.Name,
                                                           CustomerCode = Geotagging.Key.CustomerCode,
                                                           CustomerName = Geotagging.Key.customername,
                                                           GeoTagStatus = Geotagging.Max(x => x.Latitude) != null ? true : false,
                                                           EmployeeStatus = Geotagging.Key.IsActive,
                                                           EmployeeStatusInSp = Geotagging.Key.IsActive,
                                                           HQCode = Geotagging.Key.HQCode,
                                                           CustomerType = Geotagging.Key.Type
                                                       });


            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);

            if (hqList != null)
            {
                geoTagging = geoTagging.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (searchCriteria.ApplyClientNameFilter)
            {
                geoTagging = geoTagging.Where(x => x.CustomerName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                geoTagging = geoTagging.Where(x => x.EmployeeCode.Equals(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                geoTagging = geoTagging.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            if (searchCriteria.ApplyEmployeeStatusFilter)
            {
                geoTagging = geoTagging.Where(x => searchCriteria.EmployeeStatus == (x.EmployeeStatus));
            }

            if (searchCriteria.ApplyGeoTagStatusFilter)
            {
                geoTagging = geoTagging.Where(x => searchCriteria.GeoTagStatus == x.GeoTagStatus);
            }

            if (searchCriteria.ApplyBusinessRoleFilter)
            {
                if (searchCriteria.BusinessRole.Equals("Not Managers", StringComparison.OrdinalIgnoreCase))
                {
                    geoTagging = geoTagging.Where(x => soStaffCodes.Contains(x.EmployeeCode)).AsQueryable();

                }
                else if (searchCriteria.BusinessRole.Equals("Managers", StringComparison.OrdinalIgnoreCase))
                {
                    geoTagging = geoTagging.Where(x => !soStaffCodes.Contains(x.EmployeeCode)).AsQueryable();

                }

            }

            List<GeoTaggingReport> geoTaggingCount = new List<GeoTaggingReport>();

            foreach (var item in geoTagging)
            {
                string DivisionCode = orm.StaffDivisions.Where(x => x.StaffCode == item.EmployeeCode).OrderBy(x => x.DivisionCode).Select(x => x.DivisionCode).FirstOrDefault();
                item.DivisionName = orm.CodeTables.Where(x => x.CodeType == "Division" && x.CodeValue == DivisionCode).Select(x => x.CodeName).FirstOrDefault();
                item.BranchName = oh.Where(x => x.HQCode == item.HQCode).Select(x => x.AreaName).FirstOrDefault();
                item.ZoneName = oh.Where(x => x.HQCode == item.HQCode).Select(x => x.ZoneName).FirstOrDefault();
                geoTaggingCount.Add(item);
            }
            return geoTaggingCount;

        }


        //Author by : Swetha M  on Date:2022/12/03
        //Getting the HQ Codes based on Assignment for each StaffCode
        public static ICollection<DealersSummaryReportData> GetHQCodeForStaff(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            ICollection<DomainEntities.OfficeHierarchy> oh = DBLayer.GetDetailedAssociations(Utils.SiteConfigData.TenantId);


            IQueryable<DealersSummaryReportData> customerCount = (from sp in orm.SalesPersons
                                                                  join spa in orm.SalesPersonAssociations.Where(x => x.IsDeleted == false)
                                                                  on sp.StaffCode equals spa.StaffCode
                                                                  into associations
                                                                  from a in associations.DefaultIfEmpty()
                                                                  join cu in orm.Customers
                                                                  on a.CodeValue equals cu.HQCode
                                                                  into customers
                                                                  from c in customers.DefaultIfEmpty()
                                                                  where sp.BusinessRole!="PPA"
                                                                  group c by new
                                                                  {
                                                                      sp.StaffCode,
                                                                      sp.Name,
                                                                      sp.IsActive,
                                                                      sp.HQCode,
                                                                      sp.Id,
                                                                  } into Customers
                                                                  select new DealersSummaryReportData
                                                                  {
                                                                      StaffCode = Customers.Key.StaffCode,
                                                                      EmployeeName = Customers.Key.Name,
                                                                      HQCode = Customers.Key.HQCode,
                                                                      EmployeeStatus = Customers.Key.IsActive,
                                                                      TotalDealersCount = Customers.Count(x => x.CustomerCode != null)
                                                                  });

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);

            if (hqList != null)
            {
                customerCount = customerCount.Where(x => hqList.Any(y => y == x.HQCode));
            }
            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                customerCount = customerCount.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                customerCount = customerCount.Where(x => x.StaffCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyEmployeeStatusFilter)
            {
                customerCount = customerCount.Where(x => searchCriteria.EmployeeStatus == (x.EmployeeStatus));
            }

            List<DealersSummaryReportData> dealerSummary = new List<DealersSummaryReportData>();

            foreach (var item in customerCount)
            {
                string DivisionCode = orm.StaffDivisions.Where(x => x.StaffCode == item.StaffCode).OrderBy(x => x.DivisionCode).Select(x => x.DivisionCode).FirstOrDefault();
                item.DivisionName = orm.CodeTables.Where(x => x.CodeType == "Division" && x.CodeValue == DivisionCode).Select(x => x.CodeName).FirstOrDefault();
                item.BranchName = oh.Where(x => x.HQCode == item.HQCode).Select(x => x.AreaName).FirstOrDefault();
                dealerSummary.Add(item);

            }



            return dealerSummary.ToList();
        }
        public static ICollection<DealersGeoTagCount> GetGeoTaggingCount()
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            ICollection<DealersGeoTagCount> geotaggingCount = (from sp in orm.SalesPersons
                                                               join spa in orm.SalesPersonAssociations.Where(x => x.IsDeleted == false)
                                                               on sp.StaffCode equals spa.StaffCode
                                                               join cu in orm.Customers on spa.CodeValue equals cu.HQCode
                                                               join gl in orm.GeoLocations on cu.CustomerCode equals gl.ClientCode
                                                               into GeoTagging
                                                               from gt in GeoTagging.DefaultIfEmpty()
                                                               where gt.IsActive == true
                                                               group gt by new
                                                               {
                                                                   cu.CustomerCode,
                                                                   sp.StaffCode,
                                                               } into geo
                                                               select new
                                                               {
                                                                   StaffCode = geo.Key.StaffCode,
                                                                   Count = geo.Count()
                                                               }).GroupBy(x => x.StaffCode)
                                                               .Select(group => new DealersGeoTagCount
                                                               {
                                                                   StaffCode = group.Key,
                                                                   GeoTagCount = group.Count()
                                                               }).ToList();

            return geotaggingCount;

        }

        ////Author by : Swetha M  on Date:2023/01/15
        ////Get all the agreements 
        public static IQueryable<AgreementReportData> GetAgreementsReportData(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;


            IQueryable<AgreementReportData> agreements = from ea in orm.EntityAgreements
                                                         join et in orm.Entities.Include("TenantEmployee") on ea.EntityId equals et.Id
                                                         select new AgreementReportData
                                                         {
                                                             AgreementNumber = ea.AgreementNumber,
                                                             Status = ea.Status,
                                                             ZoneCode = ea.ZoneCode,
                                                             ZoneName = ea.ZoneName,
                                                             AreaCode = ea.AreaCode,
                                                             AreaName = ea.AreaName,
                                                             TerritoryCode = ea.TerritoryCode,
                                                             TerritoryName = ea.TerritoryName,
                                                             HQCode = ea.HQCode,
                                                             HQName = ea.HQName,
                                                             RatePerKg = ea.RatePerKg,
                                                             LandSize = ea.LandSizeInAcres,
                                                             WorkflowSeasonName = ea.WorkflowSeason.SeasonName,
                                                             EmployeeCode = et.TenantEmployee.EmployeeCode,
                                                             EmployeeName = et.TenantEmployee.Name,
                                                             AtBusiness = et.AtBusiness,
                                                             EntityName = et.EntityName,
                                                             EntityDate = et.EntityDate,
                                                             UniqueIdType = et.UniqueIdType,
                                                             UniqueId = et.UniqueId,
                                                             FatherHusbandName = et.FatherHusbandName,
                                                             IsActive = et.IsActive,
                                                             BankDetailCount = et.EntityBankDetails.Count,
                                                             ContactCount = et.ContactCount,
                                                             ImageCount = et.ImageCount,
                                                             Latitude = et.Latitude,
                                                             Longitude = et.Longitude,
                                                             EntityId = et.Id,
                                                             CropName = ea.WorkflowSeason.TypeName
                                                         };




            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                agreements = agreements.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                agreements = agreements.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }


            if (searchCriteria.ApplyClientNameFilter)
            {
                agreements = agreements.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyUniqueIdFilter)
            {
                agreements = agreements.Where(x => x.UniqueId.Contains(searchCriteria.UniqueId));
            }
            if (searchCriteria.ApplySeasonNameFilter)
            {
                if (searchCriteria.SeasonName != "All")
                {
                    agreements = agreements.Where(x => x.WorkflowSeasonName.Equals(searchCriteria.SeasonName));
                }
            }
            if (searchCriteria.ApplyCropFilter)
            {
                agreements = agreements.Where(x => x.CropName.Equals(searchCriteria.Crop, StringComparison.OrdinalIgnoreCase));
            }

            if (searchCriteria.ApplyAgreementStatusFilter)
            {
                agreements = agreements.Where(x => x.Status.Equals(searchCriteria.AgreementStatus, StringComparison.OrdinalIgnoreCase));
            }

            if (searchCriteria.ApplyAgreementNumberFilter)
            {
                agreements = agreements.Where(x => x.AgreementNumber.Contains(searchCriteria.AgreementNumber));
            }

            if (searchCriteria.ApplyBankDetailStatusFilter)
            {
                // find those entityIds that satisfy the condition
                var entityBankDetails = orm.EntityBankDetails
                    .Where(x => x.Status == searchCriteria.BankDetailStatus)
                    .Select(x => x.EntityId)
                    .ToList();

                agreements = agreements.Where(x => entityBankDetails.Any(y => y == x.EntityId));
            }
            if (searchCriteria.ApplyZoneFilter)
            {
                agreements = agreements.Where(x => x.ZoneName.Contains(searchCriteria.Zone));
            }

            if (searchCriteria.ApplyAreaFilter)
            {
                agreements = agreements.Where(x => x.AreaName.Contains(searchCriteria.Area));
            }

            if (searchCriteria.ApplyTerritoryFilter)
            {
                agreements = agreements.Where(x => x.TerritoryName.Contains(searchCriteria.Territory));
            }
            if (searchCriteria.ApplyHQFilter)
            {
                agreements = agreements.Where(x => x.HQCode.Contains(searchCriteria.HQ));
            }

            return agreements;
        }
        //Author by : Swetha M  on Date:2023/01/31
        //Save the bulk approved expenses
        public static int ApproveBulkExpense(List<ApprovalData> approvalData, SalesPersonLevel level)
        {
            using (EpicCrmEntities orm = DBLayer.GetOrm)
            {
                foreach(var item in approvalData)
                {
                    var expense = orm.Expenses.Find(item.Id);
                    if (expense == null)
                    {
                        return 2; //Invalid expense Id
                    }

                    if ((level == SalesPersonLevel.Territory && expense.IsTerritoryApproved) ||
                        (level == SalesPersonLevel.Area && expense.IsAreaApproved) ||
                        (level == SalesPersonLevel.Zone && expense.IsZoneApproved))
                    {
                        return 0; //expense already approved
                    }

                    switch (level)
                    {
                        case SalesPersonLevel.Territory:
                            expense.IsTerritoryApproved = true;
                            expense.Timestamp = DateTime.UtcNow;
                            break;
                        case SalesPersonLevel.Area:
                            expense.IsTerritoryApproved = true;
                            expense.IsAreaApproved = true;
                            expense.Timestamp = DateTime.UtcNow;
                            break;
                        case SalesPersonLevel.Zone:
                            expense.IsTerritoryApproved = true;
                            expense.IsAreaApproved = true;
                            expense.IsZoneApproved = true;
                            expense.Timestamp = DateTime.UtcNow;
                            break;
                        default:
                            return 5;
                    }

                    DBModel.ExpenseApproval ea = new DBModel.ExpenseApproval()
                    {
                        ApproveLevel = level.ToString(),
                        ApproveDate = item.ApprovedDate,
                        ApproveNotes = item.ApproveComments,
                        ApproveAmount = item.ApprovedAmt,
                        ApprovedBy = item.ApprovedBy,
                        Timestamp = DateTime.UtcNow
                    };
                    expense.ExpenseApprovals.Add(ea);
                    orm.SaveChanges();
                }

                return 1; //Success
            }
        }
        ////Author by : GAgana  on Date:2023/01/30
        ////Get all the Duplicate Farmers
        public static IQueryable<DuplicateFarmersReport> GetDuplicateFarmersReportData(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var Duplicates = (from et in orm.Entities
                       group et by new
                       {
                           et.UniqueId
                       } into g
                       where g.Count() > 1 && g.Key.UniqueId != ""   
                       select new
                       {
                           g.Key.UniqueId
                       }).ToList();

            IQueryable<DuplicateFarmersReport> farmers = (from et in Duplicates
                                                          join ea in orm.Entities.Include("TenantEmployee")
                                                          on et.UniqueId equals ea.UniqueId
                                                          select new DuplicateFarmersReport
                                                          {
                                                              TerritoryCode = ea.TerritoryCode,
                                                              TerritoryName = ea.TerritoryName,
                                                              HQCode = ea.HQCode,
                                                              HQName = ea.HQName,
                                                              EmployeeCode = ea.TenantEmployee.EmployeeCode,
                                                              EmployeeName = ea.TenantEmployee.Name,
                                                              EntityName = ea.EntityName,
                                                              UniqueId = et.UniqueId,
                                                              FatherHusbandName = ea.FatherHusbandName,
                                                              IsActive = ea.IsActive,
                                                              EntityNumber = ea.EntityNumber,
                                                              AgreementCount = ea.EntityAgreements.Count,
                                                              EntityType = ea.EntityType,
                                                          }).AsQueryable();

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);

            if (hqList != null)
            {
                farmers = farmers.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                farmers = farmers.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                farmers = farmers.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyClientTypeFilter)
            {
                farmers = farmers.Where(x => x.EntityType == searchCriteria.ClientType);
            }

           
            if (searchCriteria.ApplyUniqueIdFilter)
            {
                farmers = farmers.Where(x => x.UniqueId.Contains(searchCriteria.UniqueId));
            }

            if (searchCriteria.ApplyProfileStatusFilter)
            {
                farmers = farmers.Where(x => searchCriteria.ProfileStatus == x.IsActive);
            }

            return farmers;
        }


        //Author:Gagana, Purpose:Get Existing ActiveCrops to create Agreement; Date:10-02-2023

        public static IQueryable<DomainEntities.WorkflowSeason> GetEntityActiveCrops(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            var Agreementcrops = orm.EntityAgreements.Where(x => x.EntityId == entityId && (x.Status == "Approved" || x.Status == "Pending"))
                 .Select(x => new DomainEntities.EntityAgreement
                 {
                     WorkflowSeasonId = x.WorkflowSeasonId
                 }).ToList();

            var crop = GetOpenWorkflowSeasons();

            IQueryable<DomainEntities.WorkflowSeason> ActiveCrops = (from et in crop
                                                                     where !(from ea in Agreementcrops
                                                                             select ea.WorkflowSeasonId).Contains(et.Id)
                                                                     select new DomainEntities.WorkflowSeason
                                                                     {
                                                                         Id = et.Id,
                                                                         TypeName = et.TypeName
                                                                     }).AsQueryable();

            return ActiveCrops;
        }

        //Author:Gagana Purpose:Save details of new Agreement details; Date:15-02-2023
        public static void AddAgreement(DomainEntities.EntityAgreement ea, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DateTime ts = DateTime.UtcNow;
            DBModel.AgreementNumber rec = new AgreementNumber();

            using (TransactionScope scope = new
                         TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                         {
                             IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                         }))
            {

                var AgreementNum = orm.AgreementNumbers.Where(x => x.IsUsed==false).OrderBy(x => x.Sequence)
                                    .Select(x=>x.AgreementNumber1).First();
                rec = orm.AgreementNumbers.Where(x => x.AgreementNumber1 == AgreementNum).FirstOrDefault();
                rec.IsUsed = true;
                rec.UsedTimestamp = ts;
                orm.AgreementNumbers.AddOrUpdate(rec);
                scope.Complete();
            }

            DBModel.EntityAgreement Ag
              = new DBModel.EntityAgreement()
              {
                  EntityId = ea.EntityId,
                  WorkflowSeasonId = ea.WorkflowSeasonId,
                  ZoneCode = ea.ZoneCode,
                  AreaCode = ea.AreaCode,
                  TerritoryCode = ea.TerritoryCode,
                  HQCode = ea.HQCode,
                  ZoneName = ea.ZoneName,
                  AreaName = ea.AreaName,
                  TerritoryName = ea.TerritoryName,
                  HQName = ea.HQName,
                  Status = Utils.SiteConfigData.AgreementDefaultStatus,
                  LandSizeInAcres = ea.LandSizeInAcres,
                  AgreementNumber = rec.AgreementNumber1,
                  ActivityId = 0,
                  BonusProcessed = false,
                  PassbookReceivedDate = ea.PassbookReceivedDate,
                  IsPassbookReceived = false,
                  CreatedBy = currentUser,
                  UpdatedBy = currentUser,
                  DateCreated = ts,
                  DateUpdated = ts,
                  RatePerKg = orm.WorkflowSeasons.Where(x => x.Id == ea.WorkflowSeasonId).Select(x => x.RatePerKg).FirstOrDefault(),
                  EmployeeId = ea.EmployeeId
              };
            orm.EntityAgreements.Add(Ag);
            orm.SaveChanges();
 
        }


        ///Author by : GAgana  on Date:2023/01/30
        ////Get all the Duplicate Farmers
        public static IQueryable<FarmersBankAccountReport> GetFarmersBankAccountReportData(SearchCriteria searchCriteria)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<FarmersBankAccountReport> farmersbankdetails = (from ea in orm.Entities.Include("TenantEmployee")
                                                          join eb in orm.EntityBankDetails
                                                          on ea.Id equals eb.EntityId
                                                          select new FarmersBankAccountReport
                                                          {
                                                              TerritoryCode = ea.TerritoryCode,
                                                              TerritoryName = ea.TerritoryName,
                                                              HQCode = ea.HQCode,
                                                              HQName = ea.HQName,
                                                              EmployeeCode = ea.TenantEmployee.EmployeeCode,
                                                              EmployeeName = ea.TenantEmployee.Name,
                                                              EntityName = ea.EntityName,
                                                              EntityNumber = ea.EntityNumber,
                                                              IsSelfAccount = eb.IsSelfAccount,
                                                              AccountHolderName = eb.AccountHolderName,
                                                              AccountHolderPAN = eb.AccountHolderPAN,
                                                              BankName = eb.BankName,
                                                              BankAccount = eb.BankAccount,
                                                              BankIFSC = eb.BankIFSC,
                                                              BankBranch = eb.BankBranch,
                                                              Status = eb.Status,
                                                          }).AsQueryable();

            var hqList = DBLayer.GetFilteringHQCodes(searchCriteria);

            if (hqList != null)
            {
                farmersbankdetails = farmersbankdetails.Where(x => hqList.Any(y => y == x.HQCode));
            }

            if (searchCriteria.ApplyEmployeeNameFilter)
            {
                farmersbankdetails = farmersbankdetails.Where(x => x.EmployeeName.Contains(searchCriteria.EmployeeName));
            }

            if (searchCriteria.ApplyEmployeeCodeFilter)
            {
                farmersbankdetails = farmersbankdetails.Where(x => x.EmployeeCode.Contains(searchCriteria.EmployeeCode));
            }

            if (searchCriteria.ApplyClientNameFilter)
            {
                farmersbankdetails = farmersbankdetails.Where(x => x.EntityName.Contains(searchCriteria.ClientName));
            }

            if (searchCriteria.ApplyBankDetailStatusFilter)
            {
                farmersbankdetails = farmersbankdetails.Where(x => x.Status.Contains(searchCriteria.BankDetailStatus));
            }
            return farmersbankdetails;
        }

        //Author:Gowtham S , Purpose:Save Add Bank Account details ; Date:27-02-2023

        public static void AddBankDetails(DomainEntities.EntityBankDetail ea, string currentUser, string fileName)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DateTime ts = DateTime.UtcNow;

            DBModel.EntityBankDetail rec = new DBModel.EntityBankDetail()
            {
                EntityId = ea.EntityId,
                IsSelfAccount = ea.IsSelfAccount,
                AccountHolderName = ea.AccountHolderName,
                AccountHolderPAN = ea.AccountHolderPAN,
                BankAccount = ea.BankAccount,
                BankName = ea.BankName,
                BankIFSC = ea.BankIFSC,
                BankBranch = ea.BankBranch,
                ImageCount = ea.ImageCount,
                SqliteBankDetailId = 0,
                DateCreated = ts,
                DateUpdated =ts,
                CreatedBy = currentUser,
                UpdatedBy = currentUser,
                IsActive= ea.IsActive,
                Status = ea.Status,
                IsApproved = ea.IsApproved,
                Comments = ea.Comments

            };
            orm.EntityBankDetails.Add(rec);

            DBModel.Image image = new DBModel.Image()
            {
                ImageFileName = fileName,
                SourceId = 0
            };
            orm.Images.Add(image);

            DBModel.EntityBankDetailImage bankImage = new DBModel.EntityBankDetailImage()
            {
              EntityBankDetailId = rec.Id,
              SequenceNumber = 1,
              ImageFileName = fileName
            };
            orm.EntityBankDetailImages.Add(bankImage);
            orm.SaveChanges();
        }
    

        //Author:Raj Kumar M, Purpose:To add new Profile; Date:21-02-2023
        public static void AddEntity(DomainEntities.Entity model, IEnumerable<string> crop, string number)
        {

            EpicCrmEntities orm = DBLayer.GetOrm;
            DBModel.EntityNumber entityNumber;
            DBModel.Day dt = new DBModel.Day();
            DateTime ts = DateTime.UtcNow;
            var date = ts.ToString("yyyy/MM/dd");

            var dayId = orm.Days.Where(x => x.DATE.ToString() == date).Select(x => x.Id).FirstOrDefault();

            using (TransactionScope scope = new
                         TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                         {
                             IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                         }))
            {
                var entityNum = orm.EntityNumbers.Where(x => x.IsUsed == false).OrderBy(x => x.Sequence)
                                    .Select(x => x.EntityNumber1).First();
                entityNumber = orm.EntityNumbers.Where(x => x.EntityNumber1 == entityNum).FirstOrDefault();
                entityNumber.IsUsed = true;
                orm.EntityNumbers.AddOrUpdate(entityNumber);

                if (dayId == 0)
                {
                    dt.DATE = Convert.ToDateTime(date);
                    orm.Days.Add(dt);
                    orm.SaveChanges();
                    dayId = orm.Days.Where(x => x.DATE.ToString() == date).Select(x => x.Id).FirstOrDefault();
                }

                scope.Complete();
            }

            DBModel.Entity entity = new DBModel.Entity()
            {
                EmployeeId = model.EmployeeId,
                DayId = dayId,
                HQCode = model.HQCode,
                AtBusiness = false,
                EntityType = "Farmer",
                ApproveDate = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                EntityName = model.EntityName,
                EntityDate = DateTime.UtcNow,
                DateCreated = DateTime.UtcNow,
                LandSize = model.LandSize,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                ContactCount =1,
                ApprovedBy="",
                CropCount = model.CropCount,
                UniqueIdType = "Aadhar",
                TaxId="",
                UniqueId = model.UniqueId,
                UpdatedBy = model.CurrentUser,
                CreatedBy = model.CurrentUser,
                FatherHusbandName = model.FatherHusbandName,
                HQName = model.HQName,
                TerritoryCode = model.TerritoryCode,
                TerritoryName = model.TerritoryName,
                EntityNumber = entityNumber.EntityNumber1,
                IsActive = true
            };

            DBModel.Entity res = orm.Entities.Add(entity);

            DBModel.EntityContact entityContact = new DBModel.EntityContact()
            {
                EntityId = res.Id,
                Name = model.EntityName,
                PhoneNumber = number,
                IsPrimary = true
            };
            orm.EntityContacts.Add(entityContact);

            foreach (var obj in crop)
            {
                DBModel.EntityCrop cropEntity = new DBModel.EntityCrop()
                {
                    EntityId = res.Id,
                    CropName = obj
                };
                orm.EntityCrops.Add(cropEntity);
            }
            orm.SaveChanges();
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        public static ICollection<DomainEntities.EntityAgreementForIR> GetEntityAgreementsForIR(long entityId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            return orm.EntityAgreements.Where(x => x.EntityId == entityId && x.Status == "Approved")
                    .Select(x => new DomainEntities.EntityAgreementForIR()
                    {
                        Id = x.Id,
                        TypeName = x.WorkflowSeason.TypeName,
                        AgreementNumber = x.AgreementNumber
                    }).ToList();
        }


        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        public static ICollection<DomainEntities.Entity> GetAssociatedEntityName(string hqCode)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.Entity> associatedEntityName = (from en in orm.Entities
                                                                      where en.HQCode == hqCode
                                                                      select new DomainEntities.Entity()
                                                                      {
                                                                          Id = en.Id,
                                                                          EmployeeId = en.EmployeeId,
                                                                          EntityName = en.EntityName,
                                                                          IsActive = en.IsActive == true,
                                                                          EntityType = en.EntityType                                                                 
                                                                      });
            return associatedEntityName.ToList();
        }
        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:15-02-2023
        public static ICollection<DomainEntities.IssueReturn> GetTypeName(long aggId)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;

            IQueryable<DomainEntities.IssueReturn> associatedTypeName = (from ea in orm.EntityAgreements
                                                                         join wfs in orm.WorkflowSeasons
                                                                         on ea.WorkflowSeasonId equals wfs.Id

                                                                         join imtn in orm.ItemMasterTypeNames
                                                                         on wfs.TypeName equals imtn.TypeName
                                                                         join im in orm.ItemMasters on imtn.ItemMasterId equals im.Id
                                                                         where ea.Id == aggId && im.IsActive == true
                                                                         select new DomainEntities.IssueReturn()
                                                                         {
                                                                             TypeName = im.Category,
                                                                             ItemType = im.ItemDesc,
                                                                             Rate = imtn.Rate,
                                                                             ReturnRate = imtn.ReturnRate,
                                                                             ItemMasterId = imtn.ItemMasterId,
                                                                             ItemUnit = im.Unit
                                                                         });
            return associatedTypeName.ToList();
        }

        //Author:Gowtham S, Purpose:Add Issue/Reteruns; Date:17-02-2023
        public static void SaveAddIssueReturnsDetails(DomainEntities.IssueReturn sp, string currentUser)
        {
            EpicCrmEntities orm = DBLayer.GetOrm;
            DBModel.Day dt = new DBModel.Day();

            DateTime ts = DateTime.UtcNow;
            var date = ts.ToString("yyyy/MM/dd");
            string curYear = ts.Year.ToString().Substring(2, 2).ToString();
            var dayid = orm.Days.Where(x => x.DATE.ToString() == date).Select(x => x.Id).FirstOrDefault();

            if (dayid == 0)
            {
                using (TransactionScope scope = new
                         TransactionScope(TransactionScopeOption.Required, new TransactionOptions
                         {
                             IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                         }))
                {

                    dt.DATE = Convert.ToDateTime(date);
                    orm.Days.Add(dt);
                    orm.SaveChanges();
                    dayid = orm.Days.Where(x => x.DATE.ToString() == date).Select(x => x.Id).FirstOrDefault();
                    scope.Complete();
                }
            }
            DBModel.Entity Et = new DBModel.Entity();
            Et.DayId = dayid;

            DBModel.IssueReturn sp1 = new DBModel.IssueReturn()
            {
                AppliedItemMasterId = sp.ItemMasterId,
                ItemMasterId = sp.ItemMasterId,
                Quantity = sp.Quantity,
                AppliedQuantity = sp.Quantity,
                ItemRate = sp.ItemRate,
                AppliedItemRate = sp.AppliedItemRate,
                EntityAgreementId = sp.EntityAgreementId,
                EntityId = sp.EntityId,
                TransactionType = sp.TransactionType,
                AppliedTransactionType = sp.TransactionType,
                TransactionDate = sp.TransactionDate,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                SqliteIssueReturnId = 1,
                EmployeeId = sp.EmployeeId,
                Status = "Pending",
                SlipNumber = "IR"+curYear+"/"+ sp.SlipNumber,
                Comments = sp.Comments,
                DayId = dayid,
                ActivityId = 0,
                LandSizeInAcres = Convert.ToDecimal(0),
                CreatedBy = currentUser,
                UpdatedBy = currentUser,               
                CyclicCount = 0
            };
            orm.IssueReturns.Add(sp1);
            orm.SaveChanges();       
        }

    }
}

