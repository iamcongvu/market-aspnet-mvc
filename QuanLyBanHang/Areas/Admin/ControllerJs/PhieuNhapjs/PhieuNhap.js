var dialog_phieunhap = $("#dialog_phieunhap");
var thead = $("#thead");
var filter = $('#Filter');

$(function () {
    LoadData();
    $(".pagination").on("click", ".page-item", function () {
        var pages = $(this).find(".page-link").text();
        LoadData(pages);
    });

    $("#SaveRecord").on("click", function () {
        SaveRecord.call();
    });

    $('#TenNCCFilter').on('keypress', function (e) {
        if (e.which == 13) {
            $('#btn-Filter').click();
        }
    });

    $('#NgayNhapFilter').on('keypress', function (e) {
        if (e.which == 13) {
            $('#btn-Filter').click();
        }
    });

    $("#btn-Filter").on("click", function () {
        LoadData(1);
    });
    GetListNCC();

})

function LoadData(pages) {
    var obj = {
        Pages: pages,
        TenNCCFilter: $("#TenNCCFilter").val(),
        NgayNhapFilter: $("#NgayNhapFilter").val(),
    }

    $.ajax({
        url: "/PhieuNhap/GetAllPN",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.phieunhap, function (key, val) {
                stringhtml += "<tr>";
                // Cột số thứ tự
                stringhtml += "<td>" + val.index + "</td>";
                // ở đây tạo nút cho event sửa và xóa
                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-info' onclick='DetailThanhVien(" + val.MaPN + ")'><span class='glyphicon glyphicon-search'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaPN + ")'><span class='glyphicon glyphicon-edit'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaPN + ")'> <span class='glyphicon glyphicon-trash'></span></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + val.TenNCC + "</td>";
                stringhtml += "<td>" + Formatdate(val.NgayNhap) + "</td>";

                stringhtml += "<tr>";
            });
            $("#tbody").html(stringhtml);
            paging(pages, res.totalpage);
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function GetDataForDialog(id) {
    $.ajax({
        url: "/PhieuNhap/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            $("#SaveRecord").show();

            dialog_phieunhap.modal("show");

            setTimeout(function () {
                BindingDataForInput(dialog_phieunhap, res);
                $("#MaNCC").val();
            }, 1000);

        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DetailThanhVien(id) {
    $.ajax({
        url: "/PhieuNhap/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            $("#TenNCC").prop('readonly', true);
            $("#NgayNhap").prop('readonly', true);

            $("#SaveRecord").hide();

            dialog_phieunhap.modal("show");
            BindingDataForInput(dialog_phieunhap, res)
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/PhieuNhap/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_phieunhap),
        success: function (res) {
            dialog_phieunhap.modal("hide");
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
        url: "/PhieuNhap/DeleteRecord",
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

function GetListNCC() {
    $.ajax({
        url: "/NhaCungCap/GetListNCC",
        type: "GET",
        success: function (res) {
            if (res.code == 200) {
                $.each(res.allNCC, function (key, val) {
                    var opt = '<option value="' + val.MaNCC+'">' + val.TenNCC + '</option>';
                    $("#MaNCC").append(opt);
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
        buttons: [{
            label: 'Đóng',
            action: function (dialogRef) {
                dialogRef.close();
            }
        }]
    });
}


