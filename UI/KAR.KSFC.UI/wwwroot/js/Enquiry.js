function AddDatepicker() {

    $("#txtProDateOfBirth").datepicker({
        dateFormat: 'dd-mm-yy',
        changeMonth: true,
        changeYear: true,
        maxDate: '12-11-1988',
        minDate: '12-11-2004'//,
        //yearRange: '-99:-18'
    });
}


var progressBarValidation = false;
var isCustomersDetailsSaved = false;



//Updating the User Active Time for Customer 
function UpdateActiveTime() {
    var counter = $("#UserActiveTimeSec").val();
    var counterMin = $("#UserActiveTimeMin").val();
    var counterHr = $("#UserActiveTimeHr").val();
    setInterval(function () {
        counter++;
        if (counter >= 60) {
            counterMin++;
            if (counterMin >= 60) {
                counterHr++;
                $("#UserActiveTimeHr").html(counterHr);
                counterMin = 0;
            }
            $("#UserActiveTimeMin").html(counterMin);
            counter = 0;
        }
        $("#UserActiveTimeSec").html(counter);
        //clearInterval(interval);
    }, 1000);
}

var selDistrictId1;
var selTalukaId1;
var selHobliId1;
var selVil1;
var casDistrictDDlflag;
var casvillageDDlflag;
var cashobliDDlflag;
var casTalukaDDlflag;



