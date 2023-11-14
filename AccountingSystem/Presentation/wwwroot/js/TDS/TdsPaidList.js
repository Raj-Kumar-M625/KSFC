function exportToExcel() {
    var url = `/${contr}/ExportTdsPaidList`;
    // var url = '@Url.Action("ExportTdsPaidList", contr)';
    var $form = $('<form method="post"/>').attr('action', url).appendTo('body');
    var grid = $('#' + gridId);
    // var grid = $('#@(gridId)');

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
    var toDate = $('#filterchallanFilters_ToDate').val();
    //site.dayfirst(opt, toDate);
}

function daylast(opt) {
    var fromDate = $('#filterchallanFilters_FromDate').val();
    //site.daylast(opt, fromDate);
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

        g.on('awerender', function (e) {
            var tdsPaidfilters = {};
            tdsPaidfilters.payableMinAmount = $('#filtertdsPaidFilters_payableMinAmount').val();
            tdsPaidfilters.payableMaxAmount = $('#filtertdsPaidFilters_payableMaxAmount').val();
            tdsPaidfilters.fromDate = $('#filtertdsPaidFilters_fromDate').val();
            tdsPaidfilters.toDate = $('#filtertdsPaidFilters_toDate').val();
            tdsPaidfilters.tdsSection = $('#filtertdsPaidFilters_tdsSection').val();
            tdsPaidfilters.vendorName = $('#filtertdsPaidFilters_vendorName').val();
            tdsPaidfilters.paymentReferenceNo = $('#filtertdsPaidFilters_paymentReferenceNo').val();
            tdsPaidfilters.tdsStatus = $('#filtertdsPaidFilters_tdsStatus').val();
            tdsPaidfilters.challanNo = $('#filtertdsPaidFilters_challanNo').val();
            tdsPaidfilters.utrNo = $('#filtertdsPaidFilters_utrNo').val();
            tdsPaidfilters.bsrCode = $('#filtertdsPaidFilters_bsrCode').val();
            $.ajax({
                type: "POST",
                async: false,
                url: "GetTotalTDSPaidAmount",
                data: { tdsPaidfilters: tdsPaidfilters, forder: o.fltopt.order },
                success: function (res) {
                    $("#totalTDSPaidAmount").val(res);
                }
            });
            $('#numberOfRecords').val($('#TDSPaidList').data("api").getResult().ic);
        });
    });
}

// get data for filter editor from grid model
function filterData(name) {
    return function () {
        var g = $('#@gridId');
        var o = g.data('o');
        return awem.frowData(o, name);
    }
}


