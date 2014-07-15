$(document).ready(function () {

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
                url: '/Manage/Account/ChangePasswordSubmit',
                data: {
                    oldPassword: $("#oldPwd").val(),
                    newPassword: $("#newPwd").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.isSuccess === false) {
                        alert(data.errorMessage);
                        return;
                    }
                    alert("您的密碼已變更成功，請立即以新密碼重新登入");
                    window.location = '/Manage/Account/SignOut';
                }
            });
        });

    // 取消
    $("#btn2").click(
    function ($location) {
        if (history.length > 1)
            history.back();
        else
            $location.url("/Manage");
    });
});