$(window).on('load', function () {
    $.ajax({
        url: "/Customer/Enquiry/getCascadeDDL",
        dataType: "json",
        type: "POST",
        success: function (data) {

            if (data.isValid == true) {
                selDistrictId1 = data.basicDetails.districtCd;
                selTalukaId1 = data.basicDetails.talukaCd;
                selHobliId1 = data.basicDetails.hobliCd;
                selVil1 = data.basicDetails.vilCd;

                casDistrictDDlflag = data.basicDetails.districtCd;
                casvillageDDlflag = data.basicDetails.vilCd;
                cashobliDDlflag = data.basicDetails.hobliCd;
                casTalukaDDlflag = data.basicDetails.talukaCd;
                $('#ddlDistrictEnq').find('option').remove();
                $('#ddlTalukaEnq').find('option').remove();
                $('#ddlHobliEnq').find('option').remove();
                $('#ddlVillageEnq').find('option').remove();

                $('#ddlDistrictEnq').append($('<option></option>').val(data.basicDetails.districtCd).html(data.basicDetails.district));
                $('#ddlTalukaEnq').append($('<option></option>').val(data.basicDetails.talukaCd).html(data.basicDetails.taluk));
                $('#ddlHobliEnq').append($('<option></option>').val(data.basicDetails.hobliCd).html(data.basicDetails.hobli));
                $('#ddlVillageEnq').append($('<option></option>').val(data.basicDetails.vilCd).html(data.basicDetails.villageName));
            } else {
                $(data.basicDetails).each(function (index, item) { // GETTING ERROR HERE
                    //$('#ddlDistrictEnq').append($('<option></option>').val(item.value).html(item.text));
                });
            }


        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () { })
});

//cascase on load to prefill and give specific loc based on selected loc
$("#ddlDistrictEnq").one("click", function () {
    $.ajax({
        url: "/Customer/Enquiry/getDistricForCascadeDDl",
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (casDistrictDDlflag == selDistrictId1) {
                // first remove the current options if any
                $('#ddlDistrictEnq').find('option').remove();
                $('#ddlDistrictEnq').append($('<option></option>').val("0").html("--Select District--"));
                // next iterate thru your object adding each option to the drop down\    
                $(data).each(function (index, item) { // GETTING ERROR HERE
                    $('#ddlDistrictEnq').append($('<option></option>').val(item.value).html(item.text));
                });
            }
        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () { });
});

$("#ddlTalukaEnq").one("click", function () {
    $.ajax({
        url: "/Customer/Home/PopulateSubLocationList",
        data: {
            locationType: "District",
            locationId: selDistrictId1,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (casTalukaDDlflag == selTalukaId1) {
                // first remove the current options if any
                $('#ddlTalukaEnq').find('option').remove();
                $('#ddlTalukaEnq').append($('<option></option>').val("0").html("--Select Taluka--"));
                // next iterate thru your object adding each option to the drop down\    
                $(data).each(function (index, item) { // GETTING ERROR HERE
                    $('#ddlTalukaEnq').append($('<option></option>').val(item.value).html(item.text));
                });
            }
        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () { });
});

$("#ddlHobliEnq").one("click", function () {
    $.ajax({
        url: "/Customer/Home/PopulateSubLocationList",
        data: {
            locationType: "Taluka",
            locationId: selTalukaId1,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (cashobliDDlflag == selHobliId1) {
                // first remove the current options if any
                $('#ddlHobliEnq').find('option').remove();
                $('#ddlHobliEnq').append($('<option></option>').val("0").html("--Select Hobli--"));
                // next iterate thru your object adding each option to the drop down\    
                $(data).each(function (index, item) { // GETTING ERROR HERE
                    $('#ddlHobliEnq').append($('<option></option>').val(item.value).html(item.text));
                });
            }
        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () { });
});

$("#ddlVillageEnq").one("click", function () {
    $.ajax({
        url: "/Customer/Home/PopulateSubLocationList",
        data: {
            locationType: "Hobli",
            locationId: selHobliId1,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            if (casvillageDDlflag == selVil1) {
                // first remove the current options if any
                $('#ddlVillageEnq').find('option').remove();
                $('#ddlVillageEnq').append($('<option></option>').val("0").html("--Select Village--"));
                // next iterate thru your object adding each option to the drop down\    
                $(data).each(function (index, item) { // GETTING ERROR HERE
                    $('#ddlVillageEnq').append($('<option></option>').val(item.value).html(item.text));
                });
            }
        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () { });
});


//Ajax call update Taluka on change of District drop down list
$("#ddlDistrictEnq").change(function () {
    PopulateTaluka();
});

function PopulateTaluka() {

    document.getElementById("ddlHobliEnq").innerHTML = "";
    document.getElementById("ddlVillageEnq").innerHTML = "";

    var selDistrictId = document.getElementById("ddlDistrictEnq").value;
    selDistrictId1 = selDistrictId;
    $.ajax({
        url: "/Customer/Home/PopulateSubLocationList",
        data: {
            locationType: "District",
            locationId: selDistrictId,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            // first remove the current options if any
            $('#ddlTalukaEnq').find('option').remove();
            $('#ddlTalukaEnq').append($('<option></option>').val("0").html("--Select Taluka--"));
            // next iterate thru your object adding each option to the drop down\    
            $(data).each(function (index, item) { // GETTING ERROR HERE
                $('#ddlTalukaEnq').append($('<option></option>').val(item.value).html(item.text));
            });


        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () { });
}

//Ajax call update Hobli on change of Taluka drop down list
$("#ddlTalukaEnq").change(function () {
    document.getElementById("ddlVillageEnq").innerHTML = "";

    var selTalukaId = document.getElementById("ddlTalukaEnq").value;
    selTalukaId1 = selTalukaId;
    $.ajax({
        url: "/Customer/Home/PopulateSubLocationList",
        data: {
            locationType: "Taluka",
            locationId: selTalukaId,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            // first remove the current options if any
            $('#ddlHobliEnq').find('option').remove();
            $('#ddlHobliEnq').append($('<option></option>').val("0").html("--Select Hobli--"));
            // next iterate thru your object adding each option to the drop down\    
            $(data).each(function (index, item) { // GETTING ERROR HERE
                $('#ddlHobliEnq').append($('<option></option>').val(item.value).html(item.text));
            });

        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () { });
});



//Ajax call update Village on change of Hobli drop down list
$("#ddlHobliEnq").change(function () {
    var selHobliId = document.getElementById("ddlHobliEnq").value;
    selHobliId1 = selHobliId;
    $.ajax({
        url: "/Customer/Home/PopulateSubLocationList",
        data: {
            locationType: "Hobli",
            locationId: selHobliId,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            // first remove the current options if any
            $('#ddlVillageEnq').find('option').remove();
            $('#ddlVillageEnq').append($('<option></option>').val("0").html("--Select Village--"));
            // next iterate thru your object adding each option to the drop down\    
            $(data).each(function (index, item) { // GETTING ERROR HERE
                $('#ddlVillageEnq').append($('<option></option>').val(item.value).html(item.text));
            });


        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () { });
});

$("#ddlVillageEnq").change(function () {
    var selVil = document.getElementById("ddlHobliEnq").value;
    selVil1 = selVil;
});

$('body').on('change', '#ddlfinancecatEnq', function () {
    var categoryId = $('#ddlfinancecatEnq').val();
    $.ajax({
        url: "/Customer/Home/PopulateFinanceTypeList",
        data: {
            categoryId: categoryId,
        },
        dataType: "json",
        type: "POST",
        success: function (data) {
            // first remove the current options if any
            $('#ddlfinanceTypeEnq').find('option').remove();
            $('#ddlfinanceTypeEnq').append($('<option></option>').val("0").html("--Select Finance Type--"));
            // next iterate thru your object adding each option to the drop down\
            $(data).each(function (index, item) { // GETTING ERROR HERE
                $('#ddlfinanceTypeEnq').append($('<option></option>').val(item.value).html(item.text));
            });
        },
        error: function (err) {
            console.log(err);
        }
    });
});

//Checks file format and size of Promoter photo and uploads id
function CheckPhotoAndUpload() {

    var files = $('#promoterPic').prop("files");
    formData = new FormData();
    formData.append("promoterPic", files[0]);
    if (files[0].size > 50000) {
        $("#ErrMsgPromoterPic").html("File size is greater than 50 KB");//
        $("#SucMsgPromoterPic").html("");
    }
    else
        $("#ErrMsgPromoterPic").html("");
    var validExtensions = ['jpg', 'png', 'jpeg']; //array of valid extensions
    var fileName = files[0].name;

    var fileNameExt = fileName.substr(fileName.lastIndexOf('.') + 1);
    if ($.inArray(fileNameExt, validExtensions) == -1) {
        $("#ErrMsgPromoterPic").html("Invalid file format. Please select a jpg or png file.");
        $("#SucMsgPromoterPic").html("");
        return false;
    }

    if (files[0].size < 50000 && $.inArray(fileNameExt, validExtensions) != -1) {

        jQuery.ajax({
            type: 'POST',
            url: "/Customer/Documents/UploadPhoto",
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
            },
            success: function (repo) {
                if (repo.status == "success") {
                    $("#SucMsgPromoterPic").html("File uploaded successfully");
                    $("#ErrMsgPromoterPic").html("");
                }
            },
            error: function () {
                alert("Error occurs");
            }
        });
    }
}

function CheckAccordianForUnitTab(show, invalidAccordion) {
    if (show) {
        if (invalidAccordion == "BasicDetails") {
            $("#basic_accor_btn").addClass('collapsed').attr('aria-expanded', true);
            $("#collapseOne").addClass('show');

            $("#bank_accor_btn").addClass('collapsed').attr('aria-expanded', true);
            $("#collapseThree").addClass('show');

            //hide registration
            $("#regd_accor_btn").removeClass('collapsed').attr('aria-expanded', false);
            $("#collapseFour").removeClass('show');

            //hide address
            $("#add_accor_btn").removeClass('collapsed').attr('aria-expanded', false);
            $("#collapseTwo").removeClass('show');
        }
        if (invalidAccordion == "Address") {
            $("#add_accor_btn").addClass('collapsed').attr('aria-expanded', true);
            $("#collapseTwo").addClass('show');

            //hide basic and bank 
            $("#basic_accor_btn").removeClass('collapsed').attr('aria-expanded', false);
            $("#collapseOne").removeClass('show');
            $("#bank_accor_btn").removeClass('collapsed').attr('aria-expanded', false);
            $("#collapseThree").removeClass('show');

            //hide registration
            $("#regd_accor_btn").removeClass('collapsed').attr('aria-expanded', false);
            $("#collapseFour").removeClass('show');
        }

        if (invalidAccordion == "Bank") {
            $("#bank_accor_btn").addClass('collapsed').attr('aria-expanded', true);
            $("#collapseThree").addClass('show');
        }
        if (invalidAccordion == "Registration") {
            $("#regd_accor_btn").addClass('collapsed').attr('aria-expanded', true);
            $("#collapseFour").addClass('show');

            //hide basic and bank 
            $("#basic_accor_btn").removeClass('collapsed').attr('aria-expanded', false);
            $("#collapseOne").removeClass('show');
            $("#bank_accor_btn").removeClass('collapsed').attr('aria-expanded', false);
            $("#collapseThree").removeClass('show');

            //hide address
            $("#add_accor_btn").removeClass('collapsed').attr('aria-expanded', false);
            $("#collapseTwo").removeClass('show');
        }
    }

}
//Validates Unit Details tab onClick Save Unit Details and Continue button to save new record
$("#btnTabUnitDetails").click(function (event) {
    var formUDPD = document.getElementById("UnitDetailPersonalDetails");// $("#UnitDetailPersonalDetails").val();
    $.validator.unobtrusive.parse(formUDPD);
    if ($(formUDPD).valid()) {
        var formUDBD = document.getElementById("UnitDetailBankDetails");// $("#UnitDetailBankDetails");
        $.validator.unobtrusive.parse(formUDBD);
        $("#UnitDetailBankDetails").validate({
            //ignore: false
        });
        if ($(formUDBD).valid()) {
            event.preventDefault();
            //Save Bank Details to Session


            //Save Personal Details and Bank Details to Session and Validate completion of Unit Details Tab
            var UnitDetailPersonalDetails = $("#UnitDetailPersonalDetails").serialize();
            var UnitDetailBankDetails = $("#UnitDetailBankDetails").serialize();
            var dataString = $("#UnitDetailPersonalDetails, #UnitDetailBankDetails").serialize();
            $.ajax({
                async: false,
                type: "POST",
                //contentType: "application/json; charset=utf-8",
                datatype: 'json',
                url: GetRoute('/Enquiry/SaveOrEditUnitDetailsForEnquiry'),
                data: dataString,
                //data: UnitDetailPersonalDetails, UnitDetailBankDetails,
                success: function (data) {
                    if (data.isValid == true) {
                        $("#unitDetailsProgressBar").attr('class', 'progress-bar bg-success');
                        $("#Promoter-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                        $("#Promoter").attr('class', 'tab-pane fade active show');
                        $("#Unit-tab").attr('class', 'nav-link');
                        $("#Unit-tab").attr('aria-selected', 'false');
                        $("#Unit").attr('class', 'tab-pane fade');
                        //Remove the error messages
                        $("#spanErrorMsg").html("");
                        $("#divEnquiryAlertPopup").html("");
                        window.scrollTo(0, 0);
                    }
                    else {
                       // document.getElementById("divEnquiryAlertPopup").className = "text-danger";
                        $("#unitDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                        //if (data.invalidAccordion == "Address") {
                        $("#spanErrorMsg").html(data.message);
                        $("#divEnquiryAlertPopup").html(data.message);
                        $("#modalAlertEnq").modal('show');
                        //CheckAccordianForUnitTab(true, data.invalidAccordion)

                    }

                },
                error: function (err) {
                    $("#unitDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                    console.log(err);
                }
            }).done(function () {

            });
        }
        else {
            event.preventDefault();
            progressBarValidation = true
            //$('#UnitDetailBankDetails').html(form0.html);
            $("#unitDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            $("#spanErrorMsg").html("Please enter bank details.");
            $("#divEnquiryAlertPopup").html("Please enter bank details.");
            $("#modalAlertEnq").modal('show');
            return false;
        }
        //event.preventDefault();
    } else {
        progressBarValidation = true
        $("#unitDetailsProgressBar").attr('class', 'progress-bar bg-danger');
        $("#spanErrorMsg").html("Please fill all sections before saving the details.");
        $("#divEnquiryAlertPopup").html("Please fill all sections before saving the details.");
        $("#modalAlertEnq").modal('show');
        event.preventDefault();
        return false;
    }

});

$('#btnHideModal').click(function () {
    $('#modalAlertEnq').modal('hide');
});
//promoter and guarentor save btn
$("#PAGsaveBTN").click(function (event) {

    async: false
    //Ajax call to send Prom and Guar data to API to save data to database starts here
    $.ajax({
        async: false,
        type: "POST",
        datatype: 'json',
        url: GetRoute('/Enquiry/SaveOrEditPromAndGuarDetailsForEnquiry'),
        // data: dataString,
        success: function (data) {
            if (data.isValid == true) {

                $("#PGDProgressBar").attr('class', 'progress-bar bg-success');
                $("#Associate-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Associate").attr('class', 'tab-pane fade active show');
                $("#Promoter-tab").attr('class', 'nav-link');
                $("#Promoter-tab").attr('aria-selected', 'false');
                $("#Promoter").attr('class', 'tab-pane fade');
                //Remove the error messages
                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);

            }
            else {
                $("#PGDProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }

        },
        error: function (err) {
            $("#PGDProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    }).done(function () {

    });
    //Ajax call to send Prom and Guar data to API to save data to database ends here

    //$("#PGDProgressBar").attr('class', 'progress-bar bg-success');

    //$("#Associate-tab").attr('class', 'nav-link active');
    //$("#Associate-tab").attr('aria-selected', 'true');
    //$("#Associate").attr('class', 'tab-pane fade active show');
    //$("#Promoter-tab").attr('class', 'nav-link');
    //$("#Promoter-tab").attr('aria-selected', 'false');
    //$("#Promoter").attr('class', 'tab-pane fade');

});
//Associate and sister concern save btn
function has_associate_sisterconcern() {
    if ($("#has_associate_sisterconcern_checked").prop('checked') == true) {
        $('.has_associate_sisterconcern').css('display', 'none')
    } else {
        $('.has_associate_sisterconcern').css('display', 'block')
    }
}
$('body').on('change', '#has_associate_sisterconcern_checked', function () {

    has_associate_sisterconcern();
})
has_associate_sisterconcern();
$("#ASCDsaveBTN").click(function (event) {
    if ($("#has_associate_sisterconcern_checked").prop('checked') == true) {
        try {
            $.ajax({
                async: false,
                type: 'POST',
                url: GetRoute('/Enquiry/UpdateAssociateSisterconcernChecked'),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.isValid == true) {
                        $("#ASCDDetailsProgressBar").attr('class', 'progress-bar bg-success');
                        $("#Project-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                        $("#Project").attr('class', 'tab-pane fade active show');
                        $("#Associate-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
                        $("#Associate").attr('class', 'tab-pane fade');
                        //Remove the error messages
                        $("#spanErrorMsg").html("");
                        $("#divEnquiryAlertPopup").html("");
                        window.scrollTo(0, 0);
                    }
                    else {
                        $("#ASCDDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                        $("#spanErrorMsg").html(res.message);
                        $("#divEnquiryAlertPopup").html(res.message);
                        $("#modalAlertEnq").modal('show');

                    }

                },
                error: function (err) {
                    $("#PGDProgressBar").attr('class', 'progress-bar bg-danger');
                    console.log(err)
                }
            })
        } catch (ex) {
            console.log(ex)
        }

        return false;

        async: false
        $.ajax({
            async: false,
            type: "POST",
            datatype: 'json',
            url: GetRoute('/Enquiry/UpdateAssociateSisterconcernChecked'),
            // data: dataString,
            success: function (data) {
                if (data.isValid == true) {
                    $("#ASCDDetailsProgressBar").attr('class', 'progress-bar bg-success');
                    $("#Project-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                    $("#Project").attr('class', 'tab-pane fade active show');
                    $("#Associate-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
                    $("#Associate").attr('class', 'tab-pane fade');
                    //Remove the error messages
                    $("#spanErrorMsg").html("");
                    $("#divEnquiryAlertPopup").html("");
                    window.scrollTo(0, 0);
                }
                else {
                    $("#ASCDDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                    $("#spanErrorMsg").html(data.message);
                    $("#divEnquiryAlertPopup").html(data.message);
                    $("#modalAlertEnq").modal('show');

                }

            },
            error: function (err) {
                $("#PGDProgressBar").attr('class', 'progress-bar bg-danger');
                console.log(err);
            }
        }).done(function () {

        });
    }
    else {
        async: false
        $.ajax({
            async: false,
            type: "POST",
            datatype: 'json',
            url: GetRoute('/Enquiry/SaveOrEditAssociateDetailsForEnquiry'),
            // data: dataString,
            success: function (data) {
                if (data.isValid == true) {
                    $("#ASCDDetailsProgressBar").attr('class', 'progress-bar bg-success');
                    $("#Project-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                    $("#Project").attr('class', 'tab-pane fade active show');
                    $("#Associate-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
                    $("#Associate").attr('class', 'tab-pane fade');
                    //Remove the error messages
                    $("#spanErrorMsg").html("");
                    $("#divEnquiryAlertPopup").html("");
                    window.scrollTo(0, 0);
                }
                else {
                    $("#ASCDDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                    $("#spanErrorMsg").html(data.message);
                    $("#divEnquiryAlertPopup").html(data.message);
                    $("#modalAlertEnq").modal('show');

                }

            },
            error: function (err) {
                $("#PGDProgressBar").attr('class', 'progress-bar bg-danger');
                console.log(err);
            }
        }).done(function () {

        });
    }

});



//Project Details btn
$("#PDsaveBTN").click(function (event) {
    var dataString = $("#workingCaptitalDetails").serialize();
    $.ajax({
        async: false,
        url: GetRoute('/Enquiry/SaveProjectDetails'),
        data: dataString,
        datatype: 'json',
        type: "POST",
        success: function (data) {
            if (data.isValid == true) {
                $("#ProjectDetailsProgressBar").attr('class', 'progress-bar bg-success');
                $("#Security-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#Security").attr('class', 'tab-pane fade active show');
                $("#Project-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
                $("#Project").attr('class', 'tab-pane fade');
                $("#spanErrorMsg").html("");
                $("#modalAlertEnq").modal('hide');
                window.scrollTo(0, 0);
            } else {
                $("#spanErrorMsg").html(data.message);
                $("#modalAlertEnq").modal('show');
                $("#divEnquiryAlertPopup").html(data.message);
                $("#ProjectDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            }
        },
        error: function (err) {
            $("#spanErrorMsg").html(data.message);
            $("#divEnquiryAlertPopup").html(data.message);
            $("#modalAlertEnq").modal('show');
            $("#ProjectDetailsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    }).done
        (function () {
        });
});

//Security And Documents Tab Save and Continue btn
$("#SADsaveBTN").click(function (event) {
    $.ajax({
        type: 'POST',
        async: false,
        url: GetRoute('/Enquiry/SaveSecurityAndDocsForEnquiry'),
    }).done(function (response) {
        if (response.isValid == true) {
            $("#SADetailsProgressBar").attr('class', 'progress-bar bg-success');
            $("#Review-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
            $("#Review").attr('class', 'tab-pane fade active show');
            $("#Security-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
            $("#Security").attr('class', 'tab-pane fade');
            $("#spanErrorMsg").html("");
            $("#modalAlertEnq").modal('hide');
            window.scrollTo(0, 0);
        } else {
            $("#spanErrorMsg").html(response.message);
            $("#modalAlertEnq").modal('show');
            $("#divEnquiryAlertPopup").html(response.message);
            $("#SADetailsProgressBar").attr('class', 'progress-bar bg-danger');
        }
    });
});

$('#Confirmation').click(function () {
    if ($('#Confirmation').is(':checked')) {
        $('#btnSubmitCompleteEnquiry').removeAttr('disabled');
    }
    else {
        $('#btnSubmitCompleteEnquiry').attr('disabled', '');
    }
});

$('#btnSubmitCompleteEnquiry').click(function () {

    if ($('#SubmissionNote').val()) {
        $.ajax({
            url: GetRoute("/Enquiry/SubmitEnquiry"),
            data: { sNote: $("#SubmissionNote").val() },
            dataType: "json",
            type: "Post",
            success: function (data) {
                status = data.isValid;
                resMessage = data.message;
                accordian = data.invalidAccordion;
                tab = data.tab;
            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            $('#btnSubmitCompleteEnquiry').prop('disabled', true);
            $('#Confirmation').prop("checked", false);
            if (status == 'false' && accordian == '') {
                $("#spanErrorMsg").html(resMessage);
                $("#divEnquiryAlertPopup").html(resMessage);
                $("#modalAlertEnq").modal('show');
                document.getElementById("spanErrorMsg").className = "text-danger";
              //  document.getElementById("divEnquiryAlertPopup").className = "text-danger";
            } else if (status == 'false' && accordian != '') {

                $("html, body").animate({ scrollTop: 0 }, "fast");
                $('.nav-link').removeClass('active');
                if (tab == 'Unit') {
                    $('#Unit-tab').click();
                    $("#unitDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                    CheckAccordianForUnitTab(true, accordian);
                }
                else if (tab == 'Promoter') {
                    $('#Promoter-tab').click();
                    $("#PGDProgressBar").attr('class', 'progress-bar bg-danger');
                }
                else if (tab == 'Project') {
                    $('#Project-tab').click();
                    $("#ProjectDetailsProgressBar").attr('class', 'progress-bar bg-danger');
                }
                else if (tab == 'Security') {
                    $('#Security-tab').click();
                    $("#SADetailsProgressBar").attr('class', 'progress-bar bg-danger');
                }

               // document.getElementById("divEnquiryAlertPopup").className = "text-danger";
                $("#divEnquiryAlertPopup").html(resMessage);
                $("#modalAlertEnq").modal('show');


            }
            else if (status == 'true') {
                $('#Confirmation').prop("checked", true);
                $('#Confirmation').prop("disabled", "disabled");
                $("#ReviewSetailsProgressBar").attr('class', 'progress-bar bg-success');
                $('#btnSubmitCompleteEnquiry').remove();
                $("#spanErrorMsg").html(resMessage);
                $("#divEnquiryAlertPopup").html(resMessage);
                $("#modalAlertEnq").modal('show');
                $("#ViewReview").attr('class', 'tab-pane fade active show');
                document.getElementById("spanErrorMsg").className = "text-success";
              //  document.getElementById("divEnquiryAlertPopup").className = "text-dark";
            }
        });
    }
    else {
      //  document.getElementById("divEnquiryAlertPopup").className = "text-danger";
        $("#divEnquiryAlertPopup").html("Please fill the summary Note.");
        $("#modalAlertEnq").modal('show');
    }
});
//All previous button onClick methods start here

//Review and submit tab
$("#btnpreviousSubmitCompleteEnquiry").click(function (event) {
    //Activate
    $("#Security-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Security").attr('class', 'tab-pane fade active show');
    //Deactivate
    $("#Review-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
    $("#Review").attr('class', 'tab-pane fade');
    window.scrollTo(0, 0);
});

//Security and documents
$("#SADpreviousBTN").click(function (event) {
    //Activate
    $("#Project-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Project").attr('class', 'tab-pane fade active show');
    //Deactivate
    $("#Security-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
    $("#Security").attr('class', 'tab-pane fade');
    window.scrollTo(0, 0);
});

//Project details
$("#PDpreviousBTN").click(function (event) {
    //Activate
    $("#Associate-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Associate").attr('class', 'tab-pane fade active show');
    //Deactivate
    $("#Project-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
    $("#Project").attr('class', 'tab-pane fade');
    window.scrollTo(0, 0);
});

//Associate /Sister Concern
$("#ASCDpreviousBTN").click(function (event) {
    //Activate
    $("#Promoter-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Promoter").attr('class', 'tab-pane fade active show');
    //Deactivate
    $("#Associate-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
    $("#Associate").attr('class', 'tab-pane fade');
    window.scrollTo(0, 0);
});
//Promoter and guaranter
$("#PAGpreviousBTN").click(function (event) {
    //Activate
    $("#Unit-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#Unit").attr('class', 'tab-pane fade active show');
    //Deactivate
    $("#Promoter-tab").attr('class', 'nav-link').attr('aria-selected', 'false');
    $("#Promoter").attr('class', 'tab-pane fade');
    window.scrollTo(0, 0);
});

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

//All previous button onClick methods ends here
function DeleteUploadFile(filetype) {

    var x = confirm("Are you sure you want to delete?");
    if (x) {
        var fileId = $('#Id_' + filetype).val();
        var type = $('#Type_' + filetype).val()
        var params = { fileId: fileId, documentType: type };
        if (fileId) {
            $.ajax({
                "async": true,
                "crossDomain": true,
                "url": GetRoute('/Documents/DeleteUploadFile'),
                "method": "DELETE",
                "data": params,
                success: function (data) {
                    if (data.status === "success") {
                        $('#Upload_' + filetype).show();
                        $('#Upload_' + filetype).removeAttr('disabled');
                        $('#Delete_' + filetype).addClass("invisible");
                        $('#View_' + filetype).addClass("invisible");
                        $('#File_' + filetype).removeAttr('disabled');
                        $('#File_' + filetype).removeAttr('style', 'display:none');
                        $('#File_' + filetype).val('');

                      //  document.getElementById("divEnquiryAlertPopup").className = "text-dark";
                        $("#divEnquiryAlertPopup").html('File Deleted Successfully!');
                        $("#modalAlertEnq").modal('show');
                    } else {
                        alert('Some error occured');
                    }
                }
            });
        }
    }
}
function ViewUploadFile(filetype) {
    var params = { fileId: $('#UniqueId_' + filetype).val() };
    if (params) {
        $.ajax({
            "async": true,
            "crossDomain": true,
            "url": GetRoute('/Documents/ViewUploadFile'),
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

function UploadDocument(filetype) {
    if ($('#File_' + filetype)[0].files[0]) {
        var fd = new FormData();
        fd.append('file', $('#File_' + filetype)[0].files[0]);
        fd.append('documentType', $('#Type_' + filetype).val());

        $.ajax({
            "async": true,
            "crossDomain": true,
            "url": GetRoute('/Documents/UploadFle'),
            "method": "POST",
            "processData": false,
            "contentType": false,
            "mimeType": "multipart/form-data",
            "data": fd,
            success: function (data) {
                var result = JSON.parse(data);
                if (result.status === "Success") {
                    $('#Upload_' + filetype).hide();
                    $('#Delete_' + filetype).removeClass("invisible");
                    $('#View_' + filetype).removeClass("invisible");
                    $('#File_' + filetype).attr('disabled', 'disabled');
                    $('#File_' + filetype).attr('style', 'display:none');
                    $('#Id_' + filetype).val(result.data.id);
                    $('#UniqueId_' + filetype).val(result.data.uniqueId);

                  //  document.getElementById("divEnquiryAlertPopup").className = "text-dark";
                    $("#divEnquiryAlertPopup").html('File Uploaded Successfully!');
                    $("#modalAlertEnq").modal('show');
                } else {
                   // document.getElementById("divEnquiryAlertPopup").className = "text-danger";
                    $("#divEnquiryAlertPopup").html(result.data);
                    $("#modalAlertEnq").modal('show');
                }
            }
        });
    } else {
      //  document.getElementById("divEnquiryAlertPopup").className = "text-danger";
        $("#divEnquiryAlertPopup").html('Please choose PDF file to upload');
        $("#modalAlertEnq").modal('show');
    }
}

$(document).ready(function () {
    $("#myTab li:eq(1) a").tab("show"); // show second tab (0-indexed, like an array)
});


//Get all enquiry information..
/*here sum stands for summary*/
function getEnquiryData() {

    // Enable Submit Button
    $("#btnSubmitCompleteEnquiry").attr("disabled", true);
    $('#Confirmation').prop("checked", false);

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getBasicDetails",//'@Url.Action("ResendOtpForReview","Enquiry")',//"/Customer/ReviewApplicationForm/getBasicDetails",
    }).done(function (response) {
        $('#sum-BasicDetails').html(response.html)
        $('#sum-BasicDetailsPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getAddressData",
    }).done(function (response) {
        $('#sum-Address').html(response.html)
        $('#sum-AddressPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getBankDetails",
    }).done(function (response) {
        $('#sum-BankDetails').html(response.html)
        $('#sumBankDetailsPrint').html(response.html)

    });
    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getDeclarationDetails",
    }).done(function (response) {
        $('#sumDeclarationDetails').html(response.html)
        $('#sumDeclarationDetailsPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getWorkingCapitalDetails",
    }).done(function (response) {
        $('#sumWorkingCapitalDetails').html(response.html)
        $('#sumWorkingCapitalDetailsPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getRegistrationData",
    }).done(function (response) {
        $('#sumRegistration').html(response.html)
        $('#sumRegistrationPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getPromoterData",
    }).done(function (response) {
        $('#sumPromoter').html(response.html)
        $('#sumPromoterPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getPromoterAssetData",
    }).done(function (response) {
        $('#sumPromoterAsset').html(response.html)
        $('#sumPromoterAssetPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getPromoterLiabilityData",
    }).done(function (response) {
        $('#sumPromoterLiability').html(response.html)
        $('#sumPromoterLiabilityPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getPromoterNetWorthData",
    }).done(function (response) {
        $('#sumPromoterNetWorth').html(response.html)
        $('#sumPromoterNetWorthPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getGuarantorData",
    }).done(function (response) {
        $('#sumGuarantor').html(response.html)
        $('#sumGuarantorPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getGuarentorAssetData",
    }).done(function (response) {
        $('#sumGuarentorAsset').html(response.html)
        $('#sumGuarentorAssetPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getGuarentorLiabilityData",
    }).done(function (response) {
        $('#sumGuarentorLiability').html(response.html)
        $('#sumGuarentorLiabilityPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getGuarentorNetWorthData",
    }).done(function (response) {
        $('#sumGuarentorNetWorth').html(response.html)
        $('#sumGuarentorNetWorthPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getAssociateData",
    }).done(function (response) {
        $('#sumAssociate').html(response.html)
        $('#sumAssociatePrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getPrevFYData",
    }).done(function (response) {
        $('#sumPrevFY').html(response.html)
        $('#sumPrevFYPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getProjectCostData",
    }).done(function (response) {
        $('#sumProjectCost').html(response.html)
        $('#sumProjectCostPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getProjectMeansOfFinanceData",
    }).done(function (response) {
        $('#sumProjectMeansOfFinance').html(response.html)
        $('#sumProjectMeansOfFinancePrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getProjectPrevFYData",
    }).done(function (response) {
        $('#sumProjectPrevFY').html(response.html)
        $('#sumProjectPrevFYPrint').html(response.html)

    });

    $.ajax({
        type: 'POST',
        url: "/Customer/ReviewApplicationForm/getSecurityData",
    }).done(function (response) {
        $('#sumSecurity').html(response.html)
        $('#sumSecurityPrint').html(response.html)

    });

    var file1 = $('#DetailedProjectReport').val();
    var file2 = $('#ProofofResidence').val();
    var file3 = $('#buildingcostestimates').val();
    var file4 = $('#existingmachinery').val();
    var file5 = $('#AuditedBalance').val();
    var file6 = $('#Taxreturnsfiled').val();
    var file7 = $('#LandandBuilding').val();
    var file8 = $('#onLease').val();
    //|| file2 == '' || file3 == '' || file4 == '' || file5 == '' || file6 == '' || file7 == '' || file8 == ''
    if (file1 == '') {
        $('#DPR').removeAttr('checked');
    } else {
        $('#DPR').attr('checked', 'checked');
    }

    if (file2 == '') {
        $('#POR').removeAttr('checked');
    } else {
        $('#POR').attr('checked', 'checked');
    }

    if (file3 == '') {
        $('#ASBP').removeAttr('checked');
    } else {
        $('#ASBP').attr('checked', 'checked');
    }

    if (file4 == '') {
        $('#LEM').removeAttr('checked');
    } else {
        $('#LEM').attr('checked', 'checked');
    }

    if (file5 == '') {
        $('#ABS').removeAttr('checked');
    } else {
        $('#ABS').attr('checked', 'checked');
    }

    if (file6 == '') {
        $('#COIT').removeAttr('checked');
    } else {
        $('#COIT').attr('checked', 'checked');
    }

    if (file7 == '') {
        $('#DRLB').removeAttr('checked');
    } else {
        $('#DRLB').attr('checked', 'checked');
    }

    if (file8 == '') {
        $('#LBTL').removeAttr('checked');
    } else {
        $('#LBTL').attr('checked', 'checked');
    }
}

//Get all enquiry information.. ends


//Window navigate away....function

window.onbeforeunload = function () {
    if (!isCustomersDetailsSaved) {
        return 'Unsaved changed will be lost.';
    }
}

//Jquey code Generate OTP and Resend OTP and verify otp for Enquiry Review Page starts here
//Create Timer for sending OTP--setting timeDefined before calling CreateTimer
function CreateTimer() {
    document.getElementById("spanTimeCountdownTimer").textContent = timeDefined;
    var timeLeft = timeDefined;
    $("#pMsgCountdownTimerEnq").css("display", "block");
    downloadTimer = setInterval(function () {
        timeLeft--;
        document.getElementById("spanTimeCountdownTimer").textContent = timeLeft;
        if (timeLeft <= 0) {
            clearInterval(downloadTimer);//call to clear timer, when Validate Otp button clicked by user
            $("#btnReviewResendOtp").css("display", "block");
            $("#btnReviewGenerateOtp").css("display", "none");
            $("#pMsgCountdownTimerEnq").css("display", "none");
            btnResOtpForMobile.disabled = false;
            txtMobileNo.disabled = false;
        }
    }, 1000);
}
function ClearTimer() {
    $("#pMsgCountdownTimerEnq").css("display", "none");
    clearInterval(downloadTimer);
}

var resMessageId = "0";
var resMessage = "";
var otpExpTimeLeft = 0;
var timeDefined = 15;
var downloadTimer = 5;
var messageInJson = "";
var mobileNumEnd = "";
$("#btnReviewResendOtp").attr("style", "display:none");
$("#txtReviewEntOtp").prop('disabled', true);
$("#btnReviewValOtp").prop('disabled', true);
$("#Confirmation").prop('disabled', true);
$("#btnReviewGenerateOtp").click(function () {

    if ($('#SubmissionNote').val()) {
        $.ajax({
            url: GetRoute("/Enquiry/GenerateOtpForReview"),
            //data: { mobileNo: txtMobileNo.value },
            dataType: "json",
            type: "Get",
            success: function (data) {
                resMessageId = data.id;
                resMessage = data.message;
            },
            error: function (err) {
                console.log(err);
            }
        }).done(function () {
            if (resMessageId == "0") {
                $("#spanErrorMsg").html(resMessage);
                $("#divEnquiryAlertPopup").html(resMessage);
                $("#modalAlertEnq").modal('show');
                document.getElementById("spanErrorMsg").className = "text-danger";
               // document.getElementById("divEnquiryAlertPopup").className = "text-danger";
            } else if (resMessageId == "1") {
                CreateTimer();
                $("#btnReviewGenerateOtp").css("display", "none");
                //$("#btnReviewResendOtp").css("display", "block");
                $("#pMsgCountdownTimerEnq").css("display", "block");
                // $("#spanErrorMsg").html(resMessage);
                $("#divEnquiryAlertPopup").html(resMessage);
                $("#modalAlertEnq").modal('show');
                document.getElementById("spanErrorMsg").className = "text-success";
               // document.getElementById("divEnquiryAlertPopup").className = "text-dark";
                document.getElementById("txtReviewEntOtp").disabled = false;
                document.getElementById("SubmissionNote").disabled = true;
            } else if (resMessageId == "2") {
                $("#btnReviewGenerateOtp").css("display", "none");
                $("#btnReviewResendOtp").css("display", "none");
                $("#pMsgCountdownTimerEnq").css("display", "none");
                $("#spanErrorMsg").html(resMessage);
                $("#divEnquiryAlertPopup").html(resMessage);
                $("#modalAlertEnq").modal('show');
                document.getElementById("spanErrorMsg").className = "text-danger";
               // document.getElementById("divEnquiryAlertPopup").className = "text-danger";
            }
        });
    } else {
        $("#divEnquiryAlertPopup").html('Submission note is required');
        $("#modalAlertEnq").modal('show');
       // document.getElementById("divEnquiryAlertPopup").className = "text-danger";
    }
});

$("#btnReviewResendOtp").click(function () {
    $.ajax({
        url: GetRoute("/Enquiry/ResendOtpForReview"),
        //data: { mobileNo: txtMobileNo.value },
        dataType: "json",
        type: "Get",
        success: function (data) {
            resMessageId = data.id;
            resMessage = data.message;
        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () {
        if (resMessageId == "0") {
            $("#spanErrorMsg").html(resMessage);
            $("#divEnquiryAlertPopup").html(resMessage);
            $("#modalAlertEnq").modal('show');
            document.getElementById("spanErrorMsg").className = "text-danger";
         //   document.getElementById("divEnquiryAlertPopup").className = "text-danger";
        } else if (resMessageId == "1") {
            CreateTimer();
            $("#btnReviewResendOtp").css("display", "none");
            $("#pMsgCountdownTimerEnq").css("display", "block");
            $("#spanErrorMsg").html(resMessage);
            $("#divEnquiryAlertPopup").html(resMessage);
            $("#modalAlertEnq").modal('show');
            document.getElementById("spanErrorMsg").className = "text-success";
          //  document.getElementById("divEnquiryAlertPopup").className = "text-dark";
            document.getElementById("txtReviewEntOtp").disabled = false;
            //document.getElementById("btnReviewResendOtp").disabled = true;
        } else if (resMessageId == "2") {
            $("#btnReviewGenerateOtp").css("display", "none");
            $("#btnReviewResendOtp").css("display", "none");
            $("#pMsgCountdownTimerEnq").css("display", "none");
            $("#spanErrorMsg").html(resMessage);
            $("#divEnquiryAlertPopup").html(resMessage);
            $("#modalAlertEnq").modal('show');
            document.getElementById("spanErrorMsg").className = "text-danger";
           // document.getElementById("divEnquiryAlertPopup").className = "text-danger";
        }
    });
});

//On keyup event for Enter otp text box
$("#txtReviewEntOtp").keyup(function () {
    //alert("s");
    var mobileOtpRegex = /^([0-9]{6})$/;
    //Removes characters other than numbersEnquiry/ValidateOtpForReview
    //document.getElementById("iconBtnValidateOtp").className = "";
    if (mobileOtpRegex.test($("#txtReviewEntOtp").val())) {
        //$("#txtEnterOtpForMbForm").css("border", "1px solid #00FF00");
        $("#btnReviewValOtp").prop('disabled', false);
    }
    else {
        $("#txtEnterOtpForMbForm").css("border", "1px solid #FF0000");
        $("#btnReviewValOtp").prop('disabled', true);
    }
});

$("#btnReviewValOtp").click(function () {
    $.ajax({
        url: GetRoute("/Enquiry/ValidateOtpForReview"),
        data: { entOtp: $("#txtReviewEntOtp").val() },
        dataType: "json",
        type: "Post",
        success: function (data) {
            resMessageId = data.id;
            resMessage = data.message;
        },
        error: function (err) {
            console.log(err);
        }
    }).done(function () {
        if (resMessageId == "0") {
            $("#spanErrorMsg").html(resMessage);
            $("#divEnquiryAlertPopup").html(resMessage);
            $("#modalAlertEnq").modal('show');
            document.getElementById("spanErrorMsg").className = "text-danger";
          //  document.getElementById("divEnquiryAlertPopup").className = "text-danger";
        } else if (resMessageId == "1") {
            ClearTimer();
            $("#Confirmation").prop('disabled', false);
            $("#btnReviewResendOtp").hide();
            $("#btnReviewValOtp").prop('disabled', true);
            $("#btnReviewValOtp").html('OTP verified');

            $('#txtReviewEntOtp').prop('disabled', 'disabled');
            $('#btnReviewResendOtp').prop('disabled', 'disabled');
            // $("#spanErrorMsg").html(resMessage);
            $("#divEnquiryAlertPopup").html(resMessage);
            $("#modalAlertEnq").modal('show');
            document.getElementById("spanErrorMsg").className = "text-success";
        //    document.getElementById("divEnquiryAlertPopup").className = "text-dark";
        } else if (resMessageId == "2") {
            $("#btnReviewGenerateOtp").css("display", "none");
            $("#btnReviewResendOtp").css("display", "none");
            $("#pMsgCountdownTimerEnq").css("display", "none");
            $("#spanErrorMsg").html(resMessage);
            $("#divEnquiryAlertPopup").html(resMessage);
            $("#modalAlertEnq").modal('show');
            document.getElementById("spanErrorMsg").className = "text-danger";
         //   document.getElementById("divEnquiryAlertPopup").className = "text-danger";
        }
    });
});

//$("#Confirmation").click(function () {
//    $("#btnpreviousSubmitCompleteEnquiry").prop('disabled', false);
//});

// code for generate , resend and verify ends here for revi

//print enquiry form
function printEnquiryForm() {
    var url = '@Url.Action("Details", "Branch", new { id = "__id__" })';
    window.location.href = url.replace('__id__', id);
}

function CallPrint(strid) {
    var prtContent = document.getElementById(strid);
    var WinPrint = window.open('', '', 'letf=100,top=100,width=600,height=600');
    WinPrint.document.write(`
          <html>
            <head>
            <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO" crossorigin="anonymous">
            </head>
            <style>

            </style>
        <body onload="window.print();window.close()">${prtContent.innerHTML}</body>
          </html>`
    );
    WinPrint.document.close();
}


$('body').on('focusout', '.promoterPan', function () {
    var $this = $(this);
    var pan = $this.val();
    if (pan != undefined && pan != '') {
        $.ajax({
            type: 'GET',
            url: GetRoute('/Promoter/CheckPromotorGurantorPanDuplicacy?pan=' + pan),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid && res.isdupicate) {
                    $this.css('border-color', 'red')
                    $('.promoterPanValidation').text(res.respMessage)
                    $('.promotor-btn').prop('disabled', true)
                    return false;
                }
                else {
                    $('.promotor-btn').prop('disabled', false)
                    $this.css('border-color', '')
                    $('.promoterPanValidation').text('')
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
    }
   
});
$('body').on('focusout', '.gurantorPan', function () {
    var $this = $(this);
    var pan = $this.val();
    if (pan != undefined && pan != '') {
        $.ajax({
            type: 'GET',
            url: GetRoute('/Promoter/CheckPromotorGurantorPanDuplicacy?pan=' + pan),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.isValid && res.isdupicate) {
                    $this.css('border-color', 'red')
                    $('.gurantorPanValidation').text(res.respMessage)
                    $('.gurantor-btn').prop('disabled', true)
                    return false;
                }
                else {
                    $('.gurantor-btn').prop('disabled', false)
                    $this.css('border-color', '')
                    $('.gurantorPanValidation').text('')
                }
            },
            error: function (err) {
                console.log(err)
            }
        })
    }
   
});
