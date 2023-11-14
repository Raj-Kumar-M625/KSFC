
var selItems = [];
var totalAmount = 0;
var totalPayableAmount = 0;
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


        if ($.isNumeric(totalamt)) {

            getvalue += parseFloat(totalamt);  //10

        }

    });
    var openingBalance = parseFloat($('#OpeningBalancePayableAmount').val());
    if (isNaN(openingBalance)) {
        openingBalance = 0;
    }

    var totaladvanceAmount = 0;
    var selectedISval = [];
    if (parseInt($("#advancePayments")) > 0) {

    var grid = document.getElementById("tblListOfAdvancePayments");
    var checkBoxes = grid.getElementsByTagName("INPUT");
    for (i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].checked) {
            var row = checkBoxes[i].parentNode.parentNode;
            var amount = row.cells[4].innerHTML;
            selectedISval.push(row.cells[1].innerHTML)
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

        var afterAdvanceDeduction = getvalue - totaladvanceAmount;
        if (afterAdvanceDeduction < 0) {
            var pay = 0
            $("#totalpayable").val((pay + openingBalance).toFixed(2)); //10

        }
        else {
            $("#totalpayable").val((afterAdvanceDeduction + openingBalance).toFixed(2)); //10

        }
    }
    else {
        $("#totalpayable").val((getvalue + openingBalance).toFixed(2)); //10

    }
}
        else {
    $("#totalpayable").val((getvalue + openingBalance).toFixed(2)); //10

}

if (totalb == 0) {
    if (getvalue == totalp) {
        $("#balance").val(totalp.toFixed(2))
    }
    else {
        totalremaing = 0;
        $("#balance").val(totalremaing.toFixed(2));
    }

}
else {
    if (getvalue == totalb) {
        totalremaing = 0;
        $("#balance").val(totalremaing.toFixed(2));
    }
    else {
        $("#balance").val((totalb - getvalue).toFixed(2));
    }
}
    });






//Load Remaining Payable Column

$(document).ready(function () {
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

        var tvalueb = ($(this).find("td:eq(1)").text());
        var totalbb = parseFloat(tvalueb);
        bal += parseFloat(totalbb);



    });
    if (bal != 0) {

        $("#balance").val(bal.toFixed(2))
    }
    else {
        $("#balance").val(pay.toFixed(2));
    }

});

$('td').on('input', '.text1', function () {
    $(".advancePaymentCheckBox").prop('checked',false);
    var $row = jQuery(this).closest('tr');
    var $columnA = $row.find('td:eq(2)').text();
    var colA = parseFloat($columnA);
    var $columnB = $row.find('td:eq(1)').text();
    var colB = parseFloat($columnB);
    var $columnC = $row.find("td:eq(0) input[type='number']").val();
    var colC = parseFloat($columnC);
    if (colB == 0) {
        if (colC > colA) {
            swal.fire({
                title: 'Net payable cannot be greater than the payable amount',
                icon: 'warning',
                confirmButtonText: 'Ok',
            }).then((result) => {
                $(this).val("0.00");
                $("#balance").val("");
                $("#totalpayable").val("");
            })
        }
    }
    else {
        if (colC > colB) {
            swal.fire({
                title: 'Net payable cannot be greater than the balance amount',
                icon: 'warning',
                confirmButtonText: 'Ok',
            }).then((result) => {
                $(this).val("0.00");
                $("#balance").val("");
                $("#totalpayable").val("");
            })
        }
    }
});

//$('#OpeningBalancePayableAmount').on('change', function () {
//    var getTotalNetPayableValue = 0;
//    var enteredValue = parseFloat($(this).val());
//    if (isNaN(enteredValue)) {
//        enteredValue = 0;
//    }

//    var openingBalance = parseFloat($('#txtOpeningBalance').val());
//    if (isNaN(openingBalance)) {
//        openingBalance = 0;
//    }

//    $("#tblAddPayments .text1").each(function () {
//        var totalamt = $(this).val();  //10
//        if ($.isNumeric(totalamt)) {
//            getTotalNetPayableValue += parseFloat(totalamt);  //10
//        }
//    });

//    if (openingBalance < 0) {
//    if (getTotalNetPayableValue < enteredValue){
//        enteredValue = 0;
//        swal.fire({
//            title: 'Payable amount cannot be greater than the Total Net Payable',
//            icon: 'warning',
//            confirmButtonText: 'Ok',
//        }).then((result) => {
//            $(this).val("0.00");
//        })
//    }
//    }
//    if (openingBalance < enteredValue){
//        enteredValue = 0;
//        swal.fire({
//            title: 'Payable amount cannot be greater than the opening balance amount',
//            icon: 'warning',
//            confirmButtonText: 'Ok',
//        }).then((result) => {
//            $(this).val("0.00");
//        })
//    }
//    $('#OpeningBalancePayableAmount').val(enteredValue.toFixed(2));
//    $('#z0__OpeningBalancePayableAmount').val(enteredValue.toFixed(2));


//    if (openingBalance < 0){
//        var res = getTotalNetPayableValue - enteredValue;
//        $('#totalpayable').val(res.toFixed(2));
//    }else{
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

    var $row = $(this).closest("tr");
    var model = {
        ID: $row.find("td:eq(1)").text(),
        PaymentAmount: parseFloat($row.find("td:eq(4)").text()),
        BalanceAmount: 0
    };
    var id = $(this).closest('tr').find('td:eq(1)').text();
    var amount = $(this).closest('tr').find('td:eq(4)').text();
    var totalPayable = $("#totalpayable").val();
    debugger
    if (totalPayable != "") {
        if (totalPayable > totalPayableAmount) {
            totalPayableAmount = totalPayable
        }
        if ($(this).is(':checked')) {
            totalAmount = parseFloat(totalAmount) + parseFloat(amount);
            var balance = parseFloat(amount) - parseFloat(totalPayable);
            model.BalanceAmount = balance < 0 ? 0 : balance;
            selItems.push(model);


            var actualPayableAmount = parseFloat(totalPayable) - parseFloat(amount);
            if (actualPayableAmount < 0) {
                var pay = 0;
                $("#totalpayable").val(pay.toFixed(2));
            } else {
                $("#totalpayable").val(actualPayableAmount.toFixed(2));
            }
            $("#advanceAmountUsed").val(totalAmount);

        } else {

            var index = selItems.findIndex(function (item) {
                return item.ID === model.ID;
            });
            var item = selItems.find(function (item) {
                return item.ID === model.ID;
            });

            var balanceAmount = item ? item.BalanceAmount : 0;
            if (index !== -1 && !$(this).is(':checked')) {
                selItems.splice(index, 1);
            }


            totalAmount = parseFloat(totalAmount) - parseFloat(amount);
            var actualPayableAmount = parseFloat(amount) - parseFloat(balanceAmount) + parseFloat(totalPayable);
            $("#advanceAmountUsed").val(totalAmount);
            //$("#advancePaymentId").val(selItems.join(","));
            if (actualPayableAmount < 0) {
                var pay = 0;
                $("#totalpayable").val(pay.toFixed(2))
            } else {
                $("#totalpayable").val(actualPayableAmount.toFixed(2))
            }
        }
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
