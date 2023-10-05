// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    var DobNotiyReminder = function () {
        $.ajax({
            type: "GET",
            url: "/Dashboard/GetReminderDobNotiy?datetime=" + moment(new Date()).format('MM/DD/YYYY HH:mm'),
            dataType: "json",
            data: {},
            cache: false,
            global: false,
            contentType: "application/json",
            success: function (res) {

            },
            error: function (res, err) {

            }
        });
    };

    //setInterval(function () {
    //    DobNotiyReminder();
    //}, 1000);

   // DobNotiyReminder();

    var SignalRConnectionAndMethods = function () {
        var connection = new signalR.HubConnectionBuilder().withUrl("/birthdayNotificationHub").build();

        connection.on("ReceivedDOBNotification", function (loginid, response) {
            if (response != null && response.length > 0) {
                setTimeout(function () {

                    var options = {
                        "closeButton": true,
                        "newestOnTop": true,
                        "progressBar": false,
                        "positionClass": "toast-bottom-right",
                        "preventDuplicates": false,
                        //"onclick": function () {

                        //    saveReadAsNotify(JSON.stringify(this.data), this.data.notifyid);

                        //    //localStorage.removeItem("IsMissedActivity");
                        //    //localStorage.setItem("selectedactivity", JSON.stringify(this.data));

                        //    //window.location.href = "/Activities/Activities";
                        //},
                        "showDuration": "300000",
                        "hideDuration": "300000",
                        "showMethod": "fadeIn",
                        "hideMethod": "fadeOut",
                        "timeOut": "300000",
                        "extendedTimeOut": "300000",
                    };
                    $.each(response, function (index, obj) {
                        if (obj.firstName != "" && obj.firstName != null && obj.firstName != undefined && obj.lastName != "" && obj.lastName != null && obj.lastName != undefined) {
                            var datatext = obj.firstName + " - " + obj.lastName + " - (" + moment(obj.dob).format('MM/DD/YYYY') + " )";

                            options.data = obj;

                            var TosterHeader = "Tomorrow BirthDay";

                            if (moment(new Date()).format('MM/DD/YYYY') == moment(obj.dob).format('MM/DD/YYYY')) {
                                TosterHeader = "Today BirthDay";
                            }
                            toastr.success(datatext, TosterHeader, options);
                        }
                    });
                    $("#toast-container div").css("width", "375px");
                    $("#toast-container div").css("fontSize", "18px");
                }, 5000);
            }
        });
        connection.start().then(function () {
            console.log("Signal-R connected for DOB notification");
        }).catch(function (err) {
            return console.error(err.toString());
        });
    };

    //SignalRConnectionAndMethods();

    var getcurrentpath = window.location.pathname.split('/');

    if (getcurrentpath.length > 2) {
        if (getcurrentpath[1] != "Orders") {
            window.localStorage.removeItem("grid-order-item-selected-dataid");
        }
    }

    var NotificationForLeave = function ()
    {
        $.ajax({
            url: '/Dashboard/getNotification',
            type: "GET",
            contentType: false,
            processData: false,
            success: function (result) {
                $("#NotificationDiv").html(result);
            },
            error: function (err) {
            }
        });
    };

    NotificationForLeave();

});