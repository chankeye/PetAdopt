function MyViewModel() {
    var self = this;

    self.userlist = ko.observableArray();

    self.removeUser = function (user) {
        if (confirm('確定要刪除？')) {

            if (user.IsDisable == true)
                return;

            $.ajax({
                type: 'post',
                url: '/Manage/User/Delete',
                data: {
                    id: user.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.userlist.remove(user);
                        user.IsDisable = true;
                        self.userlist.push(user);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    self.editUser = function (user) {
        window.location = "/Manage/User/Edit?id=" + user.Id;
    };
}

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

            $.ajax({
                type: 'post',
                url: '/Manage/User/AddUser',
                data: {
                    Account: $("#account").val(),
                    Display: $("#display").val(),
                    Mobile: $("#mobile").val(),
                    Email: $("#email").val(),
                    IsAdmin: $("#checkbox").prop("checked")
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        vm.userlist.push(data.ReturnObject);
                        $("#account").val('');
                        $("#display").val('');
                        $("#mobile").val('');
                        $("#email").val('');
                        $("#checkbox").prop("checked", '');
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
        $("#checkbox").prop("checked", '');
    });

    ko.applyBindings(vm);
});
