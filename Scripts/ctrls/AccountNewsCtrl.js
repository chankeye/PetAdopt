﻿var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.areas = ko.observableArray();

    self.detail = function (data) {
        window.location = "/News/Detail?id=" + data.Id;
    }

    self.removeNews = function (news) {
        if (confirm('確定要刪除？')) {
            $.ajax({
                type: 'post',
                url: '/Manage/News/Delete',
                data: {
                    id: news.Id,
                },
                success: function (data) {
                    if (data.IsSuccess) {
                        self.history.remove(news);
                    } else {
                        alert(data.ErrorMessage);
                    }
                }
            });
        }
    }

    self.editNews = function (news) {
        window.location = "/News/Edit?id=" + news.Id;
    }

    //Add PaginationModel
    //from pagination.js
    ko.utils.extend(self, new PaginationModel());

    self.loadHistory = function (page, take, query, isLike, areaId, classId, statusId) {
        self.responseMessage($.commonLocalization.loading);
        self.loading(true);
        self.history.removeAll();
        self.pagination(0, 0, 0);
        page = page || 1; // if page didn't send
        take = take || 10;
        query = query || "";
        areaId = areaId || -1;
        classId = classId || -1;
        statusId = statusId || -1;
        if (isLike == null)
            isLike = true;
        $.ajax({
            type: 'post',
            url: '/News/GetNewsList',
            data: {
                page: page,
                take: take,
                query: query,
                isLike: isLike,
                areaId: areaId,
                classId: classId,
                statusId: statusId,
                memberOnly: true
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
    // 取得地區列表
    $("#selOptionsSearch").append(window.utils.optionsAreas);
    window.utils.getAreaList();

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

    // 返回
    $("#btn2").click(
    function () {
        if (history.length > 1)
            history.back();
        else
            window.location = '/Account';
    });

    // 查詢
    $("#btn3").click(function () {
        var $btn = $("#btn3");

        $btn.button("loading");
        window.vm.loadHistory(1, 10, $("#search").val(), !$("#checkAll").is(':checked'), $("#selOptionsAreas").val(), "", "");
        $btn.button("reset");
    });
});
