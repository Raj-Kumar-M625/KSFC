namespace KAR.KSFC.UI.Helpers
{
    public static class LogAttribute
    {
        # region LD Logging Attributes
        public const string PrimarySecurityDto = " Id:{0}, SecurityHolder:{1}, SecurityCategory :{2}, SecurityType :{3}, SecurityDescription :{4}, SecurityDeedNo. :{5}, DeedDescription :{6}, SubRegistrarOffice :{7}, DateOfExecution :{8}, Value : {9} ";
        public const string ColletralSecurityDto = " Id:{0}, SecurityHolder:{1}, SecurityCategory :{2}, SecurityType :{3}, SecurityDescription :{4}, SecurityDeedNo. :{5}, DeedDescription :{6}, SubRegistrarOffice :{7}, DateOfExecution :{8}, Value : {9} ";
        public const string CondtionDto = " Id:{0}, ConditionType:{1}, ConditionDescrpition:{2}, ConditionStage:{3} ";
        public const string HypothecationDto = " Id:{0}, DeedNo:{1}, DeedDesc :{2}, Date :{3}";
        public const string GuarantorDeedDto = " Id:{0}, DeedNo:{1}, DeedDesc :{2}, Date :{3}";
        public const string SecurityChargeDto = "Id:{0}, ChargeTypeCd:{1}, RequestLtrNo :{2}," +
                 " RequestLtrDate :{3}, BankIfscCd :{4}, BankRequestLtrNo. :{5}, BankRequestLtrDate :{6}, NocIssueBy :{7}, NocIssueTo :{8}, " +
                 "NocDate : {9} ,ChargeDetails : {10} ,ChargeConditions : {11} ,AuthLetterBy : {12} ,BoardResolutionDate : {13} ," +
                 "MoeInsuredDate : {14} ,ChargePurpose : {15}";
        public const string CersaiDto = " Id:{0},CersaiRegistrationNo :{1}, CersaiRegDate :{2}, CersaiRemarks :{3} ";
        #endregion

        #region AC Logging Attributes
        public const string AuditClearanceDto = " Id:{0}, AuditObservation:{1},  AuditCond/Recomd :{2}";
        #endregion

        #region Logging Attribute For First Investment Clause
        public const string IdmFirstInvestmentClauseDTO = "DCFICId:{0}, DCFICOffc:{1}, DCFICLoanACC:{2}, DCFICSno:{3},DCFICRequestDate:{4}, DCFICApproveDate:{5}, DCFICApproveAUDate: {6}, DCFICAmount: {7}, DCFICCommunicationDate: {8}, DCFICAmountOriginal: {9}";
        #endregion

        #region UD Logging Attributes
        public const string IdmPromAddressDto = " Id:{0}, NameofPromotor:{1}, Address:{2},State:{3},District:{4},Pincode:{5},WhetherPermanentAddress{6}";
        public const string IdmPromoterDTO = " Id:{0}, NameofPromotor:{1}, PAN: {2}";
        public const string IdmUnitAddressDTO = "Id:{0},Address:{1},District:{2},Taluk:{3},Hobli:{4},Village:{5},Pincode:{6}, City{7}";
        #endregion


        #region ProductDetails Logging Attributes
        public const string productDto = " Id:{0}, UnitCode:{1}, ProductCode:{2} ";
        #endregion

        #region Logging Attribute for Receipt Payment
        public const string ReceiptPaymentDto = "/*Id:{0}, */BranchCode:{1}, IFSCOfIssuingBank:{2}, ChequeDate:{3}, DateOfChequeRealization:{4}, ChequeNo:{5} ";
        #endregion

        #region Loan Related Receipts Logging Attributes
        public const string LoanRelatedReceiptsDto = "Receipt Reference Number{0}";
        #endregion

        #region LoanAllocataion Logging Attributes
        public const string LoanAllocataionDto = " AllocationCode:{0},  AllocationDetails :{1},AllocationRequestDate :{2},AllocationAmount :{3}";
        #endregion

        #region Furniture Acquisition
            public const string FurnitureDto = " Id:{0}, Item NO:{1} ";
        #endregion

        #region Disbursment Proposal Details
        public const string ProposalDetailsDto = " ReleAmount:{0}, LoanAcc:{1}, OffcCd:{2} ";
        #endregion

        #region LoanAllocataion Logging Attributes
        public const string IdmOthdebitsDetailsDTO = " LoanAcc:{0},  LoanSub :{1}";
        #endregion
    }
}
