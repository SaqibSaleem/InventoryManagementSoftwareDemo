let LocationId = 0;
$(document).ready(function () {
})
// Add Location
$(".AddUpdateLocation").click(function () {
    if ($("#AddLocation").text() == "Add") {
        var LocationName = $("#LocationName").val();
        $.ajax({
            url: "/Locations/AddLocation",
            type: "POST",
            data: { LocationName: LocationName },
            success: function () {
                window.location.href = '/Locations/Index';
            }
        })
    }
});
