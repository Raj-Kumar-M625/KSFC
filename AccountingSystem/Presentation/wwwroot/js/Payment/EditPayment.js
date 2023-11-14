var selItems = [];
//var totalAmount = 0;
var totalPayableAmount = 0;
var button = document.getElementById("myButton");
//Load Total Payable column
$("#tblAddPayments ").on('input', '.text1', function () {

    var getvalue = 0;
    var totalp = 0;
    var totalb1 = 0;
    var totalb2 = 0;
    var totalb = 0;
  


    $("#tblAddPayments .trow").each(function () {

        var tvalue = ($(this).find("td:eq(2)").text());  //22050
        totalp += parseFloat(tvalue);  //22050
       

        var tvalueb = ($(this).find("td:eq(0) input[type='number']").val());
        var totalbb = parseFloat(tvalueb);
        var balancres = parseFloat(tvalue) - parseFloat(tvalueb);
        if (balancres < 0) {
            $(this).find("td:eq(1) input[type='hidden']").text(parseFloat(tvalue));
            $(this).find("td:eq(1)").text(parseFloat(tvalue));
        } else {
            $(this).find("td:eq(1) input[type='hidden']").text(balancres);
            $(this).find("td:eq(1)").text(balancres);
        }




        var tvalue2 = ($(this).find("td:eq(1)").text());   //21110.00
        if (parseFloat(tvalue2) == 0) {
            totalb1 += parseFloat(tvalue);
        }
        else {
            totalb2 += parseFloat(tvalue2);   //21110.00
        }
        if (totalb1 > 0) {
            totalb = totalb2 + totalb1;
        }
        else {
            totalb = totalb2;   //21110.00
        }
    });


    //get net payble and append it to total payable
    $("#tblAddPayments .text1").each(function () {
       

        var totalamt = $(this).val();  //10
        if (totalamt > 0) {
            let submitButton = document.getElementById('submit');
            submitButton.disabled = false;
        }

        if ($.isNumeric(totalamt)) {

            getvalue += parseFloat(totalamt);  //10

        }

    });
    var openingBalance = parseFloat($('#Payments_PaymentAmountAgainstOB').val());
    if (isNaN(openingBalance)) {
        openingBalance = 0;
    }
    var advanceUsedForPayments = $("#advanceAmountUsedForPayment").val();
    if (advanceUsedForPayments == "" || isNaN(advanceUsedForPayments)) {
        advanceUsedForPayments = 0
    }
    var totaladvanceAmount = 0;
    var selectedISval = [];

    if (advancePayments.Count() > 0) {
        debugger;
    var grid = document.getElementById("tblListOfAdvancePayments");
    var checkBoxes = grid.getElementsByTagName("INPUT");
    for (i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].checked) {
            var row = checkBoxes[i].parentNode.parentNode;
            var amount = row.cells[5].innerHTML;
            var paidamount = row.cells[6].innerHTML;
            selectedISval.push(row.cells[1].innerHTML)

            if (parseFloat(amount) <= 0) {
                totaladvanceAmount = parseFloat(totaladvanceAmount) + parseFloat(paidamount);

            }
            totaladvanceAmount = parseFloat(totaladvanceAmount) + parseFloat(amount);
        }
    }

    if (totaladvanceAmount > 0) {
        //var actualtotal = parseFloat(totalPayableAmount) + getvalue;
        totalPayableAmount = getvalue;
        var balance = totaladvanceAmount - getvalue;
        for (i = 0; i < selItems.length; i++) {
            for (j = 0; j < selectedISval.length; j++) {
                if (selItems[i].ID == selectedISval[j]) {
                    selItems[i].BalanceAmount = balance < 0 ? 0 : balance;
                }
            }
        }

        var selItemsJson = JSON.stringify(selItems);
        $("#advancePayments").val(selItemsJson);

        var afterAdvanceDeduction = getvalue - totaladvanceAmount - advanceUsedForPayments;
        if (afterAdvanceDeduction < 0) {
            var pay = 0
            $("#totalpayable").val((pay + openingBalance).toFixed(2)); //10
            $("#actualTotalPayable").val((pay + openingBalance).toFixed(2)); //10

        }
        else {
            $("#totalpayable").val((afterAdvanceDeduction + openingBalance).toFixed(2)); //10
            $("#actualTotalPayable").val((afterAdvanceDeduction + openingBalance).toFixed(2)); //10

        }
    }
    else {
        $("#totalpayable").val((getvalue + openingBalance - advanceUsedForPayments).toFixed(2)); //10
        $("#actualTotalPayable").val((getvalue + openingBalance - advanceUsedForPayments).toFixed(2)); //10
    }
}
        else {
    $("#totalpayable").val((getvalue + openingBalance - advanceUsedForPayments).toFixed(2)); //10
    $("#actualTotalPayable").val((getvalue + openingBalance - advanceUsedForPayments).toFixed(2)); //10

}

