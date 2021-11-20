var cart = {
    init: function () {

        cart.LoadData();
        cart.regEvents();
        $(".pagination").on("click", ".page-item", function () {
            var pages = $(this).find(".page-link").text();
            cart.LoadData(pages);
        });
        
    },
    regEvents: function () {

        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/"; //"redirect to Home"
        });

        $('#btnPayment').off('click').on('click', function () {
            window.location.href = "/thanh-toan"; //"redirect to payment"
        });

        $(".txtQuantity").off("keyup").on("keyup", function () {

            var quantity = parseInt($(this).val());
            var productId = parseInt($(this).data("id"));
            var price = parseFloat($(this).data("price"));
            if (isNaN(quantity) == false) {
                var amount = price * quantity;

                $("#amount_" + productId).text(amount);
                $("#txtTotalOrder").text(cart.getTotal());
            }
            else {
                $("#amount_" + productId).text(0);
                $("#txtTotalOrder").text(0);
            }
        });

        $('#btnDeleteAll').off('click').on('click', function (e) {
            e.preventDefault();
            cart.deleteAll();
        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            productId = parseInt($(this).data("id"));
            cart.deleteItem(productId);
        });
        

        //$("#selectAll").click(function () {
        //    $("input[type=checkbox]").prop("checked", $(this).prop($(this).prop("checked")));
        //});

        //$("input[type=checkbox]").click(function () {
        //    if (!$(this).prop("checked")) {
        //        $("#selectAll").prop("checked", false);
        //    }
        //});
    },
    LoadData: function (pages) {
        var obj = {
            Pages: pages,
        }
        $.ajax({
            url: "/Cart/GetAllCartItem",
            data: obj,
            type: "GET",
            success: function (res) {
                console.log(res);
                var stringhtml = "";
                $.each(res.cartItem, function (key, val) {
                    stringhtml += "<tr>";
                    stringhtml += "<td>" + val.index + "</td>";

                    stringhtml += "<td>" + val.Product.ID + "</td>";
                    stringhtml += "<td>" + val.Product.Name + "</td>";

                    stringhtml += "<td>";
                    stringhtml += "<img src='" + val.Product.Image + "' width='100' />";
                    stringhtml += "</td>";

                    stringhtml += "<td>";
                    stringhtml += "<input type='number' class='txtQuantity' data-id='" + val.Product.ID + "' data-price='" + val.Product.Price + "' value='" + val.Quantity + "' />";
                    stringhtml += "</td>";

                    stringhtml += "<td>" + val.Product.Price + "</td>";

                    stringhtml += "<td id='amount_" + val.Product.ID + "'>" + val.Quantity * val.Product.Price + "</td>";

                    stringhtml += "<td>";
                    stringhtml += "<a href='#' data-id='" + val.Product.ID + "' class='btn-delete'><i class='glyphicon glyphicon-trash'></i></a>";
                    stringhtml += "</td>";

                    stringhtml += "</tr>";
                });
                $("#tbody").html(stringhtml);
                cart.regEvents();
                paging(pages, res.totalPage);
                $("#txtTotalOrder").text(cart.getTotal());
            },
            error: function (res) {
                console.log(res);
            }
        });
    },
    deleteAll: function () {
        $.ajax({
            url: '/Cart/DeleteAllProduct',
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status == true) {
                    window.location.href = "/gio-hang";
                }
            },
            error: function (res) {
                console.log(res);
            }
        })
    },
    deleteItem: function (productId) {
        $.ajax({
            url: '/Cart/DeleteProduct',
            data: { id: productId },
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status == true) {
                    cart.LoadData();
                    window.location.href = "/gio-hang";
                }
            },
            error: function (res) {
                console.log(res);
            }
        })
    },
    getTotal: function() {
        var listTextbox = $(".txtQuantity");
        var total = 0;
        $.each(listTextbox, function (key, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data("price")); 
        });
        return total;
    },
    updateAll: function () {
        var listProduct = $('.txtQuantity');
        var cartList = [];
        $.each(listProduct, function (index, item) {
            cartList.push({
                Quantity: $(item).val(),
                Product: {
                    ID: $(item).data('id')
                }
            });
        });

        $.ajax({
            url: '/Cart/Update',
            data: { cartModel: JSON.stringify(cartList) },//parse cartList to string
            dataType: 'json',
            type: 'POST',
            success: function (res) {
                if (res.status == true) {
                    window.location.href = "/gio-hang";
                }
            },
            error: function (res) {
                console.log(res);
            }
        })
    }
}

cart.init();