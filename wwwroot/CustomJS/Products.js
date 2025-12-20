
let ProductId = 0;
$(document).ready(function () {
    $("#AddProduct").text("Add");
})

// Add Product
$(".AddUpdateProduct").click(function () {
    if ($("#AddProduct").text() == "Add")
    {
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
    }
    
    
});

// Select Product By Id
function SelectProduct(id) {
    ProductId = id;
    $.ajax({
        url: "/Products/GetProductById",
            type:"GET",
        data: { id: id },
        success: function (response) {
            var imageUrl = "";
            $("#Name").val(response.name);
            $("#Quantity").val(response.quantity);
            $("#Price").val(response.price);
            if (response.itemPicture != null) {
                 imageUrl = "/ProductsImages/" + (response.itemPicture);
            }
            else {
                imageUrl = "/assets/images/user/avatar-2.jpg";
            }
            $("#ShowItemImg").attr("src", imageUrl).css({
                "width": "456px",
                "height": "228px",
                "object-fit": "cover"
            });
            $("#ProductsModalLabel").text("Update Product");
            $("#AddProduct").text("Update");
            $("#ProductsModal").modal("show");
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    })
}

// Update Product
$(".AddUpdateProduct").click(function () {
    if ($("#AddProduct").text() == "Update") {
        var formData = new FormData();
        formData.append("ItemPictureFile", $("#ItemPicture")[0].files[0]);
        formData.append("Name", $("#Name").val());
        formData.append("Quantity", $("#Quantity").val());
        formData.append("Price", $("#Price").val());
        formData.append("Id", ProductId);
        $.ajax({
            url: "/Products/UpdateProduct",
            type: "POST",
            processData: false,
            contentType: false,
            data: formData,
            success: function () {
                alert("Data Updated successfully!");
                window.location.href = '/Products/Index';
            }
        })
    }
    
});
