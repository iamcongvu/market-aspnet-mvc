var dialog_ctpn = $("#dialog_ctpn");
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

    GetListSP();
    GetListPN();
})

function LoadData(pages, sortname, orderby) {
    var obj = {
        Pages: pages,
        TenNCCFilter: $("#TenNCCFilter").val(),
        TenSPFilter: $("#TenSPFilter").val(),
        SortName: sortname != null ? sortname : "MaChiTietPN",
        Orderby: orderby != null ? orderby : "DESC",
    }
    $.ajax({
        url: "/ChiTietPhieuNhap/GetAllCTPN",
        type: "GET",
        data: obj,
        success: function (res) {
            var stringhtml = "";
            $.each(res.ctpn, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-info' onclick='DetailRecord(" + val.MaChiTietPN + ")'><span class='glyphicon glyphicon-search'></span></button>"
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaChiTietPN + ")'><span class='glyphicon glyphicon-edit'></span></button>"
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaChiTietPN + ")'><span class='glyphicon glyphicon-trash'></span></button>"
                stringhtml += "</td>";

                stringhtml += "<td>" + val.TenSP + "</td>"
                stringhtml += "<td>" + val.TenNCC + "</td>"
                stringhtml += "<td>" + val.DonGiaNhap + "</td>"
                stringhtml += "<td>" + val.SoLuongNhap + "</td>"

                stringhtml += "</tr>";
            });
            $("#tbody").html(stringhtml);
            paging(pages, res.totalpage)
        },
        error: function (res) {
            console.log(res);
        }
    })
}

function GetDataForDialog(id) {
    $.ajax({
        url: "/ChiTietPhieuNhap/GetDataForDialog",
        type: "GET",
        data: { id: id },
        success: function (res) {
            ReadonlyInput(dialog_ctpn, false);

            dialog_ctpn.modal("show");
            ReadonlyInput(dialog_ctpn, false);

            BindingDataForInput(dialog_ctpn, res);
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DetailRecord(id) {
    $.ajax({
        url: "/ChiTietPhieuNhap/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_ctpn, true);

            $("#SaveRecord").hide();

            dialog_ctpn.modal("show");
            BindingDataForInput(dialog_ctpn, res)
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/ChiTietPhieuNhap/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_ctpn),
        success: function (res) {
            dialog_ctpn.modal("hide");

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
        url: "/ChiTietPhieuNhap/DeleteRecord",
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

function GetListPN() {
    $.ajax({
        url: "/PhieuNhap/GetListPN",
        type: "GET",
        success: function (res) {
            $.each(res.allPN, function (key, val) {
                var opt = '<option value="' + val.MaPN + '">' + val.TenNCC + '</option>';
                $("#MaPN").append(opt);
            });
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function GetListSP() {
    $.ajax({
        url: "/SanPham/GetListSP",
        type: "GET",
        success: function (res) {
            $.each(res.allSP, function (key, val) {
                var opt = '<option value="' + val.MaSP + '">' + val.TenSP + '</option>';
                $("#MaSP").append(opt);
            });
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