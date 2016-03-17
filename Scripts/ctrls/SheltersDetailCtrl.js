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
            url: '/Shelters/GetMessageList',
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

    // 取得地區列表
    window.utils.getAreaList();

    // 沒有輸入id直接導回
    window.id = window.utils.urlParams("id");
    if (window.id == null)
        window.location = '/Shelters';

    // 取得收容所資訊
    var photo;
    $.ajax({
        type: 'post',
        url: '/Shelters/DetailInit',
        data: {
            id: window.id
        },
        success: function (data) {
            if (data.IsSuccess) {
                photo = data.ReturnObject.Photo;
                if (photo != null) {
                    $('#coverPhoto').attr('src', "../../Content/uploads/" + photo);
                }
                $("#name").text(data.ReturnObject.Name);
                $("#selOptionsAreas").text(data.ReturnObject.Area);
                $("#introduction").html(data.ReturnObject.Introduction.replace(/\n/g, "<br>"));
                $("#address").text(data.ReturnObject.Address);
                $("#phone").text(data.ReturnObject.Phone);
                if (data.ReturnObject.Url != null) {
                    $("#source").attr('href', data.ReturnObject.Url);
                } else {
                    $("#source").hide();
                }
                var date = new Date(data.ReturnObject.Date);
                $("#date").text(date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate());
                $("#userDisplay").text(data.ReturnObject.UserDisplay);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Shelters';
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
                url: '/Shelters/AddMessage',
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
            window.location = '/Shelters';
    });
});
