var dialog_thanhvien = $("#dialog_thanhvien");
var thead = $("#thead");
var filter = $('#Filter');

$(function () {
    sessionStorage.clear();
    LoadData();
    $(".pagination").on("click", ".page-item", function () {
        var sortname = sessionStorage.getItem("sortname");
        var orderby = sessionStorage.getItem(sortname);
        var pages = $(this).find(".page-link").text();
        LoadData(pages, sortname, orderby);
    });

    $("#SaveRecord").on("click", function () {
        SaveRecord.call();
    });

    //keypress enter jquery
    //$(GetDataForInput(filter)).on('keypress', function (e) {
    //    if (e.which == 13) {
    //        $('#btn-Filter').click();
    //    }
    //});
    $('#TenLoaiFilter').on('keypress', function (e) {
        if (e.which == 13) {
            $('#btn-Filter').click();
        }
    });

    $('#UuDaiFilter').on('keypress', function (e) {
        if (e.which == 13) {
            $('#btn-Filter').click();
        }
    });

    $("#btn-Filter").on("click", function () {
        var sortname = sessionStorage.getItem("sortname");
        var orderby = sessionStorage.getItem(sortname)
        LoadData(1, sortname, orderby);
    });

    thead.on("click", ".sort", function () {
        var sortname = $(this).data("sort");
        sessionStorage.setItem("sortname", sortname);
        var orderby = sessionStorage.getItem(sortname);
        if (orderby == null || orderby == "ASC") orderby = "DESC";
        else orderby = "ASC";
        sessionStorage.setItem(sortname, orderby);
        LoadData(1, sortname, orderby);
    });

})

function LoadData(pages, sortname, orderby) {
    var obj = {
        Pages: pages,
        TenLoaiFilter: $("#TenLoaiFilter").val(),
        UuDaiFilter: $("#UuDaiFilter").val(),
        SortName: sortname != null ? sortname : "LoaiThanhVienID",
        Orderby: orderby != null ? orderby : "DESC",
    }

    $.ajax({
        url: "/LoaiThanhVien/GetALlThanhVien",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.thanhvien, function (key, val) {
                stringhtml += "<tr>";
                // Cột số thứ tự
                stringhtml += "<td>" + val.index + "</td>";
                // ở đây tạo nút cho event sửa và xóa
                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-info' onclick='DetailThanhVien(" + val.LoaiThanhVienID + ")'><span class='glyphicon glyphicon-search'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.LoaiThanhVienID + ")'><span class='glyphicon glyphicon-edit'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.LoaiThanhVienID + ")'> <span class='glyphicon glyphicon-trash'></span></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + val.TenLoai + "</td>";
                stringhtml += "<td>" + val.UuDai + "</td>";
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
        url: "/LoaiThanhVien/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            $("#SaveRecord").show();

            $("#TenLoai").prop('readonly', false);
            $("#UuDai").prop('readonly', false);

            dialog_thanhvien.modal("show");
            BindingDataForInput(dialog_thanhvien, res)

        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DetailThanhVien(id) {
    $.ajax({
        url: "/LoaiThanhVien/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            $("#TenLoai").prop('readonly', true);
            $("#UuDai").prop('readonly', true);

            $("#SaveRecord").hide();

            dialog_thanhvien.modal("show");
            BindingDataForInput(dialog_thanhvien, res)
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/LoaiThanhVien/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_thanhvien),
        success: function (res) {
            dialog_thanhvien.modal("hide");
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
        url: "/LoaiThanhVien/DeleteRecord",
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


