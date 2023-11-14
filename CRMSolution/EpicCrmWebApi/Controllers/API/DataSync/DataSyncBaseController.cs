using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EpicCrmWebApi
{
    /// <summary>
    /// This API is used to sync data from phone
    /// </summary>
    public class DataSyncBaseController : CrmBaseController
    {
        protected IEnumerable<SqliteDomainAction> FillDomainActions(SqliteData sqliteDataObject, DeviceInfo di, Func<SqliteBase, IEnumerable<string>> funcToGetImageFileNames)
        {
            if (sqliteDataObject == null || sqliteDataObject.SqliteActionDataCollection == null)
            {
                return null;
            }

            return sqliteDataObject.SqliteActionDataCollection.Select(x => new SqliteDomainAction()
            {
                Id = 0,
                PhoneDbId = Utils.TruncateString(x.Id,50),
                TimeStamp = x.TimeStamp,
                ActivityTrackingType = x.ActivityTrackingType,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                MNC = x.MNC,
                MCC = x.MCC,
                LAC = x.LAC,
                CellId = x.CellId,
                ActivityType = Utils.TruncateString(x.ActivityType,50),
                ClientType = Utils.TruncateString(x.ClientType,50),
                ClientName = Utils.TruncateString(x.ClientName,50),
                ClientPhone = Utils.TruncateString(x.ClientPhone,20),
                Comments = Utils.TruncateString(x.Comments,2048),
                Images = funcToGetImageFileNames(x),
                //Images = x.Images != null ? x.Images.Select(y => {
                //    return Business.SaveImageDataInFile(y.BinaryData, Helper.ActivityImageFilePrefix);
                //}
                //).ToList() : null,
                Contacts = x.Contacts?.Select(y => new SqliteDomainActionContact()
                {
                    Name = Utils.TruncateString(y.Name, 50),
                    PhoneNumber = Utils.TruncateString(y.PhoneNumber, 20),
                    IsPrimary = y.IsPrimary
                }).ToList(),
                IMEI = di.IMEI,
                AtBusiness = x.AtBusiness,
                LocationTaskStatus = Utils.TruncateString(x.LocationTaskStatus, 50),
                LocationException = Utils.TruncateString(x.LocationException, 256),
                ClientCode = Utils.TruncateString(x.ClientCode, 50),
                InstrumentId = Utils.TruncateString(x.InstrumentId, 50),
                ActivityAmount = x.ActivityAmount,
                PhoneModel = Utils.TruncateString(di.Model,100),
                PhoneOS = Utils.TruncateString(di.OSVersion,10),
                AppVersion = Utils.TruncateString(di.AppVersion,10),
                Locations = x.Locations?.Select(y => new SqliteDomainActionLocation()
                {
                    Source = Utils.TruncateString(y.Source, 50),
                    Latitude = y.Latitude,
                    Longitude = y.Longitude,
                    UtcAt = y.UtcAt,
                    LocationTaskStatus = Utils.TruncateString(y.LocationTaskStatus, 50),
                    LocationException = Utils.TruncateString(y.LocationException, 256),
                    IsGood = y.IsGood
                }).ToList()
            }).ToList();
        }

        /// <summary>
        /// Copy payment data to domain object
        /// </summary>
        /// <param name="sqliteDataObject"></param>
        /// <returns></returns>
        protected IEnumerable<SqliteDomainPayment> FillDomainPaymentObject(SqliteData sqliteDataObject, Func<SqliteBase, IEnumerable<string>> funcToGetImageFileNames)
        {
            IEnumerable<SqliteDomainPayment> domainPayments = null;
            if (sqliteDataObject.SqlitePayments != null)
            {
                domainPayments = sqliteDataObject.SqlitePayments.Select(x => new SqliteDomainPayment()
                {
                    PhoneDbId = x.Id ?? "",
                    ActivityId = x.ActivityId ?? "",
                    CustomerCode = Utils.TruncateString(x.CustomerCode,50),
                    PaymentMode = Utils.TruncateString(x.PaymentMode,10),
                    TimeStamp = x.TimeStamp,
                    TotalAmount = x.TotalAmount,
                    Comment = Utils.TruncateString(x.Comment,2000),
                    Images = funcToGetImageFileNames(x)
                    //Images = x.Images != null ? x.Images.Select(y=> Business.SaveImageDataInFile(y.BinaryData, Helper.PaymentImageFilePrefix)).ToList() : null
                }).ToList();
            }

            return domainPayments;
        }

        /// <summary>
        /// Copy order data to domain object
        /// </summary>
        /// <param name="sqliteDataObject"></param>
        /// <returns></returns>
        protected IEnumerable<SqliteDomainOrder> FillDomainOrderObject(SqliteData sqliteDataObject, Func<SqliteBase, IEnumerable<string>> funcToGetImageFileNames)
        {
            IEnumerable<SqliteDomainOrder> domainOrders = null;

            bool isLegacy = Business.IsDataComingFromLegacyApp(sqliteDataObject.DeviceInfo?.AppVersion ?? "");

            if (sqliteDataObject.SqliteOrders != null )
            {
                domainOrders = sqliteDataObject.SqliteOrders.Select(x => new SqliteDomainOrder()
                {
                    PhoneDbId = x.Id ?? "",
                    ActivityId = x.ActivityId ?? "",
                    CustomerCode = Utils.TruncateString(x.CustomerCode,50),
                    TimeStamp = x.TimeStamp,
                    TotalAmount = x.TotalAmount,
                    TotalGST = x.TotalGST,
                    NetAmount = x.NetAmount,
                    DiscountType = Utils.TruncateString(x.DiscountType,50),
                    MaxDiscountPercentage = x.MaxDiscountPercentage,
                    AppliedDiscountPercentage = x.AppliedDiscountPercentage,
                    ItemCount = x.Items != null ? x.Items.Count() : 0,
                    Items = x.Items != null ? x.Items.Select(y=> new SqliteDomainOrderItem()
                    {
                        ProductCode = Utils.TruncateString(y.ProductCode,50),
                        Quantity = y.Quantity,
                        BillingPrice = y.BillingPrice,
                        DiscountPercent = y.DiscountPercent,
                        DiscountedPrice = y.DiscountedPrice,
                        ItemPrice = y.ItemPrice,
                        GstPercent = y.GstPercent,
                        GstAmount = y.GstAmount,
                        NetPrice = y.NetPrice,
                        Amount = y.BillingPrice * y.Quantity  // legacy apk (v1.3 and below)
                    }).ToList() : null,
                    Images = funcToGetImageFileNames(x),
                    //Images = x.Images?.Select(y => {
                    //    return Business.SaveImageDataInFile(y.BinaryData, Helper.OrderImageFilePrefix);
                    //}).ToList(),
                }).ToList();

                // Make some fixes in data coming from legacy apks
                if( isLegacy )
                {
                    domainOrders.Select(x =>
                    {
                        x.NetAmount = x.TotalAmount;
                        x.Items.Select(y =>
                        {
                            y.DiscountPercent = 0;
                            y.DiscountedPrice = y.BillingPrice;
                            y.ItemPrice = y.DiscountedPrice * y.Quantity;
                            y.GstPercent = 0;
                            y.GstAmount = 0;
                            y.NetPrice = y.ItemPrice;
                            return 1;
                        }).Count();
                        return 1;
                    }).Count();
                }
            }

            return domainOrders;
        }

        protected IEnumerable<long> FillDomainCancelledLeavesObject(SqliteData sqliteDataObject)
        {
            IEnumerable<long> domainCancelledLeaves = null;

            if (sqliteDataObject.SqliteCancelledLeaves != null)
            {
                domainCancelledLeaves = sqliteDataObject.SqliteCancelledLeaves.Select(x => x).ToList();
            }

            return domainCancelledLeaves;
        }

        protected IEnumerable<SqliteDomainLeave> FillDomainLeavesObject(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainLeave> domainLeaves = null;

            if (sqliteDataObject.SqliteLeaves != null)
            {
                domainLeaves = sqliteDataObject.SqliteLeaves.Select(x => new SqliteDomainLeave()
                {
                    PhoneDbId = x.Id ?? "",
                  
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    LeaveType = Utils.TruncateString(x.LeaveType, 50),
                    LeaveReason = Utils.TruncateString(x.LeaveReason,50),
                    Comment = Utils.TruncateString(x.Comment, 500),
                    DaysCountExcludingHolidays=x.DaysCountExcludingHolidays,
                    DaysCount=x.DaysCount,
                    TimeStamp=x.TimeStamp,
                }).ToList();
            }

            return domainLeaves;
        }

        protected IEnumerable<SqliteDomainIssueReturn> FillDomainIssueReturns(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainIssueReturn> domainIssueReturns = null;

            if (sqliteDataObject.SqliteIssueReturns != null)
            {
                domainIssueReturns = sqliteDataObject.SqliteIssueReturns.Select(x => new SqliteDomainIssueReturn()
                {
                    IsNewEntity = x.IsNewEntity,
                    IsNewAgreement = x.IsNewAgreement,
                    EntityId = x.EntityId,
                    EntityName = Utils.TruncateString(x.EntityName, 50),
                    AgreementId = x.AgreementId,
                    Agreement = Utils.TruncateString(x.AgreementNumber, 50),
                    ParentReferenceId = Utils.TruncateString(x.ParentReferenceId, 50),

                    TranType = Utils.TruncateString(x.TranType, 50),
                    ItemId = x.ItemId,
                    ItemCode = Utils.TruncateString(x.ItemCode, 100),
                    SlipNumber = Utils.TruncateString(x.SlipNumber, 50),
                    Acreage = x.Acreage,
                    Quantity = x.Quantity,
                    TimeStamp = x.TimeStamp,
                    ActivityId = Utils.TruncateString(x.ActivityId, 50),
                    ItemRate = x.ItemRate,
                }).ToList();
            }

            return domainIssueReturns;
        }

        protected IEnumerable<SqliteDomainAdvanceRequest> FillDomainAdvanceRequests(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainAdvanceRequest> domainAdvanceRequests = null;

            if (sqliteDataObject.SqliteAdvanceRequests != null)
            {
                domainAdvanceRequests = sqliteDataObject.SqliteAdvanceRequests
                        .Select(x => new SqliteDomainAdvanceRequest()
                {
                    IsNewEntity = x.IsNewEntity,
                    IsNewAgreement = x.IsNewAgreement,
                    EntityId = x.EntityId,
                    EntityName = Utils.TruncateString(x.EntityName, 50),
                    AgreementId = x.AgreementId,
                    Agreement = Utils.TruncateString(x.AgreementNumber, 50),
                    ParentReferenceId = Utils.TruncateString(x.ParentReferenceId, 50),

                    Amount = x.Amount,
                    TimeStamp = x.TimeStamp,
                    Notes = Utils.TruncateString(x.Notes, 512),
                    ActivityId = Utils.TruncateString(x.ActivityId, 50)
                }).ToList();
            }

            return domainAdvanceRequests;
        }

        protected IEnumerable<SqliteDomainTerminateAgreementData> FillDomainTerminateAgreementRequests(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainTerminateAgreementData> domainTerminateAgreementRequests = null;

            if (sqliteDataObject.SqliteTerminateAgreementData != null)
            {
                domainTerminateAgreementRequests = sqliteDataObject.SqliteTerminateAgreementData
                        .Select(x => new SqliteDomainTerminateAgreementData()
                        {
                            EntityId = x.EntityId,
                            EntityName = Utils.TruncateString(x.EntityName, 50),
                            AgreementId = x.AgreementId,
                            Agreement = Utils.TruncateString(x.Agreement, 50),
                            TimeStamp = x.TimeStamp,
                            TypeName = Utils.TruncateString(x.TypeName, 50),
                            ActivityId = Utils.TruncateString(x.ActivityId, 50),
                            Reason = Utils.TruncateString(x.Reason, 50),
                            Notes = Utils.TruncateString(x.Notes, 512),
                        }).ToList();
            }

            return domainTerminateAgreementRequests;
        }

        protected IEnumerable<SqliteDomainAgreement> FillDomainAgreements(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainAgreement> domainAgreements = null;

            if (sqliteDataObject.SqliteAgreements != null)
            {
                domainAgreements = sqliteDataObject.SqliteAgreements.Select(x => new SqliteDomainAgreement()
                {
                    PhoneDbId = x.Id,
                    IsNewEntity = x.IsNewEntity,
                    EntityId = x.EntityId,
                    EntityName = Utils.TruncateString(x.EntityName, 50),
                    ParentReferenceId = x.ParentReferenceId,
                    SeasonName = Utils.TruncateString(x.SeasonName,50),
                    TypeName = Utils.TruncateString(x.TypeName,50),
                    Acreage = x.Acreage,
                    TimeStamp = x.TimeStamp,
                    ActivityId = Utils.TruncateString(x.ActivityId,50),
                    TerritoryCode=x.TerritoryCode,
                    TerritoryName=x.TerritoryName,
                    HQCode=x.HQCode,
                    HQName=x.HQName,    
                }).ToList();
            }

            return domainAgreements;
        }

        protected IEnumerable<SqliteDomainSurvey> FillDomainSurveys(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainSurvey> domainSurveys = null;

            if (sqliteDataObject.SqliteSurveys != null)
            {
                domainSurveys = sqliteDataObject.SqliteSurveys.Select(x => new SqliteDomainSurvey()
                {
                    PhoneDbId = x.Id,
                    IsNewEntity = x.IsNewEntity,
                    EntityId = x.EntityId,
                    EntityName = Utils.TruncateString(x.EntityName, 50),
                    ParentReferenceId = x.ParentReferenceId,

                    SeasonName = Utils.TruncateString(x.SeasonName, 50),
                    SowingType = Utils.TruncateString(x.SowingType, 50),
                    Acreage = x.Acreage,

                    MajorCrop = Utils.TruncateString(x.MajorCrop, 50),
                    LastCrop = Utils.TruncateString(x.LastCrop, 50),
                    WaterSource = Utils.TruncateString(x.WaterSource, 50),
                    SoilType = Utils.TruncateString(x.SoilType, 50),
                    SowingDate = x.SowingDate,

                    TimeStamp = x.TimeStamp,
                    ActivityId = Utils.TruncateString(x.ActivityId, 50)
                }).ToList();
            }

            return domainSurveys;
        }

        protected IEnumerable<SqliteDomainDayPlanTarget> FillDomainDayPlanTarget(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainDayPlanTarget> domainDayPlanTarget = null;

            if (sqliteDataObject.SqliteDayPlan != null)
            {
                domainDayPlanTarget = sqliteDataObject.SqliteDayPlan.Select(x => new SqliteDomainDayPlanTarget()
                {
                    PhoneDbId = x.Id,
                    PlanTimeStamp = x.DayPlanningTimeStamp,
                    TargetSales = x.TargetSales,
                    TargetCollection = x.TargetCollection,
                    TargetNewDealerAppointment = x.TargetNewDealerAppointment,
                    TargetDemoActivity = x.TargetDemoActivity,
                    TargetVigoreSales = x.TargetVigoreSales
                }).ToList();
            }

            return domainDayPlanTarget;
        }

        protected IEnumerable<SqliteDomainWorkFlowPageData> FillDomainWorkFlowPageData(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainWorkFlowPageData> domainWFPData = null;

            if (sqliteDataObject.SqliteWorkFlowPageData != null)
            {
                domainWFPData = sqliteDataObject.SqliteWorkFlowPageData.Select(x => new SqliteDomainWorkFlowPageData()
                {
                    EntityId = x.EntityId,
                    EntityType = Utils.TruncateString(Utils.SiteConfigData.WorkflowActivityEntityType, 50),
                    EntityName = Utils.TruncateString(x.EntityName, 50),
                    AgreementId = x.AgreementId,
                    Agreement = Utils.TruncateString(x.Agreement, 50),
                    EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,

                    TimeStamp = x.TimeStamp,
                    TypeName = Utils.TruncateString(x.TypeName, 50),
                    TagName = Utils.TruncateString(x.TagName, 50),
                    CurrentPhase = Utils.TruncateString(x.CurrentPhase, 50),
                    PhaseStarted = x.PhaseStarted,
                    PhaseDate = x.PhaseDate,

                    // fields for sowing
                    SeedType = Utils.TruncateString(x.SeedType, 50),
                    SeedQuantity = x.SeedQuantity,
                    GapFillingRequired = x.GapFillingRequired,
                    GapFillingSeedQuantity = x.GapFillingSeedQuantity,

                    // fields for First Harvest
                    HarvestLaborCount = x.HarvestLaborCount,

                    // fields for other
                    PercentCompleted = x.PercentCompleted,

                    ActivityId = Utils.TruncateString(x.ActivityId, 50),

                    FollowUps = x.FollowUps?.Select(y => new SqliteDomainWorkFlowFollowUp()
                    {
                        Phase = Utils.TruncateString(y.Phase, 50),
                        StartDate = y.StartDate,
                        EndDate = y.EndDate,
                        Notes = Utils.TruncateString(y.Notes, 100)
                    }).ToList(),

                    // April 11 2020
                    BatchNumber = Utils.TruncateString(x.BatchNumber, 50),
                    LandSize = Utils.TruncateString(x.LandSize, 10),
                    ItemCount = x.ItemCount,
                    DWSEntry = Utils.TruncateString(x.DWSEntry, 50),
                    YieldExpected = x.YieldExpected,
                    BagsIssued = x.BagsIssued,
                    HarvestDate = x.HarvestDate,
                    ItemsUsed = x.ItemsUsed?.Select(y => Utils.TruncateString(y, 50)).ToList()
                }).ToList();

                // March 28 2020
                // to support older apk versions, fill tag name if coming null or empty
                domainWFPData.Select(x =>
                {
                    if (String.IsNullOrEmpty(x.TagName))
                    {
                        x.TagName = x.CurrentPhase;
                    }
                    return 1;
                }).ToList();
            }

            return domainWFPData;
        }

        /// <summary>
        /// Copy order Return data to domain object
        /// </summary>
        /// <param name="sqliteDataObject"></param>
        /// <returns></returns>
        protected IEnumerable<SqliteDomainReturnOrder> FillDomainOrderReturnsObject(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainReturnOrder> domainReturnOrders = null;

            if (sqliteDataObject.SqliteOrderReturns != null)
            {
                domainReturnOrders = sqliteDataObject.SqliteOrderReturns.Select(x => new SqliteDomainReturnOrder()
                {
                    PhoneDbId = x.Id ?? "",
                    CustomerCode = Utils.TruncateString(x.CustomerCode,50),
                    TimeStamp = x.TimeStamp,
                    TotalAmount = x.TotalAmount,
                    Comment =  Utils.TruncateString(x.Comment,2000),
                    ReferenceNum = Utils.TruncateString(x.ReferenceNum, 255),
                    ItemCount = x.Items != null ? x.Items.Count() : 0,
                    Items = x.Items != null ? x.Items.Select(y => new SqliteDomainReturnOrderItem()
                    {
                        BillingPrice = y.BillingPrice,
                        ProductCode = Utils.TruncateString(y.ProductCode.Trim(),50),
                        Quantity = y.Quantity,
                        Comment = Utils.TruncateString(y.Comment,2000),
                        ItemPrice = y.ItemPrice
                    }).ToList() : null,
                    ActivityId = x.ActivityId ?? ""
                }).ToList();
            }

            return domainReturnOrders;
        }

        /// <summary>
        /// Copy expense data to domain object
        /// </summary>
        /// <param name="sqliteDataObject"></param>
        /// <returns></returns>
        protected static SqliteDomainExpense FillDomainExpenseObject(SqliteData sqliteDataObject, Func<SqliteBase, IEnumerable<string>> funcToGetImageFileNames)
        {
            SqliteDomainExpense expenseObject = null;
            if (sqliteDataObject.SqliteExpense != null)
            {
                expenseObject = new SqliteDomainExpense()
                {
                    Id = 0,
                    TimeStamp = sqliteDataObject.SqliteExpense.TimeStamp,
                    TotalAmount = sqliteDataObject.SqliteExpense.TotalAmount,
                    ExpenseItemCount = sqliteDataObject.SqliteExpense.ExpenseItems != null
                                        ? sqliteDataObject.SqliteExpense.ExpenseItems.Count() : 0,
                    ExpenseItems = null
                };

                if (expenseObject.ExpenseItemCount > 0 && sqliteDataObject.SqliteExpense.ExpenseItems != null)
                {
                    expenseObject.ExpenseItems = sqliteDataObject.SqliteExpense.ExpenseItems.Select(x => new SqliteDomainExpenseItem()
                    {
                        ExpenseType = Utils.TruncateString(x.ExpenseType,50),
                        Amount = x.Amount,
                        Comment = Utils.TruncateString(x.Comment,2000),
                        OdometerStart = x.ExpenseItemDetail != null ? x.ExpenseItemDetail.OdometerStart : 0,
                        OdometerEnd = x.ExpenseItemDetail != null ? x.ExpenseItemDetail.OdometerEnd : 0,
                        VehicleType = Utils.TruncateString(x.ExpenseItemDetail?.VehicleType,50),
                        FuelType = Utils.TruncateString(x.ExpenseItemDetail?.FuelType,50),
                        FuelQuantityInLiters = x.ExpenseItemDetail != null ? x.ExpenseItemDetail.FuelQuantityInLiters : 0,

                        //Images = x.Images != null ? x.Images.Select(y=> y.BinaryData).ToList() : null
                        //Images = x.Images != null ? x.Images.Select(y => Business.SaveImageDataInFile(y.BinaryData, Helper.ExpenseImageFilePrefix)).ToList() : null,
                        Images = funcToGetImageFileNames(x)
                    }
                    ).ToList();
                }
            }

            return expenseObject;
        }

        /// <summary>
        /// Copy entity data to domain object
        /// </summary>
        /// <param name="sqliteDataObject"></param>
        /// <returns></returns>
        protected static IEnumerable<SqliteDomainEntity> FillDomainEntityObject(SqliteData sqliteDataObject, Func<SqliteBase, IEnumerable<string>> funcToGetImageFileNames)
        {
            IEnumerable<SqliteDomainEntity> domainEntities = null;

            if (sqliteDataObject.SqliteBusinessEntities != null)
            {
                domainEntities = sqliteDataObject.SqliteBusinessEntities.Select(x => new SqliteDomainEntity()
                {
                    PhoneDbId = x.Id ?? "",
                    AtBusiness = x.AtBusiness,
                    //Consent = x.Consent,   //Swetha Made change on 24-11-2021
                    EntityType = Utils.TruncateString(x.BusinessType,50),
                    EntityName = Utils.TruncateString(x.BusinessName,50),
                    Address = Utils.TruncateString(x.Address,100),
                    City = Utils.TruncateString(x.City,50),
                    State = Utils.TruncateString(x.State,50),
                    Pincode = Utils.TruncateString(x.Pincode,10),
                    LandSize = Utils.TruncateString(x.LandSize,50),
                    TimeStamp = x.TimeStamp,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    LocationTaskStatus = Utils.TruncateString(x.LocationTaskStatus,50),
                    LocationException = Utils.TruncateString(x.LocationException,256),
                    MNC = x.MNC,
                    MCC = x.MCC,
                    LAC = x.LAC,
                    CellId = x.CellId,

                    UniqueIdType = Utils.TruncateString(x.UniqueIdType, 50),
                    UniqueId = Utils.TruncateString(x.UniqueId, 50),
                    TaxId = Utils.TruncateString(x.TaxId, 50),

                    FatherHusbandName = Utils.TruncateString(x.FatherHusbandName, 50),
                    TerritoryCode = Utils.TruncateString(x.TerritoryCode, 10),
                    TerritoryName = Utils.TruncateString(x.TerritoryName, 50),
                    HQCode = Utils.TruncateString(x.HQCode, 10),
                    HQName = Utils.TruncateString(x.HQName, 50),

                    //MajorCrop = Utils.TruncateString(x.MajorCrop, 50),
                    //LastCrop = Utils.TruncateString(x.LastCrop, 50),
                    //WaterSource = Utils.TruncateString(x.WaterSource, 50),
                    //SoilType = Utils.TruncateString(x.SoilType, 50),
                    //SowingType = Utils.TruncateString(x.SowingType, 50),
                    //SowingDate = x.SowingDate.HasValue ? x.SowingDate.Value : DateTime.MinValue,

                    Contacts = x.Contacts?.Select(y => new SqliteDomainEntityContact()
                    {
                        Name = Utils.TruncateString(y.Name, 50),
                        PhoneNumber = Utils.TruncateString(y.PhoneNumber,20),
                        IsPrimary = y.IsPrimary
                    }).ToList(),
                    Crops = x.Crops?.Select(y => new SqliteDomainEntityCrop()
                    {
                        Name = Utils.TruncateString(y.Name, 50)
                    }).ToList(),
                    Images = funcToGetImageFileNames(x),
                    //Images = x.Images?.Select(y => {
                    //    return Business.SaveImageDataInFile(y.BinaryData, Helper.EntityImageFilePrefix);
                    //}).ToList(),

                    Locations = x.Locations?.Select(y => new SqliteDomainActionLocation()
                    {
                        Source = Utils.TruncateString(y.Source, 50),
                        Latitude = y.Latitude,
                        Longitude = y.Longitude,
                        UtcAt = y.UtcAt,
                        LocationTaskStatus = Utils.TruncateString(y.LocationTaskStatus, 50),
                        LocationException = Utils.TruncateString(y.LocationException, 256),
                        IsGood = y.IsGood
                    }).ToList()

                }).ToList();
            }

            return domainEntities;
        }

        protected static IEnumerable<DomainEntities.SqliteDeviceLog> FillDomainDeviceLogs(SqliteData sqliteDataObject)
        {
            if (sqliteDataObject?.DeviceLogs == null)
            {
                return null;
            }

            return sqliteDataObject?.DeviceLogs?.Select(x => new DomainEntities.SqliteDeviceLog()
            {
                TimeStamp = x.TimeStamp,
                LogText = Utils.TruncateString(x.LogText, 255)
            }).ToList();
        }

        protected static IEnumerable<SqliteDomainBankDetail> FillDomainBankDetails(SqliteData sqliteDataObject, Func<SqliteBase, IEnumerable<string>> funcToGetImageFileNames)
        {
            IEnumerable<SqliteDomainBankDetail> domainBankDetails = null;

            if (sqliteDataObject.SqliteBankDetails != null)
            {
                domainBankDetails = sqliteDataObject.SqliteBankDetails.Select(x => new SqliteDomainBankDetail()
                {
                    PhoneDbId = x.Id,
                    IsNewEntity = x.IsNewEntity,
                    EntityId = x.EntityId,
                    EntityName = Utils.TruncateString(x.EntityName, 50),
                    ParentReferenceId = x.ParentReferenceId,

                    IsSelfAccount = x.IsSelfAccount,
                    AccountHolderName = Utils.TruncateString(x.AccountHolderName,50).ToUpper(),
                    AccountHolderPAN = Utils.TruncateString(x.AccountHolderPAN,50).ToUpper(),
                    BankName = Utils.TruncateString(x.BankName,50),
                    BankAccount = Utils.TruncateString(x.BankAccount,50),
                    BankIFSC = Utils.TruncateString(x.BankIFSC,50).ToUpper(),
                    BankBranch = Utils.TruncateString(x.BankBranch,50).ToUpper(),

                    TimeStamp = x.TimeStamp,
                    ActivityId = Utils.TruncateString(x.ActivityId, 50),
                    Images = funcToGetImageFileNames(x),
                }).ToList();
            }

            return domainBankDetails;
        }

        protected static IEnumerable<SqliteDomainSTRData> FillDomainSTRData(SqliteData sqliteDataObject, Func<SqliteBase, IEnumerable<string>> funcToGetImageFileNames)
        {
            IEnumerable<SqliteDomainSTRData> domainSTRData = null;

            if (sqliteDataObject.SqliteSTR != null)
            {
                domainSTRData = sqliteDataObject.SqliteSTR.Select(x => new SqliteDomainSTRData()
                {
                    PhoneDbId = x.Id,

                    STRNumber = Utils.TruncateString(x.STRNumber, 50),
                    VehicleNumber = Utils.TruncateString(x.VehicleNumber, 50),
                    DriverName = Utils.TruncateString(x.DriverName, 50),
                    DriverPhone = Utils.TruncateString(x.DriverPhone, 50),
                    DWSCount = x.DWSCount,
                    BagCount = x.BagCount,
                    GrossWeight = x.GrossWeight,
                    NetWeight = x.NetWeight,
                    StartOdometer = x.StartOdometer,
                    EndOdometer = x.EndOdometer,
                    STRDate = x.STRDate,
                    IsNew = x.IsNew,
                    IsTransferred = x.IsTransferred,
                    TransfereeName = Utils.TruncateString(x.TransfereeName, 50),
                    TransfereePhone = Utils.TruncateString(x.TransfereePhone, 50),
                    TimeStamp = x.TimeStamp,
                    ActivityId = Utils.TruncateString(x.ActivityId, 50),
                    TimeStamp2 = x.TimeStamp2,
                    ActivityId2 = Utils.TruncateString(x.ActivityId2, 50),

                    DomainDWSData = x.DWS?.Select(y => new SqliteDomainDWSData()
                    {
                        EntityId = y.EntityId,
                        EntityName = Utils.TruncateString(y.EntityName, 50),
                        AgreementId = y.AgreementId,
                        Agreement = Utils.TruncateString(y.Agreement, 50),
                        EntityWorkFlowDetailId = y.EntityWorkFlowDetailId,
                        TypeName = Utils.TruncateString(y.TypeName, 50),
                        TagName = Utils.TruncateString(y.TagName, 50),

                        DWSNumber = y.DWSNumber,
                        BagCount = y.BagCount,
                        FilledBagsWeightKg = y.FilledBagsWeightKg,
                        EmptyBagsWeightKg = y.EmptyBagsWeightKg,

                        TimeStamp = y.TimeStamp,
                        ActivityId = y.ActivityId
                    }).ToList(),

                    Images = funcToGetImageFileNames(x),
                }).ToList();
            }

            return domainSTRData;
        }

        /// <summary>
        /// Author: Rajesh V; Date24-06-2021 ; Purpose: dealer Questionnaire
        /// </summary>
        /// <param name="sqliteDataObject"></param>
        /// <returns></returns>
        protected IEnumerable<SqliteDomainQuestionnaire> FillDomainQuestionnaire(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainQuestionnaire> domainQuestionnaire = null;

            if (sqliteDataObject.SqliteQuestionnaireData != null)
            {
                domainQuestionnaire = sqliteDataObject.SqliteQuestionnaireData.Select(x => new SqliteDomainQuestionnaire()
                {
                    PhoneDbId = x.Id,
                    IsNewEntity = x.IsNewEntity,
                    EntityId = Convert.ToInt64(x.EntityId),
                    EntityName = Utils.TruncateString(x.EntityName, 50),
                    QuestionnaireDate = x.Date,
                    SqliteQuestionPaperName = Utils.TruncateString(x.Name, 50),
                    SqliteQuestionPaperId = Convert.ToInt64(x.QuestionPaperId),
                    ActivityId = Utils.TruncateString(x.ActivityId, 50),
                    ParentReferenceId = x.ParentReferenceId,
                    DateCreated = x.TimeStamp,

                    Answers = x.Answers?.Select(y => new SqliteDomainAnswer()
                    {
                        Id = Convert.ToInt64(y.Id),
                        CrossRefId = Convert.ToInt64(y.CrossRefId),
                        QuestionPaperQuestionId = Convert.ToInt64(y.QuestionPaperQuestionId),
                        QuestionTypeName = y.QuestionTypeName,
                        DescriptiveAnswer = y.DescriptiveAnswer,
                        HasTextComment = y.AdditionalComment,
                        TextComment = y.Comments,

                        DomainAnswerDetail = y.AnswerChoices?.Select(z => new SqliteDomainAnswerDetail()
                        {
                            Id = Convert.ToInt64(z.Id),
                            AnswerId = Convert.ToInt64(y.Id),
                            SqliteQuestionPaperQuestionId = Convert.ToInt64(y.QuestionPaperQuestionId),
                            SqliteQuestionPaperAnswerId = Convert.ToInt64(z.Id),
                            IsAnswerChecked = z.IsAnswerChecked
                        }).ToList(),
                    }).ToList(),
                }).ToList();
            }
            return domainQuestionnaire;
        }

        protected IEnumerable<SqliteDomainTask> FillDomainTask(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainTask> domainTask = null;

            if (sqliteDataObject.SqliteFollowUpTaskData != null)
            {
                domainTask = sqliteDataObject.SqliteFollowUpTaskData.Select(x => new SqliteDomainTask()
                {
                    PhoneDbId = x.Id,
                    IsNewEntity = x.IsNewEntity,
                    ParentReferenceId = x.ParentReferenceId,
                    ProjectId = x.ProjectId,
                    ProjectName = Utils.TruncateString(x.ProjectName, 50),
                    Description = Utils.TruncateString(x.Description, 200),
                    ActivityType = Utils.TruncateString(x.ActivityType, 50),
                    ClientType = Utils.TruncateString(x.ClientType, 50),
                    ClientName = Utils.TruncateString(x.ClientName, 50),
                    ClientCode = Utils.TruncateString(x.ClientCode, 50),
                    PlannedStartDate = x.FollowUpStartDate,
                    PlannedEndDate = x.FollowUpEndDate,
                    Comments = Utils.TruncateString(x.Comments, 2000),
                    Status = Utils.TruncateString(x.FollowUpStatus, 50),
                    TimeStamp = x.TimeStamp,
                    NotificationDate = x.NotificationDate
                }).ToList();
            }

            return domainTask;
        }

        protected IEnumerable<SqliteDomainTaskAction> FillDomainTaskAction(SqliteData sqliteDataObject)
        {
            IEnumerable<SqliteDomainTaskAction> domainTaskAction = null;

            if (sqliteDataObject.SqliteFollowUpTaskActionData != null)
            {
                domainTaskAction = sqliteDataObject.SqliteFollowUpTaskActionData.Select(x => new SqliteDomainTaskAction()
                {
                    PhoneDbId = x.Id,
                    IsNewTask = x.IsNewTask,
                    TaskId = x.TaskId,
                    ParentReferenceTaskId = x.ParentReferenceTaskId,
                    TaskAssignmentId = x.TaskAssignmentId,
                    SqliteActionPhoneDbId = x.SqliteActionPhoneDbId,
                    Status = Utils.TruncateString(x.Status, 50),
                    TimeStamp = x.TimeStamp,
                    NotificationDate = x.NotificationDate
                }).ToList();
            }

            return domainTaskAction;
        }
    }
}
