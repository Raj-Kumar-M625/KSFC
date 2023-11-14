$(document).ready(function () {
    UpdateActiveDateTime();
    LoadEnquiryListDataTable();

});
function UpdateActiveDateTime() {
    var counter = 0;
    setInterval(function () {
        counter++;
        if (counter > 0) {
            //alert(new Date($.now()));//new Date($.now());
            var currentDateTime = new Date($.now());
            var loginDateTime = $("#spanUserLoginDateTime").val();//
            //var diffnew = (new Date(new Date("1970-01-01") + currentDateTime) - new Date(new Date("1970-01-01") + loginDateTime)) / 1000 / 60 / 60;
            var diff = Math.abs(new Date(currentDateTime) - new Date(loginDateTime));
            //alert(diffnew);
            //var dateh = new Date(diff);
            //alert(diff);
            var seconds = Math.floor(diff / 1000); //ignore any left over units smaller than a second
            var minutes = Math.floor(seconds / 60);
            seconds = seconds % 60;
            var hours = Math.floor(minutes / 60);
            minutes = minutes % 60;

            if (hours < 10) {
                hours = "0" + hours;
            }
            if (minutes < 10) {
                minutes = "0" + minutes;
            }
            if (seconds < 10) {
                seconds = "0" + seconds;
            }
            //alert("Diff = " + hours + ":" + minutes + ":" + seconds);
            $("#UserActiveTimeHr").html(hours + ":" + minutes + ":" + seconds);
            //$("#UserActiveTimeMin").html(counterMin);
        }
        //clearInterval(interval);
    }, 1000);
}

$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});
//document.onreadystatechange = function () {
//    debugger;
//    let state = document.readyState
//    if (state == 'complete') {
//        $('.loader').fadeOut();
//    }
//}

//Address Details CRUD operation starts here
function ClosePopupForm() {

    $('#form-modal-address').modal('hide');
}

function ClearPopupForm() {
    $('#form-modal-address .modal-body').html('');
    $('#form-modal-address .modal-title').html('');
    $('#form-modal-address').modal('hide');
}

function showInPopup(url, title) {
    debugger
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-address .modal-body').html(res);
            $('#form-modal-address .modal-title').html(title);
            $('#form-modal-address').modal('show');
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

function jQueryAjaxPost(form) {
    //var form1 = document.getElementById("formBDAddress");
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
    }
    else {
        // $('#form-modal .modal-body').html(form1.html);
        return false;
    }
    try {

        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {

                if (res.isValid) {
                    $('#view-all').html(res.html)
                    $('#form-modal-address .modal-body').html('');
                    $('#form-modal-address .modal-title').html('');
                    $('#form-modal-address').modal('hide');
                    ReloadAddressDetails();
                }
                else
                    $('#form-modal-address .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}


function jQueryAjaxDeleteEnquiry(form) {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-All-Enquiry').html(res.html);
                    LoadEnquiryListDataTable();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}

function jQueryAjaxDelete(form) {
    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all').html(res.html);
                    ReloadAddressDetails();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}

//Address Details CRUD operation ends here

//Registration Details CRUD operation starts here

function ClosePopupFormRD() {

    $('#form-modal-rd').modal('hide');
}

function ClearPopupFormRD() {
    $('#form-modal-rd .modal-body').html('');
    $('#form-modal-rd .modal-title').html('');
    $('#form-modal-rd').modal('hide');
}

function showInPopupRD(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-rd .modal-body').html(res);
            $('#form-modal-rd .modal-title').html(title);
            $('#form-modal-rd').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    });
}

function jQueryAjaxPostRD(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
    }
    else {
        // $('#form-modal .modal-body').html(form1.html);
        return false;
    }
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all-rd').html(res.html)
                    $('#form-modal-rd .modal-body').html('');
                    $('#form-modal-rd .modal-title').html('');
                    $('#form-modal-rd').modal('hide');
                    ReloadRegistration();
                }
                else
                    $('#form-modal-rd .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
function fxnDeleteRegistrationList(Id) {
    if (confirm('Are you sure to delete this record ?')) {
        var formData = new FormData();
        formData.append("EnqRegnoId", Id);
        try {
            $.ajax({
                type: 'POST',
                url: '/Customer/RegistrationDetails/Delete',
                data: formData,
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-rd').html(res.html);
                    ReloadRegistration();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    return false;
}
function fxnDeleteAddressList(Id) {
    if (confirm('Are you sure to delete this record ?')) {
        var formData = new FormData();
        formData.append("EnqAddresssId", Id);
        try {
            $.ajax({
                type: 'POST',
                url: '/Customer/Address/Delete',
                data: formData,
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all').html(res.html);
                    ReloadAddressDetails();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    return false;
}


function jQueryAjaxDeleteRD(form) {

    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-rd').html(res.html);
                    ReloadRegistration();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}




function ClosePopupFormPGPD() {

    $('#form-modal-pgpd').modal('hide');
}

function ClearPopupFormPGPD() {
    $('#form-modal-pgpd .modal-body').html('');
    $('#form-modal-pgpd .modal-title').html('');
    $('#form-modal-pgpd').modal('hide');
}

function showInPopupPGPD(url, title) {


    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {

            $('#form-modal-pgpd .modal-body').html(res);
            $('#form-modal-pgpd .modal-title').html(title);
            $('#form-modal-pgpd').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    });
}
function GetRoute(path) {
    var area = $('#userarea').val();
    var url = '';
    if (area === "Admin") {
        url = "/Admin" + path;
    } else {
        url = "/Customer" + path;
    }
    return url;
}
function jQueryAjaxPostPGPD(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        $.ajax({
            type: 'POST',
            url: GetRoute('/Promoter/CheckNonProprirtoryShareholding'),
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (!res.isValid) {
                    $('#EnqPromShare').css('border-color', 'red')
                    alert('Error! Share holding percentage should not be greater than 100.');
                    return false;
                }
                else {
                    try {
                        $.ajax({
                            type: 'POST',
                            url: form.action,
                            data: new FormData(form),
                            contentType: false,
                            processData: false,
                            success: function (res) {
                                if (res.isValid) {
                                    $('#view-all-pgpd').html(res.html)
                                    $('#form-modal-pgpd .modal-body').html('');
                                    $('#form-modal-pgpd .modal-title').html('');
                                    $('#form-modal-pgpd').modal('hide');
                                    ReloadPromoter();
                                    ReloadPromoterAsset();
                                    ReloadPromoterLiability();
                                    ReloadPromoterNetWorth();
                                }
                                else {
                                    //$('#form-modal-pgpd .modal-body').html(res.html);
                                    //$("#divEnquiryAlertPopup").html('Error! Share holding percentage should be greater than 100.');
                                    //$("#modalAlertEnq").modal('show');
                                    //document.getElementById("divEnquiryAlertPopup").className = "text-danger";
                                }

                            },
                            error: function (err) {
                                console.log(err)
                            }
                        })
                        //to prevent default form submit event
                        return false;
                    } catch (ex) {
                        console.log(ex)
                    }
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        return false;
    }
    else {
        return false;
    }
}

function jQueryAjaxDeletePGPD(form) {

    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-pgpd').html(res.html);
                    ReloadPromoter();
                    ReloadPromoterAsset();
                    ReloadPromoterLiability();
                    ReloadPromoterNetWorth();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}

function

    PromoterPhoto() {
    var files = $('#promoterPic').prop("files");
    formData = new FormData();
    formData.append("promoterPic", files[0]);
    if (files[0].size > 2097)//2mb=2097152
    {
        alert("bigger size");
        //$('#form-modal-pgpa .modal-body').html(res.html);
    }

}

//Promoter All Details CRUD operation ends here

//Guarantor All Details CRUD operation starts here

function ClosePopupFormPGGD() {

    $('#form-modal-pggd').modal('hide');
}

function ClearPopupFormPGGD() {
    $('#form-modal-pggd .modal-body').html('');
    $('#form-modal-pggd .modal-title').html('');
    $('#form-modal-pggd').modal('hide');
}

function showInPopupPGGD(url, title, isCheck) {
    //if (!isCheck) {
    //    $.ajax({
    //        type: 'GET',
    //        url: '/Customer/Guarantor/CheckGuarantorExit',
    //        success: function (res) {
    //            if (!res.isValid) {
    //                $("#divEnquiryAlertPopup").html(res.message);
    //                $("#modalAlertEnq").modal('show');
    //            }
    //            else {
    //JqueryAjaxPostGuarantorDetail(url, title)
    //            }
    //        }
    //    });
    //}
    //else {
    JqueryAjaxPostGuarantorDetail(url, title)
    // }


}
function JqueryAjaxPostGuarantorDetail(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-pggd .modal-body').html(res);
            $('#form-modal-pggd .modal-title').html(title);
            $('#form-modal-pggd').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    });
}

function jQueryAjaxPostPGGD(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
    }
    else {
        // $('#form-modal .modal-body').html(form1.html);
        return false;
    }
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all-pggd').html(res.html)
                    $('#form-modal-pggd .modal-body').html('');
                    $('#form-modal-pggd .modal-title').html('');
                    $('#form-modal-pggd').modal('hide');
                    ReloadGuarantor();
                    ReloadGuarantorAsset();
                    ReloadGuaratorLiability();
                    ReloadGuaratorNetWorth();
                }
                else
                    $('#form-modal-pggd .modal-body').html(res.html);
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function jQueryAjaxDeletePGGD(form) {

    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-pggd').html(res.html);
                    ReloadGuarantor();
                    ReloadGuarantorAsset();
                    ReloadGuaratorLiability();
                    ReloadGuaratorNetWorth();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }

    //prevent default form submit event
    return false;
}

//Guarantor All Details CRUD operation ends here


//Associate Sister Concern Details CRUD operation starts here

function ClosePopupFormASD() {
    $('#form-modal-asd').modal('hide');
}

function ClearPopupFormASD() {
    $('#form-modal-asd .modal-body').html('');
    $('#form-modal-asd .modal-title').html('');
    $('#form-modal-asd').modal('hide');
}

function showInPopupASD(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-asd .modal-body').html(res);
            $('#form-modal-asd .modal-title').html(title);
            $('#form-modal-asd').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    });
}

function jQueryAjaxPostASD(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
    }
    else {
        // $('#form-modal .modal-body').html(form1.html);
        return false;
    }
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all-asd').html(res.html)
                    $('#form-modal-asd .modal-body').html('');
                    $('#form-modal-asd .modal-title').html('');
                    $('#form-modal-asd').modal('hide');
                    ReloadAssoSisterCD();
                    ReloadAssoSisterFY();
                }
                else {
                    $('#form-modal-asd .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function jQueryAjaxDeleteASD(form) {

    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-asd').html(res.html);
                    ReloadAssoSisterCD();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    //prevent default form submit event
    return false;
}

//Associate Sister Concern Details CRUD operation ends here

//Associate Sister Concern Financial Details CRUD operation starts here

function ClosePopupFormASFD() {
    $('#form-modal-asfd').modal('hide');
}

function ClearPopupFormASFD() {
    $('#form-modal-asfd .modal-body').html('');
    $('#form-modal-asfd .modal-title').html('');
    $('#form-modal-asfd').modal('hide');
}

function showInPopupASFD(url, title) {

    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-asfd .modal-body').html(res);
            $('#form-modal-asfd .modal-title').html(title);
            $('#form-modal-asfd').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    });
}

function jQueryAjaxPostASFD(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
    }
    else {
        // $('#form-modal .modal-body').html(form1.html);
        return false;
    }
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all-asd').html(res.html)
                    $('#form-modal-asfd .modal-body').html('');
                    $('#form-modal-asfd .modal-title').html('');
                    $('#form-modal-asfd').modal('hide');
                    ReloadAssoSisterFY();
                }
                else {
                    $('#form-modal-asfd .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function jQueryAjaxDeleteASFD(form) {

    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-asfd').html(res.html);
                    ReloadAssoSisterFY();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    //prevent default form submit event
    return false;
}

//Associate Sister Concern Financial Details CRUD operation ends here

//Project Details Project Cost CRUD operation starts here

function ClosePopupFormPDPC() {
    $('#form-modal-pdpc').modal('hide');
}

function ClearPopupFormPDPC() {
    $('#form-modal-pdpc .modal-body').html('');
    $('#form-modal-pdpc .modal-title').html('');
    $('#form-modal-pdpc').modal('hide');
}

function showInPopupPDPC(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-pdpc .modal-body').html(res);
            $('#form-modal-pdpc .modal-title').html(title);
            $('#form-modal-pdpc').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    });
}

function jQueryAjaxPostPDPC(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
    }
    else {
        // $('#form-modal .modal-body').html(form1.html);
        return false;
    }
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all-pdpc').html(res.html)
                    $('#form-modal-pdpc .modal-body').html('');
                    $('#form-modal-pdpc .modal-title').html('');
                    $('#form-modal-pdpc').modal('hide');
                    ReloadProjectCost();
                }
                else {
                    $('#form-modal-pdpc .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function jQueryAjaxDeletePDPC(form) {

    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-pdpc').html(res.html);
                    ReloadProjectCost();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    //prevent default form submit event
    return false;
}

//Project Details Project Cost CRUD operation ends here

//Project Details Means Of Finance CRUD operation starts here

function ClosePopupFormPDMF() {
    $('#form-modal-pdmf').modal('hide');
}

function ClearPopupFormPDMF() {
    $('#form-modal-pdmf .modal-body').html('');
    $('#form-modal-pdmf .modal-title').html('');
    $('#form-modal-pdmf').modal('hide');
}

function showInPopupPDMF(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-pdmf .modal-body').html(res);
            $('#form-modal-pdmf .modal-title').html(title);
            $('#form-modal-pdmf').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    });
}

