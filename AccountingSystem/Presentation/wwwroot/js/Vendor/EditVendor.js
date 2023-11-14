

var myElement = document.getElementById('myElement');

var Id = myElement.getAttribute('data-id');


$('form').on('change', ':checkbox', function () {
    if (this.checked) {
        $(this).val(true);
    }
    else {
        $(this).val(false);
    }
});
$('#GST_TDS_Applicable').on('change', function () {
    if (this.checked) {
        $("#VendorDefaults_GST_TDSPercentage").val('2.00');
    } else {
        $("#VendorDefaults_GST_TDSPercentage").val('0.00');
    }

});
var fileList = [];
var fileName = [];
var fielTypes = [];
inputTypeFile = document.getElementById('FileUpload1');
//showFilesList = document.getElementById('showFilesList');
documents = document.getElementById("documents");
inputTypeFile.addEventListener('change', function () {
    var selectedFileValue = $('#filetype').val();
    fielTypes.push(selectedFileValue);
    if (selectedFileValue == "" | null) {
        Swal.fire({
            text: 'Please select the file type',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        return false;
    }
    var filename = this.files[0].name;
    var selFileName = (filename + "," + selectedFileValue)
    fileName.push(selFileName)
    fileList.push(this.files);
    console.log(fileList)
    $('#filetype').val('');
    if (this.files.length > 0) {
        for (var i = 0; i < this.files.length; i++) {
            var rows = "";
            rows += '<tr><td>' + filename + '</td> <td>' + selectedFileValue + '</td><td></td><td><i value="Delete" type="button" class="fa fa-trash fa1 ml-5"></i></td>  </tr>';
            $(rows).appendTo("#example tbody");

        }
    }


    closeIT = document.getElementsByClassName('fa1')
    arrayFormCloseBtn = [...closeIT];
    arrayFormCloseBtn.forEach((onebyone) => {
        onebyone.addEventListener('click', function (e) {
            var td = e.target.parentNode;
            var tr = td.parentNode;
            tr.parentNode.removeChild(tr);
            var Cells = tr.getElementsByTagName("td");
            var object = Cells[0].innerText;
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
        })
    })
});
$("#tdsScetion").on('change', function () {
    var selectedDetails = $("#tdsScetion").val().split(',');
    var CodeValue = selectedDetails[0];
    var percentage = selectedDetails[1];
    $("#tdspercentage").val(percentage);

    $("#tdssectionsvalue").val(CodeValue);

});
$(document).ready(function () {

    // onchange of BankName Drop down
    $("#selBankName").on('change', function () {
        var selectedBank = $("#selBankName").val();
        if (selectedBank != '') {
            $("#selBranchName").find('option').hide();
            $("#selBranchName").val('');
            $("#selBranchName option[value='']").show();
            $('*[data="' + selectedBank + '"]').show();
            $("#selIfsc").val('');
            $("#selBankMasterId").val('');

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
        if (selectedBranch != '') {
            $("#selIfsc").val(selectedBranch);
            $("#selBankMasterId").val(BankMasterId);
            $("#branchid").val(BankMasterId);
        }
        else {
            // if BranchName not selected then hide all tasks
            $("#selIfsc").val('');
            $("#selBankMasterId").val('');
        }

    });
    $(function () {
        $("#NewID").change(function () {

            if ($(this).val() == "Registered") {
                $("#disableConfirmGst").removeAttr("disabled");
                $("#disableConfirmGst").focus();
                $("#VendorDefaults_GST_TDSPercentage").val('2.00');

                var res = $("#gstNumberHidden").val();
                $("#disableConfirmGst").val(res);
                $("#disableGst").val(res);
                $("#GST_TDS_Applicable").removeAttr("disabled");

            } else {
                $("#disableConfirmGst").val('');
                $("#disableGst").val('');
                $("#disableConfirmGst").attr("disabled", "disabled");
                $("#VendorDefaults_GST_TDSPercentage").val('0.00');
                $("#GST_TDS_Applicable").attr("disabled", "disabled");

            }
        });
    });
    $(function () {

        $("#NewID").change(function () {
            ;
            if ($(this).val() == "Registered") {
                $("#disableGst").removeAttr("disabled");
                $("#disableGst").focus();
                $("#VendorDefaults_GST_TDSPercentage").val('2.00');
                $("#GST_TDS_Applicable").removeAttr("disabled");
            } else {
                $("#VendorDefaults_GST_TDSPercentage").val('0.00');
                $("#disableGst").attr("disabled", "disabled");
                $("#GST_TDS_Applicable").attr("disabled", "disabled");

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
            $('#message1').html('Please enter correct PAN numbers').css('color', 'red');
    });
    $('#confirmtanNo').on('change', function () {
        if ($('#tanNo').val() == $('#confirmtanNo').val()) {
            $('#message1').html('').css('color', 'green');
        } else
            $('#message1').html('Please enter correct TAN numbers').css('color', 'red');
    });
});

function Validate() {
    // showFilesList = document.getElementById('showFilesList')
    var fileCount = document.getElementById("example").getElementsByTagName("tr").length

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

$("#disableGst").on('input', function () {
    var GSTNumber = $(this).val();

    for (i = 0; i < GSTAvailable.length; i++) {
        if (GSTNumber == GSTAvailable[i]) {
            swal.fire({
                title: 'GST number already exists!!',
                icon: 'warning',

                confirmButtonText: 'Ok',
            }).then((result) => {
                $(this).val("");
            })
        }
    }
});

$("#save1").on("click", function () {

   
   
    var fdata = new FormData();
    var GSTRegistration = $("#NewID").val();

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
    var Status = $("#Status").val();
    fdata.append("Id", Id);
    fdata.append("GSTRegistration", GSTRegistration);
    fdata.append("GSTIN_Number", GSTIN_Number);
    fdata.append("Name", Name);
    fdata.append("OwnerOrDirectorName", OwnerOrDirectorName);
    fdata.append("PAN_Number", PAN_Number);
    fdata.append("TAN_Number", TAN_Number);
    fdata.append("GST_TDS_Applicable", GST_TDS_Applicable);
    fdata.append("Status", Status);
    fdata.append("Notes", Notes);


    if (GSTRegistration == "") {

        Swal.fire({
            text: 'Please select GST Registration',
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

        swal.fire({

            text: "Please enter correct GST number or PAN number or TAN number ",
            icon: "warning",
            button: "ok !",

        }).then((result) => {
            $("#disableConfirmGst").val("");
            $("#confirmpanNo").val("");
            $("#confirmtanNo").val("");

        })

    }
    if (OwnerOrDirectorName == "") {

        Swal.fire({
            text: 'Please enter  the Owner/Director name',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    else {
       
        $(this).attr("disabled", "disabled");

        var newUrl = `/Vendor/EditVendor?id=${Id}`;
       // var newUrl = 'EditVendor/Vendor/?id=' + Id;
        //var newUrl = 'EditVendor/Vendor/' + Id;

       // var newUrl = '@Url.Action("EditVendor", "Vendor", new { Id = @Id })';

        $.ajax({
            url: "/Vendor/EditVendor",
            type: "post",
            data: fdata,
            processData: false,
            contentType: false,
            success: function (result) {

                swal.fire({
                    title: "Vendor : " + Name + "",
                    text: "Updated successfully!!",
                    icon: "success",
                    button: "Ok!",

                }).then((result, event) => {
                    
                    window.location.href = newUrl;

                    $(".tests").addClass("invisible");
                    event.preventDefault();
                    $(".vdisable").prop("disabled", true);

                    $("#disableConfirmGst").attr("disabled");

                    $("#disableConfirmGst").attr("disabled", "disabled");
                    $("#disableGst").attr("disabled");
                    $("#disableGst").attr("disabled", "disabled");
                })
            },
            error: function () {
                swal.fire({
                    text: "Failed to update vendor details!!",
                    icon: "warning",
                    button: "Ok!",

                }).then((result) => {
                    window.location.href = newUrl;
                    console.log('Failed ');

                })
            }

        });

    }
});

$("#save2").on("click", function () {
   
    var newUrl = `/Vendor/EditVendor?id=${Id}`;
    // $(this).attr("disabled", "disabled");
    var fdata = new FormData();
    var vendorName = $("#Name").val();
    var vendorid = Id;
    var contactid = $("#VendorPerson_Contacts_Id").val();
    var addressId = $("#VendorPerson_Addresses_Id").val();
    var vendorpersonid = $("#VendorPerson_Id").val();
    var Country = $("#VendorPerson_Addresses_Country").val();
    var Name = $("#VendorPerson_Contacts_Name").val();
    var Phone = $("#VendorPerson_Contacts_Phone").val();
    var Address = $("#VendorPerson_Addresses_Address").val();
    var City = $("#VendorPerson_Addresses_City").val();
    var State = $("#VendorPerson_Addresses_State").val();
    var PinCode = $("#VendorPerson_Addresses_PinCode").val();
    var Email = $("#VendorPerson_Contacts_Email").val();

    fdata.append("Addresses.Country", Country);
    fdata.append("Id", vendorpersonid);
    fdata.append("Contacts.Phone", Phone);
    fdata.append("Contacts.Name", Name);
    fdata.append("Addresses.City", City);
    fdata.append("Addresses.Address", Address);
    fdata.append("Addresses.State", State);
    fdata.append("Addresses.PinCode", PinCode);
    fdata.append("Contacts.Email", Email);
    fdata.append("VendorId", vendorid);
    fdata.append("Addresses.Id", addressId);
    fdata.append("Contacts.Id", contactid);
    fdata.append("Contacts.VendorPersonID", vendorpersonid);
    fdata.append("Addressess.VendorPersonID", vendorpersonid);
   // var newUrl = '@Url.Action("EditVendor", "Vendor", new { Id = @Id })';

    if (City == "") {

        Swal.fire({
            text: 'Please select the City',
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
    if (PinCode == "") {

        Swal.fire({
            text: 'Please enter  the pincode',
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

    $.ajax({
        url: "/VendorPerson/EditVendorPerson",
        type: "post",
        data: fdata,
        processData: false,
        contentType: false,
        success: function (result) {
            swal.fire({
                title: "Vendor : " + vendorName + "",
                text: "Contact information updated successfully!!",
                icon: "success",
                button: "Ok!",

            }).then((result, event) => {
                window.location.href = newUrl;
                $(".test1").addClass("invisible");
                event.preventDefault();
                $(".persondisable").prop("disabled", true);

            })
        },
        error: function () {
            swal.fire({
                text: "Failed to add vendor contact details!!",
                icon: "warning",
                button: "Ok!",

            }).then((result) => {
                window.location.href = newUrl;
                console.log('Failed ');

            })
        }
    });


});

$("#save3").on("click", function () {
    ;
    var fdata = new FormData();
    var vendorid = Id;
    var vendorName = $("#Name").val();
    var vendorbankId = $("#VendorBankAccounts_Id").val();
    var BeneficiaryName = $("#VendorBankAccounts_BeneficiaryName").val();
    var selectedBankDetails = $("#selBankName").val().split(',');
    var selectedBranchDetails = $("#selBranchName").val().split(',');
    console.log(selectedBranchDetails)
    console.log(selectedBankDetails)
    var BankMasterId = selectedBranchDetails[1];
    var AccountNumber = $("#accountNo").val();
    var confirmAccountNo = $("#confirmaccountNo").val();
    fdata.append("BeneficiaryName", BeneficiaryName);
    fdata.append("Id", vendorbankId);
    fdata.append("VendorId", vendorid);
    fdata.append("BankMasterId", BankMasterId);
    fdata.append("AccountNumber", AccountNumber);
    if (confirmAccountNo != AccountNumber) {
        swal.fire({
            text: "Please enter the correct account number",
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
            text: 'Please enter the Bank Details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (selectedBranchDetails == "") {

        Swal.fire({
            text: 'Please enter the Branch details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (AccountNumber == "") {

        Swal.fire({
            text: 'Please enter the Account Number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }






    else {


        $(this).attr("disabled", "disabled");
        var newUrl = `/Vendor/EditVendor?id=${Id}`;
        //var newUrl = '@Url.Action("EditVendor", "Vendor", new { Id = @Id })';

        $.ajax({
            url: "/VendorBankAccount/EditVendorBank",
            type: "post",
            data: fdata,
            processData: false,
            contentType: false,
            success: function (result) {
                swal.fire({
                    title: "Vendor : " + vendorName + "",
                    text: " Bank account details updated successfully!!",
                    icon: "success",
                    button: "Ok!",

                }).then((result, event) => {
                    window.location.href = newUrl;
                    $(".test2").addClass("invisible");
                    event.preventDefault();
                    $(".defaultdisable").prop("disabled", true);

                })
            },
            error: function () {
                swal.fire({
                    text: "Failed to add Vendor bank account details!!",
                    icon: "warning",
                    button: "Ok!",

                }).then((result) => {
                    window.location.href = newUrl;
                    console.log('Failed ');

                })
            }

        });
    }

});

$("#save4").on("click", function () {
    //$(this).attr("disabled", "disabled");
    var fdata = new FormData();
    var vendorName = $("#Name").val();
    var vendorid = Id;
    var defaultId = $("#VendorDefaults_Id").val();
    var Category = $("#VendorDefaults_Category").val();
    var GSTPercentage = $("#VendorDefaults_GSTPercentage").val();
    var PaymentMethod = $("#VendorDefaults_PaymentMethod").val();
    var PaymentTerms = $("#VendorDefaults_PaymentTerms").val();
    var TDSSection = $("#tdssectionsvalue").val();
    var TDSPercentage = $("#tdspercentage").val();
    var GST_TDSPercentage = $("#VendorDefaults_GST_TDSPercentage").val();

    fdata.append("Category", Category);
    fdata.append("GSTPercentage", GSTPercentage);
    fdata.append("Id", defaultId);
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

    if (GSTPercentage == "") {

        Swal.fire({
            text: 'Please enter  the gst percentage',
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

    if (TDSPercentage == "") {

        Swal.fire({
            text: 'Please enter the TDS %',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    var newUrl = `/Vendor/EditVendor?id=${Id}`;

    //var newUrl = '@Url.Action("EditVendor", "Vendor", new { Id = @Id })';

    $.ajax({
        url: "/VendorDefaults/EditVendorDefaults",
        type: "post",
        data: fdata,
        processData: false,
        contentType: false,
        success: function (result) {
            swal.fire({
                title: "Vendor : " + vendorName + "",
                text: "Vendor defaults details updated successfully!!",
                icon: "success",
                button: "Ok!",

            }).then((result,event) => {
                window.location.href = newUrl;
                $(".test3").addClass("invisible");
                event.preventDefault();
                $(".bankdisable").prop("disabled", true);

            })
        },
        error: function () {
            swal.fire({
                text: "Failed to add vendor default details!!",
                icon: "warning",
                button: "Ok!",

            }).then((result, event) => {
                $(".bankdisable").val("");
                $(".test3").addClass("invisible");
                event.preventDefault();


            })
        }
    });
});

$("#save5").on("click", function () {

    $(".test0").removeClass("invisible");
   
   
    var vendorName = $("#Name").val();
   // var vendorid = Id;
   // var Id = Id;
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

    var newUrl = `/Vendor/EditVendor?id=${Id}`;
   // var newUrl = '@Url.Action("EditVendor", "Vendor", new { Id = @Id })';
    $.ajax({
        url: "/Document/EditDocuments",
        type: "post",
        data: fdata,
        processData: false,
        contentType: false,
        success: function (result) {
            swal.fire({
                title: "Vendor : " + vendorName + "",
                text: "File uploaded successfully !",
                icon: "success",
                button: "Ok!",

            }).then((result, event) => {
               
                window.location.href = newUrl;
                $(".close").addClass("invisible");
                $("#FileUpload1").attr("disabled", "disabled");
                $(".test0").addClass("invisible");
                event.preventDefault();
                $(".fdisable").prop("disabled", true);
            })
        },
        error: function () {
            swal.fire({
                text: "Failed to upload files!!",
                icon: "warning",
                button: "Ok!",

            }).then((result) => {
                window.location.href = newUrl;
                $(".test0").removeClass("invisible");
            })
        }
    });

});
$("#save6").on("click", function () {

    // $(this).attr("disabled", "disabled");

    var fdata = new FormData();
    var GSTRegistration = $("#NewID").val();
    var GSTIN_Number = $("#disableGst").val();
    var Name = $("#Name").val();
    var OwnerOrDirectorName = $("#OwnerOrDirectorName").val();
    var PAN_Number = $("#panNo").val();
    var TAN_Number = $("#tanNo").val();
    var Notes = $("#Notes").val();
    var GST_TDS_Applicable = $("#GST_TDS_Applicable").val();
    var Status = $("#Status").val();
    var vendorid = $("#VendorPerson_VendorId").val();
    var contactid = $("#VendorPerson_Contacts_Id").val();
    var addressId = $("#VendorPerson_Addresses_Id").val();
    var vendorpersonid = $("#VendorPerson_Id").val();

    var Country = $("#VendorPerson_Addresses_Country").val();
    var confirmGstnumner = $("#disableConfirmGst").val();
    var confirmpannumber = $("#confirmpanNo").val();
    var confirmtannumber = $("#confirmtanNo").val();
    var Name1 = $("#VendorPerson_Contacts_Name").val();
    var Phone = $("#VendorPerson_Contacts_Phone").val();
    var Address = $("#VendorPerson_Addresses_Address").val();
    var City = $("#VendorPerson_Addresses_City").val();
    var State = $("#VendorPerson_Addresses_State").val();
    var PinCode = $("#VendorPerson_Addresses_PinCode").val();
    var Email = $("#VendorPerson_Contacts_Email").val();
    var vendorbankId = $("#VendorBankAccounts_Id").val();
    var BeneficiaryName = $("#VendorBankAccounts_BeneficiaryName").val();
    var selectedBranchDetails = $("#selBranchName").val().split(',');
    var selectedBankDetails = $("#selBankName").val().split(',');
    var BankMasterId = selectedBranchDetails[1];
    var AccountNumber = $("#accountNo").val();
    var confirmAccountNo = $("#confirmaccountNo").val();
    var defaultId = $("#VendorDefaults_Id").val();
    var Category = $("#VendorDefaults_Category").val();
    var GSTPercentage = $("#VendorDefaults_GSTPercentage").val();
    var PaymentMethod = $("#VendorDefaults_PaymentMethod").val();
    var PaymentTerms = $("#VendorDefaults_PaymentTerms").val();
    var TDSSection = $("#tdssectionsvalue").val();
    var TDSPercentage = $("#tdspercentage").val();
    var GST_TDSPercentage = $("#VendorDefaults_GST_TDSPercentage").val();



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


    fdata.append("Id", Id);
    fdata.append("GSTRegistration", GSTRegistration);
    fdata.append("GSTIN_Number", GSTIN_Number);
    fdata.append("Name", Name);
    fdata.append("OwnerOrDirectorName", OwnerOrDirectorName);
    fdata.append("PAN_Number", PAN_Number);
    fdata.append("TAN_Number", TAN_Number);
    fdata.append("GST_TDS_Applicable", GST_TDS_Applicable);
    fdata.append("Status", Status);
    fdata.append("Notes", Notes)
    fdata.append("VendorDefaults.Category", Category);
    fdata.append("VendorDefaults.GSTPercentage", GSTPercentage);
    fdata.append("VendorDefaults.Id", defaultId);

    fdata.append("VendorDefaults.VendorId", vendorid);
    fdata.append("VendorDefaults.PaymentMethod", PaymentMethod);
    fdata.append("VendorDefaults.GST_TDSPercentage", GST_TDSPercentage);
    fdata.append("VendorDefaults.PaymentTerms", PaymentTerms);
    fdata.append("VendorDefaults.TDSSection", TDSSection);
    fdata.append("VendorDefaults.TDSPercentage", TDSPercentage);
    fdata.append("VendorBankAccounts.BeneficiaryName", BeneficiaryName);
    fdata.append("VendorBankAccounts.Id", vendorbankId);
    fdata.append("VendorBankAccounts.VendorId", vendorid);
    fdata.append("VendorBankAccounts.BankMasterId", BankMasterId);
    fdata.append("VendorBankAccounts.AccountNumber", AccountNumber);
    fdata.append("VendorPerson.Addresses.Country", Country);
    fdata.append("VendorPerson.Id", vendorpersonid);
    fdata.append("VendorPerson.Contacts.Phone", Phone);
    fdata.append("VendorPerson.Contacts.Name", Name1);
    fdata.append("VendorPerson.Addresses.City", City);
    fdata.append("VendorPerson.Addresses.Address", Address);
    fdata.append("VendorPerson.Addresses.State", State);
    fdata.append("VendorPerson.Addresses.PinCode", PinCode);
    fdata.append("VendorPerson.Contacts.Email", Email);
    fdata.append("VendorPerson.VendorId", vendorid);
    fdata.append("VendorPerson.Addresses.Id", addressId);
    fdata.append("VendorPerson.Contacts.Id", contactid);
    fdata.append("VendorPerson.Contacts.VendorPersonID", vendorpersonid);
    fdata.append("VendorPerson.Addressess.VendorPersonID", vendorpersonid);


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
            text: 'Please enter the PAN number or GSTIN number or TAN number',
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

    if (City == "") {

        Swal.fire({
            text: 'Please select the City',
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
            text: 'Please enter the PAN number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (selectedBranchDetails == "") {

        Swal.fire({
            text: 'Please enter the PAN number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (AccountNumber == "") {

        Swal.fire({
            text: 'Please enter the PAN number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

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

    if (GSTPercentage == "") {

        Swal.fire({
            text: 'Please enter  the gst percentage',
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



    if (TDSPercentage = "") {

        Swal.fire({
            text: 'Please enter the TDS %',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }

    var newUrl = `/Vendor/EditVendor?id=${Id}`;

    //var newUrl = '@Url.Action("EditVendor", "Vendor", new { Id = @Id })';
    $.ajax({
        type: "POST",
        url: "/Vendor/Update",
        data: fdata,
        processData: false,
        contentType: false,
        success: function (result) {

            swal.fire({
                title: "Vendor : " + Name + "",
                text: "Updated  successfully!!",
                icon: "success",
                button: "Ok!",

            }).then((result) => {
               
                $(".test").addClass("invisible");
                //event.preventDefault();
                $(".vendordetailsdisable").prop("disabled", true);
                $(".close").addClass("invisible");
                $("FileUpload1").attr("disabled", "disabled");
                window.location.href = newUrl;


            })
        },
        error: function () {
            swal.fire({
                text: "Failed to update the vendor details!!",
                icon: "warning",
                button: "Ok!",

            }).then((result) => {
                window.location.href = newUrl;
                console.log('Failed ');

            })
        }
    });

});

$("#save7").on("click", function () {

    $(this).attr("disabled", false);
    
    var fdata = new FormData();
    var GSTRegistration = $("#NewID").val();
    var GSTIN_Number = $("#disableGst").val();
    var Name = $("#Name").val();
    var OwnerOrDirectorName = $("#OwnerOrDirectorName").val();
    var PAN_Number = $("#panNo").val();
    var TAN_Number = $("#tanNo").val();
    var Notes = $("#Notes").val();
    var GST_TDS_Applicable = $("#GST_TDS_Applicable").val();
    var Status = $("#Status").val();
    var vendorid = $("#VendorPerson_VendorId").val();
    var contactid = $("#VendorPerson_Contacts_Id").val();
    var addressId = $("#VendorPerson_Addresses_Id").val();
    var vendorpersonid = $("#VendorPerson_Id").val();
    var Country = $("#VendorPerson_Addresses_Country").val();
    var Name1 = $("#VendorPerson_Contacts_Name").val();
    var Phone = $("#VendorPerson_Contacts_Phone").val();
    var Address = $("#VendorPerson_Addresses_Address").val();
    var City = $("#VendorPerson_Addresses_City").val();
    var State = $("#VendorPerson_Addresses_State").val();
    var PinCode = $("#VendorPerson_Addresses_PinCode").val();
    var Email = $("#VendorPerson_Contacts_Email").val();
    var vendorbankId = $("#VendorBankAccounts_Id").val();
    var BeneficiaryName = $("#VendorBankAccounts_BeneficiaryName").val();
    var selectedBankDetails = $("#selBankName").val().split(',');
    var selectedBranchDetails = $("#selBranchName").val().split(',');
    var BankMasterId = selectedBranchDetails[1];
    var AccountNumber = $("#accountNo").val();
    var confirmAccountNo = $("#confirmaccountNo").val();
    var defaultId = $("#VendorDefaults_Id").val();
    var Category = $("#VendorDefaults_Category").val();
    var GSTPercentage = $("#VendorDefaults_GSTPercentage").val();
    var PaymentMethod = $("#VendorDefaults_PaymentMethod").val();
    var PaymentTerms = $("#VendorDefaults_PaymentTerms").val();
    var TDSSection = $("#tdssectionsvalue").val();
    var TDSPercentage = $("#tdspercentage").val();
    var GST_TDSPercentage = $("#VendorDefaults_GST_TDSPercentage").val();
    var confirmGstnumner = $("#disableConfirmGst").val();
    var confirmpannumber = $("#confirmpanNo").val();
    var confirmtannumber = $("#confirmtanNo").val();

    //var filedata = fileList;
    //var flength = filedata.length;
    //for (i = 0; i < flength; i++) {
    //    var data1 = filedata[i];
    //    for (j = 0; j <= data1.length; j++) {
    //        fdata.append('files', data1[j]);
    //    }
    //}
    //var postFileName = fileName;
    //var fName = postFileName.length;
    //for (i = 0; i < fName; i++) {
    //    var data1 = postFileName[i];
    //    fdata.append('DocumentName', data1);
    //}
    var filedata = fileList;
    console.log(filedata);
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






    fdata.append("Id", Id);
    fdata.append("GSTRegistration", GSTRegistration);
    fdata.append("GSTIN_Number", GSTIN_Number);
    fdata.append("Name", Name);
    fdata.append("OwnerOrDirectorName", OwnerOrDirectorName);
    fdata.append("PAN_Number", PAN_Number);
    fdata.append("TAN_Number", TAN_Number);
    fdata.append("GST_TDS_Applicable", GST_TDS_Applicable);
    fdata.append("Status", Status);
    fdata.append("Notes", Notes)
    fdata.append("VendorDefaults.Category", Category);
    fdata.append("VendorDefaults.GSTPercentage", GSTPercentage);
    fdata.append("VendorDefaults.Id", defaultId);
    fdata.append("VendorDefaults.VendorId", vendorid);
    fdata.append("VendorDefaults.PaymentMethod", PaymentMethod);
    fdata.append("VendorDefaults.GST_TDSPercentage", GST_TDSPercentage);
    fdata.append("VendorDefaults.PaymentTerms", PaymentTerms);
    fdata.append("VendorDefaults.TDSSection", TDSSection);
    fdata.append("VendorDefaults.TDSPercentage", TDSPercentage);
    fdata.append("VendorBankAccounts.BeneficiaryName", BeneficiaryName);
    fdata.append("VendorBankAccounts.Id", vendorbankId);
    fdata.append("VendorBankAccounts.VendorId", vendorid);
    fdata.append("VendorBankAccounts.BankMasterId", BankMasterId);
    fdata.append("VendorBankAccounts.AccountNumber", AccountNumber);
    fdata.append("VendorPerson.Addresses.Country", Country);
    fdata.append("VendorPerson.Id", vendorpersonid);
    fdata.append("VendorPerson.Contacts.Phone", Phone);
    fdata.append("VendorPerson.Contacts.Name", Name1);
    fdata.append("VendorPerson.Addresses.City", City);
    fdata.append("VendorPerson.Addresses.Address", Address);
    fdata.append("VendorPerson.Addresses.State", State);
    fdata.append("VendorPerson.Addresses.PinCode", PinCode);
    fdata.append("VendorPerson.Contacts.Email", Email);
    fdata.append("VendorPerson.VendorId", vendorid);
    fdata.append("VendorPerson.Addresses.Id", addressId);
    fdata.append("VendorPerson.Contacts.Id", contactid);
    fdata.append("VendorPerson.Contacts.VendorPersonID", vendorpersonid);
    fdata.append("VendorPerson.Addressess.VendorPersonID", vendorpersonid);


    if (GSTRegistration == "") {

        Swal.fire({
            text: 'Please select GST registration',
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
            text: 'Please enter the PAN number',
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

    if (City == "") {

        Swal.fire({
            text: 'Please select the City',
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
            text: 'Please enter the Bank Details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (selectedBranchDetails == "") {

        Swal.fire({
            text: 'Please enter the Branch details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    if (AccountNumber == "") {

        Swal.fire({
            text: 'Please enter the Account Number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

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

    if (GSTPercentage == "") {

        Swal.fire({
            text: 'Please enter  the gst percentage',
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


    if (TDSPercentage == "") {

        Swal.fire({
            text: 'Please enter the TDS %',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    var newUrl = `/Vendor/EditVendor?id=${Id}`;
  //  var newUrl = '@Url.Action("EditVendor", "Vendor", new { Id = @Id })';
    $.ajax({
        type: "POST",
        url: "/Vendor/Update",
        data: fdata,
        processData: false,
        contentType: false,
        success: function (result) {
           
            swal.fire({
                title: "Vendor : " + Name + "",
                text: "Updated  successfully!!",
                icon: "success",
                button: "Ok!",

            }).then((result) => {
             
            
                $(".test").addClass("invisible");
               // event.preventDefault();
                $(".vendordetailsdisable").prop("disabled", true);
                $(".close").addClass("invisible");
                $("FileUpload1").attr("disabled", "disabled");
                window.location.href = newUrl;
                


            })
        },
        error: function () {
            swal.fire({
                text: "Failed to update the vendor details!!",
                icon: "warning",
                button: "Ok!",

            }).then((result) => {
                window.location.href = newUrl;
                console.log('Failed ');

            })
        }
    });

});


$("#edit").click(function (event) {
    $(".acollapse").attr("checked", true);
    var selectedBank = $("#selBankName").val();
    if (selectedBank != '') {
        $("#selBranchName").find('option').hide();
        $("#selBranchName option[value='']").show();
        $('*[data="' + selectedBank + '"]').show();
    }
    $(".test").removeClass("invisible");
    $(".testsavebottom").removeClass("invisible");
    $(".test4").removeClass("invisible");
    $(".btnsave").attr("disabled", "disabled");
    $(".btnvendordetails").attr("disabled", "disabled");
    event.preventDefault();
    $(".vendordetailsdisable").prop("disabled", false); // Element(s) are now enabled.
    if ($('#NewID').val() == "Registered") {
        $("#disableConfirmGst").removeAttr("disabled");
        $("#disableGst").removeAttr("disabled", "disabled")
    } else {
        $("#disableConfirmGst").attr("disabled", "disabled");
        $("#disableGst").attr("disabled", "disabled")
        $("#GST_TDS_Applicable").attr("disabled", "disabled");

    }
    var selectedBranchDetails = $("#selBranchName").val().split(',');
    var selectedBranch = selectedBranchDetails[0];
    var BankMasterId = selectedBranchDetails[1];
    if (selectedBranch != '') {
        $("#selIfsc").val(selectedBranch);
        $("#selBankMasterId").val(BankMasterId);
        $("#branchid").val(BankMasterId);
    }
});

function myFunction(event) {
    $(".btnsave").removeAttr("disabled", "disabled");
    $(".btnvendordetails").removeAttr("disabled", "disabled");
    $(".vendordetailsdisable").removeAttr("disabled", "disabled");
    $(".test").addClass("invisible");
    $("#cancel").addClass("invisible"); $(".savetest").addClass("invisible");
   event.preventDefault();
    $(".vendordetailsdisable").prop("disabled", true);
    $("#disableConfirmGst").attr("disabled");
    $("#disableConfirmGst").attr("disabled", "disabled");
    $("#disableGst").attr("disabled");
    $("#disableGst").attr("disabled", "disabled");
}

$("#disableGst").on('change', function () {
    var GSTNumber = $(this).val();


    for (i = 0; i < GSTAvailable.length; i++) {
        if (GSTNumber == GSTAvailable[i]) {
            swal.fire({
                title: 'GST Number already exists',
                icon: 'warning',

                confirmButtonText: 'Ok',
            }).then((result) => {
                $(this).val("");
            })
        }
    }
});
$("#panNo").on('change', function () {
    var PanNumber = $(this).val();

    for (i = 0; i < PanAvailable.length; i++) {
        if (PanNumber == PanAvailable[i]) {
            swal.fire({
                title: 'PAN Number already exists',
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
            title: 'Account Number already exists',
            icon: 'warning',

            confirmButtonText: 'Ok',
        }).then((result) => {
            $(this).val("");
        })
    }
}
        });
function confirmation(e) {
    e.preventDefault();
    var url = e.currentTarget.getAttribute('href')
    Swal.fire({
        icon: 'warning',
        title: 'Are you sure?',
        text: 'The file will be deleted!',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'No',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            window.location.href = url;
        }
    })
}
$("#edit1").click(function (event) {
    $("#edit").attr("disabled", "disabled");
    $("#clear").attr("disabled", "disabled");
    $("#save6").attr("disabled", "disabled");
    $(".tests").removeClass("invisible");
    event.preventDefault();
    $(".vdisable").prop("disabled", false); // Element(s) are now enabled.
    if ($('#NewID').val() == "Registered") {
        $("#disableConfirmGst").removeAttr("disabled");
        $("#disableGst").removeAttr("disabled", "disabled")
        $("#GST_TDS_Applicable").removeAttr("disabled");
    } else {
        $("#disableConfirmGst").attr("disabled", "disabled");
        $("#disableGst").attr("disabled", "disabled")
        $("#GST_TDS_Applicable").attr("disabled", "disabled");
    }

});
$("#edit2").click(function (event) {
    $("#title2").attr("checked", "checked");
    $("#edit").attr("disabled", "disabled");
    $("#clear").attr("disabled", "disabled");
    $("#save6").attr("disabled", "disabled");
    $(".test1").removeClass("invisible");
    event.preventDefault();
    $(".persondisable").prop("disabled", false); // Element(s) are now enabled.
});
$("#edit3").click(function (event) {
    $("#edit").attr("disabled", "disabled");
    $("#title3").attr("checked", "checked");
    $(".test2").removeClass("invisible");
    var selectedBank = $("#selBankName").val();
    if (selectedBank != '') {
        $("#selBranchName").find('option').hide();
        $("#selBranchName option[value='']").show();
        $('*[data="' + selectedBank + '"]').show();
    }
    $("#clear").attr("disabled", "disabled");
    $("#save6").attr("disabled", "disabled");
    event.preventDefault();
    $(".defaultdisable").prop("disabled", false); // Element(s) are now enabled.
    var selectedBranchDetails = $("#selBranchName").val().split(',');
    var selectedBranch = selectedBranchDetails[0];
    var BankMasterId = selectedBranchDetails[1];
    if (selectedBranch != '') {
        $("#selIfsc").val(selectedBranch);
        $("#selBankMasterId").val(BankMasterId);
        $("#branchid").val(BankMasterId);
    }
});
$("#edit4").click(function (event) {
    $("#title4").attr("checked", "checked");
    $(".test3").removeClass("invisible");
    if ($("#VendorDefaults_GSTPercentage").val() == "") {
        $("#VendorDefaults_GSTPercentage").prop("disabled", false);
    }
    else {
        $("#VendorDefaults_GSTPercentage").prop("disabled", true)
    }
    $("#edit").attr("disabled", "disabled");
    $("#clear").attr("disabled", "disabled");
    $("#save6").attr("disabled", "disabled");
    event.preventDefault();
    $(".bankdisable").prop("disabled", false); // Element(s) are now enabled.

});
$("#edit5").click(function (event) {
    $("#title5").attr("checked", "checked");
    $(".test0").removeClass("invisible");
    $("#edit").attr("disabled", "disabled");
    $("#clear").attr("disabled", "disabled");
    $("#save6").attr("disabled", "disabled");
    event.preventDefault();
    $(".filedisable").prop("disabled", false); // Element(s) are now enabled.

});
$("#clear1").click(function (event) {
    $("#edit").removeAttr("disabled", "disabled");
    $(".tests").addClass("invisible");
    $("#test5").removeClass("invisible");
    $("#clear").removeAttr("disabled", "disabled");
    $("#save6").removeAttr("disabled", "disabled");
    if ($('#disableGst').val() != "") {
        $("#disableConfirmGst").attr("disabled", "disabled");
        $("#disableGst").attr("disabled", "disabled")
    }

    event.preventDefault();
    $(".vdisable").prop("disabled", true);
});
$("#clear2").click(function (event) {
    $("#edit").removeAttr("disabled", "disabled");
    $("#title2").attr("checked", false);
    $(".test1").addClass("invisible");
    $("#clear").removeAttr("disabled", "disabled");
    $("#save6").removeAttr("disabled", "disabled");

    event.preventDefault();
    $(".persondisable").prop("disabled", true);
});
$("#clear4").click(function (event) {
    $("#edit").removeAttr("disabled", "disabled");
    $("#title4").attr("checked", false);
    $("#VendorDefaults_GSTPercentage").attr("disabled", true);
    $(".test3").addClass("invisible");
    $("#clear").removeAttr("disabled", "disabled");
    $("#save6").removeAttr("disabled", "disabled");

    event.preventDefault();
    $(".bankdisable").prop("disabled", true);
});
$("#clear3").click(function (event) {
    $("#edit").removeAttr("disabled", "disabled");
    $("#title3").attr("checked", false);
    $(".test2").addClass("invisible");
    $("#clear").removeAttr("disabled", "disabled");
    $("#save6").removeAttr("disabled", "disabled");

    event.preventDefault();
    $(".defaultdisable").prop("disabled", true);
});
$("#clear5").click(function (event) {
    $("#FileUpload1").val("");
    $("#title5").attr("checked", false);
    $(".test0").addClass("invisible");
    // $(".test4").addClass("invisible");
    $(".list").text("");
    $("filecount").text("");
    $(".fname").each(function () {
        $(this).text("");
    });
    $("#edit").removeAttr("disabled", "disabled");
    $("#clear").removeAttr("disabled", "disabled");
    $("#save6").removeAttr("disabled", "disabled");

    event.preventDefault();
    $(".filedisable").prop("disabled", true); // Element(s) are now enabled.

});
$("#clear").click(function (event) {
    $(".acollapse").attr("checked", false);
    // $(".test4").addClass("invisible");
    $(".btnsave").removeAttr("disabled", "disabled");
    $(".btnvendordetails").removeAttr("disabled", "disabled");
    $(".vendordetailsdisable").removeAttr("disabled", "disabled");
    $(".savetest").addClass("invisible");
    $("#cancel").addClass("invisible");
    event.preventDefault();
    $(".vendordetailsdisable").prop("disabled", true);

    $("#disableConfirmGst").attr("disabled");
    $(".test").addClass("invisible");
    $("#disableConfirmGst").attr("disabled", "disabled");
    $("#disableGst").attr("disabled");
    $("#disableGst").attr("disabled", "disabled");
    event.preventDefault(); // Element(s) are now enabled.

});
$("#addBill").on("click", function () {
   

   

    var newUrl = `/Vendor/EditVendor?id=${Id}`;
    var url = `/Bill/AddBill?id=${Id}`;
    

    //var newUrl = '@Url.Action("EditVendor","Vendor",new { Id = @Id })';

    //var url = '@Url.Action("AddBill","Bill",new { Id = @Id })';

    if ($("#VendorDefaults_GSTPercentage").val() == "") {

        swal.fire({
            title: 'Vendor Default Details Not Found',
            text: 'Please Update Vendor Default Details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then((result) => {

            window.location.href = newUrl;

        })
    }
    else {
        return window.location.href = url;
    }

});
$("#addBillForUnreg").on("click", function () {

 

    var newUrl = `/Vendor/EditVendor?id=${Id}`;
    var url = `/Bill/AddBillForUnregistered?id=${Id}`;
    

   // var newUrl = '@Url.Action("EditVendor","Vendor",new { Id = @Id })';

   // var url = '@Url.Action("AddBillForUnregistered","Bill",new { Id = @Id })';

    if ($("#tdspercentage").val() == "") {

        swal.fire({
            title: 'Vendor Default Details Not Found',
            text: 'Please Update Vendor Default Details',
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then((result) => {

            window.location.href = newUrl;

        })
    }
    else {
        return window.location.href = url;
    }

});
$("#addPayment").on("click", function () {
    

    var newUrl = `/Vendor/EditVendor?id=${Id}`;
    var url = `/Payment/AddPayment?id=${Id}`;
    //var newUrl = '@Url.Action("EditVendor","Vendor",new { Id = @Id })';

    //var url = '@Url.Action("AddPayment","Payment",new { Id = @Id })';

    if ($("#VendorDefaults_GSTPercentage").val() == "") {

        swal.fire({
            title: 'Vendor default details not found',
            text: 'Please update vendor default Details!!',
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then((result) => {

            window.location.href = newUrl;

        })
    }

    else {
        return window.location.href = url;
    }

});


