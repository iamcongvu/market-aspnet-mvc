var dialog_user = $("#dialog_user");
var thead = $("#thead");

$(function () {
    sessionStorage.clear();
    LoadData();
    $(".pagination").on("click", ".page-item", function () {
        var sortname = sessionStorage.getItem("sortname");
        var orderby = sessionStorage.getItem(sortname)
        var pages = $(this).find(".page-link").text();
        LoadData(pages, sortname, orderby);
    });

    $("#SaveRecord").on("click", function () {
        SaveRecord.call();
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
    GetListLTV();
})

function LoadData(pages, sortname, orderby) {
    var obj = {
        Pages: pages,
        HoTenFilter: $("#HoTenFilter").val(),
        DiaChiFilter: $("#DiaChiFilter").val(),
        EmailFilter: $("#EmailFilter").val(),
        SoDienThoaiFilter: $("#SoDienThoaiFilter").val(),
        SortName: sortname != null ? sortname : "MaThanhVien",
        Orderby: orderby != null ? orderby : "DESC",
    }
    $.ajax({
        url: "/Users/GetAllUser",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.user, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaThanhVien + ")'><span class='glyphicon glyphicon-edit'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaThanhVien + ")'> <span class='glyphicon glyphicon-trash'></span></button>";
                stringhtml += "<a class='btn btn-xs btn-danger' style='margin-left:10px' href='/Admin/Users/UserRole/" + val.MaThanhVien + "'> <span class='glyphicon glyphicon-adjust'></span></a>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.TaiKhoan) + "</td>";
                stringhtml += "<td>" + FormatNull(val.MatKhau) + "</td>";
                stringhtml += "<td>" + FormatNull(val.HoTen) + "</td>";
                stringhtml += "<td>" + FormatNull(val.DiaChi) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Email) + "</td>";
                stringhtml += "<td>" + FormatNull(val.SoDienThoai) + "</td>";
                stringhtml += "<td>" + FormatNull(val.TenLoaiTV) + "</td>";

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
        url: "/Users/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            dialog_user.modal("show");
            setTimeout(function () {
                BindingDataForInput(dialog_user, res);
                $("#LoaiThanhVienID").val();
            }, 1000);
        },
        error: function (res) {
            alert(res.statusCode());
            if (res.code == '401') {
                window.location.href = res.content;
            }
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/Users/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_user),
        success: function (res) {
            dialog_user.modal("hide");
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
        url: "/Users/DeleteRecord",
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

function GetListLTV() {
    $.ajax({
        url: "/LoaiThanhVien/GetListLTV",
        type: "GET",
        success: function (res) {
            if (res.code == 200) {
                $.each(res.allLTV, function (key, val) {
                    var opt = '<option value="' + val.LoaiThanhVienID + '">' + val.TenLoai + '</option>';
                    $("#LoaiThanhVienID").append(opt);
                });
            }
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
