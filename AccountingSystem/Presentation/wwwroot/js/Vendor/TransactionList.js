
var myElement = document.getElementById('myElement');

var ID = myElement.getAttribute('data-id');
var pref = "filter";

function exportToExcel() {
    var url = `/${contr}/ExportToTransactions`;
   // var url = '@Url.Action("ExportToTransactions", contr)';
    var $form = $('<form method="post"/>').attr('action', url).appendTo('body');

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
    // add the id parameter to the form
    $form.append("<input type='hidden' name='id' value='" + ID+ "'/>");
    //$form.append($('#allPages').clone());
    $form.submit();
    $form.remove();
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

function gridCheckboxes(gridId) {
    // select/unselect all
    var $grid = $('#' + gridId);
    $grid.on('change', '#ChkAll', function () {
        $grid.find('[name=Id]').val($(this).val()).change();
    }).on('aweload', function () {
        $('#ChkAll').val('').change();
    });
}
$(".max_val").change('input', function () {
    $(".max_val").each(function () {
        var maxval = parseInt($(this).val());
        var minval = parseInt($(".min_val").val());
        if (minval > maxval) {
            Swal.fire({
                text: 'Maximum amount cannot be less than Minimum amount',
                icon: 'warning',
                confirmButtonText: 'Ok',
            }).then((result) => {


            })
        } $(".max_val").val("");

    });

});
$(".pstatus").each(function () {
    var status = $(this).val();
    if (status == "Pending") {
        var color = "red";
    }



});

