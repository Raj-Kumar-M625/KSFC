namespace KAR.KSFC.API.Helpers
{
    public static class LogAttribute
    {
        #region Common Logging Message
        public const string UpdateStarted = "Started- Update HttpPost method for ";
        public const string UpdateCompleted = "Completed- Update HttpPost method for ";

        public const string DeleteStarted = "Started- Delete HttpPost method for ";
        public const string DeleteCompleted = "Completed- Delete HttpPost method for ";

        public const string CreateStarted = "Started- Create HttpPost method for ";
        public const string CreateCompleted = "Completed- Create HttpPost method for ";

        public const string SubmitStarted = "Started- Update HttpPost method for ";
        public const string SubmitCompleted = "Completed- Update HttpPost method for ";
        #endregion

        #region Logging Attribute For Cersai Module
        public const string CersaiDTO = "Id:{0},CersaiRegistrationNo :{1}, CersaiRegDate :{2}, CersaiRemarks :{3}";
        #endregion

        #region Logging Attribute For Hypothecation Module
        public const string HypoDTO = "IdmHypothDetId:{0}, LoanAcc:{1}, LoanSub :{2}, OffcCd :{3}, AssetCd :{4}, HypothNo. :{5}, HypothDesc :{6}, AssetValue :{7}, ExecutionDate :{8}, " +
                  "HypothUpload : {9} ,ApprovedEmpId : {10} ,Action : {11}";
        #endregion

        #region Logging Attribute For SecurityDetails Module
        public const string SecurityDTO = "Id:{0}, SecurityHolder:{1}, SecurityCategory :{2}, SecurityType :{3}, SecurityDescription :{4}, SecurityDeedNo. :{5}, DeedDescription :{6}, SubRegistrarOffice :{7}, DateOfExecution :{8}, Value : {9}";
        #endregion

        #region Logging Attribute For SecurityChargeDetails Module
        public const string SecurityChargeDTO = "Id:{0}, ChargeTypeCd:{1}, RequestLtrNo :{2}, RequestLtrDate :{3}, BankIfscCd :{4}, BankRequestLtrNo. :{5}, BankRequestLtrDate :{6}, NocIssueBy :{7}, NocIssueTo :{8}, " +
                  "NocDate : {9} ,ChargeDetails : {10} ,ChargeConditions : {11} ,AuthLetterBy : {12} ,BoardResolutionDate : {13} ,MoeInsuredDate : {14} ,ChargePurpose : {15}";
        #endregion

        #region Logging Attribute For GuarantorDeed Module
        public const string GuarantorDeedDTO = "IdmGuarDeedId :{0}, AppAssetDesc:{1}, AppAssetvalue  :{2}, DeedDesc :{3}, ExcecutionDate:{4}, DeedNo: {5},AppLiabValue: {6}, AppNw: {7}";
        #endregion

        #region Logging Attribute For Condition Module
        public const string ConditionDTO = "Id:{0}, ConditionDescrpition:{1}, ConditionStage:{2}";
        #endregion
        public const string Form8AndForm13DTO = "Id:{0}, TypeofForm:{1}, DateofFilling:{2}, ReceiptNo:{3}";
        #region Logging Attribute For Sidbi Module
        public const string SidbiDTO = "Id:{0}, SanctionedLoanAmount:{1}, TypeOfPromoter{2}";
        #endregion

        #region AC Logging Attributes
        public const string AuditClearanceDto = " Id:{0}, AuditObservation:{1},  AuditCond/Recomd :{2}";
        #endregion

        #region Logging Attribute For First Investment Clause
        public const string IdmFirstInvestmentClauseDTO = "DCFICId:{0}, DCFICOffc:{1}, DCFICLoanACC:{2}, DCFICSno:{3},DCFICRequestDate:{4}, DCFICApproveDate:{5}, DCFICApproveAUDate: {6}, DCFICAmount: {7}, DCFICCommunicationDate: {8}, DCFICAmountOriginal: {9}";
        #endregion

        #region UD Logging Attributes
        public const string IdmPromAddressDto = " Id:{0}, NameofPromotor:{1}, Address:{2},State:{3},District:{4},Pincode:{5},WhetherPermanentAddress{6}";
        public const string IdmPromoterDTO = " Id:{0}, NameofPromotor:{1}, PromoterName:{2}";
        public const string IdmUnitAddressDTO = "Id:{0},Address:{1},District:{2},Taluk:{3},Hobli:{4},Village:{5},Pincode:{6}, City{7}";
        public const string TblIdmProjLandDTO = "Id:{0}";
        #endregion

        #region Logging Attribute For Product Details Model
        public const string ProductDTO = " Id:{0}, UnitCode:{1}, ProductCode:{2} ";
       
        #endregion
        #region Logging Attributte for Land Inspection
        public const string landInspectionDTO = "Id:{0}, RevisedLandArea:{1}, SecurityCreated:{2}, TypeOfLand:{3}, StatusOnChangedDate:{4}, CostOfLand{5}";
        #endregion
        #region Logging Attributte for Building Material Inspection
        public const string bildMatInspectionDTO = "Id:{0}, ItemNo:{1}, MaetrialDescription:{2}, Rate:{3}, Quantity:{4}";
        #endregion

        #region Logging Attribute for Machinery Acquisition
        public const string MachineDTO = "Id:{0}, MemValue:{1}, TotalRelease:{2}, SecurityConsideredForRelease:{3}, ReleaseStatus:{4}, AcquiredStatus:{5}";
        #endregion

        #region Logging Attribute for Land Acquisition
        public const string LandDTO = "Id:{0}, ActualLandUnit:{1}, SecurityConsideredForRelease:{2}, ReleaseStatus:{3}";
        #endregion
        #region Furniture Acquisition
        public const string FurnitureDto = " Id:{0}, Item NO:{1} ";
        #endregion
        #region LoanAllocataion Logging Attributes
        public const string LoanAllocataionDto = " AllocationCode:{0},  AllocationDetails :{1},AllocationRequestDate :{2},AllocationAmount :{3}";
        #endregion

        #region Logging Attribute for Building Acquisition
        public const string BuildingDTO = "Id:{0}, LoanAccNo:{1}";
        #endregion

        #region Loan Related Receipts Logging Attributes
        public const string LoanRelatedReceiptsDto = "Receipt Reference Number{0}";
        #endregion

        #region Logging Attribute For Disbursement Details
        public const string RecomDisbursementDTO = "Id:{0}, LoanAccNo:{1}";
        #endregion

        #region LoanAllocataion Logging Attributes
        public const string IdmOthdebitsDetailsDTO = " LoanAcc:{0},  LoanSub :{1}";
        #endregion
    }
}
