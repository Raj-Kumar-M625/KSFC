using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
//using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace EpicCrmWebApi
{
    [Authorize(Roles = "Admin")]
    public partial class AdminController : BaseDashboardController
    {

        // GET: Admin Page
        public ActionResult Index()
        {
            // Get rights
            ViewBag.FeatureSet = Helper.GetAvailableFeatures(CurrentUserStaffCode, IsSuperAdmin);

            return View();
        }

        //https://stackoverflow.com/questions/17657184/using-jquerys-ajax-method-to-retrieve-images-as-a-blob
        //https://stackoverflow.com/questions/26902067/load-jpeg-image-from-mvc-controller-via-javascript
        //https://www.codeproject.com/articles/33310/c-save-and-load-image-from-database
        //https://stackoverflow.com/questions/16386514/how-can-i-get-image-data-from-a-server-side-perl-program-and-display-in-a-div-us
        //[AjaxOnly]
        [HttpGet]
        public EmptyResult ImageData(long expenseId, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.ImageData(expenseId, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
            //return File(imageBytes, "image/jpeg");
        }

        [HttpGet]
        public EmptyResult PaymentImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.PaymentImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult ActivityImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.ActivityImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult STRImageData(long Id, int imageItem) // here it is strId
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.STRImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }


        [HttpGet]
        public EmptyResult EntityImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.EntityImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult EntityBankDetailImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.EntityBankDetailImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.SalesPersonFeature)]
        public ActionResult ShowStaff()
        {
            bool flagGrade = Helper.IsGradeFeatureEnabled();
            if (flagGrade)
            {
                ViewBag.Grade = Business.GetCodeTable("Grade");
            }
            ViewBag.GradesEnabled = flagGrade;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.Department = Business.GetCodeTable("Department");
            ViewBag.Designation = Business.GetCodeTable("Designation");
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.SalesPersonFeature)]
        public ActionResult GetSearchSalesPersons(StaffFilter searchCriteria)
        {
            DomainEntities.StaffFilter criteria = Helper.ParseStaffSearchCriteria(searchCriteria);
            if (!Helper.IsGradeFeatureEnabled())
            {
                criteria.ApplyGradeFilter = false;
            }

            IEnumerable<SalesPersonModel> salesPersonData = Business.GetSalesPersonData(criteria);

            var model = salesPersonData.Select(spd => new SalesPersonViewModel()
            {
                Id = spd.Id,
                IsActive = spd.IsActive,
                Name = spd.Name,
                Phone = spd.Phone,
                StaffCode = spd.StaffCode,
                EmployeeId = spd.EmployeeId,
                OnWeb = spd.OnWeb,
                HQCode = spd.HQCode,
                HQName = spd.HQName,
                Grade = spd.Grade,
                Department = spd.Department,
                Designation = spd.Designation,
                Ownership = spd.Ownership,
                VehicleType = spd.VehicleType,
                FuelType = spd.FuelType,
                VehicleNumber = spd.VehicleNumber,
                BusinessRole = spd.BusinessRole,
                OverridePrivateVehicleRatePerKM = spd.OverridePrivateVehicleRatePerKM,
                TwoWheelerRatePerKM = spd.TwoWheelerRatePerKM,
                FourWheelerRatePerKM = spd.FourWheelerRatePerKM
            }).ToList();

            ViewBag.AllowSalesPersonMaintenanceOnWeb = Utils.SiteConfigData.AllowSalesPersonMaintenanceOnWeb;
            ViewBag.GradesEnabled = Helper.IsGradeFeatureEnabled();
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;
            return PartialView("_ShowStaff", model);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult AddStaff()
        {
            if (!Utils.SiteConfigData.AllowSalesPersonMaintenanceOnWeb)
            {
                return Content("Operation not allowed!");
            }

            SalesPersonViewModel model = new SalesPersonViewModel();
            SetStaffViewBagData();
            return PartialView(model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult AddStaff(SalesPersonViewModel model)
        {
            if (!Utils.SiteConfigData.AllowSalesPersonMaintenanceOnWeb)
            {
                return Content("Operation not allowed!");
            }

            if (!ModelState.IsValid)
            {
                //LogModelErrors(nameof(AdminController));
                SetStaffViewBagData();
                return PartialView(model);
            }

            model.IsActive = true;
            SalesPersonModel sp = model.GetBusinessModel();

            try
            {
                Business.CreateSalesPersonData(sp);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                SetStaffViewBagData();
                return PartialView(model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditStaff(string staffCode)
        {
            if (!Utils.SiteConfigData.AllowSalesPersonMaintenanceOnWeb)
            {
                return Content("Operation not allowed!");
            }

            SalesPersonModel salesPerson = Business.GetSingleSalesPersonData(staffCode);

            SalesPersonViewModel model = new SalesPersonViewModel()
            {
                HQCode = salesPerson.HQCode,
                Name = salesPerson.Name,
                Phone = salesPerson.Phone,
                StaffCode = salesPerson.StaffCode,
                Grade = salesPerson.Grade,
                IsActive = salesPerson.IsActive,
                Department = salesPerson.Department,
                Designation = salesPerson.Designation,
                Ownership = salesPerson.Ownership,
                VehicleType = salesPerson.VehicleType,
                FuelType = salesPerson.FuelType,
                VehicleNumber = salesPerson.VehicleNumber,
                BusinessRole = salesPerson.BusinessRole,
                OverridePrivateVehicleRatePerKM = salesPerson.OverridePrivateVehicleRatePerKM,
                TwoWheelerRatePerKM = salesPerson.TwoWheelerRatePerKM,
                FourWheelerRatePerKM = salesPerson.FourWheelerRatePerKM
            };

            SetStaffViewBagData();

            return PartialView(model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditStaff(SalesPersonViewModel model)
        {
            if (!Utils.SiteConfigData.AllowSalesPersonMaintenanceOnWeb)
            {
                return Content("Operation not allowed!");
            }

            if (!ModelState.IsValid)
            {
                //LogModelErrors(nameof(AdminController));
                SetStaffViewBagData();
                return PartialView(model);
            }

            SalesPersonModel es = model.GetBusinessModel();

            try
            {
                Business.SaveSalesPersonData(es);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                SetStaffViewBagData();
                return PartialView(model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditEntity(long entityId)
        {
            DomainEntities.Entity entityRec = Business.GetSingleEntity(entityId);

            EntityModel model =
                         new EntityModel()
                         {
                             Id = entityRec.Id,
                             EmployeeId = entityRec.EmployeeId,
                             DayId = entityRec.DayId,
                             HQCode = entityRec.HQCode,
                             EmployeeCode = entityRec.EmployeeCode,
                             EmployeeName = entityRec.EmployeeName,
                             AtBusiness = entityRec.AtBusiness,
                             //Consent = entityRec.Consent, //swetha made changes on 24-11
                             EntityType = entityRec.EntityType,
                             EntityName = entityRec.EntityName,
                             EntityDate = entityRec.EntityDate,
                             Address = entityRec.Address,
                             City = entityRec.City,
                             State = entityRec.State,
                             Pincode = entityRec.Pincode,
                             LandSize = entityRec.LandSize,
                             Latitude = entityRec.Latitude,
                             Longitude = entityRec.Longitude,
                             SqliteEntityId = entityRec.SqliteEntityId,
                             ContactCount = entityRec.ContactCount,
                             CropCount = entityRec.CropCount,
                             ImageCount = entityRec.ImageCount,
                             AgreementCount = entityRec.AgreementCount,
                             UniqueIdType = entityRec.UniqueIdType,
                             UniqueId = entityRec.UniqueId,
                             TaxId = entityRec.TaxId,

                             FatherHusbandName = entityRec.FatherHusbandName,
                             HQName = entityRec.HQName,
                             TerritoryCode = entityRec.TerritoryCode,
                             TerritoryName = entityRec.TerritoryName,
                             //MajorCrop = entityRec.MajorCrop,
                             //LastCrop = entityRec.LastCrop,
                             //WaterSource = entityRec.WaterSource,
                             //SoilType = entityRec.SoilType,
                             //SowingType = entityRec.SowingType,
                             //SowingDate = entityRec.SowingDate.HasValue ? entityRec.SowingDate.Value : DateTime.MinValue,
                             EntityNumber = entityRec.EntityNumber,
                             IsActive = entityRec.IsActive
                         };

            FillEditEntityViewBag();

            return PartialView(model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditEntity(EntityModel model)
        {
            //string theDate = this.HttpContext.Request.Params["SowingDate"];
            //DateTime dt = new DateTime();
            //bool success = DateTime.TryParse(theDate, CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None, out dt);
            //model.SowingDate = (success) ? dt : DateTime.MinValue;

            ModelState.Clear();
            TryValidateModel(model);

            if (!ModelState.IsValid)
            {
                FillEditEntityViewBag();

                return PartialView(model);
            }

            // validate unique id only if it is enabled - and using is saving an active record
            // (if record being saved is marked as inactive, we don't care, if it has duplicate aadhar)
            if (Utils.SiteConfigData.EntityModuleUniqueIdVisible && model.IsActive)
            {
                bool uniqueIdExist = Business.IsDuplicateEntityUniqueId(model.Id, model.UniqueId.CrmTrim());
                if (uniqueIdExist)
                {
                    ModelState.AddModelError("UniqueId", "UniqueId (Aadhar) Already Exist");
                    FillEditEntityViewBag();
                    return PartialView(model);
                }
            }

            DomainEntities.Entity origRec = Business.GetSingleEntity(model.Id);

            DomainEntities.Entity entityRec = new DomainEntities.Entity()
            {
                Id = model.Id,
                EntityName = model.EntityName.CrmTrim(),
                LandSize = model.LandSize.CrmTrim(),
                TaxId = model.TaxId.CrmTrim(),
                UniqueId = model.UniqueId.CrmTrim(),
                Address = model.Address.CrmTrim(),
                City = model.City.CrmTrim(),
                State = model.State.CrmTrim(),
                Pincode = model.Pincode.CrmTrim(),
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                HQCode = model.HQCode,
                CurrentUser = CurrentUserStaffCode,
                AtBusiness = model.AtBusiness,
                //Consent = model.Consent, //Swetha Made changes on 24-11

                FatherHusbandName = model.FatherHusbandName.CrmTrim(),
                //MajorCrop = model.MajorCrop.CrmTrim(),
                //LastCrop = model.LastCrop.CrmTrim(),
                //WaterSource = model.WaterSource.CrmTrim(),
                //SoilType = model.SoilType.CrmTrim(),
                //SowingType = model.SowingType.CrmTrim(),
                //SowingDate = model.SowingDate,

                HQName = origRec.HQName,
                TerritoryCode = origRec.TerritoryCode,
                TerritoryName = origRec.TerritoryName,

                IsActive = model.IsActive
            };

            // based on HQ Code, fill HQName and Territory Code + Name
            if (!origRec.HQCode.Equals(entityRec.HQCode, StringComparison.OrdinalIgnoreCase))
            {
                var oh = GetOfficeHierarchy();
                var ohRec = oh.Where(x => x.HQCode.Equals(entityRec.HQCode, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();
                if (ohRec != null)
                {
                    entityRec.HQName = ohRec.HQName;
                    entityRec.TerritoryCode = ohRec.TerritoryCode;
                    entityRec.TerritoryName = ohRec.TerritoryName;
                }
            }

            try
            {
                Business.SaveEntityData(entityRec);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditEntity), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                FillEditEntityViewBag();
                return PartialView(model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditBankDetail(long entityBankDetailId)
        {
            EntityBankDetail bankDetail = Business.GetSingleBankDetail(entityBankDetailId);

            return EditBankDetail(bankDetail);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditBankDetail(EntityBankDetailModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Business.LogError($"{nameof(EditBankDetail)}", "Model State is invalid...");
                    PutBankDetailStatusInViewBag();
                    return PartialView("_EditBankDetail", model);
                }

                EntityBankDetail ea = new EntityBankDetail()
                {
                    Id = model.Id,
                    EntityId = model.EntityId,
                    Status = model.Status,
                    Comments = Utils.TruncateString(model.Comments, 100),
                    IsActive = model.IsActive,
                    IsApproved = "Approved".Equals(model.Status, StringComparison.OrdinalIgnoreCase),
                    IsSelfAccount = model.IsSelfAccount,
                    AccountHolderName = Utils.TruncateString(model.AccountHolderName, 50),
                    AccountHolderPAN = Utils.TruncateString(model.AccountHolderPAN, 50),
                    BankName = Utils.TruncateString(model.BankName, 50),
                    BankAccount = Utils.TruncateString(model.BankAccount, 50),
                    BankIFSC = Utils.TruncateString(model.BankIFSC, 50),
                    BankBranch = Utils.TruncateString(model.BankBranch, 50)
                };

                Business.SaveBankDetail(ea, this.CurrentUserStaffCode);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditBankDetail), ex);
                ModelState.AddModelError("", ex.Message);

                PutBankDetailStatusInViewBag();

                return PartialView("_EditBankDetail", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowDwsIssueReturns(long entityAgreementId = 0, long entityId = 0, int defaultTab = 1)
        {
            if (entityAgreementId <= 0 && entityId <= 0)
            {
                return PartialView("_CustomError", "Required input parameter is not supplied.");
            }

            EntityAgreement agreement = null;
            Entity entity = null;
            if (entityAgreementId > 0)
            {
                agreement = Business.GetSingleAgreement(entityAgreementId);
                entityId = agreement.EntityId;
            }

            if (entityId > 0)
            {
                entity = Business.GetSingleEntity(entityId);
            }

            if (entity == null)
            {
                return PartialView("_CustomError", "Required input parameter is not supplied.");
            }

            // retrieve all DWS for this Entity Id and Agreement Id
            ViewBag.AllDWS = GetDWS(entityId, entityAgreementId);

            if (Utils.SiteConfigData.IssueReturnModule)
            {
                // retrieve all Issues/Returns
                ViewBag.IssueReturns = GetIssueReturns(entityId, entityAgreementId);
            }

            if (Utils.SiteConfigData.AdvanceRequestModule)
            {
                ViewBag.AdvanceRequests = GetAdvanceRequests(entityId, entityAgreementId);
            }

            if (entityId > 0 && entityAgreementId > 0)
            {
                ViewBag.WorkFlowDetails = GetWorkFlowDetails(entityId, entityAgreementId);
            }

            ICollection<DomainEntities.WorkFlowFollowUp> wff = Business.GetWorkFlowFollowUp();
            ViewBag.WorkFlowFollowUp = wff;

            ViewBag.EntityType = entity?.EntityType ?? "";
            ViewBag.EntityName = entity?.EntityName ?? "";
            ViewBag.AgreementNumber = agreement?.AgreementNumber ?? "";
            ViewBag.TypeName = agreement?.TypeName ?? "";

            defaultTab = (defaultTab < 1) ? 1 : defaultTab;
            ViewBag.DefaultTab = defaultTab;

            ViewBag.EntityAgreementId = entityAgreementId;

            return PartialView("_ShowDwsIssueReturns");
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditAgreement(long entityAgreementId)
        {
            EntityAgreement agreement = Business.GetSingleAgreement(entityAgreementId);

            return EditAgreement(agreement);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditAgreement(EntityAgreementModel model)
        {
            try
            {
                // convert date from dd-MM-YYYY format
                string theDate = this.HttpContext.Request.Params["PassBookReceivedDate"];
                DateTime dt = new DateTime();
                bool success = DateTime.TryParse(theDate, CultureInfo.GetCultureInfo("en-GB"), DateTimeStyles.None, out dt);
                model.PassBookReceivedDate = (success) ? dt : DateTime.MinValue;

                ModelState.Clear();
                TryValidateModel(model);

                if (!ModelState.IsValid)
                {
                    Business.LogError($"{nameof(EditAgreement)}", "Model State is invalid...");
                    PutAgreementStatusInViewBag();
                    return PartialView("_EditAgreement", model);
                }

                // perform unique check only if unique id field is visible
                if (Utils.SiteConfigData.EntityModuleUniqueIdVisible)
                {
                    bool uniqueIdExist = Business.IsDuplicateEntityUniqueId(model.EntityId, model.UniqueId);

                    //check for duplicate unique id while adding agreement
                    if (uniqueIdExist)
                    {
                        ModelState.AddModelError("", "Duplicate UniqueId (Aadhar) Exist for this Client. Please fix this before Proceeding.");
                        PutAgreementStatusInViewBag();
                        return PartialView("_EditAgreement", model);
                    }
                }

                // ideally this check is not required, as user won't come to edit agreement page
                // if agreements are not enabled;
                if (Utils.SiteConfigData.IsEntityAgreementEnabled)
                {
                    bool agreementNumberExist = Business.IsDuplicateEntityAgreementNumber(model.Id, model.AgreementNumber.CrmTrim());
                    //check for duplicate agreement number while adding/editing agreement
                    if (agreementNumberExist)
                    {
                        ModelState.AddModelError("AgreementNumber", "Agreement Number Already Exist");
                        PutAgreementStatusInViewBag();
                        return PartialView("_EditAgreement", model);
                    }
                }


                EntityAgreement ea = new EntityAgreement()
                {
                    Id = model.Id,
                    EntityId = model.EntityId,
                    WorkflowSeasonId = model.WorkflowSeasonId,
                    AgreementNumber = model.AgreementNumber.CrmTrim(),
                    IsPassbookReceived = model.IsPassBookReceived,
                    PassbookReceivedDate = model.PassBookReceivedDate,
                    Status = model.Status,
                    LandSizeInAcres = model.LandSizeInAcres
                };

                Business.SaveAgreement(ea, this.CurrentUserStaffCode);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditAgreement), ex);
                ModelState.AddModelError("", ex.Message);

                PutAgreementStatusInViewBag();

                return PartialView("_EditAgreement", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditSurvey(long entitySurveyId)
        {
            EntitySurvey survey = Business.GetSingleSurvey(entitySurveyId);
            return EditSurvey(survey);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditSurvey(EntitySurveyModel model)
        {
            try
            {
                ModelState.Clear();
                TryValidateModel(model);

                if (!ModelState.IsValid)
                {
                    Business.LogError($"{nameof(EditSurvey)}", "Model State is invalid...");
                    PutAgreementStatusInViewBag();
                    return PartialView("_EditSurvey", model);
                }

                EntitySurvey ea = new EntitySurvey()
                {
                    Id = model.Id,
                    EntityId = model.EntityId,
                    WorkflowSeasonId = model.WorkflowSeasonId,
                    SurveyNumber = model.SurveyNumber.CrmTrim(),
                    Status = model.Status,
                    LandSizeInAcres = model.LandSizeInAcres
                };

                Business.SaveSurvey(ea, this.CurrentUserStaffCode);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditSurvey), ex);
                ModelState.AddModelError("", ex.Message);

                PutAgreementStatusInViewBag();

                return PartialView("_EditSurvey", model);
            }
        }

        private ActionResult EditBankDetail(EntityBankDetail bankDetailRec)
        {
            DomainEntities.Entity entityRec = Business.GetSingleEntity(bankDetailRec.EntityId);

            EntityBankDetailModel model = new EntityBankDetailModel()
            {
                Id = bankDetailRec.Id,
                EntityId = bankDetailRec.EntityId,
                EntityName = entityRec.EntityName,
                EntityType = entityRec.EntityType,

                Status = bankDetailRec.Status,
                IsApproved = bankDetailRec.IsApproved,
                Comments = bankDetailRec.Comments,
                IsActive = bankDetailRec.IsActive,
                IsSelfAccount = bankDetailRec.IsSelfAccount,
                AccountHolderName = bankDetailRec.AccountHolderName,
                AccountHolderPAN = bankDetailRec.AccountHolderPAN,
                BankName = bankDetailRec.BankName,
                BankAccount = bankDetailRec.BankAccount,
                BankIFSC = bankDetailRec.BankIFSC,
                BankBranch = bankDetailRec.BankBranch
            };

            PutBankDetailStatusInViewBag();

            return PartialView("_EditBankDetail", model);
        }

        private ActionResult EditAgreement(EntityAgreement agreementRec)
        {
            DomainEntities.Entity entityRec = Business.GetSingleEntity(agreementRec.EntityId);

            EntityAgreementModel model = new EntityAgreementModel()
            {
                Id = agreementRec.Id,
                AgreementNumber = agreementRec.AgreementNumber,
                EntityId = agreementRec.EntityId,
                Status = agreementRec.Status,
                WorkflowSeasonId = agreementRec.WorkflowSeasonId,
                EntityName = entityRec.EntityName,
                EntityType = entityRec.EntityType,
                UniqueId = entityRec.UniqueId,
                IsPassBookReceived = agreementRec.IsPassbookReceived,
                PassBookReceivedDate = agreementRec.PassbookReceivedDate,
                WorkflowSeasonName = agreementRec.WorkflowSeasonName,
                LandSizeInAcres = agreementRec.LandSizeInAcres,
                TypeName = agreementRec.TypeName
            };

            PutAgreementStatusInViewBag();

            return PartialView("_EditAgreement", model);
        }

        private ActionResult EditSurvey(EntitySurvey surveyRec)
        {
            DomainEntities.Entity entityRec = Business.GetSingleEntity(surveyRec.EntityId);
            EntitySurveyModel model = CreateEntitySurveyModel(entityRec, surveyRec);
            PutAgreementStatusInViewBag();
            return PartialView("_EditSurvey", model);
        }

        private void PutAgreementStatusInViewBag()
        {
            ViewBag.AgreementStatus = Business.GetCodeTable("AgreementStatus");

        }

        private void PutBankDetailStatusInViewBag()
        {
            ViewBag.BankDetailStatus = Business.GetCodeTable("BankDetailStatus");
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.ProductFeature)]
        public ActionResult ShowProducts()
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;
            ViewBag.Zones = Business.GetZones(IsSuperAdmin, officeHierarchy).OrderBy(s => s.CodeName);
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.CrmUserFeature)]
        public ActionResult GetSearchProducts(ProductsFilter searchCriteria)
        {
            try
            {
                DomainEntities.ProductsFilter criteria = Helper.ParseProductsSearchCriteria(searchCriteria);
                criteria.IsSuperAdmin = IsSuperAdmin;
                criteria.StaffCode = CurrentUserStaffCode;
                IEnumerable<DashboardProduct> productData = Business.GetProductData(criteria);
                Business.LogError($"{nameof(GetSearchProducts)}", $"Creating View now with {productData.Count()} items.");
                return PartialView("_ShowProducts", productData);
            }
            catch (Exception ex)
            {
                Business.LogError($"{nameof(GetSearchProducts)}", ex);
                return PartialView("_Error");
            }
        }

        [AjaxOnly]
        [HttpGet]
        public EmptyResult DisassociateUserOnPhone(long empId)
        {
            Business.DisassociateUserOnPhone(empId);
            return new EmptyResult();
        }

        [AjaxOnly]
        [HttpGet]
        public EmptyResult ToggleExecAppAccess(long empId)
        {
            Business.ToggleExecAppAccess(empId);
            return new EmptyResult();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.CrmUserFeature)]
        public ActionResult ShowRegisteredUsers()
        {
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.CrmUserFeature)]
        public ActionResult GetSearchRegisteredUsers(CRMUsersFilter searchCriteria) // don't change this parameter name
        {
            DomainEntities.CRMUsersFilter s = ParseRegisteredUsersSearchCriteria(searchCriteria);

            ICollection<EmployeeRecord> appUsers = Business.Users(s);

            var model = appUsers.Select(x => new EmployeeRecordModel()
            {
                EmployeeId = x.EmployeeId,
                EmployeeCode = x.EmployeeCode,
                ManagerId = x.ManagerId,
                Name = x.Name,
                TenantId = x.TenantId,
                IsActive = x.IsActive,
                IMEI = x.IMEI,
                OnPhone = !String.IsNullOrEmpty(x.IMEI),
                OnWebPortal = x.OnWebPortal,
                IsActiveInSap = x.IsActiveInSap,
                Phone = x.Phone,
                SendLogFromPhone = x.SendLogFromPhone,
                ExecAppAccess = x.ExecAppAccess,
                AutoUploadFromPhone = x.AutoUploadFromPhone,
                ActivityPageName = x.ActivityPageName ?? "",
                LocationFromType = x.LocationFromType ?? ""
            });

            if (searchCriteria.OnPhone == true)
            {
                model = model.Where(e => e.OnPhone == true);
            }
            if (searchCriteria.ExecAppAccess == true)
            {
                model = model.Where(e => e.OnPhone == true && e.ExecAppAccess == true);
            }

            // show advanced columns to only super admin that is created at the time of setup
            // and not to on the fly super admin accounts.
            ViewBag.ShowAdvancedColumns = IsSetupSuperAdmin;

            return PartialView("_CRMUsers", model.ToList());
        }

        private DomainEntities.CRMUsersFilter ParseRegisteredUsersSearchCriteria(CRMUsersFilter searchCriteria)
        {
            DomainEntities.CRMUsersFilter s = new DomainEntities.CRMUsersFilter()
            {
                ApplyNameFilter = false,
                ApplyEmployeeCodeFilter = false,
                ApplyIMEIFilter = false
            };
            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyNameFilter = Helper.IsValidSearchCriteria(searchCriteria.Name);
            s.Name = s.ApplyNameFilter ? searchCriteria.Name.Trim() : searchCriteria.Name;

            s.ApplyEmployeeCodeFilter = Helper.IsValidSearchCriteria(searchCriteria.EmployeeCode);
            s.EmployeeCode = s.ApplyEmployeeCodeFilter ? searchCriteria.EmployeeCode.Trim() : searchCriteria.EmployeeCode;

            s.ApplyIMEIFilter = Helper.IsValidSearchCriteria(searchCriteria.IMEI);
            s.IMEI = s.ApplyIMEIFilter ? searchCriteria.IMEI.Trim() : searchCriteria.IMEI;
            s.IsActiveInSap = searchCriteria.IsActiveInSap;
            s.OnPhone = searchCriteria.OnPhone;
            s.ExecAppAccess = searchCriteria.ExecAppAccess;
            s.OnWebPortal = searchCriteria.OnWebPortal;
            s.IsEmployeeActive = searchCriteria.IsEmployeeActive;

            return s;
        }

        public ActionResult Admin()
        {
            return View();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.AssignmentFeature)]
        public ActionResult ManageSalesPersons()
        {
            return View();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.AssignmentFeature)]
        public ActionResult ManageSalesPersonAssignmentData()
        {
            ViewBag.SalesPersons = Business.GetSalesPersons();
            return View("ManageSalesPersonRegion");
        }

        [AjaxOnly]
        [HttpGet]
        public JsonResult GetAreasforSalesPerson(string staffCode, string level)
        {
            IEnumerable<CodeTableEx> codeVals = Business.GetCodeTable(level);
            var associatedPersons = Business.GetSalesPersonAssignmentData(staffCode, level);

            IEnumerable<SalesPersonsAssociation> regionsforsalesPersonResult =
                                                                (from c in codeVals
                                                                 join a in associatedPersons
                                                                 on c.Code equals a.CodeValue into temp
                                                                 from t in temp.DefaultIfEmpty()
                                                                 select new SalesPersonsAssociation()
                                                                 {
                                                                     Code = c.Code,
                                                                     CodeName = c.CodeName,
                                                                     IsAssigned = (t != null)
                                                                 }).OrderBy(x => x.CodeName).ToList();

            if (regionsforsalesPersonResult.Count() < 1)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }

            return Json(regionsforsalesPersonResult, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        public JsonResult SaveSalesPersonAssignmentData(string staffCode, string level, string[] codes)
        {
            MinimumResponse response = new MinimumResponse();
            bool result = Business.SaveSalesPersonAssignmentData(staffCode, level, codes, this.CurrentUserStaffCode);

            switch (result)
            {
                case false:
                    response.StatusCode = 0;
                    response.Content = "Error occured while processing your data";
                    break;
                case true:
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = "Saved Successfully";
                    break;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        public JsonResult GetCodeFromLevel(string level)
        {
            IEnumerable<CodeTable> codeVals = Business.GetCodeTable(level);
            return Json(codeVals, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        public JsonResult GetSalesPersons(string level, string code)
        {
            var salesPersons = Business.GetSalesPersons();
            var associatedPersons = Business.GetSalesPersonsAssociation(level, code);

            IEnumerable<SalesPersonResult> salesPersonResult = (from s in salesPersons
                                                                join a in associatedPersons on s.StaffCode equals a.StaffCode into temp
                                                                from t in temp.DefaultIfEmpty()
                                                                select new SalesPersonResult()
                                                                {
                                                                    SalesPersonName = s.SalesPersonName,
                                                                    StaffCode = s.StaffCode,
                                                                    AssignedDate = (t == null) ? DateTime.MinValue.ToString("dd-MM-yyyy") : t.AssignedDate.ToString("dd-MM-yyyy"),
                                                                    IsAssigned = (t != null)
                                                                }).OrderBy(x => x.SalesPersonName).ToList();

            if (salesPersonResult == null || salesPersonResult.Count() < 1)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            return Json(salesPersonResult, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        public JsonResult SaveAssignedSalesPersons(string[] staffCodes, string level, string code)
        {
            MinimumResponse response = new MinimumResponse();
            bool result = Business.SaveAssignedSalesPersons(staffCodes, level, code, this.CurrentUserStaffCode);

            switch (result)
            {
                case false:
                    response.StatusCode = 0;
                    response.Content = "Unable to save the changes. Please try after sometime";
                    break;
                case true:
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = "Saved Successfully";
                    break;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult GetStaffAssociations(string staffCode)
        {
            // list of associations actually defined by admin
            IEnumerable<SalesPersonsAssociationData> salesAssociatedCode = Business.GetStaffAssociations(staffCode);

            var associationModel = salesAssociatedCode.Select(x => new SalesPersonsAssociationDataModel()
            {
                AssignedDate = x.AssignedDate,
                CodeName = x.CodeName,
                CodeType = x.CodeType,
                Code = x.Code
            }).ToList();

            // system's interpretation of associations defined.
            IEnumerable<OfficeHierarchy> officeHierarchy = Business.GetSelectableAssociations(staffCode);
            IEnumerable<SelectableOfficeHierarchyModel> selectableOfficeHierarchyModel = officeHierarchy.Select(x => new SelectableOfficeHierarchyModel()
            {
                ZoneCode = x.ZoneCode,
                ZoneName = x.ZoneName,
                AreaCode = x.AreaCode,
                AreaName = x.AreaName,
                TerritoryCode = x.TerritoryCode,
                TerritoryName = x.TerritoryName,
                HQCode = x.HQCode,
                HQName = x.HQName,
                IsZoneSelectable = x.IsZoneSelectable,
                IsAreaSelectable = x.IsAreaSelectable,
                IsTerritorySelectable = x.IsTerritorySelectable,
                IsHQSelectable = x.IsHQSelectable
            }).ToList();

            // based on defined associations, what all users come under me
            IEnumerable<string> visibleStaffCodes = Business.GetVisibleStaffCodes(false, staffCode);
            IEnumerable<SalesPersonModel> salesPersonData = Business.GetSelectedSalesPersonData(visibleStaffCodes);

            var visibleStaffData = salesPersonData
                .Select(spd => new SalesPersonViewModel()
                {
                    Id = spd.Id,
                    IsActive = spd.IsActive,
                    Name = spd.Name,
                    Phone = spd.Phone,
                    StaffCode = spd.StaffCode,
                    //CustomerCount = spd.CustomerCount,
                    EmployeeId = spd.EmployeeId,
                    //ProductCount = spd.ProductCount,
                    OnWeb = spd.OnWeb
                }).ToList();

            AssociationViewModel associationViewModel = new AssociationViewModel()
            {
                AssociationModel = associationModel,
                OfficeHierarchyModel = selectableOfficeHierarchyModel,
                VisibleStaffData = visibleStaffData,
                AreaCodes = Business.GetAreaCodes(staffCode),
                StaffCode = staffCode
            };

            return PartialView("_ShowStaffAssociations", associationViewModel);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult GetAvailableProductsForArea(string areaCode, string staffCode)
        {
            DateTime today = Helper.GetCurrentIstDateTime();
            //IEnumerable<DownloadProductEx> productData = Business.GetDownloadProductsForArea(areaCode, today);

            // show only 1000 products - Tstanes has 90K + Products and can't put 30+MB of html in response
            IEnumerable<DownloadProductEx> productData = Business.GetProductsWithPrice2(staffCode, areaCode, today);

            int itemsToDisplay = 1000;
            ViewBag.TotalItems = productData.Count();
            ViewBag.DisplayedItems = itemsToDisplay;
            productData = productData.Take(itemsToDisplay);

            ViewBag.AreaCode = areaCode;
            ViewBag.Today = today;

            return View("_ShowProductData", productData);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult GetCustomersForAreaPlusStaffCode(string areaCode, string staffCode)
        {
            IEnumerable<DownloadCustomerExtend> customers = Business.GetCustomersForAreaPlusStaffCode(areaCode, staffCode);
            var model = customers.Where(x => x.Active)
                .Select(x => new CustomerModel()
                {
                    Code = x.Code,
                    CreditLimit = x.CreditLimit,
                    LongOutstanding = x.LongOutstanding,
                    Name = x.Name,
                    Outstanding = x.Outstanding,
                    PhoneNumber = x.PhoneNumber,
                    Type = x.Type,
                    HQCode = x.HQCode,
                    Target = x.Target,
                    Sales = x.Sales,
                    Payment = x.Payment
                }).ToList();

            ViewBag.AreaCode = areaCode;
            ViewBag.StaffCode = staffCode;
            ViewBag.IsSuperAdmin = IsSetupSuperAdmin;
            return View("_Customers", model);
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.CustomerFeature)]
        public ActionResult ShowCustomers()
        {
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.CustomerFeature)]
        public ActionResult GetSearchCustomers(CustomersFilter searchCriteria)
        {
            DomainEntities.CustomersFilter criteria = Helper.ParseCustomersSearchCriteria(searchCriteria);
            IEnumerable<DownloadCustomerExtend> customers = Business.GetCustomersWithLocation(criteria);
            var model = customers.Select(x => new CustomerListModel()
            {
                Code = x.Code,
                CreditLimit = x.CreditLimit,
                LongOutstanding = x.LongOutstanding,
                Name = x.Name,
                Outstanding = x.Outstanding,
                PhoneNumber = x.PhoneNumber,
                Type = x.Type,
                HQCode = x.HQCode,
                Target = x.Target,
                Sales = x.Sales,
                Payment = x.Payment,
                //District = x.District,
                //State = x.State,
                //Branch = x.Branch,
                //Pincode = x.Pincode,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Email = x.Email,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Active = x.Active
            }).ToList();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return PartialView("_ShowCustomers", model);
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.OfficeHierarchyFeature)]
        public ActionResult ShowOfficeHierarchy()
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;
            ViewBag.Zones = Business.GetZones(IsSuperAdmin, officeHierarchy).OrderBy(s => s.CodeName);
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.OfficeHierarchyFeature)]
        public ActionResult GetSearchOfficeHierarchy(OfficeHierarchyFilter searchCriteria)
        {
            DomainEntities.OfficeHierarchyFilter s = ParseOfficeHierarchySearchCriteria(searchCriteria);
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            if (officeHierarchy != null && s != null)
            {
                if (s.ApplyZoneFilter)
                {
                    officeHierarchy = officeHierarchy.Where(o => o.ZoneCode.Equals(s.Zone, StringComparison.OrdinalIgnoreCase));
                }
                if (s.ApplyAreaFilter)
                {
                    officeHierarchy = officeHierarchy.Where(o => o.AreaCode.Equals(s.Area, StringComparison.OrdinalIgnoreCase));
                }
                if (s.ApplyTerritoryFilter)
                {
                    officeHierarchy = officeHierarchy.Where(o => o.TerritoryCode.Equals(s.Territory, StringComparison.OrdinalIgnoreCase));
                }
            }

            IEnumerable<OfficeHierarchyModel> officeHierarchyModel = officeHierarchy
                .OrderBy(x => x.ZoneName)
                .ThenBy(x => x.AreaName)
                .ThenBy(x => x.TerritoryName)
                .ThenBy(x => x.HQName)
                .Select(x => new OfficeHierarchyModel()
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

            return PartialView("_ShowOfficeHierarchy", officeHierarchyModel);
        }

        private DomainEntities.OfficeHierarchyFilter ParseOfficeHierarchySearchCriteria(OfficeHierarchyFilter searchCriteria)
        {
            DomainEntities.OfficeHierarchyFilter s = new DomainEntities.OfficeHierarchyFilter()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
            };
            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = Helper.IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = Helper.IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = Helper.IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            return s;
        }

        [AjaxOnly]
        [HttpGet]
        public EmptyResult TerminateUserAccess(long empId)
        {
            if (Helper.ShowTerminateAndDeleteLinksOnCrmUsersPage)
            {
                RemovePortalAccess(empId);
                Business.TerminateUserAccess(empId);
            }
            return new EmptyResult();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.BankAccountFeature)]
        public ActionResult BankAccounts()
        {
            IEnumerable<OfficeHierarchy> officeHierarchy = GetOfficeHierarchy();
            ViewBag.OfficeHierarchy = officeHierarchy;
            ViewBag.Areas = Business.GetAreas(IsSuperAdmin, officeHierarchy).OrderBy(s => s.CodeName);
            return View();
        }

        private static DomainEntities.BankAccountFilter ParseBankAccountSearchCriteria(BankAccountFilter searchCriteria)
        {
            DomainEntities.BankAccountFilter s = Helper.GetDefaultBankAccountFilter();

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyAreaCodeFilter = Helper.IsValidSearchCriteria(searchCriteria.AreaCode);
            s.AreaCode = searchCriteria.AreaCode;

            s.ApplyBankNameFilter = Helper.IsValidSearchCriteria(searchCriteria.BankName);
            s.BankName = s.ApplyBankNameFilter ? searchCriteria.BankName.Trim() : searchCriteria.BankName;

            return s;
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.BankAccountFeature)]
        public ActionResult GetBankAccounts(BankAccountFilter searchCriteria)
        {
            DomainEntities.BankAccountFilter criteria = ParseBankAccountSearchCriteria(searchCriteria);
            IEnumerable<DashboardBankAccount> account = Business.GetDashboardBankAccount(criteria);

            var model = account.Select(a => CreateBankAccountViewModel(a)).ToList();

            return PartialView("_GetBankAccounts", model);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult AddBankAccount()
        {
            BankAccountViewModel model = new BankAccountViewModel();
            ViewBag.AreaNames = Business.GetCodeTable("AreaOffice");
            return PartialView("_AddBankAccount", model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult AddBankAccount(BankAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //LogModelErrors(nameof(AdminController));  
                ViewBag.AreaNames = Business.GetCodeTable("AreaOffice");
                return PartialView("_AddBankAccount", model);
            }

            DashboardBankAccount eb = new DashboardBankAccount()
            {
                AreaCode = model.AreaCode.CrmTrim(),
                BankName = model.BankName.CrmTrim(),
                BankPhone = model.BankPhone.CrmTrim(),
                BranchName = model.BranchName.CrmTrim(),
                AccountNumber = model.AccountNumber.CrmTrim(),
                IFSC = model.IFSC.CrmTrim(),
                IsActive = model.IsActive,
                CurrentStaffCode = this.CurrentUserStaffCode,
                AccountName = Utils.TruncateString(model.AccountName, 50),
                AccountAddress = Utils.TruncateString(model.AccountAddress, 50),
                AccountEmail = Utils.TruncateString(model.AccountEmail, 50)
            };

            try
            {
                Business.CreateBankAccountData(eb);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                ViewBag.AreaNames = Business.GetCodeTable("AreaOffice");
                return PartialView("_AddBankAccount", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditBankAccount(long id)
        {
            DashboardBankAccount bankAccount = Business.GetBankAccountDetails(id);

            BankAccountViewModel model = new BankAccountViewModel()
            {
                Id = bankAccount.Id,
                AreaCode = bankAccount.AreaCode,
                BankName = bankAccount.BankName,
                BranchName = bankAccount.BranchName,
                BankPhone = bankAccount.BankPhone,
                AccountNumber = bankAccount.AccountNumber,
                IFSC = bankAccount.IFSC,
                IsActive = bankAccount.IsActive,
                AccountName = bankAccount.AccountName,
                AccountAddress = bankAccount.AccountAddress,
                AccountEmail = bankAccount.AccountEmail
            };

            ViewBag.AreaNames = Business.GetCodeTable("AreaOffice");
            return PartialView("_EditBankAccount", model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditBankAccount(BankAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //LogModelErrors(nameof(AdminController));
                ViewBag.AreaNames = Business.GetCodeTable("AreaOffice");
                return PartialView("_EditBankAccount", model);
            }

            DashboardBankAccount eb = new DashboardBankAccount()
            {
                Id = model.Id,
                AreaCode = model.AreaCode.CrmTrim(),
                BankName = model.BankName.CrmTrim(),
                BranchName = model.BranchName.CrmTrim(),
                BankPhone = model.BankPhone.CrmTrim(),
                AccountNumber = model.AccountNumber.CrmTrim(),
                IFSC = model.IFSC.CrmTrim(),
                IsActive = model.IsActive,
                CurrentStaffCode = this.CurrentUserStaffCode,
                AccountName = Utils.TruncateString(model.AccountName, 50),
                AccountAddress = Utils.TruncateString(model.AccountAddress, 50),
                AccountEmail = Utils.TruncateString(model.AccountEmail, 50)
            };

            try
            {
                Business.SaveBankAccountData(eb);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                ViewBag.AreaNames = Business.GetCodeTable("AreaOffice");
                return PartialView("_EditBankAccount", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult AddCustomer()
        {
            CustomerModel model = new CustomerModel();
            ViewBag.HQCode = Business.GetCodeTable("HeadQuarter");
            ViewBag.DealerType = Business.GetCodeTable("DealerType");
            return PartialView("_AddCustomer", model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult AddCustomer(CustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                //LogModelErrors(nameof(AdminController));  
                ViewBag.HQCode = Business.GetCodeTable("HeadQuarter");
                ViewBag.DealerType = Business.GetCodeTable("DealerType");
                return PartialView("_AddCustomer", model);
            }

            DownloadCustomerExtend dc = new DownloadCustomerExtend()
            {
                Code = model.Code.CrmTrim(),
                Name = model.Name.CrmTrim(),
                PhoneNumber = model.PhoneNumber.CrmTrim(),
                Type = model.Type.CrmTrim(),
                CreditLimit = model.CreditLimit,
                Outstanding = model.Outstanding,
                LongOutstanding = model.LongOutstanding,
                Target = model.Target,
                Sales = model.Sales,
                Payment = model.Payment,
                HQCode = model.HQCode.CrmTrim(),
                Address1 = model.Address1.CrmTrim(),
                Address2 = model.Address2.CrmTrim(),
                Email = model.Email.CrmTrim(),
                Active = model.Active
            };

            try
            {
                long customerId = Business.CreateCustomer(dc, CurrentUserStaffCode);
                if (customerId == -1)
                {
                    ModelState.AddModelError("", "Customer already exist.");
                    ViewBag.HQCode = Business.GetCodeTable("HeadQuarter");
                    ViewBag.DealerType = Business.GetCodeTable("DealerType");
                    return PartialView("_AddCustomer", model);
                }

                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                ViewBag.HQCode = Business.GetCodeTable("HeadQuarter");
                ViewBag.DealerType = Business.GetCodeTable("DealerType");
                return PartialView("_AddCustomer", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditCustomer(string Code)
        {
            DownloadCustomerExtend dc = Business.GetCustomerDetails(Code);

            CustomerModel model = new CustomerModel()
            {
                Id = dc.Id,
                Code = dc.Code,
                Name = dc.Name,
                PhoneNumber = dc.PhoneNumber,
                Type = dc.Type,
                CreditLimit = dc.CreditLimit,
                Outstanding = dc.Outstanding,
                LongOutstanding = dc.LongOutstanding,
                Target = dc.Target,
                Sales = dc.Sales,
                Payment = dc.Payment,
                HQCode = dc.HQCode,
                Address1 = dc.Address1,
                Address2 = dc.Address2,
                Email = dc.Email,
                Active = dc.Active
            };

            ViewBag.HQCode = Business.GetCodeTable("HeadQuarter");
            ViewBag.DealerType = Business.GetCodeTable("DealerType");
            return PartialView("_EditCustomer", model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditCustomer(CustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                //LogModelErrors(nameof(AdminController));
                ViewBag.HQCode = Business.GetCodeTable("HeadQuarter");
                ViewBag.DealerType = Business.GetCodeTable("DealerType");
                return PartialView("_EditCustomer", model);
            }

            DownloadCustomerExtend dc = new DownloadCustomerExtend()
            {
                Id = model.Id,
                Code = model.Code.CrmTrim(),
                Name = model.Name.CrmTrim(),
                PhoneNumber = model.PhoneNumber.CrmTrim(),
                Type = model.Type.CrmTrim(),
                CreditLimit = model.CreditLimit,
                Outstanding = model.Outstanding,
                LongOutstanding = model.LongOutstanding,
                Target = model.Target,
                Sales = model.Sales,
                Payment = model.Payment,
                HQCode = model.HQCode.CrmTrim(),
                Address1 = model.Address1.CrmTrim(),
                Address2 = model.Address2.CrmTrim(),
                Email = model.Email.CrmTrim(),
                Active = model.Active
            };

            try
            {
                long status = Business.SaveCustomer(dc, CurrentUserStaffCode);
                if (status == -1)
                {
                    ModelState.AddModelError("", "Invalid Customer Id.");
                    ViewBag.HQCode = Business.GetCodeTable("HeadQuarter");
                    ViewBag.DealerType = Business.GetCodeTable("DealerType");
                    return PartialView("_EditCustomer", model);
                }

                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                ViewBag.HQCode = Business.GetCodeTable("HeadQuarter");
                ViewBag.DealerType = Business.GetCodeTable("DealerType");
                return PartialView("_EditCustomer", model);
            }
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult ShowEntities(string EntityNumber,string EntityName)
        {
            ModelFilter modelFilter = new ModelFilter()
            {
                ClientType = Business.GetCodeTable("CustomerType"),
                EntityNumber = String.IsNullOrEmpty(EntityNumber) ? "" : EntityNumber,
                EntityName = String.IsNullOrEmpty(EntityName) ?  "" : EntityName,
            };

            PutAgreementStatusInViewBag();
            PutBankDetailStatusInViewBag();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View(modelFilter);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult GetDetailedEntityResultForDownload(EntitiesFilter searchCriteria)
        {
            DomainEntities.EntitiesFilter criteria = Helper.ParseEntitiesSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            // get entities
            ICollection<Entity> entities = Business.GetEntities(criteria);

            // get agreements for selected entities.
            IEnumerable<EntityAgreement> entityAgreements = Business.GetEntityAgreements(entities);
            ViewBag.EntityAgreements = entityAgreements;

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            ICollection<EntityModel> model =
                         entities.Select(x => new EntityModel()
                         {
                             Id = x.Id,
                             EmployeeId = x.EmployeeId,
                             DayId = x.DayId,
                             HQCode = x.HQCode,
                             EmployeeCode = x.EmployeeCode,
                             EmployeeName = x.EmployeeName,

                             AtBusiness = x.AtBusiness,
                             //Consent = x.Consent, //swetha amde chnages on 24-11
                             EntityType = x.EntityType,
                             EntityName = x.EntityName,
                             EntityDate = x.EntityDate,
                             Address = x.Address,
                             City = x.City,
                             State = x.State,
                             Pincode = x.Pincode,
                             LandSize = x.LandSize,
                             Latitude = x.Latitude,
                             Longitude = x.Longitude,
                             SqliteEntityId = x.SqliteEntityId,
                             ContactCount = x.ContactCount,
                             CropCount = x.CropCount,
                             ImageCount = x.ImageCount,
                             AgreementCount = x.AgreementCount,
                             BankDetailCount = x.BankDetailCount,
                             SurveyDetailCount = x.SurveryDetailCount,
                             UniqueIdType = x.UniqueIdType,
                             UniqueId = x.UniqueId,
                             TaxId = x.TaxId,

                             FatherHusbandName = x.FatherHusbandName,
                             HQName = x.HQName,
                             TerritoryCode = x.TerritoryCode,
                             TerritoryName = x.TerritoryName,
                             //MajorCrop = x.MajorCrop,
                             //LastCrop = x.LastCrop,
                             //WaterSource = x.WaterSource,
                             //SoilType = x.SoilType,
                             //SowingType = x.SowingType,
                             //SowingDate = x.SowingDate.HasValue ? x.SowingDate.Value : DateTime.MinValue,
                             EntityNumber = x.EntityNumber,
                             IsActive = x.IsActive,
                             DWSCount = x.DWSCount,
                             IssueReturnCount = x.IssueReturnCount,
                             AdvanceRequestCount = x.AdvanceRequestCount,
                             // March 21 2020 - allow edit only for Farmer
                             // as edit form has to be customized for other entity types
                             IsEditAllowed = Utils.SiteConfigData.WorkflowActivityEntityType.Equals(x.EntityType, StringComparison.OrdinalIgnoreCase)
                         }).ToList();

            return PartialView("_DownloadEntitiesWithDetail", model);
        }

        // Added by swetha - To download Entity Contacts and Crops details
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult GetEntityContactsCropsResultForDownload(EntitiesFilter searchCriteria)
        {
            DomainEntities.EntitiesFilter criteria = Helper.ParseEntitiesSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            // get entities
            ICollection<Entity> entities = Business.GetEntities(criteria);

            //get entity contacts
            IEnumerable<EntityContact> entityContacts = Business.GetEntityContacts(entities);
            ViewBag.EntityContact = entityContacts;

            //get entity crops
            IEnumerable<EntityCrop> entityCrops = Business.GetEntityCrops(entities);
            ViewBag.EntityCrop = entityCrops;

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            ICollection<EntityModel> model =
                         entities.Select(x => new EntityModel()
                         {

                             Id = x.Id,
                             EmployeeId = x.EmployeeId,
                             EmployeeCode = x.EmployeeCode,
                             EmployeeName = x.EmployeeName,
                             AtBusiness = x.AtBusiness,
                             EntityType = x.EntityType,
                             EntityName = x.EntityName,
                             EntityNumber= x.EntityNumber,
                             EntityDate = x.EntityDate,
                             HQName = x.HQName,
                             HQCode = x.HQCode,
                             TerritoryCode = x.TerritoryCode,
                             TerritoryName = x.TerritoryName,
                             IsEditAllowed = Utils.SiteConfigData.WorkflowActivityEntityType.Equals(x.EntityType, StringComparison.OrdinalIgnoreCase)
                         }).ToList();

            return PartialView("_DownloadEntitiesWithContactsCrops", model);
        }
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult GetSearchEntities(EntitiesFilter searchCriteria)
        {
            DomainEntities.EntitiesFilter criteria = Helper.ParseEntitiesSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<Entity> entities = Business.GetEntities(criteria);

            ICollection<EntityModel> model =
                         entities.Select(x => new EntityModel()
                         {
                             Id = x.Id,
                             EmployeeId = x.EmployeeId,
                             DayId = x.DayId,
                             HQCode = x.HQCode,
                             EmployeeCode = x.EmployeeCode,
                             EmployeeName = x.EmployeeName,

                             AtBusiness = x.AtBusiness,
                             //Consent = x.Consent, //swetha made changes on 24-11
                             EntityType = x.EntityType,
                             EntityName = x.EntityName,
                             EntityDate = x.EntityDate,
                             Address = x.Address,
                             City = x.City,
                             State = x.State,
                             Pincode = x.Pincode,
                             LandSize = x.LandSize,
                             Latitude = x.Latitude,
                             Longitude = x.Longitude,
                             SqliteEntityId = x.SqliteEntityId,
                             ContactCount = x.ContactCount,
                             CropCount = x.CropCount,
                             ImageCount = x.ImageCount,
                             AgreementCount = x.AgreementCount,
                             BankDetailCount = x.BankDetailCount,
                             SurveyDetailCount = x.SurveryDetailCount, // Added by swetha
                             UniqueIdType = x.UniqueIdType,
                             UniqueId = x.UniqueId,
                             TaxId = x.TaxId,
                             CreatedBy = x.CreatedBy,
                             FatherHusbandName = x.FatherHusbandName,
                             HQName = x.HQName,
                             TerritoryCode = x.TerritoryCode,
                             TerritoryName = x.TerritoryName,
                             //MajorCrop = x.MajorCrop,
                             //LastCrop = x.LastCrop,
                             //WaterSource = x.WaterSource,
                             //SoilType = x.SoilType,
                             //SowingType = x.SowingType,
                             //SowingDate = x.SowingDate.HasValue ? x.SowingDate.Value : DateTime.MinValue,
                             EntityNumber = x.EntityNumber,
                             IsActive = x.IsActive,
                             DWSCount = x.DWSCount,
                             IssueReturnCount = x.IssueReturnCount,
                             AdvanceRequestCount = x.AdvanceRequestCount,
                             // March 21 2020 - allow edit only for Farmer
                             // as edit form has to be customized for other entity types
                             IsEditAllowed = Utils.SiteConfigData.WorkflowActivityEntityType.Equals(x.EntityType, StringComparison.OrdinalIgnoreCase)
                         }).ToList();

            ViewBag.ValueAddedServices = Utils.SiteConfigData.ValueAddedServices;
            return PartialView("_ShowEntities", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult GetEntityContacts(long entityId)
        {
            ICollection<EntityContact> entityContacts = Business.GetEntityContacts(entityId);
            ICollection<EntityContactModel> model =
                         entityContacts.Select(x => new EntityContactModel()
                         {
                             Id = x.Id,
                             EntityId = x.EntityId,
                             Name = x.Name,
                             PhoneNumber = x.PhoneNumber,
                             IsPrimary = x.IsPrimary
                         }).ToList();
            return PartialView("_ShowEntityContacts", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult GetEntityCrops(long entityId)
        {
            ICollection<EntityCrop> entityCrops = Business.GetEntityCrops(entityId);

            ICollection<EntityCropModel> model =
                         entityCrops.Select(x => new EntityCropModel()
                         {
                             Id = x.Id,
                             EntityId = x.EntityId,
                             CropName = x.CropName
                         }).ToList();
            return PartialView("_ShowEntityCrops", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult GetEntityAgreements(long entityId)
        {
            ICollection<EntityAgreement> entityAgreements = Business.GetEntityAgreements(entityId);
            ICollection<EntityAgreementModel> model =
                         entityAgreements.Select(x => new EntityAgreementModel()
                         {
                             Id = x.Id,
                             EntityId = x.EntityId,
                             WorkflowSeasonId = x.WorkflowSeasonId,
                             WorkflowSeasonName = x.WorkflowSeasonName,
                             TypeName = x.TypeName,
                             AgreementNumber = x.AgreementNumber,
                             LandSizeInAcres = x.LandSizeInAcres,
                             Status = x.Status,
                             IsPassBookReceived = x.IsPassbookReceived,
                             PassBookReceivedDate = x.PassbookReceivedDate,
                             RatePerKg = x.RatePerKg,
                             DWSCount = x.DWSCount,
                             IssueReturnCount = x.IssueReturnCount,
                             AdvanceRequestCount = x.AdvanceRequestCount,
                             HasWorkflow = x.HasWorkflow,
                             ActivityId = x.ActivityId,
                             CreatedBy = x.CreatedBy
                         }).ToList();
            ViewBag.ValueAddedServices = Utils.SiteConfigData.ValueAddedServices;
            ViewBag.EntityId = entityId;
            return PartialView("_ShowEntityAgreements", model);
        }


        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult GetEntitySurveys(long entityId)
        {
            ICollection<EntitySurvey> entitySurveys = Business.GetEntitySurveys(entityId);
            ICollection<EntitySurveyModel> model =
                         entitySurveys.Select(x => CreateEntitySurveyModel(null, x)).ToList();

            return PartialView("_ShowEntitySurveys", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult GetEntityBankDetails(long entityId)
        {
            ICollection<EntityBankDetail> bankDetails = Business.GetEntityBankDetails(entityId);

            ICollection<EntityBankDetailModel> model =
                         bankDetails.Select(x => CreateEntityBankDetailModel(x)).ToList();
            ViewBag.EntityId = entityId;
            return PartialView("_ShowEntityBankDetails", model);
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.GstRateFeature)]
        public ActionResult GstRate()
        {
            return View();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.RedFarmerModule)]
        public ActionResult RedFarmer()
        {
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.GstRateFeature)]
        public ActionResult GetGSTRate(GstRateFilter searchCriteria)
        {
            DomainEntities.GstRateFilter criteria = ParseGstRateSearchCriteria(searchCriteria);
            IEnumerable<DashboardGstRate> gst = Business.GetDashboardGstRate(criteria);

            var model = gst.Select(x => new GstRateViewModel()
            {
                Id = x.Id,
                GstCode = x.GstCode,
                GstRate = x.GstRate,
                EffectiveStartDate = x.EffectiveStartDate,
                EffectiveEndDate = x.EffectiveEndDate
            }).ToList();

            return PartialView("_GstRate", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.GstRateFeature)]
        public ActionResult AddGstRate()
        {
            GstRateViewModel model = new GstRateViewModel()
            {
                IsStartDateEditable = true,
                IsEndDateEditable = true,
                IsRateEditable = true,
            };
            //model.EffectiveStartDate = Helper.GetCurrentIstDateTime();
            //model.EffectiveEndDate = new DateTime(2099, 12, 31);

            return PartialView("_AddGstRate", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.GstRateFeature)]
        public ActionResult AddGstRate(GstRateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //LogModelErrors(nameof(AdminController));  
                return PartialView("_AddGstRate", model);
            }

            GstSaveRate gstRate = new GstSaveRate()
            {
                GstCode = model.GstCode,
                GstRate = model.GstRate,
                EffectiveStartDate = model.EffectiveStartDate,
                EffectiveEndDate = model.EffectiveEndDate,
                CurrentStaffCode = this.CurrentUserStaffCode
            };

            try
            {
                var id = Business.CreateGstRate(gstRate);
                if (id > 0)
                {
                    return PartialView("ConfirmMessage");
                }
                else
                {
                    ModelState.AddModelError("EffectiveEndDate", $"Dates for {model.GstCode} overlap with other entry.");
                    return PartialView("_AddGstRate", model);
                }
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("_AddGstRate", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.GstRateFeature)]
        public ActionResult EditGstRate(long id)
        {
            GstRateViewModel model = GetGstRateViewModel(id);
            return PartialView("_EditGstRate", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.GstRateFeature)]
        public ActionResult EditGSTRate(GstRateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //LogModelErrors(nameof(AdminController));
                return PartialView("_EditGstRate", model);
            }

            // need to check for overlaps again, in case user left the edit screen open overnight
            // today = 9/9/18; 
            // GST Rate = 8.35
            // Start Date = 10/9/18 (dd/mm/yyyy)
            // End Date = 20/9/18
            // If user starts editing today, Gst Rate is editable
            // Now user sits on edit screen overnight and today is 10/9/18
            // User submits the form with Gst Rate = 8.85
            // User should not be able to save rate as 8.85 starting 10/9/18

            var origModel = GetGstRateViewModel(model.Id);

            if (origModel.IsRateEditable != model.IsRateEditable ||
                origModel.IsStartDateEditable != model.IsStartDateEditable ||
                origModel.IsEndDateEditable != model.IsEndDateEditable)
            {
                ModelState.AddModelError("", $"State of Data has changed. Please refresh the page and try again.");
                return PartialView("_EditGstRate", model);
            }

            GstSaveRate gstRate = new GstSaveRate()
            {
                Id = model.Id,
                GstCode = model.GstCode,
                GstRate = model.GstRate,
                EffectiveStartDate = model.EffectiveStartDate,
                EffectiveEndDate = model.EffectiveEndDate,
                CurrentStaffCode = this.CurrentUserStaffCode
            };

            try
            {
                var id = Business.UpdateGstRate(gstRate);
                if (id > 0)
                {
                    return PartialView("ConfirmMessage");
                }
                else
                {
                    ModelState.AddModelError("EffectiveEndDate", $"Dates for {model.GstCode}  may be overlapping with other entries.  Please refresh page and try again.");
                    return PartialView("_EditGstRate", model);
                }
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("_EditGstRate", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditAgreementWorkFlowRec(long workflowDetailId)
        {
            EntityWorkFlowDetailModel rec = GetWorkFlowDetails(0, 0, workflowDetailId)?.FirstOrDefault();

            PutWorkFlowStatusInViewBag();
            return PartialView("_EditAgreementWorkFlowRec", rec);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditAgreementWorkFlowRec(EntityWorkFlowDetailEditModel model)
        {
            try
            {
                ModelState.Clear();
                TryValidateModel(model);

                if (!ModelState.IsValid)
                {
                    Business.LogError($"{nameof(EditAgreementWorkFlowRec)}", "Model State is invalid...");
                    EntityWorkFlowDetailModel rec = GetWorkFlowDetails(0, 0, model.Id)?.FirstOrDefault();
                    PutWorkFlowStatusInViewBag();

                    rec.Notes = model.Notes;
                    rec.PlannedEndDateAsText = model.PlannedEndDateAsText;
                    rec.PlannedFromDateAsText = model.PlannedFromDateAsText;
                    rec.IsActiveAsNumber = model.IsActiveAsNumber;

                    return PartialView("_EditAgreementWorkFlowRec", rec);
                }

                EntityWorkFlowDetail ea = new EntityWorkFlowDetail()
                {
                    Id = model.Id,
                    PlannedFromDate = model.PlannedFromDate,
                    PlannedEndDate = model.PlannedEndDate,
                    IsActive = model.IsActive,
                    CurrentUserStaffCode = this.CurrentUserStaffCode,
                    Notes = Utils.TruncateString(model.Notes, 100)
                };

                Business.SaveWorkFlowDetail(ea);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditAgreementWorkFlowRec), ex);
                ModelState.AddModelError("", ex.Message);

                EntityWorkFlowDetailModel rec = GetWorkFlowDetails(0, 0, model.Id)?.FirstOrDefault();
                PutWorkFlowStatusInViewBag();

                rec.Notes = model.Notes;
                rec.PlannedEndDateAsText = model.PlannedEndDateAsText;
                rec.PlannedFromDateAsText = model.PlannedFromDateAsText;
                rec.IsActiveAsNumber = model.IsActiveAsNumber;

                return PartialView("_EditAgreementWorkFlowRec", rec);
            }
        }

        // Create followup workflow record
        [AjaxOnly]
        [HttpGet]
        public ActionResult AddAgreementWorkFlowRec(long workflowDetailId)
        {
            // retrieve WF Detail rec
            EntityWorkFlowDetailModel rec = GetWorkFlowDetails(0, 0, workflowDetailId)?.FirstOrDefault();
            if (rec == null)
            {
                return PartialView("_CustomError", $"An error has occured (Invalid Id of the record) - please refresh the page and try again.");
            }

            FillAddAgreementWorkFlowViewBag(rec.TypeName, rec.Phase);

            // change rec to new rec mode
            rec.Notes = "";
            rec.IsActiveAsNumber = 1;

            rec.PlannedFromDateAsText =
                rec.PlannedEndDateAsText =
                Helper.GetCurrentIstDateTime().ToString("dd-MM-yyyy");


            return PartialView("_AddAgreementWorkFlowRec", rec);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult AddAgreementWorkFlowRec(EntityWorkFlowDetailEditModel model)
        {
            try
            {
                ModelState.Clear();
                TryValidateModel(model);

                if (!ModelState.IsValid)
                {
                    Business.LogError($"{nameof(AddAgreementWorkFlowRec)}", "Model State is invalid...");
                    EntityWorkFlowDetailModel rec = GetWorkFlowDetails(0, 0, model.Id)?.FirstOrDefault();
                    FillAddAgreementWorkFlowViewBag(rec.TypeName, rec.Phase);

                    rec.Notes = model.Notes;
                    rec.PlannedEndDateAsText = model.PlannedEndDateAsText;
                    rec.PlannedFromDateAsText = model.PlannedFromDateAsText;
                    rec.Phase = model.Phase;
                    rec.IsActiveAsNumber = model.IsActiveAsNumber;

                    return PartialView("_AddAgreementWorkFlowRec", rec);
                }

                EntityWorkFlowDetail ea = new EntityWorkFlowDetail()
                {
                    Id = model.Id,
                    PlannedFromDate = model.PlannedFromDate,
                    PlannedEndDate = model.PlannedEndDate,
                    IsActive = model.IsActive,
                    Phase = model.Phase,
                    CurrentUserStaffCode = this.CurrentUserStaffCode,
                    Notes = Utils.TruncateString(model.Notes, 100)
                };

                Business.AddWorkFlowDetail(ea);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddAgreementWorkFlowRec), ex);
                ModelState.AddModelError("", ex.Message);

                EntityWorkFlowDetailModel rec = GetWorkFlowDetails(0, 0, model.Id)?.FirstOrDefault();
                FillAddAgreementWorkFlowViewBag(rec.TypeName, rec.Phase);

                rec.Notes = model.Notes;
                rec.PlannedEndDateAsText = model.PlannedEndDateAsText;
                rec.PlannedFromDateAsText = model.PlannedFromDateAsText;
                rec.Phase = model.Phase;
                rec.IsActiveAsNumber = model.IsActiveAsNumber;

                return PartialView("_AddAgreementWorkFlowRec", rec);
            }
        }

        private void FillAddAgreementWorkFlowViewBag(string typeName, string phaseName)
        {
            ICollection<string> availableFollowUpPhases = GetFollowUpPhases(typeName, phaseName);

            ViewBag.MaxNotesLength = 100;
            ViewBag.AvailableFollowUpPhases = availableFollowUpPhases;

            ViewBag.WorkFlowStatus = Business.GetCodeTable("WorkflowStatus");
        }

        private void PutWorkFlowStatusInViewBag()
        {
            ViewBag.MaxNotesLength = 100;
            ViewBag.WorkFlowStatus = Business.GetCodeTable("WorkflowStatus");
        }

        private ICollection<string> GetFollowUpPhases(string typeName, string phaseName)
        {
            // retrieve wf Follow up record
            ICollection<DomainEntities.WorkFlowFollowUp> wfSchedules = Business.GetWorkFlowFollowUp();

            var scheduleRec = wfSchedules.Where(x => x.TypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase) &&
                                    x.Phase.Equals(phaseName, StringComparison.OrdinalIgnoreCase) &&
                                    String.IsNullOrEmpty(x.FollowUpPhaseTag) == false).FirstOrDefault();

            if (scheduleRec == null)
            {
                return null;

            }

            // now get the list of available phases as follow up
            ICollection<CodeTableEx> codeTable = Business.GetCodeTable("WorkFlowFollowUp");
            ICollection<string> possibleFollowUpPhases = codeTable
                                    .Where(x => x.CodeName.Equals(scheduleRec.FollowUpPhaseTag, StringComparison.OrdinalIgnoreCase))
                                    .OrderBy(x => x.DisplaySequence)
                                    .Select(x => x.Code).ToList();

            return possibleFollowUpPhases;
        }

        private GstRateViewModel GetGstRateViewModel(long id)
        {
            DateTime currentIstDate = Helper.GetCurrentIstDateTime().Date;
            DashboardGstRate gst = Business.GetGstRateDetails(id);
            if (gst == null)
            {
                throw new ArgumentException($"Invalid Gst Item");
            }

            return new GstRateViewModel()
            {
                Id = gst.Id,
                GstCode = gst.GstCode,
                GstRate = gst.GstRate,
                EffectiveStartDate = gst.EffectiveStartDate,
                EffectiveEndDate = gst.EffectiveEndDate,
                IsRateEditable = currentIstDate < gst.EffectiveStartDate,
                IsStartDateEditable = currentIstDate < gst.EffectiveStartDate,
                IsEndDateEditable = currentIstDate <= gst.EffectiveEndDate,
                EffectiveStartDateAsText = gst.EffectiveStartDate.ToString("dd-MM-yyyy"),
                EffectiveEndDateAsText = gst.EffectiveEndDate.ToString("dd-MM-yyyy")
            };
        }

        private static DomainEntities.GstRateFilter ParseGstRateSearchCriteria(GstRateFilter searchCriteria)
        {
            DomainEntities.GstRateFilter s = new DomainEntities.GstRateFilter()
            {
                ApplyGstCodeFilter = false,
                ApplyDateFilter = false
            };
            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyGstCodeFilter = Helper.IsValidSearchCriteria(searchCriteria.SearchGstCode);
            s.GstCode = searchCriteria.SearchGstCode;

            // parse dates
            if (String.IsNullOrEmpty(searchCriteria.SearchDate))
            {
                searchCriteria.SearchDate = "01-01-0001";
            }

            var culture = CultureInfo.CreateSpecificCulture("en-GB");
            DateTime fromDate;
            bool isValidFromDate = DateTime.TryParse(searchCriteria.SearchDate, culture, DateTimeStyles.None, out fromDate);

            if (isValidFromDate && fromDate > DateTime.MinValue)
            {
                s.ApplyDateFilter = true;
                s.SearchDate = fromDate;
            }

            return s;
        }

        private void FillEditEntityViewBag()
        {
            ViewBag.HeadQuarters = Business.GetHeadQuarters(IsSuperAdmin, GetOfficeHierarchy());
            //ViewBag.WaterSources = Business.GetCodeTable("WaterSource");
            //ViewBag.SoilTypes = Business.GetCodeTable("SoilType");
            //ViewBag.MajorCrops = Business.GetCodeTable("MajorCrop");
            //ViewBag.LastCrops = Business.GetCodeTable("LastCrop");
            //ViewBag.SowingTypes = Business.GetCodeTable("SowingType");
        }

        private static EntityBankDetailModel CreateEntityBankDetailModel(EntityBankDetail inputRec)
        {
            return new EntityBankDetailModel()
            {
                Id = inputRec.Id,
                EntityId = inputRec.EntityId,
                IsSelfAccount = inputRec.IsSelfAccount,
                AccountHolderName = inputRec.AccountHolderName,
                AccountHolderPAN = inputRec.AccountHolderPAN,
                BankName = inputRec.BankName,
                BankAccount = inputRec.BankAccount,
                BankIFSC = inputRec.BankIFSC,
                BankBranch = inputRec.BankBranch,
                ImageCount = inputRec.ImageCount,
                IsActive = inputRec.IsActive,
                Status = inputRec.Status,
                IsApproved = inputRec.IsApproved,
                Comments = inputRec.Comments
            };
        }

        private static ICollection<IssueReturnModel> GetIssueReturns(long entityId, long agreementId)
        {
            DomainEntities.SearchCriteria issueReturnSearchCriteria = Helper.GetDefaultSearchCriteria();
            if (entityId > 0)
            {
                issueReturnSearchCriteria.ApplyEntityIdFilter = true;
                issueReturnSearchCriteria.EntityId = entityId;
            }

            if (agreementId > 0)
            {
                issueReturnSearchCriteria.ApplyAgreementIdFilter = true;
                issueReturnSearchCriteria.AgreementId = agreementId;
            }

            ICollection<IssueReturn> issueReturns = Business.GetIssueReturns(issueReturnSearchCriteria);
            return issueReturns.Select(x => Helper.CreateNewIssueReturnModel(x))
                                            .ToList();
        }

        private static ICollection<AdvanceRequestModel> GetAdvanceRequests(long entityId, long agreementId)
        {
            DomainEntities.AdvanceRequestFilter searchCriteria = Helper.GetDefaultAdvanceRequestFilter();
            if (entityId > 0)
            {
                searchCriteria.ApplyEntityIdFilter = true;
                searchCriteria.EntityId = entityId;
            }

            if (agreementId > 0)
            {
                searchCriteria.ApplyAgreementIdFilter = true;
                searchCriteria.AgreementId = agreementId;
            }

            ICollection<AdvanceRequest> advanceRequest = Business.GetAdvanceRequests(searchCriteria);
            return advanceRequest.Select(x => Helper.CreateNewAdvanceRequestModel(x))
                                            .ToList();
        }

        private static ICollection<EntityWorkFlowDetailModel> GetWorkFlowDetails(long entityId,
                                                long agreementId, long workFlowDetailId = 0)
        {
            DomainEntities.SearchCriteria searchCriteria = Helper.GetDefaultSearchCriteria();
            if (entityId > 0)
            {
                searchCriteria.ApplyEntityIdFilter = true;
                searchCriteria.EntityId = entityId;
            }

            if (agreementId > 0)
            {
                searchCriteria.ApplyAgreementIdFilter = true;
                searchCriteria.AgreementId = agreementId;
            }

            if (workFlowDetailId > 0)
            {
                searchCriteria.ApplyIdFilter = true;
                searchCriteria.FilterId = workFlowDetailId;
            }

            ICollection<EntityWorkFlowDetail> entityWorkFlowDetails = Business.GetEntityWorkFlowDetails(searchCriteria);
            return entityWorkFlowDetails.Select(x => Helper.CreateNewWorkFlowDetailModel(x))
                                            .ToList();
        }

        private static BankAccountViewModel CreateBankAccountViewModel(DashboardBankAccount ba)
        {
            return new BankAccountViewModel()
            {
                Id = ba.Id,
                AreaCode = ba.AreaCode,
                AreaName = ba.AreaName,
                BankName = ba.BankName,
                BranchName = ba.BranchName,
                BankPhone = ba.BankPhone,
                AccountNumber = ba.AccountNumber,
                IFSC = ba.IFSC,
                IsActive = ba.IsActive,
                AccountName = ba.AccountName,
                AccountAddress = ba.AccountAddress,
                AccountEmail = ba.AccountEmail
            };
        }

        private void SetStaffViewBagData()
        {
            ViewBag.HeadQuarters = Business.GetCodeTable("HeadQuarter");
            ViewBag.Grades = Business.GetCodeTable("Grade");
            ViewBag.GradesEnabled = Helper.IsGradeFeatureEnabled();
            ViewBag.Department = Business.GetCodeTable("Department");
            ViewBag.Designation = Business.GetCodeTable("Designation");
            ViewBag.Ownership = Business.GetCodeTable("OwnershipType");
            ViewBag.VehicleType = Business.GetCodeTable("VehicleType");
            ViewBag.FuelType = Business.GetCodeTable("FuelType");
            ViewBag.BusinessRoles = Business.GetCodeTable("BusinessRole");
        }

        private EntitySurveyModel CreateEntitySurveyModel(Entity entityRec, EntitySurvey es)
        {
            EntitySurveyModel esm = null;
            if (es != null)
            {
                esm = new EntitySurveyModel()
                {
                    Id = es.Id,
                    EntityId = es.EntityId,
                    WorkflowSeasonId = es.WorkflowSeasonId,
                    WorkflowSeasonName = es.WorkflowSeasonName,
                    TypeName = es.TypeName,
                    SurveyNumber = es.SurveyNumber,
                    MajorCrop = es.MajorCrop,
                    LastCrop = es.LastCrop,
                    WaterSource = es.WaterSource,
                    SoilType = es.SoilType,
                    SowingDate = es.SowingDate,
                    LandSizeInAcres = es.LandSizeInAcres,
                    Status = es.Status,
                    ActivityId = es.ActivityId,

                    EntityName = entityRec?.EntityName ?? "",
                    EntityType = entityRec?.EntityType ?? "",
                };
            }
            else
            {
                esm = new EntitySurveyModel();
            }

            return esm;
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult TransporterApproveAmount()
        {
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("TransporterPayment/TransporterApproveAmount");
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult TransporterPreparePaymentFile()
        {
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("TransporterPayment/TransporterPreparePaymentFile");
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult TransporterDownloadPaymentFile()
        {
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("TransporterPayment/TransporterDownloadPaymentFile");
        }

        /// <summary>
        /// Author : Ajith/Rajesh,Bonus calculation search on 22/07/2021
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        /// 
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult ApproveAgreementBonus()
        {

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.Seasons = Business.GetSeasonNamesBonus();
            ViewBag.BonusStatusList = new List<string>()
            {
                BonusStatus.Pending.ToString(),
                BonusStatus.AwaitingApproval.ToString(),
                BonusStatus.Rejected.ToString(),
            };
            ViewBag.SearchResultAction = nameof(GetBonusCalculation);
            ViewBag.Title = "Approve Agreement Bonus";
            return View("BonusCalculation/Index");
        }

        /// <summary>
        /// Author : Ajith/Rajesh,Bonus calculation search on 22/07/2021
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]

        public ActionResult GetBonusCalculation(BonusCalculationFilter searchCriteria)
        {
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            DomainEntities.BonusCalculationFilter criteria = Helper.BonusCalculationSearchCriteriaApproval(searchCriteria);
            criteria.IsSuperAdmin = IsSuperAdmin;
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            if (searchCriteria.BonusStatus == BonusStatus.Pending.ToString())
            {
                IEnumerable<BonusCalculation> BonusAgreements = Business.GetPendingBonusAgreement(criteria);

                IEnumerable<BonusCalculationModel> model = BonusAgreements.Select(x => new BonusCalculationModel()
                {
                    AgreementDate = x.AgreementDate,
                    AgreementNumber = x.AgreementNumber,
                    AgreementId = x.AgreementId,
                    TypeName = x.TypeName,
                    EntityName = x.EntityName,
                    LandSizeInAcres = x.LandSizeInAcres,
                    RatePerKg = x.RatePerKg,
                    NetPayableWt = x.NetPayableWt,
                    NetPayable = x.NetPayable,
                    HQCode = x.HQCode,
                    IsEditAllowed = x.IsEditAllowed,
                    BonusStatus = x.BonusStatus,
                    SeasonName = x.SeasonName
                }).ToList();

                return View("BonusCalculation/_BonusAgreementPendingList", model);
            }
            else
            {
                IEnumerable<BonusCalculation> BonusAgreements = Business.GetBonusAgreement(criteria);

                IEnumerable<BonusCalculationModel> model = BonusAgreements.Select(x => new BonusCalculationModel()
                {
                    AgreementDate = x.AgreementDate,
                    AgreementNumber = x.AgreementNumber,
                    AgreementId = x.AgreementId,
                    TypeName = x.TypeName,
                    SeasonName = x.SeasonName,
                    EntityName = x.EntityName,
                    LandSizeInAcres = x.LandSizeInAcres,
                    RatePerKg = x.RatePerKg,
                    NetPayableWt = x.NetPayableWt,
                    NetPayable = x.NetPayable,
                    BonusRate = x.BonusRate,
                    BonusAmountPayable = x.BonusAmountPayable,
                    BonusAmountPaid = x.BonusAmountPaid,
                    Comments = x.Comments,
                    AccountHolderName = x.AccountHolderName,
                    BankName = x.BankName,
                    BankAccount = x.BankAccount,
                    BankIFSC = x.BankIFSC,
                    BankBranch = x.BankBranch,
                    HQCode = x.HQCode,
                    IsEditAllowed = x.IsEditAllowed,
                    BonusStatus = searchCriteria.BonusStatus
                }).ToList();

                return View("BonusCalculation/_BonusAgreementAwaitingList", model);
            }
        }


        // Rajesh V - 30/07/2021 - Load popup to add additional Bonus data & Save
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult AddBonusData(long agreeId)
        {
            ViewBag.MaxBonusLength = 500;   //Max character length for Notes
            IEnumerable<BonusCalculation> items = Business.GetSingleBonusDetails(agreeId);
            var model =
                items.Select(x => new BonusCalculationModel()
                {
                    EntityId = x.EntityId,
                    AgreementId = x.AgreementId,
                    EntityName = x.EntityName,
                    SeasonName = x.SeasonName,
                    TypeName = x.TypeName,
                    RatePerKg = x.RatePerKg,
                    AgreementNumber = x.AgreementNumber,
                    AgreementDate = x.AgreementDate,
                    LandSizeInAcres = x.LandSizeInAcres,
                    NetPayableWt = x.NetPayableWt,
                    NetPayable = x.NetPayable,
                    YieldPerAcre = Math.Ceiling(x.NetPayableWt / x.LandSizeInAcres),
                    BonusRate = x.BonusRate,
                    BonusAmountPayable = (x.BonusAmountPayable != 0) ? x.BonusAmountPayable : x.NetPayableWt * x.BonusRate,
                    BonusAmountPaid = (x.BonusAmountPaid != 0) ? x.BonusAmountPaid : x.NetPayableWt * x.BonusRate,
                    Comments = x.Comments,
                    HQCode = x.HQCode
                }).FirstOrDefault();

            // retrieve entity bank details
            ICollection<EntityBankDetail> bankAccounts = Business.GetEntityBankDetails(model.EntityId);

            // count the number of active and approved accounts
            model.EntityBankAccounts = bankAccounts.Where(x => x.IsActive && x.IsApproved)
                .Select(x => CreateEntityBankDetailModel(x))
                .ToList();

            return PartialView("BonusCalculation/_AddBonusData", model);
        }

        // Rajesh V - 30/07/2021 - Add or Approve Vendor Payment: Add additional transporter data
        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult AddBonusData(BonusCalculationModel model)
        {
            //if (model.BankId <= 0)
            //{
            //    return PartialView("_CustomError", "Bank Details missing for : " + model.EntityName + ". PLease Add Bank Details and Try Again!!! ");
            //}


            BonusCalculation bonusRecord = new BonusCalculation()
            {
                AgreementId = model.AgreementId,
                AgreementNumber = model.AgreementNumber,
                AgreementDate = model.AgreementDate,
                EntityId = model.EntityId,
                EntityName = model.EntityName,
                HQCode = model.HQCode,
                TypeName = model.TypeName,
                SeasonName = model.SeasonName,
                LandSizeInAcres = model.LandSizeInAcres,
                RatePerKg = model.RatePerKg,
                NetPayableWt = model.NetPayableWt,
                NetPayable = model.NetPayable,
                BonusRate = model.BonusRate,
                BonusAmountPayable = model.BonusAmountPayable,
                BonusAmountPaid = model.BonusAmountPaid,
                BonusStatus = model.BonusStatus,
                Comments = Utils.TruncateString(model.Comments, 500),
                BankId = model.BankId,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedBy = CurrentUserStaffCode,
                UpdatedBy = CurrentUserStaffCode
            };

            try
            {
                Business.CreateBonusDetails(bonusRecord);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddBonusData), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }

        }

        // Rajesh V - 02/08/2021 - Add or Approve Bonus Payment: Approve Agreements with Bonus status as Awaiting Approval
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult ApproveBonusAgreement(IEnumerable<long> agreeId)
        {
            try
            {
                var currentUser = CurrentUserStaffCode;
                Business.MarkBonusAgreementAsApproved(agreeId, currentUser);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(ApproveBonusAgreement), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }

        }

        // Rajesh V - 03/08/2021 - Prepare Bonus Payment File 
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult BonusPreparePaymentFile()
        {

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.Seasons = Business.GetSeasonNamesBonus();
            ViewBag.BonusStatusList = new List<string>()
            {
                BonusStatus.Approved.ToString(),
                BonusStatus.Paid.ToString()
            };
            ViewBag.SearchResultAction = nameof(GetApprovedBonusList);
            ViewBag.Title = "Prepare Bonus Payment File";
            return View("BonusCalculation/Index");
        }

        /// <summary>
        /// Author : Rajesh,Get Approved Bonus agreements based on search on 03/08/2021
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]

        public ActionResult GetApprovedBonusList(BonusCalculationFilter searchCriteria)
        {
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            DomainEntities.BonusCalculationFilter criteria = Helper.BonusCalculationSearchCriteriaApproval(searchCriteria);
            criteria.IsSuperAdmin = IsSuperAdmin;
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            IEnumerable<BonusCalculation> BonusAgreements = Business.GetBonusAgreement(criteria);
            IEnumerable<BonusCalculationModel> model = BonusAgreements.Select(x => new BonusCalculationModel()
            {
                AgreementDate = x.AgreementDate,
                AgreementNumber = x.AgreementNumber,
                AgreementId = x.AgreementId,
                TypeName = x.TypeName,
                EntityName = x.EntityName,
                SeasonName = x.SeasonName,
                LandSizeInAcres = x.LandSizeInAcres,
                BonusRate = x.BonusRate,
                NetPayableWt = x.NetPayableWt,
                NetPayable = x.NetPayable,
                BonusAmountPayable = x.BonusAmountPayable,
                BonusAmountPaid = x.BonusAmountPaid,
                Comments = x.Comments,
                AccountHolderName = x.AccountHolderName,
                BankName = x.BankName,
                BankAccount = x.BankAccount,
                BankIFSC = x.BankIFSC,
                BankBranch = x.BankBranch,
                HQCode = x.HQCode,
                PaymentReference = x.PaymentReference,
                BonusStatus = searchCriteria.BonusStatus
            }).ToList();

            return View("BonusCalculation/_BonusAgreementApprovedList", model);

        }

        // Author : Rajesh,Get Approved Bonus agreements based on search on 03/08/2021
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult MakeBonusPayment(IEnumerable<long> agreeIds)
        {
            if (agreeIds == null || agreeIds.Count() == 0)
            {
                return PartialView("_CustomError", $"Please specify Bonus Agreement Numbers.");
            }

            ICollection<CodeTableEx> paymentTypes = Business.GetCodeTable("PaymentType");
            if ((paymentTypes?.Count ?? 0) == 0)
            {
                return PartialView("_CustomError", $"Requested Action can't be completed, because the payment types are not available.");
            }
            ViewBag.PaymentTypes = paymentTypes;

            IEnumerable<BonusCalculation> BonusAgreements = Business.GetBonusAgreementPayment(agreeIds);
            IEnumerable<BonusCalculationModel> model = BonusAgreements.Select(x => new BonusCalculationModel()
            {
                AgreementDate = x.AgreementDate,
                AgreementNumber = x.AgreementNumber,
                AgreementId = x.AgreementId,
                TypeName = x.TypeName,
                SeasonName = x.SeasonName,
                EntityName = x.EntityName,
                NetPayableWt = x.NetPayableWt,
                BonusAmountPaid = x.BonusAmountPaid,
                HQCode = x.HQCode,
                BonusStatus = x.BonusStatus
            }).ToList();

            DomainEntities.BankAccountFilter sc = Helper.GetDefaultBankAccountFilter();
            IEnumerable<DashboardBankAccount> bankAccounts = Business.GetDashboardBankAccount(sc);
            // count the number of active and approved accounts
            ICollection<BankAccountViewModel> activeBankAccounts = bankAccounts.Where(x => x.IsActive)
                .Select(x => CreateBankAccountViewModel(x))
                .ToList();

            if ((activeBankAccounts?.Count ?? 0) == 0)
            {
                return PartialView("_CustomError", $"Requested Action can't be completed, because Remiiter's bank account does not exist.");
            }

            ViewBag.RemitterBankAccounts = activeBankAccounts;
            ViewBag.MaxNotesLength = 100;

            return PartialView("BonusCalculation/_BonusPayment", model);
        }

        // Rajesh V - 03/08/2021 - Prepare Bonus Payment File: Proceed to payment
        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult MakeBonusPayment(BonusMakePaymentModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorList = GetModelErrors();
                return PartialView("_CustomError", $"Validation error occured while making Bonus payments. Please refresh the page and try again. {errorList}");
            }

            BonusPaymentReferences bonusPaymentRef = new BonusPaymentReferences()
            {
                Comments = model.Comments,
                PaymentReference = Utils.TruncateString(model.PaymentReference, 50),
                AgreementCount = model.BonusPayments.Count(),
                TotalBonusPaid = model.TotalBonusAmount,
                AgreementNumber = Utils.TruncateString(model.AgreementNumbers, 2000),
                CurrentUser = CurrentUserStaffCode,
                BankName = Utils.TruncateString(model.RemitterBankAccount.BankName, 50),
                AccountNumber = Utils.TruncateString(model.RemitterBankAccount.AccountNumber, 50),
                AccountName = Utils.TruncateString(model.RemitterBankAccount.AccountName, 50),
                AccountAddress = Utils.TruncateString(model.RemitterBankAccount.AccountAddress, 50),
                AccountEmail = Utils.TruncateString(model.RemitterBankAccount.AccountEmail, 50),
                PaymentType = Utils.TruncateString(model.PaymentType, 50),
                SenderInfo = Utils.TruncateString(model.SenderInfo, 50),
            };
            try
            {
                Business.CreateBonusPaymentReference(bonusPaymentRef);

                // Mark each of the Bonus agreement as paid
                var bonusRecord = model.BonusPayments.Select(x => new BonusCalculation()
                {
                    AgreementId = x.AgreementId,
                    UpdatedBy = CurrentUserStaffCode,
                    PaymentReference = bonusPaymentRef.PaymentReference,
                    BonusStatus = BonusStatus.Paid.ToString()
                }).ToList();

                Business.MarkBonusAsPaid(bonusRecord);

                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(MakeSTRPayment), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }

        }

        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult BonusDownloadPaymentFile()
        {
            ViewBag.SearchResultAction = nameof(GetBonusPaymentReferences);
            ViewBag.DownloadAction = nameof(DownloadBonusPaymentReference);
            ViewBag.Title = "Download Bonus Payment File";

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("DWSDownloadPaymentFile/Index");
        }

        // Rajesh V - 04/08/2021 - Download Bonus Payment File: Load search result
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult GetBonusPaymentReferences(PaymentReferenceFilter searchCriteria)
        {
            DomainEntities.PaymentReferenceFilter criteria = Helper.ParsePaymentRefSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            ICollection<BonusPaymentReferences> items = Business.GetBonusPaymentReference(criteria);

            ICollection<BonusPaymentReferenceViewModel> model =
                items.Select(x => new BonusPaymentReferenceViewModel()
                {
                    Id = x.Id,
                    AgreementNumber = x.AgreementNumber,
                    AgreementCount = x.AgreementCount,
                    Comments = x.Comments,
                    PaymentReference = x.PaymentReference,
                    TotalBonusPaid = x.TotalBonusPaid,
                    CreatedBy = x.CreatedBy,
                    DateCreated = x.DateCreated,
                    BankName = x.BankName,
                    AccountNumber = x.AccountNumber,
                    AccountName = x.AccountName,
                    AccountAddress = x.AccountAddress,
                    AccountEmail = x.AccountEmail,
                    PaymentType = x.PaymentType,
                    SenderInfo = x.SenderInfo
                }).ToList();

            return PartialView("BonusCalculation/_ListPaid", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.BonusCalculationPaymentOption)]
        public ActionResult DownloadBonusPaymentReference(PaymentReferenceFilter searchCriteria)
        {
            string paymentRef = searchCriteria.PaymentReference;
            ICollection<BonusDownloadData> downloadData = Business.GetBonusDownloadData(paymentRef);

            IEnumerable<BonusDownloadModel> model = downloadData.Select(x => new BonusDownloadModel()
            {

                AgreementNumber = x.AgreementNumber,
                AgreementDate = x.AgreementDate,
                EntityName = x.EntityName,
                TypeName = x.TypeName,
                SeasonName = x.SeasonName,
                BonusRate = x.BonusRate,
                NetWeight = x.NetWeight,
                BonusPaid = x.BonusPaid,
                Comments = x.Comments,
                PaymentDate = x.DateCreated,
                PaymentReference = x.PaymentReference,
                RemitterAccount = x.AccountNumber,
                RemitterName = x.AccountName,
                RemitterAddress = x.AccountAddress,
                BankName = x.BankName,
                SenderInfo = x.SenderInfo,
                RemitterEmail = x.AccountEmail
            }).ToList();

            return PartialView("BonusCalculation/_Download", model);

        }


        // Kartik - 15/09/2021 - Projects screen (Create, Update, Read)

        [CheckRightsAuthorize(Feature = FeatureEnum.ProjectOption)]
        public ActionResult ShowProjects()
        {
            PutProjectCategoryInViewBag();
            PutProjectStatusInViewBag();

            return View("Project/Index");
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.ProjectOption)]
        public ActionResult GetSearchProjects(ProjectsFilter searchCriteria)
        {
            DomainEntities.ProjectsFilter criteria = Helper.ParseProjectSearchCriteria(searchCriteria);
            IEnumerable<Projects> projects = Business.GetProjects(criteria);
            var model = projects.Select(x => new ProjectViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Category = x.Category,
                PlannedStartDate = x.PlannedStartDate,
                PlannedEndDate = x.PlannedEndDate,
                ActualStartDate = x.ActualStartDate,
                ActualEndDate = x.ActualEndDate,
                Status = x.Status,
                CyclicCount = x.CyclicCount,
                IsActive = x.IsActive,
            }).ToList();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return PartialView("Project/_ShowProjects", model);
        }


        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.ProjectOption)]
        public ActionResult EditProject(long projectId)
        {
            DomainEntities.Projects rec = Business.GetSingleProject(projectId);

            ViewBag.IsEditAllowed = IsSuperAdmin;

            ProjectViewModel model =
                         new ProjectViewModel()
                         {
                             Id = rec.Id,
                             Name = rec.Name,
                             Description = rec.Description,
                             Category = rec.Category,
                             PlannedStartDate = rec.PlannedStartDate,
                             PlannedEndDate = rec.PlannedEndDate,
                             ActualStartDate = rec.ActualStartDate,
                             ActualEndDate = rec.ActualEndDate,
                             Status = rec.Status,
                             CyclicCount = rec.CyclicCount,
                             IsActive = rec.IsActive
                         };

            model.PlannedStartDateAsText = model.PlannedStartDate.ToString("dd-MM-yyyy");
            model.PlannedEndDateAsText = model.PlannedEndDate.ToString("dd-MM-yyyy");

            PutProjectCategoryInViewBag();
            PutProjectStatusInViewBag();
            return PartialView("Project/_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.ProjectOption)]
        public ActionResult EditProject(ProjectViewModel model)
        {
            PutProjectCategoryInViewBag();
            PutProjectStatusInViewBag();

            //ViewBag.IsEditAllowed = true;

            if (!ModelState.IsValid)
            {
                return PartialView("Project/_Edit", model);
            }

            DomainEntities.Projects rec = (model.Id > 0) ?
                                Business.GetSingleProject(model.Id) :
                                new DomainEntities.Projects();

            rec.Name = Utils.TruncateString(model.Name, 50);
            rec.Description = Utils.TruncateString(model.Description, 200);
            rec.Category = model.Category;
            rec.PlannedStartDate = model.PlannedStartDate;
            rec.PlannedEndDate = model.PlannedEndDate;
            rec.ActualStartDate = model.ActualStartDate;
            rec.ActualEndDate = model.ActualEndDate;
            rec.Status = model.Status;
            rec.IsActive = model.IsActive;

            rec.CurrentUser = CurrentUserStaffCode;

            rec.CyclicCount = model.CyclicCount;

            try
            {
                DBSaveStatus status = Business.SaveProjectData(rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("Project/_Edit", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditProject), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("Project/_Edit", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.ProjectOption)]
        public ActionResult AddProject()
        {
            ProjectViewModel model =
                         new ProjectViewModel()
                         {
                             Id = 0,
                             Name = "",
                             PlannedStartDate = Helper.GetCurrentIstDateTime(),
                             PlannedEndDate = Helper.GetCurrentIstDateTime(),
                             CyclicCount = 0
                         };

            ViewBag.IsEditAllowed = true;
            model.PlannedStartDateAsText = model.PlannedStartDate.ToString("dd-MM-yyyy");
            model.PlannedEndDateAsText = model.PlannedEndDate.ToString("dd-MM-yyyy");

            PutProjectCategoryInViewBag();
            PutProjectStatusInViewBag();


            return PartialView("Project/_Edit", model);
        }

        private void PutProjectCategoryInViewBag()
        {
            ViewBag.ProjectCategory = Business.GetCodeTable("ProjectCategory");
        }

        private void PutProjectStatusInViewBag()
        {
            ViewBag.ProjectStatus = Business.GetCodeTable("ProjectStatus");
        }

        private void PutEmployeeListViewBag()
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.User);
            IEnumerable<string> visibleStaffCodes = Business.GetVisibleStaffCodes(securityContext.Item1, securityContext.Item2);
            IEnumerable<SalesPersonModel> salesPersonData = Business.GetSelectedSalesPersonData(visibleStaffCodes);

            ViewBag.SalesPersons = salesPersonData;
        }


        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.ProjectOption)]
        public ActionResult GetProjectAssignments(long Id, string rowId, string parentRowId) // here it is Project Id
        {
            DomainEntities.Projects rec = Business.GetSingleProject(Id);
            ICollection<DomainEntities.ProjectAssignments> items = Business.GetProjectAssignment(Id);

            ICollection<ProjectAssignmentModel> model =
                         items.Select(x => new ProjectAssignmentModel()
                         {
                             Id = x.Id,
                             XRefProjectId = x.XRefProjectId,
                             EmployeeId = x.EmployeeId,
                             EmployeeCode = x.EmployeeCode,
                             EmployeeName = x.EmployeeName,
                             StartDate = x.StartDate,
                             EndDate = x.EndDate,
                             IsAssigned = x.IsAssigned,
                             IsSelfAssigned = x.IsSelfAssigned,
                             Comments = x.Comments,
                             DateCreated = x.DateCreated,
                             DateUpdated = x.DateUpdated,
                             CreatedBy = x.CreatedBy,
                             UpdatedBy = x.UpdatedBy
                         }).ToList();

            ViewBag.ProjectId = Id;
            ViewBag.ProjectName = rec?.Name ?? "";

            ViewBag.RowId = rowId;
            ViewBag.ParentRowId = parentRowId;
            return PartialView("Project/_ListAssignment", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.ProjectOption)]
        public ActionResult EditProjectAssignment(long projectId, long assignmentId)
        {
            DomainEntities.Projects p = Business.GetSingleProject(projectId);
            DomainEntities.ProjectAssignments rec = Business.GetSingleProjectAssignment(projectId, assignmentId);

            ViewBag.IsEditAllowed = IsSuperAdmin;

            ProjectAssignmentModel model =
                         new ProjectAssignmentModel()
                         {
                             Id = rec.Id,
                             XRefProjectId = rec.XRefProjectId,
                             ProjectName = p.Name,
                             EmployeeId = rec.EmployeeId,
                             EmployeeCode = rec.EmployeeCode,
                             EmployeeName = rec.EmployeeName,
                             StartDate = rec.StartDate,
                             EndDate = rec.EndDate,
                             IsAssigned = rec.IsAssigned,
                             IsSelfAssigned = rec.IsSelfAssigned,
                             Comments = rec.Comments,
                             DateCreated = rec.DateCreated,
                             DateUpdated = rec.DateUpdated,
                             CreatedBy = rec.CreatedBy,
                             UpdatedBy = rec.UpdatedBy
                         };

            model.StartDateAsText = model.StartDate.ToString("dd-MM-yyyy");
            model.EndDateAsText = model.EndDate.ToString("dd-MM-yyyy");

            PutEmployeeListViewBag();

            return PartialView("Project/_EditAssignment", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.ProjectOption)]
        public ActionResult EditProjectAssignment(ProjectAssignmentModel model)
        {

            PutEmployeeListViewBag();


            if (!ModelState.IsValid)
            {
                return PartialView("Project/_EditAssignment", model);
            }


            EmployeeRecord empRec = Business.GetTenantEmployee(model.EmployeeCode);

            if (empRec == null)
            {
                return PartialView("_CustomError", $"User Id: {model.EmployeeCode} not registered on MyDay.  Please inform user to register on MyDay App and try again.");
            }

            IEnumerable<DomainEntities.ProjectAssignments> projectAssignments = Business.GetSingleProjectEmployeeAssignment(model.XRefProjectId, empRec.EmployeeId);

            foreach (var Id in projectAssignments)
            {
                if (model.StartDate <= Id.EndDate && Id.StartDate <= model.EndDate && model.Id != Id.Id)
                {
                    return PartialView("_CustomError", "Assignments dates are overlapping.");
                }
            }

            DomainEntities.ProjectAssignments rec = (model.Id > 0) ?
                                Business.GetSingleProjectAssignment(model.XRefProjectId, model.Id) :
                                new DomainEntities.ProjectAssignments();

            rec.Id = model.Id;
            rec.XRefProjectId = model.XRefProjectId;
            rec.EmployeeId = empRec.EmployeeId;
            rec.StartDate = model.StartDate;
            rec.EndDate = model.EndDate;
            rec.IsAssigned = model.IsAssigned;
            rec.IsSelfAssigned = model.EmployeeCode == CurrentUserStaffCode ? true : false;
            rec.Comments = Utils.TruncateString(model.Comments, 2000);
            rec.CurrentUser = CurrentUserStaffCode;


            try
            {
                DBSaveStatus status = Business.SaveProjectAssignmentData(rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("Project/_EditAssignment", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditProjectAssignment), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("Project/_EditAssignment", model);
            }
        }


        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.ProjectOption)]
        public ActionResult AddProjectAssignment(long projectId)
        {
            DomainEntities.Projects p = Business.GetSingleProject(projectId);
            ProjectAssignmentModel model =
                         new ProjectAssignmentModel()
                         {
                             Id = 0,
                             XRefProjectId = projectId,
                             ProjectName = p.Name,
                             EmployeeName = "",
                             StartDate = Helper.GetCurrentIstDateTime(),
                             EndDate = Helper.GetCurrentIstDateTime(),
                         };

            ViewBag.IsEditAllowed = true;
            model.StartDateAsText = model.StartDate.ToString("dd-MM-yyyy");
            model.EndDateAsText = model.EndDate.ToString("dd-MM-yyyy");

            PutEmployeeListViewBag();

            //PutProjectCategoryInViewBag();
            //PutProjectStatusInViewBag();


            return PartialView("Project/_EditAssignment", model);
        }

        //Author:Gagana, Purpose:Get details of new Agreement popup; Date:08-02-2023
        [AjaxOnly]
        [HttpGet]
        public ActionResult AddAgreement(long entityId)
        {
            EntityAddAgreementModel model =
                          new EntityAddAgreementModel();
            ViewBag.ActiveCrops = Business.GetEntityActiveCrops(entityId);
            return PartialView("_AddAgreement", model);
        }

        //Author:Gagana, Purpose:Save details of new Agreement details; Date:08-02-2023
        [AjaxOnly]
        [HttpPost]
        public ActionResult AddAgreement(EntityAddAgreementModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ActiveCrops = Business.GetEntityActiveCrops(model.EntityId);
                return PartialView("_AddAgreement", model);
            }
            EntityAgreement ea = new EntityAgreement()
            {
                EntityId = model.EntityId,
                WorkflowSeasonId = model.WorkflowSeasonId,
                ZoneCode = model.ZoneCode,
                AreaCode = model.AreaCode,
                TerritoryCode = model.TerritoryCode,
                HQCode = model.HQCode,
                ZoneName = model.ZoneName,
                AreaName = model.AreaName,
                TerritoryName = model.TerritoryName,
                HQName = model.HQName,
                LandSizeInAcres = model.LandSizeInAcres,
                PassbookReceivedDate = model.PassBookReceivedDate,
                EmployeeId = model.EmployeeId
            };
            try
            {
                Business.AddAgreement(ea, this.CurrentUserStaffCode);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                ViewBag.ActiveCrops = Business.GetEntityActiveCrops(model.EntityId);
                return PartialView("_AddAgreement", model); 
            }
        }

        //Author:Gagana, Purpose:Get Employee Detials for Add Agreement screen; Date:15-02-2023

        [AjaxOnly]
        [HttpGet]
        public ActionResult GetSearchSalesPersonsDetails(StaffFilter searchCriteria)
        
        {
            DomainEntities.StaffFilter criteria = Helper.ParseStaffSearchCriteria(searchCriteria);
            criteria.ApplyAssociationFilter = false;
            criteria.ApplyStatusFilter = false;
            IEnumerable<SalesPersonModel> salesPersonData = Business.GetSalesPersonData(criteria);
            var model = salesPersonData.Where(x => x.EmployeeId != 0 && x.IsActive != false).Select(spd => new SalesPersonViewModel()
            {
                Name = spd.Name + "(" + spd.StaffCode + ")",
                EmployeeId = spd.EmployeeId
            }).ToList();    

            return Json(model, JsonRequestBehavior.AllowGet);
        }

       
        //Author:Gowtham S , Purpose:Add Bank Account details ; Date:27-02-2023
        [AjaxOnly]
        [HttpGet]
        public ActionResult AddBankDetails(long entityId)
        {
            EntityBankDetailModel model = new EntityBankDetailModel();
            DomainEntities.Entity entityRec = Business.GetSingleEntity(entityId);
            ViewBag.CustomerBank = Business.GetCodeTable("CustomerBank");
            model.EntityId = entityId;
            model.EntityName = entityRec.EntityName;
            model.IsSelfAccount = true;
            model.IsActive = true;
            model.Status = "Pending";           
            return PartialView("_AddBankDetails", model);
        }

        //Author:Gowtham S , Purpose:Add Bank Account details ; Date:27-02-2023

        [AjaxOnly]
        [HttpPost]
        public ActionResult AddBankDetails(EntityBankDetailModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Business.LogError($"{nameof(AddBankDetails)}", "Model State is invalid...");
                    ViewBag.CustomerBank = Business.GetCodeTable("CustomerBank");
                    return PartialView("_AddBankDetails", model);
                }

                byte[] imageBytes = ConvertToBytes(model.ImageUpload);
                // string filename = Business.SaveImageDataInFile(imageBytes, Helper.EntityImageFilePrefix);
                string fileName = Business.SaveImageDataInFile(imageBytes,"WebActivity_");


                EntityBankDetail ea = new EntityBankDetail()
                {
                    EntityId = model.EntityId,
                    Status = model.Status,
                    Comments = Utils.TruncateString(model.Comments, 100),
                    IsActive = model.IsActive,
                    IsApproved = "Approved".Equals(model.Status, StringComparison.OrdinalIgnoreCase),
                    IsSelfAccount = model.IsSelfAccount,
                    AccountHolderName = Utils.TruncateString(model.AccountHolderName, 50),
                    AccountHolderPAN = Utils.TruncateString(model.AccountHolderPAN, 50),
                    BankName = Utils.TruncateString(model.BankName, 50),
                    BankAccount = Utils.TruncateString(model.BankAccount, 50),
                    BankIFSC = Utils.TruncateString(model.BankIFSC, 50),
                    BankBranch = Utils.TruncateString(model.BankBranch, 50),
                    ImageCount = model.ImageCount
                };

               Business.SaveAddBankDetail(ea, this.CurrentUserStaffCode, fileName);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddBankDetails), ex);
                ModelState.AddModelError("", ex.Message);
                ViewBag.CustomerBank = Business.GetCodeTable("CustomerBank");
                return PartialView("_AddBankDetails", model);
            }
        }
        public byte[] ConvertToBytes(string image)
        {
            string base64Data = image.Split(',')[1];
            byte[] binaryData = Convert.FromBase64String(base64Data);
            return binaryData;
        }
  

        //Author:Raj Kumar M, Purpose:To add New Profile; Date:21-02-2023

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult AddEntity()
        {
            ViewBag.crops = Business.GetCodeTable("CropType").ToList();
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            EntityCreateModel model = new EntityCreateModel();
            model.CreatedBy = CurrentUserStaffCode;
            return PartialView("_AddEntity", model);
        }

        //Author:Raj Kumar M, Purpose:To add New Profile; Date:21-02-2023
        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult AddEntity(EntityCreateModel entity)
        {
             bool uniqueIdExist = Business.IsDuplicateEntityUniqueId(entity.Id, entity.UniqueId.CrmTrim());

            if (!ModelState.IsValid || uniqueIdExist)
            {
                ViewBag.Crops = Business.GetCodeTable("CropType").ToList();
                ViewBag.OfficeHierarchy = GetOfficeHierarchy();
                if (uniqueIdExist)
                {
                    ModelState.AddModelError("UniqueId", "UniqueId (Aadhar) Already Exist");
                }
                return PartialView("_AddEntity",entity);
            }

            DomainEntities.EmployeeRecord empId = Business.GetTenantEmployee(entity.EmployeeCode);
            entity.EmployeeId = empId.EmployeeId;

            DomainEntities.Entity entityRec = new DomainEntities.Entity()
            {
                EmployeeId = entity.EmployeeId,
                HQCode = entity.HQCode,
                EmployeeCode = entity.EmployeeCode,
                EmployeeName = entity.EmployeeName,
                EntityName = entity.EntityName,
                LandSize = entity.LandSize,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                CropCount = entity.Crops.Count(),
                UniqueId = entity.UniqueId,
                CurrentUser = CurrentUserStaffCode,
                FatherHusbandName = entity.FatherHusbandName,
                HQName = entity.HQName,
                TerritoryCode = entity.TerritoryCode,
                TerritoryName = entity.TerritoryName,

            };

            try
            {
                Business.AddEntityData(entityRec, entity.Crops, entity.Number);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditEntity), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("_AddEntity", entity);
            }
        }

        //Author:Raj Kumar M, Purpose:To Get all Salesperson Assigned to HQ; Date:21-02-2023
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.EntityFeature)]
        public ActionResult GetSelectedSalesPersons(StaffFilter searchCriteria)
        {
            DomainEntities.StaffFilter criteria = Helper.ParseStaffSearchCriteria(searchCriteria);
            criteria.ApplyAssociationFilter = false;
            criteria.ApplyStatusFilter = false;
            IEnumerable<SalesPersonModel> salesPersonData = Business.GetSalesPersonData(criteria);

            var model = salesPersonData.Where(x => x.EmployeeId != 0 && x.IsActive != false).Select(spd => new SalesPersonViewModel()
            {
                Name = spd.Name + "(" + spd.StaffCode + ")",
                StaffCode = spd.StaffCode,
                HQCode = spd.HQCode,
            }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }

}
