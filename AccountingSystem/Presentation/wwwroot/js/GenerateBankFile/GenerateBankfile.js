
var pref = "filter";

var pid = [];
//var $grid = $('#@GridChck');

function exportToExcel() {
    //var url = '@Url.Action("ExportToGenBankBileList", contr)';
    var url = `/${contr}/ExportToGenBankBileList`;
    var $form = $('<form method="post"/>').attr('action', url).appendTo('body');
    var grid = $('#' + gridId);
    //var grid = $('#@(GridChck)');

    // request parameters (sorting, grouping, any parent parameters)
    var req = grid.data('api').getRequest();

    // send visible columns info, binds to GridExpParams
    var viscols = utils.getVisCols(grid);
    awef.loop(viscols,
        function (v) {
            $form.append("<input type='hidden' name='visNames' value='" + v + "'/>");
        });

    awef.loop(req,
        function (val) {
            $form.append("<input type='hidden' name='" + val.name + "' value='" + val.value + "'/>");
        });

    //$form.append($('#allPages').clone());
    $form.submit();
    $form.remove();
}
// outside filter row custom mod
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
    var pref = 'pref';

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

                setDefultVlaues();
            }
        });
    });


    g.on('awerender', function (e) {
        var gChk = gridPersistentCheckboxes("GridChkPersistent", "Id");
    });


}
function gridPersistentCheckboxes(gid, chkName) {
    var Id = $(this).data('val');
    var chkSel = '[name=' + chkName + ']';
    var chkAllSel = '[name=ChkAll]';
    var vals = {};
    var chkAllSelected = false;
    var selItems = [];
    var grid = $('#' + gridId);
   // var grid = $('#@GridChck');
    var rowsel = '.awe-row:not(.o-frow)';

    grid.on('awerender', function () {
        var allChecked = true;
        grid.find(rowsel).each(function () {
            var row = $(this);
            if (vals[row.data('k')]) {
                var apiData = row.find(chkSel).val(true).data('api');
                if (apiData != null) {
                    apiData.render();
                }
            } else {
                allChecked = false;
            }
        });
        var apiData = grid.find(chkAllSel).val(allChecked).data('api');
        if (apiData != null) {
            apiData.render();
        }
    });


    grid.on('change', chkAllSel, function () {
        if (!chkAllSelected && $(this).val() === 'false') {
            $(this).val('true').change();
            chkAllSelected = true;
            return;
        }
        var val = $(this).val();
        var isChecked = !chkAllSelected && val === 'true';

        grid.find(rowsel).each(function (index, element) {

            var $row = $(this);
            if ($row.find(chkSel).val(val).data('api') != undefined)
                $row.find(chkSel).val(val).data('api').render();
            var key = $row.data('k');
            var rowData = grid.data('api').model($row);

            if (isChecked) {
                vals[key] = 1;
                if (rowData != null) {
                    if (selItems.indexOf(s => s.id === rowData.id) === -1) {
                        var existing = selItems.find(x => x.id == rowData.id)
                        if (existing == null) {
                            selItems.push(rowData);
                        }
                    }
                }
            }
            else {
                var isChkAllChecked = $("input#ChkAll").val() == 'true';
                if (isChkAllChecked) {
                    $('input#ChkAll').next().find('.o-chk').removeClass('o-chked');
                    chkAllSelected = false;
                    $("input#ChkAll").val("false")

                }
                var index = selItems.findIndex(s => s.id === rowData.id);
                var test = selItems.splice(index, 1);
            }
        });
        chkAllSelected = isChecked;
        processSelections();
    });

    // check single row
    grid.on('change', chkSel, function () {
        var chk = $(this);
        var isChecked = chk.val() === 'true';
        var $row = chk.closest(rowsel);
        var key = chk.closest(rowsel).data('k');
        var rowData = grid.data('api').model($row);

        if (isChecked) {
            vals[key] = 1;
            //var index = $.map($('#GSTTDSListGrid [name=Id]'), c => c.dataset.val).indexOf(key);
            //var rowData = grid.data('api').model(grid.find('.awe-row:nth(' + index + ')'));
            if (rowData != null) {
                if (selItems.indexOf(s => s.id === key) === -1) {
                    var existing = selItems.find(x => x.id == rowData.id)
                    if (existing == null) {
                        selItems.push(rowData);
                    }
                }
            }
        }
        else {
            var isChkAllChecked = $("input#ChkAll").val() == 'true';
            if (isChkAllChecked) {
                $('input#ChkAll').next().find('.o-chk').removeClass('o-chked');
                chkAllSelected = false;
            }
            var index = selItems.findIndex(s => s.id === rowData.id);
            var test = selItems.splice(index, 1);
        }

        processSelections();
    });

    function processSelections() {

        if (Object.keys(vals).length == 0) {
            $('#noOfTransaction').val('');
            $('#noOfVendor').val('');
        }
        else {
            var sum = 0;
            console.log(selItems);
            $.each(selItems, function (i, item) { sum += parseFloat(item.paymentAmount); });
            $('#noOfTransaction').val(sum.toFixed(2));
            //  $('#noOfTransaction').val(Object.keys(vals).length);

            $('#noOfVendor').val(Array.from(new Set($.map(selItems, c => c.vendorID))).length);
        }
    }
    return {
        getAll: function () {
            var keys = [];
            for (var k in vals) {
                keys.push(k);
            }
            return keys;
        },
        clear: function () {
            vals = {};
            grid.data('api').load();
        }
    };
}
// get data for filter editor from grid model
function filterData(name) {
    return function () {
       
        var g = $('#' + GridChck);
        var o = g.data('o');
        return awem.frowData(o, name);
    }
}
//get default bindings
function setDefultVlaues() {
    $.ajax({
        type: 'Get',
        url: '/GenerateBankFile/GetDefaultReocrds',
        success: function (res) {
            if (res != null) {
                console.log(res);
                $("#totalApprovedAmount").val(res.totalAmount);
                $("#totalrecords").val(res.modelCount);
            }
        },
        error: function (err) {
            console.log(err)
            bootbox.alert("An Error Occured While fecthcing  the Details!");
        }
    });
}
function setParams() {

    Id = $('#GenBankFileListGrid [name=Id]').filter(function () {

        return $(this).val() == 'true';
    }).map(function () {
        return $(this).data('val');

    }).get();
    console.log(Id);
    if (Id.length == 0) {
        Swal.fire({
            text: 'Please select at least one bank file!!',
            icon: 'warning',
            confirmButtonText: 'Ok',
        })
        $('.awe-popup').each(function () {
            if ($(this).data('api'))
                $(this).data('api').destroy();
        });
        return false;
    }
    else {

        return { Id: Id };
    }
}


    //on page ready hide all branch option
    $("#selBranchName").find('option').hide();
    // set task as empty
    $("#selBranchName").val('');
    $("#selIfsc").val('');
    $("#selAccountNo").val('');

    // onchange of BankName Drop down
    $("#selBankName").on('change', function () {
        var selectedBank = $("#selBankName").val();
        if (selectedBank != '') {
            $("#selBranchName").find('option').hide();
            $("#selBranchName option[value='']").show();
            $('*[data="' + selectedBank + '"]').show();
            $("#selIfsc").val('');
            $("#selAccountNo").val('');
            $("#selBankMasterId").val('');

        }
        else {
            // if BankName not selected then hide all tasks
            $("#selBranchName").find('option').hide();
            $("#selBranchName").val('');

        }

    });

    // onchange of BranchName Drop down
    $("#selBranchName").on('change', function () {
        var selectedBranchDetails = $("#selBranchName").val().split(',');
        var selectedBranch = selectedBranchDetails[0];
        var BankMasterId = selectedBranchDetails[1];
        var selectedBranchAcnt = selectedBranchDetails[2];
        var selwctedBankPaymentLevel = selectedBranchDetails[3]
        //var selectedPaymentValue=
        if (selectedBranch != '') {
            $("#selIfsc").val(selectedBranch);
            $("#selAccountNo").val(selectedBranchAcnt);
            $("#selBankMasterId").val(BankMasterId);
            var id = $("#selBankMasterId").val(BankMasterId);
            $("#branchid").val(BankMasterId);
            //$("#selectedbankPaymentLevel").val(seelctedBankPaymentLevel);
        }
        else {
            // if BranchName not selected then hide all tasks
            $("#selIfsc").val('');
            $("#selAccountNo").val('');
            $("#selBankMasterId").val('');

        }

    });

