function MyViewModel() {
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
            url: '/Ask/GetMessageList',
            data: {
                id: window.id,
                page: page,
                take: take
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
};

$(function () {

    // 取得分類列表
    window.utils.getClassList();

    // 沒有輸入id直接導回
    window.id = window.utils.urlParams("id");
    if (window.id == null)
        window.location = '/Ask';

    // 取得問與答
    $.ajax({
        type: 'post',
        url: '/Ask/DetailInit',
        data: {
            id: window.id
        },
        success: function (data) {
            if (data.IsSuccess) {
                $("#title").text(data.ReturnObject.Title);
                $("#selOptionsClasses").text(data.ReturnObject.Class);
                $("#content").html(data.ReturnObject.Message.replace(/\n/g, "<br>"));
                var date = new Date(data.ReturnObject.Date);
                $("#date").text(date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate());
                $("#userDisplay").text(data.ReturnObject.UserDisplay);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Ask';
            }
        }
    });

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm, $("#mainContiner")[0]);

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
                url: '/Ask/AddMessage',
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

    // 返回
    $("#btn2").click(
    function () {
        if (history.length > 1)
            history.back();
        else
            window.location = '/Ask';
    });
});
