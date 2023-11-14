var progressBarValidation = false;
$("#previousbtnEditSaveReceiptsTab").click(function (event) {
    debugger;
    $("#SaveReceipts").attr('class', 'tab-pane fade active show');
    $("#EditSaveReceipts").attr('class', 'tab-pane fade');

    $("#SaveReceipts-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#EditSaveReceipts-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});


function showInPopupLA(url, title, module) {
    debugger

    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            debugger
            switch (module) {
                case 'GenerateReceipt':
                    $('#modelGenerateReceiptDetails .modal-body').html(res);
                    $('#modelGenerateReceiptDetails .modal-title').html(title);
                    $('#modelGenerateReceiptDetails').modal('show');
                    // to make popup draggable
                    $('.modal-dialog').draggable({
                        handle: ".modal-header"
                    });
                    break
                case 'SavedReceipt':
                    $('#modelEditSaveReceiptDetails .modal-body').html(res);
                    $('#modelEditSaveReceiptDetails .modal-title').html(title);
                    $('#modelEditSaveReceiptDetails').modal('show');

                    // to make popup draggable
                    $('.modal-dialog').draggable({
                        handle: ".modal-header"
                    });
                    break
                case 'ReceiptPayment':
                    $('#modelReceiptPaymentDetails .modal-body').html(res);
                    $('#modelReceiptPaymentDetails .modal-title').html(title);
                    $('#modelReceiptPaymentDetails').modal('show');
                    // to make popup draggable
                    $('.modal-dialog').draggable({
                        handle: ".modal-header"
                    });
                    break
                case 'LoanReceipt':
                    $('modelLoanReceiptDetails .modal-body').html(res);
                    $('modelLoanReceiptDetails .modal-title').html(title);
                    $('modelLoanReceiptDetails').modal('show');
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
            console.log(err);
        }
    });
}

function ClosePopupFormLA() {
    $('#modelGenerateReceiptDetails .modal-body').html('');
    $('#modelGenerateReceiptDetails .modal-title').html('');
    $('#modelGenerateReceiptDetails').modal('hide');

    $('#modelEditSaveReceiptDetails .modal-body').html('');
    $('#modelEditSaveReceiptDetails .modal-title').html('');
    $('#modelEditSaveReceiptDetails').modal('hide');

    $('#modelReceiptPaymentDetails .modal-body').html('');
    $('#modelReceiptPaymentDetails .modal-title').html('');
    $('#modelReceiptPaymentDetails').modal('hide');
}

$("#btnTabGenerateReceipts").click(function (event) {
    debugger;
    $("#GenerateReceiptsProgressBar").attr('class', 'progress-bar bg-success');
    $("#SaveReceipts").attr('class', 'tab-pane fade active show');
    $("#GenerateReceipts").attr('class', 'tab-pane fade');

    $("#SaveReceipts-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#GenerateReceipts-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

function showInPopupPayment(title, module, LoanSub, UnitName) {
    debugger;
    var grid = document.getElementById("tblReceiptPayment1");
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
                url: '/Admin/SavedReceipt/CreatePayment',
                traditional: true,
                data: {
                    ReferenceNumber: ReferenceNumber,
                    loansub: LoanSub,
                    unitname: UnitName
                },
                success: function (res) {
                    switch (module) {
                        case 'SavedReceipt':
                            $('#modelEditSaveReceiptDetails .modal-body').html(res);
                            $('#modelEditSaveReceiptDetails .modal-title').html(title);
                            $('#modelEditSaveReceiptDetails').modal('show');
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

$("#previousbtnSaveReceiptsTab").click(function (event) {

    $("#GenerateReceipts").attr('class', 'tab-pane fade active show');
    $("#SaveReceipts").attr('class', 'tab-pane fade');

    $("#GenerateReceipts-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
    $("#SaveReceipts-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

    $("#spanErrorMsg").html("");
    $("#divEnquiryAlertPopup").html("");
    window.scrollTo(0, 0);

});

$("#savebtnReceiptPaymentTab").click(function (event) {
    event.preventDefault();
    debugger
    var formLDSD = document.getElementById("SaveReceipts");
    $.ajax({
        async: false,
        type: "POST",
        /*url: GetRoute('/LoanRelatedReceipt/SaveReceiptPaymentDetails'),*/
        success: function (data) {
            if (data.isValid) {
                $("#SaveReceiptsProgressBar").attr('class', 'progress-bar bg-success');

                $("#GenerateReceipts").attr('class', 'tab-pane fade active show');
                $("#SaveReceipts").attr('class', 'tab-pane fade');

                $("#GenerateReceipts-tab").attr('class', 'nav-link active').attr('aria-selected', 'true');
                $("#SaveReceipts-tab").attr('class', 'nav-link').attr('aria-selected', 'true');

                $("#spanErrorMsg").html("");
                $("#divEnquiryAlertPopup").html("");
                window.scrollTo(0, 0);
            }
            else {
                $("#SaveReceiptsProgressBar").attr('class', 'progress-bar bg-danger');
                $("#spanErrorMsg").html(data.message);
                $("#divEnquiryAlertPopup").html(data.message);
                $("#modalAlertEnq").modal('show');
            }
        },
        error: function (err) {
            $("#SaveReceiptsProgressBar").attr('class', 'progress-bar bg-danger');
            console.log(err);
        }
    });
});