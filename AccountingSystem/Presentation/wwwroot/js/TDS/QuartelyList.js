var pref = "filter";
// outside filter row custom mod
function downloadQuarterlyCSV(id) {
    $.ajax({
        type: "POST",
        async: true,
        contentType: 'application/json; charset=utf-8',
        url: "/TDS/UpdateQuarterlyTDSChallanStatus?id=" + id,
        success: function (res) {
            var grid = $('#' + gridId);
           // var grid = $('#@(gridId)');
            grid.data('api').load();
            if (res == true) {
                location.href = "/TDS/DownloadCSV/" + id;
            }
        }
    });
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
            }
        });

        g.on('awerender', function (e) {
            var gChk = gridPersistentCheckboxes("GridChkPersistent", "Id");
            var tdsQuarterlyListFilters = {};
            tdsQuarterlyListFilters.payableMinAmount = $('#filtertdsQuarterlyListFilters_payableMinAmount').val();
            tdsQuarterlyListFilters.payableMaxAmount = $('#filtertdsQuarterlyListFilters_payableMaxAmount').val();
            tdsQuarterlyListFilters.noOfChallan = $('#filtertdsQuarterlyListFilters_noOfChallan').val();
            tdsQuarterlyListFilters.quarter = $('#filtertdsQuarterlyListFilters_quarter').val();
            tdsQuarterlyListFilters.assessmentYear = $('#filtertdsQuarterlyListFilters_assessmentYear').val();
            $.ajax({
                type: "POST",
                async: false,
                url: "GetTotalTDSQuarterlyListPayableAmount",
                data: { tdsQuarterlyListFilters: tdsQuarterlyListFilters, forder: o.fltopt.order },
                success: function (res) {
                    $("#totalTDSPayableAmount").val(res);
                }
            });
            $('#numberOfRecords').val($('#TDSQuarterlyList').data("api").getResult().ic);
        });
    });
}

function gridPersistentCheckboxes(gid, chkName) {
    var chkSel = '[name=' + chkName + ']';
    var chkAllSel = '[name=ChkAll]';
    var vals = {};
    var chkAllSelected = false;
    var selItems = [];
    var grid = $('#' + gridId);
    //var grid = $('#' + gid);
    //var grid = $('#@(gridId)');
    var rowsel = '.awe-row:not(.o-frow)';
    grid.on('awerender', function () {
        var allChecked = false;
        grid.find(rowsel).each(function () {
            var row = $(this);
            if (vals[row.data('k')]) {
                var apiData = row.find(chkSel).val(false).data('api');
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

    // check/uncheck all
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
            var index = $.map($('#TDSQuarterlyList [name=Id]'), c => c.dataset.val).indexOf(key);
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
            $("#btnQuarterlyFiling").attr("disabled", true);
        }
        else {
            $("#btnQuarterlyFiling").attr("disabled", false);
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
function setParams() {
    Id = $('#TDSQuarterlyList [name=Id]').filter(function () {
        return $(this).val() == 'true';
    }).map(function () {
        return $(this).data('val');
    }).get();
    if (Id.length == 0) {
        Swal.fire({
            text: 'Please select the quarter',
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
        var res = Id;
        var grid = $('#' + gridId);
      //  var grid = $('#@(gridId)');
        var rowsel = '.awe-row:not(.o-frow)';
        grid.find(rowsel).each(function () {
            var row = $(this);
            var rowData = grid.data('api').model(row);
            if (Id.includes(rowData.id)) {
                if (rowData.quarterlyStatus == "CSV Pending") {
                    Swal.fire({
                        text: 'Please Select Only the CSV Created Status!!',
                        icon: 'warning',
                        confirmButtonText: 'Ok',
                    })
                    $('.awe-popup').each(function () {
                        if ($(this).data('api'))
                            $(this).data('api').destroy();
                    });
                    return false;
                }

            } else {

            }
        });


        return { Id: Id };
    }
}