function jQueryAjaxPostPDMF(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
    }
    else {
        // $('#form-modal .modal-body').html(form1.html);
        return false;
    }
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all-pdmf').html(res.html)
                    $('#form-modal-pdmf .modal-body').html('');
                    $('#form-modal-pdmf .modal-title').html('');
                    $('#form-modal-pdmf').modal('hide');
                    ReloadProjectMOF();
                }
                else {
                    $('#form-modal-pdmf .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function jQueryAjaxDeletePDMF(form) {

    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-pdmf').html(res.html);
                    ReloadProjectMOF();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    //prevent default form submit event
    return false;
}

//Project Details Means Of Finance CRUD operation ends here

//Project Details Previous FY details CRUD operation starts here

function ClosePopupFormPDPY() {
    $('#form-modal-pdpy').modal('hide');
}

function ClearPopupFormPDPY() {
    $('#form-modal-pdpy .modal-body').html('');
    $('#form-modal-pdpy .modal-title').html('');
    $('#form-modal-pdpy').modal('hide');
}

function showInPopupPDPY(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-pdpy .modal-body').html(res);
            $('#form-modal-pdpy .modal-title').html(title);
            $('#form-modal-pdpy').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    });
}

function jQueryAjaxPostPDPY(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
    }
    else {
        // $('#form-modal .modal-body').html(form1.html);
        return false;
    }
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all-pdpy').html(res.html)
                    $('#form-modal-pdpy .modal-body').html('');
                    $('#form-modal-pdpy .modal-title').html('');
                    $('#form-modal-pdpy').modal('hide');
                    ReloadProjectPFY();
                }
                else {
                    $('#form-modal-pdpy .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function jQueryAjaxDeletePDPY(form) {

    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-pdpy').html(res.html);
                    ReloadProjectPFY();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    //prevent default form submit event
    return false;
}

//Project Details Previous FY details CRUD operation ends here

//Security and Document Details of Security CRUD operation starts here

function ClosePopupFormSDDS() {
    $('#form-modal-sdds').modal('hide');
}

function ClearPopupFormSDDS() {
    $('#form-modal-sdds .modal-body').html('');
    $('#form-modal-sdds .modal-title').html('');
    $('#form-modal-sdds').modal('hide');
}

function showInPopupSDDS(url, title) {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal-sdds .modal-body').html(res);
            $('#form-modal-sdds .modal-title').html(title);
            $('#form-modal-sdds').modal('show');
            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    });
}

