﻿const today = new Date().toISOString().split('T')[0];
const element = document.getElementById('BillDate');
if (element) {
    element.max = today;
}

Date.prototype.addDays = function (num) {
    var value = this.valueOf();
    value += 86400000 * num;
    return new Date(value);
}


$('#BillDate').on('change', function () {
    
    var $datepicker = $('#BillDate');
    var $datepicker1 = $('#DueDate');
    var selectedDate = $datepicker.val();
    var result = new Date(selectedDate);
    var dateval = result.getDate();
    var ndays = $('#txtPaymentTerms').val();
    numdays = Number(ndays);
    var expirationDate = result.addDays(numdays);
    end_date1 = new Date(expirationDate);
    var day = ("0" + end_date1.getDate()).slice(-2);
    var month = ("0" + (end_date1.getMonth() + 1)).slice(-2);
    var year = end_date1.getFullYear();
    var newdate = year + '-' + month + '-' + day;
    $datepicker1.val(newdate);
});

var fileList1 = [];
var fileName = [];
var fileCount = 0;

inputTypeFiles = document.getElementById('File');
inputTypeFiles.addEventListener('change', function () {
    var selectedFileValue = $('#filetype').val();
    if (selectedFileValue == "" | null) {
        Swal.fire({
            text: 'Please select the file type',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        return false;
    }
    else {
        // document.getElementById("uploadTable").style.visibility = "visible";
        $("#uploadTable").prop("hidden", false)
        var filename = this.files[0].name;


        var selFileName = (filename + "," + selectedFileValue)
        fileName.push(selFileName)
        fileList1.push(this.files);

        $('#filetype').val('');
        if (this.files.length > 0) {
            for (var i = 0; i < this.files.length; i++) {
                var table = document.getElementById("uploadTable");
                var row = '<tr><td>' + filename + '</td> <td>' + selectedFileValue + '</td> <td ><i value="Delete" type="button" class="fa fa-trash fa1 ml-4"></i></td>  </tr>'

                table.innerHTML = table.innerHTML + row;
            }
        }

        closeIT = document.getElementsByClassName('fa1')
        arrayFormCloseBtn = [...closeIT];
        arrayFormCloseBtn.forEach((onebyone) => {
            onebyone.addEventListener('click', function (e) {
                ;
                var data2 = [];
                var td = e.target.parentNode;
                var tr = td.parentNode;
                tr.parentNode.removeChild(tr);
                var Cells = tr.getElementsByTagName("td");
                var object = Cells[0].innerText;
                console.log(fileList1)
                var flength = fileList1.length;

                for (i = 0; i < flength; i++) {
                    var data1 = fileList1[i];
                    if (data1 != undefined) {


                        for (j = 0; j <= data1.length; j++) {



                            if (data1[j] != undefined) {


                                var data2 = data1[j].name;

                                if (data2 == object) {

                                    if (data1[j].name == object) {
                                        var spliced = fileList1.splice(i, 1);

                                    }



                                }
                            }


                        }

                    }

                }
                console.log(fileList1)

            })
        })

    }



})
function Validates() {
    fileCount = document.getElementById("uploadTable").getElementsByTagName("tr").length;
    if (fileCount > 5) {
        Swal.fire({
            text: 'Maximum 5 files only allowed',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        document.getElementById('File').value = null;
        return false;
    }
}

$("#btnAdd").on("click", function () {
    var x = document.getElementById("tblAddBills").rows.length;
    if (x < 12) {
        var txt = $("#txtAmount").val();
        if (txt == null || txt == "") {
            Swal.fire({
                text: 'Please enter the amount',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })

            return false;
        }
        if (txt <= 0) {
            Swal.fire({
                text: 'Please enter the amount',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })

            return false;
        }

        var selectedBank = $("#selCategory").val();
        if (selectedBank == null || selectedBank == "") {
            Swal.fire({
                text: 'Please select the bill category',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })
            return false;
        }

        var txtAmount = parseFloat($("#txtAmount").val());
        var txtDescription = $("#txtDescription");

        var billtotalamount = parseFloat($("#billtotal").val());
        if (isNaN(billtotalamount)) {
            billtotalamount = 0;
        } if (billtotalamount > 0) {
            var billAmt = billtotalamount + txtAmount;
            $("#billtotal").val(billAmt.toFixed(2));
        } else {
            $("#billtotal").val(txtAmount.toFixed(2));
        }


        var tds = document.getElementById("vendortds").value;
        var subtotaltds = parseFloat($("#billtotal").val());
        var subtds = (subtotaltds * tds) / 100;

        $("#tds").val("-" + subtds.toFixed(2));

        var cbf = parseFloat($("#cbfValue").val());
        var subtotalCBF = parseFloat($("#billtotal").val());
        var subCBF = (subtotalCBF * cbf) / 100;
        $("#cbf").val("-" + subCBF.toFixed(2));

        var labourWelfare = parseFloat($("#labourWelfareValue").val());
        var subtotalLabourWelfare = parseFloat($("#billtotal").val());
        var subLabourWelfare = (subtotalLabourWelfare * labourWelfare) / 100;
        $("#labourwelfare").val("-" + subLabourWelfare.toFixed(2));

        var totalpayablesubtotal = parseFloat($("#billtotal").val());
        if (isNaN(totalpayablesubtotal)) {
            totalpayablesubtotal = 0;
        }
        var minusvalue = -1;
        var totalpayabletds = (parseFloat($("#tds").val()) * minusvalue);
        var totalpayablecbf = (parseFloat($("#cbf").val()) * minusvalue);
        var totalpayablelabourWelfare = (parseFloat($("#labourwelfare").val()) * minusvalue);

        var txtRoyalty = parseFloat($('#txtRoyalty').val() * minusvalue);
        if (isNaN(txtRoyalty)) {
            txtRoyalty = 0;
        }
        var txtPenalty = parseFloat($('#txtPenalty').val() * minusvalue);
        if (isNaN(txtPenalty)) {
            txtPenalty = 0;
        }
        var txtother1 = parseFloat($('#txtother1').val());
        if (isNaN(txtother1)) {
            txtother1 = 0;
        }
        var txtother2 = parseFloat($('#txtother2').val());
        if (isNaN(txtother2)) {
            txtother2 = 0;
        }

        var txtother3 = parseFloat($('#txtother3').val());
        if (isNaN(txtother3)) {
            txtother3 = 0;
        }
        var deductedAmount = (totalpayabletds + totalpayablecbf + totalpayablelabourWelfare + txtRoyalty + txtPenalty);
        var OtherAmount = ((txtother2) + (txtother3) + (txtother1))
        var totalpay = (totalpayablesubtotal) - (deductedAmount);
        var resTotal = (totalpay) + (OtherAmount);
        $('#totalpayable').val(resTotal.toFixed(2));

        //Get the reference of the Table's TBODY element.
        var tBody = $("#tblAddBills > TBODY")[0];

        //Add Row.
        var row = tBody.insertRow(-1);
        $(row).addClass("trow");
        //Add Name cell.
        var cell = $(row.insertCell(-1));
        cell.html(selectedBank);

        cell = $(row.insertCell(-1));
        cell.html(txtAmount.toFixed(2)).addClass(" input-amount");


        cell = $(row.insertCell(-1));
        cell.html(txtDescription.val())

        //Add Button cell.
        cell = $(row.insertCell(-1));
        var btnRemove = $("<input />");
        btnRemove.attr("type", "button");
        btnRemove.attr("onclick", "Remove(this);");
        btnRemove.attr("class", "btn btn-danger");
        btnRemove.val("Remove");
        cell.append(btnRemove);

        //Clear the TextBoxes.
        $("#txtAmount").val('');
        $("#txtDescription").val('');
        $("#selCategory").val('');
    }
    else {
        Swal.fire(
            'Add Bill',
            'Maximun 10 bills are Accepted!!',
            'warning'
        )
        $("#selCategory").val('');
        $("#txtAmount").val('');
        $("#txtDescription").val('');
    }

});
function Remove(button) {
    var row = $(button).closest("TR");
    swal.fire({

        text: "Are you sure to delete this bill?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            var total = $("TD", row).eq(1).html();

            var billtotalamount = parseFloat($("#billtotal").val());
            var removedBillAmount = billtotalamount - parseFloat(total);
            $("#billtotal").val(removedBillAmount.toFixed(2));

            var tds = document.getElementById("vendortds").value;
            var subtotaltds = parseFloat($("#billtotal").val());
            var subtds = (subtotaltds * tds) / 100;
            $("#tds").val("-" + subtds.toFixed(2));


            var cbf = parseFloat($("#cbfValue").val());
            var subtotalCBF = parseFloat($("#billtotal").val());
            var subCBF = (subtotalCBF * cbf) / 100;
            $("#cbf").val("-" + subCBF.toFixed(2));


            var labourWelfare = parseFloat($("#labourWelfareValue").val());
            var subtotalLabourWelfare = parseFloat($("#billtotal").val());
            var subLabourWelfare = (subtotalLabourWelfare * labourWelfare) / 100;
            $("#labourwelfare").val("-" + subLabourWelfare.toFixed(2));

            var totalpayablesubtotal = parseFloat($("#billtotal").val());
            var minusvalue = -1;
            var totalpayabletds = (parseFloat($("#tds").val()) * minusvalue);
            var totalpayablecbf = (parseFloat($("#cbf").val()) * minusvalue);
            var totalpayablelabourWelfare = (parseFloat($("#labourwelfare").val()) * minusvalue);

            var txtRoyalty = parseFloat($('#txtRoyalty').val() * minusvalue);
            if (isNaN(txtRoyalty)) {
                txtRoyalty = 0;
            }
            var txtPenalty = parseFloat($('#txtPenalty').val() * minusvalue);
            if (isNaN(txtPenalty)) {
                txtPenalty = 0;
            }

            var txtother1 = parseFloat($('#txtother1').val());
            if (isNaN(txtother1)) {
                txtother1 = 0;
            }

            var txtother2 = parseFloat($('#txtother2').val());
            if (isNaN(txtother2)) {
                txtother2 = 0;
            }

            var txtother3 = parseFloat($('#txtother3').val());
            if (isNaN(txtother3)) {
                txtother3 = 0;
            }
            var deductedAmount = (totalpayabletds + totalpayablecbf + totalpayablelabourWelfare + txtRoyalty + txtPenalty);
            var OtherAmount = ((txtother2) + (txtother3) + (txtother1))
            var totalpay = (totalpayablesubtotal) - (deductedAmount);
            var resTotal = (totalpay) + (OtherAmount);
            $('#totalpayable').val(resTotal.toFixed(2));

            var table = $("#tblAddBills")[0];
            //Delete the Table row using it's Index.
            table.deleteRow(row[0].rowIndex);
            swal.fire(
                'Deleted!',
                'Deleted.',
                'success'
            )
        } else if (result.dismiss === Swal.DismissReason.cancel) {
            //swal.fire(
            //    'Cancelled',
            //    'Not Deleted',
            //    'info'
            //)
        }
    })

};

var newUrl = `/Bill/Index`;
$("#btnSubmit").on("click", function (e) {
    $("#btnSubmit").attr("disabled", "disabled");
    var totalPayable = $('#totalpayable').val();
    if (totalPayable < 0) {
        Swal.fire({
            text: 'Total payable amount should be greater than zero',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        $("#btnSubmit").removeAttr("disabled");
        return false;
    }
    var txtother1 = $('#txtother1').val();
    var lblother1 = $('#lblother1').val();
    if ((txtother1 <= 0 || txtother1 >= 0) && txtother1 != "") {
        if (lblother1 == null || lblother1 == "") {
            Swal.fire({
                text: 'Please enter the Other1 value',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })
            $("#btnSubmit").removeAttr("disabled");
            return false;
        }
    }
    var txtother2 = $('#txtother2').val();
    var lblother2 = $('#lblother2').val();
    if ((txtother2 <= 0 || txtother2 >= 0) && txtother2 != "") {
        if (lblother2 == null || lblother2 == "") {
            Swal.fire({
                text: 'Please enter the Other2 value',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })
            $("#btnSubmit").removeAttr("disabled");
            return false;
        }
    }
    var txtother3 = $('#txtother3').val();
    var lblother3 = $('#lblother3').val();
    if ((txtother3 <= 0 || txtother3 >= 0) && txtother3 != "") {
        if (lblother3 == null || lblother3 == "") {
            Swal.fire({
                text: 'Please enter the Other3 value',
                icon: 'warning',
                confirmButtonText: 'Ok',
            })
            $("#btnSubmit").removeAttr("disabled");
            return false;
        }
    }
    var txtRoyalty = $("#txtRoyalty").val();
    if (txtRoyalty == null || txtRoyalty == "") {
        Swal.fire({
            text: 'Please enter the royalty amount',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        $("#btnSubmit").removeAttr("disabled");
        return false;
    }

    BillDate = $("#BillDate").val();
    if (BillDate == null || BillDate == "") {
        Swal.fire({
            text: 'Please Select the Bill Date',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        $("#btnSubmit").removeAttr("disabled");
        return false;
    }
    DueDate = $("#DueDate").val();
    if (DueDate == null || DueDate == "") {
        Swal.fire({
            text: 'Please select the due date',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        $("#btnSubmit").removeAttr("disabled");
        return false;
    }
    if (DueDate < BillDate) {
        Swal.fire({
            text: 'Bill date cannot be greater than due date',
            icon: 'warning',
            confirmButtonText: 'Ok',

        })
        $("#btnSubmit").removeAttr("disabled");
        return false;
    }


    var billTot = $("#billtotal").val();
    if (billTot == null || billTot == "" || billTot == 0) {
        Swal.fire({
            text: 'Please add the bill',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        return false;
    }



    var billNumber = $("#vendorBillNumber").val();
    if (billNumber == null || billNumber == "" || billNumber == 0) {
        Swal.fire({
            text: 'Please add the bill number',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        $("#btnSubmit").removeAttr("disabled");
        return false;
    }



    //Loop through the Table rows and build a JSON array.
    var billsData = new Array();
    $("#tblAddBills TBODY TR").each(function () {
        var row = $(this);
        var bill = {};
        bill.Category = row.find("TD").eq(0).html();
        bill.Amount = row.find("TD").eq(1).html();
        bill.BaseAmount = row.find("TD").eq(1).html();
        bill.Description = row.find("TD").eq(2).html();
        billsData.push(bill);
    });
    
    var newUrl = `/Bill/Index`;
   // var newUrl = '@Url.Action("Index","Bill")';
    var billpayemntdetails = new Array();
    var billpay = {};
    billpay.BillDate = BillDate;
    billpay.BillNo = billNumber;
    billpay.BillDueDate = DueDate;
    billpay.BillAmount = $("#billtotal").val();
    billpay.TDS = $("#tds").val();
    var tds = document.getElementById("vendortds").value;
    billpay.TDSWithholdPercent = tds;
    billpay.Royalty = txtRoyalty;
    billpay.CBF = $("#cbf").val();
    billpay.LabourWelfareCess = $("#labourwelfare").val();
    billpay.Penalty = $("#txtPenalty").val();
    billpay.Other1 = $('#lblother1').val();
    billpay.Other1Value = parseFloat($("#txtother1").val());
    billpay.Other2 = $('#lblother2').val();
    billpay.Other2Value = parseFloat($("#txtother2").val());
    billpay.Other3 = $('#lblother3').val();
    billpay.Other3Value = parseFloat($("#txtother3").val());
    billpay.TotalBillAmount = $("#billtotal").val();
    billpay.NetPayable = $("#totalpayable").val();
    billpay.VendorId = $("#VendorId").val();
    billpayemntdetails.push(billpay);
    console.log(billpayemntdetails)
    $.ajax({
        type: "POST",
        url: "/Bill/AddBillRecords",
        data: { billsData: billsData, billpaymentdata: billpayemntdetails },
        success: function (result) {
        
            var id = result.data;
            var referencenores = result.referenceno;
            var fdata = new FormData();
            var filedata = fileList1;
            var flength = filedata.length;
            for (i = 0; i < flength; i++) {
                var data1 = filedata[i];
                for (j = 0; j <= data1.length; j++) {
                    fdata.append('File', data1[j]);
                    fdata.append('BillsID', id);
                }
            }

            var postFileName = fileName;
            var fName = postFileName.length;
            for (i = 0; i < fName; i++) {
                var data1 = postFileName[i];
                fdata.append('DocumentName', data1);
            }



            $.ajax({
                type: 'post',
                url: "/Document/AddBillDocument",
                data: fdata,
                processData: false,
                contentType: false,
                success: function (result) {
                   
                    // do something with the result now
                    console.log(result);
                    swal.fire({
                        title: "Bill Ref No.  " + referencenores + " ",
                        text: "Bill details saved successfully!!",
                        icon: "success",
                        button: "Ok",

                    }).then((result) => {
                        window.location.href = newUrl;

                    })
                }
            });

        },
        error: function () {
            swal.fire({
                title: "Bill Ref No.  " + referencenores + " ",
                text: "Failed to update bill details!!",
                icon: "warning",
                iconcolor: "orange",
                button: "Ok",
            }).then((result) => {
                window.location.href = newUrl;

            })
        }
    });
});





function enforceNumberValidation(ele) {
    if ($(ele).data('decimal') != null) {
        // found valid rule for decimal
        var decimal = parseInt($(ele).data('decimal')) || 0;
        var val = $(ele).val();
        if (decimal > 0) {
            var splitVal = val.split('.');
            var decimalvalue = parseInt(splitVal[0]);
            if (splitVal.length == 2 && splitVal[1].length > decimal) {
                // user entered invalid input

                $(ele).val(splitVal[0].substr(0, 6) + '.' + splitVal[1].substr(0, decimal));
            } else if (splitVal[0].length > 6) {
                if (isNaN(splitVal[1])) {
                    $(ele).val(splitVal[0].substr(0, 6));
                } else {
                    $(ele).val(splitVal[0].substr(0, 6) + '.' + splitVal[1]);
                }

            }
        } else if (decimal == 0) {
            // do not allow decimal place
            var splitVal = val.split('.');
            if (splitVal.length > 1) {
                // user entered invalid input
                $(ele).val(splitVal[0]); // always trim everything after '.'
            }
        }
    }
}

// fix the number format
$("table").on("change", "input", function () {
    var row = $(this).closest("tr");
    var RevBudgetCurrentFY = parseFloat(row.find(".actualamount").val());
    if (isNaN(RevBudgetCurrentFY)) {
        RevBudgetCurrentFY = null;
    } else {
        row.find(".actualamount").val(RevBudgetCurrentFY.toFixed(2));
    }
});

$("#vendorBillNumber").on('change', function () {
    var billNumber = $(this).val();
   

for (i = 0; i < billNumberAvailable.length; i++) {
    if (billNumber == billNumberAvailable[i]) {
        swal.fire({
            title: 'Bill number already exists',
            icon: 'warning',

            confirmButtonText: 'Ok',
        }).then((result) => {
            $(this).val("");
        })
    }
}
    });
$('#txtRoyalty').on('change', function () {
    var enteredValue = parseFloat($(this).val());
    if (isNaN(enteredValue)) {
        enteredValue = 0;
    }
    $('#txtRoyalty').val("-" + enteredValue.toFixed(2));

    var subtotal = parseFloat($('#billtotal').val());
    if (isNaN(subtotal)) {
        subtotal = 0;
    }
    var tds = parseFloat($('#tds').val());
    if (isNaN(tds)) {
        tds = -0;
    }

    var cbf = parseFloat($('#cbf').val());
    if (isNaN(cbf)) {
        cbf = -0;
    }
    var labourwelfare = parseFloat($('#labourwelfare').val());
    if (isNaN(labourwelfare)) {
        labourwelfare = -0;
    }
    var txtPenalty = parseFloat($('#txtPenalty').val());
    if (isNaN(txtPenalty)) {
        txtPenalty = -0;
    }

    var txtother1 = parseFloat($('#txtother1').val());
    if (isNaN(txtother1)) {
        txtother1 = 0;
    }

    var txtother2 = parseFloat($('#txtother2').val());
    if (isNaN(txtother2)) {
        txtother2 = 0;
    }

    var txtother3 = parseFloat($('#txtother3').val());
    if (isNaN(txtother3)) {
        txtother3 = 0;
    }
    var deductedAmount = ((tds * -1) + (cbf * -1) + (labourwelfare * -1) + (enteredValue) + (txtPenalty * -1));
    var OtherAmount = ((txtother2) + (txtother3) + (txtother1))
    var totalpay = (subtotal) - (deductedAmount);
    var resTotal = (totalpay) + (OtherAmount);
    $('#totalpayable').val(resTotal.toFixed(2));

});

$('#txtPenalty').on('change', function () {
    var enteredValue = parseFloat($(this).val());
    if (isNaN(enteredValue)) {
        enteredValue = 0;
    }
    $('#txtPenalty').val("-" + enteredValue.toFixed(2));

    var subtotal = parseFloat($('#billtotal').val());
    if (isNaN(subtotal)) {
        subtotal = 0;
    }
    var tds = parseFloat($('#tds').val());
    if (isNaN(tds)) {
        tds = -0;
    }
    var cbf = parseFloat($('#cbf').val());
    if (isNaN(cbf)) {
        cbf = -0;
    }
    var labourwelfare = parseFloat($('#labourwelfare').val());
    if (isNaN(labourwelfare)) {
        labourwelfare = -0;
    }
    var txtRoyalty = parseFloat($('#txtRoyalty').val());
    if (isNaN(txtRoyalty)) {
        txtRoyalty = -0;
    }

    var txtother1 = parseFloat($('#txtother1').val());
    if (isNaN(txtother1)) {
        txtother1 = 0;
    }

    var txtother2 = parseFloat($('#txtother2').val());
    if (isNaN(txtother2)) {
        txtother2 = 0;
    }

    var txtother3 = parseFloat($('#txtother3').val());
    if (isNaN(txtother3)) {
        txtother3 = 0;
    }
    var deductedAmount = ((tds * -1) + (cbf * -1) + (labourwelfare * -1) + (enteredValue) + (txtRoyalty * -1));
    var OtherAmount = ((txtother2) + (txtother3) + (txtother1))
    var totalpay = (subtotal) - (deductedAmount);
    var resTotal = (totalpay) + (OtherAmount);
    $('#totalpayable').val(resTotal.toFixed(2));

});
$('#txtother1').on('change', function () {
    var enteredValue = parseFloat($(this).val());
    if (isNaN(enteredValue)) {
        enteredValue = 0;
    }
    $('#txtother1').val(enteredValue.toFixed(2));

    var txtother2 = parseFloat($('#txtother2').val());
    if (isNaN(txtother2)) {
        txtother2 = 0;
    }

    var txtother3 = parseFloat($('#txtother3').val());
    if (isNaN(txtother3)) {
        txtother3 = 0;
    }

    var subtotal = parseFloat($('#billtotal').val());
    if (isNaN(subtotal)) {
        subtotal = -0;
    }
    var tds = parseFloat($('#tds').val());
    if (isNaN(tds)) {
        tds = -0;
    }

    var cbf = parseFloat($('#cbf').val());
    if (isNaN(cbf)) {
        cbf = -0;
    }
    var labourwelfare = parseFloat($('#labourwelfare').val());
    if (isNaN(labourwelfare)) {
        labourwelfare = -0;
    }
    var txtRoyalty = parseFloat($('#txtRoyalty').val());
    if (isNaN(txtRoyalty)) {
        txtRoyalty = -0;
    }
    var txtPenalty = parseFloat($('#txtPenalty').val());
    if (isNaN(txtPenalty)) {
        txtPenalty = -0;
    }
    var deductedAmount = ((tds * -1) + (cbf * -1) + (labourwelfare * -1) + (txtRoyalty * -1) + (txtPenalty * -1));
    var OtherAmount = ((txtother2) + (txtother3) + (enteredValue))
    var totalpay = (subtotal) - (deductedAmount);
    var resTotal = (totalpay) + (OtherAmount);
    $('#totalpayable').val(resTotal.toFixed(2));
});



$('#txtother2').on('change', function () {
    var enteredValue = parseFloat($(this).val());
    if (isNaN(enteredValue)) {
        enteredValue = 0;
    }
    $('#txtother2').val(enteredValue.toFixed(2));

    var txtother1 = parseFloat($('#txtother1').val());
    if (isNaN(txtother1)) {
        txtother1 = 0;
    }

    var txtother3 = parseFloat($('#txtother3').val());
    if (isNaN(txtother3)) {
        txtother3 = 0;
    }

    var subtotal = parseFloat($('#billtotal').val());
    if (isNaN(subtotal)) {
        subtotal = -0;
    }
    var tds = parseFloat($('#tds').val());
    if (isNaN(tds)) {
        tds = -0;
    }

    var cbf = parseFloat($('#cbf').val());
    if (isNaN(cbf)) {
        cbf = -0;
    }
    var labourwelfare = parseFloat($('#labourwelfare').val());
    if (isNaN(labourwelfare)) {
        labourwelfare = -0;
    }
    var txtRoyalty = parseFloat($('#txtRoyalty').val());
    if (isNaN(txtRoyalty)) {
        txtRoyalty = -0;
    }
    var txtPenalty = parseFloat($('#txtPenalty').val());
    if (isNaN(txtPenalty)) {
        txtPenalty = -0;
    }
    var deductedAmount = ((tds * -1) + (cbf * -1) + (labourwelfare * -1) + (txtRoyalty * -1) + (txtPenalty * -1));
    var OtherAmount = ((txtother1) + (txtother3) + (enteredValue))
    var totalpay = (subtotal) - (deductedAmount);
    var resTotal = (totalpay) + (OtherAmount);
    $('#totalpayable').val(resTotal.toFixed(2));
});


$('#txtother3').on('change', function () {
    var enteredValue = parseFloat($(this).val());
    if (isNaN(enteredValue)) {
        enteredValue = 0;
    }
    $('#txtother3').val(enteredValue.toFixed(2));

    var txtother1 = parseFloat($('#txtother1').val());
    if (isNaN(txtother1)) {
        txtother1 = 0;
    }

    var txtother2 = parseFloat($('#txtother2').val());
    if (isNaN(txtother2)) {
        txtother2 = 0;
    }

    var subtotal = parseFloat($('#billtotal').val());
    if (isNaN(subtotal)) {
        subtotal = -0;
    }
    var tds = parseFloat($('#tds').val());
    if (isNaN(tds)) {
        tds = -0;
    }
    var cbf = parseFloat($('#cbf').val());
    if (isNaN(cbf)) {
        cbf = -0;
    }
    var labourwelfare = parseFloat($('#labourwelfare').val());
    if (isNaN(labourwelfare)) {
        labourwelfare = -0;
    }
    var txtRoyalty = parseFloat($('#txtRoyalty').val());
    if (isNaN(txtRoyalty)) {
        txtRoyalty = -0;
    }
    var txtPenalty = parseFloat($('#txtPenalty').val());
    if (isNaN(txtPenalty)) {
        txtPenalty = -0;
    }
    var deductedAmount = ((tds * -1) + (cbf * -1) + (labourwelfare * -1) + (txtRoyalty * -1) + (txtPenalty * -1));
    var OtherAmount = ((txtother1) + (txtother2) + (enteredValue))
    var totalpay = (subtotal) - (deductedAmount);
    var resTotal = (totalpay) + (OtherAmount);
    $('#totalpayable').val(resTotal.toFixed(2));
});

