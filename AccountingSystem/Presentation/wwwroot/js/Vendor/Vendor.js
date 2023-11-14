
var pref = "filter";

//Dwonlaod Function
function exportToExcel() {
   
    // var url = 'ExportToGridVendorList/contr/' ;
   // var url = `/Vendor/ExportToGridVendorList`;
    var url = `/${contr}/ExportToGridVendorList`;
    // var url = '@Url.Action("ExportToGridVendorList", contr)';
    var $form = $('<form method="post"/>').attr('action', url).appendTo('body');
    // var grid = $('#' + gridId);
    var grid = $('#' + gridId);
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

            //if (fopt.model) {
            //    o.fltopt.model = fopt.model;
            //    o.fltopt.order = fopt.order;

            //    // set persisted model filter params
            //    var res = awef.serlObj(fopt.model);
            //    res = res.concat(awef.serlArr(fopt.order, 'forder'));
            //    o.fparams = res;
            //    var model = fopt.model;

            //    g.one('aweload', function () {
            //        for (var prop in model) {
            //            var editor = $('#' + pref + prop);
            //            if (editor.length) {
            //                editor.val(awef.sval(model[prop]));
            //                if (editor.closest('.awe-txt-field')) {
            //                    editor.data('api').render();
            //                }
            //            }
            //        }
            //    });
            //}
        }

        g.on('aweload', function (e) {
            if ($(e.target).is(g)) {
                fopt = o.fltopt;
                storage[fkey] = JSON.stringify({ model: fopt.model, order: fopt.order });
            }
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






