//$("#UpdateDetails").click(function () {

//
//});
let UserId = 0;
$(document).ready(function () {
    
})
function GetUser(id) {
    UserId = id;
    $.ajax({
        url: "/Home/GetUserById",
        type: "GET",
        data: { id: id },
        success: function (response) {
            $("#ModalEmail").val(response.email);
            $("#ModalNumber").val(response.phoneNumber);
            $("#UserDetailModal").modal("show");
        }
        
    });
}
$("#UpdateDetails").click(function () {
    var PhoneNumber = $("#ModalNumber").val();
    $.ajax({
        url: "/Home/UpdateUserDetails",
        type: "POST",
        data: { PhoneNumber: PhoneNumber, Id: UserId },
        success: function () {
            alert("Data Updated successfully!");
            window.location.href = '/Home/Index';
        }
    })
});