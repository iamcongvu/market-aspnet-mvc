var dialog_lsp = $("#dialog_lsp");

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
});

function LoadData(pages) {
    var obj = {
        Pages: pages,
        TenLoaiSPFilter: $("#TenLoaiSPFilter").val(),
    }
    $.ajax({
        url: "/LoaiSanPham/GetAllLoaiSP",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.lsp, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaLoaiSP + ")'><span class='glyphicon glyphicon-edit'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaLoaiSP + ")'> <span class='glyphicon glyphicon-trash'></span></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.TenLoaiSP) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Icon) + "</td>";
                stringhtml += "<td>" + FormatNull(val.BiDanh) + "</td>";

                stringhtml += "<tr>"
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
        url: "/LoaiSanPham/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            dialog_lsp.modal("show");
            BindingDataForInput(dialog_lsp, res);

        },
        error: function () {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/LoaiSanPham/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_lsp),
        success: function (res) {
            dialog_lsp.modal("hide");
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
        url: "/LoaiSanPham/DeleteRecord",
        type: "POST",
        data: { id: id },
        success: function (res) {
            notify(res.Titles, res.Types, res.Messages);
            LoadData();
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function notify(title, type, message) {
    BootstrapDialog.show({
        title: title,
        type: BootstrapDialog[type],
        message: message,
        button: [{
            label: 'Đóng',
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    })
}
