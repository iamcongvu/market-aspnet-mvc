var dialog_category = $('#dialog_category');
var thead = $('#thead');

$(function () {
    sessionStorage.clear();
    LoadData();
    $(".pagination").on('click', ".page-item", function () {
        var sortname = sessionStorage.getItem("sortname");
        var orderby = sessionStorage.getItem(sortname);
        var pages = $(this).find(".page-link").text();
        LoadData(pages, sortname, orderby);
    });

    $("#SaveRecord").on('click', function () {
        SaveRecord.call();
    });

    $("#btn-Filter").on('click', function () {
        var sortname = sessionStorage.getItem("sortname");
        var orderby = sessionStorage.getItem(sortname);
        LoadData(1, sortname, orderby);
    });

    thead.on('click', function () {
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
        orderby: orderby != null ? orderby : "DESC",
    }
    $.ajax({
        url: "/Category/GetAllCategory",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.category, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-info' onclick='DetailCategory(" + val.ID + ")'><i class='fas fa-info-circle'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.ID + ")'><i class='fas fa-pencil-alt'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.ID + ")'><i class='far fa-trash-alt'></i></button>";
                stringhtml += "</td>"

                stringhtml += "<td>" + FormatNull(val.Name) + "</td>";
                stringhtml += "<td>" + FormatNull(val.ParentID) + "</td>";
                stringhtml += "<td>" + Formatdate(val.CreateDate) + "</td>";
                stringhtml += "<td>" + FormatNull(val.SeoTitle) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Status == true ? "<span class=\"badge badge-success\">Actived</span> "  : "<span class=\"badge badge-danger\">Disabled</span>") + "</td>";

                stringhtml += "</tr>";
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
        url: "/Category/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_category, false);
            dialog_category.modal("show");
            BindingDataForInput(dialog_category, res);
        },
        error: function(res) {
            console.log(res);
        }
    });
}

function DetailCategory(id) {
    $.ajax({
        url: "/Category/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_category, true);

            dialog_category.modal("show");
            BindingDataForInput(dialog_category, res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/Category/SaveRecord",
        data: GetDataForInput(dialog_category),
        type: "POST",
        success: function (res) {
            dialog_category.modal("hide");
            notify(res.Titles, res.Types, res.Message);
            LoadData();
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DeleteRecord(id) {
    $.ajax({
        url: "/Category/DeleteRecord",
        data: { id: id },
        type: "POST",
        success: function (res) {
            notify(res.Titles, res.Types, res.Message);
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