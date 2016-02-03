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
            url: '/Animal/GetMessageList',
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
    if (window.id == null) {
        if (history.length > 1)
            history.back();
        else
            window.location = '/Animal';
    }

    // 取得動物資訊
    var photo;
    $.ajax({
        type: 'post',
        url: '/Animal/DetailInit',
        data: {
            id: window.id
        },
        success: function (data) {
            if (data.IsSuccess) {
                photo = data.ReturnObject.Photo;
                if (photo != null) {
                    if (photo.slice(0, "http://".length) == "http://" || photo.slice(0, "https://".length) == "https://")
                        $('#coverPhoto').attr('src', photo);
                    else
                        $('#coverPhoto').attr('src', "../../Content/uploads/" + photo);
                }
                $("#title").text(data.ReturnObject.Title);
                $("#startDate").text(data.ReturnObject.StartDate);
                $("#endDate").text(data.ReturnObject.EndDate);
                $("#introduction").html(data.ReturnObject.Introduction.replace(/\n/g, "<br>"));
                if (data.ReturnObject.Shelters == null) {
                    $("#phone").text(data.ReturnObject.Phone);
                    $("#address").text(data.ReturnObject.Address);
                    $("#selOptionsAreas").text(data.ReturnObject.Area);
                    $("#sheltersLab").hide();
                } else {
                    $("#areaLab").hide();
                    $("#addressLab").hide();
                    $("#phoneLab").hide();
                    $("#shelters").text(data.ReturnObject.Shelters);
                    $("#shelters").attr("href", "/Shelters/Detail?id=" + data.ReturnObject.SheltersId);
                }
                $("#selOptionsClasses").text(data.ReturnObject.Class);
                $("#selOptionsStatuses").text(data.ReturnObject.Status);
                $("#age").text(data.ReturnObject.Age);
                $("#date").text(data.ReturnObject.Date);
                $("#userDisplay").text(data.ReturnObject.UserDisplay);
            } else {
                alert(data.ErrorMessage);
                if (history.length > 1)
                    history.back();
                else
                    window.location = '/Animal';
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
                url: '/Animal/AddMessage',
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
            window.location = '/Animal';
    });
});
