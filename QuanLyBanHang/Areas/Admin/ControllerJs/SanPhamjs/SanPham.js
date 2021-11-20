var dialog_product = $("#dialog_product");
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

    GetListNCC();
    GetListNSX();
    GetListLSP();
})

function LoadData(pages, sortname, orderby) {
    var obj = {
        Pages: pages,
        NameFilter: $("#NameFilter").val(),
        PriceFilter: parseFloat($("#PriceFilter").val()),
        PromotionPriceFilter: parseFloat($("#PromotionPriceFilter").val()),
        DescriptionFilter: $("#DescriptionFilter").val(),
        SortName: sortname != null ? sortname : "ID",
        Orderby: orderby != null ? orderby : "DESC",
    }
    $.ajax({
        url: "/ProductAdmin/GetAllProduct",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.sp, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>"; 

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-info' onclick='DetailProduct(" + val.ID + ")'><i class='fas fa-info-circle'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.ID + ")'><i class='fas fa-pencil-alt'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.ID + ")'><i class='far fa-trash-alt'></i></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.Name) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Price) + "</td>";
                stringhtml += "<td>" + FormatNull(val.PromotionPrice) + "</td>";
                stringhtml += "<td>" + Formatdate(val.CreateDate) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Quantity) + "</td>";
                stringhtml += "<td>";
                stringhtml += "<img src='" + val.Image + "' width='100' />";
                stringhtml += "</td>";

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
        url: "/ProductAdmin/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_product, false);
            $("#SaveRecord").show();

            dialog_product.modal("show");
            BindingDataForInput(dialog_product, res);
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DetailProduct(id) {
    $.ajax({
        url: "/ProductAdmin/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {

            ReadonlyInput(dialog_product, true);

            $("#SaveRecord").hide();
            dialog_product.modal("show");
            BindingDataForInput(dialog_product, res)
        },
        error: function () {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/ProductAdmin/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_product),
        success: function (res) {
            dialog_product.modal("hide");
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
        url: "/ProductAdmin/DeleteRecord",
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

function GetListNCC() {
    $.ajax({
        url: "/NhaCungCap/GetListNCC",
        type: "GET",
        success: function (res) {
            $.each(res.allNCC, function (key, val) {
                if (code = 200) {
                    var opt = '<option value="'+val.MaNCC+'">'+val.TenNCC+'</option>';
                    $("#MaNCC").append(opt);
                    //append se appply vao cuoi cua the cha // html se thay the toan bo noi dung the cha
                }
            });
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function GetListNSX() {
    $.ajax({
        url: "/NhaSanXuat/GetListNSX",
        type: "GET",
        success: function (res) {
            $.each(res.allNSX, function (key, val) {
                var opt = '<option value="' + val.MaNSX+'">' + val.TenNSX+'</option>';
                $("#MaNSX").append(opt);
            });
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function GetListLSP() {
    $.ajax({
        url: "/ProductTypes/GetListLSP",
        type: "GET",
        success: function (res) {
            $.each(res.allLSP, function (key, val) {
                var opt = '<option value="' + val.MaLoaiSP+'">' +val.TenLoaiSP+ '</option>';
                $("#MaLoaiSP").append(opt);
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
