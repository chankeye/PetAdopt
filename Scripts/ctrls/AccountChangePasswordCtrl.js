$(document).ready(function () {

    $("#commentForm").validate({
        rules: {
            oldPwd: {
                required: true,
                minlength: 6,
                maxlength: 20
            },
            newPwd: {
                required: true,
                minlength: 6,
                maxlength: 20
            },
            comPwd: {
                equalTo: "#newPwd"
            }
        },
        messages: {
            oldPwd: {
                required: "請輸入舊密碼",
                minlength: "不得小於6個字",
                maxlength: "不得大於20個字"
            },
            newPwd: {
                required: "請輸入新密碼",
                minlength: "不得小於6個字",
                maxlength: "不得大於20個字"
            },
            comPwd: "輸入的密碼不一致"
        }
    });

    $("#btn1").click(
        function () {
            var $btn = $("#btn1");
            var $oldPWD = $("#oldPwd");
            var $newPWD = $("#newPwd");
            var $comPWD = $("#comPwd");

            if ($("#commentForm").valid() == false) {
                return;
            }

            if ($comPWD.val() != $newPWD.val()) {
                alert("新密碼與確認密碼不相同");
                return;
            }

            if ($oldPWD.val() == $newPWD.val()) {
                alert("舊密碼與新密碼相同，無需修改");
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Account/ChangePasswordSubmit',
                data: {
                    oldPassword: $("#oldPwd").val(),
                    newPassword: $("#newPwd").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess === false) {
                        alert(data.ErrorMessage);
                        return;
                    }
                    alert("您的密碼已變更成功，請立即以新密碼重新登入");
                    window.location = '/Account/SignOut';
                }
            });
        });

    // 取消
    $("#btn2").click(
    function ($location) {
        if (history.length > 1)
            history.back();
        else
            $location.url("/");
    });
});