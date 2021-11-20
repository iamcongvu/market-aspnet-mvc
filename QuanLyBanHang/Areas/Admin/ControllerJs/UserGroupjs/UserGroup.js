var dialog_userGroup = $("#dialog_userGroup");
var thead = $("#thead");
var filter = $('#Filter');

$(function () {
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

    KeypressEnter(filter);

    $("#btn-Filter").on("click", function () {
        var sortname = sessionStorage.getItem("sortname");
        var orderby = sessionStorage.getItem(sortname)
        LoadData(1, sortname, orderby);
    });

    thead.on("click", ".sort", function () {
        var sortname = $(this).data("sort");//lay duoc value "Ten Loai"
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
        url: "/UserGroup/GetALlUserGroup",
        data: obj,
        type: "GET",
        success: function (res) {
            var stringhtml = "";
            $.each(res.userGroup, function (key, val) {
                stringhtml += "<tr>";

                stringhtml += "<td>" + val.index + "</td>";

                stringhtml += "<td>";
                stringhtml += "<button type='button' class='btn btn-info' onclick='DetailThanhVien(" + val.ID + ")'><i class='fas fa-info-circle'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-warning' onclick='GetDataForDialog(" + val.ID + ")'><i class='fas fa-pencil-alt'></i></button>";
                stringhtml += "<button type='button' style='margin-left:10px' class='btn btn-danger' onclick='DeleteRecord(" + val.ID + ")'><i class='far fa-trash-alt'></i></button>";
                stringhtml += "</td>";

                stringhtml += "<td>" + FormatNull(val.ID) + "</td>";
                stringhtml += "<td>" + FormatNull(val.Name) + "</td>";
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
        url: "/UserGroup/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {

            $("#SaveRecord").show();
            ReadonlyInput(dialog_userGroup, false);

            dialog_userGroup.modal("show");
            BindingDataForInput(dialog_userGroup, res)

        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DetailThanhVien(id) {
    $.ajax({
        url: "/UserGroup/GetDataForDialog",
        data: { id: id },
        type: "GET",
        success: function (res) {
            ReadonlyInput(dialog_userGroup, true);

            $("#SaveRecord").hide();

            dialog_userGroup.modal("show");
            BindingDataForInput(dialog_userGroup, res)
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function SaveRecord() {
    $.ajax({
        url: "/UserGroup/SaveRecord",
        type: "POST",
        data: GetDataForInput(dialog_userGroup),
        success: function (res) {
            dialog_userGroup.modal("hide");
            swal("Thêm loại thành viên", "Bạn đã thêm loại thành viên thành công", "success");
            LoadData();
        },
        error: function (res) {
            console.log(res);
        }
    });
}

function DeleteRecord(id) {
    swal({
        title: "Bạn đã chắc chưa?",
        text: "Khi đã xóa, bạn sẽ không khôi phục lại được!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
    .then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: "/UserGroup/DeleteRecord",
                data: { id: id },
                type: "POST",
                success: function (res) {
                    swal("Xóa loại thành viên thành công", {
                        icon: "success",
                    });
                    //notify(res.Titles, res.Types, res.Messages);
                    LoadData();
                },
                error: function (res) {
                    console.log(res);
                }
            });
        } else {
            swal("Xóa loại thành viên thất bại!");
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