function jQueryAjaxPostSDDS(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
    }
    else {
        // $('#form-modal .modal-body').html(form1.html);
        return false;
    }
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid) {
                    $('#view-all-sdds').html(res.html)
                    $('#form-modal-sdds .modal-body').html('');
                    $('#form-modal-sdds .modal-title').html('');
                    $('#form-modal-sdds').modal('hide');
                    ReloadDetailsOfSecurity();
                }
                else {
                    $('#form-modal-sdds .modal-body').html(res.html);
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function jQueryAjaxDeleteSDDS(form) {

    if (confirm('Are you sure to delete this record ?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $('#view-all-sdds').html(res.html);
                    ReloadDetailsOfSecurity();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
    }
    //prevent default form submit event
    return false;
}

//Security and Document Details of Security CRUD operation ends here




function ReloadAddressDetails() {
    var dataTableAD;
    dataTableAD = $('#tblAddDetailsDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadpayementDetails() {
    var dataTableAD;
    dataTableAD = $('#tblAddPayments').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}



function ReloadRegistration() {
    var dataTableRD;
    dataTableRD = $('#tblRegDetailsDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadPromoter() {
    dataTablePD = $('#tblProDetailsDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadPromoterAsset() {
    dataTablePA = $('#tblProAssetDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadPromoterLiability() {
    var dataTablePL;
    dataTablePL = $('#tblProLiabilityDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadPromoterNetWorth() {
    var dataTablePN;
    dataTablePN = $('#tblProNetWorthDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadGuarantor() {
    var dataTableGD;
    dataTableGD = $('#tblGuaDetailsDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadGuarantorAsset() {
    var dataTableGA;
    dataTableGA = $('#tblGuaAssetDatatable').DataTable({

        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadGuaratorLiability() {

    dataTableGL = $('#tblGuaLiabilityDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadGuaratorNetWorth() {
    var dataTableGN;
    dataTableGN = $('#tblGuaNetWorthDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadAssoSisterCD() {
    var dataTableASCD;
    dataTableASCD = $('#tblSisterASCDDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadAssoSisterFY() {
    var dataTableASFY;
    dataTableASFY = $('#tblSisterASFYDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadProjectCost() {
    var dataTableAD;
    dataTableAD = $('#tblPCDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadProjectMOF() {
    dataTableAD = $('#tblPMOFDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadProjectPFY() {
    dataTableAD = $('#tblPPFYDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadDetailsOfSecurity() {
    dataTableAD = $('#tblDetailsOfSecDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

//Adding DataTable to Enquiry List Table
function LoadEnquiryListDataTable() {
    var dataTableEL;
    dataTableEL = $('#tblEnquiryListDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New Enquiry</b> Button"
        }
    });
}

// Adding Datatable to all Tables Ends here
function printSummary() {

    $('.accordion-collapse').removeClass('collapse');

    var printContents = document.getElementById('ViewSummary').innerHTML;
    var originalContents = document.body.innerHTML;

    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
}

function viewDocument(uniqueId) {
    var params = { fileId: uniqueId };
    if (params) {
        $.ajax({
            "async": true,
            "crossDomain": true,
            "url": "/Customer/Documents/ViewUploadFile",
            "method": "Post",
            "data": params,
            success: function (data) {
                if (data.result) {
                    var windo = window.open("", "");
                    var objbuilder = '';
                    objbuilder += ('<title>Document Viewer</title><embed width=\'100%\' height=\'100%\'  src="data:application/pdf;base64,');
                    objbuilder += (data.result);
                    objbuilder += ('" type="application/pdf"/>');
                    windo.document.write(objbuilder);
                } else {
                    alert('Please try again later!');
                }
            }
        })
    }
}

function ReloadGuarantorDetails() {
    
    var dataTableAD;
    dataTableAD = $('#tblGuarantorDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}

function ReloadHypothecationDetails() {
    var dataTableAD;
    dataTableAD = $('#tblHypothecationDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadHypothecationDetails() {
    var dataTableAD;
    dataTableAD = $('#tblHypothecationViewDeed').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}
function ReloadSecurityChargeDetails() {
  
    $('#tblSecurityCharge').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadCersaiDetails() {
    // 
    var dataTableAD;
    dataTableAD = $('#tblCersaiDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

// Author: Dev; Module: Condition: Date: 05/08/2022
function ReloadConditionDetails() {
    // 
    var dataTableAD;
    dataTableAD = $('#tblConditionDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}


function ClearPopupFormsh() {
    $('#modelSecurityChargeDetails .modal-body').html('');
    $('#modelSecurityChargeDetails .modal-title').html('');
    $('#modelSecurityChargeDetails').modal('hide');

    $('#primarySecurityDetails .modal-body').html('');
    $('#primarySecurityDetails .modal-title').html('');
    $('#primarySecurityDetails').modal('hide');

    $('#colletralSecurityDetails .modal-body').html('');
    $('#colletralSecurityDetails .modal-title').html('');
    $('#colletralSecurityDetails').modal('hide');

    $('#modelGuarantorDetails .modal-body').html('');
    $('#modelGuarantorDetails .modal-title').html('');
    $('#modelGuarantorDetails').modal('hide');

    $('#modelConditionDetails .modal-body').html('');
    $('#modelConditionDetails .modal-title').html('');
    $('#modelConditionDetails').modal('hide');

    $('#modelCERSAIDetails .modal-body').html(res);
    $('#modelCERSAIDetails .modal-title').html(title);
    $('#modelCERSAIDetails').modal('show');

    $('#modelDisbursementConditionDetails .modal-body').html(res);
    $('#modelDisbursementConditionDetails .modal-title').html(title);
    $('#modelDisbursementConditionDetails').modal('show');

    $('#modelForm8AndForm13Details .modal-body').html(res);
    $('#modelForm8AndForm13Details .modal-title').html(title);
    $('#modelForm8AndForm13Details').modal('show');

    $('#modelAdditionalConditionDetails .modal-body').html(res);
    $('#modelAdditionalConditionDetails .modal-title').html(title);
    $('#modelAdditionalConditionDetails').modal('show');

    $('#modelInspectionDetail .modal-body').html(res);
    $('#modelInspectionDetail .modal-title').html(title);
    $('#modelInspectionDetail').modal('show');

    $('#modelInspectionDetail .modal-body').html(res);
    $('#modelInspectionDetail .modal-title').html(title);
    $('#modelInspectionDetail').modal('show');


}
function ClearPopupFormcr() {

    $('#modelCERSAIDetails .modal-body').html('');
    $('#modelCERSAIDetails .modal-title').html('');
    $('#modelCERSAIDetails').modal('hide');

}



// Author: manoj; Module: hypothecation; Date: 009/08/2022
function ReloadhypothCreateHolderDetails() {
    var dataTableAD;
    dataTableAD = $('#tblhypothCreateDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}
// Author: manoj; Module: Hypothecation: Date: 09/08/2022
function jQueryAjaxPosCreateHypothetsh(form) {
    debugger;
    var grid = document.getElementById("Table1");
    var checkBoxes = grid.getElementsByTagName("INPUT");
    var IsSelected = document.getElementById('#select');
    const message = [];
    //Loop through the CheckBoxes.
    for (i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].checked) {
            var row = checkBoxes[i].parentNode.parentNode;
            var Ids = row.cells[3].innerHTML;
            var strID = parseInt(Ids);
            message.push(strID);
        }
    }
    if (message.length != 0) {

        var formData = new FormData(form);
        Id = message;
        var IsSelected = document.getElementById('#select')
        for (i = 0; i < Id.length; i++) {
            formData.append("id", Id[i]);
        }
        try {
            $.ajax({
                type: 'POST',
                url: '/Admin/Hypothecation/Create',
                data: formData,
                contentType: false,
                processData: false,
                success: function (res) {

                    $('#view-all-hypothecation').html(res.html);
                    $('#modelHypothecationDetails .modal-body').html('');
                    $('#modelHypothecationDetails .modal-title').html('');
                    $('#modelHypothecationDetails').modal('hide');
                    ReloadHypothecationDetails();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
        //to prevent default form submit event
        return false;

    }
    else {
        swal.fire(
            'Warning!',
            'Please select atleast one Assest',
            'warning'
        )

        return false;
    }
}

//function Calculate() {
//    debugger
//    var grid = document.getElementById("Table1");
//    //var checkBoxes = grid.getElementsByTagName("INPUT");
//    //var IsSelected = document.getElementById('#select');
//    var TotalValue = 0;
//    //Loop through the CheckBoxes.
//    for (i = 0; i < checkBoxes.length; i++) {
//        if (checkBoxes[i].checked) {
//            var row = checkBoxes[i].parentNode.parentNode;
//            var Value = Number(row.cells[5].innerHTML.replace(/,/g,''));
//    TotalValue += Value;
//    $("#TotValue").val(TotalValue);
//    //Loop through the CheckBoxes.
//    //for (i = 0; i < checkBoxes.length; i++) {
//    //    if (checkBoxes[i].checked) {
//    //        var row = checkBoxes[i].parentNode.parentNode;
            
//    //    }
//    //}
   
//}

function jQueryAjaxPosCreateCersai(form) {
    debugger;
    var grid = document.getElementById("Table1");
    var checkBoxes = grid.getElementsByTagName("INPUT");
    var IsSelected = document.getElementById('#select');
    const message = [];
    //Loop through the CheckBoxes.
    for (i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].checked) {
            var row = checkBoxes[i].parentNode.parentNode;
            var Ids = row.cells[3].innerHTML;
            var strID = parseInt(Ids);
            message.push(strID);
        }
    }
    if (message.length != 0) {

        var formData = new FormData(form);
        Id = message;
        var IsSelected = document.getElementById('#select')
        for (i = 0; i < Id.length; i++) {
            formData.append("id", Id[i]);
        }
        try {
            $.ajax({
                type: 'POST',
                url: '/Admin/Cersai/Register',
                data: formData,
                contentType: false,
                processData: false,
                success: function (res) {

                    $('#view-cersai').html(res.html)
                    $('#modelCERSAIDetails').modal('hide');
                    $('#modelCERSAIDetails .modal-body').html('');
                    $('#modelCERSAIDetails .modal-title').html('');
                    ReloadCersaiDetails();
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
        //to prevent default form submit event
        return false;

    }
    else {
        swal.fire(
            'Warning!',
            'Please select atleast one Assest',
            'warning'
        )

        return false;
    }
}


function ReloadDisbursementDetails() {
    var dataTableAD;
    dataTableAD = $("tblDisbursementDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadAdditionalCondtionDetails() {
    var dataTableAD;
    dataTableAD = $("tblAdditonalConditionDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadAuditDetails() {
    var dataTableAD;
    dataTableAD = $("tblAuditDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadBuildingInspectionDetails() {
    var dataTableAD;
    dataTableAD = $("tblbuildingInspectionDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadBuildingInspectionDetailsAd() {
    var dataTableAD;
    dataTableAD = $("tblbuildingInspectionDatatableAd").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}


function ReloadImportMachnieryDetails() {
    var dataTableAD;
    dataTableAD = $("tblimportMachinery").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadImportMachnieryDetailsAd() {
    var dataTableAD;
    dataTableAD = $("tblimportMachineryAd").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}



function ReloadMeansOfFinanceDetails() {
    var dataTableAD;
    dataTableAD = $("tblMeansOfFinaceDetails").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadLandInspectionDetails() { 

    var dataTableAD;
    dataTableAD = $("tbllandInspectionDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadLandInspectionDetailsAd() {

    var dataTableAD;
    dataTableAD = $("tbllandInspectionDatatableAd").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function UnitOfNamePost(form) {

    try {
        debugger
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result == "") {
                    $('#txtname').val('');
                    $("#UtName").val(res)
                    $("#UnitName").val(res)
                    //$("#ChangeNameofUnitProgressBar").attr('class', 'progress-bar bg-success');
                    //$("#ChangeLocation").attr('class', 'tab-pane fade active show');
                    //$("#ChangeNameofUnit").attr('class', 'tab-pane fade');
                    //$("#ChangeLocation-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                    //$("#ChangeNameofUnit-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                    $("#panelsStayOpen-collapseOne").attr('class', 'accordion-collapse collapsed')
                    $("#panelsStayOpen-collapseTwo").attr('class', 'accordion-collapse collapse show');

                    $("#spanErrorMsg").html("");
                    $("#divEnquiryAlertPopup").html("");
                    window.scrollTo(0, 0);

                } else {
                    var res = result.utName;
                    $('#txtname').val('');
                    $("#UtName").val(res)
                    $("#UnitName").val(res)
                    $("#ChangedUnitName").val(res)
                    //$("#ChangeNameofUnitProgressBar").attr('class', 'progress-bar bg-success');
                    //$("#ChangeLocation").attr('class', 'tab-pane fade active show');
                    //$("#ChangeNameofUnit").attr('class', 'tab-pane fade');

                    //$("#ChangeLocation-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                    //$("#ChangeNameofUnit-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                    $("#spanErrorMsg").html("");
                    $("#divEnquiryAlertPopup").html("");
                    window.scrollTo(0, 0);
                }


            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}


function ReloadAdditonalCondtionDetails() {

    var dataTableAD;
    dataTableAD = $("tblAdditonalConditionDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadForm8AndForm13Details() {

    var dataTableAD;
    dataTableAD = $("tblForm8AndForm13").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadInspectionDetailDetails() {
    var dataTableAD;
    dataTableAD = $("tblInspectionDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadGenerateReceipts() {
    var dataTableAD;
    dataTableAD = $("tblGenerateReceiptDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadSaveReceipts() {
    var dataTableAD;
    dataTableAD = $("tblSecurityDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadChangeLocationDetails1() {
    debugger;
    var dataTableAD;
    dataTableAD = $("tblChangeLocation1").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}
function ReloadChangeLocationDetails2() {
    debugger;
    var dataTableAD;
    dataTableAD = $("tblChangeLocation2").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}
function ReloadChangeLocationDetails3() {
    debugger;
    var dataTableAD;
    dataTableAD = $("tblChangeLocation3").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}
function ReloadChangePromoterProfileDetails() {

    var dataTableAD;
    dataTableAD = $("tblPromoterProfile").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No record, Please click on <b>Add New Promoter</b> Button"
        }
    });
}

function ReloadPromoterAddressDetails() {

    var dataTableAD;
    dataTableAD = $("tblPromoterAddress").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}


function ReloadProductDetails() {

    var dataTableAD;
    dataTableAD = $("tblProductDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadAssetDetails() {

    var dataTableAD;
    dataTableAD = $("tblAssetDetails").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadChangeBankDetails() {

    var dataTableAD;
    dataTableAD = $("tblChangeBankDetailsDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}

function ReloadPromoterLiabilityInfo() {

    var dataTableAD;
    dataTableAD = $("tblLiabilityInfoDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}


function ReloadChangePromoterBankDetails() {

    var dataTableAD;
    dataTableAD = $("tblPromoterBankInfo").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add Promoter Bank Details</b> Button"
        }
    });
}
function ReloadBuildMatSiteInspectionDetails() {

    var dataTableAD;
    dataTableAD = $("tblBuildMatSiteInspectionDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function CreateWorkingCapitalInspection(form, LoanAcc, DcwcIno) {
    debugger;
    $("#LoanAcc").val(LoanAcc);
    $("#DcwcIno").text(DcwcIno);

    try {

        $.ajax({
            type: "POST",
            url: GetRoute('/UnitDetails/SaveWorkingCapitalDetails'),
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {

                if (res.isValid) {
                    debugger;
                    //$("#WorkingCapitalProgressBar").attr('class', 'progress-bar bg-success');
                    //$("#StatusofImplementation").attr('class', 'tab-pane fade active show');
                    //$("#WorkingCapital").attr('class', 'tab-pane fade');

                    //$("#WorkingCapital-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                    //$("#StatusofImplementation-tab").attr('class', 'nav-link').attr('aria-selected', 'true');
                    $("#panelsStayOpen-collapseFifteen").attr('class', 'accordion-collapse collapse');
                    $("#Working_CapitalDetails_accor_btn").attr('class', 'accordion-button collapsed');
                    $("#panelsStayOpen-collapseSixteen").attr('class', 'accordion-collapse collapse show');                  
                    $("#MeansOfFinance_Detail_accor_btn").attr('class', 'accordion-button');

                    $("#MeansOfFinance-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                    

                    $("#spanErrorMsg").html("");
                    $("#divEnquiryAlertPopup").html("");
                    window.scrollTo(0, 0);
                }
                else {
                    swal.fire(
                        'Error',
                        'An Error Occured While Saving the Data.',
                        'cancelled'
                    )
                    $("#panelsstayopen-collapsefifteen").attr('class', 'accordion-collapse collapse show');
                    $("#panelsstayopen-collapsesixteen").attr('class', 'accordion-collapse collapse');
                    $("#working_capitaldetails_accor_btn").attr('class', 'accordion-button');
                    $("#meansoffinance_detail_accor_btn").attr('class', 'accordion-button collapsed');
                    $("#WorkingCapitalProgressBar").attr('class', 'progress-bar bg-danger');
                    $("#spanErrorMsg").html(data.message);
                    $("#divEnquiryAlertPopup").html(data.message);
                    $("#modalAlertEnq").modal('show');
                }

            },
            error: function (err) {
                $("#WorkingCapitalProgressBar").attr('class', 'progress-bar bg-danger');
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function ReloadFurnitureInspectionDetails() {

    var dataTableAD;
    dataTableAD = $("tblFurnitureInspectionDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadFurnitureInspectionDetailsAd() {

    var dataTableAD;
    dataTableAD = $("tblFurnitureInspectionDatatableAd").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadIndigenousInspectionDetails() {
   
    var dataTableAD;
    dataTableAD = $().DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadIndigenousInspectionDetailsAd() {

    var dataTableAD;
    dataTableAD = $("tblIndigenousMachineryInspectionDatatableAd").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadProjectCost() {

    var dataTableAD;
    dataTableAD = $("tblProjectCostDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add Inspection of Project Cost</b> Button"
        }
    });
}
function ReloadLetterOfCreditDetails() {
    var dataTableAD;
    dataTableAD = $("tblLetterOfCreditDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });

}

function ReloadFurnitureDetails() {
    var dataTableAD;
    dataTableAD = $('#tblFurnitureAcquisition').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

$("#testing").on("change", function () {

    var data = $(this).val();
    debugger
    if (data.length > 3) {
        swal.fire({
            title: "Amount Cannot be greater than 100 lakhs!",
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then((result) => {
            $(this).val("");
        })
    }
});

// Creation of Security and Acquisition of Asset

function ReloadMachineryAcquisitionDetails() {
    var dataTable;
    dataTable = $('tblMachineryAcquisitionDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadLandAcquisitionDetails() {
    var dataTable;
    dataTable = $('tblLandAcquisitionDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}


//  Building Acquisition
function ReloadBuildingAcquisitionDetails() {
    var dataTable;
    dataTable = $('tblBuildingAcquisitionDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}
function ReloadAllocationDetails() {
    var dataTable;
    dataTable = $('tblAllocationDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
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



// Recommended Disbursement Details
function ReloadRecommendedDisbursementDetails() {
    var dataTable;
    dataTable = $('tblRecommDisbursementDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function fxnSubmitGenerateReceipt(Id) {
    debugger
    swal.fire({
        title: 'Submitting ' + Id,
        html: "<h5 style='color:red'>  Action can't be reverted after Submit  </h5>",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        debugger
        if (result.isConfirmed) {
            var formData = new FormData();
            formData.append("TblLaReceiptDet.ReceiptRefNo", Id);
            try {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/GenerateReceipt/Submit',
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        if (res.isValid) {
                            swal.fire(
                                'Submitted!',
                                'Record is successfully Submitted.',
                                'success'
                            )
                            var loanno = res.data.accountNumber;
                            var loansub = res.data.loanSub;
                            var unitname = res.data.unitName;
                            location.href = '/Admin/LoanRelatedReceipt/ViewAccount?AccountNumber=' + loanno + "&LoanSub=" + loansub + "&UnitName=" + unitname;
                        }

                    },
                    error: function (err) {
                        console.log(err)
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
}

// Promoter Login
function showInPopupLAProm(url, title, module) {

    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            debugger
            switch (module) {
                case 'LoanReceipt':
                    $('#modelLoanReceiptDetails .modal-body').html(res);
                    $('#modelLoanReceiptDetails .modal-title').html(title);
                    $('#modelLoanReceiptDetails').modal('show');
                    // to make popup draggable
                    $('.modal-dialog').draggable({
                        handle: ".modal-header"
                    });
                    break
                case 'LoanPayment':
                    $('#modelLoanPaymentList .modal-body').html(res);
                    $('#modelLoanPaymentList .modal-title').html(title);
                    $('#modelLoanPaymentList').modal('show');
                    // to make popup draggable
                    $('.modal-dialog').draggable({
                        handle: ".modal-header"
                    });
                    break
                default:
                    break
            }

        }
    });
}
function showPayNowPopup(url, title) {
    debugger;
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            debugger;
            $('#viewDocuments .modal-body').html(res);
            $('#viewDocuments .modal-title').html(title);
            $('#viewDocuments').modal('show');
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
function ClosePayPopupFormsh() {
    $('#viewDocuments .modal-body').html('');
    $('#viewDocuments .modal-title').html('');
    $('#viewDocuments').modal('hide');
}

function ClosePopupFormLAProm() {

    $('#modelLoanReceiptDetails .modal-body').html('');
    $('#modelLoanReceiptDetails .modal-title').html('');
    $('#modelLoanReceiptDetails').modal('hide');

    $('#modelLoanPaymentList .modal-body').html('');
    $('#modelLoanPaymentList .modal-title').html('');
    $('#modelLoanPaymentList').modal('hide');
}

function showInPopupPayment(title, module, LoanSub, UnitName) {
    var grid = document.getElementById("tblLoanReceiptDatatable");
    var checkBoxes = grid.getElementsByTagName("INPUT");
    var IsSelected = document.getElementById('#select');
    //var loansub = LoanSub;
    //var unitname = UnitName;
    const message = [];
    //Loop through the CheckBoxes.
    for (i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].checked) {
            var row = checkBoxes[i].parentNode.parentNode;
            var RectNumber = row.cells[1].innerHTML;
            //  var strID = parseInt(Ids);
            message.push(RectNumber);
        }
    }
    if (message.length != 0) {

        const ReferenceNumber = [];

        var RefNumber = message;

        var IsSelected = document.getElementById('#select')
        for (i = 0; i < RefNumber.length; i++) {
            ReferenceNumber.push(RefNumber[i]);

        }
        console.log(ReferenceNumber);
        debugger
        try {
            $.ajax({
                type: 'GET',
                url: '/Customer/LoanReceipt/CreatePayment',
                traditional: true,
                data: {
                    ReferenceNumber: ReferenceNumber,
                    loansub: LoanSub,
                    unitname: UnitName
                },
                success: function (res) {
                    switch (module) {
                        case 'LoanReceipt':
                            $('#modelLoanReceiptDetails .modal-body').html(res);
                            $('#modelLoanReceiptDetails .modal-title').html(title);
                            $('#modelLoanReceiptDetails').modal('show');
                            // to make popup draggable
                            $('.modal-dialog').draggable({
                                handle: ".modal-header"
                            });
                            break
                        default:
                            break
                    }
                },
                error: function (err) {
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }
        //to prevent default form submit event
        return false;

    }
    else {
        alert("Please Select Atleast One Receipt Reference Number To Create Payment")
        return false;
    }
}



function ReloadProposalDetails() {
    var dataTableAD;
    dataTableAD = $("tblProposalDetailsDatatable").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}

function ReloadChargeDetails() {
    debugger
    var dataTableAD;
    dataTableAD = $("#tblChargeDetailsDatatablecreate").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found, Please click on <b>Add New</b> Button"
        }
    });
}



function BeneficiaryDetailsPost(form) {

    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.isValid) {
                    $("#BeneficiaryDetailsProgressBar").attr('class', 'progress-bar bg-success');
                    $("#BeneficiaryDetails").attr('class', 'tab-pane fade');

                    $("#BeneficiaryDetails-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                    $("#spanErrorMsg").html("");
                    $("#divEnquiryAlertPopup").html("");
                    window.scrollTo(0, 0);
                }
                else {
                    $("#BeneficiaryDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                    $("#spanErrorMsg").html(result.message);
                    $("#divEnquiryAlertPopup").html(result.message);
                    $("#modalAlertEnq").modal('show');
                }

            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function ReloadSecurityHolderDetails() {
    var dataTableAD;
    dataTableAD = $('tblSecurityDatatable').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}

function ReloadOtherDebitDetails() {
    var dataTableAD;
    dataTableAD = $('OtherdebitDetails').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}

function ReloadAddressDetails() {
    var dataTableAD;
    dataTableAD = $('tblChangeLocation1').DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}
// added by gowtham 
function showPopupMessage(title, message, type) {
    const popup = document.createElement('div');
    popup.classList.add('popup');
    popup.classList.add(type);
    popup.innerHTML = `
    <div class="popup-overlay"></div>
    <div class="popup-container">
      <div class="popup-header">
        <i class="fa fa-check"></i>
        <h2>${title}</h2>
        <button class="close-button">&times;</button>
      </div>
      <div class="popup-content">
        <p>${message}</p>
      </div>
      <div class="popup-footer">
        <button class="ok-button">OK</button>
      </div>
    </div>
  `;
    document.body.appendChild(popup);

    const closeButton = popup.querySelector('.close-button');
    closeButton.addEventListener('click', () => {
        popup.remove();
    });

    const okButton = popup.querySelector('.ok-button');
    okButton.addEventListener('click', () => {
        popup.remove();
    });
}




//Create Method 
function JqueryAjaxCollatAddMethod(form, Module) {
    debugger;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                debugger;
                var acc = res.data;
                switch (Module) {
                    case "ColletralSecurity":
                        if (res.isValid) {
                            swal.fire(
                                /*showPopupMessage(*/
                                'Created',
                                'Created Loan Account Number : ' + acc + ' Details for Collateral Security.',
                                'success'
                            );

                            $('#view-all').html(res.html)
                            $('#colletralSecurityDetails .modal-body').html('');
                            $('#colletralSecurityDetails .modal-title').html('');
                            $('#colletralSecurityDetails').modal('hide');
                            ReloadSecurityHolderDetails();
                            break;
                        }
                        else
                            $('#colletralSecurityDetails .modal-body').html(res.html);
                        break;

                    default:
                        break;
                }
            },
            error: function (err) {
                showPopupMessage('Error', 'An error occurred: ' + err.message, 'error');

                console.log(err)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
    }
}


      
//Edit Method Added By;Swetha M Legal Documentation
function JqueryAjaxPostMethod(form, Module) {
    debugger;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                debugger;
                var acc = res.data;
                switch (Module) {

                    case "PrimarySecurity":
                        if (res.isValid) {
                            swal.fire(
                            /*showPopupMessage(*/
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Primary/Collateral Security.',
                                'success'
                            );

                            $('#view-all').html(res.html)
                            $('#primarySecurityDetails .modal-body').html('');
                            $('#primarySecurityDetails .modal-title').html('');
                            $('#primarySecurityDetails').modal('hide');
                            ReloadSecurityHolderDetails();
                            break;
                        }
                        else
                            $('#primarySecurityDetails .modal-body').html(res.html);
                        break;

                    case "ColletralSecurity":
                        if (res.isValid) {
                            swal.fire(
                                /*showPopupMessage(*/
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Collateral Security.',
                                'success'
                            );

                            $('#view-all').html(res.html)
                            $('#colletralSecurityDetails .modal-body').html('');
                            $('#colletralSecurityDetails .modal-title').html('');
                            $('#colletralSecurityDetails').modal('hide');
                            ReloadSecurityHolderDetails();
                            break;
                        }
                        else
                            $('#colletralSecurityDetails .modal-body').html(res.html);
                        break;
                    case "SecurityCharge":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Security Charge.',
                                'success'
                            )
                            $('#securityTab').html(res.html)
                            $('#modelSecurityChargeDetails .modal-body').html('');
                            $('#modelSecurityChargeDetails .modal-title').html('');
                            $('#modelSecurityChargeDetails').modal('hide');
                            debugger;
                            ReloadSecurityChargeDetails();
                            break;
                        }
                        else
                            $('#modelSecurityChargeDetails .modal-body').html(res.html);
                        break;

                    case "CersaiDetails":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for CERSAI  Registration.',
                                'success'
                            )
                            $('#view-cersai').html(res.html)
                            $('#modelCERSAIDetails').modal('hide');
                            $('#modelCERSAIDetails .modal-body').html('');
                            $('#modelCERSAIDetails .modal-title').html('');
                            ReloadCersaiDetails();
                            break;
                        }
                        else
                            $('#modelCERSAIDetails .modal-body').html(res.html);
                        break;
                    case "Condition":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Condition.',
                                'success'
                            )
                            $('#view-all-condition').html(res.html)
                            $('#modelConditionDetails .modal-body').html('');
                            $('#modelConditionDetails .modal-title').html('');
                            $('#modelConditionDetails').modal('hide');
                            ReloadConditionDetails();
                            break;
                        }
                        else
                            $('#modelConditionDetails .modal-body').html(res.html);
                        break;
                    case "GuarantorDeed":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Guarantor Deed',
                                'success'
                            )
                            $('#view-all-guarantor').html(res.html)
                            $('#modelGuarantorDetails .modal-body').html('');
                            $('#modelGuarantorDetails .modal-title').html('');
                            $('#modelGuarantorDetails').modal('hide');
                            ReloadGuarantorDetails();
                            break;
                        }
                        else
                            $('#modelGuarantorDetails .modal-body').html(res.html);
                        break;

                    case "Hypothecation":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Hypothecation/Mortgage.',
                                'success'
                            )
                            $('#view-all-hypothecation').html(res.html)
                            $('#modelHypothecationDetails .modal-body').html('');
                            $('#modelHypothecationDetails .modal-title').html('');
                            $('#modelHypothecationDetails').modal('hide');
                            ReloadHypothecationDetails();
                            break;
                        }
                        else
                            $('#modelHypothecationDetails .modal-body').html(res.html);
                        break;

                    case "OtherDebit":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Other Debit Details.',
                                'success'
                            )

                            $('#view-all-OtherDebits').html(res.html)
                            $('#modelOtherDebitsDetails .modal-body').html('');
                            $('#modelOtherDebitsDetails .modal-title').html('');
                            $('#modelOtherDebitsDetails').modal('hide');
                            ReloadOtherDebitDetails();
                            break;
                        }
                        else
                            $('#modelOtherDebitsDetails .modal-body').html(res.html);
                        break;

                    case "GenerateReceipt":
                        debugger
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Details of this : ' + acc + ' Receipt Number.',
                                'success'
                            )
                            $('#view-all-GenerateReceipts').html(res.html)
                            $('#modelGenerateReceiptDetails .modal-body').html('');
                            $('#modelGenerateReceiptDetails .modal-title').html('');
                            $('#modelGenerateReceiptDetails').modal('hide');
                            ReloadGenerateReceipts();
                            break;
                        }
                        else
                            $('#modelGenerateReceiptDetails .modal-body').html(res.html);
                        break;
                    case "ReceiptPayment":
                        debugger
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Receipt Payment Details for Loan Account Number : ' + acc + '',
                                'success'
                            )
                            $('#view-all-ReceiptPayment').html(res.html)
                            $('#modelReceiptPaymentDetails .modal-body').html('');
                            $('#modelReceiptPaymentDetails .modal-title').html('');
                            $('#modelReceiptPaymentDetails').modal('hide');
                            ReloadSaveReceipts();
                            break;
                        }
                        else
                            $('#modelReceiptPaymentDetails .modal-body').html(res.html);
                        break;
                    case "SavedReceipt":
                        debugger
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Details of this ' + acc + ' Loan Account Number',
                                'success'
                            )
                            $('#view-all-SaveReceipts').html(res.html)
                            $('#modelEditSaveReceiptDetails .modal-body').html('');
                            $('#modelEditSaveReceiptDetails .modal-title').html('');
                            $('#modelEditSaveReceiptDetails').modal('hide');
                            window.location.reload();
                            break;
                        }
                        else
                            $('#modelEditSaveReceiptDetails .modal-body').html(res.html);
                        break;

                    default:
                        break;
                }
            },
            error: function (err) {
                showPopupMessage('Error', 'An error occurred: ' + err.message, 'error');

                console.log(err)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
    }
}
//Add Method Added By;Swetha M Legal Documentation
function JqueryAjaxAddMethod(form, Module) {

    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                var acc = res.data;
                switch (Module) {
                    case "Condition":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Condition Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-condition').html(res.html)
                            $('#modelConditionDetails .modal-body').html('');
                            $('#modelConditionDetails .modal-title').html('');
                            $('#modelConditionDetails').modal('hide');
                            ReloadConditionDetails();
                            break;
                        }
                        else
                            $('#modelConditionDetails .modal-body').html(res.html);
                        break;
                    case "OtherDebit":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New OtherDebit Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-OtherDebits').html(res.html)
                            $('#modelOtherDebitsDetails .modal-body').html('');
                            $('#modelOtherDebitsDetails .modal-title').html('');
                            $('#modelOtherDebitsDetails').modal('hide');
                            ReloadOtherDebitDetails();
                            break;
                        }
                        else
                            $('#modelOtherDebitsDetails .modal-body').html(res.html);
                        break;
                    case "GenerateReceipt":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Details of this ' + acc + ' Receipt Number',
                                    'success'
                                )
                            }
                            var loanno = res.data.accountNumber;
                            var loansub = res.data.loanSub;
                            var unitname = res.data.unitName;
                            location.href = '/Admin/LoanRelatedReceipt/ViewAccount?AccountNumber=' + loanno + "&LoanSub=" + loansub + "&UnitName=" + unitname;
                            break;
                        }
                        else
                            $('#modelGenerateReceiptDetails .modal-body').html(res.html);
                        break;
                    default:
                        break;
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Delete Method Added By:Swetha M Legal Dcoumentation
function JqueryAjaxDeleteMethod(url, Module) {
    swal.fire({
        title: 'Are you sure?',
        text: "You want to delete this record!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            try {
                $.ajax({
                    type: 'POST',
                    url: url,
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        swal.fire(
                            'Deleted!',
                            'Record successfully is Deleted.',
                            'success'
                        )
                        switch (Module) {
                            case "PrimarySecurity":
                                $('#view-all').html(res.html);
                                ReloadSecurityHolderDetails();
                                break;
                            case "SecurityCharge":
                                $('#securityTab').html(res.html);
                                ReloadSecurityChargeDetails();
                                break;

                            case "Cersai":
                                $('#view-cersai').html(res.html);
                                ReloadCersaiDetails();
                                break;

                            case "Condition":
                                $('#view-all-condition').html(res.html);
                                ReloadConditionDetails();
                                break;
                            case "GuarantorDeed":
                                $('#view-all-guarantor').html(res.html);
                                ReloadGuarantorDetails();
                                break;
                            case "Hypothecation":
                                $('#view-all-hypothecation').html(res.html);
                                ReloadHypothecationDetails();
                                break;
                            case "OtherDebits":
                                debugger;
                                $('#view-all-OtherDebits').html(res.html);
                                ReloadOtherDebitDetails();
                                break;
                            case "GenerateReceipt":
                                debugger;
                                $('#view-all-GenerateReceipts').html(res.html);
                                ReloadGenerateReceipts();
                                break;
                            default:
                                break;
                                
                        }
                    },
                    error: function (err) {
                        console.log(err)
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
}

function fxnSubmitOtherDebits() {

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
                    type: 'POST',
                    url: '',
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        $('#view-all-OtherDebits').html(res.html);
                        ReloadOtherDebitDetails();
                    },
                    error: function (err) {
                        console.log(err)
                    }
                });
                swal.fire(
                    'Submited Debit Details',
                    'Details Submitted Successfully!!',
                    'success'
                )
            } catch (ex) {
                console.log(ex)
            }
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            return false;
        }
        return false;
    })
}

//Add Method - Added By;Swetha M Disbursment Condition
function JqueryDisbursmentAddMethod(form, Module) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                var acc = res.data;
                switch (Module) {
                    case "AdditionalCondition":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'Created LoanAccountNumber : ' + acc + '  Details for Additional Condition.',
                                'success'
                            )
                            $('#view-all-additional-condition').html(res.html)
                            $('#modelAdditionalConditionDetails .modal-body').html('');
                            $('#modelAdditionalConditionDetails .modal-title').html('');
                            $('#modelAdditionalConditionDetails').modal('hide');
                            ReloadAdditionalCondtionDetails();
                            break;
                        }
                        else
                            $('#modelAdditionalConditionDetails .modal-body').html(res.html);
                        break;
                    case "Disbursement":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'Created LoanAccountNumber : ' + acc + '  Details for Disbursement Condition.',
                                'success'
                            )
                            $('#view-all-Disbursement').html(res.html)
                            $('#modelDisbursementConditionDetails .modal-body').html('');
                            $('#modelDisbursementConditionDetails .modal-title').html('');
                            $('#modelDisbursementConditionDetails').modal('hide');
                            ReloadDisbursementDetails();
                            break;
                        }
                        else
                            $('#modelDisbursementConditionDetails .modal-body').html(res.html);
                        break;
                    case "Form8and13":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'Created Form 8 And Form 13 details for: ' + acc + ' AccountNumber.',
                                'success'
                            )
                            $('#view-all-Form8AndForm13').html(res.html)
                            $('#modelForm8AndForm13Details .modal-body').html('');
                            $('#modelForm8AndForm13Details .modal-title').html('');
                            $('#modelForm8AndForm13Details').modal('hide');
                            ReloadForm8AndForm13Details();
                            break;
                        }
                        else
                            $('#modelForm8AndForm13Details .modal-body').html(res.html);
                        break;
                    default:
                        break;
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Edit Method - Added By;Swetha M Disbursment Condition
function JqueryDisbursmentEditMethod(form, Module) {
    debugger;
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                var acc = res.data;
                switch (Module) {
                    case "AdditionalCondition":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated LoanAccountNumber : ' + acc + '  Details for Additional Condition.',
                                'success'
                            )
                            $('#view-all-additional-condition').html(res.html)
                            $('#modelAdditionalConditionDetails .modal-body').html('');
                            $('#modelAdditionalConditionDetails .modal-title').html('');
                            $('#modelAdditionalConditionDetails').modal('hide');
                            ReloadAdditionalCondtionDetails();
                            break;
                        }
                        else
                            $('#modelAdditionalConditionDetails .modal-body').html(res.html);
                        break;
                    case "Disbursement":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated LoanAccountNumber : ' + acc + '  Details for Disbursement Details.',
                                'success'
                            )
                            $('#view-all-Disbursement').html(res.html)
                            $('#modelDisbursementConditionDetails .modal-body').html('');
                            $('#modelDisbursementConditionDetails .modal-title').html('');
                            $('#modelDisbursementConditionDetails').modal('hide');
                            ReloadDisbursementDetails();
                            break;
                        }
                        else
                            $('#modelDisbursementConditionDetails .modal-body').html(res.html);
                        break;
                    case "Form8and13":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Form 8 And Form 13 details for: ' + acc + ' AccountNumber.',
                                'success'
                            )
                            $('#view-all-Form8AndForm13').html(res.html)
                            $('#modelForm8AndForm13Details .modal-body').html('');
                            $('#modelForm8AndForm13Details .modal-title').html('');
                            $('#modelForm8AndForm13Details').modal('hide');
                            ReloadForm8AndForm13Details();
                        }
                        else
                            $('#modelForm8AndForm13Details .modal-body').html(res.html);
                        break;
                    case "SidbiApproval":
                        if (result == "") {
                            $('#txtname').val('');
                            $("#SidbiProgressBar").attr('class', 'progress-bar bg-success');
                            $("#DisbursementCondition-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                            $("#SidbiApproval-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                            $("#SidbiProgressBar").attr('class', 'progress-bar bg-success');
                            $("#DisbursementCondition").attr('class', 'tab-pane fade active show');
                            $("#SidbiApproval").attr('class', 'tab-pane fade');

                            $("#spanErrorMsg").html("");
                            $("#divEnquiryAlertPopup").html("");
                            window.scrollTo(0, 0);
                            break;
                        } else {
                            var res = result.WhAppr;
                            $('#txtname').val('');
                            $("#WhAppr").val(res)
                            $("#SidbiProgressBar").attr('class', 'progress-bar bg-success');
                            break;
                        }

                    case "FirstInvestmentClause":
                        if (res.isValid) {
                            debugger                    
                            $("#FirstInvestmentClauseProgressBar").attr('class', 'progress-bar bg-success');
                            $("#Form8AndForm13").attr('class', 'tab-pane fade active show');
                            $("#FirstInvestmentClause").attr('class', 'tab-pane fade');

                            $("#Form8AndForm13-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                            $("#FirstInvestmentClause-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                            $("#spanErrorMsg").html("");
                            $("#divEnquiryAlertPopup").html("");
                            window.scrollTo(0, 0);
                            break;
                        }

                        else {
                            swal.fire(
                                'Error',
                                'An Error Occured While Saving the Data.',
                                'error'
                            )
                            $("#FirstInvestmentClauseProgressBar").attr('class', 'progress-bar bg-danger');
                            $("#spanErrorMsg").html(result.message);
                            $("#divEnquiryAlertPopup").html(result.message);
                            $("#modalAlertEnq").modal('show');
                            break;
                        }
                    default:
                        break;
                }

            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Delete Method Added By:Swetha M Disbursment Condition
function JqueryDisbursmentDeleteMethod(url, Module) {
    swal.fire({
        title: 'Are you sure?',
        text: "You want to delete this record!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            try {
                $.ajax({
                    type: 'POST',
                    url: url,
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        swal.fire(
                            'Deleted!',
                            'Record successfully is Deleted.',
                            'success'
                        )
                        switch (Module) {
                            case "Disbursement":
                                $('#view-all-Disbursement').html(res.html);
                                ReloadDisbursementDetails();
                                break;
                            case "AdditionalCondition":
                                $('#view-all-additional-condition').html(res.html);
                                ReloadAdditionalCondtionDetails();
                                break;
                            case "Form8and13":
                                $('#view-all-Form8AndForm13').html(res.html);
                                ReloadForm8AndForm13Details();
                                break;
                            default:
                                break;
                        }
                    },
                    error: function (err) {
                        console.log(err)
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

}

//Add Method - Added By;Swetha M Inspection OF Unit
function JqueryInspectionAddMethod(form, Module) {
  
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                var acc = res.data;
                      
                switch (Module) {
                    case "InspectionDetail":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Inspection List.',
                                    'success'
                                )
                            }
                            $('#view-all-InpsectionDetail').html(res.html)
                            $('#modelInspectionDetail .modal-body').html('');
                            $('#modelInspectionDetail .modal-title').html('');
                            $('#modelInspectionDetail').modal('hide');
                            ReloadInspectionDetailDetails();
                            break;
                        }
                        else
                            $('#modelInspectionDetail .modal-body').html(res.html);
                        break;
                    case "LandInspection":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Land Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-LandInspection').html(res.html)
                            $('#modelLandInspectionDetails .modal-body').html('');
                            $('#modelLandInspectionDetails .modal-title').html('');
                            $('#modelLandInspectionDetails').modal('hide');
                            ReloadLandInspectionDetails();
                            break;
                        }
                        else
                            $('#modelLandInspectionDetails .modal-body').html(res.html);
                        break;
                    case "LandInspectionAd":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Land Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-LandInspectionAd').html(res.html)
                            $('#modelLandInspectionDetailsAd .modal-body').html('');
                            $('#modelLandInspectionDetailsAd .modal-title').html('');
                            $('#modelLandInspectionDetailsAd').modal('hide');
                            ReloadLandInspectionDetailsAd();
                            break;
                        }
                        else
                            $('#modelLandInspectionDetails_ad .modal-body').html(res.html);
                        break;
                    case "BuildingInspection":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Building Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-buildingInspection').html(res.html);
                            $('#modelBuildingInspectionDetails').modal('hide');
                            $('#modelBuildingInspectionDetails .modal-body').html('');
                            $('#modelBuildingInspectionDetails .modal-title').html('');
                            ReloadBuildingInspectionDetails();
                            break;
                        }
                        else
                            $('#modelBuildingInspectionDetails .modal-body').html(res.html);
                        break;
                    case "BuildingInspectionAd":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Building Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-BuildingInspectionAd').html(res.html);
                            $('#modelBuildingInspectionDetailsAd').modal('hide');
                            $('#modelBuildingInspectionDetailsAd .modal-body').html('');
                            $('#modelBuildingInspectionDetailsAd .modal-title').html('');
                            ReloadBuildingInspectionDetailsAd();
                            break;
                        }
                        else
                            $('#modelBuildingInspectionDetails .modal-body').html(res.html);
                        break;
                    case "BuildMatAtSite":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Building Materail At Site Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-BuildMatAtSite').html(res.html)
                            $('#modelBuildMatAtSiteDetails .modal-body').html('');
                            $('#modelBuildMatAtSiteDetails .modal-title').html('');
                            $('#modelBuildMatAtSiteDetails').modal('hide');
                            ReloadBuildMatSiteInspectionDetails();
                            break;
                        }
                        else
                            $('#modelBuildMatAtSiteDetails .modal-body').html(res.html);
                        break;
                    case "FurnitureInspection":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Furniture/Equipment Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-FurnitureInspection').html(res.html)
                            $('#modelFurnitureInspectionDetails .modal-body').html('');
                            $('#modelFurnitureInspectionDetails .modal-title').html('');
                            $('#modelFurnitureInspectionDetails').modal('hide');
                            ReloadFurnitureInspectionDetails();
                            break;
                        }
                        else
                            $('#modelFurnitureInspectionDetails .modal-body').html(res.html);
                        break;
                    case "FurnitureInspectionAd":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Furniture/Equipment Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-FurnitureInspectionAd').html(res.html)
                            $('#modelFurnitureInspectionDetailsAd .modal-body').html('');
                            $('#modelFurnitureInspectionDetailsAd .modal-title').html('');
                            $('#modelFurnitureInspectionDetailsAd').modal('hide');
                            ReloadFurnitureInspectionDetailsAd();
                            break;
                        }
                        else
                            $('#modelFurnitureInspectionDetails .modal-body').html(res.html);
                        break;
                    case "ImportMachinery":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Indigenous Machinery Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-ImportMachinery').html(res.html);
                            $('#modelImportMachineryDetails').modal('hide');
                            $('#modelImportMachineryDetails .modal-body').html('');
                            $('#modelImportMachineryDetails .modal-title').html('');
                            ReloadImportMachnieryDetails();
                            break;
                        }
                        else
                            $('#modelImportMachineryDetails .modal-body').html(res.html);
                        break;
                    case "ImportMachineryAd":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Indigenous Machinery Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-ImportedMachineryInspectionAd').html(res.html);
                            $('#modelImportedMachineryInspectionDetailsAd').modal('hide');
                            $('#modelImportedMachineryInspectionDetailsAd .modal-body').html('');
                            $('#modelImportedMachineryInspectionDetailsAd .modal-title').html('');
                            ReloadImportMachnieryDetailsAd();
                            break;
                        }
                        else
                            $('#modelImportMachineryDetails .modal-body').html(res.html);
                        break;
                    case "StatusofImplementation":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Status of Implementation.',
                                    'success'
                                )
                            }
                            $('#view-all-StatusofImplementation').html(res.html);
                            $('#modelStatusofImplementationDetails').modal('hide');
                            $('#modelStatusofImplementationDetails .modal-body').html('');
                            $('#modelStatusofImplementationDetails .modal-title').html('');
                            ReloadStatusofImplementationDetails();
                            break;
                        }
                            else
                            $('#modelStatusofImplementationDetails .modal-body').html(res.html);
                            break;
                     case "IndigenousMachinary":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Indigenous Machinery Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-IndigenousMachinary').html(res.html)
                            $('#modelIndigeneousMacInspectionDetails .modal-body').html('');
                            $('#modelIndigeneousMacInspectionDetails .modal-title').html('');
                            $('#modelIndigeneousMacInspectionDetails').modal('hide');
                            ReloadIndigenousInspectionDetails();
                            break;
                        }
                        else
                            $('#modelIndigeneousMacInspectionDetails .modal-body').html(res.html);
                        break;
                    case "IndigenousMachinaryAd":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Created',
                                    'Created Loan Account Number:' + acc + ' Details for Indigenous Machinery Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-IndigenousMachineryInspectionAd').html(res.html)
                            $('#modelIndigenousMachineryInspectionDetailsAd .modal-body').html('');
                            $('#modelIndigenousMachineryInspectionDetailsAd .modal-title').html('');
                            $('#modelIndigenousMachineryInspectionDetailsAd').modal('hide');
                            ReloadIndigenousInspectionDetailsAd();
                            break;
                        }
                        else
                            $('#modelIndigeneousMacInspectionDetails .modal-body').html(res.html);
                        break;
                    case "LetterOfCredit":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'Created Letter Of Credit Details For  Loan Account Number:' + acc + '.',
                                'success'
                            )

                            $('#view-all-LetterOfCredit').html(res.html);
                            $('#modelLetterOfCreditDetail').modal('hide');
                            $('#modelLetterOfCreditDetail .modal-body').html('');
                            $('#modelLetterOfCreditDetail .modal-title').html('');
                            ReloadLetterOfCreditDetails();
                            break;
                        }
                        else
                            $('#modelLetterOfCreditDetail .modal-body').html(res.html);
                        break;
                    case "MeansOfFinanceDetails":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'Created Means Of Details For  Loan Account Number:' + acc + '.',
                                'success'
                            )
                            $('#view-all-MeansOfFinanceDetails').html(res.html);
                            $('#modelMeansOfFinanceDetails').modal('hide');
                            $('#modelMeansOfFinanceDetails .modal-body').html('');
                            $('#modelMeansOfFinanceDetails .modal-title').html('');
                            ReloadMeansOfFinanceDetails();
                            break;
                        }
                        else
                            $('#modelMeansOfFinanceDetails .modal-body').html(res.html);
                        break;
                    case "ProjectCostDetail":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'Created Loan Account Number : ' + acc + ' Details for Project Cost.',
                                'success'
                            )
                            $('#view-all-ProjectCostDetail').html(res.html)
                            $('#modelProjectCostDetail .modal-body').html('');
                            $('#modelProjectCostDetail .modal-title').html('');
                            $('#modelProjectCostDetail').modal('hide');
                            ReloadProjectCost();
                            break;
                        }
                        else
                            $('#modelProjectCostDetail .modal-body').html(res.html);
                        break;
                    default:
                        break;
                }

            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Edit Method - Added By;Swetha M Inspection OF Unit
function JqueryInspectionEditMethod(form, Module) {

    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                var acc = res.data;
                switch (Module) {
                    case "InpsectionDetail":
                        var acc = res.data;
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Inspection List.',
                                    'success'
                                )
                            }
                            $('#view-all-InpsectionDetail').html(res.html)
                            $('#modelInspectionDetail .modal-body').html('');
                            $('#modelInspectionDetail .modal-title').html('');
                            $('#modelInspectionDetail').modal('hide');
                            ReloadInspectionDetailDetails();
                            break;
                        }
                        else
                            $('#modelInspectionDetail .modal-body').html(res.html);
                        break;
                    case "LandInspection":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Land Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-LandInspection').html(res.html)
                            $('#modelLandInspectionDetails .modal-body').html('');
                            $('#modelLandInspectionDetails .modal-title').html('');
                            $('#modelLandInspectionDetails').modal('hide');
                            ReloadLandInspectionDetails();
                            break;
                        }
                        else
                            $('#modelLandInspectionDetails .modal-body').html(res.html);
                        break;
                    case "LandInspectionAd":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Land Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-LandInspectionAd').html(res.html)
                            $('#modelLandInspectionDetailsAd .modal-body').html('');
                            $('#modelLandInspectionDetailsAd .modal-title').html('');
                            $('#modelLandInspectionDetailsAd').modal('hide');
                            ReloadLandInspectionDetailsAd();
                            break;
                        }
                        else
                            $('#modelLandInspectionDetailsAd .modal-body').html(res.html);
                        break;
                    case "BuildMatAtSite":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number:' + acc + ' Details for Building Materail At Site Inspection.',
                                'success'
                            )
                            $('#view-all-BuildMatAtSite').html(res.html)
                            $('#modelBuildMatAtSiteDetails .modal-body').html('');
                            $('#modelBuildMatAtSiteDetails .modal-title').html('');
                            $('#modelBuildMatAtSiteDetails').modal('hide');
                            ReloadBuildMatSiteInspectionDetails();
                            break;
                        }
                        else
                            $('#modelBuildMatAtSiteDetails .modal-body').html(res.html);
                        break;

                    case "BuildingInspection":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Building Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-buildingInspection').html(res.html);
                            $('#modelBuildingInspectionDetails').modal('hide');
                            $('#modelBuildingInspectionDetails .modal-body').html('');
                            $('#modelBuildingInspectionDetails .modal-title').html('');
                            ReloadBuildingInspectionDetails();
                            break;
                        }
                        else
                            $('#modelBuildingInspectionDetails .modal-body').html(res.html);
                        break;
                    case "BuildingInspectionAd":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Building Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-BuildingInspectionAd').html(res.html);
                            $('#modelBuildingInspectionDetailsAd').modal('hide');
                            $('#modelBuildingInspectionDetailsAd .modal-body').html('');
                            $('#modelBuildingInspectionDetailsAd .modal-title').html('');
                            ReloadBuildingInspectionDetailsAd();
                            break;
                        }
                        else
                            $('#modelBuildingInspectionDetails .modal-body').html(res.html);
                        break;
                    case "FurnitureInspection":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Furniture/Equipment Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-FurnitureInspection').html(res.html)
                            $('#modelFurnitureInspectionDetails .modal-body').html('');
                            $('#modelFurnitureInspectionDetails .modal-title').html('');
                            $('#modelFurnitureInspectionDetails').modal('hide');
                            ReloadFurnitureInspectionDetails();
                            break;
                        }
                        else
                            $('#modelFurnitureInspectionDetails .modal-body').html(res.html);
                        break;
                    case "FurnitureInspectionAd":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Furniture/Equipment Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-FurnitureInspectionAd').html(res.html)
                            $('#modelFurnitureInspectionDetailsAd .modal-body').html('');
                            $('#modelFurnitureInspectionDetailsAd .modal-title').html('');
                            $('#modelFurnitureInspectionDetailsAd').modal('hide');
                            ReloadFurnitureInspectionDetailsAd();
                            break;
                        }
                        else
                            $('#modelFurnitureInspectionDetails .modal-body').html(res.html);
                        break;

                    case "ImportMachinery":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number:' + acc + ' Details for Import Machinery Inspection.',
                                'success'
                            )
                            $('#view-all-ImportMachinery').html(res.html);
                            $('#modelImportMachineryDetails').modal('hide');
                            $('#modelImportMachineryDetails .modal-body').html('');
                            $('#modelImportMachineryDetails .modal-title').html('');
                            ReloadImportMachnieryDetails();
                            break;
                        }
                        else
                            $('#modelImportMachineryDetails .modal-body').html(res.html);
                        break;
                    case "ImportMachineryAd":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number:' + acc + ' Details for Import Machinery Inspection.',
                                'success'
                            )
                            $('#view-all-ImportedMachineryInspectionAd').html(res.html);
                            $('#modelImportedMachineryInspectionDetailsAd').modal('hide');
                            $('#modelImportedMachineryInspectionDetailsAd .modal-body').html('');
                            $('#modelImportedMachineryInspectionDetailsAd .modal-title').html('');
                            ReloadImportMachnieryDetailsAd();
                            break;
                        }
                        else
                            $('#ImportedMachineryInspectionAd .modal-body').html(res.html);
                        break;
                    case "IndigenousMachinary":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Indigenous Machinery Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-IndigenousMachinary').html(res.html)
                            $('#modelIndigeneousMacInspectionDetails .modal-body').html('');
                            $('#modelIndigeneousMacInspectionDetails .modal-title').html('');
                            $('#modelIndigeneousMacInspectionDetails').modal('hide');
                            ReloadIndigenousInspectionDetails();
                            break;
                        }
                        else
                            $('#modelIndigeneousMacInspectionDetails .modal-body').html(res.html);
                        break;
                    case "IndigenousMachinaryAd":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Indigenous Machinery Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-IndigenousMachineryInspectionAd').html(res.html)
                            $('#modelIndigenousMachineryInspectionDetailsAd .modal-body').html('');
                            $('#modelIndigenousMachineryInspectionDetailsAd .modal-title').html('');
                            $('#modelIndigenousMachineryInspectionDetailsAd').modal('hide');
                            ReloadIndigenousInspectionDetailsAd();
                            break;
                        }
                        else
                            $('#modelIndigeneousMacInspectionDetails .modal-body').html(res.html);
                        break;
                    case "StatusofImplementation":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Status of Implementation.',
                                    'success'
                                )
                            }
                            $('#view-all-StatusofImplementation').html(res.html);
                            $('#modelStatusofImplementationDetails').modal('hide');
                            $('#modelStatusofImplementationDetails .modal-body').html('');
                            $('#modelStatusofImplementationDetails .modal-title').html('');
                            ReloadStatusofImplementationDetails();
                            break;
                        }
                        else
                            $('#modelStatusofImplementationDetails .modal-body').html(res.html);
                        break;
                    case "LetterOfCredit":
                        if (res.isValid) {
                            if (acc != undefined) {
                                swal.fire(
                                    'Updated',
                                    'Updated Loan Account Number:' + acc + ' Details for Building Inspection.',
                                    'success'
                                )
                            }
                            $('#view-all-LetterOfCredit').html(res.html);
                            $('#modelLetterOfCreditDetail').modal('hide');
                            $('#modelLetterOfCreditDetail .modal-body').html('');
                            $('#modelLetterOfCreditDetail .modal-title').html('');
                            ReloadLetterOfCreditDetails();
                            break;
                        }
                        else
                            $('#modelLetterOfCreditDetail .modal-body').html(res.html);
                        break;
                    case "MeansOfFinanceDetails":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number:' + acc + ' Details for Means Of Finance.',
                                'success'
                            )
                            $('#view-all-MeansOfFinanceDetails').html(res.html);
                            $('#modelMeansOfFinanceDetails').modal('hide');
                            $('#modelMeansOfFinanceDetails .modal-body').html('');
                            $('#modelMeansOfFinanceDetails .modal-title').html('');
                            ReloadMeansOfFinanceDetails();
                            break;
                        }
                        else
                            $('#modelMeansOfFinanceDetails .modal-body').html(res.html);
                        break;
                    case "ProjectCostDetail":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Project Cost.',
                                'success'
                            )
                            $('#view-all-ProjectCostDetail').html(res.html)
                            $('#modelProjectCostDetail .modal-body').html('');
                            $('#modelProjectCostDetail .modal-title').html('');
                            $('#modelProjectCostDetail').modal('hide');
                            ReloadProjectCost();
                            break;
                        }
                        else
                            $('#modelProjectCostDetail .modal-body').html(res.html);
                        break;

                    default:
                        break;
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Delete Method Added By:Swetha M Inspection OF Unit
function JqueryInspectionDeleteMethod(url, Module) {
    debugger;
    swal.fire({
        title: 'Are you sure?',
        text: "You want to delete this record!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            try {
                $.ajax({
                    type: 'POST',
                    url: url,
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        debugger
                        swal.fire(
                            `${res.delete}!`,
                            `Record successfully is ${res.delete}.`,
                            'success'
                        )
                        switch (Module) {
                            case "InspectionDetail":
                                $('#view-all-InpsectionDetail').html(res.html);
                                ReloadInspectionDetailDetails();
                                break;
                            case "LandInspection":
                                $('#view-all-LandInspection').html(res.html);
                                ReloadLandInspectionDetails();
                                break;
                            case "LandInspectionAd":
                                $('#view-all-LandInspectionAd').html(res.html);
                                ReloadLandInspectionDetailsAd();
                                break;
                            case "BuildingInspection":
                                $('#view-all-buildingInspection').html(res.html);
                                ReloadBuildingInspectionDetails();
                                break;
                            case "BuildingInspectionAd":
                                $('#view-all-BuildingInspectionAd').html(res.html);
                                ReloadBuildingInspectionDetailsAd();
                                break;
                            case "BuildMatAtSite":
                                $('#view-all-BuildMatAtSite').html(res.html)
                                ReloadBuildMatSiteInspectionDetails();
                                break;
                            case "FurnitureInspection":
                                $('#view-all-FurnitureInspection').html(res.html);
                                ReloadFurnitureInspectionDetails();
                                break;
                            case "FurnitureInspectionAd":
                                $('#view-all-FurnitureInspectionAd').html(res.html);
                                ReloadFurnitureInspectionDetailsAd();
                                break;
                            case "ImportMachinery":
                                $('#view-all-ImportMachinery').html(res.html);
                                ReloadImportMachnieryDetails();
                                break;
                            case "ImportMachineryAd":
                                $('#view-all-ImportedMachineryInspectionAd').html(res.html);
                                ReloadImportMachnieryDetailsAd();
                                break;
                            case "IndigenousMachinary":
                                $('#view-all-IndigenousMachinary').html(res.html)
                                ReloadIndigenousInspectionDetails();
                                break;
                            case "IndigenousMachinaryAd":
                                $('#view-all-IndigenousMachineryInspectionAd').html(res.html)
                                ReloadIndigenousInspectionDetailAd();
                                break;
                            case "StatusofImplementation":
                                $('#view-all-StatusofImplementation').html(res.html)
                                ReloadStatusofImplementationDetails();
                                break;
                            case "LetterOfCredit":
                                $('#view-all-LetterOfCredit').html(res.html)
                                ReloadLetterOfCreditDetails();
                                break;
                            case "MeansOfFinanceDetails":
                                $('#view-all-MeansOfFinanceDetails').html(res.html);
                                ReloadMeansOfFinanceDetails();
                                break;
                            case "ProjectCostDetail":
                                $('#view-all-ProjectCostDetail').html(res.html)
                                ReloadProjectCost();
                                break;

                            default:
                                break;
                        }
                    },
                    error: function (err) {
                        console.log(err)
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

}

function AjaxSingleTabAddMethod(form, Module) {
    //debugger
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                var acc = res.data;
                switch (Module) {
                    case "AuditClearence":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Audit Clearance Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-Audit').html(res.html)
                            $('#modelAuditClearanceDetails').modal('hide');
                            $('#modelAuditClearanceDetails .modal-body').html('');
                            $('#modelAuditClearanceDetails .modal-title').html('');
                            ReloadAuditDetails();
                            break;
                        }
                        else
                            $('#modelConditionDetails .modal-body').html(res.html);
                        break;
                    case "OtherDebit":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New OtherDebit Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-OtherDebits').html(res.html)
                            $('#modelOtherDebitsDetails .modal-body').html('');
                            $('#modelOtherDebitsDetails .modal-title').html('');
                            $('#modelOtherDebitsDetails').modal('hide');
                            ReloadOtherDebitDetails();
                            break;
                        }
                        else
                            $('#modelOtherDebitsDetails .modal-body').html(res.html);
                        break;
                    case "Proposaldetails":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'Created Loan Account Number:' + acc + ' Details for Disbursment Proposal Details.',
                                'success'
                            )
                            $('#view-all-ProposalDetails').html(res.html);
                            $('#modelProposalDetails').modal('hide');
                            $('#modelProposalDetails .modal-body').html('');
                            $('#modelProposalDetails .modal-title').html('');
                            ReloadProposalDetails();
                            break;
                        }
                        else
                            $('#modelProposalDetails .modal-body').html(res.html);
                        break;
                    case "LoanAllocation":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Allocation Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-Allocation').html(res.html)
                            $('#modelLoanAllocationDetails .modal-body').html('');
                            $('#modelLoanAllocationDetails .modal-title').html('');
                            $('#modelLoanAllocationDetails').modal('hide');
                            ReloadAllocationDetails();
                            break;
                        }
                        else
                            $('#modelLoanAllocationDetails .modal-body').html(res.html);
                        break;
                    default:
                        break;

                }

            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

