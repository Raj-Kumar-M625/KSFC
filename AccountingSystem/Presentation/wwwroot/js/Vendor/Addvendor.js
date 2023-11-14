
var fileList = [];
var fileName = [];
var fielTypes = [];
document.addEventListener('DOMContentLoaded', function () {
    inputTypeFile = document.getElementById('FileUpload1');

    //documents = document.getElementById("documents");
    $(inputTypeFile).on('change', function () {

        var selectedFileValue = $('#filetype').val();

        fielTypes.push(selectedFileValue);
        console.log(fielTypes);

        if (selectedFileValue == "" | null) {
            Swal.fire({
                text: 'Please select the file type',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })

        }
        else {
            document.getElementById("uploadTable")
            $("#uploadTable").prop("hidden", false);
            var filename = this.files[0].name;
            var selFileName = (filename + "," + selectedFileValue)
            fileName.push(selFileName)
            fileList.push(this.files);
            console.log(fileList);

            $('#filetype').val('');
            if (this.files.length > 0) {
                for (var i = 0; i < this.files.length; i++) {
                    var table = document.getElementById("uploadTable");
                    var row = '<tr><td>' + filename + '</td> <td>' + selectedFileValue + '</td> <td ><i value="Delete" type="button" class="fa fa-trash fa1 ml-4"></i></td>  </tr>'

                    table.innerHTML = table.innerHTML + row;
                }
            }
            ;

            closeIT = document.getElementsByClassName('fa1')
            arrayFormCloseBtn = [...closeIT];
            arrayFormCloseBtn.forEach((onebyone) => {
                onebyone.addEventListener('click', function (e) {
                    var data2 = [];
                    var td = e.target.parentNode;
                    var tr = td.parentNode;
                    tr.parentNode.removeChild(tr);
                    var Cells = tr.getElementsByTagName("td");
                    var object = Cells[0].innerText;
                    var object1 = Cells[1].innerText;
                    console.log(fileList)
                    var flength = fileList.length;
                    for (i = 0; i < flength; i++) {
                        var data1 = fileList[i];
                        if (data1 != undefined) {


                            for (j = 0; j <= data1.length; j++) {



                                if (data1[j] != undefined) {


                                    var data2 = data1[j].name;

                                    if (data2 == object) {

                                        if (data1[j].name == object) {
                                            var spliced = fileList.splice(i, 1);

                                        }

                                    }
                                }


                            }

                        }

                    }
                    ;
                    var postFileName1 = fielTypes;
                    var fName = postFileName1.length;
                    for (i = 0; i < fName; i++) {
                        var data3 = postFileName1[i];
                        if (data3 == object1) {
                            const index = fielTypes.indexOf(data3);
                            fielTypes.splice(index, 1);
                        }
                    }



                    console.log(fileList)
                    console.log(fielTypes)






                })

            })
        }
    });
});

function Validate() {
    //.showFilesList = document.getElementById('showFilesList')
    var fileCount = document.getElementById("uploadTable").getElementsByTagName("tr").length

    if (fileCount > 5) {
        swal.fire({
            title: 'Maximum 5 files only allowed!!',
            icon: 'warning',

            confirmButtonText: 'Ok',
        })
        document.getElementById('FileUpload1').value = null;
        return false;

    }
}
$('#GST_TDS_Applicable').on('change', function () {
    if (this.checked) {
        $("#VendorDefaults_GST_TDSPercentage").val('2.00');
    } else {
        $("#VendorDefaults_GST_TDSPercentage").val('0.00');
    }

});
$('form').on('change', ':checkbox', function () {
    ;
    if (this.checked) {
        $(this).val(true);
    }
    else {
        $(this).val(false);
    }
});

$("#tdsScetion").on('change', function () {

    var selectedDetails = $("#tdsScetion").val().split(',');
    var CodeValue = selectedDetails[0];
    var percentage = selectedDetails[1];
    $("#tdspercentage").val(percentage);

    $("#tdssectionsvalue").val(CodeValue);

});
$("#IsExisting").on("click", function () {
    var IsExisting = document.getElementById("IsExisting");
    if (IsExisting.checked) {
        $("#OpeningDate").removeAttr("disabled", false);
        $("#OpeningBalance").removeAttr("disabled", false);
    }
    else {
        $("#OpeningDate").attr("disabled", true);
        $("#OpeningBalance").attr("disabled", true);
    }
});









//on page ready hide all branch option
$("#selBranchName").find('option').hide();
// set task as empty
$("#selBranchName").val('');
$("#selIfsc").val('');


