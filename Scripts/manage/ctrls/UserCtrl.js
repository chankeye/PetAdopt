﻿var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

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
                        self.history.remove(user);
                        user.IsDisable = true;
                        self.history.push(user);
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

    //Add PaginationModel
    //from pagination.js
    ko.utils.extend(self, new PaginationModel());

    self.loadHistory = function (page, take, query, isLike) {
        self.responseMessage($.commonLocalization.loading);
        self.loading(true);
        self.history.removeAll();
        self.pagination(0, 0, 0);
        page = page || 1; // if page didn't send
        take = take || 10;
        query = query || "";
        if (isLike == null)
            isLike = true;
        $.ajax({
            type: 'post',
            url: '/Manage/User/GetUserList',
            data: {
                page: page,
                take: take,
                query: query,
                isLike: isLike
            }
        }).done(function (response) {
            for (var i = 0; i < response.List.length; i++) {
                var date = new Date(response.List[i].Date);
                response.List[i].Date = date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate();
            }
            self.responseMessage('');
            self.history(response.List);
            self.pagination(page, response.Count, take);

            if (response.Count == 0)
                self.responseMessage($.commonLocalization.noRecord);

        }).always(function () {
            self.loading(false);
        });
    };
}

$(function () {

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);

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
                        //vm.userlist.push(data.ReturnObject);
                        window.vm.loadHistory();
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

    // 查詢
    $("#btn3").click(function () {
        var $btn = $("#btn3");

        $btn.button("loading");
        window.vm.loadHistory(1, 10, $("#search").val(), !$("#checkAll").is(':checked'));
        $btn.button("reset");
    });
});
