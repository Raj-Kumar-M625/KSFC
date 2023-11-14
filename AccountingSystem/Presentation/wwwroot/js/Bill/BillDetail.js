const today = new Date().toISOString().split('T')[0];
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

        var filename = this.files[0].name;


        var selFileName = (filename + "," + selectedFileValue)
        fileName.push(selFileName)
        fileList1.push(this.files);
        ;

        $('#filetype').val('');
        if (this.files.length > 0) {
            for (var i = 0; i < this.files.length; i++) {
                var rows = "";

                rows += '<tr class="addedrow"><td>' + filename + '</td> <td>' + selectedFileValue + '</td><td></td> <td ><i value="Delete" type="button" class="fa fa-trash fa1 ml-5"></i></td>  </tr>';
                $(rows).appendTo("#example tbody");

            }
        }


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


            })
        })
    }
})

function Validates() {
    var fileCount = document.getElementById("example").getElementsByTagName("tr").length;
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

$("#approvebill").on("click", function () {
    var newUrl = `/Bill/Index`;
    //var newUrl = '@Url.Action("Index","Bill")';
    var Status = "Approved";
    var billref = $("#billreferenceno").val();
    var Remarks = $("#Remarks").val();
    if (Remarks == null || Remarks == "") {
        Swal.fire({
            text: 'Remarks is required',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        return false;
    }

    $.ajax({
        type: 'Post',
        url: '/Bill/ApproveBillPayments',
        traditional: true,
        data: { BillRefNo: billref, Remarks: Remarks, Status: Status },
        success: function (res) {

            Swal.fire({
                title: 'Bill Ref No.' + billref + '',
                text: 'Bills approved successfully!!',
                icon: 'success',
                confirmButtonText: 'Ok',

            }).then((result) => {
                window.location.href = newUrl;
            });


        },
        error: function (err) {
            Swal.fire({
                title: 'Bill Ref No.' + billref + ',',
                text: 'An error orccured while approving the bills!! ',
                icon: 'error',
                confirmButtonText: 'Ok',

            })
            return false;
        }
    });


});
$("#rejectbill").on("click", function () {
    var newUrl = `/Bill/Index`;
    //var newUrl = '@Url.Action("Index","Bill")';
    var billref = $("#billreferenceno").val();
    var Status = "Rejected";
    var Remarks = $("#Remarks").val();

    if (Remarks == null || Remarks == "") {
        Swal.fire({
            text: 'Remarks is required',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        return false;
    }

    $.ajax({
        type: 'Post',
        url: '/Bill/ApproveBillPayments',
        traditional: true,
        data: { BillRefNo: billref, Remarks: Remarks, Status: Status },
        success: function (res) {

            Swal.fire({
                title: 'Bill Ref No.' + billref + '',
                text: 'Bills rejected successfully!!',
                icon: 'success',
                confirmButtonText: 'Ok',

            }).then((result) => {
                window.location.href = newUrl;
            });


        },
        error: function (err) {
            Swal.fire({
                title: 'Bill Ref No.' + billref + ',',
                text: 'An error orccured while approving the bills!! ',
                icon: 'error',
                confirmButtonText: 'Ok',

            })
            return false;
        }
    });


});
$("#edit").click(function (event) {
    $("#txtother2").removeAttr('style');
    $("#txtother3").removeAttr('style');
    $("#txtother1").removeAttr('style');
    $("#BillDate").removeAttr('disabled');
    $("#DueDate").removeAttr('disabled');
    $("div").removeClass("awe-disabled");
    $("button").removeAttr('disabled');
    $(".test").removeClass("invisible");
    $(".disbtn").removeClass("invisible");
    $(".addbilltd").removeClass("invisible");
    $("#ApproveDate").prop("disabled", true);
    $(".buttoncolurchnage").addClass("btn-danger");
    $(".buttoncolurchnage").removeClass("btn-secondary");
    $(".editrow").prop("hidden", false);
    event.preventDefault();
    $(".vendordetailsdisable").prop("disabled", false);
});

$("#clear").on("click", function (event) {
    $("#txtother2").attr("style", "background-color: #e9ecef;");
    $("#txtother3").attr("style", "background-color: #e9ecef;");
    $("#txtother1").attr("style", "background-color: #e9ecef;");  
    $("#BillDate").prop("disabled", true);
    $("#DueDate").prop("disabled", true);
    $(".addbilltd").addClass("invisible");
    $(".test").addClass("invisible");
    $(".disbtn").addClass("invisible");
    event.preventDefault();
    $(".buttoncolurchnage").addClass(" btn-secondary");
    $(".buttoncolurchnage").removeClass("btn-danger");
    $(".vendordetailsdisable").prop("disabled", true);
    $(".editrow").prop("hidden", true);
    $(".addedrow").prop("hidden", true)


});

    var billAmount = document.getElementById("hiddenBillAmount").value;
    var GSTAmount = document.getElementById("hiddenGSTAmount").value;
    var totalBillAmount = document.getElementById("HiddenTotalBillAmount").value;
    var TDSAmount = document.getElementById("HiddenTDSAmount").value;
    var GSTTDSAmount = document.getElementById("HiddenGSTTDSAmount").value;
    var netPayable = document.getElementById("HiddenNetPayable").value;
    var royalty = document.getElementById("HiddenRoyalty").value;
    var CBF = document.getElementById("HiddenCBF").value;
    var labourWelfareCess = document.getElementById("HiddenLabourWelfareCess").value;
    var penalty = document.getElementById("HiddenPenalty").value;
    var other1 = document.getElementById("HiddenOther1").value;
    var other1Value = document.getElementById("HiddenOther1Value").value;
    var other2 = document.getElementById("HiddenOther2").value;
    var other2Value = document.getElementById("HiddenOther2Value").value;
    var other3 = document.getElementById("HiddenOther3").value;
    var other3Value = document.getElementById("HiddenOther3Value").value;

    $("#billtotal").val(billAmount);
    $("#txtgstval").val(GSTAmount);
    $("#subtotal").val(totalBillAmount);
    totBillAmt = parseInt(totalBillAmount);
    $("#tds").val("-" + TDSAmount);
    $("#gsttds").val("-" + GSTTDSAmount);
    $("#cbf").val("-" + CBF);
    $("#labourwelfare").val("-" + labourWelfareCess);
    $("#txtRoyalty").val("-" + royalty);
    $("#txtPenalty").val("-" + penalty);
    $("#lblother1").val(other1);
    $("#txtother1").val(other1Value);
    $("#lblother2").val(other2);
    $("#txtother2").val(other2Value);
    $("#lblother3").val(other3);
    $("#txtother3").val(other3Value);
    $("#totalpayable").val(netPayable);

const san = [];
const doc = [];
const path = [];
var gstPercen = 0;
var totBillAmt = 0;

function funChangeValue(button) {

    var row = $(button).closest("TR");
    var val = $("TD", row).eq(2).find('input').val();
    if (val <= 0) {
        Swal.fire({
            text: 'Please enter the Valid amount',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })

        return false;
    }
    var getvalue = 0;
    var billtotalamount = 0;
    var amount = val * 1;
    var gst = $("TD", row).eq(3).find('select').val();
    var total = (parseFloat(amount) * parseFloat(gst)) / 100;
    $("TD", row).eq(4).text(total.toFixed(2));
    $("TD", row).eq(4).addClass("input-amount gstAmount");
    var billamount = parseFloat(amount) + total;
    $("TD", row).eq(5).text(billamount.toFixed(2));
    $("TD", row).eq(5).addClass("input-amount textBaseAmount");


    $("#tblAddBills .text1").each(function () {
        var totalamt = $(this).val();
        if ($.isNumeric(totalamt)) {
            getvalue += parseFloat(totalamt);
        }
    });


    var gstvaluetemp = 0;
    $("#tblAddBills .gstAmount").each(function () {
        var totalamt = $(this).val();
        if (totalamt == "") {
            totalamt = row.find("TD").eq(4).html();
        }
        if ($.isNumeric(totalamt)) {
            gstvaluetemp += parseFloat(totalamt);
        }
    });

    $("#txtgstval").val(gstvaluetemp.toFixed(2));

    var totalBillAmountValue = 0;
    $("#tblAddBills .textBaseAmount").each(function () {
        var totalbase = $(this).val();
        if (totalbase == "") {
            totalbase = row.find("TD").eq(5).html();
        }
        if ($.isNumeric(totalbase)) {
            totalBillAmountValue += parseFloat(totalbase);
        }

    });



    $(".amttest").each(function () {
        var totalamts = $(this).text();
        if ($.isNumeric(totalamts)) {
            billtotalamount += parseFloat(totalamts);
        }
    });


    var amountWithOutGst = (getvalue + billtotalamount);
    $("#billtotal").val(amountWithOutGst.toFixed(2))
    $("#subtotal").val(totalBillAmountValue.toFixed(2));


    //TDS && GST - TDS Calulation

    var tds = document.getElementById("vendortds").value;
    var subtotaltds = parseFloat($("#billtotal").val());
    var subtds = (subtotaltds * tds) / 100;
    $("#tds").val("-" + subtds.toFixed(2));

    var gsttds = document.getElementById("vendotgsttds").value;
    var subtotalgsttds = parseFloat($("#billtotal").val());
    var subgsttds = (subtotalgsttds * gsttds) / 100;
    if (isNaN(subgsttds)) {
        subgsttds = 0;
    }
    $("#gsttds").val("-" + subgsttds.toFixed(2));

    var cbf = parseFloat($("#cbfValue").val());
    var subtotalCBF = parseFloat($("#billtotal").val());
    var subCBF = (subtotalCBF * cbf) / 100;
    $("#cbf").val("-" + subCBF.toFixed(2));


    var labourWelfare = parseFloat($("#labourWelfareValue").val());
    var subtotalLabourWelfare = parseFloat($("#billtotal").val());
    var subLabourWelfare = (subtotalLabourWelfare * labourWelfare) / 100;
    $("#labourwelfare").val("-" + subLabourWelfare.toFixed(2));



    var totalpayablesubtotal = parseFloat($("#subtotal").val());
    var minusvalue = -1;
    var totalpayabletds = (parseFloat($("#tds").val()) * minusvalue);
    var totalpayablegsttds = (parseFloat($("#gsttds").val()) * minusvalue);
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
    var deductedAmount = (totalpayabletds + totalpayablegsttds + totalpayablecbf + totalpayablelabourWelfare + txtRoyalty + txtPenalty);
    var OtherAmount = ((txtother2) + (txtother3) + (txtother1))
    var totalpay = (totalpayablesubtotal) - (deductedAmount);
    var resTotal = (totalpay) + (OtherAmount);
    $('#totalpayable').val(resTotal.toFixed(2));
}






function Remove1(button) {
    var row = $(button).closest("TR");
    swal.fire({
        icon: 'warning',
        title: 'Are you sure?',
        text: 'The file will be deleted!!',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            var docid;
            docid = $("TD", row).eq(0).find('input').val();
            doc.push(docid);
            console.log(doc);
            var docpath;
            docpath = $("TD", row).eq(6).find('input').val();
            path.push(docpath);
            console.log(path);

            var table = $("#example")[0];
            //Delete the Table row using it's Index.
            table.deleteRow(row[0].rowIndex);
            swal.fire(
                'Deleted!',
                'Deleted.',
                'Success'
            )
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
        }
    })

};


function Remove(button) {

    var row = $(button).closest("TR");
    var refer = $("TD", row).eq(0).html();
    if (refer == "") {
        var fdfs = $("TD", row).eq(2).html();

    }
    swal.fire({
        icon: 'warning',
        title: 'Are you sure?',
        text: 'The row will be deleted!!',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            var total;
            var refernum;
            if (fdfs != null) {
                total = fdfs;
            }
            else {
                total = $("TD", row).eq(2).find('input').val();
                refernum = $("TD", row).eq(0).find('input').val();
                san.push(refernum);
                console.log(san);
            }

            var gstval = 0;
            gstval = $("TD", row).eq(4).html();
            if (isNaN(gstval)) {
                gstval = $("TD", row).eq(4).find('input').val();
            }
            var billtotalamount = parseFloat($("#billtotal").val());
            var totBillAmountRemove = billtotalamount - parseFloat(total);
            $("#billtotal").val(totBillAmountRemove.toFixed(2));

            var gstsubval = parseFloat($("#txtgstval").val());
            var totGSTAmountRemove = gstsubval - parseFloat(gstval);
            $("#txtgstval").val(totGSTAmountRemove.toFixed(2));

            var subTotalRemove = parseFloat($("#billtotal").val()) + parseFloat($("#txtgstval").val());
            $("#subtotal").val(subTotalRemove.toFixed(2));


            var tds = document.getElementById("vendortds").value;
            var subtotaltds = parseFloat($("#billtotal").val());
            var subtds = (subtotaltds * parseFloat(tds)) / 100;


            $("#tds").val("-" + subtds.toFixed(2));

            var gsttds = document.getElementById("vendotgsttds").value;
            var subtotalgsttds = parseFloat($("#billtotal").val());
            var subgsttds = (subtotalgsttds * parseFloat(gsttds)) / 100;
            if (isNaN(subgsttds)) {
                subgsttds = 0;
            }

            $("#gsttds").val("-" + subgsttds.toFixed(2));


            var cbf = parseFloat($("#cbfValue").val());
            var subtotalCBF = parseFloat($("#billtotal").val());
            var subCBF = (subtotalCBF * cbf) / 100;
            $("#cbf").val("-" + subCBF.toFixed(2));


            var labourWelfare = parseFloat($("#labourWelfareValue").val());
            var subtotalLabourWelfare = parseFloat($("#billtotal").val());
            var subLabourWelfare = (subtotalLabourWelfare * labourWelfare) / 100;
            $("#labourwelfare").val("-" + subLabourWelfare.toFixed(2));

            var totalpayablesubtotal = parseFloat($("#subtotal").val());
            var minusvalue = -1;
            var totalpayabletds = (parseFloat($("#tds").val()) * minusvalue);
            var totalpayablegsttds = (parseFloat($("#gsttds").val()) * minusvalue);
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
            var deductedAmount = (totalpayabletds + totalpayablegsttds + totalpayablecbf + totalpayablelabourWelfare + txtRoyalty + txtPenalty);
            var OtherAmount = ((txtother2) + (txtother3) + (txtother1))
            var totalpay = (totalpayablesubtotal) - (deductedAmount);
            var resTotal = (totalpay) + (OtherAmount);
            $('#totalpayable').val(resTotal.toFixed(2));


            var table = $("#tblAddBills")[0];
            //Delete the Table row using it's Index.
            table.deleteRow(row[0].rowIndex);
        } else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {

        }
    })

};

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
    if ((txtother1 < 0 || txtother1 > 0) && txtother1 != "") {
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
    if ((txtother2 < 0 || txtother2 > 0) && txtother2 != "") {
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
    if ((txtother3 < 0 || txtother3 > 0) && txtother3 != "") {
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
            text: 'Please select the bill date',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        return false;
    }
    DueDate = $("#DueDate").val();
    if (DueDate == null || DueDate == "") {
        Swal.fire({
            text: 'Please select the due date',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        return false;
    }
    if (DueDate < BillDate) {
        Swal.fire({
            text: 'Due date cannot be lesser than due date',
            icon: 'warning',
            confirmButtonText: 'Ok',

        })
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


    var Referencenobill = $("#billreferenceno").val();
    var id = $("#VendorId").val();
    var billTotal = $("#billtotal").val();
    var tds = $("#tds").val();
    var gsttds = $("#gsttds").val();
    var totalpayable = $("#totalpayable").val();
    var billsid = $("#billpaymentid").val();
    var billsData = new Array();
    var newUrl = `/Bill/Index`;
    //var newUrl = '@Url.Action("Index","Bill")';
    $("#tblAddBills TBODY TR").each(function () {
        var row = $(this);
        var refernum;
        var bill = {};
        var billID = $("TD", row).eq(0).html();
        if (billID != null) {
            billID = $("TD", row).eq(0).find('input').val();
            bill.Id = billID;
        } else {
            bill.Id = null;
        }

        var conceptName = row.find("TD").eq(1).find(":selected").text();
        if (conceptName == "") {
            bill.Category = row.find("TD").eq(1).html();
        } else {
            bill.Category = conceptName;
        }

        var amt = $("TD", row).eq(2).find('input').val();
        if (amt == "" || amt === undefined) {
            bill.Amount = row.find("TD").eq(2).html();
        } else {
            bill.Amount = $("TD", row).eq(2).find('input').val();
        }

        var gst = $("TD", row).eq(3).find(':selected').val();
        if (gst == "" || gst === undefined) {
            bill.GSTSWithholdPercent = row.find("TD").eq(3).html();
        } else {
            bill.GSTSWithholdPercent = gst;
        }
        var gstamtfin = $("TD", row).eq(4).find('input').val();
        if (gstamtfin == "" || gstamtfin === undefined) {
            bill.GSTAmount = row.find("TD").eq(4).html();
        } else {
            bill.GSTAmount = $("TD", row).eq(4).find('input').val();
        }


        var totbillam = $("TD", row).eq(5).find('input').val();
        if (totbillam == "" || totbillam === undefined) {
            bill.BaseAmount = row.find("TD").eq(5).html();
        } else {
            bill.BaseAmount = $("TD", row).eq(5).find('input').val();
        }

        var balanceAmt = $("TD", row).eq(5).find('input').val();
        if (balanceAmt == "" || balanceAmt === undefined) {
            bill.BalanceAmount = row.find("TD").eq(5).html();
        } else {
            bill.BalanceAmount = $("TD", row).eq(5).find('input').val();
        }



        var descrp = $("TD", row).eq(6).find('input').val();
        if (descrp == "" || descrp === undefined) {
            bill.Description = row.find("TD").eq(6).html();
        } else {
            bill.Description = $("TD", row).eq(6).find('input').val();
        }
        billsData.push(bill);
    });

    var billpayemntdetails = new Array();
    var billpay = {};
    billpay.BillDate = BillDate;
    billpay.BillDueDate = DueDate;
    billpay.BillNo = $("#vendorBillNumber").val();
    billpay.BillAmount = $("#billtotal").val();
    billpay.TotalBillAmount = $("#subtotal").val();
    billpay.TDS = $("#tds").val();
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
    var gsttds = document.getElementById("vendotgsttds").value;
    var tds = document.getElementById("vendortds").value;
    billpay.TDSWithholdPercent = tds;
    var gstValTemp = parseInt($("#txtgstval").val());
    if (isNaN(gstValTemp)) {
        gstValTemp = 0;
    }
    billpay.GSTTDSWithholdPercent = gsttds;
    billpay.GSTAmount = gstValTemp;
    billpay.GSTTDS = $("#gsttds").val();
    billpay.NetPayable = $("#totalpayable").val();
    billpay.VendorId = $("#VendorId").val();
    billpay.Id = $("#billpaymentid").val();
    billpayemntdetails.push(billpay);
    console.log(billpayemntdetails)
    $.ajax({
        type: "POST",
        url: "/Bill/UpdateBillRecords",
        data: { billsData: billsData, deltedval: san, docdeltedval: doc, docpath: path, billpaymentdata: billpayemntdetails },
        success: function (result) {
            var id = result.data;
            var referencenores = result.referenceNo;
            console.log(referencenores);
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
                    swal.fire({
                        title: "Bill Ref No.  " + referencenores + " ",
                        text: "Bill details updated successfully!!",
                        icon: "success",
                        button: "Ok !",

                    }).then((result) => {
                        
                        window.location.href = newUrl;

                    })
                }
            });

        },
        error: function () {
            swal.fire({
                title: "Bill Ref No.  " + referencenores + " ",
                text: "Failed update bill details!!",
                icon: "waring",
                iconcolor: "orange",
                button: "Ok !",

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



function calculateGSTAmount(val) {
    if (val == "") {
        val = 0;
    }
    var amount = parseFloat($("#txtAmount").val());
    var gst = parseFloat(val);
    var total = (amount * gst) / 100;
    $("#txtGStAmountTD").val(total.toFixed(2));
    var totalbillamount = parseFloat(amount) + parseFloat(total);
    document.getElementById('txtBillAmount').value = totalbillamount.toFixed(2);
}

function calculateBillAmount(val) {
    if (val == "") {
        val = 0;
    }
    var gst = parseFloat($("#txtGst").val());
    if (isNaN(gst)) {
        return false;
    }
    var amount = parseFloat(val);
    var total = (amount * gst) / 100;
    $("#txtGStAmountTD").val(total.toFixed(2));
    var totalbillamount = parseFloat(amount) + parseFloat(total);
    document.getElementById('txtBillAmount').value = totalbillamount.toFixed(2);
}


$("#btnAdd").on("click", function () {
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
            text: 'Please select bill the category',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        return false;
    }

    var selectedGST = $("#txtGst").val();
    if (selectedGST == null || selectedGST == "") {
        Swal.fire({
            text: 'Please select GST percentage',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        return false;
    }




    var txtAmount = parseFloat($("#txtAmount").val());
    var gstAmTSub = parseFloat($("#txtGStAmountTD").val());
    var txtGst = parseFloat($("#txtGst").val());
    var txtBillAmountWithGst = parseFloat($("#txtBillAmount").val());
    var txtDescription = $("#txtDescription");


    var totalBillAmount = parseFloat($("#billtotal").val());
    if (totalBillAmount > 0) {
        var billAmount = txtAmount + totalBillAmount;
        $("#billtotal").val(billAmount.toFixed(2));
    } else {
        $("#billtotal").val(txtAmount.toFixed(2));
    }


    var gstamount = parseFloat($("#txtgstval").val());
    if (gstamount > 0) {
        var billgst = gstAmTSub + gstamount;
        $("#txtgstval").val(billgst.toFixed(2));
    } else {
        $("#txtgstval").val(gstAmTSub.toFixed(2));
    }




    var AmountWithGST = parseFloat($("#subtotal").val());
    if (AmountWithGST > 0) {
        var billAmt = txtBillAmountWithGst + AmountWithGST;
        $("#subtotal").val(billAmt.toFixed(2));
    } else {
        $("#subtotal").val(txtBillAmountWithGst.toFixed(2));
    }


    //TDS && GST - TDS Calulation

    var tds = document.getElementById("vendortds").value;
    var subtotaltds = parseFloat($("#billtotal").val());
    var subtds = (subtotaltds * tds) / 100;
    $("#tds").val("-" + subtds.toFixed(2));

    var gsttds = document.getElementById("vendotgsttds").value;
    var subtotalgsttds = parseFloat($("#billtotal").val());
    var subgsttds = (subtotalgsttds * gsttds) / 100;
    if (isNaN(subgsttds)) {
        subgsttds = 0;
    }
    $("#gsttds").val("-" + subgsttds.toFixed(2));

    var cbf = parseFloat($("#cbfValue").val());
    var subtotalCBF = parseFloat($("#billtotal").val());
    var subCBF = (subtotalCBF * cbf) / 100;
    $("#cbf").val("-" + subCBF.toFixed(2));

    var labourWelfare = parseFloat($("#labourWelfareValue").val());
    var subtotalLabourWelfare = parseFloat($("#billtotal").val());
    var subLabourWelfare = (subtotalLabourWelfare * labourWelfare) / 100;
    $("#labourwelfare").val("-" + subLabourWelfare.toFixed(2));

    var totalpayablesubtotal = parseFloat($("#subtotal").val());
    var minusvalue = -1;
    var totalpayabletds = (parseFloat($("#tds").val()) * minusvalue);
    var totalpayablegsttds = (parseFloat($("#gsttds").val()) * minusvalue);
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
    var deductedAmount = (totalpayabletds + totalpayablegsttds + totalpayablecbf + totalpayablelabourWelfare + txtRoyalty + txtPenalty);
    var OtherAmount = ((txtother2) + (txtother3) + (txtother1))
    var totalpay = (totalpayablesubtotal) - (deductedAmount);
    var resTotal = (totalpay) + (OtherAmount);
    $('#totalpayable').val(resTotal.toFixed(2));

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblAddBills > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);
    $(row).addClass("trow")

    //Add Name cell.
    var cell = $(row.insertCell(-1));
    cell.attr("hidden", true);

    cell = $(row.insertCell(-1));
    cell.html(selectedBank);

    cell = $(row.insertCell(-1));
    cell.html(txtAmount.toFixed(2)).addClass(" input-amount amttest");

    cell = $(row.insertCell(-1));
    cell.html(selectedGST);


    cell = $(row.insertCell(-1));
    cell.html(gstAmTSub.toFixed(2)).addClass(" input-amount");

    cell = $(row.insertCell(-1));
    cell.html(txtBillAmountWithGst.toFixed(2)).addClass(" input-amount amtbill");


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
    $("#selCategory").val('');
    $("#txtAmount").val('');
    $("#txtGst").val('');
    $("#txtGStAmountTD").val('');
    $("#txtBillAmount").val('');
    $("#txtDescription").val('');

});

function calculateTotalPayable() {
    var subtotal = parseFloat($('#subtotal').val()) || 0;
    var tds = parseFloat($('#tds').val()) || 0;
    var gsttds = parseFloat($('#gsttds').val()) || 0;
    var cbf = parseFloat($('#cbf').val()) || 0;
    var labourwelfare = parseFloat($('#labourwelfare').val()) || 0;
    var txtRoyalty = parseFloat($('#txtRoyalty').val()) || 0;
    if (txtRoyalty > 0) {
        txtRoyalty *= -1;
    }
    var txtPenalty = parseFloat($('#txtPenalty').val()) || 0;
    if (txtPenalty > 0) {
        txtPenalty *= -1;
    }
    var txtother1 = parseFloat($('#txtother1').val()) || 0;
    var txtother2 = parseFloat($('#txtother2').val()) || 0;
    var txtother3 = parseFloat($('#txtother3').val()) || 0;
    $('#txtRoyalty').val((txtRoyalty).toFixed(2));
    $('#txtPenalty').val((txtPenalty).toFixed(2));
    var deductedAmount = -1 * (tds + gsttds + cbf + labourwelfare + txtRoyalty + txtPenalty);
    var otherAmount = txtother1 + txtother2 + txtother3;
    var totalpay = subtotal - deductedAmount;
    var resTotal = totalpay + otherAmount;
    debugger

    $('#txtother1').val(txtother1.toFixed(2));
    $('#txtother2').val(txtother2.toFixed(2));
    $('#txtother3').val(txtother3.toFixed(2));
    $('#totalpayable').val(resTotal.toFixed(2));
}

$('#txtRoyalty, #txtPenalty, #txtother1, #txtother2, #txtother3').on('change', calculateTotalPayable);

