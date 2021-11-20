var contact = {
    init: () => {
        contact.registerEvents();
    },
    registerEvents: () => {
        $('#btnSend').off('click').on('click', () => {
            var name = $("#txtName").val();
            var phone = $("#txtPhone").val();
            var address = $("#txtAddress").val();
            var email = $("#txtEmail").val();
            var content = $("#txtContent").val();

            $.ajax({
                url: "/Contact/Send",
                dataType: "JSON",
                type: "POST",
                data: {
                    name: name,
                    phone: phone,
                    address: address,
                    email: email,
                    content: content,
                },
                success: (res) => {
                    if (res.status == true) {
                        alert("Gửi thành công");
                        contact.resetForm();
                    }
                }
            });
        });
    },
    resetForm: () => {
        $("#txtName").val('');
        $("#txtPhone").val('');
        $("#txtAddress").val('');
        $("#txtEmail").val('');
        $("#txtContent").val('');
    }
}
contact.init();