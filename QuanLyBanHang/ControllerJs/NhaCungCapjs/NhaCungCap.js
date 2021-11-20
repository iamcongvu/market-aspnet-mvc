var dialog_nhacungcap = $("#dialog_nhacungcap");
var thead = $("#thead");

$(function () {
    //debugger
    //LoadData();
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

    $("#btn-Filter").on("click", function () {
        var sortname = sessionStorage.getItem("sortname");
        var orderby = sessionStorage.getItem(sortname);
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
});

function LoadData(pages, sortname, orderby) {
    var obj = {
        Pages: pages,
        TenNCCFilter: $("#TenNCCFilter").val(),
        DiaChiFilter: $("#DiaChiFilter").val(),
        EmailFilter: $("#EmailFilter").val(),
        SoDienThoaiFilter: $("#SoDienThoaiFilter").val(),
        FaxFilter: $("#FaxFilter").val(),
        SortName: sortname ? sortname : "MaNCC",
        Orderby: orderby ? orderby : "DESC",
    }
    $.ajax({
        url: "/NhaCungCap/GetAllNhaCungCap",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.ncc, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaNCC + ")'><span class='glyphicon glyphicon-edit'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaNCC + ")'> <span class='glyphicon glyphicon-trash'></span></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.TenNCC) + "</td>";
                stringhtml += "<td>" + FormatNull(val.DiaChi) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Email) + "</td>";
                stringhtml += "<td>" + FormatNull(val.SoDienThoai) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Fax) + "</td>";

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
        url: "/NhaCungCap/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            dialog_nhacungcap.modal("show");
            BindingDataForInput(dialog_nhacungcap, res);
        },
        error: function () {
            console.log(res);
        }
    })
}

function SaveRecord() {
    debugger
    $.ajax({
        url: "/NhaCungCap/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_nhacungcap),
        success: function (res) {
            dialog_nhacungcap.modal("hide");
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
        url: "/NhaCungCap/DeleteRecord",
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
