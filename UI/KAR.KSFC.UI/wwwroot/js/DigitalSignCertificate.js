﻿function openSDSession(n, t, i, r) {
    var u = null;
    u = n + "://" + t + ":" + i + r;
    console.log("openSDSession::Connecting to: " + u);
    try {
        sdSocket = new WebSocket(u);
        sdSocket.onopen = function (n) {
            console.log("SDWS OPEN: " + JSON.stringify(n, null, 4))
        };
        sdSocket.onclose = function (n) {
            console.log("SDWS CLOSE: " + JSON.stringify(n, null, 4))
        };
        sdSocket.onerror = function (n) {
            console.log("SDWS ERROR: " + JSON.stringify(n, null, 4))
        };
        sdSocket.onmessage = function (n) {
            var t = JSON.parse(n.data);
            t.action == "wsOpen" ? sd_session_id = t.data : t.action == "SignHash" && SignPdfHashAndReturnSignToSDServer(t.data, t.taskid);
            console.log("SDWS MSG: " + t)
        }
    } catch (f) {
        console.error(f)
    }
}

function SignPdfHashAndReturnSignToSDServer(n, t) {
    var i = {};
    i.taskid = t;
    i.action = "AddSign";
    DSCSignRegSignedExtension.signPdfHash(n, $("#CertThumbPrint").val()).then(function (n) {
        if (sdSocket.readyState != WebSocket.OPEN) {
            console.error("sdSocket is not open: " + sdSocket.readyState);
            i.data = "SDHost Error: SD Signer Browser Session is not open.";
            return
        }
        i.data = n;
        sdSocket.send(JSON.stringify(i))
    }, function (n) {
        if (sdSocket.readyState != WebSocket.OPEN) {
            console.error("sdSocket is not open: " + sdSocket.readyState);
            i.data = "SDHost Error: SD Signer Browser Session is not open.";
            return
        }
        i.data = n.message;
        sdSocket.send(JSON.stringify(i))
    })
}

function SignPdfHashAndReturnSignToHub(n, t) {
    DSCSignRegSignedExtension.signPdfHash(n, $("#CertThumbPrint").val(), "SHA-256").then(function (n) {
        sdHub.server.SendSignedData(n, t)
    }, function (n) {
        sdHub.server.SendSignedData(n.message, t);
        $("#ResultDisplay").html(n.message)
    })
}

function isBrowserSupportsExtension() {
    var n = navigator.browserSpecs.name,
        t = navigator.browserSpecs.version;
    //var m = navigator.browserSpecs.DSCSignRegSignedExtension;
    return n == "Chrome" || n == "Edge" ? !0 : (showDownloadLinkDialog(), !1)
}

function isExtensionInstalled() {
    return typeof DSCSignRegSignedExtension == "undefined" ? (showDownloadLinkDialog(), !1) : !0
}

function showDownloadLinkDialog() {
    alert("Digitally Signing Returns or PDF from Browser requires Browser Extension to be installed which is compatible to following browser.\n\n 1. Google Chrome Browser\n\tDownload link : https://xyz.com ")
}
var sdSocket = null,
    sd_session_id = null,
    sdHub, sdHubErrMsg, sdHubConnectionId;
$(function () {
    try {
        sdHub = $.connection.DSCSignRegSignedExtensionHub;
        sdHub.client.GetSigndData = SignPdfHashAndReturnSignToHub;
        $.connection.hub.start().done(function () {
            sdHubConnectionId = $.connection.hub.id
        })
    } catch (n) {
        sdHubErrMsg = n
    }
});
navigator.browserSpecs = function () {
    var i = navigator.userAgent,
        t, n = i.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    return /trident/i.test(n[1]) ? (t = /\brv[ :]+(\d+)/g.exec(i) || [], {
        name: "IE",
        version: t[1] || ""
    }) : n[1] === "Chrome" && (t = i.match(/\b(OPR|Edge)\/(\d+)/), t != null) ? {
        name: t[1].replace("OPR", "Opera"),
        version: t[2]
    } : (n = n[2] ? [n[1], n[2]] : [navigator.appName, navigator.appVersion, "-?"], (t = i.match(/version\/(\d+)/i)) != null && n.splice(1, 1, t[1]), {
        name: n[0],
        version: n[1]
    })
}();