$("#balance").val((totalp - getvalue).toFixed(2));

    });

//Load Remaining Payable Column



    var balancevalue = 0;
    var txt = 0;
    var txt2 = 0;
    var txt3 = 0;
    var tds = 0;
    var pay = 0;
    var bal = 0;
    var gsttds = 0;
    var total = "";
    $("#tblAddPayments .trow").each(function () {
        var tvalue = ($(this).find("td:eq(8)").text());

        var totalp = parseFloat(tvalue);
        $(this).find("td:eq(8)").text(totalp)

        var tvalue2 = ($(this).find("td:eq(9)").text());
        var totalb = parseFloat(tvalue2);
        $(this).find("td:eq(9)").text(totalb)
    });

    $("#tblAddPayments .trow").each(function () {
        var tvalue = ($(this).find("td:eq(8)").text());

        var totalp = parseInt(tvalue);
        tds += parseInt(totalp);

        var tvalue2 = ($(this).find("td:eq(9)").text());
        var totalb = parseInt(tvalue2);
        gsttds += parseInt(totalb);


    });


    $("#payabledeuction").val((tds + gsttds).toFixed(2));

    //laod remaining payable
    $("#tblAddPayments .trow").each(function () {

        var tvaluep = ($(this).find("td:eq(2)").text());

        var totalpp = parseFloat(tvaluep);
        pay += parseFloat(totalpp);

        var tvalueb = ($(this).find("td:eq(0) input[type='number']").val());
        var totalbb = parseFloat(tvalueb);
        bal += parseFloat(totalbb);

    });
    //var openingBalance = $("#txtOpeningBalance").val();
    //var openingBalancePayableAmount = parseFloat($("#Payments_PaymentAmountAgainstOB").val());
    //if (isNaN(openingBalancePayableAmount)) {
    //    openingBalancePayableAmount = 0;
    //}
    //if (openingBalance < 0) {
    //    var res = bal - openingBalancePayableAmount;
    //    $('#totalpayable').val(res.toFixed(2));
    //} else {
    //    var res = bal + openingBalancePayableAmount;
    //    $('#totalpayable').val(res.toFixed(2));
    //}

    var advanceUsedForPayments = $("#advanceAmountUsedForPayment").val();
    if (advanceUsedForPayments == "") {
        advanceUsedForPayments = 0
    }
    if (!isNaN(advanceUsedForPayments)) {
        var actualPayment = bal - advanceUsedForPayments;
        $("#actualTotalPayable").val(actualPayment);
        $('#totalpayable').val(actualPayment.toFixed(2));

    }
    else {
        $('#totalpayable').val(bal.toFixed(2));
        $('#actualTotalPayable').val(bal.toFixed(2));

    }
    $("#balance").val((pay - bal).toFixed(2));


    //var table = document.getElementById('tblListOfAdvancePayments');
    //for (var i = 0, row; row = table.rows[i]; i++) {
    //    var cell = row.cells[4];
    //    if (cell.innerHTML > 0) {
    //        var checkbox = row.querySelector('#advancePaymentCheckBox');
    //        checkbox.checked = true;
    //        var model = {
    //            ID: row.cells[1].innerHTML,
    //            PaymentAmount: row.cells[5].innerHTML,
    //            BalanceAmount: 0
    //        };
    //        selItems.push(model);
    //    }
    //}
    var selItemsJson = JSON.stringify(selItems);
    $("#advancePayments").val(selItemsJson);



