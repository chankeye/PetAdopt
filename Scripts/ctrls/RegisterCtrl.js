var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);
}

$(function () {

    window.vm = new MyViewModel();
    ko.applyBindings(window.vm);

    $("#commentForm").validate({
        rules: {
            // 注冊用戶名
            account: {
                required: true,
                minlength: 6,
                maxlength: 20
            },
            // 注冊暱稱
            display: {
                required: true,
                maxlength: 10
            },
            // email
            email: {
                required: true,
                email: true,
                maxlength: 50
            },
            // 密碼
            password: {
                required: true,
                minlength: 6,
                maxlength: 20
            },
            // 確認密碼
            comPassword: {
                equalTo: "#password"
            },
            // 手機
            mobile: {
                maxlength: 10
            },
        },
        messages: {
            // 帳號
            account: {
                required: "請輸入帳號",
                minlength: "不得小於6個字",
                maxlength: "不得大於20個字"
            },
            // 暱稱
            diaplay: {
                required: "請輸入暱稱",
                minlength: "不得小於6個字",
                maxlength: "不得大於20個字"
            },
            // email
            email: {
                required: "請輸入email",
                email: "email格式不正確",
                maxlength: "不得大於50個字"
            },
            // 密碼
            password: {
                required: "請輸入密碼",
                minlength: "不得小於6個字",
                maxlength: "不得大於20個字"
            },
            // 確認密碼
            comPassword: "輸入的密碼不一致",
            // 帳號
            mobile: {
                maxlength: "不得大於10個字"
            }
        }
    });

    // 新增
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");

            if ($("#commentForm").valid() == false) {
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Account/RegisterSumit',
                data: {
                    Account: $("#account").val(),
                    Display: $("#display").val(),
                    Mobile: $("#mobile").val(),
                    Email: $("#email").val(),
                    Password: $("#password").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("新增成功");
                        window.location = '/Account/Login';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });
});
