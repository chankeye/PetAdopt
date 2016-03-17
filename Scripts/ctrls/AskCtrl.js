var MyViewModel = function () {
    var self = this;

    self.loading = ko.observable(false);
    self.responseMessage = ko.observable($.commonLocalization.noRecord);
    self.history = ko.observableArray();

    self.classes = ko.observableArray();

    self.detail = function (ask) {
        window.location = "/Ask/Detail?id=" + ask.Id;
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
            url: '/Ask/GetAskList',
            data: {
                page: page,
                take: take,
                query: query,
                isLike: isLike,
                areaId: areaId,
                classId: classId,
                statusId: statusId
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

    (function ($) {
        var origAppend = $.fn.append;

        $.fn.append = function () {
            return origAppend.apply(this, arguments).trigger("append");
        };
    })(jQuery);

    // 取得分類列表
    $("#optionsClasses").bind("append", window.utils.getClassList);
    $("#optionsClasses").append(window.utils.optionsClasses);

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

    // 查詢
    $("#btn3").click(function () {
        var $btn = $("#btn3");

        $btn.button("loading");
        window.vm.loadHistory(1, 10, $("#search").val(), !$("#checkAll").is(':checked'), "", $("#selOptionsClasses").val(), "");
        $btn.button("reset");
    });
});
