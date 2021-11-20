var dialog_ddh = $("#dialog_ddh");
var thead = $("#thead");
var filter = $('#Filter');

$(function () {
    LoadData();
    $(".pagination").on("click", ".page-item", function () {
        var pages = $(this).find(".page-link").text();
        LoadData(pages);
    });

    $.fn.datepicker.defaults.format = "dd/mm/yyyy";
    $('.datepicker').datepicker();

    $("#SaveRecord").on("click", function () {
        SaveRecord.call();
    });

    $('#TenKHFilter').on('keypress', function (e) {
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
        LoadData(1);
    });

    GetAllKhachHang();
    GetListLTV();
})

function LoadData(pages) {
    var obj = {
        Pages: pages,
        TenKHFilter: $("#TenKHFilter").val(),
        UuDaiFilter: $("#UuDaiFilter").val(),
    }

    $.ajax({
        url: "/DonDatHang/GetAllDDH",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.ddh, function (key, val) {
                stringhtml += "<tr>";
                // Cột số thứ tự
                stringhtml += "<td>" + val.index + "</td>";
                // ở đây tạo nút cho event sửa và xóa
                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-info' onclick='DetailDDH(" + val.MaDDH + ")'><span class='glyphicon glyphicon-search'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaDDH + ")'><span class='glyphicon glyphicon-edit'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaDDH + ")'> <span class='glyphicon glyphicon-trash'></span></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + val.TenKH + "</td>";
                stringhtml += "<td>" + val.UuDai + "</td>";
                stringhtml += "<td>" + Formatdate(val.NgayDat) + "</td>";
                stringhtml += "<td>" + Formatdate(val.NgayGiao) + "</td>";
                stringhtml += "<td>" + val.TinhTrangDatHang + "</td>";
                stringhtml += "<td>" + val.DaThanhToan + "</td>";

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
        url: "/DonDatHang/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_ddh, false);

            $("#SaveRecord").show();

            dialog_ddh.modal("show");

            BindingDataForInput(dialog_ddh, res);
            $("#MaNCC").val();

        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DetailDDH(id) {
    $.ajax({
        url: "/DonDatHang/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_ddh, true);

            $("#SaveRecord").hide();

            dialog_ddh.modal("show");
            BindingDataForInput(dialog_ddh, res)
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/DonDatHang/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_ddh),
        success: function (res) {
            dialog_ddh.modal("hide");
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
        url: "/DonDatHang/DeleteRecord",
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

function GetAllKhachHang() {
    $.ajax({
        url: "/KhachHang/GetAllKhachHang",
        type: "GET",
        success: function (res) {
            $.each(res.allKH, function (key, val) {
                var opt = '<option value="' + val.MaKH + '">' + val.TenKH + '</option>';
                $("#MaKH").append(opt);
            });
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
            $.each(res.allLTV, function (key, val) {
                var opt = '<option value="' + val.LoaiThanhVienID + '">' + val.UuDai + '</option>';
                $("#LoaiThanhVienID").append(opt);
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
        buttons: [{
            label: 'Đóng',
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}


