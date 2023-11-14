using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EpicCrmWebApi
{
    public class DownloadController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId">Tenant Employee Id</param>
        /// <returns></returns>
        [HttpGet]
        public DownloadDataResponse GetData(long employeeId, string IMEI = "", string areaCode = "", string appVersion = "")
        {
            return CreateDownloadObject(employeeId, IMEI, areaCode, appVersion);
        }

        internal static DownloadDataResponse CreateDownloadObject(
                                                long employeeId, 
                                                string IMEI = "", 
                                                string areaCode = "",
                                                string appVersion = "")
        {
            DownloadDataResponse response = new DownloadDataResponse();

            try
            {
                if (Business.IsRequestSourceValid(employeeId, IMEI) == false)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = $"App is not supported on this device: {employeeId} | {IMEI}";
                    response.EraseData = true;
                    return response;
                }

                // user must be active
                if (!Business.IsUserAllowed(employeeId))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Content = $"User {employeeId} does not exist or not active.";
                    response.EraseData = true;
                    return response;
                }

                if (!String.IsNullOrEmpty(appVersion))
                {
                    if (!Business.IsAppVersionSupported(appVersion, Helper.GetCurrentIstDateTime()))
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.Content = $"App version {appVersion} is not supported.";
                        response.EraseData = true;
                        return response;
                    }
                }

                // retrieve staff code
                TenantEmployee te = Business.GetTenantEmployee(employeeId);
                string staffCode = "";
                if (te == null)
                {
                    Business.LogError($"{nameof(CreateDownloadObject)}", $"TenantEmployee not found for {employeeId}");
                    staffCode = "";
                }
                else
                {
                    staffCode = te.EmployeeCode;
                }

                Helper.FillResponseCodeTableData(response, staffCode);

                string validatedAreaCode = Business.ValidateAreaCode(employeeId, areaCode);

                IEnumerable<DownloadCustomerExtend> Customers = Business.GetDownloadCustomers(staffCode, validatedAreaCode);
                response.Customers = Customers.Where(x => x.Active)
                    .OrderBy(x => x.Name)
                    .Select(x => new DownloadCustomerModel()
                    {
                        Code = x.Code,
                        Name = x.Name,
                        PhoneNumber = x.PhoneNumber,
                        Type = x.Type,
                        HQCode = x.HQCode,

                        CreditLimit = (long)Decimal.Round(x.CreditLimit, 0),
                        Outstanding = (long)Decimal.Round(x.Outstanding, 0),
                        LongOutstanding = (long)Decimal.Round(x.LongOutstanding, 0),
                        Target = (long)Decimal.Round(x.Target, 0),
                        Sales = (long)Decimal.Round(x.Sales, 0),
                        Payment = (long)Decimal.Round(x.Payment, 0),
                        SalesPercentage = (x.Target > 0) ? (int)Decimal.Round((x.Sales / x.Target * 100), 0) : 0,
                        PaymentPercentage = (x.Target > 0) ? (int)Decimal.Round((x.Payment / x.Target * 100), 0) : 0,
                        Latitude = x.Latitude,
                        Longitude = x.Longitude
                    }).ToList();

                ConsolidatedCustomerData ccd = Business.GetConsolidatedCustomerDownloadInfo(staffCode);
                long denominatorToConvertToLakhs = 100000;
                response.ConsolidatedCustomerInfo = new DownloadConsolidatedCustomerModel()
                {
                    CustomerCount = ccd.CustomerCount,
                    Outstanding = Decimal.Round(ccd.Outstanding / denominatorToConvertToLakhs, 2),
                    LongOutstanding = Decimal.Round(ccd.LongOutstanding / denominatorToConvertToLakhs, 2),
                    Payment = Decimal.Round(ccd.Payment / denominatorToConvertToLakhs, 2),
                    Sales = Decimal.Round(ccd.Sales / denominatorToConvertToLakhs, 2),
                    Target = Decimal.Round(ccd.Target / denominatorToConvertToLakhs, 2),
                    SalesPercentage = (ccd.Target > 0) ? (int)Decimal.Round(ccd.Sales / ccd.Target * 100, 0) : 0,
                    PaymentPercentage = (ccd.Target > 0) ? (int)Decimal.Round(ccd.Payment / ccd.Target * 100, 0) : 0
                };

                response.Products = Business.GetProductsWithPrice2(employeeId, areaCode, Helper.GetCurrentIstDateTime())
                                    .OrderBy(x => x.Name)
                                    .ToList();

                response.BankAccounts = Business.GetDownloadBankAccounts(employeeId, areaCode)
                                    .Where(x=> x.IsActive)
                                    .OrderBy(x=> x.BankName)
                                    .Select(x => new DownloadBankAccountModel()
                                    {
                                        BankName = x.BankName,
                                        BankPhone = x.BankPhone,
                                        AccountNumber = x.AccountNumber,
                                        IFSC = x.IFSC,
                                        Branch = x.BranchName
                                    }).ToList();

                response.Leaves = Business.GetDownloadLeaves(employeeId)
                    .Select(x => new DownloadLeaveModel()
                    {
                        Id = x.Id,
                        
                        StartDate = x.StartDate.ToString("yyyy-MM-dd"),
                        EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                        DaysCountExcludingHolidays=x.DaysCountExcludingHolidays,
                        LeaveType = x.LeaveType,
                        LeaveReason = x.LeaveReason,
                        Comment = x.Comment,
                        ApproverNotes = x.ApproveNotes,
                        LeaveStatus=x.LeaveStatus
                       
                    }).ToList();

                //Added by Swetha -Mar 16
                response.HolidayListData = Business.GetDownloadHolidayList(staffCode)
                    .Select (x=> new DownloadHolidayList()
                    {
                        Id = x.Id,
                        AreaCode = x.AreaCode,
                        Date = x.Date.ToString("yyyy-MM-dd"),
                        Description = x.Description
                    }).ToList();
                response.AvailableLeaveData = Business.GetDownloadAvailableLeave(staffCode)
                    .Select(x => new DownloadAvailableLeaves()
                    {
                       
                        EmployeeCode=x.EmployeeCode,
                        LeaveType=x.LeaveType,
                        TotalLeaves=x.TotalLeaves,
                        AvailableLeaves=x.AvailableLeaves
                    }).ToList();

                // April 4 2019 - Reitzel changes
                // Download ServerBusinessEntities for a season
                // For now get the first open season
                //string downloadSeasonName = "";
                //if (Utils.SiteConfigData.IsEntityAgreementEnabled)
                //{
                //    ICollection<WorkflowSeason> seasons = Business.GetOpenWorkflowSeasons();
                //    downloadSeasonName = seasons?.FirstOrDefault()?.SeasonName ?? "";
                //}

                response.ServerBusinessEntities = CreateDataForServerBusinessEntities(response, staffCode, 
                                                            validatedAreaCode);

                SetWorkFlowScheduleData(response);
                SetWorkFlowFollowUpData(response);
                SetEntitiesWorkFlowData(response, staffCode, validatedAreaCode);
                SetItemMasterData(response);

                SetStaffDailyData(response, staffCode, validatedAreaCode);

                response.StaffDivisionCodes = Business.GetDivisionCodes(staffCode);

                // Customer Division balances
                response.CustomerDivisionBalances = Business.GetCustomerDivisionBalance(validatedAreaCode, staffCode);

                // PPA Data - TSTanes
                response.PPAData = Business.GetPPAData(staffCode);
                response.OfficeHierarchy = Business.GetAssociations(staffCode);

                // May 4 2020
                SetTransporterData(response);

                // May 21 2020 - control # of agreements on crop basis
                SetWorkflowSeasonData(response);

                // August 09 2020
                SetAvailableStockBalance(response, staffCode);

                // March 03 2021
                SetEmployeeAchievedData(response, staffCode);

                SetEmployeeMonthlyTargetData(response, staffCode);

                SetEmployeeYearlyTargetData(response, staffCode);

                SetQuestionnaireData(response);

                SetProjectData(response, staffCode);

                SetTaskData(response, staffCode);

                //Added by Swetha -Mar 15
                SetLeaveTypesData(response, staffCode);
                
                response.StatusCode = HttpStatusCode.OK;
                response.Content = "";
                response.MessageBarText = Business.GetMessageBarText(staffCode) 
                                    ?? Helper.GetCurrentIstDateTime().ToString(Utils.SiteConfigData.DisplayDateFormatString);
            }
            catch(Exception ex)
            {
                Business.LogError(nameof(DownloadController), ex);
                response.Content = ex.ToString();
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        private static void SetStaffDailyData(DownloadDataResponse response, string staffCode, string areaCode)
        {
            ICollection<StaffDailyData> dailyData = Business.GetStaffDailyData(-1, staffCode);
            response.StaffDailyData = dailyData
                .Where(x=> x.AreaCode.Equals(areaCode, StringComparison.OrdinalIgnoreCase))
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
            }).ToList();
        }

        private static void SetItemMasterData(DownloadDataResponse response)
        {
            ICollection<DomainEntities.ItemMaster> itemsData = Business.GetAllItemMaster();
            //response.ItemMaster = itemsData.Select(x => new DownloadItemMaster()
            //{
            //    ItemId = x.Id,
            //    ItemCode = x.ItemCode,
            //    ItemDesc = x.ItemDesc,
            //    Category = x.Category,
            //    Unit = x.Unit,
            //    Classification = x.Classification,
            //    TypeName = x.TypeName,
            //    Rate = x.Rate
            //}).ToList();

            // flatten it for phone
            response.ItemMaster = itemsData.SelectMany(x => x.TypeNames.Select(y=> new DownloadItemMaster()
            {
                ItemId = x.Id,
                ItemCode = x.ItemCode,
                ItemDesc = x.ItemDesc,
                Category = x.Category,
                Unit = x.Unit,
                Classification = x.Classification,
                TypeName = y.TypeName,
                Rate = y.Rate,
                ReturnRate = y.ReturnRate
            })).ToList();
        }

        private static void SetWorkFlowScheduleData(DownloadDataResponse response)
        {
            ICollection<DomainEntities.WorkFlowSchedule> wfSchedules = Business.GetWorkFlowSchedule();
            response.WorkFlowSchedule = wfSchedules.Select(k => new DownloadWorkFlowSchedule()
                            {
                                TypeName = k.TypeName,
                                TagName = k.TagName,
                                Phase = k.Phase,
                                Sequence = k.Sequence,
                                TargetStartAtDay = k.TargetStartAtDay,
                                TargetEndAtDay = k.TargetEndAtDay,
                                PhoneDataEntryPage = k.PhoneDataEntryPage
                            }).ToList();
        }

        private static void SetWorkFlowFollowUpData(DownloadDataResponse response)
        {
            ICollection<DomainEntities.WorkFlowFollowUp> wfSchedules = Business.GetWorkFlowFollowUp();
            response.WorkFlowFollowUp = wfSchedules.Select(x => new DownloadWorkFlowFollowUp()
            {
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

        private static void SetEntitiesWorkFlowData(DownloadDataResponse response, string staffCode, string areaCode)
        {
            // retrieve user entered entities for download
            IEnumerable<DomainEntities.EntityWorkFlow> downloadEntitiesWorkFlow = Business.GetDownloadEntitiesWorkFlow(staffCode, areaCode);

            // now send only those work flows for which entity is also being sent
            response.EntitiesWorkFlow = downloadEntitiesWorkFlow
                // Aug 25 2020
                // rows that user on web has marked as in-active are not to go on phone as followup activity
                    .Where(x=> x.IsActive)
                    .Select(k => new DownloadEntityWorkFlow()
                    {
                        Id = k.Id,
                        EntityId = k.EntityId,
                        EntityName = k.EntityName,
                        TagName = k.TagName,
                        CurrentPhase = k.CurrentPhase,
                        CurrentPhaseStartDate = k.CurrentPhaseStartDate.ToString("yyyy-MM-dd"),
                        CurrentPhaseEndDate = k.CurrentPhaseEndDate.ToString("yyyy-MM-dd"),
                        InitiationDate = k.InitiationDate.ToString("yyyy-MM-dd"),
                        IsComplete = k.IsComplete,
                        AgreementId = k.AgreementId,
                        EntityWorkFlowDetailId = k.EntityWorkFlowDetailId,
                        Notes = k.Notes,
                        IsFollowUpRow = k.IsFollowUpRow,
                        IsCurrentPhaseRow = k.IsCurrentPhaseRow
                    }).ToList();
        }

        private static IEnumerable<DownloadServerEntity> CreateDataForServerBusinessEntities(
                                        DownloadDataResponse response, 
                                        string staffCode, 
                                        string areaCode) 
        {
            List<DownloadServerEntity> downloadServerEntity = new List<DownloadServerEntity>();

            //var CustomersWithLocation = Business.GetCustomersWithLocation()

            foreach(var cust in response.Customers)
            {
                var ent = new DownloadServerEntity()
                {
                    Id = $"Dealer_{cust.Code}",
                    EntityType = "Dealer",
                    Code = cust.Code,
                    CodeAsLong = 0,
                    Name = cust.Name,
                    UIDType = "",
                    UID = "",
                    EntityNumber = "",
                    FatherHusbandName = "",
                    VillageName = "",
                    IsCustomer = true,
                    Latitude = cust.Latitude,
                    Longitude = cust.Longitude,
                    
                    Contacts = new List<DownloadContactModel>(),
                    Agreements = new List<DownloadAgreementModel>(),
                    Surveys = new List<DownloadSurveyModel>(),
                    BankDetails = new List<DownloadEntityBankDetailModel>()
                };

                ent.Contacts.Add(new DownloadContactModel()
                {
                    Name = cust.Name,
                    PhoneNumber = cust.PhoneNumber,
                    IsPrimary = true
                });

                downloadServerEntity.Add(ent);
            }

            // retrieve user entered entities for download
            IEnumerable<DownloadEntity> downloadEntities = Business.GetDownloadEntities(staffCode, areaCode); 

            if (downloadEntities != null && downloadEntities.Count() > 0)
            {
                downloadServerEntity.AddRange(
                        downloadEntities.Select(x => new DownloadServerEntity()
                        {
                            Id = $"{x.EntityType}_{x.Id}",
                            EntityType = x.EntityType,
                            Code = x.Id.ToString(),
                            CodeAsLong = x.Id,
                            Name = x.EntityName,
                            UIDType = x.UniqueIdType,
                            UID = x.UniqueId,
                            EntityNumber = x.EntityNumber,
                            FatherHusbandName = x.FatherHusbandName,
                            VillageName = x.VillageName,
                            IsCustomer = false,
                            Latitude = x.Latitude,
                            Longitude = x.Longitude,
                            Agreements = x.Agreements.Select(a => new DownloadAgreementModel()
                            {
                                SeasonName = a.SeasonName,
                                TypeName = a.TypeName,
                                AgreementId = a.AgreementId,
                                AgreementNumber = a.AgreementNumber,
                                Status = a.Status,
                                LandSizeInAcres = a.LandSizeInAcres,
                                Latitude = x.Latitude,
                                Longitude = x.Longitude,
                                IssueReturns = a.IssueReturns.Select(ir => new DownloadIssueReturnModel()
                                {
                                    Id = ir.Id,
                                    TransactionDate = ir.TransactionDate.ToString("yyyy-MM-dd"),
                                    SlipNumber = ir.SlipNumber,

                                    TransactionType = ir.IsApproved ? ir.AppliedTransactionType : ir.TransactionType,
                                    ItemMasterId = ir.IsApproved ? ir.AppliedItemMasterId : ir.ItemMasterId,
                                    ItemType = ir.IsApproved ? ir.AppliedItemType : ir.ItemType,
                                    ItemCode = ir.IsApproved ? ir.AppliedItemCode : ir.ItemCode,
                                    ItemUnit = ir.IsApproved ? ir.AppliedItemUnit : ir.ItemUnit,
                                    Quantity = ir.IsApproved ? ir.AppliedQuantity : ir.Quantity,
                                    ItemRate = ir.IsApproved ? ir.AppliedItemRate : ir.ItemRate,

                                    Status = ir.Status,
                                    IsIssueItem = ir.IsIssueItem,
                                    IsApproved = ir.IsApproved
                                }).ToList(),
                                AdvanceRequests = a.AdvanceRequests.Select(ar => new DownloadAdvanceRequestModel()
                                {
                                    Id = ar.Id,
                                    AdvanceRequestDate = ar.AdvanceRequestDate.ToString("yyyy-MM-dd"),
                                    Amount = (ar.IsApproved) ? ar.AmountApproved : ar.AmountRequested,
                                    Status = ar.Status,
                                    IsApproved = ar.IsApproved
                                }).ToList(),
                                DWS = a.DWS.Select(n => new DownloadDWSModel()
                                {
                                    Id = n.Id,
                                    DWSNumber = n.DWSNumber,
                                    DWSDate = n.DWSDate.ToString("yyyy-MM-dd"),
                                    BagCount = n.BagCount,
                                    FilledBagsWeightKg = n.FilledBagsWeightKg,
                                    EmptyBagsWeightKg = n.EmptyBagsWeightKg,
                                    GoodsWeight = n.GoodsWeight,
                                    NetPayableWt = n.NetPayableWt,
                                    SiloDeductWt = (n.IsPending) ? n.SiloDeductWt : n.SiloDeductWtOverride,
                                    GoodsPrice = n.GoodsPrice,
                                    RatePerKg = n.RatePerKg,
                                    DeductAmount = n.DeductAmount,
                                    NetPayable = n.NetPayable,
                                    Status = n.Status,
                                    BankAccountName = n.BankAccountName,
                                    BankName = n.BankName,
                                    PaidDate = (n.PaidDate.HasValue ? n.PaidDate.Value : DateTime.MinValue).ToString("yyyy-MM-dd"),
                                    IsPending = n.IsPending,
                                    IsWeightApproved = n.IsWeightApproved,
                                    IsAmountApproved = n.IsAmountApproved,
                                    IsPaid = n.IsPaid
                                }).ToList(),
                            }).ToList(),

                            Surveys = x.Surveys.Select(a => new DownloadSurveyModel()
                            {
                                SeasonName = a.SeasonName,
                                SowingType = a.TypeName,
                                SurveyId = a.SurveyId,
                                SurveyNumber = a.SurveyNumber,
                                Status = a.Status,
                                LandSizeInAcres = a.LandSizeInAcres,
                                SowingDate = a.SowingDate.ToString("yyyy-MM-dd")
                            }).ToList(),

                            Contacts = x.Contacts.Select(y => new DownloadContactModel()
                            {
                                Name = y.Name,
                                PhoneNumber = y.PhoneNumber,
                                IsPrimary = y.IsPrimary
                            }).ToList(),

                            BankDetails = x.BankDetails.Select(y => new DownloadEntityBankDetailModel
                            {
                                IsSelf = y.IsSelf,
                                AccountNumber = y.AccountNumber,
                                AccountHolderName = y.AccountHolderName,
                                BankName = y.BankName,
                                BankBranch = y.BankBranch,
                                IsApproved = y.IsApproved,
                                Status = y.Status,
                                Comments = y.Comments
                            }).ToList()
                        }).ToList()
                );
            }

            return downloadServerEntity;
        }

        private static string CalculateNumberOfDays(bool isHalfDay, DateTime StartDate, DateTime EndDate)
        {
            if (isHalfDay)
            {
                return "Half day";
            }

            int numberOfDays = (int)EndDate.Subtract(StartDate).TotalDays + 1;

            if (numberOfDays == 1)
            {
                return $"{numberOfDays} day";
            }
            else
            {
                return $"{numberOfDays} days";
            }
        }

        private static void SetTransporterData(DownloadDataResponse response)
        {
            ICollection<DomainEntities.Transporter> transporters = Business.GetTransporterData();
            response.TransporterData = transporters.Select(k => new DownloadTransporter()
            {
                Id = k.Id,
                CompanyName = k.CompanyName,
                VehicleCapacityKgs = k.VehicleCapacityKgs,
                VehicleType = k.VehicleType,
                VehicleNo = k.VehicleNo
            }).ToList();
        }

        private static void SetWorkflowSeasonData(DownloadDataResponse response)
        {
            ICollection<DomainEntities.WorkflowSeason> wfseasons = Business.GetOpenWorkflowSeasons();
            response.WorkflowSeasons = wfseasons.Select(k => new DownloadWorkflowSeason()
            {
                Id = k.Id,
                IsOpen = k.IsOpen,
                MaxAgreementsPerEntity = k.MaxAgreementsPerEntity,
                SeasonName = k.SeasonName,
                TypeName = k.TypeName,
                StartDate = k.StartDate.ToString("yyyy-MM-dd"),
                EndDate = k.EndDate.ToString("yyyy-MM-dd")
            }).ToList();
        }

        private static void SetAvailableStockBalance(DownloadDataResponse response, string staffCode)
        {
            IEnumerable<DomainEntities.StockBalance> stockBalance = Business.GetStockBalance(staffCode);
            response.StockBalances = stockBalance.Select(k => new DownloadStockBalance()
            {
                Id = k.Id,
                ItemMasterId = k.ItemMasterId,
                StockQuantity = k.StockQuantity,
                ItemCode = k.ItemCode,
                ItemDesc = k.ItemDesc,
                Category = k.Category,
                Unit = k.Unit
            }).ToList();
        }

        private static void SetEmployeeAchievedData(DownloadDataResponse response, string staffCode)
        {
            ICollection<DomainEntities.EmployeeAchieved> achievedData = Business.GetEmployeeAchieveds(staffCode);
            response.EmployeeAchievedData = achievedData.Select(ea => new EmployeeAchieved()
            {
                Id = ea.Id,
                EmployeeCode = ea.EmployeeCode,
                Month = ea.Month,
                Year = ea.Year,
                Type = ea.Type,
                AchievedMonthly = ea.AchievedMonthly
            }).ToList();
        }

        private static void SetEmployeeMonthlyTargetData(DownloadDataResponse response, string staffCode)
        {
            ICollection<DomainEntities.EmployeeMonthlyTarget> monthlyTargetData = Business.GetEmployeeMonthlyTargets(staffCode);
            response.EmployeeMonthlyTargetData = monthlyTargetData.Select(ea => new EmployeeMonthlyTarget()
            {
                Id = ea.Id,
                EmployeeCode = ea.EmployeeCode,
                Month = ea.Month,
                Year = ea.Year,
                Type = ea.Type,
                MonthlyTarget = ea.MonthlyTarget
            }).ToList();
        }

        private static void SetEmployeeYearlyTargetData(DownloadDataResponse response, string staffCode)
        {
            ICollection<DomainEntities.EmployeeYearlyTarget> yearlyTargetData = Business.GetEmployeeYearlyTargets(staffCode);
            response.EmployeeYearlyTargetData = yearlyTargetData.Select(ea => new EmployeeYearlyTarget()
            {
                Id = ea.Id,
                EmployeeCode = ea.EmployeeCode,
                Year = ea.Year,
                Type = ea.Type,
                YearlyTarget = ea.YearlyTarget
            }).ToList();
        }

        private static void SetQuestionnaireData(DownloadDataResponse response)
        {
            ICollection<DomainEntities.DownloadQuestionnaire> questionnaireData = Business.GetQuestionnaire();
            response.QuestionnaireData = questionnaireData.Select(ea => new DownloadQuestionnaire()
            {
                Id = ea.Id,
                Name = ea.Name,
                EntityType = ea.EntityType,
                QuestionCount = ea.QuestionCount,
                Questions = ea.Questions.Select(y => new DownloadQuestionPaperQuestion
                {
                    Id = y.Id,
                    QText = y.QText,
                    AnswerChoices = y.AnswerChoices.Select(z => new DownloadQuestionPaperAnswer
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

        //Author : Kartik; Purpose - Send data realed to followup functionality

        private static void SetProjectData(DownloadDataResponse response, string staffCode)
        {
            ICollection<DomainEntities.DownloadProjects> projectData = Business.GetProjectsAssignmentsData(staffCode);
            response.ProjectData = projectData.Select(ea => new DownloadServerProjects()
            {
                Id = ea.Id,
                ProjectName = ea.ProjectName,
                ProjectDescription = ea.ProjectDescription,
                ProjectCategory = ea.ProjectCategory,
                PlannedStartDate = ea.PlannedStartDate.ToString("yyyy-MM-dd"),
                PlannedEndDate = ea.PlannedEndDate.ToString("yyyy-MM-dd"),
                ActualStartDate = ea.ActualStartDate.ToString("yyyy-MM-dd"),
                ActualEndDate = ea.ActualEndDate.ToString("yyyy-MM-dd"),
                ProjectStatus = ea.ProjectStatus,
                AssignedStartDate = ea.AssignedStartDate.ToString("yyyy-MM-dd"),
                AssignedEndDate = ea.AssignedEndDate.ToString("yyyy-MM-dd"),
                CreatedBy = ea.CreatedBy,
                AssignedBy = ea.AssignedBy,
            }).ToList();
        }

        private static void SetTaskData(DownloadDataResponse response, string staffCode)
        {
            IEnumerable<DomainEntities.DownloadTasks> taskData = Business.GetTasksAssignmentsData(staffCode);
            response.FollowUpTaskData = taskData.Select(ea => new DownloadServerTasks()
            {
                TaskAssignmentId = ea.TaskAssignmentId,
                TaskId = ea.TaskId,
                XRefProjectId = ea.XRefProjectId,
                ProjectName = ea.ProjectName,
                TaskDescription = ea.TaskDescription,
                ActivityType = ea.ActivityType,
                ClientType = ea.ClientType,
                ClientName = ea.ClientName,
                ClientCode = ea.ClientCode,
                PlannedStartDate = ea.PlannedStartDate.ToString("yyyy-MM-dd"),
                PlannedEndDate = ea.PlannedEndDate.ToString("yyyy-MM-dd"),
                ActualStartDate = ea.ActualStartDate.ToString("yyyy-MM-dd"),
                ActualEndDate = ea.ActualEndDate.ToString("yyyy-MM-dd"),
                TaskStatus = ea.TaskStatus,
                Comments = ea.Comments,
                AssignedStartDate = ea.AssignedStartDate.ToString("yyyy-MM-dd"),
                AssignedEndDate = ea.AssignedEndDate.ToString("yyyy-MM-dd"),
                CreatedBy = ea.CreatedBy,
                AssignedBy = ea.AssignedBy
            }).ToList();
        }
        //Added by Swetha -Mar 15
        private static void SetLeaveTypesData(DownloadDataResponse response, string staffCode)
        {
            ICollection<DomainEntities.LeaveTypes> leaveTypes = Business.GetDownloadLeaveTypes(staffCode);
            response.LeaveTypeData = leaveTypes.Select(x => new LeaveTypes()
            {
                Id =x.Id,
                EmployeeCode = x.EmployeeCode,
                LeaveType = x.LeaveType,
                TotalLeaves = x.TotalLeaves,
            }).ToList();
        }
    }
}
