var dialog_product = $("#dialog_product");

$(function () {
    //debugger
    //LoadData();
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
    
    GetListNCC();
    GetListNSX();
    GetListLSP();
})

function LoadData(pages) {
    var obj = {
        Pages: pages,
        NameFilter: $("#NameFilter").val(),
        PriceFilter: $("#PriceFilter").val(),
        PromotionPriceFilter: $("#PromotionPriceFilter").val(),
        DescriptionFilter: $("#DescriptionFilter").val(),
    }
    $.ajax({
        url: "/SanPham/GetAllSanPham",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.sp, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>"; 

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-info' onclick='DetailProduct(" + val.MaSP + ")'><span class='glyphicon glyphicon-search'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.MaSP + ")'><span class='glyphicon glyphicon-edit'></span></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.MaSP + ")'> <span class='glyphicon glyphicon-trash'></span></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.Name) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Price) + "</td>";
                stringhtml += "<td>" + FormatNull(val.PromotionPrice) + "</td>";
                stringhtml += "<td>" + Formatdate(val.CreateDate) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Detail) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Description) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Image) + "</td>";

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
        url: "/SanPham/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_product, false);

            $("#SaveRecord").show();

            dialog_product.modal("show");
            BindingDataForInput(dialog_product, res);
            $("#MaNCC").val();
            $("#MaNSX").val();
            $("#MaLoaiSP").val();
        },
        error: function () {
            console.log(res);
        }
    });
}

function DetailProduct(id) {
    $.ajax({
        url: "/SanPham/GetDataForDialog",
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
        url: "/SanPham/SaveRecord",
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
        url: "/SanPham/DeleteRecord",
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
        url: "/LoaiSanPham/GetListLSP",
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
