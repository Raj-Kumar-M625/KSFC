


function showbuttonByStatus(gridId, prop) {

    if (gridId.billReferenceNo == "N/A") {
        return "<a href='#'>" + gridId.billReferenceNo + "</a>"
    } else {
        console.log(gridId.id);
        return "<a href='#'  onclick='window.location.href=\"" + getBillDetails(gridId.billReferenceNo, "ListOfTransaction") + "\"'>" + gridId.billReferenceNo + "</a>"
    }
}
function getBillDetails(id, list) {
    return "/Bill/BillDetails" + '?id=' + id + '&ModuleType=' + list; 
    //return "@Url.Action("BillDetails", "Bill")?id=" + id + '&ModuleType=' + list;
    //return "@Url.Action("BillDetails", "Bill", new { id = ".(Id)", ModuleType = "list" });

}
//Dwonlaod Function
function exportToExcel() {
  
   
    var url = `/${contr}/ExportToGridListOfTransaction`;
    //var url = '@Url.Action("ExportTdsList", contr)';
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
        var g = $('#' + grid);
        var o = g.data('o');
        return awem.frowData(o, name);
    }
} 
