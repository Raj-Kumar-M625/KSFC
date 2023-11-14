
$("#btnTabSecurityDetails").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/LegalDocumentation/SavePrimaryCollateralDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#SecurityProgressBar").attr('class', 'progress-bar bg-success');
                $("#Colet").attr('class', 'tab-pane fade active show');
                $("#Security").attr('class', 'tab-pane fade');

                $("#Colet-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Security-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#SecurityProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#SecurityProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#savebtncollTab").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/LegalDocumentation/SavePrimaryCollateralDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#ColetProgressBar").attr('class', 'progress-bar bg-success');
                $("#Guarantor").attr('class', 'tab-pane fade active show');
                $("#Colet").attr('class', 'tab-pane fade');

                $("#Guarantor-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Colet-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#ColetProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#ColetProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});






$("#btnSubmitSecurityChrg").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/LegalDocumentation/SaveSecurityChargeForLD'),
        success: function (data) {
            if (data.isValid) {
                $("#ChargeProgressBar").attr('class', 'progress-bar bg-success');
                $("#CERSAI").attr('class', 'tab-pane fade active show');
                $("#Charge").attr('class', 'tab-pane fade');

                $("#CERSAI-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Charge-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#ChargeProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $().attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });

});


$("#not-applicable").click(function (event) {
    event.preventDefault();
    $("#ChargeProgressBar").attr('class', 'progress-bar bg-success');
    $("#CERSAI").attr('class', 'tab-pane fade active show');
    $("#Charge").attr('class', 'tab-pane fade');

    $("#CERSAI-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Charge-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#previousbtncollTab").click(function (event) {

    $("#Security").attr('class', 'tab-pane fade active show');
    $("#Colet").attr('class', 'tab-pane fade');

    $("#Security-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Colet-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});




$("#previousbtnGuarTab").click(function (event) {

    $("#Colet").attr('class', 'tab-pane fade active show');
    $("#Guarantor").attr('class', 'tab-pane fade');

    $("#Colet-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Guarantor-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#previousbtnChargeTab").click(function (event) {

    $("#Hypthecation").attr('class', 'tab-pane fade active show');
    $("#Charge").attr('class', 'tab-pane fade');

    $("#Hypthecation-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Charge-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});


$("#previousbtnAddCondTab").click(function (event) {

    $("#DisbursementCondition").attr('class', 'tab-pane fade active show');
    $("#AdditionalCondition").attr('class', 'tab-pane fade');

    $("#DisbursementCondition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#AdditionalCondition-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});


$("#previousbtnHypoTab").click(function (event) {

    $("#Guarantor").attr('class', 'tab-pane fade active show');
    $("#Hypthecation").attr('class', 'tab-pane fade');

    $("#Guarantor-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Hypthecation-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});


$("#previousbtnCersaiTab").click(function (event) {
    $("#Charge").attr('class', 'tab-pane fade active show');
    $("#CERSAI").attr('class', 'tab-pane fade');

    $("#Charge-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#CERSAI-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#savebtnCersaiTab").click(function (event) {
    event.preventDefault();
    let dataString = $("LDCersaiRegDetails").serialize();
    //debugger;
    $.ajax({
        async: false,
        type: "POST",
        datatype: 'json',
        url: GetRoute('/LegalDocumentation/SaveCersaiRegistrationForLD'),
        data: dataString,
        success: function (data) {

            if (data.isValid) {
                $("#CERSAIProgressBar").attr('class', 'progress-bar bg-success');
                $("#Conditions").attr('class', 'tab-pane fade active show');
                $("#CERSAI").attr('class', 'tab-pane fade');

                $("#Conditions-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#CERSAI-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#CERSAIProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#CERSAIProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });

});
function showLoader() {
    $('#divLoading').removeAttr('hidden');
    $('#overlay').removeAttr('hidden');
}
function showInPopup(url, title, module) {

    $('#divLoading').removeAttr('hidden'); // show the loader
    $('#overlay').removeAttr('hidden');
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            debugger
            switch (module) {
                case 'Condition':
                    $('#modelConditionDetails .modal-body').html(res);
                    $('#modelConditionDetails .modal-title').html(title);
                    $('#modelConditionDetails').modal('show');
                    break
                case 'SecurityCharge':
                    $('#modelSecurityChargeDetails .modal-body').html(res);
                    $('#modelSecurityChargeDetails .modal-title').html(title);
                    $('#modelSecurityChargeDetails').modal('show');
                    break
                case 'GuarantorDeed':
                    $("#divLoading").hide(); // hide the loader on success
                    $('#modelGuarantorDetails .modal-body').html(res);
                    $('#modelGuarantorDetails .modal-title').html(title);
                    $('#modelGuarantorDetails').modal('show');
                    break
                case 'Cersai':
                    $('#modelCERSAIDetails .modal-body').html(res);
                    $('#modelCERSAIDetails .modal-title').html(title);
                    $('#modelCERSAIDetails').modal('show');
                    break
                case 'PrimarySecurity':

                    $('#primarySecurityDetails .modal-body').html(res);
                    $('#primarySecurityDetails .modal-title').html(title);
                    $('#primarySecurityDetails').modal('show');
                    break
                case 'ColletralSecurity':

                    $('#colletralSecurityDetails .modal-body').html(res);
                    $('#colletralSecurityDetails .modal-title').html(title);
                    $('#colletralSecurityDetails').modal('show');
                    break
                case 'Hypothecation':

                    $('#modelHypothecationDetails .modal-body').html(res);
                    $('#modelHypothecationDetails .modal-title').html(title);
                    $('#modelHypothecationDetails').modal('show');
                    break
                case 'AuditClearance':
                    $('#modelAuditClearanceDetails .modal-body').html(res);
                    $('#modelAuditClearanceDetails .modal-title').html(title);
                    $('#modelAuditClearanceDetails').modal('show');
                    break
                case 'Disbursement Condition':
                    $('#modelDisbursementConditionDetails .modal-body').html(res);
                    $('#modelDisbursementConditionDetails .modal-title').html(title);
                    $('#modelDisbursementConditionDetails').modal('show');
                    break
                case 'Form8AndForm13':
                    $('#modelForm8AndForm13Details .modal-body').html(res);
                    $('#modelForm8AndForm13Details .modal-title').html(title);
                    $('#modelForm8AndForm13Details').modal('show');
                    break
                case 'Additional Condition':
                    $('#modelAdditionalConditionDetails .modal-body').html(res);
                    $('#modelAdditionalConditionDetails .modal-title').html(title);
                    $('#modelAdditionalConditionDetails').modal('show');
                    break
                case 'ChangeLocationAddress':
                    $('#modelChangeLocation .modal-body').html(res);
                    $('#modelChangeLocation .modal-title').html(title);
                    $('#modelChangeLocation').modal('show');
                    break

                case 'PromoterProfile':
                    $('#modelChangePromoterProfile .modal-body').html(res);
                    $('#modelChangePromoterProfile .modal-title').html(title);
                    $('#modelChangePromoterProfile').modal('show');
                    break
                case 'PromoterAddress':
                    $('#modelChangePromoterAddress .modal-body').html(res);
                    $('#modelChangePromoterAddress .modal-title').html(title);
                    $('#modelChangePromoterAddress').modal('show');
                    break
                case 'PromoterBankInfo':
                    $('#modelChangePromoterBankInfo .modal-body').html(res);
                    $('#modelChangePromoterBankInfo .modal-title').html(title);
                    $('#modelChangePromoterBankInfo').modal('show');
                    break
                case 'Product':
                    $('#modelChangeProductDetails .modal-body').html(res);
                    $('#modelChangeProductDetails .modal-title').html(title);
                    $('#modelChangeProductDetails').modal('show');
                    break
                case 'ChangeBankDetails':
                    $('#modelChangeBankDetails .modal-body').html(res);
                    $('#modelChangeBankDetails .modal-title').html(title);
                    $('#modelChangeBankDetails').modal('show');
                    break
                case 'AssetDetails':
                    $('#modelChangeAssetInformation .modal-body').html(res);
                    $('#modelChangeAssetInformation .modal-title').html(title);
                    $('#modelChangeAssetInformation').modal('show');
                    break
                case 'LiabilityInformation':
                    $('#modelChangeLiabilityInfo .modal-body').html(res);
                    $('#modelChangeLiabilityInfo .modal-title').html(title);
                    $('#modelChangeLiabilityInfo').modal('show');
                    break
                case 'PromoterNetWorth':
                    $('#modelChangeNetWorth .modal-body').html(res);
                    $('#modelChangeNetWorth .modal-title').html(title);
                    $('#modelChangeNetWorth').modal('show');
                    break
                case 'Building Inspection':
                    $('#modelBuildingInspectionDetails .modal-body').html(res);
                    $('#modelBuildingInspectionDetails .modal-title').html(title);
                    $('#modelBuildingInspectionDetails').modal('show');
                    break
                case 'BuildingInspectionAd':
                    $('#modelBuildingInspectionDetailsAd .modal-body').html(res);
                    $('#modelBuildingInspectionDetailsAd .modal-title').html(title);
                    $('#modelBuildingInspectionDetailsAd').modal('show');
                    break
                case 'Import Machinery Inspection':
                    $('#modelImportMachineryDetails .modal-body').html(res);
                    $('#modelImportMachineryDetails .modal-title').html(title);
                    $('#modelImportMachineryDetails').modal('show');
                    break
                case 'Import Machinery InspectionAd':
                    $('#modelImportedMachineryInspectionDetailsAd .modal-body').html(res);
                    $('#modelImportedMachineryInspectionDetailsAd .modal-title').html(title);
                    $('#modelImportedMachineryInspectionDetailsAd').modal('show');
                    break
                case 'StatusofImplementation':
                    $('#modelStatusofImplementationDetails .modal-body').html(res);
                    $('#modelStatusofImplementationDetails .modal-title').html(title);
                    $('#modelStatusofImplementationDetails').modal('show');
                    break
                case 'Means Of Finance':
                    $('#modelMeansOfFinanceDetails .modal-body').html(res);
                    $('#modelMeansOfFinanceDetails .modal-title').html(title);
                    $('#modelMeansOfFinanceDetails').modal('show');
                    break
                case 'LandInspection':
                    $('#modelLandInspectionDetails .modal-body').html(res);
                    $('#modelLandInspectionDetails .modal-title').html(title);
                    $('#modelLandInspectionDetails').modal('show');
                    break
                case 'LandInspectionAd':
                    $('#modelLandInspectionDetailsAd .modal-body').html(res);
                    $('#modelLandInspectionDetailsAd .modal-title').html(title);
                    $('#modelLandInspectionDetailsAd').modal('show');
                    break
                case 'FurnitureInspection':
                    $('#modelFurnitureInspectionDetails .modal-body').html(res);
                    $('#modelFurnitureInspectionDetails .modal-title').html(title);
                    $('#modelFurnitureInspectionDetails').modal('show');
                    break
                case 'FurnitureInspectionAd':
                    $('#modelFurnitureInspectionDetailsAd .modal-body').html(res);
                    $('#modelFurnitureInspectionDetailsAd .modal-title').html(title);
                    $('#modelFurnitureInspectionDetailsAd').modal('show');
                    break

                case 'Inspection Detail':
                    $('#modelInspectionDetail .modal-body').html(res);
                    $('#modelInspectionDetail .modal-title').html(title);
                    $('#modelInspectionDetail').modal('show');
                    break
                case 'Creation Inspection Detail':
                    $('#modelCreationOfSecurityandAquisitionAsset .modal-body').html(res);
                    $('#modelCreationOfSecurityandAquisitionAsset .modal-title').html(title);
                    $('#modelCreationOfSecurityandAquisitionAsset').modal('show');
                    break

                case 'BuildMatSiteInspection':
                    $('#modelBuildMatAtSiteDetails .modal-body').html(res);
                    $('#modelBuildMatAtSiteDetails .modal-title').html(title);
                    $('#modelBuildMatAtSiteDetails').modal('show');
                    break

                case 'IndigenousMachinery':
                    $('#modelIndigeneousMacInspectionDetails .modal-body').html(res);
                    $('#modelIndigeneousMacInspectionDetails .modal-title').html(title);
                    $('#modelIndigeneousMacInspectionDetails').modal('show');
                    break
                case 'IndigenousMachineryAd':
                    $('#modelIndigenousMachineryInspectionDetailsAd .modal-body').html(res);
                    $('#modelIndigenousMachineryInspectionDetailsAd .modal-title').html(title);
                    $('#modelIndigenousMachineryInspectionDetailsAd').modal('show');
                    break
                case 'ProjectCost':
                    debugger
                    $('#modelProjectCostDetail .modal-body').html(res);
                    $('#modelProjectCostDetail .modal-title').html(title);
                    $('#modelProjectCostDetail').modal('show');
                    break

                case 'LetterOfCredit':
                    $('#modelLetterOfCreditDetail .modal-body').html(res);
                    $('#modelLetterOfCreditDetail .modal-title').html(title);
                    $('#modelLetterOfCreditDetail').modal('show');
                    break
                case 'Machinery Acquisition':
                    $('#modelMachineryAcquisitionDetails .modal-body').html(res);
                    $('#modelMachineryAcquisitionDetails .modal-title').html(title);
                    $('#modelMachineryAcquisitionDetails').modal('show');
                    break
                case 'Land Acquisition':
                    $('#modelLandAcquisitionDetails .modal-body').html(res);
                    $('#modelLandAcquisitionDetails .modal-title').html(title);
                    $('#modelLandAcquisitionDetails').modal('show');
                    break
                case 'LoanAllocation':
                    $('#modelLoanAllocationDetails .modal-body').html(res);
                    $('#modelLoanAllocationDetails .modal-title').html(title);
                    $('#modelLoanAllocationDetails').modal('show');
                    break

                case 'FurnitureAcquisition':
                    $('#modelFurnitureAcquisitionDetails .modal-body').html(res);
                    $('#modelFurnitureAcquisitionDetails .modal-title').html(title);
                    $('#modelFurnitureAcquisitionDetails').modal('show');
                    break

                case 'Building Acquisition':
                    $('#modelBuildingAcquisitionDetails .modal-body').html(res);
                    $('#modelBuildingAcquisitionDetails .modal-title').html(title);
                    $('#modelBuildingAcquisitionDetails').modal('show');
                    break
                case 'Recommended Disbursement':
                    $('#modelRecommDisbursementDetails .modal-body').html(res);
                    $('#modelRecommDisbursementDetails .modal-title').html(title);
                    $('#modelRecommDisbursementDetails').modal('show');
                    break
                case 'Disbursment Proposal':
                    $('#modelProposalDetails .modal-body').html(res);
                    $('#modelProposalDetails .modal-title').html(title);
                    $('#modelProposalDetails').modal('show');
                    break
                case 'OtherDebits':
                    $('#modelOtherDebitsDetails .modal-body').html(res);
                    $('#modelOtherDebitsDetails .modal-title').html(title);
                    $('#modelOtherDebitsDetails').modal('show');
                    break
                default:
                    break
            }
            $('#divLoading').attr('hidden', true); // hide the loader on success
            $('#overlay').attr('hidden', true);
        },
        error: function (err) {
            $('#divLoading').attr('hidden', true); // hide the loader on error
            $('#overlay').attr('hidden', true);
            console.log(err);
        }
    });
}

function ClosePopupFormsh() {

    $('#primarySecurityDetails .modal-body').html('');
    $('#primarySecurityDetails .modal-title').html('');
    $('#primarySecurityDetails').modal('hide');


    $('#colletralSecurityDetails .modal-body').html('');
    $('#colletralSecurityDetails .modal-title').html('');
    $('#colletralSecurityDetails').modal('hide');
    
    $('#modelSidbidocumentupload .modal-body').html('');
    $('#modelSidbidocumentupload .modal-title').html('');
    $('#modelSidbidocumentupload').modal('hide');

    $('#modelSecurityChargeDetails .modal-body').html('');
    $('#modelSecurityChargeDetails .modal-title').html('');
    $('#modelSecurityChargeDetails').modal('hide');

    $('#modelHypothecationDetails .modal-body').html('');
    $('#modelHypothecationDetails .modal-title').html('');
    $('#modelHypothecationDetails').modal('hide');

    $('#modelCERSAIDetails .modal-body').html('');
    $('#modelCERSAIDetails .modal-title').html('');
    $('#modelCERSAIDetails').modal('hide');

    $('#modelCreationOfSecurityandAquisitionAsset .modal-body').html('');
    $('#modelCreationOfSecurityandAquisitionAsset .modal-title').html('');
    $('#modelCreationOfSecurityandAquisitionAsset').modal('hide');


    $('#modelGuarantorDetails .modal-body').html('');
    $('#modelGuarantorDetails .modal-title').html('');
    $('#modelGuarantorDetails').modal('hide');

    $('#modelConditionDetails .modal-body').html('');
    $('#modelConditionDetails .modal-title').html('');
    $('#modelConditionDetails').modal('hide');

    $('#modelAdditionalConditionDetails .modal-body').html('');
    $('#modelAdditionalConditionDetails .modal-title').html('');
    $('#modelAdditionalConditionDetails').modal('hide');

    $('#modelDisbursementConditionDetails .modal-body').html('');
    $('#modelDisbursementConditionDetails .modal-title').html('');
    $('#modelDisbursementConditionDetails').modal('hide');

    $('#modelForm8AndForm13Details .modal-body').html('');
    $('#modelForm8AndForm13Details .modal-title').html('');
    $('#modelForm8AndForm13Details').modal('hide');

    $('#modelChangeLocation .modal-body').html('');
    $('#modelChangeLocation .modal-title').html('');
    $('#modelChangeLocation').modal('hide');

    $('#modelChangePromoterProfile .modal-body').html('');
    $('#modelChangePromoterProfile .modal-title').html('');
    $('#modelChangePromoterProfile').modal('hide');

    //Dev 0n 23/08/2022
    $('#modelSidbiApprovalDetails .modal-body').html('');
    $('#modelSidbiApprovalDetails .modal-title').html('');
    $('#modelSidbiApprovalDetails').modal('hide');

    $('#modelChangePromoterAddress .modal-body').html('');
    $('#modelChangePromoterAddress .modal-title').html('');
    $('#modelChangePromoterAddress').modal('hide');

    $('#modelChangePromoterBankInfo .modal-body').html('');
    $('#modelChangePromoterBankInfo .modal-title').html('');
    $('#modelChangePromoterBankInfo').modal('hide');

    $('#modelChangeProductDetails .modal-body').html('');
    $('#modelChangeProductDetails .modal-title').html('');
    $('#modelChangeProductDetails').modal('hide');

    $('#modelChangeBankDetails .modal-body').html('');
    $('#modelChangeBankDetails .modal-title').html('');
    $('#modelChangeBankDetails').modal('hide');

    $('#modelChangeAssetInformation .modal-body').html('');
    $('#modelChangeAssetInformation .modal-title').html('');
    $('#modelChangeAssetInformation').modal('hide');

    $('#modelChangeLiabilityInfo .modal-body').html('');
    $('#modelChangeLiabilityInfo .modal-title').html('');
    $('#modelChangeLiabilityInfo').modal('hide');

    $('#modelChangeNetWorth .modal-body').html('');
    $('#modelChangeNetWorth .modal-title').html('');
    $('#modelChangeNetWorth').modal('hide');



    $('#modelLandInspectionDetails .modal-body').html('');
    $('#modelLandInspectionDetails .modal-title').html('');
    $('#modelLandInspectionDetails').modal('hide');

    $('#modelLandInspectionDetailsAd .modal-body').html('');
    $('#modelLandInspectionDetailsAd .modal-title').html('');
    $('#modelLandInspectionDetailsAd').modal('hide');


    $('#modelFurnitureInspectionDetails .modal-body').html('');
    $('#modelFurnitureInspectionDetails .modal-title').html('');
    $('#modelFurnitureInspectionDetails').modal('hide');

    $('#modelFurnitureInspectionDetailsAd .modal-body').html('');
    $('#modelFurnitureInspectionDetailsAd .modal-title').html('');
    $('#modelFurnitureInspectionDetailsAd').modal('hide');

    $('#modelBuildingInspectionDetails .modal-body').html('');
    $('#modelBuildingInspectionDetails .modal-title').html('');
    $('#modelBuildingInspectionDetails').modal('hide');

    $('#modelBuildingInspectionDetailsAd .modal-body').html('');
    $('#modelBuildingInspectionDetailsAd .modal-title').html('');
    $('#modelBuildingInspectionDetailsAd').modal('hide');

    $('#modelBuildMatSiteInspectionDetails .modal-body').html('');
    $('#modelBuildMatSiteInspectionDetails .modal-title').html('');
    $('#modelBuildMatSiteInspectionDetails').modal('hide');


    $('#modelImportMachineryDetails .modal-body').html('');
    $('#modelImportMachineryDetails .modal-title').html('');
    $('#modelImportMachineryDetails').modal('hide');

    $('#modelImportedMachineryInspectionDetailsAd .modal-body').html('');
    $('#modelImportedMachineryInspectionDetailsAd .modal-title').html('');
    $('#modelImportedMachineryInspectionDetailsAd').modal('hide');

    $('#modelImportMachineryList .modal-body').html('');
    $('#modelImportMachineryList .modal-title').html('');
    $('#ImportMachineryList').modal('hide');

    $('#modelStatusofImplementationDetails .modal-body').html('');
    $('#modelStatusofImplementationDetails .modal-title').html('');
    $('#modelStatusofImplementationDetails').modal('hide');

    $('#modelStatusofImplementationList .modal-body').html('');
    $('#modelStatusofImplementationList .modal-title').html('');
    $('#StatusofImplementationList').modal('hide');



    $('#modelBuildMatAtSiteDetails .modal-body').html('');
    $('#modelBuildMatAtSiteDetails .modal-title').html('');
    $('#modelBuildMatAtSiteDetails').modal('hide');

    $('#modelMeansOfFinanceDetails .modal-body').html('');
    $('#modelMeansOfFinanceDetails .modal-title').html('');
    $('#modelMeansOfFinanceDetails').modal('hide');

    $('#modelIndigeneousMacInspectionDetails .modal-body').html('');
    $('#modelIndigeneousMacInspectionDetails .modal-title').html('');
    $('#modelIndigeneousMacInspectionDetails').modal('hide');

    $('#modelIndigeneousMacInspectionDetailsAd .modal-body').html('');
    $('#modelIndigeneousMacInspectionDetailsAd .modal-title').html('');
    $('#modelIndigeneousMacInspectionDetailsAd').modal('hide');


    $('#modelIndigenousMachineryInspectionDetails .modal-body').html('');
    $('#modelIndigenousMachineryInspectionDetails .modal-title').html('');
    $('#modelIndigenousMachineryInspectionDetails').modal('hide');

    $('#modelIndigenousMachineryInspectionDetailsAd .modal-body').html('');
    $('#modelIndigenousMachineryInspectionDetailsAd .modal-title').html('');
    $('#modelIndigenousMachineryInspectionDetailsAd').modal('hide');


    $('#modelLetterOfCreditDetail .modal-body').html('');
    $('#modelLetterOfCreditDetail .modal-title').html('');
    $('#modelLetterOfCreditDetail').modal('hide');

    $('#modelProjectCostDetail .modal-body').html('');
    $('#modelProjectCostDetail .modal-title').html('');
    $('#modelProjectCostDetail').modal('hide');


    $('#modelInspectionDetail .modal-body').html('');
    $('#modelInspectionDetail .modal-title').html('');
    $('#modelInspectionDetail').modal('hide');


    $('#modelFurnitureInspectionDetails .modal-body').html('');
    $('#modelFurnitureInspectionDetails .modal-title').html('');
    $('#modelFurnitureInspectionDetails').modal('hide');

    $('#modelMachineryAcquisitionDetails .modal-body').html('');
    $('#modelMachineryAcquisitionDetails .modal-title').html('');
    $('#modelMachineryAcquisitionDetails').modal('hide');

    $('#modelLandAcquisitionDetails .modal-body').html('');
    $('#modelLandAcquisitionDetails .modal-title').html('');
    $('#modelLandAcquisitionDetails').modal('hide');

    $('#modelChargeDetails .modal-body').html('');
    $('#modelChargeDetails .modal-title').html('');
    $('#modelChargeDetails').modal('hide');

    $('#modelLoanAllocationDetails .modal-body').html('');
    $('#modelLoanAllocationDetails .modal-title').html('');
    $('#modelLoanAllocationDetails').modal('hide');

    $('#modelBuildingAcquisitionDetails .modal-body').html('');
    $('#modelBuildingAcquisitionDetails .modal-title').html('');
    $('#modelBuildingAcquisitionDetails').modal('hide');

    $('#modelFurnitureAcquisitionDetails .modal-body').html('');
    $('#modelFurnitureAcquisitionDetails .modal-title').html('');
    $('#modelFurnitureAcquisitionDetails').modal('hide');

    $('#modelForm8AndForm13Details .modal-body').html('');
    $('#modelForm8AndForm13Details .modal-title').html('');
    $('#modelForm8AndForm13Details').modal('hide');

    $('#modelRecommDisbursementDetails .modal-body').html('');
    $('#modelRecommDisbursementDetails .modal-title').html('');
    $('#modelRecommDisbursementDetails').modal('hide');

    $('#modelProposalDetails .modal-body').html('');
    $('#modelProposalDetails .modal-title').html('');
    $('#modelProposalDetails').modal('hide');

    $('#modelOtherDebitsDetails .modal-body').html('');
    $('#modelOtherDebitsDetails .modal-title').html('');
    $('#modelOtherDebitsDetails').modal('hide');

    $('#modelPromProfile .modal-body').html('');
    $('#modelPromProfile.modal-title').html('');
    $('#modelPromProfile').modal('hide');



}

function CloseDocPopupFormsh() {
    $('#viewDocuments .modal-body').html('');
    $('#viewDocuments .modal-title').html('');
    $('#viewDocuments').modal('hide');


    $('#viewDisburmentDocuments .modal-body').html('');
    $('#viewDisburmentDocuments .modal-title').html('');
    $('#viewDisburmentDocuments').modal('hide');
}

//Akhiladevi D M 10-08-2022
$("#savebtnGuarTab").click(function (event) {

    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/LegalDocumentation/SaveGuarrantorDeedDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#GuarantorProgressBar").attr('class', 'progress-bar bg-success');
                $("#Hypthecation").attr('class', 'tab-pane fade active show');
                $("#Guarantor").attr('class', 'tab-pane fade');

                $("#Hypthecation-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Guarantor-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#GuarantorProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#GuarantorProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});


//MJ 11-08-2022
$("#btnSubmitHypothecation").click(function (event) {


    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        datatype: 'json',
        url: GetRoute('/LegalDocumentation/SaveHypothecationForLD'),
        success: function (data) {
            if (data.isValid) {
                $("#HypthecationProgressBar").attr('class', 'progress-bar bg-success');
                $("#Charge").attr('class', 'tab-pane fade active show');
                $("#Hypthecation").attr('class', 'tab-pane fade');

                $("#Charge-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Hypthecation-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#HypthecationProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }

        },
        error: function (err) {
            $("#HypthecationProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });

});

//Author: Dev; Module: Condition; Update Date: 05/08/2022
$("#previousbtnCondTab").click(function (event) {

    $("#CERSAI").attr('class', 'tab-pane fade active show');
    $("#Conditions").attr('class', 'tab-pane fade');

    $("#CERSAI-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Conditions-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

//Author: Gagana; Module: Condition; Date: 12/08/2022 

$("#savebtnCondTab").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/LegalDocumentation/SaveConditionsForLD'),
        success: function (data) {

            if (data.isValid) {

                $("#ConditionsProgressBar").attr('class', 'progress-bar bg-success');
                $("#CheckList").attr('class', 'tab-pane fade active show');
                $("#Conditions").attr('class', 'tab-pane fade')

                $("#CheckList-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Conditions-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#ConditionsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }

        },
        error: function (err) {
            $("#ConditionsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });

});

//Author: Gagana; Module: CheckList; Update Date: 05/08/2022
$("#previousbtnChecklistTab").click(function (event) {

    $("#Conditions").attr('class', 'tab-pane fade active show');
    $("#CheckList").attr('class', 'tab-pane fade');

    $("#Conditions-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#CheckList-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnTabDisbursementConditionDetails").click(function (event) {
    event.preventDefault();

    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/Disbursement/SaveDisbursementConditionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#DisbursementConditionProgressBar").attr('class', 'progress-bar bg-success');
                $("#AdditionalCondition").attr('class', 'tab-pane fade active show');
                $("#DisbursementCondition").attr('class', 'tab-pane fade');

                $("#AdditionalCondition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#DisbursementCondition-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#DisbursementConditionProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#DisbursementConditionProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#btnTabAdditionalConditionDetails").click(function (event) {
    event.preventDefault();

    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/Disbursement/SaveAddtionalConditionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#AdditionalConditionProgressBar").attr('class', 'progress-bar bg-success');
                $("#OtherRelaxation").attr('class', 'tab-pane fade active show');
                $("#AdditionalCondition").attr('class', 'tab-pane fade');

                $("#OtherRelaxation-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#AdditionalCondition-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#AdditionalConditionProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#AdditionalConditionProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

//gs 
$("#btnTabForm8AndForm13Details").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/Disbursement/SaveForm8AndForm13Details'),
        success: function (data) {
            debugger
            if (data.isValid) {
                $("#Form8AndForm13ProgressBar").attr('class', 'progress-bar bg-success');
                $("#SidbiApproval").attr('class', 'tab-pane fade active show');
                $("#Form8AndForm13").attr('class', 'tab-pane fade');

                $("#SidbiApproval-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Form8AndForm13-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#Form8AndForm13ProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {


            $("#Form8AndForm13ProgressBar").attr('class', 'progress-bar bg-danger');

            console.log(err);
        }
    });
});


$("#previousbtnForm8AndForm13Tab").click(function (event) {

    $("#FirstInvestmentClause").attr('class', 'tab-pane fade active show');
    $("#Form8AndForm13").attr('class', 'tab-pane fade');

    $("#FirstInvestmentClause-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Form8AndForm13-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

function ClosePopupFormac() {
    $('#modelAuditClearanceDetails .modal-body').html('');
    $('#modelAuditClearanceDetails .modal-title').html('');
    $('#modelAuditClearanceDetails').modal('hide');

    $('#modelEditSaveReceiptDetails .modal-body').html('');
    $('#modelEditSaveReceiptDetails .modal-title').html('');
    $('#modelEditSaveReceiptDetails').modal('hide');
}

//Akhila Module:FIC  23-08-2022
$("#previousbtnFICTab").click(function (event) {

    $("#AdditionalCondition").attr('class', 'tab-pane fade active show');
    $("#FirstInvestmentClause").attr('class', 'tab-pane fade');

    $("#AdditionalCondition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#FirstInvestmentClause-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

function showRelaxationPopup(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#viewOtherRelaxation .modal-body').html(res.html);
            $('#viewOtherRelaxation .modal-title').html(title);
            $('#viewOtherRelaxation').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function CloseRelaxPopupFormsh() {
    $('#viewOtherRelaxation .modal-body').html('');
    $('#viewOtherRelaxation .modal-title').html('');
    $('#viewOtherRelaxation').modal('hide');
}

// Dev
//$("#btnprevioussidbi").click(function (event) {

//    $("#Form8AndForm13").attr('class', 'tab-pane fade active show');
//    $("#SidbiApproval").attr('class', 'tab-pane fade');

//    $("#Form8AndForm13-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
//    $("#SidbiApproval-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

//    $("#spanErrorMsg").html("");
//    $("#divEnquiryAlertPopup").html("");
//    window.scrollTo(0, 0);

//});


$("#previousbtnOthrRelxTab").click(function (event) {

    $("#AdditionalCondition").attr('class', 'tab-pane fade active show');
    $("#OtherRelaxation").attr('class', 'tab-pane fade');

    $("#AdditionalCondition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#OtherRelaxation-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnTabSidbi").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/Disbursement/SaveSidbiApprovalDetails'),
        success: function (data) {
            debugger
            if (data.isValid) {
                $("#SidbiProgressBar").attr('class', 'progress-bar bg-success');
                $("#DisbursementCondition").attr('class', 'tab-pane fade active show');
                $("#SidbiApproval").attr('class', 'tab-pane fade');

                $("#DisbursementCondition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Sidbi-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#SidbiProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            console.log(err);
        }
    });

});

$("#btnTabAuditDetails").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/Audit/SaveAuditClearanceDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#AuditClearanceProgressBar").attr('class', 'progress-bar bg-success');
                $("#AuditClearance").attr('class', 'tab-pane fade active show');

                $("#AuditClearance-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#AuditClearanceProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#AuditClearanceProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

//Module: ChangePromoterAddress -> Added by Gagana

$("#btnSavePromoterAddress").click(function (event) {

    event.preventDefault();
    let dataString = $("UDPromotorAddress").serialize();
    $.ajax({
        async: false,
        type: "POST",
        datatype: 'json',
        url: GetRoute('/UnitDetails/SavePromotorAddressDetails'),
        data: dataString,
        success: function (data) {
            if (data.isValid) {
                $("#PromoterDetailsProgressBar").attr('class', 'progress-bar bg-success');
                $("#panelsStayOpen-collapseSix").attr('class', 'accordion-collapse collapse')
                $("#panelsStayOpen-collapseSeven").attr('class', 'accordion-collapse collapse show');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                $("#PromoterDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#panelStayOpen-collapseSix").attr('class', 'accordion-collapse collapse show');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#PromoterDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });

});

// Author Manoj on 25/08/2022
$("#btnTabLandInspectionDetails").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveLandInspectionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#LandInspectionProgressBar").attr('class', 'progress-bar bg-success');
                $("#BuildingInspection").attr('class', 'tab-pane fade active show');
                $("#LandInspection").attr('class', 'tab-pane fade');

                $("#BuildingInspection-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#LandInspection-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#panelsStayOpen-collapse-1").attr('class', 'accordion-collapse collapse');
                $("#LandInspection_detail_accor_btn").attr('class', 'accordion-button collapsed');

                $("#panelsStayOpen-collapse-2").attr('class', 'accordion-collapse collapse show');
                $("#BuildingInspection_detail_accor_btn").attr('class', 'accordion-button');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                //window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#LandInspectionProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#LandInspectionProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });

});

//Module: ChangeLocation -> Added by Gagana

$("#previousbtnChangeLocationTab").click(function (event) {

    $("#ChangeNameofUnit").attr('class', 'tab-pane fade active show');
    $("#ChangeLocation").attr('class', 'tab-pane fade');

    $("#ChangeNameofUnit-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#ChangeLocation-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#savebtnChangeLocationTab").click(function (event) {
    debugger;
    event.preventDefault();
    let dataString = $("UDChangeLocation").serialize();
    $.ajax({
        async: false,
        type: "POST",
        datatype: 'json',
        url: GetRoute('/UnitDetails/SaveChangeLocationDetails'),
        data: dataString,
        success: function (data) {
            if (data.isValid) {
                $("#ChangeLocationProgressBar").attr('class', 'progress-bar bg-success');
                $("#ChangeBankDetails").attr('class', 'tab-pane fade active show');
                $("#ChangeLocation").attr('class', 'tab-pane fade');

                $("#ChangeBankDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#ChangeLocation-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#ChangeLocationProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {

            $("#PromoterDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

// Author Swetha on 29/08/2022
$("#btnTabBuildingDetails").click(function (event) {

    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveBuildingInspectionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#BuildingInspectionProgressBar").attr('class', 'progress-bar bg-success');
                $("#BuildMatAtSite").attr('class', 'tab-pane fade active show');
                $("#BuildingInspection").attr('class', 'tab-pane fade');

                $("#BuildMatAtSite-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#BuildingInspection-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#panelsStayOpen-collapse-2").attr('class', 'accordion-collapse collapse');
                $("#BuildingInspection_detail_accor_btn").attr('class', 'accordion-button collapsed');

                $("#panelsStayOpen-collapse-3").attr('class', 'accordion-collapse collapse show');
                $("#FurnitureInspection_detail_accor_btn").attr('class', 'accordion-button');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#BuildingInspectionProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {

            $("#BuildingInspectionProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});



$("#savebtnTabInspectionDetail").click(function (event) {
    debugger
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveInpsectionDetail'),
        success: function (data) {
            if (data.isValid) {
                $("#InspectionDetailProgressBar").attr('class', 'progress-bar bg-success');
                $("#LetterOfCredit").attr('class', 'tab-pane fade active show');
                $("#InspectionDetail").attr('class', 'tab-pane fade');


                $("#LetterOfCredit-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#InspectionDetail-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                /*window.location.reload();*/
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#InspectionDetailProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {

            $("#InspectionDetailProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

// Author Manoj on 25/08/2022
$("#btnTabmodelBuildMatAtSiteDetailsDetails").click(function (event) {

    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveBuildMatSiteInspectionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#BuildMatAtSiteClauseProgressBar").attr('class', 'progress-bar bg-success');
                $("#IndiMachInsp").attr('class', 'tab-pane fade active show');
                $("#BuildMatAtSite").attr('class', 'tab-pane fade');

                $("#IndiMachInsp-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#BuildMatAtSite-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#BuildMatAtSiteClauseProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {

            $("#BuildMatAtSiteClauseProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

// Author Manoj on 25/08/2022
$("#btnTabIndigenousMachineryInspectionDetails").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveIndigenousMachineryInspectionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#IndiMachInspProgressBar").attr('class', 'progress-bar bg-success');
                $("#ImportMachinery").attr('class', 'tab-pane fade active show');
                $("#IndiMachInsp").attr('class', 'tab-pane fade');

                $("#ImportMachinery-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#IndiMachInsp-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#panelsStayOpen-collapse-4").attr('class', 'accordion-collapse collapse');
                $("#IndigenousMachineryInspection_detail_accor_btn").attr('class', 'accordion-button collapsed');

                $("#panelsStayOpen-collapse-5").attr('class', 'accordion-collapse collapse show');
                $("#ImportedMachineryInspection_detail_accor_btn").attr('class', 'accordion-button');


                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#IndiMachInspProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {

            $("#IndiMachInspProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});



// Author Sandeep M on 02/09/2022
$("#btnTabFurnitureInspectionDetails").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/FurnitureInspection/SaveFurnitureInspectionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#FurnitureInspectionProgressBar").attr('class', 'progress-bar bg-success');
                $("#RecommDisbursementDetails").attr('class', 'tab-pane fade active show');
                $("#FurnitureInspection").attr('class', 'tab-pane fade');

                $("#RecommDisbursementDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#FurnitureInspection-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#panelsStayOpen-collapse-3").attr('class', 'accordion-collapse collapse');
                $("#FurnitureInspection_detail_accor_btn").attr('class', 'accordion-button collapsed');

                $("#panelsStayOpen-collapse-4").attr('class', 'accordion-collapse collapse show');
                $("#IndigenousMachineryInspection_detail_accor_btn").attr('class', 'accordion-button');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#FurnitureInspectionProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {

            $("#FurnitureInspectionProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});
//Akhiladevi D M 06-09-2022
$("#btnpreviousProjectCost").click(function (event) {

    //$("#MeansOfFinanceDetails").attr('class', 'tab-pane fade active show');
    //$("#ProjectCost").attr('class', 'tab-pane fade');

    //$("#MeansOfFinance-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    //$("#ProjectCost-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#panelsStayOpen-collapseSeventeen").attr('class', 'accordion-collapse collapse');
    $("#Project_Cost_Detail_accor_btn").attr('class', 'accordion-button collapsed');

    $("#panelsStayOpen-collapseSixteen").attr('class', 'accordion-collapse collapse show');
    $("#MeansOfFinance_Detail_accor_btn").attr('class', 'accordion-button');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnpreviousAllocationDetails").click(function (event) {


    $("#panelsStayOpen-collapseTwo").attr('class', 'accordion-collapse collapse');
    $("#LoanAllocation_detail_accor_btn").attr('class', 'accordion-button collapsed');

    $("#panelsStayOpen-collapseSeventeen").attr('class', 'accordion-collapse collapse show');
    $("#Project_Cost_Detail_accor_btn").attr('class', 'accordion-button');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});


//Akhiladevi D M 06-09-2022
$("#savebtnProjCostDetailTab").click(function (event) {
    debugger
    event.preventDefault();

    var totalCost = Number($("#projectCost").val());
    var totalCostFromDb = Number(localStorage.getItem("projectCost"))

    if (totalCost > totalCostFromDb || totalCost < totalCostFromDb) {
        alert("Total amount is mismatching!")
        return;
    }

    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/UnitDetails/SaveProjectCostDetails'),
        success: function (data) {
            debugger;
            if (data.isValid) {
                //$("#ProjectCostProgressBar").attr('class', 'progress-bar bg-success');
                //$("#LetterOfCredit").attr('class', 'tab-pane fade active show');
                //$("#ProjectCost").attr('class', 'tab-pane fade');

                //$("#LetterOfCredit-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                //$("#ProjectCost-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#panelsStayOpen-collapseSeventeen").attr('class', 'accordion-collapse collapse');
                $("#Project_Cost_Detail_accor_btn").attr('class', 'accordion-button collapsed');

                $("#panelsStayOpen-collapseTwo").attr('class', 'accordion-collapse collapse show');
                $("#LoanAllocation_detail_accor_btn").attr('class', 'accordion-button');



                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                //window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                // $("#ProjectCostProgressBar").attr('class', 'progress-bar bg-danger');

                $("#panelsStayOpen-collapseSeventeen").attr('class', 'accordion-collapse collapse show');
                $("#Project_Cost_Detail_accor_btn").attr('class', 'accordion-button');

                $("#panelsStayOpen-collapseEighteen").attr('class', 'accordion-collapse collapse');
                $("#LetterOfCredit_Detail_accor_btn").attr('class', 'accordion-button collapsed');

                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#ProjectCostProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});



// Author Swetha on 29/08/2022
$("#btnTabmodelImportMachineryDetails").click(function (event) {
    //debugger;
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveImportMachineryDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#ImportMachineryProgressBar").attr('class', 'progress-bar bg-success');
                $("#FurnitureInspection").attr('class', 'tab-pane fade active show');
                $("#ImportMachinery").attr('class', 'tab-pane fade');

                $("#FurnitureInspection-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#ImportMachinery-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#ImportMachineryProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#ImportMachineryProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#btnTabStatusofImplementation").click(function (event) {
    //debugger;
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveStatusOfImplementaionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#StatusImplementationProgressBar").attr('class', 'progress-bar bg-success');
                $("#StatusImplementation").attr('class', 'tab-pane fade active show');
                /* $("#StatusImplementation").attr('class', 'tab-pane fade');*/

                /*  $("#WorkingCapital-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');*/
                $("#StatusofImplementation-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#StatusImplementationProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#StatusImplementationProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#btnpreviousMeansOfFinanceDetails").click(function (event) {

    //$("#InspectionDetail").attr('class', 'tab-pane fade active show');
    //$("#MeansOfFinanceDetails").attr('class', 'tab-pane fade');

    //$("#InspectionDetail-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    //$("#MeansOfFinance-tab").attr('class', 'nav-link').attr('aria-selected', 'true');
    $("#panelsStayOpen-collapseSixteen").attr('class', 'accordion-collapse collapse');
    $("#MeansOfFinance_Detail_accor_btn").attr('class', 'accordion-button collapsed');
    $("#panelsStayOpen-collapseFifteen").attr('class', 'accordion-collapse collapse show');
    $("#Working_CapitalDetails_accor_btn").attr('class', 'accordion-button');


    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    //window.scrollTo(0, 0);

});

// Author Swetha on 29/08/2022
$("#btnTabMeansOfFinanceDetails").click(function (event) {
    debugger;
    event.preventDefault();

    var totalCost = Number($("#totalCost").val());
    var totalCostFromDb = Number(localStorage.getItem("meansOfFinance"))

    if (totalCost > totalCostFromDb || totalCost < totalCostFromDb) {
        alert("Total amount is mismatching!")
        return;
    }

    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/UnitDetails/SaveMeansOfFinanceDetails'),
        success: function (data) {
            if (data.isValid) {
                //$("#MeansOfFinanceProgressBar").attr('class', 'progress-bar bg-success');
                //$("#ProjectCost").attr('class', 'tab-pane fade active show');
                //$("#MeansOfFinanceDetails").attr('class', 'tab-pane fade');

                //$("#ProjectCost-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                //$("#MeansOfFinance-tab").attr('class', 'nav-link').attr('aria-selected', 'true');
                $("#panelsStayOpen-collapseSixteen").attr('class', 'accordion-collapse collapse');
                $("#MeansOfFinance_Detail_accor_btn").attr('class', 'accordion-button collapsed');
                $("#panelsStayOpen-collapseSeventeen").attr('class', 'accordion-collapse collapse show');
                $("#Project_Cost_Detail_accor_btn").attr('class', 'accordion-button');


                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                // window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#panelsStayOpen-collapseSixteen").attr('class', 'accordion-collapse collapse show');
                $("#MeansOfFinance_Detail_accor_btn").attr('class', 'accordion-button');
                $("#panelsStayOpen-collapseSeventeen").attr('class', 'accordion-collapse collapse');
                $("#Project_Cost_Detail_accor_btn").attr('class', 'accordion-button collapsed');

                // $("#MeansOfFinanceProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#MeansOfFinanceProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});


//$("#btnpreviousLetterOfCreditDetail").click(function (event) {

//    $("#LetterOfCredit").attr('class', 'tab-pane fade active show');
//    $("#InspectionDetail").attr('class', 'tab-pane fade');

//    $("#LetterOfCredit-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
//    $("#InspectionDetail-tab").attr('class', 'nav-link').attr('aria-selected', 'true');
//    //$("#panelsStayOpen-collapseEighteen").attr('class', 'accordion-collapse collapse');
//    //$("#LetterOfCredit_Detail_accor_btn").attr('class', 'accordion-button collapsed');

//    //$("#panelsStayOpen-collapseSeventeen").attr('class', 'accordion-collapse collapse show');
//    //$("#Project_Cost_Detail_accor_btn").attr('class', 'accordion-button');


//    $("#spanErrorMsg").html("");
//    $("#divEnquiryAlertPopup").html("");
//    window.scrollTo(0, 0);

//});
$("#btnpreviousLetterOfCreditDetail").click(function (event) {

    $("#InspectionDetail").attr('class', 'tab-pane fade active show');
    $("#LetterOfCredit").attr('class', 'tab-pane fade');

    $("#InspectionDetail-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#LetterOfCredit-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

//Author Mnaoj on 05/09/2022
$("#btnSaveLetterOfCreditDetail").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveLetterOfCreditDetail'),
        success: function (data) {
            if (data.isValid) {
                //$("#LetterOfCreditProgressBar").attr('class', 'progress-bar bg-success');
                //$("#MeansOfFinance").attr('class', 'tab-pane fade active show');
                //$("#MeansOfFinance").attr('class', 'tab-pane fade');

                //$("#MeansOfFinancew-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                //$("#LetterOfCredit-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#panelsStayOpen-collapseEighteen").attr('class', 'accordion-collapse collapse');
                $("#LetterOfCredit_Detail_accor_btn").attr('class', 'accordion-button collapsed');


                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                // window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                // $("#LetterOfCreditProgressBar").attr('class', 'progress-bar bg-danger');
                $("#panelsStayOpen-collapseEighteen").attr('class', 'accordion-collapse collapse show');
                $("#LetterOfCredit_Detail_accor_btn").attr('class', 'accordion-button');

                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#LetterOfCreditProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

function showDocumentPopup(url, title, module) {
    //debugger;
    //$.ajax({
    //    type: 'GET',
    //    url: url,
    //    success: function (res) {
    //        //debugger;
    //        if (module == null) {
    //            $('#viewDocuments .modal-body').html(res.html);
    //            $('#viewDocuments .modal-title').html(title);
    //            $('#viewDocuments').modal('show');
    //            // to make popup draggable
    //            $('.modal-dialog').draggable({
    //                handle: ".modal-header"
    //            });
    //        }
    //        else {
    //            $('#modelSidbidocumentupload .modal-body').html(res.html);
    //            $('#modelSidbidocumentupload .modal-title').html(title);
    //            $('#modelSidbidocumentupload').modal('show');
    //            // to make popup draggable
    //            $('.modal-dialog').draggable({
    //                handle: ".modal-header"
    //            });
    //        }

    //    },
    //    error: function (err) {
    //        console.log(err);
    //    }
    //});
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            debugger;
            if (module == null || module == undefined) {
                $('#modelChargeDetails .modal-body').html(res.html);
                $('#modelChargeDetails .modal-title').html(title);
                $('#modelChargeDetails').modal('show');
                // to make popup draggable
                $('.modal-dialog').draggable({
                    handle: ".modal-header"
                });
                ReloadChargeDetails()
            }
            else {
                $('#modelSidbidocumentupload .modal-body').html(res.html);
                $('#modelSidbidocumentupload .modal-title').html(title);
                $('#modelSidbidocumentupload').modal('show');
                // to make popup draggable
                $('.modal-dialog').draggable({
                    handle: ".modal-header"
                });
            }

        },
        error: function (err) {
            console.log(err);
        }
    });
}

function ReloadChargeDetails() {
    var dataTable;
    dataTable = $('tblChargeDetailsDatatablecreate').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}

function showDisDocumentPopup(url, title) {
    //debugger;
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            debugger;
            $('#viewDisburmentDocuments .modal-body').html(res.html);
            $('#viewDisburmentDocuments .modal-title').html(title);
            $('#viewDisburmentDocuments').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        },
        error: function (err) {
            console.log(err); btnpreviousProjectCost
        }
    });
}

$("#btnpreviousworkingcapital").click(function (event) {

    $("#RecommDisbursementDetails").attr('class', 'tab-pane fade active show');
    $("#WorkingCapital").attr('class', 'tab-pane fade');

    $("#RecommDisbursementDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#WorkingCapital-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});



$("#btnpreviousimportmachinary").click(function (event) {

    $("#IndiMachInsp").attr('class', 'tab-pane fade active show');
    $("#ImportMachinery").attr('class', 'tab-pane fade');

    $("#IndiMachInsp-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#ImportMachinery-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});


$("#btnpreviousRecommDisbursementDetails").click(function (event) {

    $("#FurnitureInspection").attr('class', 'tab-pane fade active show');
    $("#RecommDisbursementDetails").attr('class', 'tab-pane fade');

    $("#FurnitureInspection-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#RecommDisbursementDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});








$("#btnpreviousIndigenousMachinery").click(function (event) {

    $("#BuildMatAtSite").attr('class', 'tab-pane fade active show');
    $("#IndiMachInsp").attr('class', 'tab-pane fade');

    $("#BuildMatAtSite-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#IndiMachInsp-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnpreviousStatusofImplementation").click(function (event) {
    $("#RecommDisbursementDetails").attr('class', 'tab-pane fade active show');
    $("#StatusofImplementation").attr('class', 'tab-pane fade');

    $("#RecommDisbursementDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#StatusofImplementation-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnpreviousFurnitureInspectionDetails").click(function (event) {

    $("#ImportMachinery").attr('class', 'tab-pane fade active show');
    $("#FurnitureInspection").attr('class', 'tab-pane fade');

    $("#ImportMachinery-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#FurnitureInspection-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnpreviousBuildMatAtSiteDetailsDetails").click(function (event) {

    $("#BuildingInspection").attr('class', 'tab-pane fade active show');
    $("#BuildMatAtSite").attr('class', 'tab-pane fade');

    $("#BuildingInspection-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#BuildMatAtSite-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnpreviousBuildingDetails").click(function (event) {

    $("#LandInspection").attr('class', 'tab-pane fade active show');
    $("#BuildingInspection").attr('class', 'tab-pane fade');

    $("#LandInspection-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#BuildingInspection-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});


//Akhiladevi D M 06-09-2022
$("#btnpreviousProjectCost").click(function (event) {

    $("#MeansOfFinanceDetails").attr('class', 'tab-pane fade active show');
    $("#ProjectCostDetails").attr('class', 'tab-pane fade');

    $("#MeansOfFinance-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#ProjectCost-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    // window.scrollTo(0, 0);

});


// Author Manoj on 09/09/2022
$("#btnTabSaveAllPromoterDetails").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/UnitDetails/SaveAllPromoterDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#PromoterDetailsProgressBar").attr('class', 'progress-bar bg-success');
                ////$("#panelsStayOpen-collapseNine").attr('class', 'accordion-collapse collapse')
                //$("#panelsStayOpen-collapseTen").attr('class', 'accordion-collapse collapse show');
                //$("#ChangeProductDetails").attr('class', 'tab-pane fade active show');
                //$("#ChangePromoterDetails").attr('class', 'tab-pane fade');

                //$("#ChangeProductDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                //$("#ChangePromoterDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                //$("#UnitDetailsProgressBar").attr('class', 'progress-bar bg-success');
                $("#MoneteringDetails").attr('class', 'tab-pane fade active show');
                $("#ChangePromoterDetails").attr('class', 'tab-pane fade');

                $("#ChangePromoterDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');
                $("#MoneteringDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');



                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#PromoterDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#PromoterDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});


//Author Mnaoj on 05/09/2022
$("#btnTabChangeBankDetails").click(function (event) {
    debugger;
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/UnitDetails/SaveChangeBankDetail'),
        success: function (data) {
            debugger;
            if (data.isValid) {
                $("#ChangeBankDetailsProgressBar").attr('class', 'progress-bar bg-success');
                $("#ChangePromoterDetails").attr('class', 'tab-pane fade active show');
                $("#ChangeBankDetails").attr('class', 'tab-pane fade');

                $("#ChangePromoterDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#ChangeBankDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#ChangeBankDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#ChangeBankDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#btnTabSaveUnitDetails").click(function (event) {
    debugger;
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/UnitDetails/SaveAllUnitDetails'),
        success: function (data) {
            debugger;
            if (data.isValid) {
                $("#UnitDetailsProgressBar").attr('class', 'progress-bar bg-success');
                $("#ChangePromoterDetails").attr('class', 'tab-pane fade active show');
                $("#UnitDetails").attr('class', 'tab-pane fade');

                $("#ChangePromoterDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#UnitDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Unit Details Data.',
                    'error'
                )
                $("#UnitDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#UnitDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#btnTabChangeProductDetails").click(function (event) {

    event.preventDefault();
    let dataString = $("UDProductDetails").serialize();
    $.ajax({
        async: false,
        type: "POST",
        datatype: 'json',
        url: GetRoute('/UnitDetails/SaveProductDetails'),
        data: dataString,
        success: function (data) {
            if (data.isValid) {

                $("#ListofChangeProductDetails").attr('class', 'progress-bar bg-success');
                $("#panelsStayOpen-collapseNine").attr('class', 'accordion-collapse collapse show');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#ListofChangeProductDetails").attr('class', 'progress-bar bg-danger');
                $("#panelsStayOpen-collapseNine").attr('class', 'accordion-collapse collapse show');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#ListofChangeProductDetails").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });

});


$("#previousbtnChangeProductTab").click(function (event) {
    debugger
    $("#ChangePromoterDetails").attr('class', 'tab-pane fade active show');
    $("#ChangeProductDetails").attr('class', 'tab-pane fade');

    $("#ChangePromoterDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#ChangeProductDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#previousbtnBankdetailsTab").click(function (event) {

    $("#ChangeLocation").attr('class', 'tab-pane fade active show');
    $("#ChangeBankDetails").attr('class', 'tab-pane fade');

    $("#ChangeLocation-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#ChangeBankDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#previousbtnTabChangeProductDetails").click(function (event) {
    debugger
    $("#UnitDetails").attr('class', 'tab-pane fade active show');
    $("#ChangePromoterDetails").attr('class', 'tab-pane fade');

    $("#UnitDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#ChangePromoterDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

// Creation of Security and Acquisition of Assets

$("#previousbtnMachineryAcqTab").click(function (event) {

    $("#BuildingAcquisition").attr('class', 'tab-pane fade active show');
    $("#MachineryAcquisition").attr('class', 'tab-pane fade');

    $("#BuildingAcquisition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#MachineryAcquisition-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnTabMachineryAcqDetails").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/CreationOfSecurityandAquisitionAsset/SaveMachineryAcquisitionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#MachineryAcquisitionProgressBar").attr('class', 'progress-bar bg-success');
                $("#FurnitureAcquisition").attr('class', 'tab-pane fade active show');
                $("#MachineryAcquisition").attr('class', 'tab-pane fade');

                $("#FurnitureAcquisition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#MachineryAcquisition-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#MachineryAcquisitionProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#MachineryAcquisitionProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#btnTabLandAcqDetails").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/CreationOfSecurityandAquisitionAsset/SaveLandAcquisitionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#LandAcquisitionProgressBar").attr('class', 'progress-bar bg-success');
                $("#BuildingAcquisition").attr('class', 'tab-pane fade active show');
                $("#LandAcquisition").attr('class', 'tab-pane fade');

                $("#BuildingAcquisition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#LandAcquisition-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#LandAcquisitionProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#LandAcquisitionProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#btnTabAllocationDetails").click(function (event) {
    event.preventDefault();
    debugger
    var totalCost = Number($("#allocation").val());
    var totalCostFromDb = Number(localStorage.getItem("allocation"))

    if (totalCost > totalCostFromDb || totalCost < totalCostFromDb) {
        alert("Total amount is mismatching!")
        return;
    } else {
        localStorage.clear()
    }

    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/UnitDetails/SaveAllocationDetails'),
        success: function (data) {
            if (data.isValid) {
                //$("#LoanAllocationProgressBar").attr('class', 'progress-bar bg-success');
                //$("#LoanAllocation").attr('class', 'tab-pane fade active show');

                //$("#LoanAllocation-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#panelsStayOpen-collapseTwo").attr('class', 'accordion-collapse collapse');
                $("#LoanAllocation_detail_accor_btn").attr('class', 'accordion-button collapsed');

                //$("#panelsStayOpen-collapseEighteen").attr('class', 'accordion-collapse collapse show');
                //$("#LetterOfCredit_Detail_accor_btn").attr('class', 'accordion-button');



                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
                window.location.href = "/Admin/Home/Dashboard/"
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                /*  $("#LoanAllocationProgressBar").attr('class', 'progress-bar bg-danger');*/
                $("#panelsStayOpen-collapseTwo").attr('class', 'accordion-collapse collapse show');
                $("#LoanAllocation_detail_accor_btn").attr('class', 'accordion-button');

                //$("#panelsStayOpen-collapseEighteen").attr('class', 'accordion-collapse collapse');
                //$("#LetterOfCredit_Detail_accor_btn").attr('class', 'accordion-button collapsed');

                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#LoanAllocationProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#btnTabFurnitureAcquisitionDetails").click(function (event) {
    event.preventDefault();
    console.log("Inside the Furniture Acquisition Function.");
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/CreationOfSecurityandAquisitionAsset/SaveFurnitureAcquisition'),
        success: function (data) {
            if (data.isValid) {
                console.log("Furniture Data is valid.");
                $("#FurnitureAcquisitionProgressBar").attr('class', 'progress-bar bg-success');
                $("#FurnitureAcquisition").attr('class', 'tab-pane fade active show');

                $("FurnitureAcquisition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#FurnitureAcquisitionProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#FurnitureAcquisitionProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#previousbtnBuildingAcqTab").click(function (event) {

    $("#LandAcquisition").attr('class', 'tab-pane fade active show');
    $("#BuildingAcquisition").attr('class', 'tab-pane fade');

    $("#LandAcquisition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#BuildingAcquisition-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnTabBuildingAcqDetails").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/CreationOfSecurityandAquisitionAsset/SaveBuildingAcquisitionDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#BuildingAcquisitionProgressBar").attr('class', 'progress-bar bg-success');
                $("#MachineryAcquisition").attr('class', 'tab-pane fade active show');
                $("#BuildingAcquisition").attr('class', 'tab-pane fade');

                $("#MachineryAcquisition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#BuildingAcquisition-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#BuildingAcquisitionProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#BuildingAcquisitionProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#PreviousButtonFurnitureAcuisition").click(function (event) {
    $("#MachineryAcquisition").attr('class', 'tab-pane fade active show');
    $("#FurnitureAcquisition").attr('class', 'tab-pane fade');

    $("#MachineryAcquisition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#FurnitureAcquisition-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnTabRecommDisbursementDetails").click(function (event) {
    debugger
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveRecommendedDisbursementDetails'),
        success: function (data) {
            debugger
            if (data.isValid) {
                $("#RecommDisbursementDetailsProgressBar").attr('class', 'progress-bar bg-success');
                $("#StatusofImplementation").attr('class', 'tab-pane fade active show');
                $("#RecommDisbursementDetails").attr('class', 'tab-pane fade');

                $("#StatusofImplementation-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#RecommDisbursementDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                $("#RecommDisbursementDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#RecommDisbursementDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});



$("#btnTabProposalDetailsPrevious").click(function (event) {

    $("#RecommDisbursementDetails").attr('class', 'tab-pane fade active show');
    $("#ProposalDetails").attr('class', 'tab-pane fade');

    $("#RecommDisbursementDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#ProposalDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#previousbtnBeneficiaryDetailsTab").click(function (event) {

    $("#ChargeDetails").attr('class', 'tab-pane fade active show');
    $("#BeneficiaryDetails").attr('class', 'tab-pane fade');

    $("#ChargeDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#BeneficiaryDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#btnTabProposalDetailsNext").click(function (event) {
    debugger
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/CreationOfDisbursmentProposal/SaveProposalDetails'),
        success: function (data) {
            debugger
            if (data.isValid) {
                $("#ProposalDetailsProgressBar").attr('class', 'progress-bar bg-success');
                //$("#BeneficiaryDetails").attr('class', 'tab-pane fade active show');
                //$("#ProposalDetails").attr('class', 'tab-pane fade');

                //$("#BeneficiaryDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                //$("#ProposalDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
                window.location.reload()
            }
            else {
                $("#ProposalDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#ProposalDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});

$("#btnTabChargeDetailsNext").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/CreationOfDisbursmentProposal/SaveProposalDetails'),
        success: function (data) {
            debugger
            if (data.isValid) {
                $("#ChargeDetailsProgressBar").attr('class', 'progress-bar bg-success');
                $("#BeneficiaryDetails").attr('class', 'tab-pane fade active show');
                $("#ChargeDetails").attr('class', 'tab-pane fade');

                $("#BeneficiaryDetails-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#ChargeDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                $("#ChargeDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#ChargeDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
})



function PrintPanel(Id) {
    debugger;
    $.ajax({
        type: 'POST',
        data: { id: Id },
        dataType: 'html',
        url: GetRoute('/DisbursmentProposalDetails/DownloadToPdf'),
        success: function (partialViewData) {
            $('#DownloadPanel').html(partialViewData);

            debugger;
            let printContents = document.getElementById("DownloadPanel").innerHTML;
            let originalContents = document.body.innerHTML;
            document.body.innerHTML = "<!DOCTYPE html><head><title>Print Data</title></head><body> <div class=row > " + printContents + "</div></body>";


            window.print();
            document.body.innerHTML = originalContents;
            $('#DownloadPanel').html("");
            return false;
        },
        error: function (err) {
            console.log(err)
        }
    })
}
$("#fxnSaveOtherDebits").click(function (event) {
    debugger;

    swal.fire({
        title: 'Submit Debit Details',
        text: "Once the Details are Submited, you will not be able to edit it further or delete it, are you sure you want to Submit it?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Submit',
        cancelButtonText: 'Cancel',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            try {
                $.ajax({
                    async: false,
                    type: 'POST',
                    url: GetRoute('/EntryOfOtherDebits/SubmitOtherDebitsDetails'),
                    success: function (res) {
                        $.ajax({
                            async: false,
                            type: 'POST',
                            url: GetRoute('/EntryOfOtherDebits/SaveOtherDebitsDetails')
                        })
                        location.href = "/Admin/";
                    },
                    error: function (err) {
                        $("#OtherDebitsProgressBar").attr('class', 'progress-bar bg-danger');
                        console.log(err);
                    }
                });
            } catch (ex) {
                console.log(ex)
            }
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            return false;
        }
        return false;
    })

});

$("#fxnSubmitOtherDebits").click(function (event) {
    debugger;


    $.ajax({
        async: false,
        type: 'POST',
        url: GetRoute('/EntryOfOtherDebits/SaveOtherDebitsDetails'),
        success: function (data) {
            if (data.isValid) {
                location.href = "/Admin/";
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#OtherDebitsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#OtherDebitsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });



});


$("#BuildingInspectionDetailsAd").click(function () {

    $("#panelsStayOpen-collapse-2").attr('class', 'accordion-collapse collapse');
    $("#BuildingInspection_detail_accor_btn").attr('class', 'accordion-button collapsed');

    $("#panelsStayOpen-collapse-1").attr('class', 'accordion-collapse collapse show');
    $("#LandInspection_detail_accor_btn").attr('class', 'accordion-button');

})

$("#btnFurnitureInspectionDetailsAd").click(function () {
    $("#panelsStayOpen-collapse-3").attr('class', 'accordion-collapse collapse');
    $("#FurnitureInspection_detail_accor_btn").attr('class', 'accordion-button collapsed');

    $("#panelsStayOpen-collapse-2").attr('class', 'accordion-collapse collapse show');
    $("#BuildingInspection_detail_accor_btn").attr('class', 'accordion-button');
})


$("#btnIndigenousMachineryInspectionDetailsAd").click(function () {
    $("#panelsStayOpen-collapse-4").attr('class', 'accordion-collapse collapse');
    $("#IndigenousMachineryInspection_detail_accor_btn").attr('class', 'accordion-button collapsed'); 

    $("#panelsStayOpen-collapse-3").attr('class', 'accordion-collapse collapse show');
    $("#FurnitureInspection_detail_accor_btn").attr('class', 'accordion-button');

})


$("#btnImportedMachineryInspectionDetailsAd").click(function () {
    $("#panelsStayOpen-collapse-5").attr('class', 'accordion-collapse collapse');
    $("#ImportedMachineryInspection_detail_accor_btn").attr('class', 'accordion-button collapsed');

    $("#panelsStayOpen-collapse-4").attr('class', 'accordion-collapse collapse show');
    $("#IndigenousMachineryInspection_detail_accor_btn").attr('class', 'accordion-button');

})


$("#btnTabmodelImportMachineryDetailsAd").click(function (event) {
    event.preventDefault();
    $.ajax({
        async: false,
        type: "POST",
        url: GetRoute('/InspectionOfUnit/SaveImportMachineryDetails'),
        success: function (data) {
            if (data.isValid) {
                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.location.href = "/Admin/Home/Dashboard"
            }
            else {
                swal.fire(
                    'Error',
                    'An Error Occured While Saving the Data.',
                    'error'
                )
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
        }
    });
});




