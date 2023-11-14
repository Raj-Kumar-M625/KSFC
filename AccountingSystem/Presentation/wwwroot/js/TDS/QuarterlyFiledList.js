
var pref = "filter";

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
            }
        });

        g.on('awerender', function (e) {
            var quarterlyFiledFilters = {};
            quarterlyFiledFilters.payableMinAmount = $('#filtertdsQuarterlyFiledFilters_payableMinAmount').val();
            quarterlyFiledFilters.payableMaxAmount = $('#filtertdsQuarterlyFiledFilters_payableMaxAmount').val();
            quarterlyFiledFilters.noOfChallan = $('#filtertdsQuarterlyFiledFilters_noOfChallan').val();
            quarterlyFiledFilters.quarter = $('#filtertdsQuarterlyFiledFilters_quarter').val();
            quarterlyFiledFilters.assessmentYear = $('#filtertdsQuarterlyFiledFilters_assessmentYear').val();
            $.ajax({
                type: "POST",
                async: false,
                url: "GetTotalTDSQuarterlyFiledPayableAmount",
                data: { quarterlyFiledFilters: quarterlyFiledFilters, forder: o.fltopt.order },
                success: function (res) {
                    $("#totalTDSPayableAmount").val(res);
                }
            });
            $('#numberOfRecords').val($('#TDSQuarterlyFiledList').data("api").getResult().ic);
        });
    });
}

// get data for filter editor from grid model
function filterData(name) {
    return function () {
        var g = $('#' + gridId);
        var o = g.data('o');
        return awem.frowData(o, name);
    }
}



function setParams() {

    Id = $('#TDSQuarterlyFiledList [name=Id]').filter(function () {

        return $(this).val() == 'true';
    }).map(function () {
        return $(this).data('val');

    }).get();
    return { Id: Id };
}