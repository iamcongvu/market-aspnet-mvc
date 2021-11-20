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
        url: "/ProductTypes/GetAllLoaiSP",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.lsp, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaLoaiSP + ")'><i class='fas fa-pencil-alt'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaLoaiSP + ")'><i class='far fa-trash-alt'></i></button>";
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
        url: "/ProductTypes/GetDataForDialog",
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
        url: "/ProductTypes/SaveRecord",
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
        url: "/ProductTypes/DeleteRecord",
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
