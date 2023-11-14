
var sessionExpiryTime;
function SessionNotification() {
    sessionExpiryTime = document.getElementById("hdnSessionTimeout").value;
    var sessionExpiryNoticeTimeInSec = document.getElementById("hdnSessionExpNotice").value;
    setInterval(function () {
        sessionExpiryTime--;

        if (sessionExpiryTime == 0) {
            $("#modalAlert").modal('hide');
            window.location.href = encodeURI("/Admin/Home/Logout");
            //Url.Action("Home","Logout");
            // Location.href = ("Home/Logout");
            //url: "Customer/Home/Logout";
        }
        if (sessionExpiryTime == sessionExpiryNoticeTimeInSec) {
            $("#modalAlert").modal('show');
        }
        if (sessionExpiryTime <= sessionExpiryNoticeTimeInSec) {
            document.getElementById("spanTimeCountdownTimer").textContent = sessionExpiryTime;
        }
    }, 1000);
}


$(document).ready(function () {
    SessionNotification();
});

$("#btnContinueSessionAdminNotify").click(function () {
    $("#modalAlert").modal('hide');
    resMessage = false;
    $.ajax({
        url: "Home/Index",
        // data: { /*mobileNo: txtMobileNo.value*/ },
        dataType: "json",
        type: "GET",
        success: function (data) {
            sessionExpiryTime = document.getElementById("hdnSessionTimeout").value;
            //document.getElementById("spanTimeCountdownTimer").textContent = 0;
        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () {
        // $("#modalAlert").modal('hide');
    });
});

$(document).ajaxStart(function (event, jqxhr, settings) {
    sessionExpiryTime = document.getElementById("hdnSessionTimeout").value;
});

var EnqStatus=
{
    Acknowledge: 1,
    InitiateScrutiny: 2,
    Approved: 3
}
