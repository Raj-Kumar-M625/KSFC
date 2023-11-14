using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDCS_TG.API.Migrations
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalDetailsId = table.Column<int>(type: "int", nullable: false),
                    FamilyAccept = table.Column<bool>(type: "bit", nullable: true),
                    Contact = table.Column<bool>(type: "bit", nullable: true),
                    WorkingTransgender = table.Column<bool>(type: "bit", nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CulturalFlair = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CulturalFlairOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SexWorkProfession = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManktiProfession = table.Column<bool>(type: "bit", nullable: true),
                    KPSandotherdepartment = table.Column<bool>(type: "bit", nullable: true),
                    CitizensSupport = table.Column<bool>(type: "bit", nullable: true),
                    IfOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_By = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Locale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplaySequence = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_By = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalDetailsId = table.Column<int>(type: "int", nullable: false),
                    EducationalQualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationOtherSpecify = table.Column<bool>(type: "bit", nullable: true),
                    StudySchool = table.Column<bool>(type: "bit", nullable: true),
                    StudySchoolIfOther = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceivedTrainingSkills = table.Column<int>(type: "int", nullable: true),
                    Skill = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingYears = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhereSkill = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SkillToAcquire = table.Column<bool>(type: "bit", nullable: true),
                    InformalEducation = table.Column<int>(type: "int", nullable: true),
                    InformalIfYesSpecify = table.Column<bool>(type: "bit", nullable: true),
                    InformalIfOthersSpecify = table.Column<bool>(type: "bit", nullable: true),
                    PresentlyStudying = table.Column<int>(type: "int", nullable: true),
                    WhatClass = table.Column<bool>(type: "bit", nullable: true),
                    SupportForEducation = table.Column<bool>(type: "bit", nullable: true),
                    HouseDependent = table.Column<int>(type: "int", nullable: true),
                    StudyingPeople = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportNeeded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikeToContinueEducation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhichClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpeakLanguages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpeakOthersSpecify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WriteLanguages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WriteOthersSpecify = table.Column<bool>(type: "bit", nullable: true),
                    ReadLanguages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadOthersSpecify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscriminatedSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushedOutDueToIdentity = table.Column<bool>(type: "bit", nullable: true),
                    SubjectedVoilence = table.Column<bool>(type: "bit", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_By = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalDetailsId = table.Column<int>(type: "int", nullable: false),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentStatusIfothers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatureOfEmploymentOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wheredoyouwork = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Income = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OthersSpecify = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimarySourceofIncome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondarySourceWork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondarySourceofIncome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnualIncome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HavePositionofOrganization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PositionofOrganization = table.Column<bool>(type: "bit", nullable: true),
                    EmploymentCardMembership = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmploymentCardMembershipOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredEmploymentAgency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisteredEmploymentAgencyWhich = table.Column<bool>(type: "bit", nullable: false),
                    MoneySavingInvestments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Loans = table.Column<bool>(type: "bit", nullable: true),
                    ExistingLoans = table.Column<bool>(type: "bit", nullable: true),
                    Accounts = table.Column<bool>(type: "bit", nullable: true),
                    AccountsOthers = table.Column<int>(type: "int", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_By = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Health",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalDetailsId = table.Column<int>(type: "int", nullable: false),
                    CommonHealthIssues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommonHealthIssuesOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SufferingHealthIssues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SufferingHealthIssuesOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hospital = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreatmentIssue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffirmationSurgery = table.Column<bool>(type: "bit", nullable: true),
                    PlaceofSurgery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhichHospitalSurgery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostDetailsOfGas = table.Column<bool>(type: "bit", nullable: true),
                    SuccessfulSurgery = table.Column<bool>(type: "bit", nullable: true),
                    SurgeryReDone = table.Column<bool>(type: "bit", nullable: true),
                    ReaminingSurgery = table.Column<bool>(type: "bit", nullable: true),
                    HormoneReplaceTherapy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HormoneReplacementTherapy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostPerMonth = table.Column<bool>(type: "bit", nullable: true),
                    SideEffectOfHRT = table.Column<bool>(type: "bit", nullable: true),
                    SideEffectOfHRTOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConversionTerapy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthlyMedicalExpenses = table.Column<bool>(type: "bit", nullable: true),
                    AnyDisability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IfYesWhatKind = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherDisability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhysicalDisability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MentalHealthDiagnosis = table.Column<bool>(type: "bit", nullable: true),
                    MentalHealthSupport = table.Column<bool>(type: "bit", nullable: true),
                    SensitiveMentalHealth = table.Column<bool>(type: "bit", nullable: true),
                    MedicalEmegencyInYear = table.Column<bool>(type: "bit", nullable: true),
                    Affordable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostPrice = table.Column<bool>(type: "bit", nullable: true),
                    MonetarySupport = table.Column<bool>(type: "bit", nullable: true),
                    MonetarySupportPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NeedsMedicalCare = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MedicalCostForMonth = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_By = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Health", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Housing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalDetailsId = table.Column<int>(type: "int", nullable: false),
                    TypeOfHouse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfHouseIfothers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfRoof = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfRoofIfothers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseOwnership = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseOwnershipIfothers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiveInCommunityHome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiveInCommunityHomeIfothers = table.Column<bool>(type: "bit", nullable: true),
                    Toilet = table.Column<bool>(type: "bit", nullable: true),
                    ToiletIfothers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Elecrticity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ElecrticityOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaterFecility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaterFecilityOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ownland = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfLand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Agliculturallandtype = table.Column<bool>(type: "bit", nullable: true),
                    AgliculturallandPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAgliculturalland = table.Column<bool>(type: "bit", nullable: true),
                    commercial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiveStock = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiveStockType = table.Column<bool>(type: "bit", nullable: true),
                    AreaOfResidence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaOfResidenceOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnPropertyFromParents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChosenGender = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShareOfProperty = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShelterSpaceRequired = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShelterSpaceOthers = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HousingSchemes = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WhichScheme = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NeedOldAgeHomes = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssetsYouHave = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssetsYouHaveOthers = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_By = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Housing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransgenderCommunity = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChosenGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntersexVariation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceofBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentTaluk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentHobli = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentVillage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentPinCode = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<int>(type: "int", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FamilyAccepted = table.Column<bool>(type: "bit", nullable: false),
                    LiveWithParents = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LiveSingle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Married = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dependents = table.Column<bool>(type: "bit", nullable: true),
                    DependentOnSomeone = table.Column<bool>(type: "bit", nullable: true),
                    PeopleLiveInHouse = table.Column<bool>(type: "bit", nullable: true),
                    WorkingInHouse = table.Column<bool>(type: "bit", nullable: true),
                    LiveWithCommunity = table.Column<bool>(type: "bit", nullable: true),
                    Caste = table.Column<bool>(type: "bit", nullable: true),
                    Religion = table.Column<bool>(type: "bit", nullable: true),
                    MotherTongue = table.Column<int>(type: "int", nullable: true),
                    Migration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MigrationYears = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttireWear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClothesSenseGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_By = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServeyType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_By = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServeyType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialSecurity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalDetailsId = table.Column<int>(type: "int", nullable: false),
                    GetPension = table.Column<bool>(type: "bit", nullable: true),
                    PensionScheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherPensionScheme = table.Column<bool>(type: "bit", nullable: true),
                    GetRation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RationScheme = table.Column<bool>(type: "bit", nullable: true),
                    OtherRationScheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessSupport = table.Column<bool>(type: "bit", nullable: true),
                    BusinessSupportScheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationSupport = table.Column<bool>(type: "bit", nullable: true),
                    EducationSupportScheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CasteCertificate = table.Column<bool>(type: "bit", nullable: true),
                    IncomeCertificate = table.Column<bool>(type: "bit", nullable: true),
                    InsuranceCertificate = table.Column<bool>(type: "bit", nullable: true),
                    InsuranceTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceTypesOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentifyDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentifyDocumentOthers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardType = table.Column<bool>(type: "bit", nullable: true),
                    CardTypeOthers = table.Column<bool>(type: "bit", nullable: true),
                    ProtectionAct = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeGenderNotarizedAffidavit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailGovernmentSchemeName = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AvailGovernmentSchemeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created_By = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialSecurity", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "OTPValidity", "SecurityStamp" },
                values: new object[] { "557fd91a-caac-4aca-a355-2eed21f85624", new DateTime(2022, 10, 21, 16, 15, 40, 998, DateTimeKind.Local).AddTicks(3700), "965a4d00-bda0-4f09-8fa7-a3b1c747c696" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "OTPValidity", "SecurityStamp" },
                values: new object[] { "8c8fb948-bcdd-4bae-a4fc-20aeeec85753", new DateTime(2022, 10, 21, 16, 15, 40, 998, DateTimeKind.Local).AddTicks(3724), "08c70c0d-33ad-4537-b608-2da4c7c35492" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalInformation");

            migrationBuilder.DropTable(
                name: "CodeTable");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "Employment");

            migrationBuilder.DropTable(
                name: "Health");

            migrationBuilder.DropTable(
                name: "Housing");

            migrationBuilder.DropTable(
                name: "PersonalDetails");

            migrationBuilder.DropTable(
                name: "ServeyType");

            migrationBuilder.DropTable(
                name: "SocialSecurity");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "OTPValidity", "SecurityStamp" },
                values: new object[] { "73458758-d87d-40c9-ae24-536e0a40fdd9", new DateTime(2022, 10, 21, 12, 56, 8, 17, DateTimeKind.Local).AddTicks(177), "e6cd41c8-5a57-43ac-9c2c-0f77c7481d0f" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "OTPValidity", "SecurityStamp" },
                values: new object[] { "9c757b2b-36f6-4bd5-9324-c4cd7b5309b0", new DateTime(2022, 10, 21, 12, 56, 8, 17, DateTimeKind.Local).AddTicks(202), "6cbc9317-a99b-4d42-9ea9-63567ae2525a" });
        }
    }
}
