var totalAmount = 0;
//var uri = 'Index/Reconcile';
var uri = `/Reconcile/Index`;

$(document).on("click", "#btnUnmatch", function () {
    var grid = document.getElementById("tbllistOfTransactions");
    var checkBoxes = grid.getElementsByTagName("INPUT");
    var IsSelected = document.getElementById('#select');
    const message = [];
    //Loop through the CheckBoxes.
    for (i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].checked) {
            var row = checkBoxes[i].parentNode.parentNode;
            var RectNumber = row.cells[1].innerHTML;
            message.push(RectNumber);
        }
    }
    if (message.length == 0) {
        Swal.fire({
            text: 'Please Select Atleast One Transaction to Unmatch',
            icon: 'warning',
            confirmButtonText: 'Ok',
        });

    } else {
        $.ajax({
            type: 'post',
            url: '/Reconcile/UnMatch',
            data: {
                transactionsid: message, bankTransactionId: $("#bankTranactionId").val()
            },
            success: function (data) {
                Swal.fire({
                    text: 'UnMatched the Transactions Successfully.',
                    icon: 'success',
                    confirmButtonText: 'Ok',
                }).then((result) => {
                    window.location.href = uri;
                });

            },
            error: function (result) {
                Swal.fire({
                    text: 'An Error Occured While UnMatching',
                    icon: 'error',
                    confirmButtonText: 'Ok',
                });
                console.log(result)
            }
        });
    }

})
$(document).on("click", "#btnReconcile", function () {
    var grid = document.getElementById("tbllistOfTransactions");
    var checkBoxes = grid.getElementsByTagName("INPUT");
    var IsSelected = document.getElementById('#select');
    const message = [];
    var bankAmount = $("#debit").val() != 0 ? $("#debit").val() : $("#credit").val() != 0 ? $("#credit").val() : 0;
    //Loop through the CheckBoxes.
    for (i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].checked) {
            var row = checkBoxes[i].parentNode.parentNode;
            var RectNumber = row.cells[1].innerHTML;
            var amount = row.cells[6].innerHTML;
            totalAmount = parseFloat(totalAmount) + parseFloat(amount);
            message.push(RectNumber);
        }
    }
    if (message.length == 0) {
        Swal.fire({
            text: 'Please Select Atleast One Transaction to Reconcile',
            icon: 'warning',
            confirmButtonText: 'Ok',
        });

    } else if (bankAmount < totalAmount) {
        Swal.fire({
            text: 'The Reconciling Amount Cannot be greater than Credit or Debit amount',
            icon: 'warning',
            confirmButtonText: 'Ok',
        });
        totalAmount = 0
    }
    else {
        Swal.fire({
            icon: 'warning',
            title: 'Are you sure to Reconcile ?',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'No',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    type: 'Post',
                    url: '/Reconcile/Reconcile',
                    data: {
                        transactionsid: message, bankTransactionId: $("#bankTranactionId").val()
                    },
                    success: function (data) {
                        Swal.fire({
                            text: 'Reconciled the  Transaction Successfully',
                            icon: 'success',
                            confirmButtonText: 'Ok',
                        }).then((result) => {
                            window.location.href = uri;
                        });

                    },
                    error: function (result) {
                        Swal.fire({
                            text: 'An Error Occured While Reconciling',
                            icon: 'error',
                            confirmButtonText: 'Ok',
                        });
                        console.log(result)
                    }
                });
            }
        });

    }
});


function ClosePopupFormsh() {

    $('#modelReconcileList .modal-body').html('');
    $('#modelReconcileList .modal-title').html('');
    $('#modelReconcileList').modal('hide');
}


function getReconcileUrl(id) {
    //return "@Url.Action("Reconcile", "Reconcile")?bankTransactionId=" + id;
    $.ajax({
        type: 'GET',
        url: '/Reconcile/Reconcile',
        data: { bankTransactionId: id },
        success: function (res) {
            var title = "Bank Transaction Deatils";
            $('#modelReconcileList .modal-body').html(res);
            $('#modelReconcileList .modal-title').html(title);
            $('#modelReconcileList').modal('show');
        },
        error: function (err) {
            console.log(err);
        }
    });
}

var grid = '@gridId';

function showbuttonByStatus(grid, prop) {
    if (grid.status == "Matched") {
        return "<a href='#' class='btn btn-success' onclick='getReconcileUrl(" + grid.id + ")'>Reconcile</a>"
    } else if (grid.status == "UnMatched" || grid.status == "Pending") {
        return "<a href='#' class='btn btn-warning' style='color:white' onclick='window.location.href=\"" + getManuallyReconcileUrl(grid.id) + "\"'>Manually Reconcile</a>"
    }
}


function getManuallyReconcileUrl(id) {
    return "/Reconcile/ManuallyReconcile" + '?bankTransactionId=' + id;
}
function outsideFilter(o) {
    var g = o.v;
    var fcont = $('#outFilter');
    var opt = { model: {} };
    o.fltopt = opt;

    // reload each filter control when grid loads
    g.on('aweload', function () {
        $('#outFilter .awe-val').each(function () {
            var api = $(this).data('api');
            api && api.load && api.load();
        });
    });

    // apply filters on control change
    fcont.on('change', function (e) {
        opt.inp = fcont.find('input').not('.nonflt input');
        // instead of opt.inp we could set opt.cont = fcont; but this will also include the itemsType input
        // and the grid would reload when we change the items type also

        awem.loadgflt(o);
    });

    $('#btnClearFilter').on('click', function () {
        fcont.find('.awe-val').not('.nonflt input').each(function () {
            var it = $(this).val('');
            var api = it.data('api');
            api && api.render && api.render();
            // call api.render instead of change to load the grid only once
        });

        opt.inp = fcont.find('input').not('.nonflt input');

        awem.loadgflt(o);
    });

    // keep same filter editors values after page refresh

    var fkey = 'persFout' + o.id;
    var storage = sessionStorage;
    var pref = '@pref';

    g.on('awefinit', function () {
        var fopt = storage[fkey];
        if (fopt) {
            fopt = JSON.parse(fopt, function (key, val) {
                if (val && val.length > 0 && val[0] === '[') {
                    return JSON.parse(val);
                }
                return val;
            });
            if (fopt.model) {
                o.fltopt.model = fopt.model;
                o.fltopt.order = fopt.order;

                // set persisted model filter params
                var res = awef.serlObj(fopt.model);
                res = res.concat(awef.serlArr(fopt.order, 'forder'));
                o.fparams = res;
                var model = fopt.model;

                g.one('aweload', function () {
                    for (var prop in model) {
                        var editor = $('#' + pref + prop);
                        if (editor.length) {
                            editor.val(awef.sval(model[prop]));
                            if (editor.closest('.awe-txt-field')) {
                                editor.data('api').render();
                            }
                        }
                    }
                });
            }
        }

        g.on('aweload', function (e) {
            if ($(e.target).is(g)) {
                fopt = o.fltopt;
                storage[fkey] = JSON.stringify({ model: fopt.model, order: fopt.order });
            }
        });
    });

}