// onchange of BankName Drop down
$("#selBankName").on('change', function () {

    var selectedBank = $("#selBankName").val();
    if (selectedBank != '') {
        //$("#selBranchName").find('option').hide();
        $("#selBranchName").val('');
        $("#selBranchName option[value='']").show();
        $('*[data="' + selectedBank + '"]').show();
        $("#selIfsc").val('');
        $("#selBankMasterId").val('');

        var selectedBankDetails = $("#selBankName").val().split(',');
        console.log(selectedBankDetails);
        var selectedbankId = selectedBankDetails[1];
        // var newUrl = '@Url.Action("AddVendor","Vendor")';
        $.ajax({
            type: 'get',
            url: '/Vendor/GetBranchDetails',
            data: { id: selectedbankId },
            success: function (data) {
                console.log(data)
                    ;

                $("#selBranchName").find('option:not(:first)').remove();
                var options = '';
                $.each(data, function (index, value) {

                    options += '<option value="' + value.branch_ifsc + ',' + value.branch_id + '">' + value.branch_name + '</option>';
                });
                $("#selBranchName").append(options);
            },
            error: function (result) {
                ;
                console.log(result)
            }
        });




    }
    else {
        // if BankName not selected then hide all tasks
        $("#selBranchName").find('option').hide();
        $("#selBranchName").val('');
        $("#selIfsc").val('');
    }

});

// onchange of BranchName Drop down
$("#selBranchName").on('change', function () {
    ;
    var selectedBranchDetails = $("#selBranchName").val().split(',');
    console.log(selectedBranchDetails);
    var selectedBranch = selectedBranchDetails[0];
    var BankMasterId = selectedBranchDetails[1];
    console.log(BankMasterId);
    if (selectedBranch != '') {
        $("#selIfsc").val(selectedBranch);
        $("#selBankMasterId").val(BankMasterId);
        var id = $("#selBankMasterId").val(BankMasterId);
        $("#branchid").val(BankMasterId);
    }
    else {
        // if BranchName not selected then hide all tasks
        $("#selIfsc").val('');
        $("#selBankMasterId").val('');
    }

});
$(function () {
    $("#GSTRegistration").change(function () {
        if ($(this).val() == "Registered") {
            $("#disableConfirmGst").removeAttr("disabled");
            $("#disableConfirmGst").focus();
            $("#GST_TDS_Applicable").removeAttr("disabled");
        } else {
            $("#disableConfirmGst").attr("disabled", "disabled");
            $("#GST_TDS_Applicable").attr("disabled", "disabled");
            $("#GST_TDS_Applicable").prop('checked', false);
            $("#VendorDefaults_GST_TDSPercentage").val('0.00');
        }
    });
});
$(function () {
    $("#GSTRegistration").change(function () {
        if ($(this).val() == "Registered") {
            $("#disableGst").removeAttr("disabled");
            $("#disableGst").focus();
            $("#GST_TDS_Applicable").removeAttr("disabled");

        } else {
            $("#disableGst").val('');
            $("#disableConfirmGst").val('');
            $("#disableGst").attr("disabled", "disabled");
            $("#GST_TDS_Applicable").prop('checked', false);
            $("#GST_TDS_Applicable").attr("disabled", "disabled");
            $("#VendorDefaults_GST_TDSPercentage").val('0.00');
        }
    });
});


$('#disableConfirmGst').on('change', function () {
    if ($('#disableGst').val() == $('#disableConfirmGst').val()) {
        $('#message').html('').css('color', 'green');
    } else
        $('#message').html('Please enter correct GSTIN number').css('color', 'red');
});

$('#confirmaccountNo').on('change', function () {
    if ($('#accountNo').val() == $('#confirmaccountNo').val()) {
        $('#message2').html('').css('color', 'green');
    } else
        $('#message2').html('Please enter correct Account number').css('color', 'red');
});

$('#confirmpanNo').on('change', function () {
    if ($('#panNo').val() == $('#confirmpanNo').val()) {
        $('#message1').html('').css('color', 'green');
    } else
        $('#message1').html('Please enter correct PAN number').css('color', 'red');
});

$('#confirmtanNo').on('change', function () {
    if ($('#tanNo').val() == $('#confirmtanNo').val()) {
        $('#message3').html('').css('color', 'green');
    } else
        $('#message3').html('Please enter correct TAN number').css('color', 'red');
});



