
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

// Download Product List as CSV
function exportToExcel() {
    // Show loading
    $('#loading').show();
    $('#exportBtn').prop('disabled', true)
        .html('<i class="fas fa-spinner fa-spin"></i> Generating...');

    // Create a hidden iframe or use fetch
    $.ajax({
        url: '/Products/ExportProductsToExcel',
        type: 'POST',
        xhrFields: {
            responseType: 'blob' // Important for file download
        },
        success: function (data, status, xhr) {
            // Hide loading
            $('#loading').hide();
            $('#exportBtn').prop('disabled', false)
                .html('<i class="fas fa-file-excel"></i> Export Excel');

            // Get filename from response headers or use default
            var filename = "Products.xlsx";
            var disposition = xhr.getResponseHeader('Content-Disposition');
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) {
                    filename = matches[1].replace(/['"]/g, '');
                }
            }

            // Create download link
            var blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            // Show success message
            Swal.fire({
                title: 'Success!',
                text: 'Excel file downloaded successfully',
                icon: 'success',
                timer: 2000
            });
        },
        error: function (xhr, status, error) {
            $('#loading').hide();
            $('#exportBtn').prop('disabled', false)
                .html('<i class="fas fa-file-excel"></i> Export Excel');

            // Handle errors
            if (xhr.status === 404) {
                Swal.fire('Error!', 'Export endpoint not found', 'error');
            } else if (xhr.status === 500) {
                Swal.fire('Error!', 'Server error occurred', 'error');
            } else {
                Swal.fire('Error!', 'Failed to download file: ' + error, 'error');
            }
        }
    });
}

