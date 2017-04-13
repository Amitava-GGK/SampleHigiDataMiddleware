jQuery(function ($) {

    var notificationHub = $.connection.notificationHub;

    notificationHub.client.userDataUpdateNotification = function (userHealthData) {
        console.log("user data: ", userHealthData);
        showNotification(userHealthData);
    };
    
    $.connection.hub.start().done(function () {

        console.log("signalr connection established.");

    });

    function showNotification(userHealthData) {
        var $notifications = $("#notifications");
        var newNotification = `
            <div class="alert alert-info alert-dismissible" role="alert">
                <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <code>${userHealthData}</code>
            </div>
        `;
        $notifications.append(newNotification);
    }

});
