$(document).ready(function () {
    loaddata();
});
function loaddata() {
    $('#productTable').DataTable({
        "ajax": {
            "url": "/Admin/order/GetData",
        },
        "columns": [
            { "data": "id" },
            { "data": "applicationUser.email" },
            { "data": "phonenumber" },
            { "data": "orderStatus" },
            { "data": "totalPrice" },
            {
                "data": "id",
                "render": function (data) {
                    return `<a href="/Admin/order/Detail/${data}" class="btn btn-success">Detail</a>`
                }
            }
        ],

    });
}

