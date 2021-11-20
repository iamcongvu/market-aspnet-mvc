var dialog_nsx = $("#dialog_nsx");
var thead = $("#thead");

$(function () {
    LoadData();
    $(".pagination").on("click", ".page-item", function () {
        var pages = $(this).find(".page-link").text();
        LoadData(pages);
    });

    $("#SaveRecord").on("click", function () {
        SaveRecord.call();
    });
    $("#btn-Filter").on("click", function () {
        LoadData();
    });

})

function LoadData(pages) {
    var obj = {
        Pages: pages,
        TenNSXFilter: $("#TenNSXFilter").val(),
    }

    $.ajax({
        url: "/NhaSanXuat/GetAllNSX",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.nsx, function (key, val) {
                stringhtml += "<tr>";
                // Cột số thứ tự
                stringhtml += "<td>" + val.index + "</td>";
                // ở đây tạo nút cho event sửa và xóa
                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaNSX + ")'><i class='fas fa-pencil-alt'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaNSX + ")'><i class='far fa-trash-alt'></i></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + val.TenNSX + "</td>";
                stringhtml += "<td>" + val.ThongTin + "</td>";
                stringhtml += "<td>" + val.Logo + "</td>";
                stringhtml += "<tr>";
            });
            $("#tbody").html(stringhtml);
            paging(pages, res.totalpage)
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function GetDataForDialog(id) {
    $.ajax({
        url: "/NhaSanXuat/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            dialog_nsx.modal("show");
            BindingDataForInput(dialog_nsx, res)

        },
        error: function (res) {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/NhaSanXuat/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_nsx),
        success: function (res) {
            dialog_nsx.modal("hide");
            notify(res.Titles, res.Types, res.Messages);
            LoadData();
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DeleteRecord(id) {
    $.ajax({
        url: "/NhaSanXuat/DeleteRecord",
        data: { id: id },
        type: "POST",
        success: function (res) {
            notify(res.Titles, res.Types, res.Messages);
            LoadData();
        },
        error: function (res) {
            console.log(res);
        }
    })
}

function notify(title, type, message) {
    BootstrapDialog.show({
        title: title,
        type: BootstrapDialog[type],
        message: message,
        buttons: [{
            label: 'Đóng',
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}