$('td').on('input', '.text1', function () {

    var $row = jQuery(this).closest('tr');

    var $columnA = $row.find('td:eq(2)').text();
    var colA = parseFloat($columnA);

    var $columnB = $row.find('td:eq(1)').text();

    var colB = parseFloat($columnB);



    var $columnC = $row.find("td:eq(0) input[type='number']").val();
    var colC = parseFloat($columnC);


    if (colC > colA) {
        swal.fire({
            title: 'Net payable cannot be greater than the payable amount',
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then((result) => {
            $(this).val("0.00");
            $("#balance").val("");
            $("#totalpayable").val("");
            $("#actualTotalPayable").val("");
        })
    }
});



//$('#Payments_PaymentAmountAgainstOB').on('change', function () {
//    var getTotalNetPayableValue = 0;
//    var enteredValue = parseFloat($(this).val());
//    if (isNaN(enteredValue)) {
//        enteredValue = 0;
//    }
//    var openingBalanceHidden = parseFloat($('#Payments_Vendor_VendorBalance_OpeningBalance').val());
//    var openingBalancePayableHidden = parseFloat($('#HiddenPaymentAmountAgainstOB').val());

//    var openingBalanceValue = 0;
//    var openingBalance = parseFloat($('#txtOpeningBalance').val());
//    if (isNaN(openingBalance)) {
//        openingBalance = 0;
//    }
//    debugger
//    if (openingBalancePayableHidden < enteredValue){
//        openingBalanceValue = openingBalanceHidden - (enteredValue - openingBalancePayableHidden)
//    }else{
//        openingBalanceValue = openingBalanceHidden + (openingBalancePayableHidden - enteredValue);
//    }

//    $("#tblAddPayments .text1").each(function () {
//        var totalamt = $(this).val();  //10
//        if ($.isNumeric(totalamt)) {
//            getTotalNetPayableValue += parseFloat(totalamt);  //10
//        }
//    });

//    if (openingBalance < 0) {
//        if (getTotalNetPayableValue < enteredValue) {
//            enteredValue = 0;
//            swal.fire({
//                title: 'Payable amount cannot be greater than the Total Net Payable',
//                icon: 'warning',
//                confirmButtonText: 'Ok',
//            }).then((result) => {
//                $(this).val("0.00");
//            })
//        }
//    }
//    if (openingBalance < enteredValue) {
//        enteredValue = 0;
//        openingBalanceValue = openingBalanceHidden + openingBalancePayableHidden;
//        swal.fire({
//            title: 'Payable amount cannot be greater than the opening balance amount',
//            icon: 'warning',
//            confirmButtonText: 'Ok',
//        }).then((result) => {
//            $(this).val("0.00");
//        })
//    }


//    $('#txtOpeningBalance').val(openingBalanceValue.toFixed(2));
//    $('#z0__Payments_Vendor_VendorBalance_OpeningBalance').val(openingBalanceValue.toFixed(2));
//    $('#Payments_PaymentAmountAgainstOB').val(enteredValue.toFixed(2));
//    $('#z0__Payments_PaymentAmountAgainstOB').val(enteredValue.toFixed(2));


//    if (openingBalance < 0) {
//        var res = getTotalNetPayableValue - enteredValue;
//        $('#totalpayable').val(res.toFixed(2));
//    } else {
//        var res = getTotalNetPayableValue + enteredValue;
//        $('#totalpayable').val(res.toFixed(2));
//    }


//});

function enforceNumberValidation(ele) {
    if ($(ele).data('decimal') != null) {
        // found valid rule for decimal
        var decimal = parseInt($(ele).data('decimal')) || 0;
        var val = $(ele).val();
        if (decimal > 0) {
            var splitVal = val.split('.');
            // var decimalvalue = parseInt(splitVal[0]);
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
$("#tblListOfAdvancePayments").on("click", "#advancePaymentCheckBox", function () {
    debugger;
    var balance = 0
    var actualPayableAmount = 0
    var afterdeductionAdvancePayment = 0
    var $row = $(this).closest("tr");
    var model = {
        ID: $row.find("td:eq(1)").text(),
        PaymentAmount: parseFloat($row.find("td:eq(5)").text()),
        BalanceAmount: 0
    };
    var id = $(this).closest('tr').find('td:eq(1)').text();
    var amount = $(this).closest('tr').find('td:eq(5)').text();
    var paidamount = $(this).closest('tr').find('td:eq(6)').text();
    var totalPayable = $("#totalpayable").val();

    if (totalPayable != "") {
        if (totalPayable > totalPayableAmount) {
            totalPayableAmount = totalPayable
        }
        if ($(this).is(':checked')) {
            //// totalAmount = parseFloat(totalAmount) + parseFloat(amount);
            if (amount == "0.00") {
                balance = parseFloat(paidamount) - parseFloat(totalPayable);
            }
            else {
                balance = parseFloat(amount) - parseFloat(totalPayable);
            }
            model.BalanceAmount = balance < 0 ? 0 : balance;
            selItems.push(model);

            $("#advanceAmountUsed").val(amount);
            if (amount == "0.00") {
                actualPayableAmount = parseFloat(totalPayable) - parseFloat(paidamount);
            }
            else {
                actualPayableAmount = parseFloat(totalPayable) - parseFloat(amount);
            }
            if (actualPayableAmount < 0) {
                var pay = 0;
                $("#totalpayable").val(pay.toFixed(2));
                $("#actualTotalPayable").val(pay.toFixed(2));

                //var advanceAmountforPaymentUsed = $("#advanceAmountUsedForPayment").val();
                // afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) + parseFloat(totalPayable);
                //$("#advanceAmountUsedForPayment").val(afterdeductionAdvancePayment.toFixed(2));

            } else {
                $("#totalpayable").val(actualPayableAmount.toFixed(2));
                $("#actualTotalPayable").val(actualPayableAmount.toFixed(2));
                var advanceAmountforPaymentUsed = $("#advanceAmountUsedForPayment").val();
                if (parseFloat(amount) <= 0) {
                    afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) + parseFloat(paidamount);

                }
                else {
                    afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) + parseFloat(amount);

                }
                if (afterdeductionAdvancePayment <= 0) {
                    var pay1 = 0;

                    $("#advanceAmountUsedForPayment").val(pay1.toFixed(2));

                }
                else {
                    $("#advanceAmountUsedForPayment").val(afterdeductionAdvancePayment.toFixed(2));

                }
            }
        } else {
            var index = selItems.findIndex(function (item) {
                return item.ID === model.ID;
            });
            if (index !== -1) {
                selItems.splice(index, 1);
            }
            //totalAmount = parseFloat(amount) - parseFloat(amount);
            var totaladvanceAmount = 0
            if (parseFloat(amount) <= 0) {
                actualPayableAmount = parseFloat(totalPayable) + parseFloat(paidamount);
                //var grid = document.getElementById("tblListOfAdvancePayments");
                //var checkBoxes = grid.getElementsByTagName("INPUT");
                //for (i = 0; i < checkBoxes.length; i++) {
                //    if (checkBoxes[i].checked) {
                //        var row = checkBoxes[i].parentNode.parentNode;
                //        var amount = row.cells[5].innerHTML;
                //        totaladvanceAmount = parseFloat(totaladvanceAmount) + parseFloat(amount);
                //    }
                //}

                //if (totaladvanceAmount > 0) {
                //    //var actualtotal = parseFloat(totalPayableAmount) + getvalue;
                //    actualPayableAmount = actualPayableAmount - totaladvanceAmount;
                //    //totalPayableAmount = getvalue;
                //    var balance = totaladvanceAmount - actualPayableAmount;
                //    for (i = 0; i < selItems.length; i++) {

                //        if (selItems[i].BalanceAmount > 0) {
                //            selItems[i].BalanceAmount = balance < 0 ? 0 : balance
                //        }
                //    }
                //    var selItemsJson = JSON.stringify(selItems);
                //    $("#advancePayments").val(selItemsJson);
                //}

            }
            else {
                actualPayableAmount = parseFloat(totalPayable) + parseFloat(amount);
            }
            //$("#advancePaymentId").val(selItems.join(","));

            if (actualPayableAmount < 0) {
                var pay = 0;
                $("#totalpayable").val(pay.toFixed(2))
                $("#actualTotalPayable").val(pay.toFixed(2));

            } else {
                var advanceAmountforPaymentUsed = $("#advanceAmountUsedForPayment").val();
                if (parseFloat(amount) <= 0) {
                    afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) - parseFloat(paidamount);

                }
                else {
                    afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) - parseFloat(amount);

                }
                if (afterdeductionAdvancePayment <= 0) {
                    var pay1 = 0;

                    $("#advanceAmountUsedForPayment").val(pay1.toFixed(2));

                }
                else {
                    $("#advanceAmountUsedForPayment").val(afterdeductionAdvancePayment.toFixed(2));

                }
                $("#totalpayable").val(actualPayableAmount.toFixed(2))
                $("#actualTotalPayable").val(actualPayableAmount.toFixed(2));

            }

        }
        var advanceAmountforPaymentUsedall = $("#advanceAmountUsedForPayment").val();
        $("#advanceAmountUsed").val(advanceAmountforPaymentUsedall);
        var selItemsJson = JSON.stringify(selItems);
        $("#advancePayments").val(selItemsJson);

    } else {
        Swal.fire({
            text: 'Please Enter the Net Payable Amount',
            icon: 'warning',
            confirmButtonText: 'Ok',
        });
        return false;
    }
});

var balancevalue = 0;
var txt = 0;
var txt2 = 0;
var txt3 = 0;
var tds = 0;
var pay = 0;
var bal = 0;
var gsttds = 0;
var total = "";
$("#tblAddPayments .trow").each(function () {
    var tvalue = ($(this).find("td:eq(8)").text());

    var totalp = parseFloat(tvalue);
    $(this).find("td:eq(8)").text(totalp)

    var tvalue2 = ($(this).find("td:eq(9)").text());
    var totalb = parseFloat(tvalue2);
    $(this).find("td:eq(9)").text(totalb)
});

$("#tblAddPayments .trow").each(function () {
    var tvalue = ($(this).find("td:eq(8)").text());

    var totalp = parseInt(tvalue);
    tds += parseInt(totalp);

    var tvalue2 = ($(this).find("td:eq(9)").text());
    var totalb = parseInt(tvalue2);
    gsttds += parseInt(totalb);


});


$("#payabledeuction").val((tds + gsttds).toFixed(2));

//laod remaining payable
$("#tblAddPayments .trow").each(function () {

    var tvaluep = ($(this).find("td:eq(2)").text());

    var totalpp = parseFloat(tvaluep);
    pay += parseFloat(totalpp);

    var tvalueb = ($(this).find("td:eq(0) input[type='number']").val());
    var totalbb = parseFloat(tvalueb);
    bal += parseFloat(totalbb);

});
//var openingBalance = $("#txtOpeningBalance").val();
//var openingBalancePayableAmount = parseFloat($("#Payments_PaymentAmountAgainstOB").val());
//if (isNaN(openingBalancePayableAmount)) {
//    openingBalancePayableAmount = 0;
//}
//if (openingBalance < 0) {
//    var res = bal - openingBalancePayableAmount;
//    $('#totalpayable').val(res.toFixed(2));
//} else {
//    var res = bal + openingBalancePayableAmount;
//    $('#totalpayable').val(res.toFixed(2));
//}

var advanceUsedForPayments = $("#advanceAmountUsedForPayment").val();
if (advanceUsedForPayments == "") {
    advanceUsedForPayments = 0
}
if (!isNaN(advanceUsedForPayments)) {
    var actualPayment = bal - advanceUsedForPayments;
    $("#actualTotalPayable").val(actualPayment);
    $('#totalpayable').val(actualPayment.toFixed(2));

}
else {
    $('#totalpayable').val(bal.toFixed(2));
    $('#actualTotalPayable').val(bal.toFixed(2));

}
$("#balance").val((pay - bal).toFixed(2));


//var table = document.getElementById('tblListOfAdvancePayments');
//for (var i = 0, row; row = table.rows[i]; i++) {
//    var cell = row.cells[4];
//    if (cell.innerHTML > 0) {
//        var checkbox = row.querySelector('#advancePaymentCheckBox');
//        checkbox.checked = true;
//        var model = {
//            ID: row.cells[1].innerHTML,
//            PaymentAmount: row.cells[5].innerHTML,
//            BalanceAmount: 0
//        };
//        selItems.push(model);
//    }
//}
var selItemsJson = JSON.stringify(selItems);
$("#advancePayments").val(selItemsJson);

$('td').on('input', '.text1', function () {

    var $row = jQuery(this).closest('tr');

    var $columnA = $row.find('td:eq(2)').text();
    var colA = parseFloat($columnA);

    var $columnB = $row.find('td:eq(1)').text();

    var colB = parseFloat($columnB);



    var $columnC = $row.find("td:eq(0) input[type='number']").val();
    var colC = parseFloat($columnC);


    if (colC > colA) {
        swal.fire({
            title: 'Net payable cannot be greater than the payable amount',
            icon: 'warning',
            confirmButtonText: 'Ok',
        }).then((result) => {
            $(this).val("0.00");
            $("#balance").val("");
            $("#totalpayable").val("");
            $("#actualTotalPayable").val("");
        })
    }
});



//$('#Payments_PaymentAmountAgainstOB').on('change', function () {
//    var getTotalNetPayableValue = 0;
//    var enteredValue = parseFloat($(this).val());
//    if (isNaN(enteredValue)) {
//        enteredValue = 0;
//    }
//    var openingBalanceHidden = parseFloat($('#Payments_Vendor_VendorBalance_OpeningBalance').val());
//    var openingBalancePayableHidden = parseFloat($('#HiddenPaymentAmountAgainstOB').val());

//    var openingBalanceValue = 0;
//    var openingBalance = parseFloat($('#txtOpeningBalance').val());
//    if (isNaN(openingBalance)) {
//        openingBalance = 0;
//    }
//    debugger
//    if (openingBalancePayableHidden < enteredValue){
//        openingBalanceValue = openingBalanceHidden - (enteredValue - openingBalancePayableHidden)
//    }else{
//        openingBalanceValue = openingBalanceHidden + (openingBalancePayableHidden - enteredValue);
//    }

//    $("#tblAddPayments .text1").each(function () {
//        var totalamt = $(this).val();  //10
//        if ($.isNumeric(totalamt)) {
//            getTotalNetPayableValue += parseFloat(totalamt);  //10
//        }
//    });

//    if (openingBalance < 0) {
//        if (getTotalNetPayableValue < enteredValue) {
//            enteredValue = 0;
//            swal.fire({
//                title: 'Payable amount cannot be greater than the Total Net Payable',
//                icon: 'warning',
//                confirmButtonText: 'Ok',
//            }).then((result) => {
//                $(this).val("0.00");
//            })
//        }
//    }
//    if (openingBalance < enteredValue) {
//        enteredValue = 0;
//        openingBalanceValue = openingBalanceHidden + openingBalancePayableHidden;
//        swal.fire({
//            title: 'Payable amount cannot be greater than the opening balance amount',
//            icon: 'warning',
//            confirmButtonText: 'Ok',
//        }).then((result) => {
//            $(this).val("0.00");
//        })
//    }


//    $('#txtOpeningBalance').val(openingBalanceValue.toFixed(2));
//    $('#z0__Payments_Vendor_VendorBalance_OpeningBalance').val(openingBalanceValue.toFixed(2));
//    $('#Payments_PaymentAmountAgainstOB').val(enteredValue.toFixed(2));
//    $('#z0__Payments_PaymentAmountAgainstOB').val(enteredValue.toFixed(2));


//    if (openingBalance < 0) {
//        var res = getTotalNetPayableValue - enteredValue;
//        $('#totalpayable').val(res.toFixed(2));
//    } else {
//        var res = getTotalNetPayableValue + enteredValue;
//        $('#totalpayable').val(res.toFixed(2));
//    }


//});

function enforceNumberValidation(ele) {
    if ($(ele).data('decimal') != null) {
        // found valid rule for decimal
        var decimal = parseInt($(ele).data('decimal')) || 0;
        var val = $(ele).val();
        if (decimal > 0) {
            var splitVal = val.split('.');
            // var decimalvalue = parseInt(splitVal[0]);
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
$("#tblListOfAdvancePayments").on("click", "#advancePaymentCheckBox", function () {
    debugger;
    var balance = 0
    var actualPayableAmount = 0
    var afterdeductionAdvancePayment = 0
    var $row = $(this).closest("tr");
    var model = {
        ID: $row.find("td:eq(1)").text(),
        PaymentAmount: parseFloat($row.find("td:eq(5)").text()),
        BalanceAmount: 0
    };
    var id = $(this).closest('tr').find('td:eq(1)').text();
    var amount = $(this).closest('tr').find('td:eq(5)').text();
    var paidamount = $(this).closest('tr').find('td:eq(6)').text();
    var totalPayable = $("#totalpayable").val();

    if (totalPayable != "") {
        if (totalPayable > totalPayableAmount) {
            totalPayableAmount = totalPayable
        }
        if ($(this).is(':checked')) {
            //// totalAmount = parseFloat(totalAmount) + parseFloat(amount);
            if (amount == "0.00") {
                balance = parseFloat(paidamount) - parseFloat(totalPayable);
            }
            else {
                balance = parseFloat(amount) - parseFloat(totalPayable);
            }
            model.BalanceAmount = balance < 0 ? 0 : balance;
            selItems.push(model);

            $("#advanceAmountUsed").val(amount);
            if (amount == "0.00") {
                actualPayableAmount = parseFloat(totalPayable) - parseFloat(paidamount);
            }
            else {
                actualPayableAmount = parseFloat(totalPayable) - parseFloat(amount);
            }
            if (actualPayableAmount < 0) {
                var pay = 0;
                $("#totalpayable").val(pay.toFixed(2));
                $("#actualTotalPayable").val(pay.toFixed(2));

                //var advanceAmountforPaymentUsed = $("#advanceAmountUsedForPayment").val();
                // afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) + parseFloat(totalPayable);
                //$("#advanceAmountUsedForPayment").val(afterdeductionAdvancePayment.toFixed(2));

            } else {
                $("#totalpayable").val(actualPayableAmount.toFixed(2));
                $("#actualTotalPayable").val(actualPayableAmount.toFixed(2));
                var advanceAmountforPaymentUsed = $("#advanceAmountUsedForPayment").val();
                if (parseFloat(amount) <= 0) {
                    afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) + parseFloat(paidamount);

                }
                else {
                    afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) + parseFloat(amount);

                }
                if (afterdeductionAdvancePayment <= 0) {
                    var pay1 = 0;

                    $("#advanceAmountUsedForPayment").val(pay1.toFixed(2));

                }
                else {
                    $("#advanceAmountUsedForPayment").val(afterdeductionAdvancePayment.toFixed(2));

                }
            }
        } else {
            var index = selItems.findIndex(function (item) {
                return item.ID === model.ID;
            });
            if (index !== -1) {
                selItems.splice(index, 1);
            }
            //totalAmount = parseFloat(amount) - parseFloat(amount);
            var totaladvanceAmount = 0
            if (parseFloat(amount) <= 0) {
                actualPayableAmount = parseFloat(totalPayable) + parseFloat(paidamount);
                //var grid = document.getElementById("tblListOfAdvancePayments");
                //var checkBoxes = grid.getElementsByTagName("INPUT");
                //for (i = 0; i < checkBoxes.length; i++) {
                //    if (checkBoxes[i].checked) {
                //        var row = checkBoxes[i].parentNode.parentNode;
                //        var amount = row.cells[5].innerHTML;
                //        totaladvanceAmount = parseFloat(totaladvanceAmount) + parseFloat(amount);
                //    }
                //}

                //if (totaladvanceAmount > 0) {
                //    //var actualtotal = parseFloat(totalPayableAmount) + getvalue;
                //    actualPayableAmount = actualPayableAmount - totaladvanceAmount;
                //    //totalPayableAmount = getvalue;
                //    var balance = totaladvanceAmount - actualPayableAmount;
                //    for (i = 0; i < selItems.length; i++) {

                //        if (selItems[i].BalanceAmount > 0) {
                //            selItems[i].BalanceAmount = balance < 0 ? 0 : balance
                //        }
                //    }
                //    var selItemsJson = JSON.stringify(selItems);
                //    $("#advancePayments").val(selItemsJson);
                //}

            }
            else {
                actualPayableAmount = parseFloat(totalPayable) + parseFloat(amount);
            }
            //$("#advancePaymentId").val(selItems.join(","));

            if (actualPayableAmount < 0) {
                var pay = 0;
                $("#totalpayable").val(pay.toFixed(2))
                $("#actualTotalPayable").val(pay.toFixed(2));

            } else {
                var advanceAmountforPaymentUsed = $("#advanceAmountUsedForPayment").val();
                if (parseFloat(amount) <= 0) {
                    afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) - parseFloat(paidamount);

                }
                else {
                    afterdeductionAdvancePayment = parseFloat(advanceAmountforPaymentUsed) - parseFloat(amount);

                }
                if (afterdeductionAdvancePayment <= 0) {
                    var pay1 = 0;

                    $("#advanceAmountUsedForPayment").val(pay1.toFixed(2));

                }
                else {
                    $("#advanceAmountUsedForPayment").val(afterdeductionAdvancePayment.toFixed(2));

                }
                $("#totalpayable").val(actualPayableAmount.toFixed(2))
                $("#actualTotalPayable").val(actualPayableAmount.toFixed(2));

            }

        }
        var advanceAmountforPaymentUsedall = $("#advanceAmountUsedForPayment").val();
        $("#advanceAmountUsed").val(advanceAmountforPaymentUsedall);
        var selItemsJson = JSON.stringify(selItems);
        $("#advancePayments").val(selItemsJson);

    } else {
        Swal.fire({
            text: 'Please Enter the Net Payable Amount',
            icon: 'warning',
            confirmButtonText: 'Ok',
        });
        return false;
    }
});
const approved = document.getElementById("status")
     function getStatus(status) {
            approved.value = status
            }

