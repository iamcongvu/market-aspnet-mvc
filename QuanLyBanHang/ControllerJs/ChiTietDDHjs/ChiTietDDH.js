var dialog_ddh = $("#dialog_ddh");

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

    $("#btn-Filter").on("click", function () {
        LoadData();
    });

    GetListSP();
})

function LoadData(pages) {
    var obj = {
        Pages: pages,
        TenSPFilter: $("#TenSPFilter").val(),
    }
    $.ajax({
        url: "/ChiTietDDH/GetAllChiTietDDH",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.ctddh, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-info' onclick='Detail(" + val.MaCHiTietDDH + ")'><span class='glyphicon glyphicon-search'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaCHiTietDDH + ")'><span class='glyphicon glyphicon-edit'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaCHiTietDDH + ")'> <span class='glyphicon glyphicon-trash'></span></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.TenSP) + "</td>";
                stringhtml += "<td>" + FormatNull(val.DonGia) + "</td>";
                stringhtml += "<td>" + FormatNull(val.SoLuong) + "</td>";

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
        url: "/ChiTietDDH/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_ddh, false);

            $("#SaveRecord").show();

            dialog_ddh.modal("show");
            BindingDataForInput(dialog_ddh, res);
        },
        error: function () {
            console.log(res);
        }
    });
}

function Detail(id) {
    $.ajax({
        url: "/ChiTietDDH/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {

            ReadonlyInput(dialog_ddh, true);

            $("#SaveRecord").hide();
            dialog_ddh.modal("show");
            BindingDataForInput(dialog_ddh, res)
        },
        error: function () {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/ChiTietDDH/SaveRecord",
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
        url: "/ChiTietDDH/DeleteRecord",
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
