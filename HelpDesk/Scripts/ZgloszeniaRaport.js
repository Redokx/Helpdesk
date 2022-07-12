$(document).ready(function () {
    $("btnGeneruj").click(function(){
        window.open('../Reports/ReportViewer.aspx', 'newtab');
    });
});

var ReportManager = {
    GenereateReport: function () {
        var jsonParam = "";
        var serviceUrl = "../Home/GetZgloszenieRaport";
        ReportManager.GetReport(serviceUrl, jsonParam, onFailed);

        function onFailed(error) {
            alert(error);
        }
    },

    GetReport: function (serviceUrl, jsonParams, errorCallback) {
        jQuery.ajax({
            url: serviceUrl,
            async: false,
            type: "POST",
            data: "{" + jsonParams + "}",
            contentType: "application/json; charset=utf-8",
            success: function () {
                window.open('../Reports/ReportViewer.aspx', 'newtab');
            },
            error: errorCallback
        });
    }
};
var ReportHelper = {

};