var dialog_productcate = $("#dialog_productcate");
var thead = $("#thead");

$(function () {
    sessionStorage.clear();
    LoadData();
    $(".pagination").on("click", ".page-item", function () {
        debugger
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
})

function LoadData(pages, sortname, orderby) {
    var obj = {
        Pages: pages,
        NameFilter: $("#NameFilter").val(),
        SortName: sortname != null ? sortname : "ID",
        Orderby: orderby != null ? orderby : "DESC",
    }
    $.ajax({
        url: "/ProductCategory/GetAllProdCate",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.prod, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-info' onclick='DetailProductCategory(" + val.ID + ")'><i class='fas fa-info-circle'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.ID + ")'><i class='fas fa-pencil-alt'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.ID + ")'><i class='far fa-trash-alt'></i></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.Name) + "</td>";
                stringhtml += "<td>" + Formatdate(val.CreateDate) + "</td>";
                stringhtml += "<td>" + FormatNull(val.CreateBy) + "</td>";
                stringhtml += "<td>" + Formatdate(val.ModifiedDate) + "</td>";
                stringhtml += "<td>" + FormatNull(val.ModifiedBy) + "</td>";

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
        url: "/ProductCategory/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_productcate, false);

            dialog_productcate.modal("show");
            BindingDataForInput(dialog_productcate, res);
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DetailProductCategory(id) {
    $.ajax({
        url: "/ProductCategory/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_productcate, true);

            dialog_productcate.modal("show");
            BindingDataForInput(dialog_productcate, res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/ProductCategory/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_productcate),
        success: function (res) {
            dialog_productcate.modal("hide");
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
        url: "/ProductCategory/DeleteRecord",
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