function AjaxSingleTabDeleteMethod(url, Module) {
    //debugger;
    swal.fire({
        title: 'Are you sure?',
        text: "You want to delete this record!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            try {
                $.ajax({
                    type: 'POST',
                    url: url,
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        swal.fire(
                            'Deleted!',
                            'Record successfully is Deleted.',
                            'success'
                        )
                        switch (Module) {
                            case "AuditClearence":
                                $('#view-all-Audit').html(res.html);
                                ReloadAuditDetails();
                                break;
                            case "RecommendDisbursement":
                                $('#view-all-RecommDisbursementDetails').html(res.html)
                                ReloadRecommendedDisbursementDetails();
                                break;
                            case "Proposaldetails":
                                $('#view-all-ProposalDetails').html(res.html);
                                ReloadProposalDetails();
                                break;
                            case "OtherDebits":
                                $('#view-all-OtherDebits').html(res.html);
                                ReloadOtherDebitDetails();
                                break;
                            case "LoanAllocation":
                                $('#view-all-Allocation').html(res.html);
                                ReloadAllocationDetails();
                                break;
                            case "deletecharge":
                                $("#view-all-modelChargeDetails").html(res.html)
                                //$('#modelProposalDetails .modal-body').html(res);
                                //$('#modelProposalDetails').modal('show');
                                ReloadChargeDetails();
                                break;
                            default:
                                break;

                        }

                    },
                    error: function (err) {
                        console.log(err)
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

}

function AjaxSingleTabUpdateMethod(form, Module) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                debugger;
                var acc = res.data;
                switch (Module) {
                    case "AuditClearence":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + '  Details for Audit Clearance.',
                                'success'
                            )
                            $('#view-all-Audit').html(res.html)
                            $('#modelAuditClearanceDetails').modal('hide');
                            $('#modelAuditClearanceDetails .modal-body').html('');
                            $('#modelAuditClearanceDetails .modal-title').html('');
                            ReloadAuditDetails();
                            break;
                        }
                        else
                            $('#modelAuditClearanceDetails .modal-body').html(res.html);
                        break;

                    case "OtherDebit":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Other Debit Details.',
                                'success'
                            )

                            $('#view-all-OtherDebits').html(res.html)
                            $('#modelOtherDebitsDetails .modal-body').html('');
                            $('#modelOtherDebitsDetails .modal-title').html('');
                            $('#modelOtherDebitsDetails').modal('hide');
                            ReloadOtherDebitDetails();
                            break;
                        }
                        else
                            $('#modelOtherDebitsDetails .modal-body').html(res.html);
                        break;

                    case "RecommendDisbursement":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Recommended Disbursement Details.',
                                'success'
                            )
                            $('#view-all-RecommDisbursementDetails').html(res.html)
                            $('#modelRecommDisbursementDetails .modal-body').html('');
                            $('#modelRecommDisbursementDetails .modal-title').html('');
                            $('#modelRecommDisbursementDetails').modal('hide');
                            ReloadRecommendedDisbursementDetails();
                            break;
                        }
                        else
                            $('#modelRecommDisbursementDetails .modal-body').html(res.html);
                        break;
                    case "Proposaldetails":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + '  Details for Disbursment Proposal Details.',
                                'success'
                            )
                            $('#view-all-ProposalDetails').html(res.html)
                            $('#modelProposalDetails').modal('hide');
                            $('#modelProposalDetails .modal-body').html('');
                            $('#modelProposalDetails .modal-title').html('');
                            ReloadProposalDetails();
                            break;
                        }
                        else
                            $('#modelProposalDetails .modal-body').html(res.html);
                        break;

                    case "LoanAllocation":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated  Allocation Details for this ' + acc + ' Loan Account Number ',
                                'success'
                            )
                            $('#view-all-Allocation').html(res.html)
                            $('#modelLoanAllocationDetails .modal-body').html('');
                            $('#modelLoanAllocationDetails .modal-title').html('');
                            $('#modelLoanAllocationDetails').modal('hide');
                            ReloadAllocationDetails();
                            break;
                        }
                        else
                            $('#modelLoanAllocationDetails .modal-body').html(res.html);
                        break;

                    default:
                        break;
                }

            },
            error: function (err) {
                console.log(err)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Edit Method - Added By;Gagana K, Change of Unit Information
function JqueryCUIEditMethod(form, Module) {
    debugger
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                var acc = res.data;
                switch (Module) {
                    case "ChangeAddress":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Change Address.',
                                'success'
                            )
                            $('#view-all-address').html(res.html)
                            $('#modelChangeLocation .modal-body').html('');
                            $('#modelChangeLocation .modal-title').html('');
                            $('#modelChangeLocation').modal('hide');
                            ReloadAddressDetails();
                            break;
                        }
                        else
                            $('#modelChangeLocation .modal-body').html(res.html);
                        break;
                    case "ChangeBankDetails":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for ChangeBankDetails.',
                                'success'
                            )
                            $('#view-all-ChangeBankDetails').html(res.html)
                            $('#modelChangeBankDetails .modal-body').html('');
                            $('#modelChangeBankDetails .modal-title').html('');
                            $('#modelChangeBankDetails').modal('hide');
                            ReloadChangeBankDetails();
                            break;
                        }
                        else
                            $('#modelChangeBankDetails .modal-body').html(res.html);
                        break;
                    case "PromoterDetails":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Promoter Profile.',
                                'success'
                            )
                            $('#view-all-ChangePromoterProfile').html(res.html)
                            $('#modelChangePromoterProfile .modal-body').html('');
                            $('#modelChangePromoterProfile .modal-title').html('');
                            $('#modelChangePromoterProfile').modal('hide');
                            ReloadChangePromoterProfileDetails();
                            break;
                        }
                        else
                            $('#modelChangePromoterProfile .modal-body').html(res.html);
                        break;
                    case "PromoterAddress":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Promoter Address.',
                                'success'
                            )
                            $('#view-all-ChangePromoterAddress').html(res.html);
                            $('#modelChangePromoterAddress').modal('hide');
                            $('#modelChangePromoterAddress .modal-body').html('');
                            $('#modelChangePromoterAddress .modal-title').html('');
                            ReloadPromoterAddressDetails();
                            break;
                        }
                        else
                            $('#modelChangePromoterAddress .modal-body').html(res.html);
                        break;
                    case "PromoterBankInfo":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated LoanAccountNumber : ' + acc + '  Details for Promoter Bank Information.',
                                'success'
                            )
                            $('#view-all-ChangePromoterBankInfo').html(res.html)
                            $('#modelChangePromoterBankInfo .modal-body').html('');
                            $('#modelChangePromoterBankInfo .modal-title').html('');
                            $('#modelChangePromoterBankInfo').modal('hide');
                            ReloadChangePromoterBankDetails();
                            break;
                        }
                        else
                            $('#modelChangePromoterBankInfo .modal-body').html(res.html);
                        break;
                    case "AssetDetails":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated LoanAccountNumber : ' + acc + ' Details for  Asset Information.',
                                'success'
                            )
                            $('#view-all-ChangeAssetInformation').html(res.html)
                            $('#modelChangeAssetInformation').modal('hide');
                            $('#modelChangeAssetInformation .modal-body').html('');
                            $('#modelChangeAssetInformation .modal-title').html('');
                            ReloadAssetDetails();
                            break;
                        }
                        else
                            $('#modelChangeAssetInformation .modal-body').html(res.html);
                        break;
                    case "LiabilityInformation":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Change in Liability Information.',
                                'success'
                            )
                            $('#view-all-ChangeLiabilityInfo').html(res.html)
                            $('#modelChangeLiabilityInfo .modal-body').html('');
                            $('#modelChangeLiabilityInfo .modal-title').html('');
                            $('#modelChangeLiabilityInfo').modal('hide');
                            ReloadPromoterLiabilityInfo();
                            break;
                        }
                        else
                            $('#modelChangeLiabilityInfo .modal-body').html(res.html);
                        break;
                    case "Product":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated LoanAccountNumber : ' + acc + '  Details for Product Details.',
                                'success'
                            )
                            $('#view-all-ChangeProductDetails').html(res.html)
                            $('#modelChangeProductDetails').modal('hide');
                            $('#modelChangeProductDetails .modal-body').html('');
                            $('#modelChangeProductDetails .modal-title').html('');
                            ReloadProductDetails();
                        }
                        else
                            $('#modelChangeProductDetails .modal-body').html(res.html);
                        break;
                    
                    default:
                        break;
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Add Method - Added By;Gagana K, Change of Unit Information
function JqueryCUIAddMethod(form, Module) {
    debugger
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                var acc = res.data;
                switch (Module) {
                    
                    case "ChangeBankDetails":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Bank Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-ChangeBankDetails').html(res.html)
                            $('#modelChangeBankDetails .modal-body').html('');
                            $('#modelChangeBankDetails .modal-title').html('');
                            $('#modelChangeBankDetails').modal('hide');
                            ReloadChangeBankDetails();
                            break;
                        }
                        else
                            $('#modelChangeBankDetails .modal-body').html(res.html);
                        break;
                    case "PromoterDetails":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Promoter Profile Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-ChangePromoterProfile').html(res.html)
                            $('#modelChangePromoterProfile .modal-body').html('');
                            $('#modelChangePromoterProfile .modal-title').html('');
                            $('#modelChangePromoterProfile').modal('hide');
                            ReloadChangePromoterProfileDetails();
                            break;
                        }
                        else
                            $('#modelChangePromoterProfile .modal-body').html(res.html);
                        break;
                    case "PromoterAddress":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Promoter Address Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-ChangePromoterAddress').html(res.html);
                            $('#modelChangePromoterAddress').modal('hide');
                            $('#modelChangePromoterAddress .modal-body').html('');
                            $('#modelChangePromoterAddress .modal-title').html('');
                            ReloadPromoterAddressDetails();
                            break;
                        }
                        else
                            $('#modelChangePromoterAddress .modal-body').html(res.html);
                        break;
                    case "PromoterBankInfo":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Promoter Bank Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-ChangePromoterBankInfo').html(res.html)
                            $('#modelChangePromoterBankInfo .modal-body').html('');
                            $('#modelChangePromoterBankInfo .modal-title').html('');
                            $('#modelChangePromoterBankInfo').modal('hide');
                            ReloadChangePromoterBankDetails();
                            break;
                        }
                        else
                            $('#modelChangePromoterBankInfo .modal-body').html(res.html);
                        break;
                    case "AssetDetails":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Assets Information are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-ChangeAssetInformation').html(res.html)
                            $('#modelChangeAssetInformation').modal('hide');
                            $('#modelChangeAssetInformation .modal-body').html('');
                            $('#modelChangeAssetInformation .modal-title').html('');
                            ReloadAssetDetails();
                            break;
                        }
                        else
                            $('#modelChangeAssetInformation .modal-body').html(res.html);
                        break;
                    case "LiabilityInformation":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Change in Liability Information are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-ChangeLiabilityInfo').html(res.html)
                            $('#modelChangeLiabilityInfo .modal-body').html('');
                            $('#modelChangeLiabilityInfo .modal-title').html('');
                            $('#modelChangeLiabilityInfo').modal('hide');
                            ReloadPromoterLiabilityInfo();
                        }
                        else
                            $('#modelChangeLiabilityInfo .modal-body').html(res.html);
                        break;
                    case "Product":
                        if (res.isValid) {
                            swal.fire(
                                'Created',
                                'New Product Details are Added to this Loan Number : ' + acc + '.',
                                'success'
                            )
                            $('#view-all-ChangeProductDetails').html(res.html)
                            $('#modelChangeProductDetails').modal('hide');
                            $('#modelChangeProductDetails .modal-body').html('');
                            $('#modelChangeProductDetails .modal-title').html('');
                            ReloadProductDetails();
                        }
                        else
                            $('#modelChangeProductDetails .modal-body').html(res.html);
                        break;

                    default:
                        break;

                }

            },
            error: function (err) {
                console.log(err)
            }
        })
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Edit Method - Added By:Dev Patel; Module:CreationOfSecurityAndAcquisitionOfAssets
function JqueryCSAAEditMethod(form, Module) {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                var acc = res.data;
                switch (Module) {
                    case "LandAcquisition":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Land Acquisition.',
                                'success'
                            )
                            $('#view-all-LandAcquisition').html(res.html)
                            $('#modelLandAcquisitionDetails .modal-body').html('');
                            $('#modelLandAcquisitionDetails .modal-title').html('');
                            $('#modelLandAcquisitionDetails').modal('hide');
                            ReloadLandAcquisitionDetails();
                            break;
                        }
                        else
                            $('#modelLandAcquisitionDetails .modal-body').html(res.html);
                        break;
                    case "BuildingAcquisition":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Building Acquisition.',
                                'success'
                            )
                            $('#view-all-BuildingAcquisition').html(res.html)
                            $('#modelBuildingAcquisitionDetails .modal-body').html('');
                            $('#modelBuildingAcquisitionDetails .modal-title').html('');
                            $('#modelBuildingAcquisitionDetails').modal('hide');
                            ReloadBuildingAcquisitionDetails();
                            break;
                        }
                        else
                            $('#modelBuildingAcquisitionDetails .modal-body').html(res.html);
                        break;
                    case "MachineryAcquisition":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Machinery Acquisition.',
                                'success'
                            )
                            $('#view-all-MachineryAcquisition').html(res.html)
                            $('#modelMachineryAcquisitionDetails .modal-body').html('');
                            $('#modelMachineryAcquisitionDetails .modal-title').html('');
                            $('#modelMachineryAcquisitionDetails').modal('hide');
                            ReloadMachineryAcquisitionDetails();
                        }
                        else
                            $('#modelMachineryAcquisitionDetails .modal-body').html(res.html);
                        break;
                    case "FurnitureAcquisition":
                        if (res.isValid) {
                            swal.fire(
                                'Updated',
                                'Updated Loan Account Number : ' + acc + ' Details for Furniture Acquisition.',
                                'success'
                            )
                            $('#view-all-FurnitureAcquisition').html(res.html)
                            $('#modelFurnitureAcquisitionDetails .modal-body').html('');
                            $('#modelFurnitureAcquisitionDetails .modal-title').html('');
                            $('#modelFurnitureAcquisitionDetails').modal('hide');
                            ReloadFurnitureAcquisitionDetails();
                        }
                        else
                            $('#modelFurnitureAcquisitionDetails .modal-body').html(res.html);
                        break;
                    default:
                        break;
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

