//gán dữ liệu vào các input trên dialog
function BindingDataForInput(bindingto, data) {
    bindingto.find("input, select").each(function () {
        var IdInput = $(this).attr("id");//lay ra id cua the input nhu Name, UserName ...
        //console.log(IdInput);// ID, Name, UserName,...//IdInput là các string
        //console.log(data[IdInput]); //38, Thanh Cong, thanhcong,...
        $("#" + IdInput).val(data[IdInput]);//bind data[IdIput] vào element có ID tương ứng
    });
}
// phân trang
function paging(choose, totalpage) {
    if (choose == undefined) choose = 1;
    var pagestring = "";
    if (totalpage <= 5) {
        pagestring += load(0, parseInt(totalpage), (parseInt(choose) - 1));
    } else {
        if (choose <= 2 && choose > 0) {
            pagestring += load(0, 4, (parseInt(choose) - 1));
            pagestring += '<li class="page-item"><a class="page-link">' + (parseInt(totalpage)) + '</a></li>';
        } else if (choose == (parseInt(totalpage) - 1)) {
            pagestring += '<li class="page-item"><a class="page-link">' + 1 + '</a></li>';
            pagestring += load(parseInt(choose) - 3, parseInt(choose), (parseInt(choose) - 1));
            pagestring += '<li class="page-item"><a class="page-link">' + (parseInt(totalpage)) + '</a></li>';
        } else if (choose == totalpage) {
            pagestring += '<li class="page-item"><a class="page-link">' + 1 + '</a></li>';
            pagestring += load(parseInt(choose) - 4, parseInt(choose), (parseInt(choose) - 1));
        }
        else {
            pagestring += '<li class="page-item"><a class="page-link">' + 1 + '</a></li>';
            pagestring += load(parseInt(choose) - 2, parseInt(choose) + 1, (parseInt(choose) - 1));
            pagestring += '<li class="page-item"><a class="page-link">' + (parseInt(totalpage)) + '</a></li>';
        }
    }
    $(".pagination").html(pagestring);
}
//Tạo nút bấm trang
function load(startpage, endpage, activepage) {
    var pagestring = "";
    for (var index = startpage; index < endpage; index++) {
        if (index == activepage) {
            pagestring += '<li class="page-item active"><a class="page-link">' + (index + 1) + '</a></li>';
        } else {
            pagestring += '<li class="page-item"><a class="page-link">' + (index + 1) + '</a></li>';
        }
    }
    return pagestring;
}
//Tạo 1 object json để gưi lên server
function GetDataForInput(GetValueBy) {
    var obj = {};
    GetValueBy.find("input, select").each(function () {
        var IdInput = $(this).attr("id");
        obj[IdInput] = $("#" + IdInput).val();
        //console.log(obj[IdInput])
    });
    return obj;
}

function ReadonlyInput(ReadonlyBy, prop) {
    ReadonlyBy.find("input, select").each(function () {
        var elementid = $(this).attr("id");
        $("#" + elementid).prop("readonly", prop);
    });
}

function KeypressEnter(EnterFor) {
    EnterFor.find("input").each(function () {
        var IdInput = $(this).attr("id");
        $("#" + IdInput).on("keypress", function (e) {
            if (e.which == 13) {
                $("#btn-Filter").click();
            }
        });
    });
}

function FormatNull(data) {
    if (data == null || data == undefined)
        return "";
    return data

}

function Formatdate(fdate) {
    var obj = FormatNull(fdate)
    if (obj === "") {
        return obj;
    }
    else {
        var dateString = obj.substr(6);
        var currentTime = new Date(parseInt(dateString));
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        if (month < 10) {
            month = '0' + month
        }
        if (day < 10) {
            day = '0' + day
        }
        var date = day + "/" + month + "/" + year
        return date;
    }
}

function AuthorizedRequest(response) {
    if (response.code == 403) {
        window.location.href = response.content;
    }
}
