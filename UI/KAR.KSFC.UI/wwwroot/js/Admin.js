var globalDelEmpURL;
var globalDelEmpTitle;
var flag;
var checkOutObjData = {};
var deleteRoleObjData = {}

$(function () {

    $("#btnSubmitSearchEmployee").on("click", function () {
        if (!($("#teyTicketNumber").val().trim() === "")) {
            $("#empSearchForm").submit();
        } else {
            alert("Search field cannot be left blank")
        }

    });

    $("body").delegate("#CommencementDate", "focusin", function () {
        $(this).datepicker({
            dateFormat: 'dd-mm-yy',
            changeMonth: true,
            changeYear: true,
            maxDate: '12-11-2050',
            minDate: '12-11-1988'
        });
    });

    $("body").delegate("#txtdob", "focusin", function () {
        $(this).datepicker({
            dateFormat: 'dd-mm-yy',
            changeMonth: true,
            changeYear: true,
            maxDate: '12-11-2050',
            minDate: '12-11-1988'
        });
    });

    $("body").delegate("#dtSbRankFromDate", "focusin", function () {
        $(this).datepicker({
            dateFormat: 'dd-mm-yy',
            changeMonth: true,
            changeYear: true,
        });
    });

    $("body").delegate("#dtInchargeFromDate", "focusin", function () {
        $(this).datepicker({
            dateFormat: 'dd-mm-yy',
            changeMonth: true,
            changeYear: true,
        });
    });

    $("body").delegate("#dtPersonalPromotionDate", "focusin", function () {
        $(this).datepicker({
            dateFormat: 'dd-mm-yy',
            changeMonth: true,
            changeYear: true,
        });
    });

    $("body").delegate("#dtfirstAppDate", "focusin", function () {
        $(this).datepicker({
            dateFormat: 'dd-mm-yy',
            changeMonth: true,
            changeYear: true,
        });
    });
});

/* Admin data table binding */

function EmployeeListDataTable() {
    //var dataTableASFY;
    //dataTableASFY = $('#tblEmployeeDetails').DataTable({
    //    //"columns": [
    //    //    { "data": "Sno" },
    //    //    { "data": "EmployeeNumber" },
    //    //    { "data": "EmployeeName" },
    //    //    { "data": "Gender" },
    //    //    { "data": "Designation" },
    //    //    { "data": "Mobile" },
    //    //    { "data": "Email" },
    //    //    { "data": "View" },
    //    //    { "data": "Edit" },
    //    //    { "data": "Delete" }],
    //    "language": {
    //        "emptyTable": "No data found, Please click on <b>Add New</b> Button"
    //    }
    //});
}

showInPopUpEmpDetails = (url, title, flag) => {

    $("#empNo").attr("readonly", true);
    $('#modalFooterKSFC').hide();

    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {
            $('#ksfcEmpDetails .modal-body').html(res);
            $('#ksfcEmpDetails .modal-title').html(title);
            $('#ksfcEmpDetails').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
            $("#empty1 :input").attr("disabled", true);
        },
        complete: function (res) {
            $('#ksfcEmpDetails .modal-body').html(res.responseText);
            $('#ksfcEmpDetails .modal-title').html(title);
            $('#ksfcEmpDetails').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
            $("#empty1 :input").attr("disabled", true);
        }
    })
}

