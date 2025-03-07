$(document).ready(function () {
    loaddata();
});
function loaddata() {
        $('#productTable').DataTable({
            "ajax": {
                "url": "/Admin/product/GetProduct",            
            },
            "columns": [
                { "data": "id" },
                { "data": "name" },
                { "data": "description" },
                { "data": "price" },
                { "data": "categoryName" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `<a href="/Admin/product/EditProduct/${data}" class="btn btn-success">Edit</a>
                                <a onclick=DeleteItem('/Admin/product/DeleteProduct/${data}')   class="btn btn-danger">Delete</a>`;
                    }
                }
            ],
           
        });
};
function DeleteItem(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        $('#productTable').DataTable().ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
    
