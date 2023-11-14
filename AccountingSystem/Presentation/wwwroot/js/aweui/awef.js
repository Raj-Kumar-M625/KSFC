//import * as jQuery from 'jquery';
var awef = function ($) {
    var entityMap = {
        "&": "&amp;",
        "<": "&lt;",
        '"': '&quot;',
        "'": '&#39;',
        ">": '&gt;'
    };

    var idnr = 1;
    var storage = {};

    function isNull(val) {
        return val === undefined || val === null;
    }

    function loop(arr, f) {
        if (arr) {
            for (var i = 0, ln = len(arr); i < ln; i++) {
                var col = arr[i];
                if (f(col, i) === false) {
                    break;
                };
            }
        }
    }

    function len(o) {
        return !o ? 0 : o.length;
    }

    function isNullOrEmp(val) {
        return awef.isNull(val) || len(val.toString()) === 0;
    }

    function strEquals(x, y) {
        if (isNull(x) || isNull(y)) {
            return x === y;
        }

        return x.toString() === y.toString();
    }

    function select(list, func) {
        var res = [];
        loop(list, function (el, i) {
            res.push(func(el, i));
        });

        return res;
    }

    function data(elm, name, obj) {
        var id = getOrAddIdFor(elm);
        var so = storage[id] || {};
        storage[id] = so;

        if (obj) {
            so[name] = obj;
            return id;
        } else {
            if (name) return so[name];
            return so;
        }
    }

    function getOrAddIdFor(elm) {
        elm = $(elm);
        var id = elm.attr('aweid');
        if (!id) {
            id = 'awe-' + (idnr++);
            elm.attr('aweid', id);
        }

        return id;
    }

    function remd(elm) {
        var id = getOrAddIdFor(elm);
        delete storage[id];
    }

    function trigger(elm, ename) {
        if (len(elm)) {
            var ev = new CustomEvent(ename, { bubbles: true });
            elm[0].dispatchEvent(ev);
        }
    }

    return {
        setCookie: function (name, val) {
            document.cookie = name + '=' + val;
        },
        trigger: trigger,
        data: data,
        remd: remd,
        storage: storage,
        outerh: function (o, b) {
            return o.outerHeight(!!b);
        },
        outerw: function (o, b) {
            return o.outerWidth(!!b);
        },
        seq: strEquals,
        len: len,
        sval: function (val) {
            if (Array.isArray(val)) {
                val = JSON.stringify(val);
            }

            return val;
        },
        // from json obj to namevalue array
        serlObj: function (jobj) {
            var res = [];

            for (var key in jobj) {
                if (!Array.isArray(jobj[key]))
                    res.push({ name: key, value: jobj[key] });
                else res = res.concat(awef.serlArr(jobj[key], key));
            }

            return res;
        },
        vcont: function (v, vals) {
            for (var i = 0; i < len(vals); i++) {
                if (strEquals(v, vals[i])) {
                    return 1;
                }
            }

            return 0;
        },
        scont: function (str, src) {
            return isNullOrEmp(src) || str.toLowerCase().indexOf(src.toLowerCase()) > -1;
        },
        loop: loop,
        isNotNull: function (val) {
            return !awef.isNull(val);
        },
        isNull: isNull,
        isNullOrEmp: isNullOrEmp,
        encode: function (str) {
            return String(str).replace(/[&<>"']/g, function (s) {
                return entityMap[s];
            });
        },
        //arr = [1,2,3] k="hi" -> {name:hi, value: 1} ...
        serlArr: function (arr, k) {
            var res = [];
            if (isNull(arr)) return res;

            if (!Array.isArray(arr)) arr = [arr];

            loop(arr,
                function (v) {
                    res.push({ name: k, value: v });
                });

            return res;
        },
        select: select,
        where: function (list, func) {
            var res = [];
            loop(list,
                function (el) {
                    if (func(el)) {
                        res.push(el);
                    }
                });

            return res;
        },
        logErr: console && console.error || function () { }
    };
}(jQuery);

//export {awef};