showInPopUpEmp = (url, title) => {
    $('#modalFooterKSFC').show();
    $("#empNo").attr("readonly", false);
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {
            $('#ksfcEmpDetails .modal-body').html(res);
            $('#ksfcEmpDetails .modal-title').html(title);
            $('#ksfcEmpDetails').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        },
        complete: function (res) {
            $('#ksfcEmpDetails .modal-body').html(res.responseText);
            $('#ksfcEmpDetails .modal-title').html(title);
            $('#ksfcEmpDetails').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}

submitEmployeeForm = () => {

    $('#span_email').html('');
    $('#span_pan').html('');
    $('#span_mobile').html('');

    var valdata = $("#empForm").serialize();

    $.ajax({
        type: 'POST',
        url: '/Admin/Employee/SubmitDetails',
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: valdata,
        success: function (res) {
        },
        complete: function (res) {

            if (res.status == 200) {
                if (res.responseJSON.flag == true) {
                    $("#modalOneCloseBtn").click();
                    $("#ksfcAlertModalFooterBtn").hide();
                    $('#ksfcAlertModal .modal-body').html(flag == 'edit' ? 'Updated' : 'Created');
                    $('#ksfcAlertModal').modal('show');
                    $('.modal-dialog').draggable({
                        handle: ".modal-header"
                    });
                }
                else {
                    var data = res.responseJSON.message.split(',');
                    $.each(data, function (item, value) {
                        if (value == 'Mobile') {
                            $('#span_mobile').html('Mobile is already tagged with other employee');
                            $('#txtmobile').focus();
                        }
                        
                    });

                    $.each(data, function (item, value) {
                        if (value == 'Pan') {
                            $('#span_pan').html('Pan is already tagged with other employee');
                            $('#txtpan').focus();
                        }
                         
                    });

                    $.each(data, function (item, value) {
                        if (value == 'Email') {
                            $('#span_email').html('Email is already tagged with other employee');
                            $('#txtemail').focus();
                        }
                    });


                    if (data.length == 0) {
                        $('#span_email').html('');
                        $('#span_pan').html('');
                        $('#span_mobile').html('');
                    }
                   

                }

            }
            else {
                $('#ksfcEmpDetails .modal-body').html(res.responseText);
                $('#ksfcEmpDetails .modal-title').html("New Employee");
                $('#ksfcEmpDetails').modal('show');
                $('.modal-dialog').draggable({
                    handle: ".modal-header"
                });
            }

        }
    })
}

showDeleteDialogueBox = (url, title, empName) => {
    globalDelEmpURL = url;
    globalDelEmpTitle = title;
    $('#empNameBoldElement').html(empName);
}

$("#ksfcAlertModalFooterBtn").on('click', function () {
    let localURL = globalDelEmpURL;
    $.ajax({
        type: 'GET',
        url: localURL,
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {

        },
        complete: function (res) {
            window.location.reload();
        }
    })
});

$('#ksfcAssignOfficeBtnYes').on('click', function () {
    $.ajax({
        type: 'POST',
        url: "/Admin/AssignOffice/CheckOut",
        dataType: 'json',
        data: checkOutObjData,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {

        },
        complete: function (res) {
            window.location.reload();
        }
    })
})

showInPopUpEmpDetailsForUpdate = (url, title, mark) => {
    //$("#employeeListingSection").show();
    $('#modalFooterKSFC').show();
    $("#empNo").attr("readonly", true);
    flag = mark;
    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {
            console.log('res')
            $('#ksfcEmpDetails .modal-body').html(res);
            $('#ksfcEmpDetails .modal-title').html(title);
            $('#ksfcEmpDetails').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        },
        complete: function (res) {
            $('#ksfcEmpDetails .modal-body').html(res.responseText);
            $('#ksfcEmpDetails .modal-title').html(title);
            $('#ksfcEmpDetails').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}

validatePan = () => {
    var panEle = document.getElementById("txtpan");

    if (panEle.value === '') {
        alert('Please enter PAN for validation');
    } else {

    }
}

showInPopUpEmployeeDetails = (url, title, flag) => {
    switch (flag) {
        case 'add':
            $('#modalFooterKSFC').show();
            break;
        case 'view':
            $('#modalFooterKSFC').hide();
            break;
        default:
            $('#modalFooterKSFC').hide();
            break;
    }

    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {
            $('#ksfcEmpDetails .modal-body').html(res);
            $('#ksfcEmpDetails .modal-title').html(title);
            $('#ksfcEmpDetails').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        },
        complete: function (res) {
            $('#ksfcEmpDetails .modal-body').html(res.responseText);
            $('#ksfcEmpDetails .modal-title').html(title);
            $('#ksfcEmpDetails').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}

getChairs = (val) => {
    $("#ddlChair").html('');
    $.ajax({
        type: 'GET',
        url: "/Admin/AssignBranch/GetChairs/" + val,
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {

            $.each(res, function (i, item) {

                $("#ddlChair").append('<option value="' + item.value + '">' +
                    item.text + '</option>');

            });
        },
        complete: function (res) {
            console.log('complete', res)
        }
    })

}

checkIn = () => {
    var valdata = $("#assignBranchForm").serialize();

    $.ajax({
        type: 'POST',
        url: '/Admin/AssignBranch/SubmitAssignment',
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: valdata,
        success: function (res) {
            console.log('success ', res);
        },
        complete: function (res) {

            if (res.status == 200) {
                window.location.reload();
                //$("#modalOneCloseBtn").click();
                //$('#ksfcAlertModal .modal-body').html('Checked In');
                //$('#ksfcAlertModal').modal('show');
                //$('.modal-dialog').draggable({
                //    handle: ".modal-header"
                //});

            }
            else {
                $('#ksfcEmpDetails .modal-body').html(res.responseText);
                $('#ksfcEmpDetails .modal-title').html("Assign Office");
                $('#ksfcEmpDetails').modal('show');
                $('.modal-dialog').draggable({
                    handle: ".modal-header"
                });
            }

        }
    })
}

checkOut = (url, title, empId, officeId, chairCode, designationId) => {

    checkOutObjData.url = url;
    checkOutObjData.title = title;
    checkOutObjData.employeeId = empId;
    checkOutObjData.officeId = officeId;
    checkOutObjData.chairCode = chairCode;
    checkOutObjData.opsDesignationId = designationId;

}


//showInPopUpRoleMapping = (url, title, flag) => {

//    console.log('👍',url);
//    $.ajax({
//        type: 'GET',
//        url: '/Admin/RoleMapping/GetEmpDetailById/test1111',
//        dataType: 'json',
//        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
//        success: function (res) {
//            console.lof(res)
//            $('#ksfcEmpDetailsRoleMapping .modal-body').html(res);
//            $('#ksfcEmpDetailsRoleMapping .modal-title').html(title);
//            $('#ksfcEmpDetailsRoleMapping').modal('show');
//            $('.modal-dialog').draggable({
//                handle: ".modal-header"
//            });
//        },
//        complete: function (res) {
//            $('#ksfcEmpDetailsRoleMapping .modal-body').html(res.responseText);
//            $('#ksfcEmpDetailsRoleMapping .modal-title').html(title);
//            $('#ksfcEmpDetailsRoleMapping').modal('show');
//            $('.modal-dialog').draggable({
//                handle: ".modal-header"
//            });

//        }
//    })
//}



showEmpDtlsForRoleMapping = (url, title, flag) => {

    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {
            console.lof(res)
            $('#ksfcEmpDetailsRoleMapping .modal-body').html(res);
            $('#ksfcEmpDetailsRoleMapping .modal-title').html(title);
            $('#ksfcEmpDetailsRoleMapping').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        },
        complete: function (res) {
            $('#ksfcEmpDetailsRoleMapping .modal-body').html(res.responseText);
            $('#ksfcEmpDetailsRoleMapping .modal-title').html(title);
            $('#ksfcEmpDetailsRoleMapping').modal('show');
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });

        }
    })
}