var id = 0;
$("#savevendor").on("click", function () {
    
    var Name = $("#Name").val();


    var fdata = new FormData();
    var GSTRegistration = $("#GSTRegistration").val();
    var GSTIN_Number = $("#disableGst").val();
    var confirmGstnumner = $("#disableConfirmGst").val();
    var Name = $("#Name").val();
    var OwnerOrDirectorName = $("#OwnerOrDirectorName").val();
    var PAN_Number = $("#panNo").val();
    var TAN_Number = $("#tanNo").val();
    var confirmpannumber = $("#confirmpanNo").val();
    var confirmtannumber = $("#confirmtanNo").val();
    var Notes = $("#Notes").val();
    var GST_TDS_Applicable = $("#GST_TDS_Applicable").val();
    var OpeningDate = $("#OpeningDate").val();

    var OpeningBalance = $("#OpeningBalance").val();
    console.log(OpeningDate);
    if (OpeningDate == "") {
        const openingDate = document.getElementById('OpeningDate');
        openingDate.value = null;
        console.log(OpeningDate);
    }



    var Status = document.getElementById("Status");

    if (Status.checked) {
        $("#Status").val(true);
    }
    else {
        $("#Status").val(false);
    }

    var Status = $("#Status").val();






    fdata.append("GSTRegistration", GSTRegistration);
    fdata.append("GSTIN_Number", GSTIN_Number);
    fdata.append("Name", Name);
    fdata.append("OwnerOrDirectorName", OwnerOrDirectorName);
    fdata.append("PAN_Number", PAN_Number);
    fdata.append("TAN_Number", TAN_Number);
    fdata.append("GST_TDS_Applicable", GST_TDS_Applicable);
    fdata.append("Status", Status);
    fdata.append("Notes", Notes);


    fdata.append("VendorBalance.OpeningBalanceDate", OpeningDate);
    fdata.append("VendorBalance.OpeningBalance", OpeningBalance);




    var IsExisting = document.getElementById("IsExisting");

    if (IsExisting.checked) {

        if (OpeningDate == "") {

            Swal.fire({
                text: 'Please Enter the Opening Date',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })

            return false;
        }
        if (OpeningBalance == "") {

            Swal.fire({
                text: 'Please Enter the Open Balance',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })

            return false;
        }

    }




    if (GSTRegistration == "") {

        Swal.fire({
            text: 'Please Select GST Registration',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (GSTIN_Number == "" && GSTRegistration == "Registered") {

        Swal.fire({
            text: 'Please enter GST number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (confirmGstnumner != GSTIN_Number && confirmpannumber != PAN_Number && confirmtannumber != TAN_Number) {
        swal.fire({
            text: "Please enter correct GST number or Pan number or Tan Number",
            icon: "warning",
            button: "ok !",

        }).then((result) => {
            $("#disableConfirmGst").val("");
            $("#confirmpanNo").val("");
            $("#confirmtanNo").val("");
        })

    }
    if (Name == "") {

        Swal.fire({
            text: 'Please enter  the  name',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (OwnerOrDirectorName == "") {

        Swal.fire({
            text: 'Please enter  the Owner/Director name',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })``

        return false;
    }

    if (PAN_Number == "") {

        Swal.fire({
            text: 'Please enter the PAN number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (TAN_Number == "") {

        Swal.fire({
            text: 'Please enter the TAN number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (confirmGstnumner != GSTIN_Number || confirmpannumber != PAN_Number || confirmtannumber != TAN_Number) {


        Swal.fire({
            text: 'Please enter correct GST number or PAN number or TAN Number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    else {
        $(this).attr("disabled", "disabled");
        var newUrl = `/Vendor/AddVendor`;
        // var newUrl = '@Url.Action("AddVendor","Vendor")';
        ;
        $.ajax({
            url: "/Vendor/AddVendor",
            type: "post",
            data: fdata,
            processData: false,
            contentType: false,
            success: function (result) {
                id = result.data;
                swal.fire({
                    title: "Vendor : " + Name + "",
                    text: "Added Successfully!!",
                    icon: "success",
                    button: "Ok!",

                }).then((result) => {

                    $(".vedisable").attr("disabled", "disabled");
                    $(".disbtn").attr("disabled", false);
                    //$("#save").addClass("invisible");


                })
            },
            error: function () {
                swal.fire({
                    text: "Failed to add vendor detials!!",
                    icon: "warning",
                    button: "Ok!",

                }).then((result) => {
                    $(".vedisable").val("");
                    $("#savevendor").attr("disabled", false);
                    console.log('Failed ');

                })
            }

        });
    }


});
$("#save1").on("click", function () {
    var Name = $("#VendorPerson_Contacts_Name").val();
    var vendorid = id;
    console.log()
    //if (id == 0 && Name == "") {
    //    swal.fire({
    //        text: "Please fill the vendor details!!",
    //        icon: "warning",
    //        button: "ok!",
    //    }).then((result) => {

    //    })
    //}




    //else {

    var fdatas = new FormData();
    var Country = $("#VendorPerson_Addresses_Country").val();
    var Name = $("#VendorPerson_Contacts_Name").val();
    var Phone = $("#VendorPerson_Contacts_Phone").val();
    var Address = $("#VendorPerson_Addresses_Address").val();
    var City = $("#VendorPerson_Addresses_City").val();
    var State = $("#VendorPerson_Addresses_State").val();
    var PinCode = $("#VendorPerson_Addresses_PinCode").val();
    var Email = $("#EmailTxt").val();

    fdatas.append("Addresses.Country", Country);
    fdatas.append("Contacts.Phone", Phone);
    fdatas.append("Contacts.Name", Name);
    fdatas.append("Addresses.City", City);
    fdatas.append("Addresses.Address", Address);
    fdatas.append("Addresses.State", State);
    fdatas.append("Addresses.PinCode", PinCode);
    fdatas.append("Contacts.Email", Email);
    fdatas.append("VendorId", vendorid);



    if (Name == "") {

        Swal.fire({
            text: 'Please enter  the contact person name',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    var regx = /^[6-9]\d{9}$/;

    if (regx.test(Phone)) {

    } else {
        Swal.fire({
            text: 'Please enter the valid phone number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (Email == "") {

        Swal.fire({
            text: 'Please enter the email',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (Country == "") {

        Swal.fire({
            text: 'Please select the Country',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }



    if (PinCode == "") {

        Swal.fire({
            text: 'Please enter  the pincode',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }





    if (State == "") {

        Swal.fire({
            text: 'Please select the State',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (City == "") {

        Swal.fire({
            text: 'Please select the City',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }



    if (PinCode == "") {

        Swal.fire({
            text: 'Please enter  the pincode',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    } else {
        $(this).attr("disabled", "disabled");
        var newUrl = `/Vendor/AddVendor`;
        //var newUrl = '@Url.Action("AddVendor","Vendor")';

        $.ajax({
            url: "/VendorPerson/AddVendorInformation",
            type: "post",
            data: fdatas,
            processData: false,
            contentType: false,
            success: function (result) {
                swal.fire({
                    title: "Vendor : " + Name + "",
                    text: "Contact information added successfully!!",
                    icon: "success",
                    button: "Ok!",

                }).then((result) => {
                    $(".vpdisable").attr("disabled", "disabled");


                })
            },
            error: function () {
                swal.fire({
                    text: "Failed to add vendor contact detials!!",
                    icon: "warning",
                    button: "Ok!",

                }).then((result) => {
                    $(".vpdisable").val("");
                    $("#save1").attr("disabled", false);
                    console.log('Failed ');

                })
            }
        });

    }





});
$("#save2").on("click", function () {

    var BeneficiaryName = $("#VendorBankAccounts_BeneficiaryName").val();
    var vendorid = id;
    //if (id == 0 && BeneficiaryName == "") {
    //    swal.fire({
    //        text: "Please fill the vendor details!!",
    //        icon: "warning",
    //        button: "ok!",

    //    }).then((result) => {


    //    })
    //}
    //else {
    var fdata = new FormData();
    var vendorName = $("#Name").val();
    var BeneficiaryName = $("#VendorBankAccounts_BeneficiaryName").val();
    var selectedBranchDetails = $("#selBranchName").val().split(',');
    var selectedBankDetails = $("#selBankName").val().split(',');
    console.log(selectedBankDetails);
    console.log(selectedBranchDetails);
    var BankMasterId = selectedBranchDetails[1];
    var AccountNumber = $("#accountNo").val();
    var confirmAccountNo = $("#confirmaccountNo").val();
    fdata.append("BeneficiaryName", BeneficiaryName);
    fdata.append("VendorId", vendorid);
    fdata.append("BankMasterId", BankMasterId);
    fdata.append("AccountNumber", AccountNumber);
    if (confirmAccountNo != AccountNumber) {
        swal.fire({
            text: "Please enter the correct account number ",
            icon: "warning",
            button: "ok!",

        }).then((result) => {
            $("#confirmaccountNo").val("");

        })
        return false;
    }
    if (BeneficiaryName == "") {

        Swal.fire({
            text: 'Please enter  the beneficiary name',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (selectedBankDetails == "") {

        Swal.fire({
            text: 'Please enter  the Bank Details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (selectedBranchDetails == "") {

        Swal.fire({
            text: 'Please enter  the Branch Details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (AccountNumber == "") {

        Swal.fire({
            text: 'Please enter  the Account number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    else {
        $(this).attr("disabled", "disabled");
        var newUrl = `/Vendor/AddVendor`;
        //var newUrl = '@Url.Action("AddVendor","Vendor")';

        $.ajax({
            url: "/VendorBankAccount/AddVendorBank",
            type: "post",
            data: fdata,
            processData: false,
            contentType: false,
            success: function (result) {
                swal.fire({
                    title: "Vendor : " + vendorName + "",
                    text: "Vendor bank account details added successfully!!",
                    icon: "success",
                    button: "Ok!",
                }).then((result) => {

                    $(".vbdisable").attr("disabled", "disabled");


                })
            },
            error: function () {
                swal.fire({
                    text: "Failed to add vendor bank account details!!",
                    icon: "warning",
                    button: "Ok!",

                }).then((result) => {
                    $(".vbdisable").val("");
                    $("#save2").attr("disabled", false);
                    console.log('Failed ');

                })
            }
        });
    }

});

$("#save3").on("click", function () {

    var vendorid = id;
    var Category = $("#VendorDefaults_Category").val();
    //if (id == 0 && Category == "") {
    //    swal.fire({

    //        text: "Please fill the Vendor details!!",
    //        icon: "warning",
    //        button: "ok!",

    //    }).then((result) => {


    //    })
    //}
    //else {
    // $(this).attr("disabled", "disabled");
    var fdata = new FormData();
    var vendorName = $("#Name").val();
    var Category = $("#VendorDefaults_Category").val();
    var PaymentMethod = $("#VendorDefaults_PaymentMethod").val();
    var PaymentTerms = $("#VendorDefaults_PaymentTerms").val();
    var TDSSection = $("#tdssectionsvalue").val();
    var TDSPercentage = $("#tdspercentage").val();
    var GST_TDSPercentage = $("#VendorDefaults_GST_TDSPercentage").val();

    fdata.append("Category", Category);
    fdata.append("VendorId", vendorid);
    fdata.append("PaymentMethod", PaymentMethod);
    fdata.append("GST_TDSPercentage", GST_TDSPercentage);
    fdata.append("PaymentTerms", PaymentTerms);
    fdata.append("TDSSection", TDSSection);
    fdata.append("TDSPercentage", TDSPercentage);


    if (Category == "") {

        Swal.fire({
            text: 'Please select  the category',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (PaymentMethod == "") {

        Swal.fire({
            text: 'Please select  the payment method',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (PaymentTerms == "") {

        Swal.fire({
            text: 'Please select the payment terms',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (TDSSection == "") {

        Swal.fire({
            text: 'Please select the TDS section',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (!/^[0-9]+$/.test(TDSPercentage)) {

        Swal.fire({
            text: 'Please enter the TDS %',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    var newUrl = `/Vendor/AddVendor`;
    //var newUrl = '@Url.Action("AddVendor","Vendor")';

    $.ajax({
        url: "/VendorDefaults/AddVendorDefautls",
        type: "post",
        data: fdata,
        processData: false,
        contentType: false,
        success: function (result) {
            swal.fire({
                title: "Vendor : " + vendorName + "",
                text: "Vendor default details added successfully!!",
                icon: "success",
                button: "Ok!",

            }).then((result) => {

                $(".vddisable").attr("disabled", "disabled");


            })
        },
        error: function () {
            swal.fire({
                text: "Failed to add vendor default details!!",
                icon: "warning",
                button: "Ok!",

            }).then((result) => {
                $(".vddisable").val("");
                $("#save3").attr("disabled", false);
                console.log('Failed ');

            })
        }
    });

});
$("#save4").on("click", function () {
    var Id = id;
    //if (id == 0) {
    //    swal.fire({

    //        text: "Please fill the vendor details!!",
    //        icon: "warning",
    //        button: "ok!",

    //    }).then((result) => {


    //    })
    //    //$("#Fileupload1").val("");
    //    //$(".list").text("");
    //    //$("#save4").attr("disabled", false);
    //    //return false;
    //}
    //else {

    var fdata = new FormData();

    var filedata = fileList;
    var flength = filedata.length;
    for (i = 0; i < flength; i++) {
        var data1 = filedata[i];
        for (j = 0; j <= data1.length; j++) {
            fdata.append('files', data1[j]);
            fdata.append('DocumentRefId', Id);
        }
    }
    var postFileName = fileName;
    var fName = postFileName.length;
    for (i = 0; i < fName; i++) {
        var data1 = postFileName[i];
        fdata.append('DocumentName', data1);
    }

    var postFileName1 = fielTypes;
    var fName1 = postFileName1.length;
    for (i = 0; i < fName1; i++) {
        var data1 = postFileName1[i];

        fdata.append('FileType', data1);

    }

    var newUrl = `/Vendor/Index`;
    // var newUrl = '/Vendor/Index' ;
    //var newUrl = '@Url.Action("Index","Vendor")';
    var docType = "COI";
    if (fielTypes.includes(docType)) {
        $.ajax({
            url: "/Document/AddDocuments",
            type: "post",
            data: fdata,
            processData: false,
            contentType: false,
            success: function (result) {
                console.log(result)
                swal.fire({
                    title: vendorName,
                    text: "File uploaded successfully!!",
                    icon: "success",
                    button: "Ok!",

                }).then((result) => {
                    debugger;

                    $("#FileUpload1").attr("disabled,disabled");
                    $("#fileType").attr("disabled,disabled");
                    window.location.href = newUrl;

                })
            },
            error: function () {
                swal.fire({
                    text: "Failed to Upload the files!!",
                    icon: "warning",
                    button: "Ok!",

                }).then((result) => {
                    $("#save4").attr("disabled", false);
                    console.log('Failed ');

                })
            }
        });

    }

    else {
        swal.fire({

            text: "Please upload the file for COI (Certificate Of Incorporation)!!",
            icon: "warning",
            button: "ok!",

        })
        $("#save4").attr("disabled", false);
        $("#Fileupload1").val("");
        $(".list").text("");
        return false;
    }



});

$("#disableGst").on('change', function () {
    var GSTNumber = $(this).val();

    for (var i = 0; i < GSTAvailable.length; i++) {
        if (GSTNumber == GSTAvailable[i]) {
            swal.fire({
                title: 'GST number already exists',
                icon: 'warning',
                confirmButtonText: 'Ok',
            }).then((result) => {
                $(this).val("");
            });
            break; // Exit the loop if a match is found
        }
    }
});

$("#panNo").on('change', function () {
    var PanNumber = $(this).val();

    for (i = 0; i < PanAvailable.length; i++) {
        if (PanNumber == PanAvailable[i]) {
            swal.fire({
                title: 'PAN number already exists',
                icon: 'warning',

                confirmButtonText: 'Ok',
            }).then((result) => {
                $(this).val("");
            })
        }
    }
});
$("#accountNo").on('change', function () {
    var AccountNumber = $(this).val();

    for (i = 0; i < AccountNoAvailable.length; i++) {
        if (AccountNumber == AccountNoAvailable[i]) {
            swal.fire({
                title: 'Account number already exists',
                icon: 'warning',

                confirmButtonText: 'Ok',
            }).then((result) => {
                $(this).val("");
            })
        }
    }
});

$("#clear1").on("click", function () {
    $(".vedisable").each(function () {
        $(this).val("");
        $(".chckvs").prop("checked", false);
        $(".checkgst").prop("checked", false);

    });
    $(".text-danger").addClass("invisible");
});
$("#clear2").on("click", function () {
    $(".vpdisable").each(function () {
        $(this).val("");

    });
    $(".text-danger").addClass("invisible");
});
$("#clear3").on("click", function () {
    $(".vbdisable").each(function () {
        $(this).val("");

    });
    $(".text-danger").addClass("invisible");
});
$("#clear4").on("click", function () {
    $(".vddisable").each(function () {
        $(this).val("");

    });
    $(".text-danger").addClass("invisible");
});
$("#clear5").on("click", function () {
    $("#FileUpload1").val("");
    $(".list").text("");
    $("filecount").text("");
    $(".fname").each(function () {
        $(this).text("");
    });
    $(".text-danger").addClass("invisible");

});
$("#save").on("click", function () {

    var fdata = new FormData();
    var GSTRegistration = $("#GSTRegistration").val();
    var GSTIN_Number = $("#disableGst").val();
    var confirmGstnumner = $("#disableConfirmGst").val();
    var Name = $("#Name").val();
    var OwnerOrDirectorName = $("#OwnerOrDirectorName").val();
    var PAN_Number = $("#panNo").val();
    var TAN_Number = $("#tanNo").val();
    var confirmpannumber = $("#confirmpanNo").val();
    var confirmtannumber = $("#confirmtanNo").val();
    var Notes = $("#Notes").val();
    var GST_TDS_Applicable = $("#GST_TDS_Applicable").val();
    var Status = document.getElementById("Status");
    var IsExisting = document.getElementById("IsExisting");



    if (Status.checked) {
        $("#Status").val(true);
    }
    else {
        $("#Status").val(false);
    }

    var Status = $("#Status").val();



    var Country = $("#VendorPerson_Addresses_Country").val();
    var Name1 = $("#VendorPerson_Contacts_Name").val();
    var Phone = $("#VendorPerson_Contacts_Phone").val();
    var Address = $("#VendorPerson_Addresses_Address").val();
    var City = $("#VendorPerson_Addresses_City").val();
    var State = $("#VendorPerson_Addresses_State").val();
    var PinCode = $("#VendorPerson_Addresses_PinCode").val();
    var Email = $("#EmailTxt").val();
    var BeneficiaryName = $("#VendorBankAccounts_BeneficiaryName").val();
    var selectedBranchDetails = $("#selBranchName").val().split(',');
    var selectedBankDetails = $("#selBankName").val().split(',');
    var BankMasterId = selectedBranchDetails[1];
    var AccountNumber = $("#accountNo").val();
    var confirmAccountNo = $("#confirmaccountNo").val();
    var Category = $("#VendorDefaults_Category").val();
    var PaymentMethod = $("#VendorDefaults_PaymentMethod").val();
    var PaymentTerms = $("#VendorDefaults_PaymentTerms").val();
    var TDSSection = $("#tdssectionsvalue").val();
    var TDSPercentage = $("#tdspercentage").val();
    var GST_TDSPercentage = $("#VendorDefaults_GST_TDSPercentage").val();
    var OpeningDate = $("#OpeningDate").val();

    var OpeningBalance = $("#OpeningBalance").val();
    console.log(OpeningDate);

    if (fielTypes.includes("COI")) {
        var filedata = fileList;
        var flength = filedata.length;
        for (i = 0; i < flength; i++) {
            var data1 = filedata[i];
            for (j = 0; j <= data1.length; j++) {
                fdata.append('files', data1[j]);
            }
        }
        var postFileName = fileName;
        var fName = postFileName.length;
        for (i = 0; i < fName; i++) {
            var data1 = postFileName[i];
            fdata.append('DocumentName', data1);
        }
    }

    else {
        swal.fire({

            text: "Please upload the file for COI (Certificate Of Incorporation)!!",
            icon: "warning",
            button: "ok!",

        }).then((result) => {
            $(this).attr("disabled", false);
            $("#Fileupload1").val("");
            $(".list").text("");
            return false;
        })


    }


    if (!fielTypes.includes("COI")) {
        swal.fire({

            text: "Please upload the file for COI (Certificate Of Incorporation)!!",
            icon: "warning",
            button: "ok!",

        })
        $(this).attr("disabled", false);
        $("#Fileupload1").val("");
        $(".list").text("");
        return false;
    }

    fdata.append("FileType", fielTypes);




    fdata.append("VendorDefaults.Category", Category);
    fdata.append("VendorDefaults.PaymentMethod", PaymentMethod);
    fdata.append("VendorDefaults.GST_TDSPercentage", GST_TDSPercentage);
    fdata.append("VendorDefaults.PaymentTerms", PaymentTerms);
    fdata.append("VendorDefaults.TDSSection", TDSSection);
    fdata.append("VendorDefaults.TDSPercentage", TDSPercentage);
    fdata.append("VendorBankAccounts.BeneficiaryName", BeneficiaryName);
    fdata.append("VendorBankAccounts.BankMasterId", BankMasterId);
    fdata.append("VendorBankAccounts.AccountNumber", AccountNumber);
    fdata.append("GSTRegistration", GSTRegistration);
    fdata.append("GSTIN_Number", GSTIN_Number);
    fdata.append("Name", Name);
    fdata.append("OwnerOrDirectorName", OwnerOrDirectorName);
    fdata.append("PAN_Number", PAN_Number);
    fdata.append("TAN_Number", TAN_Number);
    fdata.append("GST_TDS_Applicable", GST_TDS_Applicable);
    fdata.append("Status", Status);

    fdata.append("Notes", Notes);
    fdata.append("VendorPerson.Addresses.Country", Country);
    fdata.append("VendorPerson.Contacts.Phone", Phone);
    fdata.append("VendorPerson.Contacts.Name", Name1);
    fdata.append("VendorPerson.Addresses.City", City);
    fdata.append("VendorPerson.Addresses.Address", Address);
    fdata.append("VendorPerson.Addresses.State", State);
    fdata.append("VendorPerson.Addresses.PinCode", PinCode);
    fdata.append("VendorPerson.Contacts.Email", Email);
    fdata.append("VendorBalance.OpeningBalanceDate", OpeningDate);
    fdata.append("VendorBalance.OpeningBalance", OpeningBalance);

    //var postFileName1 = fielTypes;
    //var fName1 = postFileName1.length;
    //for (i = 0; i < fName1; i++) {
    //    var data3 = postFileName1[i];
    //    console.log(data3)
    //    fdata.append('FileType', data3);

    //}


    if (IsExisting.checked) {

        if (OpeningDate == "") {

            Swal.fire({
                text: 'Please Enter the Opening Date',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })

            return false;
        }
        if (OpeningBalance == "") {

            Swal.fire({
                text: 'Please Enter the Open Balance',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })

            return false;
        }

    }







    if (GSTRegistration == "") {
        $("#save6").removeAttr("disabled");
        Swal.fire({
            text: 'Please select GST registration',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        $("#save6").removeAttr("disabled");
        return false;
    }

    if (GSTIN_Number == "" && GSTRegistration == "Registered") {

        Swal.fire({
            text: 'Please enter GST number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (Name == "") {

        Swal.fire({
            text: 'Please enter  the contact person name',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }



    if (OwnerOrDirectorName == "") {

        Swal.fire({
            text: 'Please enter  the Owner/Director name',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (PAN_Number == "") {

        Swal.fire({
            text: 'Please enter the PAN number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (TAN_Number == "") {

        Swal.fire({
            text: 'Please enter the TAN number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (confirmGstnumner != GSTIN_Number || confirmpannumber != PAN_Number || confirmtannumber != TAN_Number) {


        Swal.fire({
            text: 'Please enter correct GST number or PAN number or TAN Number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }






    if (Name1 == "") {

        Swal.fire({
            text: 'Please enter  the contact person Name',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    var regx = /^[6-9]\d{9}$/;

    if (regx.test(Phone)) {

    } else {
        Swal.fire({
            text: 'Please enter the valid phone number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (Email == "") {

        Swal.fire({
            text: 'Please enter the email',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (Country == "") {

        Swal.fire({
            text: 'Please select the Country',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (State == "") {

        Swal.fire({
            text: 'Please select the State',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (City == "") {

        Swal.fire({
            text: 'Please select the City',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (PinCode == "") {

        Swal.fire({
            text: 'Please enter  the pincode',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (BeneficiaryName == "") {

        Swal.fire({
            text: 'Please enter  the beneficiary name',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (selectedBankDetails == "") {

        Swal.fire({
            text: 'Please enter  the Bank Details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (selectedBranchDetails == "") {

        Swal.fire({
            text: 'Please select Branch Details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (selectedBranchDetails == "") {

        Swal.fire({
            text: 'Please select Branch Details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })``

        return false;
    }
    if (AccountNumber == "") {

        Swal.fire({
            text: 'Please enter the Account number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })``

        return false;
    }

    if (AccountNumber != confirmAccountNo) {


        Swal.fire({
            text: 'Please enter correct account number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (Category == "") {

        Swal.fire({
            text: 'Please select  the category',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (PaymentMethod == "") {

        Swal.fire({
            text: 'Please select  the payment method',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (PaymentTerms == "") {

        Swal.fire({
            text: 'Please select the payment terms',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    if (TDSSection == "") {

        Swal.fire({
            text: 'Please select the TDS section',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }


    if (fielTypes == "") {

        Swal.fire({
            text: 'Please select the file type',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }




    if (!/^[0-9]+$/.test(TDSPercentage)) {

        Swal.fire({
            text: 'Please enter the TDS %',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }




    //var input1 = document.getElementById("FileUpload1");
    //var files = input1.files;
    //for (var i = 0; i != files.length; i++) {
    //    fdata.append("files", files[i]);
    //}

    console.log(fdata);
    var newUrl = `/Vendor/Index`;

    // var newUrl = '@Url.Action("Index","Vendor")';

    $.ajax({
        url: "/Vendor/Create",
        type: "post",
        data: fdata,
        processData: false,
        contentType: false,
        success: function (result) {
            swal.fire({
                title: "Vendor : " + Name + "",
                text: "Details added Successfully!!",
                icon: "success",
                iconcolor: "Green",
                button: "Ok!",

            }).then((result) => {
                window.location.href = newUrl;
            })
        },
        error: function () {
            swal.fire({
                text: "Failed to add vendor details!!",
                icon: "warning",
                button: "Ok!",

            }).then((result) => {
                $(".vddisable").val("");
                $("#save").attr("disabled", false);
                console.log('Failed ');

            })
        }
    });
});

