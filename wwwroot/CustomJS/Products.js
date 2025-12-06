

$(document).ready(function () {

})

// Add Product
$("#AddProduct").click(function () {
    
    var formData = new FormData();
    formData.append("ItemPictureFile", $("#ItemPicture")[0].files[0]);
    formData.append("Name", $("#Name").val());
    formData.append("Quantity", $("#Quantity").val());
    formData.append("Price", $("#Price").val());
    $.ajax({
        url: "/Products/AddProduct",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        success: function () {
            alert("Data Added successfully!");
            window.location.href = '/Products/Index';
        }
    })
});

// Select Product By Id
function SelectProduct(id) {
    $.ajax({
        url: "/Products/GetProductById",
            type:"GET",
        data: { id: id },
        success: function (response) {
            $("#Name").val(response.name);
            $("#Quantity").val(response.quantity);
            $("#Price").val(response.price);
            var imageUrl = "/ProductsImages/" + (response.itemPicture ?? "default.png");
            $("#ShowItemImg").attr("src", imageUrl).css({
                "width": "456px",
                "height": "228px",
                "object-fit": "cover"
            });
            $("#ProductsModalLabel").text("Update Product");
            $("#ProductsModal").modal("show");
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    })
}