getRoles = (val) => {
    $("#ddlRoles").html('');
    $.ajax({
        type: 'GET',
        url: "/Admin/RoleMapping/GetRoles/" + val,
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {

            $.each(res, function (i, item) {

                $("#ddlRoles").append('<option value="' + item.value + '">' +
                    item.text + '</option>');

            });
        },
        complete: function (res) {
            console.log('complete', res)
        }
    });
}

AssignRole = () => {
    const data = $("#assignRoleForm").serialize();

    $.ajax({
        type: 'POST',
        url: '/Admin/RoleMapping/AssignRole',
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: data,
        success: function (res) {
            console.log('success ', res);
        },
        complete: function (res) {

            if (res.status == 200) {
                $("#modalOneCloseBtn").click();

                const resultData = JSON.parse(res.responseText);

                if (resultData.responseData.statusCode === 200 && resultData.responseData.result === 1 && resultData.responseData.message === "duplicate") {
                    alert("Failure - Aleary exists");

                } else {

                    alert('Success - Done');
                }

                window.location.reload();

            }
            else {
                $('#ksfcEmpDetailsRoleMapping .modal-body').html(res.responseText);
                $('#ksfcEmpDetailsRoleMapping .modal-title').html("Role Mapping");
                $('#ksfcEmpDetailsRoleMapping').modal('show');
                $('.modal-dialog').draggable({
                    handle: ".modal-header"
                });
            }

        }
    })

}

DeleteRole = (empId, moduleId, roleId) => {
    deleteRoleObjData.EmployeeNumber = empId;
    deleteRoleObjData.ModuleId = moduleId;
    deleteRoleObjData.RoleId = roleId;

}

$("#deleteRoleBtn").on('click', function () {

    $.ajax({
        type: 'POST',
        url: "/Admin/RoleMapping/Delete",
        dataType: 'json',
        data: deleteRoleObjData,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {

        },
        complete: function (res) {
            window.location.reload();
        }
    })
});

isEmployeeNumberUnique = () => {
    var empNo = $('#empNo').val();
    if (empNo) {
        $.ajax({
            type: 'GET',
            url: '/Admin/Employee/IsEmployeeNumberUnique?empNo=' + empNo,
            dataType: 'json',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            success: function (res) {
            },
            complete: function (res) {
                if (res.responseJSON == false) {
                    $('#employee_number').html('Employee No already exists');
                    $('#empNo').focus();
                } else {
                    $('#employee_number').html('');
                }
            }
        });
    }
}