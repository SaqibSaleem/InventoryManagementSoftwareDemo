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

// Get Location By Id
function SelectLocation(id) {
    LocationId = id;
    $.ajax({
        url: "/Locations/GetLocationById",
        type: "GET",
        data: { id: id },
        success: function (response) {
            if (response.success) {
                $("#LocationName").val(response.data.locationName);
                $("#LocationModalLabel").text("Update Location");
                $("#AddLocation").text("Update");
                $("#LocationModal").modal("show");
            }
            else
            {
                Swal.fire({
                    title: 'Error!',
                    text: response.message,
                    icon: 'Error',
                    timer: 2000
                });
            }
            
        }
    })
}

// Get Location By Id
$(".AddUpdateLocation").click(function () {
    if ($("#AddLocation").text() == "Update") {
        var LocationName = $("#LocationName").val();
        $.ajax({
            url: "/Locations/updateLocation",
            type: "POST",
            data: { id: LocationId, LocationName: LocationName },
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        title: 'Success!',
                        text: response.message,
                        icon: 'Success',
                        timer: 3000
                    });
                    window.location.href = '/Locations/Index';
                }
                else {
                    Swal.fire({
                        title: 'Error!',
                        text: response.message,
                        icon: 'Error',
                        timer: 3000
                    });
                }

            }
        })
    }

});
