(function () {

    jQuery(function ($) {

        $("#user-form").submit(function (event) {
            console.log("form submitted");

            $("#send-status").text("");

            var userId = Number.parseInt($("#user-id-field").val()) || 0;

            console.log("form submitted. userId = ", userId);

            triggerUserDataUpdate(userId);

            $("#user-id-field").val("");

            event.preventDefault();
            return false;
        });

    });

    function triggerUserDataUpdate(userId) {
        $.ajax({
            "url": "http://localhost:50185/api/User/UpdateHealthData",
            "method": "POST",
            "data": JSON.stringify({ "userId": userId }),
            "contentType": "application/json"
        }).done(function (data) {
            console.log("Request sent successfully.", data);

            $("#send-status").text("Message sent successfully");

        }).fail(function () {
            console.log("Failed to send request.");
        }).always(function () {
            console.log("completed");
        });
    }

}());