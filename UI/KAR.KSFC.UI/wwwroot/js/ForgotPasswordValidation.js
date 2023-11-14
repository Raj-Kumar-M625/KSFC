$(document).ready(function () {

    $.ajaxSetup({

        beforeSend: function () {
            $(".modalLoading").show();
        },
        complete: function () {
            $(".modalLoading").hide();
        }
    });

    var txtEmpIdFP = document.getElementById("txtEmpIdFP");
    var txtEnterOtpFP = document.getElementById("txtEnterOtpFP");//btnVerOtpFP
    var btnVerOtpFP = document.getElementById("btnVerOtpFP");//btnVerOtpFP
    var btnGenOtpForgot = document.getElementById("btnGenOtpForgot");
    var btnResOtpForgot = document.getElementById("btnResOtpForgot");
    var txtCaptchaFP = document.getElementById("txtEnterCaptcha");
    var btnSubForgot = document.getElementById("btnSubForgot");

    //Hiding modal popup alert
    $('#btnHideModal').click(function () {
        $('#modalAlertAdminFP').modal('hide');
    });

    //btnVerOtpAdmin
    txtEmpIdFP.focus();
    $("#txtEmpIdFP").bind('copy paste', function (e) {
        e.preventDefault();
    });

    // Lets user to enter only numbers
    $("#txtEmpIdFP").on('keypress', function (event) {
        var regex = new RegExp("^[0-9]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });


    $("#txtEmpIdFP").keyup(function () {
        var empIdRegex = /^([0-9]{8})$/;
  
        if (empIdRegex.test(txtEmpIdFP.value)) {
            btnGenOtpForgot.disabled = false;
            $("#txtEmpIdFP").css("border", "1px solid #00FF00");
        }
        else {
            $("#txtEmpIdFP").css("border", "1px solid #FF0000");
            btnGenOtpForgot.disabled = true;
        }
    });

    var resMessageId = "0";
    var resMessage = "";
    var mobNum = 0;
    var timeDefined = 0;
    var downloadTimer = 5;
    var otpExpTimeLeft = 0;

    $("#btnGenOtpForgot").click(function () {
        $.ajax({
            url: "/Account/GenerateOtpForAdminForgotPass",
            data: { empId: txtEmpIdFP.value },
            dataType: "json",
            type: "POST",
            success: function (data) {
                resMessageId = data.id;
                resMessage = data.message;
                mobNum = data.mobNum;
                otpExpTimeLeft = data.otpExpTimeLeftInSec;
            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            if (resMessageId == "0") {
                document.getElementById("pModalAlertAdminFP").textContent = resMessage;
                $("#modalAlertAdminFP").modal('show');
                $("#pMsgGenerateOtp").css("display", "block");
                document.getElementById("iconBtnGenOtp").className = "bi bi-x text-danger ";
                document.getElementById("pMsgGenerateOtp").textContent = resMessage;
                document.getElementById("pMsgGenerateOtp").className = "text-danger";
                txtEnterOtpFP.disabled = true;
            } else if (resMessageId == "1") {
                document.getElementById("spanCurrentMobileNoReg").textContent = mobNum;
                document.getElementById("pModalAlertAdminFP").textContent = resMessage;

                $("#modalAlertAdminFP").modal('show');
                document.getElementById("spanTimeCountdownTimer").textContent = otpExpTimeLeft;
                document.getElementById("iconBtnGenOtp").className = "bi bi-check ";
                btnGenOtpForgot.disabled = true;
                txtEnterOtpFP.disabled = false;
                txtEnterOtpFP.focus();
                $("#pMsgGenerateOtp").css("display", "block");
                document.getElementById("pMsgGenerateOtp").textContent = resMessage;
                document.getElementById("pMsgGenerateOtp").className = "text-success";
                if (otpExpTimeLeft > 0) {
                    timeDefined = otpExpTimeLeft;
                }
                CreateTimer();
            } else if (resMessageId == "2") {
                document.getElementById("pModalAlertAdminFP").textContent = resMessage;
                $("#modalAlertAdminFP").modal('show');
                $("#pMsgGenerateOtp").css("display", "block");
                document.getElementById("iconBtnGenOtp").className = "bi bi-x text-danger ";
                document.getElementById("pMsgGenerateOtp").textContent = resMessage;
                document.getElementById("pMsgGenerateOtp").className = "text-danger";
                $("#pMsgCountdownTimer").css("display", "none");
                txtEnterOtpFP.disabled = true;
                btnGenOtpForgot.disabled = true;
                btnResOtpForgot.disabled = true;
                txtEmpIdFP.disabled = true;
            }
        });
    });

    $("#btnResOtpForgot").click(function () {

        $.ajax({
            url: "/Account/ResendOtpForAdminForgotPass",
            data: { empId: txtEmpIdFP.value },
            dataType: "json",
            type: "POST",
            success: function (data) {
                resMessageId = data.id;
                resMessage = data.message;
                mobNum = data.mobNum;
                otpExpTimeLeft = data.otpExpTimeLeftInSec;
            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            if (resMessageId == "0") {
                document.getElementById("pModalAlertAdminFP").textContent = resMessage;
                $("#modalAlertAdminFP").modal('show');
                $("#pMsgGenerateOtp").css("display", "block");
                document.getElementById("iconBtnGenOtp").className = "bi bi-x text-danger ";
                document.getElementById("pMsgGenerateOtp").textContent = resMessage;
                document.getElementById("pMsgGenerateOtp").className = "text-danger";
                txtEnterOtpFP.disabled = true;
            } else if (resMessageId == "1") {
                document.getElementById("spanCurrentMobileNoReg").textContent = mobNum;
                document.getElementById("pModalAlertAdminFP").textContent = resMessage;
                $("#modalAlertAdminFP").modal('show');
                document.getElementById("spanTimeCountdownTimer").textContent = otpExpTimeLeft;//
                document.getElementById("iconBtnGenOtp").className = "bi bi-check ";
                btnGenOtpForgot.disabled = true;
                txtEnterOtpFP.disabled = false;
                txtEnterOtpFP.focus();
                $("#pMsgGenerateOtp").css("display", "block");
                document.getElementById("pMsgGenerateOtp").textContent = resMessage;
                document.getElementById("pMsgGenerateOtp").className = "text-success";
                if (otpExpTimeLeft > 0) {
                    timeDefined = otpExpTimeLeft;
                }
                CreateTimer();
            } else if (resMessageId == "2") {
                document.getElementById("pModalAlertAdminFP").textContent = resMessage;
                $("#modalAlertAdminFP").modal('show');
                $("#pMsgGenerateOtp").css("display", "block");
                document.getElementById("iconBtnGenOtp").className = "bi bi-x text-danger ";
                document.getElementById("pMsgGenerateOtp").textContent = resMessage;
                document.getElementById("pMsgGenerateOtp").className = "text-danger";
                $("#pMsgCountdownTimer").css("display", "none");
                txtEnterOtpFP.disabled = true;
                btnGenOtpForgot.disabled = true;
                btnResOtpForgot.disabled = true;
                txtEmpIdFP.disabled = true;
            }
        });
    });

    $("#txtEnterOtpFP").bind('copy paste', function (e) {
        e.preventDefault();
    });
    $("#txtEnterOtpFP").keyup(function () {
        //Removes characters other than numbers
        // document.getElementById("iconBtnValidateOtp").className = "";
        var mobileOtpRegex = /^([0-9]{6})$/;
        if (mobileOtpRegex.test(txtEnterOtpFP.value)) {
            $("#txtEnterOtpFP").css("border", "1px solid #00FF00");
            $("#btnVerOtpFP").css("display", "block");
            btnResOtpForgot.disabled = true;
            btnVerOtpFP.disabled = false;
            txtEnterOtpFP.disabled = false;
            txtEmpIdFP.disabled = true;
            //txtCaptchaFP.disabled = false;
            //txt.disabled = false;
        }
        else {
            $("#txtEnterOtpFP").css("border", "1px solid #FF0000");
           // txtCaptchaFP.disabled = true;

        }
    });

    var resMessageId = "0";
    var resMessage = "";
    $("#btnVerOtpFP").click(function () {
        var resMessageId = "";
        var resMessage = "";
        $.ajax({
            url: "/Account/VerifyOtpForgotPass",
            data: {
                entOtp: txtEnterOtpFP.value,
                empId: txtEmpIdFP.value,
            },
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

            if (resMessageId == "1") {
                document.getElementById("iconBtnVerOtpFP").className = "bi bi-check";
                $("#pOtpCountdownTimerMsg").css("display", "none");
                ClearTimer();
                btnGenOtpForgot.disabled = true;
                btnResOtpForgot.disabled = true;
                btnVerOtpFP.disabled = true;
                txtEnterOtpFP.disabled = true;
                txtCaptchaFP.disabled = false;
                txtCaptchaFP.focus();
                document.getElementById("pMsgEnterOtp").className = "text-success";
                document.getElementById("pMsgEnterOtp").textContent = resMessage;
                document.getElementById("pModalAlertAdminFP").textContent = resMessage;
                $('#pMsgCountdownTimer').hide();
                $('#pMsgGenerateOtp').hide();
                $("#modalAlertAdminFP").modal('show');

            } else if (resMessageId == "2") {
                $("#pOtpCountdownTimerMsg").css("display", "none");
                ClearTimer();
                txtEnterOtpFP.disabled = true;
                btnVerOtpFP.disabled = true;
                txtCaptchaFP.disabled = true;
                document.getElementById("pMsgEnterOtp").className = "text-success";
                document.getElementById("pMsgEnterOtp").textContent = resMessage;
                document.getElementById("pModalAlertAdminFP").textContent = resMessage;
                $('#pMsgCountdownTimer').hide();
                $('#pMsgGenerateOtp').hide();
                $("#modalAlertAdminFP").modal('show');
            } else {
                document.getElementById("iconBtnVerOtpFP").className = "bi bi-x text-danger";
                document.getElementById("pMsgEnterOtp").className = "text-danger";
                document.getElementById("pMsgEnterOtp").textContent = resMessage;
                document.getElementById("pModalAlertAdminFP").textContent = resMessage;
                $("#modalAlertAdminFP").modal('show');
                txtCaptchaFP.disabled = true;
            }
        });
    });

    function ClearTimer() {
        clearInterval(downloadTimer);
    }

    function CreateTimer() {
        document.getElementById("spanTimeCountdownTimer").textContent = timeDefined;
        var timeLeft = timeDefined;
        $("#pMsgCountdownTimer").css("display", "block");
        downloadTimer = setInterval(function () {
            timeLeft--;
            document.getElementById("spanTimeCountdownTimer").textContent = timeLeft;
            if (timeLeft <= 0) {
                clearInterval(downloadTimer);//call to clear timer, when Validate Otp button clicked by user
                $("#btnResOtpForgot").css("display", "block");
                $("#btnGenOtpForgot").css("display", "none");
                btnResOtpForgot.disabled = false;
                txtEnterOtpFP.disabled = false;
            }
        }, 1000);
    }

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
        if (captchaRegex.test(txtCaptchaFP.value)) {
            var txtGenCaptchaRF = document.getElementById("Generatedcaptcha").value.replace(/\s/g, '');
            if (txtCaptchaFP.value == txtGenCaptchaRF) {
                btnSubForgot.disabled = false;
                txtCaptchaFP.disabled = true;
                $("#pMsgCaptchaNotMatch").css("display", "none");
                $("#pImgRefreshCaptcha").css("display", "none");
                $("#pImgRefreshCaptchaV2").css("display", "block");
            } else {
                $("#pMsgCaptchaNotMatch").css("display", "block");
                btnSubForgot.disabled = true;
            }
        } else {
            //$("#pMsgCaptchaNotMatch").css("display", "none");
            //checkBoxTAndC.checked = false;
            //checkBoxTAndC.disabled = true;
            //btnSubmitRegisterForm.disabled = true;
        }
    });

    //On click of Verify button
    $("#btnSubForgot").click(function () {

        $.ajax({
            url: "/Account/ValidateUserSendPassword",
            data: { empId: txtEmpIdFP.value },
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
            if (resMessageId == "1") {
                document.getElementById("pModalAlertFPHome").textContent = resMessage;// "New Password sent to your registered Mobile Number. Click “OK” to open Login Page";
                $('#modalAlertFPHome').modal({ backdrop: 'static', keyboard: false })  
                $("#modalAlertFPHome").modal('show');
                btnSubForgot.disabled = true;
            } else {
                document.getElementById("divErrorMessage").textContent = "Employee Id not found.";
               // $("#modalAlertFPHome").modal('show');
            }});
    });


});