$(document).ready(function () {

    DrawCaptcha("Generatedcaptcha", 6);

    $("#btnSubLogin").click(function () {
        document.getElementById("txtEnterOtpLogin").disabled = false;
        document.getElementById("txtPanLogin").disabled = false;
    });

    //Loading overlay gif for AJAX calls 
    $.ajaxSetup({
        beforeSend: function () {
            $(".modalCust").show();
        },
        complete: function () {
            $(".modalCust").hide();
        }
    });

    var btnGenOtpForPanNLogin = document.getElementById("btnGenOtpForPanLogin");
    var btnResOtpForPanNLogin = document.getElementById("btnResOtpForPanLogin");
    var txtPanNoLogin = document.getElementById("txtPanLogin");
    var txtEnterOtpNoLogin = document.getElementById("txtEnterOtpLogin");
    var btnVerOtpPanLogin = document.getElementById("btnVerOtpLogin");
    var txtCaptchaLogin = document.getElementById("txtEnterCaptcha");
    var btnSubLogin = document.getElementById("btnSubLogin");

    var numberRegex = new RegExp("^[0-9]+$");
    var otpRegex = /^([0-9]{6})$/;
    var panNoRegex = /^([A-Za-z]{3}[p|c|h|a|b|g|j|l|f|t|P|C|H|A|B|G|J|L|F|T]{1}[A-Za-z]{1}[0-9]{4}[A-Za-z]{1})$/;

    //Hiding modal popup alert
    $('#btnHideModalCust').click(function () {
        $('#modalAlertCust').modal('hide');
    });
    //Hiding modal popup 2 alert
    $('#btnHideModalHome').click(function () {
        $('#modalAlertHome').modal('hide');
    });
    txtPanNoLogin.focus();
    $("#txtPanLogin").bind('copy paste', function (e) {
        e.preventDefault();
    });

    // Lets user to enter only numbers
    $("#txtPanLogin").on('keypress', function (event) {
        var regex = new RegExp("^[0-9A-Za-z]+$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    $("#txtPanLogin").keyup(function () {
        document.getElementById("iconBtnGenOtpLogin").className = "";

        if (panNoRegex.test(txtPanNoLogin.value)) {
            $("#txtPanLogin").css("border", "1px solid #00FF00");
            btnGenOtpForPanNLogin.disabled = false;
            btnResOtpForPanNLogin.disabled = false;
        }
        else {
            $("#txtPanLogin").css("border", "1px solid #FF0000");
            btnGenOtpForPanNLogin.disabled = true;
            btnResOtpForPanNLogin.disabled = true;
        }
    });

    //Parameters for Generate OTP and Resend OTP
    var resMessageId = "0";
    var resMessage = "";
    var otpExpTimeLeft = 0;
    var timeDefined = 0;
    var downloadTimer = 5;
    var messageInJson = "";
    var mobileNumEnd = "";

    //On click of generate OTP button
    $("#btnGenOtpForPanLogin").click(function () {
        $.ajax({
            url: "/Account/GenerateOtpForCustLogin",
            data: { panNo: txtPanNoLogin.value },
            dataType: "json",
            type: "POST",
            success: function (data) {
                resMessageId = data.id;
                mobileNumEnd = data.mobNoEnd;
                resMessage = data.message;

                otpExpTimeLeft = data.otpExpTimeLeftInSec;
            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            if (resMessageId == "2") {

                document.getElementById("pModalAlertCust").textContent = resMessage;
                //document.getElementById("divModelAlertCust").className = "modal-body text-success";
                $("#modalAlertCust").modal('show');

                document.getElementById("iconBtnGenOtpLogin").className = "";
                $("#pOtpCountdownTimerMsg").css("display", "none");
                document.getElementById("pOtpCountdownTimerMsg").className = "text-success";
                //$("#pOtpCountdownTimerMsg").css("color", "#0000ff");
                document.getElementById("pMsgForPanLogin").textContent = resMessage;
                document.getElementById("pMsgForPanLogin").className = "text-success";
                // $("#pMsgForPanLogin").css("color", "#0000ff");
                txtPanNoLogin.disabled = true;
                btnGenOtpForPanNLogin.disabled = true;
                txtEnterOtpNoLogin.disabled = false;
                txtEnterOtpNoLogin.focus();

            } else if (resMessageId == "1") {
                $("#pMsgForPanLogin").css("display", "none");
                messageInJson = resMessage.replace('"', '').replace('"', '');
                document.getElementById("pModalAlertCust").textContent = messageInJson;
                //document.getElementById("divModelAlertCust").className = "modal-body text-success";
                $("#modalAlertCust").modal('show');
                // document.getElementById("hidMobileNo").textContent = mobileNumEnd;
                document.getElementById("spanOTPTimerMbEnding").textContent = mobileNumEnd;
                document.getElementById("iconBtnGenOtpLogin").className = "bi bi-check ";
                $("#pOtpCountdownTimerMsg").css("display", "block");
                document.getElementById("pOtpCountdownTimerMsg").className = "text-success";
                txtPanNoLogin.disabled = true;
                btnGenOtpForPanNLogin.disabled = true;
                txtEnterOtpNoLogin.disabled = false;
                txtEnterOtpNoLogin.focus();
                if (otpExpTimeLeft > 0) {
                    timeDefined = otpExpTimeLeft;
                }
                CreateTimer();
            } else if (resMessageId == "0") {
                messageInJson = resMessage.replace('"', '').replace('"', '');
                txtPanNoLogin.disabled = false;
                txtEnterOtpNoLogin.disabled = true;
                document.getElementById("iconBtnGenOtpLogin").className = "bi bi-x text-danger ";
                $("#pMsgForPanLogin").css("display", "block");
                document.getElementById("pMsgForPanLogin").textContent = messageInJson;
                document.getElementById("pMsgForPanLogin").className = "text-danger";
                //$("#pMsgForPanLogin").css("color", "#ff0000");
                document.getElementById("pModalAlertCust").textContent = messageInJson;
                //document.getElementById("divModelAlertCust").className = "modal-body text-danger";
                $("#modalAlertCust").modal('show');

                //window.location.href = ("~/Views/Shared/Error?message=" + resMessage);

            }

        });
    });

    //On click of Resend Otp Button
    $("#btnResOtpForPanLogin").click(function () {
        var resMessageId = "";
        var resMessage = "";
        var mobileNumEnd = "";

        $.ajax({
            url: "/Account/ResendOtpForCustLogin",
            data: { panNo: txtPanNoLogin.value },
            dataType: "json",
            type: "POST",
            success: function (data) {
                resMessageId = data.id;
                resMessage = data.message;
                mobileNumEnd = data.mobNoEnd;
            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            if (resMessageId == "2") {
                document.getElementById("pModalAlertCust").textContent = resMessage;
                //document.getElementById("divModelAlertCust").className = "modal-body text-success";
                $("#modalAlertCust").modal('show');


                $("#pOtpCountdownTimerMsg").css("display", "none");
                document.getElementById("iconBtnResOtpLogin").className = "";//
                $("#pMsgForPanLogin").css("display", "block");
                document.getElementById("pMsgForPanLogin").textContent = resMessage;
                //document.getElementById("pMsgForPanLogin").className = "text-success";
                // $("#pMsgForPanLogin").css("color", "#0000ff");
                txtPanNoLogin.disabled = true;
                btnResOtpForPanNLogin.disabled = true;
                txtEnterOtpNoLogin.disabled = false;
                txtEnterOtpNoLogin.focus();

            } else if (resMessageId == "1") {

                messageInJson = resMessage.replace('"', '').replace('"', '');
                document.getElementById("pModalAlertCust").textContent = messageInJson;
                //document.getElementById("divModelAlertCust").className = "modal-body text-success";
                $("#modalAlertCust").modal('show');

                // document.getElementById("spanOTPTimerMbEnding").textContent = mobileNumEnd;
                document.getElementById("iconBtnResOtpLogin").className = "bi bi-check ";
                $("#pMsgForPanLogin").css("display", "block");
                document.getElementById("pMsgForPanLogin").textContent = resMessage;
                document.getElementById("pMsgForPanLogin").className = "text-success";
                // $("#pMsgForPanLogin").css("color", "#00ff00");
                txtPanNoLogin.disabled = true;
                btnResOtpForPanNLogin.disabled = true;
                txtEnterOtpNoLogin.disabled = false;
                txtEnterOtpNoLogin.focus();
                if (otpExpTimeLeft > 0) {
                    timeDefined = otpExpTimeLeft;
                }
                CreateTimer();
            } else if (resMessageId == "0") {
                messageInJson = resMessage.replace('"', '').replace('"', '');
                txtPanNoLogin.disabled = false;
                txtEnterOtpNoLogin.disabled = true;
                document.getElementById("iconBtnResOtpLogin").className = "bi bi-x text-danger ";
                $("#pMsgForPanLogin").css("display", "block");
                document.getElementById("pMsgForPanLogin").textContent = messageInJson;
                document.getElementById("pMsgForPanLogin").className = "text-danger";
                //$("#pMsgForPanLogin").css("color", "#ff0000");
                document.getElementById("pModalAlertCust").textContent = messageInJson;
                //document.getElementById("divModelAlertCust").className = "modal-body text-danger";
                $("#modalAlertCust").modal('show');

            }

        });
    });



    //Create Timer for sending OTP--setting timeDefined before calling CreateTimer
    function CreateTimer() {
        document.getElementById("spanCountDownTimer").textContent = timeDefined;
        var timeLeft = timeDefined;
        $("#pOtpCountdownTimerMsg").css("display", "block");
        downloadTimer = setInterval(function () {
            timeLeft--;
            document.getElementById("spanCountDownTimer").textContent = timeLeft;
            if (timeLeft <= 0) {
                clearInterval(downloadTimer);//call to clear timer, when Validate Otp button clicked by user
                $("#btnResOtpForPanLogin").css("display", "block");
                $("#btnGenOtpForPanLogin").css("display", "none");
                btnGenOtpForPanNLogin.disabled = false;
                btnResOtpForPanNLogin.disabled = false;
            }
        }, 1000);
    }
    function ClearTimer() {
        clearInterval(downloadTimer);
    }


    $("#txtEnterOtpLogin").bind('copy paste', function (e) {
        e.preventDefault();
    });

    // Lets user to enter only numbers
    $("#txtEnterOtpLogin").on('keypress', function (event) {

        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!numberRegex.test(key)) {
            event.preventDefault();
            return false;
        }
    });

    $("#txtEnterOtpLogin").keyup(function () {
        if (otpRegex.test(txtEnterOtpNoLogin.value)) {
            ClearTimer();
            $("#pOtpCountdownTimerMsg").css("display", "none");
            $("#txtEnterOtpLogin").css("border", "1px solid #00FF00");
            document.getElementById("pMsgForPanLogin").textContent = "";
            btnVerOtpPanLogin.disabled = false;
            btnResOtpForPanNLogin.disabled = true;

            $("#btnVerOtpLogin").css("display", "block");
        }
        else {
            $("#txtEnterOtpLogin").css("border", "1px solid #FF0000");
            btnVerOtpPanLogin.disabled = true;

        }
    });

    //on click of validate otp button
    $("#btnVerOtpLogin").click(function () {
        var resMessageId = "";
        var resMessage = "";
        $.ajax({
            url: "/Account/VerifyOtp",
            data: {
                entOtp: txtEnterOtpNoLogin.value,
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
            //After complete loading data
            messageInJson = resMessage.replace('"', '').replace('"', '');
            $("#pMsgNoOfAttemptsForSMS").css("display", "none");
            document.getElementById("pMsgPanOtpVal").textContent = messageInJson;
            $("#pMsgPanOtpVal").css("display", "block");

            if (resMessageId == "1") {
                document.getElementById("iconBtnVerOtpLogin").className = "bi bi-check";
                $("#pOtpCountdownTimerMsg").css("display", "none");
                ClearTimer();
                btnGenOtpForPanNLogin.disabled = true;
                btnResOtpForPanNLogin.disabled = true;
                btnVerOtpPanLogin.disabled = true;
                txtEnterOtpNoLogin.disabled = true;
                txtCaptchaLogin.disabled = false;
                txtCaptchaLogin.focus();
                //$("#pMsgPanOtpVal").css("color", "#00ff00");
                document.getElementById("pMsgPanOtpVal").className = "text-success";
                document.getElementById("pModalAlertCust").textContent = messageInJson;
                document.getElementById("pMsgForPanLogin").textContent = "";
                //document.getElementById("divModelAlertCust").className = "modal-body text-success";
                $("#modalAlertCust").modal('show');

            } else if (resMessageId == "2") {
                $("#pOtpCountdownTimerMsg").css("display", "none");
                ClearTimer();
                txtEnterOtpNoLogin.disabled = true;
                btnVerOtpPanLogin.disabled = true;
                document.getElementById("pModalAlertPupHome").textContent = resMessage;
                $("#modalAlertHome").modal('show');
            } else {
                document.getElementById("iconBtnVerOtpLogin").className = "bi bi-x text-danger";
                messageInJson = resMessage.replace('"', '').replace('"', '');
                document.getElementById("pModalAlertCust").textContent = messageInJson;
                //$("#pMsgPanOtpVal").css("color", "#ff0000");
                //document.getElementById("divModelAlertCust").className = "modal-body text-danger";
                $("#modalAlertCust").modal('show');
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
        //Removes characters other than alphanumeric
        if (captchaRegex.test(txtCaptchaLogin.value)) {
            var txtGenCaptchaLogin = document.getElementById("Generatedcaptcha").value.replace(/\s/g, '');
            if (txtCaptchaLogin.value == txtGenCaptchaLogin) {
                $("#pMsgCaptchaNotMatch").css("display", "none");
                $("#pImgRefreshCaptcha").css("display", "none");
                $("#pImgRefreshCaptchaV2").css("display", "block");
                 
                btnSubLogin.disabled = false;
                txtCaptchaLogin.disabled = true;
            } else {
              
                $("#pMsgCaptchaNotMatch").css("display", "block");
                btnSubLogin.disabled = true;
            }
        } else {
            $("#pMsgCaptchaNotMatch").css("display", "none");
            btnSubLogin.disabled = true;
        }
    });
    
    $("#txtPanLogin").keyup(function () {
        $(this).val($(this).val().toUpperCase());
    });
   

});

