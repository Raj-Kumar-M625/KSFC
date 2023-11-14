$("#idGlassDiv").hide();
$('input#idConfirmYes').click(SaveConfirmed);
$('input#idConfirmNo').click(HideAlertArea);
$('input#idAlertClose').click(HideAlertArea);

HideAlertArea();

function MakeVirtualGlassWithId(idOfPopupDiv)
{
    $formContainer = $("div#divOverGlass");
    var x = $formContainer.position();

    var myPos = {};
    myPos.Top = x.top;
    myPos.Left = x.left;
    myPos.Height = $formContainer.height();
    myPos.Width = $formContainer.width();

    $("div#idGlassDiv").css({
        width: myPos.Width + 40,
        height: myPos.Height + 40,
        display: "block",
        top: myPos.Top,
        left: myPos.Left,
        position: 'fixed',
        opacity: '0.25',
        'background-color': '#e6e6e6',
        'border-radius': '20px'
    });

    $("div#" + idOfPopupDiv).css({
        //width: myPos.Width,
        display: "block",
        top: myPos.Top - 20,
        left: myPos.Left + 220,
        position: 'fixed',
        padding: '0px',
        opacity: '1',
        border: '1px solid darkblue',
        'background-color': '#ffffff',
        width: '300px'
    });
}

function HideAlertArea()
{
    $("div#idGlassDiv").css({
        display: "none"
    });

    $("div#idAlertArea").css({
        display: "none"
    });
}

function ShowAlertArea(title, msg, isErrorPop)
{
    MakeVirtualGlassWithId('idAlertArea');
    $("#idAlertAreaLabel").html(title);
    $("#idAlertMsg").html(msg);

    if (isErrorPop == true)
    {
        $("#idAlertClose").show();
        $("#idConfirmYes").hide();
        $("#idConfirmNo").hide();
    }
    else
    {
        $("#idAlertClose").hide();
        $("#idConfirmYes").show();
        $("#idConfirmNo").show();
    }
}

function SaveConfirmed() {
    HideAlertArea();
    if (typeof(Save) === typeof(Function)) {
        Save(); // container code should have this.
    }
}
