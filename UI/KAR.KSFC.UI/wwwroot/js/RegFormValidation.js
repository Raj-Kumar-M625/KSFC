$(document).ready(function () {

    $.ajaxSetup({
        beforeSend: function () {
            $(".modalLoading").show();
        },
        complete: function () {
            $(".modalLoading").hide();
        }
    });

    var mobileRegex = /^([6-9]{1}[0-9]{9})$/;
    var numberRegex = /^([0-9])+$/;
    var alphaNumRegex = /^([0-9A-Za-z])+$/;
    var mobileOtpRegex = /^([0-9]{6})$/;
    var captchaRegex = /^([0-9A-Za-z]{6})$/;
    var panNoRegex = /^([A-Za-z]{3}[p|c|h|a|b|g|j|l|f|t|P|C|H|A|B|G|J|L|F|T]{1}[A-Za-z]{1}[0-9]{4}[A-Za-z]{1})$/;
    var ddlSelConstitutionType = document.getElementById("ddlConstitutionType");
    var txtMobileNo = document.getElementById("txtBoxMobileNoReg");
    var btnGenOtpForMobile = document.getElementById("btnGenOtpForMobForm");
    var btnResOtpForMobile = document.getElementById("btnResOtpForMobForm");
    var btnValOtpForMobile = document.getElementById("btnValOtpForMobForm");
    var txtEnterOtp = document.getElementById("txtEnterOtpForMbForm");
    var txtPanNo = document.getElementById("txtBoxPanNoForm");
    var btnValPanNo = document.getElementById("btnValPanNoForm");
    var checkBoxTAndC = document.getElementById("chkBoxTAndCForm");
    var btnSubmitRegisterForm = document.getElementById("btnSubRegisterForm");
    var txtCaptchaRF = document.getElementById("txtEnterCaptcha");

    //var boolPanAndConstitutionTypeMatch = false;

    ddlSelConstitutionType.focus();

    //Hiding modal popup alert
    $('#btnHideModal').click(function () {
        $('#modalAlert').modal('hide');
    });

    //on change of Constitution type
    $("#ddlConstitutionType").change(function () {
        if (ddlSelConstitutionType.value != 0) {
            $("#ddlConstitutionType").css("border", "1px solid #00FF00");
            txtMobileNo.disabled = false;
            txtMobileNo.focus();
            // $("#modalAlert").modal('show');
        }
        else {
            $("#ddlConstitutionType").css("border", "1px solid #FF0000");
            $("#btnGenOtpForMobForm").css("display", "block");
            $("#btnResOtpForMobForm").css("display", "none");
            $("#pMsgCountdownTimer").css("display", "none");

            txtMobileNo.value = "";
            txtMobileNo.disabled = true;
            btnGenOtpForMobile.disabled = true;
        }
    });

    //Don't let user to copy/paste the Mobile no.
    $("#txtBoxMobileNoReg").bind('copy paste', function (e) {
        e.preventDefault();
    });

    // Lets user to enter only numbers
    $("#txtBoxMobileNoReg").on('keypress', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    //on change of Mobile Number
    $("#txtBoxMobileNoReg").keyup(function () {
        document.getElementById("iconBtnGenOtp").className = "";
        var mobileNo = parseFloat(txtMobileNo.value)
        if (mobileRegex.test(mobileNo)) {
            $("#txtBoxMobileNoReg").css("border", "1px solid #00FF00");
            btnGenOtpForMobile.disabled = false;
            btnResOtpForMobile.disabled = false;

        }
        else {
            $("#txtBoxMobileNoReg").css("border", "1px solid #FF0000");
            btnGenOtpForMobile.disabled = true;
            btnResOtpForMobile.disabled = true;
        }
    });

    //on click of generate otp button

    //Parameters for Generate OTP and Resend OTP
    var resMessageId = "0";
    var resMessage = "";
    var otpExpTimeLeft = 0;
    var timeDefined = 0;
    var downloadTimer = 5;
    var messageInJson = "";
    var mobileNumEnd = "";
    $("#btnGenOtpForMobForm").click(function () {

        $.ajax({
            url: "/Account/GenerateOtp",
            data: { mobileNo: txtMobileNo.value },
            dataType: "json",
            type: "POST",
            success: function (data) {
                resMessageId = data.id;
                resMessage = data.message;
                otpExpTimeLeft = data.otpExpTimeLeftInSec;
                mobileNumEnd = data.mobile;
            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            if (resMessageId == "0") {
                //When OTP is not generated then show modal popup
                //messageInJson = resMessage.replace('"', '').replace('"', '');
                document.getElementById("pModalAlertPup").textContent = resMessage;

                //document.getElementById("divModelAlertPup").className = "modal-body text-danger";
                $("#modalAlert").modal('show');

                $("#pErrMsgForMb").css("display", "block");
                document.getElementById("iconBtnGenOtp").className = "bi bi-x text-danger ";
                //window.location.href = ("/Account/Result?message=" + resMessage);
                document.getElementById("pErrMsgForMb").textContent = resMessage;
                document.getElementById("pErrMsgForMb").className = "text-danger";
                //$("#pErrMsgForMb").css("color", "#FF0000");
            } else if (resMessageId == "1") {
                document.getElementById("pModalAlertPup").textContent = resMessage;//"OTP successfully generated. Received OTP is valid for next 10 minutes.";
                document.getElementById("spanCurrentMobileNoReg").textContent = mobileNumEnd;
                //document.getElementById("divModelAlertPup").className = "modal-body text-success";
                $("#modalAlert").modal('show');
                //When OTP is generated or Already generated then Execute following
                document.getElementById("iconBtnGenOtp").className = "bi bi-check ";
                $("#btnGenOtpForMobForm").css("display", "block");
                $("#pErrMsgForMb").css("display", "none");
                //$("#pMsgNoOfAttemptsForSMS").css("display", "block");
                $("#btnResOtpForMobForm").css("display", "none");
                btnGenOtpForMobile.disabled = true;
                txtMobileNo.disabled = true;
                txtEnterOtp.disabled = false;
                txtEnterOtp.focus();
                ddlSelConstitutionType.disabled = true;
              //  document.getElementById("pMsgNoOfAttemptsForSMS").textContent = resMessage;
                if (otpExpTimeLeft > 0) {
                    timeDefined = otpExpTimeLeft;
                }
                CreateTimer();
            }
            else if (resMessageId == "2") {
                document.getElementById("iconBtnResOtp").className = "bi bi-check";
                txtMobileNo.disabled = true;
                btnResOtpForMobile.disabled = true;
                btnGenOtpForMobile.disabled = true;
                $("#btnResOtpForMobForm").css("display", "none");
                $("#pMsgCountdownTimer").css("display", "none");
                $("#pMsgNoOfAttemptsForSMS").css("display", "none");

                ClearTimer();

                document.getElementById("pErrMsgForMb").textContent = resMessage;
                $("#pErrMsgForMb").css("display", "block");
                txtEnterOtp.disabled = false;
                ddlSelConstitutionType.disabled = true;
                document.getElementById("pModalAlertPup").textContent = resMessage;
                //document.getElementById("divModelAlertPup").className = "modal-body text-success";
                $("#modalAlert").modal('show');
            }
        });
    });


    function ClearTimer() {
        clearInterval(downloadTimer);
    }

    $("#btnResOtpForMobForm").click(function () {
        $.ajax({
            url: "/Account/ResendOtp",
            data: { mobileNo: txtMobileNo.value },
            dataType: "json",
            type: "POST",
            success: function (data) {
                resMessageId = data.id;
                resMessage = data.message;
                otpExpTimeLeft = data.otpExpTimeLeftInSec;
                mobileNumEnd = data.mobile;
            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            //After complete loading data
            if (resMessageId == "1") {
                document.getElementById("iconBtnResOtp").className = "bi bi-check";//bi bi-x text-danger btn-danger     bi bi-check
                $("#btnGenOtpForMobForm").css("display", "none");
                $("#pErrMsgForMb").css("display", "none");
                $("#btnResOtpForMobForm").css("display", "block");
                $("#pMsgNoOfAttemptsForSMS").css("display", "block");
                btnResOtpForMobile.disabled = true;
                txtMobileNo.disabled = true;
                txtEnterOtp.disabled = false;
                txtEnterOtp.focus();
                ddlSelConstitutionType.disabled = true;
                txtEnterOtp.focus();
                messageInJson = resMessage.replace('"', '').replace('"', '');
                document.getElementById("pModalAlertPup").textContent = messageInJson;
                //document.getElementById("divModelAlertPup").className = "modal-body text-success";
                $("#modalAlert").modal('show');
                document.getElementById("pMsgNoOfAttemptsForSMS").textContent = resMessage;
                document.getElementById("spanCurrentMobileNoReg").textContent = mobileNumEnd;
                ClearTimer();
                if (otpExpTimeLeft > 0) {
                    timeDefined = otpExpTimeLeft;
                }
                CreateTimer();
            }
            else if (resMessageId == "2") {
                document.getElementById("iconBtnResOtp").className = "bi bi-check";
                txtMobileNo.disabled = true;
                btnResOtpForMobile.disabled = true;
                $("#btnResOtpForMobForm").css("display", "none");
                $("#pMsgCountdownTimer").css("display", "none");
                $("#pMsgNoOfAttemptsForSMS").css("display", "none");

                ClearTimer();

                document.getElementById("pErrMsgForMb").textContent = resMessage;
                $("#pErrMsgForMb").css("display", "block");
                txtEnterOtp.disabled = false;
                ddlSelConstitutionType.disabled = true;
                document.getElementById("pModalAlertPup").textContent = resMessage;
                //document.getElementById("divModelAlertPup").className = "modal-body text-success";
                $("#modalAlert").modal('show');
            }
            else {
                document.getElementById("iconBtnResOtp").className = "bi bi-x text-danger";
                $("#pMsgCountdownTimer").css("display", "none");
                ClearTimer();
                $("#pErrMsgForMb").css("display", "block");
                $("#pErrMsgForMb").append("");
                txtEnterOtp.disabled = true;
                ddlSelConstitutionType.disabled = true;
            }
        });
    });

    //Create Timer for sending OTP--setting timeDefined before calling CreateTimer
    function CreateTimer() {
        document.getElementById("spanTimeCountdownTimer").textContent = timeDefined;
        var timeLeft = timeDefined;
        $("#pMsgCountdownTimer").css("display", "block");
        downloadTimer = setInterval(function () {
            timeLeft--;
            document.getElementById("spanTimeCountdownTimer").textContent = timeLeft;
            if (timeLeft <= 0) {
                clearInterval(downloadTimer);//call to clear timer, when Validate Otp button clicked by user
                $("#btnResOtpForMobForm").css("display", "block");
                $("#btnGenOtpForMobForm").css("display", "none");
                btnResOtpForMobile.disabled = false;
                txtMobileNo.disabled = false;
            }
        }, 1000);
    }

    $("#txtEnterOtpForMbForm").bind('copy paste', function (e) {
        e.preventDefault();
    });

    // Lets user to enter only numbers
    $("#txtEnterOtpForMbForm").on('keypress', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    //On keyup event for Enter otp text box
    $("#txtEnterOtpForMbForm").keyup(function () {
        //Removes characters other than numbers
        document.getElementById("iconBtnValidateOtp").className = "";
        if (mobileOtpRegex.test(txtEnterOtp.value)) {
            $("#txtEnterOtpForMbForm").css("border", "1px solid #00FF00");
            btnResOtpForMobile.disabled = true;
            txtMobileNo.disabled = true;
            btnValOtpForMobile.disabled = false;
        }
        else {
            $("#txtEnterOtpForMbForm").css("border", "1px solid #FF0000");
            btnValOtpForMobile.disabled = true;
        }
    });

    //on click of validate otp button
    $("#btnValOtpForMobForm").click(function () {

        $.ajax({
            url: "/Account/ValidateOtp",
            data: { entOtp: txtEnterOtp.value, mobileNum: txtMobileNo.value },
            dataType: "json",
            type: "POST",
            success: function (data) {
                resMessageId = data.id;
                resMessage = data.message;

            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            //After complete loading data
            $("#pMsgNoOfAttemptsForSMS").css("display", "none");
            if (resMessageId == "1") {
                document.getElementById("iconBtnValidateOtp").className = "bi bi-check";

                $("#pMsgCountdownTimer").css("display", "none");
                ClearTimer();
                btnResOtpForMobile.disabled = true;
                btnValOtpForMobile.disabled = true;
                txtEnterOtp.disabled = true;
                txtMobileNo.disabled = true;
                txtPanNo.disabled = false;
                txtPanNo.focus();

                messageInJson = resMessage.replace('"', '').replace('"', '');
                document.getElementById("pModalAlertPup").textContent = messageInJson;
                //document.getElementById("divModelAlertPup").className = "modal-body text-success";
                $("#modalAlert").modal('show');

                document.getElementById("pMsgMobileOtpVal").textContent = messageInJson;
                document.getElementById("pMsgMobileOtpVal").className = "text-success";
                //$("#pMsgMobileOtpVal").css("color", "#0fad04");
                $("#pMsgMobileOtpVal").css("display", "block");
            } else if (resMessageId == "2") {
                document.getElementById("iconBtnValidateOtp").className = "bi bi-x text-danger";
                $("#pMsgCountdownTimer").css("display", "none");
                ClearTimer();
                btnValOtpForMobile.disabled = true;
                btnResOtpForMobile.disabled = true;
                txtMobileNo.disabled = true;
                txtEnterOtp.disabled = true;
                txtEnterOtp.focus();
                ddlSelConstitutionType.disabled = true;

                document.getElementById("pMsgMobileOtpVal").textContent = resMessage;
                document.getElementById("pMsgMobileOtpVal").className = "text-danger";
                //$("#pMsgMobileOtpVal").css("color", "#0000ff");
                $("#pMsgMobileOtpVal").css("display", "block");

                document.getElementById("pModalAlertPupHome").textContent = resMessage;
                $("#modalAlertHome").modal('show');

            } else {
                document.getElementById("iconBtnValidateOtp").className = "bi bi-x text-danger";
                $("#pMsgCountdownTimer").css("display", "none");
                ClearTimer();
                ddlSelConstitutionType.disabled = true;
                btnResOtpForMobile.disabled = true;
                txtMobileNo.disabled = true;
                txtEnterOtp.disabled = false;
                txtEnterOtp.focus();


                messageInJson = resMessage.replace('"', '').replace('"', '');
                document.getElementById("pMsgMobileOtpVal").textContent = messageInJson;
                document.getElementById("pMsgMobileOtpVal").className = "text-danger";
                $("#pMsgMobileOtpVal").css("display", "block");

                document.getElementById("pModalAlertPup").textContent = messageInJson;
                // document.getElementById("divModelAlertPup").className = "modal-body text-danger";
                $("#modalAlert").modal('show');

            }
        });
    });

    $("#txtBoxPanNoForm").bind('copy paste', function (e) {
        e.preventDefault();
    });

    $("#txtBoxPanNoForm").on('keypress', function (event) {
        var regex = new RegExp("^[0-9A-Za-z]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    //on key up event PAN text box
    $("#txtBoxPanNoForm").keyup(function () {
        document.getElementById("iconBtnValidatePan").className = "";
        $("#pMsgMobileOtpVal").css("display", "none");
        if (($('#txtBoxPanNoForm').val().length == 10) && !panNoRegex.test(txtPanNo.value)) {
            document.getElementById("pMsgPanVerifyWithDb").className = "text-danger";
            document.getElementById("pMsgPanVerifyWithDb").textContent = "Entered PAN is not in correct format";
            $("#pMsgPanVerifyWithDb").css("display", "block");
        }
        else
            document.getElementById("pMsgPanVerifyWithDb").textContent = "";

        if (panNoRegex.test(txtPanNo.value)) {
            $("#txtBoxPanNoForm").css("border", "1px solid #00FF00");
            btnValPanNo.disabled = false;
            txtMobileNo.disabled = true;
        }
        else {
            $("#txtBoxPanNoForm").css("border", "1px solid #FF0000");
            btnValPanNo.disabled = true;
        }
    });

    //on click event Validate Pan Button
    $('#btnValPanNoForm').click(function () {

        var selectedConstiTypeName = $("#ddlConstitutionType option:selected").text();

        $.ajax({
            url: "/Account/VerifyPanWithConstTypeAndDb",
            data: { panNo: txtPanNo.value, constitutionName: selectedConstiTypeName, mobileNum: txtMobileNo.value },
            dataType: "json",
            type: "POST",
            success: function (data) {
                resMessageId = data.id;
                resMessage = data.message;

            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            //After complete loading data
            if (resMessageId == "2") {
                document.getElementById("iconBtnValidatePan").className = "";
                btnValPanNo.disabled = true;
                txtPanNo.disabled = true;
                document.getElementById("pMsgPanVerifyWithDb").textContent = resMessage;
                document.getElementById("pMsgMobileOtpVal").className = "text-success";
                //$("#pMsgPanVerifyWithDb").css("color", "#0000ff");
                $("#pMsgPanVerifyWithDb").css("display", "block");
            }
            else if (resMessageId == "1") {
                document.getElementById("iconBtnValidatePan").className = "bi bi-check";
                btnValPanNo.disabled = true;
                txtPanNo.disabled = true;

                txtCaptchaRF.disabled = false;
                txtCaptchaRF.focus();
                messageInJson = resMessage.replace('"', '').replace('"', '');
                document.getElementById("pModalAlertPup").textContent = messageInJson;
                // document.getElementById("divModelAlertPup").className = "modal-body text-success";
                $("#modalAlert").modal('show');

                document.getElementById("pMsgPanVerifyWithDb").textContent = messageInJson;
                document.getElementById("pMsgPanVerifyWithDb").className = "text-success";
                //$("#pMsgPanVerifyWithDb").css("color", "#0fad04");//class="text-success"
                $("#pMsgPanVerifyWithDb").css("display", "block");
            }
            else {
                document.getElementById("iconBtnValidatePan").className = "bi bi-x text-danger";
                txtPanNo.disabled = false;
                btnValPanNo.disabled = false;

                messageInJson = resMessage.replace('"', '').replace('"', '');
                document.getElementById("pMsgPanVerifyWithDb").textContent = messageInJson;
                document.getElementById("pMsgPanVerifyWithDb").className = "text-danger";
                //$("#pMsgPanVerifyWithDb").css("color", "#ff0000");
                $("#pMsgPanVerifyWithDb").css("display", "block");

                document.getElementById("pModalAlertPup").textContent = messageInJson;
                //document.getElementById("divModelAlertPup").className = "modal-body text-danger";
                $("#modalAlert").modal('show');
            }
        });
    });

    $("#txtEnterCaptcha").bind('copy paste', function (e) {
        e.preventDefault();
    });

    $("#txtEnterCaptcha").on('keypress', function (event) {
        var regex = new RegExp("^[0-9A-Za-z]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    $("#txtEnterCaptcha").keyup(function () {
        var captchaRegex = /^([0-9A-Za-z]{6})$/;
        if (captchaRegex.test(txtCaptchaRF.value)) {
            var txtGenCaptchaRF = document.getElementById("Generatedcaptcha").value.replace(/\s/g, '');
            if (txtCaptchaRF.value == txtGenCaptchaRF) {
                $("#pMsgCaptchaNotMatch").css("display", "none");
                checkBoxTAndC.disabled = false;
                $("#pImgRefreshCaptcha").css("display", "none");
                $("#pImgRefreshCaptchaV2").css("display", "block");
            } else {
              
                $("#pMsgCaptchaNotMatch").css("display", "block");
                checkBoxTAndC.checked = false;
                checkBoxTAndC.disabled = true;
                btnSubmitRegisterForm.disabled = true;
            }
        } else {
            $("#pMsgCaptchaNotMatch").css("display", "none");
            checkBoxTAndC.checked = false;
            checkBoxTAndC.disabled = true;
            btnSubmitRegisterForm.disabled = true;
        }
    });

    //on check event of terms and conditions check box
    $("#chkBoxTAndCForm").click(function () {
        if (checkBoxTAndC.checked) {
            txtCaptchaRF.disabled = true;
            btnSubmitRegisterForm.disabled = false;
        }
        else {
            btnSubmitRegisterForm.disabled = true;
        }
    });
});


$("#btnSubRegisterForm").click(function () {
    document.getElementById("txtBoxMobileNoReg").disabled = false;
    document.getElementById("txtBoxPanNoForm").disabled = false;
    document.getElementById("ddlConstitutionType").disabled = false;
});