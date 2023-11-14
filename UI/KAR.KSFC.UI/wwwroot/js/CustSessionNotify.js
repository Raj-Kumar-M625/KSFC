$(document).ready(function () {
    SessionNotificationCust();
});

var sessionExpiryTime;
function SessionNotificationCust() {

    sessionExpiryTime = document.getElementById("hdnSessionTimeout").value;
    var sessionExpiryNoticeTimeInSec = document.getElementById("hdnSessionExpNotice").value;
    setInterval(function () {
        sessionExpiryTime--;

        if (sessionExpiryTime == 0) {
            //$("#modalAlert").modal('hide');
            // window.location.href = encodeURI("Customer/Home/Logout");
            $("#divCustSessionContinue").css("display", "none");
            $("#h5CustSessionConTitle").css("display", "none");
            $("#divCustSessionExpired").css("display", "block");
            $("#h5CustSessionExpTitle").css("display", "block");
        }
        if (sessionExpiryTime == sessionExpiryNoticeTimeInSec) {
            $("#modalAlert").modal('show');
        }
        if (sessionExpiryTime <= sessionExpiryNoticeTimeInSec) {
            document.getElementById("spanTimeCountdownTimer").textContent = sessionExpiryTime;
        }

    }, 1000);
}





$(document).ajaxStart(function (event, jqxhr, settings) {
    sessionExpiryTime = document.getElementById("hdnSessionTimeout").value;
});

//$("#btnHideSessionModalContinue").click(function () {
function btnHideSessionModalContinue() {
    $("#modalAlert").modal('hide');
    resMessage = false;
    $.ajax({
        url: "Customer/Home/SessionOnContinue",
        // data: { /*mobileNo: txtMobileNo.value*/ },
        dataType: "json",
        type: "GET",
        success: function (data) {
            SessionNotificationCust();
            //sessionExpiryTime = document.getElementById("hdnSessionTimeout").value;
            //document.getElementById("spanTimeCountdownTimer").textContent = 0;
        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () {
        // $("#modalAlert").modal('hide');
    });
}
//});
