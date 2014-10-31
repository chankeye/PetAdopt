function MyViewModel() {
    var self = this;

    self.areas = ko.observableArray();
};


$(function () {

    var vm = new MyViewModel();

    // 沒有輸入id直接導回
    window.id = window.utils.urlParams("id");
    if (window.id == null)
        window.location = '/Manage/User';

    // 取得使用者
    var account;
    $.ajax({
        type: 'post',
        url: '/Manage/User/EditInit',
        data: {
            id: window.id
        },
        success: function (data) {
            if (data.IsSuccess) {
                account = data.ReturnObject.Account;
                $("#display").val(data.ReturnObject.Display);
                $("#mobile").val(data.ReturnObject.Mobile);
                $("#email").val(data.ReturnObject.Email);
                if (data.ReturnObject.IsAdmin == true)
                    $("#checkbox").prop("checked", true);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Manage/User';
            }
        }
    });

    // 修改使用者
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");

            if ($("#commentForm").valid() == false) {
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Manage/User/EditUser',
                data: {
                    id: window.id,
                    Account: account,
                    Display: $("#display").val(),
                    Mobile: $("#mobile").val(),
                    Email: $("#email").val(),
                    IsAdmin: $("#checkbox").prop("checked")
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        alert("修改完成");
                        window.location = '/Manage/User';
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        if (history.length > 1)
            history.back();
        else
            window.location = '/Manage/User';
    });

    ko.applyBindings(vm);
});
