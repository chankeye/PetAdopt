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
    if (window.id == null)
        window.location = '/Animal';

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
                    $('#coverPhoto').attr('src', "../../Content/uploads/" + photo);
                }
                $("#title").text(data.ReturnObject.Title);
                $("#startDate").text(data.ReturnObject.StartDate),
                $("#endDate").text(data.ReturnObject.EndDate),
                $("#introduction").text(data.ReturnObject.Introduction),
                $("#shelters").text(data.ReturnObject.SheltersId),
                $("#phone").text(data.ReturnObject.Phone),
                $("#address").text(data.ReturnObject.Address),
                $("#selOptionsAreas").text(data.ReturnObject.Address),
                $("#selOptionsClasses").text(data.ReturnObject.Address),
                $("#selOptionsStatuses").text(data.ReturnObject.Address),
                $("#age").text(data.ReturnObject.Age);
                $("#address").text(data.ReturnObject.Address);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Animal';
            }
        }
    });

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);

    // 返回
    $("#btn2").click(
    function () {
        window.location = '/Animal';
    });
});
