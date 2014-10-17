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
            url: '/Shelters/GetMessageList',
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
                $("#selOptionsAreas").text(data.ReturnObject.AreaId);
                $("#introduction").html(data.ReturnObject.Introduction);
                $("#address").text(data.ReturnObject.Address);
                $("#phone").text(data.ReturnObject.Phone);
                $("#source").text(data.ReturnObject.url);
            } else {
                alert(data.ErrorMessage);
                window.location = '/Shelters';
            }
        }
    });

    window.vm = new MyViewModel();
    window.vm.loadHistory();
    ko.applyBindings(window.vm);

    // 返回
    $("#btn2").click(
    function () {
        window.location = '/Shelters';
    });
});