//Delete Method - Added By;Gagana K, Change of Unit Information
function JqueryCUIDeleteMethod(url, Module) {
    swal.fire({
        title: 'Are you sure?',
        text: "You want to delete this record!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            try {
                $.ajax({
                    type: 'POST',
                    url: url,
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        swal.fire(
                            'Deleted!',
                            'Record successfully is Deleted.',
                            'success'
                        )
                        switch (Module) {
                            case "ChangeBankDetails":
                                $('#view-all-ChangeBankDetails').html(res.html);
                                ReloadChangeBankDetails();
                                break;
                            case "PromoterProfile":
                                $('#view-all-ChangePromoterProfile').html(res.html);
                                ReloadChangePromoterProfileDetails();
                                break;
                            case "PromoterAddress":
                                $('#view-all-ChangePromoterAddress').html(res.html);
                                ReloadPromoterAddressDetails();
                                break;
                            case "PromoterBankInfo":
                                $('#view-all-ChangePromoterBankInfo').html(res.html);
                                ReloadChangePromoterBankDetails();
                                break;
                            case "AssetDetails":
                                $('#view-all-ChangeAssetInformation').html(res.html);
                                ReloadAssetDetails();
                                break;
                            case "LiabilityInformation":
                                $('#view-all-ChangeLiabilityInfo').html(res.html);
                                ReloadPromoterLiabilityInfo();
                                break;
                            case "Product":
                                $('#view-all-ChangeProductDetails').html(res.html);
                                ReloadProductDetails();
                                break;
                            default:
                                break;

                        }

                    },
                    error: function (err) {
                        console.log(err)
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

}

//Delete Method Added By:Dev Patel; Module: CreationOfSecurityAndAcquisitionOfAssets
function JqueryCSAADeleteMethod(url, Module) {
    swal.fire({
        title: 'Are you sure?',
        text: "You want to delete this record!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            try {
                $.ajax({
                    type: 'POST',
                    url: url,
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        swal.fire(
                            'Deleted!',
                            'Record is successfully Deleted.',
                            'success'
                        )
                        switch (Module) {
                            case "LandAcquisition":
                                $('#view-all-LandAcquisition').html(res.html);
                                ReloadLandAcquisitionDetails();
                                break;
                            case "BuildingAcquisition":
                                $('#view-all-BuildingAcquisition').html(res.html);
                                ReloadBuildingAcquisitionDetails();
                                break;
                            case "MachineryAcquisition":
                                $('#view-all-MachineryAcquisition').html(res.html);
                                ReloadMachineryAcquisitionDetails();
                                break;
                            case "FurnitureAcquisition":
                                $('#view-all-FurnitureAcquisition').html(res.html);
                                ReloadFurnitureAcquisitionDetails();
                                break;
                            default:
                                break;
                        }
                    },
                    error: function (err) {
                        console.log(err)
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
}

// Approve Method Added By:Dev Patel; Module: Loan Accounting
function ApprRejctPayment(Id, PaymentId, ReceiptId, Module) {
    var Message;
    var Url;
    debugger
    if (Module == "Approve") {
        Message =  "<h5 style='color:red'> Action can't be reverted after Approved </h5>";
        Url =  '/Admin/SavedReceipt/Approve'
    }
    else
    {
        Message = "<h5 style='color:red'> Action can't be reverted after Rejected </h5>";
        Url = '/Admin/SavedReceipt/Reject'
    }
    debugger
    swal.fire({

        title: Module,
        html: Message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var formData = new FormData();
            formData.append("TblLaPaymentDet.PaymentRefNo", Id);
            formData.append("PaymentId", parseInt(PaymentId));
            formData.append("ReceiptId", parseInt(ReceiptId));
            try {
                $.ajax({
                    type: 'POST',
                    data: formData,
                    url: Url,
                    contentType: false,
                    processData: false,
                    success: function (res) {
                        if (res.isValid) {
                            if (Module == "Approve") {
                                swal.fire(
                                    'Approved!',
                                    'Payment is successfully Approved.',
                                    'success'
                                )
                            }
                            else {
                                swal.fire(
                                    'Rejected!',
                                    'Payment is successfully Rejected.',
                                    'success'
                                )
                            }
                        }
                        $('#view-all-ReceiptPayment').html(res.html)
                        $('#modelReceiptPaymentDetails .modal-body').html('');
                        $('#modelReceiptPaymentDetails .modal-title').html('');
                        $('#modelReceiptPaymentDetails').modal('hide');
                        ReloadSaveReceipts();
                    },
                    error: function (err) {
                        console.log(err)
                    }
                })
            } catch (ex) {
                console.log(ex)
            }
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            return false;
        }
        return false;
    })
}

function ReloadTblAssetList() {
    var dataTableAD;
    dataTableAD = $("tblAssetList").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}

function ReloadTblLiabilityList() {
    var dataTableAD;
    dataTableAD = $("tblLiabilityList").DataTable({
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:first", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "language": {
            "emptyTable": "No data found"
        }
    });
}


