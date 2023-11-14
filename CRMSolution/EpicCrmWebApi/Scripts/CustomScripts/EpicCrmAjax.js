function showGlass() {
    $("div.glass").css({
        width: document.body.clientWidth,
        height: document.body.clientHeight,
        display: "block"
    });
}

function hideGlass() {
    $("div.glass").css({
        border: "none",
        width: 0,
        height: 0,
        display: "none"
    });
}

function showWaitMessage() {
    $formContainer = $("div#divOverGlass");
    //$formContainer.html('<p>Please wait...</p>');
    //var waitImage = '<img id="idWait" src="/Content/img/pleasewait.gif"/>';
    $formContainer.html(waitImage);
    $formContainer.css({
        display: "block"
    });
}

var cancelLinkClickOnGlassForm = function (event) {
    // hide the form
    HideGlassForm(event.data);

    // Aug 03 2018 - commented the reload of page - let user refresh the page.
    //location.reload(); // to refresh current page;

    // if callback function is provided, call it now.
    // June 16 2020
    // callback function is to refresh the data on parent page
    // If user is pressing Cancel on top right hand corner, then don't
    // call callback function.

    var idOfParentDiv = $(event.target).parent().parent().attr("id");
    if (idOfParentDiv != undefined && idOfParentDiv == "idCancelLinkContainer") {
        ;
    }
    else {
        if (callbackFunction != undefined) {
            callbackFunction();
        }
    }

    return false;
}

var HideGlassForm = function (eventData) {
    eventData.formContainer.css({ display: "none" });
    hideGlass();
}

var ShowFormOnGlass = function (data) {
    // find the div which will house the form data returned from ajax call
    $formContainer = $("div#divOverGlass");
    //alert(data);
    $formHtml = $(data); // make it Jquery object

    // attach event on cancel
    $formHtml.find("#popupCancelLink").click(
        {
            formContainer: $formContainer,
            form: $formHtml
        }, cancelLinkClickOnGlassForm);

    // attach an event to Save button
    $formHtml.find("input#popupSaveButton").click(
        {
            formContainer: $formContainer,
            form: $formHtml
        },
        saveButtonClickOnGlassForm
    );

    // attach calendar control properties
    if ($formHtml.find('input.justDate').length > 0) {
        $formHtml.find('input.justDate').datetimepicker({ inline: false, format: 'd-m-Y', timepicker: false, yearStart: 2017, yearEnd: 2099 });
        $formHtml.find('input.justDate').attr({ 'placeholder': 'dd-mm-yyyy' });
    }

    $formContainer.html($formHtml);
    // no need to show it again - as form on glass is already visible as part of 
    // wait message being shown.
    //$formContainer.css({ display:"block" });
}

// define function for save button callback when called from form on Glass
var saveButtonClickOnGlassForm = function (event) {
    var options = {
        url: event.data.form.attr('action'),
        type: 'POST',
        data: event.data.form.serialize()
    };

    // show wait message;
    //event.data.formContainer.html("<span>Please wait...</span>");

    //var waitImage = '<img id="idWait" src="/Content/img/pleasewait.gif"/>';
    event.data.formContainer.html(waitImage);


    // send call to server
    $.ajax(options).done(function (data)
    {
        $formHtml = $(data); // make it Jquery object
        event.data.formContainer.html($formHtml);
        $formHtml.find("#popupCancelLink").click({
            formContainer: event.data.formContainer,
            form: $formHtml
        }, cancelLinkClickOnGlassForm);

        // in case there was an error in input - the form is sent back - we have to assign event handler again;
        $formHtml.find("input#popupSaveButton").click(
            {
                formContainer: event.data.formContainer,
                form: $formHtml
            },
            saveButtonClickOnGlassForm
        );

        // attach calendar control properties again - in case form was sent back due to some error
        if ($formHtml.find('input.justDate').length > 0) {
            $formHtml.find('input.justDate').datetimepicker({ inline: false, format: 'd-m-Y', timepicker: false, yearStart: 2017, yearEnd: 2099 });
            $formHtml.find('input.justDate').attr({ 'placeholder': 'dd-mm-yyyy' });
        }

       return false;
    });

    return false;
}

var callbackFunction = undefined;

function SetCallbackFunction(f)
{
    callbackFunction = f;
}

// this is defining two function variables
var addPopupLinkClicked = 
    editPopupLinkClicked =
    function (event) {

        callbackFunction = undefined;

        var targetUrl = $(this).attr("href");

        //alert(targetUrl);
        showGlass();

        // show wait message
        showWaitMessage();

        //alert(targetUrl);
        var options = {
            url: targetUrl,
            type: "Get",
            data: {},
            cache: false
        };

        $.ajax(options).done(function (data)
        {
            //alert(data);
            ShowFormOnGlass(data);
        });

        // save callback function name - if provided in event
        if (event != undefined && event.data != undefined && event.data.callback != undefined) {
            callbackFunction = event.data.callback;
        }

        return false;
}

//var addPopupLinkClicked = function (event) {
//    var targetUrl = $(this).attr("href");

//    //alert(targetUrl);
//    showGlass();

//    // show wait message
//    showWaitMessage();

//    //alert(targetUrl);
//    var options = {
//        url: targetUrl,
//        type: "Get",
//        data: {},
//        cache: false
//    };

//    $.ajax(options).done(function (data) {
//        //alert(data.substring(1000));
//        ShowFormOnGlass(data);
//    });

//    // save callback function name - if provided in event
//    if (event != undefined && event.data != undefined) {
//        callbackFunction = event.data.callback;
//    }

//    return false;
//}

$(document).ready(function () {
    //SetupClickEvents();
});