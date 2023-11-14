$(document).ready(function () {
    DrawCaptcha("Generatedcaptcha", 6);

    var txtEmpIdLogin = document.getElementById("txtEmpIdAdmin");
    var txtPasswordAdminLogin = document.getElementById("txtPasswordAdmin");
    var divMbOtpAdminLogin = document.getElementById("divOtpAdminLogin");
    var btnSubAdminLogin = document.getElementById("btnSubAdmin");
    var btnForgotPassword = document.getElementById("btnForgotPassword");
    var txtEntCaptchaAdmin = document.getElementById("txtEnterCaptcha");
    var txtOtpEnterMbAdmin = document.getElementById("txtOtpEnterAdmin");
    var btnVerOtpMbAdmin = document.getElementById("btnVerOtpAdmin");
    var inpDscFileEmp = document.getElementById("btnDscVerify");
    var hidIsDscRequired = document.getElementById("hidIsDscRequired");

    $("#btnSubAdmin").click(function () {
        document.getElementById("txtEmpIdAdmin").disabled = false;
        document.getElementById("txtPasswordAdmin").disabled = false;
    });
    //Hiding modal popup alert
    $('#btnHideModal').click(function () {
        $('#modalAlertAdmin').modal('hide');
    });
    txtEmpIdLogin.value = "";
    txtEmpIdLogin.focus();
    $("#txtEmpIdAdmin").bind('copy paste', function (e) {
        e.preventDefault();
    });

    // Lets user to enter only numbers
    //$("#txtEmpIdAdmin").on('keypress', function (event) {
    //    var regex = new RegExp("/^([0-9A-Za-z])+$/^[0-9]+$");
    //    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    //    if (!regex.test(key)) {
    //        event.preventDefault();
    //        return false;
    //    }
    //});


    $("#txtEmpIdAdmin").keyup(function () {
        //if (txtEmpIdLogin.value != "") {
        var empIdRegex = /^([0-9]{8})$/;

        if (empIdRegex.test(txtEmpIdLogin.value)) {
            txtPasswordAdminLogin.disabled = false;
        }
        else {
            txtPasswordAdminLogin.disabled = true;
            txtEntCaptchaAdmin.disabled = true;
            btnSubAdminLogin.disabled = true;
        }
    });
    //$("#txtPasswordAdmin").bind('copy paste', function (e) {
    //    e.preventDefault();
    //});
    $("#txtPasswordAdmin").keyup(function () {
        // var passwordRegex = /^([a-zA-z0-9]{8})$/;
        if (txtPasswordAdminLogin.value.length >= 8) {
        if (hidIsDscRequired.value == "True") {
            inpDscFileEmp.disabled = false;
            txtEmpIdLogin.disabled = true;
        } else {
            txtEntCaptchaAdmin.disabled = false;
            txtEmpIdLogin.disabled = true;
        }
    }
        else {// if (txtPasswordAdminLogin.value == "") {
        // txtEmpIdLogin.disabled = true;
        txtEntCaptchaAdmin.disabled = true;
        btnSubAdminLogin.disabled = true;
        if (hidIsDscRequired.value == "True") {
            inpDscFileEmp.disabled = true;
        }
    }
});



    $("#btnDscVerify").click(function () {
    // function SignInWithDSCLogin() {
    if (!isBrowserSupportsExtension() || !isExtensionInstalled()) {
        alert("Please Download Chrome Extension.");
        return;
    }
    ////Call method from Extension DSCSignRegSignedExtension to get Selected Certificate Subject and SerialNumber
    // var authToken = "EmpId";//$("#um_user_name").val();
        DSCSignRegSignedExtension.getSelectedCertificate()
            .then(
                function (CertInfo) {
                    var jobj = JSON.parse(CertInfo);
                    var SelCertSubj = jobj.SelCertSubject.split(',');
                    if (SelCertSubj[1] != undefined) {
                        alert(SelCertSubj[0].split('=')[1]);
                        $("#dscValidationMessage").html("DSC validation successful.");
                        $("#dscErrorMessage").html("");
                        //$("#dscValidationMessage").className("text-success");
                        //$("#DSCName").val(SelCertSubj[0].split('=')[1]);
                        //$("#SerialNo").val(SelCertSubj[1].split('=')[1]);
                        //$("#Place").val(SelCertSubj[2].split('=')[1]);
                        //$("#PhoneNumber").val(SelCertSubj[4].split('=')[1]);
                        //$("#ExpiryDate").val(jobj.ExpDate);
                        $("#inpEmpPublicKey").val(jobj.PublicKey);
                        //$("#ValidFromDate").val(jobj.ValidFrom);
                        //$("#issuerEmail").val(jobj.eMail);
                        //$("#certifyingAuthority").val(jobj.issuerName.Name.split(',')[2].split('=')[1]);
                        //$("#createuser").show();
                        //$("#DSCUpdation").hide();
                        document.getElementById("txtEnterCaptcha").disabled = false;
                    }
                    else {
                        alert("Please insert valid DSC Key");
                        $("#dscErrorMessage").html("DSC validation unsuccessful.");
                        $("#dscValidationMessage").html("");
                        document.getElementById("txtEnterCaptcha").disabled = true;
                        //$("#DSCName").val('');
                        //$("#SerialNo").val('');
                        //$("#Place").val('');
                        //$("#PhoneNumber").val('');
                        //$("#ExpiryDate").val('');
                        //$("#PublicKey").val('');
                        //$("#ValidFromDate").val('');
                        //$("#issuerEmail").val('');
                        //$("#certifyingAuthority").val('');
                        //$("#createuser").hide();
                        //$("#DSCUpdation").show();
                    }
                },
                function (errmsg) {
                    document.getElementById("txtEnterCaptcha").disabled = true;
                    $("#dscErrorMessage").html("DSC validation unsuccessful.");
                    $("#dscValidationMessage").html("");
                    //alert(errmsg.message);
                    //$("#ResultDisplay").html(errmsg.message);
                }
            );
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

$("#txtEnterCaptcha").keyup(function () {//txtEnterCaptcha
    var alphaNumRegex = /^([0-9A-Za-z])+$/;
    var captchaRegex = /^([0-9A-Za-z]{6})$/;
    var txtGenCaptchaLogin = document.getElementById("Generatedcaptcha").value.replace(/\s/g, '');

    if (txtEntCaptchaAdmin.value == txtGenCaptchaLogin) {
        $("#pImgRefreshCaptcha").css("display", "none");
        $("#pImgRefreshCaptchaV2").css("display", "block");
        btnSubAdminLogin.disabled = true;
        $("#pMsgCaptchaNotMatch").css("display", "none");
        if (hidIsDscRequired.value == "True") {
            inpDscFileEmp.disabled = true;
        }
        txtEmpIdLogin.disabled = true;
        txtPasswordAdminLogin.disabled = true;
        txtEntCaptchaAdmin.disabled = true;
        btnSubAdminLogin.disabled = false;
        // }
    } else {
        $("#pMsgCaptchaNotMatch").css("display", "block");
        btnSubAdminLogin.disabled = true;
        // btnForgotPassword.disabled = true;
    }

});

//On click of forgot password button
//  $("#btnForgotPassword").click(function () {
// navigate to forgot password page with Employee Id disabled.
//     window.location.href = "ForgotPassword?empId=" + txtEmpIdLogin.value;
//  });

//on click of submit button
$("#btnSubAdmin").click(function () {

    //if (hidIsDscRequired.value == "True" && inpDscFileEmp.value == "") {
    //    document.getElementById("pModalAlertAdmin").textContent = "Tried logging outside KSWAN, This User needs DSC Authentication, Click OK to continue";
    //    document.getElementById("divModelAlertAdmin").className = "modal-body";
    //    $("#modalAlertAdmin").modal('show');
    //    document.getElementById("pErrDscFile").textContent = "Please upload DSC Certificate.";
    //    document.getElementById("pErrDscFile").className = "text-danger";
    //    //$("#pErrDscFile").css("color", "#ff0000");
    //    return false;
    //} else {
    //    return true;
    //}
    //if (hidIsDscRequired.value == true) {
    //    alert(inpDscFileEmp.value);
    //    if (inpDscFileEmp.value == "") {
    //        alert("a");
    //        document.getElementById("pModalAlertAdmin").textContent = "Tried logging outside KSWAN, This User needs DSC Authentication, Click OK to continue";
    //        document.getElementById("divModelAlertAdmin").className = "modal-body alert-danger";
    //        $("#modalAlertAdmin").modal('show');
    //        // alert("Tried logging outside KSWAN, This User needs DSC Authentication, Click OK to continue");
    //        // $("#divDsc").css("display", "block");
    //        inpDscFileEmp.focus();
    //        return false;
    //    } else {
    //        return true;
    //    }
    //}
});



});