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
            var imageUrl = "/ProfilePicFolder/" + (response.profilePic ?? "default.png");
            $("#ShowProfileImg").attr("src", imageUrl).css({
                "width": "456px",
                "height": "228px",
                "object-fit": "cover"
            });
            $("#UserDetailModal").modal("show");
        }
        
    });
}
$("#UpdateDetails").click(function () {
    var formData = new FormData();
    formData.append("ProfileImage", $("#ProfilePic")[0].files[0]);
    formData.append("PhoneNumber", $("#ModalNumber").val());
    formData.append("Id", UserId);
    $.ajax({
        url: "/Home/UpdateUserDetails",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function () {
            alert("Data Updated successfully!");
            window.location.href = '/Home/Index';
        }
    })
});