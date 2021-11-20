var dialog_kh = $("#dialog_kh");
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
        LoadData(1);
    });

    GetListUser();
})

function LoadData(pages) {
    var obj = {
        Pages: pages,
        TenHKFilter: $("#TenHKFilter").val(),
        DiaChiFilter: $("#DiaChiFilter").val(),
        EmailFilter: $("#EmailFilter").val(),
        SDTFilter: $("#SDTFilter").val(),
    }
    $.ajax({
        url: "/Customer/GetAllKH",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.khachhang, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaKH + ")'><i class='fas fa-pencil-alt'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaKH + ")'><i class='far fa-trash-alt'></i></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.TenKH) + "</td>";
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
        url: "/Customer/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            dialog_kh.modal("show");

            setTimeout(function () {
                BindingDataForInput(dialog_kh, res);
                $("#LoaiThanhVienID").val();
            }, 1000);


        },
        error: function (res) {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/Customer/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_kh),
        success: function (res) {
            dialog_kh.modal("hide");
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
        url: "/Customer/DeleteRecord",
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

function GetListUser() {
    $.ajax({
        url: "/Users/GetListUser",
        type: "GET",
        success: function (res) {
            console.log(res);
            $.each(res.allUser, function (key, val) {
                var opt = '<option value="' + val.MaThanhVien + '">' + val.TenLoaiTV + '</option>';
                $("#MaThanhVien").append(opt);
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
