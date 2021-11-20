var dialog_user = $("#dialog_user");
var thead = $("#thead");
var filter = $("#Filter");

$(function () {
    sessionStorage.clear();
    LoadData();
    $(".pagination").on("click", ".page-item", function () {
        var sortname = sessionStorage.getItem("sortname");
        var orderby = sessionStorage.getItem(sortname)
        var pages = $(this).find(".page-link").text();
        LoadData(pages, sortname, orderby);
    });

    //$('#formfrm').validate({
    //    rules: {
    //        Name: {
    //            required: true,
    //            minlength: 5,
    //        },
    //        UserName: "required",
    //        Password: "required",

    //    },
    //    messages: {
    //        Name: {
    //            required: "Bạn phải nhập tên đăng nhập",
    //            minlength: "Tên phải lớn hơn 5 ký tự",
    //        },
    //        UserName: "Bạn phải nhập họ tên",
    //        Password: "Bạn phải nhập mật khẩu",

    //    }
    //});

    KeypressEnter(filter);

    $("#SaveRecord").on("click", function () {
        //if ($('#formfrm').valid()) {
        //    SaveRecord.call();
        //}
        //$('#formfrm').clear();
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
})

function LoadData(pages, sortname, orderby) {
    var obj = {
        Pages: pages,
        NameFilter: $("#NameFilter").val(),
        AddressFilter: $("#AddressFilter").val(),
        EmailFilter: $("#EmailFilter").val(),
        PhoneFilter: $("#PhoneFilter").val(),
        SortName: sortname != null ? sortname : "ID",
        Orderby: orderby != null ? orderby : "DESC",
    }
    $.ajax({
        url: "/Users/GetAllUser",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.user, function (key, val) {
                stringhtml += "<tr>";
                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-xs btn-warning' onclick='GetDataForDialog(" + val.ID + ")'><i class='fas fa-pencil-alt'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-xs btn-danger' onclick='DeleteRecord(" + val.ID + ")'><i class='far fa-trash-alt'></i></button>";
                stringhtml += "<a class='btn btn-xs btn-danger' style='margin-left:10px' href='/Admin/Users/UserRole/" + val.ID + "'><i class='fab fa-angular'></i></a>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.UserName) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Password) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Name) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Adress) + "</td>";

                stringhtml += "<td>";
                stringhtml += "<a href='#' onclick='ChangeStatus(" + val.ID + ")'>" + (val.Status == true ? "Actived" : "Disabled") + "</a>";
                stringhtml += "</td>";

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
        url: "/Users/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            dialog_user.modal("show");
            BindingDataForInput(dialog_user, res);

        },
        error: function (res, xhr) {
            if (xhr.status == 403) {
                alert("you are not allowed to use this function!");
            }
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/Users/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_user),
        success: function (res) {
            dialog_user.modal("hide");
            notify(res.Titles, res.Types, res.Messages);
            LoadData();
        },
        error: function (res, xhr) {
            console.log(res);
            if (xhr.status == 403) {
                alert("Bạn không có quyền sử dụng chức năng này");
            }
        }
    });
}

function DeleteRecord(id) {
    $.ajax({
        url: "/Users/DeleteRecord",
        type: "POST",
        data: { id: id },
        success: function (res) {
            notify(res.Titles, res.Types, res.Messages);
            LoadData();
        },
        error: function (res, xhr) {
            if (xhr.status == 403) {
                alert("Bạn không có quyền sử dụng chức năng này");
            }
        }
    });      
}

function ChangeStatus(id) {
    $.ajax({
        url: "/Users/ChangeStatus",
        data: { id: id },
        type: "POST",
        success: function (res) {
            if (res.status) {
                swal("Kích hoạt trạng thái thành công!", "success");
                $(this).text("Actived");
            } else {
                swal("Vô hiệu hóa trạng thái thành công!", "success");
                $(this).text("Disabled");
            }
        },
        error: function (res) {
            console.log(res);
        }
    });
}


//function GetListLTV() {
//    $.ajax({
//        url: "/LoaiThanhVien/GetListLTV",
//        type: "GET",
//        success: function (res) {
//            if (res.code == 200) {
//                $.each(res.allLTV, function (key, val) {
//                    var opt = '<option value="' + val.LoaiThanhVienID + '">' + val.TenLoai + '</option>';
//                    $("#LoaiThanhVienID").append(opt);
//                });
//            }
//        },
//        error: function (res) {
//            console.log(res);
//        }
//    });
//}

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
