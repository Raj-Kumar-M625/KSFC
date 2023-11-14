
var pref = "filter";
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
            $.when($.post(`/GSTTDS/GetTotalGSTTDSChallanPayableAmount`))
                .done(function (res) {
                    $("#totalGSTTDSPayableAmount").val(res);
                });
            $('#numberOfRecords').val($('#JSONFileList').data("api").getResult().ic);
        });
    });
}

function gridPersistentCheckboxes(gid, chkName) {
    var chkSel = '[name=' + chkName + ']';
    var chkAllSel = '[name=ChkAll]';
    var vals = {};
    var chkAllSelected = false;
    var grid = $('#' + gridId);
   // var grid = $('#@gridId');
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

    // check/uncheck all
    grid.on('change', chkAllSel, function () {
        if (!chkAllSelected && $(this).val() === 'false') {
            $(this).val('true').change();
            chkAllSelected = true;
            return;
        }
        var val = $(this).val();
        var isChecked = !chkAllSelected && val === 'true';

        grid.find(rowsel).each(function () {
            var $row = $(this);
            if ($row.find(chkSel).val(val).data('api') != undefined)
                $row.find(chkSel).val(val).data('api').render();
            var key = $row.data('k');
            if (isChecked) vals[key] = 1;
            else delete vals[key];
        });
        chkAllSelected = isChecked;
        enableUpdateUTR();
    });

    // check single row
    grid.on('change', chkSel, function () {
        var chk = $(this);
        var isChecked = chk.val() === 'true';
        var key = chk.closest(rowsel).data('k');
        if (isChecked) {
            vals[key] = 1;
        }
        else {
            var isChkAllChecked = $("input#ChkAll").val() == 'true';
            if (isChkAllChecked) {
                $('input#ChkAll').next().find('.o-chk').removeClass('o-chked');
                chkAllSelected = false;
                $("input#ChkAll").val("false")

            }
            delete vals[key];
        }
        enableUpdateUTR();
    });

    function enableUpdateUTR() {
        if (Object.keys(vals).length > 0) {
            $("#btnEnterUTR").removeAttr("disabled");
        }
        else {
            $("#btnEnterUTR").attr("disabled", "disabled");
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
        var g = $('#' + gridId);
        var o = g.data('o');
        return awem.frowData(o, name);
    }
}

function exportToExcel() {
    var url = `/${contr}/ExportGstTdsJsonList`;
   // var url = '@Url.Action("ExportGstTdsJsonList", contr)';
    var $form = $('<form method="post"/>').attr('action', url).appendTo('body');
    var grid = $('#' + gridId);
    //var grid = $('#@(gridId)');

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







function dayfirst(opt) {
    // var toDate = $('#filterjsonFileFilters_ToDate').val();
    //site.dayfirst(opt, toDate);
}

function daylast(opt) {
    //  var fromDate = $('#filterjsonFileFilters_FromDate').val();
    //site.daylast(opt, fromDate);
}
function setParams() {

    Id = $('#JSONFileList [name=Id]').filter(function () {

        return $(this).val() == 'true';
    }).map(function () {
        return $(this).data('val');

    }).get();
    return { Id: Id };
}

 