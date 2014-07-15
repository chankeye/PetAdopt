function MyViewModel() {
    var self = this;

    self.userlist = ko.observableArray();

    self.removeUser = function (user) {
        if (confirm('確定要刪除？')) {
            $.ajax({
                type: 'post',
                url: '/Manage/User/DeleteUser',
                data: {
                    id: user.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.userlist.remove(user);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }
};

$(function () {

    var vm = new MyViewModel();

    // 取得使用者列表
    $.ajax({
        type: 'post',
        url: '/Manage/User/GetUserList',
        success: function (data) {
            vm.userlist(data);
        }
    });

    // 新增使用者
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");

            if ($("#commentForm").valid() == false) {
                return;
            }

            $btn.button("loading");
            var isAdmin;
            if ($("#isAdmin").checked > 0) {
                isAdmin = true;
            } else {
                isAdmin = false;
            }
            $.ajax({
                type: 'post',
                url: '/Manage/User/AddUser',
                data: {
                    Account: $("#account").val(),
                    Display: $("#display").val(),
                    Mobile: $("#mobile").val(),
                    Email: $("#email").val(),
                    IsAdmin: isAdmin
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        vm.userlist.push(data.ReturnObject);
                        $("#account").val('');
                        $("#display").val('');
                        $("#mobile").val('');
                        $("#email").val('');
                        $("#isAdmin").attr("checked", '');
                        alert("新增成功");
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        });

    // 取消
    $("#btn2").click(
    function () {
        $("#account").val('');
        $("#display").val('');
        $("#mobile").val('');
        $("#email").val('');
        $("#isAdmin").attr("checked", '');
    });

    ko.applyBindings(vm);
});
