﻿function MyViewModel() {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    //Add PaginationModel
    //from pagination.js
    ko.utils.extend(self, new PaginationModel());

    self.loadHistory = function (page, take) {
        self.responseMessage($.commonLocalization.loading);
        self.loading(true);
        self.history.removeAll();
        self.pagination(0, 0, 0);
        page = page || 1; // if page didn't send
        take = take || 10;
        $.ajax({
            type: 'post',
            url: '/Activity/GetMessageList',
            data: {
                id: window.id,
                page: page,
                take: take
            }
        }).done(function (response) {
            self.responseMessage('');
            self.history(response.List);
            self.pagination(page, response.Count, take);

            if (response.Count == 0)
                self.responseMessage($.commonLocalization.noRecord);

        }).always(function () {
            self.loading(false);
        });
    };
};

$(function () {

    // 沒有輸入id直接導回
    window.id = window.utils.urlParams("id");
    if (window.id == null)
        window.location = '/Activity';

    // 取得最新活動
    var photo;
    $.ajax({
        type: 'post',
        url: '/Activity/DetailInit',
        data: {
            id: window.id
        },
        success: function (data) {
            if (data.IsSuccess) {
                photo = data.ReturnObject.Photo;
                if (photo != null) {
                    $('#coverPhoto').attr('src', "../../Content/uploads/" + photo);
                }
                $("#title").text(data.ReturnObject.Title);
                $("#selOptionsAreas").text(data.ReturnObject.Area);
                $("#content").html(data.ReturnObject.Message);
                $("#address").text(data.ReturnObject.Address);
                $("#date").text(data.ReturnObject.Date);
                $("#userDisplay").text(data.ReturnObject.UserDisplay);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Activity';
            }
        }
    });

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);

    // 新增留言
    $("#btn1").click(
        function () {
            var $btn = $("#btn1");

            if ($("#commentForm").valid() == false) {
                return;
            }

            $btn.button("loading");

            $.ajax({
                type: 'post',
                url: '/Activity/AddMessage',
                data: {
                    Id: window.id,
                    Message: $("#message").val()
                },
                success: function (data) {
                    $btn.button("reset");
                    if (data.IsSuccess) {
                        window.vm.loadHistory();
                        $("#message").val('');

                        alert("已新增留言");
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
            window.location = '/Activity';
    });